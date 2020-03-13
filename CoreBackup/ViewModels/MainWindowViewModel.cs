using CoreBackup.Views;
using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;


namespace CoreBackup.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";


        
        ViewModelBase content;
        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        public void ShowConfigWindow()
        {
            Content = new ConfigurationViewModel();
           
        }

    }
}
