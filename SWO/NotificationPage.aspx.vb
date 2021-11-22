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
Imports System.Windows.Forms
Imports System.Messaging

Partial Class NotificationPage
    Inherits System.Web.UI.Page

    'Help functions from our App_Code.
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

    Public objDataGridFunctions As New DataGridFunctions

    'For connecting to the database.
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

    Dim globalResults As String = ""
    Dim _blnNotificationsExist As Boolean = False

    'Dim oCookie As System.Web.HttpCookie
    Dim ns As New SecurityValidate
    Const js As String = "TADDScript.js"

    'Page Load.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        'Response.End()

        If Request("IncidentID") = "" Then
            Response.Redirect("Incident.aspx")
        End If

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)
        objCmd = New SqlCommand("[spSelectOutgoingNotificationCommentByIncidentID]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()

        globalResults = globalResults + "<table align='center' width='100%'cellspacing='0' border='1' style='border-color:#000000'> "

        If objDR.Read() Then
            'There are records.
            objDR.Close()
            objDR = objCmd.ExecuteReader()
            _blnNotificationsExist = True

            While objDR.Read
                globalResults = globalResults + "   <tr>"
                globalResults = globalResults + "       <td width='40%' align='center' style='border-color:#000000; border-style:solid'>"
                globalResults = globalResults + "       " & objDR.Item("Notification")
                globalResults = globalResults + "       </td>"
                globalResults = globalResults + "       <td width='40%' align='center' style='border-color:#000000; border-style:solid'>"
                globalResults = globalResults + "       " & objDR.Item("Comment")
                globalResults = globalResults + "       </td>"
                globalResults = globalResults + "       <td width='10%' align='center' style='border-color:#000000; border-style:solid'>"
                globalResults = globalResults + "       " & objDR.Item("Date")
                globalResults = globalResults + "       </td>"
                globalResults = globalResults + "       <td width='10%' align='center' style='border-color:#000000; border-style:solid'>"
                globalResults = globalResults + "       " & MrDataGrabber.GrabUserFullNameByUserID(objDR.Item("UserID"))
                globalResults = globalResults + "       </td>"
                globalResults = globalResults + "   </tr>"
            End While
        Else
            globalResults = globalResults + "   <tr>"
            globalResults = globalResults + "       <td align='center' style='border-color:#000000; border-style:solid'>"
            globalResults = globalResults + "  No Records at This Time     "
            globalResults = globalResults + "       </td>"
            globalResults = globalResults + "   </tr>"

            _blnNotificationsExist = False
        End If

        globalResults = globalResults + "</table>"

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()

        lblResults.Text = globalResults

        'Moved this If block below globalResults handling so that PopulatePage() is called after _blnNotificationsExist is set
        If Page.IsPostBack = False Then
            'Set message.
            globalMessage = Request("Message")
            globalAction = Request("Action")
            globalParameter = Request("Parameter")

            PopulatePage()
        End If

    End Sub

    Protected Sub PopulatePage()
        GetSubject()
        GetIncidentNumber()

        Dim localEmailList As String = ""
        Dim localNamesList As String = ""
        Dim localAssociatedTask As String = ""

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
            'Must minus 1 to account for the array declaration.
            Dim arrIncidentType(localRecordCountForArray - 1) As Integer
            Dim arrIncidentIncidentType(localRecordCountForArray - 1) As Integer

            'Store each IncidentTypeID in the array. Checking to see if there are any worksheets.
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            'Open the connection.
            DBConStringHelper.PrepareConnection(objConn)
            objCmd = New SqlCommand("[spSelectIncidentIncidentTypeByIncidentID]", objConn)
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then
                'There are records.
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
                'Now we have to grab the IncidentTypeLevelID from each IncidentType.
                Dim localIncidentTypeTable As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("TableName", "IncidentType", "IncidentTypeID", arrIncidentType(localIncidentIncidentTypeLoopCount))
                Dim localIncidentIncidentTypeID As Integer = arrIncidentIncidentType(localIncidentIncidentTypeLoopCount)
                Dim localIncidentTypeID As Integer = arrIncidentType(localIncidentIncidentTypeLoopCount)
                Dim localIncidentID As Integer = Request("IncidentID")

                'Now we can grab IncidentTypeLevelID from the particular IncidentType table because we have the IncidentIncidentTypeID and IncidentID.
                Dim localIncidentTypeLevelID As Integer = MrDataGrabber.GrabIntegerRecordBy2Keys(localIncidentTypeTable, "IncidentTypeLevelID", "IncidentIncidentTypeID", localIncidentIncidentTypeID, "IncidentID", localIncidentID)

                If localIncidentTypeLevelID <> 0 Then
                    'This means there is an actual Level.
                    'Dim localIncidentTypeLevel2 As Integer = MrDataGrabber.GrabIntegerRecordBy2Keys("IncidentTypeLevel", "IncidentTypeLevelID", "Number", localIncidentTypeLevelID, "IncidentTypeID", localIncidentTypeID)

                    'Now we have to get the group.
                    Dim localNotificationGroupRecordCountForArray As Integer = 0

                    localNotificationGroupRecordCountForArray = MrDataGrabber.GrabRecordCountBy2Keys("NotificationGroup", "IncidentTypeID", localIncidentTypeID, "IncidentTypeLevelID", localIncidentTypeLevelID)

                    'Response.Write("localIncidentTypeLevelID: " & localIncidentTypeLevelID)
                    'Response.Write("<br>")
                    'Response.Write("-----------------------------------------")
                    'Response.Write("<br>")
                    'Response.Write("localNotificationGroupRecordCountForArray: " & localNotificationGroupRecordCountForArray)
                    'Response.End()

                    Dim localNotificationGroupLoopCount As Integer = 0

                    'Response.Write("localIncidentTypeLevel2: " & localIncidentTypeLevel2)
                    'Response.Write("<br>")
                    'Response.Write("localNotificationGroupRecordCountForArray: " & localNotificationGroupRecordCountForArray)
                    'Response.Write("<br>")
                    'Response.Write("-----------------------------------------")
                    'Response.Write("<br>")
                    'Response.End()

                    If localNotificationGroupRecordCountForArray <> 0 Then
                        'Must minus 1 to account for the array declaration.
                        'Dim arrNotificationGroup(localNotificationGroupRecordCountForArray - 1) As Integer

                        'Store each IncidentTypeID in the array. Checking to see if there are any worksheets.
                        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                        'Open the connection.
                        DBConStringHelper.PrepareConnection(objConn)
                        objCmd = New SqlCommand("[spSelectNotificationGroupByIncidentTypeIDAndIncidentTypeLevelID]", objConn)
                        objCmd.Parameters.AddWithValue("@IncidentTypeID", localIncidentTypeID)
                        objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", localIncidentTypeLevelID)
                        objCmd.CommandType = CommandType.StoredProcedure
                        objDR = objCmd.ExecuteReader()

                        If objDR.Read() Then
                            'There are records.
                            objDR.Close()
                            objDR = objCmd.ExecuteReader()

                            While objDR.Read
                                Dim localNotificationGroupID As Integer = objDR.Item("NotificationGroupID")

                                'arrNotificationGroup(localNotificationGroupLoopCount) = objDR.Item("NotificationGroupID")

                                'Response.Write("NotificationGroupID: " & arrNotificationGroup(localNotificationGroupLoopCount))
                                'Response.Write("<br>")
                                'Response.Write("-----------------------------------------")
                                'Response.Write("<br>")

                                'Now we grab NotificationGroupNotificationPerson.
                                Dim localNotificationGroupNotificationPersonRecordCountForArray As Integer = 0

                                localNotificationGroupNotificationPersonRecordCountForArray = MrDataGrabber.GrabRecordCountByKey("NotificationGroupNotificationPerson", "NotificationGroupID", localNotificationGroupID)

                                'Dim localNotificationGroupNotificationPersonLoopCount As Integer = 0

                                'Response.Write("localNotificationGroupNotificationPersonRecordCountForArray: " & localNotificationGroupNotificationPersonRecordCountForArray)
                                'Response.Write("<br>")
                                'Response.Write("-----------------------------------------")
                                'Response.Write("<br>")

                                If localNotificationGroupNotificationPersonRecordCountForArray <> 0 Then
                                    'Must minus 1 to account for the array declaration.
                                    'Dim arrNotificationGroupNotificationPerson(localNotificationGroupNotificationPersonRecordCountForArray - 1) As Integer

                                    'Store each IncidentTypeID in the array. Checking to see if there are any worksheets.
                                    objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                                    'Open the connection.
                                    DBConStringHelper.PrepareConnection(objConn2)
                                    objCmd2 = New SqlCommand("[spSelectNotificationGroupNotificationPersonByNotificationGroupID]", objConn2)
                                    objCmd2.Parameters.AddWithValue("@NotificationGroupID", localNotificationGroupID)

                                    objCmd2.CommandType = CommandType.StoredProcedure
                                    objDR2 = objCmd2.ExecuteReader()


                                    If objDR2.HasRows Then
                                        'There are records.

                                        While objDR2.Read
                                            'arrNotificationGroupNotificationPerson(localNotificationGroupNotificationPersonLoopCount) = objDR2.Item("NotificationPositionID")
                                            localNamesList = localNamesList & objDR2.Item("Position") & ", "
                                            localEmailList = localEmailList & objDR2.Item("Email") & "; "
                                            'localNotificationGroupNotificationPersonLoopCount = localNotificationGroupNotificationPersonLoopCount + 1
                                        End While

                                        'While objDR2.Read
                                        '    arrNotificationGroupNotificationPerson(localNotificationGroupNotificationPersonLoopCount) = objDR2.Item("NotificationPositionID")

                                        '    Dim localNotificationPositionID As Integer = objDR2.Item("NotificationPositionID")

                                        '    localNamesList = localNamesList & MrDataGrabber.GrabStringByKey("NotificationPosition", "Position", "NotificationPositionID", objDR2.Item("NotificationPositionID")) & ", "

                                        '    If CStr(MrDataGrabber.GrabStringByKey("NotificationPosition", "Email", "NotificationPositionID", localNotificationPositionID)) <> "" Then
                                        '        localEmailList = localEmailList + CStr(MrDataGrabber.GrabStringByKey("NotificationPosition", "Email", "NotificationPositionID", localNotificationPositionID)) + "; "
                                        '    End If

                                        '    localNotificationGroupNotificationPersonLoopCount = localNotificationGroupNotificationPersonLoopCount + 1
                                        'End While




                                        'Search for and remove duplicates.
                                        '-------------------------------------
                                        'Dim contacts As String() = localEmailList.Split(New Char() {";"c})
                                        'Dim emailList As New ArrayList

                                        'For Each contact As String In contacts
                                        '    If emailList.Contains(contact.Trim) = False Then emailList.Add(contact.Trim)
                                        'Next

                                        'localEmailList = ""

                                        'For Each contact As String In emailList
                                        '    If contact <> "" Then localEmailList = localEmailList + contact & "; "
                                        'Next
                                        '-------------------------------------

                                        'localNotificationGroupNotificationPersonLoopCount = 0
                                    Else

                                    End If

                                    objCmd2.Dispose()
                                    objCmd2 = Nothing
                                    objConn2.Close()
                                End If

                                'localNotificationGroupLoopCount = localNotificationGroupLoopCount + 1
                            End While

                            localNotificationGroupLoopCount = 0
                        Else

                        End If

                        objCmd.Dispose()
                        objCmd = Nothing
                        objConn.Close()
                    End If

                    'The associated tasks 2.
                    localNotificationGroupRecordCountForArray = 0
                    localNotificationGroupRecordCountForArray = MrDataGrabber.GrabRecordCountBy2Keys("NotificationGroup", "IncidentTypeID", localIncidentTypeID, "IncidentTypeLevelID", localIncidentTypeLevelID)

                    'Response.Write("localIncidentTypeLevelID: " & localIncidentTypeLevelID)
                    'Response.Write("<br>")
                    'Response.Write("-----------------------------------------")
                    'Response.Write("<br>")
                    'Response.Write("localNotificationGroupRecordCountForArray: " & localNotificationGroupRecordCountForArray)

                    'Response.End()

                    localNotificationGroupLoopCount = 0

                    'Response.Write("localIncidentTypeLevel2: " & localIncidentTypeLevel2)
                    'Response.Write("<br>")
                    'Response.Write("localNotificationGroupRecordCountForArray: " & localNotificationGroupRecordCountForArray)
                    'Response.Write("<br>")
                    'Response.Write("-----------------------------------------")
                    'Response.Write("<br>")
                    'Response.End()

                    If localNotificationGroupRecordCountForArray <> 0 Then
                        'Must minus 1 to account for the array declaration.
                        'Dim arrNotificationGroup(localNotificationGroupRecordCountForArray - 1) As Integer

                        'Store each IncidentTypeID in array. Checking to see if there are any worksheets.
                        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                        'Open the connection.
                        DBConStringHelper.PrepareConnection(objConn)
                        objCmd = New SqlCommand("[spSelectNotificationGroupByIncidentTypeIDAndIncidentTypeLevelID]", objConn)
                        objCmd.Parameters.AddWithValue("@IncidentTypeID", localIncidentTypeID)
                        objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", localIncidentTypeLevelID)
                        objCmd.CommandType = CommandType.StoredProcedure
                        objDR = objCmd.ExecuteReader()

                        If objDR.Read() Then
                            'There are records.
                            objDR.Close()
                            objDR = objCmd.ExecuteReader()

                            While objDR.Read
                                Dim localNotificationGroupID As Integer = objDR.Item("NotificationGroupID")

                                'arrNotificationGroup(localNotificationGroupLoopCount) = objDR.Item("NotificationGroupID")

                                'Response.Write("NotificationGroupID: " & arrNotificationGroup(localNotificationGroupLoopCount))
                                'Response.Write("<br>")
                                'Response.Write("-----------------------------------------")
                                'Response.Write("<br>")

                                'Now we grab NotificationGroupNotificationPerson.
                                Dim localNotificationGroupAssociatedTaskRecordCountForArray As Integer = 0

                                localNotificationGroupAssociatedTaskRecordCountForArray = MrDataGrabber.GrabRecordCountByKey("NotificationGroupAssociatedTask", "NotificationGroupID", localNotificationGroupID)

                                Dim localNotificationGroupAssociatedTaskLoopCount As Integer = 0

                                'Response.Write("localNotificationGroupNotificationPersonRecordCountForArray: " & localNotificationGroupNotificationPersonRecordCountForArray)
                                'Response.Write("<br>")
                                'Response.Write("-----------------------------------------")
                                'Response.Write("<br>")

                                If localNotificationGroupAssociatedTaskRecordCountForArray <> 0 Then
                                    'Must minus 1 to account for the array declaration.
                                    Dim arrNotificationGroupAssociatedTask(localNotificationGroupAssociatedTaskRecordCountForArray - 1) As Integer

                                    'Store each IncidentTypeID in array. Checking to see if there are any worksheets.
                                    objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

                                    'Open the connection.
                                    DBConStringHelper.PrepareConnection(objConn2)
                                    objCmd2 = New SqlCommand("[spSelectNotificationGroupAssociatedTaskByNotificationGroupID]", objConn2)
                                    objCmd2.Parameters.AddWithValue("@NotificationGroupID", localNotificationGroupID)

                                    objCmd2.CommandType = CommandType.StoredProcedure
                                    objDR2 = objCmd2.ExecuteReader()

                                    If objDR2.Read() Then
                                        'There are records.
                                        objDR2.Close()
                                        objDR2 = objCmd2.ExecuteReader()

                                        While objDR2.Read
                                            'Dim localNotificationGroupID As Integer = objDR.Item("NotificationGroupID")

                                            arrNotificationGroupAssociatedTask(localNotificationGroupAssociatedTaskLoopCount) = objDR2.Item("AssociatedTaskID")

                                            Dim localAssociatedTaskID As Integer = objDR2.Item("AssociatedTaskID")

                                            'globalNamesList = globalNamesList + CStr(objDR2.Item("NotificationPositionID"))

                                            localAssociatedTask = localAssociatedTask & MrDataGrabber.GrabStringByKey("AssociatedTask", "AssociatedTask", "AssociatedTaskID", objDR2.Item("AssociatedTaskID")) & ", "

                                            'Response.Write("NotificationPositionID: " & objDR2.Item("NotificationPositionID"))
                                            'Response.Write("<br>")
                                            'Response.Write("-----------------------------------------")
                                            'Response.Write("<br>")

                                            'localEmailList = localEmailList + CStr(MrDataGrabber.GrabStringByKey("NotificationPosition", "Email", "NotificationPositionID", localNotificationPositionID)) + "; "

                                            'Response.Write("Position: " & MrDataGrabber.GrabStringByKey("NotificationPosition", "Position", "NotificationPositionID", localNotificationPositionID))
                                            'Response.Write("<br>")
                                            'Response.Write("-----------------------------------------")
                                            'Response.Write("<br>")
                                            'Response.Write("Email: " & MrDataGrabber.GrabStringByKey("NotificationPosition", "Email", "NotificationPositionID", localNotificationPositionID))
                                            'Response.Write("<br>")
                                            'Response.Write("-----------------------------------------")
                                            'Response.Write("<br>")

                                            localNotificationGroupAssociatedTaskLoopCount = localNotificationGroupAssociatedTaskLoopCount + 1
                                        End While

                                        localNotificationGroupAssociatedTaskLoopCount = 0
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
                'Response.Write("-----------------------------------------")
                'Response.Write("<br>")

                localIncidentIncidentTypeLoopCount = localIncidentIncidentTypeLoopCount + 1
            End While
        End If

        '20140724 bp: Add sector positions to notification
        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        DBConStringHelper.PrepareConnection(objConn2)
        objCmd2 = New SqlCommand("[spSelectSectorPositionsByIncidentID]", objConn2)
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

        objCmd2.CommandType = CommandType.StoredProcedure
        objDR2 = objCmd2.ExecuteReader()

        If objDR2.HasRows Then
            While objDR2.Read
                localNamesList = localNamesList & objDR2.Item("Position") & ", "
                localEmailList = localEmailList & objDR2.Item("Email") & "; "
            End While
        End If

        objCmd2.Dispose()
        objCmd2 = Nothing
        objConn2.Close()

        'remove duplicate email addresses
        Dim contacts As String() = localEmailList.Split(New Char() {";"c})
        Dim emailList As New ArrayList

        For Each contact As String In contacts
            If emailList.Contains(contact.Trim) = False Then emailList.Add(contact.Trim)
        Next

        localEmailList = ""

        For Each contact As String In emailList
            If contact <> "" Then localEmailList = localEmailList + contact & "; "
        Next

        'If globalNamesList <> "" Then
        '    txtNameList.Text = globalNamesList
        'End If

        'If globalEmailList <> "" Then
        '   txtNameList.Text = globalNamesList
        'End If

        Dim oCountyRegion As New CountyRegion(Request("IncidentID"))

        Dim strRegionCoordinator As String = ""
        Dim strRegionCoordinatorEmail As String = ""
        Dim strCountyCoordinator As String = ""
        Dim strCountyCoordinatorEmail As String = ""

        strRegionCoordinator = oCountyRegion.gStrRegionCoordinator
        strRegionCoordinatorEmail = oCountyRegion.gStrRegionCoordinatorEmail
        strCountyCoordinator = oCountyRegion.gStrCountyCoordinator
        strCountyCoordinatorEmail = oCountyRegion.gStrCountyCoordinatorEmail

        If strRegionCoordinatorEmail <> "" And strCountyCoordinatorEmail <> "" Then
            strRegionCoordinatorEmail = strRegionCoordinatorEmail & "; " & strCountyCoordinatorEmail
        Else
            strRegionCoordinatorEmail = strRegionCoordinatorEmail & strCountyCoordinatorEmail
        End If

        If strRegionCoordinator <> "" And strCountyCoordinator <> "" Then
            strRegionCoordinator = strRegionCoordinator & ", " & strCountyCoordinator
        Else
            strRegionCoordinator = strRegionCoordinator & strCountyCoordinator
        End If

        'Response.Write(oCountyRegion.gStrRegionCoordinator)

        If strRegionCoordinator <> "" Then
            localNamesList = localNamesList & strRegionCoordinator
        Else
            If localNamesList <> "" Then
                localNamesList = Left(localNamesList, localNamesList.Length - 2)
            End If
        End If

        If strRegionCoordinatorEmail <> "" Then
            localEmailList = localEmailList & strRegionCoordinatorEmail
        Else
            If localEmailList <> "" Then
                localEmailList = Left(localEmailList, localEmailList.Length - 2)
            End If
        End If

        If localAssociatedTask <> "" Then
            localAssociatedTask = Left(localAssociatedTask, localAssociatedTask.Length - 2)
        End If

        txtNameList.Text = localNamesList
        txtEmailList.Text = localEmailList.Replace("  ", " ")
        txtEmailList.Text = localEmailList.Replace(" ;", ";")
        txtEmailList.Text = localEmailList.Replace(";;", ";")
        txtAssociatedTask.Text = localAssociatedTask
    End Sub

    Protected Sub GetSubject()
        Dim oCountyRegion As New CountyRegion(Request("IncidentID"))
        Dim strSubject As String = ""
        Dim strDrill As String = CType(Session.Item("isThisADrill"), String)

        If strDrill = "Yes" Then strSubject = "EXERCISE // "

        Dim intUserID As Integer = MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("LastUpdatedByID", "Incident", "IncidentID", Request("IncidentID"))
        Dim strLastName As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("LastName", "[User]", "UserID", intUserID)
        Dim intAgencyID As Integer = MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("AgencyID", "[User]", "UserID", intUserID)
        Dim strAgencyAbbreviation As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("Abbreviation", "Agency", "AgencyID", intAgencyID)

        'Get the number of updates
        Dim updateCount As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        DBConStringHelper.PrepareConnection(objConn)

        'Establish the query and enter its parameters.
        objCmd = New SqlCommand("[spSelectUpdateReportByIncidentID]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.CommandType = CommandType.StoredProcedure

        'Execute the query.
        objDR = objCmd.ExecuteReader()

        While objDR.Read()
            updateCount += 1
        End While

        'Clear command objects and close the connection.
        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()


        'If there are any notifications, select drop down Update item
        If _blnNotificationsExist AndAlso Page.IsPostBack = False Then 'Add postback check if user should be able to change dropdown manually (confirmed 15 April 2014 by Brian Misner)
            ddlSG.ClearSelection()
            ddlSG.Items.FindByValue("UPDATE").Selected = True
        End If

        strSubject = strSubject & ddlSG.SelectedValue.ToString

        '-------------------------------------------------------------
        If ddlSG.SelectedValue = "UPDATE" Then
            'Include number of updates in the subject line.
            If updateCount = 0 Then
                strSubject = strSubject & " " & ""
            Else
                strSubject = strSubject & " " & updateCount & ""
            End If
        End If
        '-------------------------------------------------------------

        If ddlSG2.SelectedValue.ToString <> "INFO ONLY" Then
            strSubject = strSubject & " - "
            strSubject = strSubject & ddlSG2.SelectedValue.ToString
        End If

        If oCountyRegion.GetStateWideElseRegionElseCountyAlphabetical <> "" Then
            strSubject = strSubject & " / " & oCountyRegion.GetStateWideElseRegionElseCountyAlphabetical() & ""
        End If

        strSubject = strSubject & " / " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentName", "Incident", "IncidentID", Request("IncidentID"))
        strSubject = strSubject & " / " & strAgencyAbbreviation & "-" & strLastName
        strSubject = strSubject & ""

        If strDrill = "Yes" Then strSubject = strSubject & " // EXERCISE"

        lblSubject.Text = strSubject

        Dim oBlackBerryReport As New BlackBerryReport(Request("IncidentID"))

        'Response.Write(oBlackBerryReport.gStrTotalReport)
        'Response.End()

        'Response.Write("Regions: " & oCountyRegion.gStrRegions)
        'Response.Write("<br>")
        'Response.Write("Regions Affected: " & oCountyRegion.gStrRegionsAffected)
        'Response.Write("<br>")
        'Response.Write("Subject: " & strSubject)

        'Response.Write("County: " & oCountyRegion.GetStateWideElseRegionElseCountyAlphabetical())
    End Sub

    Protected Sub GetIncidentNumber()
        Dim localYear As String = ""
        Dim localNumber As Integer

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectIncidentNumberByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            localYear = HelpFunction.Convertdbnulls(objDR("Year"))
            localNumber = HelpFunction.ConvertdbnullsInt(objDR("Number"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        lblIncidentNumber.Text = localYear & "-" & CStr(localNumber)
    End Sub

    'Protected Sub btnReplyComment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReplyComment.Click
    '    oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
    '    'Add cookie.
    '    Response.Cookies.Add(oCookie)

    '    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

    '    'Enter the email and password to query/command object.
    '    objCmd = New SqlCommand("spInsertReplyNotification", objConn)
    '    objCmd.CommandType = CommandType.StoredProcedure
    '    objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
    '    objCmd.Parameters.AddWithValue("@Notification", txtReplyNotification.Text)
    '    objCmd.Parameters.AddWithValue("@Comment", txtReplyComment.Text)
    '    objCmd.Parameters.AddWithValue("@Date", Now)
    '    objCmd.Parameters.AddWithValue("@UserID", oCookie.Item("UserID"))

    '    'Open the connection using the connection string.
    '    DBConStringHelper.PrepareConnection(objConn)

    '    'Execute the command to the DataReader.
    '    objCmd.ExecuteNonQuery()

    '    'Clean up our command objects and close the connection.
    '    objCmd.Dispose()
    '    objCmd = Nothing
    '    DBConStringHelper.FinalizeConnection(objConn)

    '    Response.Redirect("NotificationPage.aspx?IncidentID=" & Request("IncidentID"))
    'End Sub

    'Protected Sub lnkHistoryOutgoingNotificationComment_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkHistoryOutgoingNotificationComment.Load
    '    lnkHistoryOutgoingNotificationComment.NavigateUrl = "OutgoingNotificationComment.aspx?IncidentID=" & Request("IncidentID")
    'End Sub

    'Protected Sub lnkHistoryReplyComment_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkHistoryReplyComment.Load
    '    lnkHistoryReplyComment.NavigateUrl = "ReplyNotification.aspx?IncidentID=" & Request("IncidentID")
    'End Sub

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
            pnlShowSubjectLabel.Visible = True
            pnlShowSystemGenerated.Visible = True
        Else
            pnlShowSystemGenerated.Visible = False
            pnlShowSubjectLabel.Visible = False
        End If
    End Sub

    Protected Sub lnkViewBlackberryReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkViewBlackberryReport.Load
        'Response.Write(ddlSG.SelectedValue.ToString)

        If ddlSG.SelectedValue.ToString = "INITIAL" Then
            lnkViewBlackberryReport.NavigateUrl = "ViewBlackBerryReport.aspx?IncidentID=" & Request("IncidentID") & "&ReportType=INITIAL"
        ElseIf ddlSG.SelectedValue.ToString = "UPDATE" Then
            lnkViewBlackberryReport.NavigateUrl = "ViewBlackBerryReport.aspx?IncidentID=" & Request("IncidentID") & "&ReportType=UPDATE"
        Else

        End If
    End Sub

    Private Sub SendEmail()
        Try
            AddUpdateForEmail()

            Dim mailTo As String = ""
            Dim mailFrom As String = ""
            Dim mailSubject As String = ""
            Dim mailBody As String = ""
            Dim objException As New Exception

            'mailTo = "Brian.Misner@em.myflorida.com"
            'mailTo = "richarddible@gmail.com"
            'mailTo = "SWP@em.myflorida.com"
            mailTo = Replace(txtEmailList.Text, " ", "")
            mailTo = Replace(txtEmailList.Text, ";", ",")
            mailFrom = "SWP@em.myflorida.com"

            If rdoCustom.Checked = True Then
                mailSubject = txtCustomSubject.Text
            End If

            If rdoSystemGenerated.Checked = True Then
                mailSubject = lblSubject.Text
            End If

            If mailTo = "" Then
                'To ensure that we never get a mailto error.
                mailTo = "SWP@em.myflorida.com"
            End If

            'Create and attach the body message.
            Dim oBlackBerryReport As New BlackBerryReport(Request("IncidentID"), "NotificationPage")

            If ddlSG.SelectedValue.ToString = "INITIAL" Then
                mailBody = oBlackBerryReport.gStrTotalReport.ToString
            ElseIf ddlSG.SelectedValue.ToString = "UPDATE" Then
                mailBody = oBlackBerryReport.gStrUpdate & oBlackBerryReport.gStrTotalReport
            ElseIf ddlSG.SelectedValue.ToString = "FINAL UPDATE" Then
                mailBody = oBlackBerryReport.gStrUpdate & oBlackBerryReport.gStrTotalReport
            Else
                mailBody = oBlackBerryReport.gStrTotalReport.ToString
            End If

            Email.SendEmail(mailSubject, mailBody, mailTo, mailFrom, False, objException, mailTo)

            If Not objException.Source Is Nothing Then Throw objException

            lblMessage.Text = "Notification Sent to: " & txtEmailList.Text & " on: " & Now
            lblMessage.ForeColor = Drawing.Color.Green
            lblMessage.Visible = True

            lblMessage2.Text = "Notification Sent to: " & txtEmailList.Text & " on: " & Now
            lblMessage2.ForeColor = Drawing.Color.Green
            lblMessage2.Visible = True
        Catch ex As Exception
            'There was a problem sending it out.
            lblMessage.Text = "Please make sure the emails are in correct format and try again. If problem persists please contact customer support.  The error number is: <br /> 850-413-9907<br />" & ex.Message & ex.ToString()
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Visible = True

            lblMessage2.Text = "Please make sure the emails are in correct format and try again. If problem persists please contact customer support.  The error number is: <br /> 850-413-9907<br />" & ex.Message & ex.ToString()
            lblMessage2.ForeColor = Drawing.Color.Red
            lblMessage2.Visible = True

            Dim strMessageBody As String = ""
            strMessageBody = strMessageBody & Chr(12) & Chr(12) & "Source: " & ex.Source & Chr(12) & Chr(12) & ex.Message
            strMessageBody = strMessageBody & Chr(12) & Chr(12) & "Base Exception: " & ex.GetBaseException.Message
            strMessageBody = strMessageBody & Chr(12) & Chr(12) & "StackTrace: " & ex.StackTrace
            strMessageBody = strMessageBody & Chr(12) & Chr(12) & "TargetSite: " & ex.TargetSite.ToString
            Email.SendAdminInfoEmail(Application("ApplicationEnvironment").ToString & " Unable to Send Notification Email", strMessageBody)
        End Try
    End Sub

    Private Sub AddUpdate()
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Enter the email and password to query/command object.
        objCmd = New SqlCommand("spInsertOutgoingNotificationComment", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@Notification", txtOutgoingNotification.Text)
        objCmd.Parameters.AddWithValue("@Comment", txtOutgoingComment.Text)
        objCmd.Parameters.AddWithValue("@Date", Now)
        objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))

        'Open the connection using the connection string.
        DBConStringHelper.PrepareConnection(objConn)

        'Execute the command to the DataReader.
        objCmd.ExecuteNonQuery()

        'Clean up our command objects and close the connection.
        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)

        Response.Redirect("NotificationPage.aspx?IncidentID=" & Request("IncidentID"))
    End Sub

    Private Sub AddUpdateForEmail()
        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        'Add cookie.
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Enter the email and password to query/command object.
        objCmd = New SqlCommand("spInsertOutgoingNotificationComment", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@Notification", txtEmailList.Text)
        objCmd.Parameters.AddWithValue("@Comment", txtOutgoingComment.Text)
        objCmd.Parameters.AddWithValue("@Date", Now)
        objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))

        'Open the connection using the connection string.
        DBConStringHelper.PrepareConnection(objConn)

        'Execute the command to the DataReader.
        objCmd.ExecuteNonQuery()

        'Clean up our command objects and close the connection.
        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)

        'Response.Redirect("NotificationPage.aspx?IncidentID=" & Request("IncidentID"))
    End Sub

    Protected Sub btnSendNotification_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendNotification.Click
        SendEmail()
    End Sub

    Protected Sub btnOutgoingNotificationComment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOutgoingNotificationComment.Click
        AddUpdate()
    End Sub

    Protected Sub btnReturnToWorksheet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturnToWorksheet.Click
        Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID"))
    End Sub

    Protected Sub ddlSG_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSG.SelectedIndexChanged
        GetSubject()
    End Sub

    Protected Sub ddlSG2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSG2.SelectedIndexChanged
        GetSubject()
    End Sub
End Class