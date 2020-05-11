using CoreBackup.Models.IO;
using System;
using System.Collections.Generic;
using System.Text;
using CoreBackup.Models.Remote;

namespace CoreBackup.Models.Config
{
    class FTPConfig : Configuration
    {
        public Dictionary<string, string> credentials { get; set; }

        public Dictionary<string, string> GetCredentials()
        {
            return credentials;
        }

        public Dictionary<string, string> paths { get; set; }

        public Dictionary<string, string> GetPaths()
        {
            return paths;
        }

        public DataSource dataSource { get; set; }
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
            try
            {
                var filesList = new List<FileInformation>();
                FTP.GetAllInformationsAboutFiles(credentials["server"], credentials["username"], credentials["password"], ref filesList);
                return filesList;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public override List<string> GetConfigPaths()
        {
            throw new NotImplementedException();
        }
    }
}
