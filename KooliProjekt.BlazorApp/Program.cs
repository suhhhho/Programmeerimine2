using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using KooliProjekt.BlazorApp;
using KooliProjekt.BlazorApp.Api;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient to use your API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7136/api/") });

// Register ApiClient
builder.Services.AddScoped<IApiClient, ApiClient>();

await builder.Build().RunAsync();