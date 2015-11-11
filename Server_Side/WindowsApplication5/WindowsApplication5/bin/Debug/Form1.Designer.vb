<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.FormSkin1 = New WindowsApplication5.FormSkin()
        Me.FlatTabControl1 = New WindowsApplication5.FlatTabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.FlatAlertBox3 = New WindowsApplication5.FlatAlertBox()
        Me.Login = New WindowsApplication5.FlatButton()
        Me.pass_Login = New WindowsApplication5.FlatTextBox()
        Me.user_Login = New WindowsApplication5.FlatTextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.FlatAlertBox1 = New WindowsApplication5.FlatAlertBox()
        Me.FlatButton1 = New WindowsApplication5.FlatButton()
        Me.email_Register = New WindowsApplication5.FlatTextBox()
        Me.password_Register = New WindowsApplication5.FlatTextBox()
        Me.user_Register = New WindowsApplication5.FlatTextBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.FlatAlertBox2 = New WindowsApplication5.FlatAlertBox()
        Me.FlatClose1 = New WindowsApplication5.FlatClose()
        Me.FlatMax1 = New WindowsApplication5.FlatMax()
        Me.FlatMini1 = New WindowsApplication5.FlatMini()
        Me.FormSkin1.SuspendLayout()
        Me.FlatTabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.SuspendLayout()
        '
        'FormSkin1
        '
        Me.FormSkin1.BackColor = System.Drawing.Color.Honeydew
        Me.FormSkin1.BaseColor = System.Drawing.Color.AliceBlue
        Me.FormSkin1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.FormSkin1.Controls.Add(Me.FlatTabControl1)
        Me.FormSkin1.Controls.Add(Me.FlatClose1)
        Me.FormSkin1.Controls.Add(Me.FlatMax1)
        Me.FormSkin1.Controls.Add(Me.FlatMini1)
        Me.FormSkin1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FormSkin1.FlatColor = System.Drawing.Color.LightSkyBlue
        Me.FormSkin1.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.FormSkin1.ForeColor = System.Drawing.Color.Cornsilk
        Me.FormSkin1.HeaderColor = System.Drawing.Color.White
        Me.FormSkin1.HeaderMaximize = False
        Me.FormSkin1.Location = New System.Drawing.Point(0, 0)
        Me.FormSkin1.Name = "FormSkin1"
        Me.FormSkin1.Size = New System.Drawing.Size(409, 362)
        Me.FormSkin1.TabIndex = 44
        Me.FormSkin1.Text = "Login"
        '
        'FlatTabControl1
        '
        Me.FlatTabControl1.ActiveColor = System.Drawing.Color.LightSkyBlue
        Me.FlatTabControl1.BaseColor = System.Drawing.Color.FromArgb(CType(CType(45, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(49, Byte), Integer))
        Me.FlatTabControl1.Controls.Add(Me.TabPage1)
        Me.FlatTabControl1.Controls.Add(Me.TabPage2)
        Me.FlatTabControl1.Controls.Add(Me.TabPage3)
        Me.FlatTabControl1.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.FlatTabControl1.ItemSize = New System.Drawing.Size(120, 40)
        Me.FlatTabControl1.Location = New System.Drawing.Point(0, 36)
        Me.FlatTabControl1.Name = "FlatTabControl1"
        Me.FlatTabControl1.Padding = New System.Drawing.Point(3, 2)
        Me.FlatTabControl1.SelectedIndex = 0
        Me.FlatTabControl1.ShowToolTips = True
        Me.FlatTabControl1.Size = New System.Drawing.Size(409, 326)
        Me.FlatTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.FlatTabControl1.TabIndex = 19
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.FromArgb(CType(CType(196, Byte), Integer), CType(CType(199, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.TabPage1.Controls.Add(Me.FlatAlertBox3)
        Me.TabPage1.Controls.Add(Me.Login)
        Me.TabPage1.Controls.Add(Me.pass_Login)
        Me.TabPage1.Controls.Add(Me.user_Login)
        Me.TabPage1.Location = New System.Drawing.Point(4, 44)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(401, 278)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Login"
        '
        'FlatAlertBox3
        '
        Me.FlatAlertBox3.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(73, Byte), Integer))
        Me.FlatAlertBox3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.FlatAlertBox3.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.FlatAlertBox3.kind = WindowsApplication5.FlatAlertBox._Kind.Info
        Me.FlatAlertBox3.Location = New System.Drawing.Point(12, 6)
        Me.FlatAlertBox3.Name = "FlatAlertBox3"
        Me.FlatAlertBox3.Size = New System.Drawing.Size(373, 42)
        Me.FlatAlertBox3.TabIndex = 22
        Me.FlatAlertBox3.Text = "ALERT HERE"
        Me.FlatAlertBox3.Visible = False
        '
        'Login
        '
        Me.Login.BackColor = System.Drawing.Color.Transparent
        Me.Login.BaseColor = System.Drawing.Color.LightSkyBlue
        Me.Login.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Login.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.Login.Location = New System.Drawing.Point(12, 184)
        Me.Login.Name = "Login"
        Me.Login.Rounded = False
        Me.Login.Size = New System.Drawing.Size(373, 43)
        Me.Login.TabIndex = 3
        Me.Login.Text = "LOGIN"
        Me.Login.TextColor = System.Drawing.Color.FromArgb(CType(CType(243, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(243, Byte), Integer))
        '
        'pass_Login
        '
        Me.pass_Login.BackColor = System.Drawing.Color.Transparent
        Me.pass_Login.Location = New System.Drawing.Point(12, 89)
        Me.pass_Login.MaxLength = 32767
        Me.pass_Login.Multiline = False
        Me.pass_Login.Name = "pass_Login"
        Me.pass_Login.ReadOnly = False
        Me.pass_Login.Size = New System.Drawing.Size(373, 29)
        Me.pass_Login.TabIndex = 2
        Me.pass_Login.Text = "Password"
        Me.pass_Login.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.pass_Login.TextColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pass_Login.UseSystemPasswordChar = True
        '
        'user_Login
        '
        Me.user_Login.BackColor = System.Drawing.Color.Transparent
        Me.user_Login.Location = New System.Drawing.Point(12, 54)
        Me.user_Login.MaxLength = 32767
        Me.user_Login.Multiline = False
        Me.user_Login.Name = "user_Login"
        Me.user_Login.ReadOnly = False
        Me.user_Login.Size = New System.Drawing.Size(373, 29)
        Me.user_Login.TabIndex = 1
        Me.user_Login.Text = "User"
        Me.user_Login.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.user_Login.TextColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.user_Login.UseSystemPasswordChar = False
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.FromArgb(CType(CType(196, Byte), Integer), CType(CType(199, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.TabPage2.Controls.Add(Me.FlatAlertBox1)
        Me.TabPage2.Controls.Add(Me.FlatButton1)
        Me.TabPage2.Controls.Add(Me.email_Register)
        Me.TabPage2.Controls.Add(Me.password_Register)
        Me.TabPage2.Controls.Add(Me.user_Register)
        Me.TabPage2.Location = New System.Drawing.Point(4, 44)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(401, 278)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Register"
        '
        'FlatAlertBox1
        '
        Me.FlatAlertBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(73, Byte), Integer))
        Me.FlatAlertBox1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.FlatAlertBox1.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.FlatAlertBox1.kind = WindowsApplication5.FlatAlertBox._Kind.Info
        Me.FlatAlertBox1.Location = New System.Drawing.Point(14, 0)
        Me.FlatAlertBox1.Name = "FlatAlertBox1"
        Me.FlatAlertBox1.Size = New System.Drawing.Size(384, 42)
        Me.FlatAlertBox1.TabIndex = 17
        Me.FlatAlertBox1.Text = "FlatAlertBox1"
        Me.FlatAlertBox1.Visible = False
        '
        'FlatButton1
        '
        Me.FlatButton1.BackColor = System.Drawing.Color.Transparent
        Me.FlatButton1.BaseColor = System.Drawing.Color.LightSkyBlue
        Me.FlatButton1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.FlatButton1.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.FlatButton1.Location = New System.Drawing.Point(14, 153)
        Me.FlatButton1.Name = "FlatButton1"
        Me.FlatButton1.Rounded = False
        Me.FlatButton1.Size = New System.Drawing.Size(376, 61)
        Me.FlatButton1.TabIndex = 16
        Me.FlatButton1.Text = "Register"
        Me.FlatButton1.TextColor = System.Drawing.Color.FromArgb(CType(CType(243, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(243, Byte), Integer))
        '
        'email_Register
        '
        Me.email_Register.BackColor = System.Drawing.Color.Transparent
        Me.email_Register.Location = New System.Drawing.Point(14, 83)
        Me.email_Register.MaxLength = 32767
        Me.email_Register.Multiline = False
        Me.email_Register.Name = "email_Register"
        Me.email_Register.ReadOnly = False
        Me.email_Register.Size = New System.Drawing.Size(376, 29)
        Me.email_Register.TabIndex = 18
        Me.email_Register.Text = "Email"
        Me.email_Register.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.email_Register.TextColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.email_Register.UseSystemPasswordChar = False
        '
        'password_Register
        '
        Me.password_Register.BackColor = System.Drawing.Color.Transparent
        Me.password_Register.Location = New System.Drawing.Point(14, 118)
        Me.password_Register.MaxLength = 32767
        Me.password_Register.Multiline = False
        Me.password_Register.Name = "password_Register"
        Me.password_Register.ReadOnly = False
        Me.password_Register.Size = New System.Drawing.Size(376, 29)
        Me.password_Register.TabIndex = 19
        Me.password_Register.Text = "Password"
        Me.password_Register.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.password_Register.TextColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.password_Register.UseSystemPasswordChar = True
        '
        'user_Register
        '
        Me.user_Register.BackColor = System.Drawing.Color.Transparent
        Me.user_Register.Location = New System.Drawing.Point(14, 48)
        Me.user_Register.MaxLength = 32767
        Me.user_Register.Multiline = False
        Me.user_Register.Name = "user_Register"
        Me.user_Register.ReadOnly = False
        Me.user_Register.Size = New System.Drawing.Size(376, 29)
        Me.user_Register.TabIndex = 20
        Me.user_Register.Text = "User"
        Me.user_Register.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.user_Register.TextColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.user_Register.UseSystemPasswordChar = False
        '
        'TabPage3
        '
        Me.TabPage3.BackColor = System.Drawing.Color.FromArgb(CType(CType(196, Byte), Integer), CType(CType(199, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.TabPage3.Controls.Add(Me.Label2)
        Me.TabPage3.Controls.Add(Me.Label1)
        Me.TabPage3.Controls.Add(Me.FlatAlertBox2)
        Me.TabPage3.Location = New System.Drawing.Point(4, 44)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(401, 278)
        Me.TabPage3.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 101)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 19)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Email"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 70)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 19)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Username -"
        '
        'FlatAlertBox2
        '
        Me.FlatAlertBox2.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(73, Byte), Integer))
        Me.FlatAlertBox2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.FlatAlertBox2.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.FlatAlertBox2.kind = WindowsApplication5.FlatAlertBox._Kind.Success
        Me.FlatAlertBox2.Location = New System.Drawing.Point(36, 25)
        Me.FlatAlertBox2.Name = "FlatAlertBox2"
        Me.FlatAlertBox2.Size = New System.Drawing.Size(357, 42)
        Me.FlatAlertBox2.TabIndex = 0
        Me.FlatAlertBox2.Text = "Succesfully Logged In"
        Me.FlatAlertBox2.Visible = False
        '
        'FlatClose1
        '
        Me.FlatClose1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlatClose1.BackColor = System.Drawing.Color.White
        Me.FlatClose1.BaseColor = System.Drawing.Color.FromArgb(CType(CType(168, Byte), Integer), CType(CType(35, Byte), Integer), CType(CType(35, Byte), Integer))
        Me.FlatClose1.Font = New System.Drawing.Font("Marlett", 10.0!)
        Me.FlatClose1.Location = New System.Drawing.Point(387, 12)
        Me.FlatClose1.Name = "FlatClose1"
        Me.FlatClose1.Size = New System.Drawing.Size(18, 18)
        Me.FlatClose1.TabIndex = 18
        Me.FlatClose1.Text = "FlatClose1"
        Me.FlatClose1.TextColor = System.Drawing.Color.FromArgb(CType(CType(243, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(243, Byte), Integer))
        '
        'FlatMax1
        '
        Me.FlatMax1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlatMax1.BackColor = System.Drawing.Color.White
        Me.FlatMax1.BaseColor = System.Drawing.Color.FromArgb(CType(CType(45, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(49, Byte), Integer))
        Me.FlatMax1.Font = New System.Drawing.Font("Marlett", 12.0!)
        Me.FlatMax1.Location = New System.Drawing.Point(363, 12)
        Me.FlatMax1.Name = "FlatMax1"
        Me.FlatMax1.Size = New System.Drawing.Size(18, 18)
        Me.FlatMax1.TabIndex = 17
        Me.FlatMax1.Text = "FlatMax1"
        Me.FlatMax1.TextColor = System.Drawing.Color.FromArgb(CType(CType(243, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(243, Byte), Integer))
        '
        'FlatMini1
        '
        Me.FlatMini1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlatMini1.BackColor = System.Drawing.Color.White
        Me.FlatMini1.BaseColor = System.Drawing.Color.FromArgb(CType(CType(45, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(49, Byte), Integer))
        Me.FlatMini1.Font = New System.Drawing.Font("Marlett", 12.0!)
        Me.FlatMini1.ForeColor = System.Drawing.Color.Honeydew
        Me.FlatMini1.Location = New System.Drawing.Point(339, 12)
        Me.FlatMini1.Name = "FlatMini1"
        Me.FlatMini1.Size = New System.Drawing.Size(18, 18)
        Me.FlatMini1.TabIndex = 16
        Me.FlatMini1.Text = "FlatMini1"
        Me.FlatMini1.TextColor = System.Drawing.Color.FromArgb(CType(CType(243, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(243, Byte), Integer))
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(409, 362)
        Me.Controls.Add(Me.FormSkin1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.TransparencyKey = System.Drawing.Color.Fuchsia
        Me.FormSkin1.ResumeLayout(False)
        Me.FlatTabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents FormSkin1 As WindowsApplication5.FormSkin
    Friend WithEvents FlatClose1 As WindowsApplication5.FlatClose
    Friend WithEvents FlatMax1 As WindowsApplication5.FlatMax
    Friend WithEvents FlatMini1 As WindowsApplication5.FlatMini
    Friend WithEvents FlatTabControl1 As WindowsApplication5.FlatTabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents FlatAlertBox1 As WindowsApplication5.FlatAlertBox
    Friend WithEvents FlatButton1 As WindowsApplication5.FlatButton
    Friend WithEvents email_Register As WindowsApplication5.FlatTextBox
    Friend WithEvents password_Register As WindowsApplication5.FlatTextBox
    Friend WithEvents user_Register As WindowsApplication5.FlatTextBox
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents FlatAlertBox2 As WindowsApplication5.FlatAlertBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents FlatAlertBox3 As WindowsApplication5.FlatAlertBox
    Friend WithEvents Login As WindowsApplication5.FlatButton
    Friend WithEvents pass_Login As WindowsApplication5.FlatTextBox
    Friend WithEvents user_Login As WindowsApplication5.FlatTextBox

End Class
