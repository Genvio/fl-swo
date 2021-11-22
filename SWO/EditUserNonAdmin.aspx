<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="EditUserNonAdmin.aspx.vb" Inherits="EditUserNonAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .button
        {
            width: 90px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6">
                    <b><asp:Label ID="lblAddEdit" runat="server"></asp:Label>User</b>
                </font>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlMessage" runat="server" Visible="false">
        <table width="100%">
            <tr>
                <td align="left" colspan="2">
                    <div class="feature">
                        <table width="100%">
                            <tr>
                                <%--<td width="20%"  align="right">
                                    <img alt="Error Red X" src="Images/RedXIcon.JPG" />
                                </td>--%>
                                <td valign="middle" align="left">
                                    <%--<br />
                                    <br />
                                    <br />
                                    <br />
                                    <br />--%>
                                    <font size="5"><span style="color:#fe5105;">Please correct the following errors:</span></font>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center" colspan="2">
                                    <table width="100%">
                                        <tr align="left">
                                            <%--<td width="50%"></td>--%>
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
    </asp:Panel>
    <table width="100%" align="center">
        <tr>
            <td align="right" width="40%">
                <b><font size="3">First Name:</font></b>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFirstName" runat="server" Width="300px" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" width="40%">
                <b><font size="3">Last Name:</font></b>
            </td>
            <td align="left">
                <asp:TextBox ID="txtLastName" runat="server" Width="300px" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" width="40%">
                <b><font size="3">Email/Username:</font></b>
            </td>
            <td align="left">
                <asp:TextBox ID="txtEmail" Enabled="false" runat="server" Width="300px" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" width="40%">
                <b><font size="3">Password:</font></b>
            </td>
            <td align="left">
                <asp:TextBox ID="txtPassword" runat="server" Width="300px" TextMode="Password" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" width="40%">
                <b><font size="3">Confirm Password:</font></b>
            </td>
            <td align="left">
                <asp:TextBox ID="txtConfirmPassword" runat="server" Width="300px" TextMode="Password" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" width="40%">
                <b><font size="3">Secret Question:</font></b>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSecretQuestion" Width="306px" DataTextField="SecretQuestion" DataValueField="SecretQuestionID" runat="server" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" width="40%">
                <b><font size="3">Secret Answer:</font></b>
            </td>
            <td align="left">
                <asp:TextBox ID="txtSecretAnswer" runat="server" Width="300px" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" width="40%">
            <b><font size="3">User Level:</font></b>
            </td>
            <td align="left">
                <asp:DropDownList Enabled="false" ID="ddlUserLevel" Width="306px" DataTextField="UserLevel" DataValueField="UserLevelID" runat="server" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" width="40%">
                <b><font size="3">Is Active:</font></b>
            </td>
            <td align="left">
                <asp:CheckBox Enabled="false" ID="chkIsActive" runat="server" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <input type="button" class="button" value="Add" id="btnSave" runat="server" onserverclick="btnSave_Command" />
                &nbsp;&nbsp;&nbsp;
                <input type="button" class="button" value="Cancel" id="btnCancel" runat="server" onserverclick="btnCancel_Command" />
            </td>
        </tr>
    </table>
</asp:Content>