
''' <summary>
''' 郵便番号
''' </summary>
Public Class PostalCode
    Private ReadOnly _Code As String

    Public ReadOnly Property Code As String
        Get
            Return _Code
        End Get
    End Property

    Sub New(ByVal myPostalCode As String)
        _Code = myPostalCode
    End Sub

    Public Function GetCode() As String
        Return Code
    End Function
End Class
