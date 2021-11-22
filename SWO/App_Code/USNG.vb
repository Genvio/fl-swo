Imports GeoUtility.GeoSystem

''' <summary>
''' Provides methods for converting to and from USNG coordinates
''' </summary>
''' <remarks>Currently uses the GeoUtility library</remarks>
Public Class USNG

    ''' <summary>
    ''' Converts a latitude/longitude to USNG coordinates.
    ''' </summary>
    ''' <param name="latitude"></param>
    ''' <param name="longitude"></param>
    ''' <param name="precision">The number of digits of the easting and northing.</param>
    ''' <returns>The USNG coordinates.</returns>
    ''' <remarks></remarks>
    Function LLtoUSNG(ByVal latitude As String, ByVal longitude As String, ByVal precision As Integer) As String

        Dim geo As New Geographic(CDbl(longitude), CDbl(latitude))

        Dim mgrs As MGRS = geo

        ' GeoUtility Precision is not implemented...
        'mgrs.Precision = precision

        'Using mgrs.East and mgrs.North (Doubles), below, results in the loss of leading zeros. Must concatenate leading zeros back in.
        Dim strEastString As String = ""
        Dim strNorthString As String = ""

        If mgrs.EastString.StartsWith("0") Then
            Dim aEastString = mgrs.EastString.ToCharArray()
            Dim zeros As IEnumerable(Of Char) = aEastString.TakeWhile(Function(z) z = "0")
            For Each z As Char In zeros
                strEastString += z.ToString()
            Next
        End If

        If mgrs.NorthString.StartsWith("0") Then
            Dim aNorthString = mgrs.NorthString.ToCharArray()
            Dim zeros As IEnumerable(Of Char) = aNorthString.TakeWhile(Function(z) z = "0")
            For Each z As Char In zeros
                strNorthString += z.ToString()
            Next
        End If

        Return String.Format("{0}{1} {2} {3} {4}", _
                             mgrs.Zone, _
                             mgrs.Band, _
                             mgrs.Grid, _
                             strEastString & Math.Floor(mgrs.East / Math.Pow(10, 5 - precision)), _
                             strNorthString & Math.Floor(mgrs.North / Math.Pow(10, 5 - precision)))

        'Return mgrs.ToLongString()

    End Function

    ''' <summary>
    ''' Converts USNG coordinates to latitude/longitude.
    ''' </summary>
    ''' <param name="USNGToreplace">The USNG coordinates (e.g. "17R KQ 13540 33467")</param>
    ''' <returns>The latitude/longitude in a Decimal array.  Item 0 is latitude, 1 is longitude.</returns>
    ''' <remarks>Throws an exception if the USNG is invalid.</remarks>
    Function USNGtoLL(ByVal USNGToreplace As String) As Decimal()

        Dim mgrs = New MGRS(USNGToreplace)

        Dim geo As Geographic = mgrs

        Dim d(2) As Decimal
        d(0) = geo.Latitude
        d(1) = geo.Longitude

        Return d

    End Function

End Class


