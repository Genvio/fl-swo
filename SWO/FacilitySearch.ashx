<%@ WebHandler Language="VB" Class="FacilitySearch" %>

Imports System
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Text
Imports System.Web.Script.Serialization
Imports System.Data

Public Class FacilitySearch : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        context.Response.ContentEncoding = Encoding.UTF8

        'Validate querystring
        '   1) term must be present
        '   2) county and FacilityType are optional
        '   3) nothing else should be present
        Dim blnIsValid As Boolean = True
        Dim collQS As NameValueCollection

        If context.Request.QueryString.Count > 3 Then blnIsValid = False
        If context.Request.QueryString("term") = "" Then blnIsValid = False
        collQS = context.Request.QueryString

        For i As Int16 = 0 To collQS.Count - 1
            If collQS.Keys(i) <> "county" AndAlso collQS.Keys(i) <> "FacilityType" AndAlso collQS.Keys(i) <> "term" Then blnIsValid = False
        Next i

        If blnIsValid = False Then
            context.Response.Write("[""Bad querystring--search aborted""]")
            Exit Sub
        End If

        Try
            Using oCxn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("dbConnectionString3").ToString())
                oCxn.Open()
                Using oCmd As SqlCommand = New SqlCommand()
                    Dim oDR As SqlDataReader
                    oCmd.Connection = oCxn
                    oCmd.CommandType = CommandType.StoredProcedure
                    oCmd.CommandText = "dbo.spSelectFacilities"

                    If context.Request.QueryString("county") <> "" Then oCmd.Parameters.AddWithValue("@countyName", context.Request.QueryString("county"))
                    If context.Request.QueryString("FacilityType") <> "" Then oCmd.Parameters.AddWithValue("@facilityType", context.Request.QueryString("FacilityType"))
                    oCmd.Parameters.AddWithValue("@facilityName", context.Request.QueryString("term"))
                    oDR = oCmd.ExecuteReader()

                    If oDR.HasRows Then
                        Dim sb As New StringBuilder
                        sb.Append("[")

                        While oDR.Read()
                            sb.Append("{""label"": """ & oDR("destination_facility") & """, ""value"": """ & oDR("destination_facility_name") & """, ""street"": """ & oDR("address1") & """, ""lat"": """ & oDR("Y") & """, ""lon"": """ & oDR("X") & """, ""county"": """ & oDR("county") & """, ")
                            sb.Append("""aptnum"": """ & oDR("address2") & """, ""city"": """ & oDR("CITY") & """, ""zip"": """ & oDR("ZIP") & """, ""usng"": """ & oDR("USNG") & """, ""facility"": """ & oDR("destination_facility_name") & """, ""state"": """ & oDR("STATE") & """},")
                        End While

                        sb.Remove(sb.Length - 1, 1)
                        sb.Append("]")
                        context.Response.Write(sb.ToString.TrimEnd(","))
                    Else
                        context.Response.Write("[""No matching facilities""]")
                    End If

                End Using
            End Using
        Catch ex As Exception
            context.Response.Write("[""Error retrieving facilities""]")
            Throw New ApplicationException("spSelectFacilities may have failed--is the GIS_MASTER_SITE view ok?")
        End Try
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class