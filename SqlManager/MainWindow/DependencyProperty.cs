using SqlManager.Tools;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace SqlManager
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> Clients
        {
            get { return (ObservableCollection<string>)GetValue(ClientsProperty); }
            set { SetValue(ClientsProperty, value); }
        }

        public static readonly DependencyProperty ClientsProperty =
            DependencyProperty.Register("Clients", typeof(ObservableCollection<string>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<string>()));


        public ObservableCollection<string> Commands
        {
            get { return (ObservableCollection<string>)GetValue(CommandsProperty); }
            set { SetValue(CommandsProperty, value); }
        }

        public static readonly DependencyProperty CommandsProperty =
            DependencyProperty.Register("Commands", typeof(ObservableCollection<string>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<string>()));

        public ObservableCollection<string> CommandsByDB
        {
            get { return (ObservableCollection<string>)GetValue(CommandsByDBProperty); }
            set { SetValue(CommandsByDBProperty, value); }
        }

        public static readonly DependencyProperty CommandsByDBProperty =
            DependencyProperty.Register("CommandsByDB", typeof(ObservableCollection<string>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<string>()));

        public ObservableCollection<FileInfo> Files
        {
            get { return (ObservableCollection<FileInfo>)GetValue(FilesProperty); }
            set { SetValue(FilesProperty, value); }
        }

        public static readonly DependencyProperty FilesProperty =
            DependencyProperty.Register("Files", typeof(ObservableCollection<FileInfo>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<FileInfo>()));


        public ObservableCollection<ServerAccess> Sources
        {
            get { return (ObservableCollection<ServerAccess>)GetValue(SourcesProperty); }
            set { SetValue(SourcesProperty, value); }
        }
        public static readonly DependencyProperty SourcesProperty =
            DependencyProperty.Register("Sources", typeof(ObservableCollection<ServerAccess>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<ServerAccess>()));


        public ObservableCollection<SqlServerAccess> SqlSources
        {
            get { return (ObservableCollection<SqlServerAccess>)GetValue(SqlSourcesProperty); }
            set { SetValue(SqlSourcesProperty, value); }
        }
        public static readonly DependencyProperty SqlSourcesProperty =
            DependencyProperty.Register("SqlSources", typeof(ObservableCollection<SqlServerAccess>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<SqlServerAccess>()));

        public ObservableCollection<string> SourceDatabases
        {
            get { return (ObservableCollection<string>)GetValue(SourceDatabasesProperty); }
            set { SetValue(SourceDatabasesProperty, value); }
        }
        public static readonly DependencyProperty SourceDatabasesProperty =
            DependencyProperty.Register("SourceDatabases", typeof(ObservableCollection<string>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<string>()));

        public bool UIEnabled
        {
            get { return (bool)GetValue(UIEnabledtProperty); }
            set { SetValue(UIEnabledtProperty, value); }
        }
        public static readonly DependencyProperty UIEnabledtProperty =
            DependencyProperty.Register("UIEnabled", typeof(bool), typeof(MainWindow), new PropertyMetadata(null));


        public LogicalDrive Drive
        {
            get { return (LogicalDrive)GetValue(DriveProperty); }
            set { SetValue(DriveProperty, value); }
        }

        public static readonly DependencyProperty DriveProperty =
            DependencyProperty.Register("Drive", typeof(LogicalDrive), typeof(MainWindow), new PropertyMetadata(new LogicalDrive()));

        public DateTime? SelectedTime
        {
            get { return (DateTime?)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(DateTime?), typeof(MainWindow), new PropertyMetadata(null));
                
        public bool CleanTable
        {
            get { return (bool)GetValue(CleanTableProperty); }
            set { SetValue(CleanTableProperty, value); }
        }

        public static readonly DependencyProperty CleanTableProperty =
            DependencyProperty.Register("CleanTable", typeof(bool), typeof(MainWindow), new PropertyMetadata());

        public bool CleanTrackerPositions
        {
            get { return (bool)GetValue(CleanTrackerPositionsProperty); }
            set { SetValue(CleanTrackerPositionsProperty, value); }
        }

        public static readonly DependencyProperty CleanTrackerPositionsProperty =
            DependencyProperty.Register("CleanTrackerPositions", typeof(bool), typeof(MainWindow), new PropertyMetadata());


        public RestoreMode RestoreMode
        {
            get { return (RestoreMode)GetValue(RestoreModeProperty); }
            set { SetValue(RestoreModeProperty, value); }
        }

        public static readonly DependencyProperty RestoreModeProperty =
            DependencyProperty.Register("RestoreMode", typeof(RestoreMode), typeof(MainWindow), new PropertyMetadata());


    }
}