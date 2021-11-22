Imports Microsoft.VisualBasic
Imports System.Web.HttpContext
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
'Imports System.DirectoryServices

Public Class SecurityValidate
    Private _FullName As String
    Private _LastName As String
    Private _UserLevel As String
    Private _UserID As Integer
    Private _UserLevelID As Integer
    Private _Email As String
    Private _DateEULAAccepted As String
    Private _DatePasswordChanged As String
    Private _PhoneNumber As String
    Private _Agency As String


    Property FullName() As String
        Get
            Return _FullName
        End Get
        Set(ByVal Value As String)
            _FullName = Value
        End Set
    End Property

    Property LastName() As String
        Get
            Return _LastName
        End Get
        Set(ByVal Value As String)
            _LastName = Value
        End Set
    End Property

    Property Agency() As String
        Get
            Return _Agency
        End Get
        Set(ByVal Value As String)
            _Agency = Value
        End Set
    End Property

    Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal Value As Integer)
            _UserID = Value
        End Set
    End Property

    Property UserLevelID() As String
        Get
            Return _UserLevelID
        End Get
        Set(ByVal Value As String)
            _UserLevelID = Value
        End Set
    End Property
    Property UserLevel() As String
        Get
            Return _UserLevel
        End Get
        Set(ByVal Value As String)
            _UserLevel = Value
        End Set
    End Property

    Property Email() As String
        Get
            Return _Email
        End Get
        Set(ByVal Value As String)
            _Email = Value
        End Set
    End Property

    Property DateEULAAccepted() As String
        Get
            Return _DateEULAAccepted
        End Get
        Set(ByVal Value As String)
            _DateEULAAccepted = Value
        End Set
    End Property

    Public Sub SecurityValidate()

    End Sub


    Property DatePasswordChanged() As String
        Get
            Return _DatePasswordChanged
        End Get
        Set(ByVal Value As String)
            _DatePasswordChanged = Value
        End Set
    End Property

    Property PhoneNumber() As String
        Get
            Return _PhoneNumber
        End Get
        Set(ByVal Value As String)
            _PhoneNumber = Value
        End Set
    End Property

    Public Sub Logoff(ByRef SessionName As String)
        System.Web.HttpContext.Current.Session(SessionName) = Nothing
    End Sub


    Public Function CheckSecurity(ByVal objSecurity As SecurityValidate, ByVal UserLevel As String) As Boolean
        If (objSecurity.UserLevel <> UserLevel) Then
            Return False
        Else
            Return True
        End If

    End Function

    Public Function GetPageSecurity(ByVal Page As String) As List(Of String)
        Dim objConn As New System.Data.SqlClient.SqlConnection
        Dim objCmd As System.Data.SqlClient.SqlCommand
        Dim objDR As System.Data.SqlClient.SqlDataReader
        Dim objDS As New System.Data.DataSet
        Dim objDBConStringHelp As New DBConStringHelp
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectPageSecurityFilterPageName", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@ReferenceNumber", "Administration.aspx")
        objDBConStringHelp.PrepareConnection(objConn)
        objDR = objCmd.ExecuteReader()
        Dim Records As New List(Of String)
        If objDR.Read() Then
            Records.Add(objDR("UserLevel"))
        End If

        Return Records

    End Function

    Public Function ConvertStringsToStringList(items As String) As List(Of String)
        Dim list As New List(Of String)()
        Dim listItmes As String() = items.Split(",")
        For Each item As String In listItmes
            list.Add(item)
        Next
        Return list
    End Function

    'Public Sub ActiveDirectory()

    '    Dim userName As String = "TargetUserName"

    '    Using searcher As New DirectorySearcher("GC://Fleoc.com")
    '        searcher.Filter = String.Format("(&(objectClass=user)(sAMAccountName={0}))", userName)

    '        Using results As SearchResultCollection = searcher.FindAll()
    '            If results.Count > 0 Then
    '                Dim found As String = "Found User"

    '            End If
    '        End Using
    '    End Using
    'End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
