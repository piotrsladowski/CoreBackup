﻿using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;
using CoreBackup.ViewModels.ConfigurationViewModels;
using System.Diagnostics;
using CoreBackup.Models.Config;

namespace CoreBackup.ViewModels
{
    public partial class ConfigurationViewModel : ViewModelBase
    {
        #region Combobox Configuration Fields
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
        #endregion

        public ConfigurationViewModel()
        {
            InitializeConfViewModels();
            SaveCommand = ReactiveCommand.Create(Save);
        }

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
                if (_cBoxRightSelectedIdx == 0)
                {
                    RightData = null;
                }
                else if (_cBoxRightSelectedIdx == 1)
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
        #endregion

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        private void Save()
        {
            ConfigHub configHub = new ConfigHub();
            //configHub.AddConfigHubEntry();
            Debug.WriteLine("Save");
        }
    }
}
