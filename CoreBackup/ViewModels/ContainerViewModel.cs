using System;
using System.Text;
using ReactiveUI;

namespace CoreBackup.ViewModels
{
    class ContainerViewModel : ViewModelBase
    {
        ViewModelBase actionsScreen;
        ViewModelBase configScreen;
        ViewModelBase homeScreen;
        ViewModelBase fileExplorerScreen;
        ViewModelBase eventLogScreen;

        #region Constructor
        ViewModelBase screen;
        public ViewModelBase Screen
        {
            get => screen;
            private set => this.RaiseAndSetIfChanged(ref screen, value);
        }

        public ContainerViewModel()
        {
            Screen = new HomeViewModel();
            homeScreen = new HomeViewModel();
            actionsScreen = new ActionsViewModel();
            configScreen = new ConfigurationViewModel();
            fileExplorerScreen = new FileExplorerViewModel();
            eventLogScreen = new EventLogViewModel();
        }
        #endregion

        #region ViewModel Actions
        public ViewModelBase ActionsScreen
        {
            get => actionsScreen;
            private set => this.RaiseAndSetIfChanged(ref actionsScreen, value);
        }


        public ViewModelBase ConfigScreen
        {
            get => configScreen;
            private set => this.RaiseAndSetIfChanged(ref configScreen, value);
        }

        public ViewModelBase HomeScreen
        {
            get => homeScreen;
            private set => this.RaiseAndSetIfChanged(ref homeScreen, value);
        }

        public ViewModelBase FileExplorerScreen
        {
            get => fileExplorerScreen;
            private set => this.RaiseAndSetIfChanged(ref fileExplorerScreen, value);
        }

        public ViewModelBase EventLogScreen
        {
            get => eventLogScreen;
            private set => this.RaiseAndSetIfChanged(ref eventLogScreen, value);
        }
        #endregion
        #region ChangeScreen on Click Actions
        public void ChangeScreenHome()
        {
                Screen = homeScreen;
        }

        public void ChangeScreenConfiguration()
        {           
                Screen = configScreen;
        }

        public void ChangeScreenEventLog()
        {          
                Screen = eventLogScreen;           
        }
        public void ChangeScreenActions()
        {           
                Screen = actionsScreen;
        }
        public void ChangeScreenFileExplorer()
        {          
                Screen = fileExplorerScreen;
        }
        #endregion


    }
}
