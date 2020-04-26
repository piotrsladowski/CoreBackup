using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Windows.Input;
using ReactiveUI;

namespace CoreBackup.Models.Local
{
    public class LocalPath : ReactiveObject
    {
        public int NumericID { get; set; }
        private string path;
        public string Path
        {
            get => path;
            set => this.RaiseAndSetIfChanged(ref path, value);
        }
        public ReactiveCommand<Unit,Unit> ExplorerCommand { get; set; }
    }
}
