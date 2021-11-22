<%@ Page Language="VB" AutoEventWireup="false" CodeFile="IncidentStatusDisplay.aspx.vb" Inherits="IncidentStatusDisplay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="refresh" content="60"/>
    <title>Incident Status Display</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="pnlMessage" runat="server" Visible="false">
        <table width="100%">
            <tr>
                <td align="left">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table width="100%">
        <tr>
            <td align="center">
                <font size="6">
                    <b>Incident Status Display</b>
                </font>
            </td>
        </tr>
        <tr>
            <td align="center">
                <big><b><asp:Label ID="lblLastUpdated" runat="server" ></asp:Label></b></big>
            </td>
        </tr>
    </table>
    <table width="100%" align='center' border="1" cellspacing="0" style="border-color:#000000; font-family:Arial">
        <tr>
            <td valign="top" align="center" width="50%" border="1" cellspacing="0" style="border-color:#000000;">
                <asp:Panel ID="pnlShowIncidentDataGrid" runat="server" Visible="false">
                <%--<big><b>Open</b></big>--%>
                <asp:Panel ID="pnlShowCurrentButtons" runat="server" Visible="false">
                    <asp:DropDownList ID="ddlSearchBy" runat="server">
                    <asp:ListItem Value="[IncidentNumber].Number" Text="By Incident Number"></asp:ListItem>
                    <asp:ListItem Value="[IncidentStatus].IncidentStatus" Text="By Status"></asp:ListItem>
                    <asp:ListItem Value="[Incident].IncidentName" Text="By Incident Name"></asp:ListItem>
                    <asp:ListItem Value="[Incident].AddedCounty" Text="By County"></asp:ListItem>
                    <%--<asp:ListItem Value="[Incident].DateCreated" Text="By Date Created"></asp:ListItem>--%>
                    <asp:ListItem Value="[Incident].LastUpdated" Text="By Last Updated"></asp:ListItem>
                    <%--<asp:ListItem Value="[Incident].LastUpdated" Text="By Last Updated"></asp:ListItem>
                    <asp:ListItem Value="[Incident].LastUpdated" Text="By Last Updated"></asp:ListItem>--%>
                    </asp:DropDownList>&nbsp;&nbsp;<asp:TextBox runat="server" ID="txtSearch"></asp:TextBox>&nbsp;&nbsp;<asp:Button ID="btnSearch" Text="Search" runat="server" />
                </asp:Panel>
                <asp:DataGrid  ID="IncidentDataGrid" runat="server" Width="100%"
                    OnSortCommand="SortIncident" AutoGenerateColumns="False" AllowSorting="True"  PagerStyle-HorizontalAlign="center"
                    OnPageIndexChanged="IncidentDataGrid_PageIndexChanged" 
                        HeaderStyle-CssClass="datagridheader" 
                        AlternatingItemStyle-CssClass="datagridalt" Font-Size="XX-Large">
                    <AlternatingItemStyle CssClass="datagridalt" />
                    <Columns>
                        <%--   <asp:TemplateColumn    ItemStyle-Width="10px" >
                            <ItemTemplate>
                                <a href="Incident.aspx?IncidentID=<%# Container.dataitem("IncidentID")%>&Action=Delete"><img src="Images/delete-icon.png" alt="Delete Incident" border="0" width="16" height="16"  onclick="javascript:return confirm('Are you sure you want to delete this Incident?')" title="Delete Incident" /> </a>
                            </ItemTemplate>
                        </asp:TemplateColumn>--%>
                        <asp:TemplateColumn ItemStyle-Width="10px">
                            <ItemTemplate>
                                <a href='EditIncident.aspx?IncidentID=<%# Container.dataitem("IncidentID")%>'>
                                <img src="Images/edit.gif" alt="Edit Incident" border="0" width="16" height="16" title="Edit Incident" />
                                </a>
                            </ItemTemplate>
                            <ItemStyle Width="10px" />
                        </asp:TemplateColumn>
                        <asp:HyperLinkColumn DataNavigateUrlField="IncidentId" 
                            DataTextField="IncidentID" HeaderText="IncidentID" 
                            SortExpression="IncidentID ASC" Target="_parent" Visible="False">
                            <HeaderStyle Wrap="False" />
                        </asp:HyperLinkColumn>
                        <asp:BoundColumn DataField="IncidentStatus" ItemStyle-Width="165px"
                            HeaderText="Status" 
                            ItemStyle-HorizontalAlign="Center" SortExpression="IncidentStatus ASC">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="IncidentNumber" HeaderText="Incident #" 
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="Number ASC">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" Width="175px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="IncidentName" HeaderText="Name" 
                            ItemStyle-HorizontalAlign="Center" SortExpression="IncidentName ASC">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="AddedCounty" HeaderText="County(s)" 
                            ItemStyle-HorizontalAlign="Center" SortExpression="AddedCounty ASC">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <%--<asp:BoundColumn HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" DataField="DateCreated" SortExpression="DateCreated"  HeaderText="Date Created EST">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                        </asp:BoundColumn>--%>
                        <asp:BoundColumn DataField="LastUpdated" HeaderStyle-Width="150px" 
                            
                            HeaderText="Last Updated EST&lt;img src='Images/blue_arrow_down2.jpg' align='absmiddle' border=0&gt;" ItemStyle-HorizontalAlign="Center" 
                            SortExpression="LastUpdated">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <%--                 <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="CreatedBy" SortExpression="CreatedBy" HeaderText="Created By">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                        </asp:BoundColumn>
                        
                        <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataFormatString="{0:MM-dd-yyyy}" DataField="LastUpdated" SortExpression="LastUpdated" HeaderText="Last Updated">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                        </asp:BoundColumn>
                        
                        <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="UpdatedBy" SortExpression="UpdatedBy" HeaderText="Udated By">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                        </asp:BoundColumn>--%>
                    </Columns>
                    <HeaderStyle CssClass="datagridheader" />
                    <PagerStyle HorizontalAlign="Center" />
                </asp:DataGrid>
                </asp:Panel>
                <asp:Panel ID="pnlShowNoIncident" runat="server" Visible="false">
                <%--<big><b>No Open Incidents at this Time</b></big>--%>
                </asp:Panel>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
