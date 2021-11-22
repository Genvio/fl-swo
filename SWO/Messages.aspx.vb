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

Partial Class Messages
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

        If Page.IsPostBack = False Then
            If Request("Action") = "Delete" Then
                DeleteMessage()
            End If

            PopulateDataGrid()
        End If
    End Sub

    Sub PopulateDataGrid()
        dgMessage.CurrentPageIndex = 0
        getMessage("[DateCreated] DESC")
        dgMessage.AllowSorting = True
    End Sub

    Sub getMessage(ByVal sSortStr As String)
        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("spSelectMessages", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@OrderBy", sSortStr.ToString)

        objDS.Tables.Clear()

        'Bind our data.
        objDA = New SqlDataAdapter(objCmd)
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        'Call the calculate grid counts to show the number of records, the page you are on, etc.
        objDataGridFunctions.CalcDataGridCounts(objDS, dgMessage, "")

        'Associate the data grid with the data.
        dgMessage.DataSource = objDS.Tables(0).DefaultView
        dgMessage.DataBind()

        objDataGridFunctions.Highlightrows(dgMessage, "", "", "")
    End Sub

    Sub SortMessage(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        '--------------------------------------------------------------------------------------------------------
        'This sub figures out the column you selected and orders by that column.. It also adds the image
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
        'We run this to parse out the img portion of the header text if there is any.. This loops through all
        'the columns parsing out the <img tag if there is one and replacing it with nothing.
        '--------------------------------------------------------------------------------------------------------
        For x = 0 To dgMessage.Columns.Count - 1
            TempHeaderHolder = dgMessage.Columns(x).HeaderText
            FindImg = InStr(TempHeaderHolder, "<img")
            If FindImg <> 0 Then
                dgMessage.Columns(x).HeaderText = Left(TempHeaderHolder, FindImg - 1)
            End If
        Next

        'If a sort order is specified, get it.
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
            NewSearchMode = "DESC"
            NewHeaderImg = "&nbsp;<img src='Images/blue_arrow_down2.jpg' align='absmiddle' border=0"
        End If

        'Derive the new sort expression.
        NewSortExpr = ColumnToSort & " " & NewSearchMode

        'Figure out the column index.
        Dim iIndex As Integer

        Select Case ColumnToSort.ToUpper()
            Case "DATECREATED"
                iIndex = 2
                NewHeaderText = "Date Created EST"
            Case "MESSAGE"
                iIndex = 3
                NewHeaderText = "Message"
            Case "CREATEDBY"
                iIndex = 4
                NewHeaderText = "Created By"
        End Select

        'Alter the column's sort expression.
        dgMessage.Columns(iIndex).SortExpression = NewSortExpr

        'Alter the column's header text/image.
        dgMessage.Columns(iIndex).HeaderText = NewHeaderText & NewHeaderImg

        'Sort the data in the new order.
        getMessage(NewSortExpr)
    End Sub

    Sub dgMessage_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs)
        dgMessage.CurrentPageIndex = e.NewPageIndex
        dgMessage.DataBind()

        Dim x As Integer
        Dim TempSortHolder As String
        Dim FindImg As Integer
        Dim FindAsc As Integer
        Dim CurrentSearchMode As String = ""
        Dim NewSearchMode As String = ""
        Dim NewHeaderImg As String = ""
        Dim strSort As String = ""

        For x = 0 To dgMessage.Columns.Count - 1
            'Find the column with the <img tag.
            FindImg = InStr(dgMessage.Columns(x).HeaderText, "<img")

            If FindImg <> 0 Then
                TempSortHolder = dgMessage.Columns(x).SortExpression
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

        If strSort = "" Then strSort = "[Message].[DateCreated] DESC"

        getMessage("")
    End Sub

    Sub DeleteMessage()
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        objCmd = New SqlCommand("spDeleteMessageByMessageID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@MessageID", Request("MessageID"))

        'Open the connection using the connection string.
        DBConStringHelper.PrepareConnection(objConn)

        'Execute the command to the DataReader.
        objCmd.ExecuteNonQuery()

        'Clean up our command objects and close the connection.
        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)

        Response.Redirect("Messages.aspx")
    End Sub

    Protected Sub btnAddMessage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddMessage.Click
        Response.Redirect("EditMessage.aspx?MessageID=0")
    End Sub
End Class
