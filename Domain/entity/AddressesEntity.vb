Imports System.Collections.ObjectModel

Public Class AddressesEntity

    Private _List As New ObservableCollection(Of AddressDataEntity)

    Public Property List As ObservableCollection(Of AddressDataEntity)
        Get
            Return _List
        End Get
        Set
            _List = Value
        End Set
    End Property

    Public Sub AddItem(ByVal addressdata As AddressDataEntity)
        List.Add(addressdata)
    End Sub

End Class
