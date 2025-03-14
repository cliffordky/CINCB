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
        public async Task<Result<Core.Models.Data.Organization>> AddOrganizationAsync(Core.Models.Data.Organization item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.IsDeleted = false;
                item.AllowUserDelete = true;
                item.Sequence = 0;

                var result = await _organizationRepository.AddAsync(item);
                if (result == null)
                {
                    return Result<Core.Models.Data.Organization>.Error($"Error adding Organization ({item.Name})");
                }

                return Result.Success(result);
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.Organization>.Error(Ex.Message);
            }
        }


        public async Task<Result<bool>> DeleteOrganizationAsync(int Id)
        {
            try
            {
                var result = await _organizationRepository.DeleteAsync(Id);
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

        public async Task<IReadOnlyList<Core.Models.Data.Organization>> GetAllOrganizationsAsync()
        {
            try
            {
                return await _organizationRepository.GetAllAsync();
            }
            catch (Exception Ex)
            {
                return new List<Core.Models.Data.Organization>();
            }
        }


        public async Task<Result<Core.Models.Data.Organization>> GetOrganizationByIdAsync(int Id)
        {
            try
            {
                var item = await _organizationRepository.GetByIdAsync(Id);
                if (item == null)
                {
                    return Result<Core.Models.Data.Organization>.Error($"Organization ({Id}) not found");
                }

                return item;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.Organization>.Error(Ex.Message);
            }
        }


        public async Task<Result<Core.Models.Data.Organization>> UpdateOrganizationAsync(Core.Models.Data.Organization item)
        {
            try
            {
                var metaItem = await _organizationRepository.UpdateAsync(item);
                if (metaItem == null)
                {
                    return Result<Core.Models.Data.Organization>.Error($"Error updating Organization ({item.Name})");
                }

                return metaItem;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.Organization>.Error(Ex.Message);
            }
        }
    }
}
