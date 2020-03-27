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
        public string Filename_Download { get; set; }
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

        public void Download()
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", Server, Filename)));
            request.Credentials = new NetworkCredential(Username, Password);
            request.Method = WebRequestMethods.Ftp.DownloadFile;  


            FtpWebRequest request1 = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", Server, Filename)));
            request1.Credentials = new NetworkCredential(Username, Password);
            request1.Method = WebRequestMethods.Ftp.GetFileSize;  
            FtpWebResponse response = (FtpWebResponse)request1.GetResponse();
            double total = response.ContentLength;
            response.Close();

            FtpWebRequest request2 = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", Server, Filename)));
            request2.Credentials = new NetworkCredential(Username, Password);
            request2.Method = WebRequestMethods.Ftp.GetDateTimestamp; 
            FtpWebResponse response2 = (FtpWebResponse)request2.GetResponse();
            DateTime modify = response2.LastModified;
            response2.Close();


            Stream ftpstream = request.GetResponse().GetResponseStream();
            FileStream fs = new FileStream(Path, FileMode.Create);

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


        public List<String> GetFileList()
        {
            List<String> file_list = new List<String>();
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(Server);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(Username, Password);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            String line = String.Empty;
            while ((line = reader.ReadLine()) != null)
            {
                file_list.Add(line);
            }

            reader.Close();
            response.Close();
            return file_list;

        }

        public double GetFileSize(FtpWebRequest request)
        {
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            double total = response.ContentLength;
            response.Close();
            return total;
        }


        /*
        
        
        public void Download(FtpWebRequest request)
        {
        ;
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


        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", Server, Filename)));
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(Username, Password);
            FtpWebResponse response = (FtpWebResponse) request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            String result = reader.ReadToEnd();
            //double total = GetFileSize(request);
        */
    }


}
