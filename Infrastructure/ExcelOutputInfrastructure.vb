Imports Microsoft.Office.Interop.Excel
Imports Domain

''' <summary>
''' エクセルへの処理を行います
''' </summary>
Public Class ExcelOutputInfrastructure
    Implements IAdresseeOutputRepogitory

    Private Property ESS As IExcelSheetSetting

    ''' <summary>
    ''' 振込用紙用のデータをエクセルに入力します
    ''' </summary>
    ''' <param name="addressee">宛名</param>
    ''' <param name="title">敬称</param>
    ''' <param name="postalcode">郵便番号</param>
    ''' <param name="address1">住所1</param>
    ''' <param name="address2">住所2</param>
    ''' <param name="money">振込金額</param>
    ''' <param name="note1">備考1</param>
    ''' <param name="note2">備考2</param>
    ''' <param name="note3">備考3</param>
    ''' <param name="note4">備考4</param>
    ''' <param name="note5">備考5</param>
    ''' <param name="addressee_index">リストで出力する際のインデックス</param>
    Public Sub DataInput(addressee As String, title As String, postalcode As String, address1 As String, address2 As String,
                         outputcontents As IAdresseeOutputRepogitory.OutputData, Optional money As String = "", Optional note1 As String = "",
                         Optional note2 As String = "", Optional note3 As String = "", Optional note4 As String = "", Optional note5 As String = "",
                         Optional addressee_index As Integer = 0) Implements IAdresseeOutputRepogitory.DataInput

        Dim myaddressee As New AddresseeData(addressee, title, postalcode, address1, address2, money, note1, note2, note3, note4, note5)

        Select Case outputcontents
            Case IAdresseeOutputRepogitory.OutputData.Transfer
                ESS = New TransterPaper
            Case IAdresseeOutputRepogitory.OutputData.Cho3
                ESS = New Cho3Envlope
            Case IAdresseeOutputRepogitory.OutputData.Western
                ESS = New WesternEnvelope
            Case IAdresseeOutputRepogitory.OutputData.GravePamphlet
                ESS = New GravePamphlet
            Case IAdresseeOutputRepogitory.OutputData.Kaku2
                ESS = New Kaku2Envelope
            Case IAdresseeOutputRepogitory.OutputData.Label
                ESS = New LabelSheet
            Case IAdresseeOutputRepogitory.OutputData.Postcard
                ESS = New Postcard
            Case Else
                Exit Sub
        End Select

        ESS.Output(myaddressee, addressee_index)

    End Sub

    Public Sub OutputMediaClose() Implements IAdresseeOutputRepogitory.OutputMediaClose
        If ESS Is Nothing Then Exit Sub
        ESS.FileClose()
    End Sub

    ''' <summary>
    ''' エクセルに出力する宛名等を格納するクラス
    ''' </summary>
    Private Class AddresseeData

        ''' <summary>
        ''' 宛名
        ''' </summary>
        Public Property AddresseeName As String
        ''' <summary>
        ''' 敬称
        ''' </summary>
        Public Property Title As String
        ''' <summary>
        ''' 郵便番号
        ''' </summary>
        Public Property AddresseePostalCode As String
        ''' <summary>
        ''' 住所1
        ''' </summary>
        Public Property AddresseeAddress1 As String
        ''' <summary>
        ''' 住所2
        ''' </summary>
        Public Property AddresseeAddress2 As String
        ''' <summary>
        ''' 備考1
        ''' </summary>
        Public Property Note1 As String
        ''' <summary>
        ''' 備考2
        ''' </summary>
        Public Property Note2 As String
        ''' <summary>
        ''' 備考3
        ''' </summary>
        Public Property Note3 As String
        ''' <summary>
        ''' 備考4
        ''' </summary>
        Public Property Note4 As String
        ''' <summary>
        ''' 備考5
        ''' </summary>
        Public Property Note5 As String
        ''' <summary>
        ''' 金額
        ''' </summary>
        Public Property Money As String

        ''' <param name="_addresseename">宛名</param>
        ''' <param name="_title">敬称</param>
        ''' <param name="_postalcode">郵便番号</param>
        ''' <param name="_address1">住所1</param>
        ''' <param name="_address2">住所2</param>
        ''' <param name="_money">金額</param>
        ''' <param name="_note1">備考1</param>
        ''' <param name="_note2">備考2</param>
        ''' <param name="_note3">備考3</param>
        ''' <param name="_note4">備考4</param>
        ''' <param name="_note5">備考5</param>
        Sub New(ByVal _addresseename As String, ByVal _title As String, ByVal _postalcode As String, ByVal _address1 As String, _address2 As String,
                Optional ByVal _money As String = "", Optional ByVal _note1 As String = "", Optional ByVal _note2 As String = "",
                Optional ByVal _note3 As String = "", Optional ByVal _note4 As String = "", Optional ByVal _note5 As String = "")

            AddresseeName = _addresseename
            Title = _title
            AddresseePostalCode = _postalcode
            AddresseeAddress1 = _address1
            AddresseeAddress2 = _address2
            Note1 = _note1
            Note2 = _note2
            Note3 = _note3
            Note4 = _note4
            Note5 = _note5
            Money = _money
        End Sub

    End Class

    ''' <summary>
    ''' エクセルに値を入力する為の基本クラス。
    ''' </summary>
    Private MustInherit Class IExcelSheetSetting

        ''' <summary>
        ''' 印刷物を発行するエクセルの列のサイズを配列で保持します。
        ''' </summary>
        Protected ReadOnly ColumnSizes() As Double
        ''' <summary>
        ''' 印刷物を発行するエクセルの行のサイズを配列で保持します。
        ''' </summary>
        Protected ReadOnly RowSizes() As Double
        ''' <summary>
        ''' 複数データを印刷する際の各入力データの一番上の数値を設定します
        ''' </summary>
        Protected StartRowPosition As Integer
        ''' <summary>
        ''' クラスが自動で開いたエクセルワークブックの名前を保持する
        ''' </summary>
        Private WorkbookName As String

        ''' <summary>
        ''' エクセルアプリケーション
        ''' </summary>
        Private Property ExlApp As Application
        ''' <summary>
        ''' エクセルワークブック
        ''' </summary>
        Private Property ExlWorkbook As Workbook
        ''' <summary>
        ''' エクセルワークシート
        ''' </summary>
        Protected Property ExlWorkSheet As Worksheet

        ''' <summary>
        ''' 新たにエクセルワークブックを開きます。既に開いている場合はそのブックをカレントします。
        ''' </summary>
        Private Sub FileOpen()

            Try
                ExlApp = GetObject(, "Excel.Application")

                For Each wb As Workbook In ExlApp.Workbooks
                    If wb.Name IsNot WorkbookName Then Continue For
                    ExlWorkbook = wb
                    Exit For
                Next

            Catch ex As Exception
                ExlApp = CreateObject("Excel.Application")
            End Try

            If ExlWorkbook Is Nothing Then ExlWorkbook = ExlApp.Workbooks.Add

            ExlWorkSheet = ExlWorkbook.Sheets(1)
            WorkbookName = ExlWorkbook.Name

            SetMargin()

            ExlApp.Visible = True

        End Sub

        Private Sub SetCellProperty()

            With ExlWorkSheet
                'ColumnSizesの配列の中の数字をシートのカラムの幅に設定する
                For I As Integer = 0 To UBound(ColumnSizes)
                    .Columns(I + 1).ColumnWidth = ColumnSizes(I)
                Next
                'RowSizesの配列の中の数字をシートのローの幅に設定する
                For I = 0 To UBound(RowSizes)
                    .Rows(StartRowPosition + (I + 1)).RowHeight = RowSizes(I)
                Next
            End With

            CellProperty()

        End Sub

        '''<summary>
        ''' エクセルにデータを出力する
        ''' </summary>
        ''' <param name="addressee">出力する宛名</param>
        ''' <param name="_index">複数印刷用インデックス</param>
        Public Sub Output(ByVal addressee As AddresseeData, ByVal _index As Integer)

            StartRowPosition = _index * (UBound(RowSizes) + 1)

            With ExlWorkSheet
                .Activate()
                .Cells.UnMerge()
            End With

            SetCellProperty()
            SetCellFont()
            CellsJoin()
            DataOutput(addressee)

        End Sub

        Sub New()

            FileOpen()
            ColumnSizes = SetColumnSizes()
            RowSizes = SetRowSizes()
            ExlWorkSheet.Activate()

        End Sub

        ''' <summary>
        ''' セルの文字の大きさ、配置などを設定します
        ''' </summary>
        Protected MustOverride Sub CellProperty()
        ''' <summary>
        ''' 印刷物を発行するエクセルの列のサイズの配列
        ''' </summary>
        Protected MustOverride Function SetColumnSizes() As Double()
        ''' <summary>
        ''' 印刷物を発行するエクセルの行のサイズの配列
        ''' </summary>
        Protected MustOverride Function SetRowSizes() As Double()
        ''' <summary>
        ''' 印刷物を発行するエクセルのセルで結合する必要のある場所を設定します
        ''' </summary>
        Protected MustOverride Sub CellsJoin()
        ''' <summary>
        ''' 宛名データを出力する
        ''' </summary>
        ''' <param name="addressee">宛名データ</param>
        Protected MustOverride Sub DataOutput(ByVal addressee As AddresseeData)

        ''' <summary>
        ''' いろいろ発行用のエクセルを閉じる
        ''' </summary>
        Public Sub FileClose()

            If ExlApp.Workbooks Is Nothing Then Exit Sub

            For Each wb As Workbook In ExlApp.Workbooks
                If wb.Name <> WorkbookName Then Continue For
                wb.Close(False)
                Exit For
            Next

            If ExlApp.Workbooks.Count = 0 Then ExlApp.Quit()

        End Sub

        ''' <summary>
        ''' 住所を封筒印字用に変換します
        ''' </summary>
        ''' <param name="address2">住所2</param>
        ''' <returns></returns>
        Protected Function ConvertAddress(ByVal address2 As String) As String

            Dim ReturnString As String = StrConv(address2, vbWide)
            Dim addArray() As String = Split(ReturnString, "－")
            Dim addValue As String = ""

            ReturnString = ""

            For j As Integer = 0 To UBound(addArray)
                If IsNumeric(Mid(addArray(j), 1, Len(addArray(j) - 1))) Then addValue = ConvertNumber(Mid(addArray(j), 1, Len(addArray(j) - 1)))
                ReturnString &= addValue & "－"
            Next

            ReturnString = Replace(ReturnString, "－", "ー")

            Return Mid(ReturnString, 1, Len(ReturnString) - 1)

        End Function

        ''' <summary>
        ''' 県名等を略します。東京、神奈川、徳島は全て、その他は県名と市名が一緒の場合に略します。
        ''' </summary>
        ''' <param name="address1">検証する住所</param>
        ''' <returns></returns>
        Protected Function CutAddress(ByVal address1 As String) As String

            Dim AddressText As String

            AddressText = address1
            '東京、神奈川、徳島は略す
            AddressText = Replace(AddressText, "東京都", "")
            AddressText = Replace(AddressText, "神奈川県", "")
            AddressText = Replace(AddressText, "徳島県", "")
            If Len(address1) <> Len(AddressText) Then Return AddressText

            '郡が入っている住所はそのまま返す
            If InStr(AddressText, "郡") <> 0 Then Return AddressText

            '県と市を比べる
            AddressText = VerifyAddressString(AddressText, "県")

            '府と市を比べる
            AddressText = VerifyAddressString(AddressText, "府")

            Return AddressText

        End Function

        ''' <summary>
        ''' 検証する県、府が市と同じ名前の場合、市から始まる住所にして返します
        ''' </summary>
        ''' <param name="address">住所</param>
        ''' <param name="verifystring">検証する文字列</param>
        ''' <returns></returns>
        Private Function VerifyAddressString(ByVal address As String, ByVal verifystring As String) As String

            If InStr(address, verifystring) = 0 Then Return address

            If Mid(address, 1, InStr(1, address, verifystring) - 1) = Mid(address, InStr(1, address, verifystring) + 1,
                                                                          InStr(1, address, "市") - InStr(1, address, verifystring) - 1) Then
                Return Mid(address, InStr(1, address, verifystring) + 1)
            End If

            Return address

        End Function

        ''' <summary>
        ''' 数字を漢数字に変換します
        ''' </summary>
        ''' <param name="myNumber">変換する数字</param>
        ''' <returns></returns>
        Private Function ConvertNumber(ByVal myNumber As Integer) As String

            Select Case myNumber
                Case < 11
                    Return ConvertNumber_Under10(myNumber)
                Case < 20
                    Return ConvertNumber_Over11Under19(myNumber)
                Case Else
                    Return ConvertNumber_Orver20(myNumber)
            End Select

        End Function

        ''' <summary>
        ''' 10以下の数字を漢数字に変換します
        ''' </summary>
        ''' <param name="myNumber">変換する数字</param>
        ''' <returns></returns>
        Private Function ConvertNumber_Under10(ByVal myNumber As Integer) As String

            Select Case myNumber
                Case 0
                    Return "〇"
                Case 1
                    Return "一"
                Case 2
                    Return "二"
                Case 3
                    Return "三"
                Case 4
                    Return "四"
                Case 5
                    Return "五"
                Case 6
                    Return "六"
                Case 7
                    Return "七"
                Case 8
                    Return "八"
                Case 9
                    Return "九"
                Case 10
                    Return "十"
                Case Else
                    MsgBox("Error")
                    Return ""
            End Select

        End Function

        ''' <summary>
        ''' 11から19までの数字を変換します
        ''' </summary>
        ''' <param name="myNumber">変換する数字</param>
        ''' <returns></returns>
        Private Function ConvertNumber_Over11Under19(ByVal myNumber As Integer) As String

            Select Case myNumber
                Case 10
                    Return "一〇"
                Case 11
                    Return "十一"
                Case 12
                    Return "十二"
                Case 13
                    Return "十三"
                Case 14
                    Return "十四"
                Case 15
                    Return "十五"
                Case 16
                    Return "十六"
                Case 17
                    Return "十七"
                Case 18
                    Return "十八"
                Case 19
                    Return "十九"
                Case Else
                    MsgBox("Error")
                    Return ""
            End Select

        End Function

        ''' <summary>
        ''' 20以上の数字を変換します
        ''' </summary>
        ''' <param name="myNumber">変換する数字</param>
        ''' <returns></returns>
        Private Function ConvertNumber_Orver20(ByVal myNumber As Integer) As String

            Dim I As Integer
            Dim myValue As String = ""

            For I = 1 To Len(CStr(myNumber))
                myValue &= ConvertNumber_Under10(Mid(myNumber, I, 1))
            Next

            If Len(CStr(myValue)) <> 2 Then Return myValue
            '20、30などの数字を〇から十に変える
            If Mid(myValue, 2, 1) = "〇" Then myValue = Mid(myValue, 1, 1) & "十"

            Return myValue

        End Function


        ''' <summary>
        ''' エクセルシートの余白を0に設定する
        ''' </summary>
        Private Sub SetMargin()

            With ExlWorkSheet.PageSetup
                .TopMargin = 0
                .BottomMargin = 0
                .RightMargin = 0
                .LeftMargin = 0
            End With

        End Sub

        ''' <summary>
        ''' エクセルのシートのフォントを設定する
        ''' </summary>
        Public MustOverride Sub SetCellFont()

    End Class

    ''' <summary>
    ''' 長3封筒クラス
    ''' </summary>
    Private Class Cho3Envlope
        Inherits IExcelSheetSetting

        Public Overrides Sub SetCellFont()
            ExlWorkSheet.Cells.Font.Name = "HGP行書体"
        End Sub

        Protected Overrides Sub CellProperty()

            With ExlWorkSheet
                '宛名
                With .Cells(StartRowPosition + 4, 2)
                    .font.size = 48
                    .HorizontalAlignment = XlHAlign.xlHAlignRight
                    .verticalalignment = XlVAlign.xlVAlignTop
                    .Orientation = XlOrientation.xlVertical
                End With
                '郵便番号
                With .Range(.Cells(StartRowPosition + 2, 3), .Cells(StartRowPosition + 2, 9))
                    .Font.Size = 16
                    .VerticalAlignment = XlVAlign.xlVAlignTop
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                    .Orientation = XlOrientation.xlVertical
                End With

                '住所
                With .Range(.Cells(StartRowPosition + 4, 6), .Cells(StartRowPosition + 4, 9))
                    .Font.Size = 30
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                    .Orientation = XlOrientation.xlVertical
                End With
                .Cells(StartRowPosition + 4, 7).verticalalignment = XlVAlign.xlVAlignCenter
                .Cells(StartRowPosition + 4, 8).verticalalignment = XlVAlign.xlVAlignTop
            End With

        End Sub

        Protected Overrides Sub CellsJoin()

            With ExlWorkSheet
                '住所欄1行目
                .Range(.Cells(StartRowPosition + 4, 8), .Cells(StartRowPosition + 5, 9)).Merge()
                '住所欄2行目
                .Range(.Cells(StartRowPosition + 4, 6), .Cells(StartRowPosition + 5, 7)).Merge()
                '宛名欄
                .Range(.Cells(StartRowPosition + 4, 2), .Cells(StartRowPosition + 5, 3)).Merge()
            End With

        End Sub

        Protected Overrides Sub DataOutput(addressee As AddresseeData)

            Dim addresstext1 As String = ""
            Dim addresstext2 As String = ""
            Dim addresseename As String

            With ExlWorkSheet
                .Cells.ClearContents()
                '郵便番号
                For I As Integer = 1 To 7
                    .Cells(StartRowPosition + 2, I + 2) = Mid(Replace(addressee.AddresseePostalCode, "-", ""), I, 1)
                Next

                '住所
                If Len(addressee.AddresseeAddress1 + addressee.AddresseeAddress2) < 15 Then
                    addresstext1 = addressee.AddresseeAddress1 & " " & addressee.AddresseeAddress2
                Else
                    addresstext1 = addressee.AddresseeAddress1
                    addresstext2 = addressee.AddresseeAddress2
                    If Len(addressee.AddresseeAddress2) > 20 Then .Cells(StartRowPosition + 4, 9).Interior.ColorIndex = 6
                End If
                .Cells(StartRowPosition + 4, 8) = addressee.AddresseeAddress1
                .Cells(StartRowPosition + 4, 6) = ConvertAddress(addressee.AddresseeAddress2)

                '宛名
                If Len(addressee.AddresseeName) > 5 Then
                    addresseename = addressee.AddresseeName & addressee.Title
                Else
                    addresseename = addressee.AddresseeName & " " & addressee.Title
                End If
                .Cells(StartRowPosition + 4, 2) = addresseename
            End With

        End Sub

        Protected Overrides Function SetColumnSizes() As Double()
            Return {17.13, 7.5, 2.75, 2.75, 2.75, 2.38, 2.38, 2.38, 2.38, 1.75}
        End Function

        Protected Overrides Function SetRowSizes() As Double()
            Return {101.25, 38.25, 14.25, 409.5, 133.5, 36}
        End Function
    End Class

    ''' <summary>
    ''' 振込用紙発行クラス
    ''' </summary>
    Private Class TransterPaper
        Inherits IExcelSheetSetting

        Protected Overrides Sub CellsJoin()

            Dim row As Integer

            '宛名備考欄5行を結合
            row = 6
            Do Until row = 11
                With ExlWorkSheet
                    .Range(.Cells(StartRowPosition + row, 4), .Cells(StartRowPosition + row, 11)).Merge()
                End With
                row += 1
            Loop

            'お客様控え欄4行を結合
            row = 10
            Do Until row = 14
                With ExlWorkSheet
                    .Range(.Cells(StartRowPosition + row, 13), .Cells(StartRowPosition + row, 20)).Merge()
                End With
                row += 1
            Loop

        End Sub

        Protected Overrides Sub DataOutput(addressee As AddresseeData)

            With ExlWorkSheet
                .Cells.ClearContents()
                '振込金額入力
                Dim ColumnIndex As Integer = 0
                Dim moneystring As String = "\" & addressee.Money
                Do Until ColumnIndex = Len(moneystring)
                    .Cells(StartRowPosition + 3, 11 - ColumnIndex) = Mid(moneystring, Len(moneystring) - ColumnIndex, 1)
                    .Cells(StartRowPosition + 9, 20 - ColumnIndex) = Mid(moneystring, Len(moneystring) - ColumnIndex, 1)    'お客様控え
                    ColumnIndex += 1
                Loop

                .Cells(StartRowPosition + 6, 4) = addressee.Note1   '備考1
                .Cells(StartRowPosition + 7, 4) = addressee.Note2   '備考2
                .Cells(StartRowPosition + 8, 4) = addressee.Note3   '備考3
                .Cells(StartRowPosition + 9, 4) = addressee.Note4   '備考4
                .Cells(StartRowPosition + 10, 4) = addressee.Note5  '備考5
                .Cells(StartRowPosition + 7, 2) = "〒 " & addressee.AddresseePostalCode      '宛先郵便番号
                .Cells(StartRowPosition + 8, 2) = addressee.AddresseeAddress1               '宛先住所1

                '宛先住所2　長い場合は2行で入力
                .Cells(StartRowPosition + 9, 2) = Mid(addressee.AddresseeAddress2, 1, 20)
                If Len(addressee.AddresseeAddress2) > 20 Then .Cells(StartRowPosition + 9, 2) = Mid(addressee.AddresseeAddress2, 21)

                .Cells(StartRowPosition + 11, 2) = addressee.AddresseeName & " " & addressee.Title  '宛先の宛名
                .Cells(StartRowPosition + 13, 13) = addressee.AddresseeName & " " & addressee.Title 'お客様控えの名前

                'お客様控え住所　長い場合は3行、それでも収まらない場合は注意を促す
                Dim strings() As String = SplitYourCopyAddress(addressee.AddresseeAddress1, addressee.AddresseeAddress2)
                .Cells(StartRowPosition + 10, 13) = " " & strings(0)
                .Cells(StartRowPosition + 11, 13) = " " & strings(1)
                .Cells(StartRowPosition + 12, 13) = " " & strings(2)
            End With

        End Sub

        ''' <summary>
        ''' お客様控えの住所を分けて表示させる
        ''' </summary>
        ''' <param name="address1">住所1</param>
        ''' <param name="address2">住所2</param>
        ''' <returns></returns>
        Private Function SplitYourCopyAddress(ByVal address1 As String, ByVal address2 As String) As String()

            Dim line1, line2, line3, joinaddress As String

            If Len(address1) < 13 Then
                line1 = address1
                line2 = Mid(address2, 1, 12)
                line3 = Mid(address2, 13)
                If Len(address2) > 24 Then MsgBox("お客様控えの住所が長いので、書き直して下さい。", MsgBoxStyle.Information, "文字数がセルの幅を超えています")
            Else
                joinaddress = address1 & address2
                line1 = Mid(joinaddress, 1, 12)
                line2 = Mid(joinaddress, 13, 24)
                line3 = Mid(joinaddress, 25)
                If Len(joinaddress) > 36 Then MsgBox("お客様控えの住所が長いので、書き直して下さい。", MsgBoxStyle.Information, "文字数がセルの幅を超えています")
            End If

            Dim strings() As String = {line1, line2, line3}

            Return strings

        End Function

        Protected Overrides Sub CellProperty()

            With ExlWorkSheet
                .PageSetup.PaperSize = XlPaperSize.xlPaperB5
                '宛名欄
                .Cells(11, 2).Font.Size = 14
                '金額欄
                With .Range(.Cells(StartRowPosition + 3, 4), .Cells(StartRowPosition + 3, 11))
                    .Font.Size = 14
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                    .VerticalAlignment = XlVAlign.xlVAlignCenter
                End With

                'お客様控え金額欄
                With .Range(.Cells(StartRowPosition + 9, 13), .Cells(StartRowPosition + 9, 20))
                    .Font.Size = 14
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                    .VerticalAlignment = XlVAlign.xlVAlignCenter
                End With

                '備考欄1〜5
                With .Range(.Cells(StartRowPosition + 6, 4), .Cells(StartRowPosition + 10, 4))
                    .Font.Size = 9
                    .HorizontalAlignment = XlHAlign.xlHAlignRight
                    .Orientation = XlOrientation.xlHorizontal
                End With
                '住所欄
                With .Range(.Cells(StartRowPosition + 7, 2), .Cells(StartRowPosition + 10, 2))
                    .Font.Size = 9
                    .Orientation = XlOrientation.xlHorizontal
                End With

                'お客様控え住所欄
                With .Range(.Cells(StartRowPosition + 10, 13), .Cells(StartRowPosition + 13, 13))
                    .Font.Size = 9
                    .Orientation = XlOrientation.xlHorizontal
                End With
                .Range(.Cells(StartRowPosition + 10, 13), .Cells(StartRowPosition + 12, 13)).HorizontalAlignment = XlHAlign.xlHAlignLeft
                .Cells(StartRowPosition + 13, 13).HorizontalAlignment = XlHAlign.xlHAlignRight
            End With

        End Sub

        Protected Overrides Function SetColumnSizes() As Double()
            Return {3.75, 25.13, 4, 1.75, 1.75, 1.75, 1.75, 1.75, 1.75, 1.75, 1.75, 6, 1.75, 1.75, 1.75, 1.75, 1.75, 1.75, 1.75, 1.75, 0.31}
        End Function

        Protected Overrides Function SetRowSizes() As Double()
            Return {272.25, 171.75, 19.5, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 101.25}
        End Function

        Public Overrides Sub SetCellFont()
            ExlWorkSheet.Cells.Font.Name = "ＭＳ Ｐ明朝"
        End Sub

    End Class

    ''' <summary>
    ''' 洋封筒クラス 
    ''' </summary>
    Private Class WesternEnvelope
        Inherits IExcelSheetSetting

        Public Overrides Sub SetCellFont()
            ExlWorkSheet.Cells.Font.Name = "HGP行書体"
        End Sub

        Protected Overrides Sub CellProperty()

            With ExlWorkSheet
                '郵便番号
                With .Range(.Cells(StartRowPosition + 2, 3), .Cells(StartRowPosition + 2, 9))
                    .Font.Size = 16
                    .VerticalAlignment = XlVAlign.xlVAlignTop
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                End With
                '住所
                With .Range(.Cells(StartRowPosition + 4, 6), .Cells(StartRowPosition + 4, 8))
                    .Font.Size = 24
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                    .Orientation = XlOrientation.xlVertical
                End With
                .Cells(StartRowPosition + 4, 8).verticalalignment = XlVAlign.xlVAlignTop
                .Cells(StartRowPosition + 4, 6).verticalalignment = XlVAlign.xlVAlignCenter
                '宛名
                With .Cells(StartRowPosition + 4, 2)
                    .font.size = 36
                    .horizontalalignment = XlHAlign.xlHAlignCenter
                    .verticalalignment = XlVAlign.xlVAlignTop
                    .orientation = XlOrientation.xlVertical
                End With
            End With

        End Sub

        Protected Overrides Sub CellsJoin()

            With ExlWorkSheet
                .Range(.Cells(StartRowPosition + 4, 2), .Cells(StartRowPosition + 4, 4)).Merge()
                .Range(.Cells(StartRowPosition + 4, 6), .Cells(StartRowPosition + 4, 7)).Merge()
                .Range(.Cells(StartRowPosition + 4, 8), .Cells(StartRowPosition + 4, 9)).Merge()
            End With

        End Sub

        Protected Overrides Sub DataOutput(addressee As AddresseeData)

            Dim addresstext1 As String = ""
            Dim addresstext2 As String = ""
            Dim addresseename As String

            With ExlWorkSheet
                .Cells.ClearContents()
                '郵便番号
                For I As Integer = 1 To 7
                    .Cells(StartRowPosition + 2, I + 2) = Mid(Replace(addressee.AddresseePostalCode, "-", ""), I, 1)
                Next

                '住所
                If Len(addressee.AddresseeAddress1 + addressee.AddresseeAddress2) < 14 Then
                    addresstext1 = addressee.AddresseeAddress1 & " " & addressee.AddresseeAddress2
                Else
                    addresstext1 = addressee.AddresseeAddress1
                    addresstext2 = addressee.AddresseeAddress2
                    If Len(addressee.AddresseeAddress2) > 16 Then .Cells(StartRowPosition + 4, 6).Interior.ColorIndex = 6
                End If
                .Cells(StartRowPosition + 4, 8) = addressee.AddresseeAddress1
                .Cells(StartRowPosition + 4, 6) = ConvertAddress(addressee.AddresseeAddress2)

                '宛名
                If Len(addressee.AddresseeName) > 5 Then
                    addresseename = addressee.AddresseeName & addressee.Title
                Else
                    addresseename = addressee.AddresseeName & " " & addressee.Title
                End If
                .Cells(StartRowPosition + 4, 2) = addresseename
            End With

        End Sub

        Protected Overrides Function SetColumnSizes() As Double()
            Return {17.88, 6, 2.75, 2.75, 2.75, 2.38, 2.38, 2.38, 2.38, 0.85}
        End Function

        Protected Overrides Function SetRowSizes() As Double()
            Return {22.5, 18.75, 27.75, 372}
        End Function
    End Class

    ''' <summary>
    ''' 角2封筒クラス
    ''' </summary>
    Private Class Kaku2Envelope
        Inherits IExcelSheetSetting

        Public Overrides Sub SetCellFont()
            ExlWorkSheet.Cells.Font.Name = "HGP行書体"
        End Sub

        Protected Overrides Sub CellProperty()

            With ExlWorkSheet
                '郵便番号
                With .Cells(StartRowPosition + 2, 3)
                    .font.size = 36
                    .horizontalalignment = XlHAlign.xlHAlignCenter
                    .verticalalignment = XlVAlign.xlVAlignBottom
                    .orientation = XlOrientation.xlHorizontal
                End With

                '住所
                With .Range(.Cells(StartRowPosition + 4, 5), .Cells(StartRowPosition + 4, 4))
                    .Font.Size = 43
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                    .VerticalAlignment = XlVAlign.xlVAlignTop
                    .Orientation = XlOrientation.xlVertical
                End With
                .Cells(StartRowPosition + 4, 4).verticalalignment = XlVAlign.xlVAlignCenter

                '宛名
                With .Cells(StartRowPosition + 4, 2)
                    .font.size = 85
                    .horizontalalignment = XlHAlign.xlHAlignCenter
                    .verticalalignment = XlVAlign.xlVAlignTop
                    .orientation = XlOrientation.xlVertical
                End With
            End With

        End Sub

        Protected Overrides Sub CellsJoin()
            With ExlWorkSheet
                .Range(.Cells(StartRowPosition + 2, 3), .Cells(StartRowPosition + 2, 5)).Merge()
                .Range(.Cells(StartRowPosition + 4, 2), .Cells(StartRowPosition + 5, 2)).Merge()
                .Range(.Cells(StartRowPosition + 4, 4), .Cells(StartRowPosition + 5, 4)).Merge()
                .Range(.Cells(StartRowPosition + 4, 5), .Cells(StartRowPosition + 5, 5)).Merge()
            End With
        End Sub

        Protected Overrides Sub DataOutput(addressee As AddresseeData)

            With ExlWorkSheet
                .Cells.ClearContents()
                '郵便番号
                .Cells(StartRowPosition + 2, 3) = "〒 " & addressee.AddresseePostalCode
                '住所
                .Cells(StartRowPosition + 4, 5) = addressee.AddresseeAddress1
                .Cells(StartRowPosition + 4, 4) = ConvertAddress(addressee.AddresseeAddress2)
                '宛名
                .Cells(StartRowPosition + 4, 2) = addressee.AddresseeName & " " & addressee.Title
            End With

        End Sub

        Protected Overrides Function SetColumnSizes() As Double()
            Return {38.13, 23.5, 15.38, 9.63, 9.63, 23.38}
        End Function

        Protected Overrides Function SetRowSizes() As Double()
            Return {120, 50.25, 61.5, 409.5, 407.25}
        End Function

    End Class

    ''' <summary>
    ''' 墓地パンフクラス
    ''' </summary>
    Private Class GravePamphlet
        Inherits IExcelSheetSetting

        Public Overrides Sub SetCellFont()
            ExlWorkSheet.Cells.Font.Name = "HGP行書体"
        End Sub

        Protected Overrides Sub CellProperty()

            With ExlWorkSheet
                '郵便番号
                With .Cells(StartRowPosition + 2, 3)
                    .font.size = 36
                    .horizontalalignment = XlHAlign.xlHAlignCenter
                    .verticalalignment = XlVAlign.xlVAlignBottom
                    .orientation = XlOrientation.xlHorizontal
                End With

                '住所
                With .Range(.Cells(StartRowPosition + 4, 5), .Cells(StartRowPosition + 4, 4))
                    .Font.Size = 43
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                    .VerticalAlignment = XlVAlign.xlVAlignTop
                    .Orientation = XlOrientation.xlVertical
                End With
                .Cells(StartRowPosition + 4, 4).verticalalignment = XlVAlign.xlVAlignCenter

                '宛名
                With .Cells(StartRowPosition + 4, 2)
                    .font.size = 85
                    .horizontalalignment = XlHAlign.xlHAlignCenter
                    .verticalalignment = XlVAlign.xlVAlignTop
                    .orientation = XlOrientation.xlVertical
                End With
            End With

        End Sub

        Protected Overrides Sub CellsJoin()
            With ExlWorkSheet
                .Range(.Cells(StartRowPosition + 2, 3), .Cells(StartRowPosition + 2, 5)).Merge()
                .Range(.Cells(StartRowPosition + 4, 2), .Cells(StartRowPosition + 5, 2)).Merge()
                .Range(.Cells(StartRowPosition + 4, 4), .Cells(StartRowPosition + 5, 4)).Merge()
                .Range(.Cells(StartRowPosition + 4, 5), .Cells(StartRowPosition + 5, 5)).Merge()
            End With
        End Sub

        Protected Overrides Sub DataOutput(addressee As AddresseeData)

            With ExlWorkSheet
                .Cells.ClearContents()
                '郵便番号
                .Cells(StartRowPosition + 2, 3) = "〒 " & addressee.AddresseePostalCode
                '住所
                .Cells(StartRowPosition + 4, 5) = addressee.AddresseeAddress1
                .Cells(StartRowPosition + 4, 4) = ConvertAddress(addressee.AddresseeAddress2)
                '宛名
                .Cells(StartRowPosition + 4, 2) = addressee.AddresseeName & " " & addressee.Title
            End With

        End Sub

        Protected Overrides Function SetColumnSizes() As Double()
            Return {41.88, 23.5, 30.25, 9.63, 8.5}
        End Function

        Protected Overrides Function SetRowSizes() As Double()
            Return {71.25, 132.75, 51, 409.5, 409.5, 6.75}
        End Function

    End Class

    ''' <summary>
    ''' はがきクラス
    ''' </summary>
    Private Class Postcard
        Inherits IExcelSheetSetting

        Public Overrides Sub SetCellFont()
            ExlWorkSheet.Cells.Font.Name = "HGP行書体"
        End Sub

        Protected Overrides Sub CellProperty()

            With ExlWorkSheet
                '郵便番号
                With .Range(.Cells(StartRowPosition + 2, 3), .Cells(StartRowPosition + 2, 10))
                    .Font.Size = 16
                    .VerticalAlignment = XlVAlign.xlVAlignTop
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                End With
                '住所
                With .Range(.Cells(StartRowPosition + 4, 7), .Cells(StartRowPosition + 4, 9))
                    .Font.Size = 18
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                    .Orientation = XlOrientation.xlVertical
                End With
                .Cells(StartRowPosition + 4, 9).verticalalignment = XlVAlign.xlVAlignTop
                .Cells(StartRowPosition + 4, 7).verticalalignment = XlVAlign.xlVAlignCenter
                '宛名
                With .Cells(StartRowPosition + 4, 2)
                    .font.size = 36
                    .horizontalalignment = XlHAlign.xlHAlignCenter
                    .verticalalignment = XlVAlign.xlVAlignTop
                    .orientation = XlOrientation.xlVertical
                End With
            End With

        End Sub

        Protected Overrides Sub CellsJoin()

            With ExlWorkSheet
                .Range(.Cells(StartRowPosition + 4, 2), .Cells(StartRowPosition + 4, 5)).Merge()
                .Range(.Cells(StartRowPosition + 4, 9), .Cells(StartRowPosition + 4, 10)).Merge()
                .Range(.Cells(StartRowPosition + 4, 7), .Cells(StartRowPosition + 4, 8)).Merge()
            End With

        End Sub

        Protected Overrides Sub DataOutput(addressee As AddresseeData)

            Dim addresstext1 As String = ""
            Dim addresstext2 As String = ""
            Dim addresseename As String

            With ExlWorkSheet
                .Cells.ClearContents()
                '郵便番号
                For I As Integer = 1 To 8
                    If I = 4 Then Continue For
                    .Cells(StartRowPosition + 2, I + 2) = Mid(addressee.AddresseePostalCode, I, 1)
                Next

                '住所
                If Len(addressee.AddresseeAddress1 + addressee.AddresseeAddress2) < 14 Then
                    addresstext1 = addressee.AddresseeAddress1 & " " & addressee.AddresseeAddress2
                Else
                    addresstext1 = addressee.AddresseeAddress1
                    addresstext2 = addressee.AddresseeAddress2
                    If Len(addressee.AddresseeAddress2) > 14 Then .Cells(StartRowPosition + 4, 6).Interior.ColorIndex = 6
                End If
                .Cells(StartRowPosition + 4, 7) = addressee.AddresseeAddress1
                .Cells(StartRowPosition + 4, 9) = ConvertAddress(addressee.AddresseeAddress2)

                '宛名
                If Len(addressee.AddresseeName) > 5 Then
                    addresseename = addressee.AddresseeName & addressee.Title
                Else
                    addresseename = addressee.AddresseeName & " " & addressee.Title
                End If
                .Cells(StartRowPosition + 4, 2) = addresseename
            End With

        End Sub

        Protected Overrides Function SetColumnSizes() As Double()
            Return {16, 3.63, 2.75, 2.75, 2.75, 0.62, 2.75, 2.75, 2.75, 2.75, 0.77}
        End Function

        Protected Overrides Function SetRowSizes() As Double()
            Return {30, 22.5, 22.5, 360.75}
        End Function
    End Class

    Private Class LabelSheet
        Inherits IExcelSheetSetting

        Public Overrides Sub SetCellFont()
            ExlWorkSheet.Cells.Font.Name = "ＭＳ Ｐゴシック"
        End Sub

        Protected Overrides Sub CellProperty()

            With ExlWorkSheet
                .Cells.Font.Size = 10
                .Cells.VerticalAlignment = XlVAlign.xlVAlignCenter
                .Cells.Orientation = XlOrientation.xlHorizontal
            End With

        End Sub

        Protected Overrides Sub CellsJoin()
            'ラベル用紙のセル結合はない
        End Sub

        Protected Overrides Sub DataOutput(addressee As AddresseeData)

            Dim ColumnIndex As Integer = 1
            Dim LineIndex As Integer = 1

            With ExlWorkSheet
                Dim test As String = .Cells(LineIndex, ColumnIndex).text
                Do Until Len(Trim(.Cells(LineIndex, ColumnIndex).text)) = 0
                    ColumnIndex += 1

                    If ColumnIndex > 3 Then
                        ColumnIndex = 1
                        LineIndex += 1
                    End If
                Loop

                .Cells(LineIndex, ColumnIndex) = ReturnLabelString(LineIndex, addressee)

            End With

        End Sub

        ''' <summary>
        ''' ラベルに入力する文字列を返します
        ''' </summary>
        ''' <param name="lineindex">行番号</param>
        ''' <param name="addressee">ラベル化する宛先</param>
        ''' <returns></returns>
        Private Function ReturnLabelString(ByVal lineindex As Integer, ByVal addressee As AddresseeData) As String

            Dim ReturnString As String = "　　　　〒 " & addressee.AddresseePostalCode & vbNewLine & vbNewLine
            ReturnString &= "　　　　" & CutAddress(addressee.AddresseeAddress1) & vbCrLf
            ReturnString &= "　　　　" & Mid(addressee.AddresseeAddress2, 1, 16) & vbCrLf
            ReturnString &= "　　　　" & Mid(addressee.AddresseeAddress2, 17) & vbCrLf & vbCrLf
            ReturnString &= "　　　　" & addressee.AddresseeName & " " & addressee.Title & vbCrLf

            If lineindex Mod 6 = 0 Then
                ReturnString = vbNewLine & vbNewLine & ReturnString
                Return ReturnString
            End If

            If lineindex Mod 7 = 0 Then ReturnString = vbNewLine & vbNewLine & vbNewLine & ReturnString

            Return ReturnString

        End Function

        Protected Overrides Function SetColumnSizes() As Double()
            Return {30.5, 30.5, 30.25}
        End Function

        Protected Overrides Function SetRowSizes() As Double()
            Return {118.5, 118.5, 118.5, 118.5, 118.5, 118.5, 118.5}
        End Function

    End Class

End Class
