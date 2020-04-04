using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBackup.Models.Config
{
    abstract class Configuration
    {
        protected bool isEncrypted = false;
        protected DataSource dataSource;

    }
    public enum DataSource
    {
        Directory,
        FTP,
        Other
    }
}
