<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="ReportBuilder.aspx.vb" Inherits="ReportBuilder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        
    </style>

    <%--<link href="Includes/CSS/Report.css" type="text/css" rel="stylesheet" />--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6"><b>Report Center</b></font>
            </td>
        </tr>
        <asp:Panel ID="pnlMessage" runat="server" Visible="false">
            <tr>
                <td align="left" colspan="2">
                    <div class="feature">
                        <table width="100%">
                            <tr>
                                <%--<td width="20%" align="right">
                                    <img alt="Error Red X" src="Images/RedXIcon.JPG" />
                                </td>--%>
                                <td valign="middle" align="left">
                                    <%--<br />
                                    <br />
                                    <br />
                                    <br />
                                    <br />--%>
                                    <font size="5"><span style="color:#fe5105;">Please correct the following errors:</span></font>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center" colspan="2">
                                    <table width="100%">
                                        <tr align="left">
                                            <%--<td width="50%"></td>--%>
                                            <td align="left">
                                                <asp:Label ID="lblMessage" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </asp:Panel>
    </table>
    <br />
    <table width="100%" align="center">
        <tr>
            <td>
                <font size="4"><b>Choose a Report</b></font>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td>
                <asp:RadioButton ID="rdoIncidentWorksheet" GroupName="rdoIncidentWorksheet" Checked="True" runat="server" AutoPostBack="True"/>
                <asp:Label ID="Label1" runat="server" Text="Incident Worksheet" Width="165px" Font-Bold="True" />
                <asp:Label ID="Label2" runat="server" Width="25px" />
                <%--<asp:RadioButton ID="rdoWorksheetCountByCounty" GroupName="rdoIncidentWorksheet" runat="server" AutoPostBack="True"/>
                <asp:Label ID="Label3" runat="server" Text="Worksheet Count by County" Width="220px" Font-Bold="True" />
                <asp:Label ID="Label4" runat="server" Width="30px" />--%>
                <asp:RadioButton ID="rdoWorksheetCounty" GroupName="rdoIncidentWorksheet" runat="server" AutoPostBack="True"/>
                <asp:Label ID="Label5" runat="server" Text="Incident Worksheet/County" Width="220px" Font-Bold="True" />
            </td>
        </tr>
        <asp:Panel ID="pnlDailyReports" Visible="False" runat="server">
            <tr>
                <td>
                    <asp:RadioButton ID="rdoTotalActivity" GroupName="rdoReportGroup" Checked="True" runat="server" AutoPostBack="True"/>
                    <asp:Label ID="Label6" runat="server" Text="Daily Activity Reports" Width="165px" Font-Bold="True" />
                    <asp:Label ID="Label7" runat="server" Width="25px" />
                    <asp:RadioButton ID="rdoTotalReports" GroupName="rdoReportGroup" runat="server" AutoPostBack="True"/>
                    <asp:Label ID="Label8" runat="server" Text="Daily Reports" Width="220px" Font-Bold="True" />
                </td>
            </tr>
        </asp:Panel>
    </table>
    <br />
    <table width="100%" align="center">
        <tr>
            <td>
                <font size="4"><b>Choose Filters</b></font>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <%--<asp:Panel ID="pnlUsers" Visible="False" runat="server">
                <td align="right" valign="top">
                    <font size="3"><b>Users:</b></font>
                    <asp:Label ID="Label9" runat="server" Text="Users:" Font-Bold="True" />
                </td>
                <td align="left">
                    <asp:ListBox runat="server" ID="lbxUsers" Rows="12" Width="190px" DataTextField="Name"
                        DataValueField="UserID" CssClass="lbxFixedSize" SelectionMode="multiple" Enabled="False">
                    </asp:ListBox>
                </td>
            </asp:Panel>--%>
            <td valign="top" width="120px">
                <%--<font size="3"><b>Worksheets:</b></font>--%>
                <asp:Label ID="Label10" runat="server" Text="Worksheet:" Font-Bold="True" />
            </td>
            <td align="left">
                <asp:ListBox runat="server" ID="lbxIncidentType" Rows="12" Width="245px" DataTextField="IncidentType"
                    DataValueField="IncidentTypeID" CssClass="lbxFixedSize" SelectionMode="multiple">
                </asp:ListBox>
            </td>
            <td valign="top" width="120px">
                <%--<font size="3"><b>County:</b></font>--%>
                <asp:Label ID="Label11" runat="server" Text="County:" Font-Bold="True" />
            </td>
            <td align="left">
                <asp:ListBox runat="server" ID="lbxCounty" Enabled="false" Rows="12" 
                    Width="245px" DataTextField="County"
                    DataValueField="County" CssClass="lbxFixedSize" SelectionMode="multiple">
                </asp:ListBox>
            </td>
        </tr>
        <%--<tr>
            <td align="left" valign="top">
                <font size="3"><b>County:</b></font>
                <asp:Label ID="Label12" runat="server" Text="County:" Font-Bold="True" />
            </td>
            <td align="left">
                <asp:DropDownList Enabled="false" Id="ddlCounty" runat="server" DataTextField="County" DataValueField="County">
                </asp:DropDownList>
            </td>
        </tr>--%>
        <tr>
            <td>
                
            </td>
            <td>
                
            </td>
            <td colspan="2">
                <asp:Panel ID="pnlSummation" Visible="false" runat="server">
                    <table width="100%" align="center">
                        <tr>
                            <td>
                                <font size="4"><b>Sum Worksheets for Selected Counties</b></font>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" align="center">
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbxSummation" runat="server" />
                                <asp:Label ID="Label15" runat="server" Text="Check for summation" Font-Bold="True" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <%--<asp:Panel runat="server" ID="pnlHideExtraParameters" Visible="false">
        <table width="100%" align="center">
            <tr>
                <td align="right" valign="top">
                    <font size="3"><b>Activity:</b></font>
                    <asp:Label ID="Label13" runat="server" Text="Activity:" Font-Bold="True" />
                </td>
                <td align="left">
                    <asp:ListBox runat="server" ID="lbxActivity" Rows="12" Width="200px" DataTextField="Activity"
                        DataValueField="ActivityID" CssClass="lbxFixedSize" SelectionMode="multiple">
                    </asp:ListBox>
                </td>
                <td align="right" valign="top">
                    <font size="3"><b>Applicant:</b></font>
                    <asp:Label ID="Label14" runat="server" Text="Applicant:" Font-Bold="True" />
                </td>
                <td align="left">
                    <asp:ListBox runat="server" ID="lbxApplicant" Rows="12" Width="500px" DataTextField="Applicant"
                        DataValueField="ApplicantID" CssClass="lbxFixedSize" SelectionMode="multiple">
                    </asp:ListBox>
                </td>
            </tr>
        </table>
    </asp:Panel>--%>
    <br />
    <asp:Panel ID="pnlDates" Visible="true" runat="server">
        <table width="100%" align="center">
            <tr>
                <td>
                    <font size="4"><b>Choose Dates</b></font>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td>
                    <asp:RadioButton ID="rdoPickDates" GroupName="rdoDates" runat="server" AutoPostBack="true"/>
                    <asp:Label ID="Label16" runat="server" Text="Select Dates" Width="100px" Font-Bold="True" />
                    <asp:Label ID="Label17" runat="server" Width="25px" />
                    <asp:RadioButton ID="rdoAllDates" GroupName="rdoDates" Checked="true" runat="server" AutoPostBack="true"/>
                    <asp:Label ID="Label18" runat="server" Text="All to Date" Width="100px" Font-Bold="True" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table width="100%" align="center">
        <asp:Panel ID="pnlShowDates" Visible="false" runat="server">
            <tr>
                <td> 
                    <b>Start Date:</b>
                    <asp:TextBox runat="server" Columns="10" ID="txtStartDate" />&nbsp;&nbsp;
                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtStartDate');">
                        <img alt="Calendar Icon" src="Images/Calendar1.jpg" border="0" width="20" height="15"/>
                    </a>&nbsp;&nbsp;
                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtStartDate.value = ''"
                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />&nbsp;&nbsp;&nbsp;
                    <b>End Date:</b>
                    <asp:TextBox runat="server" Columns="10" ID="txtEndDate" />&nbsp;&nbsp;
                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtEndDate');">
                        <img alt="Calendar Icon" src="Images/Calendar1.jpg" border="0" width="20" height="15"/>
                    </a>&nbsp;&nbsp;
                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtEndDate.value = ''"
                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" height="16"/>
                </td>
            </tr>
        </asp:Panel>
    </table>
    <br />
    <table width="100%" align="center">
        <tr>
            <td>
                <font size="4"><b>Choose Report Format</b></font>
            </td>
        </tr>
    </table>
        <table width="100%" align="center">
            <tr>
                <td>
                    <%--<b>Format:</b>--%>
                    <asp:Label ID="Label19" runat="server" Text="Format:" Font-Bold="True" />
                    <asp:DropDownList ID="ddlReportFormat" Width="125px" runat="server" 
                        Enabled="True">
                        <asp:ListItem value="Graph" Selected="True">Graph</asp:ListItem>
                        <%--<asp:ListItem value="HTML">HTML</asp:ListItem>--%>
                        <asp:ListItem value="Excel">Excel</asp:ListItem>
                        <%--<asp:ListItem value="Word">Word</asp:ListItem>
                        <asp:ListItem value="PDF">PDF</asp:ListItem>--%>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    <br />
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <asp:Button ID="btnRunReport" Text="Run Report" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>