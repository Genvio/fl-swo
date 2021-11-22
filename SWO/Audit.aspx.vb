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

Partial Class Audit
    Inherits System.Web.UI.Page

    'Help functions from our App_Code.
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    Public objDataGridFunctions As New DataGridFunctions

    'For connecting to the database.
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet

    Dim globalHasErrors As Boolean = False
    Dim globalMessage As String
    'Dim oCookie As System.Web.HttpCookie

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            PopulateDataGrid()

            'Set message.
            globalMessage = Request("Message")
            Select Case globalMessage
                Case "1"
                    lblMessage.Text = " "
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblMessage.Visible = True
                Case "2"
                    lblMessage.Text = " "
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblMessage.Visible = True
                Case "3"
                    lblMessage.Text = "  "
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblMessage.Visible = True
                Case Else

            End Select
        End If
    End Sub

    Sub PopulateDataGrid()
        '--------------------------------------------------------------
        'Onclick event of the Go image used to populate the datagrid
        'and show the datagrid and show the paging button.
        '-------------------------------------------------------------
        AuditDataGrid.CurrentPageIndex = 0
        getAudit("[Audit].[AuditID] ASC", "", "")
        AuditDataGrid.AllowSorting = True
    End Sub

    Sub getAudit(ByVal sSortStr As String, ByVal sSearchBy As String, ByVal sSearchText As String)
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

        objCmd = New SqlCommand("spFilterAudit", objConn)
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
        objDataGridFunctions.CalcDataGridCounts(objDS, AuditDataGrid, "")

        'Associate the data grid with the data.
        AuditDataGrid.DataSource = objDS.Tables(0).DefaultView
        AuditDataGrid.DataBind()

        objDataGridFunctions.Highlightrows(AuditDataGrid, "", "", "")
    End Sub

    Sub SortAudit(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        '-------------------------------------------------------------
        'This sub figures out the column you selected and orders by
        'that column. It also adds the image or takes it away based
        'on the column you are sorting.
        '-------------------------------------------------------------
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

        '-------------------------------------------------------------
        'We run this to parse out the img portion of the header text
        'if there is any. Loop through all the columns parsing out
        'the <img> tag if there is one and replacing it with nothing.
        '-------------------------------------------------------------
        For x = 0 To AuditDataGrid.Columns.Count - 1
            TempHeaderHolder = AuditDataGrid.Columns(x).HeaderText
            FindImg = InStr(TempHeaderHolder, "<img")

            If FindImg <> 0 Then
                AuditDataGrid.Columns(x).HeaderText = Left(TempHeaderHolder, FindImg - 1)
            End If
        Next
        
        'If a sort order is specified, get it, else default is descending.
        If SortExprs.Length() > 1 Then
            CurrentSearchMode = SortExprs(1).ToUpper()
            If CurrentSearchMode = "ASC" Then
                NewSearchMode = "DESC"
                NewHeaderImg = "&nbsp;<img src='Images/blue_arrow_down2.jpg' align='absmiddle' border=0"
            Else
                NewSearchMode = "ASC"
                NewHeaderImg = "&nbsp;<img src='Images/blue_arrow_up2.jpg' align='absmiddle' border=0"
            End If
        Else   ' If no mode specified, Default is descending
            NewSearchMode = "ASC"
            NewHeaderImg = "&nbsp;<img src='Images/blue_arrow_up2.jpg' align='absmiddle' border=0"
        End If

        'Derive the new sort expression.
        NewSortExpr = ColumnToSort & " " & NewSearchMode

        'Figure out the column index.
        Dim iIndex As Integer

        Select Case ColumnToSort.ToUpper()
            Case "EMAIL"
                iIndex = 1
                NewHeaderText = "User"
            Case "AUDITTYPE"
                iIndex = 2
                NewHeaderText = "Audit Type"
            Case "ACTION"
                iIndex = 3
                NewHeaderText = "Action"
            Case "AUDITDATE"
                iIndex = 4
                NewHeaderText = "Date"
        End Select

        'Alter the column's sort expression.
        AuditDataGrid.Columns(iIndex).SortExpression = NewSortExpr

        'Alter the column's header image.
        AuditDataGrid.Columns(iIndex).HeaderText = NewHeaderText & NewHeaderImg

        'Sort the data in new order. Searches by provided.
        getAudit(NewSortExpr, ddlSearchBy.SelectedItem.Value, HelpFunction.Convertdbnulls(txtSearch.Text))
    End Sub

    Sub AuditDataGrid_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs)
        '-------------------------------------------------------------
        'This sub is called on the next and previous clicks for the
        'recordset. It cycles to the next 20 records.
        '-------------------------------------------------------------
        AuditDataGrid.CurrentPageIndex = e.NewPageIndex
        AuditDataGrid.DataBind()

        'Response.End()

        Dim x As Integer
        Dim TempSortHolder As String
        Dim FindImg As Integer
        Dim FindAsc As Integer
        Dim CurrentSearchMode As String = ""
        Dim NewSearchMode As String = ""
        Dim NewHeaderImg As String = ""
        Dim strSort As String = ""

        For x = 0 To AuditDataGrid.Columns.Count - 1
            'Find the column witht the <img> tag.
            FindImg = InStr(AuditDataGrid.Columns(x).HeaderText, "<img")

            If FindImg <> 0 Then
                TempSortHolder = AuditDataGrid.Columns(x).SortExpression
                FindAsc = InStr(TempSortHolder, "ASC")

                If FindAsc <> 0 Then
                    'Sort descending.
                    strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "ASC") - 1) & " ASC"
                Else
                    'Sort ascending.
                    strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "DESC") - 1) & " DESC"
                End If

                Exit For
            End If
        Next

        If strSort = "" Then strSort = "Audit.[AuditID] ASC"

        getAudit("", ddlSearchBy.SelectedItem.Value, HelpFunction.Convertdbnulls(txtSearch.Text))
    End Sub

    Protected Sub btnSearch_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnSearch.Command
        'Searches by the dropdown value and search text.
        AuditDataGrid.CurrentPageIndex = 0
        getAudit("", ddlSearchBy.SelectedItem.Value, HelpFunction.Convertdbnulls(txtSearch.Text))
    End Sub

    Protected Sub AuditDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles AuditDataGrid.ItemDataBound
        'Loop through the items.
        'If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer And e.Item.ItemType <> ListItemType.Separator Then
        '  
        'End If
    End Sub
End Class