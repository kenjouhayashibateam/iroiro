
''' <summary>
''' 住所2
''' </summary>
Public Class Address2
    Public Property Address As String

    Public Sub New(ByVal _address2 As String)
        Address = _address2
    End Sub

    Public Function GetAddress() As String
        Return Address
    End Function
End Class