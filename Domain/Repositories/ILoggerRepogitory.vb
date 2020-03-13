''' <summary>
''' ログを保管します
''' </summary>
Public Interface ILoggerRepogitory

    Enum LogInfo
        INFOMATION
        ERR
    End Enum

    Sub Log(ByVal _loginfo As LogInfo, ByVal message As String)

End Interface
