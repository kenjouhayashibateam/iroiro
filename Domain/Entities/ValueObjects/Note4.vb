''' <summary>
''' 備考4
''' </summary>
Public Class Note4
    Public Property Value As String

    Public Sub New(_note4 As String)
        Value = _note4
    End Sub

    Public Function GetNote() As String
        Return Value
    End Function
End Class
