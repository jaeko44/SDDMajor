Imports System.Diagnostics
Imports System.IO

Public Class frmServer

    Private server As TcpComm.Server
    Private lat As TcpComm.Utilities.LargeArrayTransferHelper

    Public Sub UpdateUI(ByVal bytes() As Byte, ByVal sessionID As Int32, ByVal dataChannel As Byte)

        ' Use TcpComm.Utilities.LargeArrayTransferHelper to make it easier to send and receive 
        ' large arrays sent via lat.SendArray()
        ' The LargeArrayTransferHelperb will assemble any number of incoming large arrays
        ' on any channel or from any sessionId, and pass them back to this callback
        ' when they are complete. Returns True if it has handled this incomming packet,
        ' so we exit the callback when it returns true.
        If lat.HandleIncomingBytes(bytes, dataChannel, sessionID) Then Return

        If Me.InvokeRequired() Then
            ' InvokeRequired: We're running on the background thread. Invoke the delegate.
            Me.Invoke(server.ServerCallbackObject, bytes, sessionID, dataChannel)
        Else
            ' We're on the main UI thread now.
            If dataChannel < 251 Then
                Me.lbTextInput.Items.Add("Session " & sessionID.ToString & ": " & TcpComm.Utilities.BytesToString(bytes))
                Dim m As String = TcpComm.Utilities.BytesToString(bytes)
                If m.StartsWith("CMD|") Then
                    ' MessageBox.Show("RUN The following command: " & msg.Remove(0, 4))
                    Dim command As String = m.Remove(0, 4)
                    runCommand(command)
                ElseIf m.StartsWith("REQUEST_DATA|") Then
                    Dim type As String = m.Remove(0, 13)
                    sendServerData(type)
                End If
            ElseIf dataChannel = 255 Then
                Dim tmp = ""
                Dim msg As String = TcpComm.Utilities.BytesToString(bytes)
                Dim dontReport As Boolean = False

                ' server has finished sending the bytes you put into sendBytes()
                If msg.Length > 3 Then tmp = msg.Substring(0, 3)
                If tmp = "UBS" Then ' User Bytes Sent.
                    Dim parts() As String = Split(msg, "UBS:")
                    msg = "Data sent to session: " & parts(1)
                End If

                If msg = "Connected." Then UpdateClientsList()
                If msg.Contains(" MachineID:") Then UpdateClientsList()
                If msg.Contains("Session Stopped. (") Then UpdateClientsList()

                If Not dontReport Then Me.ToolStripStatusLabel1.Text = msg
            End If
        End If

    End Sub

    Private Sub UpdateClientsList()

        Dim sessionList As List(Of TcpComm.Server.SessionCommunications) = server.GetSessionCollection()
        Dim lvi As ListViewItem

        Me.lvClients.Items.Clear()

        For Each session As TcpComm.Server.SessionCommunications In sessionList
            If session.IsRunning Then
                lvi = New ListViewItem(" Connected", 0, lvClients.Groups.Item(0))
            Else
                lvi = New ListViewItem(" Disconnected", 1, lvClients.Groups.Item(1))
            End If

            lvi.SubItems.Add(session.sessionID.ToString())
            lvi.SubItems.Add(session.machineId)
            Me.lvClients.Items.Add(lvi)
        Next
    End Sub

    Private Sub frmServer_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If server IsNot Nothing Then server.Close()
    End Sub

    Private Sub frmServer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.ToolStripStatusLabel1.Text = "Idle."
        'frmClient.Show()
        server = New TcpComm.Server(AddressOf UpdateUI, , False)
        lat = New TcpComm.Utilities.LargeArrayTransferHelper(server)

        server.Start(22490)
        btStartServer.Text = "Stop Server"
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btStartServer.Click

        If btStartServer.Text = "Start Server" Then
            server = New TcpComm.Server(AddressOf UpdateUI, , cbUniqueIds.Checked)
            lat = New TcpComm.Utilities.LargeArrayTransferHelper(server)

            server.Start(22490)
            btStartServer.Text = "Stop Server"
        Else
            If server IsNot Nothing Then server.Close()
            lat = Nothing
            Me.lvClients.Items.Clear()
            btStartServer.Text = "Start Server"
        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSendText.Click
        If Me.tbSendText.Text.Trim.Length > 0 Then
            ' Send a text message to all connected sessions on channel 1.
            server.SendText(Me.tbSendText.Text.Trim)
        End If
    End Sub

    Private Sub btStartNewClient_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btStartNewClient.Click
        Dim newClient As New frmClient
        Static uniqueMachineIDNumber As Int32 = 1
        newClient.SetMachineIDNumber(uniqueMachineIDNumber)
        newClient.Show()
        uniqueMachineIDNumber += 1
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.lbTextInput.Items.Clear()
    End Sub

    Private Sub SendAFileToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SendAFileToolStripMenuItem.Click
        If lvClients.SelectedItems.Count > 0 Then
            Dim lvi As ListViewItem = lvClients.SelectedItems.Item(0)
            ' Get the session using the sessionID pulled from the selected listview item
            Dim session As TcpComm.Server.SessionCommunications = server.GetSession(Convert.ToInt32(lvi.SubItems(1).Text))
            Dim message As String
            Dim fileName As String

            If session Is Nothing Then
                MsgBox("This session is disconnected.", MsgBoxStyle.Critical, "TcpDemoApp")
                Return
            End If

            message = "Select a file to send to " & lvi.SubItems(2).Text

            ofdSendFileToClient.Title = message
            ofdSendFileToClient.FileName = ""
            ofdSendFileToClient.ShowDialog()
            fileName = ofdSendFileToClient.FileName

            If fileName.Trim().Equals("") Then Exit Sub

            If Not server.SendFile(fileName, session.sessionID) Then
                MsgBox("This session is disconnected.", MsgBoxStyle.Critical, "TcpDemoApp")
            End If
        End If
    End Sub

    Private Sub SendTextToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SendTextToolStripMenuItem.Click
        If lvClients.SelectedItems.Count > 0 Then
            Dim lvi As ListViewItem = lvClients.SelectedItems.Item(0)
            ' Get the session using the sessionID pulled from the selected listview item
            Dim session As TcpComm.Server.SessionCommunications = server.GetSession(Convert.ToInt32(lvi.SubItems(1).Text))
            Dim message, title, defaultValue As String
            Dim retValue As Object

            If session Is Nothing Then
                MsgBox("This session is disconnected.", MsgBoxStyle.Critical, "TcpDemoApp")
                Return
            End If

            message = "Type some text to send to " & lvi.SubItems(2).Text
            title = "TcpComm Demo App"
            defaultValue = "Test text"
            retValue = InputBox(message, title, defaultValue)
            If retValue Is "" Then Return

            If session IsNot Nothing Then server.SendText(retValue.ToString(), 1, session.sessionID)
        End If
    End Sub

    Private Sub DisconnectSessionToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles DisconnectSessionToolStripMenuItem.Click
        If lvClients.SelectedItems.Count > 0 Then
            Dim lvi As ListViewItem = lvClients.SelectedItems.Item(0)
            ' Get the session using the sessionID pulled from the selected listview item
            Dim session As TcpComm.Server.SessionCommunications = server.GetSession(Convert.ToInt32(lvi.SubItems(1).Text))

            If session IsNot Nothing Then session.Close()
        End If
    End Sub

    Private Sub TestLargeArrayTransferHelperToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestLargeArrayTransferHelperToolStripMenuItem.Click
        If lvClients.SelectedItems.Count = 0 Then
            MsgBox("You must select a connected client to send an array.", MsgBoxStyle.Critical, "TcpComm Demo App")
            Return
        End If

        Dim lvi As ListViewItem = lvClients.SelectedItems.Item(0)
        ' Get the session using the sessionID pulled from the selected listview item
        Dim session As TcpComm.Server.SessionCommunications = server.GetSession(Convert.ToInt32(lvi.SubItems(1).Text))

        If session Is Nothing Then
            MsgBox("You can't send a large array to a disconnected session!", MsgBoxStyle.Critical, "TcpComm Demo App")
            Return
        End If

        Dim msg = "This version if the library includes a helper function for people attempting to send arrays larger then the maximum packetsize. " & _
            "In those cases, the array will be broken up into multiple packets, and delivered one by one. This helper class can be used to send the large arrays and " & _
            "have LAT (the TcpComm.Utilities.LargeArrayTransferHelper tool) assemble them for you in the remote machine. " & vbCrLf & vbCrLf & _
            "This test will read about 500k of a large text file into a byte array, and send it to the client you selected (this would normally arrive in about 8 pieces). When it arrives, it will be " & _
            "displayed in the 'Lat Viewer', a form with a multiline textbox on it that you can use to verify that all the text has been delivered and assembled " & _
            "properly, and verify the message size."

        Dim retVal As MsgBoxResult = MsgBox(msg, MsgBoxStyle.Information Or MsgBoxStyle.OkCancel, "TcpComm Demo App")
        If retVal = MsgBoxResult.Ok Then
            If session IsNot Nothing Then
                Dim fileBytes() As Byte = System.IO.File.ReadAllBytes("big.txt")
                Dim errMsg As String = ""
                If Not lat.SendArray(fileBytes, 100, session.sessionID, errMsg) Then MsgBox(errMsg, MsgBoxStyle.Critical, "TcpComm Demo App")
            End If
        End If
    End Sub

    Private Sub lvClients_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvClients.SelectedIndexChanged

    End Sub

    Private Sub runCommand(CmdCommand As String)
        'Dim startCMD As New ProcessStartInfo("CMD.EXE")
        'With startCMD
        '    .WindowStyle = ProcessWindowStyle.Minimized
        '    .WindowStyle = ProcessWindowStyle.Hidden
        '    .CreateNoWindow = True
        '    .UseShellExecute = False
        '    .Arguments = CmdCommand
        'End With
        'Process.Start(startCMD)

        Dim startInfo As New ProcessStartInfo("cmd") With {
            .WindowStyle = ProcessWindowStyle.Hidden,
            .UseShellExecute = False,
            .RedirectStandardInput = True,
            .RedirectStandardOutput = True,
            .CreateNoWindow = True
        }

        Dim process As New Process()
        process.StartInfo = startInfo
        process.Start()
        process.StandardInput.WriteLine(CmdCommand) ' Runs the command
        process.StandardInput.WriteLine("exit") ' Exits CMD 
        Dim output = process.StandardOutput.ReadToEnd()
        cleanUpCMD(output)
        process.Dispose()

    End Sub
    Private Sub cleanUpCMD(input As String)
        input = input.Remove(0, 96)
        Dim appPath As String = Application.StartupPath()
        Dim cleanOutput As String = input.Replace(appPath, "")
        cleanOutput = cleanOutput.Replace(">", "Process Comand: ")
        Dim appSave As String = appPath & "/tempMsg.txt"
        ' This text is added only once to the file. 
        'If File.Exists(appPath) = True Then
        '    System.IO.File.Delete(appSave)
        'End If
        'File.WriteAllText(appSave, cleanOutput)
        'MessageBox.Show(cleanOutput)
        Dim osVersion As String = System.Environment.OSVersion.ToString()
        If cleanOutput.Trim.Length > 0 Then
            server.SendText("CMDResponse|" & cleanOutput)
            server.SendText("OSVersion|" & osVersion)
        End If
    End Sub

    Private Sub sendServerData(_type As String)
        If _type = "ALL" Then
            getOS()
            getHDD()
        End If
        getCPU()
        getRam()
    End Sub

    Private Sub getCPU()
        Dim cpu As New PerformanceCounter()
        With cpu
            .CategoryName = "Processor"
            .CounterName = "% Processor Time"
            .InstanceName = "_Total"
        End With
        Dim firstValue = cpu.NextValue()
        System.Threading.Thread.Sleep(1000)
        Dim secondValue = cpu.NextValue()
        server.SendText("CPUUsage|" & secondValue)


    End Sub

    Private Sub getRam()
        Dim totram As ULong = My.Computer.Info.TotalPhysicalMemory 'need to divide since its given as bytes
        totram = CULng(totram / 1024 / 1024)
        Dim ram As New PerformanceCounter("Memory", "Available MBytes")
        Dim firstValue = ram.NextValue()
        Dim usedRam As Integer = CInt(totram - firstValue) 'Calculated the USED ram in order to calculate a percentage
        Dim usedPercent As Integer = CInt(Math.Ceiling(CInt(usedRam / totram * 100)))
        server.SendText("RAMUsage|" & usedPercent)
        server.SendText("TOTALRam|" & totram)
    End Sub

    Private Sub getHDD()
        Dim allDrives() As DriveInfo = DriveInfo.GetDrives()
        Dim cdrive As System.IO.DriveInfo
        cdrive = My.Computer.FileSystem.GetDriveInfo("C:\")

        server.SendText("TOTALSize|" & Math.Ceiling(cdrive.TotalSize / 1024 / 1024))
        server.SendText("HDDUsage|" & Math.Ceiling(100 - cdrive.TotalFreeSpace / 1024 / 1024 / cdrive.TotalSize / 1024 / 1024))

    End Sub

    Private Sub getOS()
        Dim osVersion As String = System.Environment.OSVersion.ToString()
        server.SendText("OSVersion|" & osVersion)

    End Sub

End Class