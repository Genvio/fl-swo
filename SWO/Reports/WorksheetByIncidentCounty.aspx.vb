Imports Microsoft.Office.Interop.Word
Imports Microsoft.Office.Interop.Excel
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

Partial Class Reports_WorksheetByIncidentCounty
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
    Dim strSummation As String

    Dim strIncidentTypeID As String
    Dim strIncidentType As String

    Dim countyArray() As String
    Dim combinedData(29, 29) As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Generate chart(s) in the literal(s).
        strStartDate = Request.QueryString("StartDate")
        strEndDate = Request.QueryString("EndDate")
        strAllToDate = Request.QueryString("AllToDate")
        strIncidentTypeID = Request.QueryString("IncidentTypeID")
        strIncidentType = Request.QueryString("IncidentType")
        strCounty = Request.QueryString("County")
        strReportFormat = Request.QueryString("ReportFormat")
        strSummation = Request.QueryString("Summation")

        'Check to see if there is more than one county being displayed.
        If strCounty.Contains(",") = True Then
            countyArray = strCounty.Split(New Char() {","c})

            Dim literal As String
            Dim i As Integer = 1

            'Check to see if the summation box was checked.
            If strSummation = "True" Then
                If strReportFormat = "Excel" Then
                    Try
                        'Now we need to convert this data into XML. We can convert this using a string builder.
                        Dim ArraySize As Integer = 0

                        'Connect and build the datagrid.
                        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                        'This loop runs for each county being searched.
                        For counter As Integer = 0 To countyArray.Length - 1
                            'Open the connection.
                            DBConStringHelper.PrepareConnection(objConn)
                            objCmd = New SqlCommand("[spSelectWorksheetCountByIncidentCounty]", objConn)
                            objCmd.Parameters.AddWithValue("@IncidentTypeID", strIncidentTypeID)
                            objCmd.Parameters.AddWithValue("@County", countyArray(counter))
                            objCmd.Parameters.AddWithValue("@AllToDate", strAllToDate)
                            objCmd.Parameters.AddWithValue("@StartDate", strStartDate)
                            objCmd.Parameters.AddWithValue("@EndDate", strEndDate)

                            objCmd.CommandType = CommandType.StoredProcedure

                            objDR = objCmd.ExecuteReader()

                            If objDR.HasRows Then
                                'There are records.

                                While objDR.Read
                                    ArraySize = ArraySize + 1
                                End While
                            End If

                            'Close the connection.
                            objCmd.Dispose()
                            objCmd = Nothing
                            objConn.Close()
                        Next

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

                            'This loop runs for each county being searched.
                            For counter As Integer = 0 To countyArray.Length - 1
                                'Open the connection.
                                DBConStringHelper.PrepareConnection(objConn)
                                objCmd = New SqlCommand("[spSelectWorksheetCountByIncidentCounty]", objConn)
                                objCmd.Parameters.AddWithValue("@IncidentTypeID", strIncidentTypeID)
                                objCmd.Parameters.AddWithValue("@County", countyArray(counter))
                                objCmd.Parameters.AddWithValue("@AllToDate", strAllToDate)
                                objCmd.Parameters.AddWithValue("@StartDate", strStartDate)
                                objCmd.Parameters.AddWithValue("@EndDate", strEndDate)

                                objCmd.CommandType = CommandType.StoredProcedure

                                objDR = objCmd.ExecuteReader()

                                If objDR.HasRows Then
                                    'There are records.

                                    While objDR.Read
                                        arrData(z, 0) = objDR.Item("IncidentType").ToString()
                                        arrData(z, 1) = objDR.Item("Count").ToString()

                                        z = z + 1
                                    End While
                                Else
                                    arrData(z, 0) = "No Results"
                                    arrData(z, 1) = "0"
                                End If

                                'Close the connection.
                                objCmd.Dispose()
                                objCmd = Nothing
                                objConn.Close()
                            Next

                            'Convert data to XML and append.
                            combinedData(0, 0) = "Aircraft Incident"
                            combinedData(1, 0) = "Animal or Agricultural"
                            combinedData(2, 0) = "Bomb Threat or Device"
                            combinedData(3, 0) = "Civil Event"
                            combinedData(4, 0) = "Law Enforcement Activity"
                            combinedData(5, 0) = "Dam Failure"
                            combinedData(6, 0) = "DEM Incidents"
                            combinedData(7, 0) = "Drinking Water Facility"
                            combinedData(8, 0) = "Environmental Crime"
                            combinedData(9, 0) = "Fire"
                            combinedData(10, 0) = "General"
                            combinedData(11, 0) = "Geological Event"
                            combinedData(12, 0) = "Hazardous Materials"
                            combinedData(13, 0) = "Kennedy Space Center / Cape Canaveral AFS"
                            combinedData(14, 0) = "Marine Incident"
                            combinedData(15, 0) = "Migration"
                            combinedData(16, 0) = "Military Activity"
                            combinedData(17, 0) = "Nuclear Power Plants"
                            combinedData(18, 0) = "Petroleum Spill"
                            combinedData(19, 0) = "Population Protection Actions"
                            combinedData(20, 0) = "Public Health Medical"
                            combinedData(21, 0) = "Rail Incident"
                            combinedData(22, 0) = "Road Closure or DOT Issue"
                            combinedData(23, 0) = "Search & Rescue"
                            combinedData(24, 0) = "Suspicious Activity"
                            combinedData(25, 0) = "Utility Disruption or Emergency"
                            combinedData(26, 0) = "Vehicle"
                            combinedData(27, 0) = "Wastewater or Effluent Release"
                            combinedData(28, 0) = "Weather Advisories"
                            combinedData(29, 0) = "Weather Reports"

                            For number As Integer = 0 To 29
                                combinedData(number, 1) = "0"
                            Next

                            Dim subTotal As Integer = 0

                            'This loop checks each worksheet type to see if there are any to be added.
                            For v As Integer = 0 To 29
                                'This loop compares the combinedData with the actual data to see if there is anything to increment.
                                For count As Integer = 0 To (arrData.Length / 2) - 1
                                    If combinedData(v, 0) = arrData(count, 0) Then
                                        subTotal += Integer.Parse(combinedData(v, 1)) + Integer.Parse(arrData(count, 1))
                                    End If
                                Next

                                'Put the subtotal in the array.
                                combinedData(v, 1) = subTotal.ToString

                                subTotal = 0
                            Next
                        End If
                    Catch ex As Exception

                    End Try

                    ExportArrayToExcel()
                Else
                    Dim xmlData As New StringBuilder()

                    Try
                        'Now we need to convert this data into XML. We can convert this using a string builder.
                        Dim ArraySize As Integer = 0

                        'Connect and build the datagrid.
                        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                        'This loop runs for each county being searched.
                        For counter As Integer = 0 To countyArray.Length - 1
                            'Open the connection.
                            DBConStringHelper.PrepareConnection(objConn)
                            objCmd = New SqlCommand("[spSelectWorksheetCountByIncidentCounty]", objConn)
                            objCmd.Parameters.AddWithValue("@IncidentTypeID", strIncidentTypeID)
                            objCmd.Parameters.AddWithValue("@County", countyArray(counter))
                            objCmd.Parameters.AddWithValue("@AllToDate", strAllToDate)
                            objCmd.Parameters.AddWithValue("@StartDate", strStartDate)
                            objCmd.Parameters.AddWithValue("@EndDate", strEndDate)

                            objCmd.CommandType = CommandType.StoredProcedure

                            objDR = objCmd.ExecuteReader()

                            If objDR.HasRows Then
                                'There are records.

                                While objDR.Read
                                    ArraySize = ArraySize + 1
                                End While
                            End If

                            'Close the connection.
                            objCmd.Dispose()
                            objCmd = Nothing
                            objConn.Close()
                        Next

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

                            'This loop runs for each county being searched.
                            For counter As Integer = 0 To countyArray.Length - 1
                                'Open the connection.
                                DBConStringHelper.PrepareConnection(objConn)
                                objCmd = New SqlCommand("[spSelectWorksheetCountByIncidentCounty]", objConn)
                                objCmd.Parameters.AddWithValue("@IncidentTypeID", strIncidentTypeID)
                                objCmd.Parameters.AddWithValue("@County", countyArray(counter))
                                objCmd.Parameters.AddWithValue("@AllToDate", strAllToDate)
                                objCmd.Parameters.AddWithValue("@StartDate", strStartDate)
                                objCmd.Parameters.AddWithValue("@EndDate", strEndDate)

                                objCmd.CommandType = CommandType.StoredProcedure

                                objDR = objCmd.ExecuteReader()

                                If objDR.HasRows Then
                                    'There are records.

                                    While objDR.Read
                                        arrData(z, 0) = objDR.Item("IncidentType").ToString()
                                        arrData(z, 1) = objDR.Item("Count").ToString()

                                        z = z + 1
                                    End While
                                Else
                                    arrData(z, 0) = "No Results"
                                    arrData(z, 1) = "0"
                                End If

                                'Close the connection.
                                objCmd.Dispose()
                                objCmd = Nothing
                                objConn.Close()
                            Next

                            Dim counties As String = ""

                            'This loop splits the counties being searched so that they can be displayed legibly.
                            For c As Integer = 0 To countyArray.Length - 1
                                counties = counties & countyArray(c) & ", "
                            Next

                            'Removes the last space and comma from the list of counties.
                            counties = counties.Remove(counties.Length - 2)

                            If Len(counties) - Len(counties.Replace(",", "")) = CInt(System.Configuration.ConfigurationManager.AppSettings("NumberOfFloridaCounties").ToString) - 1 Then
                                counties = "Statewide"
                            Else
                                Dim strRegions As String = ""
                                Dim strCounties As String = counties & ", " 'Need the trailing comma and space back for now

                                If System.Web.HttpContext.Current.Cache("CountyRegions") Is Nothing Then
                                    'We don't have access to the county/region mapping here, so just show all the counties (already in the "counties" variable so do nothing)
                                Else
                                    Dim colCountyRegions As New Dictionary(Of String, String()) 'Stores the counties in each region
                                    colCountyRegions = CType(System.Web.HttpContext.Current.Cache("CountyRegions"), System.Collections.Generic.Dictionary(Of String, String()))

                                    For Each kvp As KeyValuePair(Of String, String()) In colCountyRegions
                                        Dim arrCountiesInThisRegion As String() = kvp.Value
                                        Dim blnAllCountiesInRegion As Boolean = True

                                        For i = 0 To arrCountiesInThisRegion.GetLength(0) - 1
                                            If Not strCounties.Contains(arrCountiesInThisRegion(i) & ",") Then
                                                blnAllCountiesInRegion = False
                                                Exit For
                                            End If
                                        Next

                                        If blnAllCountiesInRegion Then
                                            For i = 0 To arrCountiesInThisRegion.GetLength(0) - 1
                                                If i = 0 Then strRegions = strRegions & kvp.Key & ", "
                                                strCounties = strCounties.Replace(arrCountiesInThisRegion(i) & ", ", "")
                                            Next
                                        End If
                                    Next
                                End If

                                counties = strRegions & strCounties
                                counties = counties.TrimEnd(","c, " "c)
                            End If

                            'Initialize <chart> element.
                            If strStartDate <> "" And strEndDate <> "" Then
                                Dates.Text = "<font size='4'>" & strStartDate & " to " & strEndDate & "</font>"
                                xmlData.Append("<chart caption='" & counties & "' numberPrefix='' formatNumberScale='0'>")
                            Else
                                Dates.Text = "<font size='4'>07/01/2011 to Present</font>"
                                xmlData.Append("<chart caption='" & counties & "' numberPrefix='' formatNumberScale='0'>")
                            End If

                            'Convert data to XML and append.
                            Dim x As Integer

                            combinedData(0, 0) = "Aircraft Incident"
                            combinedData(1, 0) = "Animal or Agricultural"
                            combinedData(2, 0) = "Bomb Threat or Device"
                            combinedData(3, 0) = "Civil Event"
                            combinedData(4, 0) = "Law Enforcement Activity"
                            combinedData(5, 0) = "Dam Failure"
                            combinedData(6, 0) = "DEM Incidents"
                            combinedData(7, 0) = "Drinking Water Facility"
                            combinedData(8, 0) = "Environmental Crime"
                            combinedData(9, 0) = "Fire"
                            combinedData(10, 0) = "General"
                            combinedData(11, 0) = "Geological Event"
                            combinedData(12, 0) = "Hazardous Materials"
                            combinedData(13, 0) = "Kennedy Space Center / Cape Canaveral AFS"
                            combinedData(14, 0) = "Marine Incident"
                            combinedData(15, 0) = "Migration"
                            combinedData(16, 0) = "Military Activity"
                            combinedData(17, 0) = "Nuclear Power Plants"
                            combinedData(18, 0) = "Petroleum Spill"
                            combinedData(19, 0) = "Population Protection Actions"
                            combinedData(20, 0) = "Public Health Medical"
                            combinedData(21, 0) = "Rail Incident"
                            combinedData(22, 0) = "Road Closure or DOT Issue"
                            combinedData(23, 0) = "Search & Rescue"
                            combinedData(24, 0) = "Suspicious Activity"
                            combinedData(25, 0) = "Utility Disruption or Emergency"
                            combinedData(26, 0) = "Vehicle"
                            combinedData(27, 0) = "Wastewater or Effluent Release"
                            combinedData(28, 0) = "Weather Advisories"
                            combinedData(29, 0) = "Weather Reports"

                            For number As Integer = 0 To 29
                                combinedData(number, 1) = "0"
                            Next

                            Dim subTotal As Integer = 0

                            'This loop checks each worksheet type to see if there are any to be added.
                            For v As Integer = 0 To 29
                                'This loop compares the combinedData with the actual data to see if there is anything to increment.
                                For count As Integer = 0 To (arrData.Length / 2) - 1
                                    If combinedData(v, 0) = arrData(count, 0) Then
                                        subTotal += Integer.Parse(combinedData(v, 1)) + Integer.Parse(arrData(count, 1))
                                    End If
                                Next

                                'Put the subtotal in the array.
                                combinedData(v, 1) = subTotal.ToString

                                subTotal = 0
                            Next

                            For x = combinedData.GetLowerBound(0) To combinedData.GetUpperBound(0)
                                xmlData.Append("<set label='" & combinedData(x, 0) & "' value='" & combinedData(x, 1) & "' />")
                            Next

                            'Close <chart> element.
                            xmlData.Append("</chart>")
                        End If
                    Catch ex As Exception

                    End Try

                    'Create the chart - 3D column chart with data contained in xmlData.
                    FCLiteral1.Text = FusionCharts.RenderChart("../FusionCharts/Column3D.swf", "", xmlData.ToString(), "FCLiteral1", "950", "800", False, True)
                End If
            Else
                If strReportFormat = "Excel" Then
                    ExportToExcelMultipleCounties()
                Else
                    'This loop determines which literal to display graphs in.
                    For counter As Integer = 0 To countyArray.Length - 1
                        'Sets which literal to check.
                        literal = "FCLiteral"
                        literal = literal & i.ToString

                        If literal = FCLiteral1.ID Then FCLiteral1.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral2.ID Then FCLiteral2.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral3.ID Then FCLiteral3.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral4.ID Then FCLiteral4.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral5.ID Then FCLiteral5.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral6.ID Then FCLiteral6.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral7.ID Then FCLiteral7.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral8.ID Then FCLiteral8.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral9.ID Then FCLiteral9.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral10.ID Then FCLiteral10.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral11.ID Then FCLiteral11.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral12.ID Then FCLiteral12.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral13.ID Then FCLiteral13.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral14.ID Then FCLiteral14.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral15.ID Then FCLiteral15.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral16.ID Then FCLiteral16.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral17.ID Then FCLiteral17.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral18.ID Then FCLiteral18.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral19.ID Then FCLiteral19.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral20.ID Then FCLiteral20.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral21.ID Then FCLiteral21.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral22.ID Then FCLiteral22.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral23.ID Then FCLiteral23.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral24.ID Then FCLiteral24.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral25.ID Then FCLiteral25.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral26.ID Then FCLiteral26.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral27.ID Then FCLiteral27.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral28.ID Then FCLiteral28.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral29.ID Then FCLiteral29.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral30.ID Then FCLiteral30.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral31.ID Then FCLiteral31.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral32.ID Then FCLiteral32.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral33.ID Then FCLiteral33.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral34.ID Then FCLiteral34.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral35.ID Then FCLiteral35.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral36.ID Then FCLiteral36.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral37.ID Then FCLiteral37.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral38.ID Then FCLiteral38.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral39.ID Then FCLiteral39.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral40.ID Then FCLiteral40.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral41.ID Then FCLiteral41.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral42.ID Then FCLiteral42.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral43.ID Then FCLiteral43.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral44.ID Then FCLiteral44.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral45.ID Then FCLiteral45.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral46.ID Then FCLiteral46.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral47.ID Then FCLiteral47.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral48.ID Then FCLiteral48.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral49.ID Then FCLiteral49.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral50.ID Then FCLiteral50.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral51.ID Then FCLiteral51.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral52.ID Then FCLiteral52.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral53.ID Then FCLiteral53.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral54.ID Then FCLiteral54.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral55.ID Then FCLiteral55.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral56.ID Then FCLiteral56.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral57.ID Then FCLiteral57.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral58.ID Then FCLiteral58.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral59.ID Then FCLiteral59.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral60.ID Then FCLiteral60.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral61.ID Then FCLiteral61.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral62.ID Then FCLiteral62.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral63.ID Then FCLiteral63.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral64.ID Then FCLiteral64.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral65.ID Then FCLiteral65.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral66.ID Then FCLiteral66.Text = CreateBarGraph(literal, countyArray(counter))
                        If literal = FCLiteral67.ID Then FCLiteral67.Text = CreateBarGraph(literal, countyArray(counter))

                        i += 1
                    Next
                End If
            End If
        Else
            If strReportFormat = "Excel" Then
                ExportToExcel()
            Else
                FCLiteral1.Text = CreateBarGraph("FCLiteral1", strCounty)
            End If
        End If
    End Sub

    Public Function CreateBarGraph(ByVal literal As String, ByVal county As String) As String
        Dim xmlData As New StringBuilder()

        Try
            'Now we need to convert this data into XML. We can convert this using a string builder.
            Dim ArraySize As Integer = 0

            'Connect and build the datagrid.
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Open the connection.
            DBConStringHelper.PrepareConnection(objConn)

            objCmd = New SqlCommand("[spSelectWorksheetCountByIncidentCounty]", objConn)
            objCmd.Parameters.AddWithValue("@IncidentTypeID", strIncidentTypeID)
            objCmd.Parameters.AddWithValue("@County", county)
            objCmd.Parameters.AddWithValue("@AllToDate", strAllToDate)
            objCmd.Parameters.AddWithValue("@StartDate", strStartDate)
            objCmd.Parameters.AddWithValue("@EndDate", strEndDate)

            objCmd.CommandType = CommandType.StoredProcedure

            objDR = objCmd.ExecuteReader()

            If objDR.HasRows Then
                'There are records.

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

                objCmd = New SqlCommand("[spSelectWorksheetCountByIncidentCounty]", objConn)
                objCmd.Parameters.AddWithValue("@IncidentTypeID", strIncidentTypeID)
                objCmd.Parameters.AddWithValue("@County", county)
                objCmd.Parameters.AddWithValue("@AllToDate", strAllToDate)
                objCmd.Parameters.AddWithValue("@StartDate", strStartDate)
                objCmd.Parameters.AddWithValue("@EndDate", strEndDate)

                objCmd.CommandType = CommandType.StoredProcedure

                objDR = objCmd.ExecuteReader()

                If objDR.HasRows Then
                    'There are records.

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
                If strStartDate <> "" And strEndDate <> "" Then
                    Dates.Text = "<font size='4'>" & strStartDate & " to " & strEndDate & "</font>"
                    xmlData.Append("<chart caption='" & county & " County' numberPrefix='' formatNumberScale='0'>")
                Else
                    Dates.Text = "<font size='4'>07/01/2011 to Present</font>"
                    xmlData.Append("<chart caption='" & county & " County' numberPrefix='' formatNumberScale='0'>")
                End If

                'Convert data to XML and append.
                'Dim i As Integer

                'For i = arrData.GetLowerBound(0) To arrData.GetUpperBound(0)
                '    xmlData.Append("<set label='" & arrData(i, 0) & "' value='" & arrData(i, 1) & "' />")
                'Next

                Dim x As Integer
                Dim combinedData(29, 29) As String

                combinedData(0, 0) = "Aircraft Incident"
                combinedData(1, 0) = "Animal or Agricultural"
                combinedData(2, 0) = "Bomb Threat or Device"
                combinedData(3, 0) = "Civil Event"
                combinedData(4, 0) = "Law Enforcement Activity"
                combinedData(5, 0) = "Dam Failure"
                combinedData(6, 0) = "DEM Incidents"
                combinedData(7, 0) = "Drinking Water Facility"
                combinedData(8, 0) = "Environmental Crime"
                combinedData(9, 0) = "Fire"
                combinedData(10, 0) = "General"
                combinedData(11, 0) = "Geological Event"
                combinedData(12, 0) = "Hazardous Materials"
                combinedData(13, 0) = "Kennedy Space Center / Cape Canaveral AFS"
                combinedData(14, 0) = "Marine Incident"
                combinedData(15, 0) = "Migration"
                combinedData(16, 0) = "Military Activity"
                combinedData(17, 0) = "Nuclear Power Plants"
                combinedData(18, 0) = "Petroleum Spill"
                combinedData(19, 0) = "Population Protection Actions"
                combinedData(20, 0) = "Public Health Medical"
                combinedData(21, 0) = "Rail Incident"
                combinedData(22, 0) = "Road Closure or DOT Issue"
                combinedData(23, 0) = "Search & Rescue"
                combinedData(24, 0) = "Suspicious Activity"
                combinedData(25, 0) = "Utility Disruption or Emergency"
                combinedData(26, 0) = "Vehicle"
                combinedData(27, 0) = "Wastewater or Effluent Release"
                combinedData(28, 0) = "Weather Advisories"
                combinedData(29, 0) = "Weather Reports"

                For number As Integer = 0 To 29
                    combinedData(number, 1) = "0"
                Next

                Dim subTotal As Integer = 0

                For v As Integer = 0 To 29
                    For count As Integer = 0 To (arrData.Length / 2) - 1
                        If combinedData(v, 0) = arrData(count, 0) Then
                            subTotal += Integer.Parse(combinedData(v, 1)) + Integer.Parse(arrData(count, 1))
                        End If
                    Next

                    combinedData(v, 1) = subTotal.ToString

                    subTotal = 0
                Next

                For x = combinedData.GetLowerBound(0) To combinedData.GetUpperBound(0)
                    xmlData.Append("<set label='" & combinedData(x, 0) & "' value='" & combinedData(x, 1) & "' />")
                Next

                'Close <chart> element.
                xmlData.Append("</chart>")
            End If
        Catch ex As Exception

        End Try

        'Create the chart - 3D column chart with data contained in xmlData.
        Return FusionCharts.RenderChart("../FusionCharts/Column3D.swf", "", xmlData.ToString(), literal, "950", "385", False, True)
    End Function

    Public Sub ExportToExcel()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("[spSelectWorksheetCountByIncidentCounty]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentTypeID", strIncidentTypeID)
        objCmd.Parameters.AddWithValue("@County", strCounty)
        objCmd.Parameters.AddWithValue("@AllToDate", strAllToDate)
        objCmd.Parameters.AddWithValue("@StartDate", strStartDate)
        objCmd.Parameters.AddWithValue("@EndDate", strEndDate)

        objCmd.CommandType = CommandType.StoredProcedure

        'Send the results to a data adapter.
        objDA = New System.Data.SqlClient.SqlDataAdapter
        objDA.SelectCommand = objCmd

        'Insert data into a dataset.
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()

        For i As Integer = 0 To objDS.Tables(0).Rows.Count - 1
            objDS.Tables(0).Rows.Item(i).Item(2) = strCounty
        Next

        DataSetToExcel.Convert(objDS, Response)
        objDS = Nothing
    End Sub

    Public Sub ExportToExcelMultipleCounties()
        Dim rows As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        For counter As Integer = 0 To countyArray.Length - 1
            'Open the connection.
            DBConStringHelper.PrepareConnection(objConn)

            objCmd = New SqlCommand("[spSelectWorksheetCountByIncidentCounty]", objConn)
            objCmd.Parameters.AddWithValue("@IncidentTypeID", strIncidentTypeID)
            objCmd.Parameters.AddWithValue("@County", countyArray(counter))
            objCmd.Parameters.AddWithValue("@AllToDate", strAllToDate)
            objCmd.Parameters.AddWithValue("@StartDate", strStartDate)
            objCmd.Parameters.AddWithValue("@EndDate", strEndDate)

            objCmd.CommandType = CommandType.StoredProcedure

            'Send the results to a data adapter.
            objDA = New System.Data.SqlClient.SqlDataAdapter
            objDA.SelectCommand = objCmd

            'Insert data into a dataset.
            objDA.Fill(objDS)
            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

            For i As Integer = rows To objDS.Tables(0).Rows.Count - 1
                objDS.Tables(0).Rows.Item(i).Item(2) = countyArray(counter)

                rows = i
            Next

            rows += 1
        Next

        DataSetToExcel.Convert(objDS, Response)
        objDS = Nothing
    End Sub

    Public Sub ExportArrayToExcel()
        Dim objExcel As New Excel.Application
        Dim objWorkbook As Excel.Workbook
        Dim objWorksheet As Excel.Worksheet
        Dim counties As String = ""
        Dim cell As String = ""

        'Initialize the workbook as well as the worksheet.
        objWorkbook = objExcel.Workbooks.Add
        objWorksheet = objWorkbook.Worksheets(1)

        'Apply formatting changes.
        objWorksheet.Columns("A").ColumnWidth = 33.71
        objWorksheet.Columns("B").HorizontalAlignment = Excel.Constants.xlCenter
        objWorksheet.Range("A1", "A2").Font.Bold = True
        objWorksheet.Range("A1", "A2").Font.Italic = True
        objWorksheet.Range("A4", "B4").Font.Bold = True
        For Each county As String In countyArray
            counties += county & ", "
        Next
        counties = counties.Remove(counties.Length - 2)

        'Input headers and data.
        If strStartDate <> "" And strEndDate <> "" Then
            objWorksheet.Range("A1").Value = strStartDate & " to " & strEndDate
        Else
            objWorksheet.Range("A1").Value = "07/01/2011 - Present"
        End If
        objWorksheet.Range("A2").Value = counties
        objWorksheet.Range("A4").Value = "Incident Type"
        objWorksheet.Range("B4").Value = "Count"
        objWorksheet.Range("A5").Resize(29, 2).Value = combinedData

        'Convert text fields to numbers.
        For i As Integer = 5 To 33 Step 1
            cell = "B" & i.ToString
            objWorksheet.Range(cell).Value = If(String.IsNullOrEmpty(objWorksheet.Range(cell).Text), 0, CDec(objWorksheet.Range(cell).Text))
        Next

        'Create filename.
        Dim dateCreated As String = Date.Now.ToString("MM/dd/yyyy HH:mm:ss").Replace("/", "-").Replace(":", "_")
        Dim filename As String '= "C:\Summation " & dateCreated ' & ".xls"

        Try
            filename = "Summation_" & dateCreated & ".xlsx"
            Dim path As String = Server.MapPath("~/tempExcelReports/")
            Dim intLength As Int16
            Dim Buffer() As Byte

            If Not Directory.Exists(path) Then
                Directory.CreateDirectory(path)
            End If

            objWorkbook.SaveAs(path & filename)
            objWorksheet = Nothing
            objWorkbook.Close()
            objExcel.Quit()
            objExcel = Nothing


            Response.Clear()
            Response.AddHeader("content-disposition", "attachment; filename=" + filename)
            'Response.ContentType = "application/vnd.ms-excel"
            'Response.ContentType = "application/octet-stream"
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.Buffer = True
            Response.BinaryWrite(File.ReadAllBytes(path & filename))
            'Response.OutputStream.Write(Buffer, 0, intLength - 1)
            File.Delete(path & filename)
            Response.Flush()
            Response.SuppressContent = True
            'Response.End()
            HttpContext.Current.ApplicationInstance.CompleteRequest()

            'An alternative to Response.BinaryWrite(File.ReadAllBytes(path & filename)):
            'Using fs As FileStream = File.OpenRead(path & filename)
            '    intLength = CInt(fs.Length)

            '    Using br As BinaryReader = New BinaryReader(fs)
            '        Buffer = br.ReadBytes(intLength)
            '    End Using

            '    fs.Close()
            '    File.Delete(path & filename)

            '    Response.Clear()
            '    Response.AddHeader("content-disposition", "attachment; filename=" + filename)
            '    'Response.ContentType = "application/vnd.ms-excel"
            '    'Response.ContentType = "application/octet-stream"
            '    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            '    Response.Buffer = True
            '    Response.BinaryWrite(Buffer)
            '    'Response.BinaryWrite(File.ReadAllBytes(path & filename))
            '    'Response.OutputStream.Write(Buffer, 0, intLength - 1)
            '    Response.End()
            '    'Response.Flush()
            'End Using
            'End Alternative

            'Save workbook and clear objects.
            'objExcel.Visible = True
            'objWorkbook.SaveAs(filename, Excel.XlFileFormat.xlXMLSpreadsheet)
            'objWorksheet = Nothing
            'objWorkbook.Close()
            'objExcel.Quit()
            'objExcel = Nothing

            'Set message.
            'Message.Text = "<font size='4'><b>Excel file '" & filename & "' downloaded.  Hit the back button to return to the previous page.</b></font>"

            ''Return to previous page.
            'Response.Redirect("../ReportBuilder.aspx")
        Catch ex As System.Threading.ThreadAbortException
            'Do nothing
            'Response.End() raises this, but removing Response.End() causes the spreadsheet to open with an "Excel found unreadable content" error.
        Catch ex As Exception
            Response.Write(ex)
        Finally
            objWorksheet = Nothing
            objWorkbook = Nothing
            objExcel = Nothing
        End Try
    End Sub
End Class