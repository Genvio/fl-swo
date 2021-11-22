<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WorksheetByIncidentCounty.aspx.vb" Inherits="Reports_WorksheetByIncidentCounty" %>

<html>
    <head>
        <title>SERT :: Incident Worksheet Count By Incident & County</title>
        <%
            'You need to include the following JS file, if you intend to embed the chart using JavaScript.
            'Embedding using JavaScripts avoids the "Click to Activatee that the path to this JS file is correct. Else, you would get JavaScript errors.
        %>

        <script language="Javascript" type="text/javascript" src="../FusionCharts/FusionCharts.js"></script>

        <style type="text/css">
	        body
	        {
		        font-family: Arial, Helvetica, sans-serif;
		        font-size: 12px;
	        }
	        .text
	        {
		        font-family: Arial, Helvetica, sans-serif;
		        font-size: 12px;
	        }
	    </style>
    </head>
    <body>
        <center>
            <h2>Incident Worksheet Count by Incident & County</h2>
            <asp:Literal ID="Message" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="Dates" runat="server"></asp:Literal>
            <br />
            <%--<h4>Click on any pie slice to see detailed data.</h4>--%>
            <asp:Literal ID="FCLiteral1" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral2" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral3" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral4" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral5" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral6" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral7" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral8" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral9" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral10" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral11" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral12" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral13" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral14" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral15" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral16" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral17" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral18" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral19" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral20" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral21" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral22" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral23" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral24" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral25" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral26" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral27" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral28" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral29" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral30" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral31" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral32" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral33" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral34" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral35" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral36" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral37" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral38" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral39" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral40" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral41" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral42" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral43" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral44" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral45" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral46" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral47" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral48" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral49" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral50" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral51" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral52" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral53" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral54" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral55" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral56" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral57" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral58" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral59" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral60" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral61" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral62" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral63" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral64" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral65" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral66" runat="server"></asp:Literal>
            <br />
            <asp:Literal ID="FCLiteral67" runat="server"></asp:Literal>
            <br />
        </center>
    </body>
</html>