using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBackup.Models.Config
{
    class ConfigHub
    {
        List<Configuration> leftSources;
        List<Configuration> rightSources;
        bool isActive;

        public ConfigHub()
        {
            leftSources = new List<Configuration>();
            rightSources = new List<Configuration>();
        }
    }
}
