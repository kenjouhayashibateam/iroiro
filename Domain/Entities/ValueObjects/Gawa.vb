
''' <summary>
''' 側クラス
''' </summary>
Public Class Gawa
    Inherits GraveNumberField

    Public Sub New(_value As String)
        CodeField = _value
        DisplayForField =
            If(gtc.ConvertNumber_0Delete(_value) = String.Empty, "0", gtc.ConvertNumber_0Delete(_value))
    End Sub

End Class
