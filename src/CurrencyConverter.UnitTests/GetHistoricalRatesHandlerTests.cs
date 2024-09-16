namespace CurrencyConverter.UnitTests;

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using CurrencyConverter.API.Application;
using Microsoft.Extensions.DependencyInjection;

public sealed class GetHistoricalRatesHandlerTests : IClassFixture<ApplicationFactory>
{
    private readonly IGetExchangeRatesService _getExchangeRateService;

    public GetHistoricalRatesHandlerTests(ApplicationFactory applicationFactory)
    {
        _getExchangeRateService = applicationFactory.ServiceProvider.GetRequiredService<IGetExchangeRatesService>();
    }

    [Fact]
    public async Task GetHistoricalExchangeRatesForBaseCurrencyUSDSucceeds()
    {
        Dictionary<DateOnly, Dictionary<string, decimal>> _expectedHistoricalRates = new Dictionary<DateOnly, Dictionary<string, decimal>>()
            {
                { new DateOnly(2024, 9, 13), new Dictionary<string, decimal>() 
                    { 
                        { "AUD", 1.445m },
                        { "BGN", 1.773m },
                        { "BRL", 5.5779m },
                        { "CAD", 1.3574m },
                        { "CHF", 0.84752m }
                    }
                }
            };
        
        var getHistoricalRatesQuery = new GetHistoricalRatesQuery("USD", new DateOnly(2024, 09, 13), new DateOnly(2024, 09, 15));
        var getHistoricalRatesHandler = new GetHistoricalRatesHandler(_getExchangeRateService);
        var result = await getHistoricalRatesHandler.Handle(getHistoricalRatesQuery, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Amount.Should().Be(1);
        result.Value.StartDate.Should().Be(new DateOnly(2024, 9, 13));
        result.Value.EndDate.Should().Be(new DateOnly(2024, 9, 15));
        result.Value.Rates.Should().BeEquivalentTo(_expectedHistoricalRates);
    }

    [Fact]
    public async Task GetHistoricalExchangeRatesForBaseCurrencyUSDWithPageSize10Succeeds()
    {
        Dictionary<DateOnly, Dictionary<string, decimal>> _expectedHistoricalRates = new Dictionary<DateOnly, Dictionary<string, decimal>>()
            {
                { new DateOnly(2024, 9, 13), new Dictionary<string, decimal>() 
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
                    }
                }
            };
        
        var getHistoricalRatesQuery = new GetHistoricalRatesQuery("USD", new DateOnly(2024, 09, 13), new DateOnly(2024, 09, 15), 1, 10);
        var getHistoricalRatesHandler = new GetHistoricalRatesHandler(_getExchangeRateService);
        var result = await getHistoricalRatesHandler.Handle(getHistoricalRatesQuery, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Amount.Should().Be(1);
        result.Value.StartDate.Should().Be(new DateOnly(2024, 9, 13));
        result.Value.EndDate.Should().Be(new DateOnly(2024, 9, 15));
        result.Value.Rates.Should().BeEquivalentTo(_expectedHistoricalRates);
    }

    [Fact]
    public async Task GetHistoricalExchangeRatesForBaseCurrencyUSDForPage2WithPageSize5Succeeds()
    {
        Dictionary<DateOnly, Dictionary<string, decimal>> _expectedHistoricalRates = new Dictionary<DateOnly, Dictionary<string, decimal>>()
            {
                { new DateOnly(2024, 9, 13), new Dictionary<string, decimal>() 
                    {
                        { "CNY", 7.119m },
                        { "CZK", 22.716m },
                        { "DKK", 6.7648m },
                        { "EUR", 0.90654m },
                        { "GBP", 0.76389m }
                    }
                }
            };
        
        var getHistoricalRatesQuery = new GetHistoricalRatesQuery("USD", new DateOnly(2024, 09, 13), new DateOnly(2024, 09, 15), 2, 5);
        var getHistoricalRatesHandler = new GetHistoricalRatesHandler(_getExchangeRateService);
        var result = await getHistoricalRatesHandler.Handle(getHistoricalRatesQuery, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Amount.Should().Be(1);
        result.Value.StartDate.Should().Be(new DateOnly(2024, 9, 13));
        result.Value.EndDate.Should().Be(new DateOnly(2024, 9, 15));
        result.Value.Rates.Should().BeEquivalentTo(_expectedHistoricalRates);
    }

    [Fact]
    public async Task RetrieveLatestExchangeRateForNonExistentBaseCurrencyFails()
    {
        var getHistoricalRatesQuery = new GetHistoricalRatesQuery("EUR", new DateOnly(2024, 09, 13), new DateOnly(2024, 09, 15));
        var getHistoricalRatesHandler = new GetHistoricalRatesHandler(_getExchangeRateService);
        var result = await getHistoricalRatesHandler.Handle(getHistoricalRatesQuery, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.ErrorType.Should().Be("item_not_found");
        result.Error.ErrorCodes.Should().Contain("currency_rate_not_found");
    }
}