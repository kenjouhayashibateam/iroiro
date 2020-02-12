<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MultiAddresseeDataView
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MultiAddresseeDataView))
        Me.AddresseeListView = New System.Windows.Forms.ListView()
        Me.CustomerIDColumnHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AddresseeNameColumnHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.PostalcodeColumnHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Address1ColumnHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Address2ColumnHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AddLesseeCustomerIDTextBox = New System.Windows.Forms.TextBox()
        Me.AddListButton = New System.Windows.Forms.Button()
        Me.DeleteItemButton = New System.Windows.Forms.Button()
        Me.BatchEntryCustamerIDButton = New System.Windows.Forms.Button()
        Me.BatchEntryAddresseeListButton = New System.Windows.Forms.Button()
        Me.Cho3EnvelopeButton = New System.Windows.Forms.Button()
        Me.LabelButton = New System.Windows.Forms.Button()
        Me.WesternEnvelopeButton = New System.Windows.Forms.Button()
        Me.PostcardButton = New System.Windows.Forms.Button()
        Me.KakuniEnvelopeButton = New System.Windows.Forms.Button()
        Me.GravePamphletEnvelopeButton = New System.Windows.Forms.Button()
        Me.DescriptionToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TitleTextBox = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'AddresseeListView
        '
        Me.AddresseeListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.AddresseeListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.CustomerIDColumnHeader, Me.AddresseeNameColumnHeader, Me.PostalcodeColumnHeader, Me.Address1ColumnHeader, Me.Address2ColumnHeader})
        Me.AddresseeListView.FullRowSelect = True
        Me.AddresseeListView.GridLines = True
        Me.AddresseeListView.HideSelection = False
        Me.AddresseeListView.Location = New System.Drawing.Point(12, 41)
        Me.AddresseeListView.Name = "AddresseeListView"
        Me.AddresseeListView.Size = New System.Drawing.Size(620, 373)
        Me.AddresseeListView.TabIndex = 0
        Me.AddresseeListView.UseCompatibleStateImageBehavior = False
        Me.AddresseeListView.View = System.Windows.Forms.View.Details
        '
        'CustomerIDColumnHeader
        '
        Me.CustomerIDColumnHeader.Text = "管理番号"
        '
        'AddresseeNameColumnHeader
        '
        Me.AddresseeNameColumnHeader.Text = "宛名"
        Me.AddresseeNameColumnHeader.Width = 93
        '
        'PostalcodeColumnHeader
        '
        Me.PostalcodeColumnHeader.Text = "郵便番号"
        Me.PostalcodeColumnHeader.Width = 76
        '
        'Address1ColumnHeader
        '
        Me.Address1ColumnHeader.Text = "住所1"
        Me.Address1ColumnHeader.Width = 216
        '
        'Address2ColumnHeader
        '
        Me.Address2ColumnHeader.Text = "住所2"
        Me.Address2ColumnHeader.Width = 150
        '
        'AddLesseeCustomerIDTextBox
        '
        Me.AddLesseeCustomerIDTextBox.Location = New System.Drawing.Point(12, 14)
        Me.AddLesseeCustomerIDTextBox.Name = "AddLesseeCustomerIDTextBox"
        Me.AddLesseeCustomerIDTextBox.Size = New System.Drawing.Size(100, 19)
        Me.AddLesseeCustomerIDTextBox.TabIndex = 1
        '
        'AddListButton
        '
        Me.AddListButton.Location = New System.Drawing.Point(118, 12)
        Me.AddListButton.Name = "AddListButton"
        Me.AddListButton.Size = New System.Drawing.Size(75, 23)
        Me.AddListButton.TabIndex = 2
        Me.AddListButton.Text = "一覧に入力"
        Me.AddListButton.UseVisualStyleBackColor = True
        '
        'DeleteItemButton
        '
        Me.DeleteItemButton.Location = New System.Drawing.Point(199, 12)
        Me.DeleteItemButton.Name = "DeleteItemButton"
        Me.DeleteItemButton.Size = New System.Drawing.Size(75, 23)
        Me.DeleteItemButton.TabIndex = 3
        Me.DeleteItemButton.Text = "行を削除"
        Me.DeleteItemButton.UseVisualStyleBackColor = True
        '
        'BatchEntryCustamerIDButton
        '
        Me.BatchEntryCustamerIDButton.Location = New System.Drawing.Point(280, 12)
        Me.BatchEntryCustamerIDButton.Name = "BatchEntryCustamerIDButton"
        Me.BatchEntryCustamerIDButton.Size = New System.Drawing.Size(116, 23)
        Me.BatchEntryCustamerIDButton.TabIndex = 4
        Me.BatchEntryCustamerIDButton.Text = "管理番号一括入力"
        Me.DescriptionToolTip.SetToolTip(Me.BatchEntryCustamerIDButton, "管理番号をエクセルなどのリストからクリップボードにタブ区切りでコピーしたものを使用して一覧に出力します。" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "管理番号を縦1列に表示してコピーしてください。")
        Me.BatchEntryCustamerIDButton.UseVisualStyleBackColor = True
        '
        'BatchEntryAddresseeListButton
        '
        Me.BatchEntryAddresseeListButton.Location = New System.Drawing.Point(402, 12)
        Me.BatchEntryAddresseeListButton.Name = "BatchEntryAddresseeListButton"
        Me.BatchEntryAddresseeListButton.Size = New System.Drawing.Size(116, 23)
        Me.BatchEntryAddresseeListButton.TabIndex = 5
        Me.BatchEntryAddresseeListButton.Text = "宛先リスト入力"
        Me.DescriptionToolTip.SetToolTip(Me.BatchEntryAddresseeListButton, "エクセルなどの宛先の一覧をタブ区切りでクリップボードにコピーしたものを一覧に表示します。")
        Me.BatchEntryAddresseeListButton.UseVisualStyleBackColor = True
        '
        'Cho3EnvelopeButton
        '
        Me.Cho3EnvelopeButton.Location = New System.Drawing.Point(83, 420)
        Me.Cho3EnvelopeButton.Name = "Cho3EnvelopeButton"
        Me.Cho3EnvelopeButton.Size = New System.Drawing.Size(75, 23)
        Me.Cho3EnvelopeButton.TabIndex = 6
        Me.Cho3EnvelopeButton.Text = "長3封筒"
        Me.Cho3EnvelopeButton.UseVisualStyleBackColor = True
        '
        'LabelButton
        '
        Me.LabelButton.Location = New System.Drawing.Point(488, 420)
        Me.LabelButton.Name = "LabelButton"
        Me.LabelButton.Size = New System.Drawing.Size(75, 23)
        Me.LabelButton.TabIndex = 7
        Me.LabelButton.Text = "ラベル出し"
        Me.LabelButton.UseVisualStyleBackColor = True
        '
        'WesternEnvelopeButton
        '
        Me.WesternEnvelopeButton.Location = New System.Drawing.Point(407, 420)
        Me.WesternEnvelopeButton.Name = "WesternEnvelopeButton"
        Me.WesternEnvelopeButton.Size = New System.Drawing.Size(75, 23)
        Me.WesternEnvelopeButton.TabIndex = 8
        Me.WesternEnvelopeButton.Text = "洋封筒"
        Me.WesternEnvelopeButton.UseVisualStyleBackColor = True
        '
        'PostcardButton
        '
        Me.PostcardButton.Location = New System.Drawing.Point(326, 420)
        Me.PostcardButton.Name = "PostcardButton"
        Me.PostcardButton.Size = New System.Drawing.Size(75, 23)
        Me.PostcardButton.TabIndex = 9
        Me.PostcardButton.Text = "はがき"
        Me.PostcardButton.UseVisualStyleBackColor = True
        '
        'KakuniEnvelopeButton
        '
        Me.KakuniEnvelopeButton.Location = New System.Drawing.Point(245, 420)
        Me.KakuniEnvelopeButton.Name = "KakuniEnvelopeButton"
        Me.KakuniEnvelopeButton.Size = New System.Drawing.Size(75, 23)
        Me.KakuniEnvelopeButton.TabIndex = 10
        Me.KakuniEnvelopeButton.Text = "角二封筒"
        Me.KakuniEnvelopeButton.UseVisualStyleBackColor = True
        '
        'GravePamphletEnvelopeButton
        '
        Me.GravePamphletEnvelopeButton.Location = New System.Drawing.Point(164, 420)
        Me.GravePamphletEnvelopeButton.Name = "GravePamphletEnvelopeButton"
        Me.GravePamphletEnvelopeButton.Size = New System.Drawing.Size(75, 23)
        Me.GravePamphletEnvelopeButton.TabIndex = 11
        Me.GravePamphletEnvelopeButton.Text = "墓地パンフ"
        Me.GravePamphletEnvelopeButton.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(524, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 12)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "敬称"
        '
        'TitleTextBox
        '
        Me.TitleTextBox.Location = New System.Drawing.Point(559, 14)
        Me.TitleTextBox.Name = "TitleTextBox"
        Me.TitleTextBox.Size = New System.Drawing.Size(51, 19)
        Me.TitleTextBox.TabIndex = 13
        '
        'MultiAddresseeDataView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(644, 455)
        Me.Controls.Add(Me.TitleTextBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GravePamphletEnvelopeButton)
        Me.Controls.Add(Me.KakuniEnvelopeButton)
        Me.Controls.Add(Me.PostcardButton)
        Me.Controls.Add(Me.WesternEnvelopeButton)
        Me.Controls.Add(Me.LabelButton)
        Me.Controls.Add(Me.Cho3EnvelopeButton)
        Me.Controls.Add(Me.BatchEntryAddresseeListButton)
        Me.Controls.Add(Me.BatchEntryCustamerIDButton)
        Me.Controls.Add(Me.DeleteItemButton)
        Me.Controls.Add(Me.AddListButton)
        Me.Controls.Add(Me.AddLesseeCustomerIDTextBox)
        Me.Controls.Add(Me.AddresseeListView)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MultiAddresseeDataView"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "一括発行"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents AddresseeListView As ListView
    Friend WithEvents CustomerIDColumnHeader As ColumnHeader
    Friend WithEvents AddresseeNameColumnHeader As ColumnHeader
    Friend WithEvents AddLesseeCustomerIDTextBox As TextBox
    Friend WithEvents AddListButton As Button
    Friend WithEvents PostalcodeColumnHeader As ColumnHeader
    Friend WithEvents Address1ColumnHeader As ColumnHeader
    Friend WithEvents Address2ColumnHeader As ColumnHeader
    Friend WithEvents DeleteItemButton As Button
    Friend WithEvents BatchEntryCustamerIDButton As Button
    Friend WithEvents BatchEntryAddresseeListButton As Button
    Friend WithEvents Cho3EnvelopeButton As Button
    Friend WithEvents LabelButton As Button
    Friend WithEvents WesternEnvelopeButton As Button
    Friend WithEvents PostcardButton As Button
    Friend WithEvents KakuniEnvelopeButton As Button
    Friend WithEvents GravePamphletEnvelopeButton As Button
    Friend WithEvents DescriptionToolTip As ToolTip
    Friend WithEvents Label1 As Label
    Friend WithEvents TitleTextBox As TextBox
End Class
