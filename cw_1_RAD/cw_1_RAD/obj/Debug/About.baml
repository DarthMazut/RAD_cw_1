   M S B A M L   `   `   `  ����C  ?cw_1_RAD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:cw_1_RADcw_1_RAD  5   	       cw_1_RAD.About   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:cw_1_RAD     5   	   -   5          �Name$  	xe_Window��5   	    � Title$ O aplikacji��5   	   $	��500��6   $	��650��6*    � 
ResizeMode$ NoResize=�66    � Icon$ img/hint.png��6L    � WindowStyle$
 None=�6`   
����iA�$��1q�6�   .��5	      � -   5         �Name$ xe_MainGrid��5	       � ColumnDefinitions 5
   
   �� 5      $��2* �6   �� 5      $��3* �6   .+�5   
   �� -   5         ��Name$ xe_Logo��5      $��Center=�6!   $	��128��6>   $��	0,0,0,125q�6K   $��Center=�6^   $	��128��6y   $��img/WIiNoM_small.png��6�   �� 5   
   -   5         ��Name$ xe_TextBlock_Logo��5      $��Center=�6/   $��	5,125,5,0q�6L   $
��Wrap=�6_   $1��+Wydział Informatyki i Nauki o Materiałach��6s   $��Center=�6�   $��Comic Sans MS)�6�   $��24(�6�   ���	#FF5050506�   �� 5   
   -   5        	 ��Name$	 xe_ButtonClose��5      $��Zamknij��6)   $��1��6;   $��Right=�6K   $��	0,0,10,10q�6g   $��Bottom=�6z   $	��150��6�   $��40��6�   $��18.667(�6�   6�   �� 5   
   -   5        
 ��Name$
 xe_ScrollViewer��5      $��1��60   $��10,10,10,70q�6@   $
��Auto=�6U   .��5      �� -   5        $ xe_TextBlock_Opis��5      $
��Wrap=�63   $��16(�6H   $��Segoe Print)�6V   }Niniejsza aplikacja zostałą stworzona jako jeden z projektów na zaliczenie przedmiotu "Programowanie w środowiskach RAD".�� 5      �� 6   RPAplikacja pozwala na walidację numerów kart kredytowych następujących marek:�� 5   ^   - Visa�� 5      - Master Card�� 5      - American Express�� 5   $   - Diners Club�� 5      
- Discover�� 5      - JCB�� 5      `^Regex'y do tych numerów zaczerpniętu z: http://www.regular-expressions.info/creditcard.html.�� 5      �� 6   LJAplikacja ma stylowe animacje i nowoczesny wygląd. Powinna się podobać.�� 5      �� 6   CAJedna z ikon jest w wersji linkver, tak więc: http://newidols.ru5!      5"      5$      5%      