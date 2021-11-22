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
Imports System.Text
Imports System.Security.Cryptography

Partial Class EditUser
    Inherits System.Web.UI.Page

    'Help functions from our App_Code.
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    'For connecting to the database.
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet

    Public MrDataGrabber As New DataGrabber
    Public objDataGridFunctions As New DataGridFunctions

    Dim globalHasErrors As Boolean = False
    Dim globalAction As String
    Dim globalParameter As String
    Dim globalMessage As String

    'Public ObjCookie As System.Web.HttpCookie
    'Dim oCookie As System.Web.HttpCookie
    Dim ns As New SecurityValidate

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        If ns.UserLevelID <> "1" Then 'oCookie.Item("UserLevelID").ToString.Trim <> "1" Then
            Response.Redirect("Home.aspx")
        End If

        If Page.IsPostBack = False Then
            PopulateDDLs()

            'Set message.
            globalMessage = Request("Message")
            globalAction = Request("Action")
            globalParameter = Request("Parameter")

            PopulateDDLs()

            Select Case globalAction
                Case "Delete"
                    If ns.UserLevelID = "1" Then
                        Select Case globalParameter
                            Case "IncidentType"
                                DeleteIncidentType()
                            Case Else

                        End Select
                    End If
                Case "2"

                Case "3"

                Case Else

            End Select

            globalParameter = Request("Parameter")

            Select Case globalParameter
                Case "WorkSheet"
                    'txtLinkName.Focus()
                Case Else

            End Select

            Dim localUserID As String = Request("UserID")

            If localUserID = 0 Then
                lblAddEdit.Text = "Add "
                btnSave.Value = "Add"
                tblPassword.Visible = True
                tblPasswordConfirm.Visible = True
                tblPasswordReset.Visible = False
                divUserForm.Attributes.Add("style", "width: 55%; float:left")
                divPasswordValidation.Attributes.Add("style", "margin-left: 55%")
            Else
                Dim localUserLevelID As Integer = MrDataGrabber.GrabIntegerByKey("User", "UserLevelID", "UserID", localUserID)

                If localUserLevelID <> 1 Then
                    getIncidentTypeUser()
                    pnlShowIncidentTypes.Visible = True
                End If

                lblAddEdit.Text = "Edit "
                btnSave.Value = "Save"
                tblPassword.Visible = False
                tblPasswordConfirm.Visible = False
                tblPasswordReset.Visible = True
                divUserForm.Attributes.Add("style", "float:none")
                divPasswordValidation.Attributes.Add("style", "display:none")
                PopulatePage()
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
            objCmd.Parameters.AddWithValue("@UserID", Request("UserID"))

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

            ddlUserLevel.SelectedValue = localUserLevelID
            txtConfirmPassword.Text = localPassword

            txtPassword.Attributes("value") = localPassword
            txtConfirmPassword.Attributes("value") = localPassword
            txtEmail.Text = localEmail
            txtFirstName.Text = localFirstName
            txtLastName.Text = localLastName
            chkIsActive.Checked = localIsActive
            ddlSecretQuestion.SelectedValue = localSecretQuestionID
            'txtSecretAnswer.Text = localSecretAnswer
            txtSecretAnswer.Attributes("value") = localSecretAnswer
            ddlAgency.SelectedValue = localAgencyID
        Catch ex As Exception
            Response.Write(ex.ToString)

            Exit Sub
        End Try
    End Sub

    Sub PopulateDDLs()
        'User level.
        '--------------------------------------------------------------------
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectUserLevel", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        'objCmd.Parameters.AddWithValue("@OrderBy", "") 'Optional parameter.

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        ddlUserLevel.DataSource = objCmd.ExecuteReader()
        ddlUserLevel.DataBind()

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        objCmd = Nothing

        'Add a "Select an Option" item to the list.
        ddlUserLevel.Items.Insert(0, New ListItem("Select a User Level", "0"))
        ddlUserLevel.Items(0).Selected = True
        '--------------------------------------------------------------------

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

        ddlIncidentType.Items.Clear()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectIncidentTypeOrderByIncidentType", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        'objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        'objCmd.Parameters.AddWithValue("@OrderBy", "") 'Optional parameter.

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        ddlIncidentType.DataSource = objCmd.ExecuteReader()
        ddlIncidentType.DataBind()

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        objCmd = Nothing

        'Add a "Select an Option" item to the list.
        ddlIncidentType.Items.Insert(0, New ListItem("Select An Incident Worksheet", "0"))
        'ddlIncidentType.Items.Insert(100, New ListItem("", "0"))
        ddlIncidentType.Items(0).Selected = True
    End Sub

    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As EventArgs)
        'Response.Write(ddlUserLevel.SelectedValue)
        'Response.End()

        btnSave.Disabled = False
        btnCancel.Disabled = False

        ErrorChecks()

        If globalHasErrors = False Then
            If lblAddEdit.Text.Trim() = "Add" Then
                Dim _salt As String = String.Empty
                Dim ciphertext2 As String = UserValidation.EncryptPassword(txtPassword.Text, _salt)

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionUser", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@UserID", 0)
                'objCmd.Parameters.AddWithValue("@Password", txtPassword.Text)
                objCmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text)
                objCmd.Parameters.AddWithValue("@LastName", txtLastName.Text)
                objCmd.Parameters.AddWithValue("@Email", txtEmail.Text)
                objCmd.Parameters.AddWithValue("@DatePasswordChanged", Now)
                objCmd.Parameters.AddWithValue("@UserLevelID", ddlUserLevel.SelectedValue)
                objCmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked)
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

                Response.Redirect("User.aspx?message=1")
            Else
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionUser", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@UserID", Request("UserID"))
                'objCmd.Parameters.AddWithValue("@Password", txtPassword.Text)
                objCmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text)
                objCmd.Parameters.AddWithValue("@LastName", txtLastName.Text)
                objCmd.Parameters.AddWithValue("@Email", txtEmail.Text)
                objCmd.Parameters.AddWithValue("@DatePasswordChanged", Now)
                objCmd.Parameters.AddWithValue("@UserLevelID", ddlUserLevel.SelectedValue)
                objCmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked)
                objCmd.Parameters.AddWithValue("@SecretQuestionID", ddlSecretQuestion.SelectedValue)
                objCmd.Parameters.AddWithValue("@SecretAnswer", txtSecretAnswer.Text)
                objCmd.Parameters.AddWithValue("@AgencyID", ddlAgency.SelectedValue)
                ' Per RB, user can no longer change password from EditUser.aspx

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                Response.Redirect("User.aspx?message=3")
            End If
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("User.aspx")
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

        If ddlUserLevel.SelectedValue = "0" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a User Level. <br />")
            globalHasErrors = True
        End If

        If Request("UserID") = "0" Then
            If Not UserValidation.PasswordIsValid(txtPassword.Text, txtConfirmPassword.Text) Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a valid Password and matching confirmation password. <br />")
                globalHasErrors = True
            End If

            If txtConfirmPassword.Text = "" Then
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must confirm your Password. <br />")
                globalHasErrors = True
            End If

            'Check to see if the email is already in our database.
            Dim localEmail As String = ""
            Dim localEmail2 As String = ""

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
                objCmd.Parameters.AddWithValue("@UserID", Request("UserID"))

                objDR = objCmd.ExecuteReader

                If objDR.Read() Then
                    localEmail2 = HelpFunction.Convertdbnulls(objDR("Email"))
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
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label.
        lblMessage.Text = strError.ToString
    End Sub

    Protected Sub ErrorChecksIncidentType()
        Dim strError As New System.Text.StringBuilder

        'Start The Error String
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        'Adding the appropriate errors to the error string
        If ddlIncidentType.SelectedValue = "0" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select an Incident Type. <br />")
            globalHasErrors = True
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label(s).
        lblMessage.Text = strError.ToString
        'lblMessage2.Text = strError.ToString
    End Sub

    Protected Sub btnAddIncidentType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddIncidentType.Click
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        ErrorChecksIncidentType()

        If globalHasErrors = True Then
            'If we have errors, show message then Exit Sub. No insert of record.
            pnlMessage.Visible = True
            'pnlMessage2.Visible = True

            globalHasErrors = False

            Exit Sub
        Else
            'Response.Write(ddlIncidentType.SelectedItem.ToString)
            'Response.End()

            If ddlIncidentType.SelectedItem.ToString = "All Worksheets" Then
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spInsertIncidentTypeUserAllWorksheets", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@UserID", Request("UserID"))
                objCmd.Parameters.AddWithValue("@IncidentTypeID", ddlIncidentType.SelectedValue)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)
            Else
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spInsertIncidentTypeUser", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@UserID", Request("UserID"))
                objCmd.Parameters.AddWithValue("@IncidentTypeID", ddlIncidentType.SelectedValue)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)
            End If

            'Now we can add to the AI table the new IncidentID so it all ties together.
            '--------------------------------------------------------------------
            'Now we must add a row to the Incident Update.

            'globalAuditAction = "Added Worksheet: " & ddlIncidentType.SelectedItem.ToString & "  "

            'AuditHelper.InsertReportUpdate(Request("IncidentID"), globalAuditAction, oCookie.Item("UserID"))

            'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID") & "&PagePopulation=IncidentType")

            'PopulateDDLs()
            getIncidentTypeUser()

            pnlShowIncidentTypeGrid.Visible = True
        End If
    End Sub

    Protected Sub getIncidentTypeUser()
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        'Response.Write(sSortStr.ToString)
        'Response.Write("<br>")
        'Response.Write("SearchBy: " & sSearchBy.ToString)
        'Response.Write("<br>")
        'Response.Write("Searchtext: " & sSearchText.ToString)
        'Response.Write("<br>")
        'Response.End()

        objCmd = New SqlCommand("spSelectIncidentTypeUserByUserID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@UserID", Request("UserID"))

        objDS.Tables.Clear()

        'Bind our data.
        objDA = New SqlDataAdapter(objCmd)
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        'Call the calculate grid counts to show the number of records, the page you are on, etc.
        objDataGridFunctions.CalcDataGridCounts(objDS, IncidentTypeUserDataGrid, "")

        'Associate the data grid with the data.
        IncidentTypeUserDataGrid.DataSource = objDS.Tables(0).DefaultView
        IncidentTypeUserDataGrid.DataBind()

        objDataGridFunctions.Highlightrows(IncidentTypeUserDataGrid, "", "", "")

        'Checking to see if we have any contact methods.
        'Response.Write(objDS.Tables(0).Rows.Count)
        'Response.Write(CInt(objDS.Tables(0).Rows.Count))

        If CInt(objDS.Tables(0).Rows.Count) <> 0 Then
            'We have records so show the grid.
            pnlShowIncidentTypes.Visible = True
            pnlShowIncidentTypeGrid.Visible = True
        Else
            'Hide the grid.
            'pnlShowIncidentTypes.Visible = False
        End If
    End Sub

    Private Sub DeleteIncidentType()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Enter the email and password to query/command object.
        objCmd = New SqlCommand("spDeleteIncidentTypeUserByIncidentTypeUserID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentTypeUserID", Request("IncidentTypeUserID"))

        'Open the connection using the connection string.
        DBConStringHelper.PrepareConnection(objConn)

        'Execute the command to the DataReader.
        objCmd.ExecuteNonQuery()

        'Clean up our command objects and close the connection.
        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)

        Response.Redirect("EditUser.aspx?UserID=" & Request("UserID") & "&Message=2")
    End Sub

    Protected Sub btnResetPassword_Click(sender As Object, e As System.EventArgs) Handles btnResetPassword.Click
        'Send an email to the user being edited
        If txtEmail.Text = "" Then
            pnlMessage.Visible = True
            lblMessage.Text = "Please enter a valid email address."
            txtEmail.Focus()
            Exit Sub
        End If

        Dim strEmailAddress As String = HelpFunction.ReplaceSQLInjectionCharacters(txtEmail.Text)
        Dim objDBConStringHelp As New DBConStringHelp
        Dim strGuidResult As String = ""
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objDBConStringHelp.PrepareConnection(objConn)  'if the provided connection is not open, we will open it
        Dim objCommand = New SqlCommand("spRequestPasswordReset", objConn)
        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@Email", strEmailAddress)
        Dim objDataReader As SqlDataReader
        objDataReader = objCommand.ExecuteReader()

        If objDataReader.Read() Then
            strGuidResult = HelpFunction.Convertdbnulls(objDataReader("PasswordResetKey"))
        End If

        objDataReader.Close()
        objCommand.Dispose()
        objCommand = Nothing
        objDBConStringHelp.FinalizeConnection(objConn)  'close the connection
        objConn = Nothing
        lblResetPassword.Text = "Password reset email sent to " & strEmailAddress & "."
        Dim ex As Exception = UserValidation.SendUserPasswordResetLink(strEmailAddress, strGuidResult)

        If Not ex Is Nothing Then
            pnlMessage.Visible = True
            lblResetPassword.Text = ""
            lblMessage.Text = "Failed to send password reset email. " & ex.Message
        End If
    End Sub
End Class