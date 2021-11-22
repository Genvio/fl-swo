<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="NotificationGroup.aspx.vb" Inherits="NotificationGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6"><b>Notification Group</b></font>
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
                 Click on Icon to Add a Notification Group 
            </td>
        </tr>
        <tr>
            <td align="center">
                <a href="EditNotificationGroup.aspx?NotificationGroupID=0"><img border="0" alt="Add NotificationGroup" src="Images/GroupNotification.jpg" /></a>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:DropDownList ID="ddlSearchBy" runat="server">
                    <asp:ListItem Value="[NotificationGroup].GroupName" Text="By Group Name" />
                </asp:DropDownList>&nbsp;&nbsp;
                <asp:TextBox runat="server" ID="txtSearch" />&nbsp;&nbsp;
                <asp:Button ID="btnSearch" Text="Search" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
               <asp:DataGrid  ID="NotificationGroupDataGrid" runat="server" Width="100%"
                    OnSortCommand="SortNotificationGroup" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="True" PageSize="100000" PagerStyle-HorizontalAlign="center"
                    OnPageIndexChanged="NotificationGroupDataGrid_PageIndexChanged" HeaderStyle-CssClass="datagridheader" AlternatingItemStyle-CssClass="datagridalt">
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="NotificationGroup.aspx?NotificationGroupID=<%# Container.dataitem("NotificationGroupID")%>&Action=Delete"><img src="Images/delete-icon.png" alt="Delete NotificationGroup" border="0" width="16" height="16" onclick="javascript:return confirm('Are you sure you want to delete this Notification Group?')" title="Delete Notification Group" /></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="EditNotificationGroup.aspx?NotificationGroupID=<%# Container.dataitem("NotificationGroupID")%>"><img src="Images/edit.gif" alt="Edit NotificationGroup" border="0" width="16" height="16" title="Edit NotificationGroup" /></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="NotificationGroupID"
                            DataTextField="NotificationGroupID" SortExpression="NotificationGroupID ASC" HeaderText="NotificationGroupID">
                            <HeaderStyle Wrap="False" />
                        </asp:HyperLinkColumn>
                                        
                        <asp:BoundColumn ItemStyle-HorizontalAlign="Center"   DataField="GroupName" SortExpression="GroupName ASC" HeaderText="Name <img src='Images/blue_arrow_UP2.jpg' align='absmiddle' border=0>">
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