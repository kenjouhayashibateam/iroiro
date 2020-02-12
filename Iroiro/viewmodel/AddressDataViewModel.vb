Imports Domain
Imports System.ComponentModel
Imports System.Collections.ObjectModel

''' <summary>
''' 住所一覧画面ビューモデル
''' </summary>
Public Class AddressDataViewModel
    Implements INotifyPropertyChanged

    Private _AddressList As ObservableCollection(Of String(,))
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Property AddressList As ObservableCollection(Of String(,))
        Get
            Return _AddressList
        End Get
        Set
            _AddressList = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(AddressList)))
        End Set
    End Property

    ''' <summary>
    ''' アドレスリストをビューに配列化して返します
    ''' </summary>
    ''' <param name="_addresslist">アドレスリスト</param>
    Public Sub SetList(_addresslist As List(Of AddressDataEntity))

        Dim mylist(_addresslist.Count, 2) As String
        Dim i As Integer = 0

        AddressList = New ObservableCollection(Of String(,))

        For Each ad As AddressDataEntity In _addresslist
            mylist(i, 0) = ad.GetPostalCode
            mylist(i, 1) = ad.GetAddress
            i += 1
            AddressList.Add(mylist)
        Next

        AddressDataView.SetItem(mylist)

    End Sub

End Class
