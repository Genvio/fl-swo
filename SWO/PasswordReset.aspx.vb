Imports System.Data
Imports System.Data.SqlClient

Partial Class PasswordReset
    Inherits System.Web.UI.Page

    Protected Sub PasswordReset_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim strProblemExplanation As String = "Your request is not valid (malformed request key)."

            If Not (ValidateQueryString() AndAlso ValidateResetRequest(strProblemExplanation)) Then
                pnlRequestExpired.Visible = True
                pnlResetPassword.Visible = False
                pnlResetSuccess.Visible = False
                lblProblemExplanation.Text = strProblemExplanation
            End If
        End If
    End Sub

    Private Function ValidateQueryString() As Boolean
        'Validate querystring: key (k) must be present, must be the only thing on the querystring, and must be a GUID
        If Request.QueryString.Count <> 1 Then Return False
        If Request.QueryString("k") = "" Then Return False
        If Not System.Text.RegularExpressions.Regex.IsMatch( _
            Request.QueryString("k"), _
            "^" & _
            "[A-Fa-f0-9]{8}-" & _
            "[A-Fa-f0-9]{4}-" & _
            "[A-Fa-f0-9]{4}-" & _
            "[A-Fa-f0-9]{4}-" & _
            "[A-Fa-f0-9]{12}" & _
            "$", _
            RegexOptions.IgnorePatternWhitespace) Then Return False
        Return True
    End Function

    Private Function ValidateResetRequest(ByRef inStrProblemExplanation As String) As Boolean
        'Call sp to check that request exists and has not expired. It returns a userID if all is well.
        Dim objConn As New SqlConnection
        Dim objCmd As SqlCommand
        Dim intReturnValue As Int32

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spValidatePasswordReset", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("PasswordResetKey", Request.QueryString("k"))
        objCmd.Parameters.Add("@return", SqlDbType.Int)
        objCmd.Parameters("@return").Direction = ParameterDirection.ReturnValue
        objConn.Open()
        objCmd.ExecuteNonQuery()
        intReturnValue = objCmd.Parameters("@return").Value
        objCmd = Nothing
        objConn.Close()
        objConn.Dispose()

        If intReturnValue > 0 Then
            inStrProblemExplanation = String.Empty
            Session("PWResetUserID") = intReturnValue
            Return True
        ElseIf intReturnValue = -1 Then
            inStrProblemExplanation = "Your password reset request was not found."
            Return False
        Else 'intReturnValue must be -2
            inStrProblemExplanation = "Your password reset request has expired."
            Return False
        End If
    End Function

    Protected Sub btnSubmit_Click(sender As Object, e As System.EventArgs) Handles btnSubmit.Click
        Dim objConn As New SqlConnection
        Dim objCmd As SqlCommand
        Dim intReturnValue As Int32

        If Not UserValidation.PasswordIsValid(txtPassword1.Text, txtPassword2.Text) Then
            lblNewPasswordError.Text = "The password is not valid. Please review password requirements."
            lblNewPasswordError.Visible = True
            pnlRequestExpired.Visible = False
            pnlResetPassword.Visible = True
            pnlResetSuccess.Visible = False
            Exit Sub
        End If

        Dim strSalt As String = String.Empty
        Dim strHashedPW As String
        strHashedPW = UserValidation.EncryptPassword(txtPassword1.Text, strSalt)

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spDoPasswordReset", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("UserID", Session("PWResetUserID"))
        'objCmd.Parameters.AddWithValue("Password", txtPassword1.Text)
        objCmd.Parameters.AddWithValue("SaltedPassword", strHashedPW)
        objCmd.Parameters.AddWithValue("Salt", strSalt)
        objCmd.Parameters.Add("@return", SqlDbType.Int)
        objCmd.Parameters("@return").Direction = ParameterDirection.ReturnValue
        objConn.Open()
        objCmd.ExecuteNonQuery()
        intReturnValue = objCmd.Parameters("@return").Value
        objCmd = Nothing
        objConn.Close()
        objConn.Dispose()

        If intReturnValue = 0 Then
            pnlRequestExpired.Visible = False
            pnlResetPassword.Visible = False
            pnlResetSuccess.Visible = True
        Else
            lblNewPasswordError.Text = "Error saving password. Please contact a <a href='mailto:" & ConfigurationManager.AppSettings("PasswordHelpToEmail") & "?subject=Incident%20Tracker%20Password%20Reset&body=Type%20your%20name%20and%20email%20address%20here.'>system administrator</a>."
            lblNewPasswordError.Visible = True
            pnlRequestExpired.Visible = False
            pnlResetPassword.Visible = True
            pnlResetSuccess.Visible = False
        End If
    End Sub

End Class
