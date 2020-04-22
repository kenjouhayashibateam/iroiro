﻿Imports System.Collections.ObjectModel

''' <summary>
''' 墓地札データクラス
''' </summary>
Public Class GravePanelDataEntity
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
    ''' 契約内容リストクラス
    ''' </summary>
    ''' <returns></returns>
    Public Property MyContractContents As New ContractContents

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
    ''' <summary>
    ''' 申込者名
    ''' </summary>
    ''' <returns></returns>
    Public Property MyFullName As FullName

    Public Sub New(ByVal _id As Integer, ByVal _customerid As String, ByVal _familyname As String, ByVal _fullname As String, ByVal _gravenumber As String, ByVal _area As Double, ByVal _contractdetail As String, ByVal _registrationtime As Date, ByVal _printouttime As Date)
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

    Public Sub New(ByVal _id As Integer, ByVal _customerid As String, ByVal _familyname As String, ByVal _fullname As String, ByVal _gravenumberKu As String, ByVal _gravenumberKuiki As String, ByVal _gravenumberGawa As String, ByVal _gravenumberBan As String, ByVal _gravenumberEdaban As String, ByVal _contractdetail As String, ByVal _registrationtime As Date, ByVal _printouttime As Date)
        MyOrderID = New OrderID(_id)
        MyCustomerID = New CustomerID(_customerid)
        MyFamilyName = New FamilyName(_familyname)
        MyFullName = New FullName(_fullname)
        MyGraveNumber = New GraveNumber($"{_gravenumberKu}{_gravenumberKuiki}区{_gravenumberGawa}側{_gravenumberBan}
                                                                       {_gravenumberEdaban}番")
        MyContractContent = New ContractContent(_contractdetail)
        MyRegistrationTime = New RegistrationTime(_registrationtime)
        MyPrintOutTime = New PrintoutTime(_printouttime)
        MyIsPrintout = New IsPrintout(_printouttime)
    End Sub
    ''' <summary>
    ''' OrderIDを返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetID() As Integer
        Return MyOrderID.ID
    End Function
    ''' <summary>
    ''' 申込者名を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetFullName() As String
        Return MyFullName.Name
    End Function
    ''' <summary>
    ''' 面積を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetArea() As String
        Return MyArea.AreaValue.ToString("n1")
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
    Public Function GetCustomerID() As CustomerID
        Return MyCustomerID
    End Function
    ''' <summary>
    ''' 苗字を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetFamilyName() As String
        Return MyFamilyName.GetName
    End Function
    ''' <summary>
    ''' 契約内容を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetContractContents() As ObservableCollection(Of String)
        Return MyContractContents.List
    End Function

End Class