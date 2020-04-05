using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;
using System.Diagnostics;
using Avalonia.Controls.ApplicationLifetimes;
using System.Threading.Tasks;
// FOR FTP SERVER
using Application = Avalonia.Application;
using CoreBackup.Models.Remote;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CoreBackup.Validators;
using FluentValidation;
using FluentValidation.Results;
using CoreBackup.ViewModels.ConfigurationViewModels;

namespace CoreBackup.ViewModels.ConfigurationViewModels
{
    class FTPConfViewModel : ViewModelBase
    {

       
        /// <summary>
        /// SHORTCUTS FOR VARIABLES NAMES
        /// LD - Local Directory
        /// RS - Remote Server
        /// UF - Upload File
        /// DF - Download File
        /// </summary>
        /// 

        #region Paths
        // PATH TO FILE TO BE UPLOADED
        private string _uploadPath;
        public string UploadPath
        {
            get => _uploadPath;
            set => this.RaiseAndSetIfChanged(ref _uploadPath, value);
        }

        // DESTINATION DIRECTORY PATH - DOWNLOADED FILE
        private string _downloadPath;
        public string DownloadPath
        {
            get => _downloadPath;
            set => this.RaiseAndSetIfChanged(ref _downloadPath, value);
        }
        #endregion

        // PATH SEEN BY USER 
        private string _ftpPath;

        public string FtpPath
        {
            get => _ftpPath;
            set => this.RaiseAndSetIfChanged(ref _ftpPath, value);
        }

        public FTPConfViewModel()
        {
            RemoteServerBrowseFileCommand = ReactiveCommand.Create(BtnServerActionFiles);
            RemoteServerActionCommand = ReactiveCommand.Create(FtpAction);
            ConnectFtpCommand = ReactiveCommand.Create(FtpConnect);
            _ftpFiles = new ObservableCollection<string>();
        }
        #region Reactive Commands

        // FTP SERVER FILE EXPLORER
        private ReactiveCommand<Unit, Unit> RemoteServerBrowseFileCommand { get; }

        // CONNECT TO FTP SERVER
        private ReactiveCommand<Unit, Unit> ConnectFtpCommand { get; }

        // FTP SERVER ACTION 
        private ReactiveCommand<Unit, Unit> RemoteServerActionCommand { get; }
        #endregion

        private async void BtnServerActionFiles()
        {
            if (_cBoxSelectedIdx == 0)
            {
                DownloadPath = await GetPath(false, true);
                // Export Path to User Interface
                FtpPath = DownloadPath;
            }
            else if (_cBoxSelectedIdx == 1)
            {
                UploadPath = await GetPath(true, false);
                // Export Path to User Interface
                FtpPath = UploadPath;
            }
        }

        private async Task<string> GetPath(bool Upload, bool Download)
        {
            string[] resultReturn = null;
            string fullPath = null;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {

                if (Download)
                {
                    OpenFolderDialog dialog = new OpenFolderDialog();
                    string result = await dialog.ShowAsync(desktopLifetime.MainWindow);
                    fullPath = result;
                }
                else
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    string[] result = await dialog.ShowAsync(desktopLifetime.MainWindow);
                    resultReturn = result;
                    fullPath = string.Join(" ", resultReturn);
                    if (Upload)
                    {
                        dialog.AllowMultiple = true;
                        string[] PathTreeSteps = fullPath.Split('\\');
                        ToUploadFile = PathTreeSteps[PathTreeSteps.Length - 1];
                    }
                }

            }
            return fullPath;
        }

        // FTP SERVER CONFIGURATION //
        #region FTP Configuration Fields

        private FTP FtpClient = new FTP();

        private string _username;
        public string UsernameInput
        {
            get => _username;
            set {
                this.RaiseAndSetIfChanged(ref _username, value);
            }
        }

        private string _password;

        public string PasswordInput
        {
            get => _password;
            set {
                this.RaiseAndSetIfChanged(ref _password, value);
            }
        }

        private string _server;

        public string ServerInput
        {
            get => _server;
            set {
                this.RaiseAndSetIfChanged(ref _server, value);
            }
        }

