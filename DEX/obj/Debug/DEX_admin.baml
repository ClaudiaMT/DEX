   M S B A M L   `   `   `  ����R  NSystem.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F2clr-namespace:System.Data.Sql;assembly=System.DataSystem.Data.Sql  5   @    DEX   DEX.DEX_admin   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   BSql2clr-namespace:System.Data.Sql;assembly=System.Data     6@   -   5          �Name$  DEXadmin��5      $	��400��5   	   $	��640��6   $	��640��6"   $	��400��61    � Icon$ dictionar-icon.ico��6A    � Title$ DEX administrator��6[   .��5      � $��3q�6    � ColumnDefinitions 5   
   �� 5      �� 5      .+�5
   
   �� $��Administrare conturi��6   $��3q�61   $��2q�6<   .��5      � .+�5      �� -   5         ��Name$ btn_ViewAutorizati��5      $
��View��65   $��	0,10,78,0q�6D   $	��Top=�6W   $��Right=�6o   $��45��6�   6�   �� 5      -   5        $ btn_DelAutorizati��5      $��Delete��64   $��0,10,7,0q�6E   $	��Top=�6W   $��Right=�6o   $��65��6�   {�.  6�   �� 5      -   5        $ btn_ViewNeautorizati��5      $
��View��67   $��	0,39,78,0q�6F   $	��Top=�6Y   $��Right=�6q   $��45��6�   6�   �� 5      -   5        $ btn_AutorizeazaCont��5      $��
Autorizare��66   $��0,39,8,0q�6K   $	��Top=�6]   $��Right=�6u   $��65��6�   {�.  6�   �� 5      $
��Left=�6   $��18��67   $��7,12,0,0q�6C   $
��Wrap=�6U   $%��Lista utilizatorilor autorizati��6i   $	��Top=�6�   $	��149��6�   �� 5      $
��Left=�6   $��22��67   $��7,39,0,0q�6C   $
��Wrap=�6U   $'��!Lista utilizatorilor neautorizati��6i   $	��Top=�6�   $	��163��6�    J� RenderTransformOrigin$ 0.5,0.5&�6�   z�5      V� 5      .~�5      �� �� 5      �� 5       �� Angle$
 0.43Q�6.   T� 5      5      5      �� 5      -   5         ��Name$ Lista_utilizatori��5      $��0,87,0,0q�65   $��Visible=�6G    �� DisplayMemberPath$
 Nume��6�   $	 �� IsSynchronizedWithCurrentItem$
	 TrueY�6�   
 �� SelectionMode$
 Extended=�6�   $
��Bold"�6�   ��6z   �� *	Tables[0]+6�   �� 5      -   5         ��Name$ txtblock_utilizatori��5      $��16��6:   $��
79,66,55,0q�6F   $
��Wrap=�6Z   $��Lista utilizatori��6n   $	��Top=�6�   $
��Bold"�6�   5      5      �� 5   
   $��1��6   $��	Dictionar��6#   $��3q�66   $��2q�6A   .��5       � .+�5!      �� -   5        $ btn_Exit��5!      $
��Exit��6+   $��0,39,2,0q�6:   $	��Top=�6L   $��Right=�6d   $��45��6�   6�   �� 5"      -	   5        $ btn_AddWordCategorie��5"      $	��Add��67   $
��Left=�6E   $��5,39,0,0q�6`   $	��Top=�6r   $��45��6�   {�.  6�   �� 5#      -
   5        $ btn_ModifWordCategorie��5#      $��Modify��69   $
��Left=�6J   $��	55,39,0,0q�6e   $	��Top=�6x   $��45��6�   {�.  6�   �� 5$      -   5        $ btn_DelWordCategorie��5$      $��Delete��67   $
��Left=�6H   $��
105,39,0,0q�6c   $	��Top=�6w   $��45��6�   {�.  6�   	� 5%      -   5         	�Name$ radio_categorie��5%      $��	Categorii��67   $
��Left=�6K   $��5,10,0,0q�6f   $	��Top=�6x   6�   	� 5&      -   5        $ radio_cuvinte��5&      $��Cuvinte��65   $
��Left=�6G   $��	88,10,0,0q�6b   $	��Top=�6u   6�   	� 5'      -   5        $ radio_statistici��5'      $��
Statistici��68   $
��Left=�6M   $��
155,10,0,0q�6h   $	��Top=�6|   6�   �� 5(      -   5         ��Name$ label_About��5(      $��About��6-   $��Right=�6=   $��0,5,1,0q�6Y   $��44��6j   
����  �$��23��6�   $	��Top=�6�   �� 5+      -   5        $ Lista_dictionar��5+      $��0,87,0,0q�63   $��Visible=�6E   $ Cuvant��6�   $
��Bold"�6�   $
	 TrueY�6�   $
 Extended=�6�   ��6x   �� *	Tables[0]+6�   �� 5,      -   5        $ txtblock_dictionar��5,      $��19��68   $��105,68,95,0q�6D   $
��Wrap=�6Y   $	��Top=�6m   $
��Bold"�6�   $��Lista cuvinte��6�   5-      5.      5/      50      