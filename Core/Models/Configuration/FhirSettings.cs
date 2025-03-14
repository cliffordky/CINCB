using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Configuration
{
    public class FhirSettings
    {
        public const string ConfigKey = nameof(FhirSettings);

        public string Host { get; set; }

    }
}
