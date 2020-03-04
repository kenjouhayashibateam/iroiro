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
    Function GetAddressList(ByVal address As String) As AddressesEntity

    Function GetLastSaveDate() As LastSaveDateEntity

    Function GetKuikiList(ByVal _gravekunumber As String) As GraveNumberEntity.KuikiList

    Function GetGawaList(ByVal _gravekunumber As String, ByVal _gravekuikinumber As String) As GraveNumberEntity.GawaList

    Function GetBanList(ByVal _gravekunumber As String, ByVal _gravekuikinumber As String, ByVal _gravegawanumber As String) As GraveNumberEntity.BanList

    Function GetEdabanList(ByVal _gravekunumber As String, ByVal _gravekuikinumber As String, ByVal _gravegawanumber As String, ByVal _gravebannumber As String) As GraveNumberEntity.EdabanList

    Function GetCustomerInfo_GraveNumber(ByVal _gravekunumber As String, ByVal _gravekuikinumber As String, ByVal _gravegawanumber As String, ByVal _gravebannumber As String, ByVal _graveedabannumber As String) As LesseeCustomerInfoEntity

End Interface
