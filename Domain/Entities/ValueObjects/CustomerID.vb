''' <summary>
''' 管理番号クラス
''' </summary>
Public Class CustomerID
    Public Property ID As String

    Sub New(ByVal _customerid As String)
        If String.IsNullOrEmpty(_customerid) Then
            ID = "未登録"
        Else
            ID = _customerid
        End If
    End Sub

    Public Function GetID() As String
        Return ID
    End Function

    Public Function ShowDisplay() As String
        Return $"管理番号 : {ID}"
    End Function
End Class
