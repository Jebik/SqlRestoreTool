
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace SqlManager.Tools
{
    public partial class SMOHelper
    {
        private SqlServerAccess _sqlAccess;
        private ServerConnection _sqlConnection;
        private Server _smoServer;
        private Database _targetDatabase;
        private RestoreOption _restoreOption;
        private List<int> ErrorCodeToIgnore;
        private List<string> _transactions;
        private DateTime _time;
        private RestoreMode _mode;

        public SMOHelper(SqlServerAccess sqlAccess)
        {
            _sqlAccess = sqlAccess;
            ErrorCodeToIgnore = new List<int>()
            {
                5701,
                4035,
                2528,
                944,
                951,
            };
        }

        public void Restore(string path, string restore_name, RestoreOption restoreOption)
        {
            _mode = RestoreMode.AtDate;
            RestoreBackup(path, restore_name, restoreOption);
        }

        public void RestoreAtTime(string path, List<string> transactionFilesDestPath, DateTime time, string restore_name, RestoreOption restoreOption)
        {
            _transactions = transactionFilesDestPath;
            _time = time;
            _mode = RestoreMode.AtTime;
            RestoreBackup(path, restore_name, restoreOption);
        }

        private void Restore_Information(object sender, ServerMessageEventArgs e)
        {
            if (ErrorCodeToIgnore.Contains(e.Error.Number))
                return;
            Logger.Error(e.Error.Message);
        }

        private void Restore_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            Logger.Info(e.Percent + "%");
        }


        private void RestoreBackup(string path, string restore_name, RestoreOption restoreOption)
        {
            try
            {
                _restoreOption = restoreOption;
                InitConnexion();
                if (!_smoServer.Databases.Contains(restore_name))
                {
                    var database = new Database(_smoServer, restore_name);
                    database.Create();
                }
                Logger.Success("SQL connected");
                _targetDatabase = _smoServer.Databases[restore_name];
                _targetDatabase.RecoveryModel = RecoveryModel.Simple;
                _targetDatabase.Alter(TerminationClause.RollbackTransactionsImmediately);

                Restore restore = new Restore();

                var backupDeviceItem = new BackupDeviceItem(path, DeviceType.File);
                restore.Devices.Add(backupDeviceItem);
                restore.Database = restore_name;
                restore.ReplaceDatabase = true;
                restore.NoRecovery = false;

                if (_mode == RestoreMode.AtTime)
                    restore.NoRecovery = true;

                var fileList = restore.ReadFileList(_smoServer);

                var dataFile = new RelocateFile();
                dataFile.LogicalFileName = fileList.Rows[0][0].ToString();
                dataFile.PhysicalFileName = _smoServer.Databases[restore_name].FileGroups[0].Files[0].FileName;

                var logFile = new RelocateFile();
                logFile.LogicalFileName = fileList.Rows[1][0].ToString();
                logFile.PhysicalFileName = _smoServer.Databases[restore_name].LogFiles[0].FileName;

                restore.RelocateFiles.Add(dataFile);
                restore.RelocateFiles.Add(logFile);

                _smoServer.KillAllProcesses(restore_name);
                
                restore.PercentComplete +=Restore_PercentComplete;
                restore.SqlRestoreAsync(_smoServer);
                restore.Information += Restore_Information;
                restore.Wait();
                Restore_Complete();
            }
            catch (SmoException ex)
            {
                Logger.Error("SMO Message : " + ex.Message);
                Logger.Error("SMO Exception : " + ex.InnerException);
            }
            catch (IOException ex)
            {
                Logger.Error("IO Message : " + ex.Message);
                Logger.Error("IO Exception : " + ex.InnerException);
            }
            catch (Exception ex)
            {
                Logger.Error("Message : " + ex.Message);
                Logger.Error("Exception : " + ex.InnerException);
            }
        }

        private void Restore_Complete()
        {
            if (_mode == RestoreMode.AtTime)
                ExecuteTransactions();
            else
            {
                SetDatabaseOnline();
            }
        }

        private void CleanDBAfterRestore()
        {
            if (_restoreOption != RestoreOption.None)
            {
                Logger.Info("Clean ...");
                Clean(_targetDatabase.Name, _restoreOption);
                Shrink(_targetDatabase.Name);
                Logger.Success("Clean Done");
            }
        }

        private void ExecuteTransactions()
        {
            try
            {
                var restore_name = _targetDatabase.Name;
                Restore restore = new Restore();
                var trFile = _transactions[0];
                var backupDeviceItem = new BackupDeviceItem(trFile, DeviceType.File);
                restore.Devices.Add(backupDeviceItem);
                restore.Database = restore_name;
                
                restore.ToPointInTime = _time.ToString("yyyy-MM-ddTHH:mm:ss");
                restore.ReplaceDatabase = false;
                restore.Action = RestoreActionType.Log;

                Logger.Info("Log file left: " + _transactions.Count);
                restore.PercentComplete += Restore_PercentComplete;
                restore.SqlRestoreAsync(_smoServer);
                restore.Information += Restore_Information;
                restore.Wait();
                Transaction_Complete();
            }
            catch (SmoException ex)
            {
                Logger.Error("SMO Message : " + ex.Message);
                Logger.Error("SMO Exception : " + ex.InnerException);
            }
            catch (IOException ex)
            {
                Logger.Error("IO Message : " + ex.Message);
                Logger.Error("IO Exception : " + ex.InnerException);
            }
            catch (Exception ex)
            {
                Logger.Error("Message : " + ex.Message);
                Logger.Error("Exception : " + ex.InnerException);
            }
        }

        private void Transaction_Complete()
        {
            _transactions.RemoveAt(0);

            if (_transactions.Count > 0)
                ExecuteTransactions();
            else
                SetDatabaseOnline();
        }


        private void SetDatabaseOnline()
        {
            CleanDBAfterRestore();
            _targetDatabase.DatabaseOptions.RecoveryModel = RecoveryModel.Simple;
            _targetDatabase.Alter(TerminationClause.RollbackTransactionsImmediately);
            _targetDatabase.LogFiles.Refresh();
            for (int a = 0; a < _targetDatabase.LogFiles.Count; a++)
            {
                _targetDatabase.LogFiles[a].Shrink(1, ShrinkMethod.EmptyFile);
            }
            _targetDatabase.IsReadCommittedSnapshotOn = true;
            _targetDatabase.Refresh();
            _targetDatabase.SetOnline();
            ConnexionClose();
            Logger.Success("Database ready");
        }
    }
}