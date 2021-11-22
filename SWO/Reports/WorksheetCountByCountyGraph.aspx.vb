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

Partial Class Reports_WorksheetCountByCountyGraph
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
    Dim strCounty As String

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
        strCounty = Request.QueryString("County")

        LiteralCounty.Text = GetProductSalesChartHtml()
    End Sub

    Public Function GetProductSalesChartHtml() As String
        Dim xmlData As New StringBuilder()

        Try
            'Now we need to convert this data into XML. We can convert this using a string builder.
            Dim ArraySize As Integer = 0

            'Connect and build the datagrid.
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Open the connection.
            DBConStringHelper.PrepareConnection(objConn)
            objCmd = New SqlCommand("[spSelectWorksheetCountByCounty]", objConn)
            objCmd.Parameters.AddWithValue("@County", strCounty)
            objCmd.Parameters.AddWithValue("@AllToDate", Request("AllToDate"))
            objCmd.Parameters.AddWithValue("@StartDate", Request("StartDate"))
            objCmd.Parameters.AddWithValue("@EndDate", Request("EndDate"))

            objCmd.CommandType = CommandType.StoredProcedure

            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then
                'There are records.
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read
                    ArraySize = ArraySize + 1
                End While
            Else

            End If

            'Close the table.
            'strOutput.Append("</table>")

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

            Dim z As Integer = 0
            Dim arrData(ArraySize - 1, 1) As String

            If ArraySize > 0 Then
                '--------------------------------------------------------------------------
                'In this example we plot a single series chart from data contained
                'in an array. The array will have two columns - first one for data label
                'and the next one for data values. Let's store the sales data for 6
                'products in our array. We also store the name of products. 
                '--------------------------------------------------------------------------

                'Connect and build the datagrid.
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Open the connection.
                DBConStringHelper.PrepareConnection(objConn)
                objCmd = New SqlCommand("[spSelectWorksheetCountByCounty]", objConn)
                objCmd.Parameters.AddWithValue("@County", Request("County"))
                objCmd.Parameters.AddWithValue("@AllToDate", Request("AllToDate"))
                objCmd.Parameters.AddWithValue("@StartDate", Request("StartDate"))
                objCmd.Parameters.AddWithValue("@EndDate", Request("EndDate"))

                objCmd.CommandType = CommandType.StoredProcedure

                objDR = objCmd.ExecuteReader()

                If objDR.Read() Then
                    'There are records.
                    objDR.Close()
                    objDR = objCmd.ExecuteReader()

                    While objDR.Read
                        arrData(z, 0) = objDR.Item("IncidentType").ToString()
                        arrData(z, 1) = objDR.Item("Count").ToString()

                        'Dim arrData2(,) As String = arrData.Clone

                        'strXML = strXML & "<set name='" & objDR.Item("IncidentType").ToString() & "' value='" & objDR.Item("Count").ToString() & "' link='" & Server.UrlEncode("Detailed.aspx?FactoryId=" & objDR.Item("IncidentTypeID").ToString()) & "&FactoryName=" & objDR.Item("IncidentType").ToString() & "'/>"

                        z = z + 1
                    End While
                Else
                    'Response.Write("No Reports at this Time.")
                    'Response.End()

                    arrData(z, 0) = "No Results"
                    arrData(z, 1) = "0"
                End If

                'Close the table.
                'strOutput.Append("</table>")

                objCmd.Dispose()
                objCmd = Nothing
                objConn.Close()

                'Initialize <chart> element.
                xmlData.Append("<chart caption='Worksheet Count for County: " & strCounty & "' numberPrefix='' formatNumberScale='0'>")

                'Convert data to XML and append.
                Dim i As Integer

                For i = arrData.GetLowerBound(0) To arrData.GetUpperBound(0)
                    xmlData.Append("<set label='" & arrData(i, 0) & "' value='" & arrData(i, 1) & "' />")
                Next

                'Close <chart> element.
                xmlData.Append("</chart>")
            End If
        Catch ex As Exception
            'Response.Write("No Reports at this Time.")
            'Response.End()
        End Try

        'Create the chart - 3D column chart with data contained in xmlData.
        Return FusionCharts.RenderChart("../FusionCharts/Column3D.swf", "", xmlData.ToString(), "productSales", "950", "300", False, True)
    End Function
End Class