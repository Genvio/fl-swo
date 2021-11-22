<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="User.aspx.vb" Inherits="User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6"><b>Users</b></font>
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
                 Click on Icon to Add User
            </td>
        </tr>
        <tr>
            <td  align="center">
                <a href="EditUser.aspx?UserID=0"><img border="0" alt="Add User" src="Images/AddUser.jpg" /></a>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:DropDownList ID="ddlSearchBy" runat="server">
                    <asp:ListItem Value="[User].Email" Text="By Username/Email" />
                    <asp:ListItem Value="[User].FirstName" Text="By First Name" />
                    <asp:ListItem Value="[User].LastName" Text="By Last Name" />
                    <asp:ListItem Value="[User].LastLogin" Text="By Last Login" />
                </asp:DropDownList>&nbsp;&nbsp;
                <asp:TextBox runat="server" ID="txtSearch" />&nbsp;&nbsp;
                <asp:Button ID="btnSearch" Text="Search" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center">
                Check this box to include inactive users. <asp:CheckBox runat="server" ID="cbxIncludeInactives" />
                <asp:Button runat="server" ID="btnRefresh" Text="Refresh" />
            </td>
        </tr>
        <tr>
            <td>
               <asp:DataGrid ID="UserDataGrid" runat="server" Width="100%"
                    OnSortCommand="SortUser" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="True" PageSize="50" PagerStyle-HorizontalAlign="center"
                    OnPageIndexChanged="UserDataGrid_PageIndexChanged" HeaderStyle-CssClass="datagridheader" AlternatingItemStyle-CssClass="datagridalt">
                    <Columns>
                        <asp:TemplateColumn Visible="False" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="User.aspx?UserID=<%# Container.dataitem("UserID")%>&Action=Delete"><img src="Images/delete-icon.png" alt="Delete User" border="0" width="16" height="16" onclick="javascript:return confirm('Are you sure you want to delete this User?')" title="Delete User" /></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="EditUser.aspx?UserID=<%# Container.dataitem("UserID")%>"><img src="Images/edit.gif" alt="Edit User" border="0" width="16" height="16" title="Edit User" /> </a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="UserId"
                            DataTextField="UserID" SortExpression="UserID ASC" HeaderText="UserID">
                            <HeaderStyle Wrap="False" />
                        </asp:HyperLinkColumn>
                                        
                        <asp:BoundColumn DataField="Email" SortExpression="Email ASC" HeaderText="Username/Email">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundColumn>
                        
                        <asp:BoundColumn DataField="FirstName" SortExpression="FirstName ASC" HeaderText="First Name">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundColumn>
                        
                        <asp:BoundColumn DataField="LastName" SortExpression="LastName ASC" HeaderText="Last Name <img src='Images/blue_arrow_UP2.jpg' align='absmiddle' border=0>">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundColumn>

                        <asp:BoundColumn DataField="LastLogin" SortExpression="LastLogin ASC" HeaderText="Last Login">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>