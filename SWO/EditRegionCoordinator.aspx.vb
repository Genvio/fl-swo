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

Partial Class EditRegionCoordinator
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

            Dim localRegionCoordinatorID As String = Request("RegionCoordinatorID")

            If localRegionCoordinatorID = 0 Then
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
            Dim localRegionCoordinatorName As String = ""
            Dim localEmail As String = ""
            Dim localRegionID As String = ""

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectRegionCoordinatorByRegionCoordinatorID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@RegionCoordinatorID", Request("RegionCoordinatorID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localRegionID = HelpFunction.ConvertdbnullsInt(objDR("RegionID"))
                localRegionCoordinatorName = HelpFunction.Convertdbnulls(objDR("RegionCoordinatorName"))
                localEmail = HelpFunction.Convertdbnulls(objDR("Email"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            ddlRegion.SelectedValue = localRegionID
            txtRegionCoordinatorName.Text = localRegionCoordinatorName
            txtEmail.Text = localEmail
        Catch ex As Exception
            Response.Write(ex.ToString)

            Exit Sub
        End Try
    End Sub

    Sub PopulateDDLs()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectRegion", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        'objCmd.Parameters.AddWithValue("@OrderBy", "") 'Optional parameter.

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        ddlRegion.DataSource = objCmd.ExecuteReader()
        ddlRegion.DataBind()

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        objCmd = Nothing

        'Add a "Select an Option" item to the list.
        ddlRegion.Items.Insert(0, New ListItem("Select a Region", "0"))
        ddlRegion.Items(0).Selected = True
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
                objCmd = New SqlCommand("spActionRegionCoordinator", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@RegionCoordinatorID", 0)
                objCmd.Parameters.AddWithValue("@RegionCoordinatorName", txtRegionCoordinatorName.Text)
                objCmd.Parameters.AddWithValue("@Email", txtEmail.Text)
                objCmd.Parameters.AddWithValue("@RegionID", ddlRegion.SelectedValue)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                AuditAction = "Added Region Coordinator: Name = " & txtRegionCoordinatorName.Text
                AuditAction = AuditAction & " AND "
                AuditAction = AuditAction & "Region = " & ddlRegion.SelectedItem.ToString

                AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "1")

                Response.Redirect("RegionCoordinator.aspx?message=1")
            Else
                Dim localRegionID As Integer
                Dim localRegionCoordinatorName As String = ""
                Dim localRegion As String = ""

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objConn.Open()
                objCmd = New SqlCommand("spSelectRegionCoordinatorByRegionCoordinatorID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@RegionCoordinatorID", Request("RegionCoordinatorID"))

                objDR = objCmd.ExecuteReader

                If objDR.Read() Then
                    localRegionID = HelpFunction.ConvertdbnullsInt(objDR("RegionID"))
                    localRegionCoordinatorName = HelpFunction.Convertdbnulls(objDR("RegionCoordinatorName"))
                End If

                objDR.Close()

                objCmd.Dispose()
                objCmd = Nothing

                objConn.Close()

                localRegion = MrDataGrabber.GrabOneStringColumnByPrimaryKey("Region", "Region", "RegionID", localRegionID.ToString)

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionRegionCoordinator", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@RegionCoordinatorID", Request("RegionCoordinatorID"))
                objCmd.Parameters.AddWithValue("@RegionCoordinatorName", txtRegionCoordinatorName.Text)
                objCmd.Parameters.AddWithValue("@Email", txtEmail.Text)
                objCmd.Parameters.AddWithValue("@RegionID", ddlRegion.SelectedValue)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                If localRegionCoordinatorName <> txtRegionCoordinatorName.Text Or localRegionID <> ddlRegion.SelectedValue Then
                    AuditAction = "Edited Region Coordinator: "

                    If localRegionCoordinatorName <> txtRegionCoordinatorName.Text Then
                        AuditAction = AuditAction & "Changed Name from """ & localRegionCoordinatorName & """ to """ & txtRegionCoordinatorName.Text & """"
                    End If

                    If localRegionID <> ddlRegion.SelectedValue And txtRegionCoordinatorName.Text <> localRegionCoordinatorName Then
                        AuditAction = AuditAction & " AND "
                    End If

                    If localRegionID <> ddlRegion.SelectedValue Then
                        AuditAction = AuditAction & "Region from """ & localRegion & """ to """ & ddlRegion.SelectedItem.ToString & """"
                    End If


                    AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "2")

                    Response.Redirect("RegionCoordinator.aspx?message=3")
                Else
                    Response.Redirect("RegionCoordinator.aspx?message=3")
                End If
            End If
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("RegionCoordinator.aspx")
    End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        'Adding the appropriate errors to the error string.
        If txtRegionCoordinatorName.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Name. <br />")
            globalHasErrors = True
        End If

        If txtEmail.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide an Email. <br />")
            globalHasErrors = True
        End If

        If ddlRegion.SelectedValue = 0 Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a Region. <br />")
            globalHasErrors = True
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if ny" to the label.
        lblMessage.Text = strError.ToString
    End Sub
End Class