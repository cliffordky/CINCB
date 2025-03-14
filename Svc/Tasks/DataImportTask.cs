using Coravel.Invocable;
using Core;
using Core.Models.Configuration;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using Svc.Models;

[assembly: InternalsVisibleTo("Svc.Tests")]
namespace Svc.Tasks
{
    public class DataImportTask : IInvocable
    {
        private readonly ILogger<DataImportTask> _logger;
        private readonly IProduct _product;
        private readonly ICustomer _customer;
        private readonly IOrder _order;
        private readonly IConfiguration _configuration;
        private readonly IOptions<SysproSettings> _sysproSettings;

        public DataImportTask(ILogger<DataImportTask> logger, IOptions<SysproSettings> sysproSettings, IProduct product, ICustomer customer, IOrder order, IConfiguration configuration)
        {
            _logger = logger;
            _product = product;
            _customer = customer;
            _order = order;
            _configuration = configuration;
            _sysproSettings = sysproSettings;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public async Task Invoke()
        {
            try
            {
                _logger.LogInformation($"DataImportTask started at {DateTime.Now}");

                //try
                //{
                //    _logger.LogInformation($"DataImportTask --> ImportOrders --> started at {DateTime.Now}");
                //    await ImportOrders();
                //}
                //catch (Exception Ex)
                //{
                //    _logger.LogError(Ex, Ex.Message);
                //}
                //try
                //{
                //    _logger.LogInformation($"DataImportTask --> ImportProducts --> started at {DateTime.Now}");
                //    await ImportProducts();
                //}
                //catch (Exception Ex)
                //{
                //    _logger.LogError(Ex, Ex.Message);
                //}
                //try
                //{
                //    _logger.LogInformation($"DataImportTask --> ImportPricing --> started at {DateTime.Now}");
                //    await ImportPricing();
                //}
                //catch (Exception Ex)
                //{
                //    _logger.LogError(Ex, Ex.Message);
                //}
                try
                {
                    _logger.LogInformation($"DataImportTask --> ImportCustomers --> started at {DateTime.Now}");
                    await ImportCustomers();
                }
                catch (Exception Ex)
                {
                    _logger.LogError(Ex, Ex.Message);
                }

                try
                {
                    _logger.LogInformation($"DataImportTask --> ImportOrderInvoices --> started at {DateTime.Now}");
                    await ImportOrderInvoices();
                }
                catch (Exception Ex)
                {
                    _logger.LogError(Ex, Ex.Message);
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex, Ex.Message);
            }
        }

        internal protected T? DeserializeXmlFile<T>(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamReader sr = new StreamReader(filePath))
                {
                    return (T?)serializer.Deserialize(sr);
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex, Ex.Message);
                throw;
            }
        }

