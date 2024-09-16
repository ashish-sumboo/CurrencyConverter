namespace FrankfurterApi.SDK;

public sealed record Result<TResult>
{
    public bool IsError { get; set; }
    public IEnumerable<string>? Errors { get; set; }
    public int StatusCode { get; set; }
    public TResult? Data { get; set; }
}