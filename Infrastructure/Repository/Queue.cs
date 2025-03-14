using Core;
using Core.Models.Data;
using Dapper;
using Dapper.FastCrud;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class Queue : Core.Repository.IQueue
    {
        private readonly ILogger<Queue> _logger;
        private readonly ISqlConnectionFactory _connectionFactory;

        public Queue(ILogger<Infrastructure.Repository.Queue> logger, ISqlConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        public async Task<QueueItem> AddAsync(QueueItem entity)
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

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<QueueItem>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<QueueItem?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Core.Models.Data.QueueItem> GetNextQueueItemAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.QueueItem>("uspGetNextQueueItem", commandType: CommandType.StoredProcedure);
        }

        public Task<QueueItem> UpdateAsync(QueueItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
