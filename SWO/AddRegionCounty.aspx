<%@ Page Language="VB" MasterPageFile="~/LoggedIn.master" AutoEventWireup="false" CodeFile="AddRegionCounty.aspx.vb" Inherits="AddRegionCounty" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<table>
    <tr>
        <td align="right">
            <asp:Button ID="btnSave" Text="Save" runat="server" />
        </td>
        <td align="left">
            <asp:Button ID="btnCancel" Text="Cancel" runat="server" />
        </td>
    </tr>
    <tr>
        <td style="background-color: #d4d4d4">
            <asp:CheckBox ID="cbxStatewide" runat="server" AutoPostBack="true" />
        </td>
        <td align="left" style="background-color: #d4d4d4">
            Statewide
        </td>
    </tr>
    <tr>
        <td style="background-color: #d4d4d4">
            <asp:CheckBox ID="cbxRegion1" runat="server" AutoPostBack="true" />
        </td>
        <td align="left" style="background-color: #d4d4d4">
            Region 1
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxBay" runat="server" />
        </td>
        <td align="left">
            Bay
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxCalhoun" runat="server" />
        </td>
        <td align="left">
            Calhoun
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxEscambia" runat="server" />
        </td>
        <td align="left">
            Escambia
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxGulf" runat="server" />
        </td>
        <td align="left">
            Gulf
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxHolmes" runat="server" />
        </td>
        <td align="left">
            Holmes       
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxJackson" runat="server" />
        </td>
        <td align="left">
            Jackson       
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxOkaloosa" runat="server" />
        </td>
        <td align="left">
            Okaloosa
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxSantaRosa" runat="server" />
        </td>
        <td align="left">
            Santa Rosa
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxWalton" runat="server" />
        </td>
        <td align="left">
            Walton
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxWashington" runat="server" />
        </td>
        <td align="left">
            Washington
        </td>
    </tr>
    <tr>
        <td style="background-color: #d4d4d4">
            <asp:CheckBox ID="cbxRegion2" runat="server" AutoPostBack="true" />
        </td>
        <td align="left" style="background-color: #d4d4d4">
            Region 2
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxColumbia" runat="server" />
        </td>
        <td align="left">
            Columbia
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxDixie" runat="server" />
        </td>
        <td align="left">
            Dixie
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxFranklin" runat="server" />
        </td>
        <td align="left">
            Franklin
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxGadsden" runat="server" />
        </td>
        <td align="left">
            Gadsden
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxHamilton" runat="server" />
        </td>
        <td align="left">
            Hamilton
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxJefferson" runat="server" />
        </td>
        <td align="left">
            Jefferson
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxLafayette" runat="server" />
        </td>
        <td align="left">
            Lafayette
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxLeon" runat="server" />
        </td>
        <td align="left">
            Leon
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxLiberty" runat="server" />
        </td>
        <td align="left">
            Liberty
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxMadison" runat="server" />
        </td>
        <td align="left">
            Madison
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxSuwannee" runat="server" />
        </td>
        <td align="left">
            Suwannee
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxTaylor" runat="server" />
        </td>
        <td align="left">
            Taylor
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxWakulla" runat="server" />
        </td>
        <td align="left">
            Wakulla
        </td>
    </tr>
    <tr>
        <td style="background-color: #d4d4d4">
            <asp:CheckBox ID="cbxRegion3" runat="server" AutoPostBack="true" />
        </td>
        <td align="left" style="background-color: #d4d4d4">
            Region 3
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxAlachua" runat="server" />
        </td>
        <td align="left">
            Alachua
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxBaker" runat="server" />
        </td>
        <td align="left">
            Baker
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxBradford" runat="server" />
        </td>
        <td align="left">
            Bradford
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxClay" runat="server" />
        </td>
        <td align="left">
            Clay
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxDuval" runat="server" />
        </td>
        <td align="left">
            Duval
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxFlagler" runat="server" />
        </td>
        <td align="left">
            Flagler
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxGilchrist" runat="server" />
        </td>
        <td align="left">
            Gilchrist
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxLevy" runat="server" />
        </td>
        <td align="left">
            Levy
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxMarion" runat="server" />
        </td>
        <td align="left">
            Marion
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxNassau" runat="server" />
        </td>
        <td align="left">
            Nassau
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxPutnam" runat="server" />
        </td>
        <td align="left">
            Putnam
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxStJohns" runat="server" />
        </td>
        <td align="left">
            St. Johns
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxUnion" runat="server" />
        </td>
        <td align="left">
            Union
        </td>
    </tr>
    <tr>
        <td style="background-color: #d4d4d4">
            <asp:CheckBox ID="cbxRegion4" runat="server" AutoPostBack="true" />
        </td>
        <td align="left" style="background-color: #d4d4d4">
            Region 4
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxCitrus" runat="server" />
        </td>
        <td align="left">
            Citrus
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxHardee" runat="server" />
        </td>
        <td align="left">
            Hardee
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxHernando" runat="server" />
        </td>
        <td align="left">
            Hernando
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxHillsborough" runat="server" />
        </td>
        <td align="left">
            Hillsborough
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxPasco" runat="server" />
        </td>
        <td align="left">
            Pasco
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxPinellas" runat="server" />
        </td>
        <td align="left">
            Pinellas
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxPolk" runat="server" />
        </td>
        <td align="left">
            Polk
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxSumter" runat="server" />
        </td>
        <td align="left">
            Sumter
        </td>
    </tr>
    <tr>
        <td style="background-color: #d4d4d4">
            <asp:CheckBox ID="cbxRegion5" runat="server" AutoPostBack="true"  />
        </td>
        <td align="left" style="background-color: #d4d4d4">
            Region 5
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxBrevard" runat="server" />
        </td>
        <td align="left">
            Brevard
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxIndianRiver" runat="server" />
        </td>
        <td align="left">
            Indian River
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxLake" runat="server" />
        </td>
        <td align="left">
            Lake
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxMartin" runat="server" />
        </td>
        <td align="left">
            Martin
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxOrange" runat="server" />
        </td>
        <td align="left">
            Orange
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxOsceola" runat="server" />
        </td>
        <td align="left">
            Osceola
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxSeminole" runat="server" />
        </td>
        <td align="left">
            Seminole
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxStLucie" runat="server" />
        </td>
        <td align="left">
            St. Lucie
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxVolusia" runat="server" />
        </td>
        <td align="left">
            Volusia
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxRegion6" runat="server" style="background-color: #d4d4d4" AutoPostBack="true" />
        </td>
        <td align="left" style="background-color: #d4d4d4">
            Region 6
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxCharlotte" runat="server" />
        </td>
        <td align="left">
            Charlotte
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxCollier" runat="server" />
        </td>
        <td align="left">
            Collier
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxDeSoto" runat="server" />
        </td>
        <td align="left">
            DeSoto
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxGlades" runat="server" />
        </td>
        <td align="left">
            Glades
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxHendry" runat="server" />
        </td>
        <td align="left">
            Hendry
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxHighlands" runat="server" />
        </td>
        <td align="left">
            Highlands
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxLee" runat="server" />
        </td>
        <td align="left">
            Lee
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxManatee" runat="server" />
        </td>
        <td align="left">
            Manatee
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxOkeechobee" runat="server" />
        </td>
        <td align="left">
            Okeechobee
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxSarasota" runat="server" />
        </td>
        <td align="left">
            Sarasota
        </td>
    </tr>
    <tr>
        <td style="background-color: #d4d4d4">
            <asp:CheckBox ID="cbxRegion7" runat="server" AutoPostBack="true" />
        </td>
        <td align="left" style="background-color: #d4d4d4">
            Region 7
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxBroward" runat="server" />
        </td>
        <td align="left">
            Broward
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxMiamiDade" runat="server" />
        </td>
        <td align="left">
            Miami-Dade
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxMonroe" runat="server" />
        </td>
        <td align="left">
            Monroe
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbxPalmBeach" runat="server" />
        </td>
        <td align="left">
            Palm Beach
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Button ID="btnSave2" Text="Save" runat="server" />
        </td>
        <td align="left">
            <asp:Button ID="btnCancel2" Text="Cancel" runat="server" />
        </td>
    </tr>
</table>

</asp:Content>

