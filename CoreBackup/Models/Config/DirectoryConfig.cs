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
        public DataSource DataSource { get; set; }
        public Dictionary<int, string> LocalPaths { get; set; }
        // List caused problems in GUI.

        public Dictionary<int, string> GetLocalPaths()
        {
            return LocalPaths;
        }

        public DirectoryConfig()
        {
            LocalPaths = new Dictionary<int, string>();
            DataSource = DataSource.Directory;
        }

        public void ProvideLocalPath(int key, string localPath)
        {
            LocalPaths[key] = localPath;
        }

        public string BrowseLocalPaths(int key)
        {
            string result = null;

            if (LocalPaths.ContainsKey(key))
            {
                result = LocalPaths[key];
            }
            return result;
        }

        
        public override List<FileInformation> GetFiles()
        {
            var filesList = new List<FileInformation>();
            foreach (KeyValuePair<int, string> entry in LocalPaths)
            {
                var path = entry.Value;
                string[] allfiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                foreach(var file in allfiles)
                {
                    try
                    {
                        FileInformation fileInformation = new FileInformation();
                        var fileInfo = new FileInfo(file);
                        fileInformation.FullPath = fileInfo.FullName;
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
            foreach (KeyValuePair<int, string> entry in LocalPaths)
            {
                pathsList.Add(entry.Value);
            }
            return pathsList;
        }
    }
}
