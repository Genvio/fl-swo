Imports System.Data
Imports System.Data.SqlClient
Imports System.Security.Cryptography

Partial Class _Default
    Inherits System.Web.UI.Page

    '// Connection String and Numerous Function Help 
    Private objDBConStringHelp As New DBConStringHelp
    Private HelpFunction As New HelpFunctions


    '// SQL Objects
    Private objConn As New SqlConnection
    Private objConn2 As New SqlConnection
    Private strSQL As String
    Private objCmd As SqlCommand
    Private objCmd2 As SqlCommand
    Private objDR As SqlDataReader
    Private objDR2 As SqlDataReader
    Private objDS As New System.Data.DataSet
    Private objDA As System.Data.SqlClient.SqlDataAdapter
    Private objCB As SqlCommandBuilder
    Public DBConStringHelper As New DBConStringHelp
    'password reset variables
    Dim da As New SqlDataAdapter
    Dim MyCB As SqlCommandBuilder
    Dim ds As New Data.DataSet

    '// Cookie Information
    Private strUserID As String
    Private strFullName As String
    Private strLastName As String
    Private strUserLevel As String
    Private strUserLevelID As String
    Private strAgency As String
    Private strEmail As String
    Private strPhoneNumber As String

    Private strDateEULAAccepted As String
    Private strDatePasswordChanged As String

    '//Remote Login Info
    Private strUserName As String
    Private strPassword As String

    '// Misc Vars
    Private strMessage As String
    Private strGuidResult As String
    Private strNewPassword As String

    '// Global Functions and Cookie Security Validation
    Private objPubFunctions As New HelpFunctions
    Private objSecurity As UserValidation

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        txtEmailAddress.Focus()

        If Page.IsPostBack = False Then

            'If Not Request.IsSecureConnection Then

            '    ' Redirect to https
            '    Response.Redirect("https://apps.floridadisaster.org/SWO/")

            'End If

            btnLogin.Attributes.Add("onclick", "javascript:" & btnLogin.ClientID + ".disabled=true;" & lnkForgotPassword.ClientID + ".disabled=true;")
            txtEmailAddress.Attributes.Add("onkeydown", "try{if (event.which || event.keyCode){if ((event.which==13) || (event.keyCode==13)){document.getElementById('" & btnLogin.UniqueID & "').click();return false;}}else{return true}}catch(ex){};")
            txtPassword.Attributes.Add("onkeydown", "try{if (event.which || event.keyCode){if ((event.which==13) || (event.keyCode==13)){document.getElementById('" & btnLogin.UniqueID & "').click();return false;}}else{return true}}catch(ex){};")
            '// Set message
            strMessage = Request("Message")
            strUserName = Request("Username")
            strPassword = Request("Password")

            '// Are they already logged in?
            IsLoggedIn()

            Select Case strMessage
                Case "2"
                    lblMessage.Text = "Your session has expired.  Please login below."
                    lblMessage.Visible = True
                Case "3"
                    lblMessage.Text = ""
                    ValidateLogin()
                Case "4"
                    lblMessage.Text = "You must first login to view SWO GATOR"
                    lblMessage.Visible = True
                Case Else
                    lblMessage.Text = ""
                    lblMessage.Visible = False
            End Select
        End If

    End Sub

    Private Sub IsLoggedIn()

        '// Check to see if they are logged in if they are then redirect.
        'Dim ObjCookie As System.Web.HttpCookie
        Dim ns As New SecurityValidate
        Dim strFullName As String = ""
        Dim strUserLevel As String = ""

        '// Set the properties for reference.
        'ObjCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        ns = Session("Security_Tracker")
        '---------------------------------------------------------------------------------------------------------------------
        Try
            '// Extract FullName
            strFullName = ns.FullName 'ObjCookie.Item("FullName")
            '// Extract UserLevel
            strUserLevelID = ns.UserLevelID 'ObjCookie.Item("UserLevelID")
            'email
            strEmail = ns.Email 'ObjCookie.Item("Email")
            'clientid
            '// Redirect based on user level
            Response.Redirect("Home.aspx")

        Catch ex As Exception

            '// They are not logged in. (Throws an error.)
        End Try
    End Sub

    Private Sub ValidateLogin()

        '// Checks the username and password, and if the account is active and logs them in.
        Dim emailAddressValue As String
        Dim passwordValue As String

        '// Get email and password from controls.
        'sql injection checking
        If strUserName <> "" Then
            emailAddressValue = HelpFunction.ReplaceSQLInjectionCharacters(strUserName)
            passwordValue = strPassword
            passwordValue = HelpFunction.ReplaceSQLInjectionCharacters(passwordValue)
        Else
            emailAddressValue = HelpFunction.ReplaceSQLInjectionCharacters(txtEmailAddress.Text)
            passwordValue = txtPassword.Text.Trim
            passwordValue = HelpFunction.ReplaceSQLInjectionCharacters(passwordValue)
        End If

        '// The user didn't enter an email.
        If emailAddressValue = "" Then
            lblMessage.Visible = True
            lblMessage.Text = "Invalid Email Address. Try Again"
            txtEmailAddress.Focus()
            Exit Sub
        End If

        '// The user didn't enter a password.
        If passwordValue = "" Then
            lblMessage.Visible = True
            lblMessage.Text = "Invalid Password. Try Again"
            txtPassword.Focus()
            Exit Sub
        End If

        Try
            'We will delete any Records that have no Relevance to the database
            Dim oDataDeleter As New DataDeleter()
            oDataDeleter.DeleteOldNonSavedReports()
        Catch ex As Exception

        End Try

        '// Enter the connection string for our database.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        '// Enter the email and password to query/command object.
        objCmd = New SqlCommand("spSelectUserFilterEmail", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@Email", emailAddressValue)
        '// Open the connection using the connection string.
        objDBConStringHelp.PrepareConnection(objConn)
        '// Execute the command to the DataReader.
        objDR = objCmd.ExecuteReader()

        '// Read the data from the database for matches.
        If objDR.Read() Then
            Dim plaintext As String = UserValidation.DecryptPassword(objDR("SaltedPassword"), objDR("Salt"))

            If passwordValue = plaintext Then
                '// User entered correct information, so get the account details.
                strUserID = objDR("UserID")
                strFullName = objDR("FullName")
                strLastName = objDR("LastName")
                strUserLevel = objDR("UserLevel")
                strUserLevelID = objDR("UserLevelID")
                strAgency = objDR("Abbreviation")
                strEmail = objDR("Email")
                '-----EULA Accepted
                strDateEULAAccepted = objPubFunctions.Convertdbnulls(objDR("DateEULAAccepted"))
                'last date the password was changed
                strDatePasswordChanged = objPubFunctions.Convertdbnulls(objDR("DatePasswordChanged"))

                ''log successful connection
                AccessLog(strUserID, True)
            Else
                '// User has an incorrect password.
                lblMessage.Visible = True
                lblMessage.Text = "Invalid Username or Password. Try Again."
                objDR.Close()
                objCmd.Dispose()
                objCmd = Nothing
                objDBConStringHelp.FinalizeConnection(objConn)
                ''log failed attempt
                AccessLogFailed(txtEmailAddress.Text, False) ''see if user is using an existing user name, if so log and update bruteforce
                Exit Sub
            End If

        Else
            '// User has an incorrect username.
            lblMessage.Visible = True
            lblMessage.Text = "Invalid Username or Password. Try Again."
            objDR.Close()
            objCmd.Dispose()
            objCmd = Nothing
            objDBConStringHelp.FinalizeConnection(objConn)
            ''log failed attempt
            AccessLogFailed(txtEmailAddress.Text, False) ''see if user is using an existing user name, if so log and update bruteforce
            Exit Sub
        End If

        '// Clean up our command objects and close the connection.
        objDR.Close()
        objCmd.Dispose()
        objCmd = Nothing
        objDBConStringHelp.FinalizeConnection(objConn)

        '// Set session variables and redirect to Home welcome screen
        Dim currentMonth As Integer
        Dim currentYear As Integer
        'Dim oCookie As New System.Web.HttpCookie(Application("ApplicationEnvironment").ToString)

        currentMonth = Now.Month.ToString
        currentYear = Now.Year.ToString

        Dim s As New SecurityValidate
        s.UserID = strUserID
        s.FullName = strFullName
        s.UserLevel = strUserLevel
        s.Email = strEmail
        s.DateEULAAccepted = strDateEULAAccepted
        s.DatePasswordChanged = strDatePasswordChanged
        s.PhoneNumber = strPhoneNumber
        s.LastName = strLastName
        s.Agency = strAgency
        s.UserLevelID = strUserLevelID

        Session("Security_Tracker") = s
        'TODO: Set session timeout if default is not wanted!

        '// Add user information into the cookie.
        'oCookie.Item("UserID") = strUserID
        'oCookie.Item("FullName") = strFullName
        'oCookie.Item("LastName") = strLastName
        'oCookie.Item("UserLevel") = strUserLevel
        'oCookie.Item("UserLevelID") = strUserLevelID
        'oCookie.Item("Agency") = strAgency
        'oCookie.Item("Email") = strEmail
        ''-----EULA Accepted
        'oCookie.Item("DateEULAAccepted") = strDateEULAAccepted
        'oCookie.Item("DatePasswordChanged") = strDatePasswordChanged
        'oCookie.Item("PhoneNumber") = strPhoneNumber

        '// Calculate 30 mins from now for the cookie to expire.
        Dim dt As DateTime = DateTime.Now()
        Dim ts As New TimeSpan(0, objPubFunctions.GetTimeoutTime(), 0)
        'oCookie.Item("Expires") = dt.Add(ts)

        '// Add cookie (yum).
        'Response.Cookies.Add(oCookie)

        'update the last login field
        '// Enter the connection string for our database.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        '// Enter the email and password to query/command object.
        objCmd = New SqlCommand("spUpdateUserLastLogin", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@UserID", strUserID)

        '// Open the connection using the connection string.
        objDBConStringHelp.PrepareConnection(objConn)

        '// Execute the command to the DataReader.
        objCmd.ExecuteNonQuery()
        '// Clean up our command objects and close the connection.
        objCmd.Dispose()
        objCmd = Nothing
        objDBConStringHelp.FinalizeConnection(objConn)

        'redirect
        IsLoggedIn()

    End Sub

    Protected Sub btnLogin_Command(ByVal sender As Object, ByVal e As System.EventArgs)
        lblMessage.Text = ""
        lblMessage.ForeColor = Drawing.Color.Red
        ValidateLogin()
        btnLogin.Disabled = False
        lnkForgotPassword.Disabled = False
    End Sub

    Protected Sub AccessLog(ByVal strUserID As String, ByVal boolResult As Boolean)
        'log the success or failure
        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        objCmd2 = New SqlCommand("spLogAccess", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@UserID", strUserID)
        objCmd2.Parameters.AddWithValue("@DateAttempt", DateTime.Now)
        objCmd2.Parameters.AddWithValue("@AttemptSuccessful", boolResult)
        objCmd2.Parameters.AddWithValue("@IPAddress", HttpContext.Current.Request.UserHostAddress)
        objDBConStringHelp.PrepareConnection(objConn2) 'open the connection
        objCmd2.ExecuteNonQuery()
        objCmd2.Dispose()
        objDBConStringHelp.FinalizeConnection(objConn2)
    End Sub

    Protected Sub AccessLogFailed(ByVal strEmailAddress As String, ByVal boolResult As Boolean)
        '' a failed log on attempt,  see if they are using a real username, if so update the brute force
        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        objCmd2 = New SqlCommand("spLogFailedAttempt", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@EmailAddress", strEmailAddress)
        objCmd2.Parameters.AddWithValue("@DateAttempt", DateTime.Now)
        objCmd2.Parameters.AddWithValue("@AttemptSuccessful", boolResult)
        objCmd2.Parameters.AddWithValue("@IPAddress", HttpContext.Current.Request.UserHostAddress)
        objDBConStringHelp.PrepareConnection(objConn2) 'open the connection
        objDR2 = objCmd2.ExecuteReader()
        If objDR2.Read() Then
            Dim BruteForce As Integer = HelpFunction.ConvertdbnullsInt(objDR2("Bruteforce"))
            If BruteForce >= 6 Then
                pnlForbidEntry.Visible = True
            End If
        End If
        objCmd2.Dispose()
        objDBConStringHelp.FinalizeConnection(objConn2)
    End Sub

    Protected Sub lnkForgotPassword_Command(ByVal sender As Object, ByVal e As EventArgs)
        'they forgot their password
        If txtEmailAddress.Text = "" Then
            lblMessage.Visible = True
            lblMessage.Text = "Please enter a valid email address to retreive a password."
            txtEmailAddress.Focus()
            Exit Sub
        End If

        lblMessage.Visible = False
        litPasswordHelpLink.Text = "Need help? <a href='mailto:" & ConfigurationManager.AppSettings("PasswordHelpToEmail") & "?subject=Incident%20Tracker%20Password%20Reset&body=Type%20your%20name%20and%20email%20address%20here.'>Click here</a> to request assistance."
        litPasswordHelpLink.Visible = True

        'email address
        '------------------------------
        Dim EmailHold As String
        EmailHold = HelpFunction.ReplaceSQLInjectionCharacters(txtEmailAddress.Text)
        '------------------------------

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectSecretQuestionByEmailAddress", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@EmailAddress", EmailHold)

        objDBConStringHelp.PrepareConnection(objConn) 'open the connection
        objDR = objCmd.ExecuteReader()

        If objDR.Read() Then
            lbl1.Visible = True
            lbl2.Visible = True
            btnSubmit.Visible = True
            lnkForgotPassword.Visible = False
            btnLogin.Visible = False
            txtSecretAnswer.Visible = True
            lblSecretQuestion.Visible = True
            txtPassword.Visible = False
            lblPassword.Visible = False
            lblSecretQuestion.Text = HelpFunction.Convertdbnulls(objDR.Item("SecretQuestion"))
        End If

        objCmd.Dispose()
        objCmd = Nothing
        objDBConStringHelp.FinalizeConnection(objConn) 'close the connection
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        'send this user a new login
        '------------------------------
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'email address
        '------------------------------
        Dim EmailHold As String
        EmailHold = Microsoft.VisualBasic.Trim(Replace(txtEmailAddress.Text, "'", "''"))
        '------------------------------

        'da = New SqlDataAdapter("Select SecretQuestion, SecretAnswer From [User],SecretQuestion Where SecretQuestion.SecretQuestionID = [User].SecretQuestionID And Email = '" & EmailHold & "' And SecretAnswer = '" & txtSecretAnswer.Text & "'", objConn)
        Dim cmd As SqlCommand = New SqlCommand("Select SecretQuestion, SecretAnswer From [User],SecretQuestion Where SecretQuestion.SecretQuestionID = [User].SecretQuestionID And Email = @Email And SecretAnswer = @Answer", objConn)
        cmd.Parameters.AddWithValue("@Email", EmailHold)
        cmd.Parameters.AddWithValue("@Answer", txtSecretAnswer.Text)
        da = New SqlDataAdapter()
        da.SelectCommand = cmd
        MyCB = New SqlCommandBuilder(da)
        ds = New Data.DataSet
        objDBConStringHelp.PrepareConnection(objConn)  'if the provided connection is not open, we will open it

        da.Fill(ds, "a")

        If ds.Tables(0).Rows.Count > 0 Then
            btnLoginReturn.Visible = True 'show the login link

            objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objDBConStringHelp.PrepareConnection(objConn2)  'if the provided connection is not open, we will open it
            Dim objCommand = New SqlCommand("spRequestPasswordReset", objConn2)
            objCommand.CommandType = CommandType.StoredProcedure
            objCommand.Parameters.AddWithValue("@Email", EmailHold)
            Dim objDataReader As SqlDataReader
            objDataReader = objCommand.ExecuteReader()

            If objDataReader.Read() Then
                strGuidResult = HelpFunction.Convertdbnulls(objDataReader("PasswordResetKey"))
            End If

            objDataReader.Close()
            objCommand.Dispose()
            objCommand = Nothing
            objDBConStringHelp.FinalizeConnection(objConn2)  'close the connection
            objConn2 = Nothing

            lblMessage.Text = "Password reset instructions have been emailed to you.<br><br>If you do not receive the email, please check your ""spam"" or ""junk"" email folder.<br><br>  In order to receive Incident Tracker emails, please allow emails from ""SWP@em.myflorida.com""."
            lblMessage.Visible = True
            lblMessage.ForeColor = Drawing.Color.Green

            Dim ex As Exception = UserValidation.SendUserPasswordResetLink(EmailHold, strGuidResult)

            If Not ex Is Nothing Then
                lblMessage.Text = "Please contact customer support.  The error number is : <br /> 850-413-9900<br />" & ex.Message
                lblMessage.ForeColor = Drawing.Color.Red
                lblMessage.Visible = True
            End If
        Else
            lblMessage.Text = "That is not the correct answer. Please try again."
            lblMessage.Visible = True
        End If

        MyCB = Nothing
        ds = Nothing
        da = Nothing
        objDBConStringHelp.FinalizeConnection(objConn)  'close the connection
        objConn = Nothing
    End Sub

    Protected Sub btnLoginReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoginReturn.Click
        Response.Redirect("default.aspx")
    End Sub
End Class

