using Ardalis.Result;
using Core;
using Core.Models;
using Core.Repository;
using Dapper;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class Reporting : Core.Repository.IReporting
    {
        private readonly ILogger<Reporting> _logger;
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IAssetAttribute _customerAttributeRepository;
        private readonly IAttribute _attributeRepository;

        public Reporting(ILogger<Infrastructure.Repository.Reporting> logger, ISqlConnectionFactory connectionFactory, IAssetAttribute customerAttribute, IAttribute attribute)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
            _customerAttributeRepository = customerAttribute;
            _attributeRepository = attribute;
        }

        public async Task<Result<Patient>> GetPatientDataAsync(string identifier)
        {
            var sql = @"
							SELECT TOP 1 C.*
								FROM [Customers] C
								INNER JOIN [CustomerAttributes] CA ON C.[Id] = CA.[CustomerId]
								INNER JOIN [Attributes] A ON CA.AttributeId = A.Id AND A.IsIdentifier = 1 AND A.IsDeleted = 0
								WHERE (CA.Value = @identifier) AND CA.IsDeleted = 0
						";

            using var connection = _connectionFactory.CreateConnection();
            var patient = await connection.QueryFirstOrDefaultAsync<Core.Models.Data.Asset>(sql, new { identifier });

            if (patient == null)
            {
                return Result<Patient>.NotFound();
            }

            var attributes = await _attributeRepository.GetAllAsync();
            var customerAttributes = await _customerAttributeRepository.GetAllCustomerAttributesByCustomerIdAsync(patient.Id);

            var x = new ExpandoObject() as IDictionary<string, Object>;

            foreach (var item in customerAttributes)
            {
                var attribute = attributes.FirstOrDefault(x => x.Id == item.AttributeId);
                if (attribute == null) continue;
                if (String.IsNullOrEmpty(attribute.NamespacePath)) continue;

                x.Add(attribute.NamespacePath, item.Value);
            }

            throw new NotImplementedException();
        }
    }
}