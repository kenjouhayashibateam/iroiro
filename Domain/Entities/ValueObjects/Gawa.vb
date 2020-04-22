
''' <summary>
''' 側クラス
''' </summary>
Public Class Gawa
    Inherits GraveNumberField

    Public Sub New(ByVal _value As String)
        CodeField = _value
        If gtc.ConvertNumber_0Delete(_value) = String.Empty Then
            DisplayForField = "0"
        Else
            DisplayForField = gtc.ConvertNumber_0Delete(_value)
        End If
    End Sub

End Class
