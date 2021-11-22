<%@ Page Title="" Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="Sector.aspx.vb" Inherits="Sector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6"><b>Sectors</b></font>
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
                 Click on Icon to Add Sector
            </td>
        </tr>
        <tr>
            <td align="center">
                <a href="EditSector.aspx?SectorID=0"><img border="0" alt="Add Sector" src="Images/sectorsblue.png" /></a>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:DropDownList ID="ddlSearchBy" runat="server">
                    <asp:ListItem Value="[Sector].SectorName" Text="By Name" />
                </asp:DropDownList>&nbsp;&nbsp;
                <asp:TextBox runat="server" ID="txtSearch" />&nbsp;&nbsp;
                <asp:Button ID="btnSearch" Text="Search" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
               <asp:DataGrid ID="SectorDataGrid" runat="server" Width="100%"
                    OnSortCommand="SortSector" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="True" PageSize="10000" PagerStyle-HorizontalAlign="center"
                    OnPageIndexChanged="SectorDataGrid_PageIndexChanged" >
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="Sector.aspx?SectorID=<%# Container.dataitem("SectorID")%>&Action=Delete"><img src="Images/delete-icon.png" alt="Delete Sector?" border="0" width="16" height="16" onclick="javascript:return confirm('Are you sure you want to delete this Sector?')" title="Delete Sector" /></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="EditSector.aspx?SectorID=<%# Container.dataitem("SectorID")%>"><img src="Images/edit.gif" alt="Edit Sector" border="0" width="16" height="16" title="Edit Sector" /> </a>
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="SectorID"
                            DataTextField="SectorID" SortExpression="SectorID ASC" HeaderText="SectorID">
                            <HeaderStyle Wrap="False" />
                        </asp:HyperLinkColumn>
                                        
                        <asp:BoundColumn ItemStyle-HorizontalAlign="Center"   DataField="SectorName" SortExpression="SectorName ASC" HeaderText="Name <img src='Images/blue_arrow_UP2.jpg' align='absmiddle' border=0>">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

