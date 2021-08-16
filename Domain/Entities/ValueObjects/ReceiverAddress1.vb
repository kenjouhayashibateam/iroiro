Imports System.IO.Pipes
''' <summary>
''' 送付先住所1
''' </summary>
Public Class ReceiverAddress1
    Public Property Address As String

    Public Sub New(_address1 As String)
        Address = _address1
    End Sub

    Public Function ShowDisplay() As String
        Return $"送付先住所1 : {Address}"
    End Function
End Class
