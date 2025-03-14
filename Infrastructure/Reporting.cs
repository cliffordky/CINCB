using Ardalis.Result;
using Core.Models;
using Core.Models.Data;
using Core.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public partial class Reporting : Core.IReporting
    {
        private readonly ILogger<Reporting> _logger;
        private readonly Core.Repository.IAsset _customerRepository;
        private readonly IAssetAttribute _customerAttributeRepository;
        private readonly IAttribute _attributeRepository;
        private readonly IOrganization _organizationRepository;
        private readonly IProvider _providerRepository;
        private readonly ICohort _cohortRepository;
        private readonly ICohortFilter _cohortFilterRepository;

        public Reporting(ILogger<Infrastructure.Reporting> logger, Core.Repository.IAsset customerRepository, Core.Repository.IAssetAttribute customerAttribute, Core.Repository.IAttribute attribute, Core.Repository.IOrganization organization, Core.Repository.IProvider provider, Core.Repository.ICohort cohort, Core.Repository.ICohortFilter cohortFilter)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _customerAttributeRepository = customerAttribute;
            _attributeRepository = attribute;
            _organizationRepository = organization;
            _providerRepository = provider;
            _cohortRepository = cohort;
            _cohortFilterRepository = cohortFilter;
        }

        public async Task<Result<Patient>> GetPatientDataAsync(string identifier)
        {
            var customer = await _customerRepository.GetCustomerByIdentityAttributeValueAsync(identifier);

            if (customer == null)
            {
                return Result<Patient>.NotFound();
            }

            return await GetPatientDataAsync(customer);
        }

        public async Task<Result<Patient>> GetPatientDataAsync(int Id)
        {
            var customer = await _customerRepository.GetByIdAsync(Id);

            if (customer == null)
            {
                return Result<Patient>.NotFound();
            }

            return await GetPatientDataAsync(customer);
        }

        private async Task<Result<Patient>> GetPatientDataAsync(Core.Models.Data.Asset customer)
        {
            var patient = new Core.Models.Patient();
            //patient.Identifier = identifier;
            patient.FirstName = customer.FirstName;
            patient.LastName = customer.LastName;
            patient.DateOfBirth = customer.DateOfBirth;

            var attributes = await _attributeRepository.GetAllAsync();
            var customerAttributes = await _customerAttributeRepository.GetAllCustomerAttributesByCustomerIdAsync(customer.Id);

            var x = new ExpandoObject() as IDictionary<string, Object>;

            foreach (var item in customerAttributes)
            {
                var attribute = attributes.FirstOrDefault(x => x.Id == item.AttributeId);
                if (attribute == null) continue;
                if (String.IsNullOrEmpty(attribute.NamespacePath)) continue;

                if (!x.ContainsKey(attribute.NamespacePath))
                {
                    x.Add(attribute.NamespacePath, item.Value);
                }
            }

            patient.Properties = x;

            patient.Conditions = getConditions(attributes, customerAttributes);
            patient.Observations = getObservations(attributes, customerAttributes);
            patient.Allergies = getAllergies(attributes, customerAttributes);
            patient.Encounters = await getEncounters(attributes, customerAttributes);
            return Result<Patient>.Success(patient);
        }

        private async Task<List<Patient.Encounter>> getEncounters(IReadOnlyList<Core.Models.Data.Attribute> attributes, IReadOnlyList<AssetAttribute> customerAttributes)
        {
            var organizations = await _organizationRepository.GetAllAsync();
            var providers = await _providerRepository.GetAllAsync();

            List<Core.Models.Patient.Encounter> items = new List<Patient.Encounter>();

            var _attributes = attributes.Where(x => x.DataSetId == (int)Core.Enumerations.DataSet.Encounter).ToList();
            var _attributeIds = _attributes.Select(a => a.Id).ToList();
            List<Core.Models.Data.AssetAttribute> _customerAttributes = customerAttributes.Where(x => _attributeIds.Contains(x.AttributeId)).ToList();
            var dataSets = _customerAttributes.GroupBy(x => x.GroupId).ToList();
            foreach (var dataSet in dataSets)
            {
                var conditionData = _customerAttributes.Where(x => x.GroupId == dataSet.Key).ToList();

                var encounter = new Core.Models.Patient.Encounter();

                Type objectType = typeof(Core.Models.Patient.Encounter);

                foreach (var item in conditionData)
                {
                    var attribute = _attributes.FirstOrDefault(x => x.Id == item.AttributeId);
                    if (string.IsNullOrEmpty(attribute.NamespacePath)) continue;

                    PropertyInfo piInstance = objectType.GetProperty(attribute.NamespacePath);
                    if (piInstance != null)
                    {
                        piInstance.SetValue(encounter, item.Value);
                    }
                }

                var organizationAttribute = _attributes.FirstOrDefault(x => x.NamespacePath != null && x.NamespacePath.Equals("OrganizationIdentifier"));
                if (organizationAttribute != null)
                {
                    var organization = organizations.FirstOrDefault(x => x.Identifier == conditionData.FirstOrDefault(x => x.AttributeId == organizationAttribute.Id).Value);
                    if (organization != null)
                    {
                        encounter.Organization = new Patient.Encounter.OrganizationProvider()
                        {
                            Id = organization.Id,
                            Name = organization.Name
                        };
                    }
                }

                var providerAttribute = _attributes.FirstOrDefault(x => x.NamespacePath != null && x.NamespacePath.Equals("PractitionerIdentifier"));
                if (providerAttribute != null)
                {
                    var provider = organizations.FirstOrDefault(x => x.Identifier == conditionData.FirstOrDefault(x => x.AttributeId == providerAttribute.Id).Value);
                    if (provider != null)
                    {
                        encounter.Provider = new Patient.Encounter.ServiceProvider()
                        {
                            Id = provider.Id,
                            Name = provider.Name
                        };
                    }
                }
                items.Add(encounter);
            }

            return items;
        }

        private static List<Core.Models.Patient.Condition> getConditions(IReadOnlyList<Core.Models.Data.Attribute> attributes, IReadOnlyList<AssetAttribute> customerAttributes)
        {
            List<Core.Models.Patient.Condition> items = new List<Patient.Condition>();

            var conditionAttributes = attributes.Where(x => x.DataSetId == 3).ToList();
            var conditionIds = conditionAttributes.Select(a => a.Id).ToList();
            List<Core.Models.Data.AssetAttribute> patientConditions = customerAttributes.Where(x => conditionIds.Contains(x.AttributeId)).ToList();
            var dataSets = patientConditions.GroupBy(x => x.GroupId).ToList();
            foreach (var dataSet in dataSets)
            {
                var conditionData = patientConditions.Where(x => x.GroupId == dataSet.Key).ToList();

                var condition = new Core.Models.Patient.Condition();

                Type objectType = typeof(Core.Models.Patient.Condition);

                foreach (var item in conditionData)
                {
                    var attribute = conditionAttributes.FirstOrDefault(x => x.Id == item.AttributeId);
                    if (string.IsNullOrEmpty(attribute.NamespacePath)) continue;

                    PropertyInfo piInstance = objectType.GetProperty(attribute.NamespacePath);
                    if (piInstance != null)
                    {
                        piInstance.SetValue(condition, item.Value);
                    }
                }

                items.Add(condition);
            }

            return items;
        }

        private static List<Core.Models.Patient.Allergy> getAllergies(IReadOnlyList<Core.Models.Data.Attribute> attributes, IReadOnlyList<AssetAttribute> customerAttributes)
        {
            List<Core.Models.Patient.Allergy> items = new List<Patient.Allergy>();

            var _attributes = attributes.Where(x => x.DataSetId == 4).ToList();
            var _attributeIds = _attributes.Select(a => a.Id).ToList();
            List<Core.Models.Data.AssetAttribute> _customerAttributes = customerAttributes.Where(x => _attributeIds.Contains(x.AttributeId)).ToList();
            var dataSets = _customerAttributes.GroupBy(x => x.GroupId).ToList();
            foreach (var dataSet in dataSets)
            {
                var conditionData = _customerAttributes.Where(x => x.GroupId == dataSet.Key).ToList();

                var allergy = new Core.Models.Patient.Allergy();

                Type objectType = typeof(Core.Models.Patient.Allergy);

                foreach (var item in conditionData)
                {
                    var attribute = _attributes.FirstOrDefault(x => x.Id == item.AttributeId);
                    if (string.IsNullOrEmpty(attribute.NamespacePath)) continue;

                    PropertyInfo piInstance = objectType.GetProperty(attribute.NamespacePath);
                    if (piInstance != null)
                    {
                        piInstance.SetValue(allergy, item.Value);
                    }
                }

                items.Add(allergy);
            }

            return items;
        }

        private static List<Core.Models.Patient.Observation> getObservations(IReadOnlyList<Core.Models.Data.Attribute> attributes, IReadOnlyList<AssetAttribute> customerAttributes)
        {
            List<Core.Models.Patient.Observation> items = new List<Patient.Observation>();

            var _attributes = attributes.Where(x => x.DataSetId == 6).ToList();
            var _attributeIds = _attributes.Select(a => a.Id).ToList();
            List<Core.Models.Data.AssetAttribute> _customerAttributes = customerAttributes.Where(x => _attributeIds.Contains(x.AttributeId)).ToList();
            var dataSets = _customerAttributes.GroupBy(x => x.GroupId).ToList();
            foreach (var dataSet in dataSets)
            {
                var conditionData = _customerAttributes.Where(x => x.GroupId == dataSet.Key).ToList();

                var observation = new Core.Models.Patient.Observation();

                Type objectType = typeof(Core.Models.Patient.Observation);

                foreach (var item in conditionData)
                {
                    var attribute = _attributes.FirstOrDefault(x => x.Id == item.AttributeId);
                    if (string.IsNullOrEmpty(attribute.NamespacePath)) continue;

                    PropertyInfo piInstance = objectType.GetProperty(attribute.NamespacePath);
                    if (piInstance != null)
                    {
                        piInstance.SetValue(observation, item.Value);
                    }
                }

                items.Add(observation);
            }

            return items;
        }

        public async Task<IReadOnlyList<Patient>> GenerateCohortDataAsync(int id)
        {
            var cohort = await _cohortRepository.GetByIdAsync(id);
            if (cohort == null)
            {
                return new List<Patient>();
            }

            if (cohort.Filters.Count == 0)
            {
                return new List<Patient>();
            }

            int counter = 1;
            StringBuilder sql = new StringBuilder("");
            sql.AppendFormat(" SELECT T{0}.[CustomerId] FROM [CustomerAttributes] T{0} ", counter);

            var firstFilter = cohort.Filters.OrderBy(x => x.Id).FirstOrDefault();

            var inclusiveFilters = cohort.Filters
                .Where(x => x.Id != firstFilter.Id)
                .Where(x => x.ConjunctiveOperator.Equals("OR", StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(x => x.Id).ToList();

            var exclusiveFilters = cohort.Filters
                .Where(x => x.Id != firstFilter.Id)
                .Where(x => x.ConjunctiveOperator.Equals("AND", StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(x => x.Id).ToList();

            foreach (var filter in exclusiveFilters)
            {
                counter++;
                sql.AppendFormat(" INNER JOIN [CustomerAttributes] T{0} ON T1.CustomerId = T{0}.CustomerId AND ", counter);
                sql.AppendFormat(" ((T{0}.[AttributeId] = {1}) AND {2} AND (T{0}.[IsDeleted] = 0))", counter, filter.AttributeId, BuildValueFilter(counter, filter));

             
            }

            sql.AppendFormat(" WHERE ((T{0}.[AttributeId] = {1}) AND {2} AND (T{0}.[IsDeleted] = 0) )", 1, firstFilter.AttributeId, BuildValueFilter(1, firstFilter));
            foreach (var filter in inclusiveFilters)
            {
                sql.AppendLine(" UNION ");
                sql.AppendFormat("SELECT [CustomerId] FROM [O5HC].[dbo].[CustomerAttributes]");
            }

            string where = sql.ToString();

            var customers = await _customerRepository.GetUserCustomersByAttributeFilterAsync(where);

            var patients = new List<Patient>();
            foreach (var item in customers)
            {
                patients.Add(await GetPatientDataAsync(item.Id));
                //patients.Add(new Patient()
                //{
                //    FirstName = item.FirstName,
                //    LastName = item.LastName
                //});
            }

            return patients;
        }

        private static string BuildValueFilter(int counter, CohortFilter? filter)
        {
            string returnValue = string.Empty;
            switch (filter.QueryTypeId)
            {
                case 1:
                    returnValue = string.Format(" (T{0}.[Value] LIKE '%{1}%')", counter, filter.Value);
                    break;

                case 2:
                    returnValue = string.Format(" (T{0}.[Value] LIKE '{1}%')", counter, filter.Value);
                    break;

                case 3:
                    returnValue = string.Format(" (T{0}.[Value] = '{1}')", counter, filter.Value);
                    break;

                case 4:
                    returnValue = string.Format(" (CAST(T{0}.[Value] AS DECIMAL(18, 2)) = {1})", counter, filter.Value);
                    break;

                case 5:
                    returnValue = string.Format(" (CAST(T{0}.[Value] AS DECIMAL(18, 2)) < {1})", counter, filter.Value);
                    break;

                case 6:
                    returnValue = string.Format(" (CAST(T{0}.[Value] AS DECIMAL(18, 2)) > {1})", counter, filter.Value);
                    break;

                case 7:
                    returnValue = string.Format(" (CAST(T{0}.[Value] as datetime) = LEFT(CAST('{1}' as datetime)), 13)", counter, filter.Value);
                    break;

                case 8:
                    returnValue = string.Format(" (CAST(T{0}.[Value] as datetime) < CAST('{1}' as datetime))", counter, filter.Value);
                    break;

                case 9:
                    returnValue = string.Format(" (CAST(T{0}.[Value] as datetime) > CAST('{1}' as datetime))", counter, filter.Value);
                    break;
            }

            return returnValue;
        }
    }
}