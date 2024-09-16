namespace BuildingBlocks;

public class Result
{
    private readonly Error? _error;

    internal Result()
    {
    }

    internal Result(Error error)
        => _error = error ?? throw new ArgumentNullException(nameof(error));
    
    public bool IsSuccess => _error is null;
    
    public Error Error => _error ?? throw new InvalidOperationException("Error cannot be retrieved in case of success");
    
    public static Result<TValue> Ok<TValue>(TValue value)
        => new(value);
    
    public static Result<TValue> Fail<TValue>(Error error)
        => new(error);
    
    public static Result Fail(Error error) => new Result(error);
}

public sealed class Result<TValue> : Result
{
    private readonly TValue _value;

    internal Result(TValue value)
        => _value = value;

    internal Result(Error error)
        : base(error)
        => _value = default!;
    
    public TValue Value
        => !IsSuccess
            ? throw new InvalidOperationException("Value cannot be retrieved in case of failure")
            : _value;
}