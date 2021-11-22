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

Partial Class DailyIncidentReport
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            txtDate.Text = Date.Today.ToString("MM/dd/yyyy")

            'Clean up reports.
            HelpFunction.CleanupReportDirectory()

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objCmd = New SqlCommand("spSelectAgencyAbbreviations", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            DBConStringHelper.PrepareConnection(objConn)
            ddlAgency.DataSource = objCmd.ExecuteReader()
            ddlAgency.DataBind()
            DBConStringHelper.FinalizeConnection(objConn)
            objCmd = Nothing
            ddlAgency.Items.Insert(0, New ListItem("Select An Agency", "0"))
            ddlAgency.Items(0).Selected = True
        End If
    End Sub

    Protected Sub rdoDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoDate.CheckedChanged
        pnlShowDates.Visible = False
        pnlShowDate.Visible = True
    End Sub

    Protected Sub rdoPickDates_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoPickDates.CheckedChanged
        pnlShowDates.Visible = True
        pnlShowDate.Visible = False
    End Sub

    Protected Sub rdoAllDates_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoAllDates.CheckedChanged
        pnlShowDates.Visible = False
        pnlShowDate.Visible = False
    End Sub

    Protected Sub btnRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRunReport.Click
        'Code Below Will Show All Values that are being passed to the Selected Report Page.
        'Response.Write(ddlCounty.SelectedItem)
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

        ErrorChecks()

        Dim strRedirect As String = ""
        Dim localAllToDate As String = ""
        Dim localDate As String = ""
        Dim localStartDate As String = ""
        Dim localEndDate As String = ""
        Dim localRemove As String = ""
        Dim localAgency As String = ""

        If globalHasErrors = True Then
            pnlMessage.Visible = True
            Exit Sub
        Else
            If rdoDate.Checked = True Then
                localAllToDate = "OneDate"
            End If
            If rdoPickDates.Checked = True Then
                localAllToDate = "TwoDate"
            End If
            If rdoAllDates.Checked = True Then
                localAllToDate = "Yes"
            End If
            If txtDate.Text = "" Then
                localDate = "No"
            Else
                localDate = txtDate.Text
            End If
            If txtStartDate.Text = "" Then
                localStartDate = "No"
            Else
                localStartDate = txtStartDate.Text
            End If
            If txtEndDate.Text = "" Then
                localEndDate = "No"
            Else
                localEndDate = txtEndDate.Text
            End If
            If cbRemove.Checked = True Then
                localRemove = "Yes"
            Else
                localRemove = "No"
            End If
            If ddlAgency.SelectedItem.Text = "Select An Agency" Then
                localAgency = "None"
            Else
                localAgency = ddlAgency.SelectedItem.Text
            End If

            pnlMessage.Visible = False

            'strRedirect = "Reports/TotalReports.aspx?User=" & strUser & "&UserID=" & strUserID & "&AllToDate=" & localAllToDate & "&StartDate=" & localStartDate & "&EndDate=" & localEndDate & "&ReportFormat=" & ddlReportFormat.SelectedValue
            strRedirect = "Reports/DailyIncidentReportDisplay.aspx?ReportFormat=" & ddlReportFormat.SelectedValue &  "&AllToDate=" & localAllToDate & "&StartDate=" & localStartDate & "&EndDate=" & localEndDate & "&Date=" & localDate & "&ReportType=" & ddlReportType.SelectedValue & "&Remove=" & localRemove & "&Agency=" & localAgency

            Response.Redirect(strRedirect)
        End If
    End Sub

    'Error checks.
    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        'Adding the appropriate errors to the error string.
        If rdoDate.Checked = True Then
            If txtDate.Text = "" Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Date. <br />")
                globalHasErrors = True
            End If
            If txtDate.Text <> "" Then
                If IsDate(txtDate.Text) = False Then
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a valid Date. <br />")
                    globalHasErrors = True
                End If
            End If
        End If
        If rdoPickDates.Checked = True Then
            If txtStartDate.Text = "" Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Start Date. <br />")
                globalHasErrors = True
            End If
            If txtStartDate.Text <> "" Then
                If IsDate(txtStartDate.Text) = False Then
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a valid Start Date. <br />")
                    globalHasErrors = True
                End If
            End If
            If txtEndDate.Text = "" Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide an End Date. <br />")
                globalHasErrors = True
            End If
            If txtEndDate.Text <> "" Then
                If IsDate(txtEndDate.Text) = False Then
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide an valid End Date. <br />")
                    globalHasErrors = True
                End If
            End If
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label.
        lblMessage.Text = strError.ToString
    End Sub
End Class