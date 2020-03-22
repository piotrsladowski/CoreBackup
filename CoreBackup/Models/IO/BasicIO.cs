using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreBackup.Models.IO
{
    class BasicIO
    {
        public List<string> getFilesInDirectory(string path)
        {
            List<string> fNames = new List<string>();
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (var f in di.EnumerateFiles())
            {
                fNames.Add(f.ToString());
            }

            return fNames;
        }
    }
}
