   M S B A M L   `   `   `  ����  Org.Limingnihao.Update)    #Org.Limingnihao.Update.UpdateWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   >d2http://schemas.microsoft.com/expression/blend/2008    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Icon$)  #/Resources/Images/Icon/logo_128.png��5   	    � WindowState$ 	Maximized=�5   	    � 
ResizeMode$ NoResize=�6"    � SizeToContent$ Manual=�68    � WindowStyle$
 None=�6O    � AllowsTransparency . 
������  � Topmost .   � Title$ 升级程序��5      .��5	      � .+�5
   
   � $	��430��6   $	��253��6   $��Center=�6)   $��Center=�6F   ��5      �� 5       �� ImageSource$% Resources/images/bg/bg_body.png��6   	 j� TileMode$
	 None=�6K   
 j� Stretch$

 None=�6[   .+�5      �� -   5        $��	0,10,10,0q�5      $��Right=�6(   $	��Top=�6D      style_button_close#��[  6\   6�   �� 5      $
��Left=�6   $	��Top=�64   $��	30,80,0,0q�6L   .+�5      �� -   5        $��label_version��5        style_label_config#��[ 6-   $!��版本升级：检测中...��6Y   �� 5      $
��Left=�6   $��Center=�68    �� Orientation$ 
Horizontal=�6S   $��0,10,0,0q�6l   .+�5      �� $��26��6   $��22��6'   $��0,0,0,0q�63   $
��Left=�6D   $��Center=�6_   $(��"Resources/Images/Button/update.png��6z   �� 5      -   5        $��label_message��5      #��[ 61   $��22��6]   $��	10,0,10,0q�6i   $
��Left=�6|   $��Center=�6�   $*��$正在下载更新包，请稍后...��6�   5      � 5      -   5        $��progressBar��5      $
��Left=�61   $��0,10,0,0q�6L   $	��370��6^   $��8��6j    � IsIndeterminate .  ���#394165���#e64c66$��0q�6�   5      �� 5      -   5        $��button_confirm��5        style_button_confirm#��[ 6+   $��	0,150,0,0q�6Y   $��确定��6l   5      5      5      