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

Partial Class EditUserNonAdmin
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

    Dim globalHasErrors As Boolean = False

    'Public ObjCookie As System.Web.HttpCookie
    'Dim oCookie As System.Web.HttpCookie
    Dim ns As New SecurityValidate

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            PopulateDDLs()

            'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
            'Add cookie.
            'Response.Cookies.Add(oCookie)
            ns = Session("Security_Tracker")

            'Set message.
            Dim localUserID As String = ns.UserID.ToString()

            'Edit the recon report.
            lblAddEdit.Text = "Edit "
            btnSave.Value = "Save"

            PopulatePage()
        End If
    End Sub

    Sub PopulatePage()
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        'Set message.
        Dim localUserID As String = ns.UserID.ToString()

        Dim localFirstName As String = ""
        Dim localLastName As String = ""
        Dim localEmail As String = ""
        Dim localUsername As String = ""
        Dim localPassword As String = ""
        Dim localUserLevelID As Integer = 0
        Dim localIsActive As Boolean = False
        Dim localSecretQuestionID As Integer = 0
        Dim localSecretAnswer As String = ""

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectUserByUserID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@UserID", localUserID)

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            localUserLevelID = HelpFunction.ConvertdbnullsInt(objDR("UserLevelID"))
            localFirstName = HelpFunction.Convertdbnulls(objDR("FirstName"))
            localLastName = HelpFunction.Convertdbnulls(objDR("LastName"))
            localPassword = HelpFunction.Convertdbnulls(objDR("Password"))
            localEmail = HelpFunction.Convertdbnulls(objDR("Email"))
            localIsActive = HelpFunction.ConvertdbnullsBool(objDR("IsActive"))
            localSecretQuestionID = HelpFunction.ConvertdbnullsInt(objDR("SecretQuestionID"))
            localSecretAnswer = HelpFunction.Convertdbnulls(objDR("SecretAnswer"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)
        objConn.Close()

        ddlUserLevel.SelectedValue = localUserLevelID
        txtPassword.Attributes("value") = localPassword
        txtConfirmPassword.Attributes("value") = localPassword
        txtEmail.Text = localEmail
        txtFirstName.Text = localFirstName
        txtLastName.Text = localLastName
        chkIsActive.Checked = localIsActive
        ddlSecretQuestion.SelectedValue = localSecretQuestionID
        txtSecretAnswer.Text = localSecretAnswer
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
        ddlUserLevel.Items.Insert(0, New ListItem("Select A User Level", "0"))
        ddlUserLevel.Items(0).Selected = True

        'Secret questions.
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
    End Sub

    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As EventArgs)
        btnSave.Disabled = False
        btnCancel.Disabled = False

        ErrorChecks()

        If globalHasErrors = False Then
            'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
            'Add cookie.
            'Response.Cookies.Add(oCookie)
            ns = Session("Security_Tracker")

            'Set message.
            Dim localUserID As String = ns.UserID.ToString()

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionUser", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@UserID", localUserID)
            objCmd.Parameters.AddWithValue("@Password", txtPassword.Text)
            objCmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text)
            objCmd.Parameters.AddWithValue("@LastName", txtLastName.Text)
            objCmd.Parameters.AddWithValue("@Email", txtEmail.Text)
            objCmd.Parameters.AddWithValue("@DatePasswordChanged", Now)
            objCmd.Parameters.AddWithValue("@UserLevelID", ddlUserLevel.SelectedValue)
            objCmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked)
            objCmd.Parameters.AddWithValue("@SecretQuestionID", ddlSecretQuestion.SelectedValue)
            objCmd.Parameters.AddWithValue("@SecretAnswer", txtSecretAnswer.Text)
            objCmd.Parameters.AddWithValue("@AgencyID", MrDataGrabber.GrabIntegerByKey("User", "AgencyID", "UserID", localUserID))


            'Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            'Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()

            'Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            Response.Redirect("Home.aspx")
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("Home.aspx")
    End Sub

    Protected Sub ErrorChecks()
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        'Set message.
        Dim localUserID As String = ns.UserID.ToString()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        'Adding the appropriate errors to the error string.
        If txtFirstName.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a First Name. <br />")
            globalHasErrors = True
        End If


        If ddlUserLevel.SelectedValue = "0" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a User Level. <br />")
            globalHasErrors = True
        End If

        If ddlSecretQuestion.SelectedValue = "0" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a Secret Question. <br />")
            globalHasErrors = True
        End If

        If txtLastName.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Last Name. <br />")
            globalHasErrors = True
        End If

        If txtSecretAnswer.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Secret Answer. <br />")
            globalHasErrors = True
        End If

        If txtEmail.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a valid Email. <br />")
            globalHasErrors = True
        End If

        If txtPassword.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a valid Password. <br />")
            globalHasErrors = True
        End If

        If txtConfirmPassword.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must confirm your Password. <br />")
            globalHasErrors = True
        End If

        If txtConfirmPassword.Text <> txtPassword.Text Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; Passwords do not match. Please re-enter passwords. <br />")
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

            DBConStringHelper.FinalizeConnection(objConn)
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
            objCmd.Parameters.AddWithValue("@UserID", localUserID)

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localEmail2 = HelpFunction.Convertdbnulls(objDR("Email"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            DBConStringHelper.FinalizeConnection(objConn)
            objConn.Close()
        Catch ex As Exception
            Response.Write(ex.ToString)

            Exit Sub
        End Try

        If localEmail <> "" Then
            If localEmail2 <> localEmail Then
                'This checks to see if it is the person's own email.
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; This Email is already taken by another user. You must enter another Email. <br />")
                globalHasErrors = True
            End If
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label.
        lblMessage.Text = strError.ToString
    End Sub
End Class