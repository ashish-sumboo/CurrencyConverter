namespace CurrencyConverter.UnitTests;

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using CurrencyConverter.API.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using CurrencyConverter.API.Configuration;

public sealed class ConvertCurrencyHandlerTests : IClassFixture<ApplicationFactory>
{
    private readonly IGetExchangeRatesService _getExchangeRateService;
    private readonly Dictionary<string, decimal> _rates = new Dictionary<string, decimal>()
            {
                { "AUD", 1.445m },
                { "BGN", 1.773m },
                { "BRL", 5.5779m },
                { "CAD", 1.3574m },
                { "CHF", 0.84752m },
                { "CNY", 7.119m },
                { "CZK", 22.716m },
                { "DKK", 6.7648m },
                { "EUR", 0.90654m },
                { "GBP", 0.76389m }
            };

    private readonly IOptions<CurrencyConverterSettings> _currencyConverterSettings;

    public ConvertCurrencyHandlerTests(ApplicationFactory applicationFactory)
    {
        _getExchangeRateService = applicationFactory.ServiceProvider.GetRequiredService<IGetExchangeRatesService>();
        _currencyConverterSettings = applicationFactory.ServiceProvider.GetRequiredService<IOptions<CurrencyConverterSettings>>();
    }

    [Fact]
    public async Task CurrencyConversionFromUSDToCHFSucceeds()
    {
        var convertCurrencyQuery = new ConvertCurrencyQuery(10, "USD", "CHF");
        var convertCurrencyHandler = new ConvertCurrencyHandler(_getExchangeRateService, _currencyConverterSettings);
        var result = await convertCurrencyHandler.Handle(convertCurrencyQuery, CancellationToken.None);

        var expectedRates = new Dictionary<string, decimal>()
            {
                { "CHF", 8.4752m }
            };

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Amount.Should().Be(1);
        result.Value.Date.Should().Be(new DateOnly(2024, 9, 12));
        result.Value.Rates.Should().BeEquivalentTo(expectedRates);
    }

    [Fact]
    public async Task CurrencyConversionWithEmptyCurrencyFails()
    {
        var convertCurrencyQuery = new ConvertCurrencyQuery(10, "", "");

        var convertCurrencyHandler = new ConvertCurrencyHandler(_getExchangeRateService, _currencyConverterSettings);
        var result = await convertCurrencyHandler.Handle(convertCurrencyQuery, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.ErrorType.Should().Be("invalid_request");
        result.Error.ErrorCodes.Should().Contain("currency_cannot_be_empty");
    }

    [Fact]
    public async Task CurrencyConversionFromNonExistentCurrencyFails()
    {
        var convertCurrencyQuery = new ConvertCurrencyQuery(10, "AAA", "CHF");
        var convertCurrencyHandler = new ConvertCurrencyHandler(_getExchangeRateService, _currencyConverterSettings);
        var result = await convertCurrencyHandler.Handle(convertCurrencyQuery, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.ErrorType.Should().Be("item_not_found");
        result.Error.ErrorCodes.Should().Contain("currency_rate_not_found");
    }

    [Fact]
    public async Task CurrencyConversionFromUSDToUSDFails()
    {
        var convertCurrencyQuery = new ConvertCurrencyQuery(1, "USD", "USD");
        var convertCurrencyHandler = new ConvertCurrencyHandler(_getExchangeRateService, _currencyConverterSettings);
        var result = await convertCurrencyHandler.Handle(convertCurrencyQuery, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.ErrorType.Should().Be("invalid_request");
        result.Error.ErrorCodes.Should().Contain("from_currency_cannot_be_same_as_to_currency");
    }
        
    [Fact]
    public async Task CurrencyConversionForExcludedCurrencyFails()
    {
        var convertCurrencyQuery = new ConvertCurrencyQuery(1, "PLN", "USD");
        var convertCurrencyHandler = new ConvertCurrencyHandler(_getExchangeRateService, _currencyConverterSettings);
        var result = await convertCurrencyHandler.Handle(convertCurrencyQuery, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.ErrorType.Should().Be("invalid_request");
        result.Error.ErrorCodes.Should().Contain("currency_not_allowed");
    }
}