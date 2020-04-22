
''' <summary>
''' 登録日時クラス
''' </summary>
Public Class RegistrationTime
    Public Property MyDate As Date

    Sub New(ByVal _registrationtime As Date)
        MyDate = _registrationtime.ToShortDateString
    End Sub
End Class
