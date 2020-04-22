''' <summary>
''' 金額
''' </summary>
Public Class Money
    Public Property Price As String

    Public Sub New(ByVal _money As String)
        Price = _money
    End Sub

    Public Function GetMoney() As String
        Return Price
    End Function
End Class
