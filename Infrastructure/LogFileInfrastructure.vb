Imports Domain

''' <summary>
''' メモ帳にログを書き込みます
''' </summary>
Public Class LogFileInfrastructure
    Implements ILoggerRepogitory

    Public Sub Log(_loginfo As ILoggerRepogitory.LogInfo, message As String) Implements ILoggerRepogitory.Log

        Using writer = New IO.StreamWriter(My.Resources.LogFilePath, True)
            writer.WriteLine($"{_loginfo}{vbTab}{Now}{vbTab}{message}")
            writer.Flush()
        End Using

    End Sub

End Class
