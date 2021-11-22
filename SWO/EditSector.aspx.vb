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

Partial Class EditSector
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

    Public objDataGridFunctions As New DataGridFunctions
    Public MrDataGrabber As New DataGrabber

    Dim ParamId As SqlParameter

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
            Dim localSectorID As String = Request("SectorID")

            If Request("Action") = "Delete" Then
                If Request("Parameter") = "SectorPosition" Then
                    DeletePosition()
                End If
            End If

            PopulateDDLs()

            If localSectorID = 0 Then
                lblAddEdit.Text = "Add "
                btnSave.Value = "Add"
            Else
                lblAddEdit.Text = "Edit "
                btnSave.Value = "Save"

                PopulatePage()
            End If

            getPositions()
        End If
    End Sub

    Sub PopulatePage()
        Try
            Dim localSectorName As String = ""

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectSectorBySectorID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@SectorID", Request("SectorID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localSectorName = HelpFunction.Convertdbnulls(objDR("SectorName"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            txtSectorName.Text = localSectorName
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
                objCmd = New SqlCommand("spActionSector", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@SectorID", 0)
                objCmd.Parameters.AddWithValue("@SectorName", txtSectorName.Text)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                AuditAction = "Added Sector: Name = " & txtSectorName.Text

                AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "1")

                Response.Redirect("Sector.aspx?message=1")
            Else
                Dim localSectorName As String = ""

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objConn.Open()
                objCmd = New SqlCommand("spSelectSectorBySectorID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@SectorID", Request("SectorID"))

                objDR = objCmd.ExecuteReader

                If objDR.Read() Then
                    localSectorName = HelpFunction.Convertdbnulls(objDR("SectorName"))
                End If

                objDR.Close()

                objCmd.Dispose()
                objCmd = Nothing

                objConn.Close()

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionSector", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@SectorID", Request("SectorID"))
                objCmd.Parameters.AddWithValue("@SectorName", txtSectorName.Text)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                If localSectorName <> txtSectorName.Text Then
                    AuditAction = "Edited Sector: "

                    If localSectorName <> txtSectorName.Text Then
                        AuditAction = AuditAction & "Changed Name from """ & localSectorName & """ to """ & txtSectorName.Text & """"
                    End If

                    AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "2")

                    Response.Redirect("Sector.aspx?message=3")
                Else
                    Response.Redirect("Sector.aspx?message=3")
                End If
            End If
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("Sector.aspx")
    End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        'Adding the appropriate errors to the error string.
        If txtSectorName.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Name. <br />")
            globalHasErrors = True
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if ny" to the label.
        lblMessage.Text = strError.ToString
    End Sub

    Sub PopulateDDLs()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectNotificationPosition", objConn)
        objCmd.CommandType = CommandType.StoredProcedure

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        ddlSectorPosition.DataSource = objCmd.ExecuteReader()
        ddlSectorPosition.DataBind()

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)
        objCmd = Nothing

        'Add a "Select an Option" item to the list.
        ddlSectorPosition.Items.Insert(0, New ListItem("Select a Position", "0"))
        ddlSectorPosition.Items(0).Selected = True
    End Sub

    Protected Sub getPositions()
        If objConn.State = ConnectionState.Open Then
            objConn.Close()
        End If

        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("spSelectSectorPositionBySectorID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@SectorID", Request("SectorID"))

        objDS.Tables.Clear()

        'Bind our data.
        objDA = New SqlDataAdapter(objCmd)
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)

        'Call the calculate grid counts to show the number of records, the page you are on, etc.
        objDataGridFunctions.CalcDataGridCounts(objDS, SectorPositionDataGrid, "")

        'Associate the data grid with the data.
        SectorPositionDataGrid.DataSource = objDS.Tables(0).DefaultView
        SectorPositionDataGrid.DataBind()

        objDataGridFunctions.Highlightrows(SectorPositionDataGrid, "", "", "")

        If CInt(objDS.Tables(0).Rows.Count) <> 0 Then
            'We have records so show the grid.
            pnlShowSectorPositionGrid.Visible = True
        Else
            'Hide grid.
            pnlShowSectorPositionGrid.Visible = False
        End If
    End Sub

    Private Sub DeletePosition()
        Try
            Dim AuditInfo As String = ""
            Dim AuditAction As String = ""

            'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
            'Add cookie.
            'Response.Cookies.Add(oCookie)
            ns = Session("Security_Tracker")

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Enter the email and password to query/command object.
            objCmd = New SqlCommand("spDeleteSectorPositionByIDs", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@SectorID", Request("SectorID"))
            objCmd.Parameters.AddWithValue("@SectorPositionID", Request("SectorPositionID"))

            'Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            'Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()

            'Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

            Response.Redirect("EditSector.aspx?Message=2&SectorID=" & Request("SectorID"))
        Catch ex As Exception
            DBConStringHelper.FinalizeConnection(objConn)

            lblMessage.Text = "You may not delete Position due to the fact it is tied to related imported information. You must first delete all related imported information first, and then you may delete the Position."
            lblMessage.Visible = True
            lblMessage.ForeColor = Drawing.Color.Red
        End Try
    End Sub

    Protected Sub btnAddPosition_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddPosition.Click
        ErrorChecks2()

        If globalHasErrors = False Then
            'Response.Write(lblAddEdit.Text)
            'Response.End()

            If lblAddEdit.Text = "Add " Then
                Dim TempInsertedSectorID As String

                Try
                    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                    'Enter the email and password to query/command object.
                    objCmd = New SqlCommand("spInsertSectorAndReturnID", objConn)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Parameters.AddWithValue("@SectorID", 0)
                    objCmd.Parameters.AddWithValue("@SectorName", txtSectorName.Text)

                    ParamId = objCmd.Parameters.AddWithValue("@SectorID_out", System.Data.SqlDbType.Int)
                    ParamId.Direction = System.Data.ParameterDirection.Output

                    DBConStringHelper.PrepareConnection(objConn)

                    objCmd.ExecuteNonQuery()

                    TempInsertedSectorID = objCmd.Parameters("@SectorID_out").Value

                    objCmd.Dispose()
                    objCmd = Nothing
                    DBConStringHelper.FinalizeConnection(objConn)
                Catch ex As Exception
                    Response.Write(ex.ToString)

                    Exit Sub
                End Try

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spInsertSectorPosition", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@SectorID", TempInsertedSectorID)
                objCmd.Parameters.AddWithValue("@SectorPositionID", ddlSectorPosition.SelectedValue)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                Response.Redirect("EditSector.aspx?SectorID=" & TempInsertedSectorID)
            Else
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionSector", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@SectorID", Request("SectorID"))
                objCmd.Parameters.AddWithValue("@SectorName", txtSectorName.Text)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                'Enter the email and password to query/command object.
                objCmd = New SqlCommand("spInsertSectorPosition", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@SectorID", Request("SectorID"))
                objCmd.Parameters.AddWithValue("@SectorPositionID", ddlSectorPosition.SelectedValue)

                'Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                'Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()

                'Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                Response.Redirect("EditSector.aspx?SectorID=" & Request("SectorID"))
            End If
        Else
            pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub ErrorChecks2()
        Dim strError As New System.Text.StringBuilder

        'Start the error string.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        If txtSectorName.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Sector Name. <br />")
            globalHasErrors = True
        End If

        If ddlSectorPosition.SelectedValue = "0" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must select a Position. <br />")
            globalHasErrors = True
        End If

        'Finish the error string.
        strError.Append("</span></font><br />")

        'Add errors "if any" to the label.
        lblMessage.Text = strError.ToString
    End Sub



End Class
