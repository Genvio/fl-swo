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

Public Class AuditHelp
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    Public objDataGridFunctions As New DataGridFunctions

    'For Connecting to the database.
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet

    Dim globalHasErrors As Boolean = False
    Dim globalMessage As String
    Dim oCookie As System.Web.HttpCookie

    Public Sub InsertAudit(ByVal UserID As String, ByVal Action As String, ByVal AuditTypeID As String)
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Enter the email and password to query/command object.
        objCmd = New SqlCommand("spInsertAudit", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@UserID", UserID)
        objCmd.Parameters.AddWithValue("@Action", Action)
        objCmd.Parameters.AddWithValue("@AuditDate", Now)
        objCmd.Parameters.AddWithValue("@AuditTypeID", AuditTypeID)

        'Open the connection using the connection string.
        DBConStringHelper.PrepareConnection(objConn)

        'Execute the command to the DataReader.
        objCmd.ExecuteNonQuery()

        'Clean up our command objects and close the connection.
        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)
    End Sub


    Public Function GetUserInfo(ByVal UserID As String) As String
        Dim FirstName As String = ""
        Dim LastName As String = ""
        Dim UserNameEmail As String = ""
        Dim ReturnInfo As String = ""

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectUserByUserID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@UserID", UserID)

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            FirstName = HelpFunction.Convertdbnulls(objDR("FirstName"))
            LastName = HelpFunction.Convertdbnulls(objDR("LastName"))
            UserNameEmail = HelpFunction.Convertdbnulls(objDR("Email"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        ReturnInfo = "UserName/Email = " & UserNameEmail & "Name = " & LastName & " , " & FirstName

        Return ReturnInfo
    End Function

    Public Function GetInfo(ByVal StorageProcedure As String, ByVal Parameter As String, ByVal Parameter2 As String) As String
        Dim ReturnInfo As String = ""

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand(StorageProcedure, objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@" & Parameter, Parameter2)

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            ReturnInfo = HelpFunction.Convertdbnulls(objDR(Parameter2))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        Return ReturnInfo
    End Function


    Public Sub InsertIncidentAudit(ByVal IncidentID As String, ByVal UserID As String, ByVal UpdateChange As String, ByVal AuditTypeID As String)
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Enter the email and password to query/command object.
        objCmd = New SqlCommand("spInsertIncidentUpdate", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", IncidentID)
        objCmd.Parameters.AddWithValue("@Date", Now)
        objCmd.Parameters.AddWithValue("@UserID", UserID)
        objCmd.Parameters.AddWithValue("@UpdateChange", UpdateChange)
        objCmd.Parameters.AddWithValue("@AuditTypeID", AuditTypeID)

        'Open the connection using the connection string.
        DBConStringHelper.PrepareConnection(objConn)

        'Execute the command to the DataReader.
        objCmd.ExecuteNonQuery()

        'Clean up our command objects and close the connection.
        objCmd.Dispose()
        objCmd = Nothing

        DBConStringHelper.FinalizeConnection(objConn)
    End Sub

    Public Sub InsertReportUpdate(ByVal IncidentID As String, ByVal ReportUpdate As String, ByVal UserID As String)
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Enter the email and password to query/command object.
        objCmd = New SqlCommand("spInsertReportUpdate", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", IncidentID)
        objCmd.Parameters.AddWithValue("@ReportUpdate", ReportUpdate)
        objCmd.Parameters.AddWithValue("@UserID", UserID)
        objCmd.Parameters.AddWithValue("@Date", Now)

        'Open the connection using the connection string.
        DBConStringHelper.PrepareConnection(objConn)

        'Execute the command to the DataReader.
        objCmd.ExecuteNonQuery()

        'Clean up our command objects and close the connection.
        objCmd.Dispose()
        objCmd = Nothing

        DBConStringHelper.FinalizeConnection(objConn)
    End Sub


    Public Sub UpdateIncidentLastUpdated(ByVal IncidentID As String, ByVal UserID As String)
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Enter the email and password to query/command object.
        objCmd = New SqlCommand("spUpdateIncidentLastUpdated", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", IncidentID)
        objCmd.Parameters.AddWithValue("@LastUpdated", Now)
        objCmd.Parameters.AddWithValue("@UserID", UserID)

        'Open the connection using the connection string.
        DBConStringHelper.PrepareConnection(objConn)

        'Execute the command to the DataReader.
        objCmd.ExecuteNonQuery()

        'Clean up our command objects and close the connection.
        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)
    End Sub
End Class