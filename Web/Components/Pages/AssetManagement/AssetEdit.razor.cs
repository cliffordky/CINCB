using BlazorBootstrap;
using Core.Models.Configuration;
using Core.Models.Data;
using EllipticCurve.Utils;

using Infrastructure.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OfficeOpenXml.Style;
using Radzen;
using Radzen.Blazor;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;
using System.Xml.Serialization;
using static Web.Components.Pages.AssetManagement.AssetEdit;

namespace Web.Components.Pages.AssetManagement
{
    public partial class AssetEdit : PageBase
    {
        [Inject] private Core.IAccess _access { get; set; }
        [Inject] private IOptions<DocumentStorageSettings> DocumentStorageSettings { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "pk")]
        public Guid PublicKey { get; set; }

        protected string ImageUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            users.AddRange((await _access.GetUsersAsync()).ToList());
            assetTypes.AddRange((await _meta.GetAllAssetTypesAsync()).ToList());
            attributes.AddRange((await _meta.GetAllAttributeItemsAsync()).ToList());
            organizations.AddRange((await _meta.GetAllOrganizationsAsync()).Select(x => new Organization()
            {
                Id = x.Id,
                Name = x.Name
            }));

            assets.AddRange((await _asset.GetAllAssetsAsync()).ToList());

            parentAssets = assets.Select(a => new ParentAsset()
            {
                Id = a.Id,
                Name = a.Name,
                RegisterNumber = a.RegisterNumber
            });
        }

        protected override async Task OnParametersSetAsync()
        {
            if (this.PublicKey != Guid.Empty)
            {
                var result = await _asset.GetAssetDetailsByPublicKeyAsync(this.PublicKey);
                if (result.IsSuccess)
                {
                    asset.Id = result.Value.Id;
                    asset.PublicKey = result.Value.PublicKey;
                    asset.Name = result.Value.Name;
                    asset.RegisterNumber = result.Value.RegisterNumber;
                    asset.Description = result.Value.Description;
                    asset.Notes = result.Value.Notes;
                    asset.AssetTypeId = result.Value.AssetTypeId;
                    asset.RegisterNumber = result.Value.RegisterNumber;
                    asset.PurchaseDate = result.Value.PurchaseDate;
                    asset.PurchaseValue = result.Value.PurchaseValue;
                    asset.CurrentValue = result.Value.CurrentValue;
                    asset.ImageSlug = result.Value.ImageSlug;


                    var assetUsers = await _asset.GetUsersForAssetAsync(asset.Id);
                    asset.AssignedUsers = assetUsers;


                    if ((result.Value.ParentAssetId.HasValue) && (assets.Count(x => x.Id == result.Value.ParentAssetId.Value) > 0))
                    {
                        asset.SelectedParentAsset = assets.Where(o => o.Id == result.Value.ParentAssetId).Select(x => new ParentAsset()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            RegisterNumber = x.RegisterNumber
                        }).FirstOrDefault();
                    }

                    if (organizations.Count(x => x.Id == result.Value.OrganizationId) > 0)
                    {
                        asset.SelectedOrganization = organizations.Where(o => o.Id == result.Value.OrganizationId).Select(x => new Organization()
                        {
                            Id = x.Id,
                            Name = x.Name
                        }).FirstOrDefault();
                    }



                    if (!System.String.IsNullOrEmpty(asset.ImageSlug))
                    {
                        ImageUrl = $"{DocumentStorageSettings.Value.HttpBasePath}/{DocumentStorageSettings.Value.AssetSlug}/{asset.ImageSlug}";
                    }
                    else
                    {
                        ImageUrl = $"{DocumentStorageSettings.Value.HttpBasePath}/{DocumentStorageSettings.Value.AssetSlug}/placeholder-image.jpg";
                    }
                }
                else
                {
                    //_toastService.Notify(new(ToastType.Danger, $"There was a problem loading the Asset, please try again."));
                }
            }
            else
            {
                //navItems.Add(new BreadcrumbItem { Text = "New Asset" });
            }
        }



        public class Asset
        {
            public int AssetTypeId { get; set; }

            public string RegisterNumber { get; set; }

            public DateOnly? PurchaseDate { get; set; }
            public string CardHolder { get; set; }
            public string Name { get; set; }
            public string ImageSlug { get; set; }
            public decimal? PurchaseValue { get; set; }
            public decimal? CurrentValue { get; set; }
            public DateTime OrderDate { get; set; }
            public DateTime ShipDate { get; set; }
            public int Id { get; internal set; }
            public Guid PublicKey { get; internal set; }

            //public List<int> UserIds { get; set; } = new List<int>();

            public ParentAsset SelectedParentAsset { get; set; }
            public Organization SelectedOrganization { get; set; }
            public string Notes { get; internal set; }
            public string Description { get; internal set; }
            public List<Core.Models.Data.AssetUser> AssignedUsers { get; set; } = new List<Core.Models.Data.AssetUser>();
        }

        public class ParentAsset
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string RegisterNumber { get; set; }

            public override bool Equals(object o)
            {
                var other = o as ParentAsset;

                return other?.Id == Id;
            }

            public override string ToString()
            {
                return $"{Name} ({RegisterNumber})";
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        public class Organization
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public override bool Equals(object o)
            {
                var other = o as Organization;

                return other?.Id == Id;
            }

            public override string ToString()
            {
                return $"{Name}";
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        private Asset asset = new Asset()
        {

        };

        private List<Core.Models.Data.User> users = new List<Core.Models.Data.User>();
        private List<Core.Models.Data.AssetUser> assetUsers = new List<Core.Models.Data.AssetUser>();
        private List<Core.Models.Data.AssetType> assetTypes = new List<Core.Models.Data.AssetType>();
        private List<Core.Models.Data.Attribute> attributes = new List<Core.Models.Data.Attribute>();
        private List<Organization> organizations = new List<Organization>();
        private List<Core.Models.Data.Asset> assets = new List<Core.Models.Data.Asset>();
        private IEnumerable<ParentAsset> parentAssets;

        private void OnNotes_Change(string value)
        {
            asset.Notes = value;
        }

        private void OnInvalidSubmit(FormInvalidSubmitEventArgs args)
        {
        }

        private async Task Submit(Asset arg)
        {
            var customerResult = await _asset.GetAssetDetailsByIdAsync(asset.Id);
            if (customerResult.IsSuccess)
            {
                customerResult.Value.Name = asset.Name;
                customerResult.Value.Description = asset.Description;
                customerResult.Value.Notes = asset.Notes;
                customerResult.Value.PurchaseValue = asset.PurchaseValue;
                customerResult.Value.CurrentValue = asset.CurrentValue;
                customerResult.Value.AssetTypeId = asset.AssetTypeId;
                customerResult.Value.OrganizationId = asset.SelectedOrganization.Id;
                customerResult.Value.ParentAssetId = asset.SelectedParentAsset?.Id;
                customerResult.Value.RegisterNumber = asset.RegisterNumber;
                customerResult.Value.ImageSlug = asset.ImageSlug;

                var result = await _asset.UpdateAssetAsync(customerResult.Value);
                if (result.IsSuccess)
                {
                    NotificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Summary = "Success",
                        Detail = $"Asset {customerResult.Value.Name} has been saved.",
                        Duration = 4000
                    });
                    //_toastService.Notify(new(ToastType.Success, $"Asset {customerResult.Value.Name} has been saved."));
                    //NavManager.NavigateTo("/asset-list");
                }
                else
                {
                    NotificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Error",
                        Detail = $"Error saving asset {customerResult.Value.Name}, please try again.",
                        Duration = 4000
                    });
                }
            }
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
                    var directoryPath = Path.Combine(DocumentStorageSettings.Value.FileSystemBasePath, DocumentStorageSettings.Value.AssetSlug, asset.PublicKey.ToString());
                    if (!System.IO.Directory.Exists(directoryPath))
                    {
                        System.IO.Directory.CreateDirectory(directoryPath);
                    }

                    long maxFileSize = 10 * 1024 * 1024;
                    await using FileStream fs = new(Path.Combine(directoryPath, file.Name), FileMode.Create);
                    await file.OpenReadStream(maxFileSize).CopyToAsync(fs);

                    asset.ImageSlug = $"{asset.PublicKey}/{file.Name}";
                    ImageUrl = $"{DocumentStorageSettings.Value.HttpBasePath}/{DocumentStorageSettings.Value.AssetSlug}/{asset.ImageSlug}";
                }
                catch (Exception ex)
                {
                }
            }
        }


        public async void AddUserAccessAsync(int userId)
        {
            await _asset.AddAssetUserAsync(asset.Id, userId);
        }

        public async void RemoveUserAccessAsync(int userId)
        {
            await _asset.DeleteAssetUserAsync(asset.Id, userId);
        }
    }
}