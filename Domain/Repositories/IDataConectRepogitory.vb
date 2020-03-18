Imports System.Collections.ObjectModel

''' <summary>
''' データベースと接続するリポジトリ
''' </summary>
Public Interface IDataConectRepogitory

    ''' <summary>
    ''' 名義人データを返します
    ''' </summary>
    ''' <param name="customerid">春秋苑システムの管理番号</param>
    Function GetCustomerInfo(ByVal customerid As String) As LesseeCustomerInfoEntity

    ''' <summary>
    ''' 郵便番号を基に住所を返します
    ''' </summary>
    ''' <param name="postalcode">検索する郵便番号</param>
    Function GetAddress(ByVal postalcode As String) As AddressDataEntity

    ''' <summary>
    ''' 住所をリストで返します
    ''' </summary>
    ''' <param name="address">検索する住所</param>
    ''' <returns></returns>
    Function GetAddressList(ByVal address As String) As AddressDataListEntity

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
    Function GetKuikiList(ByVal _gravekunumber As String) As GraveNumberEntity.KuikiList

    ''' <summary>
    ''' 側リストを返します
    ''' </summary>
    ''' <param name="_gravekunumber">基になる区</param>
    ''' <param name="_gravekuikinumber">基になる区域</param>
    ''' <returns></returns>
    Function GetGawaList(ByVal _gravekunumber As String, ByVal _gravekuikinumber As String) As GraveNumberEntity.GawaList

    ''' <summary>
    ''' 番リストを返します
    ''' </summary>
    ''' <param name="_gravekunumber">基になる区</param>
    ''' <param name="_gravekuikinumber">基になる区域</param>
    ''' <param name="_gravegawanumber">基になる側</param>
    ''' <returns></returns>
    Function GetBanList(ByVal _gravekunumber As String, ByVal _gravekuikinumber As String, ByVal _gravegawanumber As String) As GraveNumberEntity.BanList

    ''' <summary>
    ''' 枝番リストを返します
    ''' </summary>
    ''' <param name="_gravekunumber">基になる区</param>
    ''' <param name="_gravekuikinumber">基になる区域</param>
    ''' <param name="_gravegawanumber">基になる側</param>
    ''' <param name="_gravebannumber">基になる番</param>
    ''' <returns></returns>
    Function GetEdabanList(ByVal _gravekunumber As String, ByVal _gravekuikinumber As String, ByVal _gravegawanumber As String, ByVal _gravebannumber As String) As GraveNumberEntity.EdabanList

    ''' <summary>
    ''' 墓地番号を基に名義人クラスを検索して返します
    ''' </summary>
    ''' <param name="_gravekunumber">検索墓地番号「区」</param>
    ''' <param name="_gravekuikinumber">検索墓地番号「区域」</param>
    ''' <param name="_gravegawanumber">検索墓地番号「側」</param>
    ''' <param name="_gravebannumber">検索墓地番号「番」</param>
    ''' <param name="_graveedabannumber">検索墓地番号「枝番」</param>
    ''' <returns></returns>
    Function GetCustomerInfo_GraveNumber(ByVal _gravekunumber As String, ByVal _gravekuikinumber As String, ByVal _gravegawanumber As String, ByVal _gravebannumber As String, ByVal _graveedabannumber As String) As LesseeCustomerInfoEntity

    ''' <summary>
    ''' 墓地札を登録します
    ''' </summary>
    ''' <param name="_gravepaneldata"></param>
    Sub GravePanelRegistration(ByVal _gravepaneldata As GravePanelDataEntity)

    ''' <summary>
    ''' 墓地札リストを返します
    ''' </summary>
    ''' <returns></returns>
    Function GetGravePanelDataList() As GravePanelDataListEntity

    ''' <summary>
    ''' 墓地札データを削除します
    ''' </summary>
    ''' <param name="_graveoaneldata"></param>
    Sub GravePanelDeletion(ByVal _graveoaneldata As GravePanelDataEntity)


End Interface
