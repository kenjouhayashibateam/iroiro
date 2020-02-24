Public Class InputCustomerCommand
    Implements ICommand

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
    Public vm As MultiAddresseeDataViewModel

    Sub New(ByVal _vm As MultiAddresseeDataViewModel)
        vm = _vm
    End Sub

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        vm.AddItem()
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
