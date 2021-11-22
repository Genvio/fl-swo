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

Partial Class ViewMaps
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

    Dim globalHasErrors As Boolean = False

    'Cookie for the Login Info
    'Public ObjCookie As System.Web.HttpCookie
    'Dim oCookie As System.Web.HttpCookie

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim localLat As String = Request("Lat")
        Dim localLong As String = Request("Long")

        If localLat <> "0" Then

            pnlShowMaps.Visible = True
            pnlShowMessage.Visible = False

            ' imgGmapOverview.ImageUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" & localLat & "," & localLong & "&markers=size:mid|color:red|" & localLat & "," & localLong & "&zoom=7&size=325x275&maptype=street&format=jpg&key=ABQIAAAAaa6B5ZMUVanPrZJU5dhtshRzymbT3klSnJpNv7EI1uNYq_UBqhTmwXd4YDorUwqRsabizyja-ZgPoQ"
            'imgGmapOverview.NavigateUrl = "http://map.floridadisaster.org/mapper/Index.htm?lat=" & localLat & "&lng=" & localLong & "&zoom=12"
            imgGmapOverview.NavigateUrl = "http://maps.google.com/maps?q=" & localLat & "," & localLong

            ' imgGmapDetail.ImageUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" & localLat & "," & localLong & "&markers=size:mid|color:red|" & localLat & "," & localLong & "&zoom=14&size=325x275&maptype=street&format=jpg&key=ABQIAAAAaa6B5ZMUVanPrZJU5dhtshRzymbT3klSnJpNv7EI1uNYq_UBqhTmwXd4YDorUwqRsabizyja-ZgPoQ"
            'imgGmapDetail.NavigateUrl = "http://map.floridadisaster.org/mapper/Index.htm?lat=" & localLat & "&lng=" & localLong & "&zoom=12"
            imgGmapDetail.NavigateUrl = "https://floridadisaster.maps.arcgis.com/apps/webappviewer/index.html?id=74f3e78117fd44b28ffec5adc30c6024&scale=2000&marker=" & localLong & "," & localLat

        Else

            pnlShowMaps.Visible = False
            pnlShowMessage.Visible = True

        End If

    End Sub

End Class
