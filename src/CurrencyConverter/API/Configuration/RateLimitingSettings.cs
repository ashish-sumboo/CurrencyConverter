namespace CurrencyConverter.API.Configuration;

public sealed class RateLimitingSettings
{
    public bool EnableRateLimiting { get; set; }
    public required string Policy { get; set; }
    public int PermitLimit { get; set; }
    public int WindowInSeconds { get; set; }
}