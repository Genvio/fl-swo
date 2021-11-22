<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="RegionCoordinator.aspx.vb" Inherits="RegionCoordinator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6"><b>Region Coordinators</b></font>
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="5"><asp:Label ID="lblMessage" runat="server" Visible="true" /></font>
            </td>
        </tr>
        <tr>
            <td align="center">
                 Click on Icon to Add Region Coordinator
            </td>
        </tr>
        <tr>
            <td align="center">
                <a href="EditRegionCoordinator.aspx?RegionCoordinatorID=0"><img border="0" alt="Add RegionCoordinator" src="Images/UserLevels.png" /></a>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:DropDownList ID="ddlSearchBy" runat="server">
                    <asp:ListItem Value="[RegionCoordinator].RegionCoordinatorName" Text="By Name" />
                    <asp:ListItem Value="[RegionCoordinator].RegionCoordinator" Text="By Region" />
                </asp:DropDownList>&nbsp;&nbsp;
                <asp:TextBox runat="server" ID="txtSearch" />&nbsp;&nbsp;
                <asp:Button ID="btnSearch" Text="Search" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
               <asp:DataGrid ID="RegionCoordinatorDataGrid" runat="server" Width="100%"
                    OnSortCommand="SortRegionCoordinator" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="True" PageSize="10000" PagerStyle-HorizontalAlign="center"
                    OnPageIndexChanged="RegionCoordinatorDataGrid_PageIndexChanged" >
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="RegionCoordinator.aspx?RegionCoordinatorID=<%# Container.dataitem("RegionCoordinatorID")%>&Action=Delete"><img src="Images/delete-icon.png" alt="Delete Region Coordinator?" border="0" width="16" height="16" onclick="javascript:return confirm('Are you sure you want to delete this Region Coordinator?')" title="Delete Region Coordinator" /></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="EditRegionCoordinator.aspx?RegionCoordinatorID=<%# Container.dataitem("RegionCoordinatorID")%>"><img src="Images/edit.gif" alt="Edit RegionCoordinator" border="0" width="16" height="16" title="Edit RegionCoordinator" /> </a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="RegionCoordinatorID"
                            DataTextField="RegionCoordinatorID" SortExpression="RegionCoordinatorID ASC" HeaderText="RegionCoordinatorID">
                            <HeaderStyle Wrap="False" />
                        </asp:HyperLinkColumn>
                                        
                        <asp:BoundColumn ItemStyle-HorizontalAlign="Center"   DataField="RegionCoordinatorName" SortExpression="RegionCoordinatorName ASC" HeaderText="Name <img src='Images/blue_arrow_UP2.jpg' align='absmiddle' border=0>">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundColumn>
                        
                        <asp:BoundColumn ItemStyle-HorizontalAlign="Center"   DataField="Region" SortExpression="Region ASC" HeaderText="Region">
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
    <br />
</asp:Content>