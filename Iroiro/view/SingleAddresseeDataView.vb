''' <summary>
''' 単票印刷フォーム
''' </summary>
Public Class SingleAddresseeDataView

    '''' <summary>
    '''' 単票印刷フォームのビューモデル
    '''' </summary>
    'Private ReadOnly vm As New WinFormSingleAddresseeDataViewModel

    'Sub New()

    '    ' この呼び出しはデザイナーで必要です。
    '    InitializeComponent()

    '    ' InitializeComponent() 呼び出しの後で初期化を追加します。
    '    AddresseeNameTextBox.DataBindings.Add("Text", vm, NameOf(vm.AddresseeName))
    '    PostalCodeTextBox.DataBindings.Add("Text", vm, NameOf(vm.PostalCode))
    '    Address1TextBox.DataBindings.Add("Text", vm, NameOf(vm.Address1))
    '    Address2TextBox.DataBindings.Add("Text", vm, NameOf(vm.Address2))
    '    Note1TextBox.DataBindings.Add("Text", vm, NameOf(vm.Note1))
    '    Note2TextBox.DataBindings.Add("Text", vm, NameOf(vm.Note2))
    '    Note3TextBox.DataBindings.Add("Text", vm, NameOf(vm.Note3))
    '    Note4TextBox.DataBindings.Add("Text", vm, NameOf(vm.Note4))
    '    Note5TextBox.DataBindings.Add("Text", vm, NameOf(vm.Note5))
    '    MoneyTextBox.DataBindings.Add("Text", vm, NameOf(vm.Money))
    '    TitleTextBox.DataBindings.Add("Text", vm, NameOf(vm.Title))
    '    NotNoteInputCheckBox.DataBindings.Add("Checked", vm, NameOf(vm.IsNoteInput))
    '    MultiOutputCheckBox.DataBindings.Add("Checked", vm, NameOf(vm.MultiOutputCheck))

    'End Sub

    'Private Sub LesseeReferenceButtom_Click(sender As Object, e As EventArgs) Handles LesseeReferenceButton.Click
    '    vm.ReferenceLessee(CustomerIDTextBox.Text)
    'End Sub

    'Private Sub OutputTransferPaperButton_Click(sender As Object, e As EventArgs) Handles OutputTransferPaperButton.Click
    '    vm.InputTransferData()
    'End Sub

    'Private Sub OutputCho3EnvelopeBotton_Click(sender As Object, e As EventArgs) Handles OutputCho3EnvelopeButton.Click
    '    vm.InputCho3Envelope()
    'End Sub

    'Private Sub PostalCodeTextBox_LostFocus(sender As Object, e As EventArgs) Handles PostalCodeTextBox.LostFocus
    '    vm.GetAddress(PostalCodeTextBox.Text)
    '    Address2TextBox.Focus()
    'End Sub

    'Private Sub WesternEnvelopeButton_Click(sender As Object, e As EventArgs) Handles OutputWesternEnvelopeButton.Click
    '    vm.InputWesternEnvelope()
    'End Sub

    'Private Sub OutputGravePamphletEnvelopeButton_Click(sender As Object, e As EventArgs) Handles OutputGravePamphletEnvelopeButton.Click
    '    vm.InputGravePamphletEnvelope()
    'End Sub

    'Private Sub OutputKaku2EnvelopeButton_Click(sender As Object, e As EventArgs) Handles OutputKaku2EnvelopeButton.Click
    '    vm.InputKaku2Envelope()
    'End Sub

    'Private Sub OutputPostcardButton_Click(sender As Object, e As EventArgs) Handles OutputPostcardButton.Click
    '    vm.InputPostcard()
    'End Sub

    'Private Sub LabelPaperButton_Click(sender As Object, e As EventArgs) Handles LabelPaperButton.Click
    '    vm.InputLabel()
    'End Sub

    'Private Sub Address1TextBox_LostFocus(sender As Object, e As EventArgs) Handles Address1TextBox.LostFocus
    '    vm.ReferenceAddress(Address1TextBox.Text)
    '    Address2TextBox.Focus()
    'End Sub

    'Private Sub GotoMultiAddresseeDataViewButton_Click(sender As Object, e As EventArgs) Handles GotoMultiAddresseeDataViewButton.Click

    '    MultiAddresseeDataView.ShowDialog()
    'End Sub

End Class