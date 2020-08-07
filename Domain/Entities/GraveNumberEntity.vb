Imports System.Text.RegularExpressions
Imports System.Collections.ObjectModel

''' <summary>
''' 文字列を変換します
''' </summary>
Friend Interface ITextConvert

    ''' <summary>
    ''' 区を相当する漢字はコード、コードは漢字にして返します
    ''' </summary>
    ''' <param name="value">区ナンバーあるいはコード</param>
    ''' <returns></returns>
    Function ConvertNumber_Ku(ByVal value As String) As String
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
    Public Function ConvertNumber_Ku(value As String) As String Implements ITextConvert.ConvertNumber_Ku

        Select Case value
            Case "01"
                Return My.Resources.EastString
            Case "02"
                Return My.Resources.WestString
            Case "03"
                Return My.Resources.SouthString
            Case "04"
                Return My.Resources.NorthString
            Case "05"
                Return My.Resources.CenterString
            Case 10
                Return My.Resources.EastSpecialString
            Case 11
                Return My.Resources.SecondSpecialString
            Case 12
                Return My.Resources.NorthSpecialString
            Case 20
                Return My.Resources.Ossuary
            Case My.Resources.EastString
                Return "01"
            Case My.Resources.WestString
                Return "02"
            Case My.Resources.SouthString
                Return "03"
            Case My.Resources.NorthString
                Return "04"
            Case My.Resources.CenterString
                Return "05"
            Case My.Resources.EastSpecialString
                Return 10
            Case My.Resources.SecondSpecialString
                Return 11
            Case My.Resources.NorthSpecialString
                Return 12
            Case My.Resources.Ossuary
                Return 20
            Case Else
                Return String.Empty
        End Select

    End Function

    ''' <summary>
    ''' 区域や側などで、数字に変換できるものは数字に、文字列が入っているものは左側の0の並びを削除する
    ''' </summary>
    ''' <param name="number">変換する文字列</param>
    Public Function ConvertNumber_0Delete(number As String) As String Implements ITextConvert.ConvertNumber_0Delete

        Dim StringVerification As New Regex("^[0-9]+$")

        'numberが数字として認識できるなら、数値として返す。ただし、0は空白を返す
        If StringVerification.IsMatch(number) Then Return IIf(CDbl(number) = 0, String.Empty, CDbl(number))

        If number = String.Empty Then Return String.Empty

        'numberの左側から一文字ずつ評価して、最初に0以外の数字が来たらループを抜け、その位置からの文字列を返す
        Dim i As Integer
        For i = 0 To number.Length - 1
            If Not StringVerification.IsMatch(number.Substring(i, 1)) Then Exit For
            If number.Substring(i, 1) > 0 Then Exit For
        Next

        Return number.Substring(i)

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

    ''' <param name="_ku">区</param>
    Public Sub New(ByVal _ku As String)
        KuField = New Ku(_ku)
    End Sub

    ''' <param name="_ku">区</param>
    ''' <param name="_kuiki">区域</param>
    Public Sub New(ByVal _ku As String, ByVal _kuiki As String)
        KuField = New Ku(_ku)
        KuikiField = New Kuiki(_kuiki)
    End Sub

    ''' <param name="_ku">区</param>
    ''' <param name="_kuiki">区域</param>
    ''' <param name="_gawa">側</param>
    Public Sub New(ByVal _ku As String, ByVal _kuiki As String, ByVal _gawa As String)
        KuField = New Ku(_ku)
        KuikiField = New Kuiki(_kuiki)
        GawaField = New Gawa(_gawa)
    End Sub

    ''' <param name="_ku">区</param>
    ''' <param name="_kuiki">区域</param>
    ''' <param name="_gawa">側</param>
    ''' <param name="_ban">番</param>
    Public Sub New(ByVal _ku As String, ByVal _kuiki As String, ByVal _gawa As String, ByVal _ban As String)
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
    Public Sub New(ByVal _ku As String, ByVal _kuiki As String, ByVal _gawa As String, ByVal _ban As String, ByVal _edaban As String)
        KuField = New Ku(_ku)
        KuikiField = New Kuiki(_kuiki)
        GawaField = New Gawa(_gawa)
        BanField = New Ban(_ban)

        If _edaban Is Nothing Then
            EdabanField = New Edaban(String.Empty)
        Else
            EdabanField = New Edaban(_edaban)
        End If

    End Sub

    ''' <summary>
    ''' 表示用墓地番号を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function ReturnDisplayForGraveNumber() As String
        Dim edabanString As String
        If Not String.IsNullOrEmpty(EdabanField.DisplayForField) Then edabanString = $"の{EdabanField.DisplayForField}"
        Return $"{KuField.DisplayForField}{IIf(KuikiField.DisplayForField = "0", String.Empty, KuikiField.DisplayForField)}区{GawaField.DisplayForField}側{BanField.DisplayForField}{EdabanField.DisplayForField}番"
    End Function

    ''' <summary>
    ''' 表示用墓地番号を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetNumber() As String
        With gtc
            Return $"{ .ConvertNumber_Ku(KuField.CodeField)}{ .ConvertNumber_0Delete(KuikiField.CodeField)}区{ .ConvertNumber_0Delete(GawaField.CodeField)}側{RTrim(gtc.ConvertNumber_0Delete(BanField.CodeField))}{Space(1)}{ .ConvertNumber_0Delete(EdabanField.CodeField)}番"
        End With
    End Function

    ''' <summary>
    ''' 表示用文字列「区」
    ''' </summary>
    ''' <returns></returns>
    Public Function ConvertKuString() As String
        Return gtc.ConvertNumber_Ku(KuField.CodeField)
    End Function

    ''' <summary>
    ''' 表示用文字列「区域」
    ''' </summary>
    ''' <returns></returns>
    Public Function ConvertKuikiString() As String
        Return gtc.ConvertNumber_0Delete(KuikiField.CodeField)
    End Function

    ''' <summary>
    ''' 表示用文字列「側」
    ''' </summary>
    ''' <returns></returns>
    Public Function ConvertGawaString() As String
        Return gtc.ConvertNumber_0Delete(GawaField.CodeField)
    End Function

    ''' <summary>
    ''' 表示用文字列「番」
    ''' </summary>
    ''' <returns></returns>
    Public Function ConvertBanString() As String
        Return gtc.ConvertNumber_0Delete(BanField.CodeField)
    End Function

    ''' <summary>
    ''' 表示用文字列「枝番」
    ''' </summary>
    ''' <returns></returns>
    Public Function ConvertEdabanString() As String
        Return gtc.ConvertNumber_0Delete(EdabanField.CodeField)
    End Function

    ''' <summary>
    ''' 区クラス
    ''' </summary>
    Public Class Ku
        Inherits GraveNumberField

        Public Sub New(ByVal _value As String)

            If IsNumeric(_value) Then
                CodeField = _value
            Else
                CodeField = gtc.ConvertNumber_Ku(_value)
            End If

            DisplayForField = gtc.ConvertNumber_Ku(CodeField)

        End Sub

    End Class
End Class