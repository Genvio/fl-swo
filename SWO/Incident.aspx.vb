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
Imports UserValidation

Partial Class Incident
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
    Dim oCookie As System.Web.HttpCookie
    Dim ns As New SecurityValidate

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        ns = Session("Security_Tracker")

        Select Case ns.UserLevelID.ToString() 'oCookie.Item("UserLevelID")
            Case "1" 'Admin.

            Case "2" 'Full User.

            Case "3" 'Update User.
                btnAddIncident.Enabled = False
            Case "4", "5" 'Read Only and Read Only + Hazmat.
                btnAddIncident.Enabled = False
            Case Else

        End Select

        'If oCookie.Item("IncidentLevelID").ToString.Trim <> "1" Then
        '    Response.Redirect("Home.aspx")
        'End If

        Dim oDataDeleter As New DataDeleter()
        oDataDeleter.DeleteOldNonSavedReports()

        If Page.IsPostBack = False Then
            If Request("Action") = "Delete" Then
                DeleteIncident()
            End If

            'PullUpdates()
            PopulateDDLs()
            LoadSavedFormFields()
            PopulateDataGrid()

            'Add cookie.
            'Response.Cookies.Add(oCookie)

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

        Session("IncidentSourceGrid") = "Current"
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

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("spFilterIncident2", objConn)
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

        If ddlAgency.SelectedItem.ToString = "Select An Agency" Then
            objCmd.Parameters.AddWithValue("@AgencyAbbr", "")
        Else
            objCmd.Parameters.AddWithValue("@AgencyAbbr", ddlAgency.SelectedItem.ToString)
        End If

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

        'Sort the data in new order. Searches by provided.
        getIncident(NewSortExpr, ddlSearchBy.SelectedItem.Value, strSearch)
    End Sub

    Sub IncidentDataGrid_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs)
        '---------------------------------------------------------------------------------------------------------
        'This sub is called on the next and previous clicks for the recordset, it cycles to the next 20 records.
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

        getIncident("", ddlSearchBy.SelectedItem.Value, HelpFunction.Convertdbnulls(txtSearch.Text))
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
                getIncident("[IncidentID] DESC", ddlSearchBy.SelectedItem.Value, strSearch)
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
            getIncident("[IncidentID] DESC", ddlSearchBy.SelectedItem.Value, strSearch)
        End If

        If oCookie Is Nothing Then oCookie = Response.Cookies(Application("ApplicationEnvironment").ToString)
        oCookie.Item("IncidentSearchText") = txtSearch.Text
        oCookie.Item("IncidentSearchListValue") = ddlSearchBy.SelectedValue
        oCookie.Item("IncidentFilterListValue") = ddlIncidentType.SelectedValue
        oCookie.Item("AgencyFilterListValue") = ddlAgency.SelectedValue
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

    Protected Sub btnAddIncident_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddIncident.Click
        Response.Redirect("EditIncident.aspx?IncidentID=0")
    End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        If IsDate(txtSearch.Text) = False Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Vaild Date. <br />")
            globalHasErrors = True
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label.
        lblMessage.Text = strError.ToString
    End Sub

    Sub PopulateDDLs()
        ddlIncidentType.Items.Clear()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectIncidentTypeOrderByIncidentType", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        'objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
		'objCmd.Parameters.AddWithValue("@OrderBy", "") 'Optional Parameter.

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        ddlIncidentType.DataSource = objCmd.ExecuteReader()
        ddlIncidentType.DataBind()
        DBConStringHelper.FinalizeConnection(objConn)

        ddlAgency.Items.Clear()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd.CommandText = "spSelectAgencyAbbreviations"
        DBConStringHelper.PrepareConnection(objConn)
        ddlAgency.DataSource = objCmd.ExecuteReader()
        ddlAgency.DataBind()
        DBConStringHelper.FinalizeConnection(objConn)

        objCmd = Nothing

        'Add an "Select an Option" item to the list.
        ddlAgency.Items.Insert(0, New ListItem("Select An Agency", "0"))
        ddlAgency.Items(0).Selected = True
        ddlIncidentType.Items.FindByText("All Worksheets").Selected = True
    End Sub

    Protected Sub ddlIncidentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIncidentType.SelectedIndexChanged
        If oCookie Is Nothing Then oCookie = Response.Cookies(Application("ApplicationEnvironment").ToString)
        oCookie.Item("IncidentSearchText") = txtSearch.Text
        oCookie.Item("IncidentSearchListValue") = ddlSearchBy.SelectedValue
        oCookie.Item("IncidentFilterListValue") = ddlIncidentType.SelectedValue
        oCookie.Item("AgencyFilterListValue") = ddlAgency.SelectedValue
        oCookie.Expires = DateTime.Now.AddDays(90)
        Response.Cookies.Add(oCookie)
        LoadSavedFormFields()
        PopulateDataGrid()
    End Sub
	
	Sub PullUpdates()
        'Dim reader As XmlTextReader = New XmlTextReader("http://alerts.weather.gov/cap/fl.atom")
        Dim reader As XmlTextReader = New XmlTextReader("http://alerts.weather.gov/cap/fl.php?x=1")

        'Incident variables.
        '---------------------------------------------------
        Dim AutoUpdateID As String = ""                     'Taken from a substring of <id> within <entry>. Substring begins after .php?x=.. then next 14 characters.
        Dim Saved As Boolean = True
        Dim CreatedBy As Integer = 47
        Dim DateCreated As Date = Now
        Dim IncidentStatus As Integer = 3
        Dim IncidentName As String = ""                     'Taken from <cap:event>.
        Dim IsThisADrill As String = "No"
        Dim StateAssistance As String = "No"
        Dim SeverityID As Integer = 1
        Dim ReportingPartyTypeID As Integer = 3
        Dim ReportingPartyFirstName As String = "NWS"
        Dim OnSceneContactTypeID As Integer = 3
        Dim ResponsiblePartyTypeID As Integer = 3
        Dim IncidentOccurredDate As Date                    'Taken from <published>.
        Dim IncidentOccurredTime As String = ""             'Taken from <published>.
        Dim ReportedToSWODate As Date                       'Taken from <published>.
        Dim ReportedToSWOTime As String = ""                'Taken from <published>.
        Dim ObtainCoordinate As String = "AffectedCounties"
        Dim AddedCounty As String = ""                      'Taken from <cap:areaDesc>

        'County variables.
        '---------------------------------------------------
        Dim Region1 As Boolean = False
        Dim Region2 As Boolean = False
        Dim Region3 As Boolean = False
        Dim Region4 As Boolean = False
        Dim Region5 As Boolean = False
        Dim Region6 As Boolean = False
        Dim Region7 As Boolean = False

        Dim Statewide As Boolean = False

        Dim Region1Affected As Boolean = False
        Dim Region2Affected As Boolean = False
        Dim Region3Affected As Boolean = False
        Dim Region4Affected As Boolean = False
        Dim Region5Affected As Boolean = False
        Dim Region6Affected As Boolean = False
        Dim Region7Affected As Boolean = False

        Dim Bay As Boolean = False
        Dim Calhoun As Boolean = False
        Dim Escambia As Boolean = False
        Dim Gulf As Boolean = False
        Dim Holmes As Boolean = False
        Dim Jackson As Boolean = False
        Dim Okaloosa As Boolean = False
        Dim SantaRosa As Boolean = False
        Dim Walton As Boolean = False
        Dim Washington As Boolean = False

        Dim Columbia As Boolean = False
        Dim Dixie As Boolean = False
        Dim Franklin As Boolean = False
        Dim Gadsden As Boolean = False
        Dim Hamilton As Boolean = False
        Dim Jefferson As Boolean = False
        Dim Lafayette As Boolean = False
        Dim Leon As Boolean = False
        Dim Liberty As Boolean = False
        Dim Madison As Boolean = False
        Dim Suwanee As Boolean = False
        Dim Taylor As Boolean = False
        Dim Wakulla As Boolean = False

        Dim Alachua As Boolean = False
        Dim Baker As Boolean = False
        Dim Bradford As Boolean = False
        Dim Clay As Boolean = False
        Dim Duval As Boolean = False
        Dim Flagler As Boolean = False
        Dim Gilchrist As Boolean = False
        Dim Levy As Boolean = False
        Dim Marion As Boolean = False
        Dim Nassau As Boolean = False
        Dim Putnam As Boolean = False
        Dim StJohns As Boolean = False
        Dim Union As Boolean = False

        Dim Citrus As Boolean = False
        Dim Hardee As Boolean = False
        Dim Hernando As Boolean = False
        Dim Hillsborough As Boolean = False
        Dim Pasco As Boolean = False
        Dim Pinellas As Boolean = False
        Dim Polk As Boolean = False
        Dim Sumter As Boolean = False

        Dim Brevard As Boolean = False
        Dim IndianRiver As Boolean = False
        Dim Lake As Boolean = False
        Dim Martin As Boolean = False
        Dim Orange As Boolean = False
        Dim Osceola As Boolean = False
        Dim Seminole As Boolean = False
        Dim StLucie As Boolean = False
        Dim Volusia As Boolean = False

        Dim Charlotte As Boolean = False
        Dim Collier As Boolean = False
        Dim DeSoto As Boolean = False
        Dim Glades As Boolean = False
        Dim Hendry As Boolean = False
        Dim Highlands As Boolean = False
        Dim Lee As Boolean = False
        Dim Manatee As Boolean = False
        Dim Okeechobee As Boolean = False
        Dim Sarasota As Boolean = False

        Dim Broward As Boolean = False
        Dim MiamiDade As Boolean = False
        Dim Monroe As Boolean = False
        Dim PalmBeach As Boolean = False

        'Worksheet variables.
        '---------------------------------------------------
        Dim wsSubType As String = ""                        'Taken from <cap:event>.
        Dim wsSituation As String = "Active"
        Dim wsName As String = ""                           'Taken from <title> within <entry>.
        Dim wsIncidentTypeLevelID As Integer = 228
        Dim wsDateIssued As Date                            'Taken from <published>.
        Dim wsTimeIssued As String = ""                     'Taken from <published>.
        Dim wsEffectiveDate As Date                         'Taken from <cap:effective>.
        Dim wsEffectiveTime As String = ""                  'Taken from <cap:effective>.
        Dim wsExpireDate As Date                            'Taken from <cap:expires>.
        Dim wsExpireTime As String = ""                     'Taken from <cap:expires>.
        Dim wsIssuingOffice As String = "NOAA"
        Dim wsAdvisoryType As String = ""                   'Taken from <cap:event>.
        Dim wsAdvisoryText As String = ""                   'Taken from <summary>.

        'Node booleans.
        '---------------------------------------------------
        Dim inEntry As Boolean
        Dim gotEntryId As Boolean
        Dim gotEntryTitle As Boolean
        Dim isPublished As Boolean
        Dim isSummary As Boolean
        Dim isCapEvent As Boolean
        Dim isCapEffective As Boolean
        Dim isCapExpires As Boolean
        Dim isCapGeocode As Boolean
        Dim firstGeocodeValue As Boolean
        Dim entryComplete As Boolean
        Dim isDuplicate As Boolean
        Dim voidEntry As Boolean

        Do While (reader.Read())
            'Checks to see if an entry has been completed.
            '---------------------------------------------------
            If entryComplete = True And voidEntry = False Then
                'Get the AutoUpdateID and compare it to the current entry.
                '-----------------------------------------------------------
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Open the connection.
                objConn.Open()
                objCmd = New SqlCommand("spSelectIncidentWithAutoUpdateID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure

                'Execute the query.
                objDR = objCmd.ExecuteReader

                While objDR.Read()
                    If AutoUpdateID = objDR("AutoUpdateID") Then isDuplicate = True
                End While

                'Clear command objects and close the connection.
                objDR.Close()
                objCmd.Dispose()
                objCmd = Nothing
                objConn.Close()
                '-----------------------------------------------------------

                If isDuplicate = False Then
                    'Insert the data.
                    '-----------------------------------------------------------
                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Establish the query and enter it's parameters.
                    objCmd = New SqlCommand("spInsertAutoWeatherFeedIncident", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@Saved", Saved)
                    objCmd.Parameters.AddWithValue("@CreatedByID", CreatedBy)
                    objCmd.Parameters.AddWithValue("@DateCreated", DateCreated)
                    objCmd.Parameters.AddWithValue("@IncidentStatusID", IncidentStatus)
                    objCmd.Parameters.AddWithValue("@IncidentName", IncidentName)
                    objCmd.Parameters.AddWithValue("@IsThisADrill", IsThisADrill)
                    objCmd.Parameters.AddWithValue("@StateAssistance", StateAssistance)
                    objCmd.Parameters.AddWithValue("@SeverityID", SeverityID)
                    objCmd.Parameters.AddWithValue("@ReportingPartyTypeID", ReportingPartyTypeID)
                    objCmd.Parameters.AddWithValue("@OnSceneContactTypeID", OnSceneContactTypeID)
                    objCmd.Parameters.AddWithValue("@ResponsiblePartyTypeID", ResponsiblePartyTypeID)
                    objCmd.Parameters.AddWithValue("@IncidentOccurredDate", IncidentOccurredDate)
                    objCmd.Parameters.AddWithValue("@IncidentOccurredTime", IncidentOccurredTime)
                    objCmd.Parameters.AddWithValue("@ReportedToSWODate", ReportedToSWODate)
                    objCmd.Parameters.AddWithValue("@ReportedToSWOTime", ReportedToSWOTime)
                    objCmd.Parameters.AddWithValue("@ObtainCoordinate", ObtainCoordinate)
                    objCmd.Parameters.AddWithValue("@AddedCounty", AddedCounty)
                    objCmd.Parameters.AddWithValue("@AutoUpdateID", AutoUpdateID)

                    'Open the connection.
                    DBConStringHelper.PrepareConnection(objConn)

                    'Execute the query.
                    objCmd.ExecuteNonQuery()

                    'Clear command objects and close the connection.
                    objCmd.Dispose()
                    objCmd = Nothing
                    DBConStringHelper.FinalizeConnection(objConn)

                    'Clear counties.
                    'AddedCounty = ""

                    'Pull the incident we just added and get it's ID.
                    '-----------------------------------------------------------
                    Dim newlyAddedIncidentID As Integer

                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Open the connection.
                    objConn.Open()
                    objCmd = New SqlCommand("spSelectIncidentIDByAutoUpdateID", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@AutoUpdateID", AutoUpdateID)

                    'Execute the query.
                    objDR = objCmd.ExecuteReader

                    If objDR.Read Then
                        newlyAddedIncidentID = objDR("IncidentID")
                    End If

                    'Clear command objects and close the connection.
                    objDR.Close()
                    objCmd.Dispose()
                    objCmd = Nothing
                    objConn.Close()

                    'Create the incident number then add it.
                    '-----------------------------------------------------------
                    Dim localMaxIncidentNumber As Integer
                    Dim currentTime As System.DateTime = System.DateTime.Now
                    Dim localDate As String = CStr(currentTime.Year)

                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Open the connection.
                    objConn.Open()
                    objCmd = New SqlCommand("spSelectMaxIncidentNumber", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@Year", localDate)

                    'Execute the query.
                    objDR = objCmd.ExecuteReader

                    If objDR.Read() Then
                        localMaxIncidentNumber = HelpFunction.ConvertdbnullsInt(objDR("Count"))
                    End If

                    localMaxIncidentNumber = localMaxIncidentNumber + 1

                    'Clear command objects and close the connection.
                    objDR.Close()
                    objCmd.Dispose()
                    objCmd = Nothing
                    objConn.Close()

                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Establish the query and enter it's parameters.
                    objCmd = New SqlCommand("spInsertIncidentNumber", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@IncidentID", newlyAddedIncidentID)
                    objCmd.Parameters.AddWithValue("@Year", localDate)
                    objCmd.Parameters.AddWithValue("@Number", localMaxIncidentNumber)

                    'Open the connection.
                    DBConStringHelper.PrepareConnection(objConn)

                    'Execute the query.
                    objCmd.ExecuteNonQuery()

                    'Clear command objects and close the connection.
                    objCmd.Dispose()
                    objCmd = Nothing
                    DBConStringHelper.FinalizeConnection(objConn)

                    'Insert the initial report.
                    '-----------------------------------------------------------
                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Establish the query and enter it's parameters.
                    objCmd = New SqlCommand("spInsertInitialReport", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@IncidentID", newlyAddedIncidentID)
                    objCmd.Parameters.AddWithValue("@InitialReport", "Auto update NOAA weather feed.")
                    objCmd.Parameters.AddWithValue("@UpdateDate", Now)
                    objCmd.Parameters.AddWithValue("@UserName", "Brian Misner")

                    'Open the connection.
                    DBConStringHelper.PrepareConnection(objConn)

                    'Execute the query.
                    objCmd.ExecuteNonQuery()

                    'Clear command objects and close the connection.
                    objCmd.Dispose()
                    objCmd = Nothing
                    DBConStringHelper.FinalizeConnection(objConn)

                    'The code below may not be necessary.
                    '----------------------------------------
                    'Insert the initial reporting party.
                    '-----------------------------------------------------------
                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Establish the query and enter it's parameters.
                    objCmd = New SqlCommand("spAutoWeatherFeedReportingParty", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@IncidentID", newlyAddedIncidentID)
                    objCmd.Parameters.AddWithValue("@FirstName", "NWS")

                    'Open the connection.
                    DBConStringHelper.PrepareConnection(objConn)

                    'Execute the query.
                    objCmd.ExecuteNonQuery()

                    'Clear command objects and open the connection.
                    objCmd.Dispose()
                    objCmd = Nothing
                    DBConStringHelper.FinalizeConnection(objConn)

                    'Insert the initial on-scene contact.
                    '-----------------------------------------------------------


                    'Insert the initial responsible party.
                    '-----------------------------------------------------------


                    '----------------------------------------
                    'The code above may not be necessary.

                    'Insert affected county data.
                    '-----------------------------------------------------------
                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Establish the query and enter it's parameters.
                    objCmd = New SqlCommand("spActionCountyRegionCheck", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@CountyRegionCheckID", 0)
                    objCmd.Parameters.AddWithValue("@IncidentID", newlyAddedIncidentID)
                    objCmd.Parameters.AddWithValue("@StateWide", Statewide)
                    objCmd.Parameters.AddWithValue("@Region1", Region1)
                    objCmd.Parameters.AddWithValue("@Region2", Region2)
                    objCmd.Parameters.AddWithValue("@Region3", Region3)
                    objCmd.Parameters.AddWithValue("@Region4", Region4)
                    objCmd.Parameters.AddWithValue("@Region5", Region5)
                    objCmd.Parameters.AddWithValue("@Region6", Region6)
                    objCmd.Parameters.AddWithValue("@Region7", Region7)
                    objCmd.Parameters.AddWithValue("@Bay", Bay)
                    objCmd.Parameters.AddWithValue("@Calhoun", Calhoun)
                    objCmd.Parameters.AddWithValue("@Escambia", Escambia)
                    objCmd.Parameters.AddWithValue("@Gulf", Gulf)
                    objCmd.Parameters.AddWithValue("@Holmes", Holmes)
                    objCmd.Parameters.AddWithValue("@Jackson", Jackson)
                    objCmd.Parameters.AddWithValue("@Okaloosa", Okaloosa)
                    objCmd.Parameters.AddWithValue("@SantaRosa", SantaRosa)
                    objCmd.Parameters.AddWithValue("@Walton", Walton)
                    objCmd.Parameters.AddWithValue("@Washington", Washington)
                    objCmd.Parameters.AddWithValue("@Columbia", Columbia)
                    objCmd.Parameters.AddWithValue("@Dixie", Dixie)
                    objCmd.Parameters.AddWithValue("@Franklin", Franklin)
                    objCmd.Parameters.AddWithValue("@Gadsden", Gadsden)
                    objCmd.Parameters.AddWithValue("@Hamilton", Hamilton)
                    objCmd.Parameters.AddWithValue("@Jefferson", Jefferson)
                    objCmd.Parameters.AddWithValue("@Lafayette", Lafayette)
                    objCmd.Parameters.AddWithValue("@Leon", Leon)
                    objCmd.Parameters.AddWithValue("@Levy", Levy)
                    objCmd.Parameters.AddWithValue("@Liberty", Liberty)
                    objCmd.Parameters.AddWithValue("@Madison", Madison)
                    objCmd.Parameters.AddWithValue("@Suwannee", Suwanee)
                    objCmd.Parameters.AddWithValue("@Taylor", Taylor)
                    objCmd.Parameters.AddWithValue("@Wakulla", Wakulla)
                    objCmd.Parameters.AddWithValue("@Alachua", Alachua)
                    objCmd.Parameters.AddWithValue("@Baker", Baker)
                    objCmd.Parameters.AddWithValue("@Bradford", Bradford)
                    objCmd.Parameters.AddWithValue("@Clay", Clay)
                    objCmd.Parameters.AddWithValue("@Duval", Duval)
                    objCmd.Parameters.AddWithValue("@Flagler", Flagler)
                    objCmd.Parameters.AddWithValue("@Gilchrist", Gilchrist)
                    objCmd.Parameters.AddWithValue("@Marion", Marion)
                    objCmd.Parameters.AddWithValue("@Nassau", Nassau)
                    objCmd.Parameters.AddWithValue("@Putnam", Putnam)
                    objCmd.Parameters.AddWithValue("@StJohns", StJohns)
                    objCmd.Parameters.AddWithValue("@Union", Union)
                    objCmd.Parameters.AddWithValue("@Citrus", Citrus)
                    objCmd.Parameters.AddWithValue("@Hardee", Hardee)
                    objCmd.Parameters.AddWithValue("@Hernando", Hernando)
                    objCmd.Parameters.AddWithValue("@Hillsborough", Hillsborough)
                    objCmd.Parameters.AddWithValue("@Pasco", Pasco)
                    objCmd.Parameters.AddWithValue("@Pinellas", Pinellas)
                    objCmd.Parameters.AddWithValue("@Polk", Polk)
                    objCmd.Parameters.AddWithValue("@Sumter", Sumter)
                    objCmd.Parameters.AddWithValue("@Brevard", Brevard)
                    objCmd.Parameters.AddWithValue("@IndianRiver", IndianRiver)
                    objCmd.Parameters.AddWithValue("@Lake", Lake)
                    objCmd.Parameters.AddWithValue("@Martin", Martin)
                    objCmd.Parameters.AddWithValue("@Orange", Orange)
                    objCmd.Parameters.AddWithValue("@Osceola", Osceola)
                    objCmd.Parameters.AddWithValue("@Seminole", Seminole)
                    objCmd.Parameters.AddWithValue("@StLucie", StLucie)
                    objCmd.Parameters.AddWithValue("@Volusia", Volusia)
                    objCmd.Parameters.AddWithValue("@Charlotte", Charlotte)
                    objCmd.Parameters.AddWithValue("@Collier", Collier)
                    objCmd.Parameters.AddWithValue("@DeSoto", DeSoto)
                    objCmd.Parameters.AddWithValue("@Glades", Glades)
                    objCmd.Parameters.AddWithValue("@Hendry", Hendry)
                    objCmd.Parameters.AddWithValue("@Highlands", Highlands)
                    objCmd.Parameters.AddWithValue("@Lee", Lee)
                    objCmd.Parameters.AddWithValue("@Manatee", Manatee)
                    objCmd.Parameters.AddWithValue("@Okeechobee", Okeechobee)
                    objCmd.Parameters.AddWithValue("@Sarasota", Sarasota)
                    objCmd.Parameters.AddWithValue("@Broward", Broward)
                    objCmd.Parameters.AddWithValue("@MiamiDade", MiamiDade)
                    objCmd.Parameters.AddWithValue("@Monroe", Monroe)
                    objCmd.Parameters.AddWithValue("@PalmBeach", PalmBeach)
                    objCmd.Parameters.AddWithValue("@Region1Affected", Region1Affected)
                    objCmd.Parameters.AddWithValue("@Region2Affected", Region2Affected)
                    objCmd.Parameters.AddWithValue("@Region3Affected", Region3Affected)
                    objCmd.Parameters.AddWithValue("@Region4Affected", Region4Affected)
                    objCmd.Parameters.AddWithValue("@Region5Affected", Region5Affected)
                    objCmd.Parameters.AddWithValue("@Region6Affected", Region6Affected)
                    objCmd.Parameters.AddWithValue("@Region7Affected", Region7Affected)

                    'Open the connection.
                    DBConStringHelper.PrepareConnection(objConn)

                    'Execute the query.
                    objCmd.ExecuteNonQuery()

                    'Clear command objects and close the connection.
                    objCmd.Dispose()
                    objCmd = Nothing
                    DBConStringHelper.FinalizeConnection(objConn)

                    'Insert worksheet data. Set incident type first.
                    '-----------------------------------------------------------
                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Establish the query and enter it's parameters.
                    objCmd = New SqlCommand("spInsertAutoWeatherFeedIncidentIncidentType", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@IncidentID", newlyAddedIncidentID)
                    objCmd.Parameters.AddWithValue("@IncidentTypeID", 40)
                    objCmd.Parameters.AddWithValue("@WorkSheetDescription", "NOAA Auto Weather Feed")

                    'Open the connection.
                    DBConStringHelper.PrepareConnection(objConn)

                    'Execute the query.
                    objCmd.ExecuteNonQuery()

                    'Clear command objects and close the connection.
                    objCmd.Dispose()
                    objCmd = Nothing
                    DBConStringHelper.FinalizeConnection(objConn)

                    'Select the incident incident type to insert worksheet data.
                    '-----------------------------------------------------------
                    Dim newlyAddedIncidentIncidentID As Integer

                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Open the connection.
                    objConn.Open()
                    objCmd = New SqlCommand("spSelectAutoWeatherUpdateIncidentIncidentTypeByIncidentID", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@IncidentID", newlyAddedIncidentID)

                    'Execute the query.
                    objDR = objCmd.ExecuteReader

                    If objDR.Read() Then
                        newlyAddedIncidentIncidentID = HelpFunction.ConvertdbnullsInt(objDR("IncidentIncidentTypeID"))
                    End If

                    'Clear command objects and close the connection.
                    objDR.Close()
                    objCmd.Dispose()
                    objCmd = Nothing
                    objConn.Close()

                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Establish the query and enter it's parameters.
                    objCmd = New SqlCommand("spInsertAutoWeatherFeedWorksheet", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@IncidentID", newlyAddedIncidentID)
                    objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", newlyAddedIncidentIncidentID)
                    objCmd.Parameters.AddWithValue("@SubType", wsSubType)
                    objCmd.Parameters.AddWithValue("@Situation", wsSituation)
                    objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", wsIncidentTypeLevelID)
                    objCmd.Parameters.AddWithValue("@WWAdateIssued", wsDateIssued)
                    objCmd.Parameters.AddWithValue("@WWAtime", wsTimeIssued)
                    objCmd.Parameters.AddWithValue("@WWAeffectiveDate", wsEffectiveDate)
                    objCmd.Parameters.AddWithValue("@WWAeffectiveTime", wsEffectiveTime)
                    objCmd.Parameters.AddWithValue("@WWAexpiresDate", wsExpireDate)
                    objCmd.Parameters.AddWithValue("@WWAexpiresTime", wsExpireTime)
                    objCmd.Parameters.AddWithValue("@WWAissuingOffice", wsIssuingOffice)
                    objCmd.Parameters.AddWithValue("@WWAadvisoryType", wsAdvisoryType)
                    objCmd.Parameters.AddWithValue("@WWAadvisoryText", wsAdvisoryText)

                    'Open the connection.
                    DBConStringHelper.PrepareConnection(objConn)

                    'Execute the query.
                    objCmd.ExecuteNonQuery()

                    'Clear command objects and close the connection.
                    objCmd.Dispose()
                    objCmd = Nothing
                    DBConStringHelper.FinalizeConnection(objConn)

                    'Insert most recent update.
                    '-----------------------------------------------------------
                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Establish the query and enter it's parameters.
                    objCmd = New SqlCommand("spInsertMostRecentUpdateByIncidentID", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@IncidentID", newlyAddedIncidentID)
                    objCmd.Parameters.AddWithValue("@UpdateDate", Now)
                    objCmd.Parameters.AddWithValue("@UserID", CreatedBy)
                    objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Added Weather Advisories and Reports")

                    'Open the connection.
                    DBConStringHelper.PrepareConnection(objConn)

                    'Execute the query.
                    objCmd.ExecuteNonQuery()

                    'Clear command objects and close the connection.
                    objCmd.Dispose()
                    objCmd = Nothing
                    DBConStringHelper.FinalizeConnection(objConn)

                    'Update the incident report.
                    '-----------------------------------------------------------
                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Establish the query and enter it's parameters.
                    objCmd = New SqlCommand("spUpdateIncidentReportUpdate", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@IncidentID", newlyAddedIncidentID)
                    objCmd.Parameters.AddWithValue("@LastUpdatedByID", CreatedBy)
                    objCmd.Parameters.AddWithValue("@LastUpdated", Now)

                    'Open the connection.
                    DBConStringHelper.PrepareConnection(objConn)

                    'Execute the query.
                    objCmd.ExecuteNonQuery()

                    'Clear command objects and close the connection.
                    objCmd.Dispose()
                    objCmd = Nothing
                    DBConStringHelper.FinalizeConnection(objConn)
                Else
                    'Clear variables.
                    AddedCounty = ""
                    isDuplicate = False
                    entryComplete = False
                    voidEntry = False

                    'Set county variables to false.
                    Region1 = False
                    Region2 = False
                    Region3 = False
                    Region4 = False
                    Region5 = False
                    Region6 = False
                    Region7 = False

                    Statewide = False

                    Region1Affected = False
                    Region2Affected = False
                    Region3Affected = False
                    Region4Affected = False
                    Region5Affected = False
                    Region6Affected = False
                    Region7Affected = False

                    Bay = False
                    Calhoun = False
                    Escambia = False
                    Gulf = False
                    Holmes = False
                    Jackson = False
                    Okaloosa = False
                    SantaRosa = False
                    Walton = False
                    Washington = False

                    Columbia = False
                    Dixie = False
                    Franklin = False
                    Gadsden = False
                    Hamilton = False
                    Jefferson = False
                    Lafayette = False
                    Leon = False
                    Liberty = False
                    Madison = False
                    Suwanee = False
                    Taylor = False
                    Wakulla = False

                    Alachua = False
                    Baker = False
                    Bradford = False
                    Clay = False
                    Duval = False
                    Flagler = False
                    Gilchrist = False
                    Levy = False
                    Marion = False
                    Nassau = False
                    Putnam = False
                    StJohns = False
                    Union = False

                    Citrus = False
                    Hardee = False
                    Hernando = False
                    Hillsborough = False
                    Pasco = False
                    Pinellas = False
                    Polk = False
                    Sumter = False

                    Brevard = False
                    IndianRiver = False
                    Lake = False
                    Martin = False
                    Orange = False
                    Osceola = False
                    Seminole = False
                    StLucie = False
                    Volusia = False

                    Charlotte = False
                    Collier = False
                    DeSoto = False
                    Glades = False
                    Hendry = False
                    Highlands = False
                    Lee = False
                    Manatee = False
                    Okeechobee = False
                    Sarasota = False

                    Broward = False
                    MiamiDade = False
                    Monroe = False
                    PalmBeach = False
                End If

                'Set entry complete to false to move to the next one.
                AddedCounty = ""
                entryComplete = False
                voidEntry = False

                'Set county variables to false.
                Region1 = False
                Region2 = False
                Region3 = False
                Region4 = False
                Region5 = False
                Region6 = False
                Region7 = False

                Statewide = False

                Region1Affected = False
                Region2Affected = False
                Region3Affected = False
                Region4Affected = False
                Region5Affected = False
                Region6Affected = False
                Region7Affected = False

                Bay = False
                Calhoun = False
                Escambia = False
                Gulf = False
                Holmes = False
                Jackson = False
                Okaloosa = False
                SantaRosa = False
                Walton = False
                Washington = False

                Columbia = False
                Dixie = False
                Franklin = False
                Gadsden = False
                Hamilton = False
                Jefferson = False
                Lafayette = False
                Leon = False
                Liberty = False
                Madison = False
                Suwanee = False
                Taylor = False
                Wakulla = False

                Alachua = False
                Baker = False
                Bradford = False
                Clay = False
                Duval = False
                Flagler = False
                Gilchrist = False
                Levy = False
                Marion = False
                Nassau = False
                Putnam = False
                StJohns = False
                Union = False

                Citrus = False
                Hardee = False
                Hernando = False
                Hillsborough = False
                Pasco = False
                Pinellas = False
                Polk = False
                Sumter = False

                Brevard = False
                IndianRiver = False
                Lake = False
                Martin = False
                Orange = False
                Osceola = False
                Seminole = False
                StLucie = False
                Volusia = False

                Charlotte = False
                Collier = False
                DeSoto = False
                Glades = False
                Hendry = False
                Highlands = False
                Lee = False
                Manatee = False
                Okeechobee = False
                Sarasota = False

                Broward = False
                MiamiDade = False
                Monroe = False
                PalmBeach = False
            Else
                If entryComplete = True Then
                    'Set entry complete to false to move to the next one.
                    AddedCounty = ""
                    entryComplete = False
                    voidEntry = False

                    'Set county variables to false.
                    Region1 = False
                    Region2 = False
                    Region3 = False
                    Region4 = False
                    Region5 = False
                    Region6 = False
                    Region7 = False

                    Statewide = False

                    Region1Affected = False
                    Region2Affected = False
                    Region3Affected = False
                    Region4Affected = False
                    Region5Affected = False
                    Region6Affected = False
                    Region7Affected = False

                    Bay = False
                    Calhoun = False
                    Escambia = False
                    Gulf = False
                    Holmes = False
                    Jackson = False
                    Okaloosa = False
                    SantaRosa = False
                    Walton = False
                    Washington = False

                    Columbia = False
                    Dixie = False
                    Franklin = False
                    Gadsden = False
                    Hamilton = False
                    Jefferson = False
                    Lafayette = False
                    Leon = False
                    Liberty = False
                    Madison = False
                    Suwanee = False
                    Taylor = False
                    Wakulla = False

                    Alachua = False
                    Baker = False
                    Bradford = False
                    Clay = False
                    Duval = False
                    Flagler = False
                    Gilchrist = False
                    Levy = False
                    Marion = False
                    Nassau = False
                    Putnam = False
                    StJohns = False
                    Union = False

                    Citrus = False
                    Hardee = False
                    Hernando = False
                    Hillsborough = False
                    Pasco = False
                    Pinellas = False
                    Polk = False
                    Sumter = False

                    Brevard = False
                    IndianRiver = False
                    Lake = False
                    Martin = False
                    Orange = False
                    Osceola = False
                    Seminole = False
                    StLucie = False
                    Volusia = False

                    Charlotte = False
                    Collier = False
                    DeSoto = False
                    Glades = False
                    Hendry = False
                    Highlands = False
                    Lee = False
                    Manatee = False
                    Okeechobee = False
                    Sarasota = False

                    Broward = False
                    MiamiDade = False
                    Monroe = False
                    PalmBeach = False
                End If
            End If

            'Runs the reader to parse the xml.
            '---------------------------------------------------
            Select Case reader.NodeType
                Case XmlNodeType.Element
                    'Checks to see what attribute is next.
                    '---------------------------------------------------
                    If reader.Name = "entry" Then inEntry = True
                    If reader.Name = "id" And inEntry = True Then gotEntryId = True
                    If reader.Name = "title" And inEntry = True Then gotEntryTitle = True
                    If reader.Name = "published" Then isPublished = True
                    If reader.Name = "summary" Then isSummary = True
                    If reader.Name = "cap:event" Then isCapEvent = True
                    If reader.Name = "cap:effective" Then isCapEffective = True
                    If reader.Name = "cap:expires" Then isCapExpires = True
                    If reader.Name = "cap:geocode" Then isCapGeocode = True
                    If reader.Name = "value" And isCapGeocode = True Then firstGeocodeValue = True
                Case XmlNodeType.Text
                    'Gets values based on whatever node is current.
                    '---------------------------------------------------
                    'If gotEntryId = True Then
                    '    If reader.Value.Length < 40 Then
                    '        gotEntryId = False
                    '    Else
                    '        AutoUpdateID = reader.Value.Substring(46, 14)
                    '        gotEntryId = False
                    '    End If
                    'End If
                    'If gotEntryId = True Then
                    '    AutoUpdateID = reader.Value
                    '    gotEntryId = False
                    'End If
                    If gotEntryId = True Then
                        AutoUpdateID = reader.Value.Substring(46)
                        gotEntryId = False
                    End If

                    If gotEntryTitle = True Then
                        wsName = reader.Value
                        gotEntryTitle = False
                    End If

                    If isPublished = True Then
                        IncidentOccurredDate = Convert.ToDateTime(reader.Value.Substring(0, 10))
                        IncidentOccurredTime = reader.Value.Substring(11, 8).Replace(":", "").Remove(4)
                        ReportedToSWODate = IncidentOccurredDate
                        ReportedToSWOTime = IncidentOccurredTime
                        wsDateIssued = IncidentOccurredDate
                        wsTimeIssued = IncidentOccurredTime
                        isPublished = False
                    End If

                    If isSummary = True Then
                        wsAdvisoryText = reader.Value
                        isSummary = False
                    End If

                    If isCapEvent = True Then
                        If reader.Value.Contains("Severe Thunderstorm Watch") = True Or reader.Value.Contains("Severe Thunderstorm Warning") = True Or reader.Value.Contains("Tornado Watch") = True Or reader.Value.Contains("Tornado Warning") = True Or reader.Value.Contains("Flood Watch") = True Or reader.Value.Contains("Flood Warning") = True Or reader.Value.Contains("Flash Flood Watch") = True Or reader.Value.Contains("Flash Flood Warning") = True Or reader.Value.Contains("Coastal Flood Watch") = True Or reader.Value.Contains("Coastal Flood Warning") = True Or reader.Value.Contains("Hurricane Watch") = True Or reader.Value.Contains("Hurricane Warning") = True Or reader.Value.Contains("Freeze Warning") = True Or reader.Value.Contains("Tropical Storm Watch") = True Or reader.Value.Contains("Tropical Storm Warning") = True Or reader.Value.Contains("Extreme Wind Warning") = True Or reader.Value.Contains("High Wind Warning") = True Or reader.Value.Contains("Civil Emergency Message") = True Or reader.Value.Contains("Tsunami Advisory") = True Or reader.Value.Contains("Tsunami Watch") = True Or reader.Value.Contains("Tsunami Warning") = True Or reader.Value.Contains("Freeze Watch") = True Or reader.Value.Contains("Hard Freeze Watch") = True Or reader.Value.Contains("Hard Freeze Warning") = True Or reader.Value.Contains("Wind Chill Advisory") = True Or reader.Value.Contains("Wind Chill Watch") = True Or reader.Value.Contains("Wind Chill Warning") = True Or reader.Value.Contains("Excessive Heat Watch") = True Or reader.Value.Contains("Excessive Heat Warning") = True Or reader.Value.Contains("Dense Smoke Advisory") = True Or reader.Value.Contains("Dense Fog Advisory") = True Or reader.Value.Contains("Winter Storm Watch") = True Or reader.Value.Contains("Winter Storm Warning") = True Then
                            IncidentName = reader.Value
                            voidEntry = False
                            If reader.Value.Contains("Watch") = True Then
                                wsSubType = "Weather Watch"
                            ElseIf reader.Value.Contains("Warning") = True Then
                                wsSubType = "Weather Warning"
                            Else
                                wsSubType = "Weather Advisory"
                            End If
                            If reader.Value.Contains("Advsiory") = True Or reader.Value.Contains("Watch") = True Or reader.Value.Contains("Warning") = True Then
                                wsAdvisoryType = reader.Value
                            Else
                                wsAdvisoryType = "Select an Option"
                            End If
                        Else
                            voidEntry = True
                        End If

                        isCapEvent = False
                    End If

                    If isCapEffective = True Then
                        wsEffectiveDate = Convert.ToDateTime(reader.Value.Substring(0, 10))
                        wsEffectiveTime = reader.Value.Substring(11, 8).Replace(":", "").Remove(4)
                        isCapEffective = False
                    End If

                    If isCapExpires = True Then
                        wsExpireDate = Convert.ToDateTime(reader.Value.Substring(0, 10))
                        wsExpireTime = reader.Value.Substring(11, 8).Replace(":", "").Remove(4)
                        isCapExpires = False
                    End If

                    If firstGeocodeValue = True Then
                        Dim counties As String() = reader.Value.Split(New Char() {" "c})

                        Dim Region1Count As Integer = 0
                        Dim Region2Count As Integer = 0
                        Dim Region3Count As Integer = 0
                        Dim Region4Count As Integer = 0
                        Dim Region5Count As Integer = 0
                        Dim Region6Count As Integer = 0
                        Dim Region7Count As Integer = 0

                        Dim StatewideCount As Integer = 0

                        For i As Integer = 0 To counties.Length - 1
                            counties(i) = counties(i).Remove(0, 1)

                            Try
                                'Get the county name(s).
                                '-----------------------------------------------------------
                                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                                'Establish the query and enter it's parameters.
                                objCmd = New SqlCommand("spSelectCountyNameByFIPS", objConn)
                                objCmd.CommandType = CommandType.StoredProcedure
                                objCmd.Parameters.AddWithValue("@FIPS", Convert.ToInt16(counties(i)))

                                'Open the connection.
                                DBConStringHelper.PrepareConnection(objConn)

                                'Execute the query.
                                objDR = objCmd.ExecuteReader

                                If objDR.Read() Then
                                    AddedCounty = AddedCounty & objDR("County").ToString.Trim & ", "

                                    'Region 1 start.
                                    If objDR("County").ToString.Trim = "Bay" Then
                                        Bay = True
                                        Region1Affected = True
                                        Region1Count = Region1Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Calhoun" Then
                                        Calhoun = True
                                        Region1Affected = True
                                        Region1Count = Region1Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Escambia" Then
                                        Escambia = True
                                        Region1Affected = True
                                        Region1Count = Region1Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Gulf" Then
                                        Gulf = True
                                        Region1Affected = True
                                        Region1Count = Region1Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Holmes" Then
                                        Holmes = True
                                        Region1Affected = True
                                        Region1Count = Region1Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Jackson" Then
                                        Jackson = True
                                        Region1Affected = True
                                        Region1Count = Region1Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Okaloosa" Then
                                        Okaloosa = True
                                        Region1Affected = True
                                        Region1Count = Region1Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "SantaRosa" Then
                                        SantaRosa = True
                                        Region1Affected = True
                                        Region1Count = Region1Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Walton" Then
                                        Walton = True
                                        Region1Affected = True
                                        Region1Count = Region1Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Washington" Then
                                        Washington = True
                                        Region1Affected = True
                                        Region1Count = Region1Count + 1
                                    End If

                                    If Region1Count = 10 Then
                                        Region1 = True
                                        StatewideCount = StatewideCount + 1
                                    End If
                                    'Region 1 end.

                                    'Region 2 start.
                                    If objDR("County").ToString.Trim = "Columbia" Then
                                        Columbia = True
                                        Region2Affected = True
                                        Region2Count = Region2Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Dixie" Then
                                        Dixie = True
                                        Region2Affected = True
                                        Region2Count = Region2Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Franklin" Then
                                        Franklin = True
                                        Region2Affected = True
                                        Region2Count = Region2Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Gadsden" Then
                                        Gadsden = True
                                        Region2Affected = True
                                        Region2Count = Region2Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Hamilton" Then
                                        Hamilton = True
                                        Region2Affected = True
                                        Region2Count = Region2Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Jefferson" Then
                                        Jefferson = True
                                        Region2Affected = True
                                        Region2Count = Region2Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Lafayette" Then
                                        Lafayette = True
                                        Region2Affected = True
                                        Region2Count = Region2Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Leon" Then
                                        Leon = True
                                        Region2Affected = True
                                        Region2Count = Region2Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Liberty" Then
                                        Liberty = True
                                        Region2Affected = True
                                        Region2Count = Region2Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Madison" Then
                                        Madison = True
                                        Region2Affected = True
                                        Region2Count = Region2Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Suwanee" Then
                                        Suwanee = True
                                        Region2Affected = True
                                        Region2Count = Region2Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Taylor" Then
                                        Taylor = True
                                        Region2Affected = True
                                        Region2Count = Region2Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Wakulla" Then
                                        Wakulla = True
                                        Region2Affected = True
                                        Region2Count = Region2Count + 1
                                    End If

                                    If Region2Count = 13 Then
                                        Region2 = True
                                        StatewideCount = StatewideCount + 1
                                    End If
                                    'Region 2 end.

                                    'Region 3 start.
                                    If objDR("County").ToString.Trim = "Alachua" Then
                                        Alachua = True
                                        Region3Affected = True
                                        Region3Count = Region3Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Baker" Then
                                        Baker = True
                                        Region3Affected = True
                                        Region3Count = Region3Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Bradford" Then
                                        Bradford = True
                                        Region3Affected = True
                                        Region3Count = Region3Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Clay" Then
                                        Clay = True
                                        Region3Affected = True
                                        Region3Count = Region3Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Duval" Then
                                        Duval = True
                                        Region3Affected = True
                                        Region3Count = Region3Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Flagler" Then
                                        Flagler = True
                                        Region3Affected = True
                                        Region3Count = Region3Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Gilchrist" Then
                                        Gilchrist = True
                                        Region3Affected = True
                                        Region3Count = Region3Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Levy" Then
                                        Levy = True
                                        Region3Affected = True
                                        Region3Count = Region3Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Marion" Then
                                        Marion = True
                                        Region3Affected = True
                                        Region3Count = Region3Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Nassau" Then
                                        Nassau = True
                                        Region3Affected = True
                                        Region3Count = Region3Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Putnam" Then
                                        Putnam = True
                                        Region3Affected = True
                                        Region3Count = Region3Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "StJohns" Then
                                        StJohns = True
                                        Region3Affected = True
                                        Region3Count = Region3Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Union" Then
                                        Union = True
                                        Region3Affected = True
                                        Region3Count = Region3Count + 1
                                    End If

                                    If Region3Count = 13 Then
                                        Region3 = True
                                        StatewideCount = StatewideCount + 1
                                    End If
                                    'Region 3 end.

                                    'Region 4 start.
                                    If objDR("County").ToString.Trim = "Citrus" Then
                                        Citrus = True
                                        Region4Affected = True
                                        Region4Count = Region4Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Hardee" Then
                                        Hardee = True
                                        Region4Affected = True
                                        Region4Count = Region4Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Hernando" Then
                                        Hernando = True
                                        Region4Affected = True
                                        Region4Count = Region4Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Hillsborough" Then
                                        Hillsborough = True
                                        Region4Affected = True
                                        Region4Count = Region4Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Pasco" Then
                                        Pasco = True
                                        Region4Affected = True
                                        Region4Count = Region4Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Pinellas" Then
                                        Pinellas = True
                                        Region4Affected = True
                                        Region4Count = Region4Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Polk" Then
                                        Polk = True
                                        Region4Affected = True
                                        Region4Count = Region4Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Sumter" Then
                                        Sumter = True
                                        Region4Affected = True
                                        Region4Count = Region4Count + 1
                                    End If

                                    If Region4Count = 8 Then
                                        Region4 = True
                                        StatewideCount = StatewideCount + 1
                                    End If
                                    'Region 4 end.

                                    'Region 5 start.
                                    If objDR("County").ToString.Trim = "Brevard" Then
                                        Brevard = True
                                        Region5Affected = True
                                        Region5Count = Region5Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "IndianRiver" Then
                                        IndianRiver = True
                                        Region5Affected = True
                                        Region5Count = Region5Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Lake" Then
                                        Lake = True
                                        Region5Affected = True
                                        Region5Count = Region5Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Martin" Then
                                        Martin = True
                                        Region5Affected = True
                                        Region5Count = Region5Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Orange" Then
                                        Orange = True
                                        Region5Affected = True
                                        Region5Count = Region5Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Osceola" Then
                                        Osceola = True
                                        Region5Affected = True
                                        Region5Count = Region5Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Seminole" Then
                                        Seminole = True
                                        Region5Affected = True
                                        Region5Count = Region5Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "StLucie" Then
                                        StLucie = True
                                        Region5Affected = True
                                        Region5Count = Region5Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Volusia" Then
                                        Volusia = True
                                        Region5Affected = True
                                        Region5Count = Region5Count + 1
                                    End If

                                    If Region5Count = 9 Then
                                        Region5 = True
                                        StatewideCount = StatewideCount + 1
                                    End If
                                    'Region 5 end.

                                    'Region 6 start.
                                    If objDR("County").ToString.Trim = "Charlotte" Then
                                        Charlotte = True
                                        Region6Affected = True
                                        Region6Count = Region6Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Collier" Then
                                        Collier = True
                                        Region6Affected = True
                                        Region6Count = Region6Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "DeSoto" Then
                                        DeSoto = True
                                        Region6Affected = True
                                        Region6Count = Region6Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Glades" Then
                                        Glades = True
                                        Region6Affected = True
                                        Region6Count = Region6Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Hendry" Then
                                        Hendry = True
                                        Region6Affected = True
                                        Region6Count = Region6Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Highlands" Then
                                        Highlands = True
                                        Region6Affected = True
                                        Region6Count = Region6Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Lee" Then
                                        Lee = True
                                        Region6Affected = True
                                        Region6Count = Region6Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Manatee" Then
                                        Manatee = True
                                        Region6Affected = True
                                        Region6Count = Region6Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Okeechobee" Then
                                        Okeechobee = True
                                        Region6Affected = True
                                        Region6Count = Region6Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Sarasota" Then
                                        Sarasota = True
                                        Region6Affected = True
                                        Region6Count = Region6Count + 1
                                    End If

                                    If Region6Count = 9 Then
                                        Region6 = True
                                        StatewideCount = StatewideCount + 1
                                    End If
                                    'Region 6 end.

                                    'Region 7 start.
                                    If objDR("County").ToString.Trim = "Broward" Then
                                        Broward = True
                                        Region7Affected = True
                                        Region7Count = Region7Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "MiamiDade" Then
                                        MiamiDade = True
                                        Region7Affected = True
                                        Region7Count = Region7Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "Monroe" Then
                                        Monroe = True
                                        Region7Affected = True
                                        Region7Count = Region7Count + 1
                                    End If

                                    If objDR("County").ToString.Trim = "PalmBeach" Then
                                        PalmBeach = True
                                        Region7Affected = True
                                        Region7Count = Region7Count + 1
                                    End If

                                    If Region7Count = 5 Then
                                        Region7 = True
                                        StatewideCount = StatewideCount + 1
                                    End If
                                    'Region 7 end.

                                    If StatewideCount = 7 Then
                                        Statewide = True
                                    End If
                                End If

                                'Close the connection.
                                objCmd.Dispose()
                                objCmd = Nothing
                                DBConStringHelper.FinalizeConnection(objConn)
                            Catch ex As Exception

                            End Try
                        Next

                        If AddedCounty <> "" Then AddedCounty = AddedCounty.Remove(AddedCounty.Length - 2)

                        firstGeocodeValue = False
                        isCapGeocode = False
                        inEntry = False
                        entryComplete = True
                        If voidEntry = True Then
                            voidEntry = True
                        Else
                            voidEntry = False
                        End If
                    End If
            End Select
        Loop
    End Sub

    Private Sub LoadSavedFormFields()
        oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        If Not oCookie Is Nothing Then
            If Not String.IsNullOrEmpty(oCookie.Item("IncidentSearchText")) Then txtSearch.Text = Server.HtmlEncode(oCookie.Item("IncidentSearchText"))
            If Not String.IsNullOrEmpty(oCookie.Item("IncidentSearchListValue")) Then ddlSearchBy.SelectedValue = oCookie.Item("IncidentSearchListValue")
            If Not String.IsNullOrEmpty(oCookie.Item("IncidentFilterListValue")) Then ddlIncidentType.SelectedValue = oCookie.Item("IncidentFilterListValue")
            If Not String.IsNullOrEmpty(oCookie.Item("AgencyFilterListValue")) Then ddlAgency.SelectedValue = oCookie.Item("AgencyFilterListValue")
        End If
    End Sub

    Protected Sub btnReset_Click(sender As Object, e As System.EventArgs) Handles btnReset.Click
        If oCookie Is Nothing Then oCookie = Response.Cookies(Application("ApplicationEnvironment").ToString)
        txtSearch.Text = ""
        ddlSearchBy.SelectedValue = "[IncidentNumber].Number"
        ddlIncidentType.ClearSelection()
        ddlIncidentType.Items.FindByText("All Worksheets").Selected = True
        ddlAgency.ClearSelection()
        ddlAgency.Items.FindByText("Select An Agency").Selected = True
        oCookie.Item("IncidentSearchText") = "" 'txtSearch.Text
        oCookie.Item("IncidentSearchListValue") = "" 'ddlSearchBy.SelectedValue
        oCookie.Item("IncidentFilterListValue") = "" 'ddlIncidentType.SelectedValue
        oCookie.Item("AgencyFilterListValue") = ""
        oCookie.Expires = DateTime.Now.AddDays(90)
        Response.Cookies.Add(oCookie)
        IncidentDataGrid.CurrentPageIndex = 0
        getIncident("[IncidentID] DESC", ddlSearchBy.SelectedValue, "")
        LoadSavedFormFields()
    End Sub
End Class