<%@ Page Title="" Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="MessagesNonAdmin.aspx.vb" Inherits="MessagesNonAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <font size="6">
                    <b>Messages</b>
                </font>
            </td>
        </tr>
    </table>
    <br />
    <table align="center">
        <tr>
            <td>
                <asp:DataGrid ID="dgMessage" runat="server" Width="100%" OnSortCommand="SortMessage" AutoGenerateColumns="false"
                    AllowSorting="True" AllowPaging="false" PagerStyle-HorizontalAlign="center" OnPageIndexChanged="dgMessage_PageIndexChanged"
                    HeaderStyle-CssClass="datagridheader" AlternatingItemStyle-CssClass="datagridalt">
                    <Columns>
                        <asp:HyperLinkColumn Visible="False" Target="_parent" DataNavigateUrlField="MessageID"
                            DataTextField="MessageID" SortExpression="MessageID ASC" HeaderText="MessageID">
                            <HeaderStyle Wrap="False"></HeaderStyle>
                        </asp:HyperLinkColumn>

                        <asp:BoundColumn HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataField="DateCreated" SortExpression="DateCreated DESC" HeaderText="Date Created EST <img src='Images/blue_arrow_down2.jpg' align='absmiddle' border=0>">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                        </asp:BoundColumn>
                        
                        <asp:BoundColumn HeaderStyle-Width="500px" ItemStyle-HorizontalAlign="Center" DataField="Message" SortExpression="Message DESC" HeaderText="Message">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                        </asp:BoundColumn>
                        
                        <asp:BoundColumn HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataField="CreatedBy" SortExpression="CreatedBy DESC"  HeaderText="Created By">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                        </asp:BoundColumn>
                        
                        <%--<asp:BoundColumn HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" DataField="LastUpdatedBy" SortExpression="LastUpdatedBy"  HeaderText="Last Updated By">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                        </asp:BoundColumn>--%>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

