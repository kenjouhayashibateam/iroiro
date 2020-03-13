Imports WPF.ViewModels

Namespace Command

    ''' <summary>
    ''' 複数印刷画面遷移コマンド
    ''' </summary>
    Public Class GotoMultiAddresseeDataViewCommand
        Implements ICommand

        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
        Public vm As SingleAddresseeDataViewModel

        Sub New(ByVal _vm As SingleAddresseeDataViewModel)
            vm = _vm
        End Sub

        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            vm.ShowMultiAddresseeDataView()
        End Sub

        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return True
        End Function
    End Class
End Namespace
