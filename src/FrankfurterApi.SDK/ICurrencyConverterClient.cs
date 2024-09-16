namespace FrankfurterApi.SDK;

using FrankfurterApi.SDK.Contracts;

public interface ICurrencyConverterClient
{
    Task<Result<CurrencyConverterResponse>> GetLatestExchangeRate(BaseCurrencyForExchangeRate latestExchangeRate, CancellationToken cancellationToken);
    Task<Result<CurrencyConverterResponse>> ConvertCurrency(ConvertCurrency convertCurrency, CancellationToken cancellationToken);
    Task<Result<RetrieveHistoricalRatesResponse>> GetHistoricalRates(RetrieveHistoricalRates retrieveHistoricalRates, CancellationToken cancellationToken);
}