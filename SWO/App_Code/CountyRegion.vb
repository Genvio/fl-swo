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

Public Class CountyRegion

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

    Dim ParamId As SqlParameter

    Public MrDataGrabber As New DataGrabber


    'Global Object Variables

    'IncidentID
    Public gStrIncidentID As String = ""

    Public gStrRegionCoordinatorEmail As String = ""

    Public gStrRegionCoordinator As String = ""

    Public gStrCountyCoordinatorEmail As String = ""

    Public gStrCountyCoordinator As String = ""

    'True/False County Check
    Public gBoolAlachua As Boolean = False
    Public gBoolBaker As Boolean = False
    Public gBoolBay As Boolean = False
    Public gBoolBradford As Boolean = False
    Public gBoolBrevard As Boolean = False
    Public gBoolBroward As Boolean = False
    Public gBoolCalhoun As Boolean = False
    Public gBoolCharlotte As Boolean = False
    Public gBoolCitrus As Boolean = False
    Public gBoolClay As Boolean = False
    Public gBoolCollier As Boolean = False
    Public gBoolColumbia As Boolean = False
    Public gBoolDeSoto As Boolean = False
    Public gBoolDixie As Boolean = False
    Public gBoolDuval As Boolean = False
    Public gBoolEscambia As Boolean = False
    Public gBoolFlagler As Boolean = False
    Public gBoolFranklin As Boolean = False
    Public gBoolGadsden As Boolean = False
    Public gBoolGilchrist As Boolean = False
    Public gBoolGlades As Boolean = False
    Public gBoolGulf As Boolean = False
    Public gBoolHamilton As Boolean = False
    Public gBoolHardee As Boolean = False
    Public gBoolHendry As Boolean = False
    Public gBoolHernando As Boolean = False
    Public gBoolHighlands As Boolean = False
    Public gBoolHillsborough As Boolean = False
    Public gBoolHolmes As Boolean = False
    Public gBoolIndianRiver As Boolean = False
    Public gBoolJackson As Boolean = False
    Public gBoolJefferson As Boolean = False
    Public gBoolLafayette As Boolean = False
    Public gBoolLake As Boolean = False
    Public gBoolLee As Boolean = False
    Public gBoolLeon As Boolean = False
    Public gBoolLevy As Boolean = False
    Public gBoolLiberty As Boolean = False
    Public gBoolMadison As Boolean = False
    Public gBoolManatee As Boolean = False
    Public gBoolMarion As Boolean = False
    Public gBoolMartin As Boolean = False
    Public gBoolMiamiDade As Boolean = False
    Public gBoolMonroe As Boolean = False
    Public gBoolNassau As Boolean = False
    Public gBoolOkaloosa As Boolean = False
    Public gBoolOkeechobee As Boolean = False
    Public gBoolOrange As Boolean = False
    Public gBoolOsceola As Boolean = False
    Public gBoolPalmBeach As Boolean = False
    Public gBoolPasco As Boolean = False
    Public gBoolPinellas As Boolean = False
    Public gBoolPolk As Boolean = False
    Public gBoolPutnam As Boolean = False
    Public gBoolSantaRosa As Boolean = False
    Public gBoolSarasota As Boolean = False
    Public gBoolSeminole As Boolean = False
    Public gBoolStJohns As Boolean = False
    Public gBoolStLucie As Boolean = False
    Public gBoolSumter As Boolean = False
    Public gBoolSuwannee As Boolean = False
    Public gBoolTaylor As Boolean = False
    Public gBoolUnion As Boolean = False
    Public gBoolVolusia As Boolean = False
    Public gBoolWakulla As Boolean = False
    Public gBoolWalton As Boolean = False
    Public gBoolWashington As Boolean = False

    'True/False Region Check
    Public gBoolRegion1 As Boolean = False
    Public gBoolRegion2 As Boolean = False
    Public gBoolRegion3 As Boolean = False
    Public gBoolRegion4 As Boolean = False
    Public gBoolRegion5 As Boolean = False
    Public gBoolRegion6 As Boolean = False
    Public gBoolRegion7 As Boolean = False

    'True/False Region Affected Check
    Public gBoolRegion1Affected As Boolean = False
    Public gBoolRegion2Affected As Boolean = False
    Public gBoolRegion3Affected As Boolean = False
    Public gBoolRegion4Affected As Boolean = False
    Public gBoolRegion5Affected As Boolean = False
    Public gBoolRegion6Affected As Boolean = False
    Public gBoolRegion7Affected As Boolean = False

    'True/False Statewide Check
    Public gBoolStateWide As Boolean = False

    'This will displays the actual "Full" Region, This means all Counties in the region are involved with the
    'the Incident. Example: Region 7 will only be a part of the string if Broward,Miami-Dade,Monroe,  
    'Palm Beach, and Sarasota are involved with the Incident
    Public gStrRegions As String = ""

    'This will displays the "Partial" Region, This means at least 1 County in the region is involved with the 
    'Incident. Example: Region 7 will be a part of the string if at least one of these counties: Broward,  
    'Miami-Dade,Monroe,Palm Beach,or Sarasota is involved with the Incident
    Public gStrRegionsAffected As String = ""

    'Will retrieve ALL Counties involved with Incident
    Public gStrCountiesAffected As String = ""

    'This is a String that will Show the Total Starting with Statewide, If not Statewide then by Region
    'If no Regions Show Counties
    Public gStrStateWideElseRegionElseCounty As String = ""

    'Constructor Expects IncidentID
    Public Sub New(ByVal strIncidentID As String)

        gStrIncidentID = strIncidentID

        PopulateGlobalVariables()

        gStrRegions = GetRegions()

        gStrRegionsAffected = GetRegionsAffected()

        gStrCountiesAffected = GetCountiesAffected()

        gStrRegionCoordinatorEmail = GetRegionCoordinatorEmailByAffectedCounty()

        gStrRegionCoordinator = GetRegionCoordinatorByAffectedCounty()

        gStrCountyCoordinatorEmail = GetCountyCoordinatorEmailByAffectedCounty()

        gStrCountyCoordinator = GetCountyCoordinatorByAffectedCounty()

    End Sub

    Protected Overrides Sub Finalize()
        ' Destructor
    End Sub

    Public Sub PopulateGlobalVariables()

        'Grabbing from the CountyRegionCheck Table
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectCountyRegionCheckByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", gStrIncidentID)

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            gBoolAlachua = HelpFunction.ConvertdbnullsBool(objDR("Alachua"))
            gBoolBaker = HelpFunction.ConvertdbnullsBool(objDR("Baker"))
            gBoolBay = HelpFunction.ConvertdbnullsBool(objDR("Bay"))
            gBoolBradford = HelpFunction.ConvertdbnullsBool(objDR("Bradford"))
            gBoolBrevard = HelpFunction.ConvertdbnullsBool(objDR("Brevard"))
            gBoolBroward = HelpFunction.ConvertdbnullsBool(objDR("Broward"))
            gBoolCalhoun = HelpFunction.ConvertdbnullsBool(objDR("Calhoun"))
            gBoolCharlotte = HelpFunction.ConvertdbnullsBool(objDR("Charlotte"))
            gBoolCitrus = HelpFunction.ConvertdbnullsBool(objDR("Citrus"))
            gBoolClay = HelpFunction.ConvertdbnullsBool(objDR("Clay"))
            gBoolCollier = HelpFunction.ConvertdbnullsBool(objDR("Collier"))
            gBoolColumbia = HelpFunction.ConvertdbnullsBool(objDR("Columbia"))
            gBoolDeSoto = HelpFunction.ConvertdbnullsBool(objDR("DeSoto"))
            gBoolDixie = HelpFunction.ConvertdbnullsBool(objDR("Dixie"))
            gBoolDuval = HelpFunction.ConvertdbnullsBool(objDR("Duval"))
            gBoolEscambia = HelpFunction.ConvertdbnullsBool(objDR("Escambia"))
            gBoolFlagler = HelpFunction.ConvertdbnullsBool(objDR("Flagler"))
            gBoolFranklin = HelpFunction.ConvertdbnullsBool(objDR("Franklin"))
            gBoolGadsden = HelpFunction.ConvertdbnullsBool(objDR("Gadsden"))
            gBoolGilchrist = HelpFunction.ConvertdbnullsBool(objDR("Gilchrist"))
            gBoolGlades = HelpFunction.ConvertdbnullsBool(objDR("Glades"))
            gBoolGulf = HelpFunction.ConvertdbnullsBool(objDR("Gulf"))
            gBoolHamilton = HelpFunction.ConvertdbnullsBool(objDR("Hamilton"))
            gBoolHardee = HelpFunction.ConvertdbnullsBool(objDR("Hardee"))
            gBoolHendry = HelpFunction.ConvertdbnullsBool(objDR("Hendry"))
            gBoolHernando = HelpFunction.ConvertdbnullsBool(objDR("Hernando"))
            gBoolHighlands = HelpFunction.ConvertdbnullsBool(objDR("Highlands"))
            gBoolHillsborough = HelpFunction.ConvertdbnullsBool(objDR("Hillsborough"))
            gBoolHolmes = HelpFunction.ConvertdbnullsBool(objDR("Holmes"))
            gBoolIndianRiver = HelpFunction.ConvertdbnullsBool(objDR("Indian River"))
            gBoolJackson = HelpFunction.ConvertdbnullsBool(objDR("Jackson"))
            gBoolJefferson = HelpFunction.ConvertdbnullsBool(objDR("Jefferson"))
            gBoolLafayette = HelpFunction.ConvertdbnullsBool(objDR("Lafayette"))
            gBoolLake = HelpFunction.ConvertdbnullsBool(objDR("Lake"))
            gBoolLee = HelpFunction.ConvertdbnullsBool(objDR("Lee"))
            gBoolLeon = HelpFunction.ConvertdbnullsBool(objDR("Leon"))
            gBoolLevy = HelpFunction.ConvertdbnullsBool(objDR("Levy"))
            gBoolLiberty = HelpFunction.ConvertdbnullsBool(objDR("Liberty"))
            gBoolMadison = HelpFunction.ConvertdbnullsBool(objDR("Madison"))
            gBoolManatee = HelpFunction.ConvertdbnullsBool(objDR("Manatee"))
            gBoolMarion = HelpFunction.ConvertdbnullsBool(objDR("Marion"))
            gBoolMartin = HelpFunction.ConvertdbnullsBool(objDR("Martin"))
            gBoolMiamiDade = HelpFunction.ConvertdbnullsBool(objDR("Miami-Dade"))
            gBoolMonroe = HelpFunction.ConvertdbnullsBool(objDR("Monroe"))
            gBoolNassau = HelpFunction.ConvertdbnullsBool(objDR("Nassau"))
            gBoolOkaloosa = HelpFunction.ConvertdbnullsBool(objDR("Okaloosa"))
            gBoolOkeechobee = HelpFunction.ConvertdbnullsBool(objDR("Okeechobee"))
            gBoolOrange = HelpFunction.ConvertdbnullsBool(objDR("Orange"))
            gBoolOsceola = HelpFunction.ConvertdbnullsBool(objDR("Osceola"))
            gBoolPalmBeach = HelpFunction.ConvertdbnullsBool(objDR("Palm Beach"))
            gBoolPasco = HelpFunction.ConvertdbnullsBool(objDR("Pasco"))
            gBoolPinellas = HelpFunction.ConvertdbnullsBool(objDR("Pinellas"))
            gBoolPolk = HelpFunction.ConvertdbnullsBool(objDR("Polk"))
            gBoolPutnam = HelpFunction.ConvertdbnullsBool(objDR("Putnam"))
            gBoolSantaRosa = HelpFunction.ConvertdbnullsBool(objDR("Santa Rosa"))
            gBoolSarasota = HelpFunction.ConvertdbnullsBool(objDR("Sarasota"))
            gBoolSeminole = HelpFunction.ConvertdbnullsBool(objDR("Seminole"))
            gBoolStJohns = HelpFunction.ConvertdbnullsBool(objDR("St. Johns"))
            gBoolStLucie = HelpFunction.ConvertdbnullsBool(objDR("St. Lucie"))
            gBoolSumter = HelpFunction.ConvertdbnullsBool(objDR("Sumter"))
            gBoolSuwannee = HelpFunction.ConvertdbnullsBool(objDR("Suwannee"))
            gBoolTaylor = HelpFunction.ConvertdbnullsBool(objDR("Taylor"))
            gBoolUnion = HelpFunction.ConvertdbnullsBool(objDR("Union"))
            gBoolVolusia = HelpFunction.ConvertdbnullsBool(objDR("Volusia"))
            gBoolWakulla = HelpFunction.ConvertdbnullsBool(objDR("Wakulla"))
            gBoolWalton = HelpFunction.ConvertdbnullsBool(objDR("Walton"))
            gBoolWashington = HelpFunction.ConvertdbnullsBool(objDR("Washington"))

            'Region
            gBoolRegion1 = HelpFunction.ConvertdbnullsBool(objDR("Region1"))
            gBoolRegion2 = HelpFunction.ConvertdbnullsBool(objDR("Region2"))
            gBoolRegion3 = HelpFunction.ConvertdbnullsBool(objDR("Region3"))
            gBoolRegion4 = HelpFunction.ConvertdbnullsBool(objDR("Region4"))
            gBoolRegion5 = HelpFunction.ConvertdbnullsBool(objDR("Region5"))
            gBoolRegion6 = HelpFunction.ConvertdbnullsBool(objDR("Region6"))
            gBoolRegion7 = HelpFunction.ConvertdbnullsBool(objDR("Region7"))

            'Region Affected
            gBoolRegion1Affected = HelpFunction.ConvertdbnullsBool(objDR("Region1Affected"))
            gBoolRegion2Affected = HelpFunction.ConvertdbnullsBool(objDR("Region2Affected"))
            gBoolRegion3Affected = HelpFunction.ConvertdbnullsBool(objDR("Region3Affected"))
            gBoolRegion4Affected = HelpFunction.ConvertdbnullsBool(objDR("Region4Affected"))
            gBoolRegion5Affected = HelpFunction.ConvertdbnullsBool(objDR("Region5Affected"))
            gBoolRegion6Affected = HelpFunction.ConvertdbnullsBool(objDR("Region6Affected"))
            gBoolRegion7Affected = HelpFunction.ConvertdbnullsBool(objDR("Region7Affected"))

            gBoolStateWide = HelpFunction.ConvertdbnullsBool(objDR("StateWide"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

    End Sub

    Public Function GetRegions() As String

        Dim strRegions As String = ""

        If gBoolRegion1 = True Then
            strRegions = strRegions & "Region 1,"
        End If

        If gBoolRegion2 = True Then
            strRegions = strRegions & "Region 2,"
        End If

        If gBoolRegion3 = True Then
            strRegions = strRegions & "Region 3,"
        End If

        If gBoolRegion4 = True Then
            strRegions = strRegions & "Region 4,"
        End If

        If gBoolRegion5 = True Then
            strRegions = strRegions & "Region 5,"
        End If

        If gBoolRegion6 = True Then
            strRegions = strRegions & "Region 6,"
        End If

        If gBoolRegion7 = True Then
            strRegions = strRegions & "Region 7,"
        End If

        'remove the last comma
        If strRegions <> "" Then
            strRegions = Mid(strRegions, 1, Len(strRegions) - 1)
        End If

        Return strRegions

    End Function

    Public Function GetRegionsAffected() As String

        Dim strRegionsAffected As String = ""

        If gBoolRegion1Affected = True Then
            strRegionsAffected = strRegionsAffected & "Region 1,"
        End If

        If gBoolRegion2Affected = True Then
            strRegionsAffected = strRegionsAffected & "Region 2,"
        End If

        If gBoolRegion3Affected = True Then
            strRegionsAffected = strRegionsAffected & "Region 3,"
        End If

        If gBoolRegion4Affected = True Then
            strRegionsAffected = strRegionsAffected & "Region 4,"
        End If

        If gBoolRegion5Affected = True Then
            strRegionsAffected = strRegionsAffected & "Region 5,"
        End If

        If gBoolRegion6Affected = True Then
            strRegionsAffected = strRegionsAffected & "Region 6,"
        End If

        If gBoolRegion7Affected = True Then
            strRegionsAffected = strRegionsAffected & "Region 7,"
        End If

        'remove the last comma
        If strRegionsAffected <> "" Then
            strRegionsAffected = Mid(strRegionsAffected, 1, Len(strRegionsAffected) - 1)
        End If

        Return strRegionsAffected

    End Function

    Public Function GetCountiesAffected() As String

        Dim strCountiesAffected As String = ""

        If gBoolAlachua = True Then
            strCountiesAffected = strCountiesAffected & "Alachua, "
        End If

        If gBoolBaker = True Then
            strCountiesAffected = strCountiesAffected & "Baker, "
        End If

        If gBoolBay = True Then
            strCountiesAffected = strCountiesAffected & "Bay, "
        End If

        If gBoolBradford = True Then
            strCountiesAffected = strCountiesAffected & "Bradford, "
        End If

        If gBoolBrevard = True Then
            strCountiesAffected = strCountiesAffected & "Brevard, "
        End If

        If gBoolBroward = True Then
            strCountiesAffected = strCountiesAffected & "Broward, "
        End If

        If gBoolCalhoun = True Then
            strCountiesAffected = strCountiesAffected & "Calhoun, "
        End If

        If gBoolCharlotte = True Then
            strCountiesAffected = strCountiesAffected & "Charlotte, "
        End If

        If gBoolCitrus = True Then
            strCountiesAffected = strCountiesAffected & "Citrus, "
        End If

        If gBoolClay = True Then
            strCountiesAffected = strCountiesAffected & "Clay, "
        End If

        If gBoolCollier = True Then
            strCountiesAffected = strCountiesAffected & "Collier, "
        End If

        If gBoolColumbia = True Then
            strCountiesAffected = strCountiesAffected & "Columbia, "
        End If

        If gBoolDeSoto = True Then
            strCountiesAffected = strCountiesAffected & "DeSoto, "
        End If

        If gBoolDixie = True Then
            strCountiesAffected = strCountiesAffected & "Dixie, "
        End If

        If gBoolDuval = True Then
            strCountiesAffected = strCountiesAffected & "Duval, "
        End If

        If gBoolEscambia = True Then
            strCountiesAffected = strCountiesAffected & "Escambia, "
        End If

        If gBoolFlagler = True Then
            strCountiesAffected = strCountiesAffected & "Flagler, "
        End If

        If gBoolFranklin = True Then
            strCountiesAffected = strCountiesAffected & "Franklin, "
        End If

        If gBoolGadsden = True Then
            strCountiesAffected = strCountiesAffected & "Gadsden, "
        End If

        If gBoolGilchrist = True Then
            strCountiesAffected = strCountiesAffected & "Gilchrist, "
        End If

        If gBoolGlades = True Then
            strCountiesAffected = strCountiesAffected & "Glades, "
        End If

        If gBoolGulf = True Then
            strCountiesAffected = strCountiesAffected & "Gulf, "
        End If

        If gBoolHamilton = True Then
            strCountiesAffected = strCountiesAffected & "Hamilton, "
        End If

        If gBoolHardee = True Then
            strCountiesAffected = strCountiesAffected & "Hardee, "
        End If

        If gBoolHendry = True Then
            strCountiesAffected = strCountiesAffected & "Hendry, "
        End If

        If gBoolHernando = True Then
            strCountiesAffected = strCountiesAffected & "Hernando, "
        End If

        If gBoolHighlands = True Then
            strCountiesAffected = strCountiesAffected & "Highlands, "
        End If

        If gBoolHillsborough = True Then
            strCountiesAffected = strCountiesAffected & "Hillsborough, "
        End If

        If gBoolHolmes = True Then
            strCountiesAffected = strCountiesAffected & "Holmes, "
        End If

        If gBoolIndianRiver = True Then
            strCountiesAffected = strCountiesAffected & "Indian River, "
        End If

        If gBoolJackson = True Then
            strCountiesAffected = strCountiesAffected & "Jackson, "
        End If

        If gBoolJefferson = True Then
            strCountiesAffected = strCountiesAffected & "Jefferson, "
        End If

        If gBoolLafayette = True Then
            strCountiesAffected = strCountiesAffected & "Lafayette, "
        End If

        If gBoolLake = True Then
            strCountiesAffected = strCountiesAffected & "Lake, "
        End If

        If gBoolLee = True Then
            strCountiesAffected = strCountiesAffected & "Lee, "
        End If

        If gBoolLeon = True Then
            strCountiesAffected = strCountiesAffected & "Leon, "
        End If

        If gBoolLevy = True Then
            strCountiesAffected = strCountiesAffected & "Levy, "
        End If

        If gBoolLiberty = True Then
            strCountiesAffected = strCountiesAffected & "Liberty, "
        End If

        If gBoolMadison = True Then
            strCountiesAffected = strCountiesAffected & "Madison, "
        End If

        If gBoolManatee = True Then
            strCountiesAffected = strCountiesAffected & "Manatee, "
        End If

        If gBoolMarion = True Then
            strCountiesAffected = strCountiesAffected & "Marion, "
        End If

        If gBoolMartin = True Then
            strCountiesAffected = strCountiesAffected & "Martin, "
        End If

        If gBoolMiamiDade = True Then
            strCountiesAffected = strCountiesAffected & "Miami-Dade, "
        End If

        If gBoolMonroe = True Then
            strCountiesAffected = strCountiesAffected & "Monroe, "
        End If

        If gBoolNassau = True Then
            strCountiesAffected = strCountiesAffected & "Nassau, "
        End If

        If gBoolOkaloosa = True Then
            strCountiesAffected = strCountiesAffected & "Okaloosa, "
        End If

        If gBoolOkeechobee = True Then
            strCountiesAffected = strCountiesAffected & "Okeechobee, "
        End If

        If gBoolOrange = True Then
            strCountiesAffected = strCountiesAffected & "Orange, "
        End If

        If gBoolOsceola = True Then
            strCountiesAffected = strCountiesAffected & "Osceola, "
        End If

        If gBoolPalmBeach = True Then
            strCountiesAffected = strCountiesAffected & "Palm Beach, "
        End If

        If gBoolPasco = True Then
            strCountiesAffected = strCountiesAffected & "Pasco, "
        End If

        If gBoolPinellas = True Then
            strCountiesAffected = strCountiesAffected & "Pinellas, "
        End If

        If gBoolPolk = True Then
            strCountiesAffected = strCountiesAffected & "Polk, "
        End If

        If gBoolPutnam = True Then
            strCountiesAffected = strCountiesAffected & "Putnam, "
        End If

        If gBoolSantaRosa = True Then
            strCountiesAffected = strCountiesAffected & "Santa Rosa, "
        End If

        If gBoolSarasota = True Then
            strCountiesAffected = strCountiesAffected & "Sarasota, "
        End If

        If gBoolSeminole = True Then
            strCountiesAffected = strCountiesAffected & "Seminole, "
        End If

        If gBoolStJohns = True Then
            strCountiesAffected = strCountiesAffected & "St. Johns, "
        End If

        If gBoolStLucie = True Then
            strCountiesAffected = strCountiesAffected & "St. Lucie, "
        End If

        If gBoolSumter = True Then
            strCountiesAffected = strCountiesAffected & "Sumter, "
        End If

        If gBoolSuwannee = True Then
            strCountiesAffected = strCountiesAffected & "Suwannee, "
        End If

        If gBoolTaylor = True Then
            strCountiesAffected = strCountiesAffected & "Taylor, "
        End If

        If gBoolUnion = True Then
            strCountiesAffected = strCountiesAffected & "Union, "
        End If

        If gBoolVolusia = True Then
            strCountiesAffected = strCountiesAffected & "Volusia, "
        End If

        If gBoolWakulla = True Then
            strCountiesAffected = strCountiesAffected & "Wakulla, "
        End If

        If gBoolWalton = True Then
            strCountiesAffected = strCountiesAffected & "Walton, "
        End If

        If gBoolWashington = True Then
            strCountiesAffected = strCountiesAffected & "Washington, "
        End If

        'remove the last comma
        If strCountiesAffected <> "" Then
            strCountiesAffected = strCountiesAffected.Remove(strCountiesAffected.Length - 2, 2)
        Else
            strCountiesAffected = "NO COUNTIES ADDED AT THIS TIME"
        End If

        Return strCountiesAffected

    End Function

    Public Function GetStateWideElseRegionElseCounty() As String

        Dim strStateWideElseRegionElseCounty As String = ""




        If gBoolStateWide = True Then

            strStateWideElseRegionElseCounty = "Statewide"

        Else

            If gBoolRegion1 = True Then

                strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Region 1, "

            Else


                If gBoolRegion1Affected = True Then

                    strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & GetCountiesAffectedByRegionAndComma(1, False)


                End If

            End If


            If gBoolRegion2 = True Then

                strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Region 2, "

            Else

                If gBoolRegion2Affected = True Then
                    strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & GetCountiesAffectedByRegionAndComma(2, False)
                End If

            End If


            If gBoolRegion3 = True Then

                strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Region 3, "

            Else

                If gBoolRegion3Affected = True Then
                    strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & GetCountiesAffectedByRegionAndComma(3, False)
                End If

            End If


            If gBoolRegion4 = True Then

                strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Region 4, "

            Else

                If gBoolRegion4Affected = True Then
                    strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & GetCountiesAffectedByRegionAndComma(4, False)
                End If

            End If


            If gBoolRegion5 = True Then

                strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Region 5, "

            Else

                If gBoolRegion5Affected = True Then
                    strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & GetCountiesAffectedByRegionAndComma(5, False)
                End If

            End If


            If gBoolRegion6 = True Then

                strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Region 6, "

            Else

                If gBoolRegion6Affected = True Then
                    strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & GetCountiesAffectedByRegionAndComma(6, False)
                End If

            End If


            If gBoolRegion7 = True Then

                strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Region 7, "

            Else

                If gBoolRegion7Affected = True Then
                    strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & GetCountiesAffectedByRegionAndComma(7, False)
                End If

            End If

            'remove the last comma
            If strStateWideElseRegionElseCounty <> "" Then
                strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty.Remove(strStateWideElseRegionElseCounty.Length - 2, 2)
            End If

        End If



        Return strStateWideElseRegionElseCounty

    End Function

    Public Function GetStateWideElseRegionElseCountyAlphabetical() As String

        Dim strStateWideElseRegionElseCounty As String = ""

        If gBoolStateWide = True Then

            strStateWideElseRegionElseCounty = "Statewide"

        Else

            'First the Regions
            If gBoolRegion1 = True Then

                strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Region 1, "

            End If


            If gBoolRegion2 = True Then

                strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Region 2, "

            End If


            If gBoolRegion3 = True Then

                strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Region 3, "

            End If


            If gBoolRegion4 = True Then

                strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Region 4, "

            End If


            If gBoolRegion5 = True Then

                strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Region 5, "

            End If


            If gBoolRegion6 = True Then

                strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Region 6, "

            End If


            If gBoolRegion7 = True Then

                strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Region 7, "

            End If


            'Now the Counties
            If gBoolRegion3 = False Then

                If gBoolRegion3Affected = True Then

                    If gBoolAlachua = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Alachua, "
                    End If

                End If

            End If

            If gBoolRegion3 = False Then

                If gBoolRegion3Affected = True Then

                    If gBoolBaker = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Baker, "
                    End If

                End If

            End If

            If gBoolRegion1 = False Then

                If gBoolRegion1Affected = True Then

                    If gBoolBay = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Bay, "
                    End If

                End If

            End If

            If gBoolRegion3 = False Then

                If gBoolRegion3Affected = True Then

                    If gBoolBradford = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Bradford, "
                    End If

                End If

            End If

            If gBoolRegion5 = False Then

                If gBoolRegion5Affected = True Then

                    If gBoolBrevard = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Brevard, "
                    End If

                End If

            End If

            If gBoolRegion7 = False Then

                If gBoolRegion7Affected = True Then

                    If gBoolBroward = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Broward, "
                    End If

                End If

            End If

            If gBoolRegion1 = False Then

                If gBoolRegion1Affected = True Then

                    If gBoolCalhoun = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Calhoun, "
                    End If

                End If

            End If

            If gBoolRegion6 = False Then

                If gBoolRegion6Affected = True Then

                    If gBoolCharlotte = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Charlotte, "
                    End If

                End If

            End If

            If gBoolRegion4 = False Then

                If gBoolRegion4Affected = True Then

                    If gBoolCitrus = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Citrus, "
                    End If

                End If

            End If

            If gBoolRegion3 = False Then

                If gBoolRegion3Affected = True Then

                    If gBoolClay = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Clay, "
                    End If

                End If

            End If

            If gBoolRegion6 = False Then

                If gBoolRegion6Affected = True Then

                    If gBoolCollier = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Collier, "
                    End If

                End If

            End If

            If gBoolRegion2 = False Then

                If gBoolRegion2Affected = True Then

                    If gBoolColumbia = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Columbia, "
                    End If

                End If

            End If

            If gBoolRegion6 = False Then

                If gBoolRegion6Affected = True Then

                    If gBoolDeSoto = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "DeSoto, "
                    End If

                End If

            End If

            If gBoolRegion2 = False Then

                If gBoolRegion2Affected = True Then

                    If gBoolDixie = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Dixie, "
                    End If

                End If

            End If

            If gBoolRegion3 = False Then

                If gBoolRegion3Affected = True Then

                    If gBoolDuval = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Duval, "
                    End If

                End If

            End If

            If gBoolRegion1 = False Then

                If gBoolRegion1Affected = True Then

                    If gBoolEscambia = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Escambia, "
                    End If

                End If

            End If

            If gBoolRegion3 = False Then

                If gBoolRegion3Affected = True Then

                    If gBoolFlagler = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Flagler, "
                    End If

                End If

            End If

            If gBoolRegion2 = False Then

                If gBoolRegion2Affected = True Then

                    If gBoolFranklin = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Franklin, "
                    End If

                End If

            End If

            If gBoolRegion2 = False Then

                If gBoolRegion2Affected = True Then

                    If gBoolGadsden = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Gadsden, "
                    End If

                End If

            End If

            If gBoolRegion3 = False Then

                If gBoolRegion3Affected = True Then

                    If gBoolGilchrist = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Gilchrist, "
                    End If

                End If

            End If

            If gBoolRegion6 = False Then

                If gBoolRegion6Affected = True Then

                    If gBoolGlades = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Glades, "
                    End If

                End If

            End If

            If gBoolRegion1 = False Then

                If gBoolRegion1Affected = True Then

                    If gBoolGulf = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Gulf, "
                    End If

                End If

            End If

            If gBoolRegion2 = False Then

                If gBoolRegion2Affected = True Then

                    If gBoolHamilton = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Hamilton, "
                    End If

                End If

            End If

            If gBoolRegion4 = False Then

                If gBoolRegion4Affected = True Then

                    If gBoolHardee = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Hardee, "
                    End If

                End If

            End If

            If gBoolRegion6 = False Then

                If gBoolRegion6Affected = True Then

                    If gBoolHendry = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Hendry, "
                    End If

                End If

            End If

            If gBoolRegion4 = False Then

                If gBoolRegion4Affected = True Then

                    If gBoolHernando = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Hernando, "
                    End If

                End If

            End If

            If gBoolRegion6 = False Then

                If gBoolRegion6Affected = True Then

                    If gBoolHighlands = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Highlands, "
                    End If

                End If

            End If

            If gBoolRegion4 = False Then

                If gBoolRegion4Affected = True Then

                    If gBoolHillsborough = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Hillsborough, "
                    End If

                End If

            End If

            If gBoolRegion1 = False Then

                If gBoolRegion1Affected = True Then

                    If gBoolHolmes = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Holmes, "
                    End If

                End If

            End If

            If gBoolRegion5 = False Then

                If gBoolRegion5Affected = True Then

                    If gBoolIndianRiver = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Indian River, "
                    End If

                End If

            End If

            If gBoolRegion1 = False Then

                If gBoolRegion1Affected = True Then

                    If gBoolJackson = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Jackson, "
                    End If

                End If

            End If

            If gBoolRegion2 = False Then

                If gBoolRegion2Affected = True Then

                    If gBoolJefferson = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Jefferson, "
                    End If

                End If

            End If

            If gBoolRegion2 = False Then

                If gBoolRegion2Affected = True Then

                    If gBoolLafayette = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Lafayette, "
                    End If

                End If

            End If

            If gBoolRegion5 = False Then

                If gBoolRegion5Affected = True Then

                    If gBoolLake = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Lake, "
                    End If

                End If

            End If

            If gBoolRegion6 = False Then

                If gBoolRegion6Affected = True Then

                    If gBoolLee = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Lee, "
                    End If

                End If

            End If

            If gBoolRegion2 = False Then

                If gBoolRegion2Affected = True Then

                    If gBoolLeon = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Leon, "
                    End If

                End If

            End If

            If gBoolRegion3 = False Then

                If gBoolRegion3Affected = True Then

                    If gBoolLevy = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Levy, "
                    End If

                End If

            End If

            If gBoolRegion2 = False Then

                If gBoolRegion2Affected = True Then

                    If gBoolLiberty = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Liberty, "
                    End If

                End If

            End If

            If gBoolRegion2 = False Then

                If gBoolRegion2Affected = True Then

                    If gBoolMadison = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Madison, "
                    End If

                End If

            End If

            If gBoolRegion6 = False Then

                If gBoolRegion6Affected = True Then

                    If gBoolManatee = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Manatee, "
                    End If

                End If

            End If

            If gBoolRegion3 = False Then

                If gBoolRegion3Affected = True Then

                    If gBoolMarion = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Marion, "
                    End If

                End If

            End If

            If gBoolRegion5 = False Then

                If gBoolRegion5Affected = True Then

                    If gBoolMartin = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Martin, "
                    End If

                End If

            End If

            If gBoolRegion7 = False Then

                If gBoolRegion7Affected = True Then

                    If gBoolMiamiDade = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Miami-Dade, "
                    End If

                End If

            End If

            If gBoolRegion7 = False Then

                If gBoolRegion7Affected = True Then

                    If gBoolMonroe = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Monroe, "
                    End If

                End If

            End If

            If gBoolRegion3 = False Then

                If gBoolRegion3Affected = True Then

                    If gBoolNassau = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Nassau, "
                    End If

                End If

            End If

            If gBoolRegion1 = False Then

                If gBoolRegion1Affected = True Then

                    If gBoolOkaloosa = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Okaloosa, "
                    End If

                End If

            End If

            If gBoolRegion6 = False Then

                If gBoolRegion6Affected = True Then

                    If gBoolOkeechobee = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Okeechobee, "
                    End If

                End If

            End If

            If gBoolRegion5 = False Then

                If gBoolRegion5Affected = True Then

                    If gBoolOrange = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Orange, "
                    End If

                End If

            End If

            If gBoolRegion5 = False Then

                If gBoolRegion5Affected = True Then

                    If gBoolOsceola = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Osceola, "
                    End If

                End If

            End If

            If gBoolRegion7 = False Then

                If gBoolRegion7Affected = True Then

                    If gBoolPalmBeach = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Palm Beach, "
                    End If

                End If

            End If

            If gBoolRegion4 = False Then

                If gBoolRegion4Affected = True Then

                    If gBoolPasco = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Pasco, "
                    End If

                End If

            End If

            If gBoolRegion4 = False Then

                If gBoolRegion4Affected = True Then

                    If gBoolPinellas = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Pinellas, "
                    End If

                End If

            End If

            If gBoolRegion4 = False Then

                If gBoolRegion4Affected = True Then

                    If gBoolPolk = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Polk, "
                    End If

                End If

            End If

            If gBoolRegion3 = False Then

                If gBoolRegion3Affected = True Then

                    If gBoolPutnam = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Putnam, "
                    End If

                End If

            End If

            If gBoolRegion1 = False Then

                If gBoolRegion1Affected = True Then

                    If gBoolSantaRosa = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Santa Rosa, "
                    End If

                End If

            End If

            If gBoolRegion6 = False Then

                If gBoolRegion6Affected = True Then

                    If gBoolSarasota = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Sarasota, "
                    End If

                End If

            End If

            If gBoolRegion5 = False Then

                If gBoolRegion5Affected = True Then

                    If gBoolSeminole = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Seminole, "
                    End If

                End If

            End If

            If gBoolRegion3 = False Then

                If gBoolRegion3Affected = True Then

                    If gBoolStJohns = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "St. Johns, "
                    End If

                End If

            End If

            If gBoolRegion5 = False Then

                If gBoolRegion5Affected = True Then

                    If gBoolStLucie = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "St. Lucie, "
                    End If

                End If

            End If

            If gBoolRegion4 = False Then

                If gBoolRegion4Affected = True Then

                    If gBoolSumter = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Sumter, "
                    End If

                End If

            End If

            If gBoolRegion2 = False Then

                If gBoolRegion2Affected = True Then

                    If gBoolSuwannee = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Suwannee, "
                    End If

                End If

            End If

            If gBoolRegion2 = False Then

                If gBoolRegion2Affected = True Then

                    If gBoolTaylor = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Taylor, "
                    End If

                End If

            End If

            If gBoolRegion3 = False Then

                If gBoolRegion3Affected = True Then

                    If gBoolUnion = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Union, "
                    End If

                End If

            End If

            If gBoolRegion5 = False Then

                If gBoolRegion5Affected = True Then

                    If gBoolVolusia = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Volusia, "
                    End If

                End If

            End If

            If gBoolRegion2 = False Then

                If gBoolRegion2Affected = True Then

                    If gBoolWakulla = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Wakulla, "
                    End If

                End If

            End If

            If gBoolRegion1 = False Then

                If gBoolRegion1Affected = True Then

                    If gBoolWalton = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Walton, "
                    End If

                End If

            End If

            If gBoolRegion1 = False Then

                If gBoolRegion1Affected = True Then

                    If gBoolWashington = True Then
                        strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty & "Washington, "
                    End If

                End If

            End If


            'remove the last comma
            If strStateWideElseRegionElseCounty <> "" Then
                strStateWideElseRegionElseCounty = strStateWideElseRegionElseCounty.Remove(strStateWideElseRegionElseCounty.Length - 2, 2)
            End If

        End If



        Return strStateWideElseRegionElseCounty

    End Function

    Public Function GetCountiesAffectedByRegionAndComma(ByVal intRegionID As Integer, ByVal boolOmitComma As Boolean) As String

        Dim strCountiesAffected As String = ""

        'Region1 Begin
        If intRegionID = 1 Then

            If gBoolBay = True Then
                strCountiesAffected = strCountiesAffected & " Bay, "
            End If

            If gBoolCalhoun = True Then
                strCountiesAffected = strCountiesAffected & " Calhoun, "
            End If

            If gBoolEscambia = True Then
                strCountiesAffected = strCountiesAffected & " Escambia, "
            End If

            If gBoolGulf = True Then
                strCountiesAffected = strCountiesAffected & " Gulf, "
            End If

            If gBoolHolmes = True Then
                strCountiesAffected = strCountiesAffected & " Holmes, "
            End If

            If gBoolJackson = True Then
                strCountiesAffected = strCountiesAffected & " Jackson, "
            End If

            If gBoolOkaloosa = True Then
                strCountiesAffected = strCountiesAffected & " Okaloosa, "
            End If

            If gBoolSantaRosa = True Then
                strCountiesAffected = strCountiesAffected & " Santa Rosa, "
            End If

            If gBoolWalton = True Then
                strCountiesAffected = strCountiesAffected & " Walton, "
            End If

            If gBoolWashington = True Then
                strCountiesAffected = strCountiesAffected & " Washington, "
            End If

        End If
        'Region1 End


        'Region2 Begin
        If intRegionID = 2 Then

            If gBoolColumbia = True Then
                strCountiesAffected = strCountiesAffected & " Columbia, "
            End If

            If gBoolDixie = True Then
                strCountiesAffected = strCountiesAffected & " Dixie, "
            End If

            If gBoolFranklin = True Then
                strCountiesAffected = strCountiesAffected & " Franklin, "
            End If

            If gBoolGadsden = True Then
                strCountiesAffected = strCountiesAffected & " Gadsden, "
            End If

            If gBoolHamilton = True Then
                strCountiesAffected = strCountiesAffected & " Hamilton, "
            End If

            If gBoolJefferson = True Then
                strCountiesAffected = strCountiesAffected & " Jefferson, "
            End If

            If gBoolLafayette = True Then
                strCountiesAffected = strCountiesAffected & " Lafayette, "
            End If

            If gBoolLeon = True Then
                strCountiesAffected = strCountiesAffected & " Leon, "
            End If

            If gBoolLiberty = True Then
                strCountiesAffected = strCountiesAffected & " Liberty, "
            End If

            If gBoolMadison = True Then
                strCountiesAffected = strCountiesAffected & " Madison, "
            End If

            If gBoolSuwannee = True Then
                strCountiesAffected = strCountiesAffected & " Suwannee, "
            End If

            If gBoolTaylor = True Then
                strCountiesAffected = strCountiesAffected & " Taylor, "
            End If

            If gBoolWakulla = True Then
                strCountiesAffected = strCountiesAffected & " Wakulla, "
            End If

        End If
        'Region2 End


        'Region3 Begin
        If intRegionID = 3 Then

            If gBoolAlachua = True Then
                strCountiesAffected = strCountiesAffected & " Alachua, "
            End If

            If gBoolBaker = True Then
                strCountiesAffected = strCountiesAffected & " Baker, "
            End If

            If gBoolBradford = True Then
                strCountiesAffected = strCountiesAffected & " Bradford, "
            End If

            If gBoolClay = True Then
                strCountiesAffected = strCountiesAffected & " Clay, "
            End If

            If gBoolDuval = True Then
                strCountiesAffected = strCountiesAffected & " Duval, "
            End If

            If gBoolFlagler = True Then
                strCountiesAffected = strCountiesAffected & " Flagler, "
            End If

            If gBoolGilchrist = True Then
                strCountiesAffected = strCountiesAffected & " Gilchrist, "
            End If

            If gBoolLevy = True Then
                strCountiesAffected = strCountiesAffected & " Levy, "
            End If

            If gBoolMarion = True Then
                strCountiesAffected = strCountiesAffected & " Marion, "
            End If

            If gBoolNassau = True Then
                strCountiesAffected = strCountiesAffected & " Nassau, "
            End If

            If gBoolPutnam = True Then
                strCountiesAffected = strCountiesAffected & " Putnam, "
            End If

            If gBoolStJohns = True Then
                strCountiesAffected = strCountiesAffected & " St. Johns, "
            End If

            If gBoolUnion = True Then
                strCountiesAffected = strCountiesAffected & " Union, "
            End If

        End If
        'Region3 End


        'Region4 Begin
        If intRegionID = 4 Then

            If gBoolCitrus = True Then
                strCountiesAffected = strCountiesAffected & " Citrus, "
            End If

            If gBoolHardee = True Then
                strCountiesAffected = strCountiesAffected & " Hardee, "
            End If

            If gBoolHernando = True Then
                strCountiesAffected = strCountiesAffected & " Hernando, "
            End If

            If gBoolHillsborough = True Then
                strCountiesAffected = strCountiesAffected & " Hillsborough, "
            End If

            If gBoolPasco = True Then
                strCountiesAffected = strCountiesAffected & " Pasco, "
            End If

            If gBoolPinellas = True Then
                strCountiesAffected = strCountiesAffected & " Pinellas, "
            End If

            If gBoolPolk = True Then
                strCountiesAffected = strCountiesAffected & " Polk, "
            End If

            If gBoolSumter = True Then
                strCountiesAffected = strCountiesAffected & " Sumter, "
            End If
        End If
        'Region4 End


        'Region5 Begin
        If intRegionID = 5 Then
            If gBoolBrevard = True Then
                strCountiesAffected = strCountiesAffected & " Brevard, "
            End If

            If gBoolIndianRiver = True Then
                strCountiesAffected = strCountiesAffected & " Indian River, "
            End If

            If gBoolLake = True Then
                strCountiesAffected = strCountiesAffected & " Lake, "
            End If

            If gBoolMartin = True Then
                strCountiesAffected = strCountiesAffected & " Martin, "
            End If

            If gBoolOrange = True Then
                strCountiesAffected = strCountiesAffected & " Orange, "
            End If

            If gBoolOsceola = True Then
                strCountiesAffected = strCountiesAffected & " Osceola, "
            End If

            If gBoolSeminole = True Then
                strCountiesAffected = strCountiesAffected & " Seminole, "
            End If

            If gBoolStLucie = True Then
                strCountiesAffected = strCountiesAffected & " St. Lucie, "
            End If

            If gBoolVolusia = True Then
                strCountiesAffected = strCountiesAffected & " Volusia, "
            End If
        End If
        'Region5 End


        'Region6 Begin
        If intRegionID = 6 Then

            If gBoolCharlotte = True Then
                strCountiesAffected = strCountiesAffected & " Charlotte, "
            End If

            If gBoolCollier = True Then
                strCountiesAffected = strCountiesAffected & " Collier, "
            End If

            If gBoolDeSoto = True Then
                strCountiesAffected = strCountiesAffected & " DeSoto, "
            End If

            If gBoolGlades = True Then
                strCountiesAffected = strCountiesAffected & " Glades, "
            End If

            If gBoolHendry = True Then
                strCountiesAffected = strCountiesAffected & " Hendry, "
            End If

            If gBoolHighlands = True Then
                strCountiesAffected = strCountiesAffected & " Highlands, "
            End If

            If gBoolLee = True Then
                strCountiesAffected = strCountiesAffected & " Lee, "
            End If

            If gBoolManatee = True Then
                strCountiesAffected = strCountiesAffected & " Manatee, "
            End If

            If gBoolOkeechobee = True Then
                strCountiesAffected = strCountiesAffected & " Okeechobee, "
            End If

            If gBoolSarasota = True Then
                strCountiesAffected = strCountiesAffected & " Sarasota, "
            End If
        End If
        'Region6 End


        'Region7 Begin
        If intRegionID = 7 Then
            If gBoolBroward = True Then
                strCountiesAffected = strCountiesAffected & " Broward, "
            End If

            If gBoolMiamiDade = True Then
                strCountiesAffected = strCountiesAffected & " Miami-Dade, "
            End If

            If gBoolMonroe = True Then
                strCountiesAffected = strCountiesAffected & " Monroe, "
            End If

            If gBoolPalmBeach = True Then
                strCountiesAffected = strCountiesAffected & " Palm Beach, "
            End If
        End If
        'Region7 End


        If boolOmitComma = True Then
            'remove the last comma
            If strCountiesAffected <> "" Then
                strCountiesAffected = strCountiesAffected.Remove(strCountiesAffected.Length - 2, 2)
            End If
        End If

        If strCountiesAffected = "" Then
            strCountiesAffected = "NO COUNTIES ADDED AT THIS TIME"
        End If

        Return strCountiesAffected

    End Function



    Public Function GetRegionCoordinatorEmailByAffectedCounty() As String

        Dim strEmailList As String = ""

        If gBoolRegion1Affected = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectRegionCoordinatorByRegionID]", objConn)
            objCmd.Parameters.AddWithValue("@RegionID", 1)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolRegion2Affected = True Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectRegionCoordinatorByRegionID]", objConn)
            objCmd.Parameters.AddWithValue("@RegionID", 2)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()
        End If

        If gBoolRegion3Affected = True Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectRegionCoordinatorByRegionID]", objConn)
            objCmd.Parameters.AddWithValue("@RegionID", 3)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()
        End If

        If gBoolRegion4Affected = True Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectRegionCoordinatorByRegionID]", objConn)
            objCmd.Parameters.AddWithValue("@RegionID", 4)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()
        End If

        If gBoolRegion5Affected = True Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectRegionCoordinatorByRegionID]", objConn)
            objCmd.Parameters.AddWithValue("@RegionID", 5)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()
        End If

        If gBoolRegion6Affected = True Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectRegionCoordinatorByRegionID]", objConn)
            objCmd.Parameters.AddWithValue("@RegionID", 6)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()
        End If

        If gBoolRegion7Affected = True Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectRegionCoordinatorByRegionID]", objConn)
            objCmd.Parameters.AddWithValue("@RegionID", 7)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()
        End If

        If strEmailList <> "" Then
            strEmailList = Left(strEmailList, strEmailList.Length - 2)
        End If

        Return strEmailList

    End Function

    Public Function GetRegionCoordinatorByAffectedCounty() As String

        Dim strRegionCoordinator As String = ""

        If gBoolRegion1Affected = True Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectRegionCoordinatorByRegionID]", objConn)
            objCmd.Parameters.AddWithValue("@RegionID", 1)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strRegionCoordinator = strRegionCoordinator & objDR.Item("RegionCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()
        End If

        If gBoolRegion2Affected = True Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectRegionCoordinatorByRegionID]", objConn)
            objCmd.Parameters.AddWithValue("@RegionID", 2)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strRegionCoordinator = strRegionCoordinator & objDR.Item("RegionCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()
        End If

        If gBoolRegion3Affected = True Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectRegionCoordinatorByRegionID]", objConn)
            objCmd.Parameters.AddWithValue("@RegionID", 3)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strRegionCoordinator = strRegionCoordinator & objDR.Item("RegionCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()
        End If

        If gBoolRegion4Affected = True Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectRegionCoordinatorByRegionID]", objConn)
            objCmd.Parameters.AddWithValue("@RegionID", 4)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strRegionCoordinator = strRegionCoordinator & objDR.Item("RegionCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()
        End If

        If gBoolRegion5Affected = True Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectRegionCoordinatorByRegionID]", objConn)
            objCmd.Parameters.AddWithValue("@RegionID", 5)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strRegionCoordinator = strRegionCoordinator & objDR.Item("RegionCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()
        End If

        If gBoolRegion6Affected = True Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectRegionCoordinatorByRegionID]", objConn)
            objCmd.Parameters.AddWithValue("@RegionID", 6)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strRegionCoordinator = strRegionCoordinator & objDR.Item("RegionCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()
        End If

        If gBoolRegion7Affected = True Then
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectRegionCoordinatorByRegionID]", objConn)
            objCmd.Parameters.AddWithValue("@RegionID", 7)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strRegionCoordinator = strRegionCoordinator & objDR.Item("RegionCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()
        End If

        If strRegionCoordinator <> "" Then
            strRegionCoordinator = Left(strRegionCoordinator, strRegionCoordinator.Length - 2)
        End If

        Return strRegionCoordinator

    End Function




    Public Function GetCountyCoordinatorByAffectedCounty() As String

        Dim strCountyCoordinator As String = ""

        If gBoolAlachua = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 1)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolBaker = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 2)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolBay = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 3)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolBradford = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 4)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolBrevard = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 5)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolBroward = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 6)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolCalhoun = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 7)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolCharlotte = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 8)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolCitrus = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 9)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolClay = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 10)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolCollier = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 11)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolColumbia = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 12)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolDeSoto = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 13)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolDixie = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 14)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolDuval = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 15)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolEscambia = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 16)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolFlagler = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 17)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolFranklin = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 18)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolGadsden = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 19)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolGilchrist = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 20)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolGlades = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 21)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolGulf = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 22)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolHamilton = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 23)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolHardee = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 24)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolHendry = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 25)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolHernando = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 26)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolHighlands = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 27)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolHillsborough = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 28)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolHolmes = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 29)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolIndianRiver = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 30)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolJackson = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 31)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolJefferson = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 32)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolLafayette = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 33)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolLake = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 34)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolLee = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 35)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolLeon = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 36)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolLevy = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 37)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolLiberty = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 38)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolMadison = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 39)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolManatee = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 40)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolMarion = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 41)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolMartin = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 42)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolMiamiDade = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 43)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolMonroe = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 44)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolNassau = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 45)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolOkaloosa = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 46)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolOkeechobee = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 47)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolOrange = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 48)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolOsceola = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 49)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolPalmBeach = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 50)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolPasco = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 51)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolPinellas = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 52)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolPolk = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 53)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolPutnam = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 54)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolSantaRosa = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 55)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolSarasota = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 56)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolSeminole = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 57)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolStJohns = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 58)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolStLucie = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 59)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolSumter = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 60)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolSuwannee = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 61)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolTaylor = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 62)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolUnion = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 63)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolVolusia = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 64)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolWakulla = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 65)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolWalton = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 66)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolWashington = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 67)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strCountyCoordinator = strCountyCoordinator & objDR.Item("CountyCoordinatorName") & ", "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If strCountyCoordinator <> "" Then
            strCountyCoordinator = Left(strCountyCoordinator, strCountyCoordinator.Length - 2)
        End If

        Return strCountyCoordinator

    End Function

    Public Function GetCountyCoordinatorEmailByAffectedCounty() As String

        Dim strEmailList As String = ""

        If gBoolAlachua = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 1)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolBaker = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 2)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolBay = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 3)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolBradford = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 4)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolBrevard = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 5)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolBroward = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 6)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolCalhoun = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 7)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolCharlotte = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 8)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolCitrus = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 9)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolClay = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 10)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolCollier = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 11)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolColumbia = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 12)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolDeSoto = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 13)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolDixie = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 14)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolDuval = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 15)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolEscambia = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 16)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolFlagler = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 17)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolFranklin = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 18)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolGadsden = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 19)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolGilchrist = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 20)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolGlades = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 21)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolGulf = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 22)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolHamilton = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 23)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolHardee = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 24)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolHendry = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 25)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolHernando = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 26)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolHighlands = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 27)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolHillsborough = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 28)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolHolmes = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 29)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolIndianRiver = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 30)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolJackson = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 31)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolJefferson = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 32)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolLafayette = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 33)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolLake = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 34)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolLee = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 35)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolLeon = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 36)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolLevy = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 37)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolLiberty = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 38)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolMadison = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 39)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolManatee = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 40)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolMarion = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 41)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolMartin = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 42)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolMiamiDade = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 43)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolMonroe = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 44)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolNassau = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 45)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolOkaloosa = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 46)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolOkeechobee = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 47)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolOrange = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 48)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolOsceola = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 49)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolPalmBeach = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 50)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolPasco = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 51)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolPinellas = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 52)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolPolk = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 53)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolPutnam = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 54)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolSantaRosa = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 55)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolSarasota = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 56)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolSeminole = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 57)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolStJohns = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 58)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolStLucie = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 59)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolSumter = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 60)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolSuwannee = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 61)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolTaylor = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 62)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolUnion = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 63)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolVolusia = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 64)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolWakulla = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 65)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolWalton = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 66)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If gBoolWashington = True Then

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            DBConStringHelper.PrepareConnection(objConn) 'open the connection
            objCmd = New SqlCommand("[spSelectCountyCoordinatorByCountyID]", objConn)
            objCmd.Parameters.AddWithValue("@CountyID", 67)
            objCmd.CommandType = CommandType.StoredProcedure
            objDR = objCmd.ExecuteReader()

            If objDR.Read() Then

                'there are records
                objDR.Close()
                objDR = objCmd.ExecuteReader()

                While objDR.Read

                    strEmailList = strEmailList & objDR.Item("Email") & "; "

                End While

            End If

            objCmd.Dispose()
            objCmd = Nothing
            objConn.Close()

        End If

        If strEmailList <> "" Then
            strEmailList = Left(strEmailList, strEmailList.Length - 2)
        End If



        Return strEmailList

    End Function


    'These are Functions that are in General and Not tied to the IncidentID
    Public Function GetCountiesByRegion(ByVal intRegionID As Integer) As String

        Dim strCounties As String = ""

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        DBConStringHelper.PrepareConnection(objConn) 'open the connection
        objCmd = New SqlCommand("[spSelectCountyByRegionID]", objConn)
        objCmd.Parameters.AddWithValue("@RegionID", intRegionID)

        objCmd.CommandType = CommandType.StoredProcedure
        objDR = objCmd.ExecuteReader()


        If objDR.Read() Then
            'there are records
            objDR.Close()
            objDR = objCmd.ExecuteReader()

            While objDR.Read

                strCounties = strCounties & HelpFunction.Convertdbnulls(objDR("County")) & ", "

            End While

        Else
            'Would Never Happen But
        End If

        objCmd.Dispose()
        objCmd = Nothing
        objConn.Close()

        'remove the last comma
        If strCounties <> "" Then
            strCounties = strCounties.Remove(strCounties.Length - 2, 2)
        End If

        Return strCounties

    End Function

    ''' <summary>
    ''' Returns a comma-delimited list of regions and counties, in that order.
    ''' Regions are only returned if all counties in the region are included.
    ''' </summary>
    ''' <param name="blnBoldRegionNames">
    ''' If true, region names are wrapped in bold tags
    ''' </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRegionAndCountyList(blnBoldRegionNames As Boolean) As String
        Dim strRegionsAndCounties As String = ""
        Dim strRegions As String = GetRegions()
        Dim strCounties As String = GetCountiesAffected() & ", "
        Dim colCountyRegions As New Dictionary(Of String, String()) 'Stores the counties in each region

        If Not String.IsNullOrEmpty(strRegions) Then
            strRegions = strRegions.Replace(",", ", ")
            strRegions = strRegions & ", "

            If System.Web.HttpContext.Current.Cache("CountyRegions") Is Nothing Then
                AddCountiesAndRegionsToCache()
            End If

            colCountyRegions = CType(System.Web.HttpContext.Current.Cache("CountyRegions"), System.Collections.Generic.Dictionary(Of String, String()))

            For Each kvp As KeyValuePair(Of String, String()) In colCountyRegions
                If strRegions.Contains(kvp.Key) Then
                    Dim arrCountiesInThisRegion As String() = kvp.Value

                    For i = 0 To arrCountiesInThisRegion.GetLength(0) - 1
                        strCounties = strCounties.Replace(arrCountiesInThisRegion(i) & ", ", "")
                    Next
                End If
            Next

            If blnBoldRegionNames Then
                strRegions = strRegions.Replace("Region 1", "<b>Region 1</b>")
                strRegions = strRegions.Replace("Region 2", "<b>Region 2</b>")
                strRegions = strRegions.Replace("Region 3", "<b>Region 3</b>")
                strRegions = strRegions.Replace("Region 4", "<b>Region 4</b>")
                strRegions = strRegions.Replace("Region 5", "<b>Region 5</b>")
                strRegions = strRegions.Replace("Region 6", "<b>Region 6</b>")
                strRegions = strRegions.Replace("Region 7", "<b>Region 7</b>")
            End If
        End If

        strCounties = strCounties.TrimEnd(","c, " "c)
        strRegionsAndCounties = strRegions & strCounties
        strRegionsAndCounties = strRegionsAndCounties.TrimEnd(","c, " "c)
        Return Trim(strRegionsAndCounties)
    End Function

    Private Sub AddCountiesAndRegionsToCache()
        Dim colCountyRegions As New Dictionary(Of String, String()) 'Stores the counties in each region
        colCountyRegions.Add("Region 1", GetCountiesByRegion(1).Replace(", ", ",").Split(","))
        colCountyRegions.Add("Region 2", GetCountiesByRegion(2).Replace(", ", ",").Split(","))
        colCountyRegions.Add("Region 3", GetCountiesByRegion(3).Replace(", ", ",").Split(","))
        colCountyRegions.Add("Region 4", GetCountiesByRegion(4).Replace(", ", ",").Split(","))
        colCountyRegions.Add("Region 5", GetCountiesByRegion(5).Replace(", ", ",").Split(","))
        colCountyRegions.Add("Region 6", GetCountiesByRegion(6).Replace(", ", ",").Split(","))
        colCountyRegions.Add("Region 7", GetCountiesByRegion(7).Replace(", ", ",").Split(","))
        System.Web.HttpContext.Current.Cache.Insert("CountyRegions", colCountyRegions)
    End Sub
End Class
