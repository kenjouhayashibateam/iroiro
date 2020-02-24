Imports Microsoft.Office.Interop.Excel
Imports Domain
Imports System.Text.RegularExpressions

''' <summary>
''' 住所を宛先用に変換します
''' </summary>
Interface IAddressConvert

    ''' <summary>
    ''' 宛先用住所1を返します
    ''' </summary>
    ''' <returns></returns>
    Function GetConvertAddress1() As String

    ''' <summary>
    ''' 宛先用住所2を返します
    ''' </summary>
    ''' <returns></returns>
    Function GetConvertAddress2() As String

End Interface

''' <summary>
''' エクセルに出力する際の共通動作
''' </summary>
Interface IExcelOutputBehavior

    ''' <summary>
    ''' シート全体のフォントを設定します
    ''' </summary>
    Sub SetCellFont()

    ''' <summary>
    ''' セルのフォントサイズ、フォントポジション等を設定します
    ''' </summary>
    ''' <param name="startrowposition"></param>
    Sub CellProperty(ByVal startrowposition As Integer)

    ''' <summary>
    ''' カラムのサイズを格納した配列を返します
    ''' </summary>
    ''' <returns></returns>
    Function SetColumnSizes() As Double()

    ''' <summary>
    ''' Rowのサイズを格納した配列を返します
    ''' </summary>
    ''' <returns></returns>
    Function SetRowSizes() As Double()

    ''' <summary>
    ''' エクセルに出力するジャンルを返します
    ''' </summary>
    ''' <returns></returns>
    Function GetDataName() As String

End Interface

''' <summary>
''' エクセルデータを横向けに出力
''' </summary>
Interface IHorizontalOutputBehavior
    Inherits IExcelOutputBehavior

    ''' <summary>
    ''' 出力するデータをセットします
    ''' </summary>
    Sub SetData()

End Interface

''' <summary>
''' エクセルデータを縦向けに出力
''' </summary>
Interface IVerticalOutputBehavior
    Inherits IExcelOutputBehavior

    ''' <summary>
    ''' 出力するデータをセットします
    ''' </summary>
    ''' <param name="startrowposition"></param>
    Sub SetData(ByVal startrowposition As Integer)

    ''' <summary>
    ''' 結合するセルを設定します
    ''' </summary>
    ''' <param name="startrowposition"></param>
    Sub CellsJoin(ByVal startrowposition As Integer)

    ''' <summary>
    ''' 必ず入力されるデータ（宛名）のセル位置を設定するための行番号
    ''' </summary>
    ''' <returns></returns>
    Function CriteriaCellRowIndex() As Integer

    ''' <summary>
    ''' 必ず入力されるデータ（宛名）のセル位置を設定するための列番号
    ''' </summary>
    ''' <returns></returns>
    Function CriteriaCellColumnIndex() As Integer

End Interface

''' <summary>
''' 住所変換クラス
''' </summary>
Public Class AddressConvert
    Implements IAddressConvert

    Private Property Address1 As String
    Private Property Address2 As String

    Sub New(ByVal _address1 As String, ByVal _address2 As String)
        Address1 = _address1
        Address2 = _address2
    End Sub

    ''' <summary>
    ''' 住所の都道府県を省略できる住所は、都道府県を除いて返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetConvertAddress1() As String Implements IAddressConvert.GetConvertAddress1

        Dim AddressText As String

        AddressText = Address1
        '東京、神奈川、徳島は略す
        AddressText = Replace(AddressText, "東京都", "")
        AddressText = Replace(AddressText, "神奈川県", "")
        AddressText = Replace(AddressText, "徳島県", "")
        If Address1.Length <> AddressText.Length Then Return AddressText

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

        '検証する文字列の名称、京都府や広島県等と市の名称、京都市、広島市などが同じなら省略する
        If address.Substring(0, InStr(1, address, verifystring) - 1) = address.Substring(InStr(1, address, verifystring),
                                                                          InStr(1, address, "市") - InStr(1, address, verifystring) - 1) Then
            Return address.Substring(InStr(0, address, verifystring) + 1)
        End If

        Return address

    End Function

    ''' <summary>
    ''' 住所2の番地を漢字に変換して返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetConvertAddress2() As String Implements IAddressConvert.GetConvertAddress2

        Dim VerificationString As New Regex("[1-9１-９－-]") '数字、ハイフンを検証する正規表現
        Dim I As Integer

        '一文字ずつ正規表現か検証して、正規表現にマッチする最初の部分を I で保持する
        For I = 1 To Address2.Length
            If VerificationString.IsMatch(Address2.Substring(I - 1, 1)) Then Exit For
        Next

        Dim addressstring As String = Address2.Substring(0, I - 1)  '番地までの住所
        Dim addressblock As String = Address2.Substring(I - 1)      '番地からの住所
        addressblock = StrConv(addressblock, vbWide)

        '住所2の数字の位置からハイフンを基準に文字列を分割して格納する
        Dim addressarray() As String = Split(addressblock, "－")
        '配列に格納したら、addressblockを漢字変換した番地を格納する
        addressblock = String.Empty
        For I = 0 To UBound(addressarray)
            '番地の数字の部分を漢字変換し、マッチしない文字列が出てきたら、ConvertArrayブロックに移動する。
            If VerificationString.IsMatch(addressarray(I)) Then
                addressblock &= ConvertNumber(addressarray(I)) & "－"
            Else
                GoTo ConvertArray
            End If
        Next

        GoTo EndPart

ConvertArray:   '配列の文字列を一文字ずつ検証して漢字変換する
        Dim J As Integer = 0

        Do Until J + 1 = addressarray(I).Length
            If VerificationString.IsMatch(addressarray(I).Substring(J, 1)) Then addressblock &= ConvertNumber(addressarray(I).Substring(J, 1)) & "－"
            J += 1
        Loop

EndPart:    'ハイフンを変換して、最後のハイフンを除いた文字列を返す
        addressblock = Replace(addressblock, "－", "ー")
        Return addressstring & addressblock.Substring(0, addressblock.Length - 1)

    End Function

    ''' <summary>
    ''' 数字を漢字変換して返します
    ''' </summary>
    ''' <param name="mynumber">変換する数字</param>
    ''' <returns></returns>
    Private Function ConvertNumber(ByVal mynumber As Integer) As String
        Select Case mynumber
            Case < 11   '10まで
                Return ConvertNumber_Under10(mynumber)
            Case < 20   '19まで
                Return ConvertNumber_Over11Under19(mynumber)
            Case Else   '20以上
                Return ConvertNumber_Orver20(mynumber)
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

        Dim myValue As String = ""

        '一桁ごとに漢字変換する
        For I As Integer = 1 To myNumber.ToString.Length
            myValue &= ConvertNumber_Under10(myNumber.ToString.Substring(I - 1, 1))
        Next

        '漢字2文字でなければキリ番ではないので、そのまま返す
        If myValue.ToString.Length <> 2 Then Return myValue

        '20、30などの数字を〇から十に変える
        If myValue.Substring(1, 1) = "〇" Then myValue = myValue.Substring(0, 1) & "十"

        Return myValue

    End Function

