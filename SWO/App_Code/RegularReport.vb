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
Imports Microsoft.Office.Interop.Word
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports CountyRegion

Public Class RegularReport
    'Help Functions from our App_Code.
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

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
    Public objDS3 As New System.Data.DataSet
    Public objDS4 As New System.Data.DataSet
    Public objDS5 As New System.Data.DataSet

    Dim ParamId As SqlParameter

    Public MrDataGrabber As New DataGrabber

    'Global Object Variables.
    Dim oCountyRegions As CountyRegion

    'IncidentID.
    Public gStrIncidentID As String = ""
    Public gStrIsThisADrill As String = ""

    Dim strBody As New StringBuilder("")

    Public gStrTotalReport As String = ""
    Public gStrReportFormat As String = ""
    Public gStrUpdate As String = ""
    Public gStrNotifications As String = ""
    Private gstrInstantiator As String = String.Empty

    Dim strUpdate As New StringBuilder("")
    Dim strNotifications As New StringBuilder("")

    'Constructor Expects IncidentID.
    Public Sub New(ByVal strIncidentID As String, ByVal strReportFormat As String, Optional ByVal strInstantiator As String = "Unknown")
        gStrIncidentID = strIncidentID
        gStrReportFormat = strReportFormat
        oCountyRegions = New CountyRegion(gStrIncidentID)
        gstrInstantiator = strInstantiator

        strBody.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
        strBody.Append("<html xmlns='http://www.w3.org/1999/xhtml'>")
        strBody.Append("<head>")
        strBody.Append("<title>" & HttpContext.Current.Application("ApplicationEnvironment") & " Incident Tracker</title>")
        strBody.Append("<style>body{font-family: Times;} pre{font-family: Times; white-space:pre-wrap;}</style>")
        strBody.Append("</head>")
        strBody.Append("<body>")

        GetMainForm()

        GetWorkSheets()

        If gStrIsThisADrill = "Yes" Then
            If gStrReportFormat = "HTML" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='center' style='background-color:#ff0000; color:#000000;' >")
                strBody.Append("            <b>THIS IS AN EXERCISE</b>")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            Else
                strBody.Append("<table width='100%'cellspacing='0' border='0'>")
                strBody.Append("    <tr>")
                strBody.Append("        <td width='650px' align='center' style='background-color:#ff0000; color:#000000;' >")
                strBody.Append("            <b>THIS IS AN EXERCISE</b>")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        GetNotifications()

        gStrNotifications = strNotifications.ToString

        strBody.Append(gStrNotifications)

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            &nbsp;")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("    <tr>")
        strBody.Append("        <td width='650px' align='left'>")
        strBody.Append("            The State Watch Office values your feedback; please take a 1 minute <a href='https://www.surveymonkey.com/s/WRZW88G'>survey</a> about this notification.")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("</body>")
        strBody.Append("</html>")

        'GetUpdates()

        gStrTotalReport = strBody.ToString
        gStrUpdate = strUpdate.ToString
    End Sub

    Protected Overrides Sub Finalize()
        'Destructor.
    End Sub

    Protected Sub GetNotifications()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        DBConStringHelper.PrepareConnection(objConn) 'open the connection

        objCmd = New SqlCommand("[spSelectOutgoingNotificationCommentByIncidentID]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()

        If objDR.HasRows Then
            While objDR.Read
                Dim strLastName As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("LastName", "[User]", "UserID", HelpFunction.ConvertdbnullsInt(objDR.Item("UserID")))
                Dim intAgencyID As Integer = MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("AgencyID", "[User]", "UserID", HelpFunction.ConvertdbnullsInt(objDR.Item("UserID")))
                Dim strAgencyAbbreviation As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("Abbreviation", "Agency", "AgencyID", intAgencyID)

                strNotifications.Append("<table width='650px' cellspacing='0' border='0'>")
                strNotifications.Append("    <tr>")
                strNotifications.Append("        <td width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
                strNotifications.Append("            <b>Notification " & MrDataGrabber.GrabOneDateStringColumnAsMilitaryTimeByPrimaryKey("Date", "OutgoingNotificationComment", "OutgoingNotificationCommentID", objDR.Item("OutgoingNotificationCommentID")) & " / " & strAgencyAbbreviation & "-" & strLastName & "</b>" & " ")
                strNotifications.Append("        </td>")
                strNotifications.Append("    </tr>")
                strNotifications.Append("</table>")

                strNotifications.Append("<table>")
                strNotifications.Append("    <tr>")
                strNotifications.Append("        <td align='left'width='650px'>")
                strNotifications.Append("           " & objDR.Item("Notification") & "  ")
                strNotifications.Append("        </td>")
                strNotifications.Append("    </tr>")
                strNotifications.Append("</table>")

                'strUpdate.Append("<table>")
                'strUpdate.Append("    <tr>")
                'strUpdate.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
                'strUpdate.Append("            &nbsp;")
                'strUpdate.Append("        </td>")
                'strUpdate.Append("    </tr>")
                'strUpdate.Append("</table>")
            End While
        Else
            'This should never happen.
            'strUpdate.Append("<table>")
            'strUpdate.Append("    <tr>")
            'strUpdate.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            'strUpdate.Append("            <b>Update</b>")
            'strUpdate.Append("        </td>")
            'strUpdate.Append("    </tr>")
            'strUpdate.Append("</table>")
        End If

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()
    End Sub

    Protected Sub GetUpdatesForFullReport()
        Dim updateCounter As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("[spSelectUpdateReportByIncidentID]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()

        If objDR.Read Then
            objDR.Close()
            objDR = objCmd.ExecuteReader()

            While objDR.Read
                updateCounter += 1
            End While
        End If

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("[spSelectUpdateReportByIncidentID]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()

        If objDR.HasRows Then
            While objDR.Read
                Dim strLastName As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("LastName", "[User]", "UserID", HelpFunction.ConvertdbnullsInt(objDR.Item("UserID")))
                Dim intAgencyID As Integer = MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("AgencyID", "[User]", "UserID", HelpFunction.ConvertdbnullsInt(objDR.Item("UserID")))
                Dim strAgencyAbbreviation As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("Abbreviation", "Agency", "AgencyID", intAgencyID)

                If gStrReportFormat = "HTML" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
                    strBody.Append("            <b>Update " & updateCounter.ToString & " - " & objDR.Item("UpdateDate") & " / " & strAgencyAbbreviation & "-" & strLastName & "</b>")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    If gstrInstantiator <> "NotificationPage" Then
                        strBody.Append("        <pre>" & objDR.Item("UpdateReport") & "  </pre>")
                    Else
                        strBody.Append("           " & objDR.Item("UpdateReport") & "  ")
                    End If
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")

                    updateCounter -= 1
                Else
                    strBody.Append("<table width='100%'cellspacing='0' border='0'>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
                    strBody.Append("            <b>Update " & updateCounter.ToString & " - " & objDR.Item("UpdateDate") & " / " & strAgencyAbbreviation & "-" & strLastName & "</b>")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    If gstrInstantiator <> "NotificationPage" Then
                        strBody.Append("        <pre>" & objDR.Item("UpdateReport") & "  </pre>")
                    Else
                        strBody.Append("           " & objDR.Item("UpdateReport") & "  ")
                    End If
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")

                    updateCounter -= 1
                End If

                'strUpdate.Append("<table>")
                'strUpdate.Append("    <tr>")
                'strUpdate.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
                'strUpdate.Append("            &nbsp;")
                'strUpdate.Append("        </td>")
                'strUpdate.Append("    </tr>")
                'strUpdate.Append("</table>")
            End While
        Else
            'This should never happen.
            'strBody.Append("<table>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            'strBody.Append("            <b>Update</b>")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")
        End If

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()
    End Sub

    Protected Sub GetUpdates()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        DBConStringHelper.PrepareConnection(objConn) 'open the connection

        objCmd = New SqlCommand("[spSelectUpdateReportByIncidentID]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)


        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()

        If objDR.HasRows Then
            While objDR.Read
                Dim strLastName As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("LastName", "[User]", "UserID", HelpFunction.ConvertdbnullsInt(objDR.Item("UserID")))
                Dim intAgencyID As Integer = MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("AgencyID", "[User]", "UserID", HelpFunction.ConvertdbnullsInt(objDR.Item("UserID")))
                Dim strAgencyAbbreviation As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("Abbreviation", "Agency", "AgencyID", intAgencyID)

                If gStrReportFormat = "HTML" Then
                    strUpdate.Append("<table>")
                    strUpdate.Append("    <tr>")
                    strUpdate.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
                    strUpdate.Append("            <b>Update " & objDR.Item("UpdateDate") & " / " & strAgencyAbbreviation & "-" & strLastName & "</b>")
                    strUpdate.Append("        </td>")
                    strUpdate.Append("    </tr>")
                    strUpdate.Append("</table>")
                Else
                    strBody.Append("<table width='100%'cellspacing='0' border='0'>")
                    strUpdate.Append("    <tr>")
                    strUpdate.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
                    strUpdate.Append("            <b>Update " & objDR.Item("UpdateDate") & " / " & strAgencyAbbreviation & "-" & strLastName & "</b>")
                    strUpdate.Append("        </td>")
                    strUpdate.Append("    </tr>")
                    strUpdate.Append("</table>")
                End If

                strUpdate.Append("<table>")
                strUpdate.Append("    <tr>")
                strUpdate.Append("        <td align='left'width='650px'>")
                strUpdate.Append("           " & objDR.Item("UpdateReport") & "  ")
                strUpdate.Append("        </td>")
                strUpdate.Append("    </tr>")
                strUpdate.Append("</table>")
                'strUpdate.Append("<table>")
                'strUpdate.Append("    <tr>")
                'strUpdate.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
                'strUpdate.Append("            &nbsp;")
                'strUpdate.Append("        </td>")
                'strUpdate.Append("    </tr>")
                'strUpdate.Append("</table>")
            End While
        Else
            'This should never happen.
            'strUpdate.Append("<table>")
            'strUpdate.Append("    <tr>")
            'strUpdate.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            'strUpdate.Append("            <b>Update</b>")
            'strUpdate.Append("        </td>")
            'strUpdate.Append("    </tr>")
            'strUpdate.Append("</table>")
        End If

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()
    End Sub

    Protected Sub GetMainForm()
        Dim IncidentName As String = ""
        Dim IncidentStatus As Integer
        Dim IsThisADrill As String = ""
        Dim StateAssistance As String = ""
        Dim ReportingPartyTypeID As Integer
        Dim ResponsiblePartyTypeID As Integer
        Dim OnSceneContactTypeID As Integer
        Dim localTime As String = ""
        Dim ReportedToSWOTime As String = ""
        Dim ReportedToSWOTime2 As String = ""
        Dim ReportedToSWODate As String = ""
        Dim localTime2 As String = ""
        Dim IncidentOccurredTime As String = ""
        Dim IncidentOccurredTime2 As String = ""
        Dim IncidentOccurredDate As String = ""
        Dim Handled As String = ""
        Dim FacilityNameSceneDescription As String = ""
        Dim Address As String = ""
        Dim City As String = ""
        Dim City2 As String = ""
        Dim Address2 As String = ""
        Dim Zip As String = ""
        Dim Street As String = ""
        Dim Street2 As String = ""
        Dim AgencyDeptNotified As String = ""
        Dim ObtainCoordinate As String = ""
        Dim CoordinateType As String = ""
        Dim localLat As Decimal
        Dim localLong As Decimal
        Dim localUSNG As String = ""
        Dim SeverityID As Integer
        Dim localInitialReport As String = ""
        Dim LatestUpdate As String = ""
        Dim Injury As String = ""
        Dim InjuryText As String = ""
        Dim Fatality As String = ""
        Dim FatalityText As String = ""
        Dim EnvironmentalImpact As String = ""
        Dim DEPCallbackRequested As String = ""
        Dim CallbackContact As String = ""

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()

        objCmd = New SqlCommand("spSelectIncidentByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            IncidentName = HelpFunction.Convertdbnulls(objDR("IncidentName"))
            IncidentStatus = HelpFunction.Convertdbnulls(objDR("IncidentStatusID"))
            IsThisADrill = HelpFunction.Convertdbnulls(objDR("IsThisADrill"))
            StateAssistance = HelpFunction.Convertdbnulls(objDR("StateAssistance"))
            ReportingPartyTypeID = HelpFunction.ConvertdbnullsInt(objDR("ReportingPartyTypeID"))
            ResponsiblePartyTypeID = HelpFunction.ConvertdbnullsInt(objDR("ResponsiblePartyTypeID"))
            OnSceneContactTypeID = HelpFunction.ConvertdbnullsInt(objDR("OnSceneContactTypeID"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR("ReportedToSWOTime")))
            ReportedToSWODate = HelpFunction.Convertdbnulls(objDR("ReportedToSWODate"))
            localTime2 = CStr(HelpFunction.Convertdbnulls(objDR("IncidentOccurredTime")))
            IncidentOccurredDate = HelpFunction.Convertdbnulls(objDR("IncidentOccurredDate"))
            'Handled = HelpFunction.Convertdbnulls(objDR("Handled"))
            FacilityNameSceneDescription = HelpFunction.Convertdbnulls(objDR("FacilityNameSceneDescription"))
            Address = HelpFunction.Convertdbnulls(objDR("Address"))
            City = HelpFunction.Convertdbnulls(objDR("City"))
            Address2 = HelpFunction.Convertdbnulls(objDR("Address2"))
            Zip = HelpFunction.Convertdbnulls(objDR("Zip"))
            Street = HelpFunction.Convertdbnulls(objDR("Street"))
            Street2 = HelpFunction.Convertdbnulls(objDR("Street2"))
            City2 = HelpFunction.Convertdbnulls(objDR("City2"))
            'AgencyDeptNotified = HelpFunction.Convertdbnulls(objDR("AgencyDeptNotified"))
            ObtainCoordinate = HelpFunction.Convertdbnulls(objDR("ObtainCoordinate"))
            CoordinateType = HelpFunction.Convertdbnulls(objDR("CoordinateType"))
            localLat = HelpFunction.ConvertdbnullsDbl(objDR("Lat"))
            localLong = HelpFunction.ConvertdbnullsDbl(objDR("Long"))
            localUSNG = HelpFunction.Convertdbnulls(objDR("USNG"))
            SeverityID = HelpFunction.ConvertdbnullsInt(objDR("SeverityID"))
            Injury = HelpFunction.Convertdbnulls(objDR("Injury"))
            Fatality = HelpFunction.Convertdbnulls(objDR("Fatality"))
            InjuryText = HelpFunction.Convertdbnulls(objDR("InjuryText"))
            FatalityText = HelpFunction.Convertdbnulls(objDR("FatalityText"))
            EnvironmentalImpact = HelpFunction.Convertdbnulls(objDR("EnvironmentalImpact"))
            DEPCallbackRequested = HelpFunction.Convertdbnulls(objDR("DEPCallbackRequested"))
            CallbackContact = HelpFunction.Convertdbnulls(objDR("EnvironmentalImpactContact"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        '---------------------------------------------------------------------------------------
        'Response.Write("Hello")
        'Response.End()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()

        objCmd = New SqlCommand("spSelectLastInitialReportByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            localInitialReport = HelpFunction.Convertdbnulls(objDR("InitialReport"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()
        '---------------------------------------------------------------------------------------

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()

        objCmd = New SqlCommand("spSelectLastUpdateReportByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            LatestUpdate = HelpFunction.Convertdbnulls(objDR("UpdateReport"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        Dim intShowReortingParty As Integer = 0

        intShowReortingParty = MrDataGrabber.GrabRecordCountBy2Keys("IncidentIncidentType", "IncidentTypeID", "18", "IncidentID", gStrIncidentID)

        IncidentOccurredTime = Left(localTime2, 2)
        IncidentOccurredTime2 = Right(localTime2, 2)

        ReportedToSWOTime = Left(localTime, 2)
        ReportedToSWOTime2 = Right(localTime, 2)

        Dim localAllCounties As String = ""
        Dim localStateWide As Boolean
        Dim localRegion1 As Boolean
        Dim localRegion2 As Boolean
        Dim localRegion3 As Boolean
        Dim localRegion4 As Boolean
        Dim localRegion5 As Boolean
        Dim localRegion6 As Boolean
        Dim localRegion7 As Boolean
        Dim localBay As Boolean
        Dim localCalhoun As Boolean
        Dim localEscambia As Boolean
        Dim localGulf As Boolean
        Dim localHolmes As Boolean
        Dim localJackson As Boolean
        Dim localOkaloosa As Boolean
        Dim localSantaRosa As Boolean
        Dim localWalton As Boolean
        Dim localWashington As Boolean
        Dim localColumbia As Boolean
        Dim localDixie As Boolean
        Dim localFranklin As Boolean
        Dim localGadsden As Boolean
        Dim localHamilton As Boolean
        Dim localJefferson As Boolean
        Dim localLafayette As Boolean
        Dim localLeon As Boolean
        Dim localLevy As Boolean
        Dim localLiberty As Boolean
        Dim localMadison As Boolean
        Dim localSuwannee As Boolean
        Dim localTaylor As Boolean
        Dim localWakulla As Boolean
        Dim localAlachua As Boolean
        Dim localBaker As Boolean
        Dim localBradford As Boolean
        Dim localClay As Boolean
        Dim localDuval As Boolean
        Dim localFlagler As Boolean
        Dim localGilchrist As Boolean
        Dim localMarion As Boolean
        Dim localNassau As Boolean
        Dim localPutnam As Boolean
        Dim localStJohns As Boolean
        Dim localUnion As Boolean
        Dim localCitrus As Boolean
        Dim localHardee As Boolean
        Dim localHernando As Boolean
        Dim localHillsborough As Boolean
        Dim localPasco As Boolean
        Dim localPinellas As Boolean
        Dim localPolk As Boolean
        Dim localSumter As Boolean
        Dim localBrevard As Boolean
        Dim localIndianRiver As Boolean
        Dim localLake As Boolean
        Dim localMartin As Boolean
        Dim localOrange As Boolean
        Dim localOsceola As Boolean
        Dim localSeminole As Boolean
        Dim localStLucie As Boolean
        Dim localVolusia As Boolean
        Dim localCharlotte As Boolean
        Dim localCollier As Boolean
        Dim localDeSoto As Boolean
        Dim localGlades As Boolean
        Dim localHendry As Boolean
        Dim localHighlands As Boolean
        Dim localLee As Boolean
        Dim localManatee As Boolean
        Dim localOkeechobee As Boolean
        Dim localSarasota As Boolean
        Dim localBroward As Boolean
        Dim localMiamiDade As Boolean
        Dim localMonroe As Boolean
        Dim localPalmBeach As Boolean

        Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()

            objCmd = New SqlCommand("spSelectCountyRegionCheckByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localStateWide = HelpFunction.ConvertdbnullsBool(objDR("Statewide"))
                localRegion1 = HelpFunction.ConvertdbnullsBool(objDR("Region1"))
                localRegion2 = HelpFunction.ConvertdbnullsBool(objDR("Region2"))
                localRegion3 = HelpFunction.ConvertdbnullsBool(objDR("Region3"))
                localRegion4 = HelpFunction.ConvertdbnullsBool(objDR("Region4"))
                localRegion5 = HelpFunction.ConvertdbnullsBool(objDR("Region5"))
                localRegion6 = HelpFunction.ConvertdbnullsBool(objDR("Region6"))
                localRegion7 = HelpFunction.ConvertdbnullsBool(objDR("Region7"))
                localBay = HelpFunction.ConvertdbnullsBool(objDR("Bay"))
                localCalhoun = HelpFunction.ConvertdbnullsBool(objDR("Calhoun"))
                localEscambia = HelpFunction.ConvertdbnullsBool(objDR("Escambia"))
                localGulf = HelpFunction.ConvertdbnullsBool(objDR("Gulf"))
                localHolmes = HelpFunction.ConvertdbnullsBool(objDR("Holmes"))
                localJackson = HelpFunction.ConvertdbnullsBool(objDR("Jackson"))
                localOkaloosa = HelpFunction.ConvertdbnullsBool(objDR("Okaloosa"))
                localSantaRosa = HelpFunction.ConvertdbnullsBool(objDR("Santa Rosa"))
                localWalton = HelpFunction.ConvertdbnullsBool(objDR("Walton"))
                localWashington = HelpFunction.ConvertdbnullsBool(objDR("Washington"))
                localColumbia = HelpFunction.ConvertdbnullsBool(objDR("Columbia"))
                localDixie = HelpFunction.ConvertdbnullsBool(objDR("Dixie"))
                localFranklin = HelpFunction.ConvertdbnullsBool(objDR("Franklin"))
                localGadsden = HelpFunction.ConvertdbnullsBool(objDR("Gadsden"))
                localHamilton = HelpFunction.ConvertdbnullsBool(objDR("Hamilton"))
                localJefferson = HelpFunction.ConvertdbnullsBool(objDR("Jefferson"))
                localLafayette = HelpFunction.ConvertdbnullsBool(objDR("Lafayette"))
                localLeon = HelpFunction.ConvertdbnullsBool(objDR("Leon"))
                localLevy = HelpFunction.ConvertdbnullsBool(objDR("Levy"))
                localLiberty = HelpFunction.ConvertdbnullsBool(objDR("Liberty"))
                localMadison = HelpFunction.ConvertdbnullsBool(objDR("Madison"))
                localSuwannee = HelpFunction.ConvertdbnullsBool(objDR("Suwannee"))
                localTaylor = HelpFunction.ConvertdbnullsBool(objDR("Taylor"))
                localWakulla = HelpFunction.ConvertdbnullsBool(objDR("Wakulla"))
                localAlachua = HelpFunction.ConvertdbnullsBool(objDR("Alachua"))
                localBaker = HelpFunction.ConvertdbnullsBool(objDR("Baker"))
                localBradford = HelpFunction.ConvertdbnullsBool(objDR("Bradford"))
                localClay = HelpFunction.ConvertdbnullsBool(objDR("Clay"))
                localDuval = HelpFunction.ConvertdbnullsBool(objDR("Duval"))
                localFlagler = HelpFunction.ConvertdbnullsBool(objDR("Flagler"))
                localGilchrist = HelpFunction.ConvertdbnullsBool(objDR("Gilchrist"))
                localMarion = HelpFunction.ConvertdbnullsBool(objDR("Marion"))
                localNassau = HelpFunction.ConvertdbnullsBool(objDR("Nassau"))
                localPutnam = HelpFunction.ConvertdbnullsBool(objDR("Putnam"))
                localStJohns = HelpFunction.ConvertdbnullsBool(objDR("St. Johns"))
                localUnion = HelpFunction.ConvertdbnullsBool(objDR("Union"))
                localCitrus = HelpFunction.ConvertdbnullsBool(objDR("Citrus"))
                localHardee = HelpFunction.ConvertdbnullsBool(objDR("Hardee"))
                localHernando = HelpFunction.ConvertdbnullsBool(objDR("Hernando"))
                localHillsborough = HelpFunction.ConvertdbnullsBool(objDR("Hillsborough"))
                localPasco = HelpFunction.ConvertdbnullsBool(objDR("Pasco"))
                localPinellas = HelpFunction.ConvertdbnullsBool(objDR("Pinellas"))
                localPolk = HelpFunction.ConvertdbnullsBool(objDR("Polk"))
                localSumter = HelpFunction.ConvertdbnullsBool(objDR("Sumter"))
                localBrevard = HelpFunction.ConvertdbnullsBool(objDR("Brevard"))
                localIndianRiver = HelpFunction.ConvertdbnullsBool(objDR("Indian River"))
                localLake = HelpFunction.ConvertdbnullsBool(objDR("Lake"))
                localMartin = HelpFunction.ConvertdbnullsBool(objDR("Martin"))
                localOrange = HelpFunction.ConvertdbnullsBool(objDR("Orange"))
                localOsceola = HelpFunction.ConvertdbnullsBool(objDR("Osceola"))
                localSeminole = HelpFunction.ConvertdbnullsBool(objDR("Seminole"))
                localStLucie = HelpFunction.ConvertdbnullsBool(objDR("St. Lucie"))
                localVolusia = HelpFunction.ConvertdbnullsBool(objDR("Volusia"))
                localCharlotte = HelpFunction.ConvertdbnullsBool(objDR("Charlotte"))
                localCollier = HelpFunction.ConvertdbnullsBool(objDR("Collier"))
                localDeSoto = HelpFunction.ConvertdbnullsBool(objDR("DeSoto"))
                localGlades = HelpFunction.ConvertdbnullsBool(objDR("Glades"))
                localHendry = HelpFunction.ConvertdbnullsBool(objDR("Hendry"))
                localHighlands = HelpFunction.ConvertdbnullsBool(objDR("Highlands"))
                localLee = HelpFunction.ConvertdbnullsBool(objDR("Lee"))
                localManatee = HelpFunction.ConvertdbnullsBool(objDR("Manatee"))
                localOkeechobee = HelpFunction.ConvertdbnullsBool(objDR("Okeechobee"))
                localSarasota = HelpFunction.ConvertdbnullsBool(objDR("Sarasota"))
                localBroward = HelpFunction.ConvertdbnullsBool(objDR("Broward"))
                localMiamiDade = HelpFunction.ConvertdbnullsBool(objDR("Miami-Dade"))
                localMonroe = HelpFunction.ConvertdbnullsBool(objDR("Monroe"))
                localPalmBeach = HelpFunction.ConvertdbnullsBool(objDR("Palm Beach"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()
        Catch ex As Exception
            'Response.Write(ex.ToString)
            Exit Sub
        End Try

        'If localStateWide = True Then
        '    localAllCounties = localAllCounties & " Statewide, "
        'End If

        'If localRegion1 = True Then
        '    localAllCounties = localAllCounties & " Region1, "
        'End If

        'If localRegion2 = True Then
        '    localAllCounties = localAllCounties & " Region2, "
        'End If

        'If localRegion3 = True Then
        '    localAllCounties = localAllCounties & " Region3, "
        'End If

        'If localRegion4 = True Then
        '    localAllCounties = localAllCounties & " Region4, "
        'End If

        'If localRegion5 = True Then
        '    localAllCounties = localAllCounties & " Region5, "
        'End If

        'If localRegion6 = True Then
        '    localAllCounties = localAllCounties & " Region6, "
        'End If

        'If localRegion7 = True Then
        '    localAllCounties = localAllCounties & " Region7, "
        'End If

        If localAlachua = True Then
            localAllCounties = localAllCounties & " Alachua, "
        End If

        If localBaker = True Then
            localAllCounties = localAllCounties & " Baker, "
        End If

        If localBay = True Then
            localAllCounties = localAllCounties & " Bay, "
        End If

        If localBradford = True Then
            localAllCounties = localAllCounties & " Bradford, "
        End If

        If localBrevard = True Then
            localAllCounties = localAllCounties & " Brevard, "
        End If

        If localBroward = True Then
            localAllCounties = localAllCounties & " Broward, "
        End If

        If localCalhoun = True Then
            localAllCounties = localAllCounties & " Calhoun, "
        End If

        If localCharlotte = True Then
            localAllCounties = localAllCounties & " Charlotte, "
        End If

        If localCitrus = True Then
            localAllCounties = localAllCounties & " Citrus, "
        End If

        If localClay = True Then
            localAllCounties = localAllCounties & " Clay, "
        End If

        If localCollier = True Then
            localAllCounties = localAllCounties & " Collier, "
        End If

        If localColumbia = True Then
            localAllCounties = localAllCounties & " Columbia, "
        End If

        If localDeSoto = True Then
            localAllCounties = localAllCounties & " DeSoto, "
        End If

        If localDixie = True Then
            localAllCounties = localAllCounties & " Dixie, "
        End If

        If localDuval = True Then
            localAllCounties = localAllCounties & " Duval, "
        End If

        If localEscambia = True Then
            localAllCounties = localAllCounties & " Escambia, "
        End If

        If localFlagler = True Then
            localAllCounties = localAllCounties & " Flagler, "
        End If

        If localFranklin = True Then
            localAllCounties = localAllCounties & " Franklin, "
        End If

        If localGadsden = True Then
            localAllCounties = localAllCounties & " Gadsden, "
        End If

        If localGilchrist = True Then
            localAllCounties = localAllCounties & " Gilchrist, "
        End If

        If localGlades = True Then
            localAllCounties = localAllCounties & " Glades, "
        End If

        If localGulf = True Then
            localAllCounties = localAllCounties & " Gulf, "
        End If

        If localHamilton = True Then
            localAllCounties = localAllCounties & " Hamilton, "
        End If

        If localHardee = True Then
            localAllCounties = localAllCounties & " Hardee, "
        End If

        If localHendry = True Then
            localAllCounties = localAllCounties & " Hendry, "
        End If

        If localHernando = True Then
            localAllCounties = localAllCounties & " Hernando, "
        End If

        If localHighlands = True Then
            localAllCounties = localAllCounties & " Highlands, "
        End If

        If localHillsborough = True Then
            localAllCounties = localAllCounties & " Hillsborough, "
        End If

        If localHolmes = True Then
            localAllCounties = localAllCounties & " Holmes, "
        End If

        If localIndianRiver = True Then
            localAllCounties = localAllCounties & " Indian River, "
        End If

        If localJackson = True Then
            localAllCounties = localAllCounties & " Jackson, "
        End If

        If localJefferson = True Then
            localAllCounties = localAllCounties & " Jefferson, "
        End If

        If localLafayette = True Then
            localAllCounties = localAllCounties & " Lafayette, "
        End If

        If localLake = True Then
            localAllCounties = localAllCounties & " Lake, "
        End If

        If localLee = True Then
            localAllCounties = localAllCounties & " Lee, "
        End If

        If localLeon = True Then
            localAllCounties = localAllCounties & " Leon, "
        End If

        If localLevy = True Then
            localAllCounties = localAllCounties & " Levy, "
        End If

        If localLiberty = True Then
            localAllCounties = localAllCounties & " Liberty, "
        End If

        If localMadison = True Then
            localAllCounties = localAllCounties & " Madison, "
        End If

        If localManatee = True Then
            localAllCounties = localAllCounties & " Manatee, "
        End If

        If localMarion = True Then
            localAllCounties = localAllCounties & " Marion, "
        End If

        If localMartin = True Then
            localAllCounties = localAllCounties & " Martin, "
        End If

        If localMiamiDade = True Then
            localAllCounties = localAllCounties & " Miami-Dade, "
        End If

        If localMonroe = True Then
            localAllCounties = localAllCounties & " Monroe, "
        End If

        If localNassau = True Then
            localAllCounties = localAllCounties & " Nassau, "
        End If

        If localOkaloosa = True Then
            localAllCounties = localAllCounties & " Okaloosa, "
        End If

        If localOkeechobee = True Then
            localAllCounties = localAllCounties & " Okeechobee, "
        End If

        If localOrange = True Then
            localAllCounties = localAllCounties & " Orange, "
        End If

        If localOsceola = True Then
            localAllCounties = localAllCounties & " Osceola, "
        End If

        If localPalmBeach = True Then
            localAllCounties = localAllCounties & " Palm Beach, "
        End If

        If localPasco = True Then
            localAllCounties = localAllCounties & " Pasco, "
        End If

        If localPinellas = True Then
            localAllCounties = localAllCounties & " Pinellas, "
        End If

        If localPolk = True Then
            localAllCounties = localAllCounties & " Polk, "
        End If

        If localPutnam = True Then
            localAllCounties = localAllCounties & " Putnam, "
        End If

        If localSantaRosa = True Then
            localAllCounties = localAllCounties & " Santa Rosa, "
        End If

        If localSarasota = True Then
            localAllCounties = localAllCounties & " Sarasota, "
        End If

        If localSeminole = True Then
            localAllCounties = localAllCounties & " Seminole, "
        End If

        If localStJohns = True Then
            localAllCounties = localAllCounties & " St. Johns, "
        End If

        If localStLucie = True Then
            localAllCounties = localAllCounties & " St. Lucie, "
        End If

        If localSumter = True Then
            localAllCounties = localAllCounties & " Sumter, "
        End If

        If localSuwannee = True Then
            localAllCounties = localAllCounties & " Suwannee, "
        End If

        If localTaylor = True Then
            localAllCounties = localAllCounties & " Taylor, "
        End If

        If localUnion = True Then
            localAllCounties = localAllCounties & " Union, "
        End If

        If localVolusia = True Then
            localAllCounties = localAllCounties & " Volusia, "
        End If

        If localWakulla = True Then
            localAllCounties = localAllCounties & " Wakulla, "
        End If

        If localWalton = True Then
            localAllCounties = localAllCounties & " Walton, "
        End If

        If localWashington = True Then
            localAllCounties = localAllCounties & " Washington, "
        End If

        'Gets rid of Last ",".
        If localAllCounties <> "" Then
            localAllCounties = localAllCounties.Remove(localAllCounties.Length - 2, 2)
        Else
            localAllCounties = " NO COUNTIES ADDED AT THIS TIME"
        End If

        'IncidentNumber.
        Dim localYear As String = ""
        Dim localNumber As Integer

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()

        objCmd = New SqlCommand("spSelectIncidentNumberByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            localYear = HelpFunction.Convertdbnulls(objDR("Year"))
            localNumber = HelpFunction.ConvertdbnullsInt(objDR("Number"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        'IncidentNumber.
        Dim localSeverity As String = ""
        Dim localSeverityColor As String = ""

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()

        objCmd = New SqlCommand("spSelectSeverityBySeverityID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@SeverityID", SeverityID)

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then
            localSeverity = HelpFunction.Convertdbnulls(objDR("Severity"))
            localSeverityColor = HelpFunction.Convertdbnulls(objDR("Color"))
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        Dim localThisSituationInvolves As String = ""

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        DBConStringHelper.PrepareConnection(objConn) 'open the connection
        objCmd = New SqlCommand("[spSelectIncidentIncidentTypeByIncidentID]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()

        If objDR.HasRows Then
            'There are records.
            While objDR.Read
                If Not localThisSituationInvolves.Contains(CStr(objDR.Item("IncidentType"))) Then
                    localThisSituationInvolves = localThisSituationInvolves & CStr(objDR.Item("IncidentType")) & ", "
                End If
            End While
        End If

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()

        'Gets rid of Last ",".
        If localThisSituationInvolves <> "" Then
            localThisSituationInvolves = localThisSituationInvolves.Remove(localThisSituationInvolves.Length - 2, 2)
        Else
            localThisSituationInvolves = " NO INCIDENT WORKSHEETS ADDED AT THIS TIME"
        End If


        Dim localAffectedSectors As String = ""

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        DBConStringHelper.PrepareConnection(objConn) 'open the connection
        objCmd = New SqlCommand("[spSelectIncidentSectorByIncidentID]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()

        If objDR.HasRows Then
            While objDR.Read
                localAffectedSectors = localAffectedSectors & CStr(objDR.Item("SectorName")) & ", "
            End While

            objDR.Close()
            localAffectedSectors = localAffectedSectors.Remove(localAffectedSectors.Length - 2)
        End If

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()


        gStrIsThisADrill = IsThisADrill

        'If this is a Drill Show.
        If IsThisADrill = "Yes" Then
            If gStrReportFormat = "HTML" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td width='650px' align='center' style='background-color:#ff0000; color:#000000;' >")
                strBody.Append("            <b>THIS IS AN EXERCISE</b>")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            Else
                strBody.Append("<table width='100%'cellspacing='0' border='0'>")
                strBody.Append("    <tr>")
                strBody.Append("        <td  width='650px' align='center' style='background-color:#ff0000; color:#000000;' >")
                strBody.Append("            <b>THIS IS AN EXERCISE</b>")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        If gStrReportFormat = "HTML" Then
            'Report Name.
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            &nbsp;")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left' width='80px'>")
            strBody.Append("            <img id='imgSertLogo' alt='SertLogo' src='" & HttpContext.Current.Request.Url.Scheme & "://" & HttpContext.Current.Request.Url.Authority & Current.Request.ApplicationPath & "/Images/SertLogoGoodReport.jpg' />")
            strBody.Append("        </td>")

            'Added/edited by JD --->
            '---------------------------
            strBody.Append("        <td valign='middle' align='center' width='490px'>")
            strBody.Append("            <table>")
            strBody.Append("                <tr>")
            strBody.Append("                    <td align='center'>")
            strBody.Append("                        <font size='5.5'>Florida Division of Emergency Management</font>")
            strBody.Append("                    </td>")
            strBody.Append("                </tr>")
            strBody.Append("                <tr>")
            strBody.Append("                    <td align='center'>")
            strBody.Append("                        <font size='5.5'>State Watch Office</font>")
            strBody.Append("                    </td>")
            strBody.Append("                </tr>")
            strBody.Append("                <tr>")
            strBody.Append("                    <td align='center'>")
            strBody.Append("                        <font size='5.5'>Incident Report</font>")
            strBody.Append("                    </td>")
            strBody.Append("                </tr>")
            strBody.Append("            </table>")
            strBody.Append("        </td>")
            '---------------------------
            'Added/edited by JD <---

            strBody.Append("        <td align='left' width='80px'>")
            strBody.Append("            <img id='imgSealLogo' alt='SealLogo' src='" & HttpContext.Current.Request.Url.Scheme & "://" & HttpContext.Current.Request.Url.Authority & Current.Request.ApplicationPath & "/Images/FloridaSealReport.jpg' />")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            'Report Name.
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            &nbsp;")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='80px'>")
            strBody.Append("            <img id='imgSertLogo' alt='SertLogo' src='" & HttpContext.Current.Request.Url.Scheme & "://" & HttpContext.Current.Request.Url.Authority & Current.Request.ApplicationPath & "/Images/SertLogoGoodReport.jpg' />")
            strBody.Append("        </td>")
            'strBody.Append("        <td valign='middle' align='center' width='490px'>")
            'strBody.Append("                <font size='5.5'>FDEM SWO Incident Report</font>")
            'strBody.Append("        </td>")

            'Added/edited by JD --->
            '---------------------------
            'This may be unecessary.
            '---------------------------------------------------------------------------------------
            strBody.Append("        <td valign='middle' align='center' width='490px'>")
            strBody.Append("            <table>")
            strBody.Append("                <tr>")
            strBody.Append("                    <td align='center'>")
            strBody.Append("                        <font size='5.5'>Florida Division of Emergency Management</font>")
            strBody.Append("                    </td>")
            strBody.Append("                </tr>")
            strBody.Append("                <tr>")
            strBody.Append("                    <td align='center'>")
            strBody.Append("                        <font size='5.5'>State Watch Office</font>")
            strBody.Append("                    </td>")
            strBody.Append("                </tr>")
            strBody.Append("                <tr>")
            strBody.Append("                    <td align='center'>")
            strBody.Append("                        <font size='5.5'>Incident Report</font>")
            strBody.Append("                    </td>")
            strBody.Append("                </tr>")
            strBody.Append("            </table>")
            strBody.Append("        </td>")
            '---------------------------------------------------------------------------------------
            '---------------------------
            'Added/Edited by JD <---

            strBody.Append("        <td align='left'width='80px'>")
            strBody.Append("            <img id='imgSealLogo' alt='SertLogo' src='" & HttpContext.Current.Request.Url.Scheme & "://" & HttpContext.Current.Request.Url.Authority & Current.Request.ApplicationPath & "/Images/FloridaSealReport.jpg' />")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        '---------------------------------------------------------------------------------------
        'strBody.Append("<br>")
        'strBody.Append("<br>")
        'strBody.Append("<br>")
        'strBody.Append("<br>")

        'If this is a Drill Show.

        'Exempt Checker Start.
        Dim IsExempt As Boolean = False
        Dim localExemptCount As Integer = 0
        Dim localRecordAccountForArray As Integer = 0

        localRecordAccountForArray = MrDataGrabber.GrabRecordCountByKey("IncidentIncidentType", "IncidentID", gStrIncidentID)

        Dim localIncidentIncidentTypeLoopCount As Integer = 0

        If localRecordAccountForArray <> 0 Then
            'Must minus 1 to account for the Array Declaration.
            Dim arrIncidentType(localRecordAccountForArray - 1) As Integer

            'Store each IncidentTypeID in Array.
            'Checking to see if there are any worksheets that are exempt.
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectIncidentIncidentTypeByIncidentID]", objConn)
            objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.HasRows Then
                'There are records.
                While objDR.Read
                    arrIncidentType(localIncidentIncidentTypeLoopCount) = objDR.Item("IncidentTypeID")
                    localIncidentIncidentTypeLoopCount = localIncidentIncidentTypeLoopCount + 1
                End While

                localIncidentIncidentTypeLoopCount = 0
            Else

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

            While localIncidentIncidentTypeLoopCount < localRecordAccountForArray
                localExemptCount = MrDataGrabber.GrabRecordCountByKey("Exempt", "IncidentTypeID", arrIncidentType(localIncidentIncidentTypeLoopCount))

                If localExemptCount > 0 Then
                    IsExempt = True
                End If

                localIncidentIncidentTypeLoopCount = localIncidentIncidentTypeLoopCount + 1
                localExemptCount = 0
            End While
        End If

        If IsExempt = True Then
            If gStrReportFormat = "HTML" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td  width='650px' align='left' style='background-color:#c2ecde; color:#000000;' >")
                strBody.Append("            CONFIDENTIAL - FOUO")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left' width='650px' style='background-color:White; height:25px; color:#ff0000; font:Arial; font-size:1.1em;'> ")
                strBody.Append("            <b>This report is exempt from public records disclosure pursuant to § 119.071 F.S.</b> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            Else
                strBody.Append("<table width='100%'cellspacing='0' border='0'>")
                strBody.Append("    <tr>")
                strBody.Append("        <td  width='650px' align='left' style='background-color:#c2ecde; color:#000000;' >")
                strBody.Append("            CONFIDENTIAL - FOUO")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                strBody.Append("<table width='100%'cellspacing='0' border='0'>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left' width='650px' style='background-color:White; height:25px; color:#ff0000; font:Arial; font-size:1.1em;'> ")
                strBody.Append("            <b>This report is exempt from public records disclosure pursuant to § 119.071 F.S.</b> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            'strBody.Append("<table>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td  width='650px' align='left' style='background-color:#c2ecde; color:#000000;' >")
            'strBody.Append("            &nbsp;")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")
        End If

        'Exempt Checker End.

        If localLat <> 0 And localLong <> 0 Then
            If gStrReportFormat = "HTML" Then
                '---------------------------------------------------------------------------------------
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
                strBody.Append("            <b>Map</b>")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                '---------------------------------------------------------------------------------------
                strBody.Append("<iframe src='https://floridadisaster.maps.arcgis.com/apps/webappviewer/index.html?id=74f3e78117fd44b28ffec5adc30c6024&scale=2000&marker=" & localLong & "," & localLat & "' style='border: 0px #ffffff none;' name='myiFrame' scrolling='no' frameborder='1' marginheight='0px' marginwidth='0px' height='400px' width='653px' allowfullscreen></iframe>")

                'strBody.Append("<table>")
                'strBody.Append("    <tr>")
                'strBody.Append("        <td align='left'width='325px'>")
                'strBody.Append("            <img id='imgMap1' alt='Map1' src='http://maps.googleapis.com/maps/api/staticmap?center=" & localLat & "," & localLong & "&markers=size:mid|color:red|" & localLat & "," & localLong & "&zoom=7&size=321x250&maptype=street&format=jpg&key=ABQIAAAAaa6B5ZMUVanPrZJU5dhtshRzymbT3klSnJpNv7EI1uNYq_UBqhTmwXd4YDorUwqRsabizyja-ZgPoQ' />" & " ")
                'strBody.Append("        </td>")
                'strBody.Append("        <td align='left'width='325px'>")
                'strBody.Append("            <img id='imgMap2' alt='Map2' src='http://maps.googleapis.com/maps/api/staticmap?center=" & localLat & "," & localLong & "&markers=size:mid|color:red|" & localLat & "," & localLong & "&zoom=14&size=321x250&maptype=street&format=jpg&key=ABQIAAAAaa6B5ZMUVanPrZJU5dhtshRzymbT3klSnJpNv7EI1uNYq_UBqhTmwXd4YDorUwqRsabizyja-ZgPoQ' />" & " ")
                'strBody.Append("        </td>")
                'strBody.Append("    </tr>")
                'strBody.Append("</table>")
            Else
                '---------------------------------------------------------------------------------------
                strBody.Append("<table width='100%'cellspacing='0' border='0'>")
                strBody.Append("    <tr>")
                strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
                strBody.Append("            <b>Maps</b>")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                '---------------------------------------------------------------------------------------
                strBody.Append("<table width='100%'cellspacing='0' border='0'>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='325px'>")
                strBody.Append("            <img id='imgMap1' alt='Map1' src='http://maps.googleapis.com/maps/api/staticmap?center=" & localLat & "," & localLong & "&markers=size:mid|color:red|" & localLat & "," & localLong & "&zoom=7&size=321x250&maptype=street&format=jpg&key=ABQIAAAAaa6B5ZMUVanPrZJU5dhtshRzymbT3klSnJpNv7EI1uNYq_UBqhTmwXd4YDorUwqRsabizyja-ZgPoQ' />" & " ")
                strBody.Append("        </td>")
                strBody.Append("        <td align='left'width='325px'>")
                strBody.Append("            <img id='imgMap2' alt='Map2' src='http://maps.googleapis.com/maps/api/staticmap?center=" & localLat & "," & localLong & "&markers=size:mid|color:red|" & localLat & "," & localLong & "&zoom=14&size=321x250&maptype=street&format=jpg&key=ABQIAAAAaa6B5ZMUVanPrZJU5dhtshRzymbT3klSnJpNv7EI1uNYq_UBqhTmwXd4YDorUwqRsabizyja-ZgPoQ' />" & " ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        GetAttachments()

        GetUpdatesForFullReport()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Main Information</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Main Information</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'Response.Write(StateAssistance)
        'Response.End()

        'Added/edited by JD --->
        '---------------------------
        If StateAssistance = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <i>STATE ASSISTANCE REQUESTED</i>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            'ElseIf StateAssistance = "No" Then
            '    strBody.Append("<table>")
            '    strBody.Append("    <tr>")
            '    strBody.Append("        <td align='left'width='650px'>")
            '    strBody.Append("            <i>NO STATE ASSISTANCE REQUESTED</i>")
            '    strBody.Append("        </td>")
            '    strBody.Append("    </tr>")
            '    strBody.Append("</table>")
        End If
        '---------------------------
        'Add/edited by JD <---

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>Report #:</b> ")
        strBody.Append("                " & localYear & "-" & CStr(localNumber) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>Status:</b> ")
        strBody.Append("         " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentStatus", "IncidentStatus", "IncidentStatusID", MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("IncidentStatusID", "Incident", "IncidentID", gStrIncidentID).ToString).ToString & " ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        'Check for agency contacts.
        '-----------------------------------------------------
        'Connect and build the datagrid.
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        'Open the connection.
        DBConStringHelper.PrepareConnection(objConn)

        objCmd = New SqlCommand("spSelectAgencyContactByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

        objDS.Tables.Clear()

        'Bind our data.
        objDA = New SqlDataAdapter(objCmd)
        objDA.Fill(objDS)
        objCmd.Dispose()
        objCmd = Nothing

        'Close the connection.
        DBConStringHelper.FinalizeConnection(objConn)
        '-----------------------------------------------------

        If objDS.Tables(0).Rows.Count <> 0 Then
            Dim strAgencyContacts As String = ""

            For Each objAgency As DataRow In objDS.Tables(0).Rows
                strAgencyContacts = strAgencyContacts & objAgency.Item(2).ToString & " / " & objAgency.Item(3).ToString & " /// "
            Next

            strAgencyContacts = strAgencyContacts.Substring(0, strAgencyContacts.Length - 5)

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Assigned To:</b> ")
            strBody.Append("           " & strAgencyContacts & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>Reported to SWO on:</b> ")
        strBody.Append("        " & ReportedToSWODate & " &nbsp; " & ReportedToSWOTime & ":" & ReportedToSWOTime2 & " ET ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>Severity:</b> ")
        strBody.Append("           " & localSeverity & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("            " & IncidentName & "   ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>This situation involves:</b> ")
        strBody.Append("        " & localThisSituationInvolves & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If localAffectedSectors <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Affected Sectors:</b> ")
            strBody.Append("        " & localAffectedSectors & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>Initial Report:</b> ")

        If localThisSituationInvolves = "Weather Advisories and Reports" Then
            strBody.Append("        <pre>" & localInitialReport & "     </pre>")
        Else
            '            strBody.Append("        " & localInitialReport & "     ")
            strBody.Append("        <pre>" & localInitialReport & "     </pre>")
        End If

        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>Injuries:</b>")
        strBody.Append("            " & Injury & If(Injury = "Yes", " (" & InjuryText & ")", ""))
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>Fatalities</b> (Unconfirmed by State Medical Examiner):")
        strBody.Append("            " & Fatality & If(Fatality = "Yes", " (" & FatalityText & ")", ""))
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>Environmental impact:</b>")
        strBody.Append("            " & EnvironmentalImpact)
        strBody.Append("        </td>")
        strBody.Append("    </tr>")

        If EnvironmentalImpact = "Yes" Then
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>&nbsp;&nbsp;DEP callback requested:</b>")
            strBody.Append("            " & DEPCallbackRequested)
            strBody.Append("        </td>")
            strBody.Append("    </tr>")

            If DEPCallbackRequested = "Yes" Then
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>&nbsp;&nbsp;&nbsp;&nbsp;Contact:</b>")
                strBody.Append("            " & CallbackContact)
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
            End If
        End If

        strBody.Append("</table>")
        strBody.Append("<table>")
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>Incident Occurred:</b>")
        strBody.Append("            " & IncidentOccurredDate & " &nbsp; " & IncidentOccurredTime & ":" & IncidentOccurredTime2 & " ET ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>Most Recent Update Date/Time:</b>")
        strBody.Append("        " & MrDataGrabber.GrabOneDateStringColumnAsMilitaryTimeByPrimaryKey("LastUpdated", "Incident", "IncidentID", gStrIncidentID).ToString & " ET ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>Most Recent Update:</b>")

        If LatestUpdate = "" Then
            strBody.Append("        N/A     ")
        Else
            strBody.Append("        " & LatestUpdate & "     ")
        End If

        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("                    <b>Affected Counties:</b>     ")

        If localStateWide = False Then
            Dim strRegions As String = ""
            Dim strCountiesInRegions As String = ""

            If localRegion1 Then
                strCountiesInRegions = oCountyRegions.GetCountiesByRegion(1) & ", "
                strRegions = "Region 1, "
            End If
            If localRegion2 Then
                strCountiesInRegions = oCountyRegions.GetCountiesByRegion(2) & ", "
                strRegions = strRegions & "Region 2, "
            End If
            If localRegion3 Then
                strCountiesInRegions = strCountiesInRegions & oCountyRegions.GetCountiesByRegion(3) & ", "
                strRegions = strRegions & "Region 3, "
            End If
            If localRegion4 Then
                strCountiesInRegions = strCountiesInRegions & oCountyRegions.GetCountiesByRegion(4) & ", "
                strRegions = strRegions & "Region 4, "
            End If
            If localRegion5 Then
                strCountiesInRegions = strCountiesInRegions & oCountyRegions.GetCountiesByRegion(5) & ", "
                strRegions = strRegions & "Region 5, "
            End If
            If localRegion6 Then
                strCountiesInRegions = strCountiesInRegions & oCountyRegions.GetCountiesByRegion(6) & ", "
                strRegions = strRegions & "Region 6, "
            End If
            If localRegion7 Then
                strCountiesInRegions = strCountiesInRegions & oCountyRegions.GetCountiesByRegion(7)
                strRegions = strRegions & "Region 7, "
            End If

            Dim arrCountiesInRegions As String() = strCountiesInRegions.Replace(", ", ",").Split(",")
            Dim arrLocalAllCounties As String() = localAllCounties.Replace(",  ", ",").Split(",")
            Dim strCountiesRemaining As String = ""

            For i As Int16 = 0 To arrLocalAllCounties.GetLength(0) - 1
                If Array.IndexOf(arrCountiesInRegions, Trim(arrLocalAllCounties(i))) = -1 Then
                    strCountiesRemaining = strCountiesRemaining & arrLocalAllCounties(i) & ", "
                End If
            Next

            Dim strRegionsAndCounties As String = strRegions & strCountiesRemaining
            strRegionsAndCounties = strRegionsAndCounties.TrimEnd(","c, " "c)

            strBody.Append("                    " & strRegionsAndCounties & "     ")
        Else
            strBody.Append("           Statewide  ")
        End If

        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("                <b>Facility Name or Description:</b>     ")
        strBody.Append("                    " & FacilityNameSceneDescription & "     ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("                <b>Incident Location:</b>     ")

        If ObtainCoordinate = "AddressCity" Then
            strBody.Append("                   <i>Address:</i> " & Address & " <i>City:</i> " & City & "  ")
        ElseIf ObtainCoordinate = "AddressZip" Then
            strBody.Append("                   <i>Address:</i> " & Address2 & " <i>Zip:</i> " & Zip & "  ")
        ElseIf ObtainCoordinate = "Intersection" Then
            strBody.Append("                   <i>Street 1:</i> " & Street & " <i>Street 2:</i> " & Street2 & " <i>City:</i> " & City2 & " ")
        ElseIf ObtainCoordinate = "FacilityNameSceneDescription" Then
            strBody.Append("                   <i>Address:</i> " & Address & " <i>City:</i> " & City & "  " & " <i>Zip:</i> " & Zip & "  " & " <i>USNG:</i> " & localUSNG)
        ElseIf ObtainCoordinate = "CoordinateEntry" Then
            strBody.Append("                    " & "<i>Lat</i>: " & localLat & ", " & "<i>Long</i>: " & localLong & "     ")
        Else
            strBody.Append("                    " & "N/A" & "     ")
        End If

        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If localLat <> 0 And localLong <> 0 Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Coordinates:</b>     ")
            strBody.Append("                " & "<i>Lat</i>: " & localLat & ", " & "<i>Long</i>: " & localLong & "     ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Contact Information</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Contact Information</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        Dim ReportingPartyType As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("ReportingPartyType", "ReportingPartyType", "ReportingPartyTypeID", ReportingPartyTypeID)
        Dim ResponsiblePartyType As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("ResponsiblePartyType", "ResponsiblePartyType", "ResponsiblePartyTypeID", ResponsiblePartyTypeID)
        Dim OnSceneContactType As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("OnSceneContactType", "OnSceneContactType", "OnSceneContactTypeID", OnSceneContactTypeID)

        Dim ReportingPartyTypeInfo As String = ""

        Dim localReportingPartyTypeFirstName As String = ""
        Dim localReportingPartyTypeLastName As String = ""
        Dim localReportingPartyTypeCallBackNumber1 As String = ""
        Dim localReportingPartyTypeCallBackNumber2 As String = ""
        Dim localReportingPartyTypeEmail As String = ""
        Dim localReportingPartyTypeAddress As String = ""
        Dim localReportingPartyTypeCity As String = ""
        Dim localReportingPartyTypeState As String = ""
        Dim localReportingPartyTypeZipcode As String = ""
        Dim localReportingPartyTypeRepresents As String = ""

        If ReportingPartyType = "As Below" Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()

            objCmd = New SqlCommand("spSelectReportingPartyByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localReportingPartyTypeFirstName = HelpFunction.Convertdbnulls(objDR("FirstName"))
                localReportingPartyTypeLastName = HelpFunction.Convertdbnulls(objDR("LastName"))
                localReportingPartyTypeCallBackNumber1 = HelpFunction.Convertdbnulls(objDR("CallBackNumber1"))
                localReportingPartyTypeCallBackNumber2 = HelpFunction.Convertdbnulls(objDR("CallBackNumber2"))
                localReportingPartyTypeEmail = HelpFunction.Convertdbnulls(objDR("Email"))
                localReportingPartyTypeAddress = HelpFunction.Convertdbnulls(objDR("Address"))
                localReportingPartyTypeCity = HelpFunction.Convertdbnulls(objDR("City"))
                localReportingPartyTypeState = HelpFunction.Convertdbnulls(objDR("State"))
                localReportingPartyTypeZipcode = HelpFunction.Convertdbnulls(objDR("Zipcode"))
                localReportingPartyTypeRepresents = HelpFunction.Convertdbnulls(objDR("Represents"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            If localReportingPartyTypeFirstName <> "" Then
                If localReportingPartyTypeFirstName <> "" And localReportingPartyTypeLastName <> "" And localReportingPartyTypeRepresents <> "" Then
                    ReportingPartyTypeInfo = "<i> Name: </i>" & localReportingPartyTypeFirstName & " " & localReportingPartyTypeLastName & ", " & localReportingPartyTypeRepresents
                ElseIf localReportingPartyTypeFirstName <> "" And localReportingPartyTypeLastName <> "" Then
                    ReportingPartyTypeInfo = "<i> Name: </i>" & localReportingPartyTypeFirstName & " " & localReportingPartyTypeLastName
                ElseIf localReportingPartyTypeFirstName <> "" And localReportingPartyTypeRepresents <> "" Then
                    ReportingPartyTypeInfo = "<i> Name: </i>" & localReportingPartyTypeFirstName & ", " & localReportingPartyTypeRepresents
                ElseIf localReportingPartyTypeFirstName <> "" Then
                    ReportingPartyTypeInfo = "<i> Name: </i>" & localReportingPartyTypeFirstName
                End If
            End If

            If localReportingPartyTypeCallBackNumber1 <> "" Then
                ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Call Back Number 1: </i>" & localReportingPartyTypeCallBackNumber1
            End If

            If localReportingPartyTypeCallBackNumber2 <> "" Then
                ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Call Back Number 2: </i>" & localReportingPartyTypeCallBackNumber2
            End If

            If localReportingPartyTypeEmail <> "" Then
                ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Email: </i>" & localReportingPartyTypeEmail
            End If

            If localReportingPartyTypeAddress <> "" And localReportingPartyTypeCity <> "" And localReportingPartyTypeState <> "" And localReportingPartyTypeZipcode <> "" Then
                ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Address: </i>" & localReportingPartyTypeAddress & " " & localReportingPartyTypeCity & " " & localReportingPartyTypeState & ", " & localReportingPartyTypeZipcode
            Else
                ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Address: </i>"

                If localReportingPartyTypeAddress <> "" Then
                    ReportingPartyTypeInfo = ReportingPartyTypeInfo & "  " & localReportingPartyTypeAddress
                End If

                If localReportingPartyTypeCity <> "" Then
                    ReportingPartyTypeInfo = ReportingPartyTypeInfo & "  " & localReportingPartyTypeCity
                End If

                If localReportingPartyTypeState <> "" Then
                    ReportingPartyTypeInfo = ReportingPartyTypeInfo & "  " & localReportingPartyTypeState
                End If

                If localReportingPartyTypeZipcode <> "" Then
                    ReportingPartyTypeInfo = ReportingPartyTypeInfo & "  " & localReportingPartyTypeZipcode
                End If
            End If

            'If localReportingPartyTypeFirstName <> "" Then
            '    ReportingPartyTypeInfo = "<i> First Name: </i>" & localReportingPartyTypeFirstName
            'End If

            'If localReportingPartyTypeLastName <> "" Thenfile:///C:\Inetpub\wwwroot\IncidentRecorder\Worksheets.aspx
            '    ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Last Name: </i>" & localReportingPartyTypeLastName
            'End If

            'If localReportingPartyTypeCity <> "" Then
            '    ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |City: </i>" & localReportingPartyTypeCity
            'End If

            'If localReportingPartyTypeState <> "" Then
            '    ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |State: </i>" & localReportingPartyTypeState
            'End If

            'If localReportingPartyTypeZipcode <> "" Then
            '    ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Zipcode: </i>" & localReportingPartyTypeZipcode
            'End If

            'If localReportingPartyTypeRepresents <> "" Then
            '    ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Represents: </i>" & localReportingPartyTypeRepresents
            'End If
        Else
            ReportingPartyTypeInfo = ReportingPartyType
        End If

        Dim ResponsiblePartyInfo As String = ""

        Dim localResponsiblePartyFirstName As String = ""
        Dim localResponsiblePartyLastName As String = ""
        Dim localResponsiblePartyCallBackNumber1 As String = ""
        Dim localResponsiblePartyCallBackNumber2 As String = ""
        Dim localResponsiblePartyEmail As String = ""
        Dim localResponsiblePartyAddress As String = ""
        Dim localResponsiblePartyCity As String = ""
        Dim localResponsiblePartyState As String = ""
        Dim localResponsiblePartyZipcode As String = ""
        Dim localResponsiblePartyRepresents As String = ""

        If ResponsiblePartyType = "As Below" Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()

            objCmd = New SqlCommand("spSelectResponsiblePartyByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localResponsiblePartyFirstName = HelpFunction.Convertdbnulls(objDR("FirstName"))
                localResponsiblePartyLastName = HelpFunction.Convertdbnulls(objDR("LastName"))
                localResponsiblePartyCallBackNumber1 = HelpFunction.Convertdbnulls(objDR("CallBackNumber1"))
                localResponsiblePartyCallBackNumber2 = HelpFunction.Convertdbnulls(objDR("CallBackNumber2"))
                localResponsiblePartyEmail = HelpFunction.Convertdbnulls(objDR("Email"))
                localResponsiblePartyAddress = HelpFunction.Convertdbnulls(objDR("Address"))
                localResponsiblePartyCity = HelpFunction.Convertdbnulls(objDR("City"))
                localResponsiblePartyState = HelpFunction.Convertdbnulls(objDR("State"))
                localResponsiblePartyZipcode = HelpFunction.Convertdbnulls(objDR("Zipcode"))
                localResponsiblePartyRepresents = HelpFunction.Convertdbnulls(objDR("Represents"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            If localResponsiblePartyFirstName <> "" Then
                If localResponsiblePartyFirstName <> "" And localResponsiblePartyLastName <> "" And localResponsiblePartyRepresents <> "" Then
                    ResponsiblePartyInfo = "<i> Name: </i>" & localResponsiblePartyFirstName & " " & localResponsiblePartyLastName & ", " & localResponsiblePartyRepresents
                ElseIf localResponsiblePartyFirstName <> "" And localResponsiblePartyLastName <> "" Then
                    ResponsiblePartyInfo = "<i> Name: </i>" & localResponsiblePartyFirstName & " " & localResponsiblePartyLastName
                ElseIf localResponsiblePartyFirstName <> "" And localResponsiblePartyRepresents <> "" Then
                    ResponsiblePartyInfo = "<i> Name: </i>" & localResponsiblePartyFirstName & ", " & localResponsiblePartyRepresents
                ElseIf localResponsiblePartyFirstName <> "" Then
                    ResponsiblePartyInfo = "<i> Name: </i>" & localResponsiblePartyFirstName
                End If
            End If

            If localResponsiblePartyCallBackNumber1 <> "" Then
                ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Call Back Number 1: </i>" & localResponsiblePartyCallBackNumber1
            End If

            If localResponsiblePartyCallBackNumber2 <> "" Then
                ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Call Back Number 2: </i>" & localResponsiblePartyCallBackNumber2
            End If

            If localResponsiblePartyEmail <> "" Then
                ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Email: </i>" & localResponsiblePartyEmail
            End If

            If localResponsiblePartyAddress <> "" And localResponsiblePartyCity <> "" And localResponsiblePartyState <> "" And localResponsiblePartyZipcode <> "" Then

                ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Address: </i>" & localResponsiblePartyAddress & " " & localResponsiblePartyCity & " " & localResponsiblePartyState & ", " & localResponsiblePartyZipcode
            Else
                ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Address: </i>"

                If localResponsiblePartyAddress <> "" Then
                    ResponsiblePartyInfo = ResponsiblePartyInfo & "  " & localResponsiblePartyAddress
                End If

                If localResponsiblePartyCity <> "" Then
                    ResponsiblePartyInfo = ResponsiblePartyInfo & "  " & localResponsiblePartyCity
                End If

                If localResponsiblePartyState <> "" Then
                    ResponsiblePartyInfo = ResponsiblePartyInfo & "  " & localResponsiblePartyState
                End If

                If localResponsiblePartyZipcode <> "" Then
                    ResponsiblePartyInfo = ResponsiblePartyInfo & "  " & localResponsiblePartyZipcode
                End If
            End If

            'If localResponsiblePartyFirstName <> "" Then
            '    ResponsiblePartyInfo = "<i> First Name: </i>" & localResponsiblePartyFirstName
            'End If

            'If localResponsiblePartyLastName <> "" Then
            '    ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Last Name: </i>" & localResponsiblePartyLastName
            'End If

            'If localResponsiblePartyCallBackNumber1 <> "" Then
            '    ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Call Back Number 1: </i>" & localResponsiblePartyCallBackNumber1
            'End If

            'If localResponsiblePartyCallBackNumber2 <> "" Then
            '    ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Call Back Number 2: </i>" & localResponsiblePartyCallBackNumber2
            'End If

            'If localResponsiblePartyEmail <> "" Then
            '    ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Email: </i>" & localResponsiblePartyEmail
            'End If

            'If localResponsiblePartyAddress <> "" Then
            '    ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Address: </i>" & localResponsiblePartyAddress
            'End If

            'If localResponsiblePartyCity <> "" Then
            '    ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |City: </i>" & localResponsiblePartyCity
            'End If

            'If localResponsiblePartyState <> "" Then
            '    ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |State: </i>" & localResponsiblePartyState
            'End If

            'If localResponsiblePartyZipcode <> "" Then
            '    ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Zipcode: </i>" & localResponsiblePartyZipcode
            'End If

            'If localResponsiblePartyRepresents <> "" Then
            '    ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Represents: </i>" & localResponsiblePartyRepresents
            'End If
        Else
            ResponsiblePartyInfo = ResponsiblePartyType
        End If

        Dim OnSceneContactInfo As String = ""

        Dim localOnSceneContactFirstName As String = ""
        Dim localOnSceneContactLastName As String = ""
        Dim localOnSceneContactCallBackNumber1 As String = ""
        Dim localOnSceneContactCallBackNumber2 As String = ""
        Dim localOnSceneContactEmail As String = ""
        Dim localOnSceneContactAddress As String = ""
        Dim localOnSceneContactCity As String = ""
        Dim localOnSceneContactState As String = ""
        Dim localOnSceneContactZipcode As String = ""
        Dim localOnSceneContactRepresents As String = ""

        If OnSceneContactType = "As Below" Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()

            objCmd = New SqlCommand("spSelectOnSceneContactByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                localOnSceneContactFirstName = HelpFunction.Convertdbnulls(objDR("FirstName"))
                localOnSceneContactLastName = HelpFunction.Convertdbnulls(objDR("LastName"))
                localOnSceneContactCallBackNumber1 = HelpFunction.Convertdbnulls(objDR("CallBackNumber1"))
                localOnSceneContactCallBackNumber2 = HelpFunction.Convertdbnulls(objDR("CallBackNumber2"))
                localOnSceneContactEmail = HelpFunction.Convertdbnulls(objDR("Email"))
                localOnSceneContactAddress = HelpFunction.Convertdbnulls(objDR("Address"))
                localOnSceneContactCity = HelpFunction.Convertdbnulls(objDR("City"))
                localOnSceneContactState = HelpFunction.Convertdbnulls(objDR("State"))
                localOnSceneContactZipcode = HelpFunction.Convertdbnulls(objDR("Zipcode"))
                localOnSceneContactRepresents = HelpFunction.Convertdbnulls(objDR("Represents"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            If localOnSceneContactFirstName <> "" Then
                If localOnSceneContactFirstName <> "" And localOnSceneContactLastName <> "" And localOnSceneContactRepresents <> "" Then
                    OnSceneContactInfo = "<i> Name: </i>" & localOnSceneContactFirstName & " " & localOnSceneContactLastName & ", " & localOnSceneContactRepresents
                ElseIf localOnSceneContactFirstName <> "" And localOnSceneContactLastName <> "" Then
                    OnSceneContactInfo = "<i> Name: </i>" & localOnSceneContactFirstName & " " & localOnSceneContactLastName
                ElseIf localOnSceneContactFirstName <> "" And localOnSceneContactRepresents <> "" Then
                    OnSceneContactInfo = "<i> Name: </i>" & localOnSceneContactFirstName & ", " & localOnSceneContactRepresents
                ElseIf localOnSceneContactFirstName <> "" Then
                    OnSceneContactInfo = "<i> Name: </i>" & localOnSceneContactFirstName
                End If
            End If

            If localOnSceneContactCallBackNumber1 <> "" Then
                OnSceneContactInfo = OnSceneContactInfo & "<i> |Call Back Number 1: </i>" & localOnSceneContactCallBackNumber1
            End If

            If localOnSceneContactCallBackNumber2 <> "" Then
                OnSceneContactInfo = OnSceneContactInfo & "<i> |Call Back Number 2: </i>" & localOnSceneContactCallBackNumber2
            End If

            If localOnSceneContactEmail <> "" Then
                OnSceneContactInfo = OnSceneContactInfo & "<i> |Email: </i>" & localOnSceneContactEmail
            End If

            If localOnSceneContactAddress <> "" And localOnSceneContactCity <> "" And localOnSceneContactState <> "" And localOnSceneContactZipcode <> "" Then
                OnSceneContactInfo = OnSceneContactInfo & "<i> |Address: </i>" & localOnSceneContactAddress & " " & localOnSceneContactCity & " " & localOnSceneContactState & ", " & localOnSceneContactZipcode
            Else
                OnSceneContactInfo = OnSceneContactInfo & "<i> |Address: </i>"

                If localOnSceneContactAddress <> "" Then
                    OnSceneContactInfo = OnSceneContactInfo & "  " & localOnSceneContactAddress
                End If

                If localOnSceneContactCity <> "" Then
                    OnSceneContactInfo = OnSceneContactInfo & "  " & localOnSceneContactCity
                End If

                If localOnSceneContactState <> "" Then
                    OnSceneContactInfo = OnSceneContactInfo & "  " & localOnSceneContactState
                End If

                If localOnSceneContactZipcode <> "" Then
                    OnSceneContactInfo = OnSceneContactInfo & "  " & localOnSceneContactZipcode
                End If
            End If

            'If localOnSceneContactFirstName <> "" Then
            '    OnSceneContactInfo = "<i> First Name: </i>" & localOnSceneContactFirstName
            'End If

            'If localOnSceneContactLastName <> "" Then
            '    OnSceneContactInfo = OnSceneContactInfo & "<i> |Last Name: </i>" & localOnSceneContactLastName
            'End If

            'If localOnSceneContactCallBackNumber1 <> "" Then
            '    OnSceneContactInfo = OnSceneContactInfo & "<i> |Call Back Number 1: </i>" & localOnSceneContactCallBackNumber1
            'End If

            'If localOnSceneContactCallBackNumber2 <> "" Then
            '    OnSceneContactInfo = OnSceneContactInfo & "<i> |Call Back Number 2: </i>" & localOnSceneContactCallBackNumber2
            'End If

            'If localOnSceneContactEmail <> "" Then
            '    OnSceneContactInfo = OnSceneContactInfo & "<i> |Email: </i>" & localOnSceneContactEmail
            'End If

            'If localOnSceneContactAddress <> "" Then
            '    OnSceneContactInfo = OnSceneContactInfo & "<i> |Address: </i>" & localOnSceneContactAddress
            'End If

            'If localOnSceneContactCity <> "" Then
            '    OnSceneContactInfo = OnSceneContactInfo & "<i> |City: </i>" & localOnSceneContactCity
            'End If

            'If localOnSceneContactState <> "" Then
            '    OnSceneContactInfo = OnSceneContactInfo & "<i> |State: </i>" & localOnSceneContactState
            'End If

            'If localOnSceneContactZipcode <> "" Then
            '    OnSceneContactInfo = OnSceneContactInfo & "<i> |Zipcode: </i>" & localOnSceneContactZipcode
            'End If

            'If localOnSceneContactRepresents <> "" Then
            '    OnSceneContactInfo = OnSceneContactInfo & "<i> |Represents: </i>" & localOnSceneContactRepresents
            'End If
        Else
            OnSceneContactInfo = OnSceneContactType
        End If

        '---------------------------------------------------------------------------------------

        If intShowReortingParty = 0 Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Reporting Party:</b>     ")
            strBody.Append("            " & ReportingPartyTypeInfo & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Responsible Party:</b>     ")
            strBody.Append("            " & ResponsiblePartyInfo & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>On-Scene Contact:</b>     ")
            strBody.Append("            " & OnSceneContactInfo & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Reporting Party:</b>     ")
            strBody.Append("            " & "Protected Information-Please Contact SWO" & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Responsible Party:</b>     ")
            strBody.Append("            " & "Protected Information-Please Contact SWO" & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>On-Scene Contact:</b>     ")
            strBody.Append("            " & "Protected Information-Please Contact SWO" & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If
    End Sub

    'Added/edited by JD --->
    '---------------------------
    Protected Sub GetAttachments()
        Dim Attachment As String = ""
        Dim AttachmentName As String = ""

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()

        objCmd = New SqlCommand("spSelectAttachmentByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

        objDR = objCmd.ExecuteReader

        If objDR.HasRows = True Then
            'Generate attachment(s) section with link(s).
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Attachments</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            Do While objDR.Read()
                Attachment = HelpFunction.Convertdbnulls(objDR("Attachment"))
                AttachmentName = HelpFunction.Convertdbnulls(objDR("AttachmentName"))

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Attachment:</b> ")
                strBody.Append("            <a target='_blank' href='" & "https://apps.floridadisaster.org/" & HttpContext.Current.Application("ApplicationEnvironmentForUpload").ToString & "/Uploads/" & Attachment & "'>" & AttachmentName & "</a>")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            Loop
        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()
    End Sub
    '---------------------------
    'Added/edited by JD <---

    Protected Sub GetWorkSheets()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        DBConStringHelper.PrepareConnection(objConn) 'open the connection
        objCmd = New SqlCommand("[spSelectIncidentIncidentTypeByIncidentID2]", objConn)

        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()

        If objDR.HasRows Then
            'There are records.
            While objDR.Read
                'strBody.Append("<table width='100%' align='center'>")
                'strBody.Append("    <tr>")
                'strBody.Append("        <td align='left'>")
                'strBody.Append("            Incident Type: " & objDR.Item("IncidentType") & "")
                'strBody.Append("        </td>")
                'strBody.Append("    </tr>")
                'strBody.Append("</table>")

                If CStr(objDR.Item("IncidentType")) = "Hazardous Materials" Then
                    GetHazardousMaterials(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Road Closure or DOT Issue" Then
                    GetRoadClosureDOTIssue(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Vehicle" Then
                    GetVehicle(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Aircraft Incident" Then
                    GetAircraft(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Animal or Agricultural" Then
                    GetAnimalAgricultural(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Bomb Threat or Device" Then
                    GetBombThreatDevice(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Civil Event" Then
                    GetCivilDisturbance(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Law Enforcement Activity" Then
                    GetCriminalActivity(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Dam Failure" Then
                    GetDamFailure(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "DEM Incidents" Then
                    GetDemINR(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Drinking Water Facility" Then
                    GetDrinkingWaterFacility(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Environmental Crime" Then
                    GetEnvironmentalCrime(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Fire" Then
                    GetFire(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "General" Then
                    GetGeneral(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Geological Event" Then
                    GetGeologicalEvent(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Kennedy Space Center / Cape Canaveral AFS" Then
                    GetKennedySpaceCenter(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Marine Incident" Then
                    GetMarineIncident(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Migration" Then
                    GetMigration(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Military Activity" Then
                    GetMilitaryActivity(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Nuclear Power Plants" Then
                    GetNPP(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Petroleum Spill" Then
                    GetPetroleumSpill(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Population Protection Actions" Then
                    GetPopProtAction(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Public Health Medical" Then
                    GetPublicHealthMedical(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Rail Incident" Then
                    GetRail(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Search & Rescue" Then
                    GetSearchRescue(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Suspicious Activity" Then
                    GetSecurityThreat(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Utility Disruption or Emergency" Then
                    GetUtilityDisruptionEmergency(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Wastewater or Effluent Release" Then
                    GetWastewater(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Weather Advisories" Then
                    GetWeatherAdvisories(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Weather Reports" Then
                    GetWeatherReports(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                End If
            End While
        End If

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()

        'MarineIncident.
    End Sub

    Protected Sub test()
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        DBConStringHelper.PrepareConnection(objConn) 'open the connection

        objCmd = New SqlCommand("[spSelectLatestUpdateByIncidentID]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()

        If objDR.HasRows Then
            'strBody.Append("<tr>")
            'strBody.Append("<td align='left' width='25%'><font size='5'><b>Date</b></font></td>")
            'strBody.Append("<td align='left' width='75%'><font size='5'><b>Update</b></font></td>")
            'strBody.Append("</tr>")

            While objDR.Read
                'strBody.Append("<tr>")
                'strBody.Append("<td align='left'><font size='5'>" & objDR.Item("UpdateDate") & "</font></td>")
                'strBody.Append("<td align='left'><font size='5'>" & objDR.Item("MostRecentUpdate") & "</font></td>")
                'strBody.Append("</tr>")
                'strBody.Append("<br>")
            End While
        Else
            'There are no records.
            strBody.Append("<tr><td colspan='2' align='center'>&nbsp;</td><tr>")
            strBody.Append("<tr><td colspan='2' align='center'><b>No Records</b></td><tr>")
        End If

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()
    End Sub

    'Individual Worksheets.
    Private Sub GetRoadClosureDOTIssue(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""

        Dim RoadwayNameNumber As String = ""
        Dim At As String = ""
        Dim MileMarker As String = ""
        Dim ExitRamp As String = ""
        Dim CrossStreet1Intersection As String = ""
        Dim CrossStreet2 As String = ""
        Dim DurationOfClosure As String = ""
        Dim DepartmentAgencyDirectedClosure As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectRoadClosureDOTIssueByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            RoadwayNameNumber = HelpFunction.Convertdbnulls(objDR2("RoadwayNameNumber"))
            At = HelpFunction.Convertdbnulls(objDR2("At"))
            MileMarker = HelpFunction.Convertdbnulls(objDR2("MileMarker"))
            ExitRamp = HelpFunction.Convertdbnulls(objDR2("ExitRamp"))
            CrossStreet1Intersection = HelpFunction.Convertdbnulls(objDR2("CrossStreet1Intersection"))
            CrossStreet2 = HelpFunction.Convertdbnulls(objDR2("CrossStreet2"))
            DurationOfClosure = HelpFunction.Convertdbnulls(objDR2("DurationOfClosure"))
            DepartmentAgencyDirectedClosure = HelpFunction.Convertdbnulls(objDR2("DepartmentAgencyDirectedClosure"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Road Closure or DOT Issue</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Road Closure or DOT Issue</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>This situation is:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='650px'>")
        strBody.Append("            <b>Roadway Name and/or number:</b> ")
        strBody.Append("           " & RoadwayNameNumber & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If At <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>At:</b> ")
            strBody.Append("           " & At & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If At = "Mile Marker" And MileMarker <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Mile Marker:</b> ")
                strBody.Append("           " & MileMarker & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If At = "Exit Ramp" And ExitRamp <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Exit Ramp:</b> ")
                strBody.Append("           " & ExitRamp & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If At = "Intersection" And CrossStreet1Intersection <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Cross Street 1 or Intersection:</b> ")
                strBody.Append("           " & CrossStreet1Intersection & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If At = "Between Cross Streets" And CrossStreet2 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Cross Street 1:</b> ")
                strBody.Append("           " & CrossStreet1Intersection & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Cross Street 2:</b> ")
                strBody.Append("           " & CrossStreet2 & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        If DurationOfClosure <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Duration of closure (if known):</b> ")
            strBody.Append("           " & DurationOfClosure & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If DepartmentAgencyDirectedClosure <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>What department/agency directed the closure:</b> ")
            strBody.Append("           " & DepartmentAgencyDirectedClosure & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If
    End Sub

    Private Sub GetHazardousMaterials(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim localTime As String = ""
        Dim localTime2 As String = ""

        Dim SubType As String = ""
        Dim Situation As String = ""

        'Unknown Hazard.
        '---------------------------------------------------------------------------------------
        Dim UHChemicalState As String = ""
        Dim UHSourceContainer As String = ""
        Dim UHTotalSourceContainerVolume As String = ""
        Dim UHChemicalQuantityReleased As String = ""
        Dim UHChemicalRateOfRelease As String = ""
        Dim UHChemicalReleased As String = ""
        '---------------------------------------------------------------------------------------

        'Biological Hazard.
        '---------------------------------------------------------------------------------------
        Dim CommonName As String = ""
        Dim ScientificName As String = ""
        Dim QuantityDescription As String = ""
        Dim ContainerDeviceDescription As String = ""
        Dim BiologicalTotalQuantity As String = ""
        Dim BiologicalQuantityReleased As String = ""
        '---------------------------------------------------------------------------------------

        'Chemical Agent.
        '---------------------------------------------------------------------------------------
        Dim AgentType As String = ""
        Dim AgentName As String = ""
        Dim AgentContainerDeviceDescription As String = ""
        Dim AgentTotalQuantity As String = ""
        Dim AgentQuantityReleased As String = ""
        '---------------------------------------------------------------------------------------

        'Radiological Material.
        '---------------------------------------------------------------------------------------
        Dim RadiationType As String = ""
        Dim IsotopeName As String = ""
        Dim ContainerDeviceInstrumentDescription As String = ""
        Dim RadiationTotalQuantity As String = ""
        Dim DOHBureauNotified As String = ""
        '---------------------------------------------------------------------------------------

        'Toxic Industrial Chemical.
        '---------------------------------------------------------------------------------------
        Dim ChemicalName As String = ""
        Dim IndexName As String = ""
        Dim CASNumber As String = ""
        Dim Section304ReportableQuantity As String = ""
        Dim CERCLAReportableQuantity As String = ""
        Dim ChemicalState As String = ""
        Dim SourceContainer As String = ""
        Dim DiameterPipeline As String = ""
        Dim UnbrokenEndPipeConnectedTo As String = ""
        Dim TotalSourceContainerVolume As String = ""
        Dim QuantityReleased As String = ""
        Dim ChemicalRateOfRelease As String = ""
        Dim ChemicalReleased As String = ""
        Dim CauseOfRelease As String = ""
        Dim ReasonLateReport As String = ""
        Dim StormDrainsAffected As String = ""
        Dim WaterwaysAffected As String = ""
        Dim WaterwaysAffectedText As String = ""
        'Dim CallbackDEPRequested As String = ""
        'Dim CallbackDEPRequestedDDLValue As String = ""
        'Dim Evacuations As String = ""
        Dim MajorRoadwaysClosed As String = ""
        'Dim Injury As String = ""
        'Dim InjuryText As String = ""
        'Dim Fatality As String = ""
        'Dim FatalityText As String = ""

        Dim ChemicalQuantityReleased As String = ""
        Dim TimeReleaseDiscovered As String = ""
        Dim TimeReleaseSecured As String = ""
        '---------------------------------------------------------------------------------------

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectHazardousMaterialsByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            'Unknown Hazard.
            '---------------------------------------------------------------------------------------
            UHChemicalState = HelpFunction.Convertdbnulls(objDR2("UHChemicalState"))
            UHSourceContainer = HelpFunction.Convertdbnulls(objDR2("UHSourceContainer"))
            UHTotalSourceContainerVolume = HelpFunction.Convertdbnulls(objDR2("UHTotalSourceContainerVolume"))
            UHChemicalQuantityReleased = HelpFunction.Convertdbnulls(objDR2("UHChemicalQuantityReleased"))
            UHChemicalRateOfRelease = HelpFunction.Convertdbnulls(objDR2("UHChemicalRateOfRelease"))
            UHChemicalReleased = HelpFunction.Convertdbnulls(objDR2("UHChemicalReleased"))
            '---------------------------------------------------------------------------------------

            'Biological Hazard.
            '---------------------------------------------------------------------------------------
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            CommonName = HelpFunction.Convertdbnulls(objDR2("CommonName"))
            ScientificName = HelpFunction.Convertdbnulls(objDR2("ScientificName"))
            QuantityDescription = HelpFunction.Convertdbnulls(objDR2("QuantityDescription"))
            ContainerDeviceDescription = HelpFunction.Convertdbnulls(objDR2("ContainerDeviceDescription"))
            BiologicalTotalQuantity = HelpFunction.Convertdbnulls(objDR2("BiologicalTotalQuantity"))
            BiologicalQuantityReleased = HelpFunction.Convertdbnulls(objDR2("BiologicalQuantityReleased"))
            '---------------------------------------------------------------------------------------

            'Chemical Agent.
            '---------------------------------------------------------------------------------------
            AgentType = HelpFunction.Convertdbnulls(objDR2("AgentType"))
            AgentName = HelpFunction.Convertdbnulls(objDR2("AgentName"))
            AgentContainerDeviceDescription = HelpFunction.Convertdbnulls(objDR2("AgentContainerDeviceDescription"))
            AgentTotalQuantity = HelpFunction.Convertdbnulls(objDR2("AgentTotalQuantity"))
            AgentQuantityReleased = HelpFunction.Convertdbnulls(objDR2("AgentQuantityReleased"))
            '---------------------------------------------------------------------------------------

            'Radiological Material.
            '---------------------------------------------------------------------------------------
            RadiationType = HelpFunction.Convertdbnulls(objDR2("RadiationType"))
            IsotopeName = HelpFunction.Convertdbnulls(objDR2("IsotopeName"))
            ContainerDeviceInstrumentDescription = HelpFunction.Convertdbnulls(objDR2("ContainerDeviceInstrumentDescription"))
            RadiationTotalQuantity = HelpFunction.Convertdbnulls(objDR2("RadiationTotalQuantity"))
            DOHBureauNotified = HelpFunction.Convertdbnulls(objDR2("DOHBureauNotified"))
            '---------------------------------------------------------------------------------------

            'Toxic Industrial Chemical.
            '---------------------------------------------------------------------------------------
            ChemicalName = HelpFunction.Convertdbnulls(objDR2("ChemicalName"))
            IndexName = HelpFunction.Convertdbnulls(objDR2("IndexName"))
            CASNumber = HelpFunction.Convertdbnulls(objDR2("CASNumber"))
            Section304ReportableQuantity = HelpFunction.Convertdbnulls(objDR2("Section304ReportableQuantity"))
            CERCLAReportableQuantity = HelpFunction.Convertdbnulls(objDR2("CERCLAReportableQuantity"))
            ChemicalState = HelpFunction.Convertdbnulls(objDR2("ChemicalState"))
            SourceContainer = HelpFunction.Convertdbnulls(objDR2("SourceContainer"))
            DiameterPipeline = HelpFunction.Convertdbnulls(objDR2("DiameterPipeline"))
            UnbrokenEndPipeConnectedTo = HelpFunction.Convertdbnulls(objDR2("UnbrokenEndPipeConnectedTo"))
            TotalSourceContainerVolume = HelpFunction.Convertdbnulls(objDR2("TotalSourceContainerVolume"))
            QuantityReleased = HelpFunction.Convertdbnulls(objDR2("ChemicalQuantityReleased"))
            ChemicalRateOfRelease = HelpFunction.Convertdbnulls(objDR2("ChemicalRateOfRelease"))
            ChemicalReleased = HelpFunction.Convertdbnulls(objDR2("ChemicalReleased"))
            CauseOfRelease = HelpFunction.Convertdbnulls(objDR2("CauseOfRelease"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR2("TimeReleaseDiscovered")))
            localTime2 = CStr(HelpFunction.Convertdbnulls(objDR2("TimeReleaseSecured")))
            ReasonLateReport = HelpFunction.Convertdbnulls(objDR2("ReasonLateReport"))
            StormDrainsAffected = HelpFunction.Convertdbnulls(objDR2("StormDrainsAffected"))
            WaterwaysAffected = HelpFunction.Convertdbnulls(objDR2("WaterwaysAffected"))
            WaterwaysAffectedText = HelpFunction.Convertdbnulls(objDR2("WaterwaysAffectedText"))
            'CallbackDEPRequested = HelpFunction.Convertdbnulls(objDR2("CallbackDEPRequested"))
            'CallbackDEPRequestedDDLValue = HelpFunction.Convertdbnulls(objDR2("CallbackDEPRequestedDDLValue"))
            'Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            MajorRoadwaysClosed = HelpFunction.Convertdbnulls(objDR2("MajorRoadwaysClosed"))
            'Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            'InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            'Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            'FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            ChemicalQuantityReleased = HelpFunction.Convertdbnulls(objDR2("ChemicalQuantityReleased"))
            '---------------------------------------------------------------------------------------

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        TimeReleaseDiscovered = Left(localTime, 2) & ":" & Right(localTime, 2)
        TimeReleaseSecured = Left(localTime2, 2) & ":" & Right(localTime2, 2)

        '---------------------------------------------------------------------------------------

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Hazardous Materials</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Hazardous Materials</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "Unknown Hazard" Then
            If UHChemicalState <> "" And UHChemicalState <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Chemical State:</b> ")
                strBody.Append("           " & UHChemicalState & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If UHSourceContainer <> "" And UHSourceContainer <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Source / Container:</b> ")
                strBody.Append("           " & UHSourceContainer & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If UHTotalSourceContainerVolume <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Total source/container volume:</b> ")
                strBody.Append("           " & UHTotalSourceContainerVolume & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If UHChemicalQuantityReleased <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Quantity released:</b> ")
                strBody.Append("           " & UHChemicalQuantityReleased & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If UHChemicalRateOfRelease <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Rate of release:</b> ")
                strBody.Append("           " & UHChemicalRateOfRelease & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If UHChemicalReleased <> "" And UHChemicalReleased <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Released:</b> ")
                strBody.Append("           " & UHChemicalReleased & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Biological Hazard" Then
            If CommonName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Common Name:</b> ")
                strBody.Append("           " & CommonName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ScientificName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Scientific Name:</b> ")
                strBody.Append("           " & ScientificName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If QuantityDescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Quantity Description:</b> ")
                strBody.Append("           " & QuantityDescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ContainerDeviceDescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Container or device description:</b> ")
                strBody.Append("           " & ContainerDeviceDescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If BiologicalTotalQuantity <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Total quantity:</b> ")
                strBody.Append("           " & BiologicalTotalQuantity & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If BiologicalQuantityReleased <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Quantity released:</b> ")
                strBody.Append("           " & BiologicalQuantityReleased & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Chemical Agent" Then
            If AgentType <> "" And AgentType <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Type of Agent:</b> ")
                strBody.Append("           " & AgentType & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If AgentName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Agent name:</b> ")
                strBody.Append("           " & AgentName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If AgentContainerDeviceDescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Container or device description:</b> ")
                strBody.Append("           " & AgentContainerDeviceDescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If AgentTotalQuantity <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Total quantity:</b> ")
                strBody.Append("           " & AgentTotalQuantity & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If AgentQuantityReleased <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Quantity released:</b> ")
                strBody.Append("           " & AgentQuantityReleased & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Radiological Material" Then
            If RadiationType <> "" And RadiationType <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Radiation Type:</b> ")
                strBody.Append("           " & RadiationType & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If IsotopeName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Isotope name:</b> ")
                strBody.Append("           " & IsotopeName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ContainerDeviceInstrumentDescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Container or device description:</b> ")
                strBody.Append("           " & ContainerDeviceInstrumentDescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If RadiationTotalQuantity <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Total quantity:</b> ")
                strBody.Append("           " & RadiationTotalQuantity & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If DOHBureauNotified <> "" And DOHBureauNotified <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Local or regional assistance requested:</b> ")
                strBody.Append("           " & DOHBureauNotified & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Toxic Industrial Chemical" Then
            If ChemicalName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Chemical Name:</b> ")
                strBody.Append("           " & ChemicalName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If IndexName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Index Name:</b> ")
                strBody.Append("           " & IndexName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CASNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>CAS Number:</b> ")
                strBody.Append("           " & CASNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Section304ReportableQuantity <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Section 304 Reportable Quantity:</b> ")
                strBody.Append("           " & Section304ReportableQuantity & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CERCLAReportableQuantity <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>CERCLA Reportable Quantity:</b> ")
                strBody.Append("           " & CERCLAReportableQuantity & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ChemicalState <> "" And ChemicalState <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Chemical State:</b> ")
                strBody.Append("           " & ChemicalState & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SourceContainer <> "" And SourceContainer <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Source / Container:</b> ")
                strBody.Append("           " & SourceContainer & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SourceContainer = "Aboveground Pipeline" Then

                If DiameterPipeline <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Diameter of the Pipeline:</b> ")
                    strBody.Append("           " & DiameterPipeline & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If UnbrokenEndPipeConnectedTo <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Unbroken end of the pipe connected to:</b> ")
                    strBody.Append("           " & UnbrokenEndPipeConnectedTo & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

            End If

            If SourceContainer = "Underground Pipeline" Then
                If DiameterPipeline <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Diameter of the Pipeline:</b> ")
                    strBody.Append("           " & DiameterPipeline & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If UnbrokenEndPipeConnectedTo <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Unbroken end of the pipe connected to:</b> ")
                    strBody.Append("           " & UnbrokenEndPipeConnectedTo & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If TotalSourceContainerVolume <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Total source/container volume:</b> ")
                strBody.Append("           " & TotalSourceContainerVolume & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If QuantityReleased <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Quantity released:</b> ")
                strBody.Append("           " & QuantityReleased & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ChemicalRateOfRelease <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Rate of release:</b> ")
                strBody.Append("           " & ChemicalRateOfRelease & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ChemicalReleased <> "" And ChemicalReleased <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Released:</b> ")
                strBody.Append("           " & ChemicalReleased & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CauseOfRelease <> "" And CauseOfRelease <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Cause of release:</b> ")
                strBody.Append("           " & CauseOfRelease & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TimeReleaseDiscovered <> "" And TimeReleaseDiscovered <> ":" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Time the release was discovered:</b> ")
                strBody.Append("           " & TimeReleaseDiscovered & " ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TimeReleaseSecured <> "" And TimeReleaseSecured <> ":" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Time the release was secured:</b> ")
                strBody.Append("           " & TimeReleaseSecured & " ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ReasonLateReport <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Reason for late report:</b> ")
                strBody.Append("           " & ReasonLateReport & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If StormDrainsAffected <> "" And StormDrainsAffected <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Storm drains affected:</b> ")
                strBody.Append("           " & StormDrainsAffected & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WaterwaysAffected <> "" And WaterwaysAffected <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Waterways affected:</b> ")
                strBody.Append("           " & WaterwaysAffected & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WaterwaysAffected = "Yes" Then
                If WaterwaysAffectedText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Name(s) of waterways:</b> ")
                    strBody.Append("           " & WaterwaysAffectedText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            'If CallbackDEPRequested <> "" And CallbackDEPRequested <> "Select an Option" Then
            '    strBody.Append("<table>")
            '    strBody.Append("    <tr>")
            '    strBody.Append("        <td align='left'width='650px'>")
            '    strBody.Append("            <b>Is a callback from DEP requested?:</b> ")
            '    strBody.Append("           " & CallbackDEPRequested & "  ")
            '    strBody.Append("        </td>")
            '    strBody.Append("    </tr>")
            '    strBody.Append("</table>")
            'End If

            'If CallbackDEPRequested = "Yes" Then
            '    If CallbackDEPRequestedDDLValue <> "" And CallbackDEPRequestedDDLValue <> "Select an Option" Then
            '        strBody.Append("<table>")
            '        strBody.Append("    <tr>")
            '        strBody.Append("        <td align='left'width='650px'>")
            '        strBody.Append("            <b>DEP Contact:</b> ")
            '        strBody.Append("           " & CallbackDEPRequestedDDLValue & "  ")
            '        strBody.Append("        </td>")
            '        strBody.Append("    </tr>")
            '        strBody.Append("</table>")
            '    End If
            'End If
        End If

        'If Evacuations <> "" And Evacuations <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Evacuations</b> ")
        '    strBody.Append("           " & Evacuations & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        If MajorRoadwaysClosed <> "" And MajorRoadwaysClosed <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Major roadways closed:</b> ")
            strBody.Append("           " & MajorRoadwaysClosed & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'If Injury <> "" And Injury <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Injuries:</b> ")
        '    strBody.Append("           " & Injury & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Injury = "Yes" Then
        '    If InjuryText <> "" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and Severity of Injuries:</b> ")
        '        strBody.Append("           " & InjuryText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If

        'If Fatality <> "" And Fatality <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Fatalities:</b> ")
        '    strBody.Append("           " & Fatality & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Fatality = "Yes" Then
        '    If FatalityText <> "" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and location of fatalities:</b> ")
        '        strBody.Append("           " & FatalityText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If

        'strBody.Append("<table width='650px' align='left'>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='650px'>")
        'strBody.Append("            <b>Severity:</b> ")
        'strBody.Append("           " & localSeverity & "  ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")
    End Sub

    Private Sub GetVehicle(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim VehiclesInvolvedNumber As String = ""
        Dim VehicleType As String = ""
        Dim PeopleInvolvedNumber As String = ""
        Dim CommercialCarrierOwnedOperatedBy As String = ""
        Dim IncidentCause As String = ""
        Dim Fire As String = ""
        'Dim Injury As String = ""
        'Dim InjuryText As String = ""
        'Dim Fatality As String = ""
        'Dim FatalityText As String = ""
        Dim HazMatOnBoard As String = ""
        Dim FuelPetroleumSpills As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectVehicleByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            VehiclesInvolvedNumber = HelpFunction.Convertdbnulls(objDR2("VehiclesInvolvedNumber"))
            VehicleType = HelpFunction.Convertdbnulls(objDR2("VehicleType"))
            PeopleInvolvedNumber = HelpFunction.Convertdbnulls(objDR2("PeopleInvolvedNumber"))
            CommercialCarrierOwnedOperatedBy = HelpFunction.Convertdbnulls(objDR2("CommercialCarrierOwnedOperatedBy"))
            IncidentCause = HelpFunction.Convertdbnulls(objDR2("IncidentCause"))
            Fire = HelpFunction.Convertdbnulls(objDR2("Fire"))
            'Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            'InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            'Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            'FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            HazMatOnBoard = HelpFunction.Convertdbnulls(objDR2("HazMatOnBoard"))
            FuelPetroleumSpills = HelpFunction.Convertdbnulls(objDR2("FuelPetroleumSpills"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Vehicle</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Vehicle</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If VehiclesInvolvedNumber <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Number of vehicles involved:</b> ")
            strBody.Append("           " & VehiclesInvolvedNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If VehicleType <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Type(s) of vehicles:</b> ")
            strBody.Append("           " & VehicleType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If PeopleInvolvedNumber <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Number of people involved:</b> ")
            strBody.Append("           " & PeopleInvolvedNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If CommercialCarrierOwnedOperatedBy <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Owned/Operated By:</b> ")
            strBody.Append("           " & CommercialCarrierOwnedOperatedBy & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If IncidentCause <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Cause of incident:</b> ")
            strBody.Append("           " & IncidentCause & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Fire <> "" And Fire <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Fire:</b> ")
            strBody.Append("           " & Fire & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'If Injury <> "" And Injury <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Injuries:</b> ")
        '    strBody.Append("           " & Injury & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Injury = "Yes" Then
        '    If InjuryText <> "" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and Severity of Injuries:</b> ")
        '        strBody.Append("           " & InjuryText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If

        'If Fatality <> "" And Fatality <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Fatalities:</b> ")
        '    strBody.Append("           " & Fatality & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If FatalityText <> "" Then
        '    If Fatality = "Yes" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and location of fatalities:</b> ")
        '        strBody.Append("           " & FatalityText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If

        If HazMatOnBoard <> "" And HazMatOnBoard <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Hazardous materials onboard:</b> ")
            strBody.Append("           " & HazMatOnBoard & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If FuelPetroleumSpills <> "" And FuelPetroleumSpills <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Fuel or Petroleum Spills:</b> ")
            strBody.Append("           " & FuelPetroleumSpills & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If
    End Sub

    Private Sub GetAircraft(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""

        Dim AircraftType As String = ""
        Dim MakeModel As String = ""
        Dim TailNumber As String = ""
        Dim OwnedOperatedBy As String = ""
        Dim CauseOfIncident As String = ""
        Dim NumberPeopleOnboard As String = ""
        Dim Fire As String = ""
        'Dim Injury As String = ""
        'Dim InjuryText As String = ""
        'Dim Fatality As String = ""
        'Dim FatalityText As String = ""
        Dim StructuresRoadwaysInvolved As String = ""
        Dim StructuresRoadwaysInvolvedText As String = ""
        Dim HazMatOnboard As String = ""
        Dim FuelPetroleumSpills As String = ""
        'Dim Evacuations As String = ""
        Dim DepartmentAgencyResponding As String = ""
        Dim DepartmentAgencyNotified As String = ""

        'Response.Write(strIncidentID)
        'Response.Write("<br>")
        'Response.Write(strIncidentIncidentTypeID)
        'Response.Write("<br>")

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectAircraftIncidentByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            AircraftType = HelpFunction.Convertdbnulls(objDR2("AircraftType"))
            MakeModel = HelpFunction.Convertdbnulls(objDR2("MakeModel"))
            TailNumber = HelpFunction.Convertdbnulls(objDR2("TailNumber"))
            OwnedOperatedBy = HelpFunction.Convertdbnulls(objDR2("OwnedOperatedBy"))
            CauseOfIncident = HelpFunction.Convertdbnulls(objDR2("CauseOfIncident"))
            NumberPeopleOnboard = HelpFunction.Convertdbnulls(objDR2("NumberPeopleOnboard"))
            Fire = HelpFunction.Convertdbnulls(objDR2("Fire"))
            'Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            'InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            'Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            'FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            StructuresRoadwaysInvolved = HelpFunction.Convertdbnulls(objDR2("StructuresRoadwaysInvolved"))
            StructuresRoadwaysInvolvedText = HelpFunction.Convertdbnulls(objDR2("StructuresRoadwaysInvolvedText"))
            HazMatOnboard = HelpFunction.Convertdbnulls(objDR2("HazMatOnboard"))
            FuelPetroleumSpills = HelpFunction.Convertdbnulls(objDR2("FuelPetroleumSpills"))
            'Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            DepartmentAgencyResponding = HelpFunction.Convertdbnulls(objDR2("DepartmentAgencyResponding"))
            DepartmentAgencyNotified = HelpFunction.Convertdbnulls(objDR2("DepartmentAgencyNotified"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            If gStrReportFormat = "HTML" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
                strBody.Append("            <b>Aircraft Incident</b>")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            Else
                strBody.Append("<table width='100%'cellspacing='0' border='0'>")
                strBody.Append("    <tr>")
                strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
                strBody.Append("            <b>Aircraft Incident</b>")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>This situation is:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Aircraft Type:</b> ")
            strBody.Append("           " & AircraftType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If MakeModel <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Make/Model:</b> ")
                strBody.Append("           " & MakeModel & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TailNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Tail Number:</b> ")
                strBody.Append("           " & TailNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If OwnedOperatedBy <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Owned/Operated By:</b> ")
                strBody.Append("           " & OwnedOperatedBy & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Cause of Incident:</b> ")
            strBody.Append("           " & CauseOfIncident & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Aircraft Fire:</b> ")
            strBody.Append("           " & Fire & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If NumberPeopleOnboard <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Number of People Onboard:</b> ")
                strBody.Append("           " & NumberPeopleOnboard & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            'strBody.Append("<table>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td align='left'width='650px'>")
            'strBody.Append("            <b>Injuries:</b> ")
            'strBody.Append("           " & Injury & "  ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")

            'If Injury = "Yes" And InjuryText <> "" Then
            '    strBody.Append("<table>")
            '    strBody.Append("    <tr>")
            '    strBody.Append("        <td align='left'width='650px'>")
            '    strBody.Append("            <b>Number and Severity of Injuries:</b> ")
            '    strBody.Append("           " & InjuryText & "  ")
            '    strBody.Append("        </td>")
            '    strBody.Append("    </tr>")
            '    strBody.Append("</table>")
            'End If

            'strBody.Append("<table>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td align='left'width='650px'>")
            'strBody.Append("            <b>Fatalities:</b> ")
            'strBody.Append("           " & Fatality & "  ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")

            'If Fatality = "Yes" And FatalityText <> "" Then
            '    strBody.Append("<table>")
            '    strBody.Append("    <tr>")
            '    strBody.Append("        <td align='left'width='650px'>")
            '    strBody.Append("            <b>Number and location (aircraft or ground):</b> ")
            '    strBody.Append("           " & FatalityText & "  ")
            '    strBody.Append("        </td>")
            '    strBody.Append("    </tr>")
            '    strBody.Append("</table>")
            'End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Other structures or roadways involved:</b> ")
            strBody.Append("           " & StructuresRoadwaysInvolved & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If StructuresRoadwaysInvolved = "Yes" And StructuresRoadwaysInvolvedText <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Description:</b> ")
                strBody.Append("           " & StructuresRoadwaysInvolvedText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Hazardous materials onboard:</b> ")
            strBody.Append("           " & HazMatOnboard & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Fuel or Petroleum Spills:</b> ")
            strBody.Append("           " & FuelPetroleumSpills & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            'strBody.Append("<table>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td align='left'width='650px'>")
            'strBody.Append("            <b>Evacuations:</b> ")
            'strBody.Append("           " & Evacuations & "  ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")

            'strBody.Append("<table width='100%' align='center'>")
            'strBody.Append("<tr>")
            'strBody.Append("<td width='50%' align='left'> What departments/agencies are responding? " & DepartmentAgencyResponding & "</font></td>")
            'strBody.Append("<td width='50%' align='left'> What departments/agencies have been notified? " & DepartmentAgencyNotified & "</font></td>")
            'strBody.Append("</tr>")
            'strBody.Append("</table>")
        End If
    End Sub

    Private Sub GetAnimalAgricultural(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim SeverityLevel As String = ""
        Dim AnimalAffected As String = ""
        Dim AnimalDiseaseType As String = ""
        Dim AnimalInfected As String = ""
        Dim AnimalTestExaminations As String = ""
        Dim AnimalsDeceased As String = ""
        Dim AnimalQuarantine As String = ""
        Dim AnimalQuarantineText As String = ""
        Dim AnimalHumansAffected As String = ""
        Dim AnimalHumansAffectedText As String = ""
        Dim AnimalHumanFatalities As String = ""
        Dim AnimalHumanFatalitiesText As String = ""
        Dim ADCFcropsAffected As String = ""
        Dim ADCFdiseaseType As String = ""
        Dim ADCFacresAffected As String = ""
        Dim FSCtypeBrand As String = ""
        Dim FSCmanufacturedPacked As String = ""
        Dim FSCaffectedLotNumber As String = ""
        Dim FSCaffectedDateRange As String = ""
        Dim FSCrecallIssued As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectAnimalAgriculturalByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            SeverityLevel = HelpFunction.Convertdbnulls(objDR2("SeverityLevel"))
            AnimalAffected = HelpFunction.Convertdbnulls(objDR2("AnimalAffected"))
            AnimalDiseaseType = HelpFunction.Convertdbnulls(objDR2("AnimalDiseaseType"))
            AnimalInfected = HelpFunction.Convertdbnulls(objDR2("AnimalInfected"))
            AnimalTestExaminations = HelpFunction.Convertdbnulls(objDR2("AnimalTestExaminations"))
            AnimalsDeceased = HelpFunction.Convertdbnulls(objDR2("AnimalsDeceased"))
            AnimalQuarantine = HelpFunction.Convertdbnulls(objDR2("AnimalQuarantine"))
            AnimalQuarantineText = HelpFunction.Convertdbnulls(objDR2("AnimalQuarantineText"))
            AnimalHumansAffected = HelpFunction.Convertdbnulls(objDR2("AnimalHumansAffected"))
            AnimalHumansAffectedText = HelpFunction.Convertdbnulls(objDR2("AnimalHumansAffectedText"))
            AnimalHumanFatalities = HelpFunction.Convertdbnulls(objDR2("AnimalHumanFatalities"))
            AnimalHumanFatalitiesText = HelpFunction.Convertdbnulls(objDR2("AnimalHumanFatalitiesText"))
            ADCFcropsAffected = HelpFunction.Convertdbnulls(objDR2("ADCFcropsAffected"))
            ADCFdiseaseType = HelpFunction.Convertdbnulls(objDR2("ADCFdiseaseType"))
            ADCFacresAffected = HelpFunction.Convertdbnulls(objDR2("ADCFacresAffected"))
            FSCtypeBrand = HelpFunction.Convertdbnulls(objDR2("FSCtypeBrand"))
            FSCmanufacturedPacked = HelpFunction.Convertdbnulls(objDR2("FSCmanufacturedPacked"))
            FSCaffectedLotNumber = HelpFunction.Convertdbnulls(objDR2("FSCaffectedLotNumber"))
            FSCaffectedDateRange = HelpFunction.Convertdbnulls(objDR2("FSCaffectedDateRange"))
            FSCrecallIssued = HelpFunction.Convertdbnulls(objDR2("FSCrecallIssued"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Animal or Agricultural</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Animal or Agricultural</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SeverityLevel <> "" And SeverityLevel <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Severity Level:</b> ")
            strBody.Append("           " & SeverityLevel & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "Animal Issue" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Animal(s) that are affected:</b> ")
            strBody.Append("           " & AnimalAffected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If AnimalDiseaseType <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Disease Type:</b> ")
                strBody.Append("           " & AnimalDiseaseType & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If AnimalInfected <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Number of animals infected:</b> ")
                strBody.Append("           " & AnimalInfected & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If AnimalTestExaminations <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Number of animals deceased:</b> ")
                strBody.Append("           " & AnimalTestExaminations & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If AnimalsDeceased <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Tests or examinations planned or occurring:</b> ")
                strBody.Append("           " & AnimalsDeceased & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Quarantine in effect:</b> ")
            strBody.Append("           " & AnimalQuarantine & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If AnimalQuarantine = "Yes" And AnimalQuarantineText <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Area Description:</b> ")
                strBody.Append("           " & AnimalQuarantineText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Humans affected:</b> ")
            strBody.Append("           " & AnimalHumansAffected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If AnimalHumansAffected = "Yes" And AnimalHumansAffectedText <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Number and Severity of Illness:</b> ")
                strBody.Append("           " & AnimalHumansAffectedText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Human Fatalities:</b> ")
            strBody.Append("           " & AnimalHumanFatalities & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If AnimalHumanFatalities = "Yes" And AnimalHumanFatalitiesText <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Number and Information:</b> ")
                strBody.Append("           " & AnimalHumanFatalitiesText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Agriculture Issue" Or SubType = "Crop Issue" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Crop(s) affected:</b> ")
            strBody.Append("           " & ADCFcropsAffected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If ADCFdiseaseType <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Disease Type:</b> ")
                strBody.Append("           " & ADCFdiseaseType & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Number of acres affected:</b> ")
            strBody.Append("           " & ADCFacresAffected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        ElseIf SubType = "Food Supply Issue" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Food type / brand:</b> ")
            strBody.Append("           " & FSCtypeBrand & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Manufactured/packed:</b> ")
            strBody.Append("           " & FSCmanufacturedPacked & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If FSCaffectedLotNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Affected lot number(s):</b> ")
                strBody.Append("           " & FSCaffectedLotNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If FSCaffectedDateRange <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Affected date range:</b> ")
                strBody.Append("           " & FSCaffectedDateRange & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If FSCrecallIssued <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Recall been issued:</b> ")
                strBody.Append("           " & FSCrecallIssued & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If
    End Sub

    Private Sub GetBombThreatDevice(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim HowReceivedWhoFound As String = ""
        Dim ExactWordingThreat As String = ""
        Dim Description As String = ""
        'Dim Evacuations As String = ""
        Dim MajorRoadwaysClosed As String = ""
        Dim DepartmentAgencyResponding As String = ""
        Dim DepartmentAgencyNotified As String = ""
        'Dim Fatality As String = ""
        'Dim FatalityText As String = ""
        Dim SearchBeingConducted As String = ""
        Dim DepartmentAgencySearch As String = ""
        'Dim Injury As String = ""
        'Dim InjuryText As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectBombThreatDeviceByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            HowReceivedWhoFound = HelpFunction.Convertdbnulls(objDR2("HowReceivedWhoFound"))
            ExactWordingThreat = HelpFunction.Convertdbnulls(objDR2("ExactWordingThreat"))
            Description = HelpFunction.Convertdbnulls(objDR2("Description"))
            'Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            MajorRoadwaysClosed = HelpFunction.Convertdbnulls(objDR2("MajorRoadwaysClosed"))
            DepartmentAgencyResponding = HelpFunction.Convertdbnulls(objDR2("DepartmentAgencyResponding"))
            DepartmentAgencyNotified = HelpFunction.Convertdbnulls(objDR2("DepartmentAgencyNotified"))
            'Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            'FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            SearchBeingConducted = HelpFunction.Convertdbnulls(objDR2("SearchBeingConducted"))
            DepartmentAgencySearch = HelpFunction.Convertdbnulls(objDR2("DepartmentAgencySearch"))
            'Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            'InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Bomb Threat or Device</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Bomb Threat or Device</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "Bomb or Device Explosion" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>How threat was received/person who found device:</b> ")
            strBody.Append("           " & HowReceivedWhoFound & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If ExactWordingThreat <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Exact wording of threat:</b> ")
                strBody.Append("           " & ExactWordingThreat & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description of the bomb or device:</b> ")
            strBody.Append("           " & Description & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            'strBody.Append("<table>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td align='left'width='650px'>")
            'strBody.Append("            <b>Evacuations:</b> ")
            'strBody.Append("           " & Evacuations & "  ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Major roadways closed:</b> ")
            strBody.Append("           " & MajorRoadwaysClosed & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            'strBody.Append("<table>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td align='left'width='650px'>")
            'strBody.Append("            <b>Injuries:</b> ")
            'strBody.Append("           " & Injury & "  ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")

            'If Injury = "Yes" And InjuryText <> "" Then
            '    strBody.Append("<table>")
            '    strBody.Append("    <tr>")
            '    strBody.Append("        <td align='left'width='650px'>")
            '    strBody.Append("            <b>Number and Severity of Injuries:</b> ")
            '    strBody.Append("           " & InjuryText & "  ")
            '    strBody.Append("        </td>")
            '    strBody.Append("    </tr>")
            '    strBody.Append("</table>")
            'End If

            'strBody.Append("<table>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td align='left'width='650px'>")
            'strBody.Append("            <b>Fatalities:</b> ")
            'strBody.Append("           " & Fatality & "  ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")

            'If Fatality = "Yes" And FatalityText <> "" Then
            '    strBody.Append("<table>")
            '    strBody.Append("    <tr>")
            '    strBody.Append("        <td align='left'width='650px'>")
            '    strBody.Append("            <b>Number and location:</b> ")
            '    strBody.Append("           " & FatalityText & "  ")
            '    strBody.Append("        </td>")
            '    strBody.Append("    </tr>")
            '    strBody.Append("</table>")
            'End If
        Else
            If SubType = "Unconfirmed Threat" Or SubType = "Unfounded Threat" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>How threat was received/person who found device:</b> ")
                strBody.Append("           " & HowReceivedWhoFound & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                If ExactWordingThreat <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Exact wording of threat:</b> ")
                    strBody.Append("           " & ExactWordingThreat & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Description of the bomb or device:</b> ")
                strBody.Append("           " & Description & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                'strBody.Append("<table>")
                'strBody.Append("    <tr>")
                'strBody.Append("        <td align='left'width='650px'>")
                'strBody.Append("            <b>Evacuations:</b> ")
                'strBody.Append("           " & Evacuations & "  ")
                'strBody.Append("        </td>")
                'strBody.Append("    </tr>")
                'strBody.Append("</table>")
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Major roadways closed:</b> ")
                strBody.Append("           " & MajorRoadwaysClosed & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Search being conducted:</b> ")
                strBody.Append("           " & SearchBeingConducted & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                If SearchBeingConducted = "Yes" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Department(s)/Agencie(s) conducting search:</b> ")
                    strBody.Append("           " & DepartmentAgencySearch & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            Else
                If HowReceivedWhoFound <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>How threat was received/person who found device:</b> ")
                    strBody.Append("           " & HowReceivedWhoFound & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If ExactWordingThreat <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Exact wording of threat:</b> ")
                    strBody.Append("           " & ExactWordingThreat & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If Description <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Description of the bomb or device:</b> ")
                    strBody.Append("           " & Description & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                'If Evacuations <> "" Then
                '    strBody.Append("<table>")
                '    strBody.Append("    <tr>")
                '    strBody.Append("        <td align='left'width='650px'>")
                '    strBody.Append("            <b>Evacuations:</b> ")
                '    strBody.Append("           " & Evacuations & "  ")
                '    strBody.Append("        </td>")
                '    strBody.Append("    </tr>")
                '    strBody.Append("</table>")
                'End If

                If MajorRoadwaysClosed <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Major roadways closed:</b> ")
                    strBody.Append("           " & MajorRoadwaysClosed & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If
        End If
    End Sub

    Private Sub GetCivilDisturbance(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim Cause As String = ""
        Dim GroupOrgResponsible As String = ""
        Dim PeopleParticipatingNum As String = ""
        Dim ConfinedLocation As String = ""
        Dim ConfinedLocationOther As String = ""
        Dim LocationAreas As String = ""
        Dim ConfinedLocationMemoText As String = ""
        Dim AgencyCoordinatingResponse As String = ""
        Dim DepartmentAgencyResponding As String = ""
        'Dim Evacuations As String = ""
        Dim MajorRoadwaysClosed As String = ""
        'Dim Injury As String = ""
        'Dim InjuryText As String = ""
        'Dim Fatality As String = ""
        'Dim FatalityText As String = ""
        Dim RegionalAssistanceRequested As String = ""
        Dim RegionalAssistanceRequestedText As String = ""
        Dim AnticipatedAssistance As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectCivilDisturbanceByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            Cause = HelpFunction.Convertdbnulls(objDR2("Cause"))
            GroupOrgResponsible = HelpFunction.Convertdbnulls(objDR2("GroupOrgResponsible"))
            PeopleParticipatingNum = HelpFunction.Convertdbnulls(objDR2("PeopleParticipatingNum"))
            ConfinedLocation = HelpFunction.Convertdbnulls(objDR2("ConfinedLocation"))
            ConfinedLocationOther = HelpFunction.Convertdbnulls(objDR2("ConfinedLocationOther"))
            LocationAreas = HelpFunction.Convertdbnulls(objDR2("LocationAreas"))
            ConfinedLocationMemoText = HelpFunction.Convertdbnulls(objDR2("ConfinedLocationMemoText"))
            AgencyCoordinatingResponse = HelpFunction.Convertdbnulls(objDR2("AgencyCoordinatingResponse"))
            DepartmentAgencyResponding = HelpFunction.Convertdbnulls(objDR2("DepartmentAgencyResponding"))
            'Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            MajorRoadwaysClosed = HelpFunction.Convertdbnulls(objDR2("MajorRoadwaysClosed"))
            'Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            'InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            'Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            'FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            RegionalAssistanceRequested = HelpFunction.Convertdbnulls(objDR2("RegionalAssistanceRequested"))
            RegionalAssistanceRequestedText = HelpFunction.Convertdbnulls(objDR2("RegionalAssistanceRequestedText"))
            AnticipatedAssistance = HelpFunction.Convertdbnulls(objDR2("AnticipatedAssistance"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Civil Event</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Civil Event</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Cause <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Cause of Event:</b> ")
            strBody.Append("           " & Cause & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If GroupOrgResponsible <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Group(s) or organization(s) responsible:</b> ")
            strBody.Append("           " & GroupOrgResponsible & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If PeopleParticipatingNum <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Number of people participating:</b> ")
            strBody.Append("           " & PeopleParticipatingNum & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If ConfinedLocation <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Incident confined to one location:</b> ")
            strBody.Append("           " & ConfinedLocation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If ConfinedLocation = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Location:</b> ")
            strBody.Append("           " & ConfinedLocationOther & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If ConfinedLocationOther = "Other Area" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Areas:</b> ")
            strBody.Append("           " & LocationAreas & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If AgencyCoordinatingResponse <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Law enforcement agency coordinating response:</b> ")
            strBody.Append("           " & AgencyCoordinatingResponse & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If DepartmentAgencyResponding <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Departments/Agencies  responding or on scene:</b> ")
            strBody.Append("           " & DepartmentAgencyResponding & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'If Evacuations <> "" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Evacuations:</b> ")
        '    strBody.Append("           " & Evacuations & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        If MajorRoadwaysClosed <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Major roadways closed:</b> ")
            strBody.Append("           " & MajorRoadwaysClosed & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'If Injury <> "" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Injuries:</b> ")
        '    strBody.Append("           " & Injury & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Injury = "Yes" And InjuryText <> "" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Number and Severity of Injuries:</b> ")
        '    strBody.Append("           " & InjuryText & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Fatality <> "" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Fatalities:</b> ")
        '    strBody.Append("           " & Fatality & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If FatalityText <> "" Then
        '    If Fatality = "Yes" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and location of fatality:</b> ")
        '        strBody.Append("           " & FatalityText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If
    End Sub

    Private Sub GetCriminalActivity(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim IncidentDescription As String = ""
        Dim IndividualDescription As String = ""
        Dim ConfinedLocation As String = ""
        Dim ConfinedLocationDDL As String = ""
        Dim ConfinedLocationText As String = ""
        Dim AgencyCoordinatingResponse As String = ""
        Dim DepartmentAgencyResponding As String = ""
        Dim Lockdown As String = ""
        Dim LockdownText As String = ""
        'Dim Evacuations As String = ""
        Dim MajorRoadwaysClosed As String = ""
        'Dim Injury As String = ""
        'Dim InjuryText As String = ""
        'Dim Fatality As String = ""
        'Dim FatalityText As String = ""
        Dim StateAssistance As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectCriminalActivityByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            IncidentDescription = HelpFunction.Convertdbnulls(objDR2("IncidentDescription"))
            IndividualDescription = HelpFunction.Convertdbnulls(objDR2("IndividualDescription"))
            ConfinedLocation = HelpFunction.Convertdbnulls(objDR2("ConfinedLocation"))
            ConfinedLocationDDL = HelpFunction.Convertdbnulls(objDR2("ConfinedLocationDDL"))
            ConfinedLocationText = HelpFunction.Convertdbnulls(objDR2("ConfinedLocationText"))
            AgencyCoordinatingResponse = HelpFunction.Convertdbnulls(objDR2("AgencyCoordinatingResponse"))
            DepartmentAgencyResponding = HelpFunction.Convertdbnulls(objDR2("DepartmentAgencyResponding"))
            Lockdown = HelpFunction.Convertdbnulls(objDR2("Lockdown"))
            LockdownText = HelpFunction.Convertdbnulls(objDR2("LockdownText"))
            'Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            MajorRoadwaysClosed = HelpFunction.Convertdbnulls(objDR2("MajorRoadwaysClosed"))
            'Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            'Injury = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            'Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            'FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            StateAssistance = HelpFunction.Convertdbnulls(objDR2("StateAssistance"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Law Enforcement Activity</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Law Enforcement Activity</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If IncidentDescription <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description the incident:</b> ")
            strBody.Append("           " & IncidentDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If IndividualDescription <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description of the individual(s) responsible:</b> ")
            strBody.Append("           " & IndividualDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If ConfinedLocation <> "" And ConfinedLocation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Incident confined to one location:</b> ")
            strBody.Append("           " & ConfinedLocation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If ConfinedLocation = "No" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Area(s); specific streets/boundaries preferable:</b> ")
            strBody.Append("           " & ConfinedLocationText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        ElseIf ConfinedLocation = "Yes" Then
            If ConfinedLocationDDL <> "" And ConfinedLocationDDL <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Location:</b> ")
                strBody.Append("           " & ConfinedLocationDDL & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        If ConfinedLocationDDL = "Other area" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Area(s); specific streets/boundaries preferable:</b> ")
            strBody.Append("           " & ConfinedLocationText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If AgencyCoordinatingResponse <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Law enforcement agency coordinating response:</b> ")
            strBody.Append("           " & AgencyCoordinatingResponse & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If DepartmentAgencyResponding <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Departments/agencies responding or on scene:</b> ")
            strBody.Append("           " & DepartmentAgencyResponding & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Lockdown <> "" And Lockdown <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Lockdown:</b> ")
            strBody.Append("           " & Lockdown & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If LockdownText <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Lockdown Area Description:</b> ")
            strBody.Append("           " & LockdownText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'If Evacuations <> "" And Evacuations <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Evacuations:</b> ")
        '    strBody.Append("           " & Evacuations & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        If MajorRoadwaysClosed <> "" And MajorRoadwaysClosed <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Major roadways closed:</b> ")
            strBody.Append("           " & MajorRoadwaysClosed & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'If Injury <> "" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Injuries:</b> ")
        '    strBody.Append("           " & Injury & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If InjuryText = "Yes" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Number and Severity of Injuries:</b> ")
        '    strBody.Append("           " & InjuryText & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Fatality <> "" And Fatality <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Fatalities:</b> ")
        '    strBody.Append("           " & Fatality & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Fatality = "Yes" And FatalityText <> "" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Number and location of Fatalities:</b> ")
        '    strBody.Append("           " & FatalityText & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If StateAssistance <> "" And StateAssistance <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Anticipated need for state assistance:</b> ")
        '    strBody.Append("           " & StateAssistance & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If
    End Sub

    Private Sub GetDamFailure(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim localTime As String = ""
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim DamName As String = ""
        Dim RelatedWaterways As String = ""
        Dim PoolVolumeCapacity As String = ""
        Dim BreakOccurred As String = ""
        Dim BreakAnticipated As String = ""
        Dim CauseOfFailure As String = ""
        Dim ResponsibleForMaintaining As String = ""
        Dim CorrectiveActionsTaken As String = ""
        Dim EstimatedRepairDate As String = ""
        Dim DownstreamPopulationsThreat As String = ""
        Dim DownstreamPopulationsThreatText As String = ""
        'Dim Evacuations As String = ""
        Dim MajorRoadwaysClosed As String = ""
        'Dim Injury As String = ""
        'Dim InjuryText As String = ""
        'Dim Fatality As String = ""
        'Dim FatalityText As String = ""
        Dim StateAssistance As String = ""
        Dim StateAssistanceText As String = ""
        Dim AgencyResponse As String = ""
        Dim StagingCommandLocation As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectDamFailureByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            DamName = HelpFunction.Convertdbnulls(objDR2("DamName"))
            RelatedWaterways = HelpFunction.Convertdbnulls(objDR2("RelatedWaterways"))
            PoolVolumeCapacity = HelpFunction.Convertdbnulls(objDR2("PoolVolumeCapacity"))
            BreakOccurred = HelpFunction.Convertdbnulls(objDR2("BreakOccurred"))
            BreakAnticipated = HelpFunction.Convertdbnulls(objDR2("BreakAnticipated"))
            CauseOfFailure = HelpFunction.Convertdbnulls(objDR2("CauseOfFailure"))
            ResponsibleForMaintaining = HelpFunction.Convertdbnulls(objDR2("ResponsibleForMaintaining"))
            CorrectiveActionsTaken = HelpFunction.Convertdbnulls(objDR2("CorrectiveActionsTaken"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR2("EstimatedRepairTime")))
            EstimatedRepairDate = HelpFunction.Convertdbnulls(objDR2("EstimatedRepairDate"))
            DownstreamPopulationsThreat = HelpFunction.Convertdbnulls(objDR2("DownstreamPopulationsThreat"))
            DownstreamPopulationsThreatText = HelpFunction.Convertdbnulls(objDR2("DownstreamPopulationsThreatText"))
            'Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            MajorRoadwaysClosed = HelpFunction.Convertdbnulls(objDR2("MajorRoadwaysClosed"))
            'Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            'InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            'Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            'FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            StateAssistance = HelpFunction.Convertdbnulls(objDR2("StateAssistance"))
            StateAssistanceText = HelpFunction.Convertdbnulls(objDR2("StateAssistanceText"))
            AgencyResponse = HelpFunction.Convertdbnulls(objDR2("AgencyResponse"))
            StagingCommandLocation = HelpFunction.Convertdbnulls(objDR2("StagingCommandLocation"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Dam Failure</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Dam Failure</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If DamName <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Dam Name:</b> ")
            strBody.Append("           " & DamName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If RelatedWaterways <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Related Waterways/Tributaries:</b> ")
            strBody.Append("           " & RelatedWaterways & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If PoolVolumeCapacity <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Pool volume/capacity behind the dam:</b> ")
            strBody.Append("           " & PoolVolumeCapacity & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If BreakOccurred <> "" And BreakOccurred <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Break occurred:</b> ")
            strBody.Append("           " & BreakOccurred & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If BreakOccurred = "Yes" Then
            If CauseOfFailure <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Cause of failure:</b> ")
                strBody.Append("           " & CauseOfFailure & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf BreakOccurred = "No" Then
            If BreakAnticipated <> "Select an Option" And BreakAnticipated <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Break anticipated:</b> ")
                strBody.Append("           " & BreakAnticipated & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        If ResponsibleForMaintaining <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Responsible for maintaining the dam:</b> ")
            strBody.Append("           " & ResponsibleForMaintaining & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If CorrectiveActionsTaken <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Corrective actions being taken:</b> ")
            strBody.Append("           " & CorrectiveActionsTaken & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If EstimatedRepairDate <> "" And EstimatedRepairDate <> "1/1/1900" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Estimated date that repairs will be completed:</b> ")
            strBody.Append("           " & EstimatedRepairDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If localTime <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Estimated time that repairs will be completed:</b> ")
            strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If DownstreamPopulationsThreat <> "" And DownstreamPopulationsThreat <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Threat to downstream populations:</b> ")
            strBody.Append("           " & DownstreamPopulationsThreat & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If DownstreamPopulationsThreat = "Yes" Then
            If DownstreamPopulationsThreatText <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Size of area and the affected population:</b> ")
                strBody.Append("           " & DownstreamPopulationsThreatText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        'If Evacuations <> "" And Evacuations <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Evacuations:</b> ")
        '    strBody.Append("           " & Evacuations & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        If MajorRoadwaysClosed <> "" And MajorRoadwaysClosed <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Major roadways closed:</b> ")
            strBody.Append("           " & MajorRoadwaysClosed & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'If Injury <> "" And Injury <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Injuries:</b> ")
        '    strBody.Append("           " & Injury & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Injury = "Yes" Then
        '    If InjuryText <> "" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and Severity of Injuries:</b> ")
        '        strBody.Append("           " & InjuryText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If

        'If Fatality <> "" And Fatality <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Fatalities:</b> ")
        '    strBody.Append("           " & Fatality & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Fatality = "Yes" Then
        '    If FatalityText <> "" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and location:</b> ")
        '        strBody.Append("           " & FatalityText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If

        'If StateAssistance <> "" And StateAssistance <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Anticipated need for state assistance:</b> ")
        '    strBody.Append("           " & StateAssistance & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        If StateAssistance = "Yes" Then
            If StateAssistanceText <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Description of anticipated need(s):</b> ")
                strBody.Append("           " & StateAssistanceText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        If AgencyResponse <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Agencies responding or on scene:</b> ")
            strBody.Append("           " & AgencyResponse & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If StagingCommandLocation <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Location of Staging Area or Command Post:</b> ")
            strBody.Append("           " & StagingCommandLocation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If
    End Sub

    Private Sub GetDemINR(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim localTime As String = ""
        Dim localTime2 As String = ""
        Dim localTime3 As String = ""
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim SlrcSeocAlarmType As String = ""
        Dim SlrcSeocZoneNumber As String = ""
        Dim SlrcSeocAlarmStatus As String = ""
        Dim DepWarehouseMemo As String = ""
        Dim DepWarehouseNotification As String = ""
        Dim DepWarehouseZoneNumber As String = ""
        Dim DepWarehouseAlarmStatus As String = ""
        Dim DepWarehouseEmployeeName As String = ""
        Dim DepWarehouseEmployeeCellPhone As String = ""
        Dim DepWarehouseAgencyDivision As String = ""
        Dim DepWarehouseSupervisorName As String = ""
        Dim DepWarehouseSupervisorCalled As String = ""
        Dim DepWarehouseAccessCardNumber As String = ""
        Dim MeBuildingRoomNumber As String = ""
        Dim Me911Called As String = ""
        Dim MePersonBreathing As String = ""
        Dim MeConsiousness As String = ""
        Dim MeComplaintSymptom As String = ""
        Dim SeocActivationLevel As String = ""
        Dim SeocActivationRelatedIncidentNumbers As String = ""
        Dim SeocActivationEmcDatabase As String = ""
        Dim SeocActivationEmcDatabaseName As String = ""
        Dim SmtActivationSMT As String = ""
        Dim SmtActivationReason As String = ""
        Dim SmtActivationReportLocation As String = ""
        Dim SmtActivationAuthorizedBy As String = ""
        Dim ReservistActivationSMT As String = ""
        Dim ReservistActivationReason As String = ""
        Dim ReservistActivationReportLocation As String = ""
        Dim ReservistActivationAuthorizedBy As String = ""
        Dim GeneralNotificationMessage As String = ""
        Dim GeneralNotificationAuthorizedBy As String = ""
        Dim ItDisruptionDescription As String = ""
        Dim ItDisruptionprogramSystem As String = ""
        Dim ItDisruptionStepsTaken As String = ""
        Dim CommDisruptionSystemCircuitText As String = ""
        Dim CommDisruptionSystemCircuit As String = ""
        Dim CommDisruptionDescription As String = ""
        Dim CommDisruptionStepsTaken As String = ""
        Dim PlannedOutageDescription As String = ""
        Dim PlannedOutageScheduledStartDate As String = ""
        Dim PlannedOutageEstimatedCompletion As String = ""
        Dim PlannedOutagecontactNameNumber As String = ""
        Dim strEASRequestorName As String = ""
        Dim strEASRequestReason As String = ""
        Dim strEASBroadcastDate As String = ""
        Dim strEASBroadcastTime As String = ""
        Dim strEASBroadcastDuration As String = ""
        Dim strEASBroadcastMessage As String = ""
        Dim strEASRecommendedActions As String = ""
        Dim strEASLocation As String = ""
        Dim strEASLocationDescription As String = ""
        Dim strEASTransmittedBy As String = ""
        Dim strEASTRansmissionTime As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectDemINRByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            SlrcSeocAlarmType = HelpFunction.Convertdbnulls(objDR2("SlrcSeocAlarmType"))
            SlrcSeocZoneNumber = HelpFunction.Convertdbnulls(objDR2("SlrcSeocZoneNumber"))
            SlrcSeocAlarmStatus = HelpFunction.Convertdbnulls(objDR2("SlrcSeocAlarmStatus"))
            DepWarehouseMemo = HelpFunction.Convertdbnulls(objDR2("DepWarehouseMemo"))
            DepWarehouseNotification = HelpFunction.Convertdbnulls(objDR2("DepWarehouseNotification"))
            DepWarehouseZoneNumber = HelpFunction.Convertdbnulls(objDR2("DepWarehouseZoneNumber"))
            DepWarehouseAlarmStatus = HelpFunction.Convertdbnulls(objDR2("DepWarehouseAlarmStatus"))
            DepWarehouseEmployeeName = HelpFunction.Convertdbnulls(objDR2("DepWarehouseEmployeeName"))
            DepWarehouseEmployeeCellPhone = HelpFunction.Convertdbnulls(objDR2("DepWarehouseEmployeeCellPhone"))
            DepWarehouseAgencyDivision = HelpFunction.Convertdbnulls(objDR2("DepWarehouseAgencyDivision"))
            DepWarehouseSupervisorName = HelpFunction.Convertdbnulls(objDR2("DepWarehouseSupervisorName"))
            DepWarehouseSupervisorCalled = HelpFunction.Convertdbnulls(objDR2("DepWarehouseSupervisorCalled"))
            DepWarehouseAccessCardNumber = HelpFunction.Convertdbnulls(objDR2("DepWarehouseAccessCardNumber"))
            MeBuildingRoomNumber = HelpFunction.Convertdbnulls(objDR2("MEBuildingRoomNumber"))
            Me911Called = HelpFunction.Convertdbnulls(objDR2("Me911Called"))
            MePersonBreathing = HelpFunction.Convertdbnulls(objDR2("MePersonBreathing"))
            MeConsiousness = HelpFunction.Convertdbnulls(objDR2("MeConsiousness"))
            MeComplaintSymptom = HelpFunction.Convertdbnulls(objDR2("MeComplaintSymptom"))
            SeocActivationLevel = HelpFunction.Convertdbnulls(objDR2("SeocActivationLevel"))
            SeocActivationRelatedIncidentNumbers = HelpFunction.Convertdbnulls(objDR2("SeocActivationRelatedIncidentNumbers"))
            SeocActivationEmcDatabase = HelpFunction.Convertdbnulls(objDR2("SeocActivationEmcDatabase"))
            SeocActivationEmcDatabaseName = HelpFunction.Convertdbnulls(objDR2("SeocActivationEmcDatabaseName"))
            SmtActivationSMT = HelpFunction.Convertdbnulls(objDR2("SmtActivationSMT"))
            SmtActivationReason = HelpFunction.Convertdbnulls(objDR2("SmtActivationReason"))
            SmtActivationReportLocation = HelpFunction.Convertdbnulls(objDR2("SmtActivationReportLocation"))
            SmtActivationAuthorizedBy = HelpFunction.Convertdbnulls(objDR2("SmtActivationAuthorizedBy"))
            ReservistActivationSMT = HelpFunction.Convertdbnulls(objDR2("ReservistActivationSMT"))
            ReservistActivationReason = HelpFunction.Convertdbnulls(objDR2("ReservistActivationReason"))
            ReservistActivationReportLocation = HelpFunction.Convertdbnulls(objDR2("ReservistActivationReportLocation"))
            ReservistActivationAuthorizedBy = HelpFunction.Convertdbnulls(objDR2("ReservistActivationAuthorizedBy"))
            GeneralNotificationMessage = HelpFunction.Convertdbnulls(objDR2("GeneralNotificationMessage"))
            GeneralNotificationAuthorizedBy = HelpFunction.Convertdbnulls(objDR2("GeneralNotificationAuthorizedBy"))
            ItDisruptionDescription = HelpFunction.Convertdbnulls(objDR2("ItDisruptionDescription"))
            ItDisruptionprogramSystem = HelpFunction.Convertdbnulls(objDR2("ItDisruptionprogramSystem"))
            CommDisruptionSystemCircuitText = HelpFunction.Convertdbnulls(objDR2("CommDisruptionSystemCircuitText"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR2("ItDisruptionTime")))
            ItDisruptionStepsTaken = HelpFunction.Convertdbnulls(objDR2("ItDisruptionStepsTaken"))
            CommDisruptionSystemCircuit = HelpFunction.Convertdbnulls(objDR2("CommDisruptionSystemCircuit"))
            CommDisruptionDescription = HelpFunction.Convertdbnulls(objDR2("CommDisruptionDescription"))
            localTime2 = CStr(HelpFunction.Convertdbnulls(objDR2("CommDisruptionTime")))
            CommDisruptionStepsTaken = HelpFunction.Convertdbnulls(objDR2("CommDisruptionStepsTaken"))
            PlannedOutageDescription = HelpFunction.Convertdbnulls(objDR2("PlannedOutageDescription"))
            PlannedOutageScheduledStartDate = HelpFunction.Convertdbnulls(objDR2("PlannedOutageScheduledStartDate"))
            localTime3 = CStr(HelpFunction.Convertdbnulls(objDR2("PlannedOutageScheduledStartTime")))
            PlannedOutageEstimatedCompletion = HelpFunction.Convertdbnulls(objDR2("PlannedOutageEstimatedCompletion"))
            PlannedOutagecontactNameNumber = HelpFunction.Convertdbnulls(objDR2("PlannedOutagecontactNameNumber"))
            strEASRequestorName = HelpFunction.Convertdbnulls(objDR2("EASRequestorName"))
            strEASRequestReason = HelpFunction.Convertdbnulls(objDR2("EASRequestReason"))
            strEASBroadcastDate = HelpFunction.Convertdbnulls(objDR2("EASBroadcastDate"))
            strEASBroadcastTime = CStr(HelpFunction.Convertdbnulls(objDR2("EASBroadcastTime")))
            strEASBroadcastDuration = HelpFunction.Convertdbnulls(objDR2("EASBroadcastDuration"))
            strEASBroadcastMessage = HelpFunction.Convertdbnulls(objDR2("EASBroadcastMessage"))
            strEASRecommendedActions = HelpFunction.Convertdbnulls(objDR2("EASRecommendedActions"))
            strEASLocation = HelpFunction.Convertdbnulls(objDR2("EASLocation"))
            strEASLocationDescription = HelpFunction.Convertdbnulls(objDR2("EASLocationDescription"))
            strEASTransmittedBy = HelpFunction.Convertdbnulls(objDR2("EASTransmittedBy"))
            strEASTRansmissionTime = CStr(HelpFunction.Convertdbnulls(objDR2("EASTRansmissionTime")))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>DEM Incidents</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>DEM Incidents</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "SLRC Alarm" Or SubType = "SEOC Alarm" Then
            If SlrcSeocAlarmType <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Alarm Type:</b> ")
                strBody.Append("           " & SlrcSeocAlarmType & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SlrcSeocZoneNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Zone number(s)and/or description(s):</b> ")
                strBody.Append("           " & SlrcSeocZoneNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SlrcSeocAlarmStatus <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Alarm Status:</b> ")
                strBody.Append("           " & SlrcSeocAlarmStatus & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "DEP Alarm" Then
            If DepWarehouseMemo <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Label/Memo that appears after selection:</b> ")
                strBody.Append("           " & DepWarehouseMemo & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If DepWarehouseNotification <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Alarm or Non-Alarm Notification:</b> ")
                strBody.Append("           " & DepWarehouseNotification & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If DepWarehouseNotification = "Alarm" Then
                If DepWarehouseZoneNumber <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Zone number(s) and/or description(s):</b> ")
                    strBody.Append("           " & DepWarehouseZoneNumber & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If DepWarehouseAlarmStatus <> "Select an Option" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Alarm Status:</b> ")
                    strBody.Append("           " & DepWarehouseAlarmStatus & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If DepWarehouseNotification = "Non-Alarm Notification" Then
                If DepWarehouseEmployeeName <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Employee name:</b> ")
                    strBody.Append("           " & DepWarehouseEmployeeName & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If DepWarehouseEmployeeCellPhone <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Employee cell phone:</b> ")
                    strBody.Append("           " & DepWarehouseEmployeeCellPhone & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If DepWarehouseAgencyDivision <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Agency and Division:</b> ")
                    strBody.Append("           " & DepWarehouseAgencyDivision & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If DepWarehouseSupervisorName <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Supervisor name:</b> ")
                    strBody.Append("           " & DepWarehouseSupervisorName & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If DepWarehouseSupervisorCalled <> "Select an Option" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Supervisor called:</b> ")
                    strBody.Append("           " & DepWarehouseSupervisorCalled & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If DepWarehouseAccessCardNumber <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Access card number:</b> ")
                    strBody.Append("           " & DepWarehouseAccessCardNumber & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If
        ElseIf SubType = "Medical Emergency" Then
            If MeBuildingRoomNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Building and Room Number:</b> ")
                strBody.Append("           " & MeBuildingRoomNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Me911Called <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>911 called:</b> ")
                strBody.Append("           " & Me911Called & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If MePersonBreathing <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Is the person breathing: </b> ")
                strBody.Append("           " & MePersonBreathing & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If MeConsiousness <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Person's level of consiousness:</b> ")
                strBody.Append("           " & MeConsiousness & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If MeComplaintSymptom <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Person's complaint or symptoms:</b> ")
                strBody.Append("           " & MeComplaintSymptom & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "SEOC Activation" Then
            If SeocActivationLevel <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Activation level:</b> ")
                strBody.Append("           " & SeocActivationLevel & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SeocActivationRelatedIncidentNumbers <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Related Incident Numbers:</b> ")
                strBody.Append("           " & SeocActivationRelatedIncidentNumbers & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SeocActivationEmcDatabase <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>EM Constellation Database:</b> ")
                strBody.Append("           " & SeocActivationEmcDatabase & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SeocActivationEmcDatabaseName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>EMC Database Name:</b> ")
                strBody.Append("           " & SeocActivationEmcDatabaseName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "SMT Activation" Then
            If SmtActivationSMT <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>SMT:</b> ")
                strBody.Append("           " & SmtActivationSMT & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SmtActivationReason <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Reason for activation:</b> ")
                strBody.Append("           " & SmtActivationReason & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SmtActivationReportLocation <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Location to Report:</b> ")
                strBody.Append("           " & SmtActivationReportLocation & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SmtActivationAuthorizedBy <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Authorized By:</b> ")
                strBody.Append("           " & SmtActivationAuthorizedBy & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Reservist Activation" Then

            'strBody.Append("<table>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td align='left'width='650px'>")
            'strBody.Append("            <b>Select SMT:</b> ")
            'strBody.Append("           " & ReservistActivationSMT & "  ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")

            If ReservistActivationReason <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Reason for activation:</b> ")
                strBody.Append("           " & ReservistActivationReason & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ReservistActivationReportLocation <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Location to Report:</b> ")
                strBody.Append("           " & ReservistActivationReportLocation & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ReservistActivationAuthorizedBy <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Authorized By:</b> ")
                strBody.Append("           " & ReservistActivationAuthorizedBy & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "General Notification" Then
            If GeneralNotificationMessage <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>General Notification Message:</b> ")
                strBody.Append("           " & GeneralNotificationMessage & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If GeneralNotificationAuthorizedBy <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Authorized By:</b> ")
                strBody.Append("           " & GeneralNotificationAuthorizedBy & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "IT Disruption or Issue" Then
            If ItDisruptionDescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Description of Problem:</b> ")
                strBody.Append("           " & ItDisruptionDescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ItDisruptionprogramSystem <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Name of program(s)/system(s):</b> ")
                strBody.Append("           " & ItDisruptionprogramSystem & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Time the problem started:</b> ")
                strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ItDisruptionStepsTaken <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>List any troubleshooting steps taken:</b> ")
                strBody.Append("           " & ItDisruptionStepsTaken & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Communications Disruption or Issue" Then
            If CommDisruptionSystemCircuit <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Communication system(s) or circuit(s):</b> ")
                strBody.Append("           " & CommDisruptionSystemCircuit & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CommDisruptionSystemCircuit = "Other" Then
                If CommDisruptionSystemCircuitText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>System:</b> ")
                    strBody.Append("           " & CommDisruptionSystemCircuitText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If CommDisruptionDescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Description of problem:</b> ")
                strBody.Append("           " & CommDisruptionDescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime2 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Time the problem started:</b> ")
                strBody.Append("           " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CommDisruptionStepsTaken <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Troubleshooting steps taken:</b> ")
                strBody.Append("           " & CommDisruptionStepsTaken & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Planned Outage" Then
            If PlannedOutageDescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Description of system(s) that will be impacted:</b> ")
                strBody.Append("           " & PlannedOutageDescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If PlannedOutageScheduledStartDate <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Scheduled start date:</b> ")
                strBody.Append("           " & PlannedOutageScheduledStartDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime3 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Scheduled start time:</b> ")
                strBody.Append("           " & Left(localTime3, 2) & ":" & Right(localTime3, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If PlannedOutagecontactNameNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Point of contact name/number:</b> ")
                strBody.Append("           " & PlannedOutagecontactNameNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "EAS/IPAWS Activation" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Requestor Name:</b> ")
            strBody.Append("           " & strEASRequestorName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Request Reason:</b> ")
            strBody.Append("           " & strEASRequestReason & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If strEASBroadcastDate <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Broadcast date:</b> ")
                strBody.Append("           " & strEASBroadcastDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If strEASBroadcastTime <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Broadcast time:</b> ")
                strBody.Append("           " & Left(strEASBroadcastTime, 2) & ":" & Right(strEASBroadcastTime, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If strEASBroadcastDuration <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Broadcast duration:</b> ")
                strBody.Append("           " & strEASBroadcastDuration & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If strEASBroadcastMessage <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Broadcast message:</b> ")
                strBody.Append("           " & strEASBroadcastMessage & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If strEASRecommendedActions <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Recommended actions:</b> ")
                strBody.Append("           " & strEASRecommendedActions & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If strEASLocation <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Location:</b> ")
                strBody.Append("           " & strEASLocation & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If strEASLocationDescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Location description:</b> ")
                strBody.Append("           " & strEASLocationDescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Transmitted by:</b> ")
            strBody.Append("           " & strEASTransmittedBy & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Transmission time:</b> ")
            strBody.Append("           " & Left(strEASTRansmissionTime, 2) & ":" & Right(strEASTRansmissionTime, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If
    End Sub

    Private Sub GetDrinkingWaterFacility(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim PublicWaterSystemIDNumber As String = ""
        Dim FacilityName As String = ""
        Dim TrespassVandalismTheft As String = ""
        Dim TrespassVandalismTheftText As String = ""
        Dim DamageFacilityDistibutionSystem As String = ""
        Dim DFDSintentional As String = ""
        Dim AccessWaterSupply As String = ""
        Dim Degredation As String = ""
        Dim IndividualResponsible As String = ""
        Dim LawEnforcementContacted As String = ""
        Dim IndividualResponsibleCaseNumber As String = ""
        Dim BWpublicWaterSystemIDNumber As String = ""
        Dim BWIncidentDueTo As String = ""
        Dim BWnumberCustomersAffected As String = ""
        Dim BWaffectedAreas As String = ""
        Dim FWpublicWaterSystemIDNumber As String = ""
        Dim FWnumberCustomersAffected As String = ""
        Dim FWutilityName As String = ""
        Dim FWcauseForNeed As String = ""
        Dim FWdurationOfNeed As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectDrinkingWaterFacilityByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            PublicWaterSystemIDNumber = HelpFunction.Convertdbnulls(objDR2("PublicWaterSystemIDNumber"))
            FacilityName = HelpFunction.Convertdbnulls(objDR2("FacilityName"))
            TrespassVandalismTheft = HelpFunction.Convertdbnulls(objDR2("TrespassVandalismTheft"))
            TrespassVandalismTheftText = HelpFunction.Convertdbnulls(objDR2("TrespassVandalismTheftText"))
            DamageFacilityDistibutionSystem = HelpFunction.Convertdbnulls(objDR2("DamageFacilityDistibutionSystem"))
            DFDSintentional = HelpFunction.Convertdbnulls(objDR2("DFDSintentional"))
            AccessWaterSupply = HelpFunction.Convertdbnulls(objDR2("AccessWaterSupply"))
            Degredation = HelpFunction.Convertdbnulls(objDR2("Degredation"))
            IndividualResponsible = HelpFunction.Convertdbnulls(objDR2("IndividualResponsible"))
            LawEnforcementContacted = HelpFunction.Convertdbnulls(objDR2("LawEnforcementContacted"))
            IndividualResponsibleCaseNumber = HelpFunction.Convertdbnulls(objDR2("IndividualResponsibleCaseNumber"))
            BWpublicWaterSystemIDNumber = HelpFunction.Convertdbnulls(objDR2("BWpublicWaterSystemIDNumber"))
            BWIncidentDueTo = HelpFunction.Convertdbnulls(objDR2("BWIncidentDueTo"))
            BWnumberCustomersAffected = HelpFunction.Convertdbnulls(objDR2("BWnumberCustomersAffected"))
            BWaffectedAreas = HelpFunction.Convertdbnulls(objDR2("BWaffectedAreas"))
            FWpublicWaterSystemIDNumber = HelpFunction.Convertdbnulls(objDR2("FWpublicWaterSystemIDNumber"))
            FWnumberCustomersAffected = HelpFunction.Convertdbnulls(objDR2("FWnumberCustomersAffected"))
            FWutilityName = HelpFunction.Convertdbnulls(objDR2("FWutilityName"))
            FWcauseForNeed = HelpFunction.Convertdbnulls(objDR2("FWcauseForNeed"))
            FWdurationOfNeed = HelpFunction.Convertdbnulls(objDR2("FWdurationOfNeed"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Drinking Water Facility</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Drinking Water Facility</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "DWF Report" Then
            If PublicWaterSystemIDNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Public Water System ID Number:</b> ")
                strBody.Append("           " & PublicWaterSystemIDNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If FacilityName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Name of Facility:</b> ")
                strBody.Append("           " & FacilityName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TrespassVandalismTheft <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Trespassing, vandalism, or theft:</b> ")
                strBody.Append("           " & TrespassVandalismTheft & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TrespassVandalismTheft = "Yes" Then
                If TrespassVandalismTheftText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Description of what occurred:</b> ")
                    strBody.Append("           " & TrespassVandalismTheftText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If DamageFacilityDistibutionSystem <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Damage to the facility or distribution system:</b> ")
                strBody.Append("           " & DamageFacilityDistibutionSystem & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If DamageFacilityDistibutionSystem = "Yes" Then
                If DFDSintentional <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Intentional:</b> ")
                    strBody.Append("           " & DFDSintentional & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If AccessWaterSupply <> "" And AccessWaterSupply <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Access made to the water supply:</b> ")
                strBody.Append("           " & AccessWaterSupply & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Degredation <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Degradation to water quality, system pressure, or water production:</b> ")
                strBody.Append("           " & Degredation & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If IndividualResponsible <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Description of the individual(s) responsible:</b> ")
                strBody.Append("           " & IndividualResponsible & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If LawEnforcementContacted <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Local Law Enforcement been contacted:</b> ")
                strBody.Append("           " & LawEnforcementContacted & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If LawEnforcementContacted = "Yes" Then
                If IndividualResponsibleCaseNumber <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Case number, if known:</b> ")
                    strBody.Append("           " & IndividualResponsibleCaseNumber & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If
        ElseIf SubType = "Boil Water Advisory" Then
            If BWpublicWaterSystemIDNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Public Water System ID Number:</b> ")
                strBody.Append("           " & BWpublicWaterSystemIDNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If BWIncidentDueTo <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>This incident was due to a:</b> ")
                strBody.Append("           " & BWIncidentDueTo & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If BWnumberCustomersAffected <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Number of customers affected:</b> ")
                strBody.Append("           " & BWnumberCustomersAffected & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If BWaffectedAreas <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Affected Areas, including streets or boundaries:</b> ")
                strBody.Append("           " & BWaffectedAreas & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

        ElseIf SubType = "FlaWARN Generator Deployment" Then

            If FWpublicWaterSystemIDNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Public Water System ID Number:</b> ")
                strBody.Append("           " & FWpublicWaterSystemIDNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If FWnumberCustomersAffected <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Number of customers affected:</b> ")
                strBody.Append("           " & FWnumberCustomersAffected & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If FWutilityName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Name of Utility:</b> ")
                strBody.Append("           " & FWutilityName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If FWcauseForNeed <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Cause for need of generator:</b> ")
                strBody.Append("           " & FWcauseForNeed & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If FWdurationOfNeed <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Anticipated duration of need:</b> ")
                strBody.Append("           " & FWdurationOfNeed & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If
    End Sub

    Private Sub GetEnvironmentalCrime(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim MaterialDescription As String = ""
        Dim CrimeTimeline As String = ""
        Dim IndividalsDescription As String = ""
        Dim VehiclesDescription As String = ""
        Dim CountyCodeEnforcement As String = ""
        Dim CountyCodeEnforcementText As String = ""
        Dim CallBack As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectEnvironmentalCrimeByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            MaterialDescription = HelpFunction.Convertdbnulls(objDR2("MaterialDescription"))
            CrimeTimeline = HelpFunction.Convertdbnulls(objDR2("CrimeTimeline"))
            IndividalsDescription = HelpFunction.Convertdbnulls(objDR2("IndividalsDescription"))
            VehiclesDescription = HelpFunction.Convertdbnulls(objDR2("VehiclesDescription"))
            CountyCodeEnforcement = HelpFunction.Convertdbnulls(objDR2("CountyCodeEnforcement"))
            CountyCodeEnforcementText = HelpFunction.Convertdbnulls(objDR2("CountyCodeEnforcementText"))
            CallBack = HelpFunction.Convertdbnulls(objDR2("CallBack"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Environmental Crime</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Environmental Crime</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='650px'>")
        'strBody.Append("            <b>Sub-Type:</b> ")
        'strBody.Append("           " & SubType & "  ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")
        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='650px'>")
        'strBody.Append("            <b>Situation:</b> ")
        'strBody.Append("           " & Situation & "  ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MaterialDescription <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description of the material(s) involved:</b> ")
            strBody.Append("           " & MaterialDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If CrimeTimeline <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Length of time crime has been occurring:</b> ")
            strBody.Append("           " & CrimeTimeline & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If IndividalsDescription <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Describe the individual(s) involved:</b> ")
            strBody.Append("           " & IndividalsDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If VehiclesDescription <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Describe any vehicles(s) involved:</b> ")
            strBody.Append("           " & VehiclesDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If CountyCodeEnforcement <> "" And CountyCodeEnforcement <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Caller contacted county code enforcement:</b> ")
            strBody.Append("           " & CountyCodeEnforcement & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If CountyCodeEnforcement = "Yes" Then
            If CountyCodeEnforcementText <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Code Enforcement Actions:</b> ")
                strBody.Append("           " & CountyCodeEnforcementText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        If CallBack <> "" And CallBack <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>FWC Officer to contact caller:</b> ")
            strBody.Append("           " & CallBack & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If
    End Sub

    Private Sub GetFire(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        'Dim Evacuations As String = ""
        'Dim Injury As String = ""
        'Dim InjuryText As String = ""
        'Dim Fatality As String = ""
        'Dim FatalityText As String = ""
        Dim MajorRoadwaysClosed As String = ""
        Dim Acres As String = ""
        Dim Endangerment As String = ""
        Dim FFSNotified As String = ""
        Dim FFSFireName As String = ""
        Dim FFSFireNumber As String = ""
        Dim OtherAssistanceRequested As String = ""
        Dim StructuresThreatened As String = ""
        Dim StructuresThreatenedText As String = ""
        Dim HazardousMaterials As String = ""
        Dim Cause As String = ""
        Dim IndicentSeverity As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectFireByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            'Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            'Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            'InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            'Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            'FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            MajorRoadwaysClosed = HelpFunction.Convertdbnulls(objDR2("MajorRoadwaysClosed"))
            Acres = HelpFunction.Convertdbnulls(objDR2("Acres"))
            Endangerment = HelpFunction.Convertdbnulls(objDR2("Endangerment"))
            FFSNotified = HelpFunction.Convertdbnulls(objDR2("DOFNotified"))
            FFSFireName = HelpFunction.Convertdbnulls(objDR2("DOFFireName"))
            FFSFireNumber = HelpFunction.Convertdbnulls(objDR2("DOFFireNumber"))
            OtherAssistanceRequested = HelpFunction.Convertdbnulls(objDR2("OtherAssistanceRequested"))
            StructuresThreatened = HelpFunction.Convertdbnulls(objDR2("StructuresThreatened"))
            StructuresThreatenedText = HelpFunction.Convertdbnulls(objDR2("StructuresThreatenedText"))
            HazardousMaterials = HelpFunction.Convertdbnulls(objDR2("HazardousMaterials"))
            Cause = HelpFunction.Convertdbnulls(objDR2("Cause"))
            IndicentSeverity = HelpFunction.Convertdbnulls(objDR2("IndicentSeverity"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Fire</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Fire</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'If Evacuations <> "" And Evacuations <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Evacuations:</b> ")
        '    strBody.Append("           " & Evacuations & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Injury <> "" And Injury <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Injuries:</b> ")
        '    strBody.Append("           " & Injury & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Injury = "Yes" Then
        '    If InjuryText <> "" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and Severity of Injuries:</b> ")
        '        strBody.Append("           " & InjuryText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If

        'If Fatality <> "" And Fatality <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Fatalities:</b> ")
        '    strBody.Append("           " & Fatality & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Fatality = "Yes" Then
        '    If FatalityText <> "" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and location of fatalities:</b> ")
        '        strBody.Append("           " & FatalityText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If

        If MajorRoadwaysClosed <> "" And MajorRoadwaysClosed <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Major roadways closed:</b> ")
            strBody.Append("           " & MajorRoadwaysClosed & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "Wildfire" Then
            If Acres <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Acres on fire:</b> ")
                strBody.Append("           " & Acres & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Endangerment <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Endangerments:</b> ")
                strBody.Append("           " & Endangerment & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If FFSNotified <> "" And FFSNotified <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Florida Forest Service has been notified:</b> ")
                strBody.Append("           " & FFSNotified & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If FFSFireName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>FFS Fire Name:</b> ")
                strBody.Append("           " & FFSFireName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If FFSFireNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>FFS Fire Number:</b> ")
                strBody.Append("           " & FFSFireNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If OtherAssistanceRequested <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Other assistance requested:</b> ")
                strBody.Append("           " & OtherAssistanceRequested & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Other" Then
            If StructuresThreatened <> "" And StructuresThreatened <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Other structures threatened:</b> ")
                strBody.Append("           " & StructuresThreatened & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If StructuresThreatened = "Yes" Then
                If StructuresThreatenedText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Structures Threatened Text:</b> ")
                    strBody.Append("           " & StructuresThreatenedText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If HazardousMaterials <> "" And HazardousMaterials <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Hazardous materials inside the structure:</b> ")
                strBody.Append("           " & HazardousMaterials & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Cause <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Cause of the fire, if known:</b> ")
                strBody.Append("           " & Cause & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If IndicentSeverity <> "" And IndicentSeverity <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Incident Severity:</b> ")
                strBody.Append("           " & IndicentSeverity & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If
    End Sub

    Private Sub GetGeneral(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim GeneralDescription As String = ""
        Dim SpecificHazards As String = ""
        Dim RemedialActionsPlannedOccuring As String = ""
        Dim ActivationLevel As String = ""
        Dim CauseOfActivation As String = ""
        Dim EOCContactNumber As String = ""
        Dim EOCContactEMail As String = ""
        Dim HoursOperationalPeriodsStaffing As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectGeneralByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            GeneralDescription = HelpFunction.Convertdbnulls(objDR2("GeneralDescription"))
            SpecificHazards = HelpFunction.Convertdbnulls(objDR2("SpecificHazards"))
            RemedialActionsPlannedOccuring = HelpFunction.Convertdbnulls(objDR2("RemedialActionsPlannedOccuring"))
            ActivationLevel = HelpFunction.Convertdbnulls(objDR2("ActivationLevel"))
            CauseOfActivation = HelpFunction.Convertdbnulls(objDR2("CauseOfActivation"))
            EOCContactNumber = HelpFunction.Convertdbnulls(objDR2("EOCContactNumber"))
            EOCContactEMail = HelpFunction.Convertdbnulls(objDR2("EOCContactEMail"))
            HoursOperationalPeriodsStaffing = HelpFunction.Convertdbnulls(objDR2("HoursOperationalPeriodsStaffing"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>General</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>General</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "General Incident" Then
            If GeneralDescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Incident description:</b> ")
                strBody.Append("           " & GeneralDescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SpecificHazards <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Existing specific hazards:</b> ")
                strBody.Append("           " & SpecificHazards & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If RemedialActionsPlannedOccuring <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Remedial actions planned or occurring:</b> ")
                strBody.Append("           " & RemedialActionsPlannedOccuring & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Local/County EOC Activation" Then
            If ActivationLevel <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Level of Activation:</b> ")
                strBody.Append("           " & ActivationLevel & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CauseOfActivation <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Incident(s) or hazards(s) caused the activation:</b> ")
                strBody.Append("           " & CauseOfActivation & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If EOCContactNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>EOC Contact Number:</b> ")
                strBody.Append("           " & EOCContactNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If EOCContactEMail <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>EOC Contact E-Mail:</b> ")
                strBody.Append("           " & EOCContactEMail & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If HoursOperationalPeriodsStaffing <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Hours operation/operational periods & staffing:</b> ")
                strBody.Append("           " & HoursOperationalPeriodsStaffing & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If
    End Sub

    Private Sub GetGeologicalEvent(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim SsDimensions As String = ""
        Dim SsDepthOfSubsidence As String = ""
        Dim SsIsRockVisible As String = ""
        Dim SsIsWaterInBottom As String = ""
        Dim SsSourceOfWater As String = ""
        Dim SsSurfaceStructuresThreatenedDamaged = ""
        Dim SsSubSurfaceStructuresThreatenedDamaged = ""
        Dim EaMagnitude As String = ""
        Dim EaLocation As String = ""
        Dim EaDepth As String = ""
        'Dim Evacuations As String = ""
        Dim MajorRoadwaysClosed As String = ""
        'Dim Injury As String = ""
        'Dim InjuryText As String = ""
        'Dim Fatality As String = ""
        'Dim FatalityText As String = ""
        Dim StateAssistance As String = ""
        Dim StateAssistanceText As String = ""
        Dim AgencyResponding As String = ""
        Dim AgencyNotified As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectGeologicalEventByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            SsDimensions = HelpFunction.Convertdbnulls(objDR2("SsDimensions"))
            SsDepthOfSubsidence = HelpFunction.Convertdbnulls(objDR2("SsDepthOfSubsidence"))
            SsIsRockVisible = HelpFunction.Convertdbnulls(objDR2("SsIsRockVisible"))
            SsIsWaterInBottom = HelpFunction.Convertdbnulls(objDR2("SsIsWaterInBottom"))
            SsSourceOfWater = HelpFunction.Convertdbnulls(objDR2("SsSourceOfWater"))
            SsSurfaceStructuresThreatenedDamaged = HelpFunction.Convertdbnulls(objDR2("SsSurfaceStructuresThreatenedDamaged"))
            SsSubSurfaceStructuresThreatenedDamaged = HelpFunction.Convertdbnulls(objDR2("SsSubSurfaceStructuresThreatenedDamaged"))
            EaMagnitude = HelpFunction.Convertdbnulls(objDR2("EaMagnitude"))
            EaLocation = HelpFunction.Convertdbnulls(objDR2("EaLocation"))
            EaDepth = HelpFunction.Convertdbnulls(objDR2("EaDepth"))
            'Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            MajorRoadwaysClosed = HelpFunction.Convertdbnulls(objDR2("MajorRoadwaysClosed"))
            'Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            'InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            'Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            'FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            StateAssistance = HelpFunction.Convertdbnulls(objDR2("StateAssistance"))
            StateAssistanceText = HelpFunction.Convertdbnulls(objDR2("StateAssistanceText"))
            AgencyResponding = HelpFunction.Convertdbnulls(objDR2("AgencyResponding"))
            AgencyNotified = HelpFunction.Convertdbnulls(objDR2("AgencyNotified"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Geological Event</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Geological Event</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "Earthquake" Or SubType = "Aftershock" Then
            If EaMagnitude <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Magnitude:</b> ")
                strBody.Append("           " & EaMagnitude & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If EaLocation <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Location:</b> ")
                strBody.Append("           " & EaLocation & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If EaDepth <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Depth:</b> ")
                strBody.Append("           " & EaDepth & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Subsidence or Sinkhole" Then
            If SsDimensions <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Dimensions (length & width) of the area that subsided:</b> ")
                strBody.Append("           " & SsDimensions & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SsDepthOfSubsidence <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Depth of subsidence feature:</b> ")
                strBody.Append("           " & SsDepthOfSubsidence & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SsIsRockVisible = "Yes" Or SsIsRockVisible = "No" Or SsIsRockVisible = "Unknown" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Rocks visible in subsidence feature:</b> ")
                strBody.Append("           " & SsIsRockVisible & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SsIsWaterInBottom = "Yes" Or SsIsWaterInBottom = "No" Or SsIsWaterInBottom = "Unknown" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Water in bottom of subsidence feature:</b> ")
                strBody.Append("           " & SsIsWaterInBottom & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SsSourceOfWater <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Apparent source of water:</b> ")
                strBody.Append("           " & SsSourceOfWater & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SsSurfaceStructuresThreatenedDamaged <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Surface structures threatened or damaged:</b> ")
                strBody.Append("           " & SsSurfaceStructuresThreatenedDamaged & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If SsSubSurfaceStructuresThreatenedDamaged <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Sub-surface structures threatened or damaged:</b> ")
                strBody.Append("           " & SsSubSurfaceStructuresThreatenedDamaged & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        'If Evacuations <> "" And Evacuations <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Evacuations:</b> ")
        '    strBody.Append("           " & Evacuations & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        If MajorRoadwaysClosed <> "" And MajorRoadwaysClosed <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Major roadways closed:</b> ")
            strBody.Append("           " & MajorRoadwaysClosed & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'If Injury <> "" And Injury <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Injuries:</b> ")
        '    strBody.Append("           " & Injury & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Injury = "Yes" Then
        '    If InjuryText <> "" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and Severity of Injuries:</b> ")
        '        strBody.Append("           " & InjuryText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If

        'If Fatality <> "" And Fatality <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Fatalities:</b> ")
        '    strBody.Append("           " & Fatality & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Fatality = "Yes" Then
        '    If FatalityText <> "" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and location of fatalities:</b> ")
        '        strBody.Append("           " & FatalityText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If

        'If StateAssistance <> "" And StateAssistance <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Anticipated need for state assistance:</b> ")
        '    strBody.Append("           " & StateAssistance & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        If StateAssistance = "Yes" Then
            If StateAssistanceText <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Description of anticipated need(s):</b> ")
                strBody.Append("           " & StateAssistanceText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='650px'>")
        'strBody.Append("            <b>Situation:</b> ")
        'strBody.Append("           " & Situation & "  ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")
        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='650px'>")
        'strBody.Append("            <b>Situation:</b> ")
        'strBody.Append("           " & Situation & "  ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")
        'strBody.Append("<table width='100%' align='center'>")
        'strBody.Append("<tr>")
        'strBody.Append("<td width='50%' align='left'> What agencies are responding or on scene? " & AgencyResponding & "</font></td>")
        'strBody.Append("<td width='50%' align='left'> What agencies have been notified? " & AgencyNotified & "</font></td>")
        'strBody.Append("</tr>")
        'strBody.Append("</table>")
    End Sub

    Private Sub GetKennedySpaceCenter(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim MissionName As String = ""
        Dim InrlMissionLaunchDate As String = ""
        Dim localTime As String = ""
        Dim localTime2 As String = ""
        Dim InrlBrevardCo As String = ""
        Dim InrlBrevardCo2 As String = ""
        Dim NextMissionLaunchDate As String = ""
        Dim ScrubDate As String = ""
        Dim localTime3 As String = ""
        Dim ScrubReason As String = ""
        Dim ScrubNextLaunchDateTime As String = ""
        Dim SuccessDate As String = ""
        Dim localTime4 As String = ""
        Dim UnsuccessDate As String = ""
        Dim localTime5 As String = ""
        Dim UnsuccessReason As String = ""
        Dim UnsuccessOffSiteImpact As String = ""
        Dim UnsuccessOffSiteImpactText As String = ""
        'Dim Injury As String = ""
        'Dim InjuryText As String = ""
        'Dim Fatality As String = ""
        'Dim FatalityText As String = ""
        Dim LaunchLocation As String = ""
        Dim LaunchLocationText As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectKennedySpaceCenterByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            MissionName = HelpFunction.Convertdbnulls(objDR2("MissionName"))
            InrlMissionLaunchDate = HelpFunction.Convertdbnulls(objDR2("InrlMissionLaunchDate"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR2("InrlLaunchWindow")))
            localTime2 = CStr(HelpFunction.Convertdbnulls(objDR2("InrlLaunchWindow2")))
            InrlBrevardCo = HelpFunction.Convertdbnulls(objDR2("InrlBrevardCo"))
            InrlBrevardCo2 = HelpFunction.Convertdbnulls(objDR2("InrlBrevardCo2"))
            NextMissionLaunchDate = HelpFunction.Convertdbnulls(objDR2("NextMissionLaunchDate"))
            ScrubDate = HelpFunction.Convertdbnulls(objDR2("ScrubDate"))
            localTime3 = CStr(HelpFunction.Convertdbnulls(objDR2("ScrubTime")))
            ScrubReason = HelpFunction.Convertdbnulls(objDR2("ScrubReason"))
            ScrubNextLaunchDateTime = HelpFunction.Convertdbnulls(objDR2("ScrubNextLaunchDateTime"))
            SuccessDate = HelpFunction.Convertdbnulls(objDR2("SuccessDate"))
            localTime4 = CStr(HelpFunction.Convertdbnulls(objDR2("SuccessTime")))
            UnsuccessDate = HelpFunction.Convertdbnulls(objDR2("UnsuccessDate"))
            localTime5 = CStr(HelpFunction.Convertdbnulls(objDR2("UnsuccessTime")))
            UnsuccessReason = HelpFunction.Convertdbnulls(objDR2("UnsuccessReason"))
            UnsuccessOffSiteImpact = HelpFunction.Convertdbnulls(objDR2("UnsuccessOffSiteImpact"))
            UnsuccessOffSiteImpactText = HelpFunction.Convertdbnulls(objDR2("UnsuccessOffSiteImpactText"))
            'Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            'InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            'Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            'FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            LaunchLocation = HelpFunction.Convertdbnulls(objDR2("LaunchLocation"))
            LaunchLocationText = HelpFunction.Convertdbnulls(objDR2("LaunchLocationText"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Kennedy Space Center / Cape Canaveral AFS</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Kennedy Space Center / Cape Canaveral AFS</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "Other" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Launch Location:</b> ")
            strBody.Append("           " & LaunchLocation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If LaunchLocation = "Other" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Launch Location Description:</b> ")
                strBody.Append("           " & LaunchLocationText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        If MissionName <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Mission Name:</b> ")
            strBody.Append("           " & MissionName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "Initial Notification" Or SubType = "Rescheduled Launch" Then
            If InrlMissionLaunchDate <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Mission launch date:</b> ")
                strBody.Append("           " & InrlMissionLaunchDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Launch Window Start:</b> ")
                strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime2 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Launch Window End:</b> ")
                strBody.Append("           " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If InrlBrevardCo <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Brevard Co. Fire Rescue Staff report to KSC Morrell Operations Center:</b> ")
                strBody.Append("           " & InrlBrevardCo & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If InrlBrevardCo2 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Brevard Co. EOC Activation to Level 2 no later than:</b> ")
                strBody.Append("           " & InrlBrevardCo2 & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If NextMissionLaunchDate <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Next launch notification date:</b> ")
                strBody.Append("           " & NextMissionLaunchDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Scrubbed Launch" Then
            If ScrubDate <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Mission scrubbed date:</b> ")
                strBody.Append("           " & ScrubDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime3 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Mission scrubbed time:</b> ")
                strBody.Append("           " & Left(localTime3, 2) & ":" & Right(localTime3, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ScrubReason <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Reason:</b> ")
                strBody.Append("           " & ScrubReason & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ScrubNextLaunchDateTime <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Next launch notification date/time:</b> ")
                strBody.Append("           " & ScrubNextLaunchDateTime & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Successful Launch" Then
            If SuccessDate <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Launch date:</b> ")
                strBody.Append("           " & SuccessDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime4 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Launch time:</b> ")
                strBody.Append("           " & Left(localTime4, 2) & ":" & Right(localTime4, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Unsuccessful Launch" Then
            If UnsuccessDate <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Launch date:</b> ")
                strBody.Append("           " & UnsuccessDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime5 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Launch time:</b> ")
                strBody.Append("           " & Left(localTime5, 2) & ":" & Right(localTime5, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If UnsuccessReason <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Reason:</b> ")
                strBody.Append("           " & UnsuccessReason & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If UnsuccessOffSiteImpact <> "" And UnsuccessOffSiteImpact <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Off-site impact:</b> ")
                strBody.Append("           " & UnsuccessOffSiteImpact & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If UnsuccessOffSiteImpact = "Yes" Then

                If UnsuccessOffSiteImpactText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Description of area and hazards:</b> ")
                    strBody.Append("           " & UnsuccessOffSiteImpactText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

            End If

            'If Injury <> "" And Injury <> "Select an Option" Then
            '    strBody.Append("<table>")
            '    strBody.Append("    <tr>")
            '    strBody.Append("        <td align='left'width='650px'>")
            '    strBody.Append("            <b>Injuries:</b> ")
            '    strBody.Append("           " & Injury & "  ")
            '    strBody.Append("        </td>")
            '    strBody.Append("    </tr>")
            '    strBody.Append("</table>")
            'End If

            'If Injury = "Yes" Then
            '    If InjuryText <> "" Then
            '        strBody.Append("<table>")
            '        strBody.Append("    <tr>")
            '        strBody.Append("        <td align='left'width='650px'>")
            '        strBody.Append("            <b>Number and Severity of Injuries:</b> ")
            '        strBody.Append("           " & InjuryText & "  ")
            '        strBody.Append("        </td>")
            '        strBody.Append("    </tr>")
            '        strBody.Append("</table>")
            '    End If
            'End If

            'If Fatality <> "" And Fatality <> "Select an Option" Then
            '    strBody.Append("<table>")
            '    strBody.Append("    <tr>")
            '    strBody.Append("        <td align='left'width='650px'>")
            '    strBody.Append("            <b>Fatalities:</b> ")
            '    strBody.Append("           " & Fatality & "  ")
            '    strBody.Append("        </td>")
            '    strBody.Append("    </tr>")
            '    strBody.Append("</table>")
            'End If

            'If Fatality = "Yes" Then
            '    If FatalityText <> "" Then
            '        strBody.Append("<table>")
            '        strBody.Append("    <tr>")
            '        strBody.Append("        <td align='left'width='650px'>")
            '        strBody.Append("            <b>Number and location of fatalities:</b> ")
            '        strBody.Append("           " & FatalityText & "  ")
            '        strBody.Append("        </td>")
            '        strBody.Append("    </tr>")
            '        strBody.Append("</table>")
            '    End If
            'End If
        ElseIf SubType = "Other" Then

        End If
    End Sub

    Private Sub GetMarineIncident(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim VesselName As String = ""
        Dim VesselType As String = ""
        Dim HullLength As String = ""
        Dim Flag As String = ""
        Dim RegistrationNumber As String = ""
        Dim OwnedOperatedBy As String = ""
        Dim NumberPeopleOnboard As String = ""
        Dim IncidentCause As String = ""
        Dim Fire As String = ""
        'Dim Injury As String = ""
        'Dim InjuryText As String = ""
        'Dim Fatality As String = ""
        'Dim FatalityText As String = ""
        Dim HazardousMaterialsOnboard As String = ""
        Dim FuelPetroleumSpills As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectMarineIncidentByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            VesselName = HelpFunction.Convertdbnulls(objDR2("VesselName"))
            VesselType = HelpFunction.Convertdbnulls(objDR2("VesselType"))
            HullLength = HelpFunction.Convertdbnulls(objDR2("HullLength"))
            Flag = HelpFunction.Convertdbnulls(objDR2("Flag"))
            RegistrationNumber = HelpFunction.Convertdbnulls(objDR2("RegistrationNumber"))
            OwnedOperatedBy = HelpFunction.Convertdbnulls(objDR2("OwnedOperatedBy"))
            NumberPeopleOnboard = HelpFunction.Convertdbnulls(objDR2("NumberPeopleOnboard"))
            IncidentCause = HelpFunction.Convertdbnulls(objDR2("IncidentCause"))
            Fire = HelpFunction.Convertdbnulls(objDR2("Fire"))
            'Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            'InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            'Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            'FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            HazardousMaterialsOnboard = HelpFunction.Convertdbnulls(objDR2("HazardousMaterialsOnboard"))
            FuelPetroleumSpills = HelpFunction.Convertdbnulls(objDR2("FuelPetroleumSpills"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Marine Incident</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Marine Incident</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If VesselName <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Vessel Name:</b> ")
            strBody.Append("           " & VesselName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If VesselType <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Vessel Type:</b> ")
            strBody.Append("           " & VesselType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If HullLength <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Hull Length:</b> ")
            strBody.Append("           " & HullLength & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Flag <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Flag:</b> ")
            strBody.Append("           " & Flag & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If RegistrationNumber <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Registration Number:</b> ")
            strBody.Append("           " & RegistrationNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If OwnedOperatedBy <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Owned/Operated By:</b> ")
            strBody.Append("           " & OwnedOperatedBy & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If NumberPeopleOnboard <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Number of People Onboard (passengers/crew):</b> ")
            strBody.Append("           " & NumberPeopleOnboard & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If IncidentCause <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Cause of incident:</b> ")
            strBody.Append("           " & IncidentCause & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Fire <> "" And Fire <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Fire:</b> ")
            strBody.Append("           " & Fire & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'If Injury <> "" And Injury <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Injuries:</b> ")
        '    strBody.Append("           " & Injury & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Injury = "Yes" Then
        '    If InjuryText <> "" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and Severity of Injuries:</b> ")
        '        strBody.Append("           " & InjuryText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If

        'If Fatality <> "" And Fatality <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Fatalities:</b> ")
        '    strBody.Append("           " & Fatality & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Fatality = "Yes" Then
        '    If FatalityText <> "" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and location:</b> ")
        '        strBody.Append("           " & FatalityText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If

        If HazardousMaterialsOnboard <> "" And HazardousMaterialsOnboard <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Hazardous materials onboard:</b> ")
            strBody.Append("           " & HazardousMaterialsOnboard & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If FuelPetroleumSpills <> "" And FuelPetroleumSpills <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Fuel or Petroleum Spills:</b> ")
            strBody.Append("           " & FuelPetroleumSpills & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If
    End Sub

    Private Sub GetMigration(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim Migrants As String = ""
        Dim VesselNumber As String = ""
        Dim MigrantNumber As String = ""
        Dim CitizenshipEthnicity As String = ""
        Dim MigrantQuarantined As String = ""
        Dim MigrantQuarantinedText As String = ""
        'Dim Injury As String = ""
        'Dim InjuryText As String = ""
        'Dim Fatality As String = ""
        'Dim FatalityText As String = ""
        Dim ImmigrationNotified As String = ""
        Dim Facility As String = ""
        Dim SeverityLevel As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectMigrationByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            Migrants = HelpFunction.Convertdbnulls(objDR2("Migrants"))
            VesselNumber = HelpFunction.Convertdbnulls(objDR2("VesselNumber"))
            MigrantNumber = HelpFunction.Convertdbnulls(objDR2("MigrantNumber"))
            CitizenshipEthnicity = HelpFunction.Convertdbnulls(objDR2("CitizenshipEthnicity"))
            MigrantQuarantined = HelpFunction.Convertdbnulls(objDR2("MigrantQuarantined"))
            MigrantQuarantinedText = HelpFunction.Convertdbnulls(objDR2("MigrantQuarantinedText"))
            'Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            'InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            'Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            'FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            ImmigrationNotified = HelpFunction.Convertdbnulls(objDR2("ImmigrationNotified"))
            Facility = HelpFunction.Convertdbnulls(objDR2("Facility"))
            SeverityLevel = HelpFunction.Convertdbnulls(objDR2("SeverityLevel"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Migration</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Migration</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Migrants <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Migrants:</b> ")
            strBody.Append("           " & Migrants & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If VesselNumber <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Number of vessels:</b> ")
            strBody.Append("           " & VesselNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MigrantNumber <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Number of migrants:</b> ")
            strBody.Append("           " & MigrantNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If CitizenshipEthnicity <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Citizenship or ethnicity of the migrant(s):</b> ")
            strBody.Append("           " & CitizenshipEthnicity & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MigrantQuarantined <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Migrants been quarantined:</b> ")
            strBody.Append("           " & MigrantQuarantined & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MigrantQuarantinedText <> "" Then
            If MigrantQuarantined = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Location of Quarantined Migrants:</b> ")
                strBody.Append("           " & MigrantQuarantinedText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        'If Injury <> "" And Injury <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Injuries:</b> ")
        '    strBody.Append("           " & Injury & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If


        'If Injury = "Yes" Then
        '    If InjuryText <> "" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and Severity of Injuries:</b> ")
        '        strBody.Append("           " & InjuryText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If

        'If Fatality <> "" And Fatality <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Fatalities:</b> ")
        '    strBody.Append("           " & Fatality & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Fatality = "Yes" Then
        '    If FatalityText <> "" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and location of fatalities:</b> ")
        '        strBody.Append("           " & FatalityText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If

        If ImmigrationNotified <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>U.S. Customs and Border Protection notified:</b> ")
            strBody.Append("           " & ImmigrationNotified & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Facility <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Facility migrants are being held at:</b> ")
            strBody.Append("           " & Facility & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SeverityLevel <> "" And SeverityLevel <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Severity Level:</b> ")
            strBody.Append("           " & SeverityLevel & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

    End Sub

    Private Sub GetMilitaryActivity(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim ReportType As String = ""
        Dim LaunchDate As String = ""
        Dim localTime As String = ""
        Dim LaunchMessage As String = ""
        Dim FlightPath As String = ""
        Dim UnitConductingActivity As String = ""
        Dim ActivityDescription As String = ""
        Dim ActivityTimeDateRange As String = ""
        Dim AirspaceRestrictions As String = ""
        Dim RoadClosures As String = ""
        Dim ContactName As String = ""
        Dim ContactNumber As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectMilitaryActivityByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            ReportType = HelpFunction.Convertdbnulls(objDR2("ReportType"))
            LaunchDate = HelpFunction.Convertdbnulls(objDR2("LaunchDate"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR2("LaunchTime")))
            LaunchMessage = HelpFunction.Convertdbnulls(objDR2("LaunchMessage"))
            FlightPath = HelpFunction.Convertdbnulls(objDR2("FlightPath"))
            UnitConductingActivity = HelpFunction.Convertdbnulls(objDR2("UnitConductingActivity"))
            ActivityDescription = HelpFunction.Convertdbnulls(objDR2("ActivityDescription"))
            ActivityTimeDateRange = HelpFunction.Convertdbnulls(objDR2("ActivityTimeDateRange"))
            AirspaceRestrictions = HelpFunction.Convertdbnulls(objDR2("AirspaceRestrictions"))
            RoadClosures = HelpFunction.Convertdbnulls(objDR2("RoadClosures"))
            ContactName = HelpFunction.Convertdbnulls(objDR2("ContactName"))
            ContactNumber = HelpFunction.Convertdbnulls(objDR2("ContactNumber"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Military Activity</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Military Activity</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "Tomahawk Missile Launch" Then
            If ReportType <> "" And ReportType <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Type of report:</b> ")
                strBody.Append("           " & ReportType & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If LaunchDate <> "" And LaunchDate <> "1/1/1900" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Launch date:</b> ")
                strBody.Append("           " & LaunchDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Launch time:</b> ")
                strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If LaunchMessage <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Launch message:</b> ")
                strBody.Append("           " & LaunchMessage & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If FlightPath <> "" And FlightPath <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Flight path:</b> ")
                strBody.Append("           " & FlightPath & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        Else
            If UnitConductingActivity <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Unit conducting activity:</b> ")
                strBody.Append("           " & UnitConductingActivity & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ActivityDescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Description of the activity:</b> ")
                strBody.Append("           " & ActivityDescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ActivityTimeDateRange <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Time/date range of activity:</b> ")
                strBody.Append("           " & ActivityTimeDateRange & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If AirspaceRestrictions <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Airspace restrictions:</b> ")
                strBody.Append("           " & AirspaceRestrictions & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If RoadClosures <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Road closures:</b> ")
                strBody.Append("           " & RoadClosures & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ContactName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Point of Contact Name:</b> ")
                strBody.Append("           " & ContactName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ContactNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Point of Contact Number:</b> ")
                strBody.Append("           " & ContactNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If
    End Sub

    Private Sub GetNPP(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim CSTselectOne As String = ""
        Dim CSTdate As String = ""
        Dim localTime As String = ""
        Dim CSTreportedByName As String = ""
        Dim CSTmessageNumber As String = ""
        Dim CSTreportedFrom As String = ""
        Dim CSTfSelectOne As String = ""
        Dim CSTsite As String = ""
        Dim CSTemergencyClassification As String = ""
        Dim CSTdecTermSelectOne As String = ""
        Dim CSTdecTermDate As String = ""
        Dim localTime2 As String = ""
        Dim CSTdecTermReason As String = ""
        Dim CSTeALNumbers As String = ""
        Dim CSTeALDescription As String = ""
        Dim CSTeALai As String = ""
        Dim CSTeALaiDescription As String = ""
        Dim CSTwindDirectionDegrees As String = ""
        Dim CSTdownwindSectorsAffected As String = ""
        Dim CSTreleaseStatus As String = ""
        Dim CSTsigCatSiteBoundary As String = ""
        Dim CSTutilRecProtAct As String = ""
        Dim CSTevacuateZones As String = ""
        Dim CSTshelterZones As String = ""
        Dim CST02MilesEvacSect As String = ""
        Dim CST02MilesShelterSect As String = ""
        Dim CST02MilesNoActtionSect As String = ""
        Dim CST25MilesEvacSect As String = ""
        Dim CST25MilesShelterSect As String = ""
        Dim CST25MilesNoActtionSect As String = ""
        Dim CST510MilesEvacSect As String = ""
        Dim CST510MilesShelterSect As String = ""
        Dim CST510MilesNoActtionSect As String = ""
        Dim CST12A As String = ""
        Dim CST12B As String = ""
        Dim CST12C As String = ""
        Dim CST12D As String = ""
        Dim CST13A As String = ""
        Dim CSTProjThyroidDose As String = ""
        Dim CSTProjTotalDose As String = ""
        Dim CST13B As String = ""
        Dim CST14A As String = ""
        Dim CST14B As String = ""
        Dim CST14C As String = ""
        Dim CST14D As String = ""
        Dim CST14E As String = ""
        Dim CST14F As String = ""
        Dim CST14G As String = ""
        Dim CST14H As String = ""
        Dim CST14I As String = ""
        Dim CST15Name As String = ""
        Dim CST15Date As String = ""
        Dim localTime3 As String = ""
        Dim CSTuserComments As String = ""

        'Alabama.
        '---------------------------------------------------------------------------------------
        Dim Far1SelectOne As String = ""
        Dim Far1MessageNumber As String = ""
        Dim Far2SelectOne As String = ""
        Dim localTime4 As String = ""
        Dim Far2NotificationDate As String = ""
        Dim Far2AuthenticationNumber As String = ""
        Dim Far3Site As String = ""
        Dim Far3ConfirmationPhoneNumber As String = ""
        Dim Far4EmergencyClassification As String = ""
        Dim Far4BasedEALnumber As String = ""
        Dim Far4EALdescription As String = ""
        Dim Far5a As Boolean
        Dim Far5b As Boolean
        Dim Far5bText As String = ""
        Dim Far5c As Boolean
        Dim Far5cText As String = ""
        Dim Far5d As Boolean
        Dim Far5e As Boolean
        Dim Far5eText As String = ""
        Dim Far6EmergencyRelease As String = ""
        Dim Far7ReleaseSignificance As String = ""
        Dim Far8EventPrognosis As String = ""
        Dim Far9WindDirectDegrees As String = ""
        Dim Far9WindSpeed As String = ""
        Dim Far9Precipitation As String = ""
        Dim Far9StabilityClass As String = ""
        Dim Far10Select1 As String = ""
        Dim localTime5 As String = ""
        Dim Far10Date As String = ""
        Dim Far11AffectedUnits As String = ""
        Dim Far12AUnitPower As String = ""
        Dim localTime6 As String = ""
        Dim Far12ADate As String = ""
        Dim Far12BUnitPower As String = ""
        Dim localTime7 As String = ""
        Dim Far12BDate As String = ""
        Dim Far13Remarks As String = ""
        Dim Far14ReleaseChar As String = ""
        Dim Far14Units As String = ""
        Dim Far14NobleGasses As String = ""
        Dim Far14Iodines As String = ""
        Dim Far14Particulautes As String = ""
        Dim Far14Other As String = ""
        Dim Far14Aairborne As Boolean
        Dim localTime8 As String = ""
        Dim Far14AstartDate As String = ""
        Dim localTime9 As String = ""
        Dim Far14AstopDate As String = ""
        Dim Far14Bliquid As Boolean
        Dim localTime10 As String = ""
        Dim Far14BstartDate As String = ""
        Dim localTime11 As String = ""
        Dim Far14BendDate As String = ""
        Dim ReportType As String = ""
        Dim Far15ProjectionPeriod As String = ""
        Dim Far15EstimatedReleaseDuration As String = ""
        Dim localTime12 As String = ""
        Dim Far15ProjectionPerformedDate As String = ""
        Dim Far15AccidentType As String = ""
        Dim Far16SiteBoundaryTEDE As String = ""
        Dim Far16SiteBoundaryAdultThyroidCDE As String = ""
        Dim Far16TwoMilesTEDE As String = ""
        Dim Far16TwoMilesAdultThyroidCDE As String = ""
        Dim Far16FiveMilesTEDE As String = ""
        Dim Far16FiveMilesAdultThyroidCDE As String = ""
        Dim Far16TenMilesTEDE As String = ""
        Dim Far16MilesAdultThyroidCDE As String = ""
        Dim Far17ApprovedBy As String = ""
        Dim Far17Title As String = ""
        Dim localTime13 As String = ""
        Dim Far17Date As String = ""
        Dim Far17NotifiedBy As String = ""
        Dim Far17ReceivedBy As String = ""
        Dim localTime14 As String = ""
        Dim Far17ReceivedDate As String = ""
        '---------------------------------------------------------------------------------------
        'Crystal River Defueled Start==================================================
        Dim CRDselectOne As String = ""
        Dim CRDmessageClassification As String = ""
        Dim CRDdate As String = ""
        Dim CRDcontactTime As String = ""
        Dim CRDreportedByName As String = ""
        Dim CRDmessageNumber As String = ""
        Dim CRDfSelectOne As String = ""
        Dim CRDemergencyClassification As String = ""
        Dim CRDemClassDate As String = ""
        Dim CRDemClassTime As String = ""
        Dim CRDemTermDate As String = ""
        Dim CRDemTermTime As String = ""
        Dim CRDeALNumbers As String = ""
        Dim CRDeALDescription As String = ""
        Dim CRDeALai As String = ""
        Dim CRDeALaiDescription As String = ""
        Dim CRDwindDirectionDegrees As String = ""
        Dim CRDwindSpeed As String = ""
        Dim CRDreleaseStatus As String = ""
        Dim CRDreleaseSignificance As String = ""
        Dim CRDProjTotalDose As String = ""
        Dim CRDDistance83Mile As String = ""
        Dim CRDfacCond As String = ""
        Dim CRDmessageRecdName As String = ""
        Dim CRDmessageRecdDate As String = ""
        Dim CRDmessageRecdTime As String = ""
        Dim CRDuserComments As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectNPPByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            CSTselectOne = HelpFunction.Convertdbnulls(objDR2("CSTselectOne"))
            CSTdate = HelpFunction.Convertdbnulls(objDR2("CSTdate"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR2("CSTcontactTime")))
            CSTreportedByName = HelpFunction.Convertdbnulls(objDR2("CSTreportedByName"))
            CSTmessageNumber = HelpFunction.Convertdbnulls(objDR2("CSTmessageNumber"))
            CSTreportedFrom = HelpFunction.Convertdbnulls(objDR2("CSTreportedFrom"))
            CSTfSelectOne = HelpFunction.Convertdbnulls(objDR2("CSTfSelectOne"))
            CSTsite = HelpFunction.Convertdbnulls(objDR2("CSTsite"))
            CSTemergencyClassification = HelpFunction.Convertdbnulls(objDR2("CSTemergencyClassification"))
            CSTdecTermSelectOne = HelpFunction.Convertdbnulls(objDR2("CSTdecTermSelectOne"))
            CSTdecTermDate = HelpFunction.Convertdbnulls(objDR2("CSTdecTermDate"))
            localTime2 = CStr(HelpFunction.Convertdbnulls(objDR2("CSTdecTermTime")))
            CSTdecTermReason = HelpFunction.Convertdbnulls(objDR2("CSTdecTermReason"))
            CSTeALNumbers = HelpFunction.Convertdbnulls(objDR2("CSTeALNumbers"))
            CSTeALDescription = HelpFunction.Convertdbnulls(objDR2("CSTeALDescription"))
            CSTeALai = HelpFunction.Convertdbnulls(objDR2("CSTeALai"))
            CSTeALaiDescription = HelpFunction.Convertdbnulls(objDR2("CSTeALaiDescription"))
            CSTwindDirectionDegrees = HelpFunction.Convertdbnulls(objDR2("CSTwindDirectionDegrees"))
            CSTdownwindSectorsAffected = HelpFunction.Convertdbnulls(objDR2("CSTdownwindSectorsAffected"))
            CSTreleaseStatus = HelpFunction.Convertdbnulls(objDR2("CSTreleaseStatus"))
            CSTsigCatSiteBoundary = HelpFunction.Convertdbnulls(objDR2("CSTsigCatSiteBoundary"))
            CSTutilRecProtAct = HelpFunction.Convertdbnulls(objDR2("CSTutilRecProtAct"))
            CSTevacuateZones = HelpFunction.Convertdbnulls(objDR2("CSTevacuateZones"))
            CSTshelterZones = HelpFunction.Convertdbnulls(objDR2("CSTshelterZones"))
            CST02MilesEvacSect = HelpFunction.Convertdbnulls(objDR2("CST02MilesEvacSect"))
            CST02MilesShelterSect = HelpFunction.Convertdbnulls(objDR2("CST02MilesShelterSect"))
            CST02MilesNoActtionSect = HelpFunction.Convertdbnulls(objDR2("CST02MilesNoActtionSect"))
            CST25MilesEvacSect = HelpFunction.Convertdbnulls(objDR2("CST25MilesEvacSect"))
            CST25MilesShelterSect = HelpFunction.Convertdbnulls(objDR2("CST25MilesShelterSect"))
            CST25MilesNoActtionSect = HelpFunction.Convertdbnulls(objDR2("CST25MilesNoActtionSect"))
            CST510MilesEvacSect = HelpFunction.Convertdbnulls(objDR2("CST510MilesEvacSect"))
            CST510MilesShelterSect = HelpFunction.Convertdbnulls(objDR2("CST510MilesShelterSect"))
            CST510MilesNoActtionSect = HelpFunction.Convertdbnulls(objDR2("CST510MilesNoActtionSect"))
            CST12A = HelpFunction.Convertdbnulls(objDR2("CST12A"))
            CST12B = HelpFunction.Convertdbnulls(objDR2("CST12B"))
            CST12C = HelpFunction.Convertdbnulls(objDR2("CST12C"))
            CST12D = HelpFunction.Convertdbnulls(objDR2("CST12D"))
            CST13A = HelpFunction.Convertdbnulls(objDR2("CST13A"))
            CSTProjThyroidDose = HelpFunction.Convertdbnulls(objDR2("CSTProjThyroidDose"))
            CSTProjTotalDose = HelpFunction.Convertdbnulls(objDR2("CSTProjTotalDose"))
            CST13B = HelpFunction.Convertdbnulls(objDR2("CST13B"))
            CST14A = HelpFunction.Convertdbnulls(objDR2("CST14A"))
            CST14B = HelpFunction.Convertdbnulls(objDR2("CST14B"))
            CST14C = HelpFunction.Convertdbnulls(objDR2("CST14C"))
            CST14D = HelpFunction.Convertdbnulls(objDR2("CST14D"))
            CST14E = HelpFunction.Convertdbnulls(objDR2("CST14E"))
            CST14F = HelpFunction.Convertdbnulls(objDR2("CST14F"))
            CST14G = HelpFunction.Convertdbnulls(objDR2("CST14G"))
            CST14H = HelpFunction.Convertdbnulls(objDR2("CST14H"))
            CST14I = HelpFunction.Convertdbnulls(objDR2("CST14I"))
            CST15Name = HelpFunction.Convertdbnulls(objDR2("CST15Name"))
            CST15Date = HelpFunction.Convertdbnulls(objDR2("CST15Date"))
            localTime3 = CStr(HelpFunction.Convertdbnulls(objDR2("CST15Time")))
            CSTuserComments = HelpFunction.Convertdbnulls(objDR2("CSTuserComments"))

            'Alabama.
            '---------------------------------------------------------------------------------------
            Far1SelectOne = HelpFunction.Convertdbnulls(objDR2("Far1SelectOne"))
            Far1MessageNumber = HelpFunction.Convertdbnulls(objDR2("Far1MessageNumber"))
            Far2SelectOne = HelpFunction.Convertdbnulls(objDR2("Far2SelectOne"))
            localTime4 = CStr(HelpFunction.Convertdbnulls(objDR2("Far2NotificationTime")))
            Far2NotificationDate = HelpFunction.Convertdbnulls(objDR2("Far2NotificationDate"))
            Far2AuthenticationNumber = HelpFunction.Convertdbnulls(objDR2("Far2AuthenticationNumber"))
            Far3Site = HelpFunction.Convertdbnulls(objDR2("Far3Site"))
            Far3ConfirmationPhoneNumber = HelpFunction.Convertdbnulls(objDR2("Far3ConfirmationPhoneNumber"))
            Far4EmergencyClassification = HelpFunction.Convertdbnulls(objDR2("Far4EmergencyClassification"))
            Far4BasedEALnumber = HelpFunction.Convertdbnulls(objDR2("Far4BasedEALnumber"))
            Far4EALdescription = HelpFunction.Convertdbnulls(objDR2("Far4EALdescription"))
            Far5a = HelpFunction.ConvertdbnullsBool(objDR2("Far5a"))
            Far5b = HelpFunction.ConvertdbnullsBool(objDR2("Far5b"))
            Far5bText = HelpFunction.Convertdbnulls(objDR2("Far5bText"))
            Far5c = HelpFunction.ConvertdbnullsBool(objDR2("Far5c"))
            Far5cText = HelpFunction.Convertdbnulls(objDR2("Far5cText"))
            Far5d = HelpFunction.ConvertdbnullsBool(objDR2("Far5d"))
            Far5e = HelpFunction.ConvertdbnullsBool(objDR2("Far5e"))
            Far5eText = HelpFunction.Convertdbnulls(objDR2("Far5eText"))
            Far6EmergencyRelease = HelpFunction.Convertdbnulls(objDR2("Far6EmergencyRelease"))
            Far7ReleaseSignificance = HelpFunction.Convertdbnulls(objDR2("Far7ReleaseSignificance"))
            Far8EventPrognosis = HelpFunction.Convertdbnulls(objDR2("Far8EventPrognosis"))
            Far9WindDirectDegrees = HelpFunction.Convertdbnulls(objDR2("Far9WindDirectDegrees"))
            Far9WindSpeed = HelpFunction.Convertdbnulls(objDR2("Far9WindSpeed"))
            Far9Precipitation = HelpFunction.Convertdbnulls(objDR2("Far9Precipitation"))
            Far9StabilityClass = HelpFunction.Convertdbnulls(objDR2("Far9StabilityClass"))
            Far10Select1 = HelpFunction.Convertdbnulls(objDR2("Far10Select1"))
            localTime5 = CStr(HelpFunction.Convertdbnulls(objDR2("Far10Time")))
            Far10Date = HelpFunction.Convertdbnulls(objDR2("Far10Date"))
            Far11AffectedUnits = HelpFunction.Convertdbnulls(objDR2("Far11AffectedUnits"))
            Far12AUnitPower = HelpFunction.Convertdbnulls(objDR2("Far12AUnitPower"))
            localTime6 = CStr(HelpFunction.Convertdbnulls(objDR2("Far12ATime")))
            Far12ADate = HelpFunction.Convertdbnulls(objDR2("Far12ADate"))
            Far12BUnitPower = HelpFunction.Convertdbnulls(objDR2("Far12BUnitPower"))
            localTime7 = CStr(HelpFunction.Convertdbnulls(objDR2("Far12BTime")))
            Far12BDate = HelpFunction.Convertdbnulls(objDR2("Far12BDate"))
            Far13Remarks = HelpFunction.Convertdbnulls(objDR2("Far13Remarks"))
            Far14ReleaseChar = HelpFunction.Convertdbnulls(objDR2("Far14ReleaseChar"))
            Far14Units = HelpFunction.Convertdbnulls(objDR2("CST12D"))
            Far14NobleGasses = HelpFunction.Convertdbnulls(objDR2("Far14NobleGasses"))
            Far14Iodines = HelpFunction.Convertdbnulls(objDR2("Far14Iodines"))
            Far14Particulautes = HelpFunction.Convertdbnulls(objDR2("Far14Particulautes"))
            Far14Other = HelpFunction.Convertdbnulls(objDR2("Far14Other"))
            Far14Aairborne = HelpFunction.ConvertdbnullsBool(objDR2("Far14Aairborne"))
            localTime8 = CStr(HelpFunction.Convertdbnulls(objDR2("Far14AstartTime")))
            Far14AstartDate = HelpFunction.Convertdbnulls(objDR2("Far14AstartDate"))
            localTime9 = CStr(HelpFunction.Convertdbnulls(objDR2("Far14AstopTime")))
            Far14AstopDate = HelpFunction.Convertdbnulls(objDR2("Far14AstopDate"))
            Far14Bliquid = HelpFunction.ConvertdbnullsBool(objDR2("Far14Bliquid"))
            localTime10 = CStr(HelpFunction.Convertdbnulls(objDR2("Far14BstartTime")))
            Far14BstartDate = HelpFunction.Convertdbnulls(objDR2("Far14BstartDate"))
            localTime11 = CStr(HelpFunction.Convertdbnulls(objDR2("Far14BstopTime")))
            Far14BendDate = HelpFunction.Convertdbnulls(objDR2("Far14BendDate"))
            Far15ProjectionPeriod = HelpFunction.Convertdbnulls(objDR2("Far15ProjectionPeriod"))
            Far15EstimatedReleaseDuration = HelpFunction.Convertdbnulls(objDR2("Far15EstimatedReleaseDuration"))
            localTime12 = CStr(HelpFunction.Convertdbnulls(objDR2("Far15ProjectionPerformedTime")))
            Far15ProjectionPerformedDate = HelpFunction.Convertdbnulls(objDR2("Far15ProjectionPerformedDate"))
            Far15AccidentType = HelpFunction.Convertdbnulls(objDR2("Far15AccidentType"))
            Far16SiteBoundaryTEDE = HelpFunction.Convertdbnulls(objDR2("Far16SiteBoundaryTEDE"))
            Far16SiteBoundaryAdultThyroidCDE = HelpFunction.Convertdbnulls(objDR2("Far16SiteBoundaryAdultThyroidCDE"))
            Far16TwoMilesTEDE = HelpFunction.Convertdbnulls(objDR2("Far16TwoMilesTEDE"))
            Far16TwoMilesAdultThyroidCDE = HelpFunction.Convertdbnulls(objDR2("Far16TwoMilesAdultThyroidCDE"))
            Far16FiveMilesTEDE = HelpFunction.Convertdbnulls(objDR2("Far16FiveMilesTEDE"))
            Far16FiveMilesAdultThyroidCDE = HelpFunction.Convertdbnulls(objDR2("Far16FiveMilesAdultThyroidCDE"))
            Far16TenMilesTEDE = HelpFunction.Convertdbnulls(objDR2("Far16TenMilesTEDE"))
            Far16MilesAdultThyroidCDE = HelpFunction.Convertdbnulls(objDR2("Far16MilesAdultThyroidCDE"))
            Far17ApprovedBy = HelpFunction.Convertdbnulls(objDR2("Far17ApprovedBy"))
            Far17Title = HelpFunction.Convertdbnulls(objDR2("Far17Title"))
            localTime13 = CStr(HelpFunction.Convertdbnulls(objDR2("Far17Time")))
            Far17Date = HelpFunction.Convertdbnulls(objDR2("Far17Date"))
            Far17NotifiedBy = HelpFunction.Convertdbnulls(objDR2("Far17NotifiedBy"))
            Far17ReceivedBy = HelpFunction.Convertdbnulls(objDR2("Far17ReceivedBy"))
            localTime14 = CStr(HelpFunction.Convertdbnulls(objDR2("Far17ReceivedTime")))
            Far17ReceivedDate = HelpFunction.Convertdbnulls(objDR2("Far17ReceivedDate"))
            '---------------------------------------------------------------------------------------
            'Crystal River Defueled Start==================================================
            CRDselectOne = HelpFunction.Convertdbnulls(objDR2("CRDselectOne"))
            CRDmessageClassification = HelpFunction.Convertdbnulls(objDR2("CRDmessageClassification"))
            CRDdate = HelpFunction.Convertdbnulls(objDR2("CRDdate"))
            CRDcontactTime = CStr(HelpFunction.Convertdbnulls(objDR2("CRDcontactTime")))
            CRDreportedByName = HelpFunction.Convertdbnulls(objDR2("CRDreportedByName"))
            CRDmessageNumber = HelpFunction.Convertdbnulls(objDR2("CRDmessageNumber"))
            CRDfSelectOne = HelpFunction.Convertdbnulls(objDR2("CRDfSelectOne"))
            CRDemergencyClassification = HelpFunction.Convertdbnulls(objDR2("CRDemergencyClassification"))
            CRDemClassDate = HelpFunction.Convertdbnulls(objDR2("CRDemClassDate"))
            CRDemClassTime = CStr(HelpFunction.Convertdbnulls(objDR2("CRDemClassTime")))
            CRDemTermDate = HelpFunction.Convertdbnulls(objDR2("CRDemTermDate"))
            CRDemTermTime = CStr(HelpFunction.Convertdbnulls(objDR2("CRDemTermTime")))
            CRDeALNumbers = HelpFunction.Convertdbnulls(objDR2("CRDeALNumbers"))
            CRDeALDescription = HelpFunction.Convertdbnulls(objDR2("CRDeALDescription"))
            CRDeALai = HelpFunction.Convertdbnulls(objDR2("CRDeALai"))
            CRDeALaiDescription = HelpFunction.Convertdbnulls(objDR2("CRDeALaiDescription"))
            CRDwindDirectionDegrees = HelpFunction.Convertdbnulls(objDR2("CRDwindDirectionDegrees"))
            CRDwindSpeed = HelpFunction.Convertdbnulls(objDR2("CRDwindSpeed"))
            CRDreleaseStatus = HelpFunction.Convertdbnulls(objDR2("CRDreleaseStatus"))
            CRDreleaseSignificance = HelpFunction.Convertdbnulls(objDR2("CRDreleaseSignificance"))
            CRDProjTotalDose = HelpFunction.Convertdbnulls(objDR2("CRDProjTotalDose"))
            CRDDistance83Mile = HelpFunction.Convertdbnulls(objDR2("CRDDistance83Mile"))
            CRDfacCond = HelpFunction.Convertdbnulls(objDR2("CRDfacCond"))
            CRDmessageRecdName = HelpFunction.Convertdbnulls(objDR2("CRDmessageRecdName"))
            CRDmessageRecdDate = HelpFunction.Convertdbnulls(objDR2("CRDmessageRecdDate"))
            CRDmessageRecdTime = CStr(HelpFunction.Convertdbnulls(objDR2("CRDmessageRecdTime")))
            CRDuserComments = HelpFunction.Convertdbnulls(objDR2("CRDuserComments"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Nuclear Power Plant</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Nuclear Power Plant</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

       If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "Crystal River – Full ENF" Or SubType = "Saint Lucie" Or SubType = "Turkey Point" Then
            If CSTselectOne <> "" And CSTselectOne <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("           <b> " & CSTselectOne & " </b> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTdate <> "" And CSTdate <> "1/1/1900" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>2 A. Date:</b> ")
                strBody.Append("           " & CSTdate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>2 B. Contact Time:</b> ")
                strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTreportedByName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>2 C. Reported By (Name):</b> ")
                strBody.Append("           " & CSTreportedByName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTmessageNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>2 D. Message Number:</b> ")
                strBody.Append("           " & CSTmessageNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTreportedFrom <> "" And CSTreportedFrom <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>2 E. Reported From:</b> ")
                strBody.Append("           " & CSTreportedFrom & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTfSelectOne <> "" And CSTfSelectOne <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>2 F.:</b> ")
                strBody.Append("           " & CSTfSelectOne & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTsite <> "" And CSTsite <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>3. Site:</b> ")
                strBody.Append("           " & CSTsite & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTemergencyClassification <> "" And CSTemergencyClassification <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>4. Emergency Classification:</b> ")
                strBody.Append("           " & CSTemergencyClassification & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTdecTermSelectOne <> "" And CSTdecTermSelectOne <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>5.</b> ")
                strBody.Append("           " & CSTdecTermSelectOne & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTdecTermDate <> "" And CSTdecTermDate <> "1/1/1900" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>5. Date:</b> ")
                strBody.Append("           " & CSTdecTermDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime2 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>5. Time:</b> ")
                strBody.Append("           " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTdecTermReason <> "" And CSTdecTermReason <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>6. Reason for Emergency Declaration:</b> ")
                strBody.Append("           " & CSTdecTermReason & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTeALNumbers <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>6. EAL Number(s):</b> ")
                strBody.Append("           " & CSTeALNumbers & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTeALDescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>6. Description:</b> ")
                strBody.Append("           " & CSTeALDescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTeALai <> "" And CSTeALai <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>7. Additional Information:</b> ")
                strBody.Append("           " & CSTeALai & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTeALaiDescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>7. Description</b> ")
                strBody.Append("           " & CSTeALaiDescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTwindDirectionDegrees <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>8. Weather Data &nbsp; 8. A. Wind direction from degrees:</b> ")
                strBody.Append("           " & CSTwindDirectionDegrees & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTdownwindSectorsAffected <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>8. B. Downwind Sectors Affected:</b> ")
                strBody.Append("           " & CSTdownwindSectorsAffected & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTreleaseStatus <> "" And CSTreleaseStatus <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>9. Release Status:</b> ")
                strBody.Append("           " & CSTreleaseStatus & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTsigCatSiteBoundary <> "" And CSTsigCatSiteBoundary <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>10. Release Significance at Site Boundary:</b> ")
                strBody.Append("           " & CSTsigCatSiteBoundary & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTutilRecProtAct <> "" And CSTutilRecProtAct <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>11. Utility Recommended Protective Actions:</b> ")
                strBody.Append("           " & CSTutilRecProtAct & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTutilRecProtAct <> "A. No utility recommended actions at this time." Then
                If CSTevacuateZones <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Evacuate Zones:</b> ")
                    strBody.Append("           " & CSTevacuateZones & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CSTshelterZones <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Shelter Zones:</b> ")
                    strBody.Append("           " & CSTshelterZones & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CST02MilesEvacSect <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Evacuate Sectors 0-2 Miles:</b> ")
                    strBody.Append("           " & CST02MilesEvacSect & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CST25MilesEvacSect <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Evacuate Sectors 2-5 Miles:</b> ")
                    strBody.Append("           " & CST25MilesEvacSect & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CST510MilesEvacSect <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Evacuate Sectors 5-10 Miles:</b> ")
                    strBody.Append("           " & CST510MilesEvacSect & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CST02MilesShelterSect <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Shelter Sectors 0-2 Miles:</b> ")
                    strBody.Append("           " & CST02MilesShelterSect & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CST25MilesShelterSect <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Shelter Sectors 2-5 Miles:</b> ")
                    strBody.Append("           " & CST25MilesShelterSect & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CST510MilesShelterSect <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Shelter Sectors 5-10 Miles:</b> ")
                    strBody.Append("           " & CST510MilesShelterSect & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CST02MilesNoActtionSect <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Monitor & Prepare Sectors 0-2 Miles:</b> ")
                    strBody.Append("           " & CST02MilesNoActtionSect & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CST25MilesNoActtionSect <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Monitor & Prepare Sectors 2-5 Miles:</b> ")
                    strBody.Append("           " & CST25MilesNoActtionSect & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CST510MilesNoActtionSect <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Monitor & Prepare Sectors 5-10 Miles:</b> ")
                    strBody.Append("           " & CST510MilesNoActtionSect & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If CSTreportedFrom <> "Control Room" Then
                If CST12A <> "" And CST12A <> "Select an Option" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>12. Plant Conditions  12. A. Reactor Shutdown:</b> ")
                    strBody.Append("           " & CST12A & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CST12B <> "" And CST12B <> "Select an Option" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>12. B. Core Adequately Cooled:</b> ")
                    strBody.Append("           " & CST12B & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CST12C <> "" And CST12C <> "Select an Option" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>12. C. Containment Intact:</b> ")
                    strBody.Append("           " & CST12C & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CST12D <> "" And CST12D <> "Select an Option" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>12. D. Core Condition:</b> ")
                    strBody.Append("           " & CST12D & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CST13A <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>13. Weather Data  13. A. Wind Speed (MPH):</b> ")
                    strBody.Append("           " & CST13A & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CST13B <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>13. B. Stability Class:</b> ")
                    strBody.Append("           " & CST13B & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CST14A <> "" And CST14A <> "Select an Option" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>14 A. Additoinal Release Information:</b> ")
                    strBody.Append("           " & CST14A & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If CST14A = "As Below" Then
                    If CSTProjThyroidDose <> "" And CSTProjThyroidDose <> "" Then
                        strBody.Append("<table>")
                        strBody.Append("    <tr>")
                        strBody.Append("        <td align='left'width='650px'>")
                        strBody.Append("            <b>Distance:</b> ")

                        If CSTProjThyroidDose <> "" Then
                            strBody.Append("           Projected Thyroid Dose (CDE) for " & CSTProjThyroidDose & " hour(s), &nbsp;")
                        End If

                        If CSTProjTotalDose <> "" Then
                            strBody.Append("           Projected Total Dose (TEDE) for " & CSTProjTotalDose & " hour(s)  ")
                        End If

                        strBody.Append("        </td>")
                        strBody.Append("    </tr>")
                        strBody.Append("</table>")
                    Else
                        If CSTProjThyroidDose = "" And CSTProjTotalDose = "" Then
                            'Show Nothing.
                        Else
                            strBody.Append("<table>")
                            strBody.Append("    <tr>")
                            strBody.Append("        <td align='left'width='650px'>")
                            strBody.Append("            <b>Distance:</b> ")

                            If CSTProjThyroidDose <> "" Then
                                strBody.Append("           Projected Thyroid Dose (CDE) for " & CSTProjThyroidDose & " hour(s), &nbsp;")
                            End If

                            If CSTProjTotalDose <> "" Then
                                strBody.Append("           Projected Total Dose (TEDE) for " & CSTProjTotalDose & " hour(s)  ")
                            End If

                            strBody.Append("        </td>")
                            strBody.Append("    </tr>")
                            strBody.Append("</table>")
                        End If
                    End If

                    If CST14B <> "" And CST14C <> "" Then
                        strBody.Append("<table>")
                        strBody.Append("    <tr>")
                        strBody.Append("        <td align='left'width='650px'>")
                        strBody.Append("            <b>1 Mile (Site Boundary):</b> ")

                        If CST14B <> "" Then
                            strBody.Append("            B. " & CST14B & " mrem &nbsp;")
                        End If

                        If CST14C <> "" Then
                            strBody.Append("            C. " & CST14C & " mrem ")
                        End If

                        strBody.Append("        </td>")
                        strBody.Append("    </tr>")
                        strBody.Append("</table>")
                    Else
                        If CST14B = "" And CST14C = "" Then
                            'Show Nothing.
                        Else
                            strBody.Append("<table>")
                            strBody.Append("    <tr>")
                            strBody.Append("        <td align='left'width='650px'>")
                            strBody.Append("            <b>1 Mile (Site Boundary):</b> ")

                            If CST14B <> "" Then
                                strBody.Append("            B. " & CST14B & " mrem &nbsp;")
                            End If

                            If CST14C <> "" Then
                                strBody.Append("            C. " & CST14C & " mrem ")
                            End If

                            strBody.Append("        </td>")
                            strBody.Append("    </tr>")
                            strBody.Append("</table>")
                        End If
                    End If

                    If CST14D <> "" And CST14E <> "" Then
                        strBody.Append("<table>")
                        strBody.Append("    <tr>")
                        strBody.Append("        <td align='left'width='650px'>")
                        strBody.Append("            <b>2 Miles:</b> ")

                        If CST14D <> "" Then
                            strBody.Append("            D. " & CST14D & " mrem &nbsp;")
                        End If

                        If CST14E <> "" Then
                            strBody.Append("            E. " & CST14E & " mrem ")
                        End If

                        strBody.Append("        </td>")
                        strBody.Append("    </tr>")
                        strBody.Append("</table>")
                    Else
                        If CST14D = "" And CST14E = "" Then
                            'Show Nothing.
                        Else
                            strBody.Append("<table>")
                            strBody.Append("    <tr>")
                            strBody.Append("        <td align='left'width='650px'>")
                            strBody.Append("            <b>2 Miles:</b> ")

                            If CST14D <> "" Then
                                strBody.Append("            D. " & CST14D & " mrem &nbsp;")
                            End If

                            If CST14E <> "" Then
                                strBody.Append("            E. " & CST14E & " mrem ")
                            End If

                            strBody.Append("        </td>")
                            strBody.Append("    </tr>")
                            strBody.Append("</table>")
                        End If
                    End If

                    If CST14F <> "" And CST14G <> "" Then
                        strBody.Append("<table>")
                        strBody.Append("    <tr>")
                        strBody.Append("        <td align='left'width='650px'>")
                        strBody.Append("            <b>5 Miles:</b> ")

                        If CST14F <> "" Then
                            strBody.Append("            F. " & CST14F & " mrem &nbsp;")
                        End If

                        If CST14G <> "" Then
                            strBody.Append("            G. " & CST14G & " mrem ")
                        End If

                        strBody.Append("        </td>")
                        strBody.Append("    </tr>")
                        strBody.Append("</table>")
                    Else
                        If CST14F = "" And CST14G = "" Then
                            'Show Nothing.
                        Else
                            strBody.Append("<table>")
                            strBody.Append("    <tr>")
                            strBody.Append("        <td align='left'width='650px'>")
                            strBody.Append("            <b>5 Miles:</b> ")

                            If CST14F <> "" Then
                                strBody.Append("            F. " & CST14F & " mrem &nbsp;")
                            End If

                            If CST14G <> "" Then
                                strBody.Append("            G. " & CST14G & " mrem ")
                            End If

                            strBody.Append("        </td>")
                            strBody.Append("    </tr>")
                            strBody.Append("</table>")
                        End If
                    End If

                    If CST14H <> "" And CST14I <> "" Then

                        strBody.Append("<table>")
                        strBody.Append("    <tr>")
                        strBody.Append("        <td align='left'width='650px'>")
                        strBody.Append("            <b>10 Miles:</b> ")

                        If CST14H <> "" Then
                            strBody.Append("            H. " & CST14H & " mrem &nbsp;")
                        End If

                        If CST14I <> "" Then
                            strBody.Append("            I. " & CST14I & " mrem ")
                        End If

                        strBody.Append("        </td>")
                        strBody.Append("    </tr>")
                        strBody.Append("</table>")
                    Else
                        If CST14H = "" And CST14I = "" Then
                            'Show Nothing.
                        Else
                            strBody.Append("<table>")
                            strBody.Append("    <tr>")
                            strBody.Append("        <td align='left'width='650px'>")
                            strBody.Append("            <b>10 Miles:</b> ")

                            If CST14H <> "" Then
                                strBody.Append("            H. " & CST14H & " mrem &nbsp;")
                            End If

                            If CST14I <> "" Then
                                strBody.Append("            I. " & CST14I & " mrem ")
                            End If

                            strBody.Append("        </td>")
                            strBody.Append("    </tr>")
                            strBody.Append("</table>")
                        End If
                    End If
                End If
            End If

            If CST15Name <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>15. Message Received By:(Name):</b> ")
                strBody.Append("           " & CST15Name & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CST15Date <> "" And CST15Date <> "1/1/1900" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Date:</b> ")
                strBody.Append("           " & CST15Date & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime3 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Time:</b> ")
                strBody.Append("           " & Left(localTime3, 2) & ":" & Right(localTime3, 2) & " &nbsp;ET  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CSTuserComments <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>SWO User Comments:</b> ")
                strBody.Append("           " & CSTuserComments & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Farley" Then
            If Far1SelectOne <> "" And Far1SelectOne <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>1." & Far1SelectOne & "</b> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far1MessageNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Message #:</b> ")
                strBody.Append("           " & Far1MessageNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far2SelectOne <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>2." & Far2SelectOne & " </b> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime4 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Notification Time:</b> ")
                strBody.Append("           " & Left(localTime4, 2) & ":" & Right(localTime4, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far2NotificationDate <> "" And Far2NotificationDate <> "1/1/1900" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Date:</b> ")
                strBody.Append("           " & Far2NotificationDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far2AuthenticationNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Authentication #:</b> ")
                strBody.Append("           " & Far2AuthenticationNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far3Site <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Site:</b> ")
                strBody.Append("           " & Far3Site & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far3ConfirmationPhoneNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Confirmation Phone #:</b> ")
                strBody.Append("           " & Far3ConfirmationPhoneNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far4EmergencyClassification <> "" And Far4EmergencyClassification <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>4. Emergency Classification:</b> ")
                strBody.Append("           " & Far4EmergencyClassification & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far4BasedEALnumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Based on EAL #:</b> ")
                strBody.Append("           " & Far4BasedEALnumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far4EALdescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>EAL Description:</b> ")
                strBody.Append("           " & Far4EALdescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>5. Protective Action Recommendations</b>: ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Far5a = True Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <input type='checkbox' name='1' checked='checked' /> 5 A. None ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            ElseIf Far5a = False Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <input type='checkbox' name='1' /> 5 A. None ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far5b = True Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <input type='checkbox' name='2' checked='checked' /> 5. B. Evacuate ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            ElseIf Far5b = False Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <input type='checkbox' name='2' /> 5. B. Evacuate ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far5bText <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>5. B. Evacuate Description:</b> ")
                strBody.Append("           " & Far5bText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far5c = True Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <input type='checkbox' name='3' checked='checked' /> 5. C. Evacuate ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                strBody.Append("<td align='left'>  </td>")
            ElseIf Far5c = False Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <input type='checkbox' name='3' /> 5. C. Shelter ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                strBody.Append("<td align='left'>  </td>")
            End If

            If Far5cText <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>5. C. Shelter Description:</b> ")
                strBody.Append("           " & Far5cText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far5d = True Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <input type='checkbox' name='4' checked='checked' /> 5. D. Consider the use of KI in accordance with state plans and policy. ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            ElseIf Far5d = False Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <input type='checkbox' name='4' /> 5. D. Consider the use of KI in accordance with state plans and policy. ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far5e = True Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <input type='checkbox' name='5' checked='checked' /> 5. E. Other ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            ElseIf Far5e = False Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <input type='checkbox' name='5' /> 5. E. Other </td> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far5eText <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>5. E. Other Description:</b> ")
                strBody.Append("           " & Far5eText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far6EmergencyRelease <> "" And Far6EmergencyRelease <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>6.Emergency Release:</b> ")
                strBody.Append("           " & Far6EmergencyRelease & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far7ReleaseSignificance <> "" And Far7ReleaseSignificance <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>7. Release Significance:</b> ")
                strBody.Append("           " & Far7ReleaseSignificance & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far8EventPrognosis <> "" And Far8EventPrognosis <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>8. Event Prognosis:</b> ")
                strBody.Append("           " & Far8EventPrognosis & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>9. Meterological Data:</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Far9WindDirectDegrees <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("          Wind direction from " & Far9WindDirectDegrees & " degrees  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far9Precipitation <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("           Precipitation = " & Far9Precipitation & " ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far9WindSpeed <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("           Wind Speed " & Far9WindSpeed & " (mph) ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far9StabilityClass <> "" And Far9StabilityClass <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Stability Class:</b> ")
                strBody.Append("           " & Far9StabilityClass & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far10Select1 <> "" And Far10Select1 <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>10. " & Far10Select1 & "</b> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime5 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>10 Time:</b> ")
                strBody.Append("           " & Left(localTime5, 2) & ":" & Right(localTime5, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far10Date <> "" And Far10Date <> "1/1/1900" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>10 Date:</b> ")
                strBody.Append("           " & Far10Date & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far11AffectedUnits <> "" And Far11AffectedUnits <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>11. Affected Units:</b> ")
                strBody.Append("           " & Far11AffectedUnits & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>12. Unit Status:</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Far12AUnitPower <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            12. A. Unit 1 " & Far12AUnitPower & " % power ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime6 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Time = " & Left(localTime6, 2) & ":" & Right(localTime6, 2) & " ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far12ADate <> "" And Far12ADate <> "1/1/1900" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Date = " & Far12ADate & " ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            (Unaffected Unit(s) Status Not Required for Initial Notifications) ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Far12BUnitPower <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            12. B. Unit 2 " & Far12BUnitPower & " % power ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime7 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Time = " & Left(localTime7, 2) & ":" & Right(localTime7, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far12BDate <> "" And Far12BDate <> "1/1/1900" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Date = " & Far12BDate & " ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far13Remarks <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>13. Remarks:</b> ")
                strBody.Append("           " & Far13Remarks & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Information(Lines 14-16 not required for initial Notifications)</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Far14ReleaseChar <> "" And Far14ReleaseChar <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>14. Release Characterization: " & Far14ReleaseChar & "</b> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far14Units <> "" And Far14Units <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Units = ")
                strBody.Append("           " & Far14Units & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Magnitude:</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Far14NobleGasses <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Noble Gasses = ")
                strBody.Append("           " & Far14NobleGasses & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far14Iodines <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Iodines = ")
                strBody.Append("           " & Far14Iodines & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far14Particulautes <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Particulautes = ")
                strBody.Append("           " & Far14Particulautes & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far14Other <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Other = ")
                strBody.Append("           " & Far14Other & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far14Aairborne = True Or Far14Bliquid = True Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Form:</b> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far14Aairborne = True Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <input type='checkbox' name='6' checked='checked' /> A. Airborne: ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Start Time = ")
                strBody.Append("           " & Left(localTime8, 2) & ":" & Right(localTime8, 2) & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Date = ")
                strBody.Append("           " & Far14AstartDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Stop Time = ")
                strBody.Append("           " & Left(localTime9, 2) & ":" & Right(localTime9, 2) & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Date =")
                strBody.Append("           " & Far14AstopDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                'strBody.Append("<td align='left'>Form: <input type='checkbox' name='6' /> A. Airborne: </td>")
            End If

            If Far14Bliquid = True Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <input type='checkbox' name='7' checked='checked' /> B. Liquid: ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Start Time = ")
                strBody.Append("           " & Left(localTime10, 2) & ":" & Right(localTime10, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Date = ")
                strBody.Append("           " & Far14BstartDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Stop Time = ")
                strBody.Append("           " & Left(localTime11, 2) & ":" & Right(localTime11, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Date = ")
                strBody.Append("           " & Far14BendDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                'strBody.Append("<td align='left'> <input type='checkbox' name='7' /> B. Liquid: </td>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>15. Projection Parameters:</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Far15ProjectionPeriod <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Projection Period = ")
                strBody.Append("           " & Far15ProjectionPeriod & " (hours) ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far15EstimatedReleaseDuration <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Estimated Release Duration = ")
                strBody.Append("           " & Far15EstimatedReleaseDuration & " (hours) ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Projection Performed:</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If localTime12 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Time = ")
                strBody.Append("           " & Left(localTime12, 2) & ":" & Right(localTime12, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far15ProjectionPerformedDate <> "" And Far15ProjectionPerformedDate <> "1/1/1900" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Date = ")
                strBody.Append("           " & Far15ProjectionPerformedDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far15AccidentType <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Accident Type = ")
                strBody.Append("           " & Far15AccidentType & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>16. Projected Dose:</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Far16SiteBoundaryTEDE <> "" And Far16SiteBoundaryAdultThyroidCDE <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Site boundary:</b> ")

                If Far16SiteBoundaryTEDE <> "" Then
                    strBody.Append("            <b>TEDE(mrem)</b> = " & Far16SiteBoundaryTEDE & "  ")
                End If

                If Far16SiteBoundaryAdultThyroidCDE <> "" Then
                    strBody.Append("            <b>Adult Thyroid CDE(mrem)</b> = " & Far16SiteBoundaryAdultThyroidCDE & "  ")
                End If

                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            Else

                If Far16SiteBoundaryTEDE = "" And Far16SiteBoundaryAdultThyroidCDE = "" Then
                    'Show Nothing.
                Else
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Site boundary:</b> ")

                    If Far16SiteBoundaryTEDE <> "" Then
                        strBody.Append("            <b>TEDE(mrem)</b> = " & Far16SiteBoundaryTEDE & "  ")
                    End If

                    If Far16SiteBoundaryAdultThyroidCDE <> "" Then
                        strBody.Append("            <b>Adult Thyroid CDE(mrem)</b> = " & Far16SiteBoundaryAdultThyroidCDE & "  ")
                    End If

                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If Far16TwoMilesTEDE <> "" And Far16TwoMilesAdultThyroidCDE <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>2 Miles:</b> ")

                If Far16TwoMilesTEDE <> "" Then
                    strBody.Append("            <b>TEDE(mrem)</b> = " & Far16TwoMilesTEDE & "  ")
                End If

                If Far16TwoMilesAdultThyroidCDE <> "" Then
                    strBody.Append("            <b>Adult Thyroid CDE(mrem)</b> = " & Far16TwoMilesAdultThyroidCDE & "  ")
                End If

                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            Else

                If Far16TwoMilesTEDE = "" And Far16TwoMilesAdultThyroidCDE = "" Then
                    'Show Nothing.
                Else
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>2 Miles:</b> ")

                    If Far16TwoMilesTEDE <> "" Then
                        strBody.Append("            <b>TEDE(mrem)</b> = " & Far16TwoMilesTEDE & "  ")
                    End If

                    If Far16TwoMilesAdultThyroidCDE <> "" Then
                        strBody.Append("            <b>Adult Thyroid CDE(mrem)</b> = " & Far16TwoMilesAdultThyroidCDE & "  ")
                    End If

                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If Far16FiveMilesTEDE <> "" And Far16FiveMilesAdultThyroidCDE <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>5 Miles:</b> ")

                If Far16FiveMilesTEDE <> "" Then
                    strBody.Append("            <b>TEDE(mrem)</b> = " & Far16FiveMilesTEDE & "  ")
                End If

                If Far16FiveMilesAdultThyroidCDE <> "" Then
                    strBody.Append("            <b>Adult Thyroid CDE(mrem)</b> = " & Far16FiveMilesAdultThyroidCDE & "  ")
                End If

                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            Else
                If Far16FiveMilesTEDE = "" And Far16FiveMilesAdultThyroidCDE = "" Then
                    'Show Nothing.
                Else
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>5 Miles:</b> ")

                    If Far16FiveMilesTEDE <> "" Then
                        strBody.Append("            <b>TEDE(mrem)</b> = " & Far16FiveMilesTEDE & "  ")
                    End If

                    If Far16FiveMilesAdultThyroidCDE <> "" Then
                        strBody.Append("            <b>Adult Thyroid CDE(mrem)</b> = " & Far16FiveMilesAdultThyroidCDE & "  ")
                    End If

                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If Far16TenMilesTEDE <> "" And Far16MilesAdultThyroidCDE <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>10 Miles:</b> ")

                If Far16TenMilesTEDE <> "" Then
                    strBody.Append("            <b>TEDE(mrem)</b> = " & Far16TenMilesTEDE & "  ")
                End If

                If Far16MilesAdultThyroidCDE <> "" Then
                    strBody.Append("            <b>Adult Thyroid CDE(mrem)</b> = " & Far16MilesAdultThyroidCDE & "  ")
                End If

                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            Else
                If Far16TenMilesTEDE = "" And Far16MilesAdultThyroidCDE = "" Then
                    'Show Nothing.
                Else
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>10 Miles:</b> ")

                    If Far16TenMilesTEDE <> "" Then
                        strBody.Append("            <b>TEDE(mrem)</b> = " & Far16TenMilesTEDE & "  ")
                    End If

                    If Far16MilesAdultThyroidCDE <> "" Then
                        strBody.Append("            <b>Adult Thyroid CDE(mrem)</b> = " & Far16MilesAdultThyroidCDE & "  ")
                    End If

                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If Far17ApprovedBy <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>17. Approved By:</b> ")
                strBody.Append("           " & Far17ApprovedBy & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far17Title <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Title = ")
                strBody.Append("           " & Far17Title & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime13 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Time = ")
                strBody.Append("           " & Left(localTime13, 2) & ":" & Right(localTime13, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far17Date <> "" And Far17Date <> "1/1/1900" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Date = ")
                strBody.Append("           " & Far17Date & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far17NotifiedBy <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Notified By:</b> ")
                strBody.Append("           " & Far17NotifiedBy & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far17ReceivedBy <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Received By = ")
                strBody.Append("           " & Far17ReceivedBy & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime14 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Time = ")
                strBody.Append("           " & Left(localTime14, 2) & ":" & Right(localTime14, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far17ReceivedDate <> "" And Far17ReceivedDate <> "1/1/1900" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            Date = ")
                strBody.Append("           " & Far17ReceivedDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

        ElseIf SubType = "Crystal River – Permanently Defueled ENF" Then
            If CRDselectOne <> "" And CRDselectOne <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>1." & CRDselectOne & "</b> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDmessageClassification <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Message for:</b> ")
                strBody.Append("           " & CRDmessageClassification & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDdate <> "" And CRDdate <> "1/1/1900" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>A. Date:</b> ")
                strBody.Append("           " & CRDdate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDcontactTime <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>B. Contact Time:</b> ")
                strBody.Append("           " & Left(CRDcontactTime, 2) & ":" & Right(CRDcontactTime, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDreportedByName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>C. Reported by (Name):</b> ")
                strBody.Append("           " & CRDreportedByName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDmessageNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>D. Message #:</b> ")
                strBody.Append("           " & CRDmessageNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDfSelectOne <> "" And CRDfSelectOne <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>E." & CRDfSelectOne & "</b> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDemergencyClassification <> "" And CRDemergencyClassification <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>3. Emergency Classification:</b> ")
                strBody.Append("           " & CRDemergencyClassification & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDemClassDate <> "" And CRDemClassDate <> "1/1/1900" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Emergency Classification Date:</b> ")
                strBody.Append("           " & CRDemClassDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDemClassTime <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Emergency Classification Time:</b> ")
                strBody.Append("           " & Left(CRDemClassTime, 2) & ":" & Right(CRDemClassTime, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDemTermDate <> "" And CRDemTermDate <> "1/1/1900" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Emergency Termination Date:</b> ")
                strBody.Append("           " & CRDemTermDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDemTermTime <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Emergency Termination Time:</b> ")
                strBody.Append("           " & Left(CRDemTermTime, 2) & ":" & Right(CRDemTermTime, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>4. Reason for Emergency Declaration:</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If CRDeALNumbers <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("           <b>A. EAL Number(s):</b> " & CRDeALNumbers & " ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDeALDescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>EAL Description:</b> ")
                strBody.Append("           " & CRDeALDescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>5. Additional Information or Update:</b> ")

            If CRDeALai <> "" And CRDeALai <> "Select an Option" Then
                strBody.Append("           " & CRDeALai & "  ")
            End If

            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If CRDeALaiDescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Description:</b> ")
                strBody.Append("           " & CRDeALaiDescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>6. Weather Data:</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If CRDwindDirectionDegrees <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            A. Wind direction from degrees: ")
                strBody.Append("           " & CRDwindDirectionDegrees & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDwindSpeed <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>B. Wind speed MPH (m/sec x 2.24 = MPH):</b> ")
                strBody.Append("           " & CRDwindSpeed & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDreleaseStatus <> "" And CRDreleaseStatus <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>7. Release Status: " & CRDreleaseStatus & "</b> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDreleaseSignificance <> "" And CRDreleaseSignificance <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>8. Release Significance: (at the Exclusion Area Boundary) " & CRDreleaseSignificance & "</b> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>9. Additional Release Information:</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If CRDProjTotalDose <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>A. Projected Total Dose (TEDE) for</b> " & CRDProjTotalDose & " <b>hrs.</b> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDDistance83Mile <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>B. Distance of 0.83 Mile (Exclusion Area Boundary)</b> " & CRDDistance83Mile & " <b>mrem.</b> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>10. Facility Conditions</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If CRDfacCond <> "" And CRDfacCond <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>A. Spent Fuel Pool Adequately Cooled:</b> " & CRDfacCond & " ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDmessageRecdName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Message Received By (Name):</b> ")
                strBody.Append("           " & CRDmessageRecdName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDmessageRecdDate <> "" And CRDmessageRecdDate <> "1/1/1900" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Date</b> ")
                strBody.Append("           " & CRDmessageRecdDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDmessageRecdTime <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Time</b> ")
                strBody.Append("           " & Left(CRDmessageRecdTime, 2) & ":" & Right(CRDmessageRecdTime, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CRDuserComments <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>SWO User Comments:</b> ")
                strBody.Append("           " & CRDuserComments & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

        End If
    End Sub

    Private Sub GetPetroleumSpill(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim PetroleumType As String = ""
        Dim PetroleumNameDescription As String = ""
        Dim PetroleumOdor As String = ""
        Dim PetroleumColor As String = ""
        Dim PetroleumSourceContainer As String = ""
        Dim DiameterPipeline As String = ""
        Dim UnbrokenEndPipeConnectedTo As String = ""
        Dim RoadwayNameNumber As String = ""
        Dim TotalSourceContainerVolume As String = ""
        Dim PetroleumQuantityReleased As String = ""
        Dim PetroleumRateOfRelease As String = ""
        Dim PetroleumCauseOfRelease As String = ""
        Dim PetroleumlReleased As String = ""
        Dim localTime As String = ""
        Dim localTime2 As String = ""
        Dim StormDrainsAffected As String = ""
        Dim WaterwaysAffected As String = ""
        Dim WaterwaysAffectedText As String = ""
        Dim MajorRoadwaysClosed As String = ""
        Dim CleanupActionsTaken As String = ""
        Dim CleanupActionsTakenText As String = ""
        Dim ConductingCleanup As String = ""
        'Dim CallbackDEPRequested As String = ""
        'Dim CallbackDEPRequestedValue As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectPetroleumSpillByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            PetroleumType = HelpFunction.Convertdbnulls(objDR2("PetroleumType"))
            PetroleumNameDescription = HelpFunction.Convertdbnulls(objDR2("PetroleumNameDescription"))
            PetroleumOdor = HelpFunction.Convertdbnulls(objDR2("PetroleumOdor"))
            PetroleumColor = HelpFunction.Convertdbnulls(objDR2("PetroleumColor"))
            PetroleumSourceContainer = HelpFunction.Convertdbnulls(objDR2("PetroleumSourceContainer"))
            DiameterPipeline = HelpFunction.Convertdbnulls(objDR2("DiameterPipeline"))
            UnbrokenEndPipeConnectedTo = HelpFunction.Convertdbnulls(objDR2("UnbrokenEndPipeConnectedTo"))
            TotalSourceContainerVolume = HelpFunction.Convertdbnulls(objDR2("TotalSourceContainerVolume"))
            PetroleumQuantityReleased = HelpFunction.Convertdbnulls(objDR2("PetroleumQuantityReleased"))
            PetroleumRateOfRelease = HelpFunction.Convertdbnulls(objDR2("PetroleumRateOfRelease"))
            PetroleumCauseOfRelease = HelpFunction.Convertdbnulls(objDR2("PetroleumCauseOfRelease"))
            PetroleumlReleased = HelpFunction.Convertdbnulls(objDR2("PetroleumlReleased"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR2("TimeReleaseDiscovered")))
            localTime2 = CStr(HelpFunction.Convertdbnulls(objDR2("TimeReleaseSecured")))
            StormDrainsAffected = HelpFunction.Convertdbnulls(objDR2("StormDrainsAffected"))
            WaterwaysAffected = HelpFunction.Convertdbnulls(objDR2("WaterwaysAffected"))
            WaterwaysAffectedText = HelpFunction.Convertdbnulls(objDR2("WaterwaysAffectedText"))
            MajorRoadwaysClosed = HelpFunction.Convertdbnulls(objDR2("MajorRoadwaysClosed"))
            CleanupActionsTaken = HelpFunction.Convertdbnulls(objDR2("CleanupActionsTaken"))
            CleanupActionsTakenText = HelpFunction.Convertdbnulls(objDR2("CleanupActionsTakenText"))
            ConductingCleanup = HelpFunction.Convertdbnulls(objDR2("ConductingCleanup"))
            'CallbackDEPRequested = HelpFunction.Convertdbnulls(objDR2("CallbackDEPRequested"))
            'CallbackDEPRequestedValue = HelpFunction.Convertdbnulls(objDR2("CallbackDEPRequestedValue"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Petroleum Spill</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Petroleum Spill</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If PetroleumType <> "" Then
            If PetroleumType <> "" And PetroleumType <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Petroleum Type:</b> ")
                strBody.Append("           " & PetroleumType & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        If PetroleumNameDescription <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Name or Description:</b> ")
            strBody.Append("           " & PetroleumNameDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If PetroleumOdor <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Odor:</b> ")
            strBody.Append("           " & PetroleumOdor & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If PetroleumColor <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Color:</b> ")
            strBody.Append("           " & PetroleumColor & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If PetroleumSourceContainer <> "" And PetroleumSourceContainer <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Source / Container:</b> ")
            strBody.Append("           " & PetroleumSourceContainer & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If PetroleumSourceContainer = "Aboveground Pipeline" Or PetroleumSourceContainer = "Underground Pipeline" Then
            If DiameterPipeline <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Diameter of the Pipeline:</b> ")
                strBody.Append("           " & DiameterPipeline & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If UnbrokenEndPipeConnectedTo <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Unbroken end of the pipe connected to:</b> ")
                strBody.Append("           " & UnbrokenEndPipeConnectedTo & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        If TotalSourceContainerVolume <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Total source/container volume:</b> ")
            strBody.Append("           " & TotalSourceContainerVolume & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If PetroleumQuantityReleased <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Quantity released:</b> ")
            strBody.Append("           " & PetroleumQuantityReleased & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If PetroleumRateOfRelease <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Rate of release:</b> ")
            strBody.Append("           " & PetroleumRateOfRelease & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If PetroleumlReleased <> "" And PetroleumlReleased <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Released:</b> ")
            strBody.Append("           " & PetroleumlReleased & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If PetroleumCauseOfRelease <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Cause of release:</b> ")
            strBody.Append("           " & PetroleumCauseOfRelease & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If localTime <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Time the release was discovered:</b> ")
            strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If localTime2 <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Time the release was secured:</b> ")
            strBody.Append("           " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If StormDrainsAffected <> "" And StormDrainsAffected <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Storm drains affected:</b> ")
            strBody.Append("           " & StormDrainsAffected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If WaterwaysAffected <> "" And WaterwaysAffected <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Waterways affected:</b> ")
            strBody.Append("           " & WaterwaysAffected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If WaterwaysAffected = "Yes" Then
            If WaterwaysAffectedText <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Name(s) of waterways:</b> ")
                strBody.Append("           " & WaterwaysAffectedText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        If MajorRoadwaysClosed <> "" And MajorRoadwaysClosed <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Major roadways closed:</b> ")
            strBody.Append("           " & MajorRoadwaysClosed & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If CleanupActionsTaken <> "" And CleanupActionsTaken <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Cleanup actions been taken:</b> ")
            strBody.Append("           " & CleanupActionsTaken & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If CleanupActionsTaken = "Yes" Then
            If CleanupActionsTakenText <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>List cleanup actions:</b> ")
                strBody.Append("           " & CleanupActionsTakenText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        If ConductingCleanup <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Person conducting cleanup:</b> ")
            strBody.Append("           " & ConductingCleanup & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'If CallbackDEPRequested <> "" And CallbackDEPRequested <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Callback from DEP requested:</b> ")
        '    strBody.Append("           " & CallbackDEPRequested & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If CallbackDEPRequested = "Yes" Then
        '    If CallbackDEPRequestedValue <> "" And CallbackDEPRequestedValue <> "Select an Option" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Contact:</b> ")
        '        strBody.Append("           " & CallbackDEPRequestedValue & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If
    End Sub

    Private Sub GetPopProtAction(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim ImpactedStreetLandmark As String = ""
        Dim DeptAgencyIssuingOrder As String = ""
        Dim Duration As String = ""
        Dim ImpactResidenceNum As String = ""
        Dim ImpactBusinessNum As String = ""
        Dim TotalImpacted As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectPopProtActionByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            ImpactedStreetLandmark = HelpFunction.Convertdbnulls(objDR2("ImpactedStreetLandmark"))
            DeptAgencyIssuingOrder = HelpFunction.Convertdbnulls(objDR2("DeptAgencyIssuingOrder"))
            Duration = HelpFunction.Convertdbnulls(objDR2("Duration"))
            ImpactResidenceNum = HelpFunction.Convertdbnulls(objDR2("ImpactResidenceNum"))
            ImpactBusinessNum = HelpFunction.Convertdbnulls(objDR2("ImpactBusinessNum"))
            TotalImpacted = HelpFunction.Convertdbnulls(objDR2("TotalImpacted"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Population Protection Actions</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Population Protection Actions</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "Shelter in place" Or SubType = "Evacuation Order" Then
            If ImpactedStreetLandmark <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Impacted area:</b> ")
                strBody.Append("           " & ImpactedStreetLandmark & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If DeptAgencyIssuingOrder <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Department/Agency issuing the order:</b> ")
                strBody.Append("           " & DeptAgencyIssuingOrder & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Duration <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Duration of the order:</b> ")
                strBody.Append("           " & Duration & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ImpactResidenceNum <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Number of residences impacted:</b> ")
                strBody.Append("           " & ImpactResidenceNum & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If ImpactBusinessNum <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Number of businesses impacted:</b> ")
                strBody.Append("           " & ImpactBusinessNum & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TotalImpacted <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Total number of individuals impacted:</b> ")
                strBody.Append("           " & TotalImpacted & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        Else
            objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            DBConStringHelper.PrepareConnection(objConn2) 'open the connection
            objCmd2 = New SqlCommand("spSelectShelterByIncidentID", objConn2)
            objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
            objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)
            objCmd2.CommandType = CommandType.StoredProcedure
            objDR2 = objCmd2.ExecuteReader()

            If objDR2.HasRows Then
                'There are records.
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <u><b>Shelters Open</b></u> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                While objDR2.Read
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Shelter Name:</b> ")
                    strBody.Append("           " & objDR2.Item("ShelterName") & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")

                    If Not String.IsNullOrEmpty(objDR2.Item("Address").ToString) Then
                        strBody.Append("    <tr>")
                        strBody.Append("        <td align='left'width='650px'>")
                        strBody.Append("            &nbsp;&nbsp;<b>Address:</b> ")
                        strBody.Append("           " & objDR2.Item("Address") & "  ")
                        strBody.Append("        </td>")
                        strBody.Append("    </tr>")
                    End If

                    If Not String.IsNullOrEmpty(objDR2.Item("City").ToString) Then
                        strBody.Append("    <tr>")
                        strBody.Append("        <td align='left'width='650px'>")
                        strBody.Append("            &nbsp;&nbsp;<b>City:</b> ")
                        strBody.Append("           " & objDR2.Item("City") & "  ")
                        strBody.Append("        </td>")
                        strBody.Append("    </tr>")
                    End If

                    If Not String.IsNullOrEmpty(objDR2.Item("Zip").ToString) Then
                        strBody.Append("    <tr>")
                        strBody.Append("        <td align='left'width='650px'>")
                        strBody.Append("            &nbsp;&nbsp;<b>Zip:</b> ")
                        strBody.Append("           " & objDR2.Item("Zip") & "  ")
                        strBody.Append("        </td>")
                        strBody.Append("    </tr>")
                    End If

                    If Not String.IsNullOrEmpty(objDR2.Item("OperationHours").ToString) Then
                        strBody.Append("    <tr>")
                        strBody.Append("        <td align='left'width='650px'>")
                        strBody.Append("            &nbsp;&nbsp;<b>Hours of operation:</b> ")
                        strBody.Append("           " & objDR2.Item("OperationHours") & "  ")
                        strBody.Append("        </td>")
                        strBody.Append("    </tr>")
                    End If

                    If Not String.IsNullOrEmpty(objDR2.Item("ContactInformation").ToString) Then
                        strBody.Append("    <tr>")
                        strBody.Append("        <td align='left'width='650px'>")
                        strBody.Append("            &nbsp;&nbsp;<b>Contact information:</b> ")
                        strBody.Append("           " & objDR2.Item("ContactInformation") & "  ")
                        strBody.Append("        </td>")
                        strBody.Append("    </tr>")
                    End If

                    If Not String.IsNullOrEmpty(objDR2.Item("ShelterCapacity").ToString) Then
                        strBody.Append("    <tr>")
                        strBody.Append("        <td align='left'width='650px'>")
                        strBody.Append("            &nbsp;&nbsp;<b>Shelter capacity:</b> ")
                        strBody.Append("           " & objDR2.Item("ShelterCapacity") & "  ")
                        strBody.Append("        </td>")
                        strBody.Append("    </tr>")
                    End If

                    If Not String.IsNullOrEmpty(objDR2.Item("CurrentPopulation").ToString) Then
                        strBody.Append("    <tr>")
                        strBody.Append("        <td align='left'width='650px'>")
                        strBody.Append("            &nbsp;&nbsp;<b>Current population:</b> ")
                        strBody.Append("           " & objDR2.Item("CurrentPopulation") & "  ")
                        strBody.Append("        </td>")
                        strBody.Append("    </tr>")
                    End If

                    strBody.Append("</table>")
                End While

                strBody.Append("</table>")
            End If

            objCmd2.Dispose()
            objCmd2 = Nothing
            objConn2.Close()

            'strBody.Append("<table width='100%' align='center'>")
            'strBody.Append("<tr>")
            'strBody.Append("<td width='33%' align='left'> What crop(s) are affected? " & ADCFcropsAffected & "</font></td>")
            'strBody.Append("<td width='33%' align='left'> What type of disease, if known? " & ADCFdiseaseType & "</font></td>")
            'strBody.Append("<td width='33%' align='left'> Number of acres affected? " & ADCFacresAffected & "</font></td>")
            'strBody.Append("</tr>")
            'strBody.Append("</table>")
        End If
    End Sub

    Private Sub GetPublicHealthMedical(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim IDRdiseaseType As String = ""
        Dim IDRpeopleInfectedNumber As String = ""
        Dim IDRexamTest As String = ""
        Dim IDRquarantineEffect As String = ""
        Dim IDRquarantineEffectText As String = ""
        Dim IDRfatality As String = ""
        Dim IDRfatalityText As String = ""
        Dim IDRdOHrequested As String = ""
        Dim IDRdOHrequestedText As String = ""
        Dim PHHOhazardDescription As String = ""
        Dim PHHOdOHRequested As String = ""
        Dim PHHOdOHRequestedText As String = ""
        Dim MCIpatientNumber As String = ""
        Dim MCIcritical As String = ""
        Dim MCIimmediate As String = ""
        Dim MCIdelayed As String = ""
        Dim MCIdeceased As String = ""
        Dim MCItTA As String = ""
        Dim MCIagencyCoordinating As String = ""
        Dim MCIunmetNeeds As String = ""
        Dim MCIunmetNeedsText As String = ""
        Dim MCIdOHRequested As String = ""
        Dim MCIdOHRequestedText As String = ""
        Dim IHFpatientsAffectedNumber As String = ""
        Dim IHFfacilityDamaged As String = ""
        Dim IHFfacilityDamagedText As String = ""
        Dim IHFfacilityEvacuated As String = ""
        Dim IHFfacilityEvacuatedText As String = ""
        Dim IHFunmetNeeds As String = ""
        Dim IHFunmetNeedsText As String = ""
        Dim IHFcallbackRequested As String = ""
        Dim IHFcallbackRequestedText As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectPublicHealthMedicalByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            IDRdiseaseType = HelpFunction.Convertdbnulls(objDR2("IDRdiseaseType"))
            IDRpeopleInfectedNumber = HelpFunction.Convertdbnulls(objDR2("IDRpeopleInfectedNumber"))
            IDRexamTest = HelpFunction.Convertdbnulls(objDR2("IDRexamTest"))
            IDRquarantineEffect = HelpFunction.Convertdbnulls(objDR2("IDRquarantineEffect"))
            IDRquarantineEffectText = HelpFunction.Convertdbnulls(objDR2("IDRquarantineEffectText"))
            IDRfatality = HelpFunction.Convertdbnulls(objDR2("IDRfatality"))
            IDRfatalityText = HelpFunction.Convertdbnulls(objDR2("IDRfatalityText"))
            IDRdOHrequested = HelpFunction.Convertdbnulls(objDR2("IDRdOHrequested"))
            IDRdOHrequestedText = HelpFunction.Convertdbnulls(objDR2("IDRdOHrequestedText"))
            PHHOhazardDescription = HelpFunction.Convertdbnulls(objDR2("PHHOhazardDescription"))
            PHHOdOHRequested = HelpFunction.Convertdbnulls(objDR2("PHHOdOHRequested"))
            PHHOdOHRequestedText = HelpFunction.Convertdbnulls(objDR2("PHHOdOHRequestedText"))
            MCIpatientNumber = HelpFunction.Convertdbnulls(objDR2("MCIpatientNumber"))
            MCIcritical = HelpFunction.Convertdbnulls(objDR2("MCIcritical"))
            MCIimmediate = HelpFunction.Convertdbnulls(objDR2("MCIimmediate"))
            MCIdelayed = HelpFunction.Convertdbnulls(objDR2("MCIdelayed"))
            MCIdeceased = HelpFunction.Convertdbnulls(objDR2("MCIdeceased"))
            MCItTA = HelpFunction.Convertdbnulls(objDR2("MCItTA"))
            MCIagencyCoordinating = HelpFunction.Convertdbnulls(objDR2("MCIagencyCoordinating"))
            MCIunmetNeeds = HelpFunction.Convertdbnulls(objDR2("MCIunmetNeeds"))
            MCIunmetNeedsText = HelpFunction.Convertdbnulls(objDR2("MCIunmetNeedsText"))
            MCIdOHRequested = HelpFunction.Convertdbnulls(objDR2("MCIdOHRequested"))
            MCIdOHRequestedText = HelpFunction.Convertdbnulls(objDR2("MCIdOHRequestedText"))
            IHFpatientsAffectedNumber = HelpFunction.Convertdbnulls(objDR2("IHFpatientsAffectedNumber"))
            IHFfacilityDamaged = HelpFunction.Convertdbnulls(objDR2("IHFfacilityDamaged"))
            IHFfacilityDamagedText = HelpFunction.Convertdbnulls(objDR2("IHFfacilityDamagedText"))
            IHFfacilityEvacuated = HelpFunction.Convertdbnulls(objDR2("IHFfacilityEvacuated"))
            IHFfacilityEvacuatedText = HelpFunction.Convertdbnulls(objDR2("IHFfacilityEvacuatedText"))
            IHFunmetNeeds = HelpFunction.Convertdbnulls(objDR2("IHFunmetNeeds"))
            IHFunmetNeedsText = HelpFunction.Convertdbnulls(objDR2("IHFunmetNeedsText"))
            IHFcallbackRequested = HelpFunction.Convertdbnulls(objDR2("IHFcallbackRequested"))
            IHFcallbackRequestedText = HelpFunction.Convertdbnulls(objDR2("IHFcallbackRequestedText"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Public Health Medical</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Public Health Medical</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "Infectious Disease Report" Then
            If IDRdiseaseType <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Type of disease:</b> ")
                strBody.Append("           " & IDRdiseaseType & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If IDRpeopleInfectedNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Number of people infected:</b> ")
                strBody.Append("           " & IDRpeopleInfectedNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If IDRexamTest <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Tests/Examinations that are planned or occurring:</b> ")
                strBody.Append("           " & IDRexamTest & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If IDRquarantineEffect <> "" And IDRquarantineEffect <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Quarantine in effect:</b> ")
                strBody.Append("           " & IDRquarantineEffect & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If IDRquarantineEffect = "Yes" Then
                If IDRquarantineEffectText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Area Description:</b> ")
                    strBody.Append("           " & IDRquarantineEffectText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If IDRfatality <> "" And IDRfatality <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Fatalities:</b> ")
                strBody.Append("           " & IDRfatality & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If IDRfatality = "Yes" Then
                If IDRfatalityText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Number and location of fatalities:</b> ")
                    strBody.Append("           " & IDRfatalityText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If IDRdOHrequested <> "" And IDRdOHrequested <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Callback from DOH Requested:</b> ")
                strBody.Append("           " & IDRdOHrequested & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If IDRdOHrequested = "Yes" Then
                If IDRdOHrequestedText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Contact:</b> ")
                    strBody.Append("           " & IDRdOHrequestedText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If
        ElseIf SubType = "Public Health Hazard" Or SubType = "Other" Then
            If PHHOhazardDescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Describe the hazard:</b> ")
                strBody.Append("           " & PHHOhazardDescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If PHHOdOHRequested <> "" And PHHOdOHRequested <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Callback from DOH Requested:</b> ")
                strBody.Append("           " & PHHOdOHRequested & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If PHHOdOHRequested = "Yes" Then
                If PHHOdOHRequestedText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Contact:</b> ")
                    strBody.Append("           " & PHHOdOHRequestedText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If
        ElseIf SubType = "Mass Casualty Incident" Then
            If MCIpatientNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Number of Patients:</b> ")
                strBody.Append("           " & MCIpatientNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If MCIcritical <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Critical:</b> ")
                strBody.Append("           " & MCIcritical & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If MCIimmediate <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Immediate:</b> ")
                strBody.Append("           " & MCIimmediate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If MCIdelayed <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Delayed:</b> ")
                strBody.Append("           " & MCIdelayed & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If MCIdeceased <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Deceased:</b> ")
                strBody.Append("           " & MCIdeceased & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If MCItTA <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Location of Triage/Treatment Area(s):</b> ")
                strBody.Append("           " & MCItTA & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If MCIagencyCoordinating <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Agency coordinating the MCI:</b> ")
                strBody.Append("           " & MCIagencyCoordinating & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If MCIunmetNeeds <> "" And MCIunmetNeeds <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Unmet needs:</b> ")
                strBody.Append("           " & MCIunmetNeeds & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If MCIunmetNeeds = "Yes" Then
                If MCIunmetNeedsText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Needs description:</b> ")
                    strBody.Append("           " & MCIunmetNeedsText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If MCIdOHRequested <> "" And MCIdOHRequested <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Callback from DOH Requested:</b> ")
                strBody.Append("           " & MCIdOHRequested & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If MCIdOHRequested = "Yes" Then
                If MCIdOHRequestedText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Contact:</b> ")
                    strBody.Append("           " & MCIdOHRequestedText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If
        ElseIf SubType = "Impact to Healthcare Facility" Then
            If IHFpatientsAffectedNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Number of Patients Affected:</b> ")
                strBody.Append("           " & IHFpatientsAffectedNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If IHFfacilityDamaged <> "" And IHFfacilityDamaged <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Facility damaged:</b> ")
                strBody.Append("           " & IHFfacilityDamaged & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If IHFfacilityDamaged = "Yes" Then
                If IHFfacilityDamagedText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Damage description:</b> ")
                    strBody.Append("           " & IHFfacilityDamagedText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If IHFfacilityEvacuated <> "" And IHFfacilityEvacuated <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Facility being evacuated:</b> ")
                strBody.Append("           " & IHFfacilityEvacuated & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If IHFfacilityEvacuated = "Yes" Then
                If IHFfacilityEvacuatedText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Evacuees being taken to:</b> ")
                    strBody.Append("           " & IHFfacilityEvacuatedText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If IHFunmetNeeds <> "" And IHFunmetNeeds <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Unmet needs:</b> ")
                strBody.Append("           " & IHFunmetNeeds & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If IHFunmetNeeds = "Yes" Then
                If IHFunmetNeedsText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Unmet needs description:</b> ")
                    strBody.Append("           " & IHFunmetNeedsText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If IHFcallbackRequested <> "" And IHFcallbackRequested <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Callback from DOH Requested:</b> ")
                strBody.Append("           " & IHFcallbackRequested & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If IHFcallbackRequested = "Yes" Then
                If IHFcallbackRequestedText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Contact:</b> ")
                    strBody.Append("           " & IHFcallbackRequestedText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If
        End If
    End Sub

    Private Sub GetRail(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim TrainType As String = ""
        Dim CompanyOperatingTrain As String = ""
        Dim TrainNumber As String = ""
        Dim RailLiine As String = ""
        Dim MilePost As String = ""
        Dim DotCrossingNumber As String = ""
        Dim LineOwnedOperatedBy As String = ""
        Dim PeopleOnBoard As String = ""
        Dim IncidentCause As String = ""
        Dim Derailment As String = ""
        'Dim Injury As String = ""
        'Dim InjuryText As String = ""
        'Dim Fatality As String = ""
        'Dim FatalityText As String = ""
        Dim HazMat As String = ""
        Dim HazMatReleased As String = ""
        Dim FuelPetroleumSpills As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectRailByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            TrainType = HelpFunction.Convertdbnulls(objDR2("TrainType"))
            CompanyOperatingTrain = HelpFunction.Convertdbnulls(objDR2("CompanyOperatingTrain"))
            TrainNumber = HelpFunction.Convertdbnulls(objDR2("TrainNumber"))
            RailLiine = HelpFunction.Convertdbnulls(objDR2("RailLiine"))
            MilePost = HelpFunction.Convertdbnulls(objDR2("MilePost"))
            DotCrossingNumber = HelpFunction.Convertdbnulls(objDR2("DotCrossingNumber"))
            LineOwnedOperatedBy = HelpFunction.Convertdbnulls(objDR2("LineOwnedOperatedBy"))
            PeopleOnBoard = HelpFunction.Convertdbnulls(objDR2("PeopleOnBoard"))
            IncidentCause = HelpFunction.Convertdbnulls(objDR2("IncidentCause"))
            Derailment = HelpFunction.Convertdbnulls(objDR2("Derailment"))
            'Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            'InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            'Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            'FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            HazMat = HelpFunction.Convertdbnulls(objDR2("HazMat"))
            HazMatReleased = HelpFunction.Convertdbnulls(objDR2("HazMatReleased"))
            FuelPetroleumSpills = HelpFunction.Convertdbnulls(objDR2("FuelPetroleumSpills"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Rail Incident</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Rail Incident</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If TrainType <> "" And TrainType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Train Type:</b> ")
            strBody.Append("           " & TrainType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If CompanyOperatingTrain <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Name of company operating train:</b> ")
            strBody.Append("           " & CompanyOperatingTrain & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If TrainNumber <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Train number:</b> ")
            strBody.Append("           " & TrainNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If RailLiine <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Rail line:</b> ")
            strBody.Append("           " & RailLiine & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MilePost <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Mile post:</b> ")
            strBody.Append("           " & MilePost & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If DotCrossingNumber <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>DOT crossing number:</b> ")
            strBody.Append("           " & DotCrossingNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If LineOwnedOperatedBy <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Line owned or operated by:</b> ")
            strBody.Append("           " & LineOwnedOperatedBy & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If PeopleOnBoard <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Number of people onboard:</b> ")
            strBody.Append("           " & PeopleOnBoard & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If IncidentCause <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Cause the incident:</b> ")
            strBody.Append("           " & IncidentCause & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Derailment <> "" And Derailment <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Derailment:</b> ")
            strBody.Append("           " & Derailment & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'If Injury <> "" And Injury <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Injuries:</b> ")
        '    strBody.Append("           " & Injury & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Injury = "Yes" Then
        '    If InjuryText <> "" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and Severity of Injuries:</b> ")
        '        strBody.Append("           " & InjuryText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If

        'If Fatality <> "" And Fatality <> "Select an Option" Then
        '    strBody.Append("<table>")
        '    strBody.Append("    <tr>")
        '    strBody.Append("        <td align='left'width='650px'>")
        '    strBody.Append("            <b>Fatalities:</b> ")
        '    strBody.Append("           " & Fatality & "  ")
        '    strBody.Append("        </td>")
        '    strBody.Append("    </tr>")
        '    strBody.Append("</table>")
        'End If

        'If Fatality = "Yes" Then
        '    If FatalityText <> "" Then
        '        strBody.Append("<table>")
        '        strBody.Append("    <tr>")
        '        strBody.Append("        <td align='left'width='650px'>")
        '        strBody.Append("            <b>Number and location fatalities:</b> ")
        '        strBody.Append("           " & FatalityText & "  ")
        '        strBody.Append("        </td>")
        '        strBody.Append("    </tr>")
        '        strBody.Append("</table>")
        '    End If
        'End If

        If HazMat <> "" And HazMat <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Hazardous materials onboard:</b> ")
            strBody.Append("           " & HazMat & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If HazMatReleased <> "" And HazMatReleased <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Hazardous materials released:</b> ")
            strBody.Append("           " & HazMatReleased & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If FuelPetroleumSpills <> "" And FuelPetroleumSpills <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Fuel or Petroleum Spills:</b> ")
            strBody.Append("           " & FuelPetroleumSpills & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If
    End Sub

    Private Sub GetSearchRescue(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim SearchRescueDate As String = ""
        Dim localTime As String = ""
        Dim MissionNumber As String = ""
        Dim CoordinateAreaDescription As String = ""
        Dim RegistrationInformation As String = ""
        Dim CAPResponding As String = ""
        Dim MissingOverdueAircraft As String = ""
        Dim MissionClosedDate As String = ""
        Dim localTime2 As String = ""
        Dim Disposition As String = ""
        Dim AffectedStrutureFacility As String = ""
        Dim CausedCollapse As String = ""
        Dim NumberPeopleTrapped As String = ""
        'Dim Injury As String = ""
        'Dim InjuryText As String = ""
        'Dim Fatality As String = ""
        'Dim FatalityText As String = ""
        Dim UnmetNeeds As String = ""
        Dim UnmetNeedsText As String = ""
        Dim CoordinatingRescueEffort As String = ""
        Dim DescriptionIndividual As String = ""
        Dim LastSeen As String = ""
        Dim DescriptionVehicleRelevantInformation As String = ""
        Dim AgencyHandlingInvestigation As String = ""
        Dim IsCollapse As String = ""
        Dim PeopleTrapped As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectSearchRescueByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            SearchRescueDate = HelpFunction.Convertdbnulls(objDR2("SearchRescueDate"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR2("SearchRescueTime")))
            MissionNumber = HelpFunction.Convertdbnulls(objDR2("MissionNumber"))
            CoordinateAreaDescription = HelpFunction.Convertdbnulls(objDR2("CoordinateAreaDescription"))
            RegistrationInformation = HelpFunction.Convertdbnulls(objDR2("RegistrationInformation"))
            CAPResponding = HelpFunction.Convertdbnulls(objDR2("CAPResponding"))
            MissingOverdueAircraft = HelpFunction.Convertdbnulls(objDR2("MissingOverdueAircraft"))
            MissionClosedDate = HelpFunction.Convertdbnulls(objDR2("MissionClosedDate"))
            localTime2 = CStr(HelpFunction.Convertdbnulls(objDR2("MissionClosedTime")))
            Disposition = HelpFunction.Convertdbnulls(objDR2("Disposition"))
            AffectedStrutureFacility = HelpFunction.Convertdbnulls(objDR2("AffectedStrutureFacility"))
            CausedCollapse = HelpFunction.Convertdbnulls(objDR2("CausedCollapse"))
            NumberPeopleTrapped = HelpFunction.Convertdbnulls(objDR2("NumberPeopleTrapped"))
            'Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            'InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            'Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            'FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            UnmetNeeds = HelpFunction.Convertdbnulls(objDR2("UnmetNeeds"))
            UnmetNeedsText = HelpFunction.Convertdbnulls(objDR2("UnmetNeedsText"))
            CoordinatingRescueEffort = HelpFunction.Convertdbnulls(objDR2("CoordinatingRescueEffort"))
            DescriptionIndividual = HelpFunction.Convertdbnulls(objDR2("DescriptionIndividual"))
            LastSeen = HelpFunction.Convertdbnulls(objDR2("LastSeen"))
            DescriptionVehicleRelevantInformation = HelpFunction.Convertdbnulls(objDR2("DescriptionVehicleRelevantInformation"))
            AgencyHandlingInvestigation = HelpFunction.Convertdbnulls(objDR2("AgencyHandlingInvestigation"))
            IsCollapse = HelpFunction.Convertdbnulls(objDR2("IsCollapse"))
            PeopleTrapped = HelpFunction.Convertdbnulls(objDR2("PeopleTrapped"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Search & Rescue</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Search & Rescue</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "ELT" Or SubType = "EPIRB" Or SubType = "PLB" Then
            If SearchRescueDate <> "1/1/1900" And SearchRescueDate <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Date mission opened:</b> ")
                strBody.Append("           " & SearchRescueDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Time mission opened:</b> ")
                strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If MissionNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Mission number:</b> ")
                strBody.Append("           " & MissionNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CoordinateAreaDescription <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Last coordinates or area description:</b> ")
                strBody.Append("           " & CoordinateAreaDescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If RegistrationInformation <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Registration information:</b> ")
                strBody.Append("           " & RegistrationInformation & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If CAPResponding <> "" And CAPResponding <> "Select an Option" And CAPResponding <> "N/A" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>CAP responding:</b> ")
                strBody.Append("           " & CAPResponding & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If MissingOverdueAircraft <> "" And MissingOverdueAircraft <> "Select an Option" And MissingOverdueAircraft <> "N/A" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Missing or overdue aircraft in the area:</b> ")
                strBody.Append("           " & MissingOverdueAircraft & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If MissionClosedDate <> "" And MissionClosedDate <> "1/1/1900" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Date mission closed:</b> ")
                strBody.Append("           " & MissionClosedDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If localTime2 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Time mission closed:</b> ")
                strBody.Append("           " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Disposition <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Disposition:</b> ")
                strBody.Append("           " & Disposition & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Structure Collapse" Or SubType = "Industrial Accident" Or SubType = "Transportation Accident" Or SubType = "Other" Then
            If AffectedStrutureFacility <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Description of affected struture(s) or facilities(s):</b> ")
                strBody.Append("           " & AffectedStrutureFacility & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If IsCollapse <> "" And IsCollapse <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>There is a collapse:</b> ")
                strBody.Append("           " & IsCollapse & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                If CausedCollapse <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Cause of collapse:</b> ")
                    strBody.Append("           " & CausedCollapse & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If PeopleTrapped <> "" And PeopleTrapped <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>People are trapped:</b> ")
                strBody.Append("           " & PeopleTrapped & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                If NumberPeopleTrapped <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Number of people trapped:</b> ")
                    strBody.Append("           " & NumberPeopleTrapped & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            'If Injury <> "" And Injury <> "Select an Option" Then
            '    strBody.Append("<table>")
            '    strBody.Append("    <tr>")
            '    strBody.Append("        <td align='left'width='650px'>")
            '    strBody.Append("            <b>Injuries:</b> ")
            '    strBody.Append("           " & Injury & "  ")
            '    strBody.Append("        </td>")
            '    strBody.Append("    </tr>")
            '    strBody.Append("</table>")
            'End If

            'If Injury = "Yes" Then
            '    If InjuryText <> "" Then
            '        strBody.Append("<table>")
            '        strBody.Append("    <tr>")
            '        strBody.Append("        <td align='left'width='650px'>")
            '        strBody.Append("            <b>Number and Severity of Injuries:</b> ")
            '        strBody.Append("           " & InjuryText & "  ")
            '        strBody.Append("        </td>")
            '        strBody.Append("    </tr>")
            '        strBody.Append("</table>")
            '    End If
            'End If

            'If Fatality <> "" And Fatality <> "Select an Option" Then
            '    strBody.Append("<table>")
            '    strBody.Append("    <tr>")
            '    strBody.Append("        <td align='left'width='650px'>")
            '    strBody.Append("            <b>Fatalities:</b> ")
            '    strBody.Append("           " & Fatality & "  ")
            '    strBody.Append("        </td>")
            '    strBody.Append("    </tr>")
            '    strBody.Append("</table>")
            'End If

            'If Fatality = "Yes" Then
            '    If FatalityText <> "" Then
            '        strBody.Append("<table>")
            '        strBody.Append("    <tr>")
            '        strBody.Append("        <td align='left'width='650px'>")
            '        strBody.Append("            <b>Number and location of fatalities:</b> ")
            '        strBody.Append("           " & FatalityText & "  ")
            '        strBody.Append("        </td>")
            '        strBody.Append("    </tr>")
            '        strBody.Append("</table>")
            '    End If
            'End If

            If UnmetNeeds <> "" And UnmetNeeds <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Unmet needs for the rescue operation:</b> ")
                strBody.Append("           " & UnmetNeeds & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If UnmetNeeds = "Yes" Then
                If UnmetNeedsText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Needs description:</b> ")
                    strBody.Append("           " & UnmetNeedsText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If CoordinatingRescueEffort <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Department/agency coordinating rescue efforts:</b> ")
                strBody.Append("           " & CoordinatingRescueEffort & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "LE Search (Missing Person)" Then
            If DescriptionIndividual <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Description of the individual(s):</b> ")
                strBody.Append("           " & DescriptionIndividual & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If LastSeen <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Individual(s) were last seen in:</b> ")
                strBody.Append("           " & LastSeen & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If DescriptionVehicleRelevantInformation <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Vehicle Description/other relevant information:</b> ")
                strBody.Append("           " & DescriptionVehicleRelevantInformation & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If AgencyHandlingInvestigation <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Agency handling the investigation:</b> ")
                strBody.Append("           " & AgencyHandlingInvestigation & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If
    End Sub

    Private Sub GetSecurityThreat(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim Description As String = ""
        Dim IndividualResponsibleDescription As String = ""
        Dim Location As String = ""
        Dim ConfinedLocation As String = ""
        Dim ListAreas As String = ""
        Dim IncidentSeverity As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectSecurityThreatByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            Description = HelpFunction.Convertdbnulls(objDR2("Description"))
            IndividualResponsibleDescription = HelpFunction.Convertdbnulls(objDR2("IndividualResponsibleDescription"))
            Location = HelpFunction.Convertdbnulls(objDR2("Location"))
            ConfinedLocation = HelpFunction.Convertdbnulls(objDR2("ConfinedLocation"))
            ListAreas = HelpFunction.Convertdbnulls(objDR2("ListAreas"))
            IncidentSeverity = HelpFunction.Convertdbnulls(objDR2("IncidentSeverity"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Suspicious Activity</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Suspicious Activity</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'If SubType <> "Lockdown" Then

        If Description <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description the incident or threat:</b> ")
            strBody.Append("           " & Description & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If IndividualResponsibleDescription <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description of the individual(s) responsible:</b> ")
            strBody.Append("           " & IndividualResponsibleDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If ConfinedLocation <> "" And ConfinedLocation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Incident confined to one location:</b> ")
            strBody.Append("           " & ConfinedLocation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If ConfinedLocation = "Yes" And Location <> "" And Location <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Location:</b> ")
            strBody.Append("           " & Location & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If ListAreas <> "" Then
            If Location = "Other area" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Area(s); specific streets/boundaries:</b> ")
                strBody.Append("           " & ListAreas & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If

        If IncidentSeverity <> "" And IncidentSeverity <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Incident severity:</b> ")
            strBody.Append("           " & IncidentSeverity & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If
        'End If
    End Sub

    Private Sub GetUtilityDisruptionEmergency(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim TOcommunicationsSystem As String = ""
        Dim TOsystemOperated As String = ""
        Dim TOcustomersAffectedNumber As String = ""
        Dim TO911Affected As String = ""
        Dim TO911AffectedText As String = ""
        Dim TOdamageFacilityDistibutionSystem As String = ""
        Dim TOdamageFacilityDistibutionSystemIntentional As String = ""
        Dim TOdamageFacilityDistibutionSystemText As String = ""
        Dim DWOWaterSystemName As String = ""
        Dim DWOpublicWaterSystemID As String = ""
        Dim DWOnumberCustomersAffected As String = ""
        Dim DWOoutageResultTTVSBDSF As String = ""
        Dim DWOEstimatedDateTimeRestoration As String = ""
        Dim DWOboilAdvisory As String = ""
        Dim EOelectricSystem As String = ""
        Dim EOsystemOperatedBy As String = ""
        Dim EOwhatCausedOutage As String = ""
        Dim EONumberCustomersAffected As String = ""
        Dim EOestimatedGreaterRestoration As String = ""
        Dim EOdamageFacilityDistibutionSystem As String = ""
        Dim EOdamageFacilityDistibutionSystemIntentional As String = ""
        Dim EOdamageFacilityDistibutionSystemResposible As String = ""
        Dim GCAadvisoryType As String = ""
        Dim GCAsupplyShortage As String = ""
        Dim GCAadvisory As String = ""
        Dim NGOsystem As String = ""
        Dim NGOsystemOperatedBy As String = ""
        Dim NGOoutageCause As String = ""
        Dim NGOCustomersAffectedNumber As String = ""
        Dim NGOestimatedTimeRestoration As String = ""
        Dim NGOdFDS As String = ""
        Dim NGOdFDSintentional As String = ""
        Dim NGOdFDSdescription As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectUtilityDisruptionEmergencyByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            TOcommunicationsSystem = HelpFunction.Convertdbnulls(objDR2("TOcommunicationsSystem"))
            TOsystemOperated = HelpFunction.Convertdbnulls(objDR2("TOsystemOperated"))
            TOcustomersAffectedNumber = HelpFunction.Convertdbnulls(objDR2("TOcustomersAffectedNumber"))
            TO911Affected = HelpFunction.Convertdbnulls(objDR2("TO911Affected"))
            TO911AffectedText = HelpFunction.Convertdbnulls(objDR2("TO911AffectedText"))
            TOdamageFacilityDistibutionSystem = HelpFunction.Convertdbnulls(objDR2("TOdamageFacilityDistibutionSystem"))
            TOdamageFacilityDistibutionSystemIntentional = HelpFunction.Convertdbnulls(objDR2("TOdamageFacilityDistibutionSystemIntentional"))
            TOdamageFacilityDistibutionSystemText = HelpFunction.Convertdbnulls(objDR2("TOdamageFacilityDistibutionSystemText"))
            DWOWaterSystemName = HelpFunction.Convertdbnulls(objDR2("DWOWaterSystemName"))
            DWOpublicWaterSystemID = HelpFunction.Convertdbnulls(objDR2("DWOpublicWaterSystemID"))
            DWOnumberCustomersAffected = HelpFunction.Convertdbnulls(objDR2("DWOnumberCustomersAffected"))
            DWOoutageResultTTVSBDSF = HelpFunction.Convertdbnulls(objDR2("DWOoutageResultTTVSBDSF"))
            DWOEstimatedDateTimeRestoration = HelpFunction.Convertdbnulls(objDR2("DWOEstimatedDateTimeRestoration"))
            DWOboilAdvisory = HelpFunction.Convertdbnulls(objDR2("DWOboilAdvisory"))
            EOelectricSystem = HelpFunction.Convertdbnulls(objDR2("EOelectricSystem"))
            EOsystemOperatedBy = HelpFunction.Convertdbnulls(objDR2("EOsystemOperatedBy"))
            EOwhatCausedOutage = HelpFunction.Convertdbnulls(objDR2("EOwhatCausedOutage"))
            EONumberCustomersAffected = HelpFunction.Convertdbnulls(objDR2("EONumberCustomersAffected"))
            EOestimatedGreaterRestoration = HelpFunction.Convertdbnulls(objDR2("EOestimatedGreaterRestoration"))
            EOdamageFacilityDistibutionSystem = HelpFunction.Convertdbnulls(objDR2("EOdamageFacilityDistibutionSystem"))
            EOdamageFacilityDistibutionSystemIntentional = HelpFunction.Convertdbnulls(objDR2("EOdamageFacilityDistibutionSystemIntentional"))
            EOdamageFacilityDistibutionSystemResposible = HelpFunction.Convertdbnulls(objDR2("EOdamageFacilityDistibutionSystemResposible"))
            GCAadvisoryType = HelpFunction.Convertdbnulls(objDR2("GCAadvisoryType"))
            GCAsupplyShortage = HelpFunction.Convertdbnulls(objDR2("GCAsupplyShortage"))
            GCAadvisory = HelpFunction.Convertdbnulls(objDR2("GCAadvisory"))
            NGOsystem = HelpFunction.Convertdbnulls(objDR2("NGOsystem"))
            NGOsystemOperatedBy = HelpFunction.Convertdbnulls(objDR2("NGOsystemOperatedBy"))
            NGOoutageCause = HelpFunction.Convertdbnulls(objDR2("NGOoutageCause"))
            NGOCustomersAffectedNumber = HelpFunction.Convertdbnulls(objDR2("NGOCustomersAffectedNumber"))
            NGOestimatedTimeRestoration = HelpFunction.Convertdbnulls(objDR2("NGOestimatedTimeRestoration"))
            NGOdFDS = HelpFunction.Convertdbnulls(objDR2("NGOdFDS"))
            NGOdFDSintentional = HelpFunction.Convertdbnulls(objDR2("NGOdFDSintentional"))
            NGOdFDSdescription = HelpFunction.Convertdbnulls(objDR2("NGOdFDSdescription"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Utility Disruption/Emergency</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Utility Disruption/Emergency</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "Telecommunications Outage" Then
            If TOcommunicationsSystem <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Communications System:</b> ")
                strBody.Append("           " & TOcommunicationsSystem & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TOsystemOperated <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>System operated by:</b> ")
                strBody.Append("           " & TOsystemOperated & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TOcustomersAffectedNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Number of Customers affected:</b> ")
                strBody.Append("           " & TOcustomersAffectedNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TO911Affected <> "" And TO911Affected <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Is 911 telephone service affected:</b> ")
                strBody.Append("           " & TO911Affected & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TO911Affected = "Yes" Then
                If TO911AffectedText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Describe:</b> ")
                    strBody.Append("           " & TO911AffectedText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If TOdamageFacilityDistibutionSystem <> "" And TOdamageFacilityDistibutionSystem <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Damage to the facility or distibution system:</b> ")
                strBody.Append("           " & TOdamageFacilityDistibutionSystem & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If


            If TOdamageFacilityDistibutionSystem = "Yes" Then
                If TOdamageFacilityDistibutionSystemIntentional <> "" And TOdamageFacilityDistibutionSystemIntentional <> "Select an Option" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Intentional:</b> ")
                    strBody.Append("           " & TOdamageFacilityDistibutionSystemIntentional & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If TOdamageFacilityDistibutionSystemText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Description of the individual(s) responsible:</b> ")
                    strBody.Append("           " & TOdamageFacilityDistibutionSystemText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If
        ElseIf SubType = "Drinking Water Outage" Then
            If DWOWaterSystemName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Water System Name:</b> ")
                strBody.Append("           " & DWOWaterSystemName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If DWOpublicWaterSystemID <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Public water system ID #:</b> ")
                strBody.Append("           " & DWOpublicWaterSystemID & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If DWOnumberCustomersAffected <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Number of customers affected:</b> ")
                strBody.Append("           " & DWOnumberCustomersAffected & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If DWOoutageResultTTVSBDSF <> "" And DWOoutageResultTTVSBDSF <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Outage a result of any trespassing, theft, vandalism, or a security breach to the distribution system or its facilities:</b> ")
                strBody.Append("           " & DWOoutageResultTTVSBDSF & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If DWOEstimatedDateTimeRestoration <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Estimated date/time of restoration:</b> ")
                strBody.Append("           " & DWOEstimatedDateTimeRestoration & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If DWOboilAdvisory <> "" And DWOboilAdvisory <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Boil water advisory issued:</b> ")
                strBody.Append("           " & DWOboilAdvisory & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Electric Outage" Then
            If EOelectricSystem <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Electric System:</b> ")
                strBody.Append("           " & EOelectricSystem & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If EOsystemOperatedBy <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>System operated by:</b> ")
                strBody.Append("           " & EOsystemOperatedBy & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If EOwhatCausedOutage <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Cause of outage:</b> ")
                strBody.Append("           " & EOwhatCausedOutage & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If EONumberCustomersAffected <> "" And EONumberCustomersAffected <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Number of Customers affected:</b> ")
                strBody.Append("           " & EONumberCustomersAffected & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If EOestimatedGreaterRestoration <> "" And EOestimatedGreaterRestoration <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Estimated time to 98% or greater restoration:</b> ")
                strBody.Append("           " & EOestimatedGreaterRestoration & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If EOdamageFacilityDistibutionSystem <> "" And EOdamageFacilityDistibutionSystem <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Damage to the facility or distibution system:</b> ")
                strBody.Append("           " & EOdamageFacilityDistibutionSystem & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If EOdamageFacilityDistibutionSystem = "Yes" Then
                If EOdamageFacilityDistibutionSystemIntentional <> "" And EOdamageFacilityDistibutionSystemIntentional <> "Select an Option" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Intentional:</b> ")
                    strBody.Append("           " & EOdamageFacilityDistibutionSystemIntentional & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If EOdamageFacilityDistibutionSystemResposible <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Description of the individual(s) responsible:</b> ")
                    strBody.Append("           " & EOdamageFacilityDistibutionSystemResposible & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            'strBody.Append("<table>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td align='left'width='650px'>")
            'strBody.Append("            <b>Type of Advisory:</b> ")
            'strBody.Append("           " & GCAadvisoryType & "  ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")

            'strBody.Append("<table>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td align='left'width='650px'>")
            'strBody.Append("            <b>Advisory due to a fuel supply shortage:</b> ")
            'strBody.Append("           " & GCAsupplyShortage & "  ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")

            'strBody.Append("<table>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td align='left'width='650px'>")
            'strBody.Append("            <b>Text of the Advisory:</b> ")
            'strBody.Append("           " & GCAadvisory & "  ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")
        ElseIf SubType = "Natural Gas Outage" Then
            If NGOsystem <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Natural Gas System:</b> ")
                strBody.Append("           " & NGOsystem & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If NGOsystemOperatedBy <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>System operated by:</b> ")
                strBody.Append("           " & NGOsystemOperatedBy & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If NGOoutageCause <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Cause the outage:</b> ")
                strBody.Append("           " & NGOoutageCause & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If NGOCustomersAffectedNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Number of Customers affected:</b> ")
                strBody.Append("           " & NGOCustomersAffectedNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If NGOestimatedTimeRestoration <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Estimated time restoration:</b> ")
                strBody.Append("           " & NGOestimatedTimeRestoration & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If NGOdFDS <> "" And NGOdFDS <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Damage to the facility or distibution system:</b> ")
                strBody.Append("           " & NGOdFDS & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If NGOdFDS = "Yes" Then
                If NGOdFDSintentional <> "" And NGOdFDSintentional <> "Select an Option" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Intentional:</b> ")
                    strBody.Append("           " & NGOdFDSintentional & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If NGOdFDSdescription <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Description of the individual(s) responsible:</b> ")
                    strBody.Append("           " & NGOdFDSdescription & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If
        ElseIf SubType = "Electric Generating Capacity Advisory" Then
            If GCAadvisoryType <> "" And GCAadvisoryType <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Type of Advisory:</b> ")
                strBody.Append("           " & GCAadvisoryType & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If GCAsupplyShortage <> "" And GCAsupplyShortage <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Advisory due to a fuel supply shortage:</b> ")
                strBody.Append("           " & GCAsupplyShortage & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If GCAadvisory <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Text of the Advisory:</b> ")
                strBody.Append("           " & GCAadvisory & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If
    End Sub

    Private Sub GetWastewater(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim WWsystemIDPermitNumber As String = ""
        Dim WWsystemName As String = ""
        Dim WWsystemType As String = ""
        Dim WWreleaseOccurred As String = ""
        Dim WWtype As String = ""
        Dim WWreleaseCause As String = ""
        Dim WWreleaseStatus As String = ""
        Dim WWceasedDate As String = ""
        Dim localTime As String = ""
        Dim WWceasedTime As String = ""
        Dim WWreleasedContainedonSite As String = ""
        Dim WWreleaseAmount As String = ""
        Dim WWstormWater As String = ""
        Dim WWstormWaterLocation As String = ""
        Dim WWstormWaterDischarge As String = ""
        Dim WWcleanupActionsText As String = ""
        Dim WWsurfaceWater As String = ""
        Dim WWsurfaceWaterDDL As String = ""
        Dim WWwaterway As String = ""
        Dim WWconfirmedContamination As String = ""
        Dim WWcleanupActions As String = ""
        Dim TEsystemIDPermitNumber As String = ""
        Dim TEsystemName As String = ""
        Dim TEreleaseCause As String = ""
        Dim TEgallonsReleased As String = ""
        Dim TEcleanupActions As String = ""
        Dim TEcleanupActionsText As String = ""
        Dim WWreleaseCauseDetails As String = ""
        Dim WWreleaseOccurredDetails As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectWastewaterByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            WWsystemIDPermitNumber = HelpFunction.Convertdbnulls(objDR2("WWsystemIDPermitNumber"))
            WWsystemName = HelpFunction.Convertdbnulls(objDR2("WWsystemName"))
            WWsystemType = HelpFunction.Convertdbnulls(objDR2("WWsystemType"))
            WWreleaseOccurred = HelpFunction.Convertdbnulls(objDR2("WWreleaseOccurred"))
            WWtype = HelpFunction.Convertdbnulls(objDR2("WWtype"))
            WWreleaseCause = HelpFunction.Convertdbnulls(objDR2("WWreleaseCause"))
            WWreleaseStatus = HelpFunction.Convertdbnulls(objDR2("WWreleaseStatus"))
            WWceasedDate = HelpFunction.Convertdbnulls(objDR2("WWceasedDate"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR2("WWceasedTime")))
            WWceasedTime = HelpFunction.Convertdbnulls(objDR2("WWceasedTime"))
            WWreleasedContainedonSite = HelpFunction.Convertdbnulls(objDR2("WWreleasedContainedonSite"))
            WWreleaseAmount = HelpFunction.Convertdbnulls(objDR2("WWreleaseAmount"))
            WWstormWater = HelpFunction.Convertdbnulls(objDR2("WWstormWater"))
            WWstormWaterLocation = HelpFunction.Convertdbnulls(objDR2("WWstormWaterLocation"))
            WWstormWaterDischarge = HelpFunction.Convertdbnulls(objDR2("WWstormWaterDischarge"))
            WWcleanupActionsText = HelpFunction.Convertdbnulls(objDR2("WWcleanupActionsText"))
            WWsurfaceWater = HelpFunction.Convertdbnulls(objDR2("WWsurfaceWater"))
            WWsurfaceWaterDDL = HelpFunction.Convertdbnulls(objDR2("WWsurfaceWaterDDL"))
            WWwaterway = HelpFunction.Convertdbnulls(objDR2("WWwaterway"))
            WWconfirmedContamination = HelpFunction.Convertdbnulls(objDR2("WWconfirmedContamination"))
            WWcleanupActions = HelpFunction.Convertdbnulls(objDR2("WWcleanupActions"))
            TEsystemIDPermitNumber = HelpFunction.Convertdbnulls(objDR2("TEsystemIDPermitNumber"))
            TEsystemName = HelpFunction.Convertdbnulls(objDR2("TEsystemName"))
            TEreleaseCause = HelpFunction.Convertdbnulls(objDR2("TEreleaseCause"))
            TEgallonsReleased = HelpFunction.Convertdbnulls(objDR2("TEgallonsReleased"))
            TEcleanupActions = HelpFunction.Convertdbnulls(objDR2("TEcleanupActions"))
            TEcleanupActionsText = HelpFunction.Convertdbnulls(objDR2("TEcleanupActionsText"))
            WWreleaseCauseDetails = HelpFunction.Convertdbnulls(objDR2("WWreleaseCauseDetails"))
            WWreleaseOccurredDetails = HelpFunction.Convertdbnulls(objDR2("WWreleaseOccurredDetails"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()
        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Wastewater or Effluent</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Wastewater or Effluent</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        End If

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "Wastewater" Then
            If WWsystemIDPermitNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Public Water System ID or Permit Number:</b> ")
                strBody.Append("           " & WWsystemIDPermitNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WWsystemName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Name of System:</b> ")
                strBody.Append("           " & WWsystemName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WWsystemType <> "" And WWsystemType <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Type of system: </b> ")
                strBody.Append("           " & WWsystemType & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WWreleaseOccurred <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Release occurred from a:</b> ")
                strBody.Append("           " & WWreleaseOccurred & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WWreleaseOccurredDetails <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            &nbsp;&nbsp;<b>Additional Details:</b> ")
                strBody.Append("           " & WWreleaseOccurredDetails & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WWtype <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Type of wastewater:</b> ")
                strBody.Append("           " & WWtype & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WWreleaseCause <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Release Cause:</b> ")
                strBody.Append("           " & WWreleaseCause & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WWreleaseCauseDetails <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            &nbsp;&nbsp;<b>Additional Release Cause Details:</b> ")
                strBody.Append("           " & WWreleaseCauseDetails & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WWreleaseStatus <> "" And WWreleaseStatus <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Release status:</b> ")
                strBody.Append("           " & WWreleaseStatus & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WWreleaseStatus = "Ceased" Then
                If WWceasedDate <> "1/1/1900" And WWceasedDate <> "" Then

                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Date release ceased:</b> ")
                    strBody.Append("           " & WWceasedDate & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If localTime <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Time release ceased:</b> ")
                    strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & " &nbsp;ET ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If WWreleasedContainedonSite <> "" And WWreleasedContainedonSite <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Release contained on-site at a water reclamation facility:</b> ")
                strBody.Append("           " & WWreleasedContainedonSite & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WWreleaseAmount <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Amount of release, in gallons:</b> ")
                strBody.Append("           " & WWreleaseAmount & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WWstormWater <> "" And WWstormWater <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Release enter a storm water system:</b> ")
                strBody.Append("           " & WWstormWater & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WWstormWater = "Yes" Then
                If WWstormWaterLocation <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Location of storm drain(s) that were impacted:</b> ")
                    strBody.Append("           " & WWstormWaterLocation & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

                If WWstormWaterDischarge <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Storm drain discharges:</b> ")
                    strBody.Append("           " & WWstormWaterDischarge & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If WWsurfaceWaterDDL <> "" And WWsurfaceWaterDDL <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Release enter any surface waters:</b> ")
                strBody.Append("           " & WWsurfaceWaterDDL & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WWsurfaceWaterDDL = "Yes" Then
                If WWsurfaceWater <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Type of surface water:</b> ")
                    strBody.Append("           " & WWsurfaceWater & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If WWsurfaceWater = "Retention Pond, contained." Or WWsurfaceWater = "Retention pond, drained to waterway." Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Names of waterway(s):</b> ")
                strBody.Append("           " & WWwaterway & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WWconfirmedContamination <> "" And WWconfirmedContamination <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Affected waterway a source of drinking water:</b> ")
                strBody.Append("           " & WWconfirmedContamination & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WWcleanupActions <> "" And WWcleanupActions <> "Select an Option" Then

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Status of Cleanup Actions:</b> ")
                strBody.Append("           " & WWcleanupActions & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

            End If

            If WWcleanupActionsText <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Description of clean-up actions:</b> ")
                strBody.Append("           " & WWcleanupActionsText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        ElseIf SubType = "Treated Effluent" Then
            If TEsystemIDPermitNumber <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Public Water System ID or Permit Number:</b> ")
                strBody.Append("           " & TEsystemIDPermitNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TEsystemName <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Name of System:</b> ")
                strBody.Append("           " & TEsystemName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TEreleaseCause <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Caused of release:</b> ")
                strBody.Append("           " & TEreleaseCause & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TEgallonsReleased <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Amount of release, in gallons:</b> ")
                strBody.Append("           " & TEgallonsReleased & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TEcleanupActions <> "" And TEcleanupActions <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Cleanup actions needed:</b> ")
                strBody.Append("           " & TEcleanupActions & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TEcleanupActions = "Yes" Then
                If TEcleanupActionsText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Description of cleanup actions:</b> ")
                    strBody.Append("           " & TEcleanupActionsText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If
        End If
    End Sub

    Private Sub GetWeatherAdvisories(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim WWAdateIssued As String = ""
        Dim localTime As String = ""
        Dim WWAeffectiveDate As String = ""
        Dim WWAeffectiveTime As String = ""
        Dim WWAexpiresDate As String = ""
        Dim WWAexpiresTime As String = ""
        Dim WWAissuingOffice As String = ""
        Dim WWAadvisoryType As String = ""
        Dim WWAadvisoryText As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectWeatherByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            WWAdateIssued = HelpFunction.Convertdbnulls(objDR2("WWAdateIssued"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR2("WWAtime")))
            WWAeffectiveDate = HelpFunction.Convertdbnulls(objDR2("WWAeffectiveDate"))
            WWAeffectiveTime = HelpFunction.Convertdbnulls(objDR2("WWAeffectiveTime"))
            WWAexpiresDate = HelpFunction.Convertdbnulls(objDR2("WWAexpiresDate"))
            WWAexpiresTime = HelpFunction.Convertdbnulls(objDR2("WWAexpiresTime"))
            WWAissuingOffice = HelpFunction.Convertdbnulls(objDR2("WWAissuingOffice"))
            WWAadvisoryType = HelpFunction.Convertdbnulls(objDR2("WWAadvisoryType"))
            WWAadvisoryText = HelpFunction.Convertdbnulls(objDR2("WWAadvisoryText"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Weather Advisories</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Weather Advisories</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'Get attached weather maps.
        Dim WeatherMap As String = ""
        Dim WeatherMapName As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectWeatherMap", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.HasRows = True Then
            Do While objDR2.Read()
                WeatherMap = HelpFunction.Convertdbnulls(objDR2("Map"))
                WeatherMapName = HelpFunction.Convertdbnulls(objDR2("MapName"))

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='640px'>")
                strBody.Append("            <img id='Map' alt='Map" & WeatherMapName & "' width='650px' src='https://apps.floridadisaster.org/" & HttpContext.Current.Application("ApplicationEnvironmentForUpload").ToString & "/Uploads/" & WeatherMap & "'>")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            Loop
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        'Get attached weather links.
        Dim WeatherLink As String = ""
        Dim WeatherLinkName As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectWeatherLink", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.HasRows = True Then
            Do While objDR2.Read()
                WeatherLink = HelpFunction.Convertdbnulls(objDR2("Link"))
                WeatherLinkName = HelpFunction.Convertdbnulls(objDR2("LinkName"))

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <a target='_blank' href='" & WeatherLink & "'>" & WeatherLink & "</a>")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            Loop
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If WWAdateIssued <> "1/1/1900" And WWAdateIssued <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Date Issued:</b> ")
            strBody.Append("           " & WWAdateIssued & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If localTime <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Time Issued:</b> ")
            strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If WWAeffectiveDate <> "1/1/1900" And WWAeffectiveDate <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Effective on Date:</b> ")
            strBody.Append("           " & WWAeffectiveDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If WWAeffectiveTime <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Effective on Time:</b> ")
            strBody.Append("           " & WWAeffectiveTime & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If WWAexpiresDate <> "1/1/1900" And WWAexpiresDate <> "" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Expires on Date:</b> ")
            strBody.Append("           " & WWAexpiresDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        End If

        If WWAexpiresTime <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Expires on Time:</b> ")
            strBody.Append("           " & WWAexpiresTime & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If WWAissuingOffice <> "" And WWAissuingOffice <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Issuing Office:</b> ")
            strBody.Append("           " & WWAissuingOffice & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If WWAadvisoryType <> "" And WWAadvisoryType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Type of Advisory:</b> ")
            strBody.Append("           " & WWAadvisoryType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If WWAadvisoryText <> "" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Advisory Text:</b> ")
            strBody.Append("           <pre>" & WWAadvisoryText & "</pre>  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If
    End Sub

    Private Sub GetWeatherReports(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)
        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim LSRreportType As String = ""
        Dim LSRreportReceived As String = ""
        'Dim LSRInjury As String = ""
        'Dim LSRInjuryText As String = ""
        'Dim LSRFatality As String = ""
        'Dim LSRFatalityText As String = ""
        Dim LSRdisplacement As String = ""
        Dim LSRdisplacementText As String = ""
        Dim LSRdamageStructures As String = ""
        Dim LSRdamageStructuresText As String = ""
        Dim LSRinfrastructureDamage As String = ""
        Dim LSRinfrastructureDamageText As String = ""
        Dim TOtransmitter As String = ""
        Dim TOmakingNotification As String = ""
        Dim localTime2 As String = ""
        Dim TOserviceOutDate As String = ""
        Dim TOtransmitterServiceDueTo As String = ""
        Dim TOreturnToService As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectWeatherByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            LSRreportType = HelpFunction.Convertdbnulls(objDR2("LSRreportType"))
            LSRreportReceived = HelpFunction.Convertdbnulls(objDR2("LSRreportReceived"))
            'LSRInjury = HelpFunction.Convertdbnulls(objDR2("LSRInjury"))
            'LSRInjuryText = HelpFunction.Convertdbnulls(objDR2("LSRInjuryText"))
            'LSRFatality = HelpFunction.Convertdbnulls(objDR2("LSRFatality"))
            'LSRFatalityText = HelpFunction.Convertdbnulls(objDR2("LSRFatalityText"))
            LSRdisplacement = HelpFunction.Convertdbnulls(objDR2("LSRdisplacement"))
            LSRdisplacementText = HelpFunction.Convertdbnulls(objDR2("LSRdisplacementText"))
            LSRdamageStructures = HelpFunction.Convertdbnulls(objDR2("LSRdamageStructures"))
            LSRdamageStructuresText = HelpFunction.Convertdbnulls(objDR2("LSRdamageStructuresText"))
            LSRinfrastructureDamage = HelpFunction.Convertdbnulls(objDR2("LSRinfrastructureDamage"))
            LSRinfrastructureDamageText = HelpFunction.Convertdbnulls(objDR2("LSRinfrastructureDamageText"))
            TOtransmitter = HelpFunction.Convertdbnulls(objDR2("TOtransmitter"))
            TOmakingNotification = HelpFunction.Convertdbnulls(objDR2("TOmakingNotification"))
            localTime2 = CStr(HelpFunction.Convertdbnulls(objDR2("TOserviceOutTime")))
            TOserviceOutDate = HelpFunction.Convertdbnulls(objDR2("TOserviceOutDate"))
            TOtransmitterServiceDueTo = HelpFunction.Convertdbnulls(objDR2("TOtransmitterServiceDueTo"))
            TOreturnToService = HelpFunction.Convertdbnulls(objDR2("TOreturnToService"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If gStrReportFormat = "HTML" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Weather Reports</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        Else
            strBody.Append("<table width='100%'cellspacing='0' border='0'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='650px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>Weather Reports</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'Get attached weather maps.
        Dim WeatherMap As String = ""
        Dim WeatherMapName As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectWeatherMap", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.HasRows = True Then
            Do While objDR2.Read()
                WeatherMap = HelpFunction.Convertdbnulls(objDR2("Map"))
                WeatherMapName = HelpFunction.Convertdbnulls(objDR2("MapName"))

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='640px'>")
                strBody.Append("            <img id='Map' alt='Map" & WeatherMapName & "' width='650px' src='https://apps.floridadisaster.org/" & HttpContext.Current.Application("ApplicationEnvironmentForUpload").ToString & "/Uploads/" & WeatherMap & "'>")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            Loop
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        'Get attached weather links.
        Dim WeatherLink As String = ""
        Dim WeatherLinkName As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()

        objCmd2 = New SqlCommand("spSelectWeatherLink", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.HasRows = True Then
            Do While objDR2.Read()
                WeatherLink = HelpFunction.Convertdbnulls(objDR2("Link"))
                WeatherLinkName = HelpFunction.Convertdbnulls(objDR2("LinkName"))

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <a target='_blank' href='" & WeatherLink & "'>" & WeatherLink & "</a>")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            Loop
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        If SubType <> "" And SubType <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If Situation <> "" And Situation <> "Select an Option" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "" And MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='650px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If SubType = "Local Storm Report" Then
            If LSRreportType <> "" And LSRreportType <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Type of Report:</b> ")
                strBody.Append("           " & LSRreportType & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If LSRreportReceived <> "" And LSRreportReceived <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Report was received:</b> ")
                strBody.Append("           " & LSRreportReceived & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            'If LSRInjury <> "" And LSRInjury <> "Select an Option" Then
            '    strBody.Append("<table>")
            '    strBody.Append("    <tr>")
            '    strBody.Append("        <td align='left'width='650px'>")
            '    strBody.Append("            <b>Injuries:</b> ")
            '    strBody.Append("           " & LSRInjury & "  ")
            '    strBody.Append("        </td>")
            '    strBody.Append("    </tr>")
            '    strBody.Append("</table>")
            'End If

            'If LSRInjury = "Yes" Then
            '    If LSRInjuryText <> "" Then
            '        strBody.Append("<table>")
            '        strBody.Append("    <tr>")
            '        strBody.Append("        <td align='left'width='650px'>")
            '        strBody.Append("            <b>Number and Severity of Injuries:</b> ")
            '        strBody.Append("           " & LSRInjuryText & "  ")
            '        strBody.Append("        </td>")
            '        strBody.Append("    </tr>")
            '        strBody.Append("</table>")
            '    End If
            'End If

            'If LSRFatality <> "" And LSRFatality <> "Select an Option" Then
            '    strBody.Append("<table>")
            '    strBody.Append("    <tr>")
            '    strBody.Append("        <td align='left'width='650px'>")
            '    strBody.Append("            <b>Fatalities:</b> ")
            '    strBody.Append("           " & LSRFatality & "  ")
            '    strBody.Append("        </td>")
            '    strBody.Append("    </tr>")
            '    strBody.Append("</table>")
            'End If

            'If LSRFatality = "Yes" Then
            '    If LSRFatalityText <> "" Then
            '        strBody.Append("<table>")
            '        strBody.Append("    <tr>")
            '        strBody.Append("        <td align='left'width='650px'>")
            '        strBody.Append("            <b>Number and location:</b> ")
            '        strBody.Append("           " & LSRFatalityText & "  ")
            '        strBody.Append("        </td>")
            '        strBody.Append("    </tr>")
            '        strBody.Append("</table>")
            '    End If

            'End If

            If LSRdisplacement <> "" And LSRdisplacement <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Displacements:</b> ")
                strBody.Append("           " & LSRdisplacement & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If LSRdisplacement = "Yes" Then
                If LSRdisplacementText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Number and where are they being sheltered:</b> ")
                    strBody.Append("           " & LSRdisplacementText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If LSRdamageStructures <> "" And LSRdamageStructures <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Damage to structures:</b> ")
                strBody.Append("           " & LSRdamageStructures & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If LSRdamageStructures = "Yes" Then
                If LSRdamageStructuresText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Type of Structures/Number/Severity:</b> ")
                    strBody.Append("           " & LSRdamageStructuresText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If

            If LSRinfrastructureDamage <> "" And LSRinfrastructureDamage <> "Select an Option" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Damage to Infrastructure:</b> ")
                strBody.Append("           " & LSRinfrastructureDamage & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If LSRinfrastructureDamage = "Yes" Then
                If LSRinfrastructureDamageText <> "" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='650px'>")
                    strBody.Append("            <b>Description:</b> ")
                    strBody.Append("           " & LSRinfrastructureDamageText & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If
            End If
        ElseIf SubType = "NOAA Transnsmitter Outage" Then
            If TOtransmitter <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Transmitter(s):</b> ")
                strBody.Append("           " & TOtransmitter & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TOmakingNotification <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Weather Forecast Office making notification:</b> ")
                strBody.Append("           " & TOmakingNotification & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TOserviceOutDate <> "1/1/1900" And TOserviceOutDate <> "" Then

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Date release ceased:</b> ")
                strBody.Append("           " & TOserviceOutDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

            End If

            If localTime2 <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Time Out of Service:</b> ")
                strBody.Append("           " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TOtransmitterServiceDueTo <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Transmitter is out of service due to:</b> ")
                strBody.Append("           " & TOtransmitterServiceDueTo & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If TOreturnToService <> "" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='650px'>")
                strBody.Append("            <b>Time the transmitter(s) are expected to return to service:</b> ")
                strBody.Append("           " & TOreturnToService & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If
        End If
    End Sub
End Class