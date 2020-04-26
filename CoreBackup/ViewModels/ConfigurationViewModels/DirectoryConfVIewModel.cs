using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
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
        private int pathNumber = 0;
        private int slotLimit = 14;


        public ObservableCollection<String> localPaths;
        public ObservableCollection<LocalPath> Data { get; set; }

        public DirectoryConfViewModel()
        {
            localPaths = new ObservableCollection<string>();
            for (int i = 0; i < 10; i++)
            {
                localPaths.Add("");
            }

            var sampledata = Enumerable.Range(0, 1)
                .Select(x => new LocalPath()
                {
                    ExplorerCommand = ReactiveCommand.Create(BtnBrowseLocalFiles)
                    
                });

            Data = new ObservableCollection<LocalPath>(sampledata);
            AddNewRowCommand = new Command(AddNewRow);
        }

        public void OnSavedConfigurationEvent(object o, ConfigurationEventArgs e)
        {
            Debug.WriteLine("OnSavedConfiguration event successfully raised");
            if (e.DataType == 1)
            {
                Debug.WriteLine("DataType: " + e.DataType + ", Side: " + e.Side);
                if (e.Side == 0)
                {
                    //e.ConfigHub.AddLeftSources(FtpConfig);
                }
                else if (e.Side == 1)
                {
                    //e.ConfigHub.AddRightSources(FtpConfig);
                }
            }
        }

        private async void BtnBrowseLocalFiles()
        {
            Data[pathNumber].Path = await GetPath();
        }

        private async Task<string> GetPath()
        {
            string resultReturn = null;
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


        public Command AddNewRowCommand { get; set; }

        private void AddNewRow()
        {
            if (pathNumber + 1 < slotLimit)
            {
                pathNumber += 1;
                Data.Add(new LocalPath() { NumericID = pathNumber, ExplorerCommand = ReactiveCommand.Create(BtnBrowseLocalFiles) });
            }
        }
    }
}
