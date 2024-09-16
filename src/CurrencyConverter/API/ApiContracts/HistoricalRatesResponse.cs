namespace CurrencyConverter.API.ApiContracts;

public sealed record HistoricalRatesResponse
{
    public decimal Amount { get; set; }
    public required string Base { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public required Dictionary<DateOnly, Dictionary<string, decimal>> Rates { get; set; }
}