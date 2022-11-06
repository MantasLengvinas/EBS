using EBSAuthApi.Data;
using EBSAuthApi.Filters;
using EBSAuthApi.Options;
using EBSAuthApi.Services;
using EBSAuthApi.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(config =>
{
    //config.Filters.Add<RequireApiKeyFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(provider =>
{
    // RSA key configuration

    string key = builder.Configuration["Jwt:Key"];

    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
    rsa.ImportRSAPrivateKey(Convert.FromBase64String(key), out _);

    return new RsaSecurityKey(rsa);
});

// Singleton services

builder.Services.AddSingleton<SqlUtility>(config =>
{
    string connectionString = builder.Configuration.GetConnectionString("EBSAuth");
    return new SqlUtility(connectionString);
});

// Scoped services

builder.Services.AddScoped<IJwtGenerator, JwtGenerator>(config =>
{
    return new JwtGenerator(
        config.GetRequiredService<RsaSecurityKey>());
});

builder.Services.Configure<AuthenticationOptions>(
    builder.Configuration.GetSection(AuthenticationOptions.Position));

builder.Services.AddScoped<IAuthenticationQueries, AuthenticationQueries>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>(config => {

    return new AuthenticationService(
            config.GetRequiredService<IOptions<AuthenticationOptions>>(),
            config.GetRequiredService<IJwtGenerator>(),
            config.GetRequiredService<IAuthenticationQueries>()
        );
});

// End of services

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

