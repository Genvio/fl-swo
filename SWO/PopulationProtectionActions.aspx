<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="PopulationProtectionActions.aspx.vb" Inherits="PopulationProtectionActions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style84
        {
            width: 456px;
        }
        .style86
        {
            width: 452px;
        }
        .style87
        {
            width: 453px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <AJAX:UpdatePanel ID="AJAXUpdatePanel" runat="server">
    <ContentTemplate>
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <b>
                    <font size="6">
                        Population Protection Actions
                    </font>
                </b>
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" align="center">
        <tr>
            <td align="right">
                <big>
                    <b>
                        Sub-Types:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSubType" AutoPostBack="true" style="background-color:#c2ecde" Width="200px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Shelter in place" Text="Shelter in place"></asp:ListItem>
                    <asp:ListItem Value="Evacuation Order" Text="Evacuation Order"></asp:ListItem>
                    <asp:ListItem Value="Emergency Shelter Opened" Text="Emergency Shelter Opened"></asp:ListItem>
                 </asp:DropDownList>
            </td>
            <td align="right">
                <big>
                    <b>
                        This situation is:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSituation"  style="background-color:#c2ecde" Width="200px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Active" Text="Active"></asp:ListItem>
                    <asp:ListItem Value="Past Report" Text="Past Report"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                <big>
                    <b>
                       Worksheet Name:
                    </b>
                </big>
            </td>
            <td align="left" colspan="3">
                <asp:TextBox ID="txtWorkSheetDescription" Width="716px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <big>
                    <b>
                       Notification:
                    </b>
                </big>
            </td>
            <td align="left" colspan="3">
                <asp:DropDownList ID="ddlNotification" DataTextField="Description" DataValueField="IncidentTypeLevelID"  style="background-color:#c2ecde" Width="722px"  runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <br />
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    Information
                </h1>
            </td>
        </tr>
    </table>
    
    <asp:Panel ID="pnlShowShelterInPlaceEvacuation" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Impacted area, including streets or landmarks:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtImpactedStreetLandmark" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Department/Agency issuing the order:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDeptAgencyIssuingOrder" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Duration of the order(if known):
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDuration" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Number of residences impacted (if known):
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtImpactResidenceNum" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Number of businesses impacted (if known):
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtImpactBusinessNum" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Total number of individuals impacted (if known):
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtTotalImpacted" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <asp:Panel ID="pnlShowEmergencyShelterOpened" runat="server" Visible="false">
        <table align="center" width="100%">
            <tr>
                <td align="center">
                    <asp:Button ID="btnAddShelter" runat="server" Text="Add Shelter" />
                </td>
            </tr>
        </table>
        <asp:Panel runat="server" ID="pnlShowShelterGrid" Visible="false">
        <table align="center" width="25%">
           <tr>
                <td>
                    <asp:DataGrid ID="ShelterDataGrid" runat="server" Width="100%"
                        AutoGenerateColumns="false" AllowPaging="True" PageSize="10" PagerStyle-HorizontalAlign="center"
                        OnPageIndexChanged="ShelterDataGrid_PageIndexChanged" >
                        <Columns>
                            <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="ShelterID"
                                DataTextField="ShelterID" SortExpression="ShelterID ASC" HeaderText="ShelterID">
                                <HeaderStyle Wrap="False"></HeaderStyle>
                            </asp:HyperLinkColumn>
                            
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a href="PopulationProtectionActions.aspx?ShelterID=<%# Container.dataitem("ShelterID")%>&IncidentID=<%# Container.dataitem("IncidentID")%>&IncidentIncidentTypeID=<%# Container.dataitem("IncidentIncidentTypeID")%>&Action=Delete&Parameter=Shelter"><img src="Images/delete-icon.png" alt="Delete Shelter" border="0" width="16" height="16"  onclick="javascript:return confirm('Are you sure you want to delete this Shelter?')" title="Delete Shelter" /> </a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a href="EditShelter.aspx?ShelterID=<%# Container.dataitem("ShelterID")%>&IncidentID=<%# Container.dataitem("IncidentID")%>&IncidentIncidentTypeID=<%# Container.dataitem("IncidentIncidentTypeID")%>"><img src="Images/edit.gif" alt="Edit Shelter" border="0" width="16" height="16" title="Edit Shelter" /> </a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="ShelterName" SortExpression="ShelterName" HeaderText="<b><u>&nbsp; Shelter Name &nbsp; </u><b/>">
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" ></HeaderStyle>
                            </asp:BoundColumn>

                        </Columns>
                    </asp:DataGrid>
                    
                </td>
            </tr>
        </table>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="pnlMessage" runat="server" Visible="false">
    <table width="100%">
        <tr>
            <td align="left" colspan="2">
                <div class="feature">
                    <table width="100%">
                        <tr>
                            <td valign="top" align="center">
                                <table width="100%">
                                    <tr align="left">
                                        <td align="left">
                                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <br />
    <br />
    <table width="100%" align="center">
        <tr>
            <td align="center">
                &nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;
                <input type="button" value="Save Incident" id="btnSave" runat="server" onserverclick="btnSave_Command" />
                &nbsp;&nbsp;&nbsp;
                <input type="button" value="Cancel" id="btnCancel" runat="server" onserverclick="btnCancel_Command" />
                &nbsp;&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    <br />
    <br />
    </ContentTemplate>
    </AJAX:UpdatePanel>
</asp:Content>

