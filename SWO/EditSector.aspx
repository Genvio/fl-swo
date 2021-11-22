<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="EditSector.aspx.vb" Inherits="EditSector" title="Untitled Page" %>

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
                    <b><asp:Label ID="lblAddEdit" runat="server" />Sector</b>
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
                <asp:TextBox ID="txtSectorName" runat="server" Width="300px" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" width="40%">
                <b><font size="3">Position:</font></b>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSectorPosition" Width="555px" DataTextField="Position" DataValueField="NotificationPositionID" runat="server" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" width="40%">
                <b><font size="3">&nbsp;</font></b>
            </td>
            <td align="left">
               <asp:Button ID="btnAddPosition" runat="server" Text="Add Position" Width="150px" />
            </td>
        </tr>
    </table>
    <asp:Panel runat="server" ID="pnlShowSectorPositionGrid" Visible="false">
        <table align="center" width="100%">
           <tr>
                <td>
                    <asp:DataGrid ID="SectorPositionDataGrid" runat="server" Width="100%"
                        AutoGenerateColumns="false" AllowPaging="True" PageSize="100" PagerStyle-HorizontalAlign="center">
                        <Columns>
                            <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="SectorPositionID"
                                DataTextField="SectorPositionID" SortExpression="SectorPositionID ASC" HeaderText="SectorPositionID">
                                <HeaderStyle Wrap="False" />
                            </asp:HyperLinkColumn>
                            
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a href="EditSector.aspx?SectorPositionID=<%# Container.dataitem("SectorPositionID")%>&SectorID=<%# Container.dataitem("SectorID")%>&Action=Delete&Parameter=SectorPosition"><img src="Images/delete-icon.png" alt="Delete Position" border="0" width="16" height="16" onclick="javascript:return confirm('Are you sure you want to delete this Position?')" title="Delete Position" /></a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="Position" SortExpression="Position" HeaderText="<b><u>&nbsp; Position &nbsp; </u><b/>">
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <input type="button" class="button" value="Add" id="btnSave" runat="server" onserverclick="btnSave_Command" />
                &nbsp;&nbsp;&nbsp;
                <input type="button" class="button" value="Cancel" id="btnCancel" runat="server" onserverclick="btnCancel_Command" />
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

