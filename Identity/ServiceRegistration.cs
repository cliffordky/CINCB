using Identity;
using Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddIdentity(this IServiceCollection services, IConfiguration configuration, Action<IdentityBuilder> configure)
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsFactory>();

            var identityBuilder = services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;

                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.Sid;
                options.ClaimsIdentity.UserNameClaimType = ClaimTypes.NameIdentifier;
            })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddClaimsPrincipalFactory<ApplicationUserClaimsFactory>();

            services.AddTransient<IEmailSender<ApplicationUser>, IdentityEmailSender>();

            configure?.Invoke(identityBuilder);
        }
    }
}