Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports Domain
Imports Infrastructure

Public Class GravePanelDataViewModel
    Implements INotifyPropertyChanged, INotifyCollectionChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged
    Private _IsNewRecordOnly As Boolean = True
    Private _MyGravePanel As GravePanel
    Private _GravePanelList As ObservableCollection(Of GravePanel)
    Private _GotoCreateGravePanelDataView As ICommand

    ''' <summary>
    ''' 新規データ作成画面に遷移します
    ''' </summary>
    ''' <returns></returns>
    Public Property GotoCreateGravePanelDataView As ICommand
        Get
            If _GotoCreateGravePanelDataView Is Nothing Then _GotoCreateGravePanelDataView = New GotoCreateGravePanelDataViewCommand(Me)
            Return _GotoCreateGravePanelDataView
        End Get
        Set
            _GotoCreateGravePanelDataView = Value
        End Set
    End Property

    Public Property MyGravePanel As GravePanel
        Get
            Return _MyGravePanel
        End Get
        Set
            _MyGravePanel = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(MyGravePanel)))
        End Set
    End Property

    Public Property GravePanelList As ObservableCollection(Of GravePanel)
        Get
            Return _GravePanelList
        End Get
        Set
            _GravePanelList = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(GravePanelList)))
            RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NameOf(GravePanelList)))
        End Set
    End Property

    Public Property IsNewRecordOnly As Boolean
        Get
            Return _IsNewRecordOnly
        End Get
        Set
            If _IsNewRecordOnly.Equals(Value) Then Return
            _IsNewRecordOnly = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(_IsNewRecordOnly)))
        End Set
    End Property

    Public Sub ShowCreateGravePanelDataView()
        Dim cgodv As New CreateGravePanelDataView
        cgodv.ShowDialog()
    End Sub

    Public Class GravePanel

        Private _CustomerID As String
        Private _LesseeName As String
        Private _GraveNumber As String
        Private _ContractDetail As String
        Private _RegistrationTime As Date
        Private _IsPrintout As Boolean = True

        Public Property CustomerID As String
            Get
                Return _CustomerID
            End Get
            Set
                _CustomerID = Value
            End Set
        End Property

        Public Property LesseeName As String
            Get
                Return _LesseeName
            End Get
            Set
                _LesseeName = Value
            End Set
        End Property

        Public Property GraveNumber As String
            Get
                Return _GraveNumber
            End Get
            Set
                _GraveNumber = Value
            End Set
        End Property

        Public Property ContractDetail As String
            Get
                Return _ContractDetail
            End Get
            Set
                _ContractDetail = Value
            End Set
        End Property

        Public Property RegistrationTime As Date
            Get
                Return _RegistrationTime
            End Get
            Set
                _RegistrationTime = Value
            End Set
        End Property


        Public Property IsPrintout As Boolean
            Get
                Return _IsPrintout
            End Get
            Set
                _IsPrintout = Value
            End Set
        End Property

        Sub New(ByVal _customerid As String, ByVal _lesseename As String, ByVal _gravenumber As String, ByVal _contractdetail As String, ByVal _registrationtime As Date)

            CustomerID = _customerid
            LesseeName = _lesseename
            GraveNumber = _gravenumber
            ContractDetail = _contractdetail
            RegistrationTime = _registrationtime

        End Sub
    End Class
End Class
