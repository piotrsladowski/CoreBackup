using CoreBackup.Models.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreBackup.Models.Config
{
    class DirectoryConfig : Configuration
    {
        private DataSource dataSource;
        private Dictionary<int, string> localPaths;

        public Dictionary<int, string> GetLocalPaths()
        {
            return localPaths;
        }

        public DirectoryConfig()
        {
            localPaths = new Dictionary<int, string>();
            dataSource = DataSource.Directory;
        }

        public void ProvideLocalPath(int key, string localPath)
        {
            localPaths[key] = localPath;
        }

        public string BrowseLocalPaths(int key)
        {
            string result = null;

            if (localPaths.ContainsKey(key))
            {
                result = localPaths[key];
            }
            return result;
        }

        public override List<FileInformation> GetFiles()
        {
            throw new NotImplementedException();
        }
    }
}
