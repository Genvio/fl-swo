<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="Worksheets.aspx.vb" Inherits="Worksheets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6"><b>Worksheets</b></font>
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
            <td align="left">
                 <%--Click on Icon to Add Worksheet--%>
            </td>
        </tr>
        <tr>
            <td align="center">
                <%-- <a href="EditIncidentType.aspx?IncidentTypeID=0"><img border="0" alt="Add IncidentType" src="Images/IncidentType.JPG" /></a>--%>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:DropDownList ID="ddlSearchBy" runat="server">
                    <asp:ListItem Value="[IncidentType].IncidentType" Text="Worksheet" />
                </asp:DropDownList>&nbsp;&nbsp;
                <asp:TextBox runat="server" ID="txtSearch" />&nbsp;&nbsp;
                <asp:Button ID="btnSearch" Text="Search" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
               <asp:DataGrid ID="IncidentTypeDataGrid" runat="server" Width="100%"
                    OnSortCommand="SortIncidentType" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="True" PageSize="100" PagerStyle-HorizontalAlign="center"
                    OnPageIndexChanged="IncidentTypeDataGrid_PageIndexChanged" HeaderStyle-CssClass="datagridheader" AlternatingItemStyle-CssClass="datagridalt">
                    <Columns>
                        <asp:TemplateColumn ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="Worksheets.aspx?IncidentTypeID=<%# Container.dataitem("IncidentTypeID")%>&Action=Delete"><img src="Images/delete-icon.png" alt="Delete Worksheet" border="0" width="16" height="16" onclick="javascript:return confirm('Are you sure you want to delete this Worksheet?')" title="Delete Worksheet" /></a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="EditWorksheet.aspx?IncidentTypeID=<%# Container.dataitem("IncidentTypeID")%>"><img src="Images/edit.gif" alt="Edit Worksheet" border="0" width="16" height="16" title="Edit Worksheet" /> </a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="IncidentTypeId"
                            DataTextField="IncidentTypeID" SortExpression="IncidentTypeID ASC" HeaderText="IncidentTypeID">
                            <HeaderStyle Wrap="False" />
                        </asp:HyperLinkColumn>
                                        
                        <asp:BoundColumn  DataField="IncidentType" SortExpression="IncidentType ASC" HeaderText="Worksheet <img src='Images/blue_arrow_UP2.jpg' align='absmiddle' border=0>">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>