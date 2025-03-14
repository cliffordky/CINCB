using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IQueue : IGenericRepository<Models.Data.QueueItem>
    {
        Task<Core.Models.Data.QueueItem> GetNextQueueItemAsync();
    }
}
