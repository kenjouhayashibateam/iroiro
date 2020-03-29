Imports System.ComponentModel
Imports System.Collections.Specialized
Imports System.Collections.ObjectModel
Imports Domain
Imports Infrastructure
Imports WPF.Command
Imports WPF.Data
Imports System.Linq

Public Interface INotifyListAdd
    Sub Notify(ByVal gravepanelData As GravePanelDataEntity)
End Interface

Namespace ViewModels
    ''' <summary>
    ''' 墓地札登録画面ViewModel 
    ''' </summary>
    Public Class CreateGravePanelDataViewModel
        Inherits BaseViewModel
        Implements INotifyPropertyChanged, INotifyCollectionChanged

        Private Listener As INotifyListAdd
        Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

        ''' <summary>
        ''' 名義人か送付先どちらかを選ばせるメッセージボックス表示コマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property SelectAddresseeInfo As DelegateCommand
        ''' <summary>
        '''登録確認メッセージボックス表示コマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property ConfirmationRegistraterInfo As DelegateCommand
        ''' <summary>
        ''' 登録完了メッセージボックス表示コマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property CompleteRegistrationInfo As DelegateCommand

        Public Sub AddListAddListener(ByVal _listener As INotifyListAdd)
            Listener = _listener
        End Sub

        ''' <summary>
        ''' 墓地番号検索コマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property ReferenceGraveNumberCommand As ICommand
            Get
                _ReferenceGraveNumberCommand = New DelegateCommand(
                    Sub()
                        ReferenceLesseeData()
                        CallPropertyChanged(NameOf(ReferenceGraveNumberCommand))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
                Return _ReferenceGraveNumberCommand
            End Get
            Set
                _ReferenceGraveNumberCommand = Value
            End Set
        End Property

        ''' <summary>
        ''' メッセージボックスプロパティを保持します
        ''' </summary>
        ''' <returns></returns>
        Public Property MessageInfo As MessageBoxInfo
            Get
                Return _MessageInfo
            End Get
            Set
                _MessageInfo = Value
                CallPropertyChanged(NameOf(MessageInfo))
            End Set
        End Property

        Private ReadOnly DataConect As IDataConectRepogitory
        Private _IsEnabledKuiki As Boolean = False
        Private _IsEnabledGawa As Boolean = False
        Private _IsEnabledBan As Boolean = False
        Private _IsEnabledEdaban As Boolean = False
        Private _SelectedKu As String
        Private _SelectedKuiki As String
        Private _SelectedGawa As String
        Private _SelectedBan As String
        Private _SelectedEdaban As String
        Private _GraveNumberKuikiList As GraveNumberEntity.KuikiList
        Private _KuikiText As String
        Private _GraveNumberGawaList As GraveNumberEntity.GawaList
        Private _GawaText As String
        Private _GraveNumberKuList As ObservableCollection(Of GraveNumberEntity.Ku)
        Private _BanText As String
        Private _GraveNumberBanList As GraveNumberEntity.BanList
        Private _EdabanText As String
        Private _GraveNumberEdabanList As GraveNumberEntity.EdabanList
        Private _CustomerID As String
        Private _FamilyName As String
        Private _Area As Double
        Private _MessageInfo As MessageBoxInfo
        Private _CallSelectAddresseeInfo As Boolean = False
        Private _ReferenceGraveNumberCommand As ICommand
        Private _DisplayForGraveNumber As String
        Private _KuText As String
        Private _ContractContents As ObservableCollection(Of String)
        Private _ContractContent As String
        Private _GravePanelRegistration As ICommand
        Private _IsConfirmationRegister As Boolean
        Private _CallCompleteRegistration As Boolean
        Private _RegistrationCustomerID As String
        Private _FullName As String
        Private _CallRegistrationErrorMessageInfo As Boolean
        Private _RegistrationErrorMessageInfo As ICommand

        Public Property FullName As String
            Get
                Return _FullName
            End Get
            Set
                _FullName = Value
                CallPropertyChanged(NameOf(FullName))
            End Set
        End Property

        ''' <summary>
        ''' 登録する管理番号
        ''' </summary>
        ''' <returns></returns>
        Public Property RegistrationCustomerID As String
            Get
                Return _RegistrationCustomerID
            End Get
            Set
                _RegistrationCustomerID = Value
                CallPropertyChanged(NameOf(RegistrationCustomerID))
            End Set
        End Property

        ''' <summary>
        ''' 墓地番号「区」
        ''' </summary>
        ''' <returns></returns>
        Public Property KuField As GraveNumberEntity.Ku

        ''' <summary>
        ''' 登録完了メッセージボックスを呼び出します
        ''' </summary>
        ''' <returns></returns>
        Public Property CallCompleteRegistration As Boolean
            Get
                Return _CallCompleteRegistration
            End Get
            Set
                _CallCompleteRegistration = Value
                CallPropertyChanged(NameOf(CallCompleteRegistration))
                _CallCompleteRegistration = False
            End Set

        End Property

        ''' <summary>
        ''' 登録確認メッセージボックスを呼び出します
        ''' </summary>
        ''' <returns></returns>
        Public Property IsConfirmationRegister As Boolean
            Get
                Return _IsConfirmationRegister
            End Get
            Set
                _IsConfirmationRegister = Value
                CallPropertyChanged(NameOf(IsConfirmationRegister))
                _IsConfirmationRegister = False
            End Set
        End Property

        ''' <summary>
        ''' 墓地札登録コマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property GravePanelDataRegistration As ICommand
            Get
                _GravePanelRegistration = New DelegateCommand(
                    Sub()
                        DataRegistration()
                        CallPropertyChanged(NameOf(GravePanelDataRegistration))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
                Return _GravePanelRegistration
            End Get
            Set
                _GravePanelRegistration = Value
            End Set
        End Property

        ''' <summary>
        ''' 契約内容
        ''' </summary>
        ''' <returns></returns>
        Public Property ContractContent As String
            Get
                Return _ContractContent
            End Get
            Set
                _ContractContent = Value
                CallPropertyChanged(NameOf(ContractContent))
            End Set
        End Property

        ''' <summary>
        ''' 契約内容リスト
        ''' </summary>
        ''' <returns></returns>
        Public Property ContractContents As ObservableCollection(Of String)
            Get
                Return _ContractContents
            End Get
            Set
                _ContractContents = Value
                CallPropertyChanged(NameOf(ContractContents))
            End Set
        End Property

        ''' <summary>
        ''' 墓地番号「区」
        ''' </summary>
        ''' <returns></returns>
        Public Property KuText As String
            Get
                Return _KuText
            End Get
            Set
                _KuText = Value
                If Not Enumerable.Contains(GraveNumberKuList, New GraveNumberEntity.Ku(Value)) Then RegistrationCustomerID = My.Resources.UndefinedCustomerID
                CallPropertyChanged(NameOf(KuText))
                ValidateProperty(NameOf(KuText), Value)
            End Set
        End Property

        ''' <summary>
        ''' 表示用墓地番号
        ''' </summary>
        ''' <returns></returns>
        Public Property DisplayForGraveNumber As String
            Get
                Return _DisplayForGraveNumber
            End Get
            Set
                _DisplayForGraveNumber = Value
                CallPropertyChanged(NameOf(DisplayForGraveNumber))
                ValidateProperty(NameOf(DisplayForGraveNumber), Value)
            End Set
        End Property

        Private Property MsgResult As MessageBoxResult

        Private Property MyLessee As LesseeCustomerInfoEntity

        ''' <summary>
        ''' 名義人データと、送付先データのどちらを使用するか選択するメッセージボックスを呼び出します
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
        ''' 面積
        ''' </summary>
        ''' <returns></returns>
        Public Property Area As Double
            Get
                Return _Area
            End Get
            Set
                _Area = Value
                CallPropertyChanged(NameOf(Area))
                ValidateProperty(NameOf(Area), Value)
            End Set
        End Property

        ''' <summary>
        ''' 苗字
        ''' </summary>
        ''' <returns></returns>
        Public Property FamilyName As String
            Get
                Return _FamilyName
            End Get
            Set
                _FamilyName = Value
                CallPropertyChanged(NameOf(FamilyName))
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
        ''' 墓地番号を基に名義人クラスを生成してプロパティにセットします
        ''' </summary>
        Private Sub SetLesseeData()

            If SelectedKu = String.Empty Then Exit Sub
            If SelectedKuiki = String.Empty Then Exit Sub
            If SelectedGawa = String.Empty Then Exit Sub
            If SelectedBan = String.Empty Then Exit Sub
            Dim edabanstring As String
            If SelectedEdaban = String.Empty Then
                edabanstring = "%"
            Else
                edabanstring = SelectedEdaban
            End If

            MyLessee = DataConect.GetCustomerInfo_GraveNumber(SelectedKu, SelectedKuiki, SelectedGawa, SelectedBan, edabanstring)
            InputLesseeData()

        End Sub

        ''' <summary>
        ''' 名義人クラスのプロパティをViewModel のプロパティにセットします
        ''' </summary>
        Private Sub InputLesseeData()
            SetLesseeName()
            RegistrationCustomerID = MyLessee.GetCustomerID
            Area = MyLessee.GetArea
        End Sub

        ''' <summary>
        ''' 苗字のみをセットします
        ''' </summary>
        Private Sub SetLesseeName()
            FamilyName = MyLessee.GetLesseeName.Substring(0, InStr(MyLessee.GetLesseeName, "　")).Trim
            FullName = MyLessee.GetLesseeName
        End Sub

        ''' <summary>
        ''' 名義人データと送付先のどちらを使用するかの確認メッセージボックスを生成します
        ''' </summary>
        Public Sub CreateSelectAddresseeInfo()

            SelectAddresseeInfo = New DelegateCommand(
            Sub()
                MessageInfo = New MessageBoxInfo With
                {
                .Message = My.Resources.FieldPropertyMessage_Lessee & MyLessee.GetLesseeName & vbNewLine & My.Resources.FieldPropertyMessage_Receiver & MyLessee.GetReceiverName & vbNewLine & vbNewLine &
                                My.Resources.DataSelectInfo & vbNewLine & vbNewLine & My.Resources.LesseeDataSelect,
                                .Button = MessageBoxButton.YesNo,
                               .Image = MessageBoxImage.Question,
                               .Title = My.Resources.DataSelectInfoTitle
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
        ''' 表示用墓地番号を返します
        ''' </summary>
        ''' <returns></returns>
        Private Function ReturnDisplayForGraveNumber() As String
            Return KuText & KuikiText & My.Resources.GraveKuString & GawaText & My.Resources.GraveGawaString & BanText & EdabanText & My.Resources.GraveBanString
        End Function

        ''' <summary>
        ''' 枝番　
        ''' </summary>
        ''' <returns></returns>
        Public Property EdabanText As String
            Get
                Return _EdabanText
            End Get
            Set
                _EdabanText = Value
                CallPropertyChanged(NameOf(EdabanText))
            End Set
        End Property

        ''' <summary>
        ''' 番
        ''' </summary>
        ''' <returns></returns>
        Public Property BanText As String
            Get
                Return _BanText
            End Get
            Set
                _BanText = Value
                CallPropertyChanged(NameOf(BanText))
                ValidateProperty(NameOf(BanText), Value)
                If GraveNumberBanList Is Nothing Then Exit Property
                If Not Enumerable.Contains(GraveNumberBanList.List, New GraveNumberEntity.Ban(Value)) Then RegistrationCustomerID = My.Resources.UndefinedCustomerID
            End Set
        End Property

        ''' <summary>
        ''' 側
        ''' </summary>
        ''' <returns></returns>
        Public Property GawaText As String
            Get
                Return _GawaText
            End Get
            Set
                _GawaText = Value
                CallPropertyChanged(NameOf(GawaText))
                ValidateProperty(NameOf(GawaText), Value)
                If GraveNumberGawaList Is Nothing Then Exit Property
                If Not Enumerable.Contains(GraveNumberGawaList.List, New GraveNumberEntity.Gawa(Value)) Then RegistrationCustomerID = My.Resources.UndefinedCustomerID
            End Set
        End Property

        ''' <summary>
        ''' 区域
        ''' </summary>
        ''' <returns></returns>
        Public Property KuikiText As String
            Get
                Return _KuikiText
            End Get
            Set
                If Value = String.Empty Then
                    SelectedGawa = String.Empty
                    IsEnabledGawa = False
                Else
                    IsEnabledGawa = True
                End If
                _KuikiText = Value

                CallPropertyChanged(NameOf(KuikiText))
                ValidateProperty(NameOf(KuikiText), Value)
                If GraveNumberKuikiList Is Nothing Then Exit Property
                If Not Enumerable.Contains(GraveNumberKuikiList.List, New GraveNumberEntity.Kuiki(Value)) Then RegistrationCustomerID = My.Resources.UndefinedCustomerID
            End Set
        End Property

        ''' <summary>
        ''' 墓地番号　区リスト
        ''' </summary>
        ''' <returns></returns>
        Public Property GraveNumberKuList As ObservableCollection(Of GraveNumberEntity.Ku)
            Get
                Return _GraveNumberKuList
            End Get
            Set
                _GraveNumberKuList = Value
                CallPropertyChanged(NameOf(GraveNumberKuList))
            End Set
        End Property

        ''' <summary>
        ''' 墓地番号クラス
        ''' </summary>
        Public Class GraveNumberField

            Public Property Value As String

            Sub New(ByVal _value As String)
                Value = _value
            End Sub
        End Class

        ''' <summary>
        ''' 墓地番号　番リスト
        ''' </summary>
        ''' <returns></returns>
        Public Property GraveNumberBanList As GraveNumberEntity.BanList
            Get
                Return _GraveNumberBanList
            End Get
            Set
                _GraveNumberBanList = Value
                CallPropertyChanged(NameOf(GraveNumberBanList))
            End Set
        End Property

        ''' <summary>
        ''' 墓地番号　側リスト
        ''' </summary>
        ''' <returns></returns>
        Public Property GraveNumberGawaList As GraveNumberEntity.GawaList
            Get
                Return _GraveNumberGawaList
            End Get
            Set
                _GraveNumberGawaList = Value
                CallPropertyChanged(NameOf(GraveNumberGawaList))
            End Set
        End Property

        ''' <summary>
        ''' 墓地番号　区域リスト
        ''' </summary>
        ''' <returns></returns>
        Public Property GraveNumberKuikiList As GraveNumberEntity.KuikiList
            Get
                Return _GraveNumberKuikiList
            End Get
            Set
                _GraveNumberKuikiList = Value
                CallPropertyChanged(NameOf(GraveNumberKuikiList))
            End Set
        End Property

        ''' <summary>
        ''' 墓地番号　枝番リスト
        ''' </summary>
        ''' <returns></returns>
        Public Property GraveNumberEdabanList As GraveNumberEntity.EdabanList
            Get
                Return _GraveNumberEdabanList
            End Get
            Set
                _GraveNumberEdabanList = Value
                CallPropertyChanged(NameOf(GraveNumberEdabanList))
            End Set
        End Property

        Sub New()
            Me.New(New SQLConectInfrastructure)
        End Sub

        Sub New(ByVal _datarepository As IDataConectRepogitory)

            DataConect = _datarepository

            GraveNumberKuList = New ObservableCollection(Of GraveNumberEntity.Ku)

            AddGraveKu("01")
            AddGraveKu("02")
            AddGraveKu("03")
            AddGraveKu("04")
            AddGraveKu("05")
            AddGraveKu("10")
            AddGraveKu("11")
            AddGraveKu("12")
            AddGraveKu("20")

            ContractContents = New ObservableCollection(Of String)
            With ContractContents
                .Add(My.Resources.Contract_Kusatori)
                .Add(My.Resources.Contract_Ueki)
            End With

        End Sub

        ''' <summary>
        ''' 墓地番号の区をリストに格納します
        ''' </summary>
        ''' <param name="originalvalue"></param>
        Private Sub AddGraveKu(ByVal originalvalue As String)
            KuField = New GraveNumberEntity.Ku(originalvalue)
            GraveNumberKuList.Add(KuField)
        End Sub

        ''' <summary>
        ''' 選択された枝番
        ''' </summary>
        ''' <returns></returns>
        Public Property SelectedEdaban As String
            Get
                Return _SelectedEdaban
            End Get
            Set
                If _IsEnabledBan = False Then Return
                _SelectedEdaban = Value
                CallPropertyChanged(NameOf(SelectedEdaban))
                SetLesseeData()
            End Set
        End Property

        ''' <summary>
        ''' 選択された番
        ''' </summary>
        ''' <returns></returns>
        Public Property SelectedBan As String
            Get
                Return _SelectedBan
            End Get
            Set
                _SelectedBan = Value
                CallPropertyChanged(NameOf(SelectedBan))
                If Value = String.Empty Then
                    SelectedEdaban = String.Empty
                    IsEnabledEdaban = False
                Else
                    SetNextGraveNumberField(GravenumberGanre.EDABAN, Value)
                End If
            End Set
        End Property

        ''' <summary>
        ''' 選択された側
        ''' </summary>
        ''' <returns></returns>
        Public Property SelectedGawa As String
            Get
                Return _SelectedGawa
            End Get
            Set
                _SelectedGawa = Value
                CallPropertyChanged(NameOf(SelectedGawa))
                If Value = String.Empty Then
                    SelectedBan = String.Empty
                    IsEnabledBan = False
                Else
                    SetNextGraveNumberField(GravenumberGanre.BAN, Value)
                End If
            End Set
        End Property

        ''' <summary>
        ''' 選択された区域
        ''' </summary>
        ''' <returns></returns>
        Public Property SelectedKuiki As String
            Get
                Return _SelectedKuiki
            End Get
            Set
                _SelectedKuiki = Value
                CallPropertyChanged(NameOf(SelectedKuiki))
                If Value = String.Empty Then
                    SelectedGawa = String.Empty
                    IsEnabledGawa = False
                Else
                    SetNextGraveNumberField(GravenumberGanre.GAWA, Value)
                End If
            End Set
        End Property

        ''' <summary>
        ''' 墓地番号のパーツ
        ''' </summary>
        Private Enum GravenumberGanre
            KU
            KUIKI
            GAWA
            BAN
            EDABAN
        End Enum

        ''' <summary>
        ''' 墓地番号パーツ（区、区域、側、番、枝番）の指定したリストを呼び出します。
        ''' </summary>
        ''' <param name="nextganre">呼び出す墓地番号パーツ</param>
        ''' <param name="numbervalue">基になる墓地番号パーツ</param>
        Private Sub SetNextGraveNumberField(ByVal nextganre As GravenumberGanre, ByVal numbervalue As String)


            Select Case nextganre
                Case GravenumberGanre.KUIKI
                    GraveNumberKuikiList = DataConect.GetKuikiList(numbervalue)
                    IsEnabledKuiki = True
                Case GravenumberGanre.GAWA
                    GraveNumberGawaList = DataConect.GetGawaList(SelectedKu, numbervalue)
                    IsEnabledGawa = True
                Case GravenumberGanre.BAN
                    GraveNumberBanList = DataConect.GetBanList(SelectedKu, SelectedKuiki, numbervalue)
                    IsEnabledBan = True
                Case GravenumberGanre.EDABAN
                    GraveNumberEdabanList = DataConect.GetEdabanList(SelectedKu, SelectedKuiki, SelectedGawa, numbervalue)
                    If GraveNumberEdabanList Is Nothing Then
                        SetLesseeData()
                    Else
                        IsEnabledEdaban = True
                    End If

                Case Else
                    Exit Sub
            End Select

        End Sub

        ''' <summary>
        ''' 選択された区
        ''' </summary>
        ''' <returns></returns>
        Public Property SelectedKu As String
            Get
                Return _SelectedKu
            End Get
            Set
                _SelectedKu = Value
                CallPropertyChanged(NameOf(SelectedKu))
                If Value = String.Empty Then
                    SelectedKuiki = String.Empty
                    IsEnabledKuiki = False
                Else
                    SetNextGraveNumberField(GravenumberGanre.KUIKI, Value)
                End If
            End Set
        End Property

        ''' <summary>
        ''' 枝番のEnableを設定します
        ''' </summary>
        ''' <returns></returns>
        Public Property IsEnabledEdaban As Boolean
            Get
                Return _IsEnabledEdaban
            End Get
            Set
                _IsEnabledEdaban = Value
                CallPropertyChanged(NameOf(IsEnabledEdaban))
            End Set
        End Property

        ''' <summary>
        ''' 番のEnableを設定します
        ''' </summary>
        ''' <returns></returns>
        Public Property IsEnabledBan As Boolean
            Get
                Return _IsEnabledBan
            End Get
            Set
                _IsEnabledBan = Value
                CallPropertyChanged(NameOf(IsEnabledBan))
            End Set
        End Property

        ''' <summary>
        ''' 側のEnableを設定します
        ''' </summary>
        ''' <returns></returns>
        Public Property IsEnabledGawa As Boolean
            Get
                Return _IsEnabledGawa
            End Get
            Set
                _IsEnabledGawa = Value
                CallPropertyChanged(NameOf(IsEnabledGawa))
            End Set
        End Property

        ''' <summary>
        ''' 区域のEnableを設定します
        ''' </summary>
        ''' <returns></returns>
        Public Property IsEnabledKuiki As Boolean
            Get
                Return _IsEnabledKuiki
            End Get
            Set
                _IsEnabledKuiki = Value
                CallPropertyChanged(NameOf(IsEnabledKuiki))
            End Set
        End Property

        ''' <summary>
        ''' 管理番号を使用して名義人データを呼び出し、各プロパティに格納します
        ''' </summary>
        Public Sub ReferenceLesseeData()
            MyLessee = DataConect.GetCustomerInfo(CustomerID)
            If MyLessee Is Nothing Then Exit Sub
            InputLesseeData()
            RegistrationCustomerID = MyLessee.GetCustomerID
            DisplayForGraveNumber = MyLessee.GetGraveNumber.GetNumber
            With MyLessee.GetGraveNumber
                KuText = .KuField.DisplayForField
                KuikiText = .KuikiField.DisplayForField
                GawaText = .GawaField.DisplayForField
                BanText = .BanField.DisplayForField
                EdabanText = .EdabanField.DisplayForField
            End With
        End Sub

        ''' <summary>
        ''' 墓地札データを登録します
        ''' </summary>
        Public Sub DataRegistration()

            DisplayForGraveNumber = ReturnDisplayForGraveNumber()
            CreateConfirmationRegisterInfo()
            IsConfirmationRegister = True

            Dim NowDate As Date = Now
            Dim DefaultDate As Date = My.Resources.DefaultDate
            If MsgResult = MessageBoxResult.No Then Exit Sub
            If HasErrors Then
                CallRegistrationErrorMessageInfo = True
                Exit Sub
            End If
            Dim gpd As New GravePanelDataEntity(0, RegistrationCustomerID, FamilyName, FullName, DisplayForGraveNumber, Area, ContractContent, NowDate, DefaultDate)
            DataConect.GravePanelRegistration(gpd)

            Dim godl As GravePanelDataListEntity = GravePanelDataListEntity.GetInstance
            godl.AddItem(gpd)

            CreateCompleteRegistrationInfo()

            DataClear()

        End Sub

        Public Property CallRegistrationErrorMessageInfo As Boolean
            Get
                Return _CallRegistrationErrorMessageInfo
            End Get
            Set
                _CallRegistrationErrorMessageInfo = Value
                CallPropertyChanged(NameOf(CallRegistrationErrorMessageInfo))
                _CallRegistrationErrorMessageInfo = False
            End Set
        End Property

        Public Property RegistrationErrorMessageInfo As ICommand
            Get
                _RegistrationErrorMessageInfo = New DelegateCommand(
                    Sub()
                        MessageInfo = New MessageBoxInfo With
                        {
                        .Message = My.Resources.StringEmptyMessage,
                        .Image = MessageBoxImage.Exclamation,
                        .Title = My.Resources.ErrorRegisterTitle
                        }
                        CallPropertyChanged(NameOf(RegistrationErrorMessageInfo))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
                Return _RegistrationErrorMessageInfo
            End Get
            Set
                _RegistrationErrorMessageInfo = Value
            End Set
        End Property

        ''' <summary>
        ''' 登録完了メッセージを生成します
        ''' </summary>
        Public Sub CreateCompleteRegistrationInfo()

            CompleteRegistrationInfo = New DelegateCommand(
                Sub()
                    MessageInfo = New MessageBoxInfo With
                    {
                    .Message = My.Resources.AddComplete, .Button = MessageBoxButton.OK, .Title = My.Resources.AddCompleteTitle, .Image = MessageBoxImage.Information
                    }
                    CallPropertyChanged(NameOf(CreateCompleteRegistrationInfo))
                End Sub,
                Function()
                    Return True
                End Function
                )

            CallCompleteRegistration = True

        End Sub

        ''' <summary>
        ''' プロパティの値をクリアします
        ''' </summary>
        Private Sub DataClear()

            KuText = String.Empty
            KuikiText = String.Empty
            GawaText = String.Empty
            BanText = String.Empty
            EdabanText = String.Empty
            RegistrationCustomerID = String.Empty
            FamilyName = String.Empty
            FullName = String.Empty
            ContractContent = String.Empty
            Area = 0

        End Sub

        ''' <summary>
        ''' 登録確認メッセージを生成します
        ''' </summary>
        Public Sub CreateConfirmationRegisterInfo()

            ConfirmationRegistraterInfo = New DelegateCommand(
                Sub()
                    MessageInfo = New MessageBoxInfo With
                    {
                    .Message = My.Resources.FieldPropertyMessage_CustomerID & RegistrationCustomerID & vbNewLine & My.Resources.FieldPropertyMessage_FirstName & FamilyName &
                    vbNewLine & My.Resources.FieldPropertyMessage_GraveNumber & DisplayForGraveNumber & vbNewLine & My.Resources.FieldPropertyMessage_ContractContent &
                    ContractContent & vbNewLine & My.Resources.FieldPropertyMessage_RegistrationDate & Today.ToString("yyyy年MM月dd日") & vbNewLine & vbNewLine &
                    My.Resources.ConfirmationRegisterInfo,
                    .Button = MessageBoxButton.YesNo,
                    .Title = My.Resources.ConfirmationRegisterInfoTitle, .Image = MessageBoxImage.Question
                    }
                    CallPropertyChanged(NameOf(ConfirmationRegistraterInfo))
                    MsgResult = MessageInfo.Result
                End Sub,
                Function()
                    Return True
                End Function
                )

        End Sub

        Protected Overrides Sub ValidateProperty(propertyName As String, value As Object)
            Select Case propertyName
                Case NameOf(KuText), NameOf(KuikiText), NameOf(GawaText), NameOf(BanText)
                    SetValiDateProperty_StringEmptyMessage(propertyName, value)
                Case NameOf(Area)
                    If Area = 0 Then
                        AddError(propertyName, My.Resources.AreaFieldError)
                    Else
                        RemoveError(propertyName)
                    End If
            End Select
        End Sub

        Private Sub SetValiDateProperty_StringEmptyMessage(ByVal propertyName As String, value As Object)

            If String.IsNullOrEmpty(value) Then
                AddError(propertyName, My.Resources.StringEmptyMessage)
            Else
                RemoveError(propertyName)
            End If
        End Sub

    End Class
End Namespace

