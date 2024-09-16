namespace FrankfurterApi.SDK;

using System.Threading;
using System.Threading.Tasks;
using FrankfurterApi.SDK.Contracts;

public sealed class CurrencyConverterClient : ICurrencyConverterClient
{
    private readonly HttpClient _httpClient;

    public CurrencyConverterClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<Result<CurrencyConverterResponse>> GetLatestExchangeRate(BaseCurrencyForExchangeRate baseCurrencyForExchangeRate, CancellationToken cancellationToken)
    {
        if (baseCurrencyForExchangeRate == null)
        {
            throw new ArgumentNullException(nameof(baseCurrencyForExchangeRate));
        }
        
        return await HttpResponseHelper.GetHttpResponseContentAsync<CurrencyConverterResponse>(
            _httpClient,
            HttpMethod.Get,
            $"latest?from={baseCurrencyForExchangeRate.From}",
            cancellationToken);
    }

    public async Task<Result<CurrencyConverterResponse>> ConvertCurrency(ConvertCurrency convertCurrency, CancellationToken cancellationToken)
    {
        if (convertCurrency == null)
        {
            throw new ArgumentNullException(nameof(convertCurrency));
        }

        return await HttpResponseHelper.GetHttpResponseContentAsync<CurrencyConverterResponse>(
            _httpClient,
            HttpMethod.Get,
            $"latest?amount={convertCurrency.Amount}&from={convertCurrency.FromCurrency}&to={convertCurrency.ToCurrency}",
            cancellationToken);
    }

    public async Task<Result<RetrieveHistoricalRatesResponse>> GetHistoricalRates(RetrieveHistoricalRates retrieveHistoricalRates, CancellationToken cancellationToken)
    {
        if (retrieveHistoricalRates == null)
        {
            throw new ArgumentNullException(nameof(retrieveHistoricalRates));
        }

        return await HttpResponseHelper.GetHttpResponseContentAsync<RetrieveHistoricalRatesResponse>(
            _httpClient,
            HttpMethod.Get,
            $"{retrieveHistoricalRates.StartDate.ToString("yyyy-MM-dd")}..{retrieveHistoricalRates.EndDate.ToString("yyyy-MM-dd")}?base={retrieveHistoricalRates.BaseCurrency}",
            cancellationToken);
    }
}