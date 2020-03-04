Imports System.Text.RegularExpressions
Imports System.Collections.ObjectModel

Interface ITextConvert

    Function ConvertNumber_Ku(ByVal kunumber As GraveNumberEntity.Ku) As String
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
                Return ""
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
    Public Property CustomerID As String

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
        EdabanField = New Edaban(_edaban)

    End Sub

    ''' <summary>
    ''' 表示用墓地番号を返します
    ''' </summary>
    ''' <returns></returns>
    Public Function GetNumber() As String
        Return gtc.ConvertNumber_Ku(KuField) & gtc.ConvertNumber_0Delete(KuikiField.Field) & "区" & gtc.ConvertNumber_0Delete(GawaField.Field) & "側" & RTrim(gtc.ConvertNumber_0Delete(BanField.Field) & " " & gtc.ConvertNumber_0Delete(EdabanField.Field)) & "番"
    End Function

    Public Function ConvertKuString() As String
        Return gtc.ConvertNumber_Ku(KuField)
    End Function

    Public Function ConvertKuikiString() As String
        Return gtc.ConvertNumber_0Delete(KuikiField.Field)
    End Function

    Public Function ConvertGawaString() As String
        Return gtc.ConvertNumber_0Delete(GawaField.Field)
    End Function

    Public Function ConvertBanString() As String
        Return gtc.ConvertNumber_0Delete(BanField.Field)
    End Function

    Public Function ConvertEdabanString() As String
        Return gtc.ConvertNumber_0Delete(EdabanField.Field)
    End Function

    Public Class Ku
        Private _Field As String

        Private ReadOnly gtc As New GraveTextConvert
        Private _DisplayForField As String

        Public Property DisplayForField As String
            Get
                Return _DisplayForField
            End Get
            Set
                _DisplayForField = Value
            End Set
        End Property

        Public Property Field As String
            Get
                Return _Field
            End Get
            Set
                _Field = Value
            End Set
        End Property

        Sub New(ByVal _value As String)
            Field = _value
            DisplayForField = gtc.ConvertNumber_0Delete(_value)
        End Sub

    End Class


    Public Class Kuiki
        Private _Field As String

        Private ReadOnly gtc As New GraveTextConvert
        Private _DisplayForField As String

        Public Property DisplayForField As String
            Get
                Return _DisplayForField
            End Get
            Set
                _DisplayForField = Value
            End Set
        End Property

        Public Property Field As String
            Get
                Return _Field
            End Get
            Set
                _Field = Value
            End Set
        End Property

        Sub New(ByVal _value As String)
            Field = _value
            DisplayForField = gtc.ConvertNumber_0Delete(_value)
        End Sub

    End Class

    Public Class Gawa
        Private _Field As String
        Private ReadOnly gtc As New GraveTextConvert
        Private _DisplayForField As String

        Public Property DisplayForField As String
            Get
                Return _DisplayForField
            End Get
            Set
                _DisplayForField = Value
            End Set
        End Property

        Public Property Field As String
            Get
                Return _Field
            End Get
            Set
                _Field = Value
            End Set
        End Property

        Sub New(ByVal _value As String)
            Field = _value
            DisplayForField = gtc.ConvertNumber_0Delete(_value)
        End Sub
    End Class

    Public Class Ban
        Private _Field As String
        Private ReadOnly gtc As New GraveTextConvert
        Private _DisplayForField As String

        Public Property DisplayForField As String
            Get
                Return _DisplayForField
            End Get
            Set
                _DisplayForField = Value
            End Set
        End Property

        Public Property Field As String
            Get
                Return _Field
            End Get
            Set
                _Field = Value
            End Set
        End Property

        Sub New(ByVal _value As String)
            Field = _value
            DisplayForField = gtc.ConvertNumber_0Delete(_value)
        End Sub
    End Class

    Public Class Edaban
        Private _Field As String

        Private ReadOnly gtc As New GraveTextConvert
        Private _DisplayForField As String

        Public Property DisplayForField As String
            Get
                Return _DisplayForField
            End Get
            Set
                _DisplayForField = Value
            End Set
        End Property

        Public Property Field As String
            Get
                Return _Field
            End Get
            Set
                _Field = Value
            End Set
        End Property

        Sub New(ByVal _value As String)
            Field = _value
            DisplayForField = gtc.ConvertNumber_0Delete(_value)
        End Sub
    End Class

    Public Class EdabanList
        Private _List As ObservableCollection(Of Edaban)

        Public Property List As ObservableCollection(Of Edaban)
            Get
                Return _List
            End Get
            Set
                _List = Value
            End Set
        End Property

        Sub New(ByVal _list As ObservableCollection(Of Edaban))
            List = _list
        End Sub
    End Class

    Public Class BanList
        Private _List As ObservableCollection(Of Ban)

        Public Property List As ObservableCollection(Of Ban)
            Get
                Return _List
            End Get
            Set
                _List = Value
            End Set
        End Property

        Sub New(ByVal _list As ObservableCollection(Of Ban))
            List = _list
        End Sub
    End Class

    Public Class GawaList
        Private _List As ObservableCollection(Of Gawa)

        Public Property List As ObservableCollection(Of Gawa)
            Get
                Return _List
            End Get
            Set
                _List = Value
            End Set
        End Property

        Sub New(ByVal _list As ObservableCollection(Of Gawa))
            List = _list
        End Sub
    End Class

    Public Class KuikiList

        Private _List As ObservableCollection(Of Kuiki)

        Public Property List As ObservableCollection(Of Kuiki)
            Get
                Return _List
            End Get
            Set
                _List = Value
            End Set
        End Property

        Sub New(ByVal _list As ObservableCollection(Of Kuiki))
            List = _list
        End Sub

    End Class
End Class


