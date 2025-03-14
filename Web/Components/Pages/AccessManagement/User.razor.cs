using Core.Models.Configuration;
using Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using PasswordGenerator;
using Radzen;
using System.Text;

namespace Web.Components.Pages.AccessManagement
{
    public partial class User : PageBase
    {
        [Inject] private IOptions<DocumentStorageSettings> DocumentStorageSettings { get; set; }
        [Inject] private Core.IEmailService EmailService { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "pk")]
        public Guid PublicKey { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "id")]
        public int UserId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            roles.Clear();
            roles = RoleManager.Roles.ToList();

            Organizations.Clear();
            Organizations.AddRange((await _meta.GetAllOrganizationsAsync()).ToList());
        }

        protected override async Task OnParametersSetAsync()
        {
            if (this.PublicKey != Guid.Empty)
            {
                var getResult = await _access.GetUserByPublicKeyAsync(PublicKey);
                if (getResult.IsSuccess)
                {
                    var user = getResult.Value;

                    Input.Id = user.Id;
                    Input.PublicKey = user.PublicKey;
                    Input.FirstName = user.FirstName;
                    Input.LastName = user.LastName;
                    Input.EmailAddress = user.Email;
                    Input.PhoneNumber = user.PhoneNumber;
                    Input.ImageSlug = user.ImageSlug;

                    if (!System.String.IsNullOrEmpty(Input.ImageSlug))
                    {
                        ImageUrl = $"{DocumentStorageSettings.Value.HttpBasePath}/{DocumentStorageSettings.Value.UserSlug}/{Input.ImageSlug}";
                    }
                    else
                    {
                        ImageUrl = $"{DocumentStorageSettings.Value.HttpBasePath}/{DocumentStorageSettings.Value.UserSlug}/placeholder-image.jpg";
                    }

                    var profile = await UserManager.FindByIdAsync(Input.Id.ToString());

                    SelectedRoles.Clear();
                    SelectedRoles = (await UserManager.GetRolesAsync(profile)).ToList();

                    SelectedOrganizationIds = (await _access.GetUserOrganizationsByUserIdAsync(Input.Id)).Select(x => x.Id).ToList();
                }
            }
        }

        private void OnInvalidSubmit(FormInvalidSubmitEventArgs args)
        {
        }

        private async Task Submit(UserModel arg)
        {
            try
            {
                if (Input.Id == 0)
                {
                    var existingUser = await UserManager.FindByNameAsync(Input.EmailAddress);
                    if (existingUser != null)
                    {
                        IsAlertVisible = true;
                        AlertBody = $"The email address {Input.EmailAddress} already exists, please use a different email address.";
                        StateHasChanged();

                        return;
                    }

                    var result = await UserManager.CreateAsync(new ApplicationUser()
                    {
                        UserName = Input.EmailAddress,
                        Email = Input.EmailAddress,
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        PhoneNumber = Input.PhoneNumber
                    }, new Password().IncludeLowercase().IncludeUppercase().IncludeNumeric().IncludeSpecial().Next());

                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByEmailAsync(Input.EmailAddress);
                        await UpdateUserRolesAsync(user.Id, SelectedRoles);
                        await UpdateOrganizationLinks(user.Id, SelectedOrganizationIds);

                        var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = NavigationManager.GetUriWithQueryParameters(NavigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri, new Dictionary<string, object?> { ["userId"] = user.Id, ["code"] = code });

                        await EmailService.SendConfirmationEmailAsync(Input.EmailAddress, callbackUrl);

                        NotificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Summary = "Success",
                            Detail = $"User {Input.FirstName} has been saved.",
                            Duration = 4000
                        });
                    }
                    else
                    {
                        NotificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Summary = "Error",
                            Detail = $"Error saving user {Input.FirstName}, please try again.",
                            Duration = 4000
                        });
                    }
                }

                var getResult = await _access.GetUserByIdAsync(Input.Id);
                if (getResult.IsSuccess)
                {
                    getResult.Value.FirstName = Input.FirstName;
                    getResult.Value.LastName = Input.LastName;
                    getResult.Value.PhoneNumber = Input.PhoneNumber;
                    getResult.Value.ImageSlug = Input.ImageSlug;

                    var updateResult = await _access.UpdateUserAsync(getResult.Value);

                    await UpdateUserRolesAsync(getResult.Value.Id, SelectedRoles);

                    if (updateResult.IsSuccess)
                    {
                        NotificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Summary = "Success",
                            Detail = $"Asset {getResult.Value.FirstName} has been saved.",
                            Duration = 4000
                        });
                    }
                    else
                    {
                        NotificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Summary = "Error",
                            Detail = $"Error saving asset {getResult.Value.FirstName}, please try again.",
                            Duration = 4000
                        });
                    }
                }

                await UpdateOrganizationLinks(Input.Id, SelectedOrganizationIds);
            }
            catch (Exception Ex)
            {
            }
        }

        private async Task UpdateUserRolesAsync(int UserId, List<string> selectedRoles)
        {
            var profile = await UserManager.FindByIdAsync(UserId.ToString());
            var currentRoles = await UserManager.GetRolesAsync(profile);
            var roleResult = await UserManager.RemoveFromRolesAsync(profile, currentRoles);
            if (roleResult.Succeeded)
            {
                var addResult = await UserManager.AddToRolesAsync(profile, selectedRoles);
            }
        }

        private async Task UpdateOrganizationLinks(int UserId, List<int> selectedOrganizationIds)
        {
            await _access.UpsertUserOrganizationsAsync(UserId, selectedOrganizationIds);
        }

        private async Task OnSendPasswordReset_Click()
        {
            var user = await UserManager.FindByEmailAsync(Input.EmailAddress);
            if (user is null || !(await UserManager.IsEmailConfirmedAsync(user)))
            {
                NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = $"Error sending password reset email.  The email address {Input.EmailAddress} has not yet been confirmed.  Please resend the confirmation before sending a password reset.",
                    Duration = 4000
                });

                return;
            }

            var code = await UserManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = NavigationManager.GetUriWithQueryParameters(NavigationManager.ToAbsoluteUri("Account/ResetPassword").AbsoluteUri, new Dictionary<string, object?> { ["code"] = code });

            await EmailService.SendPasswordResetEmailAsync(Input.EmailAddress, callbackUrl);
        }

        private void Cancel()
        {
            //
        }

        private async Task OnImageUpload(UploadChangeEventArgs args)
        {
            foreach (var file in args.Files)
            {
                try
                {
                    var directoryPath = Path.Combine(DocumentStorageSettings.Value.FileSystemBasePath, DocumentStorageSettings.Value.UserSlug, Input.PublicKey.ToString());
                    if (!System.IO.Directory.Exists(directoryPath))
                    {
                        System.IO.Directory.CreateDirectory(directoryPath);
                    }

                    long maxFileSize = 10 * 1024 * 1024;
                    await using FileStream fs = new(Path.Combine(directoryPath, file.Name), FileMode.Create);
                    await file.OpenReadStream(maxFileSize).CopyToAsync(fs);

                    Input.ImageSlug = $"{Input.PublicKey}/{file.Name}";
                    ImageUrl = $"{DocumentStorageSettings.Value.HttpBasePath}/{DocumentStorageSettings.Value.UserSlug}/{Input.ImageSlug}";
                }
                catch (Exception ex)
                {
                }
            }
        }

        private UserModel Input = new UserModel();
        private string ImageUrl;
        private List<ApplicationRole> roles = new List<ApplicationRole>();
        private List<string> SelectedRoles = new List<string>();
        private List<Core.Models.Data.Organization> Organizations = new List<Core.Models.Data.Organization>();
        private List<int> SelectedOrganizationIds = new List<int>();
    }
}