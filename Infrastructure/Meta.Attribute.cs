using Ardalis.Result;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
	public partial class Meta : Core.IMeta
	{
		public async Task<Result<Core.Models.Data.Attribute>> AddAttributeAsync(Core.Models.Data.Attribute item)
		{
			try
			{
				item.CreatedDate = DateTime.Now;
				item.ModifiedDate = DateTime.Now;
				item.IsDeleted = false;
				item.AllowUserDelete = true;
				item.Sequence = 0;

				var result = await _attribute.AddAsync(item);
				if (result == null)
				{
					return Result.Invalid(
						new List<ValidationError> {
							new ValidationError
							{
								Identifier = "ERROR",
								ErrorMessage = $"Error adding Attribute ({item.Name})"
							}
						});
				}

				return Result.Success(result);
			}
			catch (Exception Ex)
			{
				return Result.Invalid(
					new List<ValidationError> {
						new ValidationError
						{
							Identifier = "ERROR",
							ErrorMessage = Ex.Message
						}
					}
				);
			}
		}

		public async Task<Result<bool>> DeleteAttributeAsync(int Id)
		{
			try
			{
				var result = await _attribute.DeleteAsync(Id);
				if (!result)
				{
					return Result<bool>.Invalid(
						new List<ValidationError> {
							new ValidationError
							{
								Identifier = "ERROR",
								ErrorMessage = $"Error deleting Attribute ({Id})"
							}
						}
					);
				}

				return Result.Success(true);
			}
			catch (Exception Ex)
			{
				return Result.Invalid(
					 new List<ValidationError> {
						new ValidationError
						{
							Identifier = "ERROR",
							ErrorMessage = Ex.Message
						}
					 }
				 );
			}
		}

		public async Task<IReadOnlyList<Core.Models.Data.Attribute>> GetAllAttributeItemsAsync()
		{
			try
			{
				return await _attribute.GetAllAsync();
			}
			catch (Exception Ex)
			{
				return new List<Core.Models.Data.Attribute>();
			}
		}

		public async Task<Result<Core.Models.Data.Attribute>> GetAttributeByIdAsync(int Id)
		{
			try
			{
				var product = await _attribute.GetByIdAsync(Id);
				if (product == null)
				{
					return Result<Core.Models.Data.Attribute>.Error($"Attribute ({Id}) not found");
				}

				return product;
			}
			catch (Exception Ex)
			{
				return Result<Core.Models.Data.Attribute>.Error(Ex.Message);
			}
		}

        public async Task<IReadOnlyList<Core.Models.Data.Attribute>> GetAttributesByAssetTypeIdAsync(int AssetTypeId)
        {
            try
            {
                return await _attribute.GetAttributesByAssetTypeIdAsync(AssetTypeId);
            }
            catch (Exception Ex)
            {
                return new List<Core.Models.Data.Attribute>();
            }
        }

        public async Task<Result<Core.Models.Data.Attribute>> UpdateAttributeAsync(Core.Models.Data.Attribute item)
		{
			try
			{
				var metaItem = await _attribute.UpdateAsync(item);
				if (metaItem == null)
				{
					return Result<Core.Models.Data.Attribute>.Error($"Error updating Attribute ({item.Name})");
				}

				return metaItem;
			}
			catch (Exception Ex)
			{
				return Result<Core.Models.Data.Attribute>.Error(Ex.Message);
			}
		}
	}
}