   M S B A M L   `   `   `  ���� U  QMaterialDesignThemes.Wpf, Version=2.3.1.953, Culture=neutral, PublicKeyToken=nulleHclr-namespace:MaterialDesignThemes.Wpf;assembly=MaterialDesignThemes.WpfMaterialDesignThemes.Wpf  {Sclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf#MaterialDesignThemes.Wpf.Converters  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem E ASqlManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:SqlManager
SqlManager 3clr-namespace:SqlManager.ToolsSqlManager.Tools    SqlManager.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     XwpfHclr-namespace:MaterialDesignThemes.Wpf;assembly=MaterialDesignThemes.Wpf     j
convertersSclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf     9System&clr-namespace:System;assembly=mscorlib     *localclr-namespace:SqlManager     /convclr-namespace:SqlManager.Tools     -     �Name$  UcMainWindow�� � Title$ 
MainWindow��$	��500��$	��900�� � 	Resources  �� MergedDictionaries �� Source;   5MaterialDesignThemes.Wpf.Converters.CardClipConverter   CardClipConverter%  SqlManager.Tools.LengthToString  LengthToStringConverter  RestoreModeValues S� 
MethodName S� 
ObjectType  System.Enum S� MethodParameters M� TypeName��  �� $e _pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml?��� $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml?��� $z tpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.DeepPurple.xaml?��� $s mpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml?�%c   &	        &	      &	 
         S� $ 	GetValues��"   M� $ local:RestoreMode��.��� {��� *	UIEnabled+	 �� UpdateSourceTrigger$	 PropertyChanged=�
 �� ElementName$
 UcMainWindow��.+��  � RowDefinitions �� $��9* ��� $��1* ��� $��1* ��� $��1* � � ColumnDefinitions �� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* �.+��� -   $��
ClientList��$��0��$��0��$��2��$��2������ *	Clients+$	 PropertyChanged=�$
 UcMainWindow��&   System.Windows.Controls.DataGrid  -   $��	FilesList��$��0��$��2��$��4��   SelectionUnit$ FullRow=�   AutoGenerateColumns .  ���� *Files+$	 PropertyChanged=�$
 UcMainWindow��   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ 2*    Header$
 NAME��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Name+  $ 1* $
 SIZE�� �� *Length+ �� 	Converter# [ �� $��1��$��2��$��Center=�$��Center=� �� Orientation$ 
Horizontal=�.+���  d� 	IsChecked �� *
CleanTable+$
 UcMainWindow���� $�� Clean CleanTable���� -   $��	DBNameBox��$��1��$��4��$��Center=�$��Center=��� -   $��1��$��5��$��Restore��#	   MaterialDesignThemes.Wpf.Card	  $��2��$��0��$��2q�.���� -   $��	ModeCombo��$��Center=�$��Center=� �� SelectedIndex$ 0������  �� Source# [  �� SelectedItem �� *RestoreMode+$	 PropertyChanged=�$
 UcMainWindow��	  $��2��$��1��$��0q�.��)
   #MaterialDesignThemes.Wpf.TimePicker
  -   $
��Time��$��Center=�$��Center=� 
  	Is24Hours .  
  SelectedTime �� *SelectedTime+$
 UcMainWindow��	  $��2��$��2��$��2q�.���� $��2��$��2��$��Center=�$��Center=����� *Drive.FreeSpace+$
 UcMainWindow��# [  �� StringFormat$ Espace libre:{0}��	  $��2��$��3��$��2q�.���� $��Center=�$��Center=����� *
Drive.Size+$
 UcMainWindow��# [ $ Espace total:{0}��	  $��2��$��4��$��2q�.���� -   $��CommandCombo��$��Center=�$��Center=�$ 0������ *
Commands+$	 PropertyChanged=�$
 UcMainWindow��	  $��2��$��5��$��2q�.���� -	   $��Execute��	  $��3��$��0��$��2q�.���� -
   $��SourcesCombo��$��Center=�$��Center=�$ 0������ *	Sources+$	 PropertyChanged=�$
 UcMainWindow��	  $��3��$��1��$��2q�.���� -   $��	SwitchBtn��$��Switch��	  $��3��$��2��$��2q�.���� -   $��SqlSourcesCombo��$��Center=�$��Center=�$ 0������ *
SqlSources+$	 PropertyChanged=�$
 UcMainWindow��	  $��3��$��3��$��2q�.���� -   $��BddCombo��$��Center=�$��Center=�$ 0������ *SourceDatabases+$	 PropertyChanged=�$
 UcMainWindow��	  $��3��$��4��$��2q�.���� -   $��CommandByDBCombo��$��Center=�$��Center=�$ 0������ *CommandsByDB+$	 PropertyChanged=�$
 UcMainWindow��	  $��3��$��5��$��2q�.���� -   $��SwitchSqlBtn��$��Execute��