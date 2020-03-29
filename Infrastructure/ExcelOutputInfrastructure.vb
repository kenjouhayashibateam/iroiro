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
''' エクセルデータを横向けに出力
''' </summary>
Interface IHorizontalOutputBehavior
    Inherits IExcelOutputBehavior

    ''' <summary>
    ''' 出力するデータをセットします
    ''' </summary>
    Sub SetData(ByVal destinationdata As DestinationDataEntity)

End Interface

Interface IVerticalOutputListBehavior
    Inherits IVerticalOutputBehavior

    ''' <summary>
    ''' 出力するデータをセットします
    ''' </summary>
    ''' <param name="startrowposition"></param>
    Sub SetData(ByVal startrowposition As Integer, ByVal destinationdata As DestinationDataEntity)

    Function GetDestinationDataList() As ObservableCollection(Of DestinationDataEntity)
End Interface

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
        AddressText = Replace(AddressText, "東京都", "")
        AddressText = Replace(AddressText, "神奈川県", "")
        AddressText = Replace(AddressText, "徳島県", "")
        If AddressText Is Nothing Then Return String.Empty
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
        If address.Substring(0, InStr(1, address, verifystring) - 1) = address.Substring(InStr(1, address, verifystring), InStr(1, address, "市") - InStr(1, address, verifystring) - 1) Then
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

        basestring = Regex.Replace(StrConv(Address2, vbWide), "[^０-９]", "*")
        Dim numberarray() As String = Split(basestring, "*")
        For i As Integer = 0 To UBound(numberarray)
            numberarray(i) = BranchConvertNumber(Trim(numberarray(i)))
        Next

        basestring = Regex.Replace(StrConv(Address2, vbWide), "[０-９]", "*")

        Do Until InStr(basestring, "**") = 0
            basestring = Replace(basestring, "**", "*")
        Loop

        For j As Integer = 0 To UBound(numberarray)
            If Not String.IsNullOrEmpty(numberarray(j)) Then basestring = Replace(basestring, "*", numberarray(j), 1, 1)
        Next
        basestring = Replace(basestring, "－", "ー")
        Return Replace(basestring, "*", String.Empty)

    End Function

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
    Implements IOutputDataRepogitory

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

    Private Shared StartIndex As Integer

    Private Const SAVEPATH As String = ".\files\IroiroFile.xlsx"

    Private Const FileName As String = "Iroiro"

    Private ReadOnly buf As String = Dir(SAVEPATH)

    Private exlworkbooks As Excel.Workbooks

    Private ReadOnly AddresseeList As List(Of DestinationDataEntity)

    Private exlapp As Excel.Application
    ''' <summary>
    ''' エクセルを起動して、アプリ用のブックを開きます
    ''' </summary>
    Private Sub SheetSetting()

        Dim bolSheetCheck As Boolean = False
        Dim myWorkbook As Excel.Workbook = Nothing

        Try
            exlapp = GetObject(, "Excel.Application")
        Catch ex As Exception
            exlapp = CreateObject("Excel.Application")
        End Try

        exlworkbooks = exlapp.Workbooks

        For Each myWorkbook In exlworkbooks
            If myWorkbook.Name <> buf Then Continue For
            bolSheetCheck = True
            Exit For
        Next

        If bolSheetCheck Then
            myWorkbook.Close(False)
        End If

        If exlworkbooks.Count = 0 Then exlapp.Quit()

        ExlWorkbook = New XLWorkbook
        If ExlWorkSheet Is Nothing Then ExlWorkSheet = ExlWorkbook.AddWorksheet(FileName)

        ExlWorkSheet.Cells.Style.NumberFormat.SetNumberFormatId(49)

    End Sub

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
            Dim openpath As String = System.IO.Path.GetFullPath(SAVEPATH)
            Dim executebook As Excel.Workbook = exlworkbooks.Open(openpath, , True)
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
            If OutputDataGanre <> Hob.GetDataName Then
                OutputDataGanre = Hob.GetDataName
                ColumnSizes = Hob.SetColumnSizes()
                RowSizes = Hob.SetRowSizes()
                .Cells.Clear()
                Hob.SetCellFont()
                .PageSetup.PrintAreas.Clear()
                .PageSetup.PrintAreas.Add(Hob.SetPrintAreaString)
            End If

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
            Next
        End With

        ExlWorkbook.SaveAs(SAVEPATH)

        ExcelOpen()

    End Sub

    ''' <summary>
    ''' 縦向けにリストのデータを入力する処理
    ''' </summary>
    ''' <param name="_vob"></param>
    ''' <param name="ismulti">複数印刷Behaviorをするかを設定します</param>
    Private Sub ListOutputVerticalProcessing(ByVal _vob As IVerticalOutputListBehavior, ByVal ismulti As Boolean)

        Volb = _vob

        SheetSetting()

        With ExlWorkSheet
            '出力するデータの種類が違えばセルをクリアする
            If OutputDataGanre <> Volb.GetDataName Then
                ColumnSizes = Volb.SetColumnSizes()
                RowSizes = Volb.SetRowSizes()
                OutputDataGanre = Volb.GetDataName
                SetMargin()
                .Cells.Clear()
                'ColumnSizesの配列の中の数字をシートのカラムの幅に設定する
                For I As Integer = 0 To UBound(ColumnSizes)
                    .Column(I + 1).Width = ColumnSizes(I)
                Next
                .PageSetup.PrintAreas.Clear()
                .PageSetup.PrintAreas.Add(Volb.SetPrintAreaString)
            End If

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
            Next
            .Style.Font.FontName = Volb.SetCellFont
        End With

        If ExlWorkbook.Worksheets.Count = 0 Then ExlWorkbook.AddWorksheet(ExlWorkSheet)
        ExlWorkbook.SaveAs(SAVEPATH)
        ExcelOpen()
    End Sub

    Private Sub GravePanelListOutputProcessing(ByVal _vob As IGravePanelListBehavior)

        Dim gpl As GravePanelDataListEntity = GravePanelDataListEntity.GetInstance
        gpb = _vob

        SheetSetting()

        With ExlWorkSheet
            '出力するデータの種類が違えばセルをクリアする
            If OutputDataGanre <> gpb.GetDataName Then
                ColumnSizes = gpb.SetColumnSizes()
                RowSizes = gpb.SetRowSizes()
                OutputDataGanre = gpb.GetDataName
                SetMargin()
                .Cells.Clear()
                'ColumnSizesの配列の中の数字をシートのカラムの幅に設定する
                For I As Integer = 0 To UBound(ColumnSizes)
                    .Column(I + 1).Width = ColumnSizes(I)
                Next
                .PageSetup.PrintAreas.Clear()
                .PageSetup.PrintAreas.Add(gpb.SetPrintAreaString)
            End If

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
        ExlWorkbook.SaveAs(SAVEPATH)
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

    Public Sub GravePanelOutput() Implements IOutputDataRepogitory.GravePanelOutput

        Dim gp As IGravePanelListBehavior = New GravePanel()
        GravePanelListOutputProcessing(gp)

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
        Throw New NotImplementedException()
    End Sub

    Public Sub GravePamphletOutput(list As ObservableCollection(Of DestinationDataEntity)) Implements IOutputDataRepogitory.GravePamphletOutput
        Dim gp As IVerticalOutputListBehavior = New GravePamphletEnvelope(list)
        ListOutputVerticalProcessing(gp, True)
    End Sub

    Public Sub PostcardOutput(list As ObservableCollection(Of DestinationDataEntity)) Implements IOutputDataRepogitory.PostcardOutput
        Dim pc As IVerticalOutputListBehavior = New Postcard(list)
        ListOutputVerticalProcessing(pc, True)
    End Sub

    ''' <summary>
    ''' 墓地札クラス
    ''' </summary>
    Private Class GravePanel
        Implements IGravePanelListBehavior

        Public Sub SetData(startrowposition As Integer, gravepanel As GravePanelDataEntity) Implements IGravePanelListBehavior.SetData
            With ExlWorkSheet
                .Cell(startrowposition + 1, 1).Value = NameConvert(gravepanel.MyFamilyName.Name) & "家"
                .Cell(startrowposition + 2, 1).Value = gravepanel.GetGraveNumber & Space(1) & gravepanel.GetArea & "㎡"
                .Cell(startrowposition + 3, 1).Value = "清掃契約"
                .Cell(startrowposition + 3, 2).Value = gravepanel.GetContractContent
            End With
        End Sub

        Private Function NameConvert(ByVal strName As String) As String

            Dim I As Integer = 0 'ループで使う添え字
            Dim nameArray() As String
            Dim nameValue As String = String.Empty

            ReDim nameArray(strName.Trim.Length - 1)

            Do Until I = strName.Trim.Length
                nameArray(I) = strName.Substring(I, 1)
                I += 1
            Loop

            For I = 0 To UBound(nameArray) Step 1
                If nameArray(I).Trim.Length <> 0 Then
                    nameValue &= Space(1) & nameArray(I)
                End If
            Next

            nameValue &= Space(1)
            If Len(Trim(nameValue)) = 1 Then nameValue &= Space(1)

            Return nameValue

        End Function

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin
            With ExlWorkSheet
                .Range(.Cell(startrowposition + 1, 1), .Cell(startrowposition + 1, 2)).Merge()
                .Range(.Cell(startrowposition + 2, 1), .Cell(startrowposition + 2, 2)).Merge()
            End With
        End Sub

        Public Function SetCellFont() As String Implements IExcelOutputBehavior.SetCellFont
            Return "HG正楷書体-PRO"
        End Function

        Public Sub CellProperty(startrowposition As Integer) Implements IExcelOutputBehavior.CellProperty

            With ExlWorkSheet
                With .Cell(startrowposition + 1, 1).Style
                    .Font.FontSize = 65
                    .Font.Bold = True
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                End With
                With .Range(.Cell(startrowposition + 2, 1), .Cell(startrowposition + 3, 2)).Style
                    .Font.FontSize = 48
                    .Font.Bold = True
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
            Return {43.88, 51.75}
        End Function

        Public Function SetRowSizes() As Double() Implements IExcelOutputBehavior.SetRowSizes
            Return {93.75, 67.5, 75}
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
                    .Cell(startrowposition + 2, I + 2).Value = Replace(destinationdata.MyPostalCode.GetCode, "-", "").Substring(I - 1, 1)
                Next

                '住所
                Dim addresstext1 As String = String.Empty
                Dim addresstext2 As String = String.Empty
                Dim ac As New AddressConvert(destinationdata.MyAddress1.GetAddress, destinationdata.MyAddress2.GetAddress)
                addresstext1 = destinationdata.MyAddress1.GetAddress
                addresstext2 = destinationdata.MyAddress2.GetAddress
                If addresstext1.Length + addresstext2.Length < 15 Then
                    addresstext1 = destinationdata.MyAddress1.GetAddress & " " & destinationdata.MyAddress2.GetAddress
                    addresstext2 = String.Empty
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
                    addresseename = destinationdata.AddresseeName.GetName & destinationdata.MyTitle.GetTitle
                Else
                    addresseename = destinationdata.AddresseeName.GetName & " " & destinationdata.MyTitle.GetTitle
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
            Return "HGP行書体"
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

        Public Function SetCellFont() As String Implements IExcelOutputBehavior.SetCellFont
            Return "ＭＳ Ｐ明朝"
        End Function

        Private Function IExcelOutputBehavior_SetColumnSizes1() As Double() Implements IVerticalOutputBehavior.SetColumnSizes
            Return {3.71, 25.14, 7.57, 1.71, 1.71, 1.71, 1.71, 1.71, 1.71, 1.71, 1.71, 7.29, 1.71, 1.71, 1.71, 1.71, 1.71, 1.71, 1.71, 1.71, 0.31}
        End Function

        Private Function IExcelOutputBehavior_SetRowSizes1() As Double() Implements IVerticalOutputBehavior.SetRowSizes
            Return {279, 172.5, 19.5, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 101.25}
        End Function

        Public Sub CellProperty(startrowposition As Integer) Implements IVerticalOutputBehavior.CellProperty

            With ExlWorkSheet
                .PageSetup.PaperSize = XLPaperSize.B5Paper
                '宛名欄
                .Cell(11, 2).Style.Font.FontSize = 14
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
                Dim moneystring As String = "\" & destinationdata.MoneyData.GetMoney
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
                .Cell(startrowposition + 7, 2).Value = "〒 " & destinationdata.MyPostalCode.GetCode      '宛先郵便番号
                Dim ac As New AddressConvert(destinationdata.MyAddress1.GetAddress, destinationdata.MyAddress2.GetAddress)
                .Cell(startrowposition + 8, 2).Value = ac.GetConvertAddress1         '宛先住所1

                Dim stringlength As Integer
                If destinationdata.MyAddress2.GetAddress.Length < 20 Then
                    stringlength = destinationdata.MyAddress2.GetAddress.Length
                Else
                    stringlength = 20
                End If
                '宛先住所2　長い場合は2行で入力
                .Cell(startrowposition + 9, 2).Value = destinationdata.MyAddress2.GetAddress.Substring(0, stringlength)
                If destinationdata.MyAddress2.GetAddress.Length > 20 Then .Cell(startrowposition + 9, 2).Value = destinationdata.MyAddress2.GetAddress.Substring(20)

                .Cell(startrowposition + 11, 2).Value = destinationdata.AddresseeName.GetName & " " & destinationdata.MyTitle.GetTitle  '宛先の宛名
                .Cell(startrowposition + 13, 13).Value = destinationdata.AddresseeName.GetName & " " & destinationdata.MyTitle.GetTitle 'お客様控えの名前

                'お客様控え住所　長い場合は3行、それでも収まらない場合は注意を促す
                Dim strings() As String = SplitYourCopyAddress(ac.GetConvertAddress1, destinationdata.MyAddress2.GetAddress)
                .Cell(startrowposition + 10, 13).Value = " " & strings(0)
                .Cell(startrowposition + 11, 13).Value = " " & strings(1)
                .Cell(startrowposition + 12, 13).Value = " " & strings(2)
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

            Dim addresstext1 As String = ""
            Dim addresstext2 As String = ""
            Dim addresseename As String

            With ExlWorkSheet
                '郵便番号
                For I As Integer = 1 To 7
                    .Cell(startrowposition + 2, I + 2).Value = Replace(destinationdata.MyPostalCode.GetCode, "-", "").Substring(I - 1, 1)
                Next

                Dim ac As New AddressConvert(destinationdata.MyAddress1.GetAddress, destinationdata.MyAddress2.GetAddress)
                '住所
                If ac.GetConvertAddress1.Length + ac.GetConvertAddress2.Length < 14 Then
                    addresstext1 = ac.GetConvertAddress1 & " " & ac.GetConvertAddress2
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
                .Cell(startrowposition + 4, 6).Value = ac.GetConvertAddress1

                '宛名
                If destinationdata.AddresseeName.GetName.Length > 5 Then
                    addresseename = destinationdata.AddresseeName.GetName & destinationdata.MyTitle.GetTitle
                Else
                    addresseename = destinationdata.AddresseeName.GetName & " " & destinationdata.MyTitle.GetTitle
                End If
                .Cell(startrowposition + 4, 2).Value = addresseename
            End With

        End Sub

        Public Function SetCellFont() As String Implements IExcelOutputBehavior.SetCellFont
            Return "HGP行書体"
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

        Public Sub CellsJoin(startrowposition As Integer) Implements IVerticalOutputBehavior.CellsJoin

            With ExlWorkSheet
                .Range(.Cell(startrowposition + 2, 3), .Cell(startrowposition + 2, 5)).Merge()
                .Range(.Cell(startrowposition + 4, 2), .Cell(startrowposition + 5, 2)).Merge()
                .Range(.Cell(startrowposition + 4, 4), .Cell(startrowposition + 5, 4)).Merge()
                .Range(.Cell(startrowposition + 4, 5), .Cell(startrowposition + 5, 5)).Merge()
            End With

        End Sub

        Public Sub SetData(startrowposition As Integer, destinationdata As DestinationDataEntity) Implements IVerticalOutputListBehavior.SetData

            With ExlWorkSheet
                '郵便番号
                .Cell(startrowposition + 2, 3).Value = "〒 " & destinationdata.MyPostalCode.GetCode
                '住所
                Dim ac As New AddressConvert(destinationdata.MyAddress1.GetAddress, destinationdata.MyAddress2.GetAddress)
                .Cell(startrowposition + 4, 5).Value = ac.GetConvertAddress1
                .Cell(startrowposition + 4, 4).Value = ac.GetConvertAddress2
                '宛名
                .Cell(startrowposition + 4, 2).Value = destinationdata.AddresseeName.GetName & " " & destinationdata.MyTitle.GetTitle
            End With

        End Sub

        Public Function SetCellFont() As String Implements IExcelOutputBehavior.SetCellFont
            Return "HGP行書体"
        End Function

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

        Public Function SetPrintAreaString() As String Implements IExcelOutputBehavior.SetPrintAreaString
            Return "a:f"
        End Function

        Public Function GetDestinationDataList() As ObservableCollection(Of DestinationDataEntity) Implements IVerticalOutputListBehavior.GetDestinationDataList
            Return AddresseeList
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
                .Cell(startrowposition + 2, 3).Value = "〒 " & destinationdata.MyPostalCode.GetCode
                '住所
                Dim ac As New AddressConvert(destinationdata.MyAddress1.GetAddress, destinationdata.MyAddress2.GetAddress)
                .Cell(startrowposition + 4, 5).Value = ac.GetConvertAddress1
                .Cell(startrowposition + 4, 4).Value = ac.GetConvertAddress2
                '宛名
                .Cell(startrowposition + 4, 2).Value = destinationdata.AddresseeName.GetName & " " & destinationdata.MyTitle.GetTitle
            End With

        End Sub

        Public Function SetCellFont() As String Implements IExcelOutputBehavior.SetCellFont
            Return "HGP行書体"
        End Function

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

        Public Function SetPrintAreaString() As String Implements IExcelOutputBehavior.SetPrintAreaString
            Return "a:e"
        End Function

        Public Function GetDestinationDataList() As ObservableCollection(Of DestinationDataEntity) Implements IVerticalOutputListBehavior.GetDestinationDataList
            Return AddresseeList
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
                '郵便番号
                With .Range(.Cell(startrowposition + 2, 3), .Cell(startrowposition + 2, 10)).Style
                    .Font.FontSize = 16
                    .Alignment.Vertical = XLAlignmentVerticalValues.Top
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                End With
                '住所
                With .Range(.Cell(startrowposition + 4, 7), .Cell(startrowposition + 4, 9)).Style
                    .Font.FontSize = 18
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Alignment.TopToBottom = True
                End With
                .Cell(startrowposition + 4, 9).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top
                .Cell(startrowposition + 4, 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
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
                .Range(.Cell(startrowposition + 4, 9), .Cell(startrowposition + 4, 10)).Merge()
                .Range(.Cell(startrowposition + 4, 7), .Cell(startrowposition + 4, 8)).Merge()
            End With

        End Sub

        Public Sub SetData(startrowposition As Integer, destinationdata As DestinationDataEntity) Implements IVerticalOutputListBehavior.SetData

            Dim addresstext1 As String = ""
            Dim addresstext2 As String = ""
            Dim addresseename As String

            With ExlWorkSheet
                '郵便番号
                For I As Integer = 1 To 8
                    If I = 4 Then Continue For
                    .Cell(startrowposition + 2, I + 2).Value = destinationdata.MyPostalCode.GetCode.Substring(I - 1, 1)
                Next

                '住所
                Dim ac As New AddressConvert(destinationdata.MyAddress1.GetAddress, destinationdata.MyAddress2.GetAddress)
                If ac.GetConvertAddress1.Length + ac.GetConvertAddress2.Length < 14 Then
                    addresstext1 = ac.GetConvertAddress1 & " " & ac.GetConvertAddress2
                Else
                    addresstext1 = ac.GetConvertAddress1
                    addresstext2 = ac.GetConvertAddress2
                End If
                If ac.GetConvertAddress2.Length > 14 Then
                    .Cell(startrowposition + 4, 7).Style.Fill.BackgroundColor = XLColor.Yellow
                Else
                    .Cell(startrowposition + 4, 7).Style.Fill.BackgroundColor = XLColor.NoColor
                End If
                .Cell(startrowposition + 4, 9).Value = addresstext1
                .Cell(startrowposition + 4, 7).Value = addresstext2

                '宛名
                If destinationdata.AddresseeName.GetName.Length > 5 Then
                    addresseename = destinationdata.AddresseeName.GetName & destinationdata.MyTitle.GetTitle
                Else
                    addresseename = destinationdata.AddresseeName.GetName & " " & destinationdata.MyTitle.GetTitle
                End If
                .Cell(startrowposition + 4, 2).Value = addresseename
            End With

        End Sub

        Public Function SetCellFont() As String Implements IExcelOutputBehavior.SetCellFont
            Return "HGP行書体"
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
            Return "a:k"
        End Function

        Public Function GetDestinationDataList() As ObservableCollection(Of DestinationDataEntity) Implements IVerticalOutputListBehavior.GetDestinationDataList
            Return AddresseeList
        End Function

        Private Function SetColumnSizes() As Double() Implements IVerticalOutputBehavior.SetColumnSizes
            Return {11.38, 3.63, 2.75, 2.75, 2.75, 0.62, 2.75, 2.75, 2.75, 2.75, 0.77}
        End Function

        Private Function SetRowSizes() As Double() Implements IVerticalOutputBehavior.SetRowSizes
            Return {30, 22.5, 22.5, 326.25}
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
            Dim ReturnString As String = Space(10) & "〒 " & addressee.MyPostalCode.GetCode & vbNewLine & vbNewLine
            Dim ac As New AddressConvert(addressee.MyAddress1.GetAddress, addressee.MyAddress2.GetAddress)
            ReturnString &= Space(10) & ac.GetConvertAddress1 & vbCrLf  '住所1

            Try
                ReturnString &= Space(10) & addressee.MyAddress2.GetAddress.Substring(0, 16) & vbNewLine   '住所2
                ReturnString &= Space(10) & addressee.MyAddress2.GetAddress.Substring(16) & vbNewLine & vbNewLine '住所2（2行目）
            Catch ex As ArgumentOutOfRangeException
                '住所2の文字列が短い場合のエラー対応（16文字以下ならエラー）
                ReturnString &= Space(10) & addressee.MyAddress2.GetAddress & vbNewLine & vbNewLine & vbNewLine
            End Try

            '宛名
            ReturnString &= Space(10) & addressee.AddresseeName.GetName & " " & addressee.MyTitle.GetTitle & vbNewLine

            'ラベルの行数によって、行を挿入する
            If lineindex Mod 6 = 0 Then
                ReturnString = vbNewLine & vbNewLine & ReturnString
                Return ReturnString
            End If

            If lineindex Mod 7 = 0 Then ReturnString = vbNewLine & vbNewLine & vbNewLine & ReturnString

            Return ReturnString

        End Function

        Public Function SetCellFont() As String Implements IExcelOutputBehavior.SetCellFont
            Return "ＭＳ Ｐゴシック"
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
