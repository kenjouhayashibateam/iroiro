''' <summary>
''' 面積クラス
''' </summary>
Public Class Area
    Public Property AreaValue As Double

    Public Sub New(_myarea As Double)
        AreaValue = _myarea
    End Sub

    Public Function ShowDisplay() As String
        Return $"面積 : {AreaValue:0.0#} ㎡"
    End Function
End Class