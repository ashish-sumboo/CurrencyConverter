namespace CurrencyConverter.API.Application;

using System.Threading;
using CurrencyConverter.API.ApiContracts;
using MediatR;
using BuildingBlocks;
using System.Threading.Tasks;

public sealed class GetHistoricalRatesHandler : IRequestHandler<GetHistoricalRatesQuery, Result<HistoricalRatesResponse>>
{
    private readonly IGetExchangeRatesService _getExchangeRateService;

    public GetHistoricalRatesHandler(IGetExchangeRatesService getExchangeRateService)
    {
        _getExchangeRateService = getExchangeRateService;
    }

    public async Task<Result<HistoricalRatesResponse>> Handle(GetHistoricalRatesQuery request, CancellationToken cancellationToken)
    {
        var historicalRates = await _getExchangeRateService.GetHistoricalRates(request.BaseCurrency, request.StartDate, request.EndDate, cancellationToken);

        if (!historicalRates.IsSuccess || historicalRates == null)
        {
            return Result.Fail<HistoricalRatesResponse>(Error.NotFound(ErrorCodes.CurrencyRateNotFound));
        }

        var rates = new Dictionary<DateOnly, Dictionary<string, decimal>>();

        // Loop in the response and apply pagination
        foreach(var key in historicalRates.Value.Rates)
        {
            rates.TryAdd(key.Key, key.Value.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToDictionary());
        }

        return Result.Ok(new HistoricalRatesResponse
        {
            Amount = historicalRates.Value.Amount,
            Base = historicalRates.Value.Base,
            StartDate = historicalRates.Value.StartDate,
            EndDate = historicalRates.Value.EndDate,
            Rates = rates
        });
    }
}