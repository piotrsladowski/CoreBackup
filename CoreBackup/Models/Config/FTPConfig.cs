using CoreBackup.Models.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBackup.Models.Config
{
    class FTPConfig : Configuration
    {
        private Dictionary<string, string> credentials;
        private DataSource dataSource;
        public FTPConfig()
        {
            dataSource = DataSource.FTP;
            credentials = new Dictionary<string, string>();
        }

        public void provideCredentials(string username, string password, string server)
        { 
            credentials["username"] = username;
            credentials["password"] = password;
            credentials["server"] = server;
        }

        public Dictionary<string, string> GetCredentials()
        {
            return credentials;
        }

        public string Get(string key)
        {
            string result = null;

            if (credentials.ContainsKey(key))
            {
                result = credentials[key];
            }
            return result;
        }

        public override List<FileInformation> GetFiles()
        {
            throw new NotImplementedException();
        }
    }
}
