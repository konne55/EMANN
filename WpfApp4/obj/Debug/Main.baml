   M S B A M L   `   `   `  ����@  <EMANN, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:EMANeuralNetwerkEMANeuralNetwerk  5   	       EMANeuralNetwerk.Main   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:EMANeuralNetwerk     5   	     � Title$  emaNN��5   	   $��Light"�6   ���	#FF00AEFF$
��1920��6A   $
��1080��6N    � Icon$ nfc_n_100_gEp_icon.ico��6\    � WindowState$ 	Maximized=�6z   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $	��65* �6   �� 5      $
��413* �6    � RowDefinitions 5   
   �� 5      $	��24* �6   �� 5      $
��961* �6   �� 5      $	��65* �6   .+�5      �� -   5         ��Name$ MainTabControl��5      $��1��61   $��1��6B   ��6O   �� *Tabs+��5      �� 5      .R�5      �� ��5      �� *Name+5       �� ContentTemplate 5      �� 5      .R�5!      �  5"      �� 5#      $	��60* �6,   �� 5$      $	��30* �6,   �� 5%      $	��10* �6,    5'      �� 5(      $	��40* �6/   �� 5)      $	��60* �6/   .+�5-      �� $��0��6%   $��0��62   .@�5.      �� $��Fehlergraph��6&   .��5/   "   �� $��
errorGraph��6)   
�������.+�50   &   �� ��60   �� *Name+51   #   52      �� 53      $��Lernrate��6&   .��54   "   �� $��
alphaGraph��6)   
�������6;   55      56      �� 59      $��1��6%   $��0��62    �� Orientation$ Vertical=�6B   .+�5:      �� $��Globaler Fehler: ��6(   �� 5;      $��Aktuelle Lernrate:  ��6(   �� 5<       � Minimum$ 0.0001Q�6%   	 � Maximum$		 1.5Q�66   
 � Value$

  0.5Q�6D   5=      �� 5?      $�� 2��6%   $��0��63   .+�5@      �� $��Netzkonfiguration: ��6(   �� 5A      $��Evolutionsstufe: ��6(   5B      �� 5D      $�� 0��6!   $��3��6/   $�� 1��6@   
�������6l   5E      5H      5J      c� 5R   
   $��1��6   $
��Left=�6"   .@�5S      m� $��25��6    m� Icon 5T      �� 5U      $��img/neuron.png��6   5W      m� 5X      $��25��6    5Y      �� 5Z      $��img/synapse.png��6   5\      5]      �� 5_   
   -   5        $��listBoxNetworks��5_   !   $��1��6   ��5`      �� 5a      .R�5b      �� $ 
Horizontal=�6!   .+�5c      �� ��6$   �� *Name+�� 5d      $�� ��6$   �� 5e      ��6$   �� *Epoch+5f      5g      5i      �� 5j   
   -   5        $��Button��5j      $��1��6"   $
��Left=�62   $��	591,2,0,0q�6M   $��75��6`   6k   5k      5l      