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

Partial Class ReportBuilder
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

    Dim globalHasErrors As Boolean = False
    Dim globalMessage As String

    'Dim ObjCookie As System.Web.HttpCookie
    Dim ns As New SecurityValidate
    Public MrDataGrabber As New DataGrabber

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            PopulateDDLs()

            'Clean up reports.
            HelpFunction.CleanupReportDirectory()
        End If
    End Sub

    Private Sub PopulateDDLs()
        'ObjCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(ObjCookie)
        ns = Session("Security_Tracker")

        'ObjCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)

        Dim strUserLevelID As String = ns.UserLevelID
        Dim strUserID As String = ns.UserID.ToString()

        Try
            'Select Case strUserLevelID
            '   Case "1"
            '       'Admin.
            '       '-------------------------------------------------------------------------------------------------------
            '       'Grab all users from the database and bind them to the drop down list.
            '       objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            '       objCmd = New SqlCommand("spSelectUserNotAdmin", objConn)
            '       objCmd.CommandType = CommandType.StoredProcedure

            '       'Open the connection.
            '       DBConStringHelper.PrepareConnection(objConn)
            '       lbxUsers.DataSource = objCmd.ExecuteReader()
            '       lbxUsers.DataBind()

            '       'Close the connection.
            '       DBConStringHelper.FinalizeConnection(objConn)

            '       objCmd = Nothing

            '       'Add an "All" item to the list.
            '       lbxUsers.Items.Insert(0, New ListItem("All", "0"))
            '       lbxUsers.Items(0).Selected = True
            '       '-------------------------------------------------------------------------------------------------------
            '   Case "2"
            '       'User.
            '       '-------------------------------------------------------------------------------------------------------
            '       'Grab all users from the database and bind them to the drop down list.
            '       objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            '       objCmd = New SqlCommand("spSelectUserByUserIDForStats", objConn)
            '       objCmd.CommandType = CommandType.StoredProcedure
            '       objCmd.Parameters.AddWithValue("@UserID", strUserID)

            '       'Open the connection.
            '       DBConStringHelper.PrepareConnection(objConn)
            '       lbxUsers.DataSource = objCmd.ExecuteReader()
            '       lbxUsers.DataBind()

            '       'Close the connection.
            '       DBConStringHelper.FinalizeConnection(objConn)\

            '       objCmd = Nothing

            '       'Add an "All" item to the list.
            '       'lbxUsers.Items.Insert(0, New ListItem("All", "0"))
            '       'lbxUsers.Items(0).Selected = True
            '       '-------------------------------------------------------------------------------------------------------
            '   Case "3"
            '       'Super PAC.
            '       '-------------------------------------------------------------------------------------------------------
            '       'Grab all users from the database and bind them to the drop down list.
            '       objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            '       objCmd = New SqlCommand("spSelectUserByRegion", objConn)
            '       objCmd.CommandType = CommandType.StoredProcedure
            '       objCmd.Parameters.AddWithValue("@Region", MrDataGrabber.GrabOneStringColumnByPrimaryKey("Region", "[User]", "UserID", strUserID))

            '       'Open the connection.
            '       DBConStringHelper.PrepareConnection(objConn)
            '       lbxUsers.DataSource = objCmd.ExecuteReader()
            '       lbxUsers.DataBind()

            '       'Close the connection.
            '       DBConStringHelper.FinalizeConnection(objConn)

            '       objCmd = Nothing

            '       'Add an "All" item to the list.
            '       lbxUsers.Items.Insert(0, New ListItem("All", "0"))
            '       lbxUsers.Items(0).Selected = True
            '       '-------------------------------------------------------------------------------------------------------
            '   Case Else

            'End Select

            'Users.
            '-------------------------------------------------------------------------------------------------------
            'Grab all users from the database and bind them to the drop down list.
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objCmd = New SqlCommand("spSelectUser", objConn)
            objCmd.CommandType = CommandType.StoredProcedure

            'Open the connection.
            DBConStringHelper.PrepareConnection(objConn)
            'lbxUsers.DataSource = objCmd.ExecuteReader()
            'lbxUsers.DataBind()

            'Close the connection.
            DBConStringHelper.FinalizeConnection(objConn)

            objCmd = Nothing

            'Add an "All" item to the list.
            'lbxUsers.Items.Insert(0, New ListItem("All", "0"))
            'lbxUsers.Items(0).Selected = True
            '-------------------------------------------------------------------------------------------------------

            'IncidentType.
            '-------------------------------------------------------------------------------------------------------
            If strUserLevelID = "1" Then
                'Grab all incident types from the database and bind them to the drop down list.
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objCmd = New SqlCommand("spSelectIncidentType", objConn)
                objCmd.CommandType = CommandType.StoredProcedure

                'Open the connection.
                DBConStringHelper.PrepareConnection(objConn)
                lbxIncidentType.DataSource = objCmd.ExecuteReader()
                lbxIncidentType.DataBind()

                'Close the connection.
                DBConStringHelper.FinalizeConnection(objConn)

                objCmd = Nothing

                'Add an "All" item to the list.
                lbxIncidentType.Items.Insert(0, New ListItem("All", "0"))
                lbxIncidentType.Items(0).Selected = True
            Else
                'Grab all incident types from the database and bind them to the drop down list.
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objCmd = New SqlCommand("spSelectIncidentTypeUserByUserID", objConn)
                objCmd.Parameters.AddWithValue("@UserID", strUserID)
                objCmd.CommandType = CommandType.StoredProcedure

                'Open the connection.
                DBConStringHelper.PrepareConnection(objConn)
                lbxIncidentType.DataSource = objCmd.ExecuteReader()
                lbxIncidentType.DataBind()

                'Close the connection.
                DBConStringHelper.FinalizeConnection(objConn)

                objCmd = Nothing

                'Add an "All" item to the list.
                lbxIncidentType.Items.Insert(0, New ListItem("All", "0"))
                lbxIncidentType.Items(0).Selected = True
            End If
            '-------------------------------------------------------------------------------------------------------

            'County.
            '-------------------------------------------------------------------------------------------------------
            'Grab all counties from the database and bind them to the drop down list.
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objCmd = New SqlCommand("spSelectCounty", objConn)
            objCmd.CommandType = CommandType.StoredProcedure

            'Open the connection.
            DBConStringHelper.PrepareConnection(objConn)
            lbxCounty.DataSource = objCmd.ExecuteReader()
            lbxCounty.DataBind()

            'Close the connection.
            DBConStringHelper.FinalizeConnection(objConn)

            objCmd = Nothing

            lbxCounty.Items(0).Selected = True
            '-------------------------------------------------------------------------------------------------------

            'Activities.
            '-------------------------------------------------------------------------------------------------------
            'Grab all counties from the database and bind them to the drop down list.
            'objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            'objCmd = New SqlCommand("spSelectActivity", objConn)
            'objCmd.CommandType = CommandType.StoredProcedure

            'Open the connection.
            'DBConStringHelper.PrepareConnection(objConn)
            'lbxActivity.DataSource = objCmd.ExecuteReader()
            'lbxActivity.DataBind()

            'Close the connection.
            'DBConStringHelper.FinalizeConnection(objConn)

            'objCmd = Nothing

            'Add an "All" item to the list.
            'lbxActivity.Items.Insert(0, New ListItem("All", "0"))
            'lbxActivity.Items(0).Selected = True
            '-------------------------------------------------------------------------------------------------------

            'Applicants.
            '-------------------------------------------------------------------------------------------------------
            'Grab all counties from the database and bind them to the drop down list.
            'objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            'objCmd = New SqlCommand("spSelectApplicant", objConn)
            'objCmd.CommandType = CommandType.StoredProcedure

            'Open the connection.
            'DBConStringHelper.PrepareConnection(objConn)
            'lbxApplicant.DataSource = objCmd.ExecuteReader()
            'lbxApplicant.DataBind()

            'Close the connection.
            'DBConStringHelper.FinalizeConnection(objConn)

            'objCmd = Nothing

            'Add an "All" item to the list.
            'lbxApplicant.Items.Insert(0, New ListItem("All", "0"))
            'lbxApplicant.Items(0).Selected = True
            '-------------------------------------------------------------------------------------------------------
        Catch ex As Exception
            Response.Write(ex.ToString)

            Exit Sub
        End Try
    End Sub

    Protected Sub btnRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRunReport.Click
        'The code below will show all values that are being passed to the selected report page.
        '-------------------------------------------------------------------------------------------------------
        'Response.Write(lbxCounty.SelectedItem)
        'Response.Write("<br>")
        'Response.Write(ddlEvent.SelectedValue)
        'Response.Write("<br>")
        'Response.Write(ddlReconTeam.SelectedValue)
        'Response.Write("<br>")
        'Response.Write(ddlReconTeamNumber.SelectedItem)
        'Response.Write("<br>")
        'Response.Write(ddlReportFormat.SelectedValue)
        'Response.Write("<br>")
        'Response.Write(ddlTriage.SelectedValue)
        'Response.Write("<br>")
        'Response.End()
        '-------------------------------------------------------------------------------------------------------

        Dim strRedirect As String = ""
        Dim intSelectedCounter As Integer = 0

        'ObjCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        ns = Session("Security_Tracker")

        Dim strUserLevelID As String = ns.UserLevelID

        Select Case strUserLevelID
            Case "1"
                'Admin.
                '-------------------------------------------------------------------------------------------------------

                '-------------------------------------------------------------------------------------------------------
            Case "2"
                'User.
                '-------------------------------------------------------------------------------------------------------

                '-------------------------------------------------------------------------------------------------------
            Case "3"
                'Super PAC.
                '-------------------------------------------------------------------------------------------------------
                'If lbxUsers.Items(0).Selected Then
                '    'Response.Write(lbxUsers.Items(0).Text)
                '    'Response.End()

                '    Dim j As Integer = 0

                '    For j = 0 To lbxUsers.Items.Count - 1
                '        'If lbxUsers.Items(j).Selected = True Or lbxUsers.Items(j).Selected = False Then
                '        lbxUsers.Items(j).Selected = True

                '        'Response.Write(lbxUsers.Items(j).Selected = True)
                '        'Response.Write("<br>")
                '        'Response.Write(j)
                '        'Response.Write("<br>")

                '        intSelectedCounter += j
                '        'End If
                '    Next

                '    lbxUsers.Items(0).Selected = False
                'End If

                'intSelectedCounter = 0

                'Dim i As Integer = 0

                'For i = 0 To lbxUsers.Items.Count - 1
                '    If lbxUsers.Items(i).Selected = True Then
                '        Response.Write(lbxUsers.Items(i).Text)
                '        Response.Write("<br>")

                '        intSelectedCounter += 1
                '    End If
                'Next

                'Response.Write("Not All")
                'Response.End()
            Case Else
        End Select

        'UserID.
        '----------------------------------------------
        Dim strUserID As String = ""
        Dim strUser As String = ""

        'If lbxUsers.Enabled = True Then
        '    'Makes sure we pass a value.
        '    If lbxUsers.SelectedValue.ToString = "" Then
        '        lbxUsers.SelectedValue = "0"
        '    End If

        '    Dim i As Integer = 0

        '    For i = 0 To lbxUsers.Items.Count - 1
        '        If lbxUsers.Items(i).Selected = True Then
        '            strUserID = strUserID & lbxUsers.Items(i).Value & ","
        '            strUser = strUser & lbxUsers.Items(i).Text & ","

        '            intSelectedCounter += 1
        '        End If
        '    Next

        '    'Remove the last comma.
        '    strUserID = Mid(strUserID, 1, Len(strUserID) - 1)
        '    strUser = Mid(strUser, 1, Len(strUser) - 1)
        'Else
        '    strUserID = "0"
        '    strUser = "All"
        'End If

        'IncidentTypeID.
        '----------------------------------------------
        Dim strIncidentTypeID As String = ""
        Dim strIncidentType As String = ""

        If lbxIncidentType.Enabled = True Then
            'Makes sure we pass a value.
            If lbxIncidentType.SelectedValue.ToString = "" Then
                lbxIncidentType.SelectedValue = "0"
            End If

            Dim i As Integer = 0

            For i = 1 To lbxIncidentType.Items.Count - 1
                If lbxIncidentType.Items(i).Selected = True Or lbxIncidentType.SelectedValue = "0" Then
                    strIncidentTypeID = strIncidentTypeID & lbxIncidentType.Items(i).Value & ","
                    strIncidentType = strIncidentType & lbxIncidentType.Items(i).Text & ","

                    intSelectedCounter += 1
                End If
            Next

            If Not String.IsNullOrEmpty(strIncidentTypeID) Then
                'Remove the last comma.
                strIncidentTypeID = Mid(strIncidentTypeID, 1, Len(strIncidentTypeID) - 1)
                strIncidentType = Mid(strIncidentType, 1, Len(strIncidentType) - 1)
            End If
        Else
            strIncidentTypeID = "0"
            strIncidentType = "All"
        End If

        'County.
        '----------------------------------------------
        'Dim strCountyID As String = ""
        Dim strCounty As String = ""

        If lbxCounty.Enabled = True Then
            'Makes sure we pass a value.
            If lbxCounty.SelectedValue.ToString = "" Then
                lbxCounty.SelectedValue = "0"
            End If

            Dim i As Integer = 0

            For i = 0 To lbxCounty.Items.Count - 1
                If lbxCounty.Items(i).Selected = True Then
                    'strCountyID = strCountyID & lbxCounty.Items(i).Value & ","
                    strCounty = strCounty & lbxCounty.Items(i).Text.Trim & ","

                    intSelectedCounter += 1
                End If
            Next

            'Remove the last comma.
            'strCountyID = Mid(strCountyID, 1, Len(strCountyID) - 1)
            strCounty = Mid(strCounty, 1, Len(strCounty) - 1)
        Else

        End If

        'DisasterID.
        '----------------------------------------------
        'Dim strDisasterID As String = ""
        'Dim strDisaster As String = ""

        'If lbxDisaster.Enabled = True Then
        '    intSelectedCounter = 0

        '    'Makes sure we pass a value.
        '    If lbxDisaster.SelectedValue.ToString = "" Then
        '        lbxDisaster.SelectedValue = "0"
        '    End If

        '    Dim i As Integer = 0

        '    For i = 0 To lbxDisaster.Items.Count - 1
        '        If lbxDisaster.Items(i).Selected = True Then
        '            strDisasterID = strDisasterID & lbxDisaster.Items(i).Value & ","
        '            strDisaster = strDisaster & lbxDisaster.Items(i).Text & ","

        '            intSelectedCounter += 1
        '        End If
        '    Next

        '    'Remove the last comma.
        '    strDisasterID = Mid(strDisasterID, 1, Len(strDisasterID) - 1)
        '    strDisaster = Mid(strDisaster, 1, Len(strDisaster) - 1)
        'Else
        '    strDisasterID = "0"
        '    strDisaster = "All"
        'End If

        'ActivityID.
        '----------------------------------------------
        'Dim strActivityID As String = ""
        'Dim strActivity As String = ""

        'If lbxActivity.Enabled = True Then
        '    intSelectedCounter = 0

        '    'Makes sure we pass a value.
        '    If lbxActivity.SelectedValue.ToString = "" Then
        '        lbxActivity.SelectedValue = "0"
        '    End If

        '    Dim i As Integer = 0

        '    For i = 0 To lbxActivity.Items.Count - 1
        '        If lbxActivity.Items(i).Selected = True Then
        '            strActivityID = strActivityID & lbxActivity.Items(i).Value & ","
        '            strActivity = strActivity & lbxActivity.Items(i).Text & ","

        '            intSelectedCounter += 1
        '        End If
        '    Next

        '    'Remove the last comma.
        '    strActivityID = Mid(strActivityID, 1, Len(strActivityID) - 1)
        '    strActivity = Mid(strActivity, 1, Len(strActivity) - 1)
        'Else
        '    strActivityID = "0"
        '    strActivity = "All"
        'End If

        'ApplicantID.
        '----------------------------------------------
        'Dim strApplicantID As String = ""
        'Dim strApplicant As String = ""

        'If lbxApplicant.Enabled = True Then
        '    intSelectedCounter = 0

        '    'Makes sure we pass a value.
        '    If lbxApplicant.SelectedValue.ToString = "" Then
        '        lbxApplicant.SelectedValue = "0"
        '    End If

        '    Dim i As Integer = 0

        '    For i = 0 To lbxApplicant.Items.Count - 1
        '        If lbxApplicant.Items(i).Selected = True Then
        '            strApplicantID = strApplicantID & lbxApplicant.Items(i).Value & ","
        '            strApplicant = strApplicant & lbxApplicant.Items(i).Text & ","
        '            intSelectedCounter += 1
        '        End If
        '    Next

        '    'Remove the last comma.
        '    strApplicantID = Mid(strApplicantID, 1, Len(strApplicantID) - 1)
        '    strApplicant = Mid(strApplicant, 1, Len(strApplicant) - 1)
        'Else
        '    strApplicantID = "0"
        '    strApplicant = "All"
        'End If

        Dim localSummation As String = ""

        If cbxSummation.Checked = True Then
            localSummation = "True"
        Else
            localSummation = "False"
        End If

        Dim localAllToDate As String = ""
        Dim localStartDate As String = ""
        Dim localEndDate As String = ""

        If rdoAllDates.Checked = True Then
            localAllToDate = "Yes"
        Else
            localAllToDate = "No"
        End If

        If txtStartDate.Text = "" Then
            'localStartDate = "All"
        Else
            localStartDate = txtStartDate.Text
        End If

        If txtEndDate.Text = "" Then
            'localEndDate = "All"
        Else
            localEndDate = txtEndDate.Text
        End If

        ErrorChecks()

        If globalHasErrors = True Then
            pnlMessage.Visible = True

            Exit Sub
        Else
            pnlMessage.Visible = False

            If rdoIncidentWorksheet.Checked = True Then
                If ddlReportFormat.SelectedValue.ToString = "Graph" Then
                    strRedirect = "Reports/IncidentWorksheetGraph.aspx?IncidentType=" & strIncidentType & "&IncidentTypeID=" & strIncidentTypeID & "&AllToDate=" & localAllToDate & "&StartDate=" & localStartDate & "&EndDate=" & localEndDate & "&ReportFormat=" & ddlReportFormat.SelectedValue
                Else
                    strRedirect = "Reports/IncidentWorksheetReport.aspx?IncidentType=" & strIncidentType & "&IncidentTypeID=" & strIncidentTypeID & "&AllToDate=" & localAllToDate & "&StartDate=" & localStartDate & "&EndDate=" & localEndDate & "&ReportFormat=" & ddlReportFormat.SelectedValue
                End If
            End If

            'If rdoWorksheetCountByCounty.Checked = True Then
            '    If ddlReportFormat.SelectedValue.ToString = "Graph" Then
            '        strRedirect = "Reports/WorksheetCountByCountyGraph.aspx?IncidentType=" & strIncidentType & "&IncidentTypeID=" & strIncidentTypeID & "&ReportFormat=" & ddlReportFormat.SelectedValue & "&County=" & lbxCounty.SelectedValue & "&AllToDate=" & localAllToDate & "&StartDate=" & localStartDate & "&EndDate=" & localEndDate
            '    Else
            '        'strRedirect = "Reports/WorksheetCountByCountyGraph.aspx?IncidentType=" & strIncidentType & "&IncidentTypeID=" & strIncidentTypeID & "&ReportFormat=" & ddlReportFormat.SelectedValue & "&County=" & lbxCounty.SelectedValue & "&AllToDate=" & localAllToDate & "&StartDate=" & localStartDate & "&EndDate=" & localEndDate
            '    End If
            'End If

            If rdoWorksheetCounty.Checked = True Then
                If ddlReportFormat.SelectedValue.ToString = "Graph" Then
                    strRedirect = "Reports/WorksheetByIncidentCounty.aspx?IncidentType=" & strIncidentType & "&IncidentTypeID=" & strIncidentTypeID & "&ReportFormat=" & ddlReportFormat.SelectedValue & "&County=" & strCounty & "&Summation=" & localSummation & "&AllToDate=" & localAllToDate & "&StartDate=" & localStartDate & "&EndDate=" & localEndDate
                Else
                    strRedirect = "Reports/WorksheetByIncidentCounty.aspx?IncidentType=" & strIncidentType & "&IncidentTypeID=" & strIncidentTypeID & "&ReportFormat=" & ddlReportFormat.SelectedValue & "&County=" & strCounty & "&Summation=" & localSummation & "&AllToDate=" & localAllToDate & "&StartDate=" & localStartDate & "&EndDate=" & localEndDate
                End If
            End If

            If rdoTotalActivity.Checked = True Then
                'strRedirect = "Reports/Daily_Activity_Reports.aspx?User=" & strUser & "&UserID=" & strUserID & "&Disaster=" & strDisaster & "&Activity=" & strActivity & "&Applicant=" & strApplicant & "&DisasterID=" & strDisasterID & "&ActivityID=" & strActivityID & "&ApplicantID=" & strApplicantID & "&AllToDate=" & localAllToDate & "&StartDate=" & localStartDate & "&EndDate=" & localEndDate & "&ReportFormat=" & ddlReportFormat.SelectedValue
            ElseIf "" = "" Then

            Else

            End If

            If rdoTotalReports.Checked = True Then
                'strRedirect = "Reports/TotalReports.aspx?User=" & strUser & "&UserID=" & strUserID & "&Disaster=" & strDisaster & "&Activity=" & strActivity & "&Applicant=" & strApplicant & "&DisasterID=" & strDisasterID & "&ActivityID=" & strActivityID & "&ApplicantID=" & strApplicantID & "&AllToDate=" & localAllToDate & "&StartDate=" & localStartDate & "&EndDate=" & localEndDate & "&ReportFormat=" & ddlReportFormat.SelectedValue
            ElseIf "" = "" Then

            Else

            End If

            Response.Redirect(strRedirect)
        End If
    End Sub

    Protected Sub ErrorChecks()
        Dim AllDates As String = ""

        If rdoAllDates.Checked = True Then
            AllDates = "All"

            'We are checking all dates. Make sure date valus are blank.
            txtStartDate.Text = ""
            txtEndDate.Text = ""
        Else
            AllDates = "Distinct"
        End If

        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        'Adding the appropriate errors to the error string.

        'Checking to see if the date is in correct format.
        If AllDates <> "All" Then
            If IsDate(txtStartDate.Text) = False Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must enter an appropriate Start Date. <br />")
                globalHasErrors = True
            End If

            If IsDate(txtEndDate.Text) = False Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must enter an appropriate End Date. <br />")
                globalHasErrors = True
            End If
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label.
        lblMessage.Text = strError.ToString
    End Sub

    Protected Sub rdoAllDates_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoAllDates.CheckedChanged
        pnlShowDates.Visible = False

        txtEndDate.Text = ""
        txtStartDate.Text = ""
    End Sub

    Protected Sub rdoPickDates_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoPickDates.CheckedChanged
        pnlShowDates.Visible = True
    End Sub

    'Protected Sub rdoWorksheetCountByCounty_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoWorksheetCountByCounty.CheckedChanged
    '    If rdoIncidentWorksheet.Checked = True Then
    '        lbxIncidentType.Enabled = True
    '    Else
    '        lbxIncidentType.Enabled = False
    '    End If

    '    If rdoWorksheetCountByCounty.Checked = True Then
    '        lbxCounty.Enabled = True
    '    Else
    '        lbxCounty.Enabled = False
    '    End If
    'End Sub

    Protected Sub rdoIncidentWorksheet_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoIncidentWorksheet.CheckedChanged
        If rdoIncidentWorksheet.Checked = True Then
            lbxIncidentType.Enabled = True
            lbxCounty.Enabled = False
            pnlSummation.Visible = False
        Else
            lbxIncidentType.Enabled = False
        End If

        'If rdoWorksheetCountByCounty.Checked = True Then
        '    lbxCounty.Enabled = True
        'Else
        '    lbxCounty.Enabled = False
        'End If
    End Sub

    Protected Sub rdoWorksheetCounty_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoWorksheetCounty.CheckedChanged
        lbxIncidentType.Enabled = True
        lbxCounty.Enabled = True
        pnlSummation.Visible = True
    End Sub
End Class