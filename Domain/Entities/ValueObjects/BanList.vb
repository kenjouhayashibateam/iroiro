﻿Imports System.Collections.ObjectModel

''' <summary>
''' 番リストクラス
''' </summary>
Public Class BanList

    Public Property List As ObservableCollection(Of Ban)

    Sub New(ByVal _list As ObservableCollection(Of Ban))
        List = _list
    End Sub
End Class