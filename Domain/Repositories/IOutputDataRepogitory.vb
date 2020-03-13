''' <summary>
''' 出力するデータの処理を行うリポジトリ
''' </summary>
Public Interface IOutputDataRepogitory

    ''' <summary>
    ''' 振込用紙
    ''' </summary>
    Sub TransferPaperPrintOutput(ByVal addressee As String, ByVal title As String, ByVal postalcode As String,
                                 ByVal address1 As String, ByVal address2 As String, ByVal money As String, ByVal note1 As String,
                                 ByVal note2 As String, ByVal note3 As String, ByVal note4 As String, ByVal note5 As String, ByVal multioutput As Boolean)
    ''' <summary>
    ''' 長3封筒
    ''' </summary>
    Sub Cho3EnvelopeOutput(ByVal addressee As String, ByVal title As String, ByVal postalcode As String,
                           ByVal address1 As String, ByVal address2 As String, ByVal multioutput As Boolean)
    ''' <summary>
    ''' 洋封筒
    ''' </summary>
    Sub WesternEnvelopeOutput(ByVal addressee As String, ByVal title As String, ByVal postalcode As String,
                              ByVal address1 As String, ByVal address2 As String, ByVal multioutput As Boolean)

    ''' <summary>
    ''' 角2
    ''' </summary>
    Sub Kaku2EnvelopeOutput(ByVal addressee As String, ByVal title As String, ByVal postalcode As String,
                            ByVal address1 As String, ByVal address2 As String, ByVal multioutput As Boolean)

    ''' <summary>
    ''' 墓地パンフ
    ''' </summary>
    Sub GravePamphletOutput(ByVal addressee As String, ByVal title As String, ByVal postalcode As String,
                            ByVal address1 As String, ByVal address2 As String, ByVal multioutput As Boolean)

    ''' <summary>
    ''' はがき
    ''' </summary>
    Sub PostcardOutput(ByVal addressee As String, ByVal title As String, ByVal postalcode As String, ByVal address1 As String,
                       ByVal address2 As String, ByVal multioutput As Boolean)

    ''' <summary>
    ''' ラベル
    ''' </summary>
    Sub LabelOutput(ByVal addressee As String, ByVal title As String, ByVal postalcode As String, ByVal address1 As String,
                    ByVal address2 As String)

    ''' <summary>
    ''' 墓地札
    ''' </summary>
    Sub GravePanelOutput(ByVal gravenumber As String, ByVal familyname As String, ByVal contractcontent As String, ByVal area As Double, ByVal startposition As Integer)

End Interface
