Imports System.Security.Cryptography
Imports Microsoft.VisualBasic
Imports System.Web.HttpContext

Public Class UserValidation
    'local variables
    Private dLastRequest As DateTime
    Private TempUserID As String
    Private TempFullName As String
    Private TempUserLevel As String
    Private TempUserLevelID As String
    Private TempEmail As String
    'EULA
    Private TempDateEULAAccepted As String
    Private TempDatePasswordChanged As String

    'Public ObjCookie As System.Web.HttpCookie
    Dim ns As New SecurityValidate

    Public HelpFunctions As New HelpFunctions

    Public Sub Logout()
        'logs the user out of the system
        'set the timeout of the requested cookies to now
        'Dim i As Integer
        'Dim aCookie As System.Web.HttpCookie
        If Not HttpContext.Current.Session("Security_Tracker") Is Nothing Then HttpContext.Current.Session("Security_Tracker") = Nothing

        'The following code is killing ALL cookies from the domain--we only want to kill those specific to this application
        'Dim limit As Integer = Current.Request.Cookies.Count - 1
        'For i = 0 To limit
        '    aCookie = Current.Request.Cookies(i)
        '    aCookie.Expires = DateTime.Now.AddDays(-1)
        '    Current.Response.Cookies.Add(aCookie)
        'Next

        'aCookie = Current.Request.Cookies(HttpContext.Current.Application("ApplicationEnvironment").ToString)
        'aCookie.Expires = DateTime.Now.AddDays(-1)
        'Current.Response.Cookies.Add(aCookie)


        'we have terminated the users cookies for this session so redirect
        SessionExpired() '<-- Session didn't expire?  Just redirect to login to skip the expiration text.
    End Sub

    Public Sub New() 'ByVal SystemID As String)

    End Sub

    Public Sub CheckSecurity()

        'Check to see if the cookies exsist first...
        Try
            'set the properties for reference
            'ObjCookie = Current.Request.Cookies(HttpContext.Current.Application("ApplicationEnvironment").ToString)
            ns = HttpContext.Current.Session("Security_Tracker")
            '---------------------------------------------------------------------------------------------------------------------
            '-  Extract UserID
            '---------------------------------------------------------------------------------------------------------------------
            TempUserID = ns.UserID.ToString()
            '---------------------------------------------------------------------------------------------------------------------
            '-  Extract FullName
            '---------------------------------------------------------------------------------------------------------------------
            TempFullName = ns.FullName
            '---------------------------------------------------------------------------------------------------------------------
            '-  Extract UserLevel
            '---------------------------------------------------------------------------------------------------------------------
            TempUserLevel = ns.UserLevel
            '---------------------------------------------------------------------------------------------------------------------
            '-  Extract UserLevelID
            '---------------------------------------------------------------------------------------------------------------------
            TempUserLevelID = ns.UserLevelID
            '---------------------------------------------------------------------------------------------------------------------
            '-  Extract Email
            '---------------------------------------------------------------------------------------------------------------------
            TempEmail = ns.Email
            ''---------------------------------------------------------------------------------------------------------------------
            ''-  Extract TempDateEULAAccepted
            ''---------------------------------------------------------------------------------------------------------------------
            TempDateEULAAccepted = ns.DateEULAAccepted
            ''---------------------------------------------------------------------------------------------------------------------
            ''-  Extract TempDatePasswordChanged
            ''---------------------------------------------------------------------------------------------------------------------
            TempDatePasswordChanged = ns.DatePasswordChanged
       
            '---------------------------------------------------------------------------------------------------------------------
            'Session now handles timeout
            'dLastRequest = ReplaceCookieCharacters(ObjCookie.Item("Expires"))
            'If DateDiff(DateInterval.Minute, Now(), dLastRequest) <= 0 Then
            '    'it has been over the 20 (or whatever set) minutes since last request, therefore time them out
            '    Logout()
            'Else
            '    Dim configAppSettings As System.Configuration.AppSettingsReader = New System.Configuration.AppSettingsReader
            '    Dim Timestring As String = HelpFunctions.GetTimeoutTime()

            '    'set the time of the current response to the new timeout...20 + 1
            '    ObjCookie.Item("Expires") = DateAdd(DateInterval.Minute, Timestring + 1, Now())
            '    Current.Response.Cookies.Set(ObjCookie)
            '    Current.Session.Timeout = Timestring
            'End If

            'password expired
            '------------------------------------------------------------------------
            If TempDatePasswordChanged = "" Then
                'they must change their password
                Current.Response.Redirect("ChangePassword.aspx")
            Else
                If IsDate(TempDatePasswordChanged) = False Then
                    'they must change their password
                    Current.Response.Redirect("ChangePassword.aspx")
                Else
                    If DateDiff(DateInterval.Day, CDate(TempDatePasswordChanged), Now()) >= 90 Then
                        'its been 90 days, they must change their password
                        Current.Response.Redirect("ChangePassword.aspx")
                    End If
                End If
            End If
            'EULA
            '------------------------------------------------------------------------
            If TempDateEULAAccepted = "" Then
                'redirect to the EULA
                Current.Response.Redirect("EULA.aspx")
            End If

        Catch e As System.NullReferenceException
            'the cookies don't exsist
            'the session has expired
            Logout()
        End Try

    End Sub

    Public ReadOnly Property SessionUserID() As String
        Get
            Return TempUserID
        End Get
    End Property

    Public ReadOnly Property SessionFullName() As String
        Get
            Return TempFullName
        End Get
    End Property

    Public ReadOnly Property SessionUserLevel() As String
        Get
            Return TempUserLevel
        End Get
    End Property

    Public ReadOnly Property SessionUserLevelID() As String
        Get
            Return TempUserLevelID
        End Get
    End Property

    Public ReadOnly Property SessionEmail() As String
        Get
            Return TempEmail
        End Get
    End Property

    Public Function ReplaceCookieCharacters(ByVal strTime As String) As String

        Dim intCounter As Integer

        'replaces /
        intCounter = Microsoft.VisualBasic.InStr(strTime, "%2F")
        If intCounter > 0 Then
            strTime = Microsoft.VisualBasic.Replace(strTime, "%2F", "/")
        End If
        'replaces space
        intCounter = Microsoft.VisualBasic.InStr(strTime, "+")
        If intCounter > 0 Then
            strTime = Microsoft.VisualBasic.Replace(strTime, "+", " ")
        End If
        'replaces space
        intCounter = Microsoft.VisualBasic.InStr(strTime, "%3A")
        If intCounter > 0 Then
            strTime = Microsoft.VisualBasic.Replace(strTime, "%3A", ":")
        End If

        Return strTime

    End Function

    Private Sub SessionExpired()
        '-----------------------------------------------------------------------
        '-  SESSION HAS EXPIRED - redirect to Logon page
        '-----------------------------------------------------------------------
        Current.Response.Redirect("~/default.aspx?message=2&action=login")
    End Sub

    ''' <summary>
    ''' Validates plain-text passwords.
    ''' Must have at least three of: uppercase letter,  lowercase letter, number,  non-alpha numeric character.
    ''' Must be at least 8 characters in length, and passwords must match.
    ''' </summary>
    ''' <param name="strPW1">Password</param>
    ''' <param name="strPW2">Confirmation password</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function PasswordIsValid(strPW1 As String, strPW2 As String) As Boolean
        Dim intCriteriaPassed As Int16 = 0
        If strPW1.IndexOfAny("abcdefghijklmnopqrstuvwxyz".ToCharArray()) > -1 Then intCriteriaPassed += 1
        If strPW1.IndexOfAny("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()) > -1 Then intCriteriaPassed += 1
        If strPW1.IndexOfAny("1234567890".ToCharArray()) > -1 Then intCriteriaPassed += 1
        If strPW1.IndexOfAny("`~1234567890_-+=[]{}\|;:'"",.<>/?".ToCharArray()) > -1 Then intCriteriaPassed += 1
        If intCriteriaPassed < 3 Then Return False
        If strPW1.Length < 8 Then Return False
        If Not strPW1.Equals(strPW2) Then Return False
        Return True
    End Function

    ''' <summary>
    ''' Encrypts a plaintext password and returns the encrypted value, plus salt string by reference.
    ''' </summary>
    ''' <param name="strPlaintextPW"></param>
    ''' <param name="strSalt">Empty string expected. If not empty, value will be ignored and overwritten. A 16-character string will be assigned.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function EncryptPassword(strPlaintextPW As String, Optional ByRef strSalt As String = "") As String
        Dim keySize As Integer = 256
        Dim iterations As Integer = 1000
        strSalt = GetPasswordSalt()
        Dim saltBytes As Byte() = Encoding.ASCII.GetBytes(strSalt)
        Dim passwordBytes As New Rfc2898DeriveBytes(strSalt, saltBytes, iterations)
        Dim keyBytes As Byte() = passwordBytes.GetBytes(keySize / 8)
        Dim aes As New AES(keyBytes)
        Return aes.Encrypt(strPlaintextPW)
    End Function

    Private Shared Function GetPasswordSalt() As String
        Dim rand As New Random()
        Dim allowableChars() As Char = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789".ToCharArray()
        Dim strSalt As String = String.Empty

        For i As Integer = 0 To 16 - 1
            strSalt += allowableChars(rand.Next(allowableChars.Length - 1))
        Next

        Return strSalt
    End Function

    Public Shared Function DecryptPassword(strHashedPW As String, strSalt As String) As String
        Dim keySize As Integer = 256
        Dim iterations As Integer = 1000
        Dim saltBytes As Byte() = Encoding.ASCII.GetBytes(strSalt)
        Dim passwordBytes As New Rfc2898DeriveBytes(strSalt, saltBytes, iterations)
        Dim keyBytes As Byte() = passwordBytes.GetBytes(keySize / 8)
        Dim aes As New AES(keyBytes)
        Return aes.Decrypt(strHashedPW)
    End Function

    Public Shared Function SendUserPasswordResetLink(strToEmail As String, strKeyGUID As String) As Exception
        Try
            'E-mails the user a link to reset their password
            Dim mailSubject As String
            Dim mailBody As String
            Dim objException As New Exception

            mailSubject = "Incident Tracker Password Reset Request"
            Dim strApplicationPath As String
            strApplicationPath = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) & HttpContext.Current.Request.ApplicationPath
            Dim strResetLink As String
            strResetLink = strApplicationPath & "/PasswordReset.aspx?k=" & strKeyGUID

            'Create the body message
            mailBody = "<table style='font-family: Verdana;font-size:12px;'><tr><td colspan='2'><h4>Incident Tracker Password Reset Request:</h4></td></tr>"
            mailBody = mailBody & "<tr><td><b>Navigate to the following page to reset your password. This page is only available for 15 minutes from the time you requested help with your password.</b></td></tr>"
            mailBody = mailBody & "<tr><td><b><a href=""" & strResetLink & """>" & strResetLink & "</a></b></td></tr>"
            mailBody = mailBody & "<tr><td>&nbsp;</td></tr><tr><td>If you cannot click on the link to open the web page, please copy it and paste it into your web browser.</td></tr>"
            mailBody = mailBody & "<tr><td>&nbsp;</td></tr><tr><td>This is an auto generated email, please do not respond to this email address.</td></tr>"
            mailBody = mailBody & "<tr><td>&nbsp;</td></tr><tr><td>Thank you for using Incident Tracker</td></tr>"
            mailBody = mailBody & "</table>"
            Email.SendEmail(mailSubject, mailBody, strToEmail, "SWP@em.myflorida.com", False, objException)
            If Not objException.Source Is Nothing Then Throw objException
        Catch ex As Exception
            Return ex
        End Try

        Return Nothing
    End Function

End Class