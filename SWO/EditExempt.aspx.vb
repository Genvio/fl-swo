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


Partial Class EditExempt
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
            PopulateDDLs()

            Dim localExemptID As String = Request("ExemptID")

            If localExemptID = 0 Then
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
            Dim localComments As String = ""
            Dim localIncidentTypeID As Integer

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectExemptByExemptID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@ExemptID", Request("ExemptID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                localComments = HelpFunction.Convertdbnulls(objDR("Comments"))
                localIncidentTypeID = HelpFunction.ConvertdbnullsInt(objDR("IncidentTypeID"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            txtComments.Text = localComments
            ddlIncidentType.SelectedValue = localIncidentTypeID
        Catch ex As Exception
            Response.Write(ex.ToString)

            Exit Sub
        End Try
    End Sub

    Sub PopulateDDLs()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectIncidentType", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        'objCmd.Parameters.AddWithValue("@OrderBy", "")' Optional parameter.

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        ddlIncidentType.DataSource = objCmd.ExecuteReader()
        ddlIncidentType.DataBind()

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        objCmd = Nothing

        ddlIncidentType.Items.Insert(0, New ListItem("Select an Incident Type", "0"))
        ddlIncidentType.Items(0).Selected = True
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
            If lblAddEdit.Text = "Add" Then
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionExempt", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@ExemptID", 0)
                objCmd.Parameters.AddWithValue("@Comments", txtComments.Text)
                objCmd.Parameters.AddWithValue("@IncidentTypeID", ddlIncidentType.SelectedValue)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                'Record and insert audit information.
                AuditAction = "Added Exempt: Incident Type = " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentType", "IncidentType", "IncidentTypeID", ddlIncidentType.SelectedValue)
                AuditAction = AuditAction & " AND "
                AuditAction = AuditAction & "Comments = " & txtComments.Text

                AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "1")

                Response.Redirect("Exempt.aspx?message=1")
            Else
                Dim localComments As String = ""
                Dim localIncidentTypeID As Integer
                Dim localIncidentType As String = ""

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objConn.Open()
                objCmd = New SqlCommand("spSelectExemptByExemptID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@ExemptID", Request("ExemptID"))

                objDR = objCmd.ExecuteReader

                If objDR.Read() Then
                    localComments = HelpFunction.Convertdbnulls(objDR("Comments"))
                    localIncidentTypeID = HelpFunction.ConvertdbnullsInt(objDR("IncidentTypeID"))
                End If

                objDR.Close()

                objCmd.Dispose()
                objCmd = Nothing

                objConn.Close()

                localIncidentType = MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentType", "IncidentType", "IncidentTypeID", localIncidentTypeID)

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionExempt", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@ExemptID", Request("ExemptID"))
                objCmd.Parameters.AddWithValue("@Comments", txtComments.Text)
                objCmd.Parameters.AddWithValue("@IncidentTypeID", ddlIncidentType.SelectedValue)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                If localComments <> txtComments.Text Or ddlIncidentType.SelectedValue <> localIncidentTypeID Then
                    AuditAction = "Edited Exempt: "

                    If localComments <> txtComments.Text Then
                        AuditAction = AuditAction & "Changed Comments from """ & localComments & """ to """ & txtComments.Text & """"
                    End If

                    If localComments <> txtComments.Text And ddlIncidentType.SelectedValue <> localIncidentTypeID Then
                        AuditAction = AuditAction & " AND "
                    End If

                    If ddlIncidentType.SelectedValue <> localIncidentTypeID Then
                        AuditAction = AuditAction & "Incident Type from """ & localIncidentType & """ to """ & ddlIncidentType.SelectedItem.ToString & """"
                    End If

                    AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "2")

                    Response.Redirect("Exempt.aspx?message=3")
                Else

                End If
            End If
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("Exempt.aspx")
    End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        'Adding the appropriate errors to the error string.
        If txtComments.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Comment. <br />")
            globalHasErrors = True
        End If

        If ddlIncidentType.SelectedIndex = 0 Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide an Incident Type. <br />")
            globalHasErrors = True
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label.
        lblMessage.Text = strError.ToString
    End Sub
End Class