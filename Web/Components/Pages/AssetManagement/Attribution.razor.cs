using Radzen.Blazor;
using Radzen;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace Web.Components.Pages.AssetManagement
{
    public partial class Attribution
    {
        [Inject] private AuthenticationStateProvider _authenticationStateProvider { get; set; }
        [Parameter] public int AssetId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var assetResult = await _asset.GetAssetDetailsByIdAsync(AssetId);

            if (!assetResult.IsSuccess) return;

            users.AddRange((await _access.GetUsersAsync()).ToList());
            assetTypes.AddRange((await _meta.GetAllAssetTypesAsync()).ToList());
            attributes.AddRange((await _meta.GetAllAttributeItemsAsync()).ToList().Where(x => x.AssetTypeId == assetResult.Value.AssetTypeId));

            await LoadGridDataAsync();
        }

        private async Task LoadGridDataAsync()
        {
            assetAttributes.Clear();

            var _ = (await _asset.GetAllAssetAttributesByAssetIdAsync(AssetId)).ToList();

            foreach (var item in _)
            {
                var assetAttribute = new AssetAttribute()
                {
                    Id = item.Id,
                    AssetId = item.AssetId,
                    AttributeId = item.AttributeId,
                    CreatedDate = item.CreatedDate,
                    DueDate = item.DueDate,
                };

                var attributeDetail = attributes.FirstOrDefault(x => x.Id == assetAttribute.AttributeId);
                if (attributeDetail is not null)
                {
                    assetAttribute.AttributeName = attributeDetail.Name;
                    assetAttribute.DataTypeId = attributeDetail.DataTypeId;

                    if (attributeDetail.DataTypeId == 2)
                    {
                        assetAttribute.ValueString = item.Value;
                    }
                    else if (attributeDetail.DataTypeId == 4)
                    {
                        if (decimal.TryParse(item.Value, out decimal _value))
                        {
                            assetAttribute.ValueNumber = _value;
                        }
                        //assetAttribute.ValueNumber = Convert.ToDecimal(item.Value);
                    }
                    else if (attributeDetail.DataTypeId == 3)
                    {
                        assetAttribute.ValueDate = Convert.ToDateTime(item.Value);
                    }
                }

                var modifiedUser = users.FirstOrDefault(x => x.Id == item.ModifiedUser);
                if (modifiedUser != null)
                {
                    assetAttribute.ModifiedUser = $"{modifiedUser.FirstName} {modifiedUser.LastName}";
                }

                assetAttributes.Add(assetAttribute);
            }

            await attributesGrid.RefreshDataAsync();
        }

        private async Task<int> getUserId()
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

        private List<Core.Models.Data.Attribute> attributes = new List<Core.Models.Data.Attribute>();
        private List<Core.Models.Data.User> users = new List<Core.Models.Data.User>();
        private List<Core.Models.Data.AssetType> assetTypes = new List<Core.Models.Data.AssetType>();

        private RadzenDataGrid<AssetAttribute> attributesGrid;
        private List<AssetAttribute> assetAttributes = new List<AssetAttribute>();

        private DataGridEditMode editMode = DataGridEditMode.Single;

        private List<AssetAttribute> attributesToInsert = new List<AssetAttribute>();
        private List<AssetAttribute> attributesToUpdate = new List<AssetAttribute>();

        private void Reset()
        {
            attributesToInsert.Clear();
            attributesToUpdate.Clear();
        }

        private void Reset(AssetAttribute assetAttribute)
        {
            attributesToInsert.Remove(assetAttribute);
            attributesToUpdate.Remove(assetAttribute);
        }

        private async Task EditRow(AssetAttribute assetAttribute)
        {
            if (!attributesGrid.IsValid) return;

            if (editMode == DataGridEditMode.Single)
            {
                Reset();
            }

            attributesToUpdate.Add(assetAttribute);
            await attributesGrid.EditRow(assetAttribute);
        }

        private string GetAttributeName(int attributeId)
        {
            return attributes.FirstOrDefault(x => x.Id == attributeId)?.Name;
        }

        private Core.Enumerations.DataType GetAttributeDataType(int attributeId)
        {
            var attribute = attributes.FirstOrDefault(x => x.Id == attributeId);
            if (attribute == null) return Core.Enumerations.DataType.TEXT;

            return (Core.Enumerations.DataType)attribute.DataTypeId;
        }

        private async void OnUpdateRow(AssetAttribute assetAttribute)
        {
            Reset(assetAttribute);
            var itemResult = await _asset.GetAssetAttributeByIdAsync(assetAttribute.Id);
            if (itemResult.IsSuccess)
            {
                if (assetAttribute.DataTypeId == 2)
                {
                    itemResult.Value.Value = assetAttribute.ValueString;
                }
                else if (assetAttribute.DataTypeId == 4)
                {
                    itemResult.Value.Value = assetAttribute.ValueNumber.ToString();
                }
                else if (assetAttribute.DataTypeId == 3)
                {
                    itemResult.Value.Value = assetAttribute.ValueDate.ToString();
                }

                itemResult.Value.AttributeId = assetAttribute.AttributeId;
                //itemResult.Value.CreatedDate = DateTime.Now;
                itemResult.Value.ModifiedDate = DateTime.Now;
                itemResult.Value.ModifiedUser = await getUserId();

                await _asset.UpdateAssetAttributeAsync(itemResult.Value);
            }
        }

        private async Task SaveRow(AssetAttribute assetAttribute)
        {
            await attributesGrid.UpdateRow(assetAttribute);
        }

        private void CancelEdit(AssetAttribute assetAttribute)
        {
            Reset(assetAttribute);
            attributesGrid.CancelEditRow(assetAttribute);
        }

        private async Task DeleteRow(AssetAttribute assetAttribute)
        {
            Reset(assetAttribute);

            if (assetAttributes.Contains(assetAttribute))
            {
                await _asset.DeleteAssetAttributeAsync(assetAttribute.Id);
                assetAttributes.Remove(assetAttribute);
                await attributesGrid.Reload();
            }
            else
            {
                attributesGrid.CancelEditRow(assetAttribute);
                await attributesGrid.Reload();
            }
        }

        private async Task InsertRow()
        {
            if (!attributesGrid.IsValid) return;

            if (editMode == DataGridEditMode.Single)
            {
                Reset();
            }

            var assetAttribute = new AssetAttribute();
            attributesToInsert.Add(assetAttribute);
            await attributesGrid.InsertRow(assetAttribute);
        }

        private async void OnCreateRow(AssetAttribute assetAttribute)
        {
            var attributeResult = await _meta.GetAttributeByIdAsync(assetAttribute.AttributeId);
            if (!attributeResult.IsSuccess)
                return;

            var item = new Core.Models.Data.AssetAttribute()
            {
                AssetId = AssetId,
                AttributeId = assetAttribute.AttributeId,
                SourceId = attributeResult.Value.SourceId.Value,
                CreatedDate = assetAttribute.CreatedDate,
                ModifiedDate = DateTime.Now,
                ModifiedUser = await getUserId()
            };

            assetAttribute.DataTypeId = attributeResult.Value.DataTypeId;

            if (assetAttribute.DataTypeId == 2)
            {
                item.Value = assetAttribute.ValueString;
            }
            else if (assetAttribute.DataTypeId == 4)
            {
                item.Value = assetAttribute.ValueNumber.ToString();
            }
            else if (assetAttribute.DataTypeId == 3)
            {
                item.Value = assetAttribute.ValueDate.ToString();
            }

            await _asset.AddAssetAttributeAsync(item);
            attributesToInsert.Remove(assetAttribute);
            //await attributesGrid.Reload();

            await LoadGridDataAsync();
        }

        public class AssetAttribute
        {
            public int Id { get; set; }
            public int AssetId { get; set; }
            public int AttributeId { get; set; }
            public string AttributeName { get; set; }
            public string ValueString { get; set; }
            public decimal? ValueNumber { get; set; }

            public bool ValueBoolean { get; set; }
            public DateTime? ValueDate { get; set; }
            public DateTime CreatedDate { get; set; }
            public int DataTypeId { get; internal set; }
            public string ModifiedUser { get; internal set; }
            public DateTime? DueDate { get; set; }
        }

        private void CellRender(DataGridCellRenderEventArgs<AssetAttribute> args)
        {
            if (args.Column.Property == "DueDate")
            {
                bool isOverdue = (args.Data.DueDate.HasValue) && (args.Data.DueDate.Value < args.Data.CreatedDate);
                bool showWarning = (isOverdue) && (string.IsNullOrEmpty(args.Data.ValueString));
                args.Attributes.Add("style", $"background-color: {(showWarning ? "var(--rz-danger)" : "var(--rz-base-background-color)")};");
            }
        }
    }
}