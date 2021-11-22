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

Partial Class DemINR
    Inherits System.Web.UI.Page

    'Help Functions from our App_Code
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    Public objDataGridFunctions As New DataGridFunctions

    'For Connecting to the database
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

    'Page Load
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
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

        If Page.IsPostBack = False Then

            'set message
            globalMessage = Request("Message")
            globalAction = Request("Action")
            globalParameter = Request("Parameter")

            PopulateDDLs()

            Dim localDemINRCount As Integer = 0

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectDemINRCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                localDemINRCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            'Response.Write(localBombThreatDeviceCount)
            'Response.End()

            If localDemINRCount > 0 Then
                PopulatePage()
            End If

        End If

    End Sub

    'PagePopulation
    Protected Sub PopulatePage()

        Dim localTime As String = ""
        Dim localTime2 As String = ""
        Dim localTime3 As String = ""
        Dim EASBroadcastTime As String = ""
        Dim EASTransmissionTime As String = ""

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectDemINRByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            ddlSubType.SelectedValue = HelpFunction.Convertdbnulls(objDR("SubType"))
            ddlSituation.SelectedValue = HelpFunction.Convertdbnulls(objDR("Situation"))
            ddlSlrcSeocAlarmType.SelectedValue = HelpFunction.Convertdbnulls(objDR("SlrcSeocAlarmType"))
            txtSlrcSeocZoneNumber.Text = HelpFunction.Convertdbnulls(objDR("SlrcSeocZoneNumber"))
            ddlSlrcSeocAlarmStatus.SelectedValue = HelpFunction.Convertdbnulls(objDR("SlrcSeocAlarmStatus"))
            txtDepWarehouseMemo.Text = HelpFunction.Convertdbnulls(objDR("DepWarehouseMemo"))
            ddlDepWarehouseNotification.SelectedValue = HelpFunction.Convertdbnulls(objDR("DepWarehouseNotification"))
            txtDepWarehouseZoneNumber.Text = HelpFunction.Convertdbnulls(objDR("DepWarehouseZoneNumber"))
            ddlDepWarehouseAlarmStatus.SelectedValue = HelpFunction.Convertdbnulls(objDR("DepWarehouseAlarmStatus"))
            txtDepWarehouseEmployeeName.Text = HelpFunction.Convertdbnulls(objDR("DepWarehouseEmployeeName"))
            txtDepWarehouseEmployeeCellPhone.Text = HelpFunction.Convertdbnulls(objDR("DepWarehouseEmployeeCellPhone"))
            txtDepWarehouseAgencyDivision.Text = HelpFunction.Convertdbnulls(objDR("DepWarehouseAgencyDivision"))
            txtDepWarehouseSupervisorName.Text = HelpFunction.Convertdbnulls(objDR("DepWarehouseSupervisorName"))
            ddlDepWarehouseSupervisorCalled.SelectedValue = HelpFunction.Convertdbnulls(objDR("DepWarehouseSupervisorCalled"))
            txtDepWarehouseAccessCardNumber.Text = HelpFunction.Convertdbnulls(objDR("DepWarehouseAccessCardNumber"))
            txtMeBuildingRoomNumber.Text = HelpFunction.Convertdbnulls(objDR("MEBuildingRoomNumber"))
            ddlMe911Called.SelectedValue = HelpFunction.Convertdbnulls(objDR("Me911Called"))
            ddlMePersonBreathing.SelectedValue = HelpFunction.Convertdbnulls(objDR("MePersonBreathing"))
            ddlMeConsiousness.SelectedValue = HelpFunction.Convertdbnulls(objDR("MeConsiousness"))
            txtMeComplaintSymptom.Text = HelpFunction.Convertdbnulls(objDR("MeComplaintSymptom"))
            ddlSeocActivationLevel.SelectedValue = HelpFunction.Convertdbnulls(objDR("SeocActivationLevel"))
            txtSeocActivationRelatedIncidentNumbers.Text = HelpFunction.Convertdbnulls(objDR("SeocActivationRelatedIncidentNumbers"))
            ddlSeocActivationEmcDatabase.SelectedValue = HelpFunction.Convertdbnulls(objDR("SeocActivationEmcDatabase"))
            txtSeocActivationEmcDatabaseName.Text = HelpFunction.Convertdbnulls(objDR("SeocActivationEmcDatabaseName"))
            ddlSmtActivationSMT.SelectedValue = HelpFunction.Convertdbnulls(objDR("SmtActivationSMT"))
            txtSmtActivationReason.Text = HelpFunction.Convertdbnulls(objDR("SmtActivationReason"))
            txtSmtActivationReportLocation.Text = HelpFunction.Convertdbnulls(objDR("SmtActivationReportLocation"))
            txtSmtActivationAuthorizedBy.Text = HelpFunction.Convertdbnulls(objDR("SmtActivationAuthorizedBy"))
            ddlReservistActivationSMT.SelectedValue = HelpFunction.Convertdbnulls(objDR("ReservistActivationSMT"))
            txtReservistActivationReason.Text = HelpFunction.Convertdbnulls(objDR("ReservistActivationReason"))
            txtReservistActivationReportLocation.Text = HelpFunction.Convertdbnulls(objDR("ReservistActivationReportLocation"))
            txtReservistActivationAuthorizedBy.Text = HelpFunction.Convertdbnulls(objDR("ReservistActivationAuthorizedBy"))
            txtGeneralNotificationMessage.Text = HelpFunction.Convertdbnulls(objDR("GeneralNotificationMessage"))
            txtGeneralNotificationAuthorizedBy.Text = HelpFunction.Convertdbnulls(objDR("GeneralNotificationAuthorizedBy"))
            txtItDisruptionDescription.Text = HelpFunction.Convertdbnulls(objDR("ItDisruptionDescription"))
            txtItDisruptionprogramSystem.Text = HelpFunction.Convertdbnulls(objDR("ItDisruptionprogramSystem"))
            txtCommDisruptionSystemCircuitText.Text = HelpFunction.Convertdbnulls(objDR("CommDisruptionSystemCircuitText"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR("ItDisruptionTime")))
            txtItDisruptionStepsTaken.Text = HelpFunction.Convertdbnulls(objDR("ItDisruptionStepsTaken"))
            ddlCommDisruptionSystemCircuit.SelectedValue = HelpFunction.Convertdbnulls(objDR("CommDisruptionSystemCircuit"))
            txtCommDisruptionDescription.Text = HelpFunction.Convertdbnulls(objDR("CommDisruptionDescription"))
            localTime2 = CStr(HelpFunction.Convertdbnulls(objDR("CommDisruptionTime")))
            txtCommDisruptionStepsTaken.Text = HelpFunction.Convertdbnulls(objDR("CommDisruptionStepsTaken"))
            txtPlannedOutageDescription.Text = HelpFunction.Convertdbnulls(objDR("PlannedOutageDescription"))
            txtPlannedOutageScheduledStartDate.Text = HelpFunction.Convertdbnulls(objDR("PlannedOutageScheduledStartDate"))
            localTime3 = CStr(HelpFunction.Convertdbnulls(objDR("PlannedOutageScheduledStartTime")))
            txtPlannedOutageEstimatedCompletion.Text = HelpFunction.Convertdbnulls(objDR("PlannedOutageEstimatedCompletion"))
            txtPlannedOutagecontactNameNumber.Text = HelpFunction.Convertdbnulls(objDR("PlannedOutagecontactNameNumber"))
            ddlNotification.SelectedValue = HelpFunction.Convertdbnulls(objDR("IncidentTypeLevelID"))
            txtEASRequestorName.Text = HelpFunction.Convertdbnulls(objDR("EASRequestorName"))
            txtEASRequestReason.Text = HelpFunction.Convertdbnulls(objDR("EASRequestReason"))
            txtEASBroadcastDate.Text = HelpFunction.Convertdbnulls(objDR("EASBroadcastDate"))
            EASBroadcastTime = HelpFunction.Convertdbnulls(objDR("EASBroadcastTime"))
            txtEASBroadcastMessage.Text = HelpFunction.Convertdbnulls(objDR("EASBroadcastMessage"))
            txtEASRecommendedActions.Text = HelpFunction.Convertdbnulls(objDR("EASRecommendedActions"))
            txtEASLocationDescription.Text = HelpFunction.Convertdbnulls(objDR("EASLocationDescription"))
            txtEASTransmittedBy.Text = HelpFunction.Convertdbnulls(objDR("EASTransmittedBy"))
            EASTransmissionTime = HelpFunction.Convertdbnulls(objDR("EASTRansmissionTime"))
            ddlEASBroadcastDuration.SelectedValue = HelpFunction.Convertdbnulls(objDR("EASBroadcastDuration"))
            ddlEASLocation.SelectedValue = HelpFunction.Convertdbnulls(objDR("EASLocation"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        txtWorkSheetDescription.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        If ddlSubType.SelectedValue = "SLRC Alarm" Or ddlSubType.SelectedValue = "SEOC Alarm" Then
            pnlShowSlrcSeoc.Visible = True
        Else
            pnlShowSlrcSeoc.Visible = False
        End If

        If ddlSubType.SelectedValue = "DEP Alarm" Then
            pnlShowDepWarehouse.Visible = True
        Else
            pnlShowDepWarehouse.Visible = False
        End If

        If ddlSubType.SelectedValue = "Medical Emergency" Then
            pnlShowMedicalEmergency.Visible = True
        Else
            pnlShowMedicalEmergency.Visible = False
        End If

        If ddlSubType.SelectedValue = "SEOC Activation" Then
            pnlShowSeocActivation.Visible = True
        Else
            pnlShowSeocActivation.Visible = False
        End If

        If ddlSubType.SelectedValue = "SMT Activation" Then
            pnlShowSMTActivation.Visible = True
        Else
            pnlShowSMTActivation.Visible = False
        End If

        If ddlSubType.SelectedValue = "Reservist Activation" Then
            pnlShowReservistActivation.Visible = True
        Else
            pnlShowReservistActivation.Visible = False
        End If

        If ddlSubType.SelectedValue = "General Notification" Then
            pnlShowGeneralNotification.Visible = True
        Else
            pnlShowGeneralNotification.Visible = False
        End If

        If ddlSubType.SelectedValue = "IT Disruption or Issue" Then
            pnlShowItDisruptionIssue.Visible = True
        Else
            pnlShowItDisruptionIssue.Visible = False
        End If

        If ddlSubType.SelectedValue = "Communications Disruption or Issue" Then
            pnlShowCommunicationsDisruptionIssue.Visible = True
        Else
            pnlShowCommunicationsDisruptionIssue.Visible = False
        End If

        If ddlSubType.SelectedValue = "Planned Outage" Then
            pnlShowPlannedOutage.Visible = True
        Else
            pnlShowPlannedOutage.Visible = False
        End If

        If ddlSubType.SelectedValue = "EAS/IPAWS Activation" Then
            pnlShowEAS_IPAWS.Visible = True
        Else
            pnlShowEAS_IPAWS.Visible = False
        End If

        If ddlDepWarehouseNotification.SelectedValue = "Alarm" Then
            pnlShowAlarm.Visible = True
        Else
            pnlShowAlarm.Visible = False
        End If

        If ddlDepWarehouseNotification.SelectedValue = "Non-Alarm Notification" Then
            pnlShowNonAlarm.Visible = True
        Else
            pnlShowNonAlarm.Visible = False
        End If

        txtItDisruptionTime.Text = Left(localTime, 2)
        txtItDisruptionTime2.Text = Right(localTime, 2)


        'If txtItDisruptionTime.Text = "0" Then
        '    txtItDisruptionTime.Text = ""
        'End If

        'If txtItDisruptionTime2.Text = "0" Then
        '    txtItDisruptionTime2.Text = ""
        'End If

        txtCommDisruptionTime.Text = Left(localTime2, 2)
        txtCommDisruptionTime2.Text = Right(localTime2, 2)


        'If txtCommDisruptionTime.Text = "0" Then
        '    txtCommDisruptionTime.Text = ""
        'End If

        'If txtCommDisruptionTime2.Text = "0" Then
        '    txtCommDisruptionTime2.Text = ""
        'End If

        txtPlannedOutageScheduledStartTime.Text = Left(localTime3, 2)
        txtPlannedOutageScheduledStartTime2.Text = Right(localTime3, 2)

        txtEASBroadcastTime.Text = Left(EASBroadcastTime, 2)
        txtEASBroadcastTime2.Text = Right(EASBroadcastTime, 2)
        txtEASTRansmissionTime.Text = Left(EASTransmissionTime, 2)
        txtEASTRansmissionTime2.Text = Right(EASTransmissionTime, 2)

        'If txtPlannedOutageScheduledStartTime.Text = "0" Then
        '    txtPlannedOutageScheduledStartTime.Text = ""
        'End If

        'If txtPlannedOutageScheduledStartTime2.Text = "0" Then
        '    txtPlannedOutageScheduledStartTime2.Text = ""
        'End If

        If txtPlannedOutageScheduledStartDate.Text = "1/1/1900" Then
            txtPlannedOutageScheduledStartDate.Text = ""
        End If

        If ddlCommDisruptionSystemCircuit.SelectedValue = "Other" Then
            pnlShowCommunicationSystemOther.Visible = True
        End If

        If ddlEASLocation.SelectedValue = "Select an Option" Then
            tblEASLocationDescription.Visible = False
        End If

    End Sub

    Sub PopulateDDLs()

        'Notification Group
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectIncidentTypeLevelForDDL", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentTypeID", MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("IncidentTypeID", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID")))

        DBConStringHelper.PrepareConnection(objConn) 'Open the connection
        ddlNotification.DataSource = objCmd.ExecuteReader()
        ddlNotification.DataBind()
        DBConStringHelper.FinalizeConnection(objConn) 'Close the connection

        objCmd = Nothing

        'add an "Select an Option" item to the list
        ddlNotification.Items.Insert(0, New ListItem("Select an Option", "Select an Option"))
        ddlNotification.Items(0).Selected = True

    End Sub

    Protected Sub ddlSubType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubType.SelectedIndexChanged

        If ddlSubType.SelectedValue = "SLRC Alarm" Or ddlSubType.SelectedValue = "SEOC Alarm" Then
            pnlShowSlrcSeoc.Visible = True
        Else
            pnlShowSlrcSeoc.Visible = False
        End If

        If ddlSubType.SelectedValue = "DEP Alarm" Then
            pnlShowDepWarehouse.Visible = True
        Else
            pnlShowDepWarehouse.Visible = False
        End If

        If ddlSubType.SelectedValue = "Medical Emergency" Then
            pnlShowMedicalEmergency.Visible = True
        Else
            pnlShowMedicalEmergency.Visible = False
        End If

        If ddlSubType.SelectedValue = "SEOC Activation" Then
            pnlShowSeocActivation.Visible = True
        Else
            pnlShowSeocActivation.Visible = False
        End If

        If ddlSubType.SelectedValue = "SMT Activation" Then
            pnlShowSMTActivation.Visible = True
        Else
            pnlShowSMTActivation.Visible = False
        End If

        If ddlSubType.SelectedValue = "Reservist Activation" Then
            pnlShowReservistActivation.Visible = True
        Else
            pnlShowReservistActivation.Visible = False
        End If

        If ddlSubType.SelectedValue = "General Notification" Then
            pnlShowGeneralNotification.Visible = True
        Else
            pnlShowGeneralNotification.Visible = False
        End If

        If ddlSubType.SelectedValue = "IT Disruption or Issue" Then
            pnlShowItDisruptionIssue.Visible = True
        Else
            pnlShowItDisruptionIssue.Visible = False
        End If

        If ddlSubType.SelectedValue = "Communications Disruption or Issue" Then
            pnlShowCommunicationsDisruptionIssue.Visible = True
        Else
            pnlShowCommunicationsDisruptionIssue.Visible = False
        End If

        If ddlSubType.SelectedValue = "Planned Outage" Then
            pnlShowPlannedOutage.Visible = True
        Else
            pnlShowPlannedOutage.Visible = False
        End If

        If ddlSubType.SelectedValue = "EAS/IPAWS Activation" Then
            pnlShowEAS_IPAWS.Visible = True
        Else
            pnlShowEAS_IPAWS.Visible = False
        End If
    End Sub


    Protected Sub ddlDepWarehouseNotification_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDepWarehouseNotification.SelectedIndexChanged

        If ddlDepWarehouseNotification.SelectedValue = "Alarm" Then
            pnlShowAlarm.Visible = True
        Else
            pnlShowAlarm.Visible = False
        End If

        If ddlDepWarehouseNotification.SelectedValue = "Non-Alarm Notification" Then
            pnlShowNonAlarm.Visible = True
        Else
            pnlShowNonAlarm.Visible = False
        End If

    End Sub


    Protected Sub ddlCommDisruptionSystemCircuit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCommDisruptionSystemCircuit.SelectedIndexChanged

        If ddlCommDisruptionSystemCircuit.SelectedValue = "Other" Then
            pnlShowCommunicationSystemOther.Visible = True
        Else
            pnlShowCommunicationSystemOther.Visible = False
        End If

    End Sub


    'Buttons
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

        Dim localDemINRCount As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectDemINRCountByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            localDemINRCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        'Response.Write(pnlShowFatalityText.Visible.ToString)
        'Response.End()

        ''We add these to blank since the panels are not visible
   
        If pnlShowSlrcSeoc.Visible = False Then
            txtSlrcSeocZoneNumber.Text = ""
            ddlSlrcSeocAlarmType.SelectedValue = "Select an Option"
            ddlSlrcSeocAlarmStatus.SelectedValue = "Select an Option"
        End If

        If pnlShowDepWarehouse.Visible = False Then

            txtDepWarehouseMemo.Text = ""
            ddlDepWarehouseAlarmStatus.SelectedValue = "Select an Option"

            'Alarm
            txtDepWarehouseZoneNumber.Text = ""
            ddlDepWarehouseAlarmStatus.SelectedValue = "Select an Option"

            'Non Alarm
            txtDepWarehouseEmployeeName.Text = ""
            txtDepWarehouseEmployeeCellPhone.Text = ""
            txtDepWarehouseAgencyDivision.Text = ""
            txtDepWarehouseSupervisorName.Text = ""
            ddlDepWarehouseSupervisorCalled.SelectedValue = "Select an Option"
            txtDepWarehouseAccessCardNumber.Text = ""
        End If

        If pnlShowAlarm.Visible = False Then
            txtDepWarehouseZoneNumber.Text = ""
            ddlDepWarehouseAlarmStatus.SelectedValue = "Select an Option"
        End If

        If pnlShowNonAlarm.Visible = False Then
            txtDepWarehouseEmployeeName.Text = ""
            txtDepWarehouseEmployeeCellPhone.Text = ""
            txtDepWarehouseAgencyDivision.Text = ""
            txtDepWarehouseSupervisorName.Text = ""
            ddlDepWarehouseSupervisorCalled.SelectedValue = "Select an Option"
            txtDepWarehouseAccessCardNumber.Text = ""
        End If

        If pnlShowMedicalEmergency.Visible = False Then
            txtMeBuildingRoomNumber.Text = ""
            ddlMe911Called.SelectedValue = "Select an Option"
            ddlMePersonBreathing.SelectedValue = "Select an Option"
            ddlMeConsiousness.SelectedValue = "Select an Option"
            txtMeComplaintSymptom.Text = ""
        End If

        If pnlShowSeocActivation.Visible = False Then
            ddlSeocActivationLevel.SelectedValue = "Select an Option"
            txtSeocActivationRelatedIncidentNumbers.Text = ""
            ddlSeocActivationEmcDatabase.SelectedValue = "Select an Option"
            txtSeocActivationEmcDatabaseName.Text = ""
        End If

        If pnlShowSMTActivation.Visible = False Then
            ddlSmtActivationSMT.SelectedValue = "Select an Option"
            txtSmtActivationReason.Text = ""
            txtSmtActivationReportLocation.Text = ""
            txtSmtActivationAuthorizedBy.Text = ""
        End If

        If pnlShowReservistActivation.Visible = False Then
            ddlReservistActivationSMT.SelectedValue = "Select an Option"
            txtReservistActivationReason.Text = ""
            txtReservistActivationReportLocation.Text = ""
            txtReservistActivationAuthorizedBy.Text = ""
        End If

        If pnlShowGeneralNotification.Visible = False Then
            txtGeneralNotificationMessage.Text = ""
            txtGeneralNotificationAuthorizedBy.Text = ""
        End If

        If pnlShowItDisruptionIssue.Visible = False Then
            txtItDisruptionDescription.Text = ""
            txtItDisruptionprogramSystem.Text = ""
            txtItDisruptionTime.Text = ""
            txtItDisruptionTime2.Text = ""
            txtItDisruptionStepsTaken.Text = ""
        End If

        If pnlShowCommunicationsDisruptionIssue.Visible = False Then
            ddlCommDisruptionSystemCircuit.SelectedValue = "Select an Option"
            txtCommDisruptionSystemCircuitText.Text = ""
            txtCommDisruptionDescription.Text = ""
            'txtCommDisruptionTime.Text = 0
            'txtCommDisruptionTime2.Text = 0
            txtCommDisruptionStepsTaken.Text = ""
        End If

        If pnlShowCommunicationSystemOther.Visible = False Then
            txtCommDisruptionSystemCircuitText.Text = ""
        End If

        If pnlShowPlannedOutage.Visible = False Then
            txtPlannedOutagecontactNameNumber.Text = ""
            txtPlannedOutageDescription.Text = ""
            txtPlannedOutageEstimatedCompletion.Text = ""
            txtPlannedOutageScheduledStartDate.Text = "1/1/1900"
            'txtPlannedOutageScheduledStartTime.Text = 0
            'txtPlannedOutageScheduledStartTime2.Text = 0
        End If

        If pnlShowEAS_IPAWS.Visible = False Then
            txtEASRequestorName.Text = ""
            txtEASRequestReason.Text = ""
            txtEASBroadcastDate.Text = ""
            txtEASBroadcastTime.Text = ""
            txtEASBroadcastTime2.Text = ""
            ddlEASBroadcastDuration.SelectedValue = "Select an Option"
            txtEASBroadcastMessage.Text = ""
            txtEASRecommendedActions.Text = ""
            ddlEASLocation.SelectedValue = "Select an Option"
            txtEASLocationDescription.Text = ""
            txtEASTransmittedBy.Text = ""
            txtEASTRansmissionTime.Text = ""
            txtEASTRansmissionTime2.Text = ""
        End If

        If localDemINRCount = 0 Then

            'Try

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionDemINR", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 0)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@SlrcSeocAlarmType", ddlSlrcSeocAlarmType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@SlrcSeocZoneNumber", txtSlrcSeocZoneNumber.Text)
            objCmd.Parameters.AddWithValue("@SlrcSeocAlarmStatus", ddlSlrcSeocAlarmStatus.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DepWarehouseMemo", txtDepWarehouseMemo.Text)
            objCmd.Parameters.AddWithValue("@DepWarehouseNotification", ddlDepWarehouseNotification.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DepWarehouseZoneNumber", txtDepWarehouseZoneNumber.Text)
            objCmd.Parameters.AddWithValue("@DepWarehouseAlarmStatus", ddlDepWarehouseAlarmStatus.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DepWarehouseEmployeeName", txtDepWarehouseEmployeeName.Text)
            objCmd.Parameters.AddWithValue("@DepWarehouseEmployeeCellPhone", txtDepWarehouseEmployeeCellPhone.Text)
            objCmd.Parameters.AddWithValue("@DepWarehouseAgencyDivision", txtDepWarehouseAgencyDivision.Text)
            objCmd.Parameters.AddWithValue("@DepWarehouseSupervisorName", txtDepWarehouseSupervisorName.Text)
            objCmd.Parameters.AddWithValue("@DepWarehouseSupervisorCalled", ddlDepWarehouseSupervisorCalled.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DepWarehouseAccessCardNumber", txtDepWarehouseAccessCardNumber.Text)
            objCmd.Parameters.AddWithValue("@MEBuildingRoomNumber", txtMeBuildingRoomNumber.Text)
            objCmd.Parameters.AddWithValue("@Me911Called ", ddlMe911Called.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@MePersonBreathing ", ddlMePersonBreathing.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@MeConsiousness ", ddlMeConsiousness.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@MeComplaintSymptom", txtMeComplaintSymptom.Text)
            objCmd.Parameters.AddWithValue("@SeocActivationLevel", ddlSeocActivationLevel.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@SeocActivationRelatedIncidentNumbers", txtSeocActivationRelatedIncidentNumbers.Text)
            objCmd.Parameters.AddWithValue("@SeocActivationEmcDatabase", ddlSeocActivationEmcDatabase.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@SeocActivationEmcDatabaseName", txtSeocActivationEmcDatabaseName.Text)
            objCmd.Parameters.AddWithValue("@SmtActivationSMT", ddlSmtActivationSMT.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@SmtActivationReason", txtSmtActivationReason.Text)
            objCmd.Parameters.AddWithValue("@SmtActivationReportLocation", txtSmtActivationReportLocation.Text)
            objCmd.Parameters.AddWithValue("@SmtActivationAuthorizedBy", txtSmtActivationAuthorizedBy.Text)
            objCmd.Parameters.AddWithValue("@ReservistActivationSMT", ddlReservistActivationSMT.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@ReservistActivationReason", txtReservistActivationReason.Text)
            objCmd.Parameters.AddWithValue("@ReservistActivationReportLocation", txtReservistActivationReportLocation.Text)
            objCmd.Parameters.AddWithValue("@ReservistActivationAuthorizedBy ", txtReservistActivationAuthorizedBy.Text)
            objCmd.Parameters.AddWithValue("@GeneralNotificationMessage", txtGeneralNotificationMessage.Text)
            objCmd.Parameters.AddWithValue("@GeneralNotificationAuthorizedBy", txtGeneralNotificationAuthorizedBy.Text)
            objCmd.Parameters.AddWithValue("@ItDisruptionDescription", txtItDisruptionDescription.Text)
            objCmd.Parameters.AddWithValue("@ItDisruptionprogramSystem", txtItDisruptionprogramSystem.Text)
            objCmd.Parameters.AddWithValue("@ItDisruptionTime", CStr(txtItDisruptionTime.Text.Trim) & CStr(txtItDisruptionTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@ItDisruptionStepsTaken", txtItDisruptionStepsTaken.Text)
            objCmd.Parameters.AddWithValue("@CommDisruptionSystemCircuit", ddlCommDisruptionSystemCircuit.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CommDisruptionSystemCircuitText", txtCommDisruptionSystemCircuitText.Text)
            objCmd.Parameters.AddWithValue("@CommDisruptionDescription", txtCommDisruptionDescription.Text)
            objCmd.Parameters.AddWithValue("@CommDisruptionTime", CStr(txtCommDisruptionTime.Text.Trim) & CStr(txtCommDisruptionTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@CommDisruptionStepsTaken", txtCommDisruptionStepsTaken.Text)
            objCmd.Parameters.AddWithValue("@PlannedOutageDescription ", txtPlannedOutageDescription.Text)
            objCmd.Parameters.AddWithValue("@PlannedOutageScheduledStartDate", txtPlannedOutageScheduledStartDate.Text)
            objCmd.Parameters.AddWithValue("@PlannedOutageScheduledStartTime", CStr(txtPlannedOutageScheduledStartTime.Text.Trim) & CStr(txtPlannedOutageScheduledStartTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@PlannedOutageEstimatedCompletion", txtPlannedOutageEstimatedCompletion.Text)
            objCmd.Parameters.AddWithValue("@PlannedOutagecontactNameNumber", txtPlannedOutagecontactNameNumber.Text)
            objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", ddlNotification.SelectedValue)
            objCmd.Parameters.AddWithValue("@EASRequestorName", txtEASRequestorName.Text)
            objCmd.Parameters.AddWithValue("@EASRequestReason", txtEASRequestReason.Text)
            objCmd.Parameters.AddWithValue("@EASBroadcastDate", txtEASBroadcastDate.Text)
            objCmd.Parameters.AddWithValue("@EASBroadcastTime", CStr(txtEASBroadcastTime.Text.Trim) & CStr(txtEASBroadcastTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@EASBroadcastMessage", txtEASBroadcastMessage.Text)
            objCmd.Parameters.AddWithValue("@EASRecommendedActions", txtEASRecommendedActions.Text)
            objCmd.Parameters.AddWithValue("@EASLocationDescription", txtEASLocationDescription.Text)
            objCmd.Parameters.AddWithValue("@EASTransmittedBy", txtEASTransmittedBy.Text)
            objCmd.Parameters.AddWithValue("@EASTRansmissionTime", CStr(txtEASTRansmissionTime.Text.Trim) & CStr(txtEASTRansmissionTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@EASBroadcastDuration", ddlEASBroadcastDuration.SelectedValue)
            objCmd.Parameters.AddWithValue("@EASLocation", ddlEASLocation.SelectedValue)

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

                '// Enter the email and password to query/command object.
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

                '// Enter the email and password to query/command object.

                objCmd = New SqlCommand("spInsertMostRecentUpdateByIncidentID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                objCmd.Parameters.AddWithValue("@UpdateDate", NowDate)
                objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Added DEM INR")

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

                '// Enter the email and password to query/command object.
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

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionDemINR", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 1)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@SlrcSeocAlarmType", ddlSlrcSeocAlarmType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@SlrcSeocZoneNumber", txtSlrcSeocZoneNumber.Text)
            objCmd.Parameters.AddWithValue("@SlrcSeocAlarmStatus", ddlSlrcSeocAlarmStatus.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DepWarehouseMemo", txtDepWarehouseMemo.Text)
            objCmd.Parameters.AddWithValue("@DepWarehouseNotification", ddlDepWarehouseNotification.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DepWarehouseZoneNumber", txtDepWarehouseZoneNumber.Text)
            objCmd.Parameters.AddWithValue("@DepWarehouseAlarmStatus", ddlDepWarehouseAlarmStatus.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DepWarehouseEmployeeName", txtDepWarehouseEmployeeName.Text)
            objCmd.Parameters.AddWithValue("@DepWarehouseEmployeeCellPhone", txtDepWarehouseEmployeeCellPhone.Text)
            objCmd.Parameters.AddWithValue("@DepWarehouseAgencyDivision", txtDepWarehouseAgencyDivision.Text)
            objCmd.Parameters.AddWithValue("@DepWarehouseSupervisorName", txtDepWarehouseSupervisorName.Text)
            objCmd.Parameters.AddWithValue("@DepWarehouseSupervisorCalled", ddlDepWarehouseSupervisorCalled.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DepWarehouseAccessCardNumber", txtDepWarehouseAccessCardNumber.Text)
            objCmd.Parameters.AddWithValue("@MEBuildingRoomNumber", txtMeBuildingRoomNumber.Text)
            objCmd.Parameters.AddWithValue("@Me911Called ", ddlMe911Called.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@MePersonBreathing ", ddlMePersonBreathing.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@MeConsiousness ", ddlMeConsiousness.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@MeComplaintSymptom", txtMeComplaintSymptom.Text)
            objCmd.Parameters.AddWithValue("@SeocActivationLevel", ddlSeocActivationLevel.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@SeocActivationRelatedIncidentNumbers", txtSeocActivationRelatedIncidentNumbers.Text)
            objCmd.Parameters.AddWithValue("@SeocActivationEmcDatabase", ddlSeocActivationEmcDatabase.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@SeocActivationEmcDatabaseName", txtSeocActivationEmcDatabaseName.Text)
            objCmd.Parameters.AddWithValue("@SmtActivationSMT", ddlSmtActivationSMT.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@SmtActivationReason", txtSmtActivationReason.Text)
            objCmd.Parameters.AddWithValue("@SmtActivationReportLocation", txtSmtActivationReportLocation.Text)
            objCmd.Parameters.AddWithValue("@SmtActivationAuthorizedBy", txtSmtActivationAuthorizedBy.Text)
            objCmd.Parameters.AddWithValue("@ReservistActivationSMT", ddlReservistActivationSMT.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@ReservistActivationReason", txtReservistActivationReason.Text)
            objCmd.Parameters.AddWithValue("@ReservistActivationReportLocation", txtReservistActivationReportLocation.Text)
            objCmd.Parameters.AddWithValue("@ReservistActivationAuthorizedBy ", txtReservistActivationAuthorizedBy.Text)
            objCmd.Parameters.AddWithValue("@GeneralNotificationMessage", txtGeneralNotificationMessage.Text)
            objCmd.Parameters.AddWithValue("@GeneralNotificationAuthorizedBy", txtGeneralNotificationAuthorizedBy.Text)
            objCmd.Parameters.AddWithValue("@ItDisruptionDescription", txtItDisruptionDescription.Text)
            objCmd.Parameters.AddWithValue("@ItDisruptionprogramSystem", txtItDisruptionprogramSystem.Text)
            objCmd.Parameters.AddWithValue("@ItDisruptionTime", CStr(txtItDisruptionTime.Text.Trim) & CStr(txtItDisruptionTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@ItDisruptionStepsTaken", txtItDisruptionStepsTaken.Text)
            objCmd.Parameters.AddWithValue("@CommDisruptionSystemCircuit", ddlCommDisruptionSystemCircuit.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CommDisruptionSystemCircuitText", txtCommDisruptionSystemCircuitText.Text)
            objCmd.Parameters.AddWithValue("@CommDisruptionDescription", txtCommDisruptionDescription.Text)
            objCmd.Parameters.AddWithValue("@CommDisruptionTime", CStr(txtCommDisruptionTime.Text.Trim) & CStr(txtCommDisruptionTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@CommDisruptionStepsTaken", txtCommDisruptionStepsTaken.Text)
            objCmd.Parameters.AddWithValue("@PlannedOutageDescription ", txtPlannedOutageDescription.Text)
            objCmd.Parameters.AddWithValue("@PlannedOutageScheduledStartDate", txtPlannedOutageScheduledStartDate.Text)
            objCmd.Parameters.AddWithValue("@PlannedOutageScheduledStartTime", CStr(txtPlannedOutageScheduledStartTime.Text.Trim) & CStr(txtPlannedOutageScheduledStartTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@PlannedOutageEstimatedCompletion", txtPlannedOutageEstimatedCompletion.Text)
            objCmd.Parameters.AddWithValue("@PlannedOutagecontactNameNumber", txtPlannedOutagecontactNameNumber.Text)
            objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", ddlNotification.SelectedValue)
            objCmd.Parameters.AddWithValue("@EASRequestorName", txtEASRequestorName.Text)
            objCmd.Parameters.AddWithValue("@EASRequestReason", txtEASRequestReason.Text)
            objCmd.Parameters.AddWithValue("@EASBroadcastDate", txtEASBroadcastDate.Text)
            objCmd.Parameters.AddWithValue("@EASBroadcastTime", CStr(txtEASBroadcastTime.Text.Trim) & CStr(txtEASBroadcastTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@EASBroadcastMessage", txtEASBroadcastMessage.Text)
            objCmd.Parameters.AddWithValue("@EASRecommendedActions", txtEASRecommendedActions.Text)
            objCmd.Parameters.AddWithValue("@EASLocationDescription", txtEASLocationDescription.Text)
            objCmd.Parameters.AddWithValue("@EASTransmittedBy", txtEASTransmittedBy.Text)
            objCmd.Parameters.AddWithValue("@EASTRansmissionTime", CStr(txtEASTRansmissionTime.Text.Trim) & CStr(txtEASTRansmissionTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@EASBroadcastDuration", ddlEASBroadcastDuration.SelectedValue)
            objCmd.Parameters.AddWithValue("@EASLocation", ddlEASLocation.SelectedValue)

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

                '// Enter the email and password to query/command object.
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

                '// Enter the email and password to query/command object.

                objCmd = New SqlCommand("spInsertMostRecentUpdateByIncidentID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                objCmd.Parameters.AddWithValue("@UpdateDate", NowDate)
                objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Updated DEM INR")

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

                '// Enter the email and password to query/command object.
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

        If ddlSubType.SelectedValue = "EAS/IPAWS Activation" Then
            If String.IsNullOrEmpty(txtEASRequestorName.Text) Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Requestor Name. <br />")
                globalHasErrors = True
            End If
            If String.IsNullOrEmpty(txtEASRequestReason.Text) Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Reason for Request. <br />")
                globalHasErrors = True
            End If
            If String.IsNullOrEmpty(txtEASBroadcastDate.Text) Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Requested Broadcast Date. <br />")
                globalHasErrors = True
            End If
            If String.IsNullOrEmpty(txtEASBroadcastTime.Text) Or String.IsNullOrEmpty(txtEASBroadcastTime2.Text) Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Requested Broadcast Time. <br />")
                globalHasErrors = True
            End If
            If String.IsNullOrEmpty(txtEASTransmittedBy.Text) Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide an Alert Transmitted By value. <br />")
                globalHasErrors = True
            End If
            If String.IsNullOrEmpty(txtEASTRansmissionTime.Text) Or String.IsNullOrEmpty(txtEASTRansmissionTime2.Text) Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide an Alert Transmitted Time. <br />")
                globalHasErrors = True
            End If
        End If


        'Finish the Error String.
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels.
        lblMessage.Text = strError.ToString
    End Sub

    Protected Sub ddlEASLocation_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlEASLocation.SelectedIndexChanged
        tblEASLocationDescription.Visible = ddlEASLocation.SelectedIndex <> 0
    End Sub
End Class
