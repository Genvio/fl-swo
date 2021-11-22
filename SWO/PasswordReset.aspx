<%@ Page Title="" Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false" CodeFile="PasswordReset.aspx.vb" Inherits="PasswordReset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
                $("#<%=txtPassword1.ClientID%>").keyup(function () {
                    //count of "optional" requirements (must hit 3 of 4)
                    var passed = 0;
                    var pass = document.getElementById("<%=txtPassword1.ClientID%>").value;

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

                $("#<%=txtPassword2.ClientID%>").keyup(function () {
                    var pass = document.getElementById("<%=txtPassword1.ClientID%>").value;
                    var passConfirm = document.getElementById("<%=txtPassword2.ClientID%>").value;

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
    <asp:Panel ID="pnlResetPassword" runat="server">
        <div style="float:left">
            <table>
                <tr>
                    <td>Enter your new password:</td>
                    <td>
                        <asp:TextBox runat="server" Width="250px" ID="txtPassword1" Columns="35" TextMode="Password" MaxLength="25"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Confirm password:</td>
                    <td>
                        <asp:TextBox runat="server" Width="250px" ID="txtPassword2" Columns="35" TextMode="Password" MaxLength="25"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button runat="server" ID="btnSubmit" Text="Submit"/>
                        <br />
                        <asp:Label runat="server" ID="lblNewPasswordError" ForeColor="Red" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div style="float:right">
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
    </asp:Panel>

    <asp:Panel ID="pnlRequestExpired" runat="server" Visible="false">
        <asp:Label runat="server" ID="lblProblemExplanation"></asp:Label>
        Return to the <a href="Default.aspx">login page</a> to begin a new request.<br />
    </asp:Panel>

    <asp:Panel ID="pnlResetSuccess" runat="server" Visible="false">
        Your password has been reset. <a href="Default.aspx">Click here</a> to return to the login page.
    </asp:Panel>
</asp:Content>

