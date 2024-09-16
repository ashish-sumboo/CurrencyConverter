namespace CurrencyConverter.UnitTests.Mocks;

using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks;
using CurrencyConverter.API.ApiContracts;
using CurrencyConverter.API.Application;

public sealed class GetExchangeRatesServiceStub : IGetExchangeRatesService
{
    private readonly ConcurrentDictionary<string, CurrencyConvertionResponse> _exchangeRates = new();
    private readonly ConcurrentDictionary<string, HistoricalRatesResponse> _historicalRates = new();

    public GetExchangeRatesServiceStub()
    {
        _exchangeRates.TryAdd("USD", new CurrencyConvertionResponse
        {
            Amount = 1,
            Base = "USD",
            Date = new DateOnly(2024, 9, 12),
            Rates = new Dictionary<string, decimal>()
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
        });

        _historicalRates.TryAdd("USD", new HistoricalRatesResponse
        {
            Amount = 1,
            Base = "USD",
            StartDate = new DateOnly(2024, 9, 13),
            EndDate = new DateOnly(2024, 9, 15),
            Rates = new Dictionary<DateOnly, Dictionary<string, decimal>>()
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
            }
        });
    }

    public Task<Result<CurrencyConvertionResponse>> GetLatestExchangeRates(string baseCurrency, CancellationToken cancellationToken)
    {
        _exchangeRates.TryGetValue(baseCurrency, out var exchangeRate);

        if (exchangeRate != null)
        {
            return Task.FromResult(Result.Ok(exchangeRate));
        }

        return Task.FromResult(Result.Fail<CurrencyConvertionResponse>(Error.Invalid()));
    }

    public Task<Result<CurrencyConvertionResponse>> ConvertCurrency(decimal amount, string fromCurrency, string toCurrency, CancellationToken cancellationToken)
    {
        _exchangeRates.TryGetValue(fromCurrency, out var exchangeRate);

        if (exchangeRate != null)
        {
            var rateKeyValuePair = exchangeRate.Rates.Where(x => x.Key == toCurrency).FirstOrDefault();

            exchangeRate.Rates = new Dictionary<string, decimal>()
            {
                { rateKeyValuePair.Key, rateKeyValuePair.Value * amount }
            };
            
            return Task.FromResult(Result.Ok(exchangeRate));
        }

        return Task.FromResult(Result.Fail<CurrencyConvertionResponse>(Error.Invalid()));
    }

    public Task<Result<HistoricalRatesResponse>> GetHistoricalRates(string baseCurrency, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
    {
        _historicalRates.TryGetValue(baseCurrency, out var historicalExchangeRate);

        if (historicalExchangeRate != null)
        {
            return Task.FromResult(Result.Ok(historicalExchangeRate));
        }

        return Task.FromResult(Result.Fail<HistoricalRatesResponse>(Error.Invalid()));
    }
}