   M S B A M L   `   `   `  ����M  Iいろいろ発行, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WPF.ViewModelsWPF.ViewModels  5   	   clr-namespace:WPFWPF  5   	   -clr-namespace:WPF.BehaviorsWPF.Behaviors  5   	       WPF.AddressDataView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +vmclr-namespace:WPF.ViewModels     5   	   #localclr-namespace:WPF     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a4ib+http://schemas.microsoft.com/xaml/behaviors  5   	   c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5	   	   h dMicrosoft.Expression.Interactions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Linteractions9http://schemas.microsoft.com/expression/2010/interactions  5
   	   #bclr-namespace:WPF.Behaviors  5   	     � Title$$  住所を選択して下さい��5   	   $��221.935��6   $��332.834��6-    � WindowStartupLocation$ CenterOwner=�6=    � DataContext 5      )   #WPF.ViewModels.AddressDataViewModel  5   
   .  (System.Windows.Interactivity.Interaction   Triggers 5      D  >Microsoft.Expression.Interactivity.Core.PropertyChangedTrigger  5   
      Binding 6.   �� *MessageInfo+.  (System.Windows.Interactivity.TriggerBase   Actions. 5      &    WPF.Behaviors.MessageBoxBehavior  5      *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5      /  )Microsoft.Xaml.Behaviors.Core.DataTrigger  5   
      Value$
 True��6>   :  4Microsoft.Xaml.Behaviors.Core.PropertyChangedTrigger   Binding 6   �� *CallCloseMessage+*	  $Microsoft.Xaml.Behaviors.TriggerBase	 	  Actions.	 5      2
  ,Microsoft.Xaml.Behaviors.InvokeCommandAction
  
 
  Command
 6%   �� *NoSelectedCloseMessage+5      .��5      �  � RowDefinitions 5   
   �� 5       $
��0.8* �6   �� 5!      $
��0.2* �6   .+�5#   
   &   System.Windows.Controls.DataGrid     AutoGenerateColumns .  $��0��5$      $��Stretch=�6&   $��Stretch=�6E   ��5#      �� *MyAddressList.MyList+ �� SelectedItem 5$   a   �� *	MyAddress+   Columns 5%      0  *System.Windows.Controls.DataGridTextColumn  5&      ,  &System.Windows.Controls.DataGridColumn   Header$ 郵便番号��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.2* 63   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6@   �� *MyPostalcode.Code+  5'      $ 住所��6%   $
 0.8* 61    6>   �� *MyAddress.Address+5)      �� 5*   
   -   5         ��Name$ 
ExitButton��5*      $��1��6%   $��Stretch=�62   $��Stretch=�6P   .��6l   MK閉じる（選択している住所が基の画面に反映されます） 5,      +  %Microsoft.Xaml.Behaviors.EventTrigger  5-         	EventName$ Click��6"   .	 5.      
  
 6-   �� *SetAddressDataCommand+5/      51      52      54      