using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using CoreBackup.Models.Tasks;
using Newtonsoft.Json;

namespace CoreBackup.Models.Config
{
    class Serializer
    {
        readonly Dictionary<string, ConfigHub> tasksList;
        string jsonConfig;

        public Serializer(Dictionary<string, ConfigHub> tasksList)
        {
            this.tasksList = tasksList;
        }

        public void Serialze()
        {
            jsonConfig = JsonConvert.SerializeObject(tasksList, Formatting.Indented);
            using var writer = new StreamWriter("E:\\core" + "\\" + "conf.json");
            writer.Write(jsonConfig);
        }

        public void DeSerialize()
        {

        }
    }
}
