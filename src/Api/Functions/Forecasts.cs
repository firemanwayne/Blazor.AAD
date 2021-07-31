using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Functions
{
    public static class Forecasts
    {
        [Function("Forecasts")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var tenantId = (string)req.Query["tenantId"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonSerializer.Deserialize<string>(requestBody);
            tenantId ??= data?.name;

            string responseMessage = string.IsNullOrEmpty(tenantId)
                ? "This HTTP triggered function executed successfully. Pass a tenantId in the query string or in the request body for a personalized response."
                : $"Hello, {tenantId}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
