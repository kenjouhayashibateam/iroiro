''' <summary>
''' ログを保管します
''' </summary>
Public Interface ILoggerRepogitory

    Enum LogInfo
        INFOMATION
        ERR
    End Enum

    Sub Log(_loginfo As LogInfo, message As String)

End Interface
