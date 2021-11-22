<%@ Page Title="" Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="HazmatReleaseSearch.aspx.vb" Inherits="HazmatReleaseSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
fieldset
{
    width:90%;
    margin: auto;
    padding: 10px;
}
p{
 margin:1em 0;
}

.centerBTN
{
    padding-left:50px;
}

.label{
 float:left;
 width:165px;
 text-align:right;
 margin-right:0.5em;
 white-space:nowrap;
}

.label_right{
 float:left;
 width:135px;
 text-align:right;
 margin-right:0.5em;
 white-space:nowrap;
}

input[type="text"]
{
 width: 15em;
}

.left-column, right-column
{
 float:left;
}

.left-column
{
 margin-right:1em;
 margin-left:4em;
}

.textarea-label
{
 float:none;
}

textarea
{
 height:5em;
 width:35em;
}

.inline-cb input[type="checkbox"] {
    width: auto;
}

.inline-cb label {
    display: inline;
    color:Black;
    font-weight:normal;
}
.column-left {
    float: left; 
    width: 39%; 
    padding-bottom:5px;
}
.column-right  {
    float: right; 
    width: 60%; 
    padding-bottom:5px;
}
.cbBox
{
    width:60%;
    margin: auto;
    padding: 10px;
}

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1 style="font-size:6;text-align:center;">Hazmat Releases</h1>
<br />
<fieldset>
<div class="left-column">
    <p>
        <asp:Label ID="lblSearchStartDate" runat="server" Text="Start/Release Date:" CssClass="label"></asp:Label>
        <asp:TextBox runat="server" ID="txtSearchStartDate" CssClass="dateText" onmouseover="Tip('Format: MM/DD/YYYY <br/> ie.) 09/21/2009 ')" onmouseout="UnTip()"></asp:TextBox>
        <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtSearchStartDate');"><img alt="Calendar Icon"
        src="Images/Calendar1.jpg" border="0" width="20" height="15"/></a>
        <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtSearchStartDate.value = ''"
        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
    </p>
    <p>
        <asp:Label ID="lblSearchChemical" runat="server" Text="Chemical Released:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtSearchChemical" runat="server"></asp:TextBox>
    
    </p>
    <p>
        <asp:Label ID="lblFacilityName" runat="server" Text="Facility Name:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtFacilityName" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:Label ID="lblAddress" runat="server" Text="Address:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:Label ID="lblSearchNarrative" runat="server" Text="Search Narrative:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtSearchNarrative" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:Label ID="lblStWarnCode" runat="server" Text="State Warning Pt Code:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtStWarnCode" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:Label ID="lblCauseDeath" runat="server" Text="Cause of Death:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtCauseDeath" runat="server"></asp:TextBox>
    </p>
</div>

<div class="right-column">
    <p>
        <asp:Label ID="lblSearchEndDate" runat="server" Text="End Date:" CssClass="label_right"></asp:Label>
        <asp:TextBox runat="server" ID="txtSearchEndDate" CssClass="dateText" onmouseover="Tip('Format: MM/DD/YYYY <br/> ie.) 09/21/2009 ')" onmouseout="UnTip()"></asp:TextBox>
        <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtSearchEndDate');"><img alt="Calendar Icon"
        src="Images/Calendar1.jpg" border="0" width="20" height="15"/></a>
        <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtSearchEndDate.value = ''"
        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
    </p>
        <asp:Label ID="lblSearchCAS" runat="server" Text="CAS #:" CssClass="label_right"></asp:Label>
        <asp:TextBox ID="txtSearchCAS" runat="server"></asp:TextBox>
    <p>
    </p>
    <p>
        <asp:Label ID="lblSERCNum" runat="server" Text="SERC #:" CssClass="label_right"></asp:Label>
        <asp:TextBox ID="txtSERCNum" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:Label ID="lblRMPNum" runat="server" Text="RMP #:" CssClass="label_right"></asp:Label>
        <asp:TextBox ID="txtRMPNum" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:Label ID="lblTRINum" runat="server" Text="TRI #:" CssClass="label_right"></asp:Label>
        <asp:TextBox ID="txtTRINum" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:Label ID="lblTierTwoNum" runat="server" Text="TIER II (EPCRA) #:" CssClass="label_right"></asp:Label>
        <asp:TextBox ID="txtTierTwoNum" runat="server"></asp:TextBox>
    </p>
    <p>
        &nbsp;
    </p>
