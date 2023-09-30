Imports System.Collections.ObjectModel

''' <summary>
''' データベースと接続するリポジトリ
''' </summary>
Public Interface IDataConectRepogitory

    ''' <summary>
    ''' 名義人データを返します
    ''' </summary>
    ''' <param name="customerid">春秋苑システムの管理番号</param>
    Function GetCustomerInfo(customerid As String) As LesseeCustomerInfoEntity

    ''' <summary>
    ''' 郵便番号を基に住所を返します
    ''' </summary>
    ''' <param name="postalcode">検索する郵便番号</param>
    Function GetPostalCodeList(postalcode As String) As AddressDataListEntity

    ''' <summary>
    ''' 住所をリストで返します
    ''' </summary>
    ''' <param name="address">検索する住所</param>
    ''' <returns></returns>
    Function GetAddressList(address As String) As AddressDataListEntity

    ''' <summary>
    ''' 春秋苑データ最終更新日を返します
    ''' </summary>
    ''' <returns></returns>
    Function GetLastSaveDate() As LastSaveDateEntity

    ''' <summary>
    ''' 区域リストを返します
    ''' </summary>
    ''' <param name="_gravekunumber">基になる区</param>
    ''' <returns></returns>
    Function GetKuikiList(_gravekunumber As String) As KuikiList

    ''' <summary>
    ''' 側リストを返します
    ''' </summary>
    ''' <param name="_gravekunumber">基になる区</param>
    ''' <param name="_gravekuikinumber">基になる区域</param>
    ''' <returns></returns>
    Function GetGawaList(_gravekunumber As String, _gravekuikinumber As String) As GawaList

    ''' <summary>
    ''' 番リストを返します
    ''' </summary>
    ''' <param name="_gravekunumber">基になる区</param>
    ''' <param name="_gravekuikinumber">基になる区域</param>
    ''' <param name="_gravegawanumber">基になる側</param>
    ''' <returns></returns>
    Function GetBanList(_gravekunumber As String, _gravekuikinumber As String, _gravegawanumber As String) As BanList

    ''' <summary>
    ''' 枝番リストを返します
    ''' </summary>
    ''' <param name="_gravekunumber">基になる区</param>
    ''' <param name="_gravekuikinumber">基になる区域</param>
    ''' <param name="_gravegawanumber">基になる側</param>
    ''' <param name="_gravebannumber">基になる番</param>
    ''' <returns></returns>
    Function GetEdabanList(_gravekunumber As String, _gravekuikinumber As String, _gravegawanumber As String, _gravebannumber As String) As EdabanList

    ''' <summary>
    ''' 墓地番号を基に名義人クラスを検索して返します
    ''' </summary>
    ''' <param name="_gravekunumber">検索墓地番号「区」</param>
    ''' <param name="_gravekuikinumber">検索墓地番号「区域」</param>
    ''' <param name="_gravegawanumber">検索墓地番号「側」</param>
    ''' <param name="_gravebannumber">検索墓地番号「番」</param>
    ''' <param name="_graveedabannumber">検索墓地番号「枝番」</param>
    ''' <returns></returns>
    Function GetCustomerInfo_GraveNumber(_gravekunumber As String, _gravekuikinumber As String, _gravegawanumber As String, _gravebannumber As String, _graveedabannumber As String) As LesseeCustomerInfoEntity

    ''' <summary>
    ''' 墓地札を登録します
    ''' </summary>
    ''' <param name="_gravepaneldata"></param>
    Function GravePanelRegistration(_gravepaneldata As GravePanelDataEntity) As Integer
    ''' <summary>
    ''' 受納証を登録します
    ''' </summary>
    ''' <param name="accountActivityDate">受納日</param>
    ''' <param name="addressee">宛名</param>
    ''' <param name="amount">総額</param>
    ''' <param name="cleak">係</param>
    ''' <returns></returns>
    Function VoucherRegistration(accountActivityDate As Date, addressee As String, amount As Integer, cleak As String)
    ''' <summary>
    ''' 墓地札リストを返します
    ''' </summary>
    ''' <returns></returns>
    Function GetGravePanelDataList(customerid As String, fullname As String, registrationdate_st As Date, registrationdate_en As Date, outputdate_st As Date, outputdate_en As Date) As GravePanelDataListEntity

    ''' <summary>
    ''' 墓地札データを削除します
    ''' </summary>
    ''' <param name="_gravepaneldata"></param>
    Sub GravePanelDeletion(_gravepaneldata As GravePanelDataEntity)

    ''' <summary>
    ''' 墓地札データを更新します
    ''' </summary>
    ''' <param name="_gravepaneldata"></param>
    Sub GravePanelUpdate(_gravepaneldata As GravePanelDataEntity)

    ''' <summary>
    ''' データベースの規定の日付を返します
    ''' </summary>
    ''' <returns></returns>
    Function GetDefaultDate() As Date

End Interface
