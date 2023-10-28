''' <summary>
''' 但し書きクラス
''' </summary>
Public Class Proviso
    Public Property Text As String
    Public Property Amount As String
    Public Property IsReducedTaxRate As Boolean

    Public Sub New(text As String, amount As String, isReducedTaxRate As Boolean)
        Me.Text = text
        Me.Amount = amount
        Me.IsReducedTaxRate = isReducedTaxRate
    End Sub

End Class
