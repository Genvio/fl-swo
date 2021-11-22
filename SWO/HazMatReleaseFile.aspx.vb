Imports System.Runtime.InteropServices
Imports System.Data
Imports System.Data.SqlClient

Partial Class HazMatReleaseFile
    Inherits System.Web.UI.Page

    Public Shared MimeSampleSize As Integer = 256
    Public Shared DefaultMimeType As String = "application/octet-stream"
    'For Connecting to the database
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet

    'Page Load
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim sb As New StringBuilder("")
            sb.Append("SELECT * FROM [dbo].[HazmatReleaseFiles]")
            sb.Append(" WHERE FileID = " & Request("FileID"))
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand(sb.ToString(), objConn)
            objCmd.CommandType = CommandType.Text
            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                Response.ContentType = objDR("ContentType")
                Response.BinaryWrite(objDR("FileData"))
                Response.End()
            End If

            objDR.Close()
            objConn.Close()
        End If

    End Sub


End Class
