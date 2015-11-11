Imports System.Threading
Imports System.Net
Imports System.Net.Sockets
Imports System.IO
Imports System.Collections.Concurrent

Public Class tcpCommClient
    Public errMsg As String

    ' Define the delegate type
    Public Delegate Sub ClientCallbackDelegate(ByVal bytes() As Byte, ByVal dataChannel As Byte)

    ' Create Delegate pointer
    Public ClientCallbackObject As ClientCallbackDelegate

    ' Monitor CPU usage
    'Private CPUutil As CpuMonitor

    Private continue_running As Boolean = False
    Private bytes() As Byte
    Private blockSize As UInt16
    Private IP As System.Net.IPAddress
    Private Port As Integer
    Private localAddr As IPAddress
    Private Client As TcpClient
    Private Stream As NetworkStream
    Private fileWriter As clsAsyncUnbuffWriter
    Private fileReader As FileStream
    Private FileBeingSentPath As String
    Private weHaveThePuck As Boolean = False
    Private isRunning As Boolean = False
    Private UserBytesToBeSentAvailable As Boolean = False
    Private UserBytesToBeSent As New MemoryStream
    Private UserOutputChannel As Byte
    Private SystemBytesToBeSentAvailable As Boolean = False
    Private SystemBytesToBeSent() As Byte
    Private SystemOutputChannel As Byte
    Private SendingFile As Boolean = False
    Private ReceivingFile As Boolean = False
    Private IncomingFileName As String
    Private IncomingFileSize As Int64 = 0
    Private outgoingFileSize As UInt64 = 0
    Private outgoingFileName As String
    Private fileBytesRecieved As Int64 = 0
    Private filebytesSent As Int64 = 0
    Private bytesSentThisSecond As Int32 = 0
    Private bytesReceivedThisSecond As Int32 = 0
    Private mbpsOneSecondAverage() As Int32
    Private ReceivedFilesFolder As String = Application.StartupPath & "\ReceivedFiles"
    Private userName As String
    Private password As String

    Private receiveBuffer As BytesReceivedQueue
    Private sendBuffer As BytesSentQueue

    Public Class clsTimingController
        Public Delegate Function EvaluateExitCondition() As Boolean

        Public Function Wait(ByVal duration As TimeSpan, ByRef exitEarlyCondition1 As Boolean, ByRef exitEarlyCondition2 As Boolean,
                             Optional ByVal exitWhenConditionIs As Boolean = True,
                             Optional ByVal exitEarlyOnlyWhenBothConditionsAreMet As Boolean = True,
                             Optional ByVal sleepWhileWaiting As Boolean = True) As Boolean

            Dim timeOut As Date             = Now + duration
            Dim onTheUIThread As Boolean    = DetectUIThread()

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
        End Class

        Private spinning As Boolean
        Private addedMore As Boolean
        Private throttle As Boolean
        Private paused As Boolean
        Private queueSize As Int32
        Private running As Boolean
        Private sendQueue As New ConcurrentQueue(Of message)
        Private thisTcpClient As tcpCommClient
        Private timingControl As New clsTimingController
        
        Sub New(ByRef _tcpCommClient As tcpCommClient, Optional ByVal _queueSize As Int32 = 100)
            thisTcpClient   = _tcpCommClient
            queueSize       = _queueSize
            spinning        = False
            addedMore       = False
            running         = True
            paused          = False
            throttle        = False

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

        Public Sub AddToQueue(ByVal bytes() As Byte, ByVal dataChannel As Byte)

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
            msg.bytes = theseBytes

            sendQueue.Enqueue(msg)

            If queueSize > 105 then MsgBox("Not working...", MsgBoxStyle.Information)

            If Not spinning Then
                Dim bgThread As New Thread(AddressOf DoSafeSend)
                bgThread.Name = "TcpClientSafeSendBackgroundThread"
                bgThread.Start()
                spinning = True
                addedMore = False
            Else
                addedMore = True
            End If
        End Sub

        Private Sub DoSafeSend()

            'If paused and running then
            '    Dim timout As Date = Now.AddMilliseconds(200)
            '    While paused and running
            '        If timout < Now then Thread.Sleep(0)
            '    End While
            'End If

            Dim msg As message = Nothing

            While spinning
                If sendQueue.TryPeek(msg) Then

                    sendQueue.TryDequeue(msg)
                    thisTcpClient.SendBytes(msg.bytes, msg.dataChannel)

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
        Private CallbackObject As ClientCallbackDelegate

        Sub New(ByRef _CallbackObject As ClientCallbackDelegate, Optional ByVal _queueSize As Int32 = 100)
            CallbackObject  = _CallbackObject
            queueSize       = _queueSize
        End Sub

        Public Sub AddToQueue(ByVal bytes() As Byte, ByVal dataChannel As Byte)

            Dim msg As New message
            Dim theseBytes(bytes.Length - 1) As Byte

            Array.Copy(bytes, theseBytes, bytes.Length)

            msg.dataChannel = dataChannel
            msg.bytes = theseBytes

            receiveQueue.Enqueue(msg)

            If Not spinning Then
                Dim bgThread As New Thread(AddressOf DoSafeReceive)
                bgThread.Start()
                spinning = True
                addedMore = False
            Else
                addedMore = True
            End If
        End Sub

        Private Sub DoSafeReceive()

            Dim msg As message = Nothing

            While spinning
                If receiveQueue.TryPeek(msg) Then
                    receiveQueue.TryDequeue(msg)
                    CallbackObject(msg.bytes, msg.dataChannel)

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

    Private mbpsSyncObject As New AutoResetEvent(False)

    Private Function StrToByteArray(ByVal text As String) As Byte()
        Dim encoding As New System.Text.UTF8Encoding()
        StrToByteArray = encoding.GetBytes(text)
    End Function

    Private Function BytesToString(ByVal data() As Byte) As String
        Dim enc As New System.Text.UTF8Encoding()
        BytesToString = enc.GetString(data)
    End Function

    Public Function isClientRunning() As Boolean
        Return isRunning
    End Function

    Public Function SetReceivedFilesFolder(ByVal _path As String) As Boolean
        ReceivedFilesFolder = _path
    End Function

    Public Function GetIncomingFileName() As String
        Return IncomingFileName
    End Function

    Public Function GetOutgoingFileName() As String
        Return outgoingFileName
    End Function

    Public Function GetPercentOfFileReceived() As UInt16
        If ReceivingFile Then
            Return CUShort((fileBytesRecieved / IncomingFileSize) * 100)
        Else
            Return 0
        End If
    End Function

    Public Function GetPercentOfFileSent() As UInt16
        If SendingFile Then
            Return CUShort((filebytesSent / outgoingFileSize) * 100)
        Else
            Return 0
        End If
    End Function

    Public Function GetMbps() As String
        Dim currentMbps As Decimal = CalculateMbps(True)
        If currentMbps > 1000000 Then
            Return (currentMbps / 1000000).ToString("N2") & " Mbps"
        Else
            Return (currentMbps / 1000).ToString("N2") & " Kbps"
        End If
    End Function

    Public Function GetLocalIpAddress() As System.Net.IPAddress
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

    Private Function GetIPFromHostname(ByVal hostname As String, Optional returnLoopbackOnFail As Boolean = True) As System.Net.IPAddress
        Dim addresses() As System.Net.IPAddress

        Try
            addresses = System.Net.Dns.GetHostAddresses(hostname)
        Catch ex As Exception
            If returnLoopbackOnFail then Return System.Net.IPAddress.Loopback
            Return Nothing
        End Try
        
        ' Find an IpV4 address
        For Each address As System.Net.IPAddress In addresses
            ' Return the first IpV4 IP Address we find in the list.
            If address.AddressFamily = AddressFamily.InterNetwork then
                Return address
            End If
        Next

        ' No IpV4 address? Return the loopback address.
        If returnLoopbackOnFail then Return System.Net.IPAddress.Loopback
        Return Nothing
    End Function

    Public Sub New(ByRef callbackMethod As ClientCallbackDelegate)

        blockSize = 10000

        ' Initialize the delegate variable to point to the user's callback method.
        ClientCallbackObject = callbackMethod
    End Sub

    Public Function Connect(ByVal IP_Address As String, ByVal prt As Integer, Optional ByRef errorMessage As String = "") As Boolean

        Try
                            ' Attempt to get the ip address by parsing the IP_Address string:
            IP              = System.Net.IPAddress.Parse(IP_Address)
        Catch ex As Exception
                            ' We got an error - it's not an ip address.
                            ' Maybe it's a hostname.
            IP              = GetIPFromHostname(IP_Address, False)
        End Try

        If IP is Nothing then
            ' Handle invalid IP address passed here.
            errorMessage    = "Could not connect to " & IP_Address & ". It is not a valid IP address or hostname on this network."
            Return False
        End If

        Port                = prt
        continue_running    = True
        errMsg              = ""

        receiveBuffer = New BytesReceivedQueue(ClientCallbackObject)
        sendBuffer = New BytesSentQueue(Me)

        Dim clientCommunicationThread As New Thread(AddressOf Run)
        clientCommunicationThread.Name = "ClientCommunication"
        clientCommunicationThread.Start()

        ' Wait for connection...
        While Not isRunning and errMsg = ""
            Thread.Sleep(5)
        End While

        ' Are we connected?
        errorMessage = errMsg
        If Not isRunning then 
            sendBuffer.Close()
            Return False
        End If

        Return True
    End Function

    Public Sub StopRunning()
        continue_running = False
        sendBuffer.Close
    End Sub

    Public Function GetBlocksize() As UInt16
        Return blockSize
    End Function

    Public Function GetFile(ByVal _path As String) As Boolean

        Do
            If Not UserBytesToBeSentAvailable Then
                SyncLock UserBytesToBeSent
                    UserBytesToBeSent.Close()
                    UserBytesToBeSent = Nothing
                    UserBytesToBeSent = New MemoryStream(StrToByteArray("GFR:" & _path))
                    UserOutputChannel = 254 ' Text messages / commands on channel 254
                    UserBytesToBeSentAvailable = True
                End SyncLock
                Exit Do
            End If

            If theClientIsStopping() Then Exit Function
            Application.DoEvents()
        Loop

    End Function

    Public Function SendFile(ByVal _path As String) As Boolean

        Do
            If Not UserBytesToBeSentAvailable Then
                SyncLock UserBytesToBeSent
                    UserBytesToBeSent.Close()
                    UserBytesToBeSent = Nothing
                    UserBytesToBeSent = New MemoryStream(StrToByteArray("SFR:" & _path))
                    UserOutputChannel = 254 ' Text messages / commands on channel 254
                    UserBytesToBeSentAvailable = True
                End SyncLock
                Exit Do
            End If

            If theClientIsStopping() Then Exit Function
            Thread.Sleep(0)
        Loop

    End Function

    Public Sub CancelIncomingFileTransfer()
        Do
            If Not UserBytesToBeSentAvailable Then
                SyncLock UserBytesToBeSent
                    UserBytesToBeSent.Close()
                    UserBytesToBeSent = Nothing
                    UserBytesToBeSent = New MemoryStream(StrToByteArray("Abort->"))
                    UserOutputChannel = 254
                    UserBytesToBeSentAvailable = True
                End SyncLock
                Exit Do
            End If

            If theClientIsStopping() Then Exit Sub
            Thread.Sleep(0)
        Loop

        FinishReceivingTheFile()
        Try
            File.Delete(ReceivedFilesFolder & "\" & IncomingFileName)
        Catch ex As Exception
        End Try
    End Sub

    Public Sub CancelOutgoingFileTransfer()
        Do
            If Not UserBytesToBeSentAvailable Then
                SyncLock UserBytesToBeSent
                    UserBytesToBeSent.Close()
                    UserBytesToBeSent = Nothing
                    UserBytesToBeSent = New MemoryStream(StrToByteArray("Abort<-"))
                    UserOutputChannel = 254
                    UserBytesToBeSentAvailable = True
                End SyncLock
                Exit Do
            End If

            If theClientIsStopping() Then Exit Sub
            Thread.Sleep(0)
        Loop

        StopSendingTheFile()
    End Sub

    Private Function SendTheBytes(ByVal bytes() As Byte, Optional ByVal channel As Byte = 1) As Boolean

        If channel = 0 Or channel > 250 Then Throw New Exception("Data can not be sent using channel numbers less then 1 or greater then 250.")

        Do
            If Not UserBytesToBeSentAvailable Then
                SyncLock UserBytesToBeSent
                    UserBytesToBeSent.Close()
                    UserBytesToBeSent = Nothing
                    UserBytesToBeSent = New MemoryStream(bytes)
                    UserOutputChannel = channel
                    UserBytesToBeSentAvailable = True
                End SyncLock
                Exit Do
            End If

            If theClientIsStopping() Then Exit Function
            Thread.Sleep(0)
        Loop

        Return True
    End Function

    Public Function GetErrorMessage() As String
        Return errMsg
    End Function

    Public Sub SendBytes(ByVal bytes() As Byte, Optional ByVal channel As Byte = 1)
        If channel = 0 Or channel > 250 Then Throw New Exception("Data can not be sent using channel numbers less then 1 or greater then 250.")

        'If Thread.CurrentThread.Name = "TcpClientSafeSendBackgroundThread" then
            SendTheBytes(bytes, channel)
        'Else
        '    sendBuffer.AddToQueue(bytes, channel)
        'End If
    End Sub

    Public Sub SendText(ByVal text As String, Optional ByVal channel As Byte = 1)
        SendBytes(StrToByteArray(text), channel)
    End Sub

    Private Function RcvBytes(ByVal data() As Byte, Optional ByVal dataChannel As Byte = 1) As Boolean
        ' dataType: >0 = data channel, 251 and up = internal messages. 0 is an invalid channel number (it's the puck)

        If dataChannel < 1 Then Return False

        Try
            receiveBuffer.AddToQueue(data, dataChannel)
        Catch ex As Exception
            ' An unexpected error.
            Debug.WriteLine("Unexpected error in Client\RcvBytes: " & ex.Message)
            Return False
        End Try

        Return True
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

    Private Function SendExternalSystemMessage(ByVal message As String) As Boolean

        SystemBytesToBeSent = StrToByteArray(message)
        SystemOutputChannel = 254 ' Text messages / commands on channel 254
        SystemBytesToBeSentAvailable = True

    End Function

    Private Function BeginToReceiveAFile(ByVal _path As String) As Boolean
        Dim readBuffer As Int32 = 0
        ReceivingFile = True
        BeginToReceiveAFile = True
        fileBytesRecieved = 0

        Try
            CreateFolders(_path)
            fileWriter = New clsAsyncUnbuffWriter(_path, True, _
                            1024 * (clsAsyncUnbuffWriter.GetPageSize()), IncomingFileSize)

        Catch ex As Exception
            _path = ex.Message
            ReceivingFile = False
        End Try

        If Not ReceivingFile Then
            Try
                fileWriter.Close()
            Catch ex As Exception
            End Try
            Return False
        End If
    End Function

    Private Function HandleIncomingFileBytes(ByRef bytes() As Byte) As Boolean

        Try
            fileWriter.Write(bytes, bytes.Length)
            HandleIncomingFileBytes = True
        Catch ex As Exception
            HandleIncomingFileBytes = False
        End Try

    End Function

    Private Sub FinishReceivingTheFile()
        Try
            fileWriter.Close()
            fileWriter = Nothing
            ReceivingFile = False
        Catch ex As Exception
            ReceivingFile = False
        End Try
    End Sub

    Private Sub StopSendingTheFile()
        Try
            SendingFile = False
            fileReader.Close()
            fileReader = Nothing
            GC.GetTotalMemory(True)
        Catch ex As Exception
            SendingFile = False
            GC.GetTotalMemory(True)
        End Try
    End Sub

    Private Sub WrapUpIncomingFile()

        If ReceivingFile Then
            Try
                fileWriter.Close()
                fileWriter = Nothing
                GC.GetTotalMemory(True)
            Catch ex As Exception
            End Try

            Try
                File.Delete(ReceivedFilesFolder & "\" & IncomingFileName)
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Function CheckSessionPermissions(ByVal cmd As String) As Boolean
        ' Your security code here...

        Return True
    End Function

    Private Function BeginFileSend(ByVal _path As String, ByVal fileLength As Long) As Boolean
        filebytesSent = 0

        Try

            fileReader = New FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.None, clsAsyncUnbuffWriter.GetPageSize)
            SendingFile = True
            BeginFileSend = True

        Catch ex As Exception
            BeginFileSend = False
            _path = ex.Message
            SendingFile = False
        End Try

        Try
            If Not BeginFileSend Then fileReader.Close()
        Catch ex As Exception
        End Try

    End Function

    Private Sub GetMoreFileBytesIfAvailable()
        Dim bytesRead As Integer

        If SendingFile And Not SystemBytesToBeSentAvailable Then
            Try
                If SystemBytesToBeSent.Length <> blockSize Then ReDim SystemBytesToBeSent(blockSize - 1)
                bytesRead = fileReader.Read(SystemBytesToBeSent, 0, blockSize)
                If bytesRead <> blockSize Then ReDim Preserve SystemBytesToBeSent(bytesRead - 1)

                If bytesRead > 0 Then
                    SystemOutputChannel = 252 ' File transfer from client to server
                    SystemBytesToBeSentAvailable = True
                    filebytesSent += bytesRead
                Else

                    ReDim SystemBytesToBeSent(blockSize - 1)
                    SendExternalSystemMessage("<-Done") ' Send the server a completion notice.
                    SystemMessage("<-Done")
                    SendingFile = False

                    ' Clean up
                    fileReader.Close()
                    fileReader = Nothing
                    GC.GetTotalMemory(True)
                End If
            Catch ex As Exception
                SendExternalSystemMessage("ERR: " & ex.Message)

                ' We're finished.
                ReDim SystemBytesToBeSent(blockSize - 1)
                SendingFile = False
                fileReader.Close()
            End Try
        End If

    End Sub

    Private Function GetFilenameFromPath(ByVal filePath As String) As String
        Dim filePathParts() As String

        If filePath.Trim = "" Then Return ""

        filePathParts = Split(filePath, "\")
        GetFilenameFromPath = filePathParts(filePathParts.Length - 1)
    End Function

    Private Sub HandleIncomingSystemMessages(ByVal bytes() As Byte, ByVal channel As Byte)

        If channel = 254 Then ' Text commands / messages passed between server and client
            Dim message As String = BytesToString(bytes)
            Dim tmp As String = ""
            Dim filePath As String

            ' Get File Request: The server wants us to send them a file.
            If message.Length > 4 Then tmp = message.Substring(0, 4)
            If tmp = "GFR:" Then ' Get File Request
                ' Get file path...
                filePath = message.Substring(4, message.Length - 4)

                ' Does it exist?
                If File.Exists(message.Substring(4, message.Length - 4)) Then
                    ' Are we already busy sending them a file?
                    If Not SendingFile Then
                        Dim _theFilesInfo As New FileInfo(filePath)
                        outgoingFileName = GetFilenameFromPath(filePath)
                        outgoingFileSize = CULng(_theFilesInfo.Length)
                        If BeginFileSend(filePath, _theFilesInfo.Length) Then
                            ' Send only the file NAME. It will have a different path on the other side.
                            SendExternalSystemMessage("Sending:" & outgoingFileName & _
                                                      ":" & outgoingFileSize.ToString)
                            SystemMessage("Sending file:" & outgoingFileName)
                        Else
                            ' FilePath contains the error message.
                            SendExternalSystemMessage("ERR: " & filePath)
                        End If
                    Else
                        ' There's already a GFR in progress.
                        SendExternalSystemMessage("ERR: File: ''" & _
                                                  FileBeingSentPath & _
                                                  "'' is still in progress. Only one file " & _
                                                  "may be transfered (from client to server) at a time.")
                    End If
                Else
                    ' File doesn't exist. Send an error.
                    SendExternalSystemMessage("ERR: The requested file can not be found by the server.")
                End If
            End If

            If message.Length > 7 Then tmp = message.Substring(0, 8)
            If tmp = "Sending:" Then
                ' Strip away the headder...
                Dim msgParts() As String = Split(message, ":")
                IncomingFileSize = Convert.ToInt64(msgParts(2))
                IncomingFileName = msgParts(1)
                tmp = ReceivedFilesFolder & "\" & IncomingFileName
                SystemMessage("Receiving file: " & IncomingFileName)
                If Not BeginToReceiveAFile(tmp) Then
                    SystemMessage("ERR: " & tmp)
                    SendExternalSystemMessage("Abort<-")
                End If
            End If

            If message.Length > 10 Then tmp = message.Substring(0, 10)
            If tmp = "blocksize:" Then
                Dim msgParts() As String = Split(message, ":")
                blockSize = Convert.ToUInt16(msgParts(1))
            End If

            If message = "->Done" Then
                FinishReceivingTheFile()
                SystemMessage("->Done")
            End If

            ' We've been notified that no file data will be forthcoming.
            If message = "Abort->" Then
                FinishReceivingTheFile()
                SystemMessage("->Aborted.")
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal
                Try
                    File.Delete(ReceivedFilesFolder & "\" & IncomingFileName)
                Catch ex As Exception
                End Try
            End If

            ' Send File Request: The server wants to send us a file.
            If message.Length > 4 Then tmp = message.Substring(0, 4)
            If tmp = "SFR:" Then
                If CheckSessionPermissions("SFR") Then
                    Dim parts() As String
                    parts = Split(message, "SFR:")
                    SendExternalSystemMessage("GFR:" & parts(1))
                Else
                    ' This user doesn't have rights to this file. Send an error.
                    SendExternalSystemMessage("ERR: You do not have permission to send files. Access Denied.")
                End If
            End If

            ' Notification that the server has complied with our  
            ' request to stop sending bytes for this 
            ' (server->client) file transfer.
            If message = "->Aborted." Then
                SystemMessage("->Aborted.")
                WrapUpIncomingFile()
            End If

            ' Notification that the server has complied with our  
            ' request to stop recieving bytes for this 
            ' (client->server) file transfer.
            If message = "<-Aborted." Then
                SystemMessage("<-Aborted.")
            End If

            If message.Length > 4 Then tmp = message.Substring(0, 4)
            If tmp = "ERR:" Then ' The server has sent us an error message.
                ' Pass it on up to the user.
                SystemMessage(message)
            End If

            ' New queue throttling code
            If message = "pause" Then
                sendBuffer.PauseSending()
            End If

            If message = "resume" Then
                sendBuffer.ResumeSending()
            End If

        ElseIf channel = 253 Then ' File transfer from server to client
            Try
                If ReceivingFile Then
                    HandleIncomingFileBytes(bytes)
                    fileBytesRecieved += bytes.LongLength
                End If
            Catch ex As Exception
            End Try
        ElseIf channel = 252 Then ' File transfer from client to server

        ElseIf channel = 251 Then ' reserved.

        End If
    End Sub

    Private Function HandleOutgoingInternalSystemMessage() As Boolean
        Dim tmp(1) As Byte
        HandleOutgoingInternalSystemMessage = False
        Dim _size As Integer

        GetMoreFileBytesIfAvailable()

        ' Handle outgoing system stuff here
        If SystemBytesToBeSentAvailable = True Then
            HandleOutgoingInternalSystemMessage = True
            If SystemBytesToBeSent.Length > blockSize Then
                ' Send Channel
                tmp(0) = SystemOutputChannel
                Stream.Write(tmp, 0, 1)
                bytesSentThisSecond += 1

                ' Send packet size
                _size = blockSize
                tmp = BitConverter.GetBytes(_size)
                Stream.Write(tmp, 0, 2)
                bytesSentThisSecond += 2

                ' Send packet
                Stream.Write(GetSome(SystemBytesToBeSent, blockSize, SystemBytesToBeSentAvailable), 0, _size)
                bytesSentThisSecond += _size
            Else
                ' Send Channel
                tmp(0) = SystemOutputChannel
                Stream.Write(tmp, 0, 1)
                bytesSentThisSecond += 1

                ' Send packet size
                _size = SystemBytesToBeSent.Length
                tmp = BitConverter.GetBytes(_size)
                Stream.Write(tmp, 0, 2)
                bytesSentThisSecond += 2

                ' Send packet
                Stream.Write(SystemBytesToBeSent, 0, _size)
                bytesSentThisSecond += _size
                SystemBytesToBeSentAvailable = False
            End If
        End If

    End Function

    Private Function HandleOutgoingUserData() As Boolean
        Dim tmp(1) As Byte
        Dim _size As UShort
        Dim notify As Boolean = False
        Static packet(0) As Byte

        If UserBytesToBeSentAvailable = True Then
            SyncLock UserBytesToBeSent
                Try
                    If (UserBytesToBeSent.Length - UserBytesToBeSent.Position) > blockSize Then
                        ' Send Channel
                        tmp(0) = UserOutputChannel
                        Stream.Write(tmp, 0, 1)

                        ' Send packet size
                        _size = blockSize
                        tmp = BitConverter.GetBytes(_size)
                        Stream.Write(tmp, 0, 2)

                        ' Send packet
                        If packet.Length <> _size Then ReDim packet(_size - 1)
                        UserBytesToBeSent.Read(packet, 0, _size)
                        'Client.NoDelay = True
                        Stream.Write(packet, 0, _size)
                        bytesSentThisSecond += 3 + _size

                        ' Check to see if we've sent it all...
                        If UserBytesToBeSent.Length = UserBytesToBeSent.Position Then
                            UserBytesToBeSentAvailable = False
                            notify = True
                        End If
                    Else
                        ' Send Channel
                        tmp(0) = UserOutputChannel
                        Stream.Write(tmp, 0, 1)

                        ' Send packet size
                        _size = Convert.ToUInt16(UserBytesToBeSent.Length - UserBytesToBeSent.Position)
                        tmp = BitConverter.GetBytes(_size)
                        Stream.Write(tmp, 0, 2)

                        ' Send packet
                        If packet.Length <> _size Then ReDim packet(_size - 1)
                        UserBytesToBeSent.Read(packet, 0, _size)
                        'Client.NoDelay = True
                        Stream.Write(packet, 0, _size)
                        bytesSentThisSecond += 3 + _size

                        UserBytesToBeSentAvailable = False
                        notify = True
                    End If
                Catch ex As Exception
                    ' Report error attempting to send user data.
                    Debug.WriteLine("Unexpected error in TcpCommClient\HandleOutgoingUserData: " & ex.Message)
                End Try
            End SyncLock

            ' Notify the user that the packet has been sent.
            If notify Then SystemMessage("UBS:" & UserOutputChannel)

            Return True
        Else
            Return False
        End If

    End Function

    'Private Function HandleOutgoingUserData() As Boolean
    '    Dim tmp(1) As Byte
    '    Dim _size As UShort

    '    If UserBytesToBeSentAvailable = True Then
    '        SyncLock UserBytesToBeSent.SyncRoot
    '            If UserBytesToBeSent.Length > blockSize Then
    '                ' Send Channel
    '                tmp(0) = UserOutputChannel
    '                Stream.Write(tmp, 0, 1)
    '                bytesSentThisSecond += 1

    '                ' Send packet size
    '                _size = blockSize
    '                tmp = BitConverter.GetBytes(_size)
    '                Stream.Write(tmp, 0, 2)
    '                bytesSentThisSecond += 2

    '                ' Send packet
    '                Stream.Write(GetSome(UserBytesToBeSent, blockSize, UserBytesToBeSentAvailable, True), 0, _size)
    '                bytesSentThisSecond += _size
    '            Else
    '                ' Send Channel
    '                tmp(0) = UserOutputChannel
    '                Stream.Write(tmp, 0, 1)
    '                bytesSentThisSecond += 1

    '                ' Send packet size
    '                _size = UserBytesToBeSent.Length
    '                tmp = BitConverter.GetBytes(_size)
    '                Stream.Write(tmp, 0, 2)
    '                bytesSentThisSecond += 2

    '                ' Send packet
    '                Stream.Write(UserBytesToBeSent, 0, _size)
    '                bytesSentThisSecond += _size
    '                UserBytesToBeSentAvailable = False
    '                SystemMessage("UBS")
    '            End If
    '        End SyncLock

    '        Return True
    '    Else

    '        Return False
    '    End If
    'End Function

    Private Function GetSome(ByRef bytes() As Byte, ByVal chunkToBreakOff As Integer, _
                             ByRef bytesToBeSentAvailable As Boolean, _
                             Optional ByVal theseAreUserBytes As Boolean = False) As Byte()

        Dim tmp(chunkToBreakOff - 1) As Byte
        Array.Copy(bytes, 0, tmp, 0, chunkToBreakOff)
        GetSome = tmp

        If bytes.Length = chunkToBreakOff Then
            bytesToBeSentAvailable = False
            If theseAreUserBytes Then SystemMessage("UBS")
        Else
            Dim tmp2(bytes.Length - chunkToBreakOff - 1) As Byte
            Array.Copy(bytes, chunkToBreakOff, tmp2, 0, bytes.Length - chunkToBreakOff)
            bytes = tmp2
        End If

    End Function

    Private Sub SystemMessage(ByVal MsgText As String)
        RcvBytes(StrToByteArray(MsgText), 255)
    End Sub

    ' Check to see if our app is closing (set in FormClosing event)
    Private Function theClientIsStopping() As Boolean

        If continue_running = False Then
            theClientIsStopping = True
        Else
            theClientIsStopping = False
        End If

    End Function

    Private Function CalculateMbps(Optional ByVal GetMbps As Boolean = False) As Decimal
        Static averagesCounter As Integer = 0
        Static tmr As Date = Now
        Static lastread As Int32 = 0
        Dim looper As Short = 0
        Dim tmp As Int32 = 0

        If mbpsOneSecondAverage Is Nothing Then ReDim mbpsOneSecondAverage(9)

        If Now >= tmr.AddMilliseconds(250) Then
            averagesCounter += 1
            If averagesCounter < 0 Then averagesCounter = 0
            Select Case averagesCounter
                Case 0
                    SyncLock (mbpsSyncObject)
                        Try
                            mbpsOneSecondAverage(averagesCounter) = bytesSentThisSecond + bytesReceivedThisSecond
                            bytesSentThisSecond = 0
                            bytesReceivedThisSecond = 0
                        Catch ex As Exception
                            averagesCounter = -1
                        End Try
                    End SyncLock
                Case 1
                    SyncLock (mbpsSyncObject)
                        Try
                            mbpsOneSecondAverage(averagesCounter) = bytesSentThisSecond + bytesReceivedThisSecond
                            bytesSentThisSecond = 0
                            bytesReceivedThisSecond = 0
                        Catch ex As Exception
                            averagesCounter = -1
                        End Try
                    End SyncLock
                Case 2
                    SyncLock (mbpsSyncObject)
                        Try
                            mbpsOneSecondAverage(averagesCounter) = bytesSentThisSecond + bytesReceivedThisSecond
                            bytesSentThisSecond = 0
                            bytesReceivedThisSecond = 0
                        Catch ex As Exception
                            averagesCounter = -1
                        End Try
                    End SyncLock
                Case 3
                    SyncLock (mbpsSyncObject)
                        Try
                            mbpsOneSecondAverage(averagesCounter) = bytesSentThisSecond + bytesReceivedThisSecond
                            bytesSentThisSecond = 0
                            bytesReceivedThisSecond = 0
                        Catch ex As Exception
                            averagesCounter = -1
                        End Try
                    End SyncLock
            End Select

            If averagesCounter > 2 Then averagesCounter = -1
            tmr = Now
        End If

        ' Did they ask us for the Mbps?
        If GetMbps Then
            For looper = 0 To 3
                SyncLock (mbpsSyncObject)
                    tmp += mbpsOneSecondAverage(looper)
                End SyncLock
            Next
            CalculateMbps = tmp
        Else
            CalculateMbps = 0
        End If

    End Function

    Private Sub Run()

        Dim puck(1) As Byte : puck(0) = 0
        Dim theBuffer(blockSize - 1) As Byte
        Dim tmp(1) As Byte
        Dim dataChannel As Byte = 0
        Dim packetSize As UShort = 0
        Dim bytesread As Integer
        Dim userOrSystemSwitcher As Integer = 0
        Dim PercentUsage As Short = -1
        Dim connectionLossTimer As Date

        Dim CPUutil As New CpuMonitor
        CPUutil.Start

        Try

            Client = New TcpClient
            Client.Connect(IP, Port)

            ' Connection Accepted.
            Stream = Client.GetStream()

            ' Set the send and receive buffers to the maximum
            ' size allowable in this application...
            Client.Client.ReceiveBufferSize = 65535
            Client.Client.SendBufferSize = 65535

            ' no delay on partially filled packets...
            ' Send it all as fast as possible.
            Client.NoDelay = True

            ' Pass a message up to the user about our status.
            isRunning = True
            SystemMessage("Connected.")

            ' Start the communication loop
            Do
                ' Check to see if our app is shutting down.
                If theClientIsStopping() Then Exit Do

                ' Normal communications...
                If weHaveThePuck Then

                    ' Send user data if there is any to be sent.
                    userOrSystemSwitcher += 1
                    Select Case userOrSystemSwitcher
                        Case 1
                            HandleOutgoingUserData()
                        Case 2
                            HandleOutgoingInternalSystemMessage()
                    End Select
                    If userOrSystemSwitcher > 1 Then userOrSystemSwitcher = 0

                    ' After sending our data, send the puck
                    Stream.Write(puck, 0, 1)

                    ' Uncomment this to see control bit traffic as part of your Mbps 
                    'bytesSentThisSecond += 1
                    weHaveThePuck = False

                End If

                If theBuffer.Length < 2 Then ReDim theBuffer(1)

                ' Read in the control byte.
                Stream.Read(theBuffer, 0, 1)
                dataChannel = theBuffer(0)

                ' Uncomment this to see control bit traffic as part of your Mbps
                'bytesReceivedThisSecond += 1

                ' If it's just the puck (communictaion syncronization byte),
                ' set weHaveThePuck true and that's all. dataChannel 0 is 
                ' reserved for the puck.
                If dataChannel = 0 Then
                    weHaveThePuck = True
                Else
                    ' It's not the puck: It's an incoming packet.

                    ' Get the packet size:
                    tmp(0) = Convert.ToByte(Stream.ReadByte)
                    tmp(1) = Convert.ToByte(Stream.ReadByte)
                    packetSize = BitConverter.ToUInt16(tmp, 0)
                    If theBuffer.Length <> packetSize Then ReDim theBuffer(packetSize - 1)
                    bytesReceivedThisSecond += 2

                    ' Get the packet:
                    connectionLossTimer = Now
                    Do
                        ' Check to see if we're stopping...
                        If theClientIsStopping() Then Exit Do
                        ' Read bytes in...
                        bytesread += Stream.Read(theBuffer, bytesread, (packetSize - bytesread))

                        ' If it takes longer then 3 seconds to get a packet, we've lost connection.
                        If connectionLossTimer.AddSeconds(3) < Now Then Exit Try

                    Loop While bytesread < packetSize
                    bytesread = 0

                    ' Record bytes read for throttling...
                    bytesReceivedThisSecond += packetSize

                    ' Handle the packet...
                    If dataChannel > 250 Then
                        ' this is an internal system packet
                        HandleIncomingSystemMessages(theBuffer, dataChannel)
                    Else
                        ' Hand data off to the calling thread.
                        RcvBytes(theBuffer, dataChannel)
                    End If

                    If theClientIsStopping() Then Exit Do
                End If

                CalculateMbps(False)

                ' Measure and display the CPU usage of the client (this thread).
                If PercentUsage <> CPUutil.ThreadUsage Then
                    PercentUsage = CPUutil.ThreadUsage
                    SystemMessage("" & PercentUsage & "% Thread Usage (" & CPUutil.CPUusage & "% across all CPUs)")
                End If

            Loop
        Catch ex As Exception
            ' An unexpected error.
            Debug.WriteLine("Unexpected error in Client\Run: " & ex.Message)
            errMsg = "Unexpected error in Client\Run: " & ex.Message
        End Try

        Try
            fileWriter.Close()
        Catch ex As Exception
        End Try

        Try
            CPUutil.StopWatcher()
            Client.Client.Close()
            SystemMessage("Disconnected.")
        Catch ex As Exception
            ' An unexpected error.
            Debug.WriteLine("Unexpected error in Client\theClientIsStopping: " & ex.Message)
        End Try

        WrapUpIncomingFile()

        isRunning = False

    End Sub

    '' Receive message queue stuff
    'Private Class message
    '    Public bytes() As Byte
    '    Public dataChannel As Integer
    'End Class
    
    'Private spinning As Boolean = False
    'Private addedMore As Boolean = False
    'Private receiveQueue As New ConcurrentQueue(Of message)

    'Private Sub AddToQueue(bytes() As Byte, dataChannel As Integer)

    '    ' Throttle receiveQueue - 1st half
    '    'If receiveQueue.Count > 99 and Not pauseSent then
    '    '    SendExternalSystemMessage("pause")
    '    '    pauseSent = True
    '    'End If

    '    Dim msg As New message 
    '    Dim theseBytes(bytes.Length - 1) As Byte

    '    Array.Copy(bytes, theseBytes, bytes.Length)

    '    msg.dataChannel = dataChannel
    '    msg.bytes = theseBytes

    '    receiveQueue.Enqueue(msg)
 
    '    If Not spinning then
    '        Dim bgThread As New Thread(AddressOf DoSafeReceive)
    '        bgThread.Start()
    '        spinning = True
    '        addedMore = False
    '    Else
    '        addedMore = True
    '    End If
    'End Sub
 
    'Private Sub DoSafeReceive()

    '    Dim msg As message = Nothing

    '    While spinning
    '        If receiveQueue.TryPeek(msg) then
    '            receiveQueue.TryDequeue(msg)
    '            ClientCallbackObject(msg.bytes, msg.dataChannel)

    '            msg = Nothing

    '            ' Throttle receive queue - 2nd half
    '            'If receiveQueue.Count <= 3 And pauseSent Then
    '            '    SendExternalSystemMessage("resume")
    '            '    pauseSent = False
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