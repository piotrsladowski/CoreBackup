using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.Graph;

namespace CoreBackup.Models.Remote
{
    public partial class FTP
    {
        // Przerobić na strukture
        public string Username { get; set; }
        public string Upload_Filename { get; set; }
        public string Server { get; set; }
        public string Password { get; set; }
        public string Path { get; set; }

        
        public List<string> directories;

        #region Upload
        public void Upload()
        {
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(new Uri(string.Format("{0}/{1}", Server, Upload_Filename)));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(Username, Password);
            Stream ftpStream = request.GetRequestStream();
            FileStream fs = System.IO.File.OpenRead(Path);
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
        #endregion
        #region Download
        public void Download(string filename, string destinationPath)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", Server, filename)));
            request.Credentials = new NetworkCredential(Username, Password);
            request.Method = WebRequestMethods.Ftp.DownloadFile;  


            FtpWebRequest request1 = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", Server, filename)));
            request1.Credentials = new NetworkCredential(Username, Password);
            request1.Method = WebRequestMethods.Ftp.GetFileSize;  
            FtpWebResponse response = (FtpWebResponse)request1.GetResponse();
            double total = response.ContentLength;
            response.Close();

            FtpWebRequest request2 = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", Server, filename)));
            request2.Credentials = new NetworkCredential(Username, Password);
            request2.Method = WebRequestMethods.Ftp.GetDateTimestamp; 
            FtpWebResponse response2 = (FtpWebResponse)request2.GetResponse();
            DateTime modify = response2.LastModified;
            response2.Close();


            Stream ftpstream = request.GetResponse().GetResponseStream();
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
                //double percentage = read / total * 100;
                //backgroundWorker1.ReportProgress((int)percentage);
            }
            while (byteRead != 0);
            ftpstream.Close();
            fs.Close();
        }
        #endregion
        #region Get List of All Server Files
        public void GetFileList()
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(Server);
            ftpRequest.Credentials = new NetworkCredential(Username, Password);
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
        #endregion
        #region GetFileSize
        public double GetFileSize(FtpWebRequest request)
        {
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            double total = response.ContentLength;
            response.Close();
            return total;
        }
        #endregion

    }


}
