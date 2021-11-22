<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WorksheetCountByCountyGraph.aspx.vb" Inherits="Reports_WorksheetCountByCountyGraph" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>SERT :: SWO :: Worksheet Count By County</title>
    <link href="../assets/ui/css/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../assets/ui/js/jquery-1.4.2.min.js"></script>

    <script type="text/javascript" src="../assets/ui/js/lib.js"></script>

    <script language="Javascript" src="../FusionCharts/FusionCharts.js"></script>

    <style type="text/css">
        h2.headline
        {
            font: normal 110%/137.5% "Trebuchet MS" , Arial, Helvetica, sans-serif;
            padding: 0;
            margin: 25px 0 25px 0;
            color: #7d7c8b;
            text-align: center;
        }
        p.small
        {
            font: normal 68.75%/150% Verdana, Geneva, sans-serif;
            color: #919191;
            padding: 0;
            margin: 0 auto;
            width: 664px;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id='form1' name='form1' method='post' runat="server">
        <div id="wrapper">
            <div class="content-area">
                <div id="content-area-inner-main">
                    <h2 class="headline">
                        <asp:Label runat="server" ID="lblCounty" ></asp:Label>
                    </h2>
                    <div class="gen-chart-render">
                        <asp:Literal ID="LiteralCounty" runat="server"></asp:Literal>
                    </div>
                    <div class="clear">
                    </div>
                    <p>
                        &nbsp;
                    </p>
                    <div class="underline-dull">
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>