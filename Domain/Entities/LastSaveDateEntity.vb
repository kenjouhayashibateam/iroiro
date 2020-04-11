''' <summary>
''' 名義人データ最終更新日クラス
''' </summary>
Public Class LastSaveDateEntity

    Private Property MySaveDate As SaveDate

    Sub New(ByVal _savedate As Date)
        MySaveDate = New SaveDate(_savedate)
    End Sub

    Public Function GetDate() As Date
        Return MySaveDate.GetDate
    End Function

    ''' <summary>
    ''' 更新日の日付クラス
    ''' </summary>
    Protected Class SaveDate

        Private Property MyDate As Date

        Sub New(ByVal _savedate As Date)
            MyDate = _savedate
        End Sub

        Function GetDate() As Date
            Return MyDate
        End Function

    End Class
End Class
