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
Imports System.Text
Imports Microsoft.Office.Interop.Word
Imports System.Diagnostics

Partial Class FullReport
    Inherits System.Web.UI.Page
    'Help Functions from our App_Code
    Public HelpFunction As New HelpFunctions
    Public DBConStringHelper As New DBConStringHelp

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
    Public objDS3 As New System.Data.DataSet
    Public objDS4 As New System.Data.DataSet
    Public objDS5 As New System.Data.DataSet

    Public MrDataGrabber As New DataGrabber

    Dim globalReportFormatParameter As String


    Dim strBody As New StringBuilder("")


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim oBlackBerryReport As New BlackBerryReport(Request("IncidentID"))

        Response.Write(oBlackBerryReport.gStrTotalReport)
        Response.End()

        globalReportFormatParameter = Request("ReportFormat")

        If Page.IsPostBack = False Then

            Select Case globalReportFormatParameter

                Case "HTML"
                    strBody.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
                    strBody.Append("<html xmlns='http://www.w3.org/1999/xhtml'>")
                    strBody.Append("<head>")
                    strBody.Append("<title>SWO Situation Tracker</title>")
                    strBody.Append("</head>")
                    strBody.Append("<body>")
                Case "Excel"
                    'ExportToExcel()
                Case "Word"
                    strBody.Append("<html xmlns:o='urn:schemas-microsoft-com:office:office' " & _
        "xmlns:w='urn:schemas-microsoft-com:office:word'" & _
        "xmlns='http://www.w3.org/TR/REC-html40'>" & _
        "<head><title>Situational Awareness Report</title>")

                    strBody.Append( _
                               "<!--[if gte mso 9]>" & _
                               "<xml>" & _
                               "<w:WordDocument>" & _
                               "<w:View>Print</w:View>" & _
                               "</w:WordDocument>" & _
                               "</xml>" & _
                               "<![endif]-->")

                    strBody.Append( _
                             "<style>" & _
                             "<!-- /* Style Definitions               */@page Section1{size:8.5in 11.0in;" & _
                             "margin:0.0in 0.25in 0.25in " & _
                             "0.25in;mso-header-margin:.25in; " & _
                             "mso-footer-margin:.5in;mso-paper-source:0;font-size:10.0pt; }" & _
                             "div.Section1{page:Section1;}-->" & _
                             "</style></head>")

                    strBody.Append( _
                                "<body lang=EN-US style='tab-interval:.5in'>" & _
                                "<div class=Section1>")
                Case Else
                    'Do Nothing

            End Select


            strBody.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
            strBody.Append("<html xmlns='http://www.w3.org/1999/xhtml'>")
            strBody.Append("<head>")
            strBody.Append("<title>SWO Situation Tracker</title>")
            strBody.Append("</head>")
            strBody.Append("<body>")


            'strBody.Append("<table width='100%' align='center'>")
            'strBody.Append("<tr>")
            'strBody.Append("<td colspan='2' align='center'>")
            'strBody.Append(" <font size='7'><big><b>Full Report</b></big></font> ")
            'strBody.Append("</td>")
            ''strBody.Append("<td align='left'>")
            ''strBody.Append(" <img id='imgLogo' src='Images/SealLogo.jpg' alt='Logo Image' /> ")
            ''strBody.Append("</td>")
            'strBody.Append("</tr>")
            'strBody.Append("</table>")
            'Incident Worksheets will go here


            GetMainForm()

            GetWorkSheets()


            'If Not System.IO.File.Exists("C:\somefile.doc") = True Then

            '    Dim file As System.IO.FileStream
            '    file = System.IO.File.Create("C:\somefile.doc")
            '    file.Close()

            'End If

            ''System.IO.File.Copy("C:\foo\somefile.txt", "C:\bar\somefile.txt")

            ''System.IO.File.Move("C:\foo\somefile.txt", "C:\bar\somefile.txt")

            'My.Computer.FileSystem.WriteAllText("C:\somefile.doc", strBody.ToString(), True)



            'Display The HTML Page
            Response.Write(strBody.ToString())

            Select Case globalReportFormatParameter

                Case "HTML"
                    strBody.Append("</body>")
                    strBody.Append("</html>")
                Case "Excel"
                    'ExportToExcel()
                Case "Word"
                    strBody.Append( _
                    "</div></body></html>")

                    'Force this content to be downloaded    'as a Word document with the name of your choice    
                    Response.AppendHeader("Content-Type", "application/msword")
                    Response.AppendHeader("Content-disposition", _
                    "attachment; filename=IR.doc")
                    Response.Charset = ""

                Case Else
                    'Do Nothing
            End Select


        End If


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

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectIncidentByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

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
            Handled = HelpFunction.Convertdbnulls(objDR("Handled"))
            FacilityNameSceneDescription = HelpFunction.Convertdbnulls(objDR("FacilityNameSceneDescription"))
            Address = HelpFunction.Convertdbnulls(objDR("Address"))
            City = HelpFunction.Convertdbnulls(objDR("City"))
            Address2 = HelpFunction.Convertdbnulls(objDR("Address2"))
            Zip = HelpFunction.Convertdbnulls(objDR("Zip"))
            Street = HelpFunction.Convertdbnulls(objDR("Street"))
            Street2 = HelpFunction.Convertdbnulls(objDR("Street2"))
            City2 = HelpFunction.Convertdbnulls(objDR("City2"))
            AgencyDeptNotified = HelpFunction.Convertdbnulls(objDR("AgencyDeptNotified"))
            ObtainCoordinate = HelpFunction.Convertdbnulls(objDR("ObtainCoordinate"))
            CoordinateType = HelpFunction.Convertdbnulls(objDR("CoordinateType"))
            localLat = HelpFunction.ConvertdbnullsDbl(objDR("Lat"))
            localLong = HelpFunction.ConvertdbnullsDbl(objDR("Long"))
            localUSNG = HelpFunction.Convertdbnulls(objDR("USNG"))
            SeverityID = HelpFunction.ConvertdbnullsInt(objDR("SeverityID"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        '=======================================================================================
        'Response.Write("Hello")
        'Response.End()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectLastInitialReportByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            localInitialReport = HelpFunction.Convertdbnulls(objDR("InitialReport"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()
        '=======================================================================================
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectLastUpdateReportByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            LatestUpdate = HelpFunction.Convertdbnulls(objDR("UpdateReport"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()



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
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

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

            Response.Write(ex.ToString)
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

        'Gets rid of Last ,
        If localAllCounties <> "" Then
            localAllCounties = localAllCounties.Remove(localAllCounties.Length - 2, 2)
        Else
            localAllCounties = " NO COUNTIES ADDED AT THIS TIME"
        End If

        'IncidentNumber
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


        'IncidentNumber
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
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()

        If objDR.Read() Then

            'there are records
            objDR.Close()
            objDR = objCmd.ExecuteReader()


            While objDR.Read

                localThisSituationInvolves = localThisSituationInvolves & CStr(objDR.Item("IncidentType")) & ", "

            End While

        End If


        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()


        'Gets rid of Last ,
        If localThisSituationInvolves <> "" Then
            localThisSituationInvolves = localThisSituationInvolves.Remove(localThisSituationInvolves.Length - 2, 2)
        Else
            localThisSituationInvolves = " NO INVOLVEMENTS ADDED AT THIS TIME"
        End If


        'Report Name
        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td align='center'><img alt='Florida Seal' src='http://www.floridadisaster.org/images/IRimages/SERT Logo-yellow arrows.jpg' /></td>")
        strBody.Append("<td align='center'style='font-size:1.5em; font:Arial' >Florida Division of Emergency Management")
        strBody.Append("<BR>")
        strBody.Append("State Watch Office")
        strBody.Append("<BR>")
        strBody.Append("<b>Situational Awareness Report</b>")
        strBody.Append("")
        strBody.Append("</font></td>")
        strBody.Append("<td align='center'><img alt='SERT Logo' src='http://www.floridadisaster.org/images/IRimages/FloridaSeal3.jpg' /></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")


        '/////////////////////////////////////////////////////////////////////////////////////////////////////

        If IsThisADrill = "Yes" Then

            strBody.Append("<table width='100%' align='center' border='1' style='border-color:#ff0000; background-color:#ff0000;'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='center' style='background-color:#ff0000; height:25px; color:White; font:Arial; font-size:1.5em; border-style:solid; border-color:#ff0000' >")
            strBody.Append("        <b>THIS IS A DRILL</b>     ")
            strBody.Append("        </td>")
            strBody.Append("        <td align='center' style='background-color:White; height:25px; color:#ff0000; font:Arial; font-size:1.5em; border-style:solid; border-color:#ff0000' >")
            strBody.Append("        <b>THIS IS A DRILL</b>     ")
            strBody.Append("        </td>")
            strBody.Append("        <td align='center' style='background-color:#ff0000; height:25px; color:White; font:Arial; font-size:1.5em; border-style:solid; border-color:#ff0000 ' >")
            strBody.Append("        <b>THIS IS A DRILL</b>     ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        End If



        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center' style='background-color:White; height:25px; color:#ff0000; font:Arial; font-size:1.7em;'> ")
        strBody.Append("            &nbsp; <b>// CONFIDENTIAL - FOUO //</b> &nbsp; ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center' style='background-color:White; height:25px; color:#ff0000; font:Arial; font-size:1.1em;'> ")
        strBody.Append("            <b>This report is exempt from public records disclosure pursuant to § 119.071 F.S.</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")


        strBody.Append("<table width='100%' align='center' border='1' style='border-color:#000000; background-color:#000000;'>")
        strBody.Append("    <tr>")


        'Response.Write(StateAssistance)
        'Response.End()

        If StateAssistance = "Yes" Then

            strBody.Append("        <td width='450px' align='center' style='background-color:#d4d4d4; color:#ff0000; font:Arial; font-size:1.5em; border-color:#000000' >")
            strBody.Append("        <b>STATE ASSISTANCE REQUESTED</b>     ")
            strBody.Append("        </td>")

        ElseIf StateAssistance = "No" Then

            strBody.Append("        <td width='450px' align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
            strBody.Append("        <b>NO STATE ASSISTANCE REQUESTED</b>     ")
            strBody.Append("        </td>")

        End If


        strBody.Append("        <td width='225px' align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("        <b>Report #:</b>     ")
        strBody.Append("        </td>")

        strBody.Append("        <td width='225px' align='center' style='background-color:White; color:000000; font:Arial; font-size:1.5em; border-color:#000000 ' >")
        strBody.Append("        " & localYear & "-" & CStr(localNumber) & "  ")
        strBody.Append("        </td>")

        strBody.Append("        <td width='225px' align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("        <b>Status:</b>     ")
        strBody.Append("        </td>")

        strBody.Append("        <td width='225px' align='center' style='background-color:White; color:000000; font:Arial; font-size:1.5em; border-color:#000000 ' >")
        strBody.Append("        <b>" & MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentStatus", "IncidentStatus", "IncidentStatusID", MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("IncidentStatusID", "Incident", "IncidentID", Request("IncidentID")).ToString).ToString & " </b> ")
        strBody.Append("        </td>")

        strBody.Append("    </tr>")



        strBody.Append("    <tr>")

        strBody.Append("        <td width='450px' align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("        <b>Reported to State Watch Office on:</b>     ")
        strBody.Append("        </td>")

        strBody.Append("        <td colspan='2' width='450px' align='center' style='background-color:White; color:000000; font:Arial; font-size:1.5em; border-color:#000000 ' >")
        strBody.Append("        " & ReportedToSWODate & " &nbsp; " & ReportedToSWOTime & ":" & ReportedToSWOTime2 & "  ")
        strBody.Append("        </td>")

        strBody.Append("        <td colspan='2' width='450px' align='center' style='background-color:" & localSeverityColor & "; color:000000; font:Arial; font-size:1.5em; border-color:#000000 ' >")
        strBody.Append("        <b>" & localSeverity & " </b> ")
        strBody.Append("        </td>")

        strBody.Append("    </tr>")



        strBody.Append("    <tr>")

        strBody.Append("        <td  align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("        <b>Description:</b>     ")
        strBody.Append("        </td>")

        strBody.Append("        <td colspan='4' align='Left' style='background-color:White; color:000000; font:Arial; font-size:1.5em; border-color:#000000 ' >")
        strBody.Append("        Which Discription will be used here? ")
        strBody.Append("        </td>")

        strBody.Append("    </tr>")


        strBody.Append("    <tr>")

        strBody.Append("        <td align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("        <b>This situation involves:</b>     ")
        strBody.Append("        </td>")

        strBody.Append("        <td colspan='4' align='Left' style='background-color:White; color:000000; font:Arial; font-size:1.5em; border-color:#000000 ' >")
        strBody.Append("        " & localThisSituationInvolves & "  ")
        strBody.Append("        </td>")

        strBody.Append("    </tr>")



        strBody.Append("</table>")

        '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        strBody.Append("    <br>")

        '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        strBody.Append("<table width='100%' align='center' border='1' style='border-color:#000000; background-color:#000000;'>")

        strBody.Append("    <tr>")

        strBody.Append("        <td width='527px' align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("        <b><u>Initial Report:</u></b>     ")
        strBody.Append("        </td>")

        strBody.Append("        <td width='533px' align='right' style='background-color:White; color:000000; font:Arial; font-size:1.5em; border-color:#000000 ' >")
        strBody.Append("        <b>Incident Occurred:</b>  ")
        strBody.Append("        </td>")

        strBody.Append("        <td width='535px' align='center' style='background-color:White; color:000000; font:Arial; font-size:1.5em; border-color:#000000 ' >")
        strBody.Append("        <b>" & IncidentOccurredDate & " &nbsp; " & IncidentOccurredTime & ":" & IncidentOccurredTime2 & " </b> ")
        strBody.Append("        </td>")

        strBody.Append("    </tr>")


        strBody.Append("    <tr>")

        strBody.Append("        <td colspan='3' align='left' style='background-color:White; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("        " & localInitialReport & "     ")
        strBody.Append("        </td>")

        strBody.Append("    </tr>")


        strBody.Append("</table>")

        '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        strBody.Append("    <br>")

        '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        strBody.Append("<table width='100%' align='center' border='1' style='border-color:#000000; background-color:#000000;'>")

        strBody.Append("    <tr>")

        strBody.Append("        <td  width='537px' align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("        <b><u>Most Recent Update:</u></b>     ")
        strBody.Append("        </td>")

        strBody.Append("        <td  align='left' style='background-color:White; color:000000; font:Arial; font-size:1.5em; border-color:#000000 ' >")
        strBody.Append("        " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("LastUpdated", "Incident", "IncidentID", Request("IncidentID")).ToString & "  ")
        strBody.Append("        </td>")

        strBody.Append("    </tr>")


        strBody.Append("    <tr>")

        strBody.Append("        <td colspan='2' align='left' style='background-color:White; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("        " & LatestUpdate & "     ")
        strBody.Append("        </td>")

        strBody.Append("    </tr>")


        strBody.Append("</table>")

        '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        strBody.Append("    <br>")

        '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        strBody.Append("<table width='100%' align='center' style='background-color:White;'>")

        strBody.Append("    <tr>")

        strBody.Append("        <td valign='Top' align='center' style='border-color:#000000'>")
        strBody.Append("            <table width='100%' align='center' border='1' style='border-color:#000000; background-color:#000000;'>")

        strBody.Append("                <tr>")

        strBody.Append("                    <td colspan='2' align='center' style='width:900px; background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("                    <b><u>Affected Counties or DEM Regions:</b>     ")
        strBody.Append("                    </td>")

        strBody.Append("                </tr>")


        strBody.Append("                <tr style='height:136px'>")

        strBody.Append("                    <td colspan='2' valign='Top' align='left' style='background-color:White; font:Arial; font-size:1.5em; color:#000000; border-color:#000000' >")
        strBody.Append("                    " & localAllCounties & "     ")
        strBody.Append("                    </td>")

        strBody.Append("                </tr>")

        strBody.Append("                <tr>")

        strBody.Append("                    <td colspan='2' align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("                    <b><u>Facility Name or Description:</b>     ")
        strBody.Append("                    </td>")

        strBody.Append("                </tr>")


        strBody.Append("                <tr style='height:65px' >")

        strBody.Append("                    <td colspan='2' align='left' valign='Top' style='background-color:White; font:Arial; font-size:1.5em; color:#000000; border-color:#000000' >")
        strBody.Append("                    " & FacilityNameSceneDescription & "     ")
        strBody.Append("                    </td>")

        strBody.Append("                </tr>")

        strBody.Append("                <tr>")

        strBody.Append("                    <td colspan='2' align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("                    <b><u>Incident Location:</b>     ")
        strBody.Append("                    </td>")

        strBody.Append("                </tr>")


        strBody.Append("                <tr style='height:130px' >")

        strBody.Append("                    <td colspan='2' valign='Top' align='left' style='background-color:White; font:Arial; font-size:1.5em; color:#000000; border-color:#000000' >")


        If ObtainCoordinate = "AddressCity" Then

            strBody.Append("                   <b>Address:</b> " & Address & " <b>City:</b> " & City & "  ")

        ElseIf ObtainCoordinate = "AddressZip" Then

            strBody.Append("                   <b>Address:</b> " & Address2 & " <b>Zip:</b> " & Zip & "  ")

        ElseIf ObtainCoordinate = "Intersection" Then

            strBody.Append("                   <b>Street 1:</b> " & Street & " <b>Street 2:</b> " & Zip & " <b>City:</b> " & City2 & " ")

        Else

            strBody.Append("                    " & "N/A" & "     ")

        End If

        strBody.Append("                    </td>")
        strBody.Append("                </tr>")
        strBody.Append("            </table>")
        strBody.Append("        </td>")


        strBody.Append("        <td align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("            <table width='100%' align='center' border='1' style='border-color:#000000; background-color:#000000;'>")

        strBody.Append("                <tr tyle='height:426px'>")

        If localLat = 0.0 Or localLong = 0.0 Then
            strBody.Append("                    <td align='center' style='background-color:White; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
            strBody.Append("                        <img src='http://www.floridadisaster.org/images/IRimages/NoImage.jpg' width='400' height='426' />   ")
            strBody.Append("                    </td>")
        Else
            strBody.Append("                    <td align='center' style='background-color:White; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
            strBody.Append("                        <a href='http://www.floridadisaster.org/gis/kml/viewer.htm?lat=" & localLat & "&lng=" & localLong & "&zoom=12' target='_blank'><img src='http://maps.googleapis.com/maps/api/staticmap?center=" & localLat & "," & localLong & "&markers=size:mid|color:red|" & localLat & "," & localLong & "&zoom=14&size=400x426&maptype=street&format=jpg&key=ABQIAAAAaa6B5ZMUVanPrZJU5dhtshRzymbT3klSnJpNv7EI1uNYq_UBqhTmwXd4YDorUwqRsabizyja-ZgPoQ" & "'alt='Incident Location' /></a>   ")
            strBody.Append("                    </td>")
        End If



        strBody.Append("                </tr>")

        strBody.Append("            </table>")
        strBody.Append("        </td>")

        strBody.Append("    </tr>")

        strBody.Append("</table>")

        '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        strBody.Append("<table width='100%' align='center' border='1' style='border-color:#000000; background-color:#000000;'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='100%' align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("            <b>Contact Information:</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


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
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

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
                ReportingPartyTypeInfo = "<b> First Name: </b>" & localReportingPartyTypeFirstName
            End If

            If localReportingPartyTypeLastName <> "" Then
                ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<b> |Last Name: </b>" & localReportingPartyTypeLastName
            End If

            If localReportingPartyTypeCallBackNumber1 <> "" Then
                ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<b> |Call Back Number 1: </b>" & localReportingPartyTypeCallBackNumber1
            End If

            If localReportingPartyTypeCallBackNumber2 <> "" Then
                ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<b> |Call Back Number 2: </b>" & localReportingPartyTypeCallBackNumber2
            End If

            If localReportingPartyTypeEmail <> "" Then
                ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<b> |Email: </b>" & localReportingPartyTypeEmail
            End If

            If localReportingPartyTypeAddress <> "" Then
                ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<b> |Address: </b>" & localReportingPartyTypeAddress
            End If

            If localReportingPartyTypeCity <> "" Then
                ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<b> |City: </b>" & localReportingPartyTypeCity
            End If

            If localReportingPartyTypeState <> "" Then
                ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<b> |State: </b>" & localReportingPartyTypeState
            End If

            If localReportingPartyTypeZipcode <> "" Then
                ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<b> |Zipcode: </b>" & localReportingPartyTypeZipcode
            End If

            If localReportingPartyTypeRepresents <> "" Then
                ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<b> |Represents: </b>" & localReportingPartyTypeRepresents
            End If

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
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

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
                ResponsiblePartyInfo = "<b> First Name: </b>" & localResponsiblePartyFirstName
            End If

            If localResponsiblePartyLastName <> "" Then
                ResponsiblePartyInfo = ResponsiblePartyInfo & "<b> |Last Name: </b>" & localResponsiblePartyLastName
            End If

            If localResponsiblePartyCallBackNumber1 <> "" Then
                ResponsiblePartyInfo = ResponsiblePartyInfo & "<b> |Call Back Number 1: </b>" & localResponsiblePartyCallBackNumber1
            End If

            If localResponsiblePartyCallBackNumber2 <> "" Then
                ResponsiblePartyInfo = ResponsiblePartyInfo & "<b> |Call Back Number 2: </b>" & localResponsiblePartyCallBackNumber2
            End If

            If localResponsiblePartyEmail <> "" Then
                ResponsiblePartyInfo = ResponsiblePartyInfo & "<b> |Email: </b>" & localResponsiblePartyEmail
            End If

            If localResponsiblePartyAddress <> "" Then
                ResponsiblePartyInfo = ResponsiblePartyInfo & "<b> |Address: </b>" & localResponsiblePartyAddress
            End If

            If localResponsiblePartyCity <> "" Then
                ResponsiblePartyInfo = ResponsiblePartyInfo & "<b> |City: </b>" & localResponsiblePartyCity
            End If

            If localResponsiblePartyState <> "" Then
                ResponsiblePartyInfo = ResponsiblePartyInfo & "<b> |State: </b>" & localResponsiblePartyState
            End If

            If localResponsiblePartyZipcode <> "" Then
                ResponsiblePartyInfo = ResponsiblePartyInfo & "<b> |Zipcode: </b>" & localResponsiblePartyZipcode
            End If

            If localResponsiblePartyRepresents <> "" Then
                ResponsiblePartyInfo = ResponsiblePartyInfo & "<b> |Represents: </b>" & localResponsiblePartyRepresents
            End If

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
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

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
                OnSceneContactInfo = "<b> First Name: </b>" & localOnSceneContactFirstName
            End If

            If localOnSceneContactLastName <> "" Then
                OnSceneContactInfo = OnSceneContactInfo & "<b> |Last Name: </b>" & localOnSceneContactLastName
            End If

            If localOnSceneContactCallBackNumber1 <> "" Then
                OnSceneContactInfo = OnSceneContactInfo & "<b> |Call Back Number 1: </b>" & localOnSceneContactCallBackNumber1
            End If

            If localOnSceneContactCallBackNumber2 <> "" Then
                OnSceneContactInfo = OnSceneContactInfo & "<b> |Call Back Number 2: </b>" & localOnSceneContactCallBackNumber2
            End If

            If localOnSceneContactEmail <> "" Then
                OnSceneContactInfo = OnSceneContactInfo & "<b> |Email: </b>" & localOnSceneContactEmail
            End If

            If localOnSceneContactAddress <> "" Then
                OnSceneContactInfo = OnSceneContactInfo & "<b> |Address: </b>" & localOnSceneContactAddress
            End If

            If localOnSceneContactCity <> "" Then
                OnSceneContactInfo = OnSceneContactInfo & "<b> |City: </b>" & localOnSceneContactCity
            End If

            If localOnSceneContactState <> "" Then
                OnSceneContactInfo = OnSceneContactInfo & "<b> |State: </b>" & localOnSceneContactState
            End If

            If localOnSceneContactZipcode <> "" Then
                OnSceneContactInfo = OnSceneContactInfo & "<b> |Zipcode: </b>" & localOnSceneContactZipcode
            End If

            If localOnSceneContactRepresents <> "" Then
                OnSceneContactInfo = OnSceneContactInfo & "<b> |Represents: </b>" & localOnSceneContactRepresents
            End If

        Else
            OnSceneContactInfo = OnSceneContactType
        End If

        '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        strBody.Append("<table width='100%' align='center' border='1' style='border-color:#000000; background-color:#000000;'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td width='25%' align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("            <b>Reporting Party:</b>     ")
        strBody.Append("        </td>")
        strBody.Append("        <td align='left' style='background-color:White; color:000000; font:Arial; font-size:1.5em; border-color:#000000 ' >")
        strBody.Append("            " & ReportingPartyTypeInfo & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("    <tr>")
        strBody.Append("        <td width='25%' align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("            <b>Responsible Party:</b>     ")
        strBody.Append("        </td>")
        strBody.Append("        <td align='left' style='background-color:White; color:000000; font:Arial; font-size:1.5em; border-color:#000000 ' >")
        strBody.Append("            " & ResponsiblePartyInfo & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("    <tr>")
        strBody.Append("        <td width='25%' align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("            <b>On-Scene Contact:</b>     ")
        strBody.Append("        </td>")
        strBody.Append("        <td align='left' style='background-color:White; color:000000; font:Arial; font-size:1.5em; border-color:#000000 ' >")
        strBody.Append("            " & OnSceneContactInfo & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        strBody.Append("    <br>")

        '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        strBody.Append("<table width='100%' align='center' border='1' style='border-color:#000000; background-color:#000000;'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='100%' align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
        strBody.Append("            INCIDENT-SPECIFIC DATA WORKSHEETS FOR THIS SITUATION ARE LISTED BELOW")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        'strBody.Append("<table width='100%' align='center'>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        'strBody.Append("             ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table width='100%' align='center'>")
        'strBody.Append("<tr>")
        'strBody.Append("<td width='25%' align='left'> Created By: " & MrDataGrabber.GrabUserFullNameByUserID(MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("CreatedByID", "Incident", "IncidentID", Request("IncidentID"))).ToString & "</font></td>")
        'strBody.Append("<td width='25%' align='left'> Created Date/Time: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("DateCreated", "Incident", "IncidentID", Request("IncidentID")).ToString & "</font></td>")
        'strBody.Append("<td width='25%' align='left'> Last Updated By: " & MrDataGrabber.GrabUserFullNameByUserID(MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("LastUpdatedByID", "Incident", "IncidentID", Request("IncidentID"))).ToString & "</font></td>")
        'strBody.Append("<td width='25%' align='left'> Last Updated Date/Time: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("LastUpdated", "Incident", "IncidentID", Request("IncidentID")).ToString & "</font></td>")
        'strBody.Append("</tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table width='100%' align='center'>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td style='background: #000 repeat;' ")
        'strBody.Append("             ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table width='100%' align='center'>")
        'strBody.Append("<tr>")
        'strBody.Append("<td width='25%' align='left'> Report Number: 2011-1</font></td>")
        'strBody.Append("<td width='25%' align='left'> Incident Status: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentStatus", "IncidentStatus", "IncidentStatusID", MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("IncidentStatusID", "Incident", "IncidentID", Request("IncidentID")).ToString).ToString & " </font></td>")
        'strBody.Append("<td width='25%' align='left'> Is this a drill? " & IsThisADrill & "</font></td>")
        'strBody.Append("<td width='25%' align='left'> State Assistance? " & StateAssistance & "</font></td>")
        'strBody.Append("</tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table width='100%' align='center'>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        'strBody.Append("             ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")


        'strBody.Append("<table width='100%' align='center'>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        'strBody.Append("            <b>Initial Report: </b> " & localInitialReport & "")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")


        'strBody.Append("<table width='100%' align='center'>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        'strBody.Append("            <b>Most Recent Update: </b> " & LatestUpdate & "")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table width='100%' align='center'>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        'strBody.Append("            <b>Incident Details: </b> ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table width='100%' align='center'>")
        'strBody.Append("<tr>")
        'strBody.Append("<td width='25%' align='left'> Date/Time Incident Occurred: " & IncidentOccurredDate & " &nbsp; " & IncidentOccurredTime & ":" & IncidentOccurredTime2 & " </font></td>")
        'strBody.Append("<td width='25%' align='left'> Date/Time Reported to SWO: " & ReportedToSWODate & " &nbsp; " & ReportedToSWOTime & ":" & ReportedToSWOTime2 & " </font></td>")
        'strBody.Append("<td width='25%' align='left'> This incident is being handled: " & Handled & "</font></td>")
        'strBody.Append("<td width='25%' align='left'> </font></td>")
        'strBody.Append("</tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table width='100%' align='center'>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        'strBody.Append("             ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table width='100%' align='center'>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='center'>")
        'strBody.Append("            <b>Radio Buttons Info Will GO here</b>")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table width='100%' align='center'>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'>")
        'strBody.Append("            <b>Affected Counties: </b>" & localAllCounties & "")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table width='100%' align='center'>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        'strBody.Append("             ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table width='100%' align='center'>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'>")
        'strBody.Append("            Dept/agencies noified, responding, scene: " & AgencyDeptNotified & "")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")


    End Sub

    Protected Sub GetWorkSheets()



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
                ElseIf CStr(objDR.Item("IncidentType")) = "Civil Disturbance" Then
                    GetCivilDisturbance(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Criminal Activity" Then
                    GetCriminalActivity(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Dam Failure" Then
                    GetDamFailure(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "DEM Incidents/Notifications/Reports" Then
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
                ElseIf CStr(objDR.Item("IncidentType")) = "Security Threat" Then
                    GetSecurityThreat(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Utility Disruption or Emergency" Then
                    GetUtilityDisruptionEmergency(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Wastewater or Effluent Release" Then
                    GetWastewater(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                ElseIf CStr(objDR.Item("IncidentType")) = "Weather Advisories and Reports" Then
                    GetWeather(CStr(objDR.Item("IncidentTypeID")), CStr(objDR.Item("IncidentIncidentTypeID")))
                End If

            End While

        End If

        '


        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()


        'MarineIncident

    End Sub

    Protected Sub test()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        DBConStringHelper.PrepareConnection(objConn) 'open the connection

        objCmd = New SqlCommand("[spSelectLatestUpdateByIncidentID]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))




        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()

        If objDR.Read() Then

            'strBody.Append("<tr>")
            'strBody.Append("<td align='left' width='25%'><font size='5'><b>Date</b></font></td>")
            'strBody.Append("<td align='left' width='75%'><font size='5'><b>Update</b></font></td>")
            'strBody.Append("</tr>")

            objDR.Close()
            objDR = objCmd.ExecuteReader()

            While objDR.Read

                'strBody.Append("<tr>")
                'strBody.Append("<td align='left'><font size='5'>" & objDR.Item("UpdateDate") & "</font></td>")
                'strBody.Append("<td align='left'><font size='5'>" & objDR.Item("MostRecentUpdate") & "</font></td>")
                'strBody.Append("</tr>")
                'strBody.Append("<br>")


            End While

        Else
            'there are no records
            strBody.Append("<tr><td colspan='2' align='center'>&nbsp;</td><tr>")
            strBody.Append("<tr><td colspan='2' align='center'><b>No Records</b></td><tr>")
        End If

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()

    End Sub


    'Individual Worksheets
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
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Road Closure or DOT Issue</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Roadway Name and/or number: " & RoadwayNameNumber & "</font></td>")
        strBody.Append("<td width='25%' align='left'> At: " & At & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Mile Marker: " & MileMarker & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Exit Ramp: " & ExitRamp & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Cross Street 1 or Intersection: " & CrossStreet1Intersection & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Cross Street 2: " & CrossStreet2 & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Duration of closure (if known): " & DurationOfClosure & "</font></td>")
        strBody.Append("<td width='25%' align='left'> What department/agency directed the closure: " & DepartmentAgencyDirectedClosure & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

    End Sub

    Private Sub GetHazardousMaterials(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)

        Dim localTime As String = ""
        Dim localTime2 As String = ""

        Dim SubType As String = ""
        Dim Situation As String = ""

        'Biological Hazard Start====================================================================
        Dim CommonName As String = ""
        Dim ScientificName As String = ""
        Dim QuantityDescription As String = ""
        Dim ContainerDeviceDescription As String = ""
        Dim BiologicalTotalQuantity As String = ""
        Dim BiologicalQuantityReleased As String = ""
        'Biological Hazard End======================================================================


        'Chemical Agent Start=======================================================================
        Dim AgentType As String = ""
        Dim AgentName As String = ""
        Dim AgentContainerDeviceDescription As String = ""
        Dim AgentTotalQuantity As String = ""
        Dim AgentQuantityReleased As String = ""
        'Chemical Agent End=========================================================================


        'Radiological Material Start================================================================
        Dim RadiationType As String = ""
        Dim IsotopeName As String = ""
        Dim ContainerDeviceInstrumentDescription As String = ""
        Dim RadiationTotalQuantity As String = ""
        Dim DOHBureauNotified As String = ""
        'Radiological Material End==================================================================

        'Toxic Industrial Chemical Start============================================================
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
        Dim ChemicalRateOfRelease As String = ""
        Dim ChemicalReleased As String = ""
        Dim CauseOfRelease As String = ""
        Dim ReasonLateReport As String = ""
        Dim StormDrainsAffected As String = ""
        Dim WaterwaysAffected As String = ""
        Dim WaterwaysAffectedText As String = ""
        Dim CallbackDEPRequested As String = ""
        Dim CallbackDEPRequestedDDLValue As String = ""
        Dim Evacuations As String = ""
        Dim MajorRoadwaysClosed As String = ""
        Dim Injury As String = ""
        Dim InjuryText As String = ""
        Dim Fatality As String = ""
        Dim FatalityText As String = ""

        Dim ChemicalQuantityReleased As String = ""
        Dim TimeReleaseDiscovered As String = ""
        Dim TimeReleaseSecured As String = ""
        'Toxic Industrial Chemical End==============================================================



        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectHazardousMaterialsByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then

            'Biological Hazard Start====================================================================
            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            CommonName = HelpFunction.Convertdbnulls(objDR2("CommonName"))
            ScientificName = HelpFunction.Convertdbnulls(objDR2("ScientificName"))
            QuantityDescription = HelpFunction.Convertdbnulls(objDR2("QuantityDescription"))
            ContainerDeviceDescription = HelpFunction.Convertdbnulls(objDR2("ContainerDeviceDescription"))
            BiologicalTotalQuantity = HelpFunction.Convertdbnulls(objDR2("BiologicalTotalQuantity"))
            BiologicalQuantityReleased = HelpFunction.Convertdbnulls(objDR2("BiologicalQuantityReleased"))
            'Biological Hazard End======================================================================

            'Chemical Agent Start=======================================================================
            AgentType = HelpFunction.Convertdbnulls(objDR2("AgentType"))
            AgentName = HelpFunction.Convertdbnulls(objDR2("AgentName"))
            AgentContainerDeviceDescription = HelpFunction.Convertdbnulls(objDR2("AgentContainerDeviceDescription"))
            AgentTotalQuantity = HelpFunction.Convertdbnulls(objDR2("AgentTotalQuantity"))
            AgentQuantityReleased = HelpFunction.Convertdbnulls(objDR2("AgentQuantityReleased"))
            'Chemical Agent End=========================================================================

            'Radiological Material Start================================================================
            RadiationType = HelpFunction.Convertdbnulls(objDR2("RadiationType"))
            IsotopeName = HelpFunction.Convertdbnulls(objDR2("IsotopeName"))
            ContainerDeviceInstrumentDescription = HelpFunction.Convertdbnulls(objDR2("ContainerDeviceInstrumentDescription"))
            RadiationTotalQuantity = HelpFunction.Convertdbnulls(objDR2("RadiationTotalQuantity"))
            DOHBureauNotified = HelpFunction.Convertdbnulls(objDR2("DOHBureauNotified"))
            'Radiological Material End==================================================================


            'Toxic Industrial Chemical Start============================================================
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
            ChemicalRateOfRelease = HelpFunction.Convertdbnulls(objDR2("ChemicalRateOfRelease"))
            ChemicalReleased = HelpFunction.Convertdbnulls(objDR2("ChemicalReleased"))
            CauseOfRelease = HelpFunction.Convertdbnulls(objDR2("CauseOfRelease"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR2("TimeReleaseDiscovered")))
            localTime2 = CStr(HelpFunction.Convertdbnulls(objDR2("TimeReleaseSecured")))
            ReasonLateReport = HelpFunction.Convertdbnulls(objDR2("ReasonLateReport"))
            StormDrainsAffected = HelpFunction.Convertdbnulls(objDR2("StormDrainsAffected"))
            WaterwaysAffected = HelpFunction.Convertdbnulls(objDR2("WaterwaysAffected"))
            WaterwaysAffectedText = HelpFunction.Convertdbnulls(objDR2("WaterwaysAffectedText"))
            CallbackDEPRequested = HelpFunction.Convertdbnulls(objDR2("CallbackDEPRequested"))
            CallbackDEPRequestedDDLValue = HelpFunction.Convertdbnulls(objDR2("CallbackDEPRequestedDDLValue"))
            Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            MajorRoadwaysClosed = HelpFunction.Convertdbnulls(objDR2("MajorRoadwaysClosed"))
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))

            ChemicalQuantityReleased = HelpFunction.Convertdbnulls(objDR2("ChemicalQuantityReleased"))
            'Toxic Industrial Chemical End==============================================================



        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        TimeReleaseDiscovered = Left(localTime, 2) & ":" & Right(localTime, 2)
        TimeReleaseSecured = Left(localTime2, 2) & ":" & Right(localTime2, 2)



        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Hazardous Materials</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        If SubType = "Biological Hazard" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
            strBody.Append("            <b>Biological Hazard</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Common Name: " & CommonName & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Scientific Name: " & ScientificName & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Quantity Description: " & QuantityDescription & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Container or device description: " & ContainerDeviceDescription & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Total quantity: " & BiologicalTotalQuantity & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Quantity released: " & BiologicalQuantityReleased & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Chemical Agent" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
            strBody.Append("            <b>Chemical Agent</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Anticipated State assistance Need: " & AgentType & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Agent name: " & AgentName & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Container or device description: " & AgentContainerDeviceDescription & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Total quantity: " & AgentTotalQuantity & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Quantity released: " & AgentQuantityReleased & "</font></td>")
            strBody.Append("<td width='33%' align='left'> &nbsp; </font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Radiological Material" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
            strBody.Append("            <b>Radiological Material</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Anticipated State assistance Need: " & RadiationType & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Agent name: " & IsotopeName & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Container or device description: " & ContainerDeviceInstrumentDescription & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Total quantity: " & RadiationTotalQuantity & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Local or regional assistance requested: " & DOHBureauNotified & "</font></td>")
            strBody.Append("<td width='33%' align='left'> &nbsp; </font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Toxic Industrial Chemical" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
            strBody.Append("            <b>Toxic Industrial Chemical</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Chemical Name: " & ChemicalName & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Index Name: " & IndexName & "</font></td>")
            strBody.Append("<td width='25%' align='left'> CAS Number: " & CASNumber & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Section 304 Reportable Quantity: " & CERCLAReportableQuantity & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> CERCLA Reportable Quantity: " & CERCLAReportableQuantity & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Chemical State: " & ChemicalState & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Source / Container: " & SourceContainer & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Diameter of the Pipeline: " & DiameterPipeline & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Unbroken end of the pipe connected to: " & UnbrokenEndPipeConnectedTo & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Total source/container volume: " & TotalSourceContainerVolume & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Quantity released: " & ChemicalRateOfRelease & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Rate of release: " & ChemicalReleased & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Cause of release: " & CauseOfRelease & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Time the release was discovered: " & TimeReleaseDiscovered & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Time the release was secured: " & TimeReleaseSecured & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Reason for late report, if applicable: " & ReasonLateReport & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Were any storm drains affected? " & StormDrainsAffected & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Were any waterways affected? " & WaterwaysAffected & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Name(s) of waterways: " & WaterwaysAffectedText & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Is a callback from DEP requested? " & CallbackDEPRequested & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='100%' align='left'> DEP Contact: " & CallbackDEPRequestedDDLValue & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


        End If

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Are there any evacuations?" & Evacuations & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are any major roadways closed? " & MajorRoadwaysClosed & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there Injuries? " & Injury & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number and Severity of Injuries: " & InjuryText & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='50%' align='left'> Are there Fatalities? " & Fatality & "</font></td>")
        strBody.Append("<td width='50%' align='left'> Number and location: " & FatalityText & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")



        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")


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
        Dim Injury As String = ""
        Dim InjuryText As String = ""
        Dim Fatality As String = ""
        Dim FatalityText As String = ""
        Dim HazMatOnBoard As String = ""
        Dim FuelPetroleumSpills As String = ""


        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectVehicleByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            Injury = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            HazMatOnBoard = HelpFunction.Convertdbnulls(objDR2("HazMatOnBoard"))
            FuelPetroleumSpills = HelpFunction.Convertdbnulls(objDR2("FuelPetroleumSpills"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Vehicle</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Number of vehicles involved: " & VehiclesInvolvedNumber & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Type(s) of vehicles: " & VehicleType & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number of people involved: " & PeopleInvolvedNumber & "</font></td>")
        strBody.Append("<td width='25%' align='left'> If commercial carrier, Owned/Operated By: " & CommercialCarrierOwnedOperatedBy & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> What is the cause the incident (if known)? " & IncidentCause & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Is there a fire? " & Fire & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there Injuries? " & Injury & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number and Severity of Injuries: " & InjuryText & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Are there Fatalities? " & Fatality & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number and location: " & FatalityText & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there any hazardous materials onboard? " & HazMatOnBoard & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there any fuel or Petroleum Spills " & FuelPetroleumSpills & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")



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
        Dim Injury As String = ""
        Dim InjuryText As String = ""
        Dim Fatality As String = ""
        Dim FatalityText As String = ""
        Dim StructuresRoadwaysInvolved As String = ""
        Dim StructuresRoadwaysInvolvedText As String = ""
        Dim HazMatOnboard As String = ""
        Dim FuelPetroleumSpills As String = ""
        Dim Evacuations As String = ""
        Dim DepartmentAgencyResponding As String = ""
        Dim DepartmentAgencyNotified As String = ""



        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectAircraftIncidentByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            StructuresRoadwaysInvolved = HelpFunction.Convertdbnulls(objDR2("StructuresRoadwaysInvolved"))
            StructuresRoadwaysInvolvedText = HelpFunction.Convertdbnulls(objDR2("StructuresRoadwaysInvolvedText"))
            HazMatOnboard = HelpFunction.Convertdbnulls(objDR2("HazMatOnboard"))
            FuelPetroleumSpills = HelpFunction.Convertdbnulls(objDR2("FuelPetroleumSpills"))
            Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            DepartmentAgencyResponding = HelpFunction.Convertdbnulls(objDR2("DepartmentAgencyResponding"))
            DepartmentAgencyNotified = HelpFunction.Convertdbnulls(objDR2("DepartmentAgencyNotified"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()


        If MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) <> "Not Currently Available" Then

            strBody.Append("<br>")
            strBody.Append("<br>")

            '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            strBody.Append("<table width='100%' align='center' border='1' style='border-color:#000000; background-color:#000000;'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='100%' align='center' style='background-color:#d4d4d4; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")
            strBody.Append("            AIRCRAFT INCIDENT")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
            '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            strBody.Append("<table width='100%' align='center' border='1' style='border-color:#000000; background-color:#000000;'>")

            strBody.Append("    <tr>")

            strBody.Append("        <td align='center' width='300px' style='background-color:#ffffff; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")

            strBody.Append("                    <b>" & SubType & "</b>     ")

            strBody.Append("        </td>")

            strBody.Append("        <td align='center' style='background-color:#ffffff; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")

            strBody.Append("                    <b>" & Situation & "</b>     ")

            strBody.Append("        </td>")

            strBody.Append("        <td align='left' style='background-color:#ffffff; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")

            strBody.Append("                    " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "    ")

            strBody.Append("        </td>")

            strBody.Append("    </tr>")

            strBody.Append("</table>")


            strBody.Append("<table width='100%' align='center' border='1' style='border-color:#000000; background-color:#000000;'>")

            strBody.Append("    <tr>")

            strBody.Append("        <td align='center' width='300px' style='background-color:#ffffff; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")

            strBody.Append("                    <b> Aircraft Type </b>     ")

            strBody.Append("        </td>")

            strBody.Append("        <td align='center' style='background-color:#ffffff; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")

            strBody.Append("                    <b>" & AircraftType & "</b>     ")

            strBody.Append("        </td>")

            strBody.Append("        <td align='center' style='background-color:#ffffff; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")

            strBody.Append("                    Tail Number:     ")

            strBody.Append("        </td>")

            strBody.Append("        <td align='center' style='background-color:#ffffff; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")

            strBody.Append("                    " & TailNumber & "    ")

            strBody.Append("        </td>")

            strBody.Append("    </tr>")

            strBody.Append("</table>")


            strBody.Append("<table width='100%' align='center' border='1' style='border-color:#000000; background-color:#000000;'>")

            strBody.Append("    <tr>")

            strBody.Append("        <td align='center' width='300px' style='background-color:#ffffff; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")

            strBody.Append("                    Owned/Operated By:     ")

            strBody.Append("        </td>")

            strBody.Append("        <td align='center' style='background-color:#ffffff; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")

            strBody.Append("                    " & OwnedOperatedBy & "    ")

            strBody.Append("        </td>")

            strBody.Append("    </tr>")

            strBody.Append("</table>")



            strBody.Append("<table width='100%' align='center' border='1' style='border-color:#000000; background-color:#000000;'>")

            strBody.Append("    <tr>")

            strBody.Append("        <td align='center' width='300px' style='background-color:#ffffff; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")

            strBody.Append("                    <b>Cause of Incident</b>:     ")

            strBody.Append("        </td>")

            strBody.Append("        <td align='center'width='903px' style='background-color:#ffffff; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")

            strBody.Append("                    <b>" & CauseOfIncident & "</b>     ")

            strBody.Append("        </td>")

            strBody.Append("        <td align='center' style='background-color:#ffffff; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")

            strBody.Append("                    Aircraft Fire:     ")

            strBody.Append("        </td>")

            strBody.Append("        <td align='center' style='background-color:#ffffff; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")

            strBody.Append("                    " & Fire & "    ")

            strBody.Append("        </td>")

            strBody.Append("    </tr>")

            strBody.Append("</table>")



            strBody.Append("<table width='100%' align='center' border='1' style='border-color:#000000; background-color:#000000;'>")

            strBody.Append("    <tr>")

            strBody.Append("        <td align='left' style='background-color:#ffffff; color:#000000; font:Arial; font-size:1.5em; border-color:#000000' >")

            strBody.Append("                    <b>Number of People Onboard:</b> " & NumberPeopleOnboard & "  ")

            strBody.Append("                    &nbsp; // <b>Injuries:</b> ")

            strBody.Append("                    " & Injury & ", ")

            strBody.Append("                    &nbsp; " & InjuryText & "")

            strBody.Append("                    &nbsp; // <b>Fatalities:</b> ")

            strBody.Append("                    " & Fatality & ", ")

            strBody.Append("                    &nbsp; " & FatalityText & "")

            strBody.Append("        </td>")

            strBody.Append("    </tr>")

            strBody.Append("</table>")


            '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            'strBody.Append("<table width='100%' align='center'>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
            'strBody.Append("             ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")

            'strBody.Append("<table width='100%' align='center'>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td align='center'>")
            'strBody.Append("            <b>Aircraft</b>")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")

            'strBody.Append("<table width='100%' align='center'>")
            'strBody.Append("<tr>")
            'strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
            'strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
            'strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
            'strBody.Append("</tr>")
            'strBody.Append("</table>")

            'strBody.Append("<table width='100%' align='center'>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
            'strBody.Append("            <b>Information</b> ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")

            'strBody.Append("<table width='100%' align='center'>")
            'strBody.Append("<tr>")
            'strBody.Append("<td width='25%' align='left'> Select Aircraft Type: " & AircraftType & "</font></td>")
            'strBody.Append("<td width='25%' align='left'> Aircraft Make & Model: " & MakeModel & "</font></td>")
            'strBody.Append("<td width='25%' align='left'> Tail Number: " & TailNumber & "</font></td>")
            'strBody.Append("<td width='25%' align='left'> Owned/Operated By: " & OwnedOperatedBy & "</font></td>")
            'strBody.Append("</tr>")
            'strBody.Append("</table>")

            'strBody.Append("<table width='100%' align='center'>")
            'strBody.Append("<tr>")
            'strBody.Append("<td width='25%' align='left'> Cause of incident (if known)? " & CauseOfIncident & "</font></td>")
            'strBody.Append("<td width='25%' align='left'> # of People Onboard (passengers/crew): " & NumberPeopleOnboard & "</font></td>")
            'strBody.Append("<td width='25%' align='left'> Is there a fire? " & Fire & "</font></td>")
            'strBody.Append("<td width='25%' align='left'> Are there Injuries? " & Injury & "</font></td>")
            'strBody.Append("</tr>")
            'strBody.Append("</table>")

            'strBody.Append("<table width='100%' align='center'>")
            'strBody.Append("<tr>")
            'strBody.Append("<td width='25%' align='left'> Number and Severity of Injuries: " & InjuryText & "</font></td>")
            'strBody.Append("<td width='25%' align='left'> Are there fatalities? " & Fatality & "</font></td>")
            'strBody.Append("<td width='25%' align='left'> Number and location (aircraft or ground): " & FatalityText & "</font></td>")
            'strBody.Append("<td width='25%' align='left'> Are other structures or roadways involved? " & StructuresRoadwaysInvolved & "</font></td>")
            'strBody.Append("</tr>")
            'strBody.Append("</table>")

            'strBody.Append("<table width='100%' align='center'>")
            'strBody.Append("<tr>")
            'strBody.Append("<td width='25%' align='left'> Description: " & StructuresRoadwaysInvolvedText & "</font></td>")
            'strBody.Append("<td width='25%' align='left'> Hazardous materials onboard? " & HazMatOnboard & "</font></td>")
            'strBody.Append("<td width='25%' align='left'> Fuel or Petroleum Spills? " & FuelPetroleumSpills & "</font></td>")
            'strBody.Append("<td width='25%' align='left'> Are there any evacuations? " & Evacuations & "</font></td>")
            'strBody.Append("</tr>")
            'strBody.Append("</table>")

            'strBody.Append("<table width='100%' align='center'>")
            'strBody.Append("<tr>")
            'strBody.Append("<td width='50%' align='left'> What departments/agencies are responding? " & DepartmentAgencyResponding & "</font></td>")
            'strBody.Append("<td width='50%' align='left'> What departments/agencies have been notified? " & DepartmentAgencyNotified & "</font></td>")
            'strBody.Append("</tr>")
            'strBody.Append("</table>")


            'strBody.Append("<table width='100%' align='center'>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
            'strBody.Append("             ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
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
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Animal or Agricultural</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Severity Level: " & SeverityLevel & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Animal Disease" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
            strBody.Append("            <b>Animal Disease</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> What animal(s) are affected? " & AnimalAffected & "</font></td>")
            strBody.Append("<td width='25%' align='left'> What type of disease, if known? " & AnimalDiseaseType & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number of animals infected? " & AnimalInfected & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number of animals deceased? " & AnimalTestExaminations & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Tests or examinations are planned or occuring? " & AnimalsDeceased & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Is there a quarantine in effect? " & AnimalQuarantine & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Describe area, listing streets or landmarks: " & AnimalQuarantineText & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Are any humans affected? " & AnimalHumansAffected & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Number and Severity of Illness: " & AnimalHumansAffectedText & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Are there any human fatalities? " & AnimalHumanFatalities & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Number and Information: " & AnimalHumanFatalitiesText & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


        ElseIf SubType = "Agricultural Disease" Or SubType = "Crop Failure" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
            If SubType = "Agricultural Disease" Then
                strBody.Append("            <b>Agricultural Disease</b> ")
            ElseIf SubType = "Crop Failure" Then
                strBody.Append("            <b>Crop Failure</b> ")
            End If
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> What crop(s) are affected? " & ADCFcropsAffected & "</font></td>")
            strBody.Append("<td width='33%' align='left'> What type of disease, if known? " & ADCFdiseaseType & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Number of acres affected? " & ADCFacresAffected & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")



        ElseIf SubType = "Food Supply Contamination" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
            strBody.Append("            <b>Food Supply Contamination</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> What type / brand of food? " & FSCtypeBrand & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Where was it manufactured/packed? " & FSCmanufacturedPacked & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Affected lot number(s)? " & FSCaffectedLotNumber & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='50%' align='left'> Affected date range? " & FSCaffectedDateRange & "</font></td>")
            strBody.Append("<td width='50%' align='left'> Has a recall been issued? " & FSCrecallIssued & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        End If


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")


    End Sub

    Private Sub GetBombThreatDevice(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)

        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim HowReceivedWhoFound As String = ""
        Dim ExactWordingThreat As String = ""
        Dim Description As String = ""
        Dim Evacuations As String = ""
        Dim MajorRoadwaysClosed As String = ""
        Dim DepartmentAgencyResponding As String = ""
        Dim DepartmentAgencyNotified As String = ""
        Dim Fatality As String = ""
        Dim FatalityText As String = ""
        Dim SearchBeingConducted As String = ""
        Dim DepartmentAgencySearch As String = ""
        Dim Injury As String = ""
        Dim InjuryText As String = ""



        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectBombThreatDeviceByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then

            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            HowReceivedWhoFound = HelpFunction.Convertdbnulls(objDR2("HowReceivedWhoFound"))
            ExactWordingThreat = HelpFunction.Convertdbnulls(objDR2("ExactWordingThreat"))
            Description = HelpFunction.Convertdbnulls(objDR2("Description"))
            Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            MajorRoadwaysClosed = HelpFunction.Convertdbnulls(objDR2("MajorRoadwaysClosed"))
            DepartmentAgencyResponding = HelpFunction.Convertdbnulls(objDR2("DepartmentAgencyResponding"))
            DepartmentAgencyNotified = HelpFunction.Convertdbnulls(objDR2("DepartmentAgencyNotified"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            SearchBeingConducted = HelpFunction.Convertdbnulls(objDR2("SearchBeingConducted"))
            DepartmentAgencySearch = HelpFunction.Convertdbnulls(objDR2("DepartmentAgencySearch"))
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Bomb Threat or Device</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")


        If SubType = "Bomb or Device Explosion" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> How was the threat received/who found the device? " & HowReceivedWhoFound & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Exact wording of threat: " & ExactWordingThreat & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Description of the bomb or device: " & Description & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Are there any evacuations? " & Evacuations & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Are any major roadways closed? " & MajorRoadwaysClosed & "</font></td>")
            strBody.Append("<td width='33%' align='left'> What departments/agencies are responding? " & DepartmentAgencyResponding & "</font></td>")
            strBody.Append("<td width='33%' align='left'> What departments/agencies have been notified? " & DepartmentAgencyNotified & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Are there Injuries? " & Injury & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number and Severity of Injuries: " & InjuryText & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Are there Fatalities? " & Fatality & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number and location: " & FatalityText & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        Else

            If SubType = "Unconfirmed Threat" Or SubType = "Unfounded Threat" Then


                strBody.Append("<table width='100%' align='center'>")
                strBody.Append("<tr>")
                strBody.Append("<td width='25%' align='left'> How was the threat received/who found the device? " & HowReceivedWhoFound & "</font></td>")
                strBody.Append("<td width='25%' align='left'> Exact wording of threat: " & ExactWordingThreat & "</font></td>")
                strBody.Append("<td width='25%' align='left'> Description of the bomb or device: " & Description & "</font></td>")
                strBody.Append("<td width='25%' align='left'> Are there any evacuations? " & Evacuations & "</font></td>")
                strBody.Append("</tr>")
                strBody.Append("</table>")

                strBody.Append("<table width='100%' align='center'>")
                strBody.Append("<tr>")
                strBody.Append("<td width='25%' align='left'> Are any major roadways closed? " & MajorRoadwaysClosed & "</font></td>")
                strBody.Append("<td width='25%' align='left'> What departments/agencies are responding? " & DepartmentAgencyResponding & "</font></td>")
                strBody.Append("<td width='25%' align='left'> What departments/agencies have been notified? " & DepartmentAgencyNotified & "</font></td>")
                strBody.Append("<td width='25%' align='left'> Is a search being conducted? " & SearchBeingConducted & "</font></td>")
                strBody.Append("</tr>")
                strBody.Append("</table>")

                strBody.Append("<table width='100%' align='center'>")
                strBody.Append("<tr>")
                strBody.Append("<td width='100%' align='left'> If Number and location  " & DepartmentAgencySearch & "</font></td>")
                strBody.Append("</tr>")
                strBody.Append("</table>")
            Else

                strBody.Append("<table width='100%' align='center'>")
                strBody.Append("<tr>")
                strBody.Append("<td width='25%' align='left'> How was the threat received/who found the device? " & HowReceivedWhoFound & "</font></td>")
                strBody.Append("<td width='25%' align='left'> Exact wording of threat: " & ExactWordingThreat & "</font></td>")
                strBody.Append("<td width='25%' align='left'> Description of the bomb or device: " & Description & "</font></td>")
                strBody.Append("<td width='25%' align='left'> Are there any evacuations? " & Evacuations & "</font></td>")
                strBody.Append("</tr>")
                strBody.Append("</table>")

                strBody.Append("<table width='100%' align='center'>")
                strBody.Append("<tr>")
                strBody.Append("<td width='33%' align='left'> Are any major roadways closed? " & MajorRoadwaysClosed & "</font></td>")
                strBody.Append("<td width='33%' align='left'> What departments/agencies are responding? " & DepartmentAgencyResponding & "</font></td>")
                strBody.Append("<td width='33%' align='left'> What departments/agencies have been notified? " & DepartmentAgencyNotified & "</font></td>")
                strBody.Append("</tr>")
                strBody.Append("</table>")

            End If

        End If


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

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
        Dim Evacuations As String = ""
        Dim MajorRoadwaysClosed As String = ""
        Dim Injury As String = ""
        Dim InjuryText As String = ""
        Dim Fatality As String = ""
        Dim FatalityText As String = ""
        Dim RegionalAssistanceRequested As String = ""
        Dim RegionalAssistanceRequestedText As String = ""
        Dim AnticipatedAssistance As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectCivilDisturbanceByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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
            Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            MajorRoadwaysClosed = HelpFunction.Convertdbnulls(objDR2("MajorRoadwaysClosed"))
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            Injury = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            RegionalAssistanceRequested = HelpFunction.Convertdbnulls(objDR2("RegionalAssistanceRequested"))
            RegionalAssistanceRequestedText = HelpFunction.Convertdbnulls(objDR2("RegionalAssistanceRequestedText"))
            AnticipatedAssistance = HelpFunction.Convertdbnulls(objDR2("AnticipatedAssistance"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Civil Disturbance</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> What is the cause of the disturbance? " & Cause & "</font></td>")
        strBody.Append("<td width='25%' align='left'> What group(s) or organization(s) are responsible? " & GroupOrgResponsible & "</font></td>")
        strBody.Append("<td width='25%' align='left'> How many people are participating? " & PeopleParticipatingNum & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Is the incident confined to one location? " & ConfinedLocation & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Select Location: " & ConfinedLocationOther & "</font></td>")
        strBody.Append("<td width='25%' align='left'> List Areas: " & LocationAreas & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Law enforcement agency coordinating response: " & ConfinedLocationMemoText & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Departments/Agencies are responding or on scene: " & AgencyCoordinatingResponse & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Are there any evacuations? " & Evacuations & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are any major roadways closed? " & MajorRoadwaysClosed & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there Injuries? " & Injury & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number and Severity of Injuries: " & InjuryText & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Are there Fatalities? " & Fatality & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number and location: " & FatalityText & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Is any local or regional assistance requested? " & RegionalAssistanceRequested & "</font></td>")
        strBody.Append("<td width='25%' align='left'> If Number and location  " & RegionalAssistanceRequestedText & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='100%' align='left'> Is there an anticipated need for state assistance? " & AnticipatedAssistance & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")



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
        Dim Evacuations As String = ""
        Dim MajorRoadwaysClosed As String = ""
        Dim Injury As String = ""
        Dim InjuryText As String = ""
        Dim Fatality As String = ""
        Dim FatalityText As String = ""
        Dim StateAssistance As String = ""



        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectCriminalActivityByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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
            Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            MajorRoadwaysClosed = HelpFunction.Convertdbnulls(objDR2("MajorRoadwaysClosed"))
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            Injury = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            StateAssistance = HelpFunction.Convertdbnulls(objDR2("StateAssistance"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Criminal Activity</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Description the incident: " & IncidentDescription & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Description of the individual(s) responsible: " & IndividualDescription & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Is the incident confined to one location? " & ConfinedLocation & "</font></td>")

        If ConfinedLocation = "No" Then
            strBody.Append("<td width='25%' align='left'> Area(s); specific streets/boundaries preferable: " & ConfinedLocationText & "</font></td>")
        ElseIf ConfinedLocation = "Yes" Then
            strBody.Append("<td width='25%' align='left'> Select Location? " & ConfinedLocationDDL & "</font></td>")
        End If


        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")

        strBody.Append("<td width='25%' align='left'> Law enforcement agency coordinating response: " & AgencyCoordinatingResponse & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Departments/agencies responding or on scene: " & DepartmentAgencyResponding & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Is there a lockdown? " & Lockdown & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Describe lockdown area: " & LockdownText & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")


        strBody.Append("<td width='25%' align='left'> Are there any evacuations? " & Evacuations & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are any major roadways closed? " & MajorRoadwaysClosed & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there Injuries? " & Injury & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number and Severity of Injuries: " & InjuryText & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='left'> Are there Fatalities? " & Fatality & "</font></td>")
        strBody.Append("<td width='33%' align='left'> Number and location: " & FatalityText & "</font></td>")
        strBody.Append("<td width='33%' align='left'> Is there an anticipated need for state assistance? " & StateAssistance & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")



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
        Dim Evacuations As String = ""
        Dim MajorRoadwaysClosed As String = ""
        Dim Injury As String = ""
        Dim InjuryText As String = ""
        Dim Fatality As String = ""
        Dim FatalityText As String = ""
        Dim StateAssistance As String = ""
        Dim StateAssistanceText As String = ""
        Dim AgencyResponse As String = ""
        Dim StagingCommandLocation As String = ""



        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectDamFailureByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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
            Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            MajorRoadwaysClosed = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            StateAssistance = HelpFunction.Convertdbnulls(objDR2("StateAssistance"))
            StateAssistanceText = HelpFunction.Convertdbnulls(objDR2("StateAssistanceText"))
            AgencyResponse = HelpFunction.Convertdbnulls(objDR2("AgencyResponse"))
            StagingCommandLocation = HelpFunction.Convertdbnulls(objDR2("StagingCommandLocation"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Dam Failure</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Dam Name: " & DamName & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Related Waterways/Tributaries: " & RelatedWaterways & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Pool volume/capacity behind the dam: " & PoolVolumeCapacity & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Has a break occurred? " & BreakOccurred & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")

        If BreakOccurred = "Yes" Then
            strBody.Append("<td width='25%' align='left'> Cause of failure: " & CauseOfFailure & "</font></td>")
        ElseIf BreakOccurred = "No" Then
            strBody.Append("<td width='25%' align='left'> Is a break anticipated? " & BreakAnticipated & "</font></td>")
        End If


        strBody.Append("<td width='25%' align='left'> Who is responsible for maintaining the dam? (if known): " & ResponsibleForMaintaining & "</font></td>")
        strBody.Append("<td width='25%' align='left'> What corrective actions are being taken? " & CorrectiveActionsTaken & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Estimated date that repairs will be completed: " & EstimatedRepairDate & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")

        strBody.Append("<td width='25%' align='left'> Estimated time that repairs will be completed: " & Left(localTime, 2) & ":" & Right(localTime, 2) & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Is the incident confined to one location? " & DownstreamPopulationsThreat & "</font></td>")
        strBody.Append("<td width='25%' align='left'> What corrective actions are being taken? " & DownstreamPopulationsThreatText & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there any evacuations? " & Evacuations & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Are any major roadways closed? " & MajorRoadwaysClosed & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there Injuries? " & Injury & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number and Severity of Injuries: " & InjuryText & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there Fatalities? " & Fatality & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Number and location: " & FatalityText & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Anticipated need for state assistance? " & StateAssistance & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Describe the anticipated need(s): " & StateAssistanceText & "</font></td>")
        strBody.Append("<td width='25%' align='left'> What agencies are responding or on scene? " & AgencyResponse & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")

        strBody.Append("<td width='100%' align='left'> Location of Staging Area or Command Post: " & StagingCommandLocation & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")



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


        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectDemINRByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>DEM Incidents/Notifications/Reports</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "SLRC Alarm" Or SubType = "SEOC Alarm" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Alarm Type: " & SlrcSeocAlarmType & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Zone number(s) and/or description(s): " & SlrcSeocZoneNumber & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Alarm Status: " & SlrcSeocAlarmStatus & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "DEP Alarm" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Label / Memo that appears after selection: " & DepWarehouseMemo & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Alarm or Non-Alarm Notification: " & DepWarehouseNotification & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Zone number(s) and/or description(s): " & DepWarehouseZoneNumber & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Alarm Status: " & DepWarehouseAlarmStatus & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Employee name: " & DepWarehouseEmployeeName & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Employee cell phone: " & DepWarehouseEmployeeCellPhone & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Agency and Division: " & DepWarehouseAgencyDivision & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Supervisor name: " & DepWarehouseSupervisorName & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='50%' align='left'> Has supervisor been called? " & DepWarehouseSupervisorCalled & "</font></td>")
            strBody.Append("<td width='50%' align='left'> Access card number: " & DepWarehouseAccessCardNumber & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Medical Emergency" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Building and Room Number: " & MeBuildingRoomNumber & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Has someone called 911? " & Me911Called & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Is the person breathing? " & MePersonBreathing & "</font></td>")
            strBody.Append("<td width='25%' align='left'> What is the person's level of consiousness? " & MeConsiousness & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='100%' align='left'> Describe the person's complaint or symptoms: " & MeComplaintSymptom & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "SEOC Activation" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Activation level: " & SeocActivationLevel & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Related Incident Numbers: " & SeocActivationRelatedIncidentNumbers & "</font></td>")
            strBody.Append("<td width='25%' align='left'> EM Constellation Database: " & SeocActivationEmcDatabase & "</font></td>")
            strBody.Append("<td width='25%' align='left'> EMC Database Name: " & SeocActivationEmcDatabaseName & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "SMT Activation" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Select SMT: " & SmtActivationSMT & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Describe reason for activation: " & SmtActivationReason & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Location to Report: " & SmtActivationReportLocation & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Authorized By: " & SmtActivationAuthorizedBy & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Reservist Activation" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Select SMT: " & ReservistActivationSMT & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Describe reason for activation: " & ReservistActivationReason & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Location to Report: " & ReservistActivationReportLocation & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Authorized By: " & ReservistActivationAuthorizedBy & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "General Notification" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='50%' align='left'> Enter message: " & GeneralNotificationMessage & "</font></td>")
            strBody.Append("<td width='50%' align='left'> Authorized By: " & GeneralNotificationAuthorizedBy & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "IT Disruption or Issue" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Describe the problem; copy error text + link if available: " & ItDisruptionDescription & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Name of program(s)/system(s), if applicable: " & ItDisruptionprogramSystem & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Time the problem started:  " & Left(localTime, 2) & ":" & Right(localTime, 2) & "</font></td>")
            strBody.Append("<td width='25%' align='left'> List any troubleshooting steps taken: " & ItDisruptionStepsTaken & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Communications Disruption or Issue" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Select communication system(s) or circuit(s): " & CommDisruptionSystemCircuitText & "</font></td>")
            strBody.Append("<td width='25%' align='left'> System: " & CommDisruptionSystemCircuit & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Describe the problem: " & CommDisruptionDescription & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Time the problem started: " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='100%' align='left'> List any troubleshooting steps taken: " & CommDisruptionStepsTaken & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Planned Outage" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Describe the system(s) that will be impacted: " & PlannedOutageDescription & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Scheduled start date: " & PlannedOutageScheduledStartDate & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Scheduled start time: " & Left(localTime3, 2) & ":" & Right(localTime3, 2) & "</font></td>")
            strBody.Append("<td width='25%' align='left'> List any troubleshooting steps taken: " & PlannedOutageEstimatedCompletion & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='100%' align='left'> Point of contact name/number: " & PlannedOutagecontactNameNumber & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        End If


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")



    End Sub

    '03-11-11----------------------------------------------------------------
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


        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectDrinkingWaterFacilityByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Drinking Water Facility</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "DWF Report" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Public Water System ID Number: " & PublicWaterSystemIDNumber & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Name of Facility: " & FacilityName & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Was there any trespassing, vandalism, or theft? " & TrespassVandalismTheft & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Describe what occurred: " & TrespassVandalismTheftText & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Any damage to the facility or distibution system? " & DamageFacilityDistibutionSystem & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Was it intentional? " & DFDSintentional & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Was ANY access made to the water supply? " & AccessWaterSupply & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Degredation to water quality, system pressure, or water production? " & Degredation & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Description of the individual(s) responsible: " & IndividualResponsible & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Has local Law Enforcement been contacted? " & LawEnforcementContacted & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Case number, if known: " & IndividualResponsibleCaseNumber & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Boil Water Advisory" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Public Water System ID Number: " & BWpublicWaterSystemIDNumber & "</font></td>")
            strBody.Append("<td width='25%' align='left'> This incident was due to a: " & BWIncidentDueTo & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number of customers affected: " & BWnumberCustomersAffected & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Affected Areas, including streets or boundaries: " & BWaffectedAreas & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        End If

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

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
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Environmental Crime</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='100%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Description of the material(s) involved: " & MaterialDescription & "</font></td>")
        strBody.Append("<td width='25%' align='left'> How long has the crime been occuring? " & CrimeTimeline & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Describe the individual(s) involved: " & IndividalsDescription & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Describe any vehicles(s) invlolved: " & VehiclesDescription & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Has the caller contacted county code enforcement? " & CountyCodeEnforcement & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Code Enforcement Actions: " & CountyCodeEnforcementText & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Do you want an FWC Officer to contact you? " & CallBack & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

    End Sub

    Private Sub GetFire(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)

        Dim SubType As String = ""
        Dim Situation As String = ""
        Dim Evacuations As String = ""
        Dim Injury As String = ""
        Dim InjuryText As String = ""
        Dim Fatality As String = ""
        Dim FatalityText As String = ""
        Dim MajorRoadwaysClosed As String = ""
        Dim Acres As String = ""
        Dim Endangerment As String = ""
        Dim DOFNotified As String = ""
        Dim DOFFireName As String = ""
        Dim DOFFireNumber As String = ""
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
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then

            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))
            Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            MajorRoadwaysClosed = HelpFunction.Convertdbnulls(objDR2("MajorRoadwaysClosed"))
            Acres = HelpFunction.Convertdbnulls(objDR2("Acres"))
            Endangerment = HelpFunction.Convertdbnulls(objDR2("Endangerment"))
            DOFNotified = HelpFunction.Convertdbnulls(objDR2("DOFNotified"))
            DOFFireName = HelpFunction.Convertdbnulls(objDR2("DOFFireName"))
            DOFFireNumber = HelpFunction.Convertdbnulls(objDR2("DOFFireNumber"))
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

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Fire</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Are there any evacuations? " & Evacuations & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there Injuries? " & Injury & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number and Severity of Injuries: " & InjuryText & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there Fatalities? " & Fatality & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='50%' align='left'> Number and location: " & FatalityText & "</font></td>")
        strBody.Append("<td width='50%' align='left'> Are any major roadways closed? " & MajorRoadwaysClosed & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        If SubType = "Wildfire" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> How many acres is the fire? " & Acres & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Are there any endangerments? " & Endangerment & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Has the Department of Forestry been notified? " & DOFNotified & "</font></td>")
            strBody.Append("<td width='25%' align='left'> DOF Fire Name: " & DOFFireName & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> DOF Fire Number: " & DOFFireNumber & "</font></td>")
            strBody.Append("<td width='25%' align='left'> List any other assistance requested:  " & OtherAssistanceRequested & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


        ElseIf SubType = "Other" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Are any other structures threatened? " & StructuresThreatened & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Structures Threatened Text: " & StructuresThreatenedText & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Any hazardous materials inside the structure? " & HazardousMaterials & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Cause of the fire, if known: " & Cause & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='100%' align='left'> Incident Severity: " & IndicentSeverity & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        End If


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

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
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>General</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "General Incident" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Describe the incident: " & GeneralDescription & "</font></td>")
            strBody.Append("<td width='33%' align='left'> What specific hazards exist? " & SpecificHazards & "</font></td>")
            strBody.Append("<td width='33%' align='left'> What remedial actions are planned or occuring? " & RemedialActionsPlannedOccuring & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Local/County EOC Activation" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Level of Activation: " & ActivationLevel & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Incident(s) or hazards(s) caused the activation: " & CauseOfActivation & "</font></td>")
            strBody.Append("<td width='25%' align='left'> EOC Contact Number: " & EOCContactNumber & "</font></td>")
            strBody.Append("<td width='25%' align='left'> EOC Contact E-Mail: " & EOCContactEMail & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='100%' align='left'> Hours operation/operational periods & staffing: " & HoursOperationalPeriodsStaffing & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        End If




        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

    End Sub


    '03-14-11----------------------------------------------------------------

    Private Sub GetGeologicalEvent(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)

        Dim SubType As String = ""
        Dim Situation As String = ""

        Dim SsSize As String = ""
        Dim SsStructuresThreatenedDamaged As String = ""
        Dim SsStructuresThreatenedDamagedText As String = ""
        Dim SsRoadwayThreatDamagedClosed As String = ""
        Dim EaMagnitude As String = ""
        Dim EaLocation As String = ""
        Dim EaDepth As String = ""
        Dim Evacuations As String = ""
        Dim MajorRoadwaysClosed As String = ""
        Dim Injury As String = ""
        Dim InjuryText As String = ""
        Dim Fatality As String = ""
        Dim FatalityText As String = ""
        Dim StateAssistance As String = ""
        Dim StateAssistanceText As String = ""
        Dim AgencyResponding As String = ""
        Dim AgencyNotified As String = ""



        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectGeologicalEventByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then

            SubType = HelpFunction.Convertdbnulls(objDR2("SubType"))
            Situation = HelpFunction.Convertdbnulls(objDR2("Situation"))

            SsSize = HelpFunction.Convertdbnulls(objDR2("SsSize"))
            SsStructuresThreatenedDamaged = HelpFunction.Convertdbnulls(objDR2("SsStructuresThreatenedDamaged"))
            SsStructuresThreatenedDamagedText = HelpFunction.Convertdbnulls(objDR2("SsStructuresThreatenedDamagedText"))
            SsRoadwayThreatDamagedClosed = HelpFunction.Convertdbnulls(objDR2("SsRoadwayThreatDamagedClosed"))
            EaMagnitude = HelpFunction.Convertdbnulls(objDR2("EaMagnitude"))
            EaLocation = HelpFunction.Convertdbnulls(objDR2("EaLocation"))
            EaDepth = HelpFunction.Convertdbnulls(objDR2("EaDepth"))
            Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            MajorRoadwaysClosed = HelpFunction.Convertdbnulls(objDR2("MajorRoadwaysClosed"))
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            Injury = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            StateAssistance = HelpFunction.Convertdbnulls(objDR2("StateAssistance"))
            StateAssistanceText = HelpFunction.Convertdbnulls(objDR2("StateAssistanceText"))
            AgencyResponding = HelpFunction.Convertdbnulls(objDR2("AgencyResponding"))
            AgencyNotified = HelpFunction.Convertdbnulls(objDR2("AgencyNotified"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Geological Event</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Earthquake" Or SubType = "Aftershock" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Magnitude: " & EaMagnitude & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Location: " & EaLocation & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Depth: " & EaDepth & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Subsidence or Sinkhole" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Diameter/Length/Width of the area that subsided? " & SsSize & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Are there any structures threatened or damaged? " & SsStructuresThreatenedDamaged & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Describe: " & SsStructuresThreatenedDamagedText & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Are there any structures threatened or damaged? " & SsRoadwayThreatDamagedClosed & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        End If

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Are there any evacuations? " & Evacuations & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are any major roadways closed? " & MajorRoadwaysClosed & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there Injuries? " & Injury & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number and Severity of Injuries: " & InjuryText & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Are there Fatalities? " & Fatality & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number and location: " & FatalityText & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Anticipated need for state assistance? " & StateAssistance & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Describe the anticipated need(s): " & StateAssistanceText & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='50%' align='left'> What agencies are responding or on scene? " & AgencyResponding & "</font></td>")
        strBody.Append("<td width='50%' align='left'> What agencies have been notified? " & AgencyNotified & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

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
        Dim Injury As String = ""
        Dim InjuryText As String = ""
        Dim Fatality As String = ""
        Dim FatalityText As String = ""
        Dim LaunchLocation As String = ""
        Dim LaunchLocationText As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectKennedySpaceCenterByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            LaunchLocation = HelpFunction.Convertdbnulls(objDR2("LaunchLocation"))
            LaunchLocationText = HelpFunction.Convertdbnulls(objDR2("LaunchLocationText"))
        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Kennedy Space Center / Cape Canaveral AFS</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType <> "Other" Then
            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")

            If LaunchLocation = "Other" Then
                strBody.Append("<td width='50%' align='left'> Launch Location: " & LaunchLocation & "</td>")
                strBody.Append("<td width='50%' align='left'> Launch Location Description: " & LaunchLocationText & "</td>")
            Else
                strBody.Append("<td width='100%' align='left'> Launch Location: " & LaunchLocation & "</td>")
            End If

            strBody.Append("</tr>")
            strBody.Append("</table>")
        End If

        If SubType = "Initial Notification" Or SubType = "Rescheduled Launch" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Mission Name: " & MissionName & "</td>")
            strBody.Append("<td width='25%' align='left'> Mission launch date: " & InrlMissionLaunchDate & "</td>")
            strBody.Append("<td width='25%' align='left'> Launch Window Start: " & Left(localTime, 2) & ":" & Right(localTime, 2) & "</td>")
            strBody.Append("<td width='25%' align='left'> Launch Window End: " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Brevard Co. Fire Rescue Staff report to KSC Morrell Operations Center: " & InrlBrevardCo & "</td>")
            strBody.Append("<td width='33%' align='left'> Brevard Co. EOC Activation to Level 2 no later than: " & InrlBrevardCo2 & "</td>")
            strBody.Append("<td width='33%' align='left'> Next launch notification date: " & NextMissionLaunchDate & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Scrubbed Launch" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Mission scrubbed date: " & ScrubDate & "</td>")
            strBody.Append("<td width='25%' align='left'> Mission scrubbed time: " & Left(localTime3, 2) & ":" & Right(localTime3, 2) & "</td>")
            strBody.Append("<td width='25%' align='left'> Reason: " & ScrubReason & "</td>")
            strBody.Append("<td width='25%' align='left'> Next launch notification date/time: " & ScrubNextLaunchDateTime & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Successful Launch" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='50%' align='left'> Launch date: " & SuccessDate & "</td>")
            strBody.Append("<td width='50%' align='left'> Launch time: " & Left(localTime4, 2) & ":" & Right(localTime4, 2) & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Unsuccessful Launch" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='50%' align='left'> Launch date: " & UnsuccessDate & "</td>")
            strBody.Append("<td width='50%' align='left'> Launch time: " & Left(localTime5, 2) & ":" & Right(localTime5, 2) & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Reason, if known: " & UnsuccessReason & "</td>")
            strBody.Append("<td width='25%' align='left'> Is there any off-site impact? " & UnsuccessOffSiteImpact & "</td>")
            strBody.Append("<td width='25%' align='left'> Describe area and hazards: " & UnsuccessOffSiteImpactText & "</td>")
            strBody.Append("<td width='25%' align='left'> Are there Injuries? " & Injury & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Number and Severity of Injuries: " & InjuryText & "</td>")
            strBody.Append("<td width='33%' align='left'> Are there Fatalities? " & Fatality & "</td>")
            strBody.Append("<td width='33%' align='left'> Number and location: " & FatalityText & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Other" Then


        End If


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

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
        Dim Injury As String = ""
        Dim InjuryText As String = ""
        Dim Fatality As String = ""
        Dim FatalityText As String = ""
        Dim HazardousMaterialsOnboard As String = ""
        Dim FuelPetroleumSpills As String = ""


        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectMarineIncidentByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            Injury = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            HazardousMaterialsOnboard = HelpFunction.Convertdbnulls(objDR2("HazardousMaterialsOnboard"))
            FuelPetroleumSpills = HelpFunction.Convertdbnulls(objDR2("FuelPetroleumSpills"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Marine Incident</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Vessel Name: " & VesselName & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Vessel Type: " & VesselType & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Hull Length: " & HullLength & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Flag: " & Flag & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Registration Number: " & RegistrationNumber & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Owned/Operated By: " & OwnedOperatedBy & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number of People Onboard (passengers/crew): " & NumberPeopleOnboard & "</font></td>")
        strBody.Append("<td width='25%' align='left'> What is the cause the incident (if known)? " & IncidentCause & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Is there a fire? " & Fire & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there Injuries? " & Injury & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number and Severity of Injuries: " & InjuryText & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there Fatalities? " & Fatality & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='left'> Number and location: " & FatalityText & "</font></td>")
        strBody.Append("<td width='33%' align='left'> Are there any hazardous materials onboard?  " & HazardousMaterialsOnboard & "</font></td>")
        strBody.Append("<td width='33%' align='left'> Are there any fuel or Petroleum Spills: " & FuelPetroleumSpills & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

    End Sub

    Private Sub GetMigration(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)



        Dim Migrants As String = ""
        Dim VesselNumber As String = ""
        Dim MigrantNumber As String = ""
        Dim CitizenshipEthnicity As String = ""
        Dim MigrantQuarantined As String = ""
        Dim MigrantQuarantinedText As String = ""
        Dim Injury As String = ""
        Dim InjuryText As String = ""
        Dim Fatality As String = ""
        Dim FatalityText As String = ""
        Dim ImmigrationNotified As String = ""
        Dim Facility As String = ""
        Dim SeverityLevel As String = ""



        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectMigrationByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)

        objDR2 = objCmd2.ExecuteReader

        If objDR2.Read() Then

            Migrants = HelpFunction.Convertdbnulls(objDR2("Migrants"))
            VesselNumber = HelpFunction.Convertdbnulls(objDR2("VesselNumber"))
            MigrantNumber = HelpFunction.Convertdbnulls(objDR2("MigrantNumber"))
            CitizenshipEthnicity = HelpFunction.Convertdbnulls(objDR2("CitizenshipEthnicity"))
            MigrantQuarantined = HelpFunction.Convertdbnulls(objDR2("MigrantQuarantined"))
            MigrantQuarantinedText = HelpFunction.Convertdbnulls(objDR2("MigrantQuarantinedText"))
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            Injury = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            ImmigrationNotified = HelpFunction.Convertdbnulls(objDR2("ImmigrationNotified"))
            Facility = HelpFunction.Convertdbnulls(objDR2("Facility"))
            SeverityLevel = HelpFunction.Convertdbnulls(objDR2("SeverityLevel"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Migration</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='100%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> The migrants: " & Migrants & "</font></td>")
        strBody.Append("<td width='25%' align='left'> How many vessels? " & VesselNumber & "</font></td>")
        strBody.Append("<td width='25%' align='left'> How many migrants? (Men/Women/Children) " & MigrantNumber & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Citizenship or ethnicity of the migrant(s) " & CitizenshipEthnicity & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Have any migrants been quarantined? " & MigrantQuarantined & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Location of Quarantined Migrants: " & MigrantQuarantinedText & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there Injuries? " & Injury & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number and Severity of Injuries: " & InjuryText & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Are there Fatalities? " & Fatality & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number and location: " & FatalityText & "</font></td>")
        strBody.Append("<td width='25%' align='left'> The migrants: " & ImmigrationNotified & "</font></td>")
        strBody.Append("<td width='25%' align='left'> What facility are the migrants being held at? " & Facility & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='100%' align='left'> Select Severity Level: " & SeverityLevel & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

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
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Military Activity</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Tomahawk Missile Launch" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Type of report: " & ReportType & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Launch date: " & LaunchDate & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Launch time: " & Left(localTime, 2) & ":" & Right(localTime, 2) & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Launch message: " & LaunchMessage & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='100%' align='left'> Flight path: " & FlightPath & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        Else

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Unit conducting activity: " & UnitConductingActivity & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Describe the activity: " & ActivityDescription & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Time/date range of activity: " & ActivityTimeDateRange & "</font></td>")
            strBody.Append("<td width='25%' align='left'> List any airspace restrictions: " & AirspaceRestrictions & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> List any road closures: " & RoadClosures & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Point of Contact Name: " & ContactName & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Point of Contact Number: " & ContactNumber & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


        End If


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

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
        'Alabama Start=================================================================
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
        ''Alabama End===================================================================



        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectNPPByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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

            'Alabama Start=================================================================
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
            ''Alabama End===================================================================

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Nuclear Power Plant</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Crystal River – Full ENF" Or SubType = "Saint Lucie" Or SubType = "Turkey Point" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> <b>1.</b></td>")
            strBody.Append("<td align='left'> <b>A.</b></td>")
            strBody.Append("<td align='left'> <b> " & CSTselectOne & "</b></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table cellpadding='0' cellspacing='0' border='3' style='border-color:#000; border-style:solid;'>")
            strBody.Append("<tr>")
            strBody.Append("<td>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> <b>2.</b></td>")
            strBody.Append("<td align='left'> <b>A.</b></td>")
            strBody.Append("<td align='left'> Date: " & CSTdate & "</td>")
            strBody.Append("<td align='left'> <b>B.</b></td>")
            strBody.Append("<td align='left'> Time: " & Left(localTime, 2) & ":" & Right(localTime, 2) & "</td>")
            strBody.Append("<td align='left'> <b>C.</b></td>")
            strBody.Append("<td align='left'> Reported By (Name): " & CSTreportedByName & "</td>")
            strBody.Append("<td align='left'> &nbsp; </td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> D. Messsage Number: " & CSTmessageNumber & "</td>")
            strBody.Append("<td align='left'> E. Reported From: " & CSTreportedFrom & "</td>")
            strBody.Append("<td align='left'> F. " & CSTfSelectOne & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 3. Site: " & CSTsite & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 4. Emergency Classification: " & CSTemergencyClassification & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 5. " & CSTdecTermSelectOne & "</td>")
            strBody.Append("<td align='left'> Date: " & CSTdecTermDate & "</td>")
            strBody.Append("<td align='left'> Time: " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 6. Reason for Emergency Declaration: " & CSTdecTermSelectOne & "</td>")
            strBody.Append("<td align='left'> 6. EAL Number(s): " & CSTeALNumbers & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 6. Description: " & CSTeALDescription & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 7. Additional Information: " & CSTeALai & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 7. Description: " & CSTeALaiDescription & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 8. <b>Weather Data</b> 8. A. Wind direction from degrees:  " & CSTwindDirectionDegrees & "</td>")
            strBody.Append("<td align='left'> 8. B. Downwind Sectors Affected: " & CSTdownwindSectorsAffected & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 9. Release Status:  " & CSTreleaseStatus & "</td>")
            strBody.Append("<td align='left'> 10. Release Significance at Site Boundary: " & CSTsigCatSiteBoundary & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")



            strBody.Append("<table cellpadding='0' cellspacing='0' border='3' style='border-color:#000; border-style:solid;'>")
            strBody.Append("<tr>")
            strBody.Append("<td>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 11. Utillity Recommended Protective Actions: " & CSTutilRecProtAct & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> Evacuate Zones: " & CSTevacuateZones & "</td>")
            strBody.Append("<td align='left'> Shelter Zones: " & CSTshelterZones & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> &nbsp; </td>")
            strBody.Append("<td align='left'> Evacuate Sectors  </td>")
            strBody.Append("<td align='left'> Miles Shelter Sectors  </td>")
            strBody.Append("<td align='left'> Miles No Action Sectors  </td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 0-2 Miles </td>")
            strBody.Append("<td align='left'>" & CST02MilesEvacSect & "</td>")
            strBody.Append("<td align='left'>" & CST25MilesEvacSect & "</td>")
            strBody.Append("<td align='left'>" & CST510MilesEvacSect & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 2-5 Miles </td>")
            strBody.Append("<td align='left'>" & CST02MilesShelterSect & "</td>")
            strBody.Append("<td align='left'>" & CST25MilesShelterSect & "</td>")
            strBody.Append("<td align='left'>" & CST510MilesShelterSect & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 5-10 Miles  </td>")
            strBody.Append("<td align='left'>" & CST02MilesNoActtionSect & "</td>")
            strBody.Append("<td align='left'>" & CST25MilesNoActtionSect & "</td>")
            strBody.Append("<td align='left'>" & CST510MilesNoActtionSect & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


            strBody.Append("<br/>")

            strBody.Append("<table cellpadding='0' cellspacing='0' border='3' style='border-color:#000; border-style:solid;'>")
            strBody.Append("<tr>")
            strBody.Append("<td>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 12. <b>Plant Conditions</b>  12. A. Reactor Shutdown:   " & CST12A & "</td>")
            strBody.Append("<td align='left'> 12. B. Core Adequately Cooled: " & CST12B & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 12. C. Containment Intact: " & CST12C & "</td>")
            strBody.Append("<td align='left'> 12. D. Core Condition: " & CST12D & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 13. <b>Weather Data </b> 13. A. Wind Speed (MPH): " & CST13A & "</td>")
            strBody.Append("<td align='left'> 13. B. Stability Class: " & CST13B & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table cellpadding='0' cellspacing='0' border='3' style='border-color:#000; border-style:solid;'>")
            strBody.Append("<tr>")
            strBody.Append("<td>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 14 A. <b>Additoinal Release Information</b>: " & CST14A & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> Distance </td>")
            strBody.Append("<td align='left'> Projected Thyroid Dose (CDE) for " & CSTProjThyroidDose & " hour(s)</td>")
            strBody.Append("<td align='left'> Projected Total Dose (TEDE) for " & CSTProjTotalDose & " hour(s)</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 1 Mile (Site Boundary) </td>")
            strBody.Append("<td align='left'> B. " & CST14B & " mrem</td>")
            strBody.Append("<td align='left'> C. " & CST14C & " mrem</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 2 Miles </td>")
            strBody.Append("<td align='left'> D. " & CST14D & " mrem</td>")
            strBody.Append("<td align='left'> E. " & CST14E & " mrem</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 5 Miles </td>")
            strBody.Append("<td align='left'> F. " & CST14F & " mrem</td>")
            strBody.Append("<td align='left'> G. " & CST14G & " mrem</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 10 Miles </td>")
            strBody.Append("<td align='left'> H. " & CST14H & " mrem</td>")
            strBody.Append("<td align='left'> I. " & CST14I & " mrem</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 15. <b>Message Received By</b>: (Name): " & CST15Name & "</td>")
            strBody.Append("<td align='left'> Date: " & CST15Date & "</td>")
            strBody.Append("<td align='left'> Time: " & Left(localTime3, 2) & ":" & Right(localTime3, 2) & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> SWO User Comments: " & CSTuserComments & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


        ElseIf SubType = "Farley" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 1. Select one: " & Far1SelectOne & "</td>")
            strBody.Append("<td align='left'> Message #: " & Far1MessageNumber & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 2. Select one: " & Far2SelectOne & "</td>")
            strBody.Append("<td align='left'> Notification Time: " & Left(localTime4, 2) & ":" & Right(localTime4, 2) & "</td>")
            strBody.Append("<td align='left'> Date: " & Far2NotificationDate & "</td>")
            strBody.Append("<td align='left'> Authentication #: " & Far2AuthenticationNumber & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> Site: " & Far3Site & "</td>")
            strBody.Append("<td align='left'> Confirmation Phone #: " & Far3ConfirmationPhoneNumber & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table cellpadding='0' cellspacing='0' border='3' style='border-color:#000; border-style:solid;'>")
            strBody.Append("<tr>")
            strBody.Append("<td>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 4. <b>Emergency Classification:</b> " & Far4EmergencyClassification & "</td>")
            strBody.Append("<td align='left'> Based on EAL #: " & Far4BasedEALnumber & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> EAL Description: " & Far4EALdescription & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            '------------------------------------------------------------------------
            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")

            If Far5a = True Then
                strBody.Append("<td align='left'> 5. <b>Protective Action Recommendations</b>: <input type='checkbox' name='1' checked='checked' /> 5 A. None </td>")
            ElseIf Far5a = False Then
                strBody.Append("<td align='left'> 5. <b>Protective Action Recommendations</b>: <input type='checkbox' name='1' /> 5 A. None </td>")
            End If

            strBody.Append("</tr>")
            strBody.Append("</table>")

            '------------------------------------------------------------------------

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")

            If Far5b = True Then
                strBody.Append("<td align='left'> <input type='checkbox' name='2' checked='checked' /> 5. B. Evacuate </td>")
            ElseIf Far5b = False Then
                strBody.Append("<td align='left'> <input type='checkbox' name='2' /> 5. B. Evacuate </td>")
            End If

            strBody.Append("<td align='left'> 5. B. Evacuate Description: " & Far5bText & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            '------------------------------------------------------------------------

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")

            If Far5c = True Then
                strBody.Append("<td align='left'> <input type='checkbox' name='3' checked='checked' /> 5. B. Evacuate </td>")
            ElseIf Far5c = False Then
                strBody.Append("<td align='left'> <input type='checkbox' name='3' /> 5. C. Shelter </td>")
            End If

            strBody.Append("<td align='left'> 5. C. Shelter Description: " & Far5cText & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            '------------------------------------------------------------------------

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")

            If Far5d = True Then
                strBody.Append("<td align='left'> <input type='checkbox' name='4' checked='checked' /> 5. D. Consider the use of KI in accordance with state plans and policy. </td>")
            ElseIf Far5d = False Then
                strBody.Append("<td align='left'> <input type='checkbox' name='4' /> 5. D. Consider the use of KI in accordance with state plans and policy. </td>")
            End If

            strBody.Append("</tr>")
            strBody.Append("</table>")

            '------------------------------------------------------------------------

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")

            If Far5e = True Then
                strBody.Append("<td align='left'> <input type='checkbox' name='5' checked='checked' /> 5. E. Other </td>")
            ElseIf Far5e = False Then
                strBody.Append("<td align='left'> <input type='checkbox' name='5' /> 5. E. Other </td>")
            End If

            strBody.Append("<td align='left'> 5. E. Other Description: " & Far5eText & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            '------------------------------------------------------------------------

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 6. <b>Emergency Release</b>: " & Far6EmergencyRelease & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


            strBody.Append("</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 7. Release Significance: " & Far7ReleaseSignificance & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 8. Event Prognosis: " & Far8EventPrognosis & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 9. Meterological Data: </td>")
            strBody.Append("<td align='left'> Wind direction from " & Far9WindDirectDegrees & " degrees: </td>")
            strBody.Append("<td align='left'> Wind Speed " & Far9WindSpeed & " (mph) </td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> &nbsp; </td>")
            strBody.Append("<td align='left'> Precipitation: " & Far9Precipitation & " </td>")
            strBody.Append("<td align='left'> Stability Class: " & Far9StabilityClass & " </td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 10. [Select One]: " & Far10Select1 & "</td>")
            strBody.Append("<td align='left'> 10 Time: " & Left(localTime5, 2) & ":" & Right(localTime5, 2) & "</td>")
            strBody.Append("<td align='left'> 10 Date: " & Far10Date & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 11. Affected Units: " & Far11AffectedUnits & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 12. Unit Status: </td>")
            strBody.Append("<td align='left'> 12. A. Unit 1 " & Far12AUnitPower & " % power</td>")
            strBody.Append("<td align='left'> Time: " & Left(localTime6, 2) & ":" & Right(localTime6, 2) & "</td>")
            strBody.Append("<td align='left'> Date: " & Far12ADate & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> (Unaffected Unit(s) Status Not Required for Initial Notifications)  </td>")
            strBody.Append("<td align='left'> 12. B. Unit 2 " & Far12BUnitPower & " % power</td>")
            strBody.Append("<td align='left'> Time: " & Left(localTime7, 2) & ":" & Right(localTime7, 2) & "</td>")
            strBody.Append("<td align='left'> Date: " & Far12BDate & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 13. Remarks: " & Far13Remarks & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("    <tr>")
            strBody.Append("        <td style='background: #000 repeat; height: 1px;' ")
            strBody.Append("             ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='center'> <font size='4'><b>Information(Lines 14-16 not required for initial Notifications)</b></font> </td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='center'> <font size='3'><b>Emergency Release Data. Not required if line 6 A is selected.</b></font> </td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 14. Release Characterization: " & Far14ReleaseChar & "</td>")
            strBody.Append("<td align='left'> Units: " & Far14Units & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> Magnitude:  Noble Gasses: " & Far14NobleGasses & "</td>")
            strBody.Append("<td align='left'> Iodines: " & Far14Iodines & "</td>")
            strBody.Append("<td align='left'> Particulautes: " & Far14Particulautes & "</td>")
            strBody.Append("<td align='left'> Other: " & Far14Other & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")

            If Far14Aairborne = True Then
                strBody.Append("<td align='left'>Form: <input type='checkbox' name='6' checked='checked' /> A. Airborne: </td>")
            ElseIf Far14Aairborne = False Then
                strBody.Append("<td align='left'>Form: <input type='checkbox' name='6' /> A. Airborne: </td>")
            End If

            strBody.Append("<td align='left'> Start Time: " & Left(localTime8, 2) & ":" & Right(localTime8, 2) & "</td>")
            strBody.Append("<td align='left'> Date: " & Far14AstartDate & "</td>")
            strBody.Append("<td align='left'> Stop Time: " & Left(localTime9, 2) & ":" & Right(localTime9, 2) & "</td>")
            strBody.Append("<td align='left'> Date: " & Far14AstopDate & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")

            If Far14Bliquid = True Then
                strBody.Append("<td align='left'> <input type='checkbox' name='7' checked='checked' /> B. Liquid: </td>")
            ElseIf Far14Bliquid = False Then
                strBody.Append("<td align='left'> <input type='checkbox' name='7' /> B. Liquid: </td>")
            End If

            strBody.Append("<td align='left'> Start Time: " & Left(localTime10, 2) & ":" & Right(localTime10, 2) & "</td>")
            strBody.Append("<td align='left'> Date: " & Far14BstartDate & "</td>")
            strBody.Append("<td align='left'> Stop Time: " & Left(localTime11, 2) & ":" & Right(localTime11, 2) & "</td>")
            strBody.Append("<td align='left'> Date: " & Far14BendDate & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 15. Projection Parameters: </td>")
            strBody.Append("<td align='left'> Projection Period: " & Far15ProjectionPeriod & " (hours) </td>")
            strBody.Append("<td align='left'> Estimated Release Duration: " & Far15EstimatedReleaseDuration & " (hours) </td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> <font size='2'>Projection Performed:</font> </td>")
            strBody.Append("<td align='left'> Time: " & Left(localTime12, 2) & ":" & Right(localTime12, 2) & "</td>")
            strBody.Append("<td align='left'> Date: " & Far15ProjectionPerformedDate & "</td>")
            strBody.Append("<td align='left'> Accident Type: " & Far15AccidentType & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 16. Projected Dose: </td>")
            strBody.Append("<td align='left'> <u>Distance</u> </td>")
            strBody.Append("<td align='left'> <u>TEDE(mrem)</u> </td>")
            strBody.Append("<td align='left'> <u>Adult Thyroid CDE(mrem)</u> </td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> &nbsp; </td>")
            strBody.Append("<td align='left'> Site boundary </td>")
            strBody.Append("<td align='left'> " & Far16SiteBoundaryTEDE & "</td>")
            strBody.Append("<td align='left'> " & Far16SiteBoundaryAdultThyroidCDE & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> &nbsp; </td>")
            strBody.Append("<td align='left'> 2 Miles </td>")
            strBody.Append("<td align='left'> " & Far16TwoMilesTEDE & "</td>")
            strBody.Append("<td align='left'> " & Far16TwoMilesAdultThyroidCDE & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> &nbsp; </td>")
            strBody.Append("<td align='left'> 5 Miles </td>")
            strBody.Append("<td align='left'> " & Far16FiveMilesTEDE & "</td>")
            strBody.Append("<td align='left'> " & Far16FiveMilesAdultThyroidCDE & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> &nbsp; </td>")
            strBody.Append("<td align='left'> 10 Miles </td>")
            strBody.Append("<td align='left'> " & Far16TenMilesTEDE & "</td>")
            strBody.Append("<td align='left'> " & Far16MilesAdultThyroidCDE & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> 17. Approved By: " & Far17ApprovedBy & "</td>")
            strBody.Append("<td align='left'> Title: " & Far17Title & "</td>")
            strBody.Append("<td align='left'> Time: " & Left(localTime13, 2) & ":" & Right(localTime13, 2) & "</td>")
            strBody.Append("<td align='left'> Date: " & Far17Date & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> Notified By: " & Far17NotifiedBy & "</td>")
            strBody.Append("<td align='left'>  ")
            strBody.Append("<table cellpadding='0' cellspacing='0' border='3' style='border-color:#000; border-style:solid;'>")
            strBody.Append("<tr>")
            strBody.Append("<td>")
            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='left'> Received By: " & Far17ReceivedBy & "</td>")
            strBody.Append("<td align='left'> Time: " & Left(localTime14, 2) & ":" & Right(localTime14, 2) & "</td>")
            strBody.Append("<td align='left'> Date: " & Far17ReceivedDate & "</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")
            strBody.Append("</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")
            strBody.Append("</td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td align='center'> <font size='2'>(To be completed by receiving organization)</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        End If

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

    End Sub

    'Ended 03-16-11----------------------------------------------------------------


    '03-17-11----------------------------------------------------------------------
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
        Dim CallbackDEPRequested As String = ""
        Dim CallbackDEPRequestedValue As String = ""


        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectPetroleumSpillByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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
            CallbackDEPRequested = HelpFunction.Convertdbnulls(objDR2("CallbackDEPRequested"))
            CallbackDEPRequestedValue = HelpFunction.Convertdbnulls(objDR2("CallbackDEPRequestedValue"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Petroleum Spill</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Petroleum Type: " & PetroleumType & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Name or Description: " & PetroleumNameDescription & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Odor: " & PetroleumOdor & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Color: " & PetroleumColor & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Source / Container: " & PetroleumSourceContainer & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Diameter of the Pipeline: " & DiameterPipeline & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Unbroken end of the pipe connected to: " & UnbrokenEndPipeConnectedTo & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Total source/container volume: " & TotalSourceContainerVolume & "</font></td>")

        strBody.Append("</tr>")
        strBody.Append("</table>")


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Quantity released: " & PetroleumQuantityReleased & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Rate of release: " & PetroleumRateOfRelease & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Released: " & PetroleumlReleased & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Cause of release: " & PetroleumCauseOfRelease & "</font></td>")

        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Time the release was discovered: " & Left(localTime, 2) & ":" & Right(localTime, 2) & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Time the release was secured: " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Were any storm drains affected?: " & StormDrainsAffected & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Were any waterways affected? " & WaterwaysAffected & "</font></td>")

        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Name(s) of waterways: " & WaterwaysAffectedText & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are any major roadways closed? " & MajorRoadwaysClosed & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Have any cleanup actions been taken? " & CleanupActionsTaken & "</font></td>")
        strBody.Append("<td width='25%' align='left'> List cleanup actions: " & CleanupActionsTakenText & "</font></td>")

        strBody.Append("</tr>")
        strBody.Append("</table>")
        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='left'> Who is conducting cleanup? " & ConductingCleanup & "</font></td>")
        strBody.Append("<td width='33%' align='left'> Is a callback from DEP requested? " & CallbackDEPRequested & "</font></td>")
        strBody.Append("<td width='33%' align='left'>  Select Contact: " & CallbackDEPRequestedValue & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

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
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Population Protection Actions</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Situation: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Shelter in place" Or SubType = "Evacuation Order" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Impacted area, including streets or landmarks: " & ImpactedStreetLandmark & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Department/Agency issuing the order: " & DeptAgencyIssuingOrder & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Duration of the order(if known): " & Duration & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number of residences impacted (if known): " & ImpactResidenceNum & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='50%' align='left'> Number of businesses impacted (if known): " & ImpactBusinessNum & "</font></td>")
            strBody.Append("<td width='50%' align='left'> Total number of individuals impacted (if known): " & TotalImpacted & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        Else

            'Response.Write("Hello")
            'Response.End()

            objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            DBConStringHelper.PrepareConnection(objConn2) 'open the connection
            objCmd2 = New SqlCommand("spSelectShelterByIncidentID", objConn2)
            objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd2.CommandType = CommandType.StoredProcedure
            objDR2 = objCmd2.ExecuteReader()

            If objDR2.Read() Then

                'there are records
                objDR2.Close()
                objDR2 = objCmd2.ExecuteReader()

                strBody.Append("<table width='100%' align='center'>")
                strBody.Append("    <tr>")
                strBody.Append("<td align='center'>")
                strBody.Append("<u><b>Shelters Open</b></u>")
                strBody.Append("</td>")
                strBody.Append("    </tr>")


                While objDR2.Read
                    strBody.Append("    <tr>")
                    strBody.Append("<td align='center'>")
                    strBody.Append("            Incident Type: " & objDR2.Item("ShelterName") & "")
                    strBody.Append("</td>")
                    strBody.Append("    </tr>")
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


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")


    End Sub


    '-03-18-11
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
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Public Health Medical</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Situation: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Infectious Disease Report" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> What type of disease, if known? " & IDRdiseaseType & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number of people infected? " & IDRpeopleInfectedNumber & "</font></td>")
            strBody.Append("<td width='25%' align='left'> What tests or examinations are planned or occuring? " & IDRexamTest & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Is there a quarantine in effect? " & IDRquarantineEffect & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Describe area, listing streets or landmarks: " & IDRquarantineEffectText & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Are there Fatalities? " & IDRfatality & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number and location: " & IDRfatalityText & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Is a callback from DOH Requested? " & IDRdOHrequested & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='100%' align='left'> Contact: " & IDRdOHrequestedText & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


        ElseIf SubType = "Public Health Hazard" Or SubType = "Other" Then


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Describe the hazard: " & PHHOhazardDescription & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Is a callback from DOH Requested? " & PHHOdOHRequested & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Contact: " & PHHOdOHRequestedText & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")



        ElseIf SubType = "Mass Casualty Incident" Then


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Number of Patients: " & MCIpatientNumber & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Critical: " & MCIcritical & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Immediate: " & MCIimmediate & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Delayed: " & MCIdelayed & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Deceased: " & MCIdeceased & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Location of Triage/Treatment Area(s): " & MCItTA & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Which agency is coordinating the MCI? " & MCIagencyCoordinating & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Are there any unmet needs? " & MCIunmetNeeds & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Describe the needs: " & MCIunmetNeedsText & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Is a callback from DOH Requested? " & MCIdOHRequested & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Contact: " & MCIdOHRequestedText & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Impact to Healthcare Facility" Then


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Number of Patients Affected: " & IHFpatientsAffectedNumber & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Is the facility damaged? " & IHFfacilityDamaged & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Describe the Damage: " & IHFfacilityDamagedText & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Is the facility being evacuated? " & IHFfacilityEvacuated & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Where are the evacuees being taken: " & IHFfacilityEvacuatedText & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Are there any unmet needs? " & IHFunmetNeeds & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Unmet needs: " & IHFunmetNeedsText & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Is a callback from DOH Requested? " & IHFcallbackRequested & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='100%' align='left'> Contact: " & IHFcallbackRequestedText & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        End If


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")


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
        Dim Injury As String = ""
        Dim InjuryText As String = ""
        Dim Fatality As String = ""
        Dim FatalityText As String = ""
        Dim HazMat As String = ""
        Dim HazMatReleased As String = ""
        Dim FuelPetroleumSpills As String = ""

        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectRailByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            Injury = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            HazMat = HelpFunction.Convertdbnulls(objDR2("HazMat"))
            HazMatReleased = HelpFunction.Convertdbnulls(objDR2("HazMatReleased"))
            FuelPetroleumSpills = HelpFunction.Convertdbnulls(objDR2("FuelPetroleumSpills"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Rail Incident</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> This situation is: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Train Type: " & TrainType & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Name of company operating train: " & CompanyOperatingTrain & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Train number: " & TrainNumber & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Rail line: " & RailLiine & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Mile post: " & MilePost & "</font></td>")
        strBody.Append("<td width='25%' align='left'> DOT crossing number (if applicable): " & DotCrossingNumber & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Line owned or operated by: " & LineOwnedOperatedBy & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number of people onboard (passengers/crew): " & PeopleOnBoard & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> What is the cause the incident (if known)? " & IncidentCause & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Is there a derailment? " & Derailment & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there Injuries? " & Injury & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number and Severity of Injuries: " & InjuryText & "</font></td>")

        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='25%' align='left'> Are there Fatalities? " & Fatality & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Number and location: " & FatalityText & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Are there any hazardous materials onboard? " & HazMat & "</font></td>")
        strBody.Append("<td width='25%' align='left'> Were any hazardous materials released? " & HazMatReleased & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='100%' align='left'> Are there any fuel or Petroleum Spills? " & FuelPetroleumSpills & "</font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

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
        Dim Injury As String = ""
        Dim InjuryText As String = ""
        Dim Fatality As String = ""
        Dim FatalityText As String = ""
        Dim UnmetNeeds As String = ""
        Dim UnmetNeedsText As String = ""
        Dim CoordinatingRescueEffort As String = ""
        Dim DescriptionIndividual As String = ""
        Dim LastSeen As String = ""
        Dim DescriptionVehicleRelevantInformation As String = ""
        Dim AgencyHandlingInvestigation As String = ""


        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectSearchRescueByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            Injury = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            UnmetNeeds = HelpFunction.Convertdbnulls(objDR2("UnmetNeeds"))
            UnmetNeedsText = HelpFunction.Convertdbnulls(objDR2("UnmetNeedsText"))
            CoordinatingRescueEffort = HelpFunction.Convertdbnulls(objDR2("CoordinatingRescueEffort"))
            DescriptionIndividual = HelpFunction.Convertdbnulls(objDR2("DescriptionIndividual"))
            LastSeen = HelpFunction.Convertdbnulls(objDR2("LastSeen"))
            DescriptionVehicleRelevantInformation = HelpFunction.Convertdbnulls(objDR2("DescriptionVehicleRelevantInformation"))
            AgencyHandlingInvestigation = HelpFunction.Convertdbnulls(objDR2("AgencyHandlingInvestigation"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Search & Rescue</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Situation: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "ELT" Or SubType = "EPIRB" Or SubType = "PLB" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Date mission opened: " & SearchRescueDate & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Time mission opened: " & Left(localTime, 2) & ":" & Right(localTime, 2) & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Mission number: " & MissionNumber & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Last coordinates or area description: " & CoordinateAreaDescription & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Registration information: " & RegistrationInformation & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Is CAP responding? " & CAPResponding & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Any missing or overdue aircraft in the area?  " & MissingOverdueAircraft & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Date mission closed: " & MissionClosedDate & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='50%' align='left'> Time mission closed: " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & "</font></td>")
            strBody.Append("<td width='50%' align='left'> Disposition: " & Disposition & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


        ElseIf SubType = "Structure Collapse" Or SubType = "Industrial Accident" Or SubType = "Transportation Accident" Or SubType = "Other" Then


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Describe the affected struture(s) or facilities(s): " & AffectedStrutureFacility & "</font></td>")
            strBody.Append("<td width='25%' align='left'> What caused the collapse (if known)? " & CausedCollapse & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number of people trapped: " & NumberPeopleTrapped & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Are there Injuries? " & Injury & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Number and Severity of Injuries: " & InjuryText & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Are there Fatalities? " & Fatality & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number and location: " & FatalityText & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Any unmet needs for the rescue operation? " & UnmetNeeds & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='50%' align='left'> Describe Needs: " & UnmetNeedsText & "</font></td>")
            strBody.Append("<td width='50%' align='left'> Department/agency coordinating rescue efforts? " & CoordinatingRescueEffort & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


        ElseIf SubType = "Food Supply Contamination" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Description of the individual(s): " & DescriptionIndividual & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Area the individual(s) were last seen in: " & LastSeen & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Vehicle Description/other relevant information: " & DescriptionVehicleRelevantInformation & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Agency handling the investigation: " & AgencyHandlingInvestigation & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        End If


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")


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
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Security Threat</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Situation: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType <> "Lockdown" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Description the incident or threat: " & Description & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Description of the individual(s) responsible: " & IndividualResponsibleDescription & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Is the incident confined to one location? " & Location & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Select Location: " & ConfinedLocation & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='50%' align='left'> Area(s); specific streets/boundaries preferable: " & ListAreas & "</font></td>")
            strBody.Append("<td width='50%' align='left'> Select incident severity: " & IncidentSeverity & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        End If


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")


    End Sub


    '03-22-11
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
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Utility Disruption or Emergency</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Situation: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Telecommunications Outage" Then


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Communications System: " & TOcommunicationsSystem & "</font></td>")
            strBody.Append("<td width='25%' align='left'> System operated by: " & TOsystemOperated & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number of Customers affected: " & TOcustomersAffectedNumber & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Is 911 telephone service affected? " & TO911Affected & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Describe: " & TO911AffectedText & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Any damage to the facility or distibution system? " & TOdamageFacilityDistibutionSystem & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Was it intentional? " & TOdamageFacilityDistibutionSystemIntentional & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Description of the individual(s) responsible: " & TOdamageFacilityDistibutionSystemText & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


        ElseIf SubType = "Drinking Water Outage" Then


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Water System Name: " & DWOWaterSystemName & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Public water system ID #: " & DWOpublicWaterSystemID & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number of customers affected: " & DWOnumberCustomersAffected & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Is the outage a result of any trespassing, theft, vandalism, or a security breach to the distribution system or its facilities? " & DWOoutageResultTTVSBDSF & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='50%' align='left'> Estimated date/time of restoration: " & DWOEstimatedDateTimeRestoration & "</font></td>")
            strBody.Append("<td width='50%' align='left'> Was a boil water advisory issued? " & DWOboilAdvisory & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


        ElseIf SubType = "Electric Outage" Then



            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Electric System: " & EOelectricSystem & "</font></td>")
            strBody.Append("<td width='25%' align='left'> System operated by: " & EOsystemOperatedBy & "</font></td>")
            strBody.Append("<td width='25%' align='left'> What caused the outage? " & EOwhatCausedOutage & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number of Customers affected: " & EONumberCustomersAffected & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Estimated time to 98% or greater restoration: " & EOestimatedGreaterRestoration & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Any damage to the facility or distibution system? " & EOdamageFacilityDistibutionSystem & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Was it intentional? " & EOdamageFacilityDistibutionSystemIntentional & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Description of the individual(s) responsible: " & EOdamageFacilityDistibutionSystemResposible & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='33%' align='left'> Type of Advisory " & GCAadvisoryType & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Advisory due to a fuel supply shortage? " & GCAsupplyShortage & "</font></td>")
            strBody.Append("<td width='33%' align='left'> Text of the Advisory: " & GCAadvisory & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


        ElseIf SubType = "Natural Gas Outage" Then


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Natural Gas System: " & NGOsystem & "</font></td>")
            strBody.Append("<td width='25%' align='left'> System operated by: " & NGOsystemOperatedBy & "</font></td>")
            strBody.Append("<td width='25%' align='left'> What caused the outage?  " & NGOoutageCause & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number of Customers affected: " & NGOCustomersAffectedNumber & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='50%' align='left'> Estimated time restoration: " & NGOestimatedTimeRestoration & "</font></td>")
            strBody.Append("<td width='50%' align='left'> Any damage to the facility or distibution system? " & NGOdFDS & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


        ElseIf SubType = "Electric Generating Capacity Advisory" Then


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='50%' align='left'> Was it intentional? " & NGOdFDSintentional & "</font></td>")
            strBody.Append("<td width='50%' align='left'> Description of the individual(s) responsible: " & NGOdFDSdescription & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


        End If


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")


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


        objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn2.Open()
        objCmd2 = New SqlCommand("spSelectWastewaterByIncidentID", objConn2)
        objCmd2.CommandType = CommandType.StoredProcedure
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Wastewater or Effluent</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Situation: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Wastewater" Then

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Public Water System ID or Permit Number: " & WWsystemIDPermitNumber & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Name of System: " & WWsystemName & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Type of System:  " & WWsystemType & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Type of wastewater:   " & WWreleaseOccurred & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Release occurred from a: " & WWtype & "</font></td>")
            strBody.Append("<td width='25%' align='left'> What caused the release? " & WWreleaseCause & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Release status:  " & WWreleaseStatus & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Date release ceased: " & WWceasedDate & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")

            strBody.Append("<td width='25%' align='left'> Time release ceased: " & Left(localTime, 2) & ":" & Right(localTime, 2) & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Was the release contained on-site at a water reclamation facility? " & WWreleasedContainedonSite & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Amount of release, in gallons: " & WWreleaseAmount & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Did the release enter a storm water system? " & WWstormWater & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")

            strBody.Append("<td width='25%' align='left'> Location of storm drain(s) that were impacted: " & WWstormWaterLocation & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Where does the storm drain discharge? " & WWstormWaterDischarge & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Did the release enter any surface waters? " & WWsurfaceWaterDDL & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Type of surface water: " & WWsurfaceWater & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")

            strBody.Append("<td width='25%' align='left'> Names of waterway(s): " & WWwaterway & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Any threat or confirmed contamination of drinking water? " & WWconfirmedContamination & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Status of Cleanup Actions: " & WWcleanupActions & "</font></td>")
            strBody.Append("<td width='100%' align='left'> Describe clean-up actions: " & WWcleanupActionsText & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")

            strBody.Append("</tr>")
            strBody.Append("</table>")




        ElseIf SubType = "Treated Effluent" Then


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Public Water System ID or Permit Number: " & TEsystemIDPermitNumber & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Name of System: " & TEsystemName & "</font></td>")
            strBody.Append("<td width='25%' align='left'> What caused the release? " & TEreleaseCause & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Amount of release, in gallons: " & TEgallonsReleased & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='50%' align='left'> Are any cleanup actions needed? " & TEcleanupActions & "</font></td>")
            strBody.Append("<td width='50%' align='left'> Describe cleanup actions: " & TEcleanupActionsText & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


        End If


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")


    End Sub

    Private Sub GetWeather(ByVal strIncidentID As String, ByVal strIncidentIncidentTypeID As String)


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
        Dim LSRreportType As String = ""
        Dim LSRreportReceived As String = ""
        Dim LSRInjury As String = ""
        Dim LSRInjuryText As String = ""
        Dim LSRFatality As String = ""
        Dim LSRFatalityText As String = ""
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
        objCmd2.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
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
            LSRreportType = HelpFunction.Convertdbnulls(objDR2("LSRreportType"))
            LSRreportReceived = HelpFunction.Convertdbnulls(objDR2("LSRreportReceived"))
            LSRInjury = HelpFunction.Convertdbnulls(objDR2("LSRInjury"))
            LSRInjuryText = HelpFunction.Convertdbnulls(objDR2("LSRInjuryText"))
            LSRFatality = HelpFunction.Convertdbnulls(objDR2("LSRFatality"))
            LSRFatalityText = HelpFunction.Convertdbnulls(objDR2("LSRFatalityText"))
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

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='center'>")
        strBody.Append("            <b>Weather Advisories and Reports</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("<tr>")
        strBody.Append("<td width='33%' align='center'> Sub-Type: " & SubType & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Situation: " & Situation & " </font></td>")
        strBody.Append("<td width='33%' align='center'> Description: " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & " </font></td>")
        strBody.Append("</tr>")
        strBody.Append("</table>")

        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #d4d4d4 repeat;'> ")
        strBody.Append("            <b>Information</b> ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Weather Watch" Or SubType = "Weather Warning" Or SubType = "Weather Advisory" Then


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Date Issued: " & WWAdateIssued & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Time Issued: " & Left(localTime, 2) & ":" & Right(localTime, 2) & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Effective on Date: " & WWAeffectiveDate & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Effective on Time: " & WWAeffectiveTime & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Expires on Date: " & WWAexpiresDate & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Expires on Time: " & WWAexpiresTime & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Issuing Office: " & WWAissuingOffice & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Type of Advisory: " & WWAadvisoryType & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='100%' align='left'> Advisory Text: " & WWAadvisoryText & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


        ElseIf SubType = "Local Storm Report" Then


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Type of Report: " & LSRreportType & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Report was received: " & LSRreportReceived & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Are there Injuries? " & LSRInjury & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number and Severity of Injuries: " & LSRInjuryText & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Are there Fatalities? " & LSRFatality & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number and location: " & LSRFatalityText & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Are there any displacements? " & LSRdisplacement & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Number and where are they being sheltered: " & LSRdisplacementText & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Is there any damage to structures? " & LSRdamageStructures & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Type of Structures / Number / Severity: " & LSRdamageStructuresText & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Is there any damage to Infrastructure? " & LSRinfrastructureDamage & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Describe: " & LSRinfrastructureDamageText & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")


        ElseIf SubType = "NOAA Transnsmitter Outage" Then


            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='25%' align='left'> Transmitter(s): " & TOtransmitter & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Weather Forecast Office making notification: " & TOmakingNotification & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Date Out of Service: " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & "</font></td>")
            strBody.Append("<td width='25%' align='left'> Time Out of Service: " & TOserviceOutDate & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

            strBody.Append("<table width='100%' align='center'>")
            strBody.Append("<tr>")
            strBody.Append("<td width='50%' align='left'> Transmitter is out of service due to: " & TOtransmitterServiceDueTo & "</font></td>")
            strBody.Append("<td width='50%' align='left'> Time the transmitter(s) are expected to return to service: " & TOreturnToService & "</font></td>")
            strBody.Append("</tr>")
            strBody.Append("</table>")

        End If


        strBody.Append("<table width='100%' align='center'>")
        strBody.Append("    <tr>")
        strBody.Append("        <td style='background: #000 repeat; height: 5px;' ")
        strBody.Append("             ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

    End Sub


End Class