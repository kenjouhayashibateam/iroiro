Imports System.ComponentModel
Imports Domain
Imports Infrastructure

''' <summary>
''' メインフォームに情報を渡すビューモデルクラス
''' </summary>
Public Class SingleAddresseeDataViewModel
    Implements INotifyPropertyChanged
    Implements IExitButtonClickListener

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    ''' <summary>
    ''' 名義人情報を保持するリポジトリ
    ''' </summary>
    Private ReadOnly DataBaseConecter As IDataConectRepogitory

    ''' <summary>
    ''' 印刷等のデータを保持するリポジトリ
    ''' </summary>
    Private ReadOnly DataOutputConecter As IAdresseeOutputRepogitory

    Private _Addresseename As String
    Private _PostalCode As String
    Private _Address1 As String
    Private _Address2 As String
    Private _Note1 As String
    Private _Note2 As String
    Private _IsNoteInput As Boolean
    Private _Note3 As String
    Private _Note4 As String
    Private _Note5 As String
    Private _Money As String
    Private _Title As String
    Private _MultiOutputCheck As Boolean

    ''' <summary>
    ''' 続けて入力する時に、既存のデータを消さずに次のデータを出力するかのチェック
    ''' </summary>
    ''' <returns></returns>
    Public Property MultiOutputCheck As Boolean
        Get
            Return _MultiOutputCheck
        End Get
        Set
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
    ''' 備考不要チェック
    ''' </summary>
    Public Property IsNoteInput As Boolean
        Get
            Return _IsNoteInput
        End Get
        Set
            If Value = IsNoteInput Then Return
            _IsNoteInput = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(IsNoteInput)))
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
        DataOutputConecter = excelrepository
        Title = "様" '敬称の大半は「様」なので設定する。Form.Loadイベント等ではデータバインディングされないので、こちらで設定する
    End Sub

    ''' <summary>
    ''' 渡された管理番号で、名義人データを生成します。
    ''' </summary>
    ''' <param name="managementNumber">検索する管理番号</param>
    Public Sub ReferenceLessee(ByVal managementNumber As String)

        Dim myLessee As LesseeCustomerInfoEntity

        myLessee = DataBaseConecter.GetCustomerInfo(managementNumber)
        If myLessee Is Nothing Then
            MsgBox("名義人データが見つかりません。", MsgBoxStyle.Critical, "管理番号が無効です")
            Exit Sub
        End If

        AddresseeName = myLessee.GetAddressee
        PostalCode = myLessee.GetPostalCode
        Address1 = myLessee.GetAddress1
        Address2 = myLessee.GetAddress2
        If IsNoteInput Then Exit Sub
        Note1 = "管理番号 " & myLessee.GetCustomerID
        Note2 = myLessee.GetGraveNumber
        Note3 = "面積 " & myLessee.GetArea & " ㎡"

    End Sub

    ''' <summary>
    ''' 郵便番号を使用して、住所を検索する
    ''' </summary>
    ''' <param name="postalcode">郵便番号</param>
    Public Sub GetAddress(ByVal postalcode As String)
        Address1 = DataBaseConecter.GetAddress(postalcode)
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
    ''' <param name="address">検索する住所</param>
    Public Sub ReferenceAddress(ByVal address As String)

        Dim AddressList As List(Of AddressDataEntity)
        Dim myAddress As AddressDataEntity

        AddressList = DataBaseConecter.GetAddressList(address)
        If AddressList.Count = 0 Then Exit Sub

        '検索結果が1件なら住所一覧画面は呼ばずにプロパティに入力する
        If AddressList.Count = 1 Then
            myAddress = AddressList.Item(0)
            Address1 = myAddress.GetAddress
            PostalCode = myAddress.GetPostalCode
            Exit Sub
        End If

        '住所一覧画面にデータを渡して開く
        AddressDataView.SetList(AddressList)
        AddressDataView.AddListener(Me)
        AddressDataView.ShowDialog()

    End Sub

    Public Sub Notify(_postalCode As String, _address As String) Implements IExitButtonClickListener.Notify
        PostalCode = _postalCode.Substring(0, 3) & "-" & _postalCode.Substring(3, 4)
        Address1 = _address
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

End Class
