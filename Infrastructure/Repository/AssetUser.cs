using Core;
using Core.Models.Data;
using Core.Repository;
using Dapper;
using Dapper.FastCrud;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class AssetUser : IAssetUser
    {
        private readonly ILogger<AssetUser> _logger;
        private readonly ISqlConnectionFactory _connectionFactory;

        public AssetUser(ILogger<Infrastructure.Repository.AssetUser> logger, ISqlConnectionFactory connection)
        {
            _logger = logger;
            _connectionFactory = connection;
        }

        public async Task<Core.Models.Data.AssetUser> AddAsync(Core.Models.Data.AssetUser entity)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.InsertAsync(entity);
            }
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = @"UPDATE [dbo].[AssetUsers] SET [IsDeleted] = 1 WHERE ([Id] = @id)";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteAsync(sql, new { id }) == 1;
        }

        public async Task<IReadOnlyList<Core.Models.Data.AssetUser>> GetAllAsync()
        {
            var sql = @"SELECT * FROM  [dbo].[AssetUsers] WHERE ([IsDeleted] = 0)";
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QueryAsync<Core.Models.Data.AssetUser>(sql);
            return result.ToList();
        }

        public async Task<List<Core.Models.Data.AssetUser>> GetAssetsForUserAsync(int UserId)
        {
            var sql = @"SELECT * FROM  [dbo].[AssetUsers] WHERE ([IsDeleted] = 0) AND ([UserId] = @UserId)";
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QueryAsync<Core.Models.Data.AssetUser>(sql, new { UserId });
            return result.ToList();
        }

        public async Task<Core.Models.Data.AssetUser?> GetByIdAsync(int id)
        {
            var sql = @"SELECT * FROM [dbo].[AssetUsers] WHERE ([IsDeleted] = 0) AND ([Id] = @id)";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.AssetUser>(sql, new { id });
        }

        public async Task<List<Core.Models.Data.AssetUser>> GetUsersForAssetAsync(int AssetId)
        {
            var sql = @"SELECT * FROM  [dbo].[AssetUsers] WHERE ([IsDeleted] = 0) AND ([AssetId] = @AssetId)";
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QueryAsync<Core.Models.Data.AssetUser>(sql, new { AssetId });
            return result.ToList();
        }

        public async Task<Core.Models.Data.AssetUser> UpdateAsync(Core.Models.Data.AssetUser entity)
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
