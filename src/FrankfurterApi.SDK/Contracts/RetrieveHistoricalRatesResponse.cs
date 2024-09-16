namespace FrankfurterApi.SDK.Contracts;

public sealed record RetrieveHistoricalRatesResponse
{
    public decimal Amount { get; set; }
    public required string Base { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public required Dictionary<DateOnly, Dictionary<string, decimal>> Rates { get; set; }
}