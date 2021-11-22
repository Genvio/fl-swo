<%@ Page Title="" Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="EditMessage.aspx.vb" Inherits="EditMessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6">
                    <b>Add Message</b>
                </font>
            </td>
        </tr>
    </table>
    <br />
    <table align="center">
        <tr>
            <td align="right">
                <font size="3">
                    <asp:Label ID="lblMessage" runat="server" Text="Message:" Font-Bold="True"></asp:Label>
                </font>
            </td>
            <td>
                <asp:TextBox ID="txtMessage" runat="server" Width="200"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <font size="3">
                    <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Italic="true" ForeColor="Red"></asp:Label>
                </font>
            </td>
        </tr>
    </table>
    <br />
    <table align="center">
        <tr>
            <td>
                <asp:Button ID="btnSaveMessage" runat="server" Text="Save Message" Width="100" />
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" Width="75" />
            </td>
            <td>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="100" />
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

