using CoreBackup.Models.IO;
using System;
using System.Collections.Generic;
using System.Text;
using CoreBackup.Models.Remote;
using CoreBackup.ViewModels;

namespace CoreBackup.Models.Config
{
    class FTPConfig : Configuration
    {
        public Dictionary<string, string> Credentials { get; set; }

        public Dictionary<string, string> GetCredentials()
        {
            return Credentials;
        }

        public Dictionary<string, string> Paths { get; set; }

        public Dictionary<string, string> GetPaths()
        {
            return Paths;
        }

        public DataSource DataSource { get; set; }
        public FTPConfig()
        {
            DataSource = DataSource.FTP;
            Credentials = new Dictionary<string, string>();
            Paths = new Dictionary<string, string>();
        }

        public void ProvideCredentials(string username, string password, string server)
        { 
            Credentials["username"] = username;
            Credentials["password"] = password;
            Credentials["server"] = server;
        }

        public void ProvideDownloadPath(string downloadDirectory)
        {
            Paths["downloadDirectory"] = downloadDirectory;
        }

        public void ProvideUploadPath(string uploadDirectory)
        {
            Paths["uploadDirectory"] = uploadDirectory;
        }

        
        public string BrowseCredentials(string key)
        {
            string result = null;

            if (Credentials.ContainsKey(key))
            {
                result = Credentials[key];
            }
            return result;
        }

        public string BrowsePaths(string key)
        {
            string result = null;

            if (Paths.ContainsKey(key))
            {
                result = Paths[key];
            }
            return result;
        }
        
        public override List<FileInformation> GetFiles()
        {
            try
            {
                var filesList = new List<FileInformation>();
                FTP.GetAllInformationsAboutFiles(Credentials["server"], Credentials["username"], Credentials["password"], ref filesList);
                return filesList;
            }
            catch (Exception e)
            {
                EventLogViewModel.AddNewRegistry("An error has occured during remote file listing", DateTime.Now, 
                    "FTP" + Credentials["server"], "ERROR");
                throw;
            }
        }

        public override List<string> GetConfigPaths()
        {
            throw new NotImplementedException();
        }
    }
}
