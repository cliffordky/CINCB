using Core;
using Dapper;
using Dapper.FastCrud;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Data;

namespace Infrastructure.Repository
{
	public class AssetAttribute : Core.Repository.IAssetAttribute
	{
		private readonly ILogger<AssetAttribute> _logger;
		private readonly ISqlConnectionFactory _connectionFactory;

		public AssetAttribute(ILogger<Infrastructure.Repository.AssetAttribute> logger, ISqlConnectionFactory connection)
		{
			_logger = logger;
			_connectionFactory = connection;
		}

		public async Task<Core.Models.Data.AssetAttribute> AddAsync(Core.Models.Data.AssetAttribute entity)
		{
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.AssetAttribute>("usp_AddAssetAttribute", 
				new { 
					AssetId = entity.AssetId,
					AttributeId = entity.AttributeId, 
					Value = entity.Value,
                    GroupId = entity.GroupId,
					SourceId = entity.SourceId,
					ModifiedUser = entity.ModifiedUser,
					CreatedDate = entity.CreatedDate,
					DueDate = entity.DueDate
                }, commandType: CommandType.StoredProcedure);
        }


        public async Task<bool> DeleteAsync(int id)
		{
			var sql = @"UPDATE [AssetAttributes] SET [IsDeleted] = 1, [ModifiedDate] = GETDATE() WHERE ([Id] = @id) AND ([AllowUserDelete] = 1)";
			using var connection = _connectionFactory.CreateConnection();
			return await connection.ExecuteAsync(sql, new { id }) == 1;
		}

		public async Task<IReadOnlyList<Core.Models.Data.AssetAttribute>> GetAllAsync()
		{
			var sql = @"SELECT * FROM [AssetAttributes] WHERE ([IsDeleted] = 0)";
			using var connection = _connectionFactory.CreateConnection();
			var result = await connection.QueryAsync<Core.Models.Data.AssetAttribute>(sql);
			return result.ToList();
		}

        public async Task<IReadOnlyList<Core.Models.Data.AssetAttribute>> GetAllAssetAttributesByAssetIdAsync(int Id)
        {
            var sql = @"SELECT * FROM [AssetAttributes] WHERE ([IsDeleted] = 0) AND ([AssetId] = @Id)";
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QueryAsync<Core.Models.Data.AssetAttribute>(sql, new { Id });
            return result.ToList();
        }


        public async Task<IReadOnlyList<Core.Models.Data.AssetAttribute>> GetAllAssetAttributesByAttributeIdAsync(int AttributeId)
        {
            var sql = @"SELECT * FROM [AssetAttributes] WHERE ([IsDeleted] = 0) AND ([AttributeId] = @AttributeId)";
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QueryAsync<Core.Models.Data.AssetAttribute>(sql, new { AttributeId });
            return result.ToList();
        }
        public async Task<Core.Models.Data.AssetAttribute?> GetByIdAsync(int id)
		{
			var sql = @"SELECT * FROM [dbo].[AssetAttributes] WHERE ([IsDeleted] = 0) AND ([Id] = @id)";
			using var connection = _connectionFactory.CreateConnection();
			return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.AssetAttribute>(sql, new { id });
		}

		public async Task<Core.Models.Data.AssetAttribute> UpdateAsync(Core.Models.Data.AssetAttribute entity)
		{
			try
			{
				entity.ModifiedDate = DateTime.Now;
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
	}
}