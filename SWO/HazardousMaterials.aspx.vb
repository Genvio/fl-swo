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

Partial Class HazardousMaterials
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

        'Response.End()

        'If Request("IncidentID") = "" Then

        '    Response.Redirect("Incident.aspx")

        'End If

        If Page.IsPostBack = False Then
            'set release link]
            lnkRelease.Target = "_blank"
            lnkRelease.NavigateUrl = "HazmatRelease.aspx?IncidentID=" & Request("IncidentID") & "&IncidentIncidentTypeID=" & Request("IncidentIncidentTypeID")
            'set message
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

            Dim localHazardousMaterialsCount As Integer = 0

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectHazardousMaterialsCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                localHazardousMaterialsCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

            End If

            objDR.Close()

            objCmd.Dispose()

            objCmd = Nothing

            objConn.Close()

           

            If localHazardousMaterialsCount > 0 Then
                PopulatePage()
                lnkRelease.Visible = True
            Else
                lnkRelease.Visible = False
            End If

        End If

    End Sub

    'PagePopulation
    Protected Sub PopulatePage()

        Dim localTime As String = ""
        Dim localTime2 As String = ""

        'Response.Write("2")
        'Response.End()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectHazardousMaterialsByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            ddlSubType.SelectedValue = HelpFunction.Convertdbnulls(objDR("SubType"))
            ddlSituation.SelectedValue = HelpFunction.Convertdbnulls(objDR("Situation"))
            txtCommonName.Text = HelpFunction.Convertdbnulls(objDR("CommonName"))
            txtScientificName.Text = HelpFunction.Convertdbnulls(objDR("ScientificName"))
            txtQuantityDescription.Text = HelpFunction.Convertdbnulls(objDR("QuantityDescription"))
            txtContainerDeviceDescription.Text = HelpFunction.Convertdbnulls(objDR("ContainerDeviceDescription"))
            txtBiologicalTotalQuantity.Text = HelpFunction.Convertdbnulls(objDR("BiologicalTotalQuantity"))
            txtBiologicalQuantityReleased.Text = HelpFunction.Convertdbnulls(objDR("BiologicalQuantityReleased"))
            ddlAgentType.SelectedValue = HelpFunction.Convertdbnulls(objDR("AgentType"))
            txtAgentName.Text = HelpFunction.Convertdbnulls(objDR("AgentName"))
            txtAgentContainerDeviceDescription.Text = HelpFunction.Convertdbnulls(objDR("AgentContainerDeviceDescription"))
            txtAgentTotalQuantity.Text = HelpFunction.Convertdbnulls(objDR("AgentTotalQuantity"))
            txtAgentQuantityReleased.Text = HelpFunction.Convertdbnulls(objDR("AgentQuantityReleased"))
            ddlRadiationType.SelectedValue = HelpFunction.Convertdbnulls(objDR("RadiationType"))
            txtIsotopeName.Text = HelpFunction.Convertdbnulls(objDR("IsotopeName"))
            txtContainerDeviceInstrumentDescription.Text = HelpFunction.Convertdbnulls(objDR("ContainerDeviceInstrumentDescription"))
            ddlDOHBureauNotified.SelectedValue = HelpFunction.Convertdbnulls(objDR("DOHBureauNotified"))
            txtChemicalName.Text = HelpFunction.Convertdbnulls(objDR("ChemicalName"))
            txtIndexName.Text = HelpFunction.Convertdbnulls(objDR("IndexName"))
            txtCASNumber.Text = HelpFunction.Convertdbnulls(objDR("CASNumber"))
            txtCERCLAReportableQuantity.Text = HelpFunction.Convertdbnulls(objDR("CERCLAReportableQuantity"))
            ddlChemicalState.SelectedValue = HelpFunction.Convertdbnulls(objDR("ChemicalState"))
            ddlSourceContainer.SelectedValue = HelpFunction.Convertdbnulls(objDR("SourceContainer"))
            txtDiameterPipeline.Text = HelpFunction.Convertdbnulls(objDR("DiameterPipeline"))
            txtUnbrokenEndPipeConnectedTo.Text = HelpFunction.Convertdbnulls(objDR("UnbrokenEndPipeConnectedTo"))
            txtTotalSourceContainerVolume.Text = HelpFunction.Convertdbnulls(objDR("TotalSourceContainerVolume"))
            txtChemicalRateOfRelease.Text = HelpFunction.Convertdbnulls(objDR("ChemicalRateOfRelease"))
            ddlChemicalReleased.SelectedValue = HelpFunction.Convertdbnulls(objDR("ChemicalReleased"))
            txtCauseOfRelease.Text = HelpFunction.Convertdbnulls(objDR("CauseOfRelease"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR("TimeReleaseDiscovered")))
            localTime2 = CStr(HelpFunction.Convertdbnulls(objDR("TimeReleaseSecured")))
            txtReasonLateReport.Text = HelpFunction.Convertdbnulls(objDR("ReasonLateReport"))
            ddlStormDrainsAffected.SelectedValue = HelpFunction.Convertdbnulls(objDR("StormDrainsAffected"))
            ddlWaterwaysAffected.SelectedValue = HelpFunction.Convertdbnulls(objDR("WaterwaysAffected"))
            txtWaterwaysAffectedText.Text = HelpFunction.Convertdbnulls(objDR("WaterwaysAffectedText"))
            ddlCallbackDEPRequested.SelectedValue = HelpFunction.Convertdbnulls(objDR("CallbackDEPRequested"))
            ddlCallbackDEPRequestedDDLValue.SelectedValue = HelpFunction.Convertdbnulls(objDR("CallbackDEPRequestedDDLValue"))
            ddlEvacuations.SelectedValue = HelpFunction.Convertdbnulls(objDR("Evacuations"))
            ddlMajorRoadwaysClosed.SelectedValue = HelpFunction.Convertdbnulls(objDR("MajorRoadwaysClosed"))
            ddlInjury.SelectedValue = HelpFunction.Convertdbnulls(objDR("Injury"))
            txtInjury.Text = HelpFunction.Convertdbnulls(objDR("InjuryText"))
            ddlFatality.SelectedValue = HelpFunction.Convertdbnulls(objDR("Fatality"))
            txtFatalityText.Text = HelpFunction.Convertdbnulls(objDR("FatalityText"))
            txtRadiationTotalQuantity.Text = HelpFunction.Convertdbnulls(objDR("RadiationTotalQuantity"))
            txtSection304ReportableQuantity.Text = HelpFunction.Convertdbnulls(objDR("Section304ReportableQuantity"))
            txtChemicalQuantityReleased.Text = HelpFunction.Convertdbnulls(objDR("ChemicalQuantityReleased"))
            ddlNotification.SelectedValue = HelpFunction.Convertdbnulls(objDR("IncidentTypeLevelID"))
            ddlUHChemicalState.SelectedValue = HelpFunction.Convertdbnulls(objDR("UHChemicalState"))
            ddlUHSourceContainer.SelectedValue = HelpFunction.Convertdbnulls(objDR("UHSourceContainer"))
            txtUHTotalSourceContainerVolume.Text = HelpFunction.Convertdbnulls(objDR("UHTotalSourceContainerVolume"))
            txtUHChemicalQuantityReleased.Text = HelpFunction.Convertdbnulls(objDR("UHChemicalQuantityReleased"))
            txtUHChemicalRateOfRelease.Text = HelpFunction.Convertdbnulls(objDR("UHChemicalRateOfRelease"))
            ddlUHChemicalReleased.SelectedValue = HelpFunction.Convertdbnulls(objDR("UHChemicalReleased"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        txtWorkSheetDescription.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        'Unknown Hazard
        If ddlSubType.SelectedValue = "Unknown Hazard" Then

            pnlUnknownHazard.Visible = True

        Else

            pnlUnknownHazard.Visible = False

        End If

        'Biological Hazard 
        If ddlSubType.SelectedValue = "Biological Hazard" Then

            pnlShowBiologicalHazard.Visible = True

        Else

            pnlShowBiologicalHazard.Visible = False

        End If

        'Chemical Agent 
        If ddlSubType.SelectedValue = "Chemical Agent" Then

            pnlShowChemicalAgent.Visible = True

        Else

            pnlShowChemicalAgent.Visible = False

        End If

        'Radiological Material
        If ddlSubType.SelectedValue = "Radiological Material" Then

            pnlShowRadiologicalMaterial.Visible = True

        Else

            pnlShowRadiologicalMaterial.Visible = False

        End If

        'Toxic Industrial Chemical
        If ddlSubType.SelectedValue = "Toxic Industrial Chemical" Then

            pnlShowToxicIndustrialChemical.Visible = True

        Else

            pnlShowToxicIndustrialChemical.Visible = False

        End If

        If ddlSourceContainer.SelectedValue = "Aboveground Pipeline" Or ddlSourceContainer.SelectedValue = "Underground Pipeline" Then
            pnlShowPipeline.Visible = True
        Else
            pnlShowPipeline.Visible = False
        End If

        If ddlWaterwaysAffected.SelectedValue = "Yes" Then
            pnlShowWaterwaysAffectedText.Visible = True
        End If


        'Making the time All Morris Day like
        txtTimeReleaseDiscovered.Text = Left(localTime, 2)
        txtTimeReleaseDiscovered2.Text = Right(localTime, 2)

        txtTimeReleaseSecured.Text = Left(localTime2, 2)
        txtTimeReleaseSecured2.Text = Right(localTime2, 2)

        If txtTimeReleaseDiscovered.Text = "0" Then
            txtTimeReleaseDiscovered.Text = ""
        End If

        If txtTimeReleaseSecured.Text = "0" Then
            txtTimeReleaseSecured.Text = ""
        End If

        If txtTimeReleaseSecured2.Text = "0" Then
            txtTimeReleaseSecured2.Text = ""
        End If

        If txtTimeReleaseDiscovered2.Text = "0" Then
            txtTimeReleaseDiscovered2.Text = ""
        End If

        If ddlInjury.SelectedValue = "Yes" Then
            'pnlShowInjuryText.Visible = True
        End If

        If ddlFatality.SelectedValue = "Yes" Then
            'pnlShowFatalityText.Visible = True
        End If

        If ddlCallbackDEPRequested.SelectedValue = "Yes" Then
            'pnlShowRegionalAssistanceRequested.Visible = True
        Else
            pnlShowRegionalAssistanceRequested.Visible = False
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

    'SubTypes Selection
    Protected Sub ddlSubType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubType.SelectedIndexChanged

        'Unknown Hazard
        If ddlSubType.SelectedValue = "Unknown Hazard" Then

            pnlUnknownHazard.Visible = True

        Else

            pnlUnknownHazard.Visible = False

        End If

        'Biological Hazard 
        If ddlSubType.SelectedValue = "Biological Hazard" Then

            pnlShowBiologicalHazard.Visible = True

        Else

            pnlShowBiologicalHazard.Visible = False

        End If

        'Chemical Agent 
        If ddlSubType.SelectedValue = "Chemical Agent" Then

            pnlShowChemicalAgent.Visible = True

        Else

            pnlShowChemicalAgent.Visible = False

        End If

        'Radiological Material
        If ddlSubType.SelectedValue = "Radiological Material" Then

            pnlShowRadiologicalMaterial.Visible = True

        Else

            pnlShowRadiologicalMaterial.Visible = False

        End If

        'Toxic Industrial Chemical
        If ddlSubType.SelectedValue = "Toxic Industrial Chemical" Then

            pnlShowToxicIndustrialChemical.Visible = True

        Else

            pnlShowToxicIndustrialChemical.Visible = False

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


    'ErrorChecks
    Protected Sub ErrorChecksStep1()

        Dim strError As New System.Text.StringBuilder

        'Start The Error String
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        

        'If pnlShowToxicIndustrialChemical.Visible = True Then

        '    'Time Validation
        '    If txtTimeReleaseDiscovered.Text = "" Then

        '        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Time the release was discovered. <br />")
        '        globalHasErrors = True

        '    End If

        '    If txtTimeReleaseDiscovered.Text <> "" Then

        '        If txtTimeReleaseDiscovered2.Text = "" Then

        '            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Time the release was discovered. <br />")
        '            globalHasErrors = True

        '        End If

        '        If txtTimeReleaseDiscovered2.Text <> "" Then
        '            'Now we check if its an integer

        '            Try
        '                Dim time1 As Integer = CInt(txtTimeReleaseDiscovered.Text)
        '                Dim time2 As Integer = CInt(txtTimeReleaseDiscovered2.Text)

        '                If time1 > 23 Or time1 < 0 Then
        '                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Time the release was discovered. <br />")
        '                    globalHasErrors = True
        '                    Exit Try
        '                End If

        '                If time2 > 59 Or time2 < 0 Then
        '                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Time the release was discovered.<br />")
        '                    globalHasErrors = True
        '                    Exit Try
        '                End If

        '            Catch ex As Exception
        '                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Incident Occurred Time. <br />")
        '                globalHasErrors = True
        '            End Try


        '        End If

        '    End If



        '    'Time Validation
        '    If txtTimeReleaseSecured.Text = "" Then

        '        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Time the release was secured. <br />")
        '        globalHasErrors = True

        '    End If

        '    If txtTimeReleaseSecured.Text <> "" Then

        '        If txtTimeReleaseSecured2.Text = "" Then

        '            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Time the release was secured. <br />")
        '            globalHasErrors = True

        '        End If

        '        If txtTimeReleaseDiscovered2.Text <> "" Then
        '            'Now we check if its an integer

        '            Try
        '                Dim time1 As Integer = CInt(txtTimeReleaseSecured.Text)
        '                Dim time2 As Integer = CInt(txtTimeReleaseSecured2.Text)

        '                If time1 > 23 Or time1 < 0 Then
        '                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Time the release was secured. <br />")
        '                    globalHasErrors = True
        '                    Exit Try
        '                End If

        '                If time2 > 59 Or time2 < 0 Then
        '                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Time the release was secured.<br />")
        '                    globalHasErrors = True
        '                    Exit Try
        '                End If

        '            Catch ex As Exception
        '                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Time the release was secured. <br />")
        '                globalHasErrors = True
        '            End Try


        '        End If

        '    End If


        'End If
        
 
        'Finish the Error String
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels
        lblMessage.Text = strError.ToString


    End Sub





    'Buttons
    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As EventArgs)

        'ErrorChecksStep1()

        ''Response.Write(globalHasErrors)
        ''Response.End()

        'If globalHasErrors = True Then

        '    'If we have errors, Show Message and Exit Sub. No Insert of Record
        '    pnlMessage.Visible = True
        '    'pnlMessage2.Visible = True

        '    globalHasErrors = False

        '    Exit Sub

        'Else

        '    Save()

        '    'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID") & "&Parameter=WorkSheet")
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)

        'End If



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

        Dim localHazardousMaterialsCount As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectHazardousMaterialsCountByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            localHazardousMaterialsCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

        End If

        objDR.Close()

        objCmd.Dispose()

        objCmd = Nothing

        objConn.Close()

        'Response.Write(localCivilDisturbanceCount)
        'Response.End()

        'Response.Write(pnlShowFatalityText.Visible.ToString)
        'Response.End()

        'Unknown Hazard
        If pnlUnknownHazard.Visible = False Then
            ddlUHChemicalState.SelectedValue = "Select an Option"
            ddlUHSourceContainer.SelectedValue = "Select an Option"
            txtUHTotalSourceContainerVolume.Text = ""
            txtUHChemicalQuantityReleased.Text = ""
            txtUHChemicalRateOfRelease.Text = ""
            ddlUHChemicalReleased.SelectedValue = "Select an Option"
        End If

        'Biological Hazard 
        If pnlShowBiologicalHazard.Visible = False Then

            txtCommonName.Text = ""
            txtScientificName.Text = ""
            txtQuantityDescription.Text = ""
            txtContainerDeviceDescription.Text = ""
            txtBiologicalTotalQuantity.Text = ""
            txtBiologicalQuantityReleased.Text = ""

        End If

        'Chemical Agent 
        If pnlShowChemicalAgent.Visible = False Then

            ddlAgentType.SelectedValue = "Select an Option"
            txtAgentName.Text = ""
            txtAgentContainerDeviceDescription.Text = ""
            txtAgentTotalQuantity.Text = ""
            txtAgentQuantityReleased.Text = ""

        End If

        'Radiological Material
        If pnlShowRadiologicalMaterial.Visible = False Then

            ddlDOHBureauNotified.SelectedValue = "Select an Option"
            ddlRadiationType.SelectedValue = "Select an Option"
            txtIsotopeName.Text = ""
            txtContainerDeviceInstrumentDescription.Text = ""
            txtRadiationTotalQuantity.Text = ""

        End If

        'Toxic Industrial Chemical
        If pnlShowToxicIndustrialChemical.Visible = False Then
            txtChemicalName.Text = ""
            txtIndexName.Text = ""
            txtCASNumber.Text = ""
            txtSection304ReportableQuantity.Text = ""
            txtSection304ReportableQuantity.Text = ""
            txtCERCLAReportableQuantity.Text = ""
            ddlChemicalState.SelectedValue = "Select an Option"
            ddlSourceContainer.SelectedValue = "Select an Option"
            txtDiameterPipeline.Text = ""
            txtUnbrokenEndPipeConnectedTo.Text = ""
            txtTotalSourceContainerVolume.Text = ""
            txtChemicalQuantityReleased.Text = ""
            txtChemicalRateOfRelease.Text = ""
            ddlChemicalReleased.SelectedValue = "Select an Option"
            txtCauseOfRelease.Text = ""
            txtTimeReleaseDiscovered.Text = ""
            txtTimeReleaseDiscovered2.Text = ""
            txtTimeReleaseSecured.Text = ""
            txtTimeReleaseSecured2.Text = ""
            txtReasonLateReport.Text = ""
            ddlStormDrainsAffected.SelectedValue = "Select an Option"
            ddlWaterwaysAffected.SelectedValue = "Select an Option"
            ddlCallbackDEPRequested.SelectedValue = "Select an Option"
            ddlCallbackDEPRequestedDDLValue.SelectedValue = "Select an Option"
            txtWaterwaysAffectedText.Text = ""

        Else

            If pnlShowPipeline.Visible = False Then
                txtDiameterPipeline.Text = ""
                txtUnbrokenEndPipeConnectedTo.Text = ""
            End If

            If pnlShowWaterwaysAffectedText.Visible = False Then
                txtWaterwaysAffectedText.Text = ""
            End If

            If pnlShowRegionalAssistanceRequested.Visible = False Then
                ddlCallbackDEPRequestedDDLValue.SelectedValue = "Select an Option"
            End If

            If pnlShowWaterwaysAffectedText.Visible = False Then
                txtWaterwaysAffectedText.Text = ""
            End If

        End If


        'We add these to blank since the panels are not visible
        If pnlShowInjuryText.Visible = False Then
            txtInjury.Text = ""
        End If

        If pnlShowFatalityText.Visible = False Then
            txtFatalityText.Text = ""
        End If

        '

        If pnlShowRegionalAssistanceRequested.Visible = False Then
            ddlCallbackDEPRequested.SelectedValue = "Select an Option"
        End If

        If localHazardousMaterialsCount = 0 Then

            'Try

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionHazardousMaterials", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 0)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CommonName", txtCommonName.Text)
            objCmd.Parameters.AddWithValue("@ScientificName", txtScientificName.Text)
            objCmd.Parameters.AddWithValue("@QuantityDescription", txtQuantityDescription.Text)
            objCmd.Parameters.AddWithValue("@ContainerDeviceDescription", txtContainerDeviceDescription.Text)
            objCmd.Parameters.AddWithValue("@BiologicalTotalQuantity", txtBiologicalTotalQuantity.Text)
            objCmd.Parameters.AddWithValue("@BiologicalQuantityReleased", txtBiologicalQuantityReleased.Text)
            objCmd.Parameters.AddWithValue("@AgentType", ddlAgentType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@AgentName", txtAgentName.Text)
            objCmd.Parameters.AddWithValue("@AgentContainerDeviceDescription", txtAgentContainerDeviceDescription.Text)
            objCmd.Parameters.AddWithValue("@AgentTotalQuantity", txtAgentTotalQuantity.Text)
            objCmd.Parameters.AddWithValue("@AgentQuantityReleased", txtAgentQuantityReleased.Text)
            objCmd.Parameters.AddWithValue("@RadiationType", ddlRadiationType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@IsotopeName", txtIsotopeName.Text)
            objCmd.Parameters.AddWithValue("@ContainerDeviceInstrumentDescription", txtContainerDeviceInstrumentDescription.Text)
            objCmd.Parameters.AddWithValue("@RadiationTotalQuantity", txtRadiationTotalQuantity.Text)
            objCmd.Parameters.AddWithValue("@DOHBureauNotified ", ddlDOHBureauNotified.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@ChemicalName", txtChemicalName.Text)
            objCmd.Parameters.AddWithValue("@IndexName", txtIndexName.Text)
            objCmd.Parameters.AddWithValue("@CASNumber", txtCASNumber.Text)
            objCmd.Parameters.AddWithValue("@Section304ReportableQuantity", txtSection304ReportableQuantity.Text)
            objCmd.Parameters.AddWithValue("@CERCLAReportableQuantity", txtCERCLAReportableQuantity.Text)
            objCmd.Parameters.AddWithValue("@ChemicalState", ddlChemicalState.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@SourceContainer", ddlSourceContainer.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DiameterPipeline", txtDiameterPipeline.Text)
            objCmd.Parameters.AddWithValue("@UnbrokenEndPipeConnectedTo", txtUnbrokenEndPipeConnectedTo.Text)
            objCmd.Parameters.AddWithValue("@TotalSourceContainerVolume", txtTotalSourceContainerVolume.Text)
            objCmd.Parameters.AddWithValue("@ChemicalQuantityReleased", txtChemicalQuantityReleased.Text)
            objCmd.Parameters.AddWithValue("@ChemicalRateOfRelease", txtChemicalRateOfRelease.Text)
            objCmd.Parameters.AddWithValue("@ChemicalReleased", ddlChemicalReleased.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CauseOfRelease", txtCauseOfRelease.Text)
            objCmd.Parameters.AddWithValue("@TimeReleaseDiscovered", CStr(txtTimeReleaseDiscovered.Text.Trim) & CStr(txtTimeReleaseDiscovered2.Text.Trim))
            objCmd.Parameters.AddWithValue("@TimeReleaseSecured", CStr(txtTimeReleaseSecured.Text.Trim) & CStr(txtTimeReleaseSecured2.Text.Trim))
            objCmd.Parameters.AddWithValue("@ReasonLateReport", txtReasonLateReport.Text)
            objCmd.Parameters.AddWithValue("@StormDrainsAffected ", ddlStormDrainsAffected.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WaterwaysAffected ", ddlWaterwaysAffected.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WaterwaysAffectedText", txtWaterwaysAffectedText.Text)
            objCmd.Parameters.AddWithValue("@CallbackDEPRequested ", ddlCallbackDEPRequested.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CallbackDEPRequestedDDLValue", ddlCallbackDEPRequestedDDLValue.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Evacuations", ddlEvacuations.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@MajorRoadwaysClosed", ddlMajorRoadwaysClosed.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Injury", ddlInjury.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@InjuryText", txtInjury.Text)
            objCmd.Parameters.AddWithValue("@Fatality", ddlFatality.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@FatalityText", txtFatalityText.Text)
            objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", ddlNotification.SelectedValue)
            objCmd.Parameters.AddWithValue("@UHChemicalState", ddlUHChemicalState.SelectedValue)
            objCmd.Parameters.AddWithValue("@UHSourceContainer", ddlUHSourceContainer.SelectedValue)
            objCmd.Parameters.AddWithValue("@UHTotalSourceContainerVolume", txtUHTotalSourceContainerVolume.Text)
            objCmd.Parameters.AddWithValue("@UHChemicalQuantityReleased", txtUHChemicalQuantityReleased.Text)
            objCmd.Parameters.AddWithValue("@UHChemicalRateOfRelease", txtUHChemicalRateOfRelease.Text)
            objCmd.Parameters.AddWithValue("@UHChemicalReleased", ddlUHChemicalReleased.SelectedValue)

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
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Added Hazardous Materials Incident Type")

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
            objCmd = New SqlCommand("spActionHazardousMaterials", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 1)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CommonName", txtCommonName.Text)
            objCmd.Parameters.AddWithValue("@ScientificName", txtScientificName.Text)
            objCmd.Parameters.AddWithValue("@QuantityDescription", txtQuantityDescription.Text)
            objCmd.Parameters.AddWithValue("@ContainerDeviceDescription", txtContainerDeviceDescription.Text)
            objCmd.Parameters.AddWithValue("@BiologicalTotalQuantity", txtBiologicalTotalQuantity.Text)
            objCmd.Parameters.AddWithValue("@BiologicalQuantityReleased", txtBiologicalQuantityReleased.Text)
            objCmd.Parameters.AddWithValue("@AgentType", ddlAgentType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@AgentName", txtAgentName.Text)
            objCmd.Parameters.AddWithValue("@AgentContainerDeviceDescription", txtAgentContainerDeviceDescription.Text)
            objCmd.Parameters.AddWithValue("@AgentTotalQuantity", txtAgentTotalQuantity.Text)
            objCmd.Parameters.AddWithValue("@AgentQuantityReleased", txtAgentQuantityReleased.Text)
            objCmd.Parameters.AddWithValue("@RadiationType", ddlRadiationType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@IsotopeName", txtIsotopeName.Text)
            objCmd.Parameters.AddWithValue("@ContainerDeviceInstrumentDescription", txtContainerDeviceInstrumentDescription.Text)
            objCmd.Parameters.AddWithValue("@RadiationTotalQuantity", txtRadiationTotalQuantity.Text)
            objCmd.Parameters.AddWithValue("@DOHBureauNotified ", ddlDOHBureauNotified.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@ChemicalName", txtChemicalName.Text)
            objCmd.Parameters.AddWithValue("@IndexName", txtIndexName.Text)
            objCmd.Parameters.AddWithValue("@CASNumber", txtCASNumber.Text)
            objCmd.Parameters.AddWithValue("@Section304ReportableQuantity", txtSection304ReportableQuantity.Text)
            objCmd.Parameters.AddWithValue("@CERCLAReportableQuantity", txtCERCLAReportableQuantity.Text)
            objCmd.Parameters.AddWithValue("@ChemicalState", ddlChemicalState.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@SourceContainer", ddlSourceContainer.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DiameterPipeline", txtDiameterPipeline.Text)
            objCmd.Parameters.AddWithValue("@UnbrokenEndPipeConnectedTo", txtUnbrokenEndPipeConnectedTo.Text)
            objCmd.Parameters.AddWithValue("@TotalSourceContainerVolume", txtTotalSourceContainerVolume.Text)
            objCmd.Parameters.AddWithValue("@ChemicalQuantityReleased", txtChemicalQuantityReleased.Text)
            objCmd.Parameters.AddWithValue("@ChemicalRateOfRelease", txtChemicalRateOfRelease.Text)
            objCmd.Parameters.AddWithValue("@ChemicalReleased", ddlChemicalReleased.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CauseOfRelease", txtCauseOfRelease.Text)
            objCmd.Parameters.AddWithValue("@TimeReleaseDiscovered", CStr(txtTimeReleaseDiscovered.Text.Trim) & CStr(txtTimeReleaseDiscovered2.Text.Trim))
            objCmd.Parameters.AddWithValue("@TimeReleaseSecured", CStr(txtTimeReleaseSecured.Text.Trim) & CStr(txtTimeReleaseSecured2.Text.Trim))
            objCmd.Parameters.AddWithValue("@ReasonLateReport", txtReasonLateReport.Text)
            objCmd.Parameters.AddWithValue("@StormDrainsAffected ", ddlStormDrainsAffected.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WaterwaysAffected ", ddlWaterwaysAffected.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WaterwaysAffectedText", txtWaterwaysAffectedText.Text)
            objCmd.Parameters.AddWithValue("@CallbackDEPRequested ", ddlCallbackDEPRequested.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CallbackDEPRequestedDDLValue", ddlCallbackDEPRequestedDDLValue.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Evacuations", ddlEvacuations.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@MajorRoadwaysClosed", ddlMajorRoadwaysClosed.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Injury", ddlInjury.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@InjuryText", txtInjury.Text)
            objCmd.Parameters.AddWithValue("@Fatality", ddlFatality.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@FatalityText", txtFatalityText.Text)
            objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", ddlNotification.SelectedValue)
            objCmd.Parameters.AddWithValue("@UHChemicalState", ddlUHChemicalState.SelectedValue)
            objCmd.Parameters.AddWithValue("@UHSourceContainer", ddlUHSourceContainer.SelectedValue)
            objCmd.Parameters.AddWithValue("@UHTotalSourceContainerVolume", txtUHTotalSourceContainerVolume.Text)
            objCmd.Parameters.AddWithValue("@UHChemicalQuantityReleased", txtUHChemicalQuantityReleased.Text)
            objCmd.Parameters.AddWithValue("@UHChemicalRateOfRelease", txtUHChemicalRateOfRelease.Text)
            objCmd.Parameters.AddWithValue("@UHChemicalReleased", ddlUHChemicalReleased.SelectedValue)

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
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Updated Added Hazardous Materials Incident Type")

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



    Protected Sub ddlSourceContainer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSourceContainer.SelectedIndexChanged

        If ddlSourceContainer.SelectedValue = "Aboveground Pipeline" Or ddlSourceContainer.SelectedValue = "Underground Pipeline" Then
            pnlShowPipeline.Visible = True
        Else
            pnlShowPipeline.Visible = False
        End If

    End Sub

    Protected Sub ddlWaterwaysAffected_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWaterwaysAffected.SelectedIndexChanged

        If ddlWaterwaysAffected.SelectedValue = "Yes" Then
            pnlShowWaterwaysAffectedText.Visible = True
        Else
            pnlShowWaterwaysAffectedText.Visible = False
        End If

    End Sub

    Protected Sub ddlCallbackDEPRequested_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCallbackDEPRequested.SelectedIndexChanged

        If ddlCallbackDEPRequested.SelectedValue = "Yes" Then
            pnlShowRegionalAssistanceRequested.Visible = True
        Else
            pnlShowRegionalAssistanceRequested.Visible = False
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



        'Finish the Error String.
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels.
        lblMessage.Text = strError.ToString
    End Sub
End Class
