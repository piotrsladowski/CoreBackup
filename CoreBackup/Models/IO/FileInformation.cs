using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBackup.Models.IO
{
    class FileInformation
    {
        public string filename { get; set; }
        public long size { get; set; }
        private long _modificationTime;
        public long modificationTime {
            get {
                return _modificationTime;
            }
            set {
                _modificationTime = value;
            }
        }
    }
}
