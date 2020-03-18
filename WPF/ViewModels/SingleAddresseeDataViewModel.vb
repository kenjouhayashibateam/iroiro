Imports System.ComponentModel
Imports Domain
Imports Infrastructure
Imports WPF.Command
Imports WPF.Data
Imports System.Text.RegularExpressions

Namespace ViewModels
    ''' <summary>
    ''' メインフォームに情報を渡すビューモデルクラス
    ''' </summary>
    Public Class SingleAddresseeDataViewModel
        Inherits BaseViewModel
        Implements IAddressDataViewCloseListener

        Public Property AddressOverLengthInfo As DelegateCommand
        Public Property SelectAddresseeInfo As DelegateCommand
        Public Property ErrorMessageInfo As DelegateCommand
        Public Property MsgResult As MessageBoxResult

        Public Property CallGravePanelDataView As Boolean
            Get
                Return _CallGravePanelDataView
            End Get
            Set
                _CallGravePanelDataView = Value
                CallPropertyChanged(NameOf(CallGravePanelDataView))
                _CallGravePanelDataView = False
            End Set
        End Property

        ''' <summary>
        ''' 住所が長い時に注意を促すメッセージを表示させるBool
        ''' </summary>
        ''' <returns></returns>
        Public Property CallAddressOverLengthMessage As Boolean
            Get
                Return _CallAddressOverLengthMessage
            End Get
            Set
                _CallAddressOverLengthMessage = Value
                CallPropertyChanged(NameOf(CallAddressOverLengthMessage))
                _CallAddressOverLengthMessage = False
            End Set
        End Property

        ''' <summary>
        ''' エラーメッセージを表示させるBool
        ''' </summary>
        ''' <returns></returns>
        Public Property CallErrorMessage As Boolean
            Get
                Return _CallErrorMessage
            End Get
            Set
                _CallErrorMessage = Value
                CallPropertyChanged(NameOf(CallErrorMessage))
                _CallErrorMessage = False
            End Set
        End Property

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
        ''' 名義人と送付先のどちらを使用するかのメッセージを表示させるBool
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
        ''' 印刷する種類を保持します
        ''' </summary>
        ''' <returns></returns>
        Public Property SelectedOutputContentsValue As OutputContents
            Get
                Return _SelectedOutputContentsValue
            End Get
            Set
                If _SelectedOutputContentsValue = Value Then Return
                _SelectedOutputContentsValue = Value
                CallPropertyChanged(NameOf(SelectedOutputContentsValue))
                If OutputContents.TransferPaper.ToString.Equals(_SelectedOutputContentsValue.ToString) Then
                    TransferPaperMenuEnabled = True
                Else
                    TransferPaperMenuEnabled = False
                End If

            End Set
        End Property

        ''' <summary>
        ''' 保持する印刷種類
        ''' </summary>
        Public Enum OutputContents
            TransferPaper
            Cho3Envelope
            Kaku2Envelope
            GravePamphletEnvelope
            LabelSheet
            Postcard
            WesternEnbelope
        End Enum

        ''' <summary>
        ''' 印刷する種類を保持しているディクショナリ
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property OutputContentsDictionary As Dictionary(Of OutputContents, String) = New Dictionary(Of OutputContents, String)

        '画面遷移の際にViewModel に値を渡すため、プロパティで保持する
        Private Property Advm As AddressDataViewModel

        ''' <summary>
        ''' 名義人情報を保持するリポジトリ
        ''' </summary>
        Private ReadOnly DataBaseConecter As IDataConectRepogitory

        ''' <summary>
        ''' 印刷等のデータを保持するリポジトリ
        ''' </summary>
        Private ReadOnly DataOutputConecter As IOutputDataRepogitory

        ''' <summary>
        ''' 墓地札画面に移動するコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property GotoGravePanelDataView As ICommand
            Get
                _GotoGravePanelDataView = New DelegateCommand(
                    Sub()
                        ShowGravePanelDataView()
                        CallPropertyChanged(NameOf(GotoGravePanelDataView))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
                Return _GotoGravePanelDataView
            End Get
            Set
                _GotoGravePanelDataView = Value
            End Set
        End Property

        ''' <summary>
        ''' 一括出力画面に移動するコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property GotoMultiAddresseeDataView As ICommand
            Get
                If _GotoMultiAddresseeDataView Is Nothing Then _GotoMultiAddresseeDataView = GotoMultiAddresseeDataViewCommand()
                Return _GotoMultiAddresseeDataView
            End Get
            Set
                _GotoMultiAddresseeDataView = Value
            End Set
        End Property

        Public Function GotoMultiAddresseeDataViewCommand() As DelegateCommand

            ShowFormCommand = New DelegateCommand(
                Sub()
                    ShowMultiAddresseeDataView()
                    CallPropertyChanged(NameOf(ShowFormCommand))
                End Sub,
                Function()
                    Return True
                End Function
                )

            Return ShowFormCommand

        End Function
        ''' <summary>
        ''' データをOutputするコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property DataOutput As ICommand
            Get
                If _DataOutput Is Nothing Then _DataOutput = DataOutputDelegate()
                Return _DataOutput
            End Get
            Set
                _DataOutput = Value
            End Set
        End Property

        Public Property DelegateCommand As DelegateCommand

        Public Function DataOutputDelegate() As DelegateCommand

            DelegateCommand = New DelegateCommand(
                Sub()
                    Output()
                    CallPropertyChanged(NameOf(DelegateCommand))
                End Sub,
                Function()
                    Dim ec As Boolean = False
                    ec = AddresseeName IsNot String.Empty
                    If ec Then ec = PostalCode IsNot String.Empty
                    If ec Then ec = Address1 IsNot String.Empty
                    If ec Then ec = Address2 IsNot String.Empty
                    If ec Then ec = Not (HasErrors)
                    Return ec
                End Function
                )

            Return DelegateCommand

        End Function

        ''' <summary>
        ''' 備考欄を空欄にするコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property NoteClear As ICommand
            Get
                _NoteClear = New DelegateCommand(
                    Sub()
                        NoteTextClear()
                        CallPropertyChanged(NameOf(NoteClear))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
                Return _NoteClear
            End Get
            Set
                _NoteClear = Value
            End Set
        End Property

        ''' <summary>
        ''' 住所で検索をかけるコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property AddressReference As ICommand
            Get
                _AddressReference = New DelegateCommand(
                    Sub()
                        ReferenceAddress()
                        CallPropertyChanged(NameOf(AddressReference))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
                Return _AddressReference
            End Get
            Set
                _AddressReference = Value
            End Set
        End Property

        ''' <summary>
        ''' 郵便番号で検索をかけるコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property PostalcodeReference As ICommand
            Get
                If _ReferencePostalcode Is Nothing Then _ReferencePostalcode = PostalcodeReferenceCommand()
                Return _ReferencePostalcode
            End Get
            Set
                _ReferencePostalcode = Value
            End Set
        End Property

        Public Function PostalcodeReferenceCommand() As DelegateCommand

            If GetErrors(PostalCode) IsNot Nothing Then Return Nothing
            DelegateCommand = New DelegateCommand(
                Sub()
                    ReferenceAddress_Postalcode()
                    CallPropertyChanged(NameOf(DelegateCommand))
                End Sub,
                Function()
                    Return True
                End Function
                )

            Return DelegateCommand

        End Function

        ''' <summary>
        ''' 名義人データ検索コマンド
        ''' </summary>
        Public Property ReferenceLesseeCommand As ICommand
            Get
                If _ReferenceLesseeCommand Is Nothing Then _ReferenceLesseeCommand = ReferenceLesseeDelegate()
                Return _ReferenceLesseeCommand
            End Get
            Set
                _ReferenceLesseeCommand = Value
            End Set
        End Property

        Public Function ReferenceLesseeDelegate() As DelegateCommand

            DelegateCommand = New DelegateCommand(
                Sub()
                    ReferenceLessee()
               CallPropertyChanged(NameOf(DelegateCommand))
                End Sub,
                Function()
                    Return True
                End Function
                )

            Return DelegateCommand

        End Function

        Private _Addresseename As String = String.Empty
        Private _PostalCode As String = String.Empty
        Private _Address1 As String = String.Empty
        Private _Address2 As String = String.Empty
        Private _Note1 As String = String.Empty
        Private _Note2 As String = String.Empty
        Private _Note3 As String = String.Empty
        Private _Note4 As String = String.Empty
        Private _Note5 As String = String.Empty
        Private _Money As String = String.Empty
        Private _Title As String = String.Empty
        Private _MultiOutputCheck As Boolean
        Private _CustomerID As String = String.Empty
        Private _PermitReference As Boolean
        Private _ReferenceLesseeCommand As ICommand
        Private _ReferencePostalcode As ICommand
        Private _AddressReference As ICommand
        Private _SelectedOutputContentsValue As OutputContents = OutputContents.TransferPaper
        Private _TransferPaperMenuEnabled As Boolean = True
        Private _NoteClear As ICommand
        Private _DataOutput As ICommand
        Private _GotoMultiAddresseeDataView As ICommand
        Private _GotoGravePanelDataView As ICommand
        Private _LastSaveDate As Date
        Private _MessageInfo As MessageBoxInfo
        Private _CallSelectAddresseeInfo As Boolean
        Private _CallErrorMessage As Boolean
        Private _CallAddressOverLengthMessage As Boolean
        Private _CallGravePanelDataView As Boolean
        Private Property MyLessee As LesseeCustomerInfoEntity

        ''' <summary>
        ''' 春秋苑データ最終更新日
        ''' </summary>
        ''' <returns></returns>
        Public Property LastSaveDate As Date
            Get
                Return _LastSaveDate
            End Get
            Set
                _LastSaveDate = Value
                CallPropertyChanged(NameOf(LastSaveDate))
            End Set
        End Property

        ''' <summary>
        ''' 検索許可
        ''' </summary>
        ''' <returns></returns>
        Public Property PermitReference As Boolean
            Get
                Return _PermitReference
            End Get
            Set
                If _PermitReference = Value Then Return
                _PermitReference = Value
                CallPropertyChanged(NameOf(PermitReference))
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
                If _CustomerID = Value Then Return
                _CustomerID = Value
                PermitReference = _CustomerID.Length = 6
                CallPropertyChanged(NameOf(CustomerID))
            End Set
        End Property

        ''' <summary>
        ''' 続けて入力する時に、既存のデータを消さずに次のデータを出力するかのチェック
        ''' </summary>
        ''' <returns></returns>
        Public Property MultiOutputCheck As Boolean
            Get
                Return _MultiOutputCheck
            End Get
            Set
                If _MultiOutputCheck = Value Then Return
                _MultiOutputCheck = Value
                CallPropertyChanged(NameOf(MultiOutputCheck))
            End Set
        End Property

        ''' <summary>
        ''' 宛名
        ''' </summary>
        Public Property AddresseeName As String
            Get
                Return _Addresseename
            End Get
            Set
                If Value = AddresseeName Then Return
                _Addresseename = Value
                CallPropertyChanged(NameOf(AddresseeName))
                ValidateProperty(NameOf(AddresseeName), Value)
            End Set
        End Property

        ''' <summary>
        ''' 敬称
        ''' </summary>
        Public Property Title As String
            Get
                Return _Title
            End Get
            Set
                If Value = Title Then Return
                _Title = Value
                CallPropertyChanged(NameOf(Title))
                ValidateProperty(NameOf(Title), Value)
            End Set
        End Property

        ''' <summary>
        ''' 郵便番号
        ''' </summary>
        Public Property PostalCode As String
            Get
                Return _PostalCode
            End Get
            Set
                If Value = PostalCode Then Return
                _PostalCode = Value
                CallPropertyChanged(NameOf(PostalCode))
                ValidateProperty(NameOf(PostalCode), Value)
            End Set
        End Property

        ''' <summary>
        ''' 住所1（郵便番号が有効な部分）
        ''' </summary>
        Public Property Address1 As String
            Get
                Return _Address1
            End Get
            Set
                If Value = Address1 Then Return
                _Address1 = Value
                CallPropertyChanged(NameOf(Address1))
                ValidateProperty(NameOf(Address1), Value)
            End Set
        End Property

        ''' <summary>
        ''' 住所2（郵便番号が無効な番地等の部分）
        ''' </summary>
        Public Property Address2 As String
            Get
                Return _Address2
            End Get
            Set
                If Value = Address2 Then Return
                _Address2 = Value
                CallPropertyChanged(NameOf(Address2))
                ValidateProperty(NameOf(Address2), Value)
            End Set
        End Property

        ''' <summary>
        ''' 備考1
        ''' </summary>
        Public Property Note1 As String
            Get
                Return _Note1
            End Get
            Set
                If Value = Note1 Then Return
                _Note1 = Value
                CallPropertyChanged(NameOf(Note1))
            End Set
        End Property

        ''' <summary>
        ''' 備考2
        ''' </summary>
        Public Property Note2 As String
            Get
                Return _Note2
            End Get
            Set
                If Value = Note2 Then Return
                _Note2 = Value
                CallPropertyChanged(NameOf(Note2))
            End Set
        End Property

        ''' <summary>
        ''' 備考3
        ''' </summary>
        Public Property Note3 As String
            Get
                Return _Note3
            End Get
            Set
                If Value = Note3 Then Return
                _Note3 = Value
                CallPropertyChanged(NameOf(Note3))
            End Set
        End Property

        ''' <summary>
        ''' 備考4
        ''' </summary>
        Public Property Note4 As String
            Get
                Return _Note4
            End Get
            Set
                If Value = Note4 Then Return
                _Note4 = Value
                CallPropertyChanged(NameOf(Note4))
            End Set
        End Property

        ''' <summary>
        ''' 備考5
        ''' </summary>
        Public Property Note5 As String
            Get
                Return _Note5
            End Get
            Set
                If Value = Note5 Then Return
                _Note5 = Value
                CallPropertyChanged(NameOf(Note5))
            End Set
        End Property

        ''' <summary>
        ''' 金額
        ''' </summary>
        Public Property Money As String
            Get
                Return _Money
            End Get
            Set
                If Value = Money Then Return
                _Money = Value
                CallPropertyChanged(NameOf(Money))
            End Set
        End Property

        ''' <summary>
        ''' 振込用紙独自の欄のEnableを保持します
        ''' </summary>
        ''' <returns></returns>
        Public Property TransferPaperMenuEnabled As Boolean
            Get
                Return _TransferPaperMenuEnabled
            End Get
            Set
                _TransferPaperMenuEnabled = Value
                CallPropertyChanged(NameOf(TransferPaperMenuEnabled))
            End Set
        End Property

        ''' <summary>
        ''' 各種リポジトリを設定します
        ''' </summary>
        Sub New()
            Me.New(New SQLConectInfrastructure, New ExcelOutputInfrastructure)
        End Sub
        ''' <param name="lesseerepository">名義人データ</param>
        ''' <param name="excelrepository">エクセルデータ</param>
        Sub New(ByVal lesseerepository As IDataConectRepogitory, ByVal excelrepository As IOutputDataRepogitory)
            DataBaseConecter = lesseerepository
            DataOutputConecter = excelrepository
            Title = "様" '敬称の大半は「様」なので設定する。Form.Loadイベント等ではデータバインディングされないので、こちらで設定する

            With OutputContentsDictionary
                .Add(OutputContents.TransferPaper, "振込用紙")
                .Add(OutputContents.Cho3Envelope, "長3封筒")
                .Add(OutputContents.GravePamphletEnvelope, "墓地パンフ封筒")
                .Add(OutputContents.Kaku2Envelope, "角２封筒")
                .Add(OutputContents.WesternEnbelope, "洋封筒"）
                .Add(OutputContents.LabelSheet, "ラベル用紙")
                .Add(OutputContents.Postcard, "はがき"）
            End With

            LastSaveDate = DataBaseConecter.GetLastSaveDate.GetDate

        End Sub

        ''' <summary>
        ''' 渡された管理番号で、名義人データを生成します。
        ''' </summary>
        Public Sub ReferenceLessee()

            MyLessee = DataBaseConecter.GetCustomerInfo(CustomerID)

            If MyLessee Is Nothing Then Exit Sub

            If MyLessee.GetReceiverName = String.Empty Then
                SetLesseeProperty(MyLessee)
                NoteInput()
                Exit Sub
            End If

            If MyLessee.GetLesseeName = MyLessee.GetReceiverName Then
                SetReceiverProperty(MyLessee)
                NoteInput()
                Exit Sub
            Else
                CreateSelectAddresseeInfo()
                CallSelectAddresseeInfo = True
            End If

            If MsgResult = MessageBoxResult.Yes Then
                SetLesseeProperty(MyLessee)
            Else
                SetReceiverProperty(MyLessee)
            End If

            NoteInput()

        End Sub

        Private Sub NoteInput()
            Note1 = "管理番号 " & MyLessee.GetCustomerID
            Note2 = MyLessee.GetGraveNumber.GetNumber
            If MyLessee.GetArea > 0 Then
                Note3 = "面積 " & MyLessee.GetArea & " ㎡"
            Else
                Note3 = String.Empty
            End If
        End Sub

        Private Sub SetReceiverProperty(ByVal mylessee As LesseeCustomerInfoEntity)
            AddresseeName = mylessee.GetReceiverName
            PostalCode = mylessee.GetReceiverPostalcode
            Address1 = mylessee.GetReceiverAddress1
            Address2 = mylessee.GetReceiverAddress1
        End Sub

        Private Sub SetLesseeProperty(ByVal mylessee As LesseeCustomerInfoEntity)
            AddresseeName = mylessee.GetLesseeName
            PostalCode = mylessee.GetPostalCode
            Address1 = mylessee.GetAddress1
            Address2 = mylessee.GetAddress2
        End Sub

        ''' <summary>
        ''' 郵便番号を使用して、住所を検索する
        ''' </summary>
        ''' <param name="postalcode">郵便番号</param>
        Public Sub GetAddress(ByVal postalcode As String)
            Dim address As AddressDataEntity = DataBaseConecter.GetAddress(postalcode)
            Address1 = address.GetAddress
        End Sub

        ''' <summary>
        ''' 振込用紙
        ''' </summary>
        Public Sub InputTransferData()
            DataOutputConecter.TransferPaperPrintOutput(CustomerID, AddresseeName, Title, PostalCode, Address1, Address2, Money, Note1, Note2, Note3, Note4, Note5, MultiOutputCheck)
            SetDefaultValue()
        End Sub

        ''' <summary>
        ''' 長3封筒
        ''' </summary>
        Public Sub InputCho3Envelope()
            DataOutputConecter.Cho3EnvelopeOutput(CustomerID, AddresseeName, Title, PostalCode, Address1, Address2, MultiOutputCheck)
        End Sub

        ''' <summary>
        ''' 洋封筒
        ''' </summary>
        Public Sub InputWesternEnvelope()
            DataOutputConecter.WesternEnvelopeOutput(CustomerID, AddresseeName, Title, PostalCode, Address1, Address2, MultiOutputCheck)
        End Sub

        ''' <summary>
        ''' 墓地パンフ
        ''' </summary>
        Public Sub InputGravePamphletEnvelope()
            DataOutputConecter.GravePamphletOutput(CustomerID, AddresseeName, Title, PostalCode, Address1, Address2, MultiOutputCheck)
        End Sub

        ''' <summary>
        ''' 角２封筒
        ''' </summary>
        Public Sub InputKaku2Envelope()
            DataOutputConecter.Kaku2EnvelopeOutput(CustomerID, AddresseeName, Title, PostalCode, Address1, Address2, MultiOutputCheck)
        End Sub

        ''' <summary>
        ''' はがき
        ''' </summary>
        Public Sub InputPostcard()
            DataOutputConecter.PostcardOutput(CustomerID, AddresseeName, Title, PostalCode, Address1, Address2, MultiOutputCheck)
        End Sub

        ''' <summary>
        ''' ラベル
        ''' </summary>
        Public Sub InputLabel()
            DataOutputConecter.LabelOutput(CustomerID, AddresseeName, Title, PostalCode, Address1, Address2)
            SetDefaultValue()
        End Sub

        Private AddressList As AddressDataListEntity
        Private _CallShowAddressDataView As Boolean

        Public Property CallShowAddressDataView As Boolean
            Get
                Return _CallShowAddressDataView
            End Get
            Set
                _CallShowAddressDataView = Value
                CallPropertyChanged(NameOf(CallShowAddressDataView))
                _CallShowAddressDataView = False
            End Set
        End Property

        ''' <summary>
        ''' 住所を検索します
        ''' </summary>
        Public Sub ReferenceAddress()

            Dim myAddress As AddressDataEntity

            AddressList = DataBaseConecter.GetAddressList(Address1)
            If AddressList.GetCount = 0 Then Exit Sub

            '検索結果が1件なら住所一覧画面は呼ばずにプロパティに入力する
            If AddressList.GetCount = 1 Then
                myAddress = AddressList.GetItem(0)
                Address1 = myAddress.MyAddress.Address
                Dim mycode As String = myAddress.MyPostalcode.Code
                PostalCode = mycode.Substring(0, 3) & "-" & mycode.Substring(3, 4)
                Exit Sub
            End If

            Advm = New AddressDataViewModel(AddressList)
            Advm.AddListener(Me)

            CreateShowFormCommand(New AddressDataView)

        End Sub

        ''' <summary>
        ''' 郵便番号で住所を検索します
        ''' </summary>
        Public Sub ReferenceAddress_Postalcode()

            If String.IsNullOrEmpty(PostalCode) Then Exit Sub
            Dim address As AddressDataEntity = DataBaseConecter.GetAddress(PostalCode)
            If address Is Nothing Then Exit Sub
            Address1 = address.GetAddress

        End Sub

        ''' <summary>
        ''' プロパティを初期化する
        ''' </summary>
        Public Sub SetDefaultValue()

            AddresseeName = String.Empty
            PostalCode = String.Empty
            Address1 = String.Empty
            Address2 = String.Empty
            Note1 = String.Empty
            Note2 = String.Empty
            Note3 = String.Empty
            Note4 = String.Empty
            Note5 = String.Empty
            Money = String.Empty

        End Sub

        ''' <summary>
        ''' エラーメッセージを生成します
        ''' </summary>
        Private Sub CreateErrorMessage()
            ErrorMessageInfo = New DelegateCommand(
                   Sub()
                       MessageInfo = New MessageBoxInfo With {
                       .Message = "宛先が不十分です",
                       .Button = MessageBoxButton.OK,
                       .Image = MessageBoxImage.Error,
                       .Title = "必須項目不備"
                       }
                       CallPropertyChanged(NameOf(ErrorMessageInfo))
                   End Sub,
                   Function()
                       Return True
                   End Function
                   )

            CallErrorMessage = True

        End Sub

        ''' <summary>
        ''' 住所検索で、帰ってきたデータを格納します
        ''' </summary>
        ''' <param name="_postalcode"></param>
        ''' <param name="_address"></param>
        Public Sub Notify(_postalcode As String, _address As String) Implements IAddressDataViewCloseListener.Notify
            PostalCode = _postalcode
            Address1 = _address
        End Sub

        ''' <summary>
        ''' 備考を空欄にする
        ''' </summary>
        Public Sub NoteTextClear()
            Note1 = String.Empty
            Note2 = String.Empty
            Note3 = String.Empty
            Note4 = String.Empty
            Note5 = String.Empty
        End Sub

        ''' <summary>
        ''' 宛名データを出力します
        ''' </summary>
        Public Sub Output()

            If HasErrors Then Exit Sub

            Select Case SelectedOutputContentsValue
                Case OutputContents.Cho3Envelope
                    InputCho3Envelope()
                Case OutputContents.GravePamphletEnvelope
                    InputGravePamphletEnvelope()
                Case OutputContents.Kaku2Envelope
                    InputKaku2Envelope()
                Case OutputContents.LabelSheet
                    InputLabel()
                Case OutputContents.Postcard
                    InputPostcard()
                Case OutputContents.TransferPaper
                    InputTransferData()
                    If Address1.Length + Address2.Length > 39 Then
                        CreateAddressOverLengthInfo()
                        CallAddressOverLengthMessage = True
                    End If
                Case OutputContents.WesternEnbelope
                    InputWesternEnvelope()
            End Select

            CallAddressOverLengthMessage = False

        End Sub

        ''' <summary>
        ''' 振込用紙の独自の欄のEnableを変えます
        ''' </summary>
        Public Sub TransferPaperMenuEnabledChange()
            TransferPaperMenuEnabled = SelectedOutputContentsValue = OutputContents.TransferPaper
        End Sub

        ''' <summary>
        ''' 一括出力画面を表示します。
        ''' </summary>
        Public Sub ShowMultiAddresseeDataView()
            CreateShowFormCommand(New MultiAddresseeDataView)
        End Sub

        ''' <summary>
        ''' 墓地札リスト画面を開きます。要コード検証　
        ''' </summary>
        Public Sub ShowGravePanelDataView()
            CreateShowFormCommand(New GravePanelDataView)
        End Sub

        ''' <summary>
        ''' 住所が長い場合の注意を促すメッセージを生成します
        ''' </summary>
        Public Sub CreateAddressOverLengthInfo()

            AddressOverLengthInfo = New DelegateCommand(
            Sub() '無名関数（匿名関数）
                MessageInfo = New MessageBoxInfo With {.Message = "住所がセルからはみ出てますので、書き直して下さい", .Button = MessageBoxButton.OK, .Image = MessageBoxImage.Information, .Title = "要住所調整"}
                CallPropertyChanged(NameOf(AddressOverLengthInfo))
            End Sub,
            Function()
                Return True
            End Function
            )

        End Sub

        ''' <summary>
        ''' 名義人と送付先のどちらを使用するかのメッセージを生成します
        ''' </summary>
        Private Sub CreateSelectAddresseeInfo()

            SelectAddresseeInfo = New DelegateCommand(
            Sub() 'テンプレート構文調べる
                MessageInfo = New MessageBoxInfo With
                {
               .Message = "名義人 " & MyLessee.GetLesseeName & vbNewLine & MyLessee.GetAddress1 & MyLessee.GetAddress2 & vbNewLine & vbNewLine &
                                    "送付先 " & MyLessee.GetReceiverName & vbNewLine & MyLessee.GetReceiverAddress1 & MyLessee.GetReceiverAddress2 &
                                    vbNewLine & vbNewLine & "名義人と送付先が違うデータです。どちらを表示しますか？" & vbNewLine & vbNewLine &
                                    "はい ⇒ 名義人　いいえ ⇒ 送付先", .Button = MessageBoxButton.YesNo, .Image = MessageBoxImage.Question, .Title = "宛先確認"
                                }
                MsgResult = MessageInfo.Result
                CallPropertyChanged(NameOf(SelectAddresseeInfo))
            End Sub,
            Function()
                Return True
            End Function
            )

        End Sub

        Protected Overrides Sub ValidateProperty(propertyName As String, value As Object)
            Select Case propertyName
                Case NameOf(CustomerID)
                    If CustomerID.Length = 6 Then
                        RemoveError(NameOf(CustomerID))
                    Else
                        AddError(NameOf(CustomerID), My.Resources.CustomerIDLengthError)
                    End If
                Case NameOf(AddresseeName)
                    If String.IsNullOrEmpty(AddresseeName) Then
                        AddError(NameOf(AddresseeName), My.Resources.StringEmptyMessage)
                    Else
                        RemoveError(NameOf(AddresseeName))
                    End If
                Case NameOf(PostalCode)
                    If String.IsNullOrEmpty(PostalCode) Then
                        AddError(NameOf(PostalCode), My.Resources.StringEmptyMessage)
                    Else
                        RemoveError(NameOf(PostalCode))
                    End If
                    Dim rx As New Regex("^[0-9]{3}-[0-9]{4}$")
                    If rx.IsMatch(PostalCode) Then
                        RemoveError(NameOf(PostalCode))
                    Else
                        AddError(NameOf(PostalCode), My.Resources.PostalCodeError)
                    End If
                Case NameOf(Address1)
                    If String.IsNullOrEmpty(Address1) Then
                        AddError(NameOf(Address1), My.Resources.StringEmptyMessage)
                    Else
                        RemoveError(NameOf(Address1))
                    End If
                Case NameOf(Address2)
                    If String.IsNullOrEmpty(Address2) Then
                        AddError(NameOf(Address2), My.Resources.StringEmptyMessage)
                    Else
                        RemoveError(NameOf(Address2))
                    End If
            End Select

            InputErrorString = GetErrors(propertyName)
        End Sub

    End Class

End Namespace