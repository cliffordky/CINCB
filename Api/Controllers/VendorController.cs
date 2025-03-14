using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Ardalis.Result;

using Ardalis.Result.AspNetCore;
using Api.Models.v1;
using Core;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class VendorController : ControllerBase
    {
        private readonly ILogger<VendorController> _logger;
        private readonly IMeta _meta;
        private readonly IAccess _access;

        public VendorController(ILogger<VendorController> logger, IMeta meta, IAccess access)
        {
            _logger = logger;
            _meta = meta;
            _access = access;
        }

        [TranslateResultToActionResult]
        [HttpPost("contribute")]
        public async Task<Result<Models.v1.ContributionResponse>> Contribute(Models.v1.ContributionRequest request)
        {
            _logger.LogDebug($"Contribute request: {JsonConvert.SerializeObject(request)}");

            try
            {
                var response = new ContributionResponse();

                var orgs = await _meta.GetAllOrganizationsAsync();
                var vendor = orgs.FirstOrDefault(x => x.PublicKey == request.VendorKey);

                if (vendor == null)
                {
                    return Result<Models.v1.ContributionResponse>.NotFound($"Invalid Vendor Key:{request.VendorKey}");
                }

                var consumerKey = Guid.Parse(request.ConsumerIdentifier);
                var consumerResult = await _access.GetUserByPublicKeyAsync(consumerKey);

                if (!consumerResult.IsSuccess)
                {
                    return Result<Models.v1.ContributionResponse>.NotFound($"Invalid Consumer Identifier:{request.ConsumerIdentifier}");
                }

                return response;
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex, Ex.Message);

                return Result<Models.v1.ContributionResponse>.Invalid(new List<ValidationError> {
                    new ValidationError
                    {
                        Identifier = "ERROR",
                        ErrorMessage = $"Invalid email address or password"
                    }
                });
            }
        }
    }
}