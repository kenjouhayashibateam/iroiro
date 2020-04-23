
''' <summary>
''' 住所1
''' </summary>
Public Class Address1
    Private ReadOnly _Address As String

    Public ReadOnly Property Address As String
        Get
            Return _Address
        End Get
    End Property

    Public Sub New(ByVal _address1 As String)
        _Address = _address1
    End Sub

    Public Function GetAddress() As String
        Return Address
    End Function

    Public Function ShowDisplay() As String
        Return $"住所1 : {Address}"
    End Function
End Class