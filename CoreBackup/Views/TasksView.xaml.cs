using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CoreBackup.Models.IO;

namespace CoreBackup.Views
{
    public class TasksView : UserControl
    {
        private DataGrid dataGridLeft;
        private DataGrid dataGridRight;
        public TasksView()
        {
            this.InitializeComponent();
            dataGridLeft = this.FindControl<DataGrid>("TasksDataGridLeft");
            dataGridRight = this.FindControl<DataGrid>("TasksDataGridRight");
            dataGridLeft.LoadingRow += new System.EventHandler<DataGridRowEventArgs>(DataGrid_LoadingRows);
            dataGridRight.LoadingRow += new System.EventHandler<DataGridRowEventArgs>(DataGrid_LoadingRows);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void DataGrid_LoadingRows(object sender, DataGridRowEventArgs e)
        {
            var dataObject = e.Row.DataContext as FileInformation;
            if (dataObject != null && dataObject.IsNewer == false)
                e.Row.Background = Brushes.Maroon;
        }
    }
}
