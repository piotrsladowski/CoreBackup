using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using CoreBackup.ViewModels;
using CoreBackup.Windows;
using ReactiveUI;

namespace CoreBackup.Views
{
    public class ConfigurationView : UserControl
    {
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        public ProgressBar progressBar;

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
