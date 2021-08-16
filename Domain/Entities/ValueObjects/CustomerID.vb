''' <summary>
''' 管理番号クラス
''' </summary>
Public Class CustomerID
    Public Property ID As String

    Public Sub New(_customerid As String)
        ID = If(String.IsNullOrEmpty(_customerid), "未登録", _customerid)
    End Sub

    Public Function GetID() As String
        Return ID
    End Function

    Public Function ShowDisplay() As String
        Return $"管理番号 : {ID}"
    End Function
End Class
