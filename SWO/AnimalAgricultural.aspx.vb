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

Partial Class AnimalAgricultural
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
        'btnCancel.Attributes.Add("onclick", "window.open('','_self');window.close();")
        'btnSave.Attributes.Add("onclick", "window.open('','_self');window.close();")

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        ''Add cookie.
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

            'Set message.
            globalMessage = Request("Message")
            globalAction = Request("Action")
            globalParameter = Request("Parameter")

            Dim localAnimalAgriculturalCount As Integer = 0

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectAnimalAgriculturalCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localAnimalAgriculturalCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            'Response.Write(localBombThreatDeviceCount)
            'Response.End()

            If localAnimalAgriculturalCount > 0 Then
                PopulatePage()
            End If
        End If
    End Sub

    Protected Sub PopulatePage()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectAnimalAgriculturalByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            ddlSubType.SelectedValue = HelpFunction.Convertdbnulls(objDR("SubType"))
            ddlSeverityLevel.SelectedValue = HelpFunction.Convertdbnulls(objDR("SeverityLevel"))
            txtAnimalAffected.Text = HelpFunction.Convertdbnulls(objDR("AnimalAffected"))
            txtAnimalDiseaseType.Text = HelpFunction.Convertdbnulls(objDR("AnimalDiseaseType"))
            txtAnimalInfected.Text = HelpFunction.Convertdbnulls(objDR("AnimalInfected"))
            txtAnimalTestExaminations.Text = HelpFunction.Convertdbnulls(objDR("AnimalTestExaminations"))
            txtAnimalsDeceased.Text = HelpFunction.Convertdbnulls(objDR("AnimalsDeceased"))
            ddlAnimalQuarantine.SelectedValue = HelpFunction.Convertdbnulls(objDR("AnimalQuarantine"))
            txtAnimalQuarantineText.Text = HelpFunction.Convertdbnulls(objDR("AnimalQuarantineText"))
            ddlAnimalHumansAffected.SelectedValue = HelpFunction.Convertdbnulls(objDR("AnimalHumansAffected"))
            txtAnimalHumansAffectedText.Text = HelpFunction.Convertdbnulls(objDR("AnimalHumansAffectedText"))
            ddlAnimalHumanFatalities.SelectedValue = HelpFunction.Convertdbnulls(objDR("AnimalHumanFatalities"))
            txtAnimalHumanFatalitiesText.Text = HelpFunction.Convertdbnulls(objDR("AnimalHumanFatalitiesText"))
            txtADCFcropsAffected.Text = HelpFunction.Convertdbnulls(objDR("ADCFcropsAffected"))
            txtADCFdiseaseType.Text = HelpFunction.Convertdbnulls(objDR("ADCFdiseaseType"))
            txtADCFacresAffected.Text = HelpFunction.Convertdbnulls(objDR("ADCFacresAffected"))
            txtFSCtypeBrand.Text = HelpFunction.Convertdbnulls(objDR("FSCtypeBrand"))
            txtFSCmanufacturedPacked.Text = HelpFunction.Convertdbnulls(objDR("FSCmanufacturedPacked"))
            txtFSCaffectedLotNumber.Text = HelpFunction.Convertdbnulls(objDR("FSCaffectedLotNumber"))
            txtFSCaffectedDateRange.Text = HelpFunction.Convertdbnulls(objDR("FSCaffectedDateRange"))
            txtFSCrecallIssued.Text = HelpFunction.Convertdbnulls(objDR("FSCrecallIssued"))
            ddlNotification.SelectedValue = HelpFunction.Convertdbnulls(objDR("IncidentTypeLevelID"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        txtWorkSheetDescription.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        If ddlAnimalQuarantine.SelectedValue = "Yes" Then
            pnlShowAnimalQuarantineText.Visible = True
        End If

        If ddlAnimalHumansAffected.SelectedValue = "Yes" Then
            pnlShowAnimalHumansAffectedText.Visible = True
        End If

        If ddlAnimalHumansAffected.SelectedValue = "Yes" Then
            pnlShowAnimalHumansAffectedText.Visible = True
        End If

        If ddlSubType.SelectedValue = "Animal Issue" Then
            pnlShowAnimal.Visible = True
        End If

        If ddlAnimalHumanFatalities.SelectedValue = "Yes" Then
            'pnlShowAnimalHumanFatalitiesText.Visible = True
        Else
            pnlShowAnimalHumanFatalitiesText.Visible = False
        End If

        If ddlSubType.SelectedValue = "Agriculture Issue" Or ddlSubType.SelectedValue = "Crop Issue" Then
            pnlAgriculturalDiseaseCropFailure.Visible = True
        End If

        If ddlSubType.SelectedValue = "Food Supply Issue" Then
            pnlFoodSupplyContamination.Visible = True
        End If
    End Sub

    Sub PopulateDDLs()
        'Notification Group.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectIncidentTypeLevelForDDL", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentTypeID", MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("IncidentTypeID", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID")))

        DBConStringHelper.PrepareConnection(objConn) 'Open the connection
        ddlNotification.DataSource = objCmd.ExecuteReader()
        ddlNotification.DataBind()
        DBConStringHelper.FinalizeConnection(objConn) 'Close the connection

        objCmd = Nothing

        'Add an "Select an Option" item to the list.
        ddlNotification.Items.Insert(0, New ListItem("Select an Option", "Select an Option"))
        ddlNotification.Items(0).Selected = True
    End Sub

    Protected Sub ddlSubType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubType.SelectedIndexChanged
        If ddlSubType.SelectedValue = "Animal Issue" Then
            pnlShowAnimal.Visible = True
            pnlAgriculturalDiseaseCropFailure.Visible = False
            pnlFoodSupplyContamination.Visible = False
        ElseIf ddlSubType.SelectedValue = "Agriculture Issue" Or ddlSubType.SelectedValue = "Crop Issue" Then
            pnlAgriculturalDiseaseCropFailure.Visible = True
            pnlShowAnimal.Visible = False
            pnlFoodSupplyContamination.Visible = False
        ElseIf ddlSubType.SelectedValue = "Food Supply Issue" Then
            pnlFoodSupplyContamination.Visible = True
            pnlShowAnimal.Visible = False
            pnlAgriculturalDiseaseCropFailure.Visible = False
        Else
            pnlShowAnimal.Visible = False
            pnlAgriculturalDiseaseCropFailure.Visible = False
            pnlFoodSupplyContamination.Visible = False
        End If
    End Sub

    Protected Sub ddlAnimalQuarantine_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAnimalQuarantine.SelectedIndexChanged
        If ddlAnimalQuarantine.SelectedValue = "Yes" Then
            pnlShowAnimalQuarantineText.Visible = True
        Else
            pnlShowAnimalQuarantineText.Visible = False
        End If
    End Sub

    Protected Sub ddlAnimalHumansAffected_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAnimalHumansAffected.SelectedIndexChanged
        If ddlAnimalHumansAffected.SelectedValue = "Yes" Then
            pnlShowAnimalHumansAffectedText.Visible = True
        Else
            pnlShowAnimalHumansAffectedText.Visible = False
        End If
    End Sub

    Protected Sub ddlAnimalHumanFatalities_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAnimalHumanFatalities.SelectedIndexChanged
        If ddlAnimalHumanFatalities.SelectedValue = "Yes" Then
            pnlShowAnimalHumanFatalitiesText.Visible = True
        Else
            pnlShowAnimalHumanFatalitiesText.Visible = False
        End If
    End Sub

    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As EventArgs)
        'Response.Write(ddlNotification.SelectedValue)
        'Response.Write("<br>")
        'Response.Write(ddlNotification.SelectedItem)
        'Response.Write("<br>")
        'Response.End()

        ErrorChecks()

        If globalHasErrors = False Then
            Save()

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        'Response.Write(ddlNotification.SelectedValue)
        'Response.Write("<br>")
        'Response.Write(ddlNotification.SelectedItem)
        'Response.Write("<br>")
        'Response.End()

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)

        'btnCancel.Attributes.Add("onclick", "window.open('','_self');window.close();")
        'btnSave.Attributes.Add("onclick", "window.open('','_self');window.close();")

        'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID") & "&Parameter=WorkSheet")
    End Sub

    Protected Sub Save()
        'Set message.
        globalMessage = Request("Message")
        globalAction = Request("Action")
        globalParameter = Request("Parameter")

        Dim localAnimalAgriculturalCount As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectAnimalAgriculturalCountByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            localAnimalAgriculturalCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        'We add these to blank since the panels are not visible.
        If pnlShowAnimalQuarantineText.Visible = False Then
            txtAnimalQuarantineText.Text = ""
        End If

        If pnlShowAnimalHumansAffectedText.Visible = False Then
            txtAnimalHumansAffectedText.Text = ""
        End If

        If pnlShowAnimalHumanFatalitiesText.Visible = False Then
            txtAnimalHumanFatalitiesText.Text = ""
        End If

        If pnlShowAnimal.Visible = False Then
            txtAnimalQuarantineText.Text = ""
            txtAnimalHumansAffectedText.Text = ""
            txtAnimalHumanFatalitiesText.Text = ""
            txtAnimalAffected.Text = ""
            txtAnimalDiseaseType.Text = ""
            txtAnimalInfected.Text = ""
            txtAnimalsDeceased.Text = ""
            txtAnimalTestExaminations.Text = ""
            ddlAnimalQuarantine.SelectedValue = "Select an Option"
            ddlAnimalHumansAffected.SelectedValue = "Select an Option"
            ddlAnimalHumanFatalities.SelectedValue = "Select an Option"
        End If

        If pnlAgriculturalDiseaseCropFailure.Visible = False Then
            txtADCFcropsAffected.Text = ""
            txtADCFdiseaseType.Text = ""
            txtADCFacresAffected.Text = ""
        End If

        If pnlFoodSupplyContamination.Visible = False Then
            txtFSCtypeBrand.Text = ""
            txtFSCmanufacturedPacked.Text = ""
            txtFSCaffectedLotNumber.Text = ""
            txtFSCaffectedDateRange.Text = ""
            txtFSCrecallIssued.Text = ""
        End If

        'Response.Write(ddlNotification.SelectedValue)
        'Response.Write("<br>")
        'Response.Write(ddlNotification.SelectedItem)
        'Response.Write("<br>")
        'Response.End()

        If localAnimalAgriculturalCount = 0 Then
            'Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionAnimalAgricultural", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 0)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@SeverityLevel", ddlSeverityLevel.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@AnimalAffected", txtAnimalAffected.Text)
            objCmd.Parameters.AddWithValue("@AnimalDiseaseType", txtAnimalDiseaseType.Text)
            objCmd.Parameters.AddWithValue("@AnimalInfected", txtAnimalInfected.Text)
            objCmd.Parameters.AddWithValue("@AnimalsDeceased", txtAnimalsDeceased.Text)
            objCmd.Parameters.AddWithValue("@AnimalTestExaminations", txtAnimalTestExaminations.Text)
            objCmd.Parameters.AddWithValue("@AnimalQuarantine", ddlAnimalQuarantine.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@AnimalQuarantineText", txtAnimalQuarantineText.Text)
            objCmd.Parameters.AddWithValue("@AnimalHumansAffected", ddlAnimalHumansAffected.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@AnimalHumansAffectedText", txtAnimalHumansAffectedText.Text)
            objCmd.Parameters.AddWithValue("@AnimalHumanFatalities", ddlAnimalHumanFatalities.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@AnimalHumanFatalitiesText", txtAnimalHumanFatalitiesText.Text)
            objCmd.Parameters.AddWithValue("@ADCFcropsAffected", txtADCFcropsAffected.Text)
            objCmd.Parameters.AddWithValue("@ADCFdiseaseType", txtADCFdiseaseType.Text)
            objCmd.Parameters.AddWithValue("@ADCFacresAffected", txtADCFacresAffected.Text)
            objCmd.Parameters.AddWithValue("@FSCtypeBrand", txtFSCtypeBrand.Text)
            objCmd.Parameters.AddWithValue("@FSCmanufacturedPacked", txtFSCmanufacturedPacked.Text)
            objCmd.Parameters.AddWithValue("@FSCaffectedLotNumber", txtFSCaffectedLotNumber.Text)
            objCmd.Parameters.AddWithValue("@FSCaffectedDateRange", txtFSCaffectedDateRange.Text)
            objCmd.Parameters.AddWithValue("@FSCrecallIssued", txtFSCrecallIssued.Text)
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

                'Enter the email and password to query/command object.
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

            AuditHelper.InsertReportUpdate(Request("IncidentID"), "Saved Initial Information for Animal or Agricultural Worksheet: " & txtWorkSheetDescription.Text, ns.UserID) 'oCookie.Item("UserID"))

            'Try
            '    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '    'Enter the email and password to query/command object.
            '    objCmd = New SqlCommand("spInsertMostRecentUpdateByIncidentID", objConn)
            '    objCmd.CommandType = CommandType.StoredProcedure
            '    objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            '    objCmd.Parameters.AddWithValue("@UpdateDate", NowDate)
            '    objCmd.Parameters.AddWithValue("@UserID", oCookie.Item("UserID"))
            '    objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Added Animal or Agricultural Issue")

            '    DBConStringHelper.PrepareConnection(objConn)

            '    objCmd.ExecuteNonQuery()

            '    objCmd.Dispose()
            '    objCmd = Nothing
            '    DBConStringHelper.FinalizeConnection(objConn)
            'Catch ex As Exception
            '    Response.Write(ex.ToString)
            '    Exit Sub
            'End Try

            'Try
            '    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '    'Enter the email and password to query/command object.
            '    objCmd = New SqlCommand("spUpdateIncidentReportUpdate", objConn)
            '    objCmd.CommandType = CommandType.StoredProcedure
            '    objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            '    objCmd.Parameters.AddWithValue("@LastUpdatedByID", oCookie.Item("UserID"))
            '    objCmd.Parameters.AddWithValue("@LastUpdated", NowDate)

            '    DBConStringHelper.PrepareConnection(objConn)

            '    objCmd.ExecuteNonQuery()

            '    objCmd.Dispose()
            '    objCmd = Nothing
            '    DBConStringHelper.FinalizeConnection(objConn)
            'Catch ex As Exception
            '    Response.Write(ex.ToString)
            '    Exit Sub
            'End Try
        Else
            'Response.Write("Its Working!")
            'Response.End()

            'Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionAnimalAgricultural", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 1)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@SeverityLevel", ddlSeverityLevel.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@AnimalAffected", txtAnimalAffected.Text)
            objCmd.Parameters.AddWithValue("@AnimalDiseaseType", txtAnimalDiseaseType.Text)
            objCmd.Parameters.AddWithValue("@AnimalInfected", txtAnimalInfected.Text)
            objCmd.Parameters.AddWithValue("@AnimalsDeceased", txtAnimalsDeceased.Text)
            objCmd.Parameters.AddWithValue("@AnimalTestExaminations", txtAnimalTestExaminations.Text)
            objCmd.Parameters.AddWithValue("@AnimalQuarantine", ddlAnimalQuarantine.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@AnimalQuarantineText", txtAnimalQuarantineText.Text)
            objCmd.Parameters.AddWithValue("@AnimalHumansAffected", ddlAnimalHumansAffected.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@AnimalHumansAffectedText", txtAnimalHumansAffectedText.Text)
            objCmd.Parameters.AddWithValue("@AnimalHumanFatalities", ddlAnimalHumanFatalities.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@AnimalHumanFatalitiesText", txtAnimalHumanFatalitiesText.Text)
            objCmd.Parameters.AddWithValue("@ADCFcropsAffected", txtADCFcropsAffected.Text)
            objCmd.Parameters.AddWithValue("@ADCFdiseaseType", txtADCFdiseaseType.Text)
            objCmd.Parameters.AddWithValue("@ADCFacresAffected", txtADCFacresAffected.Text)
            objCmd.Parameters.AddWithValue("@FSCtypeBrand", txtFSCtypeBrand.Text)
            objCmd.Parameters.AddWithValue("@FSCmanufacturedPacked", txtFSCmanufacturedPacked.Text)
            objCmd.Parameters.AddWithValue("@FSCaffectedLotNumber", txtFSCaffectedLotNumber.Text)
            objCmd.Parameters.AddWithValue("@FSCaffectedDateRange", txtFSCaffectedDateRange.Text)
            objCmd.Parameters.AddWithValue("@FSCrecallIssued", txtFSCrecallIssued.Text)
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

                'Enter the email and password to query/command object.
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

            Dim localCurrentWorkSheetDescription As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

            If localCurrentWorkSheetDescription <> txtWorkSheetDescription.Text Then
                AuditHelper.InsertReportUpdate(Request("IncidentID"), "Changed Animal or Agricultural Worksheet Description From: " & localCurrentWorkSheetDescription & " To: " & txtWorkSheetDescription.Text, ns.UserID) 'oCookie.Item("UserID"))
            End If

            AuditHelper.InsertReportUpdate(Request("IncidentID"), "Updated Information for Animal or Agricultural Worksheet: " & txtWorkSheetDescription.Text, ns.UserID) 'oCookie.Item("UserID"))

            'Try
            '    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '    'Enter the email and password to query/command object.
            '    objCmd = New SqlCommand("spInsertMostRecentUpdateByIncidentID", objConn)
            '    objCmd.CommandType = CommandType.StoredProcedure
            '    objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            '    objCmd.Parameters.AddWithValue("@UpdateDate", NowDate)
            '    objCmd.Parameters.AddWithValue("@UserID", oCookie.Item("UserID"))
            '    objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Updated Animal or Agricultural Issue")

            '    DBConStringHelper.PrepareConnection(objConn)

            '    objCmd.ExecuteNonQuery()

            '    objCmd.Dispose()
            '    objCmd = Nothing
            '    DBConStringHelper.FinalizeConnection(objConn)
            'Catch ex As Exception
            '    Response.Write(ex.ToString)
            '    Exit Sub
            'End Try

            'Try
            '    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '    'Enter the email and password to query/command object.
            '    objCmd = New SqlCommand("spUpdateIncidentReportUpdate", objConn)
            '    objCmd.CommandType = CommandType.StoredProcedure
            '    objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            '    objCmd.Parameters.AddWithValue("@LastUpdatedByID", oCookie.Item("UserID"))
            '    objCmd.Parameters.AddWithValue("@LastUpdated", NowDate)

            '    DBConStringHelper.PrepareConnection(objConn)

            '    objCmd.ExecuteNonQuery()

            '    objCmd.Dispose()
            '    objCmd = Nothing
            '    DBConStringHelper.FinalizeConnection(objConn)
            'Catch ex As Exception
            '    Response.Write(ex.ToString)
            '    Exit Sub
            'End Try
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

        If ddlSeverityLevel.SelectedValue = "Select an Option" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Severity Level. <br />")
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

        If ddlSubType.SelectedValue = "Animal Issue" Then
            If txtAnimalAffected.Text = "" Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: What animal(s) are affected? <br />")
                globalHasErrors = True
            End If

            'If txtAnimalDiseaseType.Text = "" Then
            '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: What type of disease, if known? <br />")
            '    globalHasErrors = True
            'End If

            'If txtAnimalInfected.Text = "" Then
            '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Number of animals infected? <br />")
            '    globalHasErrors = True
            'End If

            'If txtAnimalsDeceased.Text = "" Then
            '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Number of animals deceased? <br />")
            '    globalHasErrors = True
            'End If

            'If txtAnimalTestExaminations.Text = "" Then
            '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Tests or examinations are planned or occuring? <br />")
            '    globalHasErrors = True
            'End If

            If ddlAnimalQuarantine.SelectedValue = "Select an Option" Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Is there a quarantine in effect? <br />")
                globalHasErrors = True
            End If

            'If ddlAnimalQuarantine.SelectedValue = "Yes" Then
            '    If txtAnimalQuarantineText.Text = "" Then
            '        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Describe area, listing streets or landmarks: <br />")
            '        globalHasErrors = True
            '    End If
            'End If

            If ddlAnimalHumansAffected.SelectedValue = "Select an Option" Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Are any humans affected? <br />")
                globalHasErrors = True
            End If

            'If ddlAnimalHumansAffected.SelectedValue = "Yes" Then
            '    If txtAnimalHumansAffectedText.Text = "" Then
            '        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Number and Severity of Illness: <br />")
            '        globalHasErrors = True
            '    End If
            'End If

            'If ddlAnimalHumanFatalities.SelectedValue = "Select an Option" Then
            '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Are there any human fatalities? <br />")
            '    globalHasErrors = True
            'End If


            'If ddlAnimalHumanFatalities.SelectedValue = "Yes" Then
            '    If txtAnimalHumanFatalitiesText.Text = "" Then
            '        strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: (AnimalHumanFatalities) Number and Information:<br />")
            '        globalHasErrors = True
            '    End If
            'End If
        ElseIf ddlSubType.SelectedValue = "Agriculture Issue" Or ddlSubType.SelectedValue = "Crop Issue" Then
            If txtADCFcropsAffected.Text = "" Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: What crop(s) are affected? <br />")
                globalHasErrors = True
            End If

            'If txtADCFdiseaseType.Text = "" Then
            '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: What type of disease, if known? <br />")
            '    globalHasErrors = True
            'End If

            If txtADCFacresAffected.Text = "" Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Number of acres affected? <br />")
                globalHasErrors = True
            End If
        ElseIf ddlSubType.SelectedValue = "Food Supply Issue" Then
            If txtFSCtypeBrand.Text = "" Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: What type / brand of food? <br />")
                globalHasErrors = True
            End If

            If txtFSCmanufacturedPacked.Text = "" Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Where was it manufactured/packed? <br />")
                globalHasErrors = True
            End If

            'If txtFSCaffectedLotNumber.Text = "" Then
            '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Affected lot number(s)? <br />")
            '    globalHasErrors = True
            'End If

            'If txtFSCaffectedDateRange.Text = "" Then
            '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Affected date range? <br />")
            '    globalHasErrors = True
            'End If

            'If txtFSCrecallIssued.Text = "" Then
            '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a value for: Has a recall been issued? <br />")
            '    globalHasErrors = True
            'End If
        End If

        'Adding the appropriate errors to the error string.
        'If txtComments.Text = "" Then
        '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Exempt. <br />")
        '    globalHasErrors = True
        'End If

        'If ddlIncidentType.SelectedValue = "Select an Incident Type" Then
        '    strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide an Incident Type. <br />")
        '    globalHasErrors = True
        'End If

        'Finish the Error String.
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels.
        lblMessage.Text = strError.ToString
    End Sub
End Class
