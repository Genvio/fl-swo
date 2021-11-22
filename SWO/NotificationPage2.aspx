<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="NotificationPage2.aspx.vb" Inherits="NotificationPage2" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%">
        <tr>
            <td align="center" width="100%">
               <big>Notification Page</big>
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
                                            <asp:Label ID="lblMessage2" runat="server"></asp:Label>
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
               <asp:TextBox ID="txtCustomSubject" Width="850px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <asp:Panel ID="pnlShowSystemGenerated" runat="server" Visible="false">
    <table width="100%">
        <tr>
            <td align="left" width="100%">
               <b>System Generated Subject:</b>
               <asp:DropDownList ID="ddlSG" AutoPostBack="true" style="background-color:#c2ecde" Width="154px"  runat="server">
                    <asp:ListItem Value="INITIAL" Text="Initial" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="UPDATE" Text="Update"></asp:ListItem>
                    <asp:ListItem Value="FINAL UPDATE" Text="Final"></asp:ListItem>
               </asp:DropDownList>
               
               <asp:DropDownList ID="ddlSG2" AutoPostBack="true" style="background-color:#c2ecde" Width="154px"  runat="server">
                    <asp:ListItem Value="INFO ONLY" Text="Report Only" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="PLEASE REPLY" Text="Reply Requested"></asp:ListItem>
                    <asp:ListItem Value="PLEASE REPLY" Text="Reply Required"></asp:ListItem>
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
               <asp:Label ID="lblSubject" runat="server"></asp:Label> 
            </td>
        </tr>
    </table>
    </asp:Panel>
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
                <asp:TextBox ID="txtNameList" Height="100px" Width="985px" runat="server" TextMode="MultiLine"></asp:TextBox>
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
                <asp:TextBox ID="txtEmailList" Height="100px" Width="985px" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                <b>Outgoing Notifications and Comments:</b>
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
            <td align="center" style="border-color:#000000; border-style:solid">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="left" style="border-color:#000000; border-style:solid">
                <asp:TextBox ID="txtOutgoingNotification" Height="150px" Width="430px" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
            <td align="left" style="border-color:#000000; border-style:solid">
                <asp:TextBox ID="txtOutgoingComment" Height="150px" Width="430px" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
            <td align="center" style="border-color:#000000; border-style:solid">
                <asp:Button ID="btnOutgoingNotificationComment" runat="server" Text="Add Update" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <asp:HyperLink ID="lnkHistoryOutgoingNotificationComment" Target="_blank" runat="server" Text="View History"></asp:HyperLink>
            </td>
        </tr>
    </table>
    <br />
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                <b>Document Replies to Notifications:</b>
            </td>
        </tr>
    </table>
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="center" style="border-color:#000000; border-style:solid">
                Replies
            </td>
            <td align="center" style="border-color:#000000; border-style:solid">
                Comments
            </td>
            <td align="center" style="border-color:#000000; border-style:solid">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="left" style="border-color:#000000; border-style:solid">
                <asp:TextBox ID="txtReplyNotification" Height="150px" Width="430px" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
            <td align="left" style="border-color:#000000; border-style:solid">
                <asp:TextBox ID="txtReplyComment" Height="150px" Width="430px" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
            <td align="center" style="border-color:#000000; border-style:solid">
                <asp:Button ID="btnReplyComment" runat="server" Text="Add Update" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <asp:HyperLink ID="lnkHistoryReplyComment" runat="server" Target="_blank" Text="View History"></asp:HyperLink>
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
                <asp:HyperLink runat="server" ID="lnkViewBlackberryReport" Target="_blank" Text="View Blackberry Report"></asp:HyperLink>
            </td>
            <td align="center">
                <asp:Button ID="btnReturnToWorksheet" runat="server" Text="Return to Worksheet" /> 
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
</asp:Content>
