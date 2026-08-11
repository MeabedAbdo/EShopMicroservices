using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Logging.Models
{
    public class LoggingOptions
    {
        public string ApplicationName { get; set; } = string.Empty;

        public bool EnableConsole { get; set; } = true;

        public bool EnableFile { get; set; } = true;

        public string FilePath { get; set; }
            = "Logs/log-.txt";

        public bool EnableSqlServer { get; set; }

        public string? SqlConnectionString { get; set; }

        public string TableName { get; set; }
            = "Logs";
        public bool EnableSeq { get; set; } = false;
        public string? SeqURL { get; set; }
    }
}
