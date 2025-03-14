using Ardalis.Result;
using Core.Models.Data;

namespace Core.Repository
{
    public interface IAsset : IGenericRepository<Models.Data.Asset>
    {
        Task<Core.Models.Data.Asset?> GetByPublicKeyAsync(Guid PublicKey);

        Task<Core.Models.Data.Asset> GetByAssetCodeAsync(string assetCode);
        Task<IReadOnlyList<Models.Data.Asset>> GetUserAssetsAsync(int userId);
        Task<IReadOnlyList<Models.Data.Asset>> GetAllAssetGroupsAsync();
        Task<Core.Models.Data.Asset> ProvisionAssetByAttributeValueAsync(int attributeId, string value);
        Task<Core.Models.Data.Asset> GetAssetByIdentityAttributeValueAsync(string value);

        Task<IReadOnlyList<Models.Data.Asset>> GetUserAssetsByAttributeFilterAsync(string tsql);
    }
}
