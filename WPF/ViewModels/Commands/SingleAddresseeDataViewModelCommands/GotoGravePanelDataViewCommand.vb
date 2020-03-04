Public Class GotoGravePanelDataViewCommand
    Implements ICommand

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
    Public vm As SingleAddresseeDataViewModel

    Sub New(ByVal _vm As SingleAddresseeDataViewModel)
        vm = _vm
    End Sub

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        vm.ShowGravePanelDataView()
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
