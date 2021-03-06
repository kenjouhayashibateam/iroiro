﻿Imports System.Collections.ObjectModel

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
    Sub TransferPaperPrintOutput(ByVal customerid As String, ByVal addressee As String, ByVal title As String, ByVal postalcode As String,
                                 ByVal address1 As String, ByVal address2 As String, ByVal money As String, ByVal note1 As String,
                                 ByVal note2 As String, ByVal note3 As String, ByVal note4 As String, ByVal note5 As String, ByVal multioutput As Boolean)
    ''' <summary>
    ''' 長3封筒
    ''' </summary>
    Sub Cho3EnvelopeOutput(ByVal customerid As String, ByVal addressee As String, ByVal title As String, ByVal postalcode As String,
                           ByVal address1 As String, ByVal address2 As String, ByVal multioutput As Boolean)
    ''' <summary>
    ''' 長3封筒
    ''' </summary>
    Sub Cho3EnvelopeOutput(ByVal list As ObservableCollection(Of DestinationDataEntity))
    ''' <summary>
    ''' 洋封筒
    ''' </summary>
    Sub WesternEnvelopeOutput(ByVal customerid As String, ByVal addressee As String, ByVal title As String, ByVal postalcode As String,
                              ByVal address1 As String, ByVal address2 As String, ByVal multioutput As Boolean)
    ''' <summary>
    ''' 洋封筒
    ''' </summary>
    Sub WesternEnvelopeOutput(ByVal list As ObservableCollection(Of DestinationDataEntity))
    ''' <summary>
    ''' 角2封筒
    ''' </summary>
    Sub Kaku2EnvelopeOutput(ByVal customerid As String, ByVal addressee As String, ByVal title As String, ByVal postalcode As String,
                            ByVal address1 As String, ByVal address2 As String, ByVal multioutput As Boolean)
    ''' <summary>
    ''' 角2封筒
    ''' </summary>
    Sub Kaku2EnvelopeOutput(ByVal list As ObservableCollection(Of DestinationDataEntity))
    ''' <summary>
    ''' 墓地パンフ封筒
    ''' </summary>
    Sub GravePamphletOutput(ByVal customerid As String, ByVal addressee As String, ByVal title As String, ByVal postalcode As String,
                            ByVal address1 As String, ByVal address2 As String, ByVal multioutput As Boolean)
    ''' <summary>
    ''' 墓地パンフ封筒
    ''' </summary>
    Sub GravePamphletOutput(ByVal list As ObservableCollection(Of DestinationDataEntity))
    ''' <summary>
    ''' はがき
    ''' </summary>
    Sub PostcardOutput(ByVal customerid As String, ByVal addressee As String, ByVal title As String, ByVal postalcode As String, ByVal address1 As String,
                       ByVal address2 As String, ByVal multioutput As Boolean)
    ''' <summary>
    ''' はがき
    ''' </summary>
    Sub PostcardOutput(ByVal list As ObservableCollection(Of DestinationDataEntity))
    ''' <summary>
    ''' ラベル用紙
    ''' </summary>
    Sub LabelOutput(ByVal list As ObservableCollection(Of DestinationDataEntity))

    ''' <summary>
    ''' 墓地札
    ''' </summary>
    Sub GravePanelOutput(ByVal outputPosition As Integer)

    ''' <summary>
    ''' 進捗カウントを受け取るリスナーを登録します
    ''' </summary>
    ''' <param name="_listener"></param>
    Sub AddProcessedCountListener(ByVal _listener As IProcessedCountObserver)

    ''' <summary>
    ''' 住所の長いデータの数を受け取るリスナーを登録します
    ''' </summary>
    ''' <param name="_listener"></param>
    Sub AddOverLengthAddressListener(ByVal _listener As IOverLengthAddress2Count)

End Interface
