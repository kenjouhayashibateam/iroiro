''' <summary>
''' 登録日時クラス
''' </summary>
Public Class RegistrationTime
    Public Property MyDate As Date

    Public Sub New(_registrationtime As Date)
        MyDate = _registrationtime.ToShortDateString
    End Sub
End Class
