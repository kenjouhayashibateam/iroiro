
''' <summary>
''' 住所2
''' </summary>
Public Class Address2
    Private ReadOnly _Address As String

    Public ReadOnly Property Address As String
        Get
            Return _Address
        End Get
    End Property

    Public Sub New(ByVal _address2 As String)
        _Address = _address2
    End Sub

    Public Function GetAddress() As String
        Return Address
    End Function

    Public Function ShowDisplay() As String
        Return $"住所2 : {Address}"
    End Function
End Class