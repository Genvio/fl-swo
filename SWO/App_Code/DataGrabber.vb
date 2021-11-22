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
Imports Microsoft.VisualBasic
Imports System.Web.HttpContext

Public Class DataGrabber

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


    Public Function GrabOneIntegerColumnByPrimaryKey(ByVal IntegerToGrab As String, ByVal Table As String, ByVal PK As String, ByVal PKValue As String) As String

        'This will grab One Integer Column from Any Table using the "D" Style

        Dim localQueryString As String = ""

        localQueryString = "SELECT * FROM " & Table & " WHERE " & PK & " = " & PKValue

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectColumnByPrimaryKey", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@QueryString ", localQueryString)

        objDR = objCmd.ExecuteReader

        Dim ReturnInteger As Integer

        If objDR.Read() Then

            ReturnInteger = HelpFunction.ConvertdbnullsInt(objDR(IntegerToGrab))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        Return ReturnInteger

    End Function

    Public Function GrabOneStringColumnByPrimaryKey(ByVal StringToGrab As String, ByVal Table As String, ByVal PK As String, ByVal PKValue As String) As String

        'This will grab One Integer Column from Any Table using the "D" Style

        Dim localQueryString As String = ""

        localQueryString = "SELECT * FROM " & Table & " WHERE " & PK & " = " & PKValue

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectIntegerColumnByPrimaryKey", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@QueryString ", localQueryString)

        'Current.Response.Write(localQueryString)
        'Current.Response.Write("<br>")
        'Current.Response.Write(StringToGrab)
        'Current.Response.End()

        objDR = objCmd.ExecuteReader

        Dim ReturnString As String = ""

        If objDR.Read() Then

            ReturnString = HelpFunction.Convertdbnulls(objDR(StringToGrab))

        End If



        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        'Current.Response.Write(ReturnString)
        'Current.Response.End()

        Return ReturnString

    End Function

    Public Function GrabBitColumnByKey(ByVal StringToGrab As String, ByVal Table As String, ByVal Key As String, ByVal KeyValue As String) As Boolean

        'This will grab One Integer Column from Any Table using the "D" Style

        Dim localQueryString As String = ""

        localQueryString = "SELECT * FROM " & Table & " WHERE " & Key & " = " & KeyValue

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectIntegerColumnByPrimaryKey", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@QueryString ", localQueryString)

        'Current.Response.Write(localQueryString)
        'Current.Response.Write("<br>")
        'Current.Response.Write(StringToGrab)
        'Current.Response.End()

        objDR = objCmd.ExecuteReader

        Dim ReturnBoolean As Boolean

        If objDR.Read() Then

            ReturnBoolean = HelpFunction.ConvertdbnullsBool(objDR(StringToGrab))

        End If



        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        'Current.Response.Write(ReturnString)
        'Current.Response.End()

        Return ReturnBoolean

    End Function

    Public Function GrabUserFullNameByUserID(ByVal UserID As String) As String


        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectUserFullNameFirstLastByUserID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@UserID ", UserID)

        'Current.Response.Write(localQueryString)
        'Current.Response.End()
        objDR = objCmd.ExecuteReader

        Dim ReturnString As String = ""

        If objDR.Read() Then

            ReturnString = HelpFunction.Convertdbnulls(objDR("FullNameFirstLast"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        Return ReturnString

    End Function

    Public Function GrabRecordCountByKey(ByVal Table As String, ByVal Key As String, ByVal KeyValue As String) As String

        'This will grab One Integer Column from Any Table using the "D" Style

        Dim localQueryString As String = ""

        localQueryString = "SELECT Count(*) As [Count] FROM [" & Table & "] WHERE " & Key & " = " & KeyValue

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectColumnByPrimaryKey", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@QueryString ", localQueryString)

        objDR = objCmd.ExecuteReader

        Dim ReturnInteger As Integer

        If objDR.Read() Then

            ReturnInteger = HelpFunction.ConvertdbnullsInt(objDR("Count"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        Return ReturnInteger

    End Function

    Public Function GrabStringByKey(ByVal Table As String, ByVal StringToGrab As String, ByVal Key As String, ByVal KeyValue As String) As String

        'This will grab One Integer Column from Any Table using the "D" Style

        Dim localQueryString As String = ""

        localQueryString = "SELECT * FROM [" & Table & "] WHERE " & Key & " = " & KeyValue

        'Current.Response.Write(localQueryString)
        'Current.Response.End()


        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectColumnByPrimaryKey", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@QueryString ", localQueryString)

        objDR = objCmd.ExecuteReader

        Dim ReturnString As String = ""

        If objDR.Read() Then

            ReturnString = HelpFunction.Convertdbnulls(objDR(StringToGrab))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        Return ReturnString

    End Function

    Public Function GrabIntegerByKey(ByVal Table As String, ByVal IntegerToGrab As String, ByVal Key As String, ByVal KeyValue As String) As String

        'This will grab One Integer Column from Any Table using the "D" Style

        Dim localQueryString As String = ""

        localQueryString = "SELECT * FROM [" & Table & "] WHERE " & Key & " = " & KeyValue

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectColumnByPrimaryKey", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@QueryString ", localQueryString)

        objDR = objCmd.ExecuteReader

        Dim ReturnString As Integer

        If objDR.Read() Then

            ReturnString = HelpFunction.Convertdbnulls(objDR(IntegerToGrab))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        Return ReturnString

    End Function

    Public Function GrabRecordCountBy2Keys(ByVal Table As String, ByVal Key As String, ByVal KeyValue As String, ByVal Key2 As String, ByVal KeyValue2 As String) As String

        'This will grab One Integer Column from Any Table using the "D" Style

        Dim localQueryString As String = ""

        localQueryString = "SELECT Count(*) As [Count] FROM [" & Table & "] WHERE " & Key & " = " & KeyValue & " AND " & Key2 & " = " & KeyValue2

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectColumnByPrimaryKey", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@QueryString ", localQueryString)

        objDR = objCmd.ExecuteReader

        Dim ReturnInteger As Integer

        If objDR.Read() Then

            ReturnInteger = HelpFunction.ConvertdbnullsInt(objDR("Count"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        Return ReturnInteger

    End Function

    Public Function GrabIntegerRecordBy2Keys(ByVal Table As String, ByVal IntegerRecord As String, ByVal Key As String, ByVal KeyValue As String, ByVal Key2 As String, ByVal KeyValue2 As String) As String

        'This will grab One Integer Column from Any Table using the "D" Style

        Dim localQueryString As String = ""

        localQueryString = "SELECT * FROM [" & Table & "] WHERE " & Key & " = " & KeyValue & " AND " & Key2 & " = " & KeyValue2

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectColumnByPrimaryKey", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@QueryString ", localQueryString)

        objDR = objCmd.ExecuteReader

        Dim ReturnInteger As Integer

        If objDR.Read() Then

            ReturnInteger = HelpFunction.ConvertdbnullsInt(objDR(IntegerRecord))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        Return ReturnInteger

    End Function

    Public Function GrabCountyCounty(ByVal Counties As String) As String

        Dim localCountyCount As Integer = 0

        If Counties <> "" Then
            Dim localCounties As String = Counties

            Dim T As Integer = 0

            For X = 1 To Len(localCounties)

                Dim localTest As String = ""
                localTest = localCounties.Substring(T, X)

                If localTest.Length > 3 Then
                    localTest = Right(localTest, 1)

                    If localTest = "," Then
                        localCountyCount = localCountyCount + 1
                    End If


                End If

                If localCounties.Substring(T, X) = "," Then T = T + 1

            Next X

        End If

        Dim ReturnInteger As Integer = localCountyCount

        Return ReturnInteger

    End Function

    Public Function GrabIncidentTypeUserByUserID(ByVal UserID As String) As String

        Dim strIncidentTypeID As String = ""

        Dim localRecordCountForArray As Integer = 0

        localRecordCountForArray = GrabRecordCountByKey("IncidentTypeUser", "UserID", UserID)

        Dim localIncidentTypeUserLoopCount As Integer = 0

        If localRecordCountForArray <> 0 Then

            'Must minus 1 to account for the Array Declaration
            Dim arrIncidentType(localRecordCountForArray - 1) As Integer
            'Store each IncidentTypeID in Array
            'Checking to see if there are any worksheets 
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectIncidentTypeUserByUserID]", objConn)
            objCmd.Parameters.AddWithValue("@UserID", UserID)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then
                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    arrIncidentType(localIncidentTypeUserLoopCount) = objDR.Item("IncidentTypeID")

                    strIncidentTypeID = strIncidentTypeID & objDR.Item("IncidentTypeID") & ","

                    localIncidentTypeUserLoopCount = localIncidentTypeUserLoopCount + 1

                End While

                localIncidentTypeUserLoopCount = 0

            Else

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

            'End the Loop Count

        End If

        If strIncidentTypeID <> "" Then
            strIncidentTypeID = Mid(strIncidentTypeID, 1, Len(strIncidentTypeID) - 1)
        End If

        Return strIncidentTypeID

    End Function

    Public Function strSelectInitialReportByIncidentID(ByVal strIncidentID As String) As String

        'Current.Response.Write(localQueryString)
        'Current.Response.End()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectInitialReportByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID ", strIncidentID)

        objDR = objCmd.ExecuteReader

        Dim ReturnString As String = ""

        If objDR.Read() Then

            ReturnString = HelpFunction.Convertdbnulls(objDR("InitialReport"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        Return ReturnString

    End Function

    Public Function strSelectUpdateReportByIncidentID(ByVal strIncidentID As String) As String

        'Current.Response.Write(localQueryString)
        'Current.Response.End()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectUpdateReportByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID ", strIncidentID)

        objDR = objCmd.ExecuteReader

        Dim ReturnString As String = ""

        If objDR.Read() Then

            ReturnString = HelpFunction.Convertdbnulls(objDR("UpdateReport"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        Return ReturnString

    End Function

    Public Function GrabOneDateStringColumnAsMilitaryTimeByPrimaryKey(ByVal StringToGrab As String, ByVal Table As String, ByVal PK As String, ByVal PKValue As String) As String

        'This will grab One Integer Column from Any Table using the "D" Style

        Dim localQueryString As String = ""

        localQueryString = "SELECT " & "convert(varchar, " & StringToGrab & ", 101) + '-' + " & " LEFT(convert(varchar, " & StringToGrab & " , 114),5) As " & StringToGrab & " FROM " & Table & " WHERE " & PK & " = " & PKValue

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectIntegerColumnByPrimaryKey", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@QueryString ", localQueryString)

        'Current.Response.Write(localQueryString)
        'Current.Response.Write("<br>")
        'Current.Response.Write(StringToGrab)
        'Current.Response.End()

        objDR = objCmd.ExecuteReader

        Dim ReturnString As String = ""

        If objDR.Read() Then

            ReturnString = HelpFunction.Convertdbnulls(objDR(StringToGrab))

        End If



        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        'Current.Response.Write(ReturnString)
        'Current.Response.End()

        Return ReturnString

    End Function
    ''' <summary>
    ''' DO NOT USE--this function is not fully implemented!
    ''' </summary>
    ''' <param name="UserID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GrabMostRecentUpdateReportByUserID(ByVal UserID As String) As Integer

        'This will grab One Integer Column from Any Table using the "D" Style

        Dim localUserID As Integer = 0

        'localQueryString = "SELECT * FROM [" & Table & "] WHERE " & Key & " = " & KeyValue

        'objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        'objConn.Open()
        'objCmd = New SqlCommand("spSelectColumnByPrimaryKey", objConn)
        'objCmd.CommandType = CommandType.StoredProcedure
        'objCmd.Parameters.AddWithValue("@QueryString ", localQueryString)

        'objDR = objCmd.ExecuteReader

        'Dim ReturnString As Integer

        'If objDR.Read() Then

        '    ReturnString = HelpFunction.Convertdbnulls(objDR(IntegerToGrab))

        'End If

        'objDR.Close()

        'objCmd.Dispose()
        'objCmd = Nothing

        'objConn.Close()

        Return localUserID

    End Function


End Class
