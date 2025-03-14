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
    public class Organization : Core.Repository.IOrganization
    {
        private readonly ILogger<Organization> _logger;
        private readonly ISqlConnectionFactory _connectionFactory;

        public Organization(ILogger<Infrastructure.Repository.Organization> logger, ISqlConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        public async Task<Core.Models.Data.Organization> AddAsync(Core.Models.Data.Organization entity)
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
            var sql = @"UPDATE [Organizations] SET [IsDeleted] = 1, [ModifiedDate] = GETDATE() WHERE ([Id] = @id) AND ([AllowUserDelete] = 1)";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteAsync(sql, new { id }) == 1;
        }

        public async Task<IReadOnlyList<Core.Models.Data.Organization>> GetAllAsync()
        {
            var sql = @"SELECT * FROM [Organizations] WHERE ([IsDeleted] = 0)";
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QueryAsync<Core.Models.Data.Organization>(sql);
            return result.ToList();
        }

        public async Task<Core.Models.Data.Organization?> GetByIdAsync(int id)
        {
            var sql = @"SELECT * FROM [dbo].[Organizations] WHERE ([IsDeleted] = 0) AND ([Id] = @id)";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.Organization>(sql, new { id });
        }

        public async Task<Core.Models.Data.Organization> UpdateAsync(Core.Models.Data.Organization entity)
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