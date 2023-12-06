Imports System.Globalization

Namespace Behaviors
    Public Class BoolNegConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
            If Not (TypeOf value Is Boolean) Then Return False
            Return Not (value)
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            If Not (TypeOf value Is Boolean) Then Return False
            Return Not (value)
        End Function
    End Class
End Namespace