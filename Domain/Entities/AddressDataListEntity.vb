Imports System.Collections.ObjectModel

''' <summary>
''' AddressDataEntityをリストで保持します
''' </summary>
Public Class AddressDataListEntity

    Public Property MyList As New ObservableCollection(Of AddressDataEntity)

    Public Sub AddItem(addressdata As AddressDataEntity)
        MyList.Add(addressdata)
    End Sub

    Public Sub New()
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
