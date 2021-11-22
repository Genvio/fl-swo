<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="HazardousMaterials.aspx.vb" Inherits="HazardousMaterials" %>

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
            width: 453px;
        }
        .style88
        {
            width: 522px;
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
                        Hazardous Materials
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
                    <asp:ListItem Value="Biological Hazard" Text="Biological Hazard"></asp:ListItem>
                    <asp:ListItem Value="Chemical Agent" Text="Chemical Agent"></asp:ListItem>
                    <asp:ListItem Value="Radiological Material" Text="Radiological Material"></asp:ListItem>
                    <asp:ListItem Value="Toxic Industrial Chemical" Text="Toxic Industrial Chemical"></asp:ListItem>
                    <asp:ListItem Value="Unknown Hazard" Text="Unknown Hazard"></asp:ListItem>
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
                    <asp:ListItem Value="Active, release in progress" Text="Active, release in progress"></asp:ListItem>
                    <asp:ListItem Value="Active, release contained" Text="Active, release contained"></asp:ListItem>
                    <asp:ListItem Value="Potential release" Text="Potential release"></asp:ListItem>
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
                <asp:DropDownList ID="ddlNotification" DataTextField="Description" DataValueField="IncidentTypeLevelID"  style="background-color:#c2ecde" Width="800px"  runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlUnknownHazard" runat="server" Visible="false">
    <br />
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    Unknown Hazard
                </h1>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Chemical State:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlUHChemicalState"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Gas" Text="Gas"></asp:ListItem>
                    <asp:ListItem Value="Liquid" Text="Liquid"></asp:ListItem>
                    <asp:ListItem Value="Powder" Text="Powder"></asp:ListItem>
                    <asp:ListItem Value="Solid" Text="Solid"></asp:ListItem>
                    <asp:ListItem Value="Semi-Solid" Text="Semi-Solid"></asp:ListItem>
                    <asp:ListItem Value="Aerosol" Text="Aerosol"></asp:ListItem>
                    <asp:ListItem Value="Other" Text="Other"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>

    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Source / Container:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlUHSourceContainer"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Aboveground Tank" Text="Aboveground Tank"></asp:ListItem>
                    <asp:ListItem Value="Underground Tank" Text="Underground Tank"></asp:ListItem>
                    <asp:ListItem Value="Aboveground Pipeline" Text="Aboveground Pipeline"></asp:ListItem>
                    <asp:ListItem Value="Underground Pipeline" Text="Underground Pipeline"></asp:ListItem>
                    <asp:ListItem Value="Rail Car" Text="Rail Car"></asp:ListItem>
                    <asp:ListItem Value="Road Trailer" Text="Road Trailer"></asp:ListItem>
                    <asp:ListItem Value="Drum" Text="Drum"></asp:ListItem>
                    <asp:ListItem Value="Cylinder" Text="Cylinder"></asp:ListItem>
                    <asp:ListItem Value="Valve" Text="Valve"></asp:ListItem>
                    <asp:ListItem Value="Envelope or Package" Text="Envelope or Package"></asp:ListItem>
                    <asp:ListItem Value="Other Container" Text="Other Container"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Total source/container volume:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtUHTotalSourceContainerVolume" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Quantity released:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtUHChemicalQuantityReleased" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Rate of release:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtUHChemicalRateOfRelease" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Released:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlUHChemicalReleased"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Inside facility" Text="Inside facility"></asp:ListItem>
                    <asp:ListItem Value="Outside environment" Text="Outside environment"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="pnlShowBiologicalHazard" runat="server" Visible="false">
    <br />
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    Biological Hazard
                </h1>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Common Name:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCommonName" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Scientific Name:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtScientificName" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Quantity Description:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtQuantityDescription" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Container or device description:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtContainerDeviceDescription" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Total quantity:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtBiologicalTotalQuantity" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Quantity released:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtBiologicalQuantityReleased" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="pnlShowChemicalAgent" runat="server" Visible="false">
    <br />
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    Chemical Agent
                </h1>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style87">
                <big>
                    <b>
                        Type of Agent?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlAgentType"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Lachrymatory (Tearing)" Text="Lachrymatory (Tearing)"></asp:ListItem>
                    <asp:ListItem Value="Sternator (Vomiting)" Text="Sternator (Vomiting)"></asp:ListItem>
                    <asp:ListItem Value="Incapacitating" Text="Incapacitating"></asp:ListItem>
                    <asp:ListItem Value="Blister" Text="Blister"></asp:ListItem>
                    <asp:ListItem Value="Nerve" Text="Nerve"></asp:ListItem>
                    <asp:ListItem Value="Blood" Text="Blood"></asp:ListItem>
                    <asp:ListItem Value="Choking" Text="Choking"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Agent name, if known:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAgentName" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
     <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Container or device description:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAgentContainerDeviceDescription" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
     <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Total quantity:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAgentTotalQuantity" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
     <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Quantity released:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAgentQuantityReleased" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="pnlShowRadiologicalMaterial" runat="server" Visible="false">
    <br />
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    Radiological Material
                </h1>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style87">
                <big>
                    <b>
                        Radiation Type:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlRadiationType"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Alpha" Text="Alpha"></asp:ListItem>
                    <asp:ListItem Value="Beta" Text="Beta"></asp:ListItem>
                    <asp:ListItem Value="Gamma" Text="Gamma"></asp:ListItem>
                    <asp:ListItem Value="Neutron" Text="Neutron"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Isotope name, if known:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtIsotopeName" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Container or device description:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtContainerDeviceInstrumentDescription" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Total quantity:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtRadiationTotalQuantity" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Is any local or regional assistance requested?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlDOHBureauNotified"  
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
    </asp:Panel>
    <asp:Panel ID="pnlShowToxicIndustrialChemical" runat="server" Visible="false">
    <br />
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    Toxic Industrial Chemical
                </h1>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Chemical Name:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtChemicalName" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Index Name:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtIndexName" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       CAS Number:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCASNumber" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Section 304 Reportable Quantity:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtSection304ReportableQuantity" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       CERCLA Reportable Quantity:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCERCLAReportableQuantity" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Chemical State:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlChemicalState"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Gas" Text="Gas"></asp:ListItem>
                    <asp:ListItem Value="Liquid" Text="Liquid"></asp:ListItem>
                    <asp:ListItem Value="Powder" Text="Powder"></asp:ListItem>
                    <asp:ListItem Value="Solid" Text="Solid"></asp:ListItem>
                    <asp:ListItem Value="Semi-Solid" Text="Semi-Solid"></asp:ListItem>
                    <asp:ListItem Value="Aerosol" Text="Aerosol"></asp:ListItem>
                    <asp:ListItem Value="Other" Text="Other"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Source / Container:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSourceContainer"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Aboveground Tank" Text="Aboveground Tank"></asp:ListItem>
                    <asp:ListItem Value="Underground Tank" Text="Underground Tank"></asp:ListItem>
                    <asp:ListItem Value="Aboveground Pipeline" Text="Aboveground Pipeline"></asp:ListItem>
                    <asp:ListItem Value="Underground Pipeline" Text="Underground Pipeline"></asp:ListItem>
                    <asp:ListItem Value="Rail Car" Text="Rail Car"></asp:ListItem>
                    <asp:ListItem Value="Road Trailer" Text="Road Trailer"></asp:ListItem>
                    <asp:ListItem Value="Drum" Text="Drum"></asp:ListItem>
                    <asp:ListItem Value="Cylinder" Text="Cylinder"></asp:ListItem>
                    <asp:ListItem Value="Valve" Text="Valve"></asp:ListItem>
                    <asp:ListItem Value="Other Container" Text="Other Container"></asp:ListItem>
                    <asp:ListItem Value="Envelope or Package" Text="Envelope or Package"></asp:ListItem>
                    <asp:ListItem Value="Vehicle" Text="Vehicle"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowPipeline" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Diameter of the Pipeline:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDiameterPipeline" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Unbroken end of the pipe connected to:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtUnbrokenEndPipeConnectedTo" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Total source/container volume:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtTotalSourceContainerVolume" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Quantity released:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtChemicalQuantityReleased" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Rate of release:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtChemicalRateOfRelease" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Released:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlChemicalReleased"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Inside facility" Text="Inside facility"></asp:ListItem>
                    <asp:ListItem Value="Outside environment" Text="Outside environment"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Cause of release:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCauseOfRelease" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right">
                <big><b>Time the release was discovered:</b></big>
            </td>
            <td align="left" class="style88">
                <asp:TextBox ID="txtTimeReleaseDiscovered"  Width="15px" 
                    style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                    onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                    onmouseout="UnTip()"></asp:TextBox>
                <big><b>:</b></big>
                <asp:TextBox ID="txtTimeReleaseDiscovered2"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;<b>ET</b>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right">
                <big><b>Time the release was secured:</b></big>
            </td>
            <td align="left" class="style88">
                <asp:TextBox ID="txtTimeReleaseSecured"  Width="15px" 
                    style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                    onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                    onmouseout="UnTip()"></asp:TextBox>
                <big><b>:</b></big>
                <asp:TextBox ID="txtTimeReleaseSecured2"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;<b>ET</b>
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
                       Reason for late report, if applicable:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtReasonLateReport" Width="500px"
                onmouseover="Tip('Required only if time reported to SWO is  greater than 15 minutes after time discovered. ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                onmouseout="UnTip()"
                style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Were any storm drains affected?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlStormDrainsAffected"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="275px"  
                    runat="server" >
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes, contained to storm drain" Text="Yes, contained to storm drain"></asp:ListItem>
                    <asp:ListItem Value="Yes, contained to retention pond" Text="Yes, contained to retention pond"></asp:ListItem>
                    <asp:ListItem Value="Yes, drained to waterway(s) listed" Text="Yes, drained to waterway(s) listed"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Were any waterways affected?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlWaterwaysAffected"  
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
    <asp:Panel ID="pnlShowWaterwaysAffectedText" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Name(s) of waterways
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtWaterwaysAffectedText" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <table width="100%" align="center" style="display:none">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Is a callback from DEP requested?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlCallbackDEPRequested"  
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
    <asp:Panel ID="pnlShowRegionalAssistanceRequested" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Select Contact:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlCallbackDEPRequestedDDLValue"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server" >
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Reporting Party" Text="Reporting Party"></asp:ListItem>
                    <asp:ListItem Value="Responsible Party" Text="Responsible Party"></asp:ListItem>
                    <asp:ListItem Value="On-Scene Contact" Text="On-Scene Contact"></asp:ListItem>
                    <asp:ListItem Value="Other (See Notes)" Text="Other (See Notes)"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    </asp:Panel>
    </asp:Panel>
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    Information
                </h1>
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
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Are any major roadways closed?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlMajorRoadwaysClosed"  style="background-color:#c2ecde" Width="175px"  runat="server">
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
                <asp:HyperLink ID="lnkRelease" runat="server">View/Edit Release</asp:HyperLink>
            </td>
        </tr>
    </table>
    </ContentTemplate>
    </AJAX:UpdatePanel>
</asp:Content>

