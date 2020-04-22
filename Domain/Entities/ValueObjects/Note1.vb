
''' <summary>
''' 備考1
''' </summary>
Public Class Note1
    Public Property Value As String

    Public Sub New(ByVal _note1 As String)
        Value = _note1
    End Sub

    Public Function GetNote() As String
        Return Value
    End Function
End Class