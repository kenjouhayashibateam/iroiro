'Imports System.ComponentModel
'Imports System.Collections.ObjectModel
'Imports Domain
'Imports Infrastructure
'Imports System.Text.RegularExpressions

'''' <summary>
'''' 複数印刷画面ビューモデル
'''' </summary>
'Public Class WinFormMultiAddresseeDataViewModel
'    Implements INotifyPropertyChanged

'    Private ReadOnly DataBaseConecter As IDataConectRepogitory
'    Private ReadOnly DataOutputConecter As IOutputDataRepogitory
'    Private _ListItems As ObservableCollection(Of ListViewItem)
'    Private _Title As String
'    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

'    ''' <summary>
'    ''' 敬称
'    ''' </summary>
'    ''' <returns></returns>
'    Public Property Title As String
'        Get
'            Return _Title
'        End Get
'        Set
'            _Title = Value
'            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Title)))
'        End Set
'    End Property

'    ''' <summary>
'    ''' データバインド用リスト
'    ''' </summary>
'    ''' <returns></returns>
'    Public Property ListItems As ObservableCollection(Of ListViewItem)
'        Get
'            Return _ListItems
'        End Get
'        Set
'            _ListItems = Value
'            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ListItems)))
'        End Set
'    End Property

'    Sub New()
'        Me.New(New SQLConectInfrastructure, New ExcelOutputInfrastructure)
'        Title = "様"
'    End Sub

'    ''' <summary>
'    ''' 各種リポジトリを設定します
'    ''' </summary>
'    ''' <param name="lesseerepository">名義人データ</param>
'    ''' <param name="excelrepository">エクセルデータ</param>
'    Sub New(ByVal lesseerepository As IDataConectRepogitory, ByVal excelrepository As IOutputDataRepogitory)
'        DataBaseConecter = lesseerepository
'        DataOutputConecter = excelrepository
'    End Sub

'    ''' <summary>
'    ''' リストに追加する名義人データを返します
'    ''' </summary>
'    ''' <param name="customerid">管理番号</param>
'    ''' <returns></returns>
'    Public Function AddListItem(ByVal customerid As String) As ListViewItem

'        Dim lessee As LesseeCustomerInfoEntity

'        lessee = DataBaseConecter.GetCustomerInfo(customerid)

'        If lessee Is Nothing Then Return Nothing

'        Dim listitem As New ListViewItem With {.Text = lessee.GetCustomerID}
'        With listitem.SubItems
'            .Add(lessee.GetLesseeName)
'            .Add(lessee.GetPostalCode)
'            .Add(lessee.GetAddress1)
'            .Add(lessee.GetAddress2)
'        End With

'        Return listitem

'    End Function

'    ''' <summary>
'    ''' クリップボードのデータを基にリスト表示するアイテムを格納したリストを返します
'    ''' </summary>
'    ''' <returns></returns>
'    Public Function ReturnList() As List(Of ListViewItem)

'        Dim addresseearray() As String = Split(Clipboard.GetText, vbCrLf)   '改行区切りで配列を作る
'        Dim subarray() As String    'addresseearrayの要素からさらにタブ区切りの配列を作る
'        Dim listitem As ListViewItem
'        Dim mylist As New List(Of ListViewItem)

'        For i As Integer = 0 To UBound(addresseearray) - 1
'            subarray = Split(addresseearray(i), vbTab)  '改行区切りの要素からタブ区切りの配列を生成する
'            listitem = New ListViewItem With {.Text = ""}
'            For j As Integer = 0 To UBound(subarray)    'タブ区切りの要素をlistitemにセットしてリストに加える
'                With listitem.SubItems
'                    .Add(subarray(j))
'                End With
'            Next
'            mylist.Add(listitem)
'        Next

'        Return mylist

'    End Function

