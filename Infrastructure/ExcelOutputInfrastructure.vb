Imports ClosedXML.Excel
Imports Microsoft.Office.Interop
Imports Domain
Imports System.Text.RegularExpressions
Imports System.Collections.ObjectModel

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
    Function SetCellFont() As String

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

    ''' <summary>
    ''' 印刷範囲の文字列を返します
    ''' </summary>
    ''' <returns></returns>
    Function SetPrintAreaString() As String

End Interface

''' <summary>
''' データを横向けに出力
''' </summary>
Interface IHorizontalOutputBehavior
    Inherits IExcelOutputBehavior

    ''' <summary>
    ''' 出力するデータをセットします
    ''' </summary>
    Sub SetData(ByVal destinationdata As DestinationDataEntity)

End Interface

''' <summary>
''' データのリストを縦向けに出力
''' </summary>
Interface IVerticalOutputListBehavior
    Inherits IVerticalOutputBehavior

    ''' <summary>
    ''' 出力するデータをセットします
    ''' </summary>
    ''' <param name="startrowposition"></param>
    Sub SetData(ByVal startrowposition As Integer, ByVal destinationdata As DestinationDataEntity)

    ''' <summary>
    ''' 宛名クラスを保持するリスト
    ''' </summary>
    ''' <returns></returns>
    Function GetDestinationDataList() As ObservableCollection(Of DestinationDataEntity)

    ''' <summary>
    ''' 宛名印刷の住所の長さの限界値を返します。0は検証しません
    ''' </summary>
    ''' <returns></returns>
    Function GetAddressMaxLength() As Integer

    ''' <summary>
    ''' 長さを検証する文字列
    ''' </summary>
    ''' <returns></returns>
    Function GetLengthVerificationString(ByVal destinationData As DestinationDataEntity) As String

End Interface

''' <summary>
''' 墓地札データを出力
''' </summary>
Interface IGravePanelListBehavior
    Inherits IVerticalOutputBehavior

    ''' <summary>
    ''' 出力するデータをセットします
    ''' </summary>
    ''' <param name="startrowposition"></param>
    Sub SetData(ByVal startrowposition As Integer, ByVal gravepanel As GravePanelDataEntity)

End Interface

''' <summary>
''' エクセルデータを縦向けに出力
''' </summary>
Interface IVerticalOutputBehavior
    Inherits IExcelOutputBehavior

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
        AddressText = Replace(AddressText, My.Resources.ToukyouString, String.Empty)
        AddressText = Replace(AddressText, My.Resources.KanagawaString, String.Empty)
        AddressText = Replace(AddressText, My.Resources.TokushimaString, String.Empty)
        If AddressText Is Nothing Then Return String.Empty

        '郡が入っている住所はそのまま返す
        If InStr(AddressText, My.Resources.GunString) <> 0 Then Return AddressText

        '県と市を比べる
        AddressText = VerifyAddressString(AddressText, My.Resources.KenString)

        '府と市を比べる
        AddressText = VerifyAddressString(AddressText, My.Resources.FuString)

        '”("から先を削除する
        Dim addressarray() As String = Split(AddressText, My.Resources.FullwidthClosingParenthesis)
        AddressText = addressarray(0)

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
        If address.Substring(0, InStr(1, address, verifystring) - 1) = address.Substring(InStr(1, address, verifystring), InStr(1, address, My.Resources.ShiString) - InStr(1, address, verifystring) - 1) Then
            Return address.Substring(InStr(1, address, verifystring))
        End If

        Return address

    End Function

    ''' <summary>
    ''' 住所2の番地を漢字に変換して返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetConvertAddress2() As String Implements IAddressConvert.GetConvertAddress2

        Dim basestring As String

        '7-1-7東栗谷マンション202の場合

        'Address2の数字以外の文字列を*に変換する ⇒  ７*１*７********２０２
        basestring = Regex.Replace(StrConv(Address2, vbWide), "[^０-９]", "*")
        '*を基準に配列を生成する⇒７,１,７,,,,,,,,２０２
        Dim numberarray() As String = Split(basestring, "*")
        '数字を漢字に変換する。⇒七,一,七,,,,,,,,二〇二
        For i As Integer = 0 To UBound(numberarray)
            numberarray(i) = BranchConvertNumber(Trim(numberarray(i)))
        Next

        'Address2の数字を*に置換⇒*－*－*東栗谷マンション***
        basestring = Regex.Replace(StrConv(Address2, vbWide), "[０-９]", "*")

        '**と二つ以上連続することのないように置換する⇒*－*－*東栗谷マンション*
        Do Until InStr(basestring, "**") = 0
            basestring = Replace(basestring, "**", "*")
        Loop

        'numberarrayの空文字以外の要素を最初から順に取り出し、basestringの最初の*に要素を置換する⇒七－一－七東栗谷マンション二〇二
        For j As Integer = 0 To UBound(numberarray)
            If Not String.IsNullOrEmpty(numberarray(j)) Then basestring = Replace(basestring, "*", numberarray(j), 1, 1)
        Next
        'ハイフンを置換する
        basestring = Replace(basestring, "－", "ー")
        '*を空欄に置換して値を返す
        Return Replace(basestring, "*", String.Empty)

    End Function

    ''' <summary>
    ''' 文字列の数字を漢字に変換します。数字出ない場合は引数をそのまま返します
    ''' </summary>
    ''' <param name="addressString">変換文字列</param>
    ''' <returns></returns>
    Private Function BranchConvertNumber(ByVal addressString As String) As String

        Dim rx As New Regex("^[\d]+$")

        If rx.IsMatch(addressString) Then
            Return ConvertNumber(addressString)
        Else
            Return addressString
        End If

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
                Return My.Resources.ZeroString
            Case 1
                Return My.Resources.OneString
            Case 2
                Return My.Resources.TowString
            Case 3
                Return My.Resources.ThreeString
            Case 4
                Return My.Resources.FourString
            Case 5
                Return My.Resources.FiveString
            Case 6
                Return My.Resources.SixString
            Case 7
                Return My.Resources.SevenString
            Case 8
                Return My.Resources.EightString
            Case 9
                Return My.Resources.NineString
            Case 10
                Return My.Resources.TenString
            Case Else
                Return String.Empty
        End Select

    End Function

    ''' <summary>
    ''' 11から19までの数字を変換します
    ''' </summary>
    ''' <param name="myNumber">変換する数字</param>
    ''' <returns></returns>
    Private Function ConvertNumber_Over11Under19(ByVal myNumber As Integer) As String

        Select Case myNumber
            Case 11
                Return $"{My.Resources.TenString}{My.Resources.OneString}"
            Case 12
                Return $"{My.Resources.TenString}{My.Resources.TowString}"
            Case 13
                Return $"{My.Resources.TenString}{My.Resources.ThreeString}"
            Case 14
                Return $"{My.Resources.TenString}{My.Resources.FourString}"
            Case 15
                Return $"{My.Resources.TenString}{My.Resources.FiveString}"
            Case 16
                Return $"{My.Resources.TenString}{My.Resources.SixString}"
            Case 17
                Return $"{My.Resources.TenString}{My.Resources.SevenString}"
            Case 18
                Return $"{My.Resources.TenString}{My.Resources.EightString}"
            Case 19
                Return $"{My.Resources.TenString}{My.Resources.NineString}"
            Case Else
                Return String.Empty
        End Select

    End Function

    ''' <summary>
    ''' 20以上の数字を変換します
    ''' </summary>
    ''' <param name="myNumber">変換する数字</param>
    ''' <returns></returns>
    Private Function ConvertNumber_Orver20(ByVal myNumber As Integer) As String

        Dim myValue As String = String.Empty

        '一桁ごとに漢字変換する
        For I As Integer = 1 To myNumber.ToString.Length
            myValue &= ConvertNumber_Under10(myNumber.ToString.Substring(I - 1, 1))
        Next

        '漢字2文字でなければキリ番ではないので、そのまま返す
        If myValue.ToString.Length <> 2 Then Return myValue

        '20、30などの数字を〇から十に変える
        If myValue.Substring(1, 1) = My.Resources.ZeroString Then myValue = $"{myValue.Substring(0, 1)}{My.Resources.TenString}"

        Return myValue

    End Function

End Class

