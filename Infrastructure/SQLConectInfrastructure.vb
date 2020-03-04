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

    Public Function GetAddress(postalcode As String) As AddressDataEntity Implements IDataConectRepogitory.GetAddress

        Dim ReferenceCode As String

        ReferenceCode = Replace(postalcode, "-", String.Empty)

        '郵便番号がNothingや空文字の場合は空を返す
        If ReferenceCode Is Nothing Then Return Nothing
        If ReferenceCode = String.Empty Then Return Nothing

        Cmd = New ADODB.Command

        With Cmd
            .CommandText = "GetAddress"
            .Parameters.Append(.CreateParameter("postalcode", ADODB.DataTypeEnum.adChar,, 7, ReferenceCode))
        End With

        If ExecuteStoredProc(Cmd) Then Return Nothing

        Return New AddressDataEntity(RsFields("Address"), postalcode)

    End Function

    Public Function GetAddressList(address As String) As AddressesEntity Implements IDataConectRepogitory.GetAddressList

        Dim AddressList As New AddressesEntity

        If address.Trim.Length = 0 Then Return AddressList
        Cmd = New ADODB.Command

        With Cmd
            .CommandText = "GetPostalcode"
            .Parameters.Append(.CreateParameter("address", ADODB.DataTypeEnum.adLongVarChar,, 255, address))
        End With

        ExecuteStoredProc(Cmd)

        Dim myAddress As AddressDataEntity

        Do Until Rs.EOF
            myAddress = New AddressDataEntity(RsFields("Address"), RsFields("PostalCode"))
            AddressList.AddItem(myAddress)
            Rs.MoveNext()
        Loop

        Return AddressList

    End Function

    Private Function GetLastSaveDate() As LastSaveDateEntity Implements IDataConectRepogitory.GetLastSaveDate

        Cmd = New ADODB.Command With {.CommandText = "GetLastSaveDate"}

        ExecuteStoredProc(Cmd)

        Return New LastSaveDateEntity(RsFields("LastSaveDate"))

    End Function

    ''' <summary>
    ''' 墓地番号データをRsに格納します
    ''' </summary>
    ''' <param name="Ku">区</param>
    ''' <param name="Kuiki">区域</param>
    ''' <param name="Gawa">側</param>
    ''' <param name="Ban">番</param>
    ''' <param name="Edaban"></param>
    Private Sub SetGraveData(ByVal Ku As String, Optional Kuiki As String = "%", Optional Gawa As String = "%", Optional Ban As String = "%", Optional Edaban As String = "%")

        Cmd = New ADODB.Command

        With Cmd
            .CommandText = "Reference_GraveNumber"
            .Parameters.Append(.CreateParameter("ku", ADODB.DataTypeEnum.adVarChar, , 10, Ku))
            .Parameters.Append(.CreateParameter("kuiki", ADODB.DataTypeEnum.adVarChar, , 10, Kuiki))
            .Parameters.Append(.CreateParameter("gawa", ADODB.DataTypeEnum.adVarChar, , 10, Gawa))
            .Parameters.Append(.CreateParameter("ban", ADODB.DataTypeEnum.adVarChar, , 10, Ban))
            .Parameters.Append(.CreateParameter("edaban", ADODB.DataTypeEnum.adVarChar, , 10, Edaban))
        End With

        ExecuteStoredProc(Cmd)

    End Sub

    ''' <summary>
    ''' 墓地の区に属する区域のリストを返します
    ''' </summary>
    ''' <param name="_gravekunumber">区</param>
    ''' <returns></returns>
    Public Function GetKuikiList(_gravekunumber As String) As GraveNumberEntity.KuikiList Implements IDataConectRepogitory.GetKuikiList

        SetGraveData(_gravekunumber)
        Dim kf As GraveNumberEntity.Kuiki
        Dim datastring As String = String.Empty
        Dim kl As New ObservableCollection(Of GraveNumberEntity.Kuiki)

        Do Until Rs.EOF
            If InStr(datastring, RsFields("Kuiki")) = 0 Then
                datastring &= RsFields("Kuiki") & " "
                kf = New GraveNumberEntity.Kuiki(RsFields("Kuiki"))
                kl.Add(kf)
            End If
            Rs.MoveNext()
        Loop

        Return New GraveNumberEntity.KuikiList(kl)

    End Function

    Public Function GetGawaList(_gravekunumber As String, _gravekuikinumber As String) As GraveNumberEntity.GawaList Implements IDataConectRepogitory.GetGawaList

        SetGraveData(_gravekunumber, _gravekuikinumber)

        Dim gf As GraveNumberEntity.Gawa
        Dim datastring As String = String.Empty
        Dim gl As New ObservableCollection(Of GraveNumberEntity.Gawa)

        Do Until Rs.EOF
            If InStr(datastring, RsFields("Gawa")) = 0 Then
                datastring &= RsFields("Gawa") & " "
                gf = New GraveNumberEntity.Gawa(RsFields("Gawa"))
                gl.Add(gf)
            End If
            Rs.MoveNext()
        Loop
        Return New GraveNumberEntity.GawaList(gl)

    End Function

    Public Function GetBanList(_gravekunumber As String, _gravekuikinumber As String, _gravegawanumber As String) As GraveNumberEntity.BanList Implements IDataConectRepogitory.GetBanList

        SetGraveData(_gravekunumber, _gravekuikinumber, _gravegawanumber)

        Dim bf As GraveNumberEntity.Ban
        Dim datastring As String = String.Empty
        Dim bl As New ObservableCollection(Of GraveNumberEntity.Ban)

        Do Until Rs.EOF
            If InStr(datastring, RsFields("Ban")) = 0 Then
                datastring &= RsFields("Ban") & " "
                bf = New GraveNumberEntity.Ban(RsFields("Ban"))
                bl.Add(bf)
            End If
            Rs.MoveNext()
        Loop

        Return New GraveNumberEntity.BanList(bl)

    End Function

    Public Function GetEdabanList(_gravekunumber As String, _gravekuikinumber As String, _gravegawanumber As String, _gravebannumber As String) As GraveNumberEntity.EdabanList Implements IDataConectRepogitory.GetEdabanList

        SetGraveData(_gravekunumber, _gravekuikinumber, _gravegawanumber, _gravebannumber)

        Dim ef As GraveNumberEntity.Edaban
        Dim datastring As String = String.Empty
        Dim el As New ObservableCollection(Of GraveNumberEntity.Edaban)

        Do Until Rs.EOF
            If InStr(RsFields("Edaban"), "放棄") > 0 Then
                Rs.MoveNext()
                Continue Do
            End If

            If InStr(datastring, RsFields("Edaban")) = 0 Then
                datastring &= RsFields("Edaban") & " "
                ef = New GraveNumberEntity.Edaban(RsFields("Edaban"))
                el.Add(ef)
            End If
            Rs.MoveNext()
        Loop

        If Trim(datastring) = String.Empty Then Return Nothing
        If Trim(datastring) = "00" Then Return Nothing

        Return New GraveNumberEntity.EdabanList(el)

    End Function

    Public Function GetCustomerInfo_GraveNumber(_gravekunumber As String, _gravekuikinumber As String, _gravegawanumber As String, _gravebannumber As String, _graveedabannumber As String) As LesseeCustomerInfoEntity Implements IDataConectRepogitory.GetCustomerInfo_GraveNumber

        SetGraveData(_gravekunumber, _gravekuikinumber, _gravegawanumber, _gravebannumber, _graveedabannumber)

        Dim customerid As String = RsFields("ManagementNumber")
        Dim lessee As LesseeCustomerInfoEntity = GetCustomerInfo(customerid)

        Return lessee

    End Function

End Class
