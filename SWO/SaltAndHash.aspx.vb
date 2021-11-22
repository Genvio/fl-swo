Imports System.Data.SqlClient
Imports System.Data

Partial Class SaltAndHash
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim objConn As New SqlConnection
        Dim objCmd As SqlCommand
        Dim objDBConStringHelp As New DBConStringHelp
        Dim objConn2 As New SqlConnection
        Dim objCmd2 As SqlCommand
        Dim objDR As SqlDataReader

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        'objCmd = New SqlCommand("spSelectUsers", objConn)
        'objCmd.CommandType = CommandType.StoredProcedure
        objCmd = New SqlCommand("select UserID, password from [dbo].[User]", objConn)
        objCmd.CommandType = CommandType.Text
        objDBConStringHelp.PrepareConnection(objConn)
        objDR = objCmd.ExecuteReader()

        Response.Write("<table><tr><td>Salt</td><td>New Version</td></tr>")
        While objDR.Read()
            Dim _salt As String = String.Empty
            Dim ciphertext2 As String

            ciphertext2 = UserValidation.EncryptPassword(objDR("Password"), _salt)
            Response.Write("<tr><td>" & _salt & "</td><td>" & ciphertext2 & "</td></tr>")
            Response.Write("<tr><td>" & _salt & "</td><td>" & UserValidation.DecryptPassword(ciphertext2, _salt) & "</td></tr>")
            objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objCmd2 = New SqlCommand("Update dbo.[User] set SaltedPassword = '" & ciphertext2 & "', Salt = '" & _salt & "' where UserID = " & objDR("UserID"), objConn2)
            Response.Write("<tr><td colspan='2'>Update dbo.[User] set SaltedPassword = '" & ciphertext2 & "', Salt = '" & _salt & "' where UserID = " & objDR("UserID") & "</td></tr>")
            objCmd2.CommandType = CommandType.Text
            objDBConStringHelp.PrepareConnection(objConn2)
            Dim temp As Int32
            temp = objCmd2.ExecuteNonQuery()
            Response.Write("<tr><td colspan='2'>" & temp.ToString() & " row updated</td></tr>")
            objCmd2.Dispose()
            objCmd2 = Nothing
            objDBConStringHelp.FinalizeConnection(objConn2)
        End While

        Response.Write("</table>")
        objDR.Close()
        objCmd.Dispose()
        objCmd = Nothing
        objDBConStringHelp.FinalizeConnection(objConn)
    End Sub
End Class
