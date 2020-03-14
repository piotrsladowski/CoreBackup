using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CoreBackup.Views
{
    public class SplashView : UserControl
    {
        public SplashView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
