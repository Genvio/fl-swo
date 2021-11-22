<%@ Page Language="VB" AutoEventWireup="false" CodeFile="IncidentList.aspx.vb" Inherits="IncidentList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="refresh" content="300">
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:DataGrid ID="IncidentDataGrid" runat="server" Width="100%" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="false" PagerStyle-HorizontalAlign="center"
                OnPageIndexChanged="IncidentDataGrid_PageIndexChanged" AlternatingItemStyle-CssClass="datagridalt">
                <Columns>
                    <asp:TemplateColumn ItemStyle-Width="10px">
                        <ItemTemplate>
                            <a href="FullMainReport.aspx?IncidentID=<%# Container.DataItem("IncidentID")%>&ReportFormat=HTML" target="_blank">
                                <img src="../Images/find.gif" alt="View Incident" border="0" width="16" height="16" title="View Incident" />
                            </a>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="IncidentId"
                        DataTextField="IncidentID" HeaderText="IncidentID">
                        <HeaderStyle Wrap="False"></HeaderStyle>
                    </asp:HyperLinkColumn>

                    <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="IncidentNumber"  HeaderText="Incident # ">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" ></HeaderStyle>
                    </asp:BoundColumn>

                    <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="IncidentStatus"  HeaderText="Status">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                    </asp:BoundColumn>

                    <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="IncidentName"  HeaderText="Incident Name">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                    </asp:BoundColumn>

                    <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="AddedCounty"  HeaderText="County">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                    </asp:BoundColumn>

                    <asp:BoundColumn HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center"  DataField="DateCreated" HeaderText="Date Created ET">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                    </asp:BoundColumn>

                    <asp:BoundColumn HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" DataField="LastUpdated" HeaderText="Last Updated ET">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                    </asp:BoundColumn>
                </Columns>
            </asp:DataGrid>
        </div>
    </form>
</body>
</html>
