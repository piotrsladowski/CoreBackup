using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Graph;
using File = System.IO.File;

namespace CoreBackup.Models.Remote
{
    public partial class FTP
    {
        public string Username { get; set; }
        public string Upload_Filename { get; set; }
        public string Download_Filename { get; set; }
        public string Server { get; set; }
        public string Password { get; set; }
        public string Path { get; set; }

        public List<string> directories;


        #region Validate Logging Process
        /// <summary>
        /// User Login Validation
        /// </summary>
        /// <returns>
        /// False if Username, Password or Server IP is Incorrect
        /// True if data listed above is Correct
        /// </returns>
        public bool ValidateLogging()
        {

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + Server));
                request.Credentials = new NetworkCredential(Username, Password);
                request.UseBinary = true;
                request.UsePassive = true;
                request.KeepAlive = false; // useful when only to check the connection.
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse) request.GetResponse();
                if (response.StatusCode != FtpStatusCode.NotLoggedIn)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (WebException)
            {
                Debug.WriteLine("Exception - FTP Class - Bad Credentials or Server IP");
                return false;
            }
        }

        #endregion

        #region Upload
        public void Upload()
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest) WebRequest.Create(new Uri(string.Format("{0}/{1}", "ftp://" + Server, Upload_Filename)));
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(Username, Password);
                Stream ftpStream = request.GetRequestStream();
                FileStream fs = File.OpenRead(Path);
                byte[] buffer = new byte[1024];
                double total = (double) fs.Length;
                int byteRead = 0;
                double read = 0;
                do
                {
                    byteRead = fs.Read(buffer, 0, 1024);
                    ftpStream.Write(buffer, 0, byteRead);
                    read += (double) byteRead;


                } while (byteRead != 0);
                fs.Close();
                ftpStream.Close();
            }
            catch (WebException)
            {
                Debug.WriteLine("Exception - FTP Class - Upload Method");
            }
        }
        #endregion

        #region Download
        public void Download(string filename, string destinationPath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", "ftp://" + Server, filename)));
                request.Credentials = new NetworkCredential(Username, Password);
                request.Method = WebRequestMethods.Ftp.DownloadFile;


                FtpWebRequest request1 = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", "ftp://" + Server, filename)));
                request1.Credentials = new NetworkCredential(Username, Password);
                request1.Method = WebRequestMethods.Ftp.GetFileSize;
                FtpWebResponse response = (FtpWebResponse)request1.GetResponse();
                double total = response.ContentLength;
                response.Close();

                FtpWebRequest request2 = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", "ftp://" + Server, filename)));
                request2.Credentials = new NetworkCredential(Username, Password);
                request2.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                FtpWebResponse response2 = (FtpWebResponse)request2.GetResponse();
                DateTime modify = response2.LastModified;
                response2.Close();


                Stream ftpstream = request.GetResponse().GetResponseStream();


                // Dodać rozwiązanie gdy wybrana scieżka to np. C:\  


                FileStream fs = new FileStream(destinationPath + "\\" + filename, FileMode.Create);

                // Method to calculate and show the progress.
                byte[] buffer = new byte[1024];
                int byteRead = 0;
                double read = 0;
                do
                {
                    byteRead = ftpstream.Read(buffer, 0, 1024);
                    fs.Write(buffer, 0, byteRead);
                    read += (double)byteRead;
                }
                while (byteRead != 0);
                ftpstream.Close();
                fs.Close();
            }
            catch (Exception e)
            {
               Debug.WriteLine("Exception - FTP Class - Download Method");
            }
        }
        #endregion

        #region Get List of All Server Files
        public void GetFileList()
        {
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + "127.0.0.1");
                ftpRequest.Credentials = new NetworkCredential("Mateusz", "mateusz");
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                directories = new List<string>();

                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    directories.Add(line);
                    line = streamReader.ReadLine();
                }
                streamReader.Close();
            }
            catch (WebException)
            {
               Debug.WriteLine("Exception - FTP Class - GetFileList Method");
            }
        }
        #endregion

        // SECURE FTP - SFTP

    }
}
