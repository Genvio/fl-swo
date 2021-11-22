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

Partial Class PublicHealthMedical
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

            Dim localPublicHealthMedicalCount As Integer = 0

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectPublicHealthMedicalCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                localPublicHealthMedicalCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            If localPublicHealthMedicalCount > 0 Then
                PopulatePage()
            End If

        End If

    End Sub

    'PagePopulation
    Protected Sub PopulatePage()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectPublicHealthMedicalByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            ddlSubType.SelectedValue = HelpFunction.Convertdbnulls(objDR("SubType"))
            ddlSituation.SelectedValue = HelpFunction.Convertdbnulls(objDR("Situation"))
            txtIDRdiseaseType.Text = HelpFunction.Convertdbnulls(objDR("IDRdiseaseType"))
            txtIDRpeopleInfectedNumber.Text = HelpFunction.Convertdbnulls(objDR("IDRpeopleInfectedNumber"))
            txtIDRexamTest.Text = HelpFunction.Convertdbnulls(objDR("IDRexamTest"))
            ddlIDRquarantineEffect.SelectedValue = HelpFunction.Convertdbnulls(objDR("IDRquarantineEffect"))
            txtIDRquarantineEffectText.Text = HelpFunction.Convertdbnulls(objDR("IDRquarantineEffectText"))
            ddlIDRfatality.SelectedValue = HelpFunction.Convertdbnulls(objDR("IDRfatality"))
            txtIDRfatalityText.Text = HelpFunction.Convertdbnulls(objDR("IDRfatalityText"))
            ddlIDRdOHrequested.SelectedValue = HelpFunction.Convertdbnulls(objDR("IDRdOHrequested"))
            txtIDRdOHrequestedText.Text = HelpFunction.Convertdbnulls(objDR("IDRdOHrequestedText"))
            txtPHHOhazardDescription.Text = HelpFunction.Convertdbnulls(objDR("PHHOhazardDescription"))
            ddlPHHOdOHRequested.SelectedValue = HelpFunction.Convertdbnulls(objDR("PHHOdOHRequested"))
            txtPHHOdOHRequestedText.Text = HelpFunction.Convertdbnulls(objDR("PHHOdOHRequestedText"))
            txtMCIpatientNumber.Text = HelpFunction.Convertdbnulls(objDR("MCIpatientNumber"))
            txtMCIcritical.Text = HelpFunction.Convertdbnulls(objDR("MCIcritical"))
            txtMCIimmediate.Text = HelpFunction.Convertdbnulls(objDR("MCIimmediate"))
            txtMCIdelayed.Text = HelpFunction.Convertdbnulls(objDR("MCIdelayed"))
            txtMCIdeceased.Text = HelpFunction.Convertdbnulls(objDR("MCIdeceased"))
            txtMCItTA.Text = HelpFunction.Convertdbnulls(objDR("MCItTA"))
            txtMCIagencyCoordinating.Text = HelpFunction.Convertdbnulls(objDR("MCIagencyCoordinating"))
            ddlMCIunmetNeeds.SelectedValue = HelpFunction.Convertdbnulls(objDR("MCIunmetNeeds"))
            txtMCIunmetNeedsText.Text = HelpFunction.Convertdbnulls(objDR("MCIunmetNeedsText"))
            ddlMCIdOHRequested.SelectedValue = HelpFunction.Convertdbnulls(objDR("MCIdOHRequested"))
            txtMCIdOHRequestedText.Text = HelpFunction.Convertdbnulls(objDR("MCIdOHRequestedText"))
            txtIHFpatientsAffectedNumber.Text = HelpFunction.Convertdbnulls(objDR("IHFpatientsAffectedNumber"))
            ddlIHFfacilityDamaged.SelectedValue = HelpFunction.Convertdbnulls(objDR("IHFfacilityDamaged"))
            txtIHFfacilityDamagedText.Text = HelpFunction.Convertdbnulls(objDR("IHFfacilityDamagedText"))
            ddlIHFfacilityEvacuated.SelectedValue = HelpFunction.Convertdbnulls(objDR("IHFfacilityEvacuated"))
            txtIHFfacilityEvacuatedText.Text = HelpFunction.Convertdbnulls(objDR("IHFfacilityEvacuatedText"))
            ddlIHFunmetNeeds.SelectedValue = HelpFunction.Convertdbnulls(objDR("IHFunmetNeeds"))
            txtIHFunmetNeedsText.Text = HelpFunction.Convertdbnulls(objDR("IHFunmetNeedsText"))
            ddlIHFcallbackRequested.SelectedValue = HelpFunction.Convertdbnulls(objDR("IHFcallbackRequested"))
            txtIHFcallbackRequestedText.Text = HelpFunction.Convertdbnulls(objDR("IHFcallbackRequestedText"))
            ddlNotification.SelectedValue = HelpFunction.Convertdbnulls(objDR("IncidentTypeLevelID"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        txtWorkSheetDescription.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))


        If ddlSubType.SelectedValue = "Infectious Disease Report" Then
            pnlShowInfectiousDiseaseReport.Visible = True
        Else
            pnlShowInfectiousDiseaseReport.Visible = False
        End If

        If ddlSubType.SelectedValue = "Public Health Hazard" Or ddlSubType.SelectedValue = "Other" Then
            pnlShowPublicHealthHazardOther.Visible = True
        Else
            pnlShowPublicHealthHazardOther.Visible = False
        End If

        If ddlSubType.SelectedValue = "Mass Casualty Incident" Then
            pnlShowMassCasualtyIncident.Visible = True
        Else
            pnlShowMassCasualtyIncident.Visible = False
        End If

        If ddlSubType.SelectedValue = "Impact to Healthcare Facility" Then
            pnlShowImpactHealthcareFacility.Visible = True
        Else
            pnlShowImpactHealthcareFacility.Visible = False
        End If


        If ddlIDRquarantineEffect.SelectedValue = "Yes" Then
            pnlShowIDRquarantineEffectText.Visible = True
        Else
            pnlShowIDRquarantineEffectText.Visible = False
        End If

        If ddlIDRfatality.SelectedValue = "Yes" Then
            'pnlShowIDRfatalityText.Visible = True
        Else
            pnlShowIDRfatalityText.Visible = False
        End If

        If ddlIDRdOHrequested.SelectedValue = "Yes" Then
            pnlShowIDRdOHrequestedText.Visible = True
        Else
            pnlShowIDRdOHrequestedText.Visible = False
        End If

        If ddlPHHOdOHRequested.SelectedValue = "Yes" Then
            pnlShowPHHOdOHRequestedText.Visible = True
        Else
            pnlShowPHHOdOHRequestedText.Visible = False
        End If

        If ddlMCIunmetNeeds.SelectedValue = "Yes" Then
            pnlShowMCIunmetNeedsText.Visible = True
        Else
            pnlShowMCIunmetNeedsText.Visible = False
        End If

        If ddlMCIdOHRequested.SelectedValue = "Yes" Then
            pnlShowMCIdOHRequestedText.Visible = True
        Else
            pnlShowMCIdOHRequestedText.Visible = False
        End If

        If ddlIHFfacilityDamaged.SelectedValue = "Yes" Then
            pnlShowIHFfacilityDamagedText.Visible = True
        Else
            pnlShowIHFfacilityDamagedText.Visible = False
        End If

        If ddlIHFfacilityEvacuated.SelectedValue = "Yes" Then
            pnlShowIHFfacilityEvacuatedText.Visible = True
        Else
            pnlShowIHFfacilityEvacuatedText.Visible = False
        End If

        If ddlIHFunmetNeeds.SelectedValue = "Yes" Then
            pnlShowIHFunmetNeedsText.Visible = True
        Else
            pnlShowIHFunmetNeedsText.Visible = False
        End If

        If ddlIHFcallbackRequested.SelectedValue = "Yes" Then
            pnlShowIHFcallbackRequestedText.Visible = True
        Else
            pnlShowIHFcallbackRequestedText.Visible = False
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

        'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)

    End Sub

    Protected Sub Save()


        Dim localPublicHealthMedicalCount As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectPublicHealthMedicalCountByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            localPublicHealthMedicalCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()


        If pnlShowInfectiousDiseaseReport.Visible = False Then
            txtIDRdiseaseType.Text = ""
            txtIDRpeopleInfectedNumber.Text = ""
            txtIDRexamTest.Text = ""
            ddlIDRquarantineEffect.SelectedValue = "Select an Option"
            txtIDRquarantineEffectText.Text = ""
            ddlIDRfatality.SelectedValue = "Select an Option"
            txtIDRfatalityText.Text = ""
            ddlIDRdOHrequested.SelectedValue = "Select an Option"
            txtIDRdOHrequestedText.Text = ""
        End If

        If pnlShowPublicHealthHazardOther.Visible = False Then
            txtPHHOhazardDescription.Text = ""
            ddlPHHOdOHRequested.SelectedValue = "Select an Option"
            txtPHHOdOHRequestedText.Text = ""
        End If

        If pnlShowMassCasualtyIncident.Visible = False Then
            txtMCIpatientNumber.Text = ""
            txtMCIcritical.Text = ""
            txtMCIimmediate.Text = ""
            txtMCIdelayed.Text = ""
            txtMCIdeceased.Text = ""
            txtMCItTA.Text = ""
            txtMCIagencyCoordinating.Text = ""
            ddlMCIunmetNeeds.SelectedValue = "Select an Option"
            txtMCIunmetNeedsText.Text = ""
            ddlMCIdOHRequested.SelectedValue = "Select an Option"
            txtMCIdOHRequestedText.Text = ""
        End If

        If pnlShowImpactHealthcareFacility.Visible = False Then
            txtIHFpatientsAffectedNumber.Text = ""
            ddlIHFfacilityDamaged.SelectedValue = "Select an Option"
            txtIHFfacilityDamagedText.Text = ""
            ddlIHFfacilityEvacuated.SelectedValue = "Select an Option"
            txtIHFfacilityEvacuatedText.Text = ""
            ddlIHFunmetNeeds.SelectedValue = "Select an Option"
            txtIHFunmetNeedsText.Text = ""
            ddlIHFcallbackRequested.SelectedValue = "Select an Option"
            txtIHFcallbackRequestedText.Text = ""
        End If


        'We add these to blank since the panels are not visible
        If pnlShowIDRquarantineEffectText.Visible = False Then
            txtIDRquarantineEffectText.Text = ""
        End If

        If pnlShowIDRfatalityText.Visible = False Then
            txtIDRfatalityText.Text = ""
        End If

        If pnlShowIDRdOHrequestedText.Visible = False Then
            txtIDRdOHrequestedText.Text = ""
        End If

        If pnlShowPHHOdOHRequestedText.Visible = False Then
            txtPHHOdOHRequestedText.Text = ""
        End If


        If pnlShowMCIunmetNeedsText.Visible = False Then
            txtMCIunmetNeedsText.Text = ""
        End If

        If pnlShowMCIdOHRequestedText.Visible = False Then
            txtMCIdOHRequestedText.Text = ""
        End If

        If pnlShowIHFfacilityDamagedText.Visible = False Then
            txtIHFfacilityDamagedText.Text = ""
        End If

        If pnlShowIHFfacilityEvacuatedText.Visible = False Then
            txtIHFfacilityEvacuatedText.Text = ""
        End If

        If pnlShowIHFunmetNeedsText.Visible = False Then
            txtIHFunmetNeedsText.Text = ""
        End If

        If pnlShowIHFcallbackRequestedText.Visible = False Then
            txtIHFcallbackRequestedText.Text = ""
        End If


        If localPublicHealthMedicalCount = 0 Then

            Try

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                '// Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionPublicHealthMedical", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
                objCmd.Parameters.AddWithValue("@Flag", 0)
                objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@IDRdiseaseType", txtIDRdiseaseType.Text)
                objCmd.Parameters.AddWithValue("@IDRpeopleInfectedNumber", txtIDRpeopleInfectedNumber.Text)
                objCmd.Parameters.AddWithValue("@IDRexamTest", txtIDRexamTest.Text)
                objCmd.Parameters.AddWithValue("@IDRquarantineEffect", ddlIDRquarantineEffect.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@IDRquarantineEffectText", txtIDRquarantineEffectText.Text)
                objCmd.Parameters.AddWithValue("@IDRfatality", ddlIDRfatality.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@IDRfatalityText", txtIDRfatalityText.Text)
                objCmd.Parameters.AddWithValue("@IDRdOHrequested", ddlIDRdOHrequested.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@IDRdOHrequestedText", txtIDRdOHrequestedText.Text)
                objCmd.Parameters.AddWithValue("@PHHOhazardDescription", txtPHHOhazardDescription.Text)
                objCmd.Parameters.AddWithValue("@PHHOdOHRequested", ddlPHHOdOHRequested.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@PHHOdOHRequestedText", txtPHHOdOHRequestedText.Text)
                objCmd.Parameters.AddWithValue("@MCIpatientNumber", txtMCIpatientNumber.Text)
                objCmd.Parameters.AddWithValue("@MCIcritical", txtMCIcritical.Text)
                objCmd.Parameters.AddWithValue("@MCIimmediate", txtMCIimmediate.Text)
                objCmd.Parameters.AddWithValue("@MCIdelayed", txtMCIdelayed.Text)
                objCmd.Parameters.AddWithValue("@MCIdeceased", txtMCIdeceased.Text)
                objCmd.Parameters.AddWithValue("@MCItTA", txtMCItTA.Text)
                objCmd.Parameters.AddWithValue("@MCIagencyCoordinating", txtMCIagencyCoordinating.Text)
                objCmd.Parameters.AddWithValue("@MCIunmetNeeds", ddlMCIunmetNeeds.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@MCIunmetNeedsText", txtMCIunmetNeedsText.Text)
                objCmd.Parameters.AddWithValue("@MCIdOHRequested", ddlMCIdOHRequested.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@MCIdOHRequestedText", txtMCIdOHRequestedText.Text)
                objCmd.Parameters.AddWithValue("@IHFpatientsAffectedNumber", txtIHFpatientsAffectedNumber.Text)
                objCmd.Parameters.AddWithValue("@IHFfacilityDamaged", ddlIHFfacilityDamaged.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@IHFfacilityDamagedText", txtIHFfacilityDamagedText.Text)
                objCmd.Parameters.AddWithValue("@IHFfacilityEvacuated", ddlIHFfacilityEvacuated.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@IHFfacilityEvacuatedText", txtIHFfacilityEvacuatedText.Text)
                objCmd.Parameters.AddWithValue("@IHFunmetNeeds", ddlIHFunmetNeeds.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@IHFunmetNeedsText", txtIHFunmetNeedsText.Text)
                objCmd.Parameters.AddWithValue("@IHFcallbackRequested", ddlIHFcallbackRequested.SelectedValue.ToString)
                objCmd.Parameters.AddWithValue("@IHFcallbackRequestedText", txtIHFcallbackRequestedText.Text)
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
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Added Public Health Medical")

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
            objCmd = New SqlCommand("spActionPublicHealthMedical", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 1)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@IDRdiseaseType", txtIDRdiseaseType.Text)
            objCmd.Parameters.AddWithValue("@IDRpeopleInfectedNumber", txtIDRpeopleInfectedNumber.Text)
            objCmd.Parameters.AddWithValue("@IDRexamTest", txtIDRexamTest.Text)
            objCmd.Parameters.AddWithValue("@IDRquarantineEffect", ddlIDRquarantineEffect.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@IDRquarantineEffectText", txtIDRquarantineEffectText.Text)
            objCmd.Parameters.AddWithValue("@IDRfatality", ddlIDRfatality.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@IDRfatalityText", txtIDRfatalityText.Text)
            objCmd.Parameters.AddWithValue("@IDRdOHrequested", ddlIDRdOHrequested.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@IDRdOHrequestedText", txtIDRdOHrequestedText.Text)
            objCmd.Parameters.AddWithValue("@PHHOhazardDescription", txtPHHOhazardDescription.Text)
            objCmd.Parameters.AddWithValue("@PHHOdOHRequested", ddlPHHOdOHRequested.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@PHHOdOHRequestedText", txtPHHOdOHRequestedText.Text)
            objCmd.Parameters.AddWithValue("@MCIpatientNumber", txtMCIpatientNumber.Text)
            objCmd.Parameters.AddWithValue("@MCIcritical", txtMCIcritical.Text)
            objCmd.Parameters.AddWithValue("@MCIimmediate", txtMCIimmediate.Text)
            objCmd.Parameters.AddWithValue("@MCIdelayed", txtMCIdelayed.Text)
            objCmd.Parameters.AddWithValue("@MCIdeceased", txtMCIdeceased.Text)
            objCmd.Parameters.AddWithValue("@MCItTA", txtMCItTA.Text)
            objCmd.Parameters.AddWithValue("@MCIagencyCoordinating", txtMCIagencyCoordinating.Text)
            objCmd.Parameters.AddWithValue("@MCIunmetNeeds", ddlMCIunmetNeeds.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@MCIunmetNeedsText", txtMCIunmetNeedsText.Text)
            objCmd.Parameters.AddWithValue("@MCIdOHRequested", ddlMCIdOHRequested.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@MCIdOHRequestedText", txtMCIdOHRequestedText.Text)
            objCmd.Parameters.AddWithValue("@IHFpatientsAffectedNumber", txtIHFpatientsAffectedNumber.Text)
            objCmd.Parameters.AddWithValue("@IHFfacilityDamaged", ddlIHFfacilityDamaged.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@IHFfacilityDamagedText", txtIHFfacilityDamagedText.Text)
            objCmd.Parameters.AddWithValue("@IHFfacilityEvacuated", ddlIHFfacilityEvacuated.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@IHFfacilityEvacuatedText", txtIHFfacilityEvacuatedText.Text)
            objCmd.Parameters.AddWithValue("@IHFunmetNeeds", ddlIHFunmetNeeds.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@IHFunmetNeedsText", txtIHFunmetNeedsText.Text)
            objCmd.Parameters.AddWithValue("@IHFcallbackRequested", ddlIHFcallbackRequested.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@IHFcallbackRequestedText", txtIHFcallbackRequestedText.Text)
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
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Updated Public Health Medical")

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


    'DDLs Index Change
    Protected Sub ddlSubType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubType.SelectedIndexChanged

        If ddlSubType.SelectedValue = "Infectious Disease Report" Then
            pnlShowInfectiousDiseaseReport.Visible = True
        Else
            pnlShowInfectiousDiseaseReport.Visible = False
        End If

        If ddlSubType.SelectedValue = "Public Health Hazard" Or ddlSubType.SelectedValue = "Other" Then
            pnlShowPublicHealthHazardOther.Visible = True
        Else
            pnlShowPublicHealthHazardOther.Visible = False
        End If

        If ddlSubType.SelectedValue = "Mass Casualty Incident" Then
            pnlShowMassCasualtyIncident.Visible = True
        Else
            pnlShowMassCasualtyIncident.Visible = False
        End If

        If ddlSubType.SelectedValue = "Impact to Healthcare Facility" Then
            pnlShowImpactHealthcareFacility.Visible = True
        Else
            pnlShowImpactHealthcareFacility.Visible = False
        End If

    End Sub

    Protected Sub ddlIDRquarantineEffect_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIDRquarantineEffect.SelectedIndexChanged

        If ddlIDRquarantineEffect.SelectedValue = "Yes" Then
            pnlShowIDRquarantineEffectText.Visible = True
        Else
            pnlShowIDRquarantineEffectText.Visible = False
        End If

    End Sub

    Protected Sub ddlIDRfatality_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIDRfatality.SelectedIndexChanged

        If ddlIDRfatality.SelectedValue = "Yes" Then
            pnlShowIDRfatalityText.Visible = True
        Else
            pnlShowIDRfatalityText.Visible = False
        End If

    End Sub

    Protected Sub ddlIDRdOHrequested_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIDRdOHrequested.SelectedIndexChanged

        If ddlIDRdOHrequested.SelectedValue = "Yes" Then
            pnlShowIDRdOHrequestedText.Visible = True
        Else
            pnlShowIDRdOHrequestedText.Visible = False
        End If

    End Sub

    Protected Sub ddlPHHOdOHRequested_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPHHOdOHRequested.SelectedIndexChanged

        If ddlPHHOdOHRequested.SelectedValue = "Yes" Then
            pnlShowPHHOdOHRequestedText.Visible = True
        Else
            pnlShowPHHOdOHRequestedText.Visible = False
        End If

    End Sub

    Protected Sub ddlMCIunmetNeeds_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMCIunmetNeeds.SelectedIndexChanged

        If ddlMCIunmetNeeds.SelectedValue = "Yes" Then
            pnlShowMCIunmetNeedsText.Visible = True
        Else
            pnlShowMCIunmetNeedsText.Visible = False
        End If

    End Sub

    Protected Sub ddlMCIdOHRequested_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMCIdOHRequested.SelectedIndexChanged

        If ddlMCIdOHRequested.SelectedValue = "Yes" Then
            pnlShowMCIdOHRequestedText.Visible = True
        Else
            pnlShowMCIdOHRequestedText.Visible = False
        End If

    End Sub

    Protected Sub ddlIHFfacilityDamaged_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIHFfacilityDamaged.SelectedIndexChanged

        If ddlIHFfacilityDamaged.SelectedValue = "Yes" Then
            pnlShowIHFfacilityDamagedText.Visible = True
        Else
            pnlShowIHFfacilityDamagedText.Visible = False
        End If

    End Sub

    Protected Sub ddlIHFfacilityEvacuated_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIHFfacilityEvacuated.SelectedIndexChanged

        If ddlIHFfacilityEvacuated.SelectedValue = "Yes" Then
            pnlShowIHFfacilityEvacuatedText.Visible = True
        Else
            pnlShowIHFfacilityEvacuatedText.Visible = False
        End If

    End Sub

    Protected Sub ddlIHFunmetNeeds_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIHFunmetNeeds.SelectedIndexChanged

        If ddlIHFunmetNeeds.SelectedValue = "Yes" Then
            pnlShowIHFunmetNeedsText.Visible = True
        Else
            pnlShowIHFunmetNeedsText.Visible = False
        End If

    End Sub

    Protected Sub ddlIHFcallbackRequested_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIHFcallbackRequested.SelectedIndexChanged

        If ddlIHFcallbackRequested.SelectedValue = "Yes" Then
            pnlShowIHFcallbackRequestedText.Visible = True
        Else
            pnlShowIHFcallbackRequestedText.Visible = False
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
