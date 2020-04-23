
Namespace Command

    ''' <summary>
    ''' デリゲートコマンド
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    Public Class DelegateCommand(Of T)
        Implements ICommand

        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

        Private ReadOnly _Execute As Action(Of T)
        Private ReadOnly _CanExecute As Func(Of Boolean)

        ''' <summary>コマンド実装</summary>
        ''' <param name="execute">実行メソッド</param>
        ''' <param name="canExecute">実行メソッド処理許可</param>
        Public Sub New(execute As Action(Of T), canExecute As Func(Of Boolean))

            If Not IsNothing(execute) Then
                _Execute = execute
            Else
                Throw New ArgumentException($"{NameOf(DelegateCommand(Of T))}:{NameOf(execute)}")
            End If

            If Not IsNothing(canExecute) Then
                _CanExecute = canExecute
            Else
                Throw New ArgumentException($"{NameOf(DelegateCommand(Of T))}:{NameOf(canExecute)}")
            End If

        End Sub

        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            _Execute(parameter)
        End Sub

        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return _CanExecute()
        End Function

        Public Function CanExecute() As Boolean
            Return _CanExecute()
        End Function

        Public Sub RaiseCanExecute()
            CommandManager.InvalidateRequerySuggested()
            Return
        End Sub

    End Class
    ''' <summary>コマンド実装</summary>
    Public Class DelegateCommand
        Implements ICommand

        ''' <summary>CanExecute変更イベント</summary>
        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

        ''' <summary>実行メソッド</summary>
        Private ReadOnly _Execute As Action

        ''' <summary>実行メソッド処理許可</summary>
        Private ReadOnly _CanExecute As Func(Of Boolean)

        ''' <summary>コマンド実装</summary>
        ''' <param name="execute">実行メソッド</param>
        ''' <param name="canExecute">実行メソッド処理許可</param>
        Public Sub New(execute As Action, canExecute As Func(Of Boolean))

            If Not IsNothing(execute) Then
                _Execute = execute
            Else
                Throw New ArgumentException($"{NameOf(DelegateCommand)}:{NameOf(execute)}")
            End If

            If Not IsNothing(canExecute) Then
                _CanExecute = canExecute
            Else
                Throw New ArgumentException($"{NameOf(DelegateCommand)}:{NameOf(canExecute)}")
            End If

        End Sub

        ''' <summary>コマンドが実行可能か決定する</summary>
        Public Function CanExecute() As Boolean
            Return _CanExecute()
        End Function

        ''' <summary>コマンドが実行可能か決定する</summary>
        ''' <param name="parameter">パラメータ</param>
        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return _CanExecute()
        End Function

        ''' <summary>コマンド実行</summary>
        Public Sub Execute()
            _Execute()
        End Sub

        ''' <summary>コマンド実行</summary>
        ''' <param name="parameter">パラメータ</param>
        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            _Execute()
        End Sub

    End Class
End Namespace
