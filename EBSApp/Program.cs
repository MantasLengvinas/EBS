using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using EBSApp;
using Serilog;
using EBSApp.Models;
using EBSApp.Auth;
using EBSApp.Services.General;
using EBSAuthenticationHandler.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;

Log.Logger = (Serilog.ILogger)new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    _ = config.ReadFrom.Configuration(builder.Configuration);
});


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddLogging(config => config.AddSerilog());

builder.Services.AddScoped<IApiClient, ApiClient>(config => {

    HttpClient client = new();
    string apiKey = builder.Configuration.GetValue<string>("EBSApiKey");

    client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);

    return new ApiClient(client);

});

builder.Services.AddScoped<TokenStore>();

builder.Services.AddEBSAuthentication(options =>
{
    options.TokenAudience = builder.Configuration.GetValue<string>("Auth:TokenAudience");
    options.TokenIssuer = builder.Configuration.GetValue<string>("Auth:TokenIssuer");
    options.AuthApiUrl = builder.Configuration.GetValue<string>("Auth:AuthApiUri");
    options.TokenExpirationInSeconds = builder.Configuration.GetValue<int>("Auth:TokenExpirationTimeInSeconds");
    options.ApiKey = builder.Configuration.GetValue<string>("Auth:ApiKey");
    options.TokenPublicSigningKey = builder.Configuration.GetValue<string>("Auth:SigningKey");
});

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

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy();

app.MapBlazorHub();
app.MapRazorPages();
app.MapFallbackToPage("/_Host");

app.Run();

