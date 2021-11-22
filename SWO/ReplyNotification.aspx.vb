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
Imports System.Web.Services
Imports System.IO

Partial Class ReplyNotification
    Inherits System.Web.UI.Page


    'Help Functions from our App_Code
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    Public objDataGridFunctions As New DataGridFunctions

    'For Connecting to the database
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet

    Dim ParamId As SqlParameter

    Public AuditHelper As New AuditHelp

    Public MrDataGrabber As New DataGrabber

    Dim globalRecordCount As Integer
    Dim globalAuditAction As String = ""
    Dim globalHasErrors As Boolean = False
    Dim globalMessage As String
    Dim globalCurrentStep As Integer
    Dim globalIsSaved As Boolean = False
    Dim globalIsPreSaved As Boolean = False
    Dim globalAction As String
    Dim globalParameter As String
    Const js As String = "TADDScript.js"
    Dim globalResults As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        DBConStringHelper.PrepareConnection(objConn) 'open the connection
        objCmd = New SqlCommand("[spSelectReplyNotificationByIncidentID]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()

        globalResults = globalResults + "<table align='center' width='100%'cellspacing='0' border='1' style='border-color:#000000'> "

        If objDR.Read() Then
            'there are records
            objDR.Close()
            objDR = objCmd.ExecuteReader()


            While objDR.Read
                globalResults = globalResults + "   <tr>"
                globalResults = globalResults + "       <td width='40%' align='center' style='border-color:#000000; border-style:solid'>"
                globalResults = globalResults + "       " & objDR.Item("Notification")
                globalResults = globalResults + "       </td>"
                globalResults = globalResults + "       <td width='40%' align='center' style='border-color:#000000; border-style:solid'>"
                globalResults = globalResults + "       " & objDR.Item("Comment")
                globalResults = globalResults + "       </td>"
                globalResults = globalResults + "       <td width='10%' align='center' style='border-color:#000000; border-style:solid'>"
                globalResults = globalResults + "       " & objDR.Item("Date")
                globalResults = globalResults + "       </td>"
                globalResults = globalResults + "       <td width='10%' align='center' style='border-color:#000000; border-style:solid'>"
                globalResults = globalResults + "       " & MrDataGrabber.GrabUserFullNameByUserID(objDR.Item("UserID"))
                globalResults = globalResults + "       </td>"
                globalResults = globalResults + "   </tr>"
            End While


        Else
            globalResults = globalResults + "   <tr>"
            globalResults = globalResults + "       <td align='center' style='border-color:#000000; border-style:solid'>"
            globalResults = globalResults + "  No Records at This Time     "
            globalResults = globalResults + "       </td>"
            globalResults = globalResults + "   </tr>"
        End If

        globalResults = globalResults + "</table>"

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()

        lblResults.Text = globalResults

    End Sub

End Class