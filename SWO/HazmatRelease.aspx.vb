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

Partial Class HazmatRelease
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
    Public objDT As New System.Data.DataTable

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
    Const js As String = "TADDScript.js"
    Public varIncidentNumber As String
    Public varHMNumber As String
    Dim strSQL As String = ""
    Dim fileAction As String = ""
    Dim fileID As Integer = 0
    Dim fileReleaseID As Integer = 0
    Dim fileName As String = ""
    Dim fileContentType As String = ""
    Dim fileBytes As Byte()
    Dim ns As New SecurityValidate

    'Page Load
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ns = Session("Security_Tracker")

        If Not Page.IsPostBack Then
            PopulatePage()
            txtStWarnPointCode.Enabled = False
            txtStWarnPointCodeDate.Enabled = False
            'set release link]
            lnkReport.Target = "_blank"
            lnkReport.NavigateUrl = "HazmatReleaseReport.aspx?IncidentID=" & Request("IncidentID") & "&IncidentIncidentTypeID=" & Request("IncidentIncidentTypeID")
        Else

        End If

    End Sub

    'PagePopulation
    Protected Sub PopulatePage()

        Dim localTime As String = ""
        Dim localTime2 As String = ""
        'IncidentNumber.
        Dim localYear As String = ""
        Dim localNumber As Integer
        Dim address As String = ""
        Dim releaseID As Integer = 0
        Dim sb As New StringBuilder("")
        sb.Append("SELECT *, hr.ChemicalReleased as RelChemicalReleased, hr.CASNumber as RelCASNumber, hm.CASNumber as HMCASNumber, hr.Address as RelAddress FROM [dbo].HazardousMaterials hm")
        sb.Append(" JOIN [dbo].Incident i ON hm.IncidentID = i.IncidentID")
        sb.Append(" JOIN [dbo].[IncidentNumber] inm ON hm.IncidentID = inm.IncidentID")
        sb.Append(" LEFT JOIN [dbo].[HazmatRelease] hr ON hm.IncidentID = hr.IncidentID AND hm.IncidentIncidentTypeID = hr.IncidentIncidentTypeID")
        sb.Append(" WHERE hm.IncidentID = " & Request("IncidentID") & " AND hm.IncidentIncidentTypeID = " & Request("IncidentIncidentTypeID"))
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand(sb.ToString(), objConn)
        objCmd.CommandType = CommandType.Text
        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            If HelpFunction.Convertdbnulls(objDR("HazmatReleaseID")) = "" Then
                localYear = HelpFunction.Convertdbnulls(objDR("Year"))
                localNumber = HelpFunction.ConvertdbnullsInt(objDR("Number"))
                varIncidentNumber = localYear & "-" & CStr(localNumber)
                hidHMID.Value = objDR("HazardousMaterialsID").ToString()
                txtFacilityName.Text = HelpFunction.Convertdbnulls(objDR("FacilityNameSceneDescription"))
                address = HelpFunction.Convertdbnulls(objDR("Address")) & " " & HelpFunction.Convertdbnulls(objDR("City")) & ", " & HelpFunction.Convertdbnulls(objDR("Zip"))
                If Not address = " , " Then
                    txtAddress.Text = address
                Else
                    txtAddress.Text = ""
                End If
                txtStWarnPointCode.Text = varIncidentNumber
                txtStWarnPointCodeDate.Text = HelpFunction.Convertdbnulls(objDR("ReportedToSWODate"))
                txtChemReleased.Text = HelpFunction.Convertdbnulls(objDR("ChemicalName"))
                txtCASNum.Text = HelpFunction.Convertdbnulls(objDR("HMCASNumber"))
                txtAmountReleased.Text = HelpFunction.Convertdbnulls(objDR("ChemicalQuantityReleased"))
                txtReleaseDate.Text = HelpFunction.Convertdbnulls(objDR("IncidentOccurredDate"))
                txtReleaseTime.Text = Left(CStr(HelpFunction.Convertdbnulls(objDR("IncidentOccurredTime"))), 2) & ":" & Right(CStr(HelpFunction.Convertdbnulls(objDR("IncidentOccurredTime"))), 2)
                txtNumberInjured.Text = HelpFunction.Convertdbnulls(objDR("InjuryText"))
                txtNumberFatality.Text = HelpFunction.Convertdbnulls(objDR("FatalityText"))
                If HelpFunction.Convertdbnulls(objDR("Section304ReportableQuantity")).Length > 0 Or HelpFunction.Convertdbnulls(objDR("CERCLAReportableQuantity")).Length > 0 Then
                    cbCercla.Checked = True
                End If
            Else
                hidHReleaseID.Value = HelpFunction.Convertdbnulls(objDR("HazmatReleaseID"))
                hidHMID.Value = HelpFunction.Convertdbnulls(objDR("HazardousMaterialsID"))
                txtFacilityName.Text = HelpFunction.Convertdbnulls(objDR("FacilityName"))
                txtAddress.Text = HelpFunction.Convertdbnulls(objDR("RelAddress"))
                txtBusinessType.Text = HelpFunction.Convertdbnulls(objDR("BusinessType"))
                ddlReleaseSource.SelectedValue = HelpFunction.Convertdbnulls(objDR("ReleaseSource"))
                ddlSector.SelectedValue = HelpFunction.Convertdbnulls(objDR("Sector"))
                txtStWarnPointCode.Text = HelpFunction.Convertdbnulls(objDR("STWarnPointCode"))
                txtStWarnPointCodeDate.Text = HelpFunction.Convertdbnulls(objDR("ReportedToSWODate"))
                txtChemReleased.Text = HelpFunction.Convertdbnulls(objDR("RelChemicalReleased"))
                txtCASNum.Text = HelpFunction.Convertdbnulls(objDR("RelCASNumber"))
                txtAmountReleased.Text = HelpFunction.Convertdbnulls(objDR("AmountReleased"))
                Dim releaseDateTime As String = ""
                releaseDateTime = (HelpFunction.Convertdbnulls(objDR("ReleaseDate")))
                Dim releaseDate As String = ""
                Dim releaseTime As String = ""
                If Not String.IsNullOrEmpty(releaseDateTime) Then
                    releaseDate = CDate(releaseDateTime).ToString("d")
                    releaseTime = CDate(releaseDateTime).ToString("HH:mm")
                End If
                txtReleaseDate.Text = releaseDate
                txtReleaseTime.Text = releaseTime
                txtDescNarrative.Text = HelpFunction.Convertdbnulls(objDR("DescriptionNarrative"))
                txtNumberEvacuated.Text = HelpFunction.Convertdbnulls(objDR("NumberEvacuated"))
                txtNumberInjured.Text = HelpFunction.Convertdbnulls(objDR("NumberInjured"))
                txtNumberFatality.Text = HelpFunction.Convertdbnulls(objDR("FatalitiesVerifiedNum"))
                txtFatalityCause.Text = HelpFunction.Convertdbnulls(objDR("CauseOfDeath"))
                cbCauseDeathVerify.Checked = HelpFunction.Convertdbnulls(objDR("CauseDeathVerified"))
                txtSERCNum.Text = HelpFunction.Convertdbnulls(objDR("SERCNum"))
                txtRMPNum.Text = HelpFunction.Convertdbnulls(objDR("RMPNum"))
                txtTRINum.Text = HelpFunction.Convertdbnulls(objDR("TRINum"))
                txtTierTwoNum.Text = HelpFunction.Convertdbnulls(objDR("TIER2EPlanNum"))
                cbPetroleum.Checked = HelpFunction.Convertdbnulls(objDR("PetroleumRelease"))
                cbLNG.Checked = HelpFunction.Convertdbnulls(objDR("LPLNGPropaneRelease"))
                cbToxic.Checked = HelpFunction.Convertdbnulls(objDR("Toxic"))
                cbRespPartyCall.Checked = HelpFunction.Convertdbnulls(objDR("RespPartyCalled"))
                cbFlammable.Checked = HelpFunction.Convertdbnulls(objDR("Flammable"))
                cbSevenDay.Checked = HelpFunction.Convertdbnulls(objDR("FollowUpReportFiled"))
                cbNRCNotify.Checked = HelpFunction.Convertdbnulls(objDR("NRCNotified"))
                cbCercla.Checked = HelpFunction.Convertdbnulls(objDR("CERCLA304Release"))
                cbOffsite.Checked = HelpFunction.Convertdbnulls(objDR("OffsiteRelease"))
                cbSEP.Checked = HelpFunction.Convertdbnulls(objDR("SEP"))
                txtSevenDayDue.Text = HelpFunction.Convertdbnulls(objDR("FollowUpReportDueDate"))
                objDR.Close()

                Dim sb2 As New StringBuilder("")
                sb2.Append("SELECT [FileID],[ReleaseID],[FileName],[ContentType],len([FileData]) as FileData FROM [dbo].HazmatReleaseFiles")
                sb2.Append(" WHERE ReleaseID = " & CInt(hidHReleaseID.Value))
                objCmd = New SqlCommand(sb2.ToString(), objConn)
                objDT.Clear()
                'bind our data
                objDA = New SqlDataAdapter(objCmd)
                objDA.Fill(objDT)

                Dim row As DataRow
                For Each row In objDT.Rows
                    If row.Item("FileName") = "7DayFacilityFollowupReport" Then
                        lnkFile1.Text = "View File"
                        lnkFile1.NavigateUrl = "HazmatReleaseFile.aspx?FileID=" & row.Item("FileID")
                        lnkFile1.Target = "_blank"
                        hiddFile1.Value = Convert.ToString(row.Item("FileData")) 'Convert.ToBase64String(row.Item("FileData"))
                        hiddFile1CT.Value = row.Item("ContentType")
                        hiddFileID1.Value = row.Item("FileID")
                        lbDeleteFile1.CommandArgument = row.Item("FileID")
                        divDeleteFile1.Visible = True
                    ElseIf row.Item("FileName") = "SuppDocumentationFile" Then
                        lnkFile2.Text = "View File"
                        lnkFile2.NavigateUrl = "HazmatReleaseFile.aspx?FileID=" & row.Item("FileID")
                        lnkFile2.Target = "_blank"
                        hiddFile2.Value = Convert.ToString(row.Item("FileData")) 'Convert.ToBase64String(row.Item("FileData"))
                        hiddFile2CT.Value = row.Item("ContentType")
                        hiddFileID2.Value = row.Item("FileID")
                        lbDeleteFile2.CommandArgument = row.Item("FileID")
                        divDeleteFile2.Visible = True
                    ElseIf row.Item("FileName") = "AdditionalInfoFile" Then
                        lnkFile3.Text = "View File"
                        lnkFile3.NavigateUrl = "HazmatReleaseFile.aspx?FileID=" & row.Item("FileID")
                        lnkFile3.Target = "_blank"
                        hiddFile3.Value = Convert.ToString(row.Item("FileData")) 'Convert.ToBase64String(row.Item("FileData"))
                        hiddFile3CT.Value = row.Item("ContentType")
                        hiddFileID3.Value = row.Item("FileID")
                        lbDeleteFile3.CommandArgument = row.Item("FileID")
                        divDeleteFile3.Visible = True
                    ElseIf row.Item("FileName") = "SEPInfoFile" Then
                        lnkFile4.Text = "View File"
                        lnkFile4.NavigateUrl = "HazmatReleaseFile.aspx?FileID=" & row.Item("FileID")
                        lnkFile4.Target = "_blank"
                        hiddFile4.Value = Convert.ToString(row.Item("FileData")) 'Convert.ToBase64String(row.Item("FileData"))
                        hiddFile4CT.Value = row.Item("ContentType")
                        hiddFileID4.Value = row.Item("FileID")
                        lbDeleteFile4.CommandArgument = row.Item("FileID")
                        divDeleteFile4.Visible = True
                    End If
                Next
                objCmd.Dispose()
                objCmd = Nothing
            End If
        End If

        objConn.Close()
    End Sub

    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As EventArgs)

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        '// Enter the email and password to query/command object.
        objCmd = New SqlCommand("HazmatReleaseAction", objConn)
        objCmd.CommandType = CommandType.StoredProcedure

        If hidHReleaseID.Value = "" Then
            objCmd.Parameters.AddWithValue("@StatementType", "Insert")
        Else
            objCmd.Parameters.AddWithValue("@StatementType", "Update")
        End If
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
        objCmd.Parameters.AddWithValue("@HazardousMaterialsID", CInt(hidHMID.Value))
        objCmd.Parameters.AddWithValue("@FacilityName", txtFacilityName.Text.Trim)
        objCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim)
        objCmd.Parameters.AddWithValue("@BusinessType", txtBusinessType.Text.Trim)
        objCmd.Parameters.AddWithValue("@ReleaseSource", ddlReleaseSource.SelectedValue)
        objCmd.Parameters.AddWithValue("@Sector", ddlSector.SelectedValue)
        objCmd.Parameters.AddWithValue("@STWarnPointCode", txtStWarnPointCode.Text.Trim)
        objCmd.Parameters.AddWithValue("@ChemicalReleased", txtChemReleased.Text.Trim)
        objCmd.Parameters.AddWithValue("@CASNumber", txtCASNum.Text.Trim)
        objCmd.Parameters.AddWithValue("@AmountReleased", txtAmountReleased.Text.Trim)
        objCmd.Parameters.AddWithValue("@ReleaseDate", txtReleaseDate.Text.Trim & " " & txtReleaseTime.Text.Trim)
        objCmd.Parameters.AddWithValue("@DescriptionNarrative", txtDescNarrative.Text.Trim)
        If txtNumberEvacuated.Text.Trim.Length > 0 Then
            objCmd.Parameters.AddWithValue("@NumberEvacuated", CInt(txtNumberEvacuated.Text.Trim))
        End If
        If txtNumberInjured.Text.Trim.Length > 0 Then
            objCmd.Parameters.AddWithValue("@NumberInjured", CInt(txtNumberInjured.Text.Trim))
        End If
        If txtNumberFatality.Text.Trim.Length > 0 Then
            objCmd.Parameters.AddWithValue("@FatalitiesVerifiedNum", CInt(txtNumberFatality.Text.Trim))
        End If
        objCmd.Parameters.AddWithValue("@CauseOfDeath", txtFatalityCause.Text.Trim)
        objCmd.Parameters.AddWithValue("@CauseDeathVerified", cbCauseDeathVerify.Checked)
        objCmd.Parameters.AddWithValue("@SERCNum", txtSERCNum.Text.Trim)
        objCmd.Parameters.AddWithValue("@RMPNum", txtRMPNum.Text.Trim)
        objCmd.Parameters.AddWithValue("@TRINum", txtTRINum.Text.Trim)
        objCmd.Parameters.AddWithValue("@TIER2EPlanNum", txtTierTwoNum.Text.Trim)
        objCmd.Parameters.AddWithValue("@PetroleumRelease", cbPetroleum.Checked)
        objCmd.Parameters.AddWithValue("@LPLNGPropaneRelease", cbLNG.Checked)
        objCmd.Parameters.AddWithValue("@Toxic", cbToxic.Checked)
        objCmd.Parameters.AddWithValue("@RespPartyCalled", cbRespPartyCall.Checked)
        objCmd.Parameters.AddWithValue("@Flammable", cbFlammable.Checked)
        objCmd.Parameters.AddWithValue("@FollowUpReportFiled", cbSevenDay.Checked)
        objCmd.Parameters.AddWithValue("@NRCNotified", cbNRCNotify.Checked)
        objCmd.Parameters.AddWithValue("@CERCLA304Release", cbCercla.Checked)
        objCmd.Parameters.AddWithValue("@OffsiteRelease", cbOffsite.Checked)
        objCmd.Parameters.AddWithValue("@SEP", cbSEP.Checked)
        objCmd.Parameters.AddWithValue("@FollowUpReportDueDate", IIf(String.IsNullOrEmpty(txtSevenDayDue.Text.Trim), DBNull.Value, txtSevenDayDue.Text.Trim))
        objCmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output
        DBConStringHelper.PrepareConnection(objConn)

        objCmd.ExecuteNonQuery()
        Dim releaseID As String = ""
        If hidHReleaseID.Value.Length = 0 Then
            releaseID = objCmd.Parameters("@id").Value.ToString()
        End If
        objCmd.Dispose()
        objCmd = Nothing

        Try
            'objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objCmd = New SqlCommand("spUpdateIncidentReportUpdate", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@LastUpdatedByID", ns.UserID)
            objCmd.Parameters.AddWithValue("@LastUpdated", Now)
            DBConStringHelper.PrepareConnection(objConn)
            objCmd.ExecuteNonQuery()
            objCmd.Dispose()
            objCmd = Nothing
        Catch ex As Exception
            Response.Write(ex.ToString)
            Exit Sub
        End Try

        If FileUpload1.HasFile Or hiddFile1.Value.Length > 0 Then
            fileName = "7DayFacilityFollowupReport"
            If FileUpload1.HasFile Then
                If Not hiddFile1.Value.Length > 0 Then
                    fileAction = "Insert"
                Else
                    fileAction = "Update"
                    fileID = CInt(hiddFileID1.Value)
                End If
                If hidHReleaseID.Value.Length = 0 Then
                    fileReleaseID = CInt(releaseID)
                Else
                    fileReleaseID = CInt(hidHReleaseID.Value)
                End If
                fileContentType = FileUpload1.PostedFile.ContentType.ToString()
                fileBytes = FileUpload1.FileBytes
                'ElseIf hiddFile1.Value.Length > 0 Then
                '    fileAction = "Update"
                '    fileID = CInt(hiddFileID1.Value)
                '    fileContentType = hiddFile1CT.Value
                '    Dim data As Byte() = Convert.FromBase64String(hiddFile1.Value)
                '    fileBytes = data
                ProcessFile(fileAction, fileID, fileReleaseID, fileName, fileContentType, fileBytes)
            End If
        End If

        If FileUpload2.HasFile Or hiddFile2.Value.Length > 0 Then
            fileName = "SuppDocumentationFile"
            If FileUpload2.HasFile Then
                If Not hiddFile2.Value.Length > 0 Then
                    fileAction = "Insert"
                Else
                    fileAction = "Update"
                    fileID = CInt(hiddFileID2.Value)
                End If
                If hidHReleaseID.Value.Length = 0 Then
                    fileReleaseID = CInt(releaseID)
                Else
                    fileReleaseID = CInt(hidHReleaseID.Value)
                End If
                fileContentType = FileUpload2.PostedFile.ContentType.ToString()
                fileBytes = FileUpload2.FileBytes
                'ElseIf hiddFile2.Value.Length > 0 Then
                '    fileAction = "Update"
                '    fileID = CInt(hiddFileID2.Value)
                '    fileContentType = hiddFile2CT.Value
                '    Dim data As Byte() = Convert.FromBase64String(hiddFile2.Value)
                '    fileBytes = data
                ProcessFile(fileAction, fileID, fileReleaseID, fileName, fileContentType, fileBytes)
            End If
        End If

        If FileUpload3.HasFile Or hiddFile3.Value.Length > 0 Then
            fileName = "AdditionalInfoFile"
            If FileUpload3.HasFile Then
                If Not hiddFile3.Value.Length > 0 Then
                    fileAction = "Insert"
                Else
                    fileAction = "Update"
                    fileID = CInt(hiddFileID3.Value)
                End If
                If hidHReleaseID.Value.Length = 0 Then
                    fileReleaseID = CInt(releaseID)
                Else
                    fileReleaseID = CInt(hidHReleaseID.Value)
                End If
                fileContentType = FileUpload3.PostedFile.ContentType.ToString()
                fileBytes = FileUpload3.FileBytes
                'ElseIf hiddFile3.Value.Length > 0 Then
                '    fileAction = "Update"
                '    fileID = CInt(hiddFileID3.Value)
                '    fileContentType = hiddFile3CT.Value
                '    Dim data As Byte() = Convert.FromBase64String(hiddFile3.Value)
                '    fileBytes = data
                ProcessFile(fileAction, fileID, fileReleaseID, fileName, fileContentType, fileBytes)
            End If
        End If

        If FileUpload4.HasFile Or hiddFile4.Value.Length > 0 Then
            fileName = "SEPInfoFile"
            If FileUpload4.HasFile Then
                If Not hiddFile4.Value.Length > 0 Then
                    fileAction = "Insert"
                Else
                    fileAction = "Update"
                    fileID = CInt(hiddFileID4.Value)
                End If
                If hidHReleaseID.Value.Length = 0 Then
                    fileReleaseID = CInt(releaseID)
                Else
                    fileReleaseID = CInt(hidHReleaseID.Value)
                End If
                fileContentType = FileUpload4.PostedFile.ContentType.ToString()
                fileBytes = FileUpload4.FileBytes
                'ElseIf hiddFile4.Value.Length > 0 Then
                '    fileAction = "Update"
                '    fileID = CInt(hiddFileID4.Value)
                '    fileContentType = hiddFile4CT.Value
                '    Dim data As Byte() = Convert.FromBase64String(hiddFile4.Value)
                '    fileBytes = data
                ProcessFile(fileAction, fileID, fileReleaseID, fileName, fileContentType, fileBytes)
            End If
        End If

        DBConStringHelper.FinalizeConnection(objConn)
        PopulatePage()
    End Sub

    Sub ProcessFile(ByVal action As String, ByVal fileID As Integer, ByVal releaseID As Integer, ByVal fileName As String, ByVal contentType As String, ByVal fileData As Byte())
        objCmd = New SqlCommand("HazmatReleaseFileInsertUpdate", objConn)
        objCmd.CommandType = CommandType.StoredProcedure

        objCmd.Parameters.AddWithValue("@StatementType", action)
        objCmd.Parameters.AddWithValue("@FileID", fileID)
        objCmd.Parameters.AddWithValue("@ReleaseID", releaseID)
        objCmd.Parameters.AddWithValue("@FileName", fileName)
        objCmd.Parameters.AddWithValue("@ContentType", contentType)
        objCmd.Parameters.AddWithValue("@FileData", fileData)
        DBConStringHelper.PrepareConnection(objConn)

        objCmd.ExecuteNonQuery()

        objCmd.Dispose()
        objCmd = Nothing
    End Sub

    Protected Sub btnReport_Command(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("HazmatReleaseReport.aspx?IncidentID=" & Request("IncidentID") & "&IncidentIncidentTypeID=" & Request("IncidentIncidentTypeID"))
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("HazardousMaterials.aspx?IncidentID=" & Request("IncidentID") & "&IncidentIncidentTypeID=" & Request("IncidentIncidentTypeID"))
    End Sub

    Protected Sub LinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim strFileID As String = DirectCast(sender, LinkButton).CommandArgument

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objCmd = New SqlCommand("spDeleteHazmatFileByFileID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@FileID", strFileID)
            DBConStringHelper.PrepareConnection(objConn)
            objCmd.ExecuteNonQuery()
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)
            Response.Redirect("HazmatRelease.aspx?IncidentID=" & Request("IncidentID") & "&IncidentIncidentTypeID=" & Request("IncidentIncidentTypeID"))
        Catch ex As Exception
            'Page had no error handling but this when I got here! bp
            Response.Write(ex.ToString)
            Exit Sub
        End Try

    End Sub

End Class
