using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CoreBackup.Models.Config;
using CoreBackup.Models.Local;
using ReactiveUI;

namespace CoreBackup.ViewModels.ConfigurationViewModels
{
    class DirectoryConfViewModel : ViewModelBase
    {
        #region Variables
        private int counter = 0;
        private readonly int slotLimits = 10;
        
        public Command AddNewRowCommand { get; set; }
        public Command EraseLastRowCommand { get; set; }
        public ReactiveCommand<Unit, Unit>[] commandsArray;
        public ObservableCollection<LocalPath> Data { get; set; }
        private DirectoryConfig directoryConfig;
        #endregion

        #region Constructor
        public DirectoryConfViewModel()
        {
            AddNewRowCommand = new Command(AddNewRow);
            EraseLastRowCommand = new Command(EraseLastRow);
            EraseRowVisible = false;
            directoryConfig = new DirectoryConfig();
            Data = new ObservableCollection<LocalPath>();
            commandsArray = new ReactiveCommand<Unit, Unit>[10];
            commandsArray[0] = ReactiveCommand.Create(Btn0BrowseLocalFiles);
            commandsArray[1] = ReactiveCommand.Create(Btn1BrowseLocalFiles);
            commandsArray[2] = ReactiveCommand.Create(Btn2BrowseLocalFiles);
            commandsArray[3] = ReactiveCommand.Create(Btn3BrowseLocalFiles);
            commandsArray[4] = ReactiveCommand.Create(Btn4BrowseLocalFiles);
            commandsArray[5] = ReactiveCommand.Create(Btn5BrowseLocalFiles);
            commandsArray[6] = ReactiveCommand.Create(Btn6BrowseLocalFiles);
            commandsArray[7] = ReactiveCommand.Create(Btn7BrowseLocalFiles);
            commandsArray[8] = ReactiveCommand.Create(Btn8BrowseLocalFiles);
            commandsArray[9] = ReactiveCommand.Create(Btn9BrowseLocalFiles);
        }
        #endregion

        #region Buttons Binded Functions
        private bool eraseRowVisible;
        public bool EraseRowVisible
        {
            get => eraseRowVisible;
            set => this.RaiseAndSetIfChanged(ref eraseRowVisible, value);
        }

        private async void Btn0BrowseLocalFiles()
        {
            Data[0].Path = await GetPath();
            directoryConfig.ProvideLocalPath(0, Data[0].Path);
        }

        private async void Btn1BrowseLocalFiles()
        {
            Data[1].Path = await GetPath();
            directoryConfig.ProvideLocalPath(1, Data[1].Path);
        }

        private async void Btn2BrowseLocalFiles()
        {
            Data[2].Path = await GetPath();
            directoryConfig.ProvideLocalPath(2, Data[2].Path);
        }

        private async void Btn3BrowseLocalFiles()
        {
            Data[3].Path = await GetPath();
            directoryConfig.ProvideLocalPath(3, Data[3].Path);
        }

        private async void Btn4BrowseLocalFiles()
        {
            Data[4].Path = await GetPath();
            directoryConfig.ProvideLocalPath(4, Data[4].Path);
        }

        private async void Btn5BrowseLocalFiles()
        {
            Data[5].Path = await GetPath();
            directoryConfig.ProvideLocalPath(5, Data[5].Path);
        }

        private async void Btn6BrowseLocalFiles()
        {
            Data[6].Path = await GetPath();
            directoryConfig.ProvideLocalPath(6, Data[6].Path);
        }

        private async void Btn7BrowseLocalFiles()
        {
            Data[7].Path = await GetPath();
            directoryConfig.ProvideLocalPath(7, Data[7].Path);
        }

        private async void Btn8BrowseLocalFiles()
        {
            Data[8].Path = await GetPath();
            directoryConfig.ProvideLocalPath(8, Data[8].Path);
        }

        private async void Btn9BrowseLocalFiles()
        {
            Data[9].Path = await GetPath();
            directoryConfig.ProvideLocalPath(9, Data[9].Path);
        }
        #endregion

        #region GetPath
        private async Task<string> GetPath()
        {
            string resultReturn;
            string fullPath = null;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                OpenFolderDialog dialog = new OpenFolderDialog();
                //string result = await d.ShowAsync(desktopLifetime.MainWindow);
                //OpenFileDialog dialog = new OpenFileDialog();
                string result = await dialog.ShowAsync(desktopLifetime.MainWindow);
                resultReturn = result;
                fullPath = string.Join(" ", resultReturn);
            }

            return fullPath;
        }
        #endregion

        #region Dynamic Rows Manipulation
        private void AddNewRow()
        {
            if (counter >= 0)
            {
                EraseRowVisible = true;
            } else

            {
                EraseRowVisible = false;
            }

            if (counter + 1 <= slotLimits)
            {
                Data.Add(new LocalPath() { NumericID = counter, ExplorerCommand = commandsArray[counter] });
                EventLogViewModel.AddNewRegistry("Local Path Slot Added",
                    DateTime.Now, this.GetType().Name, "LOW");
                counter += 1;
            }
        }

        private void EraseLastRow()
        {
            if (counter > 0)
            {
                Data.RemoveAt(counter-1);
                EventLogViewModel.AddNewRegistry("Local Path Slot Removed",
                    DateTime.Now, this.GetType().Name, "LOW");
                counter -= 1;
                if (counter == 0)
                {
                    EraseRowVisible = false;
                }
            }
        }
        #endregion

        #region Handled Events
        public void OnSavedConfigurationEvent(object o, ConfigurationEventArgs e)
        {
            Debug.WriteLine("OnSavedConfiguration event successfully raised from DirectoryConf");
            if (e.DataType == 1)
            {
                Debug.WriteLine("DataType: " + e.DataType + ", Side: " + e.Side);
                if (e.Side == 0)
                {

                    e.ConfigHub.AddLeftSources(directoryConfig);
                    EventLogViewModel.AddNewRegistry("DataType: " + e.DataType + ", Side: " + e.Side + "Config Saved", DateTime.Now, this.GetType().Name, "MEDIUM");
                }
                else if (e.Side == 1)
                {
                    e.ConfigHub.AddRightSources(directoryConfig);
                    EventLogViewModel.AddNewRegistry("DataType: " + e.DataType + ", Side: " + e.Side + "Config Saved", DateTime.Now, this.GetType().Name, "MEDIUM");
                }
            }
            directoryConfig = new DirectoryConfig();
        }

        public void OnRefreshSourcesEvent(object o, EventArgs e)
        {
            counter = 0;
            Data.Clear();
        }
        #endregion
    }
}
