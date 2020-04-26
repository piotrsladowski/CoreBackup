﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using CoreBackup.Models.Config;
using CoreBackup.Models.Remote;
using CoreBackup.Models.Tasks;
using ReactiveUI;
using CoreBackup.ViewModels.ConfigurationViewModels;
using System.Threading;

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
            SubscribeToEvents();
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

        private void SubscribeToEvents()
        {
            SavedConfigurationLeftEvent += ftpLeftView.OnSavedConfigurationEvent;
            SavedConfigurationLeftEvent += directoryLeftView.OnSavedConfigurationEvent;
            SavedConfigurationRightEvent += ftpRightView.OnSavedConfigurationEvent;
            SavedConfigurationRightEvent += directoryRightView.OnSavedConfigurationEvent;
        }

        #region Events
        public event EventHandler<ConfigurationEventArgs> SavedConfigurationLeftEvent;

        protected virtual void OnSavedConfigurationLeftEvent(ConfigHub configHub, int dataType, int side)
        {
            SavedConfigurationLeftEvent(this, new ConfigurationEventArgs() { ConfigHub = configHub, DataType = dataType, Side = side});
        }

        public event EventHandler<ConfigurationEventArgs> SavedConfigurationRightEvent;

        protected virtual void OnSavedConfigurationRightEvent(ConfigHub configHub, int dataType, int side)
        {
            SavedConfigurationRightEvent(this, new ConfigurationEventArgs() { ConfigHub = configHub, DataType = dataType, Side = side });
        }
        #endregion

        private string _configurationName;

        public string ConfigurationName
        {
            get => _configurationName;
            set => this.RaiseAndSetIfChanged(ref _configurationName, value);
        }

        private async void SaveConfiguration()
        {
            ConfigHub configHub = new ConfigHub();
            OnSavedConfigurationLeftEvent(configHub, _cBoxLeftSelectedIdx, 0);
            OnSavedConfigurationRightEvent(configHub, _cBoxRightSelectedIdx, 1);

            Thread.Sleep(100); // TODO add notify after events complete
            CoreTask.AddTaskEntry(_configurationName, configHub);

            Debug.WriteLine(_configurationName);
        }
    }
}
