Imports Domain

Public Class AddressDataViewModel

    Public Sub SetList(_addresslist As List(Of AddressDataEntity))

        Dim mylist(_addresslist.Count, 2) As String
        Dim i As Integer = 0

        For Each ad As AddressDataEntity In _addresslist
            mylist(i, 0) = ad.GetPostalCode
            mylist(i, 1) = ad.GetAddress
            i += 1
        Next

        AddressDataView.SetItem(mylist)

    End Sub

End Class
