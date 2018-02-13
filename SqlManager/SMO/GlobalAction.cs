
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

        internal void ShrinkAllDBFile()
        {
            try
            {
                InitConnexion();
                Logger.Success("SQL connected");
                int c = 0;
                foreach (Database db in _smoServer.Databases)
                {
                    if (db.Name == "tempdb")
                    {
                        c++;
                        Logger.Info($"{c}/{_smoServer.Databases.Count}");
                        continue;
                    }
                    db.DatabaseOptions.RecoveryModel = RecoveryModel.Simple;
                    db.Alter();

                    db.Shrink(20, ShrinkMethod.Default);
                    db.Refresh();
                    c++;
                    Logger.Info($"{c}/{_smoServer.Databases.Count}");
                }
                Logger.Success("DONE");
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
            ConnexionClose();
        }


        internal void ShrinkAllLogFile()
        {
            try
            {
                InitConnexion();
                Logger.Success("SQL connected");
                int c = 0;
                foreach (Database db in _smoServer.Databases)
                {
                    if (db.Name == "tempdb")
                    {
                        c++;
                        Logger.Info($"{c}/{_smoServer.Databases.Count}");
                        continue;
                    }
                    db.DatabaseOptions.RecoveryModel = RecoveryModel.Simple;
                    db.Alter();
                    db.LogFiles.Refresh();
                    for (int a = 0; a < db.LogFiles.Count; a++)
                    {
                        db.LogFiles[a].Shrink(1, ShrinkMethod.EmptyFile);
                    }

                    db.Alter();
                    db.Refresh();
                    c++;
                    Logger.Info($"{c}/{_smoServer.Databases.Count}");
                }
                Logger.Success("DONE");
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
            ConnexionClose();
        }
    }
}