</div>
<div class="field">
    <div class="cbBox">
        <div class="column-left">
            <asp:CheckBox ID="cbPetroleum" Text="Petroleum release" CssClass="inline-cb" runat="server" />
        </div>
        <div class="column-right">
            <asp:CheckBox ID="cbLNG" Text="Liquid petroleum/LNG/Propane" CssClass="inline-cb" runat="server" />
        </div>
        <div class="column-left">
            <asp:CheckBox ID="cbToxic" Text="Toxic" CssClass="inline-cb" runat="server" />
        </div>
        <div class="column-right">
            <asp:CheckBox ID="cbRespPartyCall" Text="Responsible Party called in release" CssClass="inline-cb" runat="server" />
        </div>
        <div class="column-left">
            <asp:CheckBox ID="cbFlammable" Text="Flammable" CssClass="inline-cb" runat="server" />
        </div>
        <div class="column-right">
            <asp:CheckBox ID="cbSevenDay" Text="7 Day Followup Report Filed by Facility" CssClass="inline-cb" runat="server" />
        </div>
        <div class="column-left">
            <asp:CheckBox ID="cbNRCNotify" Text="NRC notified" CssClass="inline-cb" runat="server" />
        </div>
        <div class="column-right">
            <asp:CheckBox ID="cbCercla" Text="CERCLA/304 Release" CssClass="inline-cb" runat="server" />
        </div>
        <div class="column-left">
            <asp:CheckBox ID="cbOffsite" Text="Offsite release" CssClass="inline-cb" runat="server" />
        </div>
        <div class="column-right">
            <asp:CheckBox ID="cbEvacuate" Text="Evacuations" CssClass="inline-cb" runat="server" />
        </div>
        <div class="column-left">
            <asp:CheckBox ID="cbInjury" Text="Injuries" CssClass="inline-cb" runat="server" />
        </div>
        <div class="column-right">
            <asp:CheckBox ID="cbFatality" Text="Fatalities" CssClass="inline-cb" runat="server" />
        </div>
        <div class="column-left">
            <asp:CheckBox ID="cbCounty" Text="County Search" CssClass="inline-cb" runat="server" AutoPostBack="true" />
        </div>
        <div class="column-right">
            &nbsp;
        </div>
    </div>
</div>
<div style="padding-left:50px;">
<asp:Panel ID="pnlCounty" runat="server" Visible="false">
<fieldset>
    <asp:checkboxlist id="cblCounties" runat="server" RepeatLayout="table" RepeatColumns="5" RepeatDirection="horizontal" CellPadding="5" CellSpacing="10"/>
</fieldset>
</asp:Panel>
</div>
</fieldset>
<br />
<div style="margin: 0 auto; text-align: center;">
        <asp:Button ID="btnSearch" runat="server" Text="Search" style="width:100px; font-weight:bold;"/>
