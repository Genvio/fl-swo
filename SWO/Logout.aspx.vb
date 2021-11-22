
Partial Class Logout
    Inherits System.Web.UI.Page
    Public objSecurity As New UserValidation

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'drop all cookies and session variables if they logged out...
        objSecurity.Logout()

    End Sub

End Class
