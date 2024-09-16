namespace CurrencyConverter.UnitTests;

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using CurrencyConverter.API.Application;
using Microsoft.Extensions.DependencyInjection;

public sealed class GetLatestCurrencyRateHandlerTests : IClassFixture<ApplicationFactory>
{
    private readonly IGetExchangeRatesService _getExchangeRateService;
    private readonly Dictionary<string, decimal> _expectedRates = new Dictionary<string, decimal>()
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

    public GetLatestCurrencyRateHandlerTests(ApplicationFactory applicationFactory)
    {
        _getExchangeRateService = applicationFactory.ServiceProvider.GetRequiredService<IGetExchangeRatesService>();
    }
    
    [Fact]
    public async Task RetrieveLatestExchangeRateForBaseCurrencyUSDSucceeds()
    {
        var getLatestRateQuery = new GetLatestRateQuery("USD");
        var getCurrencyRateHandler = new GetLatestCurrencyRateHandler(_getExchangeRateService);
        var result = await getCurrencyRateHandler.Handle(getLatestRateQuery, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Amount.Should().Be(1);
        result.Value.Date.Should().Be(new DateOnly(2024, 9,12));
        result.Value.Rates.Should().BeEquivalentTo(_expectedRates);
    }

    [Fact]
    public async Task RetrieveLatestExchangeRateForNonExistentBaseCurrencyFails()
    {
        var getLatestRateQuery = new GetLatestRateQuery("ZZZ");
        var getCurrencyRateHandler = new GetLatestCurrencyRateHandler(_getExchangeRateService);
        var result = await getCurrencyRateHandler.Handle(getLatestRateQuery, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.ErrorType.Should().Be("item_not_found");
        result.Error.ErrorCodes.Should().Contain("currency_rate_not_found");
    }
}