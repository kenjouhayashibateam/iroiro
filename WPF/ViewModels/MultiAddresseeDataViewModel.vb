Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports Domain
Imports Infrastructure
Imports System.Text.RegularExpressions

''' <summary>
''' 複数印刷画面ビューモデル
''' </summary>
Public Class MultiAddresseeDataViewModel
    Implements INotifyPropertyChanged, INotifyCollectionChanged

    Private ReadOnly DataBaseConecter As IDataConectRepogitory
    Private ReadOnly DataOutputConecter As IAdresseeOutputRepogitory
    Private _Title As String
    Private _AddresseeList As New ObservableCollection(Of LesseeCustomerInfoEntity)
    Private _CustomerID As String
    Private _InputLessee As ICommand
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

    Public Property InputLessee As ICommand
        Get
            If _InputLessee Is Nothing Then _InputLessee = New InputCustomerCommand(Me)
            Return _InputLessee
        End Get
        Set
            _InputLessee = Value
        End Set
    End Property

    Public Property CustomerID As String
        Get
            Return _CustomerID
        End Get
        Set
            _CustomerID = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(CustomerID)))
        End Set
    End Property

    ''' <summary>
    ''' 敬称
    ''' </summary>
    ''' <returns></returns>
    Public Property Title As String
        Get
            Return _Title
        End Get
        Set
            _Title = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Title)))
        End Set
    End Property

    ''' <summary>
    ''' データバインド用リスト
    ''' </summary>
    ''' <returns></returns>
    Public Property AddresseeList As ObservableCollection(Of LesseeCustomerInfoEntity)
        Get
            Return _AddresseeList
        End Get
        Set
            _AddresseeList = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(AddresseeList)))
            RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NameOf(AddresseeList)))
        End Set
    End Property

    Sub New()
        Me.New(New SQLConectInfrastructure, New ExcelOutputInfrastructure)
        Title = "様"
    End Sub

    ''' <summary>
    ''' 各種リポジトリを設定します
    ''' </summary>
    ''' <param name="lesseerepository">名義人データ</param>
    ''' <param name="excelrepository">エクセルデータ</param>
    Sub New(ByVal lesseerepository As IDataConectRepogitory, ByVal excelrepository As IAdresseeOutputRepogitory)
        DataBaseConecter = lesseerepository
        DataOutputConecter = excelrepository
    End Sub

    ''' <summary>
    ''' リストに追加する名義人データを返します
    ''' </summary>
    Public Sub AddItem()

        Dim lessee As LesseeCustomerInfoEntity

        lessee = DataBaseConecter.GetCustomerInfo(CustomerID)

        If lessee Is Nothing Then Exit Sub

        With lessee
            AddresseeList.Add(lessee)
        End With

        CustomerID = String.Empty

    End Sub

    ''' <summary>
    ''' クリップボードのデータを基にリスト表示するアイテムを格納したリストを返します
    ''' </summary>
    ''' <returns></returns>
    Public Function ReturnList() As ObservableCollection(Of String())

        Dim addresseearray() As String = Split(Clipboard.GetText, vbCrLf)   '改行区切りで配列を作る
        Dim subarray() As String    'addresseearrayの要素からさらにタブ区切りの配列を作る
        Dim mylist As New ObservableCollection(Of String())

        For i As Integer = 0 To UBound(addresseearray) - 1
            subarray = Split(addresseearray(i), vbTab)  '改行区切りの要素からタブ区切りの配列を生成する

            mylist.Add(subarray)
        Next

        Return mylist

    End Function

    ''' <summary>
    ''' 管理番号の列を格納したクリップボードを使用してリスト表示するアイテムを格納したリストを返します
    ''' </summary>
    ''' <returns></returns>
    Public Function ReturnList_CustomerID() As ObservableCollection(Of String)

        Dim customeridarray() As String = Split(Clipboard.GetText, vbCrLf)
        Dim mylist As New ObservableCollection(Of String)
        Dim StringVerification As New Regex("^[0-9]{6}")
        Dim test As String = Clipboard.GetText

        For i As Integer = 0 To UBound(customeridarray) - 1
            If Not StringVerification.IsMatch(customeridarray(i)) Then Continue For
            mylist.Add(customeridarray(i))
        Next

        Return mylist

    End Function

    ''' <summary>
    ''' 長3封筒印刷
    ''' </summary>
    Public Sub OutputList_Cho3Envelope()

        For Each lci As LesseeCustomerInfoEntity In AddresseeList
            DataOutputConecter.Cho3EnvelopeOutput(lci.GetAddressee, Title, lci.GetPostalCode, lci.GetAddress1, lci.GetAddress2, True)
        Next

    End Sub

    ''' <summary>
    ''' 墓地パンフ印刷
    ''' </summary>
    Public Sub OutputList_GravePamphletEnvelope()

        For Each lci As LesseeCustomerInfoEntity In AddresseeList
            DataOutputConecter.GravePamphletOutput(lci.GetAddressee, Title, lci.GetPostalCode, lci.GetAddress1, lci.GetAddress2, True)
        Next

    End Sub

    ''' <summary>
    ''' 角2封筒印刷
    ''' </summary>
    Public Sub OutputList_Kaku2Envelope()

        For Each lci As LesseeCustomerInfoEntity In AddresseeList
            DataOutputConecter.Kaku2EnvelopeOutput(lci.GetAddressee, Title, lci.GetPostalCode, lci.GetAddress1, lci.GetAddress2, True)
        Next

    End Sub

    ''' <summary>
    ''' ハガキ印刷
    ''' </summary>
    Public Sub OutputList_Postcard()

        For Each lci As LesseeCustomerInfoEntity In AddresseeList
            DataOutputConecter.PostcardOutput(lci.GetAddressee, Title, lci.GetPostalCode, lci.GetAddress1, lci.GetAddress2, True)
        Next

    End Sub

    ''' <summary>
    ''' 洋封筒印刷
    ''' </summary>
    Public Sub OutputList_WesternEnvelope()

        For Each lci As LesseeCustomerInfoEntity In AddresseeList
            DataOutputConecter.WesternEnvelopeOutput(lci.GetAddressee, Title, lci.GetPostalCode, lci.GetAddress1, lci.GetAddress2, True)
        Next

    End Sub

    ''' <summary>
    ''' ラベル用紙印刷
    ''' </summary>
    Public Sub OutputList_LabelSheet()

        For Each lci As LesseeCustomerInfoEntity In AddresseeList
            DataOutputConecter.LabelOutput(lci.GetAddressee, Title, lci.GetPostalCode, lci.GetAddress1, lci.GetAddress2)
        Next

    End Sub

    Protected Class AddresseeItem


    End Class

End Class
