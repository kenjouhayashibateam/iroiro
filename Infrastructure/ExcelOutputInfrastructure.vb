﻿Imports ClosedXML.Excel
Imports Microsoft.Office.Interop
Imports Domain
Imports System.Text.RegularExpressions
Imports System.Collections.ObjectModel
Imports System.Globalization

''' <summary>
''' 住所を宛先用に変換します
''' </summary>
Friend Interface IAddressConvert

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
Friend Interface IExcelOutputBehavior

    ''' <summary>
    ''' シート全体のフォントを設定します
    ''' </summary>
    Function SetCellFont(_isIPAmjMintyo As Boolean) As String

    ''' <summary>
    ''' セルのフォントサイズ、フォントポジション等を設定します
    ''' </summary>
    ''' <param name="startrowposition"></param>
    Sub CellProperty(startrowposition As Integer)

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
Friend Interface IHorizontalOutputBehavior
    Inherits IExcelOutputBehavior

    ''' <summary>
    ''' 出力するデータをセットします
    ''' </summary>
    Sub SetData(destinationdata As DestinationDataEntity)


    ''' <summary>
    ''' 宛名クラスを保持するリスト
    ''' </summary>
    ''' <returns></returns>
    Function GetDestinationDataList() As ObservableCollection(Of DestinationDataEntity)

End Interface

''' <summary>
''' データのリストを縦向けに出力
''' </summary>
Friend Interface IVerticalOutputListBehavior
    Inherits IVerticalOutputBehavior

    ''' <summary>
    ''' 出力するデータをセットします
    ''' </summary>
    ''' <param name="startrowposition"></param>
    Sub SetData(startrowposition As Integer, destinationdata As DestinationDataEntity)

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
    Function GetLengthVerificationString(destinationData As DestinationDataEntity) As String

End Interface

