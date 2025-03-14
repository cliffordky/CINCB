using Ardalis.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IQueue
    {
        Task<Result<Core.Models.Data.QueueItem>> GetNextQueueItemAsync();

        Task<Result<Core.Models.Data.QueueItem>> QueueWorkItemAsync(Core.Enumerations.QueueHandlerType handler, dynamic payload, DateTimeOffset? executeDateStamp);
    }
}
