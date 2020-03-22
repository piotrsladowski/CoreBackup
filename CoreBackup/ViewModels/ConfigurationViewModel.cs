using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System.Threading.Tasks;


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

        
        public ReactiveCommand<Unit, Unit> FileExplorerCommand { get; }
        public ReactiveCommand<Unit, Unit> LocalDirectoryCommand { get; }
        public ReactiveCommand<Unit, Unit> RemoteServerCommand { get; }

        public ConfigurationViewModel()
        {

            FileExplorerCommand = ReactiveCommand.Create(BtnBrowseFiles);
            LocalDirectoryCommand = ReactiveCommand.Create(LocalRadioBox);
            RemoteServerCommand = ReactiveCommand.Create(RemoteRadioBox);


        }

        public void LocalRadioBox()
        {
            RemoteServerChoice = false;
            LocalDirectoryChoice = true;
            Debug.WriteLine(LocalDirectoryChoice);

            
        }

        public void RemoteRadioBox()
        {
            LocalDirectoryChoice = false;
            RemoteServerChoice = true;
        }

        private async void BtnBrowseFiles()
        {
            Path = await GetPath();
            Debug.WriteLine(Path);
        }

        public async Task<string> GetPath()
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

        // FTP server config
    }
}
