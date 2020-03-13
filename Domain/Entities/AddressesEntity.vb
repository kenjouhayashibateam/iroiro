Imports System.Collections.ObjectModel

Public Class AddressesEntity

    Public Property List As New ObservableCollection(Of AddressDataEntity)

    ''' <summary>
    ''' 住所データをリストに格納します
    ''' </summary>
    ''' <param name="addressdata"></param>
    Public Sub AddItem(ByVal addressdata As AddressDataEntity)
        List.Add(addressdata)
    End Sub

End Class
