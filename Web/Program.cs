using Coravel;
using Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Radzen;
using Serilog;
using SmartComponents.Inference.OpenAI;
using Web.Components;
using Web.Components.Account;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOptions();

            builder.Host.UseSerilog((hostingContext, configBuilder) => {
                configBuilder.ReadFrom.Configuration(hostingContext.Configuration);
                configBuilder.Enrich.FromLogContext();
            });


            builder.Services.Configure<Core.Models.Configuration.DocumentStorageSettings>(builder.Configuration.GetSection(Core.Models.Configuration.DocumentStorageSettings.ConfigKey));

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            //builder.Services.AddBlazorBootstrap();
            builder.Services.AddRadzenComponents();
            //builder.Services.AddRadzenComponents()..AddInteractiveServerComponents();
            builder.Services.AddInfrastructure(builder.Configuration);
            

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            builder.Services.AddIdentity(builder.Configuration, identityBuilder => {
                identityBuilder.AddSignInManager();
                identityBuilder.AddDefaultTokenProviders();
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

            builder.Services.AddTransient<HtmlRenderer>();

            //builder.Services.AddSmartComponents()
            //    .WithInferenceBackend<OpenAIInferenceBackend>();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddQueue();    //  use Coravel for background tasks
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            app.Run();
        }
    }
}
