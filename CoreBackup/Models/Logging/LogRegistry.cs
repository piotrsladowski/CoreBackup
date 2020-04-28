using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBackup.Models.Logging
{
    public class LogRegistry 
    {
        public int Id { get; set; }
        public String Content { get; set; }
        public DateTime CreationDate { get; set; }
        public String Source { get; set; }
        public String Importance { get; set; }
        
        public enum Priority
        {
            Low,
            Medium,
            High
        }
    }
}
