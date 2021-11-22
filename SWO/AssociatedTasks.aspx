<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="AssociatedTasks.aspx.vb" Inherits="AssociatedTasks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6"><b>Associated Tasks</b></font>
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
                 Click on Icon to Add Associated Task
            </td>
        </tr>
        <tr>
            <td align="center">
                <a href="EditAssociatedTask.aspx?AssociatedTaskID=0"><img border="0" alt="Add AssociatedTask" src="Images/Tasks.jpg" /></a>
            </td>
        </tr >
        <tr>
            <td align="center">
                <asp:DropDownList ID="ddlSearchBy" runat="server">
                    <asp:ListItem Value="[AssociatedTask].AssociatedTaskName" Text="By Name" />
                    <asp:ListItem Value="[AssociatedTask].AssociatedTask" Text="By Task" />
                </asp:DropDownList>&nbsp;&nbsp;
                <asp:TextBox runat="server" ID="txtSearch" />&nbsp;&nbsp;
                <asp:Button ID="btnSearch" Text="Search" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
               <asp:DataGrid ID="AssociatedTaskDataGrid" runat="server" Width="100%"
                    OnSortCommand="SortAssociatedTask" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="True" PageSize="15" PagerStyle-HorizontalAlign="center"
                    OnPageIndexChanged="AssociatedTaskDataGrid_PageIndexChanged" >
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="AssociatedTasks.aspx?AssociatedTaskID=<%# Container.dataitem("AssociatedTaskID")%>&Action=Delete"><img src="Images/delete-icon.png" alt="Delete Associated Task" border="0" width="16" height="16"  onclick="javascript:return confirm('Are you sure you want to delete this Associated Task?')" title="Delete Associated Task" /> </a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="EditAssociatedTask.aspx?AssociatedTaskID=<%# Container.dataitem("AssociatedTaskID")%>"><img src="Images/edit.gif" alt="Edit AssociatedTask" border="0" width="16" height="16" title="Edit AssociatedTask" /> </a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="AssociatedTaskID"
                            DataTextField="AssociatedTaskID" SortExpression="AssociatedTaskID ASC" HeaderText="AssociatedTaskID">
                            <HeaderStyle Wrap="False" />
                        </asp:HyperLinkColumn>
                                        
                        <asp:BoundColumn ItemStyle-HorizontalAlign="Center"   DataField="AssociatedTaskName" SortExpression="AssociatedTaskName ASC" HeaderText="Name <img src='Images/blue_arrow_UP2.jpg' align='absmiddle' border=0>">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundColumn>
                        
                        <asp:BoundColumn ItemStyle-HorizontalAlign="Center"   DataField="AssociatedTask" SortExpression="AssociatedTask ASC" HeaderText="Task">
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