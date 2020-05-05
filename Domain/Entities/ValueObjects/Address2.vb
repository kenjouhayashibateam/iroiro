
''' <summary>
''' 住所2
''' </summary>
Public Class Address2
    Private _Address As String

    Public Property Address As String
        Get
            Return _Address
        End Get
        Set
            _Address = Value
        End Set
    End Property

    Public Sub New(ByVal _address2 As String)
        _Address = _address2
    End Sub

    Public Function ShowDisplay() As String
        Return $"住所2 : {Address}"
    End Function
End Class