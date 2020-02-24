Imports Domain

''' <summary>
''' Exitボタンを押されたときにリスナーに所定のデータを通知します
''' </summary>
Public Interface IExitButtonClickListener
    ''' <summary>
    ''' リスナーに通知する
    ''' </summary>
    ''' <param name="_postalCode">郵便番号</param>
    ''' <param name="_address">住所</param>
    Sub Notify(_postalCode As String, _address As String)
End Interface

''' <summary>
''' アドレスリストをセットします
''' </summary>
Interface ISetAddressList
    ''' <summary>
    ''' リストをセットします
    ''' </summary>
    ''' <param name="_addresslist"></param>
    Sub SetList(ByVal _addresslist As List(Of AddressDataEntity))
End Interface

''' <summary>
''' 住所の検索結果をリストビューに表示します
''' </summary>
Public Class AddressDataView
    Implements ISetAddressList

    Private ReadOnly vm As New WinFormAddressDataViewModel
    Private Listener As IExitButtonClickListener

    Sub New()

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        AddressResultDataGridView.DataBindings.Add("DataSource", vm, NameOf(vm.MyAddressList))
    End Sub

    ''' <summary>
    ''' リスナーを追加します
    ''' </summary>
    ''' <param name="_listener"></param>
    Public Sub AddListener(ByVal _listener As IExitButtonClickListener)
        Listener = _listener
    End Sub

    ''' <summary>
    ''' 渡された配列をリストビューに格納します。データバインドできたら削除
    ''' </summary>
    ''' <param name="addresslist">住所データの配列</param>
    Public Sub SetItem(ByVal addresslist(,) As String)

        Dim myitem As ListViewItem

        For I As Integer = 0 To UBound(addresslist)
            myitem = New ListViewItem With {.Text = addresslist(I, 0)}
            myitem.SubItems.Add(addresslist(I, 1))
            AddressResultListView.Items.Add(myitem)
        Next

    End Sub

    ''' <summary>
    ''' 住所のセットをvmに渡します
    ''' </summary>
    ''' <param name="_addresslist"></param>
    Public Sub SetList(_addresslist As List(Of AddressDataEntity)) Implements ISetAddressList.SetList

        vm.SetList(_addresslist)
        AddressResultDataGridView.DataSource = vm.MyAddressList

    End Sub

    Private Sub ExitButton_Click(sender As Object, e As EventArgs) Handles ExitButton.Click

        If AddressResultDataGridView.SelectedCells.Count = 0 Then Close()

        Dim postalcode As String = AddressResultDataGridView.SelectedRows(0).Cells(0).Value
        Dim address As String = AddressResultDataGridView.SelectedRows(0).Cells(1).Value

        Listener.Notify(postalcode, address)

        Close()

    End Sub

    Private Sub ComboBox1_DataSourceChanged(sender As Object, e As EventArgs) Handles ComboBox1.DataSourceChanged

    End Sub

    Private Sub AddressResultDataGridView_DataSourceChanged(sender As Object, e As EventArgs) Handles AddressResultDataGridView.DataSourceChanged


    End Sub
End Class