</div>
<br />
<div style="text-align: center;">
    <asp:Label ID="lblSearchResults" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnExport"
        runat="server" Text="Export to Excel" /><br /><br />
        <asp:GridView ID="gvReleases" runat="server" HorizontalAlign="Center" 
        CellPadding="3" AlternatingRowStyle-BackColor="LightGray" 
        AutoGenerateColumns="False" EnableModelValidation="True" BackColor="White" 
        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" OnPageIndexChanging="gvReleases_PageIndexChanging" AllowPaging="True" PageSize="10">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="IncidentID,IncidentIncidentTypeID" 
                    DataNavigateUrlFormatString="HazmatRelease.aspx?IncidentID={0}&amp;IncidentIncidentTypeID={1}" 
                    Target="_blank" Text="View" />
                 <asp:BoundField DataField="STWarnPointCode" HeaderText="Report #" 
                    SortExpression="STWarnPointCode" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="ReleaseDate" HeaderText="Release Date" 
                    SortExpression="ReleaseDate" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
               <asp:BoundField DataField="ReleaseSource" HeaderText="Release Source" 
                    SortExpression="ReleaseSource" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Sector" HeaderText="Sector" 
                    SortExpression="Sector" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="DescriptionNarrative" HeaderText="Description" 
                    SortExpression="DescriptionNarrative" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="ChemicalName" HeaderText="Chemical" 
                    SortExpression="ChemicalName" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="AmountReleased" HeaderText="Amount Released" 
                    SortExpression="AmountReleased" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="TIER2EPLANNum" HeaderText="TIER 2 EPLAN #" 
                    SortExpression="TIER2EPLANNum" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FacilityName" HeaderText="Facility Name" 
                    SortExpression="FacilityName" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="NumberInjured" HeaderText="Injured" 
                    SortExpression="NumberInjured" >
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FatalitiesVerifiedNum" HeaderText="Fatalities" 
                    SortExpression="FatalitiesVerifiedNum" >
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
</div>
<br /><br />
   <asp:Panel ID="Panel1" runat="server" Visible="false">
         <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="False" EnableModelValidation="True" OnRowDataBound="gvExport_RowDataBound">
            <Columns>
                <asp:BoundField DataField="STWarnPointCode" HeaderText="Report #" 
                    SortExpression="STWarnPointCode" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="IncidentStatus" HeaderText="Status" 
                    SortExpression="IncidentStatus" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="ReportedToSWODate" HeaderText="Reported to SWO" 
                    SortExpression="ReportedToSWODate" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Severity" HeaderText="Severity" 
                    SortExpression="Severity" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="IncidentName" HeaderText="Description" 
                    SortExpression="IncidentName" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="IncidentName" HeaderText="Situation involves" 
                    SortExpression="" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="IncidentName" HeaderText="Affected Sectors" 
                    SortExpression="" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="IncidentName" HeaderText="Initial Report" 
                    SortExpression="" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="EnvironmentalImpact" HeaderText="Environmental impact" 
                    SortExpression="EnvironmentalImpact" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="UpdateReport" HeaderText="Most Recent Update" 
                    SortExpression="UpdateReport" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="AddedCounty" HeaderText="Affected Counties" 
                    SortExpression="AddedCounty" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="LEPCNumber" HeaderText="LEPC Number" 
                    SortExpression="LEPCNumber" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="LEPCName" HeaderText="LEPC Name" 
                    SortExpression="LEPCName" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FacilityNameSceneDescription" HeaderText="Facility Name or Description" 
                    SortExpression="FacilityNameSceneDescription" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Address" HeaderText="Incident Location" 
                    SortExpression="Address" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Long" HeaderText="Coordinates" 
                    SortExpression="Long" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="RepLName" HeaderText="Reporting Party" 
                    SortExpression="RepLName" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="RespLName" HeaderText="Responsible Party" 
                    SortExpression="RespLName" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="oscLName" HeaderText="On-Scene Contact" 
                    SortExpression="oscLName" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="SubType" HeaderText="Sub-Type" 
                    SortExpression="SubType" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Situation" HeaderText="Situation" 
                    SortExpression="Situation" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="ChemicalName" HeaderText="Chemical Name" 
                    SortExpression="ChemicalName" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="IndexName" HeaderText="Index Name" 
                    SortExpression="IndexName" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="CASNumber" HeaderText="CAS Number" 
                    SortExpression="CASNumber" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Section304ReportableQuantity" HeaderText="Section 304 Reportable Quantity" 
                    SortExpression="Section304ReportableQuantity" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="CERCLAReportableQuantity" HeaderText="CERCLA Reportable Quantity" 
                    SortExpression="CERCLAReportableQuantity" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="ChemicalState" HeaderText="Chemical State" 
                    SortExpression="ChemicalState" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="SourceContainer" HeaderText="Source/Container" 
                    SortExpression="SourceContainer" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="ChemicalQuantityReleased" HeaderText="Quantity released" 
                    SortExpression="ChemicalQuantityReleased" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="ChemicalRateOfRelease" HeaderText="Rate of release" 
                    SortExpression="ChemicalRateOfRelease" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="ChemicalReleased" HeaderText="Released" 
                    SortExpression="ChemicalReleased" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="CauseOfRelease" HeaderText="Cause of release" 
                    SortExpression="CauseOfRelease" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="StormDrainsAffected" HeaderText="Storm drains affected" 
                    SortExpression="StormDrainsAffected" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="WaterwaysAffected" HeaderText="Waterways affected" 
                    SortExpression="WaterwaysAffected" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="MajorRoadwaysClosed" HeaderText="Major roadways closed" 
                    SortExpression="MajorRoadwaysClosed" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="DescriptionNarrative" HeaderText="Investigation Details" 
                    SortExpression="DescriptionNarrative" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FacilityName" HeaderText="Facility Name" 
                    SortExpression="FacilityName" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                 <asp:BoundField DataField="BusinessType" HeaderText="Business Type" 
                    SortExpression="BusinessType" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="RelAddress" HeaderText="Address" 
                    SortExpression="RelAddress" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
               <asp:BoundField DataField="ReleaseSource" HeaderText="Release Source" 
                    SortExpression="ReleaseSource" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Sector" HeaderText="Sector" 
                    SortExpression="Sector" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="ChemicalReleased" HeaderText="Chemical Released" 
                    SortExpression="ChemicalReleased" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="RelCAS" HeaderText="CAS #" 
                    SortExpression="RelCAS" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="AmountReleased" HeaderText="Amount Released" 
                    SortExpression="AmountReleased" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="ReleaseDate" HeaderText="Release Date" 
                    SortExpression="ReleaseDate" >
                <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="NumberEvacuated" HeaderText="Evacuated" 
                    SortExpression="NumberEvacuated" >
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="NumberInjured" HeaderText="Injured" 
                    SortExpression="NumberInjured" >
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FatalitiesVerifiedNum" HeaderText="Fatalities" 
                    SortExpression="FatalitiesVerifiedNum" >
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="CauseOfDeath" HeaderText="Cause Of Death" 
                    SortExpression="CauseOfDeath" >
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="CauseDeathVerified" HeaderText="COD Verified" 
                    SortExpression="CauseDeathVerified" >
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="SERCNum" HeaderText="SERC #" 
                    SortExpression="SERCNum" >
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="RMPNum" HeaderText="RMP #" 
                    SortExpression="RMPNum" >
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="TRINum" HeaderText="TRI #"
                    SortExpression="TRINum" >
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="TIER2EPLANNum" HeaderText="TIER II EPLAN #" 
                    SortExpression="TIER2EPLANNum" >
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="PetroleumRelease" HeaderText="Petroleum Release" 
                    SortExpression="PetroleumRelease" >
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="LPLNGPropaneRelease" 
                    HeaderText="Liquid petroleum/LNG/Propane" />
                <asp:BoundField DataField="Toxic" HeaderText="Toxic" />
                <asp:BoundField DataField="RespPartyCalled" 
                    HeaderText="Responsible Party Called" SortExpression="RespPartyCalled" />
                <asp:BoundField DataField="Flammable" HeaderText="Flammable" />
                <asp:BoundField DataField="FollowUpReportFiled" 
                    HeaderText="Follow Up Report Filed" />
                <asp:BoundField DataField="NRCNotified" HeaderText="NRC Notified" />
                <asp:BoundField DataField="CERCLA304Release" HeaderText="CERCLA304 Release" />
                <asp:BoundField DataField="OffsiteRelease" HeaderText="Offsite Release" />
                <asp:BoundField DataField="SEP" HeaderText="SEP" />
                <asp:BoundField DataField="FollowUpReportDueDate" 
                    HeaderText="FollowUp Report Due" SortExpression="FollowUpReportDueDate">
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </asp:Panel>

</asp:Content>
