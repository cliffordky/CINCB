using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IAssetUser : IGenericRepository<Models.Data.AssetUser>
    {
        Task<List<Models.Data.AssetUser>> GetUsersForAssetAsync(int AssetId);
        Task<List<Models.Data.AssetUser>> GetAssetsForUserAsync(int UserId);
    }
}
