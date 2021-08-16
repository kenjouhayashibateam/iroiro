''' <summary>
''' 番クラス
''' </summary>
Public Class Ban
    Inherits GraveNumberField

    Public Sub New(ByVal _value As String)
        CodeField = _value
        DisplayForField = gtc.ConvertNumber_0Delete(_value)
    End Sub

End Class
