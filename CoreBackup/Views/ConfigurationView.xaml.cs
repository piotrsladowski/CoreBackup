using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using CoreBackup.ViewModels;
using CoreBackup.Windows;
using ReactiveUI;

namespace CoreBackup.Views
{
    public class ConfigurationView : UserControl
    {
        public ConfigurationView()
        {
            this.InitializeComponent();
            
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

    }
}
