   M S B A M L   `   `   `  ���� 
  Update)    #Org.Limingnihao.Update.UpdateWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    >d2http://schemas.microsoft.com/expression/blend/2008    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     � Icon$'  !/Resources/Images/Icon/update.png�� � WindowState$ 	Maximized=� � 
ResizeMode$ NoResize=� � SizeToContent$ Manual=� � WindowStyle$
 None=� � AllowsTransparency . 
������  � Topmost .   � Title$ 升级程序��.��� .+�� $	��500��$	��280��$��Center=�$��Center=�����  �� ImageSource$% Resources/images/bg/bg_body.png��	 j� TileMode$	 FlipXY=�
 j� Stretch$

 Fill=�.+��� -   $��	0,10,10,0q�$��Right=�$	��Top=�   style_button_close#��[  �� $
��Left=�$	��Top=�$��	30,80,0,0q�.+��� -   $��label_version��  style_label_config#��[ $!��版本升级：检测中...���� $
��Left=�$��Center=� �� Orientation$ 
Horizontal=�$��0,10,0,0q�.+��� $��26��$��22��$��0,0,0,0q�$
��Left=�$��Center=�$(��"Resources/Images/Button/update.png���� -   $��label_message��  style_text_message#��[ $	��400��$��40��$��	10,0,10,0q�$
��Wrap=�$
��Left=�$��Center=�$*��$正在下载更新包，请稍后...��� -   $��progressBar��$
��Left=�$��0,10,0,0q�$	��440��$��10�� � IsIndeterminate .  ���#394165���#e64c66$��0q��� -   $��button_confirm��  style_button_confirm#��[ $��	0,150,0,0q�$��确定��