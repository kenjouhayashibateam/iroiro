
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
    Private ReadOnly myArea As Area

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
    Public Sub New(_myCustomerID As String, _myLesseeName As String, _myPostalCode As String, _myAddress1 As String,
                   _myAddress2 As String, _gravekuiki As String, _graveku As String, _gravegawa As String, _graveban As String,
                   _graveedaban As String, _area As Double, _myReceiverName As String, _myReceiverPostalCode As String,
                   _myReceiverAddress1 As String, _myReceiverAddress2 As String)

        myGraveNumber = New GraveNumberEntity(_gravekuiki, _graveku, _gravegawa, _graveban, _graveedaban) '墓地番号を生成する。
        myArea = New Area(_area) '面積を生成する
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
    Public Function GetLesseeName() As LesseeName
        Return myLesseName
    End Function

    ''' <summary>
    ''' 郵便番号を返します
    ''' </summary>
    Public Function GetPostalCode() As PostalCode
        Return myPostalCode
    End Function

    ''' <summary>
    ''' 住所1を返します
    ''' </summary>
    Public Function GetAddress1() As Address1
        Return myAddress1
    End Function

    ''' <summary>
    ''' 住所2を返します
    ''' </summary>
    Public Function GetAddress2() As Address2
        Return myAddress2
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
    Public Function GetCustomerID() As CustomerID
        Return myCustomerID
    End Function

    ''' <summary>
    ''' 面積を返します
    ''' </summary>
    Public Function GetArea() As Area
        Return myArea
    End Function

    ''' <summary>
    ''' 送付先名を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetReceiverName() As ReceiverName
        Return myReceiverName
    End Function

    ''' <summary>
    ''' 送付先郵便番号を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetReceiverPostalcode() As ReceiverPostalcode
        Return myReceiverPostalcode
    End Function

    ''' <summary>
    ''' 送付先住所1を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetReceiverAddress1() As ReceiverAddress1
        Return myReceiverAddress1
    End Function

    ''' <summary>
    ''' 送付先住所2を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetReceiverAddress2() As ReceiverAddress2
        Return myReceiverAddress2
    End Function

End Class
