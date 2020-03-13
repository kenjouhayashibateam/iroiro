Public Class GravePanelDataEntity

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
    ''' <summary>
    ''' プリントアウト日時
    ''' </summary>
    ''' <returns></returns>
    Public Property MyPrintOutTime As PrintoutTime
    ''' <summary>
    ''' データベースID
    ''' </summary>
    ''' <returns></returns>
    Public Property MyOrderID As Integer
    ''' <summary>
    ''' 面積
    ''' </summary>
    ''' <returns></returns>
    Public Property MyArea As Area

    Sub New(ByVal _id As Integer, ByVal _customerid As String, ByVal _familyname As String, ByVal _gravenumber As String, ByVal _area As Double, ByVal _contractdetail As String, ByVal _registrationtime As Date, ByVal _printouttime As Date)
        MyOrderID = _id
        MyCustomerID = New CustomerID(_customerid)
        MyFamilyName = New FamilyName(_familyname)
        MyGraveNumber = New GraveNumber(_gravenumber)
        MyArea = New Area(_area)
        MyContractContent = New ContractContent(_contractdetail)
        MyRegistrationTime = New RegistrationTime(_registrationtime)
        MyPrintOutTime = New PrintoutTime(_printouttime)
        MyIsPrintout = New IsPrintout(MyPrintOutTime)
    End Sub

    Sub New(ByVal _id As Integer, ByVal _customerid As String, ByVal _familyname As String, ByVal _gravenumberKu As String, ByVal _gravenumberKuiki As String, ByVal _gravenumberGawa As String, ByVal _gravenumberBan As String, ByVal _gravenumberEdaban As String, ByVal _contractdetail As String, ByVal _registrationtime As Date, ByVal _printouttime As Date)
        MyOrderID = _id
        MyCustomerID = New CustomerID(_customerid)
        MyFamilyName = New FamilyName(_familyname)
        MyGraveNumber = New GraveNumber(_gravenumberKu & _gravenumberKuiki & "区" & _gravenumberGawa & "側" & _gravenumberBan & _gravenumberEdaban & "番")
        MyContractContent = New ContractContent(_contractdetail)
        MyRegistrationTime = New RegistrationTime(_registrationtime)
        MyPrintOutTime = New PrintoutTime(_printouttime)
        MyIsPrintout = New IsPrintout(MyPrintOutTime)
    End Sub

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

        Sub New(ByVal _printouttime As PrintoutTime)

            If _printouttime.MyDate = #1900/01/01# Then
                Value = False
            Else
                Value = True
            End If
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
            ID = _customerid
        End Sub

        Public Function GetID() As String
            Return ID
        End Function
    End Class

    ''' <summary>
    ''' 面積クラス
    ''' </summary>
    Public Class Area

        Public Property MyArea As Double

        Sub New(ByVal _myarea As Double)
            MyArea = _myarea
        End Sub

        Public Function GetArea() As Double
            Return MyArea
        End Function

    End Class
End Class
