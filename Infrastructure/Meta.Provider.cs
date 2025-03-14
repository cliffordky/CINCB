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

        public async Task<Result<Core.Models.Data.Provider>> AddProviderAsync(Core.Models.Data.Provider item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.IsDeleted = false;
                item.AllowUserDelete = true;
                item.Sequence = 0;

                var result = await _providerRepository.AddAsync(item);
                if (result == null)
                {
                    return Result<Core.Models.Data.Provider>.Error($"Error adding Provider ({item.Name})");
                }

                return Result.Success(result);
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.Provider>.Error(Ex.Message);
            }
        }


        public async Task<Result<bool>> DeleteProviderAsync(int Id)
        {
            try
            {
                var result = await _providerRepository.DeleteAsync(Id);
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

        public async Task<IReadOnlyList<Core.Models.Data.Provider>> GetAllProvidersAsync()
        {
            try
            {
                return await _providerRepository.GetAllAsync();
            }
            catch (Exception Ex)
            {
                return new List<Core.Models.Data.Provider>();
            }
        }


        public async Task<Result<Core.Models.Data.Provider>> GetProviderByIdAsync(int Id)
        {
            try
            {
                var item = await _providerRepository.GetByIdAsync(Id);
                if (item == null)
                {
                    return Result<Core.Models.Data.Provider>.Error($"Provider ({Id}) not found");
                }

                return item;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.Provider>.Error(Ex.Message);
            }
        }


        public async Task<Result<Core.Models.Data.Provider>> UpdateProviderAsync(Core.Models.Data.Provider item)
        {
            try
            {
                var metaItem = await _providerRepository.UpdateAsync(item);
                if (metaItem == null)
                {
                    return Result<Core.Models.Data.Provider>.Error($"Error updating Provider ({item.Name})");
                }

                return metaItem;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.Provider>.Error(Ex.Message);
            }
        }
    }
}
