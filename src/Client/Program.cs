using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using System;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient(HttpConstants.ApiClientName, c
                => c.BaseAddress = new Uri(builder.HostEnvironment.IsDevelopment() ? "http://localhost:7071" : builder.HostEnvironment.BaseAddress));

            builder.Services.AddHttpClient(HttpConstants.ApiClientName, c
                => c.BaseAddress = new Uri(builder.HostEnvironment.IsDevelopment() ? "http://localhost:7071" : builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.LoginMode = "redirect";
            });

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            await builder
                .Build()
                .RunAsync();
        }
    }
}
