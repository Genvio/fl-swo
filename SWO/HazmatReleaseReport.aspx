<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HazmatReleaseReport.aspx.vb" Inherits="HazmatReleaseReport" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Hazmat Release Report</title>
<style type="text/css">
h1
{
    margin:0px,0px,0px,0px;
}
h2
{
    background-color:Silver;
    margin:0px,0px,0px,0px;
}
fieldset {
  overflow: hidden;
  width: 95%;
}

.field 
{
    padding-bottom:5px;
}

.indentinfo 
{
    padding-top:5px;
    padding-left:20px;
    padding-bottom:5px;
}

.label {
  clear: both;
  width: 250px;
  float:left;
  vertical-align:top;
  text-align:right;
  padding-right:5px;
}

.repLable
{
    font-size:larger;
    font-weight:bold;
}
input.numText
{
    width: 50px;
}

input.dateText
{
    width: 65px;
}

select 
{
    background-color: #c2ecde;
}

textarea 
{
    height: 100px;
    width: 456px;
}

.cbBox
{
    width:35%;
    padding: 10px;
}

.buttonBox
{
    width:30%;
    margin: auto;
    padding: 10px;
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
.reportheader
{
    font-size:x-large;
    background-color:Silver;
    width:10%;
}

.infoBlock
{
    padding-top:5px;
    padding-left:20px;
    padding-bottom:5px;
}

</style>
</head>
<body>
<h1 align="center">Florida Division of Emergency Management<br />State Watch Office Incident Report</h1>
<form id="butForm" runat="server">
    <asp:button runat="server" ID="btnExportWord" text="Export to Word"/>
<h1>Original Report</h1>
<h2>Main Information</h2>
    <div class="field">
        <span class="repLable">Report #: </span> <asp:Label ID="lblReportNum" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Status: </span> <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Reported to SWO on: </span> <asp:Label ID="lblReportDateTime" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Severity: </span> <asp:Label ID="lblSeverity" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Description: </span> <asp:Label ID="lblDescription" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">This situation involves: </span> <asp:Label ID="lblSitInvolves" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Affected Sectors: </span> <asp:Label ID="lblAffectedSectors" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Initial Report: </span> <asp:Label ID="lblInitialReport" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Evacuations: </span> <asp:Label ID="lblEvac" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Injuries: </span> <asp:Label ID="lblInjury" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Fatalities: </span> <asp:Label ID="lblFatality" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Environmental impact: </span> <asp:Label ID="lblEnviroImpact" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Most Recent Update: </span> <asp:Label ID="lblUpdate" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Affected Counties: </span> <asp:Label ID="lblCounty" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Facility Name or Description: </span> <asp:Label ID="lblFacilityName" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Incident Location: </span> <asp:Label ID="lblIncidentLocation" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Coordinates: </span> <asp:Label ID="lblCoords" runat="server" Text=""></asp:Label>
    </div>

<h2>Contact Information</h2>
    <div class="field">
        <span class="repLable">Reporting Party: </span><asp:Label ID="lblReportParty" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Responsible Party: </span><asp:Label ID="lblRespParty" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">On-Scene Contact: </span><asp:Label ID="lblOnsceneContact" runat="server" Text=""></asp:Label>
    </div>
<h2>Hazardous Materials</h2>
    <div class="field">
        <span class="repLable">Sub-Type: </span> <asp:Label ID="lblSubType" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Situation: </span><asp:Label ID="lblSituation" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Chemical Name: </span><asp:Label ID="lblChemicalName" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Index Name: </span><asp:Label ID="lblIndexName" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">CAS Number: </span><asp:Label ID="lblCASNum" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Section 304 Reportable Quantity: </span><asp:Label ID="lblSection304Qty" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">CERCLA Reportable Quantity: </span><asp:Label ID="lblCerclaQty" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Chemical State: </span><asp:Label ID="lblChemState" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Source / Container: </span><asp:Label ID="lblSourceContainer" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Quantity released: </span><asp:Label ID="lblQtyReleased" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Rate of release: </span><asp:Label ID="lblRateRelease" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Released: </span><asp:Label ID="lblReleased" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Cause of release: </span><asp:Label ID="lblCauseRelease" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Storm drains affected: </span><asp:Label ID="lblStormDrains" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Waterways affected: </span><asp:Label ID="lblWaterways" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Major roadways closed: </span><asp:Label ID="lblRoadways" runat="server" Text=""></asp:Label>
    </div>
<h1>Hazmat Investigation Report</h1>
    <div class="field">
        <span class="repLable">Investigation Details: </span> <asp:Label ID="lblHMNarrative" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Facility Name or Description: </span> <asp:Label ID="lblHMFacName" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Business Type: </span> <asp:Label ID="lblBusinessType" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Address: </span> <asp:Label ID="lblHMIncidentLocation" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Release Source: </span> <asp:Label ID="lblHMReleaseSource" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Sector: </span> <asp:Label ID="lblHMSector" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Chemical Released: </span> <asp:Label ID="lblHMChemicalName" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">CAS #: </span> <asp:Label ID="lblHMCAS" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Amount Released: </span> <asp:Label ID="lblHMAmtReleased" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Release Date: </span> <asp:Label ID="lblHMReleaseDate" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Evacuations: </span> <asp:Label ID="lblHMEvac" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Injuries: </span> <asp:Label ID="lblHMInjury" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">Fatalities: </span> <asp:Label ID="lblHMFatality" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">SERC #: </span> <asp:Label ID="lblSERC" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">RMP #: </span> <asp:Label ID="lblRMP" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">TRI #: </span> <asp:Label ID="lblTRI" runat="server" Text=""></asp:Label>
    </div>
    <div class="field">
        <span class="repLable">TIER II EPLAN #: </span> <asp:Label ID="lblTIER2EPLAN" runat="server" Text=""></asp:Label>
    </div>
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
                <asp:CheckBox ID="cbSEP" Text="SEP" CssClass="inline-cb" runat="server" />
            </div>
        </div>
    <div class="field">
        <span class="repLable">7 Day Followup Report Due Date: </span> <asp:Label ID="lblFollowUpReportDueDate" runat="server" Text=""></asp:Label>
    </div>
</form>
</body>

</html>
