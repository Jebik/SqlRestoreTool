using SqlManager.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SqlManager
{
    partial class MainWindow : Window
    {
        private void RestoreAtDateExecute(ServerAccess ba, SqlServerAccess sa, FileInfo f, string dbName, RestoreOption opt)
        {
            try
            {
                using (new NetworkConnection(ba.uri, new NetworkCredential(ba.username, ba.password)))
                {
                    var filePath = f.FullName;
                    using (new NetworkConnection(sa.uri, new NetworkCredential(sa.username, sa.password)))
                    {
                        string destPath = Path.Combine(sa.uri, sa.folder, f.Name);
                        if (File.Exists(destPath))
                        {
                            File.Delete(destPath);
                        }
                        File.Copy(filePath, destPath);var bakFilePath = destPath;
                        destPath = destPath.Replace(sa.uri, sa.disk);

                        SMOHelper restoreInstance = new SMOHelper(sa);
                        restoreInstance.Restore(destPath, dbName, opt);
                        if (File.Exists(bakFilePath))
                        {
                            File.Delete(bakFilePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        private void RestoreAtTimeExecute(ServerAccess ba, SqlServerAccess sa, string c, string dbName, DateTime time, RestoreOption opt)
        {
            try
            {
                using (new NetworkConnection(ba.uri, new NetworkCredential(ba.username, ba.password)))
                {
                    string path = Path.Combine(ba.uri, ba.dailyfolder, c);
                    DirectoryInfo dir = new DirectoryInfo(path);
                    FileInfo[] files = dir.GetFiles("*.bak").OrderByDescending(p => p.CreationTime).ToArray();
                    var f = files.FirstOrDefault();
                    var filePath = f.FullName;
                    var creationDate = f.CreationTime;
                    FileInfo[] transactionFiles = dir.GetFiles("*.trn").OrderByDescending(p => p.CreationTime).ToArray();
                    using (new NetworkConnection(sa.uri, new NetworkCredential(sa.username, sa.password)))
                    {
                        string destPath = Path.Combine(sa.uri, sa.folder, f.Name);
                        if (File.Exists(destPath))
                        {
                            File.Delete(destPath);
                        }
                        File.Copy(filePath, destPath);
                        var bakFilePath = destPath;
                        var transactionFilesPath = CopyAllTransactionFiles(transactionFiles, sa, creationDate, time);

                        destPath = destPath.Replace(sa.uri, sa.disk);
                        var transactionFilesDestPath = new List<string>();
                        foreach (var tf in transactionFilesPath)
                        {
                            transactionFilesDestPath.Add(tf.Replace(sa.uri, sa.disk));
                        }

                        SMOHelper restoreInstance = new SMOHelper(sa);
                        restoreInstance.RestoreAtTime(destPath, transactionFilesDestPath, time, dbName, opt);
                        if (File.Exists(bakFilePath))
                        {
                            File.Delete(bakFilePath);
                        }
                        foreach (var fileToDel in transactionFilesPath)
                        {
                            if (File.Exists(fileToDel))
                            {
                                File.Delete(bakFilePath);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        private List<string> CopyAllTransactionFiles(FileInfo[] transactionFiles, SqlServerAccess sa, DateTime BAKDate, DateTime EndDate)
        {
            var transactionsFilesDestPath = new List<string>();
            var tfList = transactionFiles.OrderBy(tf => tf.CreationTime).ToList();
            var copy = true;

            foreach (var f in tfList)
            {
                if (f.CreationTime > BAKDate && copy)
                {
                    string destPath = Path.Combine(sa.uri, sa.folder, f.Name);
                    if (File.Exists(destPath))
                    {
                        File.Delete(destPath);
                    }
                    File.Copy(f.FullName, destPath);
                    transactionsFilesDestPath.Add(destPath);
                    if (f.CreationTime > EndDate)
                        copy = false;
                }
            }
            return transactionsFilesDestPath;
        }

        private async Task SwitchExecute(ServerAccess ba, RestoreMode mode)
        {
            try
            {
                await Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)(() =>
                {
                    Clients.Clear();
                }));
                using (new NetworkConnection(ba.uri, new NetworkCredential(ba.username, ba.password)))
                {
                    var listClientRemote = Directory.GetDirectories(ba.uri);

                    var path = Path.Combine(ba.uri, ba.dailyfolder);
                    var listClient = Directory.GetDirectories(path);

                    var cleanList = new List<string>();
                    foreach (var c in listClient)
                    {
                        var substring = c.Remove(c.IndexOf(path), path.Length + 1);
                        cleanList.Add(substring);
                    }
                    var sortedList = cleanList.OrderBy(c => c).ToList();

                    await Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)(() =>
                    {
                        foreach (var c in sortedList)
                        {
                            Clients.Add(c);
                        }
                    }));
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        private async Task CleanExecute(SqlServerAccess sa, RestoreOption opt, string db)
        {
            SMOHelper instance = new SMOHelper(sa);
            instance.Clean(db, opt);
        }

        private async Task ShrinkExecute(SqlServerAccess sa, string db)
        {
            SMOHelper instance = new SMOHelper(sa);
            instance.Shrink(db);
        }

        private async Task ShrinkLogExecute(SqlServerAccess sa)
        {
            SMOHelper instance = new SMOHelper(sa);
            instance.ShrinkAllLogFile();
        }

        private async Task ShrinkFileExecute(SqlServerAccess sa)
        {
            SMOHelper instance = new SMOHelper(sa);
            instance.ShrinkAllDBFile();
        }

        private async Task RemoveBAKFilesExecute(SqlServerAccess sa)
        {
            using (new NetworkConnection(sa.uri, new NetworkCredential(sa.username, sa.password)))
            {
                string path = Path.Combine(sa.uri, sa.folder);
                DirectoryInfo dir = new DirectoryInfo(path);
                FileInfo[] files = dir.GetFiles("*.bak").OrderByDescending(p => p.CreationTime).ToArray();

                int i = 0;
                foreach (var f in files)
                {
                    File.Delete(f.FullName);
                    i++;
                }
            }
        }

        private async Task SqlOnClickExecute(SqlServerAccess sa)
        {
            SMOHelper restoreInstance = new SMOHelper(sa);
            var bddList = restoreInstance.GetDatabase();

            await Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)(() =>
            {
                SourceDatabases.Clear();
                foreach (var bdd in bddList)
                {
                    SourceDatabases.Add(bdd);
                }
            }));            
        }

        private async Task ClientOnClickExecute(ServerAccess ba, string client, RestoreMode mode)
        {
            try
            {
                await Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)(() =>
                {
                    Files.Clear();
                }));
                using (new NetworkConnection(ba.uri, new NetworkCredential(ba.username, ba.password)))
                {
                    string path = Path.Combine(ba.uri, ba.dailyfolder, client);
                    DirectoryInfo dir = new DirectoryInfo(path);
                    FileInfo[] files = dir.GetFiles("*.bak").OrderByDescending(p => p.CreationTime).ToArray();
                    
                    if (files.Count() > 0)
                    {
                        await Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)(() =>
                        {
                            SelectedTime = DateTime.Now;
                            for (int i = 0; i < files.Count(); i++)
                            {
                                Files.Add(files[i]);
                            }
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        private async Task UpdateBackUpInfoExecute(SqlServerAccess sa)
        {
            try
            {
                // Now load the information from the machine.
                if (RemoteSysInfo.Get(sa.host, sa.username, sa.password) != 0)
                {
                    throw new Exception(RemoteSysInfo.Error);
                }
                foreach (var d in RemoteSysInfo.LogicalDrives)
                {
                    if (d.Name == sa.disk)
                    {
                        await Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)(() =>
                        {
                            Drive = d;
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
}