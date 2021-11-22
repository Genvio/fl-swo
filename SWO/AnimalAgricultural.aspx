<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="AnimalAgricultural.aspx.vb" Inherits="AnimalAgricultural" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style84
        {
            width: 456px;
        }
        .style86
        {
            width: 452px;
        }
        .style87
        {
            width: 453px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <AJAX:UpdatePanel ID="AJAXUpdatePanel" runat="server">
    <ContentTemplate>
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <b>
                    <font size="6">
                        Animal or Agricultural
                    </font>
                </b>
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" align="center">
        <tr>
            <td align="right">
                <big>
                    <b>
                        Sub-Types:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSubType" AutoPostBack="true" style="background-color:#c2ecde" Width="200px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Animal Issue" Text="Animal Issue"></asp:ListItem>
                    <asp:ListItem Value="Agriculture Issue" Text="Agriculture Issue"></asp:ListItem>
                    <asp:ListItem Value="Crop Issue" Text="Crop Issue"></asp:ListItem>
                    <asp:ListItem Value="Food Supply Issue" Text="Food Supply Issue"></asp:ListItem>
                 </asp:DropDownList>
            </td>
            <td align="right">
                <big>
                    <b>
                        Severity Level:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSeverityLevel"  style="background-color:#c2ecde" Width="200px"  runat="server">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Reported" Text="Reported"></asp:ListItem>
                    <asp:ListItem Value="Confirmed" Text="Confirmed"></asp:ListItem>
                    <asp:ListItem Value="Secondary" Text="Secondary"></asp:ListItem>
                 </asp:DropDownList>
                 
            </td>
        </tr>
        <tr>
            <td align="right">
                <big>
                    <b>
                        Worksheet Name:
                    </b>
                </big>
            </td>
            <td align="left" colspan="3">
                <asp:TextBox ID="txtWorkSheetDescription" Width="710px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <big>
                    <b>
                       Notification:
                    </b>
                </big>
            </td>
            <td align="left" colspan="3">
                <asp:DropDownList ID="ddlNotification" DataTextField="Description" DataValueField="IncidentTypeLevelID"  style="background-color:#c2ecde" Width="715px"  runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <br />
    <table align="center" width="100%">
        <tr>
            <td style="background-color: #d4d4d4" align="left">
                <h1>
                    Information
                </h1>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowAnimal" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        What animal(s) are affected?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAnimalAffected" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        What type of disease, if known?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAnimalDiseaseType" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Number of animals infected?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAnimalInfected" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Number of animals deceased?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAnimalsDeceased" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Tests or examinations are planned or occuring?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAnimalTestExaminations" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Is there a quarantine in effect?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlAnimalQuarantine"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowAnimalQuarantineText" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Describe area, listing streets or landmarks:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAnimalQuarantineText" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Are any humans affected?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlAnimalHumansAffected"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowAnimalHumansAffectedText" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Number and Severity of Illness:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAnimalHumansAffectedText" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <table width="100%" align="center" style="display:none">
        <tr>
            <td align="right" class="style86">
                <big>
                    <b>
                        Are there any human fatalities?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlAnimalHumanFatalities"  
                    style="background-color:#c2ecde; margin-left: 4px;" Width="175px"  
                    runat="server" AutoPostBack="true">
                    <asp:ListItem Value="Select an Option" Text="Select an Option" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Unknown" Text="Unknown"></asp:ListItem>
                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowAnimalHumanFatalitiesText" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Number and Information:
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAnimalHumanFatalitiesText" Width="500px"  runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="pnlAgriculturalDiseaseCropFailure" runat="server" Visible="false">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        What crop(s) are affected?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtADCFcropsAffected" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        What type of disease, if known?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtADCFdiseaseType" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Number of acres affected?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtADCFacresAffected" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="pnlFoodSupplyContamination" runat="server" Visible="false">
     <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        What type / brand of food?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFSCtypeBrand" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
     <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Where was it manufactured/packed?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFSCmanufacturedPacked" Width="500px" style="background-color:#c2ecde" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
     <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Affected lot number(s)?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFSCaffectedLotNumber" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
     <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Affected date range?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFSCaffectedDateRange" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
     <table width="100%" align="center">
        <tr>
            <td align="right" class="style84">
                <big>
                    <b>
                        Has a recall been issued?
                    </b>
                </big>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFSCrecallIssued" Width="500px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <br />
    <br />
    <table width="100%" align="center">
        <tr>
            <td align="center">
                &nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;
                <input type="button" value="Save Incident" id="btnSave" runat="server" onserverclick="btnSave_Command" />
                &nbsp;&nbsp;&nbsp;
                <input type="button" value="Cancel" id="btnCancel" runat="server" onserverclick="btnCancel_Command" />
                &nbsp;&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlMessage" runat="server" Visible="false">
    <table width="100%">
        <tr>
            <td align="left" colspan="2">
                <div class="feature">
                    <table width="100%">
                        <tr>
                            <td valign="top" align="center">
                                <table width="100%">
                                    <tr align="left">
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
    </table>
    </asp:Panel>

    <br />
    <br />
    </ContentTemplate>
    </AJAX:UpdatePanel>
</asp:Content>

