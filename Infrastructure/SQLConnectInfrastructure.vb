Imports Domain
Imports System.Collections.ObjectModel

''' <summary>
''' SQLServerと接続するモデルクラス
''' </summary>
Public Class SQLConnectInfrastructure
    Implements IDataConectRepogitory

    ''' <summary>
    ''' データを取得するためのルートを確立するコマンドクラス
    ''' </summary>
    Private Property Cmd As ADODB.Command

    ''' <summary>
    ''' ログファイルを生成します
    ''' </summary>
    ''' <returns></returns>
    Private Property LogFileConecter As ILoggerRepogitory

    Private Const DEFAULTDATE As Date = #1900/01/01#

    ''' <summary>
    ''' SQLServerに接続するための接続文字列
    ''' </summary>
    Private Const SHUNJUENCONSTRING As String = "PROVIDER=SQLOLEDB;SERVER=192.168.44.163\SQLEXPRESS2014;DATABASE=COMMON;user id=sa;password=sqlserver"
    Private Const HAYASHIBACONSTRING As String = "PROVIDER=SQLOLEDB;SERVER=DESKTOP-MUJVB5O\SQLEXPRESS;DATABASE=COMMON;user id=sa;password=sqlserver"

    Private ReadOnly MyConnectionString As String = SHUNJUENCONSTRING

    ''' <summary>
    ''' コマンドから取得したデータを格納するクラス
    ''' </summary>
    Private Property Rs As ADODB.Recordset

    ''' <summary>
    ''' VB.NETとSQLServerを接続するクラス
    ''' </summary>
    Private Property Cn As ADODB.Connection

    Sub New()
        Me.New(New LogFileInfrastructure)
    End Sub

    Sub New(ByVal _logfile As ILoggerRepogitory)
        LogFileConecter = _logfile
    End Sub

    ''' <summary>
    ''' Rsにデータを格納します
    ''' </summary>
    ''' <param name="exeCmd">使用するストアドプロシージャ等のデータを格納したコマンド</param>
    Private Sub ExecuteStoredProcSetRecord(ByRef exeCmd As ADODB.Command)

        exeCmd = GetCompleteCmd(exeCmd)
        Try
            Rs = exeCmd.Execute
        Catch ex As Exception
            LogFileConecter.Log(ILoggerRepogitory.LogInfo.ERR, ex.StackTrace)
        End Try

    End Sub

    ''' <summary>
    ''' Cmdのプロパティを完成させて返します
    ''' </summary>
    ''' <param name="execmd"></param>
    ''' <returns></returns>
    Private Function GetCompleteCmd(ByVal execmd As ADODB.Command) As ADODB.Command

        Cn = New ADODB.Connection With {.ConnectionString = MyConnectionString}
        Cn.Open()

        With execmd
            .ActiveConnection = Cn
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
        End With

        Return execmd

    End Function

    '''' <summary>
    '''' 戻り値のないストアドプロシージャを実行します
    '''' </summary>
    '''' <param name="execmd">使用するストアドプロシージャ等のデータを格納したコマンド</param>
    'Private Function ExecuteStoredProc(ByRef execmd As ADODB.Command) As Boolean

    '    execmd = GetCompleteCmd(execmd)
    '    Try
    '        execmd.Execute()
    '    Catch ex As Exception
    '        LogFileConecter.Log(ILoggerRepogitory.LogInfo.ERR, ex.StackTrace)
    '        Return False
    '    End Try
    '    Return True

    'End Function

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
    ''' 名義人データを検索します
    ''' </summary>
    ''' <param name="strManagementNumber">検索する管理番号</param>
    Private Sub SetLesseeRecord(ByVal strManagementNumber As String)

        Cmd = New ADODB.Command

        With Cmd
            .CommandText = My.Resources.StoredProc_Call_ShunjyuenData
            .Parameters.Append(.CreateParameter("managementnumber", ADODB.DataTypeEnum.adChar,, 6, strManagementNumber))
            .Parameters.Append(.CreateParameter("lesseename", ADODB.DataTypeEnum.adVarChar,, 100, "%"))
        End With

        ExecuteStoredProcSetRecord(Cmd)

    End Sub

    ''' <summary>
    ''' レコードセットのフィールドのValueを文字列形式で返します
    ''' </summary>
    ''' <param name="FieldName">データベース（ストアドプロシージャ）から取得するフィールドの名前</param>
    Private Function RsFields(ByVal FieldName As String) As String
        If Rs.EOF Then Return String.Empty
        Return IIf(IsDBNull(Rs.Fields(FieldName).Value), String.Empty, Rs.Fields(FieldName).Value)
    End Function

    ''' <summary>
    ''' 名義人クラスを返します
    ''' </summary>
    ''' <param name="customerid">検索する管理番号</param>
    ''' <returns></returns>
    Public Function GetCustomerInfo(customerid As String) As LesseeCustomerInfoEntity Implements IDataConectRepogitory.GetCustomerInfo

        Dim myLessee As LesseeCustomerInfoEntity
        Dim Area As Double

        If customerid = String.Empty Then Return Nothing

        SetLesseeRecord(customerid)

        If RsFields("LesseeName") = String.Empty Then Return Nothing

        '御廟は面積がない上にDouble型なので、0にして対応する
        Dim areaString As String = RsFields("AreaOfGrave")
        If String.IsNullOrEmpty(areaString) Then
            Area = 0
        Else
            Area = areaString
        End If

        myLessee = New LesseeCustomerInfoEntity(RsFields(My.Resources.ManagementNumber), RsFields(My.Resources.LesseeName), RsFields("PostalCode"),
                                                RsFields("Address1"), RsFields("Address2"), RsFields($"{My.Resources.GraveNumber}Ku"),
                                                RsFields($"{My.Resources.GraveNumber}Kuiki"), RsFields($"{My.Resources.GraveNumber}Gawa"),
                                                RsFields($"{My.Resources.GraveNumber}Ban"), RsFields($"{My.Resources.GraveNumber}Edaban"), Area,
                                                RsFields("ReceiverName"), RsFields("ReceiverPostalCode"), RsFields("ReceiverAddress1"),
                                                RsFields("ReceiverAddress2"))

        Return myLessee

        ADONothing()

    End Function

    ''' <summary>
    ''' 住所を検索します
    ''' </summary>
    ''' <param name="postalcode">検索する郵便番号</param>
    ''' <returns></returns>
    Public Function GetAddress(postalcode As String) As AddressDataEntity Implements IDataConectRepogitory.GetAddress

        Dim ReferenceCode As String

        ReferenceCode = Replace(postalcode, My.Resources.StringHalfHyphen, String.Empty)

        '郵便番号がNothing等のエラーの場合は空を返す
        If Not ReferenceCode.Length = 7 Then Return Nothing
        If ReferenceCode Is Nothing Then Return Nothing
        If ReferenceCode = String.Empty Then Return Nothing

        Cmd = New ADODB.Command

        With Cmd
            .CommandText = My.Resources.StoredProc_GetAddress
            .Parameters.Append(.CreateParameter("postalcode", ADODB.DataTypeEnum.adChar,, 7, ReferenceCode))
        End With

        ExecuteStoredProcSetRecord(Cmd)

        Return New AddressDataEntity(RsFields("Address"), postalcode)

    End Function

    ''' <summary>
    ''' 住所欄の文字列を使って住所検索し、該当する住所をリストにして返します
    ''' </summary>
    ''' <param name="address">検索する住所</param>
    ''' <returns></returns>
    Public Function GetAddressList(address As String) As AddressDataListEntity Implements IDataConectRepogitory.GetAddressList

        Dim AddressList As New AddressDataListEntity

        If String.IsNullOrEmpty(address) Then Return AddressList
        Cmd = New ADODB.Command

        With Cmd
            .CommandText = My.Resources.StoredProc_GetPostalcode
            .Parameters.Append(.CreateParameter("address", ADODB.DataTypeEnum.adLongVarChar,, 255, address))
        End With

        ExecuteStoredProcSetRecord(Cmd)

        Dim myAddress As AddressDataEntity

        Do Until Rs.EOF
            myAddress = New AddressDataEntity(RsFields("Address"), RsFields("PostalCode"))
            AddressList.AddItem(myAddress)
            Rs.MoveNext()
        Loop

        Return AddressList

    End Function

    ''' <summary>
    ''' 春秋苑システムデータの最終更新日を取得します
    ''' </summary>
    ''' <returns></returns>
    Private Function GetLastSaveDate() As LastSaveDateEntity Implements IDataConectRepogitory.GetLastSaveDate

        Cmd = New ADODB.Command With {.CommandText = My.Resources.StoredProc_GetLastSaveDate}

        ExecuteStoredProcSetRecord(Cmd)

        Return New LastSaveDateEntity(RsFields(My.Resources.LastSaveDate))

    End Function

    ''' <summary>
    ''' 墓地番号データをRsに格納します
    ''' </summary>
    ''' <param name="Ku">区</param>
    ''' <param name="Kuiki">区域</param>
    ''' <param name="Gawa">側</param>
    ''' <param name="Ban">番</param>
    ''' <param name="Edaban">枝番</param>
    Private Sub SetGraveData(ByVal Ku As String, Optional Kuiki As String = "%", Optional Gawa As String = "%", Optional Ban As String = "%", Optional Edaban As String = "%")

        Cmd = New ADODB.Command

        With Cmd
            .CommandText = My.Resources.StoredProc_Reference_GraveNumber
            .Parameters.Append(.CreateParameter("ku", ADODB.DataTypeEnum.adVarChar, , 10, Ku))
            .Parameters.Append(.CreateParameter("kuiki", ADODB.DataTypeEnum.adVarChar, , 10, Kuiki))
            .Parameters.Append(.CreateParameter("gawa", ADODB.DataTypeEnum.adVarChar, , 10, Gawa))
            .Parameters.Append(.CreateParameter("ban", ADODB.DataTypeEnum.adVarChar, , 10, Ban))
            .Parameters.Append(.CreateParameter("edaban", ADODB.DataTypeEnum.adVarChar, , 10, Edaban))
        End With

        ExecuteStoredProcSetRecord(Cmd)

    End Sub

    ''' <summary>
    ''' 墓地の区に属する区域のリストを返します
    ''' </summary>
    ''' <param name="_gravekunumber">区</param>
    ''' <returns></returns>
    Public Function GetKuikiList(_gravekunumber As String) As KuikiList Implements IDataConectRepogitory.GetKuikiList

        SetGraveData(_gravekunumber)
        Dim kf As Kuiki
        Dim datastring As String = String.Empty
        Dim kl As New ObservableCollection(Of Kuiki)
        Dim kuikiString As String
        Do Until Rs.EOF
            kuikiString = RsFields("Kuiki")
            If InStr(datastring, kuikiString) = 0 Then
                datastring &= $"{kuikiString}{Space(1)}"
                kf = New Kuiki(kuikiString)
                kl.Add(kf)
            End If
            Rs.MoveNext()
        Loop

        Return New KuikiList(kl)

    End Function
    ''' <summary>
    ''' 墓地の区、区域に属する側のリストを返します
    ''' </summary>
    ''' <param name="_gravekunumber">区</param>
    ''' <param name="_gravekuikinumber">区域</param>
    ''' <returns></returns>
    Public Function GetGawaList(_gravekunumber As String, _gravekuikinumber As String) As GawaList Implements IDataConectRepogitory.GetGawaList

        SetGraveData(_gravekunumber, _gravekuikinumber)

        Dim gf As Gawa
        Dim datastring As String = String.Empty
        Dim gl As New ObservableCollection(Of Gawa)

        Do Until Rs.EOF
            If InStr(datastring, RsFields(My.Resources.Gawa)) <> 0 Then
                Rs.MoveNext()
                Continue Do
            End If
            If RsFields(My.Resources.Gawa) = "0" Then
                Rs.MoveNext()
                Continue Do
            End If
            datastring &= $"{RsFields(My.Resources.Gawa)}{Space(1)}"
            gf = New Gawa(RsFields(My.Resources.Gawa))
            gl.Add(gf)
            Rs.MoveNext()
        Loop
        Return New GawaList(gl)

    End Function

    ''' <summary>
    ''' 墓地の側に属する番のリストを返します
    ''' </summary>
    ''' <param name="_gravekunumber">区</param>
    ''' <param name="_gravekuikinumber">区域</param>
    ''' <param name="_gravegawanumber">側</param>
    ''' <returns></returns>
    Public Function GetBanList(_gravekunumber As String, _gravekuikinumber As String, _gravegawanumber As String) As BanList Implements IDataConectRepogitory.GetBanList

        SetGraveData(_gravekunumber, _gravekuikinumber, _gravegawanumber)

        Dim bf As Ban
        Dim datastring As String = String.Empty
        Dim bl As New ObservableCollection(Of Ban)
        Dim BanString As String

        Do Until Rs.EOF
            BanString = RsFields("Ban")
            If InStr(datastring, BanString) = 0 Then
                datastring &= $"{BanString}{Space(1)}"
                bf = New Ban(BanString)
                bl.Add(bf)
            End If
            Rs.MoveNext()
        Loop

        Return New BanList(bl)

    End Function

    ''' <summary>
    ''' 墓地の番に属する枝番のリストを返します
    ''' </summary>
    ''' <param name="_gravekunumber">区</param>
    ''' <param name="_gravekuikinumber">区域</param>
    ''' <param name="_gravegawanumber">側</param>
    ''' <param name="_gravebannumber">番</param>
    ''' <returns></returns>
    Public Function GetEdabanList(_gravekunumber As String, _gravekuikinumber As String, _gravegawanumber As String, _gravebannumber As String) As EdabanList Implements IDataConectRepogitory.GetEdabanList

        SetGraveData(_gravekunumber, _gravekuikinumber, _gravegawanumber, _gravebannumber)

        Dim ef As Edaban
        Dim datastring As String = String.Empty
        Dim el As New ObservableCollection(Of Edaban)
        Dim edabanString As String

        Do Until Rs.EOF
            edabanString = RsFields("Edaban")
            If InStr(edabanString, My.Resources.DiscardString) > 0 Then
                Rs.MoveNext()
                Continue Do
            End If

            If InStr(datastring, edabanString) = 0 Then
                datastring &= $"{edabanString}{Space(1)}"
                ef = New Edaban(edabanString)
                el.Add(ef)
            End If
            Rs.MoveNext()
        Loop

        If Trim(datastring) = String.Empty Then Return Nothing
        If Trim(datastring) = "00" Then Return Nothing

        Return New EdabanList(el)

    End Function

    ''' <summary>
    ''' 墓地番号から、名義人情報を取得します
    ''' </summary>
    ''' <param name="_gravekunumber">区</param>
    ''' <param name="_gravekuikinumber">区域</param>
    ''' <param name="_gravegawanumber">側</param>
    ''' <param name="_gravebannumber">番</param>
    ''' <param name="_graveedabannumber">枝番</param>
    ''' <returns></returns>
    Public Function GetCustomerInfo_GraveNumber(_gravekunumber As String, _gravekuikinumber As String, _gravegawanumber As String, _gravebannumber As String, _graveedabannumber As String) As LesseeCustomerInfoEntity Implements IDataConectRepogitory.GetCustomerInfo_GraveNumber

        SetGraveData(_gravekunumber, _gravekuikinumber, _gravegawanumber, _gravebannumber, _graveedabannumber)

        Dim customerid As String = RsFields(My.Resources.ManagementNumber)
        Dim lessee As LesseeCustomerInfoEntity = GetCustomerInfo(customerid)

        Return lessee

    End Function

    ''' <summary>
    ''' 墓地札データ登録
    ''' </summary>
    ''' <param name="_gravepaneldata">登録墓地札データ</param>
    Public Function GravePanelRegistration(_gravepaneldata As GravePanelDataEntity) As Integer Implements IDataConectRepogitory.GravePanelRegistration

        Cmd = New ADODB.Command

        With Cmd
            .CommandText = My.Resources.StoredProc_RegistrationGravePanel
            .Parameters.Append(.CreateParameter("customerid", ADODB.DataTypeEnum.adChar,, 6, _gravepaneldata.GetCustomerID.ID))
            .Parameters.Append(.CreateParameter("familyname", ADODB.DataTypeEnum.adVarChar,, 50, _gravepaneldata.GetFamilyName.Name))
            .Parameters.Append(.CreateParameter("fullname", ADODB.DataTypeEnum.adVarChar,, 50, _gravepaneldata.GetFullName.Name))
            .Parameters.Append(.CreateParameter("gravenumber", ADODB.DataTypeEnum.adVarChar,, 50, _gravepaneldata.GetGraveNumber.Number))
            .Parameters.Append(.CreateParameter("area", ADODB.DataTypeEnum.adDouble,,, _gravepaneldata.GetArea.AreaValue))
            .Parameters.Append(.CreateParameter("contractdetail", ADODB.DataTypeEnum.adVarChar,, 50, _gravepaneldata.GetContractContent.Content))
            .Parameters.Append(.CreateParameter("registrationtime", ADODB.DataTypeEnum.adDate,,, _gravepaneldata.GetRegistrationTime.MyDate))
            .Parameters.Append(.CreateParameter("purintouttime", ADODB.DataTypeEnum.adDate,,, _gravepaneldata.GetPrintoutTime.MyDate))
        End With

        ExecuteStoredProcSetRecord(Cmd)

        Return RsFields("ID")
    End Function

    ''' <summary>
    ''' 墓地札データの一覧を取得し、リストで返します
    ''' </summary>
    ''' <param name="customerid">管理番号</param>
    ''' <param name="fullname">申込者名</param>
    ''' <param name="registrationdate_st">登録日の始め</param>
    ''' <param name="registrationdate_en">登録日</param>
    ''' <param name="outputdate_st"></param>
    ''' <param name="outputdate_en"></param>
    ''' <returns></returns>
    Public Function GetGravePanelDataList(customerid As String, fullname As String, registrationdate_st As Date, registrationdate_en As Date, outputdate_st As Date, outputdate_en As Date) As GravePanelDataListEntity Implements IDataConectRepogitory.GetGravePanelDataList

        Dim refid As String = customerid
        Dim refname As String = fullname
        Dim refRegistrationstdate As Date = registrationdate_st
        Dim refRegistrationendate As Date = registrationdate_en
        Dim refOutputStDate As Date = outputdate_st
        Dim refOutputEnDate As Date = outputdate_en

        Cmd = New ADODB.Command

        With Cmd
            .CommandText = My.Resources.StoredProc_GetGravePanelList
            .Parameters.Append(.CreateParameter("CustomerID", ADODB.DataTypeEnum.adChar,, 6, refid))
            .Parameters.Append(.CreateParameter("FullName", ADODB.DataTypeEnum.adVarChar,, 50, refname))
            .Parameters.Append(.CreateParameter("RegistrationTime_st", ADODB.DataTypeEnum.adDate,,, refRegistrationstdate))
            .Parameters.Append(.CreateParameter("RegistrationTime_en", ADODB.DataTypeEnum.adDate,,, refRegistrationendate))
            .Parameters.Append(.CreateParameter("OutputTime_st", ADODB.DataTypeEnum.adDate,,, refOutputStDate))
            .Parameters.Append(.CreateParameter("OutputTime_en", ADODB.DataTypeEnum.adDate,,, refOutputEnDate))
        End With

        ExecuteStoredProcSetRecord(Cmd)

        Dim gpd As GravePanelDataEntity
        Dim gpdlist As New GravePanelDataListEntity
        Do Until Rs.EOF
            gpd = New GravePanelDataEntity(RsFields("OrderID"), RsFields("CustomerID"), RsFields("FamilyName"), RsFields("FullName"),
                                           RsFields(My.Resources.GraveNumber), RsFields("Area"), RsFields("ContractDetail"), RsFields("RegistrationTime"),
                                           RsFields("OutputTime"))
            gpdlist.AddItem(gpd)
            Rs.MoveNext()
        Loop

        Return gpdlist

    End Function

    ''' <summary>
    ''' 墓地札データを削除します
    ''' </summary>
    ''' <param name="_gravepaneldata"></param>
    Public Sub GravePanelDeletion(_gravepaneldata As GravePanelDataEntity) Implements IDataConectRepogitory.GravePanelDeletion

        Cmd = New ADODB.Command

        With Cmd
            .CommandText = My.Resources.StoredProc_DeleteGravePanel
            .Parameters.Append(.CreateParameter("orderid", ADODB.DataTypeEnum.adChar,, 6, _gravepaneldata.GetID.ID))
        End With

        ExecuteStoredProcSetRecord(Cmd)

    End Sub

    ''' <summary>
    ''' 墓地札データを更新します
    ''' </summary>
    ''' <param name="_gravepaneldata"></param>
    Public Sub GravePanelUpdate(_gravepaneldata As GravePanelDataEntity) Implements IDataConectRepogitory.GravePanelUpdate

        Cmd = New ADODB.Command

        With Cmd
            .CommandText = My.Resources.StoredProc_UpdateGravePanel
            .Parameters.Append(.CreateParameter("orderid", ADODB.DataTypeEnum.adChar,, 6, _gravepaneldata.GetID.ID))
            .Parameters.Append(.CreateParameter("customerid", ADODB.DataTypeEnum.adChar,, 6, _gravepaneldata.GetCustomerID.ID))
            .Parameters.Append(.CreateParameter("familyname", ADODB.DataTypeEnum.adVarChar,, 50, _gravepaneldata.GetFamilyName.Name))
            .Parameters.Append(.CreateParameter("fullname", ADODB.DataTypeEnum.adVarChar,, 50, _gravepaneldata.GetFullName.Name))
            .Parameters.Append(.CreateParameter("gravenumber", ADODB.DataTypeEnum.adVarChar,, 50, _gravepaneldata.GetGraveNumber.Number))
            .Parameters.Append(.CreateParameter("area", ADODB.DataTypeEnum.adDouble,,, _gravepaneldata.GetArea.AreaValue))
            .Parameters.Append(.CreateParameter("contractdetail", ADODB.DataTypeEnum.adVarChar,, 50, _gravepaneldata.GetContractContent.Content))
            .Parameters.Append(.CreateParameter("registrationtime", ADODB.DataTypeEnum.adDate,,, _gravepaneldata.GetRegistrationTime.MyDate))
            .Parameters.Append(.CreateParameter("outputtime", ADODB.DataTypeEnum.adDate,,, _gravepaneldata.GetPrintoutTime.MyDate))
        End With

        ExecuteStoredProcSetRecord(Cmd)

    End Sub

    Public Function GetDefaultDate() As Date Implements IDataConectRepogitory.GetDefaultDate
        Return DEFAULTDATE
    End Function
End Class
