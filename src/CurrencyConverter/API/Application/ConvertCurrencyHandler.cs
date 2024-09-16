namespace CurrencyConverter.API.Application;

using System.Threading;
using CurrencyConverter.API.ApiContracts;
using MediatR;
using BuildingBlocks;
using Microsoft.Extensions.Options;
using CurrencyConverter.API.Configuration;

public sealed class ConvertCurrencyHandler : IRequestHandler<ConvertCurrencyQuery, Result<CurrencyConvertionResponse>>
{
    private readonly IGetExchangeRatesService _getExchangeRateService;
    private readonly IOptions<CurrencyConverterSettings> _currencyConverterSettings;

    public ConvertCurrencyHandler(IGetExchangeRatesService getExchangeRateService, 
                                  IOptions<CurrencyConverterSettings> currencyConverterSettings)
    {
        _getExchangeRateService = getExchangeRateService;
        _currencyConverterSettings = currencyConverterSettings;
    }

    public async Task<Result<CurrencyConvertionResponse>> Handle(ConvertCurrencyQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FromCurrency) || string.IsNullOrWhiteSpace(request.ToCurrency))
        {
            return Result.Fail<CurrencyConvertionResponse>(Error.Invalid(ErrorCodes.CurrencyCannotBeEmpty));
        }
        
        if (request.FromCurrency == request.ToCurrency)
        {
            return Result.Fail<CurrencyConvertionResponse>(Error.Invalid(ErrorCodes.FromCurrencyCannotBeSameAsToCurrency));
        }

        if (_currencyConverterSettings != null && _currencyConverterSettings.Value != null
            && _currencyConverterSettings.Value.ExcludedCurrencies != null)
        {
            if (_currencyConverterSettings.Value.ExcludedCurrencies.Contains(request.FromCurrency)
            || _currencyConverterSettings.Value.ExcludedCurrencies.Contains(request.ToCurrency))
            {
                return Result.Fail<CurrencyConvertionResponse>(Error.Invalid(ErrorCodes.CurrencyNotAllowed));
            }
        }
        
        var exchangeRates = await _getExchangeRateService.ConvertCurrency(request.Amount, request.FromCurrency, request.ToCurrency, cancellationToken);

        if (!exchangeRates.IsSuccess || exchangeRates == null)
        {
            return Result.Fail<CurrencyConvertionResponse>(Error.NotFound(ErrorCodes.CurrencyRateNotFound));
        }

        return Result.Ok(new CurrencyConvertionResponse
        {
            Amount = exchangeRates.Value.Amount,
            Base = exchangeRates.Value.Base,
            Date = exchangeRates.Value.Date,
            Rates = exchangeRates.Value.Rates
        });
    }
}