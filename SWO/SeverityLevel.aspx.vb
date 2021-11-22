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

Partial Class SeverityLevel
    Inherits System.Web.UI.Page

    'Help Functions from our App_Code
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp
    Public AuditHelper As New AuditHelp
    Public objDataGridFunctions As New DataGridFunctions

    'For Connecting to the database
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
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        If ns.UserLevelID <> "1" Then 'oCookie.Item("UserLevelID").ToString.Trim <> "1" Then
            Response.Redirect("Home.aspx")
        End If

        If Page.IsPostBack = False Then

            If Request("Action") = "Delete" Then
                DeleteSeverityLevel()
            End If

            PopulateDataGrid()
            'set message
            globalMessage = Request("Message")
            Select Case globalMessage
                Case "1"
                    lblMessage.Text = "Severity Level Has Been Added."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblMessage.Visible = True
                Case "2"
                    lblMessage.Text = "Severity Level Has Been Deleted."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblMessage.Visible = True
                Case "3"
                    lblMessage.Text = "Severity Level Has Been Updated."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblMessage.Visible = True
                Case Else

            End Select

        End If

    End Sub

    Sub PopulateDataGrid()
        '--------------------------------------------------------------
        '  Onclick event of the Go image used to populate the datagrid
        '  and show the datagrid and show the paging button
        '-------------------------------------------------------------
        SeverityLevelDataGrid.CurrentPageIndex = 0
        getSeverityLevel("[SeverityLevel].[SeverityLevelID] ASC", "", "")
        SeverityLevelDataGrid.AllowSorting = True
    End Sub

    Sub getSeverityLevel(ByVal sSortStr As String, ByVal sSearchBy As String, ByVal sSearchText As String)

        'connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        DBConStringHelper.PrepareConnection(objConn) 'open the connection

        'Response.Write(sSortStr.ToString)
        'Response.Write("<br>")
        'Response.Write("SearchBy: " & sSearchBy.ToString)
        'Response.Write("<br>")
        'Response.Write("Searchtext: " & sSearchText.ToString)
        'Response.Write("<br>")
        'Response.End()


        objCmd = New SqlCommand("spFilterSeverityLevel", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@OrderBy", sSortStr.ToString)
        objCmd.Parameters.AddWithValue("@SearchBy", sSearchBy.ToString)
        objCmd.Parameters.AddWithValue("@Searchtext", sSearchText.ToString)

        objDS.Tables.Clear()
        'bind our data
        objDA = New SqlDataAdapter(objCmd)
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing

        DBConStringHelper.FinalizeConnection(objConn) 'close the connection

        'call the calculate grid counts to show the number of records, the page you are on, etc.
        objDataGridFunctions.CalcDataGridCounts(objDS, SeverityLevelDataGrid, "")
        'Associate the data grid with the data
        SeverityLevelDataGrid.DataSource = objDS.Tables(0).DefaultView
        SeverityLevelDataGrid.DataBind()

        objDataGridFunctions.Highlightrows(SeverityLevelDataGrid, "", "", "")

    End Sub

    Sub SortSeverityLevel(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        '--------------------------------------------------------------------------------------------------------
        '  This sub figures out the column you selected and orders by that column...It also adds the image
        '  or takes away the image based on the column you are sorting
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

        '  Parse the sort expression - delimiter space to ignore the ASC
        SortExprs = Split(e.SortExpression, " ")
        ColumnToSort = SortExprs(0)

        '--------------------------------------------------------------------------------------------------------
        ' We run this to parse out the img portion of the header text if there is any....
        ' Loop through all the columns parsing out the <img tag if there is one and replacing it with nothing...
        For x = 0 To SeverityLevelDataGrid.Columns.Count - 1
            TempHeaderHolder = SeverityLevelDataGrid.Columns(x).HeaderText
            FindImg = InStr(TempHeaderHolder, "<img")
            If FindImg <> 0 Then
                SeverityLevelDataGrid.Columns(x).HeaderText = Left(TempHeaderHolder, FindImg - 1)
            End If
        Next
        '--------------------------------------------------------------------------------------------------------

        ' If a sort order is specified get it, else default is descending
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
        '--------------------------------------------------------------------------------------------------------
        '  Derive the new sort expression.
        NewSortExpr = ColumnToSort & " " & NewSearchMode
        '--------------------------------------------------------------------------------------------------------
        ' Figure out the column index
        Dim iIndex As Integer

        Select Case ColumnToSort.ToUpper()

            Case "SEVERITYLEVEL"
                iIndex = 3
                NewHeaderText = "Severity Level"

        End Select
        '--------------------------------------------------------------------------------------------------------
        ' alter the column's sort expression
        SeverityLevelDataGrid.Columns(iIndex).SortExpression = NewSortExpr
        'alter the column's header image
        SeverityLevelDataGrid.Columns(iIndex).HeaderText = NewHeaderText & NewHeaderImg

        '--------------------------------------------------------------------------------------------------------
        ' Sort the data in new order
        'searches by provided
        getSeverityLevel(NewSortExpr, ddlSearchBy.SelectedItem.Value, HelpFunction.Convertdbnulls(txtSearch.Text))

    End Sub

    Sub SeverityLevelDataGrid_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs)
        '---------------------------------------------------------------------------------------------------------
        '   This sub is called on the next and previous clicks for the recordset, it cycles to the next 20 records
        '---------------------------------------------------------------------------------------------------------
        SeverityLevelDataGrid.CurrentPageIndex = e.NewPageIndex
        SeverityLevelDataGrid.DataBind()


        'Response.End()

        Dim x As Integer
        Dim TempSortHolder As String
        Dim FindImg As Integer
        Dim FindAsc As Integer
        Dim CurrentSearchMode As String = ""
        Dim NewSearchMode As String = ""
        Dim NewHeaderImg As String = ""
        Dim strSort As String = ""

        For x = 0 To SeverityLevelDataGrid.Columns.Count - 1
            FindImg = InStr(SeverityLevelDataGrid.Columns(x).HeaderText, "<img") 'find the column with the <img tag
            If FindImg <> 0 Then
                TempSortHolder = SeverityLevelDataGrid.Columns(x).SortExpression
                FindAsc = InStr(TempSortHolder, "ASC")
                If FindAsc <> 0 Then 'sort desc
                    strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "ASC") - 1) & " ASC"
                Else 'sort asc
                    strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "DESC") - 1) & " DESC"
                End If
                Exit For
            End If
        Next

        If strSort = "" Then strSort = "SeverityLevel.[SeverityLevelID] ASC"


        getSeverityLevel("", ddlSearchBy.SelectedItem.Value, HelpFunction.Convertdbnulls(txtSearch.Text))


    End Sub

    Protected Sub btnSearch_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnSearch.Command

        'searches by the dropdown value and search text
        SeverityLevelDataGrid.CurrentPageIndex = 0
        getSeverityLevel("", ddlSearchBy.SelectedItem.Value, HelpFunction.Convertdbnulls(txtSearch.Text))

    End Sub

    Protected Sub SeverityLevelDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles SeverityLevelDataGrid.ItemDataBound
        'loop through the items...
        'If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer And e.Item.ItemType <> ListItemType.Separator Then
        '  
        'End If

    End Sub

    Private Sub DeleteSeverityLevel()

        Try
            Dim AuditInfo As String = ""
            Dim AuditAction As String = ""

            'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
            '// Add cookie
            'Response.Cookies.Add(oCookie)
            ns = Session("Security_Tracker")

            Dim localSeverityLevel As String = ""
            Dim localNotes As String = ""

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectSeverityLevelBySeverityLevelID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@SeverityLevelID", Request("SeverityLevelID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                AuditInfo = HelpFunction.Convertdbnulls(objDR("SeverityLevel"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            AuditAction = "Deleted " & AuditInfo & " from Severity Levels"

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spDeleteSeverityLevelBySeverityLevelID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@SeverityLevelID", Request("SeverityLevelID"))


            '// Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            '// Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()
            '// Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            'Now We Insert the Audit

            AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "3")


            Response.Redirect("SeverityLevel.aspx?Message=2")

        Catch ex As Exception

            DBConStringHelper.FinalizeConnection(objConn)
            lblMessage.Text = "You may not delete this Severity Level due to the fact it is tied to related imported information. You must first delete all related imported information first, and then you may delete the Severity Level."
            lblMessage.Visible = True
            lblMessage.ForeColor = Drawing.Color.Red

        End Try


    End Sub




End Class
