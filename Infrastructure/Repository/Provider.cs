using Core;
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
    public class Provider : Core.Repository.IProvider
    {
        private readonly ILogger<Provider> _logger;
        private readonly ISqlConnectionFactory _connectionFactory;

        public Provider(ILogger<Infrastructure.Repository.Provider> logger, ISqlConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }
        public async Task<Core.Models.Data.Provider> AddAsync(Core.Models.Data.Provider entity)
        {
            try
            {
                entity.CreatedDate = DateTime.Now;
                entity.ModifiedDate = DateTime.Now;
                entity.IsDeleted = false;
                entity.StatusId = 1;

                using var connection = _connectionFactory.CreateConnection();
                await connection.InsertAsync(entity);
                return entity;
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex, Ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = @"UPDATE [Providers] SET [IsDeleted] = 1, [ModifiedDate] = GETDATE() WHERE ([Id] = @id) AND ([AllowUserDelete] = 1)";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteAsync(sql, new { id }) == 1;
        }

        public async Task<IReadOnlyList<Core.Models.Data.Provider>> GetAllAsync()
        {
            var sql = @"SELECT * FROM [Providers] WHERE ([IsDeleted] = 0)";
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QueryAsync<Core.Models.Data.Provider>(sql);
            return result.ToList();
        }

        public async Task<Core.Models.Data.Provider?> GetByIdAsync(int id)
        {
            var sql = @"SELECT * FROM [dbo].[Providers] WHERE ([IsDeleted] = 0) AND ([Id] = @id)";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.Provider>(sql, new { id });
        }

        public async Task<Core.Models.Data.Provider> UpdateAsync(Core.Models.Data.Provider entity)
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
