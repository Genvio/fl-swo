<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="EditCountyCoordinator.aspx.vb" Inherits="EditCountyCoordinator"  %>

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
                    <b><asp:Label ID="lblAddEdit" runat="server" />County Coordinator</b>
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
                <b><font size="3">Name:</font></b>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCountyCoordinatorName" runat="server" Width="300px" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" width="40%">
                <b><font size="3">Email:</font></b>
            </td>
            <td align="left">
                <asp:TextBox ID="txtEmail" runat="server" Width="300px" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" width="40%">
                <b><font size="3">County:</font></b>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlCounty" Width="306px" DataTextField="County" DataValueField="CountyID" runat="server" />
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