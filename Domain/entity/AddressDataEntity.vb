Imports System.ComponentModel
Imports System.Collections.ObjectModel

''' <summary>
''' 住所クラス
''' </summary>
Public Class AddressDataEntity

    Private _MyAddress As Address
    Private _MyPostalcode As PostalCode
    Private _MyAddresses As AddressesEntity

    ''' <summary>
    ''' 保持する住所
    ''' </summary>
    ''' <returns></returns>
    Public Property MyAddress As Address
        Get
            Return _MyAddress
        End Get
        Set
            _MyAddress = Value
        End Set
    End Property

    ''' <summary>
    ''' 保持する郵便番号
    ''' </summary>
    ''' <returns></returns>
    Public Property MyPostalcode As PostalCode
        Get
            Return _MyPostalcode
        End Get
        Set
            _MyPostalcode = Value
        End Set
    End Property

    Public Property MyAddresses As AddressesEntity
        Get
            Return _MyAddresses
        End Get
        Set
            _MyAddresses = Value
        End Set
    End Property

    Sub New(ByVal _address As String, ByVal _postalcode As String)

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

    Public Function GetAddress() As String
        Return MyAddress.GetAddress
    End Function

    ''' <summary>
    ''' 住所
    ''' </summary>
    Public Class Address

        Private _Address As String

        Public Property Address As String
            Get
                Return _Address
            End Get
            Set
                _Address = Value
            End Set
        End Property

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
    Public Class PostalCode

        Private _Code As String

        Public Property Code As String
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
