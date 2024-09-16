using BuildingBlocks;
using CurrencyConverter.API.ApiContracts;
using CurrencyConverter.API.Application;
using FrankfurterApi.SDK;
using FrankfurterApi.SDK.Contracts;

namespace CurrencyConverter.Infrastructure.Services;

public sealed class GetExchangeRatesService : IGetExchangeRatesService
{
    private readonly ICurrencyConverterClient _currencyConverterClient;
    private readonly ILogger<GetExchangeRatesService> _logger;

    public GetExchangeRatesService(ICurrencyConverterClient CurrencyConverterClient, ILogger<GetExchangeRatesService> logger)
    {
        _currencyConverterClient = CurrencyConverterClient;
        _logger = logger;
    }

    public async Task<BuildingBlocks.Result<CurrencyConvertionResponse>> GetLatestExchangeRates(string baseCurrency, CancellationToken cancellationToken)
    {
        try
        {
            var baseCurrencyForExchangeRate = new BaseCurrencyForExchangeRate { From = baseCurrency };
            var response = await _currencyConverterClient.GetLatestExchangeRate(baseCurrencyForExchangeRate, cancellationToken);

            if (!response.IsError && response.Data != null)
            {           
                return Result.Ok(new CurrencyConvertionResponse
                {
                    Amount = response.Data.Amount,
                    Base = response.Data.Base,
                    Date = response.Data.Date,
                    Rates = response.Data.Rates
                });
            }
            
            return Result.Fail<CurrencyConvertionResponse>(response.Errors is null
                                   ? Error.Invalid()
                                   : Error.Invalid(response.Errors.ToArray()));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, "An unexpected error occured while contacting the Frankfurter API");
            return Result.Fail<CurrencyConvertionResponse>(Error.ServiceUnavailable());
        }
    }

    public async Task<BuildingBlocks.Result<CurrencyConvertionResponse>> ConvertCurrency(decimal amount, string fromCurrency, string toCurrency, CancellationToken cancellationToken)
    {
        try
        {
            var convertCurrency = new ConvertCurrency { Amount = amount, FromCurrency = fromCurrency, ToCurrency = toCurrency };
            var response = await _currencyConverterClient.ConvertCurrency(convertCurrency, cancellationToken);

            if (!response.IsError && response.Data != null)
            {           
                return Result.Ok(new CurrencyConvertionResponse
                {
                    Amount = response.Data.Amount,
                    Base = response.Data.Base,
                    Date = response.Data.Date,
                    Rates = response.Data.Rates
                });
            }

            return Result.Fail<CurrencyConvertionResponse>(response.Errors is null
                                   ? Error.Invalid()
                                   : Error.Invalid(response.Errors.ToArray()));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, "An unexpected error occured while contacting the Frankfurter API");
            return Result.Fail<CurrencyConvertionResponse>(Error.ServiceUnavailable());
        }
    }

    public async Task<BuildingBlocks.Result<HistoricalRatesResponse>> GetHistoricalRates(string baseCurrency, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
    {
        try
        {
            var retrieveHistoricalrates = new RetrieveHistoricalRates { BaseCurrency = baseCurrency, StartDate = startDate, EndDate = endDate };
            var response = await _currencyConverterClient.GetHistoricalRates(retrieveHistoricalrates, cancellationToken);

            if (!response.IsError && response.Data != null)
            {           
                return Result.Ok(new HistoricalRatesResponse
                {
                    Amount = response.Data.Amount,
                    Base = response.Data.Base,
                    StartDate = response.Data.StartDate,
                    EndDate = response.Data.EndDate,
                    Rates = response.Data.Rates
                });
            }

            return Result.Fail<HistoricalRatesResponse>(response.Errors is null
                                   ? Error.Invalid()
                                   : Error.Invalid(response.Errors.ToArray()));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, "An unexpected error occured while contacting the Frankfurter API");
            return Result.Fail<HistoricalRatesResponse>(Error.ServiceUnavailable());
        }
    }
}