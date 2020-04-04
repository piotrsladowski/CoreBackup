using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBackup.Models.Config
{
    class CoreTask
    {
        //Dictionary<taskName, configuration>
        private static Dictionary<string, Configuration> tasksList = new Dictionary<string, Configuration>();
        public CoreTask()
        {
            tasksList = new Dictionary<string, Configuration>();
        }

        public static void RemoveTaskEntry(string taskName)
        {
            tasksList.Remove(taskName);
        }


        public static void AddTaskEntry(string taskName, Configuration configuration)
        {
            tasksList.Add(taskName, configuration);
        }
    }
}
