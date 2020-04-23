
''' <summary>
''' 送付先住所2
''' </summary>
Public Class ReceiverAddress2
    Private Property Address As String

    Sub New(ByVal _address2 As String)
        Address = _address2
    End Sub

    Public Function GetAddress() As String
        Return Address
    End Function

    Public Function ShowDisplay() As String
        Return $"送付先住所2 : {Address}"
    End Function
End Class
