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
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using Avalonia.Themes.Default;
using CoreBackup.Models.Config;
using DynamicData;
using CoreBackup.Models.Tasks;

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
                EventLogViewModel.AddNewRegistry("CoreBackup GitHub Page visited",
                    DateTime.Now, "Git Hub", "Low");
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
        public ReactiveCommand<Unit, Unit> SaveConfigCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenConfigCommand { get; }

        public ReactiveCommand<Unit, Unit> OpenSettingsWindowCommand { get; }
        //public ReactiveCommand<Unit, Unit> ExitAppCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenAboutWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitAppCommand { get; }
        public ReactiveCommand<Unit, Unit> ChangeThemeCommand { get; }
        public NavbarViewModel()
        {
            OpenSettingsWindowCommand = ReactiveCommand.Create(OpenSettingsWindow);
            OpenAboutWindowCommand = ReactiveCommand.Create(OpenAboutWindow);
            ExitAppCommand = ReactiveCommand.Create(ExitApp);
            OpenGitWebsiteCommand = ReactiveCommand.Create(OpenGitWebsite);

            SaveAsConfigCommand = ReactiveCommand.Create(SaveAs);
            SaveConfigCommand = ReactiveCommand.Create(Save);
            OpenConfigCommand = ReactiveCommand.Create(Open);

            ChangeThemeCommand = ReactiveCommand.Create(ChangeTheme);
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
                if (!string.IsNullOrEmpty(result))
                {
                    string[] resultArray = result.Split("\\");
                    CoreTask.configFilename = resultArray[^1];
                    resultArray = resultArray.Where((source, index) => index != (resultArray.Length - 1)).ToArray();
                    CoreTask.jsonConfigPath = string.Join("\\", resultArray);
                    CoreTask.SaveConfigToJsonFile();
                    EventLogViewModel.AddNewRegistry("Configuration has been saved to JSON",
                        DateTime.Now, "Git Hub", "Low");

                }
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(CoreTask.configFilename) || string.IsNullOrEmpty(CoreTask.jsonConfigPath))
            {

            }
            else
            {
                CoreTask.SaveConfigToJsonFile();
                EventLogViewModel.AddNewRegistry("Configuration has been updated in JSON",
                    DateTime.Now, "Git Hub", "Low");

            }
        }

        private async void Open()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filters.Add(new FileDialogFilter() { Name = "Json files (*.json)|Text files (*.txt)", Extensions = { "json", "txt" } });
                string[] result = await dialog.ShowAsync(desktopLifetime.MainWindow);
                string fullPath = string.Join(" ", result);
                if (!string.IsNullOrEmpty(fullPath))
                {
                    string[] resultArray = fullPath.Split("\\");
                    CoreTask.configFilename = resultArray[^1];
                    resultArray = resultArray.Where((source, index) => index != (resultArray.Length - 1)).ToArray();
                    CoreTask.jsonConfigPath = string.Join("\\", resultArray);
                    CoreTask.ReadConfigFromJsonFile();
                    EventLogViewModel.AddNewRegistry("Configuration has been loaded from JSON",
                        DateTime.Now, "Git Hub", "Low");
                }
            }
        }

        private int themeTracker = 1;

        private void ChangeTheme()
        {
            if (themeTracker % 2 == 1)
            {
                ChangeThemeToLight();
                themeTracker += 1;
            } 
            else if (themeTracker % 2 == 0)
            {
                ChangeThemeToDark();
                themeTracker += 1;
            }
        }

        private void ChangeThemeToLight()
        {
            // create new style
            var newStyle = new StyleInclude(new Uri("avares://AvaloniaApplicationTest/App.xaml"))
            {
                Source = new Uri("avares://Avalonia.Themes.Default/Accents/BaseLight.xaml")
            };
            // load style to get access to the ressources
            var baseDarkStyle = newStyle.Loaded as Style;

            // get the original source (BaseDark)
            var ressourceFromAppXaml = ((Style)((StyleInclude)Application.Current.Styles[1]).Loaded).Resources;
            foreach (var item in baseDarkStyle.Resources)
            {
                // for secure lookup if the key exists for the resource otherwise create it
                if (ressourceFromAppXaml.ContainsKey(item.Key))
                    ressourceFromAppXaml[item.Key] = item.Value;
                else
                    ressourceFromAppXaml.Add(item.Key, item.Value);
            }
            // set source name for the new theme
            ((StyleInclude)Application.Current.Styles[1]).Source = new Uri("avares://Avalonia.Themes.Default/Accents/BaseLight.xaml");

            EventLogViewModel.AddNewRegistry("Application Theme has been changed to Light",
                DateTime.Now, this.GetType().Name, "LOW");
        }
    

        private void ChangeThemeToDark()
        {
            // Create new style.
            var newStyle = new StyleInclude(new Uri("avares://AvaloniaApplicationTest/App.xaml"))
            {
                Source = new Uri("avares://Avalonia.Themes.Default/Accents/BaseDark.xaml")
            };
            // Load style to get access to the ressources.
            var baseLightStyle = newStyle.Loaded as Style;

            // Get the original source (BaseDark).
            var ressourceFromAppXaml = ((Style)((StyleInclude)Application.Current.Styles[1]).Loaded).Resources;
            foreach (var item in baseLightStyle.Resources)
            {
                // For secure lookup if the key exists for the resource otherwise create it.
                if (ressourceFromAppXaml.ContainsKey(item.Key))
                    ressourceFromAppXaml[item.Key] = item.Value;
                else
                    ressourceFromAppXaml.Add(item.Key, item.Value);
            }
            // Set source name for the new theme.
            ((StyleInclude)Application.Current.Styles[1]).Source = new Uri("avares://Avalonia.Themes.Default/Accents/BaseDark.xaml");

            EventLogViewModel.AddNewRegistry("Application Theme has been changed to Dark",
                DateTime.Now, this.GetType().Name, "LOW");
        }
    }
}
