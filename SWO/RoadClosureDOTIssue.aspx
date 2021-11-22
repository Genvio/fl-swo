<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="RoadClosureDOTIssue.aspx.vb" Inherits="RoadClosureDOTIssue" %>

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
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <b>
                    <font size="6">
                        Road Closure or DOT Issue
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
                <asp:DropDownList ID="ddlSubType"  style="background-color:#c2ecde" Width="290px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Major Local Road Closure" Text="Major Local Road Closure"></asp:ListItem>
                    <asp:ListItem Value="DOT Road Closure (State/US/Interstate Hwy.)" Text="DOT Road Closure (State/US/Interstate Hwy.)"></asp:ListItem>
                    <asp:ListItem Value="Bridge Problem" Text="Bridge Problem"></asp:ListItem>
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
                <asp:DropDownList ID="ddlSituation"  style="background-color:#c2ecde" Width="275px"  runat="server">
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
                <asp:DropDownList ID="ddlNotification" DataTextField="Description" DataValueField="IncidentTypeLevelID"  style="background-color:#c2ecde" Width="754px"  runat="server">
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
            <td align="right" class="style84">
                <big>
                    <b>
                        Roadway Name and/or number:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtRoadwayNameNumber" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style87">
                <big>
                    <b>
                        At:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlAt"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Incident Address" Text="Incident Address"></asp:ListItem>
                    <asp:ListItem Value="Mile Marker" Text="Mile Marker"></asp:ListItem>
                    <asp:ListItem Value="Exit Ramp" Text="Exit Ramp"></asp:ListItem>
                    <asp:ListItem Value="Intersection" Text="Intersection"></asp:ListItem>
                    <asp:ListItem Value="Between Cross Streets" Text="Between Cross Streets"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Mile Marker:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtMileMarker" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Exit Ramp:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtExitRamp" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Cross Street 1 or Intersection:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCrossStreet1Intersection" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Cross Street 2:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCrossStreet2" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Duration of closure (if known):
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDurationOfClosure" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        What department/agency directed the closure:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDepartmentAgencyDirectedClosure" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
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

