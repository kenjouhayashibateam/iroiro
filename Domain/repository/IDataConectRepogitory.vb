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
    Function GetAddress(ByVal postalcode As String) As String

    ''' <summary>
    ''' 住所をリストで返します
    ''' </summary>
    ''' <param name="address">検索する住所</param>
    ''' <returns></returns>
    Function GetAddressList(ByVal address As String) As ObservableCollection(Of AddressDataEntity)

End Interface
