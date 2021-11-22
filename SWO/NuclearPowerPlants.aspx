<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="NuclearPowerPlants.aspx.vb" Inherits="NuclearPowerPlants" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style86 {
            width: 452px;
        }

        .style88 {
            width: 143px;
        }

        .style89 {
            width: 265px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <AJAX:UpdatePanel ID="AJAXUpdatePanel" runat="server">
        <ContentTemplate>
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        <b>Nuclear Power Plants
                        </b>
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%" align="center">
                <tr>
                    <td align="right">Sub-Types:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlSubType" AutoPostBack="true" Style="background-color: #c2ecde" Width="300px" runat="server">
                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Crystal River – Full ENF" Text="Crystal River – Full ENF"></asp:ListItem>
                            <asp:ListItem Value="Crystal River – Permanently Defueled ENF" Text="Crystal River – Permanently Defueled ENF"></asp:ListItem>
                            <asp:ListItem Value="Farley" Text="Farley"></asp:ListItem>
                            <asp:ListItem Value="Saint Lucie" Text="Saint Lucie"></asp:ListItem>
                            <asp:ListItem Value="Turkey Point" Text="Turkey Point"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right" style="width: 287px">This situation is:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlSituation" Style="background-color: #c2ecde" Width="300px" runat="server">
                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Communications Test" Text="Communications Test"></asp:ListItem>
                            <asp:ListItem Value="Drill" Text="Drill"></asp:ListItem>
                            <asp:ListItem Value="Emergency" Text="Emergency"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right">Worksheet Name:
                    </td>
                    <td align="left" colspan="3">
                        <asp:TextBox ID="txtWorkSheetDescription" Width="889px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">Notification:
                    </td>
                    <td align="left" colspan="3">
                        <asp:DropDownList ID="ddlNotification" DataTextField="Description" DataValueField="IncidentTypeLevelID" Style="background-color: #c2ecde" Width="902px" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table width="100%" align="center">
                <tr>
                    <td style="background: #000 repeat; height: 2px"></td>
                </tr>
            </table>

            <table width="60%" align="left">
                <tr>
                    <td style="width: 205px">1 A.
                            <asp:RadioButton ID="rdoDrill" runat="server" Text="This is a drill" GroupName="Drill" />
                    </td>
                    <td style="width: 300px">B
                                <asp:RadioButton ID="rdoEvent" runat="server" Text="This is An Actual Event" GroupName="Drill" />
                    </td>
                </tr>
            </table>
            <table width="100%" align="left">
                <tr>
                    <td style="width: 205px">2 A. Date:
                                        <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtCSTdate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                        <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                            href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtCSTdate');">
                            <img alt="Calendar Icon"
                                src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                        <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtCSTdate.value = ''"
                            onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                    </td>
                    <td style="width: 205px">B. Contact Time:
                                        <asp:TextBox ID="txtCSTcontactTime" Width="15px"
                                            Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                            onmouseout="UnTip()"></asp:TextBox>
                        <big><b>:</b></big>
                        <asp:TextBox ID="txtCSTcontactTime2" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                        &nbsp;ET
                    </td>
                    <td style="width: 50%">C. Reported By (Name):
                                        <asp:TextBox ID="txtCSTreportedByName" Width="300px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table width="100%" align="left">
                <tr>
                    <td style="width: 205px">3. Verification
                        <asp:checkbox ID="chkStateWatchOffice" runat="server" Text="State Watch Office" GroupName="Verification" />
                        <asp:checkbox ID="chkDOH" runat="server" Text="DOH/BRC" GroupName="Verification" />
                        <asp:checkbox ID="chkStLucieCo" runat="server" Text="St. Lucie Co." GroupName="Verification" />
                        <asp:checkbox ID="chkMartinCo" runat="server" Text="Martin Co." GroupName="Verification" />
                        <asp:checkbox ID="chkMiamiDade" runat="server" Text="Miami-Dade Co." GroupName="Verification" />
                        <asp:checkbox ID="chkMonroeCo" runat="server" Text="Monroe Co." GroupName="Verification" />
                    </td>
                </tr>
            </table>
            <table width="100%" align="left">
                <tr>
                    <td style="width: 50%">4. Message Number:
                                        <asp:TextBox ID="txtmessageNumber" Width="200px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>

            <table cellpadding="0" cellspacing="0" align="left">
                <tr>
                    <td>5. <u><strong>AFFECTED SITE</strong></u>:
                        <asp:CheckBox ID="rdoStLucieUnit1" runat="server" Text="St. Lucie Unit 1"  />
                        <asp:CheckBox ID="rdoStLucieUnit2" runat="server" Text="St. Lucie Unit 2"  />
                        <asp:CheckBox ID="rdoTurkeyPointUnit3" runat="server" Text="Turkey Point Unit 3"  />
                        <asp:CheckBox ID="rdoTurkeyPointUnit4" runat="server" Text="Turkey Point Unit 4"  />
                </tr>
            </table>
            <table width="100%" align="center" style="border-color: #000; border-style: solid; width: 993px">
                <tr>
                    <td align="left" colspan="3">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 400px">6. <u><strong>EMERGENCY CLASSIFICATION</strong></u>:
                                </td>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td>A
                                                <asp:RadioButton ID="rdoNotificationOfUnusualEvent" runat="server" Text="Notification Of Unusual Event" GroupName="EmergencyClassificaiton" /></td>
                                            <td>B
                                                <asp:RadioButton ID="rdoAlert" runat="server" Text="Alert" GroupName="EmergencyClassificaiton" /></td>
                                        </tr>
                                        <tr>
                                            <td>C
                                                <asp:RadioButton ID="rdoSiteEmergencyArea" runat="server" Text="Site Emergency Area" GroupName="EmergencyClassificaiton" /></td>
                                            <td>D
                                                <asp:RadioButton ID="rdoGeneralEmergency" runat="server" Text="GeneralEmergency" GroupName="EmergencyClassificaiton" /></td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%" align="center" style="border-color: #000; border-style: solid; width: 993px">
                <tr>
                    <td align="left" colspan="3">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>7. 
                                    <asp:RadioButton ID="rdoEmergencyDeclaration" runat="server" Text="Emergency Declaration" GroupName="DeclartionTermination" Style="text-decoration: underline" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdoEmergencyTermination" runat="server" Text="Emergency Termination" GroupName="DeclartionTermination" Style="text-decoration: underline" />
                                </td>
                                <td style="width: 250px">&nbsp; &nbsp;Date
                                        <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtCSTdecTermDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtCSTdecTermDate');">
                                        <img alt="Calendar Icon"
                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtCSTdecTermDate.value = ''"
                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                </td>
                                <td>Time
                                        <asp:TextBox ID="txtCSTdecTermTime" Width="15px"
                                            Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                            onmouseout="UnTip()"></asp:TextBox>
                                    <big><b>:</b></big>
                                    <asp:TextBox ID="txtCSTdecTermTime2" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                    &nbsp;ET
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="100%" align="left">
                <tr>
                    <td width="500px">8. <u><strong>REASON FOR EMERGENCY DECLARATION</strong></u>
                    </td>
                    <td>A. EAL Number(s):
                                        <asp:TextBox ID="txtCSTeALNumbers" Width="50px" Style="background-color: #c2ecde" runat="server"></asp:TextBox><small style="font-size: 10px">Alpha / Alpha / Numberic</small>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top">B. Description:<br />
                        <asp:TextBox ID="txtCSTeALDescription" Width="99%" TextMode="MultiLine" Rows="4" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>9. <u><strong>WEATHER DATA</strong></u>:</td>
                    <td style="width: 420px">A. Wind direction from degrees:
                                        <asp:TextBox ID="txtCSTwindDirectionDegrees" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                    </td>
                    <td>B. Downwind Sectors Affected:
                                        <asp:TextBox ID="txtCSTdownwindSectorsAffected" Width="170px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td width="250px">10. <u><strong>RELEASE STATUS</strong></u>:</td>
                    <td>
                        <asp:RadioButton ID="rdoNone" runat="server" Text="None" GroupName="ReleaseStatus" />
                        <asp:RadioButton ID="rdoInProgress" runat="server" Text="In Progress" GroupName="ReleaseStatus" />
                        <asp:RadioButton ID="rdoHasOccured" runat="server" Text="Has occured, but stopped" GroupName="ReleaseStatus" />
                    </td>
                </tr>
            </table>
            <table width="100%" align="center" style="border-color: #000; border-style: solid; width: 993px">
                <tr>
                    <td align="left">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>11. <u><strong>UTILITY RECOMMENDED PROTECTIVE ACTIONS FOR THE PUCLIC:</strong></u>
                                </td>
                            </tr>
                            <tr>
                                <td>A.
                                    <asp:RadioButton ID="rdoNoUtilityRecommended" runat="server" Text="No utility recommended actions at this time" GroupName="Utility" />&nbsp; &nbsp; B. 
                                    <asp:RadioButton ID="rdoUtilityRecommendations" runat="server" Text="The utility recommends the following protective actions for the public" GroupName="Utility" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkPapa1" runat="server" Text="Papa 1" /><br />
                                    <table border="1">
                                        <tr>
                                            <td style="font-size: 9px">Miles</td>
                                            <td style="font-size: 9px">Evacuation Sectors</td>
                                            <td style="font-size: 9px">Shelter Sectors</td>
                                            <td style="font-size: 9px">Monitor & Prepare Sectors</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">0-2</td>
                                            <td style="font-size: 9px">All</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">None</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">2-5</td>
                                            <td style="font-size: 9px">Downwind</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">None</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">5-10</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">All</td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkPapa2" runat="server" Text="Papa 2" /><br />
                                    <table border="1">
                                        <tr>
                                            <td style="font-size: 9px">Miles</td>
                                            <td style="font-size: 9px">Evacuation Sectors</td>
                                            <td style="font-size: 9px">Shelter Sectors</td>
                                            <td style="font-size: 9px">Monitor & Prepare Sectors</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">0-2</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">All</td>
                                            <td style="font-size: 9px">None</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">2-5</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">Downwind</td>
                                            <td style="font-size: 9px">All Remaining</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">5-10</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">All</td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkPapa3" runat="server" Text="Papa 3" /><br />
                                    <table border="1">
                                        <tr>
                                            <td style="font-size: 9px">Miles</td>
                                            <td style="font-size: 9px">Evacuation Sectors</td>
                                            <td style="font-size: 9px">Shelter Sectors</td>
                                            <td style="font-size: 9px">Monitor & Prepare Sectors</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">0-2</td>
                                            <td style="font-size: 9px">All</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">None</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">2-5</td>
                                            <td style="font-size: 9px">Downwind</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">All Remaining</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">5-10</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">Downwind</td>
                                            <td style="font-size: 9px">All Remaining</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkDelta1" runat="server" Text="Delta 1" /><br />
                                    <table border="1">
                                        <tr>
                                            <td style="font-size: 9px">Miles</td>
                                            <td style="font-size: 9px">Evacuation Sectors</td>
                                            <td style="font-size: 9px">Shelter Sectors</td>
                                            <td style="font-size: 9px">Monitor & Prepare Sectors</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">0-2</td>
                                            <td style="font-size: 9px">All</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">None</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">2-5</td>
                                            <td style="font-size: 9px">Downwind</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">All Remaining</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">5-10</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">All</td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkDelta2" runat="server" Text="Delta 2" /><br />
                                    <table border="1">
                                        <tr>
                                            <td style="font-size: 9px">Miles</td>
                                            <td style="font-size: 9px">Evacuation Sectors</td>
                                            <td style="font-size: 9px">Shelter Sectors</td>
                                            <td style="font-size: 9px">Monitor & Prepare Sectors</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">0-2</td>
                                            <td style="font-size: 9px">All</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">None</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">2-5</td>
                                            <td style="font-size: 9px">Downwind</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">All Remaining</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">5-10</td>
                                            <td style="font-size: 9px">Downwind</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">All Remaining</td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkDelta3" runat="server" Text="Delta 3" /><br />
                                    <table border="1">
                                        <tr>
                                            <td style="font-size: 9px">Miles</td>
                                            <td style="font-size: 9px">Evacuation Sectors</td>
                                            <td style="font-size: 9px">Shelter Sectors</td>
                                            <td style="font-size: 9px">Monitor & Prepare Sectors</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">0-2</td>
                                            <td style="font-size: 9px">All</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">None</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">2-5</td>
                                            <td style="font-size: 9px">Downwind</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">All Remaining</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">5-10</td>
                                            <td style="font-size: 9px">Downwind</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">All Remaining</td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 9px">>10</td>
                                            <td style="font-size: 9px">Downwind</td>
                                            <td style="font-size: 9px">None</td>
                                            <td style="font-size: 9px">All Remaining</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkPotassium" runat="server" Text="Consider issuance of potassium iodide (KI)" /><br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <hr width="98%" style="border: none; border-top: 1px dashed #000; color: #fff; background-color: #fff; height: 1px; width: 98%;" />
            <strong>The following information shall be provided on a 60 minute update notificaitions from the TSC or EOF</strong>

            <table width="100%">
                <tr>
                    <td colspan="2">12. <strong><u>Plant Conditions</u></strong> Notify Plant Conditions? <asp:RadioButton ID="rdoShowPlantConditionsYes" runat="server" Text="Yes" GroupName="ShowPlantConditions" /><asp:RadioButton ID="rdoShowPlantConditionsNo" runat="server" Text="No" Checked="true" GroupName="ShowPlantConditions" /></td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;A. Reactor Shutdown?<asp:RadioButton ID="rdoReactionShutdownYes" runat="server" Text="Yes" GroupName="ReactorShutdown" /><asp:RadioButton ID="rdoReactorShutdownNo" runat="server" Text="No" GroupName="ReactorShutdown" /></td>
                    <td>&nbsp;&nbsp;B. Core Adequately Cooled?<asp:RadioButton ID="rdoCoreAdequatelyCooledYes" runat="server" Text="Yes" GroupName="CoreAdequatelyCooled" /><asp:RadioButton ID="rdoCoreAdequatelyCooledNo" runat="server" Text="No" GroupName="CoreAdequatelyCooled" /></td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;C. Containment Intact?<asp:RadioButton ID="rdoContainmentIntactYes" runat="server" Text="Yes" GroupName="ContainmentIntact" /><asp:RadioButton ID="rdoContainmentIntactNo" runat="server" Text="No" GroupName="ContainmentIntact" /></td>
                    <td>&nbsp;&nbsp;D. Core Condition<asp:RadioButton ID="rdoCoreConditionStable" runat="server" Text="Stable" GroupName="CoreCondition" /><asp:RadioButton ID="rdoCoreConditionDegrading" runat="server" Text="Degrading" GroupName="CoreCondition" /></td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>13. <u><strong>WEATHER DATA</strong></u>:</td>
                    <td style="width: 420px">A. Wind Speed:
                                        <asp:TextBox ID="txtCST13A" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox> mph
                    </td>
                    <td>B. Stability Class:
                                        <asp:TextBox ID="txtCST13B" Width="170px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td colspan="2">14. <strong><u>RELEASE SIGNIFCANCE CATEGORY (at the Site Boundary)</u></strong></td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;A.
                        <asp:RadioButton ID="rdoUnderEvaluation" runat="server" Text="Under Evaluation" GroupName="ReleaseSignificance" /></td>
                    <td>&nbsp;&nbsp;B.
                        <asp:RadioButton ID="rdoReleaseWithinNormal" runat="server" Text="Release within Normal Operating Limits" GroupName="ReleaseSignificance" /></td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;C.
                        <asp:RadioButton ID="rdoNonSignificant" runat="server" Text="Non-Significant (Fraction of PAG Range)" GroupName="ReleaseSignificance" /></td>
                    <td>&nbsp;&nbsp;D.
                        <asp:RadioButton ID="rdoPAGRang" runat="server" Text="PAG Range (Protective Actions Required)" GroupName="ReleaseSignificance" /></td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;E.
                        <asp:RadioButton ID="rdoLiquidRelease" runat="server" Text="Liquid release (No Actions Required)" GroupName="ReleaseSignificance" /></td>
                    <td></td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>15. <strong><u>ADDITIONAL RELEASE INFORMATION</u></strong></td>
                    <td>
                        <asp:RadioButton ID="rdoAdditionalInformationNotApplicable" runat="server" Text="Not Applicable (Go to item 16)" GroupName="AdditionalInformation" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rdoAdditionalInformationDose" runat="server" Text="Dose Protection" GroupName="AdditionalInformation" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td colspan="2">Distance Projected Total Dose for duration of release:</td>
                </tr>
                <tr>
                    <td width="100%">
                        <table width="100%">
                            <tr>
                                <td></td>
                                <td>TEDE</td>
                                <td>CDE Thyroid</td>
                            </tr>
                            <tr>
                                <td>1 mile(site boundary)</td>
                                <td>A.
                                    <asp:TextBox ID="txtOneMileTEDE" Style="background-color: #c2ecde" runat="server" TabIndex="101"/></td>
                                <td>E.
                                    <asp:TextBox ID="txtOneMileCDE" Style="background-color: #c2ecde" runat="server" TabIndex="105" /></td>
                            </tr>
                            <tr>
                                <td>2 miles</td>
                                <td>B.
                                    <asp:TextBox ID="txtTwoMileTEDE" Style="background-color: #c2ecde" runat="server" TabIndex="102"/></td>
                                <td>F.
                                    <asp:TextBox ID="txtTwoMileCDE" Style="background-color: #c2ecde" runat="server" TabIndex="106" /></td>
                            </tr>
                            <tr>
                                <td>5 Miles</td>
                                <td>C.
                                    <asp:TextBox ID="txtFiveMileTEDE" Style="background-color: #c2ecde" runat="server" TabIndex="103" /></td>
                                <td>G.
                                    <asp:TextBox ID="txtFiveMileCDE" Style="background-color: #c2ecde" runat="server" TabIndex="107" /></td>
                            </tr>
                            <tr>
                                <td>10 Miles</td>
                                <td>D.
                                    <asp:TextBox ID="txtTenMileTEDE" Style="background-color: #c2ecde" runat="server" TabIndex="104" /></td>
                                <td>H.
                                    <asp:TextBox ID="txtTenMileCDE" Style="background-color: #c2ecde" runat="server" TabIndex="108" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td style="width: 200px">16. <b><u>Message Received By:</u></b>
                    </td>
                    <td style="width: 380px">
                                        <asp:TextBox ID="txtCST15Name" Width="295px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                    </td>
                    <td style="width: 200px">Date:
                                        <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtCST15Date" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                        <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                            href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtCST15Date');">
                            <img alt="Calendar Icon"
                                src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                        <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtCST15Date.value = ''"
                            onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                    </td>
                    <td>Time:
                                        <asp:TextBox ID="txtCST15Time" Width="15px"
                                            Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                            onmouseout="UnTip()"></asp:TextBox>
                        <big><b>:</b></big>
                        <asp:TextBox ID="txtCST15Time2" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                        &nbsp;ET
                    </td>
                </tr>
            </table>
            




            <%--<table width="100%" align="center">
                <tr>
                    <td align="right">Sub-Types:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlSubType" AutoPostBack="true" Style="background-color: #c2ecde" Width="300px" runat="server">
                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Crystal River – Full ENF" Text="Crystal River – Full ENF"></asp:ListItem>
                            <asp:ListItem Value="Crystal River – Permanently Defueled ENF" Text="Crystal River – Permanently Defueled ENF"></asp:ListItem>
                            <asp:ListItem Value="Farley" Text="Farley"></asp:ListItem>
                            <asp:ListItem Value="Saint Lucie" Text="Saint Lucie"></asp:ListItem>
                            <asp:ListItem Value="Turkey Point" Text="Turkey Point"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right" style="width: 287px">This situation is:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlSituation" Style="background-color: #c2ecde" Width="300px" runat="server">
                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Communications Test" Text="Communications Test"></asp:ListItem>
                            <asp:ListItem Value="Drill" Text="Drill"></asp:ListItem>
                            <asp:ListItem Value="Emergency" Text="Emergency"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right">Worksheet Name:
                    </td>
                    <td align="left" colspan="3">
                        <asp:TextBox ID="txtWorkSheetDescription" Width="889px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">Notification:
                    </td>
                    <td align="left" colspan="3">
                        <asp:DropDownList ID="ddlNotification" DataTextField="Description" DataValueField="IncidentTypeLevelID" Style="background-color: #c2ecde" Width="902px" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table width="100%" align="center">
                <tr>
                    <td style="background: #000 repeat; height: 2px"></td>
                </tr>
            </table>


            <asp:Panel ID="pnlShowFlorida" runat="server" Visible="false">

                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>1. Please Select one:
                                       <asp:DropDownList ID="ddlCSTselectOne"
                                           Style="background-color: #c2ecde; margin-left: 4px;" Width="175px" runat="server">
                                           <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                           <asp:ListItem Value="This is a DRILL" Text="This is a DRILL"></asp:ListItem>
                                           <asp:ListItem Value="This is an EMERGENCY" Text="This is an EMERGENCY"></asp:ListItem>
                                       </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table cellpadding="0" cellspacing="0" border="3" style="border-color: #000; border-style: solid; width: 993px">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 205px">2 A. Date:
                                        <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtCSTdate2" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtCSTdate');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtCSTdate.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                </td>
                                                <td style="width: 205px">2 B. Contact Time:
                                        <asp:TextBox ID="txtCSTcontactTime12" Width="15px"
                                            Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                            onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtCSTcontactTime22" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET
                                                </td>
                                                <td style="width: 50%">2 C. Reported By (Name):
                                        <asp:TextBox ID="txtCSTreportedByName2" Width="300px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 245px">2 D. Message Number:
                                        <asp:TextBox ID="txtCSTmessageNumber" Width="65px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td style="width: 305px">2 E. Reported From:
                                        <asp:DropDownList ID="ddlCSTreportedFrom"
                                            Style="background-color: #c2ecde; margin-left: 4px;" Width="125px" runat="server">
                                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="TSC" Text="TSC"></asp:ListItem>
                                            <asp:ListItem Value="EOF" Text="EOF"></asp:ListItem>
                                            <asp:ListItem Value="Control Room" Text="Control Room"></asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                                <td style="width: 435px">2 F. [Select One]
                                        <asp:DropDownList ID="ddlCSTfSelectOne"
                                            Style="background-color: #c2ecde; margin-left: 4px;" Width="210px" runat="server">
                                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="Initial/New Classification" Text="Initial/New Classification"></asp:ListItem>
                                            <asp:ListItem Value="Update Notification" Text="Update Notification"></asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 325px">3. Site:
                                        <asp:DropDownList ID="ddlCSTsite"
                                            Style="background-color: #c2ecde; margin-left: 4px;" Width="175px" runat="server">
                                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="A. Crystal River Unit 3" Text="A. Crystal River Unit 3"></asp:ListItem>
                                            <asp:ListItem Value="B. St. Lucie Unit 1" Text="B. St. Lucie Unit 1"></asp:ListItem>
                                            <asp:ListItem Value="C. St. Lucie Unit 2" Text="C. St. Lucie Unit 2"></asp:ListItem>
                                            <asp:ListItem Value="D. Turkey Point Unit 3" Text="D. Turkey Point Unit 3"></asp:ListItem>
                                            <asp:ListItem Value="E. Turkey Point Unit 4" Text="E. Turkey Point Unit 4"></asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                                <td>4. Emergency Classification:
                                        <asp:DropDownList ID="ddlCSTemergencyClassification"
                                            Style="background-color: #c2ecde; margin-left: 4px;" Width="210px" runat="server">
                                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="A. Notification of Unusual Event" Text="A. Notification of Unusual Event"></asp:ListItem>
                                            <asp:ListItem Value="B. Alert" Text="B. Alert"></asp:ListItem>
                                            <asp:ListItem Value="C. Site Area Emergency" Text="C. Site Area Emergency"></asp:ListItem>
                                            <asp:ListItem Value="D. General Emergency" Text="D. General Emergency"></asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 325px">5. [Select One]
                                        <asp:DropDownList ID="ddlCSTdecTermSelectOne"
                                            Style="background-color: #c2ecde; margin-left: 4px;" Width="180px" runat="server">
                                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="A. Emergency Declaration" Text="A. Emergency Declaration"></asp:ListItem>
                                            <asp:ListItem Value="B. Emergency Termination" Text="B. Emergency Termination"></asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                                <td style="width: 250px">5. Date
                                        <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtCSTdecTermDate2" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtCSTdecTermDate');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtCSTdecTermDate.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                </td>
                                                <td>5. Time
                                        <asp:TextBox ID="txtCSTdecTermTime12" Width="15px"
                                            Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                            onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtCSTdecTermTime22" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>6. Reason for Emergency Declaration
                                        <asp:DropDownList ID="ddlCSTdecTermReason"
                                            Style="background-color: #c2ecde; margin-left: 4px;" Width="175px" runat="server">
                                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="A. EAL Number(s)" Text="A. EAL Number(s)"></asp:ListItem>
                                            <asp:ListItem Value="B. Description" Text="B. Description"></asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                                <td>6. EAL Number(s):
                                        <asp:TextBox ID="txtCSTeALNumbers2" Width="50px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td>6. Description:
                                        <asp:TextBox ID="txtCSTeALDescription2" Width="230px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 443px">7. Additional Information:
                                        <asp:DropDownList ID="ddlCSTeALai"
                                            Style="background-color: #c2ecde; margin-left: 4px;" Width="175px" runat="server">
                                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="A. None" Text="A. None"></asp:ListItem>
                                            <asp:ListItem Value="B. Description" Text="B. Description"></asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                                <td>7. Description
                                        <asp:TextBox ID="txtCSTeALaiDescription" Width="425px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 143px">8. <b>Weather Data</b>
                                                </td>
                                                <td style="width: 420px">8. A. Wind direction from degrees:
                                        <asp:TextBox ID="txtCSTwindDirectionDegrees2" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td>8. B. Downwind Sectors Affected:
                                        <asp:TextBox ID="txtCSTdownwindSectorsAffected2" Width="170px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>9. Release Status
                                        <asp:DropDownList ID="ddlCSTreleaseStatus"
                                            Style="background-color: #c2ecde; margin-left: 4px;" Width="185px" runat="server">
                                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="A. None (Go to item 11)" Text="A. None (Go to item 11)"></asp:ListItem>
                                            <asp:ListItem Value="B. In Progress" Text="B. In Progress"></asp:ListItem>
                                            <asp:ListItem Value="C. Has occurred, but stopped" Text="C. Has occurred, but stopped"></asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                                <td>10. Release Significance at Site Boundary:
                                        <asp:DropDownList ID="ddlCSTsigCatSiteBoundary"
                                            Style="background-color: #c2ecde; margin-left: 4px;" Width="365px" runat="server">
                                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="A. Under Evaluation" Text="A. Under Evaluation"></asp:ListItem>
                                            <asp:ListItem Value="B. Release is within normal operating limits" Text="B. Release is within normal operating limits"></asp:ListItem>
                                            <asp:ListItem Value="C. Non-significant (fraction of protective action guide range)" Text="C. Non-significant (fraction of protective action guide range)"></asp:ListItem>
                                            <asp:ListItem Value="D. Protective action guide range" Text="D. Protective action guide range"></asp:ListItem>
                                            <asp:ListItem Value="E. Liquid release (no actions required)" Text="E. Liquid release (no actions required)"></asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table cellpadding="0" cellspacing="0" border="3" style="border-color: #000; border-style: solid; width: 993px">
                                <tr>
                                    <td>
                                        <table width="100%" align="center">
                                            <tr>
                                                <td>11. Utility Recommended Protective Actions:
                                        <asp:DropDownList ID="ddlCSTutilRecProtAct"
                                            Style="background-color: #c2ecde; margin-left: 4px;" Width="350px" runat="server">
                                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="A. No utility recommended actions at this time." Text="A. No utility recommended actions at this time."></asp:ListItem>
                                            <asp:ListItem Value="B. Utility recommedns the following Protective Actions" Text="B. Utility recommedns the following Protective Actions"></asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%" align="center">
                                            <tr>
                                                <td>Evacuate Zones
                                        <asp:TextBox ID="txtCSTevacuateZones" Width="350px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td>Shelter Zones
                                        <asp:TextBox ID="txtCSTshelterZones" Width="387px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%" align="center">
                                            <tr>
                                                <td style="width: 124px">&nbsp;
                                                </td>
                                                <td style="width: 270px">Evacuate Sectors
                                                </td>
                                                <td style="width: 280px">Shelter Sectors
                                                </td>
                                                <td>Monitor & Prepare Sectors
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%" align="center">
                                            <tr>
                                                <td style="width: 110px">0-2 Miles
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCST02MilesEvacSect" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCST02MilesShelterSect" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCST02MilesNoActtionSect" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%" align="center">
                                            <tr>
                                                <td style="width: 110px">2-5 Miles 
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCST25MilesEvacSect" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCST25MilesShelterSect" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCST25MilesNoActtionSect" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%" align="center">
                                            <tr>
                                                <td style="width: 110px">5-10 Miles
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCST510MilesEvacSect" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCST510MilesShelterSect" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCST510MilesNoActtionSect" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table cellpadding="0" cellspacing="0" border="3" style="border-color: #000; border-style: solid; width: 993px">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 182px">12. <b>Plant Conditions</b>
                                                </td>
                                                <td style="width: 360px">12. A. Reactor Shutdown:
                                        <asp:DropDownList ID="ddlCST12A"
                                            Style="background-color: #c2ecde; margin-left: 4px;" Width="150px" runat="server">
                                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="No" Text="No"></asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                                <td>12. B. Core Adequately Cooled:
                                        <asp:DropDownList ID="ddlCST12B"
                                            Style="background-color: #c2ecde; margin-left: 4px;" Width="206px" runat="server">
                                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="No" Text="No"></asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 543px">12. C. Containment Intact:
                                        <asp:DropDownList ID="ddlCST12C"
                                            Style="background-color: #c2ecde; margin-left: 4px;" Width="150px" runat="server">
                                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="No" Text="No"></asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                                <td>12. D. Core Condition:
                                        <asp:DropDownList ID="ddlCST12D"
                                            Style="background-color: #c2ecde; margin-left: 4px;" Width="150px" runat="server">
                                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="Stable" Text="Stable"></asp:ListItem>
                                            <asp:ListItem Value="Degrading" Text="Degrading"></asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 182px">13. <b>Weather Data</b>
                                                </td>
                                                <td style="width: 361px">13. A. Wind Speed (MPH):
                                        <asp:TextBox ID="txtCST13A2" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td>13. B. Stability Class:
                                        <asp:TextBox ID="txtCST13B2" Width="275px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table cellpadding="0" cellspacing="0" border="3" style="border-color: #000; border-style: solid; width: 993px">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>14 A. <b>Additional Release Information</b>
                                                    <asp:DropDownList ID="ddlCST14A" AutoPostBack="true"
                                                        Style="background-color: #c2ecde; margin-left: 4px;" Width="350px" runat="server">
                                                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="Not Applicable (go to item 15)" Text="Not Applicable (go to item 15)"></asp:ListItem>
                                                        <asp:ListItem Value="As Below" Text="As Below"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="pnlCST14Show" runat="server" Visible="false">
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td style="width: 182px">Distance
                                                    </td>
                                                    <td>Projected Thyroid Dose (CDE) for 
                                        <asp:TextBox ID="txtCSTProjThyroidDose" Width="35px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                        hour(s)
                                                    </td>
                                                    <td>Projected Total Dose (TEDE) for
                                        <asp:TextBox ID="txtCSTProjTotalDose" Width="35px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                        hour(s)
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td style="width: 182px">1 Mile (Site Boundary)
                                                    </td>
                                                    <td>B. 
                                        <asp:TextBox ID="txtCST14B" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                        mrem
                                                    </td>
                                                    <td>C. 
                                        <asp:TextBox ID="txtCST14C" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                        mrem
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td style="width: 182px">2 Miles
                                                    </td>
                                                    <td>D. 
                                        <asp:TextBox ID="txtCST14D" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                        mrem
                                                    </td>
                                                    <td>E. 
                                        <asp:TextBox ID="txtCST14E" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                        mrem
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td style="width: 182px">5 Miles
                                                    </td>
                                                    <td>F. 
                                        <asp:TextBox ID="txtCST14F" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                        mrem
                                                    </td>
                                                    <td>G. 
                                        <asp:TextBox ID="txtCST14G" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                        mrem
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td style="width: 182px">10 Miles
                                                    </td>
                                                    <td>H. 
                                        <asp:TextBox ID="txtCST14H" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                        mrem
                                                    </td>
                                                    <td>I. 
                                        <asp:TextBox ID="txtCST14I" Width="150px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                        mrem
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 200px">16. <b><u>Message Received By:</u></b>
                                                </td>
                                                <td style="width: 380px">
                                        <asp:TextBox ID="txtCST15Name22" Width="295px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td style="width: 200px">Date:
                                        <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtCST15Date22" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtCST15Date');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtCST15Date.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                </td>
                                                <td>Time:
                                        <asp:TextBox ID="txtCST15Time222" Width="15px"
                                            Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                            onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtCST15Time12" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>SWO User Comments:
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:TextBox ID="txtCSTuserComments" Width="820px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="pnlShowAlabama" runat="server" Visible="false">
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 250px">1. Select one:
                                       <asp:DropDownList ID="ddlFar1SelectOne"
                                           Style="background-color: #c2ecde; margin-left: 4px;" Width="125px" runat="server">
                                           <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                           <asp:ListItem Value="A. DRILL" Text="A. DRILL"></asp:ListItem>
                                           <asp:ListItem Value="B. Actual Event" Text="B. Actual Event"></asp:ListItem>
                                       </asp:DropDownList>
                                                </td>
                                                <td>Message #:
                                        <asp:TextBox ID="txtFar1MessageNumber" Width="240px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 250px">2. Select one:
                                       <asp:DropDownList ID="ddlFar2SelectOne"
                                           Style="background-color: #c2ecde; margin-left: 4px;" Width="125px" runat="server">
                                           <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                           <asp:ListItem Value="A. Initial" Text="A. Initial"></asp:ListItem>
                                           <asp:ListItem Value="B. Follow-Up" Text="B. Follow-Up"></asp:ListItem>
                                       </asp:DropDownList>
                                                </td>
                                                <td style="width: 225px">Notification Time:
                                        <asp:TextBox ID="txtFar2NotificationTime" Width="15px"
                                            Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                            onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtFar2NotificationTime2" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET
                                                </td>
                                                <td style="width: 200px">Date:
                                        <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtFar2NotificationDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtFar2NotificationDate');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtFar2NotificationDate.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                </td>
                                                <td>Authentication #:
                                        <asp:TextBox ID="txtFar2AuthenticationNumber" Width="185px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 500px">3. Site:
                                        <asp:TextBox ID="txtFar3Site" Width="430px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td>Confirmation Phone #:
                                        <asp:TextBox ID="txtFar3ConfirmationPhoneNumber" Width="314px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table cellpadding="0" cellspacing="0" border="3" style="border-color: #000; border-style: solid; width: 993px">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 535px">4. <b>Emergency Classification</b>:
                                       <asp:DropDownList ID="ddlFar4EmergencyClassification"
                                           Style="background-color: #c2ecde; margin-left: 4px;" Width="155px" runat="server">
                                           <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                           <asp:ListItem Value="Unusual Event" Text="Unusual Event"></asp:ListItem>
                                           <asp:ListItem Value="B. Alert" Text="B. Alert"></asp:ListItem>
                                           <asp:ListItem Value="C. Site Area Emergency" Text="C. Site Area Emergency"></asp:ListItem>
                                           <asp:ListItem Value="D. General Emergency" Text="D. General Emergency"></asp:ListItem>
                                       </asp:DropDownList>
                                                </td>
                                                <td>Based on EAL #:
                                        <asp:TextBox ID="txtFar4BasedEALnumber" Width="313px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>EAL Description:
                                       <asp:TextBox ID="txtFar4EALdescription" Width="847px" Style="background-color: #c2ecde" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>5. <b>Protective Action Recommendations</b>:
                                       <asp:CheckBox ID="cbxFar5a" runat="server" Text="5 A. None" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 164px">
                                                    <asp:CheckBox ID="cbxFar5b" runat="server" Text="5. B. Evacuate" />
                                                </td>
                                                <td>5. B. Evacuate Description:
                                        <asp:TextBox ID="txtFar5bText" Width="612px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 179px">
                                                    <asp:CheckBox ID="cbxFar5c" runat="server" Text="5. C. Shelter" />
                                                </td>
                                                <td>5. C. Shelter Description:
                                        <asp:TextBox ID="txtFar5cText" Width="612px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="cbxFar5d" runat="server" Text="5. D. Consider the use of KI in accordance with state plans and policy." />
                                                </td>

                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 191px">
                                                    <asp:CheckBox ID="cbxFar5e" runat="server" Text="5. E. Other" />
                                                </td>
                                                <td>5. E. Other Description:
                                        <asp:TextBox ID="txtFar5eText" Width="612px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 535px">6. <b>Emergency Release</b>:
                                       <asp:DropDownList ID="ddlFar6EmergencyRelease"
                                           Style="background-color: #c2ecde; margin-left: 4px;" Width="155px" runat="server">
                                           <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                           <asp:ListItem Value="None" Text="None"></asp:ListItem>
                                           <asp:ListItem Value="Is Occuring" Text="Is Occuring"></asp:ListItem>
                                           <asp:ListItem Value="Has Occurred" Text="Has Occurred"></asp:ListItem>
                                       </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 500px">7. Release Significance:
                                       <asp:DropDownList ID="ddlFar7ReleaseSignificance"
                                           Style="background-color: #c2ecde; margin-left: 4px;" Width="225px" runat="server">
                                           <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                           <asp:ListItem Value="A. Not Applicable" Text="A. Not Applicable"></asp:ListItem>
                                           <asp:ListItem Value="B. Within normal operating limits" Text="B. Within normal operating limits"></asp:ListItem>
                                           <asp:ListItem Value="C. Above normal operating limits" Text="C. Above normal operating limits"></asp:ListItem>
                                           <asp:ListItem Value="D. Under evaluation" Text="D. Under evaluation"></asp:ListItem>
                                       </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 500px">8. Event Prognosis:
                                       <asp:DropDownList ID="ddlFar8EventPrognosis"
                                           Style="background-color: #c2ecde; margin-left: 4px;" Width="225px" runat="server">
                                           <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                           <asp:ListItem Value="A. Improving" Text="A. Improving"></asp:ListItem>
                                           <asp:ListItem Value="B. Stable" Text="B. Stable"></asp:ListItem>
                                           <asp:ListItem Value="C. Degrading" Text="C. Degrading"></asp:ListItem>
                                       </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 200px">9. Meterological Data:
                                                </td>
                                                <td style="width: 350px">Wind direction from
                                        <asp:TextBox ID="txtFar9WindDirectDegrees" Width="75px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                    degrees:
                                                </td>
                                                <td>Wind Speed
                                        <asp:TextBox ID="txtFar9WindSpeed" Width="75px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                    (mph)
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 285px">&nbsp;
                                                </td>
                                                <td style="width: 320px">Precipitation:
                                        <asp:TextBox ID="txtFar9Precipitation" Width="75px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td style="width: 500px">Stability Class:
                                       <asp:DropDownList ID="ddlFar9StabilityClass"
                                           Style="background-color: #c2ecde; margin-left: 4px;" Width="225px" runat="server">
                                           <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                           <asp:ListItem Value="A" Text="A"></asp:ListItem>
                                           <asp:ListItem Value="B" Text="B"></asp:ListItem>
                                           <asp:ListItem Value="C" Text="C"></asp:ListItem>
                                           <asp:ListItem Value="D" Text="D"></asp:ListItem>
                                           <asp:ListItem Value="E" Text="E"></asp:ListItem>
                                           <asp:ListItem Value="F" Text="F"></asp:ListItem>
                                           <asp:ListItem Value="G" Text="G"></asp:ListItem>
                                       </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 350px">10. [Select One]:
                                       <asp:DropDownList ID="ddlFar10Select1"
                                           Style="background-color: #c2ecde; margin-left: 4px;" Width="125px" runat="server">
                                           <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                           <asp:ListItem Value="A. Declaration" Text="A. Declaration"></asp:ListItem>
                                           <asp:ListItem Value="B. Termination" Text="B. Termination"></asp:ListItem>
                                       </asp:DropDownList>
                                                </td>
                                                <td style="width: 205px">10 Time:
                                        <asp:TextBox ID="txtFar10Time" Width="15px"
                                            Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                            onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtFar10Time2" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET
                                                </td>
                                                <td style="width: 200px">10 Date:
                                        <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtFar10Date" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtFar10Date');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtFar10Date.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 500px">11. Affected Units
                                       <asp:DropDownList ID="ddlFar11AffectedUnits"
                                           Style="background-color: #c2ecde; margin-left: 4px;" Width="225px" runat="server">
                                           <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                           <asp:ListItem Value="Unit 1" Text="Unit 1"></asp:ListItem>
                                           <asp:ListItem Value="Unit 2" Text="Unit 2"></asp:ListItem>
                                           <asp:ListItem Value="All" Text="All"></asp:ListItem>
                                       </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 400px">12. Unit Status:
                                                </td>
                                                <td style="width: 255px">12. A. Unit 1 
                                        <asp:TextBox ID="txtFar12AUnitPower" Width="75px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                    % power
                                                </td>
                                                <td style="width: 155px">Time:
                                        <asp:TextBox ID="txtFar12ATime" Width="15px"
                                            Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                            onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtFar12ATime2" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET
                                                </td>
                                                <td style="width: 200px">Date:
                                        <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtFar12ADate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtFar12ADate');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtFar12ADate.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 400px">
                                                    <font size="2">(Unaffected Unit(s) Status Not Required for Initial Notifications)</font>
                                                </td>
                                                <td style="width: 255px">12. B. Unit 2 
                                        <asp:TextBox ID="txtFar12BUnitPower" Width="75px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                    % power
                                                </td>
                                                <td style="width: 155px">Time:
                                        <asp:TextBox ID="txtFar12BTime" Width="15px"
                                            Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                            onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtFar12BTime2" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET
                                                </td>
                                                <td style="width: 200px">Date:
                                        <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtFar12BDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtFar12BDate');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtFar12BDate.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>13. Remarks:
                                       <asp:TextBox ID="txtFar13Remarks" Width="870px" Style="background-color: #c2ecde" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td style="background: #000 repeat; height: 2px"></td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="center">
                            <big><b>Information(Lines 14-16 not required for initial Notifications)</b></big>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <b>Emergency Release Data.  Not required if line 6 A is selected.</b>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 465px">14. Release Characterization
                                        <asp:DropDownList ID="ddlFar14ReleaseChar"
                                            Style="background-color: #c2ecde; margin-left: 4px;" Width="185px" runat="server">
                                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="A. Elevated" Text="A. Elevated"></asp:ListItem>
                                            <asp:ListItem Value="B. Mixed" Text="B. Mixed"></asp:ListItem>
                                            <asp:ListItem Value="C. Ground" Text="C. Ground"></asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                                <td>Units:
                                        <asp:DropDownList ID="ddlFar14Units"
                                            Style="background-color: #c2ecde; margin-left: 4px;" Width="185px" runat="server">
                                            <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="A. Ci" Text="A. Ci"></asp:ListItem>
                                            <asp:ListItem Value="B. Ci/sec" Text="B. Ci/sec"></asp:ListItem>
                                            <asp:ListItem Value="C. uCi/Sec" Text="C. uCi/Sec"></asp:ListItem>
                                        </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 69px">&nbsp;
                                                </td>
                                                <td style="width: 100px">Magnitude:
                                                </td>
                                                <td style="width: 320px">Noble Gasses:
                                        <asp:TextBox ID="txtFar14NobleGasses" Width="100px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td style="width: 250px">Iodines:
                                        <asp:TextBox ID="txtFar14Iodines" Width="100px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td style="width: 285px">Particulautes:
                                        <asp:TextBox ID="txtFar14Particulautes" Width="100px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td style="width: 250px">Other:
                                        <asp:TextBox ID="txtFar14Other" Width="115px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 69px">&nbsp;
                                                </td>
                                                <td style="width: 45px">Form:
                                                </td>
                                                <td style="width: 125px">
                                                    <asp:CheckBox ID="cbxFar14Aairborne" runat="server" Text="A. " />
                                                    Airborne:
                                                </td>
                                                <td style="width: 180px">Start Time:
                                        <asp:TextBox ID="txtFar14AstartTime" Width="15px"
                                            Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                            onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtFar14AstartTime2" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET
                                                </td>
                                                <td style="width: 210px">Date:
                                        <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtFar14AstartDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtFar14AstartDate');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtFar14AstartDate.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                </td>
                                                <td style="width: 180px">Stop Time:
                                        <asp:TextBox ID="txtFar14AstopTime" Width="15px"
                                            Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                            onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtFar14AstopTime2" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET
                                                </td>
                                                <td style="width: 210px">Date:
                                        <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtFar14AstopDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtFar14AstopDate');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtFar14AstopDate.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 69px">&nbsp;
                                                </td>
                                                <td style="width: 47px">&nbsp;
                                                </td>
                                                <td style="width: 125px">
                                                    <asp:CheckBox ID="cbxFar14Bliquid" runat="server" Text="B. " />
                                                    Liquid:
                                                </td>
                                                <td style="width: 181px">Start Time:
                                        <asp:TextBox ID="txtFar14BstartTime" Width="15px"
                                            Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                            onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtFar14BstartTime2" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET
                                                </td>
                                                <td style="width: 210px">Date:
                                        <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtFar14BstartDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtFar14BstartDate');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtFar14BstartDate.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                </td>
                                                <td style="width: 180px">Stop Time:
                                        <asp:TextBox ID="txtFar14BstopTime" Width="15px"
                                            Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                            onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtFar14BstopTime2" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET
                                                </td>
                                                <td style="width: 210px">Date:
                                        <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtFar14BendDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtFar14BendDate');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtFar14BendDate.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 235px">15. Projection Parameters:
                                                </td>
                                                <td style="width: 345px">Projection Period:
                                        <asp:TextBox ID="txtFar15ProjectionPeriod" Width="115px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                    (hours)
                                                </td>
                                                <td>Estimated Release Duration:
                                        <asp:TextBox ID="txtFar15EstimatedReleaseDuration" Width="115px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                    (hours)
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 62px">&nbsp;
                                                </td>
                                                <td style="width: 172px">
                                                    <font size="2">Projection Performed:</font>
                                                </td>
                                                <td style="width: 150px">Time:
                                        <asp:TextBox ID="txtFar15ProjectionPerformedTime" Width="15px"
                                            Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                            onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtFar15ProjectionPerformedTime2" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET
                                                </td>
                                                <td style="width: 200px">Date:
                                        <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtFar15ProjectionPerformedDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtFar15ProjectionPerformedDate');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtFar15ProjectionPerformedDate.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                </td>
                                                <td>Accident Type:
                                        <asp:TextBox ID="txtFar15AccidentType" Width="115px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 235px">16. Projected Dose:
                                                </td>
                                                <td style="width: 235px">
                                                    <u>Distance</u>
                                                </td>
                                                <td style="width: 235px">
                                                    <u>TEDE(mrem)</u>
                                                </td>
                                                <td>
                                                    <u>Adult Thyroid CDE(mrem)</u>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 235px">&nbsp;
                                                </td>
                                                <td style="width: 185px">Site boundary
                                                </td>
                                                <td style="width: 283px">
                                                    <asp:TextBox ID="txtFar16SiteBoundaryTEDE" Width="180px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFar16SiteBoundaryAdultThyroidCDE" Width="180px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 235px">&nbsp;
                                                </td>
                                                <td style="width: 185px">2 Miles
                                                </td>
                                                <td style="width: 283px">
                                                    <asp:TextBox ID="txtFar16TwoMilesTEDE" Width="180px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFar16TwoMilesAdultThyroidCDE" Width="180px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 235px">&nbsp;
                                                </td>
                                                <td style="width: 185px">5 Miles
                                                </td>
                                                <td style="width: 283px">
                                                    <asp:TextBox ID="txtFar16FiveMilesTEDE" Width="180px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFar16FiveMilesAdultThyroidCDE" Width="180px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 235px">&nbsp;
                                                </td>
                                                <td style="width: 185px">10 Miles
                                                </td>
                                                <td style="width: 283px">
                                                    <asp:TextBox ID="txtFar16TenMilesTEDE" Width="180px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFar16MilesAdultThyroidCDE" Width="180px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 125px">17. Approved By:
                                                </td>
                                                <td style="width: 292px">
                                                    <asp:TextBox ID="txtFar17ApprovedBy" Width="185px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td style="width: 215px">Title:
                                        <asp:TextBox ID="txtFar17Title" Width="145px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                                <td style="width: 150px">Time:
                                        <asp:TextBox ID="txtFar17Time" Width="15px"
                                            Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                            onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                            onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtFar17Time2" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET
                                                </td>
                                                <td style="width: 200px">Date:
                                        <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtFar17Date" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtFar17Date');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtFar17Date.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 40px">&nbsp;
                                                </td>
                                                <td>Notified By:
                                       <asp:TextBox ID="txtFar17NotifiedBy" Width="185px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" border="3" style="border-color: #000; border-style: solid; width: 100%">
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 310px">Received By:
                                                    <asp:TextBox ID="txtFar17ReceivedBy" Width="180px" Style="background-color: #c2ecde" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 150px">Time:
                                                    <asp:TextBox ID="txtFar17ReceivedTime" Width="15px"
                                                        Style="background-color: #c2ecde; margin-left: 0px;" runat="server"
                                                        onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                                        onmouseout="UnTip()"></asp:TextBox>
                                                                <big><b>:</b></big>
                                                                <asp:TextBox ID="txtFar17ReceivedTime2" Width="15px" Style="background-color: #c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                                &nbsp;ET
                                                            </td>
                                                            <td style="width: 200px">Date:
                                                    <asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtFar17ReceivedDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                                <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                                    href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtFar17ReceivedDate');">
                                                                    <img alt="Calendar Icon"
                                                                        src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                                <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtFar17ReceivedDate.value = ''"
                                                                    onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <font size="2">(To be completed by receiving organization)</font>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlShowCRDefueled" runat="server" Visible="false">
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>1. Please Select one:
										   <asp:DropDownList ID="ddlCRDselectOne"
                                               Style="background-color: #c2ecde; margin-left: 4px;" Width="175px" runat="server">
                                               <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                               <asp:ListItem Value="This is a DRILL" Text="This is a DRILL"></asp:ListItem>
                                               <asp:ListItem Value="This is an EMERGENCY" Text="This is an EMERGENCY"></asp:ListItem>
                                           </asp:DropDownList>
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>I have a message for an:
										   <asp:DropDownList ID="ddlCRDmessageClassification"
                                               Style="background-color: #c2ecde; margin-left: 4px;" runat="server">
                                               <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                               <asp:ListItem Value="Unusual Event" Text="Unusual Event"></asp:ListItem>
                                               <asp:ListItem Value="Alert" Text="Alert"></asp:ListItem>
                                           </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>2&nbsp;
                                                </td>
                                                <td>A. Date:
											<asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtCRDdate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtCRDdate');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtCRDdate.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td>B. Contact Time:
											<asp:TextBox ID="txtCRDcontactTime" Width="15px"
                                                Style="background-color: #c2ecde; margin-left: 0px;" runat="server" MaxLength="25"
                                                onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                                onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtCRDcontactTime2" Width="15px" Style="background-color: #c2ecde" MaxLength="25" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET&nbsp;&nbsp;
                                                </td>
                                                <td>C. Reported By (Name):
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCRDreportedByName" Width="300px" Style="background-color: #c2ecde" MaxLength="250" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td style="width: 245px">D. Message Number:
											<asp:TextBox ID="txtCRDmessageNumber2" Width="65px" Style="background-color: #c2ecde" MaxLength="250" runat="server"></asp:TextBox>
                                                </td>
                                                <td>E. [Select One]:
                                            <asp:DropDownList ID="ddlCRDfSelectOne" runat="server"
                                                Style="background-color: #c2ecde; margin-left: 4px;">
                                                <asp:ListItem Selected="True" Text="Select an Option" Value="Select an Option"></asp:ListItem>
                                                <asp:ListItem Text="Initial/New Classification" Value="Initial/New Classification"></asp:ListItem>
                                                <asp:ListItem Text="Update" Value="Update"></asp:ListItem>
                                            </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table cellpadding="0" cellspacing="0" border="3" style="border-color: #000; border-style: solid; width: 993px">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>3.
                                                </td>
                                                <td>Emergency Classification:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCRDemergencyClassification"
                                                        Style="background-color: #c2ecde; margin-left: 4px;" runat="server">
                                                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="A. Unusual Event" Text="A. Unusual Event"></asp:ListItem>
                                                        <asp:ListItem Value="B. Alert" Text="B. Alert"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>Date
											<asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtCRDEmClassDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtCRDEmClassDate');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtCRDEmClassDate.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>Time
											<asp:TextBox ID="txtCRDEmClassTime" Width="15px"
                                                Style="background-color: #c2ecde; margin-left: 0px;" runat="server" MaxLength="25"
                                                onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                                onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtCRDEmClassTime2" Width="15px" Style="background-color: #c2ecde" runat="server" MaxLength="25" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td colspan="2">Emergency Termination:
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>Date
											<asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtCRDEmTermDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtCRDEmTermDate');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtCRDEmTermDate.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>Time
											<asp:TextBox ID="txtCRDEmTermTime" Width="15px"
                                                Style="background-color: #c2ecde; margin-left: 0px;" runat="server" MaxLength="25"
                                                onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                                onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtCRDEmTermTime2" Width="15px" Style="background-color: #c2ecde" runat="server" MaxLength="25" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>4. Reason for Emergency Declaration
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>A. EAL Number(s):
											<asp:TextBox ID="txtCRDeALNumbers" Width="50px" Style="background-color: #c2ecde" MaxLength="250" runat="server"></asp:TextBox>
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>Description:
											<asp:TextBox ID="txtCRDeALDescription" Width="230px" Style="background-color: #c2ecde" MaxLength="250" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 443px">5. Additional Information or Update:
											<asp:DropDownList ID="ddlCRDeALai"
                                                Style="background-color: #c2ecde; margin-left: 4px;" runat="server">
                                                <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="A. None" Text="A. None"></asp:ListItem>
                                                <asp:ListItem Value="B. Description" Text="B. Description"></asp:ListItem>
                                            </asp:DropDownList>
                                                </td>
                                                <td>Description:
											<asp:TextBox ID="txtEALaiDescription" Width="425px" Style="background-color: #c2ecde" MaxLength="250" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 143px">6. Weather Data
                                                </td>
                                                <td>A. Wind direction from degrees:
											<asp:TextBox ID="txtCRDwindDirectionDegrees" Width="150px" Style="background-color: #c2ecde" MaxLength="250" runat="server"></asp:TextBox>
                                                </td>
                                                <td>B. Wind speed MPH (m/sec x 2.24 = MPH):
											<asp:TextBox ID="txtCRDwindSpeed" Width="170px" Style="background-color: #c2ecde" MaxLength="250" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>7. Release Status:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCRDreleaseStatus"
                                                        Style="background-color: #c2ecde; margin-left: 4px;" runat="server">
                                                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="A. None (go to item 10)" Text="A. None (go to item 10)"></asp:ListItem>
                                                        <asp:ListItem Value="B. In Progress" Text="B. In Progress"></asp:ListItem>
                                                        <asp:ListItem Value="C. Has occurred, but stopped (go to item 10)" Text="C. Has occurred, but stopped (go to item 10)"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>8. Release Significance: (at the Exclusion Area Boundary)
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCRDreleaseSignificance"
                                                        Style="background-color: #c2ecde; margin-left: 4px;" runat="server">
                                                        <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="A. Under Evaluation" Text="A. Under Evaluation"></asp:ListItem>
                                                        <asp:ListItem Value="B. Release is within Normal Operating Limits" Text="B. Release is within Normal Operating Limits"></asp:ListItem>
                                                        <asp:ListItem Value="C. Liquid release (no actions required)" Text="C. Liquid release (no actions required)"></asp:ListItem>
                                                        <asp:ListItem Value="D. Non-significant Fraction of PAG Range" Text="D. Non-significant Fraction of PAG Range"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>9.&nbsp;
                                                </td>
                                                <td colspan="3">Additional Release Information:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>A. Projected Total Dose (TEDE) for
											<asp:TextBox ID="txtCRDProjTotalDose" Width="35px" Style="background-color: #c2ecde" MaxLength="250" runat="server"></asp:TextBox>
                                                    hrs.
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>B. Distance of 0.83 Mile (Exclusion Area Boundary) 
											<asp:TextBox ID="txtCRDDistance83Mile" Width="35px" Style="background-color: #c2ecde" MaxLength="250" runat="server"></asp:TextBox>
                                                    mrem.
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 182px">10. Facility Conditions
                                                </td>
                                                <td>A. Spent Fuel Pool Adequately Cooled:
											<asp:DropDownList ID="ddlCRDfacCond"
                                                Style="background-color: #c2ecde; margin-left: 4px;" runat="server">
                                                <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                                <asp:ListItem Value="No" Text="No"></asp:ListItem>
                                            </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 200px">11. Message Received By
                                                </td>
                                                <td style="width: 380px">(Name):
											<asp:TextBox ID="txtCRDmessageRecdName" Width="295px" Style="background-color: #c2ecde" MaxLength="250" runat="server"></asp:TextBox>
                                                </td>
                                                <td style="width: 200px">Date
											<asp:TextBox runat="server" Style="background-color: #c2ecde" Columns="10" Width="80px" ID="txtCRDmessageRecdDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                                                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtCRDmessageRecdDate');">
                                                        <img alt="Calendar Icon"
                                                            src="Images/Calendar1.jpg" border="0" width="20" height="15" /></a>
                                                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtCRDmessageRecdDate.value = ''"
                                                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                                                </td>
                                                <td>Time
											<asp:TextBox ID="txtCRDmessageRecdTime" Width="15px"
                                                Style="background-color: #c2ecde; margin-left: 0px;" runat="server" MaxLength="25"
                                                onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')"
                                                onmouseout="UnTip()"></asp:TextBox>
                                                    <big><b>:</b></big>
                                                    <asp:TextBox ID="txtCRDmessageRecdTime2" Width="15px" Style="background-color: #c2ecde" runat="server" MaxLength="25" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                                                    &nbsp;ET
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" align="center">
                    <tr>
                        <td align="left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>SWO User Comments:
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:TextBox ID="txtCRDuserComments" Width="820px" Style="background-color: #c2ecde" runat="server" MaxLength="500"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
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
            <br />--%>
            <table width="100%" align="center">
                <tr>
                    <td align="center">&nbsp;&nbsp;&nbsp;
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

