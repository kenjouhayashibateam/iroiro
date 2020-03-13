Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports Domain
Imports Infrastructure
Imports System.Text.RegularExpressions
Imports WPF.Command
Imports WPF.Data

Namespace ViewModels
    ''' <summary>
    ''' 複数印刷画面ビューモデル
    ''' </summary>
    Public Class MultiAddresseeDataViewModel
        Implements INotifyPropertyChanged, INotifyCollectionChanged

        Private ReadOnly DataBaseConecter As IDataConectRepogitory
        Private ReadOnly DataOutputConecter As IOutputDataRepogitory
        Private _Title As String
        Private _AddresseeList As New ObservableCollection(Of AddresseeListItem)
        Private _CustomerID As String
        Private _InputLessee As ICommand
        Private _MyAddressee As AddresseeListItem
        Private _DeleteItemCmmand As ICommand
        Private _ReturnList_CustomerIDCommand As Object
        Private _ReturnListCommand As Object
        Private _SelectedOutputContentsValue As OutputContents = OutputContents.Cho3Envelope
        Private _DataOutputCommand As ICommand
        Private _IsShowInTaskBer As Boolean = True
        Private _MessageInfo As MessageBoxInfo
        Private _CallErrorMessageInfo As Boolean
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

        ''' <summary>
        ''' エラーメッセージを呼び出すBool
        ''' </summary>
        ''' <returns></returns>
        Public Property CallErrorMessageInfo As Boolean
            Get
                Return _CallErrorMessageInfo
            End Get
            Set
                _CallErrorMessageInfo = Value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(CallErrorMessageInfo)))
                _CallErrorMessageInfo = False
            End Set
        End Property

        ''' <summary>
        ''' エラーメッセージを格納します
        ''' </summary>
        ''' <returns></returns>
        Public Property ErrorMessageInfo As DelegateCommand

        Public Property MessageInfo As MessageBoxInfo
            Get
                Return _MessageInfo
            End Get
            Set
                _MessageInfo = Value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(MessageInfo)))
            End Set
        End Property

        ''' <summary>
        ''' タスクバーにアプリを載せるかのBool。複数のViewをまとめて1つのアイコンにして、クリックしたら一番前面のViewが表示されるようにしたい
        ''' </summary>
        ''' <returns></returns>
        Public Property IsShowInTaskBer As Boolean
            Get
                Return _IsShowInTaskBer
            End Get
            Set
                _IsShowInTaskBer = Value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(IsShowInTaskBer)))
            End Set
        End Property

        ''' <summary>
        ''' AddresseeListを出力します
        ''' </summary>
        ''' <returns></returns>
        Public Property DataOutputCommand As ICommand
            Get
                If _DataOutputCommand Is Nothing Then _DataOutputCommand = New OutputListDataCommand(Me)
                Return _DataOutputCommand
            End Get
            Set
                _DataOutputCommand = Value
            End Set
        End Property

        ''' <summary>
        ''' 選択している印刷物の種類
        ''' </summary>
        ''' <returns></returns>
        Public Property SelectedOutputContentsValue As OutputContents
            Get
                Return _SelectedOutputContentsValue
            End Get
            Set
                _SelectedOutputContentsValue = Value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(SelectedOutputContentsValue)))
            End Set
        End Property

        ''' <summary>
        ''' 印刷する種類を保持しているディクショナリ
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property OutputContentsDictionary As Dictionary(Of OutputContents, String) = New Dictionary(Of OutputContents, String)

        ''' <summary>
        ''' 出力する印刷物の種類の列挙型
        ''' </summary>
        Public Enum OutputContents
            Cho3Envelope
            GravePamphletEnvelope
            Kaku2Envelope
            Postcard
            WesternEnvelope
            LabelSheet
        End Enum

        ''' <summary>
        ''' コピーした宛先リストをビューに表示するコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property ReturnListCommand As ICommand
            Get
                If _ReturnListCommand Is Nothing Then _ReturnListCommand = New ReturnListCommand(Me)
                Return _ReturnListCommand
            End Get
            Set
                _ReturnListCommand = Value
            End Set
        End Property

        ''' <summary>
        ''' コピーした管理番号を基にリストを返すコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property ReturnList_CustomerIDCommand As ICommand
            Get
                If _ReturnList_CustomerIDCommand Is Nothing Then _ReturnList_CustomerIDCommand = New ReturnList_CustomerIDCommand(Me)
                Return _ReturnList_CustomerIDCommand
            End Get
            Set
                _ReturnList_CustomerIDCommand = Value
            End Set
        End Property

        ''' <summary>
        ''' リストのアイテムを削除します
        ''' </summary>
        ''' <returns></returns>
        Public Property DeleteItemCommand As ICommand
            Get
                If _DeleteItemCmmand Is Nothing Then _DeleteItemCmmand = New DeleteAddresseeCommand(Me)
                Return _DeleteItemCmmand
            End Get
            Set
                _DeleteItemCmmand = Value
            End Set
        End Property

        ''' <summary>
        ''' リストに表示する宛先クラス
        ''' </summary>
        ''' <returns></returns>
        Public Property MyAddressee As AddresseeListItem
            Get
                Return _MyAddressee
            End Get
            Set
                _MyAddressee = Value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(MyAddressee)))
            End Set
        End Property

        ''' <summary>
        ''' リストに表示する名義人クラス
        ''' </summary>
        ''' <returns></returns>
        Public Property InputLessee As ICommand
            Get
                If _InputLessee Is Nothing Then _InputLessee = New InputCustomerCommand(Me)
                Return _InputLessee
            End Get
            Set
                _InputLessee = Value
            End Set
        End Property

        ''' <summary>
        ''' 管理番号
        ''' </summary>
        ''' <returns></returns>
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
        Public Property AddresseeList As ObservableCollection(Of AddresseeListItem)
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
        End Sub

        ''' <summary>
        ''' 各種リポジトリを設定します
        ''' </summary>
        ''' <param name="lesseerepository">名義人データ</param>
        ''' <param name="excelrepository">エクセルデータ</param>
        Sub New(ByVal lesseerepository As IDataConectRepogitory, ByVal excelrepository As IOutputDataRepogitory)
            DataBaseConecter = lesseerepository
            DataOutputConecter = excelrepository
            Title = "様"

            OutputContentsDictionary.Add(OutputContents.Cho3Envelope, "長3封筒")
            OutputContentsDictionary.Add(OutputContents.GravePamphletEnvelope, "墓地パンフ")
            OutputContentsDictionary.Add(OutputContents.Kaku2Envelope, "角2封筒")
            OutputContentsDictionary.Add(OutputContents.LabelSheet, "ラベル用紙")
            OutputContentsDictionary.Add(OutputContents.Postcard, "ハガキ")
            OutputContentsDictionary.Add(OutputContents.WesternEnvelope, "洋封筒")

            SelectedOutputContentsValue = OutputContents.Cho3Envelope

        End Sub

        ''' <summary>
        ''' リストに追加する名義人データを返します
        ''' </summary>
        Public Sub AddItem()

            Dim lessee As LesseeCustomerInfoEntity
            Dim myaddressee As AddresseeListItem

            lessee = DataBaseConecter.GetCustomerInfo(CustomerID)

            If lessee Is Nothing Then Exit Sub

            With lessee
                myaddressee = New AddresseeListItem(.GetCustomerID, .GetLesseeName, .GetPostalCode, .GetAddress1, .GetAddress2)
            End With

            AddresseeList.Add(myaddressee)

            CustomerID = String.Empty

        End Sub

        ''' <summary>
        ''' クリップボードのデータを基にリスト表示するアイテムを格納したリストを返します
        ''' </summary>
        Public Sub ReturnList()

            Dim addresseearray() As String = Split(Clipboard.GetText, vbCrLf)   '改行区切りで配列を作る
            Dim subarray() As String    'addresseearrayの要素から名文字列配列を生成する
            Dim addressee As AddresseeListItem
            Dim mylist As New ObservableCollection(Of AddresseeListItem)

            For i As Integer = 0 To UBound(addresseearray) - 1 'vbCrLfで区切っている影響で、最終行が空文字になるので-1を付ける
                subarray = Split(addresseearray(i), vbTab)  '改行区切りの要素からタブ区切りの配列を生成する
                If subarray.Length <> 4 Then
                    CreateErrorMessageInfo()
                    CallErrorMessageInfo = True
                    Continue For
                End If
                addressee = New AddresseeListItem(i + 1, subarray(0), subarray(1), subarray(2), subarray(3))
                mylist.Add(addressee)
            Next
            AddresseeList = mylist

        End Sub

        ''' <summary>
        ''' エラーメッセージを生成します
        ''' </summary>
        Public Sub CreateErrorMessageInfo()

            ErrorMessageInfo = New DelegateCommand(
            Sub()
                MessageInfo = New MessageBoxInfo With
                {
                .Message = "コピー形式が正しくありません。" & vbNewLine & "宛名、郵便番号、住所、番地の順で作ったリストをコピーしてください。次のレコードに進みます。",
                .Title = "形式エラー", .Button = MessageBoxButton.OK, .Image = MessageBoxImage.Error
                }
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ErrorMessageInfo)))
            End Sub,
        Function()
            Return True
        End Function
        )
        End Sub

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

            For Each ali As AddresseeListItem In AddresseeList
                DataOutputConecter.Cho3EnvelopeOutput(ali.Addressee.DataString, Title, ali.Postalcode.DataString, ali.Address1.DataString, ali.Address2.DataString, True)
            Next

        End Sub

        ''' <summary>
        ''' 墓地パンフ印刷
        ''' </summary>
        Public Sub OutputList_GravePamphletEnvelope()

            For Each ali As AddresseeListItem In AddresseeList
                DataOutputConecter.GravePamphletOutput(ali.Addressee.DataString, Title, ali.Postalcode.DataString, ali.Address1.DataString, ali.Address2.DataString, True)
            Next

        End Sub

        ''' <summary>
        ''' 角2封筒印刷
        ''' </summary>
        Public Sub OutputList_Kaku2Envelope()

            For Each ali As AddresseeListItem In AddresseeList
                DataOutputConecter.Kaku2EnvelopeOutput(ali.Addressee.DataString, Title, ali.Postalcode.DataString, ali.Address1.DataString, ali.Address2.DataString, True)
            Next

        End Sub

        ''' <summary>
        ''' ハガキ印刷
        ''' </summary>
        Public Sub OutputList_Postcard()

            For Each ali As AddresseeListItem In AddresseeList
                DataOutputConecter.PostcardOutput(ali.Addressee.DataString, Title, ali.Postalcode.DataString, ali.Address1.DataString, ali.Address2.DataString, True)
            Next

        End Sub

        ''' <summary>
        ''' 洋封筒印刷
        ''' </summary>
        Public Sub OutputList_WesternEnvelope()

            For Each ali As AddresseeListItem In AddresseeList
                DataOutputConecter.WesternEnvelopeOutput(ali.Addressee.DataString, Title, ali.Postalcode.DataString, ali.Address1.DataString, ali.Address2.DataString, True)
            Next

        End Sub

        ''' <summary>
        ''' ラベル用紙印刷
        ''' </summary>
        Public Sub OutputList_LabelSheet()

            For Each ali As AddresseeListItem In AddresseeList
                DataOutputConecter.LabelOutput(ali.Addressee.DataString, Title, ali.Postalcode.DataString, ali.Address1.DataString, ali.Address2.DataString)
            Next

        End Sub

        ''' <summary>
        ''' リストの行を削除します
        ''' </summary>
        Public Sub DeleteItem()

            For Each ali As AddresseeListItem In AddresseeList
                If Not MyAddressee.Equals(ali) Then Continue For
                AddresseeList.Remove(ali)
                Exit For
            Next

        End Sub

        ''' <summary>
        ''' 印刷物を出力します
        ''' </summary>
        Public Sub Output()

            If AddresseeList.Count = 0 Then Exit Sub

            Select Case SelectedOutputContentsValue
                Case OutputContents.Cho3Envelope
                    OutputList_Cho3Envelope()
                Case OutputContents.GravePamphletEnvelope
                    OutputList_GravePamphletEnvelope()
                Case OutputContents.Kaku2Envelope
                    OutputList_Kaku2Envelope()
                Case OutputContents.LabelSheet
                    OutputList_LabelSheet()
                Case OutputContents.Postcard
                    OutputList_Postcard()
                Case OutputContents.WesternEnvelope
                    OutputList_WesternEnvelope()
            End Select

        End Sub

        ''' <summary>
        ''' リストに表示する宛先クラス
        ''' </summary>
        Public Class AddresseeListItem

            Public Property CustomerID As DataField
            Public Property Addressee As DataField
            Public Property Postalcode As DataField
            Public Property Address1 As DataField
            Public Property Address2 As DataField

            Sub New(ByVal _customerid As String, ByVal _addressee As String, ByVal _postalcode As String, ByVal _address1 As String, ByVal _address2 As String)

                CustomerID = New DataField(_customerid)
                Addressee = New DataField(_addressee)
                Postalcode = New DataField(_postalcode)
                Address1 = New DataField(_address1)
                Address2 = New DataField(_address2)
            End Sub

            Public Class DataField

                Public Property DataString As String

                Sub New(ByVal _datastring As String)
                    DataString = _datastring
                End Sub

            End Class
        End Class
    End Class
End Namespace