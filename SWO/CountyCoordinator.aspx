<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="CountyCoordinator.aspx.vb" Inherits="CountyCoordinator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6"><b>County Coordinators</b></font>
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
                 Click on Icon to Add County Coordinator
            </td>
        </tr>
        <tr>
            <td align="center">
                <a href="EditCountyCoordinator.aspx?CountyCoordinatorID=0"><img border="0" alt="Add CountyCoordinator" src="Images/CountyCoordinator.png" /></a>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:DropDownList ID="ddlSearchBy" runat="server">
                    <asp:ListItem Value="[CountyCoordinator].CountyCoordinatorName" Text="By Name" />
                    <asp:ListItem Value="[County].County" Text="By County" />
                </asp:DropDownList>&nbsp;&nbsp;
                <asp:TextBox runat="server" ID="txtSearch" />&nbsp;&nbsp;
                <asp:Button ID="btnSearch" Text="Search" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
               <asp:DataGrid ID="CountyCoordinatorDataGrid" runat="server" Width="100%"
                    OnSortCommand="SortCountyCoordinator" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="True" PageSize="100000" PagerStyle-HorizontalAlign="center"
                    OnPageIndexChanged="CountyCoordinatorDataGrid_PageIndexChanged" >
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="CountyCoordinator.aspx?CountyCoordinatorID=<%# Container.dataitem("CountyCoordinatorID")%>&Action=Delete"><img src="Images/delete-icon.png" alt="Delete County Coordinator" border="0" width="16" height="16" onclick="javascript:return confirm('Are you sure you want to delete this County Coordinator?')" title="Delete Region Coordinator" /></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="EditCountyCoordinator.aspx?CountyCoordinatorID=<%# Container.dataitem("CountyCoordinatorID")%>"><img src="Images/edit.gif" alt="Edit CountyCoordinator" border="0" width="16" height="16" title="Edit CountyCoordinator" /></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="CountyCoordinatorID"
                            DataTextField="CountyCoordinatorID" SortExpression="CountyCoordinatorID ASC" HeaderText="CountyCoordinatorID">
                            <HeaderStyle Wrap="False" />
                        </asp:HyperLinkColumn>
                                        
                        <asp:BoundColumn ItemStyle-HorizontalAlign="Center"   DataField="CountyCoordinatorName" SortExpression="CountyCoordinatorName ASC" HeaderText="Name <img src='Images/blue_arrow_UP2.jpg' align='absmiddle' border=0>">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundColumn>
                        
                        <asp:BoundColumn ItemStyle-HorizontalAlign="Center"   DataField="County" SortExpression="County ASC" HeaderText="County">
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