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
Partial Class SearchRescue
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

        'Response.End()

        'If Request("IncidentID") = "" Then

        '    Response.Redirect("Incident.aspx")

        'End If

        If Page.IsPostBack = False Then

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

            Dim localSearchRescueCount As Integer = 0

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectSearchRescueCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                localSearchRescueCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()



            If localSearchRescueCount > 0 Then
                PopulatePage()
            End If

        End If

        ShowHiddenTables()

    End Sub

    'PagePopulation
    Protected Sub PopulatePage()

        'Response.Write("Hello")
        'Response.End()


        Dim localTime As String = ""
        Dim localTime2 As String = ""

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectSearchRescueByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            ddlSubType.SelectedValue = HelpFunction.Convertdbnulls(objDR("SubType"))
            ddlSituation.SelectedValue = HelpFunction.Convertdbnulls(objDR("Situation"))
            txtSearchRescueDate.Text = HelpFunction.Convertdbnulls(objDR("SearchRescueDate"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR("SearchRescueTime")))
            txtMissionNumber.Text = HelpFunction.Convertdbnulls(objDR("MissionNumber"))
            txtCoordinateAreaDescription.Text = HelpFunction.Convertdbnulls(objDR("CoordinateAreaDescription"))
            txtRegistrationInformation.Text = HelpFunction.Convertdbnulls(objDR("RegistrationInformation"))
            ddlCAPResponding.SelectedValue = HelpFunction.Convertdbnulls(objDR("CAPResponding"))
            ddlMissingOverdueAircraft.SelectedValue = HelpFunction.Convertdbnulls(objDR("MissingOverdueAircraft"))
            txtMissionClosedDate.Text = HelpFunction.Convertdbnulls(objDR("MissionClosedDate"))
            localTime2 = CStr(HelpFunction.Convertdbnulls(objDR("MissionClosedTime")))
            txtDisposition.Text = HelpFunction.Convertdbnulls(objDR("Disposition"))
            txtAffectedStrutureFacility.Text = HelpFunction.Convertdbnulls(objDR("AffectedStrutureFacility"))
            txtCausedCollapse.Text = HelpFunction.Convertdbnulls(objDR("CausedCollapse"))
            txtNumberPeopleTrapped.Text = HelpFunction.Convertdbnulls(objDR("NumberPeopleTrapped"))
            ddlInjury.SelectedValue = HelpFunction.Convertdbnulls(objDR("Injury"))
            txtInjury.Text = HelpFunction.Convertdbnulls(objDR("InjuryText"))
            ddlFatality.SelectedValue = HelpFunction.Convertdbnulls(objDR("Fatality"))
            txtFatalityText.Text = HelpFunction.Convertdbnulls(objDR("FatalityText"))
            ddlUnmetNeeds.SelectedValue = HelpFunction.Convertdbnulls(objDR("UnmetNeeds"))
            txtUnmetNeedsText.Text = HelpFunction.Convertdbnulls(objDR("UnmetNeedsText"))
            txtCoordinatingRescueEffort.Text = HelpFunction.Convertdbnulls(objDR("CoordinatingRescueEffort"))
            txtDescriptionIndividual.Text = HelpFunction.Convertdbnulls(objDR("DescriptionIndividual"))
            txtLastSeen.Text = HelpFunction.Convertdbnulls(objDR("LastSeen"))
            txtDescriptionVehicleRelevantInformation.Text = HelpFunction.Convertdbnulls(objDR("DescriptionVehicleRelevantInformation"))
            txtAgencyHandlingInvestigation.Text = HelpFunction.Convertdbnulls(objDR("AgencyHandlingInvestigation"))
            ddlNotification.SelectedValue = HelpFunction.Convertdbnulls(objDR("IncidentTypeLevelID"))
            ddlIsCollapse.SelectedValue = HelpFunction.Convertdbnulls(objDR("IsCollapse"))
            ddlPeopletrapped.SelectedValue = HelpFunction.Convertdbnulls(objDR("PeopleTrapped"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        txtWorkSheetDescription.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        txtSearchRescueTime.Text = Left(localTime, 2)
        txtSearchRescueTime2.Text = Right(localTime, 2)


        'If txtSearchRescueTime.Text = "0" Then
        '    txtSearchRescueTime.Text = ""
        'End If

        'If txtSearchRescueTime2.Text = "0" Then
        '    txtSearchRescueTime2.Text = ""
        'End If


        txtMissionClosedTime.Text = Left(localTime2, 2)
        txtMissionClosedTime2.Text = Right(localTime2, 2)


        'If txtMissionClosedTime.Text = "0" Then
        '    txtMissionClosedTime.Text = ""
        'End If

        'If txtMissionClosedTime2.Text = "0" Then
        '    txtMissionClosedTime2.Text = ""
        'End If

        If txtSearchRescueDate.Text = "1/1/1900" Then
            txtSearchRescueDate.Text = ""
        End If

        If txtMissionClosedDate.Text = "1/1/1900" Then
            txtMissionClosedDate.Text = ""
        End If



        If ddlInjury.SelectedValue = "Yes" Then
            'pnlShowInjuryText.Visible = True
        End If

        If ddlFatality.SelectedValue = "Yes" Then
            'pnlShowFatalityText.Visible = True
        End If

        If ddlUnmetNeeds.SelectedValue = "Yes" Then
            pnlShowUnmetNeeds.Visible = True
        End If

        If ddlSubType.SelectedValue = "ELT" Or ddlSubType.SelectedValue = "EPIRB" Or ddlSubType.SelectedValue = "PLB" Then
            pnlShowEltEpirbPlb.Visible = True
            tblCAPResponding.Visible = ddlSubType.SelectedValue.Equals("ELT")
            tblAircraftOverdue.Visible = ddlSubType.SelectedValue.Equals("ELT")
        Else
            pnlShowEltEpirbPlb.Visible = False
        End If

        '==================================================
        If ddlSubType.SelectedValue = "Structure Collapse" Or ddlSubType.SelectedValue = "Industrial Accident" Or ddlSubType.SelectedValue = "Transportation Accident" Or ddlSubType.SelectedValue = "Other" Then
            pnlStructCollapseIndusAccTransAccOther.Visible = True
        Else
            pnlStructCollapseIndusAccTransAccOther.Visible = False
        End If

        '==================================================
        If ddlSubType.SelectedValue = "LE Search (Missing Person)" Then
            pnlShowLESearch.Visible = True
        Else
            pnlShowLESearch.Visible = False
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

    Protected Sub Save()

        Dim localSearchRescueCount As Integer = 0
        Dim strCAPResponding As String = ""
        Dim strMissingOverdueAircraft As String = ""
        Dim strCausedCollapse As String = ""
        Dim strNumberPeopleTrapped As String = ""

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectSearchRescueCountByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            localSearchRescueCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()


        If pnlShowEltEpirbPlb.Visible = False Then

            txtSearchRescueDate.Text = ""
            txtSearchRescueTime.Text = ""
            txtSearchRescueTime2.Text = ""
            txtMissionNumber.Text = ""
            txtCoordinateAreaDescription.Text = ""
            txtRegistrationInformation.Text = ""
            ddlCAPResponding.SelectedValue = "Select an Option"
            ddlMissingOverdueAircraft.Text = "Select an Option"
            txtMissionClosedDate.Text = ""
            txtMissionClosedTime.Text = ""
            txtMissionClosedTime2.Text = ""
            txtDisposition.Text = ""

        End If

        '==================================================
        If pnlStructCollapseIndusAccTransAccOther.Visible = False Then

            txtAffectedStrutureFacility.Text = ""
            txtCausedCollapse.Text = ""
            txtNumberPeopleTrapped.Text = ""
            ddlInjury.SelectedValue = "Select an Option"
            ddlFatality.SelectedValue = "Select an Option"
            ddlUnmetNeeds.SelectedValue = "Select an Option"
            txtCoordinatingRescueEffort.Text = ""

        End If

        '==================================================
        If pnlShowLESearch.Visible = False Then

            txtDescriptionIndividual.Text = ""
            txtLastSeen.Text = ""
            txtDescriptionVehicleRelevantInformation.Text = ""
            txtAgencyHandlingInvestigation.Text = ""

        End If

        'We add these to blank since the panels are not visible
        If pnlShowInjuryText.Visible = False Then
            txtInjury.Text = ""
        End If

        If pnlShowFatalityText.Visible = False Then
            txtFatalityText.Text = ""
        End If

        If pnlShowUnmetNeeds.Visible = False Then
            txtUnmetNeedsText.Text = ""
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

        If ddlSubType.SelectedValue = "EPIRB" Or ddlSubType.SelectedValue = "PLB" Then
            strCAPResponding = "N/A"
            strMissingOverdueAircraft = "N/A"
        Else
            strCAPResponding = ddlCAPResponding.SelectedValue.ToString
            strMissingOverdueAircraft = ddlMissingOverdueAircraft.SelectedValue.ToString
        End If

        If tblCollapseCause.Visible Then
            strCausedCollapse = txtCausedCollapse.Text
        Else
            strCausedCollapse = "N/A"
        End If

        If tblNumberPeopleTrapped.Visible Then
            strNumberPeopleTrapped = txtNumberPeopleTrapped.Text
        Else
            strNumberPeopleTrapped = "N/A"
        End If

        If localSearchRescueCount = 0 Then

            'Try

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionSearchRescue", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 0)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@SearchRescueDate", txtSearchRescueDate.Text)
            objCmd.Parameters.AddWithValue("@SearchRescueTime", CStr(txtSearchRescueTime.Text.Trim) & CStr(txtSearchRescueTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@MissionNumber", txtMissionNumber.Text)
            objCmd.Parameters.AddWithValue("@CoordinateAreaDescription", txtCoordinateAreaDescription.Text)
            objCmd.Parameters.AddWithValue("@RegistrationInformation", txtRegistrationInformation.Text)
            objCmd.Parameters.AddWithValue("@CAPResponding", strCAPResponding)
            objCmd.Parameters.AddWithValue("@MissingOverdueAircraft", strMissingOverdueAircraft)
            objCmd.Parameters.AddWithValue("@MissionClosedDate", txtMissionClosedDate.Text)
            objCmd.Parameters.AddWithValue("@MissionClosedTime", CStr(txtMissionClosedTime.Text.Trim) & CStr(txtMissionClosedTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@Disposition", txtDisposition.Text)
            objCmd.Parameters.AddWithValue("@AffectedStrutureFacility", txtAffectedStrutureFacility.Text)
            objCmd.Parameters.AddWithValue("@CausedCollapse", strCausedCollapse)
            objCmd.Parameters.AddWithValue("@NumberPeopleTrapped", strNumberPeopleTrapped)
            objCmd.Parameters.AddWithValue("@Injury", ddlInjury.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@InjuryText", txtInjury.Text)
            objCmd.Parameters.AddWithValue("@Fatality", ddlFatality.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@FatalityText", txtFatalityText.Text)
            objCmd.Parameters.AddWithValue("@UnmetNeeds", ddlUnmetNeeds.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@UnmetNeedsText", txtUnmetNeedsText.Text)
            objCmd.Parameters.AddWithValue("@CoordinatingRescueEffort", txtCoordinatingRescueEffort.Text)
            objCmd.Parameters.AddWithValue("@DescriptionIndividual", txtDescriptionIndividual.Text)
            objCmd.Parameters.AddWithValue("@LastSeen", txtLastSeen.Text)
            objCmd.Parameters.AddWithValue("@DescriptionVehicleRelevantInformation", txtDescriptionVehicleRelevantInformation.Text)
            objCmd.Parameters.AddWithValue("@AgencyHandlingInvestigation", txtAgencyHandlingInvestigation.Text)
            objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", ddlNotification.SelectedValue)
            objCmd.Parameters.AddWithValue("@IsCollapse", ddlIsCollapse.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@PeopleTrapped", ddlPeopletrapped.SelectedValue.ToString)

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
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Added Search Rescue")

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
            objCmd = New SqlCommand("spActionSearchRescue", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 1)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@SearchRescueDate", txtSearchRescueDate.Text)
            objCmd.Parameters.AddWithValue("@SearchRescueTime", CStr(txtSearchRescueTime.Text.Trim) & CStr(txtSearchRescueTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@MissionNumber", txtMissionNumber.Text)
            objCmd.Parameters.AddWithValue("@CoordinateAreaDescription", txtCoordinateAreaDescription.Text)
            objCmd.Parameters.AddWithValue("@RegistrationInformation", txtRegistrationInformation.Text)
            objCmd.Parameters.AddWithValue("@CAPResponding", strCAPResponding)
            objCmd.Parameters.AddWithValue("@MissingOverdueAircraft", strMissingOverdueAircraft)
            objCmd.Parameters.AddWithValue("@MissionClosedDate", txtMissionClosedDate.Text)
            objCmd.Parameters.AddWithValue("@MissionClosedTime", CStr(txtMissionClosedTime.Text.Trim) & CStr(txtMissionClosedTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@Disposition", txtDisposition.Text)
            objCmd.Parameters.AddWithValue("@AffectedStrutureFacility", txtAffectedStrutureFacility.Text)
            objCmd.Parameters.AddWithValue("@CausedCollapse", strCausedCollapse)
            objCmd.Parameters.AddWithValue("@NumberPeopleTrapped", txtNumberPeopleTrapped.Text)
            objCmd.Parameters.AddWithValue("@Injury", ddlInjury.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@InjuryText", txtInjury.Text)
            objCmd.Parameters.AddWithValue("@Fatality", ddlFatality.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@FatalityText", txtFatalityText.Text)
            objCmd.Parameters.AddWithValue("@UnmetNeeds", ddlUnmetNeeds.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@UnmetNeedsText", txtUnmetNeedsText.Text)
            objCmd.Parameters.AddWithValue("@CoordinatingRescueEffort", txtCoordinatingRescueEffort.Text)
            objCmd.Parameters.AddWithValue("@DescriptionIndividual", txtDescriptionIndividual.Text)
            objCmd.Parameters.AddWithValue("@LastSeen", txtLastSeen.Text)
            objCmd.Parameters.AddWithValue("@DescriptionVehicleRelevantInformation", txtDescriptionVehicleRelevantInformation.Text)
            objCmd.Parameters.AddWithValue("@AgencyHandlingInvestigation", txtAgencyHandlingInvestigation.Text)
            objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", ddlNotification.SelectedValue)
            objCmd.Parameters.AddWithValue("@IsCollapse", ddlIsCollapse.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@PeopleTrapped", ddlPeopletrapped.SelectedValue.ToString)


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
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Updated Search Rescue")

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


    Protected Sub ddlSubType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubType.SelectedIndexChanged

        If ddlSubType.SelectedValue = "ELT" Or ddlSubType.SelectedValue = "EPIRB" Or ddlSubType.SelectedValue = "PLB" Then
            pnlShowEltEpirbPlb.Visible = True
            tblCAPResponding.Visible = ddlSubType.SelectedValue.Equals("ELT")
            tblAircraftOverdue.Visible = ddlSubType.SelectedValue.Equals("ELT")
        Else
            pnlShowEltEpirbPlb.Visible = False
        End If

        '==================================================
        If ddlSubType.SelectedValue = "Structure Collapse" Or ddlSubType.SelectedValue = "Industrial Accident" Or ddlSubType.SelectedValue = "Transportation Accident" Or ddlSubType.SelectedValue = "Other" Then
            pnlStructCollapseIndusAccTransAccOther.Visible = True
        Else
            pnlStructCollapseIndusAccTransAccOther.Visible = False
        End If

        '==================================================
        If ddlSubType.SelectedValue = "LE Search (Missing Person)" Then
            pnlShowLESearch.Visible = True
        Else
            pnlShowLESearch.Visible = False
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

    Protected Sub ddlUnmetNeeds_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUnmetNeeds.SelectedIndexChanged

        If ddlUnmetNeeds.SelectedValue = "Yes" Then
            pnlShowUnmetNeeds.Visible = True
        Else
            pnlShowUnmetNeeds.Visible = False
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

        'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)

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

    Protected Sub ddlIsCollapse_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlIsCollapse.SelectedIndexChanged
        tblCollapseCause.Visible = ddlIsCollapse.SelectedItem.Value.Equals("Yes")
    End Sub

    Protected Sub ddlPeopletrapped_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlPeopletrapped.SelectedIndexChanged
        tblNumberPeopleTrapped.Visible = ddlPeopletrapped.SelectedItem.Value.Equals("Yes")
    End Sub

    Private Sub ShowHiddenTables()
        tblCollapseCause.Visible = ddlIsCollapse.SelectedItem.Value.Equals("Yes")
        tblNumberPeopleTrapped.Visible = ddlPeopletrapped.SelectedItem.Value.Equals("Yes")
    End Sub

End Class
