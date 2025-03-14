using Ardalis.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public partial class Meta
    {
        public async Task<Result<Core.Models.Data.AssetType>> AddAssetTypeAsync(Core.Models.Data.AssetType item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.IsDeleted = false;
                item.AllowUserDelete = true;
                item.Sequence = 0;

                var result = await _dataSet.AddAsync(item);
                if (result == null)
                {
                    return Result<Core.Models.Data.AssetType>.Error($"Error adding AssetType ({item.Name})");
                }

                return Result.Success(result);
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.AssetType>.Error(Ex.Message);
            }
        }


        public async Task<Result<bool>> DeleteAssetTypeAsync(int Id)
        {
            try
            {
                var result = await _dataSet.DeleteAsync(Id);
                if (!result)
                {
                    return Result<bool>.Error($"Error deleting Source ({Id})");
                }

                return Result.Success(true);
            }
            catch (Exception Ex)
            {
                return Result<bool>.Error(Ex.Message);
            }
        }

        public async Task<IReadOnlyList<Core.Models.Data.AssetType>> GetAllAssetTypesAsync()
        {
            try
            {
                return await _dataSet.GetAllAsync();
            }
            catch (Exception Ex)
            {
                return new List<Core.Models.Data.AssetType>();
            }
        }


        public async Task<Result<Core.Models.Data.AssetType>> GetAssetTypeByIdAsync(int Id)
        {
            try
            {
                var item = await _dataSet.GetByIdAsync(Id);
                if (item == null)
                {
                    return Result<Core.Models.Data.AssetType>.Error($"AssetType ({Id}) not found");
                }

                return item;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.AssetType>.Error(Ex.Message);
            }
        }


        public async Task<Result<Core.Models.Data.AssetType>> UpdateAssetTypeAsync(Core.Models.Data.AssetType item)
        {
            try
            {
                var metaItem = await _dataSet.UpdateAsync(item);
                if (metaItem == null)
                {
                    return Result<Core.Models.Data.AssetType>.Error($"Error updating AssetType ({item.Name})");
                }

                return metaItem;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.AssetType>.Error(Ex.Message);
            }
        }
    }
}