''' <summary>
''' 墓地札データを出力
''' </summary>
Friend Interface IGravePanelListBehavior
    Inherits IVerticalOutputBehavior

    ''' <summary>
    ''' 出力するデータをセットします
    ''' </summary>
    ''' <param name="startrowposition"></param>
    Sub SetData(startrowposition As Integer, gravepanel As GravePanelDataEntity)

End Interface

''' <summary>
''' エクセルデータを縦向けに出力
''' </summary>
Friend Interface IVerticalOutputBehavior
    Inherits IExcelOutputBehavior

    ''' <summary>
    ''' 結合するセルを設定します
    ''' </summary>
    ''' <param name="startrowposition"></param>
    Sub CellsJoin(startrowposition As Integer)

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

    Public Sub New(_address1 As String, _address2 As String)
        Address1 = _address1
        Address2 = _address2
    End Sub

    ''' <summary>
    ''' 住所の都道府県を省略できる住所は、都道府県を除いて返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetConvertAddress1() As String Implements IAddressConvert.GetConvertAddress1

        Dim AddressText As String
        Dim isReturn As Boolean

        AddressText = Address1
        '東京、神奈川、徳島は略す
        AddressText = Replace(AddressText, My.Resources.ToukyouString, String.Empty)
        AddressText = Replace(AddressText, My.Resources.KanagawaString, String.Empty)
        AddressText = Replace(AddressText, My.Resources.TokushimaString, String.Empty)
        If AddressText Is Nothing Then Return String.Empty

        isReturn = Address1.Length <> AddressText.Length

        '”("から先を削除する
        Dim addressarray() As String = Split(AddressText, My.Resources.FullWidthClosingParenthesis)
        AddressText = addressarray(0)

        If isReturn Then Return AddressText

        '郡が入っている住所はそのまま返す
        If InStr(AddressText, My.Resources.GunString) <> 0 Then Return AddressText

        If InStr(AddressText, My.Resources.KenString) <> 0 Then
            '県と市を比べる
            AddressText = VerifyAddressString(AddressText, My.Resources.KenString)
        Else
            '府と市を比べる
            AddressText = VerifyAddressString(AddressText, My.Resources.FuString)
        End If

        Return AddressText

    End Function

    ''' <summary>
    ''' 検証する県、府が市と同じ名前の場合、市から始まる住所にして返します
    ''' </summary>
    ''' <param name="address">住所</param>
    ''' <param name="verifystring">検証する文字列</param>
    ''' <returns></returns>
    Private Function VerifyAddressString(address As String, verifystring As String) As String

        If InStr(address, verifystring) = 0 Then Return address
        If InStr(1, address, My.Resources.ShiString) = 0 Then Return address

        '検証する文字列の名称、京都府や広島県等と市の名称、京都市、広島市などが同じなら省略する
        Return If(address.Substring(0, InStr(1, address, verifystring) - 1) = address.Substring(InStr(1, address, verifystring), InStr(1, address, My.Resources.ShiString) - InStr(1, address, verifystring) - 1),
            address.Substring(InStr(1, address, verifystring)),
            address)

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
        basestring = Regex.Replace(basestring, "[－―]", "ー")
        If basestring Is Nothing Then Return String.Empty
        '*を空欄に置換して値を返す
        Return Replace(basestring, "*", String.Empty)

    End Function

    ''' <summary>
    ''' 文字列の数字を漢字に変換します。数字出ない場合は引数をそのまま返します
    ''' </summary>
    ''' <param name="addressString">変換文字列</param>
    ''' <returns></returns>
    Private Function BranchConvertNumber(addressString As String) As String

        Dim rx As New Regex("^[\d]+$")

        Return If(rx.IsMatch(addressString), ConvertNumber(addressString), addressString)

    End Function

    ''' <summary>
    ''' 数字を漢字変換して返します
    ''' </summary>
    ''' <param name="mynumber">変換する数字</param>
    ''' <returns></returns>
    Private Function ConvertNumber(mynumber As Integer) As String
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
    Private Function ConvertNumber_Under10(myNumber As Integer) As String

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
    Private Function ConvertNumber_Over11Under19(myNumber As Integer) As String

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
    Private Function ConvertNumber_Orver20(myNumber As Integer) As String

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
    ''' ログファイルを生成します
    ''' </summary>
    ''' <returns></returns>
    Private Property LogFileConecter As ILoggerRepogitory

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
    Friend Property ProcessedCount As Integer

    Public Sub New()
        Me.New(New LogFileInfrastructure)
    End Sub
    Public Sub New(_logger As ILoggerRepogitory)
        LogFileConecter = _logger
    End Sub

    ''' <summary>
    ''' いろいろ発行エクセルファイルを閉じて、メモリ上にClosedXMLのSheetを生成します。
    ''' </summary>
    Private Sub SheetSetting()

        Try
            ExcelClose()

            ExlWorkbook = New XLWorkbook
            If ExlWorkSheet Is Nothing Then ExlWorkSheet = ExlWorkbook.AddWorksheet(My.Resources.FILENAME)
            Dim unused = ExlWorkSheet.Cells.Clear()
            Dim i As Integer = ExlWorkSheet.Pictures.Count
            Dim j As Integer = 1
            Do Until i < j
                ExlWorkSheet.Pictures.Delete($"Picture {j}")

                j += 1
            Loop
            For Each p In ExlWorkSheet.Pictures
                p.Delete()
            Next

            ExlWorkSheet.Cells.Style.NumberFormat.NumberFormatId = 49
        Catch ex As Exception
            LogFileConecter.Log(ILoggerRepogitory.LogInfo.ERR, ex.Message)
        End Try

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

        Try
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
        Catch ex As Exception
            LogFileConecter.Log(ILoggerRepogitory.LogInfo.ERR, ex.Message)
        End Try

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
            'Dim openpath As String = "Z:\生田フォルダ\Tools\Applications\testIroIro\files\IroiroFile.xlsx"
            Dim openpath As String = IO.Path.GetFullPath(My.Resources.SAVEPATH)
            Try
                Dim executebook As Excel.Workbook = exlworkbooks.Open(openpath, , True)
                executebook.Activate()
            Catch ex As Exception
                Dim unused = MsgBox("開いているエクセルが編集モードの為、出力できません。モードを解除してから出力して下さい。" & vbNewLine & vbNewLine &
                       "もし、編集モードが原因でない場合は、林飛を呼んでください。", MsgBoxStyle.Critical, "出力が弱い")
                LogFileConecter.Log(ILoggerRepogitory.LogInfo.ERR, ex.StackTrace)
                Exit Sub
            End Try

        End If

    End Sub

    ''' <summary>
    ''' 入力するでーたの印刷範囲の一番上のRowを返します
    ''' </summary>
    ''' <returns></returns>
    Private Function SetStartRowPosition(vob As IVerticalOutputBehavior) As Integer

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
    Private Sub SettingNewSheet_Horizontal(eob As IExcelOutputBehavior)

        ColumnSizes = eob.SetColumnSizes
        RowSizes = eob.SetRowSizes

        If OutputDataGanre = eob.GetDataName Then Exit Sub
        OutputDataGanre = eob.GetDataName
        DataClear()
        SetMargin()

        With ExlWorkSheet
            .PageSetup.PrintAreas.Clear()
            .PageSetup.PrintAreas.Add(eob.SetPrintAreaString)
        End With

    End Sub

    ''' <summary>
    ''' 縦向けOutput用のシートをセッティングします
    ''' </summary>
    ''' <param name="eob"></param>
    Private Sub SettingNewSheet_Vertical(eob As IExcelOutputBehavior)

        ColumnSizes = eob.SetColumnSizes
        RowSizes = eob.SetRowSizes
        If OutputDataGanre = eob.GetDataName Then Exit Sub
        OutputDataGanre = eob.GetDataName
        SetMargin()

        With ExlWorkSheet
            Dim unused = .Cells.Clear()
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
    Private Sub OutputLabelProcessing(_hob As IHorizontalOutputBehavior, _isIPAmjMintyo As Boolean)

        Hob = _hob
        SheetSetting()

        Dim column As Integer = 1
        Dim row As Integer = 1
        Dim sheetindex As Integer = 0

        With ExlWorkSheet
            '出力するデータの種類が違えばセルをクリアする
            SettingNewSheet_Horizontal(Hob)
            ProcessedCount = 0
            'カラムの幅を設定する
            For i As Integer = 0 To UBound(ColumnSizes)
                .Column(i + 1).Width = ColumnSizes(i)
            Next

            Dim linecount As Integer = 1
            For Each dde As DestinationDataEntity In Hob.GetDestinationDataList
                'ラベルのマスに値がない初めの位置と、ラベル件数からページ数を割り出し設定する
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

                'ロウの高さを設定する
                For j As Integer = 0 To UBound(RowSizes)
                    .Row(j + 1 + (sheetindex * UBound(RowSizes))).Height = RowSizes(j)
                Next

                Hob.SetData(dde)
                Hob.CellProperty(sheetindex)
                ProcessedCount += 1
                If ProcessedCountListener IsNot Nothing Then ProcessedCountListener.ProcessedCountNotify(ProcessedCount)
            Next
            .Style.Font.FontName = Hob.SetCellFont(_isIPAmjMintyo)
        End With

        If ExlWorkbook.Worksheets.Count = 0 Then ExlWorkbook.AddWorksheet(ExlWorkSheet)
        ExlWorkbook.SaveAs(My.Resources.SAVEPATH)

        ExcelOpen()

    End Sub

    ''' <summary>
    ''' 縦向けにリストのデータを入力する処理
    ''' </summary>
    ''' <param name="_vob"></param>
    ''' <param name="isMulti">複数印刷Behaviorをするかを設定します</param>
    Private Sub ListOutputVerticalProcessing(_vob As IVerticalOutputListBehavior, isMulti As Boolean, isIPAmjMintyo As Boolean)

        Dim overLengthCount As Integer = 0

        Volb = _vob
        SheetSetting()

        With ExlWorkSheet
            '出力するデータの種類が違えばセルをクリアする
            SettingNewSheet_Vertical(Volb)
            ProcessedCount = 0
            For Each dde As DestinationDataEntity In Volb.GetDestinationDataList

                '複数印刷するならポジションを設定
                If isMulti Then
                    StartRowPosition = SetStartRowPosition(Volb)
                Else
                    Dim unused = .Unmerge()
                    StartRowPosition = 0
                End If
                Volb.CellProperty(StartRowPosition)

                'RowSizesの配列の中の数字をシートのローの幅に設定する
                For I = 0 To UBound(RowSizes)
                    .Rows(StartRowPosition + I + 1).Height = RowSizes(I)
                Next

                Volb.CellsJoin(StartRowPosition)
                Volb.SetData(StartRowPosition, dde)

                ProcessedCount += 1
                If Volb.GetLengthVerificationString(dde).Length > Volb.GetAddressMaxLength Then overLengthCount += 1
                If String.IsNullOrEmpty(dde.MyPostalCode.GetCode) Then
                    Continue For
                ElseIf Replace(dde.MyPostalCode.GetCode, "-", String.Empty).Length <> 7 Then
                    overLengthCount += 1
                End If
                If ProcessedCountListener IsNot Nothing Then ProcessedCountListener.ProcessedCountNotify(ProcessedCount)
            Next
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
    Private Sub NotificationOverLengthCount(count As Integer)
        OverLengthAddressCountListener.OverLengthCountNotify(count)
    End Sub

    Private Sub VoucherOutputProcessing(_voc As Voucher)
        SheetSetting()
        ColumnSizes = _voc.SetColumnSizes
        RowSizes = _voc.SetRowSizes
        OutputDataGanre = _voc.GetDataName

        With ExlWorkSheet
            Dim unused = .Cells.Clear()
            _voc.CellsJoin(1)
            _voc.CellProperty(1)
            SetMargin()
            _voc.SetBorderStyle()
            'ColumnSizesの配列の中の数字をシートのカラムの幅に設定する
            For i As Integer = 0 To UBound(ColumnSizes)
                .Column(i + 1).Width = ColumnSizes(i)
            Next
            For i As Integer = 0 To UBound(RowSizes)
                .Row(i + 1).Height = RowSizes(i)
            Next
            .PageSetup.PrintAreas.Clear()
            .PageSetup.PrintAreas.Add(_voc.SetPrintAreaString)
            .Style.Font.FontName = _voc.SetCellFont(False)
        End With
        ExlWorkSheet.Style.NumberFormat.SetFormat("@")
        _voc.SetData()
        If ExlWorkbook.Worksheets.Count = 0 Then ExlWorkbook.AddWorksheet(ExlWorkSheet)
        ExlWorkbook.SaveAs(My.Resources.SAVEPATH)
        ExcelOpen()
    End Sub

    Private Sub GraveVoucherOutputProcessing(_gv As GraveVoucher)
        SheetSetting()
        ColumnSizes = _gv.SetColumnSizes
        RowSizes = _gv.SetRowSizes
        OutputDataGanre = _gv.GetDataName

        With ExlWorkSheet
            Dim unused = .Cells.Clear()
            _gv.CellsJoin(1)
            _gv.CellProperty(1)
            SetMargin()
            'ColumnSizesの配列の中の数字をシートのカラムの幅に設定する
            For i As Integer = 0 To UBound(ColumnSizes)
                .Column(i + 1).Width = ColumnSizes(i)
            Next
            For i As Integer = 0 To UBound(RowSizes)
                .Row(i + 1).Height = RowSizes(i)
            Next
            .PageSetup.PrintAreas.Clear()
            .PageSetup.PrintAreas.Add(_gv.SetPrintAreaString)
            .Style.Font.FontName = _gv.SetCellFont(False)
        End With
        ExlWorkSheet.Style.NumberFormat.SetFormat("@")
        _gv.SetData()
        If ExlWorkbook.Worksheets.Count = 0 Then ExlWorkbook.AddWorksheet(ExlWorkSheet)
        ExlWorkbook.SaveAs(My.Resources.SAVEPATH)
        ExcelOpen()
    End Sub
    ''' <summary>
    ''' 墓地札データリスト出力
    ''' </summary>
    ''' <param name="_vob"></param>
    ''' <param name="outputPositon"></param>
    Private Sub GravePanelListOutputProcessing(_vob As IGravePanelListBehavior, outputPositon As Integer, isIPAmjMintyo As Boolean)

        Dim gpl As GravePanelDataListEntity = GravePanelDataListEntity.GetInstance
        gpb = _vob

        SheetSetting()

        With ExlWorkSheet
            SettingNewSheet_Vertical(gpb)
            .PageSetup.Margins.Bottom = 2
            StartIndex = 0
            Dim unused = .Cells.Clear()

            StartRowPosition = 0
            Do Until StartIndex = outputPositon - 1
                For i = 0 To UBound(RowSizes)
                    .Rows(StartRowPosition + i + 1).Height = RowSizes(i)
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
                    .Rows(StartRowPosition + I + 1).Height = RowSizes(I)
                Next

                gpb.CellsJoin(StartRowPosition)
                gpb.SetData(StartRowPosition, gp)
            Next

            .Style.Font.FontName = gpb.SetCellFont(isIPAmjMintyo)
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
                                        money As Integer, note1 As String, note2 As String, note3 As String, note4 As String,
                                        note5 As String, multioutput As Boolean, _isIPAmjMintyo As Boolean) Implements IOutputDataRepogitory.TransferPaperPrintOutput

        MyAddressee = New DestinationDataEntity(customerid, addressee, title, postalcode, address1, address2, money, note1, note2, note3, note4, note5)
        Dim tp As IVerticalOutputListBehavior = New TransferPaper(MyAddressee, _isIPAmjMintyo)
        ListOutputVerticalProcessing(tp, multioutput, _isIPAmjMintyo)

    End Sub

    Public Sub Cho3EnvelopeOutput(customerid As String, addressee As String, title As String, postalcode As String, address1 As String, address2 As String,
                                  multioutput As Boolean, _isIPAmjMintyo As Boolean) Implements IOutputDataRepogitory.Cho3EnvelopeOutput

        MyAddressee = New DestinationDataEntity(customerid, addressee, title, postalcode, address1, address2)
        Dim ce As IVerticalOutputListBehavior = New Cho3Envelope(MyAddressee, _isIPAmjMintyo)
        ListOutputVerticalProcessing(ce, multioutput, _isIPAmjMintyo)

    End Sub

    Public Sub WesternEnvelopeOutput(customerid As String, addressee As String, title As String, postalcode As String, address1 As String, address2 As String, multioutput As Boolean, _isIPAmjMintyo As Boolean) Implements IOutputDataRepogitory.WesternEnvelopeOutput

        MyAddressee = New DestinationDataEntity(customerid, addressee, title, postalcode, address1, address2)
        Dim we As IVerticalOutputListBehavior = New WesternEnvelope(MyAddressee, _isIPAmjMintyo)
        ListOutputVerticalProcessing(we, multioutput, _isIPAmjMintyo)

    End Sub

    Public Sub Kaku2EnvelopeOutput(customerid As String, addressee As String, title As String, postalcode As String, address1 As String, address2 As String, multioutput As Boolean, _isIPAmjMintyo As Boolean) Implements IOutputDataRepogitory.Kaku2EnvelopeOutput

        MyAddressee = New DestinationDataEntity(customerid, addressee, title, postalcode, address1, address2)
        Dim ke As IVerticalOutputListBehavior = New Kaku2Envelope(MyAddressee, _isIPAmjMintyo)
        ListOutputVerticalProcessing(ke, multioutput, _isIPAmjMintyo)

    End Sub

    Public Sub GravePamphletOutput(customerid As String, addressee As String, title As String, postalcode As String, address1 As String, address2 As String, multioutput As Boolean, _isIPAmjMintyo As Boolean) Implements IOutputDataRepogitory.GravePamphletOutput

        MyAddressee = New DestinationDataEntity(customerid, addressee, title, postalcode, address1, address2)
        Dim gp As IVerticalOutputListBehavior = New GravePamphletEnvelope(MyAddressee, _isIPAmjMintyo)
        ListOutputVerticalProcessing(gp, multioutput, _isIPAmjMintyo)

    End Sub

    Public Sub PostcardOutput(customerid As String, addressee As String, title As String, postalcode As String, address1 As String, address2 As String, multioutput As Boolean, _isIPAmjMintyo As Boolean) Implements IOutputDataRepogitory.PostcardOutput
        MyAddressee = New DestinationDataEntity(customerid, addressee, title, postalcode, address1, address2)
        Dim pc As IVerticalOutputListBehavior = New Postcard(MyAddressee, _isIPAmjMintyo)
        ListOutputVerticalProcessing(pc, multioutput, _isIPAmjMintyo)
    End Sub

    Public Sub GravePanelOutput(outputPosition As Integer, _isIPAmjMintyo As Boolean) Implements IOutputDataRepogitory.GravePanelOutput
        Dim gp As IGravePanelListBehavior = New GravePanel()
        GravePanelListOutputProcessing(gp, outputPosition, _isIPAmjMintyo)
    End Sub

    Public Sub Cho3EnvelopeOutput(list As ObservableCollection(Of DestinationDataEntity), _isIPAmjMintyo As Boolean) Implements IOutputDataRepogitory.Cho3EnvelopeOutput
        Dim ce As IVerticalOutputListBehavior = New Cho3Envelope(list)
        ListOutputVerticalProcessing(ce, True, _isIPAmjMintyo)
    End Sub

    Public Sub WesternEnvelopeOutput(list As ObservableCollection(Of DestinationDataEntity), _isIPAmjMintyo As Boolean) Implements IOutputDataRepogitory.WesternEnvelopeOutput
        Dim we As IVerticalOutputListBehavior = New WesternEnvelope(list)
        ListOutputVerticalProcessing(we, True, _isIPAmjMintyo)
    End Sub

    Public Sub Kaku2EnvelopeOutput(list As ObservableCollection(Of DestinationDataEntity), _isIPAmjMintyo As Boolean) Implements IOutputDataRepogitory.Kaku2EnvelopeOutput
        Dim ke As IVerticalOutputListBehavior = New Kaku2Envelope(list)
        ListOutputVerticalProcessing(ke, True, _isIPAmjMintyo)
    End Sub

    Public Sub GravePamphletOutput(list As ObservableCollection(Of DestinationDataEntity), _isIPAmjMintyo As Boolean) Implements IOutputDataRepogitory.GravePamphletOutput
        Dim gp As IVerticalOutputListBehavior = New GravePamphletEnvelope(list)
        ListOutputVerticalProcessing(gp, True, _isIPAmjMintyo)
    End Sub

    Public Sub PostcardOutput(list As ObservableCollection(Of DestinationDataEntity), _isIPAmjMintyo As Boolean) Implements IOutputDataRepogitory.PostcardOutput
        Dim pc As IVerticalOutputListBehavior = New Postcard(list)
        ListOutputVerticalProcessing(pc, True, _isIPAmjMintyo)
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

    Public Sub LabelOutput(list As ObservableCollection(Of DestinationDataEntity), _isIPAmjMintyo As Boolean) Implements IOutputDataRepogitory.LabelOutput
        Dim ls As IHorizontalOutputBehavior = New LabelSheet(list, _isIPAmjMintyo)
        OutputLabelProcessing(ls, _isIPAmjMintyo)
    End Sub

    Public Sub VoucherOutput(id As Integer, addressee As String, provisoList As ObservableCollection(Of Proviso), isShunjuen As Boolean, isReissue As Boolean, cleakName As String, isDisplayTax As Boolean, prepaidDate As Date, accountActivityDate As Date, IsIPAmjMintyo As Boolean) Implements IOutputDataRepogitory.VoucherOutput
        Dim v As IVerticalOutputBehavior = New Voucher(id, addressee, provisoList, isShunjuen, isReissue, cleakName, isDisplayTax, prepaidDate, accountActivityDate, IsIPAmjMintyo)
        VoucherOutputProcessing(v)
    End Sub

    Public Sub GraveVoucherOutput(addressee As String, amount As Integer, accountActivityDate As Date, lessee As LesseeCustomerInfoEntity, graveNote As String, frontage As Double, depth As Double, isEarnest As Boolean, isDeposit As Boolean, isRemainingMoney As Boolean, isFullAmount As Boolean, note As String, cleakName As String, isIPAmjMintyo As Boolean) Implements IOutputDataRepogitory.GraveVoucherOutput
        Dim v As IVerticalOutputBehavior = New GraveVoucher(addressee, amount, accountActivityDate, lessee, graveNote, frontage, depth, isEarnest, isDeposit, isRemainingMoney, isFullAmount, note, cleakName, isIPAmjMintyo)
        GraveVoucherOutputProcessing(v)
    End Sub

    Public Function ReturnXlsxFilePath() As String Implements IOutputDataRepogitory.ReturnXlsxFilePath
        Return My.Resources.SAVEPATH
    End Function

    ''' <summary>
    ''' 墓地管理料受納証クラス
    ''' </summary>
    Private Class GraveVoucher
        Implements IVerticalOutputBehavior
        ''' <summary>
        ''' 宛名
        ''' </summary>
        Private ReadOnly Addressee As String
        ''' <summary>
        ''' 金額
        ''' </summary>
        Private ReadOnly Amount As Integer
        ''' <summary>
        ''' 名義人情報
        ''' </summary>
        Private ReadOnly MyLessee As LesseeCustomerInfoEntity
        ''' <summary>
        ''' 使用地備考
        ''' </summary>
        Private ReadOnly GraveNote As String
        ''' <summary>
        ''' 間口
        ''' </summary>
        Private ReadOnly Frontage As Double
        ''' <summary>
        ''' 奥行
        ''' </summary>
        Private ReadOnly Depth As Double
        ''' <summary>
        ''' 手付金チェック
        ''' </summary>
        Private ReadOnly IsEarnest As Boolean
        ''' <summary>
        ''' 内金チェック
        ''' </summary>
        Private ReadOnly IsDeposit As Boolean
        ''' <summary>
        ''' 残金チェック
        ''' </summary>
        Private ReadOnly IsRemainingMoney As Boolean
        ''' <summary>
        ''' 全額チェック
        ''' </summary>
        Private ReadOnly IsFullAmount As Boolean
        ''' <summary>
        ''' 備考
        ''' </summary>
        Private ReadOnly Note As String
        ''' <summary>
        ''' 扱者
        ''' </summary>
        Private ReadOnly CleakName As String
        ''' <summary>
        ''' 入金日
        ''' </summary>
        Private ReadOnly AccountActivityDate As Date

        Private ReadOnly IsIPAmjMintyo As Boolean

        ''' <param name="addressee">宛名</param>
        ''' <param name="amount">金額</param>
        ''' <param name="accountActivityDate">入金日</param>
        ''' <param name="myLessee">名義人情報</param>
        ''' <param name="graveNote">墓地備考</param>
        ''' <param name="frontage">間口</param>
        ''' <param name="depth">奥行</param>
        ''' <param name="isEarnest">手付金チェック</param>
        ''' <param name="isDeposit">内金チェック</param>
        ''' <param name="isRemainingMoney">残金チェック</param>
        ''' <param name="isFullAmount">全額チェック</param>
        ''' <param name="note">備考</param>
        ''' <param name="cleakName">扱者</param>
        Public Sub New(addressee As String, amount As Integer, accountActivityDate As Date, myLessee As LesseeCustomerInfoEntity, graveNote As String, frontage As Double, depth As Double, isEarnest As Boolean, isDeposit As Boolean, isRemainingMoney As Boolean, isFullAmount As Boolean, note As String, cleakName As String, isIPAmjMintyo As Boolean)
            Me.Addressee = addressee
            Me.Amount = amount
            Me.AccountActivityDate = accountActivityDate
            Me.MyLessee = myLessee
            Me.GraveNote = graveNote
            Me.Frontage = frontage
            Me.Depth = depth
            Me.IsEarnest = isEarnest
            Me.IsDeposit = isDeposit
            Me.IsRemainingMoney = isRemainingMoney
            Me.IsFullAmount = isFullAmount
            Me.Note = note
            Me.CleakName = cleakName
            Me.IsIPAmjMintyo = isIPAmjMintyo
        End Sub

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin
            With ExlWorkSheet
                Dim unused = .Range(.Cell(2, 3), .Cell(2, 11)).Merge
                unused = .Range(.Cell(4, 3), .Cell(4, 11)).Merge
                unused = .Range(.Cell(6, 3), .Cell(6, 4)).Merge
                unused = .Range(.Cell(6, 6), .Cell(6, 7)).Merge
                unused = .Range(.Cell(6, 9), .Cell(6, 10)).Merge
                unused = .Range(.Cell(6, 12), .Cell(6, 13)).Merge
                unused = .Range(.Cell(7, 4), .Cell(7, 13)).Merge
                unused = .Range(.Cell(9, 5), .Cell(9, 6)).Merge
                unused = .Range(.Cell(9, 12), .Cell(9, 13)).Merge
                unused = .Range(.Cell(10, 5), .Cell(10, 6)).Merge
                unused = .Range(.Cell(14, 2), .Cell(14, 14)).Merge
                unused = .Range(.Cell(15, 5), .Cell(15, 6)).Merge
                unused = .Range(.Cell(15, 7), .Cell(15, 8)).Merge
                unused = .Range(.Cell(15, 9), .Cell(15, 10)).Merge
                unused = .Range(.Cell(17, 14), .Cell(17, 15)).Merge
            End With
        End Sub

        Public Sub CellProperty(startrowposition As Integer) Implements IExcelOutputBehavior.CellProperty
            With ExlWorkSheet
                With .PageSetup
                    .SetPaperSize(ClosedXML.Excel.XLPaperSize.B5Paper)
                    .PageOrientation = XLPageOrientation.Portrait
                End With
                With .Cell(2, 3).Style
                    With .Alignment
                        .SetVertical(XLAlignmentVerticalValues.Center)
                        .SetHorizontal(XLAlignmentHorizontalValues.Center)
                    End With
                    With .Font
                        .SetBold(True)
                        .SetFontSize(24)
                    End With
                End With
                With .Cell(4, 3).Style
                    With .Alignment
                        .SetVertical(XLAlignmentVerticalValues.Center)
                        .SetHorizontal(XLAlignmentHorizontalValues.Center)
                    End With
                    With .Font
                        .SetBold(True)
                        .SetFontSize(24)
                    End With
                End With
                With .Range(.Cell(6, 3), .Cell(6, 12)).Style.Alignment
                    .SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .SetVertical(XLAlignmentVerticalValues.Center)
                End With
                With .Cell(7, 4).Style.Alignment
                    .SetVertical(XLAlignmentVerticalValues.Center)
                    .SetHorizontal(XLAlignmentHorizontalValues.Left)
                    .SetShrinkToFit(True)
                End With
                With .Range(.Cell(9, 5), .Cell(10, 5)).Style.Alignment
                    .SetVertical(XLAlignmentVerticalValues.Center)
                    .SetHorizontal(XLAlignmentHorizontalValues.Right)
                End With
                With .Range("12:12").Style
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    .Font.SetFontSize(16)
                End With
                With .Cell(14, 2).Style
                    .Alignment.SetShrinkToFit(True)
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                End With
                With .Range(.Cell(15, 5), .Cell(15, 9)).Style.Alignment
                    .SetVertical(XLAlignmentVerticalValues.Center)
                    .SetHorizontal(XLAlignmentHorizontalValues.Center)
                End With
                With .Cell(17, 14).Style
                    .Alignment.SetShrinkToFit(True)
                    .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                End With
            End With
        End Sub

        Public Function CriteriaCellRowIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellRowIndex
            Return 2
        End Function

        Public Function CriteriaCellColumnIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellColumnIndex
            Return 3
        End Function

        Public Function SetCellFont(_isIPAmjMintyo As Boolean) As String Implements IExcelOutputBehavior.SetCellFont
            Return "ＭＳ 明朝"
        End Function

        Public Function SetColumnSizes() As Double() Implements IExcelOutputBehavior.SetColumnSizes
            Return {13.57, 8.57, 3.43, 3.43, 3.43, 2.86, 3.43, 2.86, 3.43, 2.86, 3.43, 3.29, 3.29, 3.29, 3.29, 3.71}
        End Function

        Public Function SetRowSizes() As Double() Implements IExcelOutputBehavior.SetRowSizes
            Return {217.5, 28.5, 31.5, 28.5, 33.75, 18.75, 18.75, 3, 18.75, 18.75, 12, 25.5, 54, 27, 18.75, 111, 24}
        End Function

        Public Function GetDataName() As String Implements IExcelOutputBehavior.GetDataName
            Return ToString()
        End Function

        Public Function SetPrintAreaString() As String Implements IExcelOutputBehavior.SetPrintAreaString
            Return "a:p"
        End Function

        Public Sub SetData()
            With ExlWorkSheet
                .Cell(2, 3).Value = Addressee
                If IsIPAmjMintyo Then .Cell(2, 3).Style.Font.FontName = My.Resources.IPAmjMintyoString
                .Cell(4, 3).Value = $"{Amount:N0}"
                .Cell(6, 3).Value = $"{MyLessee.GetGraveNumber().ConvertKuString}{MyLessee.GetGraveNumber().ConvertKuikiString}"
                .Cell(6, 6).Value = IIf(MyLessee.GetGraveNumber().GawaField.DisplayForField = 0, String.Empty, MyLessee.GetGraveNumber().GawaField.DisplayForField)
                .Cell(6, 9).Value = MyLessee.GetGraveNumber().BanField.DisplayForField
                .Cell(6, 12).Value = MyLessee.GetGraveNumber().EdabanField.DisplayForField
                .Cell(7, 4).Value = GraveNote
                .Cell(9, 5).Value = IIf(Frontage = 0, String.Empty, DoubleConvert(Frontage))
                .Cell(9, 12).Value = IIf(Depth = 0, String.Empty, DoubleConvert(Depth))
                .Cell(10, 5).Value = IIf(MyLessee.GetArea.AreaValue = 0, String.Empty, DoubleConvert(MyLessee.GetArea.AreaValue))
                .Cell(12, 5).Value = IIf(IsEarnest, "〇", String.Empty)
                .Cell(12, 7).Value = IIf(IsDeposit, "〇", String.Empty)
                .Cell(12, 9).Value = IIf(IsRemainingMoney, "〇", String.Empty)
                .Cell(12, 11).Value = IIf(IsFullAmount, "〇", String.Empty)
                .Cell(14, 2).Value = Note
                Dim I As Integer = AccountActivityDate.ToString("yy", JapanCulture)
                .Cell(15, 5).Value = I
                .Cell(15, 7).Value = AccountActivityDate.Month
                .Cell(15, 9).Value = AccountActivityDate.Day
                .Cell(17, 14).Value = CleakName
            End With
        End Sub
        Private Function DoubleConvert(d As Double) As String
            Return IIf($"{d:0.00}".Substring($"{d:0.00}".Length - 1) = 0, $"{d:0.0}", $"{d:0.00}")
        End Function
        Private ReadOnly Property JapanCulture As CultureInfo
            Get
                Dim value As New CultureInfo("ja-JP", True)
                value.DateTimeFormat.Calendar = New JapaneseCalendar
                Return value
            End Get
        End Property


    End Class
    ''' <summary>
    ''' 受納証クラス
    ''' </summary>
    Private Class Voucher
        Implements IVerticalOutputBehavior

        Private ReadOnly addressee As String
        Private ReadOnly provisoList As ObservableCollection(Of Proviso)
        Private ReadOnly isShunjuen As Boolean
        Private ReadOnly isReissue As Boolean
        Private ReadOnly cleakName As String
        Private horizontal As XLAlignmentHorizontalValues
        Private vertical As XLAlignmentVerticalValues
        Private fontSize As Integer
        Private ReadOnly iD As Integer
        Private ReadOnly isDisplayTax As Boolean
        Private ReadOnly prepaidDate As Date
        Private ReadOnly accountActivityDate As Date
        Private ReadOnly isIPAmjMintyo As Boolean

        Public Sub New(id As String, addressee As String, provisoList As ObservableCollection(Of Proviso), isShunjuen As Boolean, isReissue As Boolean, cleakName As String, isDisplayTax As Boolean, prepaidDate As Date, accountActivityDate As Date, isIPAmjMintyo As Boolean)
            Me.iD = id
            Me.addressee = addressee
            Me.provisoList = provisoList
            Me.isShunjuen = isShunjuen
            Me.isReissue = isReissue
            Me.cleakName = cleakName
            Me.isDisplayTax = isDisplayTax
            Me.prepaidDate = prepaidDate
            Me.accountActivityDate = accountActivityDate
            Me.isIPAmjMintyo = isIPAmjMintyo
        End Sub

        Private Function CopyColumnPosition(originalColumn As Integer) As Integer
            Dim i As Integer = originalColumn + Math.Floor(SetColumnSizes().Length / 2) + 1
            Return i
        End Function
        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin
            ''タイトル
            SetMergeOriginalAndCopy(1, 1, 1, 7)
            SetMergeOriginalAndCopy(1, 8, 1, 10)
            SetMergeOriginalAndCopy(2, 2, 2, 5)
            ''日付
            SetMergeOriginalAndCopy(2, 7, 2, 10)
            ''T番号
            SetMergeOriginalAndCopy(3, 7, 3, 10)
            ''宛名
            SetMergeOriginalAndCopy(4, 1, 4, 4)
            ''総額
            SetMergeOriginalAndCopy(6, 3, 6, 7)
            ''円也
            SetMergeOriginalAndCopy(6, 8, 6, 9)
            ''事前領収日
            SetMergeOriginalAndCopy(7, 7, 7, 9)
            ''軽減税率対象です
            SetMergeOriginalAndCopy(8, 7, 8, 9)
            ''但し書き　2行と1行の表記形式の対応
            If (provisoList.Count > 4) Then
                SetMergeOriginalAndCopy(9, 2, 9, 5)
                SetMergeOriginalAndCopy(10, 2, 10, 5)
                SetMergeOriginalAndCopy(11, 2, 11, 5)
                SetMergeOriginalAndCopy(12, 2, 12, 5)
                SetMergeOriginalAndCopy(9, 6, 9, 9)
                SetMergeOriginalAndCopy(10, 6, 10, 9)
                SetMergeOriginalAndCopy(11, 6, 11, 9)
                SetMergeOriginalAndCopy(12, 6, 12, 9)
            Else
                SetMergeOriginalAndCopy(9, 2, 9, 9)
                SetMergeOriginalAndCopy(10, 2, 10, 9)
                SetMergeOriginalAndCopy(11, 2, 11, 9)
                SetMergeOriginalAndCopy(12, 2, 12, 9)
            End If
            ''春秋苑の受納証と信行寺の受納証で上記有難くおうけいたしましたの文字列の位置を変える
            If isDisplayTax Then
                SetMergeOriginalAndCopy(13, 1, 13, 2)
                SetMergeOriginalAndCopy(13, 3, 13, 5)
                SetMergeOriginalAndCopy(13, 6, 13, 7)
                SetMergeOriginalAndCopy(13, 8, 13, 10)
                SetMergeOriginalAndCopy(14, 1, 14, 2)
                SetMergeOriginalAndCopy(14, 3, 14, 5)
                SetMergeOriginalAndCopy(14, 6, 14, 7)
                SetMergeOriginalAndCopy(14, 8, 14, 10)
                SetMergeOriginalAndCopy(15, 2, 15, 9)
            Else
                SetMergeOriginalAndCopy(13, 2, 14, 9)
            End If
            ''団体名
            SetMergeOriginalAndCopy(16, 5, 17, 7)
            ''団体肩書
            SetMergeOriginalAndCopy(17, 1, 17, 4)
            ''係
            SetMergeOriginalAndCopy(17, 9, 17, 10)
            ''郵便番号
            SetMergeOriginalAndCopy(18, 1, 18, 2)
            ''住所
            SetMergeOriginalAndCopy(18, 3, 18, 8)
            ''係印
            SetMergeOriginalAndCopy(18, 9, 19, 10)
            ''電話番号
            SetMergeOriginalAndCopy(19, 2, 19, 7)
        End Sub

        Private Sub SetMergeOriginalAndCopy(row1 As Integer, column1 As Integer, row2 As Integer, column2 As Integer)
            ''両側のセルを結合する
            Dim unused = MySheetCellRange(row1, column1, row2, column2).Merge()
            unused = MySheetCellRange(row1, CopyColumnPosition(column1), row2, CopyColumnPosition(column2)).Merge()
        End Sub

        Private Function ToInch(m As Double) As Double
            Return m * 0.3937
        End Function

        Public Sub CellProperty(startrowposition As Integer) Implements IExcelOutputBehavior.CellProperty
            With ExlWorkSheet.PageSetup
                .SetPaperSize(ClosedXML.Excel.XLPaperSize.B5Paper)
                .PageOrientation = XLPageOrientation.Landscape
            End With
            ''タイトル欄
            SetLocalProperty(XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, 26)
            SetCellPropertyOriginalAndCopy(1, 1)
            ''ナンバー
            SetLocalProperty(XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Bottom, 11)
            SetCellPropertyOriginalAndCopy(1, 8)
            ''日付欄
            SetLocalProperty(XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, 11)
            SetCellPropertyOriginalAndCopy(2, 7)
            ''Tナンバー欄
            SetLocalProperty(XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, 11)
            SetCellPropertyOriginalAndCopy(3, 7)
            ''宛名欄
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 18)
            SetRangeProperyOriginalAndCopy(4, 1, 4, 4)
            Dim unused = ExlWorkSheet.Cell(4, 1).Style.Alignment.SetShrinkToFit(True)
            unused = ExlWorkSheet.Cell(4, CopyColumnPosition(1)).Style.Alignment.SetShrinkToFit(True)
            SetLocalProperty(XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Bottom, 18)
            SetCellPropertyOriginalAndCopy(4, 5)
            ''冥加金文字列
            SetLocalProperty(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Bottom, 11)
            SetCellPropertyOriginalAndCopy(5, 2)
            ''総額欄
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 24)
            SetCellPropertyOriginalAndCopy(6, 3)
            ''円也文字列
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 11)
            SetCellPropertyOriginalAndCopy(6, 8)
            ''但し文字列
            SetLocalProperty(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Bottom, 11)
            SetCellPropertyOriginalAndCopy(7, 2)
            ''軽減税率対象です
            unused = ExlWorkSheet.Cell(8, 7).Style.Alignment.SetShrinkToFit(True)
            unused = ExlWorkSheet.Cell(8, CopyColumnPosition(7)).Style.Alignment.SetShrinkToFit(True)
            ''但し書き欄
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, 14)
            SetRangeProperyOriginalAndCopy(9, 2, 12, 9)
            unused = MySheetCellRange(9, 2, 12, 9).Style.Alignment.SetShrinkToFit(True)
            unused = MySheetCellRange(9, CopyColumnPosition(2), 12, CopyColumnPosition(9)).Style _
            .Alignment.SetShrinkToFit(True)
            ''税率欄
            SetLocalProperty(XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, 11)
            unused = MySheetCellRange(13, 1, 14, 10).Style.Alignment.SetShrinkToFit(True)
            unused = MySheetCellRange(13, CopyColumnPosition(1), 14, CopyColumnPosition(10)).Style _
              .Alignment.SetShrinkToFit(True)
            SetRangeProperyOriginalAndCopy(13, 1, 14, 10)
            ''上記有難くお受けしました文字列
            If isDisplayTax Then
                SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, 11)
                SetCellPropertyOriginalAndCopy(15, 2)
            Else
                SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, 11)
                SetCellPropertyOriginalAndCopy(13, 2)
            End If
            ''宗派、法人名文字列
            SetLocalProperty(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Bottom, 10)
            SetCellPropertyOriginalAndCopy(17, 1)
            ''団体名文字列
            SetLocalProperty(XLAlignmentHorizontalValues.Distributed, XLAlignmentVerticalValues.Bottom, 18)
            SetCellPropertyOriginalAndCopy(16, 5)
            ''係文字列、係印欄
            SetLocalProperty(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, 11)
            SetRangeProperyOriginalAndCopy(17, 9, 19, 10)
            ''郵便番号欄
            SetLocalProperty(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, 10)
            SetCellPropertyOriginalAndCopy(18, 1)
            ''住所欄
            SetLocalProperty(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, 10)
            SetCellPropertyOriginalAndCopy(18, 3)
            ''電話番号欄
            SetLocalProperty(XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, 10)
            SetCellPropertyOriginalAndCopy(19, 2)
        End Sub
        Private Sub SetRangeAlignmentAndFontSize(horizontal As XLAlignmentHorizontalValues, vertical As XLAlignmentVerticalValues, fontSize As Integer, row1 As Integer, column1 As Integer, row2 As Integer, column2 As Integer)
            ''セルレンジに書式設定を反映します
            Dim unused = MySheetCellRange(row1, column1, row2, column2).Style.Alignment.SetHorizontal(horizontal).Alignment.SetVertical(vertical).Font.SetFontSize(fontSize)
        End Sub
        Private Sub SetRangeProperyOriginalAndCopy(row1 As Integer, column1 As Integer, row2 As Integer, column2 As Integer)
            ''両側のセルレンジに書式設定を反映します
            SetRangeAlignmentAndFontSize(horizontal, vertical, fontSize, row1, column1, row2, column2)
            SetRangeAlignmentAndFontSize(horizontal, vertical, fontSize, row1, CopyColumnPosition(column1), row2,
                                                                    CopyColumnPosition(column2))
        End Sub
        Private Sub SetCellPropertyOriginalAndCopy(row As Integer, column As Integer)
            ''両側のセルに書式設定を反映します
            SetAlignmentAndFontSize(row, column)
            SetAlignmentAndFontSize(row, CopyColumnPosition(column))
        End Sub

        Private Sub SetAlignmentAndFontSize(row As Integer, column As Integer)
            Dim unused = ExlWorkSheet.Cell(row, column).Style.Alignment.SetHorizontal(horizontal).Alignment.SetVertical(vertical).Font.SetFontSize(fontSize)
        End Sub

        Private Sub SetLocalProperty(horizontalValues As XLAlignmentHorizontalValues,
                                                 verticalValues As XLAlignmentVerticalValues, ByRef size As Integer)
            horizontal = horizontalValues
            vertical = verticalValues
            fontSize = size
        End Sub
        Public Function CriteriaCellRowIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellRowIndex
            Return 1
        End Function

        Public Function CriteriaCellColumnIndex() As Integer Implements IVerticalOutputBehavior.CriteriaCellColumnIndex
            Return 1
        End Function

        Public Function SetCellFont(_isIPAmjMintyo As Boolean) As String Implements IExcelOutputBehavior.SetCellFont
            Return "ＭＳ 明朝"
        End Function

        Public Function SetColumnSizes() As Double() Implements IExcelOutputBehavior.SetColumnSizes
            Return {3.13, 6.88, 3.13, 1.38, 6.88, 4, 7.86, 3.13, 3.13, 3.5, 14.86, 3.13, 6.88, 3.13, 1.38, 6.88, 4, 7.86, 3.13, 3.13, 3.5}
        End Function

        Public Function SetRowSizes() As Double() Implements IExcelOutputBehavior.SetRowSizes
            Return {37.5, 18, 18, 25.5, 37.5, 37.5, 18.75, 18.75, 22.5, 22.5, 22.5, 22.5, 15, 15, 21.75, 18, 18.5, 18.5, 18, 18}

        End Function

        Public Function GetDataName() As String Implements IExcelOutputBehavior.GetDataName
            Return ToString()
        End Function

        Public Function SetPrintAreaString() As String Implements IExcelOutputBehavior.SetPrintAreaString
            Return "a:u"
        End Function

        Protected Function MySheetCellRange(cell1Row As Integer, cell1Column As Integer, cell2Row As Integer, cell2Column As Integer) As IXLRange
            Return ExlWorkSheet.Range(ExlWorkSheet.Cell(cell1Row, cell1Column), ExlWorkSheet.Cell(cell2Row, cell2Column))
        End Function

        Public Sub SetData()

            Dim prevText As String = String.Empty ''前の但し書き内容
            Dim addresseeText As String ''宛名
            Dim provisoAmount As Integer = 0 ''但し書きの金額
            Dim contentCount As Integer = 0 ''但し書きの件数

            ''タイトル
            SetStringOriginalAndCopy(1, 1, "受　納　証")
            If isReissue Then SetStringOriginalAndCopy(2, 2, "※再発行")
            ''ナンバー
            SetStringOriginalAndCopy(1, 8, $"№{iD}")
            ''日付
            SetStringOriginalAndCopy(2, 7, $"{accountActivityDate.Year}年{accountActivityDate.Month}月{accountActivityDate.Day}日")
            ''T番号
            If isDisplayTax Then SetStringOriginalAndCopy(3, 7, My.Resources.InVoiceRegistrationNumber)
            ''宛名　2文字ならスペースを入れる
            If addressee.Length <> 0 Then
                addresseeText = IIf(addressee.Length = 2, $"{addressee.Substring(0, 1)}　{addressee.Substring(1, 1)}", addressee)
            Else
                addresseeText = String.Empty
            End If
            SetStringOriginalAndCopy(4, 1, addresseeText)
            If isIPAmjMintyo Then
                With ExlWorkSheet
                    .Cell(4, 1).Style.Font.FontName = My.Resources.IPAmjMintyoString
                    .Cell(4, CopyColumnPosition(1))
                End With
            End If
            SetStringOriginalAndCopy(4, 5, "様")
            ''総額
            SetStringOriginalAndCopy(5, 2, "冥加金")
            Dim i As Integer = 0
            For Each p As Proviso In provisoList
                i += p.Amount
            Next
            SetStringOriginalAndCopy(6, 3, $"{i:N0} -")
            SetStringOriginalAndCopy(6, 8, "円也")
            SetStringOriginalAndCopy(8, 2, "但し")
            ''但し書き
            ''受納証データが保持している出納データから同一のContentを1件とした件数を算出する
            ''税率ごとの総額も加算していく
            Dim containTaxRateAmount, taxRate, containReducedTaxRateAmount, reducedTaxRate As Integer
            Dim b As Boolean
            Dim datePosition As Integer = 8

            For Each p As Proviso In provisoList
                ''但し書きが既に受納証に表記されていれば書き加えない
                If Not prevText = p.Text Then
                    prevText = p.Text
                    b = contentCount > 0
                    contentCount += 1
                End If
                If p.IsReducedTaxRate Then
                    containReducedTaxRateAmount += p.Amount
                    datePosition = 7
                Else
                    containTaxRateAmount += p.Amount
                End If
            Next
            'reducedTaxRate = Math.Floor(containReducedTaxRateAmount / 1.08 * 0.08)
            'taxRate = Math.Floor(containTaxRateAmount / 1.1 * 0.1)
            Dim s As Single = containTaxRateAmount / 1.1
            taxRate = Math.Floor(s * 0.1)
            s = containReducedTaxRateAmount / 1.08
            reducedTaxRate = Math.Floor(s * 0.08)
            prevText = String.Empty
            ''件数が4件以下なら真ん中に1列で、それ以上なら2列で表記する
            If contentCount < 5 Then
                SingleLineOutput(contentCount, prevText, provisoAmount, b)
            Else
                MultipleLineOutput(prevText, provisoAmount)
            End If
            ''団体名、電話番号、税詳細
            If (isDisplayTax) Then SetTaxRate(taxRate, reducedTaxRate, containTaxRateAmount, containReducedTaxRateAmount)
            SetName()
            ''郵便番号
            SetStringOriginalAndCopy(18, 1, "〒214-0036")
            ''住所
            SetStringOriginalAndCopy(18, 3, "川崎市多摩区南生田８－１－１")
            Dim imagePath As String = ".\files\ReceiptStamp.png"
            Dim rowPosition = IIf(isDisplayTax, 16, 15)
            Try
                Dim unused = ExlWorkSheet.AddPicture(imagePath).MoveTo(ExlWorkSheet.Cell(rowPosition, 7))
                unused = ExlWorkSheet.AddPicture(imagePath).MoveTo(ExlWorkSheet.Cell(rowPosition, 18))
            Catch ex As Exception
                Dim log As New LogFileInfrastructure
                log.Log(ILoggerRepogitory.LogInfo.ERR, ex.Message)
            End Try
            'Dim unused = ExlWorkSheet.AddPicture(imagePath).MoveTo(ExlWorkSheet.Cell(rowPosition, 7))
            'unused = ExlWorkSheet.AddPicture(imagePath).MoveTo(ExlWorkSheet.Cell(rowPosition, 18))
            ''係
            SetStringOriginalAndCopy(17, 9, "係")
            SetStringOriginalAndCopy(18, 9, cleakName)
            ''事前領収の日付
            If Not prepaidDate = "1900-01-01" Then SetStringOriginalAndCopy(datePosition, 7, $"{prepaidDate:M/d}分")
            With ExlWorkSheet.PageSetup.Margins
                .Top = ToInch(1.9)
                .Bottom = ToInch(1.3)
                .Right = ToInch(1.3)
                .Left = ToInch(1.3)
            End With
        End Sub

        Private Sub SetName()
            If isDisplayTax Then
                SetStringOriginalAndCopy(15, 2, "上記有難くお受けいたしました")
            Else
                SetStringOriginalAndCopy(13, 2, "上記有難くお受けいたしました")
            End If

            If isShunjuen Then
                SetStringOriginalAndCopy(17, 1, "宗教法人信行寺")
                SetStringOriginalAndCopy(16, 5, "春秋苑")
            Else
                SetStringOriginalAndCopy(17, 1, "浄土真宗本願寺派")
                SetStringOriginalAndCopy(16, 5, "信行寺")
            End If
            SetStringOriginalAndCopy(19, 2, "電話０４４－９７７－３４６６㈹")
        End Sub
        Private Sub SetBottomBorderOriginalAndCopy(row1 As Integer, column1 As Integer, row2 As Integer, column2 As Integer)
            ''左右の同じ部分に下線を引きます
            SetBottomBorderThin(row1, column1, row2, column2)
            SetBottomBorderThin(row1, CopyColumnPosition(column1), row2, CopyColumnPosition(column2))
        End Sub

        Private Sub SetBottomBorderThin(row1 As Integer, column1 As Integer, row2 As Integer, column2 As Integer)
            Dim unused = MySheetCellRange(row1, column1, row2, column2).Style.
                    Border.SetBottomBorder(XLBorderStyleValues.Thin)
        End Sub

        Private Sub SetClerkMarkField(row1 As Integer, column1 As Integer, row2 As Integer, column2 As Integer)
            Dim unused = MySheetCellRange(row1, column1, row2, column2).Style _
            .Border.SetBottomBorder(XLBorderStyleValues.Thin) _
            .Border.SetRightBorder(XLBorderStyleValues.Thin) _
            .Border.SetLeftBorder(XLBorderStyleValues.Thin) _
            .Border.SetDiagonalBorder(XLBorderStyleValues.Thin) _
            .Border.SetTopBorder(XLBorderStyleValues.Thin)
            unused = MySheetCellRange(17, CopyColumnPosition(9), 19, CopyColumnPosition(10)).Style _
            .Border.SetBottomBorder(XLBorderStyleValues.Thin) _
            .Border.SetRightBorder(XLBorderStyleValues.Thin) _
            .Border.SetLeftBorder(XLBorderStyleValues.Thin) _
            .Border.SetDiagonalBorder(XLBorderStyleValues.Thin) _
            .Border.SetTopBorder(XLBorderStyleValues.Thin)

        End Sub
        Public Sub SetBorderStyle()
            ''ボーダーをすべて消去
            Dim unused = ExlWorkSheet.Style _
            .Border.SetLeftBorder(XLBorderStyleValues.None) _
            .Border.SetTopBorder(XLBorderStyleValues.None) _
            .Border.SetRightBorder(XLBorderStyleValues.None) _
            .Border.SetBottomBorder(XLBorderStyleValues.None)
            ''宛名欄
            SetBottomBorderOriginalAndCopy(4, 1, 4, 5)
            ''総額欄
            SetBottomBorderOriginalAndCopy(6, 2, 6, 9)
            ''但し書き欄
            SetBottomBorderOriginalAndCopy(12, 2, 12, 9)
            ''係印欄
            SetClerkMarkField(17, 9, 19, 10)
            SetClerkMarkField(17, CopyColumnPosition(9), 19, CopyColumnPosition(10))
        End Sub

        Private Sub SetTaxRate(taxRate As Integer, reducedTaxRate As Integer, containTaxRateAmount As Integer, containReducedTaxRateAmount As Integer)
            If Not taxRate = 0 Then
                SetStringOriginalAndCopy(13, 1, "10％対象")
                SetStringOriginalAndCopy(13, 3, $"{containTaxRateAmount:N0} 円")
                SetStringOriginalAndCopy(13, 6, $"消費税")
                SetStringOriginalAndCopy(13, 8, $"{taxRate:N0} 円")
            End If
            If Not reducedTaxRate = 0 Then
                SetStringOriginalAndCopy(14, 1, "8％対象")
                SetStringOriginalAndCopy(14, 3, $"{containReducedTaxRateAmount:N0} 円")
                SetStringOriginalAndCopy(14, 6, $"消費税")
                SetStringOriginalAndCopy(14, 8, $"{reducedTaxRate:N0} 円")
            End If
        End Sub
        Private Sub MultipleLineOutput(prevText As String, provisoAmount As Integer)
            Dim i As Integer = 8 ''但し書き左側のRowPositionを設定する変数
            Dim j As Integer = 4 ''但し書き右側のRowPositionを設定する変数
            Dim length As Integer = 0

            For Each p As Proviso In provisoList
                ''但し書きが前のデータと同じなら金額を加算する
                If prevText = p.Text Then
                    provisoAmount += p.Amount
                Else
                    ''違えば変数に代入
                    provisoAmount = p.Amount
                    prevText = p.Text
                    ''変数を減算する
                    j -= 1
                    i -= 1
                End If
                ''iが7～4なら左側に、それ以外は右側に表記する
                If i > 3 Then
                    SetStringOriginalAndCopy(12 - j, 2, $"{p.Text} {provisoAmount:N0}円{IIf(p.IsReducedTaxRate, " ※", String.Empty)}")
                Else
                    SetStringOriginalAndCopy(12 - i, 6, $"{p.Text} {provisoAmount:N0}円{IIf(p.IsReducedTaxRate, " ※", String.Empty)}")
                End If
                If p.IsReducedTaxRate Then SetStringOriginalAndCopy(8, 7, "※は軽減税率対象です")
                If length < $"{p.Text} {provisoAmount:N0}円{IIf(p.IsReducedTaxRate, " ※", String.Empty)}".Length Then
                    length = $"{p.Text} {provisoAmount:N0}円{IIf(p.IsReducedTaxRate, " ※", String.Empty)}".Length
                End If
            Next
            If length > 9 Then
                MySheetCellRange(9, 2, 12, 9).Style.Font.FontSize = 11
                MySheetCellRange(9, CopyColumnPosition(2), 12, CopyColumnPosition(9)).Style.Font.FontSize = 11
            End If
            If length > 11 Then
                MySheetCellRange(9, 2, 12, 9).Style.Font.FontSize = 8
                MySheetCellRange(9, CopyColumnPosition(2), 12, CopyColumnPosition(9)).Style.Font.FontSize = 8
            End If
            If length > 13 Then
                MySheetCellRange(9, 2, 12, 9).Style.Font.FontSize = 6
                MySheetCellRange(9, CopyColumnPosition(2), 12, CopyColumnPosition(9)).Style.Font.FontSize = 6
            End If
        End Sub
        Private Sub SingleLineOutput(contentCount As Integer, ByRef prevText As String, provisoAmount As Integer, isMulti As Boolean)
            Dim i As Integer = contentCount - 1
            Dim length As Integer = 0

            For Each p As Proviso In provisoList
                ''但し書きが前のデータと同じなら金額を加算する
                If prevText = p.Text Then
                    provisoAmount += p.Amount
                    ''但し書きのforeachの前の処理で入力した文字列を消す。iも加算する
                    i += 1
                    SetStringOriginalAndCopy(12 - i, 2, String.Empty)
                Else
                    ''違えば変数に代入
                    provisoAmount = p.Amount
                    prevText = p.Text
                End If
                ''但し書きを入力する
                SetStringOriginalAndCopy(12 - i, 2, $"{prevText}{IIf(isMulti, $"：{provisoAmount:N0} 円", String.Empty)}{IIf(p.IsReducedTaxRate, " ※", String.Empty)}")
                If p.IsReducedTaxRate Then SetStringOriginalAndCopy(8, 7, "※は軽減税率対象です")
                ''カウントを減算する（次を1行上に入力するようにする）
                i -= 1
                If length < $"{prevText}{IIf(isMulti, $"：{provisoAmount:N0} 円", String.Empty)}{IIf(p.IsReducedTaxRate, " ※", String.Empty)}".Length Then
                    length = $"{prevText}{IIf(isMulti, $"：{provisoAmount:N0} 円", String.Empty)}{IIf(p.IsReducedTaxRate, " ※", String.Empty)}".Length
                End If
            Next
            If length > 15 Then
                MySheetCellRange(9, 2, 12, 9).Style.Font.FontSize = 12
                MySheetCellRange(9, CopyColumnPosition(2), 12, CopyColumnPosition(9)).Style.Font.FontSize = 12
            End If
            If length > 17 Then
                MySheetCellRange(9, 2, 12, 9).Style.Font.FontSize = 11
                MySheetCellRange(9, CopyColumnPosition(2), 12, CopyColumnPosition(9)).Style.Font.FontSize = 11
            End If
            If length > 20 Then
                MySheetCellRange(9, 2, 12, 9).Style.Font.FontSize = 10
                MySheetCellRange(9, CopyColumnPosition(2), 12, CopyColumnPosition(9)).Style.Font.FontSize = 10
            End If
        End Sub


        Private Sub SetString(row As Integer, column As Integer, value As String)
            ExlWorkSheet.Cell(row, column).Value = value
        End Sub

        Private Sub SetStringOriginalAndCopy(row As Integer, column As Integer, value As String)
            SetString(row, column, value)
            SetString(row, CopyColumnPosition(column), value)
        End Sub
    End Class

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
        Private Function NameConvert(strName As String) As String

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

                Case Else
                    Exit Select
            End Select

            Return nameValue

        End Function

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin
            With ExlWorkSheet
                Dim unused = .Range(.Cell(startrowposition + 1, 1), .Cell(startrowposition + 1, 2)).Merge()
                unused = .Range(.Cell(startrowposition + 2, 1), .Cell(startrowposition + 2, 2)).Merge()
            End With
        End Sub

        Public Function SetCellFont(_IsIPAmjMintyo As Boolean) As String Implements IExcelOutputBehavior.SetCellFont
            Return If(_IsIPAmjMintyo, My.Resources.IPAmjMintyoString, My.Resources.HGRegularRegularScriptPRO)
        End Function

        Public Sub CellProperty(startrowposition As Integer) Implements IExcelOutputBehavior.CellProperty

            With ExlWorkSheet
                .PageSetup.PaperSize = XLPaperSize.A4Paper
                .Style.Font.FontName = SetCellFont(False)
                With .Cell(startrowposition + 1, 1).Style
                    .Font.FontSize = 65
                    .Font.Bold = True
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                End With
                With .Range(.Cell(startrowposition + 2, 1), .Cell(startrowposition + 3, 2)).Style
                    .Font.FontSize = 48
                    .Font.Bold = True
                    .Alignment.ShrinkToFit = True
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                End With
                With .Range(.Cell(startrowposition + 1, 1), .Cell(startrowposition + 3, 2)).Style.Border
                    Dim unused = .SetTopBorder(XLBorderStyleValues.Thick)
                    unused = .SetBottomBorder(XLBorderStyleValues.Thick)
                    unused = .SetLeftBorder(XLBorderStyleValues.Thick)
                    unused = .SetRightBorder(XLBorderStyleValues.Thick)
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
            Return {75, 67.5, 63.75, 25.5}
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
        Private ReadOnly IsIPAmjMintyo As Boolean = False

        Public Sub New(_addressee As DestinationDataEntity, _isIPAmjMintyo As Boolean)
            IsIPAmjMintyo = _isIPAmjMintyo
            AddresseeList = New ObservableCollection(Of DestinationDataEntity) From {_addressee}
        End Sub

        Public Sub New(_addresseelist As ObservableCollection(Of DestinationDataEntity))
            AddresseeList = _addresseelist
        End Sub

        Public Sub SetData(startrowposition As Integer, destinationdata As DestinationDataEntity) Implements IVerticalOutputListBehavior.SetData

            Dim addresseename As String

            With ExlWorkSheet
                '郵便番号
                For I As Integer = 1 To 8
                    If I = 4 Then Continue For
                    .Cell(startrowposition + 2, I + 2).Value = destinationdata.MyPostalCode.GetCode.Substring(I - 1, 1)
                Next

                '住所
                Dim addresstext1 As String = String.Empty
                Dim addresstext2 As String = String.Empty
                Dim ac As New AddressConvert(destinationdata.MyAddress1.Address, destinationdata.MyAddress2.Address)
                addresstext1 = ac.GetConvertAddress1
                addresstext2 = ac.GetConvertAddress2
                If addresstext1.Length + addresstext2.Length < 15 Then
                    .Cell(startrowposition + 4, 9).Value = $"{ac.GetConvertAddress1}{Space(1)}{ac.GetConvertAddress2}"
                    .Cell(startrowposition + 4, 7).Value = String.Empty
                Else
                    .Cell(startrowposition + 4, 9).Value = ac.GetConvertAddress1
                    .Cell(startrowposition + 4, 7).Value = ac.GetConvertAddress2
                End If

                If ac.GetConvertAddress2.Length > GetAddressMaxLength() Then
                    .Cell(startrowposition + 4, 7).Style.Fill.BackgroundColor = XLColor.Yellow
                Else
                    .Cell(startrowposition + 4, 7).Style.Fill.BackgroundColor = XLColor.NoColor
                End If

                '宛名
                addresseename = If(destinationdata.AddresseeName.GetName.Length > 5,
                    $"{Space(1)}{destinationdata.AddresseeName.GetName}{destinationdata.MyTitle.GetTitle}",
                    $"{Space(1)}{destinationdata.AddresseeName.GetName}{Space(1)}{destinationdata.MyTitle.GetTitle}")
                .Cell(startrowposition + 4, 2).Value = addresseename
            End With

        End Sub

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin

            With ExlWorkSheet
                '住所欄1行目
                Dim unused = .Range(.Cell(startrowposition + 4, 9), .Cell(startrowposition + 5, 10)).Merge()
                '住所欄2行目
                unused = .Range(.Cell(startrowposition + 4, 7), .Cell(startrowposition + 5, 8)).Merge()
                '宛名肩書
                unused = .Range(.Cell(startrowposition + 4, 4), .Cell(startrowposition + 5, 5)).Merge()
                '宛名欄
                unused = .Range(.Cell(startrowposition + 4, 2), .Cell(startrowposition + 5, 3)).Merge()
            End With

        End Sub

        Public Function SetCellFont(_IsIPAmjMintyo As Boolean) As String Implements IExcelOutputBehavior.SetCellFont
            Return If(_IsIPAmjMintyo, My.Resources.IPAmjMintyoString, My.Resources.FontName_HGPGyoushotai)
        End Function

        Private Function ColumnSizes() As Double() Implements IVerticalOutputBehavior.SetColumnSizes
            Return {21.43, 7.43, 2.71, 2.71, 2.71, 0.33, 2.71, 2.71, 2.71, 2.71}
        End Function

        Private Function SetRowSizes() As Double() Implements IVerticalOutputBehavior.SetRowSizes
            Return {101.25, 38.25, 14.25, 409.5, 133.5, 36}
        End Function

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                '宛名
                With .Cell(startrowposition + 4, 2).Style
                    .Font.FontSize = If(IsIPAmjMintyo, 45, 48)
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
                    With .Alignment
                        .Horizontal = XLAlignmentHorizontalValues.Right
                        .Vertical = XLAlignmentVerticalValues.Top
                        .TopToBottom = True
                    End With
                End With
                '郵便番号
                With .Range(.Cell(startrowposition + 2, 3), .Cell(startrowposition + 2, 10)).Style
                    .Font.FontSize = 16
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
                    With .Alignment
                        .Vertical = XLAlignmentVerticalValues.Top
                        .Horizontal = XLAlignmentHorizontalValues.Center
                        .TopToBottom = True
                    End With
                End With

                '住所
                With .Range(.Cell(startrowposition + 4, 4), .Cell(startrowposition + 4, 10)).Style
                    .Font.FontSize = If(IsIPAmjMintyo, 28, 30)
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
                    With .Alignment
                        .Horizontal = XLAlignmentHorizontalValues.Center
                        .Vertical = XLAlignmentVerticalValues.Center
                        .TopToBottom = True
                    End With
                End With
                .Cell(startrowposition + 4, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                .Cell(startrowposition + 4, 9).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top
                .Cell(startrowposition + 4, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top
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
        Private Const YourCopyAddressMaxLengh As Integer = 11
        Friend ReadOnly IsIPAmjMintyo As Boolean

        Public Sub New(_addressee As DestinationDataEntity, _isIPAmjMintyo As Boolean)
            IsIPAmjMintyo = _isIPAmjMintyo
            AddresseeList = New ObservableCollection(Of DestinationDataEntity) From {_addressee}
        End Sub

        Public Sub New(_addresseelist As ObservableCollection(Of DestinationDataEntity))
            AddresseeList = _addresseelist
        End Sub

        ''' <summary>
        ''' お客様控えの住所を分けて表示させるための文字列の配列を返します
        ''' </summary>
        ''' <param name="address1">住所1</param>
        ''' <param name="address2">住所2</param>
        ''' <returns></returns>
        Private Function SplitYourCopyAddress(address1 As String, address2 As String) As String()

            Dim line1, line2, line3, joinaddress As String

            '住所をつなげる
            joinaddress = $"{address1}{address2}"

            'つなげた住所の文字列が長ければ関数を呼び出し値を返す
            If joinaddress.Length > YourCopyAddressMaxLengh * 2 Then Return ReturnLongAddressArray(joinaddress)

            '住所1が長ければ2行に分ける
            If address1.Length < YourCopyAddressMaxLengh Then
                line1 = address1
                line2 = String.Empty
            Else
                line1 = address1.Substring(0, YourCopyAddressMaxLengh)
                line2 = address1.Substring(YourCopyAddressMaxLengh)
            End If

            '住所１の２行目と住所2を合わせたものが長ければ2行に分ける
            joinaddress = line2 & address2
            If joinaddress.Length < YourCopyAddressMaxLengh Then
                line2 &= address2
                line3 = String.Empty
            Else
                line2 = joinaddress.Substring(0, YourCopyAddressMaxLengh)
                line3 = joinaddress.Substring(YourCopyAddressMaxLengh)
            End If

            Return {line1, line2, line3}

        End Function

        ''' <summary>
        ''' 長い住所を区切ります。1行目を住所2の文字も使用して3行で表示させます。
        ''' </summary>
        ''' <param name="absolutenessaddress"></param>
        ''' <returns></returns>
        Private Function ReturnLongAddressArray(absolutenessaddress As String) As String()

            Dim line1, line2, line3 As String

            line1 = absolutenessaddress.Substring(0, YourCopyAddressMaxLengh)
            line2 = absolutenessaddress.Substring(YourCopyAddressMaxLengh, YourCopyAddressMaxLengh)
            line3 = absolutenessaddress.Substring(YourCopyAddressMaxLengh * 2)

            Return {line1, line2, line3}

        End Function

        Public Function SetCellFont(_IsIPAmjMintyo As Boolean) As String Implements IExcelOutputBehavior.SetCellFont
            Return If(_IsIPAmjMintyo, My.Resources.IPAmjMintyoString, My.Resources.FontName_MSPMintyo)
        End Function

        Private Function SetColumnSizes() As Double() Implements IVerticalOutputBehavior.SetColumnSizes
            Return {3.71, 25.14, 7.57, 1.71, 1.71, 1.71, 1.71, 1.71, 1.71, 1.71, 1.71, 7.29, 1.71, 1.71, 1.71, 1.71, 2.14, 1.71, 1.71, 1.71, 0.31}
        End Function

        Private Function SetRowSizes() As Double() Implements IVerticalOutputBehavior.SetRowSizes
            Return {283.5, 165, 19.5, 15, 18.75, 15, 15, 15, 15, 15, 15, 15, 15, 105.75}
        End Function

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                .PageSetup.PaperSize = XLPaperSize.B5Paper
                '宛名欄
                With .Cell(12, 2).Style.Font
                    .FontSize = If(IsIPAmjMintyo, 12, 14)
                    .FontName = SetCellFont(IsIPAmjMintyo)
                End With
                '金額欄
                With .Range(.Cell(startrowposition + 3, 4), .Cell(startrowposition + 3, 11)).Style
                    .Font.FontSize = 14
                    .Font.FontName = My.Resources.FontName_MSPMintyo
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                End With

                'お客様控え金額欄
                With .Range(.Cell(startrowposition + 9, 13), .Cell(startrowposition + 9, 20)).Style
                    .Font.FontSize = 14
                    .Font.FontName = My.Resources.FontName_MSPMintyo
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                End With

                '備考欄1〜5
                With .Range(.Cell(startrowposition + 6, 4), .Cell(startrowposition + 10, 4)).Style
                    .Font.FontSize = 9
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                    .Alignment.TopToBottom = False
                    .Alignment.ShrinkToFit = True
                End With
                '住所欄
                With .Range(.Cell(startrowposition + 7, 2), .Cell(startrowposition + 10, 2)).Style
                    .Font.FontSize = If(IsIPAmjMintyo, 7.5, 9)
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
                    .Alignment.TopToBottom = False
                End With

                'お客様控え住所欄
                With .Range(.Cell(startrowposition + 10, 13), .Cell(startrowposition + 13, 13)).Style
                    .Font.FontSize = If(IsIPAmjMintyo, 7.5, 9)
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
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
                    Dim unused = .Range(.Cell(startrowposition + row, 4), .Cell(startrowposition + row, 11)).Merge()
                End With
                row += 1
            Loop

            'お客様控え欄4行を結合
            row = 10
            Do Until row = 14
                With ExlWorkSheet
                    Dim unused = .Range(.Cell(startrowposition + row, 13), .Cell(startrowposition + row, 20)).Merge()
                End With
                row += 1
            Loop

        End Sub

        Public Sub SetData(startrowposition As Integer, destinationdata As DestinationDataEntity) Implements IVerticalOutputListBehavior.SetData

            With ExlWorkSheet
                '振込金額入力
                Dim ColumnIndex As Integer
                Dim moneystring As String = $"\{destinationdata.MoneyData.GetMoney}"
                Dim moneyField As String

                For ColumnIndex = 0 To 8
                    moneyField = If(moneystring.Length - 1 < ColumnIndex, String.Empty, moneystring.Substring((moneystring.Length - 1) - ColumnIndex, 1))
                    .Cell(startrowposition + 3, 11 - ColumnIndex).Value = moneyField
                    .Cell(startrowposition + 9, 20 - ColumnIndex).Value = moneyField    'お客様控え
                Next

                .Cell(startrowposition + 6, 4).Value = destinationdata.Note1Data.GetNote   '備考1
                .Cell(startrowposition + 7, 4).Value = destinationdata.Note2Data.GetNote   '備考2
                .Cell(startrowposition + 8, 4).Value = destinationdata.Note3Data.GetNote   '備考3
                .Cell(startrowposition + 9, 4).Value = destinationdata.Note4Data.GetNote  '備考4
                .Cell(startrowposition + 10, 4).Value = destinationdata.Note5Data.GetNote  '備考5
                .Cell(startrowposition + 7, 2).Value = $"〒{destinationdata.MyPostalCode.GetCode}"      '宛先郵便番号
                Dim ac As New AddressConvert(destinationdata.MyAddress1.Address, destinationdata.MyAddress2.Address)
                .Cell(startrowposition + 8, 2).Value = ac.GetConvertAddress1         '宛先住所1

                Dim sad2 As String = StrConv(destinationdata.MyAddress2.Address, vbWide)
                Dim stringlength = If(sad2.Length < 20, sad2.Length, 18)
                '宛先住所2　長い場合は2行で入力
                .Cell(startrowposition + 9, 2).Value = sad2.Substring(0, stringlength)
                If sad2.Length > stringlength Then .Cell(startrowposition + 10, 2).Value = sad2.Substring(stringlength)

                .Cell(startrowposition + 12, 2).Value = $"{destinationdata.AddresseeName.GetName}{Space(1)}{destinationdata.MyTitle.GetTitle}"  '宛先の宛名
                .Cell(startrowposition + 13, 13).Value = $"{destinationdata.AddresseeName.GetName}{Space(1)}{destinationdata.MyTitle.GetTitle}" 'お客様控えの名前

                'お客様控え住所　長い場合は3行、それでも収まらない場合は注意を促す
                Dim strings() As String = SplitYourCopyAddress(ac.GetConvertAddress1, sad2)
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
        Friend ReadOnly IsIPAmjMintyo As Boolean

        Public Sub New(_addressee As DestinationDataEntity, _isIPAmjMintyo As Boolean)
            IsIPAmjMintyo = _isIPAmjMintyo
            AddresseeList = New ObservableCollection(Of DestinationDataEntity) From {_addressee}
        End Sub

        Public Sub New(_addresseelist As ObservableCollection(Of DestinationDataEntity))
            AddresseeList = _addresseelist
        End Sub

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputListBehavior.CellProperty

            With ExlWorkSheet
                .PageSetup.PaperSize = XLPaperSize.C6Envelope
                '郵便番号
                With .Range(.Cell(startrowposition + 2, 3), .Cell(startrowposition + 2, 9)).Style
                    .Font.FontSize = 16
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
                    .Alignment.Vertical = XLAlignmentVerticalValues.Top
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                End With
                '住所
                With .Range(.Cell(startrowposition + 4, 6), .Cell(startrowposition + 4, 8)).Style
                    .Font.FontSize = 24
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Alignment.TopToBottom = True
                End With
                .Cell(startrowposition + 4, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top
                .Cell(startrowposition + 4, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                '宛名
                With .Cell(startrowposition + 4, 2).Style
                    .Font.FontSize = 36
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
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
                Dim unused = .Range(.Cell(startrowposition + 4, 2), .Cell(startrowposition + 4, 4)).Merge()
                unused = .Range(.Cell(startrowposition + 4, 6), .Cell(startrowposition + 4, 7)).Merge()
                unused = .Range(.Cell(startrowposition + 4, 8), .Cell(startrowposition + 4, 9)).Merge()
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

                .Cell(startrowposition + 4, 6).Style.Fill.BackgroundColor = If(ac.GetConvertAddress2.Length > 16, XLColor.Yellow, XLColor.NoColor)

                .Cell(startrowposition + 4, 8).Value = ac.GetConvertAddress1
                .Cell(startrowposition + 4, 6).Value = ac.GetConvertAddress2

                '宛名
                addresseename = If(destinationdata.AddresseeName.GetName.Length > 5,
                    $"{Space(1)}{destinationdata.AddresseeName.GetName}{destinationdata.MyTitle.GetTitle}",
                    $"{Space(1)}{destinationdata.AddresseeName.GetName}{Space(1)}{destinationdata.MyTitle.GetTitle}")
                .Cell(startrowposition + 4, 2).Value = addresseename
            End With

        End Sub

        Public Function SetCellFont(_IsIPAmjMintyo As Boolean) As String Implements IExcelOutputBehavior.SetCellFont
            Return If(_IsIPAmjMintyo, My.Resources.IPAmjMintyoString, My.Resources.FontName_HGPGyoushotai)
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
        Friend ReadOnly IsIPAmjMintyo As Boolean

        Public Sub New(_addressee As DestinationDataEntity, _isIPAmjMintyo As Boolean)
            IsIPAmjMintyo = _isIPAmjMintyo
            AddresseeList = New ObservableCollection(Of DestinationDataEntity) From {_addressee}
        End Sub

        Public Sub New(_addresseelist As ObservableCollection(Of DestinationDataEntity))
            AddresseeList = _addresseelist
        End Sub

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                '郵便番号
                With .Cell(startrowposition + 2, 3).Style
                    .Font.FontSize = 36
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
                    With .Alignment
                        .Horizontal = XLAlignmentHorizontalValues.Center
                        .Vertical = XLAlignmentVerticalValues.Bottom
                        .TopToBottom = False
                    End With
                End With

                '住所
                With .Range(.Cell(startrowposition + 4, 5), .Cell(startrowposition + 4, 4)).Style
                    .Font.FontSize = If(IsIPAmjMintyo, 36, 43)
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
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
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
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
                Dim unused = .Range(.Cell(startrowposition + 2, 3), .Cell(startrowposition + 2, 5)).Merge()
                unused = .Range(.Cell(startrowposition + 5, 2), .Cell(startrowposition + 6, 2)).Merge()
                unused = .Range(.Cell(startrowposition + 4, 4), .Cell(startrowposition + 6, 4)).Merge()
                unused = .Range(.Cell(startrowposition + 4, 5), .Cell(startrowposition + 6, 5)).Merge()
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

                If ac.GetConvertAddress2.Length > 16 Then
                    .Cell(startrowposition + 4, 4).Style.Fill.BackgroundColor = XLColor.Yellow
                Else
                    .Cell(startrowposition + 4, 4).Style.Fill.BackgroundColor = XLColor.NoColor
                End If

                '宛名
                Dim name As String = destinationdata.AddresseeName.GetName
                If name.Length <= 5 Then name &= Space(1)
                .Cell(startrowposition + 5, 2).Value = $"{name}{destinationdata.MyTitle.GetTitle}"
            End With

        End Sub

        Public Function SetCellFont(_IsIPAmjMintyo As Boolean) As String Implements IExcelOutputBehavior.SetCellFont
            Return If(_IsIPAmjMintyo, My.Resources.IPAmjMintyoString, My.Resources.FontName_HGPGyoushotai)
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
            Return 15
        End Function

        Private Function GetLengthVerificationString(destinationData As DestinationDataEntity) As String Implements IVerticalOutputListBehavior.GetLengthVerificationString
            Return destinationData.MyAddress2.Address
        End Function

    End Class

    ''' <summary>
    ''' 墓地パンフクラス
    ''' </summary>
    Private Class GravePamphletEnvelope
        Implements IVerticalOutputListBehavior

        Private ReadOnly AddresseeList As ObservableCollection(Of DestinationDataEntity)
        Private ReadOnly IsIPAmjMintyo As Boolean = False

        Public Sub New(_addressee As DestinationDataEntity, _isIPAmjMintyo As Boolean)
            IsIPAmjMintyo = _isIPAmjMintyo
            AddresseeList = New ObservableCollection(Of DestinationDataEntity) From {_addressee}
        End Sub

        Public Sub New(_addresseelist As ObservableCollection(Of DestinationDataEntity))
            AddresseeList = _addresseelist
        End Sub

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin

            With ExlWorkSheet
                Dim unused = .Range(.Cell(startrowposition + 2, 3), .Cell(startrowposition + 2, 5)).Merge()
                unused = .Range(.Cell(startrowposition + 4, 2), .Cell(startrowposition + 5, 2)).Merge()
                unused = .Range(.Cell(startrowposition + 4, 4), .Cell(startrowposition + 5, 4)).Merge()
                unused = .Range(.Cell(startrowposition + 4, 5), .Cell(startrowposition + 5, 5)).Merge()
            End With

        End Sub

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                '郵便番号
                With .Cell(startrowposition + 2, 3).Style
                    .Font.FontSize = 36
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
                    With .Alignment
                        .Horizontal = XLAlignmentHorizontalValues.Center
                        .Vertical = XLAlignmentVerticalValues.Bottom
                        .TopToBottom = False
                    End With
                End With

                '住所
                With .Range(.Cell(startrowposition + 4, 5), .Cell(startrowposition + 4, 4)).Style
                    .Font.FontSize = If(IsIPAmjMintyo, 36, 43)
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
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
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
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
                Dim ad1 As String = ac.GetConvertAddress1
                Dim ad2 As String = ac.GetConvertAddress2

                If ad1.Length + ad2.Length < 16 Then
                    .Cell(startrowposition + 4, 5).Value = $"{ad1}{ad2}"
                    .Cell(startrowposition + 4, 4).Value = String.Empty
                Else
                    .Cell(startrowposition + 4, 5).Value = ad1
                    .Cell(startrowposition + 4, 4).Value = ad2
                End If

                If ac.GetConvertAddress2.Length > GetAddressMaxLength() Then
                    .Cell(startrowposition + 4, 4).Style.Fill.BackgroundColor = XLColor.Yellow
                Else
                    .Cell(startrowposition + 4, 4).Style.Fill.BackgroundColor = XLColor.NoColor
                End If

                '宛名
                Dim name As String = destinationdata.AddresseeName.GetName
                If name.Length <= 5 Then name &= Space(1)
                .Cell(startrowposition + 4, 2).Value = $"{name}{destinationdata.MyTitle.GetTitle}"
            End With

        End Sub

        Public Function SetCellFont(_IsIPAmjMintyo As Boolean) As String Implements IExcelOutputBehavior.SetCellFont
            Return If(_IsIPAmjMintyo, My.Resources.IPAmjMintyoString, My.Resources.FontName_HGPGyoushotai)
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
            Return {45.75, 132.75, 51.75, 409.5, 375, 34.5}
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
            Return 15
        End Function

        Public Function GetLengthVerificationString(destinationData As DestinationDataEntity) As String Implements IVerticalOutputListBehavior.GetLengthVerificationString
            Return destinationData.MyAddress2.Address
        End Function
    End Class

    ''' <summary>
    ''' はがきクラス
    ''' </summary>
    Private Class Postcard
        Implements IVerticalOutputListBehavior

        Private ReadOnly AddresseeList As ObservableCollection(Of DestinationDataEntity)
        Private ReadOnly IsIPAmjMintyo As Boolean = False

        Public Sub New(_addressee As DestinationDataEntity, _isIPAmjMintyo As Boolean)
            IsIPAmjMintyo = _isIPAmjMintyo
            AddresseeList = New ObservableCollection(Of DestinationDataEntity) From {_addressee}
        End Sub

        Public Sub New(_addresseelist As ObservableCollection(Of DestinationDataEntity))
            AddresseeList = _addresseelist
        End Sub

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                .PageSetup.PaperSize = XLPaperSize.EPaper
                '郵便番号
                With .Range(.Cell(startrowposition + 2, 3), .Cell(startrowposition + 2, 9)).Style
                    .Font.FontSize = 16
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
                    .Alignment.Vertical = XLAlignmentVerticalValues.Top
                End With
                .Range(.Cell(startrowposition + 2, 6), .Cell(startrowposition + 2, 9)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                .Range(.Cell(startrowposition + 2, 3), .Cell(startrowposition + 2, 5)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left

                '住所
                With .Range(.Cell(startrowposition + 4, 6), .Cell(startrowposition + 4, 8)).Style
                    .Font.FontSize = 18
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                    .Alignment.TopToBottom = True
                End With
                .Cell(startrowposition + 4, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top
                .Cell(startrowposition + 4, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                '宛名
                With .Cell(startrowposition + 4, 2).Style
                    .Font.FontSize = 36
                    .Font.FontName = SetCellFont(IsIPAmjMintyo)
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
                Dim unused = .Range(.Cell(startrowposition + 4, 2), .Cell(startrowposition + 4, 5)).Merge()
                unused = .Range(.Cell(startrowposition + 4, 8), .Cell(startrowposition + 4, 9)).Merge()
                unused = .Range(.Cell(startrowposition + 4, 6), .Cell(startrowposition + 4, 7)).Merge()
            End With

        End Sub

        Public Sub SetData(startrowposition As Integer, destinationdata As DestinationDataEntity) Implements IVerticalOutputListBehavior.SetData

            Dim addressText1 As String = String.Empty
            Dim addressText2 As String = String.Empty
            Dim addresseeName, postalcode As String

            With ExlWorkSheet
                '郵便番号
                postalcode = Replace(destinationdata.MyPostalCode.GetCode, "-", String.Empty)
                If postalcode.Length = 7 Then
                    For I As Integer = 1 To 7
                        .Cell(startrowposition + 2, I + 2).Value = postalcode.Substring(I - 1, 1)
                    Next
                Else
                    .Range(.Cell(startrowposition + 2, 3), .Cell(startrowposition + 2, 9)).Style.Fill.BackgroundColor = XLColor.Yellow
                End If

                '住所
                Dim ac As New AddressConvert(destinationdata.MyAddress1.Address, destinationdata.MyAddress2.Address)
                If ac.GetConvertAddress1.Length + ac.GetConvertAddress2.Length < 14 Then
                    addressText1 = $"{ac.GetConvertAddress1}{Space(1)}{ac.GetConvertAddress2}"
                    addressText2 = String.Empty
                Else
                    addressText1 = ac.GetConvertAddress1
                    addressText2 = ac.GetConvertAddress2
                End If
                .Cell(startrowposition + 4, 6).Style.Fill.BackgroundColor = If(ac.GetConvertAddress2.Length > GetAddressMaxLength(), XLColor.Yellow, XLColor.NoColor)
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

        Public Function SetCellFont(_IsIPAmjMintyo As Boolean) As String Implements IExcelOutputBehavior.SetCellFont
            Return If(_IsIPAmjMintyo, My.Resources.IPAmjMintyoString, My.Resources.FontName_HGPGyoushotai)
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
            Return 15
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

        Private ReadOnly MyList As ObservableCollection(Of DestinationDataEntity)
        Private ReadOnly IsIPAmjMintyo As Boolean

        Public Sub New(list As ObservableCollection(Of DestinationDataEntity), _isIPAmjMintyo As Boolean)
            MyList = list
            IsIPAmjMintyo = _isIPAmjMintyo
        End Sub

        ''' <summary>
        ''' ラベルに入力する文字列を返します
        ''' </summary>
        ''' <param name="lineindex">行番号</param>
        ''' <param name="addressee">ラベル化する宛先</param>
        ''' <returns></returns>
        Private Function ReturnLabelString(lineindex As Integer, addressee As DestinationDataEntity) As String

            'セルに入力する宛先を格納する文字列　初期値に郵便番号
            Dim ReturnString As String = $"{Space(10)}〒 {addressee.MyPostalCode.GetCode}{vbNewLine}{vbNewLine}"
            Dim ac As New AddressConvert(addressee.MyAddress1.Address, addressee.MyAddress2.Address)
            ReturnString &= $"{Space(10)}{ac.GetConvertAddress1}{vbCrLf}"  '住所1

            Try
                ReturnString &= $"{Space(10)}{addressee.MyAddress2.Address.Substring(0, 15)}{vbNewLine}"   '住所2
                ReturnString &= $"{Space(10)}{addressee.MyAddress2.Address.Substring(15)}{vbNewLine}{vbNewLine}" '住所2（2行目）
            Catch ex As ArgumentOutOfRangeException
                '住所2の文字列が短い場合のエラー対応（16文字以下ならエラー）
                ReturnString &= $"{Space(10)}{addressee.MyAddress2.Address}{vbNewLine}{vbNewLine}{vbNewLine}"
            End Try

            '宛名
            ReturnString &= $"{Space(10)}{addressee.AddresseeName.GetName}{Space(1)}{addressee.MyTitle.GetTitle}{vbNewLine}"

            'ラベルの行数によって、行を挿入する
            If lineindex > 4 Then
                ReturnString = $"{vbNewLine}{vbNewLine}{ReturnString}"
            End If

            Return ReturnString

        End Function

        Public Function SetCellFont(_IsIPAmjMintyo As Boolean) As String Implements IExcelOutputBehavior.SetCellFont
            Return If(_IsIPAmjMintyo, My.Resources.IPAmjMintyoString, My.Resources.FontName_MSPGothic)
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
            Return {34, 34, 30.86}
        End Function

        Private Function SetRowSizes() As Double() Implements IHorizontalOutputBehavior.SetRowSizes
            Return {120, 120, 120, 120, 120, 120, 120}
        End Function

        Public Function GetDataName() As String Implements IHorizontalOutputBehavior.GetDataName
            Return ToString()
        End Function

        Private Sub SetData(destinationdata As DestinationDataEntity) Implements IHorizontalOutputBehavior.SetData

            Dim column As Integer = 1
            Dim row As Integer = 1
            Dim rowCount As Integer = 1

            With ExlWorkSheet
                Do Until .Cell(row, column).Value.Trim.Length = 0
                    column += 1
                    If column > 3 Then
                        column = 1
                        row += 1
                        rowCount += 1
                    End If
                    If rowCount > 7 Then
                        rowCount = 1
                    End If
                Loop

                .Cell(row, column).Value = ReturnLabelString(rowCount, destinationdata)
            End With

        End Sub

        Public Function SetPrintAreaString() As String Implements IExcelOutputBehavior.SetPrintAreaString
            Return "a:c"
        End Function

        Public Function GetDestinationDataList() As ObservableCollection(Of DestinationDataEntity) Implements IHorizontalOutputBehavior.GetDestinationDataList
            Return MyList
        End Function
    End Class

End Class
