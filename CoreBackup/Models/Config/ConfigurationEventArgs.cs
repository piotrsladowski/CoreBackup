using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBackup.Models.Config
{
    public class ConfigurationEventArgs : EventArgs
    {
        public ConfigHub ConfigHub { get; set; }

        public int Side { get; set; } // 0-left, 1-right

        public int DataType { get; set; } // 1-dir, 2-FTP
    }
}
