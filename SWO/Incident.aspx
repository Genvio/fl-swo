<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="Incident.aspx.vb" Inherits="Incident" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <AJAX:UpdatePanel ID="AJAXUpdatePanel" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMessage" runat="server" Visible="false">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        <font size="6"><b>Current Incidents</b></font>
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnAddIncident" Text="Add Incident" runat="server" />
                        <asp:Panel ID="pnlSearchIncidents" runat="server" DefaultButton="btnSearch" style="display:inline;">
                            <asp:DropDownList ID="ddlSearchBy" runat="server">
                                <asp:ListItem Value="[IncidentNumber].Number" Text="By Incident Number"></asp:ListItem>
                                <asp:ListItem Value="[IncidentStatus].IncidentStatus" Text="By Status"></asp:ListItem>
                                <asp:ListItem Value="[Incident].IncidentName" Text="By Incident Name"></asp:ListItem>
                                <asp:ListItem Value="[Incident].AddedCounty" Text="By County"></asp:ListItem>
                                <asp:ListItem Value="[Incident].DateCreated" Text="By Date Created"></asp:ListItem>
                                <asp:ListItem Value="[Incident].LastUpdated" Text="By Last Updated"></asp:ListItem>
                                <asp:ListItem Value="[Incident].Address" Text="By Street Address"></asp:ListItem>
                                <asp:ListItem Value="[Incident].FacilityNameSceneDescription" Text="By Facility/Scene Desc"></asp:ListItem>
                                <asp:ListItem Value="[Incident].City" Text="By City"></asp:ListItem>
                                <%--<asp:ListItem Value="[Incident].LastUpdated" Text="By Last Updated"></asp:ListItem>
                                <asp:ListItem Value="[Incident].LastUpdated" Text="By Last Updated"></asp:ListItem>--%>
                            </asp:DropDownList>
                            &nbsp;
                            <asp:TextBox runat="server" ID="txtSearch"></asp:TextBox>
                            &nbsp;
                            <asp:Button ID="btnSearch" Text="Search" runat="server" />
                        </asp:Panel>
                        &nbsp;Filter By Worksheet Type:
                        <asp:DropDownList ID="ddlIncidentType" Width="200px" DataTextField="IncidentType" DataValueField="IncidentTypeID" runat="server" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        Filter By Agency:
                        <asp:DropDownList ID="ddlAgency" Width="200px" DataTextField="Abbreviation" DataValueField="AgencyID" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlIncidentType_SelectedIndexChanged" />
                        &nbsp;
                        <asp:Button ID="btnReset" Text="Reset" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                       <asp:DataGrid  ID="IncidentDataGrid" runat="server" Width="100%"
                            OnSortCommand="SortIncident" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="false" PagerStyle-HorizontalAlign="center"
                            OnPageIndexChanged="IncidentDataGrid_PageIndexChanged" HeaderStyle-CssClass="datagridheader" AlternatingItemStyle-CssClass="datagridalt">
                            <Columns>
                                <%--<asp:TemplateColumn ItemStyle-Width="10px" >
                                    <ItemTemplate>
                                        <a href="Incident.aspx?IncidentID=<%# Container.dataitem("IncidentID")%>&Action=Delete"><img src="Images/delete-icon.png" alt="Delete Incident" border="0" width="16" height="16"  onclick="javascript:return confirm('Are you sure you want to delete this Incident?')" title="Delete Incident" /> </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>--%>
                        
                                <asp:TemplateColumn ItemStyle-Width="10px" >
                                    <ItemTemplate>
                                        <a href="EditIncident.aspx?IncidentID=<%# Container.dataitem("IncidentID")%>"><img src="Images/edit.gif" alt="Edit Incident" border="0" width="16" height="16" title="Edit Incident" /> </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>

                                <asp:TemplateColumn ItemStyle-Width="10px" >
                                    <ItemTemplate>
                                        <a href="Reports/FullMainReport.aspx?IncidentID=<%# Container.dataitem("IncidentID")%>&ReportFormat=HTML"><img src="Images/find.gif" alt="View Incident" border="0" width="16" height="16" title="View Incident" /> </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                        
                                <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="IncidentId"
                                    DataTextField="IncidentID" SortExpression="IncidentID" HeaderText="IncidentID">
                                    <HeaderStyle Wrap="False"></HeaderStyle>
                                </asp:HyperLinkColumn>
                              
                                <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="IncidentNumber" SortExpression="IncidentID" HeaderText="Incident # <img src='Images/blue_arrow_down2.jpg' align='absmiddle' border=0>">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                </asp:BoundColumn>
                        
                                <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="IncidentStatus" SortExpression="IncidentStatus" HeaderText="Status">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                </asp:BoundColumn>
                                  
                                <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="IncidentName" SortExpression="IncidentName" HeaderText="Incident Name">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                </asp:BoundColumn>
                        
                                <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="AddedCounty" SortExpression="AddedCounty" HeaderText="County">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                </asp:BoundColumn>
                        
                                <asp:BoundColumn HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" DataField="DateCreated" SortExpression="DateCreatedSort"  HeaderText="Date Created ET">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                </asp:BoundColumn>
                        
                                <asp:BoundColumn HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" DataField="LastUpdated" SortExpression="LastUpdatedSort"  HeaderText="Last Updated ET">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                </asp:BoundColumn>
                        
                                <%--<asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="CreatedBy" SortExpression="CreatedBy" HeaderText="Created By">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                </asp:BoundColumn>
                        
                                <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataFormatString="{0:MM-dd-yyyy}" DataField="LastUpdated" SortExpression="LastUpdated" HeaderText="Last Updated">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                </asp:BoundColumn>
                        
                                <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="UpdatedBy" SortExpression="UpdatedBy" HeaderText="Udated By">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                </asp:BoundColumn>--%>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>    
            </table>
            <br />
        </ContentTemplate>
    </AJAX:UpdatePanel>
</asp:Content>