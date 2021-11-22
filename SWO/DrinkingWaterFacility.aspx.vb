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

Partial Class DrinkingWaterFacility
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

        If Page.IsPostBack = False Then

            'set message
            globalMessage = Request("Message")
            globalAction = Request("Action")
            globalParameter = Request("Parameter")

            PopulateDDLs()

            Dim localDrinkingWaterFacilityCount As Integer = 0

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectDrinkingWaterFacilityCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                localDrinkingWaterFacilityCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            'Response.Write(localBombThreatDeviceCount)
            'Response.End()

            If localDrinkingWaterFacilityCount > 0 Then
                PopulatePage()
            End If

        End If
    End Sub

    'PagePopulation
    Protected Sub PopulatePage()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectDrinkingWaterFacilityByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            ddlSubType.SelectedValue = HelpFunction.Convertdbnulls(objDR("SubType"))
            ddlSituation.SelectedValue = HelpFunction.Convertdbnulls(objDR("Situation"))
            txtPublicWaterSystemIDNumber.Text = HelpFunction.Convertdbnulls(objDR("PublicWaterSystemIDNumber"))
            txtFacilityName.Text = HelpFunction.Convertdbnulls(objDR("FacilityName"))
            ddlTrespassVandalismTheft.SelectedValue = HelpFunction.Convertdbnulls(objDR("TrespassVandalismTheft"))
            txtTrespassVandalismTheftText.Text = HelpFunction.Convertdbnulls(objDR("TrespassVandalismTheftText"))
            ddlDamageFacilityDistibutionSystem.SelectedValue = HelpFunction.Convertdbnulls(objDR("DamageFacilityDistibutionSystem"))
            txtDFDSintentional.Text = HelpFunction.Convertdbnulls(objDR("DFDSintentional"))
            ddlAccessWaterSupply.SelectedValue = HelpFunction.Convertdbnulls(objDR("AccessWaterSupply"))
            ddlDegredation.SelectedValue = HelpFunction.Convertdbnulls(objDR("Degredation"))
            txtIndividualResponsible.Text = HelpFunction.Convertdbnulls(objDR("IndividualResponsible"))
            ddlLawEnforcementContacted.SelectedValue = HelpFunction.Convertdbnulls(objDR("LawEnforcementContacted"))
            txtIndividualResponsibleCaseNumber.Text = HelpFunction.Convertdbnulls(objDR("IndividualResponsibleCaseNumber"))
            txtBWpublicWaterSystemIDNumber.Text = HelpFunction.Convertdbnulls(objDR("BWpublicWaterSystemIDNumber"))
            ddlBWIncidentDueTo.SelectedValue = HelpFunction.Convertdbnulls(objDR("BWIncidentDueTo"))
            txtBWnumberCustomersAffected.Text = HelpFunction.Convertdbnulls(objDR("BWnumberCustomersAffected"))
            txtBWaffectedAreas.Text = HelpFunction.Convertdbnulls(objDR("BWaffectedAreas"))
            ddlNotification.SelectedValue = HelpFunction.Convertdbnulls(objDR("IncidentTypeLevelID"))
            txtFWpublicWaterSystemIDNumber.Text = HelpFunction.Convertdbnulls(objDR("FWpublicWaterSystemIDNumber"))
            txtFWnumberCustomersAffected.Text = HelpFunction.Convertdbnulls(objDR("FWnumberCustomersAffected"))
            txtFWutilityName.Text = HelpFunction.Convertdbnulls(objDR("FWutilityName"))
            txtFWcauseForNeed.Text = HelpFunction.Convertdbnulls(objDR("FWcauseForNeed"))
            txtFWdurationOfNeed.Text = HelpFunction.Convertdbnulls(objDR("FWdurationOfNeed"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        txtWorkSheetDescription.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        '=============================================================================
        If ddlSubType.SelectedValue = "DWF Report" Then
            pnlShowDWFReport.Visible = True
        Else
            pnlShowDWFReport.Visible = False
        End If
        '=============================================================================
        If ddlSubType.SelectedValue = "Boil Water Advisory" Then
            pnlShowBoilWaterAdvisory.Visible = True
        Else
            pnlShowBoilWaterAdvisory.Visible = False
        End If
        '=============================================================================
        If ddlSubType.SelectedValue = "FlaWARN Generator Deployment" Then
            pnlShowFlaWARN.Visible = True
        Else
            pnlShowFlaWARN.Visible = False
        End If
        '=============================================================================
        If ddlTrespassVandalismTheft.SelectedValue = "Yes" Then
            pnlShowtxtTrespassVandalismTheftText.Visible = True
        Else
            pnlShowtxtTrespassVandalismTheftText.Visible = False
        End If
        '=============================================================================
        If ddlTrespassVandalismTheft.SelectedValue = "Yes" Then
            pnlShowtxtTrespassVandalismTheftText.Visible = True
        Else
            pnlShowtxtTrespassVandalismTheftText.Visible = False
        End If
        '=============================================================================
        If ddlLawEnforcementContacted.SelectedValue = "Yes" Then
            pnlShowEnforcementContactedCaseNumber.Visible = True
        Else
            pnlShowEnforcementContactedCaseNumber.Visible = False
        End If
        '=============================================================================


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

        'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID") & "&Parameter=WorkSheet")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)

    End Sub

    Protected Sub Save()

        Dim localDrinkingWaterFacilityCount As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectDrinkingWaterFacilityCountByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            localDrinkingWaterFacilityCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()


        'We add these to blank since the panels are not visible


        If pnlShowDWFReport.Visible = False Then

            txtPublicWaterSystemIDNumber.Text = ""
            txtFacilityName.Text = ""
            ddlTrespassVandalismTheft.SelectedValue = "Select an Option"
            txtTrespassVandalismTheftText.Text = ""
            ddlDamageFacilityDistibutionSystem.SelectedValue = "Select an Option"
            txtDFDSintentional.Text = ""
            ddlAccessWaterSupply.SelectedValue = "Select an Option"
            ddlDegredation.SelectedValue = "Select an Option"
            txtIndividualResponsible.Text = ""
            ddlLawEnforcementContacted.SelectedValue = "Select an Option"
            txtIndividualResponsibleCaseNumber.Text = ""

        End If

        If pnlShowBoilWaterAdvisory.Visible = False Then

            txtBWpublicWaterSystemIDNumber.Text = ""
            ddlBWIncidentDueTo.SelectedValue = "Select an Option"
            txtBWnumberCustomersAffected.Text = ""
            txtBWaffectedAreas.Text = ""

        End If

        If pnlShowFlaWARN.Visible = False Then
            txtFWcauseForNeed.Text = ""
            txtFWdurationOfNeed.Text = ""
            txtFWnumberCustomersAffected.Text = ""
            txtFWpublicWaterSystemIDNumber.Text = ""
            txtFWutilityName.Text = ""
        End If

        If pnlShowtxtTrespassVandalismTheftText.Visible = False Then
            txtTrespassVandalismTheftText.Text = ""
        End If

        If pnlShowIntentional.Visible = False Then
            txtDFDSintentional.Text = ""
        End If

        If pnlShowEnforcementContactedCaseNumber.Visible = False Then
            txtIndividualResponsibleCaseNumber.Text = ""
        End If

        If localDrinkingWaterFacilityCount = 0 Then

            'Try

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionDrinkingWaterFacility", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 0)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@PublicWaterSystemIDNumber", txtPublicWaterSystemIDNumber.Text)
            objCmd.Parameters.AddWithValue("@FacilityName", txtFacilityName.Text)
            objCmd.Parameters.AddWithValue("@TrespassVandalismTheft", ddlTrespassVandalismTheft.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@TrespassVandalismTheftText", txtTrespassVandalismTheftText.Text)
            objCmd.Parameters.AddWithValue("@DamageFacilityDistibutionSystem", ddlDamageFacilityDistibutionSystem.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DFDSintentional", txtDFDSintentional.Text)
            objCmd.Parameters.AddWithValue("@AccessWaterSupply", ddlAccessWaterSupply.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Degredation", ddlDegredation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@IndividualResponsible", txtIndividualResponsible.Text)
            objCmd.Parameters.AddWithValue("@LawEnforcementContacted", ddlLawEnforcementContacted.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@IndividualResponsibleCaseNumber", txtIndividualResponsibleCaseNumber.Text)
            objCmd.Parameters.AddWithValue("@BWpublicWaterSystemIDNumber", txtBWpublicWaterSystemIDNumber.Text)
            objCmd.Parameters.AddWithValue("@BWIncidentDueTo", ddlBWIncidentDueTo.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@BWnumberCustomersAffected", txtBWnumberCustomersAffected.Text)
            objCmd.Parameters.AddWithValue("@BWaffectedAreas", txtBWaffectedAreas.Text)
            objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", ddlNotification.SelectedValue)
            objCmd.Parameters.AddWithValue("@FWpublicWaterSystemIDNumber", txtFWpublicWaterSystemIDNumber.Text)
            objCmd.Parameters.AddWithValue("@FWnumberCustomersAffected", txtFWnumberCustomersAffected.Text)
            objCmd.Parameters.AddWithValue("@FWutilityName", txtFWutilityName.Text)
            objCmd.Parameters.AddWithValue("@FWcauseForNeed", txtFWcauseForNeed.Text)
            objCmd.Parameters.AddWithValue("@FWdurationOfNeed", txtFWdurationOfNeed.Text)


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
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Added Drinking Water Facility")

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
            objCmd = New SqlCommand("spActionDrinkingWaterFacility", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 1)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@PublicWaterSystemIDNumber", txtPublicWaterSystemIDNumber.Text)
            objCmd.Parameters.AddWithValue("@FacilityName", txtFacilityName.Text)
            objCmd.Parameters.AddWithValue("@TrespassVandalismTheft", ddlTrespassVandalismTheft.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@TrespassVandalismTheftText", txtTrespassVandalismTheftText.Text)
            objCmd.Parameters.AddWithValue("@DamageFacilityDistibutionSystem", ddlDamageFacilityDistibutionSystem.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@DFDSintentional", txtDFDSintentional.Text)
            objCmd.Parameters.AddWithValue("@AccessWaterSupply", ddlAccessWaterSupply.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Degredation", ddlDegredation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@IndividualResponsible", txtIndividualResponsible.Text)
            objCmd.Parameters.AddWithValue("@LawEnforcementContacted", ddlLawEnforcementContacted.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@IndividualResponsibleCaseNumber", txtIndividualResponsibleCaseNumber.Text)
            objCmd.Parameters.AddWithValue("@BWpublicWaterSystemIDNumber", txtBWpublicWaterSystemIDNumber.Text)
            objCmd.Parameters.AddWithValue("@BWIncidentDueTo", ddlBWIncidentDueTo.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@BWnumberCustomersAffected", txtBWnumberCustomersAffected.Text)
            objCmd.Parameters.AddWithValue("@BWaffectedAreas", txtBWaffectedAreas.Text)
            objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", ddlNotification.SelectedValue)
            objCmd.Parameters.AddWithValue("@FWpublicWaterSystemIDNumber", txtFWpublicWaterSystemIDNumber.Text)
            objCmd.Parameters.AddWithValue("@FWnumberCustomersAffected", txtFWnumberCustomersAffected.Text)
            objCmd.Parameters.AddWithValue("@FWutilityName", txtFWutilityName.Text)
            objCmd.Parameters.AddWithValue("@FWcauseForNeed", txtFWcauseForNeed.Text)
            objCmd.Parameters.AddWithValue("@FWdurationOfNeed", txtFWdurationOfNeed.Text)


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
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Updated Drinking Water Facility")

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

        If ddlSubType.SelectedValue = "DWF Report" Then
            pnlShowDWFReport.Visible = True
        Else
            pnlShowDWFReport.Visible = False
        End If

        If ddlSubType.SelectedValue = "Boil Water Advisory" Then
            pnlShowBoilWaterAdvisory.Visible = True
        Else
            pnlShowBoilWaterAdvisory.Visible = False
        End If

        If ddlSubType.SelectedValue = "FlaWARN Generator Deployment" Then
            pnlShowFlaWARN.Visible = True
        Else
            pnlShowFlaWARN.Visible = False
        End If

    End Sub

    Protected Sub ddlTrespassVandalismTheft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTrespassVandalismTheft.SelectedIndexChanged

        If ddlTrespassVandalismTheft.SelectedValue = "Yes" Then
            pnlShowtxtTrespassVandalismTheftText.Visible = True
        Else
            pnlShowtxtTrespassVandalismTheftText.Visible = False
        End If

    End Sub

    Protected Sub ddlDamageFacilityDistibutionSystem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDamageFacilityDistibutionSystem.SelectedIndexChanged

        If ddlDamageFacilityDistibutionSystem.SelectedValue = "Yes" Then
            pnlShowIntentional.Visible = True
        Else
            pnlShowIntentional.Visible = False
        End If

    End Sub

    Protected Sub ddlLawEnforcementContacted_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLawEnforcementContacted.SelectedIndexChanged

        If ddlLawEnforcementContacted.SelectedValue = "Yes" Then
            pnlShowEnforcementContactedCaseNumber.Visible = True
        Else
            pnlShowEnforcementContactedCaseNumber.Visible = False
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
