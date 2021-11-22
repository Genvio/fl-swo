
Partial Class Master_LoggedIn
    Inherits System.Web.UI.MasterPage

    'Cookie for the Login Info
    'Public ObjCookie As System.Web.HttpCookie
    'Public ns As New SecurityValidate
    Dim ns As New SecurityValidate

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'set the properties for reference
        'ObjCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        ns = Session("Security_Tracker")

        Try
            lblFullName.Text = ns.FullName 'ObjCookie.Item("FullName")
            lblUserLevel.Text = ns.UserLevel 'ObjCookie.Item("UserLevel")
        Catch ex As Exception
            'they are not logged in
            Response.Redirect("Default.aspx")
        End Try

        Dim strApplicationName As String

        Select Case Application("ApplicationEnvironment").ToString
            Case "SWODEV"
                strApplicationName = " (development)"
            Case "SWOX"
                strApplicationName = " (exercise)"
            Case "SWOtest"
                strApplicationName = " (testing)"
            Case Else
                strApplicationName = String.Empty
        End Select

        'lblAppName.Text = strApplicationName 'Application("ApplicationEnvironment").ToString
        Page.Title = "Incident Tracker" + strApplicationName 'Application("ApplicationEnvironment").ToString + " Incident Tracker"
    End Sub


End Class

