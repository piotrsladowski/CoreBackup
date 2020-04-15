using System;
using System.Collections.Generic;
using System.Text;
using CoreBackup.Models.IO;

namespace CoreBackup.Models.Config
{
    abstract class Configuration
    {
        protected bool isEncrypted = false;
        public DataSource dataSource;
        public abstract List<FileInformation> GetFiles();
    }
    public enum DataSource
    {
        NotSet,
        Directory,
        FTP,
        Other
    }
}
