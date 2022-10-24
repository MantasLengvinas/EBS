﻿using EBSApi.Utility;
using EBSApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Common services
builder.Services.AddSingleton(config =>
{
    string connectionString = builder.Configuration.GetConnectionString("ConnectionString");
    return new SqlUtility(connectionString);
});

builder.Services.AddScoped<IUserQueries, UserQueries>();
builder.Services.AddScoped<IAddressQueries, AddressQueries>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

