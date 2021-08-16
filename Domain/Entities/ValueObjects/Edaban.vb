''' <summary>
''' 枝番クラス
''' </summary>
Public Class Edaban
    Inherits GraveNumberField

    Public Sub New(_value As String)
        CodeField = _value
        DisplayForField = gtc.ConvertNumber_0Delete(_value)
    End Sub
End Class
