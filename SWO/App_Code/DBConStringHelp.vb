Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Public Class DBConStringHelp
    Public configAppSettings As System.Configuration.AppSettingsReader = New System.Configuration.AppSettingsReader
    Public objFunction As New HelpFunctions

    Dim objConn As New System.Data.SqlClient.SqlConnection
    Dim objCmd As System.Data.SqlClient.SqlCommand
    Dim objDR As System.Data.SqlClient.SqlDataReader
    Dim objDA As System.Data.SqlClient.SqlDataAdapter
    Dim ObjDS As New System.Data.DataSet
    Dim objCmdText As String
    Public Sub PrepareConnection(ByRef connection As SqlConnection)

        'if the provided connection is not open, we will open it
        If connection.State <> ConnectionState.Open Then
            connection.Open()
        End If

    End Sub

    Public Sub FinalizeConnection(ByRef connection As SqlConnection)

        'if the provided connection is open, we will close it
        If connection.State = ConnectionState.Open Then
            connection.Close()
            connection.Dispose()
        End If

    End Sub

    Public Shared ReadOnly Property ConnectionString() As String
        Get
            Return ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        End Get
    End Property

End Class

