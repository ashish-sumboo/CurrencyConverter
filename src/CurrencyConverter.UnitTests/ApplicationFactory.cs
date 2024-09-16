namespace CurrencyConverter.UnitTests;

using CurrencyConverter.API.Application;
using CurrencyConverter.UnitTests.Mocks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.Options;
using CurrencyConverter.API.Configuration;

public sealed class ApplicationFactory
{
    internal readonly ServiceProvider ServiceProvider;

    public ApplicationFactory()
    {
        ServiceProvider = new ServiceCollection()
            .AddScoped<IGetExchangeRatesService, GetExchangeRatesServiceStub>()
            .AddMediatR(typeof(Program))
            .Configure<CurrencyConverterSettings>(options => options.ExcludedCurrencies = [ "TRY", "PLN", "THB", "MXN" ])
            .BuildServiceProvider();
    }
}