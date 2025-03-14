using Core;
using Core.Models.Configuration;
using Dapper.FastCrud;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //Default dialect for FastCrud 
            OrmConfiguration.DefaultDialect = SqlDialect.MsSql;

            //Set up the Sql Connection factory
            services.AddSingleton<ISqlConnectionFactory, SqlDbConnectionFactory<SqlConnection>>();

            //set up the Application Db Context
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContextFactory<ApplicationDbContext>(options => {
                options.UseSqlServer(connectionString);
            });

            //Set up the repository implementations
            services.AddSingleton<Core.Repository.IAssetUser, Infrastructure.Repository.AssetUser>();
            services.AddSingleton<Core.Repository.IAsset, Infrastructure.Repository.Asset>();
            services.AddSingleton<Core.Repository.IUser, Infrastructure.Repository.User>();
			services.AddSingleton<Core.Repository.IAttribute, Infrastructure.Repository.Attribute>();
			services.AddSingleton<Core.Repository.IAssetAttribute, Infrastructure.Repository.AssetAttribute>();
            services.AddSingleton<Core.Repository.IAssetDocument, Infrastructure.Repository.AssetDocument>();
            services.AddSingleton<Core.Repository.ISource, Infrastructure.Repository.Source>();
            services.AddSingleton<Core.Repository.IQueue, Infrastructure.Repository.Queue>();
            services.AddSingleton<Core.Repository.IAssetType, Infrastructure.Repository.AssetType>();
            services.AddSingleton<Core.Repository.IOrganization, Infrastructure.Repository.Organization>();
            services.AddSingleton<Core.Repository.IProvider, Infrastructure.Repository.Provider>();
            services.AddSingleton<Core.Repository.IAspNetUserOrganization, Infrastructure.Repository.AspNetUserOrganization>();
            services.AddSingleton<Core.Repository.IMessageTemplate, Infrastructure.Repository.MessageTemplate>();

            //Set up the business layer implementations
            services.AddSingleton<Core.IAccess, Infrastructure.Access>();
            services.AddSingleton<Core.IAsset, Infrastructure.Asset>();
			services.AddSingleton<Core.IMeta, Infrastructure.Meta>();
            services.AddSingleton<Core.IQueue, Infrastructure.Queue>();
            services.AddSingleton<Core.Integrations.IOpenAI, Infrastructure.Integrations.OpenAI>();

            //Set up the Email Service
            var key = configuration["EmailSettings:ApiKey"];
            services.AddFluentEmail(configuration["EmailSettings:SenderEmail"])
                    .AddRazorRenderer()
                    .AddSendGridSender(configuration["EmailSettings:ApiKey"]);

            services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.ConfigKey));
            services.AddSingleton<IEmailService, EmailService>();

            //services.AddSingleton<IEncryptionService, EncryptionService>();


        }
    }
}