using Ardalis.Result;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;
using System.Security.Claims;

namespace Web.Components.Pages.Meta
{
    public partial class AssetTypeEdit : PageBase
    {

        [Parameter]
        [SupplyParameterFromQuery(Name = "id")]
        public int Id { get; set; }

        [Parameter]
        public EventCallback<AssetTypeModel> OnSaveClickCallback { get; set; }

        private bool IsAlertVisible { get; set; } = false;
        private string AlertBody { get; set; } = "";
        private bool IsAttributeGridVisible { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            dialogService.OnClose += DialogService_OnClose; ;
        }

        private async void DialogService_OnClose(dynamic result)
        {
            if (result != null) // if the user hits the x near the top right null is returned
            {
                // result is false if the user clicks no
                if ((bool)result)
                {
                    await _meta.DeleteAssetTypeAsync(input.Id);
                }

                NavigationManager.NavigateTo("/meta/asset-type-list", true);
            }
        }

        public void Dispose()
        {
            // The DialogService is a singleton so it is advisable to unsubscribe.
            dialogService.OnClose -= DialogService_OnClose;
        }

        protected override async Task OnParametersSetAsync()
        {
            if (Id > 0)
            {
                var result = await _meta.GetAssetTypeByIdAsync(Id);
                if (result.IsSuccess)
                {
                    input.Id = result.Value.Id;
                    input.Name = result.Value.Name;
                    input.Description = result.Value.Description;
                    input.ModifiedUser = result.Value.ModifiedUser;
                    input.ModifiedDate = result.Value.ModifiedDate;

                    IsAttributeGridVisible = true;

                    var attributes = await _meta.GetAttributesByAssetTypeIdAsync(Id);
                    foreach (var attribute in attributes)
                    {
                        listItems.Add(new AttributeModel()
                        {
                            Id = attribute.Id,
                            Name = attribute.Name,
                            Description = attribute.Description,
                            DataTypeId = attribute.DataTypeId,
                        });
                    }

                    await listGrid.RefreshDataAsync();
                }
                else
                {
                    IsAlertVisible = true;
                    AlertBody = $"There was an error loading the asset type, please try again.";
                }
            }
        }

        private void OnInvalidSubmit(FormInvalidSubmitEventArgs args)
        {
        }

        private async Task Submit(AssetTypeModel arg)
        {
            var getResult = await _meta.GetAssetTypeByIdAsync(arg.Id);
            if (getResult.IsSuccess)
            {
                var item = getResult.Value;
                item.Name = arg.Name;
                item.Description = arg.Description;
                item.ModifiedUser = await base.GetUserId();

                await _meta.UpdateAssetTypeAsync(item);
                await OnSaveClickCallback.InvokeAsync(arg);
            }
            else
            {
                var addResult = await _meta.AddAssetTypeAsync(new Core.Models.Data.AssetType()
                {
                    Name = arg.Name,
                    Description = arg.Description,
                    ModifiedUser = await base.GetUserId()
                });

                if (addResult.IsSuccess)
                {
                    arg.Id = addResult.Value.Id;
                    await OnSaveClickCallback.InvokeAsync(arg);
                }
                else
                {
                    IsAlertVisible = true;
                    AlertBody = $"There was an error adding the new asset type, please try again.";
                    StateHasChanged();
                }
            }
        }



        private AssetTypeModel input = new AssetTypeModel()
        {
        };

        private RadzenDataGrid<AttributeModel> listGrid;
        private List<AttributeModel> listItems = new List<AttributeModel>();

        private async Task HandleRowClickEvent(AttributeModel args)
        {
            NavigationManager.NavigateTo($"/meta/attribute?id={args.Id}&assetTypeId={Id}", true);
        }

        private async Task Add_Clicked()
        {
            NavigationManager.NavigateTo($"/meta/attribute?id=0&assetTypeId={Id}", true);
        }



    }
}