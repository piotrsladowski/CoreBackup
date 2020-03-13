using CoreBackup.Views;
using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;


namespace CoreBackup.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        
        ViewModelBase content;
        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        public MainWindowViewModel()
        {
            Content = new ConfigurationViewModel();
            // Tutaj powinno być:
            // Content = new ContainerViewModel();
            // Ale sypie błąd. Nie wiem dlaczego, jest identycznie zrobione 
        }

        public void ShowConfigWindow()
        {
            Content = new ConfigurationViewModel();
            // Binding działa do okienka Configu
           
        }

    }
}
