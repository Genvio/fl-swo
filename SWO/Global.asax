<%@ Application Language="VB" %>

<script runat="server">
    
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
        Dim strApplicationEnvironment As String = AppDomain.CurrentDomain.BaseDirectory.ToString
        
        If strApplicationEnvironment.ToUpper.Contains("APPDEV") Then
            Application("ApplicationEnvironment") = "SWODEV"
            Application("ApplicationEnvironmentForUpload") = "SWOX"
        ElseIf strApplicationEnvironment.ToUpper.Contains("SWOX") Then
            Application("ApplicationEnvironment") = "SWOX"
            Application("ApplicationEnvironmentForUpload") = "SWOX"
        Else
            If System.Environment.MachineName = "TDMZDEVWS1" Then
                Application("ApplicationEnvironment") = "SWOSTAGING"
                Application("ApplicationEnvironmentForUpload") = "SWO"
            Else
                Application("ApplicationEnvironment") = "SWO"
                Application("ApplicationEnvironmentForUpload") = "SWO"
            End If
        End If
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
        Try
            Dim ourEx As Exception = Server.GetLastError()
            
            If ourEx.Message = "Object reference not set to an instance of an object." Or (Not ourEx.InnerException Is Nothing AndAlso ourEx.InnerException.Message = "Object reference not set to an instance of an object.") Then
                If HttpContext.Current.Session("Security_Tracker") = Nothing Then
                    'No error occurred--the session just timed out
                    Dim uv As New UserValidation
                    uv.Logout()
                    Exit Sub
                End If
            End If
            
            'Dim oCookie As System.Web.HttpCookie
            Dim ns As New SecurityValidate
            Dim objException As New Exception
            Dim strSubject As String = ""
            Dim strBody As String = ""
            
            'msgEmail.To.Add("Brandon.Porter@em.myflorida.com, richard.butgereit@em.myflorida.com") ' "Brian.Misner@em.myflorida.com" "richard.butgereit@em.myflorida.com"
            
            If Not ourEx Is Nothing Then
                strSubject = "ERROR: " & Application("ApplicationEnvironment").ToString & " " & ourEx.Source
                strBody = "Source: " & ourEx.Source & Chr(12) & Chr(12) & ourEx.Message
                
                If Not IsNothing(ourEx.InnerException) Then
                    strBody = strBody & Chr(12) & Chr(12) & "Inner Exception: " & ourEx.InnerException.Message
                End If
                
                strBody = strBody & Chr(12) & Chr(12) & "StackTrace: " & ourEx.StackTrace & Chr(12) & Chr(12) & "TargetSite: " & ourEx.TargetSite.ToString
            Else
                strSubject = "ERROR: " & Application("ApplicationEnvironment").ToString & "(Source Unknown)"
                strBody = "Source: unknown" & vbCrLf & vbCrLf & "Error: unknown"
            End If

            Try
                'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
                ns = Session("Security_Tracker")
                strBody = strBody & Chr(12) & "User: " & ns.FullName
                strBody = strBody & Chr(12) & "User Level: " & ns.UserLevel
                strBody = strBody & Chr(12) & "Agency: " & ns.Agency
                strBody = strBody & Chr(12) & "Email: " & ns.Email
            Catch ex As Exception
                'oCookie = Request.Cookies("shell")
                'strBody = strBody & Chr(12) & "Cookie: Shell"
                strBody = strBody & Chr(12) & "User: not logged in"
                strBody = strBody & Chr(12) & "User Level: N/A"
                strBody = strBody & Chr(12) & "Agency: unknown"
                strBody = strBody & Chr(12) & "Email: unknown"
            End Try

            Email.SendEmail(strSubject, strBody, ConfigurationManager.AppSettings("AdminInfoMessageToEmail"), "EOC-TechnicalServices@em.myflorida.com", True, objException)
        Catch ex As Exception
            'Swallow the error--this routine catches unhandled errors as a last resort. Nothing left to do but shrug it off.
        End Try
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub
       
</script>