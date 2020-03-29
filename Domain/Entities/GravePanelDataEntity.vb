Public Class GravePanelDataEntity

    Private _MyIsPrintout As IsPrintout
    Private _MyPrintOutTime As PrintoutTime

    ''' <summary>
    ''' 管理番号クラス
    ''' </summary>
    ''' <returns></returns>
    Public Property MyCustomerID As CustomerID
    ''' <summary>
    ''' 苗字クラス
    ''' </summary>
    ''' <returns></returns>
    Public Property MyFamilyName As FamilyName
    ''' <summary>
    ''' 墓地番号クラス
    ''' </summary>
    ''' <returns></returns>
    Public Property MyGraveNumber As GraveNumber
    ''' <summary>
    ''' 契約内容クラス
    ''' </summary>
    ''' <returns></returns>
    Public Property MyContractContent As ContractContent
    ''' <summary>
    ''' 登録日時クラス
    ''' </summary>
    ''' <returns></returns>
    Public Property MyRegistrationTime As RegistrationTime

    ''' <summary>
    ''' プリントアウトするかのBool
    ''' </summary>
    ''' <returns></returns>
    Public Property MyIsPrintout As IsPrintout
        Get
            Return _MyIsPrintout
        End Get
        Set
            _MyIsPrintout = Value
        End Set
    End Property

    ''' <summary>
    ''' プリントアウト日時
    ''' </summary>
    ''' <returns></returns>
    Public Property MyPrintOutTime As PrintoutTime
        Get
            Return _MyPrintOutTime
        End Get
        Set
            If Equals(Value) Then Return

            If MyIsPrintout Is Nothing Then
                _MyPrintOutTime = Value
                Return
            End If

            MyIsPrintout.ComparisonCheck(Value.MyDate)
            _MyPrintOutTime = Value
        End Set
    End Property

    ''' <summary>
    ''' データベースID
    ''' </summary>
    ''' <returns></returns>
    Public Property MyOrderID As OrderID
    ''' <summary>
    ''' 面積
    ''' </summary>
    ''' <returns></returns>
    Public Property MyArea As Area

    Public Property MyFullName As FullName

    Sub New(ByVal _id As Integer, ByVal _customerid As String, ByVal _familyname As String, ByVal _fullname As String, ByVal _gravenumber As String, ByVal _area As Double, ByVal _contractdetail As String, ByVal _registrationtime As Date, ByVal _printouttime As Date)
        MyOrderID = New OrderID(_id)
        MyCustomerID = New CustomerID(_customerid)
        MyFamilyName = New FamilyName(_familyname)
        MyFullName = New FullName(_fullname)
        MyGraveNumber = New GraveNumber(_gravenumber)
        MyArea = New Area(_area)
        MyContractContent = New ContractContent(_contractdetail)
        MyRegistrationTime = New RegistrationTime(_registrationtime)
        MyPrintOutTime = New PrintoutTime(_printouttime)
        MyIsPrintout = New IsPrintout(_printouttime)
    End Sub

    Sub New(ByVal _id As Integer, ByVal _customerid As String, ByVal _familyname As String, ByVal _fullname As String, ByVal _gravenumberKu As String, ByVal _gravenumberKuiki As String, ByVal _gravenumberGawa As String, ByVal _gravenumberBan As String, ByVal _gravenumberEdaban As String, ByVal _contractdetail As String, ByVal _registrationtime As Date, ByVal _printouttime As Date)
        MyOrderID = New OrderID(_id)
        MyCustomerID = New CustomerID(_customerid)
        MyFamilyName = New FamilyName(_familyname)
        MyFullName = New FullName(_fullname)
        MyGraveNumber = New GraveNumber(_gravenumberKu & _gravenumberKuiki & "区" & _gravenumberGawa & "側" & _gravenumberBan & _gravenumberEdaban & "番")
        MyContractContent = New ContractContent(_contractdetail)
        MyRegistrationTime = New RegistrationTime(_registrationtime)
        MyPrintOutTime = New PrintoutTime(_printouttime)
        MyIsPrintout = New IsPrintout(_printouttime)
    End Sub

    Public Function GetID() As Integer
        Return MyOrderID.ID
    End Function

    Public Function GetFullName() As String
        Return MyFullName.Name
    End Function

    Public Function GetArea() As Double
        Return MyArea.AreaValue
    End Function

    ''' <summary>
    ''' プリントアウト日時を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetPrintoutTime() As Date
        Return MyPrintOutTime.MyDate
    End Function

    ''' <summary>
    ''' 登録日時を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetRegistrationTime() As Date
        Return MyRegistrationTime.MyDate
    End Function

    ''' <summary>
    ''' 契約内容を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetContractContent() As String
        Return MyContractContent.GetContent
    End Function

    ''' <summary>
    ''' 墓地番号を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetGraveNumber() As String
        Return MyGraveNumber.Number
    End Function

    ''' <summary>
    ''' 管理番号を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetCustomerID() As String
        Return MyCustomerID.GetID
    End Function

    ''' <summary>
    ''' 苗字を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetFamilyName() As String
        Return MyFamilyName.GetName
    End Function

    Public Class FullName
        Public Property Name As String

        Sub New(ByVal _name As String)
            Name = _name
        End Sub
    End Class

    ''' <summary>
    ''' データベースIDクラス
    ''' </summary>
    Public Class OrderID

        Public Property ID As Integer

        Sub New(ByVal _orderid As Integer)
            ID = _orderid
        End Sub
    End Class

    ''' <summary>
    ''' プリントアウトするかの確認クラス
    ''' </summary>
    Public Class IsPrintout

        Public Property Value As Boolean

        Sub New(ByVal _printouttime As Date)
            ComparisonCheck(_printouttime)
        End Sub

        Public Sub ComparisonCheck(ByVal _printouttime As Date)
            Value = _printouttime = #1900/01/01#
        End Sub

    End Class

    ''' <summary>
    ''' プリントアウト日時クラス
    ''' </summary>
    Public Class PrintoutTime

        Public Property MyDate As Date

        Sub New(ByVal _purintouttime As Date)
            MyDate = _purintouttime
        End Sub

    End Class

    ''' <summary>
    ''' 墓地番号クラス
    ''' </summary>
    Public Class GraveNumber

        Public Property Number As String

        Sub New(ByVal _gravenumber As String)
            Number = _gravenumber
        End Sub
    End Class

    ''' <summary>
    ''' 登録日時クラス
    ''' </summary>
    Public Class RegistrationTime

        Public Property MyDate As Date

        Sub New(ByVal _registrationtime As Date)
            MyDate = _registrationtime
        End Sub

    End Class

    ''' <summary>
    ''' 契約内容クラス
    ''' </summary>
    Public Class ContractContent

        Public Property Content As String

        Sub New(ByVal _contractdetail As String)
            Content = _contractdetail
        End Sub

        Public Function GetContent() As String
            Return Content
        End Function
    End Class

    ''' <summary>
    ''' 苗字クラス
    ''' </summary>
    Public Class FamilyName

        Public Property Name As String

        Sub New(ByVal _name As String)
            Name = _name
        End Sub

        Public Function GetName() As String
            Return Name
        End Function
    End Class

    ''' <summary>
    ''' 管理番号クラス
    ''' </summary>
    Public Class CustomerID

        Public Property ID As String

        Sub New(ByVal _customerid As String)
            If String.IsNullOrEmpty(_customerid) Then
                ID = "未登録"
            Else
                ID = _customerid
            End If
        End Sub

        Public Function GetID() As String
            Return ID
        End Function
    End Class

    ''' <summary>
    ''' 面積クラス
    ''' </summary>
    Public Class Area

        Public Property AreaValue As Double

        Sub New(ByVal _myarea As Double)
            AreaValue = _myarea
        End Sub

        Public Function GetArea() As Double
            Return AreaValue
        End Function

    End Class
End Class
