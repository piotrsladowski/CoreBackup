using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreBackup.Models.IO
{
    class BasicIO
    {
        public IEnumerable<FileInfo> getFilesInDirectory(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            //foreach (FileInfo f in di.EnumerateFiles())
            //{
            //    fNames.Add(f.ToString());
            //}

            return di.EnumerateFiles();
        }
    }
}
