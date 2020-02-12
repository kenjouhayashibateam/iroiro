''' <summary>
''' 住所クラス
''' </summary>
Public Class AddressDataEntity

    ''' <summary>
    ''' 保持する住所
    ''' </summary>
    ''' <returns></returns>
    Private Property MyAddress As Address

    ''' <summary>
    ''' 保持する郵便番号
    ''' </summary>
    ''' <returns></returns>
    Private Property MyPostalcode As PostalCode

    Sub New(ByVal _address As String, ByVal _postalcode As String)

        MyAddress = New Address(_address)
        MyPostalcode = New PostalCode(_postalcode)

    End Sub

    ''' <summary>
    ''' 住所を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetAddress() As String
        Return MyAddress.GetAddress
    End Function

    ''' <summary>
    ''' 郵便番号を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetPostalCode() As String
        Return MyPostalcode.GetCode
    End Function

    ''' <summary>
    ''' 住所
    ''' </summary>
    Private Class Address
        Private Property Address As String

        Sub New(ByVal myAddress1 As String)
            Address = myAddress1
        End Sub

        Friend Function GetAddress() As String
            Return Address
        End Function
    End Class

    ''' <summary>
    ''' 郵便番号
    ''' </summary>
    Private Class PostalCode

        Private _Code As String

        Private Property Code As String
            Get
                Return _Code
            End Get
            Set
                _Code = Value
            End Set
        End Property

        Sub New(ByVal myPostalCode As String)
            Code = myPostalCode
        End Sub

        Friend Function GetCode() As String
            Return Code
        End Function

    End Class
End Class
