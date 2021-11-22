<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Nav.ascx.vb" Inherits="Menus_Nav" %>

<asp:Panel ID="pnlShowAll" Visible="false" runat="server">
    <p>
        <asp:Panel ID="pnlShowAdminNav" Visible="false" runat="server">
            <span class="style17"><a class="menu" href="Home.aspx">Home</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"><a class="menu" href="Incident.aspx">Current Incidents</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"><a class="menu" href="EditIncident.aspx?IncidentID=0">Add Incident</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"><a class="menu"  href="#" onclick="Gator('<%= ConfigurationManager.AppSettings("GATORURL") %>','<%= ConfigurationManager.AppSettings("GATORPASS") %>','<%= ConfigurationManager.AppSettings("GATORUSR") %>')">SWO GATOR</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"><a class="menu" href="Messages.aspx">Messages</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"><a class="menu" href="Archive.aspx">Archive</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"><a class="menu" href="Administration.aspx">Administration</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"><a class="menu" href="DailyIncidentReport.aspx">Daily Incident Report</a></span>
        </asp:Panel>
    
        <asp:Panel ID="pnlShowFullUser" Visible="false" runat="server">
            <span class="style17"><a class="menu" href="Home.aspx">Home</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"> <a class="menu" href="IncidentNonAdmin.aspx">Current Incidents</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"> <a class="menu" href="EditIncident.aspx?IncidentID=0">Add Incident</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"> <a class="menu"  href="#" onclick="Gator('<%= ConfigurationManager.AppSettings("GATORURL") %>','<%= ConfigurationManager.AppSettings("GATORPASS") %>','<%= ConfigurationManager.AppSettings("GATORUSR") %>')">SWO GATOR</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"> <a class="menu" href="ArchiveNonAdmin.aspx">Archive</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"> <a class="menu" href="MyProfile.aspx">My Profile</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"> <a class="menu" href="ReportBuilder.aspx">Reports</a></span>
        </asp:Panel>
    
        <asp:Panel ID="pnlShowUpdateUser" Visible="false" runat="server">
            <span class="style17"><a class="menu" href="Home.aspx">Home</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"><a class="menu" href="IncidentNonAdmin.aspx">Current Incidents</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"><a class="menu"  href="#" onclick="Gator('<%= ConfigurationManager.AppSettings("GATORURL") %>','<%= ConfigurationManager.AppSettings("GATORPASS") %>','<%= ConfigurationManager.AppSettings("GATORUSR") %>')">SWO GATOR</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"><a class="menu" href="ArchiveNonAdmin.aspx">Archive</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"><a class="menu" href="MyProfile.aspx">My Profile</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"> <a class="menu" href="ReportBuilder.aspx">Reports</a></span>
        </asp:Panel>
    
        <asp:Panel ID="pnlShowReadOnly" Visible="false" runat="server">
            <span class="style17"><a class="menu" href="Home.aspx">Home</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"><a class="menu" href="IncidentNonAdmin.aspx">Current Incidents</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"><a class="menu"  href="#" onclick="Gator('<%= ConfigurationManager.AppSettings("GATORURL") %>','<%= ConfigurationManager.AppSettings("GATORPASS") %>','<%= ConfigurationManager.AppSettings("GATORUSR") %>')">SWO GATOR</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"><a class="menu" href="ArchiveNonAdmin.aspx">Archive</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"><a class="menu" href="MyProfile.aspx">My Profile</a></span>
            <span class="style16"><span class="style17">|</span></span>
            <span class="style17"> <a class="menu" href="ReportBuilder.aspx">Reports</a></span>
        </asp:Panel>
    </p>
</asp:Panel>