namespace FrankfurterApi.SDK.Contracts;

using System.ComponentModel.DataAnnotations;

public sealed record RetrieveHistoricalRates
{
    [Required(ErrorMessage = "Base currency is required")]
    [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Base currency must consist of 3 capital letters")]
    public required string BaseCurrency { get; set; }

    [Required(ErrorMessage = "StartDate is required")]
    public DateOnly StartDate { get; set; }

    [Required(ErrorMessage = "EndDate is required")]
    public DateOnly EndDate { get; set; }
}