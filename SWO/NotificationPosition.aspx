<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="NotificationPosition.aspx.vb" Inherits="NotificationPosition"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6"><b>Positions</b></font>
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
                 Click on Icon to Add a Position
            </td>
        </tr>
        <tr>
            <td  align="center">
                <a href="EditNotificationPosition.aspx?NotificationPositionID=0"><img border="0" alt="Add NotificationPosition" src="Images/Position.jpg" /></a> 
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:DropDownList ID="ddlSearchBy" runat="server">
                    <asp:ListItem Value="[NotificationPosition].Position" Text="By Position" />
                    <%--<asp:ListItem Value="[NotificationPosition].Abbreviation" Text="By Abbreviation" />--%>
                </asp:DropDownList>&nbsp;&nbsp;
                <asp:TextBox runat="server" ID="txtSearch" />&nbsp;&nbsp;
                <asp:Button ID="btnSearch" Text="Search" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
               <asp:DataGrid ID="NotificationPositionDataGrid" runat="server" Width="100%"
                    OnSortCommand="SortNotificationPosition" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="True" PageSize="100000" PagerStyle-HorizontalAlign="center"
                    OnPageIndexChanged="NotificationPositionDataGrid_PageIndexChanged" HeaderStyle-CssClass="datagridheader" AlternatingItemStyle-CssClass="datagridalt">
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="NotificationPosition.aspx?NotificationPositionID=<%# Container.dataitem("NotificationPositionID")%>&Action=Delete"><img src="Images/delete-icon.png" alt="Delete Position" border="0" width="16" height="16"  onclick="javascript:return confirm('Are you sure you want to delete this Position?')" title="Delete Position" /></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="EditNotificationPosition.aspx?NotificationPositionID=<%# Container.dataitem("NotificationPositionID")%>"><img src="Images/edit.gif" alt="Edit Position" border="0" width="16" height="16" title="Edit Position" /></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="NotificationPositionID"
                            DataTextField="NotificationPositionID" SortExpression="NotificationPositionID ASC" HeaderText="NotificationPositionID">
                            <HeaderStyle Wrap="False" />
                        </asp:HyperLinkColumn>
                                        
                        <asp:BoundColumn ItemStyle-HorizontalAlign="Center"   DataField="Position" SortExpression="Position ASC" HeaderText="Position <img src='Images/blue_arrow_UP2.jpg' align='absmiddle' border=0>">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>