using BlazorBootstrap;
using Core.Models.Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Radzen;
using Radzen.Blazor;

namespace Web.Components.Pages.AssetManagement
{
    public partial class AssetListing
    {
        [Inject] private IOptions<DocumentStorageSettings> DocumentStorageSettings { get; set; }

        List<ListModel> listItems = new()!;
        RadzenDataGrid<ListModel> listGrid;

        List<Core.Models.Data.Asset> assets = new List<Core.Models.Data.Asset>();

        protected override async Task OnInitializedAsync()
        {
            try
            {


                try
                {
                    assets.Clear();  //  first off we need to remove any existing items
                    assets = await _asset.GetAllAssetsAsync();

                    foreach (var item in assets.Where(x => x.ParentAssetId == null))
                    {
                        listItems.Add(new ListModel()
                        {
                            Id = item.Id,
                            ParentAssetId = item.ParentAssetId,
                            PublicKey = item.PublicKey,
                            Name = item.Name,
                            RegisterNumber = item.RegisterNumber,
                            Description = item.Description,
                            CreatedDate = item.CreatedDate,
                            ModifiedDate = item.ModifiedDate,
                            PurchaseDate = item.PurchaseDate,
                            ImageUrl = GetImageUrl(item.ImageSlug)
                        });
                    }

                    await listGrid.RefreshDataAsync();
                }
                catch
                {
                }

            }
            catch (Exception Ex)
            {
            }
        }

        private string GetImageUrl(string imageSlug)
        {
            if (String.IsNullOrEmpty(imageSlug))
            {
                return $"{DocumentStorageSettings.Value.HttpBasePath}/{DocumentStorageSettings.Value.AssetSlug}/placeholder-image.jpg";
            }
            else
            {
                return $"{DocumentStorageSettings.Value.HttpBasePath}/{DocumentStorageSettings.Value.AssetSlug}/{imageSlug}";
            }
        }

        void AddAsset_Clicked()
        {
            NavManager.NavigateTo($"/asset-management/asset");
        }

        void HandleRowClickEvent(ListModel args)
        {
            NavManager.NavigateTo($"/asset-management/asset?pk={args.PublicKey}");
        }

        void RowRender(RowRenderEventArgs<ListModel> args)
        {
            try
            {
                //if (args.Data.ParentAssetId == null) return;
                args.Expandable = assets.Count(e => e.ParentAssetId ==  args.Data.Id) > 0;
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        void LoadChildData(DataGridLoadChildDataEventArgs<ListModel> args)
        {
            try
            {
                var filteredAssets = assets.Where(e => e.ParentAssetId == args.Item.Id).ToList();
                args.Data = filteredAssets.Select(x => new ListModel()
                {
                    Id = x.Id,
                    ParentAssetId = x.ParentAssetId,
                    PublicKey = x.PublicKey,
                    Name = x.Name,
                    RegisterNumber = x.RegisterNumber,
                    Description = x.Description,
                    CreatedDate = x.CreatedDate,
                    ModifiedDate = x.ModifiedDate,
                    PurchaseDate = x.PurchaseDate,
                    ImageUrl = GetImageUrl(x.ImageSlug)
                });
            }
            catch (Exception Ex)
            {

                throw;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                base.OnAfterRender(firstRender);

                if (firstRender)
                {
                    //await listGrid.ExpandRow(listItems.FirstOrDefault());
                }
            }
            catch (Exception Ex)
            {

                throw;
            }
        }

        void OnRowClick(ListModel args)
        {
            NavManager.NavigateTo($"/asset?pk={args.PublicKey}");
        }

        private sealed class ListModel
        {
            public int Id { get; set; }
            public int? ParentAssetId { get; set; }

            public Guid PublicKey { get; set; }

            public string RegisterNumber { get; set; } = null!;
            public string Name { get; set; } = null!;

            public string Description { get; set; } = null!;


            public DateTime CreatedDate { get; set; }
            public DateTime ModifiedDate { get; set; }
            public DateOnly? PurchaseDate { get; set; }
            public string ImageUrl { get;  set; }
        }
    }
}