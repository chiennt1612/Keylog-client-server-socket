Imports System
Imports System.Text
Imports System.Net
Imports System.Net.Sockets
'Imports System.Security.Cryptography.X509Certificates
Imports System.Threading
Imports System.IO
Namespace KeyLoggerServer
    Public Class Program
        Public ip As IPAddress = IPAddress.Parse("127.0.0.1")
        Public port As Integer = 2000
        Public running As Boolean = True
        Public server As TcpListener
        'Public cert As X509Certificate2 = New X509Certificate2("server.pfx", "instant")
        Sub Listen()  '// Listen to incoming connections.
            While (running)
                Dim tcpCl As TcpClient
                tcpCl = server.AcceptTcpClient() ' // Accept incoming connection.
                Dim C As New Client(Me, tcpCl) '  // Handle in another thread.
            End While
        End Sub
        Public Sub New()
            Console.Title = "InstantMessenger Server"
            Console.WriteLine("----- InstantMessenger Server -----")
            Console.WriteLine("[{0}] Starting server...", DateTime.Now)
            server = New TcpListener(ip, port)
            server.Start()
            Console.WriteLine("[{0}] Server is running properly!", DateTime.Now)
            Listen()
        End Sub
    End Class
End Namespace
