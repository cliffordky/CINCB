using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
	public interface IAssetAttribute : IGenericRepository<Models.Data.AssetAttribute>
	{
        Task<IReadOnlyList<Core.Models.Data.AssetAttribute>> GetAllAssetAttributesByAssetIdAsync(int Id);
        Task<IReadOnlyList<Core.Models.Data.AssetAttribute>> GetAllAssetAttributesByAttributeIdAsync(int Id);
    }
}
