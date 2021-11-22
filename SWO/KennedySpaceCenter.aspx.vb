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

Partial Class KennedySpaceCenter
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

            Dim localKennedySpaceCenterCount As Integer = 0

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectKennedySpaceCenterCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                localKennedySpaceCenterCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()


            If localKennedySpaceCenterCount > 0 Then
                PopulatePage()
            End If

        End If

    End Sub


    'PagePopulation
    Protected Sub PopulatePage()

        Dim localTime As String = ""
        Dim localTime2 As String = ""
        Dim localTime3 As String = ""
        Dim localTime4 As String = ""
        Dim localTime5 As String = ""
        

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectKennedySpaceCenterByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            ddlSubType.SelectedValue = HelpFunction.Convertdbnulls(objDR("SubType"))
            ddlSituation.SelectedValue = HelpFunction.Convertdbnulls(objDR("Situation"))
            txtMissionName.Text = HelpFunction.Convertdbnulls(objDR("MissionName"))
            txtInrlMissionLaunchDate.Text = HelpFunction.Convertdbnulls(objDR("InrlMissionLaunchDate"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR("InrlLaunchWindow")))
            localTime2 = CStr(HelpFunction.Convertdbnulls(objDR("InrlLaunchWindow2")))
            txtInrlBrevardCo.Text = HelpFunction.Convertdbnulls(objDR("InrlBrevardCo"))
            txtInrlBrevardCo2.Text = HelpFunction.Convertdbnulls(objDR("InrlBrevardCo2"))
            txtNextMissionLaunchDate.Text = HelpFunction.Convertdbnulls(objDR("NextMissionLaunchDate"))
            txtScrubDate.Text = HelpFunction.Convertdbnulls(objDR("ScrubDate"))
            localTime3 = CStr(HelpFunction.Convertdbnulls(objDR("ScrubTime")))
            txtScrubReason.Text = HelpFunction.Convertdbnulls(objDR("ScrubReason"))
            txtScrubNextLaunchDateTime.Text = HelpFunction.Convertdbnulls(objDR("ScrubNextLaunchDateTime"))
            txtSuccessDate.Text = HelpFunction.Convertdbnulls(objDR("SuccessDate"))
            localTime4 = CStr(HelpFunction.Convertdbnulls(objDR("SuccessTime")))
            txtUnsuccessDate.Text = HelpFunction.Convertdbnulls(objDR("UnsuccessDate"))
            localTime5 = CStr(HelpFunction.Convertdbnulls(objDR("UnsuccessTime")))
            txtUnsuccessReason.Text = HelpFunction.Convertdbnulls(objDR("UnsuccessReason"))
            ddlUnsuccessOffSiteImpact.SelectedValue = HelpFunction.Convertdbnulls(objDR("UnsuccessOffSiteImpact"))
            txtUnsuccessOffSiteImpactText.Text = HelpFunction.Convertdbnulls(objDR("UnsuccessOffSiteImpactText"))
            ddlInjury.SelectedValue = HelpFunction.Convertdbnulls(objDR("Injury"))
            txtInjury.Text = HelpFunction.Convertdbnulls(objDR("InjuryText"))
            ddlFatality.SelectedValue = HelpFunction.Convertdbnulls(objDR("Fatality"))
            txtFatalityText.Text = HelpFunction.Convertdbnulls(objDR("FatalityText"))
            ddlNotification.SelectedValue = HelpFunction.Convertdbnulls(objDR("IncidentTypeLevelID"))
            ddlLaunchLocation.SelectedValue = HelpFunction.Convertdbnulls(objDR("LaunchLocation"))
            txtLaunchLocationDescription.Text = HelpFunction.Convertdbnulls(objDR("LaunchLocationText"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        txtWorkSheetDescription.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        ''Notification or Rescheduled
        If ddlSubType.SelectedValue = "Initial Notification" Or ddlSubType.SelectedValue = "Rescheduled Launch" Then
            pnlInitialNotificationRescheduledLaunch.Visible = True
        Else
            pnlInitialNotificationRescheduledLaunch.Visible = False
        End If

        ''Scrubbed Launch 
        If ddlSubType.SelectedValue = "Scrubbed Launch" Then
            pnlScrubbedLaunch.Visible = True
        Else
            pnlScrubbedLaunch.Visible = False
        End If

        'Successful Launch
        If ddlSubType.SelectedValue = "Successful Launch" Then
            pnlSuccessfulLaunch.Visible = True
        Else
            pnlSuccessfulLaunch.Visible = False
        End If

        ''Unsuccessful Launch
        If ddlSubType.SelectedValue = "Unsuccessful Launch" Then
            pnlUnsuccessfulLaunch.Visible = True
        Else
            pnlUnsuccessfulLaunch.Visible = False
        End If

        ''Other
        If ddlSubType.SelectedValue = "Other" Then
            pnlShowOther.Visible = True
        Else
            pnlShowOther.Visible = False
        End If

        If ddlLaunchLocation.SelectedValue = "Other" Then
            tblLaunchLocationText.Visible = True
        End If


        ''Making the time All Morris Day like
        txtInrlLaunchWindow.Text = Left(localTime, 2)
        txtInrlLaunchWindowB.Text = Right(localTime, 2)

        txtInrlLaunchWindow2.Text = Left(localTime2, 2)
        txtInrlLaunchWindow2B.Text = Right(localTime2, 2)

        txtScrubTime.Text = Left(localTime3, 2)
        txtScrubTime2.Text = Right(localTime3, 2)

        txtSuccessTime.Text = Left(localTime4, 2)
        txtSuccessTime2.Text = Right(localTime4, 2)

        txtUnsuccessTime.Text = Left(localTime5, 2)
        txtUnsuccessTime2.Text = Right(localTime5, 2)



        If txtInrlLaunchWindow.Text = "0" Then
            txtInrlLaunchWindow.Text = ""
        End If

        If txtInrlLaunchWindowB.Text = "0" Then
            txtInrlLaunchWindowB.Text = ""
        End If

        If txtInrlLaunchWindow2B.Text = "0" Then
            txtInrlLaunchWindow2B.Text = ""
        End If

        If txtInrlLaunchWindow2.Text = "0" Then
            txtInrlLaunchWindow2.Text = ""
        End If

        If txtScrubTime.Text = "0" Then
            txtScrubTime.Text = ""
        End If

        If txtScrubTime2.Text = "0" Then
            txtScrubTime2.Text = ""
        End If

        If txtSuccessTime.Text = "0" Then
            txtSuccessTime.Text = ""
        End If

        If txtSuccessTime2.Text = "0" Then
            txtSuccessTime2.Text = ""
        End If

        If txtUnsuccessTime.Text = "0" Then
            txtUnsuccessTime.Text = ""
        End If

        If txtUnsuccessTime2.Text = "0" Then
            txtUnsuccessTime2.Text = ""
        End If

        If txtInrlMissionLaunchDate.Text = "1/1/1900" Then
            txtInrlMissionLaunchDate.Text = ""
        End If

        If txtNextMissionLaunchDate.Text = "1/1/1900" Then
            txtNextMissionLaunchDate.Text = ""
        End If

        If txtSuccessDate.Text = "1/1/1900" Then
            txtSuccessDate.Text = ""
        End If

        If txtUnsuccessDate.Text = "1/1/1900" Then
            txtUnsuccessDate.Text = ""
        End If

        If ddlUnsuccessOffSiteImpact.SelectedValue = "Yes" Then
            pnlShowUnsuccessOffSiteImpactText.Visible = False
        End If

        If ddlInjury.SelectedValue = "Yes" Then
            pnlShowInjuryText.Visible = True
        End If

        If ddlFatality.SelectedValue = "Yes" Then
            'pnlShowFatalityText.Visible = True
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


    'DropDownLists
    Protected Sub ddlSubType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubType.SelectedIndexChanged

        If ddlSubType.SelectedValue = "Initial Notification" Or ddlSubType.SelectedValue = "Rescheduled Launch" Then
            pnlInitialNotificationRescheduledLaunch.Visible = True
        Else
            pnlInitialNotificationRescheduledLaunch.Visible = False
        End If

        If ddlSubType.SelectedValue = "Scrubbed Launch" Then
            pnlScrubbedLaunch.Visible = True
        Else
            pnlScrubbedLaunch.Visible = False
        End If

        If ddlSubType.SelectedValue = "Successful Launch" Then
            pnlSuccessfulLaunch.Visible = True
        Else
            pnlSuccessfulLaunch.Visible = False
        End If

        If ddlSubType.SelectedValue = "Unsuccessful Launch" Then
            pnlUnsuccessfulLaunch.Visible = True
        Else
            pnlUnsuccessfulLaunch.Visible = False
        End If

        If ddlSubType.SelectedValue = "Other" Then
            pnlShowOther.Visible = True
            pnlLaunchLocation.Visible = False
        Else
            pnlShowOther.Visible = False
            pnlLaunchLocation.Visible = True
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

    Protected Sub ddlUnsuccessOffSiteImpact_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUnsuccessOffSiteImpact.SelectedIndexChanged

        If ddlUnsuccessOffSiteImpact.SelectedValue = "Yes" Then
            pnlShowUnsuccessOffSiteImpactText.Visible = True
        Else
            pnlShowUnsuccessOffSiteImpactText.Visible = False
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

        Dim localKennedySpaceCenterCount As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectKennedySpaceCenterCountByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            localKennedySpaceCenterCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()


        'We add these to blank since the panels are not visible
        If pnlShowInjuryText.Visible = False Then
            txtInjury.Text = ""
        End If

        If pnlShowFatalityText.Visible = False Then
            txtFatalityText.Text = ""
        End If

        If pnlInitialNotificationRescheduledLaunch.Visible = False Then
            txtInrlMissionLaunchDate.Text = ""
            txtInrlLaunchWindow.Text = ""
            txtInrlLaunchWindowB.Text = ""
            txtInrlLaunchWindow2.Text = ""
            txtInrlLaunchWindow2B.Text = ""
            txtInrlBrevardCo.Text = ""
            txtInrlBrevardCo2.Text = ""
            txtNextMissionLaunchDate.Text = ""
        End If

        If pnlScrubbedLaunch.Visible = False Then
            txtScrubDate.Text = ""
            txtScrubTime.Text = ""
            txtScrubTime2.Text = ""
            txtScrubReason.Text = ""
            txtScrubNextLaunchDateTime.Text = ""
        End If

        If pnlSuccessfulLaunch.Visible = False Then
            txtSuccessDate.Text = ""
            txtSuccessTime.Text = ""
            txtSuccessTime2.Text = ""
        End If

        If pnlUnsuccessfulLaunch.Visible = False Then
            txtUnsuccessDate.Text = ""
            txtUnsuccessTime.Text = ""
            txtUnsuccessTime2.Text = ""
            txtUnsuccessReason.Text = ""
            txtUnsuccessOffSiteImpactText.Text = ""
            ddlUnsuccessOffSiteImpact.SelectedValue = "Select an Option"
            txtUnsuccessOffSiteImpactText.Text = ""
            ddlInjury.SelectedValue = "Select an Option"
            txtInjury.Text = ""
            ddlFatality.SelectedValue = "Select an Option"
            txtFatalityText.Text = ""
        End If

        If pnlShowOther.Visible = True Then
            ddlLaunchLocation.SelectedValue = "Select an Option"
            txtLaunchLocationDescription.Text = ""
        End If

        If ddlLaunchLocation.SelectedValue <> "Other" Then txtLaunchLocationDescription.Text = ""

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


        If localKennedySpaceCenterCount = 0 Then

            'Try

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionKennedySpaceCenter", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 0)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@MissionName", txtMissionName.Text)
            objCmd.Parameters.AddWithValue("@InrlMissionLaunchDate", txtInrlMissionLaunchDate.Text)
            objCmd.Parameters.AddWithValue("@InrlLaunchWindow", CStr(txtInrlLaunchWindow.Text.Trim) & CStr(txtInrlLaunchWindowB.Text.Trim))
            objCmd.Parameters.AddWithValue("@InrlLaunchWindow2", CStr(txtInrlLaunchWindow2.Text.Trim) & CStr(txtInrlLaunchWindow2B.Text.Trim))
            objCmd.Parameters.AddWithValue("@InrlBrevardCo", txtInrlBrevardCo.Text)
            objCmd.Parameters.AddWithValue("@InrlBrevardCo2 ", txtInrlBrevardCo2.Text)
            objCmd.Parameters.AddWithValue("@NextMissionLaunchDate", txtNextMissionLaunchDate.Text)
            objCmd.Parameters.AddWithValue("@ScrubDate", txtScrubDate.Text)
            objCmd.Parameters.AddWithValue("@ScrubTime", CStr(txtScrubTime.Text.Trim) & CStr(txtScrubTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@ScrubReason", txtScrubReason.Text)
            objCmd.Parameters.AddWithValue("@ScrubNextLaunchDateTime", txtScrubNextLaunchDateTime.Text)
            objCmd.Parameters.AddWithValue("@SuccessDate", txtSuccessDate.Text)
            objCmd.Parameters.AddWithValue("@SuccessTime", CStr(txtSuccessTime.Text.Trim) & CStr(txtSuccessTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@UnsuccessReason ", txtUnsuccessReason.Text)
            objCmd.Parameters.AddWithValue("@UnsuccessDate", txtUnsuccessDate.Text)
            objCmd.Parameters.AddWithValue("@UnsuccessTime", CStr(txtUnsuccessTime.Text.Trim) & CStr(txtUnsuccessTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@UnsuccessOffSiteImpact", ddlUnsuccessOffSiteImpact.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@UnsuccessOffSiteImpactText ", txtUnsuccessOffSiteImpactText.Text)
            objCmd.Parameters.AddWithValue("@Injury", ddlInjury.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@InjuryText", txtInjury.Text)
            objCmd.Parameters.AddWithValue("@Fatality", ddlFatality.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@FatalityText", txtFatalityText.Text)
            objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", ddlNotification.SelectedValue)
            objCmd.Parameters.AddWithValue("@LaunchLocation", ddlLaunchLocation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@LaunchLocationText", txtLaunchLocationDescription.Text)

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
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Added Kennedy Space Center")

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
            objCmd = New SqlCommand("spActionKennedySpaceCenter", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 1)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@MissionName", txtMissionName.Text)
            objCmd.Parameters.AddWithValue("@InrlMissionLaunchDate", txtInrlMissionLaunchDate.Text)
            objCmd.Parameters.AddWithValue("@InrlLaunchWindow", CStr(txtInrlLaunchWindow.Text.Trim) & CStr(txtInrlLaunchWindowB.Text.Trim))
            objCmd.Parameters.AddWithValue("@InrlLaunchWindow2", CStr(txtInrlLaunchWindow2.Text.Trim) & CStr(txtInrlLaunchWindow2B.Text.Trim))
            objCmd.Parameters.AddWithValue("@InrlBrevardCo", txtInrlBrevardCo.Text)
            objCmd.Parameters.AddWithValue("@InrlBrevardCo2 ", txtInrlBrevardCo2.Text)
            objCmd.Parameters.AddWithValue("@NextMissionLaunchDate", txtNextMissionLaunchDate.Text)
            objCmd.Parameters.AddWithValue("@ScrubDate", txtScrubDate.Text)
            objCmd.Parameters.AddWithValue("@ScrubTime", CStr(txtScrubTime.Text.Trim) & CStr(txtScrubTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@ScrubReason", txtScrubReason.Text)
            objCmd.Parameters.AddWithValue("@ScrubNextLaunchDateTime", txtScrubNextLaunchDateTime.Text)
            objCmd.Parameters.AddWithValue("@SuccessDate", txtSuccessDate.Text)
            objCmd.Parameters.AddWithValue("@SuccessTime", CStr(txtSuccessTime.Text.Trim) & CStr(txtSuccessTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@UnsuccessReason ", txtUnsuccessReason.Text)
            objCmd.Parameters.AddWithValue("@UnsuccessDate", txtUnsuccessDate.Text)
            objCmd.Parameters.AddWithValue("@UnsuccessTime", CStr(txtUnsuccessTime.Text.Trim) & CStr(txtUnsuccessTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@UnsuccessOffSiteImpact", ddlUnsuccessOffSiteImpact.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@UnsuccessOffSiteImpactText ", txtUnsuccessOffSiteImpactText.Text)
            objCmd.Parameters.AddWithValue("@Injury", ddlInjury.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@InjuryText", txtInjury.Text)
            objCmd.Parameters.AddWithValue("@Fatality", ddlFatality.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@FatalityText", txtFatalityText.Text)
            objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", ddlNotification.SelectedValue)
            objCmd.Parameters.AddWithValue("@LaunchLocation", ddlLaunchLocation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@LaunchLocationText", txtLaunchLocationDescription.Text)

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
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Updated Kennedy Space Center")

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



        'Finish the Error String.
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels.
        lblMessage.Text = strError.ToString
    End Sub

    Protected Sub ddlLaunchLocation_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlLaunchLocation.SelectedIndexChanged
        tblLaunchLocationText.Visible = ddlLaunchLocation.SelectedValue = "Other"
    End Sub
End Class
