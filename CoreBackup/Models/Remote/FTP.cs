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
        public string Filename { get; set; } 
        //public string Fullname { get; set; }
        public string Server { get; set; }
        public string Password { get; set; }
        public string Path { get; set; }
        //private string Localdest { get; set; }

        public List<string> FTP_Actions = new List<string>();
        public string Action { get; set; }
        public FTP()
        {
            FTP_Actions.Add("Download");
            FTP_Actions.Add("Upload");
        }



        public void Upload()
        {
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(new Uri(string.Format("{0}/{1}", Server, Filename)));
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
        
        /*
        public static FtpWebRequest Configuration()
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", Server)));
                request.Credentials = new NetworkCredential(client_ftp.FtpSettings.Username, client_ftp.Password);
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
        */
    }
    

}
