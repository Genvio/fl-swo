<%@ Page Title="" Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="MyProfile.aspx.vb" Inherits="MyProfile" %>

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
                    <b>My Profile</b>
                </font>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblConfirmationMessage" runat="server" Visible="false"></asp:Label>
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
    <div style="width: 55%; float:left">
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
                    <asp:TextBox ID="txtEmail" runat="server" Width="300px" Enabled="false" />
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td align="right" width="40%">
                    <b><font size="3">Current Password:</font></b>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtCurrentPassword" runat="server" Width="300px" TextMode="Password" />
                </td>
            </tr>
        </table>
        <div id="divShowChangePassword" runat="server">
            <table width="100%" align="center">
                <tr>
                    <td align="center" width="100%" colspan="2">
                        <a href="#" onclick="document.getElementById( '<%=divNewPasswordRequirements.ClientID %>' ).style.backgroundColor = '#FFFFFF'; document.getElementById( '<%=divNewPasswordRequirements.ClientID %>' ).style.color = 'Black'; document.getElementById( '<%=hidChangingPassword.ClientID %>' ).value = 'true'; document.getElementById( '<%=divShowChangePassword.ClientID %>' ).style.display = 'none'; document.getElementById( '<%=divChangePassword.ClientID %>' ).style.display = 'inline'; return false;">Click here to change your password</a>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divChangePassword" runat="server" style="display:none">
            <table width="100%" align="center">
                <tr>
                    <td align="right" width="40%">
                        <b><font size="3">New Password:</font></b>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtPassword" runat="server" Width="300px" TextMode="Password" />
                        <asp:HiddenField ID="hidChangingPassword" runat="server" Value="false" />
                    </td>
                </tr>
            </table>
           <table width="100%" align="center">
                <tr>
                    <td align="right" width="40%">
                        <b><font size="3">Confirm New Password:</font></b>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtConfirmPassword" runat="server" Width="300px" TextMode="Password" />
                    </td>
                </tr>
            </table>
        </div>
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
                    <asp:DropDownList ID="ddlAgency" Width="306px" DataTextField="Agency" DataValueField="AgencyID" runat="server" Enabled="false" />
                </td>
            </tr>
        </table>
    </div>
    <div id="divNewPasswordRequirements" runat="server" style="margin-left: 55%; color:Gray; background-color: #EBEBE4;">
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
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <input type="button" class="button" value="Save" id="btnSave" runat="server" onserverclick="btnSave_Command"  />
                &nbsp;&nbsp;&nbsp;
                <input type="button" class="button" value="Cancel" id="btnCancel" runat="server" onserverclick="btnCancel_Command" />
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

