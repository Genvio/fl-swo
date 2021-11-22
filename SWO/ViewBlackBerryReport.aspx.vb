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

Partial Class ViewBlackBerryReport
    Inherits System.Web.UI.Page

    'Page Load
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim strFullReportToShow As String = ""

        Dim oBlackBerryReport As New BlackBerryReport(Request("IncidentID"))

        If Request("ReportType") = "INITIAL" Then
            strFullReportToShow = oBlackBerryReport.gStrTotalReport
        ElseIf Request("ReportType") = "UPDATE" Then
            strFullReportToShow = oBlackBerryReport.gStrUpdate & oBlackBerryReport.gStrTotalReport
        Else
            strFullReportToShow = oBlackBerryReport.gStrTotalReport
        End If



        Response.Write(strFullReportToShow)
        Response.End()

    End Sub

End Class
