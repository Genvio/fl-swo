<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="HazmatRelease.aspx.vb" Inherits="HazmatRelease" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">

fieldset {
  overflow: hidden;
  width: 95%;
}

.field 
{
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

input:not([type=button]), textarea 
{
    background-color: #c2ecde;
    width: 456px;
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
    width:60%;
    margin: auto;
    padding: 10px;
}

.buttonBox
{
    width:35%;
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

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1 style="font-size:6;text-align:center;">Hazardous Materials Release</h1>
<asp:HiddenField ID="hidHMID" runat="server" /><asp:HiddenField ID="hidHReleaseID" runat="server" />
<fieldset>
    <div class="field">
        <asp:Label ID="lblFacilityName" runat="server" Text="Facility Name:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtFacilityName" runat="server"></asp:TextBox>
    </div>
    <div class="field">
        <asp:Label ID="lblAddress" runat="server" Text="Address:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
    </div>
    <div class="field">
        <asp:Label ID="lblBusinessType" runat="server" Text="Business Type:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtBusinessType" runat="server"></asp:TextBox>
    </div>
    <div class="field">
        <asp:Label ID="lblReleaseSource" runat="server" Text="Release Source:" CssClass="label"></asp:Label>
        <asp:DropDownList ID="ddlReleaseSource" runat="server">
            <asp:ListItem Value="" Text="Select an Option" Selected="True"></asp:ListItem>
            <asp:ListItem Value="Transportation" Text="Transportation"></asp:ListItem>
            <asp:ListItem Value="Fixed Facility" Text="Fixed Facility"></asp:ListItem>
            <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="field">
        <asp:Label ID="lblSector" runat="server" Text="Sector:" CssClass="label"></asp:Label>
        <asp:DropDownList ID="ddlSector" runat="server">
            <asp:ListItem Value="" Text="Select an Option" Selected="True"></asp:ListItem>
            <asp:ListItem Value="Private" Text="Private"></asp:ListItem>
            <asp:ListItem Value="Public" Text="Public"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="field">
        <asp:Label ID="lblStWarnPointCode" runat="server" Text="State Warning Point Code:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtStWarnPointCode" runat="server"></asp:TextBox>
    </div>
    <div class="field">
        <asp:Label ID="lblStWarnPointCodeDate" runat="server" Text="State Warning Point Date:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtStWarnPointCodeDate" runat="server" CssClass="dateText"></asp:TextBox>
    </div>
    <div class="field">
        <asp:Label ID="lblChemReleased" runat="server" Text="Chemical Released:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtChemReleased" runat="server"></asp:TextBox>
    </div>
    <div class="field">
        <asp:Label ID="lblCASNum" runat="server" Text="CAS #:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtCASNum" runat="server"></asp:TextBox>
    </div>
    <div class="field">
        <asp:Label ID="lblAmountReleased" runat="server" Text="Amount Released:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtAmountReleased" runat="server"></asp:TextBox>
    </div>
    <div class="field">
        <asp:Label ID="lblReleaseDate" runat="server" Text="Release Date:" CssClass="label"></asp:Label>
        <asp:TextBox runat="server" ID="txtReleaseDate" CssClass="dateText" onmouseover="Tip('Format: MM/DD/YYYY <br/> ie.) 09/21/2009 ')" onmouseout="UnTip()"></asp:TextBox>
        <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtReleaseDate');"><img alt="Calendar Icon"
        src="Images/Calendar1.jpg" border="0" width="20" height="15"/></a>
        <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtReleaseDate.value = ''"
        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
        <asp:TextBox ID="txtReleaseTime"  Width="35px" 
        style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
        onmouseover="Tip('Format: 24 hour time  hh:mm <BR> ie.) 16:21 ')" 
        onmouseout="UnTip()"></asp:TextBox>&nbsp;ET
    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ValidationExpression="^(?:[01][0-9]|2[0-3]):[0-5][0-9]$" ControlToValidate="txtReleaseTime" runat="server" ErrorMessage="Incorrect 24 hour time format (hh:mm)"></asp:RegularExpressionValidator>
    </div>
    <div class="field">
        <asp:Label ID="lblDescNarrative" runat="server" Text="Chemical Release Description Narrative:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtDescNarrative" TextMode="MultiLine" runat="server"></asp:TextBox>
    </div>
    <div class="field">
        <asp:Label ID="lblNumberEvacuated" runat="server" Text="Number Evacuated:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtNumberEvacuated" runat="server" CssClass="numText"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtNumberEvacuated" ValidationExpression="^\d+$" runat="server" ErrorMessage="* Must be a Number"></asp:RegularExpressionValidator>
    </div>
    <div class="field">
        <asp:Label ID="lblNumberInjured" runat="server" Text="Number Injured:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtNumberInjured" runat="server" CssClass="numText"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtNumberInjured" ValidationExpression="^\d+$" runat="server" ErrorMessage="* Must be a Number"></asp:RegularExpressionValidator>
    </div>
    <div class="field">
        <asp:Label ID="lblNumberFatality" runat="server" Text="Number of Fatalities Verified:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtNumberFatality" runat="server" CssClass="numText"></asp:TextBox>
        <asp:CheckBox ID="cbCauseDeathVerify" Text="Cause of Death Verified" CssClass="inline-cb" runat="server" />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txtNumberFatality" ValidationExpression="^\d+$" runat="server" ErrorMessage="* Must be a Number"></asp:RegularExpressionValidator>
    </div>
     <div class="field">
        <asp:Label ID="lblFatalityCause" runat="server" Text="Cause of Death:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtFatalityCause" runat="server"></asp:TextBox>
    </div>
   <div class="field">
        <asp:Label ID="lblSERCNum" runat="server" Text="SERC #:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtSERCNum" runat="server"></asp:TextBox>
    </div>
    <div class="field">
        <asp:Label ID="lblRMPNum" runat="server" Text="RMP #:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtRMPNum" runat="server"></asp:TextBox>
    </div>
    <div class="field">
        <asp:Label ID="lblTRINum" runat="server" Text="TRI #:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtTRINum" runat="server"></asp:TextBox>
    </div>
    <div class="field">
        <asp:Label ID="lblTierTwoNum" runat="server" Text="TIER II EPLAN #:" CssClass="label"></asp:Label>
        <asp:TextBox ID="txtTierTwoNum" runat="server"></asp:TextBox>
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
                <asp:CheckBox ID="cbSEP" Text="SEP" CssClass="inline-cb" runat="server" />
            </div>
        </div>
    </div>
    <div class="field">
        <asp:Label ID="lblSevenDayDue" runat="server" Text="7 Day Followup Report Due Date:" CssClass="label"></asp:Label>
        <asp:TextBox runat="server" ID="txtSevenDayDue" CssClass="dateText" onmouseover="Tip('Format: MM/DD/YYYY <br/> ie.) 09/21/2009 ')" onmouseout="UnTip()"></asp:TextBox>
        <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtSevenDayDue');"><img alt="Calendar Icon"
        src="Images/Calendar1.jpg" border="0" width="20" height="15"/></a>
        <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtSevenDayDue.value = ''"
        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
    </div>
    <div class="field">
        <asp:Label ID="lblSevenDayReport" runat="server" Text="7 Day Facility Followup Reports:" CssClass="label"></asp:Label>
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:HyperLink ID="lnkFile1" runat="server"></asp:HyperLink>
        <div id="divDeleteFile1" runat="server" visible="false" style="display:inline">
            <asp:LinkButton ID="lbDeleteFile1" CommandName="DeleteFile" runat="server" OnClick="LinkButton_Click" OnClientClick="javascript:return confirm('Are you sure you want to permanently delete this file?')">
                <img alt="Delete File"  title="Delete File" runat="server" src="Images/delete-icon.png" width="16" />
            </asp:LinkButton>
        </div>
        <asp:HiddenField ID="hiddFile1" runat="server" /><asp:HiddenField ID="hiddFile1CT" runat="server" /><asp:HiddenField ID="hiddFileID1" runat="server" />
    </div>
    <div class="field">
        <asp:Label ID="Label9" runat="server" Text="Supporting Documentation:" CssClass="label"></asp:Label>
        <asp:FileUpload ID="FileUpload2" runat="server" />
        <asp:HyperLink ID="lnkFile2" runat="server"></asp:HyperLink>
        <div id="divDeleteFile2" runat="server" visible="false" style="display:inline">
            <asp:LinkButton ID="lbDeleteFile2" CommandName="DeleteFile" runat="server" OnClick="LinkButton_Click" OnClientClick="javascript:return confirm('Are you sure you want to permanently delete this file?')">
                <img alt="Delete File"  title="Delete File" runat="server" src="Images/delete-icon.png" width="16" />
            </asp:LinkButton>
        </div>
        <asp:HiddenField ID="hiddFile2" runat="server" /><asp:HiddenField ID="hiddFile2CT" runat="server" /><asp:HiddenField ID="hiddFileID2" runat="server" />
    </div>
    <div class="field">
        <asp:Label ID="Label10" runat="server" Text="Additional Information:" CssClass="label"></asp:Label>
        <asp:FileUpload ID="FileUpload3" runat="server" />
        <asp:HyperLink ID="lnkFile3" runat="server"></asp:HyperLink>
        <div id="divDeleteFile3" runat="server" visible="false" style="display:inline">
            <asp:LinkButton ID="lbDeleteFile3" CommandName="DeleteFile" runat="server" OnClick="LinkButton_Click" OnClientClick="javascript:return confirm('Are you sure you want to permanently delete this file?')">
                <img id="Img1" alt="Delete File"  title="Delete File" runat="server" src="Images/delete-icon.png" width="16" />
            </asp:LinkButton>
        </div>
        <asp:HiddenField ID="hiddFile3" runat="server" /><asp:HiddenField ID="hiddFile3CT" runat="server" /><asp:HiddenField ID="hiddFileID3" runat="server" />
    </div>
    <div class="field">
        <asp:Label ID="Label11" runat="server" Text="SEP Information:" CssClass="label"></asp:Label>
        <asp:FileUpload ID="FileUpload4" runat="server" />
        <asp:HyperLink ID="lnkFile4" runat="server"></asp:HyperLink>
        <div id="divDeleteFile4" runat="server" visible="false" style="display:inline">
            <asp:LinkButton ID="lbDeleteFile4" CommandName="DeleteFile" runat="server" OnClick="LinkButton_Click" OnClientClick="javascript:return confirm('Are you sure you want to permanently delete this file?')">
                <img id="Img2" alt="Delete File"  title="Delete File" runat="server" src="Images/delete-icon.png" width="16" />
            </asp:LinkButton>
        </div>
        <asp:HiddenField ID="hiddFile4" runat="server" /><asp:HiddenField ID="hiddFile4CT" runat="server" /><asp:HiddenField ID="hiddFileID4" runat="server" />
    </div>
</fieldset>
    <div class="buttonBox">
        <input type="button" value="Save Release" id="btnSave" runat="server" onserverclick="btnSave_Command" />
        &nbsp;&nbsp;&nbsp;
        <input type="button" value="Cancel" id="btnCancel" runat="server" onserverclick="btnCancel_Command" />
        &nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="lnkReport" runat="server">View Report</asp:HyperLink>
    </div>
</asp:Content>

