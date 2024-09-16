namespace FrankfurterApi.SDK.Contracts;

public sealed record CurrencyConverterResponse
{
    public decimal Amount { get; set; }
    public required string Base { get; set; }
    public DateOnly Date { get; set; }
    public required Dictionary<string, decimal> Rates { get; set; }
}