Namespace Data

    ''' <summary>
    ''' メッセージボックスの値を保持するクラス
    ''' </summary>
    Public Class MessageBoxInfo

        Public Property Message As String = ""
        Public Property Title As String = ""
        Public Property Button As MessageBoxButton = MessageBoxButton.OK
        Public Property Image As MessageBoxImage = MessageBoxImage.None
        Public Property DefaultResult As MessageBoxResult = MessageBoxResult.None
        Public Property Options As MessageBoxOptions = MessageBoxOptions.None
        Public Property Result As MessageBoxResult = MessageBoxResult.None

    End Class
End Namespace


