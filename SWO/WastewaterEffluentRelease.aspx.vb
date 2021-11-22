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

Partial Class WastewaterEffluentRelease
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

    Dim globalAuditAction As String = ""
    Dim globalHasErrors As Boolean = False
    Dim globalMessage As String
    Dim globalCurrentStep As Integer
    Dim globalIsSaved As Boolean = False
    Dim globalAction As String
    Dim globalParameter As String
    'Dim oCookie As System.Web.HttpCookie
    Dim ns As New SecurityValidate
    Const js As String = "TADDScript.js"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        Select Case ns.UserLevelID.ToString() 'oCookie.Item("UserLevelID")

            Case "1" 'Admin

            Case "2" 'Full User

            Case "3" 'Update User

                btnSave.Disabled = True

            Case "4", "5" 'Read Only and Read Only + Hazmat

                btnSave.Disabled = True

            Case Else

        End Select

        If Page.IsPostBack = False Then

            PopulateDDLs()

            'set message
            globalMessage = Request("Message")
            globalAction = Request("Action")
            globalParameter = Request("Parameter")

            Dim localWastewaterCount As Integer = 0

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectWastewaterCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                localWastewaterCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            'Response.Write(localBombThreatDeviceCount)
            'Response.End()

            If localWastewaterCount > 0 Then
                PopulatePage()
            End If

        End If

    End Sub


    'PagePopulation
    Protected Sub PopulatePage()

        Dim localTime As String = ""

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectWastewaterByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            ddlSubType.SelectedValue = HelpFunction.Convertdbnulls(objDR("SubType"))
            ddlSituation.SelectedValue = HelpFunction.Convertdbnulls(objDR("Situation"))
            txtWWsystemIDPermitNumber.Text = HelpFunction.Convertdbnulls(objDR("WWsystemIDPermitNumber"))
            txtWWsystemName.Text = HelpFunction.Convertdbnulls(objDR("WWsystemName"))
            ddlWWsystemType.SelectedValue = HelpFunction.Convertdbnulls(objDR("WWsystemType"))
            txtPrivateCollectionSystemName.Text = HelpFunction.Convertdbnulls(objDR("WWPrivateCollectionSystemName"))
            ddlWWreleaseOccurred.Text = HelpFunction.Convertdbnulls(objDR("WWreleaseOccurred"))
            txtWWreleaseOccurredDetails.Text = HelpFunction.Convertdbnulls(objDR("WWreleaseOccurredDetails"))
            txtWWtype.Text = HelpFunction.Convertdbnulls(objDR("WWtype"))
            ddlWWreleaseCause.SelectedValue = HelpFunction.Convertdbnulls(objDR("WWreleaseCause"))
            txtWWreleaseCauseDetails.Text = HelpFunction.Convertdbnulls(objDR("WWreleaseCauseDetails"))
            ddlWWreleaseStatus.SelectedValue = HelpFunction.Convertdbnulls(objDR("WWreleaseStatus"))
            txtWWceasedDate.Text = HelpFunction.Convertdbnulls(objDR("WWceasedDate"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR("WWceasedTime")))
            txtWWceasedTime.Text = HelpFunction.Convertdbnulls(objDR("WWceasedTime"))
            ddlWWreleasedContainedonSite.SelectedValue = HelpFunction.Convertdbnulls(objDR("WWreleasedContainedonSite"))
            txtWWreleaseAmount.Text = HelpFunction.Convertdbnulls(objDR("WWreleaseAmount"))
            ddlWWstormWater.SelectedValue = HelpFunction.Convertdbnulls(objDR("WWstormWater"))
            txtWWstormWaterLocation.Text = HelpFunction.Convertdbnulls(objDR("WWstormWaterLocation"))
            txtWWstormWaterDischarge.Text = HelpFunction.Convertdbnulls(objDR("WWstormWaterDischarge"))
            txtWWcleanupActionsText.Text = HelpFunction.Convertdbnulls(objDR("WWcleanupActionsText"))
            ddlWWsurfaceWater.SelectedValue = HelpFunction.Convertdbnulls(objDR("WWsurfaceWater"))
            ddlWWsurfaceWaterDDL.SelectedValue = HelpFunction.Convertdbnulls(objDR("WWsurfaceWaterDDL"))
            txtWWwaterway.Text = HelpFunction.Convertdbnulls(objDR("WWwaterway"))
            ddlWWconfirmedContamination.SelectedValue = HelpFunction.Convertdbnulls(objDR("WWconfirmedContamination"))
            ddlWWcleanupActions.SelectedValue = HelpFunction.Convertdbnulls(objDR("WWcleanupActions"))
            txtTEsystemIDPermitNumber.Text = HelpFunction.Convertdbnulls(objDR("TEsystemIDPermitNumber"))
            txtTEsystemName.Text = HelpFunction.Convertdbnulls(objDR("TEsystemName"))
            txtTEreleaseCause.Text = HelpFunction.Convertdbnulls(objDR("TEreleaseCause"))
            txtTEgallonsReleased.Text = HelpFunction.Convertdbnulls(objDR("TEgallonsReleased"))
            ddlTEcleanupActions.SelectedValue = HelpFunction.Convertdbnulls(objDR("TEcleanupActions"))
            txtTEcleanupActionsText.Text = HelpFunction.Convertdbnulls(objDR("TEcleanupActionsText"))
            ddlNotification.SelectedValue = HelpFunction.Convertdbnulls(objDR("IncidentTypeLevelID"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        txtWorkSheetDescription.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        txtWWceasedTime.Text = Left(localTime, 2)
        txtWWceasedTime2.Text = Right(localTime, 2)

        If txtWWceasedDate.Text = "1/1/1900" Then
            txtWWceasedDate.Text = ""
        End If

        If ddlSubType.SelectedValue = "Wastewater" Then
            pnlShowWastewater.Visible = True
        End If

        If ddlSubType.SelectedValue = "Treated Effluent" Then
            pnlShowTreatedEffluent.Visible = True
        End If

        If txtWWceasedTime.Text = "0" Then
            txtWWceasedTime.Text = ""
        End If

        If txtWWceasedTime2.Text = "0" Then
            txtWWceasedTime2.Text = ""
        End If

        If ddlWWreleaseStatus.SelectedValue = "Yes" Then
            pnlShowCeasedTimeDate.Visible = True
        End If

        If ddlWWstormWater.SelectedValue = "Yes" Then
            pnlShowStormWaterSystem.Visible = True
        End If

        If ddlWWsurfaceWater.SelectedValue = "Yes" Then
            pnlShowRetentionPond.Visible = True
        End If

        If ddlWWsurfaceWaterDDL.SelectedValue = "Retention Pond, contained." Or ddlWWsurfaceWaterDDL.SelectedValue = "Retention pond, drained to waterway." Then
            pnlShowWaterway.Visible = True
        End If

        If ddlTEcleanupActions.SelectedValue = "Yes" Then
            pnlShowTEcleanupActions.Visible = True
        End If

        If ddlWWreleaseStatus.SelectedValue = "Ceased" Then
            pnlShowCeasedTimeDate.Visible = True
        End If

        tblPrivateCollectionSystemName.Visible = ddlWWsystemType.SelectedValue.Equals("Private Collection System")
        tblReleaseCauseDetails.Visible = ddlWWreleaseCause.SelectedValue.Equals("Other")
        tblReleaseOccurrenceDetails.Visible = ddlWWreleaseOccurred.SelectedValue.Equals("Other (note in cause below)")

    End Sub

    Sub PopulateDDLs()

        'Notification Group
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectIncidentTypeLevelForDDL", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentTypeID", MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("IncidentTypeID", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID")))

        DBConStringHelper.PrepareConnection(objConn) 'Open the connection
        ddlNotification.DataSource = objCmd.ExecuteReader()
        ddlNotification.DataBind()
        DBConStringHelper.FinalizeConnection(objConn) 'Close the connection

        objCmd = Nothing

        'add an "Select an Option" item to the list
        ddlNotification.Items.Insert(0, New ListItem("Select an Option", "Select an Option"))
        ddlNotification.Items(0).Selected = True

    End Sub

    'Buttons
    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As EventArgs)
        ErrorChecks()

        If globalHasErrors = False Then
            Save()

            'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID") & "&Parameter=WorkSheet")

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)

        'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)

    End Sub

    Protected Sub Save()


        Dim localWastewaterCount As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectWastewaterCountByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            localWastewaterCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()


        If pnlShowWastewater.Visible = False Then
            txtWWsystemIDPermitNumber.Text = ""
            txtWWsystemName.Text = ""
            ddlWWsystemType.SelectedValue = "Select an Option"
            txtPrivateCollectionSystemName.Text = ""
            txtWWtype.Text = ""
            ddlWWreleaseOccurred.SelectedValue = "Select an Option"
            txtWWreleaseOccurredDetails.Text = ""
            ddlWWreleaseCause.SelectedValue = "Select an Option"
            txtWWreleaseCauseDetails.Text = ""
            ddlWWreleaseStatus.SelectedValue = "Select an Option"
            txtWWceasedTime.Text = ""
            txtWWceasedTime2.Text = ""
            txtWWceasedDate.Text = ""
            ddlWWstormWater.SelectedValue = "Select an Option"
            txtWWstormWaterLocation.Text = ""
            txtWWstormWaterDischarge.Text = ""
            ddlWWsurfaceWater.SelectedValue = "Select an Option"
            ddlWWsurfaceWaterDDL.SelectedValue = "Select an Option"
            txtWWwaterway.Text = ""
            ddlWWconfirmedContamination.SelectedValue = "Select an Option"
            ddlWWcleanupActions.SelectedValue = "Select an Option"
            txtWWcleanupActionsText.Text = ""
            ddlWWreleasedContainedonSite.SelectedValue = "Select an Option"
            txtWWreleaseAmount.Text = ""
        End If

        If pnlShowTreatedEffluent.Visible = False Then
            txtTEsystemIDPermitNumber.Text = ""
            txtTEsystemName.Text = ""
            txtTEreleaseCause.Text = ""
            txtTEgallonsReleased.Text = ""
            ddlTEcleanupActions.SelectedValue = "Select an Option"
            txtTEcleanupActionsText.Text = ""
        End If

        If pnlShowCeasedTimeDate.Visible = False Then
            'ddlWWreleaseStatus.SelectedValue = "Select an Option"
            txtWWceasedTime.Text = ""
            txtWWceasedTime2.Text = ""
        End If

        If pnlShowStormWaterSystem.Visible = False Then
            'ddlWWstormWater.SelectedValue = "Select an Option"
            txtWWstormWaterLocation.Text = ""
        End If

        If pnlShowRetentionPond.Visible = False Then
            ddlWWsurfaceWaterDDL.SelectedValue = "Select an Option"
        End If

        If pnlShowWaterway.Visible = False Then
            txtWWwaterway.Text = ""
        End If

        If pnlShowTEcleanupActions.Visible = False Then
            txtTEcleanupActionsText.Text = ""
        End If

        If localWastewaterCount = 0 Then

            'Try

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionWastewater", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 0)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWsystemIDPermitNumber", txtWWsystemIDPermitNumber.Text)
            objCmd.Parameters.AddWithValue("@WWsystemName", txtWWsystemName.Text)
            objCmd.Parameters.AddWithValue("@WWsystemType", ddlWWsystemType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWPrivateCollectionSystemName", txtPrivateCollectionSystemName.Text)
            objCmd.Parameters.AddWithValue("@WWtype", txtWWtype.Text)
            objCmd.Parameters.AddWithValue("@WWreleaseOccurred", ddlWWreleaseOccurred.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWreleaseCause", ddlWWreleaseCause.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWreleaseCauseDetails", txtWWreleaseCauseDetails.Text)
            objCmd.Parameters.AddWithValue("@WWreleaseOccurredDetails", txtWWreleaseOccurredDetails.Text)
            objCmd.Parameters.AddWithValue("@WWreleaseStatus", ddlWWreleaseStatus.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWceasedDate", txtWWceasedDate.Text)
            objCmd.Parameters.AddWithValue("@WWceasedTime", CStr(txtWWceasedTime.Text.Trim) & CStr(txtWWceasedTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@WWreleasedContainedonSite", ddlWWreleasedContainedonSite.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWreleaseAmount", txtWWreleaseAmount.Text)
            objCmd.Parameters.AddWithValue("@WWstormWater", ddlWWstormWater.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWstormWaterLocation", txtWWstormWaterLocation.Text)
            objCmd.Parameters.AddWithValue("@WWstormWaterDischarge", txtWWstormWaterDischarge.Text)
            objCmd.Parameters.AddWithValue("@WWcleanupActionsText", txtWWcleanupActionsText.Text)
            objCmd.Parameters.AddWithValue("@WWsurfaceWater", ddlWWsurfaceWater.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWsurfaceWaterDDL", ddlWWsurfaceWaterDDL.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWwaterway", txtWWwaterway.Text)
            objCmd.Parameters.AddWithValue("@WWconfirmedContamination", ddlWWconfirmedContamination.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWcleanupActions", ddlWWcleanupActions.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@TEsystemIDPermitNumber", txtTEsystemIDPermitNumber.Text)
            objCmd.Parameters.AddWithValue("@TEsystemName", txtTEsystemName.Text)
            objCmd.Parameters.AddWithValue("@TEreleaseCause", txtTEreleaseCause.Text)
            objCmd.Parameters.AddWithValue("@TEgallonsReleased", txtTEgallonsReleased.Text)
            objCmd.Parameters.AddWithValue("@TEcleanupActions", ddlTEcleanupActions.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@TEcleanupActionsText", txtTEcleanupActionsText.Text)
            objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", ddlNotification.SelectedValue)


            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            'Catch ex As Exception

            '    Response.Write(ex.ToString)
            '    Exit Sub

            'End Try



            Try

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                '// Enter the email and password to query/command object.
                objCmd = New SqlCommand("spUpdateIncidentIncidentType", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
                objCmd.Parameters.AddWithValue("@WorkSheetDescription", txtWorkSheetDescription.Text)



                DBConStringHelper.PrepareConnection(objConn)

                objCmd.ExecuteNonQuery()

                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

            Catch ex As Exception

                Response.Write(ex.ToString)
                Exit Sub

            End Try




            Dim NowDate As Date = Now

            Try
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                '// Enter the email and password to query/command object.

                objCmd = New SqlCommand("spInsertMostRecentUpdateByIncidentID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                objCmd.Parameters.AddWithValue("@UpdateDate", NowDate)
                objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Added Wastewater or Effluent")

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

        Else


            'Response.Write("Its Working!")
            'Response.End()

            'Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionWastewater", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 1)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWsystemIDPermitNumber", txtWWsystemIDPermitNumber.Text)
            objCmd.Parameters.AddWithValue("@WWsystemName", txtWWsystemName.Text)
            objCmd.Parameters.AddWithValue("@WWsystemType", ddlWWsystemType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWPrivateCollectionSystemName", txtPrivateCollectionSystemName.Text)
            objCmd.Parameters.AddWithValue("@WWtype", txtWWtype.Text)
            objCmd.Parameters.AddWithValue("@WWreleaseOccurred", ddlWWreleaseOccurred.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWreleaseCause", ddlWWreleaseCause.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWreleaseCauseDetails", txtWWreleaseCauseDetails.Text)
            objCmd.Parameters.AddWithValue("@WWreleaseOccurredDetails", txtWWreleaseOccurredDetails.Text)
            objCmd.Parameters.AddWithValue("@WWreleaseStatus", ddlWWreleaseStatus.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWceasedDate", txtWWceasedDate.Text)
            objCmd.Parameters.AddWithValue("@WWceasedTime", CStr(txtWWceasedTime.Text.Trim) & CStr(txtWWceasedTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@WWreleasedContainedonSite", ddlWWreleasedContainedonSite.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWreleaseAmount", txtWWreleaseAmount.Text)
            objCmd.Parameters.AddWithValue("@WWstormWater", ddlWWstormWater.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWstormWaterLocation", txtWWstormWaterLocation.Text)
            objCmd.Parameters.AddWithValue("@WWstormWaterDischarge", txtWWstormWaterDischarge.Text)
            objCmd.Parameters.AddWithValue("@WWcleanupActionsText", txtWWcleanupActionsText.Text)
            objCmd.Parameters.AddWithValue("@WWsurfaceWater", ddlWWsurfaceWater.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWsurfaceWaterDDL", ddlWWsurfaceWaterDDL.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWwaterway", txtWWwaterway.Text)
            objCmd.Parameters.AddWithValue("@WWconfirmedContamination", ddlWWconfirmedContamination.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWcleanupActions", ddlWWcleanupActions.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@TEsystemIDPermitNumber", txtTEsystemIDPermitNumber.Text)
            objCmd.Parameters.AddWithValue("@TEsystemName", txtTEsystemName.Text)
            objCmd.Parameters.AddWithValue("@TEreleaseCause", txtTEreleaseCause.Text)
            objCmd.Parameters.AddWithValue("@TEgallonsReleased", txtTEgallonsReleased.Text)
            objCmd.Parameters.AddWithValue("@TEcleanupActions", ddlTEcleanupActions.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@TEcleanupActionsText", txtTEcleanupActionsText.Text)
            objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", ddlNotification.SelectedValue)


            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            'Catch ex As Exception

            '    Response.Write(ex.ToString)
            '    Exit Sub

            'End Try

            Try

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                '// Enter the email and password to query/command object.
                objCmd = New SqlCommand("spUpdateIncidentIncidentType", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
                objCmd.Parameters.AddWithValue("@WorkSheetDescription", txtWorkSheetDescription.Text)



                DBConStringHelper.PrepareConnection(objConn)

                objCmd.ExecuteNonQuery()

                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

            Catch ex As Exception

                Response.Write(ex.ToString)
                Exit Sub

            End Try


            Dim NowDate As Date = Now

            Try
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                '// Enter the email and password to query/command object.

                objCmd = New SqlCommand("spInsertMostRecentUpdateByIncidentID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                objCmd.Parameters.AddWithValue("@UpdateDate", NowDate)
                objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Updated Wastewater or Effluent")

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

        End If

    End Sub



    Protected Sub ddlSubType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubType.SelectedIndexChanged

        If ddlSubType.SelectedValue = "Wastewater" Then
            pnlShowWastewater.Visible = True
        Else
            pnlShowWastewater.Visible = False
        End If

        If ddlSubType.SelectedValue = "Treated Effluent" Then
            pnlShowTreatedEffluent.Visible = True
        Else
            pnlShowTreatedEffluent.Visible = False
        End If
        'Ceased
    End Sub



    Protected Sub ddlWWreleaseStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWWreleaseStatus.SelectedIndexChanged

        If ddlWWreleaseStatus.SelectedValue = "Ceased" Then
            pnlShowCeasedTimeDate.Visible = True
        Else
            pnlShowCeasedTimeDate.Visible = False
        End If

    End Sub



    Protected Sub ddlWWstormWater_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWWstormWater.SelectedIndexChanged

        If ddlWWstormWater.SelectedValue = "Yes" Then
            pnlShowStormWaterSystem.Visible = True
        Else
            pnlShowStormWaterSystem.Visible = False
        End If
        'ddlWWsurfaceWater
    End Sub



    Protected Sub ddlWWsurfaceWater_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWWsurfaceWater.SelectedIndexChanged


        If ddlWWsurfaceWater.SelectedValue = "Yes" Then
            pnlShowRetentionPond.Visible = True
        Else
            pnlShowRetentionPond.Visible = False
        End If

        'ddlWWsurfaceWaterDDL
    End Sub


    Protected Sub ddlWWsurfaceWaterDDL_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWWsurfaceWaterDDL.SelectedIndexChanged

        If ddlWWsurfaceWaterDDL.SelectedValue = "Waterway or Body of Water" Or ddlWWsurfaceWaterDDL.SelectedValue = "Retention pond, drained to waterway." Then
            pnlShowWaterway.Visible = True
        Else
            pnlShowWaterway.Visible = False
        End If

    End Sub


    Protected Sub ddlTEcleanupActions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTEcleanupActions.SelectedIndexChanged

        If ddlTEcleanupActions.SelectedValue = "Yes" Then
            pnlShowTEcleanupActions.Visible = True
        Else
            pnlShowTEcleanupActions.Visible = False
        End If

    End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start The Error String.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        If ddlSubType.SelectedValue = "Select an Option" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Sub-Type. <br />")
            globalHasErrors = True
        End If

        If ddlSituation.SelectedValue = "Select an Option" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Situation. <br />")
            globalHasErrors = True
        End If

        If txtWorkSheetDescription.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Description. <br />")
            globalHasErrors = True
        End If

        If ddlNotification.SelectedValue = "Select an Option" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Notification. <br />")
            globalHasErrors = True
        End If

        If pnlShowWastewater.Visible Then
            If String.IsNullOrEmpty(txtWWsystemIDPermitNumber.Text.Trim) And String.IsNullOrEmpty(txtWWsystemName.Text.Trim) Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Wastewater Facility Permit ID # or a Facility Name / Collection System Name. <br />")
                globalHasErrors = True
            End If
        End If

        'Finish the Error String.
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels.
        lblMessage.Text = strError.ToString
    End Sub

    Protected Sub ddlWWsystemType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlWWsystemType.SelectedIndexChanged
        tblPrivateCollectionSystemName.Visible = ddlWWsystemType.SelectedValue.Equals("Private Collection System")
    End Sub

    Protected Sub ddlWWreleaseCause_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlWWreleaseCause.SelectedIndexChanged
        tblReleaseCauseDetails.Visible = ddlWWreleaseCause.SelectedValue.Equals("Other")
    End Sub

    Protected Sub ddlWWreleaseOccurred_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlWWreleaseOccurred.SelectedIndexChanged
        tblReleaseOccurrenceDetails.Visible = ddlWWreleaseOccurred.SelectedValue.Equals("Other (note in cause below)")
    End Sub
End Class
