using CoreBackup.Models.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBackup.Models.Config
{
    class FTPConfig : Configuration
    {
        private Dictionary<string, string> credentials;

        public Dictionary<string, string> GetCredentials()
        {
            return credentials;
        }

        private Dictionary<string, string> paths;

        public Dictionary<string, string> GetPaths()
        {
            return paths;
        }

        private DataSource dataSource;
        public FTPConfig()
        {
            dataSource = DataSource.FTP;
            credentials = new Dictionary<string, string>();
            paths = new Dictionary<string, string>();
        }

        public void provideCredentials(string username, string password, string server)
        { 
            credentials["username"] = username;
            credentials["password"] = password;
            credentials["server"] = server;
        }

        public void provideDownloadPath(string downloadDirectory)
        {
            paths["downloadDirectory"] = downloadDirectory;
        }

        public void provideUploadPath(string uploadDirectory)
        {
            paths["uploadDirectory"] = uploadDirectory;
        }

        
        public string BrowseCredentials(string key)
        {
            string result = null;

            if (credentials.ContainsKey(key))
            {
                result = credentials[key];
            }
            return result;
        }

        public string BrowsePaths(string key)
        {
            string result = null;

            if (paths.ContainsKey(key))
            {
                result = paths[key];
            }
            return result;
        }

        public override List<FileInformation> GetFiles()
        {
            throw new NotImplementedException();
        }
    }
}
