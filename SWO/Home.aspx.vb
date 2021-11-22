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

Partial Class Home
    Inherits System.Web.UI.Page

    'Help functions from our App_Code.
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    Public objDataGridFunctions As New DataGridFunctions

    'For connecting to the database.
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet

    Dim globalHasErrors As Boolean = False
    Dim globalMessage As String

    'Dim oCookie As System.Web.HttpCookie
    Dim ns As New SecurityValidate

    Public MrDataGrabber As New DataGrabber

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        'Set message.
        If Request("Message") = "ProfileUpdated" Then
            lblMessage.Text = "Profile updated."
            pnlMessage.Visible = True
        End If

        Dim localUserID As String = ns.UserID.ToString()
        Dim localAcknowledge = MrDataGrabber.GrabBitColumnByKey("Acknowledge", "Acknowledge", "UserID", localUserID)

        If localAcknowledge = True Then
            pnlShowAcknowledge.Visible = False
            pnlShowAll.Visible = True

            Select Case ns.UserLevelID.ToString() 'oCookie.Item("UserLevelID")
                Case "1" 'Admin.
                    pnlShowAdmin.Visible = True
                    pnlShowReadOnly.Visible = False
                    pnlShowFullUser.Visible = False
                    pnlShowUpdateUser.Visible = False
                Case "2" 'Full User.
                    pnlShowFullUser.Visible = True
                    pnlShowAdmin.Visible = False
                    pnlShowReadOnly.Visible = False
                    pnlShowUpdateUser.Visible = False
                Case "3" 'Update User.
                    pnlShowUpdateUser.Visible = True
                    pnlShowAdmin.Visible = False
                    pnlShowReadOnly.Visible = False
                    pnlShowFullUser.Visible = False
                Case "4", "5" 'Read Only and Read Only + Hazmat.
                    pnlShowReadOnly.Visible = True
                    pnlShowAdmin.Visible = False
                    pnlShowUpdateUser.Visible = False
                    pnlShowFullUser.Visible = False
                Case Else

            End Select
        Else
            pnlShowAcknowledge.Visible = True
            pnlShowAll.Visible = False
        End If

        If Page.IsPostBack = False Then

        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        Dim localUserID As String = ns.UserID

        ErrorChecks()

        If globalHasErrors = False Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spInsertAcknowledge", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@DateAcknowledged", Now)
            objCmd.Parameters.AddWithValue("@Acknowledge", cbxAcknowledge.Checked)
            objCmd.Parameters.AddWithValue("@UserID", localUserID)

            'Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            'Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()

            'Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            'Added/edited on 1/26/12 by JD.
            '------------------------------------------------------
            If ns.UserLevelID <> "1" Then 'oCookie.Item("UserLevelID").ToString.Trim <> "1" Then
                Response.Redirect("MyProfile.aspx")
            Else
                Response.Redirect("Home.aspx")
            End If
            '------------------------------------------------------
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span style='color:#fe5105;'> ")

        'Adding the appropriate errors to the error string.
        If cbxAcknowledge.Checked = False Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must check the checkbox below in order to continue. <br />")
            globalHasErrors = True
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the labels.
        lblMessage.Text = strError.ToString
    End Sub
End Class