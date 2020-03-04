Imports System.ComponentModel
Imports System.Collections.Specialized
Imports System.Collections.ObjectModel
Imports Domain
Imports Infrastructure

Public Class CreateGravePanelDataViewModel
    Implements INotifyPropertyChanged, INotifyCollectionChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Public Event CollectionChanged As NotifyCollectionChangedEventHandler Implements INotifyCollectionChanged.CollectionChanged

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
    Private _KuikiComboBoxText As String
    Private _GraveNumberGawaList As GraveNumberEntity.GawaList
    Private _GawaComboBoxText As String
    Private _GraveNumberKuList As ObservableCollection(Of ReasonField)
    Private _BanComboBoxText As String
    Private _GraveNumberBanList As GraveNumberEntity.BanList
    Private _EdabanComboBoxText As String
    Private _GraveNumberEdabanList As GraveNumberEntity.EdabanList
    Private _CustomerID As String
    Private _FamilyName As String
    Private _Area As Double

    Public Property Area As Double
        Get
            Return _Area
        End Get
        Set
            _Area = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Area)))
        End Set
    End Property

    Public Property FamilyName As String
        Get
            Return _FamilyName
        End Get
        Set
            _FamilyName = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(FamilyName)))
        End Set
    End Property

    Public Property CustomerID As String
        Get
            Return _CustomerID
        End Get
        Set
            _CustomerID = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(CustomerID)))
        End Set
    End Property

    Private Sub GetCustomerID()

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

        Dim lessee As LesseeCustomerInfoEntity = DataConect.GetCustomerInfo_GraveNumber(SelectedKu, SelectedKuiki, SelectedGawa, SelectedBan, edabanstring)
        InputLesseeData(lessee)

    End Sub

    Private Sub InputLesseeData(ByVal lessee As LesseeCustomerInfoEntity)
        CustomerID = lessee.GetCustomerID
        FamilyName = Mid(lessee.GetLesseeName, 1, InStr(lessee.GetLesseeName, "　") - 1)
        Area = lessee.GetArea
    End Sub

    Public Property EdabanComboBoxText As String
        Get
            Return _EdabanComboBoxText
        End Get
        Set
            _EdabanComboBoxText = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(EdabanComboBoxText)))
        End Set
    End Property

    Public Property BanComboBoxText As String
        Get
            Return _BanComboBoxText
        End Get
        Set
            _BanComboBoxText = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(BanComboBoxText)))
        End Set
    End Property

    Public Property GawaComboBoxText As String
        Get
            Return _GawaComboBoxText
        End Get
        Set
            _GawaComboBoxText = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(GawaComboBoxText)))
        End Set
    End Property

    Public Property KuikiComboBoxText As String
        Get
            Return _KuikiComboBoxText
        End Get
        Set
            If Value = String.Empty Then
                SelectedGawa = String.Empty
                IsEnabledGawa = False
            Else
                IsEnabledGawa = True
            End If
            _KuikiComboBoxText = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(KuikiComboBoxText)))
        End Set
    End Property


    Public Property GraveNumberKuList As ObservableCollection(Of ReasonField)
        Get
            Return _GraveNumberKuList
        End Get
        Set
            _GraveNumberKuList = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(GraveNumberKuList)))
        End Set
    End Property

    Public Class ReasonField

        Public Property DisplayForValue As String
        Public Property OriginalValue As String

        Sub New(ByVal _displayforvalue As String, ByVal _originalvalue As String)
            DisplayForValue = _displayforvalue
            OriginalValue = _originalvalue
        End Sub
    End Class

    Public Class GraveNumberField

        Public Property Value As String

        Sub New(ByVal _value As String)
            Value = _value
        End Sub
    End Class

    Public Property GraveNumberBanList As GraveNumberEntity.BanList
        Get
            Return _GraveNumberBanList
        End Get
        Set
            _GraveNumberBanList = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(GraveNumberBanList)))
        End Set
    End Property

    Public Property GraveNumberGawaList As GraveNumberEntity.GawaList
        Get
            Return _GraveNumberGawaList
        End Get
        Set
            _GraveNumberGawaList = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(GraveNumberGawaList)))
        End Set
    End Property

    Public Property GraveNumberKuikiList As GraveNumberEntity.KuikiList
        Get
            Return _GraveNumberKuikiList
        End Get
        Set
            _GraveNumberKuikiList = Value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(GraveNumberKuikiList)))
        End Set
    End Property

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

        GraveNumberKuList = New ObservableCollection(Of ReasonField)

        AddGraveKu("東", "01")
        AddGraveKu("西", "02")
        AddGraveKu("南", "03")
        AddGraveKu("北", "04")
        AddGraveKu("中", "05")
        AddGraveKu("東特", "10")
        AddGraveKu("二特", "11")
        AddGraveKu("北特", "12")
        AddGraveKu("御廟", "20")

    End Sub

    Private Sub AddGraveKu(ByVal displayforvalue As String, ByVal originalvalue As String)
        Dim kustring As New ReasonField(displayforvalue, originalvalue)
        GraveNumberKuList.Add(kustring)
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
            GetCustomerID()
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
                SetNextGraveNumberField(GravenumberGanre.edaban, Value)
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
                SetNextGraveNumberField(GravenumberGanre.ban, Value)
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
                SetNextGraveNumberField(GravenumberGanre.gawa, Value)
            End If
        End Set
    End Property

    Private Enum GravenumberGanre
        ku
        kuiki
        gawa
        ban
        edaban
    End Enum

    Private Sub SetNextGraveNumberField(ByVal nextganre As GravenumberGanre, ByVal numbervalue As String)

        Select Case nextganre
            Case GravenumberGanre.kuiki
                GraveNumberKuikiList = DataConect.GetKuikiList(numbervalue)
                IsEnabledKuiki = True
            Case GravenumberGanre.gawa
                GraveNumberGawaList = DataConect.GetGawaList(SelectedKu, numbervalue)
                IsEnabledGawa = True
            Case GravenumberGanre.ban
                GraveNumberBanList = DataConect.GetBanList(SelectedKu, SelectedKuiki, numbervalue)
                IsEnabledBan = True
            Case GravenumberGanre.edaban
                GraveNumberEdabanList = DataConect.GetEdabanList(SelectedKu, SelectedKuiki, SelectedGawa, numbervalue)
                If GraveNumberEdabanList Is Nothing Then
                    GetCustomerID()
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
                SetNextGraveNumberField(GravenumberGanre.kuiki, Value)
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

End Class
