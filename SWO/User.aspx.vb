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

Partial Class User
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
    Dim ns As New SecurityValidate

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        If ns.UserLevelID <> "1" Then 'oCookie.Item("UserLevelID").ToString.Trim <> "1" Then
            Response.Redirect("Home.aspx")
        End If

        If Page.IsPostBack = False Then
            If Request("Action") = "Delete" Then
                DeleteUser()
            End If

            PopulateDataGrid()

            'Set message.
            globalMessage = Request("Message")
            Select Case globalMessage
                Case "1"
                    lblMessage.Text = "User Has Been Added."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblMessage.Visible = True
                Case "2"
                    lblMessage.Text = "User Has Been Deleted."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblMessage.Visible = True
                Case "3"
                    lblMessage.Text = "User Has Been Updated."
                    lblMessage.ForeColor = Drawing.Color.Green
                    lblMessage.Visible = True
                Case Else

            End Select
        End If
    End Sub

    Sub PopulateDataGrid()
        '--------------------------------------------------------------
        'OnClick event of the Go image used to populate the datagrid
        'and show the datagrid as well as the paging button.
        '--------------------------------------------------------------
        UserDataGrid.CurrentPageIndex = 0
        getUser("[User].[LastName] ASC", "", "")
        UserDataGrid.AllowSorting = True
    End Sub

    Sub getUser(ByVal sSortStr As String, ByVal sSearchBy As String, ByVal sSearchText As String)
        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        If cbxIncludeInactives.Checked = True Then
            objCmd = New SqlCommand("spFilterUser", objConn)
        Else
            objCmd = New SqlCommand("spFilterUserWithoutInactives", objConn)
        End If

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
        objDataGridFunctions.CalcDataGridCounts(objDS, UserDataGrid, "")

        'Associate the data grid with the data.
        UserDataGrid.DataSource = objDS.Tables(0).DefaultView
        UserDataGrid.DataBind()

        objDataGridFunctions.Highlightrows(UserDataGrid, "", "", "")
    End Sub

    Sub SortUser(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
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
        For x = 0 To UserDataGrid.Columns.Count - 1
            TempHeaderHolder = UserDataGrid.Columns(x).HeaderText
            FindImg = InStr(TempHeaderHolder, "<img")

            If FindImg <> 0 Then
                UserDataGrid.Columns(x).HeaderText = Left(TempHeaderHolder, FindImg - 1)
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
        Else
            'If no mode is specified, default is descending.
            NewSearchMode = "ASC"
            NewHeaderImg = "&nbsp;<img src='Images/blue_arrow_up2.jpg' align='absmiddle' border=0"
        End If

        'Derive the new sort expression.
        NewSortExpr = ColumnToSort & " " & NewSearchMode

        'Figure out the column index.
        Dim iIndex As Integer

        Select Case ColumnToSort.ToUpper()
            Case "EMAIL"
                iIndex = 3
                NewHeaderText = "Username/Email"
            Case "FIRSTNAME"
                iIndex = 4
                NewHeaderText = "First Name"
            Case "LASTNAME"
                iIndex = 5
                NewHeaderText = "Last Name"
            Case "LASTLOGIN"
                iIndex = 6
                NewHeaderText = "Last Login"
        End Select

        'Alter the column's sort expression.
        UserDataGrid.Columns(iIndex).SortExpression = NewSortExpr

        'Alter the column's header image.
        UserDataGrid.Columns(iIndex).HeaderText = NewHeaderText & NewHeaderImg

        'Sort the data in new order. Searches by provided.
        getUser(NewSortExpr, ddlSearchBy.SelectedItem.Value, HelpFunction.Convertdbnulls(txtSearch.Text))
    End Sub

    Sub UserDataGrid_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs)
        '-------------------------------------------------------------
        'This sub is called on the next and previous clicks for the
        'recordset. It cycles to the next 20 records.
        '-------------------------------------------------------------
        UserDataGrid.CurrentPageIndex = e.NewPageIndex
        UserDataGrid.DataBind()

        'Response.End()

        Dim x As Integer
        Dim TempSortHolder As String
        Dim FindImg As Integer
        Dim FindAsc As Integer
        Dim CurrentSearchMode As String = ""
        Dim NewSearchMode As String = ""
        Dim NewHeaderImg As String = ""
        Dim strSort As String = ""

        For x = 0 To UserDataGrid.Columns.Count - 1
            'Find the column with the <img> tag.
            FindImg = InStr(UserDataGrid.Columns(x).HeaderText, "<img")

            If FindImg <> 0 Then
                TempSortHolder = UserDataGrid.Columns(x).SortExpression
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

        If strSort = "" Then strSort = "User.[LastName] ASC"

        getUser("", ddlSearchBy.SelectedItem.Value, HelpFunction.Convertdbnulls(txtSearch.Text))
    End Sub

    Protected Sub btnSearch_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnSearch.Command
        'Searches by the dropdown value and search text.
        UserDataGrid.CurrentPageIndex = 0
        getUser("", ddlSearchBy.SelectedItem.Value, HelpFunction.Convertdbnulls(txtSearch.Text))
    End Sub

    Protected Sub UserDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles UserDataGrid.ItemDataBound
        'Loop through the items.
        'If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer And e.Item.ItemType <> ListItemType.Separator Then
        '  
        'End If
    End Sub

    Private Sub DeleteUser()
        Try
            'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
            'Add cookie.
            'Response.Cookies.Add(oCookie)
            ns = Session("Security_Tracker")

            Dim tempDeletedUserID As String
            tempDeletedUserID = ns.UserID.ToString.Trim

            Dim tempDeletedUserID2 As String
            tempDeletedUserID2 = Request("UserID").ToString.Trim

            If tempDeletedUserID = tempDeletedUserID2 Then
                lblMessage.Text = "You may not delete YOUR account."
                lblMessage.Visible = True
                lblMessage.ForeColor = Drawing.Color.Red

                Exit Sub
            End If

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spDeleteUserByUserID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@UserID", Request("UserID"))

            'Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            'Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()

            'Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            Response.Redirect("User.aspx?Message=2")
        Catch ex As Exception
            DBConStringHelper.FinalizeConnection(objConn)
            lblMessage.Text = "You may not delete this User due to the fact it is tied to related imported information. You must first delete all related imported information first, and then you may delete the User."
            lblMessage.Visible = True
            lblMessage.ForeColor = Drawing.Color.Red
        End Try
    End Sub

    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        PopulateDataGrid()
    End Sub
End Class