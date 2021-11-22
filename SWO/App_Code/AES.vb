Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Security.Cryptography


Public Class AES
    ' Symmetric algorithm interface is used to store the AES service provider  
    Private AESProvider As SymmetricAlgorithm

    Private Shared _iterations As Integer = 1000
    Private Shared _keySize As Integer = 256


    ''' <summary>  
    ''' Constructor for AES class that takes a byte array for the key  
    ''' </summary>  
    ''' <param name="key">256 bit key (32 bytes)</param>  
    Public Sub New(key As Byte())
        ' Throw error if key is not 256 bits  
        'if (key.Length != 32) throw new CryptographicException("Key must be 256 bits (32 bytes)");  

        ' Initialize AESProvider with AES algorithm service  
        AESProvider = New AesCryptoServiceProvider()
        AESProvider.KeySize = _keySize

        ' Set the key for AESProvider  
        AESProvider.Key = key
    End Sub

    ''' <summary>  
    ''' Constructor for AES class that generates the key from a hashed, salted password  
    ''' </summary>  
    ''' <param name="passphrase">passphrase used to generate the key (Minimum of 8 characters)</param>  
    ''' <param name="salt">Salt used to secure hash from rainbow table attacks (Minimum of 8 characters)</param>  
    Public Sub New(passphrase As String, salt As String)
        ' Throw error if the password or salt are too short  
        If passphrase.Length < 8 Then
            Throw New CryptographicException("Password must be at least 8 characters long")
        End If
        If salt.Length < 8 Then
            Throw New CryptographicException("Salt must be at least 8 characters long")
        End If

        ' Initialize AESProvider with AES algorithm service  
        AESProvider = New AesCryptoServiceProvider()
        AESProvider.KeySize = 256

        ' Initialize a hasher with the 256 bit SHA algorithm  
        Dim sha256 As SHA256 = System.Security.Cryptography.SHA256.Create()

        ' Hash salted password  
        Dim key As Byte() = sha256.ComputeHash(UnicodeEncoding.Unicode.GetBytes(passphrase & salt))

        ' Set the key for AESProvider  

        AESProvider.Key = key
    End Sub

    ''' <summary>  
    ''' Encrypts a string with AES algorithm  
    ''' </summary>  
    ''' <param name="plainText">String to encrypt</param>  
    ''' <returns>Encrypted string with IV prefix</returns>  
    Public Function Encrypt(plainText As String) As String
        ' Create new random IV  
        AESProvider.GenerateIV()

        ' Initialize encryptor now that the IV is set  
        Dim encryptor As ICryptoTransform = AESProvider.CreateEncryptor()

        ' Convert string to bytes  
        Dim plainBytes As Byte() = UnicodeEncoding.Unicode.GetBytes(plainText)

        ' Encrypt plain bytes  
        Dim secureBytes As Byte() = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length)

        ' Add IV to the beginning of the encrypted bytes  
        secureBytes = AESProvider.IV.Concat(secureBytes).ToArray()

        ' Return encrypted bytes as a string  
        Return Convert.ToBase64String(secureBytes)
    End Function

    ''' <summary>  
    ''' Decrypts a string with AES algorithm  
    ''' </summary>  
    ''' <param name="secureText">Encrypted string with IV prefix</param>  
    ''' <returns>Decrypted string</returns>  
    Public Function Decrypt(secureText As String) As String
        ' Convert encrypted string to bytes  
        Dim secureBytes As Byte() = Convert.FromBase64String(secureText)

        ' Take IV from beginning of secureBytes  
        AESProvider.IV = secureBytes.Take(16).ToArray()

        ' Initialize decryptor now that the IV is set  
        Dim decryptor As ICryptoTransform = AESProvider.CreateDecryptor()

        ' Decrypt bytes after the IV  
        Dim plainBytes As Byte() = decryptor.TransformFinalBlock(secureBytes, 16, secureBytes.Length - 16)

        ' Return decrypted bytes as a string  
        Return UnicodeEncoding.Unicode.GetString(plainBytes)
    End Function

End Class
