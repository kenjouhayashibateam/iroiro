   M S B A M L   `   `   `  ����M  Iいろいろ発行, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:WPFWPF  5   	   /clr-namespace:WPF.ViewModelsWPF.ViewModels  5   	   -clr-namespace:WPF.BehaviorsWPF.Behaviors  5   	   X TXceed.Wpf.Toolkit, Version=3.8.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4P:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.ToolkitXceed.Wpf.Toolkit 5   	   "    WPF.CreateGravePanelDataView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:WPF     5   	   +vmclr-namespace:WPF.ViewModels     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a4ib+http://schemas.microsoft.com/xaml/behaviors  5   	   c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5	   	   h dMicrosoft.Expression.Interactions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Linteractions9http://schemas.microsoft.com/expression/2010/interactions  5
   	   #bclr-namespace:WPF.Behaviors  5   	   Eextk:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.Toolkit  5   	     � Title$  墓地札新規登録��5   	   $	��400��6   $	��600��6&    � WindowStartupLocation$ CenterOwner=�62    +� FocusedElement 6W   ��  �� ElementName$ CustomerIDTextBox�� � DataContext 5      2   ,WPF.ViewModels.CreateGravePanelDataViewModel  5   
   .  (System.Windows.Interactivity.Interaction   Triggers 5      D  >Microsoft.Expression.Interactivity.Core.PropertyChangedTrigger  5   
      Binding 6.   �� *MessageInfo+.  (System.Windows.Interactivity.TriggerBase   Actions. 5      &    WPF.Behaviors.MessageBoxBehavior  5      *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5      /  )Microsoft.Xaml.Behaviors.Core.DataTrigger  5   
   	   Value$
	 True��6E   :  4Microsoft.Xaml.Behaviors.Core.PropertyChangedTrigger
   Binding
 6   �� *CallSelectAddresseeInfo+*	  $Microsoft.Xaml.Behaviors.TriggerBase 	  Actions. 5      2
  ,Microsoft.Xaml.Behaviors.InvokeCommandAction
   
  Command 6%   �� *SelectAddresseeInfo+5        5   
   $
	 True��6D   
 6   �� *IsConfirmationRegister+. 5       
   6%   �� *ConfirmationRegistraterInfo+5!        5"   
   $
	 True��6F   
 6   �� *CallCompleteRegistration+. 5#      
   6%   �� *CompleteRegistrationInfo+5$        5%   
   $
	 true��6N   
 6   �� *" CallRegistrationErrorMessageInfo+. 5&      
   6%   �� *RegistrationErrorMessageInfo+5'      .��5*      � -   5         �Name$ MainGrid��5*       � RowDefinitions 5+   
   �� 5,      $��1* �6   �� 5-      $
��1.4* �6   �� 5.      $
��0.4* �6   .+�50   
   �� $��検索メニュー��6   $��0��6#   $��Stretch=�60   $��Stretch=�6N   $��5q�6j   .��51      � -   5        $ ReferenceGraveMenu��51      $��Stretch=�6/   $��0��6K   $��0��6[    52      �� 53      $��1* �6$   �� 54      $��1* �6$   .+�56      � $��Stretch=�6   $��Stretch=�66   $��0��6R   $��0��6b    � ColumnDefinitions 57      �� 58      $��1* �6+   �� 59      $
��0.8* �6+   �� 5:      $
��0.3* �6+   �� 5;      $
��1.2* �6+   �� 5<      $
��0.3* �6+   �� 5=      $
��1.2* �6+   �� 5>      $
��0.8* �6+   �� 5?      $
��0.3* �6+   .+�5A      �� -   5         ��Name$ GraveNumberKuComboBox��5A      $��0��6>   $��Stretch=�6N   $��Stretch=�6l   $��
15,10,5,10q�6�   $��Center=�5B       �� DisplayMemberPath$ DisplayForField��6k    �� SelectedValuePath$ 	CodeField��6�   ��6A   �� *GraveNumberKuList+ �� SelectedValue 6�   �� *
SelectedKu+ �� Text 6�   �� *KuText+ �� Mode$ OneWayToSource=��� 5C      -   5        $ GraveNumberKuikiComboBox��5C      $��1��6A   $��Stretch=�6Q   $��Stretch=�6o   $ DisplayForField��6�   $ 	CodeField��6	  $
��5,10q�5D      $��Center=�6,   ��5C   �   �� *GraveNumberKuikiList.List+ 6�   �� *SelectedKuiki+ 6'  �� *	KuikiText+$ OneWayToSource=�{�6V  �� *IsEnabledKuiki+5D   ,   �� 5E      $��2��6   $
��Left=�6,   $��Center=�6G   $	��区��6b   $��16(�6n   �� 5F      -   5        $ GraveNumberGawaComboBox��5F      $��3��6@   $��Stretch=�6P   $��Stretch=�6n   $ DisplayForField��5G      $ 	CodeField��6j   $��
15,10,5,10q�6�   $��Center=�6�   ��5F   �   �� *GraveNumberGawaList.List+ 5G   C   �� *SelectedGawa+{�6�   �� *IsEnabledGawa+ 6�   �� *
GawaText+$ OneWayToSource=�6�   �� 5H      $��4��6   $
��Left=�6,   $��Center=�6G   $	��側��6b   $��16(�6n   �� 5I      -   5        $ GraveNumberBanComboBox��5I      $��5��6?   $��Stretch=�6O   $��Stretch=�6m   $ DisplayForField��5J      $ 	CodeField��6i   $��
15,10,5,10q�6�   $��Center=�6�   ��5I   �   �� *GraveNumberBanList.List+ 5J   C   �� *SelectedBan+{�6�   �� *IsEnabledBan+ 6�   �� *	BanText+$ OneWayToSource=�6�   )  #Xceed.Wpf.Toolkit.WatermarkComboBox  5K      -   5          Name$ GraveNumberEdabanComboBox��5K   -   $��6��6P   $��Stretch=�6`   $��Stretch=�6~   $ DisplayForField��5L   -   $ 	CodeField��6z   $
��5,10q�6�      	Watermark$ 枝番��6�   $��Center=�6  ��5K   �   �� *GraveNumberEdabanList.List+ 5L   Q   �� *SelectedEdaban+{�6�   �� *IsEnabledEdaban+ 6�   �� *
EdabanText+$ OneWayToSource=�6  �� 5M      $��7��6   $
��Left=�6,   $��Center=�6G   $	��番��6b   $��16(�6n   5N      � 5O      $��1��6   $��Stretch=�6$   $��Stretch=�6B    5P      �� 5Q      $��1* �6+   �� 5R      $��1* �6+   �� 5S      $��3* �6+   .+�5U      (  "Xceed.Wpf.Toolkit.WatermarkTextBox  -   5          Name$ CustomerIDTextBox��5U   ,   $��0��6G    �� 	MaxLength$ 6��6W   $��Stretch=�6e   $��Center=�6�   $��Stretch=�6�      	Watermark$ 管理番号��6�   $��
15,10,5,10q�6�    �� PreferredImeState$	 Off=�6&  ��6�   �� *
CustomerID+ �� UpdateSourceTrigger$ PropertyChanged=�6&  �� 5V      -	   5         ��Name$ ReferenceButton��5V      $��1��66   $��80��6F   $��30��6Q   $��3,5,10,5q�6]    �� 	IsDefault . $��管理番号検索��6�    5W      +  %Microsoft.Xaml.Behaviors.EventTrigger  5X         	EventName$ Click��6.   . 5Y   "   
   69   �� *ReferenceGraveNumberCommand+5Z      5\      5]      5^      5_      �� 5`   
   $��登録墓地札データ��6   $��1��6%   $��Stretch=�62   $��Stretch=�6P   $	��5,0q�6l   .��5a      � $��1��6   $��Stretch=�6    $��Stretch=�6>    5b      �� 5c      $��1* �6$   �� 5d      $��1* �6$   .+�5f      � $��Stretch=�6   $��Stretch=�65   $��0��6Q    5g      �� 5h      $��1* �6(   �� 5i      $��2* �6(   .+�5k      � $��Stretch=�6   $��Stretch=�69   $��0��6U    5l      �� 5m      $��2* �6/   �� 5n      $��1* �6/   �� 5o      $��5* �6/   .+�5q      �� $��管理番号��6    $��0��6/   $
��Left=�6?   $��Bottom=�6Z   �� 5r      $��墓地番号��6    $��1��6/   $
��Left=�6?   $��Bottom=�6Z   5t      � 5u      -
   5        $ GraveNumberGrid��5u      $��1��64   $��Stretch=�6A   $��Stretch=�6_   $��5q�6{    5v      �� 5w      $
��140* �6/   �� 5x      $	��70* �6/   �� 5y      $��85 �6/   �� 5z      $	��28* �6/   �� 5{      $	��99* �6/   �� 5|      $	��28* �6/   �� 5}      $	��71* �6/   �� 5~      $	��70* �6/   �� 5      $	��28* �6/   .+�5�        -   5        $ CustmerIDAddTextBox��5�   0   $��0��6M   $��Stretch=�6]   $��Stretch=�6{   $ 6��6�   $ 管理番号��6�   $��Right=�6�   $��Center=�6�   $��	5,0,15,10q�6#  $��16(�66  $	 Off=�6D  ��6�   �� *RegistraterCustomerID+6D    5�      -   5        $ GraveNumberKuTextBox��5�   0   $��1��6N   $��Stretch=�6^   $��Stretch=�6|   $	 区��6�   $��Right=�6�   $��Center=�6�   $��	10,0,3,10q�6  $��16(�6  ��6�   �� *KuText+6    5�      -   5        $ GraveNumberKuikiTextBox��5�   0   $��2��6Q   $��Stretch=�6a   $��Stretch=�6   $ 区域��6�   $��Right=�6�   $��Center=�6�   $��3,0,3,10q�6
  $��16(�6  ��6�   �� *	KuikiText+6  �� 5�      $	��区��6    $��3��6-   $
��Left=�6=   $��Center=�6X     5�      -   5        $ GraveNumberGawaTextBox��5�   0   $��4��6P   $��Stretch=�6`   $��Stretch=�6~   $	 側��6�   $��Right=�6�   $��Center=�6�   $��5,0,5,10q�6  $��16(�6  ��6�   �� *
GawaText+6  �� 5�      $	��側��6    $��5��6,   $
��Left=�6<   $��Center=�6W     5�      -   5        $ GraveNumberBanTextBox��5�   0   $��6��6O   $��Stretch=�6_   $��Stretch=�6}   $	 番��6�   $��Right=�6�   $��Center=�6�   $��5,0,5,10q�6  $��16(�6  ��6�   �� *	BanText+6    5�      -   5        $ GraveNumberEdabanTextBox��5�   0   $��7��6R   $��Stretch=�6b   $��Stretch=�6�   $ 枝番��6�   $��Right=�6�   $��Center=�6�   $��5,0,5,10q�6  $��16(�6  ��6�   �� *
EdabanText+6  �� 5�      $	��番��6    $��8��6,   $
��Left=�6<   $��Center=�6W   5�      5�      � 5�      $��Stretch=�6   $��Stretch=�65   $��1��6Q    5�      �� 5�      $��1* �6(   �� 5�      $��2* �6(   .+�5�      � $��Stretch=�6   $��Stretch=�69   $��0��6U    5�      �� 5�      $��7* �6/   �� 5�      $��3* �6/   �� 5�      $��1* �6/   �� 5�      $��1* �6/   .+�5�      �� $��申込氏名等��6    $��0��60   $
��Left=�6@   $��Bottom=�6[   �� 5�      $��1��6    $
��Left=�60   $��Bottom=�6K   $��契約内容��6f   �� 5�      $��2��6    $��Right=�60   $��Bottom=�6L   $��面積��6g   5�      � 5�      -   5        $ DataContentsMenuGrid��5�      $��1��69   $��Stretch=�6F   $��Stretch=�6d    5�      �� 5�      $
��112* �6/   �� 5�      $	��25* �6/   �� 5�      $
��150* �6/   �� 5�      $
��150* �6/   �� 5�      $	��45* �6/   �� 5�      $	��25* �6/   .+�5�        -   5        $ FamilyNameTextBox��5�   0   $��0��6K   $��Stretch=�6[   $��Center=�6y   $��Stretch=�6�   $ 苗字��6�   $��	10,5,5,15q�6�   $��16(�6�   $ On=�6  !  �� PreferredImeConversionMode$  FullShape,native=�6&  ��6�   �� *
FamilyName+6&  �� 5�      $��1��6    $
��Left=�60   $��Center=�6K   $	��家��6f   $��16(�6r   $
��0,11q�6�     5�      -   5        $ FullNameTextBox��5�   0   $��2��6I   $��Stretch=�6Y   $��Stretch=�6w   $��Center=�6�   $ 申込者名��6�   $��
10,5,10,15q�6�   $��16(�6�   $ On=�6  $  FullShape,native=�6%  ��6�   �� *
FullName+6%  �� 5�      -   5        $ ContractDetailComboBox��5�   #   $��3��6C   $��Stretch=�6S   $��Stretch=�6q   ! �� 
IsEditable! . $
��Left=�6�   $��Center=�6�   $��16(�62  $��
10,5,10,15q�6@  $ On=�6T  $  FullShape,native=�6w  ��6�   �� *ContractContents.List+ 6  �� *ContractContent+6w    5�      -   5        $ AreaTextBox��5�   0   $��4��6E   $��Stretch=�6U   $��Stretch=�6s   $ 面積��6�   $��Center=�6�   $��Center=�6�   $��5,5,5,15q�6�   $��16(�6  $	 Off=�6  ��6�   �� *Area+6  �� 5�      $��5��6    $
��Left=�60   $��Center=�6K   $	��㎡��6f   $��16(�6r   $
��0,11q�6�   5�      5�      5�      5�      � 5�   
   -   5        $ AddItemGrid��5�      $��2��6$   $��Stretch=�61   $��Stretch=�6O    5�      �� 5�      $��2* �6#   �� 5�      $��1* �6#   �� 5�      $��1* �6#   �� 5�      $��1* �6#   .+�5�      �� -   5        $ 
ExitButton��5�      $��3��6)   $��80��69   $��30��6D   $��Right=�6P   $
��10,0q�6l   $��	閉じる��6z   �� 5�      -   5        $ ExecuteButton��5�      $��2��6,   $��80��6<   $��30��6G   $��Right=�6S   $��	10,0,10,0q�6o   .��6�   追加 5�        5�      $ Click��6&   . 5�      
   61   �� *GravePanelDataRegistration+5�      5�      5�      5�      5�      