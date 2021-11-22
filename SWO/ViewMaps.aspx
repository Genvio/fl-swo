<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false" CodeFile="ViewMaps.aspx.vb" Inherits="ViewMaps" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="pnlShowMaps" runat="server" Visible="false">
        <br />
        <br />
        <center>
            <iframe src="https://floridadisaster.maps.arcgis.com/apps/webappviewer/index.html?id=74f3e78117fd44b28ffec5adc30c6024&scale=2000&marker= <%=Request("Long") %>,<%=Request("Lat") %>" style="border: 0px #ffffff none;" name="myiFrame" scrolling="no" frameborder="1" marginheight="0px" marginwidth="0px" height="400px" width="600px" allowfullscreen></iframe>
         
        <%--    <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3439.5439433581014!2d<%=Request("Lat") %>!3d<%=Request("Long") %>!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x0!2zMzDCsDI2JzU2LjUiTiA4NMKwMTMnNTMuNiJX!5e0!3m2!1sen!2sus!4v1547160398733" width="400" height="400" frameborder="0" style="border: 0" allowfullscreen></iframe>--%>
        </center>
        <table width="100%" align="center">
            <tr>
                <td align="right">
                    <asp:HyperLink ID="imgGmapOverview" runat="server" AlternateText="Google Map Overview" Width="325px" Height="275px" Target="_blank"> 
                    Google Map Overview
                    </asp:HyperLink>
                </td>

                <td>
                    <asp:HyperLink ID="imgGmapDetail" runat="server" AlternateText="Google Map Detail" Width="325px" Height="275px" Target="_blank"> 
                       View Larger Map
                    </asp:HyperLink>
                </td>
            </tr>
        </table>

       <%-- <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3439.5439433581014!2d-84.23155!3d30.449017!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x0!2zMzDCsDI2JzU2LjUiTiA4NMKwMTMnNTMuNiJX!5e0!3m2!1sen!2sus!4v1547160398733" width="400" height="400" frameborder="0" style="border: 0" allowfullscreen></iframe>--%>

    </asp:Panel>
    <asp:Panel ID="pnlShowMessage" runat="server" Visible="false">
        <table width="100%" align="center">
            <tr>
                <td align="center">
                    <big>No Maps Available</big>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

