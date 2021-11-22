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
Imports InfoSoftGlobal

Partial Class Reports_IncidentWorksheetGraph
    Inherits System.Web.UI.Page

    'Help functions from our App_Code.
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    'For connecting to the database.
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

    Dim globalHasErrors As Boolean = False
    Dim strStartDate As String
    Dim strEndDate As String
    Dim strAllToDate As String
    Dim strUserID As String
    Dim strUser As String
    Dim strReportFormat As String

    Dim strIncidentTypeID As String
    Dim strIncidentType As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Generate chart in the literal control.
        strStartDate = Request.QueryString("StartDate")
        strEndDate = Request.QueryString("EndDate")
        strAllToDate = Request.QueryString("AllToDate")
        strIncidentTypeID = Request.QueryString("IncidentTypeID")
        strIncidentType = Request.QueryString("IncidentType")
        strReportFormat = Request.QueryString("ReportFormat")

        If strReportFormat = "Excel" Then
            ExportToExcel()
        Else
            FCLiteral.Text = CreateChart()
        End If
    End Sub

    Public Function CreateChart() As String
        'strXML will be used to store the entire XML document generated.
        Dim strXML As String

        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("[spSelectWorkSheetCount]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentTypeID", strIncidentTypeID)
        objCmd.Parameters.AddWithValue("@AllToDate", Request("AllToDate"))
        objCmd.Parameters.AddWithValue("@StartDate", Request("StartDate"))
        objCmd.Parameters.AddWithValue("@EndDate", Request("EndDate"))

        'Generate the graph element.
        If strStartDate <> "" And strEndDate <> "" Then
            Dates.Text = "<font size='4'>" & strStartDate & " to " & strEndDate & "</font>"
            strXML = "<graph decimalPrecision='0' showNames='1' numberSuffix=' instance(s)' pieSliceDepth='30'formatNumberScale='0' >"
        Else
            Dates.Text = "<font size='4'>07/01/2011 to Present</font>"
            strXML = "<graph decimalPrecision='0' showNames='1' numberSuffix=' instance(s)' pieSliceDepth='30'formatNumberScale='0' >"
        End If

        objCmd.CommandType = CommandType.StoredProcedure

        objDR = objCmd.ExecuteReader()

        If objDR.Read() Then
            'There are records.
            objDR.Close()
            objDR = objCmd.ExecuteReader()

            While objDR.Read
                strXML = strXML & "<set name='" & objDR.Item("IncidentType").ToString() & "' value='" & objDR.Item("Count").ToString() & "'/>"

                'This code provides the link. It should be added to the end of the above line if needed.
                '---> ' link='" & Server.UrlEncode("Detailed.aspx?FactoryId=" & objDR.Item("IncidentTypeID").ToString()) & "&FactoryName=" & objDR.Item("IncidentType").ToString() & "'/>"
            End While
        Else

        End If

        'Close the connection.
        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()

        'Close the <graph> element.
        strXML = strXML & "</graph>"

        'Create the chart - 3D pie chart with data from strXML.
        Return FusionCharts.RenderChart("../FusionCharts/Pie3D.swf", "", strXML.ToString(), "FCLiteral1", "1400", "600", False, True)
    End Function

    Public Sub ExportToExcel()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("[spSelectWorkSheetCount]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentTypeID", strIncidentTypeID)
        objCmd.Parameters.AddWithValue("@AllToDate", Request("AllToDate"))
        objCmd.Parameters.AddWithValue("@StartDate", Request("StartDate"))
        objCmd.Parameters.AddWithValue("@EndDate", Request("EndDate"))

        objCmd.CommandType = CommandType.StoredProcedure

        'Send the results to a data adapter.
        objDA = New System.Data.SqlClient.SqlDataAdapter
        objDA.SelectCommand = objCmd

        'Insert data into a dataset.
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()

        DataSetToExcel.Convert(objDS, Response)
        objDS = Nothing
    End Sub
End Class