using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;
using System.Diagnostics;
using Avalonia.Controls.ApplicationLifetimes;
using System.Threading.Tasks;
// FOR FTP SERVER
using Application = Avalonia.Application;
using CoreBackup.Models.Remote;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CoreBackup.Validators;
using FluentValidation;
using FluentValidation.Results;
using CoreBackup.ViewModels.ConfigurationViewModels;

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
        /// 
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

        ViewModelBase directoryLeft;
        ViewModelBase ftpLeft;
        ViewModelBase directoryRight;
        ViewModelBase ftpRight;

        ViewModelBase leftData;
        ViewModelBase rightData;
        public ViewModelBase LeftData
        {
            get => leftData;
            private set => this.RaiseAndSetIfChanged(ref leftData, value);
        }

        public ViewModelBase RightData
        {
            get => rightData;
            private set => this.RaiseAndSetIfChanged(ref rightData, value);
        }
        public ConfigurationViewModel()
        {
            InitializeConfViewModels();
            LocalDirectoryCommand = ReactiveCommand.Create(LocalRadioBox);
            RemoteServerCommand = ReactiveCommand.Create(RemoteRadioBox);

        }

        private int _cBoxLeftSelectedIdx;
        public int CBoxLeftSelectedIdx
        {
            get => _cBoxLeftSelectedIdx;
            set {
                this.RaiseAndSetIfChanged(ref _cBoxLeftSelectedIdx, value);
                // Clear Fields in XAML
                if (_cBoxLeftSelectedIdx == 1)
                {
                    LeftData = directoryLeft;
                }
                else if (_cBoxLeftSelectedIdx == 2)
                {
                    LeftData = ftpLeft;
                }
            }
        }

        private int _cBoxRightSelectedIdx;
        public int CBoxRightSelectedIdx
        {
            get => _cBoxRightSelectedIdx;
            set {
                this.RaiseAndSetIfChanged(ref _cBoxRightSelectedIdx, value);
                // Clear Fields in XAML
                if (_cBoxRightSelectedIdx == 1)
                {
                    RightData = directoryRight;
                }
                else if (_cBoxRightSelectedIdx == 2)
                {
                    RightData = ftpRight;
                }
            }
        }

        private void InitializeConfViewModels()
        {
            ftpLeft = new FTPConfViewModel();
            ftpRight = new FTPConfViewModel();
            directoryLeft = new DirectoryConfViewModel();
            directoryRight = new DirectoryConfViewModel();
        }

        #region Reactive Commands and Binded Functions
        // RADIO BOXES 
        private ReactiveCommand<Unit, Unit> LocalDirectoryCommand { get; }
        private ReactiveCommand<Unit, Unit> RemoteServerCommand { get; }

        // LOCAL FILE EXPLORER
        private ReactiveCommand<Unit, Unit> FileExplorerCommand { get; }

        // FTP SERVER FILE EXPLORER
        private ReactiveCommand<Unit, Unit> RemoteServerBrowseFileCommand { get; }

        // CONNECT TO FTP SERVER
        private ReactiveCommand<Unit, Unit> ConnectFtpCommand { get; }

        // FTP SERVER ACTION 
        private ReactiveCommand<Unit, Unit> RemoteServerActionCommand { get; }


        #endregion

    }
}
