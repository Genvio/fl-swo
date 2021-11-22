<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="AircraftIncident.aspx.vb" Inherits="AircraftIncident" %>

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
            width: 452px;
        }
        .style87
        {
            width: 455px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <AJAX:UpdatePanel ID="AJAXUpdatePanel" runat="server">
    <ContentTemplate>
    <asp:Literal ID="lblAjaxHelper" runat="server"></asp:Literal>
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <b>
                    <font size="6">
                        Aircraft Incident
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
                <asp:DropDownList ID="ddlSubType"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Alert 1" Text="Alert 1"></asp:ListItem>
                    <asp:ListItem Value="Alert 2" Text="Alert 2"></asp:ListItem>
                    <asp:ListItem Value="Alert 3" Text="Alert 3"></asp:ListItem>
                    <asp:ListItem Value="Aircraft Accident" Text="Aircraft Accident"></asp:ListItem>
                    <asp:ListItem Value="Aircraft Hijacking" Text="Aircraft Hijacking"></asp:ListItem>
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
                <asp:DropDownList ID="ddlSituation"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="In Progress" Text="In Progress"></asp:ListItem>
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
                <asp:TextBox ID="txtWorkSheetDescription" Width="697px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
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
                <asp:DropDownList ID="ddlNotification" DataTextField="Description" DataValueField="IncidentTypeLevelID"  style="background-color:#c2ecde" Width="702px"  runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <br />
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    Aircraft Information
                </h1>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style87">
                <big>
                    <b>
                        Select Aircraft Type:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlAircraftType"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Aeromedical" Text="Aeromedical"></asp:ListItem>
                    <asp:ListItem Value="Commercial Cargo" Text="Commercial Cargo"></asp:ListItem>
                    <asp:ListItem Value="Commercial Passenger" Text="Commercial Passenger"></asp:ListItem>
                    <asp:ListItem Value="Military" Text="Military"></asp:ListItem>
                    <asp:ListItem Value="Private" Text="Private"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Aircraft Make & Model:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtMakeModel" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style85">
                <big>
                    <b>
                        Tail Number:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtTailNumber" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style85">
                <big>
                    <b>
                        Owned/Operated By:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtOwnedOperatedBy" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style85">
                <big>
                    <b>
                        Cause of incident (if known)?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCauseOfIncident" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style85">
                <big>
                    <b>
                        Number of People Onboard (passengers/crew):
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtNumberPeopleOnboard" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style85">
                <big>
                    <b>
                        Is there a fire?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlFire"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center" style="display:none">
        <tr>
            <td align="right" class="style85">
                <big>
                    <b>
                        Are there Injuries?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlInjury"  style="background-color:#c2ecde" Width="175px"  runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowInjuryTextBox" runat="server" Visible="false">
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
                <asp:TextBox ID="txtInjuryText" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <table width="100%" align="center" style="display:none">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Are there fatalities?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlFatality" AutoPostBack="true"  
                    style="background-color:#c2ecde; margin-left: 0px;" Width="175px"  
                    runat="server">
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
                        Number and location (aircraft or ground):
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFatalityText" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Are other structures or roadways involved?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlStructuresRoadwaysInvolved"  
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
    <asp:Panel ID="pnlShowStructuresRoadwaysInvolvedText" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Description:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtStructuresRoadwaysInvolvedText" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Hazardous materials onboard?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlHazMatOnboard"  style="background-color:#c2ecde" Width="175px"  runat="server">
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
            <td align="right" class="style85">
                <big>
                    <b>
                        Fuel or Petroleum Spills?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlFuelPetroleumSpills"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center" style="display:none">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Are there any evacuations?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlEvacuations"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel runat="server" Visible="false" ID="pnlShowExtraInfo">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        What departments/agencies are responding?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDepartmentAgencyResponding" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        What departments/agencies have been notified?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDepartmentAgencyNotified" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
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
    </ContentTemplate>
    </AJAX:UpdatePanel>
</asp:Content>

