Imports Domain
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Collections.Specialized

''' <summary>
''' 住所一覧画面ビューモデル
''' </summary>
Public Class WinFormAddressDataViewModel
    Implements INotifyPropertyChanged, INotifyCollectionChanged

    Private Shared _MyAddressList As ObservableCollection(Of AddressDataEntity)
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

    Public Property MyAddressList As ObservableCollection(Of AddressDataEntity)
        Get
            Return _MyAddressList
        End Get
        Set
            _MyAddressList = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(MyAddressList)))
            RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NameOf(MyAddressList)))
        End Set
    End Property

    ''' <summary>
    ''' アドレスリストをビューに配列化して返します
    ''' </summary>
    ''' <param name="_addresslist">アドレスリスト</param>
    Public Sub SetList(_addresslist As ObservableCollection(Of AddressDataEntity))
        _MyAddressList = _addresslist
    End Sub

End Class
