Imports System.Collections.ObjectModel

''' <summary>
''' リストの処理の進捗を知らせます
''' </summary>
Public Interface IProcessedCountObserver
    Sub ProcessedCountNotify(ByVal _count As Integer)
End Interface

''' <summary>
''' 住所の長いデータの数を知らせます
''' </summary>
Public Interface IOverLengthAddress2Count
    Sub OverLengthCountNotify(ByVal _count As Integer)
End Interface

''' <summary>
''' 出力するデータの処理を行うリポジトリ
''' </summary>
Public Interface IOutputDataRepogitory
    ''' <summary>
    ''' OutputしたデータをClearします
    ''' </summary>
    Sub DataClear()

    ''' <summary>
    ''' 振込用紙
    ''' </summary>
    Sub TransferPaperPrintOutput(customerid As String, addressee As String, title As String, postalcode As String,
                                 address1 As String, address2 As String, money As String, note1 As String, note2 As String,
                                 note3 As String, note4 As String, note5 As String, multioutput As Boolean, _isIPAmjMintyo As Boolean)
    ''' <summary>
    ''' 長3封筒
    ''' </summary>
    Sub Cho3EnvelopeOutput(customerid As String, addressee As String, title As String, postalcode As String,
                           address1 As String, address2 As String, multioutput As Boolean, _isIPAmjMintyo As Boolean)
    ''' <summary>
    ''' 長3封筒
    ''' </summary>
    Sub Cho3EnvelopeOutput(list As ObservableCollection(Of DestinationDataEntity), _isIPAmjMintyo As Boolean)
    ''' <summary>
    ''' 洋封筒
    ''' </summary>
    Sub WesternEnvelopeOutput(customerid As String, addressee As String, title As String, postalcode As String,
                              address1 As String, address2 As String, multioutput As Boolean, _isIPAmjMintyo As Boolean)
    ''' <summary>
    ''' 洋封筒
    ''' </summary>
    Sub WesternEnvelopeOutput(ByVal list As ObservableCollection(Of DestinationDataEntity), _isIPAmjMintyo As Boolean)
    ''' <summary>
    ''' 角2封筒
    ''' </summary>
    Sub Kaku2EnvelopeOutput(customerid As String, addressee As String, title As String, postalcode As String,
                            address1 As String, address2 As String, multioutput As Boolean, _isIPAmjMintyo As Boolean)
    ''' <summary>
    ''' 角2封筒
    ''' </summary>
    Sub Kaku2EnvelopeOutput(list As ObservableCollection(Of DestinationDataEntity), _isIPAmjMintyo As Boolean)
    ''' <summary>
    ''' 墓地パンフ封筒
    ''' </summary>
    Sub GravePamphletOutput(customerid As String, addressee As String, title As String, postalcode As String,
                            address1 As String, address2 As String, multioutput As Boolean, _isIPAmjMintyo As Boolean)
    ''' <summary>
    ''' 墓地パンフ封筒
    ''' </summary>
    Sub GravePamphletOutput(ByVal list As ObservableCollection(Of DestinationDataEntity), _isIPAmjMintyo As Boolean)
    ''' <summary>
    ''' はがき
    ''' </summary>
    Sub PostcardOutput(customerid As String, addressee As String, title As String, postalcode As String, address1 As String,
                       address2 As String, multioutput As Boolean, _isIPAmjMintyo As Boolean)
    ''' <summary>
    ''' はがき
    ''' </summary>
    Sub PostcardOutput(list As ObservableCollection(Of DestinationDataEntity), _isIPAmjMintyo As Boolean)
    ''' <summary>
    ''' ラベル用紙
    ''' </summary>
    Sub LabelOutput(list As ObservableCollection(Of DestinationDataEntity), _isIPAmjMintyo As Boolean)

    ''' <summary>
    ''' 墓地札
    ''' </summary>
    Sub GravePanelOutput(outputPosition As Integer, _isIPAmjMintyo As Boolean)

    ''' <summary>
    ''' 進捗カウントを受け取るリスナーを登録します
    ''' </summary>
    ''' <param name="_listener"></param>
    Sub AddProcessedCountListener(_listener As IProcessedCountObserver)

    ''' <summary>
    ''' 住所の長いデータの数を受け取るリスナーを登録します
    ''' </summary>
    ''' <param name="_listener"></param>
    Sub AddOverLengthAddressListener(_listener As IOverLengthAddress2Count)
    ''' <summary>
    ''' 受納証
    ''' </summary>
    Sub VoucherOutput(id As Integer, addressee As String, provisoList As ObservableCollection(Of Proviso), isShunjuen As Boolean, isReissue As Boolean, cleakName As String, isDisplayTax As Boolean, prepaidDate As Date, accountActivityDate As Date)

End Interface
