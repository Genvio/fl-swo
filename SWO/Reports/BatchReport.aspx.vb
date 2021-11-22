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

Partial Class BatchReport
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

        strReportFormat = "HTML"

        strOutputFileName = HelpFunction.RandomStringGenerator(6)

        strUrlString = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FilePath").ToString) & "\Reports\ReportOutputFiles\" & strOutputFileName & ".htm"

        If Page.IsPostBack = False Then

            Select Case strReportFormat

                Case "HTML"
                    ExportToHTML()
                Case Else
                    'Do Nothing
            End Select

        End If

    End Sub

    'Export Subs
    Sub ExportToHTML()
	
	    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        DBConStringHelper.PrepareConnection(objConn) 'open the connection

        objCmd = New SqlCommand("[spSelectBatchReport]", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()

        If objDR.Read() Then

            While objDR.Read

                Dim sIncidentID As String
                sIncidentID = objDR("IncidentID")
				Dim oRegularReport As New RegularReport(sIncidentID, strReportFormat)
                strOutput.Append(oRegularReport.gStrTotalReport)
                strOutput.Append("<hr>")
                Response.Write(strOutput.ToString())
            End While
        End If

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()


    End Sub

 

End Class