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

Partial Class Menus_Nav
    Inherits System.Web.UI.UserControl

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

        Dim localUserID As String = ns.UserID.ToString()
        Dim localAcknowledge = MrDataGrabber.GrabBitColumnByKey("Acknowledge", "Acknowledge", "UserID", localUserID)

        If localAcknowledge = True Then
            pnlShowAll.Visible = True

            Select Case ns.UserLevelID.ToString() 'oCookie.Item("UserLevelID")
                Case "1" 'Admin.
                    pnlShowAdminNav.Visible = True
                Case "2" 'User.
                    pnlShowFullUser.Visible = True
                Case "3" 'Read Only.
                    pnlShowUpdateUser.Visible = True
                Case "4", "5" 'Read Only and Read Only + Hazmat.
                    pnlShowReadOnly.Visible = True
                Case Else

            End Select
        Else
            pnlShowAll.Visible = False
        End If

        If Page.IsPostBack = False Then

        End If
    End Sub
End Class