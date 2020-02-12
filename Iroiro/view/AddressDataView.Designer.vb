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
        Me.AddressResultListView = New System.Windows.Forms.ListView()
        Me.clmPostalCode = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.clmAddress = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'AddressResultListView
        '
        Me.AddressResultListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.clmPostalCode, Me.clmAddress})
        Me.AddressResultListView.FullRowSelect = True
        Me.AddressResultListView.GridLines = True
        Me.AddressResultListView.HideSelection = False
        Me.AddressResultListView.Location = New System.Drawing.Point(12, 12)
        Me.AddressResultListView.Name = "AddressResultListView"
        Me.AddressResultListView.Size = New System.Drawing.Size(390, 161)
        Me.AddressResultListView.TabIndex = 4
        Me.AddressResultListView.UseCompatibleStateImageBehavior = False
        Me.AddressResultListView.View = System.Windows.Forms.View.Details
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
        'ExitButton
        '
        Me.ExitButton.Location = New System.Drawing.Point(12, 179)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(390, 23)
        Me.ExitButton.TabIndex = 5
        Me.ExitButton.Text = "閉じる(選択されている住所が基の画面に反映されます。)"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'AddressDataView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(414, 214)
        Me.Controls.Add(Me.AddressResultListView)
        Me.Controls.Add(Me.ExitButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AddressDataView"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "住所検索結果"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents AddressResultListView As ListView
    Friend WithEvents clmPostalCode As ColumnHeader
    Friend WithEvents clmAddress As ColumnHeader
    Friend WithEvents ExitButton As Button
End Class
