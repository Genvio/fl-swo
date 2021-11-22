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

Partial Class DamFailure
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

            Dim localDamFailureCount As Integer = 0

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectDamFailureCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localDamFailureCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            'Response.Write(localBombThreatDeviceCount)
            'Response.End()

            If localDamFailureCount > 0 Then
                PopulatePage()
            End If
        End If
    End Sub

    Protected Sub PopulatePage()
        'Response.Write("Hello")
        'Response.End()

        Dim localTime As String = ""
        Dim localTime2 As String = ""

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectDamFailureByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            ddlSubType.SelectedValue = HelpFunction.Convertdbnulls(objDR("SubType"))
            ddlSituation.SelectedValue = HelpFunction.Convertdbnulls(objDR("Situation"))
            txtDamName.Text = HelpFunction.Convertdbnulls(objDR("DamName"))
            txtRelatedWaterways.Text = HelpFunction.Convertdbnulls(objDR("RelatedWaterways"))
            txtPoolVolumeCapacity.Text = HelpFunction.Convertdbnulls(objDR("PoolVolumeCapacity"))
            ddlBreakOccurred.SelectedValue = HelpFunction.Convertdbnulls(objDR("BreakOccurred"))
            ddlBreakAnticipated.SelectedValue = HelpFunction.Convertdbnulls(objDR("BreakAnticipated"))
            txtCauseOfFailure.Text = HelpFunction.Convertdbnulls(objDR("CauseOfFailure"))
            txtResponsibleForMaintaining.Text = HelpFunction.Convertdbnulls(objDR("ResponsibleForMaintaining"))
            txtCorrectiveActionsTaken.Text = HelpFunction.Convertdbnulls(objDR("CorrectiveActionsTaken"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR("EstimatedRepairTime")))
            txtEstimatedRepairDate.Text = HelpFunction.Convertdbnulls(objDR("EstimatedRepairDate"))
            ddlDownstreamPopulationsThreat.SelectedValue = HelpFunction.Convertdbnulls(objDR("DownstreamPopulationsThreat"))
            txtDownstreamPopulationsThreatText.Text = HelpFunction.Convertdbnulls(objDR("DownstreamPopulationsThreatText"))
            ddlEvacuations.SelectedValue = HelpFunction.Convertdbnulls(objDR("Evacuations"))
            ddlMajorRoadwaysClosed.SelectedValue = HelpFunction.Convertdbnulls(objDR("Evacuations"))
            ddlInjury.SelectedValue = HelpFunction.Convertdbnulls(objDR("Injury"))
            txtInjury.Text = HelpFunction.Convertdbnulls(objDR("InjuryText"))
            ddlFatality.SelectedValue = HelpFunction.Convertdbnulls(objDR("Fatality"))
            txtFatalityText.Text = HelpFunction.Convertdbnulls(objDR("FatalityText"))
            ddlStateAssistance.SelectedValue = HelpFunction.Convertdbnulls(objDR("StateAssistance"))
            txtStateAssistanceText.Text = HelpFunction.Convertdbnulls(objDR("StateAssistanceText"))
            txtAgencyResponse.Text = HelpFunction.Convertdbnulls(objDR("AgencyResponse"))
            txtStagingCommandLocation.Text = HelpFunction.Convertdbnulls(objDR("StagingCommandLocation"))
            ddlNotification.SelectedValue = HelpFunction.Convertdbnulls(objDR("IncidentTypeLevelID"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        txtWorkSheetDescription.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        If ddlBreakOccurred.SelectedValue = "No" Then
            pnlShowBreakAnticipated.Visible = True
        End If

        If ddlBreakOccurred.SelectedValue = "Yes" Then
            pnlShowCauseOfFailure.Visible = True
        End If

        txtEstimatedRepairTime.Text = Left(localTime, 2)
        txtEstimatedRepairTime2.Text = Right(localTime, 2)

        If txtEstimatedRepairTime.Text = "0" Then
            txtEstimatedRepairTime.Text = ""
        End If

        If txtEstimatedRepairTime2.Text = "0" Then
            txtEstimatedRepairTime2.Text = ""
        End If

        If ddlDownstreamPopulationsThreat.SelectedValue = "Yes" Then
            pnlShowDownstreamPopulationsThreat.Visible = True
        End If

        If txtEstimatedRepairDate.Text = "1/1/1900" Then
            txtEstimatedRepairDate.Text = ""
        End If

        If ddlInjury.SelectedValue = "Yes" Then
            'pnlShowInjuryText.Visible = True
        End If

        If ddlFatality.SelectedValue = "Yes" Then
            'pnlShowFatalityText.Visible = True
        End If

        If ddlStateAssistance.SelectedValue = "Yes" Then
            pnlShowStateAssistance.Visible = True
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

    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As EventArgs)
        ErrorChecks()

        If globalHasErrors = False Then
            Save()

            'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID") & "&Parameter=WorkSheet")

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID") & "&Parameter=WorkSheet")

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)
    End Sub

    Protected Sub Save()
        Dim localDamFailureCount As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectDamFailureCountByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            localDamFailureCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))
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

        If pnlShowBreakAnticipated.Visible = False Then
            ddlBreakAnticipated.SelectedValue = "Select an Option"
        End If

        If pnlShowCauseOfFailure.Visible = False Then
            txtCauseOfFailure.Text = ""
        End If

        If pnlShowDownstreamPopulationsThreat.Visible = False Then
            txtDownstreamPopulationsThreatText.Text = ""
        End If

        If pnlShowStateAssistance.Visible = False Then
            txtStateAssistanceText.Text = ""
        End If

        'If pnlShowRegionalAssistanceRequested.Visible = False Then
        '    txtRegionalAssistanceRequestedText.Text = ""
        'End If

        'If pnlShowConfinedLocationMemoText.Visible = False Then
        '    txtConfinedLocationMemoText.Text = ""
        'End If

        'If pnlShowOtherArea.Visible = False Then
        '    txtLocationAreas.Text = ""
        'End If

        'If pnlShowConfinedLocationOther.Visible = False Then
        '    ddlConfinedLocationOther.SelectedValue = "Select an Option"
        'End If

        'If ddlConfinedLocationOther.SelectedValue = "Location on Main Form" Then
        '    txtLocationAreas.Text = ""
        'End If

        If localDamFailureCount = 0 Then
            Try
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionDamFailure", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
                objCmd.Parameters.AddWithValue("@Flag", 0)
                objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@DamName", txtDamName.Text)
                objCmd.Parameters.AddWithValue("@RelatedWaterways", txtRelatedWaterways.Text)
                objCmd.Parameters.AddWithValue("@PoolVolumeCapacity", txtPoolVolumeCapacity.Text)
                objCmd.Parameters.AddWithValue("@BreakOccurred", ddlBreakOccurred.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@BreakAnticipated", ddlBreakAnticipated.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@CauseOfFailure", txtCauseOfFailure.Text)
                objCmd.Parameters.AddWithValue("@ResponsibleForMaintaining", txtResponsibleForMaintaining.Text)
                objCmd.Parameters.AddWithValue("@CorrectiveActionsTaken", txtCorrectiveActionsTaken.Text)
                objCmd.Parameters.AddWithValue("@EstimatedRepairTime", CStr(txtEstimatedRepairTime.Text.Trim) & CStr(txtEstimatedRepairTime2.Text.Trim))
                objCmd.Parameters.AddWithValue("@EstimatedRepairDate", txtEstimatedRepairDate.Text)
                objCmd.Parameters.AddWithValue("@DownstreamPopulationsThreat", ddlDownstreamPopulationsThreat.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@DownstreamPopulationsThreatText", txtDownstreamPopulationsThreatText.Text)
                objCmd.Parameters.AddWithValue("@Evacuations", ddlEvacuations.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@MajorRoadwaysClosed", ddlMajorRoadwaysClosed.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@Injury", ddlInjury.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@InjuryText", txtInjury.Text)
                objCmd.Parameters.AddWithValue("@Fatality", ddlFatality.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@FatalityText", txtFatalityText.Text)
                objCmd.Parameters.AddWithValue("@StateAssistance ", ddlStateAssistance.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@StateAssistanceText", txtStateAssistanceText.Text)
                objCmd.Parameters.AddWithValue("@AgencyResponse ", txtAgencyResponse.Text)
                objCmd.Parameters.AddWithValue("@StagingCommandLocation", txtStagingCommandLocation.Text)
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
                objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Added Dam Failure")

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
        Else
            'Response.Write("Its Working!")
            'Response.End()

            'Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionDamFailure", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 1)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DamName", txtDamName.Text)
            objCmd.Parameters.AddWithValue("@RelatedWaterways", txtRelatedWaterways.Text)
            objCmd.Parameters.AddWithValue("@PoolVolumeCapacity", txtPoolVolumeCapacity.Text)
            objCmd.Parameters.AddWithValue("@BreakOccurred", ddlBreakOccurred.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@BreakAnticipated", ddlBreakAnticipated.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CauseOfFailure", txtCauseOfFailure.Text)
            objCmd.Parameters.AddWithValue("@ResponsibleForMaintaining", txtResponsibleForMaintaining.Text)
            objCmd.Parameters.AddWithValue("@CorrectiveActionsTaken", txtCorrectiveActionsTaken.Text)
            objCmd.Parameters.AddWithValue("@EstimatedRepairTime", CStr(txtEstimatedRepairTime.Text.Trim) & CStr(txtEstimatedRepairTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@EstimatedRepairDate", txtEstimatedRepairDate.Text)
            objCmd.Parameters.AddWithValue("@DownstreamPopulationsThreat", ddlDownstreamPopulationsThreat.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DownstreamPopulationsThreatText", txtDownstreamPopulationsThreatText.Text)
            objCmd.Parameters.AddWithValue("@Evacuations", ddlEvacuations.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@MajorRoadwaysClosed", ddlMajorRoadwaysClosed.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Injury", ddlInjury.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@InjuryText", txtInjury.Text)
            objCmd.Parameters.AddWithValue("@Fatality", ddlFatality.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@FatalityText", txtFatalityText.Text)
            objCmd.Parameters.AddWithValue("@StateAssistance ", ddlStateAssistance.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@StateAssistanceText", txtStateAssistanceText.Text)
            objCmd.Parameters.AddWithValue("@AgencyResponse ", txtAgencyResponse.Text)
            objCmd.Parameters.AddWithValue("@StagingCommandLocation", txtStagingCommandLocation.Text)
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
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Updated Dam Failure")

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

    Protected Sub ddlInjury_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlInjury.SelectedIndexChanged
        If ddlInjury.SelectedValue = "Yes" Then
            pnlShowInjuryText.Visible = True
        Else
            pnlShowInjuryText.Visible = False
        End If
    End Sub

    Protected Sub ddlFatality_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFatality.SelectedIndexChanged
        If ddlFatality.SelectedValue = "Yes" Then
            pnlShowFatalityText.Visible = True
        Else
            pnlShowFatalityText.Visible = False
        End If
    End Sub

    Protected Sub ddlBreakOccurred_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBreakOccurred.SelectedIndexChanged
        If ddlBreakOccurred.SelectedValue = "Yes" Then
            pnlShowCauseOfFailure.Visible = True
        Else
            pnlShowCauseOfFailure.Visible = False
        End If
        If ddlBreakOccurred.SelectedValue = "No" Then
            pnlShowBreakAnticipated.Visible = True
        Else
            pnlShowBreakAnticipated.Visible = False
        End If
    End Sub

    Protected Sub ddlDownstreamPopulationsThreat_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDownstreamPopulationsThreat.SelectedIndexChanged
        If ddlDownstreamPopulationsThreat.SelectedValue = "Yes" Then
            pnlShowDownstreamPopulationsThreat.Visible = True
        Else
            pnlShowDownstreamPopulationsThreat.Visible = False
        End If
    End Sub

    Protected Sub ddlStateAssistance_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStateAssistance.SelectedIndexChanged
        If ddlStateAssistance.SelectedValue = "Yes" Then
            pnlShowStateAssistance.Visible = True
        Else
            pnlShowStateAssistance.Visible = False
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

        If txtDamName.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Dam Name. <br />")
            globalHasErrors = True
        End If

        If txtRelatedWaterways.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Related Waterways/Tributaries. <br />")
            globalHasErrors = True
        End If

        If ddlBreakOccurred.SelectedValue = "Select an Option" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Has a break occurred? <br />")
            globalHasErrors = True
        End If

        'Finish the Error String.
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels.
        lblMessage.Text = strError.ToString
    End Sub
End Class
