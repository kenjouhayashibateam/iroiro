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
        Me.GboInfo = New System.Windows.Forms.GroupBox()
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
        Me.ExcelOutputMenu = New System.Windows.Forms.GroupBox()
        Me.OutputWesternEnvelopeButton = New System.Windows.Forms.Button()
        Me.OutputPostcardButton = New System.Windows.Forms.Button()
        Me.LabelPaperButton = New System.Windows.Forms.Button()
        Me.OutputGravePamphletEnvelopeButton = New System.Windows.Forms.Button()
        Me.OutputCho3EnvelopeButton = New System.Windows.Forms.Button()
        Me.OutputTransferPaperButton = New System.Windows.Forms.Button()
        Me.OutputKaku2EnvelopeButton = New System.Windows.Forms.Button()
        Me.CustomerIDTextBox = New System.Windows.Forms.TextBox()
        Me.LblMnagementNumber = New System.Windows.Forms.Label()
        Me.GotoMultiAddresseeDataViewButton = New System.Windows.Forms.Button()
        Me.GboInfo.SuspendLayout()
        Me.ExcelOutputMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'NotNoteInputCheckBox
        '
        Me.NotNoteInputCheckBox.AutoSize = True
        Me.NotNoteInputCheckBox.Location = New System.Drawing.Point(175, 14)
        Me.NotNoteInputCheckBox.Name = "NotNoteInputCheckBox"
        Me.NotNoteInputCheckBox.Size = New System.Drawing.Size(141, 16)
        Me.NotNoteInputCheckBox.TabIndex = 2
        Me.NotNoteInputCheckBox.Text = "備考に使用者情報不要"
        Me.NotNoteInputCheckBox.UseVisualStyleBackColor = True
        '
        'GboInfo
        '
        Me.GboInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GboInfo.Controls.Add(Me.Note5TextBox)
        Me.GboInfo.Controls.Add(Me.Note4TextBox)
        Me.GboInfo.Controls.Add(Me.Note3TextBox)
        Me.GboInfo.Controls.Add(Me.Note2TextBox)
        Me.GboInfo.Controls.Add(Me.Note1TextBox)
        Me.GboInfo.Controls.Add(Me.MoneyTextBox)
        Me.GboInfo.Controls.Add(Me.LblNote5)
        Me.GboInfo.Controls.Add(Me.LblNote4)
        Me.GboInfo.Controls.Add(Me.LblNote3)
        Me.GboInfo.Controls.Add(Me.LblNote2)
        Me.GboInfo.Controls.Add(Me.LblNote1)
        Me.GboInfo.Controls.Add(Me.LblMoney)
        Me.GboInfo.Controls.Add(Me.Address2TextBox)
        Me.GboInfo.Controls.Add(Me.Address1TextBox)
        Me.GboInfo.Controls.Add(Me.PostalCodeTextBox)
        Me.GboInfo.Controls.Add(Me.LblAdress2)
        Me.GboInfo.Controls.Add(Me.LblAdress1)
        Me.GboInfo.Controls.Add(Me.LblPostalCode)
        Me.GboInfo.Controls.Add(Me.AddresseeNameTextBox)
        Me.GboInfo.Controls.Add(Me.TitleTextBox)
        Me.GboInfo.Controls.Add(Me.LblName)
        Me.GboInfo.Location = New System.Drawing.Point(12, 41)
        Me.GboInfo.Name = "GboInfo"
        Me.GboInfo.Size = New System.Drawing.Size(413, 168)
        Me.GboInfo.TabIndex = 4
        Me.GboInfo.TabStop = False
        Me.GboInfo.Text = "送付内容"
        '
        'Note5TextBox
        '
        Me.Note5TextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.Note5TextBox.Location = New System.Drawing.Point(252, 143)
        Me.Note5TextBox.MaxLength = 12
        Me.Note5TextBox.Name = "Note5TextBox"
        Me.Note5TextBox.Size = New System.Drawing.Size(155, 19)
        Me.Note5TextBox.TabIndex = 20
        '
        'Note4TextBox
        '
        Me.Note4TextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.Note4TextBox.Location = New System.Drawing.Point(252, 118)
        Me.Note4TextBox.MaxLength = 12
        Me.Note4TextBox.Name = "Note4TextBox"
        Me.Note4TextBox.Size = New System.Drawing.Size(155, 19)
        Me.Note4TextBox.TabIndex = 18
        '
        'Note3TextBox
        '
        Me.Note3TextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.Note3TextBox.Location = New System.Drawing.Point(252, 93)
        Me.Note3TextBox.MaxLength = 12
        Me.Note3TextBox.Name = "Note3TextBox"
        Me.Note3TextBox.Size = New System.Drawing.Size(155, 19)
        Me.Note3TextBox.TabIndex = 16
        '
        'Note2TextBox
        '
        Me.Note2TextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.Note2TextBox.Location = New System.Drawing.Point(252, 68)
        Me.Note2TextBox.MaxLength = 12
        Me.Note2TextBox.Name = "Note2TextBox"
        Me.Note2TextBox.Size = New System.Drawing.Size(155, 19)
        Me.Note2TextBox.TabIndex = 14
        '
        'Note1TextBox
        '
        Me.Note1TextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.Note1TextBox.Location = New System.Drawing.Point(252, 43)
        Me.Note1TextBox.MaxLength = 12
        Me.Note1TextBox.Name = "Note1TextBox"
        Me.Note1TextBox.Size = New System.Drawing.Size(155, 19)
        Me.Note1TextBox.TabIndex = 12
        '
        'MoneyTextBox
        '
        Me.MoneyTextBox.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.MoneyTextBox.Location = New System.Drawing.Point(270, 18)
        Me.MoneyTextBox.MaxLength = 9
        Me.MoneyTextBox.Name = "MoneyTextBox"
        Me.MoneyTextBox.Size = New System.Drawing.Size(137, 19)
        Me.MoneyTextBox.TabIndex = 10
        '
        'LblNote5
        '
        Me.LblNote5.AutoSize = True
        Me.LblNote5.Location = New System.Drawing.Point(211, 146)
        Me.LblNote5.Name = "LblNote5"
        Me.LblNote5.Size = New System.Drawing.Size(35, 12)
        Me.LblNote5.TabIndex = 19
        Me.LblNote5.Text = "備考5"
        '
        'LblNote4
        '
        Me.LblNote4.AutoSize = True
        Me.LblNote4.Location = New System.Drawing.Point(210, 121)
        Me.LblNote4.Name = "LblNote4"
        Me.LblNote4.Size = New System.Drawing.Size(35, 12)
        Me.LblNote4.TabIndex = 17
        Me.LblNote4.Text = "備考4"
        '
        'LblNote3
        '
        Me.LblNote3.AutoSize = True
        Me.LblNote3.Location = New System.Drawing.Point(211, 96)
        Me.LblNote3.Name = "LblNote3"
        Me.LblNote3.Size = New System.Drawing.Size(35, 12)
        Me.LblNote3.TabIndex = 15
        Me.LblNote3.Text = "備考3"
        '
        'LblNote2
        '
        Me.LblNote2.AutoSize = True
        Me.LblNote2.Location = New System.Drawing.Point(211, 71)
        Me.LblNote2.Name = "LblNote2"
        Me.LblNote2.Size = New System.Drawing.Size(35, 12)
        Me.LblNote2.TabIndex = 13
        Me.LblNote2.Text = "備考2"
        '
        'LblNote1
        '
        Me.LblNote1.AutoSize = True
        Me.LblNote1.Location = New System.Drawing.Point(211, 46)
        Me.LblNote1.Name = "LblNote1"
        Me.LblNote1.Size = New System.Drawing.Size(35, 12)
        Me.LblNote1.TabIndex = 11
        Me.LblNote1.Text = "備考1"
        '
        'LblMoney
        '
        Me.LblMoney.AutoSize = True
        Me.LblMoney.Location = New System.Drawing.Point(211, 21)
        Me.LblMoney.Name = "LblMoney"
        Me.LblMoney.Size = New System.Drawing.Size(53, 12)
        Me.LblMoney.TabIndex = 9
        Me.LblMoney.Text = "払込金額"
        '
        'Address2TextBox
        '
        Me.Address2TextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.Address2TextBox.Location = New System.Drawing.Point(65, 118)
        Me.Address2TextBox.Multiline = True
        Me.Address2TextBox.Name = "Address2TextBox"
        Me.Address2TextBox.Size = New System.Drawing.Size(140, 44)
        Me.Address2TextBox.TabIndex = 8
        '
        'Address1TextBox
        '
        Me.Address1TextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.Address1TextBox.Location = New System.Drawing.Point(65, 68)
        Me.Address1TextBox.Multiline = True
        Me.Address1TextBox.Name = "Address1TextBox"
        Me.Address1TextBox.Size = New System.Drawing.Size(140, 44)
        Me.Address1TextBox.TabIndex = 6
        '
        'PostalCodeTextBox
        '
        Me.PostalCodeTextBox.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.PostalCodeTextBox.Location = New System.Drawing.Point(65, 43)
        Me.PostalCodeTextBox.Name = "PostalCodeTextBox"
        Me.PostalCodeTextBox.Size = New System.Drawing.Size(96, 19)
        Me.PostalCodeTextBox.TabIndex = 4
        '
        'LblAdress2
        '
        Me.LblAdress2.AutoSize = True
        Me.LblAdress2.Location = New System.Drawing.Point(30, 118)
        Me.LblAdress2.Name = "LblAdress2"
        Me.LblAdress2.Size = New System.Drawing.Size(29, 12)
        Me.LblAdress2.TabIndex = 7
        Me.LblAdress2.Text = "番地"
        '
        'LblAdress1
        '
        Me.LblAdress1.AutoSize = True
        Me.LblAdress1.Location = New System.Drawing.Point(30, 68)
        Me.LblAdress1.Name = "LblAdress1"
        Me.LblAdress1.Size = New System.Drawing.Size(29, 12)
        Me.LblAdress1.TabIndex = 5
        Me.LblAdress1.Text = "住所"
        '
        'LblPostalCode
        '
        Me.LblPostalCode.AutoSize = True
        Me.LblPostalCode.Location = New System.Drawing.Point(6, 46)
        Me.LblPostalCode.Name = "LblPostalCode"
        Me.LblPostalCode.Size = New System.Drawing.Size(53, 12)
        Me.LblPostalCode.TabIndex = 3
        Me.LblPostalCode.Text = "郵便番号"
        '
        'AddresseeNameTextBox
        '
        Me.AddresseeNameTextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.AddresseeNameTextBox.Location = New System.Drawing.Point(65, 18)
        Me.AddresseeNameTextBox.Name = "AddresseeNameTextBox"
        Me.AddresseeNameTextBox.Size = New System.Drawing.Size(96, 19)
        Me.AddresseeNameTextBox.TabIndex = 1
        '
        'TitleTextBox
        '
        Me.TitleTextBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.TitleTextBox.Location = New System.Drawing.Point(167, 18)
        Me.TitleTextBox.Name = "TitleTextBox"
        Me.TitleTextBox.Size = New System.Drawing.Size(38, 19)
        Me.TitleTextBox.TabIndex = 2
        '
        'LblName
        '
        Me.LblName.AutoSize = True
        Me.LblName.Location = New System.Drawing.Point(30, 21)
        Me.LblName.Name = "LblName"
        Me.LblName.Size = New System.Drawing.Size(29, 12)
        Me.LblName.TabIndex = 0
        Me.LblName.Text = "宛名"
        '
        'LesseeReferenceButton
        '
        Me.LesseeReferenceButton.Location = New System.Drawing.Point(350, 12)
        Me.LesseeReferenceButton.Name = "LesseeReferenceButton"
        Me.LesseeReferenceButton.Size = New System.Drawing.Size(75, 23)
        Me.LesseeReferenceButton.TabIndex = 3
        Me.LesseeReferenceButton.Text = "検索"
        Me.LesseeReferenceButton.UseVisualStyleBackColor = True
        '
        'ExcelOutputMenu
        '
        Me.ExcelOutputMenu.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ExcelOutputMenu.Controls.Add(Me.OutputWesternEnvelopeButton)
        Me.ExcelOutputMenu.Controls.Add(Me.OutputPostcardButton)
        Me.ExcelOutputMenu.Controls.Add(Me.LabelPaperButton)
        Me.ExcelOutputMenu.Controls.Add(Me.OutputGravePamphletEnvelopeButton)
        Me.ExcelOutputMenu.Controls.Add(Me.OutputCho3EnvelopeButton)
        Me.ExcelOutputMenu.Controls.Add(Me.OutputTransferPaperButton)
        Me.ExcelOutputMenu.Controls.Add(Me.OutputKaku2EnvelopeButton)
        Me.ExcelOutputMenu.Location = New System.Drawing.Point(12, 215)
        Me.ExcelOutputMenu.Name = "ExcelOutputMenu"
        Me.ExcelOutputMenu.Size = New System.Drawing.Size(413, 195)
        Me.ExcelOutputMenu.TabIndex = 5
        Me.ExcelOutputMenu.TabStop = False
        '
        'OutputWesternEnvelopeButton
        '
        Me.OutputWesternEnvelopeButton.Location = New System.Drawing.Point(278, 77)
        Me.OutputWesternEnvelopeButton.Name = "OutputWesternEnvelopeButton"
        Me.OutputWesternEnvelopeButton.Size = New System.Drawing.Size(129, 53)
        Me.OutputWesternEnvelopeButton.TabIndex = 5
        Me.OutputWesternEnvelopeButton.Text = "洋封筒"
        Me.OutputWesternEnvelopeButton.UseVisualStyleBackColor = True
        '
        'OutputPostcardButton
        '
        Me.OutputPostcardButton.Location = New System.Drawing.Point(141, 77)
        Me.OutputPostcardButton.Name = "OutputPostcardButton"
        Me.OutputPostcardButton.Size = New System.Drawing.Size(131, 53)
        Me.OutputPostcardButton.TabIndex = 4
        Me.OutputPostcardButton.Text = "ハガキ"
        Me.OutputPostcardButton.UseVisualStyleBackColor = True
        '
        'LabelPaperButton
        '
        Me.LabelPaperButton.Location = New System.Drawing.Point(8, 136)
        Me.LabelPaperButton.Name = "LabelPaperButton"
        Me.LabelPaperButton.Size = New System.Drawing.Size(129, 53)
        Me.LabelPaperButton.TabIndex = 6
        Me.LabelPaperButton.Text = "ラベル用紙作成"
        Me.LabelPaperButton.UseVisualStyleBackColor = True
        '
        'OutputGravePamphletEnvelopeButton
        '
        Me.OutputGravePamphletEnvelopeButton.Location = New System.Drawing.Point(278, 18)
        Me.OutputGravePamphletEnvelopeButton.Name = "OutputGravePamphletEnvelopeButton"
        Me.OutputGravePamphletEnvelopeButton.Size = New System.Drawing.Size(129, 53)
        Me.OutputGravePamphletEnvelopeButton.TabIndex = 2
        Me.OutputGravePamphletEnvelopeButton.Text = "墓地パンフ封筒"
        Me.OutputGravePamphletEnvelopeButton.UseVisualStyleBackColor = True
        '
        'OutputCho3EnvelopeButton
        '
        Me.OutputCho3EnvelopeButton.Location = New System.Drawing.Point(141, 18)
        Me.OutputCho3EnvelopeButton.Name = "OutputCho3EnvelopeButton"
        Me.OutputCho3EnvelopeButton.Size = New System.Drawing.Size(131, 53)
        Me.OutputCho3EnvelopeButton.TabIndex = 1
        Me.OutputCho3EnvelopeButton.Text = "長３封筒"
        Me.OutputCho3EnvelopeButton.UseVisualStyleBackColor = True
        '
        'OutputTransferPaperButton
        '
        Me.OutputTransferPaperButton.Location = New System.Drawing.Point(6, 18)
        Me.OutputTransferPaperButton.Name = "OutputTransferPaperButton"
        Me.OutputTransferPaperButton.Size = New System.Drawing.Size(129, 53)
        Me.OutputTransferPaperButton.TabIndex = 0
        Me.OutputTransferPaperButton.Text = "振込用紙"
        Me.OutputTransferPaperButton.UseVisualStyleBackColor = True
        '
        'OutputKaku2EnvelopeButton
        '
        Me.OutputKaku2EnvelopeButton.Location = New System.Drawing.Point(8, 77)
        Me.OutputKaku2EnvelopeButton.Name = "OutputKaku2EnvelopeButton"
        Me.OutputKaku2EnvelopeButton.Size = New System.Drawing.Size(129, 53)
        Me.OutputKaku2EnvelopeButton.TabIndex = 3
        Me.OutputKaku2EnvelopeButton.Text = "角二封筒"
        Me.OutputKaku2EnvelopeButton.UseVisualStyleBackColor = True
        '
        'CustomerIDTextBox
        '
        Me.CustomerIDTextBox.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.CustomerIDTextBox.Location = New System.Drawing.Point(69, 12)
        Me.CustomerIDTextBox.MaxLength = 6
        Me.CustomerIDTextBox.Name = "CustomerIDTextBox"
        Me.CustomerIDTextBox.Size = New System.Drawing.Size(100, 19)
        Me.CustomerIDTextBox.TabIndex = 1
        '
        'LblMnagementNumber
        '
        Me.LblMnagementNumber.AutoSize = True
        Me.LblMnagementNumber.Location = New System.Drawing.Point(12, 15)
        Me.LblMnagementNumber.Name = "LblMnagementNumber"
        Me.LblMnagementNumber.Size = New System.Drawing.Size(53, 12)
        Me.LblMnagementNumber.TabIndex = 0
        Me.LblMnagementNumber.Text = "管理番号"
        '
        'GotoMultiAddresseeDataViewButton
        '
        Me.GotoMultiAddresseeDataViewButton.Location = New System.Drawing.Point(336, 416)
        Me.GotoMultiAddresseeDataViewButton.Name = "GotoMultiAddresseeDataViewButton"
        Me.GotoMultiAddresseeDataViewButton.Size = New System.Drawing.Size(89, 23)
        Me.GotoMultiAddresseeDataViewButton.TabIndex = 6
        Me.GotoMultiAddresseeDataViewButton.Text = "一括出力画面"
        Me.GotoMultiAddresseeDataViewButton.UseVisualStyleBackColor = True
        '
        'SingleAddresseeDataView
        '
        Me.AcceptButton = Me.LesseeReferenceButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(437, 451)
        Me.Controls.Add(Me.GotoMultiAddresseeDataViewButton)
        Me.Controls.Add(Me.NotNoteInputCheckBox)
        Me.Controls.Add(Me.GboInfo)
        Me.Controls.Add(Me.LesseeReferenceButton)
        Me.Controls.Add(Me.ExcelOutputMenu)
        Me.Controls.Add(Me.CustomerIDTextBox)
        Me.Controls.Add(Me.LblMnagementNumber)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "SingleAddresseeDataView"
        Me.Text = "いろいろ発行"
        Me.GboInfo.ResumeLayout(False)
        Me.GboInfo.PerformLayout()
        Me.ExcelOutputMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents NotNoteInputCheckBox As CheckBox
    Friend WithEvents GboInfo As GroupBox
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
    Friend WithEvents ExcelOutputMenu As GroupBox
    Friend WithEvents OutputPostcardButton As Button
    Friend WithEvents LabelPaperButton As Button
    Friend WithEvents OutputGravePamphletEnvelopeButton As Button
    Friend WithEvents OutputCho3EnvelopeButton As Button
    Friend WithEvents OutputTransferPaperButton As Button
    Friend WithEvents OutputKaku2EnvelopeButton As Button
    Friend WithEvents CustomerIDTextBox As TextBox
    Friend WithEvents LblMnagementNumber As Label
    Friend WithEvents GotoMultiAddresseeDataViewButton As Button
    Friend WithEvents OutputWesternEnvelopeButton As Button
End Class
