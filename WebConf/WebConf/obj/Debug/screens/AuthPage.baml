   M S B A M L   `   `   `  ����B  >WebConf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WebConf.screensWebConf.screens  5          WebConf.screens.AuthPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:WebConf.screens     5        N� Title$  AuthPage��5      .��5      � ���	#FFC3EFFF$	��200��6"   $	��300��5       � RowDefinitions 5   
   �� 5      .+�5   
   �� -   5         ��Name$ 
loginInput��5      $
��Left=�6&   $��23��6A   $��	40,75,0,0q�6M    �� TextWrapping$
 Wrap=�6`   $	��Top=�6t   $	��120��6�   $��16(�6�   ���	#FFF7F7F76�   G� 5   
   -   5         G�Name$ passwordInput��5      $
��Left=�6-   $��23��6H   $��
40,135,0,0q�6T   $	��Top=�6h   $	��120��6�   ���	#FFF7F7F76�   �� 5   
   -   5         ��Name$ submitLogin��5      $��30��6&   $��Register��62   
�������$��14(�6X   ���	#FF715BFF$��
40,194,0,0q�6�   $	��120��6�   $
��Left=�6�   $	��Top=�6�   �� 5   
   -   5         ��Name$ ErrorMessage��5      $��Label��6&   $��Center=�66   $��10,163,10,0q�6S   $	��Top=�6h   $��16(�6�   ���	#FFAC0F0F$	��180��5      $��Center=�6   $��Center=�6>   $y�Hidden=�6b   $��31��6v   ��5   �   Z� 5   v   �� 5   
   -   5        $ go_to_login��5      $	��180��5      $��30��5      $��Alredy have an account?��5      ���	#33654EFB���	#FF747474$��14(�5   (   $��
10,243,0,0q�5   +   $
��Left=�6?   $	��Top=�6Z   5      �� 5   
   $��username��6   $
��Left=�6#   $��	40,59,0,0q�6>   $	��Top=�6Q   $	��120��6i   $��16��6u   ���	#FF715BFF$	��5,0q�6�   ��6�   Z� 6�   �� 5   
   $��password��6   $��40,115,40,118q�6#   ���	#FF715BFF$��Center=�6Q   $	��5,0q�6s   $
��Left=�6�   $	��Top=�6�   �� 5    
   -   5        $ title��5       $��Create new account��6   $
��Left=�6<   $	��Top=�6W   $	��200��6o   ���	#FF715BFF$��Center=�6�   $��Center=�6�   $��18(�6�   $��34��6�   5"      5#      