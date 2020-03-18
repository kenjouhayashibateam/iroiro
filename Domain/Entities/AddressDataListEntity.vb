Imports System.Collections.ObjectModel

Public Class AddressDataListEntity

    Public Property MyList As New ObservableCollection(Of AddressDataEntity)

    Public Sub AddItem(ByVal addressdata As AddressDataEntity)
        MyList.Add(addressdata)
    End Sub

    Sub New()
        MyList = New ObservableCollection(Of AddressDataEntity)
    End Sub

    Public Function GetList() As ObservableCollection(Of AddressDataEntity)
        Return MyList
    End Function

    Public Function GetCount() As Integer
        Return MyList.Count
    End Function

    Public Function GetItem(ByVal index As Integer) As AddressDataEntity
        Return MyList.Item(index)
    End Function

End Class
