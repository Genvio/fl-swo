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

Partial Class WeatherReports
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
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        Select Case ns.UserLevelID.ToString() 'oCookie.Item("UserLevelID")
            Case "1" 'Admin.

            Case "2" 'Full user.

            Case "3" 'Update user.
                btnSave.Disabled = True
            Case "4", "5" 'Read Only and Read Only + Hazmat.
                btnSave.Disabled = True
            Case Else

        End Select

        If Page.IsPostBack = False Then
            'Set message.
            globalMessage = Request("Message")
            globalAction = Request("Action")
            globalParameter = Request("Parameter")

            PopulateDDLs()
            getWeatherMap()
            getWeatherLink()

            Select Case globalAction
                Case "Delete"
                    If ns.UserLevelID = "1" Then
                        Select Case globalParameter
                            Case "WeatherMap"
                                DeleteWeatherMap()
                            Case "WeatherLink"
                                DeleteWeatherLink()
                        End Select
                    End If
                Case Else
            End Select

            Dim localWeatherCount As Integer = 0

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectWeatherCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localWeatherCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            If localWeatherCount > 0 Then
                PopulatePage()
            End If
        End If
    End Sub

    Protected Sub PopulatePage()
        Dim localTime2 As String = ""

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectWeatherByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            ddlSubType.SelectedValue = HelpFunction.Convertdbnulls(objDR("SubType"))
            ddlSituation.SelectedValue = HelpFunction.Convertdbnulls(objDR("Situation"))
            ddlLSRreportType.SelectedValue = HelpFunction.Convertdbnulls(objDR("LSRreportType"))
            ddlLSRreportReceived.SelectedValue = HelpFunction.Convertdbnulls(objDR("LSRreportReceived"))
            ddlLSRInjury.SelectedValue = HelpFunction.Convertdbnulls(objDR("LSRInjury"))
            txtLSRInjuryText.Text = HelpFunction.Convertdbnulls(objDR("LSRInjuryText"))
            ddlLSRFatality.SelectedValue = HelpFunction.Convertdbnulls(objDR("LSRFatality"))
            txtLSRFatalityText.Text = HelpFunction.Convertdbnulls(objDR("LSRFatalityText"))
            ddlLSRdisplacement.SelectedValue = HelpFunction.Convertdbnulls(objDR("LSRdisplacement"))
            txtLSRdisplacementText.Text = HelpFunction.Convertdbnulls(objDR("LSRdisplacementText"))
            ddlLSRdamageStructures.SelectedValue = HelpFunction.Convertdbnulls(objDR("LSRdamageStructures"))
            txtLSRdamageStructuresText.Text = HelpFunction.Convertdbnulls(objDR("LSRdamageStructuresText"))
            ddlLSRinfrastructureDamage.SelectedValue = HelpFunction.Convertdbnulls(objDR("LSRinfrastructureDamage"))
            txtLSRinfrastructureDamageText.Text = HelpFunction.Convertdbnulls(objDR("LSRinfrastructureDamageText"))
            txtTOtransmitter.Text = HelpFunction.Convertdbnulls(objDR("TOtransmitter"))
            txtTOmakingNotification.Text = HelpFunction.Convertdbnulls(objDR("TOmakingNotification"))
            localTime2 = CStr(HelpFunction.Convertdbnulls(objDR("TOserviceOutTime")))
            txtTOserviceOutDate.Text = HelpFunction.Convertdbnulls(objDR("TOserviceOutDate"))
            txtTOtransmitterServiceDueTo.Text = HelpFunction.Convertdbnulls(objDR("TOtransmitterServiceDueTo"))
            txtTOreturnToService.Text = HelpFunction.Convertdbnulls(objDR("TOreturnToService"))
            ddlNotification.SelectedValue = HelpFunction.Convertdbnulls(objDR("IncidentTypeLevelID"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        txtWorkSheetDescription.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        If ddlSubType.SelectedValue = "Local Storm Report" Then
            pnlShowLocalStormReport.Visible = True
        Else
            pnlShowLocalStormReport.Visible = False
        End If

        If ddlSubType.SelectedValue = "NOAA Transnsmitter Outage" Then
            pnlShowTransmitterOutage.Visible = True
        Else
            pnlShowTransmitterOutage.Visible = False
        End If

        txtTOserviceOutTime.Text = Left(localTime2, 2)
        txtTOserviceOutTime2.Text = Right(localTime2, 2)

        If txtTOserviceOutDate.Text = "1/1/1900" Then
            txtTOserviceOutDate.Text = ""
        End If

        If ddlLSRdisplacement.SelectedValue = "Yes" Then
            pnlShowLSRdisplacementText.Visible = True
        End If

        If ddlLSRdamageStructures.SelectedValue = "Yes" Then
            pnlShowLSRdamageStructuresText.Visible = True
        End If

        If ddlLSRinfrastructureDamage.SelectedValue = "Yes" Then
            txtLSRinfrastructureDamageText.Visible = True
        End If

        If ddlLSRInjury.SelectedValue = "Yes" Then
            pnlShowInjuryText.Visible = True
        End If

        If ddlLSRFatality.SelectedValue = "Yes" Then
            pnlShowFatalityText.Visible = True
        End If
    End Sub

    Sub PopulateDDLs()
        'Notification group.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectIncidentTypeLevelForDDL", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentTypeID", MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("IncidentTypeID", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID")))

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        ddlNotification.DataSource = objCmd.ExecuteReader()
        ddlNotification.DataBind()

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        objCmd = Nothing

        'Add a "Select an Option" item to the list.
        ddlNotification.Items.Insert(0, New ListItem("Select an Option", "Select an Option"))
        ddlNotification.Items(0).Selected = True
    End Sub

    Protected Sub getWeatherMap()
        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connecion.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("spSelectWeatherMap", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDS.Tables.Clear()

        'Bind our data.
        objDA = New SqlDataAdapter(objCmd)
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        'Call the calculate grid counts to show the number of records, the page you are on, etc.
        objDataGridFunctions.CalcDataGridCounts(objDS, dgWeatherMap, "")

        'Associate the data grid with the data.
        dgWeatherMap.DataSource = objDS.Tables(0).DefaultView
        dgWeatherMap.DataBind()

        objDataGridFunctions.Highlightrows(dgWeatherMap, "", "", "")
    End Sub

    Protected Sub getWeatherLink()
        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("spSelectWeatherLink", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDS.Tables.Clear()

        'Bind our data.
        objDA = New SqlDataAdapter(objCmd)
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing

        'CLose the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        'Call the calculate grid counts to show the number of records, the page you are on, etc.
        objDataGridFunctions.CalcDataGridCounts(objDS, dgWeatherLink, "")

        'Associate the data grid with the data.
        dgWeatherLink.DataSource = objDS.Tables(0).DefaultView
        dgWeatherLink.DataBind()

        objDataGridFunctions.Highlightrows(dgWeatherLink, "", "", "")
    End Sub

    Protected Sub Save()
        Dim localWeatherCount As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectWeatherCountByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            localWeatherCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        'We make these blank since the panels are not visible.
        If pnlShowInjuryText.Visible = False Then
            txtLSRInjuryText.Text = ""
        End If

        If pnlShowFatalityText.Visible = False Then
            txtLSRFatalityText.Text = ""
        End If

        If pnlShowLSRdisplacementText.Visible = False Then
            txtLSRdisplacementText.Text = ""
        End If

        If pnlShowLSRdamageStructuresText.Visible = False Then
            txtLSRdamageStructuresText.Text = ""
        End If

        If pnlShowLSRinfrastructureDamageText.Visible = False Then
            txtLSRinfrastructureDamageText.Text = ""
        End If

        If pnlShowLocalStormReport.Visible = False Then
            ddlLSRreportType.SelectedValue = "Select an Option"
            ddlLSRreportReceived.SelectedValue = "Select an Option"
            ddlLSRInjury.SelectedValue = "Select an Option"
            txtLSRInjuryText.Text = ""
            ddlLSRFatality.SelectedValue = "Select an Option"
            txtLSRFatalityText.Text = ""
            ddlLSRdisplacement.SelectedValue = "Select an Option"
            txtLSRdisplacementText.Text = ""
            ddlLSRdamageStructures.SelectedValue = "Select an Option"
            txtLSRdamageStructuresText.Text = ""
            ddlLSRinfrastructureDamage.SelectedValue = "Select an Option"
            txtLSRinfrastructureDamageText.Text = ""
        End If

        If pnlShowTransmitterOutage.Visible = False Then
            txtTOtransmitter.Text = ""
            txtTOmakingNotification.Text = ""
            txtTOserviceOutTime.Text = ""
            txtTOserviceOutTime2.Text = ""
            txtTOserviceOutDate.Text = ""
            txtTOreturnToService.Text = ""
        End If

        If localWeatherCount = 0 Then
            'Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionWeather", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 0)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWAdateIssued", "")
            objCmd.Parameters.AddWithValue("@WWAtime", "")
            objCmd.Parameters.AddWithValue("@WWAeffectiveDate ", "")
            objCmd.Parameters.AddWithValue("@WWAeffectiveTime", "")
            objCmd.Parameters.AddWithValue("@WWAexpiresDate", "")
            objCmd.Parameters.AddWithValue("@WWAexpiresTime", "")
            objCmd.Parameters.AddWithValue("@WWAissuingOffice", "")
            objCmd.Parameters.AddWithValue("@WWAadvisoryType", "")
            objCmd.Parameters.AddWithValue("@WWAadvisoryText", "")

            objCmd.Parameters.AddWithValue("@LSRreportType", ddlLSRreportType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@LSRreportReceived", ddlLSRreportReceived.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@LSRInjury", ddlLSRInjury.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@LSRInjuryText", txtLSRInjuryText.Text)
            objCmd.Parameters.AddWithValue("@LSRFatality", ddlLSRFatality.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@LSRFatalityText", txtLSRFatalityText.Text)
            objCmd.Parameters.AddWithValue("@LSRdisplacement", ddlLSRdisplacement.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@LSRdisplacementText", txtLSRdisplacementText.Text)
            objCmd.Parameters.AddWithValue("@LSRdamageStructures", ddlLSRdamageStructures.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@LSRdamageStructuresText", txtLSRdamageStructuresText.Text)
            objCmd.Parameters.AddWithValue("@LSRinfrastructureDamage", ddlLSRinfrastructureDamage.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@LSRinfrastructureDamageText", txtLSRinfrastructureDamageText.Text)
            objCmd.Parameters.AddWithValue("@TOtransmitter", txtTOtransmitter.Text)
            objCmd.Parameters.AddWithValue("@TOmakingNotification", txtTOmakingNotification.Text)
            objCmd.Parameters.AddWithValue("@TOserviceOutTime", CStr(txtTOserviceOutTime.Text.Trim) & CStr(txtTOserviceOutTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@TOserviceOutDate", txtTOserviceOutDate.Text)
            objCmd.Parameters.AddWithValue("@TOtransmitterServiceDueTo", txtTOtransmitterServiceDueTo.Text)
            objCmd.Parameters.AddWithValue("@TOreturnToService", txtTOreturnToService.Text)
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

            Try
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spInsertMostRecentUpdateByIncidentID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                objCmd.Parameters.AddWithValue("@UpdateDate", NowDate)
                objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Added Weather Reports")

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

                'Enter the email and password to query/command object.
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
            'Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionWeather", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 1)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@WWAdateIssued", "")
            objCmd.Parameters.AddWithValue("@WWAtime", "")
            objCmd.Parameters.AddWithValue("@WWAeffectiveDate ", "")
            objCmd.Parameters.AddWithValue("@WWAeffectiveTime", "")
            objCmd.Parameters.AddWithValue("@WWAexpiresDate", "")
            objCmd.Parameters.AddWithValue("@WWAexpiresTime", "")
            objCmd.Parameters.AddWithValue("@WWAissuingOffice", "")
            objCmd.Parameters.AddWithValue("@WWAadvisoryType", "")
            objCmd.Parameters.AddWithValue("@WWAadvisoryText", "")
            objCmd.Parameters.AddWithValue("@LSRreportType", ddlLSRreportType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@LSRreportReceived", ddlLSRreportReceived.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@LSRInjury", ddlLSRInjury.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@LSRInjuryText", txtLSRInjuryText.Text)
            objCmd.Parameters.AddWithValue("@LSRFatality", ddlLSRFatality.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@LSRFatalityText", txtLSRFatalityText.Text)
            objCmd.Parameters.AddWithValue("@LSRdisplacement", ddlLSRdisplacement.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@LSRdisplacementText", txtLSRdisplacementText.Text)
            objCmd.Parameters.AddWithValue("@LSRdamageStructures", ddlLSRdamageStructures.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@LSRdamageStructuresText", txtLSRdamageStructuresText.Text)
            objCmd.Parameters.AddWithValue("@LSRinfrastructureDamage", ddlLSRinfrastructureDamage.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@LSRinfrastructureDamageText", txtLSRinfrastructureDamageText.Text)
            objCmd.Parameters.AddWithValue("@TOtransmitter", txtTOtransmitter.Text)
            objCmd.Parameters.AddWithValue("@TOmakingNotification", txtTOmakingNotification.Text)
            objCmd.Parameters.AddWithValue("@TOserviceOutTime", CStr(txtTOserviceOutTime.Text.Trim) & CStr(txtTOserviceOutTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@TOserviceOutDate", txtTOserviceOutDate.Text)
            objCmd.Parameters.AddWithValue("@TOtransmitterServiceDueTo", txtTOtransmitterServiceDueTo.Text)
            objCmd.Parameters.AddWithValue("@TOreturnToService", txtTOreturnToService.Text)
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

            Try
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spInsertMostRecentUpdateByIncidentID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                objCmd.Parameters.AddWithValue("@UpdateDate", NowDate)
                objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))
                objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Updated Weather Reports")

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

                'Enter the email and password to query/command object.
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

    Protected Sub ddlLSRInjury_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLSRInjury.SelectedIndexChanged
        If ddlLSRInjury.SelectedValue = "Yes" Then
            pnlShowInjuryText.Visible = True
        Else
            pnlShowInjuryText.Visible = False
        End If
    End Sub

    Protected Sub ddlLSRFatality_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLSRFatality.SelectedIndexChanged
        If ddlLSRFatality.SelectedValue = "Yes" Then
            pnlShowFatalityText.Visible = True
        Else
            pnlShowFatalityText.Visible = False
        End If
    End Sub

    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As EventArgs)
        ErrorChecks()

        If globalHasErrors = False Then
            Save()

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)
    End Sub

    Protected Sub ddlLSRdisplacement_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLSRdisplacement.SelectedIndexChanged
        If ddlLSRdisplacement.SelectedValue = "Yes" Then
            pnlShowLSRdisplacementText.Visible = True
        Else
            pnlShowLSRdisplacementText.Visible = False
        End If
    End Sub

    Protected Sub ddlLSRdamageStructures_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLSRdamageStructures.SelectedIndexChanged
        If ddlLSRdamageStructures.SelectedValue = "Yes" Then
            pnlShowLSRdamageStructuresText.Visible = True
        Else
            pnlShowLSRdamageStructuresText.Visible = False
        End If
    End Sub

    Protected Sub ddlLSRinfrastructureDamage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLSRinfrastructureDamage.SelectedIndexChanged
        If ddlLSRinfrastructureDamage.SelectedValue = "Yes" Then
            pnlShowLSRinfrastructureDamageText.Visible = True
        Else
            pnlShowLSRinfrastructureDamageText.Visible = False
        End If
    End Sub

    Protected Sub ddlSubType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubType.SelectedIndexChanged
        If ddlSubType.SelectedValue = "Local Storm Report" Then
            pnlShowLocalStormReport.Visible = True
        Else
            pnlShowLocalStormReport.Visible = False
        End If

        If ddlSubType.SelectedValue = "NOAA Transnsmitter Outage" Then
            pnlShowTransmitterOutage.Visible = True
        Else
            pnlShowTransmitterOutage.Visible = False
        End If
    End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
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

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label.
        lblMessage.Text = strError.ToString
    End Sub

    Protected Sub btnAddMap_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddMap.Click
        ErrorCheckMap()

        If globalHasErrors = True Then
            pnlMessage.Visible = True

            globalHasErrors = False

            Exit Sub
        Else
            'Variables for creation of random image name.
            Dim localRandomStringForImage As String = ""
            Dim localImageFileName As String = ""

            'Checking for an upload.
            If FileUpload1.HasFile Then
                'Random string append to image name so we do not write over an existing image.
                localRandomStringForImage = HelpFunction.RandomStringGenerator(6)
                localImageFileName = localRandomStringForImage & FileUpload1.FileName
                localImageFileName = Replace(localImageFileName, " ", "")
                localImageFileName = Replace(localImageFileName, "%", "")
                localImageFileName = Replace(localImageFileName, "#", "")
                localImageFileName = Replace(localImageFileName, "!", "")

                'Upload and save the image to the "Uploads" folder.
                FileUpload1.SaveAs(Server.MapPath("Uploads") & "\" & localImageFileName)

                'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
                'Add cookie.
                'Response.Cookies.Add(oCookie)
                ns = Session("Security_Tracker")

                Try
                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Enter the email and password to query/command object.
                    objCmd = New SqlCommand("spInsertWeatherMap", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                    objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
                    objCmd.Parameters.AddWithValue("@Map", localImageFileName)
                    objCmd.Parameters.AddWithValue("@MapName", txtMapName.Text)
                    objCmd.Parameters.AddWithValue("@UploadDate", Now)
                    objCmd.Parameters.AddWithValue("@UserName", ns.FullName)

                    DBConStringHelper.PrepareConnection(objConn)

                    objCmd.ExecuteNonQuery()

                    objCmd.Dispose()
                    objCmd = Nothing

                    DBConStringHelper.FinalizeConnection(objConn)

                    getWeatherMap()
                Catch ex As Exception
                    Response.Write(ex.ToString)
                    Exit Sub
                End Try
            End If
        End If
    End Sub

    Protected Sub btnAddLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLink.Click
        ErrorCheckLink()

        If globalHasErrors = True Then
            pnlMessage.Visible = True

            globalHasErrors = False

            Exit Sub
        Else
            Try
                'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
                'Add cookie.
                'Response.Cookies.Add(oCookie)
                ns = Session("Security_Tracker")

                If Not (txtLink.Text.Contains("http://") Or txtLink.Text.Contains("https://") Or txtLink.Text.Contains("ftp://")) Then
                    txtLink.Text = "http://" & txtLink.Text
                End If

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spInsertWeatherLink", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
                objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
                objCmd.Parameters.AddWithValue("@Link", txtLink.Text)
                objCmd.Parameters.AddWithValue("@LinkName", "") 'Removed link name from UI 20140325 bp
                objCmd.Parameters.AddWithValue("@UploadDate", Now)
                objCmd.Parameters.AddWithValue("@UserName", ns.FullName)

                DBConStringHelper.PrepareConnection(objConn)

                objCmd.ExecuteNonQuery()

                objCmd.Dispose()
                objCmd = Nothing

                DBConStringHelper.FinalizeConnection(objConn)

                getWeatherLink()
            Catch ex As Exception
                Response.Write(ex.ToString)
                Exit Sub
            End Try
        End If
    End Sub

    Protected Sub ErrorCheckMap()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span style='color:#fe5105;'>")

        If FileUpload1.HasFile = False Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a Map.<br />")
            globalHasErrors = True
        End If

        If txtMapName.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Map Name.<br />")
            globalHasErrors = True
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label.
        lblMessage.Text = strError.ToString
    End Sub

    Protected Sub ErrorCheckLink()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span style='color:#fe5105;'>")

        If txtLink.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Link. <br />")
            globalHasErrors = True
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label.
        lblMessage.Text = strError.ToString
    End Sub

    Private Sub DeleteWeatherMap()
        Try
            'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
            'Add cookie.
            'Response.Cookies.Add(oCookie)
            ns = Session("Security_Tracker")

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spDeleteWeatherMapByWeatherMapID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@WeatherMapID", Request("WeatherMapID"))

            'Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            'Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()

            'Clean up our command objects then close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            Response.Redirect("WeatherReports.aspx?IncidentID=" & Request("IncidentID") & "&IncidentIncidentTypeID=" & Request("IncidentIncidentTypeID"))
        Catch ex As Exception
            'DBConStringHelper.FinalizeConnection(objConn)
            'lblMessage.Text = "You may not delete this Weather Map due to the fact it is tied to related imported information. You must first delete all related imported information first, and then you may delete the Weather Map."
            'lblMessage.Visible = True
            'lblMessage.ForeColor = Drawing.Color.Red
        End Try
    End Sub

    Private Sub DeleteWeatherLink()
        Try
            'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
            'Add cookie.
            'Response.Cookies.Add(oCookie)
            ns = Session("Security_Tracker")

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spDeleteWeatherLinkByWeatherLinkID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@WeatherLinkID", Request("WeatherLinkID"))

            'Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            'Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()

            'Clean up our command objects then close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            Response.Redirect("WeatherReports.aspx?IncidentID=" & Request("IncidentID") & "&IncidentIncidentTypeID=" & Request("IncidentIncidentTypeID"))
        Catch ex As Exception
            'DBConStringHelper.FinalizeConnection(objConn)
            'lblMessage.Text = "You may not delete this Weather Link due to the fact it is tied to related imported information. You must first delete all related imported information first, and then you may delete the Weather Link."
            'lblMessage.Visible = True
            'lblMessage.ForeColor = Drawing.Color.Red
        End Try
    End Sub
End Class