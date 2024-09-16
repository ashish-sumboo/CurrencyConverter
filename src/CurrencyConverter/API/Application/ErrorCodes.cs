namespace CurrencyConverter.API.Application;

public static class ErrorCodes
{
    public const string CurrencyRateNotFound = "currency_rate_not_found";
    public const string CurrencyNotAllowed = "currency_not_allowed";
    public const string CurrencyCannotBeEmpty = "currency_cannot_be_empty";
    public const string FromCurrencyCannotBeSameAsToCurrency = "from_currency_cannot_be_same_as_to_currency";
}