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
Imports System.Web.Services
Imports System.IO
Imports System.Net
Imports System.Xml
Imports System.Text
Imports Microsoft.Office.Interop.Word
Imports System.Diagnostics

Partial Class LiveTestPage
    Inherits System.Web.UI.Page
'Help Functions from our App_Code
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    Public objDataGridFunctions As New DataGridFunctions

    'For Connecting to the database
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet


    Dim ParamId As SqlParameter

    Public AuditHelper As New AuditHelp

    Public MrDataGrabber As New DataGrabber

    Public MrEmail As New Email

    Dim globalRecordCount As Integer
    Dim globalCountyCount As Integer
    Dim globalAuditAction As String = ""
    Dim globalHasErrors As Boolean = False
    Dim globalMessage As String
    Dim globalCurrentStep As Integer
    Dim globalIsSaved As Boolean = False
    Dim globalIsPreSaved As Boolean = False
    Dim globalAction As String
    Dim globalParameter As String
    'Dim oCookie As System.Web.HttpCookie
    Dim ns As New SecurityValidate
    Const js As String = "TADDScript.js"

    'Page Load
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim intShowReortingParty As Integer = 0

        If Request("IncidentID") = "" Then

            Response.Redirect("Incident.aspx")

        End If

        intShowReortingParty = MrDataGrabber.GrabRecordCountBy2Keys("IncidentIncidentType", "IncidentTypeID", "18", "IncidentID", Request("IncidentID"))

        'Response.Write(intShowReortingParty)

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        If Page.IsPostBack = False Then

            'set message
            globalMessage = Request("Message")
            globalAction = Request("Action")
            globalParameter = Request("Parameter")

            PopulateDDLs()

            Select Case globalAction

                Case "Delete"
                    'Response.Write("1")
                    If ns.UserLevelID = "1" Then

                        Select Case globalParameter
                            Case "IncidentType"
                                DeleteIncidentType()
                            Case "Attachment"
                                DeleteAttachment()
                            Case "Link"
                                DeleteLink()
                            Case Else

                        End Select

                    End If

                Case Else

            End Select

            globalParameter = Request("Parameter")

            'Select Case globalParameter

            '    Case "WorkSheet"

            '        txtLinkName.Focus()

            '    Case Else

            'End Select

            PopulatePage()


            Select Case ns.UserLevelID.ToString() 'oCookie.Item("UserLevelID")

                Case "1" 'Admin

                Case "2" 'Full User

                Case "3" 'Update User
                    'btnUpdateInitialReport.Enabled = False
                    btnAddAttachment.Enabled = False
                    btnAddLink.Enabled = False
                    btnAddIncidentType.Enabled = False
                    btnSave.Disabled = True
                    lnkAddAffectedCounty.Enabled = False
                    lnkNotify.Visible = False
                    If intShowReortingParty <> 0 Then
                        pnlShowReportingParty.Visible = False
                        ddlReportingPartyType.Enabled = False
                        pnlShowReportingPartyCensored.Visible = True
                    End If
                Case "4", "5" 'Read Only and Read Only + Hazmat
                    'btnUpdateInitialReport.Enabled = False
                    btnUpdateReport.Enabled = False
                    btnAddAttachment.Enabled = False
                    btnAddLink.Enabled = False
                    btnAddIncidentType.Enabled = False
                    btnSave.Disabled = True
                    lnkAddAffectedCounty.Enabled = False
                    lnkNotify.Visible = False

                    If intShowReortingParty <> 0 Then
                        pnlShowReportingParty.Visible = False
                        ddlReportingPartyType.Enabled = False
                        pnlShowReportingPartyCensored.Visible = True
                    End If

                Case Else

            End Select

        End If

    End Sub



    'Populations Subs
    Protected Sub PopulatePage()

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        If Request("IncidentID") = "0" Then
            'We Create a temp Incident
            AddInitialIncident()
        End If

        'Check To See If the Incident Has Been Saved
        globalIsSaved = MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("Saved", "Incident", "IncidentID", Request("IncidentID"))

        If globalIsSaved = False Then

            If globalIsPreSaved = False Then

                txtIncidentOccurredDate.Text = DateValue(Now)

                txtIncidentOccurredTime.Text = Format(Now, "HH")

                txtIncidentOccurredTime2.Text = Format(Now, "mm")

                txtReportedToSWODate.Text = DateValue(Now)

                txtReportedToSWOTime.Text = Format(Now, "HH")

                txtReportedToSWOTime2.Text = Format(Now, "mm")

                lblIncidentNumber.Text = "N/A"

            End If

            LinkDataGrid.Columns(1).Visible = False
            AttachmentDataGrid.Columns(1).Visible = False
            IncidentIncidentTypeDataGrid.Columns(1).Visible = False

        End If

        getAffectedCounty()

        getAttachment()

        getLink()

        getIncidentIncidentType()

        'Incident Type Starts Here
        Dim localIncidentTypeCount As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectIncidentIncidentTypeCountByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            localIncidentTypeCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        'Hiding the Indexing of the Grids if we don't have enough rows to index


        If localIncidentTypeCount <= 4 Then

            IncidentIncidentTypeDataGrid.AllowPaging = False

        End If

        If localIncidentTypeCount > 0 Then

            pnlShowIncidentTypeGrid.Visible = True

        End If


        getIncidentIncidentType()


        If localIncidentTypeCount <> 0 Then


            ddlIncidentType.Items.Clear()

            If ns.UserLevelID = "1" Then

                '=======================================================================================================
                'IncidentType
                '=======================================================================================================
                ddlIncidentType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objCmd = New SqlCommand("spSelectIncidentType", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                'objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

                'objCmd.Parameters.AddWithValue("@OrderBy", "") Optional Parameter

                DBConStringHelper.PrepareConnection(objConn) 'Open the connection
                ddlIncidentType.DataSource = objCmd.ExecuteReader()
                ddlIncidentType.DataBind()
                DBConStringHelper.FinalizeConnection(objConn) 'Close the connection

                objCmd = Nothing

                'add an "Select an Option" item to the list
                ddlIncidentType.Items.Insert(0, New ListItem("Select An Incident Type", "0"))
                ddlIncidentType.Items(0).Selected = True

            Else

                'Must Find the user Level and grab Worksheets associated to that User except the Admin
                Dim strIncidentTypeID As String = ""

                'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
                '// Add cookie
                'Response.Cookies.Add(oCookie)
                ns = Session("Security_Tracker")


                'oCookie.Item("UserID")

                strIncidentTypeID = MrDataGrabber.GrabIncidentTypeUserByUserID(ns.UserID.ToString())


                'Response.Write(strIncidentTypeID)
                '=======================================================================================================
                'IncidentType
                '=======================================================================================================
                ddlIncidentType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objCmd = New SqlCommand("spSelectIncidentTypeByMultipleIncidentTypeID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentTypeID", strIncidentTypeID)

                'objCmd.Parameters.AddWithValue("@OrderBy", "") Optional Parameter

                DBConStringHelper.PrepareConnection(objConn) 'Open the connection
                ddlIncidentType.DataSource = objCmd.ExecuteReader()
                ddlIncidentType.DataBind()
                DBConStringHelper.FinalizeConnection(objConn) 'Close the connection

                objCmd = Nothing

                'add an "Select an Option" item to the list
                ddlIncidentType.Items.Insert(0, New ListItem("Select An Incident Type", "0"))
                ddlIncidentType.Items(0).Selected = True

            End If


        End If

        ''Incident Type Ends Here

        globalIsSaved = MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("Saved", "Incident", "IncidentID", Request("IncidentID"))

        If globalIsSaved = True Then

            'txtInitialReport.Enabled = False
            pnlShowIncidentTypes.Visible = True
            pnlShowReportUpdate.Visible = True
            pnlShowAttachmentsLinks.Visible = True

            lblCreatedBy.Text = MrDataGrabber.GrabUserFullNameByUserID(MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("CreatedByID", "Incident", "IncidentID", Request("IncidentID")))
            lblUpdatedBy.Text = MrDataGrabber.GrabUserFullNameByUserID(MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("LastUpdatedByID", "Incident", "IncidentID", Request("IncidentID")))
            lblLastUpdatedOn.Text = MrDataGrabber.GrabOneDateStringColumnAsMilitaryTimeByPrimaryKey("LastUpdated", "Incident", "IncidentID", Request("IncidentID"))
            lblCreatedOn.Text = MrDataGrabber.GrabOneDateStringColumnAsMilitaryTimeByPrimaryKey("DateCreated", "Incident", "IncidentID", Request("IncidentID"))


            'MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("Saved", "Incident", "IncidentID", Request("IncidentID"))
            'MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("Saved", "Incident", "IncidentID", Request("IncidentID"))
            'MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("Saved", "Incident", "IncidentID", Request("IncidentID"))

            Dim localTime As String = ""
            Dim localTime2 As String = ""
            Dim localObtainCoordinate As String = ""
            Dim localCoordinateType As String = ""
            Dim localLat As Decimal
            Dim localLong As Decimal
            Dim localUSNG As String = ""

            ''Grabs Initial Report
            'txtInitialReport.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("InitialReport", "Incident", "IncidentID", Request("IncidentID"))
            GrabInitialReport()

            GrabReportUpdate()

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectIncidentByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                'localIncidentTypeCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))
                txtIncidentName.Text = HelpFunction.Convertdbnulls(objDR("IncidentName"))
                ddlIncidentStatus.SelectedValue = HelpFunction.Convertdbnulls(objDR("IncidentStatusID"))
                ddlIsThisADrill.SelectedValue = HelpFunction.Convertdbnulls(objDR("IsThisADrill"))
                ddlStateAssistance.SelectedValue = HelpFunction.Convertdbnulls(objDR("StateAssistance"))
                ddlReportingPartyType.SelectedValue = HelpFunction.ConvertdbnullsInt(objDR("ReportingPartyTypeID"))
                ddlResponsiblePartyType.SelectedValue = HelpFunction.ConvertdbnullsInt(objDR("ResponsiblePartyTypeID"))
                ddlOnSceneContactType.SelectedValue = HelpFunction.ConvertdbnullsInt(objDR("OnSceneContactTypeID"))
                localTime = CStr(HelpFunction.Convertdbnulls(objDR("ReportedToSWOTime")))
                txtReportedToSWODate.Text = HelpFunction.Convertdbnulls(objDR("ReportedToSWODate"))
                localTime2 = CStr(HelpFunction.Convertdbnulls(objDR("IncidentOccurredTime")))
                txtIncidentOccurredDate.Text = HelpFunction.Convertdbnulls(objDR("IncidentOccurredDate"))

                txtFacilityNameSceneDescription.Text = HelpFunction.Convertdbnulls(objDR("FacilityNameSceneDescription"))
                txtAddress.Text = HelpFunction.Convertdbnulls(objDR("Address"))
                txtCity.Text = HelpFunction.Convertdbnulls(objDR("City"))
                txtAddress2.Text = HelpFunction.Convertdbnulls(objDR("Address2"))
                txtZip.Text = HelpFunction.Convertdbnulls(objDR("Zip"))
                txtStreet.Text = HelpFunction.Convertdbnulls(objDR("Street"))
                txtStreet2.Text = HelpFunction.Convertdbnulls(objDR("Street2"))
                txtCity2.Text = HelpFunction.Convertdbnulls(objDR("City2"))

                localObtainCoordinate = HelpFunction.Convertdbnulls(objDR("ObtainCoordinate"))
                localCoordinateType = HelpFunction.Convertdbnulls(objDR("CoordinateType"))
                localLat = HelpFunction.ConvertdbnullsDbl(objDR("Lat"))
                localLong = HelpFunction.ConvertdbnullsDbl(objDR("Long"))
                localUSNG = HelpFunction.Convertdbnulls(objDR("USNG"))

                ddlSeverity.SelectedValue = HelpFunction.Convertdbnulls(objDR("SeverityID"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            ddlSeverity.Style.Add("background-color", MrDataGrabber.GrabOneStringColumnByPrimaryKey("Color", "Severity", "SeverityID", ddlSeverity.SelectedValue))

            lnkLocation.NavigateUrl = "ViewMaps.aspx?Lat=" & localLat & "&Long=" & localLong


            'IncidentNumber
            Dim localYear As String = ""
            Dim localNumber As Integer

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectIncidentNumberByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                localYear = HelpFunction.Convertdbnulls(objDR("Year"))
                localNumber = HelpFunction.ConvertdbnullsInt(objDR("Number"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            lblIncidentNumber.Text = localYear & "-" & CStr(localNumber)

            pnlShowViewLocation.Visible = True

            pnlShowViewAllReportUpdates.Visible = True
            lnkViewAllReportUpdates.NavigateUrl = "AllUpdates.aspx?IncidentID=" & Request("IncidentID")

            pnlShowViewFullReport.Visible = True
            lnkViewFullReport.NavigateUrl = "FullReport.aspx?IncidentID=" & Request("IncidentID") & "&ReportFormat=HTML"


            pnlShowViewFullReportText.Visible = True
            lnkViewFullReportText.NavigateUrl = "FullReportPlainText.aspx?IncidentID=" & Request("IncidentID") & "&ReportFormat=HTML"


            'Picking the ObtainCoordinate Value
            If localObtainCoordinate = "FacilityNameSceneDescription" Then
                rdoFacilityNameSceneDescription.Checked = True
            ElseIf localObtainCoordinate = "AddressCity" Then
                rdoAddressCity.Checked = True
            ElseIf localObtainCoordinate = "AddressZip" Then
                rdoByAddressZip.Checked = True
            ElseIf localObtainCoordinate = "Intersection" Then
                rdoByIntersection.Checked = True
            ElseIf localObtainCoordinate = "AffectedCounties" Then
                rdoAffectedCounties.Checked = True
            ElseIf localObtainCoordinate = "CoordinateEntry" Then
                rdoByCoordinateEntry.Checked = True
                pnlShowCoordinates.Visible = True

                txtLatDecimalDegrees.Text = localLat
                txtLongDecimalDegrees.Text = localLong

                If localLat <> 0.0 And localLong <> 0.0 Then
                    Convert2()
                End If

                'Picking the localCoordinateType Value
                If localCoordinateType = "DecimalDegrees" Then
                    rdoDecimalDegrees.Checked = True

                    pnlShowDecimalDegrees.Visible = True
                    pnlShowDegreesMinutes.Visible = False
                    pnlShowDegreesMinutesSeconds.Visible = False
                    pnlShowUSNG.Visible = False

                ElseIf localCoordinateType = "DegreesMinutes" Then
                    PopulateCoordTextBoxes()
                    rdoDegreesMinutes.Checked = True

                    pnlShowDecimalDegrees.Visible = False
                    pnlShowDegreesMinutes.Visible = True
                    pnlShowDegreesMinutesSeconds.Visible = False
                    pnlShowUSNG.Visible = False

                ElseIf localCoordinateType = "DegreesMinutesSeconds" Then
                    PopulateCoordTextBoxes()
                    rdoDegreesMinutesSeconds.Checked = True

                    pnlShowDecimalDegrees.Visible = False
                    pnlShowDegreesMinutes.Visible = False
                    pnlShowDegreesMinutesSeconds.Visible = True
                    pnlShowUSNG.Visible = False

                ElseIf localCoordinateType = "USNG" Then
                    rdoUSNG.Checked = True
                    txtUSNG.Text = localUSNG

                    pnlShowDecimalDegrees.Visible = False
                    pnlShowDegreesMinutes.Visible = False
                    pnlShowDegreesMinutesSeconds.Visible = False
                    pnlShowUSNG.Visible = True

                ElseIf localCoordinateType = "N/A" Then
                    'Noting
                Else
                    'Noting
                End If

            Else
                'Noting
            End If

            If ddlReportingPartyType.SelectedValue = 3 Then
                pnlShowReportingParty.Visible = True
                PopulateReportingParty()
            End If

            If ddlResponsiblePartyType.SelectedValue = 4 Then
                pnlShowResponsibleParty.Visible = True
                PopulateResponsibleParty()
            End If

            If ddlOnSceneContactType.SelectedValue = 2 Then
                pnlShowOnSceneContact.Visible = True
                PopulateOnSceneContact()
            End If

            txtReportedToSWOTime.Text = Left(localTime, 2)
            txtReportedToSWOTime2.Text = Right(localTime, 2)


            If txtReportedToSWOTime.Text = "0" Then
                txtReportedToSWOTime.Text = ""
            End If

            If txtReportedToSWOTime2.Text = "0" Then
                txtReportedToSWOTime2.Text = ""
            End If

            txtIncidentOccurredTime.Text = Left(localTime2, 2)
            txtIncidentOccurredTime2.Text = Right(localTime2, 2)


            If txtIncidentOccurredTime.Text = "0" Then
                txtIncidentOccurredTime.Text = ""
            End If

            If txtIncidentOccurredTime2.Text = "0" Then
                txtIncidentOccurredTime2.Text = ""
            End If

            'If oCookie.Item("UserLevelID") = "1" Or oCookie.Item("UserLevelID") = "2" Then
            '    btnUpdateInitialReport.Enabled = True
            'End If

        Else
            lnkNotify.Enabled = False
            lblCreatedBy.Text = "N/A"
            lblCreatedOn.Text = "N/A"
            lblLastUpdatedOn.Text = "N/A"
            lblUpdatedBy.Text = "N/A"

        End If

        'Shows all the Updates
        lnkAllUpdates.NavigateUrl = "ViewUpdates.aspx?IncidentID=" & Request("IncidentID")
        'lnkAllUpdates.ImageUrl = "Images/UpdateIcon2.jpg"
        lnkInitialReportUpdates.NavigateUrl = "ViewIntialChanges.aspx?IncidentID=" & Request("IncidentID")

        Dim localMostRecentUpdateCount As Integer

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectCountFromMostRecentUpdateByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            localMostRecentUpdateCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        If localMostRecentUpdateCount = 0 Then
            'lblLatestUpdate.Text = "There has been No Report Updates at this time."
        Else

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectLatestUpdateChangeFromUpdateChangeByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                lblLatestUpdate.Text = HelpFunction.Convertdbnulls(objDR("UpdateReport"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

        End If


    End Sub

    Private Sub PopulateDDLs()

        '=======================================================================================================
        'Incident Status
        '=======================================================================================================
        ddlIncidentStatus.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectIncidentStatus", objConn)
        objCmd.CommandType = CommandType.StoredProcedure

        'objCmd.Parameters.AddWithValue("@OrderBy", "") Optional Parameter

        DBConStringHelper.PrepareConnection(objConn) 'Open the connection
        ddlIncidentStatus.DataSource = objCmd.ExecuteReader()
        ddlIncidentStatus.DataBind()
        DBConStringHelper.FinalizeConnection(objConn) 'Close the connection

        objCmd = Nothing

        'add an "Select an Option" item to the list
        ddlIncidentStatus.Items.Insert(0, New ListItem("Select Status", "0"))



        '=======================================================================================================
        'Reporting Party Types
        '=======================================================================================================
        '

        ddlReportingPartyType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectReportingPartyType", objConn)
        objCmd.CommandType = CommandType.StoredProcedure

        'objCmd.Parameters.AddWithValue("@OrderBy", "") Optional Parameter

        DBConStringHelper.PrepareConnection(objConn) 'Open the connection
        ddlReportingPartyType.DataSource = objCmd.ExecuteReader()
        ddlReportingPartyType.DataBind()
        DBConStringHelper.FinalizeConnection(objConn) 'Close the connection

        objCmd = Nothing

        'add an "Select an Option" item to the list
        ddlReportingPartyType.Items.Insert(0, New ListItem("Select Reporting Party", "0"))
        ddlReportingPartyType.Items(0).Selected = True


        '=======================================================================================================
        'OnScene Contact Type
        '=======================================================================================================
        '
        ddlOnSceneContactType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectOnSceneContactType", objConn)
        objCmd.CommandType = CommandType.StoredProcedure

        'objCmd.Parameters.AddWithValue("@OrderBy", "") Optional Parameter

        DBConStringHelper.PrepareConnection(objConn) 'Open the connection
        ddlOnSceneContactType.DataSource = objCmd.ExecuteReader()
        ddlOnSceneContactType.DataBind()
        DBConStringHelper.FinalizeConnection(objConn) 'Close the connection

        objCmd = Nothing

        'add an "Select an Option" item to the list
        ddlOnSceneContactType.Items.Insert(0, New ListItem("Select On-Scene Contact", "0"))
        ddlOnSceneContactType.Items(3).Selected = True

        '=======================================================================================================
        'Responsible Party Type
        '=======================================================================================================
        '
        ddlResponsiblePartyType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectResponsiblePartyType", objConn)
        objCmd.CommandType = CommandType.StoredProcedure

        'objCmd.Parameters.AddWithValue("@OrderBy", "") Optional Parameter

        DBConStringHelper.PrepareConnection(objConn) 'Open the connection
        ddlResponsiblePartyType.DataSource = objCmd.ExecuteReader()
        ddlResponsiblePartyType.DataBind()
        DBConStringHelper.FinalizeConnection(objConn) 'Close the connection

        objCmd = Nothing

        'add an "Select an Option" item to the list
        ddlResponsiblePartyType.Items.Insert(0, New ListItem("Select Responsible Party", "0"))
        ddlResponsiblePartyType.Items(3).Selected = True


        If ns.UserLevelID = "1" Then

            '=======================================================================================================
            'IncidentType
            '=======================================================================================================
            ddlIncidentType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objCmd = New SqlCommand("spSelectIncidentType", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            'objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

            'objCmd.Parameters.AddWithValue("@OrderBy", "") Optional Parameter

            DBConStringHelper.PrepareConnection(objConn) 'Open the connection
            ddlIncidentType.DataSource = objCmd.ExecuteReader()
            ddlIncidentType.DataBind()
            DBConStringHelper.FinalizeConnection(objConn) 'Close the connection

            objCmd = Nothing

            'add an "Select an Option" item to the list
            ddlIncidentType.Items.Insert(0, New ListItem("Select An Incident Type", "0"))
            ddlIncidentType.Items(0).Selected = True

        Else

            'Must Find the user Level and grab Worksheets associated to that User except the Admin
            Dim strIncidentTypeID As String = ""

            'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
            '// Add cookie
            'Response.Cookies.Add(oCookie)
            ns = Session("Security_Tracker")

            'oCookie.Item("UserID")

            strIncidentTypeID = MrDataGrabber.GrabIncidentTypeUserByUserID(ns.UserID.ToString())


            'Response.Write(strIncidentTypeID)
            '=======================================================================================================
            'IncidentType
            '=======================================================================================================
            ddlIncidentType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objCmd = New SqlCommand("spSelectIncidentTypeByMultipleIncidentTypeID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentTypeID", strIncidentTypeID)

            'objCmd.Parameters.AddWithValue("@OrderBy", "") Optional Parameter

            DBConStringHelper.PrepareConnection(objConn) 'Open the connection
            ddlIncidentType.DataSource = objCmd.ExecuteReader()
            ddlIncidentType.DataBind()
            DBConStringHelper.FinalizeConnection(objConn) 'Close the connection

            objCmd = Nothing

            'add an "Select an Option" item to the list
            ddlIncidentType.Items.Insert(0, New ListItem("Select An Incident Type", "0"))
            ddlIncidentType.Items(0).Selected = True

        End If













        '=======================================================================================================
        'Severity
        '=======================================================================================================
        ddlSeverity.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectSeverity", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        'objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

        'objCmd.Parameters.AddWithValue("@OrderBy", "") Optional Parameter

        DBConStringHelper.PrepareConnection(objConn) 'Open the connection
        ddlSeverity.DataSource = objCmd.ExecuteReader()
        ddlSeverity.DataBind()
        DBConStringHelper.FinalizeConnection(objConn) 'Close the connection

        objCmd = Nothing

        'add an "Select an Option" item to the list
        ddlSeverity.Items.Insert(0, New ListItem("Select A Severity", "0"))
        ddlSeverity.Items(0).Selected = True

    End Sub

    Protected Sub PopulateReportingParty()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectReportingPartyByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            'localIncidentTypeCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))
            txtReportingPartyFirstName.Text = HelpFunction.Convertdbnulls(objDR("FirstName"))
            txtReportingPartyLastName.Text = HelpFunction.Convertdbnulls(objDR("LastName"))
            txtReportingPartyCallBackNumber1.Text = HelpFunction.Convertdbnulls(objDR("CallBackNumber1"))
            txtReportingPartyCallBackNumber2.Text = HelpFunction.Convertdbnulls(objDR("CallBackNumber2"))
            txtReportingPartyEmail.Text = HelpFunction.Convertdbnulls(objDR("Email"))
            txtReportingPartyAddress.Text = HelpFunction.Convertdbnulls(objDR("Address"))
            txtReportingPartyCity.Text = HelpFunction.Convertdbnulls(objDR("City"))
            txtReportingPartyState.Text = HelpFunction.Convertdbnulls(objDR("State"))
            txtReportingPartyZipcode.Text = HelpFunction.Convertdbnulls(objDR("Zipcode"))
            txtReportingPartyRepresents.Text = HelpFunction.Convertdbnulls(objDR("Represents"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

    End Sub

    Protected Sub PopulateOnSceneContact()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectOnSceneContactByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            'localIncidentTypeCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))
            txtOnSceneContactFirstName.Text = HelpFunction.Convertdbnulls(objDR("FirstName"))
            txtOnSceneContactLastName.Text = HelpFunction.Convertdbnulls(objDR("LastName"))
            txtOnSceneContactPhone1.Text = HelpFunction.Convertdbnulls(objDR("CallBackNumber1"))
            txtOnSceneContactPhone2.Text = HelpFunction.Convertdbnulls(objDR("CallBackNumber2"))
            txtOnSceneContactEmail.Text = HelpFunction.Convertdbnulls(objDR("Email"))
            txtOnSceneContactAddress.Text = HelpFunction.Convertdbnulls(objDR("Address"))
            txtOnSceneContactCity.Text = HelpFunction.Convertdbnulls(objDR("City"))
            txtOnSceneContactState.Text = HelpFunction.Convertdbnulls(objDR("State"))
            txtOnSceneContactZipcode.Text = HelpFunction.Convertdbnulls(objDR("Zipcode"))
            txtOnSceneContactRepresents.Text = HelpFunction.Convertdbnulls(objDR("Represents"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

    End Sub

    Protected Sub PopulateResponsibleParty()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectResponsiblePartyByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            'localIncidentTypeCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))
            txtResponsiblePartyFirstName.Text = HelpFunction.Convertdbnulls(objDR("FirstName"))
            txtResponsiblePartyLastName.Text = HelpFunction.Convertdbnulls(objDR("LastName"))
            txtResponsiblePartyPhone1.Text = HelpFunction.Convertdbnulls(objDR("CallBackNumber1"))
            txtResponsiblePartyPhone2.Text = HelpFunction.Convertdbnulls(objDR("CallBackNumber2"))
            txtResponsiblePartyEmail.Text = HelpFunction.Convertdbnulls(objDR("Email"))
            txtResponsiblePartyAddress.Text = HelpFunction.Convertdbnulls(objDR("Address"))
            txtResponsiblePartyCity.Text = HelpFunction.Convertdbnulls(objDR("City"))
            txtResponsiblePartyState.Text = HelpFunction.Convertdbnulls(objDR("State"))
            txtResponsiblePartyZipcode.Text = HelpFunction.Convertdbnulls(objDR("Zipcode"))
            txtResponsiblePartyRepresents.Text = HelpFunction.Convertdbnulls(objDR("Represents"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

    End Sub


    'Drop Down List Changes
    Protected Sub ddlReportingPartyType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReportingPartyType.SelectedIndexChanged

        'Response.Write(ddlReportingPartyType.SelectedItem.ToString)
        'Response.End()

        If ddlReportingPartyType.SelectedItem.ToString = "As Below" Then

            'Show the Reporting Party
            pnlShowReportingParty.Visible = True

        Else

            'Must Hide
            pnlShowReportingParty.Visible = False

            'If Other related DDL lists show 'Same as Reporting Party' then we must hide them as well
            If ddlOnSceneContactType.SelectedItem.ToString = "Same as Reporting Party" Then
                pnlShowOnSceneContact.Visible = False
            End If

            If ddlResponsiblePartyType.SelectedItem.ToString = "Same as Reporting Party" Then
                pnlShowResponsibleParty.Visible = False
            End If

        End If

    End Sub

    Protected Sub ddlOnSceneContactType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOnSceneContactType.SelectedIndexChanged

        If ddlOnSceneContactType.SelectedItem.ToString = "As Below" Then
            pnlShowOnSceneContact.Visible = True
        Else
            pnlShowOnSceneContact.Visible = False
        End If

    End Sub

    Protected Sub ddlResponsiblePartyType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlResponsiblePartyType.SelectedIndexChanged

        If ddlResponsiblePartyType.SelectedItem.ToString = "As Below" Then
            pnlShowResponsibleParty.Visible = True
        Else
            pnlShowResponsibleParty.Visible = False
        End If

    End Sub

    Protected Sub ddlSeverity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSeverity.SelectedIndexChanged

        ddlSeverity.Style.Add("background-color", MrDataGrabber.GrabOneStringColumnByPrimaryKey("Color", "Severity", "SeverityID", ddlSeverity.SelectedValue))

    End Sub



    'Add InitialIncident
    Protected Sub AddInitialIncident()

        'Since there is no Incident Created yet we must create it

        Dim TempInsertedIncidentID As String

        Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionIncidentIDForIncidentType", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))
            objCmd.Parameters.AddWithValue("@InitalStartTime", Now)


            ParamId = objCmd.Parameters.AddWithValue("@IncidentID_out", System.Data.SqlDbType.Int)
            ParamId.Direction = System.Data.ParameterDirection.Output

            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            TempInsertedIncidentID = objCmd.Parameters("@IncidentID_out").Value

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

        Catch ex As Exception
            Response.Write(ex.ToString)

            Exit Sub
        End Try

        Response.Redirect("EditIncident.aspx?IncidentID=" & TempInsertedIncidentID)

    End Sub



    'Coordinates
    Protected Sub Convert()

        If rdoDecimalDegrees.Checked = True Then
            UsingDecimalDegrees()
        End If

        '=====================================================

        If rdoDegreesMinutes.Checked = True Then
            UsingDegreesMinutes()
        End If

        '=====================================================

        If rdoDegreesMinutesSeconds.Checked = True Then
            UsingDegreesMinutesSeconds()
        End If

        '=====================================================

        If rdoUSNG.Checked = True Then
            UsingUSNG()
        End If

    End Sub

    Protected Sub Convert2()

        UsingDecimalDegrees()

    End Sub

    Protected Sub PopulateCoordTextBoxes()

        Dim localStrLat As String = lblLatDecimalDegrees.Text
        Dim localStrLong As String = lblLongDecimalDegrees.Text
        Dim localStrUSNG As String = lblUSNG.Text

        'Degrees Minutes Start
        Dim localStrLatDegreesMinutes As String = lblLatDegreesMinutes.Text
        Dim localStrLongDegreesMinutes As String = lblLongDegreesMinutes.Text

        Dim localStrLatDegreesMinutesSeconds As String = lblLatDegreesMinutesSeconds.Text
        Dim localStrLongDegreesMinutesSeconds As String = lblLongDegreesMinutesSeconds.Text

        Dim intStartLatDegreesMinutes As Integer
        Dim intStartLongDegreesMinutes As Integer

        'Response.Write(localStrLongDegreesMinutes)
        'Response.Write("<br>")

        'Get first part in localStrlatDegreesMinutes
        intStartLatDegreesMinutes = InStr(localStrlatDegreesMinutes, " ")
        txtLatDegreesMinutes.Text = Mid(localStrLatDegreesMinutes, 1, intStartLatDegreesMinutes - 1)

        'Next Grab the second part
        localStrLatDegreesMinutes = localStrLatDegreesMinutes.Remove(0, intStartLatDegreesMinutes)
        localStrLatDegreesMinutes = Replace(localStrLatDegreesMinutes, " ", "")
        localStrLatDegreesMinutes = Replace(localStrLatDegreesMinutes, ",", "")
        txtLatDegreesMinutes2.Text = localStrLatDegreesMinutes


        ''Next we do the same but with the Long
        intStartLongDegreesMinutes = InStr(localStrLongDegreesMinutes, " ")
        txtLongDegreesMinutes.Text = Mid(localStrLongDegreesMinutes, 1, intStartLongDegreesMinutes - 1)

        'Next Grab the second part
        localStrLongDegreesMinutes = localStrLongDegreesMinutes.Remove(0, intStartLongDegreesMinutes)
        localStrLongDegreesMinutes = Replace(localStrLongDegreesMinutes, " ", "")
        localStrLongDegreesMinutes = Replace(localStrLongDegreesMinutes, ",", "")
        txtLongDegreesMinutes2.Text = localStrLatDegreesMinutes
        'Degrees Minutes End

        localStrLat = Replace(localStrLat, " ", "")
        localStrLat = Replace(localStrLat, ",", "")
        'Degrees Minutes Seconds Start
        'Response.Write(localStrLongDegreesMinutes)
        'Response.Write("<br>")
        'Response.Write(localStrLatDegreesMinutes)
        'Response.Write("<br>")

        Try
            Dim DecDegAbs As Decimal = Math.Abs(CDec(localStrLat))
            Dim ReturnValue As String = "'"
            Dim DegreeSymbol As String = "°"
            Dim MinutesSymbol As String = "’"
            Dim SecondsSymbol As String = """"
            Dim Degrees As String = Math.Truncate(DecDegAbs) & DegreeSymbol
            Dim MinutesDecimal As Decimal = (DecDegAbs - Math.Truncate(DecDegAbs)) * 60
            Dim SecondsDecimal As Decimal = (MinutesDecimal - Math.Truncate(MinutesDecimal))
            Dim Minutes As String = Math.Truncate(MinutesDecimal) & MinutesSymbol
            Dim Seconds As String = String.Format("{0:##.0000}", (SecondsDecimal * 60)) & SecondsSymbol
            ReturnValue = Degrees & " " & Minutes & " " & Seconds

            txtLatDegreesMinutesSeconds.Text = Math.Truncate(DecDegAbs).ToString
            txtLatDegreesMinutesSeconds2.Text = MinutesDecimal
            txtLatDegreesMinutesSeconds3.Text = String.Format("{0:##.0000}", (SecondsDecimal * 60))
            'Response.Write(String.Format("{0:##.0000}", (SecondsDecimal * 60)))
            'Response.Write("<br>")

        Catch ex As Exception

        End Try

        Try
            Dim DecDegAbs As Decimal = Math.Abs(CDec(localStrLong))
            Dim ReturnValue As String = "'"
            Dim DegreeSymbol As String = "°"
            Dim MinutesSymbol As String = "’"
            Dim SecondsSymbol As String = """"
            Dim Degrees As String = Math.Truncate(DecDegAbs) & DegreeSymbol
            Dim MinutesDecimal As Decimal = (DecDegAbs - Math.Truncate(DecDegAbs)) * 60
            Dim SecondsDecimal As Decimal = (MinutesDecimal - Math.Truncate(MinutesDecimal))
            Dim Minutes As String = Math.Truncate(MinutesDecimal) & MinutesSymbol
            Dim Seconds As String = String.Format("{0:##.0000}", (SecondsDecimal * 60)) & SecondsSymbol
            ReturnValue = Degrees & " " & Minutes & " " & Seconds

            txtLongDegreesMinutesSeconds.Text = "-" & Math.Truncate(DecDegAbs).ToString
            txtLongDegreesMinutesSeconds2.Text = MinutesDecimal
            txtLongDegreesMinutesSeconds3.Text = String.Format("{0:##.0000}", (SecondsDecimal * 60))

        Catch ex As Exception

        End Try


        'Response.End()

        localStrLong = Replace(localStrLong, " ", "")
        localStrUSNG = Replace(localStrUSNG, " ", "")

        'lbl()

    End Sub


    Protected Sub UsingDecimalDegrees()

        'Populate Decimal Degrees==========================================
        Dim LatDecimalDegrees As Decimal = txtLatDecimalDegrees.Text

        Dim LongDecimalDegrees As Decimal = txtLongDecimalDegrees.Text

        LatDecimalDegrees = Math.Round(LatDecimalDegrees, 5)

        LongDecimalDegrees = Math.Round(LongDecimalDegrees, 5)

        lblLatDecimalDegrees.Text = LatDecimalDegrees.ToString & " , "

        lblLongDecimalDegrees.Text = LongDecimalDegrees.ToString

        '==================================================================


        'Populate the USNG ================================================
        Dim Precision As Integer = 4
        Dim USNG1 As New USNG

        Dim USNGOutput As String

        USNGOutput = USNG1.LLtoUSNG(CStr(LatDecimalDegrees), CStr(LongDecimalDegrees), Precision)

        lblUSNG.Text = USNGOutput
        '==================================================================


        'Populate DegreesMinutesSeconds ===================================
        Dim LatDegreeMinuteSeconds As String = ReturnDegreesMinutesSecondsFromDecimalDegrees(CDec(txtLatDecimalDegrees.Text))
        Dim LongDegreeMinuteSeconds As String = ReturnDegreesMinutesSecondsFromDecimalDegrees(CDec(txtLongDecimalDegrees.Text))

        lblLatDegreesMinutesSeconds.Text = LatDegreeMinuteSeconds & " , "
        lblLongDegreesMinutesSeconds.Text = "-" & LongDegreeMinuteSeconds
        '==================================================================



        'Populate DegreesMinutes===========================================
        Dim DegreesMinutes() As String

        DegreesMinutes = ReturnDegreesMinutesFromDegreesMinutesSeconds(LatDegreeMinuteSeconds, LongDegreeMinuteSeconds)

        lblLatDegreesMinutes.Text = DegreesMinutes(0) & " , "
        lblLongDegreesMinutes.Text = "-" & DegreesMinutes(1)
        '==================================================================


    End Sub

    Protected Sub UsingUSNG()

        'Populate Decimal Degrees====================================
        Dim USNG1 As New USNG

        Dim latLongOutput() As Decimal

        Dim USNGToreplace As String = txtUSNG.Text

        USNGToreplace = USNGToreplace.Replace(" ", "")

        latLongOutput = USNG1.USNGtoLL(USNGToreplace)

        Dim LatDecimalDegrees As Decimal

        Dim LongDecimalDegrees As Decimal

        LatDecimalDegrees = Math.Round(latLongOutput(0), 5)

        lblLatDecimalDegrees.Text = LatDecimalDegrees.ToString & " , "

        LongDecimalDegrees = Math.Round(latLongOutput(1), 5)

        lblLongDecimalDegrees.Text = LongDecimalDegrees.ToString
        '==================================================================


        'Populate the USNG ================================================
        'Even though we have the USNG already we run it through the converter to get 
        'the adjusted format
        Dim Precision As Integer = 4
        Dim USNG2 As New USNG

        Dim USNGOutput As String

        USNGOutput = USNG2.LLtoUSNG(LatDecimalDegrees, LongDecimalDegrees, Precision)

        lblUSNG.Text = USNGOutput
        '==================================================================


        'Populate DegreesMinutesSeconds ===================================
        Dim LatDegreeMinuteSeconds As String = ReturnDegreesMinutesSecondsFromDecimalDegrees(LatDecimalDegrees)
        Dim LongDegreeMinuteSeconds As String = ReturnDegreesMinutesSecondsFromDecimalDegrees(LongDecimalDegrees)

        lblLatDegreesMinutesSeconds.Text = LatDegreeMinuteSeconds & " , "
        lblLongDegreesMinutesSeconds.Text = "-" & LongDegreeMinuteSeconds
        '==================================================================



        'Populate DegreesMinutes===========================================
        Dim DegreesMinutes() As String

        DegreesMinutes = ReturnDegreesMinutesFromDegreesMinutesSeconds(LatDegreeMinuteSeconds, LongDegreeMinuteSeconds)

        lblLatDegreesMinutes.Text = DegreesMinutes(0) & " , "
        lblLongDegreesMinutes.Text = "-" & DegreesMinutes(1)
        '==================================================================


    End Sub

    Protected Sub UsingDegreesMinutesSeconds()


        'Populate Decimal Degrees====================================
        Dim latDegree As Decimal = Replace(txtLatDegreesMinutesSeconds.Text, "-", "")
        Dim latMinute As Decimal = Replace(txtLatDegreesMinutesSeconds2.Text, "-", "")
        Dim latSecond As Decimal = Replace(txtLatDegreesMinutesSeconds3.Text, "-", "")
        Dim longDegree As Decimal = Replace(txtLongDegreesMinutesSeconds.Text, "-", "")
        Dim longMinute As Decimal = Replace(txtLongDegreesMinutesSeconds2.Text, "-", "")
        Dim longSecond As Decimal = Replace(txtLongDegreesMinutesSeconds3.Text, "-", "")

        Dim LatDegreesMinutesSeconds As Decimal = ReturnDecimalDegreesFromDegreesMinutesSeconds(latDegree, latMinute, latSecond)
        Dim LongDegreesMinutesSeconds As Decimal = ReturnDecimalDegreesFromDegreesMinutesSeconds(longDegree, longMinute, longSecond)
        lblLatDecimalDegrees.Text = LatDegreesMinutesSeconds & " , "
        lblLongDecimalDegrees.Text = "-" & LongDegreesMinutesSeconds
        '==================================================================

        'Populate the USNG ================================================
        Dim Precision As Integer = 4
        Dim USNG1 As New USNG

        Dim USNGOutput As String

        USNGOutput = USNG1.LLtoUSNG(CStr(LatDegreesMinutesSeconds), CStr("-" & LongDegreesMinutesSeconds), Precision)

        lblUSNG.Text = USNGOutput
        '==================================================================


        'Populate DegreesMinutesSeconds ===================================
        Dim LatDegreeMinuteSeconds As String = ReturnDegreesMinutesSecondsFromDecimalDegrees(CDec(LatDegreesMinutesSeconds))
        Dim LongDegreeMinuteSeconds As String = ReturnDegreesMinutesSecondsFromDecimalDegrees(CDec("-" & LongDegreesMinutesSeconds))

        lblLatDegreesMinutesSeconds.Text = LatDegreeMinuteSeconds & " , "
        lblLongDegreesMinutesSeconds.Text = "-" & LongDegreeMinuteSeconds
        '==================================================================



        'Populate DegreesMinutes===========================================
        Dim DegreesMinutes() As String

        DegreesMinutes = ReturnDegreesMinutesFromDegreesMinutesSeconds(LatDegreeMinuteSeconds, LongDegreeMinuteSeconds)

        lblLatDegreesMinutes.Text = DegreesMinutes(0) & " , "
        lblLongDegreesMinutes.Text = "-" & DegreesMinutes(1)
        '==================================================================



    End Sub

    Protected Sub UsingDegreesMinutes()

        'Populate Decimal Degrees==========================================
        Dim LatDecimalDegrees As Decimal = txtLatDegreesMinutes.Text & Replace(Math.Round(txtLatDegreesMinutes2.Text / 60, 5), "0.", ".")

        Dim LongDecimalDegrees As Decimal = txtLongDegreesMinutes.Text & Replace(Math.Round(txtLongDegreesMinutes2.Text / 60, 5), "0.", ".")

        'LatDecimalDegrees = Math.Round(LatDecimalDegrees, 5)

        'LongDecimalDegrees = Math.Round(LongDecimalDegrees, 5)

        lblLatDecimalDegrees.Text = LatDecimalDegrees.ToString & " , "

        lblLongDecimalDegrees.Text = LongDecimalDegrees.ToString

        '==================================================================


        'Populate the USNG ================================================
        Dim Precision As Integer = 4
        Dim USNG1 As New USNG

        Dim USNGOutput As String

        USNGOutput = USNG1.LLtoUSNG(CStr(LatDecimalDegrees), CStr(LongDecimalDegrees), Precision)

        lblUSNG.Text = USNGOutput
        '==================================================================


        'Populate DegreesMinutesSeconds ===================================
        Dim LatDegreeMinuteSeconds As String = ReturnDegreesMinutesSecondsFromDecimalDegrees(LatDecimalDegrees)
        Dim LongDegreeMinuteSeconds As String = ReturnDegreesMinutesSecondsFromDecimalDegrees(LongDecimalDegrees)

        lblLatDegreesMinutesSeconds.Text = LatDegreeMinuteSeconds & " , "
        lblLongDegreesMinutesSeconds.Text = "-" & LongDegreeMinuteSeconds
        '==================================================================



        'Populate DegreesMinutes===========================================
        Dim DegreesMinutes() As String

        DegreesMinutes = ReturnDegreesMinutesFromDegreesMinutesSeconds(LatDegreeMinuteSeconds, LongDegreeMinuteSeconds)

        lblLatDegreesMinutes.Text = DegreesMinutes(0) & " , "
        lblLongDegreesMinutes.Text = "-" & DegreesMinutes(1)
        '==================================================================


    End Sub

    Protected Sub UsingZipAddress()

        Dim URL As String = ""

        Dim urlStart As String = "http://tasks.arcgisonline.com/ArcGIS/rest/services/Locators/TA_Address_NA/GeocodeServer/findAddressCandidates?Address="

        Dim urlMid As String = "&City=&State=&Zip="

        Dim urlEnd As String = "&Zip4=&Country=&outFields=&f=html"

        'Dim DataType As String = strDataType

        ' Get data stream from remote server
        Dim data As Stream
        Dim client As WebClient = New WebClient()
        Dim strAddress As String = ""

        Dim sString As String = txtAddress2.Text

        Dim sWords() As String = sString.Split(" ")
        Dim Zip As String = txtZip.Text.Trim

        Dim i As Integer

        For i = 0 To sWords.Length - 1
            'Response.Write(sWords(i) + "<br  />")
            strAddress = strAddress & sWords(i) & "+"
        Next
        strAddress = strAddress.Remove(strAddress.Length - 1, 1)

        URL = urlStart + strAddress + urlMid + Zip + urlEnd

        'Response.Write(URL)
        'Response.End()



        data = client.OpenRead(URL)

        Dim reader As StreamReader = New StreamReader(data)

        Dim webPageTable As String = reader.ReadToEnd
        Dim xString As String = webPageTable
        Dim yString As String = webPageTable

        Dim CharCount As Integer = webPageTable.Length
        Dim xStart As Integer = InStr(webPageTable, "X:")
        Dim YStart As Integer = InStr(webPageTable, "Y:")



        txtLatDecimalDegrees.Text = Mid(yString, YStart + 2, 9)
        txtLongDecimalDegrees.Text = Mid(xString, xStart + 3, 9)

        Try
            Dim ErrorChecker As Decimal = CDec(txtLatDecimalDegrees.Text)
            Dim ErrorChecker2 As Decimal = CDec(txtLatDecimalDegrees.Text)
        Catch ex As Exception

            txtLatDecimalDegrees.Text = 0.0
            txtLongDecimalDegrees.Text = 0.0

        End Try

        'Response.Write(lblLatDecimalDegrees.Text)
        'Response.Write("<br>")
        'Response.Write(lblLongDecimalDegrees.Text)
        'Response.End()

        'lblResults.Text = Mid(yString, YStart + 2, 9) & " , " & Mid(xString, xStart + 3, 9)
        'Response.End()

    End Sub

    Protected Sub UsingAddressCity()

        Dim URL As String = ""

        Dim urlStart As String = "http://tasks.arcgisonline.com/ArcGIS/rest/services/Locators/TA_Address_NA/GeocodeServer/findAddressCandidates?Address="

        Dim urlMid As String = "&City="

        Dim urlMid2 As String = "&State="

        Dim urlEnd As String = "&Zip=&Zip4=&Country=&outFields=&f=html"

        'Dim DataType As String = strDataType

        ' Get data stream from remote server
        Dim data As Stream
        Dim client As WebClient = New WebClient()
        Dim strAddress As String = ""
        Dim strCity As String = ""
        Dim strState As String = "Florida"

        Dim sString As String = txtAddress.Text

        Dim sWords() As String = sString.Split(" ")

        Dim sString2 As String = txtCity.Text

        Dim sWords2() As String = sString2.Split(" ")

        Dim i As Integer

        'Response.Write(sString2)
        'Response.End()

        Dim a As Integer = 0
        For i = 0 To sWords.Length - 1
            'Response.Write(sWords(i) + "<br  />")
            strAddress = strAddress & sWords(i) & "+"
        Next
        strAddress = strAddress.Remove(strAddress.Length - 1, 1)


        For i = 0 To sWords2.Length - 1
            'Response.Write(sWords(i) + "<br  />")
            strCity = strCity & sWords2(i) & "+"
            a = a + 1
        Next

        strCity = strCity.Remove(strCity.Length - 1, 1)


        URL = urlStart + strAddress + urlMid + strCity + urlMid2 + strState + urlEnd

        'Response.Write(URL)
        'Response.End()



        data = client.OpenRead(URL)

        Dim reader As StreamReader = New StreamReader(data)

        Dim webPageTable As String = reader.ReadToEnd
        Dim xString As String = webPageTable
        Dim yString As String = webPageTable

        Dim CharCount As Integer = webPageTable.Length
        Dim xStart As Integer = InStr(webPageTable, "X:")
        Dim YStart As Integer = InStr(webPageTable, "Y:")

        'Response.Write(Mid(xString, xStart + 3, 9))
        'Response.Write("<br>")
        'Response.Write(Mid(yString, YStart + 2, 9))

        txtLatDecimalDegrees.Text = Mid(yString, YStart + 2, 9)
        txtLongDecimalDegrees.Text = Mid(xString, xStart + 3, 9)
        'lblResults.Text = Mid(yString, YStart + 2, 9) & " , " & Mid(xString, xStart + 3, 9)
        'Response.End()

        Try
            Dim ErrorChecker As Decimal = CDec(txtLatDecimalDegrees.Text)
            Dim ErrorChecker2 As Decimal = CDec(txtLatDecimalDegrees.Text)
        Catch ex As Exception

            txtLatDecimalDegrees.Text = 0.0
            txtLongDecimalDegrees.Text = 0.0

        End Try
        'Response.Write("txtLatDecimalDegrees.Text: " & txtLatDecimalDegrees.Text)
        'Response.Write("<br>")
        'Response.Write("txtLatDecimalDegrees.Text: " & txtLongDecimalDegrees.Text)
        'Response.End()

    End Sub

    Protected Sub UsingStreetsCity()

        Dim URL As String = ""

        Dim urlStart As String = "http://tasks.arcgisonline.com/ArcGIS/rest/services/Locators/TA_Streets_US/GeocodeServer/findAddressCandidates?Street="

        Dim urlMid As String = "&City="

        Dim urlMid2 As String = "&State=Fl"

        Dim urlEnd As String = "&ZIP=&outFields=&f=html"

        'Dim DataType As String = strDataType

        ' Get data stream from remote server
        Dim data As Stream
        Dim client As WebClient = New WebClient()

        Dim strStreet1 As String = ""
        Dim strStreet2 As String = ""
        Dim strIntersection As String
        Dim strCity As String = ""


        Dim sString As String = txtStreet.Text
        Dim sString2 As String = txtStreet2.Text
        Dim sString3 As String = txtCity2.Text


        Dim sWords() As String = sString.Split(" ")
        Dim sWords2() As String = sString2.Split(" ")
        Dim sWords3() As String = sString3.Split(" ")


        Dim i As Integer

        'Response.Write(sString2)
        'Response.End()

        Dim a As Integer = 0
        For i = 0 To sWords.Length - 1
            'Response.Write(sWords(i) + "<br  />")
            strStreet1 = strStreet1 & sWords(i) & "+"
        Next

        strStreet1 = strStreet1.Remove(strStreet1.Length - 1, 1)


        For i = 0 To sWords2.Length - 1
            'Response.Write(sWords(i) + "<br  />")
            strStreet2 = strStreet2 & sWords2(i) & "+"
        Next

        strStreet2 = strStreet2.Remove(strStreet2.Length - 1, 1)

        For i = 0 To sWords3.Length - 1
            'Response.Write(sWords(i) + "<br  />")
            strCity = strCity & sWords3(i) & "+"
        Next

        strCity = strCity.Remove(strCity.Length - 1, 1)

        strIntersection = strStreet1 & "+and+" & strStreet2


        URL = urlStart + strIntersection + urlMid + strCity + urlMid2 + urlEnd


        'Response.Write(URL)
        'Response.End()



        data = client.OpenRead(URL)

        Dim reader As StreamReader = New StreamReader(data)

        Dim webPageTable As String = reader.ReadToEnd
        Dim xString As String = webPageTable
        Dim yString As String = webPageTable

        Dim CharCount As Integer = webPageTable.Length
        Dim xStart As Integer = InStr(webPageTable, "X:")
        Dim YStart As Integer = InStr(webPageTable, "Y:")

        'Response.Write(Mid(xString, xStart + 3, 9))
        'Response.Write("<br>")
        'Response.Write(Mid(yString, YStart + 2, 9))

        txtLatDecimalDegrees.Text = Mid(yString, YStart + 2, 9)
        txtLongDecimalDegrees.Text = Mid(xString, xStart + 3, 9)
        'lblResults.Text = Mid(yString, YStart + 2, 9) & " , " & Mid(xString, xStart + 3, 9)
        'Response.End()

        Try
            Dim ErrorChecker As Decimal = CDec(txtLatDecimalDegrees.Text)
            Dim ErrorChecker2 As Decimal = CDec(txtLatDecimalDegrees.Text)
        Catch ex As Exception

            txtLatDecimalDegrees.Text = 0.0
            txtLongDecimalDegrees.Text = 0.0

        End Try

    End Sub



    'Functions
    Function ReturnDegreesMinutesSecondsFromDecimalDegrees(ByVal DecimalDegrees As Decimal) As String

        Dim DecDegAbs As Decimal = Math.Abs(DecimalDegrees)
        Dim ReturnValue As String = "'"
        Dim DegreeSymbol As String = "°"
        Dim MinutesSymbol As String = "’"
        Dim SecondsSymbol As String = """"
        Dim Degrees As String = Math.Truncate(DecDegAbs) & DegreeSymbol
        Dim MinutesDecimal As Decimal = (DecDegAbs - Math.Truncate(DecDegAbs)) * 60
        Dim SecondsDecimal As Decimal = (MinutesDecimal - Math.Truncate(MinutesDecimal))
        Dim Minutes As String = Math.Truncate(MinutesDecimal) & MinutesSymbol
        Dim Seconds As String = String.Format("{0:##.0000}", (SecondsDecimal * 60)) & SecondsSymbol
        ReturnValue = Degrees & " " & Minutes & " " & Seconds
        Return ReturnValue

    End Function

    Function ReturnDegreesMinutesFromDegreesMinutesSeconds(ByVal localDegreeMinuteLat As String, ByVal localDegreeMinuteLong As String) As Array

        Dim ReturnValue(2) As String

        'Grabbing the Middle Part of(DD MM SS.ss) of Lat
        Dim DecMinStartPointLatBegin As Integer = 1
        Dim DecMinEndPointLatBegin As Integer = InStr(localDegreeMinuteLat, "°")

        'Grabbing the End Part of(DD MM SS.ss) of Lat
        Dim DecMinStartPointLongBegin As Integer = 1
        Dim DecMinEndPointLongBegin As Integer = InStr(localDegreeMinuteLong, "°")

        '===================================================================================

        'Grabbing the Middle Part of(DD MM SS.ss) of Lat
        Dim DecMinStartPointLatMiddle As Integer = InStr(localDegreeMinuteLat, "°")
        Dim DecMinEndPointLatMiddle As Integer = InStr(localDegreeMinuteLat, "’")

        'Grabbing the End Part of(DD MM SS.ss) of Lat
        Dim DecMinStartPointLongMiddle As Integer = InStr(localDegreeMinuteLong, "°")
        Dim DecMinEndPointLongMiddle As Integer = InStr(localDegreeMinuteLong, "’")


        '===================================================================================

        'Grabbing the Middle Part of(DD MM SS.ss) of Long
        Dim DecMinStartPointLatEnd As Integer = InStr(localDegreeMinuteLat, "’")
        Dim DecMinEndPointLatEnd As Integer = InStr(localDegreeMinuteLat, """")

        'Grabbing the End Part of(DD MM SS.ss) of Long
        Dim DecMinStartPointLongEnd As Integer = InStr(localDegreeMinuteLong, "’")
        Dim DecMinEndPointLongEnd As Integer = InStr(localDegreeMinuteLong, """")




        Dim LatStart As Decimal
        Dim LongStart As Decimal

        Dim LatMiddle As Decimal
        Dim LongMiddle As Decimal

        Dim LatEnd As Decimal
        Dim LongEnd As Decimal


        'Response.Write(localDegreeMinuteLat)
        'Response.Write("<br>")

        'Response.Write(InStr(localDegreeMinuteLat, "°"))
        'Response.Write("<br>")



        LatStart = CDec(Mid(localDegreeMinuteLat, DecMinStartPointLatBegin, DecMinEndPointLatBegin - 1))
        LongStart = (CDec(Mid(localDegreeMinuteLong, DecMinStartPointLongBegin, DecMinEndPointLongBegin - 1)))

        LatMiddle = CDec(Mid(localDegreeMinuteLat, DecMinStartPointLatMiddle + 1, DecMinEndPointLatMiddle - (DecMinStartPointLatMiddle + 1)))
        LongMiddle = CDec(Mid(localDegreeMinuteLong, DecMinStartPointLongMiddle + 1, DecMinEndPointLongMiddle - (DecMinStartPointLongMiddle + 1)))

        LatEnd = CDec(Mid(localDegreeMinuteLat, DecMinStartPointLatEnd + 1, DecMinEndPointLatEnd - (DecMinStartPointLatEnd + 1)))
        LongEnd = CDec(Mid(localDegreeMinuteLong, DecMinStartPointLongEnd + 1, DecMinEndPointLongEnd - (DecMinStartPointLongEnd + 1)))


        'Divide end number by 60 
        LatEnd = LatEnd / 60

        'Add the middle back to thend
        LatMiddle = LatMiddle + Decimal.Round(LatEnd, 4)

        'Divide end number by 60 
        LongEnd = LongEnd / 60
        'Add the middle back to thend
        LongMiddle = LongMiddle + Decimal.Round(LongEnd, 4)


        Dim DecimalMinutesLatitude As String
        Dim DecimalMinutesLongitude As String

        'Now Tie the start number back with the end
        DecimalMinutesLatitude = LatStart.ToString & "  " & LatMiddle.ToString
        DecimalMinutesLongitude = LongStart.ToString & "  " & LongMiddle.ToString

        ReturnValue(0) = DecimalMinutesLatitude

        ReturnValue(1) = DecimalMinutesLongitude

        Return ReturnValue

    End Function

    Function ReturnDecimalDegreesFromDegreesMinutesSeconds(ByVal Degree As Decimal, ByVal Minute As Decimal, ByVal Second As Decimal) As String

        Dim ReturnValue As Decimal
        ReturnValue = Degree + ((Minute + (Second / 60)) / 60)
        ReturnValue = Math.Round(ReturnValue, 5)
        Return ReturnValue

    End Function


    'Radio Buttons
    Protected Sub rdoDecimalDegrees_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoDecimalDegrees.CheckedChanged
        ShowHidePanels()
    End Sub

    Protected Sub rdoDegreesMinutes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoDegreesMinutes.CheckedChanged
        ShowHidePanels()
    End Sub

    Protected Sub rdoDegreesMinutesSeconds_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoDegreesMinutesSeconds.CheckedChanged
        ShowHidePanels()
    End Sub

    Protected Sub rdoUSNG_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoUSNG.CheckedChanged
        ShowHidePanels()
    End Sub

    Protected Sub rdoByCoordinateEntry_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoByCoordinateEntry.CheckedChanged
        pnlShowCoordinates.Visible = True
        rdoDecimalDegrees.Checked = True
    End Sub

    Protected Sub rdoAddressCity_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoAddressCity.CheckedChanged
        pnlShowCoordinates.Visible = False
    End Sub

    Protected Sub rdoByAddressZip_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoByAddressZip.CheckedChanged
        pnlShowCoordinates.Visible = False
    End Sub

    Protected Sub rdoByIntersection_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoByIntersection.CheckedChanged
        pnlShowCoordinates.Visible = False
    End Sub


    Protected Sub Disolve()

        'txtLatDecimalDegrees.Text = ""
        'txtLongDecimalDegrees.Text = ""
        'txtLatDegreesMinutes.Text = ""
        'txtLongDegreesMinutes.Text = ""
        'txtLatDegreesMinutes2.Text = ""
        'txtLongDegreesMinutes2.Text = ""
        'txtLatDegreesMinutesSeconds.Text = ""
        'txtLongDegreesMinutesSeconds.Text = ""
        'txtLatDegreesMinutesSeconds2.Text = ""
        'txtLongDegreesMinutesSeconds2.Text = ""
        'txtLatDegreesMinutesSeconds3.Text = ""
        'txtLongDegreesMinutesSeconds3.Text = ""
        'txtUSNG.Text = ""

        'lblLatDecimalDegrees.Text = ""
        'lblLongDecimalDegrees.Text = ""
        'lblLatDegreesMinutes.Text = ""
        'lblLongDegreesMinutes.Text = ""
        'lblLatDegreesMinutesSeconds.Text = ""
        'lblLongDegreesMinutesSeconds.Text = ""
        'lblUSNG.Text = ""

        ShowHidePanels()

    End Sub

    Protected Sub ShowHidePanels()

        'pnlShowResults.Visible = False

        '=====================================================

        If rdoDecimalDegrees.Checked = True Then
            pnlShowDecimalDegrees.Visible = True
            pnlShowDegreesMinutes.Visible = False
            pnlShowDegreesMinutesSeconds.Visible = False
            pnlShowUSNG.Visible = False
        End If

        '=====================================================

        If rdoDegreesMinutes.Checked = True Then
            pnlShowDegreesMinutes.Visible = True
            pnlShowDecimalDegrees.Visible = False
            pnlShowDegreesMinutesSeconds.Visible = False
            pnlShowUSNG.Visible = False
        End If

        '=====================================================

        If rdoDegreesMinutesSeconds.Checked = True Then
            pnlShowDegreesMinutesSeconds.Visible = True
            pnlShowDegreesMinutes.Visible = False
            pnlShowDecimalDegrees.Visible = False
            pnlShowUSNG.Visible = False
        End If

        '=====================================================

        If rdoUSNG.Checked = True Then
            pnlShowUSNG.Visible = True
            pnlShowDegreesMinutesSeconds.Visible = False
            pnlShowDegreesMinutes.Visible = False
            pnlShowDecimalDegrees.Visible = False
        End If


    End Sub


    'Grids Subs
    Protected Sub getIncidentIncidentType()

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

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


        objCmd = New SqlCommand("spSelectIncidentIncidentTypeByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

        objDS.Tables.Clear()
        'bind our data
        objDA = New SqlDataAdapter(objCmd)
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing

        DBConStringHelper.FinalizeConnection(objConn) 'close the connection

        'call the calculate grid counts to show the number of records, the page you are on, etc.
        objDataGridFunctions.CalcDataGridCounts(objDS, IncidentIncidentTypeDataGrid, "")
        'Associate the data grid with the data
        IncidentIncidentTypeDataGrid.DataSource = objDS.Tables(0).DefaultView
        IncidentIncidentTypeDataGrid.DataBind()

        objDataGridFunctions.Highlightrows(IncidentIncidentTypeDataGrid, "", "", "")

        'Checking to see if we have any Contact Methods
        'Response.Write(objDS.Tables(0).Rows.Count)

        If CInt(objDS.Tables(0).Rows.Count) <> 0 Then
            'We have records show the Grid
            pnlShowIncidentTypes.Visible = True
        Else
            'Hide Grid
            'pnlShowIncidentTypes.Visible = False
        End If

    End Sub

    Sub IncidentIncidentTypeDataGrid_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs)
        '---------------------------------------------------------------------------------------------------------
        '   This sub is called on the next and previous clicks for the recordset, it cycles to the next 20 records
        '---------------------------------------------------------------------------------------------------------
        IncidentIncidentTypeDataGrid.CurrentPageIndex = e.NewPageIndex
        IncidentIncidentTypeDataGrid.DataBind()

        Dim x As Integer
        Dim TempSortHolder As String
        Dim FindImg As Integer
        Dim FindAsc As Integer
        Dim CurrentSearchMode As String = ""
        Dim NewSearchMode As String = ""
        Dim NewHeaderImg As String = ""
        Dim strSort As String = ""

        For x = 0 To IncidentIncidentTypeDataGrid.Columns.Count - 1
            FindImg = InStr(IncidentIncidentTypeDataGrid.Columns(x).HeaderText, "<img") 'find the column with the <img tag
            If FindImg <> 0 Then
                TempSortHolder = IncidentIncidentTypeDataGrid.Columns(x).SortExpression
                FindAsc = InStr(TempSortHolder, "ASC")
                If FindAsc <> 0 Then 'sort desc
                    strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "ASC") - 1) & " ASC"
                Else 'sort asc
                    strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "DESC") - 1) & " DESC"
                End If
                Exit For
            End If
        Next


        getIncidentIncidentType()


    End Sub

    Sub AttachmentDataGrid_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs)
        '---------------------------------------------------------------------------------------------------------
        '   This sub is called on the next and previous clicks for the recordset, it cycles to the next 20 records
        '---------------------------------------------------------------------------------------------------------
        AttachmentDataGrid.CurrentPageIndex = e.NewPageIndex
        AttachmentDataGrid.DataBind()

        Dim x As Integer
        Dim TempSortHolder As String
        Dim FindImg As Integer
        Dim FindAsc As Integer
        Dim CurrentSearchMode As String = ""
        Dim NewSearchMode As String = ""
        Dim NewHeaderImg As String = ""
        Dim strSort As String = ""

        For x = 0 To AttachmentDataGrid.Columns.Count - 1
            FindImg = InStr(AttachmentDataGrid.Columns(x).HeaderText, "<img") 'find the column with the <img tag
            If FindImg <> 0 Then
                TempSortHolder = AttachmentDataGrid.Columns(x).SortExpression
                FindAsc = InStr(TempSortHolder, "ASC")
                If FindAsc <> 0 Then 'sort desc
                    strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "ASC") - 1) & " ASC"
                Else 'sort asc
                    strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "DESC") - 1) & " DESC"
                End If
                Exit For
            End If
        Next


        getAttachment()


    End Sub

    Sub LinkDataGrid_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs)
        '---------------------------------------------------------------------------------------------------------
        '   This sub is called on the next and previous clicks for the recordset, it cycles to the next 20 records
        '---------------------------------------------------------------------------------------------------------
        LinkDataGrid.CurrentPageIndex = e.NewPageIndex
        LinkDataGrid.DataBind()

        Dim x As Integer
        Dim TempSortHolder As String
        Dim FindImg As Integer
        Dim FindAsc As Integer
        Dim CurrentSearchMode As String = ""
        Dim NewSearchMode As String = ""
        Dim NewHeaderImg As String = ""
        Dim strSort As String = ""

        For x = 0 To LinkDataGrid.Columns.Count - 1
            FindImg = InStr(LinkDataGrid.Columns(x).HeaderText, "<img") 'find the column with the <img tag
            If FindImg <> 0 Then
                TempSortHolder = LinkDataGrid.Columns(x).SortExpression
                FindAsc = InStr(TempSortHolder, "ASC")
                If FindAsc <> 0 Then 'sort desc
                    strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "ASC") - 1) & " ASC"
                Else 'sort asc
                    strSort = Mid(TempSortHolder, 1, InStr(TempSortHolder, "DESC") - 1) & " DESC"
                End If
                Exit For
            End If
        Next


        getLink()


    End Sub

    Protected Sub getAttachment()


        'connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        DBConStringHelper.PrepareConnection(objConn) 'open the connection

        objCmd = New SqlCommand("spSelectAttachmentByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

        objDS.Tables.Clear()
        'bind our data
        objDA = New SqlDataAdapter(objCmd)
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing

        DBConStringHelper.FinalizeConnection(objConn) 'close the connection

        'call the calculate grid counts to show the number of records, the page you are on, etc.
        objDataGridFunctions.CalcDataGridCounts(objDS, AttachmentDataGrid, "")
        'Associate the data grid with the data
        AttachmentDataGrid.DataSource = objDS.Tables(0).DefaultView
        AttachmentDataGrid.DataBind()

        objDataGridFunctions.Highlightrows(AttachmentDataGrid, "", "", "")

        If CInt(objDS.Tables(0).Rows.Count) <> 0 Then
            'We have records show the Grid
            pnlShowAttachment.Visible = True
        Else
            'Hide Grid
            pnlShowAttachment.Visible = False
        End If


    End Sub

    Protected Sub getAffectedCounty()

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        Dim localRecordCount As Integer

        Try

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectCountyRegionCheckCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                localRecordCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

        Catch ex As Exception

            Response.Write(ex.ToString)
            Exit Sub

        End Try

        PopulateCounties()

        Dim localAllCounties As String = ""
        Dim localStateWide As Boolean
        Dim localRegion1 As Boolean
        Dim localRegion2 As Boolean
        Dim localRegion3 As Boolean
        Dim localRegion4 As Boolean
        Dim localRegion5 As Boolean
        Dim localRegion6 As Boolean
        Dim localRegion7 As Boolean
        Dim localBay As Boolean
        Dim localCalhoun As Boolean
        Dim localEscambia As Boolean
        Dim localGulf As Boolean
        Dim localHolmes As Boolean
        Dim localJackson As Boolean
        Dim localOkaloosa As Boolean
        Dim localSantaRosa As Boolean
        Dim localWalton As Boolean
        Dim localWashington As Boolean
        Dim localColumbia As Boolean
        Dim localDixie As Boolean
        Dim localFranklin As Boolean
        Dim localGadsden As Boolean
        Dim localHamilton As Boolean
        Dim localJefferson As Boolean
        Dim localLafayette As Boolean
        Dim localLeon As Boolean
        Dim localLevy As Boolean
        Dim localLiberty As Boolean
        Dim localMadison As Boolean
        Dim localSuwannee As Boolean
        Dim localTaylor As Boolean
        Dim localWakulla As Boolean
        Dim localAlachua As Boolean
        Dim localBaker As Boolean
        Dim localBradford As Boolean
        Dim localClay As Boolean
        Dim localDuval As Boolean
        Dim localFlagler As Boolean
        Dim localGilchrist As Boolean
        Dim localMarion As Boolean
        Dim localNassau As Boolean
        Dim localPutnam As Boolean
        Dim localStJohns As Boolean
        Dim localUnion As Boolean
        Dim localCitrus As Boolean
        Dim localHardee As Boolean
        Dim localHernando As Boolean
        Dim localHillsborough As Boolean
        Dim localPasco As Boolean
        Dim localPinellas As Boolean
        Dim localPolk As Boolean
        Dim localSumter As Boolean
        Dim localBrevard As Boolean
        Dim localIndianRiver As Boolean
        Dim localLake As Boolean
        Dim localMartin As Boolean
        Dim localOrange As Boolean
        Dim localOsceola As Boolean
        Dim localSeminole As Boolean
        Dim localStLucie As Boolean
        Dim localVolusia As Boolean
        Dim localCharlotte As Boolean
        Dim localCollier As Boolean
        Dim localDeSoto As Boolean
        Dim localGlades As Boolean
        Dim localHendry As Boolean
        Dim localHighlands As Boolean
        Dim localLee As Boolean
        Dim localManatee As Boolean
        Dim localOkeechobee As Boolean
        Dim localSarasota As Boolean
        Dim localBroward As Boolean
        Dim localMiamiDade As Boolean
        Dim localMonroe As Boolean
        Dim localPalmBeach As Boolean

        Try

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectCountyRegionCheckByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                localStateWide = HelpFunction.ConvertdbnullsBool(objDR("Statewide"))
                localRegion1 = HelpFunction.ConvertdbnullsBool(objDR("Region1"))
                localRegion2 = HelpFunction.ConvertdbnullsBool(objDR("Region2"))
                localRegion3 = HelpFunction.ConvertdbnullsBool(objDR("Region3"))
                localRegion4 = HelpFunction.ConvertdbnullsBool(objDR("Region4"))
                localRegion5 = HelpFunction.ConvertdbnullsBool(objDR("Region5"))
                localRegion6 = HelpFunction.ConvertdbnullsBool(objDR("Region6"))
                localRegion7 = HelpFunction.ConvertdbnullsBool(objDR("Region7"))
                localBay = HelpFunction.ConvertdbnullsBool(objDR("Bay"))
                localCalhoun = HelpFunction.ConvertdbnullsBool(objDR("Calhoun"))
                localEscambia = HelpFunction.ConvertdbnullsBool(objDR("Escambia"))
                localGulf = HelpFunction.ConvertdbnullsBool(objDR("Gulf"))
                localHolmes = HelpFunction.ConvertdbnullsBool(objDR("Holmes"))
                localJackson = HelpFunction.ConvertdbnullsBool(objDR("Jackson"))
                localOkaloosa = HelpFunction.ConvertdbnullsBool(objDR("Okaloosa"))
                localSantaRosa = HelpFunction.ConvertdbnullsBool(objDR("Santa Rosa"))
                localWalton = HelpFunction.ConvertdbnullsBool(objDR("Walton"))
                localWashington = HelpFunction.ConvertdbnullsBool(objDR("Washington"))
                localColumbia = HelpFunction.ConvertdbnullsBool(objDR("Columbia"))
                localDixie = HelpFunction.ConvertdbnullsBool(objDR("Dixie"))
                localFranklin = HelpFunction.ConvertdbnullsBool(objDR("Franklin"))
                localGadsden = HelpFunction.ConvertdbnullsBool(objDR("Gadsden"))
                localHamilton = HelpFunction.ConvertdbnullsBool(objDR("Hamilton"))
                localJefferson = HelpFunction.ConvertdbnullsBool(objDR("Jefferson"))
                localLafayette = HelpFunction.ConvertdbnullsBool(objDR("Lafayette"))
                localLeon = HelpFunction.ConvertdbnullsBool(objDR("Leon"))
                localLevy = HelpFunction.ConvertdbnullsBool(objDR("Levy"))
                localLiberty = HelpFunction.ConvertdbnullsBool(objDR("Liberty"))
                localMadison = HelpFunction.ConvertdbnullsBool(objDR("Madison"))
                localSuwannee = HelpFunction.ConvertdbnullsBool(objDR("Suwannee"))
                localTaylor = HelpFunction.ConvertdbnullsBool(objDR("Taylor"))
                localWakulla = HelpFunction.ConvertdbnullsBool(objDR("Wakulla"))
                localAlachua = HelpFunction.ConvertdbnullsBool(objDR("Alachua"))
                localBaker = HelpFunction.ConvertdbnullsBool(objDR("Baker"))
                localBradford = HelpFunction.ConvertdbnullsBool(objDR("Bradford"))
                localClay = HelpFunction.ConvertdbnullsBool(objDR("Clay"))
                localDuval = HelpFunction.ConvertdbnullsBool(objDR("Duval"))
                localFlagler = HelpFunction.ConvertdbnullsBool(objDR("Flagler"))
                localGilchrist = HelpFunction.ConvertdbnullsBool(objDR("Gilchrist"))
                localMarion = HelpFunction.ConvertdbnullsBool(objDR("Marion"))
                localNassau = HelpFunction.ConvertdbnullsBool(objDR("Nassau"))
                localPutnam = HelpFunction.ConvertdbnullsBool(objDR("Putnam"))
                localStJohns = HelpFunction.ConvertdbnullsBool(objDR("St. Johns"))
                localUnion = HelpFunction.ConvertdbnullsBool(objDR("Union"))
                localCitrus = HelpFunction.ConvertdbnullsBool(objDR("Citrus"))
                localHardee = HelpFunction.ConvertdbnullsBool(objDR("Hardee"))
                localHernando = HelpFunction.ConvertdbnullsBool(objDR("Hernando"))
                localHillsborough = HelpFunction.ConvertdbnullsBool(objDR("Hillsborough"))
                localPasco = HelpFunction.ConvertdbnullsBool(objDR("Pasco"))
                localPinellas = HelpFunction.ConvertdbnullsBool(objDR("Pinellas"))
                localPolk = HelpFunction.ConvertdbnullsBool(objDR("Polk"))
                localSumter = HelpFunction.ConvertdbnullsBool(objDR("Sumter"))
                localBrevard = HelpFunction.ConvertdbnullsBool(objDR("Brevard"))
                localIndianRiver = HelpFunction.ConvertdbnullsBool(objDR("Indian River"))
                localLake = HelpFunction.ConvertdbnullsBool(objDR("Lake"))
                localMartin = HelpFunction.ConvertdbnullsBool(objDR("Martin"))
                localOrange = HelpFunction.ConvertdbnullsBool(objDR("Orange"))
                localOsceola = HelpFunction.ConvertdbnullsBool(objDR("Osceola"))
                localSeminole = HelpFunction.ConvertdbnullsBool(objDR("Seminole"))
                localStLucie = HelpFunction.ConvertdbnullsBool(objDR("St. Lucie"))
                localVolusia = HelpFunction.ConvertdbnullsBool(objDR("Volusia"))
                localCharlotte = HelpFunction.ConvertdbnullsBool(objDR("Charlotte"))
                localCollier = HelpFunction.ConvertdbnullsBool(objDR("Collier"))
                localDeSoto = HelpFunction.ConvertdbnullsBool(objDR("DeSoto"))
                localGlades = HelpFunction.ConvertdbnullsBool(objDR("Glades"))
                localHendry = HelpFunction.ConvertdbnullsBool(objDR("Hendry"))
                localHighlands = HelpFunction.ConvertdbnullsBool(objDR("Highlands"))
                localLee = HelpFunction.ConvertdbnullsBool(objDR("Lee"))
                localManatee = HelpFunction.ConvertdbnullsBool(objDR("Manatee"))
                localOkeechobee = HelpFunction.ConvertdbnullsBool(objDR("Okeechobee"))
                localSarasota = HelpFunction.ConvertdbnullsBool(objDR("Sarasota"))
                localBroward = HelpFunction.ConvertdbnullsBool(objDR("Broward"))
                localMiamiDade = HelpFunction.ConvertdbnullsBool(objDR("Miami-Dade"))
                localMonroe = HelpFunction.ConvertdbnullsBool(objDR("Monroe"))
                localPalmBeach = HelpFunction.ConvertdbnullsBool(objDR("Palm Beach"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()


        Catch ex As Exception

            Response.Write(ex.ToString)
            Exit Sub

        End Try

        If localAlachua = True Then
            localAllCounties = localAllCounties & " Alachua, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localBaker = True Then
            localAllCounties = localAllCounties & " Baker, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localBay = True Then
            localAllCounties = localAllCounties & " Bay, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localBradford = True Then
            localAllCounties = localAllCounties & " Bradford, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localBrevard = True Then
            localAllCounties = localAllCounties & " Brevard, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localBroward = True Then
            localAllCounties = localAllCounties & " Broward, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localCalhoun = True Then
            localAllCounties = localAllCounties & " Calhoun, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localCharlotte = True Then
            localAllCounties = localAllCounties & " Charlotte, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localCitrus = True Then
            localAllCounties = localAllCounties & " Citrus, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localClay = True Then
            localAllCounties = localAllCounties & " Clay, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localCollier = True Then
            localAllCounties = localAllCounties & " Collier, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localColumbia = True Then
            localAllCounties = localAllCounties & " Columbia, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localDeSoto = True Then
            localAllCounties = localAllCounties & " DeSoto, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localDixie = True Then
            localAllCounties = localAllCounties & " Dixie, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localDuval = True Then
            localAllCounties = localAllCounties & " Duval, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localEscambia = True Then
            localAllCounties = localAllCounties & " Escambia, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localFlagler = True Then
            localAllCounties = localAllCounties & " Flagler, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localFranklin = True Then
            localAllCounties = localAllCounties & " Franklin, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localGadsden = True Then
            localAllCounties = localAllCounties & " Gadsden, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localGilchrist = True Then
            localAllCounties = localAllCounties & " Gilchrist, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localGlades = True Then
            localAllCounties = localAllCounties & " Glades, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localGulf = True Then
            localAllCounties = localAllCounties & " Gulf, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localHamilton = True Then
            localAllCounties = localAllCounties & " Hamilton, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localHardee = True Then
            localAllCounties = localAllCounties & " Hardee, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localHendry = True Then
            localAllCounties = localAllCounties & " Hendry, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localHernando = True Then
            localAllCounties = localAllCounties & " Hernando, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localHighlands = True Then
            localAllCounties = localAllCounties & " Highlands, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localHillsborough = True Then
            localAllCounties = localAllCounties & " Hillsborough, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localHolmes = True Then
            localAllCounties = localAllCounties & " Holmes, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localIndianRiver = True Then
            localAllCounties = localAllCounties & " Indian River, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localJackson = True Then
            localAllCounties = localAllCounties & " Jackson, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localJefferson = True Then
            localAllCounties = localAllCounties & " Jefferson, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localLafayette = True Then
            localAllCounties = localAllCounties & " Lafayette, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localLake = True Then
            localAllCounties = localAllCounties & " Lake, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localLee = True Then
            localAllCounties = localAllCounties & " Lee, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localLeon = True Then
            localAllCounties = localAllCounties & " Leon, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localLevy = True Then
            localAllCounties = localAllCounties & " Levy, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localLiberty = True Then
            localAllCounties = localAllCounties & " Liberty, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localMadison = True Then
            localAllCounties = localAllCounties & " Madison, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localManatee = True Then
            localAllCounties = localAllCounties & " Manatee, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localMarion = True Then
            localAllCounties = localAllCounties & " Marion, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localMartin = True Then
            localAllCounties = localAllCounties & " Martin, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localMiamiDade = True Then
            localAllCounties = localAllCounties & " Miami-Dade, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localMonroe = True Then
            localAllCounties = localAllCounties & " Monroe, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localNassau = True Then
            localAllCounties = localAllCounties & " Nassau, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localOkaloosa = True Then
            localAllCounties = localAllCounties & " Okaloosa, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localOkeechobee = True Then
            localAllCounties = localAllCounties & " Okeechobee, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localOrange = True Then
            localAllCounties = localAllCounties & " Orange, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localOsceola = True Then
            localAllCounties = localAllCounties & " Osceola, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localPalmBeach = True Then
            localAllCounties = localAllCounties & " Palm Beach, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localPasco = True Then
            localAllCounties = localAllCounties & " Pasco, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localPinellas = True Then
            localAllCounties = localAllCounties & " Pinellas, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localPolk = True Then
            localAllCounties = localAllCounties & " Polk, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localPutnam = True Then
            localAllCounties = localAllCounties & " Putnam, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localSantaRosa = True Then
            localAllCounties = localAllCounties & " Santa Rosa, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localSarasota = True Then
            localAllCounties = localAllCounties & " Sarasota, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localSeminole = True Then
            localAllCounties = localAllCounties & " Seminole, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localStJohns = True Then
            localAllCounties = localAllCounties & " St. Johns, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localStLucie = True Then
            localAllCounties = localAllCounties & " St. Lucie, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localSumter = True Then
            localAllCounties = localAllCounties & " Sumter, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localSuwannee = True Then
            localAllCounties = localAllCounties & " Suwannee, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localTaylor = True Then
            localAllCounties = localAllCounties & " Taylor, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localUnion = True Then
            localAllCounties = localAllCounties & " Union, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localVolusia = True Then
            localAllCounties = localAllCounties & " Volusia, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localWakulla = True Then
            localAllCounties = localAllCounties & " Wakulla, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localWalton = True Then
            localAllCounties = localAllCounties & " Walton, "
            globalCountyCount = globalCountyCount + 1
        End If

        If localWashington = True Then
            localAllCounties = localAllCounties & " Washington, "
            globalCountyCount = globalCountyCount + 1
        End If

        'Gets rid of Last ,
        If localAllCounties <> "" Then
            localAllCounties = localAllCounties.Remove(localAllCounties.Length - 2, 2)
        Else
            localAllCounties = " NO COUNTIES ADDED AT THIS TIME"
        End If

        lblAffectedCounties.Text = localAllCounties

        If localRecordCount <> 0 Then
            'We have records show label
            pnlShowAffectedCounties.Visible = True
        Else
            'Hide Grid
            pnlShowAffectedCounties.Visible = False
        End If

    End Sub

    Protected Sub getLink()

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

        objCmd = New SqlCommand("spSelectLinkByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

        objDS.Tables.Clear()
        'bind our data
        objDA = New SqlDataAdapter(objCmd)
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing

        DBConStringHelper.FinalizeConnection(objConn) 'close the connection

        'call the calculate grid counts to show the number of records, the page you are on, etc.
        objDataGridFunctions.CalcDataGridCounts(objDS, LinkDataGrid, "")
        'Associate the data grid with the data
        LinkDataGrid.DataSource = objDS.Tables(0).DefaultView
        LinkDataGrid.DataBind()

        objDataGridFunctions.Highlightrows(LinkDataGrid, "", "", "")

        'Checking to see if we have any Contact Methods
        'Response.Write(objDS.Tables(0).Rows.Count)

        If CInt(objDS.Tables(0).Rows.Count) <> 0 Then
            'We have records show the Grid
            pnlShowLink.Visible = True
        Else
            'Hide Grid
            pnlShowLink.Visible = False
        End If

    End Sub


    'ErrorChecks
    Protected Sub ErrorChecksStep1()

        Dim strError As New System.Text.StringBuilder

        'Start The Error String
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        If txtIncidentName.Text = "" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide an Incident Name. <br />")
            globalHasErrors = True

        End If

        If ddlIncidentStatus.SelectedValue = "0" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select an Incident Status. <br />")
            globalHasErrors = True

        End If

        If ddlReportingPartyType.SelectedValue = "0" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a Reporting Party. <br />")
            globalHasErrors = True

        End If

        If ddlSeverity.SelectedValue = "0" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a Severity. <br />")
            globalHasErrors = True

        End If


        If ddlReportingPartyType.SelectedValue = "3" Then

            If txtReportingPartyFirstName.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Reporting Party First Name. <br />")
                globalHasErrors = True

            End If

        End If

        If ddlOnSceneContactType.SelectedValue = "0" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select an On-Scene Contact. <br />")
            globalHasErrors = True

        End If

        If ddlOnSceneContactType.SelectedValue = "2" Then

            If txtOnSceneContactFirstName.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide an On-Scene Contact First Name. <br />")
                globalHasErrors = True

            End If

        End If


        If ddlResponsiblePartyType.SelectedValue = "0" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a Responsible Party. <br />")
            globalHasErrors = True

        End If

        If ddlResponsiblePartyType.SelectedValue = "4" Then

            If txtResponsiblePartyFirstName.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Responsible Party First Name. <br />")
                globalHasErrors = True

            End If

        End If

        If txtInitialReport.Text = "" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide an Initial Report. <br />")
            globalHasErrors = True

        End If


        'Time and Date Checks
        'Time Validation
        If txtIncidentOccurredTime.Text = "" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Time. <br />")
            globalHasErrors = True

        End If

        If txtIncidentOccurredTime.Text <> "" Then

            If txtIncidentOccurredTime2.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Time. <br />")
                globalHasErrors = True

            End If

            If txtIncidentOccurredTime2.Text <> "" Then
                'Now we check if its an integer

                Try
                    Dim time1 As Integer = CInt(txtIncidentOccurredTime.Text)
                    Dim time2 As Integer = CInt(txtIncidentOccurredTime2.Text)

                    If time1 > 23 Or time1 < 0 Then
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Time. <br />")
                        globalHasErrors = True
                        Exit Try
                    End If

                    If time2 > 59 Or time2 < 0 Then
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Incident Occurred Time.<br />")
                        globalHasErrors = True
                        Exit Try
                    End If

                Catch ex As Exception
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Incident Occurred Time. <br />")
                    globalHasErrors = True
                End Try


            End If

        End If


        'Date Validation
        If IsDate(txtIncidentOccurredDate.Text) = False Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Incident Occurred Date. <br />")
            globalHasErrors = True
        End If

        'Time Validation
        If txtReportedToSWOTime.Text = "" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Reported to SWO Time. <br />")
            globalHasErrors = True

        End If

        If txtReportedToSWOTime.Text <> "" Then

            If txtReportedToSWOTime2.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Reported to SWO Time. <br />")
                globalHasErrors = True

            End If

            If txtReportedToSWOTime2.Text <> "" Then
                'Now we check if its an integer

                Try
                    Dim time1 As Integer = CInt(txtReportedToSWOTime.Text)
                    Dim time2 As Integer = CInt(txtReportedToSWOTime2.Text)

                    If time1 > 23 Or time1 < 0 Then
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Reported to SWO Time. <br />")
                        globalHasErrors = True
                        Exit Try
                    End If

                    If time2 > 59 Or time2 < 0 Then
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Reported to SWO Time. <br />")
                        globalHasErrors = True
                        Exit Try
                    End If

                Catch ex As Exception
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Reported to SWO Time. <br />")
                    globalHasErrors = True
                End Try


            End If

        End If


        'Date Validation
        If IsDate(txtReportedToSWODate.Text) = False Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Reported to SWO Date. <br />")
            globalHasErrors = True
        End If




        'The Choose from the Radio Buttons Below to Obtain Coordinates Check Start==========================
        Dim localCoordinateRadioButtonCount As Integer = 0

        '1
        If rdoFacilityNameSceneDescription.Checked = False Then
            localCoordinateRadioButtonCount = localCoordinateRadioButtonCount + 1
        End If

        If rdoFacilityNameSceneDescription.Checked = True Then

            If txtFacilityNameSceneDescription.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for Facility Name or Scene Description <br />")
                globalHasErrors = True

            End If

        End If

        '2
        If rdoAddressCity.Checked = False Then
            localCoordinateRadioButtonCount = localCoordinateRadioButtonCount + 1
        End If

        If rdoAddressCity.Checked = True Then

            If txtAddress.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for Address in Address City <br />")
                globalHasErrors = True

            End If

            If txtCity.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for City in Address City <br />")
                globalHasErrors = True

            End If

        End If

        '3
        If rdoByAddressZip.Checked = False Then
            localCoordinateRadioButtonCount = localCoordinateRadioButtonCount + 1
        End If

        If rdoByAddressZip.Checked = True Then

            If txtAddress2.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for Address in Address Zip <br />")
                globalHasErrors = True

            End If

            If txtZip.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for Zip in Address Zip <br />")
                globalHasErrors = True

            End If

        End If

        '4
        If rdoByIntersection.Checked = False Then

            localCoordinateRadioButtonCount = localCoordinateRadioButtonCount + 1

        End If

        If rdoByIntersection.Checked = True Then

            If txtStreet.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for Street in Intersection <br />")
                globalHasErrors = True

            End If

            If txtStreet2.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for Street 2 in Intersection <br />")
                globalHasErrors = True

            End If

            If txtCity2.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for City in Intersection City <br />")
                globalHasErrors = True

            End If

        End If


        '5
        If rdoAffectedCounties.Checked = False Then
            localCoordinateRadioButtonCount = localCoordinateRadioButtonCount + 1
        End If

        If rdoAffectedCounties.Checked = True Then

            If lblAffectedCounties.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide at least one county in Affected Counties <br />")
                globalHasErrors = True

            End If

        End If

        '6
        If rdoByCoordinateEntry.Checked = False Then
            localCoordinateRadioButtonCount = localCoordinateRadioButtonCount + 1
        End If

        If rdoByCoordinateEntry.Checked = True Then
            ' Now we must see if we have legit coordinates:
            If rdoUSNG.Checked = True Then

                If txtUSNG.Text = "" Then

                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a USNG. <br />")
                    globalHasErrors = True

                End If

            End If

            'Degrees Minutes Check
            If rdoDegreesMinutes.Checked = True Then

                'Latitude Checks Start Here=====================================================================
                'If txtLatDegreesMinutes.Text = "" Then

                '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Degrees Latitude. <br />")
                '    globalHasErrors = True

                'End If

                'If txtLatDegreesMinutes2.Text = "" Then

                '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Minutes Latitude. <br />")
                '    globalHasErrors = True

                'End If

                Dim localLatitudeDegreesString As String
                Dim localLatitudeDegrees As Decimal

                Try
                    localLatitudeDegreesString = txtLatDegreesMinutes.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLatitudeDegrees = CDec(localLatitudeDegreesString)

                    'Its a number so we check if it falls between the values
                    If localLatitudeDegrees < 24 Or localLatitudeDegrees > 32 Then
                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Latitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Latitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try


                Dim localLatitudeMinutesString As String
                Dim localLatitudeMinutes As Decimal

                Try
                    localLatitudeMinutesString = txtLatDegreesMinutes2.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLatitudeMinutes = CDec(localLatitudeMinutesString)

                    'Its a number so we check if it falls between the values
                    If localLatitudeMinutes < 0.0 Or localLatitudeMinutes > 59.99 Then
                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Minutes Latitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Minutes Latitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try

                'Latitude Checks Ends Here=====================================================================


                'Longitude Checks Start Here===================================================================
                'If txtLongDegreesMinutes.Text = "" Then

                '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Degrees Longitude. <br />")
                '    globalHasErrors = True

                'End If


                Dim localLongitudeDegreesString As String
                Dim localLongitudeDegrees As Decimal

                Try
                    localLongitudeDegreesString = txtLongDegreesMinutes.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLongitudeDegrees = CDec(localLongitudeDegreesString)

                    'Its a number so we check if it falls between the values
                    If localLongitudeDegrees < -89 Or localLongitudeDegrees > -79 Then
                        'Response.Write("localLatitude" & localLongitude)
                        'Response.End()

                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Longitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Longitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try

                Dim localLongitudeMinutesString As String
                Dim localLongitudeMinutes As Decimal

                Try
                    localLongitudeMinutesString = txtLongDegreesMinutes2.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLongitudeMinutes = CDec(localLongitudeMinutesString)

                    'Its a number so we check if it falls between the values
                    If localLongitudeMinutes < 0.0 Or localLongitudeMinutes > 59.99 Then
                        'Response.Write("localLatitude" & localLongitude)
                        'Response.End()

                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Longitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Longitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try

                'Longitude Checks Ends Here===================================================================

            End If



            'Degrees Minutes Check
            If rdoDegreesMinutesSeconds.Checked = True Then


                'Latitude Checks Start Here===================================================================


                'Lat Degrees==============================================
                Dim localLatitudeDegreesString As String
                Dim localLatitudeDegrees As Decimal

                Try
                    localLatitudeDegreesString = txtLatDegreesMinutesSeconds.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLatitudeDegrees = CDec(localLatitudeDegreesString)

                    'Its a number so we check if it falls between the values
                    If localLatitudeDegrees < 24 Or localLatitudeDegrees > 32 Then
                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Latitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Latitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try
                'Lat Degrees==============================================


                'Lat Minutes==============================================
                Dim localLatitudeMinutesString As String
                Dim localLatitudeMinutes As Decimal

                Try
                    localLatitudeMinutesString = txtLatDegreesMinutesSeconds2.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLatitudeMinutes = CDec(localLatitudeMinutesString)

                    'Its a number so we check if it falls between the values
                    If localLatitudeMinutes < 0.0 Or localLatitudeMinutes > 59.99 Then
                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Minutes Latitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Minutes Latitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try
                'Lat Minutes==============================================


                'Lat Seconds==============================================
                Dim localLatitudeSecondsString As String
                Dim localLatitudeSeconds As Decimal

                Try
                    localLatitudeSecondsString = txtLatDegreesMinutesSeconds3.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLatitudeSeconds = CDec(localLatitudeSecondsString)

                    'Its a number so we check if it falls between the values
                    If localLatitudeSeconds < 0.0 Or localLatitudeSeconds > 59.99 Then
                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Seconds Latitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Seconds Latitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try

                'Lat Seconds==============================================


                'Latitude Checks End Here===================================================================



                'Longitude Checks Start Here===================================================================
                'If txtLongDegreesMinutes.Text = "" Then

                '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Degrees Longitude. <br />")
                '    globalHasErrors = True

                'End If


                Dim localLongitudeDegreesString As String
                Dim localLongitudeDegrees As Decimal

                Try
                    localLongitudeDegreesString = txtLongDegreesMinutesSeconds.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLongitudeDegrees = CDec(localLongitudeDegreesString)

                    'Its a number so we check if it falls between the values
                    If localLongitudeDegrees < -89 Or localLongitudeDegrees > -79 Then
                        'Response.Write("localLatitude" & localLongitude)
                        'Response.End()

                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Longitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Longitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try

                Dim localLongitudeMinutesString As String
                Dim localLongitudeMinutes As Decimal

                Try
                    localLongitudeMinutesString = txtLongDegreesMinutesSeconds2.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLongitudeMinutes = CDec(localLongitudeMinutesString)

                    'Its a number so we check if it falls between the values
                    If localLongitudeMinutes < 0.0 Or localLongitudeMinutes > 59.99 Then
                        'Response.Write("localLatitude" & localLongitude)
                        'Response.End()

                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Longitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Longitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try

                Dim localLongitudeSecondsString As String
                Dim localLongitudeSeconds As Decimal

                Try
                    localLongitudeSecondsString = txtLongDegreesMinutesSeconds3.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLongitudeSeconds = CDec(localLongitudeSecondsString)

                    'Its a number so we check if it falls between the values
                    If localLongitudeSeconds < 0.0 Or localLongitudeSeconds > 59.99 Then
                        'Response.Write("localLatitude" & localLongitude)
                        'Response.End()

                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Seconds Longitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Seconds Longitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try

                'Longitude Checks Ends Here===================================================================

            End If




            'Decimal Degrees Check
            If rdoDecimalDegrees.Checked = True Then

                ''Latitude and Long Checks Start Here
                'If txtLatDecimalDegrees.Text = "" Then

                '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Latitude. <br />")
                '    globalHasErrors = True

                'End If

                Dim localLatitudeString As String
                Dim localLatitude As Decimal

                Try
                    localLatitudeString = txtLatDecimalDegrees.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLatitude = CDec(localLatitudeString)

                    'Its a number so we check if it falls between the values
                    If localLatitude < 24 Or localLatitude > 32 Then
                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Latitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Latitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try


                'Response.Write("localLatitude" & localLatitude)
                'Response.End()

                ''localLatitude = CDec(localLatitude)


                'If txtLongDecimalDegrees.Text = "" Then

                '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Longitude. <br />")
                '    globalHasErrors = True

                'End If


                Dim localLongitudeString As String
                Dim localLongitude As Decimal

                Try
                    localLongitudeString = txtLongDecimalDegrees.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLongitude = CDec(localLongitudeString)

                    'Its a number so we check if it falls between the values
                    If localLongitude < -89 Or localLongitude > -79 Then
                        'Response.Write("localLatitude" & localLongitude)
                        'Response.End()

                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Longitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Longitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try

            End If

        End If

        If localCoordinateRadioButtonCount = 6 Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must choose an option to obtain location coordinates. <br />")
            globalHasErrors = True

        End If
        'The Choose from the Radio Buttons Below to Obtain Coordinates Check End==========================





        'Finish the Error String
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels
        lblMessage.Text = strError.ToString


    End Sub

    Protected Sub ErrorChecksStep2()

        Dim strError As New System.Text.StringBuilder

        'Start The Error String
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        If txtIncidentName.Text = "" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide an Incident Name. <br />")
            globalHasErrors = True

        End If

        If ddlIncidentStatus.SelectedValue = "0" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select an Incident Status. <br />")
            globalHasErrors = True

        End If

        If ddlReportingPartyType.SelectedValue = "0" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a Reporting Party. <br />")
            globalHasErrors = True

        End If

        If ddlReportingPartyType.SelectedValue = "3" Then

            If txtReportingPartyFirstName.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Reporting Party First Name. <br />")
                globalHasErrors = True

            End If

        End If

        If ddlOnSceneContactType.SelectedValue = "0" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select an On-Scene Contact. <br />")
            globalHasErrors = True

        End If

        If ddlOnSceneContactType.SelectedValue = "2" Then

            If txtOnSceneContactFirstName.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide an On-Scene Contact First Name. <br />")
                globalHasErrors = True

            End If

        End If


        If ddlResponsiblePartyType.SelectedValue = "0" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a Responsible Party. <br />")
            globalHasErrors = True

        End If

        If ddlResponsiblePartyType.SelectedValue = "4" Then

            If txtResponsiblePartyFirstName.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Responsible Party First Name. <br />")
                globalHasErrors = True

            End If

        End If


        'Time and Date Checks
        'Time Validation
        If txtIncidentOccurredTime.Text = "" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Time. <br />")
            globalHasErrors = True

        End If

        If txtIncidentOccurredTime.Text <> "" Then

            If txtIncidentOccurredTime2.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Time. <br />")
                globalHasErrors = True

            End If

            If txtIncidentOccurredTime2.Text <> "" Then
                'Now we check if its an integer

                Try
                    Dim time1 As Integer = CInt(txtIncidentOccurredTime.Text)
                    Dim time2 As Integer = CInt(txtIncidentOccurredTime2.Text)

                    If time1 > 23 Or time1 < 0 Then
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Time. <br />")
                        globalHasErrors = True
                        Exit Try
                    End If

                    If time2 > 59 Or time2 < 0 Then
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Incident Occurred Time.<br />")
                        globalHasErrors = True
                        Exit Try
                    End If

                Catch ex As Exception
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Incident Occurred Time. <br />")
                    globalHasErrors = True
                End Try


            End If

        End If


        'Date Validation
        If IsDate(txtIncidentOccurredDate.Text) = False Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Incident Occurred Date. <br />")
            globalHasErrors = True
        End If

        'Time Validation
        If txtReportedToSWOTime.Text = "" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Reported to SWO Time. <br />")
            globalHasErrors = True

        End If

        If txtReportedToSWOTime.Text <> "" Then

            If txtReportedToSWOTime2.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Reported to SWO Time. <br />")
                globalHasErrors = True

            End If

            If txtReportedToSWOTime2.Text <> "" Then
                'Now we check if its an integer

                Try
                    Dim time1 As Integer = CInt(txtReportedToSWOTime.Text)
                    Dim time2 As Integer = CInt(txtReportedToSWOTime2.Text)

                    If time1 > 23 Or time1 < 0 Then
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Reported to SWO Time. <br />")
                        globalHasErrors = True
                        Exit Try
                    End If

                    If time2 > 59 Or time2 < 0 Then
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Reported to SWO Time. <br />")
                        globalHasErrors = True
                        Exit Try
                    End If

                Catch ex As Exception
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Reported to SWO Time. <br />")
                    globalHasErrors = True
                End Try


            End If

        End If


        'Date Validation
        If IsDate(txtReportedToSWODate.Text) = False Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Reported to SWO Date. <br />")
            globalHasErrors = True
        End If




        'The Choose from the Radio Buttons Below to Obtain Coordinates Check Start==========================
        Dim localCoordinateRadioButtonCount As Integer = 0

        '1
        If rdoFacilityNameSceneDescription.Checked = False Then
            localCoordinateRadioButtonCount = localCoordinateRadioButtonCount + 1
        End If

        If rdoFacilityNameSceneDescription.Checked = True Then

            If txtFacilityNameSceneDescription.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for Facility Name or Scene Description <br />")
                globalHasErrors = True

            End If

        End If

        '2
        If rdoAddressCity.Checked = False Then
            localCoordinateRadioButtonCount = localCoordinateRadioButtonCount + 1
        End If

        If rdoAddressCity.Checked = True Then

            If txtAddress.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for Address in Address City <br />")
                globalHasErrors = True

            End If

            If txtCity.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for City in Address City <br />")
                globalHasErrors = True

            End If

        End If

        '3
        If rdoByAddressZip.Checked = False Then
            localCoordinateRadioButtonCount = localCoordinateRadioButtonCount + 1
        End If

        If rdoByAddressZip.Checked = True Then

            If txtAddress2.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for Address in Address Zip <br />")
                globalHasErrors = True

            End If

            If txtZip.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for Zip in Address Zip <br />")
                globalHasErrors = True

            End If

        End If

        '4
        If rdoByIntersection.Checked = False Then

            localCoordinateRadioButtonCount = localCoordinateRadioButtonCount + 1

        End If

        If rdoByIntersection.Checked = True Then

            If txtStreet.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for Street in Intersection <br />")
                globalHasErrors = True

            End If

            If txtStreet2.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for Street 2 in Intersection <br />")
                globalHasErrors = True

            End If

        End If


        '5
        If rdoAffectedCounties.Checked = False Then
            localCoordinateRadioButtonCount = localCoordinateRadioButtonCount + 1
        End If

        If rdoAffectedCounties.Checked = True Then

            If lblAffectedCounties.Text = "" Then

                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide at least one county in Affected Counties <br />")
                globalHasErrors = True

            End If

        End If

        '6
        If rdoByCoordinateEntry.Checked = False Then
            localCoordinateRadioButtonCount = localCoordinateRadioButtonCount + 1
        End If

        If rdoByCoordinateEntry.Checked = True Then
            ' Now we must see if we have legit coordinates:
            If rdoUSNG.Checked = True Then

                If txtUSNG.Text = "" Then

                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a USNG. <br />")
                    globalHasErrors = True

                End If

            End If

            'Degrees Minutes Check
            If rdoDegreesMinutes.Checked = True Then

                'Latitude Checks Start Here=====================================================================
                'If txtLatDegreesMinutes.Text = "" Then

                '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Degrees Latitude. <br />")
                '    globalHasErrors = True

                'End If

                'If txtLatDegreesMinutes2.Text = "" Then

                '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Minutes Latitude. <br />")
                '    globalHasErrors = True

                'End If

                Dim localLatitudeDegreesString As String
                Dim localLatitudeDegrees As Decimal

                Try
                    localLatitudeDegreesString = txtLatDegreesMinutes.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLatitudeDegrees = CDec(localLatitudeDegreesString)

                    'Its a number so we check if it falls between the values
                    If localLatitudeDegrees < 24 Or localLatitudeDegrees > 32 Then
                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Latitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Latitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try


                Dim localLatitudeMinutesString As String
                Dim localLatitudeMinutes As Decimal

                Try
                    localLatitudeMinutesString = txtLatDegreesMinutes2.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLatitudeMinutes = CDec(localLatitudeMinutesString)

                    'Its a number so we check if it falls between the values
                    If localLatitudeMinutes < 0.0 Or localLatitudeMinutes > 59.99 Then
                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Minutes Latitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Minutes Latitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try

                'Latitude Checks Ends Here=====================================================================


                'Longitude Checks Start Here===================================================================
                'If txtLongDegreesMinutes.Text = "" Then

                '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Degrees Longitude. <br />")
                '    globalHasErrors = True

                'End If


                Dim localLongitudeDegreesString As String
                Dim localLongitudeDegrees As Decimal

                Try
                    localLongitudeDegreesString = txtLongDegreesMinutes.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLongitudeDegrees = CDec(localLongitudeDegreesString)

                    'Its a number so we check if it falls between the values
                    If localLongitudeDegrees < -89 Or localLongitudeDegrees > -79 Then
                        'Response.Write("localLatitude" & localLongitude)
                        'Response.End()

                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Longitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Longitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try

                Dim localLongitudeMinutesString As String
                Dim localLongitudeMinutes As Decimal

                Try
                    localLongitudeMinutesString = txtLongDegreesMinutes2.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLongitudeMinutes = CDec(localLongitudeMinutesString)

                    'Its a number so we check if it falls between the values
                    If localLongitudeMinutes < 0.0 Or localLongitudeMinutes > 59.99 Then
                        'Response.Write("localLatitude" & localLongitude)
                        'Response.End()

                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Longitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Longitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try

                'Longitude Checks Ends Here===================================================================

            End If



            'Degrees Minutes Check
            If rdoDegreesMinutesSeconds.Checked = True Then


                'Latitude Checks Start Here===================================================================


                'Lat Degrees==============================================
                Dim localLatitudeDegreesString As String
                Dim localLatitudeDegrees As Decimal

                Try
                    localLatitudeDegreesString = txtLatDegreesMinutesSeconds.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLatitudeDegrees = CDec(localLatitudeDegreesString)

                    'Its a number so we check if it falls between the values
                    If localLatitudeDegrees < 24 Or localLatitudeDegrees > 32 Then
                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Latitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Latitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try
                'Lat Degrees==============================================


                'Lat Minutes==============================================
                Dim localLatitudeMinutesString As String
                Dim localLatitudeMinutes As Decimal

                Try
                    localLatitudeMinutesString = txtLatDegreesMinutesSeconds2.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLatitudeMinutes = CDec(localLatitudeMinutesString)

                    'Its a number so we check if it falls between the values
                    If localLatitudeMinutes < 0.0 Or localLatitudeMinutes > 59.99 Then
                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Minutes Latitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Minutes Latitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try
                'Lat Minutes==============================================


                'Lat Seconds==============================================
                Dim localLatitudeSecondsString As String
                Dim localLatitudeSeconds As Decimal

                Try
                    localLatitudeSecondsString = txtLatDegreesMinutesSeconds3.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLatitudeSeconds = CDec(localLatitudeSecondsString)

                    'Its a number so we check if it falls between the values
                    If localLatitudeSeconds < 0.0 Or localLatitudeSeconds > 59.99 Then
                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Seconds Latitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Seconds Latitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try

                'Lat Seconds==============================================


                'Latitude Checks End Here===================================================================



                'Longitude Checks Start Here===================================================================
                'If txtLongDegreesMinutes.Text = "" Then

                '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Degrees Longitude. <br />")
                '    globalHasErrors = True

                'End If


                Dim localLongitudeDegreesString As String
                Dim localLongitudeDegrees As Decimal

                Try
                    localLongitudeDegreesString = txtLongDegreesMinutesSeconds.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLongitudeDegrees = CDec(localLongitudeDegreesString)

                    'Its a number so we check if it falls between the values
                    If localLongitudeDegrees < -89 Or localLongitudeDegrees > -79 Then
                        'Response.Write("localLatitude" & localLongitude)
                        'Response.End()

                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Longitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Longitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try

                Dim localLongitudeMinutesString As String
                Dim localLongitudeMinutes As Decimal

                Try
                    localLongitudeMinutesString = txtLongDegreesMinutesSeconds2.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLongitudeMinutes = CDec(localLongitudeMinutesString)

                    'Its a number so we check if it falls between the values
                    If localLongitudeMinutes < 0.0 Or localLongitudeMinutes > 59.99 Then
                        'Response.Write("localLatitude" & localLongitude)
                        'Response.End()

                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Longitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Degrees Longitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try

                Dim localLongitudeSecondsString As String
                Dim localLongitudeSeconds As Decimal

                Try
                    localLongitudeSecondsString = txtLongDegreesMinutesSeconds3.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLongitudeSeconds = CDec(localLongitudeSecondsString)

                    'Its a number so we check if it falls between the values
                    If localLongitudeSeconds < 0.0 Or localLongitudeSeconds > 59.99 Then
                        'Response.Write("localLatitude" & localLongitude)
                        'Response.End()

                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Seconds Longitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Seconds Longitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try

                'Longitude Checks Ends Here===================================================================

            End If




            'Decimal Degrees Check
            If rdoDecimalDegrees.Checked = True Then

                ''Latitude and Long Checks Start Here
                'If txtLatDecimalDegrees.Text = "" Then

                '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Latitude. <br />")
                '    globalHasErrors = True

                'End If

                Dim localLatitudeString As String
                Dim localLatitude As Decimal

                Try
                    localLatitudeString = txtLatDecimalDegrees.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLatitude = CDec(localLatitudeString)

                    'Its a number so we check if it falls between the values
                    If localLatitude < 24 Or localLatitude > 32 Then
                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Latitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Latitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try


                'Response.Write("localLatitude" & localLatitude)
                'Response.End()

                ''localLatitude = CDec(localLatitude)


                'If txtLongDecimalDegrees.Text = "" Then

                '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Longitude. <br />")
                '    globalHasErrors = True

                'End If


                Dim localLongitudeString As String
                Dim localLongitude As Decimal

                Try
                    localLongitudeString = txtLongDecimalDegrees.Text.ToString.Trim

                    'Checks to See if this is a string
                    localLongitude = CDec(localLongitudeString)

                    'Its a number so we check if it falls between the values
                    If localLongitude < -89 Or localLongitude > -79 Then
                        'Response.Write("localLatitude" & localLongitude)
                        'Response.End()

                        'Number out of range so we fail
                        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Longitude Within Florida Area of Operations. <br />")
                        globalHasErrors = True

                    End If

                Catch ex As Exception
                    ' Its a String we kill the process
                    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Longitude Within Florida Area of Operations. <br />")
                    globalHasErrors = True

                End Try

            End If

        End If

        If localCoordinateRadioButtonCount = 6 Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must choose an option to obtain location coordinates. <br />")
            globalHasErrors = True

        End If
        'The Choose from the Radio Buttons Below to Obtain Coordinates Check End==========================





        'Finish the Error String
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels
        lblMessage.Text = strError.ToString


    End Sub

    Protected Sub ErrorChecksAttachment()

        Dim strError As New System.Text.StringBuilder

        'Start The Error String
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        If txtAttachmentName.Text = "" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide an Attachment Name. <br />")
            globalHasErrors = True

        End If

        If FileUpload1.HasFile = False Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select an Attachment. <br />")
            globalHasErrors = True

        End If

        'Finish the Error String
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels
        lblMessage.Text = strError.ToString

    End Sub

    Protected Sub ErrorChecksLink()

        Dim strError As New System.Text.StringBuilder

        'Start The Error String
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        If txtLink.Text = "" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Link. <br />")
            globalHasErrors = True

        End If


        'Finish the Error String
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels
        lblMessage.Text = strError.ToString

    End Sub

    Protected Sub ErrorChecksIncidentType()


        Dim strError As New System.Text.StringBuilder

        'Start The Error String
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")


        'Adding the appropriate errors to the error string


        If ddlIncidentType.SelectedValue = "0" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select an Incident Type. <br />")
            globalHasErrors = True

        End If

        'Finish the Error String
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels
        lblMessage.Text = strError.ToString
        'lblMessage2.Text = strError.ToString

    End Sub


    'Delete
    Private Sub DeleteIncidentType()

        'Response.Write("Good")
        'Response.End()

        Dim localIncidentType As String = ""
        Dim localIncidentTypeID As Integer
        'Try
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectIncidentIncidentTypeIDByIncidentIncidentTypeID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            localIncidentTypeID = HelpFunction.ConvertdbnullsInt(objDR("IncidentTypeID"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        'Response.Write("localIncidentTypeID:" & localIncidentTypeID)
        'Response.Write("<br>")
        'Response.Write("localIncidentType:" & localIncidentType)
        'Response.Write("<br>")
        'Response.Write("IncidentIncidentTypeID:" & Request("IncidentIncidentTypeID"))
        'Response.End()

        '=============================================================================
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectIncidentTypeByIncidentTypeID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentTypeID", localIncidentTypeID)

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            localIncidentType = HelpFunction.Convertdbnulls(objDR("IncidentType"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        '================================================================================================
        Dim IncidentTypeID As Integer = MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("IncidentTypeID", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        'Response.Write(IncidentTypeID)
        'Response.End()


        Dim localIncidentTable As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("TableName", "IncidentType", "IncidentTypeID", IncidentTypeID)


        'Because of Late architect Info I have to make this bad algorithm because lack of time

        Dim LocalrecordCount As Integer = 0
        Dim localCountQueryString As String = ""


        localCountQueryString = "SELECT Count(*) As [Count] FROM " & localIncidentTable & " WHERE IncidentIncidentTypeID = " & Request("IncidentIncidentTypeID")

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectDynamicRowCount", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@QueryString ", localCountQueryString)

        'Response.Write(localCountQueryString)
        'Response.End()

        objDR = objCmd.ExecuteReader


        If objDR.Read() Then

            LocalrecordCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()


        '================================================================================================


        '================================================================================================
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        '// Enter the email and password to query/command object.
        objCmd = New SqlCommand("spDeleteIncidentIncidentTypeByIncidentIncidentTypeID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))


        '// Open the connection using the connection string.
        DBConStringHelper.PrepareConnection(objConn)

        '// Execute the command to the DataReader.
        objCmd.ExecuteNonQuery()

        '// Clean up our command objects and close the connection.
        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)

        '================================================================================================



        If LocalrecordCount <> 0 Then

            Dim localQueryString As String = ""

            localQueryString = "DELETE  FROM " & localIncidentTable & " WHERE IncidentIncidentTypeID = " & Request("IncidentIncidentTypeID")

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spDeleteIncidentTypeByIncidentIncidentTypeIDAndTable", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@QueryString", localQueryString)


            '// Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            '// Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()

            '// Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            '===============================================================================================
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString


        End If




        globalAuditAction = "Deleted Worksheet: " & localIncidentType & " "

        'AuditHelper.InsertIncidentAudit(Request("IncidentID"), oCookie.Item("UserID"), globalAuditAction, 3)

        AuditHelper.InsertReportUpdate(Request("IncidentID"), globalAuditAction, ns.UserID) 'oCookie.Item("UserID"))

        Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID") & "&Message=2")

        'Catch ex As Exception
        '    DBConStringHelper.FinalizeConnection(objConn)
        '    lblMessage.Text = "You may not delete this Incident Type due to the fact it is tied to related imported information. You must first delete all related imported information first, and then you may delete the Incident Type."
        '    lblMessage.Visible = True
        '    lblMessage.ForeColor = Drawing.Color.Red
        'End Try

    End Sub

    Private Sub DeleteAttachment()

        Dim localIncidentType As String = ""
        Dim localAttachmentName As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("AttachmentName", "Attachment", "AttachmentID", Request("AttachmentID"))
        Try
            'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
            '// Add cookie
            'Response.Cookies.Add(oCookie)
            ns = Session("Security_Tracker")

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spDeleteAttachmentByAttachmentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@AttachmentID", Request("AttachmentID"))


            '// Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            '// Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()

            '// Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            AuditHelper.InsertReportUpdate(Request("IncidentID"), "Deleted Attachment: " & localAttachmentName, ns.UserID) 'oCookie.Item("UserID"))

            Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID") & "&Message=2")

        Catch ex As Exception
            DBConStringHelper.FinalizeConnection(objConn)
            lblMessage.Text = "You may not delete this Attachment due to the fact it is tied to related imported information. You must first delete all related imported information first, and then you may delete the Attachment."
            lblMessage.Visible = True
            lblMessage.ForeColor = Drawing.Color.Red
        End Try

    End Sub

    Private Sub DeleteLink()

        Dim localIncidentType As String = ""
        Dim localLinkName As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("LinkName", "Link", "LinkID", Request("LinkID"))

        Try
            'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
            '// Add cookie
            'Response.Cookies.Add(oCookie)
            ns = Session("Security_Tracker")

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spDeleteLinkByLinkID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@LinkID", Request("LinkID"))


            '// Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            '// Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()

            '// Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            AuditHelper.InsertReportUpdate(Request("IncidentID"), "Deleted Link: " & localLinkName, ns.UserID) 'oCookie.Item("UserID"))

            Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID") & "&Message=2")

        Catch ex As Exception
            DBConStringHelper.FinalizeConnection(objConn)
            lblMessage.Text = "You may not delete this Link due to the fact it is tied to related imported information. You must first delete all related imported information first, and then you may delete the Link."
            lblMessage.Visible = True
            lblMessage.ForeColor = Drawing.Color.Red
        End Try

    End Sub

    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As EventArgs)





        globalIsSaved = MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("Saved", "Incident", "IncidentID", Request("IncidentID"))

        If globalIsSaved = False Then

            ErrorChecksStep1()

            If globalHasErrors = True Then

                'If we have errors, Show Message and Exit Sub. No Insert of Record
                pnlMessage.Visible = True
                'pnlMessage2.Visible = True

                globalHasErrors = False

                Exit Sub

            Else


                'Time to grab the Coordinates
                If rdoByCoordinateEntry.Checked = True Then
                    Convert()
                ElseIf rdoByAddressZip.Checked = True Then
                    UsingZipAddress()
                    Convert2()
                ElseIf rdoAddressCity.Checked = True Then
                    UsingAddressCity()
                    Convert2()
                End If

                SaveInitialReport()

                If MrDataGrabber.GrabRecordCountByKey("IncidentNumber", "IncidentID", Request("IncidentID")) = 0 Then
                    InsertIncidentNumber()
                End If

                InsertInitialReport()

                If ddlReportingPartyType.SelectedValue = 3 Then
                    SaveInitialReportingParty()
                End If

                If ddlOnSceneContactType.SelectedValue = 2 Then
                    SaveInitialOnSceneContact()
                End If

                If ddlResponsiblePartyType.SelectedValue = 4 Then
                    SaveInitialResponsibleParty()
                End If

            End If

        Else

            ErrorChecksStep2()


            If globalHasErrors = True Then

                'If we have errors, Show Message and Exit Sub. No Insert of Record
                pnlMessage.Visible = True
                'pnlMessage2.Visible = True

                globalHasErrors = False

                Exit Sub

            Else

                'Time to grab the Coordinates
                If rdoByCoordinateEntry.Checked = True Then
                    Convert()
                ElseIf rdoByAddressZip.Checked = True Then
                    UsingZipAddress()
                    Convert2()
                ElseIf rdoAddressCity.Checked = True Then
                    UsingAddressCity()
                    Convert2()
                ElseIf rdoByIntersection.Checked = True Then
                    UsingStreetsCity()
                    Convert2()
                End If

                SaveInitialReport2()

                If ddlReportingPartyType.SelectedValue = 3 Then
                    SaveInitialReportingParty()
                End If

                If ddlOnSceneContactType.SelectedValue = 2 Then
                    SaveInitialOnSceneContact()
                End If

                If ddlResponsiblePartyType.SelectedValue = 4 Then
                    SaveInitialResponsibleParty()
                End If

            End If

        End If

        Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID"))

    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        If ns.UserLevelID = "1" Then
            Response.Redirect("Incident.aspx")
        Else
            Response.Redirect("IncidentNonAdmin.aspx")
        End If

    End Sub

    Protected Sub btnAddIncidentType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddIncidentType.Click

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        ErrorChecksIncidentType()

        If globalHasErrors = True Then

            'If we have errors, Show Message and Exit Sub. No Insert of Record
            pnlMessage.Visible = True
            'pnlMessage2.Visible = True

            globalHasErrors = False

            Exit Sub

        Else

            '====================================================================
            'Now we can add to AI Table the new IncidentID so it all ties together

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spInsertIncidentIncidentType", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentTypeID", ddlIncidentType.SelectedValue)


            '// Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            '// Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()
            '// Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            '====================================================================
            'Now we must add a row to the Incident Update

            globalAuditAction = "Added Worksheet: " & ddlIncidentType.SelectedItem.ToString & "  "


            AuditHelper.InsertReportUpdate(Request("IncidentID"), globalAuditAction, ns.UserID) 'oCookie.Item("UserID"))


            'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID") & "&PagePopulation=IncidentType")

            'PopulateDDLs()

            getIncidentIncidentType()

            pnlShowIncidentTypeGrid.Visible = True

        End If


    End Sub

    Protected Sub UpdateReport()

        Dim NowDate As Date = Now

        Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.

            objCmd = New SqlCommand("spInsertMostRecentUpdateByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@UpdateDate", NowDate)
            objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))
            objCmd.Parameters.AddWithValue("@MostRecentUpdate", txtReportUpdate.Text)

            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

        Catch ex As Exception
            Response.Write(ex.ToString)

            Exit Sub
        End Try

        Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spUpdateIncidentReportUpdate", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@LastUpdatedByID", ns.UserID) 'oCookie.Item("UserID"))
            objCmd.Parameters.AddWithValue("@LastUpdated", NowDate)

            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

        Catch ex As Exception
            Response.Write(ex.ToString)

            Exit Sub
        End Try


        Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID"))

    End Sub



    Protected Sub btnAddAttachment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddAttachment.Click

        'Response.Write("Hello")

        ErrorChecksAttachment()

        If globalHasErrors = True Then

            'If we have errors, Show Message and Exit Sub. No Insert of Record
            pnlMessage.Visible = True
            'pnlMessage2.Visible = True

            globalHasErrors = False

            Exit Sub

        Else

            'Variables For Creation of Random Image Name
            Dim localRandomStringForImage As String = ""
            Dim localImageFileName As String = ""
            Dim localImageFilePathName As String = ""

            'Checking for a Upload
            If FileUpload1.HasFile Then

                'We have an upload

                'Random String Append to Image Name so we do not write over an existing Image
                localRandomStringForImage = HelpFunction.RandomStringGenerator(6)
                localImageFileName = localRandomStringForImage & FileUpload1.FileName
                localImageFileName = Replace(localImageFileName, " ", "")
                localImageFileName = Replace(localImageFileName, "%", "")
                localImageFileName = Replace(localImageFileName, "#", "")
                localImageFileName = Replace(localImageFileName, "!", "")
                ''Getting the Path Name of Image to store location in Database
                'localImageFilePathName = "Uploads\" & localImageFileName

                'Uploading and Saving the Image to the "Uploads" Folder
                FileUpload1.SaveAs(Server.MapPath("Uploads") & "\" & localImageFileName)

                'Response.Write(localImageFileName)
                'Response.End()

                'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
                '// Add cookie
                'Response.Cookies.Add(oCookie)
                ns = Session("Security_Tracker")

                Try
                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    '// Enter the email and password to query/command object.
                    objCmd = New SqlCommand("spInsertAttachment", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                    objCmd.Parameters.AddWithValue("@Attachment", localImageFileName)
                    objCmd.Parameters.AddWithValue("@AttachmentDate", Now)
                    objCmd.Parameters.AddWithValue("@AttachmentName", txtAttachmentName.Text)
                    objCmd.Parameters.AddWithValue("@UserName", ns.FullName)

                    DBConStringHelper.PrepareConnection(objConn)

                    objCmd.ExecuteNonQuery()

                    objCmd.Dispose()
                    objCmd = Nothing
                    DBConStringHelper.FinalizeConnection(objConn)

                Catch ex As Exception
                    Response.Write(ex.ToString)

                    Exit Sub
                End Try

            End If

            AuditHelper.InsertReportUpdate(Request("IncidentID"), "Added Attachment: " & txtAttachmentName.Text, ns.UserID) 'oCookie.Item("UserID"))

            'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID"))
            'PopulateDDLs()
            getAttachment()
            pnlShowAttachment.Visible = True
            txtAttachmentName.Text = ""
            btnSave.Focus()
        End If


    End Sub

    Protected Sub btnAddLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLink.Click

        ErrorChecksLink()

        If globalHasErrors = True Then

            'If we have errors, Show Message and Exit Sub. No Insert of Record
            pnlMessage.Visible = True
            'pnlMessage2.Visible = True

            globalHasErrors = False

            Exit Sub

        Else

            Try

                'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
                '// Add cookie
                'Response.Cookies.Add(oCookie)
                ns = Session("Security_Tracker")

                If Not (txtLink.Text.Contains("http://") Or txtLink.Text.Contains("https://") Or txtLink.Text.Contains("ftp://")) Then
                    txtLink.Text = "http://" & txtLink.Text
                End If

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                '// Enter the email and password to query/command object.
                objCmd = New SqlCommand("spInsertLink", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                objCmd.Parameters.AddWithValue("@Link", txtLink.Text)
                objCmd.Parameters.AddWithValue("@LinkDate", Now)
                objCmd.Parameters.AddWithValue("@LinkName", "") 'Removed link name from UI 20140325 bp
                objCmd.Parameters.AddWithValue("@UserName", ns.FullName)

                DBConStringHelper.PrepareConnection(objConn)

                objCmd.ExecuteNonQuery()

                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

            Catch ex As Exception
                Response.Write(ex.ToString)

                Exit Sub
            End Try

            AuditHelper.InsertReportUpdate(Request("IncidentID"), "Added Link: " & txtLink.Text, ns.UserID) 'oCookie.Item("UserID"))
            'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID"))
            'PopulateDDLs()
            getLink()
            pnlShowLink.Visible = True
            txtLink.Text = ""

        End If


    End Sub

    'Protected Sub btnUpdateInitialReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateInitialReport.Click

    '    oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
    '    '// Add cookie
    '    Response.Cookies.Add(oCookie)

    '    InsertInitialReport()

    '    AuditHelper.InsertReportUpdate(Request("IncidentID"), "Updated Initial Report", oCookie.Item("UserID"))



    '    Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID"))

    'End Sub

    Protected Sub btnUpdateReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateReport.Click

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        'InsertUpdateReport()
        ''txtReportUpdate.Text

        'AuditHelper.InsertReportUpdate(Request("IncidentID"), "Added Most Recent Update", oCookie.Item("UserID"))

        'AuditHelper.UpdateIncidentLastUpdated(Request("IncidentID"), oCookie.Item("UserID"))

        'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID"))

        MrEmail.SendUpdateEmail("Incident # " & lblIncidentNumber.Text & " updated", "<b>Date:</b> " & Format(Now, "MM-dd-yyyy") & "-" & Format(Now, "HH:mm") & " -- <b>Name:</b> " & ns.FullName & " -- <b>Update:</b> " & txtReportUpdate.Text)

    End Sub

    'Save Functions
    Protected Sub SaveInitialReport()

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        Dim localStrLat As String = lblLatDecimalDegrees.Text
        Dim localStrLong As String = lblLongDecimalDegrees.Text
        Dim localStrUSNG As String = lblUSNG.Text

        If localStrLat = "" Then
            localStrLat = "0.0"
        End If

        If localStrLong = "" Then
            localStrLong = "0.0"
        End If

        If localStrUSNG = "" Then
            localStrUSNG = "N/A"
        End If

        localStrLat = Replace(localStrLat, " ", "")
        localStrLat = Replace(localStrLat, ",", "")
        localStrLong = Replace(localStrLong, " ", "")
        localStrUSNG = Replace(localStrUSNG, " ", "")


        Dim localLat As Decimal = CDec(localStrLat)
        Dim localLong As Decimal = CDec(localStrLong)

        'Response.Write("County Count: " & MrDataGrabber.GrabCountyCounty(lblAffectedCounties.Text))
        'Response.Write("<br>")
        'Response.Write("Statewide: " & cbxStatewide.Checked)
        'Response.End()
        'Response.Write(localLat)
        'Response.Write("<br>")
        'Response.Write(localLong)
        'Response.Write("<br>")
        'Response.Write(localStrUSNG)
        'Response.Write("<br>")
        'Response.End()

        'Try
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        '// Enter the email and password to query/command object.
        objCmd = New SqlCommand("spUpdateIncidentInitialReport", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@CreatedByID", ns.UserID) 'oCookie.Item("UserID"))
        objCmd.Parameters.AddWithValue("@LastUpdatedByID", ns.UserID) 'oCookie.Item("UserID"))
        objCmd.Parameters.AddWithValue("@DateCreated", Now)
        objCmd.Parameters.AddWithValue("@LastUpdated", Now)
        objCmd.Parameters.AddWithValue("@IncidentName", txtIncidentName.Text)
        objCmd.Parameters.AddWithValue("@IncidentStatusID", ddlIncidentStatus.SelectedValue)
        objCmd.Parameters.AddWithValue("@IsThisADrill", ddlIsThisADrill.SelectedValue)
        objCmd.Parameters.AddWithValue("@StateAssistance", ddlStateAssistance.SelectedValue)
        objCmd.Parameters.AddWithValue("@ReportingPartyTypeID", ddlReportingPartyType.SelectedValue)
        objCmd.Parameters.AddWithValue("@OnSceneContactTypeID", ddlOnSceneContactType.SelectedValue)
        objCmd.Parameters.AddWithValue("@ResponsiblePartyTypeID", ddlResponsiblePartyType.SelectedValue)

        'Response.Write(CStr(txtIncidentOccurredTime.Text.Trim) & CStr(txtIncidentOccurredTime2.Text.Trim))
        'Response.End()

        objCmd.Parameters.AddWithValue("@ReportedToSWOTime", CStr(txtReportedToSWOTime.Text.Trim) & CStr(txtReportedToSWOTime2.Text.Trim))
        objCmd.Parameters.AddWithValue("@ReportedToSWODate", txtReportedToSWODate.Text)
        objCmd.Parameters.AddWithValue("@IncidentOccurredTime", CStr(txtIncidentOccurredTime.Text.Trim) & CStr(txtIncidentOccurredTime2.Text.Trim))
        objCmd.Parameters.AddWithValue("@IncidentOccurredDate", txtIncidentOccurredDate.Text)

        objCmd.Parameters.AddWithValue("@FacilityNameSceneDescription", txtFacilityNameSceneDescription.Text)
        objCmd.Parameters.AddWithValue("@Address", txtAddress.Text)
        objCmd.Parameters.AddWithValue("@City", txtCity.Text)
        objCmd.Parameters.AddWithValue("@Address2", txtAddress2.Text)
        objCmd.Parameters.AddWithValue("@Zip", txtZip.Text)
        objCmd.Parameters.AddWithValue("@Street", txtStreet.Text)
        objCmd.Parameters.AddWithValue("@Street2", txtStreet2.Text)
        objCmd.Parameters.AddWithValue("@City2", txtCity2.Text)

        objCmd.Parameters.AddWithValue("@Lat", localLat)
        objCmd.Parameters.AddWithValue("@Long", localLong)
        objCmd.Parameters.AddWithValue("@USNG", localStrUSNG)

        'Picking the ObtainCoordinate Value
        If rdoFacilityNameSceneDescription.Checked = True Then
            objCmd.Parameters.AddWithValue("@ObtainCoordinate", "FacilityNameSceneDescription")
        ElseIf rdoAddressCity.Checked = True Then
            objCmd.Parameters.AddWithValue("@ObtainCoordinate", "AddressCity")
        ElseIf rdoByAddressZip.Checked = True Then
            objCmd.Parameters.AddWithValue("@ObtainCoordinate", "AddressZip")
        ElseIf rdoByIntersection.Checked = True Then
            objCmd.Parameters.AddWithValue("@ObtainCoordinate", "Intersection")
        ElseIf rdoAffectedCounties.Checked = True Then
            objCmd.Parameters.AddWithValue("@ObtainCoordinate", "AffectedCounties")
        ElseIf rdoByCoordinateEntry.Checked = True Then
            objCmd.Parameters.AddWithValue("@ObtainCoordinate", "CoordinateEntry")
        Else
            'This Should Never Happen but we will account for it
            objCmd.Parameters.AddWithValue("@ObtainCoordinate", "N/A")
        End If


        'Picking the CoordinateType Value
        If rdoDecimalDegrees.Checked = True Then
            objCmd.Parameters.AddWithValue("@CoordinateType", "DecimalDegrees")
        ElseIf rdoDegreesMinutes.Checked = True Then
            objCmd.Parameters.AddWithValue("@CoordinateType", "DegreesMinutes")
        ElseIf rdoDegreesMinutesSeconds.Checked = True Then
            objCmd.Parameters.AddWithValue("@CoordinateType", "DegreesMinutesSeconds")
        ElseIf rdoUSNG.Checked = True Then
            objCmd.Parameters.AddWithValue("@CoordinateType", "USNG")
        Else
            objCmd.Parameters.AddWithValue("@CoordinateType", "N/A")
        End If

        objCmd.Parameters.AddWithValue("@SeverityID", ddlSeverity.SelectedValue)
        objCmd.Parameters.AddWithValue("@AddedCounty", lblAffectedCounties.Text)

        If MrDataGrabber.GrabCountyCounty(lblAffectedCounties.Text) = CInt(System.Configuration.ConfigurationManager.AppSettings("NumberOfFloridaCounties").ToString) - 1 _
            Or cbxStatewide.Checked = True Then
            objCmd.Parameters.AddWithValue("@Statewide", "Statewide")
        Else
            objCmd.Parameters.AddWithValue("@Statewide", "No")
        End If

        DBConStringHelper.PrepareConnection(objConn)

        objCmd.ExecuteNonQuery()

        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)


        AuditHelper.InsertReportUpdate(Request("IncidentID"), "Incident Created", ns.UserID) 'oCookie.Item("UserID"))

        'Catch ex As Exception
        '    Response.Write(ex.ToString)

        '    Exit Sub
        'End Try

    End Sub

    Protected Sub SaveInitialReport2()

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        Dim localStrLat As String = lblLatDecimalDegrees.Text
        Dim localStrLong As String = lblLongDecimalDegrees.Text
        Dim localStrUSNG As String = lblUSNG.Text

        If localStrLat = "" Then
            localStrLat = "0.0"
        End If

        If localStrLong = "" Then
            localStrLong = "0.0"
        End If

        If localStrUSNG = "" Then
            localStrUSNG = "N/A"
        End If

        localStrLat = Replace(localStrLat, " ", "")
        localStrLat = Replace(localStrLat, ",", "")
        localStrLong = Replace(localStrLong, " ", "")
        localStrUSNG = Replace(localStrUSNG, " ", "")


        Dim localLat As Decimal = CDec(localStrLat)
        Dim localLong As Decimal = CDec(localStrLong)

        'Response.Write(localLat)
        'Response.Write("<br>")
        'Response.Write(localLong)
        'Response.Write("<br>")
        'Response.Write("hello")
        'Response.Write("<br>")
        'Response.End()

        'Try
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        '// Enter the email and password to query/command object.
        objCmd = New SqlCommand("spUpdateIncidentInitialReport2", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@CreatedByID", MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("CreatedByID", "Incident", "IncidentID", Request("IncidentID")))
        objCmd.Parameters.AddWithValue("@LastUpdatedByID", ns.UserID) 'oCookie.Item("UserID"))
        objCmd.Parameters.AddWithValue("@LastUpdated", Now)
        objCmd.Parameters.AddWithValue("@IncidentName", txtIncidentName.Text)
        objCmd.Parameters.AddWithValue("@IncidentStatusID", ddlIncidentStatus.SelectedValue)
        objCmd.Parameters.AddWithValue("@IsThisADrill", ddlIsThisADrill.SelectedValue)
        objCmd.Parameters.AddWithValue("@StateAssistance", ddlStateAssistance.SelectedValue)
        objCmd.Parameters.AddWithValue("@ReportingPartyTypeID", ddlReportingPartyType.SelectedValue)
        objCmd.Parameters.AddWithValue("@OnSceneContactTypeID", ddlOnSceneContactType.SelectedValue)
        objCmd.Parameters.AddWithValue("@ResponsiblePartyTypeID", ddlResponsiblePartyType.SelectedValue)
        objCmd.Parameters.AddWithValue("@ReportedToSWOTime", CStr(txtReportedToSWOTime.Text.Trim) & CStr(txtReportedToSWOTime2.Text.Trim))
        objCmd.Parameters.AddWithValue("@ReportedToSWODate", txtReportedToSWODate.Text)
        objCmd.Parameters.AddWithValue("@IncidentOccurredTime", CStr(txtIncidentOccurredTime.Text.Trim) & CStr(txtIncidentOccurredTime2.Text.Trim))
        objCmd.Parameters.AddWithValue("@IncidentOccurredDate", txtIncidentOccurredDate.Text)

        objCmd.Parameters.AddWithValue("@FacilityNameSceneDescription", txtFacilityNameSceneDescription.Text)
        objCmd.Parameters.AddWithValue("@Address", txtAddress.Text)
        objCmd.Parameters.AddWithValue("@City", txtCity.Text)
        objCmd.Parameters.AddWithValue("@Address2", txtAddress2.Text)
        objCmd.Parameters.AddWithValue("@Zip", txtZip.Text)
        objCmd.Parameters.AddWithValue("@Street", txtStreet.Text)
        objCmd.Parameters.AddWithValue("@Street2", txtStreet2.Text)
        objCmd.Parameters.AddWithValue("@City2", txtCity2.Text)

        objCmd.Parameters.AddWithValue("@Lat", localLat)
        objCmd.Parameters.AddWithValue("@Long", localLong)
        objCmd.Parameters.AddWithValue("@USNG", localStrUSNG)

        'Picking the ObtainCoordinate Value
        If rdoFacilityNameSceneDescription.Checked = True Then
            objCmd.Parameters.AddWithValue("@ObtainCoordinate", "FacilityNameSceneDescription")
        ElseIf rdoAddressCity.Checked = True Then
            objCmd.Parameters.AddWithValue("@ObtainCoordinate", "AddressCity")
        ElseIf rdoByAddressZip.Checked = True Then
            objCmd.Parameters.AddWithValue("@ObtainCoordinate", "AddressZip")
        ElseIf rdoByIntersection.Checked = True Then
            objCmd.Parameters.AddWithValue("@ObtainCoordinate", "Intersection")
        ElseIf rdoAffectedCounties.Checked = True Then
            objCmd.Parameters.AddWithValue("@ObtainCoordinate", "AffectedCounties")
        ElseIf rdoByCoordinateEntry.Checked = True Then
            objCmd.Parameters.AddWithValue("@ObtainCoordinate", "CoordinateEntry")
        Else
            'This Should Never Happen but we will account for it
            objCmd.Parameters.AddWithValue("@ObtainCoordinate", "N/A")
        End If


        'Picking the CoordinateType Value
        If rdoDecimalDegrees.Checked = True Then
            objCmd.Parameters.AddWithValue("@CoordinateType", "DecimalDegrees")
        ElseIf rdoDegreesMinutes.Checked = True Then
            objCmd.Parameters.AddWithValue("@CoordinateType", "DegreesMinutes")
        ElseIf rdoDegreesMinutesSeconds.Checked = True Then
            objCmd.Parameters.AddWithValue("@CoordinateType", "DegreesMinutesSeconds")
        ElseIf rdoUSNG.Checked = True Then
            objCmd.Parameters.AddWithValue("@CoordinateType", "USNG")
        Else
            objCmd.Parameters.AddWithValue("@CoordinateType", "N/A")
        End If

        objCmd.Parameters.AddWithValue("@SeverityID", ddlSeverity.SelectedValue)
        objCmd.Parameters.AddWithValue("@AddedCounty", lblAffectedCounties.Text)

        If MrDataGrabber.GrabCountyCounty(lblAffectedCounties.Text) = CInt(System.Configuration.ConfigurationManager.AppSettings("NumberOfFloridaCounties").ToString) - 1 _
            Or cbxStatewide.Checked = True Then
            objCmd.Parameters.AddWithValue("@Statewide", "Statewide")
        Else
            objCmd.Parameters.AddWithValue("@Statewide", "No")
        End If

        DBConStringHelper.PrepareConnection(objConn)

        objCmd.ExecuteNonQuery()

        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)

        AuditHelper.InsertReportUpdate(Request("IncidentID"), "Edited Main Incident", ns.UserID) 'oCookie.Item("UserID"))

        'Catch ex As Exception
        '    Response.Write(ex.ToString)

        '    Exit Sub
        'End Try

    End Sub

    Protected Sub SaveInitialReportingParty()
        Dim localReportingPartyCount As Integer = 0
        'Populating Grids
        Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectReportingPartyCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                localReportingPartyCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()


        Catch ex As Exception

            Response.Write(ex.ToString)
            Exit Sub

        End Try


        If localReportingPartyCount = 0 Then

            'Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionReportingParty", objConn)
            objCmd.CommandType = CommandType.StoredProcedure

            objCmd.Parameters.AddWithValue("@ReportingPartyID", 0)
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@FirstName", txtReportingPartyFirstName.Text)
            objCmd.Parameters.AddWithValue("@LastName", txtReportingPartyLastName.Text)
            objCmd.Parameters.AddWithValue("@CallBackNumber1", txtReportingPartyCallBackNumber1.Text)
            objCmd.Parameters.AddWithValue("@CallBackNumber2", txtReportingPartyCallBackNumber2.Text)
            objCmd.Parameters.AddWithValue("@Email", txtReportingPartyEmail.Text)
            objCmd.Parameters.AddWithValue("@Address", txtReportingPartyAddress.Text)
            objCmd.Parameters.AddWithValue("@City", txtReportingPartyCity.Text)
            objCmd.Parameters.AddWithValue("@State", txtReportingPartyState.Text)
            objCmd.Parameters.AddWithValue("@Zipcode", txtReportingPartyZipcode.Text)
            objCmd.Parameters.AddWithValue("@Represents", txtReportingPartyRepresents.Text)

            'objCmd.Parameters.AddWithValue("@IncidentName", txtIncidentName.Text)

            'txtIncidentName


            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

        Else

            'Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionReportingParty", objConn)
            objCmd.CommandType = CommandType.StoredProcedure

            objCmd.Parameters.AddWithValue("@ReportingPartyID", 1)
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@FirstName", txtReportingPartyFirstName.Text)
            objCmd.Parameters.AddWithValue("@LastName", txtReportingPartyLastName.Text)
            objCmd.Parameters.AddWithValue("@CallBackNumber1", txtReportingPartyCallBackNumber1.Text)
            objCmd.Parameters.AddWithValue("@CallBackNumber2", txtReportingPartyCallBackNumber2.Text)
            objCmd.Parameters.AddWithValue("@Email", txtReportingPartyEmail.Text)
            objCmd.Parameters.AddWithValue("@Address", txtReportingPartyAddress.Text)
            objCmd.Parameters.AddWithValue("@City", txtReportingPartyCity.Text)
            objCmd.Parameters.AddWithValue("@State", txtReportingPartyState.Text)
            objCmd.Parameters.AddWithValue("@Zipcode", txtReportingPartyZipcode.Text)
            objCmd.Parameters.AddWithValue("@Represents", txtReportingPartyRepresents.Text)

            'objCmd.Parameters.AddWithValue("@IncidentName", txtIncidentName.Text)

            'txtIncidentName


            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

        End If


        'Response.Write("All Good2")
        'Response.End()

        'Catch ex As Exception
        '    Response.Write(ex.ToString)

        '    Exit Sub
        'End Try

    End Sub

    Protected Sub SaveInitialOnSceneContact()


        Dim localOnSceneContactCount As Integer = 0
        'Populating Grids
        Try


            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectOnSceneContactCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                localOnSceneContactCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()


        Catch ex As Exception

            Response.Write(ex.ToString)
            Exit Sub

        End Try

        If localOnSceneContactCount = 0 Then

            'Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionOnSceneContact", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@OnSceneContactID", 0)
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@FirstName", txtOnSceneContactFirstName.Text)
            objCmd.Parameters.AddWithValue("@LastName", txtOnSceneContactLastName.Text)
            objCmd.Parameters.AddWithValue("@CallBackNumber1", txtOnSceneContactPhone1.Text)
            objCmd.Parameters.AddWithValue("@CallBackNumber2", txtOnSceneContactPhone2.Text)
            objCmd.Parameters.AddWithValue("@Email", txtOnSceneContactEmail.Text)
            objCmd.Parameters.AddWithValue("@Address", txtOnSceneContactAddress.Text)
            objCmd.Parameters.AddWithValue("@City", txtOnSceneContactCity.Text)
            objCmd.Parameters.AddWithValue("@State", txtOnSceneContactState.Text)
            objCmd.Parameters.AddWithValue("@Zipcode", txtOnSceneContactZipcode.Text)
            objCmd.Parameters.AddWithValue("@Represents", txtOnSceneContactRepresents.Text)

            'objCmd.Parameters.AddWithValue("@IncidentName", txtIncidentName.Text)

            'txtIncidentName


            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

        Else

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionOnSceneContact", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@OnSceneContactID", 1)
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@FirstName", txtOnSceneContactFirstName.Text)
            objCmd.Parameters.AddWithValue("@LastName", txtOnSceneContactLastName.Text)
            objCmd.Parameters.AddWithValue("@CallBackNumber1", txtOnSceneContactPhone1.Text)
            objCmd.Parameters.AddWithValue("@CallBackNumber2", txtOnSceneContactPhone2.Text)
            objCmd.Parameters.AddWithValue("@Email", txtOnSceneContactEmail.Text)
            objCmd.Parameters.AddWithValue("@Address", txtOnSceneContactAddress.Text)
            objCmd.Parameters.AddWithValue("@City", txtOnSceneContactCity.Text)
            objCmd.Parameters.AddWithValue("@State", txtOnSceneContactState.Text)
            objCmd.Parameters.AddWithValue("@Zipcode", txtOnSceneContactZipcode.Text)
            objCmd.Parameters.AddWithValue("@Represents", txtOnSceneContactRepresents.Text)

            'objCmd.Parameters.AddWithValue("@IncidentName", txtIncidentName.Text)

            'txtIncidentName


            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

        End If


        'Response.Write("All Good2")
        'Response.End()

        'Catch ex As Exception
        '    Response.Write(ex.ToString)

        '    Exit Sub
        'End Try

    End Sub

    Protected Sub SaveInitialResponsibleParty()


        Dim localResponsiblePartyCount As Integer = 0
        'Populating Grids
        Try


            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectResponsiblePartyCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                localResponsiblePartyCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()


        Catch ex As Exception

            Response.Write(ex.ToString)
            Exit Sub

        End Try

        If localResponsiblePartyCount = 0 Then

            'Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionResponsibleParty", objConn)
            objCmd.CommandType = CommandType.StoredProcedure

            objCmd.Parameters.AddWithValue("@ResponsiblePartyID", 0)
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@FirstName", txtResponsiblePartyFirstName.Text)
            objCmd.Parameters.AddWithValue("@LastName", txtResponsiblePartyLastName.Text)
            objCmd.Parameters.AddWithValue("@CallBackNumber1", txtResponsiblePartyPhone1.Text)
            objCmd.Parameters.AddWithValue("@CallBackNumber2", txtResponsiblePartyPhone2.Text)
            objCmd.Parameters.AddWithValue("@Email", txtResponsiblePartyEmail.Text)
            objCmd.Parameters.AddWithValue("@Address", txtResponsiblePartyAddress.Text)
            objCmd.Parameters.AddWithValue("@City", txtResponsiblePartyCity.Text)
            objCmd.Parameters.AddWithValue("@State", txtResponsiblePartyState.Text)
            objCmd.Parameters.AddWithValue("@Zipcode", txtResponsiblePartyZipcode.Text)
            objCmd.Parameters.AddWithValue("@Represents", txtResponsiblePartyRepresents.Text)

            'objCmd.Parameters.AddWithValue("@IncidentName", txtIncidentName.Text)

            'txtIncidentName


            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            'Response.Write("All Good2")
            'Response.End()

            'Catch ex As Exception
            '    Response.Write(ex.ToString)

            '    Exit Sub
            'End Try

        Else

            'Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionResponsibleParty", objConn)
            objCmd.CommandType = CommandType.StoredProcedure

            objCmd.Parameters.AddWithValue("@ResponsiblePartyID", 1)
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@FirstName", txtResponsiblePartyFirstName.Text)
            objCmd.Parameters.AddWithValue("@LastName", txtResponsiblePartyLastName.Text)
            objCmd.Parameters.AddWithValue("@CallBackNumber1", txtResponsiblePartyPhone1.Text)
            objCmd.Parameters.AddWithValue("@CallBackNumber2", txtResponsiblePartyPhone2.Text)
            objCmd.Parameters.AddWithValue("@Email", txtResponsiblePartyEmail.Text)
            objCmd.Parameters.AddWithValue("@Address", txtResponsiblePartyAddress.Text)
            objCmd.Parameters.AddWithValue("@City", txtResponsiblePartyCity.Text)
            objCmd.Parameters.AddWithValue("@State", txtResponsiblePartyState.Text)
            objCmd.Parameters.AddWithValue("@Zipcode", txtResponsiblePartyZipcode.Text)
            objCmd.Parameters.AddWithValue("@Represents", txtResponsiblePartyRepresents.Text)

            'objCmd.Parameters.AddWithValue("@IncidentName", txtIncidentName.Text)

            'txtIncidentName


            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            'Response.Write("All Good2")
            'Response.End()

            'Catch ex As Exception
            '    Response.Write(ex.ToString)

            '    Exit Sub
            'End Try

        End If



    End Sub







    Protected Sub UpdateIncident()

    End Sub



    Protected Sub GrabInitialReport()

        'Response.Write("Hello")
        'Response.End()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectLastInitialReportByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            txtInitialReport.Text = HelpFunction.Convertdbnulls(objDR("InitialReport"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

    End Sub

    Protected Sub GrabReportUpdate()

        'Response.Write("Hello")
        'Response.End()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectLastUpdateReportByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            lblLatestUpdate.Text = HelpFunction.Convertdbnulls(objDR("UpdateReport"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

    End Sub


    Protected Sub InsertInitialReport()


        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        '// Enter the email and password to query/command object.
        objCmd = New SqlCommand("spInsertInitialReport", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@InitialReport", txtInitialReport.Text)
        objCmd.Parameters.AddWithValue("@UpdateDate", Now)
        objCmd.Parameters.AddWithValue("@UserName", ns.FullName)

        DBConStringHelper.PrepareConnection(objConn)

        objCmd.ExecuteNonQuery()

        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)

        'Catch ex As Exception
        '    Response.Write(ex.ToString)

        '    Exit Sub
        'End Try

    End Sub

    Protected Sub InsertUpdateReport()

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        'Try
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        '// Enter the email and password to query/command object.
        objCmd = New SqlCommand("spInsertUpdateReport", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@InitialReport", txtReportUpdate.Text)
        objCmd.Parameters.AddWithValue("@UpdateDate", Now)
        objCmd.Parameters.AddWithValue("@UserName", ns.FullName)
        objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))

        DBConStringHelper.PrepareConnection(objConn)

        objCmd.ExecuteNonQuery()

        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)

        'Catch ex As Exception
        '    Response.Write(ex.ToString)

        '    Exit Sub
        'End Try

    End Sub

    Protected Sub lnkAddAffectedCounty_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAddAffectedCounty.Click

        'Response.Redirect("AddRegionCounty.aspx?IncidentID=" & Request("IncidentID"))

        pnlShowCountyGrabber.Visible = True

    End Sub








    Protected Sub SaveCounties()

        Dim Region1Affected As Boolean = False
        Dim Region2Affected As Boolean = False
        Dim Region3Affected As Boolean = False
        Dim Region4Affected As Boolean = False
        Dim Region5Affected As Boolean = False
        Dim Region6Affected As Boolean = False
        Dim Region7Affected As Boolean = False

        Dim Region1 As Boolean = False
        Dim Region2 As Boolean = False
        Dim Region3 As Boolean = False
        Dim Region4 As Boolean = False
        Dim Region5 As Boolean = False
        Dim Region6 As Boolean = False
        Dim Region7 As Boolean = False
        Dim Statewide As Boolean = False

        Dim Region1Count As Integer = 0
        Dim Region2Count As Integer = 0
        Dim Region3Count As Integer = 0
        Dim Region4Count As Integer = 0
        Dim Region5Count As Integer = 0
        Dim Region6Count As Integer = 0
        Dim Region7Count As Integer = 0
        Dim StatewideCount As Integer = 0


        Try

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectCountyRegionCheckCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                globalRecordCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

        Catch ex As Exception

            Response.Write(ex.ToString)
            Exit Sub

        End Try

        'Response.Write(globalRecordCount)
        'Response.End()

        ' Region 1 Start
        If cbxBay.Checked = True Then
            Region1Affected = True
            Region1Count = Region1Count + 1
        End If

        If cbxCalhoun.Checked = True Then
            Region1Affected = True
            Region1Count = Region1Count + 1
        End If

        If cbxEscambia.Checked = True Then
            Region1Affected = True
            Region1Count = Region1Count + 1
        End If

        If cbxGulf.Checked = True Then
            Region1Affected = True
            Region1Count = Region1Count + 1
        End If

        If cbxHolmes.Checked = True Then
            Region1Affected = True
            Region1Count = Region1Count + 1
        End If

        If cbxJackson.Checked = True Then
            Region1Affected = True
            Region1Count = Region1Count + 1
        End If

        If cbxOkaloosa.Checked = True Then
            Region1Affected = True
            Region1Count = Region1Count + 1
        End If

        If cbxSantaRosa.Checked = True Then
            Region1Affected = True
            Region1Count = Region1Count + 1
        End If

        If cbxWalton.Checked = True Then
            Region1Affected = True
            Region1Count = Region1Count + 1
        End If

        If cbxWashington.Checked = True Then
            Region1Affected = True
            Region1Count = Region1Count + 1
        End If

        If Region1Count = 10 Then
            Region1 = True
            StatewideCount = StatewideCount + 1
            cbxRegion1.Checked = True
        End If
        ' Region 1 End


        ' Region 2 Start
        If cbxColumbia.Checked = True Then
            Region2Affected = True
            Region2Count = Region2Count + 1
        End If

        If cbxDixie.Checked = True Then
            Region2Affected = True
            Region2Count = Region2Count + 1
        End If

        If cbxFranklin.Checked = True Then
            Region2Affected = True
            Region2Count = Region2Count + 1
        End If

        If cbxGadsden.Checked = True Then
            Region2Affected = True
            Region2Count = Region2Count + 1
        End If

        If cbxHamilton.Checked = True Then
            Region2Affected = True
            Region2Count = Region2Count + 1
        End If

        If cbxJefferson.Checked = True Then
            Region2Affected = True
            Region2Count = Region2Count + 1
        End If

        If cbxLafayette.Checked = True Then
            Region2Affected = True
            Region2Count = Region2Count + 1
        End If

        If cbxLeon.Checked = True Then
            Region2Affected = True
            Region2Count = Region2Count + 1
        End If

        If cbxLiberty.Checked = True Then
            Region2Affected = True
            Region2Count = Region2Count + 1
        End If

        If cbxMadison.Checked = True Then
            Region2Affected = True
            Region2Count = Region2Count + 1
        End If

        If cbxSuwannee.Checked = True Then
            Region2Affected = True
            Region2Count = Region2Count + 1
        End If

        If cbxTaylor.Checked = True Then
            Region2Affected = True
            Region2Count = Region2Count + 1
        End If

        If cbxWakulla.Checked = True Then
            Region2Affected = True
            Region2Count = Region2Count + 1
        End If

        If Region2Count = 13 Then
            Region2 = True
            StatewideCount = StatewideCount + 1
            cbxRegion2.Checked = True
        End If
        ' Region 2 End


        ' Region 3 Start
        If cbxAlachua.Checked = True Then
            Region3Affected = True
            Region3Count = Region3Count + 1
        End If

        If cbxBaker.Checked = True Then
            Region3Affected = True
            Region3Count = Region3Count + 1
        End If

        If cbxBradford.Checked = True Then
            Region3Affected = True
            Region3Count = Region3Count + 1
        End If

        If cbxClay.Checked = True Then
            Region3Affected = True
            Region3Count = Region3Count + 1
        End If

        If cbxDuval.Checked = True Then
            Region3Affected = True
            Region3Count = Region3Count + 1
        End If

        If cbxFlagler.Checked = True Then
            Region3Affected = True
            Region3Count = Region3Count + 1
        End If

        If cbxGilchrist.Checked = True Then
            Region3Affected = True
            Region3Count = Region3Count + 1
        End If

        If cbxLevy.Checked = True Then
            Region3Affected = True
            Region3Count = Region3Count + 1
        End If

        If cbxMarion.Checked = True Then
            Region3Affected = True
            Region3Count = Region3Count + 1
        End If

        If cbxNassau.Checked = True Then
            Region3Affected = True
            Region3Count = Region3Count + 1
        End If

        If cbxPutnam.Checked = True Then
            Region3Affected = True
            Region3Count = Region3Count + 1
        End If

        If cbxStJohns.Checked = True Then
            Region3Affected = True
            Region3Count = Region3Count + 1
        End If

        If cbxUnion.Checked = True Then
            Region3Affected = True
            Region3Count = Region3Count + 1
        End If

        If Region3Count = 13 Then
            Region3 = True
            StatewideCount = StatewideCount + 1
            cbxRegion3.Checked = True
        End If
        ' Region 3 End


        ' Region 4 Start
        If cbxCitrus.Checked = True Then
            Region4Affected = True
            Region4Count = Region4Count + 1
        End If

        If cbxHardee.Checked = True Then
            Region4Affected = True
            Region4Count = Region4Count + 1
        End If

        If cbxHernando.Checked = True Then
            Region4Affected = True
            Region4Count = Region4Count + 1
        End If

        If cbxHillsborough.Checked = True Then
            Region4Affected = True
            Region4Count = Region4Count + 1
        End If

        If cbxPasco.Checked = True Then
            Region4Affected = True
            Region4Count = Region4Count + 1
        End If

        If cbxPinellas.Checked = True Then
            Region4Affected = True
            Region4Count = Region4Count + 1
        End If

        If cbxPolk.Checked = True Then
            Region4Affected = True
            Region4Count = Region4Count + 1
        End If

        If cbxSumter.Checked = True Then
            Region4Affected = True
            Region4Count = Region4Count + 1
        End If

        If Region4Count = 8 Then
            Region4 = True
            StatewideCount = StatewideCount + 1
            cbxRegion4.Checked = True
        End If
        ' Region 4 End


        ' Region 5 Start
        If cbxBrevard.Checked = True Then
            Region5Affected = True
            Region5Count = Region5Count + 1
        End If

        If cbxIndianRiver.Checked = True Then
            Region5Affected = True
            Region5Count = Region5Count + 1
        End If

        If cbxLake.Checked = True Then
            Region5Affected = True
            Region5Count = Region5Count + 1
        End If

        If cbxMartin.Checked = True Then
            Region5Affected = True
            Region5Count = Region5Count + 1
        End If

        If cbxOrange.Checked = True Then
            Region5Affected = True
            Region5Count = Region5Count + 1
        End If

        If cbxOsceola.Checked = True Then
            Region5Affected = True
            Region5Count = Region5Count + 1
        End If

        If cbxSeminole.Checked = True Then
            Region5Affected = True
            Region5Count = Region5Count + 1
        End If

        If cbxStLucie.Checked = True Then
            Region5Affected = True
            Region5Count = Region5Count + 1
        End If

        If cbxVolusia.Checked = True Then
            Region5Affected = True
            Region5Count = Region5Count + 1
        End If

        If Region5Count = 9 Then
            Region5 = True
            StatewideCount = StatewideCount + 1
            cbxRegion5.Checked = True
        End If
        ' Region 5 End


        ' Region 6 Start
        If cbxCharlotte.Checked = True Then
            Region6Affected = True
            Region6Count = Region6Count + 1
        End If

        If cbxCollier.Checked = True Then
            Region6Affected = True
            Region6Count = Region6Count + 1
        End If

        If cbxDeSoto.Checked = True Then
            Region6Affected = True
            Region6Count = Region6Count + 1
        End If

        If cbxGlades.Checked = True Then
            Region6Affected = True
            Region6Count = Region6Count + 1
        End If

        If cbxHendry.Checked = True Then
            Region6Affected = True
            Region6Count = Region6Count + 1
        End If

        If cbxHighlands.Checked = True Then
            Region6Affected = True
            Region6Count = Region6Count + 1
        End If

        If cbxLee.Checked = True Then
            Region6Affected = True
            Region6Count = Region6Count + 1
        End If

        If cbxManatee.Checked = True Then
            Region6Affected = True
            Region6Count = Region6Count + 1
        End If

        If cbxOkeechobee.Checked = True Then
            Region6Affected = True
            Region6Count = Region6Count + 1
        End If

        If Region6Count = 9 Then
            Region6 = True
            StatewideCount = StatewideCount + 1
            cbxRegion6.Checked = True
        End If

        If cbxSarasota.Checked = True Then
            Region6Affected = True
            Region6Count = Region6Count + 1
        End If
        ' Region 6 End


        ' Region 7 Start
        If cbxBroward.Checked = True Then
            Region7Affected = True
            Region7Count = Region7Count + 1
        End If

        If cbxMiamiDade.Checked = True Then
            Region7Affected = True
            Region7Count = Region7Count + 1
        End If

        If cbxMonroe.Checked = True Then
            Region7Affected = True
            Region7Count = Region7Count + 1
        End If

        If cbxPalmBeach.Checked = True Then
            Region7Affected = True
            Region7Count = Region7Count + 1
        End If

        If Region7Count = 5 Then
            Region7 = True
            StatewideCount = StatewideCount + 1
            cbxRegion7.Checked = True
        End If
        ' Region 7 End

        If StatewideCount = 7 Then
            cbxStatewide.Checked = True
        End If

        If globalRecordCount = 0 Then

            'We Add

            'If cbx

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionCountyRegionCheck", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@CountyRegionCheckID", 0)
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@StateWide", cbxStatewide.Checked)
            objCmd.Parameters.AddWithValue("@Region1", cbxRegion1.Checked)
            objCmd.Parameters.AddWithValue("@Region2", cbxRegion2.Checked)
            objCmd.Parameters.AddWithValue("@Region3", cbxRegion3.Checked)
            objCmd.Parameters.AddWithValue("@Region4", cbxRegion4.Checked)
            objCmd.Parameters.AddWithValue("@Region5", cbxRegion5.Checked)
            objCmd.Parameters.AddWithValue("@Region6", cbxRegion6.Checked)
            objCmd.Parameters.AddWithValue("@Region7", cbxRegion7.Checked)
            objCmd.Parameters.AddWithValue("@Bay", cbxBay.Checked)
            objCmd.Parameters.AddWithValue("@Calhoun", cbxCalhoun.Checked)
            objCmd.Parameters.AddWithValue("@Escambia", cbxEscambia.Checked)
            objCmd.Parameters.AddWithValue("@Gulf", cbxGulf.Checked)
            objCmd.Parameters.AddWithValue("@Holmes", cbxHolmes.Checked)
            objCmd.Parameters.AddWithValue("@Jackson", cbxJackson.Checked)
            objCmd.Parameters.AddWithValue("@Okaloosa", cbxOkaloosa.Checked)
            objCmd.Parameters.AddWithValue("@SantaRosa", cbxSantaRosa.Checked)
            objCmd.Parameters.AddWithValue("@Walton", cbxWalton.Checked)
            objCmd.Parameters.AddWithValue("@Washington", cbxWashington.Checked)
            objCmd.Parameters.AddWithValue("@Columbia", cbxColumbia.Checked)
            objCmd.Parameters.AddWithValue("@Dixie", cbxDixie.Checked)
            objCmd.Parameters.AddWithValue("@Franklin", cbxFranklin.Checked)
            objCmd.Parameters.AddWithValue("@Gadsden", cbxGadsden.Checked)
            objCmd.Parameters.AddWithValue("@Hamilton", cbxHamilton.Checked)
            objCmd.Parameters.AddWithValue("@Jefferson", cbxJefferson.Checked)
            objCmd.Parameters.AddWithValue("@Lafayette", cbxLafayette.Checked)
            objCmd.Parameters.AddWithValue("@Leon", cbxLeon.Checked)
            objCmd.Parameters.AddWithValue("@Levy", cbxLevy.Checked)
            objCmd.Parameters.AddWithValue("@Liberty", cbxLiberty.Checked)
            objCmd.Parameters.AddWithValue("@Madison", cbxMadison.Checked)
            objCmd.Parameters.AddWithValue("@Suwannee", cbxSuwannee.Checked)
            objCmd.Parameters.AddWithValue("@Taylor", cbxTaylor.Checked)
            objCmd.Parameters.AddWithValue("@Wakulla", cbxWakulla.Checked)
            objCmd.Parameters.AddWithValue("@Alachua", cbxAlachua.Checked)
            objCmd.Parameters.AddWithValue("@Baker", cbxBaker.Checked)
            objCmd.Parameters.AddWithValue("@Bradford", cbxBradford.Checked)
            objCmd.Parameters.AddWithValue("@Clay", cbxClay.Checked)
            objCmd.Parameters.AddWithValue("@Duval", cbxDuval.Checked)
            objCmd.Parameters.AddWithValue("@Flagler", cbxFlagler.Checked)
            objCmd.Parameters.AddWithValue("@Gilchrist", cbxGilchrist.Checked)
            objCmd.Parameters.AddWithValue("@Marion", cbxMarion.Checked)
            objCmd.Parameters.AddWithValue("@Nassau", cbxNassau.Checked)
            objCmd.Parameters.AddWithValue("@Putnam", cbxPutnam.Checked)
            objCmd.Parameters.AddWithValue("@StJohns", cbxStJohns.Checked)
            objCmd.Parameters.AddWithValue("@Union", cbxUnion.Checked)
            objCmd.Parameters.AddWithValue("@Citrus", cbxCitrus.Checked)
            objCmd.Parameters.AddWithValue("@Hardee", cbxHardee.Checked)
            objCmd.Parameters.AddWithValue("@Hernando", cbxHernando.Checked)
            objCmd.Parameters.AddWithValue("@Hillsborough", cbxHillsborough.Checked)
            objCmd.Parameters.AddWithValue("@Pasco", cbxPasco.Checked)
            objCmd.Parameters.AddWithValue("@Pinellas", cbxPinellas.Checked)
            objCmd.Parameters.AddWithValue("@Polk", cbxPolk.Checked)
            objCmd.Parameters.AddWithValue("@Sumter", cbxSumter.Checked)
            objCmd.Parameters.AddWithValue("@Brevard", cbxBrevard.Checked)
            objCmd.Parameters.AddWithValue("@IndianRiver", cbxIndianRiver.Checked)
            objCmd.Parameters.AddWithValue("@Lake", cbxLake.Checked)
            objCmd.Parameters.AddWithValue("@Martin", cbxMartin.Checked)
            objCmd.Parameters.AddWithValue("@Orange", cbxOrange.Checked)
            objCmd.Parameters.AddWithValue("@Osceola", cbxOsceola.Checked)
            objCmd.Parameters.AddWithValue("@Seminole", cbxSeminole.Checked)
            objCmd.Parameters.AddWithValue("@StLucie", cbxStLucie.Checked)
            objCmd.Parameters.AddWithValue("@Volusia", cbxVolusia.Checked)
            objCmd.Parameters.AddWithValue("@Charlotte", cbxCharlotte.Checked)
            objCmd.Parameters.AddWithValue("@Collier", cbxCollier.Checked)
            objCmd.Parameters.AddWithValue("@DeSoto", cbxDeSoto.Checked)
            objCmd.Parameters.AddWithValue("@Glades", cbxGlades.Checked)
            objCmd.Parameters.AddWithValue("@Hendry", cbxHendry.Checked)
            objCmd.Parameters.AddWithValue("@Highlands", cbxHighlands.Checked)
            objCmd.Parameters.AddWithValue("@Lee", cbxLee.Checked)
            objCmd.Parameters.AddWithValue("@Manatee", cbxManatee.Checked)
            objCmd.Parameters.AddWithValue("@Okeechobee", cbxOkeechobee.Checked)
            objCmd.Parameters.AddWithValue("@Sarasota", cbxSarasota.Checked)
            objCmd.Parameters.AddWithValue("@Broward", cbxBroward.Checked)
            objCmd.Parameters.AddWithValue("@MiamiDade", cbxMiamiDade.Checked)
            objCmd.Parameters.AddWithValue("@Monroe", cbxMonroe.Checked)
            objCmd.Parameters.AddWithValue("@PalmBeach", cbxPalmBeach.Checked)
            objCmd.Parameters.AddWithValue("@Region1Affected", Region1Affected)
            objCmd.Parameters.AddWithValue("@Region2Affected", Region2Affected)
            objCmd.Parameters.AddWithValue("@Region3Affected", Region3Affected)
            objCmd.Parameters.AddWithValue("@Region4Affected", Region4Affected)
            objCmd.Parameters.AddWithValue("@Region5Affected", Region5Affected)
            objCmd.Parameters.AddWithValue("@Region6Affected", Region6Affected)
            objCmd.Parameters.AddWithValue("@Region7Affected", Region7Affected)


            '// Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            '// Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()
            '// Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)



        Else
            'We update


            Dim localCountyRegionCheckID As Integer = MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("CountyRegionCheckID", "CountyRegionCheck", "IncidentID", Request("IncidentID"))

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionCountyRegionCheck", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@CountyRegionCheckID", localCountyRegionCheckID)
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@StateWide", cbxStatewide.Checked)
            objCmd.Parameters.AddWithValue("@Region1", cbxRegion1.Checked)
            objCmd.Parameters.AddWithValue("@Region2", cbxRegion2.Checked)
            objCmd.Parameters.AddWithValue("@Region3", cbxRegion3.Checked)
            objCmd.Parameters.AddWithValue("@Region4", cbxRegion4.Checked)
            objCmd.Parameters.AddWithValue("@Region5", cbxRegion5.Checked)
            objCmd.Parameters.AddWithValue("@Region6", cbxRegion6.Checked)
            objCmd.Parameters.AddWithValue("@Region7", cbxRegion7.Checked)
            objCmd.Parameters.AddWithValue("@Bay", cbxBay.Checked)
            objCmd.Parameters.AddWithValue("@Calhoun", cbxCalhoun.Checked)
            objCmd.Parameters.AddWithValue("@Escambia", cbxEscambia.Checked)
            objCmd.Parameters.AddWithValue("@Gulf", cbxGulf.Checked)
            objCmd.Parameters.AddWithValue("@Holmes", cbxHolmes.Checked)
            objCmd.Parameters.AddWithValue("@Jackson", cbxJackson.Checked)
            objCmd.Parameters.AddWithValue("@Okaloosa", cbxOkaloosa.Checked)
            objCmd.Parameters.AddWithValue("@SantaRosa", cbxSantaRosa.Checked)
            objCmd.Parameters.AddWithValue("@Walton", cbxWalton.Checked)
            objCmd.Parameters.AddWithValue("@Washington", cbxWashington.Checked)
            objCmd.Parameters.AddWithValue("@Columbia", cbxColumbia.Checked)
            objCmd.Parameters.AddWithValue("@Dixie", cbxDixie.Checked)
            objCmd.Parameters.AddWithValue("@Franklin", cbxFranklin.Checked)
            objCmd.Parameters.AddWithValue("@Gadsden", cbxGadsden.Checked)
            objCmd.Parameters.AddWithValue("@Hamilton", cbxHamilton.Checked)
            objCmd.Parameters.AddWithValue("@Jefferson", cbxJefferson.Checked)
            objCmd.Parameters.AddWithValue("@Lafayette", cbxLafayette.Checked)
            objCmd.Parameters.AddWithValue("@Leon", cbxLeon.Checked)
            objCmd.Parameters.AddWithValue("@Levy", cbxLevy.Checked)
            objCmd.Parameters.AddWithValue("@Liberty", cbxLiberty.Checked)
            objCmd.Parameters.AddWithValue("@Madison", cbxMadison.Checked)
            objCmd.Parameters.AddWithValue("@Suwannee", cbxSuwannee.Checked)
            objCmd.Parameters.AddWithValue("@Taylor", cbxTaylor.Checked)
            objCmd.Parameters.AddWithValue("@Wakulla", cbxWakulla.Checked)
            objCmd.Parameters.AddWithValue("@Alachua", cbxAlachua.Checked)
            objCmd.Parameters.AddWithValue("@Baker", cbxBaker.Checked)
            objCmd.Parameters.AddWithValue("@Bradford", cbxBradford.Checked)
            objCmd.Parameters.AddWithValue("@Clay", cbxClay.Checked)
            objCmd.Parameters.AddWithValue("@Duval", cbxDuval.Checked)
            objCmd.Parameters.AddWithValue("@Flagler", cbxFlagler.Checked)
            objCmd.Parameters.AddWithValue("@Gilchrist", cbxGilchrist.Checked)
            objCmd.Parameters.AddWithValue("@Marion", cbxMarion.Checked)
            objCmd.Parameters.AddWithValue("@Nassau", cbxNassau.Checked)
            objCmd.Parameters.AddWithValue("@Putnam", cbxPutnam.Checked)
            objCmd.Parameters.AddWithValue("@StJohns", cbxStJohns.Checked)
            objCmd.Parameters.AddWithValue("@Union", cbxUnion.Checked)
            objCmd.Parameters.AddWithValue("@Citrus", cbxCitrus.Checked)
            objCmd.Parameters.AddWithValue("@Hardee", cbxHardee.Checked)
            objCmd.Parameters.AddWithValue("@Hernando", cbxHernando.Checked)
            objCmd.Parameters.AddWithValue("@Hillsborough", cbxHillsborough.Checked)
            objCmd.Parameters.AddWithValue("@Pasco", cbxPasco.Checked)
            objCmd.Parameters.AddWithValue("@Pinellas", cbxPinellas.Checked)
            objCmd.Parameters.AddWithValue("@Polk", cbxPolk.Checked)
            objCmd.Parameters.AddWithValue("@Sumter", cbxSumter.Checked)
            objCmd.Parameters.AddWithValue("@Brevard", cbxBrevard.Checked)
            objCmd.Parameters.AddWithValue("@IndianRiver", cbxIndianRiver.Checked)
            objCmd.Parameters.AddWithValue("@Lake", cbxLake.Checked)
            objCmd.Parameters.AddWithValue("@Martin", cbxMartin.Checked)
            objCmd.Parameters.AddWithValue("@Orange", cbxOrange.Checked)
            objCmd.Parameters.AddWithValue("@Osceola", cbxOsceola.Checked)
            objCmd.Parameters.AddWithValue("@Seminole", cbxSeminole.Checked)
            objCmd.Parameters.AddWithValue("@StLucie", cbxStLucie.Checked)
            objCmd.Parameters.AddWithValue("@Volusia", cbxVolusia.Checked)
            objCmd.Parameters.AddWithValue("@Charlotte", cbxCharlotte.Checked)
            objCmd.Parameters.AddWithValue("@Collier", cbxCollier.Checked)
            objCmd.Parameters.AddWithValue("@DeSoto", cbxDeSoto.Checked)
            objCmd.Parameters.AddWithValue("@Glades", cbxGlades.Checked)
            objCmd.Parameters.AddWithValue("@Hendry", cbxHendry.Checked)
            objCmd.Parameters.AddWithValue("@Highlands", cbxHighlands.Checked)
            objCmd.Parameters.AddWithValue("@Lee", cbxLee.Checked)
            objCmd.Parameters.AddWithValue("@Manatee", cbxManatee.Checked)
            objCmd.Parameters.AddWithValue("@Okeechobee", cbxOkeechobee.Checked)
            objCmd.Parameters.AddWithValue("@Sarasota", cbxSarasota.Checked)
            objCmd.Parameters.AddWithValue("@Broward", cbxBroward.Checked)
            objCmd.Parameters.AddWithValue("@MiamiDade", cbxMiamiDade.Checked)
            objCmd.Parameters.AddWithValue("@Monroe", cbxMonroe.Checked)
            objCmd.Parameters.AddWithValue("@PalmBeach", cbxPalmBeach.Checked)
            objCmd.Parameters.AddWithValue("@Region1Affected", Region1Affected)
            objCmd.Parameters.AddWithValue("@Region2Affected", Region2Affected)
            objCmd.Parameters.AddWithValue("@Region3Affected", Region3Affected)
            objCmd.Parameters.AddWithValue("@Region4Affected", Region4Affected)
            objCmd.Parameters.AddWithValue("@Region5Affected", Region5Affected)
            objCmd.Parameters.AddWithValue("@Region6Affected", Region6Affected)
            objCmd.Parameters.AddWithValue("@Region7Affected", Region7Affected)


            '// Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            '// Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()
            '// Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

        End If


    End Sub

    Protected Sub btnSaveCounties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveCounties.Click

        SaveCounties()
        PopulateCounties()
        getAffectedCounty()
        pnlShowCountyGrabber.Visible = False
        pnlShowAffectedCounties.Visible = True

    End Sub

    Protected Sub btnSaveCounties2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveCounties2.Click

        SaveCounties()
        PopulateCounties()
        getAffectedCounty()
        pnlShowCountyGrabber.Visible = False
        pnlShowAffectedCounties.Visible = True

    End Sub

    Protected Sub btnCancelCounties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelCounties.Click

        pnlShowCountyGrabber.Visible = False

    End Sub


    Protected Sub btnCancelsCounties2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelsCounties2.Click
        pnlShowCountyGrabber.Visible = False
    End Sub

    Protected Sub PopulateCounties()

        'County Grabber Starts Here

        Try

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectCountyRegionCheckCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                globalRecordCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

        Catch ex As Exception

            Response.Write(ex.ToString)
            Exit Sub

        End Try

        'Response.Write(globalRecordCount)
        'Response.End()

        If globalRecordCount <> 0 Then

            Try

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objConn.Open()
                objCmd = New SqlCommand("spSelectCountyRegionCheckByIncidentID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

                objDR = objCmd.ExecuteReader

                If objDR.Read() Then

                    'Response.Write(objDR("Statewide"))


                    cbxStatewide.Checked = HelpFunction.ConvertdbnullsBool(objDR("Statewide"))
                    cbxRegion1.Checked = HelpFunction.ConvertdbnullsBool(objDR("Region1"))
                    cbxRegion2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Region2"))
                    cbxRegion3.Checked = HelpFunction.ConvertdbnullsBool(objDR("Region3"))
                    cbxRegion4.Checked = HelpFunction.ConvertdbnullsBool(objDR("Region4"))
                    cbxRegion5.Checked = HelpFunction.ConvertdbnullsBool(objDR("Region5"))
                    cbxRegion6.Checked = HelpFunction.ConvertdbnullsBool(objDR("Region6"))
                    cbxRegion7.Checked = HelpFunction.ConvertdbnullsBool(objDR("Region7"))
                    cbxBay.Checked = HelpFunction.ConvertdbnullsBool(objDR("Bay"))
                    cbxCalhoun.Checked = HelpFunction.ConvertdbnullsBool(objDR("Calhoun"))
                    cbxEscambia.Checked = HelpFunction.ConvertdbnullsBool(objDR("Escambia"))
                    cbxGulf.Checked = HelpFunction.ConvertdbnullsBool(objDR("Gulf"))
                    cbxHolmes.Checked = HelpFunction.ConvertdbnullsBool(objDR("Holmes"))
                    cbxJackson.Checked = HelpFunction.ConvertdbnullsBool(objDR("Jackson"))
                    cbxOkaloosa.Checked = HelpFunction.ConvertdbnullsBool(objDR("Okaloosa"))
                    cbxSantaRosa.Checked = HelpFunction.ConvertdbnullsBool(objDR("Santa Rosa"))
                    cbxWalton.Checked = HelpFunction.ConvertdbnullsBool(objDR("Walton"))
                    cbxWashington.Checked = HelpFunction.ConvertdbnullsBool(objDR("Washington"))
                    cbxColumbia.Checked = HelpFunction.ConvertdbnullsBool(objDR("Columbia"))
                    cbxDixie.Checked = HelpFunction.ConvertdbnullsBool(objDR("Dixie"))
                    cbxFranklin.Checked = HelpFunction.ConvertdbnullsBool(objDR("Franklin"))
                    cbxGadsden.Checked = HelpFunction.ConvertdbnullsBool(objDR("Gadsden"))
                    cbxHamilton.Checked = HelpFunction.ConvertdbnullsBool(objDR("Hamilton"))
                    cbxJefferson.Checked = HelpFunction.ConvertdbnullsBool(objDR("Jefferson"))
                    cbxLafayette.Checked = HelpFunction.ConvertdbnullsBool(objDR("Lafayette"))
                    cbxLeon.Checked = HelpFunction.ConvertdbnullsBool(objDR("Leon"))
                    cbxLevy.Checked = HelpFunction.ConvertdbnullsBool(objDR("Levy"))
                    cbxLiberty.Checked = HelpFunction.ConvertdbnullsBool(objDR("Liberty"))
                    cbxMadison.Checked = HelpFunction.ConvertdbnullsBool(objDR("Madison"))
                    cbxSuwannee.Checked = HelpFunction.ConvertdbnullsBool(objDR("Suwannee"))
                    cbxTaylor.Checked = HelpFunction.ConvertdbnullsBool(objDR("Taylor"))
                    cbxWakulla.Checked = HelpFunction.ConvertdbnullsBool(objDR("Wakulla"))
                    cbxAlachua.Checked = HelpFunction.ConvertdbnullsBool(objDR("Alachua"))
                    cbxBaker.Checked = HelpFunction.ConvertdbnullsBool(objDR("Baker"))
                    cbxBradford.Checked = HelpFunction.ConvertdbnullsBool(objDR("Bradford"))
                    cbxClay.Checked = HelpFunction.ConvertdbnullsBool(objDR("Clay"))
                    cbxDuval.Checked = HelpFunction.ConvertdbnullsBool(objDR("Duval"))
                    cbxFlagler.Checked = HelpFunction.ConvertdbnullsBool(objDR("Flagler"))
                    cbxGilchrist.Checked = HelpFunction.ConvertdbnullsBool(objDR("Gilchrist"))
                    cbxMarion.Checked = HelpFunction.ConvertdbnullsBool(objDR("Marion"))
                    cbxNassau.Checked = HelpFunction.ConvertdbnullsBool(objDR("Nassau"))
                    cbxPutnam.Checked = HelpFunction.ConvertdbnullsBool(objDR("Putnam"))
                    cbxStJohns.Checked = HelpFunction.ConvertdbnullsBool(objDR("St. Johns"))
                    cbxUnion.Checked = HelpFunction.ConvertdbnullsBool(objDR("Union"))
                    cbxCitrus.Checked = HelpFunction.ConvertdbnullsBool(objDR("Citrus"))
                    cbxHardee.Checked = HelpFunction.ConvertdbnullsBool(objDR("Hardee"))
                    cbxHernando.Checked = HelpFunction.ConvertdbnullsBool(objDR("Hernando"))
                    cbxHillsborough.Checked = HelpFunction.ConvertdbnullsBool(objDR("Hillsborough"))
                    cbxPasco.Checked = HelpFunction.ConvertdbnullsBool(objDR("Pasco"))
                    cbxPinellas.Checked = HelpFunction.ConvertdbnullsBool(objDR("Pinellas"))
                    cbxPolk.Checked = HelpFunction.ConvertdbnullsBool(objDR("Polk"))
                    cbxSumter.Checked = HelpFunction.ConvertdbnullsBool(objDR("Sumter"))
                    cbxBrevard.Checked = HelpFunction.ConvertdbnullsBool(objDR("Brevard"))
                    cbxIndianRiver.Checked = HelpFunction.ConvertdbnullsBool(objDR("Indian River"))
                    cbxLake.Checked = HelpFunction.ConvertdbnullsBool(objDR("Lake"))
                    cbxMartin.Checked = HelpFunction.ConvertdbnullsBool(objDR("Martin"))
                    cbxOrange.Checked = HelpFunction.ConvertdbnullsBool(objDR("Orange"))
                    cbxOsceola.Checked = HelpFunction.ConvertdbnullsBool(objDR("Osceola"))
                    cbxSeminole.Checked = HelpFunction.ConvertdbnullsBool(objDR("Seminole"))
                    cbxStLucie.Checked = HelpFunction.ConvertdbnullsBool(objDR("St. Lucie"))
                    cbxVolusia.Checked = HelpFunction.ConvertdbnullsBool(objDR("Volusia"))
                    cbxCharlotte.Checked = HelpFunction.ConvertdbnullsBool(objDR("Charlotte"))
                    cbxCollier.Checked = HelpFunction.ConvertdbnullsBool(objDR("Collier"))
                    cbxDeSoto.Checked = HelpFunction.ConvertdbnullsBool(objDR("DeSoto"))
                    cbxGlades.Checked = HelpFunction.ConvertdbnullsBool(objDR("Glades"))
                    cbxHendry.Checked = HelpFunction.ConvertdbnullsBool(objDR("Hendry"))
                    cbxHighlands.Checked = HelpFunction.ConvertdbnullsBool(objDR("Highlands"))
                    cbxLee.Checked = HelpFunction.ConvertdbnullsBool(objDR("Lee"))
                    cbxManatee.Checked = HelpFunction.ConvertdbnullsBool(objDR("Manatee"))
                    cbxOkeechobee.Checked = HelpFunction.ConvertdbnullsBool(objDR("Okeechobee"))
                    cbxSarasota.Checked = HelpFunction.ConvertdbnullsBool(objDR("Sarasota"))
                    cbxBroward.Checked = HelpFunction.ConvertdbnullsBool(objDR("Broward"))
                    cbxMiamiDade.Checked = HelpFunction.ConvertdbnullsBool(objDR("Miami-Dade"))
                    cbxMonroe.Checked = HelpFunction.ConvertdbnullsBool(objDR("Monroe"))
                    cbxPalmBeach.Checked = HelpFunction.ConvertdbnullsBool(objDR("Palm Beach"))

                    cbxBay2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Bay"))
                    cbxCalhoun2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Calhoun"))
                    cbxEscambia2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Escambia"))
                    cbxGulf2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Gulf"))
                    cbxHolmes2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Holmes"))
                    cbxJackson2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Jackson"))
                    cbxOkaloosa2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Okaloosa"))
                    cbxSantaRosa2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Santa Rosa"))
                    cbxWalton2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Walton"))
                    cbxWashington2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Washington"))
                    cbxColumbia2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Columbia"))
                    cbxDixie2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Dixie"))
                    cbxFranklin2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Franklin"))
                    cbxGadsden2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Gadsden"))
                    cbxHamilton2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Hamilton"))
                    cbxJefferson2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Jefferson"))
                    cbxLafayette2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Lafayette"))
                    cbxLeon2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Leon"))
                    cbxLevy2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Levy"))
                    cbxLiberty2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Liberty"))
                    cbxMadison2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Madison"))
                    cbxSuwannee2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Suwannee"))
                    cbxTaylor2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Taylor"))
                    cbxWakulla2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Wakulla"))
                    cbxAlachua2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Alachua"))
                    cbxBaker2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Baker"))
                    cbxBradford2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Bradford"))
                    cbxClay2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Clay"))
                    cbxDuval2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Duval"))
                    cbxFlagler2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Flagler"))
                    cbxGilchrist2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Gilchrist"))
                    cbxMarion2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Marion"))
                    cbxNassau2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Nassau"))
                    cbxPutnam2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Putnam"))
                    cbxStJohns2.Checked = HelpFunction.ConvertdbnullsBool(objDR("St. Johns"))
                    cbxUnion2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Union"))
                    cbxCitrus2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Citrus"))
                    cbxHardee2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Hardee"))
                    cbxHernando2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Hernando"))
                    cbxHillsborough2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Hillsborough"))
                    cbxPasco2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Pasco"))
                    cbxPinellas2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Pinellas"))
                    cbxPolk2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Polk"))
                    cbxSumter2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Sumter"))
                    cbxBrevard2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Brevard"))
                    cbxIndianRiver2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Indian River"))
                    cbxLake2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Lake"))
                    cbxMartin2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Martin"))
                    cbxOrange2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Orange"))
                    cbxOsceola2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Osceola"))
                    cbxSeminole2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Seminole"))
                    cbxStLucie2.Checked = HelpFunction.ConvertdbnullsBool(objDR("St. Lucie"))
                    cbxVolusia2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Volusia"))
                    cbxCharlotte2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Charlotte"))
                    cbxCollier2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Collier"))
                    cbxDeSoto2.Checked = HelpFunction.ConvertdbnullsBool(objDR("DeSoto"))
                    cbxGlades2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Glades"))
                    cbxHendry2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Hendry"))
                    cbxHighlands2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Highlands"))
                    cbxLee2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Lee"))
                    cbxManatee2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Manatee"))
                    cbxOkeechobee2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Okeechobee"))
                    cbxSarasota2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Sarasota"))
                    cbxBroward2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Broward"))
                    cbxMiamiDade2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Miami-Dade"))
                    cbxMonroe2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Monroe"))
                    cbxPalmBeach2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Palm Beach"))

                End If

                objDR.Close()

                objCmd.Dispose()
                objCmd = Nothing

                objConn.Close()


            Catch ex As Exception

                Response.Write(ex.ToString)
                Exit Sub

            End Try

        End If

        'County Grabber Ends Here

    End Sub


    Protected Sub GetRidOf()

        'This incident is being handled: Start
        txtHandled.Text = HelpFunction.Convertdbnulls(objDR("Handled"))

        If txtHandled.Text = "" Then

            'strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for: This incident is being handled. <br />")
            'globalHasErrors = True

        End If

        If txtHandled.Text = "" Then

            'strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for: This incident is being handled. <br />")
            'globalHasErrors = True

        End If


        objCmd.Parameters.AddWithValue("@Handled", txtHandled.Text)


        'This incident is being handled: End


        'Dept/agencies noified, responding, scene: Start
        txtAgencyDeptNotified.Text = HelpFunction.Convertdbnulls(objDR("AgencyDeptNotified"))

        If txtAgencyDeptNotified.Text = "" Then

            'strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for Dept/agencies noified, responding, scene <br />")
            'globalHasErrors = True

        End If

        If txtAgencyDeptNotified.Text = "" Then

            'strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide value for Dept/agencies noified, responding, scene <br />")
            'globalHasErrors = True

        End If

        objCmd.Parameters.AddWithValue("@AgencyDeptNotified", txtAgencyDeptNotified.Text)


        'Dept/agencies noified, responding, scene: End

    End Sub

    Protected Sub InsertIncidentNumber()


        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        Dim localMaxIncidentNumber As Integer = 0

        Dim currentTime As System.DateTime = System.DateTime.Now

        Dim localDate As String = CStr(currentTime.Year)

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectMaxIncidentNumber", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@Year", localDate)

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            localMaxIncidentNumber = HelpFunction.ConvertdbnullsInt(objDR("Count"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        Response.Write(localMaxIncidentNumber)

        localMaxIncidentNumber = localMaxIncidentNumber + 1

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        '// Enter the email and password to query/command object.
        objCmd = New SqlCommand("spInsertIncidentNumber", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@Year", localDate)
        objCmd.Parameters.AddWithValue("@Number", localMaxIncidentNumber)

        DBConStringHelper.PrepareConnection(objConn)

        objCmd.ExecuteNonQuery()

        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)


    End Sub

    Protected Sub btnRefreshWorksheets_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefreshWorksheets.Click
        getIncidentIncidentType()
    End Sub


    'Export Report to Word
    Protected Sub ExportTOWord()

        'Dim localRandomStringForDOC As String = ""

        'localRandomStringForDOC = HelpFunction.RandomStringGenerator(6)


        ''First we will Delete all Old Reports
        ''HelpFunction.CleanupReportDirectory()
        ''HelpFunction.CleanupReportDirectory2()


        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)

        ''// Add cookie
        'Response.Cookies.Add(oCookie)

    End Sub

    'CheckBox Changes
    Protected Sub cbxStatewide_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxStatewide.CheckedChanged

        If cbxStatewide.Checked = True Then

            cbxBay.Checked = True
            cbxCalhoun.Checked = True
            cbxEscambia.Checked = True
            cbxGulf.Checked = True
            cbxHolmes.Checked = True
            cbxJackson.Checked = True
            cbxOkaloosa.Checked = True
            cbxSantaRosa.Checked = True
            cbxWalton.Checked = True
            cbxWashington.Checked = True
            cbxColumbia.Checked = True
            cbxDixie.Checked = True
            cbxFranklin.Checked = True
            cbxGadsden.Checked = True
            cbxHamilton.Checked = True
            cbxJefferson.Checked = True
            cbxLafayette.Checked = True
            cbxLeon.Checked = True
            cbxLevy.Checked = True
            cbxLiberty.Checked = True
            cbxMadison.Checked = True
            cbxSuwannee.Checked = True
            cbxTaylor.Checked = True
            cbxWakulla.Checked = True
            cbxAlachua.Checked = True
            cbxBaker.Checked = True
            cbxBradford.Checked = True
            cbxClay.Checked = True
            cbxDuval.Checked = True
            cbxFlagler.Checked = True
            cbxGilchrist.Checked = True
            cbxMarion.Checked = True
            cbxNassau.Checked = True
            cbxPutnam.Checked = True
            cbxStJohns.Checked = True
            cbxUnion.Checked = True
            cbxCitrus.Checked = True
            cbxHardee.Checked = True
            cbxHernando.Checked = True
            cbxHillsborough.Checked = True
            cbxPasco.Checked = True
            cbxPinellas.Checked = True
            cbxPolk.Checked = True
            cbxSumter.Checked = True
            cbxBrevard.Checked = True
            cbxIndianRiver.Checked = True
            cbxLake.Checked = True
            cbxMartin.Checked = True
            cbxOrange.Checked = True
            cbxOsceola.Checked = True
            cbxSeminole.Checked = True
            cbxStLucie.Checked = True
            cbxVolusia.Checked = True
            cbxCharlotte.Checked = True
            cbxCollier.Checked = True
            cbxDeSoto.Checked = True
            cbxGlades.Checked = True
            cbxHendry.Checked = True
            cbxHighlands.Checked = True
            cbxLee.Checked = True
            cbxManatee.Checked = True
            cbxOkeechobee.Checked = True
            cbxSarasota.Checked = True
            cbxBroward.Checked = True
            cbxMiamiDade.Checked = True
            cbxMonroe.Checked = True
            cbxPalmBeach.Checked = True

            cbxBay2.Checked = True
            cbxCalhoun2.Checked = True
            cbxEscambia2.Checked = True
            cbxGulf2.Checked = True
            cbxHolmes2.Checked = True
            cbxJackson2.Checked = True
            cbxOkaloosa2.Checked = True
            cbxSantaRosa2.Checked = True
            cbxWalton2.Checked = True
            cbxWashington2.Checked = True
            cbxColumbia2.Checked = True
            cbxDixie2.Checked = True
            cbxFranklin2.Checked = True
            cbxGadsden2.Checked = True
            cbxHamilton2.Checked = True
            cbxJefferson2.Checked = True
            cbxLafayette2.Checked = True
            cbxLeon2.Checked = True
            cbxLevy2.Checked = True
            cbxLiberty2.Checked = True
            cbxMadison2.Checked = True
            cbxSuwannee2.Checked = True
            cbxTaylor2.Checked = True
            cbxWakulla2.Checked = True
            cbxAlachua2.Checked = True
            cbxBaker2.Checked = True
            cbxBradford2.Checked = True
            cbxClay2.Checked = True
            cbxDuval2.Checked = True
            cbxFlagler2.Checked = True
            cbxGilchrist2.Checked = True
            cbxMarion2.Checked = True
            cbxNassau2.Checked = True
            cbxPutnam2.Checked = True
            cbxStJohns2.Checked = True
            cbxUnion2.Checked = True
            cbxCitrus2.Checked = True
            cbxHardee2.Checked = True
            cbxHernando2.Checked = True
            cbxHillsborough2.Checked = True
            cbxPasco2.Checked = True
            cbxPinellas2.Checked = True
            cbxPolk2.Checked = True
            cbxSumter2.Checked = True
            cbxBrevard2.Checked = True
            cbxIndianRiver2.Checked = True
            cbxLake2.Checked = True
            cbxMartin2.Checked = True
            cbxOrange2.Checked = True
            cbxOsceola2.Checked = True
            cbxSeminole2.Checked = True
            cbxStLucie2.Checked = True
            cbxVolusia2.Checked = True
            cbxCharlotte2.Checked = True
            cbxCollier2.Checked = True
            cbxDeSoto2.Checked = True
            cbxGlades2.Checked = True
            cbxHendry2.Checked = True
            cbxHighlands2.Checked = True
            cbxLee2.Checked = True
            cbxManatee2.Checked = True
            cbxOkeechobee2.Checked = True
            cbxSarasota2.Checked = True
            cbxBroward2.Checked = True
            cbxMiamiDade2.Checked = True
            cbxMonroe2.Checked = True
            cbxPalmBeach2.Checked = True

            cbxRegion1.Checked = True
            cbxRegion2.Checked = True
            cbxRegion3.Checked = True
            cbxRegion4.Checked = True
            cbxRegion5.Checked = True
            cbxRegion6.Checked = True
            cbxRegion7.Checked = True

        Else

            cbxBay.Checked = False
            cbxCalhoun.Checked = False
            cbxEscambia.Checked = False
            cbxGulf.Checked = False
            cbxHolmes.Checked = False
            cbxJackson.Checked = False
            cbxOkaloosa.Checked = False
            cbxSantaRosa.Checked = False
            cbxWalton.Checked = False
            cbxWashington.Checked = False
            cbxColumbia.Checked = False
            cbxDixie.Checked = False
            cbxFranklin.Checked = False
            cbxGadsden.Checked = False
            cbxHamilton.Checked = False
            cbxJefferson.Checked = False
            cbxLafayette.Checked = False
            cbxLeon.Checked = False
            cbxLevy.Checked = False
            cbxLiberty.Checked = False
            cbxMadison.Checked = False
            cbxSuwannee.Checked = False
            cbxTaylor.Checked = False
            cbxWakulla.Checked = False
            cbxAlachua.Checked = False
            cbxBaker.Checked = False
            cbxBradford.Checked = False
            cbxClay.Checked = False
            cbxDuval.Checked = False
            cbxFlagler.Checked = False
            cbxGilchrist.Checked = False
            cbxMarion.Checked = False
            cbxNassau.Checked = False
            cbxPutnam.Checked = False
            cbxStJohns.Checked = False
            cbxUnion.Checked = False
            cbxCitrus.Checked = False
            cbxHardee.Checked = False
            cbxHernando.Checked = False
            cbxHillsborough.Checked = False
            cbxPasco.Checked = False
            cbxPinellas.Checked = False
            cbxPolk.Checked = False
            cbxSumter.Checked = False
            cbxBrevard.Checked = False
            cbxIndianRiver.Checked = False
            cbxLake.Checked = False
            cbxMartin.Checked = False
            cbxOrange.Checked = False
            cbxOsceola.Checked = False
            cbxSeminole.Checked = False
            cbxStLucie.Checked = False
            cbxVolusia.Checked = False
            cbxCharlotte.Checked = False
            cbxCollier.Checked = False
            cbxDeSoto.Checked = False
            cbxGlades.Checked = False
            cbxHendry.Checked = False
            cbxHighlands.Checked = False
            cbxLee.Checked = False
            cbxManatee.Checked = False
            cbxOkeechobee.Checked = False
            cbxSarasota.Checked = False
            cbxBroward.Checked = False
            cbxMiamiDade.Checked = False
            cbxMonroe.Checked = False
            cbxPalmBeach.Checked = False

            cbxBay2.Checked = False
            cbxCalhoun2.Checked = False
            cbxEscambia2.Checked = False
            cbxGulf2.Checked = False
            cbxHolmes2.Checked = False
            cbxJackson2.Checked = False
            cbxOkaloosa2.Checked = False
            cbxSantaRosa2.Checked = False
            cbxWalton2.Checked = False
            cbxWashington2.Checked = False
            cbxColumbia2.Checked = False
            cbxDixie2.Checked = False
            cbxFranklin2.Checked = False
            cbxGadsden2.Checked = False
            cbxHamilton2.Checked = False
            cbxJefferson2.Checked = False
            cbxLafayette2.Checked = False
            cbxLeon2.Checked = False
            cbxLevy2.Checked = False
            cbxLiberty2.Checked = False
            cbxMadison2.Checked = False
            cbxSuwannee2.Checked = False
            cbxTaylor2.Checked = False
            cbxWakulla2.Checked = False
            cbxAlachua2.Checked = False
            cbxBaker2.Checked = False
            cbxBradford2.Checked = False
            cbxClay2.Checked = False
            cbxDuval2.Checked = False
            cbxFlagler2.Checked = False
            cbxGilchrist2.Checked = False
            cbxMarion2.Checked = False
            cbxNassau2.Checked = False
            cbxPutnam2.Checked = False
            cbxStJohns2.Checked = False
            cbxUnion2.Checked = False
            cbxCitrus2.Checked = False
            cbxHardee2.Checked = False
            cbxHernando2.Checked = False
            cbxHillsborough2.Checked = False
            cbxPasco2.Checked = False
            cbxPinellas2.Checked = False
            cbxPolk2.Checked = False
            cbxSumter2.Checked = False
            cbxBrevard2.Checked = False
            cbxIndianRiver2.Checked = False
            cbxLake2.Checked = False
            cbxMartin2.Checked = False
            cbxOrange2.Checked = False
            cbxOsceola2.Checked = False
            cbxSeminole2.Checked = False
            cbxStLucie2.Checked = False
            cbxVolusia2.Checked = False
            cbxCharlotte2.Checked = False
            cbxCollier2.Checked = False
            cbxDeSoto2.Checked = False
            cbxGlades2.Checked = False
            cbxHendry2.Checked = False
            cbxHighlands2.Checked = False
            cbxLee2.Checked = False
            cbxManatee2.Checked = False
            cbxOkeechobee2.Checked = False
            cbxSarasota2.Checked = False
            cbxBroward2.Checked = False
            cbxMiamiDade2.Checked = False
            cbxMonroe2.Checked = False
            cbxPalmBeach2.Checked = False


            cbxRegion1.Checked = False
            cbxRegion2.Checked = False
            cbxRegion3.Checked = False
            cbxRegion4.Checked = False
            cbxRegion5.Checked = False
            cbxRegion6.Checked = False
            cbxRegion7.Checked = False

        End If

    End Sub

    Protected Sub cbxRegion1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxRegion1.CheckedChanged

        If cbxRegion1.Checked = True Then

            cbxBay.Checked = True
            cbxCalhoun.Checked = True
            cbxEscambia.Checked = True
            cbxGulf.Checked = True
            cbxHolmes.Checked = True
            cbxJackson.Checked = True
            cbxOkaloosa.Checked = True
            cbxSantaRosa.Checked = True
            cbxWalton.Checked = True
            cbxWashington.Checked = True

            cbxBay2.Checked = True
            cbxCalhoun2.Checked = True
            cbxEscambia2.Checked = True
            cbxGulf2.Checked = True
            cbxHolmes2.Checked = True
            cbxJackson2.Checked = True
            cbxOkaloosa2.Checked = True
            cbxSantaRosa2.Checked = True
            cbxWalton2.Checked = True
            cbxWashington2.Checked = True

        Else

            cbxBay.Checked = False
            cbxCalhoun.Checked = False
            cbxEscambia.Checked = False
            cbxGulf.Checked = False
            cbxHolmes.Checked = False
            cbxJackson.Checked = False
            cbxOkaloosa.Checked = False
            cbxSantaRosa.Checked = False
            cbxWalton.Checked = False
            cbxWashington.Checked = False

            cbxBay2.Checked = False
            cbxCalhoun2.Checked = False
            cbxEscambia2.Checked = False
            cbxGulf2.Checked = False
            cbxHolmes2.Checked = False
            cbxJackson2.Checked = False
            cbxOkaloosa2.Checked = False
            cbxSantaRosa2.Checked = False
            cbxWalton2.Checked = False
            cbxWashington2.Checked = False

        End If

    End Sub

    Protected Sub cbxRegion2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxRegion2.CheckedChanged


        If cbxRegion2.Checked = True Then

            cbxColumbia.Checked = True
            cbxDixie.Checked = True
            cbxFranklin.Checked = True
            cbxGadsden.Checked = True
            cbxHamilton.Checked = True
            cbxJefferson.Checked = True
            cbxLafayette.Checked = True
            cbxLeon.Checked = True
            cbxLiberty.Checked = True
            cbxMadison.Checked = True
            cbxSuwannee.Checked = True
            cbxTaylor.Checked = True
            cbxWakulla.Checked = True

            cbxColumbia2.Checked = True
            cbxDixie2.Checked = True
            cbxFranklin2.Checked = True
            cbxGadsden2.Checked = True
            cbxHamilton2.Checked = True
            cbxJefferson2.Checked = True
            cbxLafayette2.Checked = True
            cbxLeon2.Checked = True
            cbxLiberty2.Checked = True
            cbxMadison2.Checked = True
            cbxSuwannee2.Checked = True
            cbxTaylor2.Checked = True
            cbxWakulla2.Checked = True

        Else

            cbxColumbia.Checked = False
            cbxDixie.Checked = False
            cbxFranklin.Checked = False
            cbxGadsden.Checked = False
            cbxHamilton.Checked = False
            cbxJefferson.Checked = False
            cbxLafayette.Checked = False
            cbxLeon.Checked = False
            cbxLiberty.Checked = False
            cbxMadison.Checked = False
            cbxSuwannee.Checked = False
            cbxTaylor.Checked = False
            cbxWakulla.Checked = False

            cbxColumbia2.Checked = False
            cbxDixie2.Checked = False
            cbxFranklin2.Checked = False
            cbxGadsden2.Checked = False
            cbxHamilton2.Checked = False
            cbxJefferson2.Checked = False
            cbxLafayette2.Checked = False
            cbxLeon2.Checked = False
            cbxLiberty2.Checked = False
            cbxMadison2.Checked = False
            cbxSuwannee2.Checked = False
            cbxTaylor2.Checked = False
            cbxWakulla2.Checked = False

        End If


    End Sub

    Protected Sub cbxRegion3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxRegion3.CheckedChanged

        If cbxRegion3.Checked = True Then


            cbxAlachua.Checked = True
            cbxBaker.Checked = True
            cbxBradford.Checked = True
            cbxClay.Checked = True
            cbxDuval.Checked = True
            cbxFlagler.Checked = True
            cbxGilchrist.Checked = True
            cbxLevy.Checked = True
            cbxMarion.Checked = True
            cbxNassau.Checked = True
            cbxPutnam.Checked = True
            cbxStJohns.Checked = True
            cbxUnion.Checked = True

            cbxAlachua2.Checked = True
            cbxBaker2.Checked = True
            cbxBradford2.Checked = True
            cbxClay2.Checked = True
            cbxDuval2.Checked = True
            cbxFlagler2.Checked = True
            cbxGilchrist2.Checked = True
            cbxLevy2.Checked = True
            cbxMarion2.Checked = True
            cbxNassau2.Checked = True
            cbxPutnam2.Checked = True
            cbxStJohns2.Checked = True
            cbxUnion2.Checked = True

        Else

            cbxAlachua.Checked = False
            cbxBaker.Checked = False
            cbxBradford.Checked = False
            cbxClay.Checked = False
            cbxDuval.Checked = False
            cbxFlagler.Checked = False
            cbxGilchrist.Checked = False
            cbxLevy.Checked = False
            cbxMarion.Checked = False
            cbxNassau.Checked = False
            cbxPutnam.Checked = False
            cbxStJohns.Checked = False
            cbxUnion.Checked = False

            cbxAlachua2.Checked = False
            cbxBaker2.Checked = False
            cbxBradford2.Checked = False
            cbxClay2.Checked = False
            cbxDuval2.Checked = False
            cbxFlagler2.Checked = False
            cbxGilchrist2.Checked = False
            cbxLevy2.Checked = False
            cbxMarion2.Checked = False
            cbxNassau2.Checked = False
            cbxPutnam2.Checked = False
            cbxStJohns2.Checked = False
            cbxUnion2.Checked = False

        End If

    End Sub

    Protected Sub cbxRegion4_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxRegion4.CheckedChanged

        If cbxRegion4.Checked = True Then


            cbxCitrus.Checked = True
            cbxHardee.Checked = True
            cbxHernando.Checked = True
            cbxHillsborough.Checked = True
            cbxPasco.Checked = True
            cbxPinellas.Checked = True
            cbxPolk.Checked = True
            cbxSumter.Checked = True

            cbxCitrus2.Checked = True
            cbxHardee2.Checked = True
            cbxHernando2.Checked = True
            cbxHillsborough2.Checked = True
            cbxPasco2.Checked = True
            cbxPinellas2.Checked = True
            cbxPolk2.Checked = True
            cbxSumter2.Checked = True

        Else

            cbxCitrus.Checked = False
            cbxHardee.Checked = False
            cbxHernando.Checked = False
            cbxHillsborough.Checked = False
            cbxPasco.Checked = False
            cbxPinellas.Checked = False
            cbxPolk.Checked = False
            cbxSumter.Checked = False

            cbxCitrus2.Checked = False
            cbxHardee2.Checked = False
            cbxHernando2.Checked = False
            cbxHillsborough2.Checked = False
            cbxPasco2.Checked = False
            cbxPinellas2.Checked = False
            cbxPolk2.Checked = False
            cbxSumter2.Checked = False

        End If

    End Sub

    Protected Sub cbxRegion5_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxRegion5.CheckedChanged

        If cbxRegion5.Checked = True Then

            cbxBrevard.Checked = True
            cbxIndianRiver.Checked = True
            cbxLake.Checked = True
            cbxMartin.Checked = True
            cbxOrange.Checked = True
            cbxOsceola.Checked = True
            cbxSeminole.Checked = True
            cbxStLucie.Checked = True
            cbxVolusia.Checked = True

            cbxBrevard2.Checked = True
            cbxIndianRiver2.Checked = True
            cbxLake2.Checked = True
            cbxMartin2.Checked = True
            cbxOrange2.Checked = True
            cbxOsceola2.Checked = True
            cbxSeminole2.Checked = True
            cbxStLucie2.Checked = True
            cbxVolusia2.Checked = True

        Else

            cbxBrevard.Checked = False
            cbxIndianRiver.Checked = False
            cbxLake.Checked = False
            cbxMartin.Checked = False
            cbxOrange.Checked = False
            cbxOsceola.Checked = False
            cbxSeminole.Checked = False
            cbxStLucie.Checked = False
            cbxVolusia.Checked = False

            cbxBrevard2.Checked = False
            cbxIndianRiver2.Checked = False
            cbxLake2.Checked = False
            cbxMartin2.Checked = False
            cbxOrange2.Checked = False
            cbxOsceola2.Checked = False
            cbxSeminole2.Checked = False
            cbxStLucie2.Checked = False
            cbxVolusia2.Checked = False

        End If

    End Sub

    Protected Sub cbxRegion6_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxRegion6.CheckedChanged

        If cbxRegion6.Checked = True Then

            cbxCharlotte.Checked = True
            cbxCollier.Checked = True
            cbxDeSoto.Checked = True
            cbxGlades.Checked = True
            cbxHendry.Checked = True
            cbxHighlands.Checked = True
            cbxLee.Checked = True
            cbxManatee.Checked = True
            cbxOkeechobee.Checked = True
            cbxSarasota.Checked = True

            cbxCharlotte2.Checked = True
            cbxCollier2.Checked = True
            cbxDeSoto2.Checked = True
            cbxGlades2.Checked = True
            cbxHendry2.Checked = True
            cbxHighlands2.Checked = True
            cbxLee2.Checked = True
            cbxManatee2.Checked = True
            cbxOkeechobee2.Checked = True
            cbxSarasota2.Checked = True

        Else

            cbxCharlotte.Checked = False
            cbxCollier.Checked = False
            cbxDeSoto.Checked = False
            cbxGlades.Checked = False
            cbxHendry.Checked = False
            cbxHighlands.Checked = False
            cbxLee.Checked = False
            cbxManatee.Checked = False
            cbxOkeechobee.Checked = False
            cbxSarasota.Checked = False

            cbxCharlotte2.Checked = False
            cbxCollier2.Checked = False
            cbxDeSoto2.Checked = False
            cbxGlades2.Checked = False
            cbxHendry2.Checked = False
            cbxHighlands2.Checked = False
            cbxLee2.Checked = False
            cbxManatee2.Checked = False
            cbxOkeechobee2.Checked = False
            cbxSarasota2.Checked = False

        End If

    End Sub

    Protected Sub cbxRegion7_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxRegion7.CheckedChanged

        If cbxRegion7.Checked = True Then

            cbxBroward.Checked = True
            cbxMiamiDade.Checked = True
            cbxMonroe.Checked = True
            cbxPalmBeach.Checked = True

            cbxBroward2.Checked = True
            cbxMiamiDade2.Checked = True
            cbxMonroe2.Checked = True
            cbxPalmBeach2.Checked = True

        Else

            cbxBroward.Checked = False
            cbxMiamiDade.Checked = False
            cbxMonroe.Checked = False
            cbxPalmBeach.Checked = False

            cbxBroward2.Checked = False
            cbxMiamiDade2.Checked = False
            cbxMonroe2.Checked = False
            cbxPalmBeach2.Checked = False

        End If

    End Sub



    'Regular County Check Boxes 
    Protected Sub cbxAlachua_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxAlachua.CheckedChanged

        If cbxAlachua.Checked = True Then
            cbxAlachua2.Checked = True
        End If

        If cbxAlachua.Checked = False Then
            cbxAlachua2.Checked = False
        End If

    End Sub
    Protected Sub cbxBaker_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxBaker.CheckedChanged

        If cbxBaker.Checked = True Then
            cbxBaker2.Checked = True
        End If

        If cbxBaker.Checked = False Then
            cbxBaker2.Checked = False
        End If

    End Sub
    Protected Sub cbxBay_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxBay.CheckedChanged

        If cbxBay.Checked = True Then
            cbxBay2.Checked = True
        End If

        If cbxBay.Checked = False Then
            cbxBay2.Checked = False
        End If

    End Sub
    Protected Sub cbxBradford_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxBradford.CheckedChanged

        If cbxBradford.Checked = True Then
            cbxBradford2.Checked = True
        End If

        If cbxBradford.Checked = False Then
            cbxBradford2.Checked = False
        End If

    End Sub
    Protected Sub cbxBrevard_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxBrevard.CheckedChanged

        If cbxBrevard.Checked = True Then
            cbxBrevard2.Checked = True
        End If

        If cbxBrevard.Checked = False Then
            cbxBrevard2.Checked = False
        End If

    End Sub
    Protected Sub cbxBroward_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxBroward.CheckedChanged

        If cbxBroward.Checked = True Then
            cbxBroward2.Checked = True
        End If

        If cbxBroward.Checked = False Then
            cbxBroward2.Checked = False
        End If

    End Sub
    Protected Sub cbxCalhoun_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxCalhoun.CheckedChanged

        If cbxCalhoun.Checked = True Then
            cbxCalhoun2.Checked = True
        End If

        If cbxCalhoun.Checked = False Then
            cbxCalhoun2.Checked = False
        End If

    End Sub
    Protected Sub cbxCharlotte_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxCharlotte.CheckedChanged

        If cbxCharlotte.Checked = True Then
            cbxCharlotte2.Checked = True
        End If

        If cbxCharlotte.Checked = False Then
            cbxCharlotte2.Checked = False
        End If

    End Sub
    Protected Sub cbxCitrus_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxCitrus.CheckedChanged

        If cbxCitrus.Checked = True Then
            cbxCitrus2.Checked = True
        End If

        If cbxCitrus.Checked = False Then
            cbxCitrus2.Checked = False
        End If

    End Sub
    Protected Sub cbxClay_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxClay.CheckedChanged

        If cbxClay.Checked = True Then
            cbxClay2.Checked = True
        End If

        If cbxClay.Checked = False Then
            cbxClay2.Checked = False
        End If

    End Sub
    Protected Sub cbxCollier_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxCollier.CheckedChanged

        If cbxCollier.Checked = True Then
            cbxCollier2.Checked = True
        End If

        If cbxCollier.Checked = False Then
            cbxCollier2.Checked = False
        End If

    End Sub
    Protected Sub cbxColumbia_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxColumbia.CheckedChanged

        If cbxColumbia.Checked = True Then
            cbxColumbia2.Checked = True
        End If

        If cbxColumbia.Checked = False Then
            cbxColumbia2.Checked = False
        End If

    End Sub
    Protected Sub cbxDeSoto_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxDeSoto.CheckedChanged

        If cbxDeSoto.Checked = True Then
            cbxDeSoto2.Checked = True
        End If

        If cbxDeSoto.Checked = False Then
            cbxDeSoto2.Checked = False
        End If

    End Sub
    Protected Sub cbxDixie_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxDixie.CheckedChanged

        If cbxDixie.Checked = True Then
            cbxDixie2.Checked = True
        End If

        If cbxDixie.Checked = False Then
            cbxDixie2.Checked = False
        End If

    End Sub
    Protected Sub cbxDuval_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxDuval.CheckedChanged

        If cbxDuval.Checked = True Then
            cbxDuval2.Checked = True
        End If

        If cbxDuval.Checked = False Then
            cbxDuval2.Checked = False
        End If

    End Sub
    Protected Sub cbxEscambia_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxEscambia.CheckedChanged

        If cbxEscambia.Checked = True Then
            cbxEscambia2.Checked = True
        End If

        If cbxEscambia.Checked = False Then
            cbxEscambia2.Checked = False
        End If

    End Sub
    Protected Sub cbxFlagler_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxFlagler.CheckedChanged

        If cbxFlagler.Checked = True Then
            cbxFlagler2.Checked = True
        End If

        If cbxFlagler.Checked = False Then
            cbxFlagler2.Checked = False
        End If

    End Sub
    Protected Sub cbxFranklin_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxFranklin.CheckedChanged

        If cbxFranklin.Checked = True Then
            cbxFranklin2.Checked = True
        End If

        If cbxFranklin.Checked = False Then
            cbxFranklin2.Checked = False
        End If

    End Sub
    Protected Sub cbxGadsden_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxGadsden.CheckedChanged

        If cbxGadsden.Checked = True Then
            cbxGadsden2.Checked = True
        End If

        If cbxGadsden.Checked = False Then
            cbxGadsden2.Checked = False
        End If

    End Sub
    Protected Sub cbxGilchrist_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxGilchrist.CheckedChanged

        If cbxGilchrist.Checked = True Then
            cbxGilchrist2.Checked = True
        End If

        If cbxGilchrist.Checked = False Then
            cbxGilchrist2.Checked = False
        End If

    End Sub
    Protected Sub cbxGlades_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxGlades.CheckedChanged

        If cbxGlades.Checked = True Then
            cbxGlades2.Checked = True
        End If

        If cbxGlades.Checked = False Then
            cbxGlades2.Checked = False
        End If

    End Sub
    Protected Sub cbxGulf_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxGulf.CheckedChanged

        If cbxGulf.Checked = True Then
            cbxGulf2.Checked = True
        End If

        If cbxGulf.Checked = False Then
            cbxGulf2.Checked = False
        End If

    End Sub
    Protected Sub cbxHamilton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxHamilton.CheckedChanged

        If cbxHamilton.Checked = True Then
            cbxHamilton2.Checked = True
        End If

        If cbxHamilton.Checked = False Then
            cbxHamilton2.Checked = False
        End If

    End Sub
    Protected Sub cbxHardee_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxHardee.CheckedChanged

        If cbxHendry.Checked = True Then
            cbxHendry2.Checked = True
        End If

        If cbxHendry.Checked = False Then
            cbxHendry2.Checked = False
        End If

    End Sub
    Protected Sub cbxHendry_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxHendry.CheckedChanged

        If cbxHendry.Checked = True Then
            cbxHendry2.Checked = True
        End If

        If cbxHendry.Checked = False Then
            cbxHendry2.Checked = False
        End If

    End Sub
    Protected Sub cbxHernando_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxHernando.CheckedChanged

        If cbxHernando.Checked = True Then
            cbxHernando2.Checked = True
        End If

        If cbxHernando.Checked = False Then
            cbxHernando2.Checked = False
        End If

    End Sub
    Protected Sub cbxHighlands_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxHighlands.CheckedChanged

        If cbxHighlands.Checked = True Then
            cbxHighlands2.Checked = True
        End If

        If cbxHighlands.Checked = False Then
            cbxHighlands2.Checked = False
        End If

    End Sub
    Protected Sub cbxHillsborough_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxHillsborough.CheckedChanged

        If cbxHillsborough.Checked = True Then
            cbxHillsborough2.Checked = True
        End If

        If cbxHillsborough.Checked = False Then
            cbxHillsborough2.Checked = False
        End If

    End Sub
    Protected Sub cbxHolmes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxHolmes.CheckedChanged

        If cbxHolmes.Checked = True Then
            cbxHolmes2.Checked = True
        End If

        If cbxHolmes.Checked = False Then
            cbxHolmes2.Checked = False
        End If

    End Sub
    Protected Sub cbxIndianRiver_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxIndianRiver.CheckedChanged

        If cbxIndianRiver.Checked = True Then
            cbxIndianRiver.Checked = True
        End If

        If cbxIndianRiver.Checked = False Then
            cbxIndianRiver.Checked = False
        End If

    End Sub
    Protected Sub cbxJackson_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxJackson.CheckedChanged

        If cbxJackson.Checked = True Then
            cbxJackson2.Checked = True
        End If

        If cbxJackson.Checked = False Then
            cbxJackson2.Checked = False
        End If

    End Sub
    Protected Sub cbxJefferson_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxJefferson.CheckedChanged

        If cbxJefferson.Checked = True Then
            cbxJefferson2.Checked = True
        End If

        If cbxJefferson.Checked = False Then
            cbxJefferson2.Checked = False
        End If

    End Sub
    Protected Sub cbxLafayette_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxLafayette.CheckedChanged

        If cbxLafayette.Checked = True Then
            cbxLafayette2.Checked = True
        End If

        If cbxLafayette.Checked = False Then
            cbxLafayette2.Checked = False
        End If

    End Sub
    Protected Sub cbxLake_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxLake.CheckedChanged

        If cbxLake.Checked = True Then
            cbxLake2.Checked = True
        End If

        If cbxLake.Checked = False Then
            cbxLake2.Checked = False
        End If

    End Sub
    Protected Sub cbxLee_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxLee.CheckedChanged

        If cbxLee.Checked = True Then
            cbxLee2.Checked = True
        End If

        If cbxLee.Checked = False Then
            cbxLee2.Checked = False
        End If

    End Sub
    Protected Sub cbxLeon_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxLeon.CheckedChanged

        If cbxLeon.Checked = True Then
            cbxLeon2.Checked = True
        End If

        If cbxLeon.Checked = False Then
            cbxLeon2.Checked = False
        End If

    End Sub
    Protected Sub cbxLevy_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxLevy.CheckedChanged

        If cbxLevy.Checked = True Then
            cbxLevy2.Checked = True
        End If

        If cbxLevy.Checked = False Then
            cbxLevy2.Checked = False
        End If

    End Sub
    Protected Sub cbxLiberty_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxLiberty.CheckedChanged

        If cbxLiberty.Checked = True Then
            cbxLiberty2.Checked = True
        End If

        If cbxLiberty.Checked = False Then
            cbxLiberty2.Checked = False
        End If

    End Sub
    Protected Sub cbxMadison_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxMadison.CheckedChanged

        If cbxMadison.Checked = True Then
            cbxMadison2.Checked = True
        End If

        If cbxMadison.Checked = False Then
            cbxMadison2.Checked = False
        End If

    End Sub
    Protected Sub cbxManatee_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxManatee.CheckedChanged

        If cbxManatee.Checked = True Then
            cbxManatee2.Checked = True
        End If

        If cbxManatee.Checked = False Then
            cbxManatee2.Checked = False
        End If

    End Sub
    Protected Sub cbxMarion_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxMarion.CheckedChanged

        If cbxMarion.Checked = True Then
            cbxMarion2.Checked = True
        End If

        If cbxMarion.Checked = False Then
            cbxMarion2.Checked = False
        End If
    End Sub
    Protected Sub cbxMartin_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxMartin.CheckedChanged

        If cbxMartin.Checked = True Then
            cbxMartin2.Checked = True
        End If

        If cbxMartin.Checked = False Then
            cbxMartin2.Checked = False
        End If

    End Sub
    Protected Sub cbxMiamiDade_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxMiamiDade.CheckedChanged

        If cbxMiamiDade.Checked = True Then
            cbxMiamiDade2.Checked = False
        End If

        If cbxMiamiDade.Checked = True Then
            cbxMiamiDade2.Checked = False
        End If

    End Sub
    Protected Sub cbxMonroe_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxMonroe.CheckedChanged

        If cbxAlachua.Checked = True Then
            cbxAlachua2.Checked = True
        End If

        If cbxAlachua.Checked = False Then
            cbxAlachua2.Checked = False
        End If

    End Sub
    Protected Sub cbxNassau_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxNassau.CheckedChanged

        If cbxNassau.Checked = True Then
            cbxNassau2.Checked = True
        End If

        If cbxNassau.Checked = False Then
            cbxNassau2.Checked = False
        End If

    End Sub
    Protected Sub cbxOkaloosa_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxOkaloosa.CheckedChanged

        If cbxOkaloosa.Checked = True Then
            cbxOkaloosa2.Checked = True
        End If

        If cbxOkaloosa.Checked = False Then
            cbxOkaloosa2.Checked = False
        End If

    End Sub
    Protected Sub cbxOkeechobee_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxOkeechobee.CheckedChanged

        If cbxAlachua.Checked = True Then
            cbxOkeechobee2.Checked = True
        End If

        If cbxAlachua.Checked = False Then
            cbxOkeechobee2.Checked = False
        End If

    End Sub
    Protected Sub cbxOrange_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxOrange.CheckedChanged

        If cbxOrange.Checked = True Then
            cbxOrange2.Checked = True
        End If

        If cbxOrange.Checked = False Then
            cbxOrange2.Checked = False
        End If

    End Sub
    Protected Sub cbxOsceola_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxOsceola.CheckedChanged

        If cbxOsceola.Checked = True Then
            cbxOsceola2.Checked = True
        End If

        If cbxOsceola.Checked = False Then
            cbxOsceola2.Checked = False
        End If

    End Sub
    Protected Sub cbxPalmBeach_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxPalmBeach.CheckedChanged

        If cbxPalmBeach.Checked = True Then
            cbxPalmBeach2.Checked = True
        End If

        If cbxPalmBeach.Checked = False Then
            cbxPalmBeach2.Checked = False
        End If

    End Sub
    Protected Sub cbxPasco_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxPasco.CheckedChanged

        If cbxPasco.Checked = True Then
            cbxPasco2.Checked = True
        End If

        If cbxPasco.Checked = False Then
            cbxPasco2.Checked = False
        End If

    End Sub
    Protected Sub cbxPinellas_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxPinellas.CheckedChanged

        If cbxPinellas.Checked = True Then
            cbxPinellas2.Checked = True
        End If

        If cbxPinellas.Checked = False Then
            cbxPinellas2.Checked = False
        End If

    End Sub
    Protected Sub cbxPolk_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxPolk.CheckedChanged

        If cbxPolk.Checked = True Then
            cbxPolk2.Checked = True
        End If

        If cbxPolk.Checked = False Then
            cbxPolk2.Checked = False
        End If

    End Sub
    Protected Sub cbxPutnam_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxPutnam.CheckedChanged

        If cbxPutnam.Checked = True Then
            cbxPutnam2.Checked = True
        End If

        If cbxPutnam.Checked = False Then
            cbxPutnam2.Checked = False
        End If

    End Sub
    Protected Sub cbxSantaRosa_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxSantaRosa.CheckedChanged

        If cbxSantaRosa.Checked = True Then
            cbxSantaRosa2.Checked = True
        End If

        If cbxSantaRosa.Checked = False Then
            cbxSantaRosa2.Checked = False
        End If

    End Sub
    Protected Sub cbxSarasota_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxSarasota.CheckedChanged

        If cbxSarasota.Checked = True Then
            cbxSarasota2.Checked = True
        End If

        If cbxSarasota.Checked = False Then
            cbxSarasota2.Checked = False
        End If

    End Sub
    Protected Sub cbxSeminole_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxSeminole.CheckedChanged

        If cbxSeminole.Checked = True Then
            cbxSeminole2.Checked = True
        End If

        If cbxSeminole.Checked = False Then
            cbxSeminole2.Checked = False
        End If

    End Sub
    Protected Sub cbxStJohns_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxStJohns.CheckedChanged

        If cbxStJohns.Checked = True Then
            cbxStJohns2.Checked = True
        End If

        If cbxStJohns.Checked = False Then
            cbxStJohns2.Checked = False
        End If

    End Sub
    Protected Sub cbxStLucie_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxStLucie.CheckedChanged

        If cbxStLucie.Checked = True Then
            cbxStLucie2.Checked = True
        End If

        If cbxStLucie.Checked = False Then
            cbxStLucie2.Checked = False
        End If

    End Sub
    Protected Sub cbxSumter_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxSumter.CheckedChanged

        If cbxSumter.Checked = True Then
            cbxSumter2.Checked = True
        End If

        If cbxSumter.Checked = False Then
            cbxSumter2.Checked = False
        End If

    End Sub
    Protected Sub cbxSuwannee_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxSuwannee.CheckedChanged

        If cbxSuwannee.Checked = True Then
            cbxSuwannee2.Checked = True
        End If

        If cbxSuwannee.Checked = False Then
            cbxSuwannee2.Checked = False
        End If

    End Sub
    Protected Sub cbxTaylor_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxTaylor.CheckedChanged

        If cbxTaylor.Checked = True Then
            cbxTaylor2.Checked = True
        End If

        If cbxTaylor.Checked = False Then
            cbxTaylor2.Checked = False
        End If

    End Sub
    Protected Sub cbxUnion_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxUnion.CheckedChanged

        If cbxUnion.Checked = True Then
            cbxUnion2.Checked = True
        End If

        If cbxUnion.Checked = False Then
            cbxUnion2.Checked = False
        End If

    End Sub
    Protected Sub cbxVolusia_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxVolusia.CheckedChanged

        If cbxVolusia.Checked = True Then
            cbxVolusia2.Checked = True
        End If

        If cbxVolusia.Checked = False Then
            cbxVolusia2.Checked = False
        End If

    End Sub
    Protected Sub cbxWakulla_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxWakulla.CheckedChanged

        If cbxWakulla.Checked = True Then
            cbxWakulla2.Checked = True
        End If

        If cbxWakulla.Checked = False Then
            cbxWakulla2.Checked = False
        End If

    End Sub
    Protected Sub cbxWalton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxWalton.CheckedChanged

        If cbxWalton.Checked = True Then
            cbxWalton2.Checked = True
        End If

        If cbxWalton.Checked = False Then
            cbxWalton2.Checked = False
        End If

    End Sub
    Protected Sub cbxWashington_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxWashington.CheckedChanged

        If cbxWashington.Checked = True Then
            cbxWashington2.Checked = True
        End If

        If cbxWashington.Checked = False Then
            cbxWashington2.Checked = False
        End If

    End Sub



    '2 County Check Boxes 
    Protected Sub cbxAlachua2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxAlachua2.CheckedChanged

        If cbxAlachua2.Checked = True Then
            cbxAlachua.Checked = True
        End If

        If cbxAlachua2.Checked = False Then
            cbxAlachua.Checked = False
        End If

    End Sub
    Protected Sub cbxBaker2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxBaker2.CheckedChanged

        If cbxBaker2.Checked = True Then
            cbxBaker.Checked = True
        End If

        If cbxBaker2.Checked = False Then
            cbxBaker.Checked = False
        End If

    End Sub
    Protected Sub cbxBay2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxBay2.CheckedChanged

        If cbxBay2.Checked = True Then
            cbxBay.Checked = True
        End If

        If cbxBay2.Checked = False Then
            cbxBay.Checked = False
        End If

    End Sub
    Protected Sub cbxBradford2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxBradford2.CheckedChanged

        If cbxBradford2.Checked = True Then
            cbxBradford.Checked = True
        End If

        If cbxBradford2.Checked = False Then
            cbxBradford.Checked = False
        End If

    End Sub
    Protected Sub cbxBrevard2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxBrevard2.CheckedChanged

        If cbxBrevard2.Checked = True Then
            cbxBrevard.Checked = True
        End If

        If cbxBrevard2.Checked = False Then
            cbxBrevard.Checked = False
        End If

    End Sub
    Protected Sub cbxBroward2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxBroward2.CheckedChanged

        If cbxBroward2.Checked = True Then
            cbxBroward.Checked = True
        End If

        If cbxBroward2.Checked = False Then
            cbxBroward.Checked = False
        End If

    End Sub
    Protected Sub cbxCalhoun2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxCalhoun2.CheckedChanged

        If cbxCalhoun2.Checked = True Then
            cbxCalhoun.Checked = True
        End If

        If cbxCalhoun2.Checked = False Then
            cbxCalhoun.Checked = False
        End If

    End Sub
    Protected Sub cbxCharlotte2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxCharlotte2.CheckedChanged

        If cbxCharlotte2.Checked = True Then
            cbxCharlotte.Checked = True
        End If

        If cbxCharlotte2.Checked = False Then
            cbxCharlotte.Checked = False
        End If

    End Sub
    Protected Sub cbxCitrus2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxCitrus2.CheckedChanged

        If cbxCitrus2.Checked = True Then
            cbxCitrus.Checked = True
        End If

        If cbxCitrus2.Checked = False Then
            cbxCitrus.Checked = False
        End If

    End Sub
    Protected Sub cbxClay2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxClay2.CheckedChanged

        If cbxClay2.Checked = True Then
            cbxClay.Checked = True
        End If

        If cbxClay2.Checked = False Then
            cbxClay.Checked = False
        End If

    End Sub
    Protected Sub cbxCollier2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxCollier2.CheckedChanged

        If cbxCollier2.Checked = True Then
            cbxCollier.Checked = True
        End If

        If cbxCollier2.Checked = False Then
            cbxCollier.Checked = False
        End If

    End Sub
    Protected Sub cbxColumbia2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxColumbia2.CheckedChanged

        If cbxColumbia2.Checked = True Then
            cbxColumbia.Checked = True
        End If

        If cbxColumbia2.Checked = False Then
            cbxColumbia.Checked = False
        End If

    End Sub
    Protected Sub cbxDeSoto2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxDeSoto2.CheckedChanged

        If cbxDeSoto2.Checked = True Then
            cbxDeSoto.Checked = True
        End If

        If cbxDeSoto2.Checked = False Then
            cbxDeSoto.Checked = False
        End If

    End Sub
    Protected Sub cbxDixie2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxDixie2.CheckedChanged

        If cbxDixie2.Checked = True Then
            cbxDixie.Checked = True
        End If

        If cbxDixie2.Checked = False Then
            cbxDixie.Checked = False
        End If

    End Sub
    Protected Sub cbxDuval2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxDuval2.CheckedChanged

        If cbxDuval2.Checked = True Then
            cbxDuval.Checked = True
        End If

        If cbxDuval2.Checked = False Then
            cbxDuval.Checked = False
        End If

    End Sub
    Protected Sub cbxEscambia2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxEscambia2.CheckedChanged

        If cbxEscambia2.Checked = True Then
            cbxEscambia.Checked = True
        End If

        If cbxEscambia2.Checked = False Then
            cbxEscambia.Checked = False
        End If

    End Sub
    Protected Sub cbxFlagler2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxFlagler2.CheckedChanged

        If cbxFlagler2.Checked = True Then
            cbxFlagler.Checked = True
        End If

        If cbxFlagler2.Checked = False Then
            cbxFlagler.Checked = False
        End If

    End Sub
    Protected Sub cbxFranklin2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxFranklin2.CheckedChanged

        If cbxFranklin2.Checked = True Then
            cbxFranklin.Checked = True
        End If

        If cbxFranklin2.Checked = False Then
            cbxFranklin.Checked = False
        End If

    End Sub
    Protected Sub cbxGadsden2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxGadsden2.CheckedChanged

        If cbxGadsden2.Checked = True Then
            cbxGadsden.Checked = True
        End If

        If cbxGadsden2.Checked = False Then
            cbxGadsden.Checked = False
        End If

    End Sub
    Protected Sub cbxGilchrist2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxGilchrist2.CheckedChanged

        If cbxGilchrist2.Checked = True Then
            cbxGilchrist.Checked = True
        End If

        If cbxGilchrist2.Checked = False Then
            cbxGilchrist.Checked = False
        End If

    End Sub
    Protected Sub cbxGlades2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxGlades2.CheckedChanged

        If cbxGlades2.Checked = True Then
            cbxGlades.Checked = True
        End If

        If cbxGlades2.Checked = False Then
            cbxGlades.Checked = False
        End If

    End Sub
    Protected Sub cbxGulf2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxGulf2.CheckedChanged

        If cbxGulf2.Checked = True Then
            cbxGulf.Checked = True
        End If

        If cbxGulf2.Checked = False Then
            cbxGulf.Checked = False
        End If

    End Sub
    Protected Sub cbxHamilton2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxHamilton2.CheckedChanged

        If cbxHamilton2.Checked = True Then
            cbxHamilton.Checked = True
        End If

        If cbxHamilton2.Checked = False Then
            cbxHamilton.Checked = False
        End If

    End Sub
    Protected Sub cbxHardee2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxHardee2.CheckedChanged

        If cbxHardee2.Checked = True Then
            cbxHardee.Checked = True
        End If

        If cbxHardee2.Checked = False Then
            cbxHardee.Checked = False
        End If

    End Sub
    Protected Sub cbxHendry2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxHendry2.CheckedChanged

        If cbxHendry2.Checked = True Then
            cbxHendry.Checked = True
        End If

        If cbxHendry2.Checked = False Then
            cbxHendry.Checked = False
        End If

    End Sub
    Protected Sub cbxHernando2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxHernando2.CheckedChanged

        If cbxHernando2.Checked = True Then
            cbxHernando.Checked = True
        End If

        If cbxHernando2.Checked = False Then
            cbxHernando.Checked = False
        End If

    End Sub
    Protected Sub cbxHighlands2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxHighlands2.CheckedChanged

        If cbxHighlands2.Checked = True Then
            cbxHighlands.Checked = True
        End If

        If cbxHighlands2.Checked = False Then
            cbxHighlands.Checked = False
        End If

    End Sub
    Protected Sub cbxHillsborough2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxHillsborough2.CheckedChanged

        If cbxHillsborough2.Checked = True Then
            cbxHillsborough.Checked = True
        End If

        If cbxHillsborough2.Checked = False Then
            cbxHillsborough.Checked = False
        End If

    End Sub
    Protected Sub cbxHolmes2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxHolmes2.CheckedChanged

        If cbxHolmes2.Checked = True Then
            cbxHolmes.Checked = True
        End If

        If cbxHolmes2.Checked = False Then
            cbxHolmes.Checked = False
        End If

    End Sub
    Protected Sub cbxIndianRiver2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxIndianRiver2.CheckedChanged

        If cbxIndianRiver2.Checked = True Then
            cbxIndianRiver.Checked = True
        End If

        If cbxIndianRiver2.Checked = False Then
            cbxIndianRiver.Checked = False
        End If

    End Sub
    Protected Sub cbxJackson2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxJackson2.CheckedChanged

        If cbxJackson2.Checked = True Then
            cbxJackson.Checked = True
        End If

        If cbxJackson2.Checked = False Then
            cbxJackson.Checked = False
        End If

    End Sub
    Protected Sub cbxJefferson2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxJefferson2.CheckedChanged

        If cbxJefferson2.Checked = True Then
            cbxJefferson.Checked = True
        End If

        If cbxJefferson2.Checked = False Then
            cbxJefferson.Checked = False
        End If

    End Sub
    Protected Sub cbxLafayette2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxLafayette2.CheckedChanged

        If cbxLafayette2.Checked = True Then
            cbxLafayette.Checked = True
        End If

        If cbxLafayette2.Checked = False Then
            cbxLafayette.Checked = False
        End If

    End Sub
    Protected Sub cbxLake2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxLake2.CheckedChanged

        If cbxLake2.Checked = True Then
            cbxLake.Checked = True
        End If

        If cbxLake2.Checked = False Then
            cbxLake.Checked = False
        End If

    End Sub
    Protected Sub cbxLee2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxLee2.CheckedChanged

        If cbxLee2.Checked = True Then
            cbxLee.Checked = True
        End If

        If cbxLee2.Checked = False Then
            cbxLee.Checked = False
        End If

    End Sub
    Protected Sub cbxLeon2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxLeon2.CheckedChanged

        If cbxLeon2.Checked = True Then
            cbxLeon.Checked = True
        End If

        If cbxLeon2.Checked = False Then
            cbxLeon.Checked = False
        End If

    End Sub
    Protected Sub cbxLevy2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxLevy2.CheckedChanged

        If cbxLevy2.Checked = True Then
            cbxLevy.Checked = True
        End If

        If cbxLevy2.Checked = False Then
            cbxLevy.Checked = False
        End If

    End Sub
    Protected Sub cbxLiberty2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxLiberty2.CheckedChanged

        If cbxLiberty2.Checked = True Then
            cbxLiberty.Checked = True
        End If

        If cbxLiberty2.Checked = False Then
            cbxLiberty.Checked = False
        End If

    End Sub
    Protected Sub cbxMadison2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxMadison2.CheckedChanged

        If cbxMadison2.Checked = True Then
            cbxMadison.Checked = True
        End If

        If cbxMadison2.Checked = False Then
            cbxMadison.Checked = False
        End If

    End Sub
    Protected Sub cbxManatee2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxManatee2.CheckedChanged

        If cbxManatee2.Checked = True Then
            cbxManatee.Checked = True
        End If

        If cbxManatee2.Checked = False Then
            cbxManatee.Checked = False
        End If

    End Sub
    Protected Sub cbxMarion2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxMarion2.CheckedChanged

        If cbxMarion2.Checked = True Then
            cbxMarion.Checked = True
        End If

        If cbxMarion2.Checked = False Then
            cbxMarion.Checked = False
        End If

    End Sub
    Protected Sub cbxMartin2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxMartin2.CheckedChanged

        If cbxMartin2.Checked = True Then
            cbxMartin.Checked = True
        End If

        If cbxMartin2.Checked = False Then
            cbxMartin.Checked = False
        End If

    End Sub
    Protected Sub cbxMiamiDade2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxMiamiDade2.CheckedChanged

        If cbxMiamiDade2.Checked = True Then
            cbxMiamiDade.Checked = True
        End If

        If cbxMiamiDade2.Checked = False Then
            cbxMiamiDade.Checked = False
        End If

    End Sub
    Protected Sub cbxMonroe2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxMonroe2.CheckedChanged

        If cbxMonroe2.Checked = True Then
            cbxMonroe.Checked = True
        End If

        If cbxMonroe2.Checked = False Then
            cbxMonroe.Checked = False
        End If

    End Sub
    Protected Sub cbxNassau2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxNassau2.CheckedChanged

        If cbxNassau2.Checked = True Then
            cbxNassau.Checked = True
        End If

        If cbxNassau2.Checked = False Then
            cbxNassau.Checked = False
        End If

    End Sub
    Protected Sub cbxOkaloosa2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxOkaloosa2.CheckedChanged

        If cbxOkaloosa2.Checked = True Then
            cbxOkaloosa.Checked = True
        End If

        If cbxOkaloosa2.Checked = False Then
            cbxOkaloosa.Checked = False
        End If

    End Sub
    Protected Sub cbxOkeechobee2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxOkeechobee2.CheckedChanged

        If cbxOkeechobee2.Checked = True Then
            cbxOkeechobee.Checked = True
        End If

        If cbxOkeechobee2.Checked = False Then
            cbxOkeechobee.Checked = False
        End If

    End Sub
    Protected Sub cbxOrange2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxOrange2.CheckedChanged

        If cbxOrange2.Checked = True Then
            cbxOrange.Checked = True
        End If

        If cbxOrange2.Checked = False Then
            cbxOrange.Checked = False
        End If

    End Sub
    Protected Sub cbxOsceola2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxOsceola2.CheckedChanged

        If cbxOsceola2.Checked = True Then
            cbxOsceola.Checked = True
        End If

        If cbxOsceola2.Checked = False Then
            cbxOsceola.Checked = False
        End If

    End Sub
    Protected Sub cbxPalmBeach2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxPalmBeach2.CheckedChanged

        If cbxPalmBeach2.Checked = True Then
            cbxPalmBeach.Checked = True
        End If

        If cbxPalmBeach2.Checked = False Then
            cbxPalmBeach.Checked = False
        End If

    End Sub
    Protected Sub cbxPasco2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxPasco2.CheckedChanged

        If cbxPasco2.Checked = True Then
            cbxPasco.Checked = True
        End If

        If cbxPasco2.Checked = False Then
            cbxPasco.Checked = False
        End If

    End Sub
    Protected Sub cbxPinellas2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxPinellas2.CheckedChanged

        If cbxPinellas2.Checked = True Then
            cbxPinellas.Checked = True
        End If

        If cbxPinellas2.Checked = False Then
            cbxPinellas.Checked = False
        End If

    End Sub
    Protected Sub cbxPolk2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxPolk2.CheckedChanged

        If cbxPolk2.Checked = True Then
            cbxPolk.Checked = True
        End If

        If cbxPolk2.Checked = False Then
            cbxPolk.Checked = False
        End If

    End Sub
    Protected Sub cbxPutnam2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxPutnam2.CheckedChanged

        If cbxPutnam2.Checked = True Then
            cbxPutnam.Checked = True
        End If

        If cbxPutnam2.Checked = False Then
            cbxPutnam.Checked = False
        End If

    End Sub
    Protected Sub cbxSantaRosa2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxSantaRosa2.CheckedChanged

        If cbxSantaRosa2.Checked = True Then
            cbxSantaRosa.Checked = True
        End If

        If cbxSantaRosa2.Checked = False Then
            cbxSantaRosa.Checked = False
        End If

    End Sub
    Protected Sub cbxSarasota2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxSarasota2.CheckedChanged

        If cbxSarasota2.Checked = True Then
            cbxSarasota.Checked = True
        End If

        If cbxSarasota2.Checked = False Then
            cbxSarasota.Checked = False
        End If

    End Sub
    Protected Sub cbxSeminole2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxSeminole2.CheckedChanged

        If cbxSeminole2.Checked = True Then
            cbxSeminole.Checked = True
        End If

        If cbxSeminole2.Checked = False Then
            cbxSeminole.Checked = False
        End If

    End Sub
    Protected Sub cbxStJohns2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxStJohns2.CheckedChanged

        If cbxStJohns2.Checked = True Then
            cbxStJohns.Checked = True
        End If

        If cbxStJohns2.Checked = False Then
            cbxStJohns.Checked = False
        End If

    End Sub
    Protected Sub cbxStLucie2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxStLucie2.CheckedChanged

        If cbxStLucie2.Checked = True Then
            cbxStLucie.Checked = True
        End If

        If cbxStLucie2.Checked = False Then
            cbxStLucie.Checked = False
        End If

    End Sub
    Protected Sub cbxSumter2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxSumter2.CheckedChanged

        If cbxSumter2.Checked = True Then
            cbxSumter.Checked = True
        End If

        If cbxSumter2.Checked = False Then
            cbxSumter.Checked = False
        End If

    End Sub
    Protected Sub cbxSuwannee2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxSuwannee2.CheckedChanged

        If cbxSuwannee2.Checked = True Then
            cbxSuwannee.Checked = True
        End If

        If cbxSuwannee2.Checked = False Then
            cbxSuwannee.Checked = False
        End If

    End Sub
    Protected Sub cbxTaylor2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxTaylor2.CheckedChanged

        If cbxTaylor2.Checked = True Then
            cbxTaylor.Checked = True
        End If

        If cbxTaylor2.Checked = False Then
            cbxTaylor.Checked = False
        End If

    End Sub
    Protected Sub cbxUnion2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxUnion2.CheckedChanged

        If cbxUnion2.Checked = True Then
            cbxUnion.Checked = True
        End If

        If cbxUnion2.Checked = False Then
            cbxUnion.Checked = False
        End If

    End Sub
    Protected Sub cbxVolusia2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxVolusia2.CheckedChanged

        If cbxVolusia2.Checked = True Then
            cbxVolusia.Checked = True
        End If

        If cbxVolusia2.Checked = False Then
            cbxVolusia.Checked = False
        End If

    End Sub
    Protected Sub cbxWakulla2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxWakulla2.CheckedChanged

        If cbxWakulla2.Checked = True Then
            cbxWakulla.Checked = True
        End If

        If cbxWakulla2.Checked = False Then
            cbxWakulla.Checked = False
        End If

    End Sub
    Protected Sub cbxWalton2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxWalton2.CheckedChanged

        If cbxWalton2.Checked = True Then
            cbxWalton.Checked = True
        End If

        If cbxWalton2.Checked = False Then
            cbxWalton.Checked = False
        End If

    End Sub
    Protected Sub cbxWashington2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxWashington2.CheckedChanged

        If cbxWashington2.Checked = True Then
            cbxWashington.Checked = True
        End If

        If cbxWashington2.Checked = False Then
            cbxWashington.Checked = False
        End If

    End Sub

    Protected Sub lnkNotify_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkNotify.Load

        lnkNotify.NavigateUrl = "NotificationPage.aspx?IncidentID=" & Request("IncidentID")

    End Sub

End Class