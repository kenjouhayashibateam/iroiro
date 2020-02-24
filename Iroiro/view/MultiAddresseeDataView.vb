Imports System.Collections.ObjectModel

''' <summary>
''' 複数印刷フォーム
''' </summary>
Public Class MultiAddresseeDataView

    Private ReadOnly vm As New WinFormMultiAddresseeDataViewModel

    Sub New()

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        TitleTextBox.DataBindings.Add("Text", vm, NameOf(vm.Title))
    End Sub

    Private Sub AddListButton_Click(sender As Object, e As EventArgs) Handles AddListButton.Click

        Dim listitem As ListViewItem = vm.AddListItem(AddLesseeCustomerIDTextBox.Text)

        If listitem Is Nothing Then Exit Sub

        AddresseeListView.Items.Add(listitem)
        AddLesseeCustomerIDTextBox.Text = String.Empty

    End Sub

    Private Sub DeleteItemButton_Click(sender As Object, e As EventArgs) Handles DeleteItemButton.Click

        If AddresseeListView.SelectedItems.Count = 0 Then Exit Sub

        AddresseeListView.SelectedItems(0).Remove()

    End Sub

    Private Sub BatchEntryAddresseeListButton_Click(sender As Object, e As EventArgs) Handles BatchEntryAddresseeListButton.Click

        Dim mylist As List(Of ListViewItem) = vm.ReturnList

        For Each lvi As ListViewItem In mylist
            AddresseeListView.Items.Add(lvi)
        Next

    End Sub

    Private Sub BatchEntryCustamerIDButton_Click(sender As Object, e As EventArgs) Handles BatchEntryCustamerIDButton.Click

        Dim mylist As List(Of ListViewItem) = vm.ReturnList_CustomerID

        For Each lvi As ListViewItem In mylist
            AddresseeListView.Items.Add(lvi)
        Next

    End Sub

    Private Sub Cho3EnvelopeButton_Click(sender As Object, e As EventArgs) Handles Cho3EnvelopeButton.Click
        vm.OutputList_Cho3Envelope(AddresseeListView.Items)
    End Sub

    Private Sub GravePamphletEnvelopeButton_Click(sender As Object, e As EventArgs) Handles GravePamphletEnvelopeButton.Click
        vm.OutputList_GravePamphletEnvelope(AddresseeListView.Items)
    End Sub

    Private Sub KakuniEnvelopeButton_Click(sender As Object, e As EventArgs) Handles KakuniEnvelopeButton.Click
        vm.OutputList_Kaku2Envelope(AddresseeListView.Items)
    End Sub

    Private Sub PostcardButton_Click(sender As Object, e As EventArgs) Handles PostcardButton.Click
        vm.OutputList_Postcard(AddresseeListView.Items)
    End Sub

    Private Sub WesternEnvelopeButton_Click(sender As Object, e As EventArgs) Handles WesternEnvelopeButton.Click
        vm.OutputList_WesternEnvelope(AddresseeListView.Items)
    End Sub

    Private Sub LabelButton_Click(sender As Object, e As EventArgs) Handles LabelButton.Click
        vm.OutputList_LabelSheet(AddresseeListView.Items)
    End Sub

End Class
