''' <summary>
''' エクセルに出力する宛名等を格納するクラス
''' </summary>
Public Class DestinationDataEntity

    ''' <summary>
    ''' 宛名
    ''' </summary>
    Public Property AddresseeName As Name
    ''' <summary>
    ''' 敬称
    ''' </summary>
    Public Property MyTitle As Title
    ''' <summary>
    ''' 郵便番号
    ''' </summary>
    Public Property MyPostalCode As PostalCode
    ''' <summary>
    ''' 住所1
    ''' </summary>
    Public Property MyAddress1 As Address1
    ''' <summary>
    ''' 住所2
    ''' </summary>
    Public Property MyAddress2 As Address2
    ''' <summary>
    ''' 備考1
    ''' </summary>
    Public Property Note1Data As Note1
    ''' <summary>
    ''' 備考2
    ''' </summary>
    Public Property Note2Data As Note2
    ''' <summary>
    ''' 備考3
    ''' </summary>
    Public Property Note3Data As Note3
    ''' <summary>
    ''' 備考4
    ''' </summary>
    Public Property Note4Data As Note4
    ''' <summary>
    ''' 備考5
    ''' </summary>
    Public Property Note5Data As Note5
    ''' <summary>
    ''' 金額
    ''' </summary>
    Public Property MoneyData As Money
    ''' <summary>
    ''' 管理番号
    ''' </summary>
    ''' <returns></returns>
    Public Property MyCustomerID As CustomerID

    '''<param name="_customerid">管理番号</param>
    ''' <param name="_addresseename">宛名</param>
    ''' <param name="_title">敬称</param>
    ''' <param name="_postalcode">郵便番号</param>
    ''' <param name="_address1">住所1</param>
    ''' <param name="_address2">住所2</param>
    ''' <param name="_money">金額</param>
    ''' <param name="_note1">備考1</param>
    ''' <param name="_note2">備考2</param>
    ''' <param name="_note3">備考3</param>
    ''' <param name="_note4">備考4</param>
    ''' <param name="_note5">備考5</param>
    Sub New(ByVal _customerid As String, ByVal _addresseename As String, ByVal _title As String, ByVal _postalcode As String, ByVal _address1 As String, _address2 As String,
                ByVal _money As String, ByVal _note1 As String, ByVal _note2 As String, ByVal _note3 As String, ByVal _note4 As String, ByVal _note5 As String)

        MyCustomerID = New CustomerID(_customerid)
        AddresseeName = New Name(_addresseename)
        MyTitle = New Title(_title)
        MyPostalCode = New PostalCode(_postalcode)
        MyAddress1 = New Address1(_address1)
        MyAddress2 = New Address2(_address2)
        Note1Data = New Note1(_note1)
        Note2Data = New Note2(_note2)
        Note3Data = New Note3(_note3)
        Note4Data = New Note4(_note4)
        Note5Data = New Note5(_note5)
        MoneyData = New Money(_money)
    End Sub

    '''<param name="_customerid">管理番号</param>
    ''' <param name="_addresseename">宛名</param>
    ''' <param name="_title">敬称</param>
    ''' <param name="_postalcode">郵便番号</param>
    ''' <param name="_address1">住所1</param>
    ''' <param name="_address2">住所2</param>
    Sub New(ByVal _customerid As String, ByVal _addresseename As String, ByVal _title As String, ByVal _postalcode As String, ByVal _address1 As String, _address2 As String)

        MyCustomerID = New CustomerID(_customerid)
        AddresseeName = New Name(_addresseename)
        MyTitle = New Title(_title)
        MyPostalCode = New PostalCode(_postalcode)
        MyAddress1 = New Address1(_address1)
        MyAddress2 = New Address2(_address2)
    End Sub

    ''' <summary>
    ''' 管理番号
    ''' </summary>
    Public Class CustomerID
        Private _ID As String

        Public Property ID As String
            Get
                Return _ID
            End Get
            Set
                _ID = Value
            End Set
        End Property

        Sub New(ByVal _customerid As String)
            ID = _customerid
        End Sub
        Function GetID() As String
            Return ID
        End Function
    End Class
    ''' <summary>
    ''' 名前
    ''' </summary>
    Public Class Name
        Private _MyName As String

        Public Property MyName As String
            Get
                Return _MyName
            End Get
            Set
                _MyName = Value
            End Set
        End Property

        Sub New(ByVal _name As String)
            MyName = _name
        End Sub

        Public Function GetName() As String
            Return MyName
        End Function

    End Class
    ''' <summary>
    ''' 敬称
    ''' </summary>
    Public Class Title
        Private _TitleString As String

        Public Property TitleString As String
            Get
                Return _TitleString
            End Get
            Set
                _TitleString = Value
            End Set
        End Property

        Sub New(ByVal _title As String)
            TitleString = _title
        End Sub

        Public Function GetTitle() As String
            Return TitleString
        End Function
    End Class
    ''' <summary>
    ''' 郵便番号
    ''' </summary>
    Public Class PostalCode
        Public Property Code As String

        Sub New(ByVal _code As String)
            Code = _code
        End Sub

        Public Function GetCode() As String
            Return Code
        End Function
    End Class
    ''' <summary>
    ''' 住所1
    ''' </summary>
    Public Class Address1
        Public Property Address As String

        Sub New(ByVal _address1 As String)
            Address = _address1
        End Sub

        Public Function GetAddress() As String
            Return Address
        End Function
    End Class
    ''' <summary>
    ''' 住所2
    ''' </summary>
    Public Class Address2
        Public Property Address As String

        Sub New(ByVal _address2 As String)
            Address = _address2
        End Sub

        Public Function GetAddress() As String
            Return Address
        End Function
    End Class
    ''' <summary>
    ''' 備考1
    ''' </summary>
    Public Class Note1
        Public Property Value As String

        Sub New(ByVal _note1 As String)
            Value = _note1
        End Sub

        Public Function GetNote() As String
            Return Value
        End Function
    End Class
    ''' <summary>
    ''' 備考2
    ''' </summary>
    Public Class Note2
        Public Property Value As String

        Sub New(ByVal _note2 As String)
            Value = _note2
        End Sub

        Public Function GetNote() As String
            Return Value
        End Function
    End Class
    ''' <summary>
    ''' 備考3
    ''' </summary>
    Public Class Note3
        Public Property Value As String

        Sub New(ByVal _note3 As String)
            Value = _note3
        End Sub

        Public Function GetNote() As String
            Return Value
        End Function
    End Class
    ''' <summary>
    ''' 備考4
    ''' </summary>
    Public Class Note4
        Public Property Value As String

        Sub New(ByVal _note4 As String)
            Value = _note4
        End Sub

        Public Function GetNote() As String
            Return Value
        End Function
    End Class
    ''' <summary>
    ''' 備考5
    ''' </summary>
    Public Class Note5
        Public Property Value As String

        Sub New(ByVal _note5 As String)
            Value = _note5
        End Sub

        Public Function GetNote() As String
            Return Value
        End Function
    End Class
    ''' <summary>
    ''' 金額
    ''' </summary>
    Public Class Money
        Public Property Price As String

        Sub New(ByVal _money As String)
            Price = _money
        End Sub

        Public Function GetMoney() As String
            Return Price
        End Function
    End Class
End Class
