using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svc.Jobs
{
    public class JobSettings
    {
        public List<JobDetail> Jobs { get; set; }

        public JobSettings()
        {
            Jobs = new List<JobDetail>();
        }
    }

    public class JobDetail
    {
        public string TypeName { get; set; }

        public string JobKey { get; set; }
        public string? CronSchedule { get; set; }
        public int? IntervalMiliSeconds { get; set; }
        public bool IsActive { get; set; }
    }
}