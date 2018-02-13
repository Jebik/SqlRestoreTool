using SqlManager.Tools;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SqlManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private static Config _config;
        
        public static Config Config
        {
            get { return _config ?? (_config = new ConfigLoader().Config); }
        }

        public SysInfo RemoteSysInfo { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            InitCommands();
            InitCommandsByDB();
            RemoteSysInfo = new SysInfo();
            ClientList.SelectionChanged += ClientOnClick;
            FilesList.SelectionChanged += FileOnClick;
            SqlSourcesCombo.SelectionChanged += SqlOnClick;
            Loaded += InitData;
            foreach (var b in Config.backUps)
            {
                Sources.Add(b);
            }
            foreach (var b in Config.sql)
            {
                SqlSources.Add(b);
            }
            CleanTrackerPositions = true;
            UIEnabled = false;
        }

        private void InitData(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, (ThreadStart)(() =>
            {
                UpdateBackUpInfo();
                Switch(null, null);
            }));
        }

        private async Task UpdateBackUpInfo()
        {
            SqlServerAccess sa = null;
            await Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)(() =>
            {
                sa = SqlSourcesCombo.SelectedItem as SqlServerAccess;
            }));
            if (sa != null)
                await Task.Run(() => UpdateBackUpInfoExecute(sa));
        }

        private void InitCommands()
        {
            Commands.Add("ShrinkAllDBLog");
            Commands.Add("ShrinkAllDBFile");
            Commands.Add("RemoveBAKFiles");
            Commands.Add("UpdateDriveInfo");
        }

        private void InitCommandsByDB()
        {
            CommandsByDB.Add("Clean");
            CommandsByDB.Add("Shrink");
        }
        
        private async void SqlOnClick(object sender, SelectionChangedEventArgs e)
        {
            StartLoader();
            var sa =  SqlSourcesCombo.SelectedItem as SqlServerAccess;
            if (sa != null)
                await Task.Run(() => SqlOnClickExecute(sa));
            StopLoader();
        }

        private async void ClientOnClick(object sender, SelectionChangedEventArgs arg)
        {
            StartLoader();
            var ba = SourcesCombo.SelectedItem as ServerAccess;
            string client = (string)ClientList.SelectedItem;
            var mode = RestoreMode;
            if (ba != null && client != null)
                await Task.Run(() => ClientOnClickExecute(ba, client, mode));
            StopLoader();
        }
                
        private async void FileOnClick(object sender, SelectionChangedEventArgs arg)
        {
            var client = (string)ClientList.SelectedItem;
            var date = DateTime.Now;
            var dateString = date.ToString("ddMM");

            DBNameBox.Text = client + "_" + dateString;
        }
                
        private void StopLoader()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)(() =>
            {
                UIEnabled = true;
            }));
        }

        private void StartLoader()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)(() =>
            {
                UIEnabled = false;
            }));
        }

        private async void Execute(object sender, RoutedEventArgs e)
        {
            StartLoader();
            var cmd = CommandCombo.SelectedItem as string;
            if (cmd != null)
                await Task.Run(() => ExecuteCmd(cmd));
            StopLoader();
        }


        private async void ByDBExecute(object sender, RoutedEventArgs e)
        {
            StartLoader();
            var cmd = CommandByDBCombo.SelectedItem as string;
            var db = BddCombo.SelectedItem as string;
            if (cmd != null)
                await Task.Run(() => ByDBExecuteCmd(cmd, db));
            StopLoader();
        }

        private async Task ByDBExecuteCmd(string cmd, string db)
        {
            switch (cmd)
            {
                case "Clean":
                    await Clean(db);
                    break;
                case "Shrink":
                    await Shrink(db);
                    break;
            }
        }

        private async Task Clean(string db)
        {
            SqlServerAccess sa = null;
            RestoreOption opt = RestoreOption.None;
            await Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)(() =>
            {

                opt = (CleanTable ? RestoreOption.CleanTable : 0);
                sa = SqlSourcesCombo.SelectedItem as SqlServerAccess;
            }));
            if (sa != null)
                await Task.Run(() => CleanExecute(sa, opt, db));
        }
        
        private async Task Shrink(string db)
        {
            SqlServerAccess sa = null;
            await Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)(() =>
            {
                sa = SqlSourcesCombo.SelectedItem as SqlServerAccess;
            }));
            if (sa != null)
                await Task.Run(() => ShrinkExecute(sa, db));
        }

        private async Task ExecuteCmd(string cmd)
        {
            switch (cmd)
            {
                case "ShrinkAllDBLog":
                    await ShrinkLog();
                    break;
                case "ShrinkAllDBFile":
                    await ShrinkFile();
                    break;
                case "UpdateDriveInfo":
                    await UpdateBackUpInfo();
                    break;
                case "RemoveBAKFiles":
                    await RemoveBAKFiles();
                    break;
            }
        }

        private async Task RemoveBAKFiles()
        {
            SqlServerAccess sa = null;
               await Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)(() =>
            {
                sa = SqlSourcesCombo.SelectedItem as SqlServerAccess;
            }));
            if (sa != null)
                await Task.Run(() => RemoveBAKFilesExecute(sa));
        }

        private async Task ShrinkLog()
        {
            SqlServerAccess sa = null;
            await Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)(() =>
            {
                sa = SqlSourcesCombo.SelectedItem as SqlServerAccess;
            }));
            if (sa != null)
                await Task.Run(() => ShrinkLogExecute(sa));
        }

        private async Task ShrinkFile()
        {
            SqlServerAccess sa = null;
            await Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)(() =>
            {
                sa = SqlSourcesCombo.SelectedItem as SqlServerAccess;
            }));
            if (sa != null)
                await Task.Run(() => ShrinkFileExecute(sa));
        }

        private async void Switch(object sender, RoutedEventArgs e)
        {
            StartLoader();
            var ba = SourcesCombo.SelectedItem as ServerAccess;
            var mode = RestoreMode;
            if (ba != null)
                await Task.Run(() => SwitchExecute(ba, mode));
            StopLoader();
        }

        private async void Restore(object sender, RoutedEventArgs e)
        {
            if (RestoreMode == RestoreMode.AtTime && !SelectedTime.HasValue)
            {
                return;
            }
            if (RestoreMode == RestoreMode.AtDate && FilesList.SelectedItem == null)
            {
                return;
            }
            StartLoader();
            string dbName = DBNameBox.Text;
            var f = FilesList.SelectedItem as FileInfo;
            var ba = SourcesCombo.SelectedItem as ServerAccess;
            var sa = SqlSourcesCombo.SelectedItem as SqlServerAccess;
            var c = ClientList.SelectedItem as string;
            var opt = (CleanTable ? RestoreOption.CleanTable : 0);
            if (ba != null && sa != null && !String.IsNullOrWhiteSpace(dbName))
            {
                if (RestoreMode == RestoreMode.AtDate && f != null)
                    await Task.Run(() => RestoreAtDateExecute(ba, sa, f, dbName, opt));
                else
                {
                    var time = SelectedTime.Value;
                    await Task.Run(() => RestoreAtTimeExecute(ba, sa, c, dbName, time, opt));
                }
            }
            StopLoader();
        }
    }
}