using Coravel;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Serilog;
//using Svc.Handlers;
using Svc.Jobs;

using System.Configuration;

namespace Svc
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddWindowsService(options =>
            {
                options.ServiceName = "_CIGHSAPOC Background Service";
            });

            builder.Services.Configure<Core.Models.Configuration.CollectorSettings>(builder.Configuration.GetSection(Core.Models.Configuration.CollectorSettings.ConfigKey));
            builder.Services.Configure<Core.Models.Configuration.FhirSettings>(builder.Configuration.GetSection(Core.Models.Configuration.FhirSettings.ConfigKey));
            builder.Services.Configure<Core.Models.Configuration.OpenAISettings>(builder.Configuration.GetSection(Core.Models.Configuration.OpenAISettings.ConfigKey));

            builder.Services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                );

            builder.Services.Configure<JobSettings>(builder.Configuration.GetSection("JobSetting"));
            builder.Services.RegisterBackgroundServices(builder.Configuration.GetSection("JobSetting").Get<JobSettings>() ?? new JobSettings());

            builder.Services.AddInfrastructure(builder.Configuration);

            //builder.Services.AddTransient<AnalyzePatientInvokable>();
            builder.Services.AddQueue();

            var host = builder.Build();

            host.Run();
        }
    }
}