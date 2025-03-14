using Core.Models.Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Radzen.Blazor;

namespace Web.Components.Pages.AccessManagement
{
    public partial class UserListing : PageBase
    {
        [Inject] private IOptions<DocumentStorageSettings> DocumentStorageSettings { get; set; }
        protected override async Task OnInitializedAsync() 
        {
            users.AddRange((await _access.GetUsersAsync()).ToList());

            foreach (var user in users)
            {
                userList.Add(new UserModel()
                {
                    Id = user.Id,
                    PublicKey = user.PublicKey,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ImageSlug = GetImageUrl(user.ImageSlug) 
                });

            }

            await userGrid.RefreshDataAsync();

        }
        void HandleRowClickEvent(UserModel args)
        {
            NavManager.NavigateTo($"/access-management/user?pk={args.PublicKey}");
        }
        void AddUser_Clicked()
        {
            NavManager.NavigateTo($"/access-management/user");
        }
        private RadzenDataGrid<UserModel> userGrid;
        private List<UserModel> userList = new List<UserModel>();
        private List<Core.Models.Data.User> users = new List<Core.Models.Data.User>();

        private string GetImageUrl(string imageSlug)
        {
            if (String.IsNullOrEmpty(imageSlug))
            {
                return $"{DocumentStorageSettings.Value.HttpBasePath}/{DocumentStorageSettings.Value.UserSlug}/placeholder-image.jpg";
            }
            else
            {
                return $"{DocumentStorageSettings.Value.HttpBasePath}/{DocumentStorageSettings.Value.UserSlug}/{imageSlug}";
            }
        }
    }
}
