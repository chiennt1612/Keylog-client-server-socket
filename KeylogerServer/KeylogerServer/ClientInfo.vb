Imports System
Imports System.Text
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.IO
Namespace KeyLoggerServer
    <Serializable()> Public Class ClientInfo
        Public Clientname As String
        <NonSerialized()> Public IsConnected As Boolean
        <NonSerialized()> Public Connection As Client
        Public Sub New(CName As String, Conn As Client)
            Clientname = CName
            Connection = Conn
            IsConnected = True
        End Sub
        Public Sub New()
            IsConnected = False
        End Sub
    End Class
End Namespace