''' <summary>
''' エクセルへの処理を行います
''' </summary>
Public Class ExcelOutputInfrastructure
    Implements IOutputDataRepogitory

    ''' <summary>
    ''' 進捗を受け取るリスナー
    ''' </summary>
    Private Shared ProcessedCountListener As IProcessedCountObserver

    ''' <summary>
    ''' 住所の長いデータの数を受け取るリスナー
    ''' </summary>
    Private Shared OverLengthAddressCountListener As IOverLengthAddress2Count

    ''' <summary> 
    ''' 出力するデータの種類を保持する
    ''' </summary>
    ''' <returns></returns>
    Private Shared Property OutputDataGanre As String

    ''' <summary>
    ''' 宛先データ
    ''' </summary>
    Private Property MyAddressee As DestinationDataEntity

    ''' <summary>
    ''' ワークブック
    ''' </summary>
    Private Shared ExlWorkbook As XLWorkbook

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
    Private Shared ExlWorkSheet As IXLWorksheet

    Private Volb As IVerticalOutputListBehavior

    Private gpb As IGravePanelListBehavior

    Private Hob As IHorizontalOutputBehavior

    ''' <summary>
    ''' 開始位置を決める為の基準のIndex
    ''' </summary>
    Private Shared StartIndex As Integer

    ''' <summary>
    ''' ファイルの位置を指定、選択します
    ''' </summary>
    Private ReadOnly buf As String = Dir(My.Resources.SAVEPATH)

    Private exlworkbooks As Excel.Workbooks

    ''' <summary>
    ''' 宛先クラスを格納したリスト
    ''' </summary>
    Private ReadOnly AddresseeList As List(Of DestinationDataEntity)

    Private exlapp As Excel.Application

    ''' <summary>
    ''' 進捗のカウント
    ''' </summary>
    ''' <returns></returns>
    Private Property ProcessedCount As Integer

    ''' <summary>
    ''' いろいろ発行エクセルファイルを閉じて、メモリ上にClosedXMLのSheetを生成します。
    ''' </summary>
    Private Sub SheetSetting()

        ExcelClose()

        ExlWorkbook = New XLWorkbook
        If ExlWorkSheet Is Nothing Then ExlWorkSheet = ExlWorkbook.AddWorksheet(My.Resources.FILENAME)
        ExlWorkSheet.Cells.Style.NumberFormat.SetNumberFormatId(49)

    End Sub

    ''' <summary>
    ''' いろいろ発行エクセルファイルを閉じます
    ''' </summary>
    Private Sub ExcelClose()
        Try
            exlapp = GetObject(, My.Resources.ExcelApp)
        Catch ex As Exception
            exlapp = CreateObject(My.Resources.ExcelApp)
        End Try

        exlworkbooks = exlapp.Workbooks

        Dim bolSheetCheck As Boolean = False
        Dim myWorkbook As Excel.Workbook = Nothing
        For Each myWorkbook In exlworkbooks
            If myWorkbook.Name <> buf Then Continue For
            bolSheetCheck = True
            Exit For
        Next

        If bolSheetCheck Then myWorkbook.Close(False)
        If exlworkbooks.Count = 0 Then exlapp.Quit()
    End Sub

    ''' <summary>
    ''' いろいろ発行エクセルファイルを開きます
    ''' </summary>
    Private Sub ExcelOpen()

        Dim bolSheetCheck As Boolean = False
        Dim myWorkbook As Excel.Workbook

        For Each myWorkbook In exlworkbooks
            If myWorkbook.Name <> buf Then Continue For
            bolSheetCheck = True
            Exit For
        Next

        If bolSheetCheck = False Then
            exlapp.Visible = True
            Dim openpath As String = System.IO.Path.GetFullPath(My.Resources.SAVEPATH)
            Dim executebook As Excel.Workbook = exlworkbooks.Open(openpath, , True)
            executebook.Activate()
        End If

    End Sub

    ''' <summary>
    ''' 入力するでーたの印刷範囲の一番上のRowを返します
    ''' </summary>
    ''' <returns></returns>
    Private Function SetStartRowPosition(ByVal vob As IVerticalOutputBehavior) As Integer

        Dim addint As Integer = UBound(RowSizes) + 1    '一回に移動する数字。印刷データの１ページ分移動します
        Dim column As Integer = vob.CriteriaCellColumnIndex '入力時に必ず値が入っているセルのColumn
        Dim row As Integer = vob.CriteriaCellRowIndex   '入力時に必ず値が入っているセルのRow

        '入力時に必ず値が入っているセルに文字列があればインデックスをプラスする
        With ExlWorkSheet
            StartIndex += 1
            Return (StartIndex - 1) * addint
        End With

    End Function

    ''' <summary>
    ''' 横向けOutput用のシートをセッティングします
    ''' </summary>
    ''' <param name="eob"></param>
    Private Sub SettingNewSheet_Horizontal(ByVal eob As IExcelOutputBehavior)

        If OutputDataGanre = eob.GetDataName Then Exit Sub

        OutputDataGanre = eob.GetDataName
        ColumnSizes = eob.SetColumnSizes
        RowSizes = eob.SetRowSizes

        DataClear()
        eob.SetCellFont()

        With ExlWorkSheet
            .PageSetup.PrintAreas.Clear()
            .PageSetup.PrintAreas.Add(eob.SetPrintAreaString)
        End With

    End Sub

    ''' <summary>
    ''' 縦向けOutput用のシートをセッティングします
    ''' </summary>
    ''' <param name="eob"></param>
    Private Sub SettingNewSheet_Vertical(ByVal eob As IExcelOutputBehavior)

        ColumnSizes = eob.SetColumnSizes
        RowSizes = eob.SetRowSizes
        If OutputDataGanre = eob.GetDataName Then Exit Sub
        OutputDataGanre = eob.GetDataName
        SetMargin()

        With ExlWorkSheet
            .Cells.Clear()
            'ColumnSizesの配列の中の数字をシートのカラムの幅に設定する
            For i As Integer = 0 To UBound(ColumnSizes)
                .Column(i + 1).Width = ColumnSizes(i)
            Next
            .PageSetup.PrintAreas.Clear()
            .PageSetup.PrintAreas.Add(eob.SetPrintAreaString)
        End With

    End Sub

    ''' <summary>
    ''' 横向けにデータを入力する処理。ラベル用紙用
    ''' </summary>
    ''' <param name="_hob"></param>
    Private Sub OutputHorizontalProcessing(ByVal _hob As IHorizontalOutputBehavior)

        Hob = _hob
        SheetSetting()

        Dim column As Integer = 1
        Dim row As Integer = 1
        Dim sheetindex As Integer = 0

        With ExlWorkSheet
            '出力するデータの種類が違えばセルをクリアする
            SettingNewSheet_Horizontal(Hob)
            ProcessedCount = 0
            For Each dde As DestinationDataEntity In AddresseeList
                'ラベルのマスに値がない初めの位置と、ラベル件数からページ数を割り出し設定する
                Dim linecount As Integer = 1
                Do Until .Cell(row, column).Value = String.Empty
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
                    .Column(i + 1).Width = ColumnSizes(i)
                Next
                'ロウの高さを設定する
                For j As Integer = 0 To UBound(RowSizes)
                    .Row((j + 1) + sheetindex * UBound(RowSizes)).Height = RowSizes(j)
                Next

                Hob.CellProperty(sheetindex)
                Hob.SetData(dde)
                ProcessedCount += 1
                If ProcessedCountListener IsNot Nothing Then ProcessedCountListener.ProcessedCountNotify(ProcessedCount)
            Next
        End With

        ExlWorkbook.SaveAs(My.Resources.SAVEPATH)

        ExcelOpen()

    End Sub

    ''' <summary>
    ''' 縦向けにリストのデータを入力する処理
    ''' </summary>
    ''' <param name="_vob"></param>
    ''' <param name="ismulti">複数印刷Behaviorをするかを設定します</param>
    Private Sub ListOutputVerticalProcessing(ByVal _vob As IVerticalOutputListBehavior, ByVal ismulti As Boolean)

        Dim overLengthCount As Integer = 0

        Volb = _vob
        SheetSetting()

        With ExlWorkSheet
            '出力するデータの種類が違えばセルをクリアする
            SettingNewSheet_Vertical(Volb)
            ProcessedCount = 0
            For Each dde As DestinationDataEntity In Volb.GetDestinationDataList

                '複数印刷するならポジションを設定
                If ismulti Then
                    StartRowPosition = SetStartRowPosition(Volb)
                Else
                    .Unmerge()
                    StartRowPosition = 0
                End If
                Volb.CellProperty(StartRowPosition)

                'RowSizesの配列の中の数字をシートのローの幅に設定する
                For I = 0 To UBound(RowSizes)
                    .Rows(StartRowPosition + (I + 1)).Height = RowSizes(I)
                Next

                Volb.CellsJoin(StartRowPosition)
                Volb.SetData(StartRowPosition, dde)

                ProcessedCount += 1
                If Volb.GetLengthVerificationString(dde).Length > Volb.GetAddressMaxLength Then overLengthCount += 1
                If ProcessedCountListener IsNot Nothing Then ProcessedCountListener.ProcessedCountNotify(ProcessedCount)
            Next
            .Style.Font.FontName = Volb.SetCellFont
        End With

        If overLengthCount > 0 Then NotificationOverLengthCount(overLengthCount)
        If ExlWorkbook.Worksheets.Count = 0 Then ExlWorkbook.AddWorksheet(ExlWorkSheet)
        ExlWorkbook.SaveAs(My.Resources.SAVEPATH)
        ExcelOpen()

    End Sub

    ''' <summary>
    ''' 住所の文字列が長いデータの件数を知らせます
    ''' </summary>
    ''' <param name="count">件数</param>
    Private Sub NotificationOverLengthCount(ByVal count As Integer)
        OverLengthAddressCountListener.OverLengthCountNotify(count)
    End Sub

    ''' <summary>
    ''' 墓地札データリスト出力
    ''' </summary>
    ''' <param name="_vob"></param>
    ''' <param name="outputPositon"></param>
    Private Sub GravePanelListOutputProcessing(ByVal _vob As IGravePanelListBehavior, ByVal outputPositon As Integer)

        Dim gpl As GravePanelDataListEntity = GravePanelDataListEntity.GetInstance
        gpb = _vob

        SheetSetting()

        With ExlWorkSheet
            SettingNewSheet_Vertical(gpb)
            .PageSetup.Margins.Bottom = 2
            StartIndex = 0
            .Cells.Clear()

            StartRowPosition = 0
            Do Until StartIndex = outputPositon - 1
                For i = 0 To UBound(RowSizes)
                    .Rows(StartRowPosition + (i + 1)).Height = RowSizes(i)
                Next
                StartRowPosition += UBound(RowSizes) + 1
                StartIndex += 1
            Loop

            For Each gp As GravePanelDataEntity In gpl.List
                If gp.MyIsPrintout.Value = False Then Continue For

                StartRowPosition = SetStartRowPosition(gpb)
                gpb.CellProperty(StartRowPosition)

                'RowSizesの配列の中の数字をシートのローの幅に設定する
                For I = 0 To UBound(RowSizes)
                    .Rows(StartRowPosition + (I + 1)).Height = RowSizes(I)
                Next

                gpb.CellsJoin(StartRowPosition)
                gpb.SetData(StartRowPosition, gp)
            Next

            .Style.Font.FontName = gpb.SetCellFont
        End With

        If ExlWorkbook.Worksheets.Count = 0 Then ExlWorkbook.AddWorksheet(ExlWorkSheet)
        ExlWorkbook.SaveAs(My.Resources.SAVEPATH)
        ExcelOpen()

    End Sub

    ''' <summary>
    ''' エクセルシートの余白を0に設定する
    ''' </summary>
    Private Sub SetMargin()

        With ExlWorkSheet.PageSetup.Margins
            .Top = 0
            .Bottom = 0
            .Right = 0
            .Left = 0
        End With

    End Sub

    Public Sub TransferPaperPrintOutput(customerid As String, addressee As String, title As String, postalcode As String, address1 As String, address2 As String,
                                        money As String, note1 As String, note2 As String, note3 As String, note4 As String,
                                        note5 As String, multioutput As Boolean) Implements IOutputDataRepogitory.TransferPaperPrintOutput

        MyAddressee = New DestinationDataEntity(customerid, addressee, title, postalcode, address1, address2, money, note1, note2, note3, note4, note5)
        Dim tp As IVerticalOutputListBehavior = New TransferPaper(MyAddressee)
        ListOutputVerticalProcessing(tp, multioutput)

    End Sub

    Public Sub LabelOutput(customerid As String, addressee As String, title As String, postalcode As String, address1 As String, address2 As String) Implements IOutputDataRepogitory.LabelOutput

        MyAddressee = New DestinationDataEntity(customerid, addressee, title, postalcode, address1, address2)
        Dim ls As IHorizontalOutputBehavior = New LabelSheet(MyAddressee)
        OutputHorizontalProcessing(ls)

    End Sub

    Public Sub Cho3EnvelopeOutput(customerid As String, addressee As String, title As String, postalcode As String, address1 As String, address2 As String,
                                  multioutput As Boolean) Implements IOutputDataRepogitory.Cho3EnvelopeOutput

        MyAddressee = New DestinationDataEntity(customerid, addressee, title, postalcode, address1, address2)
        Dim ce As IVerticalOutputListBehavior = New Cho3Envelope(MyAddressee)
        ListOutputVerticalProcessing(ce, multioutput)

    End Sub

    Public Sub WesternEnvelopeOutput(customerid As String, addressee As String, title As String, postalcode As String, address1 As String, address2 As String, multioutput As Boolean) Implements IOutputDataRepogitory.WesternEnvelopeOutput

        MyAddressee = New DestinationDataEntity(customerid, addressee, title, postalcode, address1, address2)
        Dim we As IVerticalOutputListBehavior = New WesternEnvelope(MyAddressee)
        ListOutputVerticalProcessing(we, multioutput)

    End Sub

    Public Sub Kaku2EnvelopeOutput(customerid As String, addressee As String, title As String, postalcode As String, address1 As String, address2 As String, multioutput As Boolean) Implements IOutputDataRepogitory.Kaku2EnvelopeOutput

        MyAddressee = New DestinationDataEntity(customerid, addressee, title, postalcode, address1, address2)
        Dim ke As IVerticalOutputListBehavior = New Kaku2Envelope(MyAddressee)
        ListOutputVerticalProcessing(ke, multioutput)

    End Sub

    Public Sub GravePamphletOutput(customerid As String, addressee As String, title As String, postalcode As String, address1 As String, address2 As String, multioutput As Boolean) Implements IOutputDataRepogitory.GravePamphletOutput

        MyAddressee = New DestinationDataEntity(customerid, addressee, title, postalcode, address1, address2)
        Dim gp As IVerticalOutputListBehavior = New GravePamphletEnvelope(MyAddressee)
        ListOutputVerticalProcessing(gp, multioutput)

    End Sub

    Public Sub PostcardOutput(customerid As String, addressee As String, title As String, postalcode As String, address1 As String, address2 As String, multioutput As Boolean) Implements IOutputDataRepogitory.PostcardOutput

        MyAddressee = New DestinationDataEntity(customerid, addressee, title, postalcode, address1, address2)
        Dim pc As IVerticalOutputListBehavior = New Postcard(MyAddressee)
        ListOutputVerticalProcessing(pc, multioutput)

    End Sub

    Public Sub GravePanelOutput(outputPosition As Integer) Implements IOutputDataRepogitory.GravePanelOutput

        Dim gp As IGravePanelListBehavior = New GravePanel()
        GravePanelListOutputProcessing(gp, outputPosition)

    End Sub

    Public Sub Cho3EnvelopeOutput(list As ObservableCollection(Of DestinationDataEntity)) Implements IOutputDataRepogitory.Cho3EnvelopeOutput
        Dim ce As IVerticalOutputListBehavior = New Cho3Envelope(list)
        ListOutputVerticalProcessing(ce, True)
    End Sub

    Public Sub WesternEnvelopeOutput(list As ObservableCollection(Of DestinationDataEntity)) Implements IOutputDataRepogitory.WesternEnvelopeOutput
        Dim we As IVerticalOutputListBehavior = New WesternEnvelope(list)
        ListOutputVerticalProcessing(we, True)
    End Sub

    Public Sub Kaku2EnvelopeOutput(list As ObservableCollection(Of DestinationDataEntity)) Implements IOutputDataRepogitory.Kaku2EnvelopeOutput
        Dim ke As IVerticalOutputListBehavior = New Kaku2Envelope(list)
        ListOutputVerticalProcessing(ke, True)
    End Sub

    Public Sub GravePamphletOutput(list As ObservableCollection(Of DestinationDataEntity)) Implements IOutputDataRepogitory.GravePamphletOutput
        Dim gp As IVerticalOutputListBehavior = New GravePamphletEnvelope(list)
        ListOutputVerticalProcessing(gp, True)
    End Sub

    Public Sub PostcardOutput(list As ObservableCollection(Of DestinationDataEntity)) Implements IOutputDataRepogitory.PostcardOutput
        Dim pc As IVerticalOutputListBehavior = New Postcard(list)
        ListOutputVerticalProcessing(pc, True)
    End Sub

    Private Sub AddProcessedCountListener(_listener As IProcessedCountObserver) Implements IOutputDataRepogitory.AddProcessedCountListener
        ProcessedCountListener = _listener
    End Sub

    Public Sub DataClear() Implements IOutputDataRepogitory.DataClear
        If ExlWorkSheet Is Nothing Then Exit Sub
        ExlWorkSheet.Cells.Clear()
        StartIndex = 0
    End Sub

    Public Sub AddOverLengthAddressListener(_listener As IOverLengthAddress2Count) Implements IOutputDataRepogitory.AddOverLengthAddressListener
        OverLengthAddressCountListener = _listener
    End Sub

    ''' <summary>
    ''' 墓地札クラス
    ''' </summary>
    Private Class GravePanel
        Implements IGravePanelListBehavior

        Public Sub SetData(startrowposition As Integer, gravepanel As GravePanelDataEntity) Implements IGravePanelListBehavior.SetData
            With ExlWorkSheet
                If Not String.IsNullOrEmpty(gravepanel.GetFamilyName.Name) Then .Cell(startrowposition + 1, 1).Value = $"{NameConvert(gravepanel.GetFamilyName.Name)}家"
                .Cell(startrowposition + 2, 1).Value = $"{gravepanel.GetGraveNumber.Number}{Space(1)}{gravepanel.GetArea.AreaValue}{My.Resources.SquareFootageText}"
                .Cell(startrowposition + 3, 1).Value = My.Resources.CleaningContract
                .Cell(startrowposition + 3, 2).Value = gravepanel.GetContractContent.Content
            End With
        End Sub

        ''' <summary>
        ''' 名前の文字列の必要箇所にスペースを挿入します
        ''' </summary>
        ''' <param name="strName"></param>
        ''' <returns></returns>
        Private Function NameConvert(ByVal strName As String) As String

            Dim I As Integer = 0 'ループで使う添え字
            Dim nameArray() As String
            Dim nameValue As String = String.Empty

            ReDim nameArray(strName.Trim.Length - 1)

            Do Until I = strName.Trim.Length
                nameArray(I) = strName.Substring(I, 1)
                I += 1
            Loop

            Select Case strName.Trim.Length
                Case 1, 2
                    For I = 0 To UBound(nameArray) Step 1
                        If nameArray(I).Trim.Length <> 0 Then nameValue &= $"{nameArray(I)}{StrConv(Space(1), VbStrConv.Wide)}"
                    Next
                Case > 2
                    For I = 0 To UBound(nameArray) Step 1
                        If nameArray(I).Trim.Length <> 0 Then nameValue &= $"{nameArray(I)}{Space(1)}"
                    Next

            End Select

            Return nameValue

        End Function

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin
            With ExlWorkSheet
                .Range(.Cell(startrowposition + 1, 1), .Cell(startrowposition + 1, 2)).Merge()
                .Range(.Cell(startrowposition + 2, 1), .Cell(startrowposition + 2, 2)).Merge()
            End With
        End Sub

        Public Function SetCellFont() As String Implements IExcelOutputBehavior.SetCellFont
            Return My.Resources.HGRegularRegularScriptPRO
        End Function

        Public Sub CellProperty(startrowposition As Integer) Implements IExcelOutputBehavior.CellProperty

            With ExlWorkSheet
                .PageSetup.PaperSize = XLPaperSize.A4Paper
                With .Cell(startrowposition + 1, 1).Style
                    .Font.FontSize = 65
                    .Font.Bold = True
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                End With
                With .Range(.Cell(startrowposition + 2, 1), .Cell(startrowposition + 3, 2)).Style
                    .Font.FontSize = 48
                    .Font.Bold = True
                    .Alignment.ShrinkToFit=True
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                End With
                With .Range(.Cell(startrowposition + 1, 1), .Cell(startrowposition + 3, 2)).Style.Border
                    .SetTopBorder(XLBorderStyleValues.Thick)
                    .SetBottomBorder(XLBorderStyleValues.Thick)
                    .SetLeftBorder(XLBorderStyleValues.Thick)
                    .SetRightBorder(XLBorderStyleValues.Thick)
                End With
            End With
        End Sub

        Public Function CriteriaCellRowIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellRowIndex
            Return 1
        End Function

        Public Function CriteriaCellColumnIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellColumnIndex
            Return 1
        End Function

        Public Function SetColumnSizes() As Double() Implements IExcelOutputBehavior.SetColumnSizes
            Return {42.86, 51.71}
        End Function

        Public Function SetRowSizes() As Double() Implements IExcelOutputBehavior.SetRowSizes
            Return {82.5, 67.5, 73.5, 10.5}
        End Function

        Public Function GetDataName() As String Implements IExcelOutputBehavior.GetDataName
            Return ToString()
        End Function

        Public Function SetPrintAreaString() As String Implements IExcelOutputBehavior.SetPrintAreaString
            Return "a:b"
        End Function

    End Class

    ''' <summary>
    ''' 長3封筒クラス
    ''' </summary>
    Private Class Cho3Envelope
        Implements IVerticalOutputListBehavior

        Private ReadOnly AddresseeList As ObservableCollection(Of DestinationDataEntity)

        Sub New(ByVal _addressee As DestinationDataEntity)
            AddresseeList = New ObservableCollection(Of DestinationDataEntity) From {_addressee}
        End Sub

        Sub New(ByVal _addresseelist As ObservableCollection(Of DestinationDataEntity))
            AddresseeList = _addresseelist
        End Sub

        Public Sub SetData(startrowposition As Integer, destinationdata As DestinationDataEntity) Implements IVerticalOutputListBehavior.SetData

            Dim addresseename As String

            With ExlWorkSheet
                '郵便番号
                For I As Integer = 1 To 7
                    .Cell(startrowposition + 2, I + 2).Value = Replace(destinationdata.MyPostalCode.GetCode, "-", String.Empty).Substring(I - 1, 1)
                Next

                '住所
                Dim addresstext1 As String = String.Empty
                Dim addresstext2 As String = String.Empty
                Dim ac As New AddressConvert(destinationdata.MyAddress1.Address, destinationdata.MyAddress2.Address)
                addresstext1 = ac.GetConvertAddress1
                addresstext2 = ac.GetConvertAddress2
                If addresstext1.Length + addresstext2.Length < 15 Then
                    .Cell(startrowposition + 4, 8).Value = $"{ac.GetConvertAddress1}{Space(1)}{ac.GetConvertAddress2}"
                    .Cell(startrowposition + 4, 6).Value = String.Empty
                Else
                    .Cell(startrowposition + 4, 8).Value = ac.GetConvertAddress1
                    .Cell(startrowposition + 4, 6).Value = ac.GetConvertAddress2
                End If

                If ac.GetConvertAddress2.Length > 15 Then
                    .Cell(startrowposition + 4, 6).Style.Fill.BackgroundColor = XLColor.Yellow
                Else
                    .Cell(startrowposition + 4, 6).Style.Fill.BackgroundColor = XLColor.NoColor
                End If

                '宛名
                If destinationdata.AddresseeName.GetName.Length > 5 Then
                    addresseename = $"{Space(1)}{destinationdata.AddresseeName.GetName}{destinationdata.MyTitle.GetTitle}"
                Else
                    addresseename = $"{Space(1)}{destinationdata.AddresseeName.GetName}{Space(1)}{destinationdata.MyTitle.GetTitle}"
                End If
                .Cell(startrowposition + 4, 2).Value = addresseename
            End With

        End Sub

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin

            With ExlWorkSheet
                '住所欄1行目
                .Range(.Cell(startrowposition + 4, 8), .Cell(startrowposition + 5, 9)).Merge()
                '住所欄2行目
                .Range(.Cell(startrowposition + 4, 6), .Cell(startrowposition + 5, 7)).Merge()
                '宛名欄
                .Range(.Cell(startrowposition + 4, 2), .Cell(startrowposition + 5, 3)).Merge()
            End With

        End Sub

        Public Function SetCellFont() As String Implements IExcelOutputBehavior.SetCellFont
            Return My.Resources.FontName_HGPGyoushotai
        End Function

        Private Function ColumnSizes() As Double() Implements IVerticalOutputBehavior.SetColumnSizes
            Return {21.43, 7.43, 2.71, 2.71, 2.71, 2.86, 2.86, 2.86, 2.86, 1.43}
        End Function

        Private Function SetRowSizes() As Double() Implements IVerticalOutputBehavior.SetRowSizes
            Return {101.25, 38.25, 14.25, 409.5, 133.5, 36}
        End Function

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                '宛名
                With .Cell(startrowposition + 4, 2).Style
                    .Font.FontSize = 48
                    With .Alignment
                        .Horizontal = XLAlignmentHorizontalValues.Right
                        .Vertical = XLAlignmentVerticalValues.Top
                        .TopToBottom = True
                    End With
                End With
                '郵便番号
                With .Range(.Cell(startrowposition + 2, 3), .Cell(startrowposition + 2, 9)).Style
                    .Font.FontSize = 16
                    With .Alignment
                        .Vertical = XLAlignmentVerticalValues.Top
                        .Horizontal = XLAlignmentHorizontalValues.Center
                        .TopToBottom = True
                    End With
                End With

                '住所
                With .Range(.Cell(startrowposition + 4, 6), .Cell(startrowposition + 4, 9)).Style
                    .Font.FontSize = 30
                    With .Alignment
                        .Horizontal = XLAlignmentHorizontalValues.Center
                        .Vertical = XLAlignmentVerticalValues.Center
                        .TopToBottom = True
                    End With
                End With
                .Cell(startrowposition + 4, 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                .Cell(startrowposition + 4, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top
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

        Public Function SetPrintAreaString() As String Implements IExcelOutputBehavior.SetPrintAreaString
            Return "a:j"
        End Function

        Public Function GetDestinationDataList() As ObservableCollection(Of DestinationDataEntity) Implements IVerticalOutputListBehavior.GetDestinationDataList
            Return AddresseeList
        End Function

        Public Function GetAddressMaxLength() As Integer Implements IVerticalOutputListBehavior.GetAddressMaxLength
            Return 15
        End Function

        Public Function GetLengthVerificationString(destinationData As DestinationDataEntity) As String Implements IVerticalOutputListBehavior.GetLengthVerificationString
            Return destinationData.MyAddress2.Address
        End Function
    End Class

    ''' <summary>
    ''' 振込用紙発行クラス
    ''' </summary>
    Private Class TransferPaper
        Implements IVerticalOutputListBehavior

        Private ReadOnly AddresseeList As ObservableCollection(Of DestinationDataEntity)

        Sub New(ByVal _addressee As DestinationDataEntity)
            AddresseeList = New ObservableCollection(Of DestinationDataEntity) From {_addressee}
        End Sub

        Sub New(ByVal _addresseelist As ObservableCollection(Of DestinationDataEntity))
            AddresseeList = _addresseelist
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
            joinaddress = $"{address1}{address2}"

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

        Public Function SetCellFont() As String Implements IExcelOutputBehavior.SetCellFont
            Return My.Resources.FontName_MSPMintyo
        End Function

        Private Function SetColumnSizes() As Double() Implements IVerticalOutputBehavior.SetColumnSizes
            Return {3.71, 25.14, 7.57, 1.71, 1.71, 1.71, 1.71, 1.71, 1.71, 1.71, 1.71, 7.29, 1.71, 1.71, 1.71, 1.71, 2.14, 1.71, 1.71, 1.71, 0.31}
        End Function

        Private Function SetRowSizes() As Double() Implements IVerticalOutputBehavior.SetRowSizes
            Return {283.5, 172.5, 19.5, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 101.25}
        End Function

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                .PageSetup.PaperSize = XLPaperSize.B5Paper
                '宛名欄
                .Cell(12, 2).Style.Font.FontSize = 14
                '金額欄
                With .Range(.Cell(startrowposition + 3, 4), .Cell(startrowposition + 3, 11)).Style
                    .Font.FontSize = 14
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                End With

                'お客様控え金額欄
                With .Range(.Cell(startrowposition + 9, 13), .Cell(startrowposition + 9, 20)).Style
                    .Font.FontSize = 14
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                End With

                '備考欄1〜5
                With .Range(.Cell(startrowposition + 6, 4), .Cell(startrowposition + 10, 4)).Style
                    .Font.FontSize = 9
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                    .Alignment.TopToBottom = False
                End With
                '住所欄
                With .Range(.Cell(startrowposition + 7, 2), .Cell(startrowposition + 10, 2)).Style
                    .Font.FontSize = 9
                    .Alignment.TopToBottom = False
                End With

                'お客様控え住所欄
                With .Range(.Cell(startrowposition + 10, 13), .Cell(startrowposition + 13, 13)).Style
                    .Font.FontSize = 9
                    .Alignment.TopToBottom = False
                End With
                .Range(.Cell(startrowposition + 10, 13), .Cell(startrowposition + 12, 13)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                .Cell(startrowposition + 13, 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
            End With

        End Sub

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin

            Dim row As Integer

            '宛名備考欄5行を結合
            row = 6
            Do Until row = 11
                With ExlWorkSheet
                    .Range(.Cell(startrowposition + row, 4), .Cell(startrowposition + row, 11)).Merge()
                End With
                row += 1
            Loop

            'お客様控え欄4行を結合
            row = 10
            Do Until row = 14
                With ExlWorkSheet
                    .Range(.Cell(startrowposition + row, 13), .Cell(startrowposition + row, 20)).Merge()
                End With
                row += 1
            Loop

        End Sub

        Public Sub SetData(startrowposition As Integer, destinationdata As DestinationDataEntity) Implements IVerticalOutputListBehavior.SetData

            With ExlWorkSheet
                '振込金額入力
                Dim ColumnIndex As Integer = 0
                Dim moneystring As String = $"\{destinationdata.MoneyData.GetMoney}"
                Do Until ColumnIndex = moneystring.Length
                    .Cell(startrowposition + 3, 11 - ColumnIndex).Value = moneystring.Substring((moneystring.Length - 1) - ColumnIndex, 1)
                    .Cell(startrowposition + 9, 20 - ColumnIndex).Value = moneystring.Substring((moneystring.Length - 1) - ColumnIndex, 1)    'お客様控え
                    ColumnIndex += 1
                Loop

                .Cell(startrowposition + 6, 4).Value = destinationdata.Note1Data.GetNote   '備考1
                .Cell(startrowposition + 7, 4).Value = destinationdata.Note2Data.GetNote   '備考2
                .Cell(startrowposition + 8, 4).Value = destinationdata.Note3Data.GetNote   '備考3
                .Cell(startrowposition + 9, 4).Value = destinationdata.Note4Data.GetNote  '備考4
                .Cell(startrowposition + 10, 4).Value = destinationdata.Note5Data.GetNote  '備考5
                .Cell(startrowposition + 7, 2).Value = $"〒{destinationdata.MyPostalCode.GetCode}"      '宛先郵便番号
                Dim ac As New AddressConvert(destinationdata.MyAddress1.Address, destinationdata.MyAddress2.Address)
                .Cell(startrowposition + 8, 2).Value = ac.GetConvertAddress1         '宛先住所1

                Dim stringlength As Integer
                If destinationdata.MyAddress2.Address.Length < 20 Then
                    stringlength = destinationdata.MyAddress2.Address.Length
                Else
                    stringlength = 18
                End If
                '宛先住所2　長い場合は2行で入力
                .Cell(startrowposition + 9, 2).Value = destinationdata.MyAddress2.Address.Substring(0, stringlength)
                If destinationdata.MyAddress2.Address.Length > stringlength Then .Cell(startrowposition + 10, 2).Value = destinationdata.MyAddress2.Address.Substring(stringlength)

                .Cell(startrowposition + 12, 2).Value = $"{destinationdata.AddresseeName.GetName}{Space(1)}{destinationdata.MyTitle.GetTitle}"  '宛先の宛名
                .Cell(startrowposition + 13, 13).Value = $"{destinationdata.AddresseeName.GetName}{Space(1)}{destinationdata.MyTitle.GetTitle}" 'お客様控えの名前

                'お客様控え住所　長い場合は3行、それでも収まらない場合は注意を促す
                Dim strings() As String = SplitYourCopyAddress(ac.GetConvertAddress1, destinationdata.MyAddress2.Address)
                .Cell(startrowposition + 10, 13).Value = $"{Space(1)}{strings(0)}"
                .Cell(startrowposition + 11, 13).Value = $"{Space(1)}{ strings(1)}"
                .Cell(startrowposition + 12, 13).Value = $"{Space(1)}{ strings(2)}"
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

        Public Function SetPrintAreaString() As String Implements IExcelOutputBehavior.SetPrintAreaString
            Return "a:u"
        End Function

        Public Function GetDestinationDataList() As ObservableCollection(Of DestinationDataEntity) Implements IVerticalOutputListBehavior.GetDestinationDataList
            Return AddresseeList
        End Function

        Public Function GetAddressMaxLength() As Integer Implements IVerticalOutputListBehavior.GetAddressMaxLength
            Return 36
        End Function

        Private Function GetLengthVerificationString(destinationData As DestinationDataEntity) As String Implements IVerticalOutputListBehavior.GetLengthVerificationString
            Return $"{destinationData.MyAddress1.Address}{destinationData.MyAddress2.Address}"
        End Function
    End Class

    ''' <summary>
    ''' 洋封筒クラス 
    ''' </summary>
    Private Class WesternEnvelope
        Implements IVerticalOutputListBehavior

        Private ReadOnly AddresseeList As ObservableCollection(Of DestinationDataEntity)

        Sub New(ByVal _addressee As DestinationDataEntity)
            AddresseeList = New ObservableCollection(Of DestinationDataEntity) From {_addressee}
        End Sub

        Sub New(ByVal _addresseelist As ObservableCollection(Of DestinationDataEntity))
            AddresseeList = _addresseelist
        End Sub

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputListBehavior.CellProperty

            With ExlWorkSheet
                .PageSetup.PaperSize = XLPaperSize.C6Envelope
                '郵便番号
                With .Range(.Cell(startrowposition + 2, 3), .Cell(startrowposition + 2, 9)).Style
                    .Font.FontSize = 16
                    .Alignment.Vertical = XLAlignmentVerticalValues.Top
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                End With
                '住所
                With .Range(.Cell(startrowposition + 4, 6), .Cell(startrowposition + 4, 8)).Style
                    .Font.FontSize = 24
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Alignment.TopToBottom = True
                End With
                .Cell(startrowposition + 4, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top
                .Cell(startrowposition + 4, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                '宛名
                With .Cell(startrowposition + 4, 2).Style
                    .Font.FontSize = 36
                    With .Alignment
                        .Horizontal = XLAlignmentHorizontalValues.Center
                        .Vertical = XLAlignmentVerticalValues.Top
                        .TopToBottom = True
                    End With
                End With
            End With

        End Sub

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin

            With ExlWorkSheet
                .Range(.Cell(startrowposition + 4, 2), .Cell(startrowposition + 4, 4)).Merge()
                .Range(.Cell(startrowposition + 4, 6), .Cell(startrowposition + 4, 7)).Merge()
                .Range(.Cell(startrowposition + 4, 8), .Cell(startrowposition + 4, 9)).Merge()
            End With

        End Sub

        Public Sub SetData(startrowposition As Integer, destinationdata As DestinationDataEntity) Implements IVerticalOutputListBehavior.SetData

            Dim addresstext1 As String = String.Empty
            Dim addresstext2 As String = String.Empty
            Dim addresseename As String

            With ExlWorkSheet
                '郵便番号
                For I As Integer = 1 To 7
                    .Cell(startrowposition + 2, I + 2).Value = Replace(destinationdata.MyPostalCode.GetCode, "-", String.Empty).Substring(I - 1, 1)
                Next

                Dim ac As New AddressConvert(destinationdata.MyAddress1.Address, destinationdata.MyAddress2.Address)
                '住所
                If ac.GetConvertAddress1.Length + ac.GetConvertAddress2.Length < 14 Then
                    addresstext1 = $"{ac.GetConvertAddress1}{Space(1)}{ac.GetConvertAddress2}"
                Else
                    addresstext1 = ac.GetConvertAddress1
                    addresstext2 = ac.GetConvertAddress2
                End If

                If ac.GetConvertAddress2.Length > 16 Then
                    .Cell(startrowposition + 4, 6).Style.Fill.BackgroundColor = XLColor.Yellow
                Else
                    .Cell(startrowposition + 4, 6).Style.Fill.BackgroundColor = XLColor.NoColor
                End If

                .Cell(startrowposition + 4, 8).Value = ac.GetConvertAddress1
                .Cell(startrowposition + 4, 6).Value = ac.GetConvertAddress2

                '宛名
                If destinationdata.AddresseeName.GetName.Length > 5 Then
                    addresseename = $"{Space(1)}{destinationdata.AddresseeName.GetName}{destinationdata.MyTitle.GetTitle}"
                Else
                    addresseename = $"{Space(1)}{destinationdata.AddresseeName.GetName}{Space(1)}{destinationdata.MyTitle.GetTitle}"
                End If
                .Cell(startrowposition + 4, 2).Value = addresseename
            End With

        End Sub

        Public Function SetCellFont() As String Implements IExcelOutputBehavior.SetCellFont
            Return My.Resources.FontName_HGPGyoushotai
        End Function

        Public Function CriteriaCellRowIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellRowIndex
            Return 4
        End Function

        Public Function CriteriaCellColumnIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellColumnIndex
            Return 2
        End Function

        Public Function GetDataName() As String Implements IVerticalOutputBehavior.GetDataName
            Return ToString()
        End Function

        Public Function SetPrintAreaString() As String Implements IExcelOutputBehavior.SetPrintAreaString
            Return "a:j"
        End Function

        Public Function GetDestinationDataList() As ObservableCollection(Of DestinationDataEntity) Implements IVerticalOutputListBehavior.GetDestinationDataList
            Return AddresseeList
        End Function

        Public Function GetAddressMaxLength() As Integer Implements IVerticalOutputListBehavior.GetAddressMaxLength
            Return 36
        End Function

        Private Function SetColumnSizes() As Double() Implements IVerticalOutputBehavior.SetColumnSizes
            Return {17.88, 6, 2.75, 2.75, 2.75, 2.38, 2.38, 2.38, 2.38, 0.85}
        End Function

        Private Function IExcelOutputBehavior_SetRowSizes() As Double() Implements IVerticalOutputBehavior.SetRowSizes
            Return {22.5, 18.75, 27.75, 372}
        End Function

        Private Function GetLengthVerificationString(destinationData As DestinationDataEntity) As String Implements IVerticalOutputListBehavior.GetLengthVerificationString
            Return destinationData.MyAddress2.Address
        End Function
    End Class

    ''' <summary>
    ''' 角2封筒クラス
    ''' </summary>
    Private Class Kaku2Envelope
        Implements IVerticalOutputListBehavior

        Private ReadOnly AddresseeList As ObservableCollection(Of DestinationDataEntity)

        Sub New(ByVal _addressee As DestinationDataEntity)
            AddresseeList = New ObservableCollection(Of DestinationDataEntity) From {_addressee}
        End Sub

        Sub New(ByVal _addresseelist As ObservableCollection(Of DestinationDataEntity))
            AddresseeList = _addresseelist
        End Sub

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                '郵便番号
                With .Cell(startrowposition + 2, 3).Style
                    .Font.FontSize = 36
                    With .Alignment
                        .Horizontal = XLAlignmentHorizontalValues.Center
                        .Vertical = XLAlignmentVerticalValues.Bottom
                        .TopToBottom = False
                    End With
                End With

                '住所
                With .Range(.Cell(startrowposition + 4, 5), .Cell(startrowposition + 4, 4)).Style
                    .Font.FontSize = 43
                    With .Alignment
                        .Horizontal = XLAlignmentHorizontalValues.Center
                        .Vertical = XLAlignmentVerticalValues.Top
                        .TopToBottom = True
                    End With
                End With
                .Cell(startrowposition + 4, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

                '宛名
                With .Cell(startrowposition + 5, 2).Style
                    .Font.FontSize = 74
                    With .Alignment
                        .Horizontal = XLAlignmentHorizontalValues.Center
                        .Vertical = XLAlignmentVerticalValues.Top
                        .TopToBottom = True
                    End With
                End With
            End With

        End Sub

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin

            With ExlWorkSheet
                .Range(.Cell(startrowposition + 2, 3), .Cell(startrowposition + 2, 5)).Merge()
                .Range(.Cell(startrowposition + 5, 2), .Cell(startrowposition + 6, 2)).Merge()
                .Range(.Cell(startrowposition + 4, 4), .Cell(startrowposition + 6, 4)).Merge()
                .Range(.Cell(startrowposition + 4, 5), .Cell(startrowposition + 6, 5)).Merge()
            End With

        End Sub

        Public Sub SetData(startrowposition As Integer, destinationdata As DestinationDataEntity) Implements IVerticalOutputListBehavior.SetData

            With ExlWorkSheet
                '郵便番号
                .Cell(startrowposition + 2, 3).Value = $"〒{destinationdata.MyPostalCode.GetCode}"
                '住所
                Dim ac As New AddressConvert(destinationdata.MyAddress1.Address, destinationdata.MyAddress2.Address)
                If $"{ac.GetConvertAddress1}{ac.GetConvertAddress2}".Length < 16 Then
                    .Cell(startrowposition + 4, 5).Value = $"{ac.GetConvertAddress1}{ac.GetConvertAddress2}"
                    .Cell(startrowposition + 4, 4).Value = String.Empty
                Else
                    .Cell(startrowposition + 4, 5).Value = ac.GetConvertAddress1
                    .Cell(startrowposition + 4, 4).Value = ac.GetConvertAddress2
                End If
                '宛名
                .Cell(startrowposition + 5, 2).Value = $"{destinationdata.AddresseeName.GetName}{destinationdata.MyTitle.GetTitle}"
            End With

        End Sub

        Public Function SetCellFont() As String Implements IExcelOutputBehavior.SetCellFont
            Return My.Resources.FontName_HGPGyoushotai
        End Function

        Public Function CriteriaCellRowIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellRowIndex
            Return 4
        End Function

        Public Function CriteriaCellColumnIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellColumnIndex
            Return 2
        End Function

        Private Function SetColumnSizes() As Double() Implements IVerticalOutputBehavior.SetColumnSizes
            Return {45.29, 23.43, 15.43, 9.57, 9.57, 8.57}
        End Function

        Private Function SetRowSizes() As Double() Implements IVerticalOutputBehavior.SetRowSizes
            Return {120, 50.25, 61.5, 61.5, 409.5, 279, 67.5}
        End Function

        Public Function GetDataName() As String Implements IVerticalOutputBehavior.GetDataName
            Return ToString()
        End Function

        Public Function SetPrintAreaString() As String Implements IExcelOutputBehavior.SetPrintAreaString
            Return "a:f"
        End Function

        Public Function GetDestinationDataList() As ObservableCollection(Of DestinationDataEntity) Implements IVerticalOutputListBehavior.GetDestinationDataList
            Return AddresseeList
        End Function

        Public Function GetAddressMaxLength() As Integer Implements IVerticalOutputListBehavior.GetAddressMaxLength
            Return 0
        End Function

        Private Function GetLengthVerificationString(destinationData As DestinationDataEntity) As String Implements IVerticalOutputListBehavior.GetLengthVerificationString
            Return String.Empty
        End Function

    End Class

    ''' <summary>
    ''' 墓地パンフクラス
    ''' </summary>
    Private Class GravePamphletEnvelope
        Implements IVerticalOutputListBehavior

        Private ReadOnly AddresseeList As ObservableCollection(Of DestinationDataEntity)

        Sub New(ByVal _addressee As DestinationDataEntity)
            AddresseeList = New ObservableCollection(Of DestinationDataEntity) From {_addressee}
        End Sub

        Sub New(ByVal _addresseelist As ObservableCollection(Of DestinationDataEntity))
            AddresseeList = _addresseelist
        End Sub

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin

            With ExlWorkSheet
                .Range(.Cell(startrowposition + 2, 3), .Cell(startrowposition + 2, 5)).Merge()
                .Range(.Cell(startrowposition + 4, 2), .Cell(startrowposition + 5, 2)).Merge()
                .Range(.Cell(startrowposition + 4, 4), .Cell(startrowposition + 5, 4)).Merge()
                .Range(.Cell(startrowposition + 4, 5), .Cell(startrowposition + 5, 5)).Merge()
            End With

        End Sub

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                '郵便番号
                With .Cell(startrowposition + 2, 3).Style
                    .Font.FontSize = 36
                    With .Alignment
                        .Horizontal = XLAlignmentHorizontalValues.Center
                        .Vertical = XLAlignmentVerticalValues.Bottom
                        .TopToBottom = False
                    End With
                End With

                '住所
                With .Range(.Cell(startrowposition + 4, 5), .Cell(startrowposition + 4, 4)).Style
                    .Font.FontSize = 43
                    With .Alignment
                        .Horizontal = XLAlignmentHorizontalValues.Center
                        .Vertical = XLAlignmentVerticalValues.Top
                        .TopToBottom = True
                    End With
                End With
                .Cell(startrowposition + 4, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

                '宛名
                With .Cell(startrowposition + 4, 2).Style
                    .Font.FontSize = 85
                    With .Alignment
                        .Horizontal = XLAlignmentHorizontalValues.Center
                        .Vertical = XLAlignmentVerticalValues.Top
                        .TopToBottom = True
                    End With
                End With
            End With

        End Sub

        Public Sub SetData(startrowposition As Integer, destinationdata As DestinationDataEntity) Implements IVerticalOutputListBehavior.SetData

            With ExlWorkSheet
                '郵便番号
                .Cell(startrowposition + 2, 3).Value = $"〒{destinationdata.MyPostalCode.GetCode}"
                '住所
                Dim ac As New AddressConvert(destinationdata.MyAddress1.Address, destinationdata.MyAddress2.Address)
                .Cell(startrowposition + 4, 5).Value = ac.GetConvertAddress1
                .Cell(startrowposition + 4, 4).Value = ac.GetConvertAddress2
                '宛名
                .Cell(startrowposition + 4, 2).Value = $"{destinationdata.AddresseeName.GetName}{Space(1)}{destinationdata.MyTitle.GetTitle}"
            End With

        End Sub

        Public Function SetCellFont() As String Implements IExcelOutputBehavior.SetCellFont
            Return My.Resources.FontName_HGPGyoushotai
        End Function

        Public Function CriteriaCellRowIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellRowIndex
            Return 4
        End Function

        Public Function CriteriaCellColumnIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellColumnIndex
            Return 2
        End Function

        Private Function SetColumnSizes() As Double() Implements IVerticalOutputBehavior.SetColumnSizes
            Return {49.43, 23.43, 28.86, 9.43, 9.43}
        End Function

        Private Function SetRowSizes() As Double() Implements IVerticalOutputBehavior.SetRowSizes
            Return {45.75, 132.75, 51.75, 409.5, 409.5, 9}
        End Function

        Public Function GetDataName() As String Implements IVerticalOutputBehavior.GetDataName
            Return ToString()
        End Function

        Public Function SetPrintAreaString() As String Implements IExcelOutputBehavior.SetPrintAreaString
            Return "a:e"
        End Function

        Public Function GetDestinationDataList() As ObservableCollection(Of DestinationDataEntity) Implements IVerticalOutputListBehavior.GetDestinationDataList
            Return AddresseeList
        End Function

        Public Function GetAddressMaxLength() As Integer Implements IVerticalOutputListBehavior.GetAddressMaxLength
            Return 0
        End Function

        Public Function GetLengthVerificationString(destinationData As DestinationDataEntity) As String Implements IVerticalOutputListBehavior.GetLengthVerificationString
            Return String.Empty
        End Function
    End Class

    ''' <summary>
    ''' はがきクラス
    ''' </summary>
    Private Class Postcard
        Implements IVerticalOutputListBehavior

        Private ReadOnly AddresseeList As ObservableCollection(Of DestinationDataEntity)

        Sub New(ByVal _addressee As DestinationDataEntity)
            AddresseeList = New ObservableCollection(Of DestinationDataEntity) From {_addressee}
        End Sub

        Sub New(ByVal _addresseelist As ObservableCollection(Of DestinationDataEntity))
            AddresseeList = _addresseelist
        End Sub

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                .PageSetup.PaperSize = XLPaperSize.EPaper
                '郵便番号
                With .Range(.Cell(startrowposition + 2, 3), .Cell(startrowposition + 2, 9)).Style
                    .Font.FontSize = 16
                    .Alignment.Vertical = XLAlignmentVerticalValues.Top
                End With
                .Range(.Cell(startrowposition + 2, 6), .Cell(startrowposition + 2, 9)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                .Range(.Cell(startrowposition + 2, 3), .Cell(startrowposition + 2, 5)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left

                '住所
                With .Range(.Cell(startrowposition + 4, 6), .Cell(startrowposition + 4, 8)).Style
                    .Font.FontSize = 18
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                    .Alignment.TopToBottom = True
                End With
                .Cell(startrowposition + 4, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top
                .Cell(startrowposition + 4, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                '宛名
                With .Cell(startrowposition + 4, 2).Style
                    .Font.FontSize = 36
                    With .Alignment
                        .Horizontal = XLAlignmentHorizontalValues.Center
                        .Vertical = XLAlignmentVerticalValues.Top
                        .TopToBottom = True
                    End With
                End With
            End With

        End Sub

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin

            With ExlWorkSheet
                .Range(.Cell(startrowposition + 4, 2), .Cell(startrowposition + 4, 5)).Merge()
                .Range(.Cell(startrowposition + 4, 8), .Cell(startrowposition + 4, 9)).Merge()
                .Range(.Cell(startrowposition + 4, 6), .Cell(startrowposition + 4, 7)).Merge()
            End With

        End Sub

        Public Sub SetData(startrowposition As Integer, destinationdata As DestinationDataEntity) Implements IVerticalOutputListBehavior.SetData

            Dim addressText1 As String = String.Empty
            Dim addressText2 As String = String.Empty
            Dim addresseeName, postalcode As String

            With ExlWorkSheet
                postalcode = Replace(destinationdata.MyPostalCode.GetCode, "-", String.Empty)

                '郵便番号
                For I As Integer = 1 To 7
                    .Cell(startrowposition + 2, I + 2).Value = postalcode.Substring(I - 1, 1)
                Next

                '住所
                Dim ac As New AddressConvert(destinationdata.MyAddress1.Address, destinationdata.MyAddress2.Address)
                If ac.GetConvertAddress1.Length + ac.GetConvertAddress2.Length < 14 Then
                    addressText1 = $"{ac.GetConvertAddress1}{Space(1)}{ac.GetConvertAddress2}"
                    addressText2 = String.Empty
                Else
                    addressText1 = ac.GetConvertAddress1
                    addressText2 = ac.GetConvertAddress2
                End If
                If ac.GetConvertAddress2.Length > 14 Then
                    .Cell(startrowposition + 4, 6).Style.Fill.BackgroundColor = XLColor.Yellow
                Else
                    .Cell(startrowposition + 4, 6).Style.Fill.BackgroundColor = XLColor.NoColor
                End If
                .Cell(startrowposition + 4, 8).Value = addressText1
                .Cell(startrowposition + 4, 6).Value = addressText2

                '宛名
                If destinationdata.AddresseeName.GetName.Length > 5 Then
                    addresseeName = $"{destinationdata.AddresseeName.GetName}{destinationdata.MyTitle.GetTitle}"
                Else
                    addresseeName = $"{destinationdata.AddresseeName.GetName}{Space(1)}{destinationdata.MyTitle.GetTitle}"
                End If
                .Cell(startrowposition + 4, 2).Value = addresseeName
            End With

        End Sub

        Public Function SetCellFont() As String Implements IExcelOutputBehavior.SetCellFont
            Return My.Resources.FontName_HGPGyoushotai
        End Function

        Public Function CriteriaCellRowIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellRowIndex
            Return 4
        End Function

        Public Function CriteriaCellColumnIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellColumnIndex
            Return 2
        End Function

        Public Function GetDataName() As String Implements IVerticalOutputBehavior.GetDataName
            Return ToString()
        End Function

        Public Function SetPrintAreaString() As String Implements IExcelOutputBehavior.SetPrintAreaString
            Return "a:i"
        End Function

        Public Function GetDestinationDataList() As ObservableCollection(Of DestinationDataEntity) Implements IVerticalOutputListBehavior.GetDestinationDataList
            Return AddresseeList
        End Function

        Public Function GetAddressMaxLength() As Integer Implements IVerticalOutputListBehavior.GetAddressMaxLength
            Return 14
        End Function

        Private Function SetColumnSizes() As Double() Implements IVerticalOutputBehavior.SetColumnSizes
            Return {15.57, 3.57, 2.71, 2.71, 2.71, 2.71, 2.71, 2.71, 2.71}
        End Function

        Private Function SetRowSizes() As Double() Implements IVerticalOutputBehavior.SetRowSizes
            Return {30, 22.5, 22.5, 326.25}
        End Function

        Private Function GetLengthVerificationString(destinationData As DestinationDataEntity) As String Implements IVerticalOutputListBehavior.GetLengthVerificationString
            Return destinationData.MyAddress2.Address
        End Function
    End Class

    ''' <summary>
    ''' ラベルシートクラス
    ''' </summary>
    Private Class LabelSheet
        Implements IHorizontalOutputBehavior

        Private ReadOnly myAddressee As DestinationDataEntity

        Sub New(ByVal addressee As DestinationDataEntity)
            myAddressee = addressee
        End Sub

        ''' <summary>
        ''' ラベルに入力する文字列を返します
        ''' </summary>
        ''' <param name="lineindex">行番号</param>
        ''' <param name="addressee">ラベル化する宛先</param>
        ''' <returns></returns>
        Private Function ReturnLabelString(ByVal lineindex As Integer, ByVal addressee As DestinationDataEntity) As String

            'セルに入力する宛先を格納する文字列　初期値に郵便番号
            Dim ReturnString As String = $"{Space(10)}〒 {addressee.MyPostalCode.GetCode}{vbNewLine}{vbNewLine}"
            Dim ac As New AddressConvert(addressee.MyAddress1.Address, addressee.MyAddress2.Address)
            ReturnString &= $"{Space(10)}{ac.GetConvertAddress1}{vbCrLf}"  '住所1

            Try
                ReturnString &= $"{Space(10)}{addressee.MyAddress2.Address.Substring(0, 16)}{vbNewLine}"   '住所2
                ReturnString &= $"{Space(10)}{addressee.MyAddress2.Address.Substring(16)}{vbNewLine}{vbNewLine}" '住所2（2行目）
            Catch ex As ArgumentOutOfRangeException
                '住所2の文字列が短い場合のエラー対応（16文字以下ならエラー）
                ReturnString &= $"{Space(10)}{addressee.MyAddress2.Address}{vbNewLine}{vbNewLine}{vbNewLine}"
            End Try

            '宛名
            ReturnString &= $"{Space(10)}{addressee.AddresseeName.GetName}{Space(1)}{addressee.MyTitle.GetTitle}{vbNewLine}"

            'ラベルの行数によって、行を挿入する
            If lineindex Mod 6 = 0 Then
                ReturnString = $"{vbNewLine}{vbNewLine}{ReturnString}"
                Return ReturnString
            End If

            If lineindex Mod 7 = 0 Then ReturnString = $"{vbNewLine}{vbNewLine}{vbNewLine}{ReturnString}"

            Return ReturnString

        End Function

        Public Function SetCellFont() As String Implements IExcelOutputBehavior.SetCellFont
            Return My.Resources.FontName_MSPGothic
        End Function

        Public Sub CellProperty(startrowposition As Integer) Implements IHorizontalOutputBehavior.CellProperty

            With ExlWorkSheet
                .PageSetup.PaperSize = XLPaperSize.A4Paper
                With .Style
                    .Font.FontSize = 10
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.TextRotation = False
                End With
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

        Private Sub SetData(destinationdata As DestinationDataEntity) Implements IHorizontalOutputBehavior.SetData

            Dim column As Integer = 1
            Dim row As Integer = 1

            With ExlWorkSheet
                Do Until .Cell(row, column).Value.Trim.Length = 0
                    column += 1
                    If column > 3 Then
                        column = 1
                        row += 1
                    End If
                Loop

                .Cell(row, column).Value = ReturnLabelString(row, destinationdata)
            End With

        End Sub

        Public Function SetPrintAreaString() As String Implements IExcelOutputBehavior.SetPrintAreaString
            Return "a:c"
        End Function

    End Class

End Class
