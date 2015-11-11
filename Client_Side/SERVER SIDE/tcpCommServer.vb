Imports System
Imports System.Threading
Imports System.Net
Imports System.Net.Sockets
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Collections.Concurrent

Public Class TcpCommServer
    Public errMsg As String

    ' Define the callback delegate type
    Public Delegate Sub ServerCallbackDelegate(ByVal bytes() As Byte, ByVal sessionID As Int32, ByVal dataChannel As Byte)
    Private Delegate Sub SendQueueDelegate(ByVal bytes() As Byte, ByVal dataChannel As Byte, ByVal sessionID As Int32)

    ' Create Delegate object
    Public ServerCallbackObject As ServerCallbackDelegate
    Private SendCallback As SendQueueDelegate

    Private Listener As TcpListener
    Private continue_running As Boolean = False
    Private blockSize As UInt16
    Private Port As Integer
    Private localAddr As IPAddress
    Private Mbps As UInt32
    Public IsRunning As Boolean = False

    Private receiveBuffer As BytesReceivedQueue
    Private sendBuffer As BytesSentQueue

    Public Class clsTimingController
        Public Delegate Function EvaluateExitCondition() As Boolean

        Public Function Wait(ByVal duration As TimeSpan, ByRef exitEarlyCondition1 As Boolean, ByRef exitEarlyCondition2 As Boolean,
                             Optional ByVal exitWhenConditionIs As Boolean = True,
                             Optional ByVal exitEarlyOnlyWhenBothConditionsAreMet As Boolean = True,
                             Optional ByVal sleepWhileWaiting As Boolean = True) As Boolean

            Dim timeOut As Date = Now + duration
            Dim onTheUIThread As Boolean = DetectUIThread()

            While Now < timeOut

                If exitEarlyOnlyWhenBothConditionsAreMet then
                    If exitWhenConditionIs = exitEarlyCondition1 And
                       exitWhenConditionIs = exitEarlyCondition2 Then Return False
                Else
                    If exitWhenConditionIs = exitEarlyCondition1 Or
                       exitWhenConditionIs = exitEarlyCondition2 Then Return False
                End If

                If onTheUIThread Then
                    If sleepWhileWaiting Then System.Windows.Forms.Application.DoEvents()
                Else
                    If sleepWhileWaiting Then Threading.Thread.Sleep(1)
                End If

            End While

            Return True
        End Function

        Public Function Wait(ByVal duration As TimeSpan, ByRef exitCondition As EvaluateExitCondition,
                             Optional ByVal exitWhenConditionIs As Boolean = True,
                             Optional ByVal sleepWhileWaiting As Boolean = True) As Boolean

            Dim timeOut As Date             = Now + duration
            Dim onTheUIThread As Boolean    = DetectUIThread()
            Dim exitEarly As Boolean        = False

            While Now < timeOut

                If exitCondition() Then Return False

                If onTheUIThread Then
                    If sleepWhileWaiting Then System.Windows.Forms.Application.DoEvents()
                Else
                    If sleepWhileWaiting Then Threading.Thread.Sleep(1)
                End If

            End While

            Return True
        End Function

        Public Function DetectUIThread() As Boolean
            Return System.Windows.Forms.Application.MessageLoop
        End Function
