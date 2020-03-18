Imports System.ComponentModel
Imports WPF.Command
Imports WPF.Data

Namespace ViewModels

    Public MustInherit Class BaseViewModel
        Implements INotifyPropertyChanged, INotifyDataErrorInfo

        Private _CallShowForm As Boolean
        Private _ShowForm As ShowFormData
        Private _InputErrorString As String
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Public Event ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged

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

        Public Property ShowFormCommand As DelegateCommand

        Protected Overridable Overloads Sub CallPropertyChanged()

            Dim caller As StackFrame = New StackFrame(1)
            Dim methodNames As String() = caller.GetMethod.Name.Split("_")
            Dim propertyName As String = methodNames(methodNames.Length - 1)

            CallPropertyChanged(propertyName)

        End Sub

        Protected Overloads Sub CallPropertyChanged(ByVal propertyname As String)
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

        Protected MustOverride Sub ValidateProperty(ByVal propertyName As String, ByVal value As Object)

        Private ReadOnly _currentErrors As Dictionary(Of String, String) = New Dictionary(Of String, String)()

        Protected Sub AddError(ByVal propertyName As String, ByVal _error As String)
            If Not _currentErrors.ContainsKey(propertyName) Then
                _currentErrors(propertyName) = _error
                OnErrorsChanged(propertyName)
            End If
        End Sub

        Protected Sub RemoveError(ByVal propertyName As String)
            If _currentErrors.ContainsKey(propertyName) Then
                _currentErrors.Remove(propertyName)
                OnErrorsChanged(propertyName)
            End If
        End Sub

        Private Sub OnErrorsChanged(ByVal propertyName As String)
            RaiseEvent ErrorsChanged(Me, New DataErrorsChangedEventArgs(propertyName))
        End Sub

        Public Function GetErrors(propertyName As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors

            If String.IsNullOrEmpty(propertyName) Then Return Nothing
            If Not _currentErrors.ContainsKey(propertyName) Then Return Nothing

            Return _currentErrors(propertyName)

        End Function

        Public ReadOnly Property HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
            Get
                Return _currentErrors.Count > 0
            End Get
        End Property

    End Class
End Namespace
