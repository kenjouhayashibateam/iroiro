
''' <summary>
''' プリントアウトするかの確認クラス
''' </summary>
Public Class IsPrintout
    Public Property Value As Boolean

    Sub New(ByVal _printouttime As Date)
        ComparisonCheck(_printouttime)
    End Sub

    Public Sub ComparisonCheck(ByVal _printouttime As Date)
        Value = _printouttime = My.Resources.DeraultDate
    End Sub
End Class
