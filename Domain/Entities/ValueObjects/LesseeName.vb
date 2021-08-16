''' <summary>
''' 宛名
''' </summary>
Public Class LesseeName

    Private Property Name As String

    Public Sub New(name_ As String)
        Name = name_
    End Sub

    Public Function GetName() As String
        Return Name
    End Function

    Public Function ShowDisplay() As String
        Return $"名義人名 : {Name}"
    End Function
End Class
