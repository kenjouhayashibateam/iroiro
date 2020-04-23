
''' <summary>
''' 送付先郵便番号
''' </summary>
Public Class ReceiverPostalcode
    Private Property Code As String

    Sub New(ByVal _postalcode As String)
        Code = _postalcode
    End Sub

    Public Function GetCode() As String
        Return Code
    End Function
End Class
