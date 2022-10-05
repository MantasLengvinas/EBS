using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using EBSApp;
using Serilog;
using EBSApp.Models;
using EBSApp.Auth;
using EBSApp.Services.General;

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

    return new ApiClient(client);

});

builder.Services.AddScoped<EBSAuthenticationStateProvider>();



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

//app.UseAuthentication();
//app.UseAuthorization();

app.MapBlazorHub();
app.MapRazorPages();
app.MapFallbackToPage("/_Host");

app.Run();

