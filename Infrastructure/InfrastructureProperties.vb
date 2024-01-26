''' <summary>
''' インフラストラクチャのプロパティ設定クラス
''' </summary>
Public Class InfrastructureProperties
    Public Shared Sub UpdateSQLServerConectionStringServerName(newValue As String)
        My.Settings.ServerName = newValue
        My.Settings.Save()
    End Sub

    Public Shared Function ReturnServerName() As String
        Return My.Settings.ServerName
    End Function

    Public Shared Sub UpdateDefaultDataBaseName(newValue As String)
        My.Settings.DefaultDataBaseName = newValue
        My.Settings.Save()
    End Sub

    Public Shared Function ReturnDefaultDataBaseName() As String
        Return My.Settings.DefaultDataBaseName
    End Function
End Class
