using Ardalis.Result;
using Coravel.Invocable;
using Core;
using Core.Integrations;
using Core.Models;
using Core.Models.Data;
using Hl7.Fhir.Rest;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenAI;
using OpenAI.Assistants;
using OpenAI.Chat;
using OpenAI.Files;
using Svc.Jobs;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Enumerations;

namespace Svc.Handlers
{
    public class AnalyzePatientInvokable : IInvocable, IInvocableWithPayload<Core.Models.Data.QueueItem>
    {
        private readonly ILogger<AnalyzePatientInvokable> _logger;
        private readonly IAsset _customer;
        private readonly IMeta _meta;
        private readonly IAzureFhir _azureFhir;
        private readonly IOpenAI _openAI;
        private readonly IReporting _reporting;

        public Core.Models.Data.QueueItem Payload { get; set; }

        public AnalyzePatientInvokable(ILogger<AnalyzePatientInvokable> logger, Core.IAsset customer, Core.IMeta meta, Core.Integrations.IAzureFhir azureFhir, Core.Integrations.IOpenAI openAI, Core.IReporting reporting)
        {
            _logger = logger;
            _customer = customer;
            _meta = meta;
            _azureFhir = azureFhir;
            _openAI = openAI;
            _reporting = reporting;
        }

        public async Task Invoke()
        {
            try
            {
                //https://www.edandersen.com/file-search-in-azure-openai-service-assistants-v2-api/
                var payload = JsonConvert.DeserializeObject<QueuePayload>(Payload.Payload);
                _logger.LogDebug($"Analyze Patient {payload.PatientIdentifier}");

                var patientResult = await _reporting.GetPatientDataAsync(payload.PatientIdentifier);
                if (patientResult.IsSuccess)
                {
                    var patient = patientResult.Value;

                    StringBuilder prompt = new StringBuilder();
                    prompt.AppendLine("Patient medical information:");

                    patient.Identifier = null;
                    patient.Properties = null;

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(patient, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    var customerResult = await _customer.GetCustomerByIdentityAttributeValueAsync(payload.PatientIdentifier);
                    if (customerResult.IsSuccess)
                    {
                        var results = await _openAI.AnalyzePatientDataAsync(json);

                        foreach (var item in results)
                        {
                            item.CustomerId = customerResult.Value.Id;
                            item.TypeId = 1;
                            await _customer.AddCustomerAnalysisAsync(item);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message);
            }
        }

        private string TryGetConditions(Patient patient)
        {
            StringBuilder conditionText = new StringBuilder();

            foreach (var condition in patient.Conditions)
            {
                conditionText.AppendLine($"{condition.Description} (Loinc:{condition.Loinc})");
            }

            return conditionText.ToString();
        }

        private class QueuePayload
        {
            public string PatientIdentifier { get; set; }
        }
    }
}