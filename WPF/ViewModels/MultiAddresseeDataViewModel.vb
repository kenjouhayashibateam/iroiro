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
        Inherits BaseViewModel
        Implements INotifyPropertyChanged, INotifyCollectionChanged, IProcessedCountObserver, IAddressDataViewCloseListener,
            IOverLengthAddress2Count

        ''' <summary>
        ''' 住所の長いデータの件数
        ''' </summary>
        ''' <returns></returns>
        Private Property OverLengthAddressCount As Integer

        Private ReadOnly DataBaseConecter As IDataConectRepogitory
        Private ReadOnly DataOutputConecter As IOutputDataRepogitory
        Private _Title As String
        Private _AddresseeList As New ObservableCollection(Of DestinationDataEntity)
        Private _CustomerID As String
        Private _InputLessee As ICommand
        Private _MyAddressee As DestinationDataEntity
        Private _DeleteItemCmmand As ICommand
        Private _ReturnList_CustomerIDCommand As Object
        Private _ReturnListCommand As Object
        Private _SelectedOutputContentsValue As OutputContents = OutputContents.Cho3Envelope
        Private _DataOutputCommand As ICommand
        Private _IsShowInTaskBer As Boolean = True
        Private _MessageInfo As MessageBoxInfo
        Private _CallErrorMessageInfo As Boolean
        Private _ReferenceAddressCommand As ICommand
        Private _ProgressVisiblity As Visibility = Visibility.Hidden
        Private _ProgressText As String
        Private _ProgressListCount As Integer
        Private _ProcessedCount As Integer
        Private _IsOutputEnabled As Boolean
        Private _Addressee As String
        Private _Postalcode As String
        Private _Address1 As String
        Private _ReferenceAddressListCommand As ICommand
        Private _Address2 As String = String.Empty
        Private _CallSelectAddresseeInfo As Boolean
        Private _MsgResult As MessageBoxResult
        Private _ReferenceLesseeCommand As DelegateCommand
        Private _CallAddressLengthOverInfo As Boolean
        Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

        ''' <summary>
        ''' 宛名
        ''' </summary>
        ''' <returns></returns>
        Public Property Addressee As String
            Get
                Return _Addressee
            End Get
            Set
                _Addressee = Value
                CallPropertyChanged(NameOf(Addressee))
                ValidateProperty(NameOf(Addressee), Value)
            End Set
        End Property

        ''' <summary>
        ''' 出力ボタンのEnableを管理します
        ''' </summary>
        ''' <returns></returns>
        Public Property IsOutputEnabled As Boolean
            Get
                Return _IsOutputEnabled
            End Get
            Set
                _IsOutputEnabled = Value
                CallPropertyChanged(NameOf(IsOutputEnabled))
            End Set
        End Property

        ''' <summary>
        ''' 処理数
        ''' </summary>
        ''' <returns></returns>
        Public Property ProgressListCount As Integer
            Get
                Return _ProgressListCount
            End Get
            Set
                _ProgressListCount = Value
                CallPropertyChanged(NameOf(ProgressListCount))
            End Set
        End Property

        ''' <summary>
        ''' 進捗カウント
        ''' </summary>
        ''' <returns></returns>
        Public Property ProcessedCount As Integer
            Get
                Return _ProcessedCount
            End Get
            Set
                _ProcessedCount = Value
                CallPropertyChanged(NameOf(ProcessedCount))
            End Set
        End Property

        ''' <summary>
        ''' 進捗メニューに表示するカウント
        ''' </summary>
        ''' <returns></returns>
        Public Property ProgressText As String
            Get
                Return _ProgressText
            End Get
            Set
                _ProgressText = Value
                CallPropertyChanged(NameOf(ProgressText))
            End Set
        End Property

        ''' <summary>
        ''' 進捗メニューを可視化を管理します
        ''' </summary>
        ''' <returns></returns>
        Public Property ProgressVisiblity As Visibility
            Get
                Return _ProgressVisiblity
            End Get
            Set
                _ProgressVisiblity = Value
                CallPropertyChanged(NameOf(ProgressVisiblity))
            End Set
        End Property

        ''' <summary>
        ''' 住所で検索するコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property ReferenceAddressListCommand As ICommand
            Get
                _ReferenceAddressListCommand = New DelegateCommand(
                Sub()
                    ReferenceAddress()
                    CallPropertyChanged(NameOf(ReferenceAddressListCommand))
                End Sub,
                Function()
                    Return True
                End Function
                )
                Return _ReferenceAddressListCommand
            End Get
            Set
                _ReferenceAddressListCommand = Value
            End Set
        End Property

        ''' <summary>
        ''' 郵便番号で検索するコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property ReferenceAddressCommand As ICommand
            Get
                _ReferenceAddressCommand = New DelegateCommand(
                    Sub()
                        ReferenceAddress_Postalcode()
                        CallPropertyChanged(NameOf(ReferenceAddressCommand))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
                Return _ReferenceAddressCommand
            End Get
            Set
                _ReferenceAddressCommand = Value
            End Set
        End Property

        ''' <summary>
        ''' 住所2
        ''' </summary>
        ''' <returns></returns>
        Public Property Address2 As String
            Get
                Return _Address2
            End Get
            Set
                _Address2 = Value
                CallPropertyChanged(NameOf(Address2))
                ValidateProperty(NameOf(Address2), Value)
            End Set
        End Property

        ''' <summary>
        ''' 郵便番号
        ''' </summary>
        ''' <returns></returns>
        Public Property Postalcode As String
            Get
                Return _Postalcode
            End Get
            Set
                _Postalcode = Value
                CallPropertyChanged(NameOf(Postalcode))
                ValidateProperty(NameOf(Postalcode), Value)
            End Set
        End Property

        ''' <summary>
        ''' 住所を検索します
        ''' </summary>
        Public Sub ReferenceAddress()

            Dim myAddress As AddressDataEntity
            Dim AddressList As AddressDataListEntity

            AddressList = DataBaseConecter.GetAddressList(Address1)
            If AddressList.GetCount = 0 Then Exit Sub

            '検索結果が1件なら住所一覧画面は呼ばずにプロパティに入力する
            If AddressList.GetCount = 1 Then
                myAddress = AddressList.GetItem(0)
                Address1 = myAddress.MyAddress.Address
                Dim mycode As String = myAddress.MyPostalcode.Code
                Postalcode = $"{mycode.Substring(0, 3)}-{mycode.Substring(3, 4)}"
                Exit Sub
            End If

            Dim advm As AddressDataViewModel

            advm = New AddressDataViewModel(AddressList)
            advm.AddListener(Me)

            CreateShowFormCommand(New AddressDataView)

        End Sub

        ''' <summary>
        ''' 住所1
        ''' </summary>
        ''' <returns></returns>
        Public Property Address1 As String
            Get
                Return _Address1
            End Get
            Set
                _Address1 = Value
                CallPropertyChanged(NameOf(Address1))
                ValidateProperty(NameOf(Address1), Value)
            End Set
        End Property

        ''' <summary>
        ''' 郵便番号での住所検索
        ''' </summary>
        Private Sub ReferenceAddress_Postalcode()
            If String.IsNullOrEmpty(Postalcode) Then Exit Sub
            Dim ade As AddressDataEntity = DataBaseConecter.GetAddress(Postalcode)
            If ade Is Nothing Then Exit Sub
            Postalcode = ade.MyPostalcode.Code
            Address1 = ade.MyAddress.Address
        End Sub

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
                CallPropertyChanged(NameOf(CallErrorMessageInfo))
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
                CallPropertyChanged(NameOf(MessageInfo))
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
                CallPropertyChanged(NameOf(IsShowInTaskBer))
            End Set
        End Property

        ''' <summary>
        ''' AddresseeListを出力します
        ''' </summary>
        ''' <returns></returns>
        Public Property DataOutputCommand As ICommand
            Get
                _DataOutputCommand = New DelegateCommand(
                    Sub()
                        Output()
                        CallPropertyChanged(NameOf(DataOutputCommand))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )

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
                CallPropertyChanged(NameOf(SelectedOutputContentsValue))
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
                _ReturnListCommand = New DelegateCommand(
                    Sub()
                        ReturnList()
                        CallPropertyChanged(NameOf(ReturnListCommand))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
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
                _ReturnList_CustomerIDCommand = New DelegateCommand(
                    Sub()
                        AddresseeList = ReturnList_CustomerID()
                        CallPropertyChanged(NameOf(ReturnList_CustomerIDCommand))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
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
                _DeleteItemCmmand = New DelegateCommand(
                    Sub()
                        DeleteItem()
                        CallPropertyChanged(NameOf(DeleteItemCommand))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
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
        Public Property MyAddressee As DestinationDataEntity
            Get
                Return _MyAddressee
            End Get
            Set
                _MyAddressee = Value
                CallPropertyChanged(NameOf(MyAddressee))
            End Set
        End Property

        ''' <summary>
        ''' リストに表示する名義人クラス
        ''' </summary>
        ''' <returns></returns>
        Public Property InputLessee As ICommand
            Get
                _InputLessee = New DelegateCommand(
                    Sub()
                        AddItem()
                        CallPropertyChanged(NameOf(InputLessee))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
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
                If Value.Length > 6 Then Value = String.Empty
                _CustomerID = Value
                CallPropertyChanged(NameOf(CustomerID))
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
                For Each dde As DestinationDataEntity In AddresseeList
                    dde.MyTitle.TitleString = Value
                Next
                CallPropertyChanged(NameOf(Title))
            End Set
        End Property

        ''' <summary>
        ''' データバインド用リスト
        ''' </summary>
        ''' <returns></returns>
        Public Property AddresseeList As ObservableCollection(Of DestinationDataEntity)
            Get
                Return _AddresseeList
            End Get
            Set
                _AddresseeList = Value
                CallPropertyChanged(NameOf(AddresseeList))
                RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NameOf(AddresseeList)))
            End Set
        End Property

        Public Sub New()
            Me.New(New SQLConnectInfrastructure, New ExcelOutputInfrastructure)
        End Sub

        ''' <summary>
        ''' 各種リポジトリを設定します
        ''' </summary>
        ''' <param name="lesseerepository">名義人データ</param>
        ''' <param name="excelrepository">エクセルデータ</param>
        Public Sub New(lesseerepository As IDataConectRepogitory, excelrepository As IOutputDataRepogitory)
            DataBaseConecter = lesseerepository
            DataOutputConecter = excelrepository
            Title = My.Resources.HonorificsText

            OutputContentsDictionary.Add(OutputContents.Cho3Envelope, My.Resources.Cho3EnvelopeText)
            OutputContentsDictionary.Add(OutputContents.GravePamphletEnvelope, My.Resources.GravePamphletEnvelopeText)
            OutputContentsDictionary.Add(OutputContents.Kaku2Envelope, My.Resources.Kaku2EnvelopeText)
            OutputContentsDictionary.Add(OutputContents.LabelSheet, My.Resources.LabelPaperText)
            OutputContentsDictionary.Add(OutputContents.Postcard, My.Resources.PostcardText)
            OutputContentsDictionary.Add(OutputContents.WesternEnvelope, My.Resources.WesternEnvelopeText)

            SelectedOutputContentsValue = OutputContents.Cho3Envelope

        End Sub

        ''' <summary>
        ''' 名義人検索コマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property ReferenceLesseeCommand As DelegateCommand
            Get
                _ReferenceLesseeCommand = New DelegateCommand(
                Sub()
                    Dim unused = ReferenceLessee(CustomerID)
                    CallPropertyChanged(NameOf(ReferenceLesseeCommand))
                End Sub,
                Function()
                    Return True
                End Function
                )
                Return _ReferenceLesseeCommand
            End Get
            Set
                _ReferenceLesseeCommand = Value
            End Set
        End Property
        ''' <summary>
        ''' 名義人検索
        ''' </summary>
        Private Function ReferenceLessee(managementNumber As String) As LesseeCustomerInfoEntity

            Dim lce As LesseeCustomerInfoEntity

            lce = DataBaseConecter.GetCustomerInfo(managementNumber)

            If lce Is Nothing Then Return Nothing

            If lce.GetReceiverName.GetName = String.Empty Then
                SetLesseeProperty(lce)
                Return lce
            End If

            If lce.GetLesseeName.GetName = lce.GetReceiverName.GetName Then
                SetReceiverProperty(lce)
                Return lce
            Else
                CreateSelectAddresseeInfo(lce)
                CallSelectAddresseeInfo = True
            End If

            If MsgResult = MessageBoxResult.Yes Then
                SetLesseeProperty(lce)
            Else
                SetReceiverProperty(lce)
            End If
            Return lce
        End Function

        Public Property MsgResult As MessageBoxResult
            Get
                Return _MsgResult
            End Get
            Set
                _MsgResult = Value
                CallPropertyChanged(NameOf(MsgResult))
            End Set
        End Property

        ''' <summary>
        ''' 名義人と送付先のどちらを使用するかのメッセージを表示するタイミングを管理します
        ''' </summary>
        ''' <returns></returns>
        Public Property CallSelectAddresseeInfo As Boolean
            Get
                Return _CallSelectAddresseeInfo
            End Get
            Set
                _CallSelectAddresseeInfo = Value
                CallPropertyChanged(NameOf(CallSelectAddresseeInfo))
                _CallSelectAddresseeInfo = False
            End Set
        End Property

        ''' <summary>
        ''' 名義人と送付先のどちらを使用するかのメッセージを表示するコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property SelectAddresseeInfo As DelegateCommand

        ''' <summary>
        ''' 名義人と送付先のどちらを使用するかのメッセージを表示するコマンドを生成します
        ''' </summary>
        ''' <param name="lse">名義人データ</param>
        Public Sub CreateSelectAddresseeInfo(lse As LesseeCustomerInfoEntity)

            SelectAddresseeInfo = New DelegateCommand(
            Sub() 'テンプレート構文調べる
                MessageInfo = New MessageBoxInfo With
                {
               .Message = $"{lse.GetLesseeName.ShowDisplay}{vbNewLine}{lse.GetAddress1.Address}{lse.GetAddress2.ShowDisplay}{vbNewLine}{vbNewLine}{lse.GetReceiverName.ShowDisplay}{vbNewLine}{lse.GetReceiverAddress1.ShowDisplay}{lse.GetReceiverAddress2.ShowDisplay}{vbNewLine}{vbNewLine}{My.Resources.DataSelectInfo}{vbNewLine}{vbNewLine}{My.Resources.LesseeDataSelect}",
                                 .Button = MessageBoxButton.YesNo, .Image = MessageBoxImage.Question, .Title = My.Resources.DataSelectInfoTitle
                                }
                MsgResult = MessageInfo.Result
                CallPropertyChanged(NameOf(SelectAddresseeInfo))
            End Sub,
            Function()
                Return True
            End Function
            )

        End Sub

        ''' <summary>
        ''' 送付先情報をプロパティにセットします
        ''' </summary>
        ''' <param name="mylessee">名義人データ</param>
        Private Sub SetReceiverProperty(mylessee As LesseeCustomerInfoEntity)
            With mylessee
                Addressee = .GetReceiverName.GetName
                Postalcode = .GetReceiverPostalcode.Code
                Address1 = .GetReceiverAddress1.Address
                Address2 = .GetReceiverAddress2.Address
            End With
        End Sub

        ''' <summary>
        ''' 名義人情報をプロパティにセットします
        ''' </summary>
        ''' <param name="mylessee">名義人データ</param>
        Private Sub SetLesseeProperty(mylessee As LesseeCustomerInfoEntity)
            With mylessee
                Addressee = .GetLesseeName.GetName
                Postalcode = .GetPostalCode.Code
                Address1 = .GetAddress1.Address
                Address2 = .GetAddress2.Address
            End With
        End Sub

        ''' <summary>
        ''' リストに追加します
        ''' </summary>
        Public Sub AddItem()

            If HasErrors Then Exit Sub

            Dim myaddressee As New DestinationDataEntity(CustomerID, Addressee, Title, Postalcode, Address1, Address2)

            AddresseeList.Add(myaddressee)
            IsOutputEnabled = True
            ClearProperty()
        End Sub

        Private Sub ClearProperty()
            CustomerID = String.Empty
            Addressee = String.Empty
            Address1 = String.Empty
            Address2 = String.Empty
            Postalcode = String.Empty
        End Sub

        ''' <summary>
        ''' クリップボードのデータを基にリスト表示するアイテムを格納したリストを返します
        ''' </summary>
        Public Sub ReturnList()

            Dim addresseeArray() As String = Split(Clipboard.GetText, vbCrLf)   '改行区切りで配列を作る
            Dim subArray() As String    'addresseearrayの要素から名文字列配列を生成する
            Dim addressee As DestinationDataEntity
            Dim mylist As New ObservableCollection(Of DestinationDataEntity)
            Dim j As Integer = 0

            For i As Integer = 0 To UBound(addresseeArray) - 1 'vbCrLfで区切っている影響で、最終行が空文字になるので-1を付ける
                subArray = Split(addresseeArray(i), vbTab)   '改行区切りの要素からタブ区切りの配列を生成する
                If subArray.Length <> 4 Then
                    j += 1
                    Continue For
                End If
                addressee = New DestinationDataEntity(i + 1, subArray(0), Title, subArray(1), subArray(2), subArray(3))
                mylist.Add(addressee)
            Next

            If j > 0 Then
                CreateErrorMessageInfo(j)
                CallErrorMessageInfo = True
            End If

            AddresseeList = mylist
            IsOutputEnabled = True
        End Sub

        ''' <summary>
        ''' エラーメッセージを生成します
        ''' </summary>
        Public Sub CreateErrorMessageInfo(count As Integer)

            ErrorMessageInfo = New DelegateCommand(
            Sub()
                MessageInfo = New MessageBoxInfo With
                {
                .Message = $"コピー形式の違うデータが {count} 件ありました。{vbNewLine}これらを排除して出力しましたので、確認してください。",
                .Title = My.Resources.FormatErrorTitle,
                .Button = MessageBoxButton.OK,
                .Image = MessageBoxImage.Error
                }
                CallPropertyChanged(NameOf(ErrorMessageInfo))
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
        Public Function ReturnList_CustomerID() As ObservableCollection(Of DestinationDataEntity)

            Dim customeridArray() As String = Split(Clipboard.GetText, vbCrLf)
            Dim mylist As New ObservableCollection(Of DestinationDataEntity)
            Dim dde As DestinationDataEntity
            Dim lce As LesseeCustomerInfoEntity
            Dim StringVerification As New Regex("^[0-9]{6}")
            Dim test As String = Clipboard.GetText

            For i As Integer = 0 To UBound(customeridArray) - 1
                CustomerID = customeridArray(i)
                If Not StringVerification.IsMatch(CustomerID) Then Continue For
                lce = ReferenceLessee(CustomerID)
                If lce Is Nothing Then Continue For
                dde = New DestinationDataEntity(CustomerID, Addressee, Title, Postalcode, Address1, Address2)
                mylist.Add(dde)
            Next
            If mylist.Count > 0 Then IsOutputEnabled = True
            ClearProperty()
            Return mylist

        End Function

        ''' <summary>
        ''' 長3封筒印刷
        ''' </summary>
        Public Sub OutputList_Cho3Envelope()
            DataOutputConecter.Cho3EnvelopeOutput(AddresseeList, IsIPAmjMintyo)
        End Sub

        ''' <summary>
        ''' 墓地パンフ印刷
        ''' </summary>
        Public Sub OutputList_GravePamphletEnvelope()

            DataOutputConecter.GravePamphletOutput(AddresseeList, IsIPAmjMintyo)

        End Sub

        ''' <summary>
        ''' 角2封筒印刷
        ''' </summary>
        Public Sub OutputList_Kaku2Envelope()

            DataOutputConecter.Kaku2EnvelopeOutput(AddresseeList, IsIPAmjMintyo)

        End Sub

        ''' <summary>
        ''' ハガキ印刷
        ''' </summary>
        Public Sub OutputList_Postcard()

            DataOutputConecter.PostcardOutput(AddresseeList, IsIPAmjMintyo)

        End Sub

        ''' <summary>
        ''' 洋封筒印刷
        ''' </summary>
        Public Sub OutputList_WesternEnvelope()

            DataOutputConecter.WesternEnvelopeOutput(AddresseeList, IsIPAmjMintyo)

        End Sub

        ''' <summary>
        ''' ラベル用紙印刷
        ''' </summary>
        Public Sub OutputList_LabelSheet()

            DataOutputConecter.LabelOutput(AddresseeList, IsIPAmjMintyo)

        End Sub

        ''' <summary>
        ''' リストの行を削除します
        ''' </summary>
        Public Sub DeleteItem()

            If MyAddressee Is Nothing Then Exit Sub

            For Each ali As DestinationDataEntity In AddresseeList
                If Not MyAddressee.Equals(ali) Then Continue For
                AddresseeList.Remove(ali)
                Exit For
            Next

            If AddresseeList.Count = 0 Then IsOutputEnabled = False

        End Sub

        ''' <summary>
        ''' 印刷物を出力します
        ''' </summary>
        Public Async Sub Output()

            If AddresseeList.Count = 0 Then Exit Sub

            DataOutputConecter.AddProcessedCountListener(Me)
            DataOutputConecter.AddOverLengthAddressListener(Me)
            DataOutputConecter.DataClear()
            ProgressListCount = AddresseeList.Count
            Dim copyText As String = String.Empty

            For Each dde As DestinationDataEntity In AddresseeList
                copyText += $"{dde.AddresseeName.MyName}{vbTab}{dde.MyPostalCode.Code}{vbTab}{dde.MyAddress1.Address}{vbTab}{dde.MyAddress2.Address}{vbCrLf}"
            Next
            Clipboard.SetText(copyText)

            Await Task.Run(Sub()
                               IsOutputEnabled = False
                               ProgressVisiblity = Visibility.Visible
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
                                   Case Else
                                       Exit Select
                               End Select
                               ProgressVisiblity = Visibility.Hidden
                               IsOutputEnabled = True
                           End Sub
                           )

            If OverLengthAddressCount > 0 Then CallAddressLengthOverInfo = True

        End Sub

        Public Property CallAddressLengthOverInfo As Boolean
            Get
                Return _CallAddressLengthOverInfo
            End Get
            Set
                _CallAddressLengthOverInfo = Value
                CreateAddressLengthOverInfo()
                CallPropertyChanged(NameOf(CallAddressLengthOverInfo))
                _CallAddressLengthOverInfo = False
            End Set
        End Property

        Public Property AddressLengthOverInfoCommad As DelegateCommand

        Public Sub CreateAddressLengthOverInfo()

            AddressLengthOverInfoCommad = New DelegateCommand(
                Sub()
                    MessageInfo = New MessageBoxInfo With {
                        .Message = $"{My.Resources.AddressLengthOverInfo_Multi1}{OverLengthAddressCount}{My.Resources.AddressLengthOverInfo_Multi2}{vbNewLine}{My.Resources.AddressLengthOverInfo_CellYellow}",
                        .Button = MessageBoxButton.OK,
                        .Title = "データ修正して下さい",
                        .Image = MessageBoxImage.Information
                        }
                End Sub,
                 Function()
                     Return True
                 End Function
                )

        End Sub

        Protected Overrides Sub ValidateProperty(propertyName As String, value As Object)

            If String.IsNullOrEmpty(value) Then
                AddError(propertyName, My.Resources.StringEmptyMessage)
            Else
                RemoveError(propertyName)
            End If

        End Sub

        Public Sub ProcessedCountNotify(_count As Integer) Implements IProcessedCountObserver.ProcessedCountNotify
            ProcessedCount = _count
            ProgressText = $"{ProcessedCount}{My.Resources.SlashClipSpace}{ProgressListCount}"
        End Sub

        Public Sub AddressDataNotify(_postalcode As String, _address As String) Implements IAddressDataViewCloseListener.AddressDataNotify
            Postalcode = _postalcode
            Address1 = _address
        End Sub

        Public Sub OverLengthCountNotify(_count As Integer) Implements IOverLengthAddress2Count.OverLengthCountNotify
            OverLengthAddressCount = _count
            CallAddressLengthOverInfo = True
        End Sub
    End Class
End Namespace