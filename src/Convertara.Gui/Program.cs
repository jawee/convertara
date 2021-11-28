using Convertara.Core;
using Convertara.Core.Clients;
using Convertara.Infrastructure.Clients;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var clientId = config["twitch_client_id"];
var clientSecret = config["twitch_client_secret"];
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddTransient<ITwitchService, TwitchService>();
builder.Services.AddTransient<ITwitchClient, TwitchClient>();
// services.AddSingleton<IMyInterface, MyInterface>();
// services.AddSingleton<ICacheProvider>(provider => 
//     RedisCacheProvider("myPrettyLocalhost:6379", provider.GetService<IMyInterface>()));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
