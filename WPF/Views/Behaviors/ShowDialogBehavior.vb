Imports System.Windows.Interactivity

Namespace Behaviors
    ''' <summary>
    ''' フォームを呼び出します
    ''' </summary>
    Public Class ShowDialogBehavior
        Inherits TriggerAction(Of FrameworkElement)

        Protected Overrides Sub Invoke(parameter As Object)

            Dim e As DependencyPropertyChangedEventArgs = parameter
            Dim form As ShowFormData = DirectCast(e.NewValue, ShowFormData)

            form.FormData.ShowDialog()

        End Sub

    End Class
End Namespace

