using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBackup.Models.Config
{
    public class ConfigHub
    {
        public List<Configuration> LeftSources { get; set; }
        public List<Configuration> RightSources { get; set; }
        public bool IsActive { get; set; }
        public ConfigHub()
        {
            LeftSources = new List<Configuration>();
            RightSources = new List<Configuration>();
            IsActive = true;
        }

        internal void AddLeftSources(Configuration leftSources)
        {
            LeftSources.Clear();
            LeftSources.Add(leftSources);
        }

        internal void AddRightSources(Configuration rightSources)
        {
            RightSources.Clear();
            RightSources.Add(rightSources);
        }
    }
}
