using Core;
using Dapper;
using Dapper.FastCrud;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repository
{
	public class Source : Core.Repository.ISource
	{
		private readonly ILogger<Source> _logger;
		private readonly ISqlConnectionFactory _connectionFactory;

		public Source(ILogger<Infrastructure.Repository.Source> logger, ISqlConnectionFactory connection)
		{
			_logger = logger;
			_connectionFactory = connection;
		}

		public async Task<Core.Models.Data.Source> AddAsync(Core.Models.Data.Source entity)
		{
			using (var connection = _connectionFactory.CreateConnection())
			{
				await connection.InsertAsync(entity);
			}
			return entity;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var sql = @"UPDATE [Sources] SET [IsDeleted] = 1, [ModifiedDate] = GETDATE() WHERE ([Id] = @id) AND ([AllowUserDelete] = 1)";
			using var connection = _connectionFactory.CreateConnection();
			return await connection.ExecuteAsync(sql, new { id }) == 1;
		}

		public async Task<IReadOnlyList<Core.Models.Data.Source>> GetAllAsync()
		{
			var sql = @"SELECT * FROM [Sources] WHERE ([IsDeleted] = 0)";
			using var connection = _connectionFactory.CreateConnection();
			var result = await connection.QueryAsync<Core.Models.Data.Source>(sql);
			return result.ToList();
		}

		public async Task<Core.Models.Data.Source?> GetByIdAsync(int id)
		{
			var sql = @"SELECT * FROM [dbo].[Sources] WHERE ([IsDeleted] = 0) AND ([Id] = @id)";
			using var connection = _connectionFactory.CreateConnection();
			return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.Source>(sql, new { id });
		}

		public async Task<Core.Models.Data.Source> UpdateAsync(Core.Models.Data.Source entity)
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