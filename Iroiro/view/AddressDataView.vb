Imports Domain

Public Interface IExitButtonClickListener
    Sub Notify(_postalCode As String, _address As String)
End Interface

''' <summary>
''' 住所の検索結果をリストビューに表示します
''' </summary>
Public Class AddressDataView

    Private ReadOnly vm As New AddressDataViewModel
    Private Listener As IExitButtonClickListener

    Public Sub AddListener(ByVal _listener As IExitButtonClickListener)
        Listener = _listener
    End Sub
    Public Sub SetItem(ByVal addresslist(,) As String)

        Dim myitem As ListViewItem

        For I As Integer = 0 To UBound(addresslist)
            myitem = New ListViewItem With {.Text = addresslist(I, 0)}
            myitem.SubItems.Add(addresslist(I, 1))
            AddressResultListView.Items.Add(myitem)
        Next

    End Sub

    Public Sub SetAddressList(ByVal _addresslist As List(Of AddressDataEntity))
        vm.SetList(_addresslist)
    End Sub

    Private Sub ExitButton_Click(sender As Object, e As EventArgs) Handles ExitButton.Click

        If AddressResultListView.SelectedItems.Count = 0 Then
            Listener.Notify("", "")
            Exit Sub
        End If

        Dim postalcode As String = AddressResultListView.SelectedItems(0).Text
        Dim address As String = AddressResultListView.SelectedItems(0).SubItems(1).Text

        Listener.Notify(postalcode, address)

        Close()

    End Sub

End Class