End Class


    Private Class BytesSentQueue
        Private Class message
            Public bytes() As Byte
            Public dataChannel As Byte
            Public sessionID As Int32
        End Class

        Private spinning As Boolean
        Private addedMore As Boolean
        Private throttle As Boolean
        Private paused As Boolean
        Private queueSize As Int32
        Private running As Boolean
        Private sendQueue As New ConcurrentQueue(Of message)
        Private thisTcpCommServer As TcpCommServer
        Private timingControl As New clsTimingController

        Sub New(ByRef _tcpCommServer As TcpCommServer, Optional ByVal _queueSize As Int32 = 100)
            thisTcpCommServer   = _tcpCommServer
            queueSize           = _queueSize
            spinning            = False
            addedMore           = False
            running             = True
            paused              = False
            throttle            = False

            Dim st As New Thread(AddressOf sendThrottle)
            st.Start
        End Sub

        Public Sub Close()
            running = False
        End Sub

        Public Sub PauseSending()
            paused = True
        End Sub

        Public Sub ResumeSending()
            paused = False
        End Sub

        Public Function IsPaused() As Boolean
            Return paused
        End Function

        Private Sub sendThrottle()

            While running
                If sendQueue.Count > queueSize then
                    throttle = True
                Else 
                    throttle = False
                End If

                Thread.Sleep(20)
            End While

            throttle = False
        End Sub

        Public Sub AddToQueue(ByVal bytes() As Byte, ByVal dataChannel As Byte, ByRef sessionID As Int32)

            ' Throttle the send queue. If we don't do this, then
            ' The user may accidently use up all the available memory
            ' queueing sends while waiting for the machine on the other 
            ' end to process the data sent.
            'If throttle then timingControl.Wait(New TimeSpan(0, 0, 0, 60), throttle, throttle, False, False)

            ' Did we dump out of the loop because we're being closed?
            If Not running then Exit Sub

            Dim msg As New message
            Dim theseBytes(bytes.Length - 1) As Byte

            Array.Copy(bytes, theseBytes, bytes.Length)

            msg.dataChannel = dataChannel
            msg.sessionId = sessionID
            msg.bytes = theseBytes

            sendQueue.Enqueue(msg)

            If Not spinning Then
                Dim bgThread As New Thread(AddressOf DoSafeSend)
                bgThread.Name = "TcpServerSafeSendBackgroundThread"
                bgThread.Start()
                spinning = True
                addedMore = False
            Else
                addedMore = True
            End If
        End Sub

        Private Sub DoSafeSend()

            If paused and running then
                Dim timout As Date = Now.AddMilliseconds(200)
                While paused and running
                    If timout < Now then Thread.Sleep(0)
                End While
            End If

            Dim msg As message = Nothing

            While spinning
                If sendQueue.TryPeek(msg) Then
                    sendQueue.TryDequeue(msg)
                    thisTcpCommServer.SendBytes(msg.bytes, msg.dataChannel, msg.sessionID)

                    msg = Nothing
                Else
                    Dim timer As Date = Now
                    While timer.AddMilliseconds(200) < Now
                        If sendQueue.TryPeek(msg) Then Exit While
                    End While

                    If Not sendQueue.TryPeek(msg) Then
                        spinning = False
                        Exit Sub
                    End If
                End If
            End While
        End Sub
    End Class

    Private Class BytesReceivedQueue
        Private Class message
            Public bytes() As Byte
            Public dataChannel As Byte
            Public sessionId As Int32
        End Class

        Private spinning As Boolean = False
        Private addedMore As Boolean = False
        Private queueSize As Int32
        Private receiveQueue As New ConcurrentQueue(Of message)
        Private CallbackObject As ServerCallbackDelegate

        Sub New(ByRef _CallbackObject As ServerCallbackDelegate, Optional ByVal _queueSize As Int32 = 100)
            CallbackObject  = _CallbackObject
            queueSize       = _queueSize
        End Sub

        Public Sub AddToQueue(ByVal bytes() As Byte, ByVal dataChannel As Byte, ByRef session As SessionCommunications)

            Dim msg As New message
            Dim theseBytes(bytes.Length - 1) As Byte

            Array.Copy(bytes, theseBytes, bytes.Length)

            msg.dataChannel = dataChannel
            msg.sessionId = session.sessionID
            msg.bytes = theseBytes

            receiveQueue.Enqueue(msg)

            If Not spinning Then
                Dim bgThread As New Thread(AddressOf DoSafeReceive)
                bgThread.Start(session)
                spinning = True
                addedMore = False
            Else
                addedMore = True
            End If
        End Sub

        Private Sub DoSafeReceive(ByVal _session As Object)

            Dim msg As message = Nothing
            Dim session As SessionCommunications = CType(_session, SessionCommunications)

            While spinning
                If receiveQueue.TryPeek(msg) Then
                    receiveQueue.TryDequeue(msg)
                    CallbackObject(msg.bytes, msg.sessionId, msg.dataChannel)

                    msg = Nothing
                Else
                    Dim timer As Date = Now
                    While timer.AddMilliseconds(200) < Now
                        If receiveQueue.TryPeek(msg) Then Exit While
                    End While

                    If Not receiveQueue.TryPeek(msg) Then
                        spinning = False
                        Exit Sub
                    End If
                End If
            End While
        End Sub
    End Class

    Public SessionCollection As New ArrayList

    Private Class SessionCommunications
        Public UserBytesToBeSentAvailable As Boolean = False
        Public UserBytesToBeSent As New MemoryStream
        Public UserOutputChannel As Byte
        Public SystemBytesToBeSentAvailable As Boolean = False
        Public SystemBytesToBeSent() As Byte
        Public SystemOutputChannel As Byte
        Public theClient As TcpClient
        Public IsRunning As Boolean = False
        Public remoteIpAddress As System.Net.IPAddress
        Public bytesRecieved() As Byte
        Public sessionID As Int32
        Public disConnect As Boolean = False
        Public bytesSentThisSecond As Int32 = 0
        Public bytesRecievedThisSecond As Int32 = 0
        Public fileBytesRecieved As Int64 = 0
        Public filebytesSent As Int64 = 0
        Public SendingFile As Boolean = False
        Public FileBeingSentPath As String
        Public IncomingFileSize As Int64
        Public IncomingFileName As String
        Public ReceivingFile As Boolean = False
        Public sendPacketSize As Boolean = False
        Public fileReader As FileStream
        Public fileWriter As clsAsyncUnbuffWriter
        Public ReceivedFilesFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\ServerReceivedFiles"
        Public userName As String
        Public password As String
        Public paused As Boolean
        Public pauseSent As Boolean

        Public Sub New(ByVal _theClient As TcpClient, ByVal _sessionID As Int32)
            theClient   = _theClient
            sessionID   = _sessionID
            paused      = False
            pauseSent   = False
        End Sub

        Public Sub Close()
            disConnect = True

            Try
                theClient.Client.Blocking = False
                theClient.Client.Close()
            Catch ex As Exception
                IsRunning = False
            End Try
        End Sub
    End Class

    Private Function StrToByteArray(ByVal text As String) As Byte()
        Dim encoding As New System.Text.UTF8Encoding()
        StrToByteArray = encoding.GetBytes(text)
    End Function

    Private Function BytesToString(ByVal data() As Byte) As String
        Dim enc As New System.Text.UTF8Encoding()
        BytesToString = enc.GetString(data)
    End Function

    ' CallbackForm must implement an UpdateUI Sub.
    Public Sub New(ByVal callbackMethod As ServerCallbackDelegate, Optional ByVal _throttledBytesPerSecond As UInt32 = 9000000)

        Mbps = _throttledBytesPerSecond

        'receiveBuffer = New BytesReceivedQueue(callbackMethod)
        'sendBuffer = New BytesSentQueue(me)

        ' BlockSize should be 62500 or 63100, depending on requested speed. 
        ' Excellent performance, and works great with throttling.
        Dim _blockSize As UInt16

        ' Get corrected blocksize for throttling.
        If Mbps < 300000 Then
            If Mbps > 16000 Then
                blockSize = 4000
            Else
                blockSize = CUShort((Mbps / 4))
            End If
        ElseIf Mbps > 300000 And Mbps < 500000 Then
            blockSize = 16000
        ElseIf Mbps > 500000 And Mbps < 1000000 Then
            blockSize = 32000
        Else
            Dim count As UInt32 = 0
            Dim aFourth As Decimal = 0

            If Mbps > 25000000 Then
                _blockSize = 63100
            Else
                _blockSize = 62500
            End If

            aFourth = CDec(Mbps / 4)

            Do
                count += _blockSize
                If (count + _blockSize) > aFourth Then
                    Mbps = CUInt(count * 4)
                    blockSize = _blockSize
                    Exit Do
                End If
            Loop

        End If

        ' Initialize the delegate object to point to the user's callback method.
        ServerCallbackObject = callbackMethod

    End Sub

    Public Sub ThrottleNetworkBps(ByVal bytesPerSecond As UInteger)
        ' Default value is 9000000 Mbps. Ok throughput, and 
        ' good performance for the server (low CPU usage).
        Mbps = bytesPerSecond
    End Sub

    Public Sub Start(ByVal prt As Integer)
        receiveBuffer = New BytesReceivedQueue(ServerCallbackObject)
        sendBuffer = New BytesSentQueue(me)

        Port = prt
        localAddr = GetLocalIpAddress()
        continue_running = True
        IsRunning = True

        Dim listenerThread As New Thread(AddressOf theListener)
        listenerThread.Name = "Server Listener Thread"
        listenerThread.Start()
    End Sub

    Public Sub StopRunning()
        Dim theresSillOneRunning As Boolean = True
        continue_running = False

        While theresSillOneRunning
            Try
                For Each item As SessionCommunications In SessionCollection
                    item.Close()
                Next
            Catch ex As Exception
            End Try

            Try
                For Each item As SessionCommunications In SessionCollection
                    If item.IsRunning Then Exit Try
                Next
                theresSillOneRunning = False
            Catch ex As Exception
            End Try

        End While

        Try
            Listener.Stop()
        Catch ex As Exception
        End Try

        IsRunning = False

        sendBuffer.Close
    End Sub

    Private Function GetLocalIpAddress() As System.Net.IPAddress
        Dim strHostName As String
        Dim addresses() As System.Net.IPAddress

        strHostName = System.Net.Dns.GetHostName()
        addresses = System.Net.Dns.GetHostAddresses(strHostName)

        ' Find an IpV4 address
        For Each address As System.Net.IPAddress In addresses
            ' Return the first IpV4 IP Address we find in the list.
            If address.AddressFamily = AddressFamily.InterNetwork then
                Return address
            End If
        Next

        ' No IpV4 address? Return the loopback address.
        Return System.Net.IPAddress.Loopback
    End Function

    Public Function GetBlocksize() As UInt16
        Return blockSize
    End Function

    Public Function GetFile(ByVal _path As String, ByVal sessionID As Int32) As Boolean
        Dim foundSession As Boolean = False
        GetFile = True

        ' Find the session we want to talk to and send it a Get File Request
        For Each session As SessionCommunications In SessionCollection
            If session.sessionID = sessionID Then
                ' we found it.
                foundSession = True
                Do
                    If Not session.UserBytesToBeSentAvailable Then
                        SyncLock session.UserBytesToBeSent
                            session.UserBytesToBeSent.Close()
                            session.UserBytesToBeSent = Nothing
                            session.UserBytesToBeSent = New MemoryStream(StrToByteArray("GFR:" & _path))
                            session.UserOutputChannel = 254 ' Text messages / commands on channel 254
                            session.UserBytesToBeSentAvailable = True
                        End SyncLock
                        Exit Do
                    End If

                    If Not session.IsRunning Then Exit Do
                    Thread.Sleep(0)
                Loop
            End If
        Next

        If Not foundSession Then Return False
    End Function

    Public Function SendFile(ByVal _path As String, ByVal sessionID As Int32) As Boolean
        Dim foundSession As Boolean = False
        SendFile = True

        ' Find the session we want to talk to and send it a Send File Request
        For Each session As SessionCommunications In SessionCollection
            If session.sessionID = sessionID Then
                ' we found it.
                foundSession = True
                Do
                    If Not session.UserBytesToBeSentAvailable Then
                        SyncLock session.UserBytesToBeSent
                            session.UserBytesToBeSent.Close()
                            session.UserBytesToBeSent = Nothing
                            session.UserBytesToBeSent = New MemoryStream(StrToByteArray("SFR:" & _path))
                            session.UserOutputChannel = 254 ' Text messages / commands on channel 254
                            session.UserBytesToBeSentAvailable = True
                        End SyncLock
                        Exit Do
                    End If

                    If Not session.IsRunning Then Exit Do
                    Thread.Sleep(0)
                Loop
            End If
        Next

        If Not foundSession Then Return False
    End Function

    Private Function SendTheBytes(ByVal bytes() As Byte, ByVal channel As Byte, ByVal sessionID As Int32) As Boolean

        Dim foundSession As Boolean = False
        SendTheBytes = True

        If channel = 0 Or channel > 250 Then
            MsgBox("Data can not be sent using channel numbers less then 1 or greater then 250.", MsgBoxStyle.Critical, "TCP_Server")
            Exit Function
        End If

        If sessionID > -1 Then
            ' Find the session we want to talk to and send it the message
            For Each session As SessionCommunications In SessionCollection
                If session.sessionID = sessionID Then
                    ' we found it.
                    foundSession = True
                    Do
                        If Not session.UserBytesToBeSentAvailable Then
                            SyncLock session.UserBytesToBeSent
                                session.UserBytesToBeSent.Close()
                                session.UserBytesToBeSent = Nothing
                                session.UserBytesToBeSent = New MemoryStream(bytes)
                                session.UserOutputChannel = channel
                                session.UserBytesToBeSentAvailable = True
                            End SyncLock
                            Exit Do
                        End If

                        If Not session.IsRunning Then Exit Do
                        Thread.Sleep(0)
                    Loop
                End If
            Next

            If Not foundSession Then Return False
        ElseIf sessionID = -1 Then
            ' Send our message to everyone connected
            For Each session As SessionCommunications In SessionCollection
                If session.IsRunning Then
                    Do
                        If Not session.UserBytesToBeSentAvailable Then
                            SyncLock session.UserBytesToBeSent
                                session.UserBytesToBeSent.Close()
                                session.UserBytesToBeSent = Nothing
                                session.UserBytesToBeSent = New MemoryStream(bytes)
                                session.UserOutputChannel = channel
                                session.UserBytesToBeSentAvailable = True
                            End SyncLock
                            Exit Do
                        End If

                        If Not session.IsRunning Then Exit Do
                        Thread.Sleep(0)
                    Loop
                End If
            Next

        Else
            Return False
        End If

    End Function

    Public Function SendBytes(ByVal bytes() As Byte, Optional ByVal channel As Byte = 1, Optional ByVal sessionID As Int32 = -1) As Boolean
        'If Thread.CurrentThread.Name = "TcpServerSafeSendBackgroundThread" then
            SendTheBytes(bytes, channel, sessionID)
        'Else
        '    sendBuffer.AddToQueue(bytes, channel, sessionID)
        'End If
    End Function

    Private Function RcvBytes(ByVal data() As Byte, ByRef session As SessionCommunications, Optional ByVal dataChannel As Byte = 1) As Boolean
        ' dataType: >0 = data channel, > 250 = internal messages. 0 is an invalid channel number (it's the puck)

        If dataChannel < 1 Then
            RcvBytes = False
            Exit Function
        End If

        Try
            ' Check to see if our app is closing
            If Not continue_running Then Exit Function

            receiveBuffer.AddToQueue(data, dataChannel, session)
        Catch ex As Exception
            RcvBytes = False

            ' An unexpected error.
            Debug.WriteLine("Unexpected error in server\RcvBytes: " & ex.Message)
        End Try
    End Function

    Private Function SendExternalSystemMessage(ByVal message As String, ByVal session As SessionCommunications) As Boolean

        session.SystemBytesToBeSent = StrToByteArray(message)
        session.SystemOutputChannel = 254 ' Text messages / commands on channel 254
        session.SystemBytesToBeSentAvailable = True

    End Function

    Private Function CheckSessionPermissions(ByVal session As SessionCommunications, ByVal cmd As String) As Boolean
        ' Your security code here...

        Return True
    End Function

    Private Function BeginFileSend(ByVal _path As String, ByVal session As SessionCommunications, ByVal fileLength As Long) As Boolean

        Try

            session.fileReader = New FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.None, clsAsyncUnbuffWriter.GetPageSize)
            session.SendingFile = True
            BeginFileSend = True

        Catch ex As Exception
            BeginFileSend = False
            _path = ex.Message
            session.SendingFile = False
        End Try

        Try
            If Not BeginFileSend Then session.fileReader.Close()
        Catch ex As Exception
        End Try

    End Function

    Private Sub GetMoreFileBytesIfAvailable(ByVal session As SessionCommunications)
        Dim bytesRead As Int32 = 0

        If session.SendingFile And Not session.SystemBytesToBeSentAvailable Then
            Try
                If session.SystemBytesToBeSent.Length <> blockSize Then ReDim session.SystemBytesToBeSent(blockSize - 1)
                bytesRead = session.fileReader.Read(session.SystemBytesToBeSent, 0, blockSize)
                If bytesRead <> blockSize Then ReDim Preserve session.SystemBytesToBeSent(bytesRead - 1)

                If bytesRead > 0 Then
                    session.SystemOutputChannel = 253 ' File transfer from server to client
                    session.SystemBytesToBeSentAvailable = True
                Else

                    ReDim session.SystemBytesToBeSent(blockSize - 1)
                    SendExternalSystemMessage("->Done", session) ' Send the client a completion notice.
                    session.SendingFile = False

                    ' Clean up
                    session.fileReader.Close()
                    session.fileReader = Nothing
                    GC.GetTotalMemory(True)
                End If
            Catch ex As Exception
                SendExternalSystemMessage("ERR: " & ex.Message, session)

                ' We're finished.
                ReDim session.SystemBytesToBeSent(blockSize - 1)
                session.SendingFile = False
                session.fileReader.Close()
            End Try
        End If

    End Sub

    Private Function GetFilenameFromPath(ByRef filePath As String) As String
        Dim filePathParts() As String

        If filePath.Trim = "" Then Return ""

        Try
            filePathParts = Split(filePath, "\")
            GetFilenameFromPath = filePathParts(filePathParts.Length - 1)
        Catch ex As Exception
            filePath = ex.Message
            Return ""
        End Try

    End Function

    Private Function CreateFolders(ByVal _path As String) As Boolean
        CreateFolders = True

        Dim parts() As String
        Dim path As String = ""
        Dim count As Int32
        parts = Split(_path, "\")

        path = parts(0)
        For count = 1 To parts.Length - 2
            path += "\" & parts(count)
            Try
                If Not Directory.Exists(path) Then
                    Directory.CreateDirectory(path)
                End If
            Catch ex As Exception
            End Try
        Next

    End Function

    Private Function BeginToReceiveAFile(ByVal _path As String, ByVal session As SessionCommunications) As Boolean
        Dim readBuffer As Int32 = 0
        session.ReceivingFile = True
        BeginToReceiveAFile = True
        session.fileBytesRecieved = 0

        Try

            CreateFolders(_path) ' Just a 256k write buffer for the server. Let's try to avoid memory problems...
            session.fileWriter = New clsAsyncUnbuffWriter(_path, True, 1024 * 256, session.IncomingFileSize)

        Catch ex As Exception
            _path = ex.Message
            session.ReceivingFile = False
        End Try

        If Not session.ReceivingFile Then
            Try
                session.fileWriter.Close()
            Catch ex As Exception
            End Try
            Return False
        End If
    End Function

    Private Function HandleIncomingFileBytes(ByRef bytes() As Byte, ByVal session As SessionCommunications) As Boolean

        Try
            session.fileWriter.Write(bytes, bytes.Length)
            HandleIncomingFileBytes = True
        Catch ex As Exception
            HandleIncomingFileBytes = False
        End Try

    End Function

    Private Sub FinishReceivingTheFile(ByVal session As SessionCommunications)
        Try
            session.fileWriter.Close()
            session.fileWriter = Nothing
            session.ReceivingFile = False
        Catch ex As Exception
            session.ReceivingFile = False
        End Try
    End Sub

    Private Sub HandleIncomingSystemMessages(ByVal bytes() As Byte, ByVal channel As Byte, ByVal session As SessionCommunications)

        If channel = 254 Then ' Text commands / messages passed between server and client
            Dim message As String = BytesToString(bytes)
            Dim filePath As String
            Dim tmp As String = ""

            ' Get File Request: The client wants us to send them a file.
            If message.Length > 4 Then tmp = message.Substring(0, 4)
            If tmp = "GFR:" Then
                ' Get file path...
                filePath = message.Substring(4, message.Length - 4)

                ' Does it exist?
                If File.Exists(filePath) Then
                    ' Do they have permission to get this file?
                    If CheckSessionPermissions(session, "GFR") Then
                        ' Are we already busy sending them a file?
                        If Not session.SendingFile Then
                            Dim _theFilesInfo As New FileInfo(filePath)
                            If BeginFileSend(filePath, session, _theFilesInfo.Length) Then
                                ' Send only the file NAME. It will have a different path on the other side.
                                SendExternalSystemMessage("Sending:" & GetFilenameFromPath(filePath) & _
                                                          ":" & _theFilesInfo.Length, session)
                            Else
                                ' FilePath contains the error message.
                                SendExternalSystemMessage("ERR: " & filePath, session)
                            End If
                        Else
                            ' There's already a GFR in progress.
                            SendExternalSystemMessage("ERR: File: ''" & _
                                                      session.FileBeingSentPath & _
                                                      "'' is still in progress. Only one file " & _
                                                      "may be transfered (from server to client) at a time.", session)
                        End If
                    Else
                        ' This user doesn't have rights to "get" this file. Send an error.
                        SendExternalSystemMessage("ERR: You do not have permission to receive files. Access Denied.", session)
                    End If
                Else
                    ' File doesn't exist. Send an error.
                    SendExternalSystemMessage("ERR: The requested file can not be found by the server.", session)
                End If
            End If

            ' We're being informed that we will be receiving a file:
            If message.Length > 7 Then tmp = message.Substring(0, 8)
            If tmp = "Sending:" Then
                ' Strip away the headder...
                Dim msgParts() As String = Split(message, ":")
                session.IncomingFileSize = Convert.ToInt64(msgParts(2))
                session.IncomingFileName = msgParts(1)
                tmp = session.ReceivedFilesFolder & "\" & session.IncomingFileName
                SystemMessage("Receiving file: " & session.IncomingFileName)
                If Not BeginToReceiveAFile(tmp, session) Then
                    SystemMessage("ERR: " & tmp)
                    SendExternalSystemMessage("Abort->", session)
                End If
            End If

            If message = "<-Done" Then
                FinishReceivingTheFile(session)
                SystemMessage("<-Done")
            End If

            ' We've been notified that no file data will be forthcoming.
            If message = "Abort<-" Then
                WrapUpIncomingFile(session)
                SystemMessage("<-Aborted.")
                SendExternalSystemMessage("<-Aborted.", session)
            End If

            ' Send File Request: The client wants to send us a file.
            If message.Length > 4 Then tmp = message.Substring(0, 4)
            If tmp = "SFR:" Then
                If CheckSessionPermissions(session, "SFR") Then
                    Dim parts() As String
                    parts = Split(message, "SFR:")
                    SendExternalSystemMessage("GFR:" & parts(1), session)
                Else
                    ' This user doesn't have rights to send us a file. Send an error.
                    SendExternalSystemMessage("ERR: You do not have permission to send files. Access Denied.", session)
                End If
            End If

            If message.Length > 4 Then tmp = message.Substring(0, 4)
            If tmp = "GDR:" Then ' Get Directory Request
                ' Send each file in the directory and all subdirectories.
                ' To be implemented in the future.
            End If

            If message.Length > 4 Then tmp = message.Substring(0, 4)
            If tmp = "ERR:" Then ' The client has sent us an error message.
                ' Pass it on up to the user.
                SystemMessage(message)
            End If

            ' New queue throttling code
            If message = "pause" Then
                session.paused = True
            End If

            If message = "resume" Then
                session.paused = False
            End If

            If message = "Abort->" Then
                Try
                    session.SendingFile = False
                    ReDim session.SystemBytesToBeSent(blockSize - 1)
                    SendExternalSystemMessage("->Aborted.", session)
                    SystemMessage("->Aborted.")
                    session.fileReader.Close()
                Catch ex As Exception
                End Try
            End If
        ElseIf channel = 253 Then ' File transfer from server to client

        ElseIf channel = 252 Then ' File transfer from client to server
            Try
                If session.ReceivingFile Then
                    HandleIncomingFileBytes(bytes, session)
                    session.fileBytesRecieved += bytes.Length
                End If
            Catch ex As Exception
            End Try
        ElseIf channel = 251 Then ' reserved.

        End If
    End Sub

    Private Function HandleOutgoingInternalSystemMessage(ByVal Stream As NetworkStream, _
                                                         ByVal session As SessionCommunications) As Boolean
        Dim tmp(1) As Byte
        Dim _size As UShort
        'Static OurTurn As Boolean = False
        HandleOutgoingInternalSystemMessage = False

        ' Create a one time outgoing system message to syncronize packet size.
        If Not session.sendPacketSize Then
            SendExternalSystemMessage("blocksize:" & blockSize.ToString, session)
            session.sendPacketSize = True
        End If

        GetMoreFileBytesIfAvailable(session)

        ' Handle outgoing system stuff here
        If session.SystemBytesToBeSentAvailable = True Then
            HandleOutgoingInternalSystemMessage = True
            If session.SystemBytesToBeSent.Length > blockSize Then
                ' Send Channel
                tmp(0) = session.SystemOutputChannel
                Stream.Write(tmp, 0, 1)

                ' Send packet size
                _size = blockSize
                tmp = BitConverter.GetBytes(_size)
                Stream.Write(tmp, 0, 2)

                ' Send packet
                Stream.Write(GetSome(session.SystemBytesToBeSent, blockSize, session.SystemBytesToBeSentAvailable, session), 0, _size)
                session.bytesSentThisSecond += 3 + blockSize
            Else
                ' Send Channel
                tmp(0) = session.SystemOutputChannel
                Stream.Write(tmp, 0, 1)

                ' Send packet size
                _size = convert.ToUInt16(session.SystemBytesToBeSent.Length)
                tmp = BitConverter.GetBytes(_size)
                Stream.Write(tmp, 0, 2)

                ' Send packet
                Stream.Write(session.SystemBytesToBeSent, 0, _size)
                session.bytesSentThisSecond += 3 + _size
                session.SystemBytesToBeSentAvailable = False
            End If
        End If

    End Function

    Private Function HandleOutgoingUserData(ByVal Stream As NetworkStream, ByVal session As SessionCommunications) As Boolean
        Dim tmp(1) As Byte
        Dim _size As UShort
        Dim notify As Boolean = False
        Static packet(0) As Byte

        If session.UserBytesToBeSentAvailable = True Then
            SyncLock session.UserBytesToBeSent
                Try
                    If (session.UserBytesToBeSent.Length - session.UserBytesToBeSent.Position) > blockSize Then
                        ' Send Channel
                        tmp(0) = session.UserOutputChannel
                        Stream.Write(tmp, 0, 1)

                        ' Send packet size
                        _size = blockSize
                        tmp = BitConverter.GetBytes(_size)
                        Stream.Write(tmp, 0, 2)

                        ' Send packet
                        If packet.Length <> _size Then ReDim packet(_size - 1)
                        session.UserBytesToBeSent.Read(packet, 0, _size)
                        'session.theClient.NoDelay = True
                        Stream.Write(packet, 0, _size)
                        session.bytesSentThisSecond += 3 + _size

                        ' Check to see if we've sent it all...
                        If session.UserBytesToBeSent.Length = session.UserBytesToBeSent.Position Then
                            session.UserBytesToBeSentAvailable = False
                            notify = True
                        End If
                    Else
                        ' Send Channel
                        tmp(0) = session.UserOutputChannel
                        Stream.Write(tmp, 0, 1)

                        ' Send packet size
                        _size = Convert.ToUInt16(session.UserBytesToBeSent.Length - session.UserBytesToBeSent.Position)
                        tmp = BitConverter.GetBytes(_size)
                        Stream.Write(tmp, 0, 2)

                        ' Send packet
                        If packet.Length <> _size Then ReDim packet(_size - 1)
                        session.UserBytesToBeSent.Read(packet, 0, _size)
                        'session.theClient.NoDelay = True
                        Stream.Write(packet, 0, _size)
                        session.bytesSentThisSecond += 3 + _size

                        session.UserBytesToBeSentAvailable = False
                        notify = True
                    End If
                Catch ex As Exception
                    ' Report error attempting to send user data.
                    Debug.WriteLine("Unexpected error in TcpCommServer\HandleOutgoingUserData: " & ex.Message)
                End Try
            End SyncLock

            ' Notify the user that the packet has been sent.
            If notify Then SystemMessage("UBS:" & session.sessionID & ":" & session.UserOutputChannel)

            Return True
        Else
            Return False
        End If

        'If session.UserBytesToBeSentAvailable = True Then
        '    If (session.UserBytesToBeSent.Length - session.UserBytesToBeSent.Position) > blockSize Then
        '        ' Send Channel
        '        tmp(0) = session.UserOutputChannel
        '        Stream.Write(tmp, 0, 1)

        '        ' Send packet size
        '        _size = blockSize
        '        tmp = BitConverter.GetBytes(_size)
        '        Stream.Write(tmp, 0, 2)

        '        ' Send packet
        '        If packet.Length <> _size Then ReDim packet(_size - 1)
        '        session.UserBytesToBeSent.Read(packet, 0, _size)
        '        Stream.Write(packet, 0, _size)
        '        session.bytesSentThisSecond += 3 + _size

        '        ' Check to see if we've sent it all...
        '        If session.UserBytesToBeSent.Length = session.UserBytesToBeSent.Position Then
        '            session.UserBytesToBeSentAvailable = False
        '            SystemMessage("UBS:" & session.sessionID & ":" & session.UserOutputChannel)
        '        End If
        '    Else
        '        ' Send Channel
        '        tmp(0) = session.UserOutputChannel
        '        Stream.Write(tmp, 0, 1)

        '        ' Send packet size
        '        _size = (session.UserBytesToBeSent.Length - session.UserBytesToBeSent.Position)
        '        tmp = BitConverter.GetBytes(_size)
        '        Stream.Write(tmp, 0, 2)

        '        ' Send packet
        '        If packet.Length <> _size Then ReDim packet(_size - 1)
        '        session.UserBytesToBeSent.Read(packet, 0, _size)
        '        Stream.Write(packet, 0, _size)
        '        session.bytesSentThisSecond += 3 + _size

        '        session.UserBytesToBeSentAvailable = False
        '        SystemMessage("UBS:" & session.sessionID & ":" & session.UserOutputChannel)
        '    End If

        '    Return True
        'Else
        '    Return False
        'End If

    End Function

    Private Function GetSome(ByRef bytes() As Byte, ByVal chunkToBreakOff As Integer, _
                             ByRef bytesToBeSentAvailable As Boolean, ByVal session As SessionCommunications, _
                             Optional ByVal theseAreUserBytes As Boolean = False) As Byte()

        Dim tmp(chunkToBreakOff - 1) As Byte
        Array.Copy(bytes, 0, tmp, 0, chunkToBreakOff)
        GetSome = tmp

        If bytes.Length = chunkToBreakOff Then
            bytesToBeSentAvailable = False
            If theseAreUserBytes Then SystemMessage("UBS:" & session.sessionID & ":" & session.UserOutputChannel)
        Else
            Dim tmp2(bytes.Length - chunkToBreakOff - 1) As Byte
            Array.Copy(bytes, chunkToBreakOff, tmp2, 0, bytes.Length - chunkToBreakOff)
            bytes = tmp2
        End If

    End Function

    Private Sub SystemMessage(ByVal MsgText As String)
        RcvBytes(StrToByteArray(MsgText), New SessionCommunications(New TcpClient, -1), 255)
    End Sub

    ' Check to see if our app is closing (set in FormClosing event)
    Private Function theServerIsStopping(ByVal Server As TcpClient, ByVal session As SessionCommunications) As Boolean

        Try
            If Not continue_running Or session.disConnect Then
                theServerIsStopping = True
            Else
                theServerIsStopping = False
            End If
        Catch ex As Exception
            ' An unexpected error.
            Debug.WriteLine("Unexpected error in server\theServerIsStopping: " & ex.Message)
        End Try

    End Function

    Private Sub theListener()

        ' Start listening
        SystemMessage("Listening...")
        Listener = New TcpListener(localAddr, Port)

        Listener.Start()
        StartAccept()

    End Sub

    Private Function StartAccept() As Boolean
        Try
            Listener.BeginAcceptTcpClient(AddressOf HandleAsyncConnection, Listener)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub HandleAsyncConnection(ByVal res As IAsyncResult)
        Static conID As Int32 = 0
        If Not StartAccept() Then Exit Sub

        conID += 1
        If conID > 2000000000 Then conID = 1 ' 2 billion connections before the ID cycles

        Dim client As TcpClient = Listener.EndAcceptTcpClient(res)
        SessionCollection.Insert(0, New SessionCommunications(client, conID))
        SystemMessage("Connected.")

        'ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf Run), SessionCollection.Item(0))
        Dim newSession As New Thread(AddressOf Run)
        newSession.IsBackground = True
        newSession.Name = "Server Session #" & conID
        newSession.Start(SessionCollection.Item(0))
    End Sub

    Private Sub WrapUpIncomingFile(ByVal session As SessionCommunications)

        If session.ReceivingFile Then
            Try
                session.fileWriter.Close()
                session.fileWriter = Nothing
                GC.GetTotalMemory(True)
            Catch ex As Exception
            End Try

            Try
                File.Delete(session.ReceivedFilesFolder & "\" & session.IncomingFileName)
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub Run(ByVal _session As Object)

        Dim session As SessionCommunications = DirectCast(_session, SessionCommunications)

        Dim Server As TcpClient
        Dim Stream As NetworkStream
        Dim IpEndPoint As IPEndPoint
        Dim puck(1) As Byte : puck(0) = 0
        Dim theBuffer(blockSize - 1) As Byte
        Dim tmp(1) As Byte
        Dim dataChannel As Byte = 0
        Dim packetSize As UShort = 0
        Dim idleTimer, bandwidthTimer As Date
        Dim bytesread As Integer = 0
        Dim weHaveThePuck As Boolean = True
        Dim bandwidthUsedThisSecond As Int32 = 0
        Dim userOrSystemSwitcher As Integer = 0

        Try
            ' Create a local Server and Stream objects for clarity.
            Server = session.theClient
            Stream = Server.GetStream()
        Catch ex As Exception
            ' An unexpected error.
            Debug.WriteLine("Could not create local Server or Stream object in server. Message: " & ex.Message)
            Exit Sub
        End Try

        Try

            ' Get the remote machine's IP address.
            IpEndPoint = CType(Server.Client.RemoteEndPoint, Net.IPEndPoint)
            session.remoteIpAddress = IpEndPoint.Address

            ' Set the send and receive buffers to the maximum
            ' size allowable in this application...
            Server.Client.ReceiveBufferSize = 65535
            Server.Client.SendBufferSize = 65535

            ' no delay on partially filled packets...
            ' Send it all as fast as possible.
            Server.NoDelay = True

            ' Set the timers...
            idleTimer = Now
            bandwidthTimer = Now

            session.IsRunning = True

            ' Start the communication loop
            Do
                ' Check to see if our app is shutting down.
                If theServerIsStopping(Server, session) Then Exit Do

                ' Throttle network Mbps...
                bandwidthUsedThisSecond = session.bytesSentThisSecond + session.bytesRecievedThisSecond
                If bandwidthTimer.AddMilliseconds(250) >= Now And bandwidthUsedThisSecond >= (Mbps / 4) Then
                    While bandwidthTimer.AddMilliseconds(250) > Now
                        Thread.Sleep(0)
                    End While
                End If
                If bandwidthTimer.AddMilliseconds(250) <= Now Then
                    bandwidthTimer = Now
                    session.bytesRecievedThisSecond = 0
                    session.bytesSentThisSecond = 0
                    bandwidthUsedThisSecond = 0
                End If

                ' Normal communications...
                If weHaveThePuck Then

                    ' Send data if there is any to be sent...
                    userOrSystemSwitcher += 1
                    Select Case userOrSystemSwitcher
                        Case 1
                            If Not session.paused Then
                                If HandleOutgoingUserData(Stream, session) Then idleTimer = Now
                            End If
                        Case 2
                            If HandleOutgoingInternalSystemMessage(Stream, session) Then idleTimer = Now
                    End Select
                    If userOrSystemSwitcher > 1 Then userOrSystemSwitcher = 0
                    
                    ' After sending out data, send the puck
                    Stream.Write(puck, 0, 1)
                    weHaveThePuck = False
                End If

                If theBuffer.Length < 2 Then ReDim theBuffer(1)

                ' Read in the control byte.
                Stream.Read(theBuffer, 0, 1)
                dataChannel = theBuffer(0)

                ' If it's just the puck (communictaion syncronization byte),
                ' set weHaveThePuck true, record the byte read for throttling,
                ' and that's all. dataChannel 0 is reserved for the puck.
                If dataChannel = 0 Then
                    weHaveThePuck = True
                    session.bytesRecievedThisSecond += 1
                Else
                    ' It's not the puck: It's an incoming packet.

                    ' Get the packet size:
                    tmp(0) = Convert.ToByte(Stream.ReadByte)
                    tmp(1) = Convert.ToByte(Stream.ReadByte)
                    packetSize = BitConverter.ToUInt16(tmp, 0)
                    session.bytesRecievedThisSecond += 2

                    ' Get the packet:
                    If theBuffer.Length <> packetSize Then ReDim theBuffer(packetSize - 1)
                    Do
                        ' Check to see if we're stopping...
                        If theServerIsStopping(Server, session) Then Exit Do
                        ' Read bytes in...
                        bytesread += Stream.Read(theBuffer, bytesread, (packetSize - bytesread))
                    Loop While bytesread < packetSize
                    bytesread = 0

                    ' Record bytes read for throttling...
                    session.bytesRecievedThisSecond += packetSize

                    ' Handle the packet...
                    If dataChannel > 250 Then
                        ' this is an internal system packet
                        If Not theServerIsStopping(Server, session) Then HandleIncomingSystemMessages(theBuffer, dataChannel, session)
                    Else
                        ' Hand user data off to the calling thread.
                        If Not theServerIsStopping(Server, session) Then RcvBytes(theBuffer, session, dataChannel)
                    End If

                    idleTimer = Now
                End If

                ' Throttle CPU usage when idle.
                If Now > idleTimer.AddMilliseconds(500) Then
                    Thread.Sleep(50)
                End If

            Loop

        Catch ex As Exception
            ' An unexpected error.
            Debug.WriteLine("Unexpected error in server: " & ex.Message)
        End Try

        Try
            session.fileReader.Close()
        Catch ex As Exception
        End Try

        Try
            Server.Client.Close()
            Server.Client.Blocking = False
        Catch ex As Exception
        End Try

        ' If we're in the middle of receiving a file,
        ' close the filestream, release the memory and
        ' delete the partial file.
        WrapUpIncomingFile(session)

        session.IsRunning = False
        SystemMessage("Session " & session.sessionID.ToString & " Stopped.")
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    ' Receive message queue stuff
    'Private Class message
    '    Public bytes() As Byte
    '    Public dataChannel As Integer
    '    Public sessionId As Int32
    'End Class
    
    'Private spinning As Boolean = False
    'Private addedMore As Boolean = False
    'Private receiveQueue As New ConcurrentQueue(Of message)
 
    'Private Sub AddToQueue(bytes() As Byte, dataChannel As Integer, ByRef session As SessionCommunications)

    '    ' Throttle receiveQueue - 1st half
    '    'If receiveQueue.Count > 99 and Not session.pauseSent then
    '    '    SendExternalSystemMessage("pause", session)
    '    '    session.pauseSent = True
    '    'End If

    '    Dim msg As New message 
    '    Dim theseBytes(bytes.Length - 1) As Byte

    '    Array.Copy(bytes, theseBytes, bytes.Length)

    '    msg.dataChannel = dataChannel
    '    msg.sessionId = session.sessionID
    '    msg.bytes = theseBytes

    '    receiveQueue.Enqueue(msg)
 
    '    If Not spinning then
    '        Dim bgThread As New Thread(AddressOf DoSafeReceive)
    '        bgThread.Start(session)
    '        spinning = True
    '        addedMore = False
    '    Else
    '        addedMore = True
    '    End If
    'End Sub
 
    'Private Sub DoSafeReceive(ByVal _session As Object)

    '    Dim msg As message = Nothing
    '    Dim session As SessionCommunications = CType(_session, SessionCommunications)

    '    While spinning
    '        If receiveQueue.TryPeek(msg) then
    '            receiveQueue.TryDequeue(msg)
    '            ServerCallbackObject(msg.bytes, msg.sessionId, msg.dataChannel)

    '            msg = Nothing

    '            ' Throttle receive queue - 2nd half
    '            'If receiveQueue.Count <= 3 And session.pauseSent Then
    '            '    SendExternalSystemMessage("resume", session)
    '            '    session.pauseSent = False
    '            'End If
    '        Else
    '            Dim timer As Date = Now
    '            While timer.AddMilliseconds(200) < Now
    '                If receiveQueue.TryPeek(msg) then Exit While
    '            End While

    '            If Not receiveQueue.TryPeek(msg) then 
    '                spinning = False
    '                Exit Sub
    '            End If
    '        End If
    '    End While
    'End Sub
End Class