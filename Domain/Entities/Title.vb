''' <summary>
''' 敬称
''' </summary>
Public Class Title
    Public Property TitleString As String

    Sub New(ByVal _title As String)
        TitleString = _title
    End Sub

    Public Function GetTitle() As String
        Return TitleString
    End Function
End Class