'    ''' <summary>
'    ''' 管理番号の列を格納したクリップボードを使用してリスト表示するアイテムを格納したリストを返します
'    ''' </summary>
'    ''' <returns></returns>
'    Public Function ReturnList_CustomerID() As List(Of ListViewItem)

'        Dim customeridarray() As String = Split(Clipboard.GetText, vbCrLf)
'        Dim mylist As New List(Of ListViewItem)
'        Dim StringVerification As New Regex("^[0-9]{6}")
'        Dim test As String = Clipboard.GetText

'        For i As Integer = 0 To UBound(customeridarray) - 1
'            If Not StringVerification.IsMatch(customeridarray(i)) Then Continue For
'            mylist.Add(AddListItem(customeridarray(i)))
'        Next

'        Return mylist

'    End Function

'    ''' <summary>
'    ''' 長3封筒印刷
'    ''' </summary>
'    ''' <param name="listitems"></param>
'    Public Sub OutputList_Cho3Envelope(ByVal listitems As ListView.ListViewItemCollection)

'        For Each lvi As ListViewItem In listitems
'            With lvi
'                DataOutputConecter.Cho3EnvelopeOutput(.SubItems(1).Text, Title, .SubItems(2).Text, .SubItems(3).Text, .SubItems(4).Text, True)
'            End With
'        Next

'    End Sub

'    ''' <summary>
'    ''' 墓地パンフ印刷
'    ''' </summary>
'    ''' <param name="listitems"></param>
'    Public Sub OutputList_GravePamphletEnvelope(ByVal listitems As ListView.ListViewItemCollection)

'        For Each lvi As ListViewItem In listitems
'            With lvi
'                DataOutputConecter.GravePamphletOutput(.SubItems(1).Text, Title, .SubItems(2).Text, .SubItems(3).Text, .SubItems(4).Text, True)
'            End With
'        Next

'    End Sub

'    ''' <summary>
'    ''' 角2封筒印刷
'    ''' </summary>
'    ''' <param name="listitems"></param>
'    Public Sub OutputList_Kaku2Envelope(ByVal listitems As ListView.ListViewItemCollection)

'        For Each lvi As ListViewItem In listitems
'            With lvi
'                DataOutputConecter.Kaku2EnvelopeOutput(.SubItems(1).Text, Title, .SubItems(2).Text, .SubItems(3).Text, .SubItems(4).Text, True)
'            End With
'        Next

'    End Sub

'    ''' <summary>
'    ''' ハガキ印刷
'    ''' </summary>
'    ''' <param name="listitems"></param>
'    Public Sub OutputList_Postcard(ByVal listitems As ListView.ListViewItemCollection)

'        For Each lvi As ListViewItem In listitems
'            With lvi
'                DataOutputConecter.PostcardOutput(.SubItems(1).Text, Title, .SubItems(2).Text, .SubItems(3).Text, .SubItems(4).Text, True)
'            End With
'        Next

'    End Sub

'    ''' <summary>
'    ''' 洋封筒印刷
'    ''' </summary>
'    ''' <param name="listitems"></param>
'    Public Sub OutputList_WesternEnvelope(ByVal listitems As ListView.ListViewItemCollection)

'        For Each lvi As ListViewItem In listitems
'            With lvi
'                DataOutputConecter.WesternEnvelopeOutput(.SubItems(1).Text, Title, .SubItems(2).Text, .SubItems(3).Text, .SubItems(4).Text, True)
'            End With
'        Next

'    End Sub

'    ''' <summary>
'    ''' ラベル用紙印刷
'    ''' </summary>
'    ''' <param name="listitems"></param>
'    Public Sub OutputList_LabelSheet(ByVal listitems As ListView.ListViewItemCollection)
'        For Each lvi As ListViewItem In listitems
'            With lvi
'                DataOutputConecter.LabelOutput(.SubItems(1).Text, Title, .SubItems(2).Text, .SubItems(3).Text, .SubItems(4).Text)
'            End With
'        Next
'    End Sub

'End Class
