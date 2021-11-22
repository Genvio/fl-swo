<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="KennedySpaceCenter.aspx.vb" Inherits="KennedySpaceCenter"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style84
        {
            width: 456px;
        }
        .style85
        {
            width: 457px;
        }
        .style86
        {
            width: 458px;
        }
        .style87
        {
            width: 453px;
        }
        .style88
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
                        Kennedy Space Center / Cape Canaveral AFS
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
                <asp:DropDownList ID="ddlSubType" AutoPostBack="true" style="background-color:#c2ecde" Width="200px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Initial Notification" Text="Initial Notification"></asp:ListItem>
                    <asp:ListItem Value="Rescheduled Launch" Text="Rescheduled Launch"></asp:ListItem>
                    <asp:ListItem Value="Scrubbed Launch" Text="Scrubbed Launch"></asp:ListItem>
                    <asp:ListItem Value="Successful Launch" Text="Successful Launch"></asp:ListItem>
                    <asp:ListItem Value="Unsuccessful Launch" Text="Unsuccessful Launch"></asp:ListItem>
                    <asp:ListItem Value="Other" Text="Other"></asp:ListItem>
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
                <asp:DropDownList ID="ddlSituation"  style="background-color:#c2ecde" Width="200px"  runat="server">
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
                <asp:TextBox ID="txtWorkSheetDescription" Width="716px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
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
                <asp:DropDownList ID="ddlNotification" DataTextField="Description" DataValueField="IncidentTypeLevelID"  style="background-color:#c2ecde" Width="722px"  runat="server">
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

    <asp:Panel ID="pnlLaunchLocation" runat="server" Visible="true">
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style87">
                    <big>
                        <b>
                            Launch Location:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlLaunchLocation"  
                        style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                        runat="server" AutoPostBack="true">
                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="Kennedy Space Center" Text="Kennedy Space Center"></asp:ListItem>
                        <asp:ListItem Value="Cape Canaveral Air Force Station" Text="Cape Canaveral Air Force Station"></asp:ListItem>
                        <asp:ListItem Value="Other" Text="Other"></asp:ListItem>
                     </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table width="100%" align="center" id="tblLaunchLocationText" runat="server" visible="false">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Description:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtLaunchLocationDescription" Width="500px" style="background-color:#c2ecde" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Mission Name:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtMissionName" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>

    <asp:Panel ID="pnlInitialNotificationRescheduledLaunch" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td align="right" class="style85">
                <big><b>Mission launch date:</b></big>
            </td>
            <td align="left">
                <asp:TextBox runat="server" style="background-color:#c2ecde" Columns="10" Width="80px" ID="txtInrlMissionLaunchDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;&nbsp;<a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtInrlMissionLaunchDate');"><img alt="Calendar Icon"
                src="Images/Calendar1.jpg" border="0" width="20" height="15"/></a>&nbsp;
                &nbsp;<img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtInrlMissionLaunchDate.value = ''"
                onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style85">
                <big><b>Launch Window Start:</b></big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtInrlLaunchWindow"  Width="15px" 
                    style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                    onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                    onmouseout="UnTip()"></asp:TextBox>
                <big><b>:</b></big>
                <asp:TextBox ID="txtInrlLaunchWindowB"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;<big><b>ET</b></big>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big><b>Launch Window End:</b></big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtInrlLaunchWindow2"  Width="15px" 
                    style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                    onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                    onmouseout="UnTip()"></asp:TextBox>
                <big><b>:</b></big>
                <asp:TextBox ID="txtInrlLaunchWindow2B"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;<big><b>ET</b></big>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Brevard Co. Fire Rescue Staff report to KSC Morrell Operations Center:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtInrlBrevardCo" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Brevard Co. EOC Activation to Level 2 no later than:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtInrlBrevardCo2" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table align="center" width="100%">
        <tr>
            <td align="right" class="style85">
                <big><b>Next launch notification date:</b></big>
            </td>
            <td align="left">
                <asp:TextBox runat="server" style="background-color:#c2ecde" Columns="10" Width="80px" ID="txtNextMissionLaunchDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;&nbsp;<a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtNextMissionLaunchDate');"><img alt="Calendar Icon"
                src="Images/Calendar1.jpg" border="0" width="20" height="15"/></a>&nbsp;
                &nbsp;<img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtNextMissionLaunchDate.value = ''"
                onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <asp:Panel ID="pnlScrubbedLaunch" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td align="right" class="style85">
                <big><b>Mission scrubbed date:</b></big>
            </td>
            <td align="left">
                <asp:TextBox runat="server" style="background-color:#c2ecde" Columns="10" Width="80px" ID="txtScrubDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;&nbsp;<a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtScrubDate');"><img alt="Calendar Icon"
                src="Images/Calendar1.jpg" border="0" width="20" height="15"/></a>&nbsp;
                &nbsp;<img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtScrubDate.value = ''"
                onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style85">
                <big><b>Mission scrubbed time:</b></big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtScrubTime"  Width="15px" 
                    style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                    onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                    onmouseout="UnTip()"></asp:TextBox>
                <big><b>:</b></big>
                <asp:TextBox ID="txtScrubTime2"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;<big><b>ET</b></big>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Reason:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtScrubReason" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Next launch notification date/time:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtScrubNextLaunchDateTime" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <asp:Panel ID="pnlSuccessfulLaunch" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td align="right" class="style85">
                <big><b>Launch date:</b></big>
            </td>
            <td align="left">
                <asp:TextBox runat="server" style="background-color:#c2ecde" Columns="10" Width="80px" ID="txtSuccessDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;&nbsp;<a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtSuccessDate');"><img alt="Calendar Icon"
                src="Images/Calendar1.jpg" border="0" width="20" height="15"/></a>&nbsp;
                &nbsp;<img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtSuccessDate.value = ''"
                onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style85">
                <big><b>Launch time:</b></big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtSuccessTime"  Width="15px" 
                    style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                    onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                    onmouseout="UnTip()"></asp:TextBox>
                <big><b>:</b></big>
                <asp:TextBox ID="txtSuccessTime2"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;<big><b>ET</b></big>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <asp:Panel ID="pnlUnsuccessfulLaunch" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td align="right" class="style85">
                <big><b>Launch date:</b></big>
            </td>
            <td align="left">
                <asp:TextBox runat="server" style="background-color:#c2ecde" Columns="10" Width="80px" ID="txtUnsuccessDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;&nbsp;<a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtUnsuccessDate');"><img alt="Calendar Icon"
                src="Images/Calendar1.jpg" border="0" width="20" height="15"/></a>&nbsp;
                &nbsp;<img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtUnsuccessDate.value = ''"
                onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style85">
                <big><b>Launch time:</b></big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtUnsuccessTime"  Width="15px" 
                    style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                    onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                    onmouseout="UnTip()"></asp:TextBox>
                <big><b>:</b></big>
                <asp:TextBox ID="txtUnsuccessTime2"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;<big><b>ET</b></big>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Reason, if known:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtUnsuccessReason" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style87">
                <big>
                    <b>
                        Is there any off-site impact?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlUnsuccessOffSiteImpact"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowUnsuccessOffSiteImpactText" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                         Describe area and hazards: 
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtUnsuccessOffSiteImpactText" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <table width="100%" align="center" style="display:none">
        <tr>
            <td align="right" class="style87">
                <big>
                    <b>
                        Are there Injuries?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlInjury"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowInjuryText" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Number and Severity of Injuries:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtInjury" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <table width="100%" align="center" style="display:none">
        <tr>
            <td align="right" class="style88">
                <big>
                    <b>
                        Are there Fatalities?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlFatality"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowFatalityText" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Number and location:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFatalityText" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    </asp:Panel>
    
    <asp:Panel ID="pnlShowOther" runat="server" Visible="false">
    
    </asp:Panel>
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
    </ContentTemplate>
    </AJAX:UpdatePanel>
</asp:Content>

