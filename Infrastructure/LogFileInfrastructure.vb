Imports Domain

''' <summary>
''' メモ帳にログを書き込みます
''' </summary>
Public Class LogFileInfrastructure
    Implements ILoggerRepogitory

    Sub Log(ByVal _loginfo As ILoggerRepogitory.LogInfo, ByVal message As String) Implements ILoggerRepogitory.Log

        Using writer = New System.IO.StreamWriter("./logs/ExceptionLogs.txt", True)
            writer.WriteLine($"{_loginfo}{vbTab}{Now}{vbTab}{message}")
            writer.Flush()
        End Using

    End Sub

End Class
