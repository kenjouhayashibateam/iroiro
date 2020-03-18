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
                _OutputGravePanelCommand = New DelegateCommand(
                    Sub()
                        OutputDataConecter.GravePanelOutput(MyGravePanel.MyGraveNumber.Number, MyGravePanel.MyFamilyName.Name,
                                                            MyGravePanel.MyContractContent.Content, MyGravePanel.MyArea.MyArea, OutputPosition)
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

        Public Property IsPast3MonthsPart As Boolean
            Get
                Return _IsPast3MonthsPart
            End Get
            Set
                _IsPast3MonthsPart = Value
                CallPropertyChanged(NameOf(IsPast3MonthsPart))
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
            End Set
        End Property

        ''' <summary>
        ''' 墓地札データ削除
        ''' </summary>
        Public Sub DeleteGravePanelData()

            If HasErrors Then Exit Sub
            CreateDeletedItemInfo()
            DataBaseConecter.GravePanelDeletion(MyGravePanel)
            GravePanelList.List.Remove(MyGravePanel)

        End Sub

        ''' <summary>
        ''' 墓地札データ削除完了メッセージを生成します
        ''' </summary>
        Public Sub CreateDeletedItemInfo()

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
            End Select
        End Sub

        Public Sub Notify(gravepanelData As GravePanelDataEntity) Implements INotifyListAdd.Notify
            GravePanelList.AddItem(gravepanelData)
        End Sub
    End Class
End Namespace
