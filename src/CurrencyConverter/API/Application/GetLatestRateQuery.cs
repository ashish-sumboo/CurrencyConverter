using CurrencyConverter.API.ApiContracts;
using MediatR;
using BuildingBlocks;

namespace CurrencyConverter.API.Application;
public sealed record GetLatestRateQuery(string BaseCurrency) :
    IRequest<Result<CurrencyConvertionResponse>>;