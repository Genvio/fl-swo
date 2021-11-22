Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Collections
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text

Partial Class EditNotificationGroup
    Inherits System.Web.UI.Page

    'Help functions from our App_Code.
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    Public AuditHelper As New AuditHelp

    'For connecting to the database.
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet

    Dim globalHasErrors As Boolean = False

    Public MrDataGrabber As New DataGrabber

    Dim ParamId As SqlParameter

    Public objDataGridFunctions As New DataGridFunctions

    'Public ObjCookie As System.Web.HttpCookie
    'Dim oCookie As System.Web.HttpCookie
    Dim ns As New SecurityValidate

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        If ns.UserLevelID <> "1" Then 'oCookie.Item("UserLevelID").ToString.Trim <> "1" Then
            Response.Redirect("Home.aspx")
        End If

        If Page.IsPostBack = False Then
            If Request("Action") = "Delete" Then
                If Request("Parameter") = "NotificationPosition" Then
                    DeletePosition()
                ElseIf Request("Parameter") = "AssociatedTask" Then
                    DeleteAssociatedTask()
                Else

                End If
            End If

            Dim localNotificationGroupID As String = Request("NotificationGroupID")

            PopulateDDLs()

            If localNotificationGroupID = 0 Then
                lblAddEdit.Text = "Add "
                btnSave.Value = "Add"
            Else
                lblAddEdit.Text = "Edit "
                btnSave.Value = "Save"

                PopulatePage()
            End If

            'lblWorksheet.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentType", "IncidentType", "IncidentTypeID", Request("IncidentTypeID"))

            getPositions()
            getAssociatedTasks()
        End If
    End Sub

    Sub PopulateDDLs()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectIncidentType", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        'objCmd.Parameters.AddWithValue("@OrderBy", "") 'Optional parameter.

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        ddlIncidentType.DataSource = objCmd.ExecuteReader()
        ddlIncidentType.DataBind()

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        objCmd = Nothing

        'Add a "Select an Option" item to the list.
        ddlIncidentType.Items.Insert(0, New ListItem("Select an Incident Type", "0"))
        ddlIncidentType.Items(0).Selected = True

        '-----------------------------------------------------------------------------------
        'Add a "Select an Option" item to the list.
        'ddlIncidentTypeLevel.Items.Insert(0, New ListItem("Select a Worksheet Level", "0"))
        'ddlIncidentTypeLevel.Items(0).Selected = True
        '-----------------------------------------------------------------------------------

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectNotificationPosition", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        'objCmd.Parameters.AddWithValue("@OrderBy", "") 'Optional parameter.

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        ddlNotificationPosition.DataSource = objCmd.ExecuteReader()
        ddlNotificationPosition.DataBind()

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        objCmd = Nothing

        'Add a "Select an Option" item to the list.
        ddlNotificationPosition.Items.Insert(0, New ListItem("Select a Position", "0"))
        ddlNotificationPosition.Items(0).Selected = True

        '-----------------------------------------------------------------------------------
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectAssociatedTask", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        'objCmd.Parameters.AddWithValue("@OrderBy", "") 'Optional parameter.

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        ddlAssociatedTask.DataSource = objCmd.ExecuteReader()
        ddlAssociatedTask.DataBind()

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        objCmd = Nothing

        'Add a "Select an Option" item to the list.
        ddlAssociatedTask.Items.Insert(0, New ListItem("Select an Associated Task", "0"))
        ddlAssociatedTask.Items(0).Selected = True
    End Sub

    Sub PopulatePage()
        Dim localIncidentTypeLevel As Integer

        Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectNotificationGroupByNotificationGroupID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@NotificationGroupID", Request("NotificationGroupID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                txtName.Text = HelpFunction.Convertdbnulls(objDR("GroupName"))
                ddlIncidentType.SelectedValue = HelpFunction.ConvertdbnullsInt(objDR("IncidentTypeID"))
                localIncidentTypeLevel = HelpFunction.ConvertdbnullsInt(objDR("IncidentTypeLevelID"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()
        Catch ex As Exception
            Response.Write(ex.ToString)

            Exit Sub
        End Try

        If ddlIncidentType.SelectedValue <> "0" Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objCmd = New SqlCommand("spSelectIncidentTypeLevelForDDL", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentTypeID", ddlIncidentType.SelectedValue)

            'Open the connection.
            DBConStringHelper.PrepareConnection(objConn)
            ddlIncidentTypeLevel.DataSource = objCmd.ExecuteReader()
            ddlIncidentTypeLevel.DataBind()

            'Close the connection.
            DBConStringHelper.FinalizeConnection(objConn)

            objCmd = Nothing

            'Add a "Select an Option" item to the list.
            'ddlIncidentTypeLevel.Items.Insert(0, New ListItem("Select A Worksheet Level", "0"))
            'ddlIncidentTypeLevel.Items(0).Selected = True

            ddlIncidentTypeLevel.SelectedValue = localIncidentTypeLevel
            ddlIncidentTypeLevel.Enabled = True
        End If

        Dim NotificationGroupNotificationPersonCount As Integer = MrDataGrabber.GrabRecordCountByKey("NotificationGroupNotificationPerson", "NotificationGroupID", Request("NotificationGroupID"))

        If NotificationGroupNotificationPersonCount > 0 Then
            ddlIncidentType.Enabled = False
        End If
    End Sub

    Protected Sub getPositions()
        If objConn.State = ConnectionState.Open Then
            objConn.Close()
        End If

        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("spSelectNotificationPositionByNotificationGroupID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@NotificationGroupID", Request("NotificationGroupID"))

        objDS.Tables.Clear()

        'Bind our data.
        objDA = New SqlDataAdapter(objCmd)
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        'Call the calculate grid counts to show the number of records, the page you are on, etc.
        objDataGridFunctions.CalcDataGridCounts(objDS, NotificationPositionDataGrid, "")

        'Associate the data grid with the data.
        NotificationPositionDataGrid.DataSource = objDS.Tables(0).DefaultView
        NotificationPositionDataGrid.DataBind()

        objDataGridFunctions.Highlightrows(NotificationPositionDataGrid, "", "", "")

        If CInt(objDS.Tables(0).Rows.Count) <> 0 Then
            'We have records so show the grid.
            pnlShowNotificationPositionGrid.Visible = True
            pnlShowAssociatedTaskTools.Visible = True
        Else
            'Hide grid.
            pnlShowNotificationPositionGrid.Visible = False
            pnlShowAssociatedTaskTools.Visible = False
        End If
    End Sub

    Protected Sub getAssociatedTasks()
        If objConn.State = ConnectionState.Open Then
            objConn.Close()
        End If

        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("spSelectAssociatedTaskByNotificationGroupID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@NotificationGroupID", Request("NotificationGroupID"))

        objDS.Tables.Clear()

        'Bind our data.
        objDA = New SqlDataAdapter(objCmd)
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        'Call the calculate grid counts to show the number of records, the page you are on, etc.
        objDataGridFunctions.CalcDataGridCounts(objDS, AssociatedTaskDataGrid, "")

        'Associate the data grid with the data.
        AssociatedTaskDataGrid.DataSource = objDS.Tables(0).DefaultView
        AssociatedTaskDataGrid.DataBind()

        objDataGridFunctions.Highlightrows(AssociatedTaskDataGrid, "", "", "")

        If CInt(objDS.Tables(0).Rows.Count) <> 0 Then
            'We have records so show the grid.
            pnlShowAssociatedTask.Visible = True
        Else
            'Hide grid.
            pnlShowAssociatedTask.Visible = False
        End If
    End Sub

    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As EventArgs)
        'Dim AuditInfo As String = ""
        'Dim AuditAction As String = ""

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)

        'btnSave.Disabled = False
        'btnCancel.Disabled = False

        ErrorChecks()

        If globalHasErrors = False Then
            If lblAddEdit.Text = "Add " Then
                Dim TempInsertedNotificationGroupID As String

                Try
                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Enter the email and password to query/command object.
                    objCmd = New SqlCommand("spInsertNotificationGroupAndReturnID", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@NotificationGroupID", 0)
                    objCmd.Parameters.AddWithValue("@IncidentTypeID", ddlIncidentType.SelectedValue)
                    objCmd.Parameters.AddWithValue("@GroupName", txtName.Text)
                    objCmd.Parameters.AddWithValue("@IncidentTypeLevelID ", ddlIncidentTypeLevel.SelectedValue)

                    ParamId = objCmd.Parameters.AddWithValue("@NotificationGroupID_out", System.Data.SqlDbType.Int)
                    ParamId.Direction = System.Data.ParameterDirection.Output

                    DBConStringHelper.PrepareConnection(objConn)

                    objCmd.ExecuteNonQuery()

                    TempInsertedNotificationGroupID = objCmd.Parameters("@NotificationGroupID_out").Value

                    objCmd.Dispose()
                    objCmd = Nothing
                    DBConStringHelper.FinalizeConnection(objConn)
                Catch ex As Exception
                    Response.Write(ex.ToString)

                    Exit Sub
                End Try

                If ddlNotificationPosition.SelectedValue <> "0" Then

                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Enter the email and password to query/command object.
                    objCmd = New SqlCommand("spInsertNotificationGroupNotificationPerson", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@NotificationGroupID", TempInsertedNotificationGroupID)
                    objCmd.Parameters.AddWithValue("@NotificationPositionID", ddlNotificationPosition.SelectedValue)

                    'Open the connection using the connection string.
                    DBConStringHelper.PrepareConnection(objConn)

                    'Execute the command to the DataReader.
                    objCmd.ExecuteNonQuery()

                    'Clean up our command objects and close the connection.
                    objCmd.Dispose()
                    objCmd = Nothing
                    DBConStringHelper.FinalizeConnection(objConn)
                End If

                'AuditAction = "Added Worksheet Level: " & txtNumber.Text & "to Worksheet = " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentType", "IncidentType", "IncidentTypeID", Request("IncidentTypeID"))
                'AuditAction = AuditAction & " with "
                'AuditAction = AuditAction & "Level Description = " & txtLevelDescription.Text

                'AuditHelper.InsertAudit(oCookie.Item("UserID").ToString.Trim, AuditAction, "1")

                'Response.Redirect("EditWorksheet.aspx?IncidentTypeID=" & Request("IncidentTypeID"))
                Response.Redirect("NotificationGroup.aspx")
            Else
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionNotificationGroup", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@NotificationGroupID", Request("NotificationGroupID"))
                objCmd.Parameters.AddWithValue("@IncidentTypeID", ddlIncidentType.SelectedValue)
                objCmd.Parameters.AddWithValue("@GroupName", txtName.Text)
                objCmd.Parameters.AddWithValue("@IncidentTypeLevelID ", ddlIncidentTypeLevel.SelectedValue)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                If ddlNotificationPosition.SelectedValue <> "0" Then
                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Enter the email and password to query/command object.
                    objCmd = New SqlCommand("spInsertNotificationGroupNotificationPerson", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@NotificationGroupID", Request("NotificationGroupID"))
                    objCmd.Parameters.AddWithValue("@NotificationPositionID", ddlNotificationPosition.SelectedValue)

                    'Open the connection using the connection string.
                    DBConStringHelper.PrepareConnection(objConn)

                    'Execute the command to the DataReader.
                    objCmd.ExecuteNonQuery()

                    'Clean up our command objects and close the connection.
                    objCmd.Dispose()
                    objCmd = Nothing
                    DBConStringHelper.FinalizeConnection(objConn)
                End If

                'AuditAction = "Added Worksheet Level: " & txtNumber.Text & "to Worksheet = " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentType", "IncidentType", "IncidentTypeID", Request("IncidentTypeID"))
                'AuditAction = AuditAction & " with "
                'AuditAction = AuditAction & "Level Description = " & txtLevelDescription.Text

                'AuditHelper.InsertAudit(oCookie.Item("UserID").ToString.Trim, AuditAction, "1")

                'Response.Redirect("EditWorksheet.aspx?IncidentTypeID=" & Request("IncidentTypeID"))
                Response.Redirect("NotificationGroup.aspx")
            End If
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("NotificationGroup.aspx")
    End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        If txtName.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Group Name. <br />")
            globalHasErrors = True
        End If

        'Adding the appropriate errors to the error string.
        If ddlIncidentType.SelectedValue = "0" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a Worksheet Type. <br />")
            globalHasErrors = True
        End If

        If ddlIncidentTypeLevel.SelectedValue = "0" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a Worksheet Level. <br />")
            globalHasErrors = True
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label.
        lblMessage.Text = strError.ToString
    End Sub

    Protected Sub ErrorChecks2()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        If txtName.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Group Name. <br />")
            globalHasErrors = True
        End If

        'Adding the appropriate errors to the error string.
        If ddlIncidentType.SelectedValue = "0" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a Worksheet Type. <br />")
            globalHasErrors = True
        End If

        If ddlIncidentTypeLevel.SelectedValue = "0" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a Worksheet Level. <br />")
            globalHasErrors = True
        End If

        If ddlNotificationPosition.SelectedValue = "0" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a Position. <br />")
            globalHasErrors = True
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label.
        lblMessage.Text = strError.ToString
    End Sub

    Protected Sub ErrorChecks3()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        'Adding the appropriate errors to the error string.
        If ddlAssociatedTask.SelectedValue = "0" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select an Associated Task. <br />")
            globalHasErrors = True
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the abel.
        lblMessage.Text = strError.ToString
    End Sub

    Private Sub DeletePosition()
        Try
            Dim AuditInfo As String = ""
            Dim AuditAction As String = ""

            'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
            'Add cookie.
            'Response.Cookies.Add(oCookie)
            ns = Session("Security_Tracker")

            'Response.Write("It Worked")
            'Response.End()

            'AuditAction = "Deleted Level: " & localNumber & " From WorkSheet: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentType", "IncidentType", "IncidentTypeID", localIncidentTypeID) & " "

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spDeleteNotificationGroupNotificationPersonIDByNotificationGroupNotificationPersonID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@NotificationGroupNotificationPersonID", Request("NotificationGroupNotificationPersonID"))

            'Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            'Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()

            'Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            'Insert audit information.
            'Response.Write(Request("It Worked"))
            'AuditHelper.InsertAudit(oCookie.Item("UserID").ToString.Trim, AuditAction, "3")

            Response.Redirect("EditNotificationGroup.aspx?Message=2&NotificationGroupID=" & Request("NotificationGroupID"))
        Catch ex As Exception
            DBConStringHelper.FinalizeConnection(objConn)

            lblMessage.Text = "You may not delete Position due to the fact it is tied to related imported information. You must first delete all related imported information first, and then you may delete the Position."
            lblMessage.Visible = True
            lblMessage.ForeColor = Drawing.Color.Red
        End Try
    End Sub

    Private Sub DeleteAssociatedTask()
        Try
            Dim AuditInfo As String = ""
            Dim AuditAction As String = ""

            'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
            'Add cookie.
            'Response.Cookies.Add(oCookie)
            ns = Session("Security_Tracker")

            'Response.Write("It Worked")
            'Response.End()

            'AuditAction = "Deleted Level: " & localNumber & " From WorkSheet: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentType", "IncidentType", "IncidentTypeID", localIncidentTypeID) & " "

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spDeleteNotificationGroupAssociatedTaskByNotificationGroupAssociatedTaskID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@NotificationGroupAssociatedTaskID", Request("NotificationGroupAssociatedTaskID"))

            'Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            'Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()

            'Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            'Insert audit information.
            'Response.Write(Request("It Worked"))
            'AuditHelper.InsertAudit(oCookie.Item("UserID").ToString.Trim, AuditAction, "3")

            Response.Redirect("EditNotificationGroup.aspx?Message=2&NotificationGroupID=" & Request("NotificationGroupID"))
        Catch ex As Exception
            DBConStringHelper.FinalizeConnection(objConn)

            lblMessage.Text = "You may not delete Associated Task due to the fact it is tied to related imported information. You must first delete all related imported information first, and then you may delete the Associated Task."
            lblMessage.Visible = True
            lblMessage.ForeColor = Drawing.Color.Red
        End Try
    End Sub

    Protected Sub ddlIncidentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIncidentType.SelectedIndexChanged
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectIncidentTypeLevelForDDL", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentTypeID", ddlIncidentType.SelectedValue)

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        ddlIncidentTypeLevel.DataSource = objCmd.ExecuteReader()
        ddlIncidentTypeLevel.DataBind()

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        objCmd = Nothing

        'Add a "Select an Option" item to the list.
        ddlIncidentTypeLevel.Items.Insert(0, New ListItem("Select a Worksheet Level", "0"))
        ddlIncidentTypeLevel.Items(0).Selected = True
        ddlIncidentTypeLevel.Enabled = True
    End Sub

    Protected Sub btnAddPosition_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddPosition.Click
        ErrorChecks2()

        If globalHasErrors = False Then
            'Response.Write(lblAddEdit.Text)
            'Response.End()

            If lblAddEdit.Text = "Add Notification Group" Then
                Dim TempInsertedNotificationGroupID As String

                Try
                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Enter the email and password to query/command object.
                    objCmd = New SqlCommand("spInsertNotificationGroupAndReturnID", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@NotificationGroupID", 0)
                    objCmd.Parameters.AddWithValue("@IncidentTypeID", ddlIncidentType.SelectedValue)
                    objCmd.Parameters.AddWithValue("@GroupName", txtName.Text)
                    objCmd.Parameters.AddWithValue("@IncidentTypeLevelID ", ddlIncidentTypeLevel.SelectedValue)

                    ParamId = objCmd.Parameters.AddWithValue("@NotificationGroupID_out", System.Data.SqlDbType.Int)
                    ParamId.Direction = System.Data.ParameterDirection.Output

                    DBConStringHelper.PrepareConnection(objConn)

                    objCmd.ExecuteNonQuery()

                    TempInsertedNotificationGroupID = objCmd.Parameters("@NotificationGroupID_out").Value

                    objCmd.Dispose()
                    objCmd = Nothing
                    DBConStringHelper.FinalizeConnection(objConn)
                Catch ex As Exception
                    Response.Write(ex.ToString)

                    Exit Sub
                End Try

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spInsertNotificationGroupNotificationPerson", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@NotificationGroupID", TempInsertedNotificationGroupID)
                objCmd.Parameters.AddWithValue("@NotificationPositionID", ddlNotificationPosition.SelectedValue)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)


                'AuditAction = "Added Worksheet Level: " & txtNumber.Text & "to Worksheet = " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentType", "IncidentType", "IncidentTypeID", Request("IncidentTypeID"))
                'AuditAction = AuditAction & " with "
                'AuditAction = AuditAction & "Level Description = " & txtLevelDescription.Text

                'AuditHelper.InsertAudit(oCookie.Item("UserID").ToString.Trim, AuditAction, "1")

                Response.Redirect("EditNotificationGroup.aspx?NotificationGroupID=" & TempInsertedNotificationGroupID)
            Else
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionNotificationGroup", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@NotificationGroupID", Request("NotificationGroupID"))
                objCmd.Parameters.AddWithValue("@IncidentTypeID", ddlIncidentType.SelectedValue)
                objCmd.Parameters.AddWithValue("@GroupName", txtName.Text)
                objCmd.Parameters.AddWithValue("@IncidentTypeLevelID ", ddlIncidentTypeLevel.SelectedValue)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spInsertNotificationGroupNotificationPerson", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@NotificationGroupID", Request("NotificationGroupID"))
                objCmd.Parameters.AddWithValue("@NotificationPositionID", ddlNotificationPosition.SelectedValue)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                'AuditAction = "Added Worksheet Level: " & txtNumber.Text & "to Worksheet = " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentType", "IncidentType", "IncidentTypeID", Request("IncidentTypeID"))
                'AuditAction = AuditAction & " with "
                'AuditAction = AuditAction & "Level Description = " & txtLevelDescription.Text

                'AuditHelper.InsertAudit(oCookie.Item("UserID").ToString.Trim, AuditAction, "1")

                Response.Redirect("EditNotificationGroup.aspx?NotificationGroupID=" & Request("NotificationGroupID"))
            End If
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnAssociatedTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAssociatedTask.Click
        ErrorChecks3()

        If globalHasErrors = False Then
            DBConStringHelper.FinalizeConnection(objConn)

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spInsertNotificationGroupAssociatedTask", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@NotificationGroupID", Request("NotificationGroupID"))
            objCmd.Parameters.AddWithValue("@AssociatedTaskID", ddlAssociatedTask.SelectedValue)

            'Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            'Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()

            'Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            'AuditAction = "Added Worksheet Level: " & txtNumber.Text & "to Worksheet = " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentType", "IncidentType", "IncidentTypeID", Request("IncidentTypeID"))
            'AuditAction = AuditAction & " with "
            'AuditAction = AuditAction & "Level Description = " & txtLevelDescription.Text

            'AuditHelper.InsertAudit(oCookie.Item("UserID").ToString.Trim, AuditAction, "1")

            Response.Redirect("EditNotificationGroup.aspx?NotificationGroupID=" & Request("NotificationGroupID"))
        Else
            pnlMessage.Visible = True
        End If
    End Sub
End Class