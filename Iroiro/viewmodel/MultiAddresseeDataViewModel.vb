Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports Domain
Imports Infrastructure

Public Class MultiAddresseeDataViewModel
    Implements INotifyPropertyChanged

    Private ReadOnly DataBaseConecter As IDataConectRepogitory
    Private _ListItems As ObservableCollection(Of ListViewItem)
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Property ListItems As ObservableCollection(Of ListViewItem)
        Get
            Return _ListItems
        End Get
        Set
            _ListItems = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ListItems)))
        End Set
    End Property

    Sub New()
        Me.New(New SQLConectInfrastructure, New ExcelOutputInfrastructure)
    End Sub

    ''' <summary>
    ''' 各種リポジトリを設定します
    ''' </summary>
    ''' <param name="lesseerepository">名義人データ</param>
    ''' <param name="excelrepository">エクセルデータ</param>
    Sub New(ByVal lesseerepository As IDataConectRepogitory, ByVal excelrepository As IAdresseeOutputRepogitory)
        DataBaseConecter = lesseerepository
        'DataOutputConecter = excelrepository
    End Sub

    Public Function AddListItem(ByVal customerid As String) As ListViewItem

        Dim lessee As LesseeCustomerInfoEntity

        lessee = DataBaseConecter.GetCustomerInfo(customerid)

        If lessee Is Nothing Then Return Nothing

        Dim listitem As New ListViewItem With {.Text = lessee.GetCustomerID}
        With listitem.SubItems
            .Add(lessee.GetAddressee)
            .Add(lessee.GetPostalCode)
            .Add(lessee.GetAddress1)
            .Add(lessee.GetAddress2)
        End With

        Return listitem

    End Function

    Public Function ReturnList_CustomerID() As List(Of ListViewItem)

        Dim customeridarray() As String = Split(Clipboard.GetText, vbCrLf)
        Dim mylist As New List(Of ListViewItem)

        Dim test As String = Clipboard.GetText

        For i As Integer = 0 To UBound(customeridarray) - 1
            mylist.Add(AddListItem(customeridarray(i)))
        Next

        Return mylist

    End Function

End Class
