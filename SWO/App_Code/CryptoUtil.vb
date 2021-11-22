Imports Microsoft.VisualBasic

Imports System.Diagnostics
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Public Class CryptoUtil

    '8 bytes randomly selected for both the Key and the Initialization Vector
    'the IV is used to encrypt the first block of text so that any repetitive 
    'patterns are not apparent
    Private Shared KEY_64() As Byte = {42, 16, 93, 156, 78, 4, 218, 32}
    Private Shared IV_64() As Byte = {55, 103, 246, 79, 36, 99, 167, 3}

    '24 byte or 192 bit key and IV for TripleDES
    Private Shared KEY_192() As Byte = {42, 16, 93, 156, 78, 4, 218, 32, _
            15, 167, 44, 80, 26, 250, 155, 112, _
            2, 94, 11, 204, 119, 35, 184, 197}
    Private Shared IV_192() As Byte = {55, 103, 246, 79, 36, 99, 167, 3, _
            42, 5, 62, 83, 184, 7, 209, 13, _
            145, 23, 200, 58, 173, 10, 121, 222}

    'MD5 encryption
    Public Shared Function EncryptMD5(ByVal value As String) As String

        Dim strEncripted As String
        Dim arrHashInput As Byte()
        Dim arrHashOutput As Byte()
        Dim objMD5 As System.Security.Cryptography.MD5CryptoServiceProvider

        objMD5 = New System.Security.Cryptography.MD5CryptoServiceProvider
        arrHashInput = Convert2ByteArray(value)
        arrHashOutput = objMD5.ComputeHash(arrHashInput)
        strEncripted = BitConverter.ToString(arrHashOutput)

        objMD5 = Nothing

        Return strEncripted

    End Function

    'MD5 Decryption
    Public Shared Function DecryptMD5(ByVal value As String) As String

        Dim strDecrypted As String
        'Dim arrHashInput As Byte()
        Dim arrHashOutput As Byte()
        Dim objMD5 As System.Security.Cryptography.MD5CryptoServiceProvider

        objMD5 = New System.Security.Cryptography.MD5CryptoServiceProvider
        strDecrypted = Convert2StringArray(value)
        arrHashOutput = objMD5.ComputeHash(Convert2ByteArray(strDecrypted))
        strDecrypted = BitConverter.ToString(arrHashOutput)

        objMD5 = Nothing

        Dim i As Integer, c As Integer

        For i = 1 To Len(value)
            c = Asc(Mid$(value, i, 1))
            c = c - Asc(Mid$(value, (i Mod Len(value)) + 1, 1))
            strDecrypted = strDecrypted & Chr(c And &HFF)
        Next i


        Return strDecrypted

    End Function

    Private Shared Function Convert2ByteArray(ByVal strInput As String) As Byte()

        Dim intCounter As Integer
        Dim arrChar As Char()

        arrChar = strInput.ToCharArray()

        Dim arrByte(arrChar.Length - 1) As Byte

        For intCounter = 0 To arrByte.Length - 1
            arrByte(intCounter) = Convert.ToByte(arrChar(intCounter))
        Next

        Return arrByte

    End Function

    Private Shared Function Convert2StringArray(ByVal strInput As String) As String

        Dim charArray(strInput.Length) As Char
        Dim len As Integer = strInput.Length - 1
        Dim i As Integer = 0
        While i <= len
            charArray(i) = strInput(len - i)
            System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
        End While
        Return New String(charArray)

    End Function

    'Standard DES encryption
    Public Shared Function Encrypt(ByVal value As String) As String
        If value <> "" Then
            Dim cryptoProvider As DESCryptoServiceProvider = New DESCryptoServiceProvider()
            Dim ms As MemoryStream = New MemoryStream()
            Dim cs As CryptoStream = New CryptoStream(ms, cryptoProvider.CreateEncryptor(KEY_64, IV_64), CryptoStreamMode.Write)
            Dim sw As StreamWriter = New StreamWriter(cs)

            sw.Write(value)
            sw.Flush()
            cs.FlushFinalBlock()
            ms.Flush()

            'convert back to a string
            Return Convert.ToBase64String(ms.GetBuffer(), 0, ms.Length)
        Else
            Return ""
        End If
    End Function

    'Standard DES decryption
    Public Shared Function Decrypt(ByVal value As String) As String
        If value <> "" Then
            Dim cryptoProvider As DESCryptoServiceProvider = New DESCryptoServiceProvider()

            'convert from string to byte array
            Dim buffer As Byte() = Convert2ByteArray(value)
            Dim ms As MemoryStream = New MemoryStream(buffer)
            Dim cs As CryptoStream = New CryptoStream(ms, cryptoProvider.CreateDecryptor(KEY_64, IV_64), CryptoStreamMode.Read)
            Dim sr As StreamReader = New StreamReader(cs)

            Return sr.ReadToEnd()
        Else
            Return ""
        End If
    End Function

    'TRIPLE DES encryption
    Public Shared Function EncryptTripleDES2(ByVal value As String) As String
        If value <> "" Then
            Dim cryptoProvider As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()
            Dim ms As MemoryStream = New MemoryStream()
            Dim cs As CryptoStream = New CryptoStream(ms, cryptoProvider.CreateEncryptor(KEY_192, IV_192), CryptoStreamMode.Write)
            Dim sw As StreamWriter = New StreamWriter(cs)

            sw.Write(value)
            sw.Flush()
            cs.FlushFinalBlock()
            ms.Flush()

            'convert back to a string
            Return Convert.ToBase64String(ms.GetBuffer(), 0, ms.Length)
        End If
        Return ""
    End Function

    Public Shared Function EncryptTripleDes(ByVal value As String) As String

        Dim cryptoProvider As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider

        Dim ms As MemoryStream = New MemoryStream
        Dim cs As CryptoStream = New CryptoStream(ms, cryptoProvider.CreateEncryptor(KEY_192, IV_192), CryptoStreamMode.Write)
        Dim sw As StreamWriter = New StreamWriter(cs)
        sw.Write(value)
        sw.Flush()
        cs.FlushFinalBlock()
        ms.Flush()
        'convert back to a string
        Return Convert.ToBase64String(ms.GetBuffer(), 0, ms.Length)

    End Function

    'TRIPLE DES decryption
    Public Shared Function DecryptTripleDES2(ByVal value As String) As String
        If value <> "" Then
            Dim cryptoProvider As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()

            'convert from string to byte array
            Dim buffer As Byte() = Convert2ByteArray(value)
            Dim ms As MemoryStream = New MemoryStream(buffer)
            Dim cs As CryptoStream = _
                New CryptoStream(ms, cryptoProvider.CreateDecryptor(KEY_192, IV_192), CryptoStreamMode.Read)
            Dim sr As StreamReader = New StreamReader(cs)

            Return sr.ReadToEnd()
        Else
            Return ""
        End If
    End Function

    Public Shared Function DecryptTripleDes(ByVal value As String) As String
        If value <> "" Then
            Dim cryptoProvider As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider

            'convert from string to byte array

            Dim buffer As Byte() = Convert.FromBase64String(value)

            Dim ms As MemoryStream = New MemoryStream(buffer)

            Dim cs As CryptoStream = New CryptoStream(ms, cryptoProvider.CreateDecryptor(KEY_192, IV_192), CryptoStreamMode.Read)

            Dim sr As StreamReader = New StreamReader(cs)

            Return sr.ReadToEnd()
        Else
            Return ""
        End If

    End Function
End Class

