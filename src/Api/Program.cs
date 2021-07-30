using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(c => 
                {
                    c.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureFunctionsWorkerDefaults(b => 
                { })
                .ConfigureServices((ctx, services) =>
                {
                    services.AddLogging(o => 
                    {
                        o.AddConsole();
                        o.AddDebug();
                        o.AddEventLog();
                        o.AddEventSourceLogger();
                    });         

                })
                .Build();

            host.Run();
        }
    }
}