        private int _cBoxSelectedIdx;
        public int CBoxSelectedIdx
        {
            get => _cBoxSelectedIdx;
            set {
                this.RaiseAndSetIfChanged(ref _cBoxSelectedIdx, value);
                // Clear Fields in XAML
                FtpPath = "";
                if (_cBoxSelectedIdx == 0)
                {
                    IsUpload = false;
                    IsDownload = true;
                    ListFiles();
                }
                else if (_cBoxSelectedIdx == 1)
                {
                    IsUpload = true;
                    IsDownload = false;
                    ListFiles();
                }
            }
        }

        // True if Upload Option is a ComboBox's choice
        private bool _isUpload;
        public bool IsUpload
        {
            get => _isUpload;
            set => this.RaiseAndSetIfChanged(ref _isUpload, value);
        }

        // True if Download Option is a ComboBox's choice
        private bool _isDownload = true;
        public bool IsDownload
        {
            get => _isDownload;
            set => this.RaiseAndSetIfChanged(ref _isDownload, value);
        }

        // Validation State
        private bool _isLogged;
        public bool IsLogged
        {
            get => _isLogged;
            set => this.RaiseAndSetIfChanged(ref _isLogged, value);
        }


        // Name of the file which is going to be Downloaded
        private string _toDownloadFile;
        public string ToDownloadFile
        {
            get => _toDownloadFile;
            set => this.RaiseAndSetIfChanged(ref _toDownloadFile, value);
        }

        // Name of the file which is going to be Uploaded
        private string _toUploadFile;

        public string ToUploadFile
        {
            get => _toUploadFile;
            set => this.RaiseAndSetIfChanged(ref _toUploadFile, value);
        }

        #endregion

        #region FTP Actions
        // Collection contains the names of files on the server
        private ObservableCollection<string> _ftpFiles;
        public ObservableCollection<string> FtpFiles
        {
            get => _ftpFiles;
            set => this.RaiseAndSetIfChanged(ref _ftpFiles, value);
        }


        // Get All files' names and inject them to local Collection
        private void ListFiles()
        {
            _ftpFiles.Clear();
            FtpClient.GetFileList();
            foreach (var item in FtpClient.directories)
            {
                _ftpFiles.Add(item);
            }
        }

        // Collection contains the validation failures
        private BindingList<string> errors = new BindingList<string>();
        private ObservableCollection<string> _errorMessages = new ObservableCollection<string>();

        public ObservableCollection<string> ErrorMessages
        {
            get => _errorMessages;
            set => this.RaiseAndSetIfChanged(ref _errorMessages, value);
        }

        private void FtpConnect()
        {
            errors.Clear();
            ErrorMessages.Clear();
            FTPConfViewModelValidator validator = new FTPConfViewModelValidator();
            //ConfigurationViewModelValidator validator = new ConfigurationViewModelValidator();
            ValidationResult results = validator.Validate(this);

            if (results.IsValid == false)
            {
                foreach (ValidationFailure failure in results.Errors)
                {
                    errors.Add($"{failure.PropertyName} : {failure.ErrorMessage}");
                    ErrorMessages.Add($"{failure.ErrorMessage}");
                }
            }
            else
            {
                FtpClient.Username = UsernameInput;
                FtpClient.Password = PasswordInput;
                FtpClient.Server = ServerInput;

                try
                {
                    IsLogged = FtpClient.ValidateLogging();
                    if (IsLogged)
                    {
                        ListFiles();
                    }
                    else
                    {
                        ErrorMessages.Add("Access denied - wrong Username / Password / IP");
                    }

                }
                catch (NullReferenceException) { }
                catch (Exception) { }
            }
        }

        // DOWNLOAD or UPLOAD Action
        private void FtpAction()
        {
            if (IsDownload)
            {
                FtpClient.Download(ToDownloadFile, DownloadPath);
            }
            else if (IsUpload)
            {
                FtpClient.Upload(ToUploadFile, UploadPath);
            }
        }

        #endregion
    }
}
