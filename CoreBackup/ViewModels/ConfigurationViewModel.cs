using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;
using System.Diagnostics;
using Avalonia.Controls.ApplicationLifetimes;
using System.Threading.Tasks;
// FOR FTP SERVER
using System.Net;
using System.Threading;
using System.IO;
using Microsoft.Graph;
using Application = Avalonia.Application;
using System.Collections.Generic;
using CoreBackup.Models.Remote;
using System;
using Avalonia.Input;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using File = Microsoft.Graph.File;

namespace CoreBackup.ViewModels
{
    public partial class ConfigurationViewModel : ViewModelBase
    {
        private bool _localDirectoryChoice;

        public bool LocalDirectoryChoice
        {
            get => _localDirectoryChoice;
            set => this.RaiseAndSetIfChanged(ref _localDirectoryChoice, value);
        }

        private bool _remoteServerChoice;

        public bool RemoteServerChoice
        {
            get => _remoteServerChoice;
            set => this.RaiseAndSetIfChanged(ref _remoteServerChoice, value);
        }

        private string _path;

        public string Path
        {
            get => _path;
            set => this.RaiseAndSetIfChanged(ref _path, value);
        }


        private ReactiveCommand<Unit, Unit> FileExplorerCommand { get; }
        private ReactiveCommand<Unit, Unit> LocalDirectoryCommand { get; }
        private ReactiveCommand<Unit, Unit> RemoteServerCommand { get; }

        public ConfigurationViewModel()
        {

            FileExplorerCommand = ReactiveCommand.Create(BtnBrowseFiles);
            LocalDirectoryCommand = ReactiveCommand.Create(LocalRadioBox);
            RemoteServerCommand = ReactiveCommand.Create(RemoteRadioBox);


        }

        private void LocalRadioBox()
        {
            RemoteServerChoice = false;
            LocalDirectoryChoice = true;
        }

        private void RemoteRadioBox()
        {
            LocalDirectoryChoice = false;
            RemoteServerChoice = true;
        }

        private async void BtnBrowseFiles()
        {
            Path = await GetPath();
            Debug.WriteLine(Path);
        }

        private async Task<string> GetPath()
        {
            string[] resultReturn = null;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filters.Add(new FileDialogFilter() {Name = "Text", Extensions = {"txt"}});
                string[] result = await dialog.ShowAsync(desktopLifetime.MainWindow);
                resultReturn = result;
            }

            //await GetPath();
            return string.Join(" ", resultReturn);

        }

        // -----------------  FTP SERVER CONFIGURATION -------------

        private string _username;
        public string UsernameInput
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        private string _password;

        public string PasswordInput
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        private string _server;

        public string ServerInput
        {
            get => _server;
            set => this.RaiseAndSetIfChanged(ref _server, value);
        }

        private string _filename;

        public string FilenameInput
        {
            get => _filename;
            set => this.RaiseAndSetIfChanged(ref _filename, value);
        }

        private int _cBoxSelectedIdx;

        public int CBoxSelectedIdx
        {
            get => _cBoxSelectedIdx;
            set => this.RaiseAndSetIfChanged(ref _cBoxSelectedIdx, value);
        }

        public void Check()
        {
            Debug.WriteLine(UsernameInput);
            Debug.WriteLine(PasswordInput);
            Debug.WriteLine(ServerInput);
            Debug.WriteLine(FilenameInput);
            Debug.WriteLine(CBoxSelectedIdx);
        }

        public void FtpAction()
        {
            string Localdest = "C:\\Users\\Mateusz\\Desktop\\AkcjePlikow";
            FTP client = new FTP(UsernameInput, FilenameInput, ServerInput, PasswordInput, Localdest);
            FtpWebRequest request = FTP.Configuration(client);
            //client.Download(client.Configuration(client));
            double total = client.GetFileSize(request);
            Debug.WriteLine(total);

        }

    }
}