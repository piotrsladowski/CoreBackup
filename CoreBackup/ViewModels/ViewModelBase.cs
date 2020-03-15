using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace CoreBackup.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        internal bool isVisible = false;
    }
}
