
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

        private void ShrinkDB(Database db)
        {
            try
            {
                db.RecalculateSpaceUsage();
                var percent = (int)Math.Truncate(db.SpaceAvailable / (db.Size * 10));
                db.Shrink(percent, ShrinkMethod.Default);
                db.Refresh();
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }

        internal void Clean(string dbName, RestoreOption opt)
        {
            string dbUse = $"USE [{dbName}]\n";
            string connectionString = $"Data Source={_sqlAccess.name};Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            if ((opt & RestoreOption.CleanTable) == RestoreOption.CleanTable)
            {
                ExecuteSql(dbUse + SqlQuery.CleanTable, connectionString, "Clean Table");
            }
        }

        private void ExecuteSql(string query, string connectionString, string success)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                try
                {
                    int res = command.ExecuteNonQuery();
                    Logger.Info(success);
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                }
            }
        }

        internal List<string> GetDatabase()
        {
            var bddList = new List<string>();
            try
            {
                InitConnexion();
                var size = _smoServer.Databases.Count;
                for (int i = 0; i < size; i++)
                {
                    bddList.Add(_smoServer.Databases[i].Name);
                }
                ConnexionClose();
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
            return bddList;
        }

        internal void Shrink(string db)
        {
            try
            {
                InitConnexion();
                if (!_smoServer.Databases.Contains(db))
                {
                    Logger.Error("Database Not Found");
                    return;
                }

                var database = _smoServer.Databases[db];
                ShrinkDB(database);
                ConnexionClose();
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }
        
        private void ConnexionClose()
        {
            _sqlConnection.Disconnect();
        }

        internal void InitConnexion()
        {
            if (String.IsNullOrWhiteSpace(_sqlAccess.usernameSql))
            {
                _sqlConnection = new ServerConnection(_sqlAccess.uriSql);
            }
            else
            {
                _sqlConnection = new ServerConnection(_sqlAccess.uriSql, _sqlAccess.usernameSql, _sqlAccess.passwordSql);
            }
            _sqlConnection.StatementTimeout = 6000;
            _smoServer = new Server(_sqlConnection);
        }
    }
}