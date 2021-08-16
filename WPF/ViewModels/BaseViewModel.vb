Imports System.ComponentModel
Imports WPF.Command

Namespace ViewModels
    ''' <summary>
    ''' ビューモデルの基本クラス
    ''' </summary>
    Public MustInherit Class BaseViewModel
        Implements INotifyPropertyChanged, INotifyDataErrorInfo

        Private _CallShowForm As Boolean
        Private _ShowForm As ShowFormData
        Private _InputErrorString As String
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Public Event ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged

        ''' <summary>
        ''' エラーメッセージ
        ''' </summary>
        ''' <returns></returns>
        Public Property InputErrorString As String
            Get
                Return _InputErrorString
            End Get
            Set
                _InputErrorString = Value
                CallPropertyChanged(NameOf(InputErrorString))
            End Set
        End Property

        Public Property ShowForm As ShowFormData
            Get
                Return _ShowForm
            End Get
            Set
                _ShowForm = Value
                CallPropertyChanged(NameOf(ShowForm))
            End Set
        End Property

        ''' <summary>
        ''' フォームを呼び出すタイミングを管理します
        ''' </summary>
        ''' <returns></returns>
        Public Property CallShowForm As Boolean
            Get
                Return _CallShowForm
            End Get
            Set
                _CallShowForm = Value
                CallPropertyChanged(NameOf(CallShowForm))
                _CallShowForm = False
            End Set
        End Property

        ''' <summary>
        ''' フォームを呼び出すコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property ShowFormCommand As DelegateCommand

        ''' <summary>
        ''' プロパティの値が変わったことを知らせるイベントを発生させます
        ''' </summary>
        Protected Overridable Overloads Sub CallPropertyChanged()
            Dim caller As New StackFrame(1)
            Dim methodNames As String() = caller.GetMethod.Name.Split("_")
            Dim propertyName As String = methodNames(methodNames.Length - 1)

            CallPropertyChanged(propertyName)
        End Sub

        ''' <summary>
        ''' プロパティの値が変わったことを知らせるイベントを発生させます
        ''' </summary>
        Protected Overloads Sub CallPropertyChanged(propertyname As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyname))
        End Sub

        Public Sub CreateShowFormCommand(ByVal myform As Window)
            ShowFormCommand = New DelegateCommand(
                            Sub()
                                ShowForm = New ShowFormData With {.FormData = myform}
                                CallPropertyChanged(NameOf(ShowFormCommand))
                            End Sub,
                           Function()
                               Return True
                           End Function
                            )

            CallShowForm = True
        End Sub

        ''' <summary>
        ''' エラーを管理するメソッド
        ''' </summary>
        ''' <param name="propertyName">プロパティ名</param>
        ''' <param name="value">プロパティの値</param>
        Protected MustOverride Sub ValidateProperty(propertyName As String, value As Object)

        ''' <summary>
        ''' エラーメッセージを保持するディクショナリ
        ''' </summary>
        Private ReadOnly _currentErrors As New Dictionary(Of String, String)()

        ''' <summary>
        ''' エラーメッセージをディクショナリに追加します
        ''' </summary>
        ''' <param name="propertyName">プロパティ名</param>
        ''' <param name="_error">エラーメッセージ</param>
        Protected Sub AddError(propertyName As String, _error As String)
            If Not _currentErrors.ContainsKey(propertyName) Then
                _currentErrors(propertyName) = _error
                OnErrorsChanged(propertyName)
            End If
        End Sub

        ''' <summary>
        ''' エラーを削除します
        ''' </summary>
        ''' <param name="propertyName">プロパティ名</param>
        Protected Sub RemoveError(propertyName As String)
            If _currentErrors.ContainsKey(propertyName) Then
                Dim unused = _currentErrors.Remove(propertyName)
                OnErrorsChanged(propertyName)
            End If
        End Sub

        ''' <summary>
        ''' エラーが発生、または解消されたことを知らせるイベントを発生させます
        ''' </summary>
        ''' <param name="propertyName"></param>
        Private Sub OnErrorsChanged(propertyName As String)
            RaiseEvent ErrorsChanged(Me, New DataErrorsChangedEventArgs(propertyName))
        End Sub

        ''' <summary>
        ''' エラーメッセージを取得します
        ''' </summary>
        ''' <param name="propertyName">プロパティ名</param>
        ''' <returns></returns>
        Public Function GetErrors(propertyName As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors

            If String.IsNullOrEmpty(propertyName) Then Return Nothing
            If Not _currentErrors.ContainsKey(propertyName) Then Return Nothing

            Return _currentErrors(propertyName)

        End Function

        ''' <summary>
        ''' 現在エラーがあるかどうかを判断し、あればTrueを返します
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
            Get
                Return _currentErrors.Count > 0
            End Get
        End Property

    End Class
End Namespace
