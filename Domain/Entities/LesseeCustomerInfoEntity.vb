
''' <summary>
''' 名義人データ格納クラス
''' </summary>
Public Class LesseeCustomerInfoEntity

    ''' <summary>
    ''' 名義人
    ''' </summary>
    Private ReadOnly myLesseName As LesseeName

    ''' <summary>
    ''' 住所（郵便番号で表される部分）
    ''' </summary>
    Private ReadOnly myAddress1 As Address1

    ''' <summary>
    ''' 住所（郵便番号で表さない番地等）
    ''' </summary>
    Private ReadOnly myAddress2 As Address2

    ''' <summary>
    ''' 郵便番号
    ''' </summary>
    Private ReadOnly myPostalCode As PostalCode

    ''' <summary>
    ''' 墓地番号
    ''' </summary>
    Private ReadOnly myGraveNumber As GraveNumberEntity

    ''' <summary>
    ''' 管理番号    
    ''' </summary>
    Private ReadOnly myCustomerID As CustomerID

    ''' <summary>
    ''' 面積
    ''' </summary>
    Private ReadOnly myArea As GraveArea

    ''' <summary>
    ''' 送付先名
    ''' </summary>
    Private ReadOnly myReceiverName As ReceiverName

    ''' <summary>
    ''' 送付先住所1
    ''' </summary>
    Private ReadOnly myReceiverAddress1 As ReceiverAddress1

    ''' <summary>
    ''' 送付先郵便番号
    ''' </summary>
    Private ReadOnly myReceiverPostalcode As ReceiverPostalcode

    ''' <summary>
    ''' 送付先住所2
    ''' </summary>
    Private ReadOnly myReceiverAddress2 As ReceiverAddress2

    ''' <summary>
    ''' 名義人クラスを生成します
    ''' </summary>
    ''' <param name="_myCustomerId">管理番号</param>
    ''' <param name="_myLesseeName">名義人名</param>
    ''' <param name="_myPostalCode">郵便番号</param>
    ''' <param name="_myAddress1">住所1</param>
    ''' <param name="_myAddress2">住所2</param>
    ''' <param name="_gravekuiki">墓地番号　区域</param>
    ''' <param name="_graveku">墓地番号　区</param>
    ''' <param name="_gravegawa">墓地番号　側</param>
    ''' <param name="_graveban">墓地番号　番</param>
    ''' <param name="_graveedaban">墓地番号　枝番</param>
    ''' <param name="_area"></param>面積
    ''' <param name="_myReceiverName">送付先名</param>
    ''' <param name="_myReceiverPostalCode">送付先郵便番号</param>
    ''' <param name="_myReceiverAddress1">送付先住所1</param>
    ''' <param name="_myReceiverAddress2">送付先住所2</param>
    Sub New(ByVal _myCustomerID As String, ByVal _myLesseeName As String, ByVal _myPostalCode As String, ByVal _myAddress1 As String, ByVal _myAddress2 As String, ByVal _gravekuiki As String,
            ByVal _graveku As String, ByVal _gravegawa As String, ByVal _graveban As String, ByVal _graveedaban As String, ByVal _area As Double, ByVal _myReceiverName As String, ByVal _myReceiverPostalCode As String, ByVal _myReceiverAddress1 As String,
             ByVal _myReceiverAddress2 As String)

        myGraveNumber = New GraveNumberEntity(_gravekuiki, _graveku, _gravegawa, _graveban, _graveedaban) '墓地番号を生成する。
        myArea = New GraveArea(_area) '面積を生成する
        myCustomerID = New CustomerID(_myCustomerID)
        myLesseName = New LesseeName(_myLesseeName)
        myAddress1 = New Address1(_myAddress1)
        myAddress2 = New Address2(_myAddress2)
        myPostalCode = New PostalCode(_myPostalCode)
        myReceiverName = New ReceiverName(_myReceiverName)
        myReceiverPostalcode = New ReceiverPostalcode(_myReceiverPostalCode)
        myReceiverAddress1 = New ReceiverAddress1(_myReceiverAddress1)
        myReceiverAddress2 = New ReceiverAddress2(_myReceiverAddress2)

    End Sub

    ''' <summary>
    ''' 宛名を返します
    ''' </summary>
    Public Function GetLesseeName() As String
        Return myLesseName.GetName
    End Function

    ''' <summary>
    ''' 郵便番号を返します
    ''' </summary>
    Public Function GetPostalCode() As String
        Return myPostalCode.GetCode
    End Function

    ''' <summary>
    ''' 住所1を返します
    ''' </summary>
    Public Function GetAddress1() As String
        Return myAddress1.GetAddress
    End Function

    ''' <summary>
    ''' 住所2を返します
    ''' </summary>
    Public Function GetAddress2() As String
        Return myAddress2.GetAddress
    End Function

    ''' <summary>
    ''' 墓地番号を返します
    ''' </summary>
    Public Function GetGraveNumber() As GraveNumberEntity
        Return myGraveNumber
    End Function

    ''' <summary>
    ''' 管理番号を返します
    ''' </summary>
    Public Function GetCustomerID() As String
        Return myCustomerID.GetNumber
    End Function

    ''' <summary>
    ''' 面積を返します
    ''' </summary>
    Public Function GetArea() As String
        Return myArea.GetArea
    End Function

    ''' <summary>
    ''' 送付先名を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetReceiverName() As String
        Return myReceiverName.GetName
    End Function

    ''' <summary>
    ''' 送付先郵便番号を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetReceiverPostalcode() As String
        Return myReceiverPostalcode.GetCode
    End Function

    ''' <summary>
    ''' 送付先住所1を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetReceiverAddress1() As String
        Return myReceiverAddress1.GetAddress
    End Function

    ''' <summary>
    ''' 送付先住所2を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetReceiverAddress2() As String
        Return myReceiverAddress2.GetAddress
    End Function

    ''' <summary>
    ''' 宛名
    ''' </summary>
    Private Class LesseeName

        Private Property Name As String

        Sub New(ByVal name_ As String)
            Name = name_
        End Sub

        Friend Function GetName() As String
            Return Name
        End Function

    End Class

    ''' <summary>
    ''' 住所1
    ''' </summary>
    Private Class Address1

        Private Property Address As String

        Sub New(ByVal myAddress1 As String)
            Address = myAddress1
        End Sub

        Friend Function GetAddress() As String
            Return Address
        End Function

    End Class

    ''' <summary>
    ''' 住所2
    ''' </summary>
    Private Class Address2

        Private Property Address As String

        Sub New(ByVal myAddress2 As String)
            Address = myAddress2
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
    ''' <summary>
    ''' 管理番号
    ''' </summary>
    Private Class CustomerID
        Private _Number As String

        Private Property Number As String
            Get
                Return _Number
            End Get
            Set
                _Number = Value
            End Set
        End Property

        Sub New(ByVal managementnumber As String)
            Number = managementnumber
        End Sub

        Friend Function GetNumber() As String
            Return Number
        End Function

    End Class

    ''' <summary>
    ''' 面積
    ''' </summary>
    Private Class GraveArea
        Private _Area As Double

        Sub New(ByVal myarea As Double)
            Area = myarea
        End Sub
        Private Property Area As Double
            Get
                Return _Area
            End Get
            Set
                _Area = Value
            End Set
        End Property

        Friend Function GetArea() As String
            Return Area.ToString("n1")
        End Function

    End Class

    ''' <summary>
    ''' 送付先名
    ''' </summary>
    Private Class ReceiverName
        Private Property Name As String

        Sub New(ByVal _name As String)
            Name = _name
        End Sub

        Friend Function GetName() As String
            Return Name
        End Function

    End Class

    ''' <summary>
    ''' 送付先住所1
    ''' </summary>
    Private Class ReceiverAddress1
        Private Property Address As String

        Sub New(ByVal _address1 As String)
            Address = _address1
        End Sub

        Friend Function GetAddress() As String
            Return Address
        End Function
    End Class

    ''' <summary>
    ''' 送付先住所2
    ''' </summary>
    Private Class ReceiverAddress2
        Private Property Address As String

        Sub New(ByVal _address2 As String)
            Address = _address2
        End Sub

        Friend Function GetAddress() As String
            Return Address
        End Function
    End Class

    ''' <summary>
    ''' 送付先郵便番号
    ''' </summary>
    Private Class ReceiverPostalcode
        Private Property Code As String

        Sub New(ByVal _postalcode As String)
            Code = _postalcode
        End Sub

        Friend Function GetCode() As String
            Return Code
        End Function
    End Class

End Class
