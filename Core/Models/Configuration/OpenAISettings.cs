using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Configuration
{
   public class OpenAISettings
    {
        public const string ConfigKey = nameof(OpenAISettings);

        public string WorkingFolderPath { get; set; }
        public string OpenAIKey { get; set; }

    }
}
