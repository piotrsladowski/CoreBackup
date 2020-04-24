using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;

namespace CoreBackup.ViewModels.ConfigurationViewModels
{
    public abstract class ConfViewModelBase : ReactiveObject
    {
        internal bool isVisible = false;
    }
}
