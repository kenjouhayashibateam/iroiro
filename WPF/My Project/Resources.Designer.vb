﻿'------------------------------------------------------------------------------
' <auto-generated>
'     このコードはツールによって生成されました。
'     ランタイム バージョン:4.0.30319.42000
'
'     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
'     コードが再生成されるときに損失したりします。
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'このクラスは StronglyTypedResourceBuilder クラスが ResGen
    'または Visual Studio のようなツールを使用して自動生成されました。
    'メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    'ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    '''<summary>
    '''  ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("WPF.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        '''  現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  登録しました に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AddComplete() As String
            Get
                Return ResourceManager.GetString("AddComplete", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  登録完了 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AddCompleteTitle() As String
            Get
                Return ResourceManager.GetString("AddCompleteTitle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''   家 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AddHomeString() As String
            Get
                Return ResourceManager.GetString("AddHomeString", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  宛先が不十分です に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AddresseeErrorInfo() As String
            Get
                Return ResourceManager.GetString("AddresseeErrorInfo", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  住所がセルからはみ出てますので、書き直して下さい に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AddressLengthOverInfo() As String
            Get
                Return ResourceManager.GetString("AddressLengthOverInfo", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  面積が正しく入力されていません に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property AreaFieldError() As String
            Get
                Return ResourceManager.GetString("AreaFieldError", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  長3封筒 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property Cho3EnvelopeText() As String
            Get
                Return ResourceManager.GetString("Cho3EnvelopeText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  コピー形式が正しくありません。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ClipBoardDataErrorInfo() As String
            Get
                Return ResourceManager.GetString("ClipBoardDataErrorInfo", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  削除しました に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CompleteDeleteInfo() As String
            Get
                Return ResourceManager.GetString("CompleteDeleteInfo", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  削除完了 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CompleteDeleteInfoTitle() As String
            Get
                Return ResourceManager.GetString("CompleteDeleteInfoTitle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  登録しますか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ConfirmationRegisterInfo() As String
            Get
                Return ResourceManager.GetString("ConfirmationRegisterInfo", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  登録確認 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ConfirmationRegisterInfoTitle() As String
            Get
                Return ResourceManager.GetString("ConfirmationRegisterInfoTitle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  6桁の管理番号を入力 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property CustomerIDLengthError() As String
            Get
                Return ResourceManager.GetString("CustomerIDLengthError", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  どちらのデータを使用しますか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property DataSelectInfo() As String
            Get
                Return ResourceManager.GetString("DataSelectInfo", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  データ選択 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property DataSelectInfoTitle() As String
            Get
                Return ResourceManager.GetString("DataSelectInfoTitle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  1900/01/01 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property DefaultDate() As String
            Get
                Return ResourceManager.GetString("DefaultDate", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  削除しますか？ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property DeleteInfo() As String
            Get
                Return ResourceManager.GetString("DeleteInfo", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  削除確認 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property DeleteInfoTitle() As String
            Get
                Return ResourceManager.GetString("DeleteInfoTitle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  登録エラー に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ErrorRegisterTitle() As String
            Get
                Return ResourceManager.GetString("ErrorRegisterTitle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  面積 :  に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property FieldPropertyMessage_Area() As String
            Get
                Return ResourceManager.GetString("FieldPropertyMessage.Area", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  契約内容 :  に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property FieldPropertyMessage_ContractContent() As String
            Get
                Return ResourceManager.GetString("FieldPropertyMessage.ContractContent", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  管理番号 :  に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property FieldPropertyMessage_CustomerID() As String
            Get
                Return ResourceManager.GetString("FieldPropertyMessage.CustomerID", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  苗字 :  に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property FieldPropertyMessage_FirstName() As String
            Get
                Return ResourceManager.GetString("FieldPropertyMessage.FirstName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  墓地番号 :  に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property FieldPropertyMessage_GraveNumber() As String
            Get
                Return ResourceManager.GetString("FieldPropertyMessage.GraveNumber", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  名義人 :  に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property FieldPropertyMessage_Lessee() As String
            Get
                Return ResourceManager.GetString("FieldPropertyMessage_Lessee", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  送付先 :  に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property FieldPropertyMessage_Receiver() As String
            Get
                Return ResourceManager.GetString("FieldPropertyMessage_Receiver", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  登録日時 :  に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property FieldPropertyMessage_RegistrationDate() As String
            Get
                Return ResourceManager.GetString("FieldPropertyMessage.RegistrationDate", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  形式エラー に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property FormatErrorTitle() As String
            Get
                Return ResourceManager.GetString("FormatErrorTitle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  番 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property GraveBanString() As String
            Get
                Return ResourceManager.GetString("GraveBanString", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  側 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property GraveGawaString() As String
            Get
                Return ResourceManager.GetString("GraveGawaString", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  区 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property GraveKuString() As String
            Get
                Return ResourceManager.GetString("GraveKuString", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  墓地パンフ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property GravePamphletEnvelopeText() As String
            Get
                Return ResourceManager.GetString("GravePamphletEnvelopeText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  様 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property HonorificsText() As String
            Get
                Return ResourceManager.GetString("HonorificsText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  角2封筒 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property Kaku2EnvelopeText() As String
            Get
                Return ResourceManager.GetString("Kaku2EnvelopeText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  ラベル用紙 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LabelPaperText() As String
            Get
                Return ResourceManager.GetString("LabelPaperText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  はい ⇒　名義人　　いいえ ⇒ 送付先 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property LesseeDataSelect() As String
            Get
                Return ResourceManager.GetString("LesseeDataSelect", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  必須項目不備 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MandatoryItemsNotPreparedTitle() As String
            Get
                Return ResourceManager.GetString("MandatoryItemsNotPreparedTitle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  金額は半角数字で入力してください に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property MoneyInputError() As String
            Get
                Return ResourceManager.GetString("MoneyInputError", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  アイテムが選択されないまま画面を閉じます。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property NoAddressItemCloseInfo() As String
            Get
                Return ResourceManager.GetString("NoAddressItemCloseInfo", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  アイテムが選択されていません に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property NothingSelectedItemMessage() As String
            Get
                Return ResourceManager.GetString("NothingSelectedItemMessage", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  チェックの入った墓地札データをエクセルに出力しました に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property OutputInfo() As String
            Get
                Return ResourceManager.GetString("OutputInfo", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  印刷するアイテムがありません。印刷フラグにチェックを入れるか、墓地札を追加してください に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property OutputInfo_Count0() As String
            Get
                Return ResourceManager.GetString("OutputInfo_Count0", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  出力結果 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property OutputInfoTitle() As String
            Get
                Return ResourceManager.GetString("OutputInfoTitle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  宛名、郵便番号、住所、番地の順で作ったリストをコピーしてください。次のレコードに進みます。 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property PassAddresseeRecordInfo() As String
            Get
                Return ResourceManager.GetString("PassAddresseeRecordInfo", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  半角数字、ハイフンあり（***-****）で入力してください に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property PostalCodeError() As String
            Get
                Return ResourceManager.GetString("PostalCodeError", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  はがき に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property PostcardText() As String
            Get
                Return ResourceManager.GetString("PostcardText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''   /  に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property SlashClipSpace() As String
            Get
                Return ResourceManager.GetString("SlashClipSpace", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''   ㎡ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property SquareFootageText() As String
            Get
                Return ResourceManager.GetString("SquareFootageText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  必須項目です。必ず入力してください に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property StringEmptyMessage() As String
            Get
                Return ResourceManager.GetString("StringEmptyMessage", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  要住所調整 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ToBeAdjusted() As String
            Get
                Return ResourceManager.GetString("ToBeAdjusted", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  振込用紙 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property TransferPaperText() As String
            Get
                Return ResourceManager.GetString("TransferPaperText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  未登録 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property UndefinedCustomerID() As String
            Get
                Return ResourceManager.GetString("UndefinedCustomerID", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  洋封筒 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property WesternEnvelopeText() As String
            Get
                Return ResourceManager.GetString("WesternEnvelopeText", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
