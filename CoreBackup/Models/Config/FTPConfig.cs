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

        public void provideCredentials(string username, string password, string url)
        {
            credentials["username"] = username;
            credentials["password"] = password;
            credentials["url"] = url;
        }

        public Dictionary<string, string> GetCredentials()
        {
            return credentials;
        }
      
    }
}
