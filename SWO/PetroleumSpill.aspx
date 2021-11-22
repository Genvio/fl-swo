<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="PetroleumSpill.aspx.vb" Inherits="PetroleumSpill" %>

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
                        Petroleum Spill
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
                <asp:DropDownList ID="ddlSubType" style="background-color:#c2ecde" Width="200px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="On Land" Text="On Land"></asp:ListItem>
                    <asp:ListItem Value="Inland Waterway" Text="Inland Waterway"></asp:ListItem>
                    <asp:ListItem Value="Intertidal Area / Beach" Text="Intertidal Area / Beach"></asp:ListItem>
                    <asp:ListItem Value="Florida Coastal Waters" Text="Florida Coastal Waters"></asp:ListItem>
                    <asp:ListItem Value="Offshore" Text="Offshore"></asp:ListItem>
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
                    <asp:ListItem Value="Active, release dissipated" Text="Active, release dissipated"></asp:ListItem>
                    <asp:ListItem Value="Past Report" Text="Past Report"></asp:ListItem>
                    <asp:ListItem Value="Potential release" Text="Potential release"></asp:ListItem>
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
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style87">
                <big>
                    <b>
                        Petroleum Type:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlPetroleumType"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="225px"  
                    runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Fuel" Text="Fuel"></asp:ListItem>
                    <asp:ListItem Value="Lubricating Oil" Text="Lubricating Oil"></asp:ListItem>
                    <asp:ListItem Value="Crude Oil" Text="Crude Oil"></asp:ListItem>
                    <asp:ListItem Value="Sheen" Text="Sheen"></asp:ListItem>
                    <asp:ListItem Value="Tar Balls/Tar Patties/" Text="Tar Balls/Tar Patties/"></asp:ListItem>
                    <asp:ListItem Value="Other" Text="Other"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Name or Description:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtPetroleumNameDescription" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Odor:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtPetroleumOdor" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Color:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtPetroleumColor" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
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
                <asp:DropDownList ID="ddlPetroleumSourceContainer"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="225px"  
                    runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Aboveground Tank" Text="Aboveground Tank"></asp:ListItem>
                    <asp:ListItem Value="Underground Tank" Text="Underground Tank"></asp:ListItem>
                    <asp:ListItem Value="Aboveground Pipeline" Text="Aboveground Pipeline"></asp:ListItem>
                    <asp:ListItem Value="Underground Pipeline" Text="Underground Pipeline"></asp:ListItem>
                    <asp:ListItem Value="Vehicle" Text="Vehicle"></asp:ListItem>
                    <asp:ListItem Value="Marine Vessel" Text="Marine Vessel"></asp:ListItem>
                    <asp:ListItem Value="Rail Car" Text="Rail Car"></asp:ListItem>
                    <asp:ListItem Value="Road Trailer" Text="Road Trailer"></asp:ListItem>
                    <asp:ListItem Value="Drum" Text="Drum"></asp:ListItem>
                    <asp:ListItem Value="Cylinder" Text="Cylinder"></asp:ListItem>
                    <asp:ListItem Value="Valve" Text="Valve"></asp:ListItem>
                    <asp:ListItem Value="Other Container" Text="Other Container"></asp:ListItem>
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
                <asp:TextBox ID="txtPetroleumQuantityReleased" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
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
                <asp:TextBox ID="txtPetroleumRateOfRelease" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
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
                <asp:DropDownList ID="ddlPetroleumlReleased"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="225px"  
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
                <asp:TextBox ID="txtPetroleumCauseOfRelease" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
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
                &nbsp;<big><b>ET</b></big>
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
                <big>
                    <b>
                        Were any storm drains affected?:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlStormDrainsAffected"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="225px"  
                    runat="server" >
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                    <asp:ListItem Value="Yes, contained to storm drain" Text="Yes, contained to storm drain"></asp:ListItem>
                    <asp:ListItem Value="Yes, contained to retention pond" Text="Yes, contained to retention pond"></asp:ListItem>
                    <asp:ListItem Value="Yes, drained to waterway(s) listed" Text="Yes, drained to waterway(s) listed"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="N/A" Text="N/A"></asp:ListItem>
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
    <asp:Panel ID="pnlShowWaterwaysAffectedText" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Name(s) of waterways:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtWaterwaysAffectedText" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
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
                <asp:DropDownList ID="ddlMajorRoadwaysClosed"  style="background-color:#c2ecde" Width="225px"  runat="server">
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
                        Have any cleanup actions been taken?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlCleanupActionsTaken"  
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
    <asp:Panel ID="pnlShowCleanupActionsTaken" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        List cleanup actions:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCleanupActionsTakenText" Width="500px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('If third party, include: <BR> Company Name, Contact Name, Phone Number. ', TITLEBGCOLOR , '#FF0000' ,TITLE, '')" 
                    onmouseout="UnTip()"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Who is conducting cleanup?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtConductingCleanup" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
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
    <asp:Panel ID="pnlShowCallbackDEPRequested" runat="server" Visible="false">
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
                <asp:DropDownList ID="ddlCallbackDEPRequestedValue"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="225px"  
                    runat="server">
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

