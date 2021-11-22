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


Partial Class PetroleumSpill
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

    'Page Load
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

        'Response.End()

        'If Request("IncidentID") = "" Then

        '    Response.Redirect("Incident.aspx")

        'End If

        If Page.IsPostBack = False Then

            'set message
            globalMessage = Request("Message")
            globalAction = Request("Action")
            globalParameter = Request("Parameter")

            PopulateDDLs()


            'Select Case globalAction

            '    Case "Delete"

            '        Select Case globalParameter
            '            Case "IncidentType"
            '                DeleteIncidentType()
            '            Case ""

            '            Case "3"

            '            Case Else

            '        End Select

            '    Case "2"
            '        lblMessage.Text = "User Has Been Deleted."
            '        lblMessage.ForeColor = Drawing.Color.Green
            '        lblMessage.Visible = True
            '    Case "3"
            '        lblMessage.Text = "User Has Been Updated."
            '        lblMessage.ForeColor = Drawing.Color.Green
            '        lblMessage.Visible = True
            '    Case Else

            'End Select

            Dim localPetroleumSpillCount As Integer = 0

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectPetroleumSpillCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                localPetroleumSpillCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

            End If

            objDR.Close()

            objCmd.Dispose()

            objCmd = Nothing

            objConn.Close()



            If localPetroleumSpillCount > 0 Then
                PopulatePage()
            End If

        End If


    End Sub

    'PagePopulation
    Protected Sub PopulatePage()

        Dim localTime As String = ""
        Dim localTime2 As String = ""

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectPetroleumSpillByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            ddlSubType.SelectedValue = HelpFunction.Convertdbnulls(objDR("SubType"))
            ddlSituation.SelectedValue = HelpFunction.Convertdbnulls(objDR("Situation"))
            ddlPetroleumType.SelectedValue = HelpFunction.Convertdbnulls(objDR("PetroleumType"))
            txtPetroleumNameDescription.Text = HelpFunction.Convertdbnulls(objDR("PetroleumNameDescription"))
            txtPetroleumOdor.Text = HelpFunction.Convertdbnulls(objDR("PetroleumOdor"))
            txtPetroleumColor.Text = HelpFunction.Convertdbnulls(objDR("PetroleumColor"))
            ddlPetroleumSourceContainer.SelectedValue = HelpFunction.Convertdbnulls(objDR("PetroleumSourceContainer"))
            txtDiameterPipeline.Text = HelpFunction.Convertdbnulls(objDR("DiameterPipeline"))
            txtUnbrokenEndPipeConnectedTo.Text = HelpFunction.Convertdbnulls(objDR("UnbrokenEndPipeConnectedTo"))
            txtTotalSourceContainerVolume.Text = HelpFunction.Convertdbnulls(objDR("TotalSourceContainerVolume"))
            txtPetroleumQuantityReleased.Text = HelpFunction.Convertdbnulls(objDR("PetroleumQuantityReleased"))
            txtPetroleumRateOfRelease.Text = HelpFunction.Convertdbnulls(objDR("PetroleumRateOfRelease"))
            txtPetroleumCauseOfRelease.Text = HelpFunction.Convertdbnulls(objDR("PetroleumCauseOfRelease"))
            ddlPetroleumlReleased.SelectedValue = HelpFunction.Convertdbnulls(objDR("PetroleumlReleased"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR("TimeReleaseDiscovered")))
            localTime2 = CStr(HelpFunction.Convertdbnulls(objDR("TimeReleaseSecured")))
            ddlStormDrainsAffected.SelectedValue = HelpFunction.Convertdbnulls(objDR("StormDrainsAffected"))
            ddlWaterwaysAffected.SelectedValue = HelpFunction.Convertdbnulls(objDR("WaterwaysAffected"))
            txtWaterwaysAffectedText.Text = HelpFunction.Convertdbnulls(objDR("WaterwaysAffectedText"))
            ddlMajorRoadwaysClosed.SelectedValue = HelpFunction.Convertdbnulls(objDR("MajorRoadwaysClosed"))
            ddlCleanupActionsTaken.SelectedValue = HelpFunction.Convertdbnulls(objDR("CleanupActionsTaken"))
            txtCleanupActionsTakenText.Text = HelpFunction.Convertdbnulls(objDR("CleanupActionsTakenText"))
            txtConductingCleanup.Text = HelpFunction.Convertdbnulls(objDR("ConductingCleanup"))
            ddlCallbackDEPRequested.SelectedValue = HelpFunction.Convertdbnulls(objDR("CallbackDEPRequested"))
            ddlCallbackDEPRequestedValue.SelectedValue = HelpFunction.Convertdbnulls(objDR("CallbackDEPRequestedValue"))
            ddlNotification.SelectedValue = HelpFunction.Convertdbnulls(objDR("IncidentTypeLevelID"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()


        txtWorkSheetDescription.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))


        'Making the time All Morris Day like
        txtTimeReleaseDiscovered.Text = Left(localTime, 2)
        txtTimeReleaseDiscovered2.Text = Right(localTime, 2)

        txtTimeReleaseSecured.Text = Left(localTime2, 2)
        txtTimeReleaseSecured2.Text = Right(localTime2, 2)

        If txtTimeReleaseDiscovered.Text = "0" Then
            txtTimeReleaseDiscovered.Text = ""
        End If

        If txtTimeReleaseSecured.Text = "0" Then
            txtTimeReleaseSecured.Text = ""
        End If

        If txtTimeReleaseSecured2.Text = "0" Then
            txtTimeReleaseSecured2.Text = ""
        End If

        If txtTimeReleaseDiscovered2.Text = "0" Then
            txtTimeReleaseDiscovered2.Text = ""
        End If

        If ddlPetroleumSourceContainer.SelectedValue = "Aboveground Pipeline" Or ddlPetroleumSourceContainer.SelectedValue = "Underground Pipeline" Then
            pnlShowPipeline.Visible = True
        Else
            pnlShowPipeline.Visible = False
        End If

        If ddlWaterwaysAffected.SelectedValue = "Yes" Then
            pnlShowWaterwaysAffectedText.Visible = True
        Else
            pnlShowWaterwaysAffectedText.Visible = False
        End If

        If ddlCleanupActionsTaken.SelectedValue = "Yes" Then
            pnlShowCleanupActionsTaken.Visible = True
        Else
            pnlShowCleanupActionsTaken.Visible = False
        End If

        If ddlCallbackDEPRequested.SelectedValue = "Yes" Then
            'pnlShowCallbackDEPRequested.Visible = True
        Else
            pnlShowCallbackDEPRequested.Visible = False
        End If

    End Sub

    Protected Sub ddlSourceContainer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPetroleumSourceContainer.SelectedIndexChanged

        If ddlPetroleumSourceContainer.SelectedValue = "Aboveground Pipeline" Or ddlPetroleumSourceContainer.SelectedValue = "Underground Pipeline" Then
            pnlShowPipeline.Visible = True
        Else
            pnlShowPipeline.Visible = False
        End If

    End Sub

    Protected Sub ddlWaterwaysAffected_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWaterwaysAffected.SelectedIndexChanged

        If ddlWaterwaysAffected.SelectedValue = "Yes" Then
            pnlShowWaterwaysAffectedText.Visible = True
        Else
            pnlShowWaterwaysAffectedText.Visible = False
        End If

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

        Dim localPetroleumSpillCount As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectPetroleumSpillCountByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            localPetroleumSpillCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

        End If

        objDR.Close()

        objCmd.Dispose()

        objCmd = Nothing

        objConn.Close()


        If pnlShowPipeline.Visible = False Then

            txtDiameterPipeline.Text = ""
            txtUnbrokenEndPipeConnectedTo.Text = ""
        End If

        If pnlShowWaterwaysAffectedText.Visible = False Then
            txtWaterwaysAffectedText.Text = ""
        End If

        If pnlShowCleanupActionsTaken.Visible = False Then
            txtCleanupActionsTakenText.Text = ""
        End If

        If pnlShowCallbackDEPRequested.Visible = False Then
            ddlCallbackDEPRequestedValue.SelectedValue = "Select an Option"
        End If


        'Response.Write(localCivilDisturbanceCount)
        'Response.End()

        'Response.Write(pnlShowFatalityText.Visible.ToString)
        'Response.End()

        If localPetroleumSpillCount = 0 Then

            'Try

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionPetroleumSpill", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 0)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@PetroleumType", ddlPetroleumType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@PetroleumNameDescription", txtPetroleumNameDescription.Text)
            objCmd.Parameters.AddWithValue("@PetroleumOdor", txtPetroleumOdor.Text)
            objCmd.Parameters.AddWithValue("@PetroleumColor", txtPetroleumColor.Text)
            objCmd.Parameters.AddWithValue("@PetroleumSourceContainer", ddlPetroleumSourceContainer.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DiameterPipeline", txtDiameterPipeline.Text)
            objCmd.Parameters.AddWithValue("@UnbrokenEndPipeConnectedTo", txtUnbrokenEndPipeConnectedTo.Text)
            objCmd.Parameters.AddWithValue("@TotalSourceContainerVolume", txtTotalSourceContainerVolume.Text)
            objCmd.Parameters.AddWithValue("@PetroleumQuantityReleased", txtPetroleumQuantityReleased.Text)
            objCmd.Parameters.AddWithValue("@PetroleumRateOfRelease", txtPetroleumRateOfRelease.Text)
            objCmd.Parameters.AddWithValue("@PetroleumCauseOfRelease", txtPetroleumCauseOfRelease.Text)
            objCmd.Parameters.AddWithValue("@PetroleumlReleased ", ddlPetroleumlReleased.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@TimeReleaseDiscovered", CStr(txtTimeReleaseDiscovered.Text.Trim) & CStr(txtTimeReleaseDiscovered2.Text.Trim))
            objCmd.Parameters.AddWithValue("@TimeReleaseSecured", CStr(txtTimeReleaseSecured.Text.Trim) & CStr(txtTimeReleaseSecured2.Text.Trim))
            objCmd.Parameters.AddWithValue("@StormDrainsAffected ", ddlStormDrainsAffected.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WaterwaysAffected ", ddlWaterwaysAffected.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WaterwaysAffectedText", txtWaterwaysAffectedText.Text)
            objCmd.Parameters.AddWithValue("@MajorRoadwaysClosed", ddlMajorRoadwaysClosed.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CleanupActionsTaken", ddlCleanupActionsTaken.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CleanupActionsTakenText", txtCleanupActionsTakenText.Text)
            objCmd.Parameters.AddWithValue("@ConductingCleanup", txtConductingCleanup.Text)
            objCmd.Parameters.AddWithValue("@CallbackDEPRequested", ddlCallbackDEPRequested.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CallbackDEPRequestedValue", ddlCallbackDEPRequestedValue.SelectedValue.ToString)
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
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Added Petroleum Spill Incident Type")

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
            objCmd = New SqlCommand("spActionPetroleumSpill", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 1)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@PetroleumType", ddlPetroleumType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@PetroleumNameDescription", txtPetroleumNameDescription.Text)
            objCmd.Parameters.AddWithValue("@PetroleumOdor", txtPetroleumOdor.Text)
            objCmd.Parameters.AddWithValue("@PetroleumColor", txtPetroleumColor.Text)
            objCmd.Parameters.AddWithValue("@PetroleumSourceContainer", ddlPetroleumSourceContainer.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DiameterPipeline", txtDiameterPipeline.Text)
            objCmd.Parameters.AddWithValue("@UnbrokenEndPipeConnectedTo", txtUnbrokenEndPipeConnectedTo.Text)
            objCmd.Parameters.AddWithValue("@TotalSourceContainerVolume", txtTotalSourceContainerVolume.Text)
            objCmd.Parameters.AddWithValue("@PetroleumQuantityReleased", txtPetroleumQuantityReleased.Text)
            objCmd.Parameters.AddWithValue("@PetroleumRateOfRelease", txtPetroleumRateOfRelease.Text)
            objCmd.Parameters.AddWithValue("@PetroleumCauseOfRelease", txtPetroleumCauseOfRelease.Text)
            objCmd.Parameters.AddWithValue("@PetroleumlReleased ", ddlPetroleumlReleased.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@TimeReleaseDiscovered", CStr(txtTimeReleaseDiscovered.Text.Trim) & CStr(txtTimeReleaseDiscovered2.Text.Trim))
            objCmd.Parameters.AddWithValue("@TimeReleaseSecured", CStr(txtTimeReleaseSecured.Text.Trim) & CStr(txtTimeReleaseSecured2.Text.Trim))
            objCmd.Parameters.AddWithValue("@StormDrainsAffected ", ddlStormDrainsAffected.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WaterwaysAffected ", ddlWaterwaysAffected.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WaterwaysAffectedText", txtWaterwaysAffectedText.Text)
            objCmd.Parameters.AddWithValue("@MajorRoadwaysClosed", ddlMajorRoadwaysClosed.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CleanupActionsTaken", ddlCleanupActionsTaken.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CleanupActionsTakenText", txtCleanupActionsTakenText.Text)
            objCmd.Parameters.AddWithValue("@ConductingCleanup", txtConductingCleanup.Text)
            objCmd.Parameters.AddWithValue("@CallbackDEPRequested", ddlCallbackDEPRequested.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CallbackDEPRequestedValue", ddlCallbackDEPRequestedValue.SelectedValue.ToString)
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
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Updated Added Petroleum Spill Incident Type")

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


    Protected Sub ddlCleanupActionsTaken_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCleanupActionsTaken.SelectedIndexChanged

        If ddlCleanupActionsTaken.SelectedValue = "Yes" Then
            pnlShowCleanupActionsTaken.Visible = True
        Else
            pnlShowCleanupActionsTaken.Visible = False
        End If

    End Sub


    Protected Sub ddlCallbackDEPRequested_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCallbackDEPRequested.Load

        If ddlCallbackDEPRequested.SelectedValue = "Yes" Then
            'pnlShowCallbackDEPRequested.Visible = True
        Else
            pnlShowCallbackDEPRequested.Visible = False
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



        'Finish the Error String.
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels.
        lblMessage.Text = strError.ToString
    End Sub
End Class
