using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;

namespace CoreBackup.Models.Remote
{
    public partial class FTP
    {
        private string Username { get; set; }
        private string Filename { get; set; }
        private string Fullname { get; set; }
        // Pełna scieżka do pliku
        private string Server { get; set; }
        private string Password { get; set; }
        //private string path { get; set; }
        private string Localdest { get; set; }

        
        public FTP(string Username, string Filename,  string Server, string Password,  string Localdest)
        {
            this.Username = Username;
            this.Filename = Filename;
            //this.Fullname = Fullname;
            this.Server = Server;
            this.Password = Server;
            this.Localdest = Localdest;
        }

        public static FtpWebRequest Configuration(FTP client_ftp)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", client_ftp.Server, client_ftp.Filename)));
                request.Credentials = new NetworkCredential(client_ftp.Username, client_ftp.Password);
                return request;
            }
            catch (Exception e)
            { 
                Debug.WriteLine(e.Message);
            }

            return null;
        }

        public void Download(FtpWebRequest request)
        {
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            double total = GetFileSize(request);
            /*
            Stream ftpstream = request.GetResponse().GetResponseStream();
            FileStream fs = new FileStream(Localdest, FileMode.Create);

            byte[] buffer = new byte[1024];
            int byteRead = 0;
            double read = 0;
            do
            {
                byteRead = ftpstream.Read(buffer, 0, 1024);
                fs.Write(buffer, 0, byteRead);
                read += (double)byteRead;
                double percentage = read / total * 100;
            } while (byteRead != 0);

            ftpstream.Close();
            fs.Close();
            */
        }

        public double GetFileSize(FtpWebRequest request)
        {
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            FtpWebResponse response = (FtpWebResponse) request.GetResponse();
            double total = response.ContentLength;
            response.Close();
            return total;
        }

        public void GetDateTimeStamp(FtpWebRequest request)
        {
            request.Method = WebRequestMethods.Ftp.GetDateTimestamp; 
            FtpWebResponse response = (FtpWebResponse) request.GetResponse();
            DateTime modify = response.LastModified;
            response.Close();
        }

        public void Upload(FTP client_ftp)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", Server, Filename)));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(Username, Password);
            Stream ftpstream = request.GetRequestStream();
            FileStream fs = File.OpenRead(Fullname);

            // Method to calculate and show the progress.
            byte[] buffer = new byte[1024];
            double total = (double)fs.Length;
            int byteRead = 0;
            double read = 0;
            do
            {
                byteRead = fs.Read(buffer, 0, 1024);
                ftpstream.Write(buffer, 0, byteRead);
                read += (double)byteRead;
                double percentage2 = read / total * 100;

            }
            while (byteRead != 0);
            fs.Close();
            ftpstream.Close();
        }

    }
    

}
