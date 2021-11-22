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

Partial Class BombThreatDevice
    Inherits System.Web.UI.Page

    'Help Functions from our App_Code.
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    Public objDataGridFunctions As New DataGridFunctions

    'For Connecting to the database.
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
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        Select Case ns.UserLevelID.ToString() 'oCookie.Item("UserLevelID")
            Case "1" 'Admin

            Case "2" 'Full User

            Case "3" 'Update User
                btnSave.Disabled = True
            Case "4", "5" 'Read Only and Read Only + Hazmat
                btnSave.Disabled = True
            Case Else

        End Select

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

            Dim localBombThreatDeviceCount As Integer = 0

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectBombThreatDeviceCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localBombThreatDeviceCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            'Response.Write(localBombThreatDeviceCount)
            'Response.End()

            If localBombThreatDeviceCount > 0 Then
                PopulatePage()
            End If
        End If
    End Sub

    Protected Sub PopulatePage()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectBombThreatDeviceByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            ddlSubType.SelectedValue = HelpFunction.Convertdbnulls(objDR("SubType"))
            ddlSituation.SelectedValue = HelpFunction.Convertdbnulls(objDR("Situation"))
            txtHowReceivedWhoFound.Text = HelpFunction.Convertdbnulls(objDR("HowReceivedWhoFound"))
            txtExactWordingThreat.Text = HelpFunction.Convertdbnulls(objDR("ExactWordingThreat"))
            txtDescription.Text = HelpFunction.Convertdbnulls(objDR("Description"))
            ddlEvacuations.SelectedValue = HelpFunction.Convertdbnulls(objDR("Evacuations"))
            ddlMajorRoadwaysClosed.SelectedValue = HelpFunction.Convertdbnulls(objDR("MajorRoadwaysClosed"))
            txtDepartmentAgencyResponding.Text = HelpFunction.Convertdbnulls(objDR("DepartmentAgencyResponding"))
            txtDepartmentAgencyNotified.Text = HelpFunction.Convertdbnulls(objDR("DepartmentAgencyNotified"))
            ddlFatality.SelectedValue = HelpFunction.Convertdbnulls(objDR("Fatality"))
            txtFatalityText.Text = HelpFunction.Convertdbnulls(objDR("FatalityText"))
            ddlSearchBeingConducted.SelectedValue = HelpFunction.Convertdbnulls(objDR("SearchBeingConducted"))
            txtDepartmentAgencySearch.Text = HelpFunction.Convertdbnulls(objDR("DepartmentAgencySearch"))
            ddlInjury.SelectedValue = HelpFunction.Convertdbnulls(objDR("Injury"))
            txtInjury.Text = HelpFunction.Convertdbnulls(objDR("InjuryText"))
            ddlNotification.SelectedValue = HelpFunction.Convertdbnulls(objDR("IncidentTypeLevelID"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        txtWorkSheetDescription.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        If ddlInjury.SelectedValue = "Yes" Then
            'pnlShowInjuryText.Visible = True
        End If

        If ddlFatality.SelectedValue = "Yes" Then
            'pnlShowFatalityText.Visible = True
        End If

        If ddlSubType.SelectedValue = "Bomb or Device Explosion" Then
            pnlShowBombDeviceExplosion.Visible = True
        End If

        If ddlSubType.SelectedValue = "Unconfirmed Threat" Or ddlSubType.SelectedValue = "Unfounded Threat" Then
            pnlShowUnconfirmedUnfoundedThreat.Visible = True
        End If

        If ddlSearchBeingConducted.SelectedValue = "Yes" Then
            pnlShowSearchBeingConducted.Visible = True
        End If
    End Sub


    Sub PopulateDDLs()
        'Notification Group.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectIncidentTypeLevelForDDL", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentTypeID", MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("IncidentTypeID", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID")))

        DBConStringHelper.PrepareConnection(objConn) 'Open the connection
        ddlNotification.DataSource = objCmd.ExecuteReader()
        ddlNotification.DataBind()
        DBConStringHelper.FinalizeConnection(objConn) 'Close the connection

        objCmd = Nothing

        'Add an "Select an Option" item to the list.
        ddlNotification.Items.Insert(0, New ListItem("Select an Option", "Select an Option"))
        ddlNotification.Items(0).Selected = True
    End Sub

    Protected Sub Save()
        Dim localBombThreatDeviceCount As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectBombThreatDeviceCountByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            localBombThreatDeviceCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        'We add these to blank since the panels are not visible.
        If pnlShowInjuryText.Visible = False Then
            txtInjury.Text = ""
        End If

        If pnlShowFatalityText.Visible = False Then
            txtFatalityText.Text = ""
        End If

        If pnlShowSearchBeingConducted.Visible = False Then
            txtDepartmentAgencySearch.Text = ""
        End If

        If localBombThreatDeviceCount = 0 Then
            'Response.Write("Its Working!")
            'Response.End()

            Try
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionBombThreatDevice", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
                objCmd.Parameters.AddWithValue("@Flag", 0)
                objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@HowReceivedWhoFound", txtHowReceivedWhoFound.Text)
                objCmd.Parameters.AddWithValue("@ExactWordingThreat", txtExactWordingThreat.Text)
                objCmd.Parameters.AddWithValue("@Description", txtDescription.Text)
                objCmd.Parameters.AddWithValue("@Evacuations", ddlEvacuations.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@MajorRoadwaysClosed", ddlMajorRoadwaysClosed.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@DepartmentAgencyResponding", txtDepartmentAgencyResponding.Text)
                objCmd.Parameters.AddWithValue("@DepartmentAgencyNotified", txtDepartmentAgencyNotified.Text)
                objCmd.Parameters.AddWithValue("@Fatality", ddlFatality.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@FatalityText", txtFatalityText.Text)
                objCmd.Parameters.AddWithValue("@SearchBeingConducted", ddlSearchBeingConducted.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@DepartmentAgencySearch", txtDepartmentAgencySearch.Text)
                objCmd.Parameters.AddWithValue("@Injury", ddlInjury.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@InjuryText", txtInjury.Text)
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

            Try
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spInsertMostRecentUpdateByIncidentID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                objCmd.Parameters.AddWithValue("@UpdateDate", NowDate)
                objCmd.Parameters.AddWithValue("@UserID", ns.UserID)
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Added Bomb Threat or Device")

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
                objCmd = New SqlCommand("spUpdateIncidentReportUpdate", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                objCmd.Parameters.AddWithValue("@LastUpdatedByID", ns.UserID)
                objCmd.Parameters.AddWithValue("@LastUpdated", NowDate)

                DBConStringHelper.PrepareConnection(objConn)

                objCmd.ExecuteNonQuery()

                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)
            Catch ex As Exception
                Response.Write(ex.ToString)
                Exit Sub
            End Try
        Else
            'Response.Write(ddlSubType.SelectedValue.ToString)
            'Response.End()

            'Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionBombThreatDevice", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 1)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@HowReceivedWhoFound", txtHowReceivedWhoFound.Text)
            objCmd.Parameters.AddWithValue("@ExactWordingThreat", txtExactWordingThreat.Text)
            objCmd.Parameters.AddWithValue("@Description", txtDescription.Text)
            objCmd.Parameters.AddWithValue("@Evacuations", ddlEvacuations.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@MajorRoadwaysClosed", ddlMajorRoadwaysClosed.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DepartmentAgencyResponding", txtDepartmentAgencyResponding.Text)
            objCmd.Parameters.AddWithValue("@DepartmentAgencyNotified", txtDepartmentAgencyNotified.Text)
            objCmd.Parameters.AddWithValue("@Fatality", ddlFatality.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@FatalityText", txtFatalityText.Text)
            objCmd.Parameters.AddWithValue("@SearchBeingConducted", ddlSearchBeingConducted.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DepartmentAgencySearch", txtDepartmentAgencySearch.Text)
            objCmd.Parameters.AddWithValue("@Injury", ddlInjury.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@InjuryText", txtInjury.Text)
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

            Try
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spInsertMostRecentUpdateByIncidentID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                objCmd.Parameters.AddWithValue("@UpdateDate", NowDate)
                objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Updated Bomb Threat or Device")

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
                objCmd = New SqlCommand("spUpdateIncidentReportUpdate", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                objCmd.Parameters.AddWithValue("@LastUpdatedByID", ns.UserID) 'oCookie.Item("UserID"))
                objCmd.Parameters.AddWithValue("@LastUpdated", NowDate)

                DBConStringHelper.PrepareConnection(objConn)

                objCmd.ExecuteNonQuery()

                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)
            Catch ex As Exception
                Response.Write(ex.ToString)
                Exit Sub
            End Try
        End If
    End Sub

    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As EventArgs)
        ErrorChecks()

        If globalHasErrors = False Then
            Save()

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)

            'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID") & "&Parameter=WorkSheet")
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)

        'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID") & "&Parameter=WorkSheet")
    End Sub

    Protected Sub ddlSubType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubType.SelectedIndexChanged
        If ddlSubType.SelectedValue = "Bomb or Device Explosion" Then
            pnlShowBombDeviceExplosion.Visible = True
            pnlShowUnconfirmedUnfoundedThreat.Visible = False
            ddlSearchBeingConducted.SelectedValue = "Select an Option"
            pnlShowSearchBeingConducted.Visible = False
        Else
            If ddlSubType.SelectedValue = "Unconfirmed Threat" Or ddlSubType.SelectedValue = "Unfounded Threat" Then
                pnlShowUnconfirmedUnfoundedThreat.Visible = True
                pnlShowBombDeviceExplosion.Visible = False
                ddlFatality.SelectedValue = "Select an Option"
                ddlInjury.SelectedValue = "Select an Option"
                pnlShowFatalityText.Visible = False
                pnlShowInjuryText.Visible = False
            Else
                pnlShowBombDeviceExplosion.Visible = False
                pnlShowUnconfirmedUnfoundedThreat.Visible = False
                ddlFatality.SelectedValue = "Select an Option"
                ddlInjury.SelectedValue = "Select an Option"
                pnlShowFatalityText.Visible = False
                pnlShowInjuryText.Visible = False
            End If
        End If
    End Sub

    Protected Sub ddlFatality_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFatality.SelectedIndexChanged
        If ddlFatality.SelectedValue = "Yes" Then
            pnlShowFatalityText.Visible = True
        Else
            pnlShowFatalityText.Visible = False
        End If
    End Sub

    Protected Sub ddlInjury_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlInjury.SelectedIndexChanged
        If ddlInjury.SelectedValue = "Yes" Then
            pnlShowInjuryText.Visible = True
        Else
            pnlShowInjuryText.Visible = False
        End If
    End Sub

    Protected Sub ddlSearchBeingConducted_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSearchBeingConducted.SelectedIndexChanged
        If ddlSearchBeingConducted.SelectedValue = "Yes" Then
            pnlShowSearchBeingConducted.Visible = True
        Else
            pnlShowSearchBeingConducted.Visible = False
        End If
    End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start The Error String.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

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

        If txtHowReceivedWhoFound.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: How was the threat received/who found the device? <br />")
            globalHasErrors = True
        End If

        If txtDescription.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Description of the bomb or device: <br />")
            globalHasErrors = True
        End If

        'If ddlEvacuations.SelectedValue = "Select an Option" Then
        '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Are there any evacuations? <br />")
        '    globalHasErrors = True
        'End If

        If ddlMajorRoadwaysClosed.SelectedValue = "Select an Option" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Are any major roadways closed? <br />")
            globalHasErrors = True
        End If

        'If ddlSubType.SelectedValue = "Bomb or Device Explosion" Then
        '    If ddlInjury.SelectedValue = "Select an Option" Then
        '        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Are there Injuries? <br />")
        '        globalHasErrors = True
        '    End If

        '    If ddlFatality.SelectedValue = "Select an Option" Then
        '        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Are there any human fatalities? <br />")
        '        globalHasErrors = True
        '    End If
        'Else
        If ddlSubType.SelectedValue = "Unconfirmed Threat" Or ddlSubType.SelectedValue = "Unfounded Threat" Then
            If ddlSearchBeingConducted.SelectedValue = "Select an Option" Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Is a search being conducted? <br />")
                globalHasErrors = True
            End If

            If ddlSearchBeingConducted.SelectedValue = "Yes" Then
                If txtDepartmentAgencySearch.Text = "" Then
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: By which departments/agencies? <br />")
                    globalHasErrors = True
                End If
            End If
        End If

        'Finish the Error String.
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels.
        lblMessage.Text = strError.ToString
    End Sub
End Class
