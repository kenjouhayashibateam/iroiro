﻿
''' <summary>
''' 住所1
''' </summary>
Public Class Address1
    Public Property Address As String

    Public Sub New(ByVal _address1 As String)
        Address = _address1
    End Sub

    Public Function GetAddress() As String
        Return Address
    End Function
End Class