Imports Domain
Imports Infrastructure
Imports WPF.Command
Imports WPF.Data
Imports System.Text.RegularExpressions
Imports System.Collections.ObjectModel

Namespace ViewModels
    ''' <summary>
    ''' メインフォームに情報を渡すビューモデルクラス
    ''' </summary>
    Public Class SingleAddresseeDataViewModel
        Inherits BaseViewModel
        Implements IAddressDataViewCloseListener, IOverLengthAddress2Count

        ''' <summary>
        ''' 名義人と送付先のどちらのデータを使用するかを選択させるメッセージコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property SelectAddresseeInfo As DelegateCommand
        ''' <summary>
        ''' エラーメッセージを表示するコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property ErrorMessageInfo As ICommand
        Public Property MsgResult As MessageBoxResult
        ''' <summary>
        ''' 住所の長い場合の注意を促すメッセージ
        ''' </summary>
        ''' <returns></returns>
        Public Property AddressOverLengthInfoCommand As DelegateCommand

        ''' <summary>
        ''' 墓地札管理画面を呼び出すタイミングを管理します
        ''' </summary>
        ''' <returns></returns>
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
                If _CallAddressOverLengthMessage = Value Then Return
                _CallAddressOverLengthMessage = Value
                AddressOverLengthInfo()
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
        ''' 出力中メッセージ
        ''' </summary>
        ''' <returns></returns>
        Public Property OutputInfo As String
            Get
                Return _OutputInfo
            End Get
            Set
                _OutputInfo = Value
                CallPropertyChanged(NameOf(OutputInfo))
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

        Private ReadOnly myLogger As ILoggerRepogitory

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
                _GotoMultiAddresseeDataView = New DelegateCommand(
                    Sub()
                        ShowMultiAddresseeDataView()
                        CallPropertyChanged(NameOf(GotoMultiAddresseeDataView))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
                Return _GotoMultiAddresseeDataView
            End Get
            Set
                _GotoMultiAddresseeDataView = Value
            End Set
        End Property

        ''' <summary>
        ''' データをOutputするコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property DataOutput As ICommand
            Get
                _DataOutput = New DelegateCommand(
                    Sub()
                        Output()
                        CallPropertyChanged(NameOf(DataOutput))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
                Return _DataOutput
            End Get
            Set
                _DataOutput = Value
            End Set
        End Property

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
                _ReferencePostalcode = New DelegateCommand(
                    Sub()
                        If GetErrors(PostalCode) IsNot Nothing Then Return
                        ReferenceAddress_Postalcode()
                        CallPropertyChanged(NameOf(PostalcodeReference))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
                Return _ReferencePostalcode
            End Get
            Set
                _ReferencePostalcode = Value
            End Set
        End Property

        ''' <summary>
        ''' 名義人データ検索コマンド
        ''' </summary>
        Public Property ReferenceLesseeCommand As ICommand
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
        Private Property MyLessee As LesseeCustomerInfoEntity
        Private Property ProvisoList As ObservableCollection(Of Proviso)

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
        Private _ProvisoClearCommand As ICommand
        Private _VoucherOutputCommand As ICommand
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
        Private _CallNoteInfo As Boolean
        Private _IsNoteInfoIgnored As Boolean
        Private _IsReducedTaxRate1 As Boolean
        Private _IsReducedTaxRate2 As Boolean
        Private _IsReducedTaxRate3 As Boolean
        Private _IsReducedTaxRate4 As Boolean
        Private _IsReducedTaxRate5 As Boolean
        Private _IsReducedTaxRate6 As Boolean
        Private _IsReducedTaxRate7 As Boolean
        Private _IsReducedTaxRate8 As Boolean
        Private _Amount1 As String = 0
        Private _Amount2 As String = 0
        Private _Amount3 As String = 0
        Private _Amount4 As String = 0
        Private _Amount5 As String = 0
        Private _Amount6 As String = 0
        Private _Amount7 As String = 0
        Private _Amount8 As String = 0
        Private _Proviso1 As String
        Private _Proviso2 As String
        Private _Proviso3 As String
        Private _Proviso4 As String
        Private _Proviso5 As String
        Private _Proviso6 As String
        Private _Proviso7 As String
        Private _Proviso8 As String
        Private _totalAmount As Integer

        Public Property AccountActivityDate As Date
            Get
                Return _AccountActivityDate
            End Get
            Set
                _AccountActivityDate = Value
                CallPropertyChanged(NameOf(AccountActivityDate))
            End Set
        End Property

        Public Property PrepaidDate As Date
            Get
                Return _PrepaidDate
            End Get
            Set
                _PrepaidDate = Value
                CallPropertyChanged(NameOf(PrepaidDate))
            End Set
        End Property

        Public Property IsPrepaid As Boolean
            Get
                Return _IsPrepaid
            End Get
            Set
                _IsPrepaid = Value
                CallPropertyChanged(NameOf(IsPrepaid))
                PrepaidDate = IIf(Value, Date.Now, "1900-01-01")
            End Set
        End Property

        Public Property CanOutput As Boolean
            Get
                Return _CanOutput
            End Get
            Set
                _CanOutput = Value
                CallPropertyChanged(NameOf(CanOutput))
            End Set
        End Property
        Public Property VoucherOutputCommand As ICommand
            Get
                _VoucherOutputCommand = New DelegateCommand(
                    Sub()
                        VoucherOutput()
                        CallPropertyChanged(NameOf(VoucherOutputCommand))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
                Return _VoucherOutputCommand
            End Get
            Set(value As ICommand)
                _VoucherOutputCommand = value
            End Set
        End Property

        Private Sub VoucherOutput()
            Try
                SetProvisoList()
                Dim iD = DataBaseConecter.VoucherRegistration(AccountActivityDate, AddresseeName, TotalAmount, Note5)
                DataOutputConecter.VoucherOutput(iD, AddresseeName, ProvisoList, IsShunjuen, IsReissue, Note5, IsDisplayTax, PrepaidDate, AccountActivityDate)
            Catch ex As Exception
                Dim log As ILoggerRepogitory = New LogFileInfrastructure
                log.Log(ILoggerRepogitory.LogInfo.ERR, ex.Message)
            End Try

            ProvisoClear()

        End Sub


        Private Sub SetProvisoList()
            ProvisoList = New ObservableCollection(Of Proviso)

            AddProviso(Proviso1, Amount1, IsReducedTaxRate1)
            AddProviso(Proviso2, Amount2, IsReducedTaxRate2)
            AddProviso(Proviso3, Amount3, IsReducedTaxRate3)
            AddProviso(Proviso4, Amount4, IsReducedTaxRate4)
            AddProviso(Proviso5, Amount5, IsReducedTaxRate5)
            AddProviso(Proviso6, Amount6, IsReducedTaxRate6)
            AddProviso(Proviso7, Amount7, IsReducedTaxRate7)
            AddProviso(Proviso8, Amount8, IsReducedTaxRate8)
        End Sub

        Private Sub AddProviso(text As String, amount As Integer, isReducedTaxRate As Boolean)
            If String.IsNullOrEmpty(text) Then Exit Sub

            ProvisoList.Add(New Proviso(text, amount, isReducedTaxRate))
        End Sub

        Public Property ProvisoClearCommand As ICommand
            Get
                _ProvisoClearCommand = New DelegateCommand(
                    Sub()
                        ProvisoClear()
                        CallPropertyChanged(NameOf(ProvisoClearCommand))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
                Return _ProvisoClearCommand
            End Get
            Set
                _ProvisoClearCommand = Value
            End Set
        End Property

        Private Sub ProvisoClear()
            Proviso1 = String.Empty
            Proviso2 = String.Empty
            Proviso3 = String.Empty
            Proviso4 = String.Empty
            Proviso5 = String.Empty
            Proviso6 = String.Empty
            Proviso7 = String.Empty
            Proviso8 = String.Empty
            Amount1 = 0
            Amount2 = 0
            Amount3 = 0
            Amount4 = 0
            Amount5 = 0
            Amount6 = 0
            Amount7 = 0
            Amount8 = 0
            IsReducedTaxRate1 = False
            IsReducedTaxRate2 = False
            IsReducedTaxRate3 = False
            IsReducedTaxRate4 = False
            IsReducedTaxRate5 = False
            IsReducedTaxRate6 = False
            IsReducedTaxRate7 = False
            IsReducedTaxRate8 = False
        End Sub

        Public Property IsReissue As Boolean
            Get
                Return _IsReissue
            End Get
            Set
                _IsReissue = Value
                CallPropertyChanged(NameOf(IsReference))
            End Set
        End Property

        Public Property IsDisplayTax As Boolean
            Get
                Return _IsDisplayTax
            End Get
            Set
                _IsDisplayTax = Value
                CallPropertyChanged(NameOf(IsDisplayTax))
            End Set
        End Property

        Public Property IsShunjuen As Boolean
            Get
                Return _IsShunjuen
            End Get
            Set
                _IsShunjuen = Value
                CallPropertyChanged(NameOf(IsShunjuen))
                IsDisplayTax = Value
            End Set
        End Property

        Public Property Proviso1 As String
            Get
                Return _Proviso1
            End Get
            Set
                _Proviso1 = Value
                CallPropertyChanged(NameOf(Proviso1))
            End Set
        End Property

        Public Property Proviso2 As String
            Get
                Return _Proviso2
            End Get
            Set
                _Proviso2 = Value
                CallPropertyChanged(NameOf(Proviso2))
            End Set
        End Property

        Public Property Proviso3 As String
            Get
                Return _Proviso3
            End Get
            Set
                _Proviso3 = Value
                CallPropertyChanged(NameOf(Proviso3))
            End Set
        End Property

        Public Property Proviso4 As String
            Get
                Return _Proviso4
            End Get
            Set
                _Proviso4 = Value
                CallPropertyChanged(NameOf(Proviso4))
            End Set
        End Property

        Public Property Proviso5 As String
            Get
                Return _Proviso5
            End Get
            Set
                _Proviso5 = Value
                CallPropertyChanged(NameOf(Proviso5))
            End Set
        End Property

        Public Property Proviso6 As String
            Get
                Return _Proviso6
            End Get
            Set
                _Proviso6 = Value
                CallPropertyChanged(NameOf(Proviso6))
            End Set
        End Property

        Public Property Proviso7 As String
            Get
                Return _Proviso7
            End Get
            Set
                _Proviso7 = Value
                CallPropertyChanged(NameOf(Proviso7))
            End Set
        End Property

        Public Property Proviso8 As String
            Get
                Return _Proviso8
            End Get
            Set
                _Proviso8 = Value
                CallPropertyChanged(NameOf(Proviso8))
            End Set
        End Property

        Private Sub SetTotalAmount()
            TotalAmount = CInt(Amount1) + CInt(Amount2) + CInt(Amount3) + CInt(Amount4) + CInt(Amount5) + CInt(Amount6) + CInt(Amount7) + CInt(Amount8)
        End Sub

        Public Property Amount1 As String
            Get
                Return _Amount1
            End Get
            Set
                _Amount1 = IIf(Regex.IsMatch(Value, "^[1-9]"), Value, 0)
                CallPropertyChanged(NameOf(Amount1))
                SetTotalAmount()
            End Set
        End Property
        Public Property Amount2 As String
            Get
                Return _Amount2
            End Get
            Set
                _Amount2 = IIf(Regex.IsMatch(Value, "^[1-9]"), Value, 0)
                CallPropertyChanged(NameOf(Amount2))
                SetTotalAmount()
            End Set
        End Property

        Public Property Amount3 As String
            Get
                Return _Amount3
            End Get
            Set
                _Amount3 = IIf(Regex.IsMatch(Value, "^[1-9]"), Value, 0)
                CallPropertyChanged(NameOf(Amount3))
                SetTotalAmount()
            End Set
        End Property

        Public Property Amount4 As String
            Get
                Return _Amount4
            End Get
            Set
                _Amount4 = IIf(Regex.IsMatch(Value, "^[1-9]"), Value, 0)
                CallPropertyChanged(NameOf(Amount4))
                SetTotalAmount()
            End Set
        End Property

        Public Property Amount5 As String
            Get
                Return _Amount5
            End Get
            Set
                _Amount5 = IIf(Regex.IsMatch(Value, "^[1-9]"), Value, 0)
                CallPropertyChanged(NameOf(Amount5))
                SetTotalAmount()
            End Set
        End Property

        Public Property Amount6 As String
            Get
                Return _Amount6
            End Get
            Set
                _Amount6 = IIf(Regex.IsMatch(Value, "^[1-9]"), Value, 0)
                CallPropertyChanged(NameOf(Amount6))
                SetTotalAmount()
            End Set
        End Property

        Public Property Amount7 As String
            Get
                Return _Amount7
            End Get
            Set
                _Amount7 = IIf(Regex.IsMatch(Value, "^[1-9]"), Value, 0)
                CallPropertyChanged(NameOf(Amount7))
                SetTotalAmount()
            End Set
        End Property

        Public Property Amount8 As String
            Get
                Return _Amount8
            End Get
            Set
                _Amount8 = IIf(Regex.IsMatch(Value, "^[1-9]"), Value, 0)
                CallPropertyChanged(NameOf(Amount8))
                SetTotalAmount()
            End Set
        End Property

        Public Property IsReducedTaxRate1 As Boolean
            Get
                Return _IsReducedTaxRate1
            End Get
            Set
                _IsReducedTaxRate1 = Value
                CallPropertyChanged(NameOf(IsReducedTaxRate1))
            End Set
        End Property

        Public Property IsReducedTaxRate2 As Boolean
            Get
                Return _IsReducedTaxRate2
            End Get
            Set
                _IsReducedTaxRate2 = Value
                CallPropertyChanged(NameOf(IsReducedTaxRate2))
            End Set
        End Property

        Public Property IsReducedTaxRate3 As Boolean
            Get
                Return _IsReducedTaxRate3
            End Get
            Set
                _IsReducedTaxRate3 = Value
                CallPropertyChanged(NameOf(IsReducedTaxRate3))
            End Set
        End Property

        Public Property IsReducedTaxRate4 As Boolean
            Get
                Return _IsReducedTaxRate4
            End Get
            Set
                _IsReducedTaxRate4 = Value
                CallPropertyChanged(NameOf(IsReducedTaxRate4))
            End Set
        End Property

        Public Property IsReducedTaxRate5 As Boolean
            Get
                Return _IsReducedTaxRate5
            End Get
            Set
                _IsReducedTaxRate5 = Value
                CallPropertyChanged(NameOf(IsReducedTaxRate5))
            End Set
        End Property

        Public Property IsReducedTaxRate6 As Boolean
            Get
                Return _IsReducedTaxRate6
            End Get
            Set
                _IsReducedTaxRate6 = Value
                CallPropertyChanged(NameOf(IsReducedTaxRate6))
            End Set
        End Property

        Public Property IsReducedTaxRate7 As Boolean
            Get
                Return _IsReducedTaxRate7
            End Get
            Set
                _IsReducedTaxRate7 = Value
                CallPropertyChanged(NameOf(IsReducedTaxRate7))
            End Set
        End Property

        Public Property IsReducedTaxRate8 As Boolean
            Get
                Return _IsReducedTaxRate8
            End Get
            Set
                _IsReducedTaxRate8 = Value
                CallPropertyChanged(NameOf(IsReducedTaxRate8))
            End Set
        End Property

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
                If Regex.IsMatch(Value, "\d{7}") Then _PostalCode = $"{Mid(Value, 1, 3)}-{Mid(Value, 4)}"
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
                Dim i As Integer
                _Money = If(Integer.TryParse(Value, i), i.ToString, String.Empty)
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
        Public Sub New()
            Me.New(New SQLConnectInfrastructure, New ExcelOutputInfrastructure)
        End Sub

        <Runtime.InteropServices.DllImport("winmm.dll", CharSet:=Runtime.InteropServices.CharSet.Auto)>
        Private Shared Function MciSendString(command As String, buffer As Text.StringBuilder, bufferSize As Integer, hwndCallback As IntPtr) As Integer
        End Function

        Private Sub PlaySound()
            Dim fileName As String = ".\sounds\Cry.mp3"
            Dim aliasName As String = "MediaFile"
            Dim audio As String
            'ファイルを開く
            audio = "open """ + fileName + """ type mpegvideo alias " + aliasName
            If MciSendString(audio, Nothing, 0, IntPtr.Zero) <> 0 Then
                Return
            End If '再生する
            audio = "play " + aliasName
            Dim unused = MciSendString(audio, Nothing, 0, IntPtr.Zero)
        End Sub

        Public Property ViewTitle As String
            Get
                Return _ViewTitle
            End Get
            Set
                _ViewTitle = Value
                CallPropertyChanged(NameOf(ViewTitle))
            End Set
        End Property

        ''' <param name="lesseerepository">名義人データ</param>
        ''' <param name="excelrepository">エクセルデータ</param>
        Public Sub New(lesseerepository As IDataConectRepogitory, excelrepository As IOutputDataRepogitory)
            'PlaySound()
            DataBaseConecter = lesseerepository
            DataOutputConecter = excelrepository
            DataOutputConecter.AddOverLengthAddressListener(Me)
            Title = My.Resources.HonorificsText '敬称の大半は「様」なので設定する。Form.Loadイベント等ではデータバインディングされないので、こちらで設定する

            'Dim ver As System.Diagnostics.FileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location)
            ViewTitle = "いろいろ発行" '& ver.FileVersion
            With OutputContentsDictionary
                .Add(OutputContents.TransferPaper, My.Resources.TransferPaperText)
                .Add(OutputContents.Cho3Envelope, My.Resources.Cho3EnvelopeText)
                .Add(OutputContents.GravePamphletEnvelope, My.Resources.GravePamphletEnvelopeText)
                .Add(OutputContents.Kaku2Envelope, My.Resources.Kaku2EnvelopeText)
                .Add(OutputContents.WesternEnbelope, My.Resources.WesternEnvelopeText）
                .Add(OutputContents.Postcard, My.Resources.PostcardText）
            End With

            LastSaveDate = DataBaseConecter.GetLastSaveDate.GetDate

        End Sub

        ''' <summary>
        ''' 渡された管理番号で、名義人データを生成します。
        ''' </summary>
        Public Sub ReferenceLessee()

            MyLessee = DataBaseConecter.GetCustomerInfo(CustomerID)
            CustomerID = String.Empty

            If MyLessee Is Nothing Then Exit Sub

            If MyLessee.GetReceiverName.GetName = String.Empty Then
                SetLesseeProperty(MyLessee)
                NoteInput()
                Exit Sub
            End If

            If MyLessee.GetLesseeName.GetName = MyLessee.GetReceiverName.GetName Then
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

        Private Sub SetReceiverProperty(mylessee As LesseeCustomerInfoEntity)
            With mylessee
                AddresseeName = .GetReceiverName.GetName
                PostalCode = .GetReceiverPostalcode.Code
                Address1 = .GetReceiverAddress1.Address
                Address2 = .GetReceiverAddress2.Address
            End With
        End Sub

        Private Sub SetLesseeProperty(mylessee As LesseeCustomerInfoEntity)
            With mylessee
                AddresseeName = .GetLesseeName.GetName
                PostalCode = .GetPostalCode.Code
                Address1 = .GetAddress1.Address
                Address2 = .GetAddress2.Address
            End With
        End Sub

        Private Sub NoteInput()
            With MyLessee
                Note1 = .GetCustomerID.ShowDisplay
                Note2 = .GetGraveNumber.ReturnDisplayForGraveNumber
                Note3 = If(.GetArea.AreaValue > 0, $"{ .GetArea.ShowDisplay}", String.Empty)
            End With
        End Sub

        ''' <summary>
        ''' 振込用紙
        ''' </summary>
        Public Sub InputTransferData()
            Try
                Dim outputNote5 As String = IIf(IsNoteInfoIgnored, Note5, $"担当：{Note5}")

                DataOutputConecter.TransferPaperPrintOutput(CustomerID, AddresseeName, Title, PostalCode, Address1, Address2, Money, Note1, Note2, Note3, Note4, outputNote5, MultiOutputCheck, IsIPAmjMintyo)

            Catch ex As Exception
                Dim log As ILoggerRepogitory = New LogFileInfrastructure
                log.Log(ILoggerRepogitory.LogInfo.ERR, ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' 長3封筒
        ''' </summary>
        Public Sub InputCho3Envelope()
            Try
                DataOutputConecter.Cho3EnvelopeOutput(CustomerID, AddresseeName, Title, PostalCode, Address1, Address2, MultiOutputCheck, IsIPAmjMintyo)
            Catch ex As Exception
                Dim log As ILoggerRepogitory = New LogFileInfrastructure
                log.Log(ILoggerRepogitory.LogInfo.ERR, ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' 洋封筒
        ''' </summary>
        Public Sub InputWesternEnvelope()
            Try
                DataOutputConecter.WesternEnvelopeOutput(CustomerID, AddresseeName, Title, PostalCode, Address1, Address2, MultiOutputCheck, IsIPAmjMintyo)
            Catch ex As Exception
                Dim log As ILoggerRepogitory = New LogFileInfrastructure
                log.Log(ILoggerRepogitory.LogInfo.ERR, ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' 墓地パンフ
        ''' </summary>
        Public Sub InputGravePamphletEnvelope()
            Try
                DataOutputConecter.GravePamphletOutput(CustomerID, AddresseeName, Title, PostalCode, Address1, Address2, MultiOutputCheck, IsIPAmjMintyo)
            Catch ex As Exception
                Dim log As ILoggerRepogitory = New LogFileInfrastructure
                log.Log(ILoggerRepogitory.LogInfo.ERR, ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' 角２封筒
        ''' </summary>
        Public Sub InputKaku2Envelope()
            Try
                DataOutputConecter.Kaku2EnvelopeOutput(CustomerID, AddresseeName, Title, PostalCode, Address1, Address2, MultiOutputCheck, IsIPAmjMintyo)
            Catch ex As Exception
                Dim log As ILoggerRepogitory = New LogFileInfrastructure
                log.Log(ILoggerRepogitory.LogInfo.ERR, ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' はがき
        ''' </summary>
        Public Sub InputPostcard()
            Try
                DataOutputConecter.PostcardOutput(CustomerID, AddresseeName, Title, PostalCode, Address1, Address2, MultiOutputCheck, IsIPAmjMintyo)
            Catch ex As Exception
                Dim log As ILoggerRepogitory = New LogFileInfrastructure
                log.Log(ILoggerRepogitory.LogInfo.ERR, ex.Message)
            End Try
        End Sub

        Private AddressList As AddressDataListEntity
        Private _CallShowAddressDataView As Boolean
        Private _ViewTitle As String
        Private _OutputInfo As String
        Private _ButtonText As String = "出力"
        Private _OutputButtonIsEnabled As Boolean = True
        Private _IsShunjuen As Boolean
        Private _IsDisplayTax As Boolean
        Private _IsReissue As Boolean
        Private _CanOutput As Boolean
        Private _IsPrepaid As Boolean
        Private _PrepaidDate As Date = "1900-1-1"
        Private _AccountActivityDate As Date = Now

        ''' <summary>
        ''' 住所リストを表示するタイミングを管理します
        ''' </summary>
        ''' <returns></returns>
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
                PostalCode = $"{mycode.Substring(0, 3)}-{mycode.Substring(3, 4)}"
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
            AddressList = DataBaseConecter.GetPostalCodeList(PostalCode)
            If AddressList.GetCount = 0 Then
                Address1 = String.Empty
                Exit Sub
            End If

            If AddressList.GetCount = 1 Then
                Address1 = AddressList.GetItem(0).MyAddress.Address
                Exit Sub
            End If

            Advm = New AddressDataViewModel(AddressList)
            Advm.AddListener(Me)

            If InStr(PostalCode, "-") = 0 Then PostalCode = $"{PostalCode.Substring(0, 3)}-{PostalCode.Substring(3, 4)}"
            CreateShowFormCommand(New AddressDataView)

        End Sub

        ''' <summary>
        ''' プロパティを初期化する
        ''' </summary>
        Public Sub SetDefaultValue()

            AddresseeName = String.Empty
            PostalCode = String.Empty
            Address1 = String.Empty
            Address2 = String.Empty
            NoteTextClear()
            Money = String.Empty

        End Sub

        Private Sub AddressOverLengthInfo()
            AddressOverLengthInfoCommand = New DelegateCommand(
                Sub()
                    MessageInfo = New MessageBoxInfo With {
                    .Message = "住所が長いので、修正してください。",
                    .Button = MessageBoxButton.OK,
                    .Image = MessageBoxImage.Information,
                    .Title = "セルに収まりません"}
                    CallPropertyChanged(NameOf(AddressOverLengthInfoCommand))
                End Sub,
                Function()
                    Return True
                End Function
                )
        End Sub
        ''' <summary>
        ''' エラーメッセージを生成します
        ''' </summary>
        Private Sub CreateErrorMessage()
            ErrorMessageInfo = New DelegateCommand(
                   Sub()
                       MessageInfo = New MessageBoxInfo With {
                       .Message = My.Resources.AddresseeErrorInfo,
                       .Button = MessageBoxButton.OK,
                       .Image = MessageBoxImage.Error,
                       .Title = My.Resources.MandatoryItemsNotPreparedTitle
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
        Public Sub AddressDataNotify(_postalcode As String, _address As String) Implements IAddressDataViewCloseListener.AddressDataNotify
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

            Dim ac As New AddressConvert(Address1, Address2)

            OutputButtonIsEnabled = False
            ButtonText = "出力中"
            Select Case SelectedOutputContentsValue
                Case OutputContents.Cho3Envelope
                    InputCho3Envelope()
                Case OutputContents.GravePamphletEnvelope
                    InputGravePamphletEnvelope()
                Case OutputContents.Kaku2Envelope
                    InputKaku2Envelope()
                Case OutputContents.Postcard
                    InputPostcard()
                Case OutputContents.TransferPaper
                    If Not String.IsNullOrEmpty(Note4) And Not String.IsNullOrEmpty(Note5) Then
                        InputTransferData()
                    Else
                        IsNoteInfoIgnoredOutput()
                    End If
                Case OutputContents.WesternEnbelope
                    InputWesternEnvelope()
                Case Else
                    Exit Select
            End Select
            OutputButtonIsEnabled = True
            ButtonText = "出力"
        End Sub
        Private Sub IsNoteInfoIgnoredOutput()
            If IsNoteInfoIgnored Then
                InputTransferData()
                Exit Sub
            End If
            NoteInfoMessage()
            CallNoteInfo = True
        End Sub
        Public Property IsNoteInfoIgnored As Boolean
            Get
                Return _IsNoteInfoIgnored
            End Get
            Set
                _IsNoteInfoIgnored = Value
                CallPropertyChanged(NameOf(IsNoteInfoIgnored))
            End Set
        End Property

        Public Property NoteInfoMessageCommand As DelegateCommand

        Private Sub NoteInfoMessage()
            NoteInfoMessageCommand = New DelegateCommand(
                Sub()
                    CreateMessage()
                    CallPropertyChanged(NameOf(NoteInfoMessageCommand))
                End Sub,
                Function()
                    Return True
                End Function)
        End Sub

        Private Sub CreateMessage()
            MessageInfo = New MessageBoxInfo() With {
                    .Message = "入力必須項目に値が入っていません。",
                    .Button = MessageBoxButton.OK,
                    .Image = MessageBoxImage.Warning,
                    .Title = "名目、担当者は必須です"}
        End Sub
        Public Property CallNoteInfo As Boolean
            Get
                Return _CallNoteInfo
            End Get
            Set
                _CallNoteInfo = Value
                NoteInfoMessage()
                CallPropertyChanged(NameOf(CallNoteInfo))
                _CallNoteInfo = False
            End Set
        End Property
        Public Property OutputButtonIsEnabled As Boolean
            Get
                Return _OutputButtonIsEnabled
            End Get
            Set
                _OutputButtonIsEnabled = Value
                CallPropertyChanged(NameOf(OutputButtonIsEnabled))
            End Set
        End Property

        Public Property ButtonText As String
            Get
                Return _ButtonText
            End Get
            Set
                _ButtonText = Value
                CallPropertyChanged(NameOf(ButtonText))
            End Set
        End Property

        Public Property TotalAmount As Integer
            Get
                Return _totalAmount
            End Get
            Set(value As Integer)
                _totalAmount = value
                CallPropertyChanged(NameOf(TotalAmount))
                CanOutput = value > 0
            End Set
        End Property

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
        ''' 名義人と送付先のどちらを使用するかのメッセージを生成します
        ''' </summary>
        Private Sub CreateSelectAddresseeInfo()

            SelectAddresseeInfo = New DelegateCommand(
            Sub() 'テンプレート構文調べる
                MessageInfo = New MessageBoxInfo With
                {
               .Message = $"{MyLessee.GetLesseeName.ShowDisplay}{vbNewLine}{MyLessee.GetAddress1.ShowDisplay}{MyLessee.GetAddress2.ShowDisplay}{vbNewLine}{vbNewLine}{MyLessee.GetReceiverName.ShowDisplay}{vbNewLine}{MyLessee.GetReceiverAddress1.ShowDisplay}{MyLessee.GetReceiverAddress2.ShowDisplay}{vbNewLine}{vbNewLine}{My.Resources.DataSelectInfo}{vbNewLine}{vbNewLine}{My.Resources.LesseeDataSelect}",
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

        Protected Overrides Sub ValidateProperty(propertyName As String, value As Object)

            Dim rx As Regex

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
                    rx = New Regex("^[0-9]{3}-[0-9]{4}$")
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

                Case Else
                    Exit Select
            End Select
            InputErrorString = GetErrors(propertyName)
        End Sub

        Public Sub OverLengthCountNotify(_count As Integer) Implements IOverLengthAddress2Count.OverLengthCountNotify
            If _count > 0 Then CallAddressOverLengthMessage = True
        End Sub
    End Class

End Namespace