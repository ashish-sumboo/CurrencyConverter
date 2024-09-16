namespace CurrencyConverter.API.Application;

using System.Threading;
using CurrencyConverter.API.ApiContracts;
using MediatR;
using BuildingBlocks;

public sealed class GetLatestCurrencyRateHandler : IRequestHandler<GetLatestRateQuery, Result<CurrencyConvertionResponse>>
{
    private readonly IGetExchangeRatesService _getExchangeRateService;

    public GetLatestCurrencyRateHandler(IGetExchangeRatesService getExchangeRateService)
    {
        _getExchangeRateService = getExchangeRateService;
    }
    
    public async Task<Result<CurrencyConvertionResponse>> Handle(GetLatestRateQuery request, CancellationToken cancellationToken)
    {
        var exchangeRates = await _getExchangeRateService.GetLatestExchangeRates(request.BaseCurrency, cancellationToken);

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