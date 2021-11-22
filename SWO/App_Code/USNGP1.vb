Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Net.Mail

Public Class USNGP1

    Public sq1 As String = ""
    Public sq2 As String = ""
    Public north As String = "0"
    Public east As String = "0"
    Public zone As Integer = 0
    Public lett As String = ""
    Public precision As Decimal = 0.0

    'ret.N=appxNorth*1000000+Number(north)*Math.pow(10,5-north.length);
    'ret.E=appxEast*100000+Number(east)*Math.pow(10,5-east.length);
    'ret.zone=zone;
    'ret.letter=let;
End Class


