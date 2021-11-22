Imports System.Data.SqlClient
Imports System.Data

Partial Class MyProfile
    Inherits System.Web.UI.Page

    'Help functions from our App_Code.
    Dim HelpFunction As New HelpFunctions
    Dim DBConStringHelper As New DBConStringHelp

    'For connecting to the database.
    Dim objConn As New System.Data.SqlClient.SqlConnection
    Dim objCmd As System.Data.SqlClient.SqlCommand
    Dim objDR As System.Data.SqlClient.SqlDataReader

    Dim MrDataGrabber As New DataGrabber
    Dim objDataGridFunctions As New DataGridFunctions

    Dim globalHasErrors As Boolean = False
    Dim globalAction As String
    Dim globalParameter As String
    Dim globalMessage As String
    Dim ns As New SecurityValidate

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ns = Session("Security_Tracker")

        If Page.IsPostBack = False Then
            PopulateDDLs()
            PopulatePage()
        Else
            If hidChangingPassword.Value = "true" Then
                divShowChangePassword.Attributes.Add("style", "display: none")
                divChangePassword.Attributes.Add("style", "display: inline")
                divNewPasswordRequirements.Attributes.Add("style", "color: Black")
                divNewPasswordRequirements.Attributes.Add("style", "background-color: #FFFFFF")
                divNewPasswordRequirements.Attributes.Add("style", "margin-left: 55%")
            End If
        End If
    End Sub

    Sub PopulatePage()
        Try
            Dim localFirstName As String = ""
            Dim localLastName As String = ""
            Dim localEmail As String = ""
            Dim localPassword As String = ""
            Dim localUserLevelID As Integer = 0
            Dim localIsActive As Boolean = False
            Dim localSecretQuestionID As Integer = 0
            Dim localSecretAnswer As String = ""
            Dim localAgencyID As Integer = 0

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectUserByUserID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@UserID", ns.UserID.ToString)
            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localUserLevelID = HelpFunction.ConvertdbnullsInt(objDR("UserLevelID"))
                localFirstName = HelpFunction.Convertdbnulls(objDR("FirstName"))
                localLastName = HelpFunction.Convertdbnulls(objDR("LastName"))
                'localPassword = HelpFunction.Convertdbnulls(objDR("Password"))
                localEmail = HelpFunction.Convertdbnulls(objDR("Email"))
                localIsActive = HelpFunction.ConvertdbnullsBool(objDR("IsActive"))
                localSecretQuestionID = HelpFunction.ConvertdbnullsInt(objDR("SecretQuestionID"))
                localSecretAnswer = HelpFunction.Convertdbnulls(objDR("SecretAnswer"))
                localAgencyID = HelpFunction.ConvertdbnullsInt(objDR("AgencyID"))
            End If

            objDR.Close()
            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

            'txtConfirmPassword.Text = localPassword
            'txtPassword.Attributes("value") = localPassword
            'txtConfirmPassword.Attributes("value") = localPassword
            txtEmail.Text = localEmail
            txtFirstName.Text = localFirstName
            txtLastName.Text = localLastName
            ddlSecretQuestion.SelectedValue = localSecretQuestionID
            txtSecretAnswer.Attributes("value") = localSecretAnswer
            ddlAgency.SelectedValue = localAgencyID
        Catch ex As Exception
            Response.Write(ex.ToString)
            Exit Sub
        End Try
    End Sub

    Sub PopulateDDLs()
        'Agency.
        '--------------------------------------------------------------------
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectAgency", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        'objCmd.Parameters.AddWithValue("@OrderBy", "") 'Optional parameter.

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        ddlAgency.DataSource = objCmd.ExecuteReader()
        ddlAgency.DataBind()

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        objCmd = Nothing

        'Add a "Select an Option" item to the list.
        ddlAgency.Items.Insert(0, New ListItem("Select an Agency", "0"))
        ddlAgency.Items(0).Selected = True
        '--------------------------------------------------------------------

        'Secret Questions.
        '--------------------------------------------------------------------
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectSecretQuestion", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        'objCmd.Parameters.AddWithValue("@OrderBy", "") 'Optional parameter.

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        ddlSecretQuestion.DataSource = objCmd.ExecuteReader()
        ddlSecretQuestion.DataBind()

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        objCmd = Nothing

        'Add a "Select an Option" item to the list.
        ddlSecretQuestion.Items.Insert(0, New ListItem("Select A Secret Question", "0"))
        ddlSecretQuestion.Items(0).Selected = True
        '--------------------------------------------------------------------
    End Sub

    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As EventArgs)
        btnSave.Disabled = False
        btnCancel.Disabled = False

        ErrorChecks()

        If globalHasErrors = False Then
            Dim _salt As String = String.Empty
            Dim ciphertext2 As String = String.Empty
            Dim strPasswordToSave As String = String.Empty

            If hidChangingPassword.Value = "true" Then
                strPasswordToSave = txtPassword.Text
            Else
                strPasswordToSave = txtCurrentPassword.Text
            End If

            ciphertext2 = UserValidation.EncryptPassword(strPasswordToSave, _salt)
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spUpdateUserProfile", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@UserID", ns.UserID.ToString)
            'objCmd.Parameters.AddWithValue("@Password", strPasswordToSave)
            objCmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text)
            objCmd.Parameters.AddWithValue("@LastName", txtLastName.Text)
            objCmd.Parameters.AddWithValue("@Email", txtEmail.Text)
            objCmd.Parameters.AddWithValue("@DatePasswordChanged", Now)
            objCmd.Parameters.AddWithValue("@SecretQuestionID", ddlSecretQuestion.SelectedValue)
            objCmd.Parameters.AddWithValue("@SecretAnswer", txtSecretAnswer.Text)
            objCmd.Parameters.AddWithValue("@AgencyID", ddlAgency.SelectedValue)
            objCmd.Parameters.AddWithValue("@SaltedPassword", ciphertext2)
            objCmd.Parameters.AddWithValue("@Salt", _salt)

            'Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            'Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()

            'Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            Response.Redirect("Home.aspx?message=ProfileUpdated")
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("Home.aspx")
    End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span style='color:#fe5105;'> ")

        'Adding the appropriate errors to the error string.
        If txtFirstName.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a First Name. <br />")
            globalHasErrors = True
        End If

        If txtLastName.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Last Name. <br />")
            globalHasErrors = True
        End If

        If txtEmail.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a valid UserName/Email. <br />")
            globalHasErrors = True
        End If

        'Check to see if the email is already in our database.
        Dim localEmail As String = ""
        Dim localEmail2 As String = ""
        Dim strSaltedPassword As String = ""
        Dim strSalt As String = ""

        Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectEmailByEmail", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@Email", txtEmail.Text.ToString)

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localEmail = HelpFunction.Convertdbnulls(objDR("Email"))
            End If

            objDR.Close()
            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()
        Catch ex As Exception
            Response.Write(ex.ToString)
            Exit Sub
        End Try

        Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectUserByUserID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@UserID", ns.UserID)
            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localEmail2 = HelpFunction.Convertdbnulls(objDR("Email"))
                strSaltedPassword = HelpFunction.Convertdbnulls(objDR("SaltedPassword"))
                strSalt = HelpFunction.Convertdbnulls(objDR("Salt"))
            End If

            objDR.Close()
            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()
        Catch ex As Exception
            Response.Write(ex.ToString)
            Exit Sub
        End Try

        If localEmail <> "" Then
            If localEmail2 <> localEmail Then
                'This checks to see if it is the persons own email.
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; This Email is already taken by another user. You must enter another Email. <br />")
                globalHasErrors = True
            End If
        End If

        If txtCurrentPassword.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide your current password. <br />")
            globalHasErrors = True
        Else
            If UserValidation.DecryptPassword(strSaltedPassword, strSalt) <> txtCurrentPassword.Text Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; Your current password is incorrect. <br />")
                globalHasErrors = True
            End If
        End If

        If ddlSecretQuestion.SelectedValue = "0" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a Secret Question. <br />")
            globalHasErrors = True
        End If

        If txtSecretAnswer.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Secret Answer. <br />")
            globalHasErrors = True
        End If

        If ddlAgency.SelectedValue = "0" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select an Agency. <br />")
            globalHasErrors = True
        End If

        If hidChangingPassword.Value = "true" Then
            If Not UserValidation.PasswordIsValid(txtPassword.Text, txtConfirmPassword.Text) Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a valid New Password and matching confirmation password. <br />")
                globalHasErrors = True
            End If
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label.
        lblMessage.Text = strError.ToString
    End Sub

End Class
