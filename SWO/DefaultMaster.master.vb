

Partial Class Master_DefaultMaster
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim strApplicationName As String

        Select Case Application("ApplicationEnvironment").ToString
            Case "SWODEV"
                strApplicationName = " (development)"
            Case "SWOX"
                strApplicationName = " (exercise)"
            Case "SWOTEST"
                strApplicationName = " (testing)"
            Case Else
                strApplicationName = String.Empty
        End Select

        lblAppName.Text = strApplicationName 'IIf(Application("ApplicationEnvironment").ToString = "SWO", "", " (" + Application("ApplicationEnvironment").ToString + ")")
        Page.Title = "Incident Tracker" + strApplicationName 'IIf(Application("ApplicationEnvironment").ToString = "SWO", "", " (" + Application("ApplicationEnvironment").ToString + ")")
    End Sub
End Class

