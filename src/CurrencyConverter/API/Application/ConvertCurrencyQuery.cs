using CurrencyConverter.API.ApiContracts;
using MediatR;
using BuildingBlocks;

namespace CurrencyConverter.API.Application;
public sealed record ConvertCurrencyQuery(decimal Amount, string FromCurrency, string ToCurrency) :
    IRequest<Result<CurrencyConvertionResponse>>;