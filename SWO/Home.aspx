<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="Home.aspx.vb" Inherits="Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .left {
            width: 328px;
        }

        .middleRight {
            width: 329px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="pnlMessage" runat="server" Visible="false">
        <table width="100%">
            <tr>
                <td align="left">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlShowAcknowledge" Visible="false" runat="server">
        <table width="100%">
            <tr>
                <td align="center">
                    <b><i>This system and the data that it contains are the property of the State of Florida, Division of Emergency Management.
                        Any use of this system implies consent to electronic monitoring activities with no expectation of privacy.
                    </i></b>
                    <br />
                    <br />
                    <b><i>Data contained on, and obtained from, this system may be exempt from public disclosure 
                        and public access under F.S. 119.071, 281.301, and s. 24(a), Art. I of the State 
                        Constitution.  All information from this system should be treated as confidential without 
                        an authoritative opinion to the contrary.
                    </i></b>
                    <br />
                    <br />
                    <b><i>To continue using the system please check the “Acknowledge” checkbox and click submit.
                    </i></b>
                    <br />
                    <br />
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:CheckBox ID="cbxAcknowledge" runat="server" Text="I Acknowledge" />
                    <br />
                    <br />
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlShowAll" Visible="false" runat="server">
        <asp:Panel ID="pnlShowAdmin" Visible="false" runat="server">
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        <font size="6"><b>Home</b></font>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center" class="left" />
                    <td align="center" class="middleRight" />
                    <td align="center" />
                </tr>
                <tr>
                    <td align="center" class="left">Current Incidents
                        <br />
                        <br />
                        <a href="Incident.aspx">
                            <img src="Images/IncidentRecorder.png" alt="Current Incidents" /></a>
                    </td>
                    <td align="center" class="middleRight">SWO GATOR
                        <br />
                        <br />
                        <a href="#" onclick="Gator('<%= ConfigurationManager.AppSettings("GATORURL") %>','<%= ConfigurationManager.AppSettings("GATORPASS") %>','<%= ConfigurationManager.AppSettings("GATORUSR") %>')">
                            <img src="Images/GATORIcon.jpg" alt="SWO GATOR" /></a>
                    </td>
                    <td align="center">Messages 
                        <br />
                        <br />
                        <a href="Messages.aspx">
                            <img border="0" alt="Messages" src="Images/updates.jpg" style="height: 126px; width: 120px" /></a>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="left" />
                    <td align="center" class="middleRight" />
                    <td align="center" />
                </tr>
                <tr>
                    <td align="center" class="left">Archive
                        <br />
                        <br />
                        <a href="Archive.aspx">
                            <img border="0" alt="Incident Archive" src="Images/SearchQueryIcon.png" /></a>
                    </td>
                    <td align="center" class="middleRight">Administration
                        <br />
                        <br />
                        <a href="Administration.aspx">
                            <img border="0" alt="Administration" src="Images/AdminIcon.png" /></a>
                    </td>
                    <td align="center">Daily Incident Report 
                        <br />
                        <br />
                        <a href="DailyIncidentReport.aspx">
                            <img border="0" alt="Reports" src="Images/StatsIcon2.jpg" /></a>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="left" />
                    <td align="center" class="middleRight" />
                    <td align="center" />
                </tr>
                <tr>
                    <td align="center" class="left">Incident Status Display
                        <br />
                        <br />
                        <a href="IncidentStatusDisplay.aspx" target="_blank">
                            <img border="0" alt="Incident Archive" src="Images/phone_icon.jpg" /></a>
                    </td>
                    <td align="center" class="middleRight">My Profile
                        <br />
                        <br />
                        <a href="MyProfile.aspx">
                            <img border="0" alt="Incident Archive" src="Images/loginuser-nobkgrd.png" width="100px" height="107px" /></a>
                    </td>
                    <td align="center">Hazmat Release Search
                        <br />
                        <br />
                        <a href="HazmatReleaseSearch.aspx">
                            <img border="0" alt="Hazmat Release Search" src="Images/green-115x94.jpg" /></a>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="left" />
                    <td align="center" class="middleRight" />
                    <td align="center" />
                </tr>
            </table>
        </asp:Panel>

        <asp:Panel ID="pnlShowFullUser" Visible="false" runat="server">
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        <font size="6"><b>Home</b></font>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center" class="left" />
                    <td align="center" class="middleRight" />
                    <td align="center" />
                </tr>
                <tr>
                    <td align="center" class="left">Current Incidents
                        <br />
                        <br />
                        <a href="IncidentNonAdmin.aspx">
                            <img src="Images/IncidentRecorder.png" alt="Current Incidents" /></a>
                    </td>
                    <td align="center" class="middleRight">SWO GATOR
                        <br />
                        <br />
                        <a href="#" onclick="Gator('<%= ConfigurationManager.AppSettings("GATORURL") %>','<%= ConfigurationManager.AppSettings("GATORPASS") %>','<%= ConfigurationManager.AppSettings("GATORUSR") %>')">
                            <img src="Images/GATORIcon.jpg" alt="SWO GATOR" /></a>
                    </td>
                    <td align="center">Hazmat Release Search
                        <br />
                        <br />
                        <a href="HazmatReleaseSearch.aspx">
                            <img border="0" alt="Hazmat Release Search" src="Images/green-115x94.jpg" /></a>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="left" />
                    <td align="center" class="middleRight" />
                    <td align="center" />
                </tr>
                <tr>
                    <td align="center" class="left">Archive 
                        <br />
                        <br />
                        <a href="ArchiveNonAdmin.aspx">
                            <img border="0" alt="Incident Archive" src="Images/SearchQueryIcon.png" /></a>
                    </td>
                    <td align="center" class="middleRight">My Profile
                       <br />
                        <br />
                        <a href="MyProfile.aspx">
                            <img border="0" alt="Incident Archive" src="Images/loginuser-nobkgrd.png" width="100px" height="107px" /></a>
                    </td>
                    <td align="center">Reports
                       <br />
                        <br />
                        <a href="ReportBuilder.aspx">
                            <img border="0" alt="Add User" src="Images/ReportIcon.bmp" /></a>
                    </td>
                </tr>
            </table>
        </asp:Panel>

        <asp:Panel ID="pnlShowUpdateUser" Visible="false" runat="server">
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        <font size="6"><b>Home</b></font>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center" class="left" />
                    <td align="center" class="middleRight" />
                    <td align="center" />
                </tr>
                <tr>
                    <td align="center" class="left">Current Incidents
                        <br />
                        <br />
                        <a href="IncidentNonAdmin.aspx">
                            <img src="Images/IncidentRecorder.png" alt="Current Incidents" /></a>
                    </td>
                    <td align="center" class="middleRight">SWO GATOR
                        <br />
                        <br />
                     
                        <a href="#" onclick="Gator('<%= ConfigurationManager.AppSettings("GATORURL") %>','<%= ConfigurationManager.AppSettings("GATORPASS") %>','<%= ConfigurationManager.AppSettings("GATORUSR") %>')">
                            <img src="Images/GATORIcon.jpg" alt="SWO GATOR" /></a>



                    </td>
                    <td align="center">Hazmat Release Search
                        <br />
                        <br />
                        <a href="HazmatReleaseSearch.aspx">
                            <img border="0" alt="Hazmat Release Search" src="Images/green-115x94.jpg" /></a>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="left" />
                    <td align="center" class="middleRight" />
                    <td align="center" />
                </tr>
                <tr>
                    <td align="center" class="left">Archive
                        <br />
                        <br />
                        <a href="ArchiveNonAdmin.aspx">
                            <img border="0" alt="Incident Archive" src="Images/SearchQueryIcon.png" /></a>
                    </td>
                    <td align="center" class="middleRight">My Profile
                        <br />
                        <br />
                        <a href="MyProfile.aspx">
                            <img border="0" alt="Incident Archive" src="Images/loginuser-nobkgrd.png" width="100px" height="107px" /></a>
                    </td>
                    <td align="center">Reports
                       <br />
                        <br />
                        <a href="ReportBuilder.aspx">
                            <img border="0" alt="Add User" src="Images/ReportIcon.bmp" /></a>
                    </td>
                </tr>
            </table>
        </asp:Panel>

        <asp:Panel ID="pnlShowReadOnly" Visible="false" runat="server">
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        <font size="6"><b>Home</b></font>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center" class="left" />
                    <td align="center" class="middleRight" />
                    <td align="center" />
                </tr>
                <tr>
                    <td align="center" class="left">Current Incidents
                        <br />
                        <br />
                        <a href="IncidentNonAdmin.aspx">
                            <img src="Images/IncidentRecorder.png" alt="Current Incidents" /></a>
                    </td>
                    <td align="center" class="middleRight">SWO GATOR
                        <br />
                        <br />
                         <a href="#" onclick="Gator('<%= ConfigurationManager.AppSettings("GATORURL") %>','<%= ConfigurationManager.AppSettings("GATORPASS") %>','<%= ConfigurationManager.AppSettings("GATORUSR") %>')">
                            <img src="Images/GATORIcon.jpg" alt="SWO GATOR" /></a>
                    </td>
                    <td align="center">Hazmat Release Search
                        <br />
                        <br />
                        <a href="HazmatReleaseSearch.aspx">
                            <img border="0" alt="Hazmat Release Search" src="Images/green-115x94.jpg" /></a>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="left" />
                    <td align="center" class="middleRight" />
                    <td align="center" />
                </tr>
                <tr>
                    <td align="center" class="left">Archive 
                        <br />
                        <br />
                        <a href="ArchiveNonAdmin.aspx">
                            <img border="0" alt="Incident Archive" src="Images/SearchQueryIcon.png" /></a>
                    </td>
                    <td align="center" class="middleRight">My Profile
                        <br />
                        <br />
                        <a href="MyProfile.aspx">
                            <img border="0" alt="Incident Archive" src="Images/loginuser-nobkgrd.png" width="100px" height="107px" /></a>
                    </td>
                    <td align="center">Reports
                       <br />
                        <br />
                        <a href="ReportBuilder.aspx">
                            <img border="0" alt="Add User" src="Images/ReportIcon.bmp" /></a>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
  

</asp:Content>
