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
Imports System.Web.Services
Imports System.IO

Partial Class NotificationPage2
    Inherits System.Web.UI.Page
    'Help Functions from our App_Code
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    Public objDataGridFunctions As New DataGridFunctions

    'For Connecting to the database
    Public objConn As New System.Data.SqlClient.SqlConnection
    Public objCmd As System.Data.SqlClient.SqlCommand
    Public objDR As System.Data.SqlClient.SqlDataReader
    Public objDA As System.Data.SqlClient.SqlDataAdapter
    Public objDS As New System.Data.DataSet

    Public objConn2 As New System.Data.SqlClient.SqlConnection
    Public objCmd2 As System.Data.SqlClient.SqlCommand
    Public objDR2 As System.Data.SqlClient.SqlDataReader
    Public objDA2 As System.Data.SqlClient.SqlDataAdapter
    Public objDS2 As New System.Data.DataSet

    Dim ParamId As SqlParameter

    Public AuditHelper As New AuditHelp

    Public MrDataGrabber As New DataGrabber

    Dim globalAuditAction As String = ""
    Dim globalHasErrors As Boolean = False
    Dim globalMessage As String
    Dim globalCurrentStep As Integer
    Dim globalIsSaved As Boolean = False
    Dim globalAction As String
    Dim globalParameter As String

    'Dim oCookie As System.Web.HttpCookie
    Dim ns As New SecurityValidate
    Const js As String = "TADDScript.js"

    Dim strBody As New StringBuilder("")

    'Page Load
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        'Response.End()

        If Request("IncidentID") = "" Then

            Response.Redirect("Incident.aspx")

        End If

        If Page.IsPostBack = False Then

            'set message
            globalMessage = Request("Message")
            globalAction = Request("Action")
            globalParameter = Request("Parameter")
            PopulatePage()
        End If

    End Sub

    Protected Sub PopulatePage()

        GetSubject()

        Dim localEmailList As String = ""
        Dim localNamesList As String = ""

        If rdoCustom.Checked = True Then
            pnlShowCustom.Visible = True
        Else
            pnlShowCustom.Visible = False
        End If

        If rdoSystemGenerated.Checked = True Then
            pnlShowSystemGenerated.Visible = True
            pnlShowSubjectLabel.Visible = True
        Else
            pnlShowSystemGenerated.Visible = False
            pnlShowSubjectLabel.Visible = False
        End If

        Dim localRecordCountForArray As Integer = 0

        localRecordCountForArray = MrDataGrabber.GrabRecordCountByKey("IncidentIncidentType", "IncidentID", Request("IncidentID"))

        Dim localIncidentIncidentTypeLoopCount As Integer = 0

        If localRecordCountForArray <> 0 Then

            'Must minus 1 to account for the Array Declaration
            Dim arrIncidentType(localRecordCountForArray - 1) As Integer
            Dim arrIncidentIncidentType(localRecordCountForArray - 1) As Integer
            'Store each IncidentTypeID in Array
            'Checking to see if there are any worksheets 
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectIncidentIncidentTypeByIncidentID]", objConn)
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then
                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    arrIncidentType(localIncidentIncidentTypeLoopCount) = objDR.Item("IncidentTypeID")
                    arrIncidentIncidentType(localIncidentIncidentTypeLoopCount) = objDR.Item("IncidentIncidentTypeID")

                    localIncidentIncidentTypeLoopCount = localIncidentIncidentTypeLoopCount + 1

                End While

                localIncidentIncidentTypeLoopCount = 0

            Else

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()



            While localIncidentIncidentTypeLoopCount < localRecordCountForArray


                'Now we have to grab the IncidentTypeLevelID from each IncidentType 
                Dim localIncidentTypeTable As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("TableName", "IncidentType", "IncidentTypeID", arrIncidentType(localIncidentIncidentTypeLoopCount))

                Dim localIncidentIncidentTypeID As Integer = arrIncidentIncidentType(localIncidentIncidentTypeLoopCount)
                Dim localIncidentTypeID As Integer = arrIncidentType(localIncidentIncidentTypeLoopCount)
                Dim localIncidentID As Integer = Request("IncidentID")

                'Last Now We can Grab IncidentTypeLevelID From The Particular IncidentType Table because we have the IncidentIncidentTypeID and IncidentID
                Dim localIncidentTypeLevelID As Integer = MrDataGrabber.GrabIntegerRecordBy2Keys(localIncidentTypeTable, "IncidentTypeLevelID", "IncidentIncidentTypeID", localIncidentIncidentTypeID, "IncidentID", localIncidentID)




                If localIncidentTypeLevelID <> 0 Then
                    'This means there is an actual Level

                    'Dim localIncidentTypeLevel2 As Integer = MrDataGrabber.GrabIntegerRecordBy2Keys("IncidentTypeLevel", "IncidentTypeLevelID", "Number", localIncidentTypeLevelID, "IncidentTypeID", localIncidentTypeID)
                    'Now we have to get the Group


                    Dim localNotificationGroupRecordCountForArray As Integer = 0

                    localNotificationGroupRecordCountForArray = MrDataGrabber.GrabRecordCountBy2Keys("NotificationGroup", "IncidentTypeID", localIncidentTypeID, "IncidentTypeLevelID", localIncidentTypeLevelID)
                    'Response.Write("localIncidentTypeLevelID: " & localIncidentTypeLevelID)
                    'Response.Write("<br>")
                    'Response.Write("=========================================")
                    'Response.Write("<br>")
                    'Response.Write("localNotificationGroupRecordCountForArray: " & localNotificationGroupRecordCountForArray)

                    'Response.End()
                    Dim localNotificationGroupLoopCount As Integer = 0

                    'MrDataGrabber.
                    'Response.Write("localIncidentTypeLevel2: " & localIncidentTypeLevel2)
                    'Response.Write("<br>")
                    'Response.Write("localNotificationGroupRecordCountForArray: " & localNotificationGroupRecordCountForArray)
                    'Response.Write("<br>")
                    'Response.Write("=========================================")
                    'Response.Write("<br>")
                    ''Response.End()

                    If localNotificationGroupRecordCountForArray <> 0 Then


                        'Must minus 1 to account for the Array Declaration
                        Dim arrNotificationGroup(localNotificationGroupRecordCountForArray - 1) As Integer
                        'Store each IncidentTypeID in Array
                        'Checking to see if there are any worksheets 
                        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                        DBConStringHelper.PrepareConnection(objConn) 'open the connection
                        objCmd = New SqlCommand("[spSelectNotificationGroupByIncidentTypeIDAndIncidentTypeLevelID]", objConn)
                        objCmd.Parameters.AddWithValue("@IncidentTypeID", localIncidentTypeID)
                        objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", localIncidentTypeLevelID)
                        objCmd.CommandType = CommandType.StoredProcedure
                        objDR = objCmd.ExecuteReader()

                        If objDR.Read() Then
                            'there are records
                            objDR.Close()
                            objDR = objCmd.ExecuteReader()

                            While objDR.Read


                                Dim localNotificationGroupID As Integer = objDR.Item("NotificationGroupID")

                                arrNotificationGroup(localNotificationGroupLoopCount) = objDR.Item("NotificationGroupID")

                                'Response.Write("NotificationGroupID: " & arrNotificationGroup(localNotificationGroupLoopCount))
                                'Response.Write("<br>")
                                'Response.Write("=========================================")
                                'Response.Write("<br>")



                                'Now We Grab NotificationGroupNotificationPerson
                                Dim localNotificationGroupNotificationPersonRecordCountForArray As Integer = 0

                                localNotificationGroupNotificationPersonRecordCountForArray = MrDataGrabber.GrabRecordCountByKey("NotificationGroupNotificationPerson", "NotificationGroupID", localNotificationGroupID)

                                Dim localNotificationGroupNotificationPersonLoopCount As Integer = 0

                                'Response.Write("localNotificationGroupNotificationPersonRecordCountForArray: " & localNotificationGroupNotificationPersonRecordCountForArray)
                                'Response.Write("<br>")
                                'Response.Write("=========================================")
                                'Response.Write("<br>")

                                If localNotificationGroupNotificationPersonRecordCountForArray <> 0 Then

                                    'Must minus 1 to account for the Array Declaration
                                    Dim arrNotificationGroupNotificationPerson(localNotificationGroupNotificationPersonRecordCountForArray - 1) As Integer

                                    'Store each IncidentTypeID in Array
                                    'Checking to see if there are any worksheets 
                                    objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                                    DBConStringHelper.PrepareConnection(objConn2) 'open the connection
                                    objCmd2 = New SqlCommand("[spSelectNotificationGroupNotificationPersonByNotificationGroupID]", objConn2)
                                    objCmd2.Parameters.AddWithValue("@NotificationGroupID", localNotificationGroupID)

                                    objCmd2.CommandType = CommandType.StoredProcedure
                                    objDR2 = objCmd2.ExecuteReader()


                                    If objDR2.Read() Then
                                        'there are records
                                        objDR2.Close()
                                        objDR2 = objCmd2.ExecuteReader()

                                        While objDR2.Read


                                            'Dim localNotificationGroupID As Integer = objDR.Item("NotificationGroupID")

                                            arrNotificationGroupNotificationPerson(localNotificationGroupNotificationPersonLoopCount) = objDR2.Item("NotificationPositionID")

                                            Dim localNotificationPositionID As Integer = objDR2.Item("NotificationPositionID")

                                            'globalNamesList = globalNamesList + CStr(objDR2.Item("NotificationPositionID"))

                                            localNamesList = localNamesList & MrDataGrabber.GrabStringByKey("NotificationPosition", "Position", "NotificationPositionID", objDR2.Item("NotificationPositionID")) & ", "



                                            'Response.Write("NotificationPositionID: " & objDR2.Item("NotificationPositionID"))
                                            'Response.Write("<br>")
                                            'Response.Write("=========================================")
                                            'Response.Write("<br>")

                                            localEmailList = localEmailList + CStr(MrDataGrabber.GrabStringByKey("NotificationPosition", "Email", "NotificationPositionID", localNotificationPositionID)) + "; "

                                            'Response.Write("Position: " & MrDataGrabber.GrabStringByKey("NotificationPosition", "Position", "NotificationPositionID", localNotificationPositionID))
                                            'Response.Write("<br>")
                                            'Response.Write("=========================================")
                                            'Response.Write("<br>")
                                            'Response.Write("Email: " & MrDataGrabber.GrabStringByKey("NotificationPosition", "Email", "NotificationPositionID", localNotificationPositionID))
                                            'Response.Write("<br>")
                                            'Response.Write("=========================================")
                                            'Response.Write("<br>")
                                            localNotificationGroupNotificationPersonLoopCount = localNotificationGroupNotificationPersonLoopCount + 1



                                        End While


                                        localNotificationGroupNotificationPersonLoopCount = 0

                                    Else

                                    End If

                                    objCmd2.Dispose()
                                    objCmd2 = Nothing
                                    objConn2.Close()

                                End If



                                localNotificationGroupLoopCount = localNotificationGroupLoopCount + 1



                            End While

                            localNotificationGroupLoopCount = 0

                        Else

                        End If

                        objCmd.Dispose()
                        objCmd = Nothing
                        objConn.Close()


                    End If



                End If

                'Response.Write(localIncidentTypeTable)
                'Response.Write("<br>")
                'Response.Write("IncidentIncidentType:" & arrIncidentIncidentType(localIncidentIncidentTypeLoopCount))
                'Response.Write("<br>")
                'Response.Write("IncidentType:" & arrIncidentType(localIncidentIncidentTypeLoopCount))
                'Response.Write("<br>")
                'Response.Write("localIncidentTypeLevelID:" & localIncidentTypeLevelID)
                'Response.Write("<br>")
                'Response.Write("==========================================================================")
                'Response.Write("<br>")

                localIncidentIncidentTypeLoopCount = localIncidentIncidentTypeLoopCount + 1



            End While





        End If

        'If globalNamesList <> "" Then
        '    txtNameList.Text = globalNamesList
        'End If

        'If globalEmailList <> "" Then
        '   txtNameList.Text = globalNamesList
        'End If

        txtNameList.Text = localNamesList
        txtEmailList.Text = localEmailList


    End Sub

    Protected Sub GetSubject()

        Dim oCountyRegion As New CountyRegion(Request("IncidentID"))

        Dim strSubject As String = ""

        Dim intUserID As Integer = MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("CreatedByID", "Incident", "IncidentID", Request("IncidentID"))

        Dim strLastName As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("LastName", "[User]", "UserID", intUserID)

        Dim intAgencyID As Integer = MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("AgencyID", "[User]", "UserID", intUserID)

        Dim strAgencyAbbreviation As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("Abbreviation", "Agency", "AgencyID", intAgencyID)

        strSubject = ddlSG.SelectedValue.ToString

        strSubject = strSubject & " - "

        strSubject = strSubject & ddlSG2.SelectedValue.ToString

        If oCountyRegion.GetStateWideElseRegionElseCountyAlphabetical <> "" Then
            strSubject = strSubject & " / " & oCountyRegion.GetStateWideElseRegionElseCountyAlphabetical() & ""
        End If

        strSubject = strSubject & " / " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentName", "Incident", "IncidentID", Request("IncidentID"))

        strSubject = strSubject & " / " & strAgencyAbbreviation & "-" & strLastName

        strSubject = strSubject & ""

        lblSubject.Text = strSubject

        Dim oBlackBerryReport As New BlackBerryReport(Request("IncidentID"))

        'Response.Write(oBlackBerryReport.gStrTotalReport)
        'Response.End()

        'Response.Write("Regions: " & oCountyRegion.gStrRegions)
        'Response.Write("<br>")
        'Response.Write("Regions Affected: " & oCountyRegion.gStrRegionsAffected)
        'Response.Write("<br>")
        'Response.Write("Subject: " & strSubject)

        ''Good One
        'Response.Write("County: " & oCountyRegion.GetStateWideElseRegionElseCountyAlphabetical())

    End Sub


    Protected Sub btnOutgoingNotificationComment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOutgoingNotificationComment.Click

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        '// Enter the email and password to query/command object.
        objCmd = New SqlCommand("spInsertOutgoingNotificationComment", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@Notification", txtOutgoingNotification.Text)
        objCmd.Parameters.AddWithValue("@Comment", txtOutgoingComment.Text)
        objCmd.Parameters.AddWithValue("@Date", Now)
        objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))

        '// Open the connection using the connection string.
        DBConStringHelper.PrepareConnection(objConn)

        '// Execute the command to the DataReader.
        objCmd.ExecuteNonQuery()
        '// Clean up our command objects and close the connection.
        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)

        Response.Redirect("NotificationPage.aspx?IncidentID=" & Request("IncidentID"))

    End Sub

    Protected Sub btnReplyComment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReplyComment.Click

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        '// Enter the email and password to query/command object.
        objCmd = New SqlCommand("spInsertReplyNotification", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@Notification", txtReplyNotification.Text)
        objCmd.Parameters.AddWithValue("@Comment", txtReplyComment.Text)
        objCmd.Parameters.AddWithValue("@Date", Now)
        objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))

        '// Open the connection using the connection string.
        DBConStringHelper.PrepareConnection(objConn)

        '// Execute the command to the DataReader.
        objCmd.ExecuteNonQuery()
        '// Clean up our command objects and close the connection.
        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)

        Response.Redirect("NotificationPage.aspx?IncidentID=" & Request("IncidentID"))

    End Sub

    Protected Sub lnkHistoryOutgoingNotificationComment_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkHistoryOutgoingNotificationComment.Load
        lnkHistoryOutgoingNotificationComment.NavigateUrl = "OutgoingNotificationComment.aspx?IncidentID=" & Request("IncidentID")
    End Sub

    Protected Sub lnkHistoryReplyComment_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkHistoryReplyComment.Load
        lnkHistoryReplyComment.NavigateUrl = "ReplyNotification.aspx?IncidentID=" & Request("IncidentID")
    End Sub



    Protected Sub rdoCustom_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoCustom.CheckedChanged

        If rdoCustom.Checked = True Then
            pnlShowCustom.Visible = True
        Else
            pnlShowCustom.Visible = False
        End If

        If rdoSystemGenerated.Checked = True Then
            pnlShowSystemGenerated.Visible = True
            pnlShowSubjectLabel.Visible = True
        Else
            pnlShowSystemGenerated.Visible = False
            pnlShowSubjectLabel.Visible = False
        End If

    End Sub

    Protected Sub rdoSystemGenerated_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoSystemGenerated.CheckedChanged

        If rdoCustom.Checked = True Then
            pnlShowCustom.Visible = True
        Else
            pnlShowCustom.Visible = False
        End If

        If rdoSystemGenerated.Checked = True Then
            pnlShowSystemGenerated.Visible = True
        Else
            pnlShowSystemGenerated.Visible = False
        End If

    End Sub

    Protected Sub lnkViewBlackberryReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkViewBlackberryReport.Load

        lnkViewBlackberryReport.NavigateUrl = "ViewBlackBerryReport.aspx?IncidentID=" & Request("IncidentID")

    End Sub

    Private Sub SendEmail()
        Try
            Dim mailTo As String
            Dim mailFrom As String
            Dim mailSubject As String
            Dim mailBody As String
            Dim objException As New Exception

            mailTo = Replace(txtEmailList.Text, " ", "")
            mailTo = Replace(txtEmailList.Text, ";", ",")
            mailFrom = "SWP@em.myflorida.com"
            mailSubject = ""

            If rdoCustom.Checked = True Then
                mailSubject = txtCustomSubject.Text
            End If

            If rdoSystemGenerated.Checked = True Then
                mailSubject = lblSubject.Text
            End If

            If mailTo = "" Then
                'To ensure that we never get a mailto error
                mailTo = "SWP@em.myflorida.com"
            End If

            mailTo = "richarddible@gmail.com,richie.dible@em.myflorida.com"
            mailBody = "Industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,"
            mailBody += Environment.NewLine
            mailBody += "Industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,"
            'Dim oBlackBerryReport As New BlackBerryReport(Request("IncidentID"))
            Email.SendEmail(mailSubject, mailBody, mailTo, mailFrom, False, objException)
            If Not objException.Source Is Nothing Then Throw objException

            lblMessage.Text = "Notification Sent to: " & txtEmailList.Text & " on: " & Now
            lblMessage.ForeColor = Drawing.Color.Green
            lblMessage.Visible = True

            lblMessage2.Text = "Notification Sent to: " & txtEmailList.Text & " on: " & Now
            lblMessage2.ForeColor = Drawing.Color.Green
            lblMessage2.Visible = True
        Catch ex As Exception
            '// There was a problem sending it out.
            lblMessage.Text = "Please make sure the emails are in correct format and try again. If problem persists lease contact customer support.  The error number is: <br /> 850-413-9907<br />" & Err.Description
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Visible = True

            lblMessage2.Text = "Please make sure the emails are in correct format and try again. If problem persists lease contact customer support.  The error number is: <br /> 850-413-9907<br />" & Err.Description
            lblMessage2.ForeColor = Drawing.Color.Red
            lblMessage2.Visible = True
        End Try

    End Sub

    Protected Sub btnSendNotification_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendNotification.Click
        SendEmail()
    End Sub

    Protected Sub btnReturnToWorksheet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturnToWorksheet.Click
        Response.Write("<script language='javascript'> { window.open('','_self');window.close();}</script>")
    End Sub

End Class