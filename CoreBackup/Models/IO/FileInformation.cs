using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBackup.Models.IO
{
    public class FileInformation
    {
        public string RelativePath { get; set; }
        public string FullPath { get; set; }
        public string Extension { get; set; }
        public bool IsChecked { get; set; }
        public string LocalPath { get; set; }
        public long Size { get; set; }
        public string ConfigurationName { get; set; }
        public FileVersion FileVersion { get; set; }
        private long _modificationTime;
        public long ModificationTime {
            get {
                return _modificationTime;
            }
            set {
                _modificationTime = value;
            }
        }

    }
    public enum FileVersion
    {
        NotSet = 0,
        Newer = 1,
        Older = 2,
        Equal = 3
    }
}
