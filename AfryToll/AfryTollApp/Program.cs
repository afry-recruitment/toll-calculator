using AfryTollApp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AfryTollApp.Services;
using AfryTollApp.Services.Contracts;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
//builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7005/") });
builder.Services.AddScoped<ITollService, TollService>();
builder.Services.AddScoped<ICategoryService, Categoryservice>();
builder.Services.AddScoped<ITollCostService, TollCostService>();
builder.Services.AddScoped<IUserService, UserService>();
await builder.Build().RunAsync();
