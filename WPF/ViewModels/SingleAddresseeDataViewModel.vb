Imports System.ComponentModel
Imports Domain
Imports Infrastructure
Imports System.Collections.ObjectModel

''' <summary>
''' メインフォームに情報を渡すビューモデルクラス
''' </summary>
Public Class SingleAddresseeDataViewModel
    Implements INotifyPropertyChanged, IAddressDataViewCloseListener

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

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
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(SelectedOutputContentsValue)))
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
    Private ReadOnly DataOutputConecter As IAdresseeOutputRepogitory

    ''' <summary>
    ''' 墓地札画面に移動するコマンド
    ''' </summary>
    ''' <returns></returns>
    Public Property GotoGravePanelDataView As ICommand
        Get
            If _GotoGravePanelDataView Is Nothing Then _GotoGravePanelDataView = New GotoGravePanelDataViewCommand(Me)
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
            If _GotoMultiAddresseeDataView Is Nothing Then _GotoMultiAddresseeDataView = New GotoMultiAddresseeDataViewCommand(Me)
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
            If _DataOutput Is Nothing Then _DataOutput = New OutputDataCommand(Me)
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
            If _NoteClear Is Nothing Then _NoteClear = New NoteClearCommand(Me)
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
            If _AddressReference Is Nothing Then _AddressReference = New AddressReferenceCommand(Me)
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
            If _ReferencePostalcode Is Nothing Then _ReferencePostalcode = New PostalcodeReferemceCommand(Me)
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
            If _ReferenceLesseeCommand Is Nothing Then _ReferenceLesseeCommand = New ReferenceLesseeCommand(Me)
            Return _ReferenceLesseeCommand
        End Get
        Set
            _ReferenceLesseeCommand = Value
        End Set
    End Property

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

    Public Property LastSaveDate As Date
        Get
            Return _LastSaveDate
        End Get
        Set

            _LastSaveDate = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(LastSaveDate)))
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
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(PermitReference)))
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
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(CustomerID)))
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
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(MultiOutputCheck)))
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
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(AddresseeName)))
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
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Title)))
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
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(PostalCode)))
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
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Address1)))
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
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Address2)))
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
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Note1)))
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
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Note2)))
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
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Note3)))
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
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Note4)))
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
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Note5)))
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
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Money)))
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
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(TransferPaperMenuEnabled)))
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
    Sub New(ByVal lesseerepository As IDataConectRepogitory, ByVal excelrepository As IAdresseeOutputRepogitory)
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

        LastSaveDate = DataBaseConecter.GetLastSaveDate.SaveDate

    End Sub

    ''' <summary>
    ''' 渡された管理番号で、名義人データを生成します。
    ''' </summary>
    Public Sub ReferenceLessee()

        Dim myLessee As LesseeCustomerInfoEntity

        myLessee = DataBaseConecter.GetCustomerInfo(CustomerID)

        If myLessee Is Nothing Then Exit Sub

        If myLessee.GetReceiverName = String.Empty Then SetLesseeProperty(myLessee)

        If myLessee.GetLesseeName = myLessee.GetReceiverName Then SetReceiverProperty(myLessee)

        If myLessee.GetLesseeName <> myLessee.GetReceiverName Then
            SetLesseeProperty(myLessee)
        Else
            SetReceiverProperty(myLessee)
        End If

        Note1 = "管理番号 " & myLessee.GetCustomerID
        Note2 = myLessee.GetGraveNumber
        If myLessee.GetArea > 0 Then
            Note3 = "面積 " & myLessee.GetArea & " ㎡"
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
        If HasError() Then Exit Sub
        DataOutputConecter.TransferPaperPrintOutput(AddresseeName, Title, PostalCode, Address1, Address2, Money, Note1, Note2, Note3, Note4, Note5, MultiOutputCheck)
        SetDefaultValue()
    End Sub

    ''' <summary>
    ''' 長3封筒
    ''' </summary>
    Public Sub InputCho3Envelope()
        If HasError() Then Exit Sub
        DataOutputConecter.Cho3EnvelopeOutput(AddresseeName, Title, PostalCode, Address1, Address2, MultiOutputCheck)
    End Sub

    ''' <summary>
    ''' 洋封筒
    ''' </summary>
    Public Sub InputWesternEnvelope()
        If HasError() Then Exit Sub
        DataOutputConecter.WesternEnvelopeOutput(AddresseeName, Title, PostalCode, Address1, Address2, MultiOutputCheck)
    End Sub

    ''' <summary>
    ''' 墓地パンフ
    ''' </summary>
    Public Sub InputGravePamphletEnvelope()
        If HasError() Then Exit Sub
        DataOutputConecter.GravePamphletOutput(AddresseeName, Title, PostalCode, Address1, Address2, MultiOutputCheck)
    End Sub

    ''' <summary>
    ''' 角２封筒
    ''' </summary>
    Public Sub InputKaku2Envelope()
        If HasError() Then Exit Sub
        DataOutputConecter.Kaku2EnvelopeOutput(AddresseeName, Title, PostalCode, Address1, Address2, MultiOutputCheck)
    End Sub

    ''' <summary>
    ''' はがき
    ''' </summary>
    Public Sub InputPostcard()
        If HasError() Then Exit Sub
        DataOutputConecter.PostcardOutput(AddresseeName, Title, PostalCode, Address1, Address2, MultiOutputCheck)
    End Sub

    ''' <summary>
    ''' ラベル
    ''' </summary>
    Public Sub InputLabel()
        If HasError() Then Exit Sub
        DataOutputConecter.LabelOutput(AddresseeName, Title, PostalCode, Address1, Address2)
        SetDefaultValue()
    End Sub

    ''' <summary>
    ''' 住所を検索します
    ''' </summary>
    Public Sub ReferenceAddress()

        Dim AddressList As AddressesEntity
        Dim myAddress As AddressDataEntity

        AddressList = DataBaseConecter.GetAddressList(Address1)
        If AddressList.List.Count = 0 Then Exit Sub

        '検索結果が1件なら住所一覧画面は呼ばずにプロパティに入力する
        If AddressList.List.Count = 1 Then
            myAddress = AddressList.List.Item(0)
            Address1 = myAddress.MyAddress.Address
            Dim mycode As String = myAddress.MyPostalcode.Code
            PostalCode = mycode.Substring(0, 3) & "-" & mycode.Substring(3, 4)
            Exit Sub
        End If

        Advm = New AddressDataViewModel(AddressList)
        Advm.AddListener(Me)
        Dim adv As New AddressDataView
        adv.ShowDialog()

    End Sub

    ''' <summary>
    ''' 郵便番号で住所を検索します
    ''' </summary>
    Public Sub ReferenceAddress_Postalcode()

        If PostalCode.Length < 7 Then Exit Sub
        Dim address As AddressDataEntity = DataBaseConecter.GetAddress(PostalCode)
        Address1 = address.MyAddress.Address
        If PostalCode.Length = 7 Then PostalCode = PostalCode.Substring(0, 3) & "-" & PostalCode.Substring(3, 4)

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
    ''' 必ず値が入っていないといけないプロパティがEmptyならTrueを返す
    ''' </summary>
    ''' <returns></returns>
    Private Function HasError() As Boolean

        If AddresseeName = String.Empty Then GoTo TruePart
        If PostalCode = String.Empty Then GoTo TruePart
        If Address1 = String.Empty Then GoTo TruePart
        If Address2 = String.Empty Then GoTo TruePart

        Return False

TruePart:
        MsgBox("宛先が不十分です", MsgBoxStyle.Critical, "必須項目不備")
        Return True
    End Function

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
            Case OutputContents.WesternEnbelope
                InputWesternEnvelope()
        End Select

    End Sub

    ''' <summary>
    ''' 振込用紙の独自の欄のEnableを変えます
    ''' </summary>
    Public Sub TransferPaperMenuEnabledChange()
        TransferPaperMenuEnabled = SelectedOutputContentsValue = OutputContents.TransferPaper
    End Sub

    ''' <summary>
    ''' 一括出力画面を表示します
    ''' </summary>
    Public Sub ShowMultiAddresseeDataView()
        Dim madv As New MultiAddresseeDataView
        madv.ShowDialog()
    End Sub

    Public Sub ShowGravePanelDataView()
        Dim gpdv As New GravePanelDataView
        gpdv.ShowDialog()
    End Sub
End Class
