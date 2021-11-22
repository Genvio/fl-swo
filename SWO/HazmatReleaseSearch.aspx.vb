Imports System.Runtime.InteropServices
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports System.Configuration

Partial Class HazmatReleaseSearch
    Inherits System.Web.UI.Page
    'For Connecting to the database
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objCmd2 As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDR2 As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet
    Public objDT As New System.Data.DataTable
    Public HelpFunction As New HelpFunctions


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BindCounties()
            ' Popluate gridview with records for the past 90 days
            Dim sb As New StringBuilder("")
            Dim rowCount As Integer = 0
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
            sb.Append(" WHERE ReleaseDate BETWEEN '" & DateTime.Now.AddDays(-90).ToShortDateString() & "' AND '" & DateTime.Now.AddDays(+1).ToShortDateString() & "'")
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objDA = New System.Data.SqlClient.SqlDataAdapter(sb.ToString(), objConn)
            objDA.Fill(objDS)
            rowCount = objDS.Tables(0).Rows.Count.ToString()
            gvReleases.DataSource = objDS
            'gvExport.DataSource = objDS
            gvReleases.DataBind()
            'gvExport.DataBind()
            If rowCount = 0 Then
                lblSearchResults.Text = "There are no records for the past 90 days."
            Else
                lblSearchResults.Text = rowCount.ToString() & " records for the past 90 days."
            End If
            objDS.Clear()
            objConn.Close()
        End If
    End Sub
    ' On Submit button click: Binds gridview based on selected parameters and makes export button visible 
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        btnExport.Visible = True
        BindGridview()
    End Sub

    ' On Export to Excel button click: Creates a workbook, iterates through the gridview and adds data to the workbook 
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=HazmatReleaseExport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)
            gvExport.AllowPaging = False
            BindGridview()
            gvExport.RenderControl(hw)
            Response.Write(sw)
            Response.End()
        End Using

        'NOTE: The remaining code below will populate a actual Excel document. Currently this is not supported in Staging or Production environments

        'Dim rowsTotal, colsTotal As Short
        'Dim I, j, iC As Short
        'System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        'Dim xlApp As New Excel.Application
        'Try
        '    Dim excelBook As Excel.Workbook = xlApp.Workbooks.Add
        '    Dim excelWorksheet As Excel.Worksheet = CType(excelBook.Worksheets(1), Excel.Worksheet)
        '    xlApp.Visible = True
        '    rowsTotal = gvExport.Rows.Count
        '    colsTotal = gvExport.Columns.Count - 1
        '    With excelWorksheet
        '        .Cells.Select()
        '        .Cells.Delete()
        '        For iC = 0 To colsTotal
        '            .Cells(1, iC + 1).Value = gvExport.Columns(iC).HeaderText
        '        Next
        '        For I = 0 To rowsTotal - 1
        '            For j = 0 To colsTotal
        '                .Cells(I + 2, j + 1).value = Replace(gvExport.Rows(I).Cells(j).Text, "&nbsp;", "")
        '            Next j
        '        Next I
        '        .Rows("1:1").Font.FontStyle = "Bold"
        '        .Rows("1:1").Font.Size = 10
        '        .Cells.Columns.AutoFit()
        '        .Cells.Select()
        '        .Cells.EntireColumn.AutoFit()
        '        .Cells(1, 1).Select()
        '    End With
        'Catch ex As Exception
        '    MsgBox("Export Excel Error " & ex.Message)
        'Finally
        '    'RELEASE ALLOACTED RESOURCES
        '    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        '    xlApp = Nothing
        'End Try
    End Sub

    ' Binds the gridview based on selected criteria 
    Protected Sub BindGridview()
        Dim sb As New StringBuilder("")
        Dim rowCount As Integer = 0

        sb.Append("SELECT *, hr.ChemicalReleased as RelChemicalReleased, hr.Address as RelAddress, '=""'+hr.CASNumber+'""' as RelCAS")
        sb.Append(", rp.FirstName as RepFName,rp.LastName as RepLName,rp.CallBackNumber1 as RepCallBackNumber1,rp.CallBackNumber2 as RepCallBackNumber2,rp.Email as RepEmail,rp.Address as RepAddress, rp.City as RepCity, rp.State as RepState, rp.Zipcode as RepZip, rp.Represents as RepRep,rpt.ReportingPartyType")
        sb.Append(", resp.FirstName as RespFName,resp.LastName as RespLName,resp.CallBackNumber1 as RespCallBackNumber1,resp.CallBackNumber2 as RespCallBackNumber2,resp.Email as RespEmail,resp.Address as RespAddress, resp.City as RespCity, resp.State as RespState, resp.Zipcode as RespZip, resp.Represents as RespRep, respt.ResponsiblePartyType")
        sb.Append(", osc.FirstName as oscFName,osc.LastName as oscLName,osc.CallBackNumber1 as oscCallBackNumber1,osc.CallBackNumber2 as oscCallBackNumber2,osc.Email as oscEmail,osc.Address as oscAddress, osc.City as oscCity, osc.State as oscState, osc.Zipcode as oscZip, osc.Represents as oscRep, osct.OnSceneContactType")
        sb.Append(", case (SELECT cast([Alachua] as int) + cast([Baker] as int) + cast([Bay] as int) + cast([Bradford] as int) + cast([Brevard] as int) + cast([Broward] as int) + cast([Calhoun] as int) + cast([Charlotte] as int) + cast([Citrus] as int) + cast([Clay] as int) + cast([Collier] as int) + cast([Columbia] as int) + cast([DeSoto] as int) + cast([Dixie] as int) + cast([Duval] as int) + cast([Escambia] as int) + cast([Flagler] as int) + cast([Franklin] as int) + cast([Gadsden] as int) + cast([Gilchrist] as int) + cast([Glades] as int) + cast([Gulf] as int) + cast([Hamilton] as int) + cast([Hardee] as int) + cast([Hendry] as int) + cast([Hernando] as int) + cast([Highlands] as int) + cast([Hillsborough] as int) + cast([Holmes] as int) + cast([Indian River] as int) + cast([Jackson] as int) + cast([Jefferson] as int) + cast([Lafayette] as int) + cast([Lake] as int) + cast([Lee] as int) + cast([Leon] as int) + cast([Levy] as int) + cast([Liberty] as int) + cast([Madison] as int) + cast([Manatee] as int) + cast([Marion] as int) + cast([Martin] as int) + cast([Miami-Dade] as int) + cast([Monroe] as int) + cast([Nassau] as int) + cast([Okaloosa] as int) + cast([Okeechobee] as int) + cast([Orange] as int) + cast([Osceola] as int) + cast([Palm Beach] as int) + cast([Pasco] as int) + cast([Pinellas] as int) + cast([Polk] as int) + cast([Putnam] as int) + cast([Santa Rosa] as int) + cast([Sarasota] as int) + cast([Seminole] as int) + cast([St. Johns] as int) + cast([St. Lucie] as int) + cast([Sumter] as int) + cast([Suwannee] as int) + cast([Taylor] as int) + cast([Union] as int) + cast([Volusia] as int) + cast([Wakulla] as int) + cast([Walton] as int) + cast([Washington] as int) FROM [dbo].[CountyRegionCheck] where [IncidentID] = i.[IncidentID] ) when 0 then 'No county recorded' when 1 then cast((select LEPCName from [dbo].[LocalEmergencyPlanningCommittee] where [LEPCID] = (select FK_LEPCID from [dbo].[county] where [county] = ltrim(rtrim(i.[AddedCounty])))) as varchar(250)) else 'Multple counties affected' end as LEPCName")
        sb.Append(", case (SELECT cast([Alachua] as int) + cast([Baker] as int) + cast([Bay] as int) + cast([Bradford] as int) + cast([Brevard] as int) + cast([Broward] as int) + cast([Calhoun] as int) + cast([Charlotte] as int) + cast([Citrus] as int) + cast([Clay] as int) + cast([Collier] as int) + cast([Columbia] as int) + cast([DeSoto] as int) + cast([Dixie] as int) + cast([Duval] as int) + cast([Escambia] as int) + cast([Flagler] as int) + cast([Franklin] as int) + cast([Gadsden] as int) + cast([Gilchrist] as int) + cast([Glades] as int) + cast([Gulf] as int) + cast([Hamilton] as int) + cast([Hardee] as int) + cast([Hendry] as int) + cast([Hernando] as int) + cast([Highlands] as int) + cast([Hillsborough] as int) + cast([Holmes] as int) + cast([Indian River] as int) + cast([Jackson] as int) + cast([Jefferson] as int) + cast([Lafayette] as int) + cast([Lake] as int) + cast([Lee] as int) + cast([Leon] as int) + cast([Levy] as int) + cast([Liberty] as int) + cast([Madison] as int) + cast([Manatee] as int) + cast([Marion] as int) + cast([Martin] as int) + cast([Miami-Dade] as int) + cast([Monroe] as int) + cast([Nassau] as int) + cast([Okaloosa] as int) + cast([Okeechobee] as int) + cast([Orange] as int) + cast([Osceola] as int) + cast([Palm Beach] as int) + cast([Pasco] as int) + cast([Pinellas] as int) + cast([Polk] as int) + cast([Putnam] as int) + cast([Santa Rosa] as int) + cast([Sarasota] as int) + cast([Seminole] as int) + cast([St. Johns] as int) + cast([St. Lucie] as int) + cast([Sumter] as int) + cast([Suwannee] as int) + cast([Taylor] as int) + cast([Union] as int) + cast([Volusia] as int) + cast([Wakulla] as int) + cast([Walton] as int) + cast([Washington] as int) FROM [dbo].[CountyRegionCheck] where [IncidentID] = i.[IncidentID] ) when 1 then (select LEPCNumber from [dbo].[LocalEmergencyPlanningCommittee] where [LEPCID] = (select FK_LEPCID from [dbo].[county] where [county] = ltrim(rtrim(i.[AddedCounty])))) else 0 end as LEPCNumber")
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
        sb.Append(" WHERE hr.IncidentID IS NOT NULL")
        If Not txtSearchStartDate.Text = "" And txtSearchEndDate.Text = "" Then
            Dim startDate As DateTime = Convert.ToDateTime(txtSearchStartDate.Text)
            sb.Append(" AND DATEPART(YEAR, ReleaseDate) ='" & startDate.Year.ToString() & "'")
            sb.Append(" AND DATEPART(MONTH, ReleaseDate) ='" & startDate.Month.ToString() & "'")
            sb.Append(" AND DATEPART(DAY, ReleaseDate) ='" & startDate.Day.ToString() & "'")
        ElseIf Not txtSearchStartDate.Text = "" And Not txtSearchEndDate.Text = "" Then
            Dim endDate As DateTime = Convert.ToDateTime(txtSearchEndDate.Text)
            sb.Append(" AND ReleaseDate BETWEEN '" & txtSearchStartDate.Text & "' AND '" & endDate.AddDays(1).ToShortDateString() & "'")
        End If
        If Not txtSearchChemical.Text.Trim() = "" Then
            sb.Append(" AND hr.ChemicalReleased LIKE '%" & txtSearchChemical.Text.Trim() & "%'")
        End If
        If Not txtSearchCAS.Text.Trim() = "" Then
            sb.Append(" AND hr.CASNumber = '" & txtSearchCAS.Text.Trim() & "'")
        End If
        If Not txtFacilityName.Text.Trim() = "" Then
            sb.Append(" AND FacilityName LIKE '%" & txtFacilityName.Text.Trim() & "%'")
        End If
        If Not txtAddress.Text.Trim() = "" Then
            sb.Append(" AND hr.Address LIKE '%" & txtAddress.Text.Trim() & "%'")
        End If
        If Not txtSERCNum.Text.Trim() = "" Then
            sb.Append(" AND SERCNum LIKE '%" & txtSERCNum.Text.Trim() & "%'")
        End If
        If Not txtRMPNum.Text.Trim() = "" Then
            sb.Append(" AND RMPNum LIKE '%" & txtRMPNum.Text.Trim() & "%'")
        End If
        If Not txtTRINum.Text.Trim() = "" Then
            sb.Append(" AND TRINum LIKE '%" & txtTRINum.Text.Trim() & "%'")
        End If
        If Not txtTierTwoNum.Text.Trim() = "" Then
            sb.Append(" AND TIER2EPlanNum LIKE '%" & txtTierTwoNum.Text.Trim() & "%'")
        End If
        If Not txtSearchNarrative.Text.Trim() = "" Then
            sb.Append(" AND DescriptionNarrative LIKE '%" & txtSearchNarrative.Text.Trim() & "%'")
        End If
        If Not txtStWarnCode.Text.Trim() = "" Then
            sb.Append(" AND STWarnPointCode LIKE '%" & txtStWarnCode.Text.Trim() & "%'")
        End If
        If cbPetroleum.Checked Then
            sb.Append(" AND PetroleumRelease = 1")
        End If
        If cbLNG.Checked Then
            sb.Append(" AND LPLNGPropaneRelease = 1")
        End If
        If cbToxic.Checked Then
            sb.Append(" AND Toxic = 1")
        End If
        If cbRespPartyCall.Checked Then
            sb.Append(" AND RespPartyCalled = 1")
        End If
        If cbFlammable.Checked Then
            sb.Append(" AND Flammable = 1")
        End If
        If cbSevenDay.Checked Then
            sb.Append(" AND FollowUpReportFiled = 1")
        End If
        If cbNRCNotify.Checked Then
            sb.Append(" AND NRCNotified = 1")
        End If
        If cbCercla.Checked Then
            sb.Append(" AND CERCLA304Release = 1")
        End If
        If cbOffsite.Checked Then
            sb.Append(" AND OffsiteRelease = 1")
        End If
        If cbInjury.Checked Then
            sb.Append(" AND NumberInjured IS NOT NULL AND NumberInjured > 0")
        End If
        If cbFatality.Checked Then
            sb.Append(" AND FatalitiesVerifiedNum IS NOT NULL AND FatalitiesVerifiedNum > 0")
        End If
        If cbEvacuate.Checked Then
            sb.Append(" AND NumberEvacuated IS NOT NULL AND NumberEvacuated > 0")
        End If
        If cbCounty.Checked Then
            Dim MyItem As ListItem
            Dim itemCount As Integer = 0
            For Each MyItem In cblCounties.Items
                If MyItem.Selected = True Then
                    itemCount += 1
                    If itemCount = 1 Then
                        sb.Append(" AND (AddedCounty LIKE '%" & MyItem.Text.Trim() & "%'")
                    Else
                        sb.Append(" OR AddedCounty LIKE '%" & MyItem.Text.Trim() & "%'")
                    End If
                End If
            Next
            If itemCount > 0 Then
                sb.Append(")")
            End If
        End If
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objDA = New System.Data.SqlClient.SqlDataAdapter(sb.ToString(), objConn)
        objDA.Fill(objDS)
        rowCount = objDS.Tables(0).Rows.Count.ToString()
        gvReleases.DataSource = objDS
        gvExport.DataSource = objDS
        gvReleases.DataBind()
        gvExport.DataBind()
        If rowCount = 0 Then
            lblSearchResults.Text = "No records matched your criteria."
            btnExport.Visible = False
        Else
            lblSearchResults.Text = rowCount.ToString() & " record(s) matched your criteria."
        End If
        objDS.Clear()
        objConn.Close()
    End Sub

    ' Binds the checkbox list for counties
    Protected Sub BindCounties()
        Dim sb As New StringBuilder("")
        Dim dr As DataRow
        sb.Append("SELECT * FROM [dbo].[County]")
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objDA = New System.Data.SqlClient.SqlDataAdapter(sb.ToString(), objConn)
        objDA.Fill(objDT)
        For Each dr In objDT.Rows
            Dim item As New ListItem()
            item.Text = dr("County").ToString().Trim()
            item.Value = dr("County").ToString().Trim()
            item.Attributes.Add("class", "inline-cb")
            cblCounties.Items.Add(item)
        Next
        objDS.Clear()
        objConn.Close()
    End Sub

    ' Manages page index for gridview paging
    Protected Sub gvReleases_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvReleases.PageIndexChanging
        gvReleases.PageIndex = e.NewPageIndex
        BindGridview()
    End Sub

    ' Toggles county search panel
    Protected Sub cbCounty_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbCounty.CheckedChanged
        If cbCounty.Checked Then
            pnlCounty.Visible = True
        ElseIf Not cbCounty.Checked Then
            pnlCounty.Visible = False
        End If
    End Sub

    'This sub must be in place to export Excel data from gridview
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub

    Protected Sub gvExport_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim incidentID As Integer = CInt(DataBinder.Eval(e.Row.DataItem, "IncidentID"))

            'format CASNumber as string - export to excel is changing some values as date 
            e.Row.Cells(23).Text = "=""" + e.Row.Cells(23).Text + """"

            'format reported to swo date and time
            If Not HelpFunction.Convertdbnulls(Replace(e.Row.Cells(2).Text, "&nbsp;", "")) = "" Then
                e.Row.Cells(2).Text = CDate(DataBinder.Eval(e.Row.DataItem, "ReportedToSWODate")).ToShortDateString() & " " & Left(CStr(HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "ReportedToSWOTime"))), 2) & ":" & Right(CStr(HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "ReportedToSWOTime"))), 2)
            End If

            ' GET SITUATION INVOLVES
            objCmd = New SqlCommand("[spSelectIncidentIncidentTypeByIncidentID]", objConn)
            objCmd.Parameters.AddWithValue("@IncidentID", incidentID)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()
            Dim sb As New StringBuilder("")
            If objDR.HasRows Then
                While objDR.Read
                    sb.Append(objDR("IncidentType") + ", ")
                End While
                sb.Remove(sb.Length - 2, 1) 'Remove trailing comma
            End If
            e.Row.Cells(5).Text = sb.ToString()
            objDR.Close()
            objCmd.Dispose()
            objCmd = Nothing

            ' GET SECTORS AFFECTED
            sb.Length = 0
            objCmd = New SqlCommand("spSelectIncidentSectorByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", incidentID)
            objDR = objCmd.ExecuteReader

            If objDR.HasRows Then
                While objDR.Read()
                    sb.Append(objDR("SectorName") + ", ")
                End While
                sb.Remove(sb.Length - 2, 1) 'Remove trailing comma
            End If
            e.Row.Cells(6).Text = sb.ToString()
            objDR.Close()
            objCmd.Dispose()
            objCmd = Nothing

            ' GET INITIAL REPORT
            sb.Length = 0
            sb.Append("SELECT * FROM [dbo].[InitialReport] WHERE InitialReportID IN")
            sb.Append(" (SELECT MIN(InitialReportID) FROM [dbo].[InitialReport] WHERE IncidentID = " & incidentID & " AND InitialReport <> '')")
            objCmd = New SqlCommand(sb.ToString(), objConn)
            objCmd.CommandType = CommandType.Text
            objDR = objCmd.ExecuteReader
            If objDR.Read Then
                e.Row.Cells(7).Text = HelpFunction.Convertdbnulls(objDR("InitialReport"))
            End If
            objDR.Close()
            objCmd.Dispose()
            objCmd = Nothing

            'e.Row.Cells(8).Text = Replace(HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "Evacuations")), "Select an Option", "")

            'If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "Injury")) = "" Then
            '    sb.Length = 0
            '    sb.Append(Replace(DataBinder.Eval(e.Row.DataItem, "Injury"), "Select an Option", ""))
            '    If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "InjuryText")) = "" Then
            '        sb.Append(", " & HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "InjuryText")))
            '    End If
            '    e.Row.Cells(9).Text = sb.ToString()
            'End If

            'If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "Fatality")) = "" Then
            '    sb.Length = 0
            '    sb.Append(Replace(DataBinder.Eval(e.Row.DataItem, "Fatality"), "Select an Option", ""))
            '    If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "FatalityText")) = "" Then
            '        sb.Append(", " & HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "FatalityText")))
            '    End If
            '    e.Row.Cells(10).Text = sb.ToString()
            'End If

            If HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "EnvironmentalImpact")) = "Yes" Then
                sb.Length = 0
                sb.Append(DataBinder.Eval(e.Row.DataItem, "EnvironmentalImpact"))
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "DEPCallbackRequested")) = "Select an Option" Then
                    If DataBinder.Eval(e.Row.DataItem, "DEPCallbackRequested") = "Yes" Then
                        sb.Append("<br style=""mso-data-placement:same-cell;"" />DEP callback requested: Yes")
                        If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "EnvironmentalImpactContact")) = "Select an Option" Then
                            sb.Append("<br style=""mso-data-placement:same-cell;"" />Contact: ")
                            sb.Append(DataBinder.Eval(e.Row.DataItem, "EnvironmentalImpactContact"))
                        End If
                    Else
                        sb.Append("<br style=""mso-data-placement:same-cell;"" />DEP callback requested: No ")
                    End If
                End If
                e.Row.Cells(8).Text = sb.ToString()
            Else
                e.Row.Cells(8).Text = HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "EnvironmentalImpact"))
            End If

            If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "UpdateReport")) = "" Then
                e.Row.Cells(9).Text = HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "UpdateDate")) & " - " & DataBinder.Eval(e.Row.DataItem, "UpdateReport")
            End If

            If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "Lat")) = "" And Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "Long")) = "" Then
                e.Row.Cells(15).Text = "Lat: " & DataBinder.Eval(e.Row.DataItem, "Lat") & ", Long: " & DataBinder.Eval(e.Row.DataItem, "Long")
            End If

            ' REPORTING PARTY
            If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "ReportingPartyType")) = "As Below" Then
                e.Row.Cells(16).Text = DataBinder.Eval(e.Row.DataItem, "ReportingPartyType")
            Else
                sb.Length = 0
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RepFName")) = "" Then
                    sb.Append(DataBinder.Eval(e.Row.DataItem, "RepFName"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RepLName")) = "" Then
                    sb.Append(" " & DataBinder.Eval(e.Row.DataItem, "RepLName"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RepRep")) = "" Then
                    sb.Append(", " & DataBinder.Eval(e.Row.DataItem, "RepRep"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RepCallBackNumber1")) = "" Then
                    sb.Append("<br style='mso-data-placement:same-cell;'>" & DataBinder.Eval(e.Row.DataItem, "RepCallBackNumber1"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RepCallBackNumber2")) = "" Then
                    sb.Append(" or " & DataBinder.Eval(e.Row.DataItem, "RepCallBackNumber2"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RepEmail")) = "" Then
                    sb.Append("<br style='mso-data-placement:same-cell;'>" & DataBinder.Eval(e.Row.DataItem, "RepEmail"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RepAddress")) = "" Then
                    sb.Append("<br style='mso-data-placement:same-cell;'>" & DataBinder.Eval(e.Row.DataItem, "RepAddress"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RepCity")) = "" Then
                    sb.Append(", " & DataBinder.Eval(e.Row.DataItem, "RepCity"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RepState")) = "" Then
                    sb.Append(", " & DataBinder.Eval(e.Row.DataItem, "RepState"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RepZip")) = "" Then
                    sb.Append(" " & DataBinder.Eval(e.Row.DataItem, "RepZip"))
                End If
                e.Row.Cells(16).Text = sb.ToString()
            End If
            ' RESPONSIBLE PARTY
            If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "ResponsiblePartyType")) = "As Below" Then
                e.Row.Cells(17).Text = DataBinder.Eval(e.Row.DataItem, "ResponsiblePartyType")
            Else
                sb.Length = 0
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RespFName")) = "" Then
                    sb.Append(DataBinder.Eval(e.Row.DataItem, "RespFName"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RespLName")) = "" Then
                    sb.Append(" " & DataBinder.Eval(e.Row.DataItem, "RespLName"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RespRep")) = "" Then
                    sb.Append(", " & DataBinder.Eval(e.Row.DataItem, "RespRep"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RespCallBackNumber1")) = "" Then
                    sb.Append("<br style='mso-data-placement:same-cell;'>" & DataBinder.Eval(e.Row.DataItem, "RespCallBackNumber1"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RespCallBackNumber2")) = "" Then
                    sb.Append(" or " & DataBinder.Eval(e.Row.DataItem, "RespCallBackNumber2"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RespEmail")) = "" Then
                    sb.Append("<br style='mso-data-placement:same-cell;'>" & DataBinder.Eval(e.Row.DataItem, "RespEmail"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RespAddress")) = "" Then
                    sb.Append("<br style='mso-data-placement:same-cell;'>" & DataBinder.Eval(e.Row.DataItem, "RespAddress"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RespCity")) = "" Then
                    sb.Append(", " & DataBinder.Eval(e.Row.DataItem, "RespCity"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RespState")) = "" Then
                    sb.Append(", " & DataBinder.Eval(e.Row.DataItem, "RespState"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "RespZip")) = "" Then
                    sb.Append(" " & DataBinder.Eval(e.Row.DataItem, "RespZip"))
                End If
                e.Row.Cells(17).Text = sb.ToString()
            End If
            ' ON SCENE CONTACT
            If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "OnSceneContactType")) = "As Below" Then
                e.Row.Cells(18).Text = DataBinder.Eval(e.Row.DataItem, "OnSceneContactType")
            Else
                sb.Length = 0
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "oscFName")) = "" Then
                    sb.Append(DataBinder.Eval(e.Row.DataItem, "oscFName"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "oscLName")) = "" Then
                    sb.Append(" " & DataBinder.Eval(e.Row.DataItem, "oscLName"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "oscRep")) = "" Then
                    sb.Append(", " & DataBinder.Eval(e.Row.DataItem, "oscRep"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "oscCallBackNumber1")) = "" Then
                    sb.Append("<br style='mso-data-placement:same-cell;'>" & DataBinder.Eval(e.Row.DataItem, "oscCallBackNumber1"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "oscCallBackNumber2")) = "" Then
                    sb.Append(" or " & DataBinder.Eval(e.Row.DataItem, "oscCallBackNumber2"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "oscEmail")) = "" Then
                    sb.Append("<br style='mso-data-placement:same-cell;'>" & DataBinder.Eval(e.Row.DataItem, "oscEmail"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "oscAddress")) = "" Then
                    sb.Append("<br style='mso-data-placement:same-cell;'>" & DataBinder.Eval(e.Row.DataItem, "oscAddress"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "oscCity")) = "" Then
                    sb.Append(", " & DataBinder.Eval(e.Row.DataItem, "oscCity"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "oscState")) = "" Then
                    sb.Append(", " & DataBinder.Eval(e.Row.DataItem, "oscState"))
                End If
                If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "oscZip")) = "" Then
                    sb.Append(" " & DataBinder.Eval(e.Row.DataItem, "oscZip"))
                End If
                e.Row.Cells(18).Text = sb.ToString()
            End If

            If Not HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "WaterwaysAffected")) = "No" Then
                e.Row.Cells(33).Text = DataBinder.Eval(e.Row.DataItem, "WaterwaysAffectedText")
            Else
                e.Row.Cells(33).Text = HelpFunction.Convertdbnulls(DataBinder.Eval(e.Row.DataItem, "WaterwaysAffected"))
            End If

        End If
    End Sub

End Class
