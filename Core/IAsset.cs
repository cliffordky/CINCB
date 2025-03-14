using Ardalis.Result;
using Core.Models.Data;

namespace Core
{
    public interface IAsset
    {
        Task<List<Models.Data.Asset>> GetAllAssetsAsync();

        Task<List<Models.Data.Asset>> GetUserAssetsAsync(int userId);

        Task<Result<Models.Data.Asset>> GetAssetDetailsByAssetCodeAsync(string AssetCode);

        Task<Result<Core.Models.Data.Asset>> GetAssetDetailsByPublicKeyAsync(Guid PublicKey);

        Task<Result<Models.Data.Asset>> GetAssetDetailsByIdAsync(int Id);

        Task<Result<Core.Models.Data.Asset>> GetAssetPublicKeyAsync(Guid PublicKey);

        Task<List<Models.Data.Asset>> GetAllAssetGroupsAsync();

        Task<Result<Models.Data.Asset>> UpdateAssetAsync(Models.Data.Asset asset);

        Task<Result<bool>> DeleteAssetAsync(int Id);



        Task<Result<Core.Models.Data.Asset>> ProvisionAssetByAttributeValueAsync(int attributeId, string value);
        Task<Result<Core.Models.Data.Asset>> GetAssetByIdentityAttributeValueAsync(string value);

        #region AssetAttribute
        Task<Result<Core.Models.Data.AssetAttribute>> GetAssetAttributeByIdAsync(int Id);
        Task<IReadOnlyList<Core.Models.Data.AssetAttribute>> GetAllAssetAttributesAsync();
        Task<IReadOnlyList<Core.Models.Data.AssetAttribute>> GetAllAssetAttributesByAssetIdAsync(int Id);
        Task<IReadOnlyList<Core.Models.Data.AssetAttribute>> GetAllAssetAttributesByAttributeIdAsync(int Id);
        Task<Result<Core.Models.Data.AssetAttribute>> UpdateAssetAttributeAsync(Models.Data.AssetAttribute item);
        Task<Result<Core.Models.Data.AssetAttribute>> AddAssetAttributeAsync(Models.Data.AssetAttribute item);
        Task<Result<bool>> DeleteAssetAttributeAsync(int Id);
        #endregion





        #region Asset Analyses
        Task<Result<Core.Models.Data.AssetDocument>> GetAssetDocumentByIdAsync(int Id);
        Task<Result<Core.Models.Data.AssetDocument>> GetAssetDocumentByPublicKeyAsync(Guid PublicKey);
        Task<IReadOnlyList<Core.Models.Data.AssetDocument>> GetAllAssetDocumentsAsync();
        Task<IReadOnlyList<Core.Models.Data.AssetDocument>> GetAllAssetDocumentsByAssetIdAsync(int Id);
        Task<Result<Core.Models.Data.AssetDocument>> UpdateAssetDocumentAsync(Models.Data.AssetDocument item);
        Task<Result<Core.Models.Data.AssetDocument>> AddAssetDocumentAsync(Models.Data.AssetDocument item);
        Task<Result<bool>> DeleteAssetDocumentAsync(int Id);
        #endregion



        Task<List<Models.Data.AssetUser>> GetUsersForAssetAsync(int AssetId);
        Task<List<Models.Data.AssetUser>> GetAssetsForUserAsync(int UserId);
        Task<Ardalis.Result.Result<Models.Data.AssetUser>> AddAssetUserAsync(int id, int userId);
        Task<Ardalis.Result.Result<bool>> DeleteAssetUserAsync(int id, int userId);
    }
}