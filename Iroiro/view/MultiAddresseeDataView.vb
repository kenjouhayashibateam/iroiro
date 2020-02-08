''' <summary>
''' 複数印刷フォーム
''' </summary>
Public Class MultiAddresseeDataView

    Private vm As New MultiAddresseeDataViewModel

    Sub New()

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        AddresseeListView.DataBindings.Add("Add", vm, vm.ListItems)

    End Sub

    Private Sub AddListButton_Click(sender As Object, e As EventArgs) Handles AddListButton.Click

        Dim listitem As ListViewItem = vm.AddListItem(AddLesseeCustomerIDTextBox.Text)

        If listitem Is Nothing Then Exit Sub

        AddresseeListView.Items.Add(vm.AddListItem(AddLesseeCustomerIDTextBox.Text))
        AddLesseeCustomerIDTextBox.Text = String.Empty

    End Sub

    Private Sub DeleteItemButton_Click(sender As Object, e As EventArgs) Handles DeleteItemButton.Click

        If AddresseeListView.SelectedItems.Count = 0 Then Exit Sub

        AddresseeListView.SelectedItems(0).Remove()

    End Sub

    Private Sub BatchEntryAddresseeListButton_Click(sender As Object, e As EventArgs) Handles BatchEntryAddresseeListButton.Click

    End Sub

    Private Sub BatchEntryCustamerIDButton_Click(sender As Object, e As EventArgs) Handles BatchEntryCustamerIDButton.Click

        Dim mylist As List(Of ListViewItem) = vm.ReturnList_CustomerID

        For Each lvi As ListViewItem In mylist
            AddresseeListView.Items.Add(lvi)
        Next

    End Sub
End Class
