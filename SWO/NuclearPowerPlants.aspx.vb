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

Partial Class NuclearPowerPlants
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

    'Page Load
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)
        ns = Session("Security_Tracker")

        Select Case ns.UserLevelID.ToString() 'oCookie.Item("UserLevelID")

            Case "1" 'Admin

            Case "2" 'Full User

            Case "3" 'Update User

                btnSave.Disabled = True

            Case "4", "5" 'Read Only and Read Only + Hazmat

                btnSave.Disabled = True

            Case Else

        End Select

        If Page.IsPostBack = False Then

            'set message
            globalMessage = Request("Message")
            globalAction = Request("Action")
            globalParameter = Request("Parameter")

            PopulateDDLs()

            Dim localNPPCount As Integer = 0

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objConn.Open()
            objCmd = New SqlCommand("spSelectNPPCountByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

            objDR = objCmd.ExecuteReader

            If objDR.Read() Then

                localNPPCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

            End If

            objDR.Close()

            objCmd.Dispose()
            objCmd = Nothing

            objConn.Close()

            'Response.Write(localBombThreatDeviceCount)
            'Response.End()

            If localNPPCount > 0 Then
                PopulatePage()
            End If

        End If

    End Sub


    'PagePopulation
    Protected Sub PopulatePage()

        Dim localTime As String = ""
        Dim localTime2 As String = ""
        Dim localTime3 As String = ""
        Dim localTime4 As String = ""
        Dim localTime5 As String = ""
        Dim localTime6 As String = ""
        Dim localTime7 As String = ""
        Dim localTime8 As String = ""
        Dim localTime9 As String = ""
        Dim localTime10 As String = ""
        Dim localTime11 As String = ""
        Dim localTime12 As String = ""
        Dim localTime13 As String = ""
        Dim localTime14 As String = ""
        Dim strCRDcontactTime As String = ""
        Dim strCRDemClassTime As String = ""
        Dim strCRDemTermTime As String = ""
        Dim strCRDmessageRecdTime As String = ""


        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectNPPByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            ddlSubType.SelectedValue = HelpFunction.Convertdbnulls(objDR("SubType"))
            ddlSituation.SelectedValue = HelpFunction.Convertdbnulls(objDR("Situation"))
            ddlNotification.SelectedValue = HelpFunction.Convertdbnulls(objDR("IncidentTypeLevelID"))

            ' ddlCSTselectOne.SelectedValue = HelpFunction.Convertdbnulls(objDR("CSTselectOne"))

            'Select one Replaced
            If (HelpFunction.Convertdbnulls(objDR("CSTselectOne")) = "This is a DRILL" Or HelpFunction.Convertdbnulls(objDR("CSTselectOne")) = "") Then
                rdoDrill.Checked = True
            ElseIf (HelpFunction.Convertdbnulls(objDR("CSTselectOne")) = "This is an EMERGENCY") Then
                rdoEvent.Checked = True
            End If

            If (HelpFunction.Convertdbnulls(objDR("ShowPlantConditions")) = "1") Then
                rdoShowPlantConditionsYes.Checked = True
            ElseIf (HelpFunction.Convertdbnulls(objDR("ShowPlantConditions")) = "0") Then
                rdoShowPlantConditionsNo.Checked = True
            End If

            'Added Verification 12/16/2019
            If (HelpFunction.Convertdbnulls(objDR("VerificationStateWatchOffice")) = "1") Then
                chkStateWatchOffice.Checked = True
            End If
            If (HelpFunction.Convertdbnulls(objDR("VerificationDOH")) = "1") Then
                chkDOH.Checked = True
            End If

            If (HelpFunction.Convertdbnulls(objDR("VerificationStLucie")) = "1") Then
                chkStLucieCo.Checked = True
            End If

            If (HelpFunction.Convertdbnulls(objDR("VerificationMartinCo")) = "1") Then
                chkMartinCo.Checked = True
            End If
            If (HelpFunction.Convertdbnulls(objDR("VerificationMiamiDade")) = "1") Then
                chkMiamiDade.Checked = True
            End If
            If (HelpFunction.Convertdbnulls(objDR("VerificationMonroeCo")) = "1") Then
                chkMonroeCo.Checked = True
            End If


            If (HelpFunction.Convertdbnulls(objDR("Papa1")) = "1") Then
                chkPapa1.Checked = True
            End If
            If (HelpFunction.Convertdbnulls(objDR("Papa2")) = "1") Then
                chkPapa2.Checked = True
            End If
            If (HelpFunction.Convertdbnulls(objDR("Papa3")) = "1") Then
                chkPapa3.Checked = True
            End If
            If (HelpFunction.Convertdbnulls(objDR("Delta1")) = "1") Then
                chkDelta1.Checked = True
            End If
            If (HelpFunction.Convertdbnulls(objDR("Delta2")) = "1") Then
                chkDelta2.Checked = True
            End If
            If (HelpFunction.Convertdbnulls(objDR("Delta3")) = "1") Then
                chkDelta3.Checked = True
            End If
            If (HelpFunction.Convertdbnulls(objDR("Potassium")) = "1") Then
                chkPotassium.Checked = True
            End If


            txtCSTdate.Text = HelpFunction.Convertdbnulls(objDR("CSTdate"))
            localTime = CStr(HelpFunction.Convertdbnulls(objDR("CSTcontactTime")))
            txtCSTreportedByName.Text = HelpFunction.Convertdbnulls(objDR("CSTreportedByName"))
            txtmessageNumber.Text = HelpFunction.Convertdbnulls(objDR("CSTmessageNumber"))
            'ddlCSTreportedFrom.SelectedValue = HelpFunction.Convertdbnulls(objDR("CSTreportedFrom"))
            'ddlCSTfSelectOne.SelectedValue = HelpFunction.Convertdbnulls(objDR("CSTfSelectOne"))

            If HelpFunction.Convertdbnulls(objDR("CSTsite")) = "St. Lucie Unit 1" Or HelpFunction.Convertdbnulls(objDR("CSTsite")) = "B. St. Lucie Unit 1" Or HelpFunction.Convertdbnulls(objDR("SiteLucie1")) = "1" Then
                rdoStLucieUnit1.Checked = True
            End If
            If HelpFunction.Convertdbnulls(objDR("CSTsite")) = "St. Lucie Unit 2" Or HelpFunction.Convertdbnulls(objDR("CSTsite")) = "C. St. Lucie Unit 2" Or HelpFunction.Convertdbnulls(objDR("SiteLucie2")) = "1" Then
                rdoStLucieUnit2.Checked = True
            End If
            If HelpFunction.Convertdbnulls(objDR("CSTsite")) = "Turkey Point Unit 3" Or HelpFunction.Convertdbnulls(objDR("CSTsite")) = "D. Turkey Point Unit 3" Or HelpFunction.Convertdbnulls(objDR("SiteTurkey1")) = "1" Then
                rdoTurkeyPointUnit3.Checked = True
            End If
            If HelpFunction.Convertdbnulls(objDR("CSTsite")) = "Turkey Point Unit 4" Or HelpFunction.Convertdbnulls(objDR("CSTsite")) = "E. Turkey Point Unit 4" Or HelpFunction.Convertdbnulls(objDR("SiteTurkey2")) = "1" Then
                rdoTurkeyPointUnit4.Checked = True
            End If

            ' ddlCSTsite.SelectedValue = HelpFunction.Convertdbnulls(objDR("CSTsite"))

            If HelpFunction.Convertdbnulls(objDR("CSTemergencyClassification")) = "A. Notification of Unusual Event" Then
                rdoNotificationOfUnusualEvent.Checked = True
            ElseIf HelpFunction.Convertdbnulls(objDR("CSTemergencyClassification")) = "B. Alert" Then
                rdoAlert.Checked = True
            ElseIf HelpFunction.Convertdbnulls(objDR("CSTemergencyClassification")) = "C. Site Area Emergency" Then
                rdoSiteEmergencyArea.Checked = True
            ElseIf HelpFunction.Convertdbnulls(objDR("CSTemergencyClassification")) = "D. General Emergency" Then
                rdoGeneralEmergency.Checked = True
            End If
            '  ddlCSTemergencyClassification.SelectedValue = HelpFunction.Convertdbnulls(objDR("CSTemergencyClassification"))

            If HelpFunction.Convertdbnulls(objDR("CSTdecTermSelectOne")) = "A. Emergency Declaration" Then
                rdoEmergencyDeclaration.Checked = True
            ElseIf HelpFunction.Convertdbnulls(objDR("CSTdecTermSelectOne")) = "B. Emergency Termination" Then
                rdoEmergencyTermination.Checked = True
            End If
            'ddlCSTdecTermSelectOne.SelectedValue = HelpFunction.Convertdbnulls(objDR("CSTdecTermSelectOne"))

            txtCSTdecTermDate.Text = HelpFunction.Convertdbnulls(objDR("CSTdecTermDate"))
            localTime2 = CStr(HelpFunction.Convertdbnulls(objDR("CSTdecTermTime")))
            ' ddlCSTdecTermReason.SelectedValue = HelpFunction.Convertdbnulls(objDR("CSTdecTermReason"))
            txtCSTeALNumbers.Text = HelpFunction.Convertdbnulls(objDR("CSTeALNumbers"))
            txtCSTeALDescription.Text = HelpFunction.Convertdbnulls(objDR("CSTeALDescription"))

            If HelpFunction.Convertdbnulls(objDR("CSTeALai")) = "A. None" Then
                rdoAdditionalInformationNotApplicable.Checked = True
            Else
                rdoAdditionalInformationDose.Checked = True
            End If
            'ddlCSTeALai.SelectedValue = HelpFunction.Convertdbnulls(objDR("CSTeALai"))

            ' txtCSTeALaiDescription.Text = HelpFunction.Convertdbnulls(objDR("CSTeALaiDescription"))
            txtCSTwindDirectionDegrees.Text = HelpFunction.Convertdbnulls(objDR("CSTwindDirectionDegrees"))
            txtCSTdownwindSectorsAffected.Text = HelpFunction.Convertdbnulls(objDR("CSTdownwindSectorsAffected"))


            If HelpFunction.Convertdbnulls(objDR("CSTreleaseStatus")) = "A. None (Go to item 11)" Then
                rdoNone.Checked = True
            ElseIf HelpFunction.Convertdbnulls(objDR("CSTreleaseStatus")) = "B. In Progress" Then
                rdoInProgress.Checked = True
            ElseIf HelpFunction.Convertdbnulls(objDR("CSTreleaseStatus")) = "C. Has occurred, but stopped" Then
                rdoHasOccured.Checked = True
            End If

            'ddlCSTreleaseStatus.SelectedValue = HelpFunction.Convertdbnulls(objDR("CSTreleaseStatus"))

            If HelpFunction.Convertdbnulls(objDR("CSTsigCatSiteBoundary")) = "A. Under Evaluation" Then
                rdoUnderEvaluation.Checked = True
            ElseIf HelpFunction.Convertdbnulls(objDR("CSTsigCatSiteBoundary")) = "B. Release is within normal operating limits" Then
                rdoReleaseWithinNormal.Checked = True
            ElseIf HelpFunction.Convertdbnulls(objDR("CSTsigCatSiteBoundary")) = "C. Non-significant (fraction of protective action " Then
                rdoNonSignificant.Checked = True
            ElseIf HelpFunction.Convertdbnulls(objDR("CSTsigCatSiteBoundary")) = "D. Protective action guide range" Then
                rdoPAGRang.Checked = True
            ElseIf HelpFunction.Convertdbnulls(objDR("CSTsigCatSiteBoundary")) = "Liquid Release" Then
                rdoLiquidRelease.Checked = True
            End If
            ' ddlCSTsigCatSiteBoundary.SelectedValue = HelpFunction.Convertdbnulls(objDR("CSTsigCatSiteBoundary"))

            If HelpFunction.Convertdbnulls(objDR("CSTutilRecProtAct")) = "A. No utility recommended actions at this time." Then
                rdoNoUtilityRecommended.Checked = True
            ElseIf HelpFunction.Convertdbnulls(objDR("CSTutilRecProtAct")) = "B. The utility recommends the following protective actions for the public" Then
                rdoUtilityRecommendations.Checked = True
            End If

            ' ddlCSTutilRecProtAct.SelectedValue = HelpFunction.Convertdbnulls(objDR("CSTutilRecProtAct"))
            txtmessageNumber.Text = HelpFunction.Convertdbnulls(objDR("CSTmessageNumber"))
            txtOneMileTEDE.Text = HelpFunction.Convertdbnulls(objDR("OneMileTEDE"))
            txtTwoMileTEDE.Text = HelpFunction.Convertdbnulls(objDR("TwoMileTEDE"))
            txtFiveMileTEDE.Text = HelpFunction.Convertdbnulls(objDR("OneMileTEDE"))
            txtTenMileTEDE.Text = HelpFunction.Convertdbnulls(objDR("TenMileTEDE"))
            txtOneMileCDE.Text = HelpFunction.Convertdbnulls(objDR("OneMileCDE"))
            txtTwoMileCDE.Text = HelpFunction.Convertdbnulls(objDR("TwoMileCDE"))
            txtFiveMileCDE.Text = HelpFunction.Convertdbnulls(objDR("OneMileCDE"))
            txtTenMileCDE.Text = HelpFunction.Convertdbnulls(objDR("TenMileCDE"))

            'txtCSTevacuateZones.Text = HelpFunction.Convertdbnulls(objDR("CSTevacuateZones"))
            'txtCSTshelterZones.Text = HelpFunction.Convertdbnulls(objDR("CSTshelterZones"))
            'txtCST02MilesEvacSect.Text = HelpFunction.Convertdbnulls(objDR("CST02MilesEvacSect"))
            'txtCST02MilesShelterSect.Text = HelpFunction.Convertdbnulls(objDR("CST02MilesShelterSect"))
            'txtCST02MilesNoActtionSect.Text = HelpFunction.Convertdbnulls(objDR("CST02MilesNoActtionSect"))
            'txtCST25MilesEvacSect.Text = HelpFunction.Convertdbnulls(objDR("CST25MilesEvacSect"))
            'txtCST25MilesShelterSect.Text = HelpFunction.Convertdbnulls(objDR("CST25MilesShelterSect"))
            'txtCST25MilesNoActtionSect.Text = HelpFunction.Convertdbnulls(objDR("CST25MilesNoActtionSect"))
            'txtCST510MilesEvacSect.Text = HelpFunction.Convertdbnulls(objDR("CST510MilesEvacSect"))
            'txtCST510MilesShelterSect.Text = HelpFunction.Convertdbnulls(objDR("CST510MilesShelterSect"))
            'txtCST510MilesNoActtionSect.Text = HelpFunction.Convertdbnulls(objDR("CST510MilesNoActtionSect"))

            If (HelpFunction.Convertdbnulls(objDR("CST12A")) = "Yes") Then
                rdoReactionShutdownYes.Checked = True
            ElseIf (HelpFunction.Convertdbnulls(objDR("CST12A")) = "No") Then
                rdoReactorShutdownNo.Checked = True
            End If

            If (HelpFunction.Convertdbnulls(objDR("CST12B"))) = "Yes" Then
                rdoCoreAdequatelyCooledYes.Checked = True
            ElseIf (HelpFunction.Convertdbnulls(objDR("CST12B"))) = "No" Then
                rdoCoreAdequatelyCooledNo.Checked = True
            End If

            If (HelpFunction.Convertdbnulls(objDR("CST12C")) = "Yes") Then
                rdoContainmentIntactYes.Checked = True
            ElseIf (HelpFunction.Convertdbnulls(objDR("CST12C")) = "No") Then
                rdoContainmentIntactNo.Checked = True
            End If

            If (HelpFunction.Convertdbnulls(objDR("CST12D")) = "Stable") Then
                rdoCoreConditionStable.Checked = True
            ElseIf (HelpFunction.Convertdbnulls(objDR("CST12D")) = "Degrading") Then
                rdoCoreConditionDegrading.Checked = True
            End If

            'ddlCST12A.SelectedValue = HelpFunction.Convertdbnulls(objDR("CST12A"))
            'ddlCST12B.SelectedValue = HelpFunction.Convertdbnulls(objDR("CST12B"))
            'ddlCST12C.SelectedValue = HelpFunction.Convertdbnulls(objDR("CST12C"))
            'ddlCST12D.SelectedValue = HelpFunction.Convertdbnulls(objDR("CST12D"))
            txtCST13A.Text = HelpFunction.Convertdbnulls(objDR("CST13A"))
            '  txtCSTProjThyroidDose.Text = HelpFunction.Convertdbnulls(objDR("CSTProjThyroidDose"))
            ' txtCSTProjTotalDose.Text = HelpFunction.Convertdbnulls(objDR("CSTProjTotalDose"))
            txtCST13B.Text = HelpFunction.Convertdbnulls(objDR("CST13B"))
            'ddlCST14A.SelectedValue = HelpFunction.Convertdbnulls(objDR("CST14A"))
            'txtCST14B.Text = HelpFunction.Convertdbnulls(objDR("CST14B"))
            'txtCST14C.Text = HelpFunction.Convertdbnulls(objDR("CST14C"))
            'txtCST14D.Text = HelpFunction.Convertdbnulls(objDR("CST14D"))
            'txtCST14E.Text = HelpFunction.Convertdbnulls(objDR("CST14E"))
            'txtCST14F.Text = HelpFunction.Convertdbnulls(objDR("CST14F"))
            'txtCST14G.Text = HelpFunction.Convertdbnulls(objDR("CST14G"))
            'txtCST14H.Text = HelpFunction.Convertdbnulls(objDR("CST14H"))
            'txtCST14I.Text = HelpFunction.Convertdbnulls(objDR("CST14I"))
            txtCST15Name.Text = HelpFunction.Convertdbnulls(objDR("CST15Name"))
            txtCST15Date.Text = HelpFunction.Convertdbnulls(objDR("CST15Date"))
            localTime3 = CStr(HelpFunction.Convertdbnulls(objDR("CST15Time")))
            'txtCSTuserComments.Text = HelpFunction.Convertdbnulls(objDR("CSTuserComments"))

            'Alabama Start=================================================================
            'ddlFar1SelectOne.SelectedValue = HelpFunction.Convertdbnulls(objDR("Far1SelectOne"))
            'txtFar1MessageNumber.Text = HelpFunction.Convertdbnulls(objDR("Far1MessageNumber"))
            'ddlFar2SelectOne.SelectedValue = HelpFunction.Convertdbnulls(objDR("Far2SelectOne"))
            localTime4 = CStr(HelpFunction.Convertdbnulls(objDR("Far2NotificationTime")))
            'txtFar2NotificationDate.Text = HelpFunction.Convertdbnulls(objDR("Far2NotificationDate"))
            'txtFar2AuthenticationNumber.Text = HelpFunction.Convertdbnulls(objDR("Far2AuthenticationNumber"))
            'txtFar3Site.Text = HelpFunction.Convertdbnulls(objDR("Far3Site"))
            'txtFar3ConfirmationPhoneNumber.Text = HelpFunction.Convertdbnulls(objDR("Far3ConfirmationPhoneNumber"))
            'ddlFar4EmergencyClassification.SelectedValue = HelpFunction.Convertdbnulls(objDR("Far4EmergencyClassification"))
            'txtFar4BasedEALnumber.Text = HelpFunction.Convertdbnulls(objDR("Far4BasedEALnumber"))
            'txtFar4EALdescription.Text = HelpFunction.Convertdbnulls(objDR("Far4EALdescription"))
            'cbxFar5a.Checked = HelpFunction.ConvertdbnullsBool(objDR("Far5a"))
            'cbxFar5b.Checked = HelpFunction.ConvertdbnullsBool(objDR("Far5b"))
            'txtFar5bText.Text = HelpFunction.Convertdbnulls(objDR("Far5bText"))
            'cbxFar5c.Checked = HelpFunction.ConvertdbnullsBool(objDR("Far5c"))
            'txtFar5cText.Text = HelpFunction.Convertdbnulls(objDR("Far5cText"))
            'cbxFar5d.Checked = HelpFunction.ConvertdbnullsBool(objDR("Far5d"))
            'cbxFar5e.Checked = HelpFunction.ConvertdbnullsBool(objDR("Far5e"))
            'txtFar5eText.Text = HelpFunction.Convertdbnulls(objDR("Far5eText"))
            'ddlFar6EmergencyRelease.SelectedValue = HelpFunction.Convertdbnulls(objDR("Far6EmergencyRelease"))
            'ddlFar7ReleaseSignificance.SelectedValue = HelpFunction.Convertdbnulls(objDR("Far7ReleaseSignificance"))
            'ddlFar8EventPrognosis.SelectedValue = HelpFunction.Convertdbnulls(objDR("Far8EventPrognosis"))
            'txtFar9WindDirectDegrees.Text = HelpFunction.Convertdbnulls(objDR("Far9WindDirectDegrees"))
            'txtFar9WindSpeed.Text = HelpFunction.Convertdbnulls(objDR("Far9WindSpeed"))
            'txtFar9Precipitation.Text = HelpFunction.Convertdbnulls(objDR("Far9Precipitation"))
            'ddlFar9StabilityClass.SelectedValue = HelpFunction.Convertdbnulls(objDR("Far9StabilityClass"))
            'ddlFar10Select1.SelectedValue = HelpFunction.Convertdbnulls(objDR("Far10Select1"))
            localTime5 = CStr(HelpFunction.Convertdbnulls(objDR("Far10Time")))
            'txtFar10Date.Text = HelpFunction.Convertdbnulls(objDR("Far10Date"))
            'ddlFar11AffectedUnits.SelectedValue = HelpFunction.Convertdbnulls(objDR("Far11AffectedUnits"))
            'txtFar12AUnitPower.Text = HelpFunction.Convertdbnulls(objDR("Far12AUnitPower"))
            localTime6 = CStr(HelpFunction.Convertdbnulls(objDR("Far12ATime")))
            'txtFar12ADate.Text = HelpFunction.Convertdbnulls(objDR("Far12ADate"))
            'txtFar12BUnitPower.Text = HelpFunction.Convertdbnulls(objDR("Far12BUnitPower"))
            localTime7 = CStr(HelpFunction.Convertdbnulls(objDR("Far12BTime")))
            'txtFar12BDate.Text = HelpFunction.Convertdbnulls(objDR("Far12BDate"))
            'txtFar13Remarks.Text = HelpFunction.Convertdbnulls(objDR("Far13Remarks"))
            'ddlFar14ReleaseChar.SelectedValue = HelpFunction.Convertdbnulls(objDR("Far14ReleaseChar"))
            'ddlFar14Units.SelectedValue = HelpFunction.Convertdbnulls(objDR("Far14Units"))
            'txtFar14NobleGasses.Text = HelpFunction.Convertdbnulls(objDR("Far14NobleGasses"))
            'txtFar14Iodines.Text = HelpFunction.Convertdbnulls(objDR("Far14Iodines"))
            'txtFar14Particulautes.Text = HelpFunction.Convertdbnulls(objDR("Far14Particulautes"))
            'txtFar14Other.Text = HelpFunction.Convertdbnulls(objDR("Far14Other"))
            'cbxFar14Aairborne.Checked = HelpFunction.ConvertdbnullsBool(objDR("Far14Aairborne"))
            'localTime8 = CStr(HelpFunction.Convertdbnulls(objDR("Far14AstartTime")))
            'txtFar14AstartDate.Text = HelpFunction.Convertdbnulls(objDR("Far14AstartDate"))
            localTime9 = CStr(HelpFunction.Convertdbnulls(objDR("Far14AstopTime")))
            'txtFar14AstopDate.Text = HelpFunction.Convertdbnulls(objDR("Far14AstopDate"))
            'cbxFar14Bliquid.Checked = HelpFunction.ConvertdbnullsBool(objDR("Far14Bliquid"))
            localTime10 = CStr(HelpFunction.Convertdbnulls(objDR("Far14BstartTime")))
            'txtFar14BstartDate.Text = HelpFunction.Convertdbnulls(objDR("Far14BstartDate"))
            localTime11 = CStr(HelpFunction.Convertdbnulls(objDR("Far14BstopTime")))
            'txtFar14BendDate.Text = HelpFunction.Convertdbnulls(objDR("Far14BendDate"))
            'txtFar15ProjectionPeriod.Text = HelpFunction.Convertdbnulls(objDR("Far15ProjectionPeriod"))
            'txtFar15EstimatedReleaseDuration.Text = HelpFunction.Convertdbnulls(objDR("Far15EstimatedReleaseDuration"))
            localTime12 = CStr(HelpFunction.Convertdbnulls(objDR("Far15ProjectionPerformedTime")))
            'txtFar15ProjectionPerformedDate.Text = HelpFunction.Convertdbnulls(objDR("Far15ProjectionPerformedDate"))
            'txtFar15AccidentType.Text = HelpFunction.Convertdbnulls(objDR("Far15AccidentType"))
            'txtFar16SiteBoundaryTEDE.Text = HelpFunction.Convertdbnulls(objDR("Far16SiteBoundaryTEDE"))
            'txtFar16SiteBoundaryAdultThyroidCDE.Text = HelpFunction.Convertdbnulls(objDR("Far16SiteBoundaryAdultThyroidCDE"))
            'txtFar16TwoMilesTEDE.Text = HelpFunction.Convertdbnulls(objDR("Far16TwoMilesTEDE"))
            'txtFar16TwoMilesAdultThyroidCDE.Text = HelpFunction.Convertdbnulls(objDR("Far16TwoMilesAdultThyroidCDE"))
            'txtFar16FiveMilesTEDE.Text = HelpFunction.Convertdbnulls(objDR("Far16FiveMilesTEDE"))
            'txtFar16FiveMilesAdultThyroidCDE.Text = HelpFunction.Convertdbnulls(objDR("Far16FiveMilesAdultThyroidCDE"))
            'txtFar16TenMilesTEDE.Text = HelpFunction.Convertdbnulls(objDR("Far16TenMilesTEDE"))
            'txtFar16MilesAdultThyroidCDE.Text = HelpFunction.Convertdbnulls(objDR("Far16MilesAdultThyroidCDE"))
            'txtFar17ApprovedBy.Text = HelpFunction.Convertdbnulls(objDR("Far17ApprovedBy"))
            'txtFar17Title.Text = HelpFunction.Convertdbnulls(objDR("Far17Title"))
            'localTime13 = CStr(HelpFunction.Convertdbnulls(objDR("Far17Time")))
            'txtFar17Date.Text = HelpFunction.Convertdbnulls(objDR("Far17Date"))
            'txtFar17NotifiedBy.Text = HelpFunction.Convertdbnulls(objDR("Far17NotifiedBy"))
            'txtFar17ReceivedBy.Text = HelpFunction.Convertdbnulls(objDR("Far17ReceivedBy"))
            'localTime14 = CStr(HelpFunction.Convertdbnulls(objDR("Far17ReceivedTime")))
            'txtFar17ReceivedDate.Text = HelpFunction.Convertdbnulls(objDR("Far17ReceivedDate"))
            'Alabama End===================================================================

            'Crystal River Defueled Start==================================================
            'ddlCRDselectOne.SelectedValue = HelpFunction.Convertdbnulls(objDR("CRDselectOne"))
            'ddlCRDmessageClassification.SelectedValue = HelpFunction.Convertdbnulls(objDR("CRDmessageClassification"))
            'txtCRDdate.Text = HelpFunction.Convertdbnulls(objDR("CRDdate"))
            'txtCRDreportedByName.Text = HelpFunction.Convertdbnulls(objDR("CRDreportedByName"))
            'txtmessageNumber.Text = HelpFunction.Convertdbnulls(objDR("CRTmessageNumber"))
            'ddlCRDfSelectOne.SelectedValue = HelpFunction.Convertdbnulls(objDR("CRDfSelectOne"))
            'ddlCRDemergencyClassification.SelectedValue = HelpFunction.Convertdbnulls(objDR("CRDemergencyClassification"))
            'txtCRDEmClassDate.Text = HelpFunction.Convertdbnulls(objDR("CRDemClassDate"))
            'txtCRDEmTermDate.Text = HelpFunction.Convertdbnulls(objDR("CRDemTermDate"))
            'txtCRDeALNumbers.Text = HelpFunction.Convertdbnulls(objDR("CRDeALNumbers"))
            'txtCRDeALDescription.Text = HelpFunction.Convertdbnulls(objDR("CRDeALDescription"))
            'ddlCRDeALai.SelectedValue = HelpFunction.Convertdbnulls(objDR("CRDeALai"))
            'txtEALaiDescription.Text = HelpFunction.Convertdbnulls(objDR("CRDeALaiDescription"))
            'txtCRDwindDirectionDegrees.Text = HelpFunction.Convertdbnulls(objDR("CRDwindDirectionDegrees"))
            'txtCRDwindSpeed.Text = HelpFunction.Convertdbnulls(objDR("CRDwindSpeed"))
            'ddlCRDreleaseStatus.SelectedValue = HelpFunction.Convertdbnulls(objDR("CRDreleaseStatus"))
            'ddlCRDreleaseSignificance.SelectedValue = HelpFunction.Convertdbnulls(objDR("CRDreleaseSignificance"))
            'txtCRDProjTotalDose.Text = HelpFunction.Convertdbnulls(objDR("CRDProjTotalDose"))
            'txtCRDDistance83Mile.Text = HelpFunction.Convertdbnulls(objDR("CRDDistance83Mile"))
            'ddlCRDfacCond.SelectedValue = HelpFunction.Convertdbnulls(objDR("CRDfacCond"))
            'txtCRDmessageRecdName.Text = HelpFunction.Convertdbnulls(objDR("CRDmessageRecdName"))
            'txtCRDmessageRecdDate.Text = HelpFunction.Convertdbnulls(objDR("CRDmessageRecdDate"))
            'txtCRDuserComments.Text = HelpFunction.Convertdbnulls(objDR("CRDuserComments"))
            'strCRDcontactTime = HelpFunction.Convertdbnulls(objDR("CRDcontactTime"))
            'strCRDemClassTime = HelpFunction.Convertdbnulls(objDR("CRDemClassTime"))
            'strCRDemTermTime = HelpFunction.Convertdbnulls(objDR("CRDemTermTime"))
            'strCRDmessageRecdTime = HelpFunction.Convertdbnulls(objDR("CRDmessageRecdTime"))
            'Crystal River Defueled End====================================================

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()

        txtWorkSheetDescription.Text = MrDataGrabber.GrabOneStringColumnByPrimaryKey("WorkSheetDescription", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))


        'If ddlSubType.SelectedValue = "Crystal River – Full ENF" Or ddlSubType.SelectedValue = "Saint Lucie" Or ddlSubType.SelectedValue = "Turkey Point" Then
        '    pnlShowFlorida.Visible = True
        'End If

        'If ddlSubType.SelectedValue = "Farley" Then
        '    pnlShowAlabama.Visible = True
        'End If

        'If ddlSubType.SelectedValue = "Crystal River – Permanently Defueled ENF" Then
        '    pnlShowCRDefueled.Visible = True
        'End If

        'Florida Start===================================================================
        txtCSTcontactTime.Text = Left(localTime, 2)
        txtCSTcontactTime2.Text = Right(localTime, 2)

        'If txtCSTcontactTime.Text = "0" Then
        '    txtCSTcontactTime.Text = ""
        'End If

        'If txtCSTcontactTime2.Text = "0" Then
        '    txtCSTcontactTime2.Text = ""
        'End If

        txtCSTdecTermTime.Text = Left(localTime2, 2)
        txtCSTdecTermTime2.Text = Right(localTime2, 2)

        'If txtCSTdecTermTime.Text = "0" Then
        '    txtCSTdecTermTime.Text = ""
        'End If

        'If txtCSTdecTermTime2.Text = "0" Then
        '    txtCSTdecTermTime2.Text = ""
        'End If

        txtCST15Time.Text = Left(localTime3, 2)
        txtCST15Time2.Text = Right(localTime3, 2)

        'If txtCST15Time.Text = "0" Then
        '    txtCST15Time.Text = ""
        'End If

        'If txtCST15Time2.Text = "0" Then
        '    txtCST15Time2.Text = ""
        'End If

        If txtCSTdate.Text = "1/1/1900" Then
            txtCSTdate.Text = ""
        End If

        If txtCSTdecTermDate.Text = "1/1/1900" Then
            txtCSTdecTermDate.Text = ""
        End If

        If txtCST15Date.Text = "1/1/1900" Then
            txtCST15Date.Text = ""
        End If

        'If ddlCST14A.SelectedValue = "As Below" Then
        '    pnlCST14Show.Visible = True
        'End If
        'Florida End===================================================================


        'Alabama Start=================================================================
        'txtFar2NotificationTime.Text = Left(localTime4, 2)
        'txtFar2NotificationTime2.Text = Right(localTime4, 2)

        ''If txtFar2NotificationTime.Text = "0" Then
        ''    txtFar2NotificationTime.Text = ""
        ''End If

        ''If txtFar2NotificationTime2.Text = "0" Then
        ''    txtFar2NotificationTime2.Text = ""
        ''End If

        'If txtFar2NotificationDate.Text = "1/1/1900" Then
        '    txtFar2NotificationDate.Text = ""
        'End If

        'txtFar10Time.Text = Left(localTime5, 2)
        'txtFar10Time2.Text = Right(localTime5, 2)

        'If txtFar10Time.Text = "0" Then
        '    txtFar10Time.Text = ""
        'End If

        'If txtFar10Time2.Text = "0" Then
        '    txtFar10Time2.Text = ""
        'End If

        'If txtFar10Date.Text = "1/1/1900" Then
        '    txtFar10Date.Text = ""
        'End If

        'txtFar12ATime.Text = Left(localTime6, 2)
        'txtFar12ATime2.Text = Right(localTime6, 2)

        'If txtFar12ATime.Text = "0" Then
        '    txtFar12ATime.Text = ""
        'End If

        'If txtFar12ATime2.Text = "0" Then
        '    txtFar12ATime2.Text = ""
        'End If

        'If txtFar12ADate.Text = "1/1/1900" Then
        '    txtFar12ADate.Text = ""
        'End If

        'txtFar12BTime.Text = Left(localTime7, 2)
        'txtFar12BTime2.Text = Right(localTime7, 2)

        'If txtFar12BTime.Text = "0" Then
        '    txtFar12BTime.Text = ""
        'End If

        'If txtFar12BTime2.Text = "0" Then
        '    txtFar12BTime2.Text = ""
        'End If

        'If txtFar12BDate.Text = "1/1/1900" Then
        '    txtFar12BDate.Text = ""
        'End If

        'txtFar14AstartTime.Text = Left(localTime8, 2)
        'txtFar14AstartTime2.Text = Right(localTime8, 2)

        'If txtFar14AstartTime.Text = "0" Then
        '    txtFar14AstartTime.Text = ""
        'End If

        'If txtFar14AstartTime2.Text = "0" Then
        '    txtFar14AstartTime2.Text = ""
        'End If

        'If txtFar14AstartDate.Text = "1/1/1900" Then
        '    txtFar14AstartDate.Text = ""
        'End If

        'txtFar14AstopTime.Text = Left(localTime9, 2)
        'txtFar14AstopTime2.Text = Right(localTime9, 2)

        'If txtFar14AstopTime.Text = "0" Then
        '    txtFar14AstopTime.Text = ""
        'End If

        'If txtFar14AstopTime2.Text = "0" Then
        '    txtFar14AstopTime2.Text = ""
        'End If

        'If txtFar14AstopDate.Text = "1/1/1900" Then
        '    txtFar14AstopDate.Text = ""
        'End If

        'txtFar14BstartTime.Text = Left(localTime10, 2)
        'txtFar14BstartTime2.Text = Right(localTime10, 2)

        'If txtFar14BstartTime.Text = "0" Then
        '    txtFar14BstartTime.Text = ""
        'End If

        'If txtFar14BstartTime2.Text = "0" Then
        '    txtFar14BstartTime2.Text = ""
        'End If

        'If txtFar14BstartDate.Text = "1/1/1900" Then
        '    txtFar14BstartDate.Text = ""
        'End If

        'txtFar14BstopTime.Text = Left(localTime11, 2)
        'txtFar14BstopTime2.Text = Right(localTime11, 2)

        'If txtFar14BstopTime.Text = "0" Then
        '    txtFar14BstopTime.Text = ""
        'End If

        'If txtFar14BstopTime2.Text = "0" Then
        '    txtFar14BstopTime2.Text = ""
        'End If

        'If txtFar14BendDate.Text = "1/1/1900" Then
        '    txtFar14BendDate.Text = ""
        'End If

        'txtFar15ProjectionPerformedTime.Text = Left(localTime12, 2)
        'txtFar15ProjectionPerformedTime2.Text = Right(localTime12, 2)

        'If txtFar15ProjectionPerformedTime.Text = "0" Then
        '    txtFar15ProjectionPerformedTime.Text = ""
        'End If

        'If txtFar15ProjectionPerformedTime2.Text = "0" Then
        '    txtFar15ProjectionPerformedTime2.Text = ""
        'End If

        'If txtFar15ProjectionPerformedDate.Text = "1/1/1900" Then
        '    txtFar15ProjectionPerformedDate.Text = ""
        'End If

        'txtFar17Time.Text = Left(localTime13, 2)
        'txtFar17Time2.Text = Right(localTime13, 2)

        'If txtFar17Time.Text = "0" Then
        '    txtFar17Time.Text = ""
        'End If

        'If txtFar17Time2.Text = "0" Then
        '    txtFar17Time2.Text = ""
        'End If

        'If txtFar17Date.Text = "1/1/1900" Then
        '    txtFar17Date.Text = ""
        'End If

        'txtFar17ReceivedTime.Text = Left(localTime14, 2)
        'txtFar17ReceivedTime2.Text = Right(localTime14, 2)

        'If txtFar17ReceivedTime.Text = "0" Then
        '    txtFar17ReceivedTime.Text = ""
        'End If

        'If txtFar17ReceivedTime2.Text = "0" Then
        '    txtFar17ReceivedTime2.Text = ""
        'End If

        'If txtFar17ReceivedDate.Text = "1/1/1900" Then
        '    txtFar17ReceivedDate.Text = ""
        'End If
        'Alabama End===================================================================


        'Crystal River Defueled Start==================================================
        'txtCRDcontactTime.Text = Left(strCRDcontactTime, 2)
        'txtCRDcontactTime2.Text = Right(strCRDcontactTime, 2)
        'txtCRDEmClassTime.Text = Left(strCRDemClassTime, 2)
        'txtCRDEmClassTime2.Text = Right(strCRDemClassTime, 2)
        'txtCRDEmTermTime.Text = Left(strCRDemTermTime, 2)
        'txtCRDEmTermTime2.Text = Right(strCRDemTermTime, 2)
        'txtCRDmessageRecdTime.Text = Left(strCRDmessageRecdTime, 2)
        'txtCRDmessageRecdTime2.Text = Right(strCRDmessageRecdTime, 2)

        'If txtCRDdate.Text = "1/1/1900" Then
        '    txtCRDdate.Text = ""
        'End If
        'If txtCRDEmClassDate.Text = "1/1/1900" Then
        '    txtCRDEmClassDate.Text = ""
        'End If
        'If txtCRDEmTermDate.Text = "1/1/1900" Then
        '    txtCRDEmTermDate.Text = ""
        'End If
        'If txtCRDmessageRecdDate.Text = "1/1/1900" Then
        '    txtCRDmessageRecdDate.Text = ""
        'End If
        'Crystal River Defueled End====================================================

    End Sub

    Sub PopulateDDLs()

        'Notification Group
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objCmd = New SqlCommand("spSelectIncidentTypeLevelForDDL", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentTypeID", MrDataGrabber.GrabOneIntegerColumnByPrimaryKey("IncidentTypeID", "IncidentIncidentType", "IncidentIncidentTypeID", Request("IncidentIncidentTypeID")))

        DBConStringHelper.PrepareConnection(objConn) 'Open the connection
        ddlNotification.DataSource = objCmd.ExecuteReader()
        ddlNotification.DataBind()
        DBConStringHelper.FinalizeConnection(objConn) 'Close the connection

        objCmd = Nothing

        'add an "Select an Option" item to the list
        ddlNotification.Items.Insert(0, New ListItem("Select an Option", "Select an Option"))
        ddlNotification.Items(0).Selected = True

    End Sub


    Protected Sub ddlSubType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubType.SelectedIndexChanged





    End Sub

    'Protected Sub ddlCST14A_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCST14A.SelectedIndexChanged

    '    If ddlCST14A.SelectedValue = "As Below" Then
    '        pnlCST14Show.Visible = True
    '    Else
    '        pnlCST14Show.Visible = False
    '    End If

    'End Sub


    'Buttons
    Protected Sub btnSave_Command(ByVal sender As Object, ByVal e As EventArgs)
        ErrorChecks()

        If globalHasErrors = False Then
            Save()

            'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID") & "&Parameter=WorkSheet")

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)
        Else
            '  pnlMessage.Visible = True
        End If
    End Sub

    Protected Sub btnCancel_Command(ByVal sender As Object, ByVal e As EventArgs)

        'Response.Redirect("EditIncident.aspx?IncidentID=" & Request("IncidentID") & "&Parameter=WorkSheet")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "key", "<script language='javascript'> { window.open('','_self');window.close();}</script>", False)

    End Sub

    Protected Sub Save()


        Dim localNPPCount As Integer = 0

        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
        objConn.Open()
        objCmd = New SqlCommand("spSelectNPPCountByIncidentID", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))

        objDR = objCmd.ExecuteReader

        If objDR.Read() Then

            localNPPCount = HelpFunction.ConvertdbnullsInt(objDR("Count"))

        End If

        objDR.Close()

        objCmd.Dispose()
        objCmd = Nothing

        objConn.Close()



        'Florida Start===================================================================
        'If pnlShowFlorida.Visible = False Then

        '    ddlCSTselectOne.SelectedValue = "Select an Option"
        '    txtCSTdate.Text = ""
        '    txtCSTcontactTime.Text = ""
        '    txtCSTcontactTime2.Text = ""
        '    txtCSTreportedByName.Text = ""
        '    txtCSTmessageNumber.Text = ""
        '    ddlCSTreportedFrom.SelectedValue = "Select an Option"
        '    ddlCSTfSelectOne.SelectedValue = "Select an Option"
        '    ddlCSTsite.SelectedValue = "Select an Option"
        '    ddlCSTemergencyClassification.SelectedValue = "Select an Option"
        '    ddlCSTdecTermSelectOne.SelectedValue = "Select an Option"
        '    txtCSTdecTermDate.Text = ""
        '    txtCSTdecTermTime.Text = ""
        '    txtCSTdecTermTime2.Text = ""
        '    ddlCSTdecTermReason.SelectedValue = "Select an Option"
        '    txtCSTeALNumbers.Text = ""
        '    txtCSTeALDescription.Text = ""
        '    ddlCSTeALai.SelectedValue = "Select an Option"
        '    txtCSTeALaiDescription.Text = ""
        '    txtCSTwindDirectionDegrees.Text = ""
        '    txtCSTdownwindSectorsAffected.Text = ""
        '    ddlCSTreleaseStatus.SelectedValue = "Select an Option"
        '    ddlCSTsigCatSiteBoundary.SelectedValue = "Select an Option"
        '    ddlCSTutilRecProtAct.SelectedValue = "Select an Option"
        '    txtCSTevacuateZones.Text = ""
        '    txtCSTshelterZones.Text = ""
        '    txtCST02MilesEvacSect.Text = ""
        '    txtCST02MilesShelterSect.Text = ""
        '    txtCST02MilesNoActtionSect.Text = ""
        '    txtCST25MilesEvacSect.Text = ""
        '    txtCST25MilesShelterSect.Text = ""
        '    txtCST25MilesNoActtionSect.Text = ""
        '    txtCST510MilesEvacSect.Text = ""
        '    txtCST510MilesShelterSect.Text = ""
        '    txtCST510MilesNoActtionSect.Text = ""
        '    ddlCST12A.SelectedValue = "Select an Option"
        '    ddlCST12B.SelectedValue = "Select an Option"
        '    ddlCST12C.SelectedValue = "Select an Option"
        '    ddlCST12D.SelectedValue = "Select an Option"
        '    txtCST13A.Text = ""
        '    txtCST13B.Text = ""

        '    txtCSTProjThyroidDose.Text = ""
        '    txtCSTProjTotalDose.Text = ""
        '    ddlCST14A.SelectedValue = "Select an Option"
        '    txtCST14B.Text = ""
        '    txtCST14C.Text = ""
        '    txtCST14D.Text = ""
        '    txtCST14E.Text = ""
        '    txtCST14F.Text = ""
        '    txtCST14G.Text = ""
        '    txtCST14H.Text = ""
        '    txtCST14I.Text = ""

        '    txtCST15Name.Text = ""
        '    txtCST15Date.Text = ""
        '    txtCST15Time.Text = ""
        '    txtCST15Time2.Text = ""
        '    txtCSTuserComments.Text = ""
        '    txtOneMileTEDE.Text = ""
        '    txtTwoMileTEDE.Text = ""
        '    txtFiveMileTEDE.Text = ""
        '    txtTenMileTEDE.Text = ""
        '    txtOneMileCDE.Text = ""
        '    txtTwoMileCDE.Text = ""
        '    txtFiveMileCDE.Text = ""
        '    txtTenMileCDE.Text = ""

        'End If

        'If pnlCST14Show.Visible = False Then

        '    txtCST14B.Text = ""
        '    txtCST14C.Text = ""
        '    txtCST14D.Text = ""
        '    txtCST14E.Text = ""
        '    txtCST14F.Text = ""
        '    txtCST14G.Text = ""
        '    txtCST14H.Text = ""
        '    txtCSTProjThyroidDose.Text = ""
        '    txtCSTProjTotalDose.Text = ""

        'End If
        ''Florida End===================================================================



        ''Alabama Start=================================================================
        'If pnlShowAlabama.Visible = False Then
        '    ddlFar1SelectOne.SelectedValue = "Select an Option"
        '    txtFar1MessageNumber.Text = ""
        '    ddlFar2SelectOne.SelectedValue = "Select an Option"
        '    txtFar2NotificationTime.Text = ""
        '    txtFar2NotificationTime2.Text = ""
        '    txtFar2NotificationDate.Text = ""
        '    txtFar2AuthenticationNumber.Text = ""
        '    txtFar3Site.Text = ""
        '    txtFar3ConfirmationPhoneNumber.Text = ""
        '    ddlFar4EmergencyClassification.SelectedValue = "Select an Option"
        '    txtFar4BasedEALnumber.Text = ""
        '    txtFar4EALdescription.Text = ""
        '    cbxFar5a.Checked = False
        '    cbxFar5b.Checked = False
        '    txtFar5bText.Text = ""
        '    cbxFar5c.Checked = False
        '    txtFar5cText.Text = ""
        '    cbxFar5d.Checked = False
        '    cbxFar5e.Checked = False
        '    txtFar5eText.Text = ""
        '    ddlFar6EmergencyRelease.SelectedValue = "Select an Option"
        '    ddlFar7ReleaseSignificance.SelectedValue = "Select an Option"
        '    ddlFar8EventPrognosis.SelectedValue = "Select an Option"
        '    txtFar9WindDirectDegrees.Text = ""
        '    txtFar9WindSpeed.Text = ""
        '    txtFar9Precipitation.Text = ""
        '    ddlFar9StabilityClass.SelectedValue = "Select an Option"
        '    ddlFar10Select1.SelectedValue = "Select an Option"
        '    txtFar10Time.Text = ""
        '    txtFar10Time2.Text = ""
        '    txtFar10Date.Text = ""
        '    ddlFar11AffectedUnits.SelectedValue = "Select an Option"
        '    txtFar12AUnitPower.Text = ""
        '    txtFar12ATime.Text = ""
        '    txtFar12ATime2.Text = ""
        '    txtFar12ADate.Text = ""
        '    txtFar12BUnitPower.Text = ""
        '    txtFar12BTime.Text = ""
        '    txtFar12BTime2.Text = ""
        '    txtFar12BDate.Text = ""
        '    txtFar13Remarks.Text = ""
        '    ddlFar14ReleaseChar.SelectedValue = "Select an Option"
        '    ddlFar14Units.SelectedValue = "Select an Option"
        '    txtFar14NobleGasses.Text = ""
        '    txtFar14Iodines.Text = ""
        '    txtFar14Particulautes.Text = ""
        '    txtFar14Other.Text = ""
        '    cbxFar14Aairborne.Checked = False
        '    txtFar14AstartDate.Text = ""
        '    txtFar14AstopTime.Text = ""
        '    txtFar14AstopTime2.Text = ""
        '    txtFar14AstopDate.Text = ""
        '    cbxFar14Bliquid.Checked = False
        '    txtFar14BstartTime.Text = ""
        '    txtFar14BstartTime2.Text = ""
        '    txtFar14BstartDate.Text = ""
        '    txtFar14BstopTime.Text = ""
        '    txtFar14BstopTime2.Text = ""
        '    txtFar14BendDate.Text = ""
        '    txtFar15ProjectionPeriod.Text = ""
        '    txtFar15EstimatedReleaseDuration.Text = ""
        '    txtFar15ProjectionPerformedTime.Text = ""
        '    txtFar15ProjectionPerformedTime2.Text = ""
        '    txtFar15ProjectionPerformedDate.Text = ""
        '    txtFar15AccidentType.Text = ""
        '    txtFar16SiteBoundaryTEDE.Text = ""
        '    txtFar16SiteBoundaryAdultThyroidCDE.Text = ""
        '    txtFar16TwoMilesTEDE.Text = ""
        '    txtFar16TwoMilesAdultThyroidCDE.Text = ""
        '    txtFar16FiveMilesTEDE.Text = ""
        '    txtFar16FiveMilesAdultThyroidCDE.Text = ""
        '    txtFar16TenMilesTEDE.Text = ""
        '    txtFar16MilesAdultThyroidCDE.Text = ""
        '    txtFar17ApprovedBy.Text = ""
        '    txtFar17Title.Text = ""
        '    txtFar17Time.Text = ""
        '    txtFar17Time2.Text = ""
        '    txtFar17Date.Text = ""
        '    txtFar17NotifiedBy.Text = ""
        '    txtFar17ReceivedBy.Text = ""
        '    txtFar17ReceivedTime.Text = ""
        '    txtFar17ReceivedTime2.Text = ""
        '    txtFar17ReceivedDate.Text = ""
        'End If
        ''Alabama End===================================================================



        ''Crystal River Defueled Start===================================================================
        'If pnlShowCRDefueled.Visible = False Then
        '    ddlCRDselectOne.SelectedValue = "Select an Option"
        '    ddlCRDmessageClassification.SelectedValue = "Select an Option"
        '    txtCRDdate.Text = ""
        '    txtCRDcontactTime.Text = ""
        '    txtCRDcontactTime2.Text = ""
        '    txtCRDreportedByName.Text = ""
        '    txtCRDmessageNumber.Text = ""
        '    ddlCRDfSelectOne.SelectedValue = "Select an Option"
        '    ddlCRDemergencyClassification.SelectedValue = "Select an Option"
        '    txtCRDEmClassDate.Text = ""
        '    txtCRDEmClassTime.Text = ""
        '    txtCRDEmClassTime2.Text = ""
        '    txtCRDEmTermDate.Text = ""
        '    txtCRDEmTermTime.Text = ""
        '    txtCRDEmTermTime2.Text = ""
        '    txtCRDeALNumbers.Text = ""
        '    txtCRDeALDescription.Text = ""
        '    ddlCRDeALai.SelectedValue = "Select an Option"
        '    txtEALaiDescription.Text = ""
        '    txtCRDwindDirectionDegrees.Text = ""
        '    txtCRDwindSpeed.Text = ""
        '    ddlCRDreleaseStatus.SelectedValue = "Select an Option"
        '    ddlCRDreleaseSignificance.SelectedValue = "Select an Option"
        '    txtCRDProjTotalDose.Text = ""
        '    txtCRDDistance83Mile.Text = ""
        '    ddlCRDfacCond.SelectedValue = "Select an Option"
        '    txtCRDmessageRecdName.Text = ""
        '    txtCRDmessageRecdDate.Text = ""
        '    txtCRDmessageRecdTime.Text = ""
        '    txtCRDmessageRecdTime2.Text = ""
        '    txtCRDuserComments.Text = ""
        'End If
        'Crystal River Defueled End=====================================================================



        If localNPPCount = 0 Then

            'Try

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spActionNPP", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@Flag", 0)
            objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", ddlNotification.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CSTmessageNumber", txtmessageNumber.Text)
            'Florida Start===================================================================

            'Emergency Type 
            Dim EmergencyType As String = ""
            If rdoDrill.Checked = True Then
                EmergencyType = "This is a Drill"
            ElseIf rdoEvent.Checked = True Then
                EmergencyType = "This is an EMERGENCY"
            End If
            objCmd.Parameters.AddWithValue("@CSTselectOne", EmergencyType)


            'ShowPlantConditions 
            Dim ShowPlantConditions As String = ""
            If rdoShowPlantConditionsYes.Checked = True Then
                ShowPlantConditions = "1"
            ElseIf rdoShowPlantConditionsNo.Checked = True Then
                ShowPlantConditions = "0"
            End If
            objCmd.Parameters.AddWithValue("@ShowPlantConditions", ShowPlantConditions)


            Dim VerificationStateWatchOffice As String = ""
            Dim VerificationDOH As String = ""
            Dim VerificationStLucie As String = ""
            Dim VerificationMartinCo As String = ""
            Dim VerificationMiamiDade As String = ""
            Dim VerificationMonroeCo As String = ""

            VerificationStateWatchOffice = IIf(chkStateWatchOffice.Checked = True, "1", "0")
            VerificationDOH = IIf(chkDOH.Checked = True, "1", "0")
            VerificationStLucie = IIf(chkStLucieCo.Checked = True, "1", "0")
            VerificationMartinCo = IIf(chkMartinCo.Checked = True, "1", "0")
            VerificationMiamiDade = IIf(chkMiamiDade.Checked = True, "1", "0")
            VerificationMonroeCo = IIf(chkMonroeCo.Checked = True, "1", "0")



            objCmd.Parameters.AddWithValue("@VerificationStateWatchOffice", VerificationStateWatchOffice)
            objCmd.Parameters.AddWithValue("@VerificationDOH", VerificationDOH)
            objCmd.Parameters.AddWithValue("@VerificationStLucie", VerificationStLucie)
            objCmd.Parameters.AddWithValue("@VerificationMartinCo", VerificationMartinCo)
            objCmd.Parameters.AddWithValue("@VerificationMiamiDade", VerificationMiamiDade)
            objCmd.Parameters.AddWithValue("@VerificationMonroeCo", VerificationMonroeCo)


            ' objCmd.Parameters.AddWithValue("@CSTselectOne", ddlCSTselectOne.SelectedValue.ToString)
            objCmd.Parameters.AddWithValue("@CSTdate", txtCSTdate.Text)
            objCmd.Parameters.AddWithValue("@CSTcontactTime", CStr(txtCSTcontactTime.Text.Trim) & CStr(txtCSTcontactTime2.Text.Trim))
            objCmd.Parameters.AddWithValue("@CSTreportedByName", txtCSTreportedByName.Text)

            objCmd.Parameters.AddWithValue("@CSTreportedFrom", "")



            objCmd.Parameters.AddWithValue("@CSTfSelectOne", "")
            'Site
            Dim Site As String = ""
            If rdoStLucieUnit1.Checked = True Then
                Site = "St. Lucie Unit 1"
            ElseIf rdoStLucieUnit2.Checked = True Then
                Site = "St. Lucie Unit 2"
            ElseIf rdoTurkeyPointUnit3.Checked = True Then
                Site = "Turkey Point Unit 3"
            ElseIf rdoTurkeyPointUnit4.Checked = True Then
                Site = "Turkey Point Unit 4"
            End If

            Dim SiteLucie1 = "0", SiteLucie2 = "0", SiteTurkey1 = "0", SiteTurkey2 = "0"
            If rdoStLucieUnit1.Checked = True Then
                SiteLucie1 = "1"
            End If
            If rdoStLucieUnit2.Checked = True Then
                SiteLucie2 = "1"
            End If
            If rdoTurkeyPointUnit3.Checked = True Then
                SiteTurkey1 = "1"
            End If
            If rdoTurkeyPointUnit4.Checked = True Then
                SiteTurkey2 = "1"
            End If
            objCmd.Parameters.AddWithValue("@SiteLucie1", SiteLucie1)
            objCmd.Parameters.AddWithValue("@SiteLucie2", SiteLucie2)
            objCmd.Parameters.AddWithValue("@SiteTurkey1", SiteTurkey1)
            objCmd.Parameters.AddWithValue("@SiteTurkey2", SiteTurkey2)

            objCmd.Parameters.AddWithValue("@CSTsite", "")

            'objCmd.Parameters.AddWithValue("@CSTsite", ddlCSTsite.SelectedValue.ToString)

            'Classification
            Dim EmergencyClassification As String = ""
        If rdoNotificationOfUnusualEvent.Checked = True Then
            EmergencyClassification = "A. Notification of Unusual Event"
        ElseIf rdoAlert.Checked = True Then
            EmergencyClassification = "B. Alert"
        ElseIf rdoSiteEmergencyArea.Checked = True Then
            EmergencyClassification = "C. Site Area Emergency"
        ElseIf rdoGeneralEmergency.Checked = True Then
            EmergencyClassification = "D. General Emergency"
        End If
        objCmd.Parameters.AddWithValue("@CSTemergencyClassification", EmergencyClassification)
        'objCmd.Parameters.AddWithValue("@CSTemergencyClassification", ddlCSTemergencyClassification.SelectedValue.ToString)

        'Declaration
        Dim Declararion As String = ""
        If rdoEmergencyDeclaration.Checked = True Then
            Declararion = "A. Emergency Declaration"
        ElseIf rdoEmergencyTermination.Checked = True Then
            Declararion = "B. Emergency Termination"
        End If
        objCmd.Parameters.AddWithValue("@CSTdecTermSelectOne", Declararion)
        ' objCmd.Parameters.AddWithValue("@CSTdecTermSelectOne", ddlCSTdecTermSelectOne.SelectedValue.ToString)

        Dim Papa1 As String = ""
        Dim Papa2 As String = ""
        Dim Papa3 As String = ""
        Dim Delta1 As String = ""
        Dim Delta2 As String = ""
        Dim Delta3 As String = ""
        Dim Potassium As String = ""

        Papa1 = IIf(chkPapa1.Checked = True, "1", "0")
        Papa2 = IIf(chkPapa2.Checked = True, "1", "0")
        Papa3 = IIf(chkPapa3.Checked = True, "1", "0")
        Delta1 = IIf(chkDelta1.Checked = True, "1", "0")
        Delta2 = IIf(chkDelta2.Checked = True, "1", "0")
        Delta3 = IIf(chkDelta3.Checked = True, "1", "0")
        Potassium = IIf(chkPotassium.Checked = True, "1", "0")

        objCmd.Parameters.AddWithValue("@Papa1", Papa1)
        objCmd.Parameters.AddWithValue("@Papa2", Papa2)
        objCmd.Parameters.AddWithValue("@Papa3", Papa3)
        objCmd.Parameters.AddWithValue("@Delta1", Delta1)
        objCmd.Parameters.AddWithValue("@Delta2", Delta2)
        objCmd.Parameters.AddWithValue("@Delta3", Delta3)
        objCmd.Parameters.AddWithValue("@Potassium", Potassium)

        objCmd.Parameters.AddWithValue("@CSTdecTermDate", txtCSTdecTermDate.Text)
        objCmd.Parameters.AddWithValue("@CSTdecTermTime", CStr(txtCSTdecTermTime.Text.Trim) & CStr(txtCSTdecTermTime2.Text.Trim))
        objCmd.Parameters.AddWithValue("@CSTdecTermReason", "")
        objCmd.Parameters.AddWithValue("@CSTeALNumbers", txtCSTeALNumbers.Text)
        objCmd.Parameters.AddWithValue("@CSTeALDescription", txtCSTeALDescription.Text)

        Dim Additional As String = ""
        If rdoAdditionalInformationDose.Checked Then
            Additional = "Dose"
        Else
            Additional = "A. None"
        End If
        objCmd.Parameters.AddWithValue("@CSTeALai", Additional)
        ' objCmd.Parameters.AddWithValue("@CSTeALai", ddlCSTeALai.SelectedValue.ToString)

        objCmd.Parameters.AddWithValue("@CSTeALaiDescription", "")
        objCmd.Parameters.AddWithValue("@CSTwindDirectionDegrees", txtCSTwindDirectionDegrees.Text)
        objCmd.Parameters.AddWithValue("@CSTdownwindSectorsAffected", txtCSTdownwindSectorsAffected.Text)

        Dim ReleaseStatus As String = ""
        If rdoNone.Checked = True Then
            ReleaseStatus = "A. None (Go to item 11)"
        ElseIf rdoInProgress.Checked = True Then
            ReleaseStatus = "B. In Progress"
        ElseIf rdoHasOccured.Checked = True Then
            ReleaseStatus = "C. Has occurred, but stopped"
        End If
        objCmd.Parameters.AddWithValue("@CSTreleaseStatus ", ReleaseStatus)
        'objCmd.Parameters.AddWithValue("@CSTreleaseStatus ", ddlCSTreleaseStatus.SelectedValue.ToString)


        Dim SiteBoudary As String = ""
        If rdoUnderEvaluation.Checked = True Then
            SiteBoudary = "A. Under Evaluation"
        ElseIf rdoReleaseWithinNormal.Checked = True Then
            SiteBoudary = "B. Release is within normal operating limits"
        ElseIf rdoNonSignificant.Checked = True Then
            SiteBoudary = "C. Non-significant (fraction of protective action"
        ElseIf rdoPAGRang.Checked = True Then
            SiteBoudary = "D. Protective action guide range"
        ElseIf rdoLiquidRelease.Checked Then
            SiteBoudary = "Liquid Release"
        End If
        objCmd.Parameters.AddWithValue("@CSTsigCatSiteBoundary", SiteBoudary)
        'objCmd.Parameters.AddWithValue("@CSTsigCatSiteBoundary", ddlCSTsigCatSiteBoundary.SelectedValue.ToString)

        Dim UtilityRecommendations As String = ""
        If rdoNoUtilityRecommended.Checked = True Then
            UtilityRecommendations = "A. No utility recommended actions at this time."
        ElseIf rdoUtilityRecommendations.Checked = True Then
            UtilityRecommendations = "B. The utility recommends the following protective actions for the public"
        End If
        objCmd.Parameters.AddWithValue("@CSTutilRecProtAct", UtilityRecommendations)
        ' objCmd.Parameters.AddWithValue("@CSTutilRecProtAct", ddlCSTutilRecProtAct.SelectedValue.ToString)

        objCmd.Parameters.AddWithValue("@CSTevacuateZones", "")
        objCmd.Parameters.AddWithValue("@CSTshelterZones", "")
        objCmd.Parameters.AddWithValue("@CST02MilesEvacSect", "")
        objCmd.Parameters.AddWithValue("@CST02MilesShelterSect", "")
        objCmd.Parameters.AddWithValue("@CST02MilesNoActtionSect", "")
        objCmd.Parameters.AddWithValue("@CST25MilesEvacSect", "")
        objCmd.Parameters.AddWithValue("@CST25MilesShelterSect", "")
        objCmd.Parameters.AddWithValue("@CST25MilesNoActtionSect", "")
        objCmd.Parameters.AddWithValue("@CST510MilesEvacSect", "")
        objCmd.Parameters.AddWithValue("@CST510MilesShelterSect", "")
        objCmd.Parameters.AddWithValue("@CST510MilesNoActtionSect", "")

        Dim Reactor As String = ""
        If rdoReactionShutdownYes.Checked = True Then
            Reactor = "Yes"
        Else
            Reactor = "No"
        End If
        objCmd.Parameters.AddWithValue("@CST12A", Reactor)

        Dim Cooled As String = ""
        If rdoCoreAdequatelyCooledYes.Checked = True Then
            Cooled = "Yes"
        Else
            Cooled = "No"
        End If
        objCmd.Parameters.AddWithValue("@CST12B", Cooled)

        Dim Intact As String = ""
        If rdoContainmentIntactYes.Checked = True Then
            Intact = "Yes"
        Else
            Intact = "No"
        End If
        objCmd.Parameters.AddWithValue("@CST12C", Intact)

        Dim Core As String = ""
        If rdoCoreConditionStable.Checked = True Then
            Core = "Stable"
        Else
            Core = "Degrading"
        End If
        objCmd.Parameters.AddWithValue("@CST12D", Core)


        objCmd.Parameters.AddWithValue("@OneMileTEDE", txtOneMileTEDE.Text)
        objCmd.Parameters.AddWithValue("@TwoMileTEDE", txtTwoMileTEDE.Text)
        objCmd.Parameters.AddWithValue("@FiveMileTEDE", txtFiveMileTEDE.Text)
        objCmd.Parameters.AddWithValue("@TenMileTEDE", txtTenMileTEDE.Text)

        objCmd.Parameters.AddWithValue("@OneMileCDE", txtOneMileCDE.Text)
        objCmd.Parameters.AddWithValue("@TwoMileCDE", txtTwoMileCDE.Text)
        objCmd.Parameters.AddWithValue("@FiveMileCDE", txtFiveMileCDE.Text)
        objCmd.Parameters.AddWithValue("@TenMileCDE", txtTenMileCDE.Text)

        'objCmd.Parameters.AddWithValue("@CST12A", ddlCST12A.SelectedValue.ToString)
        'objCmd.Parameters.AddWithValue("@CST12B", ddlCST12B.SelectedValue.ToString)
        'objCmd.Parameters.AddWithValue("@CST12C", ddlCST12C.SelectedValue.ToString)
        ' objCmd.Parameters.AddWithValue("@CST12D", ddlCST12D.SelectedValue.ToString)
        objCmd.Parameters.AddWithValue("@CST13A", txtCST13A.Text)
        objCmd.Parameters.AddWithValue("@CSTProjThyroidDose", "")
        objCmd.Parameters.AddWithValue("@CSTProjTotalDose", "")
        objCmd.Parameters.AddWithValue("@CST13B", txtCST13B.Text)
        objCmd.Parameters.AddWithValue("@CST14A", "")
        objCmd.Parameters.AddWithValue("@CST14B", "")
        objCmd.Parameters.AddWithValue("@CST14C", "")
        objCmd.Parameters.AddWithValue("@CST14D", "")
        objCmd.Parameters.AddWithValue("@CST14E", "")
        objCmd.Parameters.AddWithValue("@CST14F", "")
        objCmd.Parameters.AddWithValue("@CST14G", "")
        objCmd.Parameters.AddWithValue("@CST14H", "")
        objCmd.Parameters.AddWithValue("@CST14I", "")
        objCmd.Parameters.AddWithValue("@CST15NamE", txtCST15Name.Text)
        objCmd.Parameters.AddWithValue("@CST15Date", txtCST15Date.Text)
        objCmd.Parameters.AddWithValue("@CST15Time", CStr(txtCST15Time.Text.Trim) & CStr(txtCST15Time2.Text.Trim))
        objCmd.Parameters.AddWithValue("@CSTuserComments", "")

        objCmd.Parameters.AddWithValue("@ApproveName", "")
        objCmd.Parameters.AddWithValue("@ApproveTime", "")
        objCmd.Parameters.AddWithValue("@ApproveDate", "")

        'Florida End===================================================================

        'Alabama Start=================================================================
        objCmd.Parameters.AddWithValue("@Far1SelectOne", "")
        objCmd.Parameters.AddWithValue("@Far1MessageNumber", "")
        objCmd.Parameters.AddWithValue("@Far2SelectOne", "")
        objCmd.Parameters.AddWithValue("@Far2NotificationTime", "" & "")
        objCmd.Parameters.AddWithValue("@Far2NotificationDate", "")
        objCmd.Parameters.AddWithValue("@Far2AuthenticationNumber", "")
        objCmd.Parameters.AddWithValue("@Far3Site", "")
        objCmd.Parameters.AddWithValue("@Far3ConfirmationPhoneNumber", "")
        objCmd.Parameters.AddWithValue("@Far4EmergencyClassification", "")
        objCmd.Parameters.AddWithValue("@Far4BasedEALnumber", "")
        objCmd.Parameters.AddWithValue("@Far4EALdescription", "")
        objCmd.Parameters.AddWithValue("@Far5a", "")
        objCmd.Parameters.AddWithValue("@Far5b", "")
        objCmd.Parameters.AddWithValue("@Far5bText", "")
        objCmd.Parameters.AddWithValue("@Far5c", "")
        objCmd.Parameters.AddWithValue("@Far5cText", "")
        objCmd.Parameters.AddWithValue("@Far5d", "")
        objCmd.Parameters.AddWithValue("@Far5e", "")
        objCmd.Parameters.AddWithValue("@Far5eText", "")
        objCmd.Parameters.AddWithValue("@Far6EmergencyRelease", "")
        objCmd.Parameters.AddWithValue("@Far7ReleaseSignificance", "")
        objCmd.Parameters.AddWithValue("@Far8EventPrognosis", "")
        objCmd.Parameters.AddWithValue("@Far9WindDirectDegrees", "")
        objCmd.Parameters.AddWithValue("@Far9WindSpeed", "")
        objCmd.Parameters.AddWithValue("@Far9Precipitation", "")
        objCmd.Parameters.AddWithValue("@Far9StabilityClass", "")
        objCmd.Parameters.AddWithValue("@Far10Select1", "")
        objCmd.Parameters.AddWithValue("@Far10Time", "")
        objCmd.Parameters.AddWithValue("@Far10Date", "")
        objCmd.Parameters.AddWithValue("@Far11AffectedUnits", "")
        objCmd.Parameters.AddWithValue("@Far12AUnitPower", "")
        objCmd.Parameters.AddWithValue("@Far12ATime", "")
        objCmd.Parameters.AddWithValue("@Far12ADate", "")
        objCmd.Parameters.AddWithValue("@Far12BUnitPower", "")
        objCmd.Parameters.AddWithValue("@Far12BTime", "")
        objCmd.Parameters.AddWithValue("@Far12BDate", "")
        objCmd.Parameters.AddWithValue("@Far13Remarks", "")
        objCmd.Parameters.AddWithValue("@Far14ReleaseChar", "")
        objCmd.Parameters.AddWithValue("@Far14Units", "")
        objCmd.Parameters.AddWithValue("@Far14NobleGasses", "")
        objCmd.Parameters.AddWithValue("@Far14Iodines", "")
        objCmd.Parameters.AddWithValue("@Far14Particulautes", "")
        objCmd.Parameters.AddWithValue("@Far14Other", "")
        objCmd.Parameters.AddWithValue("@Far14Aairborne", "")
        objCmd.Parameters.AddWithValue("@Far14AstartTime", "")
        objCmd.Parameters.AddWithValue("@Far14AstartDate", "")
        objCmd.Parameters.AddWithValue("@Far14AstopTime", "")
        objCmd.Parameters.AddWithValue("@Far14AstopDate", "")
        objCmd.Parameters.AddWithValue("@Far14Bliquid", "")
        objCmd.Parameters.AddWithValue("@Far14BstartTime", "")
        objCmd.Parameters.AddWithValue("@Far14BstartDate", "")
        objCmd.Parameters.AddWithValue("@Far14BstopTime", "")
        objCmd.Parameters.AddWithValue("@Far14BendDate", "")
        objCmd.Parameters.AddWithValue("@Far15ProjectionPeriod", "")
        objCmd.Parameters.AddWithValue("@Far15EstimatedReleaseDuration", "")
        objCmd.Parameters.AddWithValue("@Far15ProjectionPerformedTime", "")
        objCmd.Parameters.AddWithValue("@Far15ProjectionPerformedDate", "")
        objCmd.Parameters.AddWithValue("@Far15AccidentType", "")
        objCmd.Parameters.AddWithValue("@Far16SiteBoundaryTEDE", "")
        objCmd.Parameters.AddWithValue("@Far16SiteBoundaryAdultThyroidCDE", "")
        objCmd.Parameters.AddWithValue("@Far16TwoMilesTEDE", "")
        objCmd.Parameters.AddWithValue("@Far16TwoMilesAdultThyroidCDE", "")
        objCmd.Parameters.AddWithValue("@Far16FiveMilesTEDE", "")
        objCmd.Parameters.AddWithValue("@Far16FiveMilesAdultThyroidCDE", "")
        objCmd.Parameters.AddWithValue("@Far16TenMilesTEDE", "")
        objCmd.Parameters.AddWithValue("@Far16MilesAdultThyroidCDE", "")
        objCmd.Parameters.AddWithValue("@Far17ApprovedBy", "")
        objCmd.Parameters.AddWithValue("@Far17Title", "")
        objCmd.Parameters.AddWithValue("@Far17Time", "")
        objCmd.Parameters.AddWithValue("@Far17Date", "")
        objCmd.Parameters.AddWithValue("@Far17NotifiedBy", "")
        objCmd.Parameters.AddWithValue("@Far17ReceivedBy", "")
        objCmd.Parameters.AddWithValue("@Far17ReceivedTime", "")
        objCmd.Parameters.AddWithValue("@Far17ReceivedDate", "")
        '  Alabama End===================================================================

        'Crystal River – Permanently Defueled ENF Start===================================================================
        objCmd.Parameters.AddWithValue("@CRDselectOne", "")
        objCmd.Parameters.AddWithValue("@CRDmessageClassification", "")
        objCmd.Parameters.AddWithValue("@CRDdate", "")
        objCmd.Parameters.AddWithValue("@CRDcontactTime", "")
        objCmd.Parameters.AddWithValue("@CRDreportedByName", "")
        objCmd.Parameters.AddWithValue("@CRDmessageNumber", "")
        objCmd.Parameters.AddWithValue("@CRDfSelectOne", "")
        objCmd.Parameters.AddWithValue("@CRDemergencyClassification", "")
        objCmd.Parameters.AddWithValue("@CRDemClassDate", "")
        objCmd.Parameters.AddWithValue("@CRDemClassTime", "")
        objCmd.Parameters.AddWithValue("@CRDemTermDate", "")
        objCmd.Parameters.AddWithValue("@CRDemTermTime", "")
        objCmd.Parameters.AddWithValue("@CRDeALNumbers", "")
        objCmd.Parameters.AddWithValue("@CRDeALDescription", "")
        objCmd.Parameters.AddWithValue("@CRDeALai", "")
        objCmd.Parameters.AddWithValue("@CRDeALaiDescription", "")
        objCmd.Parameters.AddWithValue("@CRDwindDirectionDegrees", "")
        objCmd.Parameters.AddWithValue("@CRDwindSpeed", "")
        objCmd.Parameters.AddWithValue("@CRDreleaseStatus", "")
        objCmd.Parameters.AddWithValue("@CRDreleaseSignificance", "")
        objCmd.Parameters.AddWithValue("@CRDProjTotalDose", "")
        objCmd.Parameters.AddWithValue("@CRDDistance83Mile", "")
        objCmd.Parameters.AddWithValue("@CRDfacCond", "")
        objCmd.Parameters.AddWithValue("@CRDmessageRecdName", "")
        objCmd.Parameters.AddWithValue("@CRDmessageRecdDate", "")
        objCmd.Parameters.AddWithValue("@CRDmessageRecdTime", "")
        objCmd.Parameters.AddWithValue("@CRDuserComments", "")
        'Crystal River – Permanently Defueled ENF End===================================================================

        DBConStringHelper.PrepareConnection(objConn)

        objCmd.ExecuteNonQuery()

        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)

        'Catch ex As Exception

        '    Response.Write(ex.ToString)
        '    Exit Sub

        'End Try



        Try

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spUpdateIncidentIncidentType", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@WorkSheetDescription", txtWorkSheetDescription.Text)



            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

        Catch ex As Exception

            Response.Write(ex.ToString)
            Exit Sub

        End Try




        Dim NowDate As Date = Now

        Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.

            objCmd = New SqlCommand("spInsertMostRecentUpdateByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@UpdateDate", NowDate)
            objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))
            objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Added Nuclear Power Plant")

            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

        Catch ex As Exception
            Response.Write(ex.ToString)

            Exit Sub
        End Try


        Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spUpdateIncidentReportUpdate", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@LastUpdatedByID", ns.UserID) 'oCookie.Item("UserID"))
            objCmd.Parameters.AddWithValue("@LastUpdated", NowDate)

            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

        Catch ex As Exception
            Response.Write(ex.ToString)

            Exit Sub
        End Try

        Else


        'Response.Write("Its Working!")
        'Response.End()

        'Try
        objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

        '// Enter the email and password to query/command object.
        objCmd = New SqlCommand("spActionNPP", objConn)
        objCmd.CommandType = CommandType.StoredProcedure
        objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
        objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
        objCmd.Parameters.AddWithValue("@Flag", 1)
        objCmd.Parameters.AddWithValue("@SubType", ddlSubType.SelectedValue.ToString)
        objCmd.Parameters.AddWithValue("@Situation", ddlSituation.SelectedValue.ToString)
        objCmd.Parameters.AddWithValue("@IncidentTypeLevelID", ddlNotification.SelectedValue.ToString)
        objCmd.Parameters.AddWithValue("@CSTmessageNumber", txtmessageNumber.Text)

        'Florida Start===================================================================
        Dim EmergencyType As String = ""
        If rdoDrill.Checked = True Then
            EmergencyType = "This is a Drill"
        ElseIf rdoEvent.Checked = True Then
            EmergencyType = "This is an EMERGENCY"
        End If
        objCmd.Parameters.AddWithValue("@CSTselectOne", EmergencyType)

        'ShowPlantConditions 
        Dim ShowPlantConditions As String = ""
        If rdoShowPlantConditionsYes.Checked = True Then
            ShowPlantConditions = "1"
        ElseIf rdoShowPlantConditionsNo.Checked = True Then
            ShowPlantConditions = "0"
        End If
        objCmd.Parameters.AddWithValue("@ShowPlantConditions", ShowPlantConditions)
        '  objCmd.Parameters.AddWithValue("@CSTselectOne", ddlCSTselectOne.SelectedValue.ToString)

        Dim VerificationStateWatchOffice As String = ""
        Dim VerificationDOH As String = ""
        Dim VerificationStLucie As String = ""
        Dim VerificationMartinCo As String = ""
        Dim VerificationMiamiDade As String = ""
        Dim VerificationMonroeCo As String = ""

        VerificationStateWatchOffice = IIf(chkStateWatchOffice.Checked = True, "1", "0")
        VerificationDOH = IIf(chkDOH.Checked = True, "1", "0")
        VerificationStLucie = IIf(chkStLucieCo.Checked = True, "1", "0")
        VerificationMartinCo = IIf(chkMartinCo.Checked = True, "1", "0")
        VerificationMiamiDade = IIf(chkMiamiDade.Checked = True, "1", "0")
        VerificationMonroeCo = IIf(chkMonroeCo.Checked = True, "1", "0")

        objCmd.Parameters.AddWithValue("@VerificationStateWatchOffice", VerificationStateWatchOffice)
        objCmd.Parameters.AddWithValue("@VerificationDOH", VerificationDOH)
        objCmd.Parameters.AddWithValue("@VerificationStLucie", VerificationStLucie)
        objCmd.Parameters.AddWithValue("@VerificationMartinCo", VerificationMartinCo)
        objCmd.Parameters.AddWithValue("@VerificationMiamiDade", VerificationMiamiDade)
        objCmd.Parameters.AddWithValue("@VerificationMonroeCo", VerificationMonroeCo)

        objCmd.Parameters.AddWithValue("@CSTdate", txtCSTdate.Text)
        objCmd.Parameters.AddWithValue("@CSTcontactTime", CStr(txtCSTcontactTime.Text.Trim) & CStr(txtCSTcontactTime2.Text.Trim))
        objCmd.Parameters.AddWithValue("@CSTreportedByName", txtCSTreportedByName.Text)

        objCmd.Parameters.AddWithValue("@CSTreportedFrom", "")
        objCmd.Parameters.AddWithValue("@CSTfSelectOne", "")

        'Site
        Dim Site As String = ""
            If rdoStLucieUnit1.Checked = True Then
                Site = "St. Lucie Unit 1"
            ElseIf rdoStLucieUnit2.Checked = True Then
                Site = "St. Lucie Unit 2"
            ElseIf rdoTurkeyPointUnit3.Checked = True Then
                Site = "Turkey Point Unit 3"
            ElseIf rdoTurkeyPointUnit4.Checked = True Then
                Site = "Turkey Point Unit 4"
            End If

            Dim SiteLucie1 = "0", SiteLucie2 = "0", SiteTurkey1 = "0", SiteTurkey2 = "0"
            If rdoStLucieUnit1.Checked = True Then
                SiteLucie1 = "1"
            End If
            If rdoStLucieUnit2.Checked = True Then
                SiteLucie2 = "1"
            End If
            If rdoTurkeyPointUnit3.Checked = True Then
                SiteTurkey1 = "1"
            End If
            If rdoTurkeyPointUnit4.Checked = True Then
                SiteTurkey2 = "1"
            End If
            objCmd.Parameters.AddWithValue("@SiteLucie1", SiteLucie1)
            objCmd.Parameters.AddWithValue("@SiteLucie2", SiteLucie2)
            objCmd.Parameters.AddWithValue("@SiteTurkey1", SiteTurkey1)
            objCmd.Parameters.AddWithValue("@SiteTurkey2", SiteTurkey2)

            objCmd.Parameters.AddWithValue("@CSTsite", "")
            'objCmd.Parameters.AddWithValue("@CSTsite", ddlCSTsite.SelectedValue.ToString)

            'Classification
            Dim EmergencyClassification As String = ""
        If rdoNotificationOfUnusualEvent.Checked = True Then
            EmergencyClassification = "A. Notification of Unusual Event"
        ElseIf rdoAlert.Checked = True Then
            EmergencyClassification = "B. Alert"
        ElseIf rdoSiteEmergencyArea.Checked = True Then
            EmergencyClassification = "C. Site Area Emergency"
        ElseIf rdoGeneralEmergency.Checked = True Then
            EmergencyClassification = "D. General Emergency"
        End If
        objCmd.Parameters.AddWithValue("@CSTemergencyClassification", EmergencyClassification)
        ' objCmd.Parameters.AddWithValue("@CSTemergencyClassification", ddlCSTemergencyClassification.SelectedValue.ToString)

        'Declaration
        Dim Declararion As String = ""
        If rdoEmergencyDeclaration.Checked = True Then
            Declararion = "A. Emergency Declaration"
        ElseIf rdoEmergencyTermination.Checked = True Then
            Declararion = "B. Emergency Termination"
        End If
        objCmd.Parameters.AddWithValue("@CSTdecTermSelectOne", Declararion)
        ' objCmd.Parameters.AddWithValue("@CSTdecTermSelectOne", ddlCSTdecTermSelectOne.SelectedValue.ToString)

        Dim Papa1 As String = ""
        Dim Papa2 As String = ""
        Dim Papa3 As String = ""
        Dim Delta1 As String = ""
        Dim Delta2 As String = ""
        Dim Delta3 As String = ""
        Dim Potassium As String = ""

        Papa1 = IIf(chkPapa1.Checked = True, "1", "0")
        Papa2 = IIf(chkPapa2.Checked = True, "1", "0")
        Papa3 = IIf(chkPapa3.Checked = True, "1", "0")
        Delta1 = IIf(chkDelta1.Checked = True, "1", "0")
        Delta2 = IIf(chkDelta2.Checked = True, "1", "0")
        Delta3 = IIf(chkDelta3.Checked = True, "1", "0")
        Potassium = IIf(chkPotassium.Checked = True, "1", "0")

        objCmd.Parameters.AddWithValue("@Papa1", Papa1)
        objCmd.Parameters.AddWithValue("@Papa2", Papa2)
        objCmd.Parameters.AddWithValue("@Papa3", Papa3)
        objCmd.Parameters.AddWithValue("@Delta1", Delta1)
        objCmd.Parameters.AddWithValue("@Delta2", Delta2)
        objCmd.Parameters.AddWithValue("@Delta3", Delta3)
        objCmd.Parameters.AddWithValue("@Potassium", Potassium)


        objCmd.Parameters.AddWithValue("@CSTdecTermDate", txtCSTdecTermDate.Text)
        objCmd.Parameters.AddWithValue("@CSTdecTermTime", CStr(txtCSTdecTermTime.Text.Trim) & CStr(txtCSTdecTermTime2.Text.Trim))
        objCmd.Parameters.AddWithValue("@CSTdecTermReason", "")
        objCmd.Parameters.AddWithValue("@CSTeALNumbers", txtCSTeALNumbers.Text)
        objCmd.Parameters.AddWithValue("@CSTeALDescription", txtCSTeALDescription.Text)
        Dim Additional As String = ""
        If rdoAdditionalInformationDose.Checked Then
            Additional = "Dose"
        Else
            Additional = "A. None"
        End If
        objCmd.Parameters.AddWithValue("@CSTeALai", Additional)
        ' objCmd.Parameters.AddWithValue("@CSTeALai", ddlCSTeALai.SelectedValue.ToString)

        objCmd.Parameters.AddWithValue("@CSTeALaiDescription", "")
        objCmd.Parameters.AddWithValue("@CSTwindDirectionDegrees", txtCSTwindDirectionDegrees.Text)
        objCmd.Parameters.AddWithValue("@CSTdownwindSectorsAffected", txtCSTdownwindSectorsAffected.Text)

        Dim ReleaseStatus As String = ""
        If rdoNone.Checked = True Then
            ReleaseStatus = "A. None (Go to item 11)"
        ElseIf rdoInProgress.Checked = True Then
            ReleaseStatus = "B. In Progress"
        ElseIf rdoHasOccured.Checked = True Then
            ReleaseStatus = "C. Has occurred, but stopped"
        End If
        objCmd.Parameters.AddWithValue("@CSTreleaseStatus ", ReleaseStatus)
        'objCmd.Parameters.AddWithValue("@CSTreleaseStatus ", ddlCSTreleaseStatus.SelectedValue.ToString)

        Dim SiteBoudary As String = ""
        If rdoUnderEvaluation.Checked = True Then
            SiteBoudary = "A. Under Evaluation"
        ElseIf rdoReleaseWithinNormal.Checked = True Then
            SiteBoudary = "B. Release is within normal operating limits"
        ElseIf rdoNonSignificant.Checked = True Then
            SiteBoudary = "C. Non-significant (fraction of protective action"
        ElseIf rdoPAGRang.Checked = True Then
            SiteBoudary = "D. Protective action guide range"
        ElseIf rdoLiquidRelease.Checked Then
            SiteBoudary = "Liquid Release"
        End If
        objCmd.Parameters.AddWithValue("@CSTsigCatSiteBoundary", SiteBoudary)
        'objCmd.Parameters.AddWithValue("@CSTsigCatSiteBoundary", ddlCSTsigCatSiteBoundary.SelectedValue.ToString)

        Dim UtilityRecommendations As String = ""
        If rdoNoUtilityRecommended.Checked = True Then
            UtilityRecommendations = "A. No utility recommended actions at this time."
        ElseIf rdoUtilityRecommendations.Checked = True Then
            UtilityRecommendations = "B. The utility recommends the following protective actions for the public"
        End If
        objCmd.Parameters.AddWithValue("@CSTutilRecProtAct", UtilityRecommendations)
        ' objCmd.Parameters.AddWithValue("@CSTutilRecProtAct", ddlCSTutilRecProtAct.SelectedValue.ToString)

        objCmd.Parameters.AddWithValue("@CSTevacuateZones", "")
        objCmd.Parameters.AddWithValue("@CSTshelterZones", "")
        objCmd.Parameters.AddWithValue("@CST02MilesEvacSect", "")
        objCmd.Parameters.AddWithValue("@CST02MilesShelterSect", "")
        objCmd.Parameters.AddWithValue("@CST02MilesNoActtionSect", "")
        objCmd.Parameters.AddWithValue("@CST25MilesEvacSect", "")
        objCmd.Parameters.AddWithValue("@CST25MilesShelterSect", "")
        objCmd.Parameters.AddWithValue("@CST25MilesNoActtionSect", "")
        objCmd.Parameters.AddWithValue("@CST510MilesEvacSect", "")
        objCmd.Parameters.AddWithValue("@CST510MilesShelterSect", "")
        objCmd.Parameters.AddWithValue("@CST510MilesNoActtionSect", "")

        Dim Reactor As String = ""
        If rdoReactionShutdownYes.Checked = True Then
            Reactor = "Yes"
        Else
            Reactor = "No"
        End If
        objCmd.Parameters.AddWithValue("@CST12A", Reactor)

        Dim Cooled As String = ""
        If rdoCoreAdequatelyCooledYes.Checked = True Then
            Cooled = "Yes"
        Else
            Cooled = "No"
        End If
        objCmd.Parameters.AddWithValue("@CST12B", Cooled)

        Dim Intact As String = ""
        If rdoContainmentIntactYes.Checked = True Then
            Intact = "Yes"
        Else
            Intact = "No"
        End If
        objCmd.Parameters.AddWithValue("@CST12C", Intact)

        Dim Core As String = ""
        If rdoCoreConditionStable.Checked = True Then
            Core = "Stable"
        Else
            Core = "Degrading"
        End If
        objCmd.Parameters.AddWithValue("@CST12D", Core)

        objCmd.Parameters.AddWithValue("@OneMileTEDE", txtOneMileTEDE.Text)
        objCmd.Parameters.AddWithValue("@TwoMileTEDE", txtTwoMileTEDE.Text)
        objCmd.Parameters.AddWithValue("@FiveMileTEDE", txtFiveMileTEDE.Text)
        objCmd.Parameters.AddWithValue("@TenMileTEDE", txtTenMileTEDE.Text)

        objCmd.Parameters.AddWithValue("@OneMileCDE", txtOneMileCDE.Text)
        objCmd.Parameters.AddWithValue("@TwoMileCDE", txtTwoMileCDE.Text)
        objCmd.Parameters.AddWithValue("@FiveMileCDE", txtFiveMileCDE.Text)
        objCmd.Parameters.AddWithValue("@TenMileCDE", txtTenMileCDE.Text)


        'objCmd.Parameters.AddWithValue("@CST12A", ddlCST12A.SelectedValue.ToString)
        'objCmd.Parameters.AddWithValue("@CST12B", ddlCST12B.SelectedValue.ToString)
        'objCmd.Parameters.AddWithValue("@CST12C", ddlCST12C.SelectedValue.ToString)
        ' objCmd.Parameters.AddWithValue("@CST12D", ddlCST12D.SelectedValue.ToString)

        objCmd.Parameters.AddWithValue("@CST13A", txtCST13A.Text)
        objCmd.Parameters.AddWithValue("@CSTProjThyroidDose", "")
        objCmd.Parameters.AddWithValue("@CSTProjTotalDose", "")
        objCmd.Parameters.AddWithValue("@CST13B", txtCST13B.Text)
        objCmd.Parameters.AddWithValue("@CST14A", "")
        objCmd.Parameters.AddWithValue("@CST14B", "")
        objCmd.Parameters.AddWithValue("@CST14C", "")
        objCmd.Parameters.AddWithValue("@CST14D", "")
        objCmd.Parameters.AddWithValue("@CST14E", "")
        objCmd.Parameters.AddWithValue("@CST14F", "")
        objCmd.Parameters.AddWithValue("@CST14G", "")
        objCmd.Parameters.AddWithValue("@CST14H", "")
        objCmd.Parameters.AddWithValue("@CST14I", "")
        objCmd.Parameters.AddWithValue("@CST15NamE", txtCST15Name.Text)
        objCmd.Parameters.AddWithValue("@CST15Date", txtCST15Date.Text)
        objCmd.Parameters.AddWithValue("@CST15Time", CStr(txtCST15Time.Text.Trim) & CStr(txtCST15Time2.Text.Trim))
        objCmd.Parameters.AddWithValue("@CSTuserComments", "")

        objCmd.Parameters.AddWithValue("@ApproveName", "")
        objCmd.Parameters.AddWithValue("@ApproveTime", "")
        objCmd.Parameters.AddWithValue("@ApproveDate", "")
        'Florida End===================================================================

        'Alabama Start=================================================================
        objCmd.Parameters.AddWithValue("@Far1SelectOne", "")
        objCmd.Parameters.AddWithValue("@Far1MessageNumber", "")
        objCmd.Parameters.AddWithValue("@Far2SelectOne", "")
        objCmd.Parameters.AddWithValue("@Far2NotificationTime", "" & "")
        objCmd.Parameters.AddWithValue("@Far2NotificationDate", "")
        objCmd.Parameters.AddWithValue("@Far2AuthenticationNumber", "")
        objCmd.Parameters.AddWithValue("@Far3Site", "")
        objCmd.Parameters.AddWithValue("@Far3ConfirmationPhoneNumber", "")
        objCmd.Parameters.AddWithValue("@Far4EmergencyClassification", "")
        objCmd.Parameters.AddWithValue("@Far4BasedEALnumber", "")
        objCmd.Parameters.AddWithValue("@Far4EALdescription", "")
        objCmd.Parameters.AddWithValue("@Far5a", "")
        objCmd.Parameters.AddWithValue("@Far5b", "")
        objCmd.Parameters.AddWithValue("@Far5bText", "")
        objCmd.Parameters.AddWithValue("@Far5c", "")
        objCmd.Parameters.AddWithValue("@Far5cText", "")
        objCmd.Parameters.AddWithValue("@Far5d", "")
        objCmd.Parameters.AddWithValue("@Far5e", "")
        objCmd.Parameters.AddWithValue("@Far5eText", "")
        objCmd.Parameters.AddWithValue("@Far6EmergencyRelease", "")
        objCmd.Parameters.AddWithValue("@Far7ReleaseSignificance", "")
        objCmd.Parameters.AddWithValue("@Far8EventPrognosis", "")
        objCmd.Parameters.AddWithValue("@Far9WindDirectDegrees", "")
        objCmd.Parameters.AddWithValue("@Far9WindSpeed", "")
        objCmd.Parameters.AddWithValue("@Far9Precipitation", "")
        objCmd.Parameters.AddWithValue("@Far9StabilityClass", "")
        objCmd.Parameters.AddWithValue("@Far10Select1", "")
        objCmd.Parameters.AddWithValue("@Far10Time", "")
        objCmd.Parameters.AddWithValue("@Far10Date", "")
        objCmd.Parameters.AddWithValue("@Far11AffectedUnits", "")
        objCmd.Parameters.AddWithValue("@Far12AUnitPower", "")
        objCmd.Parameters.AddWithValue("@Far12ATime", "")
        objCmd.Parameters.AddWithValue("@Far12ADate", "")
        objCmd.Parameters.AddWithValue("@Far12BUnitPower", "")
        objCmd.Parameters.AddWithValue("@Far12BTime", "")
        objCmd.Parameters.AddWithValue("@Far12BDate", "")
        objCmd.Parameters.AddWithValue("@Far13Remarks", "")
        objCmd.Parameters.AddWithValue("@Far14ReleaseChar", "")
        objCmd.Parameters.AddWithValue("@Far14Units", "")
        objCmd.Parameters.AddWithValue("@Far14NobleGasses", "")
        objCmd.Parameters.AddWithValue("@Far14Iodines", "")
        objCmd.Parameters.AddWithValue("@Far14Particulautes", "")
        objCmd.Parameters.AddWithValue("@Far14Other", "")
        objCmd.Parameters.AddWithValue("@Far14Aairborne", "")
        objCmd.Parameters.AddWithValue("@Far14AstartTime", "")
        objCmd.Parameters.AddWithValue("@Far14AstartDate", "")
        objCmd.Parameters.AddWithValue("@Far14AstopTime", "")
        objCmd.Parameters.AddWithValue("@Far14AstopDate", "")
        objCmd.Parameters.AddWithValue("@Far14Bliquid", "")
        objCmd.Parameters.AddWithValue("@Far14BstartTime", "")
        objCmd.Parameters.AddWithValue("@Far14BstartDate", "")
        objCmd.Parameters.AddWithValue("@Far14BstopTime", "")
        objCmd.Parameters.AddWithValue("@Far14BendDate", "")
        objCmd.Parameters.AddWithValue("@Far15ProjectionPeriod", "")
        objCmd.Parameters.AddWithValue("@Far15EstimatedReleaseDuration", "")
        objCmd.Parameters.AddWithValue("@Far15ProjectionPerformedTime", "")
        objCmd.Parameters.AddWithValue("@Far15ProjectionPerformedDate", "")
        objCmd.Parameters.AddWithValue("@Far15AccidentType", "")
        objCmd.Parameters.AddWithValue("@Far16SiteBoundaryTEDE", "")
        objCmd.Parameters.AddWithValue("@Far16SiteBoundaryAdultThyroidCDE", "")
        objCmd.Parameters.AddWithValue("@Far16TwoMilesTEDE", "")
        objCmd.Parameters.AddWithValue("@Far16TwoMilesAdultThyroidCDE", "")
        objCmd.Parameters.AddWithValue("@Far16FiveMilesTEDE", "")
        objCmd.Parameters.AddWithValue("@Far16FiveMilesAdultThyroidCDE", "")
        objCmd.Parameters.AddWithValue("@Far16TenMilesTEDE", "")
        objCmd.Parameters.AddWithValue("@Far16MilesAdultThyroidCDE", "")
        objCmd.Parameters.AddWithValue("@Far17ApprovedBy", "")
        objCmd.Parameters.AddWithValue("@Far17Title", "")
        objCmd.Parameters.AddWithValue("@Far17Time", "")
        objCmd.Parameters.AddWithValue("@Far17Date", "")
        objCmd.Parameters.AddWithValue("@Far17NotifiedBy", "")
        objCmd.Parameters.AddWithValue("@Far17ReceivedBy", "")
        objCmd.Parameters.AddWithValue("@Far17ReceivedTime", "")
        objCmd.Parameters.AddWithValue("@Far17ReceivedDate", "")
        'Alabama End===================================================================

        'Crystal River – Permanently Defueled ENF Start===================================================================
        objCmd.Parameters.AddWithValue("@CRDselectOne", "")
        objCmd.Parameters.AddWithValue("@CRDmessageClassification", "")
        objCmd.Parameters.AddWithValue("@CRDdate", "")
        objCmd.Parameters.AddWithValue("@CRDcontactTime", "")
        objCmd.Parameters.AddWithValue("@CRDreportedByName", "")
        objCmd.Parameters.AddWithValue("@CRDmessageNumber", "")
        objCmd.Parameters.AddWithValue("@CRDfSelectOne", "")
        objCmd.Parameters.AddWithValue("@CRDemergencyClassification", "")
        objCmd.Parameters.AddWithValue("@CRDemClassDate", "")
        objCmd.Parameters.AddWithValue("@CRDemClassTime", "")
        objCmd.Parameters.AddWithValue("@CRDemTermDate", "")
        objCmd.Parameters.AddWithValue("@CRDemTermTime", "")
        objCmd.Parameters.AddWithValue("@CRDeALNumbers", "")
        objCmd.Parameters.AddWithValue("@CRDeALDescription", "")
        objCmd.Parameters.AddWithValue("@CRDeALai", "")
        objCmd.Parameters.AddWithValue("@CRDeALaiDescription", "")
        objCmd.Parameters.AddWithValue("@CRDwindDirectionDegrees", "")
        objCmd.Parameters.AddWithValue("@CRDwindSpeed", "")
        objCmd.Parameters.AddWithValue("@CRDreleaseStatus", "")
        objCmd.Parameters.AddWithValue("@CRDreleaseSignificance", "")
        objCmd.Parameters.AddWithValue("@CRDProjTotalDose", "")
        objCmd.Parameters.AddWithValue("@CRDDistance83Mile", "")
        objCmd.Parameters.AddWithValue("@CRDfacCond", "")
        objCmd.Parameters.AddWithValue("@CRDmessageRecdName", "")
        objCmd.Parameters.AddWithValue("@CRDmessageRecdDate", "")
        objCmd.Parameters.AddWithValue("@CRDmessageRecdTime", "")
        objCmd.Parameters.AddWithValue("@CRDuserComments", "")
        'Crystal River – Permanently Defueled ENF End===================================================================


        DBConStringHelper.PrepareConnection(objConn)

        objCmd.ExecuteNonQuery()

        objCmd.Dispose()
        objCmd = Nothing
        DBConStringHelper.FinalizeConnection(objConn)

        'Catch ex As Exception

        '    Response.Write(ex.ToString)
        '    Exit Sub

        'End Try

        Try

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spUpdateIncidentIncidentType", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentIncidentTypeID", Request("IncidentIncidentTypeID"))
            objCmd.Parameters.AddWithValue("@WorkSheetDescription", txtWorkSheetDescription.Text)



            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

        Catch ex As Exception

            Response.Write(ex.ToString)
            Exit Sub

        End Try


        Dim NowDate As Date = Now

        Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.

            objCmd = New SqlCommand("spInsertMostRecentUpdateByIncidentID", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@UpdateDate", NowDate)
            objCmd.Parameters.AddWithValue("@UserID", ns.UserID) 'oCookie.Item("UserID"))
            objCmd.Parameters.AddWithValue("@MostRecentUpdate", "Updated Nuclear Power Plant")

            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

        Catch ex As Exception
            Response.Write(ex.ToString)

            Exit Sub
        End Try

        Try
            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString

            '// Enter the email and password to query/command object.
            objCmd = New SqlCommand("spUpdateIncidentReportUpdate", objConn)
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Parameters.AddWithValue("@IncidentID", Request("IncidentID"))
            objCmd.Parameters.AddWithValue("@LastUpdatedByID", ns.UserID) 'oCookie.Item("UserID"))
            objCmd.Parameters.AddWithValue("@LastUpdated", NowDate)

            DBConStringHelper.PrepareConnection(objConn)

            objCmd.ExecuteNonQuery()

            objCmd.Dispose()
            objCmd = Nothing
            DBConStringHelper.FinalizeConnection(objConn)

        Catch ex As Exception
            Response.Write(ex.ToString)

            Exit Sub
        End Try

        End If


    End Sub

    Protected Sub ErrorChecks()
        Dim strError As New System.Text.StringBuilder

        'Start The Error String.
        strError.Append("<font size='3'><span  style='color:#fe5105;'> ")

        If ddlSubType.SelectedValue = "Select an Option" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Sub-Type. <br />")
            globalHasErrors = True
        End If

        If ddlSituation.SelectedValue = "Select an Option" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Situation. <br />")
            globalHasErrors = True
        End If

        If txtWorkSheetDescription.Text = "" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Description. <br />")
            globalHasErrors = True
        End If

        If ddlNotification.SelectedValue = "Select an Option" Then
            strError.Append("<img alt='Error Red X Bullet' src='Images/BulletRedXIcon.png' /> &nbsp; You must provide a Notification. <br />")
            globalHasErrors = True
        End If



        'Finish the Error String.
        strError.Append("</span></font><br />")

        'Add Errors "If Any" to the Labels.
        ' lblMessage.Text = strError.ToString
    End Sub
End Class
