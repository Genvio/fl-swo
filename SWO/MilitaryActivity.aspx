<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="MilitaryActivity.aspx.vb" Inherits="MilitaryActivity" %>

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
                        Military Activity
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
                    <asp:ListItem Value="Tomahawk Missile Launch" Text="Tomahawk Missile Launch"></asp:ListItem>
                    <asp:ListItem Value="US Military Activity" Text="US Military Activity"></asp:ListItem>
                    <asp:ListItem Value="FLNG Activity" Text="FLNG Activity"></asp:ListItem>
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
    <asp:Panel ID="pnlShowTomahawk" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Type of report:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlReportType"   
                    style="background-color:#c2ecde; margin-left: 4px;" Width="225px"  
                    runat="server" >
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Notification of impending launch." Text="Notification of impending launch."></asp:ListItem>
                    <asp:ListItem Value="Notification of launch." Text="Notification of launch."></asp:ListItem>
                    <asp:ListItem Value="Notification of successful flight." Text="Notification of successful flight."></asp:ListItem>
                    <asp:ListItem Value="Premature flight termination." Text="Premature flight termination."></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table align="center" width="100%">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                       Launch date:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox runat="server" style="background-color:#c2ecde" Columns="10" Width="80px" ID="txtLaunchDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;&nbsp;<a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtLaunchDate');"><img alt="Calendar Icon"
                src="Images/Calendar1.jpg" border="0" width="20" height="15"/></a>&nbsp;
                &nbsp;<img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtLaunchDate.value = ''"
                onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
            </td>
        </tr>
     </table>
     <table align="center" width="100%">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Launch time:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtLaunchTime"  Width="15px" 
                    style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                    onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                    onmouseout="UnTip()"></asp:TextBox>
                <big><b>:</b></big>
                <asp:TextBox ID="txtLaunchTime2"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;<big><b>ET</b></big>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Launch message:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtLaunchMessage" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style87">
                <big>
                    <b>
                        Flight path:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlFlightPath" style="background-color:#c2ecde; margin-left: 4px;" Width="225px" runat="server" >
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="IR-30" Text="IR-30"></asp:ListItem>
                    <asp:ListItem Value="IR-31" Text="IR-31"></asp:ListItem>
                    <asp:ListItem Value="IR-33" Text="IR-33"></asp:ListItem>
                    <asp:ListItem Value="Other" Text="Other"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <asp:Panel ID="pnlShowOther" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Unit conducting activity:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtUnitConductingActivity" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Describe the activity:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtActivityDescription" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Time/date range of activity:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtActivityTimeDateRange" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        List any airspace restrictions:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAirspaceRestrictions" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        List any road closures:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtRoadClosures" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Point of Contact Name:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtContactName" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Point of Contact Number:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtContactNumber" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
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

