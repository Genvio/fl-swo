<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="DrinkingWaterFacility.aspx.vb" Inherits="DrinkingWaterFacility" %>

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
                        Drinking Water Facility
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
                    <asp:ListItem Value="DWF Report" Text="DWF Report"></asp:ListItem>
                    <asp:ListItem Value="Boil Water Advisory" Text="Boil Water Advisory"></asp:ListItem>
                    <asp:ListItem Value="FlaWARN Generator Deployment" Text="FlaWARN Generator Deployment"></asp:ListItem>
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
                        Description:
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
    
    
    <asp:Panel ID="pnlShowDWFReport" runat="server" Visible="false">
    
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Public Water System ID Number:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtPublicWaterSystemIDNumber" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Name of Facility:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFacilityName" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    
    
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Was there any trespassing, vandalism, or theft?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlTrespassVandalismTheft"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowtxtTrespassVandalismTheftText" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Describe what occurred:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtTrespassVandalismTheftText" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Any damage to the facility or distribution system?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlDamageFacilityDistibutionSystem"   
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowIntentional" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Was it intentional?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDFDSintentional" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    
    
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Was ANY access made to the water supply?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlAccessWaterSupply"   
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Degradation to water quality, system pressure, or water production?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlDegredation"   
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                    <asp:ListItem Value="Potential" Text="Potential"></asp:ListItem>
                    <asp:ListItem Value="Likely" Text="Likely"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Description of the individual(s) responsible:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtIndividualResponsible" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Has local Law Enforcement been contacted?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlLawEnforcementContacted"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowEnforcementContactedCaseNumber" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Case number, if known:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtIndividualResponsibleCaseNumber" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <br />
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <big>
                    <b>
                        <i>
                            Attach any relevant documentation received to this incident on Main 
                Form.
                        </i>
                    </b>
                </big>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    
    <asp:Panel ID="pnlShowBoilWaterAdvisory" runat="server" Visible="false">
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Public Water System ID Number:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtBWpublicWaterSystemIDNumber" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style86">
                    <big>
                        <b>
                            This incident was due to a:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlBWIncidentDueTo"   
                        style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                        runat="server">
                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="Failure" Text="Failure"></asp:ListItem>
                        <asp:ListItem Value="Planned Outage" Text="Planned Outage"></asp:ListItem>
                     </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Number of customers affected:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtBWnumberCustomersAffected" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" class="style84">
                    <big>
                        <b>
                            Affected Areas, including streets or boundaries:
                        </b>
                    </big>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtBWaffectedAreas" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <table width="100%" align="center">
            <tr>
                <td align="center">
                    <big>
                        <b>
                            <i>
                                Attach any relevant documentation received to this incident on Main 
                    Form.
                            </i>
                        </b>
                    </big>
                </td>
            </tr>
        </table>
    </asp:Panel>


    <asp:Panel ID="pnlShowFlaWARN" runat="server" Visible="false">
		<table width="100%" align="center">
			<tr>
				<td align="right" class="style84">
					<big>
						<b>
							Public Water System ID Number:
						</b>
					</big>
				</td>
				<td align="left">
					<asp:TextBox ID="txtFWpublicWaterSystemIDNumber" Width="500px" style="background-color:#c2ecde" runat="server" MaxLength="250"></asp:TextBox>
				</td>
			</tr>
		</table>
		<table width="100%" align="center">
			<tr>
				<td align="right" class="style84">
					<big>
						<b>
							Number of customers affected:
						</b>
					</big>
				</td>
				<td align="left">
					<asp:TextBox ID="txtFWnumberCustomersAffected" Width="500px" style="background-color:#c2ecde" runat="server" MaxLength="250"></asp:TextBox>
				</td>
			</tr>
		</table>
		<table width="100%" align="center">
			<tr>
				<td align="right" class="style84">
					<big>
						<b>
							Name of Utility:
						</b>
					</big>
				</td>
				<td align="left">
					<asp:TextBox ID="txtFWutilityName" Width="500px" style="background-color:#c2ecde" runat="server" MaxLength="250"></asp:TextBox>
				</td>
			</tr>
		</table>
		<table width="100%" align="center">
			<tr>
				<td align="right" class="style84">
					<big>
						<b>
							Cause for need of generator:
						</b>
					</big>
				</td>
				<td align="left">
					<asp:TextBox ID="txtFWcauseForNeed" Width="500px" style="background-color:#c2ecde" runat="server" MaxLength="250"></asp:TextBox>
				</td>
			</tr>
		</table>
		<table width="100%" align="center">
			<tr>
				<td align="right" class="style84">
					<big>
						<b>
							Anticipated duration of need:
						</b>
					</big>
				</td>
				<td align="left">
					<asp:TextBox ID="txtFWdurationOfNeed" Width="500px" style="background-color:#c2ecde" runat="server" MaxLength="250"></asp:TextBox>
				</td>
			</tr>
		</table>
        <br />
        <table width="100%" align="center">
            <tr>
                <td align="center">
                    <big>
                        <b>
                            <i>
                                List a point of contact for the requesting utility as the “on-scene contact” for this incident.
                            </i>
                        </b>
                    </big>
                </td>
            </tr>
        </table>
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

