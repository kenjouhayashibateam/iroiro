Imports WPF.ViewModels

Namespace Command

    ''' <summary>
    ''' 墓地番号検索コマンド
    ''' </summary>
    Public Class ReferenceGraveNumberPanelCommand
        Implements ICommand

        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
        Public vm As New CreateGravePanelDataViewModel

        Sub New(ByVal _vm As CreateGravePanelDataViewModel)
            vm = _vm
        End Sub

        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            vm.ReferenceLesseeData()
        End Sub

        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return True
        End Function
    End Class
End Namespace