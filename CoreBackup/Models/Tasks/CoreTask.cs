using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using CoreBackup.Models.Config;
using System.Runtime.Serialization.Json;
using System.Diagnostics;
using CoreBackup.ViewModels;

namespace CoreBackup.Models.Tasks
{
    static class CoreTask
    {
        //Dictionary<taskName, configuration>
        public static Dictionary<string, ConfigHub> tasksList = new Dictionary<string, ConfigHub>();
        private static string jsonConfig;


        public static string jsonConfigPath;
        public static string configFilename;

        public static List<FTPConfig> ftpConf;
        public static DirectoryConfig directoryConf;


        public static void RemoveTaskEntry(string taskName)
        {
            tasksList.Remove(taskName);
        }


        public static void AddTaskEntry(string taskName, ConfigHub configuration)
        {
            try
            {
                tasksList.Add(taskName, configuration);
                EventLogViewModel.AddNewRegistry("Custom Configuration " +  taskName + " has been Saved",
                    DateTime.Now, "Config", "MEDIUM");
            }
            catch (ArgumentException)
            {
                EventLogViewModel.AddNewRegistry("Config " + taskName + " already exists",
                    DateTime.Now, "Config", "HIGH");
            }
            catch(Exception)
            {
                EventLogViewModel.AddNewRegistry( taskName + " config can not be added ",
                    DateTime.Now, "Config", "HIGH");
            }
        }


        public static void SaveConfigToJsonFile()
        {
            jsonConfig = JsonConvert.SerializeObject(CoreTask.tasksList, Formatting.Indented);
            using var writer = new StreamWriter(jsonConfigPath + "\\" + configFilename);
            writer.Write(jsonConfig);

        }

        public static void ReadConfigFromJsonFile()
        {
            string jsonFromFile;
            using (var reader = new StreamReader(jsonConfigPath + "/" + configFilename))
            {
                jsonFromFile = reader.ReadToEnd();
            }
            tasksList = JsonConvert.DeserializeObject<Dictionary<string, ConfigHub>>(jsonFromFile);
            Debug.WriteLine("test");
        }

        public static Dictionary<string, ConfigHub> Clone<T>(this T obj)
        {
            return (Dictionary<string, ConfigHub>)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(tasksList));
        }

        public static bool UpdateTasksList(string oldName, string newName)
        {
            try
            {
                ConfigHub confValue = tasksList[oldName];
                tasksList.Remove(oldName);
                AddTaskEntry(newName, confValue);
                EventLogViewModel.AddNewRegistry("Updated " + oldName + " name",
                DateTime.Now, typeof(CoreTask).Name, "MEDIUM");
                return true;
            }
            catch (Exception e)
            {
                EventLogViewModel.AddNewRegistry("Error during updating " + oldName + " name",
                DateTime.Now, typeof(CoreTask).Name, "ERROR");
                return false;
            }
        }

        public static bool UpdateTasksList(string name, bool status)
        {
            try
            {
                tasksList[name].IsActive = status;
                EventLogViewModel.AddNewRegistry("Updated " + name + " activity status",
                DateTime.Now, typeof(CoreTask).Name, "MEDIUM");
                return true;
            }
            catch (Exception e)
            {
                EventLogViewModel.AddNewRegistry("Error during updating " + name + " activity status",
                DateTime.Now, typeof(CoreTask).Name, "ERROR");
                return false;
            }

        }
    }
}
