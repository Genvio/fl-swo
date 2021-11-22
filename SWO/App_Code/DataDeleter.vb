Imports Microsoft.VisualBasic
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
Imports System.Web.HttpContext
Public Class DataDeleter

    'Help Functions from our App_Code
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp


    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet
    Public objConn2 As New System.Data.SqlClient.SqlConnection
    Public objCmd2 As System.Data.SqlClient.SqlCommand
    Public objDR2 As System.Data.SqlClient.SqlDataReader
    Public objDA2 As System.Data.SqlClient.SqlDataAdapter
    Public objDS2 As New System.Data.DataSet
    Public objDS3 As New System.Data.DataSet
    Public objDS4 As New System.Data.DataSet
    Public objDS5 As New System.Data.DataSet

    Dim ParamId As SqlParameter

    Public MrDataGrabber As New DataGrabber

    'Constructor Expects IncidentID
    Public Sub New()

    End Sub

    ' Destructor
    Protected Overrides Sub Finalize()
        ' Destructor
    End Sub

    Public Sub DeleteOldNonSavedReports()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        DBConStringHelper.PrepareConnection(objConn) 'open the connection
        objCmd = New SqlCommand("[spSelectIncidentIfNotSavedAndTimeExpired]", objConn)

        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()

        If objDR.Read() Then

            'there are records
            objDR.Close()
            objDR = objCmd.ExecuteReader()


            While objDR.Read

                'Current.Response.Write(objDR.Item("IncidentID"))
                'Current.Response.Write("<br>")

                objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                '// Enter the email and password to query/command object.
                objCmd2 = New SqlCommand("spDeleteIncidentIfNotSavedAndTimeExpired", objConn2)
                objCmd2.CommandType = CommandType.StoredProcedure
                objCmd2.Parameters.AddWithValue("@IncidentID ", objDR.Item("IncidentID"))
                '// Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn2)

                '// Execute the command to the DataReader.
                objCmd2.ExecuteNonQuery()
                '// Clean up our command objects and close the connection.
                objCmd2.Dispose()
                objCmd2 = Nothing
                DBConStringHelper.FinalizeConnection(objConn2)

            End While

        End If

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()

    End Sub

End Class
