''' <summary>
''' エクセルの処理を行うリポジトリ
''' </summary>
Public Interface IAdresseeOutputRepogitory

    Enum OutputData
        Transfer
        Cho3
        Western
        Kaku2
        GravePamphlet
        Postcard
        Label
    End Enum

    ''' <summary>
    ''' 振込用紙入力
    ''' </summary>
    Sub DataInput(ByVal addressee As String, ByVal title As String, ByVal postalcode As String, ByVal address1 As String,
                           ByVal address2 As String, ByVal outputcontents As OutputData, Optional ByVal money As String = "", Optional ByVal note1 As String = "",
                           Optional ByVal note2 As String = "", Optional ByVal note3 As String = "", Optional ByVal note4 As String = "",
                           Optional ByVal note5 As String = "", Optional ByVal addressee_index As Integer = 0)

    ''' <summary>
    ''' アウトプットするメディア（呼び方違うなら名前変えます）をクローズします
    ''' </summary>
    Sub OutputMediaClose()

End Interface
