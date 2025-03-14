using Ardalis.Result;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
	public partial class Meta : Core.IMeta
	{
		public async Task<Result<Core.Models.Data.Source>> AddSourceAsync(Core.Models.Data.Source item)
		{
			try
			{
				item.CreatedDate = DateTime.Now;
				item.ModifiedDate = DateTime.Now;
				item.IsDeleted = false;
				item.AllowUserDelete = true;
				item.Sequence = 0;

				var result = await _source.AddAsync(item);
				if (result == null)
				{
					return Result<Core.Models.Data.Source>.Error($"Error adding Source ({item.Name})");
				}

				return Result.Success(result);
			}
			catch (Exception Ex)
			{
				return Result<Core.Models.Data.Source>.Error(Ex.Message);
			}
		}

		public async Task<Result<bool>> DeleteSourceAsync(int Id)
		{
			try
			{
				var result = await _source.DeleteAsync(Id);
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

		public async Task<IReadOnlyList<Core.Models.Data.Source>> GetAllSourceItemsAsync()
		{
			try
			{
				return await _source.GetAllAsync();
			}
			catch (Exception Ex)
			{
				return new List<Core.Models.Data.Source>();
			}
		}

		public async Task<Result<Core.Models.Data.Source>> GetSourceByIdAsync(int Id)
		{
			try
			{
				var product = await _source.GetByIdAsync(Id);
				if (product == null)
				{
					return Result<Core.Models.Data.Source>.Error($"Source ({Id}) not found");
				}

				return product;
			}
			catch (Exception Ex)
			{
				return Result<Core.Models.Data.Source>.Error(Ex.Message);
			}
		}

		public async Task<Result<Core.Models.Data.Source>> UpdateSourceAsync(Core.Models.Data.Source item)
		{
			try
			{
				var metaItem = await _source.UpdateAsync(item);
				if (metaItem == null)
				{
					return Result<Core.Models.Data.Source>.Error($"Error updating Source ({item.Name})");
				}

				return metaItem;
			}
			catch (Exception Ex)
			{
				return Result<Core.Models.Data.Source>.Error(Ex.Message);
			}
		}
	}
}