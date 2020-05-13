using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using CoreBackup.Models.Config;
using CoreBackup.Models.Tasks;
using ReactiveUI;
using CoreBackup.ViewModels.ConfigurationViewModels;
using System.Threading;
using CoreBackup.Models.Logging;

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
            ftpLeftView = new FTPConfViewModel();
            ftpRightView = new FTPConfViewModel();
            directoryLeftView = new DirectoryConfViewModel();
            directoryRightView = new DirectoryConfViewModel();
        }
        #region Events
        private void SubscribeToEvents()
        {
            SavedConfigurationLeftEvent += ftpLeftView.OnSavedConfigurationEvent;
            SavedConfigurationLeftEvent += directoryLeftView.OnSavedConfigurationEvent;
            SavedConfigurationRightEvent += ftpRightView.OnSavedConfigurationEvent;
            SavedConfigurationRightEvent += directoryRightView.OnSavedConfigurationEvent;
            RefreshSourcesEvent += directoryLeftView.OnRefreshSourcesEvent;
            RefreshSourcesEvent += directoryRightView.OnRefreshSourcesEvent;
            RefreshSourcesEvent += ftpLeftView.OnRefreshSourcesEvent;
            RefreshSourcesEvent += ftpRightView.OnRefreshSourcesEvent;
        }


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

        public event EventHandler<EventArgs> SavedConfiguration;

        protected virtual void OnSavedConfigurationEvent()
        {
            SavedConfiguration(this, EventArgs.Empty);
        }


        public event EventHandler RefreshSourcesEvent;

        protected virtual void OnRefreshSourcesEvent()
        {
            RefreshSourcesEvent(this, EventArgs.Empty);
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
            if ((_cBoxLeftSelectedIdx == 2 && !ftpLeftView.IsLogged) ||
                (_cBoxRightSelectedIdx == 2 && !ftpRightView.IsLogged))
            {
                ConfigurationName = "";
                EventLogViewModel.AddNewRegistry("You Can not save configuration without proper credentials",
                    DateTime.Now,
                    "Configuration", "ERROR");
            }
            else
            {
                if (!CoreTask.tasksList.ContainsKey(_configurationName))
                {
                    ConfigHub configHub = new ConfigHub();
                    OnSavedConfigurationLeftEvent(configHub, _cBoxLeftSelectedIdx, 0);
                    OnSavedConfigurationRightEvent(configHub, _cBoxRightSelectedIdx, 1);


                    Thread.Sleep(100); // TODO add notify after events complete
                    CoreTask.AddTaskEntry(_configurationName, configHub);
                    OnSavedConfigurationEvent();
                    Debug.WriteLine(_configurationName);

                    OnRefreshSourcesEvent();
                    ConfigurationName = "";
                }
                else
                {
                    ConfigurationName = "";
                    EventLogViewModel.AddNewRegistry("Configuration " + ConfigurationName + " already exists!", DateTime.Now,
                        "Configuration", "ERROR");
                }
            }
        }

        private void UpdateConfiguraions()
        {

        }
    }
}
