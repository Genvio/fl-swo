Imports Microsoft.VisualBasic
Public Class DataGridFunctions
    'commonly used datagrid functions

    Sub CalcDataGridCounts(ByRef dset As System.Data.DataSet, ByRef dgrid As System.Web.UI.WebControls.DataGrid, ByVal TableName As String)
        '---------------------------------------------------------------------------------------------------------
        ' This sub calculates the number of records, the page you are on, etc.
        ' sets:   RowCount - the total number of rows for this recordset
        '         CurrentPageIndex - the current page you are on ex: 1 of 10
        '         PageCount - the total number of pages
        '         BottCount - the bottom records you are on
        '         TopCount - the top records you are on
        ' ex:
        ' call the calculate grid counts to show the number of records, the page you are on, etc.
        ' GeneralFunctionsClass.CalcDataGridCounts(ObjDS1, ListingDataGrid, "TableName")
        '---------------------------------------------------------------------------------------------------------
        'Initialize all variables:
        Dim RowCount As Integer
        Dim CurrentPageIndex As Integer
        Dim PageCount As Integer
        Dim BottCount As Integer
        Dim TopCount As Integer

        RowCount = 0
        CurrentPageIndex = 0
        PageCount = 0
        BottCount = 0
        TopCount = 0

        'Grab the total row count for the dataset
        If TableName <> "" Then
            RowCount = dset.Tables(TableName).Rows.Count
        Else 'count the first table
            RowCount = dset.Tables(0).Rows.Count
        End If

        'Grab which page we are on ex: 1 out of 10
        CurrentPageIndex = dgrid.CurrentPageIndex + 1
        'Figure out the top count based on the CurrentPageIndex and PageSize (number of items to display per page)
        TopCount = CurrentPageIndex * dgrid.PageSize
        'Figure out the bottom count based on the Top count minus the pagesize
        BottCount = (TopCount - dgrid.PageSize) + 1
        'Figure out the pagecount, using \ to drop the remainder...
        ' if there is a remainder (mod) then add 1 to the rowcount/pagesize...
        If (CInt(RowCount) Mod CInt(dgrid.PageSize)) > 0 Then
            PageCount = (CInt(RowCount) \ CInt(dgrid.PageSize)) + 1
        Else
            PageCount = (CInt(RowCount) \ CInt(dgrid.PageSize))
        End If
        'Check if we are at the end of the recordset, if we are then set the topcount to the rowcount
        If RowCount < TopCount Then
            TopCount = RowCount
        End If

        'Format the PrevPageText, and NextPageText to reflect the calculated totals
        dgrid.PagerStyle.PrevPageText = "<img src='Images/prevarrow.gif' border=0>&nbsp;&nbsp;" & _
                                          "Previous&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & _
                                          "<font color='Black'>Page<b>&nbsp;" & CurrentPageIndex & _
                                          "&nbsp;</b>of<b>&nbsp;" & PageCount & "&nbsp;</b></font>"

        dgrid.PagerStyle.NextPageText = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & _
                                          "<font color='Black'>Rows<b>&nbsp;" & BottCount & _
                                          "&nbsp;</b>through<b>&nbsp;" & TopCount & "&nbsp;</b>" & _
                                          "&nbsp;of<b>&nbsp;" & RowCount & "&nbsp;</b></font>" & _
                                          "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Next" & _
                                          "&nbsp;&nbsp;<img src='Images/nextarrow.gif' border=0>"



    End Sub



    Sub CalcDataGridCounts(ByRef listCount As Integer, ByRef dgrid As System.Web.UI.WebControls.DataGrid, ByVal TableName As String)
        '---------------------------------------------------------------------------------------------------------
        ' This sub calculates the number of records, the page you are on, etc.
        ' sets:   RowCount - the total number of rows for this recordset
        '         CurrentPageIndex - the current page you are on ex: 1 of 10
        '         PageCount - the total number of pages
        '         BottCount - the bottom records you are on
        '         TopCount - the top records you are on
        ' ex:
        ' call the calculate grid counts to show the number of records, the page you are on, etc.
        ' GeneralFunctionsClass.CalcDataGridCounts(ObjDS1, ListingDataGrid, "TableName")
        '---------------------------------------------------------------------------------------------------------
        'Initialize all variables:
        Dim RowCount As Integer
        Dim CurrentPageIndex As Integer
        Dim PageCount As Integer
        Dim BottCount As Integer
        Dim TopCount As Integer

        RowCount = listCount
        CurrentPageIndex = 0
        PageCount = 0
        BottCount = 0
        TopCount = 0

        'Grab the total row count for the dataset


        'Grab which page we are on ex: 1 out of 10
        CurrentPageIndex = dgrid.CurrentPageIndex + 1
        'Figure out the top count based on the CurrentPageIndex and PageSize (number of items to display per page)
        TopCount = CurrentPageIndex * dgrid.PageSize
        'Figure out the bottom count based on the Top count minus the pagesize
        BottCount = (TopCount - dgrid.PageSize) + 1
        'Figure out the pagecount, using \ to drop the remainder...
        ' if there is a remainder (mod) then add 1 to the rowcount/pagesize...
        If (CInt(RowCount) Mod CInt(dgrid.PageSize)) > 0 Then
            PageCount = (CInt(RowCount) \ CInt(dgrid.PageSize)) + 1
        Else
            PageCount = (CInt(RowCount) \ CInt(dgrid.PageSize))
        End If
        'Check if we are at the end of the recordset, if we are then set the topcount to the rowcount
        If RowCount < TopCount Then
            TopCount = RowCount
        End If

        'Format the PrevPageText, and NextPageText to reflect the calculated totals
        dgrid.PagerStyle.PrevPageText = "<img src='" & ConfigurationManager.AppSettings("FilePath") & "/Images/prevarrow.gif'" & "border=0>&nbsp;&nbsp;" & _
                                          "Previous&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & _
                                          "<font color='Black'>Page<b>&nbsp;" & CurrentPageIndex & _
                                          "&nbsp;</b>of<b>&nbsp;" & PageCount & "&nbsp;</b></font>"

        dgrid.PagerStyle.NextPageText = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & _
                                          "<font color='Black'>Rows<b>&nbsp;" & BottCount & _
                                          "&nbsp;</b>through<b>&nbsp;" & TopCount & "&nbsp;</b>" & _
                                          "&nbsp;of<b>&nbsp;" & RowCount & "&nbsp;</b></font>" & _
                                          "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Next" & _
                                          "&nbsp;&nbsp;" & "<img src='" & ConfigurationManager.AppSettings("FilePath") & "/Images/nextarrow.gif'" & "border=0>"




    End Sub

    Sub CalcDataGridCounts(ByRef count As Integer, ByRef dgrid As System.Web.UI.WebControls.DataGrid)
        '---------------------------------------------------------------------------------------------------------
        ' This sub calculates the number of records, the page you are on, etc.
        ' sets:   RowCount - the total number of rows for this recordset
        '         CurrentPageIndex - the current page you are on ex: 1 of 10
        '         PageCount - the total number of pages
        '         BottCount - the bottom records you are on
        '         TopCount - the top records you are on
        ' ex:
        ' call the calculate grid counts to show the number of records, the page you are on, etc.
        ' GeneralFunctionsClass.CalcDataGridCounts(ObjDS1, ListingDataGrid, "TableName")
        '---------------------------------------------------------------------------------------------------------
        'Initialize all variables:
        Dim RowCount As Integer
        Dim CurrentPageIndex As Integer
        Dim PageCount As Integer
        Dim BottCount As Integer
        Dim TopCount As Integer

        RowCount = 0
        CurrentPageIndex = 0
        PageCount = 0
        BottCount = 0
        TopCount = 0

        'Grab the total row count for the List

        RowCount = count


        'Grab which page we are on ex: 1 out of 10
        CurrentPageIndex = dgrid.CurrentPageIndex + 1
        'Figure out the top count based on the CurrentPageIndex and PageSize (number of items to display per page)
        TopCount = CurrentPageIndex * dgrid.PageSize
        'Figure out the bottom count based on the Top count minus the pagesize
        BottCount = (TopCount - dgrid.PageSize) + 1
        'Figure out the pagecount, using \ to drop the remainder...
        ' if there is a remainder (mod) then add 1 to the rowcount/pagesize...
        If (CInt(RowCount) Mod CInt(dgrid.PageSize)) > 0 Then
            PageCount = (CInt(RowCount) \ CInt(dgrid.PageSize)) + 1
        Else
            PageCount = (CInt(RowCount) \ CInt(dgrid.PageSize))
        End If
        'Check if we are at the end of the recordset, if we are then set the topcount to the rowcount
        If RowCount < TopCount Then
            TopCount = RowCount
        End If

        'Format the PrevPageText, and NextPageText to reflect the calculated totals
        'Format the PrevPageText, and NextPageText to reflect the calculated totals
        dgrid.PagerStyle.PrevPageText = "<img src='" & ConfigurationManager.AppSettings("FilePath") & "/Images/prevarrow.gif'" & "border=0>&nbsp;&nbsp;" & _
                                         "Previous&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & _
                                         "<font color='Black'>Page<b>&nbsp;" & CurrentPageIndex & _
                                         "&nbsp;</b>of<b>&nbsp;" & PageCount & "&nbsp;</b></font>"

        dgrid.PagerStyle.NextPageText = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & _
                                          "<font color='Black'>Rows<b>&nbsp;" & BottCount & _
                                          "&nbsp;</b>through<b>&nbsp;" & TopCount & "&nbsp;</b>" & _
                                          "&nbsp;of<b>&nbsp;" & RowCount & "&nbsp;</b></font>" & _
                                          "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Next" & _
                                          "&nbsp;&nbsp;" & "<img src='" & ConfigurationManager.AppSettings("FilePath") & "/Images/nextarrow.gif'" & "border=0>"


    End Sub

    Sub Highlightrows(ByVal dgrid As System.Web.UI.WebControls.DataGrid, ByVal mouseoncolor As String, ByVal mouseoffcolor As String, ByVal altcolor As String)
        '--------------------------------------------------------------------------------------------------------
        'Loop through all the rows in the output datagrid table, and add the javascript to highlight the rows...
        '
        '
        ' ex:
        ' cycle through the rows of the grid and add the highlighting feature to the row...
        ' GeneralFunctionsClass.Highlightrows(ListingDataGrid, "#ffff80", "#ffffff", "#ededed")
        '--------------------------------------------------------------------------------------------------------
        If mouseoncolor = "" Then
            mouseoncolor = "#eeeeef"
        End If

        If altcolor = "" Then
            altcolor = "#f7f7f7"
        End If

        If mouseoffcolor = "" Then
            mouseoffcolor = ""
        End If

        Dim bgcolorcounter As Integer
        Dim item As System.Web.UI.WebControls.DataGridItem
        For Each item In dgrid.Items
            'Grab the id of the current item, and check to see if the id exsists in the
            ' table, if it does, we cannot delete this record so set the text to ""
            If bgcolorcounter Mod 2 Then
                item.BackColor = Drawing.ColorTranslator.FromHtml("#f7f7f7")
            Else
            End If

            bgcolorcounter = bgcolorcounter + 1
        Next item


        Dim bgcolorcounter2 As Integer
        Dim item2 As System.Web.UI.WebControls.DataGridItem
        For Each item2 In dgrid.Items
            'Grab the id of the current item, and check to see if the id exsists in the
            ' table, if it does, we cannot delete this record so set the text to ""
            item2.Attributes.Add("onmouseover", "javascript:this.style.backgroundColor='" & mouseoncolor & "';")
            If bgcolorcounter2 Mod 2 Then
                item2.Attributes.Add("onmouseout", "javascript:this.style.backgroundColor='" & altcolor & "';")
            Else
                item2.Attributes.Add("onmouseout", "javascript:this.style.backgroundColor='" & mouseoffcolor & "';")
            End If

            bgcolorcounter2 = bgcolorcounter2 + 1
        Next item2

        '-------------------------------------------------------------------------------------------------------

    End Sub

    Public Shared Function SortDataGrid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs, ByRef objDgrid As DataGrid) As String
        '--------------------------------------------------------------------------------------------------------
        '  This sub figures out the column you selected and orders by that column...It also adds the image
        '  or takes away the image based on the column you are sorting
        '--------------------------------------------------------------------------------------------------------
        Dim SortExprs() As String
        Dim Header() As String
        Dim CurrentSearchMode As String
        Dim NewSearchMode As String
        Dim ColumnToSort As String
        Dim NewSortExpr As String
        Dim NewHeaderImg As String = ""
        Dim NewHeaderText As String = ""
        Dim x As Integer
        Dim TempHeaderHolder As String
        Dim FindImg As Integer

        '  Parse the sort expression - delimiter space to ignore the ASC
        SortExprs = Split(e.SortExpression, " ")
        ColumnToSort = SortExprs(0)


        'get this columns index
        Dim pos As Integer = GetDatGridColumnNumber(objDgrid, e.SortExpression)
        Header = Split(GetHeaderText(objDgrid, e.SortExpression))

        '--------------------------------------------------------------------------------------------------------
        ' We run this to parse out the img portion of the header text if there is any....
        ' Loop through all the columns parsing out the <img tag if there is one and replacing it with nothing...
        For x = 0 To objDgrid.Columns.Count - 1
            TempHeaderHolder = objDgrid.Columns(x).HeaderText
            FindImg = InStr(TempHeaderHolder, "<img")
            If FindImg <> 0 Then
                'get the header minus the <img stuff>
                objDgrid.Columns(x).HeaderText = Left(TempHeaderHolder, FindImg - 1)

            End If
        Next

        ' If a sort order is specified get it, else default is descending
        If SortExprs.Length() > 1 Then
            CurrentSearchMode = SortExprs(1).ToUpper()
            If CurrentSearchMode = "ASC" Then
                NewSearchMode = "DESC"
                NewHeaderImg = " "
            Else
                NewSearchMode = "ASC"
                NewHeaderImg = " "
            End If
        Else   ' If no mode specified, Default is descending
            NewSearchMode = "ASC"
            NewHeaderImg = " "
        End If
        '--------------------------------------------------------------------------------------------------------
        '  Derive the new sort expression.
        NewSortExpr = ColumnToSort & " " & NewSearchMode
        '--------------------------------------------------------------------------------------------------------
        ' Figure out the column index
        'maybe this is x

        '--------------------------------------------------------------------------------------------------------
        ' alter the column's sort expression
        objDgrid.Columns(pos).SortExpression = NewSortExpr
        'alter the column's header image
        objDgrid.Columns(pos).HeaderText = ""
        objDgrid.Columns(pos).HeaderText = Header(0).ToString() & NewHeaderText & NewHeaderImg

        '--------------------------------------------------------------------------------------------------------
        ' Sort the data in new order
        'searches by provided
        Return NewSortExpr
    End Function

    Public Shared Function GetDatGridColumnNumber(ByVal objDGrid As DataGrid, ByVal SortExpression As String) As Integer
        Dim pos As Integer = 0
        Dim objColumn As DataGridColumn

        For Each objColumn In objDGrid.Columns
            If objColumn.SortExpression = SortExpression Then
                Return pos
            End If
            pos = pos + 1
        Next

        Return Nothing
    End Function

    Public Shared Function GetHeaderText(ByVal objDGrid As DataGrid, ByVal SortExpression As String) As String
        Dim pos As Integer = 0
        Dim objColumn As DataGridColumn

        For Each objColumn In objDGrid.Columns
            If objColumn.SortExpression = SortExpression Then
                Return objColumn.HeaderText
            End If
        Next

        Return Nothing
    End Function

    Public Shared Function dgrid_PageIndexChanged(ByRef sender As Object, ByRef e As DataGridPageChangedEventArgs, ByRef objDgrid As DataGrid, ByVal defaultSort As String) As String

        Dim x As Integer
        Dim TempSortHolder As String
        Dim FindImg As Integer
        Dim FindAsc As Integer
        Dim CurrentSearchMode As String = ""
        Dim NewSearchMode As String = ""
        Dim NewHeaderImg As String = ""
        Dim strSort As String = ""

        objDgrid.CurrentPageIndex = e.NewPageIndex
        objDgrid.DataBind()

        For x = 0 To objDgrid.Columns.Count - 1
            FindImg = InStr(objDgrid.Columns(x).HeaderText, "<img") 'find the column with the <img tag
            If FindImg <> 0 Then
                TempSortHolder = objDgrid.Columns(x).SortExpression
                FindAsc = InStr(TempSortHolder, "ASC")
                If FindAsc <> 0 Then 'sort desc
                    strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "ASC") - 1) & " ASC"
                Else 'sort asc
                    strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "DESC") - 1) & " DESC"
                End If
                Exit For
            End If
        Next
        If strSort = "" Then strSort = defaultSort

        'searches...
        'getProperties(strSort, ddlSearchBy.SelectedItem.Value, PublicFunctions.Convertdbnulls(txtSearch.Text))
        Return strSort

    End Function

End Class

