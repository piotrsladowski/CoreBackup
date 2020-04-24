using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using CoreBackup.Models.Config;
using CoreBackup.Models.Remote;
using ReactiveUI;
using CoreBackup.ViewModels.ConfigurationViewModels;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace CoreBackup.ViewModels
{
    public partial class ConfigurationViewModel : ViewModelBase
    {
        #region Combobox Configuration Fields
        ViewModelBase directoryLeftView;
        ViewModelBase ftpLeftView;
        ViewModelBase directoryRightView;
        ViewModelBase ftpRightView;

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

        #region Combobox
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
        #endregion

        private async void SaveConfiguration()
        {
            //Debug.WriteLine(ftpLeftConfig.Get("username"));
        }
    }
}
