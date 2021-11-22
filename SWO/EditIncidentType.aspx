<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="EditIncidentType.aspx.vb" Inherits="EditIncidentType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6">
                    <b>
                       <asp:Label ID="lblAddEdit" runat="server"></asp:Label> Incident Type     
                    </b>
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
                            <td width="20%"  align="right">
                                <img alt="Error Red X" src="Images/RedXIcon.JPG" />
                            </td>
                            <td valign="middle" align="left">
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <font size="5"><span  style="color:#fe5105;">Please Correct the following Errors:</span></font>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="center" colspan="2">
                                <table width="100%">
                                    <tr align="left">
                                        <td width="50%">
                                            &nbsp;
                                        </td>
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
    <table width="100%" align="center">
        <tr>
            <td align="right" width="40%">
            <b><font size="3">Incident Type:</font></b>
            </td>
            <td align="left">
                <asp:TextBox ID="txtIncidentType" runat="server" Width="300px"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="center">
            &nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;
            <input type="button" value="Add" id="btnSave" runat="server" onserverclick="btnSave_Command" />
            &nbsp;&nbsp;&nbsp;
            <input type="button" value="Cancel" id="btnCancel" runat="server" onserverclick="btnCancel_Command" />
            &nbsp;&nbsp;&nbsp;
            </td>
        </tr>
    </table>
</asp:Content>