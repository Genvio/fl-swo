<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="DailyIncidentReport.aspx.vb" Inherits="DailyIncidentReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6"><b>Daily Incident Report</b></font>
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
        </asp:Panel>
    </table>
    <br />
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
                <asp:RadioButton ID="rdoDate" GroupName="rdoDates" Checked="true" runat="server" AutoPostBack="true"/>
                <asp:Label ID="Label1" runat="server" Text="Select a Date" Width="100px" Font-Bold="True" />
                <asp:Label ID="Label2" runat="server" Width="25px" />
                <asp:RadioButton ID="rdoPickDates" GroupName="rdoDates" runat="server" AutoPostBack="true"/>
                <asp:Label ID="Label3" runat="server" Text="Select Dates" Width="100px" Font-Bold="True" />
                <asp:Label ID="Label4" runat="server" Width="25px" />
                <asp:RadioButton ID="rdoAllDates" GroupName="rdoDates" runat="server" AutoPostBack="true"/>
                <asp:Label ID="Label5" runat="server" Text="All to Date" Width="100px" Font-Bold="True" />
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <asp:Panel ID="pnlShowDate" Visible="true" runat="server">
            <tr>
                <td> 
                    <b>Date:</b>
                    <asp:TextBox runat="server" Columns="10" ID="txtDate" />&nbsp;&nbsp;
                    <a onmouseover="window.status='Date Picker';return true;" onmouseout="window.status='';return true;"
                        href="javascript:show_calendar('aspnetForm.ctl00_ContentPlaceHolder1_txtDate');">
                        <img alt="Calendar Icon" src="Images/Calendar1.jpg" border="0" width="20" height="15"/>
                    </a>&nbsp;&nbsp;
                    <img alt="Delete Icon" onmouseover="window.status='Delete Date';return true;" onclick="javascript:document.aspnetForm.ctl00_ContentPlaceHolder1_txtDate.value = ''"
                        onmouseout="window.status='';return true;" src="Images/delete-icon.png" width="16" />
                </td>
            </tr>
        </asp:Panel>
    </table>
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
    <table>
        <tr>
            <td align="left">
                <font size="4"><b>Choose Report Type</b></font>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table>
                    <tr>
                        <td>
                            <font size="3">
                                <asp:Label ID="Label6" runat="server" Text="Type:" Font-Bold="True" Width="85px" />
                            </font>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlReportType" Width="125px" runat="server">
                                <asp:ListItem value="All" Selected="True">All</asp:ListItem>
                                <asp:ListItem value="Open">Open</asp:ListItem>
                                <asp:ListItem value="Closed">Closed</asp:ListItem>
                                <asp:ListItem value="Assigned">Assigned</asp:ListItem>
                                <asp:ListItem value="Pending">Pending</asp:ListItem>
                                <asp:ListItem value="Dismissed">Dismissed</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
            <td align="left">
                <font size="4"><b>Choose Report Format</b></font>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table>
                    <tr>
                        <td>
                            <font size="3">
                                <asp:Label ID="Label7" runat="server" Text="Format:" Font-Bold="True" Width="85px" />
                            </font>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlReportFormat" Width="125px" runat="server">
                                <asp:ListItem value="HTML" Selected="True">HTML</asp:ListItem>
                                <asp:ListItem value="Mobile">Mobile</asp:ListItem>
                                <asp:ListItem value="GovDelivery">GovDelivery</asp:ListItem>
                                <asp:ListItem value="Excel">Excel</asp:ListItem>
                                <%--<asp:ListItem value="Word">Microsoft Word</asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
            <td align="left">
                <font size="4"><b>Environmental Crimes & DEM INR</b></font>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table>
                    <tr>
                        <td>
                            <font size="3">
                                <asp:Label ID="Label8" runat="server" Text="Remove:" Font-Bold="True" Width="85px" />
                            </font>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbRemove" runat="server" Checked="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
            <td align="left">
                <font size="4"><b>Choose Assigned Agency</b></font>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table>
                    <tr>
                        <td>
                            <font size="3">
                                <asp:Label ID="Label9" runat="server" Text="Agency:" Font-Bold="True" Width="85px" />
                            </font>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAgency" Width="150px" runat="server" DataTextField="Abbreviation" DataValueField="AgencyID" ></asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <div align="center">
        <asp:Button ID="btnRunReport" Text="Run Report" runat="server" />
    </div>
    <br />
</asp:Content>