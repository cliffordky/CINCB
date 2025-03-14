using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Configuration
{
    public class CollectorSettings
    {
        public const string ConfigKey = nameof(CollectorSettings);

        public string PickupFolderPath { get; set; }
        public string BackupFolderPath { get; set; }

        public string FailedFolderPath { get; set; }
    }
}