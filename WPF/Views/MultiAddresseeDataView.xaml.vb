Public Class MultiAddresseeDataView
    Private Sub InputButton_Click(sender As Object, e As RoutedEventArgs) Handles InputButton.Click
        CustomerIDTextBox.Focus()
    End Sub

    Private Sub ExitButton_Click(sender As Object, e As RoutedEventArgs) Handles ExitButton.Click
        Close()
    End Sub
End Class
