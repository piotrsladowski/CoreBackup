using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CoreBackup.Models.Local;
using ReactiveUI;

namespace CoreBackup.ViewModels.ConfigurationViewModels
{
    class DirectoryConfViewModel : ViewModelBase
    {
        private ReactiveCommand<Unit, Unit> FileExplorerCommand { get; }

        private string _path;
        public string Path
        {
            get => _path;
            set => this.RaiseAndSetIfChanged(ref _path, value);
        }

        public DirectoryConfViewModel()
        {
            FileExplorerCommand = ReactiveCommand.Create(BtnBrowseLocalFiles);

            var sampledata = Enumerable.Range(0, 1)
                .Select(x => new LocalPath()
                {
                    Path = "Sample Text" + x.ToString()
                });

            Data = new ObservableCollection<LocalPath>(sampledata);
            AddNewRowCommand = new Command(AddNewRow);
        }

        private async void BtnBrowseLocalFiles()
        {
            Path = await GetPath();
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


        public ObservableCollection<LocalPath> Data { get; set; }

        public Command AddNewRowCommand { get; set; }

        private void AddNewRow()
        {
            Data.Add(new LocalPath() { Path = "Path Address" });
        }

    }
}
