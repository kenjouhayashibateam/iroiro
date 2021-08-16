''' <summary>
''' 送付先住所2
''' </summary>
Public Class ReceiverAddress2
    Public Property Address As String

    Public Sub New(_address2 As String)
        Address = _address2
    End Sub

    Public Function ShowDisplay() As String
        Return $"送付先住所2 : {Address}"
    End Function
End Class
