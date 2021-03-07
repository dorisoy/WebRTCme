using Blazored.Modal;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebRTCme.DemoApp.Blazor.Extensions;
using WebRTCme.DemoApp.Blazor.Services;
using WebRTCme.Middleware;

namespace WebRTCme.DemoApp.Blazor.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            ConfigureServices(builder.Services);

            builder.Services.AddScoped(sp => new HttpClient 
            { 
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
            });

            _ = CrossWebRtcMiddlewareBlazor.Current;

            builder.Services.AddAppServices();

            var host = builder.Build();
            ConfigureProviders(host.Services);
            await host.RunAsync();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
        }

        // To ignore null in JSInterop JSON globally.
        public static void ConfigureProviders(IServiceProvider services)
        {
            try
            {
                var jsRuntime = services.GetService<IJSRuntime>();
                var prop = typeof(JSRuntime).GetProperty("JsonSerializerOptions", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                JsonSerializerOptions value = (JsonSerializerOptions)Convert.ChangeType(
                    prop.GetValue(jsRuntime, null), typeof(JsonSerializerOptions));
                value.IgnoreNullValues = true;
                value.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex}");
            }
        }
    }
}
