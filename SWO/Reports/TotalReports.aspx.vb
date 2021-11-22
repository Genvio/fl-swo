'Imports Microsoft.Office.Interop.Word
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



Partial Class Reports_TotalReports
    Inherits System.Web.UI.Page
    'Help Functions from our App_Code
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    'For Connecting to the database
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet
    Public objDS2 As New System.Data.DataSet
    Public objDS3 As New System.Data.DataSet
    Public objDS4 As New System.Data.DataSet
    Public objDS5 As New System.Data.DataSet

    Dim globalHasErrors As Boolean = False

    Dim strStartDate As String
    Dim strEndDate As String
    Dim strAllToDate As String
    Dim strUserID As String
    Dim strUser As String
    Dim strReportFormat As String

    Dim strOutput As New System.Text.StringBuilder
    Dim strOutputFileName As String 'the name of the html file
    Dim strUrlString As String 'the path to the file


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        strStartDate = Request.QueryString("StartDate")
        strEndDate = Request.QueryString("EndDate")
        strAllToDate = Request.QueryString("AllToDate")
        strUserID = Request.QueryString("UserID")
        strUser = Request.QueryString("User")
        strReportFormat = Request.QueryString("ReportFormat")

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
                Case Else
                    'Do Nothing
            End Select

        End If

    End Sub

    Public Sub BuildGridView()
        'build the report
        '-------------------------------------------------------------------------------

        Dim sw As System.IO.StreamWriter = System.IO.File.CreateText(strUrlString)

        'Make sure there is data and if so write out the body info
        '---------------------------------------------------------------------------------------------

        strOutput.Append("<HTML>")
        strOutput.Append("<HEAD>")
        strOutput.Append("<title>SERT :: SWO :: Incident By User</title>")
       
        strOutput.Append("<LINK href='" & HttpContext.Current.Request.Url.Scheme & "://" & HttpContext.Current.Request.Url.Authority & Request.ApplicationPath & "/Includes/CSS/Report.css' type='text/css' rel='stylesheet'>")


        strOutput.Append("</head>")
        strOutput.Append("<BODY class=""bodyreport"">")

        'for each item in the table write out a report
        '---------------------------------------------------------------------------------------------

   

        'connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        DBConStringHelper.PrepareConnection(objConn) 'open the connection

        Response.Write(strUserID)
        Response.End()

        objCmd = New SqlCommand("[spFilterTotalIncidentsByUser]", objConn)
        objCmd.Parameters.AddWithValue("@UserID", strUserID)
       
        If strAllToDate = "Yes" Then
            objCmd.Parameters.AddWithValue("@StartDate", "")
            objCmd.Parameters.AddWithValue("@EndDate", "")
        Else

            'Response.Write("<br>")
            'Response.Write(strStartDate)
            'Response.Write("<br>")
            'Response.Write(strEndDate)
            'Response.Write("<br>")
            'Response.End()

            objCmd.Parameters.AddWithValue("@StartDate", strStartDate)
            objCmd.Parameters.AddWithValue("@EndDate", strEndDate)

        End If


        '------------------------------------------------------------------
        ' Response.Write(PublicFunctions.GetSqlParameters(objCmd.Parameters, "spFilterAllCompaniesByZipCodeForReport"))
        ' Response.End()
        objCmd.CommandType = CommandType.StoredProcedure

       
        objDR = objCmd.ExecuteReader()
        Dim bgcolor As String
        Dim intcounter As Integer = 0
        Dim intReportCounter As Integer = 0
        Dim strHoldTopic As String = ""
        Dim strHref As String = ""
        Dim strHrefClose As String = ""
        Dim strHoldUser As String = ""
        Dim totReports As Integer = 0
        'strOutput.Append("")
        'build the report header
        '------------------------------------------------------------------
        strOutput.Append("<table width='100%' class='reportheader' cellspacing='0' border='0'>")
        'strOutput.Append("  <tr>")
        'strOutput.Append("      <td colspan='7' align='center' style='background-color: eeeeef'>") 'the logo image
        'strOutput.Append("          <img src='" & HttpContext.Current.Request.Url.Scheme & "://" & HttpContext.Current.Request.Url.Authority & Request.ApplicationPath & "/Images/PACDARlogo.jpg' border='0' alt='0'>")
        'strOutput.Append("      </td>")
        'strOutput.Append("  </tr>")
        strOutput.Append("  <tr style='background-color: eeeeef'>")
        strOutput.Append("      <td colspan='7' align='Center'>")
        strOutput.Append("          <b><font size='+1'>SERT :: SWO :: Incident By User</font></b>")
        strOutput.Append("      </td>")
        strOutput.Append("  </tr>")
        strOutput.Append("  <tr style='background-color: eeeeef'>")
        strOutput.Append("      <td colspan='7' align='Center'>" & Now() & "")
        strOutput.Append("      </td>")
        strOutput.Append("")
        strOutput.Append("</tr>")

        If strAllToDate = "Yes" Then
            strOutput.Append("<tr style='background-color: eeeeef'><td colspan='7' align='Center' >All to Date</td></tr>")
        Else

            If strStartDate = "" Then
                strStartDate = "All"
            End If

            If strEndDate = "" Then
                strEndDate = "All"
            End If

            strOutput.Append("<tr style='background-color: eeeeef'><td colspan='7' align='Center'>From " & strStartDate & " To " & strEndDate & "</td></tr>")

            If strStartDate = "All" Then
                strStartDate = ""
            End If

            If strEndDate = "All" Then
                strEndDate = ""
            End If

        End If


        strOutput.Append("</table>")
        '------------------------------------------------------------------

        strOutput.Append("<table width='100%' cellspacing='0' border='0' ><tr style='background-color: d4d4d4; border:0;'>")
        strOutput.Append("<td align=Left><b>User</b></td>")
        strOutput.Append("<td align=center>&nbsp;</td>")
        strOutput.Append("<td align=center><b>Status</b></td>")
        strOutput.Append("<td align=center><b>Incident Name</b></td>")
        strOutput.Append("<td align=center><b>Date</b></td>")
        strOutput.Append("<td align=center><b>Time</b></td>")
        strOutput.Append("<td align=center><b>I. Report</b></td>")
        strOutput.Append("</tr>")


        If objDR.Read() Then
            'there are records
            objDR.Close()
            objDR = objCmd.ExecuteReader()

            While objDR.Read
                'loop through and write out the report....
                '-----------------------------------------------------------
                If intcounter Mod 2 = 0 Then
                    bgcolor = ""
                Else
                    bgcolor = "f7f7f7"
                End If

                '8c87c1

                If strHoldUser <> objDR.Item("User") Then

                    If intcounter > 0 Then
                        strOutput.Append("<tr style='background-color:d4d4d4' ><td colspan='8' align=right><b>Incident Total: " & intReportCounter & "</b></td>")
                        strOutput.Append("</tr>")
                        intReportCounter = 0
                    End If

                    strOutput.Append("<tr style='background-color: " & bgcolor & "'>")
                    strOutput.Append("<td>" & objDR.Item("User") & "</td>")

                Else
                    strOutput.Append("<tr style='background-color:" & bgcolor & "'>")
                    strOutput.Append("<td>&nbsp;</td>")
                End If

                strOutput.Append("<td align=center>&nbsp;</td>")
                strOutput.Append("<td align=center>" & objDR.Item("IncidentStatus") & "</td>")
                strOutput.Append("<td align=center>" & objDR.Item("IncidentName") & "</td>")
                strOutput.Append("<td align=center>" & objDR.Item("IncidentOccurredDate") & "</td>")
                strOutput.Append("<td align=center>" & objDR.Item("IncidentOccurredTime") & "</td>")
                strOutput.Append("<td align=center>" & objDR.Item("InitialReport") & "</td>")
                strOutput.Append("</tr>")


                'increment report totals
                '-----------------------------------------------------------
                strHoldUser = objDR.Item("User")
                intcounter = intcounter + 1
                intReportCounter = intReportCounter + 1

            End While
            'Write out the totals
            '-----------------------------------------
            strOutput.Append("<tr style='background-color: d4d4d4; border:0;'><td colspan='8' align='right'><b>Total Incidents By User: " & intcounter & "</b></td>")
            strOutput.Append("</tr></table>")
        Else
            'there are no records
            strOutput.Append("<table width='100%' class='reportinnertable'><tr><td colspan='8' align='center'>No Records</td><tr></table>")
        End If
        'Close the table
        strOutput.Append("</table>")

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()


        strOutput.Append("</BODY>")
        strOutput.Append("</HTML>")

    End Sub


    'Export Subs
    Sub ExportToHTML()

        BuildGridView()

        Response.Write(strOutput.ToString())

    End Sub

    Sub ExportToWord()

        'build the content for the dynamic Word document    
        'in HTML alongwith some Office specific style properties. 

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
        '         "<style>" & _
        '         "<!-- /* Style Definitions               */@page Section1{size:8.5in 11.0in;" & _
        '         "margin:1.0in 1.25in 1.0in " & _
        '         "1.25in;mso-header-margin:.5in; " & _
        '         "mso-footer-margin:.5in;    mso-paper-source:0;}" & _
        '         "div.Section1{page:Section1;}-->" & _
        '         "</style></head>")

        'strOutput.Append( _
        '            "<body lang=EN-US style='tab-interval:.5in'>" & _
        '            "<div class=Section1>")

        ''The Guts of Report Go Below

        'BuildGridView()

        ''The Guts of Report Go Above

        'strOutput.Append( _
        '            "</div></body></html>")



        ''Force this content to be downloaded    'as a Word document with the name of your choice    
        'Response.AppendHeader("Content-Type", "application/msword")
        'Response.AppendHeader("Content-disposition", _
        '"attachment; filename=Total Incidents By User.doc")
        'Response.Charset = ""

        ''Display the Word Document

        ''If Not System.IO.File.Exists("C:\somefile.doc") = True Then

        ''    Dim file As System.IO.FileStream
        ''    file = System.IO.File.Create("C:\somefile.doc")
        ''    file.Close()

        ''End If

        '' ''System.IO.File.Copy("C:\foo\somefile.txt", "C:\bar\somefile.txt")

        '' ''System.IO.File.Move("C:\foo\somefile.txt", "C:\bar\somefile.txt")

        ''My.Computer.FileSystem.WriteAllText("C:\somefile.doc", strOutput.ToString(), True)

        'Response.Write(strOutput)

    End Sub

    Sub ExportToExcel()

        


        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        DBConStringHelper.PrepareConnection(objConn) 'open the connection

        objCmd = New SqlCommand("[spFilterTotalIncidentsByUser]", objConn)
        objCmd.Parameters.AddWithValue("@UserID", strUserID)

        If strAllToDate = "Yes" Then
            objCmd.Parameters.AddWithValue("@StartDate", "")
            objCmd.Parameters.AddWithValue("@EndDate", "")
        Else

            'Response.Write("<br>")
            'Response.Write(strStartDate)
            'Response.Write("<br>")
            'Response.Write(strEndDate)
            'Response.Write("<br>")
            'Response.End()

            objCmd.Parameters.AddWithValue("@StartDate", strStartDate)
            objCmd.Parameters.AddWithValue("@EndDate", strEndDate)

        End If


        '------------------------------------------------------------------
        ' Response.Write(PublicFunctions.GetSqlParameters(objCmd.Parameters, "spFilterAllCompaniesByZipCodeForReport"))
        ' Response.End()
        objCmd.CommandType = CommandType.StoredProcedure

        'send the results to a dataset
        objDA = New System.Data.SqlClient.SqlDataAdapter
        objDA.SelectCommand = objCmd
        objDA.Fill(objDS) 'put the data into the dataset
        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()


        DataSetToExcel.Convert(objDS, Response)
        objDS = Nothing

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

        ''Response.Write(Server.MapPath("StartFiles\"))
        ''Response.End()

        ''First we will Delete all Old Reports
        'HelpFunction.CleanupReportDirectory()
        'HelpFunction.CleanupReportDirectory2()

        ''build the content for the dynamic Word document    
        ''in HTML alongwith some Office specific style properties. 

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
        '         "<style>" & _
        '         "<!-- /* Style Definitions               */@page Section1{size:8.5in 11.0in;" & _
        '         "margin:1.0in 1.25in 1.0in " & _
        '         "1.25in;mso-header-margin:.5in; " & _
        '         "mso-footer-margin:.5in;    mso-paper-source:0;}" & _
        '         "div.Section1{page:Section1;}-->" & _
        '         "</style></head>")

        'strOutput.Append( _
        '            "<body lang=EN-US style='tab-interval:.5in'>" & _
        '            "<div class=Section1>")

        ''The Guts of Report Go Below

        'BuildGridView()

        ''The Guts of Report Go Above

        'strOutput.Append( _
        '            "</div></body></html>")

        'Dim localStartWordFile As String = HelpFunction.RandomStringGenerator(6)




        ''Force this content to be downloaded    'as a Word document with the name of your choice    
        'Response.AppendHeader("Content-Type", "application/msword")
        'Response.AppendHeader("Content-disposition", _
        '"attachment; filename=Total Incident By User.doc")
        'Response.Charset = ""

        ''Display the Word Document

        ''Response.Write(Server.MapPath("StartFiles\") & localStartWordFile & ".doc")
        ''Response.End()

        'If Not System.IO.File.Exists(Server.MapPath("StartFiles\") & localStartWordFile & ".doc") = True Then

        '    Dim file As System.IO.FileStream
        '    file = System.IO.File.Create(Server.MapPath("StartFiles\") & localStartWordFile & ".doc")
        '    file.Close()

        'End If

        'My.Computer.FileSystem.WriteAllText(Server.MapPath("StartFiles\") & localStartWordFile & ".doc", strOutput.ToString(), True)

        '''PDF
        'System.IO.File.Copy(Server.MapPath("StartFiles\") & localStartWordFile & ".doc", Server.MapPath("ReportOutputFiles\") & localStartWordFile & ".doc")



        '' Create a new Microsoft Word application object 
        'Dim word As New Microsoft.Office.Interop.Word.Application()

        '' C# doesn't have optional arguments so we'll need a dummy value 
        'Dim oMissing As Object = System.Reflection.Missing.Value

        '' Get list of Word files in specified directory 

        ''Response.Write(Server.MapPath("StartFiles\"))
        ''Response.End()

        'Dim dirInfo As New DirectoryInfo(Server.MapPath("StartFiles\"))
        'Dim wordFiles As FileInfo() = dirInfo.GetFiles("*.doc")

        'word.Visible = False
        'word.ScreenUpdating = False

        'For Each wordFile As FileInfo In wordFiles
        '    ' Cast as Object for word Open method 
        '    Dim filename As [Object] = DirectCast(wordFile.FullName, [Object])

        '    ' Use the dummy value as a placeholder for optional arguments 
        '    Dim doc As Document = word.Documents.Open(filename, oMissing, oMissing, oMissing, oMissing, oMissing, _
        '     oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, _
        '     oMissing, oMissing, oMissing, oMissing)
        '    doc.Activate()

        '    Dim outputFileName As Object = wordFile.FullName.Replace(".doc", ".pdf")
        '    Dim fileFormat As Object = WdSaveFormat.wdFormatPDF

        '    ' Save document into PDF Format 
        '    doc.SaveAs(outputFileName, fileFormat, oMissing, oMissing, oMissing, oMissing, _
        '     oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, _
        '     oMissing, oMissing, oMissing, oMissing)

        '    ' Close the Word document, but leave the Word application open. 
        '    ' doc has to be cast to type _Document so that it will find the 
        '    ' correct Close method.                 
        '    Dim saveChanges As Object = WdSaveOptions.wdDoNotSaveChanges
        '    DirectCast(doc, _Document).Close(saveChanges, oMissing, oMissing)
        '    doc = Nothing
        'Next

        '' word has to be cast to type _Application so that it will find 
        '' the correct Quit method. 
        'DirectCast(word, _Application).Quit(oMissing, oMissing, oMissing)
        'word = Nothing

        'Response.Redirect("StartFiles\" & localStartWordFile & ".pdf")


    End Sub

End Class