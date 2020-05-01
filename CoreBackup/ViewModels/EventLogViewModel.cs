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
            //AddRegistry(rg);
            AddNewRegistry("OKOKOKO", "OKOKOKO");
           

        }

        public void AddRegistry()
        {
            //LogsData.Add();
        }

        private static void AddNewRegistry(string content, string source)
        {
            id += 1;
            LogsData.Add(new LogRegistry(){Id = id, Content = content});
        }
    }
}
