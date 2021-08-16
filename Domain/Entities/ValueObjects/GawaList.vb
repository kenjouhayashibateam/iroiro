Imports System.Collections.ObjectModel

''' <summary>
''' 側リストクラス
''' </summary>
Public Class GawaList

    Public Property List As ObservableCollection(Of Gawa)

    Public Sub New(_list As ObservableCollection(Of Gawa))
        List = _list
    End Sub
End Class
