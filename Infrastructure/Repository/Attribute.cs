using Core;
using Dapper;
using Dapper.FastCrud;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repository
{
    public class Attribute : Core.Repository.IAttribute
    {
        private readonly ILogger<Attribute> _logger;
        private readonly ISqlConnectionFactory _connectionFactory;

        public Attribute(ILogger<Infrastructure.Repository.Attribute> logger, ISqlConnectionFactory connection)
        {
            _logger = logger;
            _connectionFactory = connection;
        }

        public async Task<Core.Models.Data.Attribute> AddAsync(Core.Models.Data.Attribute entity)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.InsertAsync(entity);
            }
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = @"UPDATE [Attributes] SET [IsDeleted] = 1, [ModifiedDate] = GETDATE() WHERE ([Id] = @id) AND ([AllowUserDelete] = 1)";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteAsync(sql, new { id }) == 1;
        }

        public async Task<IReadOnlyList<Core.Models.Data.Attribute>> GetAllAsync()
        {
            var sql = @"SELECT * FROM [Attributes] WHERE ([IsDeleted] = 0)";
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QueryAsync<Core.Models.Data.Attribute>(sql);
            return result.ToList();
        }

        public async Task<IReadOnlyList<Core.Models.Data.Attribute>> GetAttributesByAssetTypeIdAsync(int AssetTypeId)
        {
            var sql = @"SELECT * FROM [Attributes] WHERE ([IsDeleted] = 0) AND ([AssetTypeId] = @AssetTypeId)";
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QueryAsync<Core.Models.Data.Attribute>(sql, new { AssetTypeId});
            return result.ToList();
        }

        public async Task<Core.Models.Data.Attribute?> GetByIdAsync(int id)
        {
            var sql = @"SELECT * FROM [dbo].[Attributes] WHERE ([IsDeleted] = 0) AND ([Id] = @id)";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.Attribute>(sql, new { id });
        }

        public async Task<Core.Models.Data.Attribute> UpdateAsync(Core.Models.Data.Attribute entity)
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