using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CoreBackup.Views
{
    public class TestScreenView : UserControl
    {
        public TestScreenView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
