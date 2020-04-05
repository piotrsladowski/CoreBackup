﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
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
        }

        private async void BtnBrowseLocalFiles()
        {
            Path = await GetPath();
        }

        private async Task<string> GetPath()
        {
            string[] resultReturn = null;
            string fullPath = null;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                string[] result = await dialog.ShowAsync(desktopLifetime.MainWindow);
                resultReturn = result;
                fullPath = string.Join(" ", resultReturn);
            }

            return fullPath;
        }
    }
}