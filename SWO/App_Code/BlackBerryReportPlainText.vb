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

Public Class BlackBerryReportPlainText
  'Help Functions from our App_Code
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

    'Global Object Variables

    'IncidentID
    Public gStrIncidentID As String = ""
    Public gStrIsThisADrill As String = ""



    Public gStrTotalReport As String = ""

    Public gStrTotalReportHTML As String = ""

    Dim strBody As New StringBuilder("")
    Dim strBodyHTML As New StringBuilder("")



    'Constructor Expects IncidentID
    Public Sub New(ByVal strIncidentID As String)

        gStrIncidentID = strIncidentID

        strBodyHTML.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
        strBodyHTML.Append("<html xmlns='http://www.w3.org/1999/xhtml'>")
        strBodyHTML.Append("<head>")
        strBodyHTML.Append("<title>SWO Incident Tracker</title>")
        strBodyHTML.Append("</head>")
        strBodyHTML.Append("<body>")

        GetMainForm()

        'GetWorkSheets()

        If gStrIsThisADrill = "Yes" Then


            strBody.Append("THIS IS A DRILL")
            strBody.Append(Environment.NewLine)


            strBodyHTML.Append("THIS IS A DRILL")
            strBodyHTML.Append("<br>")


        End If

        strBodyHTML.Append("</body>")
        strBodyHTML.Append("</html>")

        gStrTotalReport = strBody.ToString
        gStrTotalReportHTML = strBodyHTML.ToString

    End Sub

    Protected Overrides Sub Finalize()
        ' Destructor
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
            ' AgencyDeptNotified = HelpFunction.Convertdbnulls(objDR("AgencyDeptNotified"))
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
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

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
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

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
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
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
            localThisSituationInvolves = " NO INCIDENT WORKSHEETS ADDED AT THIS TIME"
        End If

        gStrIsThisADrill = IsThisADrill

        'If this is a Drill Show
        If IsThisADrill = "Yes" Then

            strBody.Append("THIS IS A DRILL")
            strBody.Append(Environment.NewLine)

            strBodyHTML.Append("THIS IS A DRILL")
            strBodyHTML.Append("<br>")

        End If

        'Report Name

        strBody.Append("FDEM SWO Situational Awareness Report")
        strBody.Append(Environment.NewLine)


        strBodyHTML.Append("FDEM SWO Situational Awareness Report")
        strBodyHTML.Append("<br>")


        '/////////////////////////////////////////////////////////////////////////////////////////////////////

        'strBody.Append("<br>")
        'strBody.Append("<br>")
        'strBody.Append("<br>")
        'strBody.Append("<br>")
        'If this is a Drill Show







        'Exempt Checker Start
        Dim IsExempt As Boolean = False

        Dim localExemptCount As Integer = 0

        Dim localRecordAccountForArray As Integer = 0

        localRecordAccountForArray = MrDataGrabber.GrabRecordCountByKey("IncidentIncidentType", "IncidentID", gStrIncidentID)

        Dim localIncidentIncidentTypeLoopCount As Integer = 0

        If localRecordAccountForArray <> 0 Then

            'Must minus 1 to account for the Array Declaration
            Dim arrIncidentType(localRecordAccountForArray - 1) As Integer

            'Store each IncidentTypeID in Array
            'Checking to see if there are any worksheets that are exempt
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectIncidentIncidentTypeByIncidentID]", objConn)
            objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then
                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

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

            '
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

            strBody.Append("CONFIDENTIAL - FOUO")
            strBody.Append(Environment.NewLine)
            strBody.Append("This report is exempt from public records disclosure pursuant to § 119.071 F.S.")
            strBody.Append(Environment.NewLine)


            strBodyHTML.Append("CONFIDENTIAL - FOUO")
            strBodyHTML.Append("<br>")
            strBodyHTML.Append("This report is exempt from public records disclosure pursuant to § 119.071 F.S.")
            strBodyHTML.Append("<br>")

        End If

        'Exempt Checker End

        strBody.Append("Main Information:")
        strBody.Append(Environment.NewLine)


        strBodyHTML.Append("Main Information:")
        strBodyHTML.Append("<br>")


        ' ''Response.Write(StateAssistance)
        ' ''Response.End()

        If StateAssistance = "Yes" Then

            strBody.Append("STATE ASSISTANCE REQUESTED")
            strBody.Append(Environment.NewLine)

            strBodyHTML.Append("STATE ASSISTANCE REQUESTED")
            strBodyHTML.Append("<br>")

        ElseIf StateAssistance = "No" Then

            strBody.Append("NO STATE ASSISTANCE REQUESTED")
            strBody.Append(Environment.NewLine)

            strBodyHTML.Append("NO STATE ASSISTANCE REQUESTED")
            strBodyHTML.Append("<br>")

        End If

        strBody.Append("Report #: ")
        strBody.Append(localYear & "-" & CStr(localNumber))
        strBody.Append(Environment.NewLine)

        strBodyHTML.Append("Report #: ")
        strBodyHTML.Append(localYear & " - " & CStr(localNumber))
        strBodyHTML.Append("<br>")
        '===========================================================

        strBody.Append("Status: ")
        strBody.Append(MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentStatus", "IncidentStatus", "IncidentStatusID", MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("IncidentStatusID", "Incident", "IncidentID", gStrIncidentID).ToString).ToString)
        strBody.Append(Environment.NewLine)

        strBodyHTML.Append("Status: ")
        strBodyHTML.Append(MrDataGrabber.GrabOneStringColumnByPrimaryKey("IncidentStatus", "IncidentStatus", "IncidentStatusID", MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("IncidentStatusID", "Incident", "IncidentID", gStrIncidentID).ToString).ToString)
        strBodyHTML.Append("<br>")
        '===========================================================

        strBody.Append("Reported to SWO on: ")
        strBody.Append(ReportedToSWODate & " " & ReportedToSWOTime & ":" & ReportedToSWOTime2 & " ET ")
        strBody.Append(Environment.NewLine)

        strBodyHTML.Append("Reported to SWO on: ")
        strBodyHTML.Append(ReportedToSWODate & " &nbsp; " & ReportedToSWOTime & ":" & ReportedToSWOTime2 & " ET ")
        strBodyHTML.Append("<br>")
        '===========================================================

        strBody.Append("Severity: ")
        strBody.Append(localSeverity)
        strBody.Append(Environment.NewLine)

        strBodyHTML.Append("Severity: ")
        strBodyHTML.Append(localSeverity)
        strBodyHTML.Append("<br>")
        '===========================================================





        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
        'strBody.Append("            <b>Description:</b> ")
        'strBody.Append("            " & IncidentName & "   ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")


        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
        'strBody.Append("            <b>This situation involves:</b> ")
        'strBody.Append("        " & localThisSituationInvolves & "  ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")


        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
        'strBody.Append("            <b>Initial Report:</b> ")
        'strBody.Append("        " & localInitialReport & "     ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
        'strBody.Append("            <b>Incident Occurred:</b>")
        'strBody.Append("            " & IncidentOccurredDate & " &nbsp; " & IncidentOccurredTime & ":" & IncidentOccurredTime2 & " ET ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
        'strBody.Append("            <b>Most Recent Update Date/Time:</b>")
        'strBody.Append("        " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("LastUpdated", "Incident", "IncidentID", gStrIncidentID).ToString & " ET ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
        'strBody.Append("            <b>Most Recent Update:</b>")

        'If LatestUpdate = "" Then
        '    strBody.Append("        N/A     ")
        'Else
        '    strBody.Append("        " & LatestUpdate & "     ")
        'End If

        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")


        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
        'strBody.Append("                    <b>Affected Counties:</b>     ")
        'strBody.Append("                    " & localAllCounties & "     ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
        'strBody.Append("                <b>Facility Name or Description:</b>     ")
        'strBody.Append("                    " & FacilityNameSceneDescription & "     ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")


        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
        'strBody.Append("                <b>Incident Location:</b>     ")

        'If ObtainCoordinate = "AddressCity" Then

        '    strBody.Append("                   <i>Address:</i> " & Address & " <i>City:</i> " & City & "  ")

        'ElseIf ObtainCoordinate = "AddressZip" Then

        '    strBody.Append("                   <i>Address:</i> " & Address2 & " <i>Zip:</i> " & Zip & "  ")

        'ElseIf ObtainCoordinate = "Intersection" Then

        '    strBody.Append("                   <i>Street 1:</i> " & Street & " <i>Street 2:</i> " & Zip & " <i>City:</i> " & City2 & " ")

        'Else

        '    strBody.Append("                    " & "N/A" & "     ")

        'End If


        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        'strBody.Append("            <b>Contact Information</b>")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'Dim ReportingPartyType As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("ReportingPartyType", "ReportingPartyType", "ReportingPartyTypeID", ReportingPartyTypeID)

        'Dim ResponsiblePartyType As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("ResponsiblePartyType", "ResponsiblePartyType", "ResponsiblePartyTypeID", ResponsiblePartyTypeID)

        'Dim OnSceneContactType As String = MrDataGrabber.GrabOneStringColumnByPrimaryKey("OnSceneContactType", "OnSceneContactType", "OnSceneContactTypeID", OnSceneContactTypeID)


        'Dim ReportingPartyTypeInfo As String = ""

        'Dim localReportingPartyTypeFirstName As String = ""
        'Dim localReportingPartyTypeLastName As String = ""
        'Dim localReportingPartyTypeCallBackNumber1 As String = ""
        'Dim localReportingPartyTypeCallBackNumber2 As String = ""
        'Dim localReportingPartyTypeEmail As String = ""
        'Dim localReportingPartyTypeAddress As String = ""
        'Dim localReportingPartyTypeCity As String = ""
        'Dim localReportingPartyTypeState As String = ""
        'Dim localReportingPartyTypeZipcode As String = ""
        'Dim localReportingPartyTypeRepresents As String = ""

        'If ReportingPartyType = "As Below" Then

        '    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        '    objConn.Open()
        '    objCmd = New SqlCommand("spSelectReportingPartyByIncidentID", objConn)
        '    objCmd.CommandType = CommandType.StoredProcedure
        '    objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

        '    objDR = objCmd.ExecuteReader

        '    If objDR.Read() Then

        '        localReportingPartyTypeFirstName = HelpFunction.Convertdbnulls(objDR("FirstName"))
        '        localReportingPartyTypeLastName = HelpFunction.Convertdbnulls(objDR("LastName"))
        '        localReportingPartyTypeCallBackNumber1 = HelpFunction.Convertdbnulls(objDR("CallBackNumber1"))
        '        localReportingPartyTypeCallBackNumber2 = HelpFunction.Convertdbnulls(objDR("CallBackNumber2"))
        '        localReportingPartyTypeEmail = HelpFunction.Convertdbnulls(objDR("Email"))
        '        localReportingPartyTypeAddress = HelpFunction.Convertdbnulls(objDR("Address"))
        '        localReportingPartyTypeCity = HelpFunction.Convertdbnulls(objDR("City"))
        '        localReportingPartyTypeState = HelpFunction.Convertdbnulls(objDR("State"))
        '        localReportingPartyTypeZipcode = HelpFunction.Convertdbnulls(objDR("Zipcode"))
        '        localReportingPartyTypeRepresents = HelpFunction.Convertdbnulls(objDR("Represents"))

        '    End If

        '    objDR.Close()

        '    objCmd.Dispose()
        '    objCmd = Nothing

        '    objConn.Close()




        'If localReportingPartyTypeFirstName <> "" Then
        '    ReportingPartyTypeInfo = "<i> Name: </i>" & localReportingPartyTypeFirstName & " " & localReportingPartyTypeLastName & ", " & localReportingPartyTypeRepresents
        'End If

        'If localReportingPartyTypeCallBackNumber1 <> "" Then
        '    ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Call Back Number 1: </i>" & localReportingPartyTypeCallBackNumber1
        'End If

        'If localReportingPartyTypeCallBackNumber2 <> "" Then
        '    ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Call Back Number 2: </i>" & localReportingPartyTypeCallBackNumber2
        'End If

        'If localReportingPartyTypeEmail <> "" Then
        '    ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Email: </i>" & localReportingPartyTypeEmail
        'End If

        'If localReportingPartyTypeAddress <> "" Then
        '    ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Address: </i>" & localReportingPartyTypeAddress & " " & localReportingPartyTypeCity & " " & localReportingPartyTypeState & ", " & localReportingPartyTypeZipcode
        'End If





        '    If localReportingPartyTypeFirstName <> "" Then
        '        ReportingPartyTypeInfo = "<i> First Name: </i>" & localReportingPartyTypeFirstName
        '    End If

        '    If localReportingPartyTypeLastName <> "" Then
        '        ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Last Name: </i>" & localReportingPartyTypeLastName
        '    End If

        '    If localReportingPartyTypeCallBackNumber1 <> "" Then
        '        ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Call Back Number 1: </i>" & localReportingPartyTypeCallBackNumber1
        '    End If

        '    If localReportingPartyTypeCallBackNumber2 <> "" Then
        '        ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Call Back Number 2: </i>" & localReportingPartyTypeCallBackNumber2
        '    End If

        '    If localReportingPartyTypeEmail <> "" Then
        '        ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Email: </i>" & localReportingPartyTypeEmail
        '    End If

        '    If localReportingPartyTypeAddress <> "" Then
        '        ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Address: </i>" & localReportingPartyTypeAddress
        '    End If

        '    If localReportingPartyTypeCity <> "" Then
        '        ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |City: </i>" & localReportingPartyTypeCity
        '    End If

        '    If localReportingPartyTypeState <> "" Then
        '        ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |State: </i>" & localReportingPartyTypeState
        '    End If

        '    If localReportingPartyTypeZipcode <> "" Then
        '        ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Zipcode: </i>" & localReportingPartyTypeZipcode
        '    End If

        '    If localReportingPartyTypeRepresents <> "" Then
        '        ReportingPartyTypeInfo = ReportingPartyTypeInfo & "<i> |Represents: </i>" & localReportingPartyTypeRepresents
        '    End If

        'Else
        '    ReportingPartyTypeInfo = ReportingPartyType
        'End If

        'Dim ResponsiblePartyInfo As String = ""

        'Dim localResponsiblePartyFirstName As String = ""
        'Dim localResponsiblePartyLastName As String = ""
        'Dim localResponsiblePartyCallBackNumber1 As String = ""
        'Dim localResponsiblePartyCallBackNumber2 As String = ""
        'Dim localResponsiblePartyEmail As String = ""
        'Dim localResponsiblePartyAddress As String = ""
        'Dim localResponsiblePartyCity As String = ""
        'Dim localResponsiblePartyState As String = ""
        'Dim localResponsiblePartyZipcode As String = ""
        'Dim localResponsiblePartyRepresents As String = ""


        'If ResponsiblePartyType = "As Below" Then

        '    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        '    objConn.Open()
        '    objCmd = New SqlCommand("spSelectResponsiblePartyByIncidentID", objConn)
        '    objCmd.CommandType = CommandType.StoredProcedure
        '    objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

        '    objDR = objCmd.ExecuteReader

        '    If objDR.Read() Then

        '        localResponsiblePartyFirstName = HelpFunction.Convertdbnulls(objDR("FirstName"))
        '        localResponsiblePartyLastName = HelpFunction.Convertdbnulls(objDR("LastName"))
        '        localResponsiblePartyCallBackNumber1 = HelpFunction.Convertdbnulls(objDR("CallBackNumber1"))
        '        localResponsiblePartyCallBackNumber2 = HelpFunction.Convertdbnulls(objDR("CallBackNumber2"))
        '        localResponsiblePartyEmail = HelpFunction.Convertdbnulls(objDR("Email"))
        '        localResponsiblePartyAddress = HelpFunction.Convertdbnulls(objDR("Address"))
        '        localResponsiblePartyCity = HelpFunction.Convertdbnulls(objDR("City"))
        '        localResponsiblePartyState = HelpFunction.Convertdbnulls(objDR("State"))
        '        localResponsiblePartyZipcode = HelpFunction.Convertdbnulls(objDR("Zipcode"))
        '        localResponsiblePartyRepresents = HelpFunction.Convertdbnulls(objDR("Represents"))

        '    End If

        '    objDR.Close()

        '    objCmd.Dispose()
        '    objCmd = Nothing

        '    objConn.Close()






        'If localResponsiblePartyFirstName <> "" Then
        '    ResponsiblePartyInfo = "<i> Name: </i>" & localResponsiblePartyFirstName & " " & localResponsiblePartyLastName & ", " & localResponsiblePartyRepresents
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
        '    ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Address: </i>" & localResponsiblePartyAddress & " " & localResponsiblePartyCity & " " & localResponsiblePartyState & ", " & localResponsiblePartyZipcode
        'End If



        '    If localResponsiblePartyFirstName <> "" Then
        '        ResponsiblePartyInfo = "<i> First Name: </i>" & localResponsiblePartyFirstName
        '    End If

        '    If localResponsiblePartyLastName <> "" Then
        '        ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Last Name: </i>" & localResponsiblePartyLastName
        '    End If

        '    If localResponsiblePartyCallBackNumber1 <> "" Then
        '        ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Call Back Number 1: </i>" & localResponsiblePartyCallBackNumber1
        '    End If

        '    If localResponsiblePartyCallBackNumber2 <> "" Then
        '        ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Call Back Number 2: </i>" & localResponsiblePartyCallBackNumber2
        '    End If

        '    If localResponsiblePartyEmail <> "" Then
        '        ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Email: </i>" & localResponsiblePartyEmail
        '    End If

        '    If localResponsiblePartyAddress <> "" Then
        '        ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Address: </i>" & localResponsiblePartyAddress
        '    End If

        '    If localResponsiblePartyCity <> "" Then
        '        ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |City: </i>" & localResponsiblePartyCity
        '    End If

        '    If localResponsiblePartyState <> "" Then
        '        ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |State: </i>" & localResponsiblePartyState
        '    End If

        '    If localResponsiblePartyZipcode <> "" Then
        '        ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Zipcode: </i>" & localResponsiblePartyZipcode
        '    End If

        '    If localResponsiblePartyRepresents <> "" Then
        '        ResponsiblePartyInfo = ResponsiblePartyInfo & "<i> |Represents: </i>" & localResponsiblePartyRepresents
        '    End If

        'Else
        '    ResponsiblePartyInfo = ResponsiblePartyType
        'End If


        'Dim OnSceneContactInfo As String = ""

        'Dim localOnSceneContactFirstName As String = ""
        'Dim localOnSceneContactLastName As String = ""
        'Dim localOnSceneContactCallBackNumber1 As String = ""
        'Dim localOnSceneContactCallBackNumber2 As String = ""
        'Dim localOnSceneContactEmail As String = ""
        'Dim localOnSceneContactAddress As String = ""
        'Dim localOnSceneContactCity As String = ""
        'Dim localOnSceneContactState As String = ""
        'Dim localOnSceneContactZipcode As String = ""
        'Dim localOnSceneContactRepresents As String = ""

        'If OnSceneContactType = "As Below" Then

        '    objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        '    objConn.Open()
        '    objCmd = New SqlCommand("spSelectOnSceneContactByIncidentID", objConn)
        '    objCmd.CommandType = CommandType.StoredProcedure
        '    objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

        '    objDR = objCmd.ExecuteReader

        '    If objDR.Read() Then

        '        localOnSceneContactFirstName = HelpFunction.Convertdbnulls(objDR("FirstName"))
        '        localOnSceneContactLastName = HelpFunction.Convertdbnulls(objDR("LastName"))
        '        localOnSceneContactCallBackNumber1 = HelpFunction.Convertdbnulls(objDR("CallBackNumber1"))
        '        localOnSceneContactCallBackNumber2 = HelpFunction.Convertdbnulls(objDR("CallBackNumber2"))
        '        localOnSceneContactEmail = HelpFunction.Convertdbnulls(objDR("Email"))
        '        localOnSceneContactAddress = HelpFunction.Convertdbnulls(objDR("Address"))
        '        localOnSceneContactCity = HelpFunction.Convertdbnulls(objDR("City"))
        '        localOnSceneContactState = HelpFunction.Convertdbnulls(objDR("State"))
        '        localOnSceneContactZipcode = HelpFunction.Convertdbnulls(objDR("Zipcode"))
        '        localOnSceneContactRepresents = HelpFunction.Convertdbnulls(objDR("Represents"))

        '    End If

        '    objDR.Close()

        '    objCmd.Dispose()
        '    objCmd = Nothing

        '    objConn.Close()




        'If localOnSceneContactFirstName <> "" Then
        '    OnSceneContactInfo = "<i> Name: </i>" & localOnSceneContactFirstName & " " & localOnSceneContactLastName & ", " & localOnSceneContactRepresents
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
        '    OnSceneContactInfo = OnSceneContactInfo & "<i> |Address: </i>" & localOnSceneContactAddress & " " & localOnSceneContactCity & " " & localOnSceneContactState & ", " & localOnSceneContactZipcode
        'End If




        '    If localOnSceneContactFirstName <> "" Then
        '        OnSceneContactInfo = "<i> First Name: </i>" & localOnSceneContactFirstName
        '    End If

        '    If localOnSceneContactLastName <> "" Then
        '        OnSceneContactInfo = OnSceneContactInfo & "<i> |Last Name: </i>" & localOnSceneContactLastName
        '    End If

        '    If localOnSceneContactCallBackNumber1 <> "" Then
        '        OnSceneContactInfo = OnSceneContactInfo & "<i> |Call Back Number 1: </i>" & localOnSceneContactCallBackNumber1
        '    End If

        '    If localOnSceneContactCallBackNumber2 <> "" Then
        '        OnSceneContactInfo = OnSceneContactInfo & "<i> |Call Back Number 2: </i>" & localOnSceneContactCallBackNumber2
        '    End If

        '    If localOnSceneContactEmail <> "" Then
        '        OnSceneContactInfo = OnSceneContactInfo & "<i> |Email: </i>" & localOnSceneContactEmail
        '    End If

        '    If localOnSceneContactAddress <> "" Then
        '        OnSceneContactInfo = OnSceneContactInfo & "<i> |Address: </i>" & localOnSceneContactAddress
        '    End If

        '    If localOnSceneContactCity <> "" Then
        '        OnSceneContactInfo = OnSceneContactInfo & "<i> |City: </i>" & localOnSceneContactCity
        '    End If

        '    If localOnSceneContactState <> "" Then
        '        OnSceneContactInfo = OnSceneContactInfo & "<i> |State: </i>" & localOnSceneContactState
        '    End If

        '    If localOnSceneContactZipcode <> "" Then
        '        OnSceneContactInfo = OnSceneContactInfo & "<i> |Zipcode: </i>" & localOnSceneContactZipcode
        '    End If

        '    If localOnSceneContactRepresents <> "" Then
        '        OnSceneContactInfo = OnSceneContactInfo & "<i> |Represents: </i>" & localOnSceneContactRepresents
        '    End If

        'Else
        '    OnSceneContactInfo = OnSceneContactType
        'End If

        ''////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
        'strBody.Append("            <b>Reporting Party:</b>     ")
        'strBody.Append("            " & ReportingPartyTypeInfo & "  ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
        'strBody.Append("            <b>Responsible Party:</b>     ")
        'strBody.Append("            " & ResponsiblePartyInfo & "  ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
        'strBody.Append("            <b>On-Scene Contact:</b>     ")
        'strBody.Append("            " & OnSceneContactInfo & "  ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")



    End Sub

    Protected Sub GetWorkSheets()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        DBConStringHelper.PrepareConnection(objConn) 'open the connection
        objCmd = New SqlCommand("[spSelectIncidentIncidentTypeByIncidentID]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
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
                ElseIf CStr(objDR.Item("IncidentType")) = "Kennedy Space Center" Then
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

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()

        'MarineIncident

    End Sub

    Protected Sub test()

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        DBConStringHelper.PrepareConnection(objConn) 'open the connection

        objCmd = New SqlCommand("[spSelectLatestUpdateByIncidentID]", objConn)
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)




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


        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Road Closure or DOT Issue</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>This situation is:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Roadway Name and/or number:</b> ")
        strBody.Append("           " & RoadwayNameNumber & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>At:</b> ")
        strBody.Append("           " & At & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Mile Marker:</b> ")
        strBody.Append("           " & MileMarker & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Exit Ramp:</b> ")
        strBody.Append("           " & ExitRamp & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")



        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Cross Street 1 or Intersection:</b> ")
        strBody.Append("           " & CrossStreet1Intersection & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Cross Street 2:</b> ")
        strBody.Append("           " & CrossStreet2 & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Duration of closure (if known):</b> ")
        strBody.Append("           " & DurationOfClosure & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>What department/agency directed the closure:</b> ")
        strBody.Append("           " & DepartmentAgencyDirectedClosure & "  ")
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
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
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





        ''////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Hazardous Materials</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>This situation is:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")


        If SubType = "Biological Hazard" Then


            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Common Name:</b> ")
            strBody.Append("           " & CommonName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")



            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Scientific Name:</b> ")
            strBody.Append("           " & ScientificName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")



            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Quantity Description:</b> ")
            strBody.Append("           " & QuantityDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")



            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Container or device description:</b> ")
            strBody.Append("           " & ContainerDeviceDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")



            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Total quantity:</b> ")
            strBody.Append("           " & BiologicalTotalQuantity & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")



            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Quantity released:</b> ")
            strBody.Append("           " & BiologicalQuantityReleased & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")


        ElseIf SubType = "Chemical Agent" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Anticipated State assistance Need:</b> ")
            strBody.Append("           " & AgentType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Agent name:</b> ")
            strBody.Append("           " & AgentName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Container or device description:</b> ")
            strBody.Append("           " & AgentContainerDeviceDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")



            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Total quantity:</b> ")
            strBody.Append("           " & AgentTotalQuantity & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Quantity released:</b> ")
            strBody.Append("           " & AgentQuantityReleased & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")




        ElseIf SubType = "Radiological Material" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Anticipated State assistance Need:</b> ")
            strBody.Append("           " & RadiationType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Agent name:</b> ")
            strBody.Append("           " & IsotopeName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Container or device description:</b> ")
            strBody.Append("           " & ContainerDeviceInstrumentDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")



            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Total quantity:</b> ")
            strBody.Append("           " & RadiationTotalQuantity & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Local or regional assistance requested:</b> ")
            strBody.Append("           " & DOHBureauNotified & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")



        ElseIf SubType = "Toxic Industrial Chemical" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Chemical Name:</b> ")
            strBody.Append("           " & ChemicalName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")


            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Index Name:</b> ")
            strBody.Append("           " & IndexName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")


            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>CAS Number:</b> ")
            strBody.Append("           " & CASNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")


            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Section 304 Reportable Quantity:</b> ")
            strBody.Append("           " & CERCLAReportableQuantity & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")


            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>CERCLA Reportable Quantity:</b> ")
            strBody.Append("           " & CERCLAReportableQuantity & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")


            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Chemical State:</b> ")
            strBody.Append("           " & ChemicalState & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")


            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Source / Container:</b> ")
            strBody.Append("           " & SourceContainer & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If SourceContainer = "Aboveground Pipeline" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Diameter of the Pipeline:</b> ")
                strBody.Append("           " & DiameterPipeline & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Unbroken end of the pipe connected to:</b> ")
                strBody.Append("           " & UnbrokenEndPipeConnectedTo & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

            End If

            If SourceContainer = "Underground Pipeline" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Diameter of the Pipeline:</b> ")
                strBody.Append("           " & DiameterPipeline & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Unbroken end of the pipe connected to:</b> ")
                strBody.Append("           " & UnbrokenEndPipeConnectedTo & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

            End If







            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Total source/container volume:</b> ")
            strBody.Append("           " & TotalSourceContainerVolume & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")


            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Quantity released:</b> ")
            strBody.Append("           " & ChemicalRateOfRelease & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Rate of release:</b> ")
            strBody.Append("           " & ChemicalReleased & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Cause of release:</b> ")
            strBody.Append("           " & CauseOfRelease & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Time the release was discovered:</b> ")
            strBody.Append("           " & TimeReleaseDiscovered & " ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Time the release was secured:</b> ")
            strBody.Append("           " & TimeReleaseSecured & " ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Reason for late report, if applicable:</b> ")
            strBody.Append("           " & ReasonLateReport & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Were any storm drains affected?</b> ")
            strBody.Append("           " & StormDrainsAffected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Were any waterways affected?</b> ")
            strBody.Append("           " & WaterwaysAffected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If WaterwaysAffected = "Yes" Then

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Name(s) of waterways:</b> ")
                strBody.Append("           " & WaterwaysAffectedText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Is a callback from DEP requested?:</b> ")
            strBody.Append("           " & CallbackDEPRequested & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If CallbackDEPRequested = "Yes" Then

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>DEP Contact:</b> ")
                strBody.Append("           " & CallbackDEPRequestedDDLValue & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

            End If

        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are there any evacuations?</b> ")
        strBody.Append("           " & Evacuations & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are any major roadways closed?</b> ")
        strBody.Append("           " & MajorRoadwaysClosed & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are there Injuries?</b> ")
        strBody.Append("           " & Injury & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If Injury = "Yes" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and Severity of Injuries:</b> ")
            strBody.Append("           " & InjuryText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are there Fatalities?</b> ")
        strBody.Append("           " & Fatality & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")


        If Fatality = "Yes" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and location:</b> ")
            strBody.Append("           " & FatalityText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        End If


        'strBody.Append("<table width='400px' align='left'>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
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
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            HazMatOnBoard = HelpFunction.Convertdbnulls(objDR2("HazMatOnBoard"))
            FuelPetroleumSpills = HelpFunction.Convertdbnulls(objDR2("FuelPetroleumSpills"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Vehicle</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>This situation is:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Number of vehicles involved:</b> ")
        strBody.Append("           " & VehiclesInvolvedNumber & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Type(s) of vehicles:</b> ")
        strBody.Append("           " & VehicleType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Number of people involved:</b> ")
        strBody.Append("           " & PeopleInvolvedNumber & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>If commercial carrier, Owned/Operated By:</b> ")
        strBody.Append("           " & CommercialCarrierOwnedOperatedBy & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")


        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>What is the cause the incident (if known)?</b> ")
        strBody.Append("           " & IncidentCause & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Is there a fire?</b> ")
        strBody.Append("           " & Fire & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are there Injuries?</b> ")
        strBody.Append("           " & Injury & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If Injury = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and Severity of Injuries:</b> ")
            strBody.Append("           " & InjuryText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are there Fatalities?</b> ")
        strBody.Append("           " & Fatality & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If Fatality = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and location:</b> ")
            strBody.Append("           " & FatalityText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If


        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are there any hazardous materials onboard?</b> ")
        strBody.Append("           " & HazMatOnBoard & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are there any fuel or Petroleum Spills:</b> ")
        strBody.Append("           " & FuelPetroleumSpills & "  ")
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

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
            strBody.Append("            <b>AIRCRAFT INCIDENT</b>")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Sub-Type:</b> ")
            strBody.Append("           " & SubType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>This situation is:</b> ")
            strBody.Append("           " & Situation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Description:</b> ")
            strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Aircraft Type:</b> ")
            strBody.Append("           " & AircraftType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Tail Number:</b> ")
            strBody.Append("           " & TailNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Owned/Operated By:</b> ")
            strBody.Append("           " & OwnedOperatedBy & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Cause of Incident:</b> ")
            strBody.Append("           " & CauseOfIncident & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Aircraft Fire:</b> ")
            strBody.Append("           " & Fire & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")


            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number of People Onboard:</b> ")
            strBody.Append("           " & NumberPeopleOnboard & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Injuries:</b> ")
            strBody.Append("           " & Injury & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Injury = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Number and Severity of Injuries:</b> ")
                strBody.Append("           " & InjuryText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If


            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Fatalities:</b> ")
            strBody.Append("           " & Fatality & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Fatality = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Number and location (aircraft or ground):</b> ")
                strBody.Append("           " & FatalityText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If


            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Are other structures or roadways involved?</b> ")
            strBody.Append("           " & StructuresRoadwaysInvolved & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")


            If StructuresRoadwaysInvolved = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Description:</b> ")
                strBody.Append("           " & StructuresRoadwaysInvolvedText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

 
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Hazardous materials onboard?</b> ")
            strBody.Append("           " & HazMatOnboard & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Fuel or Petroleum Spills?</b> ")
            strBody.Append("           " & FuelPetroleumSpills & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Are there any evacuations?</b> ")
            strBody.Append("           " & Evacuations & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")


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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Animal or Agricultural</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Severity Level:</b> ")
        strBody.Append("           " & SeverityLevel & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        

        If SubType = "Animal Disease" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>What animal(s) are affected?</b> ")
            strBody.Append("           " & AnimalAffected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>What type of disease, if known?</b> ")
            strBody.Append("           " & AnimalDiseaseType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number of animals infected?</b> ")
            strBody.Append("           " & AnimalInfected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number of animals deceased?</b> ")
            strBody.Append("           " & AnimalTestExaminations & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Tests or examinations are planned or occuring?</b> ")
            strBody.Append("           " & AnimalsDeceased & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Is there a quarantine in effect?</b> ")
            strBody.Append("           " & AnimalQuarantine & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If AnimalQuarantine = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Describe area, listing streets or landmarks:</b> ")
                strBody.Append("           " & AnimalQuarantineText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Are any humans affected?</b> ")
            strBody.Append("           " & AnimalHumansAffected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If AnimalHumansAffected = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Number and Severity of Illness:</b> ")
                strBody.Append("           " & AnimalHumansAffectedText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Are there any human fatalities?</b> ")
            strBody.Append("           " & AnimalHumanFatalities & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If AnimalHumanFatalities = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Number and Information:</b> ")
                strBody.Append("           " & AnimalHumanFatalitiesText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

        ElseIf SubType = "Agricultural Disease" Or SubType = "Crop Failure" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>What crop(s) are affected?</b> ")
            strBody.Append("           " & ADCFcropsAffected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>What type of disease, if known?</b> ")
            strBody.Append("           " & ADCFdiseaseType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number of acres affected? </b> ")
            strBody.Append("           " & ADCFacresAffected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Food Supply Contamination" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>What type / brand of food?</b> ")
            strBody.Append("           " & FSCtypeBrand & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Where was it manufactured/packed?</b> ")
            strBody.Append("           " & FSCmanufacturedPacked & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Affected lot number(s)?</b> ")
            strBody.Append("           " & FSCaffectedLotNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Affected date range?</b> ")
            strBody.Append("           " & FSCaffectedDateRange & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Has a recall been issued?</b> ")
            strBody.Append("           " & FSCrecallIssued & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        End If

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
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Bomb Threat or Device</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")



        If SubType = "Bomb or Device Explosion" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>How was the threat received/who found the device?</b> ")
            strBody.Append("           " & HowReceivedWhoFound & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Exact wording of threat:</b> ")
            strBody.Append("           " & ExactWordingThreat & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Description of the bomb or device:</b> ")
            strBody.Append("           " & Description & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Are there any evacuations?</b> ")
            strBody.Append("           " & Evacuations & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Are any major roadways closed?</b> ")
            strBody.Append("           " & MajorRoadwaysClosed & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")


            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Are there Injuries?</b> ")
            strBody.Append("           " & Injury & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Injury = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Number and Severity of Injuries:</b> ")
                strBody.Append("           " & InjuryText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Are there Fatalities?</b> ")
            strBody.Append("           " & Fatality & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Fatality = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Number and location:</b> ")
                strBody.Append("           " & FatalityText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If


        Else

            If SubType = "Unconfirmed Threat" Or SubType = "Unfounded Threat" Then

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>How was the threat received/who found the device?</b> ")
                strBody.Append("           " & HowReceivedWhoFound & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Exact wording of threat:</b> ")
                strBody.Append("           " & ExactWordingThreat & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Description of the bomb or device:</b> ")
                strBody.Append("           " & Description & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Are there any evacuations?</b> ")
                strBody.Append("           " & Evacuations & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Are any major roadways closed?</b> ")
                strBody.Append("           " & MajorRoadwaysClosed & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Is a search being conducted?</b> ")
                strBody.Append("           " & SearchBeingConducted & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                If SearchBeingConducted = "Yes" Then
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='400px'>")
                    strBody.Append("            <b>Number and location:</b> ")
                    strBody.Append("           " & DepartmentAgencySearch & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")
                End If

            Else

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>How was the threat received/who found the device?</b> ")
                strBody.Append("           " & HowReceivedWhoFound & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Exact wording of threat:</b> ")
                strBody.Append("           " & ExactWordingThreat & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Description of the bomb or device:</b> ")
                strBody.Append("           " & Description & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Are there any evacuations?</b> ")
                strBody.Append("           " & Evacuations & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Are any major roadways closed?</b> ")
                strBody.Append("           " & MajorRoadwaysClosed & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

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
            Evacuations = HelpFunction.Convertdbnulls(objDR2("Evacuations"))
            MajorRoadwaysClosed = HelpFunction.Convertdbnulls(objDR2("MajorRoadwaysClosed"))
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Civil Disturbance</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>What is the cause of the disturbance?</b> ")
        strBody.Append("           " & Cause & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>What group(s) or organization(s) are responsible?</b> ")
        strBody.Append("           " & GroupOrgResponsible & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>How many people are participating?</b> ")
        strBody.Append("           " & PeopleParticipatingNum & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Is the incident confined to one location?</b> ")
        strBody.Append("           " & ConfinedLocation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If ConfinedLocation = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Location:</b> ")
            strBody.Append("           " & ConfinedLocationOther & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        If ConfinedLocationOther = "Other Area" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Areas:</b> ")
            strBody.Append("           " & LocationAreas & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If


        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Law enforcement agency coordinating response:</b> ")
        strBody.Append("           " & AgencyCoordinatingResponse & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Departments/Agencies are responding or on scene:</b> ")
        strBody.Append("           " & DepartmentAgencyResponding & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are there any evacuations?</b> ")
        strBody.Append("           " & Evacuations & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are any major roadways closed?</b> ")
        strBody.Append("           " & MajorRoadwaysClosed & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Injuries:</b> ")
        strBody.Append("           " & Injury & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")


        If Injury = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and Severity of Injuries:</b> ")
            strBody.Append("           " & InjuryText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Fatalities:</b> ")
        strBody.Append("           " & Fatality & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If Fatality = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and location:</b> ")
            strBody.Append("           " & FatalityText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Criminal Activity</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description the incident:</b> ")
        strBody.Append("           " & IncidentDescription & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description of the individual(s) responsible:</b> ")
        strBody.Append("           " & IndividualDescription & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Is the incident confined to one location?</b> ")
        strBody.Append("           " & ConfinedLocation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")


        If ConfinedLocation = "No" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Area(s); specific streets/boundaries preferable:</b> ")
            strBody.Append("           " & ConfinedLocationText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf ConfinedLocation = "Yes" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Location:</b> ")
            strBody.Append("           " & ConfinedLocationDDL & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        End If

        If ConfinedLocationDDL = "Other area" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Area(s); specific streets/boundaries preferable:</b> ")
            strBody.Append("           " & ConfinedLocationText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Law enforcement agency coordinating response:</b> ")
        strBody.Append("           " & AgencyCoordinatingResponse & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Departments/agencies responding or on scene:</b> ")
        strBody.Append("           " & DepartmentAgencyResponding & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Is there a lockdown?</b> ")
        strBody.Append("           " & Lockdown & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Lockdown Area Description:</b> ")
        strBody.Append("           " & LockdownText & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are there any evacuations?</b> ")
        strBody.Append("           " & Evacuations & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Major roadways closed:</b> ")
        strBody.Append("           " & MajorRoadwaysClosed & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Injuries:</b> ")
        strBody.Append("           " & Injury & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Number and Severity of Injuries:</b> ")
        strBody.Append("           " & InjuryText & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Fatalities:</b> ")
        strBody.Append("           " & Fatality & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Number and location:</b> ")
        strBody.Append("           " & FatalityText & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Anticipated need for state assistance:</b> ")
        strBody.Append("           " & StateAssistance & "  ")
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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Dam Failure</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Dam Name:</b> ")
        strBody.Append("           " & DamName & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Related Waterways/Tributaries:</b> ")
        strBody.Append("           " & RelatedWaterways & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Pool volume/capacity behind the dam:</b> ")
        strBody.Append("           " & PoolVolumeCapacity & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Has a break occurred?</b> ")
        strBody.Append("           " & BreakOccurred & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If BreakOccurred = "Yes" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Cause of failure:</b> ")
            strBody.Append("           " & CauseOfFailure & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf BreakOccurred = "No" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Is a break anticipated?</b> ")
            strBody.Append("           " & BreakAnticipated & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Who is responsible for maintaining the dam? (if known):</b> ")
        strBody.Append("           " & ResponsibleForMaintaining & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>What corrective actions are being taken?</b> ")
        strBody.Append("           " & CorrectiveActionsTaken & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Estimated date that repairs will be completed:</b> ")
        strBody.Append("           " & EstimatedRepairDate & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Estimated time that repairs will be completed:</b> ")
        strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Is the incident confined to one location?</b> ")
        strBody.Append("           " & DownstreamPopulationsThreat & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If DownstreamPopulationsThreat = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Corrective actions being taken:</b> ")
            strBody.Append("           " & DownstreamPopulationsThreatText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Evacuations?</b> ")
        strBody.Append("           " & Evacuations & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Major roadways closed:</b> ")
        strBody.Append("           " & MajorRoadwaysClosed & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Injuries:</b> ")
        strBody.Append("           " & Injury & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If Injury = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and Severity of Injuries:</b> ")
            strBody.Append("           " & InjuryText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Fatalities:</b> ")
        strBody.Append("           " & Fatality & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If Fatality = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and location:</b> ")
            strBody.Append("           " & FatalityText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Anticipated need for state assistance?</b> ")
        strBody.Append("           " & StateAssistance & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If StateAssistance = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Describe the anticipated need(s):</b> ")
            strBody.Append("           " & StateAssistanceText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Agencies are responding or on scene:</b> ")
        strBody.Append("           " & AgencyResponse & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Location of Staging Area or Command Post:</b> ")
        strBody.Append("           " & StagingCommandLocation & "  ")
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

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>DEM Incidents/Notifications/Reports</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "SLRC Alarm" Or SubType = "SEOC Alarm" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Alarm Type:</b> ")
            strBody.Append("           " & SlrcSeocAlarmType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Zone number(s)and/or description(s):</b> ")
            strBody.Append("           " & SlrcSeocZoneNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Alarm Status:</b> ")
            strBody.Append("           " & SlrcSeocAlarmStatus & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "DEP Alarm" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Label/Memo that appears after selection:</b> ")
            strBody.Append("           " & DepWarehouseMemo & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Alarm or Non-Alarm Notification:</b> ")
            strBody.Append("           " & DepWarehouseNotification & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If DepWarehouseNotification = "Alarm" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Zone number(s) and/or description(s):</b> ")
                strBody.Append("           " & DepWarehouseZoneNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Alarm Status:</b> ")
                strBody.Append("           " & DepWarehouseAlarmStatus & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If DepWarehouseNotification = "Non-Alarm Notification" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Employee name:</b> ")
                strBody.Append("           " & DepWarehouseEmployeeName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Employee cell phone:</b> ")
                strBody.Append("           " & DepWarehouseEmployeeCellPhone & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Agency and Division:</b> ")
                strBody.Append("           " & DepWarehouseAgencyDivision & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Supervisor name:</b> ")
                strBody.Append("           " & DepWarehouseSupervisorName & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Has supervisor been called?</b> ")
                strBody.Append("           " & DepWarehouseSupervisorCalled & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Access card number:</b> ")
                strBody.Append("           " & DepWarehouseAccessCardNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

        ElseIf SubType = "Medical Emergency" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Building and Room Number:</b> ")
            strBody.Append("           " & MeBuildingRoomNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Has someone called 911?</b> ")
            strBody.Append("           " & Me911Called & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Is the person breathing? </b> ")
            strBody.Append("           " & MePersonBreathing & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Person's level of consiousness?</b> ")
            strBody.Append("           " & MeConsiousness & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Person's complaint or symptoms:</b> ")
            strBody.Append("           " & MeComplaintSymptom & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "SEOC Activation" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Activation level:</b> ")
            strBody.Append("           " & SeocActivationLevel & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Related Incident Numbers:</b> ")
            strBody.Append("           " & SeocActivationRelatedIncidentNumbers & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>EM Constellation Database:</b> ")
            strBody.Append("           " & SeocActivationEmcDatabase & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>EMC Database Name:</b> ")
            strBody.Append("           " & SeocActivationEmcDatabaseName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "SMT Activation" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Select SMT:</b> ")
            strBody.Append("           " & SmtActivationSMT & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Describe reason for activation:</b> ")
            strBody.Append("           " & SmtActivationReason & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Location to Report:</b> ")
            strBody.Append("           " & SmtActivationReportLocation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Authorized By:</b> ")
            strBody.Append("           " & SmtActivationAuthorizedBy & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Reservist Activation" Then

            'strBody.Append("<table>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td align='left'width='400px'>")
            'strBody.Append("            <b>Select SMT:</b> ")
            'strBody.Append("           " & ReservistActivationSMT & "  ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Describe reason for activation:</b> ")
            strBody.Append("           " & ReservistActivationReason & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Location to Report:</b> ")
            strBody.Append("           " & ReservistActivationReportLocation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Authorized By:</b> ")
            strBody.Append("           " & ReservistActivationAuthorizedBy & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "General Notification" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>General Notification Message:</b> ")
            strBody.Append("           " & GeneralNotificationMessage & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Authorized By:</b> ")
            strBody.Append("           " & GeneralNotificationAuthorizedBy & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "IT Disruption or Issue" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Description of Problem:</b> ")
            strBody.Append("           " & ItDisruptionDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Name of program(s)/system(s):</b> ")
            strBody.Append("           " & ItDisruptionprogramSystem & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Time the problem started:</b> ")
            strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>List any troubleshooting steps taken:</b> ")
            strBody.Append("           " & ItDisruptionStepsTaken & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Communications Disruption or Issue" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Communication system(s) or circuit(s):</b> ")
            strBody.Append("           " & CommDisruptionSystemCircuit & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If CommDisruptionSystemCircuit = "Other" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>System:</b> ")
                strBody.Append("           " & CommDisruptionSystemCircuitText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If


            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Describe the problem:</b> ")
            strBody.Append("           " & CommDisruptionDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Time the problem started:</b> ")
            strBody.Append("           " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>List any troubleshooting steps taken:</b> ")
            strBody.Append("           " & CommDisruptionStepsTaken & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Planned Outage" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Describe the system(s) that will be impacted:</b> ")
            strBody.Append("           " & PlannedOutageDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Scheduled start date:</b> ")
            strBody.Append("           " & PlannedOutageScheduledStartDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Scheduled start time:</b> ")
            strBody.Append("           " & Left(localTime3, 2) & ":" & Right(localTime3, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Point of contact name/number:</b> ")
            strBody.Append("           " & PlannedOutagecontactNameNumber & "  ")
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

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Drinking Water Facility</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "DWF Report" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Public Water System ID Number:</b> ")
            strBody.Append("           " & PublicWaterSystemIDNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Name of Facility:</b> ")
            strBody.Append("           " & FacilityName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Was there any trespassing, vandalism, or theft?</b> ")
            strBody.Append("           " & TrespassVandalismTheft & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If TrespassVandalismTheft = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Describe what occurred:</b> ")
                strBody.Append("           " & TrespassVandalismTheftText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Damage to the facility or distibution system:</b> ")
            strBody.Append("           " & DamageFacilityDistibutionSystem & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If DamageFacilityDistibutionSystem = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Was it intentional?</b> ")
                strBody.Append("           " & DFDSintentional & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If


            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>ANY access made to the water supply?</b> ")
            strBody.Append("           " & AccessWaterSupply & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Degredation to water quality, system pressure, or water production:</b> ")
            strBody.Append("           " & Degredation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Description of the individual(s) responsible:</b> ")
            strBody.Append("           " & IndividualResponsible & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Local Law Enforcement been contacted:</b> ")
            strBody.Append("           " & LawEnforcementContacted & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If LawEnforcementContacted = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Case number, if known:</b> ")
                strBody.Append("           " & IndividualResponsibleCaseNumber & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

        ElseIf SubType = "Boil Water Advisory" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Public Water System ID Number:</b> ")
            strBody.Append("           " & BWpublicWaterSystemIDNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>This incident was due to a:</b> ")
            strBody.Append("           " & BWIncidentDueTo & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number of customers affected:</b> ")
            strBody.Append("           " & BWnumberCustomersAffected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Affected Areas, including streets or boundaries:</b> ")
            strBody.Append("           " & BWaffectedAreas & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Environmental Crime</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
        'strBody.Append("            <b>Sub-Type:</b> ")
        'strBody.Append("           " & SubType & "  ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
        'strBody.Append("            <b>Situation:</b> ")
        'strBody.Append("           " & Situation & "  ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description of the material(s) involved:</b> ")
        strBody.Append("           " & MaterialDescription & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>How long has the crime been occuring?</b> ")
        strBody.Append("           " & CrimeTimeline & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Describe the individual(s) involved:</b> ")
        strBody.Append("           " & IndividalsDescription & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Describe any vehicles(s) invlolved:</b> ")
        strBody.Append("           " & VehiclesDescription & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Caller contacted county code enforcement:</b> ")
        strBody.Append("           " & CountyCodeEnforcement & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If CountyCodeEnforcement = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Code Enforcement Actions:</b> ")
            strBody.Append("           " & CountyCodeEnforcementText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>FWC Officer to contact caller:</b> ")
        strBody.Append("           " & CallBack & "  ")
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
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Fire</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are there any evacuations?</b> ")
        strBody.Append("           " & Evacuations & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are there Injuries?</b> ")
        strBody.Append("           " & Injury & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If Injury = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and Severity of Injuries:</b> ")
            strBody.Append("           " & InjuryText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If


        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are there Fatalities?</b> ")
        strBody.Append("           " & Fatality & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If Fatality = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and location:</b> ")
            strBody.Append("           " & FatalityText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Any major roadways closed?</b> ")
        strBody.Append("           " & MajorRoadwaysClosed & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Wildfire" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>How many acres is the fire?</b> ")
            strBody.Append("           " & Acres & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Are there any endangerments?</b> ")
            strBody.Append("           " & Endangerment & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Department of Forestry been notified:</b> ")
            strBody.Append("           " & DOFNotified & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>DOF Fire Name:</b> ")
            strBody.Append("           " & DOFFireName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>DOF Fire Number:</b> ")
            strBody.Append("           " & DOFFireNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Other assistance requested:</b> ")
            strBody.Append("           " & OtherAssistanceRequested & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Other" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Any other structures threatened:</b> ")
            strBody.Append("           " & StructuresThreatened & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If StructuresThreatened = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Structures Threatened Text:</b> ")
                strBody.Append("           " & StructuresThreatenedText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Hazardous materials inside the structure:</b> ")
            strBody.Append("           " & HazardousMaterials & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Cause of the fire, if known:</b> ")
            strBody.Append("           " & Cause & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Incident Severity:</b> ")
            strBody.Append("           " & IndicentSeverity & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>General</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "General Incident" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Describe the incident:</b> ")
            strBody.Append("           " & GeneralDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>What specific hazards exist?</b> ")
            strBody.Append("           " & SpecificHazards & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Remedial actions are planned or occuring:</b> ")
            strBody.Append("           " & RemedialActionsPlannedOccuring & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Local/County EOC Activation" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Level of Activation:</b> ")
            strBody.Append("           " & ActivationLevel & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Incident(s) or hazards(s) caused the activation:</b> ")
            strBody.Append("           " & CauseOfActivation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>EOC Contact Number:</b> ")
            strBody.Append("           " & EOCContactNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>EOC Contact E-Mail:</b> ")
            strBody.Append("           " & EOCContactEMail & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Hours operation/operational periods & staffing:</b> ")
            strBody.Append("           " & HoursOperationalPeriodsStaffing & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        End If

    End Sub

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
        objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Geological Event</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Earthquake" Or SubType = "Aftershock" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Magnitude:</b> ")
            strBody.Append("           " & EaMagnitude & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Location:</b> ")
            strBody.Append("           " & EaLocation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Depth:</b> ")
            strBody.Append("           " & EaDepth & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Subsidence or Sinkhole" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Diameter/Length/Width of the area that subsided:</b> ")
            strBody.Append("           " & SsSize & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Any structures threatened or damaged?</b> ")
            strBody.Append("           " & SsStructuresThreatenedDamaged & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If SsStructuresThreatenedDamaged = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Description:</b> ")
                strBody.Append("           " & SsStructuresThreatenedDamagedText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Any structures threatened or damaged?</b> ")
            strBody.Append("           " & SsRoadwayThreatDamagedClosed & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are there any evacuations?</b> ")
        strBody.Append("           " & Evacuations & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are any major roadways closed?</b> ")
        strBody.Append("           " & MajorRoadwaysClosed & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are there Injuries?</b> ")
        strBody.Append("           " & Injury & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If Injury = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and Severity of Injuries:</b> ")
            strBody.Append("           " & InjuryText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are there Fatalities?</b> ")
        strBody.Append("           " & Fatality & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If Fatality = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and location:</b> ")
            strBody.Append("           " & FatalityText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Anticipated need for state assistance:</b> ")
        strBody.Append("           " & StateAssistance & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If StateAssistance = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Description of anticipated need(s):</b> ")
            strBody.Append("           " & StateAssistanceText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
        'strBody.Append("            <b>Situation:</b> ")
        'strBody.Append("           " & Situation & "  ")
        'strBody.Append("        </td>")
        'strBody.Append("    </tr>")
        'strBody.Append("</table>")

        'strBody.Append("<table>")
        'strBody.Append("    <tr>")
        'strBody.Append("        <td align='left'width='400px'>")
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
        Dim Injury As String = ""
        Dim InjuryText As String = ""
        Dim Fatality As String = ""
        Dim FatalityText As String = ""

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
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Kennedy Space Center</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Mission Name:</b> ")
        strBody.Append("           " & MissionName & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Initial Notification" Or SubType = "Rescheduled Launch" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Mission launch date:</b> ")
            strBody.Append("           " & InrlMissionLaunchDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Launch Window Start:</b> ")
            strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Launch Window End:</b> ")
            strBody.Append("           " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Brevard Co. Fire Rescue Staff report to KSC Morrell Operations Center:</b> ")
            strBody.Append("           " & InrlBrevardCo & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Brevard Co. EOC Activation to Level 2 no later than:</b> ")
            strBody.Append("           " & InrlBrevardCo2 & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Next launch notification date:</b> ")
            strBody.Append("           " & NextMissionLaunchDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Scrubbed Launch" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Mission scrubbed date:</b> ")
            strBody.Append("           " & ScrubDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Mission scrubbed time:</b> ")
            strBody.Append("           " & Left(localTime3, 2) & ":" & Right(localTime3, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Reason:</b> ")
            strBody.Append("           " & ScrubReason & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Next launch notification date/time:</b> ")
            strBody.Append("           " & ScrubNextLaunchDateTime & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Successful Launch" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Launch date:</b> ")
            strBody.Append("           " & SuccessDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Launch time:</b> ")
            strBody.Append("           " & Left(localTime4, 2) & ":" & Right(localTime4, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Unsuccessful Launch" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Launch date:</b> ")
            strBody.Append("           " & UnsuccessDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Launch time:</b> ")
            strBody.Append("           " & Left(localTime5, 2) & ":" & Right(localTime5, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Reason, if known:</b> ")
            strBody.Append("           " & UnsuccessReason & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Is there any off-site impact?</b> ")
            strBody.Append("           " & UnsuccessOffSiteImpact & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If UnsuccessOffSiteImpact = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Describe area and hazards:</b> ")
                strBody.Append("           " & UnsuccessOffSiteImpactText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Are there Injuries?</b> ")
            strBody.Append("           " & Injury & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Injury = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Number and Severity of Injuries:</b> ")
                strBody.Append("           " & InjuryText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Fatalities:</b> ")
            strBody.Append("           " & Fatality & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Fatality = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Number and location:</b> ")
                strBody.Append("           " & FatalityText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

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
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
            Fatality = HelpFunction.Convertdbnulls(objDR2("Fatality"))
            FatalityText = HelpFunction.Convertdbnulls(objDR2("FatalityText"))
            HazardousMaterialsOnboard = HelpFunction.Convertdbnulls(objDR2("HazardousMaterialsOnboard"))
            FuelPetroleumSpills = HelpFunction.Convertdbnulls(objDR2("FuelPetroleumSpills"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Marine Incident</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Vessel Name:</b> ")
        strBody.Append("           " & VesselName & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Vessel Type:</b> ")
        strBody.Append("           " & VesselType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Hull Length:</b> ")
        strBody.Append("           " & HullLength & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Flag:</b> ")
        strBody.Append("           " & Flag & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Registration Number:</b> ")
        strBody.Append("           " & RegistrationNumber & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Owned/Operated By:</b> ")
        strBody.Append("           " & OwnedOperatedBy & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Number of People Onboard (passengers/crew):</b> ")
        strBody.Append("           " & NumberPeopleOnboard & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Cause the incident (if known):</b> ")
        strBody.Append("           " & IncidentCause & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Fire:</b> ")
        strBody.Append("           " & Fire & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Injuries:</b> ")
        strBody.Append("           " & Injury & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If Injury = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and Severity of Injuries:</b> ")
            strBody.Append("           " & InjuryText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Fatalities:</b> ")
        strBody.Append("           " & Fatality & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If Fatality = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and location:</b> ")
            strBody.Append("           " & FatalityText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Hazardous materials onboard:</b> ")
        strBody.Append("           " & HazardousMaterialsOnboard & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Fuel or Petroleum Spills:</b> ")
        strBody.Append("           " & FuelPetroleumSpills & "  ")
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
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Migration</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>The migrants:</b> ")
        strBody.Append("           " & Migrants & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>How many vessels?</b> ")
        strBody.Append("           " & VesselNumber & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>How many migrants? (Men/Women/Children)?</b> ")
        strBody.Append("           " & MigrantNumber & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Citizenship or ethnicity of the migrant(s):</b> ")
        strBody.Append("           " & CitizenshipEthnicity & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Migrants been quarantined:</b> ")
        strBody.Append("           " & MigrantQuarantined & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If MigrantQuarantined = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Location of Quarantined Migrants:</b> ")
            strBody.Append("           " & MigrantQuarantinedText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Injuries:</b> ")
        strBody.Append("           " & Injury & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If Injury = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and Severity of Injuries:</b> ")
            strBody.Append("           " & InjuryText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Are there Fatalities:</b> ")
        strBody.Append("           " & Fatality & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If Fatality = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and location:</b> ")
            strBody.Append("           " & FatalityText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>The migrants:</b> ")
        strBody.Append("           " & ImmigrationNotified & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Facility are the migrants being held at:</b> ")
        strBody.Append("           " & Facility & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Select Severity Level:</b> ")
        strBody.Append("           " & SeverityLevel & "  ")
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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Military Activity</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Tomahawk Missile Launch" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Type of report:</b> ")
            strBody.Append("           " & ReportType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Launch date:</b> ")
            strBody.Append("           " & LaunchDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Launch time:</b> ")
            strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Launch message:</b> ")
            strBody.Append("           " & LaunchMessage & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Flight path:</b> ")
            strBody.Append("           " & FlightPath & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        Else

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Unit conducting activity:</b> ")
            strBody.Append("           " & UnitConductingActivity & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Describe the activity:</b> ")
            strBody.Append("           " & ActivityDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Time/date range of activity:</b> ")
            strBody.Append("           " & ActivityTimeDateRange & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>List any airspace restrictions:</b> ")
            strBody.Append("           " & AirspaceRestrictions & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>List any road closures:</b> ")
            strBody.Append("           " & RoadClosures & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Point of Contact Name:</b> ")
            strBody.Append("           " & ContactName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Point of Contact Number:</b> ")
            strBody.Append("           " & ContactNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        End If

    End Sub

    'Added To BB Report As of 05-20-11
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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Nuclear Power Plant</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Crystal River – Full ENF" Or SubType = "Saint Lucie" Or SubType = "Turkey Point" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("           <b> " & CSTselectOne & " </b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>2 A. Date:</b> ")
            strBody.Append("           " & CSTdate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>2 B. Contact Time:</b> ")
            strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>2 C. Reported By (Name):</b> ")
            strBody.Append("           " & CSTreportedByName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>2 D. Message Number:</b> ")
            strBody.Append("           " & CSTmessageNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>2 E. Reported From:</b> ")
            strBody.Append("           " & CSTreportedFrom & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>2 F.:</b> ")
            strBody.Append("           " & CSTfSelectOne & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>3. Site:</b> ")
            strBody.Append("           " & CSTsite & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>4. Emergency Classification:</b> ")
            strBody.Append("           " & CSTemergencyClassification & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>5.</b> ")
            strBody.Append("           " & CSTdecTermSelectOne & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>5. Date:</b> ")
            strBody.Append("           " & CSTdecTermDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>5. Time:</b> ")
            strBody.Append("           " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>6. Reason for Emergency Declaration:</b> ")
            strBody.Append("           " & CSTdecTermSelectOne & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>6. EAL Number(s):</b> ")
            strBody.Append("           " & CSTeALNumbers & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>6. Description:</b> ")
            strBody.Append("           " & CSTeALDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>7. Additional Information:</b> ")
            strBody.Append("           " & CSTeALai & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>7. Description</b> ")
            strBody.Append("           " & CSTeALaiDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>8. Weather Data &nbsp; 8. A. Wind direction from degrees: :</b> ")
            strBody.Append("           " & CSTwindDirectionDegrees & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>8. B. Downwind Sectors Affected:</b> ")
            strBody.Append("           " & CSTdownwindSectorsAffected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>9. Release Status:</b> ")
            strBody.Append("           " & CSTreleaseStatus & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>10. Release Significance at Site Boundary:</b> ")
            strBody.Append("           " & CSTsigCatSiteBoundary & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>11. Utility Recommended Protective Actions:</b> ")
            strBody.Append("           " & CSTutilRecProtAct & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If CSTutilRecProtAct <> "A. No utility recommended actions at this time." Then

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Evacuate Zones:</b> ")
                strBody.Append("           " & CSTevacuateZones & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Shelter Zones:</b> ")
                strBody.Append("           " & CSTshelterZones & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Evacuate Sectors 0-2 Miles:</b> ")
                strBody.Append("           " & CST02MilesEvacSect & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Evacuate Sectors 2-5 Miles:</b> ")
                strBody.Append("           " & CST25MilesEvacSect & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Evacuate Sectors 5-10 Miles:</b> ")
                strBody.Append("           " & CST510MilesEvacSect & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Miles Shelter Sectors 0-2 Miles:</b> ")
                strBody.Append("           " & CST02MilesShelterSect & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Miles Shelter Sectors 2-5 Miles:</b> ")
                strBody.Append("           " & CST25MilesShelterSect & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Miles Shelter Sectors 5-10 Miles:</b> ")
                strBody.Append("           " & CST510MilesShelterSect & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Miles No Action Sectors 0-2 Miles:</b> ")
                strBody.Append("           " & CST02MilesNoActtionSect & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Miles No Action Sectors 2-5 Miles:</b> ")
                strBody.Append("           " & CST25MilesNoActtionSect & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Miles No Action Sectors 5-10 Miles:</b> ")
                strBody.Append("           " & CST510MilesNoActtionSect & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

            End If

            If CSTreportedFrom <> "Control Room" Then

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>12. Plant Conditions  12. A. Reactor Shutdown:</b> ")
                strBody.Append("           " & CST12A & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>12. B. Core Adequately Cooled:</b> ")
                strBody.Append("           " & CST12B & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>12. C. Containment Intact:</b> ")
                strBody.Append("           " & CST12C & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>12. D. Core Condition:</b> ")
                strBody.Append("           " & CST12D & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>13. Weather Data  13. A. Wind Speed (MPH):</b> ")
                strBody.Append("           " & CST13A & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>13. B. Stability Class:</b> ")
                strBody.Append("           " & CST13B & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>14 A. Additoinal Release Information:</b> ")
                strBody.Append("           " & CST14A & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                If CST14A = "As Below" Then

                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='400px'>")
                    strBody.Append("            <b>Distance:</b> ")
                    strBody.Append("           Projected Thyroid Dose (CDE) for " & CSTProjThyroidDose & " hour(s), &nbsp;")
                    strBody.Append("           Projected Total Dose (TEDE) for " & CSTProjTotalDose & " hour(s)  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")

                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='400px'>")
                    strBody.Append("            <b>1 Mile (Site Boundary):</b> ")
                    strBody.Append("            B. " & CST14B & " mrem &nbsp;")
                    strBody.Append("            C. " & CST14C & " mrem ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")

                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='400px'>")
                    strBody.Append("            <b>2 Miles:</b> ")
                    strBody.Append("            D. " & CST14D & " mrem &nbsp;")
                    strBody.Append("            E. " & CST14E & " mrem ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")

                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='400px'>")
                    strBody.Append("            <b>5 Miles:</b> ")
                    strBody.Append("            F. " & CST14F & " mrem &nbsp;")
                    strBody.Append("            G. " & CST14G & " mrem ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")

                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='400px'>")
                    strBody.Append("            <b>10 Miles:</b> ")
                    strBody.Append("            H. " & CST14H & " mrem &nbsp;")
                    strBody.Append("            I. " & CST14I & " mrem ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
                    strBody.Append("</table>")



                End If

            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>15. Message Received By:(Name):</b> ")
            strBody.Append("           " & CST15Name & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Date:</b> ")
            strBody.Append("           " & CST15Date & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Time:</b> ")
            strBody.Append("           " & Left(localTime3, 2) & ":" & Right(localTime3, 2) & " &nbsp;ET  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>SWO User Comments:</b> ")
            strBody.Append("           " & CSTuserComments & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Farley" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>1." & Far1SelectOne & "</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Message #:</b> ")
            strBody.Append("           " & Far1MessageNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>2." & Far2SelectOne & " </b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Notification Time:</b> ")
            strBody.Append("           " & Left(localTime4, 2) & ":" & Right(localTime4, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Date:</b> ")
            strBody.Append("           " & Far2NotificationDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Authentication #:</b> ")
            strBody.Append("           " & Far2AuthenticationNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Site:</b> ")
            strBody.Append("           " & Far3Site & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Confirmation Phone #:</b> ")
            strBody.Append("           " & Far3ConfirmationPhoneNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>4. Emergency Classification:</b> ")
            strBody.Append("           " & Far4EmergencyClassification & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Based on EAL #:</b> ")
            strBody.Append("           " & Far4BasedEALnumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>EAL Description:</b> ")
            strBody.Append("           " & Far4EALdescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>5. Protective Action Recommendations</b>: ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Far5a = True Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <input type='checkbox' name='1' checked='checked' /> 5 A. None ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            ElseIf Far5a = False Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <input type='checkbox' name='1' /> 5 A. None ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far5b = True Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <input type='checkbox' name='2' checked='checked' /> 5. B. Evacuate ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            ElseIf Far5b = False Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <td align='left'> <input type='checkbox' name='2' /> 5. B. Evacuate </td> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>5. B. Evacuate Description:</b> ")
            strBody.Append("           " & Far5bText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Far5c = True Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <input type='checkbox' name='3' checked='checked' /> 5. C. Evacuate ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                strBody.Append("<td align='left'>  </td>")
            ElseIf Far5c = False Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <input type='checkbox' name='3' /> 5. C. Shelter ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
                strBody.Append("<td align='left'>  </td>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>5. C. Shelter Description:</b> ")
            strBody.Append("           " & Far5cText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Far5d = True Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <input type='checkbox' name='4' checked='checked' /> 5. D. Consider the use of KI in accordance with state plans and policy. ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            ElseIf Far5d = False Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <input type='checkbox' name='4' /> 5. D. Consider the use of KI in accordance with state plans and policy. ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far5e = True Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <input type='checkbox' name='5' checked='checked' /> 5. E. Other ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            ElseIf Far5e = False Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <input type='checkbox' name='5' /> 5. E. Other </td> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>5. E. Other Description:</b> ")
            strBody.Append("           " & Far5eText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>6.Emergency Release:</b> ")
            strBody.Append("           " & Far6EmergencyRelease & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>7. Release Significance:</b> ")
            strBody.Append("           " & Far7ReleaseSignificance & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>8. Event Prognosis:</b> ")
            strBody.Append("           " & Far8EventPrognosis & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>9. Meterological Data:</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("          Wind direction from " & Far9WindDirectDegrees & " degrees  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("           Wind direction from " & Far9WindDirectDegrees & " degrees  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("           Precipitation = " & Far9Precipitation & " ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("           Wind Speed " & Far9WindSpeed & " (mph) ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Stability Class:</b> ")
            strBody.Append("           " & Far9StabilityClass & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>10. " & Far10Select1 & "</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>10 Time:</b> ")
            strBody.Append("           " & Left(localTime5, 2) & ":" & Right(localTime5, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>10 Date:</b> ")
            strBody.Append("           " & Far10Date & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>11. Affected Units:</b> ")
            strBody.Append("           " & Far11AffectedUnits & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>12. Unit Status:</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            12. A. Unit 1 " & Far12AUnitPower & " % power ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Time = " & Left(localTime6, 2) & ":" & Right(localTime6, 2) & " ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Date = " & Far12ADate & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            (Unaffected Unit(s) Status Not Required for Initial Notifications) ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            12. B. Unit 2 " & Far12BUnitPower & " % power ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Time = " & Left(localTime7, 2) & ":" & Right(localTime7, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Date = " & Far12BDate & " ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>13. Remarks:</b> ")
            strBody.Append("           " & Far13Remarks & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Information(Lines 14-16 not required for initial Notifications)</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Information(Lines 14-16 not required for initial Notifications)</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>14. Release Characterization: " & Far14ReleaseChar & "</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Units = ")
            strBody.Append("           " & Far14Units & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Magnitude:</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Noble Gasses = ")
            strBody.Append("           " & Far14NobleGasses & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Iodines = ")
            strBody.Append("           " & Far14Iodines & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Particulautes = ")
            strBody.Append("           " & Far14Particulautes & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Other = ")
            strBody.Append("           " & Far14Other & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Far14Aairborne = True Or Far14Bliquid = True Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Form:</b> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Far14Aairborne = True Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <input type='checkbox' name='6' checked='checked' /> A. Airborne: ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            Start Time = ")
                strBody.Append("           " & Left(localTime8, 2) & ":" & Right(localTime8, 2) & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            Date = ")
                strBody.Append("           " & Far14AstartDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            Stop Time = ")
                strBody.Append("           " & Left(localTime9, 2) & ":" & Right(localTime9, 2) & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
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
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <input type='checkbox' name='7' checked='checked' /> B. Liquid: ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            Start Time = ")
                strBody.Append("           " & Left(localTime10, 2) & ":" & Right(localTime10, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            Date = ")
                strBody.Append("           " & Far14BstartDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            Stop Time = ")
                strBody.Append("           " & Left(localTime11, 2) & ":" & Right(localTime11, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            Date = ")
                strBody.Append("           " & Far14BendDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                'strBody.Append("<td align='left'> <input type='checkbox' name='7' /> B. Liquid: </td>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>15. Projection Parameters:</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Projection Period = ")
            strBody.Append("           " & Far15ProjectionPeriod & " (hours) ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Estimated Release Duration = ")
            strBody.Append("           " & Far15EstimatedReleaseDuration & " (hours) ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Projection Performed:</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Time = ")
            strBody.Append("           " & Left(localTime12, 2) & ":" & Right(localTime12, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Date = ")
            strBody.Append("           " & Far15ProjectionPerformedDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Accident Type = ")
            strBody.Append("           " & Far15AccidentType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>16. Projected Dose:</b> ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Site boundary:</b> ")
            strBody.Append("          <b>TEDE(mrem)</b> = " & Far16SiteBoundaryTEDE & " <b>Adult Thyroid CDE(mrem)</b> = " & Far16SiteBoundaryAdultThyroidCDE & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("          <b>TEDE(mrem)</b> = " & Far16TwoMilesTEDE & " <b>Adult Thyroid CDE(mrem)</b> = " & Far16TwoMilesAdultThyroidCDE & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("          <b>TEDE(mrem)</b> = " & Far16FiveMilesTEDE & " <b>Adult Thyroid CDE(mrem)</b> = " & Far16FiveMilesAdultThyroidCDE & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Situation:</b> ")
            strBody.Append("          <b>TEDE(mrem)</b> = " & Far16TenMilesTEDE & " <b>Adult Thyroid CDE(mrem)</b> = " & Far16MilesAdultThyroidCDE & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>17. Approved By:</b> ")
            strBody.Append("           " & Far17ApprovedBy & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Title = ")
            strBody.Append("           " & Far17Title & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Time = ")
            strBody.Append("           " & Left(localTime13, 2) & ":" & Right(localTime13, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Date = ")
            strBody.Append("           " & Far17Date & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Notified By:</b> ")
            strBody.Append("           " & Far17NotifiedBy & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Received By = ")
            strBody.Append("           " & Far17ReceivedBy & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Time = ")
            strBody.Append("           " & Left(localTime14, 2) & ":" & Right(localTime14, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            Date = ")
            strBody.Append("           " & Far17ReceivedDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

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
        Dim CallbackDEPRequested As String = ""
        Dim CallbackDEPRequestedValue As String = ""

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
            CallbackDEPRequested = HelpFunction.Convertdbnulls(objDR2("CallbackDEPRequested"))
            CallbackDEPRequestedValue = HelpFunction.Convertdbnulls(objDR2("CallbackDEPRequestedValue"))

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Petroleum Spill</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Petroleum Type:</b> ")
        strBody.Append("           " & PetroleumType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Name or Description:</b> ")
        strBody.Append("           " & PetroleumNameDescription & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Odor:</b> ")
        strBody.Append("           " & PetroleumOdor & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Color:</b> ")
        strBody.Append("           " & PetroleumColor & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Source / Container:</b> ")
        strBody.Append("           " & PetroleumSourceContainer & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If PetroleumSourceContainer = "Aboveground Pipeline" Or PetroleumSourceContainer = "Underground Pipeline" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Diameter of the Pipeline:</b> ")
            strBody.Append("           " & DiameterPipeline & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Unbroken end of the pipe connected to:</b> ")
            strBody.Append("           " & UnbrokenEndPipeConnectedTo & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Total source/container volume:</b> ")
        strBody.Append("           " & TotalSourceContainerVolume & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")
        
        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Quantity released:</b> ")
        strBody.Append("           " & PetroleumQuantityReleased & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Rate of release:</b> ")
        strBody.Append("           " & PetroleumRateOfRelease & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Released:</b> ")
        strBody.Append("           " & PetroleumlReleased & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Cause of release:</b> ")
        strBody.Append("           " & PetroleumCauseOfRelease & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Time the release was discovered:</b> ")
        strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & " &nbsp;ET ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Time the release was secured:</b> ")
        strBody.Append("           " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & " &nbsp;ET ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Storm drains affected:</b> ")
        strBody.Append("           " & StormDrainsAffected & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Waterways affected:</b> ")
        strBody.Append("           " & WaterwaysAffected & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If WaterwaysAffected = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Name(s) of waterways:</b> ")
            strBody.Append("           " & WaterwaysAffectedText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Major roadways closed:</b> ")
        strBody.Append("           " & MajorRoadwaysClosed & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Cleanup actions been taken:</b> ")
        strBody.Append("           " & CleanupActionsTaken & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If CleanupActionsTaken = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>List cleanup actions:</b> ")
            strBody.Append("           " & CleanupActionsTakenText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Who is conducting cleanup?</b> ")
        strBody.Append("           " & ConductingCleanup & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Callback from DEP requested:</b> ")
        strBody.Append("           " & CallbackDEPRequested & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If CallbackDEPRequested = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Contact:</b> ")
            strBody.Append("           " & CallbackDEPRequestedValue & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Population Protection Actions</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Shelter in place" Or SubType = "Evacuation Order" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Impacted area, including streets or landmarks:</b> ")
            strBody.Append("           " & ImpactedStreetLandmark & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Department/Agency issuing the order:</b> ")
            strBody.Append("           " & DeptAgencyIssuingOrder & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Duration of the order(if known):</b> ")
            strBody.Append("           " & Duration & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number of residences impacted (if known):</b> ")
            strBody.Append("           " & ImpactResidenceNum & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number of businesses impacted (if known):</b> ")
            strBody.Append("           " & ImpactBusinessNum & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Total number of individuals impacted (if known):</b> ")
            strBody.Append("           " & TotalImpacted & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        Else

            'Response.Write("Hello")
            'Response.End()

            objConn2.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            DBConStringHelper.PrepareConnection(objConn2) 'open the connection
            objCmd2 = New SqlCommand("spSelectShelterByIncidentID", objConn2)
            objCmd2.Parameters.AddWithValue("@IncidentID", gStrIncidentID)
            objCmd2.Parameters.AddWithValue("@IncidentIncidentTypeID", strIncidentIncidentTypeID)
            objCmd2.CommandType = CommandType.StoredProcedure
            objDR2 = objCmd2.ExecuteReader()

            If objDR2.Read() Then

                'there are records
                objDR2.Close()
                objDR2 = objCmd2.ExecuteReader()

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <u><b>Shelters Open</b></u> ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                While objDR2.Read
                    strBody.Append("<table>")
                    strBody.Append("    <tr>")
                    strBody.Append("        <td align='left'width='400px'>")
                    strBody.Append("            <b>Shelter Name:</b> ")
                    strBody.Append("           " & objDR2.Item("ShelterName") & "  ")
                    strBody.Append("        </td>")
                    strBody.Append("    </tr>")
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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Public Health Medical</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Infectious Disease Report" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Type of disease, if known:</b> ")
            strBody.Append("           " & IDRdiseaseType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number of people infected:</b> ")
            strBody.Append("           " & IDRpeopleInfectedNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Tests/Examinations that are planned or occuring:</b> ")
            strBody.Append("           " & IDRexamTest & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Quarantine in effect:</b> ")
            strBody.Append("           " & IDRquarantineEffect & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If IDRquarantineEffect = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Describe area, listing streets or landmarks:</b> ")
                strBody.Append("           " & IDRquarantineEffectText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Are there Fatalities:</b> ")
            strBody.Append("           " & IDRfatality & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If IDRfatality = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Number and location:</b> ")
                strBody.Append("           " & IDRfatalityText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Callback from DOH Requested:</b> ")
            strBody.Append("           " & IDRdOHrequested & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If IDRdOHrequested = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Contact:</b> ")
                strBody.Append("           " & IDRdOHrequestedText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

        ElseIf SubType = "Public Health Hazard" Or SubType = "Other" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Describe the hazard:</b> ")
            strBody.Append("           " & PHHOhazardDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Callback from DOH Requested:</b> ")
            strBody.Append("           " & PHHOdOHRequested & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If PHHOdOHRequested = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Contact:</b> ")
                strBody.Append("           " & PHHOdOHRequestedText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

        ElseIf SubType = "Mass Casualty Incident" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number of Patients:</b> ")
            strBody.Append("           " & MCIpatientNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Critical:</b> ")
            strBody.Append("           " & MCIcritical & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Immediate:</b> ")
            strBody.Append("           " & MCIimmediate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Delayed:</b> ")
            strBody.Append("           " & MCIdelayed & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Deceased:</b> ")
            strBody.Append("           " & MCIdeceased & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Location of Triage/Treatment Area(s):</b> ")
            strBody.Append("           " & MCItTA & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Agency coordinating the MCI:</b> ")
            strBody.Append("           " & MCIagencyCoordinating & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Unmet needs:</b> ")
            strBody.Append("           " & MCIunmetNeeds & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If MCIunmetNeeds = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Needs description:</b> ")
                strBody.Append("           " & MCIunmetNeedsText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Callback from DOH Requested:</b> ")
            strBody.Append("           " & MCIdOHRequested & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If MCIdOHRequested = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Contact:</b> ")
                strBody.Append("           " & MCIdOHRequestedText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

        ElseIf SubType = "Impact to Healthcare Facility" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number of Patients Affected:</b> ")
            strBody.Append("           " & IHFpatientsAffectedNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Facility damaged:</b> ")
            strBody.Append("           " & IHFfacilityDamaged & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If IHFfacilityDamaged = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Damage description:</b> ")
                strBody.Append("           " & IHFfacilityDamagedText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Facility being evacuated:</b> ")
            strBody.Append("           " & IHFfacilityEvacuated & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If IHFfacilityEvacuated = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Evacuees being taken to:</b> ")
                strBody.Append("           " & IHFfacilityEvacuatedText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Unmet needs:</b> ")
            strBody.Append("           " & IHFunmetNeeds & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If IHFunmetNeeds = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Unmet needs description:</b> ")
                strBody.Append("           " & IHFunmetNeedsText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Callback from DOH Requested:</b> ")
            strBody.Append("           " & IHFcallbackRequested & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If IHFcallbackRequested = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Contact:</b> ")
                strBody.Append("           " & IHFcallbackRequestedText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
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
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Rail Incident</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Train Type:</b> ")
        strBody.Append("           " & TrainType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Name of company operating train:</b> ")
        strBody.Append("           " & CompanyOperatingTrain & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Train number:</b> ")
        strBody.Append("           " & TrainNumber & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Rail line:</b> ")
        strBody.Append("           " & RailLiine & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Mile post:</b> ")
        strBody.Append("           " & MilePost & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>DOT crossing number (if applicable):</b> ")
        strBody.Append("           " & DotCrossingNumber & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Line owned or operated by:</b> ")
        strBody.Append("           " & LineOwnedOperatedBy & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Number of people onboard (passengers/crew):</b> ")
        strBody.Append("           " & PeopleOnBoard & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Cause the incident (if known):</b> ")
        strBody.Append("           " & IncidentCause & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Is there a derailment:</b> ")
        strBody.Append("           " & Derailment & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Injuries:</b> ")
        strBody.Append("           " & Injury & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If Injury = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and Severity of Injuries:</b> ")
            strBody.Append("           " & InjuryText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Fatalities:</b> ")
        strBody.Append("           " & Fatality & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If Fatality = "Yes" Then
            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number and location:</b> ")
            strBody.Append("           " & FatalityText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")
        End If

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Hazardous materials onboard:</b> ")
        strBody.Append("           " & HazMat & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Hazardous materials released:</b> ")
        strBody.Append("           " & HazMatReleased & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Fuel or Petroleum Spills:</b> ")
        strBody.Append("           " & FuelPetroleumSpills & "  ")
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
            Injury = HelpFunction.Convertdbnulls(objDR2("Injury"))
            InjuryText = HelpFunction.Convertdbnulls(objDR2("InjuryText"))
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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Search & Rescue</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "ELT" Or SubType = "EPIRB" Or SubType = "PLB" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Date mission opened:</b> ")
            strBody.Append("           " & SearchRescueDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Time mission opened:</b> ")
            strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Mission number:</b> ")
            strBody.Append("           " & MissionNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Last coordinates or area description:</b> ")
            strBody.Append("           " & CoordinateAreaDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Registration information:</b> ")
            strBody.Append("           " & RegistrationInformation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>CAP responding:</b> ")
            strBody.Append("           " & CAPResponding & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Missing or overdue aircraft in the area:</b> ")
            strBody.Append("           " & MissingOverdueAircraft & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Date mission closed:</b> ")
            strBody.Append("           " & MissionClosedDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Time mission closed:</b> ")
            strBody.Append("           " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Disposition:</b> ")
            strBody.Append("           " & Disposition & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Structure Collapse" Or SubType = "Industrial Accident" Or SubType = "Transportation Accident" Or SubType = "Other" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Describe the affected struture(s) or facilities(s):</b> ")
            strBody.Append("           " & AffectedStrutureFacility & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Cause of collapse (if known):</b> ")
            strBody.Append("           " & CausedCollapse & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number of people trapped:</b> ")
            strBody.Append("           " & NumberPeopleTrapped & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Injuries:</b> ")
            strBody.Append("           " & Injury & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Injury = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Number and Severity of Injuries:</b> ")
                strBody.Append("           " & InjuryText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Fatalities:</b> ")
            strBody.Append("           " & Fatality & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If Fatality = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Number and location:</b> ")
                strBody.Append("           " & FatalityText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Unmet needs for the rescue operation:</b> ")
            strBody.Append("           " & UnmetNeeds & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If UnmetNeeds = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Needs description:</b> ")
                strBody.Append("           " & UnmetNeedsText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Department/agency coordinating rescue efforts:</b> ")
            strBody.Append("           " & CoordinatingRescueEffort & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "LE Search (Missing Person)" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Description of the individual(s):</b> ")
            strBody.Append("           " & DescriptionIndividual & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Individual(s) were last seen in:</b> ")
            strBody.Append("           " & LastSeen & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Vehicle Description/other relevant information:</b> ")
            strBody.Append("           " & DescriptionVehicleRelevantInformation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Agency handling the investigation:</b> ")
            strBody.Append("           " & AgencyHandlingInvestigation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Security Threat</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType <> "Lockdown" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Description the incident or threat:</b> ")
            strBody.Append("           " & Description & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Description of the individual(s) responsible:</b> ")
            strBody.Append("           " & IndividualResponsibleDescription & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Incident confined to one location:</b> ")
            strBody.Append("           " & ConfinedLocation & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If ConfinedLocation = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Incident confined to one location:</b> ")
                strBody.Append("           " & Location & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If Location = "Other area" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Area(s); specific streets/boundaries:</b> ")
                strBody.Append("           " & ListAreas & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Incident severity:</b> ")
            strBody.Append("           " & IncidentSeverity & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        End If

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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Utility Disruption/Emergency</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Telecommunications Outage" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Communications System:</b> ")
            strBody.Append("           " & TOcommunicationsSystem & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>System operated by:</b> ")
            strBody.Append("           " & TOsystemOperated & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number of Customers affected:</b> ")
            strBody.Append("           " & TOcustomersAffectedNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Is 911 telephone service affected:</b> ")
            strBody.Append("           " & TO911Affected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If TO911Affected = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Describe:</b> ")
                strBody.Append("           " & TO911AffectedText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Damage to the facility or distibution system:</b> ")
            strBody.Append("           " & TOdamageFacilityDistibutionSystem & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If TOdamageFacilityDistibutionSystem = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Intentional:</b> ")
                strBody.Append("           " & TOdamageFacilityDistibutionSystemIntentional & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Description of the individual(s) responsible:</b> ")
                strBody.Append("           " & TOdamageFacilityDistibutionSystemText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

        ElseIf SubType = "Drinking Water Outage" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Water System Name:</b> ")
            strBody.Append("           " & DWOWaterSystemName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Public water system ID #:</b> ")
            strBody.Append("           " & DWOpublicWaterSystemID & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number of customers affected:</b> ")
            strBody.Append("           " & DWOnumberCustomersAffected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Outage a result of any trespassing, theft, vandalism, or a security breach to the distribution system or its facilities:</b> ")
            strBody.Append("           " & DWOoutageResultTTVSBDSF & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Estimated date/time of restoration:</b> ")
            strBody.Append("           " & DWOEstimatedDateTimeRestoration & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Boil water advisory issued:</b> ")
            strBody.Append("           " & DWOboilAdvisory & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Electric Outage" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Electric System:</b> ")
            strBody.Append("           " & EOelectricSystem & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>System operated by:</b> ")
            strBody.Append("           " & EOsystemOperatedBy & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Cause of outage:</b> ")
            strBody.Append("           " & EOwhatCausedOutage & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number of Customers affected:</b> ")
            strBody.Append("           " & EONumberCustomersAffected & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Estimated time to 98% or greater restoration:</b> ")
            strBody.Append("           " & EOestimatedGreaterRestoration & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Damage to the facility or distibution system:</b> ")
            strBody.Append("           " & EOdamageFacilityDistibutionSystem & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If EOdamageFacilityDistibutionSystem = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Intentional:</b> ")
                strBody.Append("           " & EOdamageFacilityDistibutionSystemIntentional & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Description of the individual(s) responsible:</b> ")
                strBody.Append("           " & EOdamageFacilityDistibutionSystemResposible & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            'strBody.Append("<table>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td align='left'width='400px'>")
            'strBody.Append("            <b>Type of Advisory:</b> ")
            'strBody.Append("           " & GCAadvisoryType & "  ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")

            'strBody.Append("<table>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td align='left'width='400px'>")
            'strBody.Append("            <b>Advisory due to a fuel supply shortage:</b> ")
            'strBody.Append("           " & GCAsupplyShortage & "  ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")

            'strBody.Append("<table>")
            'strBody.Append("    <tr>")
            'strBody.Append("        <td align='left'width='400px'>")
            'strBody.Append("            <b>Text of the Advisory:</b> ")
            'strBody.Append("           " & GCAadvisory & "  ")
            'strBody.Append("        </td>")
            'strBody.Append("    </tr>")
            'strBody.Append("</table>")

        ElseIf SubType = "Natural Gas Outage" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Natural Gas System:</b> ")
            strBody.Append("           " & NGOsystem & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>System operated by:</b> ")
            strBody.Append("           " & NGOsystemOperatedBy & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Cause the outage:</b> ")
            strBody.Append("           " & NGOoutageCause & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Number of Customers affected:</b> ")
            strBody.Append("           " & NGOCustomersAffectedNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Estimated time restoration:</b> ")
            strBody.Append("           " & NGOestimatedTimeRestoration & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Damage to the facility or distibution system:</b> ")
            strBody.Append("           " & NGOdFDS & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If NGOdFDS = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Intentional:</b> ")
                strBody.Append("           " & NGOdFDSintentional & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Description of the individual(s) responsible:</b> ")
                strBody.Append("           " & NGOdFDSdescription & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

        ElseIf SubType = "Electric Generating Capacity Advisory" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Type of Advisory:</b> ")
            strBody.Append("           " & GCAadvisoryType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Advisory due to a fuel supply shortage:</b> ")
            strBody.Append("           " & GCAsupplyShortage & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Text of the Advisory:</b> ")
            strBody.Append("           " & GCAadvisory & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

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

        End If

        objDR2.Close()

        objCmd2.Dispose()
        objCmd2 = Nothing

        objConn2.Close()

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Wastewater or Effluent</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Wastewater" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Public Water System ID or Permit Number:</b> ")
            strBody.Append("           " & WWsystemIDPermitNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Name of System:</b> ")
            strBody.Append("           " & WWsystemName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Type of System: </b> ")
            strBody.Append("           " & WWsystemType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Type of wastewater:</b> ")
            strBody.Append("           " & WWreleaseOccurred & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Release occurred from a:</b> ")
            strBody.Append("           " & WWtype & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Release Cause:</b> ")
            strBody.Append("           " & WWreleaseCause & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Release status:</b> ")
            strBody.Append("           " & WWreleaseStatus & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If WWreleaseStatus = "Ceased" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Date release ceased:</b> ")
                strBody.Append("           " & WWceasedDate & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Time release ceased:</b> ")
                strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & " &nbsp;ET ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Release contained on-site at a water reclamation facility:</b> ")
            strBody.Append("           " & WWreleasedContainedonSite & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Amount of release, in gallons:</b> ")
            strBody.Append("           " & WWreleaseAmount & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Release enter a storm water system:</b> ")
            strBody.Append("           " & WWstormWater & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If WWstormWater = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Location of storm drain(s) that were impacted:</b> ")
                strBody.Append("           " & WWstormWaterLocation & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")

                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Storm drain discharges:</b> ")
                strBody.Append("           " & WWstormWaterDischarge & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Release enter any surface waters:</b> ")
            strBody.Append("           " & WWsurfaceWaterDDL & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If WWsurfaceWaterDDL = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Type of surface water::</b> ")
                strBody.Append("           " & WWsurfaceWater & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            If WWsurfaceWater = "Retention Pond, contained." Or WWsurfaceWater = "Retention pond, drained to waterway." Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Names of waterway(s):</b> ")
                strBody.Append("           " & WWwaterway & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Affected waterway a source of drinking water:</b> ")
            strBody.Append("           " & WWconfirmedContamination & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Status of Cleanup Actions:</b> ")
            strBody.Append("           " & WWcleanupActions & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Describe clean-up actions:</b> ")
            strBody.Append("           " & WWcleanupActionsText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Treated Effluent" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Public Water System ID or Permit Number:</b> ")
            strBody.Append("           " & TEsystemIDPermitNumber & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Name of System:</b> ")
            strBody.Append("           " & TEsystemName & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Caused of release:</b> ")
            strBody.Append("           " & TEreleaseCause & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Amount of release, in gallons:</b> ")
            strBody.Append("           " & TEgallonsReleased & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Cleanup actions needed:</b> ")
            strBody.Append("           " & TEcleanupActions & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If TEcleanupActions = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Describe cleanup actions:</b> ")
                strBody.Append("           " & TEcleanupActionsText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

        End If

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

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td  width='400px' align='left' style='background-color:#d4d4d4; color:#000000;' >")
        strBody.Append("            <b>Weather Advisories and Reports</b>")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Sub-Type:</b> ")
        strBody.Append("           " & SubType & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Situation:</b> ")
        strBody.Append("           " & Situation & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        strBody.Append("<table>")
        strBody.Append("    <tr>")
        strBody.Append("        <td align='left'width='400px'>")
        strBody.Append("            <b>Description:</b> ")
        strBody.Append("           " & MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", strIncidentIncidentTypeID) & "  ")
        strBody.Append("        </td>")
        strBody.Append("    </tr>")
        strBody.Append("</table>")

        If SubType = "Weather Watch" Or SubType = "Weather Warning" Or SubType = "Weather Advisory" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Date Issued:</b> ")
            strBody.Append("           " & WWAdateIssued & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Time Issued:</b> ")
            strBody.Append("           " & Left(localTime, 2) & ":" & Right(localTime, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Effective on Date:</b> ")
            strBody.Append("           " & WWAeffectiveDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Effective on Time:</b> ")
            strBody.Append("           " & WWAeffectiveTime & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Expires on Date:</b> ")
            strBody.Append("           " & WWAexpiresDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Expires on Time:</b> ")
            strBody.Append("           " & WWAexpiresTime & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Issuing Office:</b> ")
            strBody.Append("           " & WWAissuingOffice & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Type of Advisory:</b> ")
            strBody.Append("           " & WWAadvisoryType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Advisory Text:</b> ")
            strBody.Append("           " & WWAadvisoryText & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        ElseIf SubType = "Local Storm Report" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Type of Report:</b> ")
            strBody.Append("           " & LSRreportType & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Report was received:</b> ")
            strBody.Append("           " & LSRreportReceived & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Injuries:</b> ")
            strBody.Append("           " & LSRInjury & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If LSRInjury = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Number and Severity of Injuries:</b> ")
                strBody.Append("           " & LSRInjuryText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Fatalities:</b> ")
            strBody.Append("           " & LSRFatality & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If LSRFatality = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Number and location:</b> ")
                strBody.Append("           " & LSRFatalityText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Displacements:</b> ")
            strBody.Append("           " & LSRdisplacement & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If LSRdisplacement = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Number and where are they being sheltered:</b> ")
                strBody.Append("           " & LSRdisplacementText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Damage to structures:</b> ")
            strBody.Append("           " & LSRdamageStructures & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If LSRdamageStructures = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Type of Structures/Number/Severity:</b> ")
                strBody.Append("           " & LSRdamageStructuresText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Damage to Infrastructure:</b> ")
            strBody.Append("           " & LSRinfrastructureDamage & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            If LSRinfrastructureDamage = "Yes" Then
                strBody.Append("<table>")
                strBody.Append("    <tr>")
                strBody.Append("        <td align='left'width='400px'>")
                strBody.Append("            <b>Description:</b> ")
                strBody.Append("           " & LSRinfrastructureDamageText & "  ")
                strBody.Append("        </td>")
                strBody.Append("    </tr>")
                strBody.Append("</table>")
            End If

        ElseIf SubType = "NOAA Transnsmitter Outage" Then

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Transmitter(s):</b> ")
            strBody.Append("           " & TOtransmitter & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Weather Forecast Office making notification:</b> ")
            strBody.Append("           " & TOmakingNotification & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Date Out of Service:</b> ")
            strBody.Append("           " & TOserviceOutDate & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Time Out of Service:</b> ")
            strBody.Append("           " & Left(localTime2, 2) & ":" & Right(localTime2, 2) & " &nbsp;ET ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Transmitter is out of service due to:</b> ")
            strBody.Append("           " & TOtransmitterServiceDueTo & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

            strBody.Append("<table>")
            strBody.Append("    <tr>")
            strBody.Append("        <td align='left'width='400px'>")
            strBody.Append("            <b>Time the transmitter(s) are expected to return to service:</b> ")
            strBody.Append("           " & TOreturnToService & "  ")
            strBody.Append("        </td>")
            strBody.Append("    </tr>")
            strBody.Append("</table>")

        End If

    End Sub


End Class

