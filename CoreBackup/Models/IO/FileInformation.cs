using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBackup.Models.IO
{
    public class FileInformation
    {
        public string Filename { get; set; }
        public string Extension { get; set; }
        public bool IsChecked { get; set; }
        public long Size { get; set; }
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
}
