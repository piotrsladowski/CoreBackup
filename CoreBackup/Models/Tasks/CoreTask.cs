using System;
using System.Collections.Generic;
using System.Text;
using CoreBackup.Models.Config;

namespace CoreBackup.Models.Tasks
{
    static class CoreTask
    {
        //Dictionary<taskName, configuration>
        public static Dictionary<string, ConfigHub> tasksList = new Dictionary<string, ConfigHub>();

        public static void RemoveTaskEntry(string taskName)
        {
            tasksList.Remove(taskName);
        }


        public static void AddTaskEntry(string taskName, ConfigHub configuration)
        {
            tasksList.Add(taskName, configuration);
        }
        
    }
}
