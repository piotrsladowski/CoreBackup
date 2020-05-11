using CoreBackup.Models.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CoreBackup.Models.Config
{
    class DirectoryConfig : Configuration
    {
        public DataSource dataSource { get; set; }
        public Dictionary<int, string> localPaths { get; set; }
        // List caused problems in GUI.

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
            var filesList = new List<FileInformation>();
            foreach (KeyValuePair<int, string> entry in localPaths)
            {
                var path = entry.Value;
                string[] allfiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                foreach(var file in allfiles)
                {
                    try
                    {
                        FileInformation fileInformation = new FileInformation();
                        var fileInfo = new FileInfo(file);
                        //var dirName = fileInfo.Directory.Name;
                        fileInformation.FullPath = fileInfo.FullName;
                        //fileInformation.RelativePath = dirName + Path.DirectorySeparatorChar + fileInfo.FullName.Remove(0, path.Length + 1);
                        fileInformation.RelativePath = fileInfo.FullName.Remove(0, path.Length + 1);
                        fileInformation.Extension = fileInfo.Extension;
                        fileInformation.Size = fileInfo.Length;
                        fileInformation.ModificationTime = new DateTimeOffset(fileInfo.LastWriteTime).ToUnixTimeSeconds();
                        fileInformation.LocalPath = path;
                        filesList.Add(fileInformation);
                    }
                    catch (FileNotFoundException)
                    {
                        Debug.WriteLine(file);
                    }
                }
            }
            return filesList;
        }

        public override List<string> GetConfigPaths()
        {
            var pathsList = new List<string>();
            foreach (KeyValuePair<int, string> entry in localPaths)
            {
                pathsList.Add(entry.Value);
            }
            return pathsList;
        }
    }
}
