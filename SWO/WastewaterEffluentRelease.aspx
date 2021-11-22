<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="WastewaterEffluentRelease.aspx.vb" Inherits="WastewaterEffluentRelease" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style84
        {
            width: 456px;
        }
        .style86
        {
            width: 452px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <AJAX:UpdatePanel ID="AJAXUpdatePanel" runat="server">
    <ContentTemplate>
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <b>
                    <font size="6">
                        Wastewater or Effluent
                    </font>
                </b>
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" align="center">
        <tr>
            <td align="right">
                <big>
                    <b>
                        Sub-Types:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSubType" AutoPostBack="true" style="background-color:#c2ecde" Width="225px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Wastewater" Text="Wastewater"></asp:ListItem>
                    <asp:ListItem Value="Treated Effluent" Text="Treated Effluent"></asp:ListItem>
                 </asp:DropDownList>
            </td>
            <td align="right">
                <big>
                    <b>
                        This situation is:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSituation"  style="background-color:#c2ecde" Width="225px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Active" Text="Active"></asp:ListItem>
                    <asp:ListItem Value="Past Report" Text="Past Report"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                <big>
                    <b>
                        Worksheet Name:
                    </b>
                </big>
            </td>
            <td align="left" colspan="3">
                <asp:TextBox ID="txtWorkSheetDescription" Width="740px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <big>
                    <b>
                       Notification:
                    </b>
                </big>
            </td>
            <td align="left" colspan="3">
                <asp:DropDownList ID="ddlNotification" DataTextField="Description" DataValueField="IncidentTypeLevelID"  style="background-color:#c2ecde" Width="900px"  runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <br />
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    Information
                </h1>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowWastewater" runat="server" Visible="false">
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Wastewater Facility Permit ID #:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtWWsystemIDPermitNumber" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Facility Name / Collection System Name:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtWWsystemName" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style86">
                    <big>
                        <b>
                            Type of System:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlWWsystemType"   
                        style="background-color:#c2ecde; margin-left: 4px;" Width="225px"  
                        runat="server" AutoPostBack="true">
                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="Industrial" Text="Industrial"></asp:ListItem>
                        <asp:ListItem Value="Municipal" Text="Municipal"></asp:ListItem>
                        <asp:ListItem Value="Other" Text="Other"></asp:ListItem>
                        <asp:ListItem Value="Private" Text="Private"></asp:ListItem>
                        <asp:ListItem Value="Private Collection System" Text="Private Collection System"></asp:ListItem>
                        <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                     </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table id="tblPrivateCollectionSystemName" width="100%" align="center" runat="server" visible="false">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Private Collection System Name:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtPrivateCollectionSystemName" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Type of wastewater:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtWWtype" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Release occurred from a:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlWWreleaseOccurred" style="background-color:#c2ecde;" Width="265px" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="Air Release Valve" Text="Air Release Valve"></asp:ListItem>
                        <asp:ListItem Value="Force Main" Text="Force Main"></asp:ListItem>
                        <asp:ListItem Value="Gravity Line" Text="Gravity Line"></asp:ListItem>
                        <asp:ListItem Value="Lift Station" Text="Lift Station"></asp:ListItem>
                        <asp:ListItem Value="Manhole" Text="Manhole"></asp:ListItem>
                        <asp:ListItem Value="Reclamation Facility" Text="Reclamation Facility"></asp:ListItem>
                        <asp:ListItem Value="Retention Pond" Text="Retention Pond"></asp:ListItem>
                        <asp:ListItem Value="Other (note in cause below)" Text="Other (note in occurrence details below)"></asp:ListItem>
                        <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                        <asp:ListItem Value="Wastewater Treatment Plant" Text="Wastewater Treatment Plant"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>

        <table id="tblReleaseOccurrenceDetails" runat="server" width="100%" align="center" visible="false">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Additional occurrence details:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtWWreleaseOccurredDetails" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>

        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            What caused the release?
                        </b>
                    </big>
                </td>
                <td align="left">
                    <%--<asp:TextBox ID="txtWWreleaseCause" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>--%>
                    <!--disabled property of Overflow option set by design, so do not remove it (bp 20160204)-->
                    <asp:DropDownList ID="ddlWWreleaseCause" style="background-color:#c2ecde;" Width="225px"
                        runat="server" AutoPostBack="true">
                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="Accident" Text="Accident"></asp:ListItem>
                        <asp:ListItem Value="Blockage" Text="Blockage"></asp:ListItem>
                        <asp:ListItem Value="Break-in-Line" Text="Break-in-Line"></asp:ListItem>
                        <asp:ListItem Value="Contractor" Text="Contractor"></asp:ListItem>
                        <asp:ListItem Value="Equipment" Text="Equipment"></asp:ListItem>
                        <asp:ListItem Value="FOG" Text="FOG"></asp:ListItem>
                        <asp:ListItem Value="Hauling" Text="Hauling"></asp:ListItem>
                        <asp:ListItem Value="Industrial" Text="Industrial"></asp:ListItem>
                        <asp:ListItem Value="Negligence" Text="Negligence"></asp:ListItem>
                        <asp:ListItem Value="On-site" Text="On-site"></asp:ListItem>
                        <asp:ListItem Value="Other" Text="Other (explain below)"></asp:ListItem>
                        <asp:ListItem Value="Overflow" Text="Overflow" disabled="disabled"></asp:ListItem>
                        <asp:ListItem Value="Power" Text="Power"></asp:ListItem>
                        <asp:ListItem Value="Rain" Text="Rain"></asp:ListItem>
                        <asp:ListItem Value="Repair" Text="Repair"></asp:ListItem>
                        <asp:ListItem Value="Security" Text="Security"></asp:ListItem>
                        <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                        <asp:ListItem Value="Vessel" Text="Vessel"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        
        <table id="tblReleaseCauseDetails" runat="server" width="100%" align="center" visible="false">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Additional cause of release details:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtWWreleaseCauseDetails" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>

         <table width="100%" align="center">
            <tr>
                <td align="right" class="style86">
                    <big>
                        <b>
                            Release status:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlWWreleaseStatus"   
                        style="background-color:#c2ecde; margin-left: 4px;" Width="225px"  
                        runat="server" AutoPostBack="true">
                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="Ongoing" Text="Ongoing"></asp:ListItem>
                        <asp:ListItem Value="Ceased" Text="Ceased"></asp:ListItem>
                        <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                     </asp:DropDownList>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlShowCeasedTimeDate" runat="server" Visible="false">
            <table width="100%" align="center">
                <tr>
                    <td align="right" class="style84">
                        <big>
                            <b>
                                Date release ceased:
                            </b>
                        </big>
                    </td>
                    <td align="left">
                        <asp:TextBox runat="server" style="background-color:#c2ecde" Columns="10" Width="80px" ID="txtWWceasedDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                        <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtWWceasedDate');"><img alt="Calendar Icon"
                        src="Images/Calendar1.jpg" border="0" width="20" height="15"/></a>
                        <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtWWceasedDate.value = ''"
                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                    </td>
                </tr>
            </table>
            <table width="100%" align="center">
                <tr>
                    <td align="right" class="style84">
                        <big><b>Time release ceased:</b></big>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtWWceasedTime"  Width="15px" 
                            style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                            onmouseout="UnTip()"></asp:TextBox>
                        <big><b>:</b></big>
                        <asp:TextBox ID="txtWWceasedTime2"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                        &nbsp;<big><b>ET</b></big>
                    </td>
                </tr>
            </table>
        </asp:Panel> <%--End pnlShowCeasedTimeDate--%>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style86">
                    <big>
                        <b>
                            Was the release contained on-site at a water reclamation facility?
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlWWreleasedContainedonSite"   
                        style="background-color:#c2ecde; margin-left: 4px;" Width="225px"  
                        runat="server">
                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                        <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                        <asp:ListItem Value="No" Text="No"></asp:ListItem>
                     </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Amount of release, in gallons:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtWWreleaseAmount" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style86">
                    <big>
                        <b>
                            Did the release enter a storm water system?
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlWWstormWater"   
                        style="background-color:#c2ecde; margin-left: 4px;" Width="225px"  
                        runat="server" AutoPostBack="true">
                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                        <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                        <asp:ListItem Value="No" Text="No"></asp:ListItem>
                     </asp:DropDownList>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlShowStormWaterSystem" runat="server" Visible="false">
            <table width="100%" align="center">
                <tr>
                    <td align="right" class="style84">
                        <big>
                            <b>
                                Location of storm drain(s) that were impacted:
                            </b>
                        </big>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtWWstormWaterLocation" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table width="100%" align="center">
                <tr>
                    <td align="right" class="style84">
                        <big>
                            <b>
                                Where does the storm drain discharge?
                            </b>
                        </big>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtWWstormWaterDischarge" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel> <%--End pnlShowStormWaterSystem--%>
        
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style86">
                    <big>
                        <b>
                            Did the release enter any surface waters?
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlWWsurfaceWater"  
                        style="background-color:#c2ecde; margin-left: 4px;" Width="225px"  
                        runat="server" AutoPostBack="true">
                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                        <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                        <asp:ListItem Value="No" Text="No"></asp:ListItem>
                     </asp:DropDownList>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlShowRetentionPond" runat="server" Visible="false">
            <table width="100%" align="center">
                <tr>
                    <td align="right" class="style86">
                        <big>
                            <b>
                                Type of surface water:
                            </b>
                        </big>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlWWsurfaceWaterDDL" AutoPostBack="true"   
                            style="background-color:#c2ecde; margin-left: 4px;" Width="225px"  
                            runat="server">
                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Retention Pond, contained." Text="Retention Pond, contained."></asp:ListItem>
                            <asp:ListItem Value="Retention pond, drained to waterway." Text="Retention pond, drained to waterway."></asp:ListItem>
                            <asp:ListItem Value="Waterway or Body of Water" Text="Waterway or Body of Water"></asp:ListItem>
                         </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </asp:Panel> <%--End pnlShowRetentionPond--%>
        <asp:Panel ID="pnlShowWaterway" runat="server" Visible="false">
            <table width="100%" align="center">
                <tr>
                    <td align="right" class="style84">
                        <big>
                            <b>
                                Names of waterway(s):
                            </b>
                        </big>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtWWwaterway" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel><%--End pnlShowWaterway--%>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style86">
                    <big>
                        <b>
                            Affected waterway a source of drinking water?
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlWWconfirmedContamination"   
                        style="background-color:#c2ecde; margin-left: 4px;" Width="225px"  
                        runat="server">
                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                        <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                        <asp:ListItem Value="No" Text="No"></asp:ListItem>
                     </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style86">
                    <big>
                        <b>
                            Status of Cleanup Actions:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlWWcleanupActions"   
                        style="background-color:#c2ecde; margin-left: 4px;" Width="225px"  
                        runat="server">
                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="None Reported" Text="None Reported"></asp:ListItem>
                        <asp:ListItem Value="None Planned" Text="None Planned"></asp:ListItem>
                        <asp:ListItem Value="Planned" Text="Planned"></asp:ListItem>
                        <asp:ListItem Value="In-Progress" Text="In-Progress"></asp:ListItem>
                        <asp:ListItem Value="Complete" Text="Complete"></asp:ListItem>
                     </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Describe clean-up actions:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtWWcleanupActionsText" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel> <%--End of pnlShowWastewater--%>
    
    
    <asp:Panel ID="pnlShowTreatedEffluent" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Public Water System ID or Permit Number:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtTEsystemIDPermitNumber" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Name of System:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtTEsystemName" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        What caused the release?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtTEreleaseCause" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Amount of release, in gallons:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtTEgallonsReleased" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Are any cleanup actions needed?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlTEcleanupActions"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="225px"  
                    runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowTEcleanupActions" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Describe cleanup actions:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtTEcleanupActionsText" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel> <%--End of pnlShowTEcleanupActions--%>
    </asp:Panel> <%--End of pnlShowTreatedEffluent--%>
    <asp:Panel ID="pnlMessage" runat="server" Visible="false">
    <table width="100%">
        <tr>
            <td align="left" colspan="2">
                <div class="feature">
                    <table width="100%">
                        <tr>
                            <td valign="top" align="center">
                                <table width="100%">
                                    <tr align="left">
                                        <td align="left">
                                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <br />
    <br />
    <table width="100%" align="center">
        <tr>
            <td align="center">
                &nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;
                <input type="button" value="Save Incident" id="btnSave" runat="server" onserverclick="btnSave_Command" />
                &nbsp;&nbsp;&nbsp;
                <input type="button" value="Cancel" id="btnCancel" runat="server" onserverclick="btnCancel_Command" />
                &nbsp;&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    <br />
    <br />
    </ContentTemplate>
    </AJAX:UpdatePanel>
</asp:Content>

