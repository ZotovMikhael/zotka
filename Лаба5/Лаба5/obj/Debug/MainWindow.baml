   M S B A M L   `   `   `  ����D  @Лаба5, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:лаба5	лаба5  5   	       лаба5.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   )localclr-namespace:лаба5     5   	   -   5          � Title$  Блокнот��5   	   $	��450��6   $	��600��6&   
���   � J� CommandBindings 5	      )  #System.Windows.Input.CommandBinding  5
   
   -   5           лаба5.MainWindow   
NewCommand   Command# Z 5
      6J     5   
   -   5           OpenCommand# Z 5      6K     5   
   -   5           SaveCommand# Z 5      6K   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ NoteTextBox��5      $��0��5      $��0 0 0 10q�5      $��14(�5       �� AcceptsReturn . "	 �� VerticalScrollBarVisibility$
	 Auto=�5      $	��350��5      �� 5   
   $��1��6   
 �� Orientation$
 
Horizontal=�6"   $��Right=�6;   .+�5      �� $��Создать (Ctrl+N)��6   $��0 0 10 0q�5      $
��15 5q�5       #��Z 5!      �� 5"      $!��Сохранить (Ctrl+S)��6   $��0 0 10 0q�5#      $
��15 5q�5$      #��Z 5%      �� 5&      $��Открыть (Ctrl+O)��6   $
��15 5q�5'      #��Z 5(      5)      5*      5+      