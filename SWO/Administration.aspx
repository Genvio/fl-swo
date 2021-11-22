<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="Administration.aspx.vb" Inherits="Administration"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6"><b>Administration</b></font>
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" align="center">
        <tr>
           <td align="center">
                Users
                <br />
                <br />
                <a href="User.aspx"><img border="0" alt="Users" src="Images/AdminIcon.jpg" /></a> 
           </td>
           <td align="center">
                Audit
                <br />
                <br />
                <a href="Audit.aspx"><img border="0" alt="Audit" src="Images/AuditIcon.jpg" /></a> 
           </td>
           <td align="center">
                Associated Tasks
                <br />
                <br />
                <a href="AssociatedTasks.aspx"><img border="0" alt="Associated Tasks" src="Images/Tasks.jpg" /></a> 
           </td>
           <td align="center">
                Positions
                <br />
                <br />
                <a href="NotificationPosition.aspx"><img border="0" alt="Positions" src="Images/Position.jpg" /></a> 
           </td>
        </tr>
        <tr>
           <td align="center">
                &nbsp;
           </td>
           <td align="center">
                &nbsp;
           </td>
           <td align="center">
                &nbsp;
           </td>
           <td align="center">
                &nbsp;
           </td>
        </tr>
        <tr>
           <td align="center">
                Exempt
                <br />
                <br />
                <a href="Exempt.aspx"><img border="0" alt="Exempt" src="Images/Exempt.gif" /></a> 
           </td>
           <td align="center">
                Agency
                <br />
                <br />
                <a href="Agency.aspx"><img border="0" alt="Agency" src="Images/Agency.jpg" /></a>
           </td>
           <td align="center">
                Worksheets
                <br />
                <br />
                <a href="Worksheets.aspx"><img border="0" alt="Worksheets" src="Images/IncidentType.JPG" /></a>
           </td>
           <td align="center">
                Daily Incident Report
                <br />
                <br />
                <a href="DailyIncidentReport.aspx"><img border="0" alt="Reports" src="Images/StatsIcon2.jpg" /></a> 
           </td>
        </tr>
        <tr>
           <td align="center">
                &nbsp;
           </td>
           <td align="center">
                &nbsp;
           </td>
           <td align="center">
                &nbsp;
           </td>
           <td align="center">
                &nbsp;
           </td>
        </tr>
        <tr>
           <td align="center">
                Notification Group
                <br />
                <br />
                <a href="NotificationGroup.aspx"><img border="0" alt="Notification Group" src="Images/GroupNotification.jpg" /></a> 
           </td>
       
           <td align="center">
                Reports
                <br />
                <br />
                <a href="ReportBuilder.aspx"><img border="0" alt="Reports" src="Images/ReportIcon.bmp" /></a> 
           </td>
           <td align="center">
                Region Coordinators
                <br />
                <br />
                <a href="RegionCoordinator.aspx"><img border="0" alt="Region Coordinators" src="Images/UserLevels.png" /></a> 
           </td>
           <td align="center">
                County Coordinators
                <br />
                <br />
                <a href="CountyCoordinator.aspx"><img border="0" alt="County Coordinators" src="Images/CountyCoordinator.png" /></a> 
           </td>
        </tr>
        <tr>
           <td align="center">
                &nbsp;
           </td>
           <td align="center">
                &nbsp;
           </td>
           <td align="center">
                &nbsp;
           </td>
           <td align="center">
                &nbsp;
           </td>
        </tr>
        <tr>
           <td align="center">
                Sectors
                <br />
                <br />
                <a href="Sector.aspx"><img border="0" alt="Sectors" src="Images/sectorsblue.png" /></a> 
           </td>
           <td align="center">
                &nbsp;
           </td>
           <td align="center">
                &nbsp;
           </td>
           <td align="center">
                &nbsp;
           </td>
        </tr>
    </table>
    <%--<br />
    <table width="100%" align="center">
        <tr>
           <td align="center">
                &nbsp;       
           </td>
           <td align="center">
                &nbsp;  
           </td>
           <td align="center">
                &nbsp;
                 Severity Levels
                <br />
                <br />
                <a href="SeverityLevel.aspx"><img border="0" alt="Severity Levels" src="Images/SeverityLevel.jpg" /></a>
           </td>
        </tr>
    </table>--%>
    <br />
</asp:Content>