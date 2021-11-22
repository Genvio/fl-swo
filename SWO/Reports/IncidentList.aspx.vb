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
Imports System.Xml


Partial Class IncidentList
    Inherits System.Web.UI.Page
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp
    Public objDataGridFunctions As New DataGridFunctions

    'For connecting to the database.
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet
    Public EOCID As Int32



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        EOCID = Request.QueryString("EOCID")
        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("spIncidentFilterEOCID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@EOCID", EOCID)




        objDS.Tables.Clear()

        'Bind our data.
        objDA = New SqlDataAdapter(objCmd)
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        'Call the calculate grid counts to show the number of records, the page you are on, etc.
        objDataGridFunctions.CalcDataGridCounts(objDS, IncidentDataGrid, "")

        'Associate the data grid with the data.
        IncidentDataGrid.DataSource = objDS.Tables(0).DefaultView
        IncidentDataGrid.DataBind()

        objDataGridFunctions.Highlightrows(IncidentDataGrid, "", "", "")
    End Sub

    Sub SortIncident(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        '--------------------------------------------------------------------------------------------------------
        'This sub figures out the column you selected and orders by that column...It also adds the image
        'or takes away the image based on the column you are sorting.
        '--------------------------------------------------------------------------------------------------------
        Dim SortExprs() As String
        Dim CurrentSearchMode As String
        Dim NewSearchMode As String
        Dim ColumnToSort As String
        Dim NewSortExpr As String
        Dim NewHeaderImg As String = ""
        Dim NewHeaderText As String = ""
        Dim x As Integer
        Dim TempHeaderHolder As String
        Dim FindImg As Integer

        'Parse the sort expression - delimiter space to ignore the ASC.
        SortExprs = Split(e.SortExpression, " ")
        ColumnToSort = SortExprs(0)

        '--------------------------------------------------------------------------------------------------------
        'We run this to parse out the img portion of the header text if there is any..
        'Loop through all the columns parsing out the <img tag if there is one and replacing it with nothing..
        For x = 0 To IncidentDataGrid.Columns.Count - 1
            TempHeaderHolder = IncidentDataGrid.Columns(x).HeaderText
            FindImg = InStr(TempHeaderHolder, "<img")
            If FindImg <> 0 Then
                IncidentDataGrid.Columns(x).HeaderText = Left(TempHeaderHolder, FindImg - 1)
            End If
        Next
        '--------------------------------------------------------------------------------------------------------

        'If a sort order is specified get it, else default is descending.
        If SortExprs.Length() > 1 Then
            CurrentSearchMode = SortExprs(1).ToUpper()
            If CurrentSearchMode = "ASC" Then
                NewSearchMode = "DESC"
                NewHeaderImg = "&nbsp;<img src='Images/blue_arrow_down2.jpg' align='absmiddle' border=0"
            Else
                NewSearchMode = "ASC"
                NewHeaderImg = "&nbsp;<img src='Images/blue_arrow_up2.jpg' align='absmiddle' border=0"
            End If
        Else
            'If no mode specified, default is descending.
            NewSearchMode = "DESC"
            NewHeaderImg = "&nbsp;<img src='Images/blue_arrow_down2.jpg' align='absmiddle' border=0"
        End If

        'Derive the new sort expression.
        NewSortExpr = ColumnToSort & " " & NewSearchMode

        'Figure out the column index.
        Dim iIndex As Integer

        Select Case ColumnToSort.ToUpper()
            Case "INCIDENTID"
                iIndex = 3
                NewHeaderText = "Incident #"
            Case "INCIDENTSTATUS"
                iIndex = 4
                NewHeaderText = "Status"
            Case "INCIDENTNAME"
                iIndex = 5
                NewHeaderText = "Incident Name"
            Case "ADDEDCOUNTY"
                iIndex = 6
                NewHeaderText = "County"
            Case "DATECREATEDSORT"
                iIndex = 7
                NewHeaderText = "Date Created EST"
            Case "LASTUPDATEDSORT"
                iIndex = 8
                NewHeaderText = "Last Updated EST"
                'Case "UPDATEDBY"
                '    iIndex = 9
                '    NewHeaderText = "Updated By"
        End Select

        '--------------------------------------------------------------------------------------------------------
        'Alter the column's sort expression.
        IncidentDataGrid.Columns(iIndex).SortExpression = NewSortExpr

        'Alter the column's header image.
        IncidentDataGrid.Columns(iIndex).HeaderText = NewHeaderText & NewHeaderImg

        Dim strSearch As String = ""


        '--------------------------------------------------------------------------------------------------------

        'Sort the data in new order. Searches by provided.
        ' getIncident(NewSortExpr, ddlSearchBy.SelectedItem.Value, strSearch)
    End Sub

    Sub IncidentDataGrid_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs)
        '---------------------------------------------------------------------------------------------------------
        ' This sub is called on the next and previous clicks for the recordset, it cycles to the next 20 records.
        '---------------------------------------------------------------------------------------------------------
        IncidentDataGrid.CurrentPageIndex = e.NewPageIndex
        IncidentDataGrid.DataBind()

        Dim x As Integer
        Dim TempSortHolder As String
        Dim FindImg As Integer
        Dim FindAsc As Integer
        Dim CurrentSearchMode As String = ""
        Dim NewSearchMode As String = ""
        Dim NewHeaderImg As String = ""
        Dim strSort As String = ""

        For x = 0 To IncidentDataGrid.Columns.Count - 1
            FindImg = InStr(IncidentDataGrid.Columns(x).HeaderText, "<img") 'find the column with the <img tag

            If FindImg <> 0 Then
                TempSortHolder = IncidentDataGrid.Columns(x).SortExpression
                FindAsc = InStr(TempSortHolder, "ASC")

                If FindAsc <> 0 Then
                    'Sort desc.
                    strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "ASC") - 1) & " ASC"
                Else
                    'Sort asc.
                    strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "DESC") - 1) & " DESC"
                End If
                Exit For
            End If
        Next

        If strSort = "" Then strSort = "Incident.[IncidentID] ASC"

        'getIncident("", ddlSearchBy.SelectedItem.Value, HelpFunction.Convertdbnulls(txtSearch.Text))
    End Sub
End Class
