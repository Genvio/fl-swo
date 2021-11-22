<%@ Page Language="VB" AutoEventWireup="false" CodeFile="IncidentWorksheetGraph.aspx.vb" Inherits="Reports_IncidentWorksheetGraph" %>

<html>
    <head>
        <title>SERT :: Incident Worksheet Count by Incident</title>
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
            <h2>Incident Worksheet Count by Incident</h2>
            <asp:Literal ID="Dates" runat="server"></asp:Literal>
            <br />
            <%--<h4>Click on any pie slice to see detailed data.</h4>--%>
            <asp:Literal ID="FCLiteral" runat="server"></asp:Literal>
            <br />
        </center>
    </body>
</html>