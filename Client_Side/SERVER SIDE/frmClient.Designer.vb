<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmClient
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim btSendFileBrowse As System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button()
        Me.tbSendText = New System.Windows.Forms.TextBox()
        Me.gbTextIn = New System.Windows.Forms.GroupBox()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.btSendText = New System.Windows.Forms.Button()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.gbIpAddress = New System.Windows.Forms.GroupBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.gbServerPort = New System.Windows.Forms.GroupBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.tbGetFileReq = New System.Windows.Forms.TextBox()
        Me.btGetFile = New System.Windows.Forms.Button()
        Me.gbGetFilePregress = New System.Windows.Forms.GroupBox()
        Me.pbIncomingFile = New System.Windows.Forms.ProgressBar()
        Me.gbGetFile = New System.Windows.Forms.GroupBox()
        Me.btGetFileBrowse = New System.Windows.Forms.Button()
        Me.gbSendFile = New System.Windows.Forms.GroupBox()
        Me.tbSendFile = New System.Windows.Forms.TextBox()
        Me.btSendFile = New System.Windows.Forms.Button()
        Me.gbSendText = New System.Windows.Forms.GroupBox()
        Me.gbSendFileProgress = New System.Windows.Forms.GroupBox()
        Me.pbOutgoingFile = New System.Windows.Forms.ProgressBar()
        Me.tmrPoll = New System.Windows.Forms.Timer(Me.components)
        Me.gbMachineID = New System.Windows.Forms.GroupBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.tbMachineID = New System.Windows.Forms.TextBox()
        Me.cbReconnect = New System.Windows.Forms.CheckBox()
        btSendFileBrowse = New System.Windows.Forms.Button()
        Me.gbTextIn.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.gbIpAddress.SuspendLayout()
        Me.gbServerPort.SuspendLayout()
        Me.gbGetFilePregress.SuspendLayout()
        Me.gbGetFile.SuspendLayout()
        Me.gbSendFile.SuspendLayout()
        Me.gbSendText.SuspendLayout()
        Me.gbSendFileProgress.SuspendLayout()
        Me.gbMachineID.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(194, 22)
        Me.Button2.Margin = New System.Windows.Forms.Padding(2)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(72, 23)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "Connect"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'tbSendText
        '
        Me.tbSendText.Location = New System.Drawing.Point(4, 17)
        Me.tbSendText.Margin = New System.Windows.Forms.Padding(2)
        Me.tbSendText.Multiline = True
        Me.tbSendText.Name = "tbSendText"
        Me.tbSendText.Size = New System.Drawing.Size(177, 24)
        Me.tbSendText.TabIndex = 8
        '
        'gbTextIn
        '
        Me.gbTextIn.Controls.Add(Me.ListBox1)
        Me.gbTextIn.Location = New System.Drawing.Point(9, 139)
        Me.gbTextIn.Margin = New System.Windows.Forms.Padding(2)
        Me.gbTextIn.Name = "gbTextIn"
        Me.gbTextIn.Padding = New System.Windows.Forms.Padding(2)
        Me.gbTextIn.Size = New System.Drawing.Size(257, 119)
        Me.gbTextIn.TabIndex = 7
        Me.gbTextIn.TabStop = False
        Me.gbTextIn.Text = "Text in:"
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(4, 17)
        Me.ListBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(245, 95)
        Me.ListBox1.TabIndex = 0
        '
        'btSendText
        '
        Me.btSendText.Location = New System.Drawing.Point(190, 17)
        Me.btSendText.Margin = New System.Windows.Forms.Padding(2)
        Me.btSendText.Name = "btSendText"
        Me.btSendText.Size = New System.Drawing.Size(64, 24)
        Me.btSendText.TabIndex = 5
        Me.btSendText.Text = "Send"
        Me.btSendText.UseVisualStyleBackColor = True
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(29, 17)
        Me.ToolStripStatusLabel1.Text = "Idle."
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 512)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 10, 0)
        Me.StatusStrip1.Size = New System.Drawing.Size(271, 22)
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'gbIpAddress
        '
        Me.gbIpAddress.Controls.Add(Me.TextBox2)
        Me.gbIpAddress.Location = New System.Drawing.Point(9, 10)
        Me.gbIpAddress.Margin = New System.Windows.Forms.Padding(2)
        Me.gbIpAddress.Name = "gbIpAddress"
        Me.gbIpAddress.Padding = New System.Windows.Forms.Padding(2)
        Me.gbIpAddress.Size = New System.Drawing.Size(103, 38)
        Me.gbIpAddress.TabIndex = 9
        Me.gbIpAddress.TabStop = False
        Me.gbIpAddress.Text = "Server IP Address"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(4, 15)
        Me.TextBox2.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(95, 20)
        Me.TextBox2.TabIndex = 0
        '
        'gbServerPort
        '
        Me.gbServerPort.Controls.Add(Me.TextBox3)
        Me.gbServerPort.Location = New System.Drawing.Point(116, 10)
        Me.gbServerPort.Margin = New System.Windows.Forms.Padding(2)
        Me.gbServerPort.Name = "gbServerPort"
        Me.gbServerPort.Padding = New System.Windows.Forms.Padding(2)
        Me.gbServerPort.Size = New System.Drawing.Size(74, 38)
        Me.gbServerPort.TabIndex = 10
        Me.gbServerPort.TabStop = False
        Me.gbServerPort.Text = "Server Port"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(4, 15)
        Me.TextBox3.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(62, 20)
        Me.TextBox3.TabIndex = 0
        Me.TextBox3.Text = "22490"
        '
        'tbGetFileReq
        '
        Me.tbGetFileReq.Location = New System.Drawing.Point(4, 17)
        Me.tbGetFileReq.Margin = New System.Windows.Forms.Padding(2)
        Me.tbGetFileReq.Multiline = True
        Me.tbGetFileReq.Name = "tbGetFileReq"
        Me.tbGetFileReq.Size = New System.Drawing.Size(124, 24)
        Me.tbGetFileReq.TabIndex = 12
        '
        'btGetFile
        '
        Me.btGetFile.Location = New System.Drawing.Point(195, 17)
        Me.btGetFile.Margin = New System.Windows.Forms.Padding(2)
        Me.btGetFile.Name = "btGetFile"
        Me.btGetFile.Size = New System.Drawing.Size(58, 24)
        Me.btGetFile.TabIndex = 13
        Me.btGetFile.Text = "Get File"
        Me.btGetFile.UseVisualStyleBackColor = True
        '
        'gbGetFilePregress
        '
        Me.gbGetFilePregress.Controls.Add(Me.pbIncomingFile)
        Me.gbGetFilePregress.Location = New System.Drawing.Point(9, 421)
        Me.gbGetFilePregress.Margin = New System.Windows.Forms.Padding(2)
        Me.gbGetFilePregress.Name = "gbGetFilePregress"
        Me.gbGetFilePregress.Padding = New System.Windows.Forms.Padding(2)
        Me.gbGetFilePregress.Size = New System.Drawing.Size(257, 41)
        Me.gbGetFilePregress.TabIndex = 14
        Me.gbGetFilePregress.TabStop = False
        Me.gbGetFilePregress.Text = "File -> Client:"
        '
        'pbIncomingFile
        '
        Me.pbIncomingFile.Location = New System.Drawing.Point(4, 25)
        Me.pbIncomingFile.Margin = New System.Windows.Forms.Padding(2)
        Me.pbIncomingFile.Name = "pbIncomingFile"
        Me.pbIncomingFile.Size = New System.Drawing.Size(248, 14)
        Me.pbIncomingFile.TabIndex = 0
        '
        'gbGetFile
        '
        Me.gbGetFile.Controls.Add(Me.btGetFileBrowse)
        Me.gbGetFile.Controls.Add(Me.tbGetFileReq)
        Me.gbGetFile.Controls.Add(Me.btGetFile)
        Me.gbGetFile.Location = New System.Drawing.Point(9, 315)
        Me.gbGetFile.Margin = New System.Windows.Forms.Padding(2)
        Me.gbGetFile.Name = "gbGetFile"
        Me.gbGetFile.Padding = New System.Windows.Forms.Padding(2)
        Me.gbGetFile.Size = New System.Drawing.Size(257, 49)
        Me.gbGetFile.TabIndex = 1
        Me.gbGetFile.TabStop = False
        Me.gbGetFile.Text = "Get a file from the server"
        '
        'btGetFileBrowse
        '
        Me.btGetFileBrowse.Location = New System.Drawing.Point(132, 17)
        Me.btGetFileBrowse.Margin = New System.Windows.Forms.Padding(2)
        Me.btGetFileBrowse.Name = "btGetFileBrowse"
        Me.btGetFileBrowse.Size = New System.Drawing.Size(58, 24)
        Me.btGetFileBrowse.TabIndex = 14
        Me.btGetFileBrowse.Text = "Browse"
        Me.btGetFileBrowse.UseVisualStyleBackColor = True
        '
        'gbSendFile
        '
        Me.gbSendFile.Controls.Add(btSendFileBrowse)
        Me.gbSendFile.Controls.Add(Me.tbSendFile)
        Me.gbSendFile.Controls.Add(Me.btSendFile)
        Me.gbSendFile.Location = New System.Drawing.Point(9, 368)
        Me.gbSendFile.Margin = New System.Windows.Forms.Padding(2)
        Me.gbSendFile.Name = "gbSendFile"
        Me.gbSendFile.Padding = New System.Windows.Forms.Padding(2)
        Me.gbSendFile.Size = New System.Drawing.Size(257, 49)
        Me.gbSendFile.TabIndex = 16
        Me.gbSendFile.TabStop = False
        Me.gbSendFile.Text = "Send a file to the server"
        '
        'btSendFileBrowse
        '
        btSendFileBrowse.Location = New System.Drawing.Point(132, 17)
        btSendFileBrowse.Margin = New System.Windows.Forms.Padding(2)
        btSendFileBrowse.Name = "btSendFileBrowse"
        btSendFileBrowse.Size = New System.Drawing.Size(58, 24)
        btSendFileBrowse.TabIndex = 14
        btSendFileBrowse.Text = "Browse"
        btSendFileBrowse.UseVisualStyleBackColor = True
        AddHandler btSendFileBrowse.Click, AddressOf Me.btSendFileBrowse_Click
        '
        'tbSendFile
        '
        Me.tbSendFile.Location = New System.Drawing.Point(4, 17)
        Me.tbSendFile.Margin = New System.Windows.Forms.Padding(2)
        Me.tbSendFile.Multiline = True
        Me.tbSendFile.Name = "tbSendFile"
        Me.tbSendFile.Size = New System.Drawing.Size(124, 24)
        Me.tbSendFile.TabIndex = 12
        '
        'btSendFile
        '
        Me.btSendFile.Location = New System.Drawing.Point(195, 17)
        Me.btSendFile.Margin = New System.Windows.Forms.Padding(2)
        Me.btSendFile.Name = "btSendFile"
        Me.btSendFile.Size = New System.Drawing.Size(58, 24)
        Me.btSendFile.TabIndex = 13
        Me.btSendFile.Text = "Send File"
        Me.btSendFile.UseVisualStyleBackColor = True
        '
        'gbSendText
        '
        Me.gbSendText.Controls.Add(Me.btSendText)
        Me.gbSendText.Controls.Add(Me.tbSendText)
        Me.gbSendText.Location = New System.Drawing.Point(9, 262)
        Me.gbSendText.Margin = New System.Windows.Forms.Padding(2)
        Me.gbSendText.Name = "gbSendText"
        Me.gbSendText.Padding = New System.Windows.Forms.Padding(2)
        Me.gbSendText.Size = New System.Drawing.Size(257, 49)
        Me.gbSendText.TabIndex = 17
        Me.gbSendText.TabStop = False
        Me.gbSendText.Text = "Sent Text"
        '
        'gbSendFileProgress
        '
        Me.gbSendFileProgress.Controls.Add(Me.pbOutgoingFile)
        Me.gbSendFileProgress.Location = New System.Drawing.Point(9, 466)
        Me.gbSendFileProgress.Margin = New System.Windows.Forms.Padding(2)
        Me.gbSendFileProgress.Name = "gbSendFileProgress"
        Me.gbSendFileProgress.Padding = New System.Windows.Forms.Padding(2)
        Me.gbSendFileProgress.Size = New System.Drawing.Size(257, 41)
        Me.gbSendFileProgress.TabIndex = 18
        Me.gbSendFileProgress.TabStop = False
        Me.gbSendFileProgress.Text = "File -> Server:"
        '
        'pbOutgoingFile
        '
        Me.pbOutgoingFile.Location = New System.Drawing.Point(4, 25)
        Me.pbOutgoingFile.Margin = New System.Windows.Forms.Padding(2)
        Me.pbOutgoingFile.Name = "pbOutgoingFile"
        Me.pbOutgoingFile.Size = New System.Drawing.Size(248, 14)
        Me.pbOutgoingFile.TabIndex = 0
        '
        'tmrPoll
        '
        '
        'gbMachineID
        '
        Me.gbMachineID.Controls.Add(Me.Button1)
        Me.gbMachineID.Controls.Add(Me.tbMachineID)
        Me.gbMachineID.Location = New System.Drawing.Point(9, 55)
        Me.gbMachineID.Margin = New System.Windows.Forms.Padding(2)
        Me.gbMachineID.Name = "gbMachineID"
        Me.gbMachineID.Padding = New System.Windows.Forms.Padding(2)
        Me.gbMachineID.Size = New System.Drawing.Size(257, 38)
        Me.gbMachineID.TabIndex = 19
        Me.gbMachineID.TabStop = False
        Me.gbMachineID.Text = "MachineID"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(185, 12)
        Me.Button1.Margin = New System.Windows.Forms.Padding(2)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(64, 24)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Set"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'tbMachineID
        '
        Me.tbMachineID.Location = New System.Drawing.Point(4, 15)
        Me.tbMachineID.Margin = New System.Windows.Forms.Padding(2)
        Me.tbMachineID.Name = "tbMachineID"
        Me.tbMachineID.Size = New System.Drawing.Size(177, 20)
        Me.tbMachineID.TabIndex = 0
        Me.tbMachineID.Text = "Unique Machine ID0"
        '
        'cbReconnect
        '
        Me.cbReconnect.Checked = True
        Me.cbReconnect.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbReconnect.Location = New System.Drawing.Point(9, 99)
        Me.cbReconnect.Name = "cbReconnect"
        Me.cbReconnect.Size = New System.Drawing.Size(257, 35)
        Me.cbReconnect.TabIndex = 20
        Me.cbReconnect.Text = "Automatically attempt to reconect for 30 seconds in the event of connection loss." & _
    ""
        Me.cbReconnect.UseVisualStyleBackColor = True
        '
        'frmClient
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(271, 534)
        Me.Controls.Add(Me.cbReconnect)
        Me.Controls.Add(Me.gbMachineID)
        Me.Controls.Add(Me.gbSendFileProgress)
        Me.Controls.Add(Me.gbSendText)
        Me.Controls.Add(Me.gbSendFile)
        Me.Controls.Add(Me.gbGetFile)
        Me.Controls.Add(Me.gbGetFilePregress)
        Me.Controls.Add(Me.gbServerPort)
        Me.Controls.Add(Me.gbIpAddress)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.gbTextIn)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "frmClient"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TcpComm Client"
        Me.gbTextIn.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.gbIpAddress.ResumeLayout(False)
        Me.gbIpAddress.PerformLayout()
        Me.gbServerPort.ResumeLayout(False)
        Me.gbServerPort.PerformLayout()
        Me.gbGetFilePregress.ResumeLayout(False)
        Me.gbGetFile.ResumeLayout(False)
        Me.gbGetFile.PerformLayout()
        Me.gbSendFile.ResumeLayout(False)
        Me.gbSendFile.PerformLayout()
        Me.gbSendText.ResumeLayout(False)
        Me.gbSendText.PerformLayout()
        Me.gbSendFileProgress.ResumeLayout(False)
        Me.gbMachineID.ResumeLayout(False)
        Me.gbMachineID.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents tbSendText As System.Windows.Forms.TextBox
    Friend WithEvents gbTextIn As System.Windows.Forms.GroupBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents btSendText As System.Windows.Forms.Button
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents gbIpAddress As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents gbServerPort As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents tbGetFileReq As System.Windows.Forms.TextBox
    Friend WithEvents btGetFile As System.Windows.Forms.Button
    Friend WithEvents gbGetFilePregress As System.Windows.Forms.GroupBox
    Friend WithEvents pbIncomingFile As System.Windows.Forms.ProgressBar
    Friend WithEvents gbGetFile As System.Windows.Forms.GroupBox
    Friend WithEvents btGetFileBrowse As System.Windows.Forms.Button
    Friend WithEvents gbSendFile As System.Windows.Forms.GroupBox
    Friend WithEvents tbSendFile As System.Windows.Forms.TextBox
    Friend WithEvents btSendFile As System.Windows.Forms.Button
    Friend WithEvents gbSendText As System.Windows.Forms.GroupBox
    Friend WithEvents gbSendFileProgress As System.Windows.Forms.GroupBox
    Friend WithEvents pbOutgoingFile As System.Windows.Forms.ProgressBar
    Friend WithEvents tmrPoll As System.Windows.Forms.Timer
    Friend WithEvents gbMachineID As System.Windows.Forms.GroupBox
    Friend WithEvents tbMachineID As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents cbReconnect As System.Windows.Forms.CheckBox
End Class
