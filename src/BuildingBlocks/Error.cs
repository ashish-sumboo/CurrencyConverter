namespace BuildingBlocks;

public sealed class Error
{
    public const string NotFoundErrorType = "item_not_found";
    
    public const string ServiceUnavailableErrorType = "service_unavailable";
    
    public const string ServiceErrorType = "service_error";

    private Error(string errorType, IEnumerable<string>? errorCodes)
    {
        ErrorType = errorType;
        ErrorCodes = errorCodes is null ? Array.Empty<string>() : errorCodes.ToArray();
    }
    
    public string ErrorType { get; }
    
    public string[] ErrorCodes { get; }
    
    public static Error NotFound(params string[] errorCodes)
        => new(NotFoundErrorType, errorCodes.AsEnumerable());
    
    public static Error ServiceError(params string[] errorCodes)
        => new(ServiceErrorType, errorCodes.AsEnumerable());
    public static Error Invalid(params string[] errorCodes) => new("invalid_request", (errorCodes).AsEnumerable());
    public static Error ServiceUnavailable(params string[] errorCodes) => new("service_unavailable", errorCodes != null ? (errorCodes).AsEnumerable() : null);
}