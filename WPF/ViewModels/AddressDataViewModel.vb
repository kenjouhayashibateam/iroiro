Imports Domain
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Collections.Specialized

Public Interface IAddressDataViewCloseListener
    Sub Notify(ByVal _postalcode As String, ByVal _address As String)
End Interface

''' <summary>
''' 住所一覧画面ビューモデル
''' </summary>
Public Class AddressDataViewModel
    Implements INotifyPropertyChanged, INotifyCollectionChanged

    Private Shared Listener As IAddressDataViewCloseListener
    Private Shared _AddressList As ObservableCollection(Of AddressDataEntity)
    Private _Postalcode As String
    Private _Address As String
    Private _myAddress As AddressDataEntity
    Private _SetAddressDataCommand As ICommand

    Public Property SetAddressDataCommand As ICommand
        Get
            If _SetAddressDataCommand Is Nothing Then _SetAddressDataCommand = New ReturnAddressDataCommand(Me)
            Return _SetAddressDataCommand
        End Get
        Set
            _SetAddressDataCommand = Value
        End Set
    End Property

    Public Property MyAddress As AddressDataEntity
        Get
            Return _myAddress
        End Get
        Set
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(MyAddress)))

            _myAddress = Value
        End Set
    End Property

    Public Sub AddListener(ByVal _listener As IAddressDataViewCloseListener)
        Listener = _listener
    End Sub

    Public Property Postalcode As String
        Get
            Return _Postalcode
        End Get
        Set
            If _Postalcode = Value Then Return
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Postalcode)))
            _Postalcode = Value
        End Set
    End Property

    Public Property Address As String
        Get
            Return _Address
        End Get
        Set
            If _Address = Value Then Return
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Address)))
            _Address = Value
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

    Sub New()
        If MyAddressList Is Nothing Then MyAddressList = New ObservableCollection(Of AddressDataEntity)
    End Sub

    Sub New(ByVal _addresslist As ObservableCollection(Of AddressDataEntity))
        MyAddressList = _addresslist
    End Sub

    ''' <summary>
    ''' 住所データリスト
    ''' </summary>
    ''' <returns></returns>
    Public Property MyAddressList As ObservableCollection(Of AddressDataEntity)
        Get
            Return _AddressList
        End Get
        Set
            _AddressList = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(MyAddressList)))
            RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NameOf(MyAddressList)))
        End Set
    End Property

    Public Sub ReturnData()
        If MyAddress IsNot Nothing Then Listener.Notify(MyAddress.GetPostalCode, MyAddress.GetAddress)
    End Sub

End Class
