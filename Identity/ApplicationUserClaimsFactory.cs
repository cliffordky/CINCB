using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Identity
{
    public class ApplicationUserClaimsFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>, IUserClaimsPrincipalFactory<ApplicationUser>
    {
        public ApplicationUserClaimsFactory(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        { }

        /// <summary>
        /// Creates a <see cref="ClaimsPrincipal"/> from an user asynchronously.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ClaimsPrincipal"/> from.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous creation operation, containing the created <see cref="ClaimsPrincipal"/>.</returns>
        public override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            return base.CreateAsync(user);
        }

        /// <summary>
        /// Generate the claims for a user.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ClaimsIdentity"/> from.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous creation operation, containing the created <see cref="ClaimsIdentity"/>.</returns>
        protected async override Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            ClaimsIdentity claimsIdentity = await base.GenerateClaimsAsync(user);

            if (!string.IsNullOrEmpty(user.FirstName))
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.FirstName));

            if (!string.IsNullOrEmpty(user.LastName))
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));

            if (!string.IsNullOrEmpty(user.Email))
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            return claimsIdentity;
        }
    }
}
