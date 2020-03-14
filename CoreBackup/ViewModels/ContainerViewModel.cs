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
            Screen = new SplashViewModel();
        }

        public void ChangeScreen()
        {
            Screen = new TestScreenViewModel();
        }

        public void ChangeScreenEventLog()
        {
            Screen = new EventLogViewModel();
        }
        public void ChangeScreenAdvanced()
        {
            Screen = new AdvancedViewModel();
        }

    }
}
