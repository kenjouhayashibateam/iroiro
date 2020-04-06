Class Application

    ' Startup、Exit、DispatcherUnhandledException などのアプリケーション レベルのイベントは、
    ' このファイルで処理できます。

    Private m As New System.Threading.Mutex(False, "ApplicationName")
    Private w As MainWindow = Nothing

    Private Sub Application_Startup(sender As System.Object, e As System.Windows.StartupEventArgs)

        'ミューテックスの所有権を要求
        If m.WaitOne(0, False) = False Then
            '既に起動していると判断し終了する
            MessageBox.Show("既に起動しています。")
            'Mutexを破棄
            m.Close()
            m = Nothing
            'アプリケーション終了
            Me.Shutdown()
        Else
            'メイン画面を手動で初期化する
            w = New MainWindow
            'メイン画面を起動
            Call w.Show()
        End If

    End Sub

    Private Sub Application_Exit(sender As System.Object, e As System.Windows.ExitEventArgs)
        'ミューテックスの存在確認
        If m IsNot Nothing Then
            '存在する場合は破棄する
            m.ReleaseMutex()
            m.Close()
            m = Nothing
        End If
    End Sub

End Class
