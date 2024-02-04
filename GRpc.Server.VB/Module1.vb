Module Module1
    Public Sub Main(args() As String)
        Dim ii As Integer = 50
        If args.Length > 0 Then
            For I As Integer = 1 To 10
                Dim a As Long : a = a + I
            Next I
        Else
            For i As Integer = 1 To 10
                Dim a As Long : a = a + i
            Next i
        End If
    End Sub
End Module
