Imports Microsoft.Office.Interop.Word
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

Partial Class FullMainReport
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
    Dim strDisasterID As String
    Dim strDisaster As String
    Dim strActivityID As String
    Dim strActivity As String
    Dim strApplicantID As String
    Dim strApplicant As String
    Dim strPwNumber As String



    Dim strOutput As New System.Text.StringBuilder
    Dim strOutputFileName As String 'the name of the html file
    Dim strUrlString As String 'the path to the file


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        strReportFormat = Request.QueryString("ReportFormat")

        strOutputFileName = HelpFunction.RandomStringGenerator(6)

        strUrlString = System.Configuration.ConfigurationManager.AppSettings("FilePath").ToString & "\Reports\ReportOutputFiles\" & strOutputFileName & ".htm"

        If Page.IsPostBack = False Then

            Select Case strReportFormat

                Case "HTML"
                    ExportToHTML()
                Case "Word"
                    ExportToWord()
                Case "PDF"
                    ExportToPDF()
                Case Else
                    'Do Nothing
            End Select

        End If

    End Sub

    'Export Subs
    Sub ExportToHTML()

        Dim oRegularReport As New RegularReport(Request("IncidentID"), strReportFormat)

        strOutput.Append(oRegularReport.gStrTotalReport)

        'Response.Write(oRegularReport.gStrTotalReport)

        Response.Write(strOutput.ToString())

    End Sub

    Sub ExportToWord()

        'build the content for the dynamic Word document    
        'in HTML alongwith some Office specific style properties. 

        strOutput.Append("<html xmlns:o='urn:schemas-microsoft-com:office:office' " & _
        "xmlns:w='urn:schemas-microsoft-com:office:word'" & _
        "xmlns='http://www.w3.org/TR/REC-html40'>" & _
        "<head><title>Total Reports</title>")

        strOutput.Append( _
                   "<!--[if gte mso 9]>" & _
                   "<xml>" & _
                   "<w:WordDocument>" & _
                   "<w:View>Print</w:View>" & _
                   "<w:Zoom>90</w:Zoom>  " & _
                   "</w:WordDocument>" & _
                   "</xml>" & _
                   "<![endif]-->")

        strOutput.Append( _
                 "<style>" & _
                 "<!-- /* Style Definitions               */@page Section1{size:8.5in 11.0in;" & _
                 "margin:1.0in 1.25in 1.0in " & _
                 "1.25in;mso-header-margin:.5in; " & _
                 "mso-footer-margin:.5in;    mso-paper-source:0;}" & _
                 "div.Section1{page:Section1;}-->" & _
                 "</style></head>")

        strOutput.Append( _
                    "<body lang=EN-US style='tab-interval:.5in'>" & _
                    "<div class=Section1>")

        'The Guts of Report Go Below

        Dim oRegularReport As New RegularReport(Request("IncidentID"), strReportFormat)

        strOutput.Append(oRegularReport.gStrTotalReport)

        'The Guts of Report Go Above

        strOutput.Append( _
                    "</div></body></html>")

        'Force this content to be downloaded    'as a Word document with the name of your choice    
        Response.AppendHeader("Content-Type", "application/msword")
        Response.AppendHeader("Content-disposition", _
        "attachment; filename=Daily Activity Reports by User.doc")
        Response.Charset = ""

        'Display the Word Document

        'If Not System.IO.File.Exists("C:\somefile.doc") = True Then

        '    Dim file As System.IO.FileStream
        '    file = System.IO.File.Create("C:\somefile.doc")
        '    file.Close()

        'End If

        ' ''System.IO.File.Copy("C:\foo\somefile.txt", "C:\bar\somefile.txt")

        ' ''System.IO.File.Move("C:\foo\somefile.txt", "C:\bar\somefile.txt")

        'My.Computer.FileSystem.WriteAllText("C:\somefile.doc", strOutput.ToString(), True)

        Response.Write(strOutput)

    End Sub

    Sub ExportToPDF()

        'Response.Write(Server.MapPath("StartFiles\"))
        'Response.End()

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

        'Dim oRegularReport As New RegularReport(Request("IncidentID"), strReportFormat)

        'strOutput.Append(oRegularReport.gStrTotalReport)

        ''The Guts of Report Go Above

        'strOutput.Append( _
        '            "</div></body></html>")

        'Dim localStartWordFile As String = HelpFunction.RandomStringGenerator(6)


        ''Force this content to be downloaded    'as a Word document with the name of your choice    
        'Response.AppendHeader("Content-Type", "application/msword")
        'Response.AppendHeader("Content-disposition", _
        '"attachment; filename=Total Daily Reports By User.doc")
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

        ' ''PDF
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