using Ardalis.Result;
using Core;
using Core.Repository;
using Flurl.Util;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public partial class Asset : Core.IAsset
    {
        private readonly ILogger<Asset> _logger;
        private readonly Core.Repository.IAsset _assetRepository;
        private readonly IAssetAttribute _assetAttribute;
        private readonly IAttribute _attribute;
        private readonly IAssetDocument _assetAnalysisRepository;
        private readonly IAssetUser _assetUserRepository;
        private readonly IAssetDocument _assetDocumentRepository;

        public Asset(ILogger<Infrastructure.Asset> logger, Core.Repository.IAsset assetRepository, Core.Repository.IAssetAttribute assetAttribute, Core.Repository.IAttribute attribute, Core.Repository.IAssetDocument assetAnalysis, Core.Repository.IAssetUser assetUser, Core.Repository.IAssetDocument assetDocument)
        {
            _logger = logger;
            _assetRepository = assetRepository;
            _assetAttribute = assetAttribute;
            _attribute = attribute;
            _assetAnalysisRepository = assetAnalysis;
            _assetUserRepository = assetUser;
            _assetDocumentRepository = assetDocument;
        }

        public async Task<List<Core.Models.Data.Asset>> GetAllAssetsAsync()
        {
            var result = await _assetRepository.GetAllAsync();

            return result.ToList();
        }

        public async Task<List<Core.Models.Data.Asset>> GetAllAssetGroupsAsync()
        {
            var result = await _assetRepository.GetAllAssetGroupsAsync();
            return result.ToList();
        }

        public async Task<List<Core.Models.Data.Asset>> GetUserAssetsAsync(int userId)
        {
            var result = await _assetRepository.GetUserAssetsAsync(userId);
            return result.ToList();
        }

        public async Task<Result<Core.Models.Data.Asset>> GetAssetDetailsByIdAsync(int Id)
        {
            var asset = await _assetRepository.GetByIdAsync(Id);
            if (asset == null)
            {
                return Result<Core.Models.Data.Asset>.Invalid(
                    new List<ValidationError> {
                        new ValidationError
                        {
                            Identifier = "CUSTOMER",
                            ErrorMessage = $"Asset with Id:{Id} not found"
                        }
                });
            }

            return asset;
        }

        public async Task<Result<Core.Models.Data.Asset>> GetAssetDetailsByPublicKeyAsync(Guid PublicKey)
        {
            var asset = await _assetRepository.GetByPublicKeyAsync(PublicKey);
            if (asset == null)
            {
                return Result<Core.Models.Data.Asset>.Invalid(
                    new List<ValidationError> {
                        new ValidationError
                        {
                            Identifier = "CUSTOMER",
                            ErrorMessage = $"Asset with PublicKey:{PublicKey} not found"
                        }
                });
            }

            return asset;
        }

        public async Task<Result<Core.Models.Data.Asset>> GetAssetDetailsByAssetCodeAsync(string AssetCode)
        {
            var asset = await _assetRepository.GetByAssetCodeAsync(AssetCode);
            if (asset == null)
            {
                return Result<Core.Models.Data.Asset>.Invalid(
                    new List<ValidationError> {
                        new ValidationError
                        {
                            Identifier = "CUSTOMER",
                            ErrorMessage = $"Asset with Asset Code:{AssetCode} not found"
                        }
                });
            }

            return asset;
        }

        public async Task<Result<Core.Models.Data.Asset>> GetAssetPublicKeyAsync(Guid PublicKey)
        {
            var asset = await _assetRepository.GetByPublicKeyAsync(PublicKey);

            return asset;
        }

        public async Task<Result<Core.Models.Data.Asset>> GetAssetDealByPublicKeyAsync(Guid PublicKey)
        {
            var asset = await _assetRepository.GetByPublicKeyAsync(PublicKey);
            if (asset == null)
            {
                return Result<Core.Models.Data.Asset>.Invalid(
                    new List<ValidationError> {
                        new ValidationError
                        {
                            Identifier = "CUSTOMER",
                            ErrorMessage = $"Asset with PublicKey:{PublicKey} not found"
                        }
                });
            }

            return asset;
        }

        public async Task<Result<Core.Models.Data.Asset>> UpdateAssetAsync(Core.Models.Data.Asset asset)
        {
            try
            {
                var result = await _assetRepository.UpdateAsync(asset);
                if (result == null)
                {
                    return Result.Invalid(
                        new List<ValidationError> {
                            new ValidationError
                            {
                                Identifier = "ERROR",
                                ErrorMessage = $"Error updating Asset:{asset.Name} {asset.Description}"
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

        public async Task<Result<bool>> DeleteAssetAsync(int Id)
        {
            if (await _assetRepository.DeleteAsync(Id))
            {
                return Result<bool>.Success(true);
            }
            else
            {
                return Result<bool>.Invalid(new List<ValidationError> {
                    new ValidationError
                    {
                        Identifier = "CUSTOMER",
                        ErrorMessage = $"Error deleting Asset with Id:{Id}"
                    }
                });
            }
        }

        public async Task<Result<Core.Models.Data.AssetAttribute>> GetAssetAttributeByIdAsync(int Id)
        {
            try
            {
                var item = await _assetAttribute.GetByIdAsync(Id);
                if (item == null)
                {
                    return Result<Core.Models.Data.AssetAttribute>.Error($"Asset attribute ({Id}) not found");
                }

                return item;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.AssetAttribute>.Error(Ex.Message);
            }
        }

        public Task<IReadOnlyList<Core.Models.Data.AssetAttribute>> GetAllAssetAttributesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Core.Models.Data.AssetAttribute>> GetAllAssetAttributesByAssetIdAsync(int Id)
        {
            try
            {
                return await _assetAttribute.GetAllAssetAttributesByAssetIdAsync(Id);
            }
            catch (Exception Ex)
            {
                return new List<Core.Models.Data.AssetAttribute>();
            }
        }

        public async Task<IReadOnlyList<Core.Models.Data.AssetAttribute>> GetAllAssetAttributesByAttributeIdAsync(int AttributeId)
        {
            try
            {
                return await _assetAttribute.GetAllAssetAttributesByAttributeIdAsync(AttributeId);
            }
            catch (Exception Ex)
            {
                return new List<Core.Models.Data.AssetAttribute>();
            }
        }

        public async Task<Result<Core.Models.Data.AssetAttribute>> UpdateAssetAttributeAsync(Core.Models.Data.AssetAttribute item)
        {
            try
            {
                var result = await _assetAttribute.UpdateAsync(item);
                if (result == null)
                {
                    return Result<Core.Models.Data.AssetAttribute>.Error($"Error updating attribute ({item.Value})");
                }

                return result;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.AssetAttribute>.Error(Ex.Message);
            }
        }

        public async Task<Result<Core.Models.Data.AssetAttribute>> AddAssetAttributeAsync(Core.Models.Data.AssetAttribute item)
        {
            try
            {
                item.ModifiedDate = DateTime.Now;
                item.IsDeleted = false;
                item.AllowUserDelete = true;
                item.Sequence = 0;

                var result = await _assetAttribute.AddAsync(item);
                if (result == null)
                {
                    return Result<Core.Models.Data.AssetAttribute>.Error($"Error adding attribute ({item.Value})");
                }

                return Result.Success(result);
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.AssetAttribute>.Error(Ex.Message);
            }
        }

        public async Task<Result<bool>> DeleteAssetAttributeAsync(int Id)
        {
            try
            {
                var result = await _assetAttribute.DeleteAsync(Id);
                if (!result)
                {
                    return Result<bool>.Error($"Error deleting attribute ({Id})");
                }

                return Result.Success(true);
            }
            catch (Exception Ex)
            {
                return Result<bool>.Error(Ex.Message);
            }
        }

        public async Task<Result<Core.Models.Data.Asset>> ProvisionAssetByAttributeValueAsync(int attributeId, string value)
        {
            try
            {
                var result = await _assetRepository.ProvisionAssetByAttributeValueAsync(attributeId, value);
                if (result == null)
                {
                    return Result<Core.Models.Data.Asset>.Error($"Error provisioning asset attribute");
                }

                return result;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.Asset>.Error(Ex.Message);
            }
        }

        public async Task<Result<Core.Models.Data.Asset>> GetAssetByIdentityAttributeValueAsync(string value)
        {
            try
            {
                var result = await _assetRepository.GetAssetByIdentityAttributeValueAsync(value);
                if (result == null)
                {
                    return Result<Core.Models.Data.Asset>.Error($"Error getting asset");
                }

                return result;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.Asset>.Error(Ex.Message);
            }
        }

        public async Task<Result<Core.Models.Data.AssetDocument>> GetAssetDocumentByIdAsync(int Id)
        {
            try
            {
                var item = await _assetDocumentRepository.GetByIdAsync(Id);
                if (item == null)
                {
                    return Result<Core.Models.Data.AssetDocument>.Error($"Asset Document ({Id}) not found");
                }

                return item;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.AssetDocument>.Error(Ex.Message);
            }
        }

        public async Task<IReadOnlyList<Core.Models.Data.AssetDocument>> GetAllAssetDocumentsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Core.Models.Data.AssetDocument>> GetAllAssetDocumentsByAssetIdAsync(int Id)
        {
            try
            {
                return await _assetDocumentRepository.GetAllAssetDocumentsByAssetIdAsync(Id);
            }
            catch (Exception Ex)
            {
                return new List<Core.Models.Data.AssetDocument>();
            }
        }
        public async Task<Result<Core.Models.Data.AssetDocument>> GetAssetDocumentByPublicKeyAsync(Guid PublicKey)
        {
            try
            {
                var item = await _assetDocumentRepository.GetAssetDocumentByPublicKeyAsync(PublicKey);
                if (item == null)
                {
                    return Result<Core.Models.Data.AssetDocument>.Error($"Asset Document ({PublicKey}) not found");
                }

                return item;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.AssetDocument>.Error(Ex.Message);
            }
        }


        public async Task<Result<Core.Models.Data.AssetDocument>> UpdateAssetDocumentAsync(Core.Models.Data.AssetDocument item)
        {
            try
            {
                var result = await _assetDocumentRepository.UpdateAsync(item);
                if (result == null)
                {
                    return Result<Core.Models.Data.AssetDocument>.Error($"Error updating Asset Document ({item.Id})");
                }

                return result;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.AssetDocument>.Error(Ex.Message);
            }
        }

        public async Task<Result<Core.Models.Data.AssetDocument>> AddAssetDocumentAsync(Core.Models.Data.AssetDocument item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.IsDeleted = false;
                item.AllowUserDelete = true;
                item.Sequence = 0;

                var result = await _assetDocumentRepository.AddAsync(item);
                if (result == null)
                {
                    return Result<Core.Models.Data.AssetDocument>.Error($"Error adding Asset Document ({item.Id})");
                }

                return Result.Success(result);
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.AssetDocument>.Error(Ex.Message);
            }
        }   

        public async Task<Result<bool>> DeleteAssetDocumentAsync(int Id)
        {
            try
            {
                var result = await _assetDocumentRepository.DeleteAsync(Id);
                if (!result)
                {
                    return Result<bool>.Error($"Error deleting Asset Document ({Id})");
                }

                return Result.Success(true);
            }
            catch (Exception Ex)
            {
                return Result<bool>.Error(Ex.Message);
            }
        }

        public async Task<List<Core.Models.Data.AssetUser>> GetUsersForAssetAsync(int AssetId)
        {
            var result = await _assetUserRepository.GetUsersForAssetAsync(AssetId);

            return result.ToList();
        }

        public async Task<List<Core.Models.Data.AssetUser>> GetAssetsForUserAsync(int UserId)
        {
            var result = await _assetUserRepository.GetAssetsForUserAsync(UserId);

            return result.ToList();
        }

        public async Task<Result<Core.Models.Data.AssetUser>> AddAssetUserAsync(int assetId, int userId)
        {
            var users = await _assetUserRepository.GetUsersForAssetAsync(assetId);
            if (users.Count(x => x.UserId == userId) == 0)
            {
                var item = new Core.Models.Data.AssetUser()
                {
                    AssetId = assetId,
                    UserId = userId,
                    RoleId = 0,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = false,
                    AllowUserDelete = true,
                    Sequence = 0,
                };

                var result = await _assetUserRepository.AddAsync(item);
                if (result == null)
                {
                    return Result<Core.Models.Data.AssetUser>.Error($"Error adding Asset User ({userId})");
                }

                return Result.Success(result);
            }

            return Result<Core.Models.Data.AssetUser>.Error($"User already exists on asset");
        }

        public async Task<Result<bool>> DeleteAssetUserAsync(int assetId, int userId)
        {
            var users = await _assetUserRepository.GetUsersForAssetAsync(assetId);
            if (users.Count(x => x.UserId == userId) == 0)
                return Result<bool>.Error($"User does not exist on asset");

            await _assetUserRepository.DeleteAsync(users.FirstOrDefault(x => x.UserId == userId).Id);

            return Result.Success(true);
        }


    }
}