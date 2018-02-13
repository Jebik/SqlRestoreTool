using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SqlManager.Tools
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "config", Namespace = "", IsNullable = false)]
    public class Config
    {
        [XmlArray("backUps")]
        [XmlArrayItem("backUpAccess")]
        public BackUps backUps { get; set; }

        [XmlArray("sql")]
        [XmlArrayItem("sqlAccess")]
        public SqlAccess sql { get; set; }
    }

    public class BackUps : List<ServerAccess> { }

    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class ServerAccess
    {
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string uri { get; set; }
        public string dailyfolder { get; set; }
        public string todayfolder { get; set; }

        public override string ToString()
        {
            return name;
        }
    }


    public class SqlAccess : List<SqlServerAccess> { }

    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class SqlServerAccess
    {

        public string host { get; set; }
        public string disk { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string uri { get; set; }
        public string folder { get; set; }


        public string usernameSql { get; set; }
        public string passwordSql { get; set; }
        public string uriSql { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}