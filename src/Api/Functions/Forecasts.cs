using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Shared.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api
{
    public class Forecasts
    {     
        [FunctionName("Forecasts")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "forecasts")] HttpRequest req,
            ILogger log)
        {
            try
            {
                if (File.Exists("sample-data/weather.json"))
                {
                    var stream = File.OpenRead("sample-data/weather.json");

                    var data = await JsonSerializer.DeserializeAsync<IEnumerable<WeatherForecast>>(stream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        IgnoreNullValues = false,
                        AllowTrailingCommas = true
                    });

                    if (data == null)
                        return new BadRequestResult();
                    else
                        return new OkObjectResult(data);
                }
                else
                    return new NotFoundResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        } 
    }
}
