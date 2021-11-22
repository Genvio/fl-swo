Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Collections
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text

Partial Class ViewUpdates
    Inherits System.Web.UI.Page

    'Help functions from our App_Code.
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    'For connecting to the database.
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet
    Public objDS2 As New System.Data.DataSet
    Public objDS3 As New System.Data.DataSet
    Public objDS4 As New System.Data.DataSet
    Public objDS5 As New System.Data.DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Dim strBody = New StringBuilder("")
            Dim updateNumber As Integer = 1

            strBody.Append("<table width='100%' align='center' border='1' cellspacing='0' style='border-color:#000000;'>")
            strBody.Append("<tr>")
            strBody.Append("<td colspan='3' align='left'>")
            strBody.Append(" <font size='5'><big><b>Updates</b></big></font> ")
            strBody.Append("</td>")
            'strBody.Append("<td align='left'>")
            'strBody.Append(" <img id='imgLogo' src='Images/SealLogo.jpg' alt='Logo Image' /> ")
            'strBody.Append("</td>")
            strBody.Append("</tr>")

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Open the connection.
            DBConStringHelper.PrepareConnection(objConn)

            objCmd = New SqlCommand("[spSelectUpdateReportByIncidentID]", objConn)
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then
                strBody.Append("<tr>")
                strBody.Append("<td align='center' style='border-color:#000000;' ><font size='5'><b>Number</b></font></td>")
                strBody.Append("<td align='center' style='border-color:#000000;' ><font size='5'><b>Update</b></font></td>")
                strBody.Append("<td align='center' style='border-color:#000000;' ><font size='5'><b>Date</b></font></td>")
                strBody.Append("<td align='center' style='border-color:#000000;' ><font size='5'><b>User</b></font></td>")
                strBody.Append("</tr>")

                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read
                    strBody.Append("<tr>")
                    strBody.Append("<td align='center' style='border-color:#000000;'><font size='5'>" & updateNumber.ToString & "</font></td>")
                    strBody.Append("<td align='left' style='border-color:#000000;'><font size='5'>" & objDR.Item("UpdateReport") & "</font></td>")
                    strBody.Append("<td align='left' style='border-color:#000000;'><font size='5'>" & objDR.Item("UpdateDate") & "</font></td>")
                    strBody.Append("<td align='left' style='border-color:#000000;'><font size='5'>" & objDR.Item("UserName") & "</font></td>")
                    strBody.Append("</tr>")
                    updateNumber += 1
                End While
            Else
                'There are no records.
                strBody.Append("<tr><td colspan='3' style='border-color:#000000;' align='center'>&nbsp;</td><tr>")
                strBody.Append("<tr><td colspan='3' style='border-color:#000000;' align='center'><b>No Records</b></td><tr>")
            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

            strBody.Append("</table>")

            'Display the HTML page.
            Response.Write(strBody.ToString())
        End If
    End Sub
End Class