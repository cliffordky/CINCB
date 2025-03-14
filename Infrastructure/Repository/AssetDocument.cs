using Ardalis.Result;
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
    public class AssetDocument : IAssetDocument
    {
        private readonly ILogger<AssetDocument> _logger;
        private readonly ISqlConnectionFactory _connectionFactory;

        public AssetDocument(ILogger<Infrastructure.Repository.AssetDocument> logger, ISqlConnectionFactory connection)
        {
            _logger = logger;
            _connectionFactory = connection;
        }

        public async Task<Core.Models.Data.AssetDocument> AddAsync(Core.Models.Data.AssetDocument entity)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.InsertAsync(entity);
            }
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = @"UPDATE [dbo].[AssetDocuments] SET [IsDeleted] = 1 WHERE ([Id] = @id)";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteAsync(sql, new { id }) == 1;
        }

        public async Task<IReadOnlyList<Core.Models.Data.AssetDocument>> GetAllAssetDocumentsByAssetIdAsync(int AssetId)
        {
            var sql = @"SELECT * FROM  [dbo].[AssetDocuments] WHERE ([IsDeleted] = 0) AND ([AssetId] = @AssetId)";
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QueryAsync<Core.Models.Data.AssetDocument>(sql, new { AssetId });
            return result.ToList();
        }

        public async Task<IReadOnlyList<Core.Models.Data.AssetDocument>> GetAllAsync()
        {
            var sql = @"SELECT * FROM  [dbo].[AssetDocuments] WHERE ([IsDeleted] = 0)";
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QueryAsync<Core.Models.Data.AssetDocument>(sql);
            return result.ToList();
        }

        public async Task<Result<Core.Models.Data.AssetDocument>> GetAssetDocumentByPublicKeyAsync(Guid PublicKey)
        {
            var sql = @"SELECT TOP (1) * FROM [dbo].[AssetDocuments] WHERE ([IsDeleted] = 0) AND ([PublicKey] = @PublicKey)";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.AssetDocument>(sql, new { PublicKey });
        }

        public async Task<Core.Models.Data.AssetDocument?> GetByIdAsync(int id)
        {
            var sql = @"SELECT * FROM [dbo].[AssetDocuments] WHERE ([IsDeleted] = 0) AND ([Id] = @id)
                        ";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.AssetDocument>(sql, new { id });
        }

        public async Task<Core.Models.Data.Asset?> GetByPublicKeyAsync(Guid PublicKey)
        {
            var sql = @"SELECT TOP (1) * FROM [dbo].[Assets] WHERE ([IsDeleted] = 0) AND ([PublicKey] = @PublicKey)
                        ";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Core.Models.Data.Asset>(sql, new { PublicKey });
        }

        public async Task<Core.Models.Data.AssetDocument> UpdateAsync(Core.Models.Data.AssetDocument entity)
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