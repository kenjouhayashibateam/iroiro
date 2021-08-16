''' <summary>
''' 備考5
''' </summary>
Public Class Note5
    Public Property Value As String

    Public Sub New(_note5 As String)
        Value = _note5
    End Sub

    Public Function GetNote() As String
        Return Value
    End Function
End Class
