   M S B A M L   `   `   `  ����?  ;EDit, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:EDitEDit  5   	       EDit.DecryptionWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:EDit     5   	     � Title$  DecryptionWindow��5   	   $	��353��6"   $	��525��6/    � 
ResizeMode$ NoResize=�6;   .��5	      � .+�5
   
   �� -   5         ��Name$ tb_descryptKey��5
      $
��Left=�6v   $	��171��6�   $��	13,96,0,0q�6�    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��484��6�   ��6*   ��  �� Path$	 Key� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5   
   $��Private Key:��6   $
��Left=�6'   $��	13,69,0,0q�6B   $	��Top=�6U   $��70��6m    J� RenderTransformOrigin$ 	0.5,0.769&�6x   �� 5   
   -   5        $ tb_manifestPath��5      $
��Left=�6�   $��23��6�   $��	13,45,0,0q�6�   $
 Wrap=�6�   $	��Top=�6�   $	��446��6�   ��6+   �� $ ManifestFilePath�$ TwoWay=�$ PropertyChanged=�6�   �� 5   
   -   5         ��Name$ bt_selManifest��5      $	��...��6)   $
��Left=�67   $��
466,45,0,0q�6R   $	��Top=�6f   $��31��6~   $��23��6�   $ 0.742,-0.435&�6�   �� 5   
   $��Manifest file path:��6   $
��Left=�6.   $��	13,17,0,0q�6I   $	��Top=�6\   �� 5   
   -   5        $ bt_importDescryptKey��5      $��Import Private Key��6/   $
��Left=�6L   $��26��6g   $��200,275,0,0q�6s   $	��Top=�6�   $	��105��6�   $ -0.877,0.645&�6�   6�   �� 5   
   -   5        $ bt_OK��5      $��OK��6    $
��Left=�6-   $��321,275,0,0q�6H   $	��Top=�6]   $��75��6u   $��26��6�   	 �� 	IsDefault	 . 6�   �� 5   
   $��Cancel��6   $
��Left=�6"   $��422,275,0,0q�6=   $	��Top=�6R   $��75��6j   $��26��6u   
 �� IsCancel
 . 6�   5      5      