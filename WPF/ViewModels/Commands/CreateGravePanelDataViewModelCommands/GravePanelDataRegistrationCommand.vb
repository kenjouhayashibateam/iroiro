Imports WPF.ViewModels

Namespace Command

    ''' <summary>
    ''' 墓地札登録コマンド
    ''' </summary>
    Public Class GravePanelDataRegistrationCommand
        Implements ICommand

        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
        Public vm As CreateGravePanelDataViewModel

        Sub New(ByVal _vm As CreateGravePanelDataViewModel)
            vm = _vm
        End Sub

        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            vm.DataRegistration()
        End Sub

        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return True
        End Function
    End Class
End Namespace
