<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Detailed.aspx.vb" Inherits="DB_DrillDown_Detailed" %>

<html>
<head>
    <title>SERT :: SWO :: Incident Worksheet Count</title>
    <%
        'You need to include the following JS file, if you intend to embed the chart using JavaScript.
        'Embedding using JavaScripts avoids the "Click to Activate..." issue in Internet Explorer
        'When you make your own charts, make sure that the path to this JS file is correct. Else, you would get JavaScript errors.
    %>

    <script language="Javascript" type="text/javascript" src="../FusionCharts/FusionCharts.js"></script>

    <style type="text/css">

	body {
		font-family: Arial, Helvetica, sans-serif;
		font-size: 12px;
	}
	.text{
		font-family: Arial, Helvetica, sans-serif;
		font-size: 12px;
	}
	
	</style>
</head>
<body>
    <center>
        <h2>
            SERT :: SWO :: Incident Worksheet Count</h2>
        <h4>
            Detailed report for the Worksheet Coming Soon</h4>
        <asp:Literal ID="FCLiteral" runat="server"></asp:Literal>    
        
     
    </center>
</body>
</html>
