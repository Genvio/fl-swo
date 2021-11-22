<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="WeatherReports.aspx.vb" Inherits="WeatherReports" %>

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
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAddMap" />
            <AJAX:PostBackTrigger ControlID="btnAddMap" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        <b><font size="6">Weather Reports</font></b>
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%" align="center">
                <tr>
                    <td align="right">
                        <big><b>Sub-Types:</b></big>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlSubType" AutoPostBack="true" style="background-color:#c2ecde" Width="200px"  runat="server">
                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Local Storm Report" Text="Local Storm Report"></asp:ListItem>
                            <asp:ListItem Value="NOAA Transnsmitter Outage" Text="NOAA Transnsmitter Outage"></asp:ListItem>
                         </asp:DropDownList>
                    </td>
                    <td align="right">
                        <big><b>This situation is:</b></big>
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
                        <big><b>Worksheet Name:</b></big>
                    </td>
                    <td align="left" colspan="3">
                        <asp:TextBox ID="txtWorkSheetDescription" Width="716px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <big><b>Notification:</b></big>
                    </td>
                    <td align="left" colspan="3">
                        <asp:DropDownList ID="ddlNotification" DataTextField="Description" DataValueField="IncidentTypeLevelID"  style="background-color:#c2ecde" Width="722px" runat="server"></asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <table align="center" width="100%">
                <tr>
                    <td style="background-color: #d4d4d4" align="left">
                        <h1>Information</h1>
                    </td>
                </tr>
            </table>

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

            <asp:Panel ID="pnlShowAttachments" runat="server" Visible="true">
                <table align="center" width="75%" cellspacing="0" border="1" style="border-color:#000000">
                    <tr>
                        <td align="left" width="10%" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                            <b>Maps:</b>
                        </td>
                        <td align="left" width="50%" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                            Select Map:<asp:FileUpload ID="FileUpload1" runat="server" />
                        </td>
                        <td align="left" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                            Map Name:<asp:TextBox ID="txtMapName" style="background-color:#c2ecde" runat="server" />
                        </td>
                        <td align="center" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                            <asp:Button ID="btnAddMap" runat="server" Text="Add Map" />
                        </td>
                    </tr>
                </table>
                <table align="center" width="75%">
                    <tr>
                        <td>
                        <asp:DataGrid ID="dgWeatherMap" runat="server" Width="100%" AutoGenerateColumns="false" PageSize="10" PagerStyle-HorizontalAlign="center">
                            <Columns>
                                <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="WeatherMapID"
                                    DataTextField="WeatherMapID" SortExpression="WeatherMapID ASC" HeaderText="WeatherMapID">
                                    <HeaderStyle Wrap="False"></HeaderStyle>
                                </asp:HyperLinkColumn>
                            
                                <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <a href="WeatherReports.aspx?WeatherMapID=<%# Container.dataitem("WeatherMapID")%>&IncidentID=<%# Container.dataitem("IncidentID")%>&IncidentIncidentTypeID=<%# Container.dataitem("IncidentIncidentTypeID")%>&Action=Delete&Parameter=WeatherMap"><img src="Images/delete-icon.png" alt="Delete Map" border="0" width="16" height="16" onclick="javascript:return confirm('Are you sure you want to delete this Map?')" title="Delete Map" /></a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            
                                <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <a target="_blank" href="Uploads/<%# Container.dataitem("Map")%>"><img src="Images/find.gif" alt="Map" border="0" width="16" height="16" title="Map" /> </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            
                                <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="MapName" SortExpression="MapName" HeaderText="<b>Map Name<b/>">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                </asp:BoundColumn>
                            
                                <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="UploadDate" SortExpression="UploadDate" HeaderText="<b>Upload Date<b/>">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                </asp:BoundColumn>

                                <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="UserName" SortExpression="UserName" HeaderText="<b>Uploaded By<b/>">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                        </td>
                    </tr>
                </table>
                <br />
                <table align="center" width="75%" cellspacing="0" border="1" style="border-color:#000000">
                    <tr>
                        <td align="left" width="10%" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                            <b>Links:</b>
                        </td>
                        <td align="left" width="50%" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                            <asp:TextBox ID="txtLink" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                        </td>
                        <td align="center" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                            <asp:Button ID="btnAddLink" runat="server" Text="Add Link" />
                        </td>
                    </tr>
                </table>
                <table align="center" width="75%">
                    <tr>
                        <td>
                        <asp:DataGrid ID="dgWeatherLink" runat="server" Width="100%" AutoGenerateColumns="false" PageSize="10" PagerStyle-HorizontalAlign="center">
                            <Columns>
                                <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="WeatherLinkID"
                                    DataTextField="WeatherLinkID" SortExpression="WeatherLinkID ASC" HeaderText="WeatherLinkID">
                                    <HeaderStyle Wrap="False"></HeaderStyle>
                                </asp:HyperLinkColumn>
                            
                                <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <a href="WeatherReports.aspx?WeatherLinkID=<%# Container.dataitem("WeatherLinkID")%>&IncidentID=<%# Container.dataitem("IncidentID")%>&IncidentIncidentTypeID=<%# Container.dataitem("IncidentIncidentTypeID")%>&Action=Delete&Parameter=WeatherLink"><img src="Images/delete-icon.png" alt="Delete Link" border="0" width="16" height="16" onclick="javascript:return confirm('Are you sure you want to delete this Link?')" title="Delete Link" /></a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            
                                <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <a target="_blank" href="<%# Container.dataitem("Link")%>"><img src="Images/find.gif" alt="Link" border="0" width="16" height="16" title="Link" /></a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            
                                <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="LinkName" SortExpression="LinkName" HeaderText="<b>Link Name<b/>">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                </asp:BoundColumn>
                            
                                <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="UploadDate" SortExpression="UploadDate" HeaderText="<b>Upload Date<b/>">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                </asp:BoundColumn>

                                <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="UserName" SortExpression="UserName" HeaderText="<b>Uploaded By<b/>">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
            
            <asp:Panel ID="pnlShowLocalStormReport" runat="server" Visible="false">
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style86">
                            <big><b>Type of Report:</b></big>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlLSRreportType"   
                                style="background-color:#c2ecde; margin-left: 4px;" Width="300px"  
                                runat="server">
                                <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Flooding" Text="Flooding"></asp:ListItem>
                                <asp:ListItem Value="Hail" Text="Hail"></asp:ListItem>
                                <asp:ListItem Value="Tornado Touchdown - Confirmed" Text="Tornado Touchdown - Confirmed"></asp:ListItem>
                                <asp:ListItem Value="Tornado Touchdown - Unconfirmed" Text="Tornado Touchdown - Unconfirmed"></asp:ListItem>
                                <asp:ListItem Value="Funnel Cloud" Text="Funnel Cloud"></asp:ListItem>
                                <asp:ListItem Value="Waterspout" Text="Waterspout"></asp:ListItem>
                                <asp:ListItem Value="Wind Damage" Text="Wind Damage"></asp:ListItem>
                                <asp:ListItem Value="Wind Damage - Thunderstorm" Text="Wind Damage - Thunderstorm"></asp:ListItem>
                                <asp:ListItem Value="Lightning Strike" Text="Lightning Strike"></asp:ListItem>
                                <asp:ListItem Value="Smoke" Text="Smoke"></asp:ListItem>
                                <asp:ListItem Value="Fog" Text="Fog"></asp:ListItem>
                                <asp:ListItem Value="Frost" Text="Frost"></asp:ListItem>
                                <asp:ListItem Value="Freeze Damage" Text="Freeze Damage"></asp:ListItem>
                                <asp:ListItem Value="Tsunami" Text="Tsunami"></asp:ListItem>
                                <asp:ListItem Value="Drought" Text="Drought"></asp:ListItem>
                             </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style86">
                            <big><b>Report was received:</b></big>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlLSRreportReceived"   
                                style="background-color:#c2ecde; margin-left: 4px;" Width="300px"  
                                runat="server">
                                <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="From NWS WFO" Text="From NWS WFO"></asp:ListItem>
                                <asp:ListItem Value="From County EM/WP/Public Safety" Text="From County EM/WP/Public Safety"></asp:ListItem>
                                <asp:ListItem Value="From Private Citizen" Text="From Private Citizen"></asp:ListItem>
                             </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center" style="display:none">
                    <tr>
                        <td align="right" class="style86">
                            <big><b>Are there Injuries?</b></big>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlLSRInjury"  
                                style="background-color:#c2ecde; margin-left: 4px;" Width="300px"  
                                runat="server" AutoPostBack="true">
                                <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                                <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                <asp:ListItem Value="No" Text="No"></asp:ListItem>
                             </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlShowInjuryText" runat="server" Visible="false">
                    <table width="100%" align="center">
                        <tr>
                            <td align="right" class="style84">
                                <big><b>Number and Severity of Injuries:</b></big>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtLSRInjuryText" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table width="100%" align="center" style="display:none">
                    <tr>
                        <td align="right" class="style86">
                            <big><b>Are there Fatalities?</b></big>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlLSRFatality"  
                                style="background-color:#c2ecde; margin-left: 4px;" Width="300px"  
                                runat="server" AutoPostBack="true">
                                <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                                <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                <asp:ListItem Value="No" Text="No"></asp:ListItem>
                             </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlShowFatalityText" runat="server" Visible="false">
                    <table width="100%" align="center">
                        <tr>
                            <td align="right" class="style84">
                                <big><b>Number and location:</b></big>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtLSRFatalityText" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style86">
                            <big><b>Are there any displacements?</b></big>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlLSRdisplacement"  
                                style="background-color:#c2ecde; margin-left: 4px;" Width="300px"  
                                runat="server" AutoPostBack="true">
                                <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                                <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                <asp:ListItem Value="No" Text="No"></asp:ListItem>
                             </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlShowLSRdisplacementText" runat="server" Visible="false">
                    <table width="100%" align="center">
                        <tr>
                            <td align="right" class="style84">
                                <big><b>Number and where are they being sheltered:</b></big>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtLSRdisplacementText" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style86">
                            <big><b>Is there any damage to structures?</b></big>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlLSRdamageStructures"  
                                style="background-color:#c2ecde; margin-left: 4px;" Width="300px"  
                                runat="server" AutoPostBack="true">
                                <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                                <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                <asp:ListItem Value="No" Text="No"></asp:ListItem>
                             </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlShowLSRdamageStructuresText" runat="server" Visible="false">
                    <table width="100%" align="center">
                        <tr>
                            <td align="right" class="style84">
                                <big><b>Type of Structures / Number / Severity:</b></big>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtLSRdamageStructuresText" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style86">
                            <big><b>Is there any damage to Infrastructure?</b></big>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlLSRinfrastructureDamage"  
                                style="background-color:#c2ecde; margin-left: 4px;" Width="300px"  
                                runat="server" AutoPostBack="true">
                                <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                                <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                <asp:ListItem Value="No" Text="No"></asp:ListItem>
                             </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlShowLSRinfrastructureDamageText" runat="server" Visible="false">
                    <table width="100%" align="center">
                        <tr>
                            <td align="right" class="style84">
                                <big><b>Describe:</b></big>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtLSRinfrastructureDamageText" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            
            <asp:Panel ID="pnlShowTransmitterOutage" runat="server" Visible="false">
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big><b>Transmitter(s):</b></big>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTOtransmitter" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big><b>Weather Forecast Office making notification:</b></big>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTOmakingNotification" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big><b>Date Out of Service:</b></big>
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" style="background-color:#c2ecde" Columns="10" Width="80px" ID="txtTOserviceOutDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                            <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                            href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtTOserviceOutDate');"><img alt="Calendar Icon"
                            src="Images/Calendar1.jpg" border="0" width="20" height="15"/></a>
                            <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtTOserviceOutDate = ''"
                            onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big><b>Time Out of Service:</b></big>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTOserviceOutTime"  Width="15px" 
                                style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                                onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                                onmouseout="UnTip()"></asp:TextBox>
                            <big><b>:</b></big>
                            <asp:TextBox ID="txtTOserviceOutTime2"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                            &nbsp;<big><b>ET</b></big>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big><b>Transmitter is out of service due to:</b></big>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTOtransmitterServiceDueTo" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style84">
                            <big><b>Time the transmitter(s) are expected to return to service:</b></big>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTOreturnToService" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
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