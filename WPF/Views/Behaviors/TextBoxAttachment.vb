Public Class TextBoxAttachment
    Inherits DependencyObject

    Public Shared IsSelectAllOnGotFocus As DependencyProperty
    Event GotFocus(obj As Object)
    Shared Sub New()
        Dim metaData As New FrameworkPropertyMetadata
        Dim propertyChangedCallback As PropertyChangedCallback = Sub(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
                                                                     IsSelectAllOnGotFocus = DependencyProperty.RegisterAttached("IsSelectAllOnGotFocus", GetType(Boolean),
                                                                             GetType(TextBoxAttachment), New PropertyMetadata(False, propertyChangedCallback))
                                                                     If d Is Nothing Then Return
                                                                     If (TypeOf d IsNot TextBox) Then Return
                                                                     Dim tb As TextBox = d
                                                                     If tb Is Nothing Then Return
                                                                     If (TypeOf e.NewValue Is Boolean) Then Return
                                                                     'RaiseEvent GotFocus(tb)
                                                                     'tb.GotFocus -= OnTextBoxGotFocus;
                                                                     '                tb.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
                                                                     '                If (isSelectAll) Then
                                                                     '                                        {
                                                                     '                    tb.GotFocus += OnTextBoxGotFocus;
                                                                     '                    tb.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
                                                                 End Sub
    End Sub

    ''' <summary>
    ''' GotFocus時にテキストをすべて選択にするかを返します
    ''' </summary>
    ''' <param name="obj">対象のテキストボックス</param>
    ''' <returns></returns>
    Public Shared Function GetIsSelectAllOnGotFocus(obj As DependencyObject) As Boolean
        Return obj.GetValue(IsSelectAllOnGotFocus)
    End Function
    ''' <summary>
    ''' GotFocus時にテキストをすべて選択した状態にします
    ''' </summary>
    ''' <param name="obj">対象のテキストボックス</param>
    ''' <param name="value"></param>
    Public Shared Sub SetIsSelectAllOnGotFocus(obj As DependencyObject, value As Boolean)
        obj.SetValue(IsSelectAllOnGotFocus, value)
    End Sub

    'Public Shared Function IsSelectAllOnGotFocusProperty() As DependencyProperty =
    '        DependencyProperty.RegisterAttached("IsSelectAllOnGotFocus", GetType(Boolean),
    '            GetType(TextBoxAttachment), New PropertyMetadata(False, (d as dele, e) >=
    '            {
    '                If(d = null Or e = null) { Return; }
    '                If (!(d Is TextBox tb)) Then { Return; }
    '                If (tb == null) Then { Return; }
    '                If (!(e.NewValue Is bool isSelectAll)) Then { Return; }

    '                tb.GotFocus -= OnTextBoxGotFocus;
    '                tb.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
    '                If (isSelectAll) Then
    '                                        {
    '                    tb.GotFocus += OnTextBoxGotFocus;
    '                    tb.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
    '                }
    '            }));
    '    End Function

    'Private Static void OnTextBoxGotFocus(Object sender, RoutedEventArgs e)
    '    {
    '        If (!(sender Is TextBox tb)) { Return; }
    '        bool isSelectAllOnGotFocus = GetIsSelectAllOnGotFocus(tb);

    '        If (isSelectAllOnGotFocus)
    '        {
    '            tb.SelectAll();
    '        }
    '    }

    '    Private Static void OnMouseLeftButtonDown
    '        (object sender, System.Windows.Input.MouseButtonEventArgs e)
    '    {
    '        If (!(sender Is TextBox tb)) { Return; }

    '        If (tb.IsFocused) { Return; }
    '        _ = tb.Focus();
    '        e.Handled = true;
    '    }
End Class
