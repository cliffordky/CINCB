using Ardalis.Result;
using Core.Models.Data;
using Core.Repository;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
	public partial class Meta : Core.IMeta
	{
		private readonly ILogger<Meta> _logger;
		private readonly ISource _source;
		private readonly IAttribute _attribute;
        private readonly IAssetType _dataSet;
        private readonly IOrganization _organizationRepository;
        private readonly IProvider _providerRepository;
        private readonly Core.Repository.IAsset _assetRepository;

		public Meta(ILogger<Infrastructure.Meta> logger, Core.Repository.ISource source, Core.Repository.IAttribute attribute, Core.Repository.IAssetType dataSet, Core.Repository.IOrganization organization, Core.Repository.IProvider provider)
		{
			_logger = logger;
			_source = source;
			_attribute = attribute;
            _dataSet = dataSet;
            _organizationRepository = organization;
            _providerRepository = provider;
        }


	}
}
