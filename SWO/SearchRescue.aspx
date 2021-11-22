<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="SearchRescue.aspx.vb" Inherits="SearchRescue" %>

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
                        Search & Rescue
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
                    <asp:ListItem Value="ELT" Text="ELT"></asp:ListItem>
                    <asp:ListItem Value="EPIRB" Text="EPIRB"></asp:ListItem>
                    <asp:ListItem Value="PLB" Text="PLB"></asp:ListItem>
                    <asp:ListItem Value="Structure Collapse" Text="Structure Collapse"></asp:ListItem>
                    <asp:ListItem Value="Industrial Accident" Text="Industrial Accident"></asp:ListItem>
                    <asp:ListItem Value="Transportation Accident" Text="Transportation Accident"></asp:ListItem>
                    <asp:ListItem Value="LE Search (Missing Person)" Text="LE Search (Missing Person)"></asp:ListItem>
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
                <asp:DropDownList ID="ddlNotification" DataTextField="Description" DataValueField="IncidentTypeLevelID"  style="background-color:#c2ecde" Width="723px"  runat="server">
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
    <asp:Panel ID="pnlShowEltEpirbPlb" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Date mission opened:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox runat="server" style="background-color:#c2ecde" Columns="10" Width="80px" ID="txtSearchRescueDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;&nbsp;<a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtSearchRescueDate');"><img alt="Calendar Icon"
                src="Images/Calendar1.jpg" border="0" width="20" height="15"/></a>&nbsp;
                &nbsp;<img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtSearchRescueDate.value = ''"
                onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
            </td>
        </tr>
     </table>
     <table align="center" width="100%">
        <tr>
            <td align="right" class="style84">
                <big><b>Time mission opened:</b></big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtSearchRescueTime"  Width="15px" 
                    style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                    onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                    onmouseout="UnTip()"></asp:TextBox>
                <big><b>:</b></big>
                <asp:TextBox ID="txtSearchRescueTime2"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;<big><b>ET</b></big>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b> 
                        Mission number:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtMissionNumber" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b> 
                        Last coordinates or area description:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCoordinateAreaDescription" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b> 
                        Registration information:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtRegistrationInformation" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center" id="tblCAPResponding" runat="server">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Is CAP responding?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlCAPResponding"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                    <asp:ListItem Value="Not Applicable" Text="Not Applicable"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center"  id="tblAircraftOverdue" runat="server">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Any missing or overdue aircraft in the area?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlMissingOverdueAircraft"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Not Applicable" Text="Not Applicable"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table align="center" width="100%">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Date mission closed:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox runat="server" style="background-color:#c2ecde" Columns="10" Width="80px" ID="txtMissionClosedDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;&nbsp;<a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtMissionClosedDate');"><img alt="Calendar Icon"
                src="Images/Calendar1.jpg" border="0" width="20" height="15"/></a>&nbsp;
                &nbsp;<img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtMissionClosedDate.value = ''"
                onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
            </td>
        </tr>
     </table>
     <table align="center" width="100%">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Time mission closed:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtMissionClosedTime"  Width="15px" 
                    style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                    onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                    onmouseout="UnTip()"></asp:TextBox>
                <big><b>:</b></big>
                <asp:TextBox ID="txtMissionClosedTime2"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;<big><b>ET</b></big>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b> 
                        Disposition:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDisposition" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="pnlStructCollapseIndusAccTransAccOther" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Describe the affected struture(s) or facilities(s):
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAffectedStrutureFacility" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Is there a collapse?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlIsCollapse"  style="background-color:#c2ecde" Width="175px"  runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center" id="tblCollapseCause" runat="server" visible="false">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        What caused the collapse (if known)?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCausedCollapse" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center" id="tblPeopleTrapped" runat="server">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Are people trapped?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlPeopletrapped"  style="background-color:#c2ecde" Width="175px"  runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center" id="tblNumberPeopleTrapped" runat="server" visible="false">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Number of people trapped:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtNumberPeopleTrapped" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center" style="display:none">
        <tr>
            <td align="right" class="style86">
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
            <td align="right" class="style86">
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
     <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Any unmet needs for the rescue operation?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlUnmetNeeds"  
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
    <asp:Panel ID="pnlShowUnmetNeeds" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Describe Needs:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtUnmetNeedsText" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Department/agency coordinating rescue efforts?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCoordinatingRescueEffort" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="pnlShowLESearch" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Description of the individual(s):
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDescriptionIndividual" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Area the individual(s) were last seen in:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtLastSeen" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Vehicle Description/other relevant information:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDescriptionVehicleRelevantInformation" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Agency handling the investigation:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAgencyHandlingInvestigation" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
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
    <br />
    <br />
    </ContentTemplate>
    </AJAX:UpdatePanel>
</asp:Content>

