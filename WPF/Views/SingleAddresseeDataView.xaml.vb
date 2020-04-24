Class MainWindow
    Private Sub ExitButton_Click(sender As Object, e As RoutedEventArgs) Handles ExitButton.Click
        Close()
    End Sub

    Private Sub Window_PreviewGotKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)
        Dim textBox As TextBox = TryCast(e.NewFocus, TextBox)
        If textBox IsNot Nothing Then textBox.SelectAll()
    End Sub
End Class
