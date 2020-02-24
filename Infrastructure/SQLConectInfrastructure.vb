Imports Domain
Imports System.Collections.ObjectModel

''' <summary>
''' SQLServerと接続するモデルクラス
''' </summary>
Public Class SQLConectInfrastructure
    Implements IDataConectRepogitory

    ''' <summary>
    ''' データを取得するためのルートを確立するコマンドクラス
    ''' </summary>
    Private Property Cmd As ADODB.Command

    ''' <summary>
    ''' SQLServerに接続するための接続文字列
    ''' </summary>
    Private Const SHUNJUENCONSTRING As String = "PROVIDER=SQLOLEDB;SERVER=192.168.44.163\SQLEXPRESS2014;DATABASE=COMMON;user id=sa;password=sqlserver"

    ''' <summary>
    ''' コマンドから取得したデータを格納するクラス
    ''' </summary>
    Private Property Rs As ADODB.Recordset
    ''' <summary>
    ''' VB.NETとSQLServerを接続するクラス
    ''' </summary>
    Private Property Cn As ADODB.Connection

    ''' <summary>
    ''' Rsにデータを格納し、Rs.EOFの結果を返します
    ''' </summary>
    ''' <param name="exeCmd">使用するストアドプロシージャ等のデータを格納したコマンド</param>
    Private Function ExecuteStoredProc(ByRef exeCmd As ADODB.Command) As Boolean

        Cn = New ADODB.Connection With {.ConnectionString = SHUNJUENCONSTRING}
        Cn.Open()

        'SQLServerのストアドプロシージャを実行するためのCommandを設定して、引っ張ってきたレコードセットを返す
        With exeCmd
            .ActiveConnection = Cn
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            'レコードセットをRsに格納する
            Rs = .Execute
        End With

        Return Rs.EOF

    End Function

    ''' <summary>
    ''' ADODBのインスタンスを削除します
    ''' </summary>
    Private Sub ADONothing()
        Cn.Close()
        Cmd = Nothing
        Cn = Nothing
        Rs = Nothing
    End Sub


    ''' <summary>
    ''' 名義人データを検索し、Rs.EOFを返します
    ''' </summary>
    ''' <param name="strManagementNumber">検索する管理番号</param>
    Private Function SetLesseeRecord(ByVal strManagementNumber As String) As Boolean

        Cmd = New ADODB.Command

        With Cmd
            .CommandText = "Call_ShunjyuenData"
            .Parameters.Append(.CreateParameter("managementnumber", ADODB.DataTypeEnum.adChar,, 6, strManagementNumber))
            .Parameters.Append(.CreateParameter("lesseename", ADODB.DataTypeEnum.adVarChar,, 100, "%"))
        End With

        Return ExecuteStoredProc(Cmd)

    End Function

    ''' <summary>
    ''' レコードセットのフィールドのValueを文字列形式で返します
    ''' </summary>
    ''' <param name="FieldName">データベース（ストアドプロシージャ）から取得するフィールドの名前</param>
    Private Function RsFields(ByVal FieldName As String) As String
        If Rs.EOF Then Return String.Empty
        Return IIf(IsDBNull(Rs.Fields(FieldName).Value), String.Empty, Rs.Fields(FieldName).Value)
    End Function

    ''' <summary>
    ''' 名義人データを管理番号を元に生成し、返します
    ''' </summary>
    ''' <param name="customerid">検索する管理番号</param>
    Public Function GetCustomerInfo(customerid As String) As LesseeCustomerInfoEntity Implements IDataConectRepogitory.GetCustomerInfo

        Dim myLessee As LesseeCustomerInfoEntity
        Dim Area As Double

        If customerid = String.Empty Then Return Nothing
        If SetLesseeRecord(customerid) Then Return Nothing

        '御廟は面積がない上にDouble型なので、0にして対応する
        If RsFields("AreaOfGrave").Trim.Length = 0 Then
            Area = 0
        Else
            Area = RsFields("AreaOfGrave")
        End If

        myLessee = New LesseeCustomerInfoEntity(RsFields("ManagementNumber"), RsFields("LesseeName"), RsFields("PostalCode"), RsFields("Address1"), RsFields("Address2"),
                                                RsFields("GraveNumberKu"), RsFields("GraveNumberkuiki"), RsFields("GraveNumberGawa"), RsFields("GraveNumberBan"),
                                                RsFields("GraveNumberEdaban"), Area, RsFields("ReceiverName"), RsFields("ReceiverPostalCode"),
                                                RsFields("ReceiverAddress1"), RsFields("ReceiverAddress2"))

        Return myLessee

        ADONothing()

    End Function

    ''' <summary>
    ''' 郵便番号を基に検索した住所を返します
    ''' </summary>
    ''' <param name="postalcode"></param>
    Public Function GetAddress(postalcode As String) As String Implements IDataConectRepogitory.GetAddress

        Dim ReferenceCode As String

        ReferenceCode = Replace(postalcode, "-", String.Empty)

        '郵便番号がNothingや空文字の場合は空を返す
        If ReferenceCode Is Nothing Then Return String.Empty
        If ReferenceCode = String.Empty Then Return String.Empty

        Cmd = New ADODB.Command

        With Cmd
            .CommandText = "GetAddress"
            .Parameters.Append(.CreateParameter("postalcode", ADODB.DataTypeEnum.adChar,, 7, ReferenceCode))
        End With

        If ExecuteStoredProc(Cmd) Then Return ""

        Return RsFields("Address")

    End Function

    Public Function GetAddressList(address As String) As ObservableCollection(Of AddressDataEntity) Implements IDataConectRepogitory.GetAddressList

        Dim AddressList As New ObservableCollection(Of AddressDataEntity)
        Dim myAddress As AddressDataEntity

        If address.Trim.Length = 0 Then Return AddressList
        Cmd = New ADODB.Command

        With Cmd
            .CommandText = "GetPostalcode"
            .Parameters.Append(.CreateParameter("address", ADODB.DataTypeEnum.adLongVarChar,, 255, address))
        End With

        ExecuteStoredProc(Cmd)

        Do Until Rs.EOF
            myAddress = New AddressDataEntity(RsFields("Address"), RsFields("PostalCode"))
            AddressList.Add(myAddress)
            Rs.MoveNext()
        Loop

        Return AddressList

    End Function

End Class
