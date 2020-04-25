using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CoreBackup.ViewModels.ConfigurationViewModels;

namespace CoreBackup.Views.ConfigurationViews
{
    public class DirectoryConfView : UserControl
    {
        public DirectoryConfView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
