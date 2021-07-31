using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var url = builder.HostEnvironment.IsDevelopment() ? "http://localhost:7071" : builder.HostEnvironment.BaseAddress;            

            builder.Services.AddHttpClient(HttpConstants.ApiClientName, c => c.BaseAddress = new Uri(url))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(HttpConstants.ApiClientName));

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add("api://bf4e3d2e-3af8-467f-84cf-1a45383f753c/api");
            });

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            await builder
                .Build()
                .RunAsync();
        }
    }
}
