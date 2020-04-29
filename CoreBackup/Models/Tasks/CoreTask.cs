﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using CoreBackup.Models.Config;
using System.Runtime.Serialization.Json;
using System.Diagnostics;

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
            tasksList.Add(taskName, configuration);
        }


        public static void saveConfigToJsonFile()
        {
                jsonConfig = JsonConvert.SerializeObject(CoreTask.tasksList, Formatting.Indented);
                using (var writer = new StreamWriter(jsonConfigPath + "\\" + configFilename))
                {
                    writer.Write(jsonConfig);
                }
            
        }

        public static void readConfigFromJsonFile()
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
    }
}