<%@ Page Language="VB" AutoEventWireup="false" CodeFile="IncidentWorksheetReport.aspx.vb" Inherits="Reports_IncidentWorksheetReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <%-- Datatables js files for excel export, copy and print.  --%>
    <script type="text/javascript" src="https://code.jquery.com/jquery-3.3.1.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.5.2/js/dataTables.buttons.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.5.2/js/buttons.flash.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.36/pdfmake.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.36/vfs_fonts.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.5.1/js/buttons.html5.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.5.1/js/buttons.print.min.js"></script>
    <%-- Datatables css.  --%>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/v/dt/dt-1.10.18/datatables.min.css" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />


</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="myGridView" runat="server" Visible="false"></asp:GridView>
        </div>
        <asp:Panel ID="pnlExcel" runat="server" Visible="false">
            <table id="example" class="display nowrap" style="width: 100%">
                <thead>
                    <tr>
                        <th>Incident ID</th>
                        <th>Lat</th>
                        <th>Long</th>
                        <th>County</th>
                        <th>ReportedToSWOTime</th>
                        <th>Incident Occurred Time</th>
                        <th>Incident Occurred Date</th>
                        <th>Reported To SOW Time</th>
                        <th>Reported To Sow Date</th>
                        <th>Incident Occured Time</th>
                        <th>Incident Type</th>
                        <th>Incident Name</th>
                        <th>Facility NameScene Description</th>
                        <th>City</th>
                        <th>Initial Report</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptExcel" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Incidentid") %></td>
                                <td><%# Eval("Lat") %></td>
                                <td><%# Eval("Long") %></td>
                                <td><%# Eval("AddedCounty") %></td>
                                <td><%# Eval("ReportedToSWOTime") %></td>
                                <td><%# Eval("IncidentOccurredTime") %></td>
                                <td><%# Eval("IncidentOccurredDate") %></td>
                                <td><%# Eval("ReportedToSWOTime") %></td>
                                <td><%# Eval("ReportedToSWODate") %></td>
                                <td><%# Eval("IncidentOccurredTime") %></td>
                                <td><%# Eval("IncidentType") %></td>
                                <td><%# Eval("IncidentName") %></td>
                                <td><%# Eval("FacilityNameSceneDescription") %></td>
                                <td><%# Eval("City") %></td>
                                <td><%# Eval("InitialReport") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </asp:Panel>



    </form>
</body>
</html>
<script type="text/javascript">
    $(document).ready(function () {
        $('#example').DataTable({
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'excel',
                    footer: 'true',
                    text: 'Excel',
                    title: 'DataExport'
                }
           , 'copy', 'print']
        });
    });
</script>
