Imports System.Collections.ObjectModel

''' <summary>
''' 契約内容リスト
''' </summary>
Public Class ContractContents
    Public Property List As New ObservableCollection(Of String)

    Sub New()
        List.Add(My.Resources.Contract_Ueki)
        List.Add(My.Resources.Contract_Kusatori)
    End Sub
End Class