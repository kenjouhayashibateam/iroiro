''' <summary>
''' 備考3
''' </summary>
Public Class Note3
    Public Property Value As String

    Public Sub New(ByVal _note3 As String)
        Value = _note3
    End Sub

    Public Function GetNote() As String
        Return Value
    End Function
End Class