using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;

namespace CoreBackup.ViewModels
{
    class ContainerViewModel : ViewModelBase
    {
        ViewModelBase screen;
        public ViewModelBase Screen
        {
            get => screen;
            private set => this.RaiseAndSetIfChanged(ref screen, value);
        }

        public ContainerViewModel()
        {
            Screen = new HomeViewModel();
        }

        public void ChangeScreenHome()
        {
            Screen = new HomeViewModel();
        }

        public void ChangeScreenConfiguration()
        {
            Screen = new ConfigurationViewModel();
        }

        public void ChangeScreenEventLog()
        {
            Screen = new EventLogViewModel();
        }
        public void ChangeScreenActions()
        {
            Screen = new ActionsViewModel();
        }

        public void ChangeScreenFileExplorer()
        {
            Screen = new FileExplorerViewModel();
        }

       
    }
}
