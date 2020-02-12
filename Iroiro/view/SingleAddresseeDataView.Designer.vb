<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SingleAddresseeDataView
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SingleAddresseeDataView))
        Me.NotNoteInputCheckBox = New System.Windows.Forms.CheckBox()
        Me.InfomationGroupBox = New System.Windows.Forms.GroupBox()
        Me.Note5TextBox = New System.Windows.Forms.TextBox()
        Me.Note4TextBox = New System.Windows.Forms.TextBox()
        Me.Note3TextBox = New System.Windows.Forms.TextBox()
        Me.Note2TextBox = New System.Windows.Forms.TextBox()
        Me.Note1TextBox = New System.Windows.Forms.TextBox()
        Me.MoneyTextBox = New System.Windows.Forms.TextBox()
        Me.LblNote5 = New System.Windows.Forms.Label()
        Me.LblNote4 = New System.Windows.Forms.Label()
        Me.LblNote3 = New System.Windows.Forms.Label()
        Me.LblNote2 = New System.Windows.Forms.Label()
        Me.LblNote1 = New System.Windows.Forms.Label()
        Me.LblMoney = New System.Windows.Forms.Label()
        Me.Address2TextBox = New System.Windows.Forms.TextBox()
        Me.Address1TextBox = New System.Windows.Forms.TextBox()
        Me.PostalCodeTextBox = New System.Windows.Forms.TextBox()
        Me.LblAdress2 = New System.Windows.Forms.Label()
        Me.LblAdress1 = New System.Windows.Forms.Label()
        Me.LblPostalCode = New System.Windows.Forms.Label()
        Me.AddresseeNameTextBox = New System.Windows.Forms.TextBox()
        Me.TitleTextBox = New System.Windows.Forms.TextBox()
        Me.LblName = New System.Windows.Forms.Label()
        Me.LesseeReferenceButton = New System.Windows.Forms.Button()
        Me.LabelPaperButton = New System.Windows.Forms.Button()
        Me.OutputTransferPaperButton = New System.Windows.Forms.Button()
        Me.CustomerIDTextBox = New System.Windows.Forms.TextBox()
        Me.LblMnagementNumber = New System.Windows.Forms.Label()
        Me.GotoMultiAddresseeDataViewButton = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.OutputPostcardButton = New System.Windows.Forms.Button()
        Me.OutputKaku2EnvelopeButton = New System.Windows.Forms.Button()
        Me.OutputGravePamphletEnvelopeButton = New System.Windows.Forms.Button()
        Me.OutputWesternEnvelopeButton = New System.Windows.Forms.Button()
        Me.OutputCho3EnvelopeButton = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel6 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel7 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel8 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel9 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel10 = New System.Windows.Forms.TableLayoutPanel()
        Me.MultiOutputCheckBox = New System.Windows.Forms.CheckBox()
        Me.InfomationGroupBox.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.TableLayoutPanel5.SuspendLayout()
        Me.TableLayoutPanel6.SuspendLayout()
        Me.TableLayoutPanel7.SuspendLayout()
        Me.TableLayoutPanel8.SuspendLayout()
        Me.TableLayoutPanel9.SuspendLayout()
        Me.TableLayoutPanel10.SuspendLayout()
        Me.SuspendLayout()
        '
        'NotNoteInputCheckBox
        '
        Me.NotNoteInputCheckBox.AutoSize = True
        Me.NotNoteInputCheckBox.Dock = System.Windows.Forms.DockStyle.Right
        Me.NotNoteInputCheckBox.Location = New System.Drawing.Point(166, 3)
        Me.NotNoteInputCheckBox.Name = "NotNoteInputCheckBox"
        Me.NotNoteInputCheckBox.Size = New System.Drawing.Size(141, 19)
        Me.NotNoteInputCheckBox.TabIndex = 2
        Me.NotNoteInputCheckBox.Text = "備考に使用者情報不要"
        Me.NotNoteInputCheckBox.UseVisualStyleBackColor = True
        '
        'InfomationGroupBox
        '
        Me.InfomationGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.InfomationGroupBox.Controls.Add(Me.TableLayoutPanel1)
        Me.InfomationGroupBox.Dock = System.Windows.Forms.DockStyle.Top
        Me.InfomationGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.InfomationGroupBox.Location = New System.Drawing.Point(0, 25)
        Me.InfomationGroupBox.Name = "InfomationGroupBox"
        Me.InfomationGroupBox.Size = New System.Drawing.Size(425, 192)
        Me.InfomationGroupBox.TabIndex = 1
        Me.InfomationGroupBox.TabStop = False
        Me.InfomationGroupBox.Text = "送付内容"
        '
        'Note5TextBox
        '
        Me.Note5TextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.Note5TextBox.Location = New System.Drawing.Point(3, 25)
        Me.Note5TextBox.MaxLength = 12
        Me.Note5TextBox.Name = "Note5TextBox"
        Me.Note5TextBox.Size = New System.Drawing.Size(134, 19)
        Me.Note5TextBox.TabIndex = 19
        Me.Note5TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Note4TextBox
        '
        Me.Note4TextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.Note4TextBox.Location = New System.Drawing.Point(3, 3)
        Me.Note4TextBox.MaxLength = 12
        Me.Note4TextBox.Name = "Note4TextBox"
        Me.Note4TextBox.Size = New System.Drawing.Size(134, 19)
        Me.Note4TextBox.TabIndex = 17
        Me.Note4TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Note3TextBox
        '
        Me.Note3TextBox.Dock = System.Windows.Forms.DockStyle.Left
        Me.Note3TextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.Note3TextBox.Location = New System.Drawing.Point(3, 25)
        Me.Note3TextBox.MaxLength = 12
        Me.Note3TextBox.Name = "Note3TextBox"
        Me.Note3TextBox.Size = New System.Drawing.Size(134, 19)
        Me.Note3TextBox.TabIndex = 15
        Me.Note3TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Note2TextBox
        '
        Me.Note2TextBox.Dock = System.Windows.Forms.DockStyle.Left
        Me.Note2TextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.Note2TextBox.Location = New System.Drawing.Point(3, 3)
        Me.Note2TextBox.MaxLength = 12
        Me.Note2TextBox.Name = "Note2TextBox"
        Me.Note2TextBox.Size = New System.Drawing.Size(134, 19)
        Me.Note2TextBox.TabIndex = 13
        Me.Note2TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Note1TextBox
        '
        Me.Note1TextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.Note1TextBox.Location = New System.Drawing.Point(3, 3)
        Me.Note1TextBox.MaxLength = 12
        Me.Note1TextBox.Name = "Note1TextBox"
        Me.Note1TextBox.Size = New System.Drawing.Size(134, 19)
        Me.Note1TextBox.TabIndex = 11
        Me.Note1TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'MoneyTextBox
        '
        Me.MoneyTextBox.Dock = System.Windows.Forms.DockStyle.Left
        Me.MoneyTextBox.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.MoneyTextBox.Location = New System.Drawing.Point(3, 3)
        Me.MoneyTextBox.MaxLength = 9
        Me.MoneyTextBox.Name = "MoneyTextBox"
        Me.MoneyTextBox.Size = New System.Drawing.Size(93, 19)
        Me.MoneyTextBox.TabIndex = 10
        Me.MoneyTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LblNote5
        '
        Me.LblNote5.AutoSize = True
        Me.LblNote5.Dock = System.Windows.Forms.DockStyle.Right
        Me.LblNote5.Location = New System.Drawing.Point(21, 23)
        Me.LblNote5.Name = "LblNote5"
        Me.LblNote5.Size = New System.Drawing.Size(35, 23)
        Me.LblNote5.TabIndex = 18
        Me.LblNote5.Text = "備考5"
        Me.LblNote5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LblNote4
        '
        Me.LblNote4.AutoSize = True
        Me.LblNote4.Dock = System.Windows.Forms.DockStyle.Right
        Me.LblNote4.Location = New System.Drawing.Point(21, 0)
        Me.LblNote4.Name = "LblNote4"
        Me.LblNote4.Size = New System.Drawing.Size(35, 23)
        Me.LblNote4.TabIndex = 16
        Me.LblNote4.Text = "備考4"
        Me.LblNote4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LblNote3
        '
        Me.LblNote3.AutoSize = True
        Me.LblNote3.Dock = System.Windows.Forms.DockStyle.Right
        Me.LblNote3.Location = New System.Drawing.Point(3, 22)
        Me.LblNote3.Name = "LblNote3"
        Me.LblNote3.Size = New System.Drawing.Size(35, 22)
        Me.LblNote3.TabIndex = 14
        Me.LblNote3.Text = "備考3"
        Me.LblNote3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LblNote2
        '
        Me.LblNote2.AutoSize = True
        Me.LblNote2.Dock = System.Windows.Forms.DockStyle.Right
        Me.LblNote2.Location = New System.Drawing.Point(3, 0)
        Me.LblNote2.Name = "LblNote2"
        Me.LblNote2.Size = New System.Drawing.Size(35, 22)
        Me.LblNote2.TabIndex = 12
        Me.LblNote2.Text = "備考2"
        Me.LblNote2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LblNote1
        '
        Me.LblNote1.AutoSize = True
        Me.LblNote1.Dock = System.Windows.Forms.DockStyle.Right
        Me.LblNote1.Location = New System.Drawing.Point(12, 0)
        Me.LblNote1.Name = "LblNote1"
        Me.LblNote1.Size = New System.Drawing.Size(35, 23)
        Me.LblNote1.TabIndex = 11
        Me.LblNote1.Text = "備考1"
        Me.LblNote1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LblMoney
        '
        Me.LblMoney.AutoSize = True
        Me.LblMoney.Dock = System.Windows.Forms.DockStyle.Right
        Me.LblMoney.Location = New System.Drawing.Point(214, 5)
        Me.LblMoney.Name = "LblMoney"
        Me.LblMoney.Size = New System.Drawing.Size(53, 33)
        Me.LblMoney.TabIndex = 8
        Me.LblMoney.Text = "払込金額"
        Me.LblMoney.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Address2TextBox
        '
        Me.Address2TextBox.Dock = System.Windows.Forms.DockStyle.Left
        Me.Address2TextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.Address2TextBox.Location = New System.Drawing.Point(62, 120)
        Me.Address2TextBox.Multiline = True
        Me.Address2TextBox.Name = "Address2TextBox"
        Me.Address2TextBox.Size = New System.Drawing.Size(140, 51)
        Me.Address2TextBox.TabIndex = 7
        '
        'Address1TextBox
        '
        Me.Address1TextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.Address1TextBox.Location = New System.Drawing.Point(62, 70)
        Me.Address1TextBox.Multiline = True
        Me.Address1TextBox.Name = "Address1TextBox"
        Me.Address1TextBox.Size = New System.Drawing.Size(140, 44)
        Me.Address1TextBox.TabIndex = 5
        '
        'PostalCodeTextBox
        '
        Me.PostalCodeTextBox.Dock = System.Windows.Forms.DockStyle.Left
        Me.PostalCodeTextBox.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.PostalCodeTextBox.Location = New System.Drawing.Point(62, 41)
        Me.PostalCodeTextBox.Name = "PostalCodeTextBox"
        Me.PostalCodeTextBox.Size = New System.Drawing.Size(76, 19)
        Me.PostalCodeTextBox.TabIndex = 3
        '
        'LblAdress2
        '
        Me.LblAdress2.AutoSize = True
        Me.LblAdress2.Dock = System.Windows.Forms.DockStyle.Top
        Me.LblAdress2.Location = New System.Drawing.Point(3, 117)
        Me.LblAdress2.Name = "LblAdress2"
        Me.LblAdress2.Size = New System.Drawing.Size(53, 12)
        Me.LblAdress2.TabIndex = 6
        Me.LblAdress2.Text = "番地"
        Me.LblAdress2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'LblAdress1
        '
        Me.LblAdress1.AutoSize = True
        Me.LblAdress1.Dock = System.Windows.Forms.DockStyle.Top
        Me.LblAdress1.Location = New System.Drawing.Point(3, 67)
        Me.LblAdress1.Name = "LblAdress1"
        Me.LblAdress1.Size = New System.Drawing.Size(53, 12)
        Me.LblAdress1.TabIndex = 4
        Me.LblAdress1.Text = "住所"
        Me.LblAdress1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LblPostalCode
        '
        Me.LblPostalCode.AutoSize = True
        Me.LblPostalCode.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LblPostalCode.Location = New System.Drawing.Point(3, 38)
        Me.LblPostalCode.Name = "LblPostalCode"
        Me.LblPostalCode.Size = New System.Drawing.Size(53, 29)
        Me.LblPostalCode.TabIndex = 2
        Me.LblPostalCode.Text = "郵便番号"
        Me.LblPostalCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AddresseeNameTextBox
        '
        Me.AddresseeNameTextBox.Dock = System.Windows.Forms.DockStyle.Left
        Me.AddresseeNameTextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.AddresseeNameTextBox.Location = New System.Drawing.Point(3, 3)
        Me.AddresseeNameTextBox.Name = "AddresseeNameTextBox"
        Me.AddresseeNameTextBox.Size = New System.Drawing.Size(90, 19)
        Me.AddresseeNameTextBox.TabIndex = 1
        '
        'TitleTextBox
        '
        Me.TitleTextBox.Dock = System.Windows.Forms.DockStyle.Left
        Me.TitleTextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.TitleTextBox.Location = New System.Drawing.Point(99, 3)
        Me.TitleTextBox.Name = "TitleTextBox"
        Me.TitleTextBox.Size = New System.Drawing.Size(38, 19)
        Me.TitleTextBox.TabIndex = 2
        '
        'LblName
        '
        Me.LblName.AutoSize = True
        Me.LblName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LblName.Location = New System.Drawing.Point(3, 5)
        Me.LblName.Name = "LblName"
        Me.LblName.Size = New System.Drawing.Size(53, 33)
        Me.LblName.TabIndex = 0
        Me.LblName.Text = "宛名"
        Me.LblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LesseeReferenceButton
        '
        Me.LesseeReferenceButton.Dock = System.Windows.Forms.DockStyle.Left
        Me.LesseeReferenceButton.Location = New System.Drawing.Point(313, 3)
        Me.LesseeReferenceButton.Name = "LesseeReferenceButton"
        Me.LesseeReferenceButton.Size = New System.Drawing.Size(75, 19)
        Me.LesseeReferenceButton.TabIndex = 3
        Me.LesseeReferenceButton.Text = "検索"
        Me.LesseeReferenceButton.UseVisualStyleBackColor = True
        '
        'LabelPaperButton
        '
        Me.LabelPaperButton.Location = New System.Drawing.Point(138, 3)
        Me.LabelPaperButton.Name = "LabelPaperButton"
        Me.LabelPaperButton.Size = New System.Drawing.Size(129, 53)
        Me.LabelPaperButton.TabIndex = 1
        Me.LabelPaperButton.Text = "ラベル用紙作成"
        Me.LabelPaperButton.UseVisualStyleBackColor = True
        '
        'OutputTransferPaperButton
        '
        Me.OutputTransferPaperButton.Location = New System.Drawing.Point(3, 3)
        Me.OutputTransferPaperButton.Name = "OutputTransferPaperButton"
        Me.OutputTransferPaperButton.Size = New System.Drawing.Size(129, 53)
        Me.OutputTransferPaperButton.TabIndex = 0
        Me.OutputTransferPaperButton.Text = "振込用紙"
        Me.OutputTransferPaperButton.UseVisualStyleBackColor = True
        '
        'CustomerIDTextBox
        '
        Me.CustomerIDTextBox.Dock = System.Windows.Forms.DockStyle.Left
        Me.CustomerIDTextBox.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.CustomerIDTextBox.Location = New System.Drawing.Point(67, 3)
        Me.CustomerIDTextBox.MaxLength = 6
        Me.CustomerIDTextBox.Name = "CustomerIDTextBox"
        Me.CustomerIDTextBox.Size = New System.Drawing.Size(87, 19)
        Me.CustomerIDTextBox.TabIndex = 1
        '
        'LblMnagementNumber
        '
        Me.LblMnagementNumber.AutoSize = True
        Me.LblMnagementNumber.Dock = System.Windows.Forms.DockStyle.Right
        Me.LblMnagementNumber.Location = New System.Drawing.Point(8, 0)
        Me.LblMnagementNumber.Name = "LblMnagementNumber"
        Me.LblMnagementNumber.Size = New System.Drawing.Size(53, 25)
        Me.LblMnagementNumber.TabIndex = 0
        Me.LblMnagementNumber.Text = "管理番号"
        Me.LblMnagementNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'GotoMultiAddresseeDataViewButton
        '
        Me.GotoMultiAddresseeDataViewButton.Dock = System.Windows.Forms.DockStyle.Right
        Me.GotoMultiAddresseeDataViewButton.Location = New System.Drawing.Point(336, 401)
        Me.GotoMultiAddresseeDataViewButton.Name = "GotoMultiAddresseeDataViewButton"
        Me.GotoMultiAddresseeDataViewButton.Size = New System.Drawing.Size(89, 41)
        Me.GotoMultiAddresseeDataViewButton.TabIndex = 4
        Me.GotoMultiAddresseeDataViewButton.Text = "一括出力画面"
        Me.GotoMultiAddresseeDataViewButton.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FlowLayoutPanel1.Controls.Add(Me.OutputCho3EnvelopeButton)
        Me.FlowLayoutPanel1.Controls.Add(Me.OutputKaku2EnvelopeButton)
        Me.FlowLayoutPanel1.Controls.Add(Me.OutputGravePamphletEnvelopeButton)
        Me.FlowLayoutPanel1.Controls.Add(Me.OutputPostcardButton)
        Me.FlowLayoutPanel1.Controls.Add(Me.OutputWesternEnvelopeButton)
        Me.FlowLayoutPanel1.Controls.Add(Me.MultiOutputCheckBox)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 217)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(425, 124)
        Me.FlowLayoutPanel1.TabIndex = 2
        '
        'OutputPostcardButton
        '
        Me.OutputPostcardButton.Location = New System.Drawing.Point(3, 62)
        Me.OutputPostcardButton.Name = "OutputPostcardButton"
        Me.OutputPostcardButton.Size = New System.Drawing.Size(131, 53)
        Me.OutputPostcardButton.TabIndex = 3
        Me.OutputPostcardButton.Text = "ハガキ"
        Me.OutputPostcardButton.UseVisualStyleBackColor = True
        '
        'OutputKaku2EnvelopeButton
        '
        Me.OutputKaku2EnvelopeButton.Location = New System.Drawing.Point(140, 3)
        Me.OutputKaku2EnvelopeButton.Name = "OutputKaku2EnvelopeButton"
        Me.OutputKaku2EnvelopeButton.Size = New System.Drawing.Size(129, 53)
        Me.OutputKaku2EnvelopeButton.TabIndex = 1
        Me.OutputKaku2EnvelopeButton.Text = "角二封筒"
        Me.OutputKaku2EnvelopeButton.UseVisualStyleBackColor = True
        '
        'OutputGravePamphletEnvelopeButton
        '
        Me.OutputGravePamphletEnvelopeButton.Location = New System.Drawing.Point(275, 3)
        Me.OutputGravePamphletEnvelopeButton.Name = "OutputGravePamphletEnvelopeButton"
        Me.OutputGravePamphletEnvelopeButton.Size = New System.Drawing.Size(129, 53)
        Me.OutputGravePamphletEnvelopeButton.TabIndex = 2
        Me.OutputGravePamphletEnvelopeButton.Text = "墓地パンフ封筒"
        Me.OutputGravePamphletEnvelopeButton.UseVisualStyleBackColor = True
        '
        'OutputWesternEnvelopeButton
        '
        Me.OutputWesternEnvelopeButton.Location = New System.Drawing.Point(140, 62)
        Me.OutputWesternEnvelopeButton.Name = "OutputWesternEnvelopeButton"
        Me.OutputWesternEnvelopeButton.Size = New System.Drawing.Size(129, 53)
        Me.OutputWesternEnvelopeButton.TabIndex = 4
        Me.OutputWesternEnvelopeButton.Text = "洋封筒"
        Me.OutputWesternEnvelopeButton.UseVisualStyleBackColor = True
        '
        'OutputCho3EnvelopeButton
        '
        Me.OutputCho3EnvelopeButton.Dock = System.Windows.Forms.DockStyle.Left
        Me.OutputCho3EnvelopeButton.Location = New System.Drawing.Point(3, 3)
        Me.OutputCho3EnvelopeButton.Name = "OutputCho3EnvelopeButton"
        Me.OutputCho3EnvelopeButton.Size = New System.Drawing.Size(131, 53)
        Me.OutputCho3EnvelopeButton.TabIndex = 0
        Me.OutputCho3EnvelopeButton.Text = "長３封筒"
        Me.OutputCho3EnvelopeButton.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FlowLayoutPanel2.Controls.Add(Me.OutputTransferPaperButton)
        Me.FlowLayoutPanel2.Controls.Add(Me.LabelPaperButton)
        Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(0, 341)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(425, 60)
        Me.FlowLayoutPanel2.TabIndex = 3
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 5
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel10, 4, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel8, 4, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel7, 3, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel6, 4, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel5, 4, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel4, 3, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel3, 3, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.PostalCodeTextBox, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.LblName, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.LblPostalCode, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.LblAdress1, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Address1TextBox, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Address2TextBox, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.LblAdress2, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.LblMoney, 3, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 15)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(419, 174)
        Me.TableLayoutPanel1.TabIndex = 10
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.84058!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.15942!))
        Me.TableLayoutPanel2.Controls.Add(Me.TitleTextBox, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.AddresseeNameTextBox, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(62, 8)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(140, 27)
        Me.TableLayoutPanel2.TabIndex = 1
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 1
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.LblNote2, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.LblNote3, 0, 1)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Right
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(226, 70)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 2
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(41, 44)
        Me.TableLayoutPanel3.TabIndex = 11
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 1
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.LblNote1, 0, 0)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Right
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(217, 41)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 1
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(50, 23)
        Me.TableLayoutPanel4.TabIndex = 10
        '
        'TableLayoutPanel5
        '
        Me.TableLayoutPanel5.ColumnCount = 1
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel5.Controls.Add(Me.MoneyTextBox, 0, 0)
        Me.TableLayoutPanel5.Location = New System.Drawing.Point(273, 8)
        Me.TableLayoutPanel5.Name = "TableLayoutPanel5"
        Me.TableLayoutPanel5.RowCount = 1
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel5.Size = New System.Drawing.Size(105, 27)
        Me.TableLayoutPanel5.TabIndex = 9
        '
        'TableLayoutPanel6
        '
        Me.TableLayoutPanel6.ColumnCount = 1
        Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel6.Controls.Add(Me.Note2TextBox, 0, 0)
        Me.TableLayoutPanel6.Controls.Add(Me.Note3TextBox, 0, 1)
        Me.TableLayoutPanel6.Location = New System.Drawing.Point(273, 70)
        Me.TableLayoutPanel6.Name = "TableLayoutPanel6"
        Me.TableLayoutPanel6.RowCount = 2
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.06383!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.93617!))
        Me.TableLayoutPanel6.Size = New System.Drawing.Size(140, 44)
        Me.TableLayoutPanel6.TabIndex = 12
        '
        'TableLayoutPanel7
        '
        Me.TableLayoutPanel7.ColumnCount = 1
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel7.Controls.Add(Me.LblNote4, 0, 0)
        Me.TableLayoutPanel7.Controls.Add(Me.LblNote5, 0, 1)
        Me.TableLayoutPanel7.Location = New System.Drawing.Point(208, 120)
        Me.TableLayoutPanel7.Name = "TableLayoutPanel7"
        Me.TableLayoutPanel7.RowCount = 2
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel7.Size = New System.Drawing.Size(59, 46)
        Me.TableLayoutPanel7.TabIndex = 13
        '
        'TableLayoutPanel8
        '
        Me.TableLayoutPanel8.ColumnCount = 1
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel8.Controls.Add(Me.Note5TextBox, 0, 1)
        Me.TableLayoutPanel8.Controls.Add(Me.Note4TextBox, 0, 0)
        Me.TableLayoutPanel8.Location = New System.Drawing.Point(273, 120)
        Me.TableLayoutPanel8.Name = "TableLayoutPanel8"
        Me.TableLayoutPanel8.RowCount = 2
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.0!))
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52.0!))
        Me.TableLayoutPanel8.Size = New System.Drawing.Size(140, 46)
        Me.TableLayoutPanel8.TabIndex = 14
        '
        'TableLayoutPanel9
        '
        Me.TableLayoutPanel9.ColumnCount = 4
        Me.TableLayoutPanel9.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64.0!))
        Me.TableLayoutPanel9.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 93.0!))
        Me.TableLayoutPanel9.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 153.0!))
        Me.TableLayoutPanel9.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8.0!))
        Me.TableLayoutPanel9.Controls.Add(Me.LesseeReferenceButton, 3, 0)
        Me.TableLayoutPanel9.Controls.Add(Me.NotNoteInputCheckBox, 2, 0)
        Me.TableLayoutPanel9.Controls.Add(Me.CustomerIDTextBox, 1, 0)
        Me.TableLayoutPanel9.Controls.Add(Me.LblMnagementNumber, 0, 0)
        Me.TableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel9.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel9.Name = "TableLayoutPanel9"
        Me.TableLayoutPanel9.RowCount = 1
        Me.TableLayoutPanel9.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel9.Size = New System.Drawing.Size(425, 25)
        Me.TableLayoutPanel9.TabIndex = 0
        '
        'TableLayoutPanel10
        '
        Me.TableLayoutPanel10.ColumnCount = 1
        Me.TableLayoutPanel10.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel10.Controls.Add(Me.Note1TextBox, 0, 0)
        Me.TableLayoutPanel10.Location = New System.Drawing.Point(273, 41)
        Me.TableLayoutPanel10.Name = "TableLayoutPanel10"
        Me.TableLayoutPanel10.RowCount = 1
        Me.TableLayoutPanel10.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel10.Size = New System.Drawing.Size(140, 23)
        Me.TableLayoutPanel10.TabIndex = 11
        '
        'MultiOutputCheckBox
        '
        Me.MultiOutputCheckBox.AutoSize = True
        Me.MultiOutputCheckBox.Dock = System.Windows.Forms.DockStyle.Right
        Me.MultiOutputCheckBox.Location = New System.Drawing.Point(275, 62)
        Me.MultiOutputCheckBox.Name = "MultiOutputCheckBox"
        Me.MultiOutputCheckBox.Size = New System.Drawing.Size(100, 53)
        Me.MultiOutputCheckBox.TabIndex = 5
        Me.MultiOutputCheckBox.Text = "複数データ出力"
        Me.MultiOutputCheckBox.UseVisualStyleBackColor = True
        '
        'SingleAddresseeDataView
        '
        Me.AcceptButton = Me.LesseeReferenceButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(425, 442)
        Me.Controls.Add(Me.GotoMultiAddresseeDataViewButton)
        Me.Controls.Add(Me.FlowLayoutPanel2)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Controls.Add(Me.InfomationGroupBox)
        Me.Controls.Add(Me.TableLayoutPanel9)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "SingleAddresseeDataView"
        Me.Text = "いろいろ発行"
        Me.InfomationGroupBox.ResumeLayout(False)
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        Me.TableLayoutPanel5.ResumeLayout(False)
        Me.TableLayoutPanel5.PerformLayout()
        Me.TableLayoutPanel6.ResumeLayout(False)
        Me.TableLayoutPanel6.PerformLayout()
        Me.TableLayoutPanel7.ResumeLayout(False)
        Me.TableLayoutPanel7.PerformLayout()
        Me.TableLayoutPanel8.ResumeLayout(False)
        Me.TableLayoutPanel8.PerformLayout()
        Me.TableLayoutPanel9.ResumeLayout(False)
        Me.TableLayoutPanel9.PerformLayout()
        Me.TableLayoutPanel10.ResumeLayout(False)
        Me.TableLayoutPanel10.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents NotNoteInputCheckBox As CheckBox
    Friend WithEvents InfomationGroupBox As GroupBox
    Friend WithEvents Note5TextBox As TextBox
    Friend WithEvents Note4TextBox As TextBox
    Friend WithEvents Note3TextBox As TextBox
    Friend WithEvents Note2TextBox As TextBox
    Friend WithEvents Note1TextBox As TextBox
    Friend WithEvents MoneyTextBox As TextBox
    Friend WithEvents LblNote5 As Label
    Friend WithEvents LblNote4 As Label
    Friend WithEvents LblNote3 As Label
    Friend WithEvents LblNote2 As Label
    Friend WithEvents LblNote1 As Label
    Friend WithEvents LblMoney As Label
    Friend WithEvents Address2TextBox As TextBox
    Friend WithEvents Address1TextBox As TextBox
    Friend WithEvents PostalCodeTextBox As TextBox
    Friend WithEvents LblAdress2 As Label
    Friend WithEvents LblAdress1 As Label
    Friend WithEvents LblPostalCode As Label
    Friend WithEvents AddresseeNameTextBox As TextBox
    Friend WithEvents TitleTextBox As TextBox
    Friend WithEvents LblName As Label
    Friend WithEvents LesseeReferenceButton As Button
    Friend WithEvents LabelPaperButton As Button
    Friend WithEvents OutputTransferPaperButton As Button
    Friend WithEvents CustomerIDTextBox As TextBox
    Friend WithEvents LblMnagementNumber As Label
    Friend WithEvents GotoMultiAddresseeDataViewButton As Button
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents OutputCho3EnvelopeButton As Button
    Friend WithEvents OutputWesternEnvelopeButton As Button
    Friend WithEvents OutputGravePamphletEnvelopeButton As Button
    Friend WithEvents OutputKaku2EnvelopeButton As Button
    Friend WithEvents OutputPostcardButton As Button
    Friend WithEvents FlowLayoutPanel2 As FlowLayoutPanel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel8 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel7 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel6 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel5 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel4 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel9 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel10 As TableLayoutPanel
    Friend WithEvents MultiOutputCheckBox As CheckBox
End Class
