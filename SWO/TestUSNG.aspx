<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TestUSNG.aspx.vb" Inherits="TestUSNG" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <asp:TextBox ID="TextBoxUSNG" runat="server" Width="190px">17R MA 12345 67890</asp:TextBox>
        <br />
        <br />
        <asp:Button ID="ButtonToLL" runat="server" Text="To LL" />
        <br />
        <br />
        <asp:Literal ID="LiteralLL" runat="server"></asp:Literal>
        <br />
        <hr />
        <br />
        Coordinates 
        <asp:TextBox ID="TextBoxLL" runat="server" Width="307px">31, -84</asp:TextBox>
        &nbsp;(latitude, longitude)<br />
        <br />
        Precision 
        <asp:TextBox ID="TextBoxPrecision" runat="server" Width="307px">4</asp:TextBox>
        <br />
        <br />
        <asp:Button ID="ButtonToUSNG" runat="server" Text="To USNG" />
        <br />
        <br />
        <asp:Literal ID="LiteralUSNG" runat="server"></asp:Literal>
        <br />
    </div>
    </form>
</body>
</html>
