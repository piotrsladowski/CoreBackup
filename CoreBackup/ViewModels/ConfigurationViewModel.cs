using System.Reactive;
using Avalonia.Controls;
using CoreBackup.Models.Remote;
using ReactiveUI;
using CoreBackup.ViewModels.ConfigurationViewModels;

namespace CoreBackup.ViewModels
{
    public partial class ConfigurationViewModel : ViewModelBase
    {
        #region Combobox Configuration Fields
        ViewModelBase directoryLeftView;
        ViewModelBase ftpLeftView;
        ViewModelBase directoryRightView;
        ViewModelBase ftpRightView;

        ViewModelBase leftData;
        ViewModelBase rightData;

        private FTP ftpLeft;
        private FTP ftpRight;


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
        public ConfigurationViewModel()
        {
            InitializeConfViewModels();
        }
#endregion

        #region Combobox
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
        private void InitializeConfViewModels()
        {
            ftpLeft = new FTP();
            ftpRight = new FTP();

            ftpLeftView = new FTPConfViewModel(ref ftpLeft);
            ftpRightView = new FTPConfViewModel(ref ftpRight);
            directoryLeftView = new DirectoryConfViewModel();
            directoryRightView = new DirectoryConfViewModel();
        }
        #endregion  


    }
}
