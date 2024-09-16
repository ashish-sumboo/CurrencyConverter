using CurrencyConverter.API.ApiContracts;

namespace CurrencyConverter.API.Application;

public interface IGetExchangeRatesService
{
    Task<BuildingBlocks.Result<CurrencyConvertionResponse>> GetLatestExchangeRates(string baseCurrency, CancellationToken cancellationToken);
    Task<BuildingBlocks.Result<CurrencyConvertionResponse>> ConvertCurrency(decimal amount, string fromCurrency, string toCurrency, CancellationToken cancellationToken);
    Task<BuildingBlocks.Result<HistoricalRatesResponse>> GetHistoricalRates(string baseCurrency, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken);
}