<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="GeoCodeByAdress.aspx.vb" Inherits="GeoCodeByAdress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:Panel ID="pnlShowByZipAndAddress" runat="server" Visible="false" >
    <table width="100%" align="center">
        <tr>
            <td>
              Adress: <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    
    <table width="100%" align="center">
        <tr>
            <td>
               Zip: <asp:TextBox ID="txtZip" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    
    <table width="100%" align="center">
        <tr>
            <td>
                
            </td>
        </tr>
    </table>
    
    <table width="100%" align="center">
        <tr>
            <td>
                
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td>
                <asp:Button ID="btnSubmit" Text="Submit" runat="server" />
            </td>
        </tr>
    </table>
</asp:Panel> 

<asp:Panel ID="pnlShowByStateCityAndAdress" runat="server" Visible="true" >
    <table width="100%" align="center">
        <tr>
            <td>
              Adress: <asp:TextBox ID="txtAddress2" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    
    <table width="100%" align="center">
        <tr>
            <td>
               City: <asp:DropDownList ID="ddlCity" style="background-color:#f4da03" Width="160px"  DataTextField="City" DataValueField="CityID"  runat="server"></asp:DropDownList>
            </td>
        </tr>
    </table>
    
    <table width="100%" align="center">
        <tr>
            <td>
               State <asp:DropDownList ID="ddlState" style="background-color:#f4da03" Width="160px"  DataTextField="State" DataValueField="StateID"  runat="server"></asp:DropDownList> 
            </td>
        </tr>
    </table>
    
    <table width="100%" align="center">
        <tr>
            <td>
                
            </td>
        </tr>
    </table>
        <table width="100%" align="center">
        <tr>
            <td>
                <asp:Button ID="btnSubmit2" Text="Submit" runat="server" />
            </td>
        </tr>
    </table>
</asp:Panel> 
  

    
     <table width="100%" align="center">
        <tr>
            <td>
                Results: <asp:Label ID="lblResults" runat="server"></asp:Label>
            </td>
        </tr>
    </table>


    
</asp:Content>

