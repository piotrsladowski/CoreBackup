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
            //dataGridLeft.AttachedToLogicalTree
            dataGridLeft.LoadingRow += new System.EventHandler<DataGridRowEventArgs>(DataGrid_LoadingRows);
            dataGridRight.LoadingRow += new System.EventHandler<DataGridRowEventArgs>(DataGrid_LoadingRows);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void DataGrid_LoadingRows(object sender, DataGridRowEventArgs e)
        {
            FileInformation dataObject = e.Row.DataContext as FileInformation;
            if (dataObject != null && dataObject.FileVersion == FileVersion.Older)
                e.Row.Background = Brushes.Maroon;
            else if (dataObject != null && dataObject.FileVersion == FileVersion.Newer)
                e.Row.Background = Brushes.DarkGreen;
            else if (dataObject != null && dataObject.FileVersion == FileVersion.Equal)
                e.Row.Background = Brushes.DarkGray;
        }
    }
}
