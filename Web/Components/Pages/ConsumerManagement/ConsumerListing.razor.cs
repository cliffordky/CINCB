
using Core.Models.Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Radzen.Blazor;


namespace Web.Components.Pages.ConsumerManagement
{
    public partial class ConsumerListing : PageBase
    {
        [Inject] private IOptions<DocumentStorageSettings> DocumentStorageSettings { get; set; }
        protected override async Task OnInitializedAsync()
        {
            users.AddRange((await _access.GetUsersAsync()).ToList());

            foreach (var user in users)
            {
                userList.Add(new ConsumerModel()
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
        void HandleRowClickEvent(ConsumerModel args)
        {
            NavManager.NavigateTo($"/consumer-management/consumer?pk={args.PublicKey}");
        }
        void AddUser_Clicked()
        {
            NavManager.NavigateTo($"/consumer-management/consumer");
        }
        private RadzenDataGrid<ConsumerModel> userGrid;
        private List<ConsumerModel> userList = new List<ConsumerModel>();
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
