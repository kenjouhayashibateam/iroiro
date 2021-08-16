''' <summary>
''' 住所1
''' </summary>
Public Class Address1
    Public Property Address As String

    Public Sub New(_address1 As String)
        Address = _address1
    End Sub

    Public Function ShowDisplay() As String
        Return $"住所1 : {Address}"
    End Function
End Class