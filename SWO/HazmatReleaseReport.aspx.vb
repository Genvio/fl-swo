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

Partial Class HazmatReleaseReport
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

    Dim strSQL As String = ""
    Dim fileAction As String = ""
    Dim fileID As Integer = 0
    Dim fileReleaseID As Integer = 0
    Dim fileName As String = ""
    Dim fileContentType As String = ""
    Dim fileBytes As Byte()
    Dim varURL As String = ""
    Public ObjCookie As System.Web.HttpCookie

    'Page Load
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'set the properties for reference
        ObjCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '---------------------------------------------------------------------------------------------------------------------
        Try

            '-  Extract UserGroup
            Dim strApplicationName As String

            Select Case Application("ApplicationEnvironment").ToString
                Case "SWODEV"
                    strApplicationName = " (development)"
                Case "SWOX"
                    strApplicationName = " (exercise)"
                Case Else
                    strApplicationName = String.Empty
            End Select
        Catch ex As Exception
            'they are not logged in
            Response.Redirect("Default.aspx")
        End Try

        If Not Page.IsPostBack Then
            varURL = "HazmatReleaseReport.aspx?IncidentID=" & Request("IncidentID") & "&IncidentIncidentTypeID=" & Request("IncidentIncidentTypeID")
            PopulatePage()
        Else

        End If

    End Sub

    'PagePopulation
    Protected Sub PopulatePage()

        Dim localTime As String = ""
        Dim localTime2 As String = ""
        Dim address As String = ""
        Dim releaseID As Integer = 0
        Dim sb As New StringBuilder("")
        sb.Append("SELECT *, hr.ChemicalReleased as RelChemicalReleased, hr.Address as RelAddress, hr.CASNumber as RelCAS")
        sb.Append(", rp.FirstName as RepFName,rp.LastName as RepLName,rp.CallBackNumber1 as RepCallBackNumber1,rp.CallBackNumber2 as RepCallBackNumber2,rp.Email as RepEmail,rp.Address as RepAddress, rp.City as RepCity, rp.State as RepState, rp.Zipcode as RepZip, rp.Represents as RepRep,rpt.ReportingPartyType")
        sb.Append(", resp.FirstName as RespFName,resp.LastName as RespLName,resp.CallBackNumber1 as RespCallBackNumber1,resp.CallBackNumber2 as RespCallBackNumber2,resp.Email as RespEmail,resp.Address as RespAddress, resp.City as RespCity, resp.State as RespState, resp.Zipcode as RespZip, resp.Represents as RespRep, respt.ResponsiblePartyType")
        sb.Append(", osc.FirstName as oscFName,osc.LastName as oscLName,osc.CallBackNumber1 as oscCallBackNumber1,osc.CallBackNumber2 as oscCallBackNumber2,osc.Email as oscEmail,osc.Address as oscAddress, osc.City as oscCity, osc.State as oscState, osc.Zipcode as oscZip, osc.Represents as oscRep, osct.OnSceneContactType")
        sb.Append(" FROM [dbo].HazardousMaterials hm")
        sb.Append(" JOIN [dbo].Incident i ON hm.IncidentID = i.IncidentID")
        sb.Append(" JOIN [dbo].[IncidentNumber] inm ON hm.IncidentID = inm.IncidentID")
        sb.Append(" JOIN [dbo].[Severity] s ON i.SeverityID = s.SeverityID")
        sb.Append(" JOIN [dbo].IncidentStatus ins ON i.IncidentStatusID = ins.IncidentStatusID")
        sb.Append(" LEFT JOIN [dbo].[ReportingParty] rp ON hm.IncidentID = rp.IncidentID")
        sb.Append(" LEFT JOIN [dbo].[ReportingPartyType] rpt ON i.ReportingPartyTypeID = rpt.ReportingPartyTypeID")
        sb.Append(" LEFT JOIN [dbo].[ResponsibleParty] resp ON hm.IncidentID = resp.IncidentID")
        sb.Append(" LEFT JOIN [dbo].[ResponsiblePartyType] respt ON i.ResponsiblePartyTypeID = respt.ResponsiblePartyTypeID")
        sb.Append(" LEFT JOIN [dbo].[OnSceneContact] osc ON hm.IncidentID = osc.IncidentID")
        sb.Append(" LEFT JOIN [dbo].[OnSceneContactType] osct ON i.OnSceneContactTypeID = osct.OnSceneContactTypeID")
        sb.Append(" LEFT JOIN (SELECT * FROM [dbo].[UpdateReport]")
        sb.Append(" WHERE UpdateReportID IN (SELECT MAX(UpdateReportID) FROM [dbo].[UpdateReport] WHERE IsDeleted = 0)) upd")
        sb.Append(" ON hm.IncidentID = upd.IncidentID")
        sb.Append(" LEFT JOIN (SELECT * FROM [dbo].[InitialReport]")
        sb.Append(" WHERE InitialReportID IN (SELECT MIN(InitialReportID) FROM [dbo].[InitialReport] WHERE InitialReport <> '')) ir")
        sb.Append(" ON hm.IncidentID = ir.IncidentID")
        sb.Append(" LEFT JOIN [dbo].[HazmatRelease] hr ON hm.IncidentID = hr.IncidentID AND hm.IncidentIncidentTypeID = hr.IncidentIncidentTypeID")
        sb.Append(" WHERE hm.IncidentID = " & Request("IncidentID") & " AND hm.IncidentIncidentTypeID = " & Request("IncidentIncidentTypeID"))
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand(sb.ToString(), objConn)
        objCmd.CommandType = CommandType.Text
        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            ' REPORTING PARTY
            If Not HelpFunction.Convertdbnulls(objDR("ReportingPartyType")) = "As Below" Then
                lblReportParty.Text = HelpFunction.Convertdbnulls(objDR("ReportingPartyType"))
            Else
                sb.Length = 0
                If Not HelpFunction.Convertdbnulls(objDR("RepFName")) = "" Then
                    sb.Append("<br/><div class='indentinfo'>" & HelpFunction.Convertdbnulls(objDR("RepFName")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RepLName")) = "" Then
                    sb.Append(" " & HelpFunction.Convertdbnulls(objDR("RepLName")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RepRep")) = "" Then
                    sb.Append(", " & HelpFunction.Convertdbnulls(objDR("RepRep")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RepCallBackNumber1")) = "" Then
                    sb.Append("<br/>" & HelpFunction.Convertdbnulls(objDR("RepCallBackNumber1")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RepCallBackNumber2")) = "" Then
                    sb.Append(" or " & HelpFunction.Convertdbnulls(objDR("RepCallBackNumber2")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RepEmail")) = "" Then
                    sb.Append("<br/>" & HelpFunction.Convertdbnulls(objDR("RepEmail")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RepAddress")) = "" Then
                    sb.Append("<br/>" & HelpFunction.Convertdbnulls(objDR("RepAddress")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RepCity")) = "" Then
                    sb.Append(", " & HelpFunction.Convertdbnulls(objDR("RepCity")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RepState")) = "" Then
                    sb.Append(", " & HelpFunction.Convertdbnulls(objDR("RepState")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RepZip")) = "" Then
                    sb.Append(" " & HelpFunction.Convertdbnulls(objDR("RepZip")))
                End If
                sb.Append("</div>")
                lblReportParty.Text = sb.ToString()
            End If
            ' RESPONSIBLE PARTY
            If Not HelpFunction.Convertdbnulls(objDR("ResponsiblePartyType")) = "As Below" Then
                lblRespParty.Text = HelpFunction.Convertdbnulls(objDR("ResponsiblePartyType"))
            Else
                sb.Length = 0

                sb.Append("<br/><div class='indentinfo'>")
                If Not HelpFunction.Convertdbnulls(objDR("RespFName")) = "" Then
                    sb.Append(HelpFunction.Convertdbnulls(objDR("RespFName")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RespLName")) = "" Then
                    sb.Append(" " & HelpFunction.Convertdbnulls(objDR("RespLName")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RespRep")) = "" Then
                    sb.Append(", " & HelpFunction.Convertdbnulls(objDR("RespRep")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RespCallBackNumber1")) = "" Then
                    sb.Append("<br/>" & HelpFunction.Convertdbnulls(objDR("RespCallBackNumber1")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RespCallBackNumber2")) = "" Then
                    sb.Append(" or " & HelpFunction.Convertdbnulls(objDR("RespCallBackNumber2")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RespEmail")) = "" Then
                    sb.Append("<br/>" & HelpFunction.Convertdbnulls(objDR("RespEmail")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RespAddress")) = "" Then
                    sb.Append("<br/>" & HelpFunction.Convertdbnulls(objDR("RespAddress")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RespCity")) = "" Then
                    sb.Append(", " & HelpFunction.Convertdbnulls(objDR("RespCity")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RespState")) = "" Then
                    sb.Append(", " & HelpFunction.Convertdbnulls(objDR("RespState")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("RespZip")) = "" Then
                    sb.Append(" " & HelpFunction.Convertdbnulls(objDR("RespZip")))
                End If
                sb.Append("</div>")
                lblRespParty.Text = sb.ToString()
            End If
            ' ON SCENE CONTACT
            If Not HelpFunction.Convertdbnulls(objDR("OnSceneContactType")) = "As Below" Then
                lblOnsceneContact.Text = HelpFunction.Convertdbnulls(objDR("OnSceneContactType"))
            Else
                sb.Length = 0
                If Not HelpFunction.Convertdbnulls(objDR("oscFName")) = "" Then
                    sb.Append("<br/><div class='indentinfo'>" & HelpFunction.Convertdbnulls(objDR("oscFName")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("oscLName")) = "" Then
                    sb.Append(" " & HelpFunction.Convertdbnulls(objDR("oscLName")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("oscRep")) = "" Then
                    sb.Append(", " & HelpFunction.Convertdbnulls(objDR("oscRep")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("oscCallBackNumber1")) = "" Then
                    sb.Append("<br/>" & HelpFunction.Convertdbnulls(objDR("oscCallBackNumber1")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("oscCallBackNumber2")) = "" Then
                    sb.Append(" or " & HelpFunction.Convertdbnulls(objDR("oscCallBackNumber2")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("oscEmail")) = "" Then
                    sb.Append("<br/>" & HelpFunction.Convertdbnulls(objDR("oscEmail")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("oscAddress")) = "" Then
                    sb.Append("<br/>" & HelpFunction.Convertdbnulls(objDR("oscAddress")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("oscCity")) = "" Then
                    sb.Append(", " & HelpFunction.Convertdbnulls(objDR("oscCity")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("oscState")) = "" Then
                    sb.Append(", " & HelpFunction.Convertdbnulls(objDR("oscState")))
                End If
                If Not HelpFunction.Convertdbnulls(objDR("oscZip")) = "" Then
                    sb.Append(" " & HelpFunction.Convertdbnulls(objDR("oscZip")))
                End If
                sb.Append("</div>")
                lblOnsceneContact.Text = sb.ToString()
            End If
            lblReportNum.Text = HelpFunction.Convertdbnulls(objDR("STWarnPointCode"))
            Session("stWarnCode") = HelpFunction.Convertdbnulls(objDR("STWarnPointCode"))
            lblStatus.Text = HelpFunction.Convertdbnulls(objDR("IncidentStatus"))
            lblReportDateTime.Text = HelpFunction.Convertdbnulls(objDR("ReportedToSWODate")) & " " & Left(CStr(HelpFunction.Convertdbnulls(objDR("ReportedToSWOTime"))), 2) & ":" & Right(CStr(HelpFunction.Convertdbnulls(objDR("ReportedToSWOTime"))), 2)
            lblSeverity.Text = HelpFunction.Convertdbnulls(objDR("Severity"))
            If Not HelpFunction.Convertdbnulls(objDR("InitialReport")) = "" Then
                lblInitialReport.Text = HelpFunction.Convertdbnulls(objDR("InitialReport"))
            End If
            If Not HelpFunction.Convertdbnulls(objDR("Evacuations")) = "" Then
                lblEvac.Text = Replace(HelpFunction.Convertdbnulls(objDR("Evacuations")), "Select an Option", "")
            End If
            If Not HelpFunction.Convertdbnulls(objDR("Injury")) = "" Then
                sb.Length = 0
                sb.Append(Replace(HelpFunction.Convertdbnulls(objDR("Injury")), "Select an Option", ""))
                If Not HelpFunction.Convertdbnulls(objDR("InjuryText")) = "" Then
                    sb.Append(", " & HelpFunction.Convertdbnulls(objDR("InjuryText")))
                End If
                lblInjury.Text = sb.ToString()
            End If
            If Not HelpFunction.Convertdbnulls(objDR("Fatality")) = "" Then
                sb.Length = 0
                sb.Append(Replace(HelpFunction.Convertdbnulls(objDR("Fatality")), "Select an Option", ""))
                If Not HelpFunction.Convertdbnulls(objDR("FatalityText")) = "" Then
                    sb.Append(", " & HelpFunction.Convertdbnulls(objDR("FatalityText")))
                End If
                lblInjury.Text = sb.ToString()
            End If
            If HelpFunction.Convertdbnulls(objDR("EnvironmentalImpact")) = "Yes" Then
                sb.Length = 0
                sb.Append(HelpFunction.Convertdbnulls(objDR("EnvironmentalImpact")))
                If Not HelpFunction.Convertdbnulls(objDR("DEPCallbackRequested")) = "Select an Option" Then
                    If HelpFunction.Convertdbnulls(objDR("DEPCallbackRequested")) = "Yes" Then
                        sb.Append("<br/><strong>DEP callback requested: </strong>Yes")
                        If Not HelpFunction.Convertdbnulls(objDR("EnvironmentalImpactContact")) = "Select an Option" Then
                            sb.Append(", ")
                            sb.Append(HelpFunction.Convertdbnulls(objDR("EnvironmentalImpactContact")))
                        End If
                    Else
                        sb.Append("<br/><strong>DEP callback requested: </strong>No ")
                    End If
                End If
                lblEnviroImpact.Text = sb.ToString()
            Else
                lblEnviroImpact.Text = HelpFunction.Convertdbnulls(objDR("EnvironmentalImpact"))
            End If
            If Not HelpFunction.Convertdbnulls(objDR("UpdateReport")) = "" Then
                lblUpdate.Text = "<div class='indentinfo'><em>" & HelpFunction.Convertdbnulls(objDR("UpdateDate")) & "</em> - " & HelpFunction.Convertdbnulls(objDR("UpdateReport") & "</div>")
            End If
            lblCounty.Text = HelpFunction.Convertdbnulls(objDR("AddedCounty"))
            lblFacilityName.Text = HelpFunction.Convertdbnulls(objDR("FacilityNameSceneDescription"))
            lblIncidentLocation.Text = HelpFunction.Convertdbnulls(objDR("Address"))
            If Not HelpFunction.Convertdbnulls(objDR("Lat")) = "" And Not HelpFunction.Convertdbnulls(objDR("Long")) = "" Then
                lblCoords.Text = "Lat: " & HelpFunction.Convertdbnulls(objDR("Lat")) & ", Long: " & HelpFunction.Convertdbnulls(objDR("Long"))
            End If
            lblSubType.Text = HelpFunction.Convertdbnulls(objDR("SubType"))
            lblSituation.Text = HelpFunction.Convertdbnulls(objDR("Situation"))
            lblDescription.Text = HelpFunction.Convertdbnulls(objDR("IncidentName"))
            lblChemicalName.Text = HelpFunction.Convertdbnulls(objDR("ChemicalName"))
            lblIndexName.Text = HelpFunction.Convertdbnulls(objDR("IndexName"))
            lblCASNum.Text = HelpFunction.Convertdbnulls(objDR("CASNumber"))
            If Not HelpFunction.Convertdbnulls(objDR("Section304ReportableQuantity")) = "" Then
                lblSection304Qty.Text = HelpFunction.Convertdbnulls(objDR("Section304ReportableQuantity"))
            Else
                lblSection304Qty.Text = "None"
            End If
            If Not HelpFunction.Convertdbnulls(objDR("Section304ReportableQuantity")) = "" Then
                lblCerclaQty.Text = HelpFunction.Convertdbnulls(objDR("CERCLAReportableQuantity"))
            Else
                lblCerclaQty.Text = "None"
            End If
            lblChemState.Text = HelpFunction.Convertdbnulls(objDR("ChemicalState"))
            lblSourceContainer.Text = HelpFunction.Convertdbnulls(objDR("SourceContainer"))
            lblRateRelease.Text = HelpFunction.Convertdbnulls(objDR("ChemicalRateOfRelease"))
            lblReleased.Text = HelpFunction.Convertdbnulls(objDR("ChemicalReleased"))
            lblQtyReleased.Text = HelpFunction.Convertdbnulls(objDR("ChemicalQuantityReleased"))
            lblCauseRelease.Text = HelpFunction.Convertdbnulls(objDR("CauseOfRelease"))
            lblStormDrains.Text = HelpFunction.Convertdbnulls(objDR("StormDrainsAffected"))
            If Not HelpFunction.Convertdbnulls(objDR("WaterwaysAffected")) = "No" Then
                lblWaterways.Text = HelpFunction.Convertdbnulls(objDR("WaterwaysAffectedText"))
            Else
                lblWaterways.Text = HelpFunction.Convertdbnulls(objDR("WaterwaysAffected"))
            End If
            lblRoadways.Text = HelpFunction.Convertdbnulls(objDR("MajorRoadwaysClosed"))

            'UPDATED HAZMAT INVESTIGATION
            lblHMNarrative.Text = HelpFunction.Convertdbnulls(objDR("DescriptionNarrative"))
            lblHMFacName.Text = HelpFunction.Convertdbnulls(objDR("FacilityName"))
            lblBusinessType.Text = HelpFunction.Convertdbnulls(objDR("BusinessType"))
            lblHMIncidentLocation.Text = HelpFunction.Convertdbnulls(objDR("RelAddress"))
            lblHMReleaseSource.Text = HelpFunction.Convertdbnulls(objDR("ReleaseSource"))
            lblHMSector.Text = Replace(HelpFunction.Convertdbnulls(objDR("Sector")), "Select an Option", "")
            lblHMChemicalName.Text = HelpFunction.Convertdbnulls(objDR("RelChemicalReleased"))
            lblHMCAS.Text = HelpFunction.Convertdbnulls(objDR("RelCAS"))
            lblHMAmtReleased.Text = HelpFunction.Convertdbnulls(objDR("AmountReleased"))
            lblHMReleaseDate.Text = HelpFunction.Convertdbnulls(objDR("ReleaseDate")).ToString()
            lblHMEvac.Text = HelpFunction.Convertdbnulls(objDR("NumberEvacuated")).ToString()
            If Not HelpFunction.Convertdbnulls(objDR("NumberInjured")) = "" Then
                lblHMInjury.Text = HelpFunction.Convertdbnulls(objDR("NumberInjured"))
            End If
            If Not HelpFunction.Convertdbnulls(objDR("FatalitiesVerifiedNum")) = "" Then
                sb.Length = 0
                sb.Append(HelpFunction.Convertdbnulls(objDR("FatalitiesVerifiedNum")))
                If Not HelpFunction.Convertdbnulls(objDR("CauseOfDeath")) = "" Then
                    sb.Append(" - Cause of Death: ")
                    sb.Append(HelpFunction.Convertdbnulls(objDR("CauseOfDeath")))
                    If objDR("CauseDeathVerified") Then
                        sb.Append(" (Verified)")
                    Else
                        sb.Append(" (Not Verified)")
                    End If
                End If
                lblHMFatality.Text = sb.ToString()
            End If
            lblSERC.Text = HelpFunction.Convertdbnulls(objDR("SERCNum"))
            lblRMP.Text = HelpFunction.Convertdbnulls(objDR("RMPNum"))
            lblTRI.Text = HelpFunction.Convertdbnulls(objDR("TRINum"))
            lblTIER2EPLAN.Text = HelpFunction.Convertdbnulls(objDR("TIER2EPLANNum"))
            cbPetroleum.Checked = HelpFunction.ConvertdbnullsBool(objDR("PetroleumRelease"))
            cbLNG.Checked = HelpFunction.ConvertdbnullsBool(objDR("LPLNGPropaneRelease"))
            cbToxic.Checked = HelpFunction.ConvertdbnullsBool(objDR("Toxic"))
            cbRespPartyCall.Checked = HelpFunction.ConvertdbnullsBool(objDR("RespPartyCalled"))
            cbFlammable.Checked = HelpFunction.ConvertdbnullsBool(objDR("Flammable"))
            cbSevenDay.Checked = HelpFunction.ConvertdbnullsBool(objDR("FollowUpReportFiled"))
            cbNRCNotify.Checked = HelpFunction.ConvertdbnullsBool(objDR("NRCNotified"))
            cbCercla.Checked = HelpFunction.ConvertdbnullsBool(objDR("CERCLA304Release"))
            cbOffsite.Checked = HelpFunction.ConvertdbnullsBool(objDR("OffsiteRelease"))
            cbSEP.Checked = HelpFunction.ConvertdbnullsBool(objDR("SEP"))
            lblFollowUpReportDueDate.Text = HelpFunction.Convertdbnulls(objDR("FollowUpReportDueDate"))
        End If
        objDR.Close()
        objCmd.Dispose()
        objCmd = Nothing

        ' GET INITIAL REPORT
        sb.Length = 0
        sb.Append("SELECT * FROM [dbo].[InitialReport] WHERE InitialReportID IN")
        sb.Append(" (SELECT MIN(InitialReportID) FROM [dbo].[InitialReport] WHERE IncidentID = " & Request("IncidentID") & " AND InitialReport <> '')")
        objCmd = New SqlCommand(sb.ToString(), objConn)
        objCmd.CommandType = CommandType.Text
        objDR = objCmd.ExecuteReader
        If objDR.Read Then
            lblInitialReport.Text = HelpFunction.Convertdbnulls(objDR("InitialReport"))
        End If
        objDR.Close()
        objCmd.Dispose()
        objCmd = Nothing

        ' GET SECTORS AFFECTED
        objCmd = New SqlCommand("spSelectIncidentSectorByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objDR = objCmd.ExecuteReader

        If objDR.HasRows Then
            While objDR.Read()
                lblAffectedSectors.Text += objDR("SectorName") + ", "
            End While
        End If
        objDR.Close()
        objCmd.Dispose()
        objCmd = Nothing

        ' GET SITUATION INVOLVES
        objCmd = New SqlCommand("[spSelectIncidentIncidentTypeByIncidentID]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()

        If objDR.HasRows Then
            While objDR.Read
                lblSitInvolves.Text += objDR("IncidentType") + ", "
            End While
        End If

        objDR.Close()
        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()
    End Sub

    'Export page to word doc
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportWord.Click
        btnExportWord.Visible = False
        EnableViewState = False
        Response.Clear()
        Response.ContentType = "application/vnd.ms-word"
        Response.AddHeader("Content-Disposition", "attachments;filename=HazmatReleaseReport_" & Session("stWarnCode").ToString() & ".doc")
        Response.Buffer = True
        Response.BufferOutput = True
    End Sub

End Class
