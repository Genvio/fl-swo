Imports Microsoft.VisualBasic
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
Imports System.Web.HttpContext

Public Class Email

    Public Sub New()

    End Sub

    Protected Overrides Sub Finalize()
        ' Destructor
    End Sub

    Public Sub SendUpdateEmail(ByVal strSubject As String, ByVal strBody As String)
        Try
            '// E-mails the user their new password
            Dim objException As New Exception
            SendEmail(strSubject, strBody, "SWP@em.myflorida.com", "SWP@em.myflorida.com", False, objException)
        Catch ext As Exception
            Exit Sub
        End Try
    End Sub

    Public Shared Sub SendAdminInfoEmail(ByVal strSubject As String, ByVal strBody As String)
        Try
            Dim objException As New Exception
            Dim ns As New SecurityValidate

            Try
                ns = HttpContext.Current.Session("Security_Tracker")
                strBody = strBody & Chr(12) & "User: " & ns.FullName
                strBody = strBody & Chr(12) & "User Level: " & ns.UserLevel
                strBody = strBody & Chr(12) & "Agency: " & ns.Agency
                strBody = strBody & Chr(12) & "Email: " & ns.Email
            Catch ex As Exception
                strBody = strBody & Chr(12) & "User: not logged in"
                strBody = strBody & Chr(12) & "User Level: N/A"
                strBody = strBody & Chr(12) & "Agency: unknown"
                strBody = strBody & Chr(12) & "Email: unknown"
            End Try

            SendEmail(strSubject, strBody, ConfigurationManager.AppSettings("AdminInfoMessageToEmail"), "EOC-TechnicalServices@em.myflorida.com", True, objException)
        Catch ex As Exception
            'Swallow the error for now
        End Try
    End Sub

    ''' <summary>
    ''' Generic routine to send email message from system
    ''' </summary>
    ''' <param name="strSubject"></param>
    ''' <param name="strBody">Is set to HTML format.</param>
    ''' <param name="strToEmail">A single email address, or a comma-delimited list of email addresses.</param>
    ''' <param name="strFromEmail"></param>
    ''' <param name="blnIncludeMachineName">True to inlcude the name of the host machine in the message body.</param>
    ''' <param name="refException">An exception, if any, raised in sending the message.</param>
    ''' <param name="strBccEmail">A single email address, or a comma-delimited list of email addresses for BCC.</param>
    ''' <remarks>
    ''' Email addresses are not validated.
    ''' Mail server is set in web.config key "MailServer".
    ''' Environment identification inlcuded in subject and body, except in real-world environment.
    ''' </remarks>
    Public Shared Sub SendEmail( _
            ByVal strSubject As String, _
            ByVal strBody As String, _
            ByVal strToEmail As String, _
            ByVal strFromEmail As String, _
            ByVal blnIncludeMachineName As Boolean, _
            ByRef refException As Exception, _
            Optional ByVal strBccEmail As String = "")
        Try
            Dim smtpServer As String = ""
            Dim msgEmail As New System.Net.Mail.MailMessage()
            Dim machineName As String = ""
            Dim strEnvironmentWarning As String = ""

            If blnIncludeMachineName Then
                Try
                    machineName = System.Environment.MachineName
                Catch ex As Exception
                    machineName = "Unable to get machine name"
                End Try
                msgEmail.Body = "Machine Name: " & machineName & Chr(12) & Chr(12)
            End If

            Select Case HttpContext.Current.Application("ApplicationEnvironment")
                Case "SWODEV"
                    strEnvironmentWarning = "***TEST message from DEVELOPMENT***"
                Case "SWOX"
                    strEnvironmentWarning = "***TEST message from EXERCISE***"
                Case "SWOStaging"
                    strEnvironmentWarning = "***TEST message from STAGING***"
                Case "SWOTEST"
                    strEnvironmentWarning = "***TEST message from STAGING***"
                Case "SWO"
                    strEnvironmentWarning = "" 'No environment warning

            End Select

            smtpServer = ConfigurationManager.AppSettings("MailServer").ToString
            'smtpServer = "204.110.208.12" '*******************COMMENT THIS!**********************
            'msgEmail.To.Add("Brandon.Porter@em.myflorida.com")
            msgEmail.To.Add(strToEmail)
            msgEmail.From = New System.Net.Mail.MailAddress(strFromEmail)
            msgEmail.Subject = strSubject & " " & strEnvironmentWarning
            msgEmail.IsBodyHtml = True
            msgEmail.Body += If(strEnvironmentWarning = String.Empty, strEnvironmentWarning, strEnvironmentWarning & Chr(12) & Chr(12)) & strBody
            Dim objEmail As New System.Net.Mail.SmtpClient(smtpServer)
            objEmail.Send(msgEmail)
        Catch ex As Exception
            refException = ex
        End Try
    End Sub


End Class
