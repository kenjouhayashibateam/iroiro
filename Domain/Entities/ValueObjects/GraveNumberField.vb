''' <summary>
''' 墓地番号のFieldクラスのスーパークラス
''' </summary>
Public MustInherit Class GraveNumberField

    Protected ReadOnly gtc As New GraveTextConvert
    Public Property DisplayForField As String
    Public Property CodeField As String

    Public Overrides Function Equals(obj As Object) As Boolean
        Return FieldTryCast(obj, Me)
    End Function

    Public Function FieldTryCast(ByRef obj As Object, _gravenumberfield As GraveNumberField) As Boolean
        Dim reasion As GraveNumberField = TryCast(obj, GraveNumberField)
        If reasion Is Nothing Then Return False
        If Not reasion.DisplayForField.Equals(_gravenumberfield.DisplayForField) Then Return False
        Return Not reasion.CodeField.Equals(_gravenumberfield.CodeField)
    End Function

End Class
