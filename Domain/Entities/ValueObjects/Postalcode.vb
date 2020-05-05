
''' <summary>
''' 郵便番号
''' </summary>
Public Class PostalCode
    Public Property Code As String

    Sub New(ByVal myPostalCode As String)
        Code = myPostalCode
    End Sub

    Public Function GetCode() As String
        Return Code
    End Function
End Class
