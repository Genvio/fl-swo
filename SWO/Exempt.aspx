<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="Exempt.aspx.vb" Inherits="Exempt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6"><b>Exempt</b></font>
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
                 Click on Icon to Add Exempt
            </td>
        </tr>
        <tr>
            <td align="center">
                <a href="EditExempt.aspx?ExemptID=0"><img border="0" alt="Add Exempt" src="Images/Exempt.gif" /></a>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:DropDownList ID="ddlSearchBy" runat="server">
                    <asp:ListItem Value="[IncidentType].IncidentType" Text="By Incident Type" />
                </asp:DropDownList>&nbsp;&nbsp;
                <asp:TextBox runat="server" ID="txtSearch" />&nbsp;&nbsp;
                <asp:Button ID="btnSearch" Text="Search" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
               <asp:DataGrid ID="ExemptDataGrid" runat="server" Width="100%"
                    OnSortCommand="SortExempt" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="True" PageSize="15" PagerStyle-HorizontalAlign="center"
                    OnPageIndexChanged="ExemptDataGrid_PageIndexChanged" HeaderStyle-CssClass="datagridheader" AlternatingItemStyle-CssClass="datagridalt">
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="Exempt.aspx?ExemptID=<%# Container.dataitem("ExemptID")%>&Action=Delete"><img src="Images/delete-icon.png" alt="Delete Exempt" border="0" width="16" height="16" onclick="javascript:return confirm('Are you sure you want to delete this Exempt?')" title="Delete Exempt" /></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="EditExempt.aspx?ExemptID=<%# Container.dataitem("ExemptID")%>"><img src="Images/edit.gif" alt="Edit Exempt" border="0" width="16" height="16" title="Edit Exempt" /></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="ExemptID"
                            DataTextField="ExemptID" SortExpression="ExemptID ASC" HeaderText="ExemptID">
                            <HeaderStyle Wrap="False" />
                        </asp:HyperLinkColumn>
                                        
                        <asp:BoundColumn ItemStyle-HorizontalAlign="Center"   DataField="IncidentType" SortExpression="IncidentType ASC" HeaderText="Incident Type <img src='Images/blue_arrow_UP2.jpg' align='absmiddle' border=0>">
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