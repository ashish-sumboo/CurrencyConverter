namespace CurrencyConverter.API.ApiContracts;

using System.ComponentModel.DataAnnotations;

public sealed record HistoricalRates
{
    [Required(ErrorMessage = "Base currency is required")]
    public required string Base { get; set; }

    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(-2));

    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public int Page { get; set; }

    public int PageSize { get; set; }
}