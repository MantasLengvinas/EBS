using EBSAuthApi.Options;
using EBSAuthApi.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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


// Scoped services

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"]
    };

});

builder.Services.AddScoped<IJwtGenerator, JwtGenerator>(config =>
{
    return new JwtGenerator(
        config.GetRequiredService<RsaSecurityKey>());
});

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>(config => {

    return new AuthenticationService(
            config.GetRequiredService<IOptions<AuthenticationOptions>>(),
            config.GetRequiredService<IJwtGenerator>()
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

