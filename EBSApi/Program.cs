using EBSApi.Utility;
using EBSApi.Data;
using EBSApi.Options;
using EBSApi.Services;

var builder = WebApplication.CreateBuilder(args);


string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (env != Environments.Development)
{
    builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
    {
        AppSettingsOptions paths = builder.Configuration
                                    .GetSection(AppSettingsOptions.Position)
                                    .Get<AppSettingsOptions>();

        config.AddJsonFile(paths.Production,
                            optional: false,
                            reloadOnChange: true);
    });
}

// Common services
builder.Services.AddSingleton(config =>
{
    string connectionString = builder.Configuration.GetConnectionString("ConnectionString");
    return new SqlUtility(connectionString);
});

builder.Services.AddScoped<IUserQueries, UserQueries>();
builder.Services.AddScoped<IAddressQueries, AddressQueries>();
builder.Services.AddScoped<IProviderQueries, ProviderQueries>();
builder.Services.AddScoped<IUsageQueries, UsageQueries>();
builder.Services.AddScoped<ITariffQueries, TariffQueries>();
builder.Services.AddScoped<ITariffServices, TariffServices>();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Localhost",
        policy =>
        {
            policy.WithOrigins(builder.Configuration.GetSection("Origins").Get<string>().Split(","));
        });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();

app.Run();

