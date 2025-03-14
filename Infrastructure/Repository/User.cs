using Core;
using Core.Repository;
using Dapper;
using Dapper.FastCrud;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repository
{
    public class User : IUser
    {
        private readonly ILogger<Infrastructure.Repository.User> _logger;
        private readonly ISqlConnectionFactory _connectionFactory;

        public User(ILogger<Infrastructure.Repository.User> logger, ISqlConnectionFactory connection)
        {
            _logger = logger;
            _connectionFactory = connection;
        }

        public Task<Core.Models.Data.User> AddAsync(Core.Models.Data.User entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Core.Models.Data.User>> GetAllAsync()
        {
            var sql = @"SELECT * FROM [AspNetUsers] WHERE ([IsDeleted] = 0)";
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QueryAsync<Core.Models.Data.User>(sql);
            return result.ToList();
        }

        public async Task<Core.Models.Data.User?> GetByIdAsync(int id)
        {
            var sql = @"SELECT TOP (1) *
                      FROM [C1nCb_m@st3r].[dbo].[AspNetUsers]
                      WHERE ([IsDeleted] = 0) AND ([Id] = @Id)";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.User>(sql, new { id });
        }

        public async Task<Core.Models.Data.User> GetUserByPublicKeyAsync(Guid PublicKey)
        {
            var sql = @"SELECT TOP (1) * FROM [dbo].[AspNetUsers] WHERE ([IsDeleted] = 0) AND ([PublicKey] = @PublicKey)";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.User>(sql, new { PublicKey });
        }

        public async Task<Core.Models.Data.User> UpdateAsync(Core.Models.Data.User entity)
        {
            try
            {
                var sql = @"
                    UPDATE [AspNetUsers]
                       SET [PhoneNumber] = @PhoneNumber,
                          [FirstName] = @FirstName,
                          [LastName] = @LastName,
                          [ImageSlug] = @ImageSlug
                     WHERE ([Id] = @Id);

                    SELECT TOP 1 * FROM [AspNetUsers] WHERE ([Id] = @Id)
                    ";

                using var connection = _connectionFactory.CreateConnection();
                await connection.QueryAsync<Core.Models.Data.User>(sql, new
                {
                    Id = entity.Id,
                    PhoneNumber = entity.PhoneNumber,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    ImageSlug = entity.ImageSlug
                });

                return entity;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

 
    }
}