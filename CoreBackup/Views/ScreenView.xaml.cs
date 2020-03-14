using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CoreBackup.Views
{
    public class ScreenView : UserControl
    {
        public ScreenView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
