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

Partial Class AddRegionCounty
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

    Dim ParamId As SqlParameter

    Public AuditHelper As New AuditHelp

    Public MrDataGrabber As New DataGrabber

    Dim globalRecordCount As Integer
    Dim globalAuditAction As String = ""
    Dim globalHasErrors As Boolean = False
    Dim globalMessage As String
    Dim globalCurrentStep As Integer
    Dim globalIsSaved As Boolean = False
    Dim globalAction As String
    Dim globalParameter As String
    Const js As String = "TADDScript.js"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            PopulatePage()
        End If

    End Sub

    Protected Sub PopulatePage()

        Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectCountyRegionCheckCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then
                globalRecordCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))
            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

        Catch ex As Exception

            Response.Write(ex.ToString)
            Exit Sub

        End Try

        'Response.Write(globalRecordCount)
        'Response.End()

        If globalRecordCount <> 0 Then

            Try
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
                objConn.Open()
                objCmd = New SqlCommand("spSelectCountyRegionCheckByIncidentID", objConn)
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

                objDR = objCmd.ExecuteReader

                If objDR.Read() Then

                    cbxStatewide.Checked = HelpFunction.ConvertdbnullsBool(objDR("Statewide"))
                    cbxRegion1.Checked = HelpFunction.ConvertdbnullsBool(objDR("Region1"))
                    cbxRegion2.Checked = HelpFunction.ConvertdbnullsBool(objDR("Region2"))
                    cbxRegion3.Checked = HelpFunction.ConvertdbnullsBool(objDR("Region3"))
                    cbxRegion4.Checked = HelpFunction.ConvertdbnullsBool(objDR("Region4"))
                    cbxRegion5.Checked = HelpFunction.ConvertdbnullsBool(objDR("Region5"))
                    cbxRegion6.Checked = HelpFunction.ConvertdbnullsBool(objDR("Region6"))
                    cbxRegion7.Checked = HelpFunction.ConvertdbnullsBool(objDR("Region7"))
                    cbxBay.Checked = HelpFunction.ConvertdbnullsBool(objDR("Bay"))
                    cbxCalhoun.Checked = HelpFunction.ConvertdbnullsBool(objDR("Calhoun"))
                    cbxEscambia.Checked = HelpFunction.ConvertdbnullsBool(objDR("Escambia"))
                    cbxGulf.Checked = HelpFunction.ConvertdbnullsBool(objDR("Gulf"))
                    cbxHolmes.Checked = HelpFunction.ConvertdbnullsBool(objDR("Holmes"))
                    cbxJackson.Checked = HelpFunction.ConvertdbnullsBool(objDR("Jackson"))
                    cbxOkaloosa.Checked = HelpFunction.ConvertdbnullsBool(objDR("Okaloosa"))
                    cbxSantaRosa.Checked = HelpFunction.ConvertdbnullsBool(objDR("Santa Rosa"))
                    cbxWalton.Checked = HelpFunction.ConvertdbnullsBool(objDR("Walton"))
                    cbxWashington.Checked = HelpFunction.ConvertdbnullsBool(objDR("Washington"))
                    cbxColumbia.Checked = HelpFunction.ConvertdbnullsBool(objDR("Columbia"))
                    cbxDixie.Checked = HelpFunction.ConvertdbnullsBool(objDR("Dixie"))
                    cbxFranklin.Checked = HelpFunction.ConvertdbnullsBool(objDR("Franklin"))
                    cbxGadsden.Checked = HelpFunction.ConvertdbnullsBool(objDR("Gadsden"))
                    cbxHamilton.Checked = HelpFunction.ConvertdbnullsBool(objDR("Hamilton"))
                    cbxJefferson.Checked = HelpFunction.ConvertdbnullsBool(objDR("Jefferson"))
                    cbxLafayette.Checked = HelpFunction.ConvertdbnullsBool(objDR("Lafayette"))
                    cbxLeon.Checked = HelpFunction.ConvertdbnullsBool(objDR("Leon"))
                    cbxLevy.Checked = HelpFunction.ConvertdbnullsBool(objDR("Levy"))
                    cbxLiberty.Checked = HelpFunction.ConvertdbnullsBool(objDR("Liberty"))
                    cbxMadison.Checked = HelpFunction.ConvertdbnullsBool(objDR("Madison"))
                    cbxSuwannee.Checked = HelpFunction.ConvertdbnullsBool(objDR("Suwannee"))
                    cbxTaylor.Checked = HelpFunction.ConvertdbnullsBool(objDR("Taylor"))
                    cbxWakulla.Checked = HelpFunction.ConvertdbnullsBool(objDR("Wakulla"))
                    cbxAlachua.Checked = HelpFunction.ConvertdbnullsBool(objDR("Alachua"))
                    cbxBaker.Checked = HelpFunction.ConvertdbnullsBool(objDR("Baker"))
                    cbxBradford.Checked = HelpFunction.ConvertdbnullsBool(objDR("Bradford"))
                    cbxClay.Checked = HelpFunction.ConvertdbnullsBool(objDR("Clay"))
                    cbxDuval.Checked = HelpFunction.ConvertdbnullsBool(objDR("Duval"))
                    cbxFlagler.Checked = HelpFunction.ConvertdbnullsBool(objDR("Flagler"))
                    cbxGilchrist.Checked = HelpFunction.ConvertdbnullsBool(objDR("Gilchrist"))
                    cbxMarion.Checked = HelpFunction.ConvertdbnullsBool(objDR("Marion"))
                    cbxNassau.Checked = HelpFunction.ConvertdbnullsBool(objDR("Nassau"))
                    cbxPutnam.Checked = HelpFunction.ConvertdbnullsBool(objDR("Putnam"))
                    cbxStJohns.Checked = HelpFunction.ConvertdbnullsBool(objDR("St. Johns"))
                    cbxUnion.Checked = HelpFunction.ConvertdbnullsBool(objDR("Union"))
                    cbxCitrus.Checked = HelpFunction.ConvertdbnullsBool(objDR("Citrus"))
                    cbxHardee.Checked = HelpFunction.ConvertdbnullsBool(objDR("Hardee"))
                    cbxHernando.Checked = HelpFunction.ConvertdbnullsBool(objDR("Hernando"))
                    cbxHillsborough.Checked = HelpFunction.ConvertdbnullsBool(objDR("Hillsborough"))
                    cbxPasco.Checked = HelpFunction.ConvertdbnullsBool(objDR("Pasco"))
                    cbxPinellas.Checked = HelpFunction.ConvertdbnullsBool(objDR("Pinellas"))
                    cbxPolk.Checked = HelpFunction.ConvertdbnullsBool(objDR("Polk"))
                    cbxSumter.Checked = HelpFunction.ConvertdbnullsBool(objDR("Sumter"))
                    cbxBrevard.Checked = HelpFunction.ConvertdbnullsBool(objDR("Brevard"))
                    cbxIndianRiver.Checked = HelpFunction.ConvertdbnullsBool(objDR("Indian River"))
                    cbxLake.Checked = HelpFunction.ConvertdbnullsBool(objDR("Lake"))
                    cbxMartin.Checked = HelpFunction.ConvertdbnullsBool(objDR("Martin"))
                    cbxOrange.Checked = HelpFunction.ConvertdbnullsBool(objDR("Orange"))
                    cbxOsceola.Checked = HelpFunction.ConvertdbnullsBool(objDR("Osceola"))
                    cbxSeminole.Checked = HelpFunction.ConvertdbnullsBool(objDR("Seminole"))
                    cbxStLucie.Checked = HelpFunction.ConvertdbnullsBool(objDR("St. Lucie"))
                    cbxVolusia.Checked = HelpFunction.ConvertdbnullsBool(objDR("Volusia"))
                    cbxCharlotte.Checked = HelpFunction.ConvertdbnullsBool(objDR("Charlotte"))
                    cbxCollier.Checked = HelpFunction.ConvertdbnullsBool(objDR("Collier"))
                    cbxDeSoto.Checked = HelpFunction.ConvertdbnullsBool(objDR("DeSoto"))
                    cbxGlades.Checked = HelpFunction.ConvertdbnullsBool(objDR("Glades"))
                    cbxHendry.Checked = HelpFunction.ConvertdbnullsBool(objDR("Hendry"))
                    cbxHighlands.Checked = HelpFunction.ConvertdbnullsBool(objDR("Highlands"))
                    cbxLee.Checked = HelpFunction.ConvertdbnullsBool(objDR("Lee"))
                    cbxManatee.Checked = HelpFunction.ConvertdbnullsBool(objDR("Manatee"))
                    cbxOkeechobee.Checked = HelpFunction.ConvertdbnullsBool(objDR("Okeechobee"))
                    cbxSarasota.Checked = HelpFunction.ConvertdbnullsBool(objDR("Sarasota"))
                    cbxBroward.Checked = HelpFunction.ConvertdbnullsBool(objDR("Broward"))
                    cbxMiamiDade.Checked = HelpFunction.ConvertdbnullsBool(objDR("Miami-Dade"))
                    cbxMonroe.Checked = HelpFunction.ConvertdbnullsBool(objDR("Monroe"))
                    cbxPalmBeach.Checked = HelpFunction.ConvertdbnullsBool(objDR("Palm Beach"))
                End If

                objDR.Close()

                objCmd.Dispose()
                objCmd = Nothing

                objConn.Close()
            Catch ex As Exception
                Response.Write(ex.ToString)
                Exit Sub
            End Try

        End If

    End Sub


    'CheckBox Changes
    Protected Sub cbxStatewide_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxStatewide.CheckedChanged

        If cbxStatewide.Checked = True Then

            cbxBay.Checked = True
            cbxCalhoun.Checked = True
            cbxEscambia.Checked = True
            cbxGulf.Checked = True
            cbxHolmes.Checked = True
            cbxJackson.Checked = True
            cbxOkaloosa.Checked = True
            cbxSantaRosa.Checked = True
            cbxWalton.Checked = True
            cbxWashington.Checked = True
            cbxColumbia.Checked = True
            cbxDixie.Checked = True
            cbxFranklin.Checked = True
            cbxGadsden.Checked = True
            cbxHamilton.Checked = True
            cbxJefferson.Checked = True
            cbxLafayette.Checked = True
            cbxLeon.Checked = True
            cbxLevy.Checked = True
            cbxLiberty.Checked = True
            cbxMadison.Checked = True
            cbxSuwannee.Checked = True
            cbxTaylor.Checked = True
            cbxWakulla.Checked = True
            cbxAlachua.Checked = True
            cbxBaker.Checked = True
            cbxBradford.Checked = True
            cbxClay.Checked = True
            cbxDuval.Checked = True
            cbxFlagler.Checked = True
            cbxGilchrist.Checked = True
            cbxMarion.Checked = True
            cbxNassau.Checked = True
            cbxPutnam.Checked = True
            cbxStJohns.Checked = True
            cbxUnion.Checked = True
            cbxCitrus.Checked = True
            cbxHardee.Checked = True
            cbxHernando.Checked = True
            cbxHillsborough.Checked = True
            cbxPasco.Checked = True
            cbxPinellas.Checked = True
            cbxPolk.Checked = True
            cbxSumter.Checked = True
            cbxBrevard.Checked = True
            cbxIndianRiver.Checked = True
            cbxLake.Checked = True
            cbxMartin.Checked = True
            cbxOrange.Checked = True
            cbxOsceola.Checked = True
            cbxSeminole.Checked = True
            cbxStLucie.Checked = True
            cbxVolusia.Checked = True
            cbxCharlotte.Checked = True
            cbxCollier.Checked = True
            cbxDeSoto.Checked = True
            cbxGlades.Checked = True
            cbxHendry.Checked = True
            cbxHighlands.Checked = True
            cbxLee.Checked = True
            cbxManatee.Checked = True
            cbxOkeechobee.Checked = True
            cbxSarasota.Checked = True
            cbxBroward.Checked = True
            cbxMiamiDade.Checked = True
            cbxMonroe.Checked = True
            cbxPalmBeach.Checked = True

            cbxRegion1.Checked = True
            cbxRegion2.Checked = True
            cbxRegion3.Checked = True
            cbxRegion4.Checked = True
            cbxRegion5.Checked = True
            cbxRegion6.Checked = True
            cbxRegion7.Checked = True

        Else

            cbxBay.Checked = False
            cbxCalhoun.Checked = False
            cbxEscambia.Checked = False
            cbxGulf.Checked = False
            cbxHolmes.Checked = False
            cbxJackson.Checked = False
            cbxOkaloosa.Checked = False
            cbxSantaRosa.Checked = False
            cbxWalton.Checked = False
            cbxWashington.Checked = False
            cbxColumbia.Checked = False
            cbxDixie.Checked = False
            cbxFranklin.Checked = False
            cbxGadsden.Checked = False
            cbxHamilton.Checked = False
            cbxJefferson.Checked = False
            cbxLafayette.Checked = False
            cbxLeon.Checked = False
            cbxLevy.Checked = False
            cbxLiberty.Checked = False
            cbxMadison.Checked = False
            cbxSuwannee.Checked = False
            cbxTaylor.Checked = False
            cbxWakulla.Checked = False
            cbxAlachua.Checked = False
            cbxBaker.Checked = False
            cbxBradford.Checked = False
            cbxClay.Checked = False
            cbxDuval.Checked = False
            cbxFlagler.Checked = False
            cbxGilchrist.Checked = False
            cbxMarion.Checked = False
            cbxNassau.Checked = False
            cbxPutnam.Checked = False
            cbxStJohns.Checked = False
            cbxUnion.Checked = False
            cbxCitrus.Checked = False
            cbxHardee.Checked = False
            cbxHernando.Checked = False
            cbxHillsborough.Checked = False
            cbxPasco.Checked = False
            cbxPinellas.Checked = False
            cbxPolk.Checked = False
            cbxSumter.Checked = False
            cbxBrevard.Checked = False
            cbxIndianRiver.Checked = False
            cbxLake.Checked = False
            cbxMartin.Checked = False
            cbxOrange.Checked = False
            cbxOsceola.Checked = False
            cbxSeminole.Checked = False
            cbxStLucie.Checked = False
            cbxVolusia.Checked = False
            cbxCharlotte.Checked = False
            cbxCollier.Checked = False
            cbxDeSoto.Checked = False
            cbxGlades.Checked = False
            cbxHendry.Checked = False
            cbxHighlands.Checked = False
            cbxLee.Checked = False
            cbxManatee.Checked = False
            cbxOkeechobee.Checked = False
            cbxSarasota.Checked = False
            cbxBroward.Checked = False
            cbxMiamiDade.Checked = False
            cbxMonroe.Checked = False
            cbxPalmBeach.Checked = False

            cbxRegion1.Checked = False
            cbxRegion2.Checked = False
            cbxRegion3.Checked = False
            cbxRegion4.Checked = False
            cbxRegion5.Checked = False
            cbxRegion6.Checked = False
            cbxRegion7.Checked = False

        End If

    End Sub

    Protected Sub cbxRegion1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxRegion1.CheckedChanged

        If cbxRegion1.Checked = True Then

            cbxBay.Checked = True
            cbxCalhoun.Checked = True
            cbxEscambia.Checked = True
            cbxGulf.Checked = True
            cbxHolmes.Checked = True
            cbxJackson.Checked = True
            cbxOkaloosa.Checked = True
            cbxSantaRosa.Checked = True
            cbxWalton.Checked = True
            cbxWashington.Checked = True

        Else

            cbxBay.Checked = False
            cbxCalhoun.Checked = False
            cbxEscambia.Checked = False
            cbxGulf.Checked = False
            cbxHolmes.Checked = False
            cbxJackson.Checked = False
            cbxOkaloosa.Checked = False
            cbxSantaRosa.Checked = False
            cbxWalton.Checked = False
            cbxWashington.Checked = False

        End If

    End Sub

    Protected Sub cbxRegion2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxRegion2.CheckedChanged


        If cbxRegion2.Checked = True Then

            cbxColumbia.Checked = True
            cbxDixie.Checked = True
            cbxFranklin.Checked = True
            cbxGadsden.Checked = True
            cbxHamilton.Checked = True
            cbxJefferson.Checked = True
            cbxLafayette.Checked = True
            cbxLeon.Checked = True
            cbxLevy.Checked = True
            cbxLiberty.Checked = True
            cbxMadison.Checked = True
            cbxSuwannee.Checked = True
            cbxTaylor.Checked = True
            cbxWakulla.Checked = True

        Else

            cbxColumbia.Checked = False
            cbxDixie.Checked = False
            cbxFranklin.Checked = False
            cbxGadsden.Checked = False
            cbxHamilton.Checked = False
            cbxJefferson.Checked = False
            cbxLafayette.Checked = False
            cbxLeon.Checked = False
            cbxLevy.Checked = False
            cbxLiberty.Checked = False
            cbxMadison.Checked = False
            cbxSuwannee.Checked = False
            cbxTaylor.Checked = False
            cbxWakulla.Checked = False

        End If


    End Sub

    Protected Sub cbxRegion3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxRegion3.CheckedChanged

        If cbxRegion3.Checked = True Then


            cbxAlachua.Checked = True
            cbxBaker.Checked = True
            cbxBradford.Checked = True
            cbxClay.Checked = True
            cbxDuval.Checked = True
            cbxFlagler.Checked = True
            cbxGilchrist.Checked = True
            cbxMarion.Checked = True
            cbxNassau.Checked = True
            cbxPutnam.Checked = True
            cbxStJohns.Checked = True
            cbxUnion.Checked = True

        Else

            cbxAlachua.Checked = False
            cbxBaker.Checked = False
            cbxBradford.Checked = False
            cbxClay.Checked = False
            cbxDuval.Checked = False
            cbxFlagler.Checked = False
            cbxGilchrist.Checked = False
            cbxMarion.Checked = False
            cbxNassau.Checked = False
            cbxPutnam.Checked = False
            cbxStJohns.Checked = False
            cbxUnion.Checked = False

        End If

    End Sub

    Protected Sub cbxRegion4_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxRegion4.CheckedChanged

        If cbxRegion4.Checked = True Then


            cbxCitrus.Checked = True
            cbxHardee.Checked = True
            cbxHernando.Checked = True
            cbxHillsborough.Checked = True
            cbxPasco.Checked = True
            cbxPinellas.Checked = True
            cbxPolk.Checked = True
            cbxSumter.Checked = True

        Else

            cbxCitrus.Checked = False
            cbxHardee.Checked = False
            cbxHernando.Checked = False
            cbxHillsborough.Checked = False
            cbxPasco.Checked = False
            cbxPinellas.Checked = False
            cbxPolk.Checked = False
            cbxSumter.Checked = False

        End If

    End Sub

    Protected Sub cbxRegion5_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxRegion5.CheckedChanged

        If cbxRegion5.Checked = True Then

            cbxBrevard.Checked = True
            cbxIndianRiver.Checked = True
            cbxLake.Checked = True
            cbxMartin.Checked = True
            cbxOrange.Checked = True
            cbxOsceola.Checked = True
            cbxSeminole.Checked = True
            cbxStLucie.Checked = True
            cbxVolusia.Checked = True

        Else

            cbxBrevard.Checked = False
            cbxIndianRiver.Checked = False
            cbxLake.Checked = False
            cbxMartin.Checked = False
            cbxOrange.Checked = False
            cbxOsceola.Checked = False
            cbxSeminole.Checked = False
            cbxStLucie.Checked = False
            cbxVolusia.Checked = False

        End If

    End Sub

    Protected Sub cbxRegion6_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxRegion6.CheckedChanged

        If cbxRegion6.Checked = True Then

            cbxCharlotte.Checked = True
            cbxCollier.Checked = True
            cbxDeSoto.Checked = True
            cbxGlades.Checked = True
            cbxHendry.Checked = True
            cbxHighlands.Checked = True
            cbxLee.Checked = True
            cbxManatee.Checked = True
            cbxOkeechobee.Checked = True
            cbxSarasota.Checked = True

        Else

            cbxCharlotte.Checked = False
            cbxCollier.Checked = False
            cbxDeSoto.Checked = False
            cbxGlades.Checked = False
            cbxHendry.Checked = False
            cbxHighlands.Checked = False
            cbxLee.Checked = False
            cbxManatee.Checked = False
            cbxOkeechobee.Checked = False
            cbxSarasota.Checked = False

        End If

    End Sub

    Protected Sub cbxRegion7_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxRegion7.CheckedChanged

        If cbxRegion7.Checked = True Then

            cbxBroward.Checked = True
            cbxMiamiDade.Checked = True
            cbxMonroe.Checked = True
            cbxPalmBeach.Checked = True

        Else

            cbxBroward.Checked = False
            cbxMiamiDade.Checked = False
            cbxMonroe.Checked = False
            cbxPalmBeach.Checked = False

        End If

    End Sub


    Protected Sub Save()

        Try

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectCountyRegionCheckCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                globalRecordCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

        Catch ex As Exception

            Response.Write(ex.ToString)
            Exit Sub

        End Try

        'Response.Write(globalRecordCount)
        'Response.End()

        If globalRecordCount = 0 Then

            'We Add

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionCountyRegionCheck", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@CountyRegionCheckID", 0)
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@StateWide", cbxStatewide.Checked)
            objCmd.Parameters.AddWithValue("@Region1", cbxRegion1.Checked)
            objCmd.Parameters.AddWithValue("@Region2", cbxRegion2.Checked)
            objCmd.Parameters.AddWithValue("@Region3", cbxRegion3.Checked)
            objCmd.Parameters.AddWithValue("@Region4", cbxRegion4.Checked)
            objCmd.Parameters.AddWithValue("@Region5", cbxRegion5.Checked)
            objCmd.Parameters.AddWithValue("@Region6", cbxRegion6.Checked)
            objCmd.Parameters.AddWithValue("@Region7", cbxRegion7.Checked)
            objCmd.Parameters.AddWithValue("@Bay", cbxBay.Checked)
            objCmd.Parameters.AddWithValue("@Calhoun", cbxCalhoun.Checked)
            objCmd.Parameters.AddWithValue("@Escambia", cbxEscambia.Checked)
            objCmd.Parameters.AddWithValue("@Gulf", cbxGulf.Checked)
            objCmd.Parameters.AddWithValue("@Holmes", cbxHolmes.Checked)
            objCmd.Parameters.AddWithValue("@Jackson", cbxJackson.Checked)
            objCmd.Parameters.AddWithValue("@Okaloosa", cbxOkaloosa.Checked)
            objCmd.Parameters.AddWithValue("@SantaRosa", cbxSantaRosa.Checked)
            objCmd.Parameters.AddWithValue("@Walton", cbxWalton.Checked)
            objCmd.Parameters.AddWithValue("@Washington", cbxWashington.Checked)
            objCmd.Parameters.AddWithValue("@Columbia", cbxColumbia.Checked)
            objCmd.Parameters.AddWithValue("@Dixie", cbxDixie.Checked)
            objCmd.Parameters.AddWithValue("@Franklin", cbxFranklin.Checked)
            objCmd.Parameters.AddWithValue("@Gadsden", cbxGadsden.Checked)
            objCmd.Parameters.AddWithValue("@Hamilton", cbxHamilton.Checked)
            objCmd.Parameters.AddWithValue("@Jefferson", cbxJefferson.Checked)
            objCmd.Parameters.AddWithValue("@Lafayette", cbxLafayette.Checked)
            objCmd.Parameters.AddWithValue("@Leon", cbxLeon.Checked)
            objCmd.Parameters.AddWithValue("@Levy", cbxLevy.Checked)
            objCmd.Parameters.AddWithValue("@Liberty", cbxLiberty.Checked)
            objCmd.Parameters.AddWithValue("@Madison", cbxMadison.Checked)
            objCmd.Parameters.AddWithValue("@Suwannee", cbxSuwannee.Checked)
            objCmd.Parameters.AddWithValue("@Taylor", cbxTaylor.Checked)
            objCmd.Parameters.AddWithValue("@Wakulla", cbxWakulla.Checked)
            objCmd.Parameters.AddWithValue("@Alachua", cbxAlachua.Checked)
            objCmd.Parameters.AddWithValue("@Baker", cbxBaker.Checked)
            objCmd.Parameters.AddWithValue("@Bradford", cbxBradford.Checked)
            objCmd.Parameters.AddWithValue("@Clay", cbxClay.Checked)
            objCmd.Parameters.AddWithValue("@Duval", cbxDuval.Checked)
            objCmd.Parameters.AddWithValue("@Flagler", cbxFlagler.Checked)
            objCmd.Parameters.AddWithValue("@Gilchrist", cbxGilchrist.Checked)
            objCmd.Parameters.AddWithValue("@Marion", cbxMarion.Checked)
            objCmd.Parameters.AddWithValue("@Nassau", cbxNassau.Checked)
            objCmd.Parameters.AddWithValue("@Putnam", cbxPutnam.Checked)
            objCmd.Parameters.AddWithValue("@StJohns", cbxStJohns.Checked)
            objCmd.Parameters.AddWithValue("@Union", cbxUnion.Checked)
            objCmd.Parameters.AddWithValue("@Citrus", cbxCitrus.Checked)
            objCmd.Parameters.AddWithValue("@Hardee", cbxHardee.Checked)
            objCmd.Parameters.AddWithValue("@Hernando", cbxHernando.Checked)
            objCmd.Parameters.AddWithValue("@Hillsborough", cbxHillsborough.Checked)
            objCmd.Parameters.AddWithValue("@Pasco", cbxPasco.Checked)
            objCmd.Parameters.AddWithValue("@Pinellas", cbxPinellas.Checked)
            objCmd.Parameters.AddWithValue("@Polk", cbxPolk.Checked)
            objCmd.Parameters.AddWithValue("@Sumter", cbxSumter.Checked)
            objCmd.Parameters.AddWithValue("@Brevard", cbxBrevard.Checked)
            objCmd.Parameters.AddWithValue("@IndianRiver", cbxIndianRiver.Checked)
            objCmd.Parameters.AddWithValue("@Lake", cbxLake.Checked)
            objCmd.Parameters.AddWithValue("@Martin", cbxMartin.Checked)
            objCmd.Parameters.AddWithValue("@Orange", cbxOrange.Checked)
            objCmd.Parameters.AddWithValue("@Osceola", cbxOsceola.Checked)
            objCmd.Parameters.AddWithValue("@Seminole", cbxSeminole.Checked)
            objCmd.Parameters.AddWithValue("@StLucie", cbxStLucie.Checked)
            objCmd.Parameters.AddWithValue("@Volusia", cbxVolusia.Checked)
            objCmd.Parameters.AddWithValue("@Charlotte", cbxCharlotte.Checked)
            objCmd.Parameters.AddWithValue("@Collier", cbxCollier.Checked)
            objCmd.Parameters.AddWithValue("@DeSoto", cbxDeSoto.Checked)
            objCmd.Parameters.AddWithValue("@Glades", cbxGlades.Checked)
            objCmd.Parameters.AddWithValue("@Hendry", cbxHendry.Checked)
            objCmd.Parameters.AddWithValue("@Highlands", cbxHighlands.Checked)
            objCmd.Parameters.AddWithValue("@Lee", cbxLee.Checked)
            objCmd.Parameters.AddWithValue("@Manatee", cbxManatee.Checked)
            objCmd.Parameters.AddWithValue("@Okeechobee", cbxOkeechobee.Checked)
            objCmd.Parameters.AddWithValue("@Sarasota", cbxSarasota.Checked)
            objCmd.Parameters.AddWithValue("@Broward", cbxBroward.Checked)
            objCmd.Parameters.AddWithValue("@MiamiDade", cbxMiamiDade.Checked)
            objCmd.Parameters.AddWithValue("@Monroe", cbxMonroe.Checked)
            objCmd.Parameters.AddWithValue("@PalmBeach", cbxPalmBeach.Checked)


            '// Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            '// Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()
            '// Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)



        Else
            'We update


            Dim localCountyRegionCheckID As Integer = MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("CountyRegionCheckID", "CountyRegionCheck", "IncidentID", Request("IncidentID"))

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionCountyRegionCheck", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@CountyRegionCheckID", localCountyRegionCheckID)
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@StateWide", cbxStatewide.Checked)
            objCmd.Parameters.AddWithValue("@Region1", cbxRegion1.Checked)
            objCmd.Parameters.AddWithValue("@Region2", cbxRegion2.Checked)
            objCmd.Parameters.AddWithValue("@Region3", cbxRegion3.Checked)
            objCmd.Parameters.AddWithValue("@Region4", cbxRegion4.Checked)
            objCmd.Parameters.AddWithValue("@Region5", cbxRegion5.Checked)
            objCmd.Parameters.AddWithValue("@Region6", cbxRegion6.Checked)
            objCmd.Parameters.AddWithValue("@Region7", cbxRegion7.Checked)
            objCmd.Parameters.AddWithValue("@Bay", cbxBay.Checked)
            objCmd.Parameters.AddWithValue("@Calhoun", cbxCalhoun.Checked)
            objCmd.Parameters.AddWithValue("@Escambia", cbxEscambia.Checked)
            objCmd.Parameters.AddWithValue("@Gulf", cbxGulf.Checked)
            objCmd.Parameters.AddWithValue("@Holmes", cbxHolmes.Checked)
            objCmd.Parameters.AddWithValue("@Jackson", cbxJackson.Checked)
            objCmd.Parameters.AddWithValue("@Okaloosa", cbxOkaloosa.Checked)
            objCmd.Parameters.AddWithValue("@SantaRosa", cbxSantaRosa.Checked)
            objCmd.Parameters.AddWithValue("@Walton", cbxWalton.Checked)
            objCmd.Parameters.AddWithValue("@Washington", cbxWashington.Checked)
            objCmd.Parameters.AddWithValue("@Columbia", cbxColumbia.Checked)
            objCmd.Parameters.AddWithValue("@Dixie", cbxDixie.Checked)
            objCmd.Parameters.AddWithValue("@Franklin", cbxFranklin.Checked)
            objCmd.Parameters.AddWithValue("@Gadsden", cbxGadsden.Checked)
            objCmd.Parameters.AddWithValue("@Hamilton", cbxHamilton.Checked)
            objCmd.Parameters.AddWithValue("@Jefferson", cbxJefferson.Checked)
            objCmd.Parameters.AddWithValue("@Lafayette", cbxLafayette.Checked)
            objCmd.Parameters.AddWithValue("@Leon", cbxLeon.Checked)
            objCmd.Parameters.AddWithValue("@Levy", cbxLevy.Checked)
            objCmd.Parameters.AddWithValue("@Liberty", cbxLiberty.Checked)
            objCmd.Parameters.AddWithValue("@Madison", cbxMadison.Checked)
            objCmd.Parameters.AddWithValue("@Suwannee", cbxSuwannee.Checked)
            objCmd.Parameters.AddWithValue("@Taylor", cbxTaylor.Checked)
            objCmd.Parameters.AddWithValue("@Wakulla", cbxWakulla.Checked)
            objCmd.Parameters.AddWithValue("@Alachua", cbxAlachua.Checked)
            objCmd.Parameters.AddWithValue("@Baker", cbxBaker.Checked)
            objCmd.Parameters.AddWithValue("@Bradford", cbxBradford.Checked)
            objCmd.Parameters.AddWithValue("@Clay", cbxClay.Checked)
            objCmd.Parameters.AddWithValue("@Duval", cbxDuval.Checked)
            objCmd.Parameters.AddWithValue("@Flagler", cbxFlagler.Checked)
            objCmd.Parameters.AddWithValue("@Gilchrist", cbxGilchrist.Checked)
            objCmd.Parameters.AddWithValue("@Marion", cbxMarion.Checked)
            objCmd.Parameters.AddWithValue("@Nassau", cbxNassau.Checked)
            objCmd.Parameters.AddWithValue("@Putnam", cbxPutnam.Checked)
            objCmd.Parameters.AddWithValue("@StJohns", cbxStJohns.Checked)
            objCmd.Parameters.AddWithValue("@Union", cbxUnion.Checked)
            objCmd.Parameters.AddWithValue("@Citrus", cbxCitrus.Checked)
            objCmd.Parameters.AddWithValue("@Hardee", cbxHardee.Checked)
            objCmd.Parameters.AddWithValue("@Hernando", cbxHernando.Checked)
            objCmd.Parameters.AddWithValue("@Hillsborough", cbxHillsborough.Checked)
            objCmd.Parameters.AddWithValue("@Pasco", cbxPasco.Checked)
            objCmd.Parameters.AddWithValue("@Pinellas", cbxPinellas.Checked)
            objCmd.Parameters.AddWithValue("@Polk", cbxPolk.Checked)
            objCmd.Parameters.AddWithValue("@Sumter", cbxSumter.Checked)
            objCmd.Parameters.AddWithValue("@Brevard", cbxBrevard.Checked)
            objCmd.Parameters.AddWithValue("@IndianRiver", cbxIndianRiver.Checked)
            objCmd.Parameters.AddWithValue("@Lake", cbxLake.Checked)
            objCmd.Parameters.AddWithValue("@Martin", cbxMartin.Checked)
            objCmd.Parameters.AddWithValue("@Orange", cbxOrange.Checked)
            objCmd.Parameters.AddWithValue("@Osceola", cbxOsceola.Checked)
            objCmd.Parameters.AddWithValue("@Seminole", cbxSeminole.Checked)
            objCmd.Parameters.AddWithValue("@StLucie", cbxStLucie.Checked)
            objCmd.Parameters.AddWithValue("@Volusia", cbxVolusia.Checked)
            objCmd.Parameters.AddWithValue("@Charlotte", cbxCharlotte.Checked)
            objCmd.Parameters.AddWithValue("@Collier", cbxCollier.Checked)
            objCmd.Parameters.AddWithValue("@DeSoto", cbxDeSoto.Checked)
            objCmd.Parameters.AddWithValue("@Glades", cbxGlades.Checked)
            objCmd.Parameters.AddWithValue("@Hendry", cbxHendry.Checked)
            objCmd.Parameters.AddWithValue("@Highlands", cbxHighlands.Checked)
            objCmd.Parameters.AddWithValue("@Lee", cbxLee.Checked)
            objCmd.Parameters.AddWithValue("@Manatee", cbxManatee.Checked)
            objCmd.Parameters.AddWithValue("@Okeechobee", cbxOkeechobee.Checked)
            objCmd.Parameters.AddWithValue("@Sarasota", cbxSarasota.Checked)
            objCmd.Parameters.AddWithValue("@Broward", cbxBroward.Checked)
            objCmd.Parameters.AddWithValue("@MiamiDade", cbxMiamiDade.Checked)
            objCmd.Parameters.AddWithValue("@Monroe", cbxMonroe.Checked)
            objCmd.Parameters.AddWithValue("@PalmBeach", cbxPalmBeach.Checked)


            '// Open the connection using the connection string.
            DBConStringHelper.PrepareConnection(objConn)

            '// Execute the command to the DataReader.
            objCmd.ExecuteNonQuery()
            '// Clean up our command objects and close the connection.
            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

        End If

        Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID"))

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID"))
    End Sub

    Protected Sub btnCancel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel2.Click
        Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID"))
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Save()
    End Sub

    Protected Sub btnSave2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave2.Click
        Save()
    End Sub

End Class
