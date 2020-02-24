Imports System.Text.RegularExpressions

''' <summary>
''' 名義人データ格納クラス
''' </summary>
Public Class LesseeCustomerInfoEntity

    ''' <summary>
    ''' 宛名
    ''' </summary>
    Private myAddressee As Addressee

    ''' <summary>
    ''' 住所（郵便番号で表される部分）
    ''' </summary>
    Private myAddress1 As Address1

    ''' <summary>
    ''' 住所（郵便番号で表さない番地等）
    ''' </summary>
    Private myAddress2 As Address2

    ''' <summary>
    ''' 郵便番号
    ''' </summary>
    Private myPostalCode As PostalCode

    ''' <summary>
    ''' 墓地番号
    ''' </summary>
    Private ReadOnly myGraveNumber As GraveNumber

    ''' <summary>
    ''' 管理番号    
    ''' </summary>
    Private myCustomerID As CustomerID

    ''' <summary>
    ''' 面積
    ''' </summary>
    Private ReadOnly myArea As Area

    ''' <summary>
    ''' 名義人クラスを生成します
    ''' </summary>
    ''' <param name="myCustomerID">管理番号</param>
    ''' <param name="myLesseeName">名義人名</param>
    ''' <param name="myPostalCode">郵便番号</param>
    ''' <param name="myAddress1">住所1</param>
    ''' <param name="myAddress2">住所2</param>
    ''' <param name="gravekuiki">墓地番号　区域</param>
    ''' <param name="graveku">墓地番号　区</param>
    ''' <param name="gravegawa">墓地番号　側</param>
    ''' <param name="graveban">墓地番号　番</param>
    ''' <param name="graveedaban">墓地番号　枝番</param>
    ''' <param name="area"></param>面積
    ''' <param name="myReceiverName">送付先名</param>
    ''' <param name="myReceiverPostalCode">送付先郵便番号</param>
    ''' <param name="myReceiverAddress1">送付先住所1</param>
    ''' <param name="myReceiverAddress2">送付先住所2</param>
    Sub New(ByVal myCustomerID As String, ByVal myLesseeName As String, ByVal myPostalCode As String, ByVal myAddress1 As String, ByVal myAddress2 As String, ByVal gravekuiki As String,
            ByVal graveku As String, ByVal gravegawa As String, ByVal graveban As String, ByVal graveedaban As String, ByVal area As Double,
            Optional ByVal myReceiverName As String = "", Optional ByVal myReceiverPostalCode As String = "", Optional ByVal myReceiverAddress1 As String = "",
            Optional ByVal myReceiverAddress2 As String = "")

        myGraveNumber = New GraveNumber(gravekuiki, graveku, gravegawa, graveban, graveedaban) '墓地番号を生成する。
        myArea = New Area(area) '面積を生成する

        Dim DataGenre As String = myLesseeName & myReceiverName '格納するデータを名義人にするか、送付先にするかの判断基準にする変数

        'DataGenreを使用して、プロパティに値を格納する
        Select Case DataGenre
            Case myLesseeName '名義人のみの場合
                DataInput(myCustomerID, myLesseeName, myPostalCode, myAddress1, myAddress2)
                Exit Sub
            Case myLesseeName & myLesseeName '送付先に名義人の名前でデータがある場合
                DataInput(myCustomerID, myLesseeName, myReceiverPostalCode, myReceiverAddress1, myReceiverAddress2)
                Exit Sub
        End Select

        '名義人と送付先で名前が違う場合
        If MsgBox("名義人データ" & vbNewLine & "名義人名 : " & myLesseeName & vbNewLine & "住所1 : " & myAddress1 & vbNewLine & "住所2 : " & myAddress2 & vbNewLine & vbNewLine &
                  "送付先データ" & vbNewLine & "送付先名 : " & myReceiverName & vbNewLine & "送付先住所1 : " & vbNewLine & "送付先住所2 : " & myReceiverAddress2 & vbNewLine &
                  vbNewLine & vbNewLine & "名義人と送付先、どちらを使用しますか？　はい → 名義人　　いいえ → 送付先", vbYesNo, "データ選択") = MsgBoxResult.Yes Then
            DataInput(myCustomerID, myLesseeName, myPostalCode, myAddress1, myAddress2)
        Else
            DataInput(myCustomerID, myReceiverName, myReceiverPostalCode, myReceiverAddress1, myReceiverAddress2)
        End If

    End Sub

    ''' <summary>
    ''' 宛名を返します
    ''' </summary>
    Public Function GetAddressee() As String
        Return myAddressee.GetName
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
    Public Function GetGraveNumber() As String
        Return myGraveNumber.GetNumber
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
    ''' 各プロパティに値を入力します
    ''' </summary>
    ''' <param name="managementnumber">管理番号</param>
    ''' <param name="addressee">宛名</param>
    ''' <param name="postalcode">郵便番号</param>
    ''' <param name="address1">住所1</param>
    ''' <param name="address2">住所2</param>
    Private Sub DataInput(ByVal managementnumber As String, ByVal addressee As String, ByVal postalcode As String, ByVal address1 As String, ByVal address2 As String)

        myCustomerID = New CustomerID(managementnumber)
        myAddressee = New Addressee(addressee)
        myAddress1 = New Address1(address1)
        myAddress2 = New Address2(address2)
        myPostalCode = New PostalCode(postalcode)

    End Sub

    ''' <summary>
    ''' 宛名
    ''' </summary>
    Private Class Addressee

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
    ''' 墓地番号
    ''' </summary>
    Private Class GraveNumber
        Private _Number As String

        Private Property Number As String
            Get
                Return _Number
            End Get
            Set
                _Number = Value
            End Set
        End Property

        ''' <summary>
        ''' 墓地番号を閲覧用に編集して保持します
        ''' </summary>
        ''' <param name="kuiki">区域</param>
        ''' <param name="ku">区</param>
        ''' <param name="gawa">側</param>
        ''' <param name="ban">番</param>
        ''' <param name="edaban">枝番</param>
        Sub New(ByVal kuiki As String, ByVal ku As String, ByVal gawa As String, ByVal ban As String, ByVal edaban As String)

            Number = ConvertNumber_Kuiki(kuiki) & ConvertNumber_0Delete(ku) & "区 " & ConvertNumber_0Delete(gawa) & "側 " & ConvertNumber_0Delete(ban) &
                     ConvertNumber_0Delete(edaban) & "番"
        End Sub

        ''' <summary>
        ''' 区域コードを漢字に変換します
        ''' </summary>
        ''' <param name="kuiki">区域コード</param>
        Private Function ConvertNumber_Kuiki(ByVal kuiki As Integer) As String

            Select Case kuiki
                Case 1
                    Return "東"
                Case 2
                    Return "西"
                Case 3
                    Return "南"
                Case 4
                    Return "北"
                Case 5
                    Return "中"
                Case 10
                    Return "東特"
                Case 11
                    Return "二特"
                Case 12
                    Return "北特"
                Case 20
                    Return "御廟"
                Case Else
                    Return ""
            End Select

        End Function

        ''' <summary>
        ''' 区域や側などで、数字に変換できるものは数字に、文字列が入っているものは左側の0の並びを削除する
        ''' </summary>
        ''' <param name="number">変換する文字列</param>
        Private Function ConvertNumber_0Delete(ByVal number As String) As String

            Dim ReturnString As String
            Dim StringVerification As New Regex("^[0-9]+$")

            If StringVerification.IsMatch(number) Then
                Return IIf(CDbl(number) = 0, "", CDbl(number))
            End If


            Dim I As Integer = 1
            Do Until number.Substring(I, 1) <> 0
                I += 1
            Loop
            ReturnString = number.Substring(I)

            Return ReturnString

        End Function

        ''' <summary>
        ''' 管理番号を返します
        ''' </summary>
        ''' <returns></returns>
        Friend Function GetNumber() As String
            Return Number()
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
    Private Class Area
        Private _Area As Double

        Sub New(ByVal myarea As Double)
            Area = myarea
        End Sub
        Private Property Area
            Get
                Return _Area
            End Get
            Set
                _Area = Value
            End Set
        End Property

        Friend Function GetArea() As Double
            Return Area
        End Function

    End Class
End Class
