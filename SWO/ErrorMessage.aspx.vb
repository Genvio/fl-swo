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

Partial Class ErrorMessage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'Try


        '    If Page.IsPostBack = False Then

        '        Dim ex As Exception = HttpContext.Current.Application.Get(("lastException"))

        '        'Response.Write(ex.Message)

        '        '// E-mails the user their new password
        '        Dim mailTo As String
        '        Dim mailFrom As String
        '        Dim mailSubject As String
        '        Dim smtpServer As String

        '        'mailTo = "richarddible@gmail.com"


        '        'mailTo = Replace(txtEmailList.Text, ";", ",")


        '        mailTo = "Kevin.Smith@dca.state.fl.us,Richard.Butgereit@em.myflorida.com"


        '        mailFrom = "EOC-TechServices@em.myflorida.com"

        '        mailSubject = "Error Incident Tracker"

        '        '// Enter JDE email server here
        '        smtpServer = ConfigurationManager.AppSettings("MailServer").ToString

        '        '// Create a new smtp mail message, and input our information.
        '        Dim msgEmail As New System.Net.Mail.MailMessage()
        '        msgEmail.To.Add(mailTo)
        '        msgEmail.From = New System.Net.Mail.MailAddress(mailFrom)
        '        msgEmail.Subject = mailSubject
        '        msgEmail.IsBodyHtml = True


        '        msgEmail.CC.Add("richie.dible@em.myflorida.com")
        '        'msgEmail.CC.Add("richard.butgereit@em.myflorida.com")

        '        msgEmail.Body = "Error: " & ex.Message



        '        Dim objEmail As New System.Net.Mail.SmtpClient(smtpServer)

        '        '// Try to send out the e-mail to the user.
        '        objEmail.Send(msgEmail)




        '        Response.Redirect("ErrorMessage2.aspx")

        '    End If


        'Catch ext As Exception
        '    Response.Write(ext.ToString)
        '    Exit Sub
        'End Try




    End Sub




End Class
