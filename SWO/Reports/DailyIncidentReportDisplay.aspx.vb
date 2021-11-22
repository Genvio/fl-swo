
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
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
Imports System.Diagnostics


Partial Class Reports_DailyIncidentReportDisplay
    Inherits System.Web.UI.Page

    'Help Functions from our App_Code.
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    'For Connecting to the database.
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet
    Public objDS2 As New System.Data.DataSet
    Public objDS3 As New System.Data.DataSet
    Public objDS4 As New System.Data.DataSet
    Public objDS5 As New System.Data.DataSet

    Public objConn2 As New System.Data.SqlClient.SqlConnection
    Public objCmd2 As System.Data.SqlClient.SqlCommand
    Public objDR2 As System.Data.SqlClient.SqlDataReader
    Public objDA2 As System.Data.SqlClient.SqlDataAdapter

    Public dt As New System.Data.DataTable

    Dim globalHasErrors As Boolean = False

    Dim strStartDate As String
    Dim strEndDate As String
    Dim strDate As String
    Dim strAllToDate As String
    Dim strUserID As String
    Dim strUser As String
    Dim strReportType As String
    Dim strReportFormat As String
    Dim strRemove As String
    Dim strAgency As String

    Dim strOutput As New StringBuilder("")

    'The name of the html file.
    Dim strOutputFileName As String

    'The path to the file.
    Dim strUrlString As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strStartDate = Request.QueryString("StartDate")
        strEndDate = Request.QueryString("EndDate")
        strDate = Request.QueryString("Date")
        strAllToDate = Request.QueryString("AllToDate")
        strUserID = Request.QueryString("UserID")
        strUser = Request.QueryString("User")
        strReportType = Request.QueryString("ReportType")
        strReportFormat = Request.QueryString("ReportFormat")
        strRemove = Request.QueryString("Remove")
        strAgency = Request.QueryString("Agency")

        'Response.Write(strReportFormat)
        'Response.Write("<br>")
        'Response.Write(strStartDate)
        'Response.Write("<br>")
        'Response.Write(strUserID)
        'Response.Write("<br>")
        'Response.End()

        strOutputFileName = HelpFunction.RandomStringGenerator(6)
        strUrlString = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FilePath").ToString) & "\Reports\ReportOutputFiles\" & strOutputFileName & ".htm"

        If Page.IsPostBack = False Then
            Select Case strReportFormat
                Case "HTML"
                    ExportToHTML()
                Case "Excel"
                    ExportToExcel()
                Case "Word"
                    ExportToWord()
                Case "PDF"
                    ExportToPDF()
                Case "Mobile"
                    ExportToMobile()
                Case "GovDelivery"
                    ExportToGovDelivery()
                Case Else
                    'Do Nothing.
            End Select
        End If
    End Sub

    Public Sub BuildGridView()
        If strRemove = "No" Then
            'Build the report.
            '---------------------------------------------------------------------------------------------
            'Make sure there is data and if so write out the body info.
            '---------------------------------------------------------------------------------------------
            strOutput.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
            strOutput.Append("<html xmlns='http://www.w3.org/1999/xhtml'>")
            strOutput.Append("<head>")
            strOutput.Append("<title>SERT :: SWO :: Daily Incident Report</title>")
            strOutput.Append("</head>")
            strOutput.Append("<body>")

            'For each item in the table write out a report.
            '---------------------------------------------------------------------------------------------
            'strOutput.Append("<table>")
            'strOutput.Append("    <tr>")
            'strOutput.Append("        <td  width='100%' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            'strOutput.Append("            <b>Road Closure or DOT Issue</b>")
            'strOutput.Append("        </td>")
            'strOutput.Append("    </tr>")
            'strOutput.Append("</table>")
            '---------------------------------------------------------------------------------------------

            'Connect and build the datagrid.
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Open the connection.
            DBConStringHelper.PrepareConnection(objConn)

            objCmd = New SqlCommand("[spFilterDailyIncidentReport]", objConn)

            If strAllToDate = "Yes" Then
                objCmd.Parameters.AddWithValue("@StartDate", "")
                objCmd.Parameters.AddWithValue("@EndDate", "")
                objCmd.Parameters.AddWithValue("@Date", "")
            ElseIf strAllToDate = "OneDate" Then
                objCmd.Parameters.AddWithValue("@StartDate", "")
                objCmd.Parameters.AddWithValue("@EndDate", "")
                objCmd.Parameters.AddWithValue("@Date", strDate)
            ElseIf strAllToDate = "TwoDate" Then
                objCmd.Parameters.AddWithValue("@StartDate", strStartDate)
                objCmd.Parameters.AddWithValue("@EndDate", strEndDate)
                objCmd.Parameters.AddWithValue("@Date", "")
            Else
                'Do nothing.
            End If

            If strReportType = "All" Then objCmd.Parameters.AddWithValue("@Type", "")
            If strReportType = "Open" Then objCmd.Parameters.AddWithValue("@Type", "1")
            If strReportType = "Closed" Then objCmd.Parameters.AddWithValue("@Type", "2")
            If strReportType = "Assigned" Then objCmd.Parameters.AddWithValue("@Type", "5")
            If strReportType = "Pending" Then objCmd.Parameters.AddWithValue("@Type", "3")
            If strReportType = "Dismissed" Then objCmd.Parameters.AddWithValue("@Type", "4")

            objCmd.Parameters.AddWithValue("@Agency", strAgency)
            '---------------------------------------------------------------------------------------------
            'Response.Write(PublicFunctions.GetSqlParameters(objCmd.Parameters, "spFilterAllCompaniesByZipCodeForReport"))
            'Response.End()

            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            Dim bgcolor As String
            Dim intcounter As Integer = 0
            Dim intReportCounter As Integer = 0
            Dim strHoldTopic As String = ""
            Dim strHref As String = ""
            Dim strHrefClose As String = ""
            Dim totReports As Integer = 0

            'strOutput.Append("")
            'build the report header
            '---------------------------------------------------------------------------------------------

            strOutput.Append("<table width='100%' cellspacing='0' border='0' style='background-color:#d4d4d4'>")
            strOutput.Append("  <tr>")
            strOutput.Append("      <td colspan='7' align='Center'>")
            strOutput.Append("          <b><font size='+1'>FLORIDA DIVISION OF EMERGENCY MANAGEMENT</font></b>")
            strOutput.Append("      </td>")
            strOutput.Append("  </tr>")
            strOutput.Append("  <tr>")
            strOutput.Append("      <td colspan='7' align='Center'>")
            strOutput.Append("          <b><font size='+1'>STATE WATCH OFFICE</font></b>")
            strOutput.Append("      </td>")
            strOutput.Append("  </tr>")
            'strOutput.Append("  <tr style='background-color: eeeeef'>")
            'strOutput.Append("      <td colspan='7' align='Center'>" & Now() & "")
            'strOutput.Append("      </td>")
            'strOutput.Append("")
            'strOutput.Append("</tr>")
            strOutput.Append("  <tr>")
            strOutput.Append("      <td colspan='7' align='Center'>")
            strOutput.Append("          &nbsp;")
            strOutput.Append("      </td>")
            strOutput.Append("  </tr>")

            If strAllToDate = "Yes" Then
                strOutput.Append("<tr style='background-color: #d4d4d4'><td colspan='7' align='Center' ><b><font size='+1'>DAILY INCIDENT REPORT FOR ALL TO DATE</font></b></td></tr>")
            Else
                If strStartDate = "" Then
                    strStartDate = "All"
                End If
                If strEndDate = "" Then
                    strEndDate = "All"
                End If

                'strOutput.Append("<tr style='background-color: #d4d4d4'><td colspan='7' align='Center'>From " & strStartDate & " To " & strEndDate & "</td></tr>")
                strOutput.Append("<tr style='background-color: #d4d4d4'><td colspan='7' align='Center'><b><font size='+1'>DAILY INCIDENT REPORT</font></b></td></tr>")

                If strStartDate = "All" Then
                    strStartDate = ""
                End If
                If strEndDate = "All" Then
                    strEndDate = ""
                End If
            End If

            strOutput.Append("</table>")

            If objDR.Read() Then
                'There are records.
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read
                    'Loop through and write out the report..
                    '---------------------------------------------------------------------------------------------
                    If intcounter Mod 2 = 0 Then
                        bgcolor = "eeeeef"
                    Else
                        bgcolor = "f7f7f7"
                    End If

                    Dim localTime As String = ""
                    Dim ReportedToSWOTime As String = ""
                    Dim ReportedToSWOTime2 As String = ""
                    Dim ReportedToSWODate As String = ""
                    Dim localTime2 As String = ""
                    Dim IncidentOccurredTime As String = ""
                    Dim IncidentOccurredTime2 As String = ""
                    Dim IncidentOccurredDate As String = ""
                    Dim WorkSheets As String = ""
                    Dim InitialReportsUpdates As String = ""
                    Dim strStatus As String

                    localTime = CStr(HelpFunction.Convertdbnulls(objDR("ReportedToSWOTime")))
                    ReportedToSWODate = HelpFunction.Convertdbnulls(objDR("ReportedToSWODate"))
                    localTime2 = CStr(HelpFunction.Convertdbnulls(objDR("IncidentOccurredTime")))
                    IncidentOccurredDate = HelpFunction.Convertdbnulls(objDR("IncidentOccurredDate"))
                    strStatus = HelpFunction.Convertdbnulls(objDR("IncidentStatus"))
                    IncidentOccurredTime = Left(localTime2, 2)
                    IncidentOccurredTime2 = Right(localTime2, 2)

                    ReportedToSWOTime = Left(localTime, 2)
                    ReportedToSWOTime2 = Right(localTime, 2)

                    'Grabbing Worksheets.
                    '---------------------------------------------------------------------------------------------
                    objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                    objConn2.Open()
                    objCmd2 = New SqlCommand("spSelectIncidentIncidentTypeByIncidentID", objConn2)
                    objCmd2.CommandType = CommandType.StoredProcedure
                    objCmd2.Parameters.AddWithValue("@IncidentID", objDR.Item("IncidentID"))

                    objDR2 = objCmd2.ExecuteReader

                    If objDR2.Read() Then
                        'There are records.
                        objDR2.Close()
                        objDR2 = objCmd2.ExecuteReader()

                        While objDR2.Read
                            If Not WorkSheets.Contains(CStr(objDR2.Item("IncidentType"))) Then
                                WorkSheets = WorkSheets & CStr(objDR2.Item("IncidentType")) & ", "
                            End If
                        End While
                    Else
                        WorkSheets = "No Worksheets added at this time.  "
                    End If

                    objDR2.Close()
                    objCmd2.Dispose()
                    objCmd2 = Nothing
                    objConn2.Close()

                    If WorkSheets <> "" Then
                        WorkSheets = WorkSheets.Remove(WorkSheets.Length - 2, 2)
                    End If
                    '---------------------------------------------------------------------------------------------

                    'Grabbing Initial Reports.
                    '---------------------------------------------------------------------------------------------
                    objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                    objConn2.Open()
                    objCmd2 = New SqlCommand("spSelectInitialReportByIncidentID", objConn2)
                    objCmd2.CommandType = CommandType.StoredProcedure
                    objCmd2.Parameters.AddWithValue("@IncidentID", objDR.Item("IncidentID"))

                    objDR2 = objCmd2.ExecuteReader

                    If objDR2.Read() Then
                        'There are records.
                        objDR2.Close()
                        objDR2 = objCmd2.ExecuteReader()

                        InitialReportsUpdates = "<b>Initial Reports</b>:  "

                        While objDR2.Read
                            InitialReportsUpdates = InitialReportsUpdates & CStr(objDR2.Item("InitialReport")) & ", "
                        End While
                    End If

                    objDR2.Close()
                    objCmd2.Dispose()
                    objCmd2 = Nothing
                    objConn2.Close()
                    '---------------------------------------------------------------------------------------------

                    'Grabbing Report Updates.
                    '---------------------------------------------------------------------------------------------
                    objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                    objConn2.Open()
                    objCmd2 = New SqlCommand("spSelectUpdateReportByIncidentID", objConn2)
                    objCmd2.CommandType = CommandType.StoredProcedure
                    objCmd2.Parameters.AddWithValue("@IncidentID", objDR.Item("IncidentID"))

                    objDR2 = objCmd2.ExecuteReader

                    If InitialReportsUpdates <> "" AndAlso InitialReportsUpdates.EndsWith(", ") Then
                        InitialReportsUpdates = InitialReportsUpdates.Remove(InitialReportsUpdates.Length - 2, 2)
                    End If

                    If objDR2.Read() Then
                        'There are records.
                        objDR2.Close()
                        objDR2 = objCmd2.ExecuteReader()

                        InitialReportsUpdates = InitialReportsUpdates & " <b>Report Updates</b>: "

                        While objDR2.Read
                            InitialReportsUpdates = InitialReportsUpdates & CStr(objDR2.Item("UpdateReport")) & ", "
                        End While
                    End If

                    objDR2.Close()
                    objCmd2.Dispose()
                    objCmd2 = Nothing
                    objConn2.Close()

                    If InitialReportsUpdates <> "" AndAlso InitialReportsUpdates.EndsWith(", ") Then
                        InitialReportsUpdates = InitialReportsUpdates.Remove(InitialReportsUpdates.Length - 2, 2)
                    End If

                    ''---------------------------------------------------------------------------------------------
                    'If strHoldIncidentID <> objDR.Item("IncidentID") Then
                    '    If intcounter > 0 Then
                    '        strOutput.Append("<tr style='background-color:d4d4d4' ><td colspan='8' align=right><b>Incident Total: " & intReportCounter & "</b></td>")
                    '        strOutput.Append("</tr>")
                    '        intReportCounter = 0
                    '    End If

                    '    strOutput.Append("<tr style='background-color: " & bgcolor & "'>")
                    '    strOutput.Append("<td>" & objDR.Item("User") & "</td>")
                    'Else
                    '    strOutput.Append("<tr style='background-color:" & bgcolor & "'>")
                    '    strOutput.Append("<td>&nbsp;</td>")
                    'End If
                    ''---------------------------------------------------------------------------------------------

                    Dim StatewideCheck = CStr(objDR.Item("Statewide"))

                    strOutput.Append("<table width='100%' align='center' border='0' style='border-color:#000000; background-color:#000000;'>")
                    strOutput.Append("    <tr>")
                    strOutput.Append("        <td align='center' width='200px' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        " & objDR.Item("IncidentNumber") & "    ")
                    strOutput.Append("        </td>")
                    strOutput.Append("        <td colspan='3' align='left' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        <b>Incident Name</b>: " & objDR.Item("IncidentName") & "    ")
                    strOutput.Append("        </td>")
                    strOutput.Append("    </tr>")
                    strOutput.Append("    <tr>")
                    strOutput.Append("        <td align='center' width='200px' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        <b>Occurred</b>:    ")
                    strOutput.Append("        </td>")
                    strOutput.Append("        <td align='left' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("         " & IncidentOccurredDate & " &nbsp; " & IncidentOccurredTime & ":" & IncidentOccurredTime2 & " ET ")
                    strOutput.Append("        </td>")
                    strOutput.Append("        <td align='right' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        <b>Reported to SWO</b>: ")
                    strOutput.Append("        </td>")
                    strOutput.Append("        <td align='left' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        " & ReportedToSWODate & " &nbsp; " & ReportedToSWOTime & ":" & ReportedToSWOTime2 & " ET ")
                    strOutput.Append("        </td>")
                    strOutput.Append("    </tr>")

                    If StatewideCheck = "No" Then
                        strOutput.Append("    <tr>")
                        strOutput.Append("        <td align='center' width='200px' style='background-color:#f7f7f7; border-color:#000000' >")
                        strOutput.Append("        <b>Affecting</b>:    ")
                        strOutput.Append("        </td>")
                        strOutput.Append("        <td colspan='3' align='left' style='background-color:#f7f7f7; border-color:#000000' >")
                        strOutput.Append("        " & objDR.Item("AddedCounty") & "    ")
                        strOutput.Append("        </td>")
                        strOutput.Append("    </tr>")
                    Else
                        strOutput.Append("    <tr>")
                        strOutput.Append("        <td align='center' width='200px' style='background-color:#f7f7f7; border-color:#000000' >")
                        strOutput.Append("        <b>Affecting</b>:    ")
                        strOutput.Append("        </td>")
                        strOutput.Append("        <td colspan='3' align='left' style='background-color:#f7f7f7; border-color:#000000' >")
                        strOutput.Append("        " & objDR.Item("Statewide") & "    ")
                        strOutput.Append("        </td>")
                        strOutput.Append("    </tr>")
                    End If

                    strOutput.Append("    <tr>")
                    strOutput.Append("        <td align='center' width='200px' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        <b>Involving</b>:    ")
                    strOutput.Append("        </td>")
                    strOutput.Append("        <td colspan='3' align='left' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        " & WorkSheets & "    ")
                    strOutput.Append("        </td>")
                    strOutput.Append("    </tr>")
                    strOutput.Append("    <tr>")
                    strOutput.Append("        <td colspan='4' align='left' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        " & InitialReportsUpdates & "    ")
                    strOutput.Append("        </td>")
                    strOutput.Append("    </tr>")
                    strOutput.Append("    <tr>")
                    strOutput.Append("        <td align='center' colspan='4' width='200px' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        <b>Status</b>: " & strStatus & " ")
                    strOutput.Append("        </td>")
                    strOutput.Append("    </tr>")
                    strOutput.Append("</table>")

                    strOutput.Append("<table width='100%' cellspacing='0' border='0' style='background-color:#d4d4d4'>")
                    strOutput.Append("  <tr>")
                    strOutput.Append("      <td align='Center'>")
                    strOutput.Append("          &nbsp;")
                    strOutput.Append("      </td>")
                    strOutput.Append("</table>")

                    'Increment report totals.
                    '---------------------------------------------------------------------------------------------
                    'strHoldIncidentID = objDR.Item("IncidentID")
                    intcounter = intcounter + 1
                    intReportCounter = intReportCounter + 1
                End While

                'Write out the totals.
                '---------------------------------------------------------------------------------------------
                strOutput.Append("<table width='100%' cellspacing='0' border='0' style='background-color:#d4d4d4'>")
                strOutput.Append("<tr><td colspan='8' align='right'><b>Total Incidents: " & intcounter & "</b></td>")
                strOutput.Append("</tr></table>")
            Else
                'There are no records.
                strOutput.Append("<table width='100%'><tr><td colspan='8' align='center'>No Records</td><tr></table>")
            End If

            'Close the table
            'strOutput.Append("</table>")

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

            strOutput.Append("</body>")
            strOutput.Append("</html>")
        Else
            'Build the report.
            '---------------------------------------------------------------------------------------------
            'Make sure there is data and if so write out the body info.
            '---------------------------------------------------------------------------------------------
            strOutput.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
            strOutput.Append("<html xmlns='http://www.w3.org/1999/xhtml'>")
            strOutput.Append("<head>")
            strOutput.Append("<title>SERT :: SWO :: Daily Incident Report</title>")
            strOutput.Append("</head>")
            strOutput.Append("<body>")

            'For each item in the table write out a report.
            '---------------------------------------------------------------------------------------------
            'strOutput.Append("<table>")
            'strOutput.Append("    <tr>")
            'strOutput.Append("        <td  width='100%' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            'strOutput.Append("            <b>Road Closure or DOT Issue</b>")
            'strOutput.Append("        </td>")
            'strOutput.Append("    </tr>")
            'strOutput.Append("</table>")
            '---------------------------------------------------------------------------------------------

            'Connect and build the datagrid.
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Open the connection.
            DBConStringHelper.PrepareConnection(objConn)

            objCmd = New SqlCommand("[spFilterDailyIncidentReport2]", objConn)

            If strAllToDate = "Yes" Then
                objCmd.Parameters.AddWithValue("@StartDate", "")
                objCmd.Parameters.AddWithValue("@EndDate", "")
                objCmd.Parameters.AddWithValue("@Date", "")
            ElseIf strAllToDate = "OneDate" Then
                objCmd.Parameters.AddWithValue("@StartDate", "")
                objCmd.Parameters.AddWithValue("@EndDate", "")
                objCmd.Parameters.AddWithValue("@Date", strDate)
            ElseIf strAllToDate = "TwoDate" Then
                objCmd.Parameters.AddWithValue("@StartDate", strStartDate)
                objCmd.Parameters.AddWithValue("@EndDate", strEndDate)
                objCmd.Parameters.AddWithValue("@Date", "")
            Else
                'Do nothing.
            End If

            If strReportType = "All" Then objCmd.Parameters.AddWithValue("@Type", "")
            If strReportType = "Open" Then objCmd.Parameters.AddWithValue("@Type", "1")
            If strReportType = "Closed" Then objCmd.Parameters.AddWithValue("@Type", "2")
            If strReportType = "Assigned" Then objCmd.Parameters.AddWithValue("@Type", "5")
            If strReportType = "Pending" Then objCmd.Parameters.AddWithValue("@Type", "3")
            If strReportType = "Dismissed" Then objCmd.Parameters.AddWithValue("@Type", "4")

            objCmd.Parameters.AddWithValue("@Agency", strAgency)
            '---------------------------------------------------------------------------------------------
            'Response.Write(PublicFunctions.GetSqlParameters(objCmd.Parameters, "spFilterAllCompaniesByZipCodeForReport"))
            'Response.End()

            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            Dim bgcolor As String
            Dim intcounter As Integer = 0
            Dim intReportCounter As Integer = 0
            Dim strHoldTopic As String = ""
            Dim strHref As String = ""
            Dim strHrefClose As String = ""
            Dim totReports As Integer = 0

            'strOutput.Append("")
            'build the report header
            '---------------------------------------------------------------------------------------------

            strOutput.Append("<table width='100%' cellspacing='0' border='0' style='background-color:#d4d4d4'>")
            strOutput.Append("  <tr>")
            strOutput.Append("      <td colspan='7' align='Center'>")
            strOutput.Append("          <b><font size='+1'>FLORIDA DIVISION OF EMERGENCY MANAGEMENT</font></b>")
            strOutput.Append("      </td>")
            strOutput.Append("  </tr>")
            strOutput.Append("  <tr>")
            strOutput.Append("      <td colspan='7' align='Center'>")
            strOutput.Append("          <b><font size='+1'>STATE WATCH OFFICE</font></b>")
            strOutput.Append("      </td>")
            strOutput.Append("  </tr>")
            'strOutput.Append("  <tr style='background-color: eeeeef'>")
            'strOutput.Append("      <td colspan='7' align='Center'>" & Now() & "")
            'strOutput.Append("      </td>")
            'strOutput.Append("")
            'strOutput.Append("</tr>")
            strOutput.Append("  <tr>")
            strOutput.Append("      <td colspan='7' align='Center'>")
            strOutput.Append("          &nbsp;")
            strOutput.Append("      </td>")
            strOutput.Append("  </tr>")

            If strAllToDate = "Yes" Then
                strOutput.Append("<tr style='background-color: #d4d4d4'><td colspan='7' align='Center' ><b><font size='+1'>DAILY INCIDENT REPORT FOR ALL TO DATE</font></b></td></tr>")
            Else
                If strStartDate = "" Then
                    strStartDate = "All"
                End If
                If strEndDate = "" Then
                    strEndDate = "All"
                End If

                'strOutput.Append("<tr style='background-color: #d4d4d4'><td colspan='7' align='Center'>From " & strStartDate & " To " & strEndDate & "</td></tr>")
                strOutput.Append("<tr style='background-color: #d4d4d4'><td colspan='7' align='Center'><b><font size='+1'>DAILY INCIDENT REPORT</font></b></td></tr>")

                If strStartDate = "All" Then
                    strStartDate = ""
                End If
                If strEndDate = "All" Then
                    strEndDate = ""
                End If
            End If

            strOutput.Append("</table>")

            If objDR.Read() Then
                'There are records.
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read
                    'Loop through and write out the report..
                    '---------------------------------------------------------------------------------------------
                    If intcounter Mod 2 = 0 Then
                        bgcolor = "eeeeef"
                    Else
                        bgcolor = "f7f7f7"
                    End If

                    Dim localTime As String = ""
                    Dim ReportedToSWOTime As String = ""
                    Dim ReportedToSWOTime2 As String = ""
                    Dim ReportedToSWODate As String = ""
                    Dim localTime2 As String = ""
                    Dim IncidentOccurredTime As String = ""
                    Dim IncidentOccurredTime2 As String = ""
                    Dim IncidentOccurredDate As String = ""
                    Dim WorkSheets As String = ""
                    Dim InitialReportsUpdates As String = ""
                    Dim strStatus As String

                    localTime = CStr(HelpFunction.Convertdbnulls(objDR("ReportedToSWOTime")))
                    ReportedToSWODate = HelpFunction.Convertdbnulls(objDR("ReportedToSWODate"))
                    localTime2 = CStr(HelpFunction.Convertdbnulls(objDR("IncidentOccurredTime")))
                    IncidentOccurredDate = HelpFunction.Convertdbnulls(objDR("IncidentOccurredDate"))
                    strStatus = HelpFunction.Convertdbnulls(objDR("IncidentStatus"))
                    IncidentOccurredTime = Left(localTime2, 2)
                    IncidentOccurredTime2 = Right(localTime2, 2)

                    ReportedToSWOTime = Left(localTime, 2)
                    ReportedToSWOTime2 = Right(localTime, 2)

                    'Grabbing Worksheets.
                    '---------------------------------------------------------------------------------------------
                    objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                    objConn2.Open()
                    objCmd2 = New SqlCommand("spSelectIncidentIncidentTypeByIncidentID", objConn2)
                    objCmd2.CommandType = CommandType.StoredProcedure
                    objCmd2.Parameters.AddWithValue("@IncidentID", objDR.Item("IncidentID"))

                    objDR2 = objCmd2.ExecuteReader

                    If objDR2.Read() Then
                        'There are records.
                        objDR2.Close()
                        objDR2 = objCmd2.ExecuteReader()

                        While objDR2.Read
                            If Not WorkSheets.Contains(CStr(objDR2.Item("IncidentType"))) Then
                                WorkSheets = WorkSheets & CStr(objDR2.Item("IncidentType")) & ", "
                            End If
                        End While
                    Else
                        WorkSheets = "No Worksheets added at this time.  "
                    End If

                    objDR2.Close()
                    objCmd2.Dispose()
                    objCmd2 = Nothing
                    objConn2.Close()

                    If WorkSheets <> "" Then
                        WorkSheets = WorkSheets.Remove(WorkSheets.Length - 2, 2)
                    End If
                    '---------------------------------------------------------------------------------------------

                    'Grabbing Initial Reports.
                    '---------------------------------------------------------------------------------------------
                    objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                    objConn2.Open()
                    objCmd2 = New SqlCommand("spSelectInitialReportByIncidentID", objConn2)
                    objCmd2.CommandType = CommandType.StoredProcedure
                    objCmd2.Parameters.AddWithValue("@IncidentID", objDR.Item("IncidentID"))

                    objDR2 = objCmd2.ExecuteReader

                    If objDR2.Read() Then
                        'There are records.
                        objDR2.Close()
                        objDR2 = objCmd2.ExecuteReader()

                        InitialReportsUpdates = "<b>Initial Reports</b>:  "

                        While objDR2.Read
                            InitialReportsUpdates = InitialReportsUpdates & CStr(objDR2.Item("InitialReport")) & ", "
                        End While
                    End If

                    objDR2.Close()
                    objCmd2.Dispose()
                    objCmd2 = Nothing
                    objConn2.Close()
                    '---------------------------------------------------------------------------------------------

                    'Grabbing Report Updates.
                    '---------------------------------------------------------------------------------------------
                    objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                    objConn2.Open()
                    objCmd2 = New SqlCommand("spSelectUpdateReportByIncidentID", objConn2)
                    objCmd2.CommandType = CommandType.StoredProcedure
                    objCmd2.Parameters.AddWithValue("@IncidentID", objDR.Item("IncidentID"))

                    objDR2 = objCmd2.ExecuteReader

                    If InitialReportsUpdates <> "" AndAlso InitialReportsUpdates.EndsWith(", ") Then
                        InitialReportsUpdates = InitialReportsUpdates.Remove(InitialReportsUpdates.Length - 2, 2)
                    End If

                    If objDR2.Read() Then
                        'There are records.
                        objDR2.Close()
                        objDR2 = objCmd2.ExecuteReader()

                        InitialReportsUpdates = InitialReportsUpdates & " <b>Report Updates</b>: "

                        While objDR2.Read
                            InitialReportsUpdates = InitialReportsUpdates & CStr(objDR2.Item("UpdateReport")) & ", "
                        End While
                    End If

                    objDR2.Close()
                    objCmd2.Dispose()
                    objCmd2 = Nothing
                    objConn2.Close()

                    If InitialReportsUpdates <> "" AndAlso InitialReportsUpdates.EndsWith(", ") Then
                        InitialReportsUpdates = InitialReportsUpdates.Remove(InitialReportsUpdates.Length - 2, 2)
                    End If

                    ''---------------------------------------------------------------------------------------------
                    'If strHoldIncidentID <> objDR.Item("IncidentID") Then
                    '    If intcounter > 0 Then
                    '        strOutput.Append("<tr style='background-color:d4d4d4' ><td colspan='8' align=right><b>Incident Total: " & intReportCounter & "</b></td>")
                    '        strOutput.Append("</tr>")
                    '        intReportCounter = 0
                    '    End If

                    '    strOutput.Append("<tr style='background-color: " & bgcolor & "'>")
                    '    strOutput.Append("<td>" & objDR.Item("User") & "</td>")
                    'Else
                    '    strOutput.Append("<tr style='background-color:" & bgcolor & "'>")
                    '    strOutput.Append("<td>&nbsp;</td>")
                    'End If
                    ''---------------------------------------------------------------------------------------------

                    Dim StatewideCheck = CStr(objDR.Item("Statewide"))

                    strOutput.Append("<table width='100%' align='center' border='0' style='border-color:#000000; background-color:#000000;'>")
                    strOutput.Append("    <tr>")
                    strOutput.Append("        <td align='center' width='200px' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        " & objDR.Item("IncidentNumber") & "    ")
                    strOutput.Append("        </td>")
                    strOutput.Append("        <td colspan='3' align='left' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        <b>Incident Name</b>: " & objDR.Item("IncidentName") & "    ")
                    strOutput.Append("        </td>")
                    strOutput.Append("    </tr>")
                    strOutput.Append("    <tr>")
                    strOutput.Append("        <td align='center' width='200px' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        <b>Occurred</b>:    ")
                    strOutput.Append("        </td>")
                    strOutput.Append("        <td align='left' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("         " & IncidentOccurredDate & " &nbsp; " & IncidentOccurredTime & ":" & IncidentOccurredTime2 & " ET ")
                    strOutput.Append("        </td>")
                    strOutput.Append("        <td align='right' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        <b>Reported to SWO</b>: ")
                    strOutput.Append("        </td>")
                    strOutput.Append("        <td align='left' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        " & ReportedToSWODate & " &nbsp; " & ReportedToSWOTime & ":" & ReportedToSWOTime2 & " ET ")
                    strOutput.Append("        </td>")
                    strOutput.Append("    </tr>")

                    If StatewideCheck = "No" Then
                        strOutput.Append("    <tr>")
                        strOutput.Append("        <td align='center' width='200px' style='background-color:#f7f7f7; border-color:#000000' >")
                        strOutput.Append("        <b>Affecting</b>:    ")
                        strOutput.Append("        </td>")
                        strOutput.Append("        <td colspan='3' align='left' style='background-color:#f7f7f7; border-color:#000000' >")
                        strOutput.Append("        " & objDR.Item("AddedCounty") & "    ")
                        strOutput.Append("        </td>")
                        strOutput.Append("    </tr>")
                    Else
                        strOutput.Append("    <tr>")
                        strOutput.Append("        <td align='center' width='200px' style='background-color:#f7f7f7; border-color:#000000' >")
                        strOutput.Append("        <b>Affecting</b>:    ")
                        strOutput.Append("        </td>")
                        strOutput.Append("        <td colspan='3' align='left' style='background-color:#f7f7f7; border-color:#000000' >")
                        strOutput.Append("        " & objDR.Item("Statewide") & "    ")
                        strOutput.Append("        </td>")
                        strOutput.Append("    </tr>")
                    End If

                    strOutput.Append("    <tr>")
                    strOutput.Append("        <td align='center' width='200px' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        <b>Involving</b>:    ")
                    strOutput.Append("        </td>")
                    strOutput.Append("        <td colspan='3' align='left' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        " & WorkSheets & "    ")
                    strOutput.Append("        </td>")
                    strOutput.Append("    </tr>")
                    strOutput.Append("    <tr>")
                    strOutput.Append("        <td colspan='4' align='left' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        " & InitialReportsUpdates & "    ")
                    strOutput.Append("        </td>")
                    strOutput.Append("    </tr>")
                    strOutput.Append("    <tr>")
                    strOutput.Append("        <td align='center' colspan='4' width='200px' style='background-color:#f7f7f7; border-color:#000000' >")
                    strOutput.Append("        <b>Status</b>: " & strStatus & " ")
                    strOutput.Append("        </td>")
                    strOutput.Append("    </tr>")
                    strOutput.Append("</table>")

                    strOutput.Append("<table width='100%' cellspacing='0' border='0' style='background-color:#d4d4d4'>")
                    strOutput.Append("  <tr>")
                    strOutput.Append("      <td align='Center'>")
                    strOutput.Append("          &nbsp;")
                    strOutput.Append("      </td>")
                    strOutput.Append("</table>")

                    'Increment report totals.
                    '---------------------------------------------------------------------------------------------
                    'strHoldIncidentID = objDR.Item("IncidentID")
                    intcounter = intcounter + 1
                    intReportCounter = intReportCounter + 1
                End While

                'Write out the totals.
                '---------------------------------------------------------------------------------------------
                strOutput.Append("<table width='100%' cellspacing='0' border='0' style='background-color:#d4d4d4'>")
                strOutput.Append("<tr><td colspan='8' align='right'><b>Total Incidents: " & intcounter & "</b></td>")
                strOutput.Append("</tr></table>")
            Else
                'There are no records.
                strOutput.Append("<table width='100%'><tr><td colspan='8' align='center'>No Records</td><tr></table>")
            End If

            'Close the table
            'strOutput.Append("</table>")

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

            strOutput.Append("</body>")
            strOutput.Append("</html>")
        End If
    End Sub

    'Export Subs.
    Sub ExportToHTML()
        BuildGridView()
        Response.Write(strOutput.ToString())
        Response.Flush()
        Response.SuppressContent = True
        HttpContext.Current.ApplicationInstance.CompleteRequest()
    End Sub

    Sub ExportToWord()
        '---------------------------------------------------------------------------------------------
        'Build the content for the dynamic Word document in HTML
        'along with some Office specific style properties. 
        '---------------------------------------------------------------------------------------------

        'strOutput.Append("<html xmlns:o='urn:schemas-microsoft-com:office:office' " & _
        '"xmlns:w='urn:schemas-microsoft-com:office:word'" & _
        '"xmlns='http://www.w3.org/TR/REC-html40'>" & _
        '"<head><title>Total Reports</title>")

        'strOutput.Append( _
        '           "<!--[if gte mso 9]>" & _
        '           "<xml>" & _
        '           "<w:WordDocument>" & _
        '           "<w:View>Print</w:View>" & _
        '           "<w:Zoom>90</w:Zoom>  " & _
        '           "</w:WordDocument>" & _
        '           "</xml>" & _
        '           "<![endif]-->")
        'strOutput.Append( _
        '           "<style>" & _
        '           "<!-- /* Style Definitions               */@page Section1{size:8.5in 11.0in;" & _
        '           "margin:1.0in 1.25in 1.0in " & _
        '           "1.25in;mso-header-margin:.5in; " & _
        '           "mso-footer-margin:.5in;    mso-paper-source:0;}" & _
        '           "div.Section1{page:Section1;}-->" & _
        '           "</style></head>")
        'strOutput.Append( _
        '            "<body lang=EN-US style='tab-interval:.5in'>" & _
        '            "<div class=Section1>")

        ''The Guts of Report Go Below.
        'BuildGridView()

        ''The Guts of Report Go Above.
        'strOutput.Append( _
        '            "</div></body></html>")

        ''Force this content to be downloaded as a Word document with the name of your choice.
        'Response.AppendHeader("Content-Type", "application/msword")
        'Response.AppendHeader("Content-disposition", _
        '"attachment; filename=Total Incidents By User.doc")
        'Response.Charset = ""

        '''Display the Word Document.
        ''If Not System.IO.File.Exists("C:\somefile.doc") = True Then
        ''    Dim file As System.IO.FileStream

        ''    file = System.IO.File.Create("C:\somefile.doc")
        ''    file.Close()
        ''End If

        ''System.IO.File.Copy("C:\foo\somefile.txt", "C:\bar\somefile.txt")
        ''System.IO.File.Move("C:\foo\somefile.txt", "C:\bar\somefile.txt")

        ''My.Computer.FileSystem.WriteAllText("C:\somefile.doc", strOutput.ToString(), True)

        ''Response.Write(strOutput)
    End Sub

    Sub ExportToExcel()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        DBConStringHelper.PrepareConnection(objConn) 'open the connection

        objCmd = New SqlCommand("spFilterDailyIncidentReport", objConn)
        'objCmd.Parameters.AddWithValue("@UserID", strUserID)

        If strAllToDate = "Yes" Then
            objCmd.Parameters.AddWithValue("@StartDate", "")
            objCmd.Parameters.AddWithValue("@EndDate", "")
            objCmd.Parameters.AddWithValue("@Date", "")
        ElseIf strAllToDate = "OneDate" Then
            objCmd.Parameters.AddWithValue("@StartDate", "")
            objCmd.Parameters.AddWithValue("@EndDate", "")
            objCmd.Parameters.AddWithValue("@Date", strDate)
        ElseIf strAllToDate = "TwoDate" Then
            objCmd.Parameters.AddWithValue("@StartDate", strStartDate)
            objCmd.Parameters.AddWithValue("@EndDate", strEndDate)
            objCmd.Parameters.AddWithValue("@Date", "")
        Else
            'Do nothing.
        End If

        If strReportType = "All" Then objCmd.Parameters.AddWithValue("@Type", "")
        If strReportType = "Open" Then objCmd.Parameters.AddWithValue("@Type", "1")
        If strReportType = "Closed" Then objCmd.Parameters.AddWithValue("@Type", "2")
        If strReportType = "Assigned" Then objCmd.Parameters.AddWithValue("@Type", "5")
        If strReportType = "Pending" Then objCmd.Parameters.AddWithValue("@Type", "3")
        If strReportType = "Dismissed" Then objCmd.Parameters.AddWithValue("@Type", "4")

        objCmd.Parameters.AddWithValue("@Agency", strAgency)


        objCmd.CommandType = CommandType.StoredProcedure

        'Send the results to a dataset.
        objDA = New System.Data.SqlClient.SqlDataAdapter
        objDA.SelectCommand = objCmd


        'Put the data into the datatable bind to repeater for report.
        pnlExcel.Visible = True
        objDA.Fill(dt)
        rptExcel.DataSource = dt
        rptExcel.DataBind()
        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()



        ''''' Old '''''
        '''''''''''''''
        'DataSetToExcel.Convert(objDS, Response)
        'objDS = Nothing

        'pnlExcel.Visible = True
        'Response.Write(localTotalReports)
        'Response.End()

        'myGridView.DataSource = objDS
        'myGridView.Visible = True
        'myGridView.DataBind()
        'objDS2.Tables(0).Rows(0).ItemArray(0) = objDS.Tables(0).Rows(0).ItemArray(0).ToString()
        'Response.Write(objDS.Tables(0).Rows(0).ItemArray(0).ToString())
        'Response.End()

        'DataSetToExcel.Convert(objDS2, Response)
        'objDS = Nothing
    End Sub

    Sub ExportToPDF()
        'Response.Write(Server.MapPath("StartFiles\"))
        'Response.End()

        'First we will Delete all Old Reports.
        'HelpFunction.CleanupReportDirectory()
        'HelpFunction.CleanupReportDirectory2()

        ''---------------------------------------------------------------------------------------------
        ''Build the content for the dynamic Word document in HTML  
        ''along with some Office specific style properties. 
        ''---------------------------------------------------------------------------------------------

        ''strOutput.Append("<html xmlns:o='urn:schemas-microsoft-com:office:office' " & _
        ''"xmlns:w='urn:schemas-microsoft-com:office:word'" & _
        ''"xmlns='http://www.w3.org/TR/REC-html40'>" & _
        ''"<head><title>Total Reports</title>")

        ''strOutput.Append( _
        ''           "<!--[if gte mso 9]>" & _
        ''           "<xml>" & _
        ''           "<w:WordDocument>" & _
        ''           "<w:View>Print</w:View>" & _
        ''           "<w:Zoom>90</w:Zoom>  " & _
        ''           "</w:WordDocument>" & _
        ''           "</xml>" & _
        ''           "<![endif]-->")
        ''strOutput.Append( _
        ''           "<style>" & _
        ''           "<!-- /* Style Definitions               */@page Section1{size:8.5in 11.0in;" & _
        ''           "margin:1.0in 1.25in 1.0in " & _
        ''           "1.25in;mso-header-margin:.5in; " & _
        ''           "mso-footer-margin:.5in;    mso-paper-source:0;}" & _
        ''           "div.Section1{page:Section1;}-->" & _
        ''           "</style></head>")
        ''strOutput.Append( _
        ''           "<body lang=EN-US style='tab-interval:.5in'>" & _
        ''           "<div class=Section1>")

        ''The Guts of Report Go Below.
        'BuildGridView()

        ''The Guts of Report Go Above.
        ''strOutput.Append( _
        ''            "</div></body></html>")

        'Dim localStartWordFile As String = HelpFunction.RandomStringGenerator(6)

        '''Force this content to be downloaded as a Word document with the name of your choice.
        ''Response.AppendHeader("Content-Type", "application/msword")
        ''Response.AppendHeader("Content-disposition", _
        ''"attachment; filename=Total Incident By User.doc")
        ''Response.Charset = ""
        ''strOutput. = ""

        '''Display the Word Document
        ''Response.Write(Server.MapPath("StartFiles\") & localStartWordFile & ".doc")
        ''Response.End()

        ''If Not System.IO.File.Exists("C:\somefile.doc") = True Then
        ''    Dim file As System.IO.FileStream

        ''    file = System.IO.File.Create("C:\somefile.doc")
        ''    file.Close()
        ''End If

        ''Response.Write(strOutput.ToString())
        ''Response.End()

        ''My.Computer.FileSystem.WriteAllText("C:\somefile.doc", strOutput.ToString(), True)

        ''If Not System.IO.File.Exists(Server.MapPath("StartFiles\") & localStartWordFile & ".doc") = True Then
        ''    Dim file As System.IO.FileStream

        ''    file = System.IO.File.Create(Server.MapPath("StartFiles\") & localStartWordFile & ".doc")
        ''    file.Close()
        ''End If

        'My.Computer.FileSystem.WriteAllText(Server.MapPath("StartFiles\") & localStartWordFile & ".doc", strOutput.ToString(), True)

        ''PDF
        'System.IO.File.Copy(Server.MapPath("StartFiles\") & localStartWordFile & ".doc", Server.MapPath("ReportOutputFiles\") & localStartWordFile & ".doc")

        ''Create a new Microsoft Word application object.
        'Dim word As New Microsoft.Office.Interop.Word.Application()

        ''C# doesn't have optional arguments so we'll need a dummy value.
        'Dim oMissing As Object = System.Reflection.Missing.Value

        ''Get list of Word files in specified directory.
        ''Response.Write(Server.MapPath("StartFiles\"))
        ''Response.End()

        'Dim dirInfo As New DirectoryInfo(Server.MapPath("StartFiles\"))
        'Dim wordFiles As FileInfo() = dirInfo.GetFiles("*.doc")

        'word.Visible = False
        'word.ScreenUpdating = False

        'For Each wordFile As FileInfo In wordFiles
        '    'Cast as Object for word Open method.
        '    Dim filename As [Object] = DirectCast(wordFile.FullName, [Object])

        '    'Use the dummy value as a placeholder for optional arguments.
        '    Dim doc As Document = word.Documents.Open(filename, oMissing, oMissing, oMissing, oMissing, oMissing, _
        '     oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, _
        '     oMissing, oMissing, oMissing, oMissing)
        '    doc.Activate()

        '    Dim outputFileName As Object = wordFile.FullName.Replace(".doc", ".pdf")
        '    Dim fileFormat As Object = WdSaveFormat.wdFormatPDF

        '    'Save document into PDF Format.
        '    doc.SaveAs(outputFileName, fileFormat, oMissing, oMissing, oMissing, oMissing, _
        '     oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, _
        '     oMissing, oMissing, oMissing, oMissing)

        '    '---------------------------------------------------------------------------------------------
        '    'Close the Word document, but leave the Word application open. 
        '    'Doc has to be cast to type _Document so that it will find the 
        '    'correct Close method.
        '    '---------------------------------------------------------------------------------------------

        '    Dim saveChanges As Object = WdSaveOptions.wdDoNotSaveChanges

        '    DirectCast(doc, _Document).Close(saveChanges, oMissing, oMissing)
        '    doc = Nothing
        'Next

        ''---------------------------------------------------------------------------------------------
        ''Word has to be cast to type _Application so that it will find
        ''the correct Quit method.
        ''---------------------------------------------------------------------------------------------

        'DirectCast(word, _Application).Quit(oMissing, oMissing, oMissing)
        'word = Nothing

        'Response.Redirect("StartFiles\" & localStartWordFile & ".pdf")
    End Sub

    Sub ExportToMobile()
        'Build the report.
        '---------------------------------------------------------------------------------------------
        'Make sure there is data and if so write out the body info.
        strOutput.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
        strOutput.Append("<html xmlns='http://www.w3.org/1999/xhtml'>")
        strOutput.Append("<head>")
        strOutput.Append("<title>SERT :: SWO :: Daily Incident Report</title>")
        strOutput.Append("</head>")
        strOutput.Append("<body>")
        '---------------------------------------------------------------------------------------------

        'For each item in the table write out a report.
        '---------------------------------------------------------------------------------------------
        strOutput.Append("<table>")
        strOutput.Append("    <tr>")
        strOutput.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strOutput.Append("            &nbsp;")
        strOutput.Append("        </td>")
        strOutput.Append("    </tr>")
        strOutput.Append("</table>")
        strOutput.Append("<table>")
        strOutput.Append("    <tr>")
        strOutput.Append("        <td align='left'width='400px'>")
        strOutput.Append("            <b>FLORIDA DIVISION OF EMERGENCY MANAGEMENT</b>")
        strOutput.Append("        </td>")
        strOutput.Append("    </tr>")
        strOutput.Append("    <tr>")
        strOutput.Append("        <td align='left'width='400px'>")
        strOutput.Append("            <b>STATE WATCH OFFICE</b>")
        strOutput.Append("        </td>")
        strOutput.Append("    </tr>")
        strOutput.Append("    <tr>")
        strOutput.Append("        <td align='left'width='400px'>")
        strOutput.Append("            <b>DAILY INCIDENT REPORT</b>")
        strOutput.Append("        </td>")
        strOutput.Append("    </tr>")
        strOutput.Append("</table>")
        strOutput.Append("<table>")
        strOutput.Append("    <tr>")
        strOutput.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strOutput.Append("            &nbsp;")
        strOutput.Append("        </td>")
        strOutput.Append("    </tr>")
        strOutput.Append("</table>")

        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("[spFilterDailyIncidentReport]", objConn)
        'objCmd.Parameters.AddWithValue("@UserID", "")
        'objCmd.Parameters.AddWithValue("@StartDate", "")
        'objCmd.Parameters.AddWithValue("@EndDate", "")
        'objCmd.Parameters.AddWithValue("@Date", "")

        If strAllToDate = "Yes" Then
            objCmd.Parameters.AddWithValue("@StartDate", "")
            objCmd.Parameters.AddWithValue("@EndDate", "")
            objCmd.Parameters.AddWithValue("@Date", "")
        ElseIf strAllToDate = "OneDate" Then
            objCmd.Parameters.AddWithValue("@StartDate", "")
            objCmd.Parameters.AddWithValue("@EndDate", "")
            objCmd.Parameters.AddWithValue("@Date", strDate)
        ElseIf strAllToDate = "TwoDate" Then
            objCmd.Parameters.AddWithValue("@StartDate", strStartDate)
            objCmd.Parameters.AddWithValue("@EndDate", strEndDate)
            objCmd.Parameters.AddWithValue("@Date", "")
        Else

        End If

        If strReportType = "All" Then objCmd.Parameters.AddWithValue("@Type", "")
        If strReportType = "Open" Then objCmd.Parameters.AddWithValue("@Type", "1")
        If strReportType = "Closed" Then objCmd.Parameters.AddWithValue("@Type", "2")
        If strReportType = "Assigned" Then objCmd.Parameters.AddWithValue("@Type", "5")
        If strReportType = "Pending" Then objCmd.Parameters.AddWithValue("@Type", "3")
        If strReportType = "Dismissed" Then objCmd.Parameters.AddWithValue("@Type", "4")

        objCmd.Parameters.AddWithValue("@Agency", strAgency)
        '---------------------------------------------------------------------------------------------

        'Response.Write(PublicFunctions.GetSqlParameters(objCmd.Parameters, "spFilterAllCompaniesByZipCodeForReport"))
        'Response.End()

        objCmd.CommandType = CommandType.StoredProcedure

        objDR = objCmd.ExecuteReader()
        Dim bgcolor As String
        Dim intcounter As Integer = 0
        Dim intReportCounter As Integer = 0
        Dim strHoldTopic As String = ""
        Dim strHref As String = ""
        Dim strHrefClose As String = ""
        Dim totReports As Integer = 0
        'strOutput.Append("")

        'Build the report header.
        '---------------------------------------------------------------------------------------------
        strOutput.Append("<table>")

        If strAllToDate = "Yes" Then
            strOutput.Append("<tr style='background-color: #d4d4d4'><td colspan='7' align='Center' ><b><font size='+1'>DAILY INCIDENT REPORT FOR ALL TO DATE</font></b></td></tr>")
        Else
            If strStartDate = "" Then
                strStartDate = "All"
            End If
            If strEndDate = "" Then
                strEndDate = "All"
            End If

            'strOutput.Append("<tr style='background-color: #d4d4d4'><td colspan='7' align='Center'>From " & strStartDate & " To " & strEndDate & "</td></tr>")
            strOutput.Append("<tr style='background-color: #d4d4d4'><td colspan='7' align='Center'><b><font size='+1'>DAILY INCIDENT REPORT</font></b></td></tr>")

            If strStartDate = "All" Then
                strStartDate = ""
            End If
            If strEndDate = "All" Then
                strEndDate = ""
            End If
        End If

        strOutput.Append("</table>")

        If objDR.Read() Then
            'There are records.
            objDR.Close()
            objDR = objCmd.ExecuteReader()

            While objDR.Read
                'Loop through and write out the report..
                '---------------------------------------------------------------------------------------------
                If intcounter Mod 2 = 0 Then
                    bgcolor = "eeeeef"
                Else
                    bgcolor = "f7f7f7"
                End If

                Dim localTime As String = ""
                Dim ReportedToSWOTime As String = ""
                Dim ReportedToSWOTime2 As String = ""
                Dim ReportedToSWODate As String = ""
                Dim localTime2 As String = ""
                Dim IncidentOccurredTime As String = ""
                Dim IncidentOccurredTime2 As String = ""
                Dim IncidentOccurredDate As String = ""
                Dim WorkSheets As String = ""
                Dim InitialReportsUpdates As String = ""
                Dim strStatus As String

                localTime = CStr(HelpFunction.Convertdbnulls(objDR("ReportedToSWOTime")))
                ReportedToSWODate = HelpFunction.Convertdbnulls(objDR("ReportedToSWODate"))
                localTime2 = CStr(HelpFunction.Convertdbnulls(objDR("IncidentOccurredTime")))
                IncidentOccurredDate = HelpFunction.Convertdbnulls(objDR("IncidentOccurredDate"))
                strStatus = HelpFunction.Convertdbnulls(objDR("IncidentStatus"))

                IncidentOccurredTime = Left(localTime2, 2)
                IncidentOccurredTime2 = Right(localTime2, 2)

                ReportedToSWOTime = Left(localTime, 2)
                ReportedToSWOTime2 = Right(localTime, 2)

                'Grabbing Worksheets.
                '---------------------------------------------------------------------------------------------
                objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objConn2.Open()
                objCmd2 = New SqlCommand("spSelectIncidentIncidentTypeByIncidentID", objConn2)
                objCmd2.CommandType = CommandType.StoredProcedure
                objCmd2.Parameters.AddWithValue("@IncidentID", objDR.Item("IncidentID"))

                objDR2 = objCmd2.ExecuteReader

                If objDR2.Read() Then
                    'There are records.
                    objDR2.Close()
                    objDR2 = objCmd2.ExecuteReader()

                    While objDR2.Read
                        If Not WorkSheets.Contains(CStr(objDR2.Item("IncidentType"))) Then
                            WorkSheets = WorkSheets & CStr(objDR2.Item("IncidentType")) & ", "
                        End If
                    End While
                Else
                    WorkSheets = "No Worksheets added at this time.  "
                End If

                objDR2.Close()
                objCmd2.Dispose()
                objCmd2 = Nothing
                objConn2.Close()

                If WorkSheets <> "" Then
                    WorkSheets = WorkSheets.Remove(WorkSheets.Length - 2, 2)
                End If
                '---------------------------------------------------------------------------------------------

                'Grabbing Initial Reports.
                '---------------------------------------------------------------------------------------------
                objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objConn2.Open()
                objCmd2 = New SqlCommand("spSelectInitialReportByIncidentID", objConn2)
                objCmd2.CommandType = CommandType.StoredProcedure
                objCmd2.Parameters.AddWithValue("@IncidentID", objDR.Item("IncidentID"))

                objDR2 = objCmd2.ExecuteReader

                If objDR2.Read() Then
                    'There are records.
                    objDR2.Close()
                    objDR2 = objCmd2.ExecuteReader()

                    InitialReportsUpdates = "<b>Initial Reports</b>:  "

                    While objDR2.Read
                        InitialReportsUpdates = InitialReportsUpdates & CStr(objDR2.Item("InitialReport")) & ", "
                    End While
                End If

                objDR2.Close()
                objCmd2.Dispose()
                objCmd2 = Nothing
                objConn2.Close()
                '---------------------------------------------------------------------------------------------

                'Grabbing Report Updates.
                '---------------------------------------------------------------------------------------------
                objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objConn2.Open()
                objCmd2 = New SqlCommand("spSelectUpdateReportByIncidentID", objConn2)
                objCmd2.CommandType = CommandType.StoredProcedure
                objCmd2.Parameters.AddWithValue("@IncidentID", objDR.Item("IncidentID"))

                objDR2 = objCmd2.ExecuteReader

                If InitialReportsUpdates <> "" AndAlso InitialReportsUpdates.EndsWith(", ") Then
                    InitialReportsUpdates = InitialReportsUpdates.Remove(InitialReportsUpdates.Length - 2, 2)
                End If

                If objDR2.Read() Then
                    'There are records.
                    objDR2.Close()
                    objDR2 = objCmd2.ExecuteReader()

                    InitialReportsUpdates = InitialReportsUpdates & " <b>Report Updates</b>: "

                    While objDR2.Read
                        InitialReportsUpdates = InitialReportsUpdates & CStr(objDR2.Item("UpdateReport")) & ", "
                    End While
                End If

                objDR2.Close()
                objCmd2.Dispose()
                objCmd2 = Nothing
                objConn2.Close()

                If InitialReportsUpdates <> "" AndAlso InitialReportsUpdates.EndsWith(", ") Then
                    InitialReportsUpdates = InitialReportsUpdates.Remove(InitialReportsUpdates.Length - 2, 2)
                End If
                '---------------------------------------------------------------------------------------------

                'If strHoldIncidentID <> objDR.Item("IncidentID") Then
                '    If intcounter > 0 Then
                '        strOutput.Append("<tr style='background-color:d4d4d4' ><td colspan='8' align=right><b>Incident Total: " & intReportCounter & "</b></td>")
                '        strOutput.Append("</tr>")
                '        intReportCounter = 0
                '    End If

                '    strOutput.Append("<tr style='background-color: " & bgcolor & "'>")
                '    strOutput.Append("<td>" & objDR.Item("User") & "</td>")
                'Else
                '    strOutput.Append("<tr style='background-color:" & bgcolor & "'>")
                '    strOutput.Append("<td>&nbsp;</td>")
                'End If

                strOutput.Append("<table>")
                strOutput.Append("    <tr>")
                strOutput.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
                strOutput.Append("            &nbsp;")
                strOutput.Append("        </td>")
                strOutput.Append("    </tr>")
                strOutput.Append("</table>")

                strOutput.Append("<table>")
                strOutput.Append("    <tr>")
                strOutput.Append("        <td align='left'width='400px'>")
                strOutput.Append("            <b>Incident Number:</b> ")
                strOutput.Append("           " & objDR.Item("IncidentNumber") & "  ")
                strOutput.Append("        </td>")
                strOutput.Append("    </tr>")
                strOutput.Append("</table>")

                strOutput.Append("<table>")
                strOutput.Append("    <tr>")
                strOutput.Append("        <td align='left'width='400px'>")
                strOutput.Append("            <b>Incident Name:</b> ")
                strOutput.Append("           " & objDR.Item("IncidentName") & "  ")
                strOutput.Append("        </td>")
                strOutput.Append("    </tr>")
                strOutput.Append("</table>")

                strOutput.Append("<table>")
                strOutput.Append("    <tr>")
                strOutput.Append("        <td align='left'width='400px'>")
                strOutput.Append("            <b>Occurred:</b> ")
                strOutput.Append("           " & IncidentOccurredDate & " &nbsp; " & IncidentOccurredTime & ":" & IncidentOccurredTime2 & " ET ")
                strOutput.Append("        </td>")
                strOutput.Append("    </tr>")
                strOutput.Append("</table>")

                strOutput.Append("<table>")
                strOutput.Append("    <tr>")
                strOutput.Append("        <td align='left'width='400px'>")
                strOutput.Append("            <b>Reported to SWO:</b> ")
                strOutput.Append("           " & ReportedToSWODate & " &nbsp; " & ReportedToSWOTime & ":" & ReportedToSWOTime2 & " ET ")
                strOutput.Append("        </td>")
                strOutput.Append("    </tr>")
                strOutput.Append("</table>")

                strOutput.Append("<table>")
                strOutput.Append("    <tr>")
                strOutput.Append("        <td align='left'width='400px'>")
                strOutput.Append("            <b>Affecting:</b> ")

                If CStr(objDR.Item("Statewide")) = "No" Then
                    Dim oCountyRegion As New CountyRegion(objDR.Item("IncidentID"))
                    Dim strCountyRegion As String = ""
                    strCountyRegion = oCountyRegion.GetRegionAndCountyList(False)
                    strOutput.Append("           " & strCountyRegion & "  ")
                    'strOutput.Append("           " & objDR.Item("AddedCounty") & "  ")
                Else
                    strOutput.Append("           Statewide  ")
                End If

                strOutput.Append("        </td>")
                strOutput.Append("    </tr>")
                strOutput.Append("</table>")

                strOutput.Append("<table>")
                strOutput.Append("    <tr>")
                strOutput.Append("        <td align='left'width='400px'>")
                strOutput.Append("            <b>Involving:</b> ")
                strOutput.Append("           " & WorkSheets & "  ")
                strOutput.Append("        </td>")
                strOutput.Append("    </tr>")
                strOutput.Append("</table>")

                strOutput.Append("<table>")
                strOutput.Append("    <tr>")
                strOutput.Append("        <td align='left'width='400px'>")
                strOutput.Append("            <b>Initial Report/Updates:</b> ")
                strOutput.Append("           " & InitialReportsUpdates & "  ")
                strOutput.Append("        </td>")
                strOutput.Append("    </tr>")
                strOutput.Append("</table>")

                strOutput.Append("<table>")
                strOutput.Append("    <tr>")
                strOutput.Append("        <td align='left'width='400px'>")
                strOutput.Append("            <b>Status:</b> ")
                strOutput.Append("           " & strStatus & "  ")
                strOutput.Append("        </td>")
                strOutput.Append("    </tr>")
                strOutput.Append("</table>")

                'Increment report totals.
                '---------------------------------------------------------------------------------------------
                'strHoldIncidentID = objDR.Item("IncidentID")
                intcounter = intcounter + 1
                intReportCounter = intReportCounter + 1
            End While

            'Write out the totals.
            '---------------------------------------------------------------------------------------------
            strOutput.Append("<table>")
            strOutput.Append("    <tr>")
            strOutput.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strOutput.Append("            <b>Total Incidents: " & intcounter & " ")
            strOutput.Append("        </td>")
            strOutput.Append("    </tr>")
            strOutput.Append("</table>")

            'strOutput.Append("<table width='100%' cellspacing='0' border='0' style='background-color:#d4d4d4'>")
            'strOutput.Append("<tr><td colspan='8' align='right'><b>Total Incidents: " & intcounter & "</b></td>")
            'strOutput.Append("</tr></table>")
        Else
            'There are no records.
            strOutput.Append("<table>")
            strOutput.Append("    <tr>")
            strOutput.Append("        <td align='left'width='400px'>")
            strOutput.Append("            <b>No Records</b> ")
            strOutput.Append("        </td>")
            strOutput.Append("    </tr>")
            strOutput.Append("</table>")
        End If

        'Close the table.
        'strOutput.Append("</table>")

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()

        strOutput.Append("</body>")
        strOutput.Append("</html>")

        Response.Write(strOutput.ToString())
        Response.Flush()
        Response.SuppressContent = True
        HttpContext.Current.ApplicationInstance.CompleteRequest()
    End Sub

    Sub ExportToGovDelivery()
        'Build the report.
        '---------------------------------------------------------------------------------------------
        'Make sure there is data and if so write out the body info.
        strOutput.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
        strOutput.Append("<html xmlns='http://www.w3.org/1999/xhtml'>")
        strOutput.Append("<head>")
        strOutput.Append("<title>SERT :: SWO :: Daily Incident Report</title>")
        strOutput.Append("<style> body {font-size:12pt;} h1 {font-size:14pt;}</style>")
        strOutput.Append("</head>")
        strOutput.Append("<body>")
        '---------------------------------------------------------------------------------------------

        'For each item in the table write out a report.
        '---------------------------------------------------------------------------------------------
        strOutput.Append("<CENTER><h1><b>FLORIDA DIVISION OF EMERGENCY MANAGEMENT</b>")
        strOutput.Append("<br />")
        strOutput.Append("<b>STATE WATCH OFFICE</b>")
        strOutput.Append("<br />")
        strOutput.Append("<b>DAILY INCIDENT REPORT</b></h1></center>")
        strOutput.Append("<p>")

        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("[spFilterDailyIncidentReport]", objConn)

        If strAllToDate = "Yes" Then
            objCmd.Parameters.AddWithValue("@StartDate", "")
            objCmd.Parameters.AddWithValue("@EndDate", "")
            objCmd.Parameters.AddWithValue("@Date", "")
        ElseIf strAllToDate = "OneDate" Then
            objCmd.Parameters.AddWithValue("@StartDate", "")
            objCmd.Parameters.AddWithValue("@EndDate", "")
            objCmd.Parameters.AddWithValue("@Date", strDate)
        ElseIf strAllToDate = "TwoDate" Then
            objCmd.Parameters.AddWithValue("@StartDate", strStartDate)
            objCmd.Parameters.AddWithValue("@EndDate", strEndDate)
            objCmd.Parameters.AddWithValue("@Date", "")
        Else

        End If

        If strReportType = "All" Then objCmd.Parameters.AddWithValue("@Type", "")
        If strReportType = "Open" Then objCmd.Parameters.AddWithValue("@Type", "1")
        If strReportType = "Closed" Then objCmd.Parameters.AddWithValue("@Type", "2")
        If strReportType = "Assigned" Then objCmd.Parameters.AddWithValue("@Type", "5")
        If strReportType = "Pending" Then objCmd.Parameters.AddWithValue("@Type", "3")
        If strReportType = "Dismissed" Then objCmd.Parameters.AddWithValue("@Type", "4")

        objCmd.Parameters.AddWithValue("@Agency", strAgency)
        '---------------------------------------------------------------------------------------------

        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()
        Dim intcounter As Integer = 0
        Dim intReportCounter As Integer = 0
        Dim strHoldTopic As String = ""
        Dim strHref As String = ""
        Dim strHrefClose As String = ""
        Dim totReports As Integer = 0

        'Build the report header.
        '---------------------------------------------------------------------------------------------
        If strAllToDate = "Yes" Then
            strOutput.Append("<b><font size='+1'>DAILY INCIDENT REPORT FOR ALL TO DATE</font></b>")
            strOutput.Append("<br />")
        Else
            If strStartDate = "" Then
                strStartDate = "All"
            End If
            If strEndDate = "" Then
                strEndDate = "All"
            End If

            'strOutput.Append("From " & strStartDate & " To " & strEndDate)
            'strOutput.Append("<br />")
            'strOutput.Append("<b><font size='+1'>DAILY INCIDENT REPORT</font></b>")
            'strOutput.Append("<br />")

            If strStartDate = "All" Then
                strStartDate = ""
            End If
            If strEndDate = "All" Then
                strEndDate = ""
            End If
        End If

        If objDR.Read() Then
            'There are records.
            objDR.Close()
            objDR = objCmd.ExecuteReader()

            While objDR.Read
                'Loop through and write out the report..
                '---------------------------------------------------------------------------------------------
                Dim localTime As String = ""
                Dim ReportedToSWOTime As String = ""
                Dim ReportedToSWOTime2 As String = ""
                Dim ReportedToSWODate As String = ""
                Dim localTime2 As String = ""
                Dim IncidentOccurredTime As String = ""
                Dim IncidentOccurredTime2 As String = ""
                Dim IncidentOccurredDate As String = ""
                Dim WorkSheets As String = ""
                Dim InitialReportsUpdates As String = ""
                Dim strStatus As String

                localTime = CStr(HelpFunction.Convertdbnulls(objDR("ReportedToSWOTime")))
                ReportedToSWODate = HelpFunction.Convertdbnulls(objDR("ReportedToSWODate"))
                localTime2 = CStr(HelpFunction.Convertdbnulls(objDR("IncidentOccurredTime")))
                IncidentOccurredDate = HelpFunction.Convertdbnulls(objDR("IncidentOccurredDate"))
                strStatus = HelpFunction.Convertdbnulls(objDR("IncidentStatus"))

                IncidentOccurredTime = Left(localTime2, 2)
                IncidentOccurredTime2 = Right(localTime2, 2)

                ReportedToSWOTime = Left(localTime, 2)
                ReportedToSWOTime2 = Right(localTime, 2)

                'Grabbing Worksheets.
                '---------------------------------------------------------------------------------------------
                objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objConn2.Open()
                objCmd2 = New SqlCommand("spSelectIncidentIncidentTypeByIncidentID", objConn2)
                objCmd2.CommandType = CommandType.StoredProcedure
                objCmd2.Parameters.AddWithValue("@IncidentID", objDR.Item("IncidentID"))

                objDR2 = objCmd2.ExecuteReader

                If objDR2.Read() Then
                    'There are records.
                    objDR2.Close()
                    objDR2 = objCmd2.ExecuteReader()

                    While objDR2.Read
                        If Not WorkSheets.Contains(CStr(objDR2.Item("IncidentType"))) Then
                            WorkSheets = WorkSheets & CStr(objDR2.Item("IncidentType")) & ", "
                        End If
                    End While
                Else
                    WorkSheets = "No Worksheets added at this time.  "
                End If

                objDR2.Close()
                objCmd2.Dispose()
                objCmd2 = Nothing
                objConn2.Close()

                If WorkSheets <> "" Then
                    WorkSheets = WorkSheets.Remove(WorkSheets.Length - 2, 2)
                End If
                '---------------------------------------------------------------------------------------------

                'Grabbing Initial Reports.
                '---------------------------------------------------------------------------------------------
                objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objConn2.Open()
                objCmd2 = New SqlCommand("spSelectInitialReportByIncidentID", objConn2)
                objCmd2.CommandType = CommandType.StoredProcedure
                objCmd2.Parameters.AddWithValue("@IncidentID", objDR.Item("IncidentID"))

                objDR2 = objCmd2.ExecuteReader

                If objDR2.HasRows Then
                    'There are records.
                    'InitialReportsUpdates = "<b>Initial Reports</b>:  "

                    While objDR2.Read
                        InitialReportsUpdates = InitialReportsUpdates & CStr(objDR2.Item("InitialReport")) & ", "
                    End While
                End If

                objDR2.Close()
                objCmd2.Dispose()
                objCmd2 = Nothing
                objConn2.Close()
                '---------------------------------------------------------------------------------------------

                'Grabbing Report Updates.
                '---------------------------------------------------------------------------------------------
                objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objConn2.Open()
                objCmd2 = New SqlCommand("spSelectUpdateReportByIncidentID", objConn2)
                objCmd2.CommandType = CommandType.StoredProcedure
                objCmd2.Parameters.AddWithValue("@IncidentID", objDR.Item("IncidentID"))

                objDR2 = objCmd2.ExecuteReader

                If InitialReportsUpdates <> "" AndAlso InitialReportsUpdates.EndsWith(", ") Then
                    InitialReportsUpdates = InitialReportsUpdates.Remove(InitialReportsUpdates.Length - 2, 2)
                End If

                If objDR2.HasRows Then
                    'There are records.
                    'InitialReportsUpdates = InitialReportsUpdates & " <b>Report Updates</b>: "

                    While objDR2.Read
                        InitialReportsUpdates = InitialReportsUpdates & CStr(If(IsDBNull(objDR2.Item("UpdateReport")), "", objDR2.Item("UpdateReport"))) & ", "
                    End While
                End If

                objDR2.Close()
                objCmd2.Dispose()
                objCmd2 = Nothing
                objConn2.Close()

                If InitialReportsUpdates <> "" AndAlso InitialReportsUpdates.EndsWith(", ") Then
                    InitialReportsUpdates = InitialReportsUpdates.Remove(InitialReportsUpdates.Length - 2, 2)
                End If
                '---------------------------------------------------------------------------------------------

                strOutput.Append("<br />")
                strOutput.Append("<b>Incident Number:</b> ")
                strOutput.Append("           " & objDR.Item("IncidentNumber") & "  ")
                strOutput.Append("<br />")
                strOutput.Append("<b>Incident Name:</b> ")
                strOutput.Append("           " & objDR.Item("IncidentName") & "  ")
                strOutput.Append("<br />")
                strOutput.Append("<b>Occurred:</b> ")
                strOutput.Append("           " & IncidentOccurredDate & " &nbsp; " & IncidentOccurredTime & ":" & IncidentOccurredTime2 & " ET ")
                strOutput.Append("<br />")
                strOutput.Append("<b>Reported to SWO:</b> ")
                strOutput.Append("           " & ReportedToSWODate & " &nbsp; " & ReportedToSWOTime & ":" & ReportedToSWOTime2 & " ET ")
                strOutput.Append("<br />")
                strOutput.Append("<b>Affecting:</b> ")

                If CStr(objDR.Item("Statewide")) = "No" Then
                    Dim oCountyRegion As New CountyRegion(objDR.Item("IncidentID"))
                    Dim strCountyRegion As String = ""
                    strCountyRegion = oCountyRegion.GetRegionAndCountyList(False)
                    strOutput.Append("           " & strCountyRegion & "  ")
                    'strOutput.Append("           " & objDR.Item("AddedCounty") & "  ")
                Else
                    strOutput.Append("           Statewide  ")
                End If

                strOutput.Append("<br />")
                strOutput.Append("<b>Involving:</b> ")
                strOutput.Append("           " & WorkSheets & "  ")
                strOutput.Append("<br />")
                strOutput.Append("<b>Summary:</b> ")
                strOutput.Append("           " & InitialReportsUpdates & "  ")
                strOutput.Append("<br />")
                strOutput.Append("<b>Status:</b> ")
                strOutput.Append("           " & strStatus & "  ")
                strOutput.Append("<br />")

                'Increment report totals.
                '---------------------------------------------------------------------------------------------
                'strHoldIncidentID = objDR.Item("IncidentID")
                intcounter = intcounter + 1
                intReportCounter = intReportCounter + 1
            End While

            'Write out the totals.
            '---------------------------------------------------------------------------------------------
            strOutput.Append("<br />")
            strOutput.Append("<br />")
            strOutput.Append("<b>Total Incidents: " & intcounter & " ")
            strOutput.Append("<br />")
        Else
            'There are no records.
            strOutput.Append("<b>No Records</b> ")
            strOutput.Append("<br />")
        End If

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()

        strOutput.Append("</body>")
        strOutput.Append("</html>")

        Response.Write(strOutput.ToString())
        Response.Flush()
        Response.SuppressContent = True
        HttpContext.Current.ApplicationInstance.CompleteRequest()
    End Sub

End Class
