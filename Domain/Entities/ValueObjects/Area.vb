''' <summary>
''' 面積クラス
''' </summary>
Public Class Area
    Public Property AreaValue As Double

    Sub New(ByVal _myarea As Double)
        AreaValue = _myarea
    End Sub

    Public Function GetArea() As String
        Return AreaValue.ToString("n1")
    End Function

    Public Function ShowDisplay() As String
        Return $"面積 : {AreaValue.ToString("n1")}"
    End Function
End Class