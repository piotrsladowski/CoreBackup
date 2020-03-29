using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;
using System.Diagnostics;
using Avalonia.Controls.ApplicationLifetimes;
using System.Threading.Tasks;
// FOR FTP SERVER
using System.Net;
using System.Threading;
using System.IO;
using Microsoft.Graph;
using Application = Avalonia.Application;
using System.Collections.Generic;
using CoreBackup.Models.Remote;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.SymbolStore;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using CoreBackup.Views;
using DynamicData;
using Microsoft.WindowsAPICodePack.Dialogs;
using SharpDX.Direct3D11;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using Directory = Microsoft.Graph.Directory;
using File = Microsoft.Graph.File;

namespace CoreBackup.ViewModels
{
    public partial class ConfigurationViewModel : ViewModelBase
    {
        /// <summary>
        /// SHORTCUTS FOR VARIABLES NAMES
        /// LD - Local Directory
        /// RS - Remote Server
        /// UF - Upload File
        /// DF - Download File
        /// </summary>
        

        #region LocalVsRemote RadioBox Choice
        private bool _localDirectoryChoice;

        public bool LocalDirectoryChoice
        {
            get => _localDirectoryChoice;
            set => this.RaiseAndSetIfChanged(ref _localDirectoryChoice, value);
        }

        private bool _remoteServerChoice;

        public bool RemoteServerChoice
        {
            get => _remoteServerChoice;
            set => this.RaiseAndSetIfChanged(ref _remoteServerChoice, value);
        }

        private void LocalRadioBox()
        {
            RemoteServerChoice = false;
            LocalDirectoryChoice = true;
        }

        private void RemoteRadioBox()
        {
            LocalDirectoryChoice = false;
            RemoteServerChoice = true;
        }
        #endregion
        #region Paths
        private string _path;
        public string Path
        {
            get => _path;
            set => this.RaiseAndSetIfChanged(ref _path, value);
        }

        // PATH TO FILE TO BE UPLOADED
        private string _uploadPath;
        public string UploadPath
        {
            get => _uploadPath;
            set => this.RaiseAndSetIfChanged(ref _uploadPath, value);
        }

        // DESTINATION DIRECTORY PATH - DOWNLOADED FILE
        private string _downloadPath;
        public string DownloadPath
        {
            get => _downloadPath;
            set => this.RaiseAndSetIfChanged(ref _downloadPath, value);
        }

        private string _ftpPath;
        public string FtpPath
        {
            get => _ftpPath;
            set => this.RaiseAndSetIfChanged(ref _ftpPath, value);
        }
        #endregion

        public ConfigurationViewModel()
        {
            FileExplorerCommand = ReactiveCommand.Create(BtnBrowseLocalFiles);
            LocalDirectoryCommand = ReactiveCommand.Create(LocalRadioBox);
            RemoteServerCommand = ReactiveCommand.Create(RemoteRadioBox);
            RemoteServerBrowseFileCommand = ReactiveCommand.Create(BtnServerActionFiles);
            RemoteServerActionCommand = ReactiveCommand.Create(FtpAction);
            _ftpFiles = new ObservableCollection<string>();
            ListFiles();
        }

        #region Reactive Commands and Binded Functions
        // RADIO BOXES 
        private ReactiveCommand<Unit, Unit> LocalDirectoryCommand { get; }
        private ReactiveCommand<Unit, Unit> RemoteServerCommand { get; }

        // LOCAL FILE EXPLORER
        private ReactiveCommand<Unit, Unit> FileExplorerCommand { get; }

        // FTP SERVER FILE EXPLORER
        private ReactiveCommand<Unit, Unit> RemoteServerBrowseFileCommand { get; }

        // FTP SERVER ACTION 
        private ReactiveCommand<Unit, Unit> RemoteServerActionCommand { get; }
       
        private async void BtnBrowseLocalFiles()
        {
            Path = await GetPath(false,false);
        }

        private async void BtnServerActionFiles()
        {
            if(_cBoxSelectedIdx == 0)
                FtpPath = await GetPath(false, true);
            else if(_cBoxSelectedIdx == 1)
                FtpPath = await GetPath(true, false);
        }

        private async Task<string> GetPath(bool Upload, bool Download)
        {
            string[] resultReturn = null;
            string fullPath = null;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                
                if (Download)
                {
                    OpenFolderDialog dialog = new OpenFolderDialog();
                    string result = await dialog.ShowAsync(desktopLifetime.MainWindow);
                    fullPath = result;
                    Debug.WriteLine(fullPath);
                }
                else
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    string[] result = await dialog.ShowAsync(desktopLifetime.MainWindow);
                    resultReturn = result;
                    fullPath = string.Join(" ", resultReturn);
                    if (Upload)
                    {
                        dialog.AllowMultiple = true;
                        string[] PathTreeSteps = fullPath.Split('\\');
                        FtpClient.Path = fullPath;
                        FtpClient.Upload_Filename = PathTreeSteps[PathTreeSteps.Length - 1];
                    }
                }
                
            }
            return fullPath;
        }
        #endregion

        // FTP SERVER CONFIGURATION //
        #region FTP Configuration Fields

        private FTP FtpClient = new FTP();

        private string _username;
        public string UsernameInput
        {
            get => _username;
            set
            {
                this.RaiseAndSetIfChanged(ref _username, value);
                FtpClient.Username = value;
            }
        }

        private string _password;

        public string PasswordInput
        {
            get => _password;
            set
            {
                this.RaiseAndSetIfChanged(ref _password, value);
                FtpClient.Password = value;
            }
        }

        private string _server;

        public string ServerInput
        {
            get => _server;
            set
            {
                this.RaiseAndSetIfChanged(ref _server, value);
                FtpClient.Server = value;
            }
        }


        private int _cBoxSelectedIdx;
        public int CBoxSelectedIdx
        {
            get => _cBoxSelectedIdx;
            set
            {
                this.RaiseAndSetIfChanged(ref _cBoxSelectedIdx, value);
                // Clear Fields in XAML
                FtpPath = "";
                PasswordInput = "";
                if (_cBoxSelectedIdx == 0)
                {
                    IsUpload = false;
                    IsDownload = true;
                } else if (_cBoxSelectedIdx == 1)
                {
                    IsUpload = true;
                    IsDownload = false;
                }
            }
        }

        private bool _isUpload;
        public bool IsUpload
        {
            get => _isUpload;
            set => this.RaiseAndSetIfChanged(ref _isUpload, value); 
        }

        private bool _isDownload = true;
        public bool IsDownload
        {
            get => _isDownload;
            set => this.RaiseAndSetIfChanged(ref _isDownload, value);
        }

        


        #endregion

        #region FTP Actions 
        public void FtpAction()
        {
            if(IsDownload)
                FtpClient.Download("Zadanie3.jpg", FtpPath);
            else if(IsUpload)
                FtpClient.Upload();
        }


        private ObservableCollection<string> _ftpFiles;
        public ObservableCollection<string> FtpFiles
        {
            get => _ftpFiles;
            set => this.RaiseAndSetIfChanged(ref _ftpFiles, value);
        }

        public void ListFiles()
        {
            FtpClient.GetFileList();
            foreach (var item in FtpClient.directories)
            {
                _ftpFiles.Add(item);
            }

        }
        #endregion
    }
}