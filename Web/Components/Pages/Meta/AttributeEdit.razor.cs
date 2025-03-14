using Ardalis.Result;
using EnumsNET;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Radzen;
using Radzen.Blazor;
using System.Security.Claims;

namespace Web.Components.Pages.Meta
{
    public partial class AttributeEdit
    {
        [Parameter]
        [SupplyParameterFromQuery(Name = "id")]
        public int Id { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "assetTypeId")]
        public int AssetTypeId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            DataTypes.Clear();
            foreach (var item in Enums.GetValues<Core.Enumerations.DataType>())
            {
                DataTypes.Add(new DropDownListItem()
                {
                    Id = (int)item,
                    Name = item.AsString(EnumFormat.Description)
                });
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if (AssetTypeId > 0)
            {
                var getResult = await _meta.GetAssetTypeByIdAsync(AssetTypeId);
                if (getResult.IsSuccess)
                {
                    input.AssetType = getResult.Value.Name;
                }
            }

            if (Id > 0)
            {
                var getResult = await _meta.GetAttributeByIdAsync(Id);
                if (getResult.IsSuccess)
                {
                    var attribute = getResult.Value;
                    input.Name = attribute.Name;
                    input.Description = attribute.Description;
                    input.DataTypeId = attribute.DataTypeId;
                    input.IsUnique = attribute.IsUnique;
                    input.CronExpression = attribute.CronExpression;
                    input.ModifiedDate = attribute.ModifiedDate;
                    input.ModifiedUser = attribute.ModifiedUser;
                }
            }
        }

        private async Task Submit(AttributeModel arg)
        {
            if (Id > 0)
            {
                var getResult = await _meta.GetAttributeByIdAsync(Id);
                if (getResult.IsSuccess)
                {
                    var attribute = getResult.Value;
                    attribute.Name = arg.Name;
                    attribute.Description = arg.Description;
                    attribute.DataTypeId = (int)arg.DataTypeId;
                    attribute.IsUnique = arg.IsUnique;
                    attribute.CronExpression = arg.CronExpression;
                    attribute.ModifiedUser = await base.GetUserId();

                    var updateResult = await _meta.UpdateAttributeAsync(attribute);
                    if (updateResult.IsSuccess)
                    {
                        NavigationManager.NavigateTo($"/meta/asset-type-list?AssetTypeId={AssetTypeId}", true);
                    }
                    else
                    {
                        IsAlertVisible = true;
                        AlertBody = $"There was an error updating this attribute, please try again.";
                        StateHasChanged();
                    }
                }
                else
                {
                    IsAlertVisible = true;
                    AlertBody = $"There was an error saving this attribute, please try again.";
                    StateHasChanged();
                }
            }
            else
            {
                Core.Models.Data.Attribute attribute = new()
                {
                    Name = arg.Name,
                    Description = arg.Description,
                    DataTypeId = (int)arg.DataTypeId,
                    IsUnique = arg.IsUnique,
                    CronExpression = arg.CronExpression,
                    AssetTypeId = AssetTypeId,
                    ModifiedUser = await base.GetUserId()
                };

                var addResult = await _meta.AddAttributeAsync(attribute);
                if (addResult.IsSuccess)
                {
                    NavigationManager.NavigateTo($"/meta/asset-type-list?AssetTypeId={AssetTypeId}", true);
                }
                else
                {
                    IsAlertVisible = true;
                    AlertBody = $"There was an error adding this attribute, please try again.";
                    StateHasChanged();
                }
            }

            // NavigationManager.NavigateTo($"/meta/asset-type-list?AssetTypeId={AssetTypeId}", true);
        }

        private void OnInvalidSubmit(FormInvalidSubmitEventArgs args)
        {
        }

        private async Task Cancel()
        {
            NavigationManager.NavigateTo($"/meta/asset-type-list?AssetTypeId={AssetTypeId}", true);
        }

        private AttributeModel input { get; set; } = new();

        private List<DropDownListItem> DataTypes { get; set; } = new List<DropDownListItem>();

        private class DropDownListItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}