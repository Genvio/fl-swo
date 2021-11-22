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

Partial Class ExportToPDF
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

  
    Dim strpdf As String 'holds the return results
    Dim strOutputFileName As String 'the name of the html file
    Dim strUrlString As String 'the path to the file

    'Cookie for the Login Info
    'Public ObjCookie As System.Web.HttpCookie
    'Dim oCookie As System.Web.HttpCookie

    Dim globalHasErrors As Boolean = False

    Dim strOutput As New System.Text.StringBuilder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then

           


        End If

    End Sub


    Protected Sub btnConvert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConvert.Click

        Dim localRandomStringForDOC As String = ""

        localRandomStringForDOC = HelpFunction.RandomStringGenerator(6)

        'First we will Delete all Old Reports

        HelpFunction.CleanupReportDirectory()


        System.IO.File.Copy(Server.MapPath("Downloads") & "\Template\Word\Template.doc", Server.MapPath("Downloads") & "\Created\" & localRandomStringForDOC & "Incident.doc")



        ' Create a new Microsoft Word application object 
        Dim word As New Microsoft.Office.Interop.Word.Application()

        ' C# doesn't have optional arguments so we'll need a dummy value 
        Dim oMissing As Object = System.Reflection.Missing.Value

        ' Get list of Word files in specified directory 
        Dim dirInfo As New DirectoryInfo(Server.MapPath("Downloads\Created"))
        Dim wordFiles As FileInfo() = dirInfo.GetFiles("*.doc")

        word.Visible = False
        word.ScreenUpdating = False

        For Each wordFile As FileInfo In wordFiles
            ' Cast as Object for word Open method 
            Dim filename As [Object] = DirectCast(wordFile.FullName, [Object])

            ' Use the dummy value as a placeholder for optional arguments 
            Dim doc As Document = word.Documents.Open(filename, oMissing, oMissing, oMissing, oMissing, oMissing, _
             oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, _
             oMissing, oMissing, oMissing, oMissing)
            doc.Activate()

            Dim outputFileName As Object = wordFile.FullName.Replace(".doc", ".pdf")
            Dim fileFormat As Object = WdSaveFormat.wdFormatPDF

            ' Save document into PDF Format 
            doc.SaveAs(outputFileName, fileFormat, oMissing, oMissing, oMissing, oMissing, _
             oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, _
             oMissing, oMissing, oMissing, oMissing)

            ' Close the Word document, but leave the Word application open. 
            ' doc has to be cast to type _Document so that it will find the 
            ' correct Close method.                 
            Dim saveChanges As Object = WdSaveOptions.wdDoNotSaveChanges
            DirectCast(doc, _Document).Close(saveChanges, oMissing, oMissing)
            doc = Nothing
        Next

        ' word has to be cast to type _Application so that it will find 
        ' the correct Quit method. 
        DirectCast(word, _Application).Quit(oMissing, oMissing, oMissing)
        word = Nothing

        Response.Redirect("Downloads/Created/" & localRandomStringForDOC & "Incident.pdf")


    End Sub

End Class
