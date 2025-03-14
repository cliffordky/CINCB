using Microsoft.Extensions.Logging;
using Quartz;
//using Svc.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Svc.Jobs
{
    [DisallowConcurrentExecution]
    public class QueueTriggerJob : IJob
    {
        private readonly ILogger<QueueTriggerJob> _logger;
        private readonly Core.IQueue _queue;
        private readonly Coravel.Queuing.Interfaces.IQueue _coravelQueue;

        public QueueTriggerJob(ILogger<QueueTriggerJob> logger, Core.IQueue queue, Coravel.Queuing.Interfaces.IQueue coravelQueue)
        {
            _logger = logger;
            _queue = queue;
            _coravelQueue = coravelQueue;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var result = await _queue.GetNextQueueItemAsync();
            if (result.IsSuccess)
            {
                switch ((Core.Enumerations.QueueHandlerType)result.Value.HandlerTypeId)
                {
                    case Core.Enumerations.QueueHandlerType.ANALYZE_PATIENT:
                        _logger.LogDebug("Importing Orders");

                        //_coravelQueue.QueueInvocableWithPayload<AnalyzePatientInvokable, Core.Models.Data.QueueItem>(result.Value);
                        break;

                    //case Core.Enumerations.QueueHandlerType.QUEUE_ZOHO_CONTACT_SYNC:
                    //    _logger.LogDebug("Sync Zoho Contact Orders");

                        //_coravelQueue.QueueInvocableWithPayload<QueueZohoContactSyncInvocable, Core.Models.Data.QueueItem>(result.Value);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
