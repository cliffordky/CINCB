using Ardalis.Result;
using Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IAssetDocument : IGenericRepository<Models.Data.AssetDocument>
    {
        Task<IReadOnlyList<AssetDocument>> GetAllAssetDocumentsByAssetIdAsync(int id);
        Task<Result<AssetDocument>> GetAssetDocumentByPublicKeyAsync(Guid publicKey);
    }
}
