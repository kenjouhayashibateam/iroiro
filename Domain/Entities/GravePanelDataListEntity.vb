Imports System.Collections.ObjectModel

''' <summary>
''' 墓地札リストクラス(シングルトン)
''' </summary>
Public Class GravePanelDataListEntity

    Private Shared _GravePanelDataList As GravePanelDataListEntity
    Public Property List As New ObservableCollection(Of GravePanelDataEntity)

    Sub New()
        List = New ObservableCollection(Of GravePanelDataEntity)
    End Sub

    Public Shared Function GetInstance() As GravePanelDataListEntity
        If _GravePanelDataList Is Nothing Then _GravePanelDataList = New GravePanelDataListEntity
        Return _GravePanelDataList
    End Function

    Public Sub AddItem(ByVal gravepaneldata As GravePanelDataEntity)
        List.Add(gravepaneldata)
    End Sub

End Class
