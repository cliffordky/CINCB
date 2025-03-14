using Core;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IAsset customer)
        {
            _logger = logger;
            Customer = customer;
        }

        public IAsset Customer { get; }

        //[HttpGet("GetTransactions/{id}")]
        //public async Task<ActionResult<CustomerDTO>> GetTransactions(int userId)
        //{
        //    ////var customers = Customer.GetCustomersByUserIdAsync(userId).Result.ToList();
        //    ////var items = customers.Select(x => new CustomerDTO()
        //    ////{
        //    ////    Name = x.Name
        //    ////});

        //    //var customer = Customer.GetAssetDetailsByIdAsync(userId).Result;
        //    //return new CustomerDTO()
        //    //{
        //    //    Name = customer.Name
        //    //};

        //    try
        //    {
        //       var customers =  await Customer.GetAllCustomersAsync();
        //        return Ok();
        //    }
        //    catch (Exception Ex)
        //    {

        //        throw;
        //    }
        //    return Ok();
        //}

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        //public class CustomerDTO
        //{
        //    public string Name { get; set; }
        //}
    }
}