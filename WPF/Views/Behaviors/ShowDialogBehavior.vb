Imports System.Windows.Interactivity

Namespace Behaviors
    Public Class ShowDialogBehavior
        Inherits TriggerAction(Of FrameworkElement)

        Protected Overrides Sub Invoke(parameter As Object)
            Dim e As DependencyPropertyChangedEventArgs = parameter
            Dim form As Window = e.NewValue

            form.ShowDialog()

        End Sub

    End Class
End Namespace

