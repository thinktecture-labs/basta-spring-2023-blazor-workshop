using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using WorkshopClient;
using WorkshopClient.Handler;
using WorkshopClient.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CustomAuthorizationHeaderHandler>();
builder.Services.AddScoped<SignalRService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddHttpClient("Workshop.WebApi", client =>
                client.BaseAddress = new Uri("https://localhost:7069/"))
                    .AddHttpMessageHandler<CustomAuthorizationHeaderHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Workshop.WebApi"));
builder.Services.AddMudServices();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Oidc", options.ProviderOptions);
});
await builder.Build().RunAsync();
