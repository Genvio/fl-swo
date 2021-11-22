<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="DemINR.aspx.vb" Inherits="DemINR" %>

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
                        DEM Incidents
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
                <asp:DropDownList ID="ddlSubType" AutoPostBack="true" style="background-color:#c2ecde" Width="250px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="SLRC Alarm" Text="SLRC Alarm"></asp:ListItem>
                    <asp:ListItem Value="SEOC Alarm" Text="SEOC Alarm"></asp:ListItem>
                    <asp:ListItem Value="DEP Alarm" Text="DEP Alarm"></asp:ListItem>
                    <asp:ListItem Value="Medical Emergency" Text="Medical Emergency"></asp:ListItem>
                    <asp:ListItem Value="SEOC Activation" Text="SEOC Activation"></asp:ListItem>
                    <asp:ListItem Value="SMT Activation" Text="SMT Activation"></asp:ListItem>
                    <asp:ListItem Value="Reservist Activation" Text="Reservist Activation"></asp:ListItem>
                    <asp:ListItem Value="General Notification" Text="General Notification"></asp:ListItem>
                    <asp:ListItem Value="IT Disruption or Issue" Text="IT Disruption or Issue"></asp:ListItem>
                    <asp:ListItem Value="Communications Disruption or Issue" Text="Communications Disruption or Issue"></asp:ListItem>
                    <asp:ListItem Value="Planned Outage" Text="Planned Outage"></asp:ListItem>
                    <asp:ListItem Value="EAS/IPAWS Activation" Text="EAS/IPAWS Activation"></asp:ListItem>
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
                <asp:TextBox ID="txtWorkSheetDescription" Width="758px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
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
                <asp:DropDownList ID="ddlNotification" DataTextField="Description" DataValueField="IncidentTypeLevelID"  style="background-color:#c2ecde" Width="762px"  runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlShowSlrcSeoc" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    SLRC and SEOC Alarms
                </h1>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Alarm Type:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSlrcSeocAlarmType"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Fire" Text="Fire"></asp:ListItem>
                    <asp:ListItem Value="Security" Text="Security"></asp:ListItem>
                    <asp:ListItem Value="Utility" Text="Utility"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Zone number(s) and/or description(s):
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtSlrcSeocZoneNumber" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Alarm Status:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSlrcSeocAlarmStatus"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Active" Text="Active"></asp:ListItem>
                    <asp:ListItem Value="Reset" Text="Reset"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <asp:Panel ID="pnlShowDepWarehouse" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    DEP Warehouse Alarm or Notification
                </h1>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Label / Memo that appears after selection:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDepWarehouseMemo" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Alarm or Non-Alarm Notification:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlDepWarehouseNotification"  style="background-color:#c2ecde" Width="175px" AutoPostBack="true"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Alarm" Text="Alarm"></asp:ListItem>
                    <asp:ListItem Value="Non-Alarm Notification" Text="Non-Alarm Notification"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowAlarm" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    Alarm
                </h1>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Zone number(s) and/or description(s):
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDepWarehouseZoneNumber" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Alarm Status:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlDepWarehouseAlarmStatus"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Active" Text="Active"></asp:ListItem>
                    <asp:ListItem Value="Reset" Text="Reset"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <asp:Panel ID="pnlShowNonAlarm" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    Non-Alarm Notification
                </h1>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Employee name:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDepWarehouseEmployeeName" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Employee cell phone:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDepWarehouseEmployeeCellPhone" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Agency and Division:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDepWarehouseAgencyDivision" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Supervisor name:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDepWarehouseSupervisorName" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Has supervisor been called?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlDepWarehouseSupervisorCalled"  style="background-color:#c2ecde" Width="175px"  runat="server">
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
                    <b>
                        Access card number:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDepWarehouseAccessCardNumber" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    </asp:Panel>
    
    <asp:Panel ID="pnlShowMedicalEmergency" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    Medical Emergency
                </h1>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Building and Room Number:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtMeBuildingRoomNumber" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Has someone called 911?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlMe911Called"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="N/A" Text="N/A"></asp:ListItem>
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
                        Is the person breathing?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlMePersonBreathing"  style="background-color:#c2ecde" Width="175px"  runat="server">
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
                    <b>
                        What is the person's level of consiousness?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlMeConsiousness"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Alert & Oriented" Text="Alert & Oriented"></asp:ListItem>
                    <asp:ListItem Value="Disoriented" Text="Disoriented"></asp:ListItem>
                    <asp:ListItem Value="Unresponsive" Text="Unresponsive"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Describe the person's complaint or symptoms:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtMeComplaintSymptom" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    
    </asp:Panel>
    
    <asp:Panel ID="pnlShowSeocActivation" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    SEOC Activation
                </h1>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Activation level:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSeocActivationLevel"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Level 3" Text="Level 3"></asp:ListItem>
                    <asp:ListItem Value="Level 2" Text="Level 2"></asp:ListItem>
                    <asp:ListItem Value="Level 1" Text="Level 1"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Related Incident Numbers:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtSeocActivationRelatedIncidentNumbers" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        EM Constellation Database:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSeocActivationEmcDatabase"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Select Existing" Text="Select Existing"></asp:ListItem>
                    <asp:ListItem Value="Create New" Text="Create New"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        EMC Database Name:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtSeocActivationEmcDatabaseName" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <asp:Panel ID="pnlShowSMTActivation" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    SMT Activation
                </h1>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Select SMT:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSmtActivationSMT"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="PENDING" Text="PENDING"></asp:ListItem>
                    <asp:ListItem Value="Blue" Text="Blue"></asp:ListItem>
                    <asp:ListItem Value="Green" Text="Green"></asp:ListItem>
                    <asp:ListItem Value="Gold" Text="Gold"></asp:ListItem>
                    <asp:ListItem Value="Grey" Text="Grey"></asp:ListItem>
                    <asp:ListItem Value="Red" Text="Red"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Describe reason for activation:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtSmtActivationReason" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Location to Report:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtSmtActivationReportLocation" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Authorized By:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtSmtActivationAuthorizedBy" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <asp:Panel ID="pnlShowReservistActivation" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    Reservist Activation
                </h1>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowSMTddl" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Select SMT:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlReservistActivationSMT"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="PENDING" Text="PENDING"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Describe reason for activation:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtReservistActivationReason" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Location to Report:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtReservistActivationReportLocation" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Authorized By:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtReservistActivationAuthorizedBy" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <asp:Panel ID="pnlShowGeneralNotification" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    General Notification
                </h1>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Enter message:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtGeneralNotificationMessage" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Authorized By:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtGeneralNotificationAuthorizedBy" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <asp:Panel ID="pnlShowItDisruptionIssue" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    IT Disruption or Issue
                </h1>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Describe the problem; copy error text + link if available:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtItDisruptionDescription" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Name of program(s)/system(s), if applicable:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtItDisruptionprogramSystem" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big><b>Time the problem started:</b></big>
            </td>
            <td align="left" class="style88">
                <asp:TextBox ID="txtItDisruptionTime"  Width="15px" 
                    style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                    onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                    onmouseout="UnTip()"></asp:TextBox>
                <big><b>:</b></big>
                <asp:TextBox ID="txtItDisruptionTime2"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp; <b>ET</b>
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
                        List any troubleshooting steps taken:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtItDisruptionStepsTaken" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    
    
    
    <asp:Panel ID="pnlShowCommunicationsDisruptionIssue" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    Communications Disruption or Issue
                </h1>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Select communication system(s) or circuit(s):
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlCommDisruptionSystemCircuit"  style="background-color:#c2ecde" Width="175px" AutoPostBack="true"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Federal NAWAS" Text="Federal NAWAS"></asp:ListItem>
                    <asp:ListItem Value="State NAWAS" Text="State NAWAS"></asp:ListItem>
                    <asp:ListItem Value="EMnet Voice Manager" Text="EMnet Voice Manager"></asp:ListItem>
                    <asp:ListItem Value="NPP HRD" Text="NPP HRD"></asp:ListItem>
                    <asp:ListItem Value="EMnet Message Manager" Text="EMnet Message Manager"></asp:ListItem>
                    <asp:ListItem Value="EMnet EAS Module" Text="EMnet EAS Module"></asp:ListItem>
                    <asp:ListItem Value="Commercial Telephone" Text="Commercial Telephone"></asp:ListItem>
                    <asp:ListItem Value="FAX Server" Text="FAX Server"></asp:ListItem>
                    <asp:ListItem Value="911 Telephone Outage" Text="911 Telephone Outage"></asp:ListItem>
                    <asp:ListItem Value="Other" Text="Other"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowCommunicationSystemOther" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        System:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCommDisruptionSystemCircuitText" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Describe the problem:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCommDisruptionDescription" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big><b>Time the problem started:</b></big>
            </td>
            <td align="left" class="style88">
                <asp:TextBox ID="txtCommDisruptionTime"  Width="15px" 
                    style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                    onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                    onmouseout="UnTip()"></asp:TextBox>
                <big><b>:</b></big>
                <asp:TextBox ID="txtCommDisruptionTime2"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp; <b>ET</b>
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
                        List any troubleshooting steps taken:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCommDisruptionStepsTaken" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
   
    
    <asp:Panel ID="pnlShowPlannedOutage" runat="server" Visible="false">
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    Planned Outage
                </h1>
            </td>
        </tr>
    </table>
     <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Describe the system(s) that will be impacted:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtPlannedOutageDescription" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
     <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Scheduled start date:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtPlannedOutageScheduledStartDate" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style85">
                <big><b>Scheduled start time:</b></big>
            </td>
            <td align="left" class="style88">
                <asp:TextBox ID="txtPlannedOutageScheduledStartTime"  Width="15px" 
                    style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                    onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                    onmouseout="UnTip()"></asp:TextBox>
                <big><b>:</b></big>
                <asp:TextBox ID="txtPlannedOutageScheduledStartTime2"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp; <b>ET</b>
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
                        List any troubleshooting steps taken:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtPlannedOutageEstimatedCompletion" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Point of contact name/number:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtPlannedOutagecontactNameNumber" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>

    <asp:Panel ID="pnlShowEAS_IPAWS" runat="server" Visible="false">
        <table align="center" width="100%">
            <tr>
                <td style="background-color: #d4d4d4" align="left">
                    <h1>
                        EAS/IPAWS Activation
                    </h1>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Requestor Name:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtEASRequestorName" Width="500px" style="background-color:#c2ecde" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Reason for Request:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtEASRequestReason" Width="500px" style="background-color:#c2ecde" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Requested Broadcast Date:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtEASBroadcastDate" Width="500px" style="background-color:#c2ecde" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style85">
                    <big><b>Requested Broadcast Time:</b></big>
                </td>
                <td align="left" class="style88">
                    <asp:TextBox ID="txtEASBroadcastTime"  Width="15px" MaxLength="2"
                        style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                        onmouseover="Tip('Enter the start time of the broadcast <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                        onmouseout="UnTip()"></asp:TextBox>
                    <big><b>:</b></big>
                    <asp:TextBox ID="txtEASBroadcastTime2"  Width="15px" MaxLength="2"
                        style="background-color:#c2ecde" runat="server"
                        onmouseover="Tip('Enter the start time of the broadcast <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                        onmouseout="UnTip()"></asp:TextBox>
                    &nbsp; <b>ET</b>
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
                            Broadcast Duration:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlEASBroadcastDuration"  style="background-color:#c2ecde" Width="175px"  runat="server">
                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="15 min" Text="15 min"></asp:ListItem>
                        <asp:ListItem Value="30 min" Text="30 min"></asp:ListItem>
                        <asp:ListItem Value="45 min" Text="45 min"></asp:ListItem>
                        <asp:ListItem Value="1 hr" Text="1 hr"></asp:ListItem>
                        <asp:ListItem Value="1.5 hrs" Text="1.5 hrs"></asp:ListItem>
                        <asp:ListItem Value="2 hrs" Text="2 hrs"></asp:ListItem>
                        <asp:ListItem Value="2.5 hrs" Text="2.5 hrs"></asp:ListItem>
                        <asp:ListItem Value="3 hrs" Text="3 hrs"></asp:ListItem>
                        <asp:ListItem Value="3.5 hrs" Text="3.5 hrs"></asp:ListItem>
                        <asp:ListItem Value="4 hrs" Text="4 hrs"></asp:ListItem>
                        <asp:ListItem Value="4.5 hrs" Text="4.5 hrs"></asp:ListItem>
                        <asp:ListItem Value="5 hrs" Text="5 hrs"></asp:ListItem>
                        <asp:ListItem Value="5.5 hrs" Text="5.5 hrs"></asp:ListItem>
                        <asp:ListItem Value="6 hrs" Text="6 hrs"></asp:ListItem>
                        <asp:ListItem Value="6.5 hrs" Text="6.5 hrs"></asp:ListItem>
                        <asp:ListItem Value="7 hrs" Text="7 hrs"></asp:ListItem>
                        <asp:ListItem Value="7.5 hrs" Text="7.5 hrs"></asp:ListItem>
                        <asp:ListItem Value="8 hrs" Text="8 hrs"></asp:ListItem>
                        <asp:ListItem Value="8.5 hrs" Text="8.5 hrs"></asp:ListItem>
                        <asp:ListItem Value="9 hrs" Text="9 hrs"></asp:ListItem>
                        <asp:ListItem Value="9.5 hrs" Text="9.5 hrs"></asp:ListItem>
                        <asp:ListItem Value="10 hrs" Text="10 hrs"></asp:ListItem>
                        <asp:ListItem Value="10.5 hrs" Text="10.5 hrs"></asp:ListItem>
                        <asp:ListItem Value="11 hrs" Text="11 hrs"></asp:ListItem>
                        <asp:ListItem Value="11.5 hrs" Text="11.5 hrs"></asp:ListItem>
                        <asp:ListItem Value="12 hrs" Text="12 hrs"></asp:ListItem>
                        <asp:ListItem Value="24 hrs" Text="24 hrs"></asp:ListItem>
                     </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Message Description:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtEASBroadcastMessage" style="background-color: #c2ecde" Height="100px" Width="500px" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Recommended Actions:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtEASRecommendedActions" style="background-color: #c2ecde" Height="100px" Width="500px" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Location Requested:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlEASLocation"  style="background-color:#c2ecde" Width="175px"  runat="server" AutoPostBack="true">
                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="Countywide" Text="Countywide"></asp:ListItem>
                        <asp:ListItem Value="Regionally" Text="Regionally"></asp:ListItem>
                        <asp:ListItem Value="Statewide" Text="Statewide"></asp:ListItem>
                     </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table width="100%" align="center" id="tblEASLocationDescription" runat="server" visible="false">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Location Description:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtEASLocationDescription" Width="500px" style="background-color:#c2ecde" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Alert Transmitted By:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtEASTransmittedBy" Width="500px" style="background-color:#c2ecde" runat="server" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style85">
                    <big><b>Alert Transmitted Time:</b></big>
                </td>
                <td align="left" class="style88">
                    <asp:TextBox ID="txtEASTRansmissionTime"  Width="15px" MaxLength="2"
                        style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                        onmouseover="Tip('Enter the transmission time <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                        onmouseout="UnTip()"></asp:TextBox>
                    <big><b>:</b></big>
                    <asp:TextBox ID="txtEASTRansmissionTime2"  Width="15px" MaxLength="2"
                        style="background-color:#c2ecde" runat="server"
                        onmouseover="Tip('Enter the transmission time <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                        onmouseout="UnTip()"></asp:TextBox>
                    &nbsp; <b>ET</b>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td align="left" class="style84" colspan="2">
                    <span style="font-weight:bold; color:#FF0000;">***Attach EAS file on main incident form after alert has been sent through EMnet.***</span>
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

