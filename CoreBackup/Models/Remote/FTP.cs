using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CoreBackup.Models.IO;
using DynamicData;
using File = System.IO.File;

namespace CoreBackup.Models.Remote
{
    public partial class FTP
    {
        public string Username { get; set; }
        public string Server { get; set; }
        public string Password { get; set; }

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
        public void Upload(string filename, string sourcePath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest) WebRequest.Create(new Uri(string.Format("{0}/{1}", "ftp://" + Server, filename)));
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(Username, Password);
                Stream ftpStream = request.GetRequestStream();
                FileStream fs = File.OpenRead(sourcePath);
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
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + Server);
                ftpRequest.Credentials = new NetworkCredential(Username,Password);
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

        #region
        public long GetFileSize(string filename)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + Server + "//" + filename);
            ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
            ftpRequest.Credentials = new NetworkCredential(Username, Password);
            try
            {
                using (FtpWebResponse response =
                    (FtpWebResponse)ftpRequest.GetResponse())
                {
                    return response.ContentLength;
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("File unavailable")) return -1;
                    throw;
            }
        }
        #endregion

        public static Int32 GetDateTimestamp(string filename, string ip, string username, string password)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + ip + "/" + filename);
            ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
            ftpRequest.Credentials = new NetworkCredential(username, password);
            try
            {
                using (FtpWebResponse response =
                    (FtpWebResponse)ftpRequest.GetResponse())
                {
                    return (Int32)(DateTime.UtcNow.Subtract(response.LastModified)).TotalSeconds;
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("File unavailable"))
                    return (Int32)(DateTime.UtcNow.Subtract(new DateTime(2020, 1, 1))).TotalSeconds;
                throw;
            }
        }


        public static long GetFileSize(string filename, string ip, string username, string password)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + ip + "/" + filename);
            ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
            ftpRequest.Credentials = new NetworkCredential(username, password);
            try
            {
                using (FtpWebResponse response =
                    (FtpWebResponse)ftpRequest.GetResponse())
                {
                    return (long)response.ContentLength;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("OKEJ");
                if (e.Message.Contains("File unavailable"))
                    return 0;
                throw;
            }
        }

        public static void GetAllInformationsAboutFiles(string ip, string username, string password, ref List<FileInformation> listFiles, string beforeFilename)
        {
            var url = "ftp://" + ip;
            var request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(username, password);

            try
            {
                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        var reader = new StreamReader(responseStream);
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            if (string.IsNullOrWhiteSpace(line) == false)
                            {
                                string[] lineComponents = line.Split('/');
                                string[] lineChecker= line.Split('.');

                                FileInformation fi = new FileInformation();

                                // DIRECTORIES
                                if (lineChecker.Length == 1)
                                {
                                    string directory = lineChecker[0];
                                    GetAllInformationsAboutFiles(ip+"/"+directory, username, password, ref listFiles, directory);
                                }
                                // FILE
                                else if (lineChecker.Length == 2)
                                {
                                    string fileName = lineComponents.Last();
                                    string[] fileNameSplitted = fileName.Split(".");
                                    fi.RelativePath = fileNameSplitted[0];
                                    fi.Extension = fileNameSplitted[1];
                                    if (beforeFilename.Equals(""))
                                    {
                                        fi.FullPath = beforeFilename + fileName;
                                    }
                                    else
                                    {
                                        fi.FullPath = beforeFilename + "/" + fileName;
                                    } 
                                    fi.ModificationTime = FTP.GetDateTimestamp(fi.FullPath, ip, username, password);
                                    fi.Size = FTP.GetFileSize(fi.FullPath, ip, username, password);
                                    listFiles.Add(fi);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
