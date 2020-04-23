
''' <summary>
''' 送付先名
''' </summary>
Public Class ReceiverName
    Private Property Name As String

    Sub New(ByVal _name As String)
        Name = _name
    End Sub

    Public Function GetName() As String
        Return Name
    End Function

    Public Function ShowDisplay() As String
        Return $"送付先名 : {Name}"
    End Function
End Class
