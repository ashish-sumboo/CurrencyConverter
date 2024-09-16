namespace FrankfurterApi.SDK;

public sealed class ClientSettings
{
    /// <summary>
    /// The Base API URL to use to connect to the Currency Converter API.
    /// </summary>
    public string BaseUrl { get; set; } = null!;

    /// <summary>
    /// Indicates the maximum number of retry attempts the SDK will perform.
    /// </summary>
    public int RetryCount { get; set; }

    /// <summary>
    /// Indicates the interval (in milliseconds) to wait between each retry.
    /// </summary>
    public int RetryIntervalMilliseconds { get; set; }

    /// <summary>
    /// Indicates the timeout (in milliseconds) before task cancellation.
    /// </summary>
    public int TimeoutMilliseconds { get; set; }
}