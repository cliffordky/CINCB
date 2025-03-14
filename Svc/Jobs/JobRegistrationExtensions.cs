using Coravel;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svc.Jobs
{
    public static class JobRegistrationExtensions
    {
        public static void RegisterBackgroundServices(this IServiceCollection services, JobSettings jobSettings)
        {
            jobSettings.Jobs.Where(s => s.IsActive).ToList().ForEach(setting =>
            {
                services.AddQuartz(options =>
                {
                    Type? _f = Type.GetType(setting.TypeName);
                    if (_f != null)
                    {
                        var jobKey = JobKey.Create(setting.JobKey);

                        if (!string.IsNullOrEmpty(setting.CronSchedule))
                        {
                            options
                                .AddJob(_f, jobKey)
                                .AddTrigger(trigger => trigger.ForJob(jobKey)
                                    .WithCronSchedule(setting.CronSchedule)
                                    .StartNow());
                        }
                        else if (setting.IntervalMiliSeconds.HasValue)
                        {
                            options
                                .AddJob(_f, jobKey)
                                .AddTrigger(trigger => trigger.ForJob(jobKey)
                                    .WithSimpleSchedule(s => s.WithInterval(TimeSpan.FromMilliseconds(setting.IntervalMiliSeconds.Value))
                                    .RepeatForever())
                                    .StartNow());
                        }
                    }
                });
            });
            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
                options.AwaitApplicationStarted = true;
            });
        }
    }
}