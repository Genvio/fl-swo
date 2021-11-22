<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="IncidentType.aspx.vb" Inherits="IncidentType" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6">
                    <b>
                        Incident Types    
                    </b>
                </font>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td>
               &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <font size="5"><asp:Label ID="lblMessage" runat="server"  Visible="true"></asp:Label></font>
            </td>
        </tr>
        <tr>
            <td align="left">
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 &nbsp;&nbsp;
                 Click on Icon to Add Incident Type
            </td>
        </tr>
        <tr>
            <td  align="center">
           
            <a  href="EditIncidentType.aspx?IncidentTypeID=0"><img border="0" alt="Add Incident Type" src="Images/IncidentType.jpg" /></a> 
                <asp:DropDownList ID="ddlSearchBy" runat="server">
                <asp:ListItem Value="[IncidentType].IncidentType" Text="By Incident Type"></asp:ListItem>
                </asp:DropDownList>&nbsp;&nbsp;<asp:TextBox runat="server" ID="txtSearch"></asp:TextBox>&nbsp;&nbsp;<asp:Button ID="btnSearch" Text="Search" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
               <asp:DataGrid  ID="IncidentTypeDataGrid" runat="server" Width="100%"
                    OnSortCommand="SortIncidentType" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="True" PageSize="15" PagerStyle-HorizontalAlign="center"
                    OnPageIndexChanged="IncidentTypeDataGrid_PageIndexChanged" HeaderStyle-CssClass="datagridheader" AlternatingItemStyle-CssClass="datagridalt">
                    <Columns>
                    
                        <asp:TemplateColumn    ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="IncidentType.aspx?IncidentTypeID=<%# Container.dataitem("IncidentTypeID")%>&Action=Delete"><img src="Images/delete-icon.png" alt="Delete Incident Type" border="0" width="16" height="16"  onclick="javascript:return confirm('Are you sure you want to delete this Incident Type?')" title="Delete IncidentType" /> </a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn   ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="EditIncidentType.aspx?IncidentTypeID=<%# Container.dataitem("IncidentTypeID")%>"><img src="Images/edit.gif" alt="Edit IncidentType" border="0" width="16" height="16" title="Edit IncidentType" /> </a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="IncidentTypeId"
                            DataTextField="IncidentTypeID" SortExpression="IncidentTypeID ASC" HeaderText="IncidentTypeID">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                        </asp:HyperLinkColumn>
                                        
                        <asp:BoundColumn  DataField="IncidentType" SortExpression="IncidentType ASC" HeaderText="Incident Type <img src='Images/blue_arrow_UP2.jpg' align='absmiddle' border=0>">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                        </asp:BoundColumn>
                      <%--  
                        <asp:BoundColumn DataField="FirstName" SortExpression="FirstName" HeaderText="First Name">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                        </asp:BoundColumn>
                      --%>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>    
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
