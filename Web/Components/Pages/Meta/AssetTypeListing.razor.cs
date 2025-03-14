using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static Dapper.SqlMapper;

namespace Web.Components.Pages.Meta
{
    public partial class AssetTypeListing
    {
        private RadzenDataGrid<AssetTypeModel> listGrid;
        private List<AssetTypeModel> listItems = new List<AssetTypeModel>();


        [Parameter]
        [SupplyParameterFromQuery(Name = "assetTypeId")]
        public int AssetTypeId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var _items = await _meta.GetAllAssetTypesAsync();
            foreach (var item in _items)
            {
                listItems.Add(new AssetTypeModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description
                });
            }

            await listGrid.RefreshDataAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (AssetTypeId > 0)
            {
                var selectedItem = listItems.FirstOrDefault(x => x.Id == AssetTypeId);
                if (selectedItem != null)
                {
                    listGrid.SelectRow(selectedItem, true);
                }
            }
        }


            private async Task HandleRowClickEvent(AssetTypeModel args)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Id", args.Id);
            parameters.Add("OnSaveClickCallback", EventCallback.Factory.Create<AssetTypeModel>(this, OnEditComplete));

            await DialogService.OpenSideAsync<AssetTypeEdit>($"Edit {args.Name}",
                   parameters,
                   options: new SideDialogOptions
                   {
                       CloseDialogOnOverlayClick = true,
                       Position = DialogPosition.Right,
                       ShowMask = true,
                       Width = "700px"
                   });
        }

        public async Task OnEditComplete(AssetTypeModel item)
        {
            DialogService.CloseSide();

            var existingItem = listItems.FirstOrDefault(x => x.Id == item.Id);
            if (existingItem != null)
            {
                existingItem.Name = item.Name;
                existingItem.Description = item.Description;
            }
            else
            {
                listItems.Add(new AssetTypeModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description
                });
            }
            await listGrid.RefreshDataAsync();
        }

        private async Task Add_Clicked()
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Id", 0);
            parameters.Add("OnSaveClickCallback", EventCallback.Factory.Create<AssetTypeModel>(this, OnEditComplete));

            await DialogService.OpenSideAsync<AssetTypeEdit>($"Add New Asset Type",
                   parameters,
                   options: new SideDialogOptions
                   {
                       CloseDialogOnOverlayClick = true,
                       Position = DialogPosition.Right,
                       ShowMask = true,
                       Width = "700px"
                   });
        }

        //private sealed class ListModel
        //{
        //    public int Id { get; set; }

        //    public string Name { get; set; } = null!;

        //    public string Description { get; set; } = null!;
        //}
    }
}