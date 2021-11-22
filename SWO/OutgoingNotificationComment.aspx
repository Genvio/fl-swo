<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="OutgoingNotificationComment.aspx.vb" Inherits="OutgoingNotificationComment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table align="center" width="100%" >
        <tr>
            <td align="center">
                <font size="5"><b>Outgoing Notifications and Comments</b></font>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblResults" runat="server" ></asp:Label>
</asp:Content>
