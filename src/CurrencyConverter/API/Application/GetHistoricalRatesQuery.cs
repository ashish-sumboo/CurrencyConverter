using CurrencyConverter.API.ApiContracts;
using MediatR;
using BuildingBlocks;

namespace CurrencyConverter.API.Application;
public sealed record GetHistoricalRatesQuery(string BaseCurrency, DateOnly StartDate, DateOnly EndDate, int Page = 1, int PageSize = 5) :
    IRequest<Result<HistoricalRatesResponse>>;