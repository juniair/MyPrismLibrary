   M S B A M L   `   `   `  ����B  >TestApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:TestApp.ViewsTestApp.Views  5   	   7 clr-namespace:TestApp.ViewModelsTestApp.ViewModels  5   	       TestApp.Views.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -localclr-namespace:TestApp.Views     5   	   4localvm clr-namespace:TestApp.ViewModels     5   	   P LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5   	   !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  . $	��750��5   "   $	��528��6.    � Title 6	   �� *Title+ � 	Resources 5	         RedBrush �� Color  SolidBorderBrush  
GreenBrush  DisabledBackgroundBrush  DisabledBorderBrush  DisabledForegroundBrush �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius	 ��Name
 �� Triggers P� Property P� Value �� 
TargetName��%  &	        &	      &	 ;     &	 Z     &	 x     &	 �     '�� �     7   7  7  �� 5
   
   $	 Red��6+   �� 5   
   $
 #888��63   �� 5   
   $ Green��6-   �� 5   
   $
 #EEE��6:   �� 5   
   $
 #AAA��66   �� 5   
   $
 #888��6:   �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $��Border��5   !   $��0,0,-4,0q�5   !   8��  8�� $��1,1,1,1q�5    !   $ 2,12,0,0��5!   !   ._�5"   "   �� $	 ContentSite��63   $��Center=�5#   %   $��Center=�5$   %   $��Header��5%   %   $��	12,2,12,2q�5&   %   ��. 5'   %   5(      5)      
 5*      P� 5+       �@��
IsSelected . . �5,   "   ��  �@K�ZIndex$	 100��6A   �� 5-   "   $ Border��6)   8   � ��6S   �� 5.   "   $ Border��6)    � ��$ 1,1,1,0q�6X   5/      52      54      55      56      .��57      � .+�58   
   �� .@�59      �� $��	MyTabItem��6   �� 5:      $��	MyTabItem��6   5;      5<      5X      