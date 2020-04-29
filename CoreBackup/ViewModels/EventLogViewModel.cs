using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CoreBackup.Models.Local;
using CoreBackup.Models.Logging;
using ReactiveUI;
using Serilog;

namespace CoreBackup.ViewModels
{
    class EventLogViewModel : ViewModelBase
    {
        public static ObservableCollection<LogRegistry> LogsData { get; set; }
        private static int id = 0;
       
        public EventLogViewModel()
        {
            LogsData = new ObservableCollection<LogRegistry>();
        }

        public static void AddNewRegistry(string content, DateTime creationDate, string source, string priority)
        {
            id += 1;
            LogsData.Add(new LogRegistry(){Id = id, Content = content, CreationDate = creationDate,
                Source  = source, Importance = priority});
        }
    }
}
