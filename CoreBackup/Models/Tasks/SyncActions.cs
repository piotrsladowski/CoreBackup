using CoreBackup.Models.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using CoreBackup.Models.IO;
using DynamicData.Binding;
using System.Linq;

namespace CoreBackup.Models.Tasks
{
    static class SyncActions
    {
        private static List<FileInformation> LeftSource;

        public static void GetFilesList()
        {
            /*
            foreach (KeyValuePair<string, ConfigHub> entry in CoreTask.tasksList)
            {
                Debug.WriteLine("E1");
                ConfigHub ch = entry.Value;
                if (ch.isActive)
                {
                    Debug.WriteLine("E2");
                    foreach (Configuration conf in ch.LeftSources)
                    {
                        var li = conf.GetFiles();
                        LeftSource = LeftSource.Concat(li).ToList();
                        Debug.WriteLine(conf.dataSource);
                        Debug.WriteLine("E3");

                    }
                }
            }*/
        }

        public static void SyncMirror()
        {

        }

        public static void SyncToLocal()
        {

        }

        public static void SyncToLocalOverride()
        {

        }
        public static void SyncToRemote()
        {

        }
        public static void SyncToRemoteOverride()
        {

        }
    }

}
