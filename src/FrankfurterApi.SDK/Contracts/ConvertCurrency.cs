namespace FrankfurterApi.SDK.Contracts;

using System.ComponentModel.DataAnnotations;

public sealed class ConvertCurrency
{
    [Required(ErrorMessage = "Amount is required")]
    public decimal Amount { get; set; }
    
    [Required(ErrorMessage = "From currency is required")]
    [RegularExpression("^[A-Z]{3}$", ErrorMessage = "From currency must consist of 3 capital letters")]
    public required string FromCurrency { get; set; }

    [Required(ErrorMessage = "To currency is required")]
    [RegularExpression("^[A-Z]{3}$", ErrorMessage = "To currency must consist of 3 capital letters")]
    public required string ToCurrency { get; set; }
}