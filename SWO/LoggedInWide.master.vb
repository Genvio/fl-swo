
Partial Class LoggedInWide
    Inherits System.Web.UI.MasterPage

    'Cookie for the Login Info
    'Public ObjCookie As System.Web.HttpCookie
    Dim ns As New SecurityValidate

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'set the properties for reference
        'ObjCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        ns = Session("Security_Tracker")
        '---------------------------------------------------------------------------------------------------------------------
        Try

            '-  Extract UserGroup
            '---------------------------------------------------------------------------------------------------------------------

            lblFullName.Text = ns.FullName
            lblUserLevel.Text = ns.UserLevel

            If Not ns.UserLevelID.Equals("1") Then

            End If

            '---------------------------------------------------------------------------------------------------------------------

        Catch ex As Exception
            'they are not logged in
            response.Redirect("Default.aspx")
        End Try

    End Sub

End Class