#region using

using Fluxor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

#endregion

namespace BlazorWebAssemblyFluxor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            ConfigureFluxor(builder.Services);

            await builder.Build().RunAsync();
        }

        private static void ConfigureFluxor(IServiceCollection services)
        {
            services.AddFluxor(options => options
            .ScanAssemblies(typeof(Program).Assembly)
            .UseReduxDevTools() // Optional. Needed if Redux Dev Tools will be used. Requires the Fluxor.Blazor.Web.ReduxDevTools NuGet package.
            .UseRouting()
            );
        }
    }
}