End Class

''' <summary>
''' エクセルへの処理を行います
''' </summary>
Public Class ExcelOutputInfrastructure
    Implements IAdresseeOutputRepogitory

    ''' <summary>
    ''' 出力するデータの種類を保持する
    ''' </summary>
    ''' <returns></returns>
    Private Shared Property OutputDataGanre As String

    ''' <summary>
    ''' 宛先データ
    ''' </summary>
    Private Property MyAddressee As AddresseeData

    ''' <summary>
    ''' エクセルアプリケーション
    ''' </summary>
    Private Property ExlApp As Application

    ''' <summary>
    ''' ワークブック
    ''' </summary>
    Private Shared ExlWorkbook As Workbook

    ''' <summary>
    ''' 印刷物を発行するエクセルの列のサイズを配列で保持します。
    ''' </summary>
    Private ColumnSizes() As Double

    ''' <summary>
    ''' 印刷物を発行するエクセルの行のサイズを配列で保持します。
    ''' </summary>
    Private RowSizes() As Double

    ''' <summary>
    ''' 複数データを印刷する際の各入力データの一番上の数値を設定します
    ''' </summary>
    Private StartRowPosition As Integer

    ''' <summary>
    ''' ワークシート
    ''' </summary>
    Private Shared ExlWorkSheet As Worksheet

    Private Vob As IVerticalOutputBehavior

    Private Hob As IHorizontalOutputBehavior

    ''' <summary>
    ''' エクセルを起動して、アプリ用のブックを開きます
    ''' </summary>
    Private Sub SheetSetting()

        Try
            ExlApp = GetObject(, "Excel.Application")
        Catch ex As Exception
            ExlApp = CreateObject("Excel.Application")
        End Try

        If ExlWorkbook Is Nothing Then ExlWorkbook = ExlApp.Workbooks.Add

        ExlApp.Visible = True
        ExlWorkSheet = ExlWorkbook.Sheets(1)
        ExlWorkSheet.Activate()

    End Sub

    ''' <summary>
    ''' 入力するでーたの印刷範囲の一番上のRowを返します
    ''' </summary>
    ''' <returns></returns>
    Private Function SetStartRowPosition() As Integer

        Dim addint As Integer = UBound(RowSizes)    '一回に移動する数字。印刷データの１ページ分移動します
        Dim index As Integer = 0    '印刷データの件数
        Dim column As Integer = Vob.CriteriaCellColumnIndex '入力時に必ず値が入っているセルのColumn
        Dim row As Integer = Vob.CriteriaCellRowIndex   '入力時に必ず値が入っているセルのRow

        '入力時に必ず値が入っているセルに文字列があればインデックスをプラスする
        Do Until ExlWorkSheet.Cells((index * addint) + row, column).Text = String.Empty
            index += 1
        Loop

        'インデックス×ページRowで、スタートの位置が割り出せる
        Return index * addint

    End Function

    ''' <summary>
    ''' 横向けにデータを入力する処理。ラベル用紙用
    ''' </summary>
    ''' <param name="_hob"></param>
    Private Sub OutputHorizontalProcessing(ByVal _hob As IHorizontalOutputBehavior)

        Hob = _hob

        SheetSetting()

        ColumnSizes = Hob.SetColumnSizes()
        RowSizes = Hob.SetRowSizes()

        Dim column As Integer = 1
        Dim row As Integer = 1
        Dim sheetindex As Integer = 0

        With ExlWorkSheet
            '出力するデータの種類が違えばセルをクリアする
            If OutputDataGanre <> Hob.GetDataName Then
                OutputDataGanre = Hob.GetDataName
                .Cells.Clear()
            End If

            'ラベルのマスに値がない初めの位置と、ラベル件数からページ数を割り出し設定する
            Dim linecount As Integer = 1
            Do Until .Cells(row, column).Text = String.Empty
                column += 1
                linecount += 1
                'カラムが4なら改行する
                If column > 3 Then
                    column = 1
                    row += 1
                End If
                'linecountが8ならページインデックスをプラスする
                If linecount = 8 Then
                    sheetindex += 1
                    linecount = 1
                End If
            Loop

            'カラムの幅を設定する
            For i As Integer = 0 To UBound(ColumnSizes)
                .Columns(i + 1).columnwidth = ColumnSizes(i)
            Next
            'ロウの高さを設定する
            For j As Integer = 0 To UBound(RowSizes)
                .Rows((j + 1) + sheetindex * UBound(RowSizes)).rowheight = RowSizes(j)
            Next
        End With

        Hob.CellProperty(sheetindex)
        Hob.SetCellFont()
        Hob.SetData()

    End Sub

    ''' <summary>
    ''' 縦向けにデータを入力する処理
    ''' </summary>
    ''' <param name="_vob"></param>
    ''' <param name="ismulti">複数印刷Behaviorをするかを設定します</param>
    Private Sub OutputVerticalProcessing(ByVal _vob As IVerticalOutputBehavior, ByVal ismulti As Boolean)

        Vob = _vob

        SheetSetting()

        ColumnSizes = Vob.SetColumnSizes()
        RowSizes = Vob.SetRowSizes()

        '複数印刷するならポジションを設定
        If ismulti Then
            StartRowPosition = SetStartRowPosition()
        Else
            StartRowPosition = 0
        End If

        With ExlWorkSheet
            '出力するデータの種類が違えばセルをクリアする
            If OutputDataGanre <> Vob.GetDataName Then
                OutputDataGanre = Vob.GetDataName
                .Cells.Clear()
                'ColumnSizesの配列の中の数字をシートのカラムの幅に設定する
                For I As Integer = 0 To UBound(ColumnSizes)
                    .Columns(I + 1).ColumnWidth = ColumnSizes(I)
                Next
            End If
            If Not ismulti Then .Cells.UnMerge()

            Vob.CellProperty(StartRowPosition)

            'RowSizesの配列の中の数字をシートのローの幅に設定する
            For I = 0 To UBound(RowSizes)
                .Rows(StartRowPosition + (I + 1)).RowHeight = RowSizes(I)
            Next
        End With

        SetMargin()
        Vob.SetCellFont()
        Vob.CellsJoin(StartRowPosition)
        Vob.SetData(StartRowPosition)

    End Sub

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

    Public Sub TransferPaperPrintOutput(addressee As String, title As String, postalcode As String, address1 As String, address2 As String,
                                        money As String, note1 As String, note2 As String, note3 As String, note4 As String,
                                        note5 As String, multioutput As Boolean) Implements IAdresseeOutputRepogitory.TransferPaperPrintOutput

        MyAddressee = New AddresseeData(addressee, title, postalcode, address1, address2, money, note1, note2, note3, note4, note5)
        Dim tp As IVerticalOutputBehavior = New TransferPaper(MyAddressee)
        OutputVerticalProcessing(tp, multioutput)

    End Sub

    Public Sub LabelOutput(addressee As String, title As String, postalcode As String, address1 As String, address2 As String) Implements IAdresseeOutputRepogitory.LabelOutput

        MyAddressee = New AddresseeData(addressee, title, postalcode, address1, address2)
        Dim ls As IHorizontalOutputBehavior = New LabelSheet(MyAddressee)
        OutputHorizontalProcessing(ls)

    End Sub

    Public Sub Cho3EnvelopeOutput(addressee As String, title As String, postalcode As String, address1 As String, address2 As String,
                                  multioutput As Boolean) Implements IAdresseeOutputRepogitory.Cho3EnvelopeOutput

        MyAddressee = New AddresseeData(addressee, title, postalcode, address1, address2)
        Dim ce As IVerticalOutputBehavior = New Cho3Envelope(MyAddressee)
        OutputVerticalProcessing(ce, multioutput)

    End Sub

    Public Sub WesternEnvelopeOutput(addressee As String, title As String, postalcode As String, address1 As String, address2 As String, multioutput As Boolean) Implements IAdresseeOutputRepogitory.WesternEnvelopeOutput

        MyAddressee = New AddresseeData(addressee, title, postalcode, address1, address2)
        Dim we As IVerticalOutputBehavior = New WesternEnvelope(MyAddressee)
        OutputVerticalProcessing(we, multioutput)

    End Sub

    Public Sub Kaku2EnvelopeOutput(addressee As String, title As String, postalcode As String, address1 As String, address2 As String, multioutput As Boolean) Implements IAdresseeOutputRepogitory.Kaku2EnvelopeOutput

        MyAddressee = New AddresseeData(addressee, title, postalcode, address1, address2)
        Dim ke As IVerticalOutputBehavior = New Kaku2Envelope(MyAddressee)
        OutputVerticalProcessing(ke, multioutput)

    End Sub

    Public Sub GravePamphletOutput(addressee As String, title As String, postalcode As String, address1 As String, address2 As String, multioutput As Boolean) Implements IAdresseeOutputRepogitory.GravePamphletOutput

        MyAddressee = New AddresseeData(addressee, title, postalcode, address1, address2)
        Dim gp As IVerticalOutputBehavior = New GravePamphletEnvelope(MyAddressee)
        OutputVerticalProcessing(gp, multioutput)

    End Sub

    Public Sub PostcardOutput(addressee As String, title As String, postalcode As String, address1 As String, address2 As String, multioutput As Boolean) Implements IAdresseeOutputRepogitory.PostcardOutput

        MyAddressee = New AddresseeData(addressee, title, postalcode, address1, address2)
        Dim pc As IVerticalOutputBehavior = New Postcard(MyAddressee)
        OutputVerticalProcessing(pc, multioutput)

    End Sub

    ''' <summary>
    ''' 長3封筒クラス
    ''' </summary>
    Private Class Cho3Envelope
        Implements IVerticalOutputBehavior

        Private ReadOnly myAddressee As AddresseeData

        Sub New(ByVal _addressee As AddresseeData)
            myAddressee = _addressee
        End Sub

        Public Sub SetData(startrowposition As Integer) Implements IVerticalOutputBehavior.SetData

            Dim addresseename As String

            With ExlWorkSheet
                '郵便番号
                For I As Integer = 1 To 7
                    .Cells(startrowposition + 2, I + 2) = Replace(myAddressee.AddresseePostalCode, "-", "").Substring(I - 1, 1)
                Next

                '住所
                Dim addresstext1 As String = String.Empty
                Dim addresstext2 As String = String.Empty
                Dim ac As New AddressConvert(myAddressee.AddresseeAddress1, myAddressee.AddresseeAddress2)
                addresstext1 = myAddressee.AddresseeAddress1
                addresstext2 = myAddressee.AddresseeAddress2
                If addresstext1.Length + addresstext2.Length < 15 Then
                    addresstext1 = myAddressee.AddresseeAddress1 & " " & myAddressee.AddresseeAddress2
                    addresstext2 = String.Empty
                Else
                    .Cells(startrowposition + 4, 9).Interior.ColorIndex = 6
                End If
                .Cells(startrowposition + 4, 8) = ac.GetConvertAddress1
                .Cells(startrowposition + 4, 6) = ac.GetConvertAddress2

                '宛名
                If myAddressee.AddresseeName.Length > 5 Then
                    addresseename = myAddressee.AddresseeName & myAddressee.Title
                Else
                    addresseename = myAddressee.AddresseeName & " " & myAddressee.Title
                End If
                .Cells(startrowposition + 4, 2) = addresseename
            End With

        End Sub

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin
            With ExlWorkSheet
                '住所欄1行目
                .Range(.Cells(startrowposition + 4, 8), .Cells(startrowposition + 5, 9)).Merge()
                '住所欄2行目
                .Range(.Cells(startrowposition + 4, 6), .Cells(startrowposition + 5, 7)).Merge()
                '宛名欄
                .Range(.Cells(startrowposition + 4, 2), .Cells(startrowposition + 5, 3)).Merge()
            End With
        End Sub

        Protected Sub SetCellFont() Implements IVerticalOutputBehavior.SetCellFont
            ExlWorkSheet.Cells.Font.Name = "HGP行書体"
        End Sub

        Private Function ColumnSizes() As Double() Implements IVerticalOutputBehavior.SetColumnSizes
            Return {17.13, 7.5, 2.75, 2.75, 2.75, 2.38, 2.38, 2.38, 2.38, 1.75}
        End Function

        Private Function SetRowSizes() As Double() Implements IVerticalOutputBehavior.SetRowSizes
            Return {101.25, 38.25, 14.25, 409.5, 133.5, 36}
        End Function

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                '宛名
                With .Cells(startrowposition + 4, 2)
                    .font.size = 48
                    .HorizontalAlignment = XlHAlign.xlHAlignRight
                    .verticalalignment = XlVAlign.xlVAlignTop
                    .Orientation = XlOrientation.xlVertical
                End With
                '郵便番号
                With .Range(.Cells(startrowposition + 2, 3), .Cells(startrowposition + 2, 9))
                    .Font.Size = 16
                    .VerticalAlignment = XlVAlign.xlVAlignTop
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                    .Orientation = XlOrientation.xlVertical
                End With

                '住所
                With .Range(.Cells(startrowposition + 4, 6), .Cells(startrowposition + 4, 9))
                    .Font.Size = 30
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                    .Orientation = XlOrientation.xlVertical
                End With
                .Cells(startrowposition + 4, 7).verticalalignment = XlVAlign.xlVAlignCenter
                .Cells(startrowposition + 4, 8).verticalalignment = XlVAlign.xlVAlignTop
            End With

        End Sub

        Public Function CriteriaCellRowIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellRowIndex
            Return 4
        End Function

        Public Function CriteriaCellColumnIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellColumnIndex
            Return 2
        End Function

        Public Function GetDataName() As String Implements IVerticalOutputBehavior.GetDataName
            Return ToString()
        End Function
    End Class

    ''' <summary>
    ''' 振込用紙発行クラス
    ''' </summary>
    Private Class TransferPaper
        Implements IVerticalOutputBehavior

        Private ReadOnly myAddressee As AddresseeData

        Sub New(ByVal addressee As AddresseeData)
            myAddressee = addressee
        End Sub

        ''' <summary>
        ''' お客様控えの住所を分けて表示させるための文字列の配列を返します
        ''' </summary>
        ''' <param name="address1">住所1</param>
        ''' <param name="address2">住所2</param>
        ''' <returns></returns>
        Private Function SplitYourCopyAddress(ByVal address1 As String, ByVal address2 As String) As String()

            Dim line1, line2, line3, joinaddress As String

            '住所をつなげる
            joinaddress = address1 & address2

            'つなげた住所の文字列が長ければ関数を呼び出し値を返す
            If joinaddress.Length > 24 Then Return ReturnLongAddressArray(joinaddress)

            '住所1が長ければ2行に分ける
            If address1.Length < 12 Then
                line1 = address1
                line2 = String.Empty
            Else
                line1 = address1.Substring(0, 12)
                line2 = address1.Substring(12)
            End If

            '住所2が長ければ2行に分ける
            If address2.Length + line2.Length < 12 Then
                line2 &= address2
                line3 = String.Empty
            Else
                line2 &= address2.Substring(0, 12)
                line3 = address2.Substring(12)
            End If

            Return {line1, line2, line3}

        End Function

        ''' <summary>
        ''' 長い住所を区切ります。1行目を住所2の文字も使用して3行で表示させます。
        ''' </summary>
        ''' <param name="absolutenessaddress"></param>
        ''' <returns></returns>
        Private Function ReturnLongAddressArray(ByVal absolutenessaddress As String) As String()

            Dim line1, line2, line3 As String

            line1 = absolutenessaddress.Substring(0, 12)
            line2 = absolutenessaddress.Substring(12, 12)
            line3 = absolutenessaddress.Substring(24)

            Return {line1, line2, line3}

        End Function

        Private Sub SetCellFont() Implements IVerticalOutputBehavior.SetCellFont
            ExlWorkSheet.Cells.Font.Name = "ＭＳ Ｐ明朝"
        End Sub

        Private Function IExcelOutputBehavior_SetColumnSizes1() As Double() Implements IVerticalOutputBehavior.SetColumnSizes
            Return {3.75, 25.13, 4, 1.75, 1.75, 1.75, 1.75, 1.75, 1.75, 1.75, 1.75, 6, 1.75, 1.75, 1.75, 1.75, 1.75, 1.75, 1.75, 1.75, 0.31}
        End Function

        Private Function IExcelOutputBehavior_SetRowSizes1() As Double() Implements IVerticalOutputBehavior.SetRowSizes
            Return {272.25, 171.75, 19.5, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 101.25}
        End Function

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                .PageSetup.PaperSize = XlPaperSize.xlPaperB5
                '宛名欄
                .Cells(11, 2).Font.Size = 14
                '金額欄
                With .Range(.Cells(startrowposition + 3, 4), .Cells(startrowposition + 3, 11))
                    .Font.Size = 14
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                    .VerticalAlignment = XlVAlign.xlVAlignCenter
                End With

                'お客様控え金額欄
                With .Range(.Cells(startrowposition + 9, 13), .Cells(startrowposition + 9, 20))
                    .Font.Size = 14
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                    .VerticalAlignment = XlVAlign.xlVAlignCenter
                End With

                '備考欄1〜5
                With .Range(.Cells(startrowposition + 6, 4), .Cells(startrowposition + 10, 4))
                    .Font.Size = 9
                    .HorizontalAlignment = XlHAlign.xlHAlignRight
                    .Orientation = XlOrientation.xlHorizontal
                End With
                '住所欄
                With .Range(.Cells(startrowposition + 7, 2), .Cells(startrowposition + 10, 2))
                    .Font.Size = 9
                    .Orientation = XlOrientation.xlHorizontal
                End With

                'お客様控え住所欄
                With .Range(.Cells(startrowposition + 10, 13), .Cells(startrowposition + 13, 13))
                    .Font.Size = 9
                    .Orientation = XlOrientation.xlHorizontal
                End With
                .Range(.Cells(startrowposition + 10, 13), .Cells(startrowposition + 12, 13)).HorizontalAlignment = XlHAlign.xlHAlignLeft
                .Cells(startrowposition + 13, 13).HorizontalAlignment = XlHAlign.xlHAlignRight
            End With

        End Sub

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin

            Dim row As Integer

            '宛名備考欄5行を結合
            row = 6
            Do Until row = 11
                With ExlWorkSheet
                    .Range(.Cells(startrowposition + row, 4), .Cells(startrowposition + row, 11)).Merge()
                End With
                row += 1
            Loop

            'お客様控え欄4行を結合
            row = 10
            Do Until row = 14
                With ExlWorkSheet
                    .Range(.Cells(startrowposition + row, 13), .Cells(startrowposition + row, 20)).Merge()
                End With
                row += 1
            Loop

        End Sub

        Public Sub SetData(startrowposition As Integer) Implements IVerticalOutputBehavior.SetData

            With ExlWorkSheet
                .Cells.ClearContents()
                '振込金額入力
                Dim ColumnIndex As Integer = 0
                Dim moneystring As String = "\" & myAddressee.Money
                Do Until ColumnIndex = moneystring.Length
                    .Cells(startrowposition + 3, 11 - ColumnIndex) = moneystring.Substring((moneystring.Length - 1) - ColumnIndex, 1)
                    .Cells(startrowposition + 9, 20 - ColumnIndex) = moneystring.Substring((moneystring.Length - 1) - ColumnIndex, 1)    'お客様控え
                    ColumnIndex += 1
                Loop

                .Cells(startrowposition + 6, 4) = myAddressee.Note1   '備考1
                .Cells(startrowposition + 7, 4) = myAddressee.Note2   '備考2
                .Cells(startrowposition + 8, 4) = myAddressee.Note3   '備考3
                .Cells(startrowposition + 9, 4) = myAddressee.Note4   '備考4
                .Cells(startrowposition + 10, 4) = myAddressee.Note5  '備考5
                .Cells(startrowposition + 7, 2) = "〒 " & myAddressee.AddresseePostalCode      '宛先郵便番号
                Dim ac As New AddressConvert(myAddressee.AddresseeAddress1, myAddressee.AddresseeAddress2)
                .Cells(startrowposition + 8, 2) = ac.GetConvertAddress1         '宛先住所1

                Dim stringlength As Integer
                If myAddressee.AddresseeAddress2.Length < 20 Then
                    stringlength = myAddressee.AddresseeAddress2.Length
                Else
                    stringlength = 20
                End If
                '宛先住所2　長い場合は2行で入力
                .Cells(startrowposition + 9, 2) = myAddressee.AddresseeAddress2.Substring(0, stringlength)
                If myAddressee.AddresseeAddress2.Length > 20 Then .Cells(startrowposition + 9, 2) = myAddressee.AddresseeAddress2.Substring(20)

                .Cells(startrowposition + 11, 2) = myAddressee.AddresseeName & " " & myAddressee.Title  '宛先の宛名
                .Cells(startrowposition + 13, 13) = myAddressee.AddresseeName & " " & myAddressee.Title 'お客様控えの名前

                'お客様控え住所　長い場合は3行、それでも収まらない場合は注意を促す
                Dim strings() As String = SplitYourCopyAddress(ac.GetConvertAddress1, myAddressee.AddresseeAddress2)
                .Cells(startrowposition + 10, 13) = " " & strings(0)
                .Cells(startrowposition + 11, 13) = " " & strings(1)
                .Cells(startrowposition + 12, 13) = " " & strings(2)
                If strings(2).Length > 13 Then MsgBox("住所がセルからはみ出てますので、書き直して下さい", MsgBoxStyle.Critical, "文字数オーバー")
            End With

        End Sub

        Public Function CriteriaCellRowIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellRowIndex
            Return 2
        End Function

        Public Function CriteriaCellColumnIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellColumnIndex
            Return 4
        End Function

        Public Function GetDataName() As String Implements IVerticalOutputBehavior.GetDataName
            Return ToString()
        End Function
    End Class

    ''' <summary>
    ''' 洋封筒クラス 
    ''' </summary>
    Private Class WesternEnvelope
        Implements IVerticalOutputBehavior

        Private ReadOnly myAddressee As AddresseeData

        Sub New(ByVal addressee As AddresseeData)
            myAddressee = addressee
        End Sub

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                .PageSetup.PaperSize = XlPaperSize.xlPaperEnvelopeC6
                '郵便番号
                With .Range(.Cells(startrowposition + 2, 3), .Cells(startrowposition + 2, 9))
                    .Font.Size = 16
                    .VerticalAlignment = XlVAlign.xlVAlignTop
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                End With
                '住所
                With .Range(.Cells(startrowposition + 4, 6), .Cells(startrowposition + 4, 8))
                    .Font.Size = 24
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                    .Orientation = XlOrientation.xlVertical
                End With
                .Cells(startrowposition + 4, 8).verticalalignment = XlVAlign.xlVAlignTop
                .Cells(startrowposition + 4, 6).verticalalignment = XlVAlign.xlVAlignCenter
                '宛名
                With .Cells(startrowposition + 4, 2)
                    .font.size = 36
                    .horizontalalignment = XlHAlign.xlHAlignCenter
                    .verticalalignment = XlVAlign.xlVAlignTop
                    .orientation = XlOrientation.xlVertical
                End With
            End With

        End Sub

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin

            With ExlWorkSheet
                .Range(.Cells(startrowposition + 4, 2), .Cells(startrowposition + 4, 4)).Merge()
                .Range(.Cells(startrowposition + 4, 6), .Cells(startrowposition + 4, 7)).Merge()
                .Range(.Cells(startrowposition + 4, 8), .Cells(startrowposition + 4, 9)).Merge()
            End With

        End Sub

        Public Sub SetData(startrowposition As Integer) Implements IVerticalOutputBehavior.SetData

            Dim addresstext1 As String = ""
            Dim addresstext2 As String = ""
            Dim addresseename As String

            With ExlWorkSheet
                .Cells.ClearContents()
                '郵便番号
                For I As Integer = 1 To 7
                    .Cells(startrowposition + 2, I + 2) = Replace(myAddressee.AddresseePostalCode, "-", "").Substring(I - 1, 1)
                Next

                Dim ac As New AddressConvert(myAddressee.AddresseeAddress1, myAddressee.AddresseeAddress2)
                '住所
                If ac.GetConvertAddress1.Length + ac.GetConvertAddress2.Length < 14 Then
                    addresstext1 = ac.GetConvertAddress1 & " " & ac.GetConvertAddress2
                Else
                    addresstext1 = ac.GetConvertAddress1
                    addresstext2 = ac.GetConvertAddress2
                End If

                If ac.GetConvertAddress2.Length > 16 Then .Cells(startrowposition + 4, 6).Interior.ColorIndex = 6

                .Cells(startrowposition + 4, 8) = ac.GetConvertAddress1
                .Cells(startrowposition + 4, 6) = ac.GetConvertAddress1

                '宛名
                If myAddressee.AddresseeName.Length > 5 Then
                    addresseename = myAddressee.AddresseeName & myAddressee.Title
                Else
                    addresseename = myAddressee.AddresseeName & " " & myAddressee.Title
                End If
                .Cells(startrowposition + 4, 2) = addresseename
            End With

        End Sub

        Private Sub SetCellFont() Implements IVerticalOutputBehavior.SetCellFont
            ExlWorkSheet.Cells.Font.Name = "HGP行書体"
        End Sub

        Public Function CriteriaCellRowIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellRowIndex
            Return 4
        End Function

        Public Function CriteriaCellColumnIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellColumnIndex
            Return 2
        End Function

        Public Function GetDataName() As String Implements IVerticalOutputBehavior.GetDataName
            Return ToString()
        End Function

        Private Function SetColumnSizes() As Double() Implements IVerticalOutputBehavior.SetColumnSizes
            Return {17.88, 6, 2.75, 2.75, 2.75, 2.38, 2.38, 2.38, 2.38, 0.85}
        End Function

        Private Function IExcelOutputBehavior_SetRowSizes() As Double() Implements IVerticalOutputBehavior.SetRowSizes
            Return {22.5, 18.75, 27.75, 372}
        End Function
    End Class

    ''' <summary>
    ''' 角2封筒クラス
    ''' </summary>
    Private Class Kaku2Envelope
        Implements IVerticalOutputBehavior

        Private ReadOnly myAddressee As AddresseeData

        Sub New(ByVal addressee As AddresseeData)
            myAddressee = addressee
        End Sub

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                '郵便番号
                With .Cells(startrowposition + 2, 3)
                    .font.size = 36
                    .horizontalalignment = XlHAlign.xlHAlignCenter
                    .verticalalignment = XlVAlign.xlVAlignBottom
                    .orientation = XlOrientation.xlHorizontal
                End With

                '住所
                With .Range(.Cells(startrowposition + 4, 5), .Cells(startrowposition + 4, 4))
                    .Font.Size = 43
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                    .VerticalAlignment = XlVAlign.xlVAlignTop
                    .Orientation = XlOrientation.xlVertical
                End With
                .Cells(startrowposition + 4, 4).verticalalignment = XlVAlign.xlVAlignCenter

                '宛名
                With .Cells(startrowposition + 4, 2)
                    .font.size = 85
                    .horizontalalignment = XlHAlign.xlHAlignCenter
                    .verticalalignment = XlVAlign.xlVAlignTop
                    .orientation = XlOrientation.xlVertical
                End With
            End With

        End Sub

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin

            With ExlWorkSheet
                .Range(.Cells(startrowposition + 2, 3), .Cells(startrowposition + 2, 5)).Merge()
                .Range(.Cells(startrowposition + 4, 2), .Cells(startrowposition + 5, 2)).Merge()
                .Range(.Cells(startrowposition + 4, 4), .Cells(startrowposition + 5, 4)).Merge()
                .Range(.Cells(startrowposition + 4, 5), .Cells(startrowposition + 5, 5)).Merge()
            End With

        End Sub

        Public Sub SetData(startrowposition As Integer) Implements IVerticalOutputBehavior.SetData

            With ExlWorkSheet
                .Cells.ClearContents()
                '郵便番号
                .Cells(startrowposition + 2, 3) = "〒 " & myAddressee.AddresseePostalCode
                '住所
                Dim ac As New AddressConvert(myAddressee.AddresseeAddress1, myAddressee.AddresseeAddress2)
                .Cells(startrowposition + 4, 5) = ac.GetConvertAddress1
                .Cells(startrowposition + 4, 4) = ac.GetConvertAddress2
                '宛名
                .Cells(startrowposition + 4, 2) = myAddressee.AddresseeName & " " & myAddressee.Title
            End With

        End Sub

        Private Sub SetCellFont() Implements IVerticalOutputBehavior.SetCellFont
            ExlWorkSheet.Cells.Font.Name = "HGP行書体"
        End Sub

        Public Function CriteriaCellRowIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellRowIndex
            Return 4
        End Function

        Public Function CriteriaCellColumnIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellColumnIndex
            Return 2
        End Function

        Private Function SetColumnSizes() As Double() Implements IVerticalOutputBehavior.SetColumnSizes
            Return {38.13, 23.5, 15.38, 9.63, 9.63, 23.38}
        End Function

        Private Function SetRowSizes() As Double() Implements IVerticalOutputBehavior.SetRowSizes
            Return {120, 50.25, 61.5, 409.5, 407.25}
        End Function

        Public Function GetDataName() As String Implements IVerticalOutputBehavior.GetDataName
            Return ToString()
        End Function
    End Class

    ''' <summary>
    ''' 墓地パンフクラス
    ''' </summary>
    Private Class GravePamphletEnvelope
        Implements IVerticalOutputBehavior

        Private ReadOnly myAddressee As AddresseeData

        Sub New(ByVal addressee As AddresseeData)
            myAddressee = addressee
        End Sub

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin

            With ExlWorkSheet
                .Range(.Cells(startrowposition + 2, 3), .Cells(startrowposition + 2, 5)).Merge()
                .Range(.Cells(startrowposition + 4, 2), .Cells(startrowposition + 5, 2)).Merge()
                .Range(.Cells(startrowposition + 4, 4), .Cells(startrowposition + 5, 4)).Merge()
                .Range(.Cells(startrowposition + 4, 5), .Cells(startrowposition + 5, 5)).Merge()
            End With

        End Sub

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                '郵便番号
                With .Cells(startrowposition + 2, 3)
                    .font.size = 36
                    .horizontalalignment = XlHAlign.xlHAlignCenter
                    .verticalalignment = XlVAlign.xlVAlignBottom
                    .orientation = XlOrientation.xlHorizontal
                End With

                '住所
                With .Range(.Cells(startrowposition + 4, 5), .Cells(startrowposition + 4, 4))
                    .Font.Size = 43
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                    .VerticalAlignment = XlVAlign.xlVAlignTop
                    .Orientation = XlOrientation.xlVertical
                End With
                .Cells(startrowposition + 4, 4).verticalalignment = XlVAlign.xlVAlignCenter

                '宛名
                With .Cells(startrowposition + 4, 2)
                    .font.size = 85
                    .horizontalalignment = XlHAlign.xlHAlignCenter
                    .verticalalignment = XlVAlign.xlVAlignTop
                    .orientation = XlOrientation.xlVertical
                End With
            End With

        End Sub

        Public Sub SetData(startrowposition As Integer) Implements IVerticalOutputBehavior.SetData

            With ExlWorkSheet
                .Cells.ClearContents()
                '郵便番号
                .Cells(startrowposition + 2, 3) = "〒 " & myAddressee.AddresseePostalCode
                '住所
                Dim ac As New AddressConvert(myAddressee.AddresseeAddress1, myAddressee.AddresseeAddress2)
                .Cells(startrowposition + 4, 5) = ac.GetConvertAddress1
                .Cells(startrowposition + 4, 4) = ac.GetConvertAddress2
                '宛名
                .Cells(startrowposition + 4, 2) = myAddressee.AddresseeName & " " & myAddressee.Title
            End With

        End Sub

        Private Sub SetCellFont() Implements IVerticalOutputBehavior.SetCellFont
            ExlWorkSheet.Cells.Font.Name = "HGP行書体"
        End Sub

        Public Function CriteriaCellRowIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellRowIndex
            Return 4
        End Function

        Public Function CriteriaCellColumnIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellColumnIndex
            Return 2
        End Function

        Private Function SetColumnSizes() As Double() Implements IVerticalOutputBehavior.SetColumnSizes
            Return {41.88, 23.5, 30.25, 9.63, 8.5}
        End Function

        Private Function SetRowSizes() As Double() Implements IVerticalOutputBehavior.SetRowSizes
            Return {71.25, 132.75, 51, 409.5, 409.5, 6.75}
        End Function

        Public Function GetDataName() As String Implements IVerticalOutputBehavior.GetDataName
            Return ToString()
        End Function
    End Class

    ''' <summary>
    ''' はがきクラス
    ''' </summary>
    Private Class Postcard
        Implements IVerticalOutputBehavior

        Private ReadOnly myAddressee As AddresseeData

        Sub New(ByVal addressee As AddresseeData)
            myAddressee = addressee
        End Sub

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                '郵便番号
                With .Range(.Cells(startrowposition + 2, 3), .Cells(startrowposition + 2, 10))
                    .Font.Size = 16
                    .VerticalAlignment = XlVAlign.xlVAlignTop
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                End With
                '住所
                With .Range(.Cells(startrowposition + 4, 7), .Cells(startrowposition + 4, 9))
                    .Font.Size = 18
                    .HorizontalAlignment = XlHAlign.xlHAlignCenter
                    .Orientation = XlOrientation.xlVertical
                End With
                .Cells(startrowposition + 4, 9).verticalalignment = XlVAlign.xlVAlignTop
                .Cells(startrowposition + 4, 7).verticalalignment = XlVAlign.xlVAlignCenter
                '宛名
                With .Cells(startrowposition + 4, 2)
                    .font.size = 36
                    .horizontalalignment = XlHAlign.xlHAlignCenter
                    .verticalalignment = XlVAlign.xlVAlignTop
                    .orientation = XlOrientation.xlVertical
                End With
            End With

        End Sub

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin

            With ExlWorkSheet
                .Range(.Cells(startrowposition + 4, 2), .Cells(startrowposition + 4, 5)).Merge()
                .Range(.Cells(startrowposition + 4, 9), .Cells(startrowposition + 4, 10)).Merge()
                .Range(.Cells(startrowposition + 4, 7), .Cells(startrowposition + 4, 8)).Merge()
            End With

        End Sub

        Public Sub SetData(startrowposition As Integer) Implements IVerticalOutputBehavior.SetData

            Dim addresstext1 As String = ""
            Dim addresstext2 As String = ""
            Dim addresseename As String

            With ExlWorkSheet
                .Cells.ClearContents()
                '郵便番号
                For I As Integer = 1 To 8
                    If I = 4 Then Continue For
                    .Cells(startrowposition + 2, I + 2) = myAddressee.AddresseePostalCode.Substring(I - 1, 1)
                Next

                '住所
                Dim ac As New AddressConvert(myAddressee.AddresseeAddress1, myAddressee.AddresseeAddress2)
                If ac.GetConvertAddress1.Length + ac.GetConvertAddress2.Length < 14 Then
                    addresstext1 = ac.GetConvertAddress1 & " " & ac.GetConvertAddress2
                Else
                    addresstext1 = ac.GetConvertAddress1
                    addresstext2 = ac.GetConvertAddress2
                End If
                If ac.GetConvertAddress2.Length > 14 Then .Cells(startrowposition + 4, 6).Interior.ColorIndex = 6
                .Cells(startrowposition + 4, 9) = addresstext1
                .Cells(startrowposition + 4, 7) = addresstext2

                '宛名
                If myAddressee.AddresseeName.Length > 5 Then
                    addresseename = myAddressee.AddresseeName & myAddressee.Title
                Else
                    addresseename = myAddressee.AddresseeName & " " & myAddressee.Title
                End If
                .Cells(startrowposition + 4, 2) = addresseename
            End With

        End Sub

        Private Sub SetCellFont() Implements IVerticalOutputBehavior.SetCellFont
            ExlWorkSheet.Cells.Font.Name = "HGP行書体"
        End Sub

        Public Function CriteriaCellRowIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellRowIndex
            Return 4
        End Function

        Public Function CriteriaCellColumnIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellColumnIndex
            Return 2
        End Function

        Public Function GetDataName() As String Implements IVerticalOutputBehavior.GetDataName
            Return ToString()
        End Function

        Private Function SetColumnSizes() As Double() Implements IVerticalOutputBehavior.SetColumnSizes
            Return {16, 3.63, 2.75, 2.75, 2.75, 0.62, 2.75, 2.75, 2.75, 2.75, 0.77}
        End Function

        Private Function SetRowSizes() As Double() Implements IVerticalOutputBehavior.SetRowSizes
            Return {30, 22.5, 22.5, 360.75}
        End Function

    End Class

    ''' <summary>
    ''' ラベルシートクラス
    ''' </summary>
    Private Class LabelSheet
        Implements IHorizontalOutputBehavior

        Private ReadOnly myAddressee As AddresseeData

        Sub New(ByVal addressee As AddresseeData)
            myAddressee = addressee
        End Sub

        ''' <summary>
        ''' ラベルに入力する文字列を返します
        ''' </summary>
        ''' <param name="lineindex">行番号</param>
        ''' <param name="addressee">ラベル化する宛先</param>
        ''' <returns></returns>
        Private Function ReturnLabelString(ByVal lineindex As Integer, ByVal addressee As AddresseeData) As String

            'セルに入力する宛先を格納する文字列　初期値に郵便番号
            Dim ReturnString As String = Space(10) & "〒 " & addressee.AddresseePostalCode & vbNewLine & vbNewLine
            Dim ac As New AddressConvert(addressee.AddresseeAddress1, addressee.AddresseeAddress2)
            ReturnString &= Space(10) & ac.GetConvertAddress1 & vbCrLf  '住所1

            Try
                ReturnString &= Space(10) & addressee.AddresseeAddress2.Substring(0, 16) & vbNewLine   '住所2
                ReturnString &= Space(10) & addressee.AddresseeAddress2.Substring(16) & vbNewLine & vbNewLine '住所2（2行目）
            Catch ex As ArgumentOutOfRangeException
                '住所2の文字列が短い場合のエラー対応（16文字以下ならエラー）
                ReturnString &= Space(10) & addressee.AddresseeAddress2 & vbNewLine & vbNewLine & vbNewLine
            End Try

            '宛名
            ReturnString &= Space(10) & addressee.AddresseeName & " " & addressee.Title & vbNewLine

            'ラベルの行数によって、行を挿入する
            If lineindex Mod 6 = 0 Then
                ReturnString = vbNewLine & vbNewLine & ReturnString
                Return ReturnString
            End If

            If lineindex Mod 7 = 0 Then ReturnString = vbNewLine & vbNewLine & vbNewLine & ReturnString

            Return ReturnString

        End Function

        Private Sub SetCellFont() Implements IHorizontalOutputBehavior.SetCellFont
            ExlWorkSheet.Cells.Font.Name = "ＭＳ Ｐゴシック"
        End Sub

        Public Sub CellProperty(startrowposition As Integer) Implements IHorizontalOutputBehavior.CellProperty

            With ExlWorkSheet
                .PageSetup.PaperSize = XlPaperSize.xlPaperA4
                .Cells.Font.Size = 10
                .Cells.VerticalAlignment = XlVAlign.xlVAlignCenter
                .Cells.Orientation = XlOrientation.xlHorizontal
            End With

        End Sub

        Private Function SetColumnSizes() As Double() Implements IHorizontalOutputBehavior.SetColumnSizes
            Return {30.5, 30.5, 30.25}
        End Function

        Private Function SetRowSizes() As Double() Implements IHorizontalOutputBehavior.SetRowSizes
            Return {118.5, 118.5, 118.5, 118.5, 118.5, 118.5, 118.5}
        End Function

        Public Function GetDataName() As String Implements IHorizontalOutputBehavior.GetDataName
            Return ToString()
        End Function

        Private Sub SetData() Implements IHorizontalOutputBehavior.SetData

            Dim column As Integer = 1
            Dim row As Integer = 1

            With ExlWorkSheet
                Do Until .Cells(row, column).Text.Trim.Length = 0
                    column += 1
                    If column > 3 Then
                        column = 1
                        row += 1
                    End If
                Loop

                .Cells(row, column) = ReturnLabelString(row, myAddressee)
            End With

        End Sub

    End Class

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
End Class
