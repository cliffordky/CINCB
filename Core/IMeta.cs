using Ardalis.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
	public interface IMeta
	{

		#region Attribute
		Task<Result<Core.Models.Data.Attribute>> GetAttributeByIdAsync(int Id);
		Task<IReadOnlyList<Core.Models.Data.Attribute>> GetAllAttributeItemsAsync();

        Task<IReadOnlyList<Core.Models.Data.Attribute>> GetAttributesByAssetTypeIdAsync(int AssetTypeId);
        Task<Result<Core.Models.Data.Attribute>> UpdateAttributeAsync(Models.Data.Attribute item);
		Task<Result<Core.Models.Data.Attribute>> AddAttributeAsync(Models.Data.Attribute item);
		Task<Result<bool>> DeleteAttributeAsync(int Id);
		#endregion


		#region Source
		Task<Result<Core.Models.Data.Source>> GetSourceByIdAsync(int Id);
		Task<IReadOnlyList<Core.Models.Data.Source>> GetAllSourceItemsAsync();
		Task<Result<Core.Models.Data.Source>> UpdateSourceAsync(Models.Data.Source item);
		Task<Result<Core.Models.Data.Source>> AddSourceAsync(Models.Data.Source item);
		Task<Result<bool>> DeleteSourceAsync(int Id);
        #endregion




        #region AssetType
        Task<Result<Core.Models.Data.AssetType>> GetAssetTypeByIdAsync(int Id);
        Task<IReadOnlyList<Core.Models.Data.AssetType>> GetAllAssetTypesAsync();
        Task<Result<Core.Models.Data.AssetType>> UpdateAssetTypeAsync(Models.Data.AssetType item);
        Task<Result<Core.Models.Data.AssetType>> AddAssetTypeAsync(Models.Data.AssetType item);
        Task<Result<bool>> DeleteAssetTypeAsync(int Id);
        #endregion



        #region Organization
        Task<Result<Core.Models.Data.Organization>> GetOrganizationByIdAsync(int Id);
        Task<IReadOnlyList<Core.Models.Data.Organization>> GetAllOrganizationsAsync();
        Task<Result<Core.Models.Data.Organization>> UpdateOrganizationAsync(Models.Data.Organization item);
        Task<Result<Core.Models.Data.Organization>> AddOrganizationAsync(Models.Data.Organization item);
        Task<Result<bool>> DeleteOrganizationAsync(int Id);
        #endregion


        #region Provider
        Task<Result<Core.Models.Data.Provider>> GetProviderByIdAsync(int Id);
        Task<IReadOnlyList<Core.Models.Data.Provider>> GetAllProvidersAsync();
        Task<Result<Core.Models.Data.Provider>> UpdateProviderAsync(Models.Data.Provider item);
        Task<Result<Core.Models.Data.Provider>> AddProviderAsync(Models.Data.Provider item);
        Task<Result<bool>> DeleteProviderAsync(int Id);
        #endregion

    }
}
