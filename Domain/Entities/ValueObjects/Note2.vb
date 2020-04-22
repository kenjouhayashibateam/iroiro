
''' <summary>
''' 備考2
''' </summary>
Public Class Note2
    Public Property Value As String

    Public Sub New(ByVal _note2 As String)
        Value = _note2
    End Sub

    Public Function GetNote() As String
        Return Value
    End Function
End Class