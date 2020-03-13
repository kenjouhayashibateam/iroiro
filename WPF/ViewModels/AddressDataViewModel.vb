Imports Domain
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports WPF.Command

Namespace ViewModels

    ''' <summary>
    ''' 住所データをリスナーに渡します
    ''' </summary>
    Public Interface IAddressDataViewCloseListener
        Sub Notify(ByVal _postalcode As String, ByVal _address As String)
    End Interface

    ''' <summary>
    ''' 住所一覧画面ビューモデル
    ''' </summary>
    Public Class AddressDataViewModel
        Implements INotifyPropertyChanged, INotifyCollectionChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

        Private Shared Listener As IAddressDataViewCloseListener
        Private Shared _AddressList As AddressesEntity
        Private _Postalcode As String
        Private _Address As String
        Private _myAddress As AddressDataEntity
        Private _SetAddressDataCommand As ICommand

        ''' <summary>
        ''' 住所データをリスナーに渡すコマンド
        ''' </summary>
        ''' <returns></returns>
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
                _myAddress = Value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(MyAddress)))
            End Set
        End Property

        ''' <summary>
        ''' リスナー登録
        ''' </summary>
        ''' <param name="_listener"></param>
        Public Sub AddListener(ByVal _listener As IAddressDataViewCloseListener)
            Listener = _listener
        End Sub

        ''' <summary>
        ''' 郵便番号
        ''' </summary>
        ''' <returns></returns>
        Public Property Postalcode As String
            Get
                Return _Postalcode
            End Get
            Set
                If _Postalcode = Value Then Return
                _Postalcode = Value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Postalcode)))
            End Set
        End Property

        ''' <summary>
        ''' 住所
        ''' </summary>
        ''' <returns></returns>
        Public Property Address As String
            Get
                Return _Address
            End Get
            Set
                If _Address = Value Then Return
                _Address = Value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Address)))
            End Set
        End Property

        Sub New()
            If MyAddressList Is Nothing Then MyAddressList = New AddressesEntity
        End Sub

        Sub New(ByVal _addresslist As AddressesEntity)
            MyAddressList = _addresslist
        End Sub

        ''' <summary>
        ''' 住所データリスト
        ''' </summary>
        ''' <returns></returns>
        Public Property MyAddressList As AddressesEntity
            Get
                Return _AddressList
            End Get
            Set
                _AddressList = Value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(MyAddressList)))
                RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NameOf(MyAddressList)))
            End Set
        End Property

        ''' <summary>
        ''' リスナーに住所データを渡す
        ''' </summary>
        Public Sub ReturnData()
            If MyAddress IsNot Nothing Then Listener.Notify(MyAddress.GetPostalCode, MyAddress.GetAddress)
        End Sub

    End Class

End Namespace

