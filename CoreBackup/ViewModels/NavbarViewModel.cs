using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace CoreBackup.ViewModels
{
    class NavbarViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> OpenSettingsWindowCommand { get; }
        //public ReactiveCommand<Unit, Unit> ExitAppCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenAboutWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitAppCommand { get; }
        public NavbarViewModel()
        {
            OpenSettingsWindowCommand = ReactiveCommand.Create(OpenSettingsWindow);
            OpenAboutWindowCommand = ReactiveCommand.Create(OpenAboutWindow);
            ExitAppCommand = ReactiveCommand.Create(ExitApp);
        }


        private void OpenSettingsWindow()
        {
            var window = new Windows.SettingsWindow();
            window.Show();
        }

        private void OpenAboutWindow()
        {
            var window = new Windows.AboutWindow();
            window.Show();
            
        }

        private void ExitApp()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime) 
            { desktopLifetime.Shutdown(); 
            } 
        }

    }
}
