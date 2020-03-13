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
        Implements INotifyPropertyChanged, INotifyCollectionChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

        Private ReadOnly DataBaseConecter As IDataConectRepogitory
        Private ReadOnly OutputDataConecter As IOutputDataRepogitory

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

        Public Property OutputGravePanelCommand As ICommand
            Get
                If _OutputGravePanelCommand Is Nothing Then _OutputGravePanelCommand = New OutputGravePanelCommand(Me)
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(RegistrationTime)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(CallCompletedDeleteGravePanelDataInfo)))
                _CallConpletedDeleteGravePanelDataInfo = False
            End Set
        End Property

        Public Property MessageInfo As MessageBoxInfo
            Get
                Return _MessageInfo
            End Get
            Set
                _MessageInfo = Value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(MessageInfo)))
            End Set
        End Property

        Public Property DeleteGravePanelDataCommand As ICommand
            Get
                If _DeleteGravePanelDataCommand Is Nothing Then _DeleteGravePanelDataCommand = New DeleteGravePanelDataCommand(Me)
                Return _DeleteGravePanelDataCommand
            End Get
            Set
                _DeleteGravePanelDataCommand = Value
            End Set
        End Property

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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(OutputPosition)))
            End Set
        End Property

        Public Property IsPast3MonthsPart As Boolean
            Get
                Return _IsPast3MonthsPart
            End Get
            Set
                _IsPast3MonthsPart = Value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(IsPast3MonthsPart)))
            End Set
        End Property

        Sub New()
            Me.New(New SQLConectInfrastructure, New ExcelOutputInfrastructure)
        End Sub

        Sub New(ByVal _databaseconecter As IDataConectRepogitory, ByVal _outputdataconecter As IOutputDataRepogitory)
            DataBaseConecter = _databaseconecter
            OutputDataConecter = _outputdataconecter
            OutputPosition = 1
            GravePanelList = DataBaseConecter.GetGravePanelDataList
        End Sub

        ''' <summary>
        ''' 新規データ作成画面に遷移します
        ''' </summary>
        ''' <returns></returns>
        Public Property GotoCreateGravePanelDataView As ICommand
            Get
                If _GotoCreateGravePanelDataView Is Nothing Then _GotoCreateGravePanelDataView = New GotoCreateGravePanelDataViewCommand(Me)
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(MyGravePanel)))
            End Set
        End Property

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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(GravePanelList)))
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
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(_IsNewRecordOnly)))
            End Set
        End Property

        ''' <summary>
        ''' 新規墓地札登録画面を呼び出します。要コード検証　Behaviorsにクラスを作ってなんとかViewModel が直接呼び出すのではなくせないか
        ''' </summary>
        Public Sub ShowCreateGravePanelDataView()
            Dim cgodv As New CreateGravePanelDataView
            cgodv.ShowDialog()
        End Sub

        ''' <summary>
        ''' 墓地札データ削除
        ''' </summary>
        Public Sub DeleteGravePanelData()

            If MyGravePanel Is Nothing Then Exit Sub
            CreateDeletedItemInfo()
            DataBaseConecter.GravePanelDeletion(MyGravePanel)
            CallCompletedDeleteGravePanelDataInfo = True
            GravePanelList.List.Remove(MyGravePanel)

        End Sub

        ''' <summary>
        ''' 墓地札データ削除完了メッセージを生成します
        ''' </summary>
        Public Sub CreateDeletedItemInfo()

            If DeletedGravePanelInfo Is Nothing Then
                DeletedGravePanelInfo = New DelegateCommand(
                             Sub()
                                 MessageInfo = New MessageBoxInfo With
                                 {
                                 .Message = "管理番号 : " & MyGravePanel.GetCustomerID & vbNewLine & "苗字 : " & MyGravePanel.GetFamilyName & " 家" & vbNewLine &
                                                 "墓地番号 : " & MyGravePanel.GetGraveNumber & vbNewLine & vbNewLine & "削除しました。",
                                  .Button = MessageBoxButton.OK,
                                  .Title = "削除完了",
                                  .Image = MessageBoxImage.Information
                                  }
                                 RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(DeletedGravePanelInfo)))
                             End Sub,
                             Function()
                                 Return True
                             End Function
                             )
            End If

        End Sub

        Public Sub Output()
            OutputDataConecter.GravePanelOutput(MyGravePanel.MyGraveNumber.Number, MyGravePanel.MyFamilyName.Name, MyGravePanel.MyContractContent.Content, MyGravePanel.MyArea.MyArea, OutputPosition)
        End Sub

    End Class
End Namespace
