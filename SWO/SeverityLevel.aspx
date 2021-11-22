<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="SeverityLevel.aspx.vb" Inherits="SeverityLevel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6"><b>Severity Levels Coming Soon</b></font>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlShowAll" Visible="false" runat="server">
        <table width="100%" align="center">
            <tr>
                <td align="center">
                    <font size="6"><b>Severity Levels</b></font>
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
                <td align="center">
                     Click on Icon to Add Severity Level
                </td>
            </tr>
            <tr>
                <td align="center">
                    <a href="EditSeverityLevel.aspx?SeverityLevelID=0"><img border="0" alt="Add SeverityLevel" src="Images/SeverityLevel.jpg" /></a>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:DropDownList ID="ddlSearchBy" runat="server">
                        <asp:ListItem Value="[SeverityLevel].SeverityLevel" Text="By Severity Level" />
                        <asp:ListItem Value="[SeverityLevel].Notes" Text="By Notes" />
                    </asp:DropDownList>&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtSearch" />&nbsp;&nbsp;
                    <asp:Button ID="btnSearch" Text="Search" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                   <asp:DataGrid  ID="SeverityLevelDataGrid" runat="server" Width="100%"
                        OnSortCommand="SortSeverityLevel" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="True" PageSize="15" PagerStyle-HorizontalAlign="center"
                        OnPageIndexChanged="SeverityLevelDataGrid_PageIndexChanged" HeaderStyle-CssClass="datagridheader" AlternatingItemStyle-CssClass="datagridalt">
                        <Columns>
                            <asp:TemplateColumn ItemStyle-Width="10px" >
                                <ItemTemplate>
                                    <a href="SeverityLevel.aspx?SeverityLevelID=<%# Container.dataitem("SeverityLevelID")%>&Action=Delete"><img src="Images/delete-icon.png" alt="Delete SeverityLevel" border="0" width="16" height="16" onclick="javascript:return confirm('Are you sure you want to delete this SeverityLevel?')" title="Delete SeverityLevel" /></a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        
                            <asp:TemplateColumn ItemStyle-Width="10px" >
                                <ItemTemplate>
                                    <a href="EditSeverityLevel.aspx?SeverityLevelID=<%# Container.dataitem("SeverityLevelID")%>"><img src="Images/edit.gif" alt="Edit SeverityLevel" border="0" width="16" height="16" title="Edit SeverityLevel" /> </a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        
                            <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="SeverityLevelId"
                                DataTextField="SeverityLevelID" SortExpression="SeverityLevelID ASC" HeaderText="SeverityLevelID">
                                <HeaderStyle Wrap="False" />
                            </asp:HyperLinkColumn>
                                        
                            <asp:BoundColumn  DataField="SeverityLevel" SortExpression="SeverityLevel ASC" HeaderText="Severity Level <img src='Images/blue_arrow_UP2.jpg' align='absmiddle' border=0>">
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundColumn>
                        
                            <%--<asp:BoundColumn DataField="FirstName" SortExpression="FirstName" HeaderText="First Name">
                                <HeaderStyle Wrap="False" />
                            </asp:BoundColumn>--%>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
</asp:Content>