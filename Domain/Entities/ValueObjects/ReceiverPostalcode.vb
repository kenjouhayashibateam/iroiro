
''' <summary>
''' 送付先郵便番号
''' </summary>
Public Class ReceiverPostalcode
    Private _Code As String

    Public Property Code As String
        Get
            Return _Code
        End Get
        Set
            _Code = Value
        End Set
    End Property

    Sub New(ByVal _postalcode As String)
        _Code = _postalcode
    End Sub

End Class
