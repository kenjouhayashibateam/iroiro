
''' <summary>
''' 契約内容クラス
''' </summary>
Public Class ContractContent
    Public Property Content As String

    Public Sub New(ByVal _contractdetail As String)
        Content = _contractdetail
    End Sub

    Public Function GetContent() As String
        Return Content
    End Function
End Class
