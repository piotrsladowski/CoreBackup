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
using CoreBackup.ViewModels;
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
        public static List<string> files;

        #region Validate Logging Process
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
                EventLogViewModel.AddNewRegistry("An error has occured during file uploading",
                    DateTime.Now, this.GetType().Name, "ERROR");
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
                EventLogViewModel.AddNewRegistry("An error has occured during file downloading",
                    DateTime.Now, this.GetType().Name, "ERROR");
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
                EventLogViewModel.AddNewRegistry("An error has occured during file listing",
                    DateTime.Now, this.GetType().Name, "ERROR");
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

        public static Int32 GetDateTimestamp(string entry, string username, string password)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(entry);
            ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
            ftpRequest.Credentials = new NetworkCredential(username, password);
            try
            {
                using (FtpWebResponse response =
                    (FtpWebResponse)ftpRequest.GetResponse())
                {
                    return (Int32) (new DateTimeOffset(response.LastModified)).ToUnixTimeSeconds();
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("File unavailable"))
                    return (Int32) (new DateTimeOffset(DateTime.Now)).ToUnixTimeSeconds();
                throw;
            }
        }


        public static long GetFileSize(string entry, string username, string password)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(entry);
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
                if (e.Message.Contains("File unavailable"))
                    return 0;
                throw;
            }
        }

        public static void ListFilesAndDirectories(string ip, string username, string password)
        {
            try
            {
                files = new List<string>();
                Queue<String> folders = new Queue<String>();
                folders.Enqueue("ftp://" + ip + "/");

                while (folders.Count > 0)
                {
                    String fld = folders.Dequeue();
                    List<String> newFiles = new List<String>();

                    FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(fld);
                    ftp.Credentials = new NetworkCredential(username, password);
                    ftp.UsePassive = false;
                    ftp.Method = WebRequestMethods.Ftp.ListDirectory;
                    using (StreamReader resp = new StreamReader(ftp.GetResponse().GetResponseStream()))
                    {
                        String line = resp.ReadLine();
                        while (line != null)
                        {
                            newFiles.Add(line.Trim());
                            line = resp.ReadLine();
                        }
                    }

                    ftp = (FtpWebRequest)FtpWebRequest.Create(fld);
                    ftp.Credentials = new NetworkCredential(username, password);
                    ftp.UsePassive = false;
                    ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                    using (StreamReader resp = new StreamReader(ftp.GetResponse().GetResponseStream()))
                    {
                        String line = resp.ReadLine();
                        while (line != null)
                        {
                            if (line.Trim().ToLower().StartsWith("d") || line.Contains(" <DIR> "))
                            {
                                String dir = newFiles.First(x => line.EndsWith(x));
                                newFiles.Remove(dir);
                                folders.Enqueue(fld + dir + "/");
                            }
                            line = resp.ReadLine();
                        }
                    }
                    files.AddRange(from f in newFiles select fld + f);
                }

                foreach (var entry in files)
                {
                    Debug.WriteLine(entry.ToString());
                }
            }
            catch (Exception e)
            {
                EventLogViewModel.AddNewRegistry("An error has occured during files listing",
                    DateTime.Now, "FTP", "ERROR");
            }
           
        }

        public static void GetAllInformationsAboutFiles(string ip, string username, string password,
            ref List<FileInformation> listFiles)
        {
            ListFilesAndDirectories(ip, username, password);
            try
            {
                foreach (var entry in files)
                {
                    FileInformation fileInfo = new FileInformation();
                    string[] entrySplitted = entry.Split("/");
                    string filename = entrySplitted[entrySplitted.Length - 1];
                    string[] filenameSplitted = filename.Split(".");
                    fileInfo.FullPath = entry;
                    fileInfo.RelativePath = filenameSplitted[0];
                    fileInfo.Extension = filenameSplitted[1];
                    fileInfo.ModificationTime = GetDateTimestamp(entry, username, password);
                    fileInfo.Size = GetFileSize(entry, username, password);
                    fileInfo.LocalPath = entry;
                    listFiles.Add(fileInfo);
                }
            }
            catch (Exception e)
            {
                EventLogViewModel.AddNewRegistry("An error has occured during collecting files' infos",
                    DateTime.Now, "FTP", "ERROR");
            }

        }

    }
}
