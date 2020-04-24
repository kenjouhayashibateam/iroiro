Imports System.ComponentModel
Imports System.Collections.Specialized
Imports Domain
Imports Infrastructure
Imports WPF.Command
Imports System.Text.RegularExpressions
Imports WPF.Data

Namespace ViewModels
    ''' <summary>
    ''' 墓地札データリスト画面ViewModel 
    ''' </summary>
    Public Class GravePanelDataViewModel
        Inherits BaseViewModel
        Implements INotifyPropertyChanged, INotifyCollectionChanged, INotifyListAdd

        Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

        Private ReadOnly DataBaseConecter As IDataConectRepogitory
        Private ReadOnly OutputDataConecter As IOutputDataRepogitory

        ''' <summary>
        ''' 墓地データを削除したことをメッセージで知らせるコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property DeletedGravePanelInfo As DelegateCommand

        Private tre As Regex
        Private _MyGravePanel As GravePanelDataEntity
        Private _GravePanelList As GravePanelDataListEntity
        Private _GotoCreateGravePanelDataView As ICommand
        Private _IsPast3MonthsPart As Boolean
        Private _IsNewRecordOnly As Boolean = True
        Private _OutputPosition As String
        Private _DeleteGravePanelDataCommand As ICommand
        Private _MessageInfo As MessageBoxInfo
        Private _CallConpletedDeleteGravePanelDataInfo As Boolean
        Private _RegistrationTime As Date
        Private _OutputGravePanelCommand As ICommand
        Private _CustomerID As String
        Private _FamilyName As String
        Private _FullName As String
        Private _CallIsDeleteDataInfo As Boolean
        Private _CallOutputInfo As Boolean

        ''' <summary>
        ''' 契約内容のリスト
        ''' </summary>
        ''' <returns></returns>
        Public Property ContractContents As New ContractContents

        ''' <summary>
        ''' 申込者名
        ''' </summary>
        ''' <returns></returns>
        Public Property FullName As String
            Get
                Return _FullName
            End Get
            Set
                _FullName = Value
                CallPropertyChanged(FullName)
                GetList()
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
                GetList()
            End Set
        End Property

        ''' <summary>
        ''' 墓地札データOutputコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property OutputGravePanelCommand As ICommand
            Get
                _OutputGravePanelCommand = New DelegateCommand(
                    Sub()
                        Output()
                        CallPropertyChanged(NameOf(OutputGravePanelCommand))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
                Return _OutputGravePanelCommand
            End Get
            Set
                _OutputGravePanelCommand = Value
            End Set
        End Property

        ''' <summary>
        ''' 登録日時
        ''' </summary>
        ''' <returns></returns>
        Public Property RegistrationTime As Date
            Get
                Return _RegistrationTime
            End Get
            Set
                _RegistrationTime = Value
                CallPropertyChanged(NameOf(RegistrationTime))
            End Set
        End Property

        ''' <summary>
        ''' データ削除確認メッセージを表示させるBool
        ''' </summary>
        ''' <returns></returns>
        Public Property CallCompletedDeleteGravePanelDataInfo As Boolean
            Get
                Return _CallConpletedDeleteGravePanelDataInfo
            End Get
            Set
                _CallConpletedDeleteGravePanelDataInfo = Value
                CallPropertyChanged(NameOf(CallCompletedDeleteGravePanelDataInfo))
                _CallConpletedDeleteGravePanelDataInfo = False
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
        ''' 墓地札データを削除するコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property DeleteGravePanelDataCommand As ICommand
            Get
                _DeleteGravePanelDataCommand = New DelegateCommand(
                    Sub()
                        DeleteGravePanelData()
                        CallPropertyChanged(NameOf(DeleteGravePanelDataCommand))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
                Return _DeleteGravePanelDataCommand
            End Get
            Set
                _DeleteGravePanelDataCommand = Value
            End Set
        End Property

        ''' <summary>
        ''' 出力する位置を1～3までで設定します
        ''' </summary>
        ''' <returns></returns>
        Public Property OutputPosition As String
            Get
                Return _OutputPosition
            End Get
            Set
                tre = New Regex("[1-3]")
                Dim Ismatch As Boolean = False
                Ismatch = tre.IsMatch(Value)
                If Ismatch Then Ismatch = (Value.Length = 1)

                If Ismatch Then
                    _OutputPosition = Value
                Else
                    _OutputPosition = 1
                End If
                CallPropertyChanged(NameOf(OutputPosition))
            End Set
        End Property

        ''' <summary>
        ''' 過去3か月分のみ表示するかのチェック
        ''' </summary>
        ''' <returns></returns>
        Public Property IsPast3MonthsPart As Boolean
            Get
                Return _IsPast3MonthsPart
            End Get
            Set
                _IsPast3MonthsPart = Value
                CallPropertyChanged(NameOf(IsPast3MonthsPart))
                GetList()
            End Set
        End Property

        Sub New()
            Me.New(New SQLConnectInfrastructure, New ExcelOutputInfrastructure)
        End Sub

        Sub New(ByVal _databaseconecter As IDataConectRepogitory, ByVal _outputdataconecter As IOutputDataRepogitory)
            DataBaseConecter = _databaseconecter
            OutputDataConecter = _outputdataconecter
            OutputPosition = 1
            GravePanelList = GravePanelDataListEntity.GetInstance
            GetList()
        End Sub

        ''' <summary>
        ''' 新規データ作成画面に遷移します
        ''' </summary>
        ''' <returns></returns>
        Public Property GotoCreateGravePanelDataView As ICommand
            Get
                _GotoCreateGravePanelDataView = New DelegateCommand(
                    Sub()
                        CreateShowFormCommand(New CreateGravePanelDataView)
                        CallPropertyChanged(NameOf(GotoCreateGravePanelDataView))
                    End Sub,
                    Function()
                        Return True
                    End Function
                    )
                Return _GotoCreateGravePanelDataView
            End Get
            Set
                _GotoCreateGravePanelDataView = Value
            End Set
        End Property

        ''' <summary>
        ''' 墓地札クラス
        ''' </summary>
        ''' <returns></returns>
        Public Property MyGravePanel As GravePanelDataEntity
            Get
                Return _MyGravePanel
            End Get
            Set
                _MyGravePanel = Value
                CallPropertyChanged(NameOf(MyGravePanel))
            End Set
        End Property

        ''' <summary>
        ''' 印刷フラグに対する日付を管理します
        ''' </summary>
        Private Sub UpdateIsPrintoutValue()

            Dim dateCheck As Boolean = MyGravePanel.MyPrintOutTime.MyDate = My.Resources.DefaultDate

            If MyGravePanel.MyIsPrintout.Value = dateCheck Then Exit Sub

            If dateCheck = False Then
                MyGravePanel.MyPrintOutTime.MyDate = My.Resources.DefaultDate
                DataBaseConecter.GravePanelUpdate(MyGravePanel)
            End If

        End Sub

        ''' <summary>
        ''' 墓地札リストクラス
        ''' </summary>
        ''' <returns></returns>
        Public Property GravePanelList As GravePanelDataListEntity
            Get
                Return _GravePanelList
            End Get
            Set
                _GravePanelList = GravePanelDataListEntity.GetInstance
                CallPropertyChanged(NameOf(GravePanelList))
                RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NameOf(GravePanelList.List)))
            End Set
        End Property

        ''' <summary>
        ''' 新規墓地札のみリストに表示するかのチェック
        ''' </summary>
        ''' <returns></returns>
        Public Property IsNewRecordOnly As Boolean
            Get
                Return _IsNewRecordOnly
            End Get
            Set
                If _IsNewRecordOnly.Equals(Value) Then Return
                _IsNewRecordOnly = Value
                CallPropertyChanged(NameOf(IsNewRecordOnly))
                GetList()
            End Set
        End Property

        ''' <summary>
        ''' データベースから墓地札データを取得し、リストに格納します
        ''' </summary>
        Private Sub GetList()
            Dim refRetistrationDate_st, refOutputDate_en As Date
            If IsNewRecordOnly Then
                refOutputDate_en = My.Resources.DefaultDate
            Else
                refOutputDate_en = Now
            End If
            If IsPast3MonthsPart Then
                refRetistrationDate_st = DateAdd(DateInterval.Month, -3, Now)
            Else
                refRetistrationDate_st = My.Resources.DefaultDate
            End If

            GravePanelList.List = DataBaseConecter.GetGravePanelDataList(CustomerID, FullName, refRetistrationDate_st, Now, #1900/01/01#, refOutputDate_en).List

            CallPropertyChanged(NameOf(GravePanelList))

            RaiseEvent CollectionChanged(Me, New NotifyCollectionChangedEventArgs(NameOf(GravePanelList.List)))

        End Sub

        ''' <summary>
        ''' 墓地札データ削除
        ''' </summary>
        Public Sub DeleteGravePanelData()

            If HasErrors Then Exit Sub
            If MyGravePanel Is Nothing Then Exit Sub
            If CreateIsDeleteDataInfo_ReturnAnswer() = MsgBoxResult.Cancel Then Exit Sub
            DataBaseConecter.GravePanelDeletion(MyGravePanel)
            If Not GravePanelList.List.Remove(MyGravePanel) Then Exit Sub
            CreateDeletedItemInfo()

        End Sub

        ''' <summary>
        ''' 墓地札データ削除確認メッセージコマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property IsDeleteDataInfoCommand As DelegateCommand
        ''' <summary>
        ''' 墓地札データ削除確認コマンドを呼び出すタイミングを管理します
        ''' </summary>
        ''' <returns></returns>
        Public Property CallIsDeleteDataInfo As Boolean
            Get
                Return _CallIsDeleteDataInfo
            End Get
            Set
                _CallIsDeleteDataInfo = Value
                CallPropertyChanged(NameOf(CallIsDeleteDataInfo))
                _CallIsDeleteDataInfo = False
            End Set
        End Property
        ''' <summary>
        ''' 墓地札データ削除確認メッセージボックスを呼び出し、その結果を返します
        ''' </summary>
        ''' <returns></returns>
        Public Function CreateIsDeleteDataInfo_ReturnAnswer() As MsgBoxResult

            IsDeleteDataInfoCommand = New DelegateCommand(
                Sub()
                    MessageInfo = New MessageBoxInfo With
                    {
                    .Button = MessageBoxButton.OKCancel,
                    .Image = MessageBoxImage.Question,
                    .Message = $"{MyGravePanelDataDetailString()}{vbNewLine}{vbNewLine}{My.Resources.DeleteInfo}",
                    .Title = My.Resources.DeleteInfoTitle
                    }
                    CallPropertyChanged(NameOf(IsDeleteDataInfoCommand))
                End Sub,
                Function()
                    Return True
                End Function
                )

            CallIsDeleteDataInfo = True

            Return MessageInfo.Result
        End Function

        ''' <summary>
        ''' 墓地札データの項目を並べた文字列を返します
        ''' </summary>
        ''' <returns></returns>
        Private Function MyGravePanelDataDetailString() As String
            If MyGravePanel Is Nothing Then Return String.Empty
            Return $"{MyGravePanel.GetCustomerID.ShowDisplay}{vbNewLine}{MyGravePanel.GetFamilyName.ShowDisplay}{vbNewLine}{MyGravePanel.GetGraveNumber.Number}"
        End Function

        ''' <summary>
        ''' 墓地札データ削除完了メッセージを生成します
        ''' </summary>
        Public Sub CreateDeletedItemInfo()

            DeletedGravePanelInfo = New DelegateCommand(
                       Sub()
                           MessageInfo = New MessageBoxInfo With
                           {
                           .Message = $"{MyGravePanelDataDetailString()}{vbNewLine}{vbNewLine}{My.Resources.CompleteDeleteInfo}",
                            .Button = MessageBoxButton.OK,
                            .Title = My.Resources.CompleteDeleteInfoTitle,
                            .Image = MessageBoxImage.Information
                            }
                           CallPropertyChanged(NameOf(DeletedGravePanelInfo))
                       End Sub,
                       Function()
                           Return True
                       End Function
                       )

            CallCompletedDeleteGravePanelDataInfo = True

        End Sub

        Protected Overrides Sub ValidateProperty(propertyName As String, value As Object)

            Select Case propertyName
                Case NameOf(MyGravePanel)
                    If MyGravePanel Is Nothing Then
                        AddError(propertyName, My.Resources.NothingSelectedItemMessage)
                    Else
                        RemoveError(propertyName)
                    End If

                Case Else
                    Exit Select
            End Select

        End Sub

        Public Sub Notify(gravepanelData As GravePanelDataEntity) Implements INotifyListAdd.Notify
            GravePanelList.AddItem(gravepanelData)
        End Sub

        ''' <summary>
        ''' 墓地札データ出力コマンド
        ''' </summary>
        ''' <returns></returns>
        Public Property OutputInfoCommand As DelegateCommand

        ''' <summary>
        ''' リストの墓地札データを出力します
        ''' </summary>
        Public Sub Output()

            OutputDataConecter.GravePanelOutput(OutputPosition)

            Dim i As Integer = 0
            For Each gpd As GravePanelDataEntity In GravePanelList.List
                If gpd.MyIsPrintout.Value = False Then Continue For
                gpd.MyPrintOutTime.MyDate = Now
                DataBaseConecter.GravePanelUpdate(gpd)
                CallPropertyChanged(NameOf(GravePanelList))
                i += 1
            Next

            If i = 0 Then
                OutputInfo(My.Resources.OutputInfo_Count0, My.Resources.OutputInfoTitle, MessageBoxImage.Warning)
            Else
                OutputInfo(My.Resources.OutputInfo, My.Resources.OutputInfoTitle, MessageBoxImage.Information)
            End If

        End Sub

        ''' <summary>
        ''' 出力確認メッセージボックスを呼び出すタイミングを管理します
        ''' </summary>
        ''' <returns></returns>
        Public Property CallOutputInfo As Boolean
            Get
                Return _CallOutputInfo
            End Get
            Set
                _CallOutputInfo = Value
                CallPropertyChanged(NameOf(CallOutputInfo))
                _CallOutputInfo = False
            End Set
        End Property

        ''' <summary>
        ''' 出力確認メッセージボックスを生成します
        ''' </summary>
        ''' <param name="msg">メッセージ</param>
        ''' <param name="title">タイトル</param>
        ''' <param name="image">メッセージボックスの種類</param>
        Private Sub OutputInfo(ByVal msg As String, ByVal title As String, ByVal image As MessageBoxImage)

            OutputInfoCommand = New DelegateCommand(
                Sub()
                    MessageInfo = New MessageBoxInfo With
                    {
                    .Message = msg,
                    .Button = MessageBoxButton.OK,
                    .Title = title,
                    .Image = image
                    }
                End Sub,
                Function()
                    Return True
                End Function
                )
            CallOutputInfo = True
        End Sub

    End Class
End Namespace
