<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="LiveTestPage.aspx.vb" Inherits="LiveTestPage" title="Untitled Page" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style100
        {
            width: 188px;
        }
        .style101
        {
            width: 191px;
        }
        .style102
        {
            width: 93px;
        }
        .style103
        {
            width: 348px;
        }
        .style104
        {
            width: 95px;
        }
        .style105
        {
            width: 369px;
        }
        .style106
        {
            width: 9px;
        }
        .style121
        {
            width: 667px;
        }
        .style123
        {
            width: 428px;
        }
        .style124
        {
            width: 271px;
        }
        .style125
        {
            width: 177px;
        }
        .style126
        {
            width: 170px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<AJAX:UpdatePanel ID="AJAXUpdatePanel" runat="server">
<Triggers>
  <asp:PostBackTrigger ControlID="btnAddAttachment" />
 </Triggers>
<ContentTemplate>


    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="border-color:#000000; border-style:solid">
                <b>Incident #:</b>
                <asp:Label ID="lblIncidentNumber"  runat="server"></asp:Label> 
            </td>
            <td style="border-color:#000000; border-style:solid">
                Created By:
                <asp:Label ID="lblCreatedBy" runat="server"></asp:Label>
                <asp:Label ID="lblCreatedOn" runat="server"></asp:Label>          
            </td>
            <td style="border-color:#000000; border-style:solid">
                Last Updated By:
                <asp:Label ID="lblUpdatedBy" runat="server"></asp:Label>
                <asp:Label ID="lblLastUpdatedOn" runat="server"></asp:Label>
            </td>
         </tr>
         <tr>
            <td align="left" style="border-color:#000000; border-style:solid">
                <b>Incident Status:</b>
                <asp:DropDownList ID="ddlIncidentStatus" Width="125px" style="background-color:#c2ecde" DataTextField="IncidentStatus" DataValueField="IncidentStatusID" runat="server"></asp:DropDownList>
            </td>
            <td align="left" style="border-color:#000000; border-style:solid" colspan="2">
                Incident Name:
                <asp:TextBox ID="txtIncidentName" Width="535px" style="background-color:#c2ecde" runat="server"></asp:TextBox>   
            </td>
        </tr>
        <tr>
            <td align="left" style="border-color:#000000; border-style:solid">
                Is this a drill? 
                <asp:DropDownList ID="ddlIsThisADrill"  style="background-color:#c2ecde" Width="154px"  runat="server">
                    <asp:ListItem Value="No" Text="No" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                </asp:DropDownList> 
            </td>
            <td align="left" style="border-color:#000000; border-style:solid">
                Assistance Requested?
                <asp:DropDownList ID="ddlStateAssistance"  style="background-color:#c2ecde" Width="175px"  runat="server">
                    <asp:ListItem Value="No" Text="No" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                </asp:DropDownList>   
            </td>
            <td align="left" style="border-color:#000000; border-style:solid">
                <b>Severity:</b>
                <asp:DropDownList ID="ddlSeverity" style="background-color:#c2ecde" DataTextField="Severity" DataValueField="SeverityID" AutoPostBack="true" runat="server"></asp:DropDownList>
            </td>
        </tr>
    </table>
    
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
         
    </table>
    <br />
    <table align="center" width="100%" cellspacing="0" border="2" style="border-color:#000000">
        <tr>
            <td valign="top" align="center" style="border-color:#000000; border-style:solid">
                <table align="center" width="100%" style="border-color:#000000; border-style:solid" border="2">
                    <tr>
                        <td align="center" style="border-color:#000000; border-style:solid" colspan="2">
                            Reporting Party
                            &nbsp;
                            <asp:DropDownList ID="ddlReportingPartyType"  style="background-color:#c2ecde" DataTextField="ReportingPartyType" DataValueField="ReportingPartyTypeID" AutoPostBack="true" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                <asp:Panel ID="pnlShowReportingParty" runat="server" Visible="false">
                    <tr>
                        <td align="right" >
                            First Name:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtReportingPartyFirstName" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Last Name:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtReportingPartyLastName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Represents:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtReportingPartyRepresents" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Phone 1:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtReportingPartyCallBackNumber1" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Phone 2:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtReportingPartyCallBackNumber2" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Email:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtReportingPartyEmail" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Address:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtReportingPartyAddress" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            City:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtReportingPartyCity" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            State:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtReportingPartyState" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Zipcode:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtReportingPartyZipcode" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </asp:Panel> <%--End pnlShowReportingParty--%>
                <asp:Panel ID="pnlShowReportingPartyCensored" runat="server" Visible="false">
                    <tr>
                        <td colspan="2" align="center" >
                            Protected Information-Please Contact SWO
                        </td>
                    </tr>
                </asp:Panel>
                </table>
            </td>
            <td valign="top" align="center" style="border-color:#000000; border-style:solid">
                <table align="center" width="100%" style="border-color:#000000; border-style:solid" border="2">
                    <tr>
                        <td align="center" style="border-color:#000000; border-style:solid" colspan="2">
                            On-Scene Contact
                            &nbsp;
                            <asp:DropDownList ID="ddlOnSceneContactType"  style="background-color:#c2ecde" Width="175px" DataTextField="OnSceneContactType" DataValueField="OnSceneContactTypeID" AutoPostBack="true" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                <asp:Panel ID="pnlShowOnSceneContact" runat="server" Visible="false">
                    <tr>
                        <td align="right" >
                            First Name:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOnSceneContactFirstName"  style="background-color:#c2ecde" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Last Name:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOnSceneContactLastName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Represents:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOnSceneContactRepresents" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Phone 1:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOnSceneContactPhone1" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Phone 2:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOnSceneContactPhone2" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Email:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOnSceneContactEmail" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Address:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOnSceneContactAddress" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            City:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOnSceneContactCity" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            State:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOnSceneContactState" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Zipcode:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOnSceneContactZipcode" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </asp:Panel> <%--End pnlShowOnSceneContact--%>
                </table>
            </td>
            <td valign="top" align="center" style="border-color:#000000; border-style:solid">
                <table align="center" width="100%" style="border-color:#000000; border-style:solid" border="2">
                    <tr>
                        <td align="center" style="border-color:#000000; border-style:solid" colspan="2">
                            Responsible Party
                            &nbsp;
                            <asp:DropDownList ID="ddlResponsiblePartyType"  style="background-color:#c2ecde" Width="175px" DataTextField="ResponsiblePartyType" DataValueField="ResponsiblePartyTypeID" AutoPostBack="true" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                <asp:Panel ID="pnlShowResponsibleParty" runat="server" Visible="false">
                    <tr>
                        <td align="right">
                            First Name:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtResponsiblePartyFirstName"  style="background-color:#c2ecde" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Last Name:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtResponsiblePartyLastName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Represents:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtResponsiblePartyRepresents" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Phone 1:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtResponsiblePartyPhone1" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Phone 2:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtResponsiblePartyPhone2" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Email:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtResponsiblePartyEmail" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Address:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtResponsiblePartyAddress" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            City:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtResponsiblePartyCity" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            State:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtResponsiblePartyState" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            Zipcode:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtResponsiblePartyZipcode" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </asp:Panel> <%--End pnlShowResponsibleParty--%>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="background-color: #d4d4d4;border-color:#000000; border-style:solid" class="style121" >
                <b>Initial Report:</b>
            </td>
            <%--<td align="center" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                <asp:Button ID="btnUpdateInitialReport" runat="server" Enabled="false" Text="Update Initial Report" />
            </td>--%>
            <td align="center" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                <asp:HyperLink ID="lnkInitialReportUpdates" Target="_blank" AlternateText="All Initial Report Updates" Text="View Changes" runat="server"></asp:HyperLink>
            </td>
        </tr>
    </table>
    <table align="center" width="100%">
        <tr>
            <td align="left">
                <asp:TextBox ID="txtInitialReport" style="background-color: #c2ecde" Height="300px" Width="990px" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowReportUpdate" runat="server" Visible="false">
        <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
            <tr>
                <td align="left" 
                    style="background-color: #d4d4d4;border-color:#000000; border-style:solid" 
                    class="style123">
                    <b>Most Recent Update:</b>
                </td>
                <td style="border-color:#000000; border-style:solid" class="style124" >
                    &nbsp;
                </td>
                <td style="border-color:#000000; border-style:solid" align="center">
                    <asp:HyperLink ID="lnkAllUpdates" Target="_blank" AlternateText="All Updates" Text="View Updates" runat="server"></asp:HyperLink>
                </td>
            </tr>
        </table>
        <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
            <tr>
                <td align="left" style="border-color:#000000; border-style:solid">
                    <asp:Label ID="lblLatestUpdate" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
            <tr>
                <td align="left" style="border-color:#000000; border-style:solid">
                    <asp:TextBox ID="txtReportUpdate" Height="53px" Width="880px" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td align="center" style="border-color:#000000; border-style:solid">
                    <asp:Button ID="btnUpdateReport" runat="server" Text="Add Update" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                <b>
                    Incident Details
                </b>
            </td>
        </tr>
    </table>
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="border-color:#000000; border-style:solid; width:50%">
                Date/Time Incident Occurred:
                <asp:TextBox runat="server" style="background-color:#c2ecde" Columns="10" Width="80px" ID="txtIncidentOccurredDate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtIncidentOccurredDate');"><img alt="Calendar Icon"
                src="Images/Calendar1.jpg" border="0" width="20" height="15"/></a>
                <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtIncidentOccurredDate.value = ''"
                onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                <asp:TextBox ID="txtIncidentOccurredTime"  Width="15px" 
                style="background-color:#c2ecde; margin-left: 0px;" runat="server" 
                onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                onmouseout="UnTip()"></asp:TextBox>
                :
                <asp:TextBox ID="txtIncidentOccurredTime2"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;ET
            </td>
            <td align="left" style="border-color:#000000; border-style:solid">
                Date/Time Reported to SWO:
                <asp:TextBox runat="server" style="background-color:#c2ecde" Columns="10" Width="80px" ID="txtReportedToSWODate" onmouseover="Tip('Enter DATE of target observation <BR> Format: MM/DD/YYYY <BR> ie.) 09/21/2009 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtReportedToSWODate');"><img alt="Calendar Icon"
                src="Images/Calendar1.jpg" border="0" width="20" height="15"/></a>&nbsp;
                <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtReportedToSWODate.value = ''"
                onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                <asp:TextBox ID="txtReportedToSWOTime"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                :
                <asp:TextBox ID="txtReportedToSWOTime2"  Width="15px" style="background-color:#c2ecde" runat="server" onmouseover="Tip('Enter the time you OBSERVED the target <BR> Format: 24 hour time  hh:mm <BR> ie.) 16:21 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()"></asp:TextBox>
                &nbsp;ET
            </td>
        </tr>
    </table>
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="border-color:#000000; border-style:solid; width:50%">
                &nbsp;
            </td>
            <td align="center" style="border-color:#000000; border-style:solid; width:50%">
                <asp:Panel ID="pnlShowViewAllReportUpdates" runat="server" Visible="false">
                    <asp:HyperLink ID="lnkViewAllReportUpdates" Target="_blank" AlternateText="View All Report Updates" Text="View All Report Updates" runat="server"></asp:HyperLink>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="border-color:#000000; border-style:solid; width:50%">
                Choose from the Radio Buttons Below to Obtain Coordinates
            </td>
            <td align="center" style="border-color:#000000; border-style:solid; width:50%">
                <asp:Panel ID="pnlShowViewLocation" runat="server" Visible="false">
                    &nbsp;<asp:HyperLink ID="lnkLocation" Target="_blank" AlternateText="View Location" Text="View Location" runat="server"></asp:HyperLink></asp:Panel>
            </td>
        </tr>
    </table>
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="border-color:#000000; border-style:solid; width:2%;">
                <asp:RadioButton ID="rdoFacilityNameSceneDescription" style="background-color: #c2ecde" runat="server" GroupName="rdoCoordinateGroup" AutoPostBack="true" onmouseout="UnTip()" 
                onmouseover="Tip('No Coordinates will be obtained', TITLEBGCOLOR , '#FF0000' ,TITLE, 'ATTENTION')"  />
            </td>
            <td align="left" style="border-color:#000000; border-style:solid">
                Facility Name or Scene Description:
            </td>
            <td align="left" style="border-color:#000000; border-style:solid">
                <asp:TextBox ID="txtFacilityNameSceneDescription" style="background-color:#c2ecde" Height="12px" Width="690px" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="border-color:#000000; border-style:solid; width:2%;">
                <asp:RadioButton ID="rdoAddressCity" style="background-color: #c2ecde" runat="server" GroupName="rdoCoordinateGroup" AutoPostBack="true" />
            </td>
            <td align="left" style="border-color:#000000; border-style:solid; width:26%" >
                 Address City   
            </td>
            <td align="left" style="border-color:#000000; border-style:solid; width:35%">
                Address:<asp:TextBox ID="txtAddress" Width="250px" style="background-color:#c2ecde" runat="server"></asp:TextBox></td>
            <td align="left" style="border-color:#000000; border-style:solid">
                City:
                <asp:TextBox ID="txtCity" style="background-color:#c2ecde" Width="250px" runat="server"></asp:TextBox>
                
            </td>
        </tr>
    </table>
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="border-color:#000000; border-style:solid; width:2%;">
                <asp:RadioButton ID="rdoByAddressZip" style="background-color: #c2ecde" runat="server" GroupName="rdoCoordinateGroup" AutoPostBack="true" />
            </td>
            <td align="left" style="border-color:#000000; border-style:solid; width:26%">
                Address Zip
            </td>
            <td align="left" style="border-color:#000000; border-style:solid; width:35%">
                Address:<asp:TextBox ID="txtAddress2" style="background-color:#c2ecde" Width="250px" runat="server"></asp:TextBox></td>
            <td align="left" style="border-color:#000000; border-style:solid">
                Zip:<asp:TextBox ID="txtZip"  style="background-color:#c2ecde; margin-left: 0px;" Width="250px" runat="server"></asp:TextBox></td>
        </tr>
    </table>
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="border-color:#000000; border-style:solid; width:2%;">
                <asp:RadioButton ID="rdoByIntersection" style="background-color: #c2ecde" runat="server" GroupName="rdoCoordinateGroup" AutoPostBack="true" />
            </td>
            <td align="left" style="border-color:#000000; border-style:solid; width:26%">
                Intersection City
            </td>
            <td align="left" style="border-color:#000000; border-style:solid">
                Street 1:<asp:TextBox ID="txtStreet" style="background-color:#c2ecde; margin-left: 0px;" runat="server" onmouseover="Tip('Correct: Monroe Street <br> Incorrect: Monroe', TITLEBGCOLOR , '#FF0000' ,TITLE, 'STREET EXTENSIONS REQUIRED')" 
                onmouseout="UnTip()"></asp:TextBox></td>
            <td align="left" style="border-color:#000000; border-style:solid">
                Street 2:<asp:TextBox ID="txtStreet2" style="background-color:#c2ecde; margin-left: 0px;" runat="server" onmouseover="Tip('Correct: Monroe Street <br> Incorrect: Monroe', TITLEBGCOLOR , '#FF0000' ,TITLE, 'STREET EXTENSIONS REQUIRED')" onmouseout="UnTip()"></asp:TextBox></td>
            <td align="left" style="border-color:#000000; border-style:solid">
                City:<asp:TextBox ID="txtCity2" style="background-color:#c2ecde; margin-left: 0px;" runat="server"></asp:TextBox></td>
        </tr>
    </table>
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="border-color:#000000; border-style:solid; width:2%;">
                <asp:RadioButton ID="rdoAffectedCounties" style="background-color: #c2ecde" runat="server" GroupName="rdoCoordinateGroup" AutoPostBack="true" onmouseout="UnTip()" 
                onmouseover="Tip('No Coordinates will be obtained', TITLEBGCOLOR , '#FF0000' ,TITLE, 'ATTENTION')"  />
            </td>
            <td style="border-color:#000000; border-style:solid">
                Affected Counties
            </td>
            <td align="center" style="border-color:#000000; border-style:solid">
                <asp:LinkButton ID="lnkAddAffectedCounty" Text="Click Here for Counties" AlternateText="Add Affected County" runat="server"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowAffectedCounties" runat="server" Visible="true">
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="border-color:#000000; border-style:solid; width:2%;">
                <b>Affected Counties:</b> <asp:Label ID="lblAffectedCounties" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    </asp:Panel><%--End pnlShowAffectedCounties--%>
    
    <asp:Panel ID="pnlShowCountyGrabber" runat="server" Visible="false">
    <table>
        <tr>
            <td align="right">
                <asp:Button ID="btnSaveCounties" Text="Save" runat="server" />
            </td>
            <td align="left">
                <asp:Button ID="btnCancelCounties" Text="Cancel" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td align="left" style="background-color: #d4d4d4">
                <big>Regions</big>
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td align="left" style="background-color: #d4d4d4">
                <big>Alphabetical</big>
            </td>
        </tr>
        <tr>
            <td style="background-color: #d4d4d4">
                <asp:CheckBox ID="cbxStatewide" runat="server" AutoPostBack="true" />
            </td>
            <td align="left" style="background-color: #d4d4d4">
                Statewide
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="background-color: #d4d4d4">
                <asp:CheckBox ID="cbxRegion1" runat="server" AutoPostBack="true" />
            </td>
            <td align="left" style="background-color: #d4d4d4">
                Region 1
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxBay" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Bay
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxAlachua2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Alachua
            </td>    
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxCalhoun" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Calhoun
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxBaker2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Baker
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxEscambia" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Escambia
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxBay2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Bay
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxGulf" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Gulf
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxBradford2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Bradford
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxHolmes" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Holmes       
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxBrevard2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Brevard
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxJackson" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Jackson       
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxBroward2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Broward
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxOkaloosa" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Okaloosa
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxCalhoun2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Calhoun
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxSantaRosa" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Santa Rosa
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxCharlotte2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Charlotte
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxWalton" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Walton
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxCitrus2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Citrus
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxWashington" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Washington
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxClay2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Clay
            </td>
        </tr>
        <tr>
            <td style="background-color: #d4d4d4">
                <asp:CheckBox ID="cbxRegion2" runat="server" AutoPostBack="true" />
            </td>
            <td align="left" style="background-color: #d4d4d4">
                Region 2
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxColumbia" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Columbia
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxCollier2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Collier
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxDixie" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Dixie
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxColumbia2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Columbia
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxFranklin" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Franklin
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxDeSoto2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                DeSoto
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxGadsden" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Gadsden
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxDixie2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Dixie
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxHamilton" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Hamilton
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxDuval2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Duval
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxJefferson" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Jefferson
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxEscambia2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Escambia
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxLafayette" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Lafayette
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxFlagler2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Flagler
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxLeon" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Leon
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxFranklin2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Franklin
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxLiberty" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Liberty
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxGadsden2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Gadsden
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxMadison" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Madison
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxGilchrist2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Gilchrist
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxSuwannee" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Suwannee
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxGlades2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Glades
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxTaylor" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Taylor
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxGulf2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Gulf
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxWakulla" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Wakulla
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxHamilton2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Hamilton
            </td>
        </tr>
        <tr>
            <td style="background-color: #d4d4d4">
                <asp:CheckBox ID="cbxRegion3" runat="server" AutoPostBack="true" />
            </td>
            <td align="left" style="background-color: #d4d4d4">
                Region 3
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxAlachua" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Alachua
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxHendry2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Hendry
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxBaker" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Baker
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxHardee2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Hardee
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxBradford" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Bradford
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxHernando2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Hernando
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxClay" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Clay
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxHighlands2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Highlands
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxDuval" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Duval
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxHillsborough2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Hillsborough
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxFlagler" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Flagler
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxIndianRiver2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Indian River
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxGilchrist" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Gilchrist
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxHolmes2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Holmes       
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxLevy" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Levy
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxJackson2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Jackson       
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxMarion" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Marion
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxJefferson2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Jefferson
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxNassau" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Nassau
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxLake2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Lake
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxPutnam" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Putnam
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxLafayette2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Lafayette
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxStJohns" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                St. Johns
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxLee2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Lee
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxUnion" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Union
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxLeon2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Leon
            </td>
        </tr>
        <tr>
            <td style="background-color: #d4d4d4">
                <asp:CheckBox ID="cbxRegion4" runat="server" AutoPostBack="true" />
            </td>
            <td align="left" style="background-color: #d4d4d4">
                Region 4
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxCitrus" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Citrus
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxLevy2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Levy
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxHardee" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Hardee
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxLiberty2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Liberty
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxHernando" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Hernando
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxMadison2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Madison
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxHillsborough" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Hillsborough
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxManatee2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Manatee
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxPasco" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Pasco
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxMarion2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Marion
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxPinellas" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Pinellas
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxMartin2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Martin
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxPolk" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Polk
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxMiamiDade2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Miami-Dade
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxSumter" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Sumter
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxMonroe2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Monroe
            </td>
        </tr>
        <tr>
            <td style="background-color: #d4d4d4">
                <asp:CheckBox ID="cbxRegion5" runat="server" AutoPostBack="true"  />
            </td>
            <td align="left" style="background-color: #d4d4d4">
                Region 5
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxBrevard" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Brevard
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxNassau2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Nassau
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxIndianRiver" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Indian River
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxOkaloosa2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Okaloosa
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxLake" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Lake
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxOrange2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Orange
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxMartin" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Martin
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxOsceola2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Osceola
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxOrange" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Orange
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxOkeechobee2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Okeechobee
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxOsceola" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Osceola
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxPalmBeach2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Palm Beach
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxSeminole" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Seminole
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxPasco2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Pasco
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxStLucie" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                St. Lucie
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxPinellas2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Pinellas
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxVolusia" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Volusia
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxPolk2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Polk
            </td>
        </tr>
        <tr>
            <td style="background-color: #d4d4d4">
                <asp:CheckBox ID="cbxRegion6" runat="server" style="background-color: #d4d4d4" AutoPostBack="true" />
            </td>
            <td align="left" style="background-color: #d4d4d4">
                Region 6
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxCharlotte" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Charlotte
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxPutnam2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Putnam
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxCollier" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Collier
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxSantaRosa2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Santa Rosa
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxDeSoto" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                DeSoto
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxSarasota2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Sarasota
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxGlades" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Glades
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxSeminole2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Seminole
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxHendry" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Hendry
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxStJohns2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                St. Johns
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxHighlands" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Highlands
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxStLucie2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                St. Lucie
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxLee" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Lee
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxSumter2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Sumter
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxManatee" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Manatee
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxSuwannee2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Suwannee
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxOkeechobee" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Okeechobee
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxTaylor2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Taylor
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxSarasota" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Sarasota
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxUnion2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Union
            </td>
        </tr>
        <tr>
            <td style="background-color: #d4d4d4">
                <asp:CheckBox ID="cbxRegion7" runat="server" AutoPostBack="true" />
            </td>
            <td align="left" style="background-color: #d4d4d4">
                Region 7
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxBroward" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Broward
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxVolusia2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Volusia
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxMiamiDade" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Miami-Dade
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxWakulla2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Wakulla
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxMonroe" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Monroe
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxWalton2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Walton
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbxPalmBeach" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Palm Beach
            </td>
            <td style="background-color: #d4d4d4">
                &nbsp;
            </td>
            <td>
                <asp:CheckBox ID="cbxWashington2" runat="server"  AutoPostBack="true" />
            </td>
            <td align="left">
                Washington
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="btnSaveCounties2" Text="Save" runat="server" />
            </td>
            <td align="left">
                <asp:Button ID="btnCancelsCounties2" Text="Cancel" runat="server" />
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="border-color:#000000; border-style:solid; width:2%;">
                <asp:RadioButton ID="rdoByCoordinateEntry" style="background-color: #c2ecde" runat="server" GroupName="rdoCoordinateGroup" AutoPostBack="true" />
            </td>
            <td align="left" style="border-color:#000000; border-style:solid">
                Coordinate Entry
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowCoordinates" Visible="false" runat="server">
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="border-color:#000000; border-style:solid">
                 <table width="100%">
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top">
                                        <big>
                                            <b>
                                                <br />
                                                Coordinates:
                                            </b>
                                        </big> 
                                    </td>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:RadioButton ID="rdoDecimalDegrees" style="background-color: #c2ecde" Text="Decimal Degrees" runat="server" GroupName="rdoConversion" AutoPostBack="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:RadioButton ID="rdoDegreesMinutes" style="background-color: #c2ecde" Text="Degrees Minutes" runat="server" GroupName="rdoConversion" AutoPostBack="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:RadioButton ID="rdoDegreesMinutesSeconds" style="background-color: #c2ecde" Text="Degrees, Minutes, Seconds" runat="server" GroupName="rdoConversion" AutoPostBack="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:RadioButton ID="rdoUSNG" style="background-color: #c2ecde" Text="USNG" runat="server" GroupName="rdoConversion" AutoPostBack="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Panel ID="pnlShowDecimalDegrees" Visible="true" runat="server">
                                        <table width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="Top" align="center" colspan="2">
                                                        <font size="3"><b><i>Decimal Degrees</i></b></font>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <big>
                                                            <b>
                                                                Lat:
                                                            </b>
                                                        </big>
                                                        <asp:TextBox ID="txtLatDecimalDegrees" runat="server" onmouseout="UnTip()" 
                                                        onmouseover="Tip('Enter LATITUDE of the target &lt;BR&gt; FORMAT DD.ddd &lt;BR&gt; ie.) 30.38659 &lt;BR&gt; ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                                                        style="background-color:#c2ecde" Width="130px"> </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <big>
                                                            <b>
                                                                Long:
                                                            </b>
                                                        </big>
                                                        <asp:TextBox ID="txtLongDecimalDegrees" runat="server" onmouseout="UnTip()" 
                                                        onmouseover="Tip('Enter LONGITUDE of the target &lt;BR&gt; FORMAT -DD.ddd &lt;BR&gt; ie.) -84.23246 &lt;BR&gt; ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" 
                                                        style="background-color:#c2ecde" Width="130px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="right">
                                                    DD.dddd
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    -DD.dddd
                                                </td>
                                            </tr>
                                        </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlShowDegreesMinutes" Visible="false" runat="server">
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <font size="3">
                                                        <b>
                                                            <i>
                                                                Degrees Minutes
                                                            </i>
                                                        </b>
                                                    </font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <big><b>Lat:</b></big>
                                                    <asp:TextBox ID="txtLatDegreesMinutes" Width="65px" style="background-color:#c2ecde" onmouseover="Tip('Enter Degrees LATITUDE of the target <BR> FORMAT DD <BR> ie.) 30 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()" runat="server"></asp:TextBox>
                                                    <asp:TextBox ID="txtLatDegreesMinutes2" Width="65px" style="background-color:#c2ecde" onmouseover="Tip('Enter Minutes LATITUDE of the target <BR> FORMAT MM.mm <BR> ie.) 23.1954 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <big><b>Long:</b></big>
                                                    <asp:TextBox ID="txtLongDegreesMinutes" Width="65px" style="background-color:#c2ecde" onmouseover="Tip('Enter Degrees LONGITUDE of the target <BR> FORMAT -DD <BR> ie.) -84 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()" runat="server"></asp:TextBox>
                                                    <asp:TextBox ID="txtLongDegreesMinutes2" Width="65px" style="background-color:#c2ecde" onmouseover="Tip('Enter Minutes LONGITUDE of the target <BR> FORMAT MM.mm <BR> ie.) 13.9476 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="right">
                                                    DD MM.mm
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    -DD MM.mm
                                                </td>
                                            </tr>
                                        </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlShowDegreesMinutesSeconds" Visible="false" runat="server">
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <font size="3">
                                                        <b>
                                                            <i>
                                                                Degrees, Minutes, Seconds
                                                            </i>
                                                        </b>
                                                    </font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <big><b>Lat:</b></big>
                                                    <asp:TextBox ID="txtLatDegreesMinutesSeconds" Width="30px" style="background-color:#c2ecde" onmouseover="Tip('Enter Degrees LATITUDE of the target <BR> FORMAT DD <BR> ie.) 30° ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()" runat="server"></asp:TextBox>
                                                    <b>°</b>
                                                    <asp:TextBox ID="txtLatDegreesMinutesSeconds2" Width="30px" style="background-color:#c2ecde" onmouseover="Tip('Enter Minutes LATITUDE of the target <BR> FORMAT MM <BR> ie.) 23’ ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()" runat="server"></asp:TextBox>
                                                    <b>’</b>
                                                    <asp:TextBox ID="txtLatDegreesMinutesSeconds3" Width="80px" style="background-color:#c2ecde" onmouseover="Tip('Enter Seconds LATITUDE of the target <BR> FORMAT SS.ss <BR> ie.) 11.7240’’ ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()" runat="server"></asp:TextBox>
                                                    <b>’’</b> 
                                                </td>
                                                <td>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <big><b>Long:</b></big>
                                                    <asp:TextBox ID="txtLongDegreesMinutesSeconds" Width="30px" style="background-color:#c2ecde" onmouseover="Tip('Enter Degrees LONGITUDE of the target <BR> FORMAT -DD <BR> ie.) -84° ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()" runat="server"></asp:TextBox>
                                                    <b>°</b>
                                                    <asp:TextBox ID="txtLongDegreesMinutesSeconds2" Width="30px" style="background-color:#c2ecde" onmouseover="Tip('Enter Minutes LONGITUDE of the target <BR> FORMAT MM <BR> ie.) 13’ ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()" runat="server"></asp:TextBox>
                                                    <b>’</b>
                                                    <asp:TextBox ID="txtLongDegreesMinutesSeconds3" Width="80px" style="background-color:#c2ecde" onmouseover="Tip('Enter Seconds LONGITUDE of the target <BR> FORMAT SS.ss <BR> ie.) 56.8560’’ ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()" runat="server"></asp:TextBox>
                                                    <b>’’</b>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="right">
                                                    DD MM SS.ss
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    -DD MM SS.ss
                                                </td>
                                            </tr>
                                        </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlShowUSNG" Visible="false" runat="server">
                                        <table width="100%">
                                             <tr >
                                                <td colspan="2" align="right">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    &nbsp;
                                                </td>
                                                <td align="center">
                                                    <font size="3">
                                                        <b>
                                                            <i>USNG</i>
                                                        </b>
                                                    </font>
                                                </td>
                                            </tr>
                                            <tr> 
                                                <td align="center"colspan="2">
                                                    <big>
                                                        <b>USNG:</b>
                                                    </big>
                                                    <asp:TextBox ID="txtUSNG" Width="300px" style="background-color:#c2ecde" onmouseover="Tip('Enter Seconds USNG of the target <BR> ie.) 16R GU 6593 6487 ', TITLEBGCOLOR , '#FF0000' ,TITLE, 'REQUIRED FIELD')" onmouseout="UnTip()" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center"colspan="2">
                                                    16R GU 6593 6487
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
            </td>
        </tr>
    </table>
    </asp:Panel><%-- End pnlShowCoordinates--%>
    
    
    
    
    <asp:Panel ID="pnlShowIncidentTypes" runat="server" Visible="true">
        <br />
        <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
            <tr>
                <td align="left" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                    <b>
                        Incident Worksheets
                    </b>
                    <asp:Button runat="server" ID="btnRefreshWorksheets" Text="Refresh" />
                </td>
                <td align="center" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                    <b>
                        <asp:DropDownList ID="ddlIncidentType" Width="250px" style="background-color:#c2ecde" DataTextField="IncidentType" DataValueField="IncidentTypeID" runat="server"></asp:DropDownList>
                    </b>
                </td>
                <td align="center" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                    <b>
                        <asp:Button ID="btnAddIncidentType" runat="server" Text="Add Worksheet Type" />
                    </b>
                </td>
            </tr>
        </table>
        <asp:Panel runat="server" ID="pnlShowIncidentTypeGrid" Visible="false">
        <table align="center" width="100%">
           <tr>
                <td>
                    <asp:DataGrid ID="IncidentIncidentTypeDataGrid" runat="server" Width="100%"
                        AutoGenerateColumns="false" AllowPaging="True" PageSize="100" PagerStyle-HorizontalAlign="center"
                        OnPageIndexChanged="IncidentIncidentTypeDataGrid_PageIndexChanged" >
                        <Columns>
                            <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="IncidentIncidentTypeID"
                                DataTextField="IncidentIncidentTypeID" SortExpression="IncidentIncidentTypeID ASC" HeaderText="IncidentIncidentTypeID">
                                <HeaderStyle Wrap="False"></HeaderStyle>
                            </asp:HyperLinkColumn>
                            
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a href="EditIncident.aspx?IncidentIncidentTypeID=<%# Container.dataitem("IncidentIncidentTypeID")%>&IncidentID=<%# Container.dataitem("IncidentID")%>&Action=Delete&Parameter=IncidentType"><img src="Images/delete-icon.png" alt="Delete Incident Worksheet" border="0" width="16" height="16"  onclick="javascript:return confirm('Are you sure you want to delete this Incident Worksheet?')" title="Delete Incident Worksheet" /> </a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a target="_blank" href="<%# Container.dataitem("PageName")%><%# Container.dataitem("IncidentID")%>&IncidentIncidentTypeID=<%# Container.dataitem("IncidentIncidentTypeID")%>"><img src="Images/edit.gif" alt="Edit User" border="0" width="16" height="16" title="Edit User" /> </a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="IncidentType" SortExpression="IncidentType" HeaderText="<b><u>&nbsp; Worksheet Type &nbsp; </u><b/>">
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" ></HeaderStyle>
                            </asp:BoundColumn>

                            <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="WorkSheetDescription" SortExpression="WorkSheetDescription" HeaderText="<b><u>&nbsp; Worksheet Name &nbsp; </u><b/>">
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" ></HeaderStyle>
                            </asp:BoundColumn>
                            
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
        </asp:Panel>
    </asp:Panel>
      
    <br />
    <asp:Panel runat="server" ID="pnlShowAttachmentsLinks" Visible="true">
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                <b>
                    Attachments:
                </b>
            </td>
            <td align="left" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                Select Attachment:<asp:FileUpload ID="FileUpload1" runat="server" /> 
            </td>
            <td align="left" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                Name:
                <asp:TextBox ID="txtAttachmentName" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
            <td align="center" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                <asp:Button ID="btnAddAttachment" runat="server" Text="Add Attachment" />
            </td>
        </tr>
    </table>
    <asp:Panel runat="server" ID="pnlShowAttachment" Visible="false">
        <table align="center" width="100%">
           <tr>
                <td>
                    <asp:DataGrid ID="AttachmentDataGrid" runat="server" Width="100%"
                        AutoGenerateColumns="false" AllowPaging="True" PageSize="100" PagerStyle-HorizontalAlign="center"
                        OnPageIndexChanged="AttachmentDataGrid_PageIndexChanged" >
                        <Columns>
                            <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="AttachmentID"
                                DataTextField="AttachmentID" SortExpression="AttachmentID ASC" HeaderText="AttachmentID">
                                <HeaderStyle Wrap="False"></HeaderStyle>
                            </asp:HyperLinkColumn>
                            
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a href="EditIncident.aspx?AttachmentID=<%# Container.dataitem("AttachmentID")%>&IncidentID=<%# Container.dataitem("IncidentID")%>&Action=Delete&Parameter=Attachment"><img src="Images/delete-icon.png" alt="Delete Attachment" border="0" width="16" height="16"  onclick="javascript:return confirm('Are you sure you want to delete this Attachment?')" title="Delete Attachment" /> </a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a target="_blank" href="Uploads/<%# Container.dataitem("Attachment")%>"><img src="Images/find.gif" alt="Attachment" border="0" width="16" height="16" title="Attachment" /> </a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="AttachmentName" SortExpression="AttachmentName" HeaderText="<b><u>&nbsp; Name &nbsp; </u><b/>">
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" ></HeaderStyle>
                            </asp:BoundColumn>
                            
                            <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="UserName" SortExpression="LinkDate" HeaderText="<b><u>&nbsp; User &nbsp; </u><b/>">
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" ></HeaderStyle>
                            </asp:BoundColumn>

                            <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="AttachmentDate" SortExpression="AttachmentDate" HeaderText="<b><u>&nbsp; Date/Time &nbsp; </u><b/>">
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" ></HeaderStyle>
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
        <tr>
            <td align="left" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                <b>
                    Links
                </b>
            </td>
            <td align="left" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                <asp:TextBox ID="txtLink" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
            <td align="center" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                <asp:Button ID="btnAddLink" runat="server" Text="Add Link" />
            </td>
        </tr>
    </table>
    <asp:Panel runat="server" ID="pnlShowLink" Visible="false">
        <table align="center" width="100%">
           <tr>
                <td>
                    <asp:DataGrid ID="LinkDataGrid" runat="server" Width="100%"
                        AutoGenerateColumns="false" AllowPaging="True" PageSize="100" PagerStyle-HorizontalAlign="center"
                        OnPageIndexChanged="LinkDataGrid_PageIndexChanged" >
                        <Columns>
                            <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="LinkID"
                                DataTextField="LinkID" SortExpression="LinkID ASC" HeaderText="LinkID">
                                <HeaderStyle Wrap="False"></HeaderStyle>
                            </asp:HyperLinkColumn>
                            
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a href="EditIncident.aspx?LinkID=<%# Container.dataitem("LinkID")%>&IncidentID=<%# Container.dataitem("IncidentID")%>&Action=Delete&Parameter=Link"><img src="Images/delete-icon.png" alt="Delete Link" border="0" width="16" height="16"  onclick="javascript:return confirm('Are you sure you want to delete this Link?')" title="Delete Link" /> </a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a target="_blank" href="<%# Container.dataitem("Link")%>"><img src="Images/find.gif" alt="Link" border="0" width="16" height="16" title="Link" /> </a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            
                            <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="Link" SortExpression="Link" HeaderText="<b><u>&nbsp; Link &nbsp; </u><b/>">
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" ></HeaderStyle>
                            </asp:BoundColumn>

                            <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="UserName" SortExpression="LinkDate" HeaderText="<b><u>&nbsp; User &nbsp; </u><b/>">
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" ></HeaderStyle>
                            </asp:BoundColumn>
                            
                            <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="LinkDate" SortExpression="LinkDate" HeaderText="<b><u>&nbsp; Date &nbsp; </u><b/>">
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" ></HeaderStyle>
                            </asp:BoundColumn>
                            
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
    </asp:Panel>
    </asp:Panel>
    <asp:Label runat="server" ID="lblTest"></asp:Label>
    <br />
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <asp:Panel ID="pnlShowViewFullReport" runat="server" Visible="false">
                    <asp:HyperLink ID="lnkViewFullReport" Target="_blank" AlternateText="View Full Report" Text="View Full Report" runat="server"></asp:HyperLink>
                </asp:Panel>
            </td>
            <td align="center">
                <asp:Panel ID="pnlShowViewFullReportText" runat="server" Visible="false">
                    <asp:HyperLink ID="lnkViewFullReportText" Visible="false" Target="_blank" AlternateText="View Full Report Text" Text="View Full Report Text" runat="server"></asp:HyperLink>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <asp:HyperLink ID="lnkNotify" runat="server" Target="_blank" Text="Notify"></asp:HyperLink>
                &nbsp;&nbsp;&nbsp;
                <input type="button" value="                      Save Incident                      " id="btnSave" runat="server" onserverclick="btnSave_Command" />
                &nbsp;&nbsp;&nbsp;
                <input type="button" value="Cancel" id="btnCancel" runat="server" onserverclick="btnCancel_Command" />
                &nbsp;&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlMessage" runat="server" Visible="false">
        <table width="100%">
            <tr>
                <td align="left">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlShowResults" runat="server" Visible="false">
    <table width="100%">
        <tr>
            <td align="center">
                <font size="5">
                    <b>
                        Conversions
                    </b>
                </font>
            </td>
        </tr>
    </table>
    <br />
    <table width="100%">
        <tr>
            <td align="right">
                <b>Decimal Degrees:</b>
            </td>
            <td align="left">
                <asp:Label ID="lblLatDecimalDegrees" runat="server"></asp:Label><asp:Label ID="lblLongDecimalDegrees" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <b>Degrees Minutes:</b>
            </td>
            <td align="left">
                 <asp:Label ID="lblLatDegreesMinutes" runat="server"></asp:Label><asp:Label ID="lblLongDegreesMinutes" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <b>Degrees, Minutes, Seconds:</b>
            </td>
            <td align="left">
                 <asp:Label ID="lblLatDegreesMinutesSeconds" runat="server"></asp:Label><asp:Label ID="lblLongDegreesMinutesSeconds" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <b>USNG:</b>
            </td>
            <td align="left">
                 <asp:Label ID="lblUSNG" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <asp:Panel ID="pnlHideStuffToGetRidOf" Visible="false" runat="server">
        <table>
            <tr>
                <td>
                    This incident is being handled: <asp:TextBox ID="txtHandled" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        
        <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
            <tr>
                <td style="border-color:#000000; border-style:solid">
                    Dept/agencies noified, responding, scene:
                    <asp:TextBox ID="txtAgencyDeptNotified" Width="650px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
   
</ContentTemplate>
</AJAX:UpdatePanel>
</asp:Content>

