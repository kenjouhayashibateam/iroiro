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
        Me.BatchEntryCustamerIDButton.UseVisualStyleBackColor = True
        '
        'BatchEntryAddresseeListButton
        '
        Me.BatchEntryAddresseeListButton.Location = New System.Drawing.Point(402, 12)
        Me.BatchEntryAddresseeListButton.Name = "BatchEntryAddresseeListButton"
        Me.BatchEntryAddresseeListButton.Size = New System.Drawing.Size(116, 23)
        Me.BatchEntryAddresseeListButton.TabIndex = 5
        Me.BatchEntryAddresseeListButton.Text = "宛先リスト入力"
        Me.BatchEntryAddresseeListButton.UseVisualStyleBackColor = True
        '
        'MultiAddresseeDataView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(644, 484)
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
End Class
