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
            Dim parent As Window = Application.Current.Windows.OfType(Of Window).SingleOrDefault(Function(w) w.IsActive)

            parent.ShowInTaskbar = False
            form.FormData.Owner = parent
            form.FormData.ShowDialog()
            parent.ShowInTaskbar = True
        End Sub

    End Class
End Namespace

