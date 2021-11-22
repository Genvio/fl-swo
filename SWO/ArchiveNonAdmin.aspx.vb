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

Partial Class ArchiveNonAdmin
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
    Dim oCookie As System.Web.HttpCookie
    Dim ns As New SecurityValidate

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        'Select Case oCookie.Item("UserLevelID")
        '    Case "1" 'Admin.

        '    Case "2" 'Full User.

        '    Case "3" 'Update User.
        '        btnAddIncident.Enabled = False
        '    Case "4" 'Read Only.
        '        btnAddIncident.Enabled = False
        '    Case Else

        'End Select

        'If oCookie.Item("IncidentLevelID").ToString.Trim <> "1" Then
        '    Response.Redirect("Home.aspx")
        'End If

        Dim oDataDeleter As New DataDeleter()
        oDataDeleter.DeleteOldNonSavedReports()

        If Page.IsPostBack = False Then
            If Request("Action") = "Delete" Then
                DeleteIncident()
            End If

            PopulateDDLs()
            LoadSavedFormFields()
            PopulateDataGrid()

            'Set message.
            globalMessage = Request("Message")

            Select Case globalMessage
                Case "1"
                    'lblMessage.Text = "Incident Has Been Added."
                    'lblMessage.ForeColor = Drawing.Color.Green
                    'lblMessage.Visible = True
                Case "2"
                    'lblMessage.Text = "Incident Has Been Deleted."
                    'lblMessage.ForeColor = Drawing.Color.Green
                    'lblMessage.Visible = True
                Case "3"
                    'lblMessage.Text = "Incident Has Been Updated."
                    'lblMessage.ForeColor = Drawing.Color.Green
                    'lblMessage.Visible = True
                Case Else

            End Select
        End If

        Session("IncidentSourceGrid") = "Archive" 'So the Cancel button in EditIncident can return the user to Archived Incidents instead of Current Incidents
    End Sub

    Sub PopulateDataGrid()
        '--------------------------------------------------------------
        'Onclick event of the Go image used to populate the datagrid
        'and show the datagrid and show the paging button.
        '--------------------------------------------------------------
        IncidentDataGrid.CurrentPageIndex = 0
        getIncident("[IncidentID] DESC", ddlSearchBy.SelectedValue, txtSearch.Text)
        IncidentDataGrid.AllowSorting = True
    End Sub

    Sub getIncident(ByVal sSortStr As String, ByVal sSearchBy As String, ByVal sSearchText As String)
        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        DBConStringHelper.PrepareConnection(objConn) 'open the connection

        'Response.Write(sSortStr.ToString)
        'Response.Write("<br>")
        'Response.Write("SearchBy: " & sSearchBy.ToString)
        'Response.Write("<br>")
        'Response.Write("Searchtext: " & sSearchText.ToString)
        'Response.Write("<br>")
        'Response.Write("ddlIncidentType.SelectedItem.ToString: " & ddlIncidentType.SelectedItem.ToString)
        'Response.Write("<br>")
        'Response.Write("Worksheet: " & ddlIncidentType.SelectedItem.ToString)
        'Response.Write("<br>")
        'Response.End()

        objCmd = New SqlCommand("spFilterIncidentNonAdmin", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@OrderBy", sSortStr.ToString)
        objCmd.Parameters.AddWithValue("@SearchBy", sSearchBy.ToString)
        objCmd.Parameters.AddWithValue("@Searchtext", sSearchText.ToString)

        If ddlIncidentType.SelectedItem.ToString = "All Worksheets" Then
            objCmd.Parameters.AddWithValue("@FilterByWorksheet", "False")
        Else
            objCmd.Parameters.AddWithValue("@FilterByWorksheet", "True")
        End If

        objCmd.Parameters.AddWithValue("@Worksheet", ddlIncidentType.SelectedItem.ToString)

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

        Session("IncidentArchiveGridSort") = NewSortExpr
        'Sort the data in new order. Searches by provided.
        getIncident(NewSortExpr, ddlSearchBy.SelectedItem.Value, strSearch)
    End Sub

    Sub IncidentDataGrid_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs)
        '---------------------------------------------------------------------------------------------------------
        'This sub is called on the next and previous clicks for the recordset, it cycles to the next 20 records.
        '---------------------------------------------------------------------------------------------------------
        IncidentDataGrid.CurrentPageIndex = e.NewPageIndex
        'IncidentDataGrid.DataBind()

        Dim CurrentSearchMode As String = ""
        Dim NewSearchMode As String = ""
        Dim NewHeaderImg As String = ""
        Dim strSort As String = Session("IncidentArchiveGridSort")

        'For x = 0 To IncidentDataGrid.Columns.Count - 1
        '    'Find the column with the <img> tag.
        '    FindImg = InStr(IncidentDataGrid.Columns(x).HeaderText, "<img")

        '    If FindImg <> 0 Then
        '        TempSortHolder = IncidentDataGrid.Columns(x).SortExpression
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

        If strSort = "" Then strSort = "Incident.[IncidentID] DESC"

        getIncident(strSort, ddlSearchBy.SelectedItem.Value, HelpFunction.Convertdbnulls(txtSearch.Text))
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
                getIncident("Incident.[IncidentID] DESC", ddlSearchBy.SelectedItem.Value, strSearch)
            End If
        Else
            Dim strSearch As String = ""

            If ddlSearchBy.SelectedItem.ToString = "By Incident Number" Then
                strSearch = txtSearch.Text

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
            getIncident("Incident.[IncidentID] DESC", ddlSearchBy.SelectedItem.Value, strSearch)
        End If

        If oCookie Is Nothing Then oCookie = Response.Cookies(Application("ApplicationEnvironment").ToString)
        oCookie.Item("ArchivedIncidentSearchText") = txtSearch.Text
        oCookie.Item("ArchivedIncidentSearchListValue") = ddlSearchBy.SelectedValue
        oCookie.Item("ArchivedIncidentFilterListValue") = ddlIncidentType.SelectedValue
        oCookie.Expires = DateTime.Now.AddDays(90)
        Response.Cookies.Add(oCookie)
    End Sub

    Protected Sub IncidentDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles IncidentDataGrid.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim strCounties As String = e.Item.Cells(6).Text

            If Not String.IsNullOrEmpty(strCounties) Then
                If Len(strCounties) - Len(strCounties.Replace(",", "")) = CInt(System.Configuration.ConfigurationManager.AppSettings("NumberOfFloridaCounties").ToString) - 1 Then
                    e.Item.Cells(6).Text = "<b>Statewide</b>"
                Else
                    Dim oCountyRegion As New CountyRegion(CType(e.Item.DataItem, DataRowView).Row.ItemArray(9).ToString())
                    Dim strCountyRegion As String = ""
                    strCountyRegion = oCountyRegion.GetRegionAndCountyList(True)
                    e.Item.Cells(6).Text = strCountyRegion
                End If
            End If
        End If
    End Sub

    Private Sub DeleteIncident()
        Try
            'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
            'Add cookie.
            'Response.Cookies.Add(oCookie)
            ns = Session("Security_Tracker")

            Dim tempDeletedIncidentID As String
            tempDeletedIncidentID = Session("IncidentID").ToString.Trim

            Dim tempDeletedIncidentID2 As String
            tempDeletedIncidentID2 = Request("IncidentID").ToString.Trim

            If tempDeletedIncidentID = tempDeletedIncidentID2 Then
                lblMessage.Text = "You may not delete YOUR Incident."
                lblMessage.Visible = True
                lblMessage.ForeColor = Drawing.Color.Red

                Exit Sub
            End If

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spDeleteIncidentByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

            'Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            'Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()

            'Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            Response.Redirect("Incident.aspx?Message=2")
        Catch ex As Exception
            DBConStringHelper.FinalizeConnection(objConn)
            lblMessage.Text = "You may not delete this Incident due to the fact it is tied to related imported information. You must first delete all related imported information first, and then you may delete the Incident."
            lblMessage.Visible = True
            lblMessage.ForeColor = Drawing.Color.Red
        End Try
    End Sub

    'Protected Sub btnAddIncident_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddIncident.Click
    '    Response.Redirect("EditIncident.aspx?IncidentID=0")
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

    Sub PopulateDDLs()
        ddlIncidentType.Items.Clear()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectIncidentTypeOrderByIncidentType", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        'objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

        'objCmd.Parameters.AddWithValue("@OrderBy", "") Optional Parameter

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        ddlIncidentType.DataSource = objCmd.ExecuteReader()
        ddlIncidentType.DataBind()

        'CLose the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        objCmd = Nothing

        'Add an "Select an Option" item to the list.
        'ddlIncidentType.Items.Insert(0, New ListItem("Select An Incident Worksheet", "0"))
        'ddlIncidentType.Items.Insert(100, New ListItem("", "0"))
        'ddlIncidentType.Items(0).Selected = True
        ddlIncidentType.Items.FindByText("All Worksheets").Selected = True
    End Sub

    Protected Sub ddlIncidentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIncidentType.SelectedIndexChanged
        If oCookie Is Nothing Then oCookie = Response.Cookies(Application("ApplicationEnvironment").ToString)
        oCookie.Item("ArchivedIncidentSearchText") = txtSearch.Text
        oCookie.Item("ArchivedIncidentSearchListValue") = ddlSearchBy.SelectedValue
        oCookie.Item("ArchivedIncidentFilterListValue") = ddlIncidentType.SelectedValue
        oCookie.Expires = DateTime.Now.AddDays(90)
        Response.Cookies.Add(oCookie)
        LoadSavedFormFields()
        PopulateDataGrid()
    End Sub

    Private Sub LoadSavedFormFields()
        oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        If Not oCookie Is Nothing Then
            If Not String.IsNullOrEmpty(oCookie.Item("ArchivedIncidentSearchText")) Then txtSearch.Text = oCookie.Item("ArchivedIncidentSearchText")
            If Not String.IsNullOrEmpty(oCookie.Item("ArchivedIncidentSearchListValue")) Then ddlSearchBy.SelectedValue = oCookie.Item("ArchivedIncidentSearchListValue")
            If Not String.IsNullOrEmpty(oCookie.Item("ArchivedIncidentFilterListValue")) Then ddlIncidentType.SelectedValue = oCookie.Item("ArchivedIncidentFilterListValue")
        End If
    End Sub

    Protected Sub btnReset_Click(sender As Object, e As System.EventArgs) Handles btnReset.Click
        If oCookie Is Nothing Then oCookie = Response.Cookies(Application("ApplicationEnvironment").ToString)
        txtSearch.Text = ""
        ddlSearchBy.SelectedValue = "[IncidentNumber].Number"
        ddlIncidentType.ClearSelection()
        ddlIncidentType.Items.FindByText("All Worksheets").Selected = True
        oCookie.Item("ArchivedIncidentSearchText") = "" 'txtSearch.Text
        oCookie.Item("ArchivedIncidentSearchListValue") = "" 'ddlSearchBy.SelectedValue
        oCookie.Item("ArchivedIncidentFilterListValue") = "" 'ddlIncidentType.SelectedValue
        oCookie.Expires = DateTime.Now.AddDays(90)
        Response.Cookies.Add(oCookie)
        IncidentDataGrid.CurrentPageIndex = 0
        Session("IncidentArchiveGridSort") = ""
        getIncident("[IncidentID] DESC", ddlSearchBy.SelectedValue, "")
        LoadSavedFormFields()
    End Sub

End Class