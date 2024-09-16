using CurrencyConverter.API.Application;
using CurrencyConverter.API.Configuration;
using CurrencyConverter.Infrastructure.Services;
using FrankfurterApi.SDK;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureHostOptions(x => x.ShutdownTimeout = TimeSpan.FromSeconds(15.0));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IGetExchangeRatesService, GetExchangeRatesService>();
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
});

var configuration = builder.Configuration;
builder.Services.Configure<CurrencyConverterSettings>(configuration.GetSection("CurrencyConverter"));
builder.Services.AddCurrencyConverterApi(configuration);
builder.Services.AddMemoryCache();

var rateLimitingOptions = builder.Configuration.GetSection("RateLimiting").Get<FixedWindowRateLimiterOptions>();

builder.Services.AddRateLimiter(x => x
    .AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        if (rateLimitingOptions != null)
        {
            options.PermitLimit = rateLimitingOptions.PermitLimit;
            options.Window = rateLimitingOptions.Window;
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            options.QueueLimit = rateLimitingOptions.QueueLimit;
        }
        else
        {
            options.PermitLimit = 5;
            options.Window = TimeSpan.Parse("00:00:10");
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            options.QueueLimit = 50;
        }
    }));

var app = builder.Build(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseRateLimiter();

await app.RunAsync();
