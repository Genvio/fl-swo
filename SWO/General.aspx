<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="General.aspx.vb" Inherits="General" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style84 {
            width: 456px;
        }

        .style86 {
            width: 452px;
        }

        .style87 {
            width: 453px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <AJAX:UpdatePanel ID="AJAXUpdatePanel" runat="server">
        <ContentTemplate>
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        <b>
                            <font size="6">General Incident
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
                            <b>Sub-Types:
                            </b>
                        </big>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlSubType" AutoPostBack="true" Style="background-color: #c2ecde" Width="200px" runat="server">
                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="General Incident" Text="General Incident"></asp:ListItem>
                            <asp:ListItem Value="Local/County EOC Activation" Text="Local/County EOC Activation"></asp:ListItem>
                            <asp:ListItem Value="Fish or Wildlife Kill" Text="Fish or Wildlife Kill"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <big>
                            <b>This situation is:
                            </b>
                        </big>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlSituation" Style="background-color: #c2ecde" Width="200px" runat="server">
                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Active" Text="Active"></asp:ListItem>
                            <asp:ListItem Value="Past Report" Text="Past Report"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <big>
                            <b>Worksheet Name:
                            </b>
                        </big>
                    </td>
                    <td align="left" colspan="3">
                        <asp:TextBox ID="txtWorkSheetDescription" Width="716px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <big>
                            <b>Notification:
                            </b>
                        </big>
                    </td>
                    <td align="left" colspan="3">
                        <asp:DropDownList ID="ddlNotification" DataTextField="Description" DataValueField="IncidentTypeLevelID" Style="background-color: #c2ecde" Width="722px" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <table align="center" width="100%">
                <tr>
                    <td style="background-color: #d4d4d4" align="left">
                        <h1>Information
                        </h1>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlShowGeneralIncident" runat="server" Visible="false">
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big>
                                <b>Describe the incident:
                                </b>
                            </big>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtGeneralDescription" Width="500px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big>
                                <b>What specific hazards exist?
                                </b>
                            </big>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSpecificHazards" Width="500px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big>
                                <b>What remedial actions are planned or occuring?
                                </b>
                            </big>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtRemedialActionsPlannedOccuring" Width="500px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlShowLocalCountyEOCActivation" runat="server" Visible="false">
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big>
                                <b>Level of Activation (read only):
                                </b>
                            </big>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtActivationLevel" Width="500px" Style="background-color: #c2ecde" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big>
                                <b>Level of Activation:
                                </b>
                            </big>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlActivationLevel" Width="500px" Style="background-color: #c2ecde" runat="server" onchange="this.style.backgroundColor = this.children[this.selectedIndex].style.backgroundColor;">
                                <asp:ListItem Value="Select an Option" Text="Select an Option" style="background-color: #c2ecde" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Partial Activation (Level 2)" Text="Partial Activation (Level 2)" style="background-color: rgb(255, 255, 80)"></asp:ListItem>
                                <asp:ListItem Value="Full Activation (Level 1)" Text="Full Activation (Level 1)" style="background-color: rgb(249, 67, 67)"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big>
                                <b>Is This a County Activation:
                                </b>
                            </big>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlIsCountyActivation" Width="500px" Style="background-color: #c2ecde" runat="server">
                                <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
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
                                <b>Incident(s) or hazards(s) caused the activation:
                                </b>
                            </big>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCauseOfActivation" Width="500px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big>
                                <b>EOC Contact Number:
                                </b>
                            </big>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtEOCContactNumber" Width="500px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big>
                                <b>EOC Contact E-Mail:
                                </b>
                            </big>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtEOCContactEMail" Width="500px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big>
                                <b>Hours operation/operational periods & staffing:
                                </b>
                            </big>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtHoursOperationalPeriodsStaffing" Width="500px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlFishorWildLifeKill" runat="server" Visible="false">
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big>
                                <b>Number of Affected Fish / Wildlife:
                                </b>
                            </big>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtNumberofAffectedFishWildLife" Width="500px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big>
                                <b>What specific hazards exist?
                                </b>
                            </big>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSpecificHazardsWildLife" Width="500px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big>
                                <b>What remedial actions are planned or occuring?
                                </b>
                            </big>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtRemedialActionsPlannedOccuringWildlife" Width="500px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
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
                    <td align="center">&nbsp;&nbsp;&nbsp;
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

