<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="Audit.aspx.vb" Inherits="Audit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6"><b>Audit</b></font>
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="5"><asp:Label ID="lblMessage" runat="server"  Visible="true" /></font>
            </td>
        </tr>
        <tr>
            <td></td>
        </tr>
        <tr>
            <td align="center">
                <asp:DropDownList ID="ddlSearchBy" runat="server">
                    <asp:ListItem Value="[User].Email" Text="By User" />
                    <asp:ListItem Value="[AuditType].AuditType" Text="By Action" />
                    <asp:ListItem Value="[Audit].Action" Text="By Audit Type" />
                    <asp:ListItem Value="[Audit].AuditDate" Text="By Date" />
                </asp:DropDownList>&nbsp;&nbsp;
                <asp:TextBox runat="server" ID="txtSearch" />&nbsp;&nbsp;
                <asp:Button ID="btnSearch" Text="Search" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
               <asp:DataGrid ID="AuditDataGrid" runat="server" Width="100%"
                    OnSortCommand="SortAudit" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="True" PageSize="15" PagerStyle-HorizontalAlign="center"
                    OnPageIndexChanged="AuditDataGrid_PageIndexChanged" HeaderStyle-CssClass="datagridheader" AlternatingItemStyle-CssClass="datagridalt">
                    <Columns>
                        <asp:HyperLinkColumn Visible="False" ItemStyle-HorizontalAlign="Center" Target="_parent" DataNavigateUrlField="AuditId"
                            DataTextField="AuditID" SortExpression="AuditID ASC" HeaderText="AuditID">
                            <HeaderStyle Wrap="False" />
                        </asp:HyperLinkColumn>
                                        
                        <asp:BoundColumn  DataField="Email" ItemStyle-HorizontalAlign="Center" SortExpression="Email ASC" HeaderText="User <img src='Images/blue_arrow_UP2.jpg' align='absmiddle' border=0>">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundColumn>
                        
                        <asp:BoundColumn DataField="AuditType" ItemStyle-HorizontalAlign="Center" SortExpression="AuditType" HeaderText="Audit Type">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundColumn>
                        
                        
                        <asp:BoundColumn DataField="Action" ItemStyle-HorizontalAlign="Center" SortExpression="Action" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundColumn>
                        
                        <asp:BoundColumn DataField="AuditDate" ItemStyle-HorizontalAlign="Center" SortExpression="AuditDate" HeaderText="Date">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>