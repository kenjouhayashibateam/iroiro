
Imports System.IO.Pipes
''' <summary>
''' 送付先住所1
''' </summary>
Public Class ReceiverAddress1
    Private _Address As String

    Public Property Address As String
        Get
            Return _Address
        End Get
        Set
            _Address = Value
        End Set
    End Property

    Sub New(ByVal _address1 As String)
        _Address = _address1
    End Sub

    Public Function ShowDisplay() As String
        Return $"送付先住所1 : {Address}"
    End Function
End Class
