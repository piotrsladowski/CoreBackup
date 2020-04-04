using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using ReactiveUI;

namespace CoreBackup.ViewModels.ConfigurationViewModels
{
    class DirectoryConfViewModel : ViewModelBase
    {
        private bool _localDirectoryChoice;

        public bool LocalDirectoryChoice
        {
            get => _localDirectoryChoice;
            set => this.RaiseAndSetIfChanged(ref _localDirectoryChoice, value);
        }

        private string _path;
        public string Path
        {
            get => _path;
            set => this.RaiseAndSetIfChanged(ref _path, value);
        }


        private ReactiveCommand<Unit, Unit> LocalDirectoryCommand { get; }
        private ReactiveCommand<Unit, Unit> FileExplorerCommand { get; }
        public DirectoryConfViewModel()
        {
            FileExplorerCommand = ReactiveCommand.Create(BtnBrowseLocalFiles);
            LocalDirectoryCommand = ReactiveCommand.Create(LocalRadioBox);
        }

        private async void BtnBrowseLocalFiles()
        {
            //Path = await GetPath(false, false);
        }
        private void LocalRadioBox()
        {
            //RemoteServerChoice = false;
            //LocalDirectoryChoice = true;
        }
    }
}
