namespace CurrencyConverter.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using BuildingBlocks;
using MediatR;
using CurrencyConverter.API.ApiContracts;
using CurrencyConverter.API.Application;

using Microsoft.AspNetCore.RateLimiting;
using System.Reflection.Metadata;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/currency")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class CurrencyConverterController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CurrencyConverterController> _logger;

    private const string RateLimitingPolicy = "fixed";

    public CurrencyConverterController(IMediator mediator, ILogger<CurrencyConverterController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("latest")]
    [EnableRateLimiting(RateLimitingPolicy)]
    public async Task<ActionResult<CurrencyConvertionResponse>> GetLatestExchangeRates([FromQuery] LatestExchangeRate latestExchangeRate, CancellationToken cancellationToken)
    {
        try
        {
            var getLatestRatesQuery = new GetLatestRateQuery(latestExchangeRate.From);
            var result = await _mediator.Send(getLatestRatesQuery, cancellationToken);
        
            return !result.IsSuccess
                ? ApiResult.From(result.Error)
                : Ok(new CurrencyConvertionResponse
                {
                    Amount = result.Value.Amount,
                    Base = result.Value.Base,
                    Date = result.Value.Date,
                    Rates = result.Value.Rates
                });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e.StackTrace, "An unexpected error occured");

            return ApiResult.From(Error.ServiceError());
        }
    }

    [HttpGet("convert")]
    [EnableRateLimiting(RateLimitingPolicy)]
    public async Task<ActionResult<CurrencyConvertionResponse>> ConvertAmountCurrency([FromQuery] ConvertAmountCurrency convertAmountCurrency, CancellationToken cancellationToken)
    {
        try
        {
            var convertCurrencyQuery = new ConvertCurrencyQuery(convertAmountCurrency.Amount, convertAmountCurrency.FromCurrency, convertAmountCurrency.ToCurrency);
            var result = await _mediator.Send(convertCurrencyQuery, cancellationToken);

            return !result.IsSuccess
                ? ApiResult.From(result.Error)
                : Ok(new CurrencyConvertionResponse
                {
                    Amount = result.Value.Amount,
                    Base = result.Value.Base,
                    Date = result.Value.Date,
                    Rates = result.Value.Rates
                });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e.StackTrace, "An unexpected error occured");

            return ApiResult.From(Error.ServiceError());
        }
    }

    [HttpGet("historical")]
    [EnableRateLimiting(RateLimitingPolicy)]
    public async Task<ActionResult<HistoricalRatesResponse>> GetHistoricalRates([FromQuery] HistoricalRates historicalRates, CancellationToken cancellationToken)
    {
        try
        {
            var getHistoricalRatesQuery = new GetHistoricalRatesQuery(historicalRates.Base, historicalRates.StartDate, 
                        historicalRates.EndDate, historicalRates.Page, historicalRates.PageSize);
            var result = await _mediator.Send(getHistoricalRatesQuery, cancellationToken);

            return !result.IsSuccess
                ? ApiResult.From(result.Error)
                : Ok(new HistoricalRatesResponse
                {
                    Amount = result.Value.Amount,
                    Base = result.Value.Base,
                    StartDate = result.Value.StartDate,
                    EndDate = result.Value.EndDate,
                    Rates = result.Value.Rates
                });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e.StackTrace, "An unexpected error occured");

            return ApiResult.From(Error.ServiceError());
        }
    }
}