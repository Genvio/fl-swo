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
Imports System.Web.Services
Imports System.IO


Partial Class AircraftIncident
    Inherits System.Web.UI.Page

    'Help functions from our App_Code.
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    Public objDataGridFunctions As New DataGridFunctions

    'For connecting to the database.
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet

    Dim ParamId As SqlParameter

    Public AuditHelper As New AuditHelp

    Public MrDataGrabber As New DataGrabber

    Dim globalAuditAction As String = ""
    Dim globalHasErrors As Boolean = False
    Dim globalMessage As String
    Dim globalCurrentStep As Integer
    Dim globalIsSaved As Boolean = False
    Dim globalAction As String
    Dim globalParameter As String
    'Dim oCookie As System.Web.HttpCookie
    Dim ns As New SecurityValidate
    Const js As String = "TADDScript.js"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        ''Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        Select Case ns.UserLevelID.ToString() 'oCookie.Item("UserLevelID")
            Case "1" 'Admin.

            Case "2" 'Full User.

            Case "3" 'Update User.
                btnSave.Disabled = True
            Case "4", "5" 'Read Only and Read Only + Hazmat.
                btnSave.Disabled = True
            Case Else

        End Select

        'btnCancel.Attributes.Add("onclick", "window.open('','_self');window.close();")
        'btnSave.Attributes.Add("onclick", "window.open('','_self');window.close();")

        'btnSave.Attributes.Add("onclick", "window.close();")

        'Response.End()

        'If Request("IncidentID") = "" Then
        '    Response.Redirect("Incident.aspx")
        'End If

        If Page.IsPostBack = False Then
            'Set message.
            globalMessage = Request("Message")
            globalAction = Request("Action")
            globalParameter = Request("Parameter")

            PopulateDDLs()

            'Select Case globalAction
            '    Case "Delete"
            '        Select Case globalParameter
            '            Case "IncidentType"
            '                DeleteIncidentType()
            '            Case ""

            '            Case "3"

            '            Case Else

            '        End Select
            '    Case "2"
            '        lblMessage.Text = "User Has Been Deleted."
            '        lblMessage.ForeColor = Drawing.Color.Green
            '        lblMessage.Visible = True
            '    Case "3"
            '        lblMessage.Text = "User Has Been Updated."
            '        lblMessage.ForeColor = Drawing.Color.Green
            '        lblMessage.Visible = True
            '    Case Else
            'End Select

            Dim localAircraftIncidentCount As Integer = 0

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectAircraftIncidentCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localAircraftIncidentCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            If localAircraftIncidentCount > 0 Then
                PopulatePage()
            End If
        End If
    End Sub

    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As EventArgs)
        btnSave.Disabled = False
        btnCancel.Disabled = False

        ErrorChecks()

        'Response.Write(globalHasErrors)
        'Response.End()

        If globalHasErrors = False Then
            Save()

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)

            'lblAjaxHelper.Text = "<script language='javascript'> { window.open('','_self');window.close();}</script>"
            'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID") & "&Parameter=WorkSheet")
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)
    End Sub

    Protected Sub ddlStructuresRoadwaysInvolved_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStructuresRoadwaysInvolved.SelectedIndexChanged
        If ddlStructuresRoadwaysInvolved.SelectedValue = "Yes" Then
            pnlShowStructuresRoadwaysInvolvedText.Visible = True
        Else
            pnlShowStructuresRoadwaysInvolvedText.Visible = False
        End If
    End Sub

    Protected Sub ddlInjury_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlInjury.SelectedIndexChanged
        If ddlInjury.SelectedValue = "Yes" Then
            pnlShowInjuryTextBox.Visible = True
        Else
            pnlShowInjuryTextBox.Visible = False
        End If
    End Sub

    Protected Sub PopulatePage()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectAircraftIncidentByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            ddlSubType.SelectedValue = HelpFunction.Convertdbnulls(objDR("SubType"))
            ddlSituation.SelectedValue = HelpFunction.Convertdbnulls(objDR("Situation"))
            ddlAircraftType.SelectedValue = HelpFunction.Convertdbnulls(objDR("AircraftType"))
            txtMakeModel.Text = HelpFunction.Convertdbnulls(objDR("MakeModel"))
            txtTailNumber.Text = HelpFunction.Convertdbnulls(objDR("TailNumber"))
            txtOwnedOperatedBy.Text = HelpFunction.Convertdbnulls(objDR("OwnedOperatedBy"))
            txtCauseOfIncident.Text = HelpFunction.Convertdbnulls(objDR("CauseOfIncident"))
            txtNumberPeopleOnboard.Text = HelpFunction.Convertdbnulls(objDR("NumberPeopleOnboard"))
            ddlFire.SelectedValue = HelpFunction.Convertdbnulls(objDR("Fire"))
            ddlInjury.SelectedValue = HelpFunction.Convertdbnulls(objDR("Injury"))
            txtInjuryText.Text = HelpFunction.Convertdbnulls(objDR("InjuryText"))
            ddlFatality.SelectedValue = HelpFunction.Convertdbnulls(objDR("Fatality"))
            txtFatalityText.Text = HelpFunction.Convertdbnulls(objDR("FatalityText"))
            ddlStructuresRoadwaysInvolved.SelectedValue = HelpFunction.Convertdbnulls(objDR("StructuresRoadwaysInvolved"))
            txtStructuresRoadwaysInvolvedText.Text = HelpFunction.Convertdbnulls(objDR("StructuresRoadwaysInvolvedText"))
            ddlHazMatOnboard.SelectedValue = HelpFunction.Convertdbnulls(objDR("HazMatOnboard"))
            ddlFuelPetroleumSpills.SelectedValue = HelpFunction.Convertdbnulls(objDR("FuelPetroleumSpills"))
            ddlEvacuations.SelectedValue = HelpFunction.Convertdbnulls(objDR("Evacuations"))
            txtDepartmentAgencyResponding.Text = HelpFunction.Convertdbnulls(objDR("DepartmentAgencyResponding"))
            txtDepartmentAgencyNotified.Text = HelpFunction.Convertdbnulls(objDR("DepartmentAgencyNotified"))
            ddlNotification.SelectedValue = HelpFunction.Convertdbnulls(objDR("IncidentTypeLevelID"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        txtWorkSheetDescription.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        If ddlInjury.SelectedValue = "Yes" Then
            'pnlShowInjuryTextBox.Visible = True
        End If

        If ddlFatality.SelectedValue = "Yes" Then
            'pnlShowFatalityText.Visible = True
        End If

        If ddlStructuresRoadwaysInvolved.SelectedValue = "Yes" Then
            pnlShowStructuresRoadwaysInvolvedText.Visible = True
        End If
    End Sub

    Sub PopulateDDLs()
        'Notification Group.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectIncidentTypeLevelForDDL", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentTypeID", MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("IncidentTypeID", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID")))

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        ddlNotification.DataSource = objCmd.ExecuteReader()
        ddlNotification.DataBind()

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        objCmd = Nothing

        'Add an "Select an Option" item to the list.
        ddlNotification.Items.Insert(0, New ListItem("Select an Option", "Select an Option"))
        ddlNotification.Items(0).Selected = True
    End Sub

    Protected Sub Save()
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        ''Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        Dim localAircraftIncidentCount As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectAircraftIncidentCountByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            localAircraftIncidentCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        'We add these to blank since the panels are not visible.
        If pnlShowInjuryTextBox.Visible = False Then
            txtInjuryText.Text = ""
        End If

        If pnlShowFatalityText.Visible = False Then
            txtFatalityText.Text = ""
        End If

        If pnlShowStructuresRoadwaysInvolvedText.Visible = False Then
            txtStructuresRoadwaysInvolvedText.Text = ""
        End If

        If localAircraftIncidentCount = 0 Then
            'Response.Write("Its Working!")
            'Response.End()

            Try
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionAircraft", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
                objCmd.Parameters.AddWithValue("@Flag", 0)
                objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@AircraftType", ddlAircraftType.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@MakeModel", txtMakeModel.Text)
                objCmd.Parameters.AddWithValue("@TailNumber", txtTailNumber.Text)
                objCmd.Parameters.AddWithValue("@OwnedOperatedBy", txtOwnedOperatedBy.Text)
                objCmd.Parameters.AddWithValue("@CauseOfIncident", txtCauseOfIncident.Text)
                objCmd.Parameters.AddWithValue("@NumberPeopleOnboard", txtNumberPeopleOnboard.Text)
                objCmd.Parameters.AddWithValue("@Fire", ddlFire.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@Injury", ddlInjury.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@InjuryText", txtInjuryText.Text)
                objCmd.Parameters.AddWithValue("@Fatality", ddlFatality.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@FatalityText", txtFatalityText.Text)
                objCmd.Parameters.AddWithValue("@StructuresRoadwaysInvolved", ddlStructuresRoadwaysInvolved.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@StructuresRoadwaysInvolvedText", txtStructuresRoadwaysInvolvedText.Text)
                objCmd.Parameters.AddWithValue("@HazMatOnboard", ddlHazMatOnboard.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@FuelPetroleumSpills", ddlFuelPetroleumSpills.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@Evacuations", ddlEvacuations.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@DepartmentAgencyResponding", txtDepartmentAgencyResponding.Text)
                objCmd.Parameters.AddWithValue("@DepartmentAgencyNotified", txtDepartmentAgencyNotified.Text)
                objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", ddlNotification.SelectedValue)

                DBConStringHelper.PrepareConnection(objConn)

                objCmd.ExecuteNonQuery()

                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)
            Catch ex As Exception
                Response.Write(ex.ToString)
                Exit Sub
            End Try

            Try
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spUpdateIncidentIncidentType", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
                objCmd.Parameters.AddWithValue("@WorkSheetDescription", txtWorkSheetDescription.Text)

                DBConStringHelper.PrepareConnection(objConn)

                objCmd.ExecuteNonQuery()

                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)
            Catch ex As Exception
                Response.Write(ex.ToString)
                Exit Sub
            End Try

            Dim NowDate As Date = Now

            AuditHelper.InsertReportUpdate(Request("IncidentID"), "Saved Initial Information for Aircraft Worksheet: " & txtWorkSheetDescription.Text, ns.UserID) 'oCookie.Item("UserID"))

            'Try
            '    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '    'Enter the email and password to query/command object.
            '    objCmd = New SqlCommand("spInsertMostRecentUpdateByIncidentID", objConn)
            '    objCmd.CommandType = CommandType.StoredProcedure
            '    objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            '    objCmd.Parameters.AddWithValue("@UpdateDate", NowDate)
            '    objCmd.Parameters.AddWithValue("@UserID", oCookie.Item("UserID"))
            '    objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Added Aircraft Incident")

            '    DBConStringHelper.PrepareConnection(objConn)

            '    objCmd.ExecuteNonQuery()

            '    objCmd.Dispose()
            '    objCmd = Nothing
            '    DBConStringHelper.FinalizeConnection(objConn)
            'Catch ex As Exception
            '    Response.Write(ex.ToString)
            '    Exit Sub
            'End Try

            'Try
            '    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '    'Enter the email and password to query/command object.
            '    objCmd = New SqlCommand("spUpdateIncidentReportUpdate", objConn)
            '    objCmd.CommandType = CommandType.StoredProcedure
            '    objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            '    objCmd.Parameters.AddWithValue("@LastUpdatedByID", oCookie.Item("UserID"))
            '    objCmd.Parameters.AddWithValue("@LastUpdated", NowDate)

            '    DBConStringHelper.PrepareConnection(objConn)

            '    objCmd.ExecuteNonQuery()

            '    objCmd.Dispose()
            '    objCmd = Nothing
            '    DBConStringHelper.FinalizeConnection(objConn)

            'Catch ex As Exception
            '    Response.Write(ex.ToString)
            '    Exit Sub
            'End Try
        Else
            'Response.Write(ddlSubType.SelectedValue.ToString)
            'Response.End()

            'Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionAircraft", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 1)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@AircraftType", ddlAircraftType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@MakeModel", txtMakeModel.Text)
            objCmd.Parameters.AddWithValue("@TailNumber", txtTailNumber.Text)
            objCmd.Parameters.AddWithValue("@OwnedOperatedBy", txtOwnedOperatedBy.Text)
            objCmd.Parameters.AddWithValue("@CauseOfIncident", txtCauseOfIncident.Text)
            objCmd.Parameters.AddWithValue("@NumberPeopleOnboard", txtNumberPeopleOnboard.Text)
            objCmd.Parameters.AddWithValue("@Fire", ddlFire.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Injury", ddlInjury.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@InjuryText", txtInjuryText.Text)
            objCmd.Parameters.AddWithValue("@Fatality", ddlFatality.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@FatalityText", txtFatalityText.Text)
            objCmd.Parameters.AddWithValue("@StructuresRoadwaysInvolved", ddlStructuresRoadwaysInvolved.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@StructuresRoadwaysInvolvedText", txtStructuresRoadwaysInvolvedText.Text)
            objCmd.Parameters.AddWithValue("@HazMatOnboard", ddlHazMatOnboard.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@FuelPetroleumSpills", ddlFuelPetroleumSpills.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Evacuations", ddlEvacuations.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DepartmentAgencyResponding", txtDepartmentAgencyResponding.Text)
            objCmd.Parameters.AddWithValue("@DepartmentAgencyNotified", txtDepartmentAgencyNotified.Text)
            objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", ddlNotification.SelectedValue)

            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            'Catch ex As Exception
            '    Response.Write(ex.ToString)
            '    Exit Sub
            'End Try

            Dim localCurrentWorkSheetDescription As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

            Try
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spUpdateIncidentIncidentType", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
                objCmd.Parameters.AddWithValue("@WorkSheetDescription", txtWorkSheetDescription.Text)

                DBConStringHelper.PrepareConnection(objConn)

                objCmd.ExecuteNonQuery()

                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)
            Catch ex As Exception
                Response.Write(ex.ToString)
                Exit Sub
            End Try

            If localCurrentWorkSheetDescription <> txtWorkSheetDescription.Text Then
                AuditHelper.InsertReportUpdate(Request("IncidentID"), "Changed Aircraft Worksheet Description From: " & localCurrentWorkSheetDescription & " To: " & txtWorkSheetDescription.Text, ns.UserID) 'oCookie.Item("UserID"))
            End If

            Dim NowDate As Date = Now

            AuditHelper.InsertReportUpdate(Request("IncidentID"), "Updated Information for Aircraft Worksheet: " & txtWorkSheetDescription.Text, ns.UserID) 'oCookie.Item("UserID"))

            'Try
            '    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '    'Enter the email and password to query/command object.
            '    objCmd = New SqlCommand("spInsertMostRecentUpdateByIncidentID", objConn)
            '    objCmd.CommandType = CommandType.StoredProcedure
            '    objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            '    objCmd.Parameters.AddWithValue("@UpdateDate", NowDate)
            '    objCmd.Parameters.AddWithValue("@UserID", oCookie.Item("UserID"))
            '    objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Updated Aircraft Incident")

            '    DBConStringHelper.PrepareConnection(objConn)

            '    objCmd.ExecuteNonQuery()

            '    objCmd.Dispose()
            '    objCmd = Nothing
            '    DBConStringHelper.FinalizeConnection(objConn)
            'Catch ex As Exception
            '    Response.Write(ex.ToString)
            '    Exit Sub
            'End Try

            'Try
            '    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '    'Enter the email and password to query/command object.
            '    objCmd = New SqlCommand("spUpdateIncidentReportUpdate", objConn)
            '    objCmd.CommandType = CommandType.StoredProcedure
            '    objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            '    objCmd.Parameters.AddWithValue("@LastUpdatedByID", oCookie.Item("UserID"))
            '    objCmd.Parameters.AddWithValue("@LastUpdated", NowDate)

            '    DBConStringHelper.PrepareConnection(objConn)

            '    objCmd.ExecuteNonQuery()

            '    objCmd.Dispose()
            '    objCmd = Nothing
            '    DBConStringHelper.FinalizeConnection(objConn)
            'Catch ex As Exception
            '    Response.Write(ex.ToString)
            '    Exit Sub
            'End Try
        End If
    End Sub

    Protected Sub ddlFatality_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFatality.SelectedIndexChanged
        If ddlFatality.SelectedValue = "Yes" Then
            pnlShowFatalityText.Visible = True
        Else
            pnlShowFatalityText.Visible = False
        End If
    End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start The Error String.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        'Adding the appropriate errors to the error string.
        If ddlSubType.SelectedValue = "Select an Option" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Sub-Type. <br />")
            globalHasErrors = True
        End If

        If ddlSituation.SelectedValue = "Select an Option" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Situation. <br />")
            globalHasErrors = True
        End If

        If txtWorkSheetDescription.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Description. <br />")
            globalHasErrors = True
        End If

        If ddlNotification.SelectedValue = "Select an Option" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Notification. <br />")
            globalHasErrors = True
        End If

        If ddlAircraftType.SelectedValue = "Select an Option" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide an Aircraft Type. <br />")
            globalHasErrors = True
        End If

        'If txtMakeModel.Text = "" Then
        '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide an Aircraft Make & Model. <br />")
        '    globalHasErrors = True
        'End If

        'If txtTailNumber.Text = "" Then
        '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Tail Number. <br />")
        '    globalHasErrors = True
        'End If

        'If txtOwnedOperatedBy.Text = "" Then
        '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for Owned/Operated By. <br />")
        '    globalHasErrors = True
        'End If

        If txtCauseOfIncident.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for Cause of incident (if known). <br />")
            globalHasErrors = True
        End If

        'If txtNumberPeopleOnboard.Text = "" Then
        '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for # of People Onboard (passengers/crew). <br />")
        '    globalHasErrors = True
        'End If

        If ddlFire.SelectedValue = "Select an Option" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for is there a fire? <br />")
            globalHasErrors = True
        End If

        'If ddlInjury.SelectedValue = "Select an Option" Then
        '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for Are there Injuries? <br />")
        '    globalHasErrors = True
        'End If

        'If ddlInjury.SelectedValue = "Yes" Then
        '    If txtInjuryText.Text = "" Then
        '        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for Number and Severity of Injuries <br />")
        '        globalHasErrors = True
        '    End If
        'End If

        'If ddlFatality.SelectedValue = "Select an Option" Then
        '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for Are there fatalities? <br />")
        '    globalHasErrors = True
        'End If

        'If ddlFatality.SelectedValue = "Yes" Then
        '    If txtFatalityText.Text = "" Then
        '        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for Number and location (aircraft or ground): <br />")
        '        globalHasErrors = True
        '    End If
        'End If

        If ddlStructuresRoadwaysInvolved.SelectedValue = "Select an Option" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for Are other structures or roadways involved? <br />")
            globalHasErrors = True

        End If

        'If ddlStructuresRoadwaysInvolved.SelectedValue = "Yes" Then
        '    If txtStructuresRoadwaysInvolvedText.Text = "" Then
        '        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for (structures or roadways involved) Description <br />")
        '        globalHasErrors = True
        '    End If
        'End If

        'If ddlFire.SelectedValue=

        If ddlHazMatOnboard.SelectedValue = "Select an Option" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for Hazardous materials onboard? <br />")
            globalHasErrors = True
        End If

        If ddlFuelPetroleumSpills.SelectedValue = "Select an Option" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for Fuel or Petroleum Spills? <br />")
            globalHasErrors = True
        End If

        'If ddlEvacuations.SelectedValue = "Select an Option" Then
        '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for Are there any evacuations? <br />")
        '    globalHasErrors = True
        'End If

        'Finish the Error String.
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels.
        lblMessage.Text = strError.ToString
    End Sub
End Class
