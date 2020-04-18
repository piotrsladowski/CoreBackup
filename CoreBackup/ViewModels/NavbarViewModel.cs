using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Runtime.InteropServices;
using System.Text;
using Avalonia.Controls;
using CoreBackup.Models.Config;
using DynamicData;

namespace CoreBackup.ViewModels
{
    class NavbarViewModel : ViewModelBase
    {
        private ReactiveCommand<Unit, Unit> OpenGitWebsiteCommand { get; }
        private void OpenGitWebsite()
        {
            string url = "https://github.com/piotrsladowski/CoreBackup";
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

        public ReactiveCommand<Unit, Unit> SaveAsConfigCommand { get; }

        public ReactiveCommand<Unit, Unit> OpenSettingsWindowCommand { get; }
        //public ReactiveCommand<Unit, Unit> ExitAppCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenAboutWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitAppCommand { get; }
        public NavbarViewModel()
        {
            OpenSettingsWindowCommand = ReactiveCommand.Create(OpenSettingsWindow);
            OpenAboutWindowCommand = ReactiveCommand.Create(OpenAboutWindow);
            ExitAppCommand = ReactiveCommand.Create(ExitApp);
            OpenGitWebsiteCommand = ReactiveCommand.Create(OpenGitWebsite);

            SaveAsConfigCommand = ReactiveCommand.Create(SaveAs);
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

        private async void SaveAs()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filters.Add(new FileDialogFilter(){Name= "Json files (*.json)|Text files (*.txt)", Extensions = {"json","txt"}});
                string result = await dialog.ShowAsync(desktopLifetime.MainWindow);
                string[] resultArray = result.Split("\\");
                CoreTask.configFilename = resultArray[resultArray.Length - 1];
                resultArray = resultArray.Where((source, index) => index != (resultArray.Length - 1)).ToArray();
                CoreTask.jsonConfigPath = string.Join("\\", resultArray);

                Debug.WriteLine(CoreTask.configFilename);
                Debug.WriteLine(CoreTask.jsonConfigPath);
            }
            CoreTask.saveConfigToJsonFile();
        }
    }
}
