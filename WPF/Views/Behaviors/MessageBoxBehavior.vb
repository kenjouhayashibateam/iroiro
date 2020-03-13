Imports System.Windows.Interactivity
Imports WPF.Data

Namespace Behaviors

    ''' <summary>
    ''' メッセージボックスを呼び出します
    ''' </summary>
    Public Class MessageBoxBehavior
        Inherits TriggerAction(Of FrameworkElement)

        Protected Overrides Sub Invoke(parameter As Object)

            Dim e As DependencyPropertyChangedEventArgs = parameter
            Dim info As MessageBoxInfo = DirectCast(e.NewValue, MessageBoxInfo)

            info.Result = MessageBox.Show(info.Message, info.Title, info.Button, info.Image, info.DefaultResult, info.Options)

        End Sub

    End Class

End Namespace