        internal protected async Task<bool> ImportOrderInvoices()
        {
            try
            {
                string[] files = Directory.GetFiles(_sysproSettings.Value.OrderInvoiceImport.PickupFolder, "*.xml", SearchOption.TopDirectoryOnly);
                foreach (var filepath in files)
                {
                    _logger.LogDebug($"Processing Order Invoice {filepath}");

                    Invoice? invoice = DeserializeXmlFile<Invoice>(filepath);
                    if (invoice == null)
                        continue;

                    
                    var result = await _order.ReconcileOrderAsync(invoice.AsOrder());
                    if (result.IsSuccess)
                    {
                        File.Move(filepath, Path.Combine(_sysproSettings.Value.OrderInvoiceImport.ProcessedFolder, Path.GetFileName(filepath)));
                    }
                }
                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex, Ex.Message);
                return false;
            }
        }

        private async Task ImportOrders()
        {
            try
            {

                var orderFilename = "Sunpac Sales Order Master File.csv";
                var orderFilePath = Path.Combine(_sysproSettings.Value.MasterFileImport.PickupFolder, orderFilename);

                using (var reader = new StreamReader(orderFilePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var orders = csv.GetRecords<SunpacSalesOrderMasterFileItem>();

                    foreach (var order in orders.ToList())
                    {
                        _logger.LogDebug($"Processing order with Sales Order:{order.SalesOrder} --> P/O:{order.CustomerPoNumber}");
                        await _order.ProcessOrderAsync(order.SalesOrder, order.Customer, order.CustomerPoNumber, order.EntrySystemDate, order.OrderStatus);

                    }
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex, Ex.Message);

                await Task.FromException(Ex);
            }
        }

        private async Task ImportProducts()
        {
            _logger.LogDebug("Importing products");

            try
            {
                List<Core.Models.Data.Product> productList = new List<Core.Models.Data.Product>();
                Dictionary<string, int> stockOnHandItems = new Dictionary<string, int>();

                var productMasterFilename = "Sunpac Product Master.csv";
                var productMasterFilePath = Path.Combine(_sysproSettings.Value.MasterFileImport.PickupFolder, productMasterFilename);

                using (var reader = new StreamReader(productMasterFilePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var products = csv.GetRecords<SunpacProductMasterItem>();

                    foreach (var item in products.ToList())
                    {
                        _logger.LogDebug("Adding Core.Models.Data.Product --> {@item}", item);

                        productList.Add(new Core.Models.Data.Product
                        {
                            Brand = item.Brand,
                            StockCode = item.StockCode,
                            Name = item.Description,
                            Department = item.Dept,
                            Description = item.Description,
                            LongDescription = item.LongDesc,
                            Category = item.Category,
                            SubCategory = item.SubCategory,
                            PackSize = item.PackSize
                        });
                    }
                }

                var stockOnHandFilename = "Sunpac SOH.csv";
                var stockOnHandFilePath = Path.Combine(_sysproSettings.Value.MasterFileImport.PickupFolder, stockOnHandFilename);

                using (var reader = new StreamReader(stockOnHandFilePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var stockItems = csv.GetRecords<SunpacStockOnHandItem>();

                    foreach (var item in stockItems.ToList())
                    {
                        _logger.LogDebug($"Adding Stock On Hand {item.StockCode}");
                        stockOnHandItems.Add(item.StockCode, item.SOH);
                    }
                }

                await _product.ImportProductsAsync(productList, stockOnHandItems);

                //var productMasterArchiveFilePath = Path.Combine(workspace, "Archive", productMasterFilename);
                //System.IO.File.Move(productMasterFilePath, productMasterArchiveFilePath);

                //var stockOnHandArchiveFilePath = Path.Combine(workspace, "Archive", stockOnHandFilename);
                //System.IO.File.Move(stockOnHandFilePath, stockOnHandArchiveFilePath);
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex, Ex.Message);

                await Task.FromException(Ex);
            }
        }

        private async Task ImportCustomers()
        {
            _logger.LogDebug("Importing customers");

            try
            {
                var filename = "Sunpac Customer Master.csv";
                var customerFilepath = Path.Combine(_sysproSettings.Value.MasterFileImport.PickupFolder, filename);

                using (var reader = new StreamReader(customerFilepath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var customers = csv.GetRecords<SunpacCustomerMasterItem>();

                    List<Core.Models.Data.Customer> customerList = new List<Core.Models.Data.Customer>();

                    foreach (var item in customers.ToList())
                    {
                        _logger.LogDebug("Adding Core.Models.Data.Customer {@item}", item);

                        var customer = new Core.Models.Data.Customer
                        {
                            Pid = Guid.NewGuid(),
                            CustomerGroup = item.CustomerGroup,
                            CustomerCode = item.Customer,
                            Name = item.Name,
                            Region = item.Region,
                            StoreNumber = item.StoreNumber,
                            CreditLimit = 0,    //  zero by default and will be set later if parsed out of the import file
                            CreditStatusId = Convert.ToInt32(item.CreditStatus),
                            KeyAccountManger = item.KeyAccountManger,
                            MfsMerchandising = item.MfsMerchandising,
                            CustomerGroup1 = item.CustomerGroup1,
                            CustomerGroup2 = item.CustomerGroup2,
                            CustomerGroup3 = item.CustomerGroup3,
                            SalesTeam = item.SalesTeam,
                            EdiSenderCode = item.EdiSenderCode,
                            PriceCode = item.PriceCode
                        };

                        decimal creditLimit;
                        if (Decimal.TryParse(item.CreditLimit, out creditLimit))
                        {
                            customer.CreditLimit = creditLimit;
                        }

                        customerList.Add(customer);
                    }

                    await _customer.ImportCustomersAsync(customerList);
                }

                //var customerArchiveFilePath = Path.Combine(workspace, "Archive", filename);
                //System.IO.File.Move(customerFilepath, customerArchiveFilePath);
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex, Ex.Message);

                await Task.FromException(Ex);
            }
        }

        private async Task ImportPricing()
        {
            try
            {
                var filename = "Sunpac Customer Price File.csv";
                var filePath = Path.Combine(_sysproSettings.Value.MasterFileImport.PickupFolder, filename);

                _logger.LogInformation($"Importing Pricing from {filePath}");

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var prices = csv.GetRecords<SunpacCustomerPriceFileItem>();

                    List<Core.Models.Data.ProductPrice> productPrices = new List<Core.Models.Data.ProductPrice>();

                    foreach (var item in prices.ToList())
                    {
                        _logger.LogDebug("Adding Core.Models.Data.ProductPrice {@item}", item);
                        productPrices.Add(new Core.Models.Data.ProductPrice
                        {
                            StockCode = item.StockCode,
                            PriceCode = item.PriceCode,
                            CustomerGroup = item.CustomerGroup,
                            SellingPrice = item.SellingPrice
                        });
                    }

                    await _product.ImportProductPricesAsync(productPrices);
                }

                //var archiveFilePath = Path.Combine(workspace, "Archive", filename);
                //System.IO.File.Move(filePath, archiveFilePath);
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex, Ex.Message);

                await Task.FromException(Ex);
            }
        }

        private class SunpacProductMasterItem
        {
            public string Brand { get; set; }
            public string StockCode { get; set; }
            public string Description { get; set; }

            public string LongDesc { get; set; }
            public string Dept { get; set; }
            public string Category { get; set; }
            public string SubCategory { get; set; }
            public string ProductClassificat { get; set; }
            public string Franchise { get; set; }
            public string RangeType { get; set; }

            public int PackSize { get; set; }
        }

        private class SunpacCustomerPriceFileItem
        {
            public string StockCode { get; set; }
            public string PriceCode { get; set; }
            public string CustomerGroup { get; set; }
            public decimal SellingPrice { get; set; }
        }


        private class SunpacSalesOrderMasterFileItem
        {
            public string SalesOrder { get; set; }
            public string OrderStatus { get; set; }
            public string Customer { get; set; }
            public string CustomerPoNumber { get; set; }

            public DateTime EntrySystemDate { get; set; }
        }


        private class SunpacStockOnHandItem
        {
            public string StockCode { get; set; }
            public int SOH { get; set; }
        }

        private class SunpacCustomerMasterItem
        {
            public string CustomerGroup { get; set; }
            public string Customer { get; set; }
            public string Name { get; set; }
            public string Region { get; set; }
            public string StoreNumber { get; set; }
            public string CreditLimit { get; set; }
            public string? CreditStatus { get; set; }
            public string? KeyAccountManger { get; set; }
            public string? MfsMerchandising { get; set; }
            public string? CustomerGroup1 { get; set; }
            public string? CustomerGroup2 { get; set; }
            public string? CustomerGroup3 { get; set; }
            public string? SalesTeam { get; set; }

            public string? EdiSenderCode { get; set; }
            public string? PriceCode { get; set; }
        }
    }
}