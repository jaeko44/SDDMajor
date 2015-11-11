Imports System.IO
Imports Newtonsoft.Json
Imports System.Threading


Public Class flatui
    Public _Client As New tcpCommClient(AddressOf UIHandler)
    Public IsClosing As Boolean = False
    Public client As New TcpComm.Client(AddressOf UIHandler, True, 30)
    Public lat As TcpComm.Utilities.LargeArrayTransferHelper
    'Movement: Begin: 
    'DOCOUMENTATION:  Movement Variables.
    Dim drag As Boolean
    Dim mousex As Integer
    Dim mousey As Integer
    Dim sizer As ScaleAssistant

    'Movement End
    'Load Servers: Begin:
    Dim jsonData As List(Of serverList)
    Dim open As Boolean = False
    Dim firstload As Boolean = True
    Dim LabelLocation(0 To 100) As Point
    Dim ButtonLocation(0 To 100) As Point
    Dim generateOnSpot As Boolean = True
    Dim totalServers As Integer = 0
    Dim currOpenint As Integer = 0
    'Load Servers: :End
    'Current Page'
    Dim currPage As Integer = 1
    Dim lastPage As Boolean = False
    Dim firstPage As Boolean = False

    'Second Servers: Begin:
    Dim json As Data

    'Current Machine Loaded
    Public currMachineLoaded As Integer = Nothing

    'Collect Data Timer
    Public _keepGoing As Boolean = False
    'Path to save to
    Dim path_to_save As String = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory) & "\ReceivedFiles"
    Private Function BytesToString(ByVal data() As Byte) As String
        Dim enc As New System.Text.UTF8Encoding()
        BytesToString = enc.GetString(data)
    End Function
    Private Sub flatui_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Size = New Size(483, 584)
        sizer = ScaleAssistant.Create(Me, 50)
        loadJsonData()
        loadInterface()
        labelsReload()
        folderDirectory.Text = path_to_save

    End Sub
    Private Sub loadJsonData()
        'Loaded Succesfully
        Dim curFile As String = "data.json"
        Dim jsonString2 As String
        If File.Exists(curFile) Then
            jsonString2 = File.ReadAllText("data2.json")
            json = JsonConvert.DeserializeObject(Of Data)(jsonString2)
        Else
            If (jsonString2 Is Nothing) Then


            End If
        End If

        ' MessageBox.Show("hi")
    End Sub


    Private Sub handleTop_MouseMove(sender As Object, e As MouseEventArgs) Handles handleTOP.MouseMove, titleMovable.MouseMove
        'If drag is set to true then move the form accordingly.
        'This following code allows the FORM To be moved around when clicking the top of the form (Since FormBorderStle = None) 
        If drag Then
            Me.Top = Windows.Forms.Cursor.Position.Y - mousey
            Me.Left = Windows.Forms.Cursor.Position.X - mousex
        End If
    End Sub

    Private Sub PictureBox2_MouseUp(sender As Object, e As MouseEventArgs) Handles handleTOP.MouseUp
        drag = False 'Sets drag to false, so the form does not move according to the code in MouseMove

    End Sub

    Private Sub PictureBox2_MouseDown(sender As Object, e As MouseEventArgs) Handles handleTOP.MouseDown
        drag = True 'Sets the variable drag to true.
        mousex = Windows.Forms.Cursor.Position.X - Me.Left 'Sets variable mousex
        mousey = Windows.Forms.Cursor.Position.Y - Me.Top 'Sets variable mousey
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles cancelCon.Click
        serverTabs.SelectTab(serverList)
        sizer.SetSize(488, 549, 100)
        closeAllConnections(True)
    End Sub
    Private Sub goback1_sub(sender As Object, e As EventArgs) Handles goBack1.Click
        serverTabs.SelectTab(serverLoad)
    End Sub

    Private Sub loadServer(id As Integer)

    End Sub
    Private Sub TabPage5_Click(sender As Object, e As EventArgs) Handles serverList.Click

    End Sub

    Private Sub loadInterface()
        'Sets the location of TabControl -> So that it is hidden above the top picturebox (Which handles draging the application)
        serverTabs.Location = New Point(0, 0)
        ' The following are the parameters for the interface, which include sizes, id's, locations, and margin parameters
        Dim labelID As Integer = 0
        Dim fixedID As Integer = 0
        Dim layer As Integer = 0
        Dim PosX As Integer = 39
        Dim PosY As Integer = 42
        Dim PosX_name_of_id As Integer = 16
        Dim PosX_name_of_id_result As Integer = 147
        Dim PosY_name_of_id As Integer = -23
        Dim PosX_ip_of_id As Integer = 16
        Dim PosX_ip_of_id_result As Integer = 88
        Dim PosY_ip_of_id As Integer = 1
        Dim PosX_bg_of_id As Integer = -4
        Dim PosY_bg_of_Id As Integer = -47
        Dim PosY_button_of_id As Integer = -47
        Dim increase As Integer = 95
        'For each data in -> Json.Servers. This loops around all the data inside json.srevers (which is loaded from json database)
        For Each data As servers In json.servers
            'Increases the labelID by 1 every time it is looped. LabelID always starts with 1 -> Whereas JSON parameters always begin with 0.
            labelID += 1
            'Each layer acts as a page -:> Since each page only holds 4 results, layers reset back to 1 -> So that the location (Y generally) can handle the 5TH result
            'as if it is located as the 1st Result. Since it is hidden, it's there but it's not because it's invisible. 
            'However this acts as a barrier for long-term performance -> If a user has over 1000's of servers in place. Loading all of them within the memory could cause
            'memory leaks and issues. 
            layer += 1
            If layer = 5 Then
                layer = 1
            End If
            'The following repositions the Y for each of the objects so that they are placed with an additioanl margin. 
            Dim PosY_ip_of_id_fixed As Integer = PosY_ip_of_id + increase * layer
            Dim PosY_name_of_id_fixed As Integer = PosY_name_of_id + increase * layer
            Dim PosY_bg_of_Id_fixed As Integer = PosY_bg_of_Id + increase * layer
            Dim PosY_button_of_Id_fixed As Integer = PosY_button_of_id + increase * layer
            'The following code creates a picturebox called STATUS_OF_ID, this is the color underneath each server which determines if it is online or offline
            'This generally acts as feedback to the user and nothing else.
            Dim status_of_id As New PictureBox
            With status_of_id
                .Location = New System.Drawing.Point(PosX_bg_of_id, PosY_bg_of_Id_fixed + 80)
                .Size = New System.Drawing.Size(480, 15)
                .Name = "status_of_" & labelID
                If data.isOnline = True Then
                    .BackColor = System.Drawing.Color.SpringGreen
                Else
                    .BackColor = System.Drawing.Color.Crimson
                End If
                If labelID > 4 Then 'This IF Statement hides the variable if it is above the first page (4 values per page) thus making it hidden.
                    .Visible = False
                End If
            End With
            'This creates a background for each data so they are seperated.
            Dim bg_of_id As New PictureBox
            With bg_of_id
                .Location = New System.Drawing.Point(PosX_bg_of_id, PosY_bg_of_Id_fixed)
                .Size = New System.Drawing.Size(479, 91)
                .BackColor = System.Drawing.Color.WhiteSmoke
                .Name = "bg_of_" & labelID
            End With
            'This creates the 'Hostname' parameter which displays the servers hostname. An identifiable FDN (Full Domain Name) or simple Name (Both can be used)
            'Generally public servers have a hostname which allows them to be identified by a domain name instead of a full IPv4 Address since they are harder to memorize.
            Dim name_of_id As New Label
            With name_of_id
                .Location = New System.Drawing.Point(PosX_name_of_id, PosY_name_of_id_fixed) 'Sets the location of the object
                .Name = "name_of_" & labelID 'Sets the identifiable name with an ID so they can later be hidden and unhidden depending on pages
                .Size = New System.Drawing.Size(125, 24) 'Sets the objects size using System.Drawing.Size(Width, Height)
                .BackColor = System.Drawing.Color.WhiteSmoke 'Sets the objects backcolor using a System.Drawing.Color
                .Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte)) 'Sets the font of this label.
                .Text = "Hostname:" 'This sets the text for this label, this is a static label (not dynamic) -> so it doesn't load any data from a database.
                .TabIndex = 0 'Gives it a tab index of 0 since it is not really an input.
                If labelID > 4 Then
                    .Visible = False
                End If
            End With
            'This returns the hostname by loading it frmo the JSonFile -> Exactly loaded from json -> servers -> connection -> hostname as string
            Dim name_of_id_result As New Label
            With name_of_id_result
                .Location = New System.Drawing.Point(PosX_name_of_id_result, PosY_name_of_id_fixed)
                .Name = "name_of_" & labelID & "_result"
                .Size = New System.Drawing.Size(200, 24)
                .BackColor = System.Drawing.Color.WhiteSmoke

                .Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                .Text = data.connection.hostname 'This sets the text for this label using dynamic data collected from Json Data
                .TabIndex = 0
                If labelID > 4 Then
                    .Visible = False
                End If
            End With
            'IPv4 are the main backbone to connecting to a server, they are unique adresses that are assigned to specific routers which lead a connection to a singular server.
            'Each server has its own IPv4, thus this is an important label to be displayed in the server list since they are uniquely able to be identified.
            'This is also important to be saved as IPv4 are needed to miake a TCPC connection.
            Dim ip_of_id As New Label
            With ip_of_id
                .Location = New System.Drawing.Point(PosX_name_of_id, PosY_ip_of_id_fixed) 'set your location
                .Name = "ip_of_" & labelID
                .Size = New System.Drawing.Size(125, 24) 'Setting Label Size
                .BackColor = System.Drawing.Color.WhiteSmoke

                .Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                .Text = "IP Address:" 'set the text for your label
                .TabIndex = 0
                If labelID > 4 Then
                    .Visible = False
                End If
            End With
            Dim ip_of_id_result As New Label
            With ip_of_id_result
                .Location = New System.Drawing.Point(PosX_name_of_id_result, PosY_ip_of_id_fixed) 'set your location
                .Name = "ip_of_" & labelID & "_result"
                .Size = New System.Drawing.Size(200, 24) 'Setting Label Size
                .BackColor = System.Drawing.Color.WhiteSmoke

                .Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                .Text = data.connection.ip_address 'set the text for your label
                .TabIndex = 0
                If labelID > 4 Then
                    .Visible = False
                End If
            End With
            Dim button_of_id_load As New Button
            With button_of_id_load
                .Name = "button_of_" & labelID & "_load"

                .TabIndex = labelID 'Since this is a button which can use input, it is best to give it a tabindex for extra functionality, allowing users to literate through the buttons using tab.
                .Tag = labelID ' This is a unique paramter only set for this button. In order to allow future use of this BUTTON we set a tag with
                'the ID of this server. Thus when required to be used in the future, the TAG is collected when the button is clicked -> allowing for dynamicly loading the 
                'required machine
                'The following code handles visuals for the button -> Including Size, Text, Location, color, style and backcolor.
                .Text = ">>>"
                .BackColor = System.Drawing.Color.LightSkyBlue
                .FlatStyle = System.Windows.Forms.FlatStyle.Flat
                .ForeColor = System.Drawing.Color.Transparent
                .Size = New System.Drawing.Size(27, 91)
                .Location = New System.Drawing.Point(450, PosY_button_of_Id_fixed)
                .UseVisualStyleBackColor = False
                If labelID > 4 Then
                    .Visible = False
                End If
            End With
            AddHandler button_of_id_load.Click, AddressOf loadServ 'Adds a handler to the newly created button
            'The following code adds all the precreated objects into the list directly, However - It is important to look at the layering off the following code
            'Having the bg add last allows it to become a 'background'
            serverList.Controls.Add(ip_of_id_result)
            serverList.Controls.Add(button_of_id_load)
            serverList.Controls.Add(ip_of_id)
            serverList.Controls.Add(name_of_id_result)
            serverList.Controls.Add(name_of_id)
            serverList.Controls.Add(bg_of_id)
            serverList.Controls.Add(status_of_id)
        Next
        totalServers = labelID
        '     objectLocations()
        '     Panel1.AutoScroll = True
    End Sub


    Private Sub MyClosing(sender As Object, e As EventArgs) Handles Me.FormClosing
        File.WriteAllText("data.json", JsonConvert.SerializeObject(jsonData)) 'Starts SAVING Stuff when THE FORM IS CLOSING
        Dim tmpJSON As String = JsonConvert.SerializeObject(json)
        File.WriteAllText("data2.json", tmpJSON)
        tmrPoll.Enabled = False
        IsClosing = True
        If client IsNot Nothing AndAlso client.isClientRunning Then

            client.Close(True)
            While client.isClientRunning
                Application.DoEvents()
            End While
        End If
    End Sub

    Private Sub loadServ(sender As Object, e As EventArgs)
        Dim btn As Button = CType(sender, Button) 'Converts 
        Load_machine(btn.Tag.ToString)
    End Sub

    Private Sub Load_machine(id As Integer)
        serverTabs.SelectTab(serverLoad)

        'SetSize(width, height, OPTIONAL Speed Override) 
        'Example: sizer.SetSize(50, 50) or sizer.SetSize (50,50, 10)
        sizer.SetSize(549, 584, 10, 10)
        Dim jsonID As Integer = id - 1
        currMachineLoaded = id
        ip_result.Text = json.servers(jsonID).connection.ip_address
        hostname.Text = json.servers(jsonID).connection.hostname
        os_result.Text = json.servers(jsonID).serverType
        cpu_result.Text = json.servers(jsonID).compute.cpu
        ram_val.Text = json.servers(jsonID).compute.ram & "MB"
        hdd_val.Text = CInt(json.servers(jsonID).compute.hdd) / 1024 & "GB"
        Connect_To_Server(jsonID)
        Dim bg_of_connection As PictureBox = CType(serverLoad.Controls("bg_of_conn"), PictureBox) 'Since 'connecting_delay' isn't added until the following code is processed. We can't simply just detect if it was ever 
        'created since the software won't compile in the first place (Due to it not being accessable). This way we create a null PictureBox with the name "connecting_delay" in order to detect if it is ever there or not
        If bg_of_connection Is Nothing Then 'If connection delay picturebox doesn't exist -> Then it makes a new one, otherwise it simply makes it visible until connected. - If it fails to connect then its never removed
            Dim bg_of_conn As New PictureBox
            With bg_of_conn
                .BackColor = System.Drawing.Color.WhiteSmoke 'Sets the color
                .Enabled = False
                .Location = New System.Drawing.Point(28, 0) 'Sets the location
                .Name = "bg_of_conn"
                .Size = New System.Drawing.Size(517, 499)
                .TabIndex = 85
                .TabStop = False
                .Visible = True
            End With
            serverLoad.Controls.Add(bg_of_conn) 'Adds this directly to ServerLoad, So it only hides the ServerLoad page -> Since the serverload page needs to be hidden until the server is completely loaded.
            bg_of_conn.BringToFront()
        Else
            bg_of_connection.Visible = True
        End If
        Dim label_of_connection As Label = CType(serverLoad.Controls("label_of_conn"), Label)
        If label_of_connection Is Nothing Then
            Dim label_of_conn As New Label
            With label_of_conn
                .AutoSize = True
                .Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                .Location = New System.Drawing.Point(126, 178) '126, 178
                .BackColor = System.Drawing.Color.WhiteSmoke
                .Name = "label_of_conn"
                .Size = New System.Drawing.Size(320, 55)
                .TabIndex = 108
                .Text = "Connecting..."
                .Visible = True
            End With
            serverLoad.Controls.Add(label_of_conn)
            label_of_conn.BringToFront()
            label_of_connection = CType(serverLoad.Controls("label_of_conn"), Label) 'Reinitializes the String
        Else
            label_of_connection.Visible = True
            label_of_connection.Text = "Connecting..."
        End If
        Dim label_of_connection2 As Label = CType(serverLoad.Controls("label_of_conn2"), Label)
        If label_of_connection2 Is Nothing Then
            Dim label_of_conn2 As New Label
            With label_of_conn2
                .AutoSize = True
                .Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                .Location = New System.Drawing.Point(126, 260) '126, 178
                .BackColor = System.Drawing.Color.WhiteSmoke
                .Name = "label_of_conn2"
                .Size = New System.Drawing.Size(100, 25)
                .TabIndex = 108
                .Text = "Please wait patiently"
                .Visible = True
            End With
            serverLoad.Controls.Add(label_of_conn2)
            label_of_conn2.BringToFront()
            label_of_connection2 = CType(serverLoad.Controls("label_of_conn2"), Label) 'Reinitializes the String
        End If


    End Sub

    Private Sub newMachine_Click(sender As Object, e As EventArgs) Handles newMachine.Click
        serverTabs.SelectTab(srvAdd)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles rightPage.Click
        Try
            Dim reset As Integer = currPage * 4 'HANDLES RIGHT Page 
            For i As Integer = reset - 3 To reset 'Resets the following SERVERS

                Dim myButton As Button = CType(serverList.Controls("button_of_" & i & "_load"), Button)

                Dim ip_label As Label = CType(serverList.Controls("ip_of_" & i), Label)
                Dim name_label As Label = CType(serverList.Controls("name_of_" & i), Label)

                Dim ip_result As Label = CType(serverList.Controls("ip_of_" & i & "_result"), Label)
                Dim name_result As Label = CType(serverList.Controls("name_of_" & i & "_result"), Label)

                Dim bg_of_id As PictureBox = CType(serverList.Controls("bg_of_" & i), PictureBox)
                Dim status_of_id As PictureBox = CType(serverList.Controls("status_of_" & i), PictureBox)

                myButton.Visible = False
                ip_label.Visible = False
                name_label.Visible = False
                ip_result.Visible = False
                name_result.Visible = False
                bg_of_id.Visible = False
                status_of_id.Visible = False
                If i + 3 > totalServers Then
                    lastPage = True
                    Continue For
                End If
            Next
            Dim show As Integer = currPage * 4 + 1 'Shows the FOLLOWING Servers
            For i = show To show + 3
                If i > totalServers Then
                    lastPage = True
                    Continue For
                End If
                Dim myButton As Button = CType(serverList.Controls("button_of_" & i & "_load"), Button)

                Dim ip_label As Label = CType(serverList.Controls("ip_of_" & i), Label)
                Dim name_label As Label = CType(serverList.Controls("name_of_" & i), Label)

                Dim ip_result As Label = CType(serverList.Controls("ip_of_" & i & "_result"), Label)
                Dim name_result As Label = CType(serverList.Controls("name_of_" & i & "_result"), Label)

                Dim bg_of_id As PictureBox = CType(serverList.Controls("bg_of_" & i), PictureBox)
                Dim status_of_id As PictureBox = CType(serverList.Controls("status_of_" & i), PictureBox)

                myButton.Visible = True
                ip_label.Visible = True
                name_label.Visible = True
                ip_result.Visible = True
                name_result.Visible = True
                bg_of_id.Visible = True
                status_of_id.Visible = True
            Next

            currPage += 1 'Logs that it is next page

            If currPage > 1 Then 'Enabled left page if page is above 1
                leftPage.Enabled = True
                leftPage.BackColor = Color.LightSkyBlue
            Else
                leftPage.Enabled = False
                leftPage.BackColor = Color.Gray
            End If
            Dim totalPages As Integer = Math.Ceiling(totalServers / 4) 'Calculates the total pages
            If currPage >= totalPages And rightPage.Enabled = True And lastPage = True Then
                rightPage.BackColor = Color.Gray
                rightPage.Enabled = False
                lastPage = False
                labelsReload()
                Return
            End If

            labelsReload()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub leftPage_Click(sender As Object, e As EventArgs) Handles leftPage.Click
        Try
            Dim reset As Integer = currPage * 4 'Handles left page
            Dim start As Integer = reset - 3
            If reset > totalServers Then reset = totalServers 'If reset (Curr page * 4) is more than total servers, then it sets reset to totalservers so it doesn't go over the amount of servers available

            For i As Integer = start To reset

                Dim myButton As Button = CType(serverList.Controls("button_of_" & i & "_load"), Button)

                Dim ip_label As Label = CType(serverList.Controls("ip_of_" & i), Label)
                Dim name_label As Label = CType(serverList.Controls("name_of_" & i), Label)

                Dim ip_result As Label = CType(serverList.Controls("ip_of_" & i & "_result"), Label)
                Dim name_result As Label = CType(serverList.Controls("name_of_" & i & "_result"), Label)

                Dim bg_of_id As PictureBox = CType(serverList.Controls("bg_of_" & i), PictureBox)
                Dim status_of_id As PictureBox = CType(serverList.Controls("status_of_" & i), PictureBox)


                myButton.Visible = False
                ip_label.Visible = False
                name_label.Visible = False
                ip_result.Visible = False
                name_result.Visible = False
                bg_of_id.Visible = False
                status_of_id.Visible = False
            Next
            currPage -= 1
            labelsReload()

            Dim show As Integer = currPage * 4 - 3
            For i = show To show + 3
                Dim myButton As Button = CType(serverList.Controls("button_of_" & i & "_load"), Button)

                Dim ip_label As Label = CType(serverList.Controls("ip_of_" & i), Label)
                Dim name_label As Label = CType(serverList.Controls("name_of_" & i), Label)

                Dim ip_result As Label = CType(serverList.Controls("ip_of_" & i & "_result"), Label)
                Dim name_result As Label = CType(serverList.Controls("name_of_" & i & "_result"), Label)

                Dim bg_of_id As PictureBox = CType(serverList.Controls("bg_of_" & i), PictureBox)
                Dim status_of_id As PictureBox = CType(serverList.Controls("status_of_" & i), PictureBox)


                myButton.Visible = True
                ip_label.Visible = True
                name_label.Visible = True
                ip_result.Visible = True
                name_result.Visible = True
                bg_of_id.Visible = True
                status_of_id.Visible = True
            Next

            If lastPage = True Or rightPage.BackColor = Color.Gray Or rightPage.Enabled = False Then
                rightPage.BackColor = Color.LightSkyBlue
                rightPage.Enabled = True
                lastPage = False
            End If

            If currPage > 1 Then
                leftPage.Enabled = True
                leftPage.BackColor = Color.LightSkyBlue
            Else
                leftPage.Enabled = False
                leftPage.BackColor = Color.Gray
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub labelsReload() 'Reloads the page labels after a page goes up or down
        Dim totalPages As Integer = Math.Ceiling(totalServers / 4)
        If totalPages <= 1 Then
            rightPage.Visible = False
            leftPage.Visible = False
            currPageLab.Visible = False
        Else
            currPageLab.Text = "Page " & currPage.ToString & "/" & totalPages.ToString.First & "."
        End If
    End Sub
    Private Sub frmClient_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        client.SetReceivedFilesFolder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\ClientReceivedFiles")
        tmrPoll.Start()
    End Sub
    Private Sub Connect_To_Server(idOfSrv As Integer, Optional IP As String = Nothing)
        Try
            Dim s As Integer = json.servers(idOfSrv).connection.port.ToString
            Dim errMsg As String = ""
            Dim IPv4 As String = json.servers(idOfSrv).connection.ip_address.ToString
            If IsNothing(IPv4) = True Then
                IPv4 = "10.0.0.9"
                s = 22490
            End If
            client = New TcpComm.Client(AddressOf UIHandler, False, 30)
            If Not client.Connect(IPv4.Trim, Convert.ToInt32(s), Nothing, errMsg) Then 'Connects to the Client (Using IPv4.Trim) -> Port = S, (Nothing for Machine ID), and ERRMSG return value to handle errmsgs's.
                If errMsg.Trim <> "" Then MsgBox(errMsg, MsgBoxStyle.Critical, "Test Tcp Communications App")
            End If
            If Not client.SendText("REQUEST_DATA|ALL", , errMsg) Then MsgBox(errMsg, MsgBoxStyle.Critical) ' Requests DATA on connect -> ALL
            If updateResources.Checked = True Then
                _keepGoing = True
                modules.COLLECT_DATA()
            End If

            ' _Client.Connect(IPv4, s)
            '   _Client.StopRunning()
            ' Me.conOrdis.Text = "Connect"
        Catch ex As Exception
            MessageBox.Show(ex.Message) 'Catches any exceptions again
        End Try
    End Sub

    Public Sub msgServer(idOfSrv As Integer, Message As String)
        If idOfSrv = Nothing Then
            _keepGoing = False
        Else
            Dim jsonID = idOfSrv - 1
            Dim s As Integer = json.servers(jsonID).connection.port.ToString 'Send message only if IDOfSrv is Actually not nothing
            Dim errMsg As String = ""
            Dim IPv4 As String = json.servers(jsonID).connection.ip_address.ToString
            If Not client.SendText(Message, , errMsg) Then MsgBox(errMsg, MsgBoxStyle.Critical) ' Requests DATA on connect -> ALL
        End If
    End Sub

    Private Function StrToByteArray(ByVal text As String) As Byte() 'Converts a STR into BYTE ARRAY So it can send packets without loss of DATA
        Dim encoding As New System.Text.UTF8Encoding()
        StrToByteArray = encoding.GetBytes(text)
    End Function


    Public Sub UIHandler(ByVal bytes() As Byte, ByVal dataChannel As Integer) 'HANDLES all UI changes

        If Me.InvokeRequired() Then
            ' InvokeRequired: We're running on the background thread. Invoke the delegate.
            Me.Invoke(_Client.ClientCallbackObject, bytes, dataChannel)
        Else
            ' We're on the main UI thread now.
            Dim dontReport As Boolean = False ' If this is true => Then it doesn't report it.

            If dataChannel < 251 Then 'All datachannels under 251 are simple messages and go through the following code.
                Dim msg As String = BytesToString(bytes) 'Receives the MSG and coverts into String (BytesToString)
                ' MsgBox("Datachannel (type of message)" & dataChannel & "Message: " & msg)
                'Detects what type of message it is.
                If msg.StartsWith("CMDResponse") Then
                    dontReport = True
                    sendCMD.Text = "Send"
                    msg = msg.Replace("CMDResponse|", "")
                    cmdPrompt.AppendText(Environment.NewLine & msg) 'NewLine ensures that msg isn't added to a current line. This code adds command result to CMDBox
                ElseIf msg.StartsWith("OSVersion") Then
                    dontReport = True
                    msg = msg.Replace("OSVersion|", "")
                    os_result.Text = msg
                    If IsNothing(currMachineLoaded) = False Then
                        json.servers(currMachineLoaded).serverType = msg
                    End If
                ElseIf msg.StartsWith("CPUUsage") Then 'CHECKS What TYPE OF Response IT IS And Proceeds ACCORDINGLY
                    dontReport = True
                    msg = msg.Replace("CPUUsage|", "")
                    If IsNumeric(msg) = True And msg < 100 Then
                        cpu.Value = msg
                    End If
                ElseIf msg.StartsWith("CPUTYPE") Then
                    msg = msg.Replace("CPUUsage|", "")
                    json.servers(currMachineLoaded).compute.cpu = msg
                    cpu_result.Text = msg
                ElseIf msg.StartsWith("RAMUsage") Then
                    dontReport = True
                    msg = msg.Replace("RAMUsage|", "")
                    'MessageBox.Show(msg)
                    If IsNumeric(msg) = True And msg <= 100 Then
                        ram.Value = msg
                    End If
                ElseIf msg.StartsWith("TOTALRam") Then
                    msg = msg.Replace("TOTALRam|", "")
                    json.servers(currMachineLoaded).compute.ram = msg
                    ram_val.Text = msg & "MB"
                ElseIf msg.StartsWith("HDDUsage") Then
                    dontReport = True
                    msg = msg.Replace("HDDUsage|", "")
                    'MessageBox.Show("HDD Usage : " & msg)
                    If IsNumeric(msg) = True And msg <= 100 Then
                        hdd.Value = msg
                    End If
                ElseIf msg.StartsWith("TOTALSize") Then
                    msg = msg.Replace("TOTALSize|", "")
                    json.servers(currMachineLoaded).compute.hdd = msg
                    hdd_val.Text = Math.Ceiling((msg) / 1024) + 1 & "GB"

                End If

            ElseIf dataChannel = 255 Then
                Dim msg As String = BytesToString(bytes)
                Dim tmp As String = ""
                If msg.ToString.StartsWith("Discon") Then
                    dontReport = True

                    closeAllConnections()
                    Return
                ElseIf msg.ToString.StartsWith("Conn") Then
                    dontReport = True
                    MessageBox.Show("Succesfully connected to server")

                    Dim label_of_connection2 As Label = CType(serverLoad.Controls("label_of_conn2"), Label)

                    Dim label_of_connection As Label = CType(serverLoad.Controls("label_of_conn"), Label)
                    Dim bg_of_connection As PictureBox = CType(serverLoad.Controls("bg_of_conn"), PictureBox)


                    If IsNothing(label_of_connection) = False Then
                        label_of_connection.Visible = False
                    End If
                    If IsNothing(bg_of_connection) = False Then
                        bg_of_connection.Visible = False
                    End If
                    If IsNothing(label_of_connection2) = False Then
                        label_of_connection2.Visible = False

                    End If


                End If
                ' Display info about the incoming file:
                If msg.Length > 15 Then tmp = msg.Substring(0, 15)
                If tmp = "Receiving file:" Then
                    MessageBox.Show("Receiving: " & _Client.GetIncomingFileName)
                    dontReport = True
                End If

                ' Display info about the outgoing file:
                If msg.Length > 13 Then tmp = msg.Substring(0, 13)
                If tmp = "Sending file:" Then
                    MessageBox.Show("Sending: " & _Client.GetOutgoingFileName)
                    dontReport = True
                End If

                ' The file being sent to the client is complete.
                If msg = "->Done" Then
                    MessageBox.Show("File->Client: Transfer complete.")
                    '  btGetFile.Text = "Get File"
                    dontReport = True
                End If

                ' The file being sent to the server is complete.
                If msg = "<-Done" Then
                    MessageBox.Show("File->Server: Transfer complete.")
                    '  btSendFile.Text = "Send File"
                    dontReport = True
                End If

                ' The file transfer to the client has been aborted.
                If msg = "->Aborted." Then
                    MessageBox.Show("File->Client: Transfer aborted.")
                    dontReport = True
                End If

                ' The file transfer to the server has been aborted.
                If msg = "<-Aborted." Then
                    MessageBox.Show("File->Server: Transfer aborted.")
                    dontReport = True
                End If

                ' _Client as finished sending the bytes you put into sendBytes()
                If msg.Length > 4 Then tmp = msg.Substring(0, 4)
                If tmp = "UBS:" Then ' User Bytes Sent on channel:???.
                    'btSendText.Enabled = True
                    dontReport = True
                End If

                ' We have an error message. Could be local, or from the server.
                If msg.Length > 4 Then tmp = msg.Substring(0, 5)
                If tmp = "ERR: " Then
                    Dim msgParts() As String
                    msgParts = Split(msg, ": ")
                    MsgBox("" & msgParts(1), MsgBoxStyle.Critical, "Test Tcp Communications App")
                    dontReport = True
                End If

                ' Display all other messages in the status strip.
                If Not dontReport Then
                    Dim msgTemp As String = BytesToString(bytes)
                    If msgTemp.Contains("Thread Usage") Then
                        Dim cleanMsg As String = msgTemp.Substring(0, 3)
                        cleanMsg = cleanMsg.Replace("%", "")
                        If IsNumeric(cleanMsg) = True Then
                            cpu.Value = cleanMsg
                        End If
                    End If
                    status.Text = msgTemp
                End If
            End If
        End If

    End Sub

    Private Sub handleTOP_Click(sender As Object, e As EventArgs) Handles handleTOP.Click

    End Sub

    Private Sub cmdPrompt_TextChanged(sender As Object, e As EventArgs) Handles cmdPrompt.TextChanged

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub button_of_id_load_Click(sender As Object, e As EventArgs) Handles button_of_id_load.Click

    End Sub

    Private Sub FlatButton1_Click_1(sender As Object, e As EventArgs) Handles FlatButton1.Click
        serverTabs.SelectTab(srvManage)

    End Sub

    Private Sub serverLoad_Click(sender As Object, e As EventArgs) Handles serverLoad.Click

    End Sub

    Private Sub CMD_COMMAND_PROCESS(sender As Object, e As EventArgs) Handles sendCMD.Click
        If Me.cmdMsg.Text.Trim.Length > 0 Then
            Dim errorMessage As String = ""
            If Not client.SendText("CMD|" & Me.cmdMsg.Text.Trim, , errorMessage) Then MsgBox(errorMessage, MsgBoxStyle.Critical)
            cmdMsg.Text = ""
            sendCMD.Text = "Sending..."
        End If
    End Sub

    Private Sub FlatClose1_Click(sender As Object, e As EventArgs) Handles FlatClose1.Click
        Dim tmpJSON As String = JsonConvert.SerializeObject(json)
        File.WriteAllText("data2.json", tmpJSON)
        tmrPoll.Enabled = False
        IsClosing = True
        If client IsNot Nothing AndAlso client.isClientRunning Then

            client.Close(True)
            While client.isClientRunning
                Application.DoEvents()
            End While
        End If
    End Sub

    Private Sub FlatButton3_Click(sender As Object, e As EventArgs) Handles FlatButton3.Click
        If ipv4text.Text.Length > 0 Then
            Dim serv As New servers()
            With serv
                .connection.ip_address = ipv4text.Text
                .connection.hostname = hostname_value.Text.ToString
                .connection.port = "22490"
                .connection.auth_id = "MEASURES-X"
                .compute.hdd = 100
                .compute.ram = 100
                .compute.cpu = "Intel Core"
                .isOnline = False
                .serverType = "Windows 2008 R2"
                .server_id = 1
            End With
            Me.json.servers.Add(serv)
            reloadFlatUi()
        End If

    End Sub

    Private Sub reloadFlatUi()
        modules.resetFlatUI()
    End Sub

    Private Sub browseFolder_funct(sender As Object, e As EventArgs) Handles browseFolder.Click
        If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
            folderDirectory.Text = FolderBrowserDialog1.SelectedPath
            path_to_save = folderDirectory.Text
        End If
    End Sub

    Private Sub activeInterval_TextChanged(sender As Object, e As EventArgs) Handles activeInterval.TextChanged, idleInterval.TextChanged
        Dim txt As FlatTextBox = CType(sender, FlatTextBox) 'Converts to TextBox From OBJECT

        If Not IsNumeric(txt.Text) Then
            txt.Text = 5
        End If
    End Sub

    Private Sub closeAllConnections(Optional safety As Boolean = False) 'This SUB Closes the WHOLE Connection
        currMachineLoaded = Nothing 'Sets NO machine Loaded
        client.Close(safety) 'Closes that
        serverTabs.SelectTab(serverList) 'Sets original tab
        _keepGoing = False 'Stop DATA COllection
        If safety = False Then
            modules.createAlert("Disconnected from Server due to TimeOut", "error", "alertHome")
        Else
            modules.createAlert("Disconnected from Server due to Aboert", "info", "alertHome")
        End If
    End Sub

    Private Sub FlatButton5_Click(sender As Object, e As EventArgs) Handles btSendFile.Click
        If btSendFile.Text = "Send File" Then
            If tbSendFile.Text.Trim <> "" Then
                If client.isClientRunning Then client.SendFile(tbSendFile.Text.Trim)
                btSendFile.Text = "Cancel"
            End If
        Else
            client.CancelOutgoingFileTransfer()
            btSendFile.Text = "Send File"
        End If
    End Sub

    Private Sub PictureBox29_Click(sender As Object, e As EventArgs) Handles PictureBox29.Click

    End Sub

    Private Sub FlatButton4_Click(sender As Object, e As EventArgs) Handles btGetFileBrowse.Click
        Dim ofd As New OpenFileDialog
        Dim _path As String
        ofd.ShowDialog()
        _path = ofd.FileName
        tbGetFileReq.Text = _path
    End Sub

    Private Sub btGetFile_Click(sender As Object, e As EventArgs) Handles btGetFile.Click
        If btGetFile.Text = "Get File" Then
            If tbGetFileReq.Text.Trim <> "" Then
                If client.isClientRunning Then client.GetFile(tbGetFileReq.Text.Trim)
                btGetFile.Text = "Cancel"
            End If
        Else
            client.CancelIncomingFileTransfer()
            btGetFile.Text = "Get File"
        End If
    End Sub

    Private Sub btSendFileBrowse_Click(sender As Object, e As EventArgs) Handles btSendFileBrowse.Click
        Dim ofd As New OpenFileDialog
        ofd.ShowDialog()
        tbSendFile.Text = ofd.FileName
    End Sub

    Private Sub folderDirectory_TextChanged(sender As Object, e As EventArgs) Handles folderDirectory.TextChanged
        path_to_save = folderDirectory.Text
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles reboot.Click
        If reboot.Text = "Reboot" Then
            If MsgBox("Prompt", MsgBoxStyle.YesNoCancel, "Are you sure you would like to restart this server?") = MsgBoxResult.Yes Then 'Ensures restart
                msgServer(currOpenint, "shutdown -r -f")
                reboot.Text = "Cancel"
                Thread.Sleep(1000) 'Gives 1 second type to cancel
            End If
            reboot.Text = "Reboot"
        Else
            msgServer(currOpenint, "shutdown -a -f") 'Cancels SHUTDOWN
            reboot.Text = "Reboot"
        End If

    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        System.Diagnostics.Process.Start("mstsc.exe", json.servers(currOpenint - 1).connection.ip_address.ToString) ' -1 Is used to return for JSON ID
        'Starts RDC with that IP

    End Sub

End Class



#Region "Create Classes for JSON Reference"

Class serverList
    Public name As String
    Public age As Integer
    Public lName As String
End Class
Class Data
    Public servers As List(Of servers)
End Class

Class servers
    Public server_id As Integer
    Public isOnline As Boolean
    Public serverType As String
    Public connection As New connection
    Public compute As New compute
End Class

Class connection
    Public hostname As String
    Public ip_address As String
    Public auth_id As String
    Public port As String
End Class
Class compute
    Public cpu As String
    Public ram As Integer
    Public hdd As Integer
End Class
#End Region