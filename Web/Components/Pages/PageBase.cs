using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Web.Components.Pages
{
    public class PageBase : ComponentBase
    {
        [Inject] private AuthenticationStateProvider _authenticationStateProvider { get; set; }
        [Inject] private Core.IAccess _access { get; set; }

        protected async Task<int> GetUserId()
        {
            if (_authenticationStateProvider is not null)
            {
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState?.User;

                if (user is not null)
                {
                    if (int.TryParse(user.FindFirstValue(ClaimTypes.Sid), out int _userId))
                    {
                        return _userId;
                    }
                }
            }
            return 0;
        }

        protected async Task<string> GetUserFullNameAsync(int Id)
        {
            var getResult = await _access.GetUserByIdAsync(Id);
            if (getResult.IsSuccess)
            {
                return $"{getResult.Value.FirstName} {getResult.Value.LastName}";
            }

            return "unknown";
        }

        protected bool IsAlertVisible { get; set; } = false;
        protected string AlertBody { get; set; } = "";
    }
}