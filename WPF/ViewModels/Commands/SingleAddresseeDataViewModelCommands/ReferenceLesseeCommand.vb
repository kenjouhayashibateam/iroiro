Imports WPF.ViewModels

Namespace Command

    ''' <summary>
    ''' 名義人データ検索コマンド
    ''' </summary>
    Public Class ReferenceLesseeCommand
        Implements ICommand
        Private ReadOnly vm As SingleAddresseeDataViewModel
        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

        Public Sub New(_vm As SingleAddresseeDataViewModel)
            vm = _vm
        End Sub

        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            vm.ReferenceLessee()
        End Sub

        ''' <summary>
        ''' エグゼが実行される前に実行する。FalseだとExecuteは実行されない
        ''' </summary>
        ''' <param name="parameter"></param>
        ''' <returns></returns>
        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Dim answer As Boolean = vm.CustomerID.Length = 6

            vm.PermitReference = answer
            Return answer
        End Function

    End Class
End Namespace