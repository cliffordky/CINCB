using Core;
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
    public class MessageTemplate : IMessageTemplate
    {
        private readonly ILogger<MessageTemplate> _logger;
        private readonly ISqlConnectionFactory _connectionFactory;

        public MessageTemplate(ILogger<Infrastructure.Repository.MessageTemplate> logger, ISqlConnectionFactory connection)
        {
            _logger = logger;
            _connectionFactory = connection;
        }

        public async Task<Core.Models.Data.MessageTemplate> AddAsync(Core.Models.Data.MessageTemplate entity)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.InsertAsync(entity);
            }
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = @"UPDATE [dbo].[MessageTemplates] SET [IsDeleted] = 1 WHERE ([Id] = @id)";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteAsync(sql, new { id }) == 1;
        }

        public async Task<IReadOnlyList<Core.Models.Data.MessageTemplate>> GetAllAsync()
        {
            var sql = @"SELECT * FROM  [dbo].[MessageTemplates] WHERE ([IsDeleted] = 0)";
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QueryAsync<Core.Models.Data.MessageTemplate>(sql);
            return result.ToList();
        }

        public async Task<Core.Models.Data.MessageTemplate?> GetByIdAsync(int id)
        {
            var sql = @"SELECT * FROM [dbo].[MessageTemplates] WHERE ([IsDeleted] = 0) AND ([Id] = @id)";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.MessageTemplate>(sql, new { id });
        }

        public async Task<Core.Models.Data.MessageTemplate> UpdateAsync(Core.Models.Data.MessageTemplate entity)
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
                return null;
            }
        }
    }
}