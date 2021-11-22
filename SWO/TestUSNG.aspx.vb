
Partial Class TestUSNG
    Inherits System.Web.UI.Page


    Protected Sub ButtonToUSNG_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonToUSNG.Click

        Dim usng As New USNG

        Dim p() = TextBoxLL.Text.Split(",")
        LiteralUSNG.Text = usng.LLtoUSNG(p(0), p(1), TextBoxPrecision.Text)

    End Sub

    Protected Sub ButtonToLL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonToLL.Click

        Dim usng As New USNG

        Dim d() = usng.USNGtoLL(TextBoxUSNG.Text)

        LiteralLL.Text = String.Format("{0}, {1}", d(0), d(1))

    End Sub
End Class
