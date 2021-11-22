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

Partial Class EditAssociatedTask
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
            Dim localAssociatedTaskID As String = Request("AssociatedTaskID")

            If localAssociatedTaskID = 0 Then
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
            Dim localAssociatedTaskName As String = ""
            Dim localAssociatedTask As String = ""

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectAssociatedTaskByAssociatedTaskID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@AssociatedTaskID", Request("AssociatedTaskID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localAssociatedTask = HelpFunction.Convertdbnulls(objDR("AssociatedTask"))
                localAssociatedTaskName = HelpFunction.Convertdbnulls(objDR("AssociatedTaskName"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            txtAssociatedTask.Text = localAssociatedTask
            txtAssociatedTaskName.Text = localAssociatedTaskName
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
                objCmd = New SqlCommand("spActionAssociatedTask", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@AssociatedTaskID", 0)
                objCmd.Parameters.AddWithValue("@AssociatedTask", txtAssociatedTask.Text)
                objCmd.Parameters.AddWithValue("@AssociatedTaskName", txtAssociatedTaskName.Text)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                'Record an insert audit information.
                AuditAction = "Added Associated Task: Name = " & txtAssociatedTaskName.Text
                AuditAction = AuditAction & " AND "
                AuditAction = AuditAction & "Task = " & txtAssociatedTask.Text

                AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "1")

                Response.Redirect("AssociatedTasks.aspx?message=1")
            Else
                Dim localAssociatedTask As String = ""
                Dim localAssociatedTaskName As String = ""

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objConn.Open()
                objCmd = New SqlCommand("spSelectAssociatedTaskByAssociatedTaskID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@AssociatedTaskID", Request("AssociatedTaskID"))

                objDR = objCmd.ExecuteReader

                If objDR.Read() Then
                    localAssociatedTask = HelpFunction.Convertdbnulls(objDR("AssociatedTask"))
                    localAssociatedTaskName = HelpFunction.Convertdbnulls(objDR("AssociatedTaskName"))
                End If

                objDR.Close()

                objCmd.Dispose()
                objCmd = Nothing

                objConn.Close()

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionAssociatedTask", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@AssociatedTaskID", Request("AssociatedTaskID"))
                objCmd.Parameters.AddWithValue("@AssociatedTask", txtAssociatedTask.Text)
                objCmd.Parameters.AddWithValue("@AssociatedTaskName", txtAssociatedTaskName.Text)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                If localAssociatedTask <> txtAssociatedTask.Text Or localAssociatedTaskName <> txtAssociatedTaskName.Text Then
                    AuditAction = "Edited Associated Task: "

                    If localAssociatedTask <> txtAssociatedTask.Text Then
                        AuditAction = AuditAction & "Changed Task from """ & localAssociatedTask & """ to """ & txtAssociatedTask.Text & """"
                    End If

                    If localAssociatedTaskName <> txtAssociatedTaskName.Text And txtAssociatedTaskName.Text <> localAssociatedTaskName Then
                        AuditAction = AuditAction & " AND "
                    End If

                    If txtAssociatedTaskName.Text <> localAssociatedTaskName Then
                        AuditAction = AuditAction & "Name from """ & localAssociatedTaskName & """ to """ & txtAssociatedTaskName.Text & """"
                    End If


                    AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "2")

                    Response.Redirect("AssociatedTasks.aspx?message=3")
                Else

                End If
            End If
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("AssociatedTasks.aspx")
    End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        'Adding the appropriate errors to the error string.
        If txtAssociatedTaskName.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Name. <br />")
            globalHasErrors = True
        End If

        If txtAssociatedTask.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Task. <br />")
            globalHasErrors = True
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label.
        lblMessage.Text = strError.ToString
    End Sub
End Class