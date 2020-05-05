
''' <summary>
''' 住所1
''' </summary>
Public Class Address1
    Private _Address As String

    Public Property Address As String
        Get
            Return _Address
        End Get
        Set
            _Address = Value
        End Set
    End Property

    Public Sub New(ByVal _address1 As String)
        Address = _address1
    End Sub

    'Public Function GetAddress() As String
    '    Return Address
    'End Function

    Public Function ShowDisplay() As String
        Return $"住所1 : {Address}"
    End Function
End Class