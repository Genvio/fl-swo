Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Net.Mail

Public Class LatLongConversion


    Function DecimalDegreesToDegreesMinutesSeconds(ByVal DecimalDegrees As Decimal) As String

        Dim DecDegAbs As Decimal = Math.Abs(DecimalDegrees)
        Dim ReturnValue As String = "'"
        Dim DegreeSymbol As String = "°"
        Dim MinutesSymbol As String = "’"
        Dim SecondsSymbol As String = """"
        Dim Degrees As String = Math.Truncate(DecDegAbs) & DegreeSymbol
        Dim MinutesDecimal As Decimal = (DecDegAbs - Math.Truncate(DecDegAbs)) * 60
        Dim SecondsDecimal As Decimal = (MinutesDecimal - Math.Truncate(MinutesDecimal))
        Dim Minutes As String = Math.Truncate(MinutesDecimal) & MinutesSymbol
        Dim Seconds As String = String.Format("{0:##.0000}", (SecondsDecimal * 60)) & SecondsSymbol
        ReturnValue = Degrees & " " & Minutes & " " & Seconds
        Return ReturnValue

    End Function

    'Public Function GetTimeoutTime() As Integer
    '    'change the timeout time here globally in minutes
    '    Return ConfigurationManager.AppSettings("TimeOutTime").ToString
    'End Function

    'Public Function Convertdbnulls(ByVal x)
    '    'returns a string value for a passed in database field
    '    If x Is System.DBNull.Value Then
    '        x = ""
    '    End If
    '    Return CStr(LTrim(RTrim(x)))
    'End Function

    'Public Function ConvertdbnullsInt(ByVal x)
    '    'returns a integer value for a passed in database field

    '    If x Is System.DBNull.Value Then
    '        x = 0
    '    End If

    '    Return CInt(x)
    'End Function

    'Public Function ConvertdbnullsDbl(ByVal x)
    '    'returns a double value for a passed in database field

    '    If x Is System.DBNull.Value Then
    '        x = 0
    '    End If

    '    Return CDbl(x)
    'End Function

    'Public Function ConvertdbnullsBool(ByVal x)
    '    'returns a boolean value for a passed in database field 

    '    If x Is System.DBNull.Value Then
    '        x = False
    '    End If

    '    Return CBool(x)
    'End Function

    'Public Function ConvertdbnullsDate(ByVal x)
    '    'returns a formated date
    '    If x Is System.DBNull.Value Then
    '        x = ""
    '    Else
    '        x = FormatDateTime(x, DateFormat.ShortDate)
    '    End If
    '    Return x
    'End Function

    'Public Function FormatPhoneNumber(ByVal x As String)
    '    'formats a 10 digit phone number from the format 1111111111 to (111) 111-1111
    '    If Trim(x) <> "" Then
    '        x = "(" & Mid(x, 1, 3) & ") " & Mid(x, 4, 3) & "-" & Mid(x, 7, 4)
    '    End If
    '    Return x
    'End Function

    'Public Function SetDistinctReportName(ByRef strReportName As String)
    '    'this function returns a report name passed in with a current "timestamp" to distinctly name it
    '    'it parses out any special characters and spaces.
    '    '---------------------------------------------------------------------------------
    '    strReportName = strReportName & Replace(Replace(Replace(Now(), "/", ""), ":", ""), " ", "")
    '    Return strReportName
    '    '---------------------------------------------------------------------------------
    'End Function

    'Public Function ReplaceSQLInjectionCharacters(ByVal strStringToReplace) As String
    '    'Filter out character like single quote, double quote, slash, back slash, semi colon, extended character like NULL, carry return, new line, etc, in all strings
    '    Dim strTempString As String

    '    strTempString = Microsoft.VisualBasic.Trim(Replace(strStringToReplace, "'", "''"))
    '    strTempString = Microsoft.VisualBasic.Trim(Replace(strStringToReplace, "/", ""))
    '    strTempString = Microsoft.VisualBasic.Trim(Replace(strStringToReplace, ";", ""))
    '    strTempString = Microsoft.VisualBasic.Trim(Replace(strStringToReplace, "NULL", ""))
    '    strTempString = Microsoft.VisualBasic.Trim(Replace(strStringToReplace, "Char(13)", ""))
    '    strTempString = Microsoft.VisualBasic.Trim(Replace(strStringToReplace, "Char(10)", ""))


    '    Return strTempString
    'End Function

    'Public Function SendEmail(ByVal strToAddress As String, ByVal strFromAddress As String, ByVal strSubject As String, ByVal strEmailBody As String, ByVal boolShowCloseButton As Boolean) As String
    '    'this function sends an email based on the parameters passed in
    '    '----------------------------------------------------------------
    '    Dim strCloseButton As String = ""
    '    If boolShowCloseButton = True Then
    '        strCloseButton = "<br><br><center><input type='button' id='btnClose' value='Close' onclick='window.close();' class='button' /></center>"
    '    End If
    '    If strFromAddress <> "" Then
    '        Try
    '            Dim mm As New MailMessage(strFromAddress, strToAddress)
    '            mm.Subject = "USF CMS Email :: " & strSubject
    '            mm.Body = strEmailBody
    '            mm.IsBodyHtml = True
    '            Dim smtp As New SmtpClient
    '            smtp.Send(mm)
    '        Catch ex As Exception
    '            Return "<br><b>Error: We are currently having trouble processing your E-mail. Please check that your email address is correct. If problem persists, please try again at a later time.</b></center><br><center>The Error is : " & ex.Message & strCloseButton
    '        End Try
    '    Else
    '        Return "<br>Error: You can not leave the 'To Address' blank. Please fill out and try again." & strCloseButton
    '    End If
    '    Return "<br>The email's were successfully sent." & strCloseButton

    'End Function

    'Public Function CheckAlphaNumeric(ByVal strInputText As String) As Boolean
    '    'checks the string for alphanumeric characters and returns true or false 
    '    Dim intCounter As Integer
    '    Dim strCompare As String
    '    Dim strInput As String
    '    CheckAlphaNumeric = False

    '    For intCounter = 1 To Len(strInputText)
    '        strCompare = Mid$(strInputText, intCounter, 1)
    '        strInput = Mid$(strInputText, intCounter + 1, Len(strInputText))
    '        If strCompare Like ("[A-Z]") Or strCompare Like ("[a-z]") Or strCompare Like ("#") Then
    '            CheckAlphaNumeric = True
    '        Else
    '            CheckAlphaNumeric = False
    '            Exit Function
    '        End If
    '    Next intCounter

    '    'make sure it contains at least one number in the password
    '    For intCounter = 1 To Len(strInputText)
    '        strCompare = Mid$(strInputText, intCounter, 1)
    '        strInput = Mid$(strInputText, intCounter + 1, Len(strInputText))
    '        If strCompare Like ("#") Then
    '            CheckAlphaNumeric = True
    '            Exit Function
    '        Else
    '            CheckAlphaNumeric = False
    '        End If
    '    Next intCounter

    'End Function

    'Public Function CheckUserPasswordHistory(ByVal strUserID As String, ByVal strNewPassword As String) As Boolean
    '    'checks the password history of the user and determines if this password is one they have used before
    '    CheckUserPasswordHistory = False
    '    'database connection and calling objects
    '    Dim DAL As New DBConStringHelp
    '    Dim objConn As New SqlConnection
    '    Dim objCmd As SqlCommand
    '    Dim ObjDR As SqlDataReader
    '    Dim ObjDS As New System.Data.DataSet
    '    '-----------------------------------------
    '    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
    '    objCmd = New SqlCommand("[spSelectUser]", objConn)
    '    objCmd.CommandType = System.Data.CommandType.StoredProcedure
    '    objCmd.Parameters.AddWithValue("@UserID", strUserID)

    '    DAL.PrepareConnection(objConn) 'open the connection

    '    ObjDR = objCmd.ExecuteReader()

    '    While ObjDR.Read
    '        'see if the current or any of the used passwords matches the password they are trying to use
    '        If strNewPassword = Convertdbnulls(ObjDR("UsedPassword1")) Or strNewPassword = Convertdbnulls(ObjDR("UsedPassword2")) _
    '            Or strNewPassword = Convertdbnulls(ObjDR("UsedPassword3")) Or strNewPassword = Convertdbnulls(ObjDR("UsedPassword4")) Then
    '            CheckUserPasswordHistory = True
    '        End If
    '    End While

    '    DAL.FinalizeConnection(objConn) 'open the connection

    '    objCmd = Nothing

    '    Return CheckUserPasswordHistory
    'End Function

    'Public Sub CleanupReportDirectory()
    '    'this subrouting is called on the reports page and cleans up any files that are older than 30 minutes
    '    '----------------------------------------------------------------------------------------------------
    '    Dim configAppSettings As System.Configuration.AppSettingsReader = New System.Configuration.AppSettingsReader
    '    ' Create a reference to the current directory.'FilePath & "\Reports\ReportOutputFiles\"

    '    Dim di As New System.IO.DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FilePath").ToString) & "\Reports\ReportOutputFiles\")
    '    ' Create an array representing the files in the current directory.
    '    Dim fi As System.IO.FileInfo() = di.GetFiles()
    '    ' Print out the names of the files in the current directory.
    '    Dim fiTemp As System.IO.FileInfo
    '    For Each fiTemp In fi
    '        If DateDiff(DateInterval.Minute, fiTemp.CreationTime, Now()) > 30 Then
    '            'file has been around for 30 minutes or more so delete it
    '            Try
    '                fiTemp.Delete()
    '            Catch ex As Exception
    '            End Try

    '        End If
    '    Next fiTemp
    'End Sub

    'Public Sub CleanupPowerPointDirectory()
    '    'this subrouting is called on the reports page and cleans up any files that are older than 30 minutes
    '    '----------------------------------------------------------------------------------------------------
    '    Dim configAppSettings As System.Configuration.AppSettingsReader = New System.Configuration.AppSettingsReader
    '    ' Create a reference to the current directory.'FilePath & "\PowerPoint\PowerPointOutputFiles\"

    '    Dim di As New System.IO.DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FilePath").ToString) & "\PowerPoint\PowerPointOutputFiles\")
    '    ' Create an array representing the files in the current directory.
    '    Dim fi As System.IO.FileInfo() = di.GetFiles()
    '    ' Print out the names of the files in the current directory.
    '    Dim fiTemp As System.IO.FileInfo
    '    For Each fiTemp In fi
    '        If DateDiff(DateInterval.Minute, fiTemp.CreationTime, Now()) > 30 Then
    '            'file has been around for 30 minutes or more so delete it
    '            Try
    '                fiTemp.Delete()
    '            Catch ex As Exception
    '            End Try

    '        End If
    '    Next fiTemp
    'End Sub

    'Public Sub CleanupGoogleImages()
    '    'this subrouting is called on the reports page and cleans up any files that are older than 30 minutes
    '    '----------------------------------------------------------------------------------------------------
    '    Dim configAppSettings As System.Configuration.AppSettingsReader = New System.Configuration.AppSettingsReader
    '    ' Create a reference to the current directory.'FilePath & "\Images\GoogleImages\"

    '    Dim di As New System.IO.DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FilePath").ToString) & "\Images\GoogleImages\")
    '    ' Create an array representing the files in the current directory.
    '    Dim fi As System.IO.FileInfo() = di.GetFiles()
    '    ' Print out the names of the files in the current directory.
    '    Dim fiTemp As System.IO.FileInfo

    '    For Each fiTemp In fi

    '        Try
    '            fiTemp.Delete()
    '        Catch ex As Exception
    '        End Try


    '    Next fiTemp

    'End Sub


    'Public Shared Function StripHTML(ByVal htmlString As String) As String


    '    'This pattern Matches everything found inside html tags;

    '    '(.|\n) - > Look for any character or a new line

    '    ' *? -> 0 or more occurences, and make a non-greedy search meaning

    '    'That the match will stop at the first available '>' it sees, and not at the last one

    '    '(if it stopped at the last one we could have overlooked

    '    'nested HTML tags inside a bigger HTML tag..)

    '    ' Thanks to Oisin and Hugh Brown for helping on this one...

    '    Dim pattern As String = "<(.|\n)*?>"




    '    Return (Regex.Replace(htmlString, pattern, String.Empty))
    'End Function


    'Public Shared Function GetSqlParameters(ByVal parms As System.Data.SqlClient.SqlParameterCollection, ByVal spName As String)
    '    Dim sqlString As String = "DECLARE" & " " & "@return_value int "
    '    sqlString &= "EXEC @return_value = " & spName & " "
    '    For Each sqlParm As SqlParameter In parms
    '        sqlString &= (sqlParm.ParameterName & " = N'" & sqlParm.Value & "' ")
    '    Next

    '    Return sqlString
    'End Function

    'Public Shared Function FirstDayOfFiscalYear(ByVal FiscalYear As Int32) As String
    '    Return "10/1/" & (FiscalYear - 1)
    'End Function

    'Public Shared Function LastDayOfFiscalYear(ByVal FiscalYear As Int32) As String
    '    Return "12/31/" & FiscalYear
    'End Function

    'Public Shared Function FirstDayOfQuarter(ByVal qt As Int32, ByVal FiscalYear As Int32)
    '    Select Case qt
    '        Case 1
    '            Return "10/1/" & (FiscalYear - 1)
    '        Case 2
    '            Return "1/1/" & FiscalYear
    '        Case 3
    '            Return "4/1/" & FiscalYear
    '        Case 4
    '            Return "7/1/" & FiscalYear

    '    End Select
    '    Return ""
    'End Function

    'Public Shared Function LastDayOfQuarter(ByVal qt As Int32, ByVal FiscalYear As Int32)
    '    Select Case qt
    '        Case 1
    '            Return "12/31/" & (FiscalYear - 1)
    '        Case 2
    '            Return "3/31/" & FiscalYear
    '        Case 3
    '            Return "6/30/" & FiscalYear
    '        Case 4
    '            Return "9/30/" & FiscalYear

    '    End Select
    '    Return ""

    'End Function

    'Public Function GetJavaScriptForNegative()
    '    Dim strReturn As String = ""
    '    strReturn = strReturn & "<style type=""text/css"">"
    '    strReturn = strReturn & "td.negative { color : red; }"
    '    strReturn = strReturn & "a.negative { color : red; }"
    '    strReturn = strReturn & "b.negative { color : red; }"
    '    strReturn = strReturn & "</style>"
    '    strReturn = strReturn & "<script language=""JavaScript"" type=""text/javascript"">"
    '    strReturn = strReturn & "<!--" & ControlChars.NewLine
    '    strReturn = strReturn & "function MakeNegative() { "
    '    strReturn = strReturn & "anchors = document.getElementsByTagName(""a"");"
    '    strReturn = strReturn & "for (var i=0; i<anchors.length; i++) {"
    '    strReturn = strReturn & "var temp = anchors[i];"
    '    strReturn = strReturn & "if (temp.innerHTML != null){ "
    '    strReturn = strReturn & "if (temp.innerHTML.indexOf('(') == 0) temp.className = ""negative"";"
    '    'temp.className = ""negative"";"
    '    'strReturn = strReturn & "if (temp.firstChild.nodeValue != null && temp.firstChild.nodeValue.indexOf('$') == 0 && temp.firstChild.nodeValue.indexOf('-') == 1) temp.className = ""negative"";"
    '    'strReturn = strReturn & "if (temp.firstChild.nodeValue.indexOf('(') == 0) temp.className = ""negative"";"
    '    strReturn = strReturn & "   }" 'end if
    '    strReturn = strReturn & "   }" 'end for

    '    strReturn = strReturn & "TDs = document.getElementsByTagName(""TD"");"
    '    strReturn = strReturn & "for (var i=0; i<TDs.length; i++) {"
    '    strReturn = strReturn & "var temp = TDs[i];"
    '    strReturn = strReturn & "if (temp.firstChild.nodeValue != null){ "
    '    strReturn = strReturn & "if (temp.firstChild.nodeValue.indexOf('(') == 0) temp.className = ""negative"";"
    '    strReturn = strReturn & "   }" 'end if
    '    strReturn = strReturn & "   }" 'end for
    '    strReturn = strReturn & "Bs = document.getElementsByTagName(""b"");"
    '    strReturn = strReturn & "for (var i=0; i<Bs.length; i++) {"
    '    strReturn = strReturn & "var temp = Bs[i];"
    '    strReturn = strReturn & "if (temp.innerHTML != null){ "
    '    strReturn = strReturn & "if (temp.innerHTML.indexOf('(') == 0) temp.className = ""negative"";"
    '    strReturn = strReturn & "   }" 'end if
    '    strReturn = strReturn & "   }" 'end for
    '    strReturn = strReturn & "}" ' end method
    '    strReturn = strReturn & "//-->"
    '    strReturn = strReturn & "</script>"

    '    Return strReturn
    'End Function

    'Public Function IsEmail(ByVal email As String)
    '    Dim regex As New Regex("^.+@[^\.].*\.[A-Za-z]{2,}$")
    '    If regex.IsMatch(email) Then
    '        Return True
    '    End If
    '    Return False

    'End Function

    'Public Function AddNBusinessDays(ByVal startDate As DateTime, ByVal numDays As Integer) As DateTime

    '    If numDays = 0 Then Return New DateTime(startDate.Ticks)

    '    If numDays < 0 Then Throw New ArgumentException()

    '    Dim i As Integer
    '    Dim totalDays As Integer
    '    Dim businessDays As Integer

    '    totalDays = 0
    '    businessDays = 0

    '    Dim currDate As DateTime
    '    While businessDays < numDays
    '        totalDays += 1

    '        currDate = startDate.AddDays(totalDays)

    '        If Not (currDate.DayOfWeek = DayOfWeek.Saturday Or currDate.DayOfWeek = DayOfWeek.Sunday) Then
    '            businessDays += 1
    '        End If

    '    End While

    '    Return currDate

    'End Function

    'Function CleanupForXML(ByVal strData As String)
    '    'cleans up a string of data passed in removing any xml characters that would interfere with writing the string to xml
    '    Dim strTempString As String = ""

    '    If strData <> "" Then

    '        strTempString = Replace(strData, "&", " ")
    '        strTempString = Replace(strTempString, "/", " ")
    '        strTempString = Replace(strTempString, "\", " ")
    '        strTempString = Replace(strTempString, ",", " ")
    '        strTempString = Replace(strTempString, "-", " ")
    '        strTempString = Replace(strTempString, ".", " ")
    '        strTempString = Replace(strTempString, "'", " ")
    '        strTempString = Replace(strTempString, """", " ")

    '    End If

    '    Return strTempString.Trim

    'End Function


    'Public Function ReplaceSingleQuotesForInsert(ByVal strStringToReplace As String) As String
    '    'Filter out character like single quote, double quote, slash, back slash, semi colon, extended character like NULL, carry return, new line, etc, in all strings
    '    Dim strTempString As String
    '    strTempString = Replace(strStringToReplace, "'", "''")

    '    'This should get rid of all "&" and "\" for GATOR 
    '    strTempString = Replace(strTempString, "&", "AND")
    '    strTempString = Replace(strTempString, "\", "-")
    '    strTempString = Replace(strTempString, ">", " ")
    '    strTempString = Replace(strTempString, "<", " ")
    '    strTempString = Replace(strTempString, "+", " ")
    '    strTempString = Replace(strTempString, "/", " ")
    '    strTempString = Replace(strTempString, "#", " ")

    '    If strStringToReplace = "" Then
    '        strTempString = ""
    '    End If

    '    Return strTempString

    'End Function



    'Public Function ReplaceSingleQuotesForPopulation(ByVal strStringToReplace As String) As String
    '    'Filter out character like single quote, double quote, slash, back slash, semi colon, extended character like NULL, carry return, new line, etc, in all strings
    '    Dim strTempString As String

    '    strTempString = Replace(strStringToReplace, "''", "'")

    '    If strStringToReplace = "" Then
    '        strTempString = ""
    '    End If

    '    Return strTempString

    'End Function

    'Public Function RandomStringGenerator(ByVal intLen As Integer) As String

    '    Dim r As New Random()

    '    Dim i As Integer

    '    Dim strTemp As String = ""

    '    For i = 0 To intLen

    '        strTemp = strTemp & Chr(Int((26 * r.NextDouble()) + 65))

    '    Next

    '    Return strTemp

    'End Function




End Class


