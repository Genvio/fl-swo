<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="pnlLogin" runat="server">
        <div>
            <table cellpadding="0" cellspacing="0" width="75%" align="center">
                <tr>
                    <td colspan="2" align="center">
                        Please Login 
                    </td>
                </tr>
                <tr>
                    <td>
                        <table align="center" width="100%">
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                    
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    &nbsp; &nbsp; &nbsp;
                                    <asp:Label ID="lblEmail" runat="server" Text="E-mail :"></asp:Label>
                                    <asp:TextBox ID="txtEmailAddress" Width="250px" tabindex="1"  Columns="35" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblPassword"  runat="server" Text="Password :"></asp:Label>
                                    <asp:TextBox runat="server" Width="250px" tabindex="2" ID="txtPassword" Columns="35" TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl1" runat="server" Text="Secret Question : " Visible="false"></asp:Label>
                                    <asp:Label ID="lblSecretQuestion" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl2" runat="server" Text="Secret Answer : " Visible="false"></asp:Label>
                                    <asp:TextBox ID="txtSecretAnswer" Columns="20" runat="server" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" nowrap="nowrap" align="center">
                                  <asp:Panel ID="pnlShowButtons" runat="server" Visible="true">
                                        <input type="button" value="&nbsp;&nbsp;&nbsp;Login&nbsp;&nbsp;&nbsp;" tabindex="3" id="btnLogin" runat="server" onserverclick="btnLogin_Command" />
                                        <input type="button" value="Forgot Password?" tabindex="4" id="lnkForgotPassword" runat="server" onserverclick="lnkForgotPassword_Command" />
                                        <asp:Button runat="server" ID="btnSubmit" Text="Submit" Visible="false" />
                                        <asp:Button runat="server" ID="btnLoginReturn" Text="Login" Visible="false"/>
                                        <br />
                                        <asp:Literal runat="server" ID="litPasswordHelpLink" Visible="false"></asp:Literal>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                     </td>
                  </tr>
               </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlForbidEntry" runat="server" Visible="False">
        <br />
        <table id="Table2" cellpadding="0" cellspacing="0" width="75%" align="center" runat="server">
            <tr>
                <td align="center">
                Too Many Login Attempts
                </td>
            </tr>
            <tr>
                <td>
                    <table align="left" width="100%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblBlockedAccount" runat="server" Text="Due to excessive login attempts, your IP address has been recorded and your account has been deactivated.  Please contact the site administrator: 850-413-9907"></asp:Label>
                            </td>
                            
                        </tr>
                    </table>
                 </td>
              </tr>
           </table>
    </asp:Panel>
</asp:Content>

