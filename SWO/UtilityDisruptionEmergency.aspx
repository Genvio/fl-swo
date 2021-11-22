<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="UtilityDisruptionEmergency.aspx.vb" Inherits="UtilityDisruptionEmergency" %>

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
        .style87
        {
            width: 450px;
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
                        Utility Disruption or Emergency
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
                <asp:DropDownList ID="ddlSubType" AutoPostBack="true" style="background-color:#c2ecde" Width="245px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Telecommunications Outage" Text="Telecommunications Outage"></asp:ListItem>
                    <asp:ListItem Value="Drinking Water Outage" Text="Drinking Water Outage"></asp:ListItem>
                    <asp:ListItem Value="Electric Outage" Text="Electric Outage"></asp:ListItem>
                    <asp:ListItem Value="Electric Generating Capacity Advisory" Text="Electric Generating Capacity Advisory"></asp:ListItem>
                    <asp:ListItem Value="Natural Gas Outage" Text="Natural Gas Outage"></asp:ListItem>
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
                    <asp:ListItem Value="N/A" Text="N/A"></asp:ListItem>
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
                <asp:TextBox ID="txtWorkSheetDescription" Width="750px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
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
                <asp:DropDownList ID="ddlNotification" DataTextField="Description" DataValueField="IncidentTypeLevelID"  style="background-color:#c2ecde" Width="800px"  runat="server">
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
    
    <asp:Panel ID="pnlShowTelecommunicationsOutage" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Communications System:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtTOcommunicationsSystem" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        System operated by:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtTOsystemOperated" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Number of Customers affected:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtTOcustomersAffectedNumber" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Is 911 telephone service affected?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlTO911Affected"  
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
    <asp:Panel ID="pnlShowTO911AffectedText" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Describe:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtTO911AffectedText" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Any damage to the facility or distibution system?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlTOdamageFacilityDistibutionSystem"  
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
    <asp:Panel ID="pnlShowTOdamageFacilityDistibutionSystemIntentional" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Was it intentional?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlTOdamageFacilityDistibutionSystemIntentional"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="pnlShowTOdamageFacilityDistibutionSystemText" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Description of the individual(s) responsible:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtTOdamageFacilityDistibutionSystemText" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    </asp:Panel>
    
    <asp:Panel ID="pnlShowDrinkingWaterOutage" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Water System Name:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDWOWaterSystemName" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Public water system ID #:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDWOpublicWaterSystemID" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Number of customers affected:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDWOnumberCustomersAffected" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Is the outage a result of any trespassing, theft, vandalism, or a security breach to the distribution system or its facilities?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlDWOoutageResultTTVSBDSF"  style="background-color:#c2ecde" Width="175px"  runat="server">
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
                        Estimated date/time of restoration:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDWOEstimatedDateTimeRestoration" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Was a boil water advisory issued?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlDWOboilAdvisory"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    
    
    <asp:Panel ID="pnlShowElectricOutage" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Electric System:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtEOelectricSystem" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        System operated by:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtEOsystemOperatedBy" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        What caused the outage?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtEOwhatCausedOutage" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Number of Customers affected:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtEOnumberCustomersAffected" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Estimated time to 98% or greater restoration:
                    </b>
                </big>
            </td>
            <td align="left">
<%--                <asp:DropDownList ID="ddlEOestimatedGreaterRestoration"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
--%>
                <asp:TextBox ID="txtEOestimatedGreaterRestoration" Width="500px" style="background-color:#c2ecde" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
    </table>
    
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Any damage to the facility or distibution system?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlEOdamageFacilityDistibutionSystem"  
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
    <asp:Panel ID="pnlShowEOdamageFacilityDistibutionSystemExtras" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Was it intentional?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlEOdamageFacilityDistibutionSystemIntentional"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
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
                        Description of the individual(s) responsible:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtEOdamageFacilityDistibutionSystemResposible" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    </asp:Panel>
    
    <asp:Panel ID="pnlShowElectricGeneratingCapacityAdvisory" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Advisory, Alert, Emergency:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlGCAadvisoryType"  style="background-color:#c2ecde" Width="175px"  runat="server">
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
                        Advisory due to a fuel supply shortage?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlGCAsupplyShortage"  style="background-color:#c2ecde" Width="175px"  runat="server">
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
                        Text of the Advisory:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtGCAadvisory" style="background-color: #c2ecde" Height="400px" Width="500px" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <asp:Panel ID="pnlShowNaturalGasOutage" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Natural Gas System:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtNGOsystem" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        System operated by:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtNGOsystemOperatedBy" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        What caused the outage?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtNGOoutageCause" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Number of Customers affected:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtNGOCustomersAffectedNumber" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Estimated time restoration:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtNGOestimatedTimeRestoration" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Any damage to the facility or distibution system?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlNGOdFDS"  
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
    <asp:Panel ID="pnlShowNGOdFDSextras" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style87">
                <big>
                    <b>
                        Was it intentional?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlNGOdFDSintentional"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server">
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
                        Description of the individual(s) responsible:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtNGOdFDSdescription" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
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

