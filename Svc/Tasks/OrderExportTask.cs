using Coravel.Invocable;
using Core.Models.Configuration;
using Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svc.Tasks
{
    public class OrderExportTask : IInvocable
    {
        private readonly ILogger<OrderExportTask> _logger;
        private readonly IProduct _product;
        private readonly ICustomer _customer;
        private readonly IOrder _order;
        private readonly IOptions<SysproSettings> _sysproSettings;

        public OrderExportTask(ILogger<OrderExportTask> logger, IOptions<SysproSettings> sysproSettings, IProduct product, ICustomer customer, IOrder order)
        {
            _logger = logger;
            _product = product;
            _customer = customer;
            _order = order;
            _sysproSettings = sysproSettings;
        }

        public async Task Invoke()
        {
            var orders = await _order.GetOrdersByStatusAsync(Enumerations.OrderStatus.Finalised);
            foreach (var order in orders)
            {
                bool result = await _order.ExportOrdersAsync(_sysproSettings.Value.OrderExport.PickupFolder, order, _sysproSettings.Value.OrderExport.SellerGLNumber);
                if (result)
                {
                    await _order.UpdateOrderStatusAsync(order.Id, Enumerations.OrderStatus.OrderFileUploaded);
                }
            }
        }
    }
}