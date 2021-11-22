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

Partial Class IncidentStatusDisplay
    Inherits System.Web.UI.Page

    'Help Functions from our App_Code.
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp
    Public objDataGridFunctions As New DataGridFunctions

    'For Connecting to the database.
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet

    Dim globalHasErrors As Boolean = False
    Dim globalMessage As String
    'Dim oCookie As System.Web.HttpCookie
    Dim ns As New SecurityValidate

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        Select Case ns.UserLevelID.ToString() 'oCookie.Item("UserLevelID")
            Case "1" 'Admin
            Case "2" 'Full User
            Case "3" 'Update User
            Case "4", "5" 'Read Only and Read Only + Hazmat
            Case Else
        End Select

        lblLastUpdated.Text = "Last Updated: " & Format(Now, "MM-dd-yyyy") & "-" & Format(Now, "HH:mm")

        If Page.IsPostBack = False Then
            PopulateDataGrid()

            IncidentDataGrid.Columns.Item(0).Visible = False
            'IncidentPendingDataGrid.Columns.Item(0).Visible = False
        End If
    End Sub

    Sub PopulateDataGrid()
        '--------------------------------------------------------------
        '  Onclick event of the Go image used to populate the datagrid
        '  and show the datagrid and show the paging button.
        '--------------------------------------------------------------

        IncidentDataGrid.CurrentPageIndex = 0
        getIncident("[Incident].LastUpdated DESC", "", "")
        IncidentDataGrid.AllowSorting = True

        'IncidentPendingDataGrid.CurrentPageIndex = 0
        'getPendingIncident("[Incident].LastUpdated DESC", "", "")
        'IncidentPendingDataGrid.AllowSorting = True
    End Sub

    Sub getIncident(ByVal sSortStr As String, ByVal sSearchBy As String, ByVal sSearchText As String)
        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        'Response.Write(sSortStr.ToString)
        'Response.Write("<br>")
        'Response.Write("SearchBy: " & sSearchBy.ToString)
        'Response.Write("<br>")
        'Response.Write("Searchtext: " & sSearchText.ToString)
        'Response.Write("<br>")
        'Response.End()

        objCmd = New SqlCommand("spFilterIncidentOpenAssignedPending", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@OrderBy", sSortStr.ToString)
        objCmd.Parameters.AddWithValue("@SearchBy", sSearchBy.ToString)
        objCmd.Parameters.AddWithValue("@Searchtext", sSearchText.ToString)

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

        'Commenting this so that mouseover doesn't mess up background colors by late updated
        'objDataGridFunctions.Highlightrows(IncidentDataGrid, "", "", "")

        If CInt(objDS.Tables(0).Rows.Count) <> 0 Then
            'We have records so show the Grid.
            pnlShowIncidentDataGrid.Visible = True
            pnlShowNoIncident.Visible = False
        Else
            'Hide Grid.
            pnlShowIncidentDataGrid.Visible = False
            pnlShowNoIncident.Visible = True
        End If
    End Sub

    Sub getPendingIncident(ByVal sSortStr As String, ByVal sSearchBy As String, ByVal sSearchText As String)
        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        DBConStringHelper.PrepareConnection(objConn) 'open the connection

        'Response.Write(sSortStr.ToString)
        'Response.Write("<br>")
        'Response.Write("SearchBy: " & sSearchBy.ToString)
        'Response.Write("<br>")
        'Response.Write("Searchtext: " & sSearchText.ToString)
        'Response.Write("<br>")
        'Response.End()

        objCmd = New SqlCommand("spFilterIncidentPending", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@OrderBy", sSortStr.ToString)
        objCmd.Parameters.AddWithValue("@SearchBy", sSearchBy.ToString)
        objCmd.Parameters.AddWithValue("@Searchtext", sSearchText.ToString)

        objDS.Tables.Clear()

        'Bind our data.
        objDA = New SqlDataAdapter(objCmd)
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        'Call the calculate grid counts to show the number of records, the page you are on, etc.
        'objDataGridFunctions.CalcDataGridCounts(objDS, IncidentPendingDataGrid, "")

        'Associate the data grid with the data.
        'IncidentPendingDataGrid.DataSource = objDS.Tables(0).DefaultView
        'IncidentPendingDataGrid.DataBind()

        'objDataGridFunctions.Highlightrows(IncidentPendingDataGrid, "", "", "")

        If CInt(objDS.Tables(0).Rows.Count) <> 0 Then
            'We have records so show the Grid.
            'pnlShowIncidentPendingDataGrid.Visible = True
            'pnlShowNoPendingIncident.Visible = False
        Else
            'Hide Grid.
            'pnlShowIncidentPendingDataGrid.Visible = False
            'pnlShowNoPendingIncident.Visible = True
        End If
    End Sub

    Sub SortIncident(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        '--------------------------------------------------------------------------------------------------------
        '  This sub figures out the column you selected and orders by that column...It also adds the image
        '  or takes away the image based on the column you are sorting.
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
        'We run this to parse out the img portion of the header text if there is any....
        'Loop through all the columns parsing out the <img tag if there is one and replacing it with nothing...
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
            'If no mode is specified, Default is descending.
            NewSearchMode = "ASC"
            NewHeaderImg = "&nbsp;<img src='Images/blue_arrow_up2.jpg' align='absmiddle' border=0"
        End If

        '--------------------------------------------------------------------------------------------------------
        '  Derive the new sort expression.
        NewSortExpr = ColumnToSort & " " & NewSearchMode
        '--------------------------------------------------------------------------------------------------------

        ' Figure out the column index
        Dim iIndex As Integer

        Select Case ColumnToSort.ToUpper()
            Case "INCIDENTSTATUS"
                iIndex = 2
                NewHeaderText = "Status"
            Case "NUMBER"
                iIndex = 3
                NewHeaderText = "Incident #"
            Case "INCIDENTNAME"
                iIndex = 4
                NewHeaderText = "Name"
            Case "ADDEDCOUNTY"
                iIndex = 5
                NewHeaderText = "County(s)"
                'Case "DATECREATED"
                '    iIndex = 6
                '    NewHeaderText = "Date Created EST"
            Case "LASTUPDATED"
                iIndex = 6
                NewHeaderText = "Last Updated EST"
        End Select

        '--------------------------------------------------------------------------------------------------------
        'Alter the column's sort expression.
        IncidentDataGrid.Columns(iIndex).SortExpression = NewSortExpr
        'Alter the column's header image.
        IncidentDataGrid.Columns(iIndex).HeaderText = NewHeaderText & NewHeaderImg

        Dim strSearch As String = ""

        If ddlSearchBy.SelectedItem.ToString = "By Incident Number" Then
            strSearch = txtSearch.Text
            'Response.Write(strSearch)
            'Response.Write("<br>")
            'Response.Write(strSearch)
            'Response.Write("<br>")

            Dim intPosition As Integer = InStr(strSearch, "-")

            If intPosition > 0 Then
                strSearch = strSearch.Remove(0, intPosition)
            Else
                strSearch = HelpFunction.Convertdbnulls(txtSearch.Text)
            End If
        Else
            strSearch = HelpFunction.Convertdbnulls(txtSearch.Text)
        End If
        '--------------------------------------------------------------------------------------------------------

        'Sort the data in new order.
        getIncident(NewSortExpr, ddlSearchBy.SelectedItem.Value, strSearch)
    End Sub

    Sub SortIncidentPending(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        '--------------------------------------------------------------------------------------------------------
        '  This sub figures out the column you selected and orders by that column...It also adds the image
        '  or takes away the image based on the column you are sorting.
        '--------------------------------------------------------------------------------------------------------
        Dim SortExprs() As String
        Dim CurrentSearchMode As String
        Dim NewSearchMode As String
        Dim ColumnToSort As String
        Dim NewSortExpr As String
        Dim NewHeaderImg As String = ""
        Dim NewHeaderText As String = ""

        'Parse the sort expression - delimiter space to ignore the ASC.
        SortExprs = Split(e.SortExpression, " ")
        ColumnToSort = SortExprs(0)

        '--------------------------------------------------------------------------------------------------------
        ' We run this to parse out the img portion of the header text if there is any....
        ' Loop through all the columns parsing out the <img tag if there is one and replacing it with nothing...
        'For x = 0 To IncidentPendingDataGrid.Columns.Count - 1
        '    TempHeaderHolder = IncidentPendingDataGrid.Columns(x).HeaderText
        '    FindImg = InStr(TempHeaderHolder, "<img")
        '    If FindImg <> 0 Then
        '        IncidentPendingDataGrid.Columns(x).HeaderText = Left(TempHeaderHolder, FindImg - 1)
        '    End If
        'Next
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
            'If no mode specified, Default is descending.
            NewSearchMode = "ASC"
            NewHeaderImg = "&nbsp;<img src='Images/blue_arrow_up2.jpg' align='absmiddle' border=0"
        End If

        '--------------------------------------------------------------------------------------------------------
        'Derive the new sort expression.
        NewSortExpr = ColumnToSort & " " & NewSearchMode
        '--------------------------------------------------------------------------------------------------------

        'Figure out the column index.
        Dim iIndex As Integer

        Select Case ColumnToSort.ToUpper()
            Case "INCIDENTSTATUS"
                iIndex = 2
                NewHeaderText = "Status"
            Case "NUMBER"
                iIndex = 3
                NewHeaderText = "Incident #"
            Case "INCIDENTNAME"
                iIndex = 4
                NewHeaderText = "Incident Name"
            Case "ADDEDCOUNTY"
                iIndex = 5
                NewHeaderText = "County"
                'Case "DATECREATED"
                '    iIndex = 6
                '    NewHeaderText = "Date Created EST"
            Case "LASTUPDATED"
                iIndex = 7
                NewHeaderText = "Last Updated EST"
                'Case "UPDATEDBY"
                '    iIndex = 8
                '    NewHeaderText = "Updated By"
        End Select

        '--------------------------------------------------------------------------------------------------------
        'Alter the column's sort expression.
        'IncidentPendingDataGrid.Columns(iIndex).SortExpression = NewSortExpr

        'alter the column's header image'
        'IncidentPendingDataGrid.Columns(iIndex).HeaderText = NewHeaderText & NewHeaderImg

        Dim strSearch As String = ""

        'If ddlSearchByPending.SelectedItem.ToString = "By Incident Number" Then
        '    strSearch = txtSearchPending.Text
        '    'Response.Write(strSearch)
        '    'Response.Write("<br>")
        '    'Response.Write(strSearch)
        '    'Response.Write("<br>")
        '    Dim intPosition As Integer = InStr(strSearch, "-")

        '    If intPosition > 0 Then
        '        strSearch = strSearch.Remove(0, intPosition)
        '    Else
        '        strSearch = HelpFunction.Convertdbnulls(txtSearchPending.Text)
        '    End If
        'Else
        '    strSearch = HelpFunction.Convertdbnulls(txtSearchPending.Text)
        'End If
        '--------------------------------------------------------------------------------------------------------

        'Sort the data in new order.
        'getPendingIncident(NewSortExpr, ddlSearchByPending.SelectedItem.Value, strSearch)
    End Sub


    Sub IncidentDataGrid_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs)
        '---------------------------------------------------------------------------------------------------------
        'This sub is called on the next and previous clicks for the recordset, it cycles to the next 20 records.
        '---------------------------------------------------------------------------------------------------------

        IncidentDataGrid.CurrentPageIndex = e.NewPageIndex
        IncidentDataGrid.DataBind()

        'Response.End()

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
                If FindAsc <> 0 Then 'sort desc
                    strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "ASC") - 1) & " ASC"
                Else 'sort asc
                    strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "DESC") - 1) & " DESC"
                End If
                Exit For
            End If
        Next

        If strSort = "" Then strSort = "Incident.[IncidentID] ASC"

        getIncident("", ddlSearchBy.SelectedItem.Value, HelpFunction.Convertdbnulls(txtSearch.Text))
    End Sub

    Sub IncidentPendingDataGrid_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs)
        '---------------------------------------------------------------------------------------------------------
        '   This sub is called on the next and previous clicks for the recordset, it cycles to the next 20 records
        '---------------------------------------------------------------------------------------------------------

        'IncidentPendingDataGrid.CurrentPageIndex = e.NewPageIndex
        'IncidentPendingDataGrid.DataBind()

        'Response.End()

        Dim CurrentSearchMode As String = ""
        Dim NewSearchMode As String = ""
        Dim NewHeaderImg As String = ""
        Dim strSort As String = ""

        'For x = 0 To IncidentPendingDataGrid.Columns.Count - 1
        '    FindImg = InStr(IncidentPendingDataGrid.Columns(x).HeaderText, "<img") 'find the column with the <img tag
        '    If FindImg <> 0 Then
        '        TempSortHolder = IncidentPendingDataGrid.Columns(x).SortExpression
        '        FindAsc = InStr(TempSortHolder, "ASC")
        '        If FindAsc <> 0 Then
        '            'Sort desc.
        '            strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "ASC") - 1) & " ASC"
        '        Else
        '            'Sort asc.
        '            strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "DESC") - 1) & " DESC"
        '        End If
        '        Exit For
        '    End If
        'Next

        If strSort = "" Then strSort = "Incident.[IncidentID] ASC"

        'getPendingIncident("", ddlSearchByPending.SelectedItem.Value, HelpFunction.Convertdbnulls(txtSearchPending.Text))
    End Sub

    Protected Sub btnSearch_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnSearch.Command
        If ddlSearchBy.SelectedItem.ToString = "By Date Created" Or ddlSearchBy.SelectedItem.ToString = "By Last Updated" Then
            ErrorChecks()

            If globalHasErrors = True Then
                'If we have errors, Show Message and Exit Sub. No Insert of Record.
                pnlMessage.Visible = True
                'pnlMessage2.Visible = True
                globalHasErrors = False

                Exit Sub
            Else
                Dim strSearch As String = txtSearch.Text

                strSearch = HelpFunction.ChangeFormatDate(CDate(strSearch), "MM/dd/yyyy")

                'Searches by the dropdown value and search text.
                IncidentDataGrid.CurrentPageIndex = 0
                getIncident("[IncidentNumber] DESC", ddlSearchBy.SelectedItem.Value, strSearch)
            End If
        Else
            Dim strSearch As String = ""

            If ddlSearchBy.SelectedItem.ToString = "By Incident Number" Then
                strSearch = txtSearch.Text
                'Response.Write(strSearch)
                'Response.Write("<br>")
                'Response.Write(strSearch)
                'Response.Write("<br>")

                Dim intPosition As Integer = InStr(strSearch, "-")

                If intPosition > 0 Then
                    strSearch = strSearch.Remove(0, intPosition)
                Else
                    strSearch = HelpFunction.Convertdbnulls(txtSearch.Text)
                End If
            Else
                strSearch = HelpFunction.Convertdbnulls(txtSearch.Text)
            End If

            'Searches by the dropdown value and search text.
            IncidentDataGrid.CurrentPageIndex = 0
            getIncident("[IncidentNumber] DESC", ddlSearchBy.SelectedItem.Value, strSearch)
        End If
    End Sub

    'Protected Sub btnSearchPending_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnSearchPending.Command

    '    If ddlSearchByPending.SelectedItem.ToString = "By Date Created" Or ddlSearchByPending.SelectedItem.ToString = "By Last Updated" Then
    '        ErrorChecks2()

    '        If globalHasErrors = True Then
    '            'If we have errors, Show Message and Exit Sub. No Insert of Record.
    '            pnlMessage.Visible = True
    '            'pnlMessage2.Visible = True
    '            globalHasErrors = False

    '            Exit Sub
    '        Else
    '            Dim strSearch As String = txtSearchPending.Text

    '            strSearch = HelpFunction.ChangeFormatDate(CDate(strSearch), "MM/dd/yyyy")

    '            'Searches by the dropdown value and search text.
    '            IncidentPendingDataGrid.CurrentPageIndex = 0
    '            getPendingIncident("[IncidentNumber] DESC", ddlSearchByPending.SelectedItem.Value, strSearch)
    '        End If
    '    Else
    '        Dim strSearch As String = ""

    '        If ddlSearchByPending.SelectedItem.ToString = "By Incident Number" Then
    '            strSearch = txtSearchPending.Text
    '            'Response.Write(strSearch)
    '            'Response.Write("<br>")
    '            'Response.Write(strSearch)
    '            'Response.Write("<br>")

    '            Dim intPosition As Integer = InStr(strSearch, "-")

    '            If intPosition > 0 Then
    '                strSearch = strSearch.Remove(0, intPosition)
    '            Else
    '                strSearch = HelpFunction.Convertdbnulls(txtSearchPending.Text)
    '            End If
    '        Else
    '            strSearch = HelpFunction.Convertdbnulls(txtSearchPending.Text)
    '        End If

    '        'Searches by the dropdown value and search text.
    '        IncidentPendingDataGrid.CurrentPageIndex = 0
    '        getPendingIncident("[IncidentNumber] DESC", ddlSearchByPending.SelectedItem.Value, strSearch)
    '    End If
    'End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start The Error String.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        If IsDate(txtSearch.Text) = False Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Vaild Date. <br />")
            globalHasErrors = True
        End If

        'Finish the Error String.
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels.
        lblMessage.Text = strError.ToString
    End Sub

    Protected Sub ErrorChecks2()
        Dim strError As New System.Text.StringBuilder

        'Start The Error String.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        'If IsDate(txtSearchPending.Text) = False Then
        '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Vaild Date. <br />")
        '    globalHasErrors = True
        'End If

        'Finish the Error String.
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels.
        lblMessage.Text = strError.ToString
    End Sub

    Protected Sub IncidentDataGrid_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles IncidentDataGrid.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim strCounties As String = e.Item.Cells(5).Text

            If Not String.IsNullOrEmpty(strCounties) Then
                If Len(strCounties) - Len(strCounties.Replace(",", "")) = CInt(System.Configuration.ConfigurationManager.AppSettings("NumberOfFloridaCounties").ToString) - 1 Then
                    e.Item.Cells(5).Text = "<b>Statewide</b>"
                Else
                    Dim oCountyRegion As New CountyRegion(CType(e.Item.DataItem, DataRowView).Row.ItemArray(7).ToString())
                    Dim strCountyRegion As String = ""
                    strCountyRegion = oCountyRegion.GetRegionAndCountyList(True)
                    e.Item.Cells(5).Text = strCountyRegion
                End If
            End If

            'The following line replaces the call to objDataGridFunctions.Highlightrows in getIncident
            If e.Item.ItemIndex Mod 2 = 0 Then e.Item.BackColor = Drawing.ColorTranslator.FromHtml("#f7f7f7")

            If IsDate(e.Item.Cells(6).Text) Then
                Dim dateUpdated As DateTime = CDate(e.Item.Cells(6).Text)
                Dim localZone As TimeZone = TimeZone.CurrentTimeZone
                Dim tsHourDiff As TimeSpan = Now - dateUpdated + localZone.GetUtcOffset(Now)

                If tsHourDiff.Days = 0 Then
                    Select Case tsHourDiff.Hours
                        Case 1
                            e.Item.BackColor = Drawing.Color.Yellow
                        Case 2 To 14
                            e.Item.BackColor = Drawing.Color.Red
                        Case Else
                            'No background color change
                    End Select
                End If
            Else
                'No background color change
            End If
        End If
    End Sub
End Class