Imports System.Collections.ObjectModel

''' <summary>
''' 区域リストクラス
''' </summary>
Public Class KuikiList

    Public Property List As ObservableCollection(Of Kuiki)

    Public Sub New(_list As ObservableCollection(Of Kuiki))
        List = _list
    End Sub

End Class