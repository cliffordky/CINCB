using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Configuration
{
    public class DocumentStorageSettings
    {
        public const string ConfigKey = nameof(DocumentStorageSettings);
        public string FileSystemBasePath { get; set; }
        public string HttpBasePath { get; set; }
        public string AssetSlug { get; set; }
        public string UserSlug { get; set; }
    }
}
