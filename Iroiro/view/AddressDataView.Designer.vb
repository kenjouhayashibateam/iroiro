<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddressDataView
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AddressDataView))
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.clmPostalCode = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.clmAddress = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AddressResultListView = New System.Windows.Forms.ListView()
        Me.AddressResultDataGridView = New System.Windows.Forms.DataGridView()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.AddressResultDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ExitButton
        '
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Dock = System.Windows.Forms.DockStyle.Top
        Me.ExitButton.Location = New System.Drawing.Point(0, 150)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(388, 23)
        Me.ExitButton.TabIndex = 5
        Me.ExitButton.Text = "閉じる(選択されている住所が基の画面に反映されます。)"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'clmPostalCode
        '
        Me.clmPostalCode.Text = "郵便番号"
        Me.clmPostalCode.Width = 79
        '
        'clmAddress
        '
        Me.clmAddress.Text = "住所"
        Me.clmAddress.Width = 284
        '
        'AddressResultListView
        '
        Me.AddressResultListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.clmPostalCode, Me.clmAddress})
        Me.AddressResultListView.FullRowSelect = True
        Me.AddressResultListView.GridLines = True
        Me.AddressResultListView.HideSelection = False
        Me.AddressResultListView.Location = New System.Drawing.Point(12, 239)
        Me.AddressResultListView.Name = "AddressResultListView"
        Me.AddressResultListView.Size = New System.Drawing.Size(390, 161)
        Me.AddressResultListView.TabIndex = 4
        Me.AddressResultListView.UseCompatibleStateImageBehavior = False
        Me.AddressResultListView.View = System.Windows.Forms.View.Details
        '
        'AddressResultDataGridView
        '
        Me.AddressResultDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.AddressResultDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
        Me.AddressResultDataGridView.Dock = System.Windows.Forms.DockStyle.Top
        Me.AddressResultDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.AddressResultDataGridView.Location = New System.Drawing.Point(0, 0)
        Me.AddressResultDataGridView.MultiSelect = False
        Me.AddressResultDataGridView.Name = "AddressResultDataGridView"
        Me.AddressResultDataGridView.RowTemplate.Height = 21
        Me.AddressResultDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.AddressResultDataGridView.Size = New System.Drawing.Size(388, 150)
        Me.AddressResultDataGridView.TabIndex = 6
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(197, 209)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(121, 20)
        Me.ComboBox1.TabIndex = 7
        '
        'Column1
        '
        Me.Column1.HeaderText = "Column1"
        Me.Column1.Name = "Column1"
        '
        'Column2
        '
        Me.Column2.HeaderText = "Column2"
        Me.Column2.Name = "Column2"
        '
        'AddressDataView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(388, 322)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.ExitButton)
        Me.Controls.Add(Me.AddressResultDataGridView)
        Me.Controls.Add(Me.AddressResultListView)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AddressDataView"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "住所検索結果"
        CType(Me.AddressResultDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ExitButton As Button
    Friend WithEvents clmPostalCode As ColumnHeader
    Friend WithEvents clmAddress As ColumnHeader
    Friend WithEvents AddressResultListView As ListView
    Friend WithEvents AddressResultDataGridView As DataGridView
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
End Class
