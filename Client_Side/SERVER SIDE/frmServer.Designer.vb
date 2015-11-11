<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmServer
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
        Dim ListViewGroup3 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Connected Clients", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup4 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Disconnected Clients", System.Windows.Forms.HorizontalAlignment.Left)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmServer))
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.btSendText = New System.Windows.Forms.Button()
        Me.gbTextInput = New System.Windows.Forms.GroupBox()
        Me.lbTextInput = New System.Windows.Forms.ListBox()
        Me.tbSendText = New System.Windows.Forms.TextBox()
        Me.btStartServer = New System.Windows.Forms.Button()
        Me.gbSentText = New System.Windows.Forms.GroupBox()
        Me.btStartNewClient = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lvClients = New System.Windows.Forms.ListView()
        Me.Status = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SessionId = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.MachineId = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.sessionRightClickMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SendTextToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SendAFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestLargeArrayTransferHelperToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DisconnectSessionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lvImages = New System.Windows.Forms.ImageList(Me.components)
        Me.ofdSendFileToClient = New System.Windows.Forms.OpenFileDialog()
        Me.cbUniqueIds = New System.Windows.Forms.CheckBox()
        Me.StatusStrip1.SuspendLayout()
        Me.gbTextInput.SuspendLayout()
        Me.gbSentText.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.sessionRightClickMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 575)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 10, 0)
        Me.StatusStrip1.Size = New System.Drawing.Size(345, 22)
        Me.StatusStrip1.TabIndex = 0
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(120, 17)
        Me.ToolStripStatusLabel1.Text = "ToolStripStatusLabel1"
        '
        'btSendText
        '
        Me.btSendText.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btSendText.Location = New System.Drawing.Point(259, 14)
        Me.btSendText.Margin = New System.Windows.Forms.Padding(2)
        Me.btSendText.Name = "btSendText"
        Me.btSendText.Size = New System.Drawing.Size(64, 24)
        Me.btSendText.TabIndex = 1
        Me.btSendText.Text = "Send Text"
        Me.btSendText.UseVisualStyleBackColor = True
        '
        'gbTextInput
        '
        Me.gbTextInput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbTextInput.Controls.Add(Me.lbTextInput)
        Me.gbTextInput.Location = New System.Drawing.Point(7, 249)
        Me.gbTextInput.Margin = New System.Windows.Forms.Padding(2)
        Me.gbTextInput.Name = "gbTextInput"
        Me.gbTextInput.Padding = New System.Windows.Forms.Padding(2)
        Me.gbTextInput.Size = New System.Drawing.Size(327, 207)
        Me.gbTextInput.TabIndex = 2
        Me.gbTextInput.TabStop = False
        Me.gbTextInput.Text = "Text in:"
        '
        'lbTextInput
        '
        Me.lbTextInput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbTextInput.FormattingEnabled = True
        Me.lbTextInput.HorizontalScrollbar = True
        Me.lbTextInput.Location = New System.Drawing.Point(4, 17)
        Me.lbTextInput.Margin = New System.Windows.Forms.Padding(2)
        Me.lbTextInput.Name = "lbTextInput"
        Me.lbTextInput.Size = New System.Drawing.Size(319, 186)
        Me.lbTextInput.TabIndex = 0
        '
        'tbSendText
        '
        Me.tbSendText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbSendText.Location = New System.Drawing.Point(4, 16)
        Me.tbSendText.Margin = New System.Windows.Forms.Padding(2)
        Me.tbSendText.Multiline = True
        Me.tbSendText.Name = "tbSendText"
        Me.tbSendText.Size = New System.Drawing.Size(251, 24)
        Me.tbSendText.TabIndex = 3
        '
        'btStartServer
        '
        Me.btStartServer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btStartServer.Location = New System.Drawing.Point(268, 542)
        Me.btStartServer.Margin = New System.Windows.Forms.Padding(2)
        Me.btStartServer.Name = "btStartServer"
        Me.btStartServer.Size = New System.Drawing.Size(69, 24)
        Me.btStartServer.TabIndex = 1
        Me.btStartServer.Text = "Start Server"
        Me.btStartServer.UseVisualStyleBackColor = True
        '
        'gbSentText
        '
        Me.gbSentText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbSentText.Controls.Add(Me.tbSendText)
        Me.gbSentText.Controls.Add(Me.btSendText)
        Me.gbSentText.Location = New System.Drawing.Point(7, 493)
        Me.gbSentText.Margin = New System.Windows.Forms.Padding(2)
        Me.gbSentText.Name = "gbSentText"
        Me.gbSentText.Padding = New System.Windows.Forms.Padding(2)
        Me.gbSentText.Size = New System.Drawing.Size(327, 45)
        Me.gbSentText.TabIndex = 1
        Me.gbSentText.TabStop = False
        Me.gbSentText.Text = "Broadcast text (send to all connected clients)"
        '
        'btStartNewClient
        '
        Me.btStartNewClient.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btStartNewClient.Location = New System.Drawing.Point(171, 542)
        Me.btStartNewClient.Margin = New System.Windows.Forms.Padding(2)
        Me.btStartNewClient.Name = "btStartNewClient"
        Me.btStartNewClient.Size = New System.Drawing.Size(93, 24)
        Me.btStartNewClient.TabIndex = 3
        Me.btStartNewClient.Text = "Start New Client"
        Me.btStartNewClient.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(91, 543)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Clear Listbox"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lvClients)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 11)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(2)
        Me.GroupBox1.Size = New System.Drawing.Size(327, 234)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Connected clients"
        '
        'lvClients
        '
        Me.lvClients.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvClients.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Status, Me.SessionId, Me.MachineId})
        Me.lvClients.ContextMenuStrip = Me.sessionRightClickMenu
        Me.lvClients.FullRowSelect = True
        ListViewGroup3.Header = "Connected Clients"
        ListViewGroup3.Name = "ConnectedClients"
        ListViewGroup4.Header = "Disconnected Clients"
        ListViewGroup4.Name = "DisconnectedClients"
        Me.lvClients.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup3, ListViewGroup4})
        Me.lvClients.Location = New System.Drawing.Point(4, 18)
        Me.lvClients.Name = "lvClients"
        Me.lvClients.Size = New System.Drawing.Size(319, 211)
        Me.lvClients.SmallImageList = Me.lvImages
        Me.lvClients.TabIndex = 0
        Me.lvClients.UseCompatibleStateImageBehavior = False
        Me.lvClients.View = System.Windows.Forms.View.Details
        '
        'Status
        '
        Me.Status.Text = "Status"
        Me.Status.Width = 121
        '
        'SessionId
        '
        Me.SessionId.Text = "SessionId"
        Me.SessionId.Width = 63
        '
        'MachineId
        '
        Me.MachineId.Text = "MachineId"
        Me.MachineId.Width = 128
        '
        'sessionRightClickMenu
        '
        Me.sessionRightClickMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SendTextToolStripMenuItem, Me.SendAFileToolStripMenuItem, Me.TestLargeArrayTransferHelperToolStripMenuItem, Me.DisconnectSessionToolStripMenuItem})
        Me.sessionRightClickMenu.Name = "sessionRightClickMenu"
        Me.sessionRightClickMenu.Size = New System.Drawing.Size(176, 92)
        '
        'SendTextToolStripMenuItem
        '
        Me.SendTextToolStripMenuItem.Name = "SendTextToolStripMenuItem"
        Me.SendTextToolStripMenuItem.Size = New System.Drawing.Size(175, 22)
        Me.SendTextToolStripMenuItem.Text = "Send Text"
        '
        'SendAFileToolStripMenuItem
        '
        Me.SendAFileToolStripMenuItem.Name = "SendAFileToolStripMenuItem"
        Me.SendAFileToolStripMenuItem.Size = New System.Drawing.Size(175, 22)
        Me.SendAFileToolStripMenuItem.Text = "Send A file"
        '
        'TestLargeArrayTransferHelperToolStripMenuItem
        '
        Me.TestLargeArrayTransferHelperToolStripMenuItem.Name = "TestLargeArrayTransferHelperToolStripMenuItem"
        Me.TestLargeArrayTransferHelperToolStripMenuItem.Size = New System.Drawing.Size(175, 22)
        Me.TestLargeArrayTransferHelperToolStripMenuItem.Text = "Send a large array"
        '
        'DisconnectSessionToolStripMenuItem
        '
        Me.DisconnectSessionToolStripMenuItem.Name = "DisconnectSessionToolStripMenuItem"
        Me.DisconnectSessionToolStripMenuItem.Size = New System.Drawing.Size(175, 22)
        Me.DisconnectSessionToolStripMenuItem.Text = "Disconnect Session"
        '
        'lvImages
        '
        Me.lvImages.ImageStream = CType(resources.GetObject("lvImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.lvImages.TransparentColor = System.Drawing.Color.Transparent
        Me.lvImages.Images.SetKeyName(0, "user-available.ico")
        Me.lvImages.Images.SetKeyName(1, "user-invisible.ico")
        '
        'ofdSendFileToClient
        '
        Me.ofdSendFileToClient.FileName = "OpenFileDialog1"
        '
        'cbUniqueIds
        '
        Me.cbUniqueIds.AutoSize = True
        Me.cbUniqueIds.Checked = True
        Me.cbUniqueIds.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbUniqueIds.Location = New System.Drawing.Point(5, 468)
        Me.cbUniqueIds.Name = "cbUniqueIds"
        Me.cbUniqueIds.Size = New System.Drawing.Size(161, 17)
        Me.cbUniqueIds.TabIndex = 6
        Me.cbUniqueIds.Text = "Enforce unique Machine IDs"
        Me.cbUniqueIds.UseVisualStyleBackColor = True
        '
        'frmServer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(345, 597)
        Me.Controls.Add(Me.cbUniqueIds)
        Me.Controls.Add(Me.gbTextInput)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btStartNewClient)
        Me.Controls.Add(Me.gbSentText)
        Me.Controls.Add(Me.btStartServer)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "frmServer"
        Me.Text = "Communications Side"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.gbTextInput.ResumeLayout(False)
        Me.gbSentText.ResumeLayout(False)
        Me.gbSentText.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.sessionRightClickMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents btSendText As System.Windows.Forms.Button
    Friend WithEvents gbTextInput As System.Windows.Forms.GroupBox
    Friend WithEvents lbTextInput As System.Windows.Forms.ListBox
    Friend WithEvents tbSendText As System.Windows.Forms.TextBox
    Friend WithEvents btStartServer As System.Windows.Forms.Button
    Friend WithEvents gbSentText As System.Windows.Forms.GroupBox
    Friend WithEvents btStartNewClient As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lvClients As System.Windows.Forms.ListView
    Friend WithEvents Status As System.Windows.Forms.ColumnHeader
    Friend WithEvents SessionId As System.Windows.Forms.ColumnHeader
    Friend WithEvents MachineId As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvImages As System.Windows.Forms.ImageList
    Friend WithEvents sessionRightClickMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SendTextToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SendAFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ofdSendFileToClient As System.Windows.Forms.OpenFileDialog
    Friend WithEvents DisconnectSessionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TestLargeArrayTransferHelperToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cbUniqueIds As System.Windows.Forms.CheckBox

End Class
