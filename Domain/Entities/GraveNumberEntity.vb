Imports System.Text.RegularExpressions
Imports System.Collections.ObjectModel

''' <summary>
''' 文字列を変換します
''' </summary>
Interface ITextConvert

    ''' <summary>
    ''' 区を相当する漢字にして返します
    ''' </summary>
    ''' <param name="kunumber">区ナンバー</param>
    ''' <returns></returns>
    Function ConvertNumber_Ku(ByVal kunumber As GraveNumberEntity.Ku) As String
    ''' <summary>
    ''' 墓地番号を表示用にするため、0を削除します
    ''' </summary>
    ''' <param name="number"></param>
    ''' <returns></returns>
    Function ConvertNumber_0Delete(ByVal number As String) As String

End Interface

''' <summary>
''' 墓地番号文字列変換クラス
''' </summary>
Public Class GraveTextConvert
    Implements ITextConvert

    ''' <summary>
    ''' 区域コードを漢字に変換します
    ''' </summary>
    Public Function ConvertNumber_Ku(kunumber As GraveNumberEntity.Ku) As String Implements ITextConvert.ConvertNumber_Ku

        Select Case kunumber.Field
            Case "01"
                Return "東"
            Case "02"
                Return "西"
            Case "03"
                Return "南"
            Case "04"
                Return "北"
            Case "05"
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
                Return kunumber.Field
        End Select

    End Function

    ''' <summary>
    ''' 区域や側などで、数字に変換できるものは数字に、文字列が入っているものは左側の0の並びを削除する
    ''' </summary>
    ''' <param name="number">変換する文字列</param>
    Public Function ConvertNumber_0Delete(number As String) As String Implements ITextConvert.ConvertNumber_0Delete

        Dim ReturnString As String
        Dim StringVerification As New Regex("^[0-9]+$")

        If StringVerification.IsMatch(number) Then
            Return IIf(CDbl(number) = 0, "", CDbl(number))
        End If

        If number = String.Empty Then Return String.Empty

        Dim I As Integer = 0
        StringVerification = New Regex("[0-9]")
        Do Until number.Length = I + 1
            If Not StringVerification.IsMatch(number.Substring(I, 1)) Then Exit Do
            If number.Substring(I, 1) > 0 Then Exit Do
            I += 1
        Loop

        ReturnString = number.Substring(I)

        Return ReturnString

    End Function
End Class

''' <summary>
''' 墓地番号クラス
''' </summary>
Public Class GraveNumberEntity

    Private ReadOnly gtc As New GraveTextConvert
    Public Property KuField As Ku
    Public Property KuikiField As Kuiki
    Public Property GawaField As Gawa
    Public Property BanField As Ban
    Public Property EdabanField As Edaban
    Public Property CustomerIDField As CustomerID
    Public Property MyFormalNumber As FormalNumber

    ''' <param name="_ku">区</param>
    Sub New(ByVal _ku As String)

        KuField = New Ku(_ku)

    End Sub

    ''' <param name="_ku">区</param>
    ''' <param name="_kuiki">区域</param>
    Sub New(ByVal _ku As String, ByVal _kuiki As String)

        KuField = New Ku(_ku)
        KuikiField = New Kuiki(_kuiki)

    End Sub

    ''' <param name="_ku">区</param>
    ''' <param name="_kuiki">区域</param>
    ''' <param name="_gawa">側</param>
    Sub New(ByVal _ku As String, ByVal _kuiki As String, ByVal _gawa As String)

        KuField = New Ku(_ku)
        KuikiField = New Kuiki(_kuiki)
        GawaField = New Gawa(_gawa)

    End Sub

    ''' <param name="_ku">区</param>
    ''' <param name="_kuiki">区域</param>
    ''' <param name="_gawa">側</param>
    ''' <param name="_ban">番</param>
    Sub New(ByVal _ku As String, ByVal _kuiki As String, ByVal _gawa As String, ByVal _ban As String)

        KuField = New Ku(_ku)
        KuikiField = New Kuiki(_kuiki)
        GawaField = New Gawa(_gawa)
        BanField = New Ban(_ban)

    End Sub

    ''' <param name="_ku">区</param>
    ''' <param name="_kuiki">区域</param>
    ''' <param name="_gawa">側</param>
    ''' <param name="_ban">番</param>
    ''' <param name="_edaban">枝番</param>
    Sub New(ByVal _ku As String, ByVal _kuiki As String, ByVal _gawa As String, ByVal _ban As String, ByVal _edaban As String)

        KuField = New Ku(_ku)
        KuikiField = New Kuiki(_kuiki)
        GawaField = New Gawa(_gawa)
        BanField = New Ban(_ban)
        If _edaban Is Nothing Then
            EdabanField = New Edaban(String.Empty)
        Else
            EdabanField = New Edaban(_edaban)
        End If

        MyFormalNumber = New FormalNumber(KuField, KuikiField, GawaField, BanField, EdabanField)

    End Sub

    ''' <summary>
    ''' 表示用墓地番号を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetNumber() As String
        Return gtc.ConvertNumber_Ku(KuField) & gtc.ConvertNumber_0Delete(KuikiField.Field) & "区" & gtc.ConvertNumber_0Delete(GawaField.Field) & "側" & RTrim(gtc.ConvertNumber_0Delete(BanField.Field) & " " & gtc.ConvertNumber_0Delete(EdabanField.Field)) & "番"
    End Function

    ''' <summary>
    ''' 表示用文字列「区」
    ''' </summary>
    ''' <returns></returns>
    Public Function ConvertKuString() As String
        Return gtc.ConvertNumber_Ku(KuField)
    End Function

    ''' <summary>
    ''' 表示用文字列「区域」
    ''' </summary>
    ''' <returns></returns>
    Public Function ConvertKuikiString() As String
        Return gtc.ConvertNumber_0Delete(KuikiField.Field)
    End Function

    ''' <summary>
    ''' 表示用文字列「側」
    ''' </summary>
    ''' <returns></returns>
    Public Function ConvertGawaString() As String
        Return gtc.ConvertNumber_0Delete(GawaField.Field)
    End Function

    ''' <summary>
    ''' 表示用文字列「番」
    ''' </summary>
    ''' <returns></returns>
    Public Function ConvertBanString() As String
        Return gtc.ConvertNumber_0Delete(BanField.Field)
    End Function

    ''' <summary>
    ''' 表示用文字列「枝番」
    ''' </summary>
    ''' <returns></returns>
    Public Function ConvertEdabanString() As String
        Return gtc.ConvertNumber_0Delete(EdabanField.Field)
    End Function

    ''' <summary>
    ''' 墓地番号
    ''' </summary>
    Public Class FormalNumber

        Public Property Number As String

        Sub New(ByVal _ku As Ku, ByVal _kuiki As Kuiki, ByVal _gawa As Gawa, ByVal _ban As Ban, ByVal _edaban As Edaban)
            Number = _ku.DisplayForField & _kuiki.DisplayForField & "区" & _gawa.DisplayForField & "側" & _ban.DisplayForField & _edaban.DisplayForField & "番"
        End Sub
    End Class

    ''' <summary>
    ''' 管理番号
    ''' </summary>
    Public Class CustomerID

        Public Property ID As String

        Sub New(ByVal _customerid As String)
            ID = _customerid
        End Sub

    End Class

    ''' <summary>
    ''' 区クラス
    ''' </summary>
    Public Class Ku

        Private ReadOnly gtc As New GraveTextConvert
        Public Property DisplayForField As String
        Public Property Field As String

        Sub New(ByVal _value As String)
            Field = _value
            DisplayForField = gtc.ConvertNumber_Ku(Me)
        End Sub
    End Class

    ''' <summary>
    ''' 区域クラス
    ''' </summary>
    Public Class Kuiki

        Private ReadOnly gtc As New GraveTextConvert
        Public Property DisplayForField As String
        Public Property Field As String

        Sub New(ByVal _value As String)
            Field = _value
            If gtc.ConvertNumber_0Delete(_value) = String.Empty Then
                DisplayForField = "0"
            Else
                DisplayForField = gtc.ConvertNumber_0Delete(_value)
            End If
        End Sub

    End Class

    ''' <summary>
    ''' 側クラス
    ''' </summary>
    Public Class Gawa

        Private ReadOnly gtc As New GraveTextConvert
        Public Property DisplayForField As String
        Public Property Field As String

        Sub New(ByVal _value As String)
            Field = _value
            If gtc.ConvertNumber_0Delete(_value) = String.Empty Then
                DisplayForField = "0"
            Else
                DisplayForField = gtc.ConvertNumber_0Delete(_value)
            End If
        End Sub

    End Class

    ''' <summary>
    ''' 番クラス
    ''' </summary>
    Public Class Ban

        Private ReadOnly gtc As New GraveTextConvert
        Public Property DisplayForField As String
        Public Property Field As String

        Sub New(ByVal _value As String)
            Field = _value
            DisplayForField = gtc.ConvertNumber_0Delete(_value)
        End Sub

    End Class

    ''' <summary>
    ''' 枝番クラス
    ''' </summary>
    Public Class Edaban

        Private ReadOnly gtc As New GraveTextConvert
        Public Property DisplayForField As String
        Public Property Field As String

        Sub New(ByVal _value As String)
            Field = _value
            DisplayForField = gtc.ConvertNumber_0Delete(_value)
        End Sub
    End Class

    ''' <summary>
    ''' 枝番リストクラス
    ''' </summary>
    Public Class EdabanList

        Public Property List As ObservableCollection(Of Edaban)

        Sub New(ByVal _list As ObservableCollection(Of Edaban))
            List = _list
        End Sub
    End Class

    ''' <summary>
    ''' 番リストクラス
    ''' </summary>
    Public Class BanList

        Public Property List As ObservableCollection(Of Ban)

        Sub New(ByVal _list As ObservableCollection(Of Ban))
            List = _list
        End Sub
    End Class

    ''' <summary>
    ''' 側リストクラス
    ''' </summary>
    Public Class GawaList

        Public Property List As ObservableCollection(Of Gawa)

        Sub New(ByVal _list As ObservableCollection(Of Gawa))
            List = _list
        End Sub
    End Class

    ''' <summary>
    ''' 区域リストクラス
    ''' </summary>
    Public Class KuikiList

        Public Property List As ObservableCollection(Of Kuiki)

        Sub New(ByVal _list As ObservableCollection(Of Kuiki))
            List = _list
        End Sub

    End Class
End Class


