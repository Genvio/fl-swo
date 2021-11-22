<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="Agency.aspx.vb" Inherits="Agency" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6"><b>Agency</b></font>
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
                 Click on Icon to Add Agency
            </td>
        </tr>
        <tr>
            <td align="center">
                <a href="EditAgency.aspx?AgencyID=0"><img border="0" alt="Add Agency" src="Images/Agency.jpg" /></a>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:DropDownList ID="ddlSearchBy" runat="server">
                    <asp:ListItem Value="[Agency].Agency" Text="By Agency" />
                    <asp:ListItem Value="[Agency].Abbreviation" Text="By Abbreviation" />
                </asp:DropDownList>&nbsp;&nbsp;
                <asp:TextBox runat="server" ID="txtSearch" />&nbsp;&nbsp;
                <asp:Button ID="btnSearch" Text="Search" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
               <asp:DataGrid ID="AgencyDataGrid" runat="server" Width="100%"
                    OnSortCommand="SortAgency" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="True" PageSize="15" PagerStyle-HorizontalAlign="center"
                    OnPageIndexChanged="AgencyDataGrid_PageIndexChanged" HeaderStyle-CssClass="datagridheader" AlternatingItemStyle-CssClass="datagridalt">
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="Agency.aspx?AgencyID=<%# Container.dataitem("AgencyID")%>&Action=Delete"><img src="Images/delete-icon.png" alt="Delete Agency" border="0" width="16" height="16"  onclick="javascript:return confirm('Are you sure you want to delete this Agency?')" title="Delete Agency" /> </a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="EditAgency.aspx?AgencyID=<%# Container.dataitem("AgencyID")%>"><img src="Images/edit.gif" alt="Edit Agency" border="0" width="16" height="16" title="Edit Agency" /> </a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="AgencyID"
                            DataTextField="AgencyID" SortExpression="AgencyID ASC" HeaderText="AgencyID">
                            <HeaderStyle Wrap="False" />
                        </asp:HyperLinkColumn>
                                        
                        <asp:BoundColumn ItemStyle-HorizontalAlign="Center"   DataField="Agency" SortExpression="Agency ASC" HeaderText="Agency <img src='Images/blue_arrow_UP2.jpg' align='absmiddle' border=0>">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundColumn>
               
                        <asp:BoundColumn DataField="Abbreviation" ItemStyle-HorizontalAlign="Center" SortExpression="Abbreviation" HeaderText="Abbreviation">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
