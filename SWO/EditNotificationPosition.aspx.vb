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

Partial Class EditNotificationPosition
    Inherits System.Web.UI.Page

    'Help functions from our App_Code.
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    Public AuditHelper As New AuditHelp

    'For connecting to the database.
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet

    Dim globalHasErrors As Boolean = False

    Public MrDataGrabber As New DataGrabber

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
            Dim localNotificationPositionID As String = Request("NotificationPositionID")

            If localNotificationPositionID = 0 Then
                lblAddEdit.Text = "Add "
                btnSave.Value = "Add"
            Else
                lblAddEdit.Text = "Edit "
                btnSave.Value = "Save"

                PopulatePage()
            End If
        End If
    End Sub

    Sub PopulatePage()
        Try
            Dim localNotificationPosition As String = ""
            Dim localEmail As String = ""

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectNotificationPositionByNotificationPositionID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@NotificationPositionID", Request("NotificationPositionID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localNotificationPosition = HelpFunction.Convertdbnulls(objDR("Position"))
                localEmail = HelpFunction.Convertdbnulls(objDR("Email"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            txtPosition.Text = localNotificationPosition
            txtEmail.Text = localEmail
        Catch ex As Exception
            Response.Write(ex.ToString)

            Exit Sub
        End Try
    End Sub

    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As EventArgs)
        Dim AuditInfo As String = ""
        Dim AuditAction As String = ""

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        btnSave.Disabled = False
        btnCancel.Disabled = False

        ErrorChecks()

        If globalHasErrors = False Then
            If lblAddEdit.Text = "Add " Then

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionNotificationPosition", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@NotificationPositionID", 0)
                objCmd.Parameters.AddWithValue("@Position", txtPosition.Text)
                objCmd.Parameters.AddWithValue("@Email", txtEmail.Text)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                AuditAction = "Added Position = " & txtPosition.Text

                AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "1")

                Response.Redirect("NotificationPosition.aspx?message=1")
            Else
                Dim localNotificationPosition As String = ""
                Dim localAbbreviation As String = ""

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objConn.Open()
                objCmd = New SqlCommand("spSelectNotificationPositionByNotificationPositionID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@NotificationPositionID", Request("NotificationPositionID"))

                objDR = objCmd.ExecuteReader

                If objDR.Read() Then
                    localNotificationPosition = HelpFunction.Convertdbnulls(objDR("Position"))
                End If

                objDR.Close()

                objCmd.Dispose()
                objCmd = Nothing

                objConn.Close()

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionNotificationPosition", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@NotificationPositionID", Request("NotificationPositionID"))
                objCmd.Parameters.AddWithValue("@Position", txtPosition.Text)
                objCmd.Parameters.AddWithValue("@Email", txtEmail.Text)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                If localNotificationPosition <> txtPosition.Text Then
                    AuditAction = "Edited Position: "

                    If localNotificationPosition <> txtPosition.Text Then
                        AuditAction = AuditAction & "Changed Position from """ & localNotificationPosition & """ to """ & txtPosition.Text & """"
                    End If

                    AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "2")
                Else

                End If

                Response.Redirect("NotificationPosition.aspx?message=3")
            End If
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("NotificationPosition.aspx")
    End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        'Adding the appropriate errors to the error string.
        If txtPosition.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Position. <br />")
            globalHasErrors = True
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label.
        lblMessage.Text = strError.ToString
    End Sub
End Class