Imports System.Collections.ObjectModel

''' <summary>
''' 枝番リストクラス
''' </summary>
Public Class EdabanList

    Public Property List As ObservableCollection(Of Edaban)

    Public Sub New(_list As ObservableCollection(Of Edaban))
        List = _list
    End Sub
End Class
