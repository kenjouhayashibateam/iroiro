''' <summary>
''' 住所クラス
''' </summary>
Public Class AddressDataEntity

    ''' <summary>
    ''' 保持する住所
    ''' </summary>
    ''' <returns></returns>
    Public Property MyAddress As Address

    ''' <summary>
    ''' 保持する郵便番号
    ''' </summary>
    ''' <returns></returns>
    Public Property MyPostalcode As PostalCode

    ''' <summary>
    ''' 住所データリスト
    ''' </summary>
    ''' <returns></returns>
    Public Property MyAddresses As AddressDataListEntity

    Public Sub New(_address As String, _postalcode As String)

        MyAddress = New Address(_address)
        MyPostalcode = New PostalCode(_postalcode)

    End Sub

    ''' <summary>
    ''' 郵便番号を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetPostalCode() As String
        Return MyPostalcode.GetCode
    End Function

    ''' <summary>
    ''' 住所を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetAddress() As String
        Return MyAddress.GetAddress
    End Function

    ''' <summary>
    ''' 住所
    ''' </summary>
    Public Class Address

        Public Property Address As String

        Public Sub New(myAddress1 As String)
            Address = myAddress1
        End Sub

        Friend Function GetAddress() As String
            Return Address
        End Function
    End Class

    ''' <summary>
    ''' 郵便番号
    ''' </summary>
    Public Class PostalCode

        Public Property Code As String

        Public Sub New(myPostalCode As String)
            Code = myPostalCode
        End Sub

        Friend Function GetCode() As String
            Return $"{Code.Substring(0, 3)}-{Code.Substring(3, 4)}"
        End Function

    End Class
End Class
