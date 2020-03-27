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
using Avalonia.Input;
using SharpDX.Direct3D11;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
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
        #endregion



        private ReactiveCommand<Unit, Unit> FileExplorerCommand { get; }
        private ReactiveCommand<Unit, Unit> LocalDirectoryCommand { get; }
        private ReactiveCommand<Unit, Unit> RemoteServerCommand { get; }
        private ReactiveCommand<Unit, Unit> RemoteServerUploadFileCommand { get; }

        public ConfigurationViewModel()
        {

            FileExplorerCommand = ReactiveCommand.Create(BtnBrowseLocalFiles);
            LocalDirectoryCommand = ReactiveCommand.Create(LocalRadioBox);
            RemoteServerCommand = ReactiveCommand.Create(RemoteRadioBox);
            RemoteServerUploadFileCommand = ReactiveCommand.Create(BtnServerUploadFiles);

        }

       

        private async void BtnBrowseLocalFiles()
        {
            Path = await GetPath(false);
        }

        private async void BtnServerUploadFiles()
        {
            UploadPath = await GetPath(true);
        }

        private async Task<string> GetPath(bool OnlyRemoteServerPart)
        {
            string[] resultReturn = null;
            string fullPath = null;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                //dialog.Filters.Add(new FileDialogFilter());
                string[] result = await dialog.ShowAsync(desktopLifetime.MainWindow);
                resultReturn = result;
                fullPath = string.Join(" ", resultReturn);
                if (OnlyRemoteServerPart)
                {
                    string[] PathTreeSteps = fullPath.Split('\\');
                    FtpClient.Path = fullPath;
                    FtpClient.Filename = PathTreeSteps[PathTreeSteps.Length - 1];
                }
                
            }
            return fullPath;
        }

        // -----------------  FTP SERVER CONFIGURATION -------------
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
                if (_cBoxSelectedIdx == 0)
                    FtpClient.Action = FtpClient.FTP_Actions[0];
                else if (_cBoxSelectedIdx == 1)
                    FtpClient.Action = FtpClient.FTP_Actions[1];
            }
        }
        #endregion
        #region FTP Xaml Events Handling

        public void FtpAction()
        {
            if(FtpClient.Action == "Upload")
                FtpClient.Upload();
            else
            {
                
            }
        }
        #endregion
    }
}