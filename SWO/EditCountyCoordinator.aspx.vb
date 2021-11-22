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

Partial Class EditCountyCoordinator
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

            Dim localCountyCoordinatorID As String = Request("CountyCoordinatorID")

            If localCountyCoordinatorID = 0 Then
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
            Dim localCountyCoordinatorName As String = ""
            Dim localEmail As String = ""
            Dim localCountyID As String = ""

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectCountyCoordinatorByCountyCoordinatorID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@CountyCoordinatorID", Request("CountyCoordinatorID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localCountyID = HelpFunction.ConvertdbnullsInt(objDR("CountyID"))
                localCountyCoordinatorName = HelpFunction.Convertdbnulls(objDR("CountyCoordinatorName"))
                localEmail = HelpFunction.Convertdbnulls(objDR("Email"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            ddlCounty.SelectedValue = localCountyID
            txtCountyCoordinatorName.Text = localCountyCoordinatorName
            txtEmail.Text = localEmail
        Catch ex As Exception
            Response.Write(ex.ToString)

            Exit Sub
        End Try
    End Sub

    Sub PopulateDDLs()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectCounty", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        'objCmd.Parameters.AddWithValue("@OrderBy", "") 'Optional parameter.

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        ddlCounty.DataSource = objCmd.ExecuteReader()
        ddlCounty.DataBind()

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        objCmd = Nothing

        'Add a "Select an Option" item to the list.
        ddlCounty.Items.Insert(0, New ListItem("Select A County", "0"))
        ddlCounty.Items(0).Selected = True
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
                objCmd = New SqlCommand("spActionCountyCoordinator", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@CountyCoordinatorID", 0)
                objCmd.Parameters.AddWithValue("@CountyCoordinatorName", txtCountyCoordinatorName.Text)
                objCmd.Parameters.AddWithValue("@Email", txtEmail.Text)
                objCmd.Parameters.AddWithValue("@CountyID", ddlCounty.SelectedValue)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                'Insert and record audit information.
                AuditAction = "Added County Coordinator: Name = " & txtCountyCoordinatorName.Text
                AuditAction = AuditAction & " AND "
                AuditAction = AuditAction & "County = " & ddlCounty.SelectedItem.ToString

                AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "1")

                Response.Redirect("CountyCoordinator.aspx?message=1")
            Else
                Dim localCountyID As Integer
                Dim localCountyCoordinatorName As String = ""
                Dim localCounty As String = ""

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objConn.Open()
                objCmd = New SqlCommand("spSelectCountyCoordinatorByCountyCoordinatorID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@CountyCoordinatorID", Request("CountyCoordinatorID"))

                objDR = objCmd.ExecuteReader

                If objDR.Read() Then
                    localCountyID = HelpFunction.ConvertdbnullsInt(objDR("CountyID"))
                    localCountyCoordinatorName = HelpFunction.Convertdbnulls(objDR("CountyCoordinatorName"))
                End If

                objDR.Close()

                objCmd.Dispose()
                objCmd = Nothing

                objConn.Close()

                localCounty = MrDataGrabber.GrabOneStringColumnByPrimaryKey("County", "County", "CountyID", localCountyID.ToString)

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionCountyCoordinator", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@CountyCoordinatorID", Request("CountyCoordinatorID"))
                objCmd.Parameters.AddWithValue("@CountyCoordinatorName", txtCountyCoordinatorName.Text)
                objCmd.Parameters.AddWithValue("@Email", txtEmail.Text)
                objCmd.Parameters.AddWithValue("@CountyID", ddlCounty.SelectedValue)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                If localCountyCoordinatorName <> txtCountyCoordinatorName.Text Or localCountyID <> ddlCounty.SelectedValue Then
                    AuditAction = "Edited County Coordinator: "

                    If localCountyCoordinatorName <> txtCountyCoordinatorName.Text Then
                        AuditAction = AuditAction & "Changed Name from """ & localCountyCoordinatorName & """ to """ & txtCountyCoordinatorName.Text & """"
                    End If

                    If localCountyID <> ddlCounty.SelectedValue And txtCountyCoordinatorName.Text <> localCountyCoordinatorName Then
                        AuditAction = AuditAction & " AND "
                    End If

                    If localCountyID <> ddlCounty.SelectedValue Then
                        AuditAction = AuditAction & "County from """ & localCounty & """ to """ & ddlCounty.SelectedItem.ToString & """"
                    End If

                    AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "2")

                    Response.Redirect("CountyCoordinator.aspx?message=3")
                Else
                    Response.Redirect("CountyCoordinator.aspx?message=3")
                End If
            End If
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("CountyCoordinator.aspx")
    End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        'Adding the appropriate errors to the error string.
        If txtCountyCoordinatorName.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Name. <br />")
            globalHasErrors = True
        End If

        If txtEmail.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide an Email. <br />")
            globalHasErrors = True
        End If

        If ddlCounty.SelectedValue = 0 Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a County. <br />")
            globalHasErrors = True
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the labels.
        lblMessage.Text = strError.ToString
    End Sub
End Class