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

namespace CoreBackup.Views.ConfigurationViews
{
    public class FTPConfView : UserControl
    {
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        public ProgressBar progressBar;
        public FTPConfView()
        {
            this.InitializeComponent();
            progressBar = this.FindControl<ProgressBar>("progressBar");
            // Acces to Xaml element by x:Name Property
            backgroundWorker.RunWorkerAsync();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.ProgressChanged += ProgressChanged;
            backgroundWorker.DoWork += ShowProgress;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }
        private void ShowProgress(object sender, DoWorkEventArgs e)
        {

            for (int i = 0; i <= 100; i++)
            {
                // Simulate long running work
                Thread.Sleep(100);
                backgroundWorker.ReportProgress(i);

            }
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // This is called on the UI thread when ReportProgress method is called
            if (progressBar.IsVisible == true)
                progressBar.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // This is called on the UI thread when the DoWork method completes
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
