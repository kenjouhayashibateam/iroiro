Imports System.ComponentModel
Imports System.Collections.Specialized
Imports System.Collections.ObjectModel
Imports Domain
Imports Infrastructure
Imports WPF.Command
Imports WPF.Data

Namespace ViewModels
    ''' <summary>
    ''' 墓地札登録画面ViewModel 
    ''' </summary>
    Public Class CreateGravePanelDataViewModel
        Implements INotifyPropertyChanged, INotifyCollectionChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
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

        ''' <summary>
        ''' 墓地番号検索コマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property ReferenceGraveNumberCommand As ICommand
            Get
                If _ReferenceGraveNumberCommand Is Nothing Then _ReferenceGraveNumberCommand = New ReferenceGraveNumberPanelCommand(Me)
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(MessageInfo)))
            End Set
        End Property

        Private ReadOnly DataConect As IDataConectRepogitory
        Private _IsEnabledKuiki As Boolean = False
        Private _IsEnabledGawa As Boolean = False
        Private _IsEnabledBan As Boolean = False
        Private _IsEnabledEdaban As Boolean = False
        Private _SelectedKu As String = String.Empty
        Private _SelectedKuiki As String = String.Empty
        Private _SelectedGawa As String = String.Empty
        Private _SelectedBan As String = String.Empty
        Private _SelectedEdaban As String = String.Empty
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(RegistrationCustomerID)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(CallCompleteRegistration)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(IsConfirmationRegister)))
                _IsConfirmationRegister = False
            End Set
        End Property

        ''' <summary>
        ''' 墓地札登録コマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property GravePanelDataRegistration As ICommand
            Get
                If _GravePanelRegistration Is Nothing Then _GravePanelRegistration = New GravePanelDataRegistrationCommand(Me)
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ContractContent)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ContractContents)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(KuText)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(DisplayForGraveNumber)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(CallSelectAddresseeInfo)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Area)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(FamilyName)))
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

            If MyLessee.GetReceiverName.Length = 0 Then
                SetLesseeName()
            ElseIf MyLessee.GetLesseeName <> MyLessee.GetReceiverName Then
                CreateSelectAddresseeInfo()
                CallSelectAddresseeInfo = True
                SetName()
            Else
                SetLesseeName()
            End If

            RegistrationCustomerID = MyLessee.GetCustomerID
            Area = MyLessee.GetArea
        End Sub

        ''' <summary>
        ''' 苗字のみをセットします
        ''' </summary>
        Private Sub SetLesseeName()
            FamilyName = Mid(MyLessee.GetLesseeName, 1, InStr(MyLessee.GetLesseeName, "　") - 1)
        End Sub

        ''' <summary>
        ''' 苗字のみをセットします
        ''' </summary>
        Private Sub SetName()
            If MsgResult = MessageBoxResult.Yes Then
                SetLesseeName()
            Else
                FamilyName = Mid(MyLessee.GetReceiverName, 1, InStr(MyLessee.GetReceiverName, "　”) - 1)
            End If
        End Sub

        ''' <summary>
        ''' 名義人データと送付先のどちらを使用するかの確認メッセージボックスを生成します
        ''' </summary>
        Public Sub CreateSelectAddresseeInfo()

            SelectAddresseeInfo = New DelegateCommand(
            Sub()
                MessageInfo = New MessageBoxInfo With
                {
                .Message = "名義人 : " & MyLessee.GetLesseeName & vbNewLine & "送付先 : " & MyLessee.GetReceiverName & vbNewLine & vbNewLine &
                                "どちらのデータを使用しますか？" & vbNewLine & vbNewLine & "はい ⇒　名義人　　いいえ ⇒ 送付先",
                                .Button = MessageBoxButton.YesNo,
                               .Image = MessageBoxImage.Question,
                               .Title = "データ選択"
                               }
                MsgResult = MessageInfo.Result
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(SelectAddresseeInfo)))
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
            Return KuText & KuikiText & "区" & GawaText & "側" & BanText & EdabanText & "番"
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
                DisplayForGraveNumber = ReturnDisplayForGraveNumber()
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(EdabanText)))
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
                DisplayForGraveNumber = ReturnDisplayForGraveNumber()
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(BanText)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(GawaText)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(KuikiText)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(GraveNumberKuList)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(GraveNumberBanList)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(GraveNumberGawaList)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(GraveNumberKuikiList)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(GraveNumberEdabanList)))
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
                .Add("草取り")
                .Add("植木手入れ")
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(SelectedEdaban)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(SelectedBan)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(SelectedGawa)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(SelectedKuiki)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(SelectedKu)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(IsEnabledEdaban)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(IsEnabledBan)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(IsEnabledGawa)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(IsEnabledKuiki)))
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

            CreateConfirmationRegisterInfo()
            IsConfirmationRegister = True
            IsConfirmationRegister = False

            If MsgResult = MessageBoxResult.No Then Exit Sub
            Dim gpd As New GravePanelDataEntity(0, CustomerID, FamilyName, DisplayForGraveNumber, Area, ContractContent, Today, #1900/01/01#)
            DataConect.GravePanelRegistration(gpd)

            Dim godl As GravePanelDataListEntity = GravePanelDataListEntity.GetInstance
            godl.AddItem(gpd)

            CreateCompleteRegistrationInfo()
            CallCompleteRegistration = True

            DataClear()

        End Sub

        ''' <summary>
        ''' 登録完了メッセージを生成します
        ''' </summary>
        Public Sub CreateCompleteRegistrationInfo()

            CompleteRegistrationInfo = New DelegateCommand(
                Sub()
                    MessageInfo = New MessageBoxInfo With
                    {
                    .Message = "追加しました。", .Button = MessageBoxButton.OK, .Title = "処理完了", .Image = MessageBoxImage.Information
                    }
                    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(CompleteRegistrationInfo)))
                End Sub,
                Function()
                    Return True
                End Function
                )

        End Sub

        ''' <summary>
        ''' プロパティの値をクリアします
        ''' </summary>
        Private Sub DataClear()

            KuText = String.Empty
            CustomerID = String.Empty
            FamilyName = String.Empty
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
                    .Message = "管理番号 : " & CustomerID & vbNewLine & "苗字 : " & FamilyName & vbNewLine & "墓地番号 : " & DisplayForGraveNumber & vbNewLine & "契約内容 : " & ContractContent & vbNewLine & "登録日時 : " & Today.ToString("yyyy年MM月dd日") & vbNewLine & vbNewLine & "登録しますか？",
                    .Button = MessageBoxButton.YesNo, .Title = "登録確認", .Image = MessageBoxImage.Question
                    }
                    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ConfirmationRegistraterInfo)))
                    MsgResult = MessageInfo.Result
                End Sub,
                Function()
                    Return True
                End Function
                )

        End Sub

    End Class
End Namespace

