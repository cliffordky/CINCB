using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IAttribute : IGenericRepository<Models.Data.Attribute>
    {
        Task<IReadOnlyList<Models.Data.Attribute>> GetAttributesByAssetTypeIdAsync(int assetTypeId);
    }
}