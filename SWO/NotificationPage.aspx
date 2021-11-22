<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="NotificationPage.aspx.vb" Inherits="NotificationPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6"><b>Notification Page</b></font>
            </td>
        </tr>
    </table>
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
                                            <asp:Label ID="lblMessage2" runat="server" />
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
    <br />
    <table width="100%">
        <tr>
            <td align="center" width="100%">
               <asp:RadioButton ID="rdoSystemGenerated" Checked="true" Text="System Generated Subject" runat="server" GroupName="rdoSubjectGroup" AutoPostBack="true" />
               <asp:RadioButton ID="rdoCustom" Text="Custom Subject" runat="server" GroupName="rdoSubjectGroup" AutoPostBack="true" />
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowCustom" runat="server" Visible="false">
        <table width="100%">
            <tr>
                <td align="left" width="100%">
                   <b>Custom Subject:</b>
                   <asp:TextBox ID="txtCustomSubject" Width="850px" style="background-color:#c2ecde" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlShowSystemGenerated" runat="server" Visible="false">
        <table width="100%">
            <tr>
                <td align="left" width="100%">
                   <b>System Generated Subject:</b>
                   <asp:DropDownList ID="ddlSG" AutoPostBack="true" style="background-color:#c2ecde" Width="154px" runat="server">
                        <asp:ListItem Value="INITIAL" Text="Initial" Selected="True" />
                        <asp:ListItem Value="UPDATE" Text="Update" />
                        <asp:ListItem Value="FINAL UPDATE" Text="Final" />
                   </asp:DropDownList>
               
                   <asp:DropDownList ID="ddlSG2" AutoPostBack="true" style="background-color:#c2ecde" Width="154px" runat="server">
                        <asp:ListItem Value="INFO ONLY" Text="Report Only" Selected="True" />
                        <asp:ListItem Value="PLEASE REPLY" Text="Reply Requested" />
                        <asp:ListItem Value="PLEASE REPLY" Text="Reply Required" />
                   </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlShowSubjectLabel" runat="server" Visible="false">
        <table width="100%">
            <tr>
                <td align="left" width="100%">
                   <b>Subject: </b>
                   <asp:Label ID="lblSubject" runat="server" /> 
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--Added/edited on 1/26/12 by JD.--%>
    <%----------------------------------------------------------%>
    <table width="100%">
        <tr>
            <td align="left" width="100%">
                <b>Incident #: </b>
                <asp:Label ID="lblIncidentNumber" runat="server" /> 
            </td>
        </tr>
    </table>
    <%----------------------------------------------------------%>
    <br />
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                <b>Notification List:</b>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td align="left">
                <asp:TextBox ID="txtNameList" Height="100px" Width="985px" runat="server" TextMode="MultiLine" />
            </td>
        </tr>
    </table>
    <br />
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                <b>Email List:</b>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td align="left">
                <asp:TextBox ID="txtEmailList" Height="100px" Width="985px" runat="server" TextMode="MultiLine" />
            </td>
        </tr>
    </table>
    <br />
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                <b>Associated Tasks:</b>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td align="left">
                <asp:TextBox ID="txtAssociatedTask" Height="100px" Width="985px" runat="server" TextMode="MultiLine" />
            </td>
        </tr>
    </table>
    <br />
    <table width="100%">
        <tr>
            <td align="center">
                <asp:Button ID="btnSendNotification" Visible="true" runat="server" Text="Send Notification" />
            </td>
            <td align="center">
                <asp:HyperLink runat="server" ID="lnkViewBlackberryReport" Target="_blank" Text="View Blackberry Report" />
            </td>
            <td align="center">
                <asp:Button ID="btnReturnToWorksheet" runat="server" Text="Return to Worksheet" /> 
            </td>
        </tr>
    </table>
    <br />
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                <b>Notifications and Comments:</b>
            </td>
        </tr>
    </table>
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="center" style="border-color:#000000; border-style:solid">
                Notifications
            </td>
            <td align="center" style="border-color:#000000; border-style:solid">
                Comments
            </td>
            <td align="center" style="border-color:#000000; border-style:solid"></td>
        </tr>
        <tr>
            <td align="left" style="border-color:#000000; border-style:solid">
                <asp:TextBox ID="txtOutgoingNotification" Height="150px" Width="430px" runat="server" TextMode="MultiLine" />
            </td>
            <td align="left" style="border-color:#000000; border-style:solid">
                <asp:TextBox ID="txtOutgoingComment" Height="150px" Width="430px" runat="server" TextMode="MultiLine" />
            </td>
            <td align="center" style="border-color:#000000; border-style:solid">
                <asp:Button ID="btnOutgoingNotificationComment" runat="server" Text="Add Update" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <%--<asp:HyperLink ID="lnkHistoryOutgoingNotificationComment" Target="_blank" runat="server" Text="View History" />--%>
                <asp:Label ID="lblResults" runat="server" Text="" />
            </td>
        </tr>
    </table>
    <br />
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
                                            <asp:Label ID="lblMessage" runat="server" />
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
</asp:Content>