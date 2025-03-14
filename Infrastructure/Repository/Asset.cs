using Ardalis.Result;
using Core;
using Dapper;
using Dapper.FastCrud;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Data;

namespace Infrastructure.Repository
{
	public class Asset : Core.Repository.IAsset
	{
		private readonly ILogger<Asset> _logger;
		private readonly ISqlConnectionFactory _connectionFactory;
        //private readonly IEncryptionService _encryptionService;

        public Asset(ILogger<Infrastructure.Repository.Asset> logger, ISqlConnectionFactory connection
			//, IEncryptionService encryptionService
			)
		{
			_logger = logger;
			_connectionFactory = connection;
            //_encryptionService = encryptionService;

			// Register the custom type handler
			//SqlMapper.AddTypeHandler(new EncryptedTypeHandler(_encryptionService));
		}

        public async Task<Core.Models.Data.Asset?> AddAsync(Core.Models.Data.Asset entity)
		{
			using (var connection = _connectionFactory.CreateConnection())
			{
				await connection.InsertAsync(entity);
			}
			return entity;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var sql = @"UPDATE [dbo].[Assets] SET [IsDeleted] = 1 WHERE ([Id] = @id)";
			using var connection = _connectionFactory.CreateConnection();
			return await connection.ExecuteAsync(sql, new { id }) == 1;
		}

		public async Task<IReadOnlyList<Core.Models.Data.Asset>> GetAllAsync()
		{
			try
			{
				var sql = @"SELECT * FROM  [dbo].[Assets] WHERE ([IsDeleted] = 0)";
				using var connection = _connectionFactory.CreateConnection();
				var result = await connection.QueryAsync<Core.Models.Data.Asset>(sql);
				return result.ToList();
			}
			catch (Exception Ex)
			{
				bool stop = true;
				return null;
			}
		}

		public async Task<Core.Models.Data.Asset?> GetByIdAsync(int id)
		{
			var sql = @"SELECT * FROM [dbo].[Assets] WHERE ([IsDeleted] = 0) AND ([Id] = @id)
                        ";
			using var connection = _connectionFactory.CreateConnection();
			return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.Asset>(sql, new { id });
		}

		public async Task<Core.Models.Data.Asset?> GetByPublicKeyAsync(Guid PublicKey)
		{
			var sql = @"SELECT TOP (1) * FROM [dbo].[Assets] WHERE ([IsDeleted] = 0) AND ([PublicKey] = @PublicKey)
                        ";
			using var connection = _connectionFactory.CreateConnection();
			return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.Asset>(sql, new { PublicKey });
		}

		public async Task<Core.Models.Data.Asset> UpdateAsync(Core.Models.Data.Asset entity)
		{
			try
			{
				entity.ModifiedDate = DateTime.Now;

				_logger.LogDebug("UPDATE Core.Models.Data.Asset {@entity}", entity);
				using var connection = _connectionFactory.CreateConnection();
				await connection.UpdateAsync(entity);
				return entity;
			}
			catch (Exception Ex)
			{
				_logger.LogError(Ex, Ex.Message);
				return null;
			}
		}

		public async Task<Core.Models.Data.Asset?> GetByAssetCodeAsync(string assetCode)
		{
			var sql = @"SELECT TOP (1) * FROM [dbo].[Assets] WHERE ([IsDeleted] = 0) AND ([AssetCode] = @assetCode)
                            ";
			using var connection = _connectionFactory.CreateConnection();
			return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.Asset>(sql, new { assetCode });
		}

		public async Task<IReadOnlyList<Core.Models.Data.Asset>> GetUserAssetsAsync(int userId)
		{
			var sql = @"SELECT C.* FROM [dbo].[Assets] C
                        INNER JOIN [fn_GetAssetCodesforUserId](@UserId) UC ON C.AssetCode = UC.[AssetCode]
                        WHERE C.[IsDeleted] = 0";
			using var connection = _connectionFactory.CreateConnection();
			var result = await connection.QueryAsync<Core.Models.Data.Asset>(sql, new { UserId = userId });
			return result.ToList();
		}

		public async Task<IReadOnlyList<Core.Models.Data.Asset>> GetAllAssetGroupsAsync()
		{
			var sql = @"SELECT DISTINCT [AssetGroup]
                        FROM [C1nCb_m@st3r].[dbo].[Assets]
                        WHERE [IsDeleted] = 0
                        ORDER BY [AssetGroup]";
			using var connection = _connectionFactory.CreateConnection();
			var result = await connection.QueryAsync<Core.Models.Data.Asset>(sql);
			return result.ToList();
		}

        public async Task<Core.Models.Data.Asset> ProvisionAssetByAttributeValueAsync(int AttributeId, string Value)
        {
			using var connection = _connectionFactory.CreateConnection();
			return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.Asset>("usp_ProvisionAssetByAttributeValueAsync", new { AttributeId, Value }, commandType: CommandType.StoredProcedure);
        }

        public async Task<Core.Models.Data.Asset> GetAssetByIdentityAttributeValueAsync(string Value)
        {
            var sql = @"
							SELECT TOP 1 C.*
								FROM [Assets] C
								INNER JOIN [AssetAttributes] CA ON C.[Id] = CA.[AssetId]
								INNER JOIN [Attributes] A ON CA.AttributeId = A.Id AND A.IsIdentifier = 1 AND A.IsDeleted = 0
								WHERE (CA.Value = @Value) AND CA.IsDeleted = 0
						";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.Asset>(sql, new { Value});
        }

        public async Task<IReadOnlyList<Core.Models.Data.Asset>> GetUserAssetsByAttributeFilterAsync(string tsql)
        {
			var sql = @"
						SELECT * FROM [C1nCb_m@st3r].[dbo].[Assets]
						WHERE ([IsDeleted] = 0) AND ([Id] IN (" + tsql + @"))";

            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QueryAsync<Core.Models.Data.Asset>(sql);
            return result.ToList();
        }
    }
}