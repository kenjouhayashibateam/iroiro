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
        Private _Address2 As String
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
                        ReturnList_CustomerID()
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

        Sub New()
            Me.New(New SQLConnectInfrastructure, New ExcelOutputInfrastructure)
        End Sub

        ''' <summary>
        ''' 各種リポジトリを設定します
        ''' </summary>
        ''' <param name="lesseerepository">名義人データ</param>
        ''' <param name="excelrepository">エクセルデータ</param>
        Sub New(ByVal lesseerepository As IDataConectRepogitory, ByVal excelrepository As IOutputDataRepogitory)
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
                    ReferenceLessee()
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
        Public Sub ReferenceLessee()

            Dim lse As LesseeCustomerInfoEntity

            lse = DataBaseConecter.GetCustomerInfo(CustomerID)

            If lse Is Nothing Then Exit Sub

            If lse.GetReceiverName.GetName = String.Empty Then
                SetLesseeProperty(lse)
                Exit Sub
            End If

            If lse.GetLesseeName.GetName = lse.GetReceiverName.GetName Then
                SetReceiverProperty(lse)
                Exit Sub
            Else
                CreateSelectAddresseeInfo(lse)
                CallSelectAddresseeInfo = True
            End If

            If MsgResult = MessageBoxResult.Yes Then
                SetLesseeProperty(lse)
            Else
                SetReceiverProperty(lse)
            End If

        End Sub

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
        Public Sub CreateSelectAddresseeInfo(ByVal lse As LesseeCustomerInfoEntity)

            SelectAddresseeInfo = New DelegateCommand(
            Sub() 'テンプレート構文調べる
                MessageInfo = New MessageBoxInfo With
                {
               .Message = $"{lse.GetLesseeName.ShowDisplay}{vbNewLine}{lse.GetAddress1.GetAddress}{lse.GetAddress2.ShowDisplay}{vbNewLine}{vbNewLine}{lse.GetReceiverName.ShowDisplay}{vbNewLine}{lse.GetReceiverAddress1.ShowDisplay}{lse.GetReceiverAddress2.ShowDisplay}{vbNewLine}{vbNewLine}{My.Resources.DataSelectInfo}{vbNewLine}{vbNewLine}{My.Resources.LesseeDataSelect}",
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
        Private Sub SetReceiverProperty(ByVal mylessee As LesseeCustomerInfoEntity)
            With mylessee
                Addressee = .GetReceiverName.GetName
                Postalcode = .GetReceiverPostalcode.GetCode
                Address1 = .GetReceiverAddress1.GetAddress
                Address2 = .GetReceiverAddress2.GetAddress
            End With
        End Sub

        ''' <summary>
        ''' 名義人情報をプロパティにセットします
        ''' </summary>
        ''' <param name="mylessee">名義人データ</param>
        Private Sub SetLesseeProperty(ByVal mylessee As LesseeCustomerInfoEntity)
            With mylessee
                Addressee = .GetLesseeName.GetName
                Postalcode = .GetPostalCode.GetCode
                Address1 = .GetAddress1.GetAddress
                Address2 = .GetAddress2.GetAddress
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

            Dim addresseearray() As String = Split(Clipboard.GetText, vbCrLf)   '改行区切りで配列を作る
            Dim subarray() As String    'addresseearrayの要素から名文字列配列を生成する
            Dim addressee As DestinationDataEntity
            Dim mylist As New ObservableCollection(Of DestinationDataEntity)

            For i As Integer = 0 To UBound(addresseearray) - 1 'vbCrLfで区切っている影響で、最終行が空文字になるので-1を付ける
                subarray = Split(addresseearray(i), vbTab)  '改行区切りの要素からタブ区切りの配列を生成する
                If subarray.Length <> 4 Then
                    CreateErrorMessageInfo()
                    CallErrorMessageInfo = True
                    Continue For
                End If
                addressee = New DestinationDataEntity(i + 1, subarray(0), Title, subarray(1), subarray(2), subarray(3))
                mylist.Add(addressee)
            Next
            AddresseeList = mylist
            IsOutputEnabled = True
        End Sub

        ''' <summary>
        ''' エラーメッセージを生成します
        ''' </summary>
        Public Sub CreateErrorMessageInfo()

            ErrorMessageInfo = New DelegateCommand(
            Sub()
                MessageInfo = New MessageBoxInfo With
                {
                .Message = $"{My.Resources.ClipBoardDataErrorInfo}{vbNewLine}{My.Resources.PassAddresseeRecordInfo}",
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
        Public Function ReturnList_CustomerID() As ObservableCollection(Of String)

            Dim customeridarray() As String = Split(Clipboard.GetText, vbCrLf)
            Dim mylist As New ObservableCollection(Of String)
            Dim StringVerification As New Regex("^[0-9]{6}")
            Dim test As String = Clipboard.GetText

            For i As Integer = 0 To UBound(customeridarray) - 1
                If Not StringVerification.IsMatch(customeridarray(i)) Then Continue For
                mylist.Add(customeridarray(i))
            Next
            If mylist.Count > 0 Then IsOutputEnabled = True
            Return mylist

        End Function

        ''' <summary>
        ''' 長3封筒印刷
        ''' </summary>
        Public Sub OutputList_Cho3Envelope()
            DataOutputConecter.Cho3EnvelopeOutput(AddresseeList)
        End Sub

        ''' <summary>
        ''' 墓地パンフ印刷
        ''' </summary>
        Public Sub OutputList_GravePamphletEnvelope()

            DataOutputConecter.GravePamphletOutput(AddresseeList)

        End Sub

        ''' <summary>
        ''' 角2封筒印刷
        ''' </summary>
        Public Sub OutputList_Kaku2Envelope()

            DataOutputConecter.Kaku2EnvelopeOutput(AddresseeList)

        End Sub

        ''' <summary>
        ''' ハガキ印刷
        ''' </summary>
        Public Sub OutputList_Postcard()

            DataOutputConecter.PostcardOutput(AddresseeList)

        End Sub

        ''' <summary>
        ''' 洋封筒印刷
        ''' </summary>
        Public Sub OutputList_WesternEnvelope()

            DataOutputConecter.WesternEnvelopeOutput(AddresseeList)

        End Sub

        ''' <summary>
        ''' ラベル用紙印刷
        ''' </summary>
        Public Sub OutputList_LabelSheet()

            For Each ali As DestinationDataEntity In AddresseeList
                DataOutputConecter.LabelOutput(ali.MyCustomerID.GetID, ali.AddresseeName.GetName, Title, ali.MyPostalCode.GetCode, ali.MyAddress1.GetAddress, ali.MyAddress2.GetAddress)
            Next

        End Sub

        ''' <summary>
        ''' リストの行を削除します
        ''' </summary>
        Public Sub DeleteItem()

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
                        .Title = "データ修正",
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
        End Sub
    End Class
End Namespace