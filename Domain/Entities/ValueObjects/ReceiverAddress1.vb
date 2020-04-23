
Imports System.IO.Pipes
''' <summary>
''' 送付先住所1
''' </summary>
Public Class ReceiverAddress1
    Private Property Address As String

    Sub New(ByVal _address1 As String)
        Address = _address1
    End Sub

    Public Function GetAddress() As String
        Return Address
    End Function

    Public Function ShowDisplay() As String
        Return $"送付先住所1 : {Address}"
    End Function
End Class
