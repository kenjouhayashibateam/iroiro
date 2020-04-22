''' <summary>
''' 苗字クラス
''' </summary>
Public Class FamilyName
    Public Property Name As String

    Sub New(ByVal _name As String)
        Name = _name
    End Sub

    Public Function GetName() As String
        Return Name
    End Function

    Public Function ShowDisplay() As String
        Return $"苗字 : {Name}"
    End Function
End Class
