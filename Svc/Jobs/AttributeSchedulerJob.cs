using Core;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cronos;
using OfficeOpenXml.FormulaParsing.Utilities;
using System.Web;

namespace Svc.Jobs
{
    [DisallowConcurrentExecution]
    public class AttributeSchedulerJob : IJob
    {
        private readonly ILogger<AttributeSchedulerJob> _logger;
        private readonly IAsset _asset;
        private readonly IMeta _meta;
        private readonly IEmailService _emailService;

        public AttributeSchedulerJob(ILogger<AttributeSchedulerJob> logger, Core.IAsset asset, Core.IMeta meta, Core.IEmailService emailService)
        {
            _logger = logger;
            _asset = asset;
            _meta = meta;
            _emailService = emailService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _emailService.SendTemplateEmail("cliff.redfern@gmail.com");

            var attributes = (await _meta.GetAllAttributeItemsAsync()).Where(x => !String.IsNullOrEmpty(x.CronExpression)).ToList();

            foreach (var attribute in attributes)
            {
                Cronos.CronExpression expression = Cronos.CronExpression.Parse(attribute.CronExpression);

                DateTime? nextUtc = expression.GetNextOccurrence(DateTime.UtcNow);

                var assetAttributes = (await _asset.GetAllAssetAttributesByAttributeIdAsync(attribute.Id)).ToList();    //   a list of all assets that have this attribute associated with them

                var assetIds = assetAttributes.Select(x => x.AssetId).Distinct().ToList();    //   a list of all distinct assets

                foreach (var assetId in assetIds)
                {
                    if ((assetAttributes.Count(x => x.AssetId == assetId && x.DueDate.HasValue && x.DueDate >= nextUtc)) > 0) continue;    //   if there is an asset that has a due date greater than the next due date, skip it

                    await _asset.AddAssetAttributeAsync(new Core.Models.Data.AssetAttribute
                    {
                        CreatedDate = DateTime.Now,
                        AssetId = assetId,
                        SourceId = 3,
                        GroupId = null,
                        AttributeId = attribute.Id,
                        DueDate = nextUtc
                    });

                
                }
            }
        }
    }
}