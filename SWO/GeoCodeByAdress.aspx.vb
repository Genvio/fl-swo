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
Imports System.Net
Imports System.Xml

Partial Class GeoCodeByAdress
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

    Dim globalIsSaved As Boolean
    Public AuditHelper As New AuditHelp

    Public MrDataGrabber As New DataGrabber

    Dim globalAuditAction As String = ""
    Dim globalHasErrors As Boolean = False
    Dim globalMessage As String
    Dim globalCurrentStep As Integer
    Dim globalAction As String
    Dim globalParameter As String
    'Dim oCookie As System.Web.HttpCookie
    Const js As String = "TADDScript.js"

    'Page Load
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'oCookie = Request.Cookies(Application("ApplicationEnvironment").ToString)
        '// Add cookie
        'Response.Cookies.Add(oCookie)

        ''For Now we are dependent on the URL Variable, If none Redirect back to 
        'If Request("IncidentID") = "" Then

        '    Response.Redirect("Incident.aspx")

        'End If

        If Page.IsPostBack = False Then

            ddlCity.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objCmd = New SqlCommand("spSelectCity", objConn)
            objCmd.CommandType = CommandType.StoredProcedure

            'objCmd.Parameters.AddWithValue("@OrderBy", "") Optional Parameter

            DBConStringHelper.PrepareConnection(objConn) 'Open the connection
            ddlCity.DataSource = objCmd.ExecuteReader()
            ddlCity.DataBind()
            DBConStringHelper.FinalizeConnection(objConn) 'Close the connection

            objCmd = Nothing

            'add an "Select an Option" item to the list
            ddlCity.Items.Insert(0, New ListItem("Select A City", "0"))
            ddlCity.Items(0).Selected = True


            ddlState.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            objConn.ConnectionString = ConfigurationManager.ConnectionStrings("dbConnectionString").ConnectionString
            objCmd = New SqlCommand("spSelectState", objConn)
            objCmd.CommandType = CommandType.StoredProcedure

            'objCmd.Parameters.AddWithValue("@OrderBy", "") Optional Parameter

            DBConStringHelper.PrepareConnection(objConn) 'Open the connection
            ddlState.DataSource = objCmd.ExecuteReader()
            ddlState.DataBind()
            DBConStringHelper.FinalizeConnection(objConn) 'Close the connection

            objCmd = Nothing

            'add an "Select an Option" item to the list
            ddlState.Items(8).Selected = True

        End If



    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

     
        Dim URL As String = ""

        Dim urlStart As String = "http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/findAddressCandidates?Address="

        Dim urlMid As String = "&City=&State=&Zip="

        Dim urlEnd As String = "&Zip4=&Country=&outFields=&f=json"

        'Dim DataType As String = strDataType

        ' Get data stream from remote server
        Dim data As Stream
        Dim client As WebClient = New WebClient()
        Dim strAddress As String = ""

        Dim sString As String = txtAddress.Text

        Dim sWords() As String = sString.Split(" ")
        Dim Zip As String = txtZip.Text.Trim

        Dim i As Integer

        For i = 0 To sWords.Length - 1
            'Response.Write(sWords(i) + "<br  />")
            strAddress = strAddress & sWords(i) & "+"
        Next
        strAddress = strAddress.Remove(strAddress.Length - 1, 1)

        URL = urlStart + strAddress + urlMid + Zip + urlEnd

        'Response.Write(URL)
        'Response.End()



        data = client.OpenRead(URL)

        Dim reader As StreamReader = New StreamReader(data)

        Dim webPageTable As String = reader.ReadToEnd
        Dim xString As String = webPageTable
        Dim yString As String = webPageTable

        Dim CharCount As Integer = webPageTable.Length
        Dim xStart As Integer = InStr(webPageTable, "X:")
        Dim YStart As Integer = InStr(webPageTable, "Y:")

        'Response.Write(Mid(xString, xStart + 3, 9))
        'Response.Write("<br>")
        'Response.Write(Mid(yString, YStart + 2, 9))

        lblResults.Text = Mid(yString, YStart + 2, 9) & " , " & Mid(xString, xStart + 3, 9)
        'lblResults.Text = webPageTable
        'Response.End()


    End Sub


    Protected Sub btnSubmit2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit2.Click
        Dim URL As String = ""

        Dim urlStart As String = "http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/findAddressCandidates?Address="

        Dim urlMid As String = "&City="

        Dim urlMid2 As String = "&State="

        Dim urlEnd As String = "&Zip=&Zip4=&Country=&outFields=&f=json"

        'Dim DataType As String = strDataType

        ' Get data stream from remote server
        Dim data As Stream
        Dim client As WebClient = New WebClient()
        Dim strAddress As String = ""
        Dim strCity As String = ""
        Dim strState As String = ddlState.SelectedItem.ToString

        Dim sString As String = txtAddress2.Text

        Dim sWords() As String = sString.Split(" ")

        Dim sString2 As String = ddlCity.SelectedItem.ToString

        Dim sWords2() As String = sString2.Split(" ")

        Dim i As Integer

        'Response.Write(sString2)
        'Response.End()
        Dim a As Integer = 0
        For i = 0 To sWords.Length - 1
            'Response.Write(sWords(i) + "<br  />")
            strAddress = strAddress & sWords(i) & "+"
        Next
        strAddress = strAddress.Remove(strAddress.Length - 1, 1)


        For i = 0 To sWords2.Length - 1
            'Response.Write(sWords(i) + "<br  />")
            strCity = strCity & sWords2(i) & "+"
            a = a + 1
        Next

        strCity = strCity.Remove(strCity.Length - 1, 1)


        URL = urlStart + strAddress + urlMid + strCity + urlMid2 + strState + urlEnd

        'Response.Write(URL)
        'Response.End()



        data = client.OpenRead(URL)

        Dim reader As StreamReader = New StreamReader(data)

        Dim webPageTable As String = reader.ReadToEnd
        Dim xString As String = webPageTable
        Dim yString As String = webPageTable

        Dim CharCount As Integer = webPageTable.Length
        Dim xStart As Integer = InStr(webPageTable, """x"":")
        Dim YStart As Integer = InStr(webPageTable, """y"":")

        Response.Write(xStart & "<br>")
        Response.Write(YStart & "<br>")


        lblResults.Text = Mid(yString, YStart + 4, 9) & " , " & Mid(xString, xStart + 4, 9)
        'lblResults.Text = webPageTable
        'Response.End()

    End Sub
End Class
