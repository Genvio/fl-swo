<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false" CodeFile="ErrorMessage.aspx.vb" Inherits="ErrorMessage"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <table width="100%" align="center">
        <tr>
            <td width="100%" align="center">
                An error has occurred in the Incident Tracker. Tech Services has been notified. Please try logging back in:<a href="https://apps.floridadisaster.org/SWO/">Click Here</a>.
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

