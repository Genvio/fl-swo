<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="EditUser.aspx.vb" Inherits="EditUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .button
        {
            width: 90px;
        }
    </style>
    <style type="text/css">
        /****************************
            Password validation
        ***************************/
        #passwordChecklist {
            border:1px solid #555;
            padding:10px 10px 10px 20px;
            margin-left:0;
        }

        #passwordChecklist .passwordRequirement {
            width:350px;
            padding:5px 10px 5px 0;
        }

        .passwordRequirement.good {
            background-image:url(images/check.png);
            background-position:100% 50%;
            background-repeat:no-repeat;
        }

        .passwordRequirement.bad {
            background-image:url(images/times.png);
            background-position:99% 50%;
            background-repeat:no-repeat;
        }

        #passed {
            font-weight:bold;
            margin-right:50px;
        }

        #passwordChecklist .PWHeading {
            background-image:none !important;
            margin-left:-10px;
            padding:10px 0px 0px 0px;
            font-weight:bold;
            font-size:13px;
        }

        #passwordChecklist .PWHeading + .passwordRequirement {
            padding:2px 0px 5px 0px;
        }
    </style>
	<script type="text/javascript" src="Includes/JQUERY/js/jquery-1.4.2.min.js"></script>
	<script type="text/javascript" src="Includes/JQUERY/js/jquery-ui-1.8.2.custom.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtPassword.ClientID%>").keyup(function () {
                //count of "optional" requirements (must hit 3 of 4)
                var passed = 0;
                var pass = document.getElementById("<%=txtPassword.ClientID%>").value;

                if (pass.length == 0) {
                    $(".passwordRequirement").removeClass("good bad");
                }

                else {
                    //8 characters check
                    if (pass.length >= 8) {
                        $("#8char").addClass("good");
                        $("#8char").removeClass("bad");
                    } else {
                        $("#8char").removeClass("good");
                        $("#8char").addClass("bad");
                    }

                    //Numeric Check
                    if (/\d/.test(pass) == true) {
                        $("#numeric").addClass("good");
                        $("#numeric").removeClass("bad");
                        passed++;
                    } else {
                        $("#numeric").removeClass("good");
                        $("#numeric").addClass("bad");
                    }

                    //Non Alpha Check
                    if (/.*[^a-zA-z0-9].*/g.test(pass)) {
                        $("#alpha").addClass("good");
                        $("#alpha").removeClass("bad");
                        passed++;
                    } else {
                        $("#alpha").removeClass("good");
                        $("#alpha").addClass("bad");
                    }
                    //Upper Case Check
                    if (/.*[A-Z].*/g.test(pass)) {
                        $("#upperCase").addClass("good");
                        $("#upperCase").removeClass("bad");
                        passed++;
                    } else {
                        $("#upperCase").removeClass("good");
                        $("#upperCase").addClass("bad");
                    }
                    //Lower Case check
                    if (/.*[a-z].*/g.test(pass)) {
                        $("#lowerCase").addClass("good");
                        $("#lowerCase").removeClass("bad");
                        passed++;
                    } else {
                        $("#lowerCase").removeClass("good");
                        $("#lowerCase").addClass("bad");
                    }
                }
            })

            $("#<%=txtConfirmPassword.ClientID%>").keyup(function () {
                var pass = document.getElementById("<%=txtPassword.ClientID%>").value;
                var passConfirm = document.getElementById("<%=txtConfirmPassword.ClientID%>").value;

                if (pass == passConfirm && pass.length > 0) {
                    $("#match").addClass("good");
                    $("#match").removeClass("bad");
                } else {
                    $("#match").removeClass("good");
                    $("#match").addClass("bad");
                }
            })
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6">
                    <b><asp:Label ID="lblAddEdit" runat="server" />User</b>
                </font>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlMessage" runat="server" Visible="false">
        <table width="100%">
            <tr>
                <td align="left" colspan="2">
                    <div class="feature">
                        <table width="100%">
                            <tr>
                                <%--<td width="20%"  align="right">
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
        </table>
    </asp:Panel>
    <div runat="server" id="divUserForm">
        <table width="100%" align="center">
            <tr>
                <td align="right" width="40%">
                    <b><font size="3">First Name:</font></b>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtFirstName" runat="server" Width="300px" />
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" width="40%">
                    <b><font size="3">Last Name:</font></b>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtLastName" runat="server" Width="300px" />
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" width="40%">
                    <b><font size="3">Username/Email:</font></b>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtEmail" runat="server" Width="300px" />
                </td>
            </tr>
        </table>
        <table runat="server" id="tblPassword" width="100%" align="center">
            <tr>
                <td align="right" width="40%">
                    <b><font size="3">Password:</font></b>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtPassword" runat="server" Width="300px" TextMode="Password" />
                </td>
            </tr>
        </table>
       <table runat="server" id="tblPasswordConfirm" width="100%" align="center">
            <tr>
                <td align="right" width="40%">
                    <b><font size="3">Confirm Password:</font></b>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtConfirmPassword" runat="server" Width="300px" TextMode="Password" />
                </td>
            </tr>
        </table>
       <table runat="server" id="tblPasswordReset" width="100%" align="center">
            <tr>
                <td align="right" width="40%">
                    <b><font size="3">Reset Password:</font></b>
                </td>
                <td align="left">
                    <asp:Button runat="server" ID="btnResetPassword" Text="Send Request" OnClientClick="return confirm('Send password reset request to this user?')" />
                    <asp:Label runat="server" ID="lblResetPassword" Width="300px"></asp:Label>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" width="40%">
                    <b><font size="3">Secret Question:</font></b>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlSecretQuestion" Width="306px" DataTextField="SecretQuestion" DataValueField="SecretQuestionID" runat="server" />
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" width="40%">
                    <b><font size="3">Secret Answer:</font></b>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtSecretAnswer" runat="server" Width="300px" TextMode="Password" />
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" width="40%">
                    <b><font size="3">Agency:</font></b>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlAgency" Width="306px" DataTextField="Agency" DataValueField="AgencyID" runat="server" />
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" width="40%">
                    <b><font size="3">User Level:</font></b>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlUserLevel" Width="306px" DataTextField="UserLevel" DataValueField="UserLevelID" runat="server" />
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" width="40%">
                    <b><font size="3">Is Active:</font></b>
                </td>
                <td align="left">
                    <asp:CheckBox ID="chkIsActive" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div runat="server" id="divPasswordValidation">
        <div id="passwordChecklist" class="seven columns" style="text-align: left">
            <div class="passwordRequirement PWHeading" id="passed">
                Passwords must have at least 3 of the following</div>
            <div class="passwordRequirement" id="upperCase">
                Upper case English letters :</div>
            <div class="passwordRequirement" id="lowerCase">
                Lower case English letters :</div>
            <div class="passwordRequirement" id="numeric">
                At least 1 number :</div>
            <div class="passwordRequirement" id="alpha">
                At least 1 non-alpha numeric character :</div>
            <div class="passwordRequirement PWHeading" id="required">
                Password length and a match are required</div>
            <div class="passwordRequirement" id="8char">
                At least 8 characters :</div>
            <div class="passwordRequirement" id="match">
                Passwords match :</div>
        </div>
    </div>
    <br />
    <asp:Panel ID="pnlShowIncidentTypes" runat="server" Visible="false">
    <br />
        <table align="center" width="100%" cellspacing="0" border="1" style="border-color:#000000">
            <tr>
                <td align="left" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                    <b>Incident Worksheets</b>
                    <asp:Button runat="server" ID="btnRefreshWorksheets" Text="Refresh" />
                </td>
                <td align="center" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                    <b><asp:DropDownList ID="ddlIncidentType" Width="250px" style="background-color:#c2ecde" DataTextField="IncidentType" DataValueField="IncidentTypeID" runat="server" /></b>
                </td>
                <td align="center" style="background-color: #d4d4d4;border-color:#000000; border-style:solid">
                    <b><asp:Button ID="btnAddIncidentType" runat="server" Text="Add Worksheet Type" /></b>
                </td>
            </tr>
        </table>
        <asp:Panel runat="server" ID="pnlShowIncidentTypeGrid" Visible="false">
            <table align="center" width="100%">
               <tr>
                    <td>
                        <asp:DataGrid ID="IncidentTypeUserDataGrid" runat="server" Width="100%"
                            AutoGenerateColumns="false" AllowPaging="True" PageSize="100" PagerStyle-HorizontalAlign="center">
                            <Columns>
                                <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="IncidentTypeUserID"
                                    DataTextField="IncidentTypeUserID" SortExpression="IncidentTypeUserID ASC" HeaderText="IncidentTypeUserID">
                                    <HeaderStyle Wrap="False" />
                                </asp:HyperLinkColumn>
                            
                                <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <a href="EditUser.aspx?IncidentTypeUserID=<%# Container.dataitem("IncidentTypeUserID")%>&UserID=<%# Container.dataitem("UserID")%>&Action=Delete&Parameter=IncidentType"><img src="Images/delete-icon.png" alt="Delete Incident Worksheet" border="0" width="16" height="16" onclick="javascript:return confirm('Are you sure you want to delete this Incident Worksheet?')" title="Delete Incident Worksheet" /></a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            
                                <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="IncidentType" SortExpression="IncidentType" HeaderText="<b><u>&nbsp; Worksheet Type &nbsp; </u><b/>">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <br />
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <input type="button" class="button" value="Add" id="btnSave" runat="server" onserverclick="btnSave_Command" />
                &nbsp;&nbsp;&nbsp;
                <input type="button" class="button" value="Cancel" id="btnCancel" runat="server" onserverclick="btnCancel_Command" />
            </td>
        </tr>
    </table>
    <br />
</asp:Content>