Imports com.shephertz.app42.paas.sdk.csharp
Imports com.shephertz.app42.paas.sdk.csharp.user
Imports com.shephertz.app42.paas.sdk.csharp.storage

Public Class Form1

    Public userName_ As String
    Public passWord_ As String
    Public emailId_ As String
    Public devModeEnabled As Boolean
    Private Sub FlatButton1_Click(sender As Object, e As EventArgs)

    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        devModeEnabled = True
    End Sub
    Private Sub FlatButton1_Click_1(sender As Object, e As EventArgs) Handles FlatButton1.Click
        'Initializes the connection with http://api.shephertz.com/
        'API Key, Secret Key (Achieved keys from - https://apphq.shephertz.com/service/deployApp)
        'For further documentation - Access https://apphq.shephertz.com with the following accounts -
        'Username: pavilion.vps@gmail.com
        'Password: march17
        Try
            If devModeEnabled = True Then
                login_Process()
            Else
                App42API.Initialize("1e9e2f082947488669b233b36fdfbb05ee6f6bb5d7f363e7db0c4db910c5eaf4", "94c60ab123425768da7926778802b7d056e0f1ac9cf0b682877f4d1f399ed0a1")
                Dim userService As UserService = App42API.BuildUserService()
                'Following strings are Required Parameters to process the API Callback. 
                Dim userName As [String] = user_Register.Text
                Dim passWord As [String] = password_Register.Text             'Identify userName/passWord/emailId variables by introducing them.
                Dim emailId As [String] = email_Register.Text
                'User is created through following code
                Dim user As User = userService.CreateUser(userName, passWord, emailId)
                'Sets the strings with data.
                userName_ = user.GetUserName()
                passWord_ = user.GetPassword()
                emailId_ = user.GetEmail()
                Dim jsonResponse As [String] = user.ToString()
                MessageBox.Show(jsonResponse) 'DEBUG (REMOVE WHEN NECESSARY)
                If jsonResponse.ToString.Contains("success") Then
                    'Login Code Here (Use userName_/passWord in order to login)
                End If
            End If
        Catch ex As Exception
            FlatAlertBox1.Text = "Failure cuz Failure"
            MessageBox.Show(ex.ToString)
            'Above is just DEBUG, REMOVE ONCE DONE M8
            FlatAlertBox1.Visible = True
            If ex.ToString.Contains("EmailAddress is Not Valid") Then
                FlatAlertBox1.Text = "Email Address is invalid"
                FlatAlertBox1.kind.Equals("Error")
                FlatAlertBox1.Visible = True
            End If
            If ex.ToString.Contains("invalid. Username '") Then
                FlatAlertBox1.Text = "Username already exists"
                FlatAlertBox1.kind.Equals("Error")
                FlatAlertBox1.Visible = True
            End If
        End Try

    End Sub
    Private Sub Login_Click(sender As Object, e As EventArgs) Handles Login.Click
        Try
            If devModeEnabled = True Then
                MessageBox.Show("Due to public networking being disabled, your account will load directly from local file.")
                login_Process()
            Else
                App42API.Initialize("1e9e2f082947488669b233b36fdfbb05ee6f6bb5d7f363e7db0c4db910c5eaf4", "94c60ab123425768da7926778802b7d056e0f1ac9cf0b682877f4d1f399ed0a1")
                Dim userService As UserService = App42API.BuildUserService()

                Dim userName As [String] = user_Login.Text
                Dim passWord As [String] = pass_Login.Text
                Dim user As User = userService.Authenticate(userName, passWord)
                Dim jsonResponse As [String] = user.ToString()
                MessageBox.Show(jsonResponse) 'DEBUG (REMOVE WHEN NECESSARY)
                If jsonResponse.ToString.Contains("success") Then
                    login_Process()
                    'Login Code Here (Use userName_/passWord in order to login)
                Else
                    MessageBox.Show("Fail?")
                End If
            End If
        Catch ex As Exception
            If ex.ToString.Contains("UserName/Password did not match") Then
                FlatAlertBox3.Text = "Username or password is wrong"
                FlatAlertBox3.kind.Equals("Error")
                FlatAlertBox3.Visible = True
            End If
        End Try
    End Sub

    Private Sub login_Process()
        Me.Visible = False
        flatui.Visible = True
    End Sub

    Private Sub TabPage3_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub user_Register_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub

    Private Sub FormSkin1_Click(sender As Object, e As EventArgs) Handles FormSkin1.Click

    End Sub
End Class
