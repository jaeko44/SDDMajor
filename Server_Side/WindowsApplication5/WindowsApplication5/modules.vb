Public Class modules
    Public Shared Sub createAlert(ByVal _text As String, alertType As String, Optional alertLoc As String = "notificationsServer")
        Dim newAlert As New FlatAlertBox
        With newAlert
            .BackColor = System.Drawing.Color.FromArgb(CType(CType(196, Byte), Integer), CType(CType(199, Byte), Integer), CType(CType(200, Byte), Integer))
            .Cursor = System.Windows.Forms.Cursors.Hand
            .Font = New System.Drawing.Font("Segoe UI", 10.0!)
            If alertType = Nothing Then
                .kind = FlatAlertBox._Kind.Info
            ElseIf alertType = "success" Then
                .kind = FlatAlertBox._Kind.Success
            ElseIf alertType = "error" Then
                .kind = FlatAlertBox._Kind.Error
            ElseIf alertType = "info" Then
                .kind = FlatAlertBox._Kind.Info
            Else
                .kind = FlatAlertBox._Kind.Info
            End If
            .Location = New System.Drawing.Point(3, 3)
            .Name = _text & "_id" & CInt(Math.Ceiling(Rnd() * 6051001)) + 1
            .Size = New System.Drawing.Size(471, 42)
            .TabIndex = 0
            .Visible = True
            If alertLoc = "notificationsServer" Then
                flatui.notificationsServer.Controls.Add(newAlert)
            ElseIf alertLoc = "alertHome" Then
                flatui.alertHome.Controls.Add(newAlert)
            End If
        End With
    End Sub

    Public Shared Sub resetFlatUI()
        flatui.Close()
        flatui.Show()
    End Sub
    Public Shared Sub COLLECT_DATA()
        Dim startTimer As New TickTock(flatui.activeInterval.Text * 1000)
    End Sub
End Class
Public Class TickTock

    Private WithEvents xTimer As New System.Windows.Forms.Timer

    Public Sub New(TickValue As Integer)
        xTimer = New System.Windows.Forms.Timer
        xTimer.Interval = TickValue
    End Sub

    Public Sub StartTimer()
        xTimer.Start()
    End Sub

    Public Sub StopTimer()
        xTimer.Stop()
    End Sub

    Private Sub Timer_Tick() Handles xTimer.Tick
        SampleProcedure()
    End Sub

    Private Sub SampleProcedure()
        If flatui.serverTabs.SelectedTab Is flatui.serverLoad Then
            xTimer.Interval = flatui.activeInterval.Text
        Else
            xTimer.Interval = flatui.idleInterval.Text
        End If
        flatui.msgServer(flatui.currMachineLoaded, "REQUEST_DATA|UPDATE")
        xTimer.Stop()
        xTimer.Start()
    End Sub

End Class