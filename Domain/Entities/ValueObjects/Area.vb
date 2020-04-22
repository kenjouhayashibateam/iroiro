''' <summary>
''' 面積クラス
''' </summary>
Public Class Area
    Public Property AreaValue As Double

    Sub New(ByVal _myarea As Double)
        AreaValue = _myarea
    End Sub

    Public Function GetArea() As String
        Return AreaValue.ToString("0.0")
    End Function

    Public Function ShowDisplay() As String
        Return $"面積 : {AreaValue}"
    End Function
End Class