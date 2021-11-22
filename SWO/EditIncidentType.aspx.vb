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

Partial Class EditIncidentType
    Inherits System.Web.UI.Page
    'Help Functions from our App_Code
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    Public AuditHelper As New AuditHelp

    'For Connecting to the database
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet

    Dim globalHasErrors As Boolean = False

    'Cookie for the Login Info
    'Public ObjCookie As System.Web.HttpCookie
    'Dim oCookie As System.Web.HttpCookie
    Dim ns As New SecurityValidate

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        If ns.UserLevelID <> "1" Then 'oCookie.Item("UserLevelID").ToString.Trim <> "1" Then
            Response.Redirect("Home.aspx")
        End If

        If Page.IsPostBack = False Then

            Dim localIncidentTypeID As String = Request("IncidentTypeID")

            If localIncidentTypeID = 0 Then

                lblAddEdit.Text = "Add"
                btnSave.Value = "Add Incident Type"
            Else

                lblAddEdit.Text = "Edit"
                btnSave.Value = "Edit Incident Type"
                PopulatePage()

            End If


        End If

    End Sub

    Sub PopulatePage()


        Try
            Dim localIncidentType As String = ""
            Dim localNotes As String = ""


            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectIncidentTypeByIncidentTypeID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentTypeID", Request("IncidentTypeID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then


                localIncidentType = HelpFunction.Convertdbnulls(objDR("IncidentType"))


            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            txtIncidentType.Text = localIncidentType



        Catch ex As Exception

            Response.Write(ex.ToString)
            Exit Sub

        End Try

    End Sub

    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As EventArgs)
        'Response.Write(ddlUserLevel.SelectedValue)
        'Response.End()
        Dim AuditInfo As String = ""
        Dim AuditAction As String = ""

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        btnSave.Disabled = False
        btnCancel.Disabled = False

        ErrorChecks()


        If globalHasErrors = False Then


            If lblAddEdit.Text = "Add" Then

                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                '// Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionIncidentType", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentTypeID", 0)
                objCmd.Parameters.AddWithValue("@IncidentType", txtIncidentType.Text)


                '// Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                '// Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()
                '// Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)

                AuditAction = "Added Incident Type " & txtIncidentType.Text

                AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "1")

                Response.Redirect("IncidentType.aspx?message=1")

            Else

                Dim localIncidentType As String = ""
                Dim localNotes As String = ""


                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objConn.Open()
                objCmd = New SqlCommand("spSelectIncidentTypeByIncidentTypeID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentTypeID", Request("IncidentTypeID"))

                objDR = objCmd.ExecuteReader

                If objDR.Read() Then


                    localIncidentType = HelpFunction.Convertdbnulls(objDR("IncidentType"))


                End If

                objDR.Close()

                objCmd.Dispose()
                objCmd = Nothing

                objConn.Close()




                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                '// Enter the email and password to query/command object.
                objCmd = New SqlCommand("spActionIncidentType", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentTypeID", Request("IncidentTypeID"))
                objCmd.Parameters.AddWithValue("@IncidentType", txtIncidentType.Text)


                '// Open the connection using the connection string.
                DBConStringHelper.PrepareConnection(objConn)

                '// Execute the command to the DataReader.
                objCmd.ExecuteNonQuery()
                '// Clean up our command objects and close the connection.
                objCmd.Dispose()
                objCmd = Nothing
                DBConStringHelper.FinalizeConnection(objConn)



                AuditAction = "Edited Incident Type: Changed Incident Type from """ & localIncidentType & """ to """ & txtIncidentType.Text & """"

                AuditHelper.InsertAudit(ns.UserID.ToString.Trim, AuditAction, "2")

                Response.Redirect("IncidentType.aspx?message=3")

            End If

        Else

            pnlMessage.Visible = True

        End If

    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)

        Response.Redirect("IncidentType.aspx")

    End Sub

    Protected Sub ErrorChecks()

        Dim strError As New System.Text.StringBuilder

        'Start The Error String
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")


        'Adding the appropriate errors to the error string

        If txtIncidentType.Text = "" Then

            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Incident Type. <br />")
            globalHasErrors = True

        End If


        'Finish the Error String
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels
        lblMessage.Text = strError.ToString

    End Sub

End Class