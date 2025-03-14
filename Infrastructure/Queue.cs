using Ardalis.Result;
using Core;
using Core.Models.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class Queue : Core.IQueue
    {
        private readonly ILogger<Queue> _logger;
        private readonly IConfiguration _configuration;
        private readonly Core.Repository.IQueue _queueRepository;

        public Queue(ILogger<Infrastructure.Queue> logger, IConfiguration configuration, Core.Repository.IQueue queueRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _queueRepository = queueRepository;
        }

        public async Task<Result<QueueItem>> GetNextQueueItemAsync()
        {
            try
            {
                var result = await _queueRepository.GetNextQueueItemAsync();
                if (result == null)
                {
                    return Result<Core.Models.Data.QueueItem>.Error("No Queue Items to process");
                }

                return result;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.QueueItem>.Error(Ex.Message);
            }
        }

        public async Task<Result<QueueItem>> QueueWorkItemAsync(Core.Enumerations.QueueHandlerType handler, dynamic payload, DateTimeOffset? executeDateStamp)
        {
            try
            {
                if (!executeDateStamp.HasValue)
                    executeDateStamp = DateTimeOffset.Now;

                var result = await _queueRepository.AddAsync(new QueueItem
                {
                    HandlerTypeId = (int)handler,
                    Payload = Newtonsoft.Json.JsonConvert.SerializeObject(payload),
                    ExecuteDateStamp = executeDateStamp.Value
                });

                if (result == null)
                {
                    return Result<Core.Models.Data.QueueItem>.Error("Failed to queue work item");
                }

                return result;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.QueueItem>.Error(Ex.Message);
            }
        }
    }
}
