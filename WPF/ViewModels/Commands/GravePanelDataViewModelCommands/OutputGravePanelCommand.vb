Imports WPF.ViewModels

Namespace Command
    Public Class OutputGravePanelCommand
        Implements ICommand

        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
        Public vm As GravePanelDataViewModel

        Sub New(ByVal _vm As GravePanelDataViewModel)
            vm = _vm
        End Sub

        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            vm.Output()
        End Sub

        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return True
        End Function
    End Class
End Namespace

