namespace CurrencyConverter.API.ApiContracts;

using System.ComponentModel.DataAnnotations;

public sealed class LatestExchangeRate
{
    [Required(ErrorMessage = "Base currency is required")]
    [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Base currency must consist of 3 capital letters")]
    public required string From { get; set; }
}