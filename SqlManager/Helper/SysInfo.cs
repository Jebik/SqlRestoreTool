using System;
using System.Management;
using System.Globalization;
using System.Net;
using Microsoft.Win32;

namespace SqlManager.Tools
{
    /// <summary>
    /// Summary description for SystemInformation.
    /// </summary>
    /// 

    public class LogicalDrive
    {
        public String Name { get; set; }
        public long Size { get; set; }
        public long FreeSpace { get; set; }
    }


    public class SysInfo
    {
        private LogicalDrive[] _drives;
        public LogicalDrive[] LogicalDrives
        {
            get { return _drives; }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
        }

        public SysInfo()
        {
            _error = "";
        }
        
        public int Get(String host, String username, String password)
        {
            // No blank username's allowed.
            if (username == "")
            {
                username = null;
                password = null;
            }
            // Configure the connection settings.
            ConnectionOptions options = new ConnectionOptions();
            options.Username = username; //could be in domain\user format
            options.Password = password;
            ManagementPath path = new ManagementPath(String.Format("\\\\{0}\\root\\cimv2", host));
            ManagementScope scope = new ManagementScope(path, options);

            // Try and connect to the remote (or local) machine.
            try
            {
                scope.Connect();
            }
            catch (Exception ex)
            {
                _error = ex.Message;
                return 1;
            }
            GetLogicalDrives(scope);
            _error = "";
            return 0;
        }

        private void GetLogicalDrives(ManagementScope scope)
        {
            ManagementObjectSearcher moSearch = new ManagementObjectSearcher(scope, new ObjectQuery("Select Name, DriveType, Size, FreeSpace, FileSystem from Win32_LogicalDisk Where DriveType = 3 Or DriveType = 6"));
            ManagementObjectCollection moReturn = moSearch.Get();

            _drives = new LogicalDrive[moReturn.Count];
            int i = 0;
            foreach (ManagementObject mo in moReturn)
            {
                _drives[i] = new LogicalDrive();
                _drives[i].FreeSpace = long.Parse(mo["FreeSpace"].ToString());
                _drives[i].Size = long.Parse(mo["Size"].ToString());
                _drives[i].Name = mo["Name"].ToString();
                i++;
            }
        }      
    }
}
