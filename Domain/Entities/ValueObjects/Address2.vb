''' <summary>
''' 住所2
''' </summary>
Public Class Address2
    Public Property Address As String

    Public Sub New(_address2 As String)
        Address = _address2
    End Sub

    Public Function ShowDisplay() As String
        Return $"住所2 : {Address}"
    End Function
End Class