Public Class ScaleAssistant
	Private Sub New()
	End Sub

	Private width As Integer
	Private height As Integer
	Private control As Form
	Private t1 As Timer
    Private speed As Integer
    Private overrideSpeed As Integer

    Private wait As Long = 0
    Private lastTime As Long

	Public Shared Function Create(form As Form, Optional speed As Integer = 1) As ScaleAssistant
		Dim assist As New ScaleAssistant
		assist.control = form
		assist.width = form.Width
		assist.height = form.Height
        assist.speed = speed
        assist.lastTime = DateTime.Now.Ticks

		' Create timer
		assist.t1 = New Timer
		assist.t1.Interval = 1
		AddHandler assist.t1.Tick, AddressOf assist.Update
        assist.t1.Start()

		Return assist
	End Function

    Public Sub SetSize(width As Integer, height As Integer, delay As Integer, Optional overrideSpeed As Integer = -1)
        Me.width = width
        Me.height = height
        Me.overrideSpeed = overrideSpeed

        wait += delay
    End Sub

    Private Sub Update(sender As Object, e As EventArgs)
        Dim nowTime As Long = DateTime.Now.Ticks

        If wait > 0 Then
            wait -= (nowTime - lastTime)

            Return
        Else
            wait = 0
        End If

        lastTime = nowTime

        If Not control.Width = width Then
            Dim direction As Integer = IIf(control.Width > width, -1, 1)
            control.Width += direction * IIf(overrideSpeed > 0, overrideSpeed, speed)

            If (control.Width > width And direction > 0) Or (control.Width < width And direction < 1) Then
                control.Width = width
            End If
        End If

        If Not control.Height = height Then
            Dim direction As Integer = IIf(control.Height > height, -1, 1)
            control.Height += direction * IIf(overrideSpeed > 0, overrideSpeed, speed)

            If (control.Height > height And direction > 0) Or (control.Height < height And direction < 1) Then
                control.Height = height
            End If

            If control.Width = width And control.Height = height Then
                overrideSpeed = -1
            End If
        End If
    End Sub
End Class
