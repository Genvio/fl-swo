Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Threading

Public Class ForbiddenLogHandler
    Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As _
            System.Web.HttpContext) Implements _
            System.Web.IHttpHandler.ProcessRequest
        Dim request As HttpRequest = context.Request
        Dim response As HttpResponse = context.Response
        ' This handler is called whenever a file ending 
        ' in .sample is requested. A file with that extension
        ' does not need to exist.
        response.Write("<html>")
        response.Write("<body>")
        response.Write("<p><H1>We know who you are...</H1></p>")
        response.Write(("Your IP is" & (request.UserHostAddress & "<br>")))
        response.Write(("Your domain is" & (request.UserHostName & "<br>")))
        response.Write("<p>Why were you requesting a restricted resource?</p>")
        response.Write("</body>")
        response.Write("</html>")
    End Sub

    Public ReadOnly Property IsReusable() As Boolean _
            Implements System.Web.IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class


