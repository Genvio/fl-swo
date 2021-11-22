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

Partial Class EditWorksheet
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

    Public objDataGridFunctions As New DataGridFunctions

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
            If Request("Action") = "Delete" Then
                DeleteWorksheetLevel()
            End If

            lblWorksheet.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentType", "IncidentType", "IncidentTypeID", Request("IncidentTypeID"))

            getLevels()

            Dim localExemptID As String = Request("ExemptID")
        End If
    End Sub

    Sub PopulatePage()
        'Try
        '    Dim localComments As String = ""
        '    Dim localIncidentTypeID As Integer

        '    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        '    objConn.Open()
        '    objCmd = New SqlCommand("spSelectExemptByExemptID", objConn)
        '    objCmd.CommandType = CommandType.StoredProcedure
        '    objCmd.Parameters.AddWithValue("@ExemptID", Request("ExemptID"))

        '    objDR = objCmd.ExecuteReader

        '    If objDR.Read() Then
        '        localComments = HelpFunction.Convertdbnulls(objDR("Comments"))
        '        localIncidentTypeID = HelpFunction.ConvertdbnullsInt(objDR("IncidentTypeID"))
        '    End If

        '    objDR.Close()

        '    objCmd.Dispose()
        '    objCmd = Nothing

        '    objConn.Close()

        '    txtComments.Text = localComments
        '    ddlIncidentType.SelectedValue = localIncidentTypeID
        'Catch ex As Exception
        '    Response.Write(ex.ToString)

        '    Exit Sub
        'End Try
    End Sub

    Protected Sub getLevels()
        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("spSelectIncidentTypeLevelByIncidentTypeID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentTypeID", Request("IncidentTypeID"))

        objDS.Tables.Clear()

        'Bind our data.
        objDA = New SqlDataAdapter(objCmd)
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        'Call the calculate grid counts to show the number of records, the page you are on, etc.
        objDataGridFunctions.CalcDataGridCounts(objDS, IncidentTypeLevelDataGrid, "")

        'Associate the data grid with the data.
        IncidentTypeLevelDataGrid.DataSource = objDS.Tables(0).DefaultView
        IncidentTypeLevelDataGrid.DataBind()

        objDataGridFunctions.Highlightrows(IncidentTypeLevelDataGrid, "", "", "")

        If CInt(objDS.Tables(0).Rows.Count) <> 0 Then
            'We have records so show the grid.
            pnlShowIncidentTypeLevelGrid.Visible = True
        Else
            'Hide the grid.
            pnlShowIncidentTypeLevelGrid.Visible = False
        End If
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
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spInsertIncidentTypeLevel", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@Number", txtNumber.Text)
            objCmd.Parameters.AddWithValue("@LevelDescription", txtLevelDescription.Text)
            objCmd.Parameters.AddWithValue("@IncidentTypeID", Request("IncidentTypeID"))

            'Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            'Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()

            'Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            AuditAction = "Added Worksheet Level: " & txtNumber.Text & "to Worksheet = " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentType", "IncidentType", "IncidentTypeID", Request("IncidentTypeID"))
            AuditAction = AuditAction & " with "
            AuditAction = AuditAction & "Level Description = " & txtLevelDescription.Text

            AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "1")

            Response.Redirect("EditWorksheet.aspx?IncidentTypeID=" & Request("IncidentTypeID"))
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("Worksheets.aspx")
    End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start the error String.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        'Adding the appropriate errors to the error string.
        If txtNumber.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Number. <br />")
            globalHasErrors = True
        End If

        If txtNumber.Text <> "" Then
            Try
                Dim localNumber As Integer = CInt(txtNumber.Text)
            Catch ex As Exception
                strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Valid Number. <br />")
                globalHasErrors = True
            End Try
        End If

        If txtLevelDescription.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Level Description. <br />")
            globalHasErrors = True
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label.
        lblMessage.Text = strError.ToString
    End Sub

    Private Sub DeleteWorksheetLevel()
        Try
            Dim AuditInfo As String = ""
            Dim AuditAction As String = ""
            Dim localIncidentTypeID As Integer

            'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
            'Add cookie.
            'Response.Cookies.Add(oCookie)
            ns = Session("Security_Tracker")

            Dim localNumber As Integer = MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("Number", "IncidentTypeLevel", "IncidentTypeLevelID", Request("IncidentTypeLevelID"))

            'Response.Write("It Worked")
            'Response.End()

            AuditAction = "Deleted Level: " & localNumber & " From WorkSheet: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentType", "IncidentType", "IncidentTypeID", localIncidentTypeID) & " "

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spDeleteIncidentTypeLevelByIncidentTypeLevelID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", Request("IncidentTypeLevelID"))


            'Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            'Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()

            'Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            'Insert the audit information.
            Response.Write(Request("It Worked"))
            AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "3")

            Response.Redirect("EditWorksheet.aspx?Message=2&IncidentTypeID=" & Request("IncidentTypeID"))
        Catch ex As Exception
            DBConStringHelper.FinalizeConnection(objConn)

            lblMessage.Text = "You may not delete this Worksheet Level due to the fact it is tied to related imported information. You must first delete all related imported information first, and then you may delete the Worksheet Level."
            lblMessage.Visible = True
            lblMessage.ForeColor = Drawing.Color.Red
        End Try
    End Sub
End Class