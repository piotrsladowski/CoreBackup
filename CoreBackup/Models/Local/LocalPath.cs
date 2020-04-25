using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Windows.Input;
using ReactiveUI;

namespace CoreBackup.Models.Local
{
    public class LocalPath
    {
        public string Path { get; set; }
        public ReactiveCommand<Unit,Unit> FileExplorerCommand { get; set; }
    }
}
