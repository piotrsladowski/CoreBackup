using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CoreBackup.Views
{
    public class EventLogView : UserControl
    {
        public EventLogView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
