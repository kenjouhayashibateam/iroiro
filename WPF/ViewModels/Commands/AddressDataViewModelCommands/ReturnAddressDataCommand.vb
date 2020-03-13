Imports WPF.ViewModels

Namespace Command

    ''' <summary>
    ''' 住所データ取得コマンド
    ''' </summary>
    Public Class ReturnAddressDataCommand
        Implements ICommand

        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
        Public vm As AddressDataViewModel

        Sub New(ByVal _vm As AddressDataViewModel)
            vm = _vm
        End Sub
        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            vm.ReturnData()
        End Sub

        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return True
        End Function
    End Class
End Namespace