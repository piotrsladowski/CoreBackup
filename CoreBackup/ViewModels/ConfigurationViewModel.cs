using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using CoreBackup.Models.Config;
using CoreBackup.Models.Remote;
using CoreBackup.Models.Tasks;
using ReactiveUI;
using CoreBackup.ViewModels.ConfigurationViewModels;

namespace CoreBackup.ViewModels
{
    public partial class ConfigurationViewModel : ViewModelBase
    {
        #region Combobox Configuration Fields
        DirectoryConfViewModel directoryLeftView;
        FTPConfViewModel ftpLeftView;
        DirectoryConfViewModel directoryRightView;
        FTPConfViewModel ftpRightView;

        ViewModelBase leftData;
        ViewModelBase rightData;

        private FTP ftpLeft;
        private FTP ftpRight;

        private FTPConfig ftpLeftConfig;
        private FTPConfig ftpRightConfig;

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

        public ReactiveCommand<Unit,Unit> SaveConfigurationCommand { get; }
        public ConfigurationViewModel()
        {
            SaveConfigurationCommand = ReactiveCommand.Create(SaveConfiguration);
            InitializeConfViewModels();
        }

        #endregion

        #region BoomBox
        private int _cBoxLeftSelectedIdx;
        public int CBoxLeftSelectedIdx
        {
            get => _cBoxLeftSelectedIdx;
            set {
                this.RaiseAndSetIfChanged(ref _cBoxLeftSelectedIdx, value);
                // Clear Fields in XAML
                if (_cBoxLeftSelectedIdx == 0)
                {
                    LeftData = null;
                }
                if (_cBoxLeftSelectedIdx == 1)
                {
                    LeftData = directoryLeftView;
                }
                else if (_cBoxLeftSelectedIdx == 2)
                {
                    LeftData = ftpLeftView;
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
                if (_cBoxRightSelectedIdx == 0)
                {
                    RightData = null;
                }
                else if (_cBoxRightSelectedIdx == 1)
                {
                    RightData = directoryRightView;
                }
                else if (_cBoxRightSelectedIdx == 2)
                {
                    RightData = ftpRightView;
                }
            }
        }
        #endregion

        private void InitializeConfViewModels()
        {
            ftpLeft = new FTP();
            ftpRight = new FTP();

            ftpLeftConfig = new FTPConfig();
            ftpRightConfig = new FTPConfig();

            ftpLeftView = new FTPConfViewModel(ref ftpLeft, ref ftpLeftConfig);
            ftpRightView = new FTPConfViewModel(ref ftpRight, ref ftpRightConfig);
            directoryLeftView = new DirectoryConfViewModel();
            directoryRightView = new DirectoryConfViewModel();
        }

        private string _configurationName;

        public string ConfigurationName
        {
            get => _configurationName;
            set => this.RaiseAndSetIfChanged(ref _configurationName, value);
        }

        private async void SaveConfiguration()
        {


            Debug.WriteLine(_configurationName);
            if (ftpLeftConfig.GetCredentials().Count == 3 && ftpLeftConfig.GetPaths().Count == 2)
            {
                CoreTask.ftpConf.Add(ftpLeftConfig);
            }
            if (ftpRightConfig.GetCredentials().Count == 3 && ftpRightConfig.GetPaths().Count == 2)
            {
                CoreTask.ftpConf.Add(ftpRightConfig);
            }

        }
    }
}
