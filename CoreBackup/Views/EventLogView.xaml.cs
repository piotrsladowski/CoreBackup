using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CoreBackup.Models.Logging;
using DynamicData;
using DynamicData.Kernel;
using Microsoft.VisualBasic;
using ReactiveUI;

namespace CoreBackup.Views
{
    public class EventLogView : UserControl
    {
        private DataGrid dataGrid;
        public EventLogView()
        {
            this.InitializeComponent();
            dataGrid = this.FindControl<DataGrid>("EventLogsDataGrid");
            this.dataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRows);
        } 

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        void dataGrid_LoadingRows(object sender, DataGridRowEventArgs e)
        {
            var dataObject = e.Row.DataContext as LogRegistry;
            if (dataObject != null && dataObject.Importance == "HIGH")
                e.Row.Background = Brushes.Red;
        }
    }
}
