using System;
using System.Collections.Generic;
using System.Text;
using CoreBackup.Models.IO;
using Newtonsoft.Json;

namespace CoreBackup.Models.Config
{
    [JsonConverter(typeof(ConfigurationConverter))]
    public abstract class Configuration
    {
        public bool IsEncrypted { get; set; }
        public DataSource dataSource;
        public abstract List<FileInformation> GetFiles();
        public abstract List<string> GetConfigPaths();
    }
    public enum DataSource
    {
        NotSet,
        Directory,
        FTP,
        Other